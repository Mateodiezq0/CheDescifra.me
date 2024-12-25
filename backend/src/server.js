const path = require('path');
require('dotenv').config({ path: path.resolve(__dirname, '../../.env') }); // Cargar .env desde la raíz

const express = require('express');
const cors = require('cors');  // Importa cors
const app = express();
app.use(cors()); // Habilitar CORS para todas las rutas

// Variables desde el .env
const HOST = process.env.BACKEND_HOST || 'localhost'; // Valor por defecto: localhost
const PORT = process.env.BACKEND_PORT || 5000;        // Valor por defecto: 5000

app.use(express.json());

// Ruta de prueba
app.get('/api', (req, res) => {
  res.json({ message: '¡Hola desde el backend!' });
});

// Iniciar servidor
app.listen(PORT, HOST, () => {
  console.log(`Servidor corriendo en http://${HOST}:${PORT}`);
});
