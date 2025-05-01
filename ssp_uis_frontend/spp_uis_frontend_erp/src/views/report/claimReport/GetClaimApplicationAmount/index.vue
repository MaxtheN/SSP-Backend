<template>
   <b-card>
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
               <div>
                  <label for>{{ $t('Region') }}</label>
                  <v-select
                     :options="Distrectlist"
                     :reduce="(item) => item.value"
                     :placeholder="$t('ChooseBelow')"
                     label="text"
                     v-model="filter.districtId"
                     @input="ChangeDistrict"
                     class="w-100"
                  ></v-select>
               </div>
            </div>
         </b-col>
         <b-col sm="12" md="2">
            <form-picker
               v-model="filter.fromDate"
               @input="Refresh"
               :label="$t('startDate')"
               :placeholder="$t('startDate')"
            />
         </b-col>
         <b-col sm="12" md="2">
            <form-picker v-model="filter.toDate" @input="Refresh" :label="$t('endDate')" :placeholder="$t('endDate')" />
         </b-col>
      </b-row>
      <b-row align-h="between">
         <b-col sm="12" md="8">
            <b-breadcrumb class="mt-1 mb-1">
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

      <b-overlay :show="isBusy">
         <b-table-simple class="report-table" hover small caption-top responsive border>
            <b-thead>
               <b-tr>
                  <b-th style="width: 40px; text-align: center">№</b-th>
                  <b-th>
                     <span v-if="filter.byRegion">
                        {{ $t('region') }}
                     </span>
                     <span v-if="filter.byDistrict">
                        {{ $t('district') }}
                     </span>
                     <span v-if="filter.byContractor">
                        {{ $t('contractor') }}
                     </span></b-th
                  >
                  <b-th>{{ $t('Келиб тушган жами аризалар сони') }}</b-th>
                  <b-th>{{ $t('treatedSum') }}</b-th>
                  <b-th>{{ $t('unidirectionalSum') }}</b-th>
                  <b-th v-if="filter.byContractor">{{ $t('inn') }}</b-th>
                  <b-th v-if="filter.byContractor">{{ $t('ClaimApplicationType') }}</b-th>
               </b-tr>
            </b-thead>
            <b-tbody>
               <b-tr v-for="(item, i) in items" :key="i">
                  <b-td>{{ i + 1 }}</b-td>
                  <b-td>
                     <span v-if="filter.byRegion" @click="SortRegion(item)" style="color: blue; cursor: pointer">
                        {{ item.region }}
                     </span>
                     <span v-if="filter.byDistrict" @click="SortDistrict(item)" style="color: blue; cursor: pointer">
                        {{ item.district }}
                     </span>
                     <span v-if="filter.byContractor">
                        {{ item.contractor }}
                     </span>
                  </b-td>
                  <b-td style="text-align: end">
                     {{ currency(item.totalClaimApplicationAmount) }}
                  </b-td>
                  <b-td style="text-align: end">
                     {{ currency(item.totalChargedAmount) }}
                  </b-td>
                  <b-td style="text-align: end">
                     {{ 0 }}
                  </b-td>
                  <b-td v-if="filter.byContractor">
                     {{ item.contractorInn }}
                  </b-td>
                  <b-td v-if="filter.byContractor">
                     {{ item.claimTheme }}
                  </b-td>
               </b-tr>
            </b-tbody>
            <b-tfoot v-if="items.length > 0">
               <b-th colspan="2">
                  <span class="ml-3">{{ $t('Total') }}</span>
               </b-th>
               <b-th class="text-right">
                  {{ currency(totals.SummTotalClaimApplicationAmount) }}
               </b-th>
               <b-th class="text-right">
                  {{ currency(totals.SummTotalChargedAmount) }}
               </b-th>
               <b-th>
                  <span class="ml-3">{{ 0 }}</span>
               </b-th>
               <b-th v-if="filter.byContractor">
                  <span class="ml-3"></span>
               </b-th>
               <b-th v-if="filter.byContractor">
                  <span class="ml-3"></span>
               </b-th>
            </b-tfoot>
            <template #overlay>
               <div class="text-center text-primary my-2">
                  <b-spinner scale="2" class="align-middle mr-2"></b-spinner>
                  <strong>{{ $t('Loading') }}...</strong>
               </div>
            </template>
         </b-table-simple>
      </b-overlay>
   </b-card>
</template>

<script>
import {
   BOverlay,
   BCard,
   BTableSimple,
   BRow,
   BTh,
   BThead,
   BTr,
   BTd,
   BTbody,
   BCol,
   BBreadcrumb,
   BBreadcrumbItem,
   BTfoot
} from 'bootstrap-vue';
import ReportService from '@/services/report/report.service';
import RegionService from '@/services/info/region.service';
import DistrictService from '@/services/info/district.service';

export default {
   components: {
      BRow,
      BCol,
      BTr,
      BTh,
      BThead,
      BOverlay,
      BTableSimple,
      BCard,
      BTd,
      BTbody,
      BBreadcrumb,
      BBreadcrumbItem,
      BTfoot
   },
   data() {
      return {
         isBusy: false,
         RegionList: [],
         Distrectlist: [],
         items: [],
         filter: {
            district: '',
            region: '',
            regionId: null,
            byRegion: true,
            districtId: null,
            byDistrict: false,
            claimThemeId: null,
            contractorId: null,
            contractorInn: '',
            byContractor: false,
            fromDate: '',
            toDate: ''
         },
         totals: {
            SummTotalClaimApplicationAmount: 0,
            SummTotalChargedAmount: 0
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
   },
   methods: {
      Refresh() {
         this.isBusy = true;
         ReportService.GetClaimApplicationAmount(this.filter)
            .then((res) => {
               this.items = res.data;
               this.totals.SummTotalChargedAmount = 0;
               this.totals.SummTotalClaimApplicationAmount = 0;
               res.data.forEach((item) => {
                  this.totals.SummTotalClaimApplicationAmount += item.totalClaimApplicationAmount;
                  this.totals.SummTotalChargedAmount += item.totalChargedAmount;
               });

               this.isBusy = false;
            })
            .catch((error) => {
               this.showApiError(error);
            });
      },
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
               this.Distrectlist = res.data;
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
               ? this.Distrectlist.filter((item) => item.value === this.filter.districtId)[0].text
               : '';

            this.Refresh();
         } else {
            this.filter.byDistrict = true;
            this.filter.byRegion = false;
            this.filter.byContractor = false;
            this.filter.district = '';
            this.Refresh();
         }
      }
   }
};
</script>

<style lang="scss" scoped>
@import '../../styles.scss';
</style>
