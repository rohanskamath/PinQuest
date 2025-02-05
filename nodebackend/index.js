const express = require("express");
const cookieParser = require("cookie-parser");
const cors = require('cors');
const sequelize = require("./config/connection");
const pinRoute = require("./routes/PinRoutes");
const userRoute = require("./routes/UserRoutes");
const port = 8800;

const app = express();
app.use(express.json());
app.use(cookieParser());
app.use(
  cors({
    origin: process.env.FRONTEND_URL, 
    credentials: true,
  })
);

/* Database connection Starts */
async function DatabaseConnection() {
  try {
    await sequelize.sync({ alter: true });
    console.log("Databases synchronized and Connected successully!");
  } catch (err) {
    console.error("Error synchronizing the database:", err);
  }
}
DatabaseConnection();
/* Database connection Ends */

app.use("/api/pins", pinRoute);
app.use("/api/user", userRoute);

/* Server connecting to port */
app.listen(port, () => {
  console.log("Server is running...");
});
