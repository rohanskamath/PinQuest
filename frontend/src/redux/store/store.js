import { configureStore } from "@reduxjs/toolkit";
import locationReducers from "../slices/locationSlice";

export const store = configureStore({
  reducer: {
    location: locationReducers,
  },
});
