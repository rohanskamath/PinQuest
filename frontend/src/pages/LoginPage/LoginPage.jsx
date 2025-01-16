import React, { useState } from 'react';
import { Avatar, Box, Stack } from '@mui/material';
import Grid from '@mui/material/Grid2';
import bgImg from '../../assets/bg-image.jpg';
import animationData from '../../assets/Login.json';
import LockOutlinedIcon from '@mui/icons-material/LockOutlined';
import Lottie from 'lottie-react';
import CustomTypography from '../../components/customFormControls/CustomTypography';
import CustomTextField from '../../components/customFormControls/CustomTextField';
import AttachEmailOutlinedIcon from '@mui/icons-material/AttachEmailOutlined';
import EnhancedEncryptionOutlinedIcon from '@mui/icons-material/EnhancedEncryptionOutlined';
import { useNavigate } from 'react-router-dom';
import CustomStyledBox from '../../components/customComponents/CustomStyledBox';
import CustomButton from '../../components/customFormControls/CustomButton';
import { loginUser } from '../../services/authService';
import CustomSnackbar from '../../components/customComponents/CustomSnackbar';
import Cookies from "js-cookie";

const LoginPage = () => {
  const [isAnimating, setIsAnimating] = useState(false);
  const navigate = useNavigate();
  const [loginData, setLoginData] = useState({
    email: "",
    password: ""
  })
  const [errors, setErrors] = useState({})
  const [snackbar, setSnackbar] = useState({ open: false, msg: '', severity: 'success' });

  const validateForm = () => {
    const newErrors = {};

    if (!loginData.email.trim()) {
      newErrors.email = "* Email is required";
    }
    if (!loginData.password.trim()) {
      newErrors.password = "* Password is required";
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleInputChange = (field, value) => {
    setLoginData({ ...loginData, [field]: value });
    setErrors({ ...errors, [field]: "" });
  };

  const handleLoginBtn = async (e) => {
    if (validateForm()) {
      // GET-API Called on Button Click
      try {
        const res = await loginUser(loginData);
        setSnackbar({ open: true, msg: res.message, severity: 'success' });

        // Push the current state to prevent going back
        window.history.replaceState(null, null, "/"); // Redirect to the homepage
        window.history.pushState(null, null, "/"); // Add a new history state
        window.addEventListener("popstate", (event) => {
          if (Cookies.get("token")) {
            // Redirect back to the homepage if the token exists
            window.history.pushState(null, null, "/");
          }
        });

        setTimeout(() => navigate('/'), 1500);
      } catch (err) {
        setSnackbar({ open: true, msg: err, severity: 'error' });
      }
    }
  }

  const handleCreateAccount = () => {
    setIsAnimating(true);
    setTimeout(() => {
      navigate('/register')
    }, 1000)
  }

  const handleCloseSnackbar = () => {
    setSnackbar({ ...snackbar, open: false });
  };

  return (
    <>
      <Box
        sx={{
          backgroundImage: `url(${bgImg})`,
          backgroundPosition: "center",
          backgroundSize: "cover",
          backgroundRepeat: "no-repeat",
          height: "100vh",
        }}
      >
        {/* Login Container Box with Image */}
        <CustomStyledBox className={isAnimating ? 'flip-open-animation' : ''}>

          <Box sx={{ flexGrow: 1 }}>

            <Grid container>
              {/* Grid containing Lottie Animation Starts*/}
              <Grid size={{ xs: 12, sm: 12, lg: 5 }} sx={{ display: { xs: "none", lg: "flex" }, justifyContent: "center", alignItems: "center" }}>
                <Lottie
                  data-testid="lottie-animation"
                  animationData={animationData}
                  loop
                  autoplay
                  style={{ width: "100%", maxWidth: "400px", height: "auto", marginTop: "10px" }}
                />
              </Grid>


              {/* Grid containing Login page Starts*/}
              <Grid size={{ xs: 12, sm: 12, lg: 7 }} sx={{
                backgroundColor: "#bec2da",
                padding: { xs: "10px", sm: "40px" },
              }}>
                <Stack sx={{ display: "flex", justifyContent: "center", alignItems: "center" }}>

                  <Avatar data-testid="avatar" sx={{ bgcolor: "white", color: "black", marginTop: { xs: "5px", sm: "10px" } }} >
                    <LockOutlinedIcon />
                  </Avatar>
                  <CustomTypography sx={{ fontSize: { xs: "18px", sm: "24px" } }}>SignIn / LogIn</CustomTypography>

                  <Stack width="100%" padding={{ xs: "20px 20px 0 20px", sm: "20px 60px 0 60px" }}>
                    <CustomTextField autoComplete="off"
                      label={"Enter your email-id"}
                      value={loginData.email}
                      onChange={(e) => handleInputChange("email", e.target.value)}
                      error={!!errors.email}
                      helperText={errors.email}
                      sx={{ marginTop: "10px" }}
                      autoFocus
                      icon={<AttachEmailOutlinedIcon />} />

                    <CustomTextField
                      type="password"
                      value={loginData.password}
                      onChange={(e) => handleInputChange("password", e.target.value)}
                      error={!!errors.password}
                      helperText={errors.password}
                      label={"Enter your password"}
                      sx={{ marginTop: "20px" }}
                      icon={<EnhancedEncryptionOutlinedIcon />} />

                    <div
                      style={{
                        display: "flex",
                        justifyContent: "flex-end",
                        width: "100%",
                      }}
                    >
                      <CustomTypography
                        className="flip-label"
                        sx={{
                          fontSize: { xs: "9px", sm: "11px" }, color: "blue"
                        }}
                        onClick={() => {
                          navigate('/forgotpassword');
                        }}
                      >
                        Forgot password?
                      </CustomTypography>
                    </div>

                    <CustomButton onClick={handleLoginBtn} sx={{ mt: "10px" }}>
                      Sign In
                    </CustomButton>
                  </Stack>

                  <CustomTypography sx={{ fontSize: "12px", marginBottom: "10px" }}>Not registered yet? <span className='flip-label' onClick={handleCreateAccount}>Create an account</span></CustomTypography>
                </Stack>
              </Grid>
            </Grid>
          </Box>
        </CustomStyledBox>
      </Box>
      {/* Custom Snackbar to display messages */}
      <CustomSnackbar
        open={snackbar.open}
        onClose={handleCloseSnackbar}
        severity={snackbar.severity}
        msg={snackbar.msg}
      />
    </>
  );
};

export default LoginPage;