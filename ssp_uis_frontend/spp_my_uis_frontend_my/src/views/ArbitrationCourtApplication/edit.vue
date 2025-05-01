<template>
    <div>
        <AppListHeaderForName :title="$t('ArbitrationCourtApplication')" page-name="ArbitrationCourtApplication" />
        <div class="container">
            <div v-if="$route.query.isProcess">
                <div>
                    <div class="flex-container container">
                        <div
                            class="step"
                            v-for="(step, i) in steps"
                            :key="i"
                            :class="{
                                done: step.number < currentStep,
                                current: step.number === currentStep
                            }"
                        >
                            <div class="step-number" :id="'step-' + step.number" @click="moveStep(step.number)">
                                <i v-if="step.number <= currentStep"><b-icon-check-lg scale="0.8"></b-icon-check-lg></i>
                            </div>
                            <div class="step-label">{{ step.label }}</div>
                        </div>
                    </div>
                </div>
            </div>
            <b-overlay :show="Loading">
                <b-row class="justify-content-center">
                    <b-col sm="12" lg="9">
                        <b-card class="form-card">
                            <validation-observer ref="ValidationDTO" v-if="Data.application">
                                <b-card-text>
                                    <b-row>
                                        <b-col sm="12" md="12" lg="12">
                                            <h3>{{ $t('Davogar') }}</h3>
                                        </b-col>
                                        <hr />
                                        <b-col sm="12" md="6" lg="6">
                                            <WInput rules="required" :label="$t('contractorInn')" name="contractorInn" disabled v-model="Data.application.contractorInn" />
                                        </b-col>
                                        <b-col sm="12" md="6" lg="6">
                                            <WInput rules="required" :label="$t('contractor')" name="contractor" disabled v-model="Data.application.contractor" />
                                        </b-col>
                                        <b-col sm="12" md="6" lg="6">
                                            <WInput
                                                rules="required"
                                                :label="$t('docNumberApplication')"
                                                name="docNumberApplication"
                                                disabled
                                                v-model="Data.application.docNumber"
                                            />
                                        </b-col>
                                        <b-col sm="12" md="6" lg="6">
                                            <WInput rules="required" :label="$t('docDateApplication')" name="docDateApplication" disabled v-model="Data.application.docOn" />
                                        </b-col>
                                        <b-col sm="12" md="6" lg="6">
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
                                        <b-col sm="12" md="6" lg="6">
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

                                        <b-col sm="12" md="6" lg="6">
                                            <WInput
                                                rules="required"
                                                :label="$t('contractorDirector')"
                                                name="contractorDirector"
                                                placeholder
                                                disabled
                                                v-model="Data.application.contractorDirector"
                                            />
                                        </b-col>

                                        <b-col sm="12" md="6" lg="6">
                                            <WInput
                                                rules="required"
                                                :label="$t('contractorAddress')"
                                                name="contractorAddress"
                                                placeholder
                                                disabled
                                                v-model="Data.contractorAddress"
                                            />
                                        </b-col>
                                        <b-col sm="12" md="6" lg="6">
                                            <WPhoneInput
                                                :label="$t('WorkPhoneNumber')"
                                                name="contractorWorkPhoneNumber"
                                                rules="required|validatorPhone"
                                                placeholder
                                                v-model="Data.contractorPhonber"
                                            />
                                        </b-col>

                                        <b-col sm="12" md="6" lg="6" class="mt-2">
                                            <h6 class="inputTitle">
                                                <span>*</span>
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
                                                <b-button v-if="canUpdate" :variant="item.isCreatedByErp ? 'danger' : 'info'" size="sm" class="ml-1" @click="DeleteFile(item.id)">
                                                    <b-icon-trash scale="0.7" />
                                                </b-button>
                                            </div>
                                        </b-col>

                                        <b-col sm="12" md="6" lg="6">
                                            <WSelect
                                                rules="required"
                                                v-model="Data.contractorResponsibleTypeId"
                                                :options="ContractorResponsibleTypeList"
                                                :label="$t('contractorResponsibleType')"
                                                clearable
                                                :name="$t('contractorResponsibleType')"
                                            />
                                        </b-col>
                                        <b-col sm="12" md="6" lg="6">
                                            <WSelect
                                                rules="required"
                                                v-model="Data.arbitrationApplicationTypeId"
                                                :options="ArbitrationApplicationTypeList"
                                                :label="$t('arbitrationApplicationType')"
                                                clearable
                                                :name="$t('arbitrationApplicationType')"
                                            />
                                        </b-col>
                                        <b-col sm="12" md="6" lg="6">
                                            <WSelect
                                                rules="required"
                                                v-model="Data.organizationId"
                                                :options="OrganizationList"
                                                :label="$t('arbitrationCourt')"
                                                clearable
                                                :name="$t('arbitrationCourt')"
                                            />
                                        </b-col>

                                        <b-col sm="12" md="12" lg="12">
                                            <h3>{{ $t('Javobgar') }}</h3>
                                        </b-col>
                                        <hr />
                                        <b-col sm="12" md="6" lg="6" class="mt-4">
                                            <WSelect
                                                rules="required"
                                                v-model="Data.responsibleTypeId"
                                                :options="ContractorResponsibleTypeList"
                                                :label="$t('responsibleType')"
                                                clearable
                                                :name="$t('responsibleType')"
                                            />
                                        </b-col>
                                        <b-col sm="12" md="6" lg="6" class="d-flex align-items-center mt-4">
                                            <WInput
                                                v-mask="'##############'"
                                                rules="required"
                                                @keyup.enter="GetByInn"
                                                :label="$t('responsibleInnPnfl')"
                                                :name="$t('responsibleInnPnfl')"
                                                v-model="Data.responsibleInnPnfl"
                                            />
                                            <b-button @click="GetByInn" variant="primary" class="p-3 mt-3">
                                                <b-icon-search scale="0.6"></b-icon-search>
                                            </b-button>
                                        </b-col>
                                        <b-col sm="12" md="6" lg="6">
                                            <WInput rules="required" :label="$t('responsible')" :name="$t('responsible')" disabled v-model="Data.responsible" />
                                        </b-col>
                                        <b-col sm="12" md="6" lg="6">
                                            <WInput
                                                rules="required"
                                                :label="$t('responsibleAddress')"
                                                :name="$t('responsibleAddress')"
                                                placeholder
                                                disabled
                                                v-model="Data.responsibleAddress"
                                            />
                                        </b-col>

                                        <b-col sm="12" md="6" lg="6">
                                            <WPhoneInput
                                                :label="$t('WorkPhoneNumber')"
                                                :name="$t('WorkPhoneNumber')"
                                                rules="required|validatorPhone"
                                                placeholder
                                                v-model="Data.responsiblePhonber"
                                            />
                                        </b-col>
                                        <b-col sm="12" md="6">
                                            <WSelect v-model="Data.currencyId" :options="CurrencySelectList" rules="required" :label="$t('currency')" :name="$t('currency')" />
                                        </b-col>
                                        <b-col sm="12" md="6" lg="6">
                                            <WCurrencyInput :label="$t('amount1')" :name="$t('amount1')" placeholder v-model="Data.amount" />
                                        </b-col>
                                        <b-col sm="12" md="6" lg="6">
                                            <WCurrencyInput :label="$t('arbitrationAmount')" :name="$t('arbitrationAmount')" placeholder v-model="Data.arbitrationAmount" />
                                        </b-col>
                                    </b-row>
                                </b-card-text>
                            </validation-observer>
                        </b-card>
                    </b-col>
                    <b-col sm="12" lg="3" v-if="!$route.query.isProcess">
                        <b-card class="form-card">
                            <b-card-text>
                                <b-button @click="SaveData" v-if="canUpdate" variant="success" block class="mb-1">
                                    <b-spinner v-if="saveLoading" small></b-spinner>
                                    <b-icon-check scale="0.8"></b-icon-check>
                                    {{ $t('save') }}
                                </b-button>

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
        </div>
    </div>
</template>

<script>
import WCurrencyInput from '@/components/forms/WCurrencyInput.vue';
import WSelect from '@/components/forms/WSelect.vue';
import WTextarea from '@/components/forms/WTextarea.vue';
import WInput from '@/components/forms/WInput.vue';
import WPhoneInput from '@/components/forms/WPhoneInput.vue';
import JustSign from '@/components/justSign.vue';
import ArbitrationCourtApplicationService from '@/services/hakamliksudi/arbitrationcourtapplication.service';
import MemshipApplicationService from '@/services/memshipapplication.service';
import AccountService from '@/services/account.service';
import ManualService from '@/services/manual.service';
import AppListHeaderForName from '@/components/application/AppListHeaderForName.vue';
import axios from 'axios';
import AppContractorSettlementAccount from '@/components/application/AppContractorSettlementAccount.vue';
import OrganizationService from '@/services/organization.service.js';

export default {
    components: {
        WCurrencyInput,
        WSelect,
        WTextarea,
        WInput,
        WPhoneInput,
        JustSign,
        AppListHeaderForName,
        AppContractorSettlementAccount
    },

    data() {
        return {
            createApplicationForBig: false,
            steps: [{ label: this.$t('step1') }, { label: this.$t('step2') }, { label: this.$t('step3') }, { label: this.$t('step4') }, { label: this.$t('step5') }],
            pBarSize: '',
            currentStep: 1,
            axios,
            Data: {},
            tabActive: 1,
            ContractorActivityTypeList: [],
            NeedChamberServiceSelectList: [],
            ContractorCategoryList: [],
            CurrencySelectList: [],
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
            OrganizationList: [],
            List: [],
            ContractorResponsibleTypeList: [],
            ArbitrationApplicationTypeList: [],
            ApplicationTypeStepList: [],
            // ArbitrationCourtList: [],

            Loading: false,
            isContractCreate: false,
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
        FileSrc() {
            return (id) => axios.defaults.baseURL + `ArbitrationCourtApplication/DownloadFile/${id}`;
        },
        canUpdate() {
            return !this.Data.canSend || this.Data.id == 0 || this.Data.canEdit;
        },
        // eslint-disable-next-line vue/return-in-computed-property
        progress() {
            if (this.$route.query.isProcess) {
                if (this.currentStep > this.steps.length) {
                    return `width: 100%`;
                }
                const first = document.getElementById('step-1');
                const current = document.getElementById(`step-${this.currentStep}`);
                if (first && current) {
                    const delta = current.getBoundingClientRect().right - first.getBoundingClientRect().right;
                    return `width: ${delta}px;`;
                }
            }
        }
    },

    watch: {
        historySidebar: {
            handler(newValue) {
                if (newValue) {
                    DocumentChatService.GetList(this.chatFilter)
                        .then((res) => {
                            this.chatData = res.data.rows;
                        })
                        .catch((error) => {
                            this.showApiError(error);
                        });
                }
            }
        }
    },
    created() {
        this.Refresh();
        ManualService.CurrencySelectList().then((res) => {
            if (Array.isArray(res.data)) {
                this.CurrencySelectList = res.data;
            }
        });
        ManualService.ApplicationTypeStepSelectList(8).then((res) => {
            if (Array.isArray(res.data)) {
                this.ApplicationTypeStepList = res.data;
            }
        });
        ManualService.NeedChamberServiceSelectList().then((res) => {
            this.NeedChamberServiceSelectList = res.data;
        });
        OrganizationService.OrganizationAsSelectListByGroup().then((res) => {
            this.OrganizationList = res.data;
        });
        ManualService.RegionSelectList().then((res) => {
            this.RegionList = res.data;
        });

        ManualService.ContractorActivityTypeSelectList().then((res) => {
            this.ContractorActivityTypeList = res.data;
        });

        ManualService.ClaimResponsibleTypeSelectList().then((res) => {
            this.ContractorResponsibleTypeList = res.data;
        });

        ManualService.ContractorCategorySelectList().then((res) => {
            this.ContractorCategoryList = res.data;
        });
        ManualService.ArbitrationApplicationTypeSelectList().then((res) => {
            this.ArbitrationApplicationTypeList = res.data;
        });
    },
    mounted() {
        if (!this.steps || this.steps.length == 0) {
            return;
        }

        this.steps = this.steps.map((s, i) => ({
            number: i + 1,
            selected: false,
            ...s
        }));

        this.steps[0].selected = true;

        this.$nextTick(() => {
            this.calculateBarPosition();
        });

        window.addEventListener('resize', this.calculateBarPosition);
    },
    beforeDestroy() {
        window.removeEventListener('resize', this.calculateBarPosition);
    },
    methods: {
        moveStep(stepNumber) {
            if (stepNumber <= this.Data.application.currentStep.id) {
                this.currentStep = stepNumber;
            }
        },
        calculateBarPosition() {
            if (this.$route.query.isProcess) {
                let docEl = document.documentElement;
                const first = document.getElementById('step-1');
                let rect = first.getBoundingClientRect();
                const offset = rect.left + (window.scrollX || docEl.scrollLeft || 0);
                const top = rect.top + rect.height / 2 - 2;
                this.pBarSize = `left: ${0}px; right: ${0}px;`;
            }
        },

        GetByInn() {
            AccountService.SearchByInnPnfl(this.Data.responsibleInnPnfl).then((res) => {
                this.$set(this.Data, 'responsibleAddress', res.data.address);
                this.$set(this.Data, 'responsibleContractorId', res.data.id);
                this.$set(this.Data, 'responsible', res.data.shortName);
                this.$set(this.Data.application, 'responsiblePhonber', res.data.phoneNumberResponsibleContractorId);
            });
        },
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

        UploadFile(event) {
            const formData = new FormData();
            formData.append('files', event.target.files[0]);
            this.fileLoading = true;
            ArbitrationCourtApplicationService.UploadFile(formData)
                .then((res) => {
                    const updatedFiles = res.data.map((file) => ({
                        ...file,
                        columnName: `${this.Data.application.currentStep.id}`
                    }));

                    this.Data.files.push(...updatedFiles);
                })
                .catch(this.showApiError)
                .finally(() => {
                    this.fileLoading = false;
                });
        },
        DeleteFile(id) {
            ArbitrationCourtApplicationService.DeleteFile(id).then(() => {
                this.Data.files = this.Data.files.filter((item) => item.id != id);
            });
        },
        Refresh() {
            this.Loading = true;
            ArbitrationCourtApplicationService.Get(this.$route.params.id)
                .then((res) => {
                    this.Data = res.data;
                    this.currentStep = this.Data.application.currentStep.id;
                    if (this.Data.id) {
                        this.tabActive = 2;
                    }
                    if (this.Data.application && this.Data.application.regionId) {
                        this.ChangeRegion(this.Data.application.regionId);
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

        SaveData() {
            this.$refs.ValidationDTO.validateWithInfo().then(({ isValid, errors }) => {
                if (isValid) {
                    this.saveLoading = true;
                    this.isContractCreate = this.Data.contractorCategoryId != 4 ? true : false;
                    ArbitrationCourtApplicationService.Update(this.Data)
                        .then((res) => {
                            this.makeToast(this.$t('SaveSuccess'), 'success');
                            this.$router.push({ name: 'ArbitrationCourtApplication' });
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
<style lang="scss" scoped>
/* vue-stepper styling */
.container {
    .flex-container {
        display: flex;
        flex-direction: row;
        justify-content: space-between;
        padding: 1em;
        position: relative;

        .step {
            text-align: center;
            z-index: 2;
            position: relative;

            &:first-child {
                &::before {
                    width: 50%;
                }
                &::after {
                    width: 0;
                }
            }

            &:last-child {
                &::before {
                    width: 50%;
                    left: 0;
                }
                &::after {
                    width: 0;
                }
            }

            &::before,
            &::after {
                content: '';
                position: absolute;
                top: 1em;
                height: 5px;
                width: 50%;
                background-color: #cdcdcd;
                z-index: -1;
            }

            &::before {
                right: 0;
            }

            &::after {
                left: 0;
            }

            &.done {
                &::before {
                    background-color: #46c0bd;
                }

                .step-number {
                    background-color: #46c0bd;
                }
            }

            &.done + .step {
                &::after {
                    background-color: #46c0bd;
                }
                &:last-child::before {
                    background-color: #46c0bd;
                }
            }

            .step-number {
                cursor: pointer;
                background-color: #cdcdcd;
                display: inline-block;
                padding: 0.5em;
                color: white;
                border-radius: 2em;
                width: 40px;
                height: 40px;
                z-index: 3;
                background-size: 0% 0%;
                background-position: center;
                background-image: radial-gradient(circle at center, #46c0bd 50%, transparent 50%);
                background-repeat: no-repeat;
            }

            &.current {
                .step-number {
                    background-size: 200% 200%;
                    transition: all 0.3s;
                }
            }

            .step-label {
                padding-top: 5px;
            }
        }
    }
}
</style>
