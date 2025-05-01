<template>
   <b-card no-body>
      <div class="m-2">
         <b-row>
            <b-col sm="12" md="3">
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
            <b-col sm="12" md="3">
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
               <form-picker v-model="filter.year" type="year" @input="Refresh" format="YYYY" :label="$t('docyear')" />
            </b-col>
            <b-col sm="12" md="2">
               <b-button @click="Refresh" :disabled="isBusy" variant="primary" class="mt-2">
                  <feather-icon icon="SearchIcon" />
                  {{ $t('Refresh') }}
               </b-button>
            </b-col>
            <b-col cols="auto" md="2" class="text-right">
               <b-button @click="Print" :disabled="PrintLoading" variant="primary" class="mt-2">
                  <feather-icon icon="PrinterIcon"></feather-icon>
                  {{ $t('Print') }}
               </b-button>
            </b-col>
         </b-row>
         <b-row align-h="between" v-show="filter.region" class="mt-2">
            <b-col sm="12" md="8">
               <b-breadcrumb>
                  <b-breadcrumb-item v-show="filter.region" :active="!!filter.region" @click="handleBread('region')">
                     <b>{{ $t('uzb') }}</b>
                  </b-breadcrumb-item>
                  <b-breadcrumb-item v-show="filter.region" :active="!!filter.region" @click="handleBread('district')">
                     <b>{{ filter.region }}</b>
                  </b-breadcrumb-item>
                  <b-breadcrumb-item v-show="filter.district" :active="!!filter.district" @click="handleBread('bank')">
                     <b>{{ filter.district }}</b>
                  </b-breadcrumb-item>
                  <b-breadcrumb-item v-show="filter.bank" :active="!!filter.bank">
                     <b>{{ filter.bank }}</b>
                  </b-breadcrumb-item>
               </b-breadcrumb>
            </b-col>
         </b-row>
      </div>
      <div class="mx-2">
         <b-overlay :show="isBusy">
            <b-table-simple class="report-table" hover caption-top responsive striped border>
               <b-thead>
                  <b-tr>
                     <b-th rowspan="4" style="vertical-align: middle">{{ $t('order') }}</b-th>
                     <b-th rowspan="4" style="vertical-align: middle">
                        <span v-if="filter.byContractor">{{ $t('contractor') }}</span>
                        <span v-else-if="filter.byBank">{{ $t('bankName') }}</span>
                        <span v-else-if="filter.byDistrict">{{ $t('Region') }}</span>
                        <span v-else-if="filter.byRegion">{{ $t('Oblast') }}</span>
                     </b-th>

                     <b-th colspan="6" rowspan="2">{{
                        $t('Дастур доирасида кредит олиш учун электрон шаклда ариза тақдим этган субъектлар сони')
                     }}</b-th>
                     <b-th colspan="19">{{ $t('shundan') }}</b-th>
                  </b-tr>
                  <b-tr>
                     <b-th colspan="6">{{ $t('51-100 тагача иш ўрни яратадиган') }}</b-th>
                     <b-th colspan="6">{{ $t('101-200 тагача иш ўрни яратадиган') }}</b-th>
                     <b-th colspan="6">{{ $t('200 дан ортиқ иш ўрни яратадиган') }}</b-th>
                  </b-tr>
                  <b-tr>
                     <template v-for="h in 4">
                        <b-th rowspan="2" :key="h + 'col0'">{{ $t('Субъект сони') }}</b-th>
                        <b-th rowspan="2" :key="h + 'col1'">{{ $t('Кредитга талаб (сўм)') }}</b-th>
                        <b-th rowspan="2" :key="h + 'col2'">{{ $t('Аризаси рад қилинганлар сони') }}</b-th>
                        <b-th rowspan="2" :key="h + 'col3'">{{ $t('Яратила-диган иш ўрни сони') }}</b-th>
                        <b-th colspan="2" :key="h + 'col5'">{{ $t('Кредит ажратилган аризалар') }}</b-th>
                     </template>
                  </b-tr>
                  <b-tr>
                     <template v-for="i in 4">
                        <b-th :key="i + 'quantity'">{{ $t('quantity') }}</b-th>
                        <b-th :key="i + 'amount'">{{ $t('amount') }}</b-th>
                     </template>
                  </b-tr>
               </b-thead>

               <b-tbody v-if="items.length > 0">
                  <b-tr v-for="(item, idx) in items" :key="idx + 'abc'">
                     <b-td>{{ idx + 1 }}</b-td>
                     <b-td class="table-b-table-default" :class="{ 'b-table-sticky-column': !isMobileDevice() }">
                        <span v-if="filter.byContractor">
                           <span style="color: blue; cursor: pointer" @click="goToBussnes(item.contractorInn)">{{
                              item.contractorInn
                           }}</span>
                           -
                           {{ item.contractor }}
                        </span>
                        <span v-else-if="filter.byBank" @click="handleBank(item)" style="color: blue; cursor: pointer">
                           {{ item.bankMfo }} -{{ item.bankName }}
                        </span>
                        <span
                           v-else-if="filter.byDistrict"
                           style="color: blue; cursor: pointer"
                           @click="handleDistrict(item)"
                        >
                           {{ item.districtName }}
                        </span>
                        <span
                           v-else-if="filter.byRegion"
                           style="color: blue; cursor: pointer"
                           @click="handleRegion(item)"
                        >
                           {{ item.regionName }}
                        </span>
                     </b-td>
                     <!-- Application -->
                     <b-td class="text-right">
                        {{ currency(item.application.approvedCount) }}
                     </b-td>
                     <b-td class="text-right">
                        {{ currency(item.application.approvedSum) }}
                     </b-td>

                     <b-td class="text-right"> {{ currency(item.application.rejectedCount) }} </b-td>
                     <b-td class="text-right"> 0 </b-td>
                     <b-td class="text-right">
                        {{ currency(item.application.issuanceCount) }}
                     </b-td>
                     <b-td class="text-right">
                        {{ currency(item.application.issuanceSum) }}
                     </b-td>
                     <!-- contractType1 -->
                     <b-td class="text-right">
                        {{ currency(item.contractType1.approvedCount) }}
                     </b-td>
                     <b-td class="text-right">
                        {{ currency(item.contractType1.approvedSum) }}
                     </b-td>

                     <b-td class="text-right"> {{ currency(item.contractType1.rejectedCount) }} </b-td>
                     <b-td class="text-right"> 0 </b-td>
                     <b-td class="text-right">
                        {{ currency(item.contractType1.issuanceCount) }}
                     </b-td>
                     <b-td class="text-right">
                        {{ currency(item.contractType1.issuanceSum) }}
                     </b-td>
                     <!-- contractType2 -->
                     <b-td class="text-right">
                        {{ currency(item.contractType2.approvedCount) }}
                     </b-td>
                     <b-td class="text-right">
                        {{ currency(item.contractType2.approvedSum) }}
                     </b-td>
                     <b-td class="text-right"> {{ currency(item.contractType2.rejectedCount) }} </b-td>
                     <b-td class="text-right"> 0 </b-td>
                     <b-td class="text-right">
                        {{ currency(item.contractType2.issuanceCount) }}
                     </b-td>
                     <b-td class="text-right">
                        {{ currency(item.contractType2.issuanceSum) }}
                     </b-td>
                     <!-- contractType3 -->
                     <b-td class="text-right">
                        {{ currency(item.contractType3.approvedCount) }}
                     </b-td>
                     <b-td class="text-right">
                        {{ currency(item.contractType3.approvedSum) }}
                     </b-td>
                     <b-td class="text-right"> {{ currency(item.contractType3.rejectedCount) }} </b-td>
                     <b-td class="text-right"> 0 </b-td>
                     <b-td class="text-right">
                        {{ currency(item.contractType3.issuanceCount) }}
                     </b-td>
                     <b-td class="text-right">
                        {{ currency(item.contractType3.issuanceSum) }}
                     </b-td>
                  </b-tr>
               </b-tbody>
               <b-tfoot v-if="items.length > 0">
                  <b-tr variant="secondary">
                     <b-td class="text-right" :class="{ 'b-table-sticky-column': !isMobileDevice() }"></b-td>
                     <b-td class="text-right" :class="{ 'b-table-sticky-column': !isMobileDevice() }">{{
                        $t('Total')
                     }}</b-td>
                     <!-- Application -->
                     <b-td class="text-right">
                        {{ currency(totals.approvedCount) }}
                     </b-td>
                     <b-td class="text-right">
                        {{ currency(totals.approvedSum) }}
                     </b-td>

                     <b-td class="text-right">
                        {{ currency(totals.rejectedCount) }}
                     </b-td>
                     <b-td class="text-right">
                        {{ currency(totals.employeeCount) }}
                     </b-td>
                     <b-td class="text-right">
                        {{ currency(totals.issuanceCount) }}
                     </b-td>
                     <b-td class="text-right">
                        {{ currency(totals.issuanceSum) }}
                     </b-td>

                     <!-- contractType1 -->
                     <b-td class="text-right">
                        {{ currency(totals.approvedCount1) }}
                     </b-td>
                     <b-td class="text-right">
                        {{ currency(totals.approvedSum1) }}
                     </b-td>
                     <b-td class="text-right">
                        {{ currency(totals.rejectedCount1) }}
                     </b-td>
                     <b-td class="text-right">
                        {{ currency(totals.employeeCount1) }}
                     </b-td>
                     <b-td class="text-right">
                        {{ currency(totals.issuanceCount1) }}
                     </b-td>
                     <b-td class="text-right">
                        {{ currency(totals.issuanceSum1) }}
                     </b-td>

                     <!-- contractType2 -->
                     <b-td class="text-right">
                        {{ currency(totals.approvedCount2) }}
                     </b-td>
                     <b-td class="text-right">
                        {{ currency(totals.approvedSum2) }}
                     </b-td>
                     <b-td class="text-right">
                        {{ currency(totals.rejectedCount2) }}
                     </b-td>

                     <b-td class="text-right">
                        {{ currency(totals.employeeCount2) }}
                     </b-td>
                     <b-td class="text-right">
                        {{ currency(totals.issuanceCount2) }}
                     </b-td>
                     <b-td class="text-right">
                        {{ currency(totals.issuanceSum2) }}
                     </b-td>

                     <!-- contractType3 -->
                     <b-td class="text-right">
                        {{ currency(totals.approvedCount3) }}
                     </b-td>
                     <b-td class="text-right">
                        {{ currency(totals.approvedSum3) }}
                     </b-td>
                     <b-td class="text-right">
                        {{ currency(totals.rejectedCount3) }}
                     </b-td>

                     <b-td class="text-right">
                        {{ currency(totals.employeeCount3) }}
                     </b-td>
                     <b-td class="text-right">
                        {{ currency(totals.issuanceCount3) }}
                     </b-td>
                     <b-td class="text-right">
                        {{ currency(totals.issuanceSum3) }}
                     </b-td>
                  </b-tr>
               </b-tfoot>
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
         PrintLoading: false,
         filter: {
            year: new Date().getFullYear(),
            bankMfo: null,
            regionId: null,
            region: '',
            districtId: null,
            district: '',
            bankId: null,
            bank: null,
            byRegion: true,
            byDistrict: false,
            byBank: false,
            byContractor: false
         },
         isBusy: false,
         totals: {
            approvedCount: 0,
            approvedSum: 0,
            rejectedCount: 0,
            issuanceCount: 0,
            issuanceSum: 0,
            employeeCount: 0,
            approvedCount1: 0,
            approvedSum1: 0,
            issuanceCount1: 0,
            rejectedCount1: 0,
            employeeCount1: 0,
            issuanceSum1: 0,
            approvedCount2: 0,
            approvedSum2: 0,
            employeeCount2: 0,
            rejectedCount2: 0,
            issuanceCount2: 0,
            issuanceSum2: 0,
            approvedCount3: 0,
            approvedSum3: 0,
            rejectedCount3: 0,
            employeeCount3: 0,
            issuanceCount3: 0,
            issuanceSum3: 0
         }
      };
   },
   created() {
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
      ChangeRegion(id) {
         this.filter.districtId = null;
         this.filter.district = null;

         if (id) {
            this.GetDistrict(id);
            this.filter.byDistrict = true;
            this.filter.byRegion = false;
            this.filter.byContractor = false;
         } else {
            this.filter.byDistrict = false;
            this.filter.byRegion = true;
            this.filter.byContractor = false;
         }
         this.Refresh();
      },
      ChangeDistrict(id) {
         if (id) {
            this.filter.byDistrict = false;
            this.filter.byRegion = false;
            this.filter.byContractor = true;
            this.filter.district = this.filter.districtId
               ? this.DistrictList.filter((item) => item.value === this.filter.districtId)[0].text
               : '';
            this.Refresh();
         } else {
            this.filter.byDistrict = true;
            this.filter.byRegion = false;
            this.filter.byContractor = false;
            this.filter.district = '';
            this.Refresh();
         }
      },
      goToBussnes(inn) {
         this.$router.push({ name: 'BusinessmanCard', query: { inn: inn } });
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
      handleRegion(item) {
         this.filter.regionId = item.regionId;
         this.filter.region = item.regionName;
         this.filter.byContractor = false;
         this.filter.byBank = false;
         this.filter.byDistrict = true;
         this.filter.byRegion = false;
         this.Refresh();
      },
      handleDistrict(item) {
         this.filter.districtId = item.districtId;
         this.filter.district = item.districtName;
         this.filter.byContractor = false;
         this.filter.byBank = true;
         this.filter.byDistrict = false;
         this.filter.byRegion = false;
         this.Refresh();
      },
      handleBank(item) {
         this.filter.bankMfo = item.bankMfo;
         this.filter.bank = item.bankName;
         this.filter.byContractor = true;
         this.filter.byBank = false;
         this.filter.byDistrict = false;
         this.filter.byRegion = false;
         this.Refresh();
      },
      handleBread(type) {
         if (type == 'region') {
            this.filter.byBank = false;
            this.filter.byDistrict = false;
            this.filter.byRegion = true;
            this.filter.regionId = null;
            this.filter.region = null;
            this.filter.districtId = null;
            this.filter.district = null;
            this.filter.byContractor = false;
         } else if (type == 'district') {
            this.filter.byBank = false;
            this.filter.byDistrict = true;
            this.filter.byRegion = false;
            this.filter.districtId = null;
            this.filter.district = null;
            this.filter.byContractor = false;
         } else if (type == 'bank') {
            this.filter.byBank = false;
            this.filter.byDistrict = true;
            this.filter.byRegion = false;
            this.filter.districtId = null;
            this.filter.district = null;
            this.filter.byContractor = true;
         }
         this.Refresh();
      },
      Print() {
         this.PrintLoading = true;
         ReportService.SaveSecondBankCreditReportAsExcel(this.filter)
            .then((res) => {
               this.forceFileDownload(res, this.$t('BankReportSecond'));
            })
            .catch((err) => {
               this.showApiError(err);
            })
            .finally(() => {
               this.PrintLoading = false;
            });
      },
      Refresh() {
         this.isBusy = true;
         this.items = [];

         ReportService.GetBankCreditReport(this.filter)
            .then((res) => {
               this.totals = {
                  approvedCount: 0,
                  approvedSum: 0,
                  issuanceCount: 0,
                  issuanceSum: 0,
                  employeeCount: 0,
                  rejectedCount: 0,
                  approvedCount1: 0,
                  approvedSum1: 0,
                  issuanceCount1: 0,
                  employeeCount1: 0,
                  issuanceSum1: 0,
                  rejectedCount1: 0,
                  approvedCount2: 0,
                  approvedSum2: 0,
                  employeeCount2: 0,
                  issuanceCount2: 0,
                  rejectedCount2: 0,
                  issuanceSum2: 0,
                  approvedCount3: 0,
                  approvedSum3: 0,
                  employeeCount3: 0,
                  rejectedCount3: 0,
                  issuanceCount3: 0,
                  issuanceSum3: 0
               };
               this.items = res.data;
               console.log(this.items);
               this.items.forEach((item) => {
                  this.totals.approvedCount += item.application.approvedCount;
                  console.log(this.totals.approvedCount);
                  this.totals.approvedSum += item.application.approvedSum;
                  this.totals.employeeCount = 0;
                  this.totals.rejectedCount += item.application.rejectedCount;
                  this.totals.issuanceCount += item.application.issuanceCount;
                  this.totals.issuanceSum += item.application.issuanceSum;

                  this.totals.approvedCount1 += item.contractType1.approvedCount;
                  this.rejectedCount1 += item.contractType1.rejectedCount1;
                  this.totals.approvedSum1 += item.contractType1.approvedSum;
                  this.totals.employeeCount1 = 0;
                  this.totals.issuanceCount1 += item.contractType1.issuanceCount;
                  this.totals.issuanceSum1 += item.contractType1.issuanceSum;

                  this.totals.approvedCount2 += item.contractType2.approvedCount;
                  this.totals.approvedSum2 += item.contractType2.approvedSum;
                  this.totals.rejectedCount3 += item.contractType2.rejectedCount3;
                  this.totals.employeeCount2 = 0;
                  this.totals.issuanceCount2 += item.contractType2.issuanceCount;
                  this.totals.issuanceSum2 += item.contractType2.issuanceSum;

                  this.totals.approvedCount3 += item.contractType3.approvedCount;
                  this.totals.approvedSum3 += item.contractType3.approvedSum;
                  this.totals.employeeCount3 = 0;
                  this.totals.issuanceCount3 += item.contractType3.issuanceCount;
                  this.totals.issuanceSum3 += item.contractType3.issuanceSum;
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
