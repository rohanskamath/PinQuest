import React, { useState } from 'react';
import { styled } from '@mui/material/styles';
import AppBar from '@mui/material/AppBar';
import Box from '@mui/material/Box';
import Toolbar from '@mui/material/Toolbar';
import CustomTypography from '../../components/customFormControls/CustomTypography';
import InputBase from '@mui/material/InputBase';
import SearchIcon from '@mui/icons-material/Search';
import MenuItem from '@mui/material/MenuItem';
import Menu from '@mui/material/Menu';
import { IconButton } from '@mui/material';
import AccountCircleSharpIcon from '@mui/icons-material/AccountCircleSharp';

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
    fontFamily:"'Merriweather', serif",
    '& .MuiInputBase-input': {
        padding: theme.spacing(1, 1, 1, 0),
        paddingLeft: `calc(1em + ${theme.spacing(4)})`,
        transition: theme.transitions.create('width'),
        [theme.breakpoints.up('sm')]: {
            width: '20ch',
            '&:focus': {
                width: '26ch',
            },
        },
    },
}));

const CustomNavigationBar = () => {
    const [anchorEl, setAnchorEl] = useState(null);
    const isMenuOpen = Boolean(anchorEl);

    const handleProfileMenuOpen = (event) => {
        setAnchorEl(event.currentTarget);
    };

    const handleMenuClose = () => {
        setAnchorEl(null);
    };

    return (
        <>
            <Box sx={{ flexGrow: 1 }}>
                <AppBar position="static" sx={{ backgroundColor: "#bec2da" }}>
                    <Toolbar>
                        <CustomTypography color="black" sx={{ flexGrow: 1, fontSize: '24px', margin: '0 10px 0 0' }}>
                            Nearify
                        </CustomTypography>
                        <Search sx={{ display: { xs: 'none', sm: 'block' }, margin: '0 10px 0 0' }}>
                            <SearchIconWrapper>
                                <SearchIcon sx={{ color: 'black'}}/>
                            </SearchIconWrapper>
                            <StyledInputBase
                                placeholder="Search location...."
                                inputProps={{ 'aria-label': 'search' }}
                            />
                        </Search>
                        <div>
                            <IconButton
                                aria-label="account of current user"
                                aria-controls="menu-appbar"
                                aria-haspopup="true"
                                color="inherit"
                                onClick={handleProfileMenuOpen}>
                                <AccountCircleSharpIcon sx={{ color: "black", fontSize: "30px" }} />
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
                                <MenuItem sx={{ fontFamily:"'Merriweather', serif"}} onClick={handleMenuClose}>Profile</MenuItem>
                                <MenuItem sx={{ fontFamily:"'Merriweather', serif"}} onClick={handleMenuClose}>My Visits</MenuItem>
                                <MenuItem sx={{ fontFamily:"'Merriweather', serif"}} onClick={handleMenuClose}>Logout</MenuItem>
                            </Menu>
                        </div>
                    </Toolbar>
                </AppBar>
            </Box>
        </>
    );
};

export default CustomNavigationBar;
