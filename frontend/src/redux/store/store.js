import { configureStore } from "@reduxjs/toolkit";
import locationReducers from "../slices/locationSlice";
import userReducers from "../slices/userSlice";

export const store = configureStore({
  reducer: {
    location: locationReducers,
    user: userReducers,
  },
});
