import appClient from './appClient'
import { jwtDecode } from "jwt-decode";
import { store } from "../redux/store/store";
import { setUserData } from '../redux/slices/userSlice';

/* POST API - Registration */
export const registerUser = async (data) => {
    try {
        const response = await appClient.post('/register', data)
        return response.data
    } catch (error) {
        throw error.response?.data?.error || "Something went wrong!";
    }
}

/* POST API - Login */
export const loginUser = async (data) => {
    try {
        const response = await appClient.post('/login', data)
        return response.data
    } catch (error) {
        throw error.response?.data?.error || "Something went wrong!";
    }
}

/* POST API - To Refresh Token */
export const refreshAccessToken = async (data) => {
    try {
        const response = await appClient.post('/refresh-token', data)
        return response.data
    } catch (error) {
        throw error.response?.data?.error || "Something went wrong!";

    }
}

/* POST API - To send OTP */
export const sendOTP = async (data) => {
    try {
        const response = await appClient.post('/send-otp', data)
        return response.data
    }
    catch (error) {
        throw error.response?.data?.error || "Something went wrong!";
    }
}

/* POST API - To verify OTP */
export const verifyOTP = async (data) => {
    try {
        const response = await appClient.post('/verify-otp', data)
        return response.data
    }
    catch (error) {
        throw error.response?.data?.error || "Something went wrong!";
    }
}

/* PUT API - To change password */
export const changePassword = async (data) => {
    try {
        const response = await appClient.put('/change-password', data)
        return response.data
    }
    catch (error) {
        throw error.response?.data?.error || "Something went wrong!";
    }
}

/* Create Username by taking emailID */
export const createUserName = (email) => {
    let uname = "";
    var count = 0;
    for (let i = 0; i < email.length; i++) {
        if (email[i] === '@') {
            count++;

            if (count === 2) {
                uname = email.slice(0, i)
                break;
            }
        }
    }
    if (count === 1) {
        uname = email.slice(0, email.indexOf('@'))
    }
    return uname;
}

/* Extract userData from token */
export const retriveUserData = (token) => {
    const tokenResponse = jwtDecode(token);
    const userData = JSON.parse(tokenResponse.UserData);
    store.dispatch(setUserData(userData));
}