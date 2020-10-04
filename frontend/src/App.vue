<template>
	<v-app>
		<v-app-bar app flat>
			<Header />
		</v-app-bar>

		<v-main>
			<!-- динамическая загрузка контента -->
			<router-view></router-view>
		</v-main>
	</v-app>
</template>

<script>
import Header from '@/components/public/Header';

export default {
	name: 'App',

	components: {
		Header
	}
}
</script>