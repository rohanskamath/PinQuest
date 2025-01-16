import axios from 'axios'

const appClient = axios.create({
    baseURL: process.env.REACT_APP_API_URL,
    timeout: 10000,
    headers: {
        'Content-Type': 'application/json',
        Accept: 'application/json'
    },
    withCredentials: true
});

appClient.interceptors.response.use(
    (response) => response,
    (error) => {
      if (error.response?.status === 401) {
        console.error('Session expired. Redirecting to login.');
        window.location.href = '/';
      }
      return Promise.reject(error);
    }
  );  

export default appClient;