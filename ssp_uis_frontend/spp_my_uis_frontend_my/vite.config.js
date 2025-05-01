import { defineConfig } from 'vite';
import vue from '@vitejs/plugin-vue2';
const path = require('path');
import basicSsl from '@vitejs/plugin-basic-ssl'

// https://vitejs.dev/config/
export default defineConfig({
    plugins: [vue(),basicSsl()],
    server: {
        https:true,
        port: 8082,
        host: '127.0.0.1',
        open: 'https://sspmyuis-ui.apptest.uz:8082/'
    },
    resolve: {
        alias: {
            '@': path.resolve(__dirname, './src')
        }
    }
});
