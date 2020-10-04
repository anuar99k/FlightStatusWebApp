<template>
    <v-container>
        <v-row justify="center">
            <v-col lg="4" md="6" sm="8" cols="12">
            <v-card :loading="loading" class="pa-3">
                <div class="pt-3">
                    <h3 class="text-center headline">Авторизация</h3>
                </div>
                <v-form ref="form">
                    <v-container>
                        <v-row>
                            <v-col cols="12">
                                <v-text-field v-model="username" label="Логин" :rules="loginRules" required
                                    dense outlined flat color="black" @keyup.enter="submit" validate-on-blur/>

                                <v-text-field v-model="password" label="Пароль" :append-icon="showPassword ? 'mdi-eye' : 'mdi-eye-off'" 
                                    :type="showPassword ? 'text' : 'password'" @click:append="showPassword = !showPassword" :rules="passwordRules" 
                                    required dense outlined flat color="black" @keyup.enter="submit" validate-on-blur/>
                            </v-col>
                        </v-row>
                        <v-row justify="center">
                            <v-btn @click="submit" :loading="loading" color="primary">
                                <span class="signInBtn">Войти</span>
                            </v-btn>
                        </v-row>
                        <v-row class="mt-3 text-center">
                            <p v-for="(errorMsg, index) in this.errorMessages" :key="index" class="errorMessage">{{ errorMsg }}</p>
                        </v-row>
                    </v-container>
                </v-form>
            </v-card>
            </v-col>
        </v-row>
    </v-container>
</template>

<script>
export default {
    data() {
        return {
            username: "",
            password: "",

            showPassword: false,
            loginRules: [
                v => !!v || 'Введите логин',
                v => /^([a-zA-Z][a-zA-Z0-9-_]{1,40})$|^([\w-]+@([\w-]+\.)+[\w-]{2,4})$/.test(v) || 'Некорректный логин'
            ],
            passwordRules:[
                v => !!v || 'Введите пароль',
                v => /^[a-zA-Z0-9]+$/.test(v) || 'Пароль должен состоять только из латинских букв и цифр',
                v => v.length > 4 || 'Пароль должен быть не менее 5 символов'
            ],
            loading: false,
            errorMessages: []
        }
    },
    methods: {
        submit(){
            if (this.$refs.form.validate()) {
                this.errorMessages = [];
                this.loading = true;
                let data = { username: this.username, password: this.password };

                this.$store.dispatch('login', data)
                    .then(() => this.$router.push({ name: 'Admin' }))
                    .catch(error => {
                        this.errorMessages.push(error.data.message);
                        this.errorMessages = this.errorMessages.concat(error.data.errors);
                    })
                    .finally(() => this.loading = false);
            }
        }
    }
}
</script>

<style lang="scss">
.signInBtn{
    text-transform: none;
    font-size: 16px;
    font-weight: 400;
}
.errorMessage{
    color: red;
    font-size: 14px;
    margin-bottom: 0px !important;
    width: 100%;
}
</style>