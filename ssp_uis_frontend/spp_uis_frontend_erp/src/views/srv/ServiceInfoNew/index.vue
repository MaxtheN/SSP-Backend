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
                     @input="SortRegion"
                     class="w-100"
                     @option:cleared="Clear"
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
            <b-tabs pills v-model="tabIndex">
               <b-tab
                  :title="$t('pullik')"
                  active
                  @click="
                     () => {
                        tabIndex = 0;
                        Refresh();
                     }
                  "
               >
                  <paid-table :items="items" :totals="totals" :filter="filter" @sortRegion="SortRegion" />
               </b-tab>
               <b-tab
                  :title="$t('tekin')"
                  @click="
                     () => {
                        tabIndex = 1;
                        Refresh();
                     }
                  "
               >
                  <un-paid-table :items="items" :totals="totals" :filter="filter" @sortRegion="SortRegion" />
               </b-tab>
            </b-tabs>
            <b-overlay :show="isBusy">
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
   VBModal,
   BTabs,
   BTab
} from 'bootstrap-vue';
import PaidTable from './PaidTable.vue';
import UnPaidTable from './UnPaidTable.vue';
import RegionService from '@/services/info/region.service';
import ReportService from '@/services/report/report.service';
import { isObject } from '@vueuse/core';

const totalsDef = {
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
   totalMemshipPaymentOrderCount: { totalCount: 0, totalSum: 0, legalAmount: 0, economomyAmount: 0, birjaAmount: 0 }
};

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
      PaidTable,
      UnPaidTable,
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
         totals: { ...totalsDef },
         RegionList: [],

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
         tabIndex: 0,
         isBusy: false,
         PrintLoading: false,
         localStorageData: {}
      };
   },
   watch: {
      selected(value) {
         if (!value) {
            this.filter.regionId = null;
            this.SortRegion(null);
         } else {
            this.filter.regionId = value;
         }
      }
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
      Clear(valid) {
         console.log(valid);
         if (!valid) {
            this.SortRegion(0);
         }
      },
      GetlocalStorageData() {
         this.localStorageData = JSON.parse(localStorage.getItem('user_info'));
         if (this.localStorageData.organizationId != 1) {
            this.filter.regionId = this.localStorageData.organizationRegionId;

            this.filter.byDistrict = true;
            this.filter.byRegion = false;
         }
      },
      SortRegion(item) {
         console.log(item);

         if (item) {
            if (isObject(item)) {
               this.filter.regionId = item.regionId;
               this.filter.region = item.region;
            } else {
               this.filter.regionId = item;
               this.filter.region = this.RegionList.filter((el) => el.value == item)[0].text;
            }
            this.filter.byDistrict = true;
            this.filter.byRegion = false;

            this.Refresh();
         } else {
            this.filter = {
               byRegion: true,
               regionId: null,
               byDistrict: false,
               region: '',
               district: '',
               byContractor: false,
               districtId: null
            };

            this.Refresh();
         }
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
         } else {
            this.filter.districtId = null;
            this.filter.byDistrict = false;
            this.filter.byRegion = true;
            this.filter.region = '';
            this.filter.byContractor = false;
            this.Refresh();
         }
      },

      Refresh() {
         this.items = [];
         this.isBusy = true;
         this.totals = JSON.parse(JSON.stringify(totalsDef));

         if (this.tabIndex == 0) {
            ReportService.GetSrvDeedReport(this.filter)
               .then((res) => {
                  this.items = res.data;
               })
               .catch((error) => {
                  this.showApiError(error);
               })
               .finally(() => {
                  this.isBusy = false;
               });
         } else {
            ReportService.GetSrvFreeDeedReport(this.filter)
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
      },
      Print() {
         this.PrintLoading = true;
         if (this.tabIndex == 0) {
            ReportService.SaveAsExcelGetSrvDeedReport(this.filter)
               .then((res) => {
                  this.forceFileDownload(res, this.$t('ServiceInfo'));
               })
               .catch((error) => {
                  this.showApiError(error);
               })
               .finally(() => {
                  this.PrintLoading = false;
               });
         } else {
            ReportService.SaveAsExcelGetSrvFreeDeedReport(this.filter)
               .then((res) => {
                  this.forceFileDownload(res, this.$t('ServiceInfoFree'));
               })
               .catch((error) => {
                  this.showApiError(error);
               })
               .finally(() => {
                  this.PrintLoading = false;
               });
         }
      }
   }
};
</script>

<style lang="scss" scoped>
@import './styles.scss';
</style>
