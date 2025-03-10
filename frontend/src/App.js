import React, { useEffect } from "react";
import { BrowserRouter, Route, Routes } from "react-router-dom";
import Cookies from "js-cookie";
import LoginPage from "./pages/LoginPage/LoginPage";
import RegisterPage from "./pages/RegisterPage/RegisterPage";
import ForgotPassword from "./pages/ForgotPassword/ForgotPassword";
import Main from "./pages/Main/Main";
import { CssBaseline } from "@mui/material";
import ProtectedRoute from "./components/customComponents/ProtectedRoute";
import Layout from "./pages/Layout";
import VisitsPage from "./pages/VisitsPage/VisitsPage";
import { retriveUserData } from "./services/authService";

const App = () => {

  useEffect(() => {
    const token = Cookies.get("token");
    if (token) {
      retriveUserData(token)
    }
  }, []);

  return (
    <>
      <CssBaseline />
      <BrowserRouter>
        <Routes>
          <Route
            path="/"
            element={
              <ProtectedRoute>
                <Layout />
              </ProtectedRoute>
            }
          >
            <Route index element={<Main />} />
            <Route path="visits" element={<VisitsPage />} />
          </Route>
          <Route path="/login" element={<LoginPage />} />
          <Route path="/register" element={<RegisterPage />} />
          <Route path="/forgotpassword" element={<ForgotPassword />} />
        </Routes>
      </BrowserRouter>
    </>
  );
};

export default App;
