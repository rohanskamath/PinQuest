import React, { useEffect, useState, useRef } from 'react';
import * as maptilersdk from '@maptiler/sdk';
import "@maptiler/sdk/dist/maptiler-sdk.css";
import CustomModal from './CustomModal'
import CustomInputModal from './CustomInputModal';
import { getAllPins } from '../../services/pinService'

const CustomMapBox = ({ location }) => {
    const mapContainer = useRef(null);
    const map = useRef(null);
    const marker = useRef(null);
    const zoom = 16;
    const [anchorPosition, setAnchorPosition] = useState(null);
    const markers = useRef([]);
    const [pins, setPins] = useState([]);
    const [selectedPin, setSelectedPin] = useState(null);
    const [newPlace, setNewPlace] = useState({

    });
    const [showReviewModal, setShowReviewModal] = useState(false);
    const [showInputModal, setShowInputModal] = useState(false);
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
        const fetchPins = async () => {
            try {
                const response = await getAllPins();
                console.log("Fetched Pins:", response);
                setPins(response);
            } catch (err) {
                console.error("Unable to fetch pins:", err);
            }
        };

        fetchPins();
    }, []);

    useEffect(() => {
        // Remove existing markers before adding new ones
        markers.current.forEach(marker => marker.remove());
        markers.current = [];
        if (pins && pins.length > 0) {
            pins.forEach((pin) => {
                const latitude = parseFloat(pin.lat);
                const longitude = parseFloat(pin.long);

                const newMarker = new maptilersdk.Marker()
                    .setLngLat([longitude, latitude])
                    .addTo(map.current);

                // Add event listeners
                const markerElement = newMarker.getElement();
                markerElement.addEventListener('click', (e) => handleMarkerClick(e, pin));
                markerElement.addEventListener('dblclick', (e) => handleMarkerDoubleClick(e, pin));

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
            if (currentTime === lastClickTime.current) { // Ensure it's still a single-click
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
        lastClickTime.current = currentTime; // Update last click time to prevent single-click logic

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
                    anchorPosition={{ top: anchorPosition.top, left: anchorPosition.left }}
                />
            )}
        </>
    );
};

export default CustomMapBox;