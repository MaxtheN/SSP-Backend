<template>
   <b-card no-body>
      <div class="m-2">
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
            <b-col sm="12" md="3">
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
            <b-col cols="12" md="3" class="mt-2">
               <b-button @click="Refresh" variant="primary">
                  <b-spinner v-if="isBusy" small></b-spinner>
                  <feather-icon icon="SearchIcon" />
               </b-button>
               <b-button @click="Print" variant="primary" class="ml-1">
                  <feather-icon icon="PrinterIcon"></feather-icon>
                  {{ $t('Print') }}
               </b-button>
            </b-col>

            <!-- <b-col sm="12" md="2" class="mt-2"></b-col> -->
         </b-row>
         <b-row align-h="between">
            <b-col sm="12" md="8" class="mt-2">
               <b-button-group @click="Refresh" size="sm" class="mr-2">
                  <b-button
                     @click="filter.contractTypeId = null"
                     :variant="null == filter.contractTypeId ? 'primary' : 'outline-primary'"
                     >{{ $t('all') }}</b-button
                  >
                  <b-button
                     v-for="type in PrtnContractTypeList"
                     :key="type.value"
                     @click="filter.contractTypeId = type.value"
                     :variant="type.value == filter.contractTypeId ? 'primary' : 'outline-primary'"
                     >{{ type.text }}</b-button
                  >
               </b-button-group>
               <b-breadcrumb class="mt-2">
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
                  <!-- <b-breadcrumb-item v-show="filter.byContractor" active>Baz</b-breadcrumb-item> -->
               </b-breadcrumb>
            </b-col>
         </b-row>
      </div>
      <div class="m-2 report-table">
         <b-tabs class="nav-tabs nav-justified1" v-model="tab">
            <b-tab :title="$t('Aggregation')"></b-tab>
            <b-tab :title="$t('GreenLane')"> </b-tab>
            <b-tab :title="$t('OnPossibilityInstallments')"> </b-tab>
         </b-tabs>
         <Tables
            :items="items"
            :filter="filter"
            :is-busy="isBusy"
            :itemsTotal="itemsTotal"
            :tab="tab"
            :SortDistrict="SortDistrict"
            :SortRegion="SortRegion"
         />
      </div>
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
   BOverlay,
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
   BTabs,
   BTab
} from 'bootstrap-vue';
import ReportService from '@/services/report/report.service';
import PrtnContractTypeService from '@/services/info/prtncontracttype.service';
import MfyService from '@/services/info/mfy.service';

import RegionService from '@/services/info/region.service';
import DistrictService from '@/services/info/district.service';

import Tables from './components/Tables.vue';
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
      BOverlay,
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
      BTabs,
      BTab,
      Tables
   },
   name: 'Index',
   directives: {
      'b-tooltip': VBTooltip,
      'b-modal': VBModal
   },
   data() {
      return {
         tab: 0,
         items: [],
         RegionList: [],
         DistrictList: [],
         PrtnContractTypeList: [],
         MfyList: [],
         lang: 'ru',
         itemsTotal: {
            certificateCount: 0,
            newVacanciesCount: 0,
            grChanContractorCount: 0,
            grChanNewVacanciesCount: 0,
            appContractorCount: 0,
            rejContractorCount: 0,
            devContractorCount: 0,
            appCount: 0,
            devCount: 0,
            rejCount: 0,
            grChanCount: 0,
            sum: 0
         },
         filter: {
            contractTypeId: null,
            regionId: null,
            byRegion: true,
            districtId: null,
            byDistrict: false,
            contractorId: null,
            contractorInn: null,
            byContractor: false,
            hasCertificate: true
         },
         isBusy: false
      };
   },

   created() {
      this.lang = localStorage.getItem('locale') || 'ru';
      RegionService.GetAsSelectList(211)
         .then((res) => {
            this.RegionList = res.data;
         })
         .catch((error) => {
            this.showApiError(error);
         });

      PrtnContractTypeService.GetAsSelectList()
         .then((res) => {
            this.PrtnContractTypeList = res.data;
            // this.filter.prtnContractTypeId = res.data[0].value;

            this.Refresh();
         })
         .catch((error) => {
            this.showApiError(error);
         });
      // this.Refresh();
   },
   methods: {
      GetMfy() {
         if (this.filter.regionId || this.filter.districtId) {
            MfyService.GetAsSelectList(this.filter.regionId, this.filter.districtId)
               .then((res) => {
                  this.MfyList = res.data;
                  this.Refresh();
               })
               .catch((error) => {
                  this.showApiError(error);
               });
         } else {
            this.filter.mfyId = null;
            this.MfyList = [];
            this.Refresh();
         }
      },

      SortRegion(item) {
         this.filter.byDistrict = true;
         this.filter.byRegion = false;
         this.filter.byContractor = false;
         this.filter.regionId = item.regionId;
         this.filter.region = item.region;
         this.GetDistrict(item.regionId);
         this.Refresh();
      },
      SortDistrict(item) {
         this.filter.byDistrict = false;
         this.filter.byRegion = false;
         this.filter.byContractor = true;
         this.filter.districtId = item.districtId;
         this.filter.district = item.district;
         this.Refresh();
      },
      SortChange(data) {
         this.filter.Sort = data.sortBy;
         this.filter.Order = data.sortDesc ? 'desc' : 'asc';
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
         this.GetMfy();
      },
      GetDistrict(id) {
         DistrictService.GetAsSelectList(id)
            .then((res) => {
               this.DistrictList = res.data;
               this.GetMfy();
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
         this.GetMfy();

         // this.GetDistrict(id);
      },
      BindValue(value) {
         this.filter.dateofbirth = value;
      },
      Print() {
         ReportService.SaveAsExcelCustomsRelief({ ...this.filter, tab: this.tab }).then((res) => {
            this.forceFileDownload(res, this.$t('getprtnapplicationbycontracttype'));
         });
      },

      Refresh() {
         this.isBusy = true;
         this.itemsTotal = {
            certificateCount: 0,
            newVacanciesCount: 0,
            grChanContractorCount: 0,
            grChanNewVacanciesCount: 0,
            appContractorCount: 0,
            rejContractorCount: 0,
            devContractorCount: 0,
            appCount: 0,
            devCount: 0,
            rejCount: 0,
            grChanCount: 0,
            sum: 0
         };
         ReportService.GetBojxonaImtiyozReportByContractor(this.filter)
            .then((res) => {
               this.items = res.data;
               res.data.forEach((item) => {
                  this.itemsTotal.certificateCount += item.certificateCount * 1;
                  this.itemsTotal.newVacanciesCount += item.newVacanciesCount * 1;
                  this.itemsTotal.grChanContractorCount += item.grChanContractorCount * 1;
                  this.itemsTotal.appContractorCount += item.appContractorCount * 1;
                  this.itemsTotal.grChanNewVacanciesCount += item.grChanNewVacanciesCount * 1;
                  this.itemsTotal.rejContractorCount += item.rejContractorCount * 1;
                  this.itemsTotal.devContractorCount += item.devContractorCount * 1;
                  this.itemsTotal.appCount += item.appCount * 1;
                  this.itemsTotal.devCount += item.devCount * 1;
                  this.itemsTotal.rejCount += item.rejCount * 1;
                  this.itemsTotal.grChanCount += item.grChanCount * 1;
                  this.itemsTotal.sum += item.sum * 1;
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
