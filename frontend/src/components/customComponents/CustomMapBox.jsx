import React, { useEffect, useState, useRef } from 'react';
import * as maptilersdk from '@maptiler/sdk';
import "@maptiler/sdk/dist/maptiler-sdk.css";
import CustomModal from './CustomModal'

const CustomMapBox = ({ location }) => {
    const mapContainer = useRef(null);
    const map = useRef(null);
    const marker = useRef(null);
    const zoom = 16;
    const [open, setOpen] = useState(false)
    const [anchorPosition, setAnchorPosition] = useState(null);

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

            // Add a marker at the initial location
            marker.current = new maptilersdk.Marker()
                .setLngLat([location.lng, location.lat])
                .addTo(map.current);

            const markerElement = marker.current.getElement();
            markerElement.addEventListener('click', handleOpen);

        } else {
            // Update map center and marker position when location changes
            map.current.setCenter([location.lng, location.lat]);

            if (marker.current) {
                marker.current.setLngLat([location.lng, location.lat]);
            }
        }
    }, [location]);

    const handleOpen = event => {
        if (marker.current) {
            const markerElement = marker.current.getElement();
            if (markerElement) {
                const rect = markerElement.getBoundingClientRect();
                setAnchorPosition({
                    top: rect.top,
                    left: rect.right
                });
                setOpen(true);
            }
        }
    }

    return (
        <>
            <div className="map-wrap">
                <div ref={mapContainer} className="map" />
            </div>
            {anchorPosition && (
                <CustomModal
                    open={open}
                    setOpen={setOpen}
                    anchorPosition={{ top: anchorPosition.top, left: anchorPosition.left }}
                />
            )}
        </>
    );
};

export default CustomMapBox;