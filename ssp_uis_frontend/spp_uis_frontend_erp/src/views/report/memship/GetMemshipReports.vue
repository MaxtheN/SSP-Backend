<template>
   <b-card>
      <b-row>
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
      <div class="report-table mt-1">
         <b-overlay :show="isBusy">
            <b-table-simple hover small caption-top responsive border>
               <b-thead>
                  <b-tr>
                     <b-th rowspan="3" style="font-weight: 900; font-size: 14px; color: black">{{ $t('order') }}</b-th>
                     <b-th
                        rowspan="3"
                        class="table-b-table-default"
                        :class="{ 'b-table-sticky-column': !isMobileDevice() }"
                     >
                        <span v-show="filter.byRegion" style="font-weight: 900; font-size: 14px; color: black">{{
                           $t('region')
                        }}</span>
                        <span v-show="filter.byDistrict" style="font-weight: 900; font-size: 14px; color: black">{{
                           $t('Region')
                        }}</span>
                     </b-th>
                     <b-th colspan="7" style="font-weight: 900; font-size: 14px; color: black">
                        {{ $t('ойи учун', { month: $t('month' + (new Date().getMonth() + 1)) }) }}</b-th
                     >
                     <b-th colspan="7" style="font-weight: 900; font-size: 14px; color: black">{{
                        $t('Йил бошидан')
                     }}</b-th>
                     <b-th rowspan="3" style="font-weight: 900; font-size: 14px; color: black">{{
                        $t('ЖАМИ ҚАРЗДОРЛИК')
                     }}</b-th>
                     <b-th rowspan="3" style="font-weight: 900; font-size: 14px; color: black">{{
                        $t('Коефицент хисобида')
                     }}</b-th>
                     <b-th rowspan="3" style="font-weight: 900; font-size: 14px; color: black">{{
                        $t('Коефицент хисобида')
                     }}</b-th>
                  </b-tr>
                  <b-tr>
                     <b-th rowspan="2" style="font-weight: 900; font-size: 14px; color: black">{{
                        $t('УМУМИЙ РЕЖА')
                     }}</b-th>
                     <b-th rowspan="2" style="font-weight: 900; font-size: 14px; color: black">{{ $t('ФАКТ') }}</b-th>
                     <b-th rowspan="2" style="font-weight: 900; font-size: 14px; color: black">{{ $t('Фоизда') }}</b-th>
                     <b-th colspan="2" style="font-weight: 900; font-size: 14px; color: black">{{
                        $t('Келиб тушган аризалар сони')
                     }}</b-th>
                     <b-th colspan="2" style="font-weight: 900; font-size: 14px; color: black">{{
                        $t('Гувоҳнома берилганлар сони')
                     }}</b-th>
                     <b-th rowspan="2" style="font-weight: 900; font-size: 14px; color: black">{{
                        $t('УМУМИЙ РЕЖА')
                     }}</b-th>
                     <b-th rowspan="2" style="font-weight: 900; font-size: 14px; color: black">{{ $t('ФАКТ') }}</b-th>
                     <b-th rowspan="2" style="font-weight: 900; font-size: 14px; color: black">{{
                        $t('Келиб тушган жами аризалар сони')
                     }}</b-th>
                     <b-th rowspan="2" style="font-weight: 900; font-size: 14px; color: black">{{
                        $t('Жами шартномага номер берилганлар сони')
                     }}</b-th>
                     <b-th colspan="3" style="font-weight: 900; font-size: 14px; color: black">{{
                        $t('shundan')
                     }}</b-th>
                  </b-tr>
                  <b-tr>
                     <b-th style="font-weight: 900; font-size: 14px; color: black">{{ $t('юридик') }}</b-th>
                     <b-th style="font-weight: 900; font-size: 14px; color: black">{{ $t('ЯТТ') }}</b-th>
                     <b-th style="font-weight: 900; font-size: 14px; color: black">{{ $t('юридик') }}</b-th>
                     <b-th style="font-weight: 900; font-size: 14px; color: black">{{ $t('ЯТТ') }}</b-th>
                     <b-th style="font-weight: 900; font-size: 14px; color: black">{{
                        $t('Гувоҳнома берилганлар сони')
                     }}</b-th>
                     <b-th style="font-weight: 900; font-size: 14px; color: black">{{
                        $t('Гувоҳнома учун юборилган жараёндаги аризалар сони')
                     }}</b-th>
                     <b-th style="font-weight: 900; font-size: 14px; color: black">{{
                        $t('Гувохнома учун базага киритилмаганлар сони')
                     }}</b-th>
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
                     <b-td class="text-right">{{ currency(item.memshipGeneralPlan) }}</b-td>
                     <b-td class="text-right">{{ currency(item.memshipFactByMonth) }}</b-td>
                     <b-td class="text-right">{{ currency(item.memshipFactByMonthPercentage) }}</b-td>
                     <b-td class="text-right">{{ currency(item.memshipApplicationLegal) }}</b-td>
                     <b-td class="text-right">{{ currency(item.memshipApplicationYtt) }}</b-td>
                     <b-td class="text-right">{{ currency(item.memshipCertificateLegal) }}</b-td>
                     <b-td class="text-right">{{ currency(item.memshipCertificateYtt) }}</b-td>
                     <b-td class="text-right">{{ currency(item.memshipGeneralPlanByYear) }}</b-td>
                     <b-td class="text-right">{{ currency(item.memshipFactByYear) }}</b-td>
                     <b-td class="text-right">{{ currency(item.memshipApplicationCountByYear) }}</b-td>
                     <b-td class="text-right">{{ currency(item.memshipContractCountByYear) }}</b-td>
                     <b-td class="text-right">{{ currency(item.memshipCertificateAcceptedCountByear) }}</b-td>
                     <b-td class="text-right">{{ currency(item.memshipCertificateProgressCountByear) }}</b-td>
                     <b-td class="text-right">{{ currency(item.memshipCertificateNotIncludedCountByear) }}</b-td>
                     <b-td class="text-right">{{ currency(item.memshipGeneralIndebtednessByYear) }}</b-td>
                     <b-td class="text-right">{{ currency(item.memshipGeneralIndebtednessCoefficientByYear) }}</b-td>
                     <b-td class="text-right">{{ currency(item.memshipGeneralIndebtednessPercentageByYear) }}</b-td>
                  </b-tr>
               </b-tbody>
               <b-tfoot>
                  <b-tr variant="secondary">
                     <b-td></b-td>
                     <b-td class="text-center" :class="{ 'b-table-sticky-column': !isMobileDevice() }">{{
                        $t('Total')
                     }}</b-td>
                     <b-td class="text-right">{{ currency(totals.memshipGeneralPlan) }}</b-td>
                     <b-td class="text-right">{{ currency(totals.memshipFactByMonth) }}</b-td>
                     <b-td class="text-right">{{ currency(totals.memshipFactByMonthPercentage) }}</b-td>
                     <b-td class="text-right">{{ currency(totals.memshipApplicationLegal) }}</b-td>
                     <b-td class="text-right">{{ currency(totals.memshipApplicationYtt) }}</b-td>
                     <b-td class="text-right">{{ currency(totals.memshipCertificateLegal) }}</b-td>
                     <b-td class="text-right">{{ currency(totals.memshipCertificateYtt) }}</b-td>
                     <b-td class="text-right">{{ currency(totals.memshipGeneralPlanByYear) }}</b-td>
                     <b-td class="text-right">{{ currency(totals.memshipFactByYear) }}</b-td>
                     <b-td class="text-right">{{ currency(totals.memshipApplicationCountByYear) }}</b-td>
                     <b-td class="text-right">{{ currency(totals.memshipContractCountByYear) }}</b-td>
                     <b-td class="text-right">{{ currency(totals.memshipCertificateAcceptedCountByear) }}</b-td>
                     <b-td class="text-right">{{ currency(totals.memshipCertificateProgressCountByear) }}</b-td>
                     <b-td class="text-right">{{ currency(totals.memshipCertificateNotIncludedCountByear) }}</b-td>
                     <b-td class="text-right">{{ currency(totals.memshipGeneralIndebtednessByYear) }}</b-td>
                     <b-td class="text-right">{{ currency(totals.memshipGeneralIndebtednessCoefficientByYear) }}</b-td>
                     <b-td class="text-right">{{ currency(totals.memshipGeneralIndebtednessPercentageByYear) }}</b-td>
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
   memshipGeneralPlan: 0,
   memshipFactByMonth: 0,
   memshipFactByMonthPercentage: 0,
   memshipApplicationLegal: 0,
   memshipApplicationYtt: 0,
   memshipCertificateLegal: 0,
   memshipCertificateYtt: 0,
   memshipGeneralPlanByYear: 0,
   memshipFactByYear: 0,
   memshipApplicationCountByYear: 0,
   memshipContractCountByYear: 0,
   memshipCertificateAcceptedCountByear: 0,
   memshipCertificateProgressCountByear: 0,
   memshipCertificateNotIncludedCountByear: 0,
   memshipGeneralIndebtednessByYear: 0,
   memshipGeneralIndebtednessCoefficientByYear: 0,
   memshipGeneralIndebtednessPercentageByYear: 0
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
         this.totals = { ...totalsDef };
         ReportService.GetMemshipReports(this.filter)
            .then((res) => {
               this.items = res.data;
               res.data.forEach((item) => {
                  this.totals.memshipGeneralPlan += item.memshipGeneralPlan;
                  this.totals.memshipFactByMonth += item.memshipFactByMonth;
                  this.totals.memshipFactByMonthPercentage += item.memshipFactByMonthPercentage;
                  this.totals.memshipApplicationLegal += item.memshipApplicationLegal;
                  this.totals.memshipApplicationYtt += item.memshipApplicationYtt;
                  this.totals.memshipCertificateLegal += item.memshipCertificateLegal;
                  this.totals.memshipCertificateYtt += item.memshipCertificateYtt;
                  this.totals.memshipGeneralPlanByYear += item.memshipGeneralPlanByYear;
                  this.totals.memshipFactByYear += item.memshipFactByYear;
                  this.totals.memshipApplicationCountByYear += item.memshipApplicationCountByYear;
                  this.totals.memshipContractCountByYear += item.memshipContractCountByYear;
                  this.totals.memshipCertificateAcceptedCountByear += item.memshipCertificateAcceptedCountByear;
                  this.totals.memshipCertificateProgressCountByear += item.memshipCertificateProgressCountByear;
                  this.totals.memshipCertificateNotIncludedCountByear += item.memshipCertificateNotIncludedCountByear;
                  this.totals.memshipGeneralIndebtednessByYear += item.memshipGeneralIndebtednessByYear;
                  this.totals.memshipGeneralIndebtednessCoefficientByYear +=
                     item.memshipGeneralIndebtednessCoefficientByYear;
                  this.totals.memshipGeneralIndebtednessPercentageByYear +=
                     item.memshipGeneralIndebtednessPercentageByYear;
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
         ReportService.SaveAsExcelGetMemshipReport(this.filter)
            .then((res) => {
               this.forceFileDownload(res, this.$t('GetMemshipReports'));
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
