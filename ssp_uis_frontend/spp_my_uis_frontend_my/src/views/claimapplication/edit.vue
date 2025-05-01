<template>
    <div >
        <AppListHeaderForName title="ClaimApplication" page-name="ClaimApplication" />
        <div class="container">
            <b-overlay :show="Loading">
                <b-card class="form-card">
                    <validation-observer ref="ValidationDTO">
                        <b-tabs pills card lazy @change="iframeLoaded = false">
                            <!-- form -->
                            <b-tab v-if="Data.statusId != 2 && Data.statusId != 8" active :title="$t('AdditionalInfo')">
                                <b-card-text>
                                    <b-row>
                                        <b-col sm="12" md="6" lg="6">
                                            <WInput rules="required" :label="$t('contractorInn')" name="contractorInn" disabled v-model="Data.application.contractorInn" />
                                        </b-col>
                                        <b-col sm="12" md="6" lg="6">
                                            <WInput rules="required" :label="$t('contractor')" name="contractor" disabled v-model="Data.application.contractor" />
                                        </b-col>
                                        <b-col sm="12" md="6" lg="6">
                                            <AppContractorSettlementAccount v-model="Data.application.contractorSettlementAccountId" />
                                        </b-col>
                                        <b-col sm="12" md="6" lg="6">
                                            <WInput rules="required" :label="$t('docNumber')" name="docNumber" disabled v-model="Data.application.docNumber" />
                                        </b-col>
                                        <b-col sm="12" md="6" lg="6">
                                            <WInput rules="required" :label="$t('docOn')" name="docOn" disabled v-model="Data.application.docOn" />
                                        </b-col>
                                        <b-col sm="12" md="6" lg="6">
                                            <WSelect
                                                rules="required"
                                                v-model="Data.organizationId"
                                                :options="OrganizationList"
                                                :label="$t('filial')"
                                                clearable
                                                :name="$t('filial')"
                                            />
                                        </b-col>
                                    </b-row>

                                    <b-row>
                                        <b-col sm="12" md="6" lg="6">
                                            <WSelect
                                                v-model="Data.claimApplicationTypeId"
                                                :options="ClaimApplicationTypeSelectList"
                                                :label="$t('claimApplicationType')"
                                                name="claimApplicationType"
                                                rules="required"
                                                clearable
                                                @input="ChangeClaimApplicationType"
                                            />
                                        </b-col>
                                        <b-col sm="12" md="6" lg="6" v-if="Data.claimApplicationTypeId == 3 || Data.claimApplicationTypeId == 4">
                                            <WSelect
                                                v-model="Data.prevApplicationId"
                                                valueid="id"
                                                :options="PrevApplicationList"
                                                :label="$t('prevApplication')"
                                                valuename="docNumber"
                                                clearable
                                            />
                                        </b-col>
                                        <b-col sm="12" md="6" lg="6">
                                            <WSelect
                                                v-model="Data.claimThemeId"
                                                :options="ClaimThemeSelectList"
                                                :label="$t('ClaimTheme')"
                                                name="ClaimTheme"
                                                rules="required"
                                                clearable
                                            />
                                        </b-col>
                                        <b-col cols="12" class="mb-1">
                                            <WTextarea v-model="Data.details" :label="$t('comment')" :placeholder="$t('comment')" />
                                        </b-col>
                                        <hr />
                                        <p class="text-left address-text">{{ $t('claimAmount') }}</p>
                                        <!-- davo summasi  -->
                                        <b-col sm="12" md="6">
                                            <WCurrencyInput :label="$t('totalAmount')" disabled v-model="Data.totalAmount" />
                                        </b-col>
                                        <b-col sm="12" md="6">
                                            <WSelect v-model="Data.currencyId" :options="CurrencySelectList" rules="required" :label="$t('currency')" name="currency" />
                                        </b-col>
                                        <b-col sm="12" md="6">
                                            <WCurrencyInput rules="required" @input="calculate" :label="$t('mainDebt')" name="mainDebt" v-model="Data.mainDebt" />
                                        </b-col>
                                        <b-col sm="12" md="6">
                                            <WCurrencyInput
                                                rules="required"
                                                @input="calculate"
                                                :label="$t('calculedPenalty')"
                                                name="calculedPenalty"
                                                v-model="Data.calculedPenalty"
                                            />
                                        </b-col>
                                        <b-col sm="12" md="6">
                                            <WCurrencyInput rules="required" @input="calculate" :label="$t('penalty')" name="penalty" v-model="Data.penalty" />
                                        </b-col>
                                        <b-col sm="12" md="6">
                                            <WCurrencyInput
                                                rules="required"
                                                @input="calculate"
                                                :label="$t('currentInterestRate')"
                                                name="currentInterestRate"
                                                v-model="Data.currentInterestRate"
                                            />
                                        </b-col>
                                        <b-col sm="12" md="6">
                                            <WCurrencyInput
                                                rules="required"
                                                @input="calculate"
                                                :label="$t('currentPrincipalInterest')"
                                                name="currentPrincipalInterest"
                                                v-model="Data.currentPrincipalInterest"
                                            />
                                        </b-col>
                                        <b-col sm="12" md="6">
                                            <WCurrencyInput rules="required" @input="calculate" :label="$t('percentAmount')" :name="$t('percentAmount')" v-model="Data.percent" />
                                        </b-col>
                                        <b-col sm="12" md="6">
                                            <WCurrencyInput
                                                rules="required"
                                                @input="calculate"
                                                :label="$t('otherDebtRepayment')"
                                                :name="$t('otherDebtRepayment')"
                                                v-model="Data.otherDebtRepayment"
                                            />
                                        </b-col>
                                    </b-row>

                                    <validation-observer ref="ValidationTable" disabled>
                                        <b-row class="mt-2">
                                            <hr />
                                            <p class="text-left address-text">
                                                {{ $t('Javobgarlar') }}
                                            </p>
                                            <b-col sm="12" md="4">
                                                <WSelect
                                                    v-model="tabrow.claimResponsibleTypeId"
                                                    :options="ClaimResponsibleTypeSelectList"
                                                    :label="$t('claimResponsibleType')"
                                                    :name="$t('claimResponsibleType')"
                                                    @change="ChangeClaim"
                                                    rules="required"
                                                    clearable
                                                />
                                            </b-col>
                                            <b-col sm="12" md="12" v-if="tabrow.claimResponsibleTypeId">
                                                <hr />
                                            </b-col>
                                            <b-col sm="12" md="4" v-if="tabrow.claimResponsibleTypeId == 3">
                                                <WInput
                                                    style="flex: 1 1"
                                                    v-model="filterPerson.Seria"
                                                    rules="required"
                                                    v-mask="'AA'"
                                                    v-uppercase
                                                    :label="$t('Seria')"
                                                    :name="$t('Seria')"
                                                    :placeholder="$t('AA')"
                                                >
                                                </WInput>
                                            </b-col>
                                            <b-col sm="12" md="4" v-if="tabrow.claimResponsibleTypeId == 3">
                                                <WInput
                                                    style="flex: 1 1"
                                                    v-model="filterPerson.Number"
                                                    rules="required"
                                                    :label="$t('Number')"
                                                    v-mask="'#######'"
                                                    :name="$t('Number')"
                                                    :placeholder="$t('1234567')"
                                                >
                                                </WInput>
                                            </b-col>
                                            <b-col sm="12" md="4" v-if="tabrow.claimResponsibleTypeId == 3">
                                                <b-input-group class="w-input-group">
                                                    <WDatePicker
                                                        style="flex: 1 1"
                                                        v-model="filterPerson.DateOfBirth"
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
                                            <b-col sm="12" md="4" v-if="tabrow.claimResponsibleTypeId == 2">
                                                <b-input-group class="w-input-group">
                                                    <WInput
                                                        style="flex: 1 1"
                                                        v-model="filterSoliq.pinfl"
                                                        rules="required"
                                                        :label="$t('pinfl')"
                                                        :name="$t('pinfl')"
                                                        :placeholder="$t('pinfl')"
                                                    >
                                                    </WInput>
                                                    <b-input-group-append>
                                                        <b-button :disabled="soliqLoading" @click="SoliqIntegration" variant="primary" class="px-3">
                                                            <b-spinner v-if="soliqLoading" small style="margin-right: 8px"></b-spinner>
                                                            <b-icon scale="0.9" icon="search"></b-icon
                                                        ></b-button>
                                                    </b-input-group-append>
                                                </b-input-group>
                                            </b-col>
                                            <b-col sm="12" md="4" v-if="tabrow.claimResponsibleTypeId == 1 || tabrow.claimResponsibleTypeId == 4">
                                                <b-input-group class="w-input-group">
                                                    <WInput
                                                        style="flex: 1 1"
                                                        v-model="filterInn.inn"
                                                        rules="required"
                                                        :label="$t('inn')"
                                                        :name="$t('inn')"
                                                        :placeholder="$t('inn')"
                                                    >
                                                    </WInput>
                                                    <b-input-group-append>
                                                        <b-button :disabled="soliqLoading" @click="GetFromSoliq" variant="primary" class="px-3">
                                                            <b-spinner v-if="soliqLoading" small style="margin-right: 8px"></b-spinner>
                                                            <b-icon scale="0.9" icon="search"></b-icon
                                                        ></b-button>
                                                    </b-input-group-append>
                                                </b-input-group>
                                            </b-col>
                                            <b-col sm="12" md="12">
                                                <hr />
                                            </b-col>
                                            <!-- <b-col sm="12" md="4">
											<WInput v-model="tabrow.orderNumber" rules="required" :label="$t('orderNumber')" :placeholder="$t('orderNumber')" />
										</b-col> -->
                                            <b-col sm="12" md="4">
                                                <WInput
                                                    style="flex: 1 1"
                                                    v-model="tabrow.innOrPinfl"
                                                    rules="required"
                                                    :label="$t('innOrPinfl')"
                                                    :name="$t('innOrPinfl')"
                                                    :placeholder="$t('innOrPinfl')"
                                                >
                                                </WInput>
                                            </b-col>

                                            <b-col sm="12" md="4">
                                                <WInput v-model="tabrow.fullName" rules="required" :label="$t('fullname')" :name="$t('fullname')" :placeholder="$t('fullname')" />
                                            </b-col>
                                            <b-col sm="12" md="4">
                                                <WInput v-model="tabrow.address" rules="required" :label="$t('address')" :name="$t('address')" :placeholder="$t('address')" />
                                            </b-col>
                                            <b-col sm="12" md="4">
                                                <WPhoneInput
                                                    v-model="tabrow.phoneNumber"
                                                    :label="$t('phonenumber')"
                                                    :name="$t('phonenumber')"
                                                    :placeholder="$t('+998')"
                                                    :mask="'+998 (##) ### ## ##'"
                                                    rules="required"
                                                />
                                            </b-col>
                                            <b-col sm="12" md="4" class="align-self-center">
                                                <b-button @click="AddTabrow" variant="success" size="sm">
                                                    <b-icon-plus></b-icon-plus>
                                                    {{ $t('Add') }}
                                                </b-button>
                                            </b-col>
                                        </b-row>
                                    </validation-observer>

                                    <b-row>
                                        <b-col sm="12" lg="12" class="mt-2">
                                            <b-table :items="Data.tables" :fields="fields" small responsive striped bordered style="font-size: 12px">
                                                <template #cell(actions)="{ index }">
                                                    <div>
                                                        <b-link v-if="Data.canSend || Data.id == 0 || Data.canEdit" @click="DeleteItem(index)">
                                                            <b-icon-trash-fill style="width: 20px; height: 20px"></b-icon-trash-fill>
                                                        </b-link>
                                                    </div>
                                                </template>
                                                <template #ceell(isRegistred)="{ item }">
                                                    <div>
                                                        {{ item.isRegistred ? $t('yes') : $t('no') }}
                                                    </div>
                                                </template>
                                            </b-table>
                                        </b-col>
                                        <b-col sm="12" md="12" lg="12">
                                            <h6 class="inputTitle">
                                                <span>*</span>
                                                {{ $t('Taklifga ilovalar (Fayl yuklash)') }} -
                                                {{ $t('10mb') }}
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
                                        <b-col sm="12" class="mt-4" style="text-align: end">
                                            <b-button @click="SaveData" :disabled="saveLoading" v-if="canUpdate" variant="success">
                                                {{ $t('send') }}
                                            </b-button>
                                        </b-col>
                                    </b-row>
                                </b-card-text>
                            </b-tab>
                            <!-- word  -->
                            <b-tab no-body :title="$t('Application')">
                                <ClaimAppWord1 v-if="Data.canEdit" :data="Data" />
                                <b-overlay v-else-if="Data.application.id2" :show="!iframeLoaded" spinner-variant="primary" spinner-type="grow" rounded="sm">
                                    <iframe :src="IframeSrc" width="100%" style="height: 100vh" frameborder="0" @load="iframeLoaded = true"></iframe>
                                </b-overlay>
                            </b-tab>
                            <b-tab no-body :title="$t('message1')" v-if="Data.application.message">
                                <div class="m-2" style="font-size: 18px">
                                    {{ Data.application.message }}
                                </div>
                            </b-tab>
                        </b-tabs>
                    </validation-observer>
                </b-card>
            </b-overlay>
            <b-modal v-model="signModal" size="md" hide-footer hide-header>
                <div style="text-align: right; margin-right: 10px; margin-top: -5px; margin-bottom: 5px; border-bottom: 1px solid lightgray">
                    <span @click="signModal = false" style="cursor: pointer; font-size: 30px"> &times; </span>
                </div>
                <div>
                    <just-sign :data-to-sign="Data" @sign="loginESP($event)"></just-sign>
                </div>
            </b-modal>

            <Chat v-if="Data && Data.id" :table-id="Data.tableId || 101" :document-id="Data.id" />
        </div>
    </div>
</template>

<script>
// service
import ClaimApplicationService from '@/services/claimapplication.servise';
import ManualService from '@/services/manual.service';
// components
import WCurrencyInput from '@/components/forms/WCurrencyInput.vue';
import JustSign from '@/components/justSign.vue';
import WSelect from '@/components/forms/WSelect.vue';
import WTextarea from '@/components/forms/WTextarea.vue';
import WInput from '@/components/forms/WInput.vue';
import WDatePicker from '@/components/forms/WDatePicker.vue';
import WPhoneInput from '@/components/forms/WPhoneInput.vue';
import ClaimAppWord1 from '@/components/word/ClaimAppWord1.vue';
import PersonService from '../../services/person.service';
import SoliqIntegrationService from '../../services/soliqintegration.service';
import AppListHeaderForName from '@/components/application/AppListHeaderForName.vue';
import axios from 'axios';
import OrganizationService from '@/services/organization.service.js';
import AccountService from '../../services/account.service';
import AppContractorSettlementAccount from '@/components/application/AppContractorSettlementAccount.vue';
const Chat = () => import('@/components/DocumentChat/Chat.vue');

const tabrowDef = {
    id: 0,
    orderNumber: '',
    claimResponsibleTypeId: null,
    innOrPinfl: '',
    fullName: '',
    address: '',
    phoneNumber: '',
    isRegistred: false
};

export default {
    components: {
        WCurrencyInput,
        WDatePicker,
        WSelect,
        JustSign,
        WTextarea,
        WInput,
        WPhoneInput,
        AppListHeaderForName,
        ClaimAppWord1,
        AppContractorSettlementAccount,
        Chat
    },
    data() {
        return {
            iframeLoaded: false,
            signModal: false,
            OrganizationList: [],

            Data: {
                application: {
                    id2: ''
                },
                id: 0,
                statusId: 0,
                status: null,
                applicationTypeId: 4,
                applicationType: null,
                contractorId: null,
                contractor: '',
                contractorInn: '',
                regionId: 1,
                region: '',
                districtId: null,
                district: '',
                totalAmount: null, // jami summasi
                mainDebt: null, // asosiy qarzdorlik
                calculedPenalty: null, // hisoblangan penya
                penalty: null, // jarima
                percent: null, // foiz
                currencyId: null,
                currency: null,
                claimApplicationType: null,
                claimTheme: null,
                canEdit: true,
                canSend: false,
                canRevoke: false,
                canReject: false,
                canAccept: false,
                canCancel: false,
                applicationId: null,
                memshipContractId: 0,
                memshipCertificateId: null,
                prevApplicationId: null,
                claimApplicationTypeId: 0,
                claimThemeId: 0,
                details: null,
                tables: [],
                files: [],
                docOn: '',
                docNumber: '',
                contractorPositionName: ''
            },

            tab: 1,
            ClaimApplicationTypeSelectList: [], // turojat turi
            ClaimThemeSelectList: [], //murojat predmeti
            ClaimResponsibleTypeSelectList: [], //javobgarlar
            PrevApplicationList: [], //oldingi application,
            CurrencySelectList: [],
            fields: [
                {
                    key: 'claimResponsibleTypeId',
                    label: this.$t('type'),
                    tdClass: 'text-center',
                    thClass: 'text-center'
                },

                {
                    key: 'innOrPinfl',
                    label: this.$t('inn'),
                    tdClass: 'text-center',
                    thClass: 'text-center'
                },
                {
                    key: 'fullName',
                    label: this.$t('fullname'),
                    tdClass: 'text-center',
                    thClass: 'text-center'
                },
                {
                    key: 'address',
                    label: this.$t('address'),
                    tdClass: 'text-center',
                    thClass: 'text-center'
                },
                {
                    key: 'phoneNumber',
                    label: this.$t('phonenumber'),
                    tdClass: 'text-center',
                    thClass: 'text-center'
                },
                {
                    key: 'isRegistred',
                    label: this.$t('isRegistred'),
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
            tabrow: { ...tabrowDef },
            Loading: false,
            saveLoading: false,
            soliqLoading: false,
            SoliqInfo: {},
            fileLoading: false,
            filterPerson: {
                Seria: '',
                Number: '',
                DateOfBirth: ''
            },
            filterSoliq: {
                pinfl: ''
            },
            filterInn: {
                inn: ''
            }
        };
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
    computed: {
        IframeSrc() {
            return axios.defaults.baseURL + `ClaimApplication/DownloadPdf?id2=${this.Data.application.id2}&lang=${this.getPdfLang()}`;
        },
        FileSrc() {
            return (id) => axios.defaults.baseURL + `ClaimApplication/DownloadFile/${id}`;
        },
        canUpdate() {
            return this.Data.canSend || this.Data.id == 0 || this.Data.canEdit;
        }
    },
    created() {
        this.GetData();
        OrganizationService.OrganizationAsSelectListByGroup().then((res) => {
            this.OrganizationList = res.data;
        });
        ManualService.ClaimApplicationTypeSelectList().then((res) => {
            if (Array.isArray(res.data)) {
                this.ClaimApplicationTypeSelectList = res.data;
            }
        });
        ManualService.ClaimThemeSelectList().then((res) => {
            if (Array.isArray(res.data)) {
                this.ClaimThemeSelectList = res.data;
            }
        });
        ManualService.ClaimResponsibleTypeSelectList().then((res) => {
            if (Array.isArray(res.data)) {
                this.ClaimResponsibleTypeSelectList = res.data;
            }
        });
        ManualService.CurrencySelectList().then((res) => {
            if (Array.isArray(res.data)) {
                this.CurrencySelectList = res.data;
            }
        });
    },
    methods: {
        ChangeClaimApplicationType(item) {
            if (item == 3) {
                ClaimApplicationService.GetListDocNumbersByClaimAppTypeId({
                    contractorId: this.Data.contractorId
                })
                    .then((res) => {
                        this.PrevApplicationList = res.data;
                    })
                    .catch(this.showApiError);
            }
        },
        ChangeClaim(item) {
            this.tabrow = {
                claimResponsibleTypeId: item.value,
                innOrPinfl: '',
                fullName: '',
                address: '',
                phoneNumber: '',
                isRegistred: false
            };
        },
        SoliqIntegration() {
            this.soliqLoading = true;
            SoliqIntegrationService.GetByPinfl(this.filterSoliq.pinfl)
                .then((res) => {
                    this.tabrow.address = res.data.address;
                    this.tabrow.fullName = res.data.name;
                    this.tabrow.phoneNumber = res.data?.contact[0]?.value;
                    this.tabrow.innOrPinfl = res.data.pinfl;
                })
                .catch(this.showApiError)
                .finally(() => {
                    this.soliqLoading = false;
                });
        },
        GetPerson() {
            this.soliqLoading = true;
            PersonService.GetByPassportDataFromDigital(this.filterPerson.Seria, this.filterPerson.Number, this.filterPerson.DateOfBirth)
                .then((res) => {
                    this.tabrow.innOrPinfl = res.data.pinfl;
                    this.tabrow.address = res.data.livingRegion + ' ' + res.data.livingDistrict;
                    this.tabrow.fullName = res.data.surnameLatin + ' ' + res.data.nameLatin + ' ' + res.data.patronymLatin;
                    this.tabrow.phoneNumber = res.data.accountantContact?.phone;
                })
                .catch(this.showApiError)
                .finally(() => {
                    this.soliqLoading = false;
                });
        },

        GetFromSoliq() {
            this.soliqLoading = true;
            AccountService.GetFromSoliq(this.filterInn.inn)
                .then((res) => {
                    this.tabrow.innOrPinfl = this.filterInn.inn;
                    this.tabrow.address =
                        res.data?.companyBillingAddress?.region?.name_uz_latn +
                        ' ' +
                        res.data?.companyBillingAddress?.district?.name_uz_latn +
                        ' ' +
                        res.data?.companyBillingAddress?.streetName;
                    this.tabrow.fullName = res.data.names.statName;
                    this.tabrow.phoneNumber = res.data.accountantContact?.phone;
                })
                .catch(this.showApiError)
                .finally(() => {
                    this.soliqLoading = false;
                });
        },
        calculate() {
            this.Data.totalAmount =
                this.Data.mainDebt +
                this.Data.calculedPenalty +
                this.Data.penalty +
                this.Data.percent +
                this.Data.currentInterestRate +
                this.Data.currentPrincipalInterest +
                this.Data.otherDebtRepayment;
        },
        DeleteItem(index) {
            this.Data.tables.splice(index, 1);
        },
        AddTabrow() {
            this.$refs.ValidationTable.validate().then((success) => {
                if (success) {
                    this.tabrow.orderNumber = this.Data.tables.length + 1 + '';
                    this.Data.tables.push(this.tabrow);
                    this.tabrow = { ...tabrowDef };
                    this.$refs.ValidationTable.reset();
                }
            });
        },
        UploadFile(event) {
            const formData = new FormData();
            formData.append('files', event.target.files[0]);
            this.fileLoading = true;
            ClaimApplicationService.UploadFile(formData)
                .then((res) => {
                    this.Data.files.push(...res.data);
                })
                .catch(this.showApiError)
                .finally(() => {
                    this.fileLoading = false;
                });
        },
        DeleteFile(id) {
            ClaimApplicationService.DeleteFile(id).then(() => {
                this.Data.files = this.Data.files.filter((item) => item.id != id);
            });
        },
        GetData() {
            this.Loading = true;
            ClaimApplicationService.Get(this.$route.params.id)
                .then((res) => {
                    this.Data = res.data;
                })
                .catch(this.showApiError)
                .finally(() => {
                    this.Loading = false;
                });
        },
        loginESP(data) {
            this.$refs.ValidationDTO.validateWithInfo().then(({ isValid, errors }) => {
                if (isValid) {
                    this.saveLoading = true;
                    var info = {};
                    if (!!data.data.alias) {
                        var arr = [];
                        data.data.alias.split(',').forEach(function (item) {
                            arr.push(item.split('='));
                        });
                        const entries = new Map(arr);
                        info = Object.fromEntries(entries);
                    }
                    var obj = {
                        signedData: data.key,
                        state: '',
                        email: null,
                        fullName: data.data.CN,
                        inn: data.data.TIN,
                        pinfl: info['1.2.860.3.16.1.2'],
                        isPinfl: info['1.2.860.3.16.1.1'] ? false : true,
                        eSignCertificateNumber: info.serialnumber
                    };
                    this.signModal = false;

                    this.Data.signedData = obj.signedData;
                    ClaimApplicationService.Update(this.Data)
                        .then(() => {
                            this.$router.push({ name: 'ClaimApplication' });
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
        SaveData() {
            this.$refs.ValidationDTO.validateWithInfo().then(({ isValid, errors }) => {
                if (isValid) {
                    this.signModal = true;
                } else {
                    this.showValidateError(errors);
                }
            });
        }
    }
};
</script>
