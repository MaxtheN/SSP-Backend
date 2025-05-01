<template>
   <b-card no-body>
      <div class="m-2">
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
            <b-col sm="12" md="3">
               <form-picker
                  :label="$t('startDate')"
                  v-model="filter.year"
                  :placeholder="$t('startDate')"
                  @input="Refresh"
                  format="YYYY"
                  type="year"
               ></form-picker>
            </b-col>
            <b-col cols="12" md="3">
               <label>{{ $t('inn') }}</label>
               <b-input-group class="text-right">
                  <b-form-input v-model="filter.inn" @keyup.enter="Refresh" :placeholder="$t('inn')" />
                  <b-input-group-append>
                     <b-button @click="Refresh" variant="primary">
                        <feather-icon icon="SearchIcon" />
                     </b-button>
                  </b-input-group-append>
               </b-input-group>
            </b-col>
            <b-col sm="12" md="2" class="mt-2 ml-auto text-right">
               <b-button @click="Print" :disabled="PrintLoading" variant="primary" class="ml-1">
                  <b-spinner small class="mr-1" v-if="PrintLoading"></b-spinner>
                  <feather-icon icon="PrinterIcon"></feather-icon>
                  {{ $t('Print') }}
               </b-button>
            </b-col>
         </b-row>
         <b-row>
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
         </b-row>
      </div>
      <div class="m-2">
         <b-overlay :show="isBusy">
            <b-table-simple hover small class="report-table" caption-top responsive border>
               <b-thead>
                  <b-tr>
                     <b-th colspan="35">
                        {{ $t('Давлат солиқ қўмитасидан интеграция орқали олинаётган маълумотлар') }}
                     </b-th>
                  </b-tr>
                  <b-tr>
                     <b-th rowspan="4" style="vertical-align: middle">{{ $t('order') }}</b-th>
                     <b-th
                        rowspan="4"
                        style="vertical-align: middle"
                        class="table-b-table-default"
                        :class="{ 'b-table-sticky-column': !isMobileDevice() }"
                     >
                        <span v-show="filter.byRegion">{{ $t('region') }}</span>
                        <span v-show="filter.byDistrict">{{ $t('Region') }}</span>
                        <span v-show="filter.byContractor">{{ $t('contractorT') }}</span>
                     </b-th>
                     <b-th rowspan="4" style="vertical-align: middle">
                        {{ $t('contractorCount') }}
                     </b-th>

                     <b-th colspan="25">{{ $t('Ишчи ходимлар ва хисобланган солиқ суммаси') }}</b-th>
                     <b-th colspan="5">{{ $t('Маҳсулот (товар, иш ва хизмат)ларни сотишдан соф тушум') }}</b-th>
                     <b-th rowspan="3">{{ $t('Солиқ тўловлари бўйича мавжуд қарздорлиги') }}</b-th>
                  </b-tr>
                  <b-tr>
                     <b-th>{{ $t('Total') }}</b-th>
                     <b-th colspan="2">{{ $t('month1') }}</b-th>
                     <b-th colspan="2">{{ $t('month2') }}</b-th>
                     <b-th colspan="2">{{ $t('month3') }}</b-th>
                     <b-th colspan="2">{{ $t('month4') }}</b-th>
                     <b-th colspan="2">{{ $t('month5') }}</b-th>
                     <b-th colspan="2">{{ $t('month6') }}</b-th>
                     <b-th colspan="2">{{ $t('month7') }}</b-th>
                     <b-th colspan="2">{{ $t('month8') }}</b-th>
                     <b-th colspan="2">{{ $t('month9') }}</b-th>
                     <b-th colspan="2">{{ $t('month10') }}</b-th>
                     <b-th colspan="2">{{ $t('month11') }}</b-th>
                     <b-th colspan="2">{{ $t('month12') }}</b-th>

                     <b-th rowspan="2">{{ $t('Total') }}</b-th>
                     <b-th rowspan="2">{{ $t('1 чорак') }}</b-th>
                     <b-th rowspan="2">{{ $t('2 чорак') }}</b-th>
                     <b-th rowspan="2">{{ $t('3 чорак') }}</b-th>
                     <b-th rowspan="2">{{ $t('4 чорак') }}</b-th>
                  </b-tr>
                  <b-tr>
                     <b-th>{{ $t('paymentTax') }}</b-th>
                     <template v-for="i in 12">
                        <b-th :key="i + 'count'">{{ $t('employeesCount') }}</b-th>
                        <b-th :key="i + 'paymentTax'">{{ $t('paymentTax') }}</b-th>
                     </template>
                  </b-tr>
               </b-thead>

               <template>
                  <b-tbody v-if="items.length > 0">
                     <b-tr v-for="(item, idx) in items" :key="idx + 'abc'">
                        <b-td>{{ idx + 1 }}</b-td>
                        <b-td
                           style="vertical-align: middle"
                           class="table-b-table-default"
                           :class="{ 'b-table-sticky-column': !isMobileDevice() }"
                        >
                           <span v-show="filter.byRegion">
                              <span style="color: blue; cursor: pointer" @click="SortRegion(item)">
                                 {{ item.region }}
                              </span>
                           </span>
                           <span v-show="filter.byDistrict">
                              <span style="color: blue; cursor: pointer" @click="SortDistrict(item)">
                                 {{ item.district }}
                              </span>
                           </span>

                           <span v-show="filter.byContractor">
                              <span style="color: blue; cursor: pointer" @click="goToBussnes(item.contractorInn)">{{
                                 item.contractorInn
                              }}</span>
                              -
                              {{ item.contractorName }}
                           </span>
                        </b-td>
                        <b-td class="text-right">{{ currency(item.contractorCount) }} </b-td>

                        <!-- <b-td class="text-right">{{ currency(item.totalNumberEmployee) }} </b-td> -->
                        <b-td class="text-right">{{ currency(item.totalPaymentTax, 2) }} </b-td>

                        <b-td class="text-right">{{ currency(item.numberEmployee1) }} </b-td>
                        <b-td class="text-right">{{ currency(item.paymentTax1, 2) }} </b-td>

                        <b-td class="text-right">{{ currency(item.numberEmployee2) }} </b-td>
                        <b-td class="text-right">{{ currency(item.paymentTax2, 2) }} </b-td>

                        <b-td class="text-right">{{ currency(item.numberEmployee3) }} </b-td>
                        <b-td class="text-right">{{ currency(item.paymentTax3, 2) }} </b-td>

                        <b-td class="text-right">{{ currency(item.numberEmployee4) }} </b-td>
                        <b-td class="text-right">{{ currency(item.paymentTax4, 2) }} </b-td>

                        <b-td class="text-right">{{ currency(item.numberEmployee5) }} </b-td>
                        <b-td class="text-right">{{ currency(item.paymentTax5, 2) }} </b-td>

                        <b-td class="text-right">{{ currency(item.numberEmployee6) }} </b-td>
                        <b-td class="text-right">{{ currency(item.paymentTax6, 2) }} </b-td>

                        <b-td class="text-right">{{ currency(item.numberEmployee7) }} </b-td>
                        <b-td class="text-right">{{ currency(item.paymentTax7, 2) }} </b-td>

                        <b-td class="text-right">{{ currency(item.numberEmployee8) }} </b-td>
                        <b-td class="text-right">{{ currency(item.paymentTax8, 2) }} </b-td>

                        <b-td class="text-right">{{ currency(item.numberEmployee9) }} </b-td>
                        <b-td class="text-right">{{ currency(item.paymentTax9, 2) }} </b-td>

                        <b-td class="text-right">{{ currency(item.numberEmployee10) }} </b-td>
                        <b-td class="text-right">{{ currency(item.paymentTax10, 2) }} </b-td>

                        <b-td class="text-right">{{ currency(item.numberEmployee11) }} </b-td>
                        <b-td class="text-right">{{ currency(item.paymentTax11, 2) }} </b-td>

                        <b-td class="text-right">{{ currency(item.numberEmployee12) }} </b-td>
                        <b-td class="text-right">{{ currency(item.paymentTax12, 2) }} </b-td>

                        <b-td class="text-right">{{ currency(item.totalNetIncome, 2) }} </b-td>
                        <b-td class="text-right">{{ currency(item.netIncome1, 2) }} </b-td>
                        <b-td class="text-right">{{ currency(item.netIncome2, 2) }} </b-td>
                        <b-td class="text-right">{{ currency(item.netIncome3, 2) }} </b-td>
                        <b-td class="text-right">{{ currency(item.netIncome4, 2) }} </b-td>
                        <b-td class="text-right">{{ currency(item.totalTaxDebt, 2) }} </b-td>
                     </b-tr>
                  </b-tbody>
                  <b-tfoot v-if="items.length > 0">
                     <b-tr variant="secondary">
                        <b-td class="text-right" colspan="2">{{ $t('Total') }}</b-td>
                        <b-td class="text-right">{{ currency(totals.contractorCount) }} </b-td>

                        <!-- <b-td class="text-right">{{ currency(totals.totalNumberEmployee) }} </b-td> -->
                        <b-td class="text-right">{{ currency(totals.totalPaymentTax, 2) }} </b-td>

                        <b-td class="text-right">{{ currency(totals.numberEmployee1) }} </b-td>
                        <b-td class="text-right">{{ currency(totals.paymentTax1, 2) }} </b-td>

                        <b-td class="text-right">{{ currency(totals.numberEmployee2) }} </b-td>
                        <b-td class="text-right">{{ currency(totals.paymentTax2, 2) }} </b-td>

                        <b-td class="text-right">{{ currency(totals.numberEmployee3) }} </b-td>
                        <b-td class="text-right">{{ currency(totals.paymentTax3, 2) }} </b-td>

                        <b-td class="text-right">{{ currency(totals.numberEmployee4) }} </b-td>
                        <b-td class="text-right">{{ currency(totals.paymentTax4, 2) }} </b-td>

                        <b-td class="text-right">{{ currency(totals.numberEmployee5) }} </b-td>
                        <b-td class="text-right">{{ currency(totals.paymentTax5, 2) }} </b-td>

                        <b-td class="text-right">{{ currency(totals.numberEmployee6) }} </b-td>
                        <b-td class="text-right">{{ currency(totals.paymentTax6, 2) }} </b-td>

                        <b-td class="text-right">{{ currency(totals.numberEmployee7) }} </b-td>
                        <b-td class="text-right">{{ currency(totals.paymentTax7, 2) }} </b-td>

                        <b-td class="text-right">{{ currency(totals.numberEmployee8) }} </b-td>
                        <b-td class="text-right">{{ currency(totals.paymentTax8, 2) }} </b-td>

                        <b-td class="text-right">{{ currency(totals.numberEmployee9) }} </b-td>
                        <b-td class="text-right">{{ currency(totals.paymentTax9, 2) }} </b-td>

                        <b-td class="text-right">{{ currency(totals.numberEmployee10) }} </b-td>
                        <b-td class="text-right">{{ currency(totals.paymentTax10, 2) }} </b-td>

                        <b-td class="text-right">{{ currency(totals.numberEmployee11) }} </b-td>
                        <b-td class="text-right">{{ currency(totals.paymentTax11, 2) }} </b-td>

                        <b-td class="text-right">{{ currency(totals.numberEmployee12) }} </b-td>
                        <b-td class="text-right">{{ currency(totals.paymentTax12, 2) }} </b-td>

                        <b-td class="text-right">{{ currency(totals.totalNetIncome, 2) }} </b-td>
                        <b-td class="text-right">{{ currency(totals.netIncome1, 2) }} </b-td>
                        <b-td class="text-right">{{ currency(totals.netIncome2, 2) }} </b-td>
                        <b-td class="text-right">{{ currency(totals.netIncome3, 2) }} </b-td>
                        <b-td class="text-right">{{ currency(totals.netIncome4, 2) }} </b-td>

                        <b-td class="text-right">{{ currency(totals.totalTaxDebt, 2) }} </b-td>
                     </b-tr>
                  </b-tfoot>
               </template>
            </b-table-simple>
            <template #overlay>
               <div class="text-center text-primary my-2">
                  <b-spinner scale="2" class="align-middle mr-2"></b-spinner>
                  <strong>{{ $t('Loading') }}...</strong>
               </div>
            </template>
         </b-overlay>
      </div>
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
   BTfoot,
   BOverlay
} from 'bootstrap-vue';
import ReportService from '@/services/report/report.service';

import RegionService from '@/services/info/region.service';
import DistrictService from '@/services/info/district.service';

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
      BOverlay
   },
   directives: {
      'b-tooltip': VBTooltip,
      'b-modal': VBModal
   },
   data() {
      return {
         items: [],
         RegionList: [],
         DistrictList: [],
         fields: [],
         PrintLoading: false,
         filter: {
            id: null,
            inn: null,
            year: new Date().getFullYear(),
            regionId: null,
            region: '',
            byRegion: true,
            districtId: null,
            district: '',
            byDistrict: false,
            contractorId: null,
            byContractor: false,
            hasCertificate: true
         },
         isBusy: false,
         totals: {}
      };
   },
   created() {
      this.getFields();
      this.Refresh();

      RegionService.GetAsSelectList(211)
         .then((res) => {
            this.RegionList = res.data;
         })
         .catch((error) => {
            this.showApiError(error);
         });
   },
   methods: {
      getFields() {
         this.fields = [
            {
               key: 'order',
               label: this.$t('№'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: this.filter.byRegion ? 'region' : '',
               label: this.$t('Oblast'),
               sortable: true
            },
            {
               key: this.filter.byDistrict ? 'district' : '',
               label: this.$t('Region'),
               sortable: true
            },
            {
               key: this.filter.byContractor ? 'contractor' : '',
               label: this.$t('contractorT'),
               sortable: true
            },
            {
               key: this.filter.byContractor ? 'prtnContractType' : '',
               label: this.$t('prtnContractType'),
               sortable: true
            },
            {
               key: 'totalApplicationCount',
               label: this.$t('totalApplicationCount'),
               thClass: 'text-right',
               tdClass: 'text-right',
               sortable: true
            },

            {
               key: 'totalPrtnApplicationSentCount',
               label: this.$t('totalPrtnApplicationCount'),
               thClass: 'text-right',
               tdClass: 'text-right',
               sortable: true
            },

            {
               key: 'totalPrtnApplicationSentForReviewCount',
               label: this.$t('totalPrtnApplicationSentForReviewCount'),
               thClass: 'text-right',
               tdClass: 'text-right',
               sortable: true
            },
            {
               key: 'totalPrtnApplicationSentRejectedCount',
               label: this.$t('totalPrtnApplicationSentRejectedCount'),
               thClass: 'text-right',
               tdClass: 'text-right',
               sortable: true
            },

            {
               key: 'totalPrtnContractCount',
               label: this.$t('totalPrtnContractCount'),
               thClass: 'text-right',
               tdClass: 'text-right',
               sortable: true
            },

            {
               key: 'totalPrtnApplicationSentForExpertisesCount',
               label: this.$t('totalPrtnApplicationSentForExpertisesCount'),
               thClass: 'text-right',
               tdClass: 'text-right',
               sortable: true
            },

            {
               key: 'totalPrtnApplicationNotPassExpertisesCount',
               label: this.$t('totalPrtnApplicationNotPassExpertisesCount'),
               thClass: 'text-right',
               tdClass: 'text-right',
               sortable: true
            },

            {
               key: 'totalPrtnApplicationPassExpertisesCount',
               label: this.$t('totalPrtnApplicationPassExpertisesCount'),
               thClass: 'text-right',
               tdClass: 'text-right',
               sortable: true
            },
            {
               key: 'totalPrtnContractCancelCount',
               label: this.$t('totalPrtnContractCancelCount'),
               thClass: 'text-right',
               tdClass: 'text-right',
               sortable: true
            },
            {
               key: 'totalPrtnApplicationSignningCount',
               label: this.$t('totalPrtnApplicationSignningCount'),
               thClass: 'text-right',
               tdClass: 'text-right',
               sortable: true
            },
            {
               key: 'totalPrtnApplicationSignedCount',
               label: this.$t('totalPrtnApplicationSignedCount'),
               thClass: 'text-right',
               tdClass: 'text-right',
               sortable: true
            },

            {
               key: 'totalPrtnCertificateCount',
               label: this.$t('totalPrtnCertificateCount'),
               thClass: 'text-right',
               tdClass: 'text-right',
               sortable: true
            },

            {
               key: 'totalNewVacanciesCount',
               label: this.$t('totalNewVacanciesCount'),
               thClass: 'text-right',
               tdClass: 'text-right',
               sortable: true
            }
         ];
      },
      Print() {
         this.PrintLoading = true;
         ReportService.SaveAsExcelSoliqReportByContractor(this.filter)
            .then((res) => {
               this.forceFileDownload(res, this.$t('GetSoliqReportByContractor'));
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
         this.getFields();
         this.isBusy = true;
         ReportService.GetSoliqReportByContractor(this.filter)
            .then((res) => {
               this.items = res.data;
               this.totals = {};

               res.data.forEach((item) => {
                  this.totals.totalNumberEmployee =
                     Number(this.totals.totalNumberEmployee || 0) + item.totalNumberEmployee;
                  this.totals.totalPaymentTax = Number(this.totals.totalPaymentTax || 0) + item.totalPaymentTax;
                  this.totals.numberEmployee1 = Number(this.totals.numberEmployee1 || 0) + item.numberEmployee1;
                  this.totals.paymentTax1 = Number(this.totals.paymentTax1 || 0) + item.paymentTax1;
                  this.totals.numberEmployee2 = Number(this.totals.numberEmployee2 || 0) + item.numberEmployee2;
                  this.totals.paymentTax2 = Number(this.totals.paymentTax2 || 0) + item.paymentTax2;
                  this.totals.numberEmployee3 = Number(this.totals.numberEmployee3 || 0) + item.numberEmployee3;
                  this.totals.paymentTax3 = Number(this.totals.paymentTax3 || 0) + item.paymentTax3;
                  this.totals.numberEmployee4 = Number(this.totals.numberEmployee4 || 0) + item.numberEmployee4;
                  this.totals.paymentTax4 = Number(this.totals.paymentTax4 || 0) + item.paymentTax4;
                  this.totals.numberEmployee5 = Number(this.totals.numberEmployee5 || 0) + item.numberEmployee5;
                  this.totals.paymentTax5 = Number(this.totals.paymentTax5 || 0) + item.paymentTax5;
                  this.totals.numberEmployee6 = Number(this.totals.numberEmployee6 || 0) + item.numberEmployee6;
                  this.totals.paymentTax6 = Number(this.totals.paymentTax6 || 0) + item.paymentTax6;
                  this.totals.numberEmployee7 = Number(this.totals.numberEmployee7 || 0) + item.numberEmployee7;
                  this.totals.paymentTax7 = Number(this.totals.paymentTax7 || 0) + item.paymentTax7;
                  this.totals.numberEmployee8 = Number(this.totals.numberEmployee8 || 0) + item.numberEmployee8;
                  this.totals.paymentTax8 = Number(this.totals.paymentTax8 || 0) + item.paymentTax8;
                  this.totals.numberEmployee9 = Number(this.totals.numberEmployee9 || 0) + item.numberEmployee9;
                  this.totals.paymentTax9 = Number(this.totals.paymentTax9 || 0) + item.paymentTax9;
                  this.totals.numberEmployee10 = Number(this.totals.numberEmployee10 || 0) + item.numberEmployee10;
                  this.totals.paymentTax10 = Number(this.totals.paymentTax10 || 0) + item.paymentTax10;
                  this.totals.numberEmployee11 = Number(this.totals.numberEmployee11 || 0) + item.numberEmployee11;
                  this.totals.paymentTax11 = Number(this.totals.paymentTax11 || 0) + item.paymentTax11;
                  this.totals.numberEmployee12 = Number(this.totals.numberEmployee12 || 0) + item.numberEmployee12;
                  this.totals.paymentTax12 = Number(this.totals.paymentTax12 || 0) + item.paymentTax12;
                  this.totals.totalNetIncome = Number(this.totals.totalNetIncome || 0) + item.totalNetIncome;
                  this.totals.netIncome1 = Number(this.totals.netIncome1 || 0) + item.netIncome1;
                  this.totals.netIncome2 = Number(this.totals.netIncome2 || 0) + item.netIncome2;
                  this.totals.netIncome3 = Number(this.totals.netIncome3 || 0) + item.netIncome3;
                  this.totals.netIncome4 = Number(this.totals.netIncome4 || 0) + item.netIncome4;
                  this.totals.totalTaxDebt = Number(this.totals.totalTaxDebt || 0) + item.totalTaxDebt;
                  this.totals.contractorCount = Number(this.totals.contractorCount || 0) + item.contractorCount;
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
};
</script>

<style lang="scss">
@import '../styles.scss';
</style>
