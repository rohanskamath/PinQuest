import React from 'react'
import Popover from '@mui/material/Popover';
import StarBorderIcon from '@mui/icons-material/StarBorder';
import CustomTypography from '../customFormControls/CustomTypography';
import { Box } from '@mui/material';

const CustomModal = ({ open, setOpen, anchorPosition }) => {

    const handleClose = () => {
        setOpen(false);
    };

    const id = open ? 'custom-popover' : undefined;

    return (
        <>
            <Popover
                id={id}
                open={open}
                onClose={handleClose}
                anchorReference="anchorPosition"
                anchorPosition={anchorPosition}
            >
                <div className='card'>
                    <Box sx={{ display: "flex" }}>
                        <CustomTypography sx={{ fontSize: "15px", color: "red", fontWeight: "bold" }} marginTop='0px'>Place name:&nbsp;</CustomTypography>
                        <CustomTypography sx={{ fontSize: "15px" }} marginTop='0px'>Sigma Park</CustomTypography>
                    </Box>
                    <CustomTypography sx={{ fontSize: "15px", color: "red", fontWeight: "bold" }} marginTop='0px'>Review</CustomTypography>
                    <div className='placeholder'>
                        <CustomTypography sx={{ fontSize: "12px" }} marginTop='0px'>Good place</CustomTypography>
                    </div>
                    <CustomTypography sx={{ fontSize: "15px", color: "red", fontWeight: "bold" }} marginTop='0px'>Ratings</CustomTypography>
                    <div className='stars'>
                        <StarBorderIcon />
                        <StarBorderIcon />
                        <StarBorderIcon />
                        <StarBorderIcon />
                        <StarBorderIcon />
                    </div>
                    <CustomTypography sx={{ fontSize: "15px", color: "red", fontWeight: "bold" }} marginTop='0px'>Created by</CustomTypography>
                    <Box sx={{ display: "flex" }}>
                        <CustomTypography sx={{ fontSize: "15px", fontWeight: "bold" }} marginTop='0px'>Rohan Kamath - &nbsp;</CustomTypography>
                        <CustomTypography sx={{ fontSize: "15px", }} marginTop='0px'>1 hour ago</CustomTypography>
                    </Box>

                </div>
            </Popover>
        </>
    )
}


export default CustomModal
