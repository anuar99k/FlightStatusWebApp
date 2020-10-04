import Vue from 'vue'
import Vuex from 'vuex'
import CreatePersistedState from "vuex-persistedstate"

import auth from './auth'
import flights from './flights'

Vue.use(Vuex)

export default new Vuex.Store({
	plugins: [CreatePersistedState()], // save state to localstorage
	state: {
	},
	mutations: {
	},
	actions: {
	},
	modules: {
		auth, flights
	}
})