<template>
   <b-card>
      <b-overlay :show="isBusy">
         <b-row>
            <b-col sm="12" md="4" lg="3">
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
            <b-col sm="12" md="4" lg="2">
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
            <b-col sm="12" md="4" lg="2" class="">
               <form-picker :label="$t('startDate')" v-model="filter.startDate" />
            </b-col>
            <b-col sm="12" md="4" lg="2" class="">
               <form-picker :label="$t('endDate')" v-model="filter.endDate" />
            </b-col>
            <b-col sm="12" md="4" lg="3" class="mt-2">
               <b-input-group class="text-right">
                  <b-form-input v-model="filter.search" :placeholder="$t('search')" />
                  <b-input-group-append>
                     <b-button @click="Refresh" variant="primary">
                        <feather-icon icon="SearchIcon" />
                     </b-button>
                  </b-input-group-append>
               </b-input-group>
            </b-col>
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
                  <b-th rowspan="2">№</b-th>
                  <b-th rowspan="2">
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
                  <b-th rowspan="2">{{ $t('totalApplicationCount') }}</b-th>
                  <b-th rowspan="2">{{ $t('totalApplicationSendToRewiedCount') }}</b-th>
                  <b-th rowspan="2">{{ $t('totalCertificateCount') }}</b-th>
                  <b-th rowspan="2">{{ $t('totalCanceledFromResultCount') }}</b-th>
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
                  <b-td class="text-right">{{ currency(item.totalApplicationCount) }}</b-td>
                  <b-td class="text-right">{{ currency(item.totalApplicationSendCount) }}</b-td>
                  <b-td class="text-right">{{ currency(item.totalCertificateCount) }}</b-td>
                  <b-td class="text-right">{{ currency(item.totalApplicationSendToAniCorruptionCount) }}</b-td>
               </b-tr>
            </b-tbody>
            <b-tfoot v-if="tableData.length > 0">
               <b-tr>
                  <b-th colspan="2">
                     <span class="ml-3">{{ $t('Total') }}</span>
                  </b-th>
                  <b-th class="text-right"> {{ currency(totals.totalApplicationCount) }} </b-th>
                  <b-th class="text-right"> {{ currency(totals.totalApplicationSendCount) }} </b-th>
                  <b-th class="text-right"> {{ currency(totals.totalCertificateCount) }} </b-th>
                  <b-th class="text-right"> {{ currency(totals.totalApplicationSendToAniCorruptionCount) }} </b-th>
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
            totalApplicationCount: 0,
            totalApplicationSendCount: 0,
            totalApplicationSendToOmbusmanCount: 0,
            totalApplicationSendToAniCorruptionCount: 0,
            totalApplicationAccepCount: 0,
            totalApplicationCanceldCount: 0,
            totalApplicationSendToRewiedCount: 0,
            totalCertificateCount: 0,
            totalCanceledFromResultCount: 0
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
         ReportService.GetCorruptionByRegion(this.filter)
            .then((res) => {
               this.tableData = res.data;

               this.totals = {
                  totalApplicationCount: 0,
                  totalApplicationSendCount: 0,
                  totalApplicationSendToOmbusmanCount: 0,
                  totalApplicationSendToAniCorruptionCount: 0,
                  totalApplicationAccepCount: 0,
                  totalApplicationCanceldCount: 0,
                  totalApplicationSendToRewiedCount: 0,
                  totalCertificateCount: 0,
                  totalCanceledFromResultCount: 0
               };

               res.data.forEach((item) => {
                  this.totals.totalApplicationCount += item.totalApplicationCount;
                  this.totals.totalApplicationSendCount += item.totalApplicationSendCount;
                  this.totals.totalApplicationSendToOmbusmanCount += item.totalApplicationSendToOmbusmanCount;
                  this.totals.totalApplicationSendToAniCorruptionCount += item.totalApplicationSendToAniCorruptionCount;

                  this.totals.totalApplicationAccepCount += item.totalApplicationAccepCount;
                  this.totals.totalApplicationCanceldCount += item.totalApplicationCanceldCount;
                  this.totals.totalApplicationSendToRewiedCount += item.totalApplicationSendToRewiedCount;
                  this.totals.totalCertificateCount += item.totalCertificateCount;

                  this.totals.totalCanceledFromResultCount += item.totalCanceledFromResultCount;
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
.report-table {
   thead {
      th {
         text-align: center;
         vertical-align: middle;
      }
   }

   td {
      white-space: nowrap;
      padding: 0.6rem 0.7rem !important;
   }

   .table:not(.table-dark) {
      td,
      th {
         border: 1px solid #ebe9f1 !important;
      }
   }
}

.breadcrumb-item.active {
   color: var(--primary);
   cursor: pointer;
}

.text-nowrap {
   white-space: nowrap !important;
}
</style>
