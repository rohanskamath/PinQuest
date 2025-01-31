import React, { useEffect, useState, useRef } from 'react';
import * as maptilersdk from '@maptiler/sdk';
import "@maptiler/sdk/dist/maptiler-sdk.css";
import CustomModal from './CustomModal'
import CustomInputModal from './CustomInputModal';
import { getAllPins } from '../../services/pinService'
import CustomSnackbar from './CustomSnackbar';

const CustomMapBox = ({ location }) => {
    const mapContainer = useRef(null);
    const map = useRef(null);
    const marker = useRef(null);
    const zoom = 16;
    const markers = useRef([]);

    const [anchorPosition, setAnchorPosition] = useState(null);
    const [pins, setPins] = useState([]);
    const [selectedPin, setSelectedPin] = useState(null);
    const [showReviewModal, setShowReviewModal] = useState(false);
    const [showInputModal, setShowInputModal] = useState(false);
    const [snackbar, setSnackbar] = useState({ open: false, msg: '', severity: 'success' });
    const lastClickTime = useRef(0);

    maptilersdk.config.apiKey = process.env.REACT_APP_MAPTILER_API_KEY;

    useEffect(() => {
        if (!map.current) {
            // Initialize the map only once
            map.current = new maptilersdk.Map({
                container: mapContainer.current,
                style: maptilersdk.MapStyle.STREETS,
                center: [location.lng, location.lat],
                zoom: zoom,
            });

            map.current.doubleClickZoom.disable();

            // Add a marker at the initial location
            marker.current = new maptilersdk.Marker()
                .setLngLat([location.lng, location.lat])
                .addTo(map.current);

            const markerElement = marker.current.getElement();
            markerElement.addEventListener('click', handleMarkerClick);
            markerElement.addEventListener('dblclick', handleMarkerDoubleClick)

        } else {
            // Update map center and marker position when location changes
            map.current.setCenter([location.lng, location.lat]);

            if (marker.current) {
                marker.current.setLngLat([location.lng, location.lat]);
            }
        }
    }, [location]);

    useEffect(() => {
        callbackPins();
    }, []);

    const callbackPins = async () => {
        try {
            const res = await getAllPins();

            // Grouping reviews for the same location and calculating the average
            const groupedPins = res.data.reduce((review, pin) => {
                const key = `${pin.lat},${pin.long}`;

                if (!review[key]) {
                    review[key] = {
                        ...pin,
                        reviews: [pin.desc],
                        ratingSum: pin.rating,
                        reviewCount: 1,
                        avgrating: pin.rating.toFixed(1),
                    };
                } else {
                    review[key].reviews.push(pin.desc);
                    review[key].ratingSum += pin.rating;
                    review[key].reviewCount += 1;
                    review[key].avgRating = (review[key].ratingSum / review[key].reviewCount).toFixed(1)
                }
                return review;
            }, {});

            console.log("hello",Object.values(groupedPins))

            setPins(Object.values(groupedPins));
            setSnackbar({ open: true, msg: res.message, severity: 'success' });
        } catch (err) {
            setSnackbar({ open: true, msg: 'Unable to fetch pins', severity: 'error' });
            console.error("Unable to fetch pins:", err);
        }
    }

    useEffect(() => {
        // Remove existing markers before adding new ones
        markers.current.forEach(marker => marker.remove());
        markers.current = [];

        if (pins && pins.length > 0) {
            pins.forEach((pin) => {
                const latitude = parseFloat(pin.lat);
                const longitude = parseFloat(pin.long);

                let markerColor;
                switch (pin.category.toLowerCase()) {
                    case "restaurant":
                        markerColor = "purple";
                        break;
                    case "hospital":
                        markerColor = "green";
                        break;
                    case "lodge":
                        markerColor = "red";
                        break;
                    default:
                        markerColor = "orange"
                }

                // Creating a custom marker
                const customMarker = document.createElement("div");
                customMarker.style.width = "20px";
                customMarker.style.height = "20px";
                customMarker.style.backgroundColor = markerColor;
                customMarker.style.borderRadius = "50%";
                customMarker.style.border = "2px solid white";
                customMarker.style.cursor = "pointer";

                const newMarker = new maptilersdk.Marker({ element: customMarker })
                    .setLngLat([longitude, latitude])
                    .addTo(map.current);

                // Add event listeners
                customMarker.addEventListener('click', (e) => handleMarkerClick(e, pin));
                customMarker.addEventListener('dblclick', (e) => handleMarkerDoubleClick(e, pin));

                // Store marker reference
                markers.current.push(newMarker);
            });
        }
    }, [pins]);

    const handleMarkerClick = (e, pin) => {
        e.stopPropagation();

        const currentTime = new Date().getTime();
        const timeSinceLastClick = currentTime - lastClickTime.current;

        // If it's a quick second click (double-click), ignore single-click logic
        if (timeSinceLastClick < 300) {
            return;
        }

        lastClickTime.current = currentTime; // Update last click time

        setTimeout(() => {
            if (currentTime === lastClickTime.current) {
                // Ensure it's still a single-click
                const markerElement = e.target;
                if (markerElement) {
                    const rect = markerElement.getBoundingClientRect();
                    setAnchorPosition({
                        top: rect.top,
                        left: rect.right
                    });
                    setSelectedPin(pin)
                    setShowReviewModal(true);
                }
            }
        }, 250);
    };

    const handleMarkerDoubleClick = (e, pin) => {
        e.stopPropagation();

        const currentTime = new Date().getTime();

        // Update last click time to prevent single-click logic
        lastClickTime.current = currentTime;

        const markerElement = e.target;
        if (markerElement) {
            const rect = markerElement.getBoundingClientRect();
            setAnchorPosition({
                top: rect.top + window.scrollY,
                left: rect.right + window.scrollX,
            });
            setShowInputModal(true);
        }
    };

    const handleCloseSnackbar = () => {
        setSnackbar({ ...snackbar, open: false });
    };

    return (
        <>
            <div className="map-wrap">
                <div ref={mapContainer} className="map" />
            </div>
            {anchorPosition && showReviewModal && (
                <CustomModal
                    pin={selectedPin}
                    open={showReviewModal}
                    setOpen={setShowReviewModal}
                    anchorPosition={{ top: anchorPosition.top, left: anchorPosition.left }}
                />
            )}

            {anchorPosition && showInputModal && (
                <CustomInputModal
                    open={showInputModal}
                    setOpen={setShowInputModal}
                    callbackPins={callbackPins}
                    anchorPosition={{ top: anchorPosition.top, left: anchorPosition.left }}
                />
            )}

            {/* Custom Snackbar to display messages */}
            <CustomSnackbar
                open={snackbar.open}
                onClose={handleCloseSnackbar}
                severity={snackbar.severity}
                msg={snackbar.msg}
            />
        </>
    );
};

export default CustomMapBox;