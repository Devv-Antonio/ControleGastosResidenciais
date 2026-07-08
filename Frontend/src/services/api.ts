import axios from 'axios';

export const api = axios.create({
  // O Vite usa import.meta.env para ler o arquivo .env
  baseURL: import.meta.env.VITE_API_URL, 
});