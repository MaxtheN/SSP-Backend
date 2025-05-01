<template>
   <b-card>
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
            <b-breadcrumb class="my-2">
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
      <b-overlay :show="isBusy">
         <b-table-simple class="report-table" hover small caption-top responsive border>
            <b-thead>
               <b-tr>
                  <b-th rowspan="4">№</b-th>
                  <b-th rowspan="4">
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
                  <b-th rowspan="4">{{ $t('totalSummaInArea') }}</b-th>
                  <b-th v-for="(item, index) of tableData.columns" :key="index">{{ item }}</b-th>
               </b-tr>
            </b-thead>

            <b-tbody>
               <b-tr v-for="(item, index) in tableData.rows" :key="index">
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
                     </span></b-td
                  >
                  <b-td class="text-right">{{ currency(item.totalApplicationAmountInArea) }}</b-td>
                  <b-td class="text-right">{{ currency(item.claimThemeCount['1']) }}</b-td>
                  <b-td class="text-right">{{ currency(item.claimThemeCount['2']) }}</b-td>
                  <b-td class="text-right">{{ currency(item.claimThemeCount['3']) }}</b-td>
                  <b-td class="text-right">{{ currency(item.claimThemeCount['4']) }}</b-td>
                  <b-td class="text-right">{{ currency(item.claimThemeCount['5']) }}</b-td>
                  <b-td class="text-right">{{ currency(item.claimThemeCount['6']) }}</b-td>
                  <b-td class="text-right">{{ currency(item.claimThemeCount['7']) }}</b-td>
                  <b-td class="text-right">{{ currency(item.claimThemeCount['8']) }}</b-td>
                  <b-td class="text-right">{{ currency(item.claimThemeCount['9']) }}</b-td>
               </b-tr>
            </b-tbody>
            <b-tfoot v-if="tableData.rows?.length > 0">
               <b-tr>
                  <b-th colspan="2">
                     <span class="ml-3">{{ $t('Total') }}</span>
                  </b-th>
                  <b-th class="text-right"> {{ currency(totals.totalApplicationAmountInArea) }}</b-th>
                  <b-th class="text-right"> {{ currency(totals.total1) }}</b-th>
                  <b-th class="text-right"> {{ currency(totals.total2) }}</b-th>
                  <b-th class="text-right"> {{ currency(totals.total3) }}</b-th>
                  <b-th class="text-right"> {{ currency(totals.total4) }}</b-th>
                  <b-th class="text-right"> {{ currency(totals.total5) }}</b-th>
                  <b-th class="text-right"> {{ currency(totals.total6) }}</b-th>
                  <b-th class="text-right"> {{ currency(totals.total7) }}</b-th>
                  <b-th class="text-right"> {{ currency(totals.total8) }}</b-th>
                  <b-th class="text-right"> {{ currency(totals.total9) }}</b-th>
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
import DistrictService from '@/services/info/district.service';
import RegionService from '@/services/info/region.service';
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
         PrintLoading: false,
         tableData: {},
         RegionList: [],
         DistrictList: [],
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
            totalSummaInArea: 0,
            total1: 0,
            total2: 0,
            total3: 0,
            total4: 0,
            total5: 0,
            total6: 0,
            total7: 0,
            total8: 0,
            total9: 0
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
         if (item.organisationId == 1) {
            this.filters.regionId = null;
            this.filters.isSsp = true;
            this.filters.byRegion = false;
            this.filters.districtId = null;
            this.filters.byDistrict = false;
            this.filters.contractorId = null;
            this.filters.byContractor = true;
            this.filters.byRegion = false;
            this.filters.claimApplicationTypeId = null;
            this.Refresh();
         } else {
            this.filters.isSsp = false;
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
         }
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
      Print() {
         this.PrintLoading = true;
         ReportService.SaveAsExcelReceivedClaimApplication(this.filters).then((res) => {
            this.forceFileDownload(res, this.$t('ReceivedClaimApplicationReport'));
            this.PrintLoading = false;
         });
      },
      Refresh() {
         this.isBusy = true;
         ReportService.ReceivedClaimApplicationReport(this.filters)
            .then((res) => {
               this.tableData = res.data;
               this.totals.totalApplicationAmountInArea = 0;
               this.totals.total1 = 0;
               this.totals.total2 = 0;
               this.totals.total3 = 0;
               this.totals.total4 = 0;
               this.totals.total5 = 0;
               this.totals.total6 = 0;
               this.totals.total7 = 0;
               this.totals.total8 = 0;
               this.totals.total9 = 0;
               this.tableData.rows.forEach((item) => {
                  this.totals.totalApplicationAmountInArea += item.totalApplicationAmountInArea;
                  this.totals.total1 += item.claimThemeCount['1'];
                  this.totals.total2 += item.claimThemeCount['2'];
                  this.totals.total3 += item.claimThemeCount['3'];
                  this.totals.total4 += item.claimThemeCount['4'];
                  this.totals.total5 += item.claimThemeCount['5'];
                  this.totals.total6 += item.claimThemeCount['6'];
                  this.totals.total7 += item.claimThemeCount['7'];
                  this.totals.total8 += item.claimThemeCount['8'];
                  this.totals.total9 += item.claimThemeCount['9'];
               });
               this.isBusy = false;
            })
            .catch((error) => {
               this.showApiError(error);
            });
      }
   }
};
</script>
<style lang="scss" scoped>
@import '../../styles.scss';
</style>
