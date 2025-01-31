import { createSlice } from "@reduxjs/toolkit";
import { jwtDecode } from "jwt-decode";
import Cookies from "js-cookie";

const token = Cookies.get("token");

let userData = null;

if (token) {
  try {
    userData = jwtDecode(token);
  } catch (error) {
    console.error("Invalid token:", error);
  }
}

const initialState = {
  user: userData || null,
};

const userSlice = createSlice({
  name: "user",
  initialState,
  reducers: {},
});

export default userSlice.reducer;
