import React from 'react'
import Popover from '@mui/material/Popover';
import { Box, List, ListItem } from '@mui/material';
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
                                    <CustomTypography sx={{ fontSize: "15px", color: "red", fontWeight: "bold" }} marginTop='0px'><CommentsDisabledIcon sx={{ fontSize: "16px" }} /> No Reviews</CustomTypography>
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
                            sx={{
                                '& .MuiPaper-root': {
                                    borderRadius: "20px"
                                }
                            }}
                        >
                            <div className='card'>
                                <Box sx={{ display: "flex" }}>
                                    <CustomTypography sx={{ fontSize: "12px", color: "#660033", fontWeight: "bold" }} marginTop='0px'>Place name:&nbsp;</CustomTypography>
                                    <CustomTypography sx={{ fontSize: "12px" }} marginTop='0px'>{pin?.title}</CustomTypography>
                                </Box>
                                <Box sx={{ display: "flex" }}>
                                    <CustomTypography sx={{ fontSize: "12px", color: "#660033", fontWeight: "bold" }} marginTop='0px'>Category:&nbsp;</CustomTypography>
                                    <CustomTypography sx={{ fontSize: "12px", textTransform: "capitalize" }} marginTop='0px'>{pin?.category}</CustomTypography>
                                </Box>
                                <Box sx={{ display: "flex" }}>
                                    <CustomTypography sx={{ fontSize: "12px", color: "#660033", fontWeight: "bold" }} marginTop='0px'>Ratings:&nbsp;</CustomTypography>
                                    <Box
                                        sx={{
                                            backgroundColor: pin?.avgrating >= 4 ? "green" :
                                                pin?.avgrating === 3 ? "yellow" :
                                                    "red",
                                            padding: "0 10px",
                                            borderRadius: "5px"
                                        }}
                                    >
                                        <CustomTypography sx={{ fontSize: "12px", color: "white" }} marginTop='0px'>{pin?.avgrating} &#x2606;</CustomTypography>
                                    </Box>
                                </Box>
                                <CustomTypography sx={{ fontSize: "12px", color: "#660033", fontWeight: "bold" }} marginTop='0px'>Reviews:</CustomTypography>
                                <Box sx={{
                                    maxHeight: "100px",
                                    overflowY: "auto",
                                    overflowX: "hidden",
                                    border: "1px solid #ccc",
                                    borderRadius: "5px"
                                }}>
                                    <List
                                        sx={{
                                            '& .MuiList-root': {
                                                margin: "0px",
                                                paddingTop: "0px"
                                            }
                                        }}
                                    >
                                        {
                                            pin.reviews.map((review, index) => (
                                                <ListItem key={index} sx={{
                                                    display: "block", '& .MuiListItem-root': {
                                                        padding: "0px"
                                                    }
                                                }}>
                                                    <CustomTypography sx={{ fontSize: "10px" }} marginTop='0px'>
                                                        {review} - {createUserName(pin?.email)}
                                                    </CustomTypography>
                                                </ListItem>
                                            ))
                                        }
                                    </List>
                                </Box>

                            </div>
                        </Popover>
                    </>
            }
        </>
    )
}


export default CustomModal
