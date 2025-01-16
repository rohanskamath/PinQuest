import React from 'react';
import { fireEvent, render, screen } from '@testing-library/react';
import { BrowserRouter as Router } from 'react-router-dom';
import '@testing-library/jest-dom';
import ForgotPassword from './ForgotPassword';

describe('Forgot password page', () => {
    test('TC001:- Renders Forgot passowrd Page <ForgotPassword /> with all fields', () => {
      render(
        <Router>
            <ForgotPassword />
        </Router>
      )
    })
    
});
