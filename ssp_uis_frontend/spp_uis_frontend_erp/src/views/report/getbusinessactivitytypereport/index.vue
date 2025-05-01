<template>
   <b-card no-body>
      <div class="mx-2 mt-2">
         <b-button
            @click="Print"
            v-b-tooltip.hover.top="$t('Print')"
            :disabled="PrintLoading"
            variant="primary"
            class="my-1"
         >
            <feather-icon icon="PrinterIcon"></feather-icon>
            {{ $t('Print') }}
         </b-button>
         <b-breadcrumb>
            <b-breadcrumb-item
               :active="filter.byRegion"
               @click="
                  () => {
                     filter.bankMfo = '';
                     filter.byBank = false;
                     filter.bankCodeId = null;
                     Refresh();
                  }
               "
            >
               <b>{{ $t('Bank') }}</b>
            </b-breadcrumb-item>
            <b-breadcrumb-item v-show="filter.bankCodeId" :active="filter.byBank">
               <b>{{ filter.bankMfo }}</b>
            </b-breadcrumb-item>
            <!-- <b-breadcrumb-item v-show="filter.byContractor" active>Baz</b-breadcrumb-item> -->
         </b-breadcrumb>
         <b-overlay :show="isBusy">
            <b-table-simple class="report-table" hover caption-top responsive striped border>
               <b-thead>
                  <b-tr>
                     <b-th rowspan="4" style="vertical-align: middle"> {{ $t('order') }}</b-th>
                     <b-th
                        rowspan="4"
                        style="vertical-align: middle"
                        :class="{ 'b-table-sticky-column': !isMobileDevice() }"
                     >
                        {{ $t('BankName') }}</b-th
                     >
                     <b-th colspan="5" rowspan="2" style="vertical-align: middle">
                        {{
                           $t('Tadbirkorlik subyektlariga ajratiladigan kreditlar uchun taqdim etilgan kafilliklar')
                        }}</b-th
                     >
                     <b-th colspan="15" style="vertical-align: middle"> {{ $t('Shundan') }}</b-th>
                  </b-tr>
                  <b-tr>
                     <b-th colspan="5" style="vertical-align: middle">{{ $t("51-100 ta ish o'rni yaratadigan") }}</b-th>
                     <b-th colspan="5" style="vertical-align: middle">{{
                        $t("101-200 ta ish o'rni yaratadigan")
                     }}</b-th>
                     <b-th colspan="5" style="vertical-align: middle">{{
                        $t("200 dan ortiq ish o'rni yaratadigan")
                     }}</b-th>
                  </b-tr>
                  <b-tr>
                     <b-th rowspan="2" style="vertical-align: middle">{{
                        $t('Imtiyozdan foydalangan subyekt soni')
                     }}</b-th>
                     <b-th rowspan="2" style="vertical-align: middle">{{ $t("Yaratiladigan ish o'rni soni") }}</b-th>
                     <b-th rowspan="2" style="vertical-align: middle">{{ $t('Kafilliga talab') }}</b-th>
                     <b-th colspan="2" style="vertical-align: middle">{{ $t('Shundan taqdim etildi') }}</b-th>
                     <b-th rowspan="2" style="vertical-align: middle">{{
                        $t('Imtiyozdan foydalangan subyekt soni')
                     }}</b-th>
                     <b-th rowspan="2" style="vertical-align: middle">{{ $t("Yaratiladigan ish o'rni soni") }}</b-th>
                     <b-th rowspan="2" style="vertical-align: middle">{{ $t('Kafilliga talab') }}</b-th>
                     <b-th colspan="2" style="vertical-align: middle">{{ $t('Shundan taqdim etildi') }}</b-th>
                     <b-th rowspan="2" style="vertical-align: middle">{{
                        $t('Imtiyozdan foydalangan subyekt soni')
                     }}</b-th>
                     <b-th rowspan="2" style="vertical-align: middle">{{ $t("Yaratiladigan ish o'rni soni") }}</b-th>
                     <b-th rowspan="2" style="vertical-align: middle">{{ $t('Kafilliga talab') }}</b-th>
                     <b-th colspan="2" style="vertical-align: middle">{{ $t('Shundan taqdim etildi') }}</b-th>
                     <b-th rowspan="2" style="vertical-align: middle">{{
                        $t('Imtiyozdan foydalangan subyekt soni')
                     }}</b-th>
                     <b-th rowspan="2" style="vertical-align: middle">{{ $t("Yaratiladigan ish o'rni soni") }}</b-th>
                     <b-th rowspan="2" style="vertical-align: middle">{{ $t('Kafilliga talab') }}</b-th>
                     <b-th colspan="2" style="vertical-align: middle">{{ $t('Shundan taqdim etildi') }}</b-th>
                  </b-tr>
                  <b-tr>
                     <b-th style="vertical-align: middle">{{ $t('Soni') }}</b-th>
                     <b-th style="vertical-align: middle">{{ $t('Summasi') }}</b-th>
                     <b-th style="vertical-align: middle">{{ $t('Soni') }}</b-th>
                     <b-th style="vertical-align: middle">{{ $t('Summasi') }}</b-th>
                     <b-th style="vertical-align: middle">{{ $t('Soni') }}</b-th>
                     <b-th style="vertical-align: middle">{{ $t('Summasi') }}</b-th>
                     <b-th style="vertical-align: middle">{{ $t('Soni') }}</b-th>
                     <b-th style="vertical-align: middle">{{ $t('Summasi') }}</b-th>
                  </b-tr>
               </b-thead>
               <b-tbody v-if="items.length > 0">
                  <b-tr v-for="(item, index) in items" :key="index">
                     <b-td>{{ index + 1 }}</b-td>
                     <b-td
                        v-if="!filter.byBank"
                        style="color: blue; cursor: pointer"
                        class="table-b-table-default"
                        :class="{ 'b-table-sticky-column': !isMobileDevice() }"
                        @click="SortBank(item)"
                        >{{ item?.bankName }}</b-td
                     >
                     <b-td
                        v-if="filter.byBank"
                        class="table-b-table-default"
                        :class="{ 'b-table-sticky-column': !isMobileDevice() }"
                        >{{ item?.bankName }}</b-td
                     >
                     <b-td class="text-right">{{ currency(item.businessActivity.userPrivilegeCount) }}</b-td>
                     <b-td class="text-right">{{ currency(item.businessActivity.createdVacanciesCount) }}</b-td>
                     <b-td class="text-right">{{ currency(item.businessActivity.financialHelpAmount) }}</b-td>
                     <b-td class="text-right">{{ currency(item.businessActivity.userPrivilegeCount) }}</b-td>
                     <b-td class="text-right">{{ currency(item.businessActivity.financialHelpAmount) }}</b-td>

                     <b-td class="text-right">{{ currency(item.businessType1.userPrivilegeCount) }}</b-td>
                     <b-td class="text-right">{{ currency(item.businessType1.createdVacanciesCount) }}</b-td>
                     <b-td class="text-right">{{ currency(item.businessType1.financialHelpAmount) }}</b-td>
                     <b-td class="text-right">{{ currency(item.businessType1.userPrivilegeCount) }}</b-td>
                     <b-td class="text-right">{{ currency(item.businessType1.financialHelpAmount) }}</b-td>

                     <b-td class="text-right">{{ currency(item.businessType2.userPrivilegeCount) }}</b-td>
                     <b-td class="text-right">{{ currency(item.businessType2.createdVacanciesCount) }}</b-td>
                     <b-td class="text-right">{{ currency(item.businessType2.financialHelpAmount) }}</b-td>
                     <b-td class="text-right">{{ currency(item.businessType2.userPrivilegeCount) }}</b-td>
                     <b-td class="text-right">{{ currency(item.businessType2.financialHelpAmount) }}</b-td>

                     <b-td class="text-right">{{ currency(item.businessType3.userPrivilegeCount) }}</b-td>
                     <b-td class="text-right">{{ currency(item.businessType3.createdVacanciesCount) }}</b-td>
                     <b-td class="text-right">{{ currency(item.businessType3.financialHelpAmount) }}</b-td>
                     <b-td class="text-right">{{ currency(item.businessType3.userPrivilegeCount) }}</b-td>
                     <b-td class="text-right">{{ currency(item.businessType3.financialHelpAmount) }}</b-td>
                  </b-tr>
               </b-tbody>
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
   BOverlay
} from 'bootstrap-vue';
import ReportService from '@/services/report/report.service';
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
      BOverlay
   },
   directives: {
      'b-tooltip': VBTooltip,
      'b-modal': VBModal
   },
   data() {
      return {
         PrintLoading: false,
         filter: {
            bankMfo: '',
            byBank: false,
            bankCodeId: null
         },
         isBusy: false,
         items: []
      };
   },
   created() {
      this.Refresh();
   },

   methods: {
      Print() {
         this.PrintLoading = true;
         ReportService.SaveAsExcelContractorFundByBank(this.filter)
            .then((res) => {
               this.forceFileDownload(res, this.$t('GetBusinessActivityTypeReport'));
            })
            .catch((e) => {
               this.showApiError(e);
            })
            .finally(() => {
               this.PrintLoading = false;
            });
      },

      SortBank(item) {
         this.filter.bankMfo = item.bankName;
         this.filter.byBank = true;
         this.filter.bankCodeId = item.bankCodeId;
         this.Refresh();
      },

      Refresh() {
         this.isBusy = true;
         ReportService.GetBusinessActivityTypeReport(this.filter)
            .then((res) => {
               this.items = res.data;
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

<style lang="scss">
@import '../styles.scss';
</style>
