<template>
    <div class="my-register-page">
        <b-overlay :show="loading">
            <div class="my-container">
                <div>
                    <b-row style="margin-top: 100px">
                        <b-col class="text-center d-flex justify-content-center">
                            <custom-button variant="success" style="margin-right: 12px; width: fit-content" @click.native="signModal = !signModal">
                                <span style="margin-left: 48px; margin-right: 48px">
                                    {{ $t('eImzo') }}
                                </span>
                            </custom-button>
                        </b-col>
                        <b-modal v-model="signModal" size="md" hide-footer hide-header>
                            <div style="text-align: right; margin-right: 10px; margin-top: -5px; margin-bottom: 5px; border-bottom: 1px solid lightgray">
                                <span @click="signModal = false" style="cursor: pointer; font-size: 30px"> &times; </span>
                            </div>
                            <div>
                                <just-sign @sign="loginESP($event)"></just-sign>
                            </div>
                        </b-modal>
                    </b-row>
                </div>
            </div>
        </b-overlay>
    </div>
</template>

<script>
import customButton from '../components/elements/customButton.vue';
import AccountService from '@/services/account.service';
import JustSign from '../components/justSign.vue';
export default {
    components: {
        customButton,
        JustSign
    },
    data() {
        return {
            lang: '',
            signModal: false,
            loading: false
        };
    },
    created() {
        this.lang = localStorage.getItem('locale') || 'uz_cyrl';
    },
    methods: {
        makeToast(message, type) {
            var a = '';
            if (message.status == 500) {
                a = message.title;
            }
            if (message.status == 400) {
                var errors = Object.values(message.errors);
                var a = errors.map((el, item) => item + 1 + '.' + el).join('\n');
            } else {
                a = message;
            }
            this.$toast.open({
                message: a,
                type: type,
                duration: 5000,
                dismissible: true
            });
        },

        loginESP(data) {
            var info = {};
            if (!!data.data.alias) {
                var arr = [];
                data.data.alias.split(',').forEach(function (item) {
                    arr.push(item.split('='));
                });
                const entries = new Map(arr);
                info = Object.fromEntries(entries);
            }
            var obj = {
                signedData: data.key,
                state: '',
                email: null,
                fullName: data.data.CN,
                inn: data.data.TIN,
                pinfl: info['1.2.860.3.16.1.2'],
                isPinfl: info['1.2.860.3.16.1.1'] ? false : true
            };
            this.signModal = false;

            AccountService.Registrate(obj)
                .then((res) => {
                    if (res.data) {
                        localStorage.setItem('user_info', JSON.stringify(res.data.user));
                        this.$router.push({ name: 'MyCabinet' });
                    }
                })
                .catch((error) => {
                    this.makeToast(error.response.data, 'error');
                });
        }
    }
};
</script>

<style></style>
