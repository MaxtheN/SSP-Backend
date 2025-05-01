<template>
   <b-card no-body>
      <div class="m-2">
         <b-tabs pills>
            <b-tab
               @click="
                  () => {
                     (hidden = true),
                        (filter.contractorInn = ''),
                        (filter.regionId = null),
                        (filter.districtId = null),
                        (filter.region = ''),
                        (filter.district = ''),
                        (filter.byRegion = true),
                        (filter.byDistrict = false),
                        (filter.byContractor = false),
                        (filter.search = ''),
                        (filter.sortBy = ''),
                        (filter.orderType = ''),
                        (filter.year = null),
                        (filter.month = null),
                        (filter.page = 1),
                        (filter.pageSize = 20);
                     Refresh();
                  }
               "
               :title="$t('variant-1')"
            >
            </b-tab>
            <b-tab
               @click="
                  () => {
                     (filter.byRegion = false),
                        (filter.byDistrict = false),
                        (filter.byContractor = true),
                        (hidden = false),
                        Refresh();
                  }
               "
               :title="$t('variant-2')"
            >
            </b-tab>
         </b-tabs>
         <b-row class="">
            <b-col sm="12" md="2" v-if="hidden">
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
            <b-col sm="12" md="2" v-if="hidden">
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
            <b-col sm="12" md="2" class="">
               <form-picker type="year" format="YYYY" v-model="filter.year" @change="Refresh" :label="$t('docyear')" />
            </b-col>
            <b-col sm="12" md="2">
               <form-select :options="MonthList" @change="Refresh" v-model="filter.month" label="month" />
            </b-col>
            <b-col sm="10" md="2">
               <form-input-hrm
                  v-model="filter.contractorInn"
                  :label="$t('inn')"
                  clearable
                  v-mask="'#########'"
                  :placeholder="$t('inn')"
               />
            </b-col>
            <b-col md="2" class="text-right">
               <b-button @click="Refresh" class="mt-2" @keyup.enter="Refresh" variant="primary">
                  <feather-icon icon="RefreshCwIcon" />
               </b-button>
            </b-col>
            <b-col sm="12" md="6" class="col-auto mt-2">
               <b-button @click="Print" :disabled="PrintLoading" variant="primary">
                  <feather-icon icon="PrinterIcon"></feather-icon>
                  {{ $t('Print') }}
               </b-button>
            </b-col>
            <b-col sm="12" md="6" :class="{ 'my-1': isMobileDevice() }">
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
         </b-row>
         <b-row align-h="between">
            <b-col sm="12" md="8">
               <b-breadcrumb class="p-0">
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
                           hidden = true;
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
                           filter.byContractor = false;
                           hidden = true;
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
         <b-overlay :show="isBusy">
            <b-table-simple hover small class="report-table" caption-top responsive border>
               <b-thead>
                  <b-tr>
                     <b-th rowspan="3">№</b-th>
                     <b-th
                        rowspan="3"
                        class="table-b-table-default"
                        :class="{ 'b-table-sticky-column': !isMobileDevice() }"
                     >
                        <span v-show="filter.byRegion && hidden">
                           {{ $t('region') }}
                        </span>
                        <span v-show="filter.byDistrict && hidden">
                           {{ $t('district') }}
                        </span>
                        <span v-show="filter.byContractor">
                           {{ $t('contractorT') }}
                        </span>
                     </b-th>
                     <b-th rowspan="3">{{ $t('contractorCount') }}</b-th>
                     <b-th colspan="35" class="text-center">{{ $t('korxanaQQS') }}</b-th>
                  </b-tr>
                  <b-tr>
                     <b-th colspan="2">{{ $t('month1') }}</b-th>
                     <b-th colspan="2">{{ $t('month2') }}</b-th>
                     <b-th colspan="2">{{ $t('month3') }}</b-th>
                     <b-th colspan="2">{{ $t('chorak1') }}</b-th>
                     <b-th colspan="2">{{ $t('month4') }}</b-th>
                     <b-th colspan="2">{{ $t('month5') }}</b-th>
                     <b-th colspan="2">{{ $t('month6') }}</b-th>
                     <b-th colspan="2">{{ $t('chorak2') }}</b-th>
                     <b-th colspan="2">{{ $t('month7') }}</b-th>
                     <b-th colspan="2">{{ $t('month8') }}</b-th>
                     <b-th colspan="2">{{ $t('month9') }}</b-th>
                     <b-th colspan="2">{{ $t('chorak3') }}</b-th>
                     <b-th colspan="2">{{ $t('month10') }}</b-th>
                     <b-th colspan="2">{{ $t('month11') }}</b-th>
                     <b-th colspan="2">{{ $t('month12') }}</b-th>
                     <b-th colspan="2">{{ $t('chorak4') }}</b-th>
                     <b-th colspan="2">{{ $t('yillik') }}</b-th>
                  </b-tr>
                  <b-tr>
                     <template v-for="i in 17">
                        <b-th :key="i">{{ $t('aylanmasumasi') }}</b-th>
                        <b-th :key="i + 'a'">{{ $t('kkss') }}</b-th>
                     </template>
                  </b-tr>
               </b-thead>
               <b-tbody>
                  <b-tr v-for="(item, i) in items" :key="i">
                     <b-td>{{ i + 1 }}</b-td>
                     <b-td
                        class="table-b-table-default"
                        :class="{ 'b-table-sticky-column': !isMobileDevice() }"
                        style="z-index: 2"
                     >
                        <span
                           v-show="filter.byRegion && hidden"
                           @click="SortRegion(item)"
                           style="cursor: pointer; color: blue"
                           >{{ item.region }}
                        </span>
                        <span
                           v-show="filter.byDistrict && hidden"
                           @click="SortDistrict(item)"
                           style="cursor: pointer; color: blue"
                           >{{ item.district }}
                        </span>
                        <div v-show="filter.byContractor">
                           <span style="cursor: pointer; color: blue"> {{ item.contractorInn }}</span> -
                           {{ item.contractorFullName }}
                        </div>
                     </b-td>

                     <b-td class="text-right">
                        {{ currency(item.contractorCount) }}
                     </b-td>

                     <b-td style="text-align: right">
                        {{ currency(item.netIncomeWithOutVat1) }}
                     </b-td>
                     <b-td style="text-align: right">
                        {{ currency(item.vatSum1) }}
                     </b-td>
                     <b-td style="text-align: right">
                        {{ currency(item.netIncomeWithOutVat2) }}
                     </b-td>
                     <b-td style="text-align: right">
                        {{ currency(item.vatSum2) }}
                     </b-td>
                     <b-td style="text-align: right">
                        {{ currency(item.netIncomeWithOutVat3) }}
                     </b-td>
                     <b-td style="text-align: right">
                        {{ currency(item.vatSum3) }}
                     </b-td>
                     <b-td style="text-align: right">
                        {{ currency(item.pNetIncomeWithoutvat1) }}
                     </b-td>
                     <b-td style="text-align: right">
                        {{ currency(item.pVatSum1) }}
                     </b-td>
                     <b-td style="text-align: right">
                        {{ currency(item.netIncomeWithOutVat4) }}
                     </b-td>
                     <b-td style="text-align: right">
                        {{ currency(item.vatSum4) }}
                     </b-td>
                     <b-td style="text-align: right">
                        {{ currency(item.netIncomeWithOutVat5) }}
                     </b-td>
                     <b-td style="text-align: right">
                        {{ currency(item.vatSum5) }}
                     </b-td>
                     <b-td style="text-align: right">
                        {{ currency(item.netIncomeWithOutVat6) }}
                     </b-td>
                     <b-td style="text-align: right">
                        {{ currency(item.vatSum6) }}
                     </b-td>
                     <b-td style="text-align: right">
                        {{ currency(item.pNetIncomeWithoutvat2) }}
                     </b-td>
                     <b-td style="text-align: right">
                        {{ currency(item.pVatSum2) }}
                     </b-td>
                     <b-td style="text-align: right">
                        {{ currency(item.netIncomeWithOutVat7) }}
                     </b-td>
                     <b-td style="text-align: right">
                        {{ currency(item.vatSum7) }}
                     </b-td>
                     <b-td style="text-align: right">
                        {{ currency(item.netIncomeWithOutVat8) }}
                     </b-td>
                     <b-td style="text-align: right">
                        {{ currency(item.vatSum8) }}
                     </b-td>
                     <b-td style="text-align: right">
                        {{ currency(item.netIncomeWithOutVat9) }}
                     </b-td>
                     <b-td style="text-align: right">
                        {{ currency(item.vatSum9) }}
                     </b-td>
                     <b-td style="text-align: right">
                        {{ currency(item.pNetIncomeWithoutvat3) }}
                     </b-td>
                     <b-td style="text-align: right">
                        {{ currency(item.pVatSum3) }}
                     </b-td>
                     <b-td style="text-align: right">
                        {{ currency(item.netIncomeWithOutVat10) }}
                     </b-td>
                     <b-td style="text-align: right">
                        {{ currency(item.vatSum10) }}
                     </b-td>
                     <b-td style="text-align: right">
                        {{ currency(item.netIncomeWithOutVat11) }}
                     </b-td>
                     <b-td style="text-align: right">
                        {{ currency(item.vatSum11) }}
                     </b-td>
                     <b-td style="text-align: right">
                        {{ currency(item.netIncomeWithOutVat12) }}
                     </b-td>
                     <b-td style="text-align: right">
                        {{ currency(item.vatSum12) }}
                     </b-td>
                     <b-td style="text-align: right">
                        {{ currency(item.p4NetIncomeWithoutvat) }}
                     </b-td>
                     <b-td style="text-align: right">
                        {{ currency(item.p4VatSum) }}
                     </b-td>
                     <b-td style="text-align: right">
                        {{ currency(item.totalNetIncomeWithoutvat) }}
                     </b-td>
                     <b-td style="text-align: right">
                        {{ currency(item.totalVatSum) }}
                     </b-td>
                  </b-tr>
               </b-tbody>
               <b-tfoot>
                  <b-tr variant="secondary">
                     <b-td></b-td>
                     <b-td class="text-center" :class="{ 'b-table-sticky-column': !isMobileDevice() }">{{
                        $t('Total')
                     }}</b-td>
                     <b-td class="text-right">{{ currency(totals.contractorCount) }}</b-td>
                     <b-td class="text-right">{{ currency(totals.netIncomeWithOutVat1) }}</b-td>
                     <b-td class="text-right">{{ currency(totals.vatSum1) }}</b-td>
                     <b-td class="text-right">{{ currency(totals.netIncomeWithOutVat2) }}</b-td>
                     <b-td class="text-right">{{ currency(totals.vatSum2) }}</b-td>
                     <b-td class="text-right">{{ currency(totals.netIncomeWithOutVat3) }}</b-td>
                     <b-td class="text-right">{{ currency(totals.vatSum3) }}</b-td>
                     <b-td class="text-right">{{ currency(totals.pNetIncomeWithoutvat1) }}</b-td>
                     <b-td class="text-right">{{ currency(totals.pVatSum1) }}</b-td>
                     <b-td class="text-right">{{ currency(totals.netIncomeWithOutVat4) }}</b-td>
                     <b-td class="text-right">{{ currency(totals.vatSum4) }}</b-td>
                     <b-td class="text-right">{{ currency(totals.netIncomeWithOutVat5) }}</b-td>
                     <b-td class="text-right">{{ currency(totals.vatSum5) }}</b-td>
                     <b-td class="text-right">{{ currency(totals.netIncomeWithOutVat6) }}</b-td>
                     <b-td class="text-right">{{ currency(totals.vatSum6) }}</b-td>
                     <b-td class="text-right">{{ currency(totals.pNetIncomeWithoutvat2) }}</b-td>
                     <b-td class="text-right">{{ currency(totals.pVatSum2) }}</b-td>
                     <b-td class="text-right">{{ currency(totals.netIncomeWithOutVat7) }}</b-td>
                     <b-td class="text-right">{{ currency(totals.vatSum7) }}</b-td>
                     <b-td class="text-right">{{ currency(totals.netIncomeWithOutVat8) }}</b-td>
                     <b-td class="text-right">{{ currency(totals.vatSum8) }}</b-td>
                     <b-td class="text-right">{{ currency(totals.netIncomeWithOutVat9) }}</b-td>
                     <b-td class="text-right">{{ currency(totals.vatSum9) }}</b-td>
                     <b-td class="text-right">{{ currency(totals.pNetIncomeWithoutvat3) }}</b-td>
                     <b-td class="text-right">{{ currency(totals.pVatSum3) }}</b-td>
                     <b-td class="text-right">{{ currency(totals.netIncomeWithOutVat10) }}</b-td>
                     <b-td class="text-right">{{ currency(totals.vatSum10) }}</b-td>
                     <b-td class="text-right">{{ currency(totals.netIncomeWithOutVat11) }}</b-td>
                     <b-td class="text-right">{{ currency(totals.vatSum11) }}</b-td>
                     <b-td class="text-right">{{ currency(totals.netIncomeWithOutVat12) }}</b-td>
                     <b-td class="text-right">{{ currency(totals.vatSum12) }}</b-td>
                     <b-td class="text-right">{{ currency(totals.p4NetIncomeWithoutvat) }}</b-td>
                     <b-td class="text-right">{{ currency(totals.p4VatSum) }}</b-td>
                     <b-td class="text-right">{{ currency(totals.totalNetIncomeWithoutvat) }}</b-td>
                     <b-td class="text-right">{{ currency(totals.totalVatSum) }}</b-td>
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
         <div class="mx-2 mb-2" v-if="!hidden">
            <b-row>
               <b-col
                  cols="12"
                  sm="6"
                  class="d-flex align-items-center justify-content-center justify-content-sm-start"
               >
                  <span class="text-muted">
                     {{ $t('Showing') }} {{ firstNumber }} {{ $t('to') }} {{ lastNumber }} {{ $t('of') }}
                     {{ filter.total }}
                     {{ $t('entries') }}
                  </span>
                  <v-select
                     v-model="filter.pageSize"
                     :options="pageOptions"
                     :clearable="false"
                     @input="Refresh"
                     class="per-page-selector d-inline-block ml-50 mr-1"
                  />
               </b-col>
               <!-- Pagination -->
               <b-col cols="12" sm="6" class="d-flex align-items-center justify-content-center justify-content-sm-end">
                  <b-pagination
                     v-model="filter.page"
                     :total-rows="filter.total"
                     :per-page="filter.pageSize"
                     first-number
                     last-number
                     @input="Refresh"
                     class="mb-0 mt-1 mt-sm-0"
                     prev-class="prev-item"
                     next-class="next-item"
                  >
                     <template #prev-text>
                        <feather-icon icon="ChevronLeftIcon" size="18" />
                     </template>
                     <template #next-text>
                        <feather-icon icon="ChevronRightIcon" size="18" />
                     </template>
                  </b-pagination>
               </b-col>
            </b-row>
         </div>
      </div>
   </b-card>
</template>

<script>
import ReportService from '@/services/report/report.service';
import ManualService from '@/services/others/manual.service';
import RegionService from '@/services/info/region.service';
import DistrictService from '@/services/info/district.service';
import {
   BButton,
   BTabs,
   BTab,
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
export default {
   components: {
      BTabs,
      BTab,
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
   data() {
      return {
         hidden: true,
         pageOptions: [10, 20, 50, 100],
         isBusy: false,
         items: [],
         MonthList: [],
         RegionList: [],
         DistrictList: [],
         fields: [],
         PrintLoading: false,
         filter: {
            contractorInn: '',
            regionId: null,
            districtId: null,
            region: '',
            district: '',
            byRegion: true,
            byDistrict: false,
            byContractor: false,
            search: '',
            sortBy: '',
            orderType: '',
            year: null,
            month: null,
            page: 1,
            pageSize: 20,
            total: 0
         },
         totals: {
            contractorCount: 0,
            netIncomeWithOutVat1: 0,
            vatSum1: 0,
            netIncomeWithOutVat2: 0,
            vatSum2: 0,
            netIncomeWithOutVat3: 0,
            vatSum3: 0,
            pNetIncomeWithoutvat1: 0,
            pVatSum1: 0,
            netIncomeWithOutVat4: 0,
            vatSum4: 0,
            netIncomeWithOutVat5: 0,
            vatSum5: 0,
            netIncomeWithOutVat6: 0,
            vatSum6: 0,
            pNetIncomeWithoutvat2: 0,
            pVatSum2: 0,
            netIncomeWithOutVat7: 0,
            vatSum7: 0,
            netIncomeWithOutVat8: 0,
            vatSum8: 0,
            netIncomeWithOutVat9: 0,
            vatSum9: 0,
            pNetIncomeWithoutvat3: 0,
            pVatSum3: 0,
            netIncomeWithOutVat10: 0,
            vatSum10: 0,
            netIncomeWithOutVat11: 0,
            vatSum11: 0,
            netIncomeWithOutVat12: 0,
            vatSum12: 0,
            p4NetIncomeWithoutvat: 0,
            p4VatSum: 0,
            totalNetIncomeWithoutvat: 0,
            totalVatSum: 0
         }
      };
   },
   computed: {
      firstNumber() {
         return (this.filter.page - 1) * this.filter.pageSize + 1;
      },
      lastNumber() {
         if (this.filter.total < this.filter.pageSize) {
            return this.filter.total;
         } else {
            if (this.filter.page * this.filter.pageSize > this.filter.total) {
               return this.filter.total;
            } else {
               return this.filter.page * this.filter.pageSize;
            }
         }
      }
   },
   created() {
      ManualService.GetMonthSelectList().then((res) => {
         this.MonthList = res.data;
      });
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
      Print() {
         this.PrintLoading = true;
         ReportService.SaveAsExcelTaxQqsAylanmaReport(this.filter)
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
         console.log(item);
         this.filter.byDistrict = true;
         this.filter.byRegion = false;
         this.filter.byContractor = false;
         this.filter.regionId = item.regionId;
         this.filter.region = item.region;
         this.GetDistrict(item.regionId);
         this.Refresh();
      },
      SortDistrict(item) {
         this.hidden = false;
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
            if (this.hidden) {
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
               this.filter.byRegion = true;
               this.filter.byContractor = true;
               this.filter.region = this.filter.regionId
                  ? this.RegionList.filter((item) => item.value === this.filter.regionId)[0].text
                  : '';
               this.Refresh();
               this.GetDistrict(id);
            }
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
            if (this.hidden) {
               this.filter.byDistrict = false;
               this.filter.byRegion = true;
               this.filter.byContractor = true;
               this.filter.district = this.filter.districtId
                  ? this.DistrictList.filter((item) => item.value === this.filter.districtId)[0].text
                  : '';
               this.Refresh();
            } else {
               this.filter.byRegion = false;
               this.filter.byDistrict = true;
               this.filter.district = this.filter.districtId
                  ? this.DistrictList.filter((item) => item.value === this.filter.districtId)[0].text
                  : '';
               this.Refresh();
            }
         } else {
            this.filter.byDistrict = true;
            this.filter.byRegion = true;
            this.filter.byContractor = true;
            this.filter.district = '';
            this.Refresh();
         }
      },
      Refresh() {
         this.isBusy = true;
         ReportService.GetTaxQqsAylanmaReport(this.filter)
            .then((res) => {
               this.items = res.data.rows;
               this.filter.total = res.data.total;

               this.totals = {
                  contractorCount: 0,
                  netIncomeWithOutVat1: 0,
                  vatSum1: 0,
                  netIncomeWithOutVat2: 0,
                  vatSum2: 0,
                  netIncomeWithOutVat3: 0,
                  vatSum3: 0,
                  pNetIncomeWithoutvat1: 0,
                  pVatSum1: 0,
                  netIncomeWithOutVat4: 0,
                  vatSum4: 0,
                  netIncomeWithOutVat5: 0,
                  vatSum5: 0,
                  netIncomeWithOutVat6: 0,
                  vatSum6: 0,
                  pNetIncomeWithoutvat2: 0,
                  pVatSum2: 0,
                  netIncomeWithOutVat7: 0,
                  vatSum7: 0,
                  netIncomeWithOutVat8: 0,
                  vatSum8: 0,
                  netIncomeWithOutVat9: 0,
                  vatSum9: 0,
                  pNetIncomeWithoutvat3: 0,
                  pVatSum3: 0,
                  netIncomeWithOutVat10: 0,
                  vatSum10: 0,
                  netIncomeWithOutVat11: 0,
                  vatSum11: 0,
                  netIncomeWithOutVat12: 0,
                  vatSum12: 0,
                  p4NetIncomeWithoutvat: 0,
                  p4VatSum: 0,
                  totalNetIncomeWithoutvat: 0,
                  totalVatSum: 0
               };

               this.items.forEach((item) => {
                  this.totals.contractorCount += item.contractorCount;
                  this.totals.netIncomeWithOutVat1 += item.netIncomeWithOutVat1;
                  this.totals.vatSum1 += item.vatSum1;
                  this.totals.netIncomeWithOutVat2 += item.netIncomeWithOutVat2;
                  this.totals.vatSum2 += item.vatSum2;
                  this.totals.netIncomeWithOutVat3 += item.netIncomeWithOutVat3;
                  this.totals.vatSum3 += item.vatSum3;
                  this.totals.pNetIncomeWithoutvat1 += item.pNetIncomeWithoutvat1;
                  this.totals.pVatSum1 += item.pVatSum1;
                  this.totals.netIncomeWithOutVat4 += item.netIncomeWithOutVat4;
                  this.totals.vatSum4 += item.vatSum4;
                  this.totals.netIncomeWithOutVat5 += item.netIncomeWithOutVat5;
                  this.totals.vatSum5 += item.vatSum5;
                  this.totals.netIncomeWithOutVat6 += item.netIncomeWithOutVat6;
                  this.totals.vatSum6 += item.vatSum6;
                  this.totals.pNetIncomeWithoutvat2 += item.pNetIncomeWithoutvat2;
                  this.totals.pVatSum2 += item.pVatSum2;
                  this.totals.netIncomeWithOutVat7 += item.netIncomeWithOutVat7;
                  this.totals.vatSum7 += item.vatSum7;
                  this.totals.netIncomeWithOutVat8 += item.netIncomeWithOutVat8;
                  this.totals.vatSum8 += item.vatSum8;
                  this.totals.netIncomeWithOutVat9 += item.netIncomeWithOutVat9;
                  this.totals.vatSum9 += item.vatSum9;
                  this.totals.pNetIncomeWithoutvat3 += item.pNetIncomeWithoutvat3;
                  this.totals.pVatSum3 += item.pVatSum3;
                  this.totals.netIncomeWithOutVat10 += item.netIncomeWithOutVat10;
                  this.totals.vatSum10 += item.vatSum10;
                  this.totals.netIncomeWithOutVat11 += item.netIncomeWithOutVat11;
                  this.totals.vatSum11 += item.vatSum11;
                  this.totals.netIncomeWithOutVat12 += item.netIncomeWithOutVat12;
                  this.totals.vatSum12 += item.vatSum12;
                  this.totals.p4NetIncomeWithoutvat += item.p4NetIncomeWithoutvat;
                  this.totals.p4VatSum += item.p4VatSum;
                  this.totals.totalNetIncomeWithoutvat += item.totalNetIncomeWithoutvat;
                  this.totals.totalVatSum += item.totalVatSum;
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

<style lang="scss" scoped>
@import '../styles.scss';
</style>
