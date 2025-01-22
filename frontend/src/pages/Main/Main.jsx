import React, { useEffect, useState } from 'react';
import CustomNavigationBar from '../../components/customComponents/CustomNavigationBar';
import CustomMapBox from '../../components/customComponents/CustomMapBox';
import { fetchLocationName } from '../../services/locationService';

const Main = () => {
  const [location, setLocation] = useState({
    lat: 0,
    lng: 0,
  })
  const [placeName, setPlaceName] = useState(null);

  const successCallback = (position) => {
    setLocation({
      lat: position.coords.latitude,
      lng: position.coords.longitude
    })
  }

  const errorCallback = () => {
    console.log("Unable to fetch the location")
  }

  useEffect(() => {
    if (navigator.geolocation) {
      navigator.geolocation.getCurrentPosition(successCallback, errorCallback);
    } else {
      console.log("Geolocation is not supported by this browser.");
    }
  }, []);

  useEffect(() => {
    const fetchPlaceName = async () => {
      if (location.lat !== 0 && location.lng !== 0) {
        try {
          const features = await fetchLocationName(location.lat, location.lng);
          if (features && features.length > 0) {
            setPlaceName(features[0].place_name);
          } else {
            console.log("No place found for the given coordinates.");
          }
        } catch (error) {
          console.error("Error fetching location name:", error);
        }
      }
    }

    fetchPlaceName()
  }, [location])

  return (
    <>
      <CustomNavigationBar placeName={placeName} />
      <CustomMapBox location={location} />
    </>
  );
}

export default Main;