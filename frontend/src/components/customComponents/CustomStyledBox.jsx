
import { styled } from '@mui/material/styles';
import Box from '@mui/material/Box';

const CustomStyledBox = styled(Box)(({ theme }) => ({
    position: "absolute",
    top: "50%",
    left: "50%",
    transform: "translate(-50%,-50%)",
    width: "79%",
    height: "auto",
    backgroundColor: "white",
    boxShadow: 24,
    borderRadius: theme.shape.borderRadius,
}));

export default CustomStyledBox;
