<template>
    <b-modal v-model="valueComp" hide-footer hide-header no-close-on-backdrop size="lg">
        <div style="text-align: right; margin-right: 10px; margin-top: -5px; margin-bottom: 5px; border-bottom: 1px solid lightgray">
            <span @click="$emit('input', false)" style="cursor: pointer; font-size: 30px"> &times; </span>
        </div>
        <div>
            <WSelect :options="Organizations" v-model="contractorId" :label="$t('contractor')" valueid="id" valuename="fullName" :loading="getLoading">
                <template #option="{ fullName, director, pinfl }">
                    <b>{{ fullName }}</b> <template v-if="pinfl">- {{ director }}</template>
                </template>
                <template #selected-option="{ fullName, director, pinfl }">
                    <span>
                        <b>{{ fullName }}</b>
                        <template v-if="pinfl">- {{ director }}</template>
                    </span>
                </template>
            </WSelect>
        </div>

        <b-button :disabled="saveLoading || !contractorId" @click="SetOrganization" variant="primary" class="w-100 mt-4">
            {{ $t('login') }}
        </b-button>
    </b-modal>
</template>

<script>
import AccountService from '@/services/account.service';
import WSelect from '@/components/forms/WSelect.vue';

export default {
    components: {
        WSelect
    },
    props: {
        value: {
            type: Boolean,
            default: false
        }
    },
    emits: ['input'],
    data() {
        return {
            Organizations: [],
            contractorId: null,
            businessmanUserId: null,
            saveLoading: false,
            getLoading: false
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
    created() {
        this.GetOrganizations();
    },
    methods: {
        async GetOrganizations() {
            try {
                this.getLoading = true;
                const user_id = JSON.parse(this.$store.state.user_info)?.id;
                this.businessmanUserId = user_id || localStorage.getItem('businessmanUserId');
                await AccountService.GetOrganizations(this.businessmanUserId)
                    .then((res) => {
                        this.Organizations = res.data;
                    })
                    .catch((error) => {
                        this.showApiError(error);
                    });
            } catch (e) {
                console.log(e);
            } finally {
                this.getLoading = false;
            }
        },
        SetOrganization() {
            this.saveLoading = true;
            AccountService.SetOrganization({
                businessmanUserId: this.businessmanUserId,
                contractorId: this.contractorId
            })
                .then((res) => {
                    this.Organizations = res.data;
                    if (res.data.user) {
                        localStorage.setItem('user_info', JSON.stringify(res.data.user));
                        this.$store.commit('setUserInfo', JSON.stringify(res.data.user));
                    }
                    this.valueComp = false;
                    this.$router.push({ name: 'MyCabinet' });
                })
                .catch((error) => {
                    this.showApiError(error);
                })
                .finally(() => {
                    this.saveLoading = false;
                });
        }
    }
};
</script>
