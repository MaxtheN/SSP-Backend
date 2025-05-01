<template>
   <b-card>
      <b-row>
         <b-col sm="12" md="4">
            <div>
               <form-select
                  @change="Refresh"
                  v-model="filter.regionalOrganizationId"
                  :options="OrganizationList"
                  :label="$t('organization')"
               />
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
         <!-- <b-col>
            <label for>{{ $t('search') }}</label>
            <b-input-group class="text-right">
               <b-form-input v-model="filter.search" :placeholder="$t('search')" />
               <b-input-group-append>
                  <b-button @click="Refresh" variant="primary">
                     <feather-icon icon="SearchIcon" />
                  </b-button>
               </b-input-group-append>
            </b-input-group>
         </b-col> -->

         <!-- <b-col class="col-auto mt-2 text-right">
            <b-button @click="Print" :disabled="PrintLoading" variant="primary">
               <feather-icon icon="PrinterIcon"></feather-icon>
               {{ $t('Print') }}
            </b-button>
         </b-col> -->
      </b-row>
      <b-row align-h="between">
         <b-col sm="12" md="8" class="p-0">
            <b-breadcrumb class="">
               <b-breadcrumb-item
                  :active="filter.byRegion"
                  @click="
                     () => {
                        filter.byOrganization = true;
                        filter.regionalOrganizationId = null;
                        filter.byContractor = false;
                        filter.contractorId = null;
                        filter.isOld = false;
                        Refresh();
                     }
                  "
               >
                  <b>{{ $t('uzb') }}</b>
               </b-breadcrumb-item>
               <b-breadcrumb-item
                  v-show="filter.byContractor"
                  :active="filter.byContractor"
                  @click="
                     () => {
                        filter.byOrganization = false;
                        filter.byContractor = true;
                        Refresh();
                     }
                  "
               >
                  <b>{{ filter.organization }}</b>
               </b-breadcrumb-item>
               <b-breadcrumb-item v-show="filter.district" :active="filter.byContractor">
                  <b>{{ filter.district }}</b>
               </b-breadcrumb-item>
            </b-breadcrumb>
         </b-col>
      </b-row>
      <div class="report-table">
         <!-- <b-tabs v-model="tab">
            <b-tab
               :title="$t('Eski')"
               @click="
                  () => {
                     filter.isOld = true;
                     Refresh();
                  }
               "
            ></b-tab>
            <b-tab
               :title="$t('new')"
               @click="
                  () => {
                     filter.isOld = false;
                     Refresh();
                  }
               "
            ></b-tab>
         </b-tabs> -->
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
                        <span style="width: 200px" v-show="filter.byOrganization">{{ $t('organization') }}</span>
                        <span v-show="filter.byContractor">{{ $t('contractor') }}</span>
                     </b-th>
                     <b-th rowspan="3" style="font-weight: 900; font-size: 14px; color: black">{{
                        $t('Жами имзоланган шартномалар сони')
                     }}</b-th>
                     <b-th style="font-weight: 900; font-size: 14px; color: black" rowspan="3">{{ $t('amount') }}</b-th>
                     <b-th style="font-weight: 900; font-size: 14px; color: black" rowspan="3">{{
                        $t("Shundan to'lov amalga oshirgan korxonalar soni")
                     }}</b-th>
                     <b-th style="font-weight: 900; font-size: 14px; color: black" rowspan="3">{{
                        $t("Shartnoma bo'yicha to'lanishi lozim bo'lgan summasi")
                     }}</b-th>
                     <b-th style="font-weight: 900; font-size: 14px; color: black" rowspan="3">{{
                        $t("Xaqiqatdato'langansummasi")
                     }}</b-th>
                     <b-th style="font-weight: 900; font-size: 14px; color: black" rowspan="3">{{
                        $t("Muddatli to'lov summasi")
                     }}</b-th>
                     <b-th style="font-weight: 900; font-size: 14px; color: black" rowspan="3">{{
                        $t('Tushim amalga oshirilmagan summasi')
                     }}</b-th>
                     <b-th style="font-weight: 900; font-size: 14px; color: black" rowspan="3">{{
                        $t("Shartnomaga muvofiq 10 bank kuni ichida to'lov amalga oshirilmagan summasi")
                     }}</b-th>
                     <b-th style="font-weight: 900; font-size: 14px; color: black" rowspan="3">{{
                        $t("Xozirgikundato'lanishilozimbo'langansummasi")
                     }}</b-th>
                  </b-tr>
                  <b-tr> </b-tr>
                  <b-tr> </b-tr>
               </b-thead>
               <b-tbody>
                  <b-tr v-for="(item, i) in items" :key="i">
                     <b-td>{{ i + 1 }}</b-td>
                     <b-td>
                        <div
                           v-show="filter.byOrganization"
                           @click="filtirByOrganization(item)"
                           style="min-width: 500px; white-space: break-spaces; color: blue"
                        >
                           {{ item.regionalOrganization }}
                        </div>
                        <div v-show="filter.byContractor" style="min-width: 500px; white-space: break-spaces">
                           <span style="color: blue">{{ item.contractorInn }}</span>
                           {{ item.contractor }}
                        </div>
                     </b-td>
                     <b-td class="text-right">{{ currency(item.totalMemshipContractCount) }}</b-td>
                     <b-td class="text-right">{{ currency(item.totalAmount) }}</b-td>
                     <b-td class="text-right">{{ currency(item.totalPaidContractCount) }}</b-td>
                     <b-td class="text-right">{{ currency(item.totalPaidFromMemshimpContractAmount) }}</b-td>
                     <b-td class="text-right">{{ currency(item.totalPaymentAmount) }}</b-td>
                     <b-td class="text-right">{{ currency(item.totalTermPaymentAmount) }}</b-td>
                     <b-td class="text-right">{{ currency(item.totalNotPaidAmount) }}</b-td>
                     <b-td class="text-right">{{ currency(item.totalUnpaidOnTimeAmount) }}</b-td>
                     <b-td class="text-right">{{ currency(item.totalPaymentInDue) }}</b-td>
                     <!-- <b-td>{{ item.totalAmountRealized }}</b-td> -->
                  </b-tr>
                  <b-tr v-if="!items.length" class="py-1 text-center"
                     ><b-td colspan="11">{{ $t('NotFound') }}</b-td></b-tr
                  >
               </b-tbody>
               <b-tfoot>
                  <b-tr variant="secondary">
                     <b-td></b-td>
                     <b-td class="text-center" :class="{ 'b-table-sticky-column': !isMobileDevice() }">{{
                        $t('Total')
                     }}</b-td>
                     <b-td class="text-right">{{ currency(totals.totalMemshipContractCount) }}</b-td>
                     <b-td class="text-right">{{ currency(totals.totalAmount) }}</b-td>
                     <b-td class="text-right">{{ currency(totals.totalPaidContractCount) }}</b-td>
                     <b-td class="text-right">{{ currency(totals.totalPaidFromMemshimpContractAmount) }}</b-td>
                     <b-td class="text-right">{{ currency(totals.totalPaymentAmount) }}</b-td>
                     <b-td class="text-right">{{ currency(totals.totalTermPaymentAmount) }}</b-td>
                     <b-td class="text-right">{{ currency(totals.totalNotPaidAmount) }}</b-td>
                     <b-td class="text-right">{{ currency(totals.totalUnpaidOnTimeAmount) }}</b-td>
                     <b-td class="text-right">{{ currency(totals.totalPaymentInDue) }}</b-td>
                     <!-- <b-td class="text-right">{{ currency(totals.memshipCertificateIndebtednessAmount) }}</b-td>
                     <b-td class="text-right">{{ currency(totals.memshipCertificateIndebtednessAmount) }}</b-td> -->
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
   directives: {
      'b-tooltip': VBTooltip,
      'b-modal': VBModal
   },
   data() {
      return {
         items: [],
         OrganizationList: [],
         tab: 0,
         filter: {
            isOld: false,
            organization: '',
            regionalOrganizationId: null,
            byOrganization: true,
            contractorId: null,
            byContractor: false,
            startDate: '',
            endDate: ''
         },
         totals: {
            totalMemshipContractCount: 0,
            totalAmount: 0,
            totalPaidContractCount: 0,
            totalPaidFromMemshimpContractAmount: 0,
            totalPaymentAmount: 0,
            totalTermPaymentAmount: 0,
            totalNotPaidAmount: 0,
            totalUnpaidOnTimeAmount: 0,
            totalPaymentInDue: 0
         },
         isBusy: false,
         PrintLoading: false
      };
   },
   created() {
      this.Refresh();
      ManualService.OrganizationAsSelectListByGroup([1, 3]).then((res) => {
         this.OrganizationList = res.data;
      });
   },
   methods: {
      Refresh() {
         this.isBusy = true;
         ReportService.GetMemshipContract(this.filter)
            .then((res) => {
               this.items = res.data;
               this.totals = {
                  totalMemshipContractCount: 0,
                  totalAmount: 0,
                  totalPaidContractCount: 0,
                  totalPaidFromMemshimpContractAmount: 0,
                  totalPaymentAmount: 0,
                  totalTermPaymentAmount: 0,
                  totalNotPaidAmount: 0,
                  totalUnpaidOnTimeAmount: 0,
                  totalPaymentInDue: 0
               };
               res.data.forEach((item) => {
                  this.totals.totalMemshipContractCount += item.totalMemshipContractCount;
                  this.totals.totalAmount += item.totalAmount;
                  this.totals.totalPaidContractCount += item.totalPaidContractCount;
                  this.totals.totalPaidFromMemshimpContractAmount += item.totalPaidFromMemshimpContractAmount;
                  this.totals.totalPaymentAmount += item.totalPaymentAmount;
                  this.totals.totalTermPaymentAmount += item.totalTermPaymentAmount;
                  this.totals.totalNotPaidAmount += item.totalNotPaidAmount;
                  this.totals.totalUnpaidOnTimeAmount += item.totalUnpaidOnTimeAmount;
                  this.totals.totalPaymentInDue += item.totalPaymentInDue;
               });
               this.isBusy = false;
            })
            .catch((error) => {
               this.showApiError(error);
            })
            .finally(() => {
               this.isBusy = false;
            });
      },
      filtirByOrganization(item) {
         console.log(item);
         this.filter.regionalOrganizationId = item.regionalOrganizationId;
         this.filter.byContractor = true;
         this.filter.byOrganization = false;
         this.filter.organization = item.regionalOrganization;
         this.Refresh();
      }
      // Print() {
      //    this.PrintLoading = true;
      //    ReportService.SaveAsExcelGetPaidMemshipReport(this.filter)
      //       .then((res) => {
      //          this.forceFileDownload(res, this.$t('GetPaidMemshipReport'));
      //       })
      //       .catch((error) => {
      //          this.PrintLoading = false;
      //          this.showApiError(error);
      //       })
      //       .finally(() => {
      //          this.PrintLoading = false;
      //       });
      // }
   }
};
</script>

<style lang="scss" scoped>
@import '../styles.scss';
</style>
