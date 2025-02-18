import React from 'react';
import Card from '@mui/material/Card';
import CardContent from '@mui/material/CardContent';
import { Box, Rating } from '@mui/material';
import CustomTypography from '../customFormControls/CustomTypography';

const CustomCard = ({ cards }) => {

    return (
        <Box
            sx={{
                display: 'flex',
                flexWrap: 'wrap',
                justifyContent: 'center',
                gap: 2,
            }}
        >
            {cards.map((card, index) => (
                <Card key={index}
                    sx={{
                        width: 300,
                        height: 170,
                        margin: '20px',
                        display: 'flex',
                        flexDirection: 'column',
                        justifyContent: 'space-between'
                    }}
                >
                    <CardContent>
                        <CustomTypography variant="h5">
                            {card.title}
                        </CustomTypography>
                        <CustomTypography variant="body2" color="text.secondary">
                            {card.description}
                        </CustomTypography>
                        <CustomTypography variant="body2" color="text.secondary">
                            Category:&nbsp;{card.category}
                        </CustomTypography>
                        <CustomTypography variant="body2" color="text.secondary">
                            <Rating
                            readOnly 
                                name="simple-controlled"
                                value={card.rating}
                            />
                        </CustomTypography>
                    </CardContent>
                </Card>
            ))}
        </Box>
    );
};

export default CustomCard;
