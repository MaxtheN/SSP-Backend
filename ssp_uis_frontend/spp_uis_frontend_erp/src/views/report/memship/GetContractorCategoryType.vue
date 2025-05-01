<template>
   <b-card>
      <b-row>
         <b-col sm="12" md="3">
            <div>
               <label for>{{ $t('region') }}</label>
               <v-select
                  :options="RegionList"
                  :reduce="(item) => item.value"
                  :disabled="localStorageData.organizationId != 1"
                  :placeholder="$t('ChooseBelow')"
                  label="text"
                  v-model="filter.regionId"
                  @input="ChangeRegion"
                  class="w-100"
               ></v-select>
            </div>
         </b-col>

         <b-col sm="12" md="2">
            <form-picker
               v-model="filter.startDate"
               :placeholder="$t('startdate')"
               :label="$t('startdate')"
               value-type="format"
               format="DD.MM.YYYY"
               @change="Refresh"
            ></form-picker>
         </b-col>
         <b-col sm="10" md="2">
            <form-picker
               v-model="filter.endDate"
               :placeholder="$t('enddate')"
               :label="$t('enddate')"
               value-type="format"
               format="DD.MM.YYYY"
               @change="Refresh"
            ></form-picker>
         </b-col>
         <b-col sm="2" class="text-right">
            <!-- <label for>{{ $t('search') }}</label> -->
            <b-button class="mt-2" @click="Refresh" variant="primary">
               <feather-icon icon="SearchIcon" />
            </b-button>
            <!-- <b-input-group class="text-right">
                <b-form-input v-model="filter.search" :placeholder="$t('search')" />
                <b-input-group-append>
                   <b-button @click="Refresh" variant="primary">
                      <feather-icon icon="SearchIcon" />
                   </b-button>
                </b-input-group-append>
             </b-input-group> -->
         </b-col>

         <b-col class="col-auto mt-2 text-right">
            <b-button @click="Print" :disabled="PrintLoading" variant="primary">
               <b-spinner small v-if="PrintLoading"></b-spinner>
               <feather-icon v-if="!PrintLoading" icon="PrinterIcon"></feather-icon>
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
               <b-breadcrumb-item v-show="filter.region" :active="filter.byDistrict">
                  <!-- @click="
                      () => {
                         filter.byDistrict = true;
                         filter.byRegion = false;
                         filter.district = '';
                         filter.districtId = null;
                         Refresh();
                      }
                   " -->
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
                        $t('Hisobot oyida yangi tashkil etilgan')
                     }}</b-th>
                     <b-th style="font-weight: 900; font-size: 14px; color: black" colspan="2">{{
                        $t('Bepul a`zolikka qabul qilinganlar soni')
                     }}</b-th>
                     <b-th style="font-weight: 900; font-size: 14px; color: black" rowspan="2">{{ $t('Jami') }}</b-th>
                     <b-th style="font-weight: 900; font-size: 14px; color: black" colspan="2">{{
                        $t('Bepul a`zolarning tadbirkorlik')
                     }}</b-th>
                     <b-th style="font-weight: 900; font-size: 14px; color: black" colspan="2">{{
                        $t('Baholash reytingi (norma bo`yicha ball)')
                     }}</b-th>
                     <b-th style="font-weight: 900; font-size: 14px; color: black" rowspan="2">{{
                        $t('O`rtacha reyting (ball)')
                     }}</b-th>
                     <b-th style="font-weight: 900; font-size: 14px; color: black" rowspan="2">{{
                        $t('Baholash')
                     }}</b-th>
                  </b-tr>
                  <b-tr>
                     <b-th style="font-weight: 900; font-size: 14px; color: black">{{ $t('Yuridik') }}</b-th>
                     <b-th style="font-weight: 900; font-size: 14px; color: black">{{ $t('YaTT') }}</b-th>
                     <b-th style="font-weight: 900; font-size: 14px; color: black">{{ $t('Yuridik') }}</b-th>
                     <b-th style="font-weight: 900; font-size: 14px; color: black">{{ $t('YaTT') }}</b-th>

                     <b-th style="font-weight: 900; font-size: 14px; color: black">{{ $t('Yuridik') }}</b-th>
                     <b-th style="font-weight: 900; font-size: 14px; color: black">{{ $t('YaTT') }}</b-th>

                     <b-th style="font-weight: 900; font-size: 14px; color: black">{{ $t('Yuridik') }}</b-th>
                     <b-th style="font-weight: 900; font-size: 14px; color: black">{{ $t('YaTT') }}</b-th>
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
                     <b-td class="text-right">{{ currency(item.totalNewCreatedContractorLegalCount) }}</b-td>
                     <b-td class="text-right">{{ currency(item.totalNewCreatedContractorPhysicalCount) }}</b-td>
                     <b-td class="text-right">{{ currency(item.totalFreeAddedMemshipLegalCount) }}</b-td>
                     <b-td class="text-right">{{ currency(item.totalFreeAddedMemshipPhysicalCount) }}</b-td>
                     <b-td class="text-right">{{ currency(item.total) }}</b-td>

                     <b-td class="text-right">{{ currency(item.totalEvaluationRatingLegalCount) }} %</b-td>
                     <b-td class="text-right">{{ currency(item.totalEvaluationRatingPhysicalCount) }} %</b-td>
                     <b-td class="text-right">{{ currency(item.totalRatingFromNormaLegalCount) }} </b-td>
                     <b-td class="text-right">{{ currency(item.totalRatingFromNormaPhysicalCount) }} </b-td>

                     <b-td class="text-right">{{ currency(item.averageRating) }}</b-td>
                     <b-td
                        :class="`text-right 
                      ${
                         getTextRating(item.averageRating)?.value == 1
                            ? 'text-danger'
                            : getTextRating(item.averageRating)?.value == 2
                            ? 'text-warning'
                            : getTextRating(item.averageRating)?.value == 3
                            ? 'text-success'
                            : ''
                      }`"
                        >{{ getTextRating(item.averageRating)?.text }}</b-td
                     >
                  </b-tr>
               </b-tbody>
               <b-tfoot>
                  <b-tr variant="secondary">
                     <b-td></b-td>
                     <b-td class="text-center" :class="{ 'b-table-sticky-column': !isMobileDevice() }">{{
                        $t('Total')
                     }}</b-td>
                     <b-td class="text-right">{{ currency(totals.totalNewCreatedContractorLegalCount) }}</b-td>
                     <b-td class="text-right">{{ currency(totals.totalNewCreatedContractorPhysicalCount) }}</b-td>
                     <b-td class="text-right">{{ currency(totals.totalFreeAddedMemshipLegalCount) }}</b-td>
                     <b-td class="text-right">{{ currency(totals.totalFreeAddedMemshipPhysicalCount) }}</b-td>
                     <b-td class="text-right">{{ currency(totals.total) }}</b-td>

                     <b-td class="text-right">{{ currency(totals.totalEvaluationRatingLegalCount) }} %</b-td>
                     <b-td class="text-right">{{ currency(totals.totalEvaluationRatingPhysicalCount) }} %</b-td>
                     <b-td class="text-right">{{ currency(totals.totalRatingFromNormaLegalCount) }} </b-td>
                     <b-td class="text-right">{{ currency(totals.totalRatingFromNormaPhysicalCount) }} </b-td>
                     <b-td class="text-right">{{ currency(totals.averageRating) }}</b-td>
                     <b-td class="text-right"></b-td>
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
import ManualService from '@/services/others/manual.service';

const totalsDef = {
   totalNewCreatedContractorLegalCount: 0,
   totalNewCreatedContractorPhysicalCount: 0,
   totalFreeAddedMemshipLegalCount: 0,
   totalFreeAddedMemshipPhysicalCount: 0,
   total: 0,
   totalRatingFromNormaLegalCount: 0,
   totalRatingFromNormaPhysicalCount: 0,
   totalEvaluationRatingLegalCount: 0,
   totalEvaluationRatingPhysicalCount: 0,
   averageRating: 0,
   evaluation: 0
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
            startDate: '',
            endDate: ''
         },
         isBusy: false,
         PrintLoading: false,
         localStorageData: {},
         RatingList: []
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
      this.GetRatingSelectList();
   },
   methods: {
      GetRatingSelectList() {
         ManualService.RatingSelectList().then((res) => {
            this.RatingList = res.data;
         });
      },
      getTextRating(rate) {
         if (this.RatingList && this.RatingList.length > 0) {
            return this.RatingList.find((item) => item.minimumPercentage <= rate && item.maximumPercentage >= rate);
         } else {
            return '-';
         }
      },
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
         ReportService.GetContractorCategoryType(this.filter)
            .then((res) => {
               this.items = res.data;
               res.data.forEach((item) => {
                  this.totals.totalNewCreatedContractorLegalCount += item.totalNewCreatedContractorLegalCount;
                  this.totals.totalNewCreatedContractorPhysicalCount += item.totalNewCreatedContractorPhysicalCount;
                  this.totals.totalFreeAddedMemshipLegalCount += item.totalFreeAddedMemshipLegalCount;
                  this.totals.totalFreeAddedMemshipPhysicalCount += item.totalFreeAddedMemshipPhysicalCount;
                  this.totals.total += item.total;
                  this.totals.totalRatingFromNormaLegalCount += item.totalRatingFromNormaLegalCount;
                  this.totals.totalRatingFromNormaPhysicalCount += item.totalRatingFromNormaPhysicalCount;
                  this.totals.totalEvaluationRatingLegalCount += item.totalEvaluationRatingLegalCount;
                  this.totals.totalEvaluationRatingPhysicalCount += item.totalEvaluationRatingPhysicalCount;
                  this.totals.averageRating += item.averageRating;
                  this.totals.evaluation += item.evaluation;
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
         ReportService.SaveAsExcelContractorCategoryType(this.filter)
            .then((res) => {
               this.forceFileDownload(res, this.$t('GetContractorCategoryType'));
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
