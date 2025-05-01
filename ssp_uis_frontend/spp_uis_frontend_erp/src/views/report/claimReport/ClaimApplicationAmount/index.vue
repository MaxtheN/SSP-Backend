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

         <b-col cols="12" md="3">
            <div>
               <label for>{{ $t('innOrPinfl') }}</label>
               <b-input-group>
                  <b-form-input
                     v-model="filters.contractorInn"
                     debounce="300"
                     v-mask="['##############']"
                     @keyup.enter="Refresh"
                     :placeholder="$t('innOrPinfl')"
                  />
                  <b-input-group-append>
                     <b-button @click="Refresh" size="sm" variant="primary">
                        <feather-icon icon="SearchIcon" />
                     </b-button>
                  </b-input-group-append>
               </b-input-group>
            </div>
         </b-col>
         <b-col cols="12" md="3">
            <form-select
               :options="ClaimThemeList"
               placeholder="ChooseBelow"
               label="ClaimTheme"
               v-model="filters.claimThemeId"
               @input="Refresh"
            />
         </b-col>

         <b-col cols="12" md="2">
            <div>
               <label for>{{ $t('startdate') }}</label>
               <form-picker v-model="filters.fromDate" :placeholder="$t('startdate')" @input="Refresh" />
            </div>
         </b-col>
         <b-col cols="12" md="2">
            <div>
               <label for>{{ $t('enddate') }}</label>
               <form-picker v-model="filters.toDate" :placeholder="$t('enddate')" @input="Refresh" />
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
                  <b-th>№</b-th>
                  <b-th>
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
                  <b-th>{{ $t('totalSummaInArea') }}</b-th>
                  <b-th>{{ $t('treatedSum') }}</b-th>

                  <b-th v-if="filters.byContractor">{{ $t('inn') }}</b-th>
                  <b-th v-if="filters.byContractor">{{ $t('ClaimTheme') }}</b-th>
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
                  <b-td style="text-align: right">{{ currency(item.totalClaimApplicationAmount) }}</b-td>
                  <b-td style="text-align: right">{{ currency(item.totalChargedAmount) }}</b-td>
                  <b-td v-if="filters.byContractor" style="text-align: right">{{ item.contractorInn }}</b-td>
                  <b-td v-if="filters.byContractor" style="text-align: right">{{ item.claimTheme }}</b-td>
                  <!-- <b-td style="text-align: right">{{ currency(item.treatedSum.legalSumma) }}</b-td>
                  <b-td style="text-align: right">{{ currency(item.treatedSum.yattSumma) }}</b-td>
                  <b-td style="text-align: right">{{ currency(item.treatedSum.individualsSumma) }}</b-td>
                  <b-td style="text-align: right">{{ currency(item.treatedSum.stateOrganizationSumma) }}</b-td>
                  <b-td style="text-align: right">{{ currency(item.treatedSum.foreignCitizenSumma) }}</b-td>
                  <b-td style="text-align: right">{{ currency(item.unidirectionalSum.totalSumma) }}</b-td>
                  <b-td style="text-align: right">{{ currency(item.unidirectionalSum.legalSumma) }}</b-td>
                  <b-td style="text-align: right">{{ currency(item.unidirectionalSum.yattSumma) }}</b-td>
                  <b-td style="text-align: right">{{ currency(item.unidirectionalSum.individualsSumma) }}</b-td>
                  <b-td style="text-align: right">{{ currency(item.unidirectionalSum.stateOrganizationSumma) }}</b-td>
                  <b-td style="text-align: right">{{ currency(item.unidirectionalSum.foreignCitizenSumma) }}</b-td> -->
               </b-tr>
            </b-tbody>
            <b-tfoot v-if="tableData.length > 0">
               <b-tr>
                  <b-th colspan="2">
                     <span class="ml-3">{{ $t('Total') }}</span>
                  </b-th>
                  <b-th style="text-align: right">{{ currency(totals.totalClaimApplicationAmount) }} </b-th>
                  <b-th style="text-align: right">{{ currency(totals.totalSumma1) }} </b-th>
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
import ClaimThemeService from '@/services/info/claimtheme.service';
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
         ClaimThemeList: [],
         PrintLoading: false,
         isBusy: false,
         filters: {
            claimApplicationTypeId: null,
            regionId: null,
            contractorInn: '',
            byRegion: true,
            districtId: null,
            byDistrict: false,
            contractorId: null,
            byContractor: false,
            region: '',
            district: ''
         },
         totals: {
            totalClaimApplicationAmount: 0,
            totalChargedAmount: 0
         }
      };
   },
   created() {
      ClaimThemeService.GetAsSelectList().then((res) => {
         if (Array.isArray(res.data)) {
            this.ClaimThemeList = res.data;
         }
      });

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

         if (this.filters.contractorInn) {
            this.filters.byContractor = true;
            this.filters.byRegion = false;
         } else {
            this.filters.byContractor = false;
            this.filters.byRegion = true;
            if (this.filters.regionId) {
               this.filters.byRegion = false;
               this.filters.byContractor = false;
               this.filters.byDistrict = true;
            }
            if (this.filters.districtId) {
               this.filters.byDistrict = false;
               this.filters.byContractor = true;
               this.filters.byRegion = false;
            }
         }

         ReportService.ClaimApplicationAmount(this.filters)
            .then((res) => {
               this.tableData = res.data;
               this.isBusy = false;

               this.totals.totalClaimApplicationAmount = 0;
               this.totals.totalChargedAmount = 0;

               this.tableData.forEach((item) => {
                  this.totals.totalClaimApplicationAmount += item.totalClaimApplicationAmount;
                  this.totals.totalChargedAmount += item.treatedSum.totalChargedAmount;
               });
            })
            .catch((errors) => {
               this.makeToast(errors.response.data.error, 'danger');
            })
            .finally(() => {
               this.isBusy = false;
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
