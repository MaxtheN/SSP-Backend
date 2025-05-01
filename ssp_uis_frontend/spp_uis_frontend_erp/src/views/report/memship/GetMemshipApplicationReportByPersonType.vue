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
                  :disabled="localStorageData.organizationId != 1"
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
         <b-col></b-col>
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
         <b-col col="12" class="text-right mt-1">
            <b-button @click="Print" :disabled="PrintLoading" variant="primary">
               <feather-icon icon="PrinterIcon"></feather-icon>
               {{ $t('Print') }}
            </b-button>
         </b-col>
      </b-row>
      <b-row align-h="between">
         <b-col sm="12" md="8">
            <b-breadcrumb class="">
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
      <b-row class="mt-1">
         <b-col>
            <b-tabs>
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
         </b-col>
      </b-row>
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
                     <b-th style="font-weight: 900; font-size: 14px; color: black" colspan="2">{{
                        $t('Қабул қилинган жами аризалар сони')
                     }}</b-th>
                     <b-th style="font-weight: 900; font-size: 14px; color: black" colspan="2">{{
                        $t('Имзоланган шартномалар')
                     }}</b-th>
                     <b-th style="font-weight: 900; font-size: 14px; color: black" colspan="2">{{
                        $t('Гувоҳнома берилганлар сони')
                     }}</b-th>
                  </b-tr>

                  <b-tr>
                     <b-th style="font-weight: 900; font-size: 14px; color: black">{{ $t('Юридик шахс') }}</b-th>
                     <b-th style="font-weight: 900; font-size: 14px; color: black">{{ $t('Жисмоний шахс') }}</b-th>
                     <b-th style="font-weight: 900; font-size: 14px; color: black">{{ $t('Юридик шахс') }}</b-th>
                     <b-th style="font-weight: 900; font-size: 14px; color: black">{{ $t('Жисмоний шахс') }}</b-th>
                     <b-th style="font-weight: 900; font-size: 14px; color: black">{{ $t('Юридик шахс') }}</b-th>
                     <b-th style="font-weight: 900; font-size: 14px; color: black">{{ $t('Жисмоний шахс') }}</b-th>
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
                     <b-td class="text-right">{{ currency(item.totalAcceptedApplication.legalPersonCount) }}</b-td>
                     <b-td class="text-right">{{ currency(item.totalAcceptedApplication.physicalPersonCount) }}</b-td>

                     <b-td class="text-right">{{ currency(item.totalSignedApplication.legalPersonCount) }}</b-td>
                     <b-td class="text-right">{{ currency(item.totalSignedApplication.physicalPersonCount) }}</b-td>

                     <b-td class="text-right">{{ currency(item.totalGivenCertificateCount.legalPersonCount) }}</b-td>
                     <b-td class="text-right">{{ currency(item.totalGivenCertificateCount.physicalPersonCount) }}</b-td>
                  </b-tr>
               </b-tbody>
               <b-tfoot>
                  <b-tr variant="secondary">
                     <b-td></b-td>
                     <b-td class="text-center" :class="{ 'b-table-sticky-column': !isMobileDevice() }">{{
                        $t('Total')
                     }}</b-td>
                     <b-td class="text-right">{{ currency(totals.totalAcceptedApplication.legalPersonCount) }}</b-td>
                     <b-td class="text-right">{{ currency(totals.totalAcceptedApplication.physicalPersonCount) }}</b-td>

                     <b-td class="text-right">{{ currency(totals.totalSignedApplication.legalPersonCount) }}</b-td>
                     <b-td class="text-right">{{ currency(totals.totalSignedApplication.physicalPersonCount) }}</b-td>

                     <b-td class="text-right">{{ currency(totals.totalGivenCertificateCount.legalPersonCount) }}</b-td>
                     <b-td class="text-right">{{
                        currency(totals.totalGivenCertificateCount.physicalPersonCount)
                     }}</b-td>
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
   totalAcceptedApplication: {
      legalPersonCount: 0,
      physicalPersonCount: 0
   },
   totalGivenCertificateCount: {
      legalPersonCount: 0,
      physicalPersonCount: 0
   },
   totalSignedApplication: {
      legalPersonCount: 0,
      physicalPersonCount: 0
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
         ReportService.GetMemshipApplicationReportByPersonType(this.filter)
            .then((res) => {
               this.items = res.data;
               res.data.forEach((item) => {
                  this.totals.totalAcceptedApplication.legalPersonCount +=
                     item.totalAcceptedApplication.legalPersonCount;
                  this.totals.totalAcceptedApplication.physicalPersonCount +=
                     item.totalAcceptedApplication.physicalPersonCount;

                  this.totals.totalGivenCertificateCount.legalPersonCount +=
                     item.totalGivenCertificateCount.legalPersonCount;
                  this.totals.totalGivenCertificateCount.physicalPersonCount +=
                     item.totalGivenCertificateCount.physicalPersonCount;

                  this.totals.totalSignedApplication.legalPersonCount += item.totalSignedApplication.legalPersonCount;
                  this.totals.totalSignedApplication.physicalPersonCount +=
                     item.totalSignedApplication.physicalPersonCount;
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
         ReportService.SaveAsExcelMemshipReportByPersonType(this.filter)
            .then((res) => {
               this.forceFileDownload(res, this.$t('GetMemshipApplicationReportByPersonType'));
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
