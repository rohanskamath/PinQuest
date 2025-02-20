import { useEffect, useState } from "react";
import { Navigate } from "react-router-dom";
import Cookies from "js-cookie";
import ClipLoader from "react-spinners/ClipLoader";
import { refreshAccessToken } from '../../services/authService';
import { jwtDecode } from "jwt-decode";

const ProtectedRoute = ({ children }) => {
  const [isAuthenticated, setIsAuthenticated] = useState(null);
  const [loading, setLoading] = useState(true);

  const isTokenExpiringSoon = (cookieToken) => {
    try {
      const decodedToken = jwtDecode(cookieToken);
      const currentTime = Date.now() / 1000;
      return decodedToken.exp - currentTime < 3;
    } catch (error) {
      return true;
    }
  }

  const refreshAccesToken = async (cookieToken) => {
    try {
      const data={
        accessToken:cookieToken
      }
      const res = await refreshAccessToken(data)
      const newToken = res.token;
      Cookies.set("token", newToken, { HttpOnly: false, secure: true, sameSite: "Strict", Path: "/" })
      return true;
    }
    catch (err) {
      console.error("Failed to refresh token:", err);
    }
  }

  const checkToken = async () => {
    const cookietoken = Cookies.get("token");
    if (!cookietoken) {
      setIsAuthenticated(false);
      setLoading(false);
      return;
    }

    if (isTokenExpiringSoon(cookietoken)) {
      const refreshed = await refreshAccesToken(cookietoken);
      if (!refreshed) {
        setIsAuthenticated(false);
        setLoading(false);
        return;
      }
    }
    setIsAuthenticated(true);
    setLoading(false);
  };

  useEffect(() => {
    checkToken();

    const intervalId = setInterval(() => {
      checkToken();
    }, 3000)

    return () => clearInterval(intervalId)
  }, []);

  if (loading) {
    return (
      <>
        <div style={{ display: "flex", justifyContent: "center", alignItems: "center", height: "100vh" }}>
          <ClipLoader color="#4A90E2" loading={loading} size={50} />
        </div>
      </>
    );
  }

  if (!isAuthenticated) {
    return <Navigate to="/login" />;
  }

  return children;
};

export default ProtectedRoute;