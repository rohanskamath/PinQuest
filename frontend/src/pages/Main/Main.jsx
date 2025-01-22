import React, { useEffect, useState } from 'react';
import CustomNavigationBar from '../../components/customComponents/CustomNavigationBar';
import CustomMapBox from '../../components/customComponents/CustomMapBox';
import { fetchLocationName } from '../../services/locationService';

const Main = () => {
  const [location, setLocation] = useState({
    lat: 0,
    lng: 0,
  })
  const placeName=''

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
      navigator.geolocation.getCurrentPosition(successCallback, errorCallback)
      fetchLocationName(location.lat, location.lng)

    } else {
      console.log("Unable to fetch the location")
    }
  }, [])

  return (
    <>
      <CustomNavigationBar />
      <CustomMapBox location={location} />
    </>
  );
}

export default Main;
