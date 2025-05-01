<template>
    <div>
        <AppListHeaderForName title="DualApplication" page-name="DualApplication" />

        <div class="container">
            <b-overlay :show="Loading" style="min-height: calc(100vh - 460px)">
                <b-row>
                    <b-col sm="12" lg="9">
                        <b-card class="form-card" v-if="Application.application">
                            <validation-observer ref="ValidationDTO">
                                <b-tabs pills card lazy>
                                    <b-tab active :title="$t('AdditionalInfo')">
                                        <b-row>
                                            <b-col sm="12" md="6" lg="6">
                                                <WInput
                                                    :label="$t('contractorInn')"
                                                    :name="$t('contractorInn')"
                                                    :placeholder="$t('contractorInn')"
                                                    disabled
                                                    v-model="Application.application.contractorInn"
                                                    rules="required"
                                                />
                                            </b-col>
                                            <b-col sm="12" md="6" lg="6">
                                                <WInput
                                                    :label="$t('contractor')"
                                                    :name="$t('contractor')"
                                                    :placeholder="$t('contractor')"
                                                    disabled
                                                    :value="
                                                        Application.application.contractorInn && Application.application.contractorInn.length == 14
                                                            ? Application.application.contractor + ' - ' + Application.application.contractorDirector
                                                            : Application.application.contractor
                                                    "
                                                    rules="required"
                                                />
                                            </b-col>
                                            <b-col sm="12" md="6" lg="6">
                                                <WInput
                                                    :label="$t('contractorDirector')"
                                                    :name="$t('contractorDirector')"
                                                    :placeholder="$t('contractorDirector')"
                                                    disabled
                                                    v-model="Application.application.contractorDirector"
                                                    rules="required"
                                                />
                                            </b-col>
                                            <b-col sm="12" md="6" lg="6">
                                                <AppContractorSettlementAccount v-model="Application.application.contractorSettlementAccountId" />
                                            </b-col>
                                            <b-col sm="12" md="6" lg="6">
                                                <WInput
                                                    :label="$t('docNumber')"
                                                    :name="$t('docNumber')"
                                                    :placeholder="$t('docNumber')"
                                                    disabled
                                                    v-model="Application.application.docNumber"
                                                    rules="required"
                                                />
                                            </b-col>
                                            <b-col sm="12" md="6" lg="6">
                                                <WInput
                                                    :label="$t('docOn')"
                                                    :name="$t('docOn')"
                                                    :placeholder="$t('docOn')"
                                                    disabled
                                                    v-model="Application.application.docOn"
                                                    rules="required"
                                                />
                                            </b-col>
                                            <b-col sm="12" md="6" lg="6">
                                                <WSelect
                                                    rules="required"
                                                    :disabled="!Application.canEdit"
                                                    :label="$t('dualEducationType')"
                                                    :name="$t('dualEducationType')"
                                                    :clearable="true"
                                                    :options="DualEducationTypeList"
                                                    v-model="Application.dualEducationTypeId"
                                                />
                                            </b-col>
                                        </b-row>
                                        <b-row class="mt-2">
                                            <p class="text-left address-text">{{ $t('address') }}</p>
                                            <b-col sm="12" md="3" lg="3">
                                                <WInput
                                                    :label="$t('region')"
                                                    :name="$t('region')"
                                                    :placeholder="$t('region')"
                                                    disabled
                                                    v-model="Application.application.region"
                                                    rules="required"
                                                />
                                            </b-col>
                                            <b-col sm="12" md="3" lg="3">
                                                <WInput
                                                    :label="$t('district')"
                                                    :name="$t('district')"
                                                    :placeholder="$t('district')"
                                                    disabled
                                                    v-model="Application.application.district"
                                                    rules="required"
                                                />
                                            </b-col>
                                            <b-col sm="12" md="6" lg="6">
                                                <WInput
                                                    :label="$t('contractorAddress')"
                                                    :name="$t('contractorAddress')"
                                                    :placeholder="$t('contractorAddress')"
                                                    disabled
                                                    v-model="Application.application.contractorAddress"
                                                    rules="required"
                                                />
                                            </b-col>
                                        </b-row>

                                        <validation-observer ref="ValidationTable" disabled>
                                            <b-row class="align-items-center">
                                                <hr />
                                                <p class="text-left address-text">{{ $t('tables') }}</p>
                                                <b-col sm="12" md="4" lg="4" v-if="Application.id == 0 || Application.canEdit">
                                                    <WSelect
                                                        rules="required"
                                                        @option:selected="(e) => (tabrow.positionClassification = e ? e.text : '')"
                                                        @input="(e) => (!e ? (tabrow.positionClassification = '') : '')"
                                                        :label="$t('position')"
                                                        :name="$t('position')"
                                                        :clearable="true"
                                                        :options="PositionList"
                                                        v-model="tabrow.positionClassificationId"
                                                    />
                                                </b-col>
                                                <b-col sm="12" md="4" lg="4" v-if="Application.id == 0 || Application.canEdit">
                                                    <WSelect
                                                        rules="required"
                                                        @option:selected="((e) => (tabrow.institute = e ? e.text : ''), ChangeInstitute)"
                                                        :label="$t('institute')"
                                                        :name="$t('institute')"
                                                        :clearable="true"
                                                        :options="InstituteList"
                                                        v-model="tabrow.instituteId"
                                                    />
                                                </b-col>
                                                <b-col sm="12" md="4" lg="4" v-if="Application.id == 0 || Application.canEdit">
                                                    <div class="w-select">
                                                        <div class="pb-3">
                                                            {{ $t('specialty') }}

                                                            <span class="text-danger">*</span>
                                                        </div>
                                                        <v-select
                                                            :reduce="
                                                                (option) => {
                                                                    return option.id;
                                                                }
                                                            "
                                                            :options="SpecialtyList"
                                                            label="shortName"
                                                            :placeholder="$t('c\hoose')"
                                                            v-model="tabrow.specialtyId"
                                                            @input="onSelect"
                                                        />
                                                    </div>
                                                </b-col>

                                                <b-col sm="12" md="4" lg="4" v-if="Application.id == 0 || Application.canEdit">
                                                    <WInput
                                                        rules="required"
                                                        v-mask="'#####################'"
                                                        :label="$t('emptyPositionsCount')"
                                                        :name="$t('emptyPositionsCount')"
                                                        placeholder="1234"
                                                        v-model="tabrow.emptyPositionsCount"
                                                    />
                                                </b-col>
                                                <b-col sm="12" md="4" lg="4" v-if="Application.id == 0 || Application.canEdit">
                                                    <WInput rules="required" :label="$t('details')" :name="$t('details')" placeholder="details" v-model="tabrow.details" />
                                                </b-col>
                                                <b-col sm="12" md="4" lg="4" style="text-align: left !important">
                                                    <b-button v-if="Application.id == 0 || Application.canEdit" variant="success" @click="AddTabrow">
                                                        <b-icon-plus></b-icon-plus> {{ $t('Add') }}
                                                    </b-button>
                                                </b-col>
                                            </b-row>
                                        </validation-observer>

                                        <b-row>
                                            <b-col sm="12" lg="12" class="mt-2">
                                                <b-table :items="Application.tables" :fields="fields" responsive striped bordered>
                                                    <template #cell(actions)="{ item, index }">
                                                        <div>
                                                            <b-link v-if="Application.canSend || Application.id == 0 || Application.canEdit" @click="DeleteItem(item, index)">
                                                                <b-icon-trash-fill style="width: 20px; height: 20px"></b-icon-trash-fill>
                                                            </b-link>
                                                        </div>
                                                    </template>
                                                </b-table>
                                            </b-col>
                                        </b-row>
                                    </b-tab>
                                </b-tabs>
                            </validation-observer>
                        </b-card>
                    </b-col>
                    <b-col sm="12" lg="3">
                        <b-card class="form-card">
                            <b-card-text>
                                <!-- save -->
                                <b-button
                                    @click="SaveData"
                                    :disabled="saveLoading"
                                    v-if="Application.canSend || Application.id == 0 || Application.canEdit"
                                    variant="success"
                                    block
                                    class="mb-1"
                                >
                                    <b-icon-check /> {{ $t('save') }}
                                </b-button>
                                <!-- send -->
                                <b-button @click="$bvModal.show('ESPmodal')" variant="primary" block class="mb-1">
                                    <b-icon-arrow-bar-up scale="0.6"></b-icon-arrow-bar-up>
                                    {{ $t('Send') }}
                                </b-button>
                                <!-- canRevoke  -->
                                <b-button v-if="Application.canRevoke" @click="OpenRevokeModal(Application)" variant="warning" block class="mb-1">
                                    <b-icon-x-circle scale="0.6"></b-icon-x-circle>
                                    {{ $t('Revoke') }}
                                </b-button>
                            </b-card-text>
                        </b-card>
                    </b-col>
                </b-row>
            </b-overlay>
            <b-modal :id="'DeleteModal' + Application.id" :title="$t('delete')" no-close-on-backdrop hide-footer>
                <p>{{ $t('WantDeleteAdm') }}</p>
                <b-row>
                    <b-col style="text-align: end" class="text-right">
                        <a @click="$bvModal.hide('DeleteModal' + Application.id)" style="margin-right: 5px" class="btn btn-sm btn-soft-danger mr-2 pr-btn">{{ $t('no') }}</a>
                        <a @click="Delete(Application)" class="btn btn-sm btn-success pr-btn">
                            <b-spinner v-if="DeleteLoading" small></b-spinner>
                            {{ $t('yes') }}
                        </a>
                    </b-col>
                </b-row>
            </b-modal>
            <b-modal :id="'SendModal' + Application.id" :title="$t('send')" no-close-on-backdrop hide-footer>
                <p>{{ $t('wantSend') }}</p>

                <b-row class="mt-3">
                    <b-col style="text-align: end" class="text-right">
                        <a @click="$bvModal.hide('SendModal' + Application.id)" class="btn btn-sm btn-soft-danger mr-2 pr-btn" style="margin-right: 5px">{{ $t('no') }}</a>

                        <b-button variant="success" @click="Send(Application)">
                            <b-spinner v-if="SendLoading" small></b-spinner>
                            {{ $t('yes') }}
                        </b-button>
                    </b-col>
                </b-row>
            </b-modal>

            <b-modal :id="'CancelModal' + Application.id" :title="$t('Revoke')" no-close-on-backdrop hide-footer>
                <p>{{ $t('WantRevoke') }}</p>
                <b-row>
                    <b-col style="text-align: end" class="text-right">
                        <a @click="$bvModal.hide('CancelModal' + Application.id)" class="btn btn-sm btn-soft-danger mr-2 pr-btn" style="margin-right: 5px">{{ $t('no') }}</a>
                        <a @click="Revoke(Application)" class="btn btn-sm btn-success pr-btn">
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
                    </b-col>
                </b-row>
            </b-modal>
        </div>
    </div>
</template>

<script>
import DualApplicationService from '@/services/dual/dualapplication.service';
import ManualService from '@/services/manual.service';
import AppListHeaderForName from '@/components/application/AppListHeaderForName.vue';
import WInput from '@/components/forms/WInput.vue';
import WSelect from '@/components/forms/WSelect.vue';
import AppWord from '@/components/word/AppWord.vue';
import AppContractorSettlementAccount from '@/components/application/AppContractorSettlementAccount.vue';
import JustSign from '@/components/justSign.vue';
import VSelect from 'vue-select';
const tabrowDef = {
    id: 0,
    ownerId: 0,
    orderNumber: '',
    positionClassificationId: 0,
    instituteId: 0,
    specialtyId: null,
    positionClassification: '',
    institute: '',
    specialty: '',
    emptyPositionsCount: 0,
    details: ''
};
export default {
    components: {
        JustSign,
        VSelect,
        AppListHeaderForName,
        WSelect,
        WInput,
        AppWord,
        AppContractorSettlementAccount
    },
    data() {
        return {
            Application: {},
            PositionList: [],
            InstituteList: [],
            SpecialtyList: [],
            DualEducationTypeList: [],
            ApplicationHTMLData: {},
            fields: [
                {
                    key: 'positionClassification',
                    label: this.$t('position'),
                    sortable: false,
                    tdClass: 'text-center',
                    thClass: 'text-center'
                },
                {
                    key: 'institute',
                    label: this.$t('institute'),
                    sortable: false,
                    tdClass: 'text-center',
                    thClass: 'text-center'
                },

                {
                    key: 'specialty',
                    label: this.$t('specialty'),
                    sortable: false,
                    tdClass: 'text-center',
                    thClass: 'text-center'
                },
                {
                    key: 'emptyPositionsCount',
                    label: this.$t('emptyPositionsCount'),
                    sortable: false,
                    tdClass: 'text-center',
                    thClass: 'text-center'
                },

                {
                    key: 'details',
                    label: this.$t('details'),
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
            filter: {
                signedData: '',
                isPinfl: true,
                id: 1,
                message: ''
            },
            tabrow: { ...tabrowDef },
            Loading: false,
            orgName: '',
            saveLoading: false,
            DeleteLoading: false,
            SendLoading: false,
            CancelLoading: false
        };
    },
    computed: {
        canEdit() {
            return !this.Application.canSend || this.Application.id == 0 || this.Application.canEdit;
        }
    },
    created() {
        this.Refresh();
        ManualService.DualEducationTypeSelectList().then((res) => {
            this.DualEducationTypeList = res.data;
        });
        ManualService.GetAsSelectList().then((res) => {
            this.PositionList = res.data;
        });
        ManualService.GetBillingUniversityList().then((res) => {
            this.InstituteList = res.data;
        });
    },
    methods: {
        ChangeInstitute(e) {
            this.tabrow.specialtyId = null;
            if (!e) {
                tabrow.institute = '';
            }
            if (e) {
                ManualService.GetBillingSpecialityList(e.value).then((res) => {
                    this.SpecialtyList = res.data.specialities;
                });
                ManualService.GetUniversity(e.value).then((res) => {
                    ManualService.CreateInstituteBilling(res.data).then((res) => {});
                });
            }
        },
        DeleteItem(item, index) {
            this.Application.tables.splice(index, 1);
            this.calculate();
        },
        AddTabrow() {
            this.$refs.ValidationTable.validate().then((success) => {
                if (success) {
                    var self = this;
                    self.tabrow.orderNumber = self.Application?.tables.length + 1 + '';
                    self.Application.tables.push(self.tabrow);
                }
                self.tabrow = { ...tabrowDef };
                this.$refs.ValidationTable.reset();
            });
        },
        Refresh() {
            this.Loading = true;
            DualApplicationService.Get(this.$route.params.id)
                .then((res) => {
                    this.Application = res.data;
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
                    DualApplicationService.Update(this.Application)
                        .then(() => {
                            this.$router.push({ name: 'DualApplication' });
                            this.makeToast(this.$t('SaveSuccess'), 'success');
                        })
                        .catch(this.showApiError)
                        .finally(() => {
                            this.saveLoading = false;
                        });
                } else {
                    this.showValidateError(errors);
                }
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
        GetESP(data) {
            this.$bvModal.hide('ESPmodal');

            this.Sign(data);
        },
        Sign(item) {
            this.SendLoading = true;
            DualApplicationService.Send({ id: this.Application.id, message: this.filter.message, signedData: item.key })
                .then(() => {
                    this.$bvModal.hide('SendModal' + item.id);
                    this.makeToast(this.$t('SuccessSend'), 'success');
                    this.$router.push({ name: 'DualApplication' });
                })
                .catch((error) => {
                    this.showApiError(error);
                })
                .finally(() => {
                    this.SendLoading = false;
                });
        },
        Revoke(item) {
            DualApplicationService.Revoke({ id: this.Application.id, message: this.filter.message })
                .then((res) => {
                    this.$bvModal.hide('CancelModal' + this.item.id);
                    this.Refresh();
                })
                .catch((error) => {
                    this.showApiError(error);
                })
                .finally(() => {
                    this.SendLoading = false;
                });
        },
        onSelect(e) {
            console.log(e);

            if (e) {
                this.tabrow.specialty = this.SpecialtyList.filter((item) => item.id == e)[0].shortName;
                ManualService.GetSpeciality(e).then((res) => {
                    ManualService.CreateSpecialtyBilling(res.data);
                });
            } else {
                this.tabrow.specialty = '';
            }
        }
    }
};
</script>
<style lang="scss" scoped>
@import '../../../components/forms/style.scss';

.w-select {
    :deep(.v-select) {
        .vs__dropdown-toggle {
            border: 1px solid rgba(38, 41, 45, 0.1);
            border-radius: 6px;
            background-color: #fff;
            padding: 10px;

            &:focus-within {
                border-color: transparent;
                box-shadow: 0 0 0 0.2rem rgba($primary, 0.25);
            }
            .vs__selected-options {
                input {
                    padding-left: 6px;
                    padding-bottom: 6px;
                    padding-top: 6px;
                    margin-bottom: 0px;
                    background-color: #fff;
                    margin-top: 0;
                    &::placeholder {
                        font-size: 15px;
                        color: $input-text-placeholder;
                    }
                }
                .vs__selected {
                    padding-left: 0px;
                    margin-top: 0;
                    margin-bottom: 0px;
                }
            }
            .vs__actions {
                .vs__open-indicator {
                    margin-top: 0px;
                    margin-right: 15px;
                }
                .vs__clear {
                    margin-top: 0px;
                    margin-right: 15px;
                }
            }
        }
        .vs__dropdown-menu {
            margin-top: 16px;
            border-radius: 12px;
        }
    }

    &.is-invalid {
        :deep(.vs__dropdown-toggle) {
            border: $border-width solid $form-feedback-invalid-color;
            &:focus-within {
                border: $border-width solid $form-feedback-invalid-color;
                box-shadow: 0 0 0 0.2rem rgba($red, 0.25);
            }
        }
    }
    &:disabled {
        :deep(.vs__dropdown-toggle) {
            background-color: $input-disabled-bg !important;
        }

        :deep(.vs__open-indicator),
        :deep(.vs__search) {
            background-color: $input-disabled-bg !important;
        }
    }
}
</style>
