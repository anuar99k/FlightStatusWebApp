<template>
    <v-container>
        <v-row justify="space-between">
            <div class="pa-3">
                <v-btn text to="/">
                    <span class="btnText">Главная</span>
                </v-btn>
                <v-btn text to="/admin">
                    <span class="btnText">Админ панель</span>
                </v-btn>
            </div>
            <div class="pa-3">
                <v-menu open-on-hover bottom offset-y nudge-left="20" v-if="!isLoggedIn">
                    <template v-slot:activator="{ on, attrs }">
                        <v-btn v-bind="attrs" v-on="on" text>
                            <span class="btnText">Вход на сайт</span>
                        </v-btn>
                    </template>
                    <v-list>
                        <v-list-item to="/login">
                            <span class="black--text subtitle-2">Войти</span>
                        </v-list-item>
                        <v-list-item to="/registration">
                            <span class="black--text subtitle-2">Зарегистрироваться</span>
                        </v-list-item>
                    </v-list>
                </v-menu>
                <v-btn @click="logout" text v-else>
                    <span class="btnText">Выйти</span>
                </v-btn>
            </div>
        </v-row>
    </v-container>
</template>

<script>
export default {
    data() {
        return {
            
        }
    },
    methods:{
        logout() {
            this.$store.dispatch('logout')
                .then(() => this.$router.push({ name: 'Home' }))
                .catch(error => console.log(error));
        },
    },
    computed: {
        isLoggedIn() {
            return this.$store.getters.isLoggedIn;
        }
    }
}
</script>

<style lang="scss">
.btnText {
    text-transform: none;
}
</style>