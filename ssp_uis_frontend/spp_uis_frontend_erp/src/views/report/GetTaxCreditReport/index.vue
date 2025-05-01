<template>
   <b-card>
      <b-tabs class="nav-tabs nav-justified1 mt-2" v-model="tab2">
         <b-tab :title="$t('variant-1')"> </b-tab>
         <b-tab :title="$t('variant-2')"> </b-tab>
      </b-tabs>
      <b-row>
         <b-col sm="12" :md="tab2 == 1 ? 2 : 3">
            <div>
               <label for>{{ $t('Oblast') }}</label>
               <v-select
                  :options="RegionList"
                  :reduce="(item) => item.value"
                  :placeholder="$t('ChooseBelow')"
                  label="text"
                  v-model="filter.regionId"
                  @input="ChangeRegion"
                  class="w-100"
               ></v-select>
            </div>
         </b-col>
         <b-col sm="12" md="2">
            <div>
               <label for>{{ $t('Region') }}</label>
               <v-select
                  :options="DistrictList"
                  :reduce="(item) => item.value"
                  :placeholder="$t('ChooseBelow')"
                  label="text"
                  v-model="filter.districtId"
                  @input="ChangeDistrict"
                  class="w-100"
               ></v-select>
            </div>
         </b-col>
         <b-col cols="12" md="2">
            <form-picker
               v-model="filter.year"
               type="year"
               @input="Refresh"
               format="YYYY"
               clearable
               :label="$t('docyear')"
            />
         </b-col>
         <b-col v-if="tab2 == 1" cols="12" md="3">
            <label for>{{ $t('inn') }}</label>
            <b-input-group class="text-right">
               <b-form-input v-model="filter.contractorInn" :placeholder="$t('search')" />
               <b-input-group-append>
                  <b-button @click="Refresh" variant="primary">
                     <feather-icon icon="SearchIcon" />
                  </b-button>
               </b-input-group-append>
            </b-input-group>
         </b-col>
         <b-col class="col-auto">
            <b-button @click="Refresh" :disabled="isBusy" variant="primary" class="mt-2">
               <feather-icon icon="SearchIcon" />
               {{ $t('Refresh') }}
            </b-button>
         </b-col>
         <b-col class="col-auto">
            <b-button @click="Print" :disabled="PrintLoading" variant="primary" class="mt-2">
               <feather-icon icon="PrinterIcon"></feather-icon>
               {{ $t('Print') }}
            </b-button>
         </b-col>
      </b-row>
      <b-row align-h="between">
         <b-col sm="12" md="8" class="mt-2">
            <b-button-group @click="Refresh" size="sm" class="mr-2">
               <b-button
                  @click="filter.contarctTypeId = null"
                  :variant="null == filter.contarctTypeId ? 'primary' : 'outline-primary'"
                  >{{ $t('all') }}</b-button
               >
               <b-button
                  v-for="type in PrtnContractTypeList"
                  :key="type.value"
                  @click="filter.contarctTypeId = type.value"
                  :variant="type.value == filter.contarctTypeId ? 'primary' : 'outline-primary'"
                  >{{ type.text }}</b-button
               >
            </b-button-group>
            <b-breadcrumb class="mt-2">
               <b-breadcrumb-item
                  :active="filter.byRegion"
                  @click="
                     () => {
                        filter.byDistrict = false;
                        filter.byRegion = true;
                        filter.byContractor = false;
                        filter.region = '';
                        filter.regionId = null;
                        filter.district = '';
                        filter.districtId = null;
                        Refresh();
                     }
                  "
               >
                  <b>{{ $t('uzb') }}</b>
               </b-breadcrumb-item>
               <b-breadcrumb-item
                  v-show="filter.region"
                  :active="filter.byDistrict"
                  @click="
                     () => {
                        filter.byDistrict = true;
                        filter.byRegion = false;
                        filter.byContractor = false;
                        filter.district = '';
                        filter.districtId = null;
                        Refresh();
                     }
                  "
               >
                  <b>{{ filter.region }}</b>
               </b-breadcrumb-item>
               <b-breadcrumb-item v-show="filter.district" :active="filter.byContractor">
                  <b>{{ filter.district }}</b>
               </b-breadcrumb-item>
            </b-breadcrumb>
         </b-col>
      </b-row>

      <b-tabs v-if="tab2 == 0" class="nav-tabs nav-justified1 mt-2" v-model="tab">
         <b-tab title="Жамланма"> </b-tab>
         <b-tab title="мол-мулк солиғи бўйича"> </b-tab>
         <b-tab title="ер солиғи бўйича"> </b-tab>
         <b-tab title="даромад солиғи бўйича"> </b-tab>
         <b-tab title="ижтимоий солиқ бўйича 50 фоизлик ставдаги"> </b-tab>
      </b-tabs>

      <Tables
         :items="items"
         :tab2="tab2"
         :filter="filter"
         :is-busy="isBusy"
         :totals="totals"
         :tab="tab"
         :SortDistrict="SortDistrict"
         :SortRegion="SortRegion"
      />

      <ReportPagination v-if="tab2 == 1" @request="Refresh" :filter="filterPaged" />
   </b-card>
</template>

<script>
import {
   BButton,
   BPagination,
   BTable,
   BCol,
   VBTooltip,
   VBModal,
   BRow,
   BSpinner,
   BCard,
   BTooltip,
   BBadge,
   BInputGroup,
   BFormInput,
   BInputGroupAppend,
   BLink,
   BModal,
   BCardText,
   BTableSimple,
   BThead,
   BTbody,
   BTr,
   BTd,
   BTh,
   BButtonGroup,
   BFormCheckbox,
   BBreadcrumb,
   BBreadcrumbItem,
   BIconPrinter,
   BOverlay,
   BTfoot,
   BTabs,
   BTab
} from 'bootstrap-vue';
import ReportService from '@/services/report/report.service';
import PrtnContractTypeService from '@/services/info/prtncontracttype.service';

import RegionService from '@/services/info/region.service';
import DistrictService from '@/services/info/district.service';

const Tables = () => import('./components/Tables.vue');
const ReportPagination = () => import('@/views/report/components/ReportPagination.vue');

const TOTALSDEF = {
   totalcertificateCount: 0,
   totalnewVacanciesCount: 0,
   incomeTaxCount: 0,
   contractorIncomeTaxCount: 0,
   contractorLandTaxCount: 0,
   landTaxSum: 0,
   contractorPropertyTaxCount: 0,
   propertyTaxSum: 0,
   contractorSocialTaxCount: 0,
   socialTaxSum: 0,
   totalContractApplicationCount: 0,
   propertyTaxCount: 0,
   propertyNewVacanciesCount: 0,
   landTaxCount: 0,
   landNewVacanciesCount: 0,
   incomeNewVacanciesCount: 0,
   incomeTaxSum: 0,
   socialTaxCount: 0,
   socialNewVacanciesCount: 0,
   privilegeTaxCount: 0,
   privilegeTaxSum: 0
};

export default {
   components: {
      BButton,
      BPagination,
      BTable,
      BCol,
      BRow,
      BSpinner,
      BCard,
      BTooltip,
      BBadge,
      BInputGroup,
      BFormInput,
      BInputGroupAppend,
      BLink,
      BModal,
      BCardText,
      BTableSimple,
      BThead,
      BTbody,
      BTr,
      BTd,
      BTh,
      BButtonGroup,
      BFormCheckbox,
      BBreadcrumb,
      BBreadcrumbItem,
      BIconPrinter,
      BOverlay,
      BTfoot,
      Tables,
      BTabs,
      BTab,
      ReportPagination
   },
   directives: {
      'b-tooltip': VBTooltip,
      'b-modal': VBModal
   },
   data() {
      return {
         tab: 0,
         tab2: 0,
         PrintLoading: false,
         items: [],
         RegionList: [],
         DistrictList: [],
         PrtnContractTypeList: [],
         filter: {
            year: null,
            contarctTypeId: null,
            regionId: null,
            region: '',
            byRegion: true,
            districtId: null,
            district: '',
            byDistrict: false,
            contractorId: null,
            byContractor: false,
            contractorInn: null
         },
         filterPaged: {
            sortBy: '',
            orderType: 'asc',
            page: 1,
            pageSize: 20,
            perPageOptions: [10, 20, 50, 100],
            total: 0
         },
         isBusy: false,
         totals: {
            ...TOTALSDEF
         }
      };
   },
   created() {
      RegionService.GetAsSelectList(211)
         .then((res) => {
            this.RegionList = res.data;
         })
         .catch((error) => {
            this.showApiError(error);
         });

      PrtnContractTypeService.GetAsSelectList()
         .then((res) => {
            this.PrtnContractTypeList = res.data;
            this.Refresh();
         })
         .catch((error) => {
            this.showApiError(error);
         });
   },
   watch: {
      tab2: {
         handler(newV, oldV) {
            if (oldV == 1 || newV == 1) {
               if (newV == 1) {
                  this.filter.byContractor = true;
                  this.filter.byDistrict = true;
                  this.filter.byRegion = true;
               } else {
                  this.filter.byContractor = false;
                  this.filter.byDistrict = false;
                  this.filter.byRegion = true;
               }
               this.Refresh();
            }
         }
      }
   },
   methods: {
      Print() {
         this.PrintLoading = true;
         ReportService.SaveAsExcelTaxCreditReport({ ...this.filter, tab: this.tab })
            .then((res) => {
               this.forceFileDownload(res, this.$t('GetTaxCreditReportShort'));
            })
            .catch((err) => {
               this.showApiError(err);
            })
            .finally(() => {
               this.PrintLoading = false;
            });
      },
      SortRegion(item) {
         this.filter.byDistrict = true;
         this.filter.byRegion = false;
         this.filter.byContractor = false;
         this.filter.regionId = item.regionId;
         this.filter.region = item.region;
         this.GetDistrict(item.regionId);
         this.Refresh();
      },
      SortDistrict(item) {
         this.filter.byDistrict = false;
         this.filter.byRegion = false;
         this.filter.byContractor = true;
         this.filter.districtId = item.districtId;
         this.filter.district = item.district;
         this.Refresh();
      },
      SortChange(data) {
         this.filter.Sort = data.sortBy;
         this.filter.Order = data.sortDesc ? 'desc' : 'asc';
         this.Refresh();
      },
      ChangeRegion(id) {
         if (id) {
            this.filter.districtId = null;

            this.filter.byDistrict = true;
            this.filter.byRegion = false;
            this.filter.byContractor = false;

            this.filter.region = this.filter.regionId
               ? this.RegionList.filter((item) => item.value === this.filter.regionId)[0].text
               : '';

            this.Refresh();
            this.GetDistrict(id);
         } else {
            this.filter.districtId = null;
            this.filter.byDistrict = false;
            this.filter.byRegion = true;
            this.filter.region = '';
            this.filter.byContractor = false;
            this.Refresh();
         }
      },
      GetDistrict(id) {
         DistrictService.GetAsSelectList(id)
            .then((res) => {
               this.DistrictList = res.data;
            })
            .catch((error) => {
               this.showApiError(error);
            });
      },
      ChangeDistrict(id) {
         if (id) {
            this.filter.byDistrict = false;
            this.filter.byRegion = true;
            this.filter.byContractor = true;
            this.filter.district = this.filter.districtId
               ? this.DistrictList.filter((item) => item.value === this.filter.districtId)[0].text
               : '';
            this.Refresh();
         } else {
            this.filter.byDistrict = true;
            this.filter.byRegion = true;
            this.filter.byContractor = true;
            this.filter.district = '';
            this.Refresh();
         }
      },
      Refresh() {
         this.isBusy = true;
         console.log(this.filter.year === null);
         if (this.tab2 == 1) {
            (this.filter.byContractor = true),
               ReportService.GetPagedTaxCreditReport({
                  ...this.filter,
                  ...this.filterPaged,
                  year: this.filter.year === null ? null : +this.filter.year
               })
                  .then((res) => {
                     this.items = res.data.rows;
                     this.filterPaged.total = res.data.total;
                  })
                  .finally(() => {
                     this.isBusy = false;
                  });
         } else {
            ReportService.GetTaxCreditReport({
               ...this.filter,
               year: +this.filter.year,
               year: this.filter.year === null ? null : +this.filter.year
            })
               .then((res) => {
                  this.items = res.data;

                  this.totals = JSON.parse(JSON.stringify(TOTALSDEF));

                  res.data.forEach((item) => {
                     this.totals.totalcertificateCount += item.certificateCount;
                     this.totals.totalnewVacanciesCount += item.newVacanciesCount;
                     this.totals.incomeTaxCount += item.incomeTaxCount;
                     this.totals.contractorIncomeTaxCount += item.contractorIncomeTaxCount;
                     this.totals.contractorLandTaxCount += item.contractorLandTaxCount;
                     this.totals.landTaxSum += item.landTaxSum;
                     this.totals.contractorPropertyTaxCount += item.contractorPropertyTaxCount;
                     this.totals.propertyTaxSum += item.propertyTaxSum;
                     this.totals.contractorSocialTaxCount += item.contractorSocialTaxCount;
                     this.totals.socialTaxSum += item.socialTaxSum;
                     this.totals.totalContractApplicationCount += item.totalContractApplicationCount;
                     this.totals.count += item.count;
                     this.totals.summa += item.summa;
                     this.totals.propertyTaxCount += item.propertyTaxCount;
                     this.totals.propertyNewVacanciesCount += item.propertyNewVacanciesCount;
                     this.totals.landTaxCount += item.landTaxCount;
                     this.totals.landNewVacanciesCount += item.landNewVacanciesCount;
                     this.totals.incomeNewVacanciesCount += item.incomeNewVacanciesCount;
                     this.totals.incomeTaxSum += item.incomeTaxSum;
                     this.totals.socialTaxCount += item.socialTaxCount;
                     this.totals.socialNewVacanciesCount += item.socialNewVacanciesCount;
                     this.totals.privilegeTaxSum += item.privilegeTaxSum;
                     this.totals.privilegeTaxCount += item.privilegeTaxCount;
                  });
               })
               .catch((error) => {
                  this.showApiError(error);
               })
               .finally(() => {
                  this.isBusy = false;
               });
         }
      }
   }
};
</script>
