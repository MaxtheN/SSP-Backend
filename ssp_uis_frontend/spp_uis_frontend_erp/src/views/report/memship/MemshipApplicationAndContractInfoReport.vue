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

         <b-col cols="12" class="text-right order-1">
            <b-button @click="PrintForSum" :disabled="PrintForSumLoading" variant="primary" class="ml-1">
               <feather-icon icon="PrinterIcon"></feather-icon>
               {{ $t('Print') }}
            </b-button>
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
                        <span v-show="filter.byRegion" style="font-weight: 900; font-size: 14px; color: black">{{
                           $t('region')
                        }}</span>
                        <span v-show="filter.byDistrict" style="font-weight: 900; font-size: 14px; color: black">{{
                           $t('Region')
                        }}</span>
                        <span v-show="filter.byContractor" style="font-weight: 900; font-size: 14px; color: black">{{
                           $t('contractorT')
                        }}</span>
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
                        <span v-show="filter.byRegion" style="color: blue; cursor: pointer" @click="SortRegion(item)">{{
                           item.region
                        }}</span>

                        <span
                           v-show="filter.byDistrict"
                           style="color: blue; cursor: pointer"
                           @click="SortDistrict(item)"
                           >{{ item.district }}</span
                        >

                        <span v-show="filter.byContractor">
                           {{ item.contractorInn }} -
                           {{ item.contractor }}
                        </span>
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
               <b-tfoot v-if="items.length > 0">
                  <tr>
                     <td></td>
                     <td style="font-weight: 900" class="text-center">
                        {{ $t('Total') }}
                     </td>
                     <td style="color: black; font-weight: 900" class="text-right">
                        {{ currency(totals.TotaltotalMemshipApplicationCount) }}
                     </td>
                     <td style="color: black; font-weight: 900" class="text-right">
                        {{ currency(totals.TotalmemshipApplicationAcceptedCount) }}
                     </td>
                     <td style="color: black; font-weight: 900" class="text-right">
                        {{ currency(totals.TotalmemshipApplicationReviewCount) }}
                     </td>
                     <td style="color: red; font-weight: 900" class="text-right">
                        {{ currency(totals.TotalmemshipApplicationRejectedCount) }}
                     </td>
                     <td class="text-right" style="color: black; font-weight: 900">
                        {{ currency(totals.TotaltotalMemshipContractCount) }}
                     </td>
                     <td style="color: black; font-weight: 900" class="text-right">
                        {{ currency(totals.TotalmemshipContractReviewCount) }}
                     </td>
                     <td style="color: black; font-weight: 900" class="text-right">
                        {{ currency(totals.TotalmemshipContractAcceptedCount) }}
                     </td>
                     <td style="color: red; font-weight: 900" class="text-right">
                        {{ currency(totals.TotalmemshipContractRejectedCount) }}
                     </td>
                     <td style="color: black; font-weight: 900" class="text-right">
                        {{ currency(totals.TotalmemshipCertificateReviewCount) }}
                     </td>
                     <td class="text-right">{{ currency(totals.TotalmemshipCertificateFormedCount) }}</td>
                  </tr>
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
            byRegion: true,
            byDistrict: false,
            byContractor: false
         },
         isBusy: false,
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

      ManualService.ContractorCategorySelectList().then((res) => {
         this.ContractorCategoryList = res.data;
      });

      ManualService.MemshipContractTypeSelectList().then((res1) => {
         this.MemshipContractTypeSelectList = res1.data;
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
         this.filter.byContractor = false;
         this.filter.regionId = item.regionId;
         this.filter.region = item.region;
         this.GetDistrict(item.regionId);
         this.Refresh();
      },
      SortDistrict(item) {
         this.filter.byDistrict = false;
         this.filter.byRegion = false;
         this.filter.byContractor = true;
         this.filter.districtId = item.districtId;
         this.filter.district = item.district;
         this.Refresh();
         this.$router.push({
            name: 'GetMemshipDocsInfoReestr',
            params: { ...this.filter }
         });
      },
      SortChange(data) {
         this.filter.Sort = data.sortBy;
         this.filter.Order = data.sortDesc ? 'desc' : 'asc';
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
            this.filter.district = '';
            this.filter.byDistrict = false;
            this.filter.byRegion = false;
            this.filter.byContractor = true;
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
         ReportService.GetMemshipApplicationAndContractInfo(this.filter)
            .then((res) => {
               this.items = res.data;
               this.isBusy = false;
               if (res.data && res.data[0].districtId != null) {
                  this.filter.regionId = res.data[0].regionId;
                  this.filter.byDistrict = true;
                  this.filter.byRegion = false;
                  this.filter.byContractor = false;
               }
               if (res.data && res.data[0].contractorId != null) {
                  this.filter.districtId = res.data[0].districtId;
                  this.filter.byDistrict = false;
                  this.filter.byRegion = false;
                  this.filter.byContractor = true;
               }
               this.totals = {
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
               };
               this.items.forEach((item) => {
                  this.totals.TotalmemshipApplicationAcceptedCount += item.memshipApplicationAcceptedCount;
                  this.totals.TotalmemshipApplicationReviewCount += item.memshipApplicationReviewCount;
                  this.totals.TotalmemshipApplicationRejectedCount += item.memshipApplicationRejectedCount;
                  this.totals.TotalmemshipContractAcceptedCount += item.memshipContractAcceptedCount;
                  this.totals.TotalmemshipContractReviewCount += item.memshipContractReviewCount;
                  this.totals.TotalmemshipContractRejectedCount += item.memshipContractRejectedCount;
                  this.totals.TotalmemshipCertificateReviewCount += item.memshipCertificateReviewCount;
                  this.totals.TotalmemshipCertificateFormedCount += item.memshipCertificateFormedCount;
                  this.totals.TotaltotalMemshipApplicationCount += item.totalMemshipApplicationCount;
                  this.totals.TotaltotalMemshipContractCount += item.totalMemshipContractCount;
                  this.totals.TotaltotalMemshipCertificateCount += item.totalMemshipCertificateCount;
               });
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

@media only screen and (max-width: 768px) {
   .order-1 {
      order: 1;
      margin-bottom: 1rem;
   }
   .order-2 {
      order: 2;
   }
}
</style>
