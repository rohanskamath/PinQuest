import { Typography } from '@mui/material';
import React from 'react';

const CustomTypography = ({
  variant = "h6",
  color = "black",
  fontFamily = "'Merriweather', serif",
  marginTop = "10px",
  children,
  ...props
}) => {
  return (
    <Typography
      variant={variant}
      color={color}
      {...props}
      sx={{
        fontFamily: fontFamily,
        marginTop: marginTop,
        ...props.sx
      }}
    >{children}</Typography>
  );
}

export default CustomTypography;
