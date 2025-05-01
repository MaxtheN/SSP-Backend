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
               <label for>{{ $t('Oblast') }}</label>
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
         <b-col sm="12" md="2">
            <form-select
               v-model="filter.contractorCategoryId"
               :options="ContractorCategoryList"
               label="contractorCategory"
               @input="Refresh"
            />
         </b-col>
         <b-col sm="12" md="2">
            <form-select
               v-model="filter.memshipContractTypeId"
               :options="MemshipContractTypeSelectList"
               label="memshipContractType"
               @input="Refresh"
            />
         </b-col>
      </b-row>
      <b-row>
         <b-col cols="12" class="order-2">
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
         <b-col></b-col>

         <b-col cols="12" md="4" class="order-1">
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

      <div class="report-table">
         <b-overlay :show="isBusy">
            <b-table-simple
               hover
               small
               caption-top
               responsive
               border
               class="table-scroll"
               style="max-height: 570px; overflow-y: auto"
            >
               <b-thead>
                  <b-tr>
                     <b-th rowspan="3" style="font-weight: 900; font-size: 14px; color: black">{{ $t('order') }}</b-th>
                     <b-th
                        rowspan="3"
                        class="table-b-table-default"
                        :class="{ 'b-table-sticky-column': !isMobileDevice() }"
                     >
                        <span style="font-weight: 900; font-size: 14px; color: black">{{ $t('contractor') }}</span>
                     </b-th>
                  </b-tr>
                  <b-tr>
                     <b-th rowspan="2"
                        ><span style="font-weight: 900; font-size: 14px; color: black">{{
                           $t('totalApplicationCount')
                        }}</span></b-th
                     >

                     <b-th colspan="3" style="font-weight: 900; font-size: 14px; color: black">
                        {{ $t('shundan') }}</b-th
                     >

                     <b-th rowspan="2"
                        ><span style="font-weight: 900; font-size: 14px; color: black">{{
                           $t('memshiptotalApplicationCount')
                        }}</span></b-th
                     >
                     <b-th colspan="3" style="font-weight: 900; font-size: 14px; color: black">{{
                        $t('shundan')
                     }}</b-th>

                     <b-th rowspan="2"
                        ><span style="font-weight: 900; font-size: 14px; color: black">{{
                           $t('ViewNumberofPresentedCERTIFICATES')
                        }}</span></b-th
                     >

                     <b-th rowspan="2"
                        ><span style="font-weight: 900; font-size: 14px; color: black">{{
                           $t('NumberofPresentedCERTIFICATES')
                        }}</span></b-th
                     >
                  </b-tr>
                  <b-tr>
                     <b-th style="font-weight: 900; font-size: 14px; color: black">{{ $t('TotalAplication') }}</b-th>
                     <b-th style="font-weight: 900; font-size: 14px; color: black">{{
                        $t('NumberOfApplicationsPendingReview')
                     }}</b-th>
                     <b-th style="font-weight: 900; font-size: 14px; color: red">{{ $t('custom3') }}</b-th>
                     <b-th style="font-weight: 900; font-size: 14px; color: black">{{
                        $t('ViewtotalPrtnApplicationSignedCount')
                     }}</b-th>
                     <b-th style="font-weight: 900; font-size: 14px; color: black">{{
                        $t('totalPrtnApplicationSignedCount')
                     }}</b-th>
                     <b-th style="font-weight: 900; font-size: 14px; color: red">{{
                        $t('TheNumberOfRejectedApplications')
                     }}</b-th>
                  </b-tr>
               </b-thead>
               <b-tbody v-if="items.length > 0">
                  <b-tr v-for="(item, idx) in items" :key="idx + 'abc'">
                     <b-td>{{ idx + 1 }}</b-td>

                     <b-td class="table-b-table-default" :class="{ 'b-table-sticky-column': !isMobileDevice() }">
                        {{ item.contractor }}
                     </b-td>

                     <b-td class="text-right">{{ currency(item.totalMemshipApplicationCount) }}</b-td>
                     <b-td class="text-right">{{ currency(item.memshipApplicationAcceptedCount) }}</b-td>
                     <b-td class="text-right">{{ currency(item.memshipApplicationReviewCount) }}</b-td>
                     <b-td class="text-right" style="color: red; font-weight: 900">{{
                        currency(item.memshipApplicationRejectedCount)
                     }}</b-td>
                     <b-td class="text-right">{{ currency(item.totalMemshipContractCount) }}</b-td>
                     <b-td class="text-right">{{ currency(item.memshipContractReviewCount) }}</b-td>
                     <b-td class="text-right">{{ currency(item.memshipContractAcceptedCount) }}</b-td>
                     <b-td class="text-right" style="color: red; font-weight: 900">{{
                        currency(item.memshipContractRejectedCount)
                     }}</b-td>
                     <b-td class="text-right">{{ currency(item.memshipCertificateReviewCount) }}</b-td>
                     <b-td class="text-right">{{ currency(item.memshipCertificateFormedCount) }}</b-td>
                  </b-tr>
               </b-tbody>
            </b-table-simple>
            <template #overlay>
               <div class="text-center text-primary my-2">
                  <b-spinner scale="2" class="align-middle mr-2"></b-spinner>
                  <strong>{{ $t('Loading') }}...</strong>
               </div>
            </template>
         </b-overlay>
         <div class="mx-2 mb-2">
            <b-row>
               <b-col
                  cols="12"
                  sm="6"
                  class="d-flex align-items-center justify-content-center justify-content-sm-start"
               >
                  <span class="text-muted">
                     {{ $t('Showing') }} {{ firstNumber }} {{ $t('to') }} {{ lastNumber }} {{ $t('of') }}
                     {{ filter.count }}
                     {{ $t('entries') }}
                  </span>
                  <v-select
                     v-model="filter.pageSize"
                     :dir="$store.state.appConfig.isRTL ? 'rtl' : 'ltr'"
                     :options="filter.perPageOptions"
                     :clearable="false"
                     @input="Refresh"
                     class="per-page-selector d-inline-block ml-50 mr-1"
                  />
               </b-col>
               <!-- Pagination -->
               <b-col cols="12" sm="6" class="d-flex align-items-center justify-content-center justify-content-sm-end">
                  <b-pagination
                     v-model="filter.page"
                     :total-rows="filter.count"
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
         items: [],
         RegionList: [],
         DistrictList: [],
         ContractorCategoryList: [],
         MemshipContractTypeSelectList: [],
         PrintForSumLoading: false,
         totals: {
            TotalmemshipApplicationAcceptedCount: 0,
            TotalmemshipApplicationRejectedCount: 0,
            TotalmemshipApplicationReviewCount: 0,
            TotalmemshipCertificateFormedCount: 0,
            TotalmemshipCertificateReviewCount: 0,
            TotalmemshipContractAcceptedCount: 0,
            TotalmemshipContractRejectedCount: 0,
            TotalmemshipContractReviewCount: 0,
            TotaltotalMemshipApplicationCount: 0,
            TotaltotalMemshipCertificateCount: 0,
            TotaltotalMemshipContractCount: 0
         },
         filter: {
            regionId: null,
            region: '',
            districtId: null,
            district: '',
            isOld: null,
            contractorCategoryId: null,
            memshipContractTypeId: null,
            fromDocDate: '',
            toDocDate: '',
            search: '',
            sortBy: '',
            orderType: 'asc',
            page: 1,
            pageSize: 20,
            perPageOptions: [10, 20, 50, 100, 300],
            count: 0
         },
         isBusy: false,
         localStorageData: {}
      };
   },
   computed: {
      firstNumber() {
         return (this.filter.page - 1) * this.filter.pageSize + 1;
      },
      lastNumber() {
         if (this.filter.count < this.filter.pageSize) {
            return this.filter.count;
         } else {
            if (this.filter.page * this.filter.pageSize > this.filter.count) {
               return this.filter.count;
            } else {
               return this.filter.page * this.filter.pageSize;
            }
         }
      }
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

      ManualService.ContractorCategorySelectList().then((res) => {
         this.ContractorCategoryList = res.data;
      });

      ManualService.MemshipContractTypeSelectList().then((res1) => {
         this.MemshipContractTypeSelectList = res1.data;
      });
      if (this.$route.params.regionId) {
         this.filter = { ...this.filter, ...this.$route.params };
         this.GetDistrict(this.filter.regionId);
      }

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
            this.filter.district = '';

            this.Refresh();
         }
      },
      BindValue(value) {
         this.filter.dateofbirth = value;
      },
      PrintForSum() {
         this.PrintForSumLoading = true;
         ReportService.SaveAsExcelGetMemshipDocsInfo(this.filter)
            .then((res) => {
               this.forceFileDownload(res, this.$t('prtnapplicationandcontractinfo'));
            })
            .catch((error) => {
               this.PrintForSumLoading = false;
               this.showApiError(error);
            })
            .finally(() => {
               this.PrintForSumLoading = false;
            });
      },
      Refresh() {
         this.isBusy = true;
         ReportService.GetMemshipDocsInfoReestr(this.filter)
            .then((res) => {
               this.items = res.data.rows;
               this.filter.count = res.data.count;

               this.isBusy = false;
               // if (res.data && res.data[0].districtId != null) {
               //    this.filter.regionId = res.data[0].regionId;
               //    this.filter.byDistrict = true;
               //    this.filter.byRegion = false;
               //    this.filter.byContractor = false;
               // }
               // if (res.data && res.data[0].contractorId != null) {
               //    this.filter.districtId = res.data[0].districtId;
               //    this.filter.byDistrict = false;
               //    this.filter.byRegion = false;
               //    this.filter.byContractor = true;
               // }
               // this.totals = {
               //    TotalmemshipApplicationAcceptedCount: 0,
               //    TotalmemshipApplicationRejectedCount: 0,
               //    TotalmemshipApplicationReviewCount: 0,
               //    TotalmemshipCertificateFormedCount: 0,
               //    TotalmemshipCertificateReviewCount: 0,
               //    TotalmemshipContractAcceptedCount: 0,
               //    TotalmemshipContractRejectedCount: 0,
               //    TotalmemshipContractReviewCount: 0,
               //    TotaltotalMemshipApplicationCount: 0,
               //    TotaltotalMemshipCertificateCount: 0,
               //    TotaltotalMemshipContractCount: 0
               // };
               // this.items.forEach((item) => {
               //    this.totals.TotalmemshipApplicationAcceptedCount += item.memshipApplicationAcceptedCount;
               //    this.totals.TotalmemshipApplicationReviewCount += item.memshipApplicationReviewCount;
               //    this.totals.TotalmemshipApplicationRejectedCount += item.memshipApplicationRejectedCount;
               //    this.totals.TotalmemshipContractAcceptedCount += item.memshipContractAcceptedCount;
               //    this.totals.TotalmemshipContractReviewCount += item.memshipContractReviewCount;
               //    this.totals.TotalmemshipContractRejectedCount += item.memshipContractRejectedCount;
               //    this.totals.TotalmemshipCertificateReviewCount += item.memshipCertificateReviewCount;
               //    this.totals.TotalmemshipCertificateFormedCount += item.memshipCertificateFormedCount;
               //    this.totals.TotaltotalMemshipApplicationCount += item.totalMemshipApplicationCount;
               //    this.totals.TotaltotalMemshipContractCount += item.totalMemshipContractCount;
               //    this.totals.TotaltotalMemshipCertificateCount += item.totalMemshipCertificateCount;
               // });
            })
            .catch((error) => {
               this.isBusy = false;
               this.showApiError(error);
            });
      }
   }
};
</script>

<style lang="scss" scoped>
@import '../styles.scss';

.table-scroll thead {
   background: #333;
   color: #fff;
   position: -webkit-sticky;
   position: sticky;
   top: 0;
   z-index: 4;
}
.table-scroll tfoot,
.table-scroll tfoot th,
.table-scroll tfoot td {
   position: -webkit-sticky;
   position: sticky;
   bottom: 0;
   background: #99bdf3;
   z-index: 4;
}
</style>
