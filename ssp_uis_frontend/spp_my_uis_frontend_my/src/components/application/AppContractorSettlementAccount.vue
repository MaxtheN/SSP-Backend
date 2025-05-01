<template>
    <div class="d-flex align-items-center">
        <WSelect
            :label="$t('accountCode')"
            class="w-100"
            :name="$t('accountCode')"
            :options="accountList"
            :loading="getLoading"
            v-model="valueComp"
            v-bind="$attrs"
            valuename="accountCode"
            valueid="id"
        >
            <template #option="option"> {{ option.accountCode }} - {{ option.bank }} </template>
            <template #selected-option="{ accountCode, bank }"> {{ accountCode }} - {{ bank }} </template>
        </WSelect>

        <span id="disabled-wrapper" class="d-inline-block" tabindex="0">
            <b-button @click="ChangeMainSettlementAccount" :disabled="!valueComp || setMainLoading" variant="light" class="text-success py-3 mt-2">
                <b-spinner v-if="setMainLoading" small color="primary" />
                <BIconCheck2Circle v-else style="width: 25px; height: 20px" />
            </b-button>
        </span>
        <b-tooltip target="disabled-wrapper">{{ $t('ChangeMainSettlementAccount') }}</b-tooltip>
    </div>
</template>

<script>
import WSelect from '@/components/forms/WSelect.vue';
import AccountService from '@/services/account.service';
import { BIconCheck2Circle } from 'bootstrap-vue';

export default {
    components: {
        WSelect,
        BIconCheck2Circle
    },
    props: {
        value: {
            type: Number,
            default: null
        }
    },
    emits: ['input'],
    data() {
        return {
            setMainLoading: false,
            getLoading: false,
            contractor: null,
            accountList: []
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
        this.GetContractorSettlementAccountList();
    },
    methods: {
        GetContractorSettlementAccountList() {
            this.getLoading = true;
            AccountService.GetContractorSettlementAccountList()
                .then((res) => {
                    this.accountList = res.data;
                })
                .finally(() => {
                    this.getLoading = false;
                });
        },
        ChangeMainSettlementAccount() {
            this.setMainLoading = true;
            AccountService.ChangeMainSettlementAccount({
                contractorId: JSON.parse(localStorage.getItem('user_info'))?.contractorId,
                settlementAccountId: this.valueComp
            })
                .then((res) => {
                    this.makeToast(this.$t('SaveSuccess'), 'success');
                })
                .catch((error) => {
                    this.showApiError(error);
                })
                .finally(() => {
                    this.setMainLoading = false;
                });
        }
    }
};
</script>
