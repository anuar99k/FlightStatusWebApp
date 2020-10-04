import Vue from 'vue';
import Vuetify from 'vuetify/lib';

Vue.use(Vuetify);

import ru from 'vuetify/es5/locale/ru'

Vue.component('my-component', {
    methods: {
		changeLocale () {
			this.$vuetify.lang.current = 'ru'
		},
    },
})

export default new Vuetify({
	lang: {
		locales: { ru },
		current: 'ru',
	}
});
