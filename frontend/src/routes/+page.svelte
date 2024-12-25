<script>
    import { onMount } from 'svelte';
  
    let message = '';
  
    onMount(async () => {
      const API_URL = import.meta.env.VITE_BACKEND_HOST || 'http://localhost';
      const API_PORT = import.meta.env.VITE_BACKEND_PORT || '5000';
  
      try {
        const response = await fetch(`${API_URL}:${API_PORT}/api`);
        const data = await response.json();
        message = data.message; // Guardamos el mensaje en la variable
      } catch (error) {
        console.error('Error al conectar con el backend:', error);
        message = 'No se pudo obtener el mensaje'; // Si hay error, mostramos un mensaje alternativo
      }
    });
</script>
  
  <main>
    <h1>Mensaje desde el Backend</h1>
    <p>{message}</p> <!-- Mostramos el mensaje obtenido de la API -->
  </main>
  