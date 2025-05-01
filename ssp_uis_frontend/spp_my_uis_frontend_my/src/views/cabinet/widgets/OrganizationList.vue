<template>
    <b-row>
        <b-col class="pb-2 d-flex justify-content-between align-items-center">
            <h5>{{ $t('myOrganizations') }}</h5>
            <b-button variant="light" v-b-tooltip.hover :title="$t('AddNewOrganization')" size="sm" class="border-1" @click="addorg = true">
                <b-icon-plus scale="1.3" variant="info"></b-icon-plus>
            </b-button>
        </b-col>
        <b-col sm="12" lg="12" class="mt-4">
            <b-table :items="Organizations" :fields="fields" responsive bordered class="w-100">
                <template #cell(fullName)="{ item }">
                    <p>{{ item.fullName }}</p>
                    <template v-if="item.pinfl"> - {{ item.director }}</template>
                </template>
                <template #cell(id)="{ item, index }">
                    {{ index + 1 }}
                </template>
                <template #cell(inn)="{ item }"> {{ item.inn || item.pinfl }}</template>
                <template #cell(actions)="{ item }">
                    <div>
                        <b-link @click="DeleteItem(item)">
                            <b-icon-trash-fill style="width: 20px; height: 20px" class="text-danger"></b-icon-trash-fill>
                        </b-link>
                    </div>
                </template>
            </b-table>
        </b-col>

        <b-modal v-model="smsModal" hide-footer :title="$t('entersmscode')" no-close-on-backdrop centered>
            <b-row class="mt-3">
                <b-col>
                    <CustomInput v-model="data.smscode" :placeholder="$t('0000')" :label="$t('smskod')"></CustomInput>
                </b-col>
            </b-row>
            <b-row class="mt-3">
                <b-col>
                    <b-button v-if="data.isRestore" @click="DeactivateAssociation" block variant="success">
                        <b-spinner v-if="saveLoading" small style="margin-right: 8px"></b-spinner>
                        {{ $t('confirm') }}
                    </b-button>
                </b-col>
            </b-row>
        </b-modal>

        <!-- add neworganisation -->

        <b-modal v-model="addorg" :title="$t('addacontacts')">
            <b-row>
                <b-col sm="12" md="9">
                    <WInput v-model="filter.companyInn" rules="required" :label="$t('inn')" :placeholder="$t('inn')" name="inn" @keyup.enter="GetFromSoliq" />
                </b-col>
                <b-col md="3">
                    <b-button style="margin-top: 30px; padding: 12px 20px" @click="GetFromSoliq" :disabled="SoliqLoading" variant="info" size="lg">
                        <b-icon icon="search" v-if="!SoliqLoading"></b-icon>
                        <b-spinner v-if="SoliqLoading" small></b-spinner>
                    </b-button>
                </b-col>
            </b-row>
            <b-row class="mt-4" v-if="companyData.innOrPinfl">
                <b-col sm="12">
                    <custom-label :content="companyData.innOrPinfl" :label="$t('inn')"></custom-label>
                </b-col>
                <b-col sm="12" class="my-3">
                    <custom-label :content="companyData.name" :label="$t('contractor')"></custom-label>
                </b-col>
            </b-row>
            <template #modal-footer>
                <div class="d-flex justify-content-between w-100">
                    <div class="d-flex justify-content-between w-100">
                        <b-button variant="light" @click="closeModal" class="border-secondary px-5">
                            <span style="color: #000">{{ $t('back') }}</span>
                        </b-button>
                        <b-button variant="info" class="px-5" :disabled="SaveLoading || !companyData.innOrPinfl" @click="signModal = true">
                            <span>{{ $t('add') }}</span>
                        </b-button>
                    </div>
                </div>
            </template>
        </b-modal>

        <div class="p-0 m-0">
            <b-modal v-model="signModal" :title="$t('ESPmodal')" no-close-on-backdrop hide-footer>
                <just-sign @sign="AddNewOrganization($event)"></just-sign>
            </b-modal>

            <SetOrganizationModal v-if="setOrganizationModal" v-model="setOrganizationModal" />
        </div>
    </b-row>
</template>

<script>
import AccountService from '@/services/account.service';
import CustomInput from '@/components/elements/customInput.vue';
import { BIconPlus } from 'bootstrap-vue';
import WInput from '@/components/forms/WInput.vue';
import AppListHeader from '@/components/application/AppListHeader.vue';
import customLabel from '@/components/elements/customLabel.vue';
import eimzoMixin from '@/mixins/eimzo';
import JustSign from '@/components/justSign.vue';
import SetOrganizationModal from '@/views/account/widgets/SetOrganizationModal.vue';

export default {
    data() {
        return {
            addorg: false,
            Organizations: [],
            smsModal: false,
            getLoading: false,
            saveLoading: false,
            data: {
                username: '',
                isRestore: false,
                smscode: ''
            },
            SoliqLoading: false,
            SaveLoading: false,
            signModal: false,
            setOrganizationModal: false,
            filter: {
                companyInn: ''
            },
            companyData: {},
            selectedItem: null,
            fields: [
                {
                    key: 'id',
                    label: this.$t('id')
                },
                {
                    key: 'fullName',
                    label: this.$t('fullname')
                },
                {
                    key: 'inn',
                    label: this.$t('inn')
                },
                {
                    key: 'actions',
                    tdClass: 'text-center',
                    thClass: 'text-center',
                    label: this.$t('actions')
                }
            ]
        };
    },
    created() {
        this.GetOrganizations();
    },
    methods: {
        closeModal() {
            this.addorg = false;
            this.companyData = {};
            this.filter.companyInn = '';
        },
        DeleteItem(item) {
            this.selectedItem = item;
            this.smsModal = true;
            this.RestorePassword();
        },
        async GetOrganizations() {
            try {
                this.getLoading = true;
                const user_id = JSON.parse(this.$store.state.user_info)?.id;
                console.log(this.$store.state);
                this.data.username = JSON.parse(this.$store.state.user_info || localStorage.getItem('user_info'))?.userName;
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
        DeactivateAssociation() {
            this.saveLoading = true;
            AccountService.DeactivateAssociation({
                ...this.data,
                contractorId: this.selectedItem?.id
            })
                .then(() => {
                    this.selectedItem = null;
                    this.data.smscode = null;
                    this.smsModal = false;
                    this.makeToast(this.$t('DeleteSuccess'), 'success');
                    this.GetOrganizations();
                })
                .catch((error) => {
                    this.showApiError(error);
                })
                .finally(() => {
                    this.saveLoading = false;
                });
        },
        RestorePassword() {
            console.log(this.data);
            AccountService.RestorePassword({
                userName: this.data.username,
                appKeyHash: ''
            })
                .then((res) => {
                    this.data.isRestore = true;
                })
                .catch((error) => {
                    this.showApiError(error);
                });
        },
        AddNewOrganization(data) {
            this.SaveLoading = true;
            const isPinfl = this.isPinfl(data);
            AccountService.AddNewOrganization({
                inn: isPinfl ? null : this.companyData.innOrPinfl,
                pinfl: isPinfl ? this.companyData.innOrPinfl : null,
                signedData: data.key,
                isPinfl: isPinfl
            })
                .then(() => {
                    this.makeToast(this.$t('SaveSuccess'), 'success');
                    // this.setOrganizationModal = true;
                    this.addorg = false;
                    this.signModal = false;
                    this.$router.push({ name: 'info' });
                })
                .catch((error) => {
                    this.showApiError(error);
                })
                .finally(() => {
                    this.SaveLoading = false;
                    this.signModal = false;
                });
        },
        GetFromSoliq() {
            const length = this.filter.companyInn.length;
            if (length == 9 || length == 14) {
                this.SoliqLoading = true;
                this.companyData = {};
                if (length == 9) {
                    AccountService.GetFromSoliq(this.filter.companyInn)
                        .then((res) => {
                            this.companyData.innOrPinfl = res.data.company.tin;
                            this.companyData.name = res.data.company.shortName;
                        })
                        .catch((error) => {
                            this.showApiError(error);
                        })
                        .finally(() => {
                            this.SoliqLoading = false;
                        });
                } else {
                    AccountService.GetFromSoliqByPinfl(this.filter.companyInn)
                        .then((res) => {
                            this.companyData.innOrPinfl = this.filter.companyInn;
                            this.companyData.name = res.data.fullName.uz;
                        })
                        .catch((error) => {
                            this.showApiError(error);
                        })
                        .finally(() => {
                            this.SoliqLoading = false;
                        });
                }
            }
        }
    },
    mixins: [eimzoMixin],
    components: {
        CustomInput,
        BIconPlus,
        WInput,
        AppListHeader,
        JustSign,
        customLabel,
        SetOrganizationModal
    }
};
</script>
