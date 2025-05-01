<template>
   <b-card nobody>
      <b-tabs pills>
         <b-tab @click="(tabData = 1), Refresh()" :title="$t('variant-1')"> </b-tab>
         <b-tab @click="(tabData = 2), Refresh()" :title="$t('variant-2')"> </b-tab>
      </b-tabs>
      <b-row>
         <b-col sm="12" md="2">
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
         <b-col sm="12" md="2">
            <form-picker
               :label="$t('startDate')"
               v-model="filter.startDate"
               :placeholder="$t('startDate')"
               @change="Refresh"
            ></form-picker>
         </b-col>
         <b-col sm="12" md="2">
            <form-picker
               :label="$t('endDate')"
               v-model="filter.endDate"
               :placeholder="$t('endDate')"
               @change="Refresh"
            ></form-picker>
         </b-col>
         <b-col sm="12" md="2">
            <label for>{{ $t('Muddati kechikanlar') }}</label>

            <v-select
               :options="expires"
               :reduce="(item) => item.value"
               :placeholder="$t('Muddati kechikanlar')"
               label="text"
               v-model="filter.expireDay"
               @input="Refresh"
               class="w-100"
            ></v-select>
         </b-col>

         <b-col cols="12" md="2">
            <label for>{{ $t('inn') }}</label>
            <b-input-group class="text-right mb-1">
               <b-form-input v-model="filter.contractorInn" :placeholder="$t('search')" />
               <b-input-group-append>
                  <b-button @click="Refresh" variant="primary">
                     <feather-icon icon="SearchIcon" />
                  </b-button>
               </b-input-group-append>
            </b-input-group>
         </b-col>

         <b-col sm="12" md="8" class="mb-2">
            <b-button-group @click="Refresh" size="sm" class="mr-2">
               <b-button
                  @click="filter.prtnContractTypeId = null"
                  :variant="filter.prtnContractTypeId == null ? 'primary' : 'outline-primary'"
                  >{{ $t('all') }}
               </b-button>

               <b-button
                  v-for="type in PrtnContractTypeList"
                  :key="type.value"
                  @click="filter.prtnContractTypeId = type.value"
                  :variant="type.value == filter.prtnContractTypeId ? 'primary' : 'outline-primary'"
               >
                  {{ type.text }}
               </b-button>
            </b-button-group>
         </b-col>
         <b-col v-if="tabData == 1" cols="12" md="4" class="d-flex mb-2 justify-content-end align-items-center">
            <b-button @click="Print" :disabled="PrintLoading" variant="primary" class="ml-1">
               <feather-icon icon="PrinterIcon"></feather-icon>
               {{ $t('Print') }}
            </b-button>
         </b-col>
         <b-col v-else cols="12" md="4" class="d-flex mb-2 justify-content-end align-items-center">
            <b-button @click="Print" :disabled="PrintLoading" variant="primary" class="ml-1">
               <feather-icon icon="PrinterIcon"></feather-icon>
               {{ $t('Print') }}
            </b-button>
         </b-col>
      </b-row>

      <div class="report-table">
         <b-overlay :show="isBusy">
            <b-table-simple
               class="table-scroll position-relative"
               hover
               small
               caption-top
               responsive
               border
               style="max-height: 570px; overflow-y: auto"
            >
               <b-thead class="position-sticky" style="top: 0">
                  <b-tr>
                     <b-th rowspan="3">№</b-th>
                     <b-th v-if="tabData == 1" rowspan="3">{{ $t('region') }}</b-th>
                     <b-th v-else rowspan="3">{{ $t('contractor') }}</b-th>
                     <b-th rowspan="3" style="min-width: 150px" v-if="false">{{
                        $t('Ijroda turgan jami arizalar')
                     }}</b-th>
                     <b-th colspan="14">{{ $t('ofThem') }}</b-th>
                  </b-tr>
                  <b-tr>
                     <b-th colspan="2"> {{ $t('Mahallabayni rivojlantirish agentligi') }} </b-th>
                     <b-th colspan="2">{{ $t('Adliya') }}</b-th>
                     <b-th colspan="2">{{ $t('Ekspertizaga qayta yuborish') }}</b-th>
                     <b-th colspan="2">{{ $t('Hokimiyatlar') }}</b-th>
                     <b-th colspan="2">{{ $t("Kambag'allikni qisqartirish va bandlik vazirligi") }}</b-th>
                     <b-th colspan="2">{{ $t('Moliya vazirligi') }}</b-th>
                     <b-th colspan="2">{{ $t('Savdo sanoat palatasi') }}</b-th>
                  </b-tr>
                  <b-tr>
                     <template v-for="i in 7">
                        <b-th :key="i + 'd'">{{ $t('totalAplication') }}</b-th>
                        <b-th :key="i + 'dd'" style="min-width: 150px">
                           {{ expireDayTitle }}
                        </b-th>
                     </template>
                  </b-tr>
               </b-thead>

               <tbody>
                  <b-tr
                     v-if="tabData == 1"
                     class="bg-light font-weight-bold sticky"
                     style="position: sticky; z-index: 0; top: 115px"
                  >
                     <b-td colspan="2" class="text-right font-weight-bold">{{ $t('ofreport') }}:</b-td>
                     <b-td v-if="false">{{ totals.allApplicationsCount }}</b-td>
                     <b-td class="text-right">{{ totals.isBeingCosideredApplicationCount }}</b-td>
                     <b-td class="text-right">{{ totals.expiredIsBeingCosideredApplicationCount }}</b-td>
                     <b-td class="text-right">{{ totals.sendToExpertiseCount }}</b-td>
                     <b-td class="text-right">{{ totals.expiredSendToExpertiseCount }}</b-td>
                     <b-td class="text-right">{{ totals.notPassCount }}</b-td>
                     <b-td class="text-right">{{ totals.expiredResentToExpiredCount }}</b-td>
                     <b-td class="text-right">{{ totals.passCount1 }}</b-td>
                     <b-td class="text-right">{{ totals.signExpireOnCount1 }}</b-td>
                     <b-td class="text-right">{{ totals.passCount2 }}</b-td>
                     <b-td class="text-right">{{ totals.signingExpireOnCount }}</b-td>
                     <b-td class="text-right">{{ totals.signingCount }}</b-td>
                     <b-td class="text-right">{{ totals.signExpireOnCount2 }}</b-td>
                     <b-td class="text-right">{{ totals.notGeneratedCertificatesCount }}</b-td>
                     <b-td class="text-right">{{ totals.generatedCertificatesCount }}</b-td>
                  </b-tr>
                  <b-tr v-for="(item, index) in items" :key="item.id">
                     <b-td>{{ index + 1 }}</b-td>
                     <template v-if="tabData == 1">
                        <b-td v-if="filter.districtId"> {{ item.contractorInn }} - {{ item.contractor }}</b-td>
                        <b-td
                           v-else-if="filter.regionId"
                           class="text-primary font-weight-bold"
                           @click="
                              () => {
                                 filter.districtId = item.districtId;
                                 Refresh();
                              }
                           "
                           >{{ item.district }}</b-td
                        >
                        <b-td v-else class="text-primary font-weight-bold" @click="ChangeRegion(item.regionId)">{{
                           item.region
                        }}</b-td>
                     </template>
                     <b-td v-else>{{ item.contractorInn }} - {{ item.contractor }}</b-td>
                     <b-td class="text-right" v-if="false">{{ item.allApplicationsCount }}</b-td>
                     <b-td class="text-right">{{ item.isBeingCosideredApplicationCount }}</b-td>
                     <b-td class="text-right">{{ item.expiredIsBeingCosideredApplicationCount }}</b-td>
                     <b-td class="text-right">{{ item.sendToExpertiseCount }}</b-td>
                     <b-td class="text-right">{{ item.expiredSendToExpertiseCount }}</b-td>
                     <b-td class="text-right">{{ item.notPassCount }}</b-td>
                     <b-td class="text-right">{{ item.expiredResentToExpiredCount }}</b-td>
                     <b-td class="text-right">{{ item.passCount1 }}</b-td>
                     <b-td class="text-right">{{ item.signExpireOnCount1 }}</b-td>
                     <b-td class="text-right">{{ item.passCount2 }}</b-td>
                     <b-td class="text-right">{{ item.signingExpireOnCount }}</b-td>
                     <b-td class="text-right">{{ item.signingCount }}</b-td>
                     <b-td class="text-right">{{ item.signExpireOnCount2 }}</b-td>
                     <b-td class="text-right">{{ item.notGeneratedCertificatesCount }}</b-td>
                     <b-td class="text-right">{{ item.generatedCertificatesCount }}</b-td>
                  </b-tr>
               </tbody>
            </b-table-simple>

            <div class="mx-2 mb-2" v-if="tabData == 2">
               <b-row>
                  <b-col
                     cols="12"
                     sm="6"
                     class="d-flex align-items-center justify-content-center justify-content-sm-start"
                  >
                     <span class="text-muted">
                        {{ $t('Showing') }} {{ firstNumber }} {{ $t('to') }} {{ lastNumber }} {{ $t('of') }}
                        {{ filter2.total }}
                        {{ $t('entries') }}
                     </span>
                     <v-select
                        v-model="filter2.pageSize"
                        :dir="$store.state.appConfig.isRTL ? 'rtl' : 'ltr'"
                        :options="filter2.pageOptions"
                        :clearable="false"
                        @input="Refresh"
                        class="per-page-selector d-inline-block ml-50 mr-1"
                     />
                  </b-col>
                  <!-- Pagination -->
                  <b-col
                     cols="12"
                     sm="6"
                     class="d-flex align-items-center justify-content-center justify-content-sm-end"
                  >
                     <b-pagination
                        v-model="filter2.page"
                        :total-rows="filter2.total"
                        :per-page="filter2.pageSize"
                        first-number
                        last-number
                        @input="Refresh"
                        class="mb-0 mt-1 mt-sm-0"
                        prev-class="prev-item"
                        next-class="next-item"
                     >
                        <template #prev-text>
                           <feather-icon icon="ChevronLeftIcon" size="18" />
                        </template>
                        <template #next-text>
                           <feather-icon icon="ChevronRightIcon" size="18" />
                        </template>
                     </b-pagination>
                  </b-col>
               </b-row>
            </div>
         </b-overlay>
      </div>
   </b-card>
</template>

<script>
import axios from 'axios';
import RegionService from '@/services/info/region.service';
import ReportService from '@/services/report/report.service';
import PrtnContractTypeService from '@/services/info/prtncontracttype.service';
import DistrictService from '@/services/info/district.service';

import {
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
   BTfoot,
   BOverlay,
   BTabs,
   BTab
} from 'bootstrap-vue';

let controller;
let controller2;

const totalsDef = {
   allApplicationsCount: 0,
   isBeingCosideredApplicationCount: 0,
   expiredIsBeingCosideredApplicationCount: 0,
   sendToExpertiseCount: 0,
   expiredSendToExpertiseCount: 0,
   notPassCount: 0,
   expiredResentToExpiredCount: 0,
   passCount1: 0,
   signExpireOnCount1: 0,
   passCount2: 0,
   signingExpireOnCount: 0,
   signingCount: 0,
   signExpireOnCount2: 0,
   notGeneratedCertificatesCount: 0,
   generatedCertificatesCount: 0
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
      BTfoot,
      BOverlay,
      BTabs,
      BTab
   },

   data() {
      return {
         PrintLoading: false,
         RegionList: [],
         DistrictList: [],
         PrtnContractTypeList: [],
         isBusy: false,
         tabData: 1,
         items: [],
         filter: {
            prtnContractTypeId: null,
            regionId: null,
            districtId: null,
            contractorId: null,
            startDate: '',
            endDate: '',
            mfyId: null,
            byMfy: false,
            contractorInn: null,
            expireDay: null
         },
         filter2: {
            page: 1,
            pageSize: 20,
            pageOptions: [10, 20, 50, 100],
            total: 0
         },
         totals: { ...totalsDef },
         expires: [
            { value: 1, text: 1 },
            { value: 2, text: 2 },
            { value: 3, text: 3 },
            { value: 4, text: 4 },
            { value: 5, text: 5 },
            { value: 6, text: 6 },
            { value: 7, text: 7 },
            { value: 8, text: "1 haftadan ko'p" },
            { value: 9, text: "1 oydan ko'p" }
         ]
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
         })
         .catch((error) => {
            this.showApiError(error);
         });
      this.Refresh();
   },
   computed: {
      expireDayTitle() {
         return this.expires.find((e) => e.value == this.filter.expireDay)?.text || this.$t('Muddati kechikanlar');
      },
      firstNumber() {
         return (this.filter2.page - 1) * this.filter2.pageSize + 1;
      },
      lastNumber() {
         if (this.filter2.total < this.filter2.pageSize) {
            return this.filter2.total;
         } else {
            if (this.filter2.page * this.filter2.pageSize > this.filter2.total) {
               return this.filter2.total;
            } else {
               return this.filter2.page * this.filter2.pageSize;
            }
         }
      }
   },
   methods: {
      ChangeRegion(id) {
         if (id) {
            this.filter.regionId = id;
            this.Refresh();
            this.GetDistrict(id);
         } else {
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
         this.Refresh();
      },
      Refresh() {
         this.items = [];
         if (controller) {
            controller.abort();
         }
         if (controller2) {
            controller2.abort();
         }

         if (this.tabData == 1) {
            controller = new AbortController();
            this.isBusy = true;
            this.totals = { ...totalsDef };

            ReportService.GetExpiredContractorsReport(this.filter, {
               signal: controller.signal
            })
               .then((res) => {
                  this.items = res.data;

                  this.items.forEach((item) => {
                     this.totals.allApplicationsCount += item.allApplicationsCount;
                     this.totals.isBeingCosideredApplicationCount += item.isBeingCosideredApplicationCount;
                     this.totals.expiredIsBeingCosideredApplicationCount +=
                        item.expiredIsBeingCosideredApplicationCount;
                     this.totals.sendToExpertiseCount += item.sendToExpertiseCount;
                     this.totals.expiredSendToExpertiseCount += item.expiredSendToExpertiseCount;
                     this.totals.notPassCount += item.notPassCount;
                     this.totals.expiredResentToExpiredCount += item.expiredResentToExpiredCount;
                     this.totals.passCount1 += item.passCount1;
                     this.totals.signExpireOnCount1 += item.signExpireOnCount1;
                     this.totals.passCount2 += item.passCount2;
                     this.totals.signingExpireOnCount += item.signingExpireOnCount;
                     this.totals.signingCount += item.signingCount;
                     this.totals.signExpireOnCount2 += item.signExpireOnCount2;
                     this.totals.notGeneratedCertificatesCount += item.notGeneratedCertificatesCount;
                     this.totals.generatedCertificatesCount += item.generatedCertificatesCount;
                  });
                  this.isBusy = false;
               })
               .catch((error) => {
                  if (axios.isCancel(error)) {
                     console.log('Request canceled', error.message);
                  } else {
                     this.showApiError(error);
                  }
               });
         }
         if (this.tabData == 2) {
            controller2 = new AbortController();
            this.isBusy = true;

            ReportService.GetExpiredReportByContractors(
               { ...this.filter, ...this.filter2 },
               {
                  signal: controller2.signal
               }
            )
               .then((res) => {
                  this.items = res.data.rows;
                  this.filter2.total = res.data.total;
               })
               .catch((error) => {
                  if (axios.isCancel(error)) {
                     console.log('Request canceled', error.message);
                  } else {
                     this.showApiError(error);
                  }
               })
               .finally(() => {
                  this.isBusy = false;
               });
         }
      },
      Print() {
         if (this.tabData == 1) {
            this.PrintLoading = true;
            ReportService.SaveAsExcelExpiredContractorsReport(this.filter).then((res) => {
               this.forceFileDownload(res, this.$t('GetExpiredContractorsReport'));
               this.PrintLoading = false;
            });
         }
         if (this.tabData == 2) {
            this.PrintLoading = true;
            ReportService.SaveAsExcelAllExpiredContractorsReport(this.filter2).then((res) => {
               this.forceFileDownload(res, this.$t('GetExpiredContractorsReport'));
               this.PrintLoading = false;
            });
         }
      }
   },
   watch: {
      tabData: function (newVal) {
         this.PrintLoading = false;
      }
   }
};
</script>

<style lang="scss" scoped>
@import '../styles.scss';
</style>
