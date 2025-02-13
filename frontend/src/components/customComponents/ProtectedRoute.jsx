import { useEffect, useState } from "react";
import { Navigate } from "react-router-dom";
import Cookies from "js-cookie";
import { verifyToken } from "../../services/authService";
import ClipLoader from "react-spinners/ClipLoader";

const ProtectedRoute = ({ children }) => {
  const [isAuthenticated, setIsAuthenticated] = useState(null);
  const [loading, setLoading] = useState(true);

  const checkToken = async () => {
    const token = Cookies.get("token");
    try {
      const res = await verifyToken(token);
      if (res.success) {
        setIsAuthenticated(true);
      } else {
        setIsAuthenticated(false);
      }
    } catch (err) {
      console.error("Error verifying token:", err);
      setIsAuthenticated(false);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    checkToken();

    const intervalId = setInterval(() => {
      checkToken();
    }, 5000)

    return () => clearInterval(intervalId)
  }, []);

  if (loading) {
    return (
      <>
        <div style={{ display: "flex", justifyContent: "center", alignItems: "center", height: "100vh" }}>
          <ClipLoader color="#4A90E2" loading={loading} size={100} />
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