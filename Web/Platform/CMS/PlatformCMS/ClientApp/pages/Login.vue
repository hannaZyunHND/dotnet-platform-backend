<template>
    <div class="app flex-row align-items-center">
        <div class="container">
            <b-row class="justify-content-center">
                <b-col md="8">
                    <b-card-group>
                        <b-card no-body class="p-4">
                            <b-card-body>
                                <b-form @submit.prevent="DoLogin">
                                    <div class="language-select d-flex">
                                        <div>
                                            <img src="https://cdn.save.moe/b/6QziFKt7.png" alt="vietnam" border="0"
                                                style="width: 30px;" @click="onChangeLanguage('vi')">

                                        </div>
                                        <div>
                                            <img src="https://cdn.save.moe/b/3abfy0.png" alt="china" border="0"
                                                style="width: 30px;margin-left:10px;" @click="onChangeLanguage('zh')">
                                        </div>
                                        <div>
                                            <img src="https://cdn.save.moe/b/JAc5Rez.png" alt="united kingdom"
                                                border="0" style="width: 30px;margin-left:10px"
                                                @click="onChangeLanguage('en')">
                                        </div>
                                    </div>
                                    <h1>{{$t('loginTitle')}}</h1>
                                    <p class="text-muted">{{$t('loginSubtitle')}}</p>
                                    <b-input-group class="mb-3">


                                        <b-input-group-prepend>
                                            <b-input-group-text><i class="icon-user"></i></b-input-group-text>
                                        </b-input-group-prepend>
                                        <b-form-input type="text" v-model.trim="$v.username.$model" name="username"
                                            :placeholder="$t('emailPlaceholder')" class="form-control"
                                            :class="{ 'is-invalid': submitted && $v.username.$error }"
                                            autocomplete="username email" />
                                        <div v-if="submitted && !$v.username.required" class="invalid-feedback">
                                            {{$t('emailRequired')}}
                                        </div>
                                    </b-input-group>
                                    <b-input-group class="mb-4">
                                        <b-input-group-prepend>
                                            <b-input-group-text><i class="icon-lock"></i></b-input-group-text>
                                        </b-input-group-prepend>
                                        <b-form-input type="password" v-model.trim="$v.password.$model" name="password"
                                            class="form-control"
                                            :class="{ 'is-invalid': submitted && $v.password.$error }"
                                            :placeholder="$t('passwordPlaceholder')" autocomplete="current-password" />
                                        <div v-if="submitted && !$v.password.required" class="invalid-feedback">
                                            {{$t('passwordRequired')}}
                                        </div>
                                    </b-input-group>
                                    <b-row>
                                        <b-col cols="6">
                                            <b-button :disabled="loading" variant="primary" class="px-4"
                                                v-on:click="DoLogin">
                                                <span class="spinner-border spinner-border-sm" v-show="loading"></span>
                                                <span>{{$t('loginButton')}}</span>
                                            </b-button>
                                        </b-col>
                                    </b-row>
                                    <div v-if="error" class="alert alert-danger">{{ error }}</div>
                                </b-form>
                            </b-card-body>
                        </b-card>
                    </b-card-group>
                </b-col>
            </b-row>
        </div>
    </div>
</template>

<script>
import { authenticationRepository } from "../repository/authentication/authenticationRepository";
import { router } from "../router/index";
import { required } from 'vuelidate/lib/validators';
import VueJwtDecode from 'vue-jwt-decode'

export default {
    name: 'Login',
    data() {
        return {
            username: '',
            password: '',
            submitted: false,
            loading: false,
            returnUrl: '',
            error: ''
        };
    },
    validations: {
        username: { required },
        password: { required }
    },
    created() {
        // redirect to home if already logged in
        // console.log(authenticationRepository.currentUserValue);
        if (authenticationRepository.currentUserValue) {
            let tokenObj = VueJwtDecode.decode(authenticationRepository.currentUserValue.token);
            if (tokenObj) {
                let roles = tokenObj.Roles;
                console.log(roles);
                if (roles) {
                    if (roles.includes('Supplier')) {
                        this.setDefaultUrl = '/supplier/orders'
                    }
                }
            }
            return router.push(this.setDefaultUrl);
        }
        // get return url from route parameters or default to '/'
        this.returnUrl = this.$route.query.returnUrl || '/';
    },
    methods: {
        onChangeLanguage(lang) {
            this.$i18n.locale = lang; // Thay đổi ngôn ngữ trong i18n
            localStorage.setItem('language', lang); // Lưu ngôn ngữ vào localStorage
        },
        DoLogin() {
            this.submitted = true;
            // stop here if form is invalid
            this.$v.$touch();
            if (this.$v.$invalid) {
                return;
            }
            this.loading = true;
            authenticationRepository.login(this.username, this.password)
                .then(
                    user => {
                        //Dịch token ở đây
                        console.log(user)
                        let tokenObj = VueJwtDecode.decode(user.token);
                        if (tokenObj) {
                            let roles = tokenObj.Roles;
                            console.log(roles);
                            if (roles) {

                                if (roles.includes('Supplier')) {
                                    this.setDefaultUrl = '/supplier/orders'
                                }
                                else if (roles.includes('Coupon')) {
                                    this.setDefaultUrl = '/agency/coupon/order/list'
                                }
                                else {
                                    this.setDefaultUrl = '/admin/dashboard'
                                }
                            }
                        }
                        router.push(this.setDefaultUrl)
                    },
                    error => {
                        this.error = error;
                        this.loading = false;
                    }
                );
        },
        decodeToken(token) {
            try {
                console.log(token)
                return jwt_decode(token.trim());
            } catch (error) {
                alert('Invalid JWT token');
                console.error(error);
            }
        }
    }
}
</script>

