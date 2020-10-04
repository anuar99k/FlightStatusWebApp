import api from "../api/auth";

export default {
    state: {
        isLoggedIn: false
    },
    actions: {
        register(stateContext, data) {
            return new Promise((resolve, reject) => {
                api.register(
                    successResponse => {
                        stateContext.commit("isLoggedIn", true);
                        resolve(successResponse);
                    },
                    errorResponse => reject(errorResponse),
                    data
                );
            });
        },
        login(stateContext, data) {
            return new Promise((resolve, reject) => {
                api.login(
                    successResponse => { 
                        stateContext.commit("isLoggedIn", true);
                        resolve(successResponse)
                    },
                    errorResponse => reject(errorResponse),
                    data
                );
            });
        },
        logout(stateContext) {
            return new Promise((resolve, reject) => {
                // TODO: revoke token on server
                stateContext.commit("isLoggedIn", false);
                resolve();
                reject();
            });
        }
    },
    mutations: {
        isLoggedIn(state, value) {
            state.isLoggedIn = value;
        }
    },
    getters: {
        isLoggedIn(state) {
            return state.isLoggedIn;
        }
    }
}