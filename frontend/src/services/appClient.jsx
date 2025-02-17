import axios from 'axios'
import Cookies from 'js-cookie';

const appClient = axios.create({
  baseURL: process.env.REACT_APP_NET_API_URL,
  timeout: 10000,
  headers: {
    'Content-Type': 'application/json',
    Accept: 'application/json'
  },
  withCredentials: true
});

// Request interceptor: Attach token to requests
appClient.interceptors.request.use(
  async (config) => {
    let token = Cookies.get('token')
    if (token) {
      config.headers.Authorization = `Bearer ${token}`
    }
    return config;
  },
  (err)=> Promise.reject(err)
);

export default appClient;