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
            <b-col sm="12" md="2" class="ml-auto text-right mt-2">
               <b-button @click="Print" :disabled="PrintLoading" variant="primary">
                  <b-spinner small v-if="PrintLoading"></b-spinner>
                  <feather-icon icon="PrinterIcon"></feather-icon>
                  {{ $t('Print') }}
               </b-button>
            </b-col>
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
                        <b-th colspan="3" rowspan="2">{{ $t('Jami tushgan arizalar') }}</b-th>
                        <b-th colspan="3" rowspan="2"> {{ $t("Ko'rib chiqilayotgan") }} </b-th>
                        <b-th colspan="7">{{ $t('Bandlikdan javob kelgan') }} </b-th>
                     </b-tr>
                     <b-tr>
                        <b-th colspan="3">{{ $t('RejectedApplications') }}</b-th>
                        <b-th colspan="4">{{ $t('Tasdiqlangan arizalar') }}</b-th>
                     </b-tr>
                     <b-tr>
                        <b-th>{{ $t('count') }}</b-th>
                        <b-th>{{ $t('totalAmount') }}</b-th>
                        <b-th>{{ $t('totalAmountprice') }}</b-th>
                        <b-th>{{ $t('count') }}</b-th>
                        <b-th>{{ $t('totalAmount') }}</b-th>
                        <b-th>{{ $t('totalAmountprice') }}</b-th>
                        <b-th>{{ $t('count') }}</b-th>
                        <b-th>{{ $t('totalAmount') }}</b-th>
                        <b-th>{{ $t('totalAmountprice') }}</b-th>
                        <b-th>{{ $t('count') }}</b-th>
                        <b-th>{{ $t('totalAmount') }}</b-th>
                        <b-th>{{ $t('totalAmountprice') }}</b-th>
                        <b-th>{{ $t('Ajratilgan subsidiya') }}</b-th>
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
                              {{ item.regionName }}
                           </span>
                           <span
                              @click="SortDistrict(item)"
                              class="text-primary cursor-pointer"
                              v-show="filter.regionId != null && filter.districtId == null"
                           >
                              {{ item.districName }}
                           </span>
                           <span v-show="filter.regionId != null && filter.districtId != null">
                              {{ item.contractorName }}
                           </span>
                        </b-td>
                        <b-td class="text-right">{{ currency(item.allApplicaions?.count) }}</b-td>
                        <b-td class="text-right">{{ currency(item.allApplicaions?.totalAmount) }}</b-td>
                        <b-td class="text-right">{{ currency(item.allApplicaions?.totalCost) }}</b-td>
                        <b-td class="text-right">{{ currency(item.reviewApplications?.count) }}</b-td>
                        <b-td class="text-right">{{ currency(item.reviewApplications?.totalAmount) }}</b-td>
                        <b-td class="text-right">{{ currency(item.reviewApplications?.totalCost) }}</b-td>
                        <b-td class="text-right">{{ currency(item.askApplications?.acceptApplicaions?.count) }}</b-td>
                        <b-td class="text-right">{{
                           currency(item.askApplications?.acceptApplicaions?.totalAmount)
                        }}</b-td>
                        <b-td class="text-right">{{
                           currency(item.askApplications?.acceptApplicaions?.totalCost)
                        }}</b-td>
                        <b-td class="text-right">{{ currency(item.askApplications?.rejectApplicaions?.count) }}</b-td>
                        <b-td class="text-right">{{
                           currency(item.askApplications?.rejectApplicaions?.totalAmount)
                        }}</b-td>
                        <b-td class="text-right">{{
                           currency(item.askApplications?.rejectApplicaions?.totalCost)
                        }}</b-td>
                        <b-td class="text-right">{{
                           currency(item.askApplications?.rejectApplicaions?.subsidyAmount)
                        }}</b-td>
                     </b-tr>
                  </b-tbody>
                  <b-tfoot v-if="items.length > 0">
                     <b-tr variant="secondary">
                        <b-td class="text-right" :class="{ 'b-table-sticky-column': !isMobileDevice() }"></b-td>
                        <b-td class="text-right" :class="{ 'b-table-sticky-column': !isMobileDevice() }">{{
                           $t('Total')
                        }}</b-td>

                        <b-td class="text-right">{{ currency(totals?.allApplicaionscount) }}</b-td>
                        <b-td class="text-right">{{ currency(totals?.allApplicaionstotalAmount) }}</b-td>
                        <b-td class="text-right">{{ currency(totals?.allApplicaionstotalCost) }}</b-td>
                        <b-td class="text-right">{{ currency(totals?.reviewApplicationscount) }}</b-td>
                        <b-td class="text-right">{{ currency(totals?.reviewApplicationstotalAmount) }}</b-td>
                        <b-td class="text-right">{{ currency(totals?.reviewApplicationstotalCost) }}</b-td>
                        <b-td class="text-right">{{ currency(totals?.rejectApplicaionscount) }}</b-td>
                        <b-td class="text-right">{{ currency(totals?.rejectApplicaionstotalAmount) }}</b-td>
                        <b-td class="text-right">{{ currency(totals?.rejectApplicaionstotalCost) }}</b-td>
                        <b-td class="text-right">{{ currency(totals?.acceptApplicaionscount) }}</b-td>
                        <b-td class="text-right">{{ currency(totals?.acceptApplicaionstotalAmount) }}</b-td>
                        <b-td class="text-right">{{ currency(totals?.acceptApplicaionstotalCost) }}</b-td>
                        <b-td class="text-right">{{ currency(totals?.acceptApplicaionssubsidyAmount) }}</b-td>
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
            districtId: null,
            startDate: '',
            endDate: ''
         },
         totals: {
            allApplicaionscount: 0,
            allApplicaionstotalAmount: 0,
            allApplicaionstotalCost: 0,
            reviewApplicationscount: 0,
            reviewApplicationstotalAmount: 0,
            reviewApplicationstotalCost: 0,
            rejectApplicaionscount: 0,
            rejectApplicaionstotalAmount: 0,
            rejectApplicaionstotalCost: 0,
            acceptApplicaionscount: 0,
            acceptApplicaionstotalAmount: 0,
            acceptApplicaionstotalCost: 0,
            acceptApplicaionssubsidyAmount: 0
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
         this.filter.districtId = null;
         this.GetDistrict(item.regionId);
         this.Refresh();
      },
      SortDistrict(item) {
         this.filter.districtId = item.districtId;
         this.Refresh();
      },

      ChangeRegion(id) {
         if (id) {
            this.filter.districtId = null;
            this.filter.region = this.filter.regionId
               ? this.RegionList.filter((item) => item.value === this.filter.regionId)[0].text
               : '';

            this.Refresh();
            this.GetDistrict(id);
         } else {
            this.filter.districtId = null;
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
            this.filter.district = this.filter.districtId
               ? this.DistrictList.filter((item) => item.value === this.filter.districtId)[0].text
               : '';

            this.Refresh();
         } else {
            this.Refresh();
         }
      },

      Refresh() {
         this.isBusy = true;
         ReportService.MonoApplicationReport(this.filter)
            .then((res) => {
               this.items = res.data;
               (this.totals = {
                  allApplicaionscount: 0,
                  allApplicaionstotalAmount: 0,
                  allApplicaionstotalCost: 0,
                  reviewApplicationscount: 0,
                  reviewApplicationstotalAmount: 0,
                  reviewApplicationstotalCost: 0,
                  rejectApplicaionscount: 0,
                  rejectApplicaionstotalAmount: 0,
                  rejectApplicaionstotalCost: 0,
                  acceptApplicaionscount: 0,
                  acceptApplicaionstotalAmount: 0,
                  acceptApplicaionstotalCost: 0,
                  acceptApplicaionssubsidyAmount: 0
               }),
                  res.data.forEach((item) => {
                     this.totals.allApplicaionscount += item.allApplicaions?.count;
                     this.totals.allApplicaionstotalAmount += item.allApplicaions?.totalAmount;
                     this.totals.allApplicaionstotalCost += item.allApplicaions?.totalCost;
                     this.totals.reviewApplicationscount += item.reviewApplications?.count;
                     this.totals.reviewApplicationstotalAmount += item.reviewApplications?.totalAmount;
                     this.totals.reviewApplicationstotalCost += item.reviewApplications?.totalCost;
                     this.totals.rejectApplicaionscount += item.askApplications?.rejectApplicaions?.count;
                     this.totals.rejectApplicaionstotalAmount += item.askApplications?.rejectApplicaions?.totalAmount;
                     this.totals.rejectApplicaionstotalCost += item.askApplications?.rejectApplicaions?.totalCost;
                     this.totals.acceptApplicaionscount += item.askApplications?.acceptApplicaions?.count;
                     this.totals.acceptApplicaionstotalAmount += item.askApplications?.acceptApplicaions?.totalAmount;
                     this.totals.acceptApplicaionstotalCost += item.askApplications?.acceptApplicaions?.totalCost;
                     this.totals.acceptApplicaionssubsidyAmount +=
                        item.askApplications?.acceptApplicaions?.subsidyAmount;
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

         ReportService.MonoApplicationReportSaveExcel(this.filter).then((res) => {
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
