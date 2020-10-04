import axios from 'axios';
import router from '@/router/index.js';
import store from '@/store/index.js'

const api = axios.create({ baseURL: process.env.VUE_APP_API_URL });

api.interceptors.request.use(request => {
    request.withCredentials = true; // allows sending cookie to different domain
    return request;
});

api.interceptors.response.use(
    response => response,
    error => {
        if (error.response.status === 401) {
            store.dispatch('logout');
            router.push("/login");
            return Promise.resolve(error.response);
        } else {
            return Promise.reject(error);
    }
})

export default api;