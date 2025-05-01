import { defineConfig } from 'vite';
import vue from '@vitejs/plugin-vue2';
const path = require('path');

// https://vitejs.dev/config/
export default defineConfig({
   plugins: [vue()],
   server: {
      port: 8088
   },
   resolve: {
      alias: {
         '@': path.resolve(__dirname, './src'),
         '@core': path.resolve(__dirname, './src/@core'),
         '@validations': path.resolve(__dirname, 'src/@core/utils/validations/validations.js')
      },
      extensions: ['.mjs', '.js', '.json']
   }
});
