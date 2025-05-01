<template>
   <b-card no-body>
      <div class="m-2">
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
                     @input="Refresh"
                     class="w-100"
                  ></v-select>
               </div>
            </b-col>
            <b-col sm="12" md="2">
               <b-button @click="Print" :disabled="PrintLoading" variant="primary" class="ml-1 mt-2">
                  <feather-icon icon="PrinterIcon"></feather-icon>
                  {{ $t('Print') }}
               </b-button>
            </b-col>
            <b-col sm="12" md="2">
               <b-button @click="Refresh" :disabled="isBusy" variant="primary" class="mt-2">
                  <feather-icon icon="SearchIcon" />
                  {{ $t('Refresh') }}
               </b-button>
            </b-col>
         </b-row>
         <b-row align-h="between">
            <b-col sm="12" md="8" class="mt-2">
               <b-button-group @click="Refresh" size="sm" class="mr-2">
                  <b-button
                     @click="filter.contractTypeId = null"
                     :variant="null == filter.contractTypeId ? 'primary' : 'outline-primary'"
                     >{{ $t('all') }}</b-button
                  >
                  <b-button
                     v-for="type in PrtnContractTypeList"
                     :key="type.value"
                     @click="
                        () => {
                           filter.contractTypeId = type.value;
                           filter.byContactType = true;
                        }
                     "
                     :variant="type.value == filter.contractTypeId ? 'primary' : 'outline-primary'"
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
         </b-row>
      </div>
      <div class="mx-2">
         <b-overlay :show="isBusy">
            <b-table-simple class="report-table" hover caption-top responsive striped border>
               <b-thead>
                  <b-tr>
                     <b-th
                        rowspan="4"
                        style="vertical-align: middle"
                        :class="{ 'b-table-sticky-column': !isMobileDevice() }"
                        >{{ $t('order') }}</b-th
                     >
                     <b-th
                        rowspan="4"
                        style="vertical-align: middle"
                        :class="{ 'b-table-sticky-column': !isMobileDevice() }"
                     >
                        <span v-if="filter.byContractor">{{ $t('contractor') }}</span>
                        <span v-else-if="filter.byBank">{{ $t('bankName') }}</span>
                        <span v-else-if="filter.byDistrict">{{ $t('Region') }}</span>
                        <span v-else-if="filter.byRegion">{{ $t('Oblast') }}</span>
                     </b-th>

                     <b-th colspan="5">{{
                        $t('Дастур доирасида кредит олиш учун электрон шаклда ариза тақдим этган субъектлар сони')
                     }}</b-th>
                  </b-tr>

                  <b-tr>
                     <b-th>{{ $t('Субъект сони') }}</b-th>
                     <b-th>{{ $t('Кредитга талаб (сўм)') }}</b-th>
                     <b-th>{{ $t('Яратила-диган иш ўрни сони') }}</b-th>
                     <b-th>{{ $t('Кредит ажратилган аризалар') }}</b-th>
                     <b-th>{{ $t('Кредит ажратилган суммаси') }}</b-th>
                  </b-tr>
               </b-thead>

               <b-tbody v-if="items.length > 0">
                  <b-tr v-for="(item, idx) in items" :key="idx + 'abc' + 1">
                     <b-td>{{ idx + 1 }}</b-td>
                     <b-td class="table-b-table-default" :class="{ 'b-table-sticky-column': !isMobileDevice() }">
                        <span v-if="filter.byContractor">
                           <span
                              style="color: blue; cursor: pointer; white-space: wrap"
                              @click="goToBussnes(item.contractorInn)"
                              >{{ item.contractorInn }}</span
                           >
                           - <span style="white-space: wrap">{{ item.contractorFullName }}</span>
                        </span>
                        <span
                           v-else-if="filter.byDistrict"
                           style="color: blue; cursor: pointer"
                           @click="handleDistrict(item)"
                        >
                           {{ item.district }}
                        </span>
                        <span
                           v-else-if="filter.byRegion"
                           style="color: blue; cursor: pointer"
                           @click="handleRegion(item)"
                        >
                           {{ item.region }}
                        </span>
                     </b-td>

                     <!-- Application -->
                     <b-td class="text-right">
                        {{ currency(item.approvedCount) }}
                     </b-td>
                     <b-td class="text-right">
                        {{ currency(item.approvedSum) }}
                     </b-td>

                     <b-td class="text-right"> {{ currency(item.approvedNewVacanciesCount) }} </b-td>
                     <b-td class="text-right">
                        {{ currency(item.issuanceCount) }}
                     </b-td>
                     <b-td class="text-right">
                        {{ currency(item.issuanceSum) }}
                     </b-td>
                  </b-tr>
               </b-tbody>
               <b-tfoot v-if="items.length > 0">
                  <b-tr variant="secondary">
                     <b-td class="text-right" :class="{ 'b-table-sticky-column': !isMobileDevice() }"></b-td>
                     <b-td class="text-right" :class="{ 'b-table-sticky-column': !isMobileDevice() }">{{
                        $t('Total')
                     }}</b-td>
                     <!-- Application -->
                     <b-td class="text-right">
                        {{ currency(totals.approvedCount) }}
                     </b-td>
                     <b-td class="text-right">
                        {{ currency(totals.approvedSum) }}
                     </b-td>

                     <b-td class="text-right">
                        {{ currency(totals.employeeCount) }}
                     </b-td>
                     <b-td class="text-right">
                        {{ currency(totals.issuanceCount) }}
                     </b-td>
                     <b-td class="text-right">
                        {{ currency(totals.issuanceSum) }}
                     </b-td>
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
   BOverlay
} from 'bootstrap-vue';
import ReportService from '@/services/report/report.service';
import PrtnContractTypeService from '@/services/info/prtncontracttype.service';

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
         items: [],
         RegionList: [],
         DistrictList: [],
         PrtnContractTypeList: [],

         PrintLoading: false,
         filter: {
            regionId: null,
            region: '',
            district: '',
            bank: '',
            byRegion: true,
            districtId: null,
            byDistrict: false,
            contractorId: null,
            contractorInn: null,
            byContractor: false,
            languageId: null,
            contractTypeId: null,
            byContactType: false,
            hasCertificate: true
         },
         isBusy: false,
         totals: {
            approvedCount: 0,
            approvedSum: 0,
            issuanceCount: 0,
            issuanceSum: 0,
            employeeCount: 0
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
      PrtnContractTypeService.GetAsSelectList()
         .then((res) => {
            this.PrtnContractTypeList = res.data;
         })
         .catch((error) => {
            this.showApiError(error);
         });
   },

   methods: {
      ChangeRegion(id) {
         this.filter.districtId = null;
         this.filter.district = null;
         if (id) {
            this.GetDistrict(id);
         }
         this.Refresh();
      },
      goToBussnes(inn) {
         this.$router.push({ name: 'BusinessmanCard', query: { inn: inn } });
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
      handleRegion(item) {
         this.filter.regionId = item.regionId;
         this.filter.region = item.regionName;
         this.filter.byContractor = false;
         this.filter.byBank = false;
         this.filter.byDistrict = true;
         this.filter.byRegion = false;
         this.Refresh();
      },
      handleDistrict(item) {
         this.filter.districtId = item.districtId;
         this.filter.district = item.districtName;
         this.filter.byContractor = true;
         this.filter.byBank = true;
         this.filter.byDistrict = false;
         this.filter.byRegion = false;
         this.Refresh();
      },
      //   handleBank(item) {
      //      this.filter.bankMfo = item.bankMfo;
      //      this.filter.bank = item.bankName;
      //      this.filter.byContractor = true;
      //      this.filter.byBank = false;
      //      this.filter.byDistrict = false;
      //      this.filter.byRegion = false;
      //      this.Refresh();
      //   },
      handleBread(type) {
         if (type == 'region') {
            this.filter.byBank = false;
            this.filter.byDistrict = false;
            this.filter.byRegion = true;
            this.filter.regionId = null;
            this.filter.region = null;
            this.filter.districtId = null;
            this.filter.district = null;
            this.filter.byContractor = false;
         } else if (type == 'district') {
            this.filter.byBank = false;
            this.filter.byDistrict = true;
            this.filter.byRegion = false;
            this.filter.districtId = null;
            this.filter.district = null;
            this.filter.byContractor = false;
         } else if (type == 'bank') {
            this.filter.byBank = false;
            this.filter.byDistrict = true;
            this.filter.byRegion = false;
            this.filter.districtId = null;
            this.filter.district = null;
            this.filter.byContractor = true;
         }
         this.Refresh();
      },
      Print() {
         this.PrintLoading = true;
         ReportService.SaveAsExcelBankCreditePrivilegeReport(this.filter)
            .then((res) => {
               this.forceFileDownload(res, this.$t('prtnapplicationandcontractinfo'));
            })
            .finally(() => {
               this.PrintLoading = false;
            });
      },
      Refresh() {
         this.isBusy = true;
         this.items = [];
         this.totals = {
            approvedCount: 0,
            approvedSum: 0,
            issuanceCount: 0,
            issuanceSum: 0,
            employeeCount: 0
         };
         ReportService.GetBankCreditApplicationReportByRegionAndDistrict(this.filter)
            .then((res) => {
               this.items = res.data;
               res.data.forEach((item) => {
                  this.totals.approvedCount += item.approvedCount;
                  this.totals.approvedSum += item.approvedSum;
                  this.totals.employeeCount = item.approvedNewVacanciesCount;
                  this.totals.issuanceCount += item.issuanceCount;
                  this.totals.issuanceSum += item.issuanceSum;
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
