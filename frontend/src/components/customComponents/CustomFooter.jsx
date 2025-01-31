import { Box } from '@mui/material'
import React from 'react'
import CustomTypography from '../customFormControls/CustomTypography'

const CustomFooter = () => {
    return (
        <>
            <footer>
                <Box component="section" sx={{ p: 2, backgroundColor: "black", textAlign: "center" }}>
                    <CustomTypography color='white' sx={{ fontSize: "12px" }}>&copy; {new Date().getFullYear()} PinQuest. All rights reserved.</CustomTypography>
                </Box>
            </footer>
        </>
    )
}

export default CustomFooter
