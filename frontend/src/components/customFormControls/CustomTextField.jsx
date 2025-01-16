import React from 'react';
import { TextField, InputAdornment } from '@mui/material';

const CustomTextField = ({ label, endIcon, isPassword = false, error = false, helperText = '', icon = '', ...props }) => {

  return (
    <TextField
      label={label}
      variant="standard"
      error={error}
      helperText={helperText}
      {...props}
      slotProps={{
        input: {
          startAdornment: (
            <InputAdornment position="start" sx={{ color: 'black' }}>
              {icon}
            </InputAdornment>
          ),
          sx: {
            color: "black",
            fontFamily: "'Merriweather', serif",
            fontSize: "12px"
          }
        },
      }}
      sx={{
        '& .MuiInput-underline:before': {
          borderBottomColor: error ? 'red' : 'black',

        },
        '& .MuiInput-underline:hover:before': {
          borderBottomColor: error ? 'red' : 'black',

        },
        '& .MuiInput-underline:after': {
          borderBottomColor: error ? 'red' : 'black',
        },
        '& .MuiInputBase-root': {
          backgroundColor: '#bec2da',
        },
        '& .MuiInputLabel-root': {
          color: 'black',
          fontFamily: "'Merriweather', serif"
        },
        '& .MuiInputLabel-root.Mui-focused': {
          color: 'black',
        },
        '& .MuiFormHelperText-root': {
          color: error ? 'red' : 'black',
          fontSize: '10px',
          fontFamily: "'Merriweather', serif",
        },
        ...props.sx
      }}
    />
  );
};

export default CustomTextField;
