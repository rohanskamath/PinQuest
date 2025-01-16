const { DataTypes } = require("sequelize");
const sequelize = require("../config/connection");

const UserSchema = sequelize.define(
  "Users",
  {
    email: {
      type: DataTypes.STRING,
      allowNull: false,
      primaryKey: true,
      validate: {
        isEmail: true,
      },
    },
    fullName: {
      type: DataTypes.STRING,
      allowNull: false,
    },
    password: {
      type: DataTypes.STRING,
      allowNull: false,
      validate: {
        len: [8],
      },
    },
  },
  { timestamps: true }
);

module.exports = UserSchema;
