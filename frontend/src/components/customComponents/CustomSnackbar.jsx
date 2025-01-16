import React from 'react';
import { Snackbar, Alert, Slide } from '@mui/material';

const SlideTransition = (props) => <Slide {...props} direction="up" />;

const CustomSnackbar = ({ open, msg, severity = "info", onClose }) => {
    return (
        <Snackbar
            open={open}
            autoHideDuration={3000}
            onClose={onClose}
            anchorOrigin={{ vertical: 'top', horizontal: 'center' }}
            key={SlideTransition}
        >
            <Alert onClose={onClose} severity={severity} sx={{ fontFamily: "'Merriweather', serif" }}>
                {msg}
            </Alert>
        </Snackbar>
    );
}

export default CustomSnackbar;
