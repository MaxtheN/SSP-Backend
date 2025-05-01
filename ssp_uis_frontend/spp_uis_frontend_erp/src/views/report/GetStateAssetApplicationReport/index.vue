<template>
   <b-card no-body>
      <div class="m-2">
         <b-row>
            <b-col sm="12" md="2">
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
            <b-col cols="12" md="4">
               <label for>{{ $t('search') }}</label>
               <b-input-group class="text-right">
                  <b-form-input v-model="filter.search" :placeholder="$t('search')" />
                  <b-input-group-append>
                     <b-button @click="Refresh" variant="primary">
                        <feather-icon icon="SearchIcon" />
                     </b-button>
                  </b-input-group-append>
               </b-input-group>
            </b-col>
            <b-col sm="12" md="2" class="ml-auto text-right mt-2">
               <b-button @click="Print" variant="primary">
                  <feather-icon icon="PrinterIcon"></feather-icon>
                  {{ $t('Print') }}
               </b-button>
            </b-col>
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
                  <!-- <b-breadcrumb-item v-show="filter.byContractor" active>Baz</b-breadcrumb-item> -->
               </b-breadcrumb>
            </b-col>
         </b-row>
      </div>
      <div class="m-2 report-table">
         <b-overlay :show="isBusy">
            <b-table-simple hover small caption-top responsive border>
               <b-thead>
                  <b-tr>
                     <b-th rowspan="3" style="font-weight: 900; font-size: 14px; color: black">{{ $t('order') }}</b-th>
                     <b-th
                        rowspan="3"
                        class="table-b-table-default"
                        :class="{ 'b-table-sticky-column': !isMobileDevice() }"
                     >
                        <span v-show="filter.byRegion" style="font-weight: 900; font-size: 14px; color: black">{{
                           $t('region')
                        }}</span>
                        <span v-show="filter.byDistrict" style="font-weight: 900; font-size: 14px; color: black">{{
                           $t('Region')
                        }}</span>
                     </b-th>
                     <b-th rowspan="3" style="font-weight: 900; font-size: 14px; color: black">{{
                        $t('certificateCount')
                     }}</b-th>
                     <b-th rowspan="3" style="font-weight: 900; font-size: 14px; color: black">{{
                        $t('newVacanciesCount')
                     }}</b-th>
                     <b-th rowspan="2" colspan="4" style="font-weight: 900; font-size: 14px; color: black">{{
                        $t(
                           'Тадбиркорлик субъектлари аукцион савдоларида кўчмас мулк объектлари ва ерга бўлган ҳуқуқни сотиб олиш учун тўловларни фоизсиз бўлиб тўлаш бўйича'
                        )
                     }}</b-th>
                     <b-th colspan="12" style="font-weight: 900; font-size: 14px; color: black">{{
                        $t('шундан:')
                     }}</b-th>
                  </b-tr>
                  <b-tr>
                     <b-th colspan="4">{{ $t('51-100 та иш ўрни яратадиган') }}</b-th>
                     <b-th colspan="4">{{ $t('101-200 гача иш ўрни яратадиган') }}</b-th>
                     <b-th colspan="4">{{ $t('200 дан ортиқ иш ўрни яратадиган') }}</b-th>
                  </b-tr>
                  <b-tr>
                     <b-th>{{ $t('Бўлиб тўлашдан фойдаланган субъект сони') }}</b-th>
                     <b-th>{{ $t('Иш ўрни сони') }}</b-th>
                     <b-th>{{ $t('Объект сони') }}</b-th>
                     <b-th>{{ $t('Бўлиб тўланадиган сумма') }}</b-th>
                     <b-th>{{ $t('Бўлиб тўлашдан фойдаланган субъект сони') }}</b-th>
                     <b-th>{{ $t('Иш ўрни сони') }}</b-th>
                     <b-th>{{ $t('Объект сони') }}</b-th>
                     <b-th>{{ $t('Бўлиб тўланадиган сумма') }}</b-th>
                     <b-th>{{ $t('Бўлиб тўлашдан фойдаланган субъект сони') }}</b-th>
                     <b-th>{{ $t('Иш ўрни сони') }}</b-th>
                     <b-th>{{ $t('Объект сони') }}</b-th>
                     <b-th>{{ $t('Бўлиб тўланадиган сумма') }}</b-th>
                     <b-th>{{ $t('Бўлиб тўлашдан фойдаланган субъект сони') }}</b-th>
                     <b-th>{{ $t('Иш ўрни сони') }}</b-th>
                     <b-th>{{ $t('Объект сони') }}</b-th>
                     <b-th>{{ $t('Бўлиб тўланадиган сумма   ') }}</b-th>
                  </b-tr>
               </b-thead>
               <b-tbody>
                  <b-tr v-for="(item, idx) in items" :key="idx + 'abc'">
                     <b-td>{{ idx + 1 }}</b-td>
                     <b-td class="table-b-table-default" :class="{ 'b-table-sticky-column': !isMobileDevice() }">
                        <span v-show="filter.byRegion" style="color: blue; cursor: pointer" @click="SortRegion(item)">{{
                           item.regionName
                        }}</span>

                        <span v-show="filter.byDistrict" style="color: blue; cursor: pointer">{{
                           item.districtName
                        }}</span>
                     </b-td>
                     <b-td class="text-right">{{ currency(item.stateAssetType.certificateCount) }}</b-td>
                     <b-td class="text-right">{{ currency(item.stateAssetType.newVacanciesCount) }}</b-td>
                     <b-td class="text-right">{{ currency(item.stateAssetType.certificateCount) }}</b-td>
                     <b-td class="text-right">{{ currency(item.stateAssetType.newVacanciesCount) }}</b-td>
                     <b-td class="text-right">{{ currency(item.stateAssetType.stateAssetApplicationCount) }}</b-td>
                     <b-td></b-td>
                     <b-td class="text-right">{{ currency(item.stateAssetType1.certificateCount) }}</b-td>
                     <b-td class="text-right">{{ currency(item.stateAssetType1.newVacanciesCount) }}</b-td>
                     <b-td class="text-right">{{ currency(item.stateAssetType1.stateAssetApplicationCount) }}</b-td>
                     <b-td></b-td>
                     <b-td class="text-right">{{ currency(item.stateAssetType2.certificateCount) }}</b-td>
                     <b-td class="text-right">{{ currency(item.stateAssetType2.newVacanciesCount) }}</b-td>
                     <b-td class="text-right">{{ currency(item.stateAssetType2.stateAssetApplicationCount) }}</b-td>
                     <b-td></b-td>
                     <b-td class="text-right">{{ currency(item.stateAssetType3.certificateCount) }}</b-td>
                     <b-td class="text-right">{{ currency(item.stateAssetType3.newVacanciesCount) }}</b-td>
                     <b-td class="text-right">{{ currency(item.stateAssetType3.stateAssetApplicationCount) }}</b-td>
                     <b-td></b-td>
                  </b-tr>
               </b-tbody>
               <b-tfoot v-if="items.length > 0">
                  <b-tr variant="secondary">
                     <b-td></b-td>
                     <b-td class="text-center" :class="{ 'b-table-sticky-column': !isMobileDevice() }">{{
                        $t('Total')
                     }}</b-td>
                     <b-td class="text-right">{{ currency(totalsStateAssetType.certificateCount) }}</b-td>
                     <b-td class="text-right">{{ currency(totalsStateAssetType.newVacanciesCount) }}</b-td>
                     <b-td class="text-right">{{ currency(totalsStateAssetType.certificateCount) }}</b-td>
                     <b-td class="text-right">{{ currency(totalsStateAssetType.newVacanciesCount) }}</b-td>
                     <b-td class="text-right">{{ currency(totalsStateAssetType.stateAssetApplicationCount) }}</b-td>
                     <b-td></b-td>
                     <b-td class="text-right">{{ currency(totalsStateAssetType1.certificateCount) }}</b-td>
                     <b-td class="text-right">{{ currency(totalsStateAssetType1.newVacanciesCount) }}</b-td>
                     <b-td class="text-right">{{ currency(totalsStateAssetType1.stateAssetApplicationCount) }}</b-td>
                     <b-td></b-td>
                     <b-td class="text-right">{{ currency(totalsStateAssetType2.certificateCount) }}</b-td>
                     <b-td class="text-right">{{ currency(totalsStateAssetType2.newVacanciesCount) }}</b-td>
                     <b-td class="text-right">{{ currency(totalsStateAssetType2.stateAssetApplicationCount) }}</b-td>
                     <b-td></b-td>
                     <b-td class="text-right">{{ currency(totalsStateAssetType3.certificateCount) }}</b-td>
                     <b-td class="text-right">{{ currency(totalsStateAssetType3.newVacanciesCount) }}</b-td>
                     <b-td class="text-right">{{ currency(totalsStateAssetType3.stateAssetApplicationCount) }}</b-td>
                     <b-td></b-td>
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
   BTfoot,
   BOverlay
} from 'bootstrap-vue';
import ReportService from '@/services/report/report.service';
import PrtnContractTypeService from '@/services/info/prtncontracttype.service';

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
      BTfoot,
      BOverlay
   },
   name: 'Index',
   directives: {
      'b-tooltip': VBTooltip,
      'b-modal': VBModal
   },
   data() {
      return {
         items: [],
         totalsStateAssetType: {
            certificateCount: 0,
            newVacanciesCount: 0,
            stateAssetApplicationCount: 0
         },
         totalsStateAssetType1: {
            certificateCount: 0,
            newVacanciesCount: 0,
            stateAssetApplicationCount: 0
         },
         totalsStateAssetType2: {
            certificateCount: 0,
            newVacanciesCount: 0,
            stateAssetApplicationCount: 0
         },
         totalsStateAssetType3: {
            certificateCount: 0,
            newVacanciesCount: 0,
            stateAssetApplicationCount: 0
         },
         RegionList: [],
         DistrictList: [],
         PrtnContractTypeList: [],
         filter: {
            regionId: null,
            region: '',
            byRegion: true,
            districtId: null,
            district: '',
            byDistrict: false
         },
         isBusy: false
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

      PrtnContractTypeService.GetAsSelectList()
         .then((res) => {
            this.PrtnContractTypeList = res.data;
            this.Refresh();
         })
         .catch((error) => {
            this.showApiError(error);
         });
   },
   methods: {
      SortRegion(item) {
         this.filter.byDistrict = true;
         this.filter.byRegion = false;
         this.filter.regionId = item.regionId;
         this.filter.region = item.region;
         this.GetDistrict(item.regionId);
         this.Refresh();
      },
      // SortDistrict(item) {
      //    this.filter.byDistrict = false;
      //    this.filter.byRegion = false;
      //    this.filter.districtId = item.districtId;
      //    this.filter.district = item.district;
      //    this.Refresh();
      // },
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
      BindValue(value) {
         this.filter.dateofbirth = value;
      },
      Print() {
         ReportService.SaveAsExcelStateAssetApplications(this.filter).then((res) => {
            this.forceFileDownload(res, this.$t('davaktiv'));
         });
      },
      Refresh() {
         this.isBusy = true;
         this.totalsStateAssetType = {
            certificateCount: 0,
            newVacanciesCount: 0,
            stateAssetApplicationCount: 0
         };
         this.totalsStateAssetType1 = {
            certificateCount: 0,
            newVacanciesCount: 0,
            stateAssetApplicationCount: 0
         };
         this.totalsStateAssetType2 = {
            certificateCount: 0,
            newVacanciesCount: 0,
            stateAssetApplicationCount: 0
         };
         this.totalsStateAssetType3 = {
            certificateCount: 0,
            newVacanciesCount: 0,
            stateAssetApplicationCount: 0
         };
         ReportService.GetStateAssetApplicationReport(this.filter)
            .then((res) => {
               this.items = res.data;
               res.data.forEach((item) => {
                  this.totalsStateAssetType.certificateCount += item.stateAssetType.certificateCount;
                  this.totalsStateAssetType.newVacanciesCount += item.stateAssetType.newVacanciesCount;
                  this.totalsStateAssetType.stateAssetApplicationCount +=
                     item.stateAssetType.stateAssetApplicationCount;
                  this.totalsStateAssetType1.certificateCount += item.stateAssetType1.certificateCount;
                  this.totalsStateAssetType1.newVacanciesCount += item.stateAssetType1.newVacanciesCount;
                  this.totalsStateAssetType1.stateAssetApplicationCount +=
                     item.stateAssetType1.stateAssetApplicationCount;

                  this.totalsStateAssetType2.certificateCount += item.stateAssetType2.certificateCount;
                  this.totalsStateAssetType2.newVacanciesCount += item.stateAssetType2.newVacanciesCount;
                  this.totalsStateAssetType2.stateAssetApplicationCount +=
                     item.stateAssetType2.stateAssetApplicationCount;

                  this.totalsStateAssetType3.certificateCount += item.stateAssetType3.certificateCount;
                  this.totalsStateAssetType3.newVacanciesCount += item.stateAssetType3.newVacanciesCount;
                  this.totalsStateAssetType3.stateAssetApplicationCount +=
                     item.stateAssetType3.stateAssetApplicationCount;
               });
            })
            .catch((error) => {
               this.showApiError(error);
            })
            .finally(() => {
               this.isBusy = false;
            });
      }
   }
};
</script>

<style lang="scss" scoped>
@import '../styles.scss';
</style>
