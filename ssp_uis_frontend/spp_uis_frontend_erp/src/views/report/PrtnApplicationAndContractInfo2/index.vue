<template>
   <b-card>
      <b-tabs pills>
         <b-tab
            @click="
               {
                  tabData = 1;
                  filter.byContractor = false;
                  Refresh();
               }
            "
            :title="$t('variant-1')"
         >
         </b-tab>
         <b-tab
            @click="
               {
                  tabData = 2;
                  filter.byContractor = true;
                  Refresh();
               }
            "
            :title="$t('variant-2')"
         >
         </b-tab>
      </b-tabs>
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
         <b-col sm="12" md="2">
            <div>
               <form-select
                  :options="OkedTypeList"
                  v-model="filter.okedTypeId"
                  @input="Refresh"
                  :label="$t('okedType')"
               ></form-select>
            </div>
         </b-col>
         <b-col sm="12" md="2">
            <form-picker :label="$t('startDate')" v-model="filter.startDate" />
         </b-col>
         <b-col sm="12" md="2">
            <form-picker :label="$t('endDate')" v-model="filter.endDate" />
         </b-col>
         <b-col cols="12" md="2" :class="{ 'mt-2': !isMobileDevice(), 'mb-1': isMobileDevice() }">
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
      <b-row>
         <b-col sm="12" md="8">
            <b-button-group @click="Refresh" size="sm" class="mr-2">
               <b-button
                  @click="filter.prtnContractTypeId = null"
                  :variant="null == filter.prtnContractTypeId ? 'primary' : 'outline-primary'"
                  >{{ $t('all') }}</b-button
               >
               <b-button
                  v-for="type in PrtnContractTypeList"
                  :key="type.value"
                  @click="filter.prtnContractTypeId = type.value"
                  :variant="type.value == filter.prtnContractTypeId ? 'primary' : 'outline-primary'"
                  >{{ type.text }}</b-button
               >
            </b-button-group>

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
               <!-- <b-breadcrumb-item v-show="filter.byContractor" active>Baz</b-breadcrumb-item> -->
            </b-breadcrumb>
         </b-col>
         <b-col class="text-right">
            <b-button @click="PrintForSum" :disabled="PrintForSumLoading" variant="primary" class="ml-1">
               <feather-icon icon="PrinterIcon"></feather-icon>
               {{ $t('Print') }}
            </b-button>
         </b-col>
      </b-row>

      <div class="mt-2 report-table">
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
                        <span v-show="filter.byContractor" style="font-weight: 900; font-size: 14px; color: black">{{
                           $t('contractorT')
                        }}</span>
                     </b-th>
                     <b-th rowspan="2" colspan="2">
                        <span style="font-weight: 900; font-size: 14px; color: black">{{
                           $t('totalApplicationCount')
                        }}</span>
                     </b-th>
                     <b-th colspan="6" style="font-weight: 900; font-size: 14px; color: black">{{
                        $t('shundan')
                     }}</b-th>
                     <b-th rowspan="3"
                        ><span style="font-weight: 900; font-size: 14px; color: black">{{
                           $t('NumberOfPartnershipAgreementsCreated')
                        }}</span>
                     </b-th>
                     <b-th colspan="8" style="font-weight: 900; font-size: 14px; color: black">{{
                        $t('shundan')
                     }}</b-th>
                     <b-th colspan="2" style="font-weight: 900; font-size: 14px; color: black">{{
                        $t('Сертификат шакллантирилган')
                     }}</b-th>
                     <b-th rowspan="3"
                        ><span style="font-weight: 900; font-size: 14px; color: red">{{
                           $t('TotalPrtnCertificateCanceledCount')
                        }}</span></b-th
                     >
                  </b-tr>
                  <b-tr>
                     <b-th rowspan="2"
                        ><span style="font-weight: 900; font-size: 14px; color: black">{{
                           $t('TheNumberApplicationsSentConclusion')
                        }}</span></b-th
                     >
                     <b-th colspan="3" style="font-weight: 900; font-size: 14px; color: black">{{
                        $t('shundan')
                     }}</b-th>

                     <b-th rowspan="2" style="font-weight: 900; font-size: 14px; color: red">{{
                        $t('Шартномаси рад этилган аризалар')
                     }}</b-th>
                     <b-th rowspan="2" style="font-weight: 900; font-size: 14px; color: red">{{
                        $t('Сертификати рад этилган аризалар')
                     }}</b-th>

                     <b-th rowspan="2"
                        ><span style="font-weight: 900; font-size: 14px; color: black">{{
                           $t('totalPrtnApplicationSentForExpertisesCount')
                        }}</span></b-th
                     >
                     <b-th colspan="3" style="font-weight: 900; font-size: 14px; color: black">{{
                        $t('shundan')
                     }}</b-th>
                     <b-th rowspan="2"
                        ><span style="font-weight: 900; font-size: 14px; color: black">{{
                           $t('TheNumberOfContractsInTheProcessOfSigning')
                        }}</span></b-th
                     >
                     <b-th rowspan="2"
                        ><span style="font-weight: 900; font-size: 14px; color: black">{{
                           $t('totalPrtnApplicationSignedCount')
                        }}</span></b-th
                     >
                     <b-th rowspan="2"
                        ><span style="font-weight: 900; font-size: 14px; color: red">{{
                           $t('Имзолаш жараёнида рад этилган шартномалар')
                        }}</span></b-th
                     >
                     <b-th rowspan="2"
                        ><span style="font-weight: 900; font-size: 14px; color: red">{{
                           $t('Сертификати рад этилган аризалар')
                        }}</span></b-th
                     >
                     <b-th rowspan="2">
                        <span style="font-weight: 900; font-size: 14px; color: black">{{ $t('CERTIFICATES') }}</span>
                     </b-th>
                     <b-th rowspan="2"
                        ><span style="font-weight: 900; font-size: 14px; color: black">{{
                           $t('TotalNumberOfNewJobsToBeCreated')
                        }}</span></b-th
                     >
                  </b-tr>
                  <b-tr>
                     <b-th style="font-weight: 900; font-size: 14px; color: black">{{ $t('count') }}</b-th>
                     <b-th style="font-weight: 900; font-size: 14px; color: black">{{ $t('newVacanciesCount') }}</b-th>
                     <b-th style="font-weight: 900; font-size: 14px; color: black">{{
                        $t('TheNumberOfApplicationsWithPositiveConclusion')
                     }}</b-th>
                     <b-th style="font-weight: 900; font-size: 14px; color: black">{{
                        $t('NumberOfApplicationsPendingReview')
                     }}</b-th>
                     <b-th style="font-weight: 900; font-size: 14px; color: red">{{ $t('custom3') }}</b-th>
                     <b-th style="font-weight: 900; font-size: 14px; color: black">{{
                        $t('NumberOfContractsExercised')
                     }}</b-th>
                     <b-th style="font-weight: 900; font-size: 14px; color: black">{{
                        $t('TheNumberOfContractsUnderReview')
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
                        <!-- {{ item.region }} -->
                     </b-td>
                     <b-td class="text-right">{{ currency(item.totalApplication.item1) }}</b-td>
                     <b-td class="text-right">{{ currency(item.totalApplication.item2) }}</b-td>
                     <b-td class="text-right">{{
                        currency(
                           item.totalPrtnApplicationSentAcceptedCount +
                              item.totalPrtnApplicationSentForReviewCount +
                              item.totalPrtnApplicationSentRejectedCount
                        )
                     }}</b-td>
                     <b-td class="text-right">{{ currency(item.totalPrtnApplicationSentAcceptedCount) }}</b-td>
                     <b-td class="text-right">{{ currency(item.totalPrtnApplicationSentForReviewCount) }}</b-td>
                     <b-td style="color: red; font-weight: 900" class="text-right">{{
                        currency(
                           item.totalPrtnApplicationCanceledWhithOutRejectCount +
                              item.totalPrtnApplicationCanceledWhithOutContractCount
                        )
                     }}</b-td>
                     <b-td style="color: red; font-weight: 900" class="text-right">{{
                        currency(
                           item.totalPrtnContractCanceledWhithOutCertificateCount +
                              item.totalPrtnContractRejectWhithOutCertificateCount
                        )
                     }}</b-td>
                     <b-td style="color: red; font-weight: 900" class="text-right">{{
                        currency(
                           item.totalPrtnCertificateCanceledApplicationCount +
                              item.totalPrtnCertificateRejectApplicationCount
                        )
                     }}</b-td>

                     <b-td class="text-right">{{ currency(item.totalPrtnContractCount) }}</b-td>
                     <b-td class="text-right">{{ currency(item.totalPrtnContractCount) }}</b-td>
                     <b-td class="text-right">{{
                        currency(
                           item.totalPrtnApplicationPassExpertisesCount +
                              item.totalPrtnApplicationSignningCount +
                              item.totalPrtnApplicationSignedCount
                        )
                     }}</b-td>
                     <b-td class="text-right">{{ currency(item.totalPrtnApplicationSentForExpertisesCount) }}</b-td>
                     <b-td class="text-right" style="color: red; font-weight: 900">{{
                        currency(item.totalPrtnApplicationNotPassExpertisesCount)
                     }}</b-td>
                     <b-td class="text-right">{{
                        currency(item.totalPrtnApplicationSignningCount + item.totalPrtnApplicationPassExpertisesCount)
                     }}</b-td>

                     <b-td class="text-right">{{ currency(item.totalPrtnApplicationSignedCount) }}</b-td>
                     <b-td style="color: red; font-weight: 900" class="text-right">{{
                        currency(
                           item.totalPrtnContractCanceledWhithOutCertificateCount +
                              item.totalPrtnContractRejectWhithOutCertificateCount
                        )
                     }}</b-td>
                     <b-td style="color: red; font-weight: 900" class="text-right">{{
                        currency(
                           item.totalPrtnCertificateCanceledApplicationCount +
                              item.totalPrtnCertificateRejectApplicationCount
                        )
                     }}</b-td>
                     <b-td class="text-right">{{ currency(item.totalPrtnCertificateCount) }}</b-td>
                     <b-td class="text-right">{{ currency(item.totalNewVacanciesCount) }}</b-td>
                     <b-td class="text-right" style="color: red; font-weight: 900">{{
                        currency(item.totalPrtnCertificateCanceledCount)
                     }}</b-td>
                  </b-tr>
               </b-tbody>
               <b-tfoot v-if="items.length > 0">
                  <tr>
                     <td></td>
                     <td
                        style="font-weight: 900"
                        class="text-center table-b-table-default"
                        :class="{ 'b-table-sticky-column': !isMobileDevice() }"
                     >
                        {{ $t('Total') }}
                     </td>
                     <td style="color: black; font-weight: 900" class="text-right">
                        {{ currency(totals.item1) }}
                     </td>
                     <td style="color: black; font-weight: 900" class="text-right">
                        {{ currency(totals.item2) }}
                     </td>

                     <td style="color: black; font-weight: 900" class="text-right">
                        {{ currency(totals.TotalPrtnApplicationSentRejectedCount2) }}
                     </td>
                     <td style="color: black; font-weight: 900" class="text-right">
                        {{ currency(totals.TotalPrtnApplicationSentAcceptedCount) }}
                     </td>
                     <td style="color: black; font-weight: 900" class="text-right">
                        {{ currency(totals.TotalPrtnApplicationSentForReviewCount) }}
                     </td>
                     <td class="text-right" style="color: red; font-weight: 900">
                        {{
                           currency(
                              totals.TotalPrtnApplicationCanceledWhithOutRejectCount +
                                 totals.TotalPrtnApplicationCanceledWhithOutContractCount
                           )
                        }}
                     </td>
                     <b-td style="color: red; font-weight: 900" class="text-right">{{
                        currency(
                           totals.TotalPrtnContractCanceledWhithOutCertificateCount +
                              totals.TotalPrtnContractRejectWhithOutCertificateCount
                        )
                     }}</b-td>
                     <b-td style="color: red; font-weight: 900" class="text-right">{{
                        currency(
                           totals.TotalPrtnCertificateCanceledApplicationCount +
                              totals.TotalPrtnCertificateRejectApplicationCount
                        )
                     }}</b-td>

                     <td style="color: black; font-weight: 900" class="text-right">
                        {{ currency(totals.TotalPrtnApplicationSentAcceptedCount) }}
                     </td>
                     <td style="color: black; font-weight: 900" class="text-right">
                        {{ currency(totals.TotalPrtnApplicationSentAcceptedCount) }}
                     </td>
                     <td style="color: black; font-weight: 900" class="text-right">
                        {{ currency(totals.TotalPrtnApplicationPassExpertisesCount) }}
                     </td>
                     <td style="color: black; font-weight: 900" class="text-right">
                        {{ currency(totals.TotalPrtnApplicationSentForExpertisesCount) }}
                     </td>
                     <td class="text-right" style="color: red; font-weight: 900">
                        {{ currency(totals.TotalPrtnContractRejectedCount) }}
                     </td>
                     <td style="color: black; font-weight: 900" class="text-right">
                        {{ currency(totals.TotalPrtnApplicationSignningCount) }}
                     </td>
                     <td style="color: black; font-weight: 900" class="text-right">
                        {{ currency(totals.TotalPrtnApplicationSignedCount) }}
                     </td>
                     <b-td style="color: red; font-weight: 900" class="text-right">{{
                        currency(
                           totals.TotalPrtnContractCanceledWhithOutCertificateCount +
                              totals.TotalPrtnContractRejectWhithOutCertificateCount
                        )
                     }}</b-td>
                     <b-td style="color: red; font-weight: 900" class="text-right">{{
                        currency(
                           totals.TotalPrtnCertificateCanceledApplicationCount +
                              totals.TotalPrtnCertificateRejectApplicationCount
                        )
                     }}</b-td>
                     <td style="color: black; font-weight: 900" class="text-right">
                        {{ currency(totals.TotalPrtnCertificateCount) }}
                     </td>
                     <td style="color: black; font-weight: 900" class="text-right">
                        {{ currency(totals.TotalNewVacanciesCount) }}
                     </td>
                     <td style="color: red; font-weight: 900" class="text-right">
                        {{ currency(totals.TotalPrtnCertificateCanceledCount) }}
                     </td>
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
         <div class="mx-2 mb-2" v-if="tabData == 2">
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
                     :dir="$store.state.appConfig.isRTL ? 'rtl' : 'ltr'"
                     :options="filter.perPageOptions"
                     @input="Refresh"
                     :clearable="false"
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
import PrtnContractTypeService from '@/services/info/prtncontracttype.service';
import ManualService from '@/services/others/manual.service';
import RegionService from '@/services/info/region.service';
import DistrictService from '@/services/info/district.service';

const TOTALSDEF = {
   item1: 0,
   item2: 0,
   TotalPrtnApplicationSentRejectedCount: 0,
   TotalPrtnApplicationSentRejectedCount2: 0,
   TotalNewVacanciesCount: 0,
   TotalPrtnApplicationSentAcceptedCount: 0,
   TotalPrtnApplicationSentForReviewCount: 0,
   ToTalPrtnApplicationSentRejectedCount: 0,
   ToTalPrtnApplicationCanceledCount: 0,
   TotalPrtnApplicationPassExpertisesCount: 0,
   TotalPrtnApplicationSentForExpertisesCount: 0,
   TotalPrtnContractRejectedCount: 0,
   TotalPrtnApplicationSignningCount: 0,
   TotalPrtnApplicationSignedCount: 0,
   TotalPrtnContractorCancelCount: 0,
   TotalPrtnCertificateCount: 0,
   TotalPrtnCertificateCanceledCount: 0,
   TotalPrtnCertificateRejectApplicationCount: 0,
   TotalPrtnContractCanceledWhithOutCertificateCount: 0,
   TotalPrtnApplicationCanceledWhithOutRejectCount: 0,
   TotalPrtnApplicationCanceledWhithOutContractCount: 0,
   TotalPrtnContractRejectWhithOutCertificateCount: 0,
   TotalPrtnCertificateCanceledApplicationCount: 0
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
   name: 'Index',
   directives: {
      'b-tooltip': VBTooltip,
      'b-modal': VBModal
   },
   data() {
      return {
         tabData: 1,
         items: [],
         RegionList: [],
         DistrictList: [],
         PrtnContractTypeList: [],
         OkedTypeList: [],
         PrintForSumLoading: false,
         totals: { ...TOTALSDEF },
         filter: {
            startDate: null,
            endDate: null,
            prtnContractTypeId: null,
            regionId: null,
            region: '',
            byRegion: true,
            districtId: null,
            district: '',
            byDistrict: false,
            contractorId: null,
            byContractor: false,
            page: 1,
            pageSize: 20,
            perPageOptions: [10, 20, 50, 100],
            total: 0
         },
         isBusy: false
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
      ManualService.OkedTypeSelectList()
         .then((res) => {
            this.OkedTypeList = res.data;
         })
         .catch((error) => {
            this.showApiError(error);
         });
      PrtnContractTypeService.GetAsSelectList()
         .then((res) => {
            this.PrtnContractTypeList = res.data;
            this.Refresh();
         })
         .catch((error) => {
            this.showApiError(error);
         });
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
   methods: {
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
            this.filter.byDistrict = true;
            this.filter.byRegion = false;
            this.filter.byContractor = false;
            this.filter.district = '';
            this.Refresh();
         }
      },
      BindValue(value) {
         this.filter.dateofbirth = value;
      },
      Print() {
         ReportService.SaveasExcelSoliqImtiyoz(this.filter).then((res) => {
            this.forceFileDownload(res, this.$t('getprtnapplicationbycontracttype'));
         });
      },
      PrintForSum() {
         this.PrintForSumLoading = true;
         ReportService.SaveAsExecelForSum(this.filter)
            .then((res) => {
               this.forceFileDownload(res, this.$t('prtnapplicationandcontractinfo'));
            })
            .finally(() => {
               this.PrintForSumLoading = false;
            });
      },
      Refresh() {
         if (this.tabData == 1) {
            this.isBusy = true;
            ReportService.GetPrtnApplicationAndContractInfo(this.filter)
               .then((res) => {
                  this.items = res.data;
                  this.isBusy = false;
                  this.totals = { ...TOTALSDEF };

                  this.items.forEach((item) => {
                     this.totals.TotalPrtnApplicationSentRejectedCount +=
                        item.totalPrtnApplicationSentRejectedCount +
                        item.totalPrtnApplicationCount +
                        item.totalPrtnApplicationCanceledCount +
                        item.totalPrtnApplicationSentRevokedCount;
                     this.totals.TotalPrtnApplicationSentRejectedCount2 +=
                        item.totalPrtnApplicationSentAcceptedCount +
                        item.totalPrtnApplicationSentForReviewCount +
                        item.totalPrtnApplicationSentRejectedCount;
                     this.totals.TotalNewVacanciesCount += item.totalNewVacanciesCount;
                     this.totals.TotalPrtnApplicationSentAcceptedCount += item.totalPrtnApplicationSentAcceptedCount;
                     this.totals.TotalPrtnApplicationSentForReviewCount += item.totalPrtnApplicationSentForReviewCount;
                     this.totals.ToTalPrtnApplicationSentRejectedCount += item.totalPrtnApplicationSentRejectedCount;
                     this.totals.ToTalPrtnApplicationCanceledCount += item.totalPrtnApplicationCanceledCount;
                     this.totals.TotalPrtnApplicationPassExpertisesCount +=
                        item.totalPrtnApplicationPassExpertisesCount +
                        item.totalPrtnApplicationSignningCount +
                        item.totalPrtnApplicationSignedCount;
                     this.totals.TotalPrtnApplicationSentForExpertisesCount +=
                        item.totalPrtnApplicationSentForExpertisesCount;
                     this.totals.TotalPrtnContractRejectedCount += item.totalPrtnApplicationNotPassExpertisesCount;
                     this.totals.TotalPrtnApplicationSignningCount +=
                        item.totalPrtnApplicationSignningCount + item.totalPrtnApplicationPassExpertisesCount;
                     this.totals.TotalPrtnApplicationSignedCount += item.totalPrtnApplicationSignedCount;
                     this.totals.TotalPrtnContractorCancelCount += item.totalPrtnContractCancelCount;
                     this.totals.TotalPrtnCertificateCount += item.totalPrtnCertificateCount;

                     this.totals.TotalPrtnCertificateCanceledCount += item.totalPrtnCertificateCanceledCount;
                     this.totals.TotalPrtnContractCanceledWhithOutCertificateCount +=
                        item.totalPrtnContractCanceledWhithOutCertificateCount;

                     this.totals.TotalPrtnCertificateRejectApplicationCount +=
                        item.totalPrtnCertificateRejectApplicationCount;

                     this.totals.TotalPrtnApplicationCanceledWhithOutRejectCount +=
                        item.totalPrtnApplicationCanceledWhithOutRejectCount;
                     this.totals.TotalPrtnApplicationCanceledWhithOutContractCount +=
                        item.totalPrtnApplicationCanceledWhithOutContractCount;
                     this.totals.TotalPrtnContractRejectWhithOutCertificateCount +=
                        item.totalPrtnContractRejectWhithOutCertificateCount;
                     this.totals.TotalPrtnCertificateCanceledApplicationCount +=
                        item.totalPrtnCertificateCanceledApplicationCount;

                     this.totals.item1 += item.totalApplication.item1;
                     this.totals.item2 += item.totalApplication.item2;
                  });
               })
               .catch((error) => {
                  this.isBusy = false;
                  this.showApiError(error);
               });
         } else {
            this.isBusy = true;
            console.log('ddd');

            ReportService.GetPrtnApplicationAndContractInfoPaged(this.filter)
               .then((res) => {
                  this.items = res.data.rows;
                  this.isBusy = false;
                  this.filter.total = res.data.total;
                  this.totals = { ...TOTALSDEF };

                  this.items.forEach((item) => {
                     this.totals.TotalPrtnApplicationSentRejectedCount +=
                        item.totalPrtnApplicationSentRejectedCount +
                        item.totalPrtnApplicationCount +
                        item.totalPrtnApplicationCanceledCount +
                        item.totalPrtnApplicationSentRevokedCount;
                     this.totals.TotalPrtnApplicationSentRejectedCount2 +=
                        item.totalPrtnApplicationSentAcceptedCount +
                        item.totalPrtnApplicationSentForReviewCount +
                        item.totalPrtnApplicationSentRejectedCount;
                     this.totals.TotalNewVacanciesCount += item.totalNewVacanciesCount;
                     this.totals.TotalPrtnApplicationSentAcceptedCount += item.totalPrtnApplicationSentAcceptedCount;
                     this.totals.TotalPrtnApplicationSentForReviewCount += item.totalPrtnApplicationSentForReviewCount;
                     this.totals.ToTalPrtnApplicationSentRejectedCount += item.totalPrtnApplicationSentRejectedCount;
                     this.totals.ToTalPrtnApplicationCanceledCount += item.totalPrtnApplicationCanceledCount;
                     this.totals.TotalPrtnApplicationPassExpertisesCount +=
                        item.totalPrtnApplicationPassExpertisesCount +
                        item.totalPrtnApplicationSignningCount +
                        item.totalPrtnApplicationSignedCount;
                     this.totals.TotalPrtnApplicationSentForExpertisesCount +=
                        item.totalPrtnApplicationSentForExpertisesCount;
                     this.totals.TotalPrtnContractRejectedCount += item.totalPrtnApplicationNotPassExpertisesCount;
                     this.totals.TotalPrtnApplicationSignningCount +=
                        item.totalPrtnApplicationSignningCount + item.totalPrtnApplicationPassExpertisesCount;
                     this.totals.TotalPrtnApplicationSignedCount += item.totalPrtnApplicationSignedCount;
                     this.totals.TotalPrtnContractorCancelCount += item.totalPrtnContractCancelCount;
                     this.totals.TotalPrtnCertificateCount += item.totalPrtnCertificateCount;

                     this.totals.TotalPrtnCertificateCanceledCount += item.totalPrtnCertificateCanceledCount;
                     this.totals.TotalPrtnContractCanceledWhithOutCertificateCount +=
                        item.totalPrtnContractCanceledWhithOutCertificateCount;

                     this.totals.TotalPrtnCertificateRejectApplicationCount +=
                        item.totalPrtnCertificateRejectApplicationCount;

                     this.totals.TotalPrtnApplicationCanceledWhithOutRejectCount +=
                        item.totalPrtnApplicationCanceledWhithOutRejectCount;
                     this.totals.TotalPrtnApplicationCanceledWhithOutContractCount +=
                        item.totalPrtnApplicationCanceledWhithOutContractCount;
                     this.totals.TotalPrtnContractRejectWhithOutCertificateCount +=
                        item.totalPrtnContractRejectWhithOutCertificateCount;
                     this.totals.TotalPrtnCertificateCanceledApplicationCount +=
                        item.totalPrtnCertificateCanceledApplicationCount;

                     this.totals.item1 += item.totalApplication.item1;
                     this.totals.item2 += item.totalApplication.item2;
                  });
               })
               .catch((error) => {
                  this.isBusy = false;
                  this.showApiError(error);
               });
         }
      }
   }
};
</script>

<style lang="scss" scoped>
@import '../styles.scss';
</style>
