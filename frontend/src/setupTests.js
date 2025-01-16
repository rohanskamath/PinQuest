import '@testing-library/jest-dom';
import 'jest-canvas-mock';
import axios from 'axios';

jest.mock('axios');

axios.create = jest.fn(() => ({
  interceptors: {
    response: {
      use: jest.fn(),
    },
  },
  get: jest.fn(),
  post: jest.fn(),
  put: jest.fn(),
  delete: jest.fn(),
}));