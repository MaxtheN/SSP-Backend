<template>
    <div>
        <AppListHeaderForName title="MonoApplication" />
        <div class="container">
            <b-overlay :show="Loading">
                <b-row class="justify-content-center">
                    <b-col sm="12" lg="9">
                        <b-card class="form-card">
                            <b-tabs pills card lazy @change="iframeLoaded = false">
                                <b-tab :active="tabActive == 1" :title="$t('AdditionalInfo')">
                                    <validation-observer ref="ValidationDTO" v-if="Data.application">
                                        <b-card-text>
                                            <b-row>
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
                                                    <WInput
                                                        rules="required"
                                                        :label="$t('docDateApplication')"
                                                        name="docDateApplication"
                                                        disabled
                                                        v-model="Data.application.docOn"
                                                    />
                                                </b-col>
                                                <b-col sm="12" md="12" lg="12">
                                                    <WInput rules="required" :label="$t('contractor')" name="contractor" disabled v-model="Data.application.contractor" />
                                                </b-col>
                                                <b-col sm="12" md="6" lg="6">
                                                    <AppContractorSettlementAccount v-model="Data.application.contractorSettlementAccountId" />
                                                </b-col>
                                                <b-col sm="12" md="6" lg="6">
                                                    <WInput rules="required" :label="$t('contractorInn')" name="contractorInn" disabled v-model="Data.application.contractorInn" />
                                                </b-col>
                                                <b-col sm="12" md="6" lg="6">
                                                    <WInput
                                                        rules="required"
                                                        :label="$t('director')"
                                                        name="contractorPositionName"
                                                        v-model="Data.application.contractorPositionName"
                                                    />
                                                </b-col>
                                            </b-row>

                                            <!-- address -->
                                            <hr />
                                            <h5 class="font-weight-bold">
                                                {{ $t('addressT') }}
                                            </h5>

                                            <AddressComponent
                                                v-if="!Loading"
                                                :region-id="Data.application.regionId"
                                                :district-id="Data.application.districtId"
                                                :mfy-id="Data.mfyId"
                                                :address="Data.application.contractorAddress"
                                                @input:regionId="(e) => (Data.application.regionId = e)"
                                                @input:districtId="(e) => (Data.application.districtId = e)"
                                                @input:mfyId="(e) => (Data.mfyId = e)"
                                                @input:address="(e) => (Data.application.contractorAddress = e)"
                                                type="address"
                                            ></AddressComponent>

                                            <!-- monoAddress -->
                                            <hr />
                                            <h5 class="font-weight-bold">
                                                {{ $t('MonoAddress') }}
                                            </h5>

                                            <AddressComponent
                                                v-if="!Loading"
                                                :region-id="Data.monoRegionId"
                                                :district-id="Data.monoDistrictId"
                                                :mfy-id="Data.monoMfyId"
                                                :address="Data.monoAdress"
                                                @input:regionId="(e) => (Data.monoRegionId = e)"
                                                @input:districtId="(e) => (Data.monoDistrictId = e)"
                                                @input:mfyId="(e) => (Data.monoMfyId = e)"
                                                @input:address="(e) => (Data.monoAdress = e)"
                                                type="monoAddress"
                                            ></AddressComponent>

                                            <b-row>
                                                <b-col sm="12" md="6" v-show="0">
                                                    <WCurrencyInput :label="$t('spendForBuild')" name="spendForBuild" v-model="Data.spendForBuild" rules="required" />
                                                </b-col>
                                                <b-col sm="12" md="6">
                                                    <WCurrencyInput :label="$t('buildingCount')" name="buildingCount" v-model="Data.buildingCount" rules="required" />
                                                </b-col>
                                            </b-row>

                                            <!-- maydon -->
                                            <b-row>
                                                <b-col sm="12" md="6">
                                                    <WInput :label="$t('learningArea')" type="number" name="learningArea" v-model="Data.learningArea" rules="required" />
                                                </b-col>
                                                <b-col sm="12" md="6">
                                                    <WInput :label="$t('totalArea')" type="number" name="totalArea" v-model="Data.totalArea" rules="required" />
                                                </b-col>
                                            </b-row>

                                            <!-- studentTables -->
                                            <hr />
                                            <h5 class="font-weight-bold">
                                                {{ $t('studentTables') }}
                                            </h5>
                                            <validation-observer ref="ValidationStudentTables" disabled>
                                                <b-row class="mt-2">
                                                    <b-col sm="12" md="4">
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
                                                    <b-col sm="12" md="4">
                                                        <WInput
                                                            style="flex: 1 1"
                                                            v-model="filterPerson.Number"
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
                                                    <b-col sm="12" md="12">
                                                        <hr />
                                                    </b-col>
                                                    <b-col sm="12" md="12" v-if="studentTabRow.personInfo && studentTabRow.personInfo.pinfl">
                                                        <b-list-group-item>
                                                            <b>{{ $t('familyname') }} </b> :
                                                            {{ studentTabRow.personInfo.surnameLatin }}
                                                        </b-list-group-item>
                                                        <b-list-group-item>
                                                            <b>{{ $t('firstname') }} </b> :
                                                            {{ studentTabRow.personInfo.nameLatin }}
                                                        </b-list-group-item>
                                                        <b-list-group-item>
                                                            <b>{{ $t('lastname') }} </b> :
                                                            {{ studentTabRow.personInfo.patronymLatin }}
                                                        </b-list-group-item>
                                                        <b-list-group-item>
                                                            <b>{{ $t('pinfl') }} </b> :
                                                            {{ studentTabRow.personInfo.pinfl }}
                                                        </b-list-group-item>
                                                        <b-list-group-item>
                                                            <b>{{ $t('livingRegion') }} </b> :
                                                            {{ studentTabRow.personInfo.livingRegion }}
                                                        </b-list-group-item>
                                                        <b-list-group-item>
                                                            <b>{{ $t('livingDistrict') }} </b> :
                                                            {{ studentTabRow.personInfo.livingDistrict }}
                                                        </b-list-group-item>
                                                    </b-col>
                                                    <b-col sm="12" md="4" class="align-self-center my-3">
                                                        <b-button @click="AddStudentTabRow" variant="success" size="sm">
                                                            <b-icon-plus></b-icon-plus>
                                                            {{ $t('Add') }}
                                                        </b-button>
                                                    </b-col>
                                                </b-row>
                                            </validation-observer>

                                            <b-table :items="Data.studentTables" :fields="studentFields" small responsive striped bordered style="font-size: 12px">
                                                <template #cell(pinfl)="{ item }">
                                                    {{ item.personInfo.pinfl }}
                                                </template>
                                                <template #cell(fullName)="{ item }">
                                                    {{ item.personInfo.surnameEng }}
                                                    {{ item.personInfo.nameLatin }}
                                                    {{ item.personInfo.patronymLatin }}
                                                </template>
                                                <template #cell(actions)="{ index }">
                                                    <div>
                                                        <b-link v-if="Data.canSend || Data.id == 0 || Data.canEdit" @click="DeleteStudentTabRow(index)" class="text-danger">
                                                            <b-icon-trash-fill style="width: 20px; height: 20px"></b-icon-trash-fill>
                                                        </b-link>
                                                    </div>
                                                </template>
                                            </b-table>

                                            <!--itemTables  -->
                                            <hr />
                                            <h5 class="font-weight-bold">
                                                {{ $t('itemTables') }}
                                            </h5>
                                            <validation-observer ref="ValidationItemTables" disabled>
                                                <b-row class="mt-2">
                                                    <b-col sm="12" md="4">
                                                        <WSelect
                                                            v-model="itemTabRow.educationItemId"
                                                            :options="EducationItemSelectList"
                                                            rules="required"
                                                            :label="$t('educationItem')"
                                                            name="educationItem"
                                                            :placeholder="$t('educationItem')"
                                                            @change="(e) => (itemTabRow.educationItem = e ? e.text : '')"
                                                        >
                                                        </WSelect>
                                                    </b-col>
                                                    <b-col sm="12" md="4">
                                                        <WSelect
                                                            v-model="itemTabRow.educationItemCurrencyId"
                                                            disabled
                                                            :options="CurrencySelectList"
                                                            rules="required"
                                                            :label="$t('educationItemCurrency')"
                                                            name="educationItemCurrency"
                                                            :placeholder="$t('educationItemCurrency')"
                                                            @change="(e) => (itemTabRow.educationItemCurrency = e ? e.text : '')"
                                                        >
                                                        </WSelect>
                                                    </b-col>
                                                    <b-col sm="12" md="4">
                                                        <WCurrencyInput
                                                            v-model="itemTabRow.educationItemCount"
                                                            rules="required"
                                                            :label="$t('educationItemCount')"
                                                            :name="$t('educationItemCount')"
                                                            :placeholder="$t('educationItemCount')"
                                                            @input="itemTabRow.educationItemAmount = itemTabRow.educationItemPrice * itemTabRow.educationItemCount"
                                                        >
                                                        </WCurrencyInput>
                                                    </b-col>
                                                    <b-col sm="12" md="4">
                                                        <WCurrencyInput
                                                            v-model="itemTabRow.educationItemPrice"
                                                            rules="required"
                                                            :label="$t('educationItemPrice')"
                                                            :name="$t('educationItemPrice')"
                                                            :placeholder="$t('educationItemPrice')"
                                                            @input="itemTabRow.educationItemAmount = itemTabRow.educationItemPrice * itemTabRow.educationItemCount"
                                                        >
                                                        </WCurrencyInput>
                                                    </b-col>
                                                    <b-col sm="12" md="4">
                                                        <WInput
                                                            v-model="itemTabRow.educationItemAmount"
                                                            rules="required"
                                                            :label="$t('educationItemAmount')"
                                                            :name="$t('educationItemAmount')"
                                                            :placeholder="$t('educationItemAmount')"
                                                            type="number"
                                                        >
                                                        </WInput>
                                                    </b-col>
                                                    <b-col sm="12" md="4" class="align-self-center">
                                                        <b-button @click="AddItemTabRow" variant="success" size="sm">
                                                            <b-icon-plus></b-icon-plus>
                                                            {{ $t('Add') }}
                                                        </b-button>
                                                    </b-col>
                                                </b-row>
                                            </validation-observer>

                                            <b-table :items="Data.itemTables" :fields="itemFields" small responsive striped bordered style="font-size: 12px">
                                                <template #cell(actions)="{ index }">
                                                    <div>
                                                        <b-link v-if="Data.canSend || Data.id == 0 || Data.canEdit" @click="DeleteItemTabRow(index)" class="text-danger">
                                                            <b-icon-trash-fill style="width: 20px; height: 20px"></b-icon-trash-fill>
                                                        </b-link>
                                                    </div>
                                                </template>
                                            </b-table>
                                            <!-- summa -->
                                            <b-row>
                                                <b-col sm="12" md="6">
                                                    <WCurrencyInput disabled :label="$t('totalAmount')" name="totalAmount" v-model="Data.totalAmount" rules="required" />
                                                </b-col>
                                                <b-col sm="12" md="6">
                                                    <WSelect v-model="Data.currencyId" :options="CurrencySelectList" rules="required" :label="$t('currency')" name="currency" />
                                                </b-col>
                                            </b-row>

                                            <!-- files -->
                                            <WCurrencyInput :label="$t('totalcost')" disabled name="totalcost" v-model="Data.totalcost" rules="required" />
                                            <FileInput :can-edit="Data.canEdit" column-name="depschema" :files="Data.files" @update:files="(e) => (Data.files = e)" />
                                            <FileInput :can-edit="Data.canEdit" column-name="auditoriesphotos" :files="Data.files" @update:files="(e) => (Data.files = e)" />
                                            <FileInput :can-edit="Data.canEdit" column-name="confdoc" :files="Data.files" @update:files="(e) => (Data.files = e)" />
                                            <FileInput :can-edit="Data.canEdit" column-name="deed" :files="Data.files" @update:files="(e) => (Data.files = e)" />
                                        </b-card-text>
                                    </validation-observer>
                                </b-tab>

                                <b-tab :active="tabActive == 2" v-if="Data.id > 0" no-body :title="$t('Application')">
                                    <b-overlay :show="!iframeLoaded" spinner-variant="primary" spinner-type="grow" rounded="sm">
                                        <iframe
                                            v-if="Data.application && Data.application.id && Data.application.id2"
                                            :src="IframeSrc"
                                            width="100%"
                                            style="height: 100vh"
                                            frameborder="0"
                                            @load="iframeLoaded = true"
                                        ></iframe>
                                    </b-overlay>
                                </b-tab>
                            </b-tabs>
                        </b-card>
                    </b-col>

                    <b-col sm="12" lg="3">
                        <b-card class="form-card">
                            <b-card-text>
                                <b-button @click="SaveData" v-if="Data.canEdit || $route.params.id == 0" variant="success" block class="mb-1">
                                    <b-spinner v-if="saveLoading" small></b-spinner>
                                    <b-icon-check scale="0.8"></b-icon-check>
                                    {{ $t('save') }}
                                </b-button>
                                <!-- delete -->
                                <b-button v-if="Data.canDelete" @click="$bvModal.show('DeleteModal' + Data.id)" variant="danger" block class="mb-1">
                                    <b-icon-trash scale="0.6"></b-icon-trash>
                                    {{ $t('delete') }}
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
                            <b-spinner v-if="deleteLoading" small></b-spinner>
                            {{ $t('yes') }}
                        </a>
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
import WPhoneInput from '@/components/forms/WPhoneInput.vue';
import JustSign from '@/components/justSign.vue';
import MonoApplicationService from '@/services/mono/monoapplication.service';
import AppListHeaderForName from '@/components/application/AppListHeaderForName.vue';
import axios from 'axios';
import ManualService from '@/services/manual.service';
import PersonService from '@/services/person.service';
import WDatePicker from '@/components/forms/WDatePicker.vue';
import AddressComponent from './widgets/Address.vue';
import FileInput from './widgets/FileInput.vue';
import AppContractorSettlementAccount from '@/components/application/AppContractorSettlementAccount.vue';

const studentTabRowDef = {
    id: 0,
    personInfo: {}
};

const itemTabRowDef = {
    id: 0,
    educationItemCurrencyId: 152,
    //   educationItemCurrency: null,
    educationItemId: null,
    educationItem: null,
    educationItemCount: null,
    educationItemPrice: null,
    educationItemAmount: null
};
export default {
    components: {
        WCurrencyInput,
        WSelect,
        WTextarea,
        WInput,
        WPhoneInput,
        JustSign,
        AppListHeaderForName,
        AddressComponent,
        FileInput,
        WDatePicker,
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
            iframeLoaded: false,
            axios,
            Data: {},
            tabActive: 1,
            Loading: false,
            saveLoading: false,
            deleteLoading: false,
            CurrencySelectList: [],
            EducationItemSelectList: [],
            filterPerson: {
                Seria: '',
                Number: '',
                DateOfBirth: ''
            },
            soliqLoading: false,
            studentTabRow: { ...studentTabRowDef },
            studentFields: [
                {
                    key: 'pinfl',
                    label: this.$t('pinfl'),
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
                    key: 'actions',
                    tdClass: 'text-center',
                    thClass: 'text-center',
                    label: this.$t('actions')
                }
            ],
            itemTabRow: { ...itemTabRowDef },
            itemFields: [
                {
                    key: 'educationItem',
                    label: this.$t('educationItem'),
                    tdClass: 'text-center',
                    thClass: 'text-center'
                },
                {
                    key: 'educationItemCount',
                    label: this.$t('educationItemCount'),
                    tdClass: 'text-center',
                    thClass: 'text-center'
                },
                {
                    key: 'educationItemCurrency',
                    label: this.$t('educationItemCurrency'),
                    tdClass: 'text-center',
                    thClass: 'text-center'
                },
                {
                    key: 'educationItemPrice',
                    label: this.$t('educationItemPrice'),
                    tdClass: 'text-center',
                    thClass: 'text-center'
                },
                {
                    key: 'educationItemAmount',
                    label: this.$t('educationItemAmount'),
                    tdClass: 'text-center',
                    thClass: 'text-center'
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
    computed: {
        IframeSrc() {
            return axios.defaults.baseURL + `MonoApplication/DownloadPdf?id2=${this.Data.application.id2}&lang=${this.getPdfLang()}`;
        }
    },
    created() {
        this.Refresh();
        ManualService.CurrencySelectList().then((res) => {
            if (Array.isArray(res.data)) {
                this.CurrencySelectList = res.data;
            }
        });
        ManualService.EducationItemSelectList().then((res) => {
            if (Array.isArray(res.data)) {
                this.EducationItemSelectList = res.data;
            }
        });
    },
    methods: {
        GetPerson() {
            this.studentTabRow.personInfo = {};
            this.soliqLoading = true;
            PersonService.GetByPassportDataFromDigital(this.filterPerson.Seria, this.filterPerson.Number, this.filterPerson.DateOfBirth)
                .then((res) => {
                    this.studentTabRow.personInfo = res.data;
                })
                .catch(this.showApiError)
                .finally(() => {
                    this.soliqLoading = false;
                });
        },
        DeleteStudentTabRow(index) {
            this.Data.studentTables.splice(index, 1);
        },
        AddStudentTabRow() {
            this.$refs.ValidationStudentTables.validate().then((success) => {
                if (success) {
                    this.studentTabRow.orderNumber = this.Data.studentTables.length + 1 + '';
                    this.Data.studentTables.push(this.studentTabRow);
                    this.studentTabRow = { ...studentTabRowDef };
                    console.log(this.studentTabRow, 'test');
                    this.$refs.ValidationStudentTables.reset();
                }
            });
        },
        DeleteItemTabRow(index) {
            this.Data.itemTables.splice(index, 1);
        },
        AddItemTabRow() {
            this.$refs.ValidationItemTables.validate().then((success) => {
                if (success) {
                    this.Data.itemTables.push(this.itemTabRow);
                    this.itemTabRow = { ...itemTabRowDef };
                    this.$refs.ValidationItemTables.reset();

                    this.Data.totalAmount = this.Data.itemTables.reduce((acc, cur) => {
                        return acc + cur.educationItemCount;
                    }, 0);

                    this.Data.totalcost = this.Data.itemTables.reduce((acc, cur) => {
                        return acc + cur.educationItemAmount;
                    }, 0);
                }
            });
        },
        Refresh() {
            this.Loading = true;
            MonoApplicationService.Get(this.$route.params.id)
                .then((res) => {
                    this.Data = res.data;
                })
                .catch(this.showApiError)
                .finally(() => {
                    this.Loading = false;
                });
        },
        Delete(item) {
            this.deleteLoading = true;
            MonoApplicationService.Delete(item.id)
                .then(() => {
                    this.deleteLoading = false;
                    this.makeToast(this.$t('DeleteSuccess'), 'success');
                    this.$bvModal.hide('DeleteModal' + item.id);

                    this.$router.push({ name: 'sspapplication' });
                })
                .catch((error) => {
                    this.showApiError(error);
                    this.deleteLoading = false;
                });
        },
        SaveData() {
            this.$refs.ValidationDTO.validateWithInfo().then(({ isValid, errors }) => {
                if (isValid) {
                    this.saveLoading = true;
                    MonoApplicationService.Update(this.Data)
                        .then((res) => {
                            this.makeToast(this.$t('SaveSuccess'), 'success');
                            this.$router.push({
                                name: 'MonoApplication'
                            });
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

<style lang="scss">
.priceTable {
    thead {
        tr {
            background-color: #f0f0f0;
        }
    }

    tr th,
    tr td {
        padding: 7px;
        border-collapse: collapse;
        border: 1px solid #f5f5f5;
    }

    tr:nth-child(even) {
        background-color: #f7f7f7;
    }

    .w-50px {
        width: 50px;
    }
}
</style>
