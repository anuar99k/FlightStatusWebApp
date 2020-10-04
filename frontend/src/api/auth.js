import axios from '../axios/index'

export default {
    register(resolve, reject, data) {
        axios.post("/api/auth/register", data)
            .then(response => resolve(response.data))
            .catch(error => reject(error.response));
    },
    login(resolve, reject, data) {
        axios.post("api/auth/login", data)
            .then(response => resolve(response.data))
            .catch(error => reject(error.response));
    }
}