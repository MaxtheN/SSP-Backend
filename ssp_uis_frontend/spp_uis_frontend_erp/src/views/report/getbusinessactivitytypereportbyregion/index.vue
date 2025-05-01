<template>
   <b-card no-body>
      <div class="mr-2 mx-2 mt-2">
         <b-row>
            <b-col sm="12" md="3">
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
            <b-col sm="12" md="3">
               <div>
                  <label for>{{ $t('Region') }}</label>
                  <v-select
                     :options="DistrictList"
                     :reduce="(item) => item.value"
                     :placeholder="$t('ChooseBelow')"
                     label="text"
                     v-model="filter.districtId"
                     class="w-100"
                  ></v-select>
               </div>
            </b-col>
            <b-col sm="12" md="3">
               <div>
                  <b-button
                     @click="Print"
                     v-b-tooltip.hover.top="$t('Print')"
                     :disabled="PrintLoading"
                     variant="primary"
                     class="mt-2"
                  >
                     <feather-icon icon="PrinterIcon"></feather-icon>
                     {{ $t('Print') }}
                  </b-button>
               </div>
            </b-col>
         </b-row>
         <div class="mt-3 mb-2 report-table">
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
                           <span v-show="filter.byRegion" style="font-weight: 900; font-size: 14px; color: black">{{
                              $t('region')
                           }}</span>
                           <span v-show="filter.byDistrict" style="font-weight: 900; font-size: 14px; color: black">{{
                              $t('Region')
                           }}</span>
                        </b-th>
                        <b-th colspan="5" rowspan="2" style="vertical-align: middle">
                           {{
                              $t('Tadbirkorlik subyektlariga ajratiladigan kreditlar uchun taqdim etilgan kafilliklar')
                           }}</b-th
                        >
                        <b-th colspan="15" style="vertical-align: middle"> {{ $t('Shundan') }}</b-th>
                     </b-tr>
                     <b-tr>
                        <b-th colspan="5" style="vertical-align: middle">{{
                           $t("51-100 ta ish o'rni yaratadigan")
                        }}</b-th>
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
                        <b-td class="table-b-table-default" :class="{ 'b-table-sticky-column': !isMobileDevice() }">
                           <span
                              v-show="filter.byRegion"
                              style="color: blue; cursor: pointer"
                              @click="SortRegion(item)"
                           >
                              {{ item.regionName }}
                           </span>
                           <span v-show="filter.byDistrict" style="cursor: pointer">{{ item.districtName }}</span>
                        </b-td>
                        <b-td class="text-right">{{ currency(item.businessActivity.userPrivilegeCount) }}</b-td>
                        <b-td class="text-right">{{ currency(item.businessActivity.createdVacanciesCount) }}</b-td>
                        <b-td class="text-right">{{ currency(item.businessActivity.financialHelpAmount) }}</b-td>
                        <b-td class="text-right">{{ currency(item.businessActivity.userPrivilegeCount) }}</b-td>
                        <b-td class="text-right">{{
                           currency(item.businessActivity.approvedFinancialHelpAmount)
                        }}</b-td>

                        <b-td class="text-right">{{ currency(item.businessType1.userPrivilegeCount) }}</b-td>
                        <b-td class="text-right">{{ currency(item.businessType1.createdVacanciesCount) }}</b-td>
                        <b-td class="text-right">{{ currency(item.businessType1.financialHelpAmount) }}</b-td>
                        <b-td class="text-right">{{ currency(item.businessType1.userPrivilegeCount) }}</b-td>
                        <b-td class="text-right">{{ currency(item.businessType1.approvedFinancialHelpAmount) }}</b-td>

                        <b-td class="text-right">{{ currency(item.businessType2.userPrivilegeCount) }}</b-td>
                        <b-td class="text-right">{{ currency(item.businessType2.createdVacanciesCount) }}</b-td>
                        <b-td class="text-right">{{ currency(item.businessType2.financialHelpAmount) }}</b-td>
                        <b-td class="text-right">{{ currency(item.businessType2.userPrivilegeCount) }}</b-td>
                        <b-td class="text-right">{{ currency(item.businessType2.approvedFinancialHelpAmount) }}</b-td>

                        <b-td class="text-right">{{ currency(item.businessType3.userPrivilegeCount) }}</b-td>
                        <b-td class="text-right">{{ currency(item.businessType3.createdVacanciesCount) }}</b-td>
                        <b-td class="text-right">{{ currency(item.businessType3.financialHelpAmount) }}</b-td>
                        <b-td class="text-right">{{ currency(item.businessType3.userPrivilegeCount) }}</b-td>
                        <b-td class="text-right">{{ currency(item.businessType3.approvedFinancialHelpAmount) }}</b-td>
                     </b-tr>
                  </b-tbody>
                  <b-tfoot v-if="items.length > 0">
                     <b-tr variant="secondary">
                        <b-td style="font-weight: 900; font-size: 14px; color: black" class="text-right" colspan="2">
                           {{ $t('Total') }}
                        </b-td>
                        <b-td style="font-weight: 900; font-size: 14px; color: black" class="text-right">
                           {{ currency(itemstotal.userPrivilegeCount) }}
                        </b-td>
                        <b-td style="font-weight: 900; font-size: 14px; color: black" class="text-right">
                           {{ currency(itemstotal.createdVacanciesCount) }}
                        </b-td>
                        <b-td style="font-weight: 900; font-size: 14px; color: black" class="text-right">
                           {{ currency(itemstotal.financialHelpAmount) }}
                        </b-td>
                        <b-td style="font-weight: 900; font-size: 14px; color: black" class="text-right">
                           {{ currency(itemstotal.userPrivilegeCount) }}
                        </b-td>
                        <b-td style="font-weight: 900; font-size: 14px; color: black" class="text-right">
                           {{ currency(itemstotal.financialHelpAmount) }}
                        </b-td>

                        <b-td style="font-weight: 900; font-size: 14px; color: black" class="text-right">
                           {{ currency(itemsbusinessType1total.userPrivilegeCount) }}
                        </b-td>
                        <b-td style="font-weight: 900; font-size: 14px; color: black" class="text-right">
                           {{ currency(itemsbusinessType1total.createdVacanciesCount) }}
                        </b-td>
                        <b-td style="font-weight: 900; font-size: 14px; color: black" class="text-right">
                           {{ currency(itemsbusinessType1total.financialHelpAmount) }}
                        </b-td>
                        <b-td style="font-weight: 900; font-size: 14px; color: black" class="text-right">
                           {{ currency(itemsbusinessType1total.userPrivilegeCount) }}
                        </b-td>
                        <b-td style="font-weight: 900; font-size: 14px; color: black" class="text-right">
                           {{ currency(itemsbusinessType1total.financialHelpAmount) }}
                        </b-td>

                        <b-td style="font-weight: 900; font-size: 14px; color: black" class="text-right">
                           {{ currency(itemsbusinessType2total.userPrivilegeCount) }}
                        </b-td>
                        <b-td style="font-weight: 900; font-size: 14px; color: black" class="text-right">
                           {{ currency(itemsbusinessType2total.createdVacanciesCount) }}
                        </b-td>
                        <b-td style="font-weight: 900; font-size: 14px; color: black" class="text-right">
                           {{ currency(itemsbusinessType2total.financialHelpAmount) }}
                        </b-td>
                        <b-td style="font-weight: 900; font-size: 14px; color: black" class="text-right">
                           {{ currency(itemsbusinessType2total.userPrivilegeCount) }}
                        </b-td>
                        <b-td style="font-weight: 900; font-size: 14px; color: black" class="text-right">
                           {{ currency(itemsbusinessType2total.financialHelpAmount) }}
                        </b-td>

                        <b-td style="font-weight: 900; font-size: 14px; color: black" class="text-right">
                           {{ currency(itemsbusinessType3total.userPrivilegeCount) }}
                        </b-td>
                        <b-td style="font-weight: 900; font-size: 14px; color: black" class="text-right">
                           {{ currency(itemsbusinessType3total.createdVacanciesCount) }}
                        </b-td>
                        <b-td style="font-weight: 900; font-size: 14px; color: black" class="text-right">
                           {{ currency(itemsbusinessType3total.financialHelpAmount) }}
                        </b-td>
                        <b-td style="font-weight: 900; font-size: 14px; color: black" class="text-right">
                           {{ currency(itemsbusinessType3total.userPrivilegeCount) }}
                        </b-td>
                        <b-td style="font-weight: 900; font-size: 14px; color: black" class="text-right">
                           {{ currency(itemsbusinessType3total.financialHelpAmount) }}
                        </b-td>
                     </b-tr>
                  </b-tfoot>
               </b-table-simple>
            </b-overlay>
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
   BOverlay
} from 'bootstrap-vue';
import ReportService from '@/services/report/report.service';

import RegionService from '@/services/info/region.service';
import DistrictService from '@/services/info/district.service';
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
            byRegion: true,
            byDistrict: false,
            districtName: '',
            regionId: null,
            regionName: '',
            districtId: null
         },
         isBusy: false,
         items: [],
         RegionList: [],
         DistrictList: [],
         itemstotal: {
            userPrivilegeCount: 0,
            createdVacanciesCount: 0,
            financialHelpAmount: 0
         },
         itemsbusinessType1total: {
            userPrivilegeCount: 0,
            createdVacanciesCount: 0,
            financialHelpAmount: 0
         },
         itemsbusinessType2total: {
            userPrivilegeCount: 0,
            createdVacanciesCount: 0,
            financialHelpAmount: 0
         },
         itemsbusinessType3total: {
            userPrivilegeCount: 0,
            createdVacanciesCount: 0,
            financialHelpAmount: 0
         }
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
      SortRegion(item) {
         (this.filter.byDistrict = true),
            (this.filter.byRegion = false),
            (this.filter.regionId = item.regionId),
            (this.filter.regionName = item.regionName),
            this.GetDistrict(item.regionId);
         this.Refresh();
      },
      ChangeRegion(id) {
         if (id) {
            this.filter.districtId = null;
            this.filter.byDistrict = true;
            this.filter.byRegion = false;
            this.filter.regionName = this.filter.regionId
               ? this.RegionList.filter((item) => item.value === this.filter.regionId)[0].text
               : '';
            this.Refresh();
            this.GetDistrict(id);
         } else {
            this.filter.districtId = null;
            this.filter.byDistrict = false;
            this.filter.byRegion = true;
            this.filter.regionName = '';
            this.Refresh();
         }
      },
      // ChangeDistrict(id) {
      //     if (id) {
      //         this.filter.byDistrict = false;
      //         this.filter.byRegion = false;
      //         this.filter.districtName = this.filter.districtId
      //             ? this.DistrictList.filter((item) => item.value === this.filter.districtId)[0].text
      //             : '';
      //         this.Refresh();
      //     } else {
      //         this.filter.byDistrict = true;
      //         this.filter.byRegion = false;
      //         this.filter.districtName = '';
      //         this.Refresh();
      //     }
      // },
      GetDistrict(id) {
         DistrictService.GetAsSelectList(id)
            .then((res) => {
               this.DistrictList = res.data;
            })
            .catch((error) => {
               this.showApiError(error);
            });
      },
      Print() {
         this.PrintLoading = true;
         ReportService.SaveAsExcelContractorFundByRegion(this.filter)
            .then((res) => {
               this.forceFileDownload(res, this.$t('GetBusinessActivityTypeReportByRegion'));
            })
            .catch((e) => {
               this.showApiError(e);
            })
            .finally(() => {
               this.PrintLoading = false;
            });
      },
      Refresh() {
         this.isBusy = true;
         this.itemstotal = {
            userPrivilegeCount: 0,
            createdVacanciesCount: 0,
            financialHelpAmount: 0
         };
         this.itemsbusinessType1total = {
            userPrivilegeCount: 0,
            createdVacanciesCount: 0,
            financialHelpAmount: 0
         };

         this.itemsbusinessType2total = {
            userPrivilegeCount: 0,
            createdVacanciesCount: 0,
            financialHelpAmount: 0
         };

         this.itemsbusinessType3total = {
            userPrivilegeCount: 0,
            createdVacanciesCount: 0,
            financialHelpAmount: 0
         };
         ReportService.GetBusinessActivityTypeReportByRegion(this.filter)
            .then((res) => {
               this.items = res.data;
               res.data.forEach((item) => {
                  this.itemstotal.userPrivilegeCount += item.businessActivity.userPrivilegeCount * 1;
                  this.itemstotal.createdVacanciesCount += item.businessActivity.createdVacanciesCount * 1;
                  this.itemstotal.financialHelpAmount += item.businessActivity.financialHelpAmount * 1;

                  this.itemsbusinessType1total.userPrivilegeCount += item.businessType1.userPrivilegeCount * 1;
                  this.itemsbusinessType1total.createdVacanciesCount += item.businessType1.createdVacanciesCount * 1;
                  this.itemsbusinessType1total.financialHelpAmount += item.businessType1.financialHelpAmount * 1;

                  this.itemsbusinessType2total.userPrivilegeCount += item.businessType2.userPrivilegeCount * 1;
                  this.itemsbusinessType2total.createdVacanciesCount += item.businessType2.createdVacanciesCount * 1;
                  this.itemsbusinessType2total.financialHelpAmount += item.businessType2.financialHelpAmount * 1;

                  this.itemsbusinessType3total.userPrivilegeCount += item.businessType3.userPrivilegeCount * 1;
                  this.itemsbusinessType3total.createdVacanciesCount += item.businessType3.createdVacanciesCount * 1;
                  this.itemsbusinessType3total.financialHelpAmount += item.businessType3.financialHelpAmount * 1;
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

<style lang="scss">
@import '../styles.scss';
</style>
