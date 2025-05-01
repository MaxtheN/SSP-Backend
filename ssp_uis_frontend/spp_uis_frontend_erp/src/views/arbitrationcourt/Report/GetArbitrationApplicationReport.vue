<template>
   <b-card>
      <b-overlay :show="isBusy">
         <b-row>
            <b-col sm="12" md="3">
               <div>
                  <label for>{{ $t('region') }}</label>
                  <v-select
                     :options="RegionList"
                     :reduce="(item) => item.value"
                     :placeholder="$t('ChooseBelow')"
                     label="text"
                     @input="ChangeRegion"
                     class="w-100"
                     v-model="filter.regionId"
                  >
                  </v-select>
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
            <!-- <b-col sm="12" md="2" class="mt-2">
               <b-button @click="Print" :disabled="PrintLoading" variant="primary" class="ml-1">
                  <feather-icon icon="PrinterIcon"></feather-icon>
               </b-button>
            </b-col> -->
         </b-row>

         <b-row align-h="between">
            <b-col sm="12" md="8">
               <b-breadcrumb class="">
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
               </b-breadcrumb>
            </b-col>
         </b-row>

         <b-table-simple class="report-table" hover small caption-top responsive border>
            <b-thead>
               <b-tr>
                  <b-th rowspan="3">№</b-th>
                  <b-th rowspan="3">
                     <span v-show="filter.byRegion">
                        {{ $t('region') }}
                     </span>
                     <span v-show="filter.byDistrict">
                        {{ $t('district') }}
                     </span>
                     <span v-show="filter.byContractor">
                        {{ $t('contractor') }}
                     </span>
                  </b-th>
                  <b-th rowspan="3">{{ $t('Киритилган даъво аризалар сони') }}</b-th>
                  <b-th colspan="3" rowspan="2">{{ $t('Даъво ариза суммаси') }}</b-th>
                  <b-th colspan="3" rowspan="2">{{ $t('Ундирилган Ҳакамлик йиғими') }}</b-th>
                  <b-th colspan="3" rowspan="2">{{ $t('Кечиктирилган Ҳакамлик йиғими') }}</b-th>
                  <b-th colspan="3" rowspan="2">{{ $t('Хакамлик суди қарорлар Сони') }}</b-th>
                  <b-th colspan="9">{{ $t('Суммаси') }}</b-th>
               </b-tr>
               <b-tr>
                  <b-th colspan="3">{{ $t('Қаноатлантирилган') }}</b-th>
                  <b-th colspan="3">{{ $t('Қисман қаноатлантирилган') }}</b-th>
                  <b-th colspan="3">{{ $t('rejects') }}</b-th>
               </b-tr>
               <b-tr>
                  <b-th>{{ $t('Сўм') }}</b-th>
                  <b-th>{{ $t('АҚШ.Долл') }}</b-th>
                  <b-th>{{ $t('Евро') }}</b-th>
                  <b-th>{{ $t('Сўм') }}</b-th>
                  <b-th>{{ $t('АҚШ.Долл') }}</b-th>
                  <b-th>{{ $t('Евро') }}</b-th>
                  <b-th>{{ $t('Сўм') }}</b-th>
                  <b-th>{{ $t('АҚШ.Долл') }}</b-th>
                  <b-th>{{ $t('Евро') }}</b-th>
                  <b-th>{{ $t('Қаноатлантирилган') }}</b-th>
                  <b-th>{{ $t('Қисман қаноатлантирилган') }}</b-th>
                  <b-th>{{ $t('rejects') }}</b-th>
                  <b-th>{{ $t('Сўм') }}</b-th>
                  <b-th>{{ $t('АҚШ.Долл') }}</b-th>
                  <b-th>{{ $t('Евро') }}</b-th>
                  <b-th>{{ $t('Сўм') }}</b-th>
                  <b-th>{{ $t('АҚШ.Долл') }}</b-th>
                  <b-th>{{ $t('Евро') }}</b-th>
                  <b-th>{{ $t('Сўм') }}</b-th>
                  <b-th>{{ $t('АҚШ.Долл') }}</b-th>
                  <b-th>{{ $t('Евро') }}</b-th>
               </b-tr>
            </b-thead>
            <b-tbody>
               <b-tr v-for="(item, index) in tableData" :key="index">
                  <b-td>{{ index + 1 }}</b-td>
                  <b-td>
                     <span v-show="filter.byRegion" @click="SortRegion(item)" style="color: blue; cursor: pointer">
                        {{ item.region }}
                     </span>
                     <span v-show="filter.byDistrict" @click="SortDistrict(item)" style="color: blue; cursor: pointer">
                        {{ item.district }}
                     </span>
                     <span v-show="filter.byContractor" @click="SortDistrict(item)">
                        {{ item.contractor }}
                     </span>
                  </b-td>
                  <b-td class="text-right">{{ currency(item.totalArbitrationApplicationCount) }}</b-td>
                  <b-td class="text-right">{{ currency(item.totalArbitrationApplicationAmount.item1) }}</b-td>
                  <b-td class="text-right">{{ currency(item.totalArbitrationApplicationAmount.item2) }}</b-td>
                  <b-td class="text-right">{{ currency(item.totalArbitrationApplicationAmount.item3) }}</b-td>
                  <b-td class="text-right">{{ currency(item.totalArbitrationPaidApplicationAmount.item1) }}</b-td>
                  <b-td class="text-right">{{ currency(item.totalArbitrationPaidApplicationAmount.item2) }}</b-td>
                  <b-td class="text-right">{{ currency(item.totalArbitrationPaidApplicationAmount.item3) }}</b-td>
                  <b-td class="text-right">{{ currency(item.totalArbitrationLatePaidApplicationAmount.item1) }}</b-td>
                  <b-td class="text-right">{{ currency(item.totalArbitrationLatePaidApplicationAmount.item2) }}</b-td>
                  <b-td class="text-right">{{ currency(item.totalArbitrationLatePaidApplicationAmount.item3) }}</b-td>
                  <b-td class="text-right">{{ currency(item.totalArbitrationAcceptedCount) }}</b-td>
                  <b-td class="text-right">{{ currency(item.totalArbitrationPartiallyAcceptedCount) }}</b-td>
                  <b-td class="text-right">{{ currency(item.totalArbitrationCanceledCount) }}</b-td>
                  <b-td class="text-right">{{ currency(item.totalArbitrationAcceptedAmount.item1) }}</b-td>
                  <b-td class="text-right">{{ currency(item.totalArbitrationAcceptedAmount.item2) }}</b-td>
                  <b-td class="text-right">{{ currency(item.totalArbitrationAcceptedAmount.item3) }}</b-td>
                  <b-td class="text-right">{{ currency(item.totalArbitrationPartiallyAcceptedAmount.item1) }}</b-td>
                  <b-td class="text-right">{{ currency(item.totalArbitrationPartiallyAcceptedAmount.item2) }}</b-td>
                  <b-td class="text-right">{{ currency(item.totalArbitrationPartiallyAcceptedAmount.item3) }}</b-td>
                  <b-td class="text-right">{{ currency(item.totalArbitrationCanceledAmount.item1) }}</b-td>
                  <b-td class="text-right">{{ currency(item.totalArbitrationCanceledAmount.item2) }}</b-td>
                  <b-td class="text-right">{{ currency(item.totalArbitrationCanceledAmount.item3) }}</b-td>
               </b-tr>
            </b-tbody>
            <b-tfoot v-if="tableData.length > 0">
               <b-tr>
                  <b-th colspan="2">
                     <span class="ml-3">{{ $t('Total') }}</span>
                  </b-th>
                  <b-th class="text-right">{{ currency(totals.totalArbitrationApplicationCount) }}</b-th>
                  <b-th class="text-right">{{ currency(totals.totalArbitrationApplicationAmount?.item1) }}</b-th>
                  <b-th class="text-right">{{ currency(totals.totalArbitrationApplicationAmount?.item2) }}</b-th>
                  <b-th class="text-right">{{ currency(totals.totalArbitrationApplicationAmount?.item3) }}</b-th>
                  <b-th class="text-right">{{ currency(totals.totalArbitrationPaidApplicationAmount?.item1) }}</b-th>
                  <b-th class="text-right">{{ currency(totals.totalArbitrationPaidApplicationAmount?.item2) }}</b-th>
                  <b-th class="text-right">{{ currency(totals.totalArbitrationPaidApplicationAmount?.item3) }}</b-th>
                  <b-th class="text-right">{{
                     currency(totals.totalArbitrationLatePaidApplicationAmount?.item1)
                  }}</b-th>
                  <b-th class="text-right">{{
                     currency(totals.totalArbitrationLatePaidApplicationAmount?.item2)
                  }}</b-th>
                  <b-th class="text-right">{{
                     currency(totals.totalArbitrationLatePaidApplicationAmount?.item3)
                  }}</b-th>
                  <b-th class="text-right">{{ currency(totals.totalArbitrationAcceptedCount) }}</b-th>
                  <b-th class="text-right">{{ currency(totals.totalArbitrationPartiallyAcceptedCount) }}</b-th>
                  <b-th class="text-right">{{ currency(totals.totalArbitrationCanceledCount) }}</b-th>
                  <b-th class="text-right">{{ currency(totals.totalArbitrationAcceptedAmount?.item1) }}</b-th>
                  <b-th class="text-right">{{ currency(totals.totalArbitrationAcceptedAmount?.item2) }}</b-th>
                  <b-th class="text-right">{{ currency(totals.totalArbitrationAcceptedAmount?.item3) }}</b-th>
                  <b-th class="text-right">{{ currency(totals.totalArbitrationPartiallyAcceptedAmount?.item1) }}</b-th>
                  <b-th class="text-right">{{ currency(totals.totalArbitrationPartiallyAcceptedAmount?.item2) }}</b-th>
                  <b-th class="text-right">{{ currency(totals.totalArbitrationPartiallyAcceptedAmount?.item3) }}</b-th>
                  <b-th class="text-right">{{ currency(totals.totalArbitrationCanceledAmount?.item1) }}</b-th>
                  <b-th class="text-right">{{ currency(totals.totalArbitrationCanceledAmount?.item2) }}</b-th>
                  <b-th class="text-right">{{ currency(totals.totalArbitrationCanceledAmount?.item3) }}</b-th>
               </b-tr>
            </b-tfoot>
            <template #overlay>
               <div class="text-center text-primary my-2">
                  <b-spinner scale="2" class="align-middle mr-2"></b-spinner>
                  <strong>{{ $t('Loading') }}...</strong>
               </div>
            </template>
         </b-table-simple>
         <template #overlay>
            <div class="text-center text-primary my-2">
               <b-spinner scale="2" class="align-middle mr-2"></b-spinner>
               <strong>{{ $t('Loading') }}...</strong>
            </div>
         </template>
      </b-overlay>
   </b-card>
</template>

<script>
import ReportService from '@/services/report/report.service';
import RegionService from '@/services/info/region.service';
import DistrictService from '@/services/info/district.service';
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
   data() {
      return {
         tableData: [],
         RegionList: [],
         DistrictList: [],
         PrintLoading: false,
         isBusy: false,
         filter: {
            regionId: null,
            byRegion: true,
            districtId: null,
            byDistrict: false,
            contractorId: null,
            byContractor: false,
            region: '',
            district: ''
         },
         totals: {
            totalArbitrationApplicationCount: 0,
            totalArbitrationApplicationAmount: {
               item1: 0,
               item2: 0,
               item3: 0
            },
            totalArbitrationPaidApplicationAmount: {
               item1: 0,
               item2: 0,
               item3: 0
            },
            totalArbitrationLatePaidApplicationAmount: {
               item1: 0,
               item2: 0,
               item3: 0
            },
            totalArbitrationAcceptedCount: 0,
            totalArbitrationPartiallyAcceptedCount: 0,
            totalArbitrationCanceledCount: 0,
            totalArbitrationAcceptedAmount: {
               item1: 0,
               item2: 0,
               item3: 0
            },
            totalArbitrationPartiallyAcceptedAmount: {
               item1: 0,
               item2: 0,
               item3: 0
            },
            totalArbitrationCanceledAmount: {
               item1: 0,
               item2: 0,
               item3: 0
            }
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
         this.filter.claimApplicationTypeId = null;
         this.filter.byRegion = false;
         this.filter.regionId = item.regionId;
         this.filter.byDistrict = true;
         this.filter.districtId = null;
         this.filter.byContractor = false;
         this.filter.contractorId = null;
         this.filter.region = item.region;
         this.Refresh();
         this.GetDistrict(item.regionId);
      },
      SortDistrict(item) {
         this.filter.claimApplicationTypeId = null;
         this.filter.byRegion = false;
         this.filter.district = item.district;
         // this.filter.regionId = item.regionId;
         this.filter.byDistrict = false;
         this.filter.districtId = item.districtId;
         this.filter.byContractor = true;
         this.filter.contractorId = null;
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
      Refresh() {
         this.isBusy = true;
         ReportService.GetArbitrationApplicationReport(this.filter)
            .then((res) => {
               this.tableData = res.data;

               this.totals = {
                  totalArbitrationApplicationCount: 0,
                  totalArbitrationApplicationAmount: {
                     item1: 0,
                     item2: 0,
                     item3: 0
                  },
                  totalArbitrationPaidApplicationAmount: {
                     item1: 0,
                     item2: 0,
                     item3: 0
                  },
                  totalArbitrationLatePaidApplicationAmount: {
                     item1: 0,
                     item2: 0,
                     item3: 0
                  },
                  totalArbitrationAcceptedCount: 0,
                  totalArbitrationPartiallyAcceptedCount: 0,
                  totalArbitrationCanceledCount: 0,
                  totalArbitrationAcceptedAmount: {
                     item1: 0,
                     item2: 0,
                     item3: 0
                  },
                  totalArbitrationPartiallyAcceptedAmount: {
                     item1: 0,
                     item2: 0,
                     item3: 0
                  },
                  totalArbitrationCanceledAmount: {
                     item1: 0,
                     item2: 0,
                     item3: 0
                  }
               };

               res.data.forEach((item) => {
                  this.totals.totalArbitrationApplicationCount += item.totalArbitrationApplicationCount;
                  this.totals.totalArbitrationApplicationAmount.item1 += item.totalArbitrationApplicationAmount.item1;
                  this.totals.totalArbitrationApplicationAmount.item2 += item.totalArbitrationApplicationAmount.item2;
                  this.totals.totalArbitrationApplicationAmount.item3 += item.totalArbitrationApplicationAmount.item3;
                  this.totals.totalArbitrationPaidApplicationAmount.item1 +=
                     item.totalArbitrationPaidApplicationAmount.item1;
                  this.totals.totalArbitrationPaidApplicationAmount.item2 +=
                     item.totalArbitrationPaidApplicationAmount.item2;
                  this.totals.totalArbitrationPaidApplicationAmount.item3 +=
                     item.totalArbitrationPaidApplicationAmount.item3;
                  this.totals.totalArbitrationLatePaidApplicationAmount.item1 +=
                     item.totalArbitrationLatePaidApplicationAmount.item1;
                  this.totals.totalArbitrationLatePaidApplicationAmount.item2 +=
                     item.totalArbitrationLatePaidApplicationAmount.item2;
                  this.totals.totalArbitrationLatePaidApplicationAmount.item3 +=
                     item.totalArbitrationLatePaidApplicationAmount.item3;
                  this.totals.totalArbitrationAcceptedCount += item.totalArbitrationAcceptedCount;
                  this.totals.totalArbitrationPartiallyAcceptedCount += item.totalArbitrationPartiallyAcceptedCount;
                  this.totals.totalArbitrationCanceledCount += item.totalArbitrationCanceledCount;
                  this.totals.totalArbitrationAcceptedAmount.item1 += item.totalArbitrationAcceptedAmount.item1;
                  this.totals.totalArbitrationAcceptedAmount.item2 += item.totalArbitrationAcceptedAmount.item2;
                  this.totals.totalArbitrationAcceptedAmount.item3 += item.totalArbitrationAcceptedAmount.item3;
                  this.totals.totalArbitrationPartiallyAcceptedAmount.item1 +=
                     item.totalArbitrationPartiallyAcceptedAmount.item1;
                  this.totals.totalArbitrationPartiallyAcceptedAmount.item2 +=
                     item.totalArbitrationPartiallyAcceptedAmount.item2;
                  this.totals.totalArbitrationPartiallyAcceptedAmount.item3 +=
                     item.totalArbitrationPartiallyAcceptedAmount.item3;
                  this.totals.totalArbitrationCanceledAmount.item1 += item.totalArbitrationCanceledAmount.item1;
                  this.totals.totalArbitrationCanceledAmount.item2 += item.totalArbitrationCanceledAmount.item2;
                  this.totals.totalArbitrationCanceledAmount.item3 += item.totalArbitrationCanceledAmount.item3;
               });

               this.isBusy = false;
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
@import '/src/@core/scss/tablestyle.scss';
</style>
