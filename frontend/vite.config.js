import { sveltekit } from '@sveltejs/kit/vite';
import { defineConfig } from 'vite';

export default defineConfig({
	plugins: [sveltekit()],
	define: {
		'process.env': process.env, // Esto también podría no ser necesario si no lo estás utilizando
	  },
});