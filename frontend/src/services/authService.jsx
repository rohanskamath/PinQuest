import appClient from './appClient'

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

export const refreshAccessToken = async (data) => {
    try {
        const response = await appClient.post('/refresh-token', data)
        return response.data
    } catch (error) {
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