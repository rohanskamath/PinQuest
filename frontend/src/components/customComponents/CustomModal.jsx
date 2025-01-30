import React from 'react'
import Popover from '@mui/material/Popover';
import { Box } from '@mui/material';
import StarIcon from '@mui/icons-material/Star';
import StarBorderIcon from '@mui/icons-material/StarBorder';
import CommentsDisabledIcon from '@mui/icons-material/CommentsDisabled';
import CustomTypography from '../customFormControls/CustomTypography';
import { createUserName } from '../../services/authService';

const CustomModal = ({ open, setOpen, anchorPosition, pin }) => {

    const handleClose = () => {
        setOpen(false);
    };

    const id = open ? 'custom-popover' : undefined;

    return (
        <>
            {
                pin === undefined ?
                    <>
                        <Popover
                            id={id}
                            open={open}
                            onClose={handleClose}
                            anchorReference="anchorPosition"
                            anchorPosition={anchorPosition}
                        >
                            <div className='card'>
                                <Box sx={{ display: "flex", justifyContent: "center" }}>
                                    <CustomTypography sx={{ fontSize: "15px", color: "red", fontWeight: "bold" }} marginTop='0px'><CommentsDisabledIcon sx={{ fontSize: "16px" }} /> No Reviews yet</CustomTypography>
                                </Box>
                            </div>
                        </Popover>
                    </>
                    :
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
                                    <CustomTypography sx={{ fontSize: "15px" }} marginTop='0px'>{pin?.title}</CustomTypography>
                                </Box>
                                <CustomTypography sx={{ fontSize: "15px", color: "red", fontWeight: "bold" }} marginTop='0px'>Review</CustomTypography>
                                <div className='placeholder'>
                                    <CustomTypography sx={{ fontSize: "12px" }} marginTop='0px'>{pin?.desc}</CustomTypography>
                                </div>
                                <CustomTypography sx={{ fontSize: "15px", color: "red", fontWeight: "bold" }} marginTop='0px'>Ratings</CustomTypography>
                                <div className='stars'>
                                    {
                                        [...Array(5)].map((_, index) => {
                                            return (
                                                index < pin?.rating ? (
                                                    <StarIcon key={index} />
                                                ) : (
                                                    <StarBorderIcon key={index} />
                                                )
                                            )
                                        })
                                    }
                                </div>
                                <CustomTypography sx={{ fontSize: "15px", color: "red", fontWeight: "bold" }} marginTop='0px'>Created by</CustomTypography>
                                <CustomTypography sx={{ fontSize: "12px" }} marginTop='0px'>{createUserName(pin?.email)} - {"1 Hour ago"}</CustomTypography>
                            </div>
                        </Popover>
                    </>
            }
        </>
    )
}


export default CustomModal
