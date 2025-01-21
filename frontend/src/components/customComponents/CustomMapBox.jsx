import React, { useEffect, useRef } from 'react'
import * as maptilersdk from '@maptiler/sdk';
import "@maptiler/sdk/dist/maptiler-sdk.css";

const CustomMapBox = () => {
    const mapContainer = useRef(null);
    const map = useRef(null);
    const tokyo = { lng: 139.753, lat: 35.6844 };
    const zoom = 14;
    maptilersdk.config.apiKey = 'VAYatajGEgbMpC217RqV';

    useEffect(() => {
        if (map.current) return;

        map.current = new maptilersdk.Map({
            container: mapContainer.current,
            style: maptilersdk.MapStyle.STREETS,
            center: [tokyo.lng, tokyo.lat],
            zoom: zoom
        });

    }, [tokyo.lng, tokyo.lat, zoom]);

    return (
        <>
            <div className='map-wrap'>
                <div ref={mapContainer} className='map' />
            </div>
        </>
    )
}

export default CustomMapBox
