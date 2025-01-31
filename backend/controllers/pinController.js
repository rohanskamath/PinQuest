const pinDao = require("../dao/pinDao");

// Controller to Create New Pin
const createPin = async (req, res) => {
  try {
    const newPin = await pinDao.createPinDao(req.body);
    res.status(200).json({
      sucess: true,
      message: "Pin added sucessfully!",
      data: newPin,
    });
  } catch (err) {
    res.status(500).json({ error: err.message });
  }
};

// Controller to fetch all Pins
const getAllPins = async (req, res) => {
  try {
    const pins = await pinDao.getAllPinsDao();
    res.status(200).json({
      sucess: true,
      message: "Pins Fetched sucessfully!",
      data: pins,
    });
  } catch (err) {
    res.status(500).json({ error: err.message });
  }
};

module.exports = {
  createPin,
  getAllPins,
};
