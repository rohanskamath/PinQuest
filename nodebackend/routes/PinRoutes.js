const router = require("express").Router();
const pinController = require("../controllers/pinController");

/* Public Routes */
router.post("/", pinController.createPin);
router.get("/", pinController.getAllPins);

module.exports = router;