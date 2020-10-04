import axios from '../axios/index'

export default {
    getFlights(resolve, reject) {
        axios.get("/api/flights")
            .then(response => resolve(response.data))
            .catch(error => reject(error.response));
    },
    getFlight(resolve, reject, id) {
        axios.get(`api/flights/${id}`)
            .then(response => resolve(response.data))
            .catch(error => reject(error.response));
    },
    updateFlight(resolve, reject, id, data) {
        axios.put(`api/flights/${id}`, data)
            .then(response => resolve(response.data))
            .catch(error => reject(error.response));
    },
    addFlight(resolve, reject, data) {
        axios.post("api/flights", data)
            .then(response => resolve(response.data))
            .catch(error => reject(error.response));
    },
    deleteFlight(resolve, reject, id) {
        axios.delete(`api/flights/${id}`)
            .then(response => resolve(response.data))
            .catch(error => reject(error.response));
    },
}