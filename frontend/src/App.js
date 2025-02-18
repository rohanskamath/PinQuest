import React, { useEffect } from "react";
import { BrowserRouter, Route, Routes } from "react-router-dom";
import Cookies from "js-cookie";
import { useDispatch } from "react-redux";
import { jwtDecode } from "jwt-decode";
import LoginPage from "./pages/LoginPage/LoginPage";
import RegisterPage from "./pages/RegisterPage/RegisterPage";
import ForgotPassword from "./pages/ForgotPassword/ForgotPassword";
import Main from "./pages/Main/Main";
import { CssBaseline } from "@mui/material";
import ProtectedRoute from "./components/customComponents/ProtectedRoute";
import Layout from "./pages/Layout";
import VisitsPage from "./pages/VisitsPage/VisitsPage";
import { setUserData } from "./redux/slices/userSlice";

const App = () => {
  const dispatch = useDispatch();

  useEffect(() => {
    const token = Cookies.get("token");
    if (token) {
      const tokenResponse = jwtDecode(token);
      const userData = JSON.parse(tokenResponse.UserData);
      dispatch(setUserData(userData));
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
