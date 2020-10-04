<template>
	<v-container>
		<v-data-table :headers="headers" :items="flights" sort-by="departureDateTime" :search="search" 
						:loading="tableLoading" loading-text="Загрузка..." class="elevation-1 col-md-6">
		<template v-slot:top>
			<v-toolbar flat>
				<v-toolbar-title>Все авиарейсы</v-toolbar-title>
				<v-divider class="mx-4" inset vertical ></v-divider>
				<v-spacer></v-spacer>
				<v-text-field v-model="search" append-icon="mdi-magnify" label="Поиск" single-line hide-details
								outlined dense></v-text-field>
				<v-spacer></v-spacer>
				<!-- adding and editing flights dialog -->
				<v-dialog v-model="dialog" max-width="500px" >
					<template v-slot:activator="{ on, attrs }">
						<v-btn color="primary" dark class="mb-2" v-bind="attrs" v-on="on"> Добавить рейс </v-btn>
					</template>
					<v-card>
					<v-form ref="flightsForm">
						<v-card-title>
							<span class="headline">{{ formTitle }}</span>
						</v-card-title>
						<v-card-text class="pb-0">
							<v-container class="pb-0">
								<v-row>
									<v-col cols="12" class="py-0">
										<v-text-field v-model="editedItem.departureCity" label="Город вылета" required
													:rules="notEmptyRule" outlined dense></v-text-field>
									</v-col>
									<v-col cols="12" class="py-0">
										<v-text-field v-model="editedItem.arrivalCity" label="Город прилета" required
													:rules="notEmptyRule" outlined dense></v-text-field>
									</v-col>
									<v-col cols="12" class="py-0">
										<v-menu v-model="departureDateMenu" :close-on-content-click="false"
												transition="scale-transition" max-width="290px" min-width="290px">
											<template v-slot:activator="{ on }">
												<v-text-field label="Дата вылета" readonly :value="departureDateDisplay" v-on="on" 
															outlined dense required :rules="notEmptyRule"></v-text-field>
											</template>
											<v-date-picker v-model="departureDate" @input="departureDateMenu = false" color="blue"
														:min="new Date().toISOString().substring(0,10)" no-title first-day-of-week="1"></v-date-picker>
										</v-menu>
									</v-col>
									<v-col cols="12" class="py-0">
										<v-text-field v-model="departureTime" label="Время вылета" required outlined dense
													:rules="timeRules" validate-on-blur placeholder="Введите время в формате HH:MM"></v-text-field>
									</v-col>
									<v-col cols="12" class="py-0">
										<v-menu v-model="arrivalDateMenu" :close-on-content-click="false"
												transition="scale-transition" max-width="290px" min-width="290px">
											<template v-slot:activator="{ on }">
												<v-text-field label="Дата прилета" readonly :value="arrivalDateDisplay" v-on="on" 
															outlined dense required :rules="notEmptyRule"></v-text-field>
											</template>
											<v-date-picker v-model="arrivalDate" @input="arrivalDateMenu = false" color="blue"
														:min="new Date().toISOString().substring(0,10)" no-title first-day-of-week="1"></v-date-picker>
										</v-menu>
									</v-col>
									<v-col cols="12" class="py-0">
										<v-text-field v-model="arrivalTime" label="Время прилета" required
													:rules="timeRules" validate-on-blur placeholder="Введите время в формате HH:MM" outlined dense></v-text-field>
									</v-col>
									<v-col cols="12" class="py-0">
										<v-text-field v-model="editedItem.delayInfo" label="Информация о задержке рейса" outlined dense></v-text-field>
									</v-col>
								</v-row>
							</v-container>
						</v-card-text>
						<v-card-actions class="pb-5 pr-7">
							<v-spacer></v-spacer>
							<v-btn color="primary" dark @click="close"> Отмена </v-btn>
							<v-btn color="primary" dark @click="save" :loading="saveBtnLoading">
								<span v-show="editedIndex > -1">Сохранить</span>
								<span v-show="editedIndex == -1">Добавить</span>
							</v-btn>
						</v-card-actions>
					</v-form>
					</v-card>
				</v-dialog>
				<v-dialog v-model="dialogDelete" max-width="500px">
					<v-card>
						<v-card-title class="grey lighten-2">Подтвердите удаление данных об авиарейсе</v-card-title>
						<div class="px-7 pt-4">
							<p>Город вылета: {{ editedItem.departureCity }}</p>
							<p>Город прилета: {{ editedItem.arrivalCity }}</p>
							<p>Дата и время вылета: {{ editedItem.departureDateTime }}</p>
							<p>Дата и время прилета: {{ editedItem.arrivalDateTime }}</p>
							<p v-show="editedItem.delayInfo != ''">Задержка рейса: {{ editedItem.delayInfo }}</p>
						</div>
						<v-card-actions class="pb-5">
							<v-spacer></v-spacer>
							<v-btn color="primary" dark @click="closeDelete">Отмена</v-btn>
							<v-btn color="error" dark @click="deleteFlightConfirm" :loading="deleteConfirmBtnLoading">Подтвердить</v-btn>
							<v-spacer></v-spacer>
						</v-card-actions>
					</v-card>
				</v-dialog>
			</v-toolbar>
		</template>
		<!-- edit and delete buttons -->
		<template v-slot:item.actions="{ item }">
			<v-icon small class="mr-2" @click="editItem(item)"> mdi-pencil </v-icon>
			<v-icon small @click="deleteItem(item)"> mdi-delete </v-icon>
		</template>
	</v-data-table>
	</v-container>
</template>

<script>
import moment from 'moment' // moment js library to work with dates
export default {
    data: () => ({
		dialog: false,
		dialogDelete: false,
		headers: [
			{ text: 'Город вылета', value: 'departureCity' },
			{ text: 'Город прилета', value: 'arrivalCity' },
			{ text: 'Дата и время вылета', value: 'departureDateTime' },
			{ text: 'Дата и время прилета', value: 'arrivalDateTime' },
			{ text: 'Задержка рейса', value: 'delayInfo' },
			{ text: 'Действие', value: 'actions', sortable: false },
		],
		editedIndex: -1,
		editedItem: {
			departureCity: '',
			arrivalCity: '',
			departureDateTime: '',
			arrivalDateTime: '',
			delayInfo: ''
		},
		defaultItem: {
			departureCity: '',
			arrivalCity: '',
			departureDateTime: '',
			arrivalDateTime: '',
			delayInfo: ''
		},
		departureDate: '',
		departureTime: '',
		arrivalDate: '',
		arrivalTime: '',
		search: '',
		tableLoading: false,
		saveBtnLoading: false,
		deleteConfirmBtnLoading: false,
		notEmptyRule: [
			v => !!v || 'Обязательное поле'
		],
		timeRules: [
			v => !!v || 'Обязательное поле',
			v => /^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$/.test(v) || 'Некорректное время'
		],
		departureDateMenu: false,
		arrivalDateMenu: false
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
			let result = this.$store.getters.getFlights;
			result.forEach(flight => {
				flight.departureDateTime = new Date(flight.departureDateTime).toLocaleString().slice(0, -3); //remove seconds
				flight.arrivalDateTime = new Date(flight.arrivalDateTime).toLocaleString().slice(0, -3); //remove seconds
			});
			return result;
		},
		formTitle () {
			return this.editedIndex === -1 ? 'Добавление нового рейса' : 'Редактирование рейса'
		},
		departureDateDisplay() {
			return this.departureDate ? new Date(this.departureDate).toLocaleDateString() : '';
		},
		arrivalDateDisplay() {
			return this.arrivalDate ? new Date(this.arrivalDate).toLocaleDateString() : '';
		}
	},
	methods: {
		editItem (item) {
			this.editedIndex = this.flights.indexOf(item);
			this.editedItem = Object.assign({}, item);
			this.departureDate = this.editedItem.departureDateTime.split(', ')[0].split('.').reverse().join('-');
			this.departureTime = this.editedItem.departureDateTime.split(', ')[1];
			this.arrivalDate = this.editedItem.arrivalDateTime.split(', ')[0].split('.').reverse().join('-');
			this.arrivalTime = this.editedItem.arrivalDateTime.split(', ')[1];
			this.dialog = true
		},
		deleteItem (item) {
			this.editedIndex = this.flights.indexOf(item);
			this.editedItem = Object.assign({}, item);
			this.dialogDelete = true;
		},
		deleteFlightConfirm () {
			this.deleteConfirmBtnLoading = true;
			this.$store.dispatch('deleteFlight', this.editedItem.flightId)
				.then(() => this.closeDelete())
				.catch(err => console.error(err))
				.finally(() => this.deleteConfirmBtnLoading = false);
		},
		save () {
			if (this.$refs.flightsForm.validate()) {
				this.saveBtnLoading = true;

				let departureTimeHM = { 
					hours: this.departureTime.split(':')[0], 
					minutes: this.departureTime.split(':')[1] 
				};
				let arrivalTimeHM = {
					hours: this.arrivalTime.split(':')[0], 
					minutes: this.arrivalTime.split(':')[1] 
				};
				
				this.editedItem.departureDateTime = new Date(this.departureDate);
				this.editedItem.arrivalDateTime = new Date(this.arrivalDate);
				
				this.editedItem.departureDateTime.setHours(departureTimeHM.hours);
				this.editedItem.departureDateTime.setMinutes(departureTimeHM.minutes);
				this.editedItem.arrivalDateTime.setHours(arrivalTimeHM.hours);
				this.editedItem.arrivalDateTime.setMinutes(arrivalTimeHM.minutes);

				this.editedItem.departureDateTime = moment(this.editedItem.departureDateTime).format();
				this.editedItem.arrivalDateTime = moment(this.editedItem.arrivalDateTime).format();
				
				if (this.editedIndex > -1) {
					this.$store.dispatch('updateFlight', { id: this.editedItem.flightId, flight: this.editedItem })
						.then(() => Object.assign(this.flights[this.editedIndex], this.editedItem))
						.catch(error => console.error(error))
						.finally(() => {
							this.saveBtnLoading = false;
							this.close();
						});
				} else {
					this.$store.dispatch('addFlight', this.editedItem)
						.catch(error => console.error(error))
						.finally(() => {
							this.saveBtnLoading = false;
							this.close();
						});
				}
			}
		},
		close () {
			this.dialog = false
			this.$nextTick(() => {
				this.editedItem = Object.assign({}, this.defaultItem);
				this.editedIndex = -1;
				this.departureDate = '';
				this.departureTime = '';
				this.arrivalDate = '';
				this.arrivalTime = '';
			});
		},
		closeDelete () {
			this.dialogDelete = false;
			this.$nextTick(() => {
				this.editedItem = Object.assign({}, this.defaultItem);
				this.editedIndex = -1;
			})
		}
	},
	watch: {
		dialog (val) {
			val || this.close()
		},
		dialogDelete (val) {
			val || this.closeDelete()
		}
	},
}
</script>