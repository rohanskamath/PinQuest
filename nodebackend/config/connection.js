const { Sequelize } = require("sequelize");
const dotenv = require("dotenv");

dotenv.config();

/* Initialize Sequelize instance with PostgreSQL connection */
const sequelize = new Sequelize(
  process.env.DB_NAME,
  process.env.DB_USER,
  process.env.DB_PASSWORD,
  {
    host: process.env.DB_HOST,
    dialect: "postgres",
    logging: false,
  }
);

/* Test the database connection */
const connectDB = async () => {
  try {
    await sequelize.authenticate();
    console.log("Database Connected!");
  } catch (err) {
    console.log("Unable to connect to the database: ", err);
  }
};

connectDB();

module.exports = sequelize;