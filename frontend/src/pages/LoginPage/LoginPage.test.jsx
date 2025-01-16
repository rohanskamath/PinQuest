import React from 'react';
import { fireEvent, render, screen } from '@testing-library/react';
import { BrowserRouter as Router } from 'react-router-dom';
import '@testing-library/jest-dom';
import LoginPage from './LoginPage';

describe('Login Page', () => {

    test('TC001:- Renders Login Page <Loginpage /> with all fields', () => {
        render(
            <Router>
                <LoginPage />
            </Router>
        );
        const animation = screen.getByTestId('lottie-animation')
        expect(animation).toBeInTheDocument();

        const avatar = screen.getByTestId('avatar')
        expect(avatar).toBeInTheDocument();

        expect(screen.getByText(/SignIn \/ LogIn/i)).toBeInTheDocument();

        expect(screen.getByLabelText(/Enter your email-id/i)).toBeInTheDocument();
        expect(screen.getByLabelText(/Enter your password/i)).toBeInTheDocument();

        expect(screen.getByText(/Forgot password/i)).toBeInTheDocument();

        expect(screen.getByRole('button', { name: /Sign In/i })).toBeInTheDocument();

        expect(screen.getByText(/Not registered yet?/i)).toBeInTheDocument();
        expect(screen.getByText(/Create an account/i)).toBeInTheDocument();
    })

    test('TC002:- Show validations erros when fields are empty', async () => {
        render(
            <Router>
                <LoginPage />
            </Router>
        );

        const signInBtn = screen.getByRole('button', { name: /Sign In/i });
        fireEvent.click(signInBtn)
        expect(await screen.findByText(/\* Email is required/i)).toBeInTheDocument();
        expect(await screen.findByText(/\* Password is required/i)).toBeInTheDocument();
    })
});
