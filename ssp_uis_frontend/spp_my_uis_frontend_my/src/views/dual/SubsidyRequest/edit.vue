<template>
    <div>
        <AppListHeaderForName title="SubsidyRequest" page-name="SubsidyRequest" />
        <div class="container">
            <b-overlay :show="Loading">
                <b-row class="justify-content-center">
                    <b-col sm="12" lg="9">
                        <b-card class="form-card">
                            <validation-observer ref="ValidationDTO" v-if="Data">
                                <b-card-text>
                                    <b-row>
                                        <b-col sm="12" md="6" lg="6">
                                            <WInput rules="required" :label="$t('docNumberApplication')" name="docNumberApplication" v-model="Data.docNumber" />
                                        </b-col>
                                        <b-col sm="12" md="6" lg="6">
                                            <WInput rules="required" :label="$t('docDateApplication')" name="docDateApplication" v-model="Data.docOn" />
                                        </b-col>
                                        <b-col sm="12" md="6" lg="6">
                                            <WInput rules="required" :label="$t('contractorInn')" name="contractorInn" disabled v-model="Data.contractorInn" />
                                        </b-col>
                                        <b-col sm="12" md="6" lg="6">
                                            <WInput rules="required" :label="$t('contractor')" name="contractor" disabled v-model="Data.contractor" />
                                        </b-col>
                                        <b-col sm="12" md="6" lg="6">
                                            <WSelect
                                                @input="ChangeRegion"
                                                disabled
                                                :label="$t('region')"
                                                :name="$t('region')"
                                                :options="RegionList"
                                                :clearable="false"
                                                v-model="Data.regionId"
                                                rules="required"
                                            />
                                        </b-col>
                                        <b-col sm="12" md="6" lg="6">
                                            <WSelect
                                                rules="required"
                                                disabled
                                                :name="$t('district')"
                                                :label="$t('district')"
                                                :options="DistrictList"
                                                :clearable="false"
                                                v-model="Data.districtId"
                                            />
                                        </b-col>
                                        <b-col sm="12" md="6" lg="6">
                                            <WInput rules="required" :label="$t('address')" name="address" v-model="Data.address" />
                                        </b-col>
                                        <b-col sm="12" md="6" lg="6">
                                            <WInput rules="email" :label="$t('email')" name="email" v-model="Data.email" />
                                        </b-col>

                                        <b-col sm="12" md="6" lg="6">
                                            <WDatePicker rules="required" :name="$t('year')" format="YYYY" type="year" :label="$t('yearIn')" v-model="Data.year" />
                                        </b-col>
                                        <b-col sm="12" md="6" lg="6">
                                            <WSelect rules="required" :label="$t('monthIn')" :name="$t('monthIn')" :clearable="true" :options="MonthInList" v-model="Data.month" />
                                        </b-col>

                                        <b-col sm="12" md="6" lg="6">
                                            <AppContractorSettlementAccount v-model="Data.contractorSettlementAccountId" />
                                        </b-col>

                                        <b-col sm="12" md="6" lg="6">
                                            <WInput rules="required" :label="$t('phone')" name="phone" v-model="Data.phone" />
                                        </b-col>

                                        <b-col sm="12" md="6" lg="6">
                                            <WCurrencyInput disabled :label="$t('totalSubsidyAmount')" name="totalSubsidyAmount" v-model="Data.totalSubsidyAmount" />
                                        </b-col>
                                    </b-row>
                                </b-card-text>
                            </validation-observer>

                            <div>
                                <b-button
                                    @click="
                                        () => {
                                            openModel = true;
                                            editIndex = -1;
                                        }
                                    "
                                    variant="success"
                                    size="md"
                                >
                                    <b-icon-plus></b-icon-plus>
                                    {{ $t('Add') }}
                                </b-button>

                                <b-modal v-model="openModel" v-if="openModel" size="xl" class="pa-2" no-close-on-backdrop hide-footer>
                                    <validation-observer ref="ValidationTables" disabled>
                                        <b-row class="mt-2">
                                            <b-col sm="12" md="4">
                                                <WInput
                                                    style="flex: 1 1"
                                                    v-model="TabRow.seria"
                                                    rules="required"
                                                    v-mask="'AA'"
                                                    v-uppercase
                                                    :label="$t('Seria')"
                                                    :name="$t('Seria')"
                                                    :placeholder="$t('AA')"
                                                >
                                                </WInput>
                                            </b-col>
                                            <b-col sm="12" md="4">
                                                <WInput
                                                    style="flex: 1 1"
                                                    v-model="TabRow.number"
                                                    rules="required"
                                                    :label="$t('Raqami')"
                                                    v-mask="'#######'"
                                                    :name="$t('Number')"
                                                    :placeholder="$t('1234567')"
                                                >
                                                </WInput>
                                            </b-col>
                                            <b-col sm="12" md="4">
                                                <b-input-group class="w-input-group">
                                                    <WDatePicker
                                                        style="flex: 1 1"
                                                        v-model="TabRow.dateOfBirth"
                                                        rules="required"
                                                        :label="$t('dateofbirth')"
                                                        :name="$t('dateofbirth')"
                                                        :placeholder="$t('dateofbirth')"
                                                    >
                                                    </WDatePicker>

                                                    <b-input-group-append>
                                                        <b-button @click="GetPerson" :disabled="soliqLoading" variant="primary" class="px-3">
                                                            <b-spinner v-if="soliqLoading" small style="margin-right: 8px"></b-spinner>
                                                            <b-icon scale="0.9" icon="search"></b-icon
                                                        ></b-button>
                                                    </b-input-group-append>
                                                </b-input-group>
                                            </b-col>

                                            <b-col sm="12" md="6" lg="6">
                                                <WInput rules="required" disabled :label="$t('familyname')" name="familyname" v-model="TabRow.surname" />
                                            </b-col>
                                            <b-col sm="12" md="6" lg="6">
                                                <WInput rules="required" disabled :label="$t('firstname')" name="firstname" v-model="TabRow.name" />
                                            </b-col>
                                            <b-col sm="12" md="6" lg="6">
                                                <WInput rules="required" disabled :label="$t('lastname')" name="lastname" v-model="TabRow.patronym" />
                                            </b-col>
                                            <b-col sm="12" md="6" lg="6">
                                                <WInput rules="required" disabled :label="$t('pinfl')" name="pinfl" v-model="TabRow.pinfl" />
                                            </b-col>

                                            <b-col sm="12" md="6" lg="6">
                                                <WCurrencyInput rules="required" :label="$t('salary')" name="salary" v-model="TabRow.salary" />
                                            </b-col>
                                            <b-col sm="12" md="6" lg="6">
                                                <WCurrencyInput rules="required" :label="$t('subsidy')" name="subsidy" v-model="TabRow.subsidy" />
                                            </b-col>

                                            <b-col sm="12" md="12" lg="12">
                                                <h6 class="inputTitle">{{ $t('3_tomonlama_shartnoma_nusxasi') }} <span class="text-danger">*</span></h6>

                                                <b-form-file
                                                    type="file"
                                                    :placeholder="$t('selectFile')"
                                                    v-model="file1"
                                                    class="mt-2"
                                                    @change="UploadFile"
                                                    :browse-text="$t('select')"
                                                    :disabled="fileLoading"
                                                />
                                                <div class="mt-3" v-for="item in TabRow.files" :key="item.id">
                                                    <b-link variant="primary" target="_blank" :href="FileSrc(item.id)">{{ item.fileName || item.id }}</b-link>
                                                    <b-button v-if="canEdit" variant="danger" size="sm" class="ml-1" @click="DeleteFile(item.id)">
                                                        <b-icon-trash scale="0.7" />
                                                    </b-button>
                                                </div>
                                            </b-col>

                                            <b-col sm="12" md="4" class="align-self-center my-3">
                                                <b-button @click="AddTabRow" variant="success" size="sm">
                                                    <b-icon-plus></b-icon-plus>
                                                    {{ $t('Add') }}
                                                </b-button>
                                            </b-col>
                                        </b-row>
                                    </validation-observer>
                                </b-modal>
                            </div>

                            <b-table :items="Data.tables" :fields="TabrowFields" small responsive striped bordered style="font-size: 12px">
                                <template #cell(pinfl)="{ item }">
                                    {{ item.pinfl }}
                                </template>
                                <template #cell(fullName)="{ item }">
                                    {{ item.surname }}
                                    {{ item.name }}
                                    {{ item.patronym }}
                                </template>
                                <template #cell(seria)="{ item }">
                                    {{ item.seria }}
                                    {{ item.number }}
                                </template>
                                <template #cell(fileName)="{ item }">
                                    {{ item.files[0]?.fileName }}
                                </template>
                                <template #cell(actions)="{ item, index }">
                                    <b-icon-trash-fill scale="1.2" @click="DeleteTabRow(index)" class="cursor-pointer mr-3 text-danger" />
                                    <b-icon-pen scale="1.2" @click="EditTabRow(item, index)" class="cursor-pointer" style="color: blue" />
                                </template>
                            </b-table>
                        </b-card>
                    </b-col>
                    <b-col sm="12" lg="3">
                        <b-card class="form-card">
                            <b-card-text>
                                <!-- reject -->
                                <b-button @click="SaveData" v-if="canEdit" :disabled="saveLoading || Loading" variant="success" block class="mb-1">
                                    <b-spinner v-if="saveLoading" small></b-spinner>
                                    <b-icon-check scale="0.8"></b-icon-check>
                                    {{ $t('save') }}
                                </b-button>
                                <!-- delete -->
                                <b-button v-if="Data.canDelete" @click="$bvModal.show('DeleteModal' + Data.id)" variant="danger" block class="mb-1">
                                    <b-icon-trash scale="0.6"></b-icon-trash>
                                    {{ $t('delete') }}
                                </b-button>
                                <!-- send -->
                                <b-button v-if="Data.canSend" @click="$bvModal.show('ESPmodal')" variant="primary" block class="mb-1">
                                    <b-icon-arrow-bar-up scale="0.6"></b-icon-arrow-bar-up>
                                    {{ $t('Send') }}
                                </b-button>
                                <!-- canRevoke  -->
                                <b-button v-if="Data.canRevoke" @click="OpenRevokeModal(Data)" variant="warning" block class="mb-1">
                                    <b-icon-x-circle scale="0.6"></b-icon-x-circle>
                                    {{ $t('Revoke') }}
                                </b-button>
                            </b-card-text>
                        </b-card>
                    </b-col>
                </b-row>
            </b-overlay>
            <b-modal :id="'DeleteModal' + Data.id" :title="$t('delete')" no-close-on-backdrop hide-footer>
                <p>{{ $t('WantDeleteAdm') }}</p>
                <b-row>
                    <b-col style="text-align: end" class="text-right">
                        <a @click="$bvModal.hide('DeleteModal' + Data.id)" style="margin-right: 5px" class="btn btn-sm btn-soft-danger mr-2 pr-btn">{{ $t('no') }}</a>
                        <a @click="Delete(Data)" class="btn btn-sm btn-success pr-btn">
                            <b-spinner v-if="DeleteLoading" small></b-spinner>
                            {{ $t('yes') }}
                        </a>
                    </b-col>
                </b-row>
            </b-modal>
            <b-modal :id="'SendModal' + Data.id" :title="$t('send')" no-close-on-backdrop hide-footer>
                <p>{{ $t('wantSend') }}</p>

                <b-row class="mt-3">
                    <b-col style="text-align: end" class="text-right">
                        <a @click="$bvModal.hide('SendModal' + Data.id)" class="btn btn-sm btn-soft-danger mr-2 pr-btn" style="margin-right: 5px">{{ $t('no') }}</a>

                        <b-button variant="success" @click="Send(Data)">
                            <b-spinner v-if="SendLoading" small></b-spinner>
                            {{ $t('yes') }}
                        </b-button>
                    </b-col>
                </b-row>
            </b-modal>

            <b-modal :id="'CancelModal' + Data.id" :title="$t('Revoke')" no-close-on-backdrop hide-footer>
                <p>{{ $t('WantRevoke') }}</p>
                <b-row>
                    <b-col style="text-align: end" class="text-right">
                        <a @click="$bvModal.hide('CancelModal' + Data.id)" class="btn btn-sm btn-soft-danger mr-2 pr-btn" style="margin-right: 5px">{{ $t('no') }}</a>
                        <a @click="Revoke(Data)" class="btn btn-sm btn-success pr-btn">
                            <b-spinner v-if="CancelLoading" small></b-spinner>
                            {{ $t('yes') }}
                        </a>
                    </b-col>
                </b-row>
            </b-modal>

            <b-modal id="ESPmodal" :title="$t('ESPmodal')" no-close-on-backdrop hide-footer>
                <just-sign @sign="GetESP($event)"></just-sign>

                <b-row>
                    <b-col style="text-align: end" class="text-right">
                        <a @click="$bvModal.hide('ESPmodal')" class="btn btn-sm btn-soft-danger mr-2 pr-btn" style="margin-right: 5px">{{ $t('no') }}</a>
                        <!-- <a @click="Send(Data)" class="btn btn-sm btn-success pr-btn">
                        <b-spinner v-if="SignLoading" small></b-spinner>
                        {{ $t('yes') }}
                    </a> -->
                    </b-col>
                </b-row>
            </b-modal>
        </div>
    </div>
</template>

<script>
// components
import WCurrencyInput from '@/components/forms/WCurrencyInput.vue';
import WSelect from '@/components/forms/WSelect.vue';
import WTextarea from '@/components/forms/WTextarea.vue';
import WInput from '@/components/forms/WInput.vue';
import WDatePicker from '@/components/forms/WDatePicker.vue';

import SubsidyRequestService from '@/services/dual/SubsidyRequest.service';
import ManualService from '@/services/manual.service';
import AppListHeaderForName from '@/components/application/AppListHeaderForName.vue';
import axios from 'axios';
import AppContractorSettlementAccount from '@/components/application/AppContractorSettlementAccount.vue';
import PersonService from '@/services/person.service';
import JustSign from '@/components/justSign.vue';

const TabRowDef = {
    id: 0,
    personId: 0,
    salary: 0,
    subsidy: 0,
    seria: '',
    number: '',
    dateOfBirth: '',
    files: [],
    name: '',
    patronym: '',
    surname: '',
    pinfl: ''
};

export default {
    components: {
        WCurrencyInput,
        JustSign,
        WSelect,
        WTextarea,
        WInput,
        WDatePicker,
        AppListHeaderForName,
        AppContractorSettlementAccount
    },
    directives: {
        uppercase: {
            update: function (el) {
                const inputEl = el.querySelector('input');
                if (inputEl) {
                    inputEl.value = inputEl.value.toUpperCase();
                }
            }
        }
    },
    data() {
        return {
            file1: null,
            iframeLoaded: false,
            axios,
            editIndex: -1,
            openModel: false,
            Data: {
                tables: [],
                totalSubsidyAmount: null
            },
            tabActive: 1,
            filter: {
                id: 0,
                message: '',
                signedData: ''
            },
            DistrictList: [],
            RegionList: [],
            MonthInList: [],
            soliqLoading: false,
            TabRow: JSON.parse(JSON.stringify(TabRowDef)),
            TabrowFields: [
                {
                    key: 'fullName',
                    label: this.$t('fio')
                },
                {
                    key: 'pinfl',
                    label: this.$t('pinfl'),
                    tdClass: 'text-center',
                    thClass: 'text-center'
                },
                {
                    key: 'seria',
                    label: this.$t('Raqami'),
                    tdClass: 'text-center',
                    thClass: 'text-center'
                },
                {
                    key: 'salary',
                    label: this.$t('salary'),
                    tdClass: 'text-right',
                    thClass: 'text-right'
                },
                {
                    key: 'subsidy',
                    label: this.$t('subsidy'),
                    tdClass: 'text-right',
                    thClass: 'text-right'
                },
                {
                    key: 'fileName',
                    label: this.$t('fileName'),
                    tdClass: 'text-right',
                    thClass: 'text-right'
                },
                {
                    key: 'actions',
                    tdClass: 'text-center',
                    thClass: 'text-center',
                    label: this.$t('actions')
                }
            ],
            Loading: false,
            saveLoading: false,
            fileLoading: false,
            DeleteLoading: false,
            SendLoading: false,
            CancelLoading: false,
            downloadLoading: false
        };
    },
    computed: {
        IframeSrc() {
            return axios.defaults.baseURL + `Dual/SubsidyRequest/DownloadPdf?id2=${this.Data.application.id2}&lang=${this.getPdfLang()}`;
        },
        FileSrc() {
            return (id) => axios.defaults.baseURL + `Dual/SubsidyRequest/DownloadFile/${id}`;
        },
        canEdit() {
            return !this.Data.canSend || this.Data.id == 0 || this.Data.canEdit;
        }
    },
    created() {
        this.Refresh();

        ManualService.RegionSelectList().then((res) => {
            this.RegionList = res.data;
        });
        ManualService.GetMonthSelectList().then((res) => {
            this.MonthInList = res.data;
        });
    },
    methods: {
        GetPerson() {
            this.TabRow.name = '';
            this.TabRow.surname = '';
            this.TabRow.patronym = '';
            this.soliqLoading = true;
            PersonService.GetByPassportDataFromDigital(this.TabRow.seria, this.TabRow.number, this.TabRow.dateOfBirth)
                .then((res) => {
                    this.TabRow.pinfl = res.data?.pinfl;
                    this.TabRow.name = res.data?.surnameLatin;
                    this.TabRow.surname = res.data?.nameLatin;
                    this.TabRow.patronym = res.data?.patronymLatin;
                })
                .catch(this.showApiError)
                .finally(() => {
                    this.soliqLoading = false;
                });
        },
        DeleteTabRow(index) {
            this.Data.tables.splice(index, 1);
            let sum = 0;
            this.Data.tables.forEach((item) => {
                sum += item.subsidy;
            });
            this.Data.totalSubsidyAmount = sum;
        },
        AddTabRow() {
            this.$refs.ValidationTables.validate().then((success) => {
                if (success) {
                    if (this.TabRow.files.length) {
                        if (this.editIndex < 0) {
                            this.TabRow.orderNumber = this.Data.tables.length + 1 + '';
                            this.Data.tables.push({ ...this.TabRow });

                            this.openModel = false;
                            let sum = 0;
                            this.Data.tables.forEach((item) => {
                                sum += item.subsidy;
                            });
                            this.Data.totalSubsidyAmount = sum;
                        } else {
                            Object.assign(this.Data.tables[this.editIndex], this.TabRow);
                            this.openModel = false;
                        }

                        this.$refs.ValidationTables.reset();
                        this.TabRow = JSON.parse(JSON.stringify(TabRowDef));
                    } else {
                        this.makeToast(this.$t('selectFile'), 'error');
                    }
                }
            });
        },
        ChangeRegion(item) {
            ManualService.DistrictSelectList(item).then((res) => {
                this.DistrictList = res.data;
            });
        },
        FileDownload(item) {
            this.downloadLoading = true;
            SubsidyRequestService.DownloadPdf(item.id2, this.lang)
                .then((res) => {
                    this.forceFileDownload(res, this.$t('SubsidyRequest'));
                })
                .catch(this.showApiError)
                .finally(() => {
                    this.downloadLoading = false;
                });
        },
        UploadFile(event) {
            const formData = new FormData();
            formData.append('files', event.target.files[0]);
            this.fileLoading = true;
            console.log(this.TabRow);
            SubsidyRequestService.UploadFiles(formData)
                .then((res) => {
                    this.TabRow.files.push(...res.data);
                })
                .catch(this.showApiError)
                .finally(() => {
                    this.fileLoading = false;
                });
        },
        DeleteFile(id) {
            SubsidyRequestService.DeleteFile(id).then(() => {
                this.TabRow.files = this.TabRow.files.filter((item) => item.id != id);
            });
        },
        Refresh() {
            this.Loading = true;
            SubsidyRequestService.Get(this.$route.params.id)
                .then((res) => {
                    this.Data = res.data;

                    if (res.data.regionId) {
                        this.ChangeRegion(res.data.regionId);
                    }
                })
                .catch(this.showApiError)
                .finally(() => {
                    this.Loading = false;
                });
        },
        OpenRevokeModal(item) {
            this.filter.id = item.id;
            this.filter.message = '';
            this.$bvModal.show('CancelModal' + item.id);
        },
        OpenSendModal(item) {
            this.filter.id = item.id;
            this.filter.message = '';
            this.$bvModal.show('SendModal' + item.id);
        },

        EditTabRow(item, index) {
            this.TabRow = { ...item };
            this.editIndex = index;
            this.openModel = true;
        },
        Delete(item) {
            this.DeleteLoading = true;
            SubsidyRequestService.Delete(item.id)
                .then(() => {
                    this.DeleteLoading = false;
                    this.makeToast(this.$t('DeleteSuccess'), 'success');
                    this.$bvModal.hide('DeleteModal' + item.id);

                    this.$router.push({ name: 'SubsidyRequest' });
                })
                .catch((error) => {
                    this.showApiError(error);
                    this.DeleteLoading = false;
                });
        },
        Revoke(item) {
            SubsidyRequestService.Revoke(this.filter)
                .then(() => {
                    this.$bvModal.hide('CancelModal' + item.id);
                    this.Refresh();
                })
                .catch((error) => {
                    this.showApiError(error);
                    this.$bvModal.hide('CancelModal' + item.id);
                })
                .finally(() => {
                    this.CancelLoading = false;
                });
        },
        SaveData() {
            this.$refs.ValidationDTO.validateWithInfo().then(({ isValid, errors }) => {
                if (isValid) {
                    if (!this.Data.tables.length == 0) {
                        this.saveLoading = true;
                        SubsidyRequestService.Update(this.Data)
                            .then(() => {
                                this.makeToast(this.$t('SaveSuccess'), 'success');
                                this.$router.push({ name: 'SubsidyRequest' });
                            })
                            .catch(this.showApiError)
                            .finally(() => {
                                this.saveLoading = false;
                            });
                    } else {
                        this.makeToast(this.$t("tablega malumot qo'shilmagan"), 'error');
                    }
                } else {
                    this.showValidateError(errors);
                }
            });
        },
        GetESP(data) {
            this.$bvModal.hide('ESPmodal');

            this.Send(data);
        },
        Send(item) {
            this.SendLoading = true;
            SubsidyRequestService.Send({ id: this.Data.id, message: this.Data.message, signedData: item.key })
                .then(() => {
                    this.$bvModal.hide('SendModal' + item.id);
                    this.makeToast(this.$t('SuccessSend'), 'success');
                    this.$router.push({ name: 'SubsidyRequest' });
                    // this.Refresh();
                })
                .catch((error) => {
                    this.showApiError(error);
                })
                .finally(() => {
                    this.SendLoading = false;
                });
        }
    }
};
</script>
