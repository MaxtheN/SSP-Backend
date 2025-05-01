<template>
   <b-card>
      <b-row>
         <b-col cols="12" md="2">
            <div>
               <label for>{{ $t('startdate') }}</label>
               <form-picker v-model="filter.fromDocDate" :placeholder="$t('startdate')" @input="Refresh" />
            </div>
         </b-col>
         <b-col cols="12" md="2">
            <div>
               <label for>{{ $t('enddate') }}</label>
               <form-picker v-model="filter.toDocDate" :placeholder="$t('enddate')" @input="Refresh" />
            </div>
         </b-col>
         <b-col sm="12" md="2">
            <div>
               <label for>{{ $t('region') }}</label>
               <v-select
                  :options="RegionList"
                  :reduce="(item) => item.value"
                  :placeholder="$t('ChooseBelow')"
                  label="text"
                  :disabled="localStorageData.organizationId != 1"
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
         <b-col> </b-col>
         <b-col cols="12" md="3">
            <label for>{{ $t('search') }}</label>
            <b-input-group class="text-right">
               <b-form-input v-model="filter.search" :placeholder="$t('search')" />
               <b-input-group-append>
                  <b-button @click="Refresh" variant="primary">
                     <feather-icon icon="SearchIcon" />
                  </b-button>
               </b-input-group-append>
            </b-input-group>
         </b-col>
         <b-col class="col-auto text-right mt-2">
            <b-button @click="Print" :disabled="PrintLoading" variant="primary">
               <feather-icon icon="PrinterIcon"></feather-icon>
               {{ $t('Print') }}
            </b-button>
         </b-col>
      </b-row>
      <b-row align-h="between">
         <b-col sm="12" md="8">
            <b-breadcrumb class="mt-1">
               <b-breadcrumb-item
                  :active="filter.byRegion"
                  @click="
                     () => {
                        filter.byDistrict = false;
                        filter.byRegion = true;
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
               <!-- <b-breadcrumb-item v-show="filter.byContractor" active>Baz</b-breadcrumb-item> -->
            </b-breadcrumb>
         </b-col>
      </b-row>
      <b-tabs class="mt-1 mb-1" nav-class="mb-0">
         <b-tab
            :active="filter.isOld === null"
            :title="$t('all')"
            @click="
               () => {
                  filter.isOld = null;
                  Refresh();
               }
            "
         >
         </b-tab>
         <b-tab
            :title="$t('new')"
            :active="filter.isOld === false"
            @click="
               () => {
                  filter.isOld = false;
                  Refresh();
               }
            "
         >
         </b-tab>
         <b-tab
            :title="$t('Eski')"
            :active="filter.isOld === true"
            @click="
               () => {
                  filter.isOld = true;
                  Refresh();
               }
            "
         >
         </b-tab>
      </b-tabs>
      <div class="report-table">
         <b-overlay :show="isBusy">
            <b-table-simple hover small caption-top responsive border>
               <b-thead>
                  <b-tr>
                     <b-th style="font-weight: 900; font-size: 14px; color: black" rowspan="3">{{ $t('order') }}</b-th>
                     <b-th
                        style="font-weight: 900; font-size: 14px; color: black"
                        rowspan="3"
                        class="table-b-table-default"
                        :class="{ 'b-table-sticky-column': !isMobileDevice() }"
                     >
                        <span v-show="filter.byRegion">{{ $t('region') }}</span>
                        <span v-show="filter.byDistrict">{{ $t('Region') }}</span>
                     </b-th>
                     <b-th rowspan="2" style="font-weight: 900; font-size: 14px; color: black">{{
                        $t('Қабул қилинган жами аризалар сони')
                     }}</b-th>
                     <b-th colspan="4" style="font-weight: 900; font-size: 14px; color: black">{{ $t('Шундан') }}</b-th>
                     <b-th rowspan="2" style="font-weight: 900; font-size: 14px; color: black">{{
                        $t('Жами имзоланган шартномалар сони')
                     }}</b-th>
                     <b-th colspan="4" style="font-weight: 900; font-size: 14px; color: black">{{ $t('Шундан') }}</b-th>
                     <b-th rowspan="2" style="font-weight: 900; font-size: 14px; color: black">{{
                        $t('Гувоҳнома берилганлар сони')
                     }}</b-th>
                     <b-th colspan="4" style="font-weight: 900; font-size: 14px; color: black">{{ $t('Шундан') }}</b-th>
                  </b-tr>
                  <b-tr>
                     <b-th style="font-weight: 900; font-size: 14px; color: black">{{ $t('Микрофирма') }}</b-th>
                     <b-th style="font-weight: 900; font-size: 14px; color: black">{{ $t('Кичик корхона') }}</b-th>
                     <b-th style="font-weight: 900; font-size: 14px; color: black">{{ $t('Ўрта корхона') }}</b-th>
                     <b-th style="font-weight: 900; font-size: 14px; color: black">{{ $t('Йирик корхона') }}</b-th>

                     <b-th style="font-weight: 900; font-size: 14px; color: black">{{ $t('Микрофирма') }}</b-th>
                     <b-th style="font-weight: 900; font-size: 14px; color: black">{{ $t('Кичик корхона') }}</b-th>
                     <b-th style="font-weight: 900; font-size: 14px; color: black">{{ $t('Ўрта корхона') }}</b-th>
                     <b-th style="font-weight: 900; font-size: 14px; color: black">{{ $t('Йирик корхона') }}</b-th>

                     <b-th style="font-weight: 900; font-size: 14px; color: black">{{ $t('Микрофирма') }}</b-th>
                     <b-th style="font-weight: 900; font-size: 14px; color: black">{{ $t('Кичик корхона') }}</b-th>
                     <b-th style="font-weight: 900; font-size: 14px; color: black">{{ $t('Ўрта корхона') }}</b-th>
                     <b-th style="font-weight: 900; font-size: 14px; color: black">{{ $t('Йирик корхона') }}</b-th>
                  </b-tr>
               </b-thead>
               <b-tbody>
                  <b-tr v-for="(item, idx) in items" :key="idx + 'abc'">
                     <b-td>{{ idx + 1 }}</b-td>
                     <b-td class="table-b-table-default" :class="{ 'b-table-sticky-column': !isMobileDevice() }">
                        <span v-show="filter.byRegion" style="color: blue; cursor: pointer" @click="SortRegion(item)">{{
                           item.region
                        }}</span>

                        <span v-show="filter.byDistrict" style="color: blue; cursor: pointer">{{ item.district }}</span>
                     </b-td>
                     <b-td class="text-right">{{ currency(item.approvedApplication.totalCount) }}</b-td>
                     <b-td class="text-right">{{ currency(item.approvedApplication.microOrgCount) }}</b-td>
                     <b-td class="text-right">{{ currency(item.approvedApplication.smallOrgCount) }}</b-td>
                     <b-td class="text-right">{{ currency(item.approvedApplication.middleOrgCount) }}</b-td>
                     <b-td class="text-right">{{ currency(item.approvedApplication.bigOrgCount) }}</b-td>

                     <b-td class="text-right">{{ currency(item.certificateGivenApplication.totalCount) }}</b-td>
                     <b-td class="text-right">{{ currency(item.certificateGivenApplication.microOrgCount) }}</b-td>
                     <b-td class="text-right">{{ currency(item.certificateGivenApplication.smallOrgCount) }}</b-td>
                     <b-td class="text-right">{{ currency(item.certificateGivenApplication.middleOrgCount) }}</b-td>
                     <b-td class="text-right">{{ currency(item.certificateGivenApplication.bigOrgCount) }}</b-td>

                     <b-td class="text-right">{{ currency(item.signedApplication.totalCount) }}</b-td>
                     <b-td class="text-right">{{ currency(item.signedApplication.microOrgCount) }}</b-td>
                     <b-td class="text-right">{{ currency(item.signedApplication.smallOrgCount) }}</b-td>
                     <b-td class="text-right">{{ currency(item.signedApplication.middleOrgCount) }}</b-td>
                     <b-td class="text-right">{{ currency(item.signedApplication.bigOrgCount) }}</b-td>
                  </b-tr>
               </b-tbody>
               <b-tfoot>
                  <b-tr variant="secondary">
                     <b-td></b-td>
                     <b-td class="text-center" :class="{ 'b-table-sticky-column': !isMobileDevice() }">{{
                        $t('Total')
                     }}</b-td>

                     <b-td class="text-right">{{ currency(totals.approvedApplication.totalCount) }}</b-td>
                     <b-td class="text-right">{{ currency(totals.approvedApplication.microOrgCount) }}</b-td>
                     <b-td class="text-right">{{ currency(totals.approvedApplication.smallOrgCount) }}</b-td>
                     <b-td class="text-right">{{ currency(totals.approvedApplication.middleOrgCount) }}</b-td>
                     <b-td class="text-right">{{ currency(totals.approvedApplication.bigOrgCount) }}</b-td>

                     <b-td class="text-right">{{ currency(totals.certificateGivenApplication.totalCount) }}</b-td>
                     <b-td class="text-right">{{ currency(totals.certificateGivenApplication.microOrgCount) }}</b-td>
                     <b-td class="text-right">{{ currency(totals.certificateGivenApplication.smallOrgCount) }}</b-td>
                     <b-td class="text-right">{{ currency(totals.certificateGivenApplication.middleOrgCount) }}</b-td>
                     <b-td class="text-right">{{ currency(totals.certificateGivenApplication.bigOrgCount) }}</b-td>

                     <b-td class="text-right">{{ currency(totals.signedApplication.totalCount) }}</b-td>
                     <b-td class="text-right">{{ currency(totals.signedApplication.microOrgCount) }}</b-td>
                     <b-td class="text-right">{{ currency(totals.signedApplication.smallOrgCount) }}</b-td>
                     <b-td class="text-right">{{ currency(totals.signedApplication.middleOrgCount) }}</b-td>
                     <b-td class="text-right">{{ currency(totals.signedApplication.bigOrgCount) }}</b-td>
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
   BOverlay,
   BTabs,
   BTab
} from 'bootstrap-vue';

import ReportService from '@/services/report/report.service';
import RegionService from '@/services/info/region.service';
import DistrictService from '@/services/info/district.service';

const totalsDef = {
   approvedApplication: {
      totalCount: 0,
      microOrgCount: 0,
      smallOrgCount: 0,
      middleOrgCount: 0,
      bigOrgCount: 0
   },
   certificateGivenApplication: {
      totalCount: 0,
      microOrgCount: 0,
      smallOrgCount: 0,
      middleOrgCount: 0,
      bigOrgCount: 0
   },
   signedApplication: {
      totalCount: 0,
      microOrgCount: 0,
      smallOrgCount: 0,
      middleOrgCount: 0,
      bigOrgCount: 0
   }
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
   directives: {
      'b-tooltip': VBTooltip,
      'b-modal': VBModal
   },
   data() {
      return {
         items: [],
         totals: { ...totalsDef },
         RegionList: [],
         DistrictList: [],
         filter: {
            regionId: null,
            region: '',
            byRegion: true,
            districtId: null,
            district: '',
            byDistrict: false,
            isOld: null,
            contractorCategoryId: null
         },
         isBusy: false,
         PrintLoading: false,
         localStorageData: {}
      };
   },
   created() {
      this.GetlocalStorageData();
      RegionService.GetAsSelectList(211)
         .then((res) => {
            this.RegionList = res.data;
         })
         .catch((error) => {
            this.showApiError(error);
         });
      this.Refresh();
   },
   methods: {
      GetlocalStorageData() {
         this.localStorageData = JSON.parse(localStorage.getItem('user_info'));
         if (this.localStorageData.organizationId != 1) {
            this.filter.regionId = this.localStorageData.organizationRegionId;
            this.GetDistrict(this.localStorageData.organizationRegionId);
            this.filter.byDistrict = true;
            this.filter.byRegion = false;
         }
      },
      SortRegion(item) {
         this.filter.byDistrict = true;
         this.filter.byRegion = false;
         this.filter.regionId = item.regionId;
         this.filter.region = item.region;
         this.GetDistrict(item.regionId);
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
      Refresh() {
         this.isBusy = true;
         this.totals = JSON.parse(JSON.stringify(totalsDef));
         ReportService.GetMemshipApplicationReportByOrganizations(this.filter)
            .then((res) => {
               this.items = res.data;
               res.data.forEach((item) => {
                  this.totals.approvedApplication.totalCount += item.approvedApplication.totalCount;
                  this.totals.approvedApplication.microOrgCount += item.approvedApplication.microOrgCount;
                  this.totals.approvedApplication.smallOrgCount += item.approvedApplication.smallOrgCount;
                  this.totals.approvedApplication.middleOrgCount += item.approvedApplication.middleOrgCount;
                  this.totals.approvedApplication.bigOrgCount += item.approvedApplication.bigOrgCount;

                  this.totals.certificateGivenApplication.totalCount += item.certificateGivenApplication.totalCount;
                  this.totals.certificateGivenApplication.microOrgCount +=
                     item.certificateGivenApplication.microOrgCount;
                  this.totals.certificateGivenApplication.smallOrgCount +=
                     item.certificateGivenApplication.smallOrgCount;
                  this.totals.certificateGivenApplication.middleOrgCount +=
                     item.certificateGivenApplication.middleOrgCount;
                  this.totals.certificateGivenApplication.bigOrgCount += item.certificateGivenApplication.bigOrgCount;

                  this.totals.signedApplication.totalCount += item.signedApplication.totalCount;
                  this.totals.signedApplication.microOrgCount += item.signedApplication.microOrgCount;
                  this.totals.signedApplication.smallOrgCount += item.signedApplication.smallOrgCount;
                  this.totals.signedApplication.middleOrgCount += item.signedApplication.middleOrgCount;
                  this.totals.signedApplication.bigOrgCount += item.signedApplication.bigOrgCount;
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
         ReportService.SaveAsExcelMemshipReportByOrganization(this.filter)
            .then((res) => {
               this.forceFileDownload(res, this.$t('GetMemshipApplicationReportByOrganizations'));
            })
            .catch((error) => {
               this.PrintLoading = false;
               this.showApiError(error);
            })
            .finally(() => {
               this.PrintLoading = false;
            });
      }
   }
};
</script>

<style lang="scss" scoped>
@import '../styles.scss';
</style>
