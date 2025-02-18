import React, { useState } from 'react';
import Popover from '@mui/material/Popover';
import PushPinIcon from '@mui/icons-material/PushPin';
import { Box, InputLabel, MenuItem, FormControl, Select, Rating } from '@mui/material';
import CustomTypography from '../customFormControls/CustomTypography';
import CustomTextField from '../customFormControls/CustomTextField';
import CustomButton from '../customFormControls/CustomButton';
import { useSelector } from 'react-redux';
import CustomSnackbar from './CustomSnackbar';
import { addNewPin } from '../../services/pinService';

const CustomInputModal = ({ open, setOpen, anchorPosition, callbackPins }) => {
    const handleClose = () => {
        setOpen(false);
    };

    const [placeName, setPlaceName] = useState('')
    const [selectedCategory, setSelectedCategory] = useState('')
    const [review, setReview] = useState('')
    const [rating, setRating] = useState(0);
    const location = useSelector((state) => state.location.location)
    const userInfo = useSelector((state) => state.user.user)
    const [snackbar, setSnackbar] = useState({ open: false, msg: '', severity: 'success' });

    const id = open ? 'custom-popover' : undefined;

    const handleSubmitReview = async () => {

        if (!placeName || !selectedCategory || !review || rating === 0) {
            setSnackbar({ open: true, msg: "All Fields are mandatory!", severity: 'error' });
            return;
        }
        const data = {
            title: placeName,
            description: review,
            category: selectedCategory,
            rating: rating,
            latitude: location.lat,
            longitude: location.lng,
            userId: userInfo.UserId
        }
        try {
            const res = await addNewPin(data);
            setSnackbar({ open: true, msg: res.message, severity: 'success' });
            handleClose();
        }
        catch (err) {
            setSnackbar({ open: true, msg: err, severity: 'error' });
            console.error("Unable to fetch pins:", err);
        } finally {
            callbackPins();
        }

    }

    const handleCloseSnackbar = () => {
        setSnackbar({ ...snackbar, open: false });
    };

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
                    value={placeName}
                    onChange={(e) => setPlaceName(e.target.value)}
                    icon={<PushPinIcon />}
                />

                {/* Category Selection */}
                <FormControl size="small" >
                    <InputLabel id="demo-simple-select-label" sx={{ fontFamily: "'Merriweather', serif", fontSize: "12px" }}>Select Category</InputLabel>
                    <Select
                        labelId="demo-simple-select-label"
                        id="demo-simple-select"
                        label="Select Category"
                        value={selectedCategory}
                        onChange={(e) => setSelectedCategory(e.target.value)}
                        sx={{ fontFamily: "'Merriweather', serif", fontSize: "12px" }}
                    >
                        <MenuItem value="restaurant" sx={{ fontFamily: "'Merriweather', serif", fontSize: "12px" }} >Restaurants</MenuItem>
                        <MenuItem value="lodge" sx={{ fontFamily: "'Merriweather', serif", fontSize: "12px" }}>Lodge</MenuItem>
                        <MenuItem value="hospital" sx={{ fontFamily: "'Merriweather', serif", fontSize: "12px" }}>Hospital</MenuItem>
                        <MenuItem value="others" sx={{ fontFamily: "'Merriweather', serif", fontSize: "12px" }}>Others</MenuItem>
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
                    value={review}
                    onChange={(e) => setReview(e.target.value)}
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
                <CustomButton
                    sx={{
                        '& .MuiTypography-root': {
                            fontSize: "12px"
                        }
                    }}
                    onClick={handleSubmitReview}
                >
                    Submit review
                </CustomButton>
            </Box>
            {/* Custom Snackbar to display messages */}
            <CustomSnackbar
                open={snackbar.open}
                onClose={handleCloseSnackbar}
                severity={snackbar.severity}
                msg={snackbar.msg}
            />
        </Popover>
    );
};

export default CustomInputModal;
