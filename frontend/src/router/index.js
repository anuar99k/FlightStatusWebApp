import Vue from 'vue'
import VueRouter from 'vue-router'
import store from '@/store/index.js'
// public views
import Home from '../views/public/Home.vue'
import Registration from '../views/public/Registration.vue'
import Login from '../views/public/Login.vue'
// admin
import Admin from '@/views/admin/Admin.vue'

Vue.use(VueRouter)

const routes = [
	{ path: '/', name: 'Home', component: Home },
	{ path: '/registration', name: 'Registration', component: Registration, meta: { requiresVisitor: true } },
	{ path: '/login', name: 'Login', component: Login },
	{ path: '/admin', name: 'Admin', component: Admin, meta: { requiresAuth: true } }
];

const router = new VueRouter({
	mode: 'history',
	base: process.env.BASE_URL,
	routes
})

// routing configuration
router.beforeEach((to, from, next) => {
	if (to.matched.some(record => record.meta.requiresAuth)) {
		// этот путь требует авторизации, проверяем залогинен ли
		// пользователь, если нет, перенаправляем на страницу логина
		if (!store.getters.isLoggedIn) {
			next({name: 'Login'});
		} else {
			next();
		}
	} else if (to.matched.some(record => record.meta.requiresVisitor)) {
		if (store.getters.isLoggedIn) {
			// если пользователь авторизован, то при переходе на страницу
			// регистрации, он перенаправляется на страницу админа
			next({name: 'Admin'});
		} else {
			next();
		}
	} else {
		next(); // всегда так или иначе нужно вызвать next()
	}
});

export default router