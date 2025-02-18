import appClient from './appClient'

/* GET API - Get all pins */
export const getAllPins = async () => {
    try {
        const response = await appClient.get('/pins');
        return response.data;

    } catch (error) {
        throw error.response?.data?.error || "Something went wrong!";
    }
}

/* POST API - Add new pins */
export const addNewPin = async (data) => {
    try {
        const response = await appClient.post('/pins', data)
        return response.data
    } catch (error) {
        console.log("err", error)
        throw error.response?.data?.error || "Something went wrong!";
    }
}

/* GET API - Get pins by userId */
export const getPinsbyUserId = async (userId) => {
    try {
        const response = await appClient.get(`/pins/${userId}`)
        return response.data
    } catch (error) {
        throw error.response?.data?.error || "Something went wrong!";
    }
}