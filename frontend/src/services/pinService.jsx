import appClient from './appClient'

/* GET API - Get all pins */
export const getAllPins = async () => {
    try {
        const res = await appClient.get('/pins');
        console.log(res.data)
        return res.data;

    } catch (error) {
        throw error.response?.data?.error || "Something went wrong!";
    }
}