<template>
    <div>
        <b-container>
            <b-row class="mt-3">
                <b-col>
                    <label for=""> {{ $t('Username') }} </label>
                    <b-form-input v-model="login.username"></b-form-input>
                </b-col>
                <b-col> </b-col>
            </b-row>
            <b-row class="mt-3">
                <b-col> </b-col>
                <b-col>
                    <label for=""> {{ $t('Password') }} </label>
                    <b-form-input type="password" v-model="login.password"></b-form-input>
                </b-col>
                <b-col> </b-col>
            </b-row>
            <b-row class="mt-3">
                <b-col> </b-col>
                <b-col>
                    <b-button @click="Login" variant="primary" style="width: 100%"> {{ $t('Login') }} </b-button>
                </b-col>
                <b-col> </b-col>
            </b-row>
            <b-modal @ok="SignwithSmsCode" no-close-on-backdrop :title="$t('smskod')" v-model="TrustedDeviceModal" id="TrustedDeviceModal">
                <b-row>
                    <b-col>
                        <label> {{ $t('smskod') }} </label>
                        <b-form-input v-model="smscode"></b-form-input>
                    </b-col>
                </b-row>
            </b-modal>
        </b-container>
    </div>
</template>

<script>
import { BContainer, BRow, BCol, BFormInput, BButton, BModal } from 'bootstrap-vue';
import AccountService from '@/services/account.service';
export default {
    components: { BContainer, BRow, BCol, BFormInput, BButton, BModal },
    data() {
        return {
            login: {
                username: '',
                password: ''
            },
            TrustedDeviceModal: false,
            smscode: ''
        };
    },
    methods: {
        Login() {
            AccountService.SignIn(this.login).then((res) => {
                if (!res.data.trusteddevice) {
                    this.TrustedDeviceModal = true;
                }
            });
        },
        SignwithSmsCode() {
            AccountService.SignInTwoFactor({ smscode: this.smscode }).then((res) => {});
        }
    }
};
</script>

<style></style>
