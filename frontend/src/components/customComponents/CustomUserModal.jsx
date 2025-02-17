import { Box, Modal, IconButton, Stack } from '@mui/material';
import React from 'react';
import { useSelector } from 'react-redux';
import CloseIcon from '@mui/icons-material/Close';
import AccountCircleIcon from '@mui/icons-material/AccountCircle';
import Divider from '@mui/material/Divider';
import CustomTypography from '../customFormControls/CustomTypography';

const style = {
    position: 'absolute',
    top: '50%',
    left: '50%',
    transform: 'translate(-50%, -50%)',
    width: { xs: '90%', sm: '60%', md: '40%' }, // Responsive width
    bgcolor: 'background.paper',
    borderRadius: '12px',
    boxShadow: 24,
    p: 4,
    background: 'linear-gradient(135deg, #f9f9f9, #e0e0e0)', // Subtle gradient
    display: 'flex',
    flexDirection: 'column',
    gap: 2,
};

const CustomUserModal = ({ open, setOpen }) => {
    const handleClose = () => setOpen(false);
    const userData = useSelector((state) => state.user.user);
    const locationData = useSelector((state) => state.location.placeName);

    return (
        <Modal
            open={open}
            onClose={handleClose}
            aria-labelledby="modal-modal-title"
            aria-describedby="modal-modal-description"
        >
            <Box sx={style}>
                <Box sx={{ display: "flex", justifyContent: "center" }}>
                    <AccountCircleIcon sx={{ fontSize: "50px", marginTop:"-8px" }} />
                    <Stack sx={{ textAlign: "center" }}>
                        <CustomTypography sx={{ fontSize: "12px", fontWeight: "bold" }} marginTop='0px'>{userData.fullName}</CustomTypography>
                        <CustomTypography sx={{ fontSize: "12px", paddingLeft:"10px" }} marginTop='0px'> {userData.email}</CustomTypography>
                    </Stack>
                    <IconButton
                        onClick={handleClose}
                        sx={{ position: "absolute", top: "15px", right: "15px", color: '#333' }}
                    >
                        <CloseIcon />
                    </IconButton>
                </Box>
                <Divider />
                <Box sx={{ display: "flex", justifyContent: "space-between" }}>
                    <CustomTypography sx={{ fontSize: "12px", fontWeight: "bold" }} marginTop='0px'>👤 Name:</CustomTypography>
                    <CustomTypography sx={{ fontSize: "12px" }} marginTop='0px'>{userData.FullName}</CustomTypography>
                </Box>
                <Box sx={{ display: "flex", justifyContent: "space-between" }}>
                    <CustomTypography sx={{ fontSize: "12px", fontWeight: "bold" }} marginTop='0px'>✉️ Email address:</CustomTypography>
                    <CustomTypography sx={{ fontSize: "12px" }} marginTop='0px'>{userData.Email}</CustomTypography>
                </Box>
                <Box sx={{ display: "flex", justifyContent: "space-between" }}>
                    <CustomTypography sx={{ fontSize: "12px", fontWeight: "bold" }} marginTop='0px'>🆔 Username:</CustomTypography>
                    <CustomTypography sx={{ fontSize: "12px" }} marginTop='0px'>{userData.Username}</CustomTypography>
                </Box>
                <Box sx={{ display: "flex", justifyContent: "space-between" }}>
                    <CustomTypography sx={{ fontSize: "12px", fontWeight: "bold" }} marginTop='0px'>📍Current Location:</CustomTypography>
                    <CustomTypography sx={{ fontSize: "12px" }} marginTop='0px'>{locationData}</CustomTypography>
                </Box>
            </Box>
        </Modal>
    );
};

export default CustomUserModal;
