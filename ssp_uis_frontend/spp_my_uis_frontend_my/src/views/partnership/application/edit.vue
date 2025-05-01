<template>
    <div>
        <AppListHeaderForName title="PartnershipApplication" page-name="Application" />
        <div class="container">
            <b-overlay :show="Loading" style="min-height: calc(100vh - 460px)">
                <b-card class="form-card">
                    <validation-observer ref="ValidationDTO">
                        <b-tabs pills card lazy>
                            <b-tab active :title="$t('AdditionalInfo')">
                                <b-row>
                                    <b-col sm="12" md="6" lg="6">
                                        <WSelect
                                            v-if="Application.statusId == 30 || Application.canEdit"
                                            :disabled="Application.canSend || !Application.canEdit || Application.statusId == 30"
                                            @input="ChangePRTN"
                                            :label="$t('prtnContractType')"
                                            :name="$t('prtnContractType')"
                                            :clearable="true"
                                            :options="PrtnList"
                                            v-model="Application.prtnContractTypeId"
                                            rules="required"
                                        />
                                    </b-col>
                                    <b-col sm="12" md="6" lg="6">
                                        <WInput
                                            v-if="Application.statusId == 30 || Application.canEdit"
                                            v-mask="'#####################'"
                                            :label="$t('newVacanciesCount')"
                                            placeholder="1234"
                                            disabled
                                            v-model="Application.newVacanciesCount"
                                            rules="required"
                                        />
                                    </b-col>
                                </b-row>
                                <b-row class="mt-2">
                                    <p class="text-left address-text">{{ $t('address') }}</p>
                                    <b-col sm="12" md="4" lg="4">
                                        <WSelect
                                            @input="ChangeRegion"
                                            disabled
                                            :label="$t('region')"
                                            :name="$t('region')"
                                            :options="RegionList"
                                            :clearable="false"
                                            v-model="Application.regionId"
                                            rules="required"
                                        />
                                    </b-col>
                                    <b-col sm="12" md="4" lg="4">
                                        <WSelect
                                            @input="ChangeDistrict"
                                            rules="required"
                                            disabled
                                            :name="$t('district')"
                                            :label="$t('district')"
                                            :options="DistrictList"
                                            :clearable="false"
                                            v-model="Application.districtId"
                                        />
                                    </b-col>
                                    <b-col sm="12" md="4" lg="4" v-if="!Application.chooseLocation">
                                        <WSelect
                                            rules="required"
                                            @input="ChangeMFY1"
                                            :disabled="!Application.canEdit"
                                            :label="$t('mfy')"
                                            :name="$t('mfy')"
                                            :clearable="true"
                                            :options="MfyListLocationFalse"
                                            v-model="Application.mfyId"
                                        />
                                    </b-col>

                                    <b-col sm="12" md="12" lg="12" class="mt-1 mb-2 text-center">
                                        <b-form-checkbox v-model="Application.chooseLocation" :disabled="!Application.canEdit" @input="ChangeCheckBox">
                                            {{ $t('chooseLocation') }}
                                        </b-form-checkbox>
                                    </b-col>
                                    <b-col sm="12" md="4" lg="4" class="mt-1" v-if="Application.chooseLocation">
                                        <WSelect
                                            rules="required"
                                            @input="ChangeChooseRegion"
                                            :disabled="!Application.canEdit"
                                            :label="$t('choosedRegion')"
                                            :name="$t('choosedRegion')"
                                            :clearable="true"
                                            :options="RegionList"
                                            v-model="Application.choosedRegionId"
                                        />
                                    </b-col>
                                    <b-col sm="12" md="4" lg="4" class="mt-1" v-if="Application.chooseLocation">
                                        <WSelect
                                            rules="required"
                                            @input="ChangeChooseDistrict"
                                            :disabled="!Application.canEdit"
                                            :label="$t('choosedDistrict')"
                                            :name="$t('choosedDistrict')"
                                            :clearable="true"
                                            :options="ChooseDistrictList"
                                            v-model="Application.choosedDistrictId"
                                        />
                                    </b-col>
                                    <b-col sm="12" md="4" lg="4" class="mt-1" v-if="Application.chooseLocation">
                                        <WSelect
                                            rules="required"
                                            @input="ChangeMFY"
                                            :disabled="!Application.canEdit"
                                            :label="$t('mfy')"
                                            :name="$t('mfy')"
                                            :clearable="true"
                                            :options="MfyList1"
                                            v-model="Application.mfyId"
                                        />
                                    </b-col>
                                </b-row>

                                <validation-observer ref="ValidationTable" disabled>
                                    <b-row>
                                        <hr />
                                        <p class="text-left address-text">{{ $t('graphs') }}</p>
                                        <b-col sm="12" md="4" lg="4" v-if="Application.canSend || Application.id == 0 || Application.canEdit">
                                            <WSelect
                                                rules="required"
                                                @input="ChangeYear"
                                                :label="$t('yearIn')"
                                                :name="$t('yearIn')"
                                                :clearable="true"
                                                :options="YearList"
                                                v-model="tabrow.yearIn"
                                            />
                                        </b-col>
                                        <b-col sm="12" md="4" lg="4" v-if="Application.id == 0 || Application.canEdit">
                                            <WSelect
                                                rules="required"
                                                @input="ChangeMonth"
                                                :label="$t('monthIn')"
                                                :name="$t('monthIn')"
                                                :disabled="tabrow.yearIn == 0 || tabrow.yearIn == null"
                                                :clearable="true"
                                                :options="MonthInList"
                                                v-model="tabrow.monthIn"
                                            />
                                        </b-col>
                                        <b-col sm="12" md="4" lg="4" v-if="Application.id == 0 || Application.canEdit">
                                            <WInput
                                                rules="required"
                                                v-mask="'#####################'"
                                                :label="$t('newVacanciesCount')"
                                                :name="$t('newVacanciesCount')"
                                                placeholder="1234"
                                                v-model="tabrow.newVacanciesCount"
                                            />
                                        </b-col>
                                        <b-col sm="12" style="text-align: center !important" v-if="Application.prtnContractTypeId">
                                            <b-button v-if="Application.id == 0 || Application.canEdit" variant="success" @click="AddTabrow">
                                                <b-icon-plus />
                                                {{ $t('Add') }}
                                            </b-button>
                                        </b-col>
                                    </b-row>
                                </validation-observer>

                                <b-row>
                                    <b-col sm="12" lg="12" class="mt-2">
                                        <b-table :items="Application.graphs" :fields="fields" responsive striped bordered>
                                            <template #cell(actions)="{ item, index }">
                                                <div>
                                                    <b-link v-if="Application.canSend || Application.id == 0 || Application.canEdit" @click="DeleteItem(item, index)">
                                                        <b-icon-trash-fill style="width: 20px; height: 20px"></b-icon-trash-fill>
                                                    </b-link>
                                                </div>
                                            </template>
                                            <template #cell(monthIn)="{ item }">
                                                <div>
                                                    {{ getMonthName(item.monthIn) }}
                                                </div>
                                            </template>
                                        </b-table>
                                    </b-col>
                                    <b-col sm="12" style="text-align: end !important">
                                        <b-button
                                            @click="SaveData"
                                            :disabled="saveLoading"
                                            v-if="Application.canSend || Application.id == 0 || Application.canEdit"
                                            variant="success"
                                        >
                                            <b-icon-arrow-bar-up scale="0.6"></b-icon-arrow-bar-up>
                                            {{ $t('send') }}
                                        </b-button>
                                    </b-col>
                                </b-row>
                            </b-tab>
                            <b-tab :title="$t('Application')" no-body>
                                <AppWord v-if="Application.canEdit" :data="Application" :org-name="orgName" />
                                <b-row v-if="!Application.canEdit" class="mt-3 mb-3">
                                    <b-col sm="12" md="7" lg="7">
                                        <div v-html="ApplicationHTMLData"></div>
                                    </b-col>
                                </b-row>
                            </b-tab>
                        </b-tabs>
                    </validation-observer>
                </b-card>
            </b-overlay>
        </div>
    </div>
</template>

<script>
import ApplicationService from '@/services/application.service';
import ManualService from '@/services/manual.service';
import AppListHeaderForName from '@/components/application/AppListHeaderForName.vue';
import WInput from '@/components/forms/WInput.vue';
import WSelect from '@/components/forms/WSelect.vue';
import AppWord from '@/components/word/AppWord.vue';

const tabrowDef = {
    id: 0,
    yearIn: 0,
    monthIn: 0,
    newVacanciesCount: 0
};
export default {
    components: {
        AppListHeaderForName,
        WSelect,
        WInput,
        AppWord
    },
    data() {
        return {
            Application: {},
            MonthInList: [],
            MonthInList1: [],
            RegionList: [],
            MfyList1: [],
            YearList: [],
            DistrictList: [],
            ChooseDistrictList: [],
            MfyListLocationFalse: [],
            ApplicationHTMLData: {},
            List: [],
            fields: [
                {
                    key: 'yearIn',
                    label: this.$t('yearIn'),
                    sortable: false,
                    tdClass: 'text-center',
                    thClass: 'text-center'
                },
                {
                    key: 'monthIn',
                    label: this.$t('monthIn'),
                    sortable: false,
                    tdClass: 'text-center',
                    thClass: 'text-center'
                },

                {
                    key: 'newVacanciesCount',
                    label: this.$t('newVacanciesCount'),
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
            PrtnList: [],
            tabrow: { ...tabrowDef },
            Loading: false,
            orgName: '',
            saveLoading: false
        };
    },
    computed: {
        getMonthName() {
            return (month) => this.MonthInList1.filter((el) => el.value == month)[0]?.text;
        }
    },
    created() {
        const today = new Date();
        const yyyy = today.getFullYear();
        this.Refresh();
        ManualService.PrtnContractTypeSelectList().then((res) => {
            this.PrtnList = res.data;
        });
        ManualService.GetMonthSelectList().then((res) => {
            this.MonthInList = res.data;
            this.MonthInList1 = res.data;
        });
        ManualService.RegionSelectList().then((res) => {
            this.RegionList = res.data;
        });
        for (let i = yyyy; i <= yyyy + 2; i++) {
            const s = {
                text: i,
                value: i
            };
            this.YearList.push(s);
        }
    },
    methods: {
        ChangeMFY(item) {
            if (this.Application.choosedDistrictId) {
                this.Application.choosedRegion = this.RegionList.filter((el) => this.Application.choosedRegionId === el.value)[0].text;
                this.Application.choosedDistrict = this.ChooseDistrictList.filter((el) => this.Application.choosedDistrictId === el.value)[0].text;
                this.Application.mfyName = this.MfyList1.filter((el) => this.Application.mfyId === el.value)[0].text;
            }
        },
        ChangeMFY1(item) {
            this.Application.mfyName = this.MfyListLocationFalse.filter((el) => item === el.value)[0]?.text;
        },
        ChangeCheckBox() {
            if (this.Application.canEdit) {
                this.Application.mfyId = null;
                this.Application.choosedDistrictId = null;
                this.Application.choosedRegionId = null;
                this.Application.choosedDistrict = '';
                this.Application.choosedRegion = '';
                this.Application.mfyName = '';
            }
        },
        ChangeRegion(item) {
            ManualService.DistrictSelectList(item).then((res) => {
                this.DistrictList = res.data;
            });
        },
        ChangeDistrict(item) {
            ManualService.MfySelectListForApplication(this.Application.regionId, item).then((res) => {
                this.MfyListLocationFalse = res.data;
            });
        },
        ChangeChooseRegion(item) {
            this.Application.choosedDistrictId = null;
            this.Application.mfyId = null;
            ManualService.DistrictSelectList(item).then((res) => {
                this.ChooseDistrictList = res.data;
            });
        },
        ChangeChooseDistrict(item) {
            this.Application.mfyId = null;

            ManualService.MfySelectListForApplication(this.Application.choosedRegionId, item).then((res) => {
                this.MfyList1 = res.data;
            });
        },
        ChangePRTN(item) {
            console.log('Application.graphs', this.Application.graphs);
            this.Application.prtnContractType = this.PrtnList.filter((el) => item === el.value)[0]?.text;
            ManualService.GetOrganizationNameByLocation(this.Application.regionId, this.Application.districtId, item)
                .then((res) => {
                    this.orgName = res.data;
                })
                .catch((error) => {
                    this.showApiError(error);
                });
            this.Application.graphs = [];
            this.Application.newVacanciesCount = null;
            this.tabrow = { ...tabrowDef };
        },
        ChangeYear() {
            const today = new Date();
            const yyyy = today.getFullYear();
            const mm = today.getMonth() + 1;
            this.tabrow.monthIn = null;
            if (this.tabrow.yearIn == yyyy + 3) {
                this.MonthInList.splice(mm, 12 - mm);
            } else {
                ManualService.GetMonthSelectList().then((res) => {
                    this.MonthInList = res.data;
                });
            }
        },
        ChangeMonth(item) {
            const today = new Date();
            const yyyy = today.getFullYear();
            const mm = today.getMonth();
            if (mm >= item) {
                if (yyyy == this.tabrow.yearIn) {
                    item = null;
                    this.tabrow.monthIn = null;
                    this.makeToast(this.$t('chooseOtherMonth'), 'error');
                }
            }
        },
        DeleteItem(item, index) {
            this.Application.graphs.splice(index, 1);
            this.calculate();
        },
        AddTabrow() {
            this.$refs.ValidationTable.validate().then((success) => {
                if (success) {
                    var self = this;
                    if (self.Application.graphs.length > 0) {
                        const isEqual = self.Application.graphs.find((el) => el.monthIn == self.tabrow.monthIn && el.yearIn == self.tabrow.yearIn);
                        if (isEqual) {
                            this.makeToast(this.$t('chooseOtherMonth'), 'error');
                            return false;
                        } else {
                            if (self.tabrow.monthIn != 0 && self.tabrow.yearIn != 0) {
                                self.Application.graphs.push(self.tabrow);
                            }
                            self.tabrow = { ...tabrowDef };
                        }
                    } else {
                        self.Application.graphs.push(self.tabrow);
                    }
                    self.calculate();
                    self.tabrow = { ...tabrowDef };
                    this.$refs.ValidationTable.reset();
                }
            });
        },
        calculate() {
            var self = this;
            var sum = 0;
            self.Application.graphs.forEach(function (item) {
                sum += item.newVacanciesCount * 1;
            });
            self.Application.newVacanciesCount = sum;
        },
        Refresh() {
            ApplicationService.Get(this.$route.params.id)
                .then((res) => {
                    this.Application = res.data;
                    if (res.data.regionId) {
                        this.ChangeRegion(res.data.regionId);
                    }
                    if (res.data.regionId && res.data.choosedRegionId) {
                        ManualService.DistrictSelectList(res.data.choosedRegionId).then((res) => {
                            this.ChooseDistrictList = res.data;
                        });
                    }
                    if (res.data.regionId && res.data.districtId && res.data.choosedRegionId && res.data.choosedDistrictId) {
                        ManualService.MfySelectListForApplication(res.data.choosedRegionId, res.data.choosedDistrictId).then((res) => {
                            this.MfyList1 = res.data;
                        });
                    } else {
                        ManualService.MfySelectListForApplication(res.data.regionId, res.data.districtId).then((res) => {
                            this.MfyListLocationFalse = res.data;
                        });
                    }

                    if (res.data.graphs?.length > 0) {
                        this.calculate();
                    }

                    if (res.data.id != 0) {
                        ApplicationService.GetApplicationAsHtml(res.data.id)
                            .then((res) => {
                                this.ApplicationHTMLData = res.data;
                            })
                            .catch(this.showApiError);
                    }
                })
                .catch(this.showApiError);
        },
        SaveData() {
            this.$refs.ValidationDTO.validate().then((success) => {
                if (success) {
                    this.saveLoading = true;
                    ApplicationService.Update(this.Application)
                        .then(() => {
                            this.$router.push({ name: 'Application' });
                            this.makeToast(this.$t('SaveSuccess'), 'success');
                        })
                        .catch(this.showApiError)
                        .finally(() => {
                            this.saveLoading = false;
                        });
                }
            });
        }
    }
};
</script>
