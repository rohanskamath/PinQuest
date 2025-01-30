import React, { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux'
import CustomNavigationBar from '../../components/customComponents/CustomNavigationBar';
import CustomMapBox from '../../components/customComponents/CustomMapBox';
import { fetchLocationName } from '../../services/locationService';
import { setLocation, setPlaceName } from '../../redux/slices/locationSlice';

const Main = () => {
  const dispatch = useDispatch();
  
  const [curretPlace, setCurrentPlace] = useState(null)
  const location = useSelector((state) => state.location.location)
  const placeName = useSelector((state) => state.location.placeName)

  const successCallback = (position) => {
    const newLocation = {
      lat: position.coords.latitude,
      lng: position.coords.longitude
    }
    dispatch(setLocation(newLocation))
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
            dispatch(setPlaceName(features[0].place_name));
          } else {
            console.log("No place found for the given coordinates.");
          }
        } catch (error) {
          console.error("Error fetching location name:", error);
        }
      }
    }

    fetchPlaceName()
  }, [location, dispatch])

  return (
    <>
      <CustomNavigationBar placeName={placeName} />
      <CustomMapBox location={location} />
    </>
  );
}

export default Main;