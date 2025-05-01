<template>
   <div>
      <b-card>
         <b-row>
            <b-col sm="12" md="2">
               <div>
                  <label for>{{ $t('region') }}</label>
                  <v-select
                     :options="RegionList"
                     :disabled="localStorageData.organizationId != 1"
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
            <b-col sm="12" md="2">
               <div>
                  <form-picker
                     v-model="filter.fromDocDate"
                     :label="$t('startDate')"
                     :placeholder="$t('startDate')"
                     @change="Refresh"
                  />
               </div>
            </b-col>
            <b-col sm="12" md="2">
               <div>
                  <form-picker
                     v-model="filter.toDocDate"
                     :label="$t('endDate')"
                     :placeholder="$t('endDate')"
                     @change="Refresh"
                  />
               </div>
            </b-col>
            <b-col> </b-col>

            <b-col class="col-auto text-right mt-2">
               <b-button @click="Print" :disabled="PrintLoading" variant="primary">
                  <feather-icon icon="PrinterIcon"></feather-icon>
                  {{ $t('Print') }}
               </b-button>
            </b-col>
         </b-row>
         <b-row align-h="between">
            <b-col sm="12" md="8">
               <b-breadcrumb class="mt-1">
                  <b-breadcrumb-item
                     :active="filter.byRegion"
                     @click="
                        () => {
                           filter.byDistrict = false;
                           filter.byRegion = true;
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
         <div class="report-table mt-1">
            <b-overlay :show="isBusy">
               <paid-table
                  :items="items"
                  :totals="totals"
                  :filter="filter"
                  @sortRegion="SortRegion($event)"
                  @sortDistrict="SortDistrict($event)"
               ></paid-table>
               <template #overlay>
                  <div class="text-center text-primary my-2">
                     <b-spinner scale="2" class="align-middle mr-2"></b-spinner>
                     <strong>{{ $t('Loading') }}...</strong>
                  </div>
               </template>
            </b-overlay>
         </div>
      </b-card>
   </div>
</template>

<script>
import {
   BButton,
   BCol,
   BRow,
   BSpinner,
   BCard,
   BBreadcrumb,
   BBreadcrumbItem,
   BOverlay,
   VBTooltip,
   VBModal
} from 'bootstrap-vue';
import PaidTable from './PaidTable.vue';

import ServiceInfoService from '@/services/srv/ServiceInfo.service';
import RegionService from '@/services/info/region.service';
import DistrictService from '@/services/info/district.service';

export default {
   components: {
      BButton,
      BCol,
      BRow,
      BSpinner,
      BCard,
      BBreadcrumb,
      BBreadcrumbItem,
      BOverlay,
      PaidTable
   },
   directives: {
      'b-tooltip': VBTooltip,
      'b-modal': VBModal
   },
   data() {
      return {
         items: [],
         totals: {
            paidCoef: 0,
            freecoef: 0,
            planPaidSum: 0,
            legalAmount: 0,
            planFreeCount: 0,
            planPaidCount: 0,
            economomyAmount: 0,
            acceptedFreeCount: 0,
            acceptedFreePercentage: 0,
            acceptedPaidSumPercentage: 0,
            acceptedPaidCountPercentage: 0,
            totalMemshipPaymentOrderCount: {
               totalCount: 0,
               totalSum: 0,
               legalAmount: 0,
               economomyAmount: 0,
               birjaAmount: 0
            }
         },
         RegionList: [],
         DistrictList: [],
         filter: {
            byRegion: true,
            regionId: null,
            byDistrict: false,
            region: '',
            district: '',
            byContractor: false,
            districtId: null,
            fromDocDate: new Date(new Date().getFullYear(), 0, 1).toLocaleDateString('ru-RU'),
            toDocDate: new Date().toLocaleDateString('ru-RU')
         },
         isBusy: false,
         PrintLoading: false,
         localStorageData: {}
      };
   },
   created() {
      this.GetlocalStorageData();
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
      GetlocalStorageData() {
         this.localStorageData = JSON.parse(localStorage.getItem('user_info'));
         if (this.localStorageData.organizationId != 1) {
            this.filter.regionId = this.localStorageData.organizationRegionId;
            this.GetDistrict(this.localStorageData.organizationRegionId);
            this.filter.byDistrict = true;
            this.filter.byRegion = false;
         }
      },
      SortRegion(item) {
         this.filter.byDistrict = true;
         this.filter.byRegion = false;
         this.filter.regionId = item.regionId;
         this.filter.region = item.region;
         this.GetDistrict(item.regionId);
         this.Refresh();
      },
      SortDistrict(item) {
         this.filter.byDistrict = false;
         this.filter.byRegion = false;
         this.filter.regionId = item.regionId;
         this.filter.region = item.region;
         this.filter.byContractor = true;
         this.filter.district = item.district;
         this.filter.districtId = item.districtId;
         // this.GetDistrict(item.regionId);
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
         this.totals = {
            paidCoef: 0,
            freecoef: 0,
            planPaidSum: 0,
            legalAmount: 0,
            planFreeCount: 0,
            planPaidCount: 0,
            economomyAmount: 0,
            acceptedFreeCount: 0,
            acceptedFreePercentage: 0,
            acceptedPaidSumPercentage: 0,
            acceptedPaidCountPercentage: 0,
            totalMemshipPaymentOrderCount: {
               totalCount: 0,
               totalSum: 0,
               legalAmount: 0,
               economomyAmount: 0,
               birjaAmount: 0
            }
         };

         ServiceInfoService.GetList(this.filter)
            .then((res) => {
               this.items = res.data;
               res.data.forEach((item) => {
                  this.totals.freecoef += item.freecoef;
                  this.totals.paidCoef += item.paidCoef;
                  this.totals.planPaidSum += item.planPaidSum;
                  this.totals.legalAmount += item.legalAmount;
                  this.totals.planFreeCount += item.planFreeCount;
                  this.totals.planPaidCount += item.planPaidCount;
                  this.totals.economomyAmount += item.economomyAmount;
                  this.totals.acceptedFreeCount += item.acceptedFreeCount;
                  this.totals.acceptedFreePercentage += item.acceptedFreePercentage;
                  this.totals.acceptedPaidSumPercentage += item.acceptedPaidSumPercentage;
                  this.totals.acceptedPaidCountPercentage += item.acceptedPaidCountPercentage;
                  this.totals.totalMemshipPaymentOrderCount.totalSum += item.totalMemshipPaymentOrderCount.totalSum;
                  this.totals.totalMemshipPaymentOrderCount.totalCount += item.totalMemshipPaymentOrderCount.totalCount;
                  this.totals.totalMemshipPaymentOrderCount.legalAmount +=
                     item.totalMemshipPaymentOrderCount.legalAmount;
                  this.totals.totalMemshipPaymentOrderCount.economomyAmount +=
                     item.totalMemshipPaymentOrderCount.economomyAmount;
                  this.totals.totalMemshipPaymentOrderCount.birjaAmount +=
                     item.totalMemshipPaymentOrderCount.birjaAmount;
               });
            })
            .catch((error) => {
               this.showApiError(error);
            })
            .finally(() => {
               this.isBusy = false;
            });
      },
      Print() {
         this.PrintLoading = true;
         ServiceInfoService.SaveAsExcelSrvServiceGetInfo(this.filter)
            .then((res) => {
               this.forceFileDownload(res, this.$t('ServiceInfo'));
            })
            .catch((error) => {
               this.showApiError(error);
            })
            .finally(() => {
               this.PrintLoading = false;
            });
      }
   }
};
</script>

<style lang="scss" scoped>
@import './styles.scss';
</style>
