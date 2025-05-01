<template>
   <b-card>
      <b-overlay :show="isBusy">
         <b-row>
            <b-col sm="12" md="2">
               <div>
                  <label for>{{ $t('region') }}</label>
                  <v-select
                     :options="RegionList"
                     :reduce="(item) => item.value"
                     :placeholder="$t('ChooseBelow')"
                     label="text"
                     @input="ChangeRegion"
                     class="w-100"
                     v-model="filters.regionId"
                  >
                  </v-select>
               </div>
            </b-col>
            <b-col sm="12" md="2">
               <div>
                  <div>
                     <label for>{{ $t('Region') }}</label>
                     <v-select
                        :options="DistrictList"
                        :reduce="(item) => item.value"
                        :placeholder="$t('ChooseBelow')"
                        label="text"
                        v-model="filters.districtId"
                        @input="ChangeDistrict"
                        class="w-100"
                     ></v-select>
                  </div>
               </div>
            </b-col>
            <b-col sm="12" md="2" class="mt-2">
               <b-button @click="Print" :disabled="PrintLoading" variant="primary" class="ml-1">
                  <feather-icon icon="PrinterIcon"></feather-icon>
               </b-button>
            </b-col>
         </b-row>

         <b-row align-h="between">
            <b-col sm="12" md="8">
               <b-breadcrumb class="mt-2 mb-1">
                  <b-breadcrumb-item
                     :active="filters.byRegion"
                     @click="
                        () => {
                           filters.byDistrict = false;
                           filters.byRegion = true;
                           filters.byContractor = false;
                           filters.region = '';
                           filters.regionId = null;
                           filters.district = '';
                           filters.districtId = null;
                           Refresh();
                        }
                     "
                  >
                     <b>{{ $t('uzb') }}</b>
                  </b-breadcrumb-item>
                  <b-breadcrumb-item
                     v-show="filters.region"
                     :active="filters.byDistrict"
                     @click="
                        () => {
                           filters.byDistrict = true;
                           filters.byRegion = false;
                           filters.byContractor = false;
                           filters.district = '';
                           filters.districtId = null;
                           Refresh();
                        }
                     "
                  >
                     <b>{{ filters.region }}</b>
                  </b-breadcrumb-item>
                  <b-breadcrumb-item v-show="filters.district" :active="filters.byContractor">
                     <b>{{ filters.district }}</b>
                  </b-breadcrumb-item>
                  <!-- <b-breadcrumb-item v-show="filter.byContractor" active>Baz</b-breadcrumb-item> -->
               </b-breadcrumb>
            </b-col>
         </b-row>
         <b-table-simple class="report-table" hover small caption-top responsive border>
            <b-thead>
               <b-tr>
                  <b-th rowspan="2">№</b-th>
                  <b-th rowspan="2">
                     <span v-show="filters.byRegion">
                        {{ $t('region') }}
                     </span>
                     <span v-show="filters.byDistrict">
                        {{ $t('district') }}
                     </span>
                     <span v-show="filters.byContractor">
                        {{ $t('contractor') }}
                     </span>
                  </b-th>
                  <b-th rowspan="4">{{ $t('totalApplicationAmountInArea') }}</b-th>
                  <b-th colspan="2">{{ $t('applicationCount') }}</b-th>
                  <b-th colspan="2">{{ $t('mediationPlanCount') }}</b-th>
                  <b-th colspan="2">{{ $t('mediationCount') }}</b-th>
                  <b-th colspan="2">{{ $t('claimApplicationForCourtCount') }}</b-th>
                  <b-th colspan="2">{{ $t('rejectCancelApplicationCancel') }}</b-th>
               </b-tr>
               <b-tr>
                  <b-th>{{ $t('item1') }}</b-th>
                  <b-th>{{ $t('item2') }}</b-th>
                  <b-th>{{ $t('item1') }}</b-th>
                  <b-th>{{ $t('item2') }}</b-th>
                  <b-th>{{ $t('item1') }}</b-th>
                  <b-th>{{ $t('item2') }}</b-th>
                  <b-th>{{ $t('item1') }}</b-th>
                  <b-th>{{ $t('item2') }}</b-th>
                  <b-th>{{ $t('item1') }}</b-th>
                  <b-th>{{ $t('item2') }}</b-th>
               </b-tr>
            </b-thead>
            <b-tbody>
               <b-tr v-for="(item, index) in tableData" :key="index">
                  <b-td>{{ index + 1 }}</b-td>
                  <b-td>
                     <span v-show="filters.byRegion" @click="SortRegion(item)" style="color: blue; cursor: pointer">
                        {{ item.region }}
                     </span>
                     <span v-show="filters.byDistrict" @click="SortDistrict(item)" style="color: blue; cursor: pointer">
                        {{ item.district }}
                     </span>
                     <span v-show="filters.byContractor" @click="SortDistrict(item)">
                        {{ item.contractor }}
                     </span>
                  </b-td>
                  <b-td class="text-right">{{ currency(item.totalApplicationAmountInArea) }}</b-td>
                  <b-td class="text-right">{{ currency(item.applicationCount['item1']) }}</b-td>
                  <b-td class="text-right">{{ currency(item.applicationCount['item2']) }}</b-td>
                  <b-td class="text-right">{{ currency(item.mediationPlanCount['item1']) }}</b-td>
                  <b-td class="text-right">{{ currency(item.mediationPlanCount['item2']) }}</b-td>
                  <b-td class="text-right">{{ currency(item.mediationCount['item1']) }}</b-td>
                  <b-td class="text-right">{{ currency(item.mediationCount['item2']) }}</b-td>
                  <b-td class="text-right">{{ currency(item.claimApplicationForCourtCount['item1']) }}</b-td>
                  <b-td class="text-right">{{ currency(item.claimApplicationForCourtCount['item2']) }}</b-td>
                  <b-td class="text-right">{{ currency(item.rejectCancelApplicationCancel['item1']) }}</b-td>
                  <b-td class="text-right">{{ currency(item.rejectCancelApplicationCancel['item2']) }}</b-td>
               </b-tr>
            </b-tbody>
            <b-tfoot v-if="tableData.length > 0">
               <b-tr>
                  <b-th colspan="2">
                     <span class="ml-3">{{ $t('Total') }}</span>
                  </b-th>
                  <b-th class="text-right"> {{ currency(totals.totalApplicationAmountInArea) }}</b-th>
                  <b-th class="text-right"> {{ currency(totals.applicationCount.item1) }} </b-th>
                  <b-th class="text-right"> {{ currency(totals.applicationCount.item2) }} </b-th>
                  <b-th class="text-right"> {{ currency(totals.mediationPlanCount.item1) }} </b-th>
                  <b-th class="text-right"> {{ currency(totals.mediationPlanCount.item2) }} </b-th>
                  <b-th class="text-right"> {{ currency(totals.mediationCount.item1) }} </b-th>
                  <b-th class="text-right"> {{ currency(totals.mediationCount.item2) }} </b-th>
                  <b-th class="text-right"> {{ currency(totals.claimApplicationForCourtCount.item1) }} </b-th>
                  <b-th class="text-right"> {{ currency(totals.claimApplicationForCourtCount.item2) }} </b-th>
                  <b-th class="text-right"> {{ currency(totals.rejectCancelApplicationCancel.item1) }} </b-th>
                  <b-th class="text-right"> {{ currency(totals.rejectCancelApplicationCancel.item2) }} </b-th>
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
         filters: {
            claimApplicationTypeId: null,
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
            totalApplicationAmountInArea: 0,
            applicationCount: {
               item1: 0,
               item2: 0
            },
            mediationPlanCount: {
               item1: 0,
               item2: 0
            },
            mediationCount: {
               item1: 0,
               item2: 0
            },
            claimApplicationForCourtCount: {
               item1: 0,
               item2: 0
            },
            rejectCancelApplicationCancel: {
               item1: 0,
               item2: 0
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
         this.filters.claimApplicationTypeId = null;
         this.filters.byRegion = false;
         this.filters.regionId = item.regionId;
         this.filters.byDistrict = true;
         this.filters.districtId = null;
         this.filters.byContractor = false;
         this.filters.contractorId = null;
         this.filters.region = item.region;
         this.Refresh();
         this.GetDistrict(item.regionId);
      },
      SortDistrict(item) {
         this.filters.claimApplicationTypeId = null;
         this.filters.byRegion = false;
         this.filters.district = item.district;
         // this.filters.regionId = item.regionId;
         this.filters.byDistrict = false;
         this.filters.districtId = item.districtId;
         this.filters.byContractor = true;
         this.filters.contractorId = null;
         this.Refresh();
      },
      ChangeRegion(id) {
         if (id) {
            this.filters.districtId = null;
            this.filters.byDistrict = true;
            this.filters.byRegion = false;
            this.filters.byContractor = false;

            this.filters.region = this.filters.regionId
               ? this.RegionList.filter((item) => item.value === this.filters.regionId)[0].text
               : '';

            this.Refresh();
            this.GetDistrict(id);
         } else {
            this.filters.districtId = null;
            this.filters.byDistrict = false;
            this.filters.byRegion = true;
            this.filters.region = '';
            this.filters.byContractor = false;
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
            this.filters.byDistrict = false;
            this.filters.byRegion = false;
            this.filters.byContractor = true;
            this.filters.district = this.filters.districtId
               ? this.DistrictList.filter((item) => item.value === this.filters.districtId)[0].text
               : '';

            this.Refresh();
         } else {
            this.filters.byDistrict = true;
            this.filters.byRegion = false;
            this.filters.byContractor = false;
            this.filters.district = '';
            this.Refresh();
         }
      },
      Refresh() {
         this.isBusy = true;
         ReportService.AppealsSentToClaimApplicationReport(this.filters)
            .then((res) => {
               this.tableData = res.data;

               this.totals.totalApplicationAmountInArea = 0;
               this.totals.applicationCount.item1 = 0;
               this.totals.applicationCount.item2 = 0;
               this.totals.mediationPlanCount.item1 = 0;
               this.totals.mediationPlanCount.item2 = 0;
               this.totals.mediationCount.item1 = 0;
               this.totals.mediationCount.item2 = 0;
               this.totals.claimApplicationForCourtCount.item1 = 0;
               this.totals.claimApplicationForCourtCount.item2 = 0;
               this.totals.rejectCancelApplicationCancel.item1 = 0;
               this.totals.rejectCancelApplicationCancel.item2 = 0;

               this.tableData.forEach((item) => {
                  this.totals.totalApplicationAmountInArea += item.totalApplicationAmountInArea;
                  this.totals.applicationCount.item1 += item.applicationCount.item1;
                  this.totals.applicationCount.item2 += item.applicationCount.item2;
                  this.totals.mediationPlanCount.item1 += item.mediationPlanCount.item1;
                  this.totals.mediationPlanCount.item2 += item.mediationPlanCount.item2;
                  this.totals.mediationCount.item1 += item.mediationCount.item1;
                  this.totals.mediationCount.item2 += item.mediationCount.item2;
                  this.totals.claimApplicationForCourtCount.item1 += item.claimApplicationForCourtCount.item1;
                  this.totals.claimApplicationForCourtCount.item2 += item.claimApplicationForCourtCount.item2;
                  this.totals.rejectCancelApplicationCancel.item1 += item.rejectCancelApplicationCancel.item1;
                  this.totals.rejectCancelApplicationCancel.item2 += item.rejectCancelApplicationCancel.item2;
               });
               this.isBusy = false;
            })
            .catch((errors) => {
               this.makeToast(errors.response.data.error, 'danger');
            });
      },
      Print() {
         this.PrintLoading = true;
         ReportService.SaveAsExcelAppealsSentToClaimApplication(this.filters).then((res) => {
            this.forceFileDownload(res, this.$t('AppealsSentToClaimApplicationReport'));
            this.PrintLoading = false;
         });
      }
   }
};
</script>
<style lang="scss" scoped>
@import '../../styles.scss';
</style>
