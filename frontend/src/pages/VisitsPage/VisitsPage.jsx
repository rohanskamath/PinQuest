import React, { useEffect, useState } from 'react';
import { Box, Stack } from '@mui/material';
import { useSelector } from 'react-redux';
import CustomCard from '../../components/customComponents/CustomCard';
import CustomTypography from '../../components/customFormControls/CustomTypography';
import { getPinsbyUserId } from '../../services/pinService';
import CustomSnackbar from '../../components/customComponents/CustomSnackbar';

const VisitsPage = () => {
  const [cards, setCards] = useState([]);
  const userData = useSelector((state) => state.user.user);
  const [snackbar, setSnackbar] = useState({ open: false, msg: '', severity: 'success' });

  const getAllpinById = async () => {
    try {
      const res = await getPinsbyUserId(userData.UserId);
      if (res.success) {
        setCards(res.data);
        setSnackbar({ open: true, msg: res.message, severity: 'success' });
      } else {
        setSnackbar({ open: true, msg: "Pin have not been fetched!", severity: 'error' });
      }
    }
    catch (err) {
      console.log(err)
      setSnackbar({ open: true, msg: "Something went wrong!", severity: 'error' });
    }
  }
  useEffect(() => {
    getAllpinById();
  }, [])

  const handleCloseSnackbar = () => {
    setSnackbar({ ...snackbar, open: false });
  };

  return (
    <Box
      sx={{
        display: 'flex',
        flexWrap: 'wrap',
        justifyContent: 'center',
        minHeight: '100vh',
        gap: 2,
        marginTop: "20px"
      }}
    >
      <Stack>
        <CustomTypography variant='h4' sx={{ textAlign: "center" }}>My Visits</CustomTypography>
        <CustomCard cards={cards} />
      </Stack>

      {/* Custom Snackbar to display messages */}
      <CustomSnackbar
        open={snackbar.open}
        onClose={handleCloseSnackbar}
        severity={snackbar.severity}
        msg={snackbar.msg}
      />
    </Box>
  );
};

export default VisitsPage;
