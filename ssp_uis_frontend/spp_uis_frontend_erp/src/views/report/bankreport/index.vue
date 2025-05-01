<template>
   <b-card>
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
         <b-col cols="12" md="2">
            <form-picker v-model="filter.year" type="year" @input="Refresh" format="YYYY" :label="$t('docyear')" />
         </b-col>
         <b-col cols="12" md="3">
            <label for>{{ $t('inn') }}</label>
            <b-input-group class="text-right">
               <b-form-input v-model="filter.contractorInn" :placeholder="$t('search')" />
               <b-input-group-append>
                  <b-button @click="Refresh" variant="primary">
                     <feather-icon icon="SearchIcon" />
                  </b-button>
               </b-input-group-append>
            </b-input-group>
         </b-col>
         <b-col cols="12" md="2">
            <b-button @click="Refresh" :disabled="isBusy" variant="primary" class="mt-2">
               <feather-icon icon="SearchIcon" />
               {{ $t('Refresh') }}
            </b-button>
         </b-col>
         <b-col sm="12" md="2" class="">
            <b-button @click="Print" :disabled="PrintLoading" variant="primary" class="mt-2">
               <feather-icon icon="PrinterIcon"></feather-icon>
               {{ $t('Print') }}
            </b-button>
         </b-col>
      </b-row>
      <b-tabs class="nav-tabs nav-justified1 mt-2 mb-0">
         <b-tab
            :title="$t('all')"
            @click="
               () => {
                  paged = false;
                  filter.byContractor = false;
                  filter.byBank = false;
                  filter.byRegion = true;
                  filter.contractorInn = null;

                  Refresh();
               }
            "
         >
         </b-tab>
         <b-tab
            :title="$t('Bank')"
            :active="filter.byBank"
            @click="
               () => {
                  paged = true;
                  filter.byBank = true;
                  filter.byContractor = false;
                  filter.bank = '';
                  filter.bankId = null;
                  filter.contractorInn = null;
                  Refresh();
               }
            "
         >
         </b-tab>
         <b-tab
            :title="$t('contractor')"
            @click="
               () => {
                  paged = true;
                  filter.byContractor = true;
                  filter.byDistrict = false;
                  filter.byBankCode = false;
                  filter.byRegion = true;
                  filter.contractorInn = null;
                  filter.byBankCode = false;
                  filter.byBank = false;

                  Refresh();
               }
            "
         >
         </b-tab>
      </b-tabs>

      <!-- table -->
      <b-overlay :show="isBusy">
         <b-table-simple
            class="report-table position-relative"
            hover
            caption-top
            responsive
            striped
            border
            style="max-height: 650px"
         >
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

                  <b-th colspan="10" rowspan="2">{{
                     $t('Дастур доирасида кредит олиш учун электрон шаклда ариза тақдим этган субъектлар сони')
                  }}</b-th>
                  <b-th colspan="30">{{ $t('shundan') }}</b-th>
               </b-tr>
               <b-tr>
                  <b-th colspan="10">{{ $t('51-100 тагача иш ўрни яратадиган') }}</b-th>
                  <b-th colspan="10">{{ $t('101-200 тагача иш ўрни яратадиган') }}</b-th>
                  <b-th colspan="10">{{ $t('200 дан ортиқ иш ўрни яратадиган') }}</b-th>
               </b-tr>
               <b-tr>
                  <template v-for="h in 4">
                     <b-th colspan="2" :key="h + 'col1'">{{ $t('Тушган аризалар') }}</b-th>
                     <b-th style="color: #daa521" colspan="2" :key="h + 'col2'">{{
                        $t('Tadbirkor tomonidan bekor qilingan arizalar')
                     }}</b-th>
                     <b-th style="color: red" colspan="2" :key="h + 'col3'">{{ $t('Рад этилган аризалар') }}</b-th>
                     <b-th style="color: green" colspan="2" :key="h + 'col4'">{{ $t('Қабул қилинган аризалар') }}</b-th>
                     <b-th colspan="2" :key="h + 'col5'">{{ $t('Кредит ажратилган аризалар') }}</b-th>
                  </template>
               </b-tr>
               <b-tr>
                  <template v-for="i in 20">
                     <b-th :key="i + 'quantity'">{{ $t('quantity') }}</b-th>
                     <b-th :key="i + 'amount'">{{ $t('amount') }}</b-th>
                  </template>
               </b-tr>
            </b-thead>

            <b-tbody v-if="items.length > 0">
               <b-tr v-for="(item, idx) in items" :key="idx + 'abc'">
                  <b-td>{{ idx + 1 }}</b-td>
                  <b-td
                     class="table-b-table-default text-wrap"
                     :class="{ 'b-table-sticky-column': !isMobileDevice() }"
                     style="max-width: 500px; min-width: 400px"
                  >
                     <span v-if="filter.byContractor">
                        <span style="color: blue; cursor: pointer" @click="goToBussnes(item.contractorInn)">{{
                           item.contractorInn
                        }}</span>
                        -
                        {{ item.contractor }}
                     </span>
                     <span v-else-if="filter.byBank" @click="handleBank(item)" style="color: blue; cursor: pointer">
                        {{ item.bankMfo }} -{{ item.bankName }}
                     </span>
                     <span
                        v-else-if="filter.byDistrict"
                        style="color: blue; cursor: pointer"
                        @click="handleDistrict(item)"
                     >
                        {{ item.districtName }}
                     </span>
                     <span v-else-if="filter.byRegion" style="color: blue; cursor: pointer" @click="handleRegion(item)">
                        {{ item.regionName }}
                     </span>
                  </b-td>

                  <template v-for="itemkey in itemKeys">
                     <template v-for="itemInnerKey in itemInnerKeys">
                        <b-td
                           class="text-right"
                           :key="itemkey + 'a' + itemInnerKey"
                           :style="{
                              color: colColor(itemInnerKey)
                           }"
                        >
                           {{ item[itemkey] ? currency(item[itemkey][itemInnerKey]) : 0 }}
                        </b-td>
                     </template>
                  </template>
               </b-tr>
            </b-tbody>
            <b-tfoot v-if="items.length > 0" class="position-sticky" style="bottom: 0; z-index: 3">
               <b-tr variant="secondary">
                  <b-td class="text-right" :class="{ 'b-table-sticky-column': !isMobileDevice() }"></b-td>
                  <b-td class="text-tright" :class="{ 'b-table-sticky-column': !isMobileDevice() }">{{
                     $t('Total')
                  }}</b-td>

                  <template v-for="itemkey in itemKeys">
                     <template v-for="itemInnerKey in itemInnerKeys">
                        <b-td
                           :style="{
                              color: colColor(itemInnerKey)
                           }"
                           class="text-right"
                           :key="itemkey + 'a' + itemInnerKey"
                        >
                           {{ currency(ItemsTotals[itemkey][itemInnerKey]) }}
                        </b-td>
                     </template>
                  </template>
               </b-tr>
            </b-tfoot>
         </b-table-simple>

         <template #overlay>
            <div class="text-center text-primary my-2">
               <b-spinner scale="2" class="align-middle mr-2"></b-spinner>
               <strong>{{ $t('Loading') }}...</strong>
            </div>
         </template>
         <div v-if="paged" class="mx-1 mb-1">
            <b-row>
               <b-col
                  cols="12"
                  sm="6"
                  class="d-flex align-items-center justify-content-center justify-content-sm-start"
               >
                  <span class="text-muted">
                     {{ $t('Showing') }} {{ firstNumber }} {{ $t('to') }} {{ lastNumber }} {{ $t('of') }}
                     {{ filter.total }} {{ $t('entries') }}
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
      </b-overlay>
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
         paged: false,
         RegionList: [],
         DistrictList: [],
         PrintLoading: false,
         filter: {
            year: new Date().getFullYear(),
            bankMfo: null,
            regionId: null,
            region: '',
            districtId: null,
            district: '',
            bankId: null,
            bank: null,
            byRegion: true,
            byDistrict: false,
            byBank: false,
            byContractor: false,
            contractorInn: '',
            page: 1,
            pageSize: 20,
            perPageOptions: [10, 20, 50, 100],
            total: 0
         },

         isBusy: false,
         itemKeys: ['application', 'contractType1', 'contractType2', 'contractType3'],
         itemInnerKeys: [
            'submittedCount',
            'submittedSum',
            'canceledCount',
            'canceledSum',
            'rejectedCount',
            'rejectedSum',
            'approvedCount',
            'approvedSum',
            'issuanceCount',
            'issuanceSum'
         ]
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
   computed: {
      ItemsTotals() {
         const DefAllObj = {
            application: {},
            contractType1: {},
            contractType2: {},
            contractType3: {}
         };

         const allObj = this.items.reduce(
            (acc, item) => {
               this.itemKeys.forEach((objKey) => {
                  this.itemInnerKeys.forEach((key) => {
                     if (item[objKey] && Object.prototype.hasOwnProperty.call(item[objKey], key)) {
                        acc[objKey][key] = Number(acc[objKey][key] || 0) + item[objKey][key];
                     }
                  });
               });

               return acc;
            },
            { ...DefAllObj }
         );

         return allObj;
      },
      colColor() {
         return (c) => {
            if (['canceledCount', 'canceledSum'].includes(c)) {
               return '#daa521';
            } else if (['rejectedCount', 'rejectedSum'].includes(c)) {
               return 'red';
            } else if (['approvedCount', 'approvedSum'].includes(c)) {
               return 'green';
            }
         };
      },
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
      ChangeRegion(id) {
         this.filter.districtId = null;
         this.filter.district = null;

         if (id) {
            this.GetDistrict(id);
            this.filter.byDistrict = true;
            this.filter.byRegion = false;
            this.filter.byContractor = false;
         } else {
            this.filter.byDistrict = false;
            this.filter.byRegion = true;
            this.filter.byContractor = false;
         }
         this.Refresh();
      },
      ChangeDistrict(id) {
         if (id) {
            // console.log(id, 'sss');
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
         this.ChangeRegion(item.regionId);
         this.Refresh();
      },
      handleDistrict(item) {
         this.filter.districtId = item.districtId;
         this.filter.district = item.districtName;
         this.filter.byContractor = false;
         this.filter.byBank = true;
         this.filter.byDistrict = false;
         this.filter.byRegion = false;
         // this.ChangeDistrict(item.districtId);
         this.Refresh();
      },
      handleBank(item) {
         this.filter.bankMfo = item.bankMfo;
         this.filter.bank = item.bankName;
         this.filter.byContractor = true;
         this.filter.byBank = true;
         this.filter.byDistrict = false;
         this.filter.byRegion = false;
         this.Refresh();
      },
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
         ReportService.SaveBankCreditReportAsExcel(this.filter)
            .then((res) => {
               this.forceFileDownload(res, this.$t('BankReport'));
            })
            .catch((err) => {
               this.showApiError(err);
            })
            .finally(() => {
               this.PrintLoading = false;
            });
      },
      Refresh(a) {
         this.isBusy = true;
         this.items = [];

         if (this.paged) {
            ReportService.GetPagedBankCreditReport(this.filter)
               .then((res) => {
                  this.items = res.data.rows;
                  this.filter.total = res.data.total;
               })
               .catch((error) => {
                  this.showApiError(error);
               })
               .finally(() => {
                  this.isBusy = false;
               });
         } else {
            ReportService.GetBankCreditReport(this.filter)
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
   }
};
</script>

<style lang="scss">
@import '../styles.scss';
</style>
