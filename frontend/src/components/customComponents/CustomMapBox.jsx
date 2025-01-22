import React, { useEffect, useRef } from 'react';
import * as maptilersdk from '@maptiler/sdk';
import "@maptiler/sdk/dist/maptiler-sdk.css";

const CustomMapBox = ({ location }) => {
    const mapContainer = useRef(null);
    const map = useRef(null);
    const marker = useRef(null); // For adding a marker
    const zoom = 16;
    maptilersdk.config.apiKey = 'VAYatajGEgbMpC217RqV';

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
            marker.current = new maptilersdk.Marker().setLngLat([location.lng, location.lat]).addTo(map.current);
        } else {
            // Update map center and marker position when location changes
            map.current.setCenter([location.lng, location.lat]);
            if (marker.current) {
                marker.current.setLngLat([location.lng, location.lat]);
            }
        }
    }, [location]);

    return (
        <div className='map-wrap'>
            <div ref={mapContainer} className='map' />
        </div>
    );
};

export default CustomMapBox;
