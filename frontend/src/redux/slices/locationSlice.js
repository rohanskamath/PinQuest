import { createSlice } from "@reduxjs/toolkit";

const initialState = {
  location: { lat: 0, lng: 0 },
  placeName: null,
};

const locationSlice = createSlice({
  name: "location",
  initialState,
  reducers: {
    setLocation: (state, action) => {
      state.location = action.payload;
    },
    setPlaceName: (state, action) => {
      state.placeName = action.payload;
    },
    clearLocationData: (state) => {
      state.location = null;
      state.placeName=null;
    },
  },
});

export const { setLocation, setPlaceName,clearLocationData } = locationSlice.actions;
export default locationSlice.reducer;
