<template>
    <div class="myWrapper">
        <AppListHeaderForName title="personaldata" class="mb-0" />
        <b-tabs class="tabsTab" v-model="tabIndex" vertical nav-wrapper-class="profile-tabs" pills>
            <b-tab>
                <template #title>
                    <Navlink icon="person" :label="$t('personaldata')" />
                </template>

                <b-card class="bg-white">
                    <div class="border-bottom pb-1">
                        <h6>{{ $t('personaldata') }}</h6>
                    </div>

                    <b-row class="mt-4">
                        <b-col sm="12" md="4">
                            <WInput :label="$t('inn')" disabled v-model="Parent.inn" />
                        </b-col>
                        <b-col sm="12" md="4">
                            <WInput :label="$t('registrationnumber')" disabled v-model="Parent.registrationNumber" />
                        </b-col>
                        <b-col sm="12" md="4">
                            <WInput :label="$t('registrationDate')" disabled v-model="Parent.registrationDate" />
                        </b-col>
                    </b-row>
                    <b-row style="margin-top: 16px">
                        <b-col sm="12" md="4">
                            <WInput :label="$t('shortname')" disabled v-model="Parent.shortName" />
                        </b-col>
                        <b-col sm="12" md="4">
                            <WInput :label="$t('fullname')" disabled v-model="Parent.fullName" />
                        </b-col>
                        <b-col sm="12" md="4">
                            <WInput :label="$t('supervisor')" disabled v-model="Parent.director" />
                        </b-col>
                    </b-row>
                    <b-row style="margin-top: 16px">
                        <b-col sm="12" md="4">
                            <WInput :label="$t('oked')" disabled v-model="Parent.oked" />
                        </b-col>

                        <b-col sm="12" md="4">
                            <WInput :label="$t('country')" disabled v-model="Parent.country" />
                        </b-col>
                        <b-col sm="12" md="4">
                            <WInput :label="$t('region')" disabled v-model="Parent.region" />
                        </b-col>
                    </b-row>
                    <b-row style="margin-top: 16px">
                        <b-col sm="12" md="4">
                            <WInput :label="$t('district')" disabled v-model="Parent.district" />
                        </b-col>
                        <b-col sm="12" md="4">
                            <WInput :label="$t('address')" :rules="!editProfileAdress ? 'required' : ''" :disabled="editProfileAdress" v-model="Parent.address" />
                        </b-col>
                        <b-col sm="12" md="4">
                            <WInput :label="$t('bank')" disabled v-model="Parent.bank" />
                        </b-col>
                    </b-row>
                    <b-row style="margin-top: 16px">
                        <b-col sm="12" md="4">
                            <WInput :label="$t('pinfl')" disabled v-model="Data.pinfl" />
                        </b-col>
                        <b-col sm="12" md="4">
                            <WInput :label="$t('userName')" disabled v-model="Data.userName" />
                        </b-col>
                    </b-row>
                    <hr />
                    <b-row style="margin-top: 16px">
                        <b-col cols="12">
                            <div class="d-flex gap-2 justify-content-end">
                                <b-button variant="light" class="border-secondary" @click="editProfile" size="sm">
                                    <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none">
                                        <path
                                            d="M13.2601 3.6L5.0501 12.29C4.7401 12.62 4.4401 13.27 4.3801 13.72L4.0101 16.96C3.8801 18.13 4.7201 18.93 5.8801 18.73L9.1001 18.18C9.5501 18.1 10.1801 17.77 10.4901 17.43L18.7001 8.74C20.1201 7.24 20.7601 5.53 18.5501 3.44C16.3501 1.37 14.6801 2.1 13.2601 3.6Z"
                                            stroke="#000"
                                            stroke-width="1.5"
                                            stroke-miterlimit="10"
                                            stroke-linecap="round"
                                            stroke-linejoin="round"
                                        />
                                        <path
                                            d="M11.8899 5.05C12.3199 7.81 14.5599 9.92 17.3399 10.2"
                                            stroke="#000"
                                            stroke-width="1.5"
                                            stroke-miterlimit="10"
                                            stroke-linecap="round"
                                            stroke-linejoin="round"
                                        />
                                        <path d="M3 22H21" stroke="#000" stroke-width="1.5" stroke-miterlimit="10" stroke-linecap="round" stroke-linejoin="round" />
                                    </svg>
                                    <span style="color: #000">{{ $t('edit') }}</span>
                                </b-button>
                                <div class="d-flex justify-content-start">
                                    <b-button variant="danger" @click="() => SyncWithSoliq()" class="" size="sm">
                                        <span>{{ $t('sync') }}</span>
                                    </b-button>
                                </div>
                                <div class="d-flex justify-content-start">
                                    <b-button variant="info" @click="() => ((editProfileAdress = true), SaveData())" class="px-5" size="sm">
                                        <span>{{ $t('save') }}</span>
                                    </b-button>
                                </div>
                            </div>
                        </b-col>
                    </b-row>
                </b-card>
            </b-tab>
            <b-tab>
                <template #title>
                    <Navlink icon="briefcase" :label="$t('myOrganizations')" />
                </template>

                <b-card class="bg-white">
                    <OrganizationList />
                </b-card>
            </b-tab>
            <b-tab>
                <template #title>
                    <Navlink icon="telephone" :label="$t('contacts')" />
                </template>
                <contacts />
            </b-tab>
            <b-tab>
                <template #title>
                    <Navlink icon="credit-card" :label="$t('accountCode')" />
                </template>
                <SettlementAccount />
            </b-tab>
        </b-tabs>
    </div>
</template>

<script>
import Navlink from '@/components/cabinet/Navlink.vue';
import WInput from '@/components/forms/WInput.vue';
import AppListHeaderForName from '@/components/application/AppListHeaderForName.vue';
import customInput from '../../components/elements/customInput.vue';
import customDatePicker from '../../components/elements/customDatePicker.vue';
import customSelect from '../../components/elements/customSelect.vue';
import customButtonOutline from '../../components/elements/customButtonOutline.vue';
import customButton from '../../components/elements/customButton.vue';
import customRadioButton from '../../components/elements/customRadioButton.vue';
import customBadge from '../../components/elements/customBadge.vue';
import customDialog from '../../components/elements/customDiaolg.vue';
import AccountService from '@/services/account.service';
import customLabel from '../../components/elements/customLabel.vue';
import PlusIcon from '../../components/custom-icons/PlusIcon.vue';
import ManualService from '@/services/manual.service';
import contacts from './MyInfo/components/contacts.vue';
import SettlementAccount from './MyInfo/components/SettlementAccount.vue';

import vSelect from 'vue-select';
import AppListHeader from '../../components/application/AppListHeader.vue';
import OrganizationList from './widgets/OrganizationList.vue';
import WSelect from '@/components/forms/WSelect.vue';
import { BModal } from 'bootstrap-vue';
export default {
    components: {
        SettlementAccount,
        contacts,
        WSelect,
        WInput,
        Navlink,
        AppListHeaderForName,
        customInput,
        customDatePicker,
        customSelect,
        customButtonOutline,
        customButton,
        PlusIcon,
        customRadioButton,
        vSelect,
        BModal,
        customDialog,
        customLabel,
        customBadge,
        AppListHeader,
        OrganizationList
    },
    data() {
        return {
            phoneModal: false,
            accountCodeModal: false,
            editProfileAdress: true,
            tabIndex: 0,
            OblastList: [],
            BankList: [],
            StateList: [],
            ContactTypeList: [],
            SearchLoading: false,
            editAddress: false,
            filter: {
                docseries: '',
                docnumber: '',
                dateofbirth: '',
                identitydocumentid: 2,
                kinshipdegreeid: 0,
                ismicroterritory: false
            },
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
            files: {
                file: [],
                filename: '',
                url: ''
            },

            Parent: {},
            Data: {
                settlementAccounts: []
            },
            IdentityDocumentList: [],
            PermanentAdress: {},
            isPermanentAdress: true,
            isActiveAdress: false,
            RefreshEgovLoading: false,
            ActiveAdress: {},
            MfyList: [],
            StreetList: [],
            StreetHouseList: [],
            RegionList: [],
            SaveLoading: false,
            KinShipDegreeList: [],
            IdentityDocumentListForParent: [],
            lang: '',
            sidebar1: false,
            passwordLoading: false,
            password: {
                oldpassword: '',
                newpassword: '',
                confirmedpassword: ''
            },
            iseditmode: false,
            FileLoading: false,
            AttachedFiles: [],
            editedIndex: -1,
            editedIndex1: -1
        };
    },
    created() {
        // if (this.$route.query.tabIndex) {
        //     this.tabIndex = +this.$route.query.tabIndex;
        //     console.log(this.tabIndex);
        // }

        this.Refresh();
        this.lang = localStorage.getItem('locale') || 'uz_cyrl';

        ManualService.BankSelectList().then((res) => {
            this.BankList = res.data;
        });

        ManualService.StateSelectList().then((res) => {
            this.StateList = res.data;
        });
        ManualService.ContactTypeSelectList().then((res) => {
            this.ContactTypeList = res.data;
        });
    },
    methods: {
        addaccountCode() {
            this.accountCodeModal = true;
        },

        editProfile() {
            this.editProfileAdress = false;
        },

        SyncWithSoliq() {
            AccountService.SyncWithSoliq()
                .then((res) => {
                    this.makeToast(this.$t('RefreshSuccess'), 'success');
                    this.Refresh();
                })
                .catch((error) => {
                    this.makeToast(error.response.data.error, 'error');
                });
        },
        Refresh() {
            this.SearchLoading = true;
            AccountService.GetUserInfo()
                .then((res) => {
                    this.Parent = res.data.contractor;
                    this.Data = res.data;
                    this.SearchLoading = false;
                })
                .catch((error) => {
                    this.SearchLoading = false;
                    this.makeToast(error.response.data.error, 'error');
                });
        },
        SaveData() {
            this.saveLoading = true;
            AccountService.UpdateUserInfo(this.Data)
                .then((res) => {
                    this.makeToast(this.$t('SaveSuccess'), 'success');
                })
                .catch((error) => {
                    this.saveLoading = false;
                    this.showApiError(error);
                })
                .finally(() => {
                    this.saveLoading = false;
                });
        },
        toggle() {
            this.sidebar1 = !this.sidebar1;
            this.password = {
                oldpassword: '',
                newpassword: '',
                confirmedpassword: ''
            };
        },
        changepassword() {
            this.passwordLoading = true;
            AccountService.ChangePassword(this.password)
                .then((res) => {
                    this.passwordLoading = false;
                    this.makeToast(this.$t('PasswordChangedSuccess'), 'success');
                })
                .catch((error) => {
                    this.passwordLoading = false;
                    this.makeToast(error.response.data.error, 'error');
                });
        },

        ChangeStreetHouse() {
            if (!!this.Parent.livestreethousename) {
                this.Parent.ismultistoreyhouse = this.StreetHouseList.filter((item) => item.streethousename === this.Parent.livestreethousename)[0].ismultistoreyhouse;
            }
        }
    },
    watch: {
        tabIndex: {
            handler(newValue) {
                if (this.$route.query.tabIndex !== String(newValue)) {
                    this.$router.replace({
                        query: { ...this.$route.query, tabIndex: String(newValue) }
                    });
                }
            }
        },
        '$route.query.tabIndex': {
            handler(newValue) {
                if (this.tabIndex !== +newValue) {
                    this.tabIndex = +newValue;
                }
            },
            immediate: true
        }
    }
};
</script>

<style lang="scss">
.myWrapper {
    height: calc(100% - 72px);
}
.tabsTab {
    margin-left: 0 !important;
    margin-right: 0 !important;
    display: flex;
    height: 100%;
    .profile-tabs {
        border-right: 1px solid var(--Light-gray, #d5d7e1);
        border-bottom: 1px solid var(--Light-gray, #d5d7e1);
        background: #fff;
        padding: 1rem;
        height: 100%;
        overflow-y: hidden;
        flex: 0 0 auto;
        .nav {
            gap: 8px;
        }

        .nav-link {
            color: var(--gray, #6a6d7d) !important;
            font-family: 'Museo Sans', serif;
            font-size: 16px;
            font-style: normal;
            font-weight: 500;
            line-height: normal;
            transition: all 0.2s ease-in-out;
            border-radius: 6px;
        }

        .nav-link.active,
        .nav-link:hover,
        .show > .nav-link {
            background: #f4f4f4;
            color: var(--Black, #000107) !important;
        }
    }
    .tab-content {
        padding: 1rem 0 0 1rem;
    }
}
.edit-input {
    position: relative;
}
.address-edit {
    position: absolute;
    top: 50%;
    transform: translateY(-50%);
    right: 15px;
    z-index: 10;
    cursor: pointer;
}
</style>
