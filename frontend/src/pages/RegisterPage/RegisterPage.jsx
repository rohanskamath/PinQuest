import React, { useState } from 'react';
import { Avatar, Box, Stack } from '@mui/material';
import Grid from '@mui/material/Grid2';
import bgImg from '../../assets/bg-image.jpg';
import animationData from '../../assets/Register.json';
import LockOpenIcon from '@mui/icons-material/LockOpen';
import Lottie from 'lottie-react';
import CustomTypography from '../../components/customFormControls/CustomTypography';
import CustomTextField from '../../components/customFormControls/CustomTextField';
import PermIdentityIcon from '@mui/icons-material/PermIdentity';
import AttachEmailOutlinedIcon from '@mui/icons-material/AttachEmailOutlined';
import EnhancedEncryptionOutlinedIcon from '@mui/icons-material/EnhancedEncryptionOutlined';
import { useNavigate } from 'react-router-dom';
import CustomStyledBox from '../../components/customComponents/CustomStyledBox';
import CustomButton from '../../components/customFormControls/CustomButton';
import { registerUser, createUserName } from '../../services/authService';
import CustomSnackbar from '../../components/customComponents/CustomSnackbar';

const RegisterPage = () => {
  const [isAnimating, setIsAnimating] = useState(false);
  const navigate = useNavigate();
  const [registerData, setRegisterData] = useState({
    fullName: "",
    email: '',
    password: '',
    cpassword: ''
  })
  const [errors, setErrors] = useState({})
  const [snackbar, setSnackbar] = useState({ open: false, msg: '', severity: 'success' });

  const validateForm = () => {
    const newErrors = {};

    if (!registerData.fullName.trim()) {
      newErrors.fullName = "* Full name is required";
    }

    if (!registerData.email.trim()) {
      newErrors.email = "* Email is required";
    } else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(registerData.email)) {
      newErrors.email = "Enter a valid email address";
    }

    if (!registerData.password.trim()) {
      newErrors.password = "* Password is required";
    } else if (registerData.password.length < 8) {
      newErrors.password = "Password must be at least 8 characters";
    } else if (!/^[a-zA-Z0-9]+$/.test(registerData.password)) {
      newErrors.password = "Password must be alphanumeric";
    }

    if (registerData.password !== registerData.cpassword) {
      newErrors.cpassword = "Passwords do not match.";
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleInputChange = (field, value) => {
    setRegisterData({ ...registerData, [field]: value });
    setErrors({ ...errors, [field]: "" });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (validateForm()) {
      const data = {
        email: registerData.email,
        fullName: registerData.fullName,
        userName: createUserName(registerData.email),
        password: registerData.password
      }
      console.log(data)
      // POST-API Called on Button Click
      try {
        const res = await registerUser(data)
        setSnackbar({ open: true, msg: res.message, severity: 'success' });
        setTimeout(() => navigate('/login'), 1500);
      } catch (err) {
        setSnackbar({ open: true, msg: err, severity: 'error' });
      }
    }
  }

  const handleLoginPage = () => {
    setIsAnimating(true);
    setTimeout(() => {
      navigate('/login')
    }, 1000)
  }

  const handleCloseSnackbar = () => {
    setSnackbar({ ...snackbar, open: false });
  };

  return (
    <>
      {/* Background Box with Image */}
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
        <CustomStyledBox className={isAnimating ? 'flip-close-animation' : ''}>

          <Box sx={{ flexGrow: 1 }}>

            <Grid container sx={{ display: 'flex', flexDirection: "columns" }}>
              {/* Grid containing Lottie Animation Starts*/}
              <Grid size={{ xs: 12, sm: 12, lg: 5 }} sx={{ display: { xs: "none", lg: "flex" }, justifyContent: "center", alignItems: "center" }}>
                <Lottie
                  animationData={animationData}
                  loop
                  autoplay
                  style={{ width: "100%", maxWidth: "400px", height: "auto", marginTop: "10px" }}
                />
              </Grid>

              {/* Grid containing Register page Starts*/}
              <Grid size={{ xs: 12, sm: 12, lg: 7 }} sx={{
                backgroundColor: "#bec2da",
                padding: { xs: "10px", sm: "40px" },
              }}>
                <Stack sx={{ display: "flex", justifyContent: "center", alignItems: "center" }}>

                  <Avatar sx={{ bgcolor: "white", color: "black", marginTop: { xs: "5px", sm: "10px" } }} >
                    <LockOpenIcon />
                  </Avatar>
                  <CustomTypography sx={{ fontSize: { xs: "18px", sm: "24px" } }}>Create your account</CustomTypography>

                  <Stack sx={{ marginTop: "30px" }} direction={{ xs: "column", sm: "row" }} spacing={3}>
                    <CustomTextField
                      value={registerData.fullName}
                      onChange={(e) => handleInputChange("fullName", e.target.value)}
                      error={!!errors.fullName}
                      helperText={errors.fullName}
                      autoComplete="off"
                      label="Enter your full name"
                      autoFocus
                      icon={<PermIdentityIcon />}
                    />

                    <CustomTextField
                      value={registerData.email}
                      onChange={(e) => handleInputChange("email", e.target.value)}
                      error={!!errors.email}
                      helperText={errors.email}
                      autoComplete="off"
                      label="Enter your email-id"
                      icon={<AttachEmailOutlinedIcon />}
                    />
                  </Stack>

                  <Stack sx={{ marginTop: "30px" }} direction={{ xs: "column", sm: "row" }} spacing={3}>
                    <CustomTextField
                      value={registerData.password}
                      onChange={(e) => handleInputChange("password", e.target.value)}
                      error={!!errors.password}
                      helperText={errors.password}
                      type="password"
                      label="Enter your password"
                      icon={<EnhancedEncryptionOutlinedIcon />}
                    />

                    <CustomTextField
                      value={registerData.cpassword}
                      onChange={(e) => handleInputChange("cpassword", e.target.value)}
                      error={!!errors.cpassword}
                      helperText={errors.cpassword}
                      type="password"
                      label="Confirm your password"
                      icon={<EnhancedEncryptionOutlinedIcon />}
                    />
                  </Stack>

                  <CustomButton
                    onClick={handleSubmit}
                    sx={{
                      mt: "30px",
                      width: {
                        xs: "75%",
                        sm: "80%",
                      }
                    }}
                  >
                    Register
                  </CustomButton>

                  <CustomTypography sx={{ fontSize: "12px", marginBottom: "10px" }}>Already have an account? <span className='flip-label' onClick={handleLoginPage}>SignIn Now</span></CustomTypography>
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
}

export default RegisterPage;