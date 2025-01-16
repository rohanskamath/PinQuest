import React from 'react';
import Button from '@mui/material/Button';
import { styled } from '@mui/material/styles';
import Typography from '@mui/material/Typography';

const StyledButton = styled(Button)(({ theme }) => ({
  borderRadius: 8,
  color: "black",
  minWidth: "170px",
  backgroundColor: "#FF9A01",
}));

const CustomButton = ({ children, onClick, type = 'submit', ...props }) => {
  return (
    <StyledButton type={type} variant="contained" size="normal" onClick={onClick} {...props}>
      <Typography sx={{ fontFamily: 'Merriweather, sans-serif' }} variant="button">{children}</Typography>
    </StyledButton>
  );
};

export default CustomButton;
