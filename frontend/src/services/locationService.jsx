import axios from 'axios'


/* Get Current user placename */
export const fetchLocationName = async (lat, lng) => {
    try {
        const response = await axios.get(`https://api.maptiler.com/geocoding/${lng},${lat}.json?key=${process.env.REACT_APP_MAPTILER_API_KEY}`);
        return response.data.features
    } catch (err) {
        throw err.response?.data?.error || "Something went wrong!";
    }
}

/* Get Search result while user search location */
export const searchLocation = async (query) => {
    try {
        const response = await axios.get(`https://api.maptiler.com/geocoding/${query}.json?key=${process.env.REACT_APP_MAPTILER_API_KEY}`);
        return response.data
    } catch (err) {
        throw err.response?.data?.error || "Something went wrong!";
    }
}