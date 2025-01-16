import React from 'react';
import { fireEvent, render, screen } from '@testing-library/react';
import { BrowserRouter as Router } from 'react-router-dom';
import '@testing-library/jest-dom';
import RegisterPage from './RegisterPage';

describe('Register Page', () => {

    test('TC001:- Renders Register Page <RegisterPage /> with all fields', () => {
        render(
            <Router>
                <RegisterPage />
            </Router >)
    })

});
