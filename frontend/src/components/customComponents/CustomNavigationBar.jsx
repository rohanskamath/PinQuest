import React, { useEffect, useState } from 'react';
import { styled } from '@mui/material/styles';
import AppBar from '@mui/material/AppBar';
import Box from '@mui/material/Box';
import Toolbar from '@mui/material/Toolbar';
import Cookies from "js-cookie";
import InputBase from '@mui/material/InputBase';
import AccountCircleIcon from '@mui/icons-material/AccountCircle';
import LocationSearchingIcon from '@mui/icons-material/LocationSearching';
import LogoutIcon from '@mui/icons-material/Logout';
import SearchIcon from '@mui/icons-material/Search';
import MenuItem from '@mui/material/MenuItem';
import Menu from '@mui/material/Menu';
import { IconButton, List, ListItem, ListItemText } from '@mui/material';
import AccountCircleSharpIcon from '@mui/icons-material/AccountCircleSharp';
import MyLocationIcon from '@mui/icons-material/MyLocation';
import { useDispatch } from 'react-redux';
import CustomTypography from '../../components/customFormControls/CustomTypography';
import CustomSnackbar from './CustomSnackbar';
import CustomUserModal from './CustomUserModal';
import { searchLocation } from '../../services/locationService';
import { setLocation, setPlaceName, clearLocationData } from '../../redux/slices/locationSlice';
import { clearUserData } from '../../redux/slices/userSlice'
import { useNavigate } from 'react-router-dom';

const Search = styled('div')(({ theme }) => ({
    position: 'relative',
    borderRadius: theme.shape.borderRadius,

    backgroundColor: 'rgba(206, 207, 216, 0.9)',
    '&:hover': {
        backgroundColor: 'rgba(206, 207, 216, 0.9)',
    },
    marginLeft: 0,
    width: '100%',
    [theme.breakpoints.up('sm')]: {
        marginLeft: theme.spacing(1),
        width: 'auto',
    },
}));

const SearchIconWrapper = styled('div')(({ theme }) => ({
    padding: theme.spacing(0, 2),
    height: '100%',
    position: 'absolute',
    pointerEvents: 'none',
    display: 'flex',
    alignItems: 'center',
    justifyContent: 'center',
}));

const StyledInputBase = styled(InputBase)(({ theme }) => ({
    color: 'black',
    width: '100%',
    fontFamily: "'Merriweather', serif",
    '& .MuiInputBase-input': {
        padding: theme.spacing(1, 1, 1, 0),
        paddingLeft: `calc(1em + ${theme.spacing(4)})`,
        transition: theme.transitions.create('width'),
        [theme.breakpoints.up('sm')]: {
            width: '30ch',
            '&:focus': {
                width: '36ch',
            },
        },
    },
}));

const CustomNavigationBar = ({ placeName }) => {
    const dispatch = useDispatch();
    const navigate = useNavigate();
    const [anchorEl, setAnchorEl] = useState(null);
    const [searchQuery, setSearchQuery] = useState(placeName)
    const [suggestions, setSuggestions] = useState([])
    const [open, setOpen] = React.useState(false);
    const isMenuOpen = Boolean(anchorEl);
    const [snackbar, setSnackbar] = useState({ open: false, msg: '', severity: 'success' });

    const handleProfileMenuOpen = (event) => {
        setAnchorEl(event.currentTarget);
    };

    const handleMenuClose = () => {
        setAnchorEl(null);
    };

    // Set default search query based on placeName
    useEffect(() => {
        if (placeName) {
            setSearchQuery(placeName);
        }
    }, [placeName]);


    const handleSearchQuery = async (e) => {
        const query = e.target.value;
        setSearchQuery(query);
        if (query.length > 2) {
            try {
                const res = await searchLocation(query);
                setSuggestions(res.features || [])
            }
            catch (err) {
                console.log('Error fetching search suggestions:', err);
            }
        } else {
            setSuggestions([])
        }
    }

    const handleSuggestionClick = (suggestion) => {
        setSearchQuery(suggestion.place_name);
        setSuggestions([]);
        dispatch(setLocation({
            lat: suggestion.geometry.coordinates[1],
            lng: suggestion.geometry.coordinates[0],
        }))
        dispatch(setPlaceName(suggestion.place_name))
    };

    const handleCloseSnackbar = () => {
        setSnackbar({ ...snackbar, open: false });
    };

    const handleVisits = () => {
        handleMenuClose();
        navigate("/visits")
    }

    const handleLogout = () => {
        setSnackbar({ open: true, msg: 'Logging out', severity: 'success' });
        Cookies.remove("token");
        dispatch(clearUserData());
        dispatch(clearLocationData());
        handleMenuClose();
    }

    const handleOpen = () => {
        setOpen(true)
    }

    return (
        <>
            <Box sx={{ flexGrow: 1 }}>
                <AppBar position="static" sx={{ backgroundColor: "#660033" }}>
                    <Toolbar>
                        <span><MyLocationIcon sx={{ margin: '4px 5px 0 0', fontSize: '24px' }} /></span>
                        <CustomTypography onClick={() => navigate("/")} color="white" sx={{ flexGrow: 1, fontSize: '24px', margin: '0 10px 0 0', cursor: "pointer" }}>
                            PinQuest
                        </CustomTypography>
                        <Search sx={{ display: { xs: 'none', sm: 'block' }, margin: '0 10px 0 0' }}>
                            <SearchIconWrapper>
                                <SearchIcon sx={{ color: 'black' }} />
                            </SearchIconWrapper>
                            <StyledInputBase
                                sx={{ fontSize: "12px" }}
                                placeholder="Search location...."
                                inputProps={{ 'aria-label': 'search' }}
                                value={searchQuery || ""}
                                onChange={handleSearchQuery}
                            />
                            {suggestions.length > 0 && (
                                <List
                                    sx={{
                                        position: 'absolute',
                                        zIndex: 10,
                                        color: 'black',
                                        backgroundColor: 'white',
                                        maxHeight: '200px',
                                        overflowY: 'auto',
                                        width: '100%',
                                        cursor: 'pointer'
                                    }}
                                >
                                    {suggestions.map((suggestion, index) => (
                                        <ListItem
                                            button={true}
                                            key={index}
                                            onClick={() => handleSuggestionClick(suggestion)}
                                        >
                                            <ListItemText primary={suggestion.place_name} />
                                        </ListItem>
                                    ))}
                                </List>
                            )}
                        </Search>
                        <div>
                            <IconButton
                                aria-label="account of current user"
                                aria-controls="menu-appbar"
                                aria-haspopup="true"
                                color="inherit"
                                onClick={handleProfileMenuOpen}>
                                <AccountCircleSharpIcon sx={{ color: "white", fontSize: "30px" }} />
                            </IconButton>
                            <Menu
                                id="menu-appbar"
                                anchorEl={anchorEl}
                                open={isMenuOpen}
                                onClose={handleMenuClose}
                                anchorOrigin={{
                                    vertical: 'top',
                                    horizontal: 'right',
                                }}
                                transformOrigin={{
                                    vertical: 'top',
                                    horizontal: 'right',
                                }}
                            >
                                <MenuItem sx={{ fontFamily: "'Merriweather', serif", fontSize: "14px" }} onClick={handleOpen}><AccountCircleIcon />&nbsp;Profile</MenuItem>
                                <MenuItem sx={{ fontFamily: "'Merriweather', serif", fontSize: "14px" }} onClick={handleVisits}><LocationSearchingIcon />&nbsp;My Visits</MenuItem>
                                <MenuItem sx={{ fontFamily: "'Merriweather', serif", fontSize: "14px" }} onClick={handleLogout}><LogoutIcon />&nbsp;Logout</MenuItem>
                            </Menu>
                        </div>
                    </Toolbar>
                </AppBar>
            </Box>
            {
                open && <CustomUserModal open={open} setOpen={setOpen} />
            }
            {/* Custom Snackbar to display messages */}
            <CustomSnackbar
                open={snackbar.open}
                onClose={handleCloseSnackbar}
                severity={snackbar.severity}
                msg={snackbar.msg}
            />
        </>
    );
};

export default CustomNavigationBar;
