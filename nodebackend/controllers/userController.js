const userDao = require("../dao/userDao");

// Controller to register new user
const registerUser = async (req, res) => {
  try {
    const newUser = await userDao.signUp(req.body);
    res.status(200).json({
      sucess: true,
      message: "Registered Sucessfully!",
    });
  } catch (err) {
    res.status(500).json({ error: err.message });
  }
};

// Controller to Login user
const loginUser = async (req, res) => {
  try {
    const user = await userDao.logIn(req.body);
    /* Generate a token for user and send it to the cookie */
    const options = {
      expires: new Date(Date.now() + 1 * 24 * 60 * 60 * 1000),
      httpOnly: false,
      sameSite: "Strict",
      secure:false,
      path:"/"
    };
    res.status(200).cookie("token", user, options).json({
      success: true,
      message: "Logged in successfully!",
    });
  } catch (err) {
    res.status(500).json({ error: err.message });
  }
};

// Controller to verify token
const verifyToken = async (req, res) => {
  try {
    const token = req.headers.authorization?.split(" ")[1];
    if (!token) {
      return res
        .status(401)
        .json({ success: false, message: "No token provided" });
    }
    result = await userDao.tokenVerification(token);
    if (!result) {
      return res
        .status(401)
        .json({ success: false, message: "Invalid / Expired Token" });
    }
    return res.status(200).json({ success: true, message: "Token is valid" });
  } catch (err) {
    res.status(500).json({ error: err.message });
  }
};

module.exports = { registerUser, loginUser, verifyToken };
