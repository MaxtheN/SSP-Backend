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

            <!-- <b-col sm="12" md="2">
               <div>
                  <label for>{{ $t("mfy") }}</label>
                  <v-select
                     :options="MfyList"
                     :reduce="(item) => item.value"
                     :placeholder="$t('ChooseBelow')"
                     label="text"
                     v-model="filter.mfyId"
                     @input="Refresh"
                     class="w-100"
                  ></v-select>
               </div>
            </b-col>-->
            <b-col sm="12" md="2">
               <form-picker
                  :label="$t('startDate')"
                  v-model="filter.startDate"
                  :placeholder="$t('startDate')"
                  @input="Refresh"
               ></form-picker>
            </b-col>
            <b-col sm="12" md="2">
               <form-picker
                  :label="$t('endDate')"
                  v-model="filter.endDate"
                  :placeholder="$t('endDate')"
                  @input="Refresh"
               ></form-picker>
            </b-col>

            <!-- </b-row>
            </b-col>-->
            <b-col cols="12" md="4">
               <b-input-group class="text-right">
                  <b-form-input v-model="filter.search" :placeholder="$t('search')" />
                  <b-input-group-append>
                     <b-button @click="Refresh" variant="primary">
                        <feather-icon icon="SearchIcon" />
                     </b-button>
                  </b-input-group-append>
               </b-input-group>
            </b-col>
            <b-col sm="12" md="2">
               <b-button @click="Print" variant="primary" class="ml-1">
                  <feather-icon icon="PrinterIcon"></feather-icon>
                  {{ $t('Print') }}
               </b-button>
               <!-- <b-button variant="outline-primary" @click="Refresh" class="ml-2">
                <feather-icon icon="RefreshCwIcon" />
               </b-button>-->
            </b-col>

            <!-- <b-col sm="12" md="2" class="mt-2"></b-col> -->
         </b-row>
         <b-row align-h="between">
            <b-col sm="12" md="8" class="mt-2">
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
         <b-table-simple hover small caption-top responsive border>
            <b-thead>
               <b-tr>
                  <b-th rowspan="3">№</b-th>
                  <b-th rowspan="3" class="table-b-table-default b-table-sticky-column">
                     <span v-show="filter.byRegion">{{ $t('region') }}</span>
                     <span v-show="filter.byDistrict">{{ $t('Region') }}</span>
                     <span v-show="filter.byContractor">{{ $t('contractorT') }}</span>
                  </b-th>
                  <!-- <b-th rowspan="3">Туман номи</b-th>
                  <b-th rowspan="3">Тижорат банки</b-th>-->
                  <b-th colspan="2" rowspan="2">{{ $t('creditTotal') }}</b-th>
                  <b-th colspan="16">{{ $t('ofThem') }}</b-th>
               </b-tr>
               <b-tr>
                  <b-th colspan="2">{{ $t('AGbenk') }}</b-th>
                  <b-th colspan="2">{{ $t('XBbenk') }}</b-th>
                  <b-th colspan="2">{{ $t('MKbenk') }}</b-th>
                  <b-th colspan="2">{{ $t('SQBbenk') }}</b-th>
                  <b-th colspan="2">{{ $t('Kapitalbenk') }}</b-th>
                  <b-th colspan="2">{{ $t('Turonbenk') }}</b-th>
                  <b-th colspan="2">{{ $t('Aloqabenk') }}</b-th>
                  <b-th colspan="2">{{ $t('Ipotekabenk') }}</b-th>
               </b-tr>
               <b-tr>
                  <b-th>{{ $t('count') }}</b-th>
                  <b-th>{{ $t('amount1') }}</b-th>
                  <b-th>{{ $t('count') }}</b-th>
                  <b-th>{{ $t('amount1') }}</b-th>
                  <b-th>{{ $t('count') }}</b-th>
                  <b-th>{{ $t('amount1') }}</b-th>
                  <b-th>{{ $t('count') }}</b-th>
                  <b-th>{{ $t('amount1') }}</b-th>
                  <b-th>{{ $t('count') }}</b-th>
                  <b-th>{{ $t('amount1') }}</b-th>
                  <b-th>{{ $t('count') }}</b-th>
                  <b-th>{{ $t('amount1') }}</b-th>
                  <b-th>{{ $t('count') }}</b-th>
                  <b-th>{{ $t('amount1') }}</b-th>
                  <b-th>{{ $t('count') }}</b-th>
                  <b-th>{{ $t('amount1') }}</b-th>
                  <b-th>{{ $t('count') }}</b-th>
                  <b-th>{{ $t('amount1') }}</b-th>
               </b-tr>
            </b-thead>
            <!-- <b-thead>
               <b-tr>
                  <b-th rowspan="3">{{ $t('order') }}</b-th>
                  <b-th rowspan="3">
                     <span v-show="filter.byRegion">{{ $t('region') }}</span>
                     <span v-show="filter.byDistrict">{{ $t('Region') }}</span>
                     <span v-show="filter.byContractor">{{ $t('contractorT') }}</span>
                  </b-th>
                  <b-th rowspan="3">{{$t('totalTaxCount')}}</b-th>
                  <b-th colspan="12">{{ $t('ofThem') }}</b-th>
               </b-tr>
               <b-tr>
                  <b-th colspan="2">{{ $t('incomeTax') }}</b-th>
                  <b-th colspan="2">{{ $t('SocialTax') }}</b-th>
                  <b-th colspan="2">{{ $t('landTax') }}</b-th>
                  <b-th colspan="2">{{ $t('propertyTax') }}</b-th>
                  <b-th colspan="2">{{ $t('QQSTax') }}</b-th>
                  <b-th colspan="2">{{ $t('QQSreturnTax') }}</b-th>
               </b-tr>
               <b-tr>
                  <b-th>{{ $t('count') }}</b-th>
                  <b-th>{{ $t('amount1') }}</b-th>
                  <b-th>{{ $t('count') }}</b-th>
                  <b-th>{{ $t('amount1') }}</b-th>
                  <b-th>{{ $t('count') }}</b-th>
                  <b-th>{{ $t('amount1') }}</b-th>
                  <b-th>{{ $t('count') }}</b-th>
                  <b-th>{{ $t('amount1') }}</b-th>
                  <b-th>{{ $t('count') }}</b-th>
                  <b-th>{{ $t('amount1') }}</b-th>
                  <b-th>{{ $t('count') }}</b-th>
                  <b-th>{{ $t('amount1') }}</b-th>
               </b-tr>
            </b-thead>-->

            <b-tbody v-if="items.rows.length > 0">
               <b-tr v-for="(item, idx) in items.rows" :key="idx + 'abc'">
                  <b-td>{{ idx + 1 }}</b-td>
                  <b-td class="table-b-table-default b-table-sticky-column">
                     <span v-show="filter.byRegion" style="color: blue; cursor: pointer" @click="SortRegion(item)">{{
                        item.region
                     }}</span>

                     <span
                        v-show="filter.byDistrict"
                        style="color: blue; cursor: pointer"
                        @click="SortDistrict(item)"
                        >{{ item.district }}</span
                     >

                     <span v-show="filter.byContractor">
                        {{ item.contractorInn }} -
                        {{ item.contractor }}
                     </span>

                     <!-- {{ item.region }} -->
                  </b-td>
                  <b-td class="text-right">{{ item.totalApplication.item1 }}</b-td>
                  <b-td class="text-right">{{ item.totalApplication.item2 }}</b-td>
                  <b-td class="text-right">{{ item.countApplication['1'].item1 }}</b-td>
                  <b-td class="text-right">{{ item.countApplication['1'].item2 }}</b-td>
                  <b-td class="text-right">{{ item.countApplication['2'].item1 }}</b-td>
                  <b-td class="text-right">{{ item.countApplication['2'].item2 }}</b-td>
                  <b-td class="text-right">{{ item.countApplication['3'].item1 }}</b-td>
                  <b-td class="text-right">{{ item.countApplication['3'].item2 }}</b-td>

                  <b-td class="text-right">{{ item.totalContract.item1 }}</b-td>
                  <b-td class="text-right">{{ item.totalContract.item2 }}</b-td>
                  <b-td class="text-right">{{ item.countContracts['1'].item1 }}</b-td>
                  <b-td class="text-right">{{ item.countContracts['1'].item2 }}</b-td>
                  <b-td class="text-right">{{ item.countContracts['2'].item1 }}</b-td>
               </b-tr>
            </b-tbody>
            <b-tfoot v-if="items.rows.length > 0">
               <b-tr variant="secondary">
                  <b-td class="text-right" colspan="2">{{ $t('Total') }}</b-td>
                  <b-td class="text-right">{{ items.applicationTotals.item1 }}</b-td>
                  <b-td class="text-right">{{ items.applicationTotals.item2 }}</b-td>
                  <b-td class="text-right">{{ items.applicationColumnTotals['1'].item1 }}</b-td>
                  <b-td class="text-right">{{ items.applicationColumnTotals['1'].item2 }}</b-td>
                  <b-td class="text-right">{{ items.applicationColumnTotals['2'].item1 }}</b-td>
                  <b-td class="text-right">{{ items.applicationColumnTotals['2'].item2 }}</b-td>
                  <b-td class="text-right">{{ items.applicationColumnTotals['3'].item1 }}</b-td>
                  <b-td class="text-right">{{ items.applicationColumnTotals['3'].item2 }}</b-td>

                  <b-td class="text-right">{{ items.contractTotals.item1 }}</b-td>
                  <b-td class="text-right">{{ items.contractTotals.item2 }}</b-td>
                  <b-td class="text-right">{{ items.contractColumnTotals['1'].item1 }}</b-td>
                  <b-td class="text-right">{{ items.contractColumnTotals['1'].item2 }}</b-td>
                  <b-td class="text-right">{{ items.contractColumnTotals['2'].item1 }}</b-td>
               </b-tr>
            </b-tfoot>
         </b-table-simple>
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
   BTfoot
} from 'bootstrap-vue';
import ReportService from '@/services/report/report.service';
import PrtnContractTypeService from '@/services/info/prtncontracttype.service';
import MfyService from '@/services/info/mfy.service';

import RegionService from '@/services/info/region.service';
import DistrictService from '@/services/info/district.service';

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
      BTfoot
   },
   name: 'Index',
   directives: {
      'b-tooltip': VBTooltip,
      'b-modal': VBModal
   },
   data() {
      return {
         items: {
            rows: [],
            contractColumnTotals: {},
            certificateColumnTotals: {},
            applicationColumnTotals: {}
         },
         RegionList: [],
         DistrictList: [],
         PrtnContractTypeList: [],
         MfyList: [],
         lang: 'ru',
         filter: {
            prtnContractTypeId: null,
            regionId: null,
            region: '',
            byRegion: true,
            districtId: null,
            district: '',
            byDistrict: false,
            contractorId: null,
            byContractor: false,
            mfyId: null,
            mfy: '',
            byMfy: false,
            startDate: '',
            endDate: ''
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
         ReportService.PrtnApplicationByContractTypeExcel(this.filter).then((res) => {
            this.forceFileDownload(res, this.$t('getprtnapplicationbycontracttype'));
         });
      },

      Refresh() {
         // this.isBusy = true;
         // ReportService.GetPrtnApplicationByContractType(this.filter)
         //    .then(res => {
         //       this.items = res.data;
         //       this.isBusy = false;
         //    })
         //    .catch(error => {
         //       this.isBusy = false;
         //       this.showApiError(error);
         //    });
      }
   }
};
</script>
