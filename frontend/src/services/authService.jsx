import appClient from './appClient'

/* POST API - Registration */
export const registerUser = async (data) => {
    try {
        const response = await appClient.post('/user/register', data)
        return response.data
    } catch (error) {
        throw error.response?.data?.error || "Something went wrong!";
    }
}

/* POST API - Login */
export const loginUser = async (data) => {
    try {
        const response = await appClient.post('/user/login', data)
        return response.data
    } catch (error) {
        throw error.response?.data?.error || "Something went wrong!";
    }
}

/* POST API - Token Verification */
export const verifyToken = async (token) => {
    try {
        const response = await appClient.post('/user/auth', {}, { headers: { Authorization: `Bearer ${token}` } })
        return response.data
    } catch (error) {
        throw error.response?.data?.error || "Something went wrong!";
    }
}