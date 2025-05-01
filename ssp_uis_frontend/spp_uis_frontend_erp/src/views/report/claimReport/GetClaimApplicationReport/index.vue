<template>
   <b-card>
      <b-overlay :show="isBusy">
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
         </b-row>

         <b-row align-h="between">
            <b-col sm="12" md="8">
               <b-breadcrumb class="mt-1 mb-1">
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
               </b-breadcrumb>
            </b-col>
         </b-row>
         <b-table-simple class="report-table" hover small caption-top responsive border>
            <b-thead>
               <b-tr>
                  <b-th rowspan="3">№</b-th>
                  <b-th rowspan="3">
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
                  <b-th rowspan="3">{{ $t('Киритилган даъво аризалар сони') }}</b-th>
                  <b-th rowspan="2" colspan="3">{{ $t('Қиймати') }}</b-th>
                  <b-th rowspan="2" colspan="3">{{ $t('Судлар бўйича') }}</b-th>
                  <b-th colspan="8">{{
                     $t('ССП томонидан аппеляция ва кассация тартибида киритилган шикоятлар')
                  }}</b-th>
                  <b-th rowspan="3">{{ $t('Судгача низони ҳал этиш жараёнида (медиация)  ҳал этилган низолар') }}</b-th>
               </b-tr>
               <b-tr>
                  <b-th colspan="4">{{ $t('Апеляция ва Кассация тартибида кириилган') }}</b-th>
                  <b-th colspan="4">{{ $t('taftish') }}</b-th>
               </b-tr>
               <b-tr>
                  <b-th>{{ $t('Сўм') }}</b-th>
                  <b-th>{{ $t('АҚШ.Долл') }}</b-th>
                  <b-th>{{ $t('Евро') }}</b-th>
                  <b-th>{{ $t('Иқтисодий суди') }}</b-th>
                  <b-th>{{ $t('Фуқаролик суди') }}</b-th>
                  <b-th>{{ $t('Маъмурий суди') }}</b-th>
                  <b-th>{{ $t('Сони') }}</b-th>
                  <b-th>{{ $t('Суммаси') }}</b-th>
                  <b-th>{{ $t('қаноатлантирилган') }}</b-th>
                  <b-th>{{ $t('қаноатлантирилмаган') }}</b-th>
                  <b-th>{{ $t('Сони') }}</b-th>
                  <b-th>{{ $t('Суммаси') }}</b-th>
                  <b-th>{{ $t('қаноатлантирилган') }}</b-th>
                  <b-th>{{ $t('қаноатлантирилмаган') }}</b-th>
               </b-tr>
            </b-thead>
            <b-tbody>
               <b-tr v-for="(item, index) in tableData" :key="index">
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
                     </span>
                  </b-td>
                  <b-td class="text-right">{{ currency(item.totalClaimApplicationCount) }}</b-td>
                  <b-td class="text-right">{{ currency(item.totalClaimApplicationAmount['item1']) }}</b-td>
                  <b-td class="text-right">{{ currency(item.totalClaimApplicationAmount['item2']) }}</b-td>
                  <b-td class="text-right">{{ currency(item.totalClaimApplicationAmount['item3']) }}</b-td>
                  <b-td class="text-right">{{ currency(item.totalEconomicCourt) }}</b-td>
                  <b-td class="text-right">{{ currency(item.totalCivilCourt) }}</b-td>
                  <b-td class="text-right">{{ currency(item.totalAdministrativeCourt) }}</b-td>
                  <b-td class="text-right">{{ currency(item.totalAppilationCount) }}</b-td>
                  <b-td class="text-right">{{ currency(item.totalAppilationAmount) }}</b-td>
                  <b-td class="text-right">{{ currency(item.totalAppilationAcceptedCount) }}</b-td>
                  <b-td class="text-right">{{ currency(item.totalAppilationCanceledCount) }}</b-td>
                  <b-td class="text-right">{{ currency(item.totalRevisionCount) }}</b-td>
                  <b-td class="text-right">{{ currency(item.totalRevisionAmount) }}</b-td>
                  <b-td class="text-right">{{ currency(item.totalRevisionAcceptedCount) }}</b-td>
                  <b-td class="text-right">{{ currency(item.totalRevisionCanceledCount) }}</b-td>
                  <b-td class="text-right">{{ currency(item.totalMediationCount) }}</b-td>
               </b-tr>
            </b-tbody>
            <b-tfoot v-if="tableData.length > 0">
               <b-tr>
                  <b-th colspan="2">
                     <span class="ml-3">{{ $t('Total') }}</span>
                  </b-th>
                  <b-th class="text-right"> {{ currency(totals.totalClaimApplicationCount) }} </b-th>
                  <b-th class="text-right"> {{ currency(totals.totalClaimApplicationAmount.item1) }} </b-th>
                  <b-th class="text-right"> {{ currency(totals.totalClaimApplicationAmount.item2) }} </b-th>
                  <b-th class="text-right"> {{ currency(totals.totalClaimApplicationAmount.item3) }} </b-th>
                  <b-th class="text-right"> {{ currency(totals.totalEconomicCourt) }} </b-th>
                  <b-th class="text-right"> {{ currency(totals.totalCivilCourt) }} </b-th>
                  <b-th class="text-right"> {{ currency(totals.totalAdministrativeCourt) }} </b-th>
                  <b-th class="text-right"> {{ currency(totals.totalAppilationCount) }} </b-th>
                  <b-th class="text-right"> {{ currency(totals.totalAppilationAmount) }} </b-th>
                  <b-th class="text-right"> {{ currency(totals.totalAppilationAcceptedCount) }} </b-th>
                  <b-th class="text-right"> {{ currency(totals.totalAppilationCanceledCount) }} </b-th>
                  <b-th class="text-right"> {{ currency(totals.totalRevisionCount) }} </b-th>
                  <b-th class="text-right"> {{ currency(totals.totalRevisionAmount) }} </b-th>
                  <b-th class="text-right"> {{ currency(totals.totalRevisionAcceptedCount) }} </b-th>
                  <b-th class="text-right"> {{ currency(totals.totalRevisionCanceledCount) }} </b-th>
                  <b-th class="text-right"> {{ currency(totals.totalMediationCount) }} </b-th>
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
         filters: {
            claimApplicationTypeId: null,
            regionId: null,
            byRegion: true,
            districtId: null,
            byDistrict: false,
            contractorId: null,
            byContractor: false,
            region: '',
            district: '',
            isSsp: false
         },
         totals: {
            totalClaimApplicationCount: 0,
            totalClaimApplicationAmount: {
               item1: 0,
               item2: 0,
               item3: 0
            },
            totalEconomicCourt: 0,
            totalCivilCourt: 0,
            totalAdministrativeCourt: 0,
            totalAppilationCount: 0,
            totalAppilationAmount: 0,
            totalAppilationAcceptedCount: 0,
            totalAppilationCanceledCount: 0,
            totalRevisionCount: 0,
            totalRevisionAmount: 0,
            totalRevisionAcceptedCount: 0,
            totalRevisionCanceledCount: 0,
            totalMediationCount: 0
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
      Refresh() {
         this.isBusy = true;
         ReportService.GetClaimApplicationReport({
            ...this.filters,
            regionId: this.filters.regionId == 15 ? null : this.filters.regionId
         })
            .then((res) => {
               this.tableData = res.data;

               this.totals = {
                  totalClaimApplicationCount: 0,
                  totalClaimApplicationAmount: {
                     item1: 0,
                     item2: 0,
                     item3: 0
                  },
                  totalEconomicCourt: 0,
                  totalCivilCourt: 0,
                  totalAdministrativeCourt: 0,
                  totalAppilationCount: 0,
                  totalAppilationAmount: 0,
                  totalAppilationAcceptedCount: 0,
                  totalAppilationCanceledCount: 0,
                  totalRevisionCount: 0,
                  totalRevisionAmount: 0,
                  totalRevisionAcceptedCount: 0,
                  totalRevisionCanceledCount: 0,
                  totalMediationCount: 0
               };

               this.tableData.forEach((item) => {
                  this.totals.totalClaimApplicationCount += item.totalClaimApplicationCount;
                  this.totals.totalClaimApplicationAmount.item1 += item.totalClaimApplicationAmount.item1;
                  this.totals.totalClaimApplicationAmount.item2 += item.totalClaimApplicationAmount.item2;
                  this.totals.totalClaimApplicationAmount.item3 += item.totalClaimApplicationAmount.item3;
                  this.totals.totalEconomicCourt += item.totalEconomicCourt;
                  this.totals.totalCivilCourt += item.totalCivilCourt;
                  this.totals.totalAdministrativeCourt += item.totalAdministrativeCourt;
                  this.totals.totalAppilationCount += item.totalAppilationCount;
                  this.totals.totalAppilationAmount += item.totalAppilationAmount;
                  this.totals.totalAppilationAcceptedCount += item.totalAppilationAcceptedCount;
                  this.totals.totalAppilationCanceledCount += item.totalAppilationCanceledCount;
                  this.totals.totalRevisionCount += item.totalRevisionCount;
                  this.totals.totalRevisionAmount += item.totalRevisionAmount;
                  this.totals.totalRevisionAcceptedCount += item.totalRevisionAcceptedCount;
                  this.totals.totalRevisionCanceledCount += item.totalRevisionCanceledCount;
                  this.totals.totalMediationCount += item.totalMediationCount;
               });
               this.isBusy = false;
            })
            .catch((errors) => {
               this.makeToast(errors.response.data.error, 'danger');
            });
      }
   }
};
</script>
<style lang="scss" scoped>
@import '../../styles.scss';
</style>
