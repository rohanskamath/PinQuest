import React, { useState } from 'react';
import Popover from '@mui/material/Popover';
import StarIcon from '@mui/icons-material/Star';
import StarBorderIcon from '@mui/icons-material/StarBorder';
import PushPinIcon from '@mui/icons-material/PushPin';
import { Box } from '@mui/material';
import CustomTypography from '../customFormControls/CustomTypography';
import CustomTextField from '../customFormControls/CustomTextField';
import CustomButton from '../customFormControls/CustomButton';

const CustomInputModal = ({ open, setOpen, anchorPosition }) => {
    const handleClose = () => {
        setOpen(false);
    };
    const [rating, setRating] = useState(0);
    const id = open ? 'custom-popover' : undefined;

    return (
        <Popover
            id={id}
            open={open}
            onClose={handleClose}
            anchorReference="anchorPosition"
            anchorPosition={anchorPosition}
        >
            <Box sx={{ display: "flex", flexDirection: "column", gap: 2, padding: "20px" }}>
                {/* Place Name Input */}
                <CustomTextField
                    sx={{
                        '& .MuiInputBase-root': {
                            backgroundColor: 'white',
                        },
                    }}
                    autoComplete="off"
                    label={"Enter place name"}
                    autoFocus
                    icon={<PushPinIcon />}
                />

                {/* Review Input */}
                <CustomTextField
                    sx={{
                        '& .MuiInputBase-root': {
                            backgroundColor: 'white',
                        },

                        '& .MuiInputAdornment-root': {
                            display: "none"
                        }
                    }}
                    autoComplete="off"
                    label={"Enter your review here"}
                    multiline
                    rows={3}
                />

                {/* Ratings Section */}
                <Box>
                    <CustomTypography sx={{ fontSize: "12px", marginTop: "0px" }}>
                        Rate your place
                    </CustomTypography>
                    <Box sx={{ display: "flex", gap: 0.5 }}>
                        {[...Array(5)].map((_, index) => (
                            <Box key={index} onClick={() => setRating(index + 1)} sx={{ cursor: "pointer" }}>
                                {index < rating ? (
                                    <StarIcon sx={{ color: "#FFD700" }} />
                                ) : (
                                    <StarBorderIcon sx={{ color: "#FFD700" }} />
                                )}
                            </Box>
                        ))}
                    </Box>
                </Box>
                <CustomButton sx={{ '& .MuiTypography-root':{
                    fontSize:"12px"
                } }}>
                    Submit review
                </CustomButton>
            </Box>
        </Popover>
    );
};

export default CustomInputModal;
