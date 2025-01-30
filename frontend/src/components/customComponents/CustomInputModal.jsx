import React, { useState } from 'react';
import Popover from '@mui/material/Popover';
import StarIcon from '@mui/icons-material/Star';
import StarBorderIcon from '@mui/icons-material/StarBorder';
import PushPinIcon from '@mui/icons-material/PushPin';
import { Box, InputLabel, MenuItem, FormControl, Select, Rating } from '@mui/material';
import CustomTypography from '../customFormControls/CustomTypography';
import CustomTextField from '../customFormControls/CustomTextField';
import CustomButton from '../customFormControls/CustomButton';

const CustomInputModal = ({ open, setOpen, anchorPosition }) => {
    const handleClose = () => {
        setOpen(false);
    };
    const [rating, setRating] = useState(0);
    const categories = [
        {
            name: "Restaurants",
            value: 1
        },
        {
            name: "Lodge",
            value: 2
        },
        {
            name: "Hospital",
            value: 3
        },
        {
            name: "Others",
            value: 4
        }
    ];
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

                <FormControl size="small" >
                    <InputLabel id="demo-simple-select-label" sx={{ fontFamily: "'Merriweather', serif", fontSize: "12px" }}>Select Category</InputLabel>
                    <Select
                        labelId="demo-simple-select-label"
                        id="demo-simple-select"
                        label="Select Category"
                        sx={{ fontFamily: "'Merriweather', serif", fontSize: "12px" }}
                    >
                        {
                            categories.map((category, index) => {
                                return (
                                    <>
                                        <MenuItem key={index} value={category.value} sx={{ fontFamily: "'Merriweather', serif", fontSize: "12px" }}>{category.name}</MenuItem>
                                    </>
                                )
                            })
                        }
                    </Select>
                </FormControl>

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
                    rows={2}
                />

                {/* Ratings Section */}
                <Box>
                    <CustomTypography sx={{ fontSize: "12px", marginTop: "0px" }}>
                        Rate your place
                    </CustomTypography>
                    <Box sx={{ display: "flex", gap: 0.5 }}>
                        <Rating
                            name="simple-controlled"
                            value={rating}
                            onChange={(e, newValue) => {
                                setRating(newValue);
                            }}
                        />
                    </Box>
                </Box>
                <CustomButton sx={{
                    '& .MuiTypography-root': {
                        fontSize: "12px"
                    }
                }}>
                    Submit review
                </CustomButton>
            </Box>
        </Popover>
    );
};

export default CustomInputModal;
