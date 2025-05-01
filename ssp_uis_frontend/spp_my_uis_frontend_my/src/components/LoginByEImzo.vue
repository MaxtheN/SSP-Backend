<template>
    <b-modal v-model="valueComp" size="md" hide-footer hide-header>
        <div style="text-align: right; margin-right: 10px; margin-top: -5px; margin-bottom: 5px; border-bottom: 1px solid lightgray">
            <span @click="valueComp = false" style="cursor: pointer; font-size: 30px"> &times; </span>
        </div>
        <div>
            <just-sign @sign="loginESP($event)"></just-sign>
        </div>
    </b-modal>
</template>

<script>
import AccountService from '@/services/account.service';
import JustSign from '@/components/justSign.vue';

export default {
    components: {
        JustSign
    },
    props: {
        value: {
            type: Boolean,
            default: null
        }
    },
    emits: ['input'],
    data() {
        return {
            loading: false
        };
    },
    computed: {
        valueComp: {
            get: function () {
                return this.value;
            },
            set: function (v) {
                this.$emit('input', v);
            }
        }
    },
    methods: {
        loginESP(item) {
            AccountService.LoginByEImzo({
                signData: item.key
            })
                .then((res) => {
                    localStorage.setItem('businessmanUserId', res.data.businessmanUserId);
                    localStorage.setItem('user_info', JSON.stringify(res.data.user));
                    this.$router.replace({ name: 'MyCabinet' });
                    this.valueComp = false;
                })
                .catch((error) => {
                    this.showApiError(error);
                });
        }
    }
};
</script>
