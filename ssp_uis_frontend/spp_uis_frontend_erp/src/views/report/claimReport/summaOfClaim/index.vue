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
                  <b-th colspan="6">{{ $t('treatedSum') }}</b-th>
                  <b-th colspan="6">{{ $t('unidirectionalSum') }}</b-th>
               </b-tr>
               <b-tr>
                  <b-th>{{ $t('totalSumma') }}</b-th>
                  <b-th>{{ $t('legalSumma') }}</b-th>
                  <b-th>{{ $t('individualsSumma') }}</b-th>
                  <b-th>{{ $t('item2') }}</b-th>
                  <b-th>{{ $t('stateOrganizationSumma') }}</b-th>
                  <b-th>{{ $t('foreignCitizenSumma') }}</b-th>

                  <b-th>{{ $t('yattSumma2') }}</b-th>
                  <b-th>{{ $t('legalSumma') }}</b-th>
                  <b-th>{{ $t('individualsSumma') }}</b-th>
                  <b-th>{{ $t('item2') }}</b-th>
                  <b-th>{{ $t('stateOrganizationSumma') }}</b-th>
                  <b-th>{{ $t('foreignCitizenSumma') }}</b-th>
               </b-tr>
            </b-thead>

            <b-tbody v-if="tableData.length > 0">
               <b-tr v-for="(item, index) in tableData" :key="index">
                  <b-td>{{ index + 1 }}</b-td>
                  <b-td>
                     <span v-show="filters.byRegion" @click="SortRegion(item)" style="color: blue; cursor: pointer">
                        {{ item.region }}
                     </span>
                     <span v-show="filters.byDistrict" @click="SortDistrict(item)" style="color: blue; cursor: pointer">
                        {{ item.district }}
                     </span>
                     <span v-show="filters.byContractor">
                        {{ item.contractor }}
                     </span>
                  </b-td>
                  <b-td style="text-align: right">{{ currency(item.totalSummaInArea) }}</b-td>
                  <b-td style="text-align: right">{{ currency(item.treatedSum.totalSumma) }}</b-td>
                  <b-td style="text-align: right">{{ currency(item.treatedSum.legalSumma) }}</b-td>
                  <b-td style="text-align: right">{{ currency(item.treatedSum.yattSumma) }}</b-td>
                  <b-td style="text-align: right">{{ currency(item.treatedSum.individualsSumma) }}</b-td>
                  <b-td style="text-align: right">{{ currency(item.treatedSum.stateOrganizationSumma) }}</b-td>
                  <b-td style="text-align: right">{{ currency(item.treatedSum.foreignCitizenSumma) }}</b-td>
                  <b-td style="text-align: right">{{ currency(item.unidirectionalSum.totalSumma) }}</b-td>
                  <b-td style="text-align: right">{{ currency(item.unidirectionalSum.legalSumma) }}</b-td>
                  <b-td style="text-align: right">{{ currency(item.unidirectionalSum.yattSumma) }}</b-td>
                  <b-td style="text-align: right">{{ currency(item.unidirectionalSum.individualsSumma) }}</b-td>
                  <b-td style="text-align: right">{{ currency(item.unidirectionalSum.stateOrganizationSumma) }}</b-td>
                  <b-td style="text-align: right">{{ currency(item.unidirectionalSum.foreignCitizenSumma) }}</b-td>
               </b-tr>
            </b-tbody>
            <b-tfoot v-if="tableData.length > 0">
               <b-tr>
                  <b-th colspan="2">
                     <span class="ml-3">{{ $t('Total') }}</span>
                  </b-th>
                  <b-th style="text-align: right">{{ currency(totals.totalSummaInArea) }} </b-th>
                  <b-th style="text-align: right">{{ currency(totals.totalSumma1) }} </b-th>
                  <b-th style="text-align: right">{{ currency(totals.legalSumma1) }} </b-th>
                  <b-th style="text-align: right">{{ currency(totals.yattSumma1) }}</b-th>
                  <b-th style="text-align: right">{{ currency(totals.individualsSumma1) }}</b-th>
                  <b-th style="text-align: right">{{ currency(totals.stateOrganizationSumma1) }} </b-th>
                  <b-th style="text-align: right">{{ currency(totals.foreignCitizenSumm1) }} </b-th>

                  <b-th style="text-align: right">{{ currency(totals.totalSumma2) }} </b-th>
                  <b-th style="text-align: right">{{ currency(totals.legalSumma2) }} </b-th>
                  <b-th style="text-align: right">{{ currency(totals.yattSumma2) }}</b-th>
                  <b-th style="text-align: right">{{ currency(totals.individualsSumma2) }}</b-th>
                  <b-th style="text-align: right">{{ currency(totals.stateOrganizationSumma2) }} </b-th>
                  <b-th style="text-align: right">{{ currency(totals.foreignCitizenSumma2) }} </b-th>
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
      BOverlay,
      VBTooltip,
      VBModal
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
            totalSummaInArea: 0,
            totalSumma1: 0,
            legalSumma1: 0,
            yattSumma1: 0,
            individualsSumma1: 0,
            stateOrganizationSumma1: 0,
            foreignCitizenSumm1: 0,
            totalSumma2: 0,
            legalSumma2: 0,
            yattSumma2: 0,
            individualsSumma2: 0,
            stateOrganizationSumma2: 0,
            foreignCitizenSumma2: 0
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
            this.filters.claimApplicationTypeId = null;
            this.filters.byDistrict = false;
            this.filters.contractorId = null;
            this.filters.byContractor = true;
            this.filters.byRegion = false;

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
      Refresh() {
         this.isBusy = true;
         ReportService.SummaOfClaimApplicationReport(this.filters)
            .then((res) => {
               this.tableData = res.data;
               this.isBusy = false;
               this.totals.totalSummaInArea = 0;
               this.totals.totalSumma1 = 0;
               this.totals.legalSumma1 = 0;
               this.totals.yattSumma1 = 0;
               this.totals.individualsSumma1 = 0;
               this.totals.stateOrganizationSumma1 = 0;
               this.totals.foreignCitizenSumm1 = 0;

               this.totals.totalSumma2 = 0;
               this.totals.legalSumma2 = 0;
               this.totals.yattSumma2 = 0;
               this.totals.individualsSumma2 = 0;
               this.totals.stateOrganizationSumma2 = 0;
               this.totals.foreignCitizenSumma2 = 0;

               this.tableData.forEach((item) => {
                  this.totals.totalSummaInArea += item.totalSummaInArea;
                  this.totals.totalSumma1 += item.treatedSum.totalSumma;
                  this.totals.legalSumma1 += item.treatedSum.legalSumma;
                  this.totals.yattSumma1 += item.treatedSum.yattSumma;
                  this.totals.individualsSumma1 += item.treatedSum.individualsSumma;
                  this.totals.stateOrganizationSumma1 += item.treatedSum.stateOrganizationSumma;
                  this.totals.foreignCitizenSumm1 += item.treatedSum.foreignCitizenSumma;
                  this.totals.totalSumma2 += item.unidirectionalSum.totalSumma;
                  this.totals.legalSumma2 += item.unidirectionalSum.legalSumma;
                  this.totals.yattSumma2 += item.unidirectionalSum.yattSumma;
                  this.totals.individualsSumma2 += item.unidirectionalSum.individualsSumma;
                  this.totals.stateOrganizationSumma2 += item.unidirectionalSum.stateOrganizationSumma;
                  this.totals.foreignCitizenSumma2 += item.unidirectionalSum.foreignCitizenSumm;
               });
            })
            .catch((errors) => {
               this.makeToast(errors.response.data.error, 'danger');
            });
      },
      Print() {
         this.PrintLoading = true;
         ReportService.SaveAsExcelSummaOfClaimApplication(this.filters).then((res) => {
            this.forceFileDownload(res, this.$t('SummaOfClaimApplicationReport'));
            this.PrintLoading = false;
         });
      }
   }
};
</script>
<style lang="scss" scoped>
@import '../../styles.scss';
</style>
