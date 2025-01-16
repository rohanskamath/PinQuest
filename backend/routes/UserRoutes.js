const router = require("express").Router();
const userController = require("../controllers/userController");

/* Public Routes */
router.post("/register", userController.registerUser);
router.post("/login", userController.loginUser);
router.post('/auth',userController.verifyToken)

module.exports = router;