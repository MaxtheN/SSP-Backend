<template>
   <b-card no-body>
      <div class="m-2 report-table">
         <!-- Filters -->
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
            <b-col sm="12" md="2" class="mb-1">
               <form-picker :label="$t('startDate')" v-model="filter.fromDay" />
            </b-col>
            <b-col sm="12" md="2" class="mb-1">
               <form-picker :label="$t('endDate')" v-model="filter.toDay" />
            </b-col>
            <b-col cols="12" md="2" class="mt-2">
               <b-input-group class="text-right">
                  <b-form-input v-model="filter.search" :placeholder="$t('search')" />
                  <b-input-group-append>
                     <b-button @click="Refresh" variant="primary">
                        <feather-icon icon="SearchIcon" />
                     </b-button>
                  </b-input-group-append>
               </b-input-group>
            </b-col>
            <b-col sm="12" md="2" class="mt-2">
               <b-button @click="Print" :disabled="PrintLoading" variant="primary" class="ml-1">
                  <feather-icon icon="PrinterIcon"></feather-icon>
               </b-button>
            </b-col>
         </b-row>
         <!-- Filters -->
         <!-- Tabs -->
         <b-row>
            <b-col sm="12" md="8">
               <b-button-group @click="Refresh" size="sm" class="mr-2">
                  <b-button @click="filter.byWeek = true" :variant="filter.byWeek ? 'primary' : 'outline-primary'">{{
                     $t('weekDays')
                  }}</b-button>
                  <b-button @click="filter.byWeek = false" :variant="!filter.byWeek ? 'primary' : 'outline-primary'">{{
                     $t('byEmployee')
                  }}</b-button>
               </b-button-group>
            </b-col>
         </b-row>
         <!-- Tabs -->
         <b-overlay class="mt-2" :show="isBusy">
            <!-- Table -->
            <b-table-simple hover small caption-top responsive border>
               <b-thead>
                  <b-tr>
                     <!-- Order -->
                     <b-th class="b-table-sticky-column" rowspan="2">
                        {{ $t('order') }}
                     </b-th>
                     <!-- Region -->
                     <b-th rowspan="2" class="table-b-table-default b-table-sticky-column">
                        <span style="font-weight: 900; font-size: 14px; color: black">{{
                           filter.byWeek ? $t('weekDay') : $t('byEmployee')
                        }}</span>
                     </b-th>
                     <!-- 1 -->
                     <b-th rowspan="1" colspan="2">
                        {{ $t('appealsCounts') }}
                     </b-th>
                     <!-- 2 -->
                     <b-th rowspan="1" colspan="2">
                        {{ $t('Ish vaqtida kelib tushgan murojaatlar') }}
                     </b-th>
                     <!-- 3 -->
                     <b-th rowspan="1" colspan="2">
                        {{ $t('Ish vaqtidan tashqari kelib tushgan murojaatlar') }}
                     </b-th>
                  </b-tr>
                  <b-tr>
                     <template v-for="item in 3">
                        <b-th :key="item + 'hjkh'">
                           {{ $t('count') }}
                        </b-th>
                        <b-th :key="item + 'hjdkh'">
                           {{ $t('percents') }}
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
                        <span v-if="filter.byWeek" style="color: blue; cursor: pointer" @click="SortRegion(item)">
                           {{ $t(item.weekDayName) }}
                        </span>
                        <span v-else style="color: blue; cursor: pointer" @click="SortDistrict(item)">
                           {{ item.userFullName }}
                        </span>
                     </b-td>
                     <b-td class="text-right"> {{ item.total }} </b-td>
                     <b-td class="text-right"> {{ item.totalPercentage }} %</b-td>
                     <b-td class="text-right"> {{ item.inWorkTime }} </b-td>
                     <b-td class="text-right"> {{ item.inWorkTimePercentage }} %</b-td>
                     <b-td class="text-right"> {{ item.outWorkTime }} </b-td>
                     <b-td class="text-right"> {{ item.outWorkTimePercentage }} %</b-td>
                  </b-tr>
               </b-tbody>
               <b-tfoot v-if="items.length > 0">
                  <b-tr variant="secondary">
                     <b-td class="text-right b-table-sticky-column row-span-1 col-span-8" colspan="2">{{
                        $t('Total')
                     }}</b-td>
                     <b-td class="text-right"> {{ totals.total }} </b-td>
                     <b-td class="text-right"> {{ Number(totals.totalPercentage).toFixed() }} % </b-td>
                     <b-td class="text-right"> {{ totals.inWorkTime }} </b-td>
                     <b-td class="text-right">
                        {{ Number((totals.inWorkTime * 100) / totals.total).toFixed(0) }} %</b-td
                     >
                     <b-td class="text-right"> {{ totals.outWorkTime }} </b-td>
                     <!-- ... -->
                     <b-td class="text-right">
                        {{ Number((totals.outWorkTime * 100) / totals.total).toFixed(0) }} %</b-td
                     >
                  </b-tr>
               </b-tfoot>
            </b-table-simple>
            <!-- Table -->
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
         DistrictList: [],
         RegionList: [],
         totals: {},
         PrintLoading: false,
         filter: {
            byWeek: true,
            regionId: null,
            region: '',
            districtId: null,
            total: 0,
            toDay: null,
            fromDay: null,
            regionId: null,
            region: '',
            byRegion: true,
            districtId: null,
            district: '',
            search: ''
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
         this.items = [];
         ReportService.GetCallCenterReportByWeek(this.filter)
            .then((res) => {
               this.isBusy = false;
               const n = res.data;

               this.totals = {
                  total: 0,
                  totalPercentage: 0,
                  inWorkTime: 0,
                  inWorkTimePercentage: 0,
                  outWorkTime: 0,
                  outWorkTimePercentage: 0
               };

               n.forEach((item) => {
                  if (item.weekDay != -1) {
                     this.totals.total += item.total;
                     this.totals.totalPercentage += item.totalPercentage;
                     this.totals.inWorkTime += item.inWorkTime;
                     this.totals.inWorkTimePercentage += item.inWorkTimePercentage;
                     this.totals.outWorkTime += item.outWorkTime;
                     this.totals.outWorkTimePercentage += item.outWorkTimePercentage;
                     this.items.push(item);
                  }
               });
            })
            .catch((error) => {
               this.isBusy = false;
               this.showApiError(error);
            });
      },
      ChangeDistrict() {
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
            this.Refresh();
         }
      },
      SortRegion(item) {
         this.filter.regionId = item.regionId;
         this.filter.region = item.regionName;
         this.filter.byDistrict = true;
         this.filter.byRegion = false;
         this.Refresh();
      },
      SortDistrict(item) {
         this.filter.byDistrict = false;
         this.filter.byRegion = false;
         this.filter.districtId = item.districtId;
         this.filter.district = item.district;
         this.Refresh();
      },
      Print() {
         this.PrintLoading = true;
         ReportService.SaveAsExcelGetCallCenterReportByWeek(this.filter).then((res) => {
            this.forceFileDownload(res, this.$t('CallCenterReportByWeek'));
            this.PrintLoading = false;
         });
      }
   }
};
</script>
<style lang="scss" scoped>
@import '/src/@core/scss/tablestyle.scss';
</style>
