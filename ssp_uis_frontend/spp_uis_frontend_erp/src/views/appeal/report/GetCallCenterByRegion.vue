<template>
   <b-card no-body>
      <div class="m-2 report-table">
         <b-overlay :show="isBusy">
            <b-row>
               <b-col>
                  <b-tabs v-model="tabIndex" small class="nav-tabs" nav-wrapper-class="" @input="ChangeTab">
                     <b-tab :title="$t('FirstTab')" lazy></b-tab>
                     <b-tab :title="$t('SecondTab')" lazy></b-tab>
                  </b-tabs>
               </b-col>
               <b-col class="text-right"> </b-col>
            </b-row>
            <b-row>
               <b-col sm="12" md="3">
                  <div>
                     <label for>{{ $t('region') }}</label>
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
               <!-- <b-col sm="12" md="2">
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
          </b-col> -->
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
               <b-col sm="12" md="2">
                  <form-picker
                     v-model="filter.endDate"
                     :placeholder="$t('enddate')"
                     :label="$t('enddate')"
                     value-type="format"
                     format="DD.MM.YYYY"
                     @change="Refresh"
                  ></form-picker>
               </b-col>
               <b-col>
                  <b-button class="mt-2" @click="Refresh" variant="primary">
                     <feather-icon icon="SearchIcon" />
                  </b-button>
               </b-col>

               <b-col class="col-auto mt-2 text-right">
                  <b-button @click="Print" :disabled="PrintLoading" variant="primary">
                     <b-spinner small v-if="PrintLoading"></b-spinner>
                     <feather-icon v-if="!PrintLoading" icon="PrinterIcon"></feather-icon>
                     {{ $t('Print') }}
                  </b-button>
               </b-col>
            </b-row>

            <b-breadcrumb class="mb-1 mt-0">
               <b-breadcrumb-item
                  @click="
                     () => {
                        filter.region = '';
                        filter.regionId = null;
                        filter.districtId = null;
                        filter.byRegion = true;
                        filter.district = null;
                        filter.byDistrict = false;
                        Refresh();
                     }
                  "
               >
                  <b>{{ $t('uzb') }}</b>
               </b-breadcrumb-item>
               <b-breadcrumb-item
                  @click="
                     () => {
                        filter.districtId = null;
                        filter.byRegion = false;
                        filter.byDistrict = true;
                        Refresh();
                     }
                  "
               >
                  <b>{{ filter.region }}</b>
               </b-breadcrumb-item>
            </b-breadcrumb>
            <b-table-simple hover small caption-top responsive border>
               <b-thead>
                  <b-tr>
                     <b-th class="b-table-sticky-column" rowspan="3">
                        {{ $t('order') }}
                     </b-th>
                     <b-th rowspan="3" class="table-b-table-default b-table-sticky-column">
                        <span
                           v-if="filter.byRegion && !filter.byDistrict"
                           style="font-weight: 900; font-size: 14px; color: black"
                           >{{ $t('region') }}</span
                        >
                        <span v-if="filter.byDistrict" style="font-weight: 900; font-size: 14px; color: black">
                           {{ $t('district') }}
                        </span>
                     </b-th>
                     <b-th rowspan="2" colspan="2">
                        {{ $t('Мурожаат сони') }}
                     </b-th>
                     <b-th colspan="8">
                        {{ tabIndex == 0 ? $t('FirstTab') : $t('Korxonalar toifasi') }}
                     </b-th>
                  </b-tr>
                  <b-tr>
                     <b-th colspan="2">
                        {{ tabIndex == 0 ? $t('item1') : $t('Mikro') }}
                     </b-th>
                     <b-th colspan="2">
                        {{ tabIndex == 0 ? $t('item2') : $t('Kichik') }}
                     </b-th>
                     <b-th colspan="2">
                        {{ tabIndex == 0 ? $t('Investor') : $t('O`rta') }}
                     </b-th>
                     <b-th colspan="2">
                        {{ tabIndex == 0 ? $t('Jismoniy shaxs bo`lgan tadbirkorlik subyekti') : $t('Yirik') }}
                     </b-th>
                  </b-tr>
                  <b-tr>
                     <template v-for="item in 5">
                        <b-th :key="item + 'hjkh'">
                           {{ $t('count') }}
                        </b-th>
                        <b-th :key="item + 'hjdkh'">
                           {{ $t('calcCoef') }}
                        </b-th>
                     </template>
                  </b-tr>
               </b-thead>
               <b-tbody>
                  <b-tr v-for="(item, idx) in items" :key="idx + 'abc'">
                     <b-td class="b-table-sticky-column">
                        {{ idx + 1 }}
                     </b-td>
                     <b-td class="table-b-table-default b-table-sticky-column">
                        <span v-if="filter.byRegion" style="color: blue; cursor: pointer" @click="SortRegion(item)">
                           {{ item.region }}
                        </span>
                        <span v-if="filter.byDistrict" @click="SortDistrict(item)">
                           {{ item.district }}
                        </span>
                     </b-td>
                     <b-td class="text-right"> {{ item.totalCallCenterAppeal }} </b-td>
                     <b-td class="text-right"> {{ item.totalCallCenterAppealPercent }} %</b-td>
                     <b-td class="text-right">
                        {{ tabIndex == 0 ? item.totalLegalCount : item.totalMikroContractorCount }}
                     </b-td>

                     <b-td class="text-right">
                        {{ tabIndex == 0 ? item.totalLegalCountPercent : item.totalMikroContractorCountPercent }} %
                     </b-td>
                     <b-td class="text-right">
                        {{ tabIndex == 0 ? item.totalPhysicalCount : item.totalLitteContractorCount }}
                     </b-td>
                     <b-td class="text-right">
                        {{ tabIndex == 0 ? item.totalPhysicalCountPercent : item.totalLittleContractorCountPercent }}
                        %</b-td
                     >

                     <b-td class="text-right">
                        {{ tabIndex == 0 ? item.totalInvestorCount : item.totalMiddleContractorCount }}
                     </b-td>
                     <b-td class="text-right">
                        {{ tabIndex == 0 ? item.totalInvestorPercent : item.totalMiddleContractorCountPercent }} %
                     </b-td>
                     <b-td class="text-right">
                        {{ tabIndex == 0 ? item.totalPhysicalContractorCount : item.totalHigheContractorCount }}
                     </b-td>

                     <b-td class="text-right">
                        {{
                           tabIndex == 0 ? item.totalPhysicalContractorPercent : item.totalHigheContractorCountPercent
                        }}
                        %</b-td
                     >
                  </b-tr>
               </b-tbody>
               <b-tfoot v-if="items.length > 0">
                  <b-tr variant="secondary">
                     <b-td class="text-right b-table-sticky-column" colspan="2">{{ $t('Total') }}</b-td>
                     <b-td class="text-right"> {{ totals.totalCallCenterAppeal }} </b-td>
                     <b-td class="text-right"> 100 % </b-td>
                     <b-td class="text-right">
                        {{ tabIndex == 0 ? totals.totalLegalCount : totals.totalMikroContractorCount }}
                     </b-td>

                     <b-td class="text-right">
                        {{
                           tabIndex == 0
                              ? totals.totalLegalCount == 0
                                 ? 0
                                 : (totals.totalLegalCount * 100) / totals.totalCallCenterAppeal
                              : totals.totalMikroContractorCount == 0
                              ? 0
                              : (totals.totalMikroContractorCount * 100) / totals.totalCallCenterAppeal
                        }}
                        %
                     </b-td>
                     <b-td class="text-right">
                        {{ tabIndex == 0 ? totals.totalPhysicalCount : totals.totalLitteContractorCount }}
                     </b-td>
                     <b-td class="text-right">
                        {{
                           tabIndex == 0
                              ? totals.totalPhysicalCount == 0
                                 ? 0
                                 : (totals.totalPhysicalCount * 100) / totals.totalCallCenterAppeal
                              : totals.totalLitteContractorCount == 0
                              ? 0
                              : (totals.totalLitteContractorCount * 100) / totals.totalCallCenterAppeal
                        }}
                        %
                     </b-td>

                     <b-td class="text-right">
                        {{ tabIndex == 0 ? totals.totalInvestorCount : totals.totalMiddleContractorCount }}
                     </b-td>
                     <b-td class="text-right">
                        {{
                           tabIndex == 0
                              ? totals.totalInvestorCount == 0
                                 ? 0
                                 : (totals.totalInvestorCount * 100) / totals.totalCallCenterAppeal
                              : totals.totalMiddleContractorCount == 0
                              ? 0
                              : (totals.totalMiddleContractorCount * 100) / totals.totalCallCenterAppeal
                        }}
                        %
                     </b-td>
                     <b-td class="text-right">
                        {{ tabIndex == 0 ? totals.totalPhysicalContractorCount : totals.totalHigheContractorCount }}
                     </b-td>

                     <b-td class="text-right">
                        {{
                           tabIndex == 0
                              ? totals.totalPhysicalContractorCount == 0
                                 ? 0
                                 : (totals.totalPhysicalContractorCount * 100) / totals.totalCallCenterAppeal
                              : totals.totalHigheContractorCount == 0
                              ? 0
                              : (totals.totalHigheContractorCount * 100) / totals.totalCallCenterAppeal
                        }}
                        %
                     </b-td>
                  </b-tr>
               </b-tfoot>
            </b-table-simple>
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
import RegionService from '@/services/info/region.service';
import ReportService from '@/services/report/report.service';
import DistrictService from '@/services/info/district.service';
export default {
   components: {
      BButton,
      BPagination,
      BTable,
      BTab,
      BTabs,
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
         items: [],
         localStorageData: [],
         RegionList: [],
         DistrictList: [],
         totals: {},
         filter: {
            byRegion: true,
            regionId: null,
            region: '',
            byDistrict: false,
            districtId: null,
            reportType: true,
            startDate: '',
            endDate: ''
         },
         tabIndex: 0,
         PrintLoading: false
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
      this.Refresh();
   },
   methods: {
      ChangeRegion(id) {
         if (id) {
            this.filter.districtId = null;
            this.filter.byDistrict = true;
            this.filter.byRegion = false;

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
            this.Refresh();
         }
      },
      Print() {
         this.PrintLoading = true;
         ReportService.SaveAsExcelGetCallCenterByRegion({ ...this.filter, reportType: this.tabIndex == 0 })
            .then((res) => {
               this.forceFileDownload(res, this.$t('GetCallCenterByRegion'));
            })
            .catch((error) => {
               this.PrintLoading = false;
               this.showApiError(error);
            })
            .finally(() => {
               this.PrintLoading = false;
            });
      },
      ChangeTab() {
         this.Refresh();
      },
      GetDistrict(id) {
         DistrictService.GetAsSelectList(id)
            .then((res) => {
               console.log(res);
               this.DistrictList = res.data;
            })
            .catch((error) => {
               // this.showApiError(error);
            });
      },
      Refresh() {
         this.isBusy = true;
         ReportService.GetCallCenterByRegion({ ...this.filter, reportType: this.tabIndex == 0 })
            .then((res) => {
               this.isBusy = false;
               this.items = res.data;

               this.totals = {
                  totalCallCenterAppeal: 0,
                  totalCallCenterAppealPercent: 0,
                  totalMikroContractorCount: 0,
                  totalLegalCount: 0,
                  totalLegalCountPercent: 0,
                  totalMikroContractorCountPercent: 0,
                  totalPhysicalCount: 0,
                  totalLitteContractorCount: 0,
                  totalPhysicalCountPercent: 0,
                  totalLittleContractorCountPercent: 0,
                  totalInvestorCount: 0,
                  totalMiddleContractorCount: 0,
                  totalInvestorPercent: 0,
                  totalMiddleContractorCountPercent: 0,
                  totalPhysicalContractorCount: 0,
                  totalHigheContractorCount: 0,
                  totalPhysicalContractorPercent: 0,
                  totalHigheContractorCountPercent: 0
               };

               this.items.forEach((item) => {
                  this.totals.totalCallCenterAppeal += item.totalCallCenterAppeal;
                  this.totals.totalCallCenterAppealPercent += item.totalCallCenterAppealPercent;
                  this.totals.totalMikroContractorCount += item.totalMikroContractorCount;
                  this.totals.totalLegalCount += item.totalLegalCount;
                  this.totals.totalLegalCountPercent += item.totalLegalCountPercent;
                  this.totals.totalMikroContractorCountPercent += item.totalMikroContractorCountPercent;
                  this.totals.totalPhysicalCount += item.totalPhysicalCount;
                  this.totals.totalLitteContractorCount += item.totalLitteContractorCount;
                  this.totals.totalPhysicalCountPercent += item.totalPhysicalCountPercent;
                  this.totals.totalLittleContractorCountPercent += item.totalLittleContractorCountPercent;
                  this.totals.totalInvestorCount += item.totalInvestorCount;
                  this.totals.totalMiddleContractorCount += item.totalMiddleContractorCount;
                  this.totals.totalInvestorPercent += item.totalInvestorPercent;
                  this.totals.totalMiddleContractorCountPercent += item.totalMiddleContractorCountPercent;
                  this.totals.totalPhysicalContractorCount += item.totalPhysicalContractorCount;
                  this.totals.totalHigheContractorCount += item.totalHigheContractorCount;
                  this.totals.totalPhysicalContractorPercent += item.totalPhysicalContractorPercent;
                  this.totals.totalHigheContractorCountPercent += item.totalHigheContractorCountPercent;
               });
            })
            .catch((error) => {
               this.isBusy = false;
               this.showApiError(error);
            });
      },
      SortRegion(item) {
         this.filter.regionId = item.regionId;
         this.filter.region = item.region;
         this.filter.byDistrict = true;
         this.filter.byRegion = false;
         this.Refresh();
      }
   }
};
</script>
<style lang="scss" scoped>
@import '/src/@core/scss/tablestyle.scss';
</style>
