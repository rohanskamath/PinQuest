import React from 'react'
import Popover from '@mui/material/Popover';
import { Box, Stack } from '@mui/material';
import CommentsDisabledIcon from '@mui/icons-material/CommentsDisabled';
import CustomTypography from '../customFormControls/CustomTypography';

const CustomReviewModal = ({ open, setOpen, anchorPosition, pin }) => {

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
                                    <CustomTypography sx={{ fontSize: "12px", color: "#660033", fontWeight: "bold" }} marginTop='0px'>Ratings:&nbsp;</CustomTypography>
                                    <Box
                                        sx={{
                                            backgroundColor: parseInt(pin?.avgRating) >= 4 ? "green" :
                                                parseInt(pin?.avgRating) === 3 ? "gold" :
                                                    "red",
                                            padding: "0 10px",
                                            borderRadius: "5px"
                                        }}
                                    >
                                        <CustomTypography sx={{ fontSize: "12px", color: "white" }} marginTop='0px'>{pin?.avgRating} &#x2606;</CustomTypography>
                                    </Box>
                                </Box>
                                <CustomTypography sx={{ fontSize: "12px", color: "#660033", fontWeight: "bold" }} marginTop='0px'>Reviews:</CustomTypography>
                                <Box sx={{
                                    maxHeight: "100px",
                                    overflowY: "auto",
                                    overflowX: "hidden",
                                    padding: "0 6px 0 0"
                                }}>
                                    {
                                        
                                        pin.reviews.map((review, index) => {
                                            console.log(pin);
                                            return (
                                                <Stack key={index}>
                                                    <Box sx={{ display: "flex", justifyContent: "space-between" }}>
                                                        <CustomTypography sx={{ fontSize: "12px" }} marginTop='0px'>{review?.username}</CustomTypography>
                                                        <CustomTypography sx={{ fontSize: "12px", fontWeight: "bold" }} marginTop='0px'>{review?.category}</CustomTypography>
                                                        <Box
                                                            sx={{
                                                                backgroundColor: review?.rating >= 4 ? "green" :
                                                                    review?.rating === 3 ? "gold" :
                                                                        "red",
                                                                padding: "0 5px",
                                                                borderRadius: "5px"
                                                            }}
                                                        >
                                                            <CustomTypography sx={{ fontSize: "12px", color: "white" }} marginTop='0px'>{review?.rating} &#x2606;</CustomTypography>
                                                        </Box>
                                                    </Box>
                                                    <Box sx={{
                                                        margin: "5px 0px",
                                                        maxHeight: "100px",
                                                        overflowY: "auto",
                                                        overflowX: "hidden",
                                                        border: "1px solid #ccc",
                                                        borderRadius: "5px"
                                                    }}>
                                                        <CustomTypography sx={{ fontSize: "12px", padding: "2px 4px" }} marginTop='0px'>{review?.description}</CustomTypography>
                                                    </Box>
                                                </Stack>
                                            )
                                        })
                                    }


                                </Box>

                            </div>
                        </Popover>
                    </>
            }
        </>
    )
}


export default CustomReviewModal
