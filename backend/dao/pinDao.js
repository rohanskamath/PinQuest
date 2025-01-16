const Pin = require("../models/Pin");

const createPinDao = async (data) => {
  try {
    const newPin = await Pin.create(data);
    return newPin;
  } catch (err) {
    throw new Error(`Error while adding pin: ${err.message}`);
  }
};

const getAllPinsDao = async () => {
  try {
    const fetchAllPins = await Pin.findAll();
    return fetchAllPins;
  } catch (err) {
    throw new Error(`Error while fetching pin: ${err.message}`);
  }
};

module.exports = {
  createPinDao,
  getAllPinsDao,
};
