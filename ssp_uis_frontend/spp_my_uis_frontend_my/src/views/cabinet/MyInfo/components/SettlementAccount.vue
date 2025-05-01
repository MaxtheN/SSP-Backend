<template>
    <div>
        <b-card class="bg-white">
            <div class="border-bottom pb-2 d-flex justify-content-between align-items-center">
                <h6>{{ $t('accountCode') }}</h6>
                <b-button variant="light" size="sm" class="border-1" @click="addaccountCode">
                    <b-icon-plus scale="1.3" variant="info"></b-icon-plus>
                </b-button>
            </div>
            <div class="mt-4">
                <b-table :items="items" :fields="fields" responsive bordered>
                    <template #cell(id)="{ item, index }">
                        {{ index + 1 }}
                    </template>
                    <template #cell(isMain)="{ item, index }">
                        <div @click="ChangeCheckBox(item, index)" style="cursor: pointer">
                            <img v-if="item.isMain" src="@/assets/images/check.svg" alt="check" />

                            <img v-else src="@/assets/images/square.svg" alt="square" />
                        </div>
                    </template>

                    <template #cell(actions)="{ item, index }">
                        <div>
                            <b-link @click="Delete(item, index)" class="mr-3">
                                <b-icon-trash-fill style="width: 20px; height: 20px" variant="danger"></b-icon-trash-fill>
                            </b-link>

                            <b-link @click="EditItem(item)" style="margin-right: 5px">
                                <b-icon-pencil-square style="width: 20px; height: 20px" variant="info"></b-icon-pencil-square>
                            </b-link>
                        </div>
                    </template>
                </b-table>
            </div>
            <b-modal v-model="accountCodeModal" :title="$t('addaccount')">
                <b-row>
                    <b-col cols="12">
                        <custom-select required :label="$t('bank')" :clearable="true" :valueid="'value'" :valuename="'text'" :options="BankList" v-model="tabrow.bankId" />
                    </b-col>
                    <b-col cols="12" class="mt-3">
                        <custom-input required v-mask="'####################'" :label="$t('accountCode')" v-model="tabrow.accountCode" @input="ChangeCodee"></custom-input>
                    </b-col>
                    <b-col cols="12" class="mt-3">
                        <custom-select required :label="$t('status')" :clearable="true" :valueid="'value'" :valuename="'text'" :options="StateList" v-model="tabrow.stateId" />
                    </b-col>
                </b-row>
                <template #modal-footer>
                    <div class="d-flex justify-content-between w-100">
                        <b-button variant="light" @click="accountCodeModal = false" class="border-secondary px-5">
                            <span style="color: #000">{{ $t('back') }}</span>
                        </b-button>
                        <b-button variant="info" class="px-5" @click="addTabrow">
                            <span>{{ $t('add') }}</span>
                        </b-button>
                    </div>
                </template>
            </b-modal>
        </b-card>
    </div>
</template>

<script>
import ManualService from '@/services/manual.service';
import CustomSelect from '@/components/elements/customSelect.vue';
import CustomInput from '@/components/elements/customInput.vue';
import ContractorSettlementAccountService from '@/services/myinfo/contractorsettlementaccount.service';

export default {
    components: {
        CustomSelect,
        CustomInput
    },
    data() {
        return {
            items: [],
            accountCodeModal: false,
            BankList: [],
            StateList: [],
            fields: [
                {
                    key: 'id',
                    label: this.$t('id'),
                    sortable: false,
                    tdClass: 'text-center',
                    thClass: 'text-center'
                },
                {
                    key: 'isMain',
                    label: this.$t('isMain'),
                    sortable: false,
                    tdClass: 'text-center',
                    thClass: 'text-center'
                },
                {
                    key: 'bank',
                    label: this.$t('bank'),
                    sortable: false,
                    tdClass: 'text-left',
                    thClass: 'text-center'
                },
                {
                    key: 'accountCode',
                    label: this.$t('accountCode'),
                    sortable: false,
                    tdClass: 'text-center',
                    thClass: 'text-center'
                },

                {
                    key: 'state',
                    label: this.$t('status'),
                    sortable: false,
                    tdClass: 'text-center',
                    thClass: 'text-center'
                },
                {
                    key: 'actions',
                    tdClass: 'text-center',
                    thClass: 'text-center',
                    label: this.$t('actions')
                }
            ],
            tabrow: {
                id: 0,
                accountName: '',
                accountCode: '',
                bankId: 0,
                bank: '',
                stateId: 0,
                state: '',
                isMain: false
            },
            ownerId: JSON.parse(localStorage.getItem('user_info')).contractorId
        };
    },
    created() {
        this.Refresh();
        ManualService.BankSelectList().then((res) => {
            this.BankList = res.data;
        });
        ManualService.StateSelectList().then((res) => {
            this.StateList = res.data;
        });
    },
    methods: {
        ChangeCodee(item) {
            this.tabrow.accountName = item;
        },
        Delete(item) {
            ContractorSettlementAccountService.Delete(item.id)
                .then((res) => this.makeToast(this.$t('DeleteSuccess'), 'success'))
                .catch((error) => {
                    this.showApiError(error);
                })
                .finally(() => {
                    this.Refresh();
                });
        },
        addaccountCode() {
            this.accountCodeModal = true;
            ContractorSettlementAccountService.Get(0).then((res) => {
                this.tabrow = res.data;
            });
        },
        closePhoneModal() {
            this.accountCodeModal = false;
        },
        checkTabrow() {
            if (this.tabrow.bankId === null || this.tabrow.bankId === undefined || this.tabrow.bankId === 0 || this.tabrow.bankId === '') {
                this.makeToast(this.$t('bankNotSelected'), 'error');
                return false;
            }
            if (this.tabrow.stateId === null || this.tabrow.stateId === undefined || this.tabrow.stateId === 0 || this.tabrow.stateId === '') {
                this.makeToast(this.$t('stateNotSelected'), 'error');
                return false;
            }
            if (this.tabrow.accountCode === null || this.tabrow.accountCode === undefined || this.tabrow.accountCode === 0 || this.tabrow.accountCode === '') {
                this.makeToast(this.$t('AccountCodeNotSelected'), 'error');
                return false;
            }

            if (this.tabrow.accountCode.length != 20) {
                this.makeToast(this.$t('AccountCode20Lower'), 'error');
                return false;
            }
            return true;
        },
        addTabrow() {
            if (!this.checkTabrow()) {
                return false;
            }
            ContractorSettlementAccountService.Update(this.tabrow)
                .then((res) => {
                    this.makeToast(this.$t('SaveSuccess'), 'success');
                })
                .catch((error) => {
                    this.saveLoading = false;
                    this.showApiError(error);
                })
                .finally(() => {
                    this.saveLoading = false;
                    this.accountCodeModal = false;
                    this.Refresh();
                });
        },
        Refresh() {
            ContractorSettlementAccountService.GetList().then((res) => {
                this.items = res.data;
            });
        },
        EditItem(item) {
            this.accountCodeModal = true;
            ContractorSettlementAccountService.Get(item.id).then((res) => {
                this.tabrow = res.data;
            });
        },
        ChangeCheckBox(item, index) {
            ContractorSettlementAccountService.SetMain(item.id)
                .then((res) => this.makeToast(this.$t('RefreshSuccess'), 'success'))
                .catch((error) => {
                    this.showApiError(error);
                })
                .finally(() => {
                    this.Refresh();
                });

            // this.items.forEach((data, idx) => {
            //     if (index == idx) {
            //         data.isMain = true;
            //     } else {
            //         data.isMain = false;
            //     }
            // });
        }
    }
};
</script>

<style lang="scss" scoped></style>
