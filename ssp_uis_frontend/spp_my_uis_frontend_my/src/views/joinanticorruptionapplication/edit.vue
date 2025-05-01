<template>
    <div>
        <AppListHeaderForName title="JoinAntiCorruptionApplication" />
        <div class="container">
            <b-overlay :show="Loading" style="min-height: calc(100vh - 460px)">
                <b-row class="justify-content-center">
                    <b-col sm="12" lg="9">
                        <validation-observer ref="ValidationDTO">
                            <b-card class="form-card">
                                <b-tabs v-model="tab" pills lazy card @change="iframeLoaded = false">
                                    <!-- form -->
                                    <b-tab active :title="$t('AdditionalInfo')">
                                        <b-card-text>
                                            <b-row>
                                                <b-col sm="12" md="6" lg="6">
                                                    <WInput
                                                        rules="required"
                                                        :label="$t('inn')"
                                                        name="inn"
                                                        disabled
                                                        :value="Data.application.contractorPinfl || Data.application.contractorInn"
                                                    />
                                                </b-col>
                                                <b-col sm="12" md="6" lg="6">
                                                    <WInput rules="required" :label="$t('contractor')" name="contractor" disabled v-model="Data.application.contractor" />
                                                </b-col>
                                                <b-col sm="12" md="6" lg="6">
                                                    <AppContractorSettlementAccount v-model="Data.application.contractorSettlementAccountId" />
                                                </b-col>
                                                <b-col sm="12" md="6" lg="6">
                                                    <WInput rules="required" :label="$t('documentnumber')" name="documentnumber" v-model="Data.application.docNumber" />
                                                </b-col>
                                                <b-col sm="12" md="6" lg="6">
                                                    <WInput rules="required" :label="$t('docOn')" name="docOn" disabled v-model="Data.application.docOn" />
                                                </b-col>
                                                <b-col sm="12" md="6" lg="6">
                                                    <WInput :label="$t('contractorDirector')" name="contractorDirector" disabled v-model="Data.application.contractorDirector" />
                                                </b-col>
                                                <b-col sm="12" md="6" lg="6">
                                                    <WInput v-model="Data.address" :label="$t('address')" name="address" rules="required" />
                                                </b-col>
                                            </b-row>

                                            <b-row>
                                                <hr />
                                                <b-col sm="12" md="6" lg="6">
                                                    <WSelect
                                                        v-model="Data.corruptionReviewTypeId"
                                                        :options="CorruptionReviewTypeSelectList"
                                                        :label="$t('corruptionReviewType')"
                                                        rules="required"
                                                        clearable
                                                        :name="$t('corruptionReviewType')"
                                                    />
                                                </b-col>
                                                <b-col sm="12" md="6" lg="6">
                                                    <WSelect
                                                        v-model="Data.contractorActivityTypeId"
                                                        :options="ContractorActivityTypeSelectList"
                                                        :label="$t('contractorActivityType')"
                                                        rules="required"
                                                        clearable
                                                        :name="$t('contractorActivityType')"
                                                    />
                                                </b-col>
                                                <b-col sm="12" md="6" lg="6">
                                                    <WInput
                                                        v-model="Data.unionMemberCount"
                                                        :label="$t('unionMemberCount')"
                                                        :name="$t('unionMemberCount')"
                                                        type="number"
                                                        rules="required|numeric"
                                                    />
                                                </b-col>

                                                <b-col sm="12" md="6" lg="6">
                                                    <WInput v-model="Data.details" :label="$t('comment')" :name="$t('comment')" />
                                                </b-col>
                                            </b-row>
                                            <b-row>
                                                <b-col sm="12" md="6" lg="6">
                                                    <WCurrencyInput v-model="Data.prevYearlyEarnings" :label="$t('prevYearlyEarnings')" :name="$t('prevYearlyEarnings')" />
                                                </b-col>
                                                <b-col sm="12" md="6">
                                                    <WSelect v-model="Data.currencyId" :options="CurrencySelectList" :label="$t('currency')" :name="$t('currency')" />
                                                </b-col>
                                                <b-col sm="12" md="6" lg="6">
                                                    <WInput
                                                        v-model="Data.avgEmployeesCount"
                                                        :label="$t('avgEmployeesCount')"
                                                        :name="$t('avgEmployeesCount')"
                                                        type="number"
                                                        rules="required|numeric"
                                                    />
                                                </b-col>
                                            </b-row>

                                            <b-row>
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
                                    </b-tab>

                                    <!-- word  -->
                                    <b-tab no-body :title="$t('Application')">
                                        <b-row class="mt-3 mb-3">
                                            <b-overlay :show="!iframeLoaded" spinner-variant="primary" spinner-type="grow" rounded="sm">
                                                <iframe
                                                    v-if="Data.application && Data.application.id2"
                                                    :src="IframeSrc"
                                                    width="100%"
                                                    style="height: 100vh"
                                                    frameborder="0"
                                                    @load="iframeLoaded = true"
                                                ></iframe>
                                            </b-overlay>
                                        </b-row>
                                    </b-tab>
                                </b-tabs>
                            </b-card>

                            <!-- tables -->
                            <template v-if="tab == 0">
                                <!-- employees table --------------------------------------------------- -->
                                <b-card class="mt-3 form-card">
                                    <b-card-header v-b-toggle.accordion-1 role="tab" class="pa-0 bg-transparent cursor-pointer">
                                        <p class="text-left address-text mb-0">
                                            {{ $t('employeesInfo') }}
                                        </p>
                                    </b-card-header>
                                    <b-collapse visible id="accordion-1" accordion="my-accordion">
                                        <validation-observer v-if="canUpdate" ref="ValidationEmployees" disabled>
                                            <b-row class="mt-2">
                                                <b-col sm="12" md="4">
                                                    <WInput
                                                        :name="$t('employee')"
                                                        v-model="employees.person"
                                                        :label="$t('employee')"
                                                        :placeholder="$t('employee')"
                                                        rules="required"
                                                    />
                                                </b-col>
                                                <b-col sm="12" md="4">
                                                    <WInput
                                                        :name="$t('position')"
                                                        v-model="employees.position"
                                                        :label="$t('position')"
                                                        :placeholder="$t('position')"
                                                        rules="required"
                                                    />
                                                </b-col>
                                                <b-col sm="12" md="4">
                                                    <WPhoneInput
                                                        v-model="employees.phoneNumber"
                                                        :label="$t('phonenumber')"
                                                        :name="$t('phonenumber')"
                                                        :placeholder="$t('+998')"
                                                        :mask="'+998 (##) ### ## ##'"
                                                        rules="required"
                                                    />
                                                </b-col>
                                                <b-col sm="12" md="4">
                                                    <WInput v-model="employees.email" rules="required|email" :label="$t('email')" :name="$t('email')" :placeholder="$t('email')" />
                                                </b-col>
                                                <b-col sm="12" md="4" class="align-self-center">
                                                    <b-button @click="AddEmployee" variant="success">
                                                        <b-icon-plus></b-icon-plus>
                                                        {{ $t('Add') }}
                                                    </b-button>
                                                </b-col>
                                            </b-row>
                                        </validation-observer>

                                        <b-table
                                            :items="Data.employees"
                                            show-empty
                                            :empty-text="$t('NotFound')"
                                            :fields="employeesFields"
                                            small
                                            responsive
                                            striped
                                            bordered
                                            class="mt-1 text-center"
                                        >
                                            <template #cell(actions)="{ index }">
                                                <div>
                                                    <b-link @click="DeleteEmployee(index)" v-if="canUpdate">
                                                        <b-icon-trash-fill style="width: 20px; height: 20px"></b-icon-trash-fill>
                                                    </b-link>
                                                </div>
                                            </template>
                                        </b-table>
                                    </b-collapse>
                                </b-card>

                                <!-- employees table end --------------------------------------------------- -->

                                <!-- tables table --------------------------------------------------- -->
                                <b-card class="mt-3 form-card">
                                    <b-card-header v-b-toggle.accordion-2 role="tab" class="pa-0 bg-transparent cursor-pointer">
                                        <p class="text-left address-text mb-0">
                                            {{ $t('tablesInfo') }}
                                        </p>
                                    </b-card-header>
                                    <b-collapse visible id="accordion-2">
                                        <validation-observer v-if="canUpdate" ref="ValidationTables" disabled>
                                            <b-row class="mt-2">
                                                <b-col sm="12" md="4">
                                                    <WInput
                                                        v-model="tables.measures"
                                                        :name="$t('measures')"
                                                        rules="required"
                                                        :label="$t('measures')"
                                                        :placeholder="$t('measures')"
                                                    />
                                                </b-col>
                                                <b-col sm="12" md="4">
                                                    <WDatePicker
                                                        v-model="tables.expireOn"
                                                        :name="$t('dateofexpire')"
                                                        rules="required"
                                                        :label="$t('dateofexpire')"
                                                        :placeholder="$t('dateofexpire')"
                                                    />
                                                </b-col>
                                                <b-col sm="12" md="4">
                                                    <WInput
                                                        v-model="tables.responsibleFio"
                                                        :name="$t('responsibleFio')"
                                                        rules="required"
                                                        :label="$t('responsibleFio')"
                                                        :placeholder="$t('responsibleFio')"
                                                    />
                                                </b-col>
                                                <b-col sm="12" md="4">
                                                    <WInput
                                                        v-model="tables.measuresResult"
                                                        :name="$t('measuresResult')"
                                                        rules="required"
                                                        :label="$t('measuresResult')"
                                                        :placeholder="$t('measuresResult')"
                                                    />
                                                </b-col>

                                                <b-col sm="12" md="4" class="align-self-center">
                                                    <b-button @click="AddTable" variant="success">
                                                        <b-icon-plus></b-icon-plus>
                                                        {{ $t('Add') }}
                                                    </b-button>
                                                </b-col>
                                            </b-row>
                                        </validation-observer>

                                        <b-table
                                            :items="Data.tables"
                                            :fields="tablesFields"
                                            show-empty
                                            :empty-text="$t('NotFound')"
                                            small
                                            responsive
                                            striped
                                            bordered
                                            class="mt-1 text-center"
                                        >
                                            <template #cell(actions)="{ index }">
                                                <div>
                                                    <b-link @click="DeleteTable(index)" v-if="canUpdate">
                                                        <b-icon-trash-fill style="width: 20px; height: 20px"></b-icon-trash-fill>
                                                    </b-link>
                                                </div>
                                            </template>
                                        </b-table>
                                    </b-collapse>
                                </b-card>

                                <!-- tables table end --------------------------------------------------- -->

                                <!-- participates table --------------------------------------------------- -->
                                <b-card class="mt-3 form-card">
                                    <b-card-header v-b-toggle.accordion-3 role="tab" class="pa-0 bg-transparent cursor-pointer">
                                        <p class="text-left address-text mb-0">
                                            {{ $t('participates') }}
                                        </p>
                                    </b-card-header>
                                    <b-collapse visible id="accordion-3">
                                        <validation-observer v-if="canUpdate" ref="ValidationParticipates" disabled>
                                            <b-row class="mt-2">
                                                <b-col sm="12" md="4">
                                                    <WInput
                                                        v-model="participates.yearIn"
                                                        :name="$t('yearIn')"
                                                        rules="required"
                                                        mask="####"
                                                        :label="$t('yearIn')"
                                                        :placeholder="$t('yearIn')"
                                                    />
                                                </b-col>
                                                <b-col sm="12" md="4">
                                                    <WInput
                                                        :name="$t('investigationOrganization')"
                                                        v-model="participates.investigationOrganization"
                                                        rules="required"
                                                        :label="$t('investigationOrganization')"
                                                        :placeholder="$t('investigationOrganization')"
                                                    />
                                                </b-col>
                                                <b-col sm="12" md="4">
                                                    <WInput
                                                        :name="$t('basisForInvestigation')"
                                                        v-model="participates.basisForInvestigation"
                                                        rules="required"
                                                        :label="$t('basisForInvestigation')"
                                                        :placeholder="$t('basisForInvestigation')"
                                                    />
                                                </b-col>
                                                <b-col sm="12" md="4">
                                                    <WInput
                                                        :name="$t('investigatedPersonFio')"
                                                        v-model="participates.investigatedPersonFio"
                                                        rules="required"
                                                        :label="$t('investigatedPersonFio')"
                                                        :placeholder="$t('investigatedPersonFio')"
                                                    />
                                                </b-col>
                                                <b-col sm="12" md="4">
                                                    <WInput
                                                        :name="$t('investigatedResult')"
                                                        v-model="participates.investigatedResult"
                                                        rules="required"
                                                        :label="$t('investigatedResult')"
                                                        :placeholder="$t('investigatedResult')"
                                                    />
                                                </b-col>

                                                <b-col sm="12" md="4" class="text-left align-self-center">
                                                    <b-button @click="AddParticipate" variant="success">
                                                        <b-icon-plus></b-icon-plus>
                                                        {{ $t('Add') }}
                                                    </b-button>
                                                </b-col>
                                            </b-row>
                                        </validation-observer>

                                        <b-table
                                            :items="Data.participates"
                                            :fields="participatesFields"
                                            show-empty
                                            :empty-text="$t('NotFound')"
                                            small
                                            responsive
                                            striped
                                            bordered
                                            class="mt-1 text-center"
                                        >
                                            <template #cell(actions)="{ index }">
                                                <div>
                                                    <b-link @click="DeleteParticipate(index)" v-if="canUpdate">
                                                        <b-icon-trash-fill style="width: 20px; height: 20px"></b-icon-trash-fill>
                                                    </b-link>
                                                </div>
                                            </template>
                                        </b-table>
                                    </b-collapse>
                                </b-card>
                                <!-- participates table end --------------------------------------------------- -->
                            </template>
                        </validation-observer>
                    </b-col>

                    <!-- actions -->
                    <b-col sm="12" lg="3" v-if="Data.canEdit || $route.params.id == 0 || Data.canDelete || Data.canSend || Data.canCancel">
                        <b-card class="form-card">
                            <b-card-text class="d-flex flex-column">
                                <b-button @click="SaveData" :disabled="saveLoading" v-if="canUpdate" variant="success">
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
                                <!-- cancel  -->
                                <b-button v-if="Data.canCancel" @click="OpenCancelModal(Data)" variant="warning" block class="mb-1">
                                    <b-icon-x-circle scale="0.6"></b-icon-x-circle>
                                    {{ $t('Cancel') }}
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

            <b-modal :id="'CancelModal' + Data.id" :title="$t('Cancel')" no-close-on-backdrop hide-footer>
                <p>{{ $t('WantCancel') }}</p>
                <b-row>
                    <b-col style="text-align: end" class="text-right">
                        <a @click="$bvModal.hide('CancelModal' + Data.id)" class="btn btn-sm btn-soft-danger mr-2 pr-btn" style="margin-right: 5px">{{ $t('no') }}</a>
                        <a @click="Cancel(Data)" class="btn btn-sm btn-success pr-btn">
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
                        <a @click="Send(Data)" class="btn btn-sm btn-success pr-btn">
                            <b-spinner v-if="CancelLoading" small></b-spinner>
                            {{ $t('yes') }}
                        </a>
                    </b-col>
                </b-row>
            </b-modal>

            <Chat v-if="Data && Data.id" :table-id="Data.tableId || 107" :document-id="Data.id" />
        </div>
    </div>
</template>

<script>
// service
import JoinAntiCorruptionApplicationService from '@/services/joinanticorruptionapplication.service';
import ManualService from '@/services/manual.service';
// components
import WCurrencyInput from '@/components/forms/WCurrencyInput.vue';
import WSelect from '@/components/forms/WSelect.vue';
import WTextarea from '@/components/forms/WTextarea.vue';
import WInput from '@/components/forms/WInput.vue';
import WPhoneInput from '@/components/forms/WPhoneInput.vue';
import WDatePicker from '@/components/forms/WDatePicker.vue';
import axios from 'axios';
import AppListHeaderForName from '@/components/application/AppListHeaderForName.vue';
import AppContractorSettlementAccount from '@/components/application/AppContractorSettlementAccount.vue';
const Chat = () => import('@/components/DocumentChat/Chat.vue');

const employeesDef = {
    id: 0,
    personId: null,
    person: null,
    positionId: null,
    position: null,
    phoneNumber: '',
    email: ''
};

const participatesDef = {
    id: 0,
    orderNumber: 0,
    yearIn: null,
    investigationOrganization: '',
    basisForInvestigation: '',
    investigatedPersonFio: '',
    investigatedResult: ''
};

const tablesDef = {
    id: 0,
    orderNumber: 0,
    measures: '',
    expireOn: '',
    responsibleFio: '',
    measuresResult: ''
};

export default {
    components: {
        WCurrencyInput,
        WSelect,
        WTextarea,
        WInput,
        WPhoneInput,
        WDatePicker,
        AppListHeaderForName,
        AppContractorSettlementAccount,
        Chat
    },
    data() {
        return {
            SendLoading: false,
            CancelLoading: false,
            DeleteLoading: false,
            iframeLoaded: false,
            Data: {
                application: {
                    id: 0,
                    id2: '',
                    statusId: 0,
                    status: null,
                    contractor: '',
                    contractorId: null,
                    contractorDirector: '',
                    contractorInn: '',
                    contractorAddress: '',
                    contractorForm: null,
                    applicationType: null,
                    regionId: null,
                    districtId: null,
                    region: '',
                    district: '',
                    applicationTypeId: 5, // *
                    docOn: '', // *
                    docNumber: null, // *
                    contractorPositionName: '' // *
                },
                tableId: 107,
                contractorActivityTypeId: null, // *
                corruptionReviewTypeId: null, // *
                address: null, // *
                details: null, // *
                contractorUnionActivityTypeId: null, // *
                unionMemberCount: null, // *
                avgEmployeesCount: null, // *
                prevYearlyEarnings: null, // *
                currencyId: 152, // *
                files: [],
                employees: [],
                participates: [],
                tables: []
            },
            CorruptionReviewTypeSelectList: [],
            CurrencySelectList: [],
            ContractorUnionActivityTypeSelectList: [],
            ContractorActivityTypeSelectList: [],
            tablesFields: [
                {
                    key: 'responsibleFio',
                    label: this.$t('responsibleFio'),
                    tdClass: 'text-center',
                    thClass: 'text-center'
                },

                {
                    key: 'expireOn',
                    label: this.$t('dateofexpire'),
                    tdClass: 'text-center',
                    thClass: 'text-center'
                },
                {
                    key: 'measures',
                    label: this.$t('measures'),
                    tdClass: 'text-center',
                    thClass: 'text-center'
                },
                {
                    key: 'measuresResult',
                    label: this.$t('measuresResult'),
                    tdClass: 'text-center',
                    thClass: 'text-center'
                },
                {
                    key: 'actions',
                    label: this.$t('actions'),
                    tdClass: 'text-center',
                    thClass: 'text-center'
                }
            ],
            employeesFields: [
                {
                    key: 'person',
                    label: this.$t('FIO')
                },
                {
                    key: 'position',
                    label: this.$t('position')
                },
                {
                    key: 'phoneNumber',
                    label: this.$t('phonenumber'),
                    tdClass: 'text-center',
                    thClass: 'text-center'
                },
                {
                    key: 'email',
                    label: this.$t('email'),
                    tdClass: 'text-center',
                    thClass: 'text-center'
                },
                {
                    key: 'actions',
                    label: this.$t('actions'),
                    tdClass: 'text-center',
                    thClass: 'text-center'
                }
            ],
            participatesFields: [
                {
                    key: 'yearIn',
                    label: this.$t('yearIn'),
                    tdClass: 'text-center',
                    thClass: 'text-center'
                },
                {
                    key: 'investigationOrganization',
                    label: this.$t('investigationOrganization')
                },
                {
                    key: 'basisForInvestigation',
                    label: this.$t('basisForInvestigation')
                },
                {
                    key: 'investigatedPersonFio',
                    label: this.$t('investigatedPersonFio')
                },
                {
                    key: 'investigatedResult',
                    label: this.$t('investigatedResult')
                },
                {
                    key: 'actions',
                    label: this.$t('actions'),
                    tdClass: 'text-center',
                    thClass: 'text-center'
                }
            ],
            employees: { ...employeesDef },
            participates: { ...participatesDef },
            tables: { ...tablesDef },
            Loading: false,
            saveLoading: false,
            fileLoading: false,
            tab: 0
        };
    },
    created() {
        this.GetData();
        ManualService.CorruptionReviewTypeSelectList().then((res) => {
            if (Array.isArray(res.data)) {
                this.CorruptionReviewTypeSelectList = res.data;
            }
        });
        ManualService.CurrencySelectList().then((res) => {
            if (Array.isArray(res.data)) {
                this.CurrencySelectList = res.data;
            }
        });
        ManualService.ContractorUnionActivityTypeSelectList().then((res) => {
            if (Array.isArray(res.data)) {
                this.ContractorUnionActivityTypeSelectList = res.data;
            }
        });
        ManualService.ContractorActivityTypeSelectList().then((res) => {
            if (Array.isArray(res.data)) {
                this.ContractorActivityTypeSelectList = res.data;
            }
        });
    },
    computed: {
        IframeSrc() {
            return axios.defaults.baseURL + `JoinAntiCorruptionApplication/DownloadPdf?id2=${this.Data.application.id2}&lang=${this.getPdfLang()}`;
        },
        FileSrc() {
            return (id) => axios.defaults.baseURL + `JoinAntiCorruptionApplication/DownloadFile/${id}`;
        },
        canUpdate() {
            return this.Data.canSend || this.Data.id == 0 || this.Data.canEdit;
        }
    },
    methods: {
        OpenCancelModal(item) {
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
            JoinAntiCorruptionApplicationService.Delete(item.id)
                .then(() => {
                    this.DeleteLoading = false;
                    this.makeToast(this.$t('DeleteSuccess'), 'success');
                    this.$bvModal.hide('DeleteModal' + item.id);

                    this.$router.push({ name: 'JoinAntiCorruptionApplication' });
                })
                .catch((error) => {
                    this.showApiError(error);
                    this.DeleteLoading = false;
                });
        },
        Send(item) {
            this.SendLoading = true;
            JoinAntiCorruptionApplicationService.Send(this.filter)
                .then((res) => {
                    this.SendLoading = false;
                    this.$bvModal.hide('SendModal' + item.id);
                    this.$router.push({ name: 'JoinAntiCorruptionApplication' });
                    // this.GetData();
                })
                .catch((error) => {
                    this.showApiError(error);
                    this.SendLoading = false;
                });
        },
        Cancel(item) {
            JoinAntiCorruptionApplicationService.Cancel(this.filter)
                .then(() => {
                    this.CancelLoading = false;
                    this.$bvModal.hide('CancelModal' + item.id);
                    this.GetData();
                })
                .catch((error) => {
                    this.showApiError(error);
                    this.$bvModal.hide('CancelModal' + item.id);
                    this.CancelLoading = false;
                });
        },
        DeleteEmployee(index) {
            this.Data.employees.splice(index, 1);
        },
        AddEmployee() {
            this.$refs.ValidationEmployees.validate().then((success) => {
                if (success) {
                    this.Data.employees.push(this.employees);
                    this.employees = { ...employeesDef };
                    this.$refs.ValidationEmployees.reset();
                }
            });
        },
        DeleteParticipate(index) {
            this.Data.participates.splice(index, 1);
        },
        AddParticipate() {
            this.$refs.ValidationParticipates.validate().then((success) => {
                if (success) {
                    this.participates.orderNumber = this.Data.participates.length + 1;
                    this.Data.participates.push(this.participates);
                    this.participates = { ...participatesDef };
                    this.$refs.ValidationParticipates.reset();
                }
            });
        },
        DeleteTable(index) {
            this.Data.tables.splice(index, 1);
        },
        AddTable() {
            this.$refs.ValidationTables.validate().then((success) => {
                if (success) {
                    this.tables.orderNumber = this.Data.tables.length + 1;
                    this.Data.tables.push(this.tables);
                    this.tables = { ...tablesDef };
                    this.$refs.ValidationTables.reset();
                }
            });
        },
        FileDownload(item) {
            JoinAntiCorruptionApplicationService.DownloadFile(item.id2, this.lang).then((res) => {
                this.forceFileDownload(res, this.$t('JoinAntiCorruptionApplication'));
            });
        },
        UploadFile(event) {
            const formData = new FormData();
            formData.append('files', event.target.files[0]);
            this.fileLoading = true;
            JoinAntiCorruptionApplicationService.UploadFile(formData)
                .then((res) => {
                    this.Data.files.push(...res.data);
                })
                .catch(this.showApiError)
                .finally(() => {
                    this.fileLoading = false;
                });
        },
        DeleteFile(id) {
            JoinAntiCorruptionApplicationService.DeleteFile(id).then(() => {
                this.Data.files = this.Data.files.filter((item) => item.id != id);
            });
        },
        GetData() {
            this.Loading = true;
            JoinAntiCorruptionApplicationService.Get(this.$route.params.id)
                .then((res) => {
                    this.Data = res.data;
                    if (this.$route.params.id == 0) {
                        this.Data.currencyId = 152;
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
                    JoinAntiCorruptionApplicationService.Update(this.Data)
                        .then(() => {
                            this.$router.push({ name: 'JoinAntiCorruptionApplication' });
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
        }
    }
};
</script>
