// frontend/src/api.js

import axios from 'axios';

const API_URL = import.meta.env.VITE_BACKEND_HOST || 'http://localhost';
const API_PORT = import.meta.env.VITE_BACKEND_PORT || '5000';

export const getMessage = async () => {
  try {
    const response = await axios.get(`${API_URL}:${API_PORT}/api`);
    return response.data.message;
  } catch (error) {
    console.error('Error al conectar con el backend:', error);
    return null;
  }
};
