const { DataTypes } = require("sequelize");
const sequelize = require("../config/connection");
const User = require("./User");

const PinSchema = sequelize.define(
  "Pin",
  {
    id:{
      type:DataTypes.INTEGER,
      autoIncrement:true,
      primaryKey:true
    },
    email: {
      type: DataTypes.STRING,
      allowNull: false,
      references: {
        model: User,
        key: "email",
      },
    },
    title: {
      type: DataTypes.STRING,
      allowNull: false,
      validate: {
        len: [3],
      },
    },
    desc: {
      type: DataTypes.STRING,
      allowNull: false,
      validate: {
        len: [3],
      },
    },
    rating: {
      type: DataTypes.INTEGER,
      allowNull: false,
      validate: {
        min: 0,
        max: 5,
      },
    },
    lat: {
      type: DataTypes.INTEGER,
      allowNull: false,
    },
    long: {
      type: DataTypes.INTEGER,
      allowNull: false,
    },
  },
  { timestamps: true }
);

/* Defiing Associations */
User.hasMany(PinSchema, { foreignKey: "email", onDelete: "CASCADE" });
PinSchema.belongsTo(User, { foreignKey: "email" });

module.exports = PinSchema;
