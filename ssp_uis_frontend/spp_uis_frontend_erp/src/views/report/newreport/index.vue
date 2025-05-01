<template>
   <b-card no-body>
      <div class="mr-2 ml-2 mt-2">
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
                     class="w-100"
                     @input="ChangeRegion"
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
                  @change="Refresh"
                  :label="$t('endDate')"
                  v-model="filter.endDate"
                  :placeholder="$t('endDate')"
               ></form-picker>
            </b-col>
            <!-- <b-col sm="12" md="2" class="ml-auto text-right mt-2">
               <b-button @click="Print" :disabled="PrintLoading" variant="primary">
                  <b-spinner small v-if="PrintLoading"></b-spinner>
                  <feather-icon icon="PrinterIcon"></feather-icon>
                  {{ $t('Print') }}
               </b-button>
            </b-col> -->
         </b-row>

         <!-- <b-col sm="12" md="8" class="mt-2">
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
         </b-col> -->

         <div class="mt-2 mb-2 report-table">
            <b-overlay :show="isBusy">
               <b-table-simple responsive class="table-b-table-default">
                  <b-thead>
                     <b-tr>
                        <b-th :class="{ 'b-table-sticky-column': !isMobileDevice() }" rowspan="4"> № </b-th>
                        <b-th :class="{ 'b-table-sticky-column': !isMobileDevice() }" rowspan="4"
                           ><span
                              v-show="filter.regionId == null && filter.districtId == null"
                              style="font-weight: 900; font-size: 14px; color: black"
                              >{{ $t('region') }}</span
                           >
                           <span
                              v-show="filter.regionId != null && filter.districtId == null"
                              style="font-weight: 900; font-size: 14px; color: black"
                              >{{ $t('Region') }}</span
                           >
                           <div
                              v-show="filter.regionId != null && filter.districtId != null"
                              style="font-weight: 900; font-size: 14px; color: black; width: 150px"
                           >
                              {{ $t('contractorT') }}
                           </div></b-th
                        >
                        <b-th>{{ $t('totalPrtnCertificateCount') }}</b-th>
                        <b-th>{{ $t('totalPlannedJobs') }}</b-th>
                        <b-th>{{ $t('todayPlan') }}</b-th>
                        <b-th>{{ $t('actualEmployeesCount') }}</b-th>
                        <b-th>{{ $t('totalEmployeesCount5') }}</b-th>
                        <b-th>{{ $t('initialEmployeesCount') }}</b-th>
                        <b-th>{{ $t('percentageOfCompletedPlan') }}</b-th>
                        <b-th>{{ $t('percentageGrowt') }}</b-th>
                     </b-tr>
                  </b-thead>
                  <b-tbody>
                     <b-tr v-for="(item, inx) in items" :key="inx">
                        <b-td class="table-b-table-default" :class="{ 'b-table-sticky-column': !isMobileDevice() }">{{
                           inx + 1
                        }}</b-td>
                        <b-td class="table-b-table-default" :class="{ 'b-table-sticky-column': !isMobileDevice() }">
                           <span
                              class="text-primary cursor-pointer"
                              @click="SortRegion(item)"
                              v-show="filter.regionId == null && filter.districtId == null"
                           >
                              {{ item.region }}
                           </span>
                           <span
                              @click="SortDistrict(item)"
                              class="text-primary cursor-pointer"
                              v-show="filter.regionId != null && filter.districtId == null"
                           >
                              {{ item.district }}
                           </span>
                           <span v-show="filter.regionId != null && filter.districtId != null">
                              {{ item.contractor }}
                           </span>
                        </b-td>
                        <b-td class="text-right">{{ currency(item.prtnCertificateCount) }}</b-td>
                        <b-td class="text-right">{{ currency(item.totalPlannedJobs) }}</b-td>
                        <b-td class="text-right">{{ currency(item.todayPlan) }}</b-td>
                        <b-td class="text-right">{{ currency(item.actualEmployeesCount) }}</b-td>
                        <b-td class="text-right">{{ currency(item.totalEmployeesCount) }}</b-td>
                        <b-td class="text-right">{{ currency(item.initialEmployeesCount) }}</b-td>
                        <b-td class="text-right">{{ currency(item.percentageOfCompletedPlan) }}</b-td>
                        <b-td class="text-right">{{ currency(item.percentageGrowt) }}</b-td>
                     </b-tr>
                  </b-tbody>
                  <b-tfoot v-if="items.length > 0">
                     <b-tr variant="secondary">
                        <b-td class="text-right" :class="{ 'b-table-sticky-column': !isMobileDevice() }"></b-td>
                        <b-td class="text-right" :class="{ 'b-table-sticky-column': !isMobileDevice() }">{{
                           $t('Total')
                        }}</b-td>

                        <b-td class="text-right">{{ currency(totals.prtnCertificateCount) }}</b-td>
                        <b-td class="text-right">{{ currency(totals.totalPlannedJobs) }}</b-td>
                        <b-td class="text-right">{{ currency(totals.todayPlan) }}</b-td>
                        <b-td class="text-right">{{ currency(totals.actualEmployeesCount) }}</b-td>
                        <b-td class="text-right">{{ currency(totals.totalEmployeesCount) }}</b-td>
                        <b-td class="text-right">{{ currency(totals.initialEmployeesCount) }}</b-td>
                     </b-tr>
                  </b-tfoot>
                  <template #overlay>
                     <div class="text-center text-primary my-2">
                        <b-spinner scale="2" class="align-middle mr-2"></b-spinner>
                        <strong>{{ $t('Loading') }}...</strong>
                     </div>
                  </template>
               </b-table-simple>
            </b-overlay>
         </div>
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
   BOverlay,
   BTabs,
   BTab
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
      BOverlay,
      BTabs,
      BTab
   },
   name: 'Index',
   directives: {
      'b-tooltip': VBTooltip,
      'b-modal': VBModal
   },
   data() {
      return {
         PrintLoading: false,
         items: [],
         RegionList: [],
         DistrictList: [],
         PrtnContractTypeList: [],
         filter: {
            regionId: null,
            byRegion: true,
            district: '',
            region: '',
            districtId: null,
            byDistrict: false,
            startDate: '',
            endDate: '',
            prtnContractTypeId: null,
            contractorId: null,
            byContractor: false
         },
         totals: {
            prtnCertificateCount: 0,
            totalPlannedJobs: 0,
            todayPlan: 0,
            actualEmployeesCount: 0,
            totalEmployeesCount: 0,
            initialEmployeesCount: 0,
            percentageOfCompletedPlan: 0,
            percentageGrowth: 0
         },

         isBusy: false
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
      SortRegion(item) {
         this.filter.regionId = item.regionId;
         this.filter.byContractor = false;
         this.filter.byDistrict = true;
         this.filter.byRegion = false;
         this.filter.districtId = null;
         this.GetDistrict(item.regionId);
         this.Refresh();
      },
      SortDistrict(item) {
         this.filter.districtId = item.districtId;
         this.filter.regionId = item.regionId;
         this.filter.byContractor = true;
         this.filter.byDistrict = false;
         this.filter.byRegion = false;
         this.Refresh();
      },

      ChangeRegion(id) {
         if (id) {
            this.filter.districtId = null;
            this.filter.byContractor = false;
            this.filter.byDistrict = true;
            this.filter.byRegion = false;
            this.filter.region = this.filter.regionId
               ? this.RegionList.filter((item) => item.value === this.filter.regionId)[0].text
               : '';

            this.Refresh();
            this.GetDistrict(id);
         } else {
            this.filter.districtId = null;
            this.filter.byContractor = false;
            this.filter.byDistrict = false;
            this.filter.byRegion = true;
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
         console.log(id);

         if (id) {
            console.log(id, 'ddd');
            this.filter.district = this.filter.districtId
               ? this.DistrictList.filter((item) => item.value === this.filter.districtId)[0].text
               : '';
            this.filter.byContractor = true;
            this.filter.byRegion = false;
            this.filter.byDistrict = false;
            this.Refresh();
         } else {
            console.log(id, 'null');

            this.filter.byRegion = false;
            this.filter.districtId = null;
            this.filter.byDistrict = true;
            this.filter.byContractor = false;
            this.Refresh();
         }
      },

      Refresh() {
         this.isBusy = true;
         ReportService.GetPrtnEmployeeJobReport(this.filter)
            .then((res) => {
               this.items = res.data;
               (this.totals = {
                  prtnCertificateCount: 0,
                  totalPlannedJobs: 0,
                  todayPlan: 0,
                  actualEmployeesCount: 0,
                  totalEmployeesCount: 0,
                  initialEmployeesCount: 0,
                  percentageOfCompletedPlan: 0,
                  percentageGrowth: 0
               }),
                  res.data.forEach((item) => {
                     this.totals.prtnCertificateCount += item.prtnCertificateCount;
                     this.totals.totalPlannedJobs += item.totalPlannedJobs;
                     this.totals.todayPlan += item.todayPlan;
                     this.totals.actualEmployeesCount += item.actualEmployeesCount;
                     this.totals.totalEmployeesCount += item.totalEmployeesCount;
                     this.totals.initialEmployeesCount += item.initialEmployeesCount;
                     this.totals.percentageOfCompletedPlan += item.percentageOfCompletedPlan;
                     this.totals.percentageGrowth += item.percentageGrowth;
                  });
            })
            .catch((error) => {
               this.showApiError(error);
            })
            .finally(() => {
               this.isBusy = false;
            });
      },
      Print() {
         this.PrintLoading = true;

         ReportService.GetPrtnEmployeeJobReport(this.filter).then((res) => {
            this.forceFileDownload(res, this.$t('MonoApplicationReport'));
            this.PrintLoading = false;
         });
      }
   }
};
</script>

<style lang="scss" scoped>
@import '../styles.scss';
</style>
