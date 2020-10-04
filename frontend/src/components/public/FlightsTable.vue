<template>
	<v-container>
		<v-data-table :headers="headers" :items="flights" sort-by="departureDateTime" :search="search" 
						:loading="tableLoading" loading-text="Загрузка..." class="elevation-1 col-md-6">
			<template v-slot:top>
				<v-toolbar flat>
					<v-toolbar-title>Авиарейсы на {{ new Date().toLocaleDateString() }}</v-toolbar-title>
					<v-divider class="mx-4" inset vertical ></v-divider>
					<v-spacer></v-spacer>
					<v-text-field v-model="search" append-icon="mdi-magnify" label="Поиск" single-line 
								hide-details outlined dense></v-text-field>
					<v-spacer></v-spacer>
				</v-toolbar>
			</template>
		</v-data-table>
	</v-container>
</template>

<script>
export default {
    data: () => ({
		headers: [
			{ text: 'Город вылета', value: 'departureCity' },
			{ text: 'Город прилета', value: 'arrivalCity' },
			{ text: 'Время вылета', value: 'departureDateTime' },
			{ text: 'Время прилета', value: 'arrivalDateTime' },
			{ text: 'Задержка рейса', value: 'delayInfo' }
		],
		search: '',
		tableLoading: false
	}),
	// created lifecycle hook
	created() {
		// get flights every time component has been created
		this.tableLoading = true;
		this.$store.dispatch("getFlights")
			.catch(error => console.error(error))
			.finally(() => this.tableLoading = false);
	},
	computed: {
		flights() {
			let allFlights = this.$store.getters.getFlights;
			
			let todayFlights = allFlights.filter(flight => {
				return new Date(flight.departureDateTime).toLocaleDateString() == new Date().toLocaleDateString();
			});

			todayFlights.forEach(flight => {
				// convert from JSON type datetime to local time and remove seconds
				flight.departureDateTime = new Date(flight.departureDateTime).toLocaleTimeString().slice(0, -3);
				flight.arrivalDateTime = new Date(flight.arrivalDateTime).toLocaleTimeString().slice(0, -3);
			});
			return todayFlights;
		}
	}
}
</script>