import { Avatar, Box, Stack } from '@mui/material';
import React, { useEffect, useRef, useState } from 'react';
import bgImg from '../../assets/bg-image.jpg';
import CustomStyledBox from '../../components/customComponents/CustomStyledBox';
import CustomTextField from '../../components/customFormControls/CustomTextField';
import Grid from '@mui/material/Grid2';
import Lottie from 'lottie-react';
import PsychologyAltIcon from '@mui/icons-material/PsychologyAlt';
import AttachEmailOutlinedIcon from '@mui/icons-material/AttachEmailOutlined';
import ArrowBackIosIcon from '@mui/icons-material/ArrowBackIos';
import EnhancedEncryptionOutlinedIcon from '@mui/icons-material/EnhancedEncryptionOutlined';
import animationData from '../../assets/ForgotPassword.json'
import CustomTypography from '../../components/customFormControls/CustomTypography';
import CustomButton from '../../components/customFormControls/CustomButton';
import { useNavigate } from 'react-router-dom';
import { changePassword, sendOTP, verifyOTP } from '../../services/authService';
import CustomSnackbar from '../../components/customComponents/CustomSnackbar';

const ForgotPassword = () => {
  const navigate = useNavigate();
  const [email, setEmail] = useState('');
  const [newpassword, setNewPassword] = useState({
    password: '',
    confirmPassword: ''
  });
  const [errors, setErrors] = useState({})
  const [otpSent, setOtpSent] = useState(false);
  const [isOtpValid, setIsOtpValid] = useState(false);
  const [otpError, setOtpError] = useState(false);
  const [otp, setOtp] = useState(['', '', '', '']);
  const [timer, setTimer] = useState(60);
  const otpRefs = useRef([]);
  const [snackbar, setSnackbar] = useState({ open: false, msg: '', severity: 'success' });

  const validateForm = () => {
    const newErrors = {};
    if (!email.trim()) {
      newErrors.email = "* Email is required";
    }
    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  }

  const handleVerifyBtn = async () => {
    if (validateForm()) {
      try {
        const data = {
          email: email
        }
        const res = await sendOTP(data);
        if (res.success) {
          setSnackbar({ open: true, msg: res.message, severity: 'success' });
          setOtpSent(true)
        } else {
          setOtpSent(false)
          setSnackbar({ open: true, msg: res.message, severity: 'error' });
        }
      }
      catch (error) {
        setSnackbar({ open: true, msg: error, severity: 'error' });
        setOtpSent(false)
      }
    }
  }

  const handleOtpChange = (index, value) => {
    if (value && !/^\d{1}$/.test(value)) return

    const updatedOtp = [...otp];
    setOtpError(false)
    updatedOtp[index] = value;
    setOtp(updatedOtp)

    if(value && index <3){
      otpRefs.current[index+1]?.focus();
    }
  }

  const handleOtpVerify = async () => {
    const enteredOtp = otp.join('');
    try {
      const data = {
        email: email,
        otp: enteredOtp
      };
      const res = await verifyOTP(data)
      if (res.success) {
        setSnackbar({ open: true, msg: res.message, severity: 'success' });
        setTimer(60);
        setOtp(["", "", "", ""]);
        setIsOtpValid(true);
        setOtpError(false);
      } else {
        setOtpError(true)
      }
    } catch (error) {
      setSnackbar({ open: true, msg: error, severity: 'success' });
      setOtpError(true)
    }
  }

  useEffect(() => {
    let countdown;
    if (otpSent && timer > 0) {
      countdown = setInterval(() => {
        setTimer((prevTimer) => prevTimer - 1)
      }, 1000);
    } else if (timer === 0) {
      clearInterval(countdown)
    }

    return () => clearInterval(countdown)
  }, [otpSent, timer])

  const formatTimer = (time) => {
    const minutes = Math.floor(timer / 60);
    const seconds = time % 60;
    return `${minutes}:${seconds < 10 ? '0' : ''}${seconds}`;
  }

  const handleResendOTP = async () => {
    try {
      const data = {
        email: email
      }
      const res = await sendOTP(data);
      if (res.success) {
        setSnackbar({ open: true, msg: res.message, severity: 'success' });
        setTimer(60)
        setOtp(["", "", "", ""]);
        setOtpSent(true);
        setIsOtpValid(false);
        setOtpError(false);
      }
    }
    catch (error) {
      setSnackbar({ open: true, msg: error, severity: 'error' });
    }
  };

  const handleChangePassword = async () => {
    try {
      const data = {
        email: email,
        newPassword: newpassword.password
      }
      const res=await changePassword(data)
      if(res.success)
      {
        setSnackbar({ open: true, msg: res.message, severity: 'success' });
        setTimeout(() => navigate('/login'), 1500);
      } else {
        setSnackbar({ open: true, msg: res.message, severity: 'error' });
      }
    }
    catch (error) {
      setSnackbar({ open: true, msg: error, severity: 'error' });
    }
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

        <CustomStyledBox>
          <Box sx={{ flexGrow: 1 }}>
            <Grid container>
              {/* Grid containing Lottie Animation Starts*/}
              <Grid size={{ xs: 12, sm: 12, lg: 5 }} sx={{ display: { xs: "none", lg: "flex" }, justifyContent: "center", alignItems: "center" }}>
                <Lottie
                  animationData={animationData}
                  loop
                  autoplay
                  style={{ width: "100%", maxWidth: "300px", height: "auto", marginTop: "10px" }}
                />
              </Grid>

              <Grid size={{ xs: 12, sm: 12, lg: 7 }} sx={{
                backgroundColor: "#bec2da",
                padding: { xs: "10px", sm: "40px" },
                position: "relative"
              }}>
                <ArrowBackIosIcon
                  onClick={() => navigate('/login')}
                  sx={{
                    position: "absolute",
                    top: { xs: "10px", sm: "20px" },
                    left: { xs: "10px", sm: "20px" },
                    fontSize: { xs: "12px", sm: "15px" },
                    color: "black",
                    cursor: "pointer",
                  }}
                />
                <Stack sx={{ display: "flex", justifyContent: "center", alignItems: "center" }}>
                  <Avatar sx={{ bgcolor: "white", color: "black", marginTop: { xs: "5px", sm: "10px" } }} >
                    <PsychologyAltIcon />
                  </Avatar>
                  <CustomTypography sx={{ fontSize: { xs: "18px", sm: "24px" } }}>Forgot your password?</CustomTypography>

                  <Stack width="100%" padding={{ xs: "20px 20px 0 20px", sm: "20px 60px 0 60px" }}>
                    <CustomTextField
                      type="text"
                      value={email}
                      disabled={otpSent}
                      onChange={(e) => setEmail(e.target.value)}
                      error={!!errors.email}
                      helperText={errors.email}
                      autoComplete="off"
                      label={"Enter your email-id"}
                      sx={{ marginTop: "10px" }}
                      autoFocus
                      icon={<AttachEmailOutlinedIcon />} />
                    {
                      !otpSent && (
                        <CustomButton onClick={handleVerifyBtn} sx={{ mt: "20px" }}>Send Code</CustomButton>
                      )
                    }
                  </Stack>

                  {otpSent && !isOtpValid && (
                    <Stack width="100%" padding={{ xs: "10px 0 20px 20px", sm: "10px 0 20px 60px" }}>
                      <CustomTypography sx={{ fontSize: { xs: "7px", sm: "10px" } }}>
                        Enter the OTP sent to your email
                      </CustomTypography>
                      <div style={{ display: "flex" }}>
                        <Stack direction="row" spacing={1}>
                          {otp.map((value, index) => (
                            <CustomTextField
                              autoComplete="off"
                              inputRef={(el) => (otpRefs.current[index] = el)}
                              error={otpError}
                              key={index}
                              value={value}
                              onChange={(e) => handleOtpChange(index, e.target.value)}
                              sx={{ width: "50px", textAlign: "center" }}
                              inputProps={{ maxLength: 1, style: { textAlign: "center" } }}
                            />
                          ))}
                        </Stack>
                        <CustomTypography
                          className="flip-label" onClick={timer === 0 ? handleResendOTP : handleOtpVerify}
                          sx={{ color: "blue", fontSize: { xs: "9px", sm: "12px" }, marginLeft: { xs: "5px", sm: "10px" } }}
                        >
                          {timer === 0 ? "Resend" : "Verify"}
                        </CustomTypography>
                      </div>
                      <CustomTypography sx={{ fontSize: { xs: "7px", sm: "10px" } }}>
                        Time remaining: {formatTimer(timer)}
                      </CustomTypography>
                    </Stack>
                  )}

                  {isOtpValid && (
                    <Stack width="100%" spacing={2} padding={{ xs: "20px 20px 20px 20px", sm: "20px 60px 20px 60px" }}>
                      <CustomTextField
                        icon={<EnhancedEncryptionOutlinedIcon />}
                        label={'New Password'}
                        autoFocus
                        type="password"
                        value={newpassword.password}
                        onChange={(e) => { setNewPassword({ ...newpassword, password: e.target.value }) }}
                      />
                      <CustomTextField
                        icon={<EnhancedEncryptionOutlinedIcon />}
                        label={'Confirm Password'}
                        type="password"
                        value={newpassword.confirmPassword}
                        onChange={(e) => { setNewPassword({ ...newpassword, confirmPassword: e.target.value }) }}
                      />
                      <CustomButton onClick={handleChangePassword}>
                        Reset Password
                      </CustomButton>
                    </Stack>
                  )}
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

export default ForgotPassword;