<template>
    <div>
        <AppListHeaderForName title="MemshipApplication" page-name="MemshipApplication" />
        <div class="container position-relative">
            <b-overlay :show="Loading">
                <b-row class="justify-content-center">
                    <b-col sm="12" lg="9">
                        <b-card class="form-card">
                            <validation-observer ref="ValidationDTO" v-if="Data.application">
                                <b-card-text class="p-2">
                                    <b-row>
                                        <b-col sm="12" md="4" lg="4" class="my-4">
                                            <WInput rules="required" :label="$t('docDateApplication')" name="docDateApplication" disabled v-model="Data.application.docOn" />
                                        </b-col>
                                        <b-col sm="12" md="4" lg="4" class="my-4">
                                            <WInput
                                                rules="required"
                                                :label="$t('docNumberApplication')"
                                                name="docNumberApplication"
                                                disabled
                                                v-model="Data.application.docNumber"
                                            />
                                        </b-col>
                                        <b-col sm="12" md="4" lg="4">
                                            <WInput :label="$t('registrationDate')" name="registrationDate" disabled v-model="Data.registrationDate" />
                                        </b-col>
                                        <b-col sm="12" md="6" lg="6">
                                            <WInput
                                                rules="required"
                                                :label="$t('contractor')"
                                                name="contractor"
                                                disabled
                                                :value="
                                                    Data.application.contractorInn && Data.application.contractorInn.length == 14
                                                        ? Data.application.contractor + ' - ' + Data.application.contractorDirector
                                                        : Data.application.contractor
                                                "
                                            />
                                        </b-col>
                                        <b-col sm="12" md="6" lg="6">
                                            <WInput rules="required" :label="$t('inn')" name="inn" disabled v-model="Data.application.contractorInn" />
                                        </b-col>
                                        <hr />
                                        <p class="text-left address-text">{{ $t('address') }}</p>
                                        <b-col sm="12" md="4" lg="4">
                                            <WSelect
                                                @input="ChangeRegion"
                                                disabled
                                                :label="$t('region')"
                                                :name="$t('region')"
                                                :options="RegionList"
                                                :clearable="false"
                                                v-model="Data.application.regionId"
                                                rules="required"
                                            />
                                        </b-col>
                                        <b-col sm="12" md="4" lg="4">
                                            <WSelect
                                                rules="required"
                                                disabled
                                                :name="$t('district')"
                                                :label="$t('district')"
                                                :options="DistrictList"
                                                :clearable="false"
                                                v-model="Data.application.districtId"
                                            />
                                        </b-col>
                                        <template v-if="Data.memshipContractTypeId == 2">
                                            <b-col sm="12" md="12" lg="12" class="mt-1 mb-2 text-center">
                                                <b-form-checkbox v-model="Data.chooseLocation" :disabled="!Data.canEdit" @input="ChangeCheckBox">
                                                    {{ $t('chooseLocationMemship') }}
                                                </b-form-checkbox>
                                            </b-col>
                                            <b-col sm="12" md="4" lg="4" class="mt-1" v-if="Data.chooseLocation">
                                                <WSelect
                                                    rules="required"
                                                    @input="ChangeChooseRegion"
                                                    :disabled="!Data.canEdit"
                                                    :label="$t('choosedRegion')"
                                                    :name="$t('choosedRegion')"
                                                    :clearable="true"
                                                    :options="RegionList"
                                                    v-model="Data.choosedRegionId"
                                                />
                                            </b-col>
                                            <b-col sm="12" md="4" lg="4" class="mt-1" v-if="Data.chooseLocation">
                                                <WSelect
                                                    rules="required"
                                                    @input="ChangeChooseDistrict"
                                                    :disabled="!Data.canEdit"
                                                    :label="$t('choosedDistrict')"
                                                    :name="$t('choosedDistrict')"
                                                    :clearable="true"
                                                    :options="ChooseDistrictList"
                                                    v-model="Data.choosedDistrictId"
                                                />
                                            </b-col>
                                            <b-col sm="12" md="12" lg="12">
                                                <WInput
                                                    rules="required"
                                                    :label="$t('contractorAddress')"
                                                    name="contractorAddress"
                                                    placeholder
                                                    v-model="Data.application.contractorAddress"
                                                />
                                            </b-col>
                                        </template>
                                        <hr />
                                        <b-col sm="12" md="6" lg="6">
                                            <WInput
                                                rules="required"
                                                :label="$t('korxonaRaxbari')"
                                                name="korxonaRaxbari"
                                                placeholder
                                                v-model="Data.application.contractorDirector"
                                            />
                                        </b-col>
                                        <b-col sm="12" md="6" lg="6">
                                            <WInput :label="$t('ownerName')" name="ownerName" placeholder v-model="Data.ownerName" />
                                        </b-col>
                                        <b-col sm="12" md="6" lg="6">
                                            <WPhoneInput
                                                :label="$t('contractorWorkPhoneNumber')"
                                                name="contractorWorkPhoneNumber"
                                                rules="required|validatorPhone"
                                                placeholder
                                                v-model="Data.contractorWorkPhoneNumber"
                                            />
                                        </b-col>
                                        <b-col sm="12" md="6" lg="6">
                                            <WPhoneInput
                                                :label="$t('contractorMobilePhoneNumber')"
                                                name="contractorMobilePhoneNumber"
                                                :rules="Data.contractorMobilePhoneNumber ? 'validatorPhone' : null"
                                                placeholder
                                                v-model="Data.contractorMobilePhoneNumber"
                                            />
                                        </b-col>
                                        <b-col sm="12" md="6" lg="6">
                                            <AppContractorSettlementAccount v-model="Data.application.contractorSettlementAccountId" />
                                        </b-col>
                                        <b-col sm="12" md="6" lg="6">
                                            <!-- rules="required|email" -->
                                            <WInput :label="$t('addressEmail')" name="addressEmail" placeholder v-model="Data.contractorEmail" />
                                        </b-col>
                                        <hr />
                                        <!-- <b-col sm="12" md="6" lg="6">
                                            <WCurrencyInput
                                                rules="required"
                                                :disabled="!(Data.application.contractorInn && Data.application.contractorInn.length == 14)"
                                                :label="$t('yearlyEarnings')"
                                                name="yearlyEarnings"
                                                v-model="Data.yearlyEarnings"
                                            />
                                        </b-col>
                                        <b-col sm="12" md="6" lg="6">
                                            <WCurrencyInput
                                                rules="required"
                                                :disabled="!(Data.application.contractorInn && Data.application.contractorInn.length == 14)"
                                                :label="$t('nowYearlyEarnings')"
                                                name="nowYearlyEarnings"
                                                v-model="Data.nowYearlyEarnings"
                                            />
                                        </b-col> -->
                                        <b-col sm="12" md="12" lg="12">
                                            <WSelect rules="required" v-model="Data.okedId" :options="OkedList" :label="$t('okedMem')" clearable name="oked" />
                                        </b-col>
                                        <b-col sm="12" md="6" lg="6">
                                            <WSelect
                                                rules="required"
                                                disabled
                                                v-model="Data.contractorCategoryId"
                                                :options="ContractorCategoryList"
                                                :label="$t('contractorCategory')"
                                                clearable
                                                name="contractorCategory"
                                            />
                                        </b-col>
                                        <b-col sm="12" md="6" lg="6">
                                            <WInput rules="required" :label="$t('employeesCount')" name="employeesCount" placeholder v-model="Data.employeesCount" />
                                        </b-col>
                                        <hr />
                                        <b-col sm="12" md="12" lg="12" class="mb-2">
                                            <h5 class="text-center font-weight-bolder text-muted">
                                                {{ $t('helpSppService') }}
                                            </h5>
                                        </b-col>
                                        <b-col sm="12" md="12" lg="12">
                                            <h6 class="inputTitle">
                                                {{ $t('Taklifga ilovalar (Fayl yuklash)') }}
                                            </h6>
                                            <b-form-file
                                                type="file"
                                                :placeholder="$t('selectFile')"
                                                class="mt-2"
                                                @change="UploadFile"
                                                :browse-text="$t('select')"
                                                :disabled="fileLoading"
                                            />
                                            <div class="mt-3" v-for="item in Data.files" :key="item.id">
                                                <b-link variant="primary" target="_blank" :href="FileSrc(item.id)">{{ item.fileName || item.id }}</b-link>
                                                <b-button v-if="canUpdate" variant="danger" size="sm" class="ml-1" @click="DeleteFile(item.id)">
                                                    <b-icon-trash scale="0.7" />
                                                </b-button>
                                            </div>
                                        </b-col>
                                    </b-row>
                                </b-card-text>
                            </validation-observer>
                        </b-card>
                    </b-col>
                    <b-col sm="12" lg="3">
                        <b-card class="form-card">
                            <b-card-text>
                                <!-- reject -->
                                <b-alert v-if="Data.application && Data.application.statusId == 25 && Data.application.message" class="mt-1" show dismissible variant="danger">
                                    {{ Data.application.message }}
                                </b-alert>
                                <b-button @click="SaveData" v-if="canUpdate" :disabled="saveLoading || Loading" variant="success" block class="mb-1">
                                    <b-spinner v-if="saveLoading" small></b-spinner>
                                    <b-icon-check scale="0.8"></b-icon-check>
                                    {{ $t('save') }}
                                </b-button>
                                <!-- download -->
                                <!-- delete -->
                                <b-button v-if="Data.canDelete" @click="$bvModal.show('DeleteModal' + Data.id)" variant="danger" block class="mb-1">
                                    <b-icon-trash scale="0.6"></b-icon-trash>
                                    {{ $t('delete') }}
                                </b-button>
                                <!-- send -->
                                <b-button v-if="Data.canSend" @click="OpenSendModal(Data)" variant="primary" block class="mb-1">
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
                <b-row class="mt-3">
                    <b-col style="text-align: end" class="text-right">
                        <a @click="$bvModal.hide('SendModal' + Data.id)" class="btn btn-sm btn-soft-danger mr-2 pr-btn" style="margin-right: 5px">{{ $t('no') }}</a>
                        <b-button variant="success" v-b-modal.ESPmodal>
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
            <b-modal hide-header no-close-on-backdrop hide-footer v-model="createApplicationForBig">
                <p class="mt-3 pt-3">{{ $t('WantCreateApplicationForNotFree') }}</p>
                <b-row>
                    <b-col style="text-align: end" class="text-right">
                        <a @click="createApplicationForBig = false" class="btn btn-sm btn-success pr-btn">
                            {{ $t('agree') }}
                        </a>
                    </b-col>
                </b-row>
            </b-modal>
            <b-modal id="ESPmodal" :title="$t('ESPmodal')" no-close-on-backdrop hide-footer>
                <just-sign @sign="GetESP($event)"></just-sign>
                <b-row>
                    <b-col style="text-align: end" class="text-right">
                        <a @click="$bvModal.hide('ESPmodal')" class="btn btn-sm btn-soft-danger mr-2 pr-btn" style="margin-right: 5px">{{ $t('no') }}</a>
                        <a @click="Send(Data)" class="btn btn-sm btn-success pr-btn">
                            <b-spinner v-if="CancelLoading" small></b-spinner>
                            {{ $t('yes') }}
                        </a>
                    </b-col>
                </b-row>
            </b-modal>
            <Chat v-if="Data && Data.id" :table-id="Data.tableId || 100" :document-id="Data.id" />
        </div>
    </div>
</template>

<script>
// components
import WCurrencyInput from '@/components/forms/WCurrencyInput.vue';
import WSelect from '@/components/forms/WSelect.vue';
import WTextarea from '@/components/forms/WTextarea.vue';
import WInput from '@/components/forms/WInput.vue';
import WPhoneInput from '@/components/forms/WPhoneInput.vue';
import JustSign from '@/components/justSign.vue';

import MemshipApplicationService from '@/services/memshipapplication.service';
import ManualService from '@/services/manual.service';
import AppListHeaderForName from '@/components/application/AppListHeaderForName.vue';
import axios from 'axios';
import AppContractorSettlementAccount from '@/components/application/AppContractorSettlementAccount.vue';
const Chat = () => import('@/components/DocumentChat/Chat.vue');

export default {
    components: {
        WCurrencyInput,
        WSelect,
        WTextarea,
        WInput,
        WPhoneInput,
        JustSign,
        AppListHeaderForName,
        AppContractorSettlementAccount,
        Chat
    },

    data() {
        return {
            createApplicationForBig: false,
            axios,
            Data: {},
            tabActive: 1,
            ContractorActivityTypeList: [],
            NeedChamberServiceSelectList: [],
            ContractorCategoryList: [],
            ApplicationHTMLData: {},
            filter: {
                id: 0,
                message: '',
                signedData: ''
            },
            ESPmodal: false,
            DistrictList: [],
            RegionList: [],
            ChooseDistrictList: [],
            List: [],
            OkedList: [],
            isOkedDisabled: false,
            Loading: false,

            OrgName: '',
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
            return axios.defaults.baseURL + `MemshipApplication/DownloadPdf?id2=${this.Data.application.id2}&lang=${this.getPdfLang()}`;
        },
        FileSrc() {
            return (id) => axios.defaults.baseURL + `MemshipApplication/DownloadFile/${id}`;
        },
        canUpdate() {
            return !this.Data.canSend || this.Data.id == 0 || this.Data.canEdit;
        }
    },
    created() {
        this.Refresh();

        ManualService.NeedChamberServiceSelectList().then((res) => {
            this.NeedChamberServiceSelectList = res.data;
        });
        ManualService.RegionSelectList().then((res) => {
            this.RegionList = res.data;
        });

        ManualService.ContractorActivityTypeSelectList().then((res) => {
            this.ContractorActivityTypeList = res.data;
        });

        ManualService.ContractorCategorySelectList().then((res) => {
            this.ContractorCategoryList = res.data;
        });
        ManualService.GetOkedAsSelectList().then((res) => {
            this.OkedList = res.data;
        });
    },
    methods: {
        ChangeCheckBox() {
            if (this.Data.canEdit) {
                this.Data.choosedDistrictId = null;
                this.Data.choosedRegionId = null;
            }
        },
        ChangeChooseRegion(item) {
            this.Data.choosedDistrictId = null;
            ManualService.DistrictSelectList(item).then((res) => {
                this.ChooseDistrictList = res.data;
            });
        },
        ChangeRegion(item) {
            ManualService.DistrictSelectList(item).then((res) => {
                this.DistrictList = res.data;
            });
        },
        GetESP(data) {
            this.filter.signedData = data.key;
            this.$bvModal.hide('ESPmodal');

            this.Send(this.Data);
        },
        FileDownload(item) {
            this.downloadLoading = true;
            MemshipApplicationService.DownloadPdf(item.id2, this.lang)
                .then((res) => {
                    this.forceFileDownload(res, this.$t('memshipapplication'));
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
            MemshipApplicationService.UploadFiles(formData)
                .then((res) => {
                    this.Data.files.push(...res.data);
                })
                .catch(this.showApiError)
                .finally(() => {
                    this.fileLoading = false;
                });
        },
        DeleteFile(id) {
            MemshipApplicationService.DeleteFile(id).then(() => {
                this.Data.files = this.Data.files.filter((item) => item.id != id);
            });
        },
        Refresh() {
            this.Loading = true;
            MemshipApplicationService.Get(this.$route.params.id)
                .then((res) => {
                    this.Data = res.data;
                    this.isOkedDisabled = res.data.okedId != null ? true : false;
                    if (this.Data.contractorCategoryId == 4 && this.Data.id == 0) {
                        this.createApplicationForBig = true;
                    }
                    if (res.data.application.regionId) {
                        this.ChangeRegion(res.data.application.regionId);
                    }
                    if (this.Data.id) {
                        this.tabActive = 2;
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
        Delete(item) {
            this.DeleteLoading = true;
            MemshipApplicationService.Delete(item.id)
                .then(() => {
                    this.DeleteLoading = false;
                    this.makeToast(this.$t('DeleteSuccess'), 'success');
                    this.$bvModal.hide('DeleteModal' + item.id);

                    this.$router.push({ name: 'sspapplication' });
                })
                .catch((error) => {
                    this.showApiError(error);
                    this.DeleteLoading = false;
                });
        },
        Send(item) {
            this.SendLoading = true;
            MemshipApplicationService.Send(this.filter)
                .then((res) => {
                    this.SendLoading = false;
                    this.$bvModal.hide('SendModal' + item.id);
                    this.$router.push({ name: 'MemshipApplication' });
                    // this.Refresh();
                })
                .catch((error) => {
                    this.showApiError(error);
                    this.SendLoading = false;
                });
        },
        Revoke(item) {
            MemshipApplicationService.Revoke(this.filter)
                .then(() => {
                    this.CancelLoading = false;
                    this.$bvModal.hide('CancelModal' + item.id);
                    this.Refresh();
                })
                .catch((error) => {
                    this.showApiError(error);
                    this.$bvModal.hide('CancelModal' + item.id);
                    this.CancelLoading = false;
                });
        },
        AfterSave(id) {
            this.Loading = true;
            MemshipApplicationService.Get(id)
                .then((res) => {
                    this.Data = res.data;
                    if (this.Data.id) {
                        this.tabActive = 2;
                    }
                })
                .catch(this.showApiError)
                .finally(() => {
                    this.Loading = false;
                });
        },
        SaveData() {
            this.$refs.ValidationDTO.validateWithInfo().then(({ isValid, errors }) => {
                if (isValid) {
                    this.saveLoading = true;

                    MemshipApplicationService.Update(this.Data)
                        .then((res) => {
                            this.makeToast(this.$t('SaveSuccess'), 'success');
                            this.AfterSave(this.$route.params.id == 0 ? res.data.id : this.Data.id);
                            if (this.Data.memshipContractTypeId == 2) {
                                this.$router.push({
                                    name: 'MemshipContractEdit',
                                    params: { id: this.$route.params.id == 0 ? res.data.contractId : this.Data.memshipContractId }
                                });
                            } else {
                                this.$router.push({
                                    name: 'MemshipApplication'
                                });
                            }
                        })
                        .catch(this.showApiError)
                        .finally(() => {
                            this.saveLoading = false;
                        });
                } else {
                    this.showValidateError(errors);
                }
            });
        }
    }
};
</script>
