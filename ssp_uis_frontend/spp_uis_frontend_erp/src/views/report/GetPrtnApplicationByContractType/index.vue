<template>
   <b-card>
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
         <b-col sm="12" md="2">
            <div>
               <form-select
                  :options="OkedTypeList"
                  v-model="filter.okedTypeId"
                  @input="Refresh"
                  :label="$t('okedType')"
               ></form-select>
            </div>
         </b-col>
         <b-col sm="12" md="2">
            <form-picker
               :label="$t('startDate')"
               v-model="filter.startDate"
               :placeholder="$t('startDate')"
               @update:modelValue="Refresh"
            ></form-picker>
         </b-col>
         <b-col sm="12" md="2">
            <form-picker
               :label="$t('endDate')"
               v-model="filter.endDate"
               :placeholder="$t('endDate')"
               @update:modelValue="Refresh"
            ></form-picker>
         </b-col>
      </b-row>
      <b-row>
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
         <b-col cols="12" md="8" class="text-right">
            <b-dropdown
               dropleft
               :disabled="PrintLoading"
               id="dropdown-right"
               :text="$t('Print')"
               variant="primary"
               class="m-2"
            >
               <b-dropdown-item @click="() => Print(1)" href="#">{{
                  $t('getprtnapplicationbycontracttype')
               }}</b-dropdown-item>
               <b-dropdown-item @click="() => Print(2)" href="#">{{
                  $t('Imtiyozlar boyicha (tadbirkorlar kesimida)')
               }}</b-dropdown-item>
               <b-dropdown-item @click="() => Print(3)" href="#">{{ $t('okedType') }}</b-dropdown-item>
               <b-dropdown-item @click="() => Print(4)" href="#">{{
                  $t('Imtiyozlar boyicha (hudud kesimida)')
               }}</b-dropdown-item>
            </b-dropdown>
         </b-col>
         <b-col sm="12" md="12">
            <b-breadcrumb>
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
      <b-row align-h="between"> </b-row>
      <div class="mt-2 report-table">
         <b-overlay :show="isBusy">
            <b-table-simple hover small caption-top responsive border>
               <b-thead>
                  <b-tr>
                     <b-th rowspan="3">{{ $t('order') }}</b-th>
                     <b-th
                        rowspan="3"
                        class="table-b-table-default"
                        :class="{ 'b-table-sticky-column': !isMobileDevice() }"
                     >
                        <span v-show="filter.byRegion">{{ $t('region') }}</span>
                        <span v-show="filter.byDistrict">{{ $t('Region') }}</span>
                        <span v-show="filter.byContractor">{{ $t('contractorT') }}</span>
                     </b-th>
                     <b-th colspan="2" rowspan="2">{{ $t('TotalAplication') }}</b-th>
                     <b-th colspan="6">{{ $t('ofThem') }}</b-th>
                     <b-th colspan="2" rowspan="2">{{ $t('totalContractCreated') }}</b-th>
                     <b-th colspan="6">{{ $t('ofThem') }}</b-th>
                     <b-th colspan="2" rowspan="2">{{ $t('totalPrtnApplicationPassExpertisesCount') }}</b-th>
                     <b-th colspan="6">{{ $t('ofThem') }}</b-th>
                     <b-th colspan="2" rowspan="2">{{ $t('totalPrtnApplicationSignedCount') }}</b-th>
                     <b-th colspan="6">{{ $t('ofThem') }}</b-th>
                     <th colspan="2" rowspan="2">{{ $t('givenCertificate') }}</th>
                     <th colspan="6">{{ $t('ofThem') }}</th>
                  </b-tr>
                  <b-tr>
                     <b-th colspan="2">{{ $t('count51to100') }}</b-th>
                     <b-th colspan="2">{{ $t('count101to200') }}</b-th>
                     <b-th colspan="2">{{ $t('count200') }}</b-th>

                     <b-th colspan="2">{{ $t('count51to100') }}</b-th>
                     <b-th colspan="2">{{ $t('count101to200') }}</b-th>
                     <b-th colspan="2">{{ $t('count200') }}</b-th>

                     <b-th colspan="2">{{ $t('count51to100') }}</b-th>
                     <b-th colspan="2">{{ $t('count101to200') }}</b-th>
                     <b-th colspan="2">{{ $t('count200') }}</b-th>

                     <b-th colspan="2">{{ $t('count51to100') }}</b-th>
                     <b-th colspan="2">{{ $t('count101to200') }}</b-th>
                     <b-th colspan="2">{{ $t('count200') }}</b-th>

                     <b-th colspan="2">{{ $t('count51to100') }}</b-th>
                     <b-th colspan="2">{{ $t('count101to200') }}</b-th>
                     <b-th colspan="2">{{ $t('count200') }}</b-th>
                  </b-tr>
                  <b-tr>
                     <b-th>{{ $t('count') }}</b-th>
                     <b-th>{{ $t('workerCount') }}</b-th>
                     <b-th>{{ $t('count') }}</b-th>
                     <b-th>{{ $t('workerCount') }}</b-th>
                     <b-th>{{ $t('count') }}</b-th>
                     <b-th>{{ $t('workerCount') }}</b-th>
                     <b-th>{{ $t('count') }}</b-th>
                     <b-th>{{ $t('workerCount') }}</b-th>

                     <b-th>{{ $t('count') }}</b-th>
                     <b-th>{{ $t('workerCount') }}</b-th>
                     <b-th>{{ $t('count') }}</b-th>
                     <b-th>{{ $t('workerCount') }}</b-th>
                     <b-th>{{ $t('count') }}</b-th>
                     <b-th>{{ $t('workerCount') }}</b-th>
                     <b-th>{{ $t('count') }}</b-th>
                     <b-th>{{ $t('workerCount') }}</b-th>

                     <b-th>{{ $t('count') }}</b-th>
                     <b-th>{{ $t('workerCount') }}</b-th>
                     <b-th>{{ $t('count') }}</b-th>
                     <b-th>{{ $t('workerCount') }}</b-th>
                     <b-th>{{ $t('count') }}</b-th>
                     <b-th>{{ $t('workerCount') }}</b-th>
                     <b-th>{{ $t('count') }}</b-th>
                     <b-th>{{ $t('workerCount') }}</b-th>

                     <b-th>{{ $t('count') }}</b-th>
                     <b-th>{{ $t('workerCount') }}</b-th>
                     <b-th>{{ $t('count') }}</b-th>
                     <b-th>{{ $t('workerCount') }}</b-th>
                     <b-th>{{ $t('count') }}</b-th>
                     <b-th>{{ $t('workerCount') }}</b-th>
                     <b-th>{{ $t('count') }}</b-th>
                     <b-th>{{ $t('workerCount') }}</b-th>

                     <b-th>{{ $t('count') }}</b-th>
                     <b-th>{{ $t('workerCount') }}</b-th>
                     <b-th>{{ $t('count') }}</b-th>
                     <b-th>{{ $t('workerCount') }}</b-th>
                     <b-th>{{ $t('count') }}</b-th>
                     <b-th>{{ $t('workerCount') }}</b-th>
                     <b-th>{{ $t('count') }}</b-th>
                     <b-th>{{ $t('workerCount') }}</b-th>
                  </b-tr>
               </b-thead>
               <b-tbody v-if="items.rows.length > 0">
                  <b-tr v-for="(item, idx) in items.rows" :key="idx + 'abc'">
                     <b-td>{{ idx + 1 }}</b-td>
                     <b-td class="table-b-table-default" :class="{ 'b-table-sticky-column': !isMobileDevice() }">
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

                     <b-td class="text-right">{{ item.totalContractCreated.item1 }}</b-td>
                     <b-td class="text-right">{{ item.totalContractCreated.item2 }}</b-td>
                     <b-td class="text-right">{{ item.countContractsCreated['1'].item1 }}</b-td>
                     <b-td class="text-right">{{ item.countContractsCreated['1'].item2 }}</b-td>
                     <b-td class="text-right">{{ item.countContractsCreated['2'].item1 }}</b-td>
                     <b-td class="text-right">{{ item.countContractsCreated['2'].item2 }}</b-td>
                     <b-td class="text-right">{{ item.countContractsCreated['3'].item1 }}</b-td>
                     <b-td class="text-right">{{ item.countContractsCreated['3'].item2 }}</b-td>

                     <b-td class="text-right">{{ item.totalContract.item1 }}</b-td>
                     <b-td class="text-right">{{ item.totalContract.item2 }}</b-td>
                     <b-td class="text-right">{{ item.countContracts['1'].item1 }}</b-td>
                     <b-td class="text-right">{{ item.countContracts['1'].item2 }}</b-td>
                     <b-td class="text-right">{{ item.countContracts['2'].item1 }}</b-td>
                     <b-td class="text-right">{{ item.countContracts['2'].item2 }}</b-td>
                     <b-td class="text-right">{{ item.countContracts['3'].item1 }}</b-td>
                     <b-td class="text-right">{{ item.countContracts['3'].item2 }}</b-td>

                     <b-td class="text-right">{{ item.totalContractSigned.item1 }}</b-td>
                     <b-td class="text-right">{{ item.totalContractSigned.item2 }}</b-td>
                     <b-td class="text-right">{{ item.countContractsSigned['1'].item1 }}</b-td>
                     <b-td class="text-right">{{ item.countContractsSigned['1'].item2 }}</b-td>
                     <b-td class="text-right">{{ item.countContractsSigned['2'].item1 }}</b-td>
                     <b-td class="text-right">{{ item.countContractsSigned['2'].item2 }}</b-td>
                     <b-td class="text-right">{{ item.countContractsSigned['3'].item1 }}</b-td>
                     <b-td class="text-right">{{ item.countContractsSigned['3'].item2 }}</b-td>

                     <b-td class="text-right">{{ item.totalCertificate.item1 }}</b-td>
                     <b-td class="text-right">{{ item.totalCertificate.item2 }}</b-td>
                     <b-td class="text-right">{{ item.countCertificates['1'].item1 }}</b-td>
                     <b-td class="text-right">{{ item.countCertificates['1'].item2 }}</b-td>
                     <b-td class="text-right">{{ item.countCertificates['2'].item1 }}</b-td>
                     <b-td class="text-right">{{ item.countCertificates['2'].item2 }}</b-td>
                     <b-td class="text-right">{{ item.countCertificates['3'].item1 }}</b-td>
                     <b-td class="text-right">{{ item.countCertificates['3'].item2 }}</b-td>
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

                     <b-td class="text-right">{{ items.contractCreatedTotals.item1 }}</b-td>
                     <b-td class="text-right">{{ items.contractCreatedTotals.item2 }}</b-td>
                     <b-td class="text-right">{{ items.contractColumnThatCreatedTotals['1'].item1 }}</b-td>
                     <b-td class="text-right">{{ items.contractColumnThatCreatedTotals['1'].item2 }}</b-td>
                     <b-td class="text-right">{{ items.contractColumnThatCreatedTotals['2'].item1 }}</b-td>
                     <b-td class="text-right">{{ items.contractColumnThatCreatedTotals['2'].item2 }}</b-td>
                     <b-td class="text-right">{{ items.contractColumnThatCreatedTotals['3'].item1 }}</b-td>
                     <b-td class="text-right">{{ items.contractColumnThatCreatedTotals['3'].item2 }}</b-td>

                     <b-td class="text-right">{{ items.contractTotals.item1 }}</b-td>
                     <b-td class="text-right">{{ items.contractTotals.item2 }}</b-td>
                     <b-td class="text-right">{{ items.contractColumnTotals['1'].item1 }}</b-td>
                     <b-td class="text-right">{{ items.contractColumnTotals['1'].item2 }}</b-td>
                     <b-td class="text-right">{{ items.contractColumnTotals['2'].item1 }}</b-td>
                     <b-td class="text-right">{{ items.contractColumnTotals['2'].item2 }}</b-td>
                     <b-td class="text-right">{{ items.contractColumnTotals['3'].item1 }}</b-td>
                     <b-td class="text-right">{{ items.contractColumnTotals['3'].item2 }}</b-td>

                     <b-td class="text-right">{{ items.contractSignedTotals.item1 }}</b-td>
                     <b-td class="text-right">{{ items.contractSignedTotals.item2 }}</b-td>
                     <b-td class="text-right">{{ items.contractColumnThatSignedTotals['1'].item1 }}</b-td>
                     <b-td class="text-right">{{ items.contractColumnThatSignedTotals['1'].item2 }}</b-td>
                     <b-td class="text-right">{{ items.contractColumnThatSignedTotals['2'].item1 }}</b-td>
                     <b-td class="text-right">{{ items.contractColumnThatSignedTotals['2'].item2 }}</b-td>
                     <b-td class="text-right">{{ items.contractColumnThatSignedTotals['3'].item1 }}</b-td>
                     <b-td class="text-right">{{ items.contractColumnThatSignedTotals['3'].item2 }}</b-td>

                     <b-td class="text-right">{{ items.certificateTotals.item1 }}</b-td>
                     <b-td class="text-right">{{ items.certificateTotals.item2 }}</b-td>
                     <b-td class="text-right">{{ items.certificateColumnTotals['1'].item1 }}</b-td>
                     <b-td class="text-right">{{ items.certificateColumnTotals['1'].item2 }}</b-td>
                     <b-td class="text-right">{{ items.certificateColumnTotals['2'].item1 }}</b-td>
                     <b-td class="text-right">{{ items.certificateColumnTotals['2'].item2 }}</b-td>
                     <b-td class="text-right">{{ items.certificateColumnTotals['3'].item1 }}</b-td>
                     <b-td class="text-right">{{ items.certificateColumnTotals['3'].item2 }}</b-td>
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
   BDropdownItem,
   BDropdown,
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
   BOverlay,
   BTabs,
   BTab
} from 'bootstrap-vue';
import ReportService from '@/services/report/report.service';
import PrtnContractTypeService from '@/services/info/prtncontracttype.service';
import RegionService from '@/services/info/region.service';
import DistrictService from '@/services/info/district.service';
import ManualService from '@/services/others/manual.service';
export default {
   components: {
      BDropdownItem,
      BDropdown,
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
      BTabs,
      BTab
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
         pechatList: [
            {
               value: 1,
               text: this.$t('getprtnapplicationbycontracttype'),
               orderCode: '01'
            },
            {
               value: 2,
               text: this.$t('Imtiyozlar boyicha (tadbirkorlar kesimida)'),
               orderCode: '02'
            },
            {
               value: 3,
               text: this.$t('okedType'),
               orderCode: '03'
            },
            {
               value: 4,
               text: this.$t('Imtiyozlar boyicha (hudud kesimida)'),
               orderCode: '03'
            }
         ],
         pechatId: 1,
         RegionList: [],
         DistrictList: [],
         PrtnContractTypeList: [],
         OkedTypeList: [],
         fields: [],
         PrintLoading: false,
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
            startDate: '',
            endDate: '',
            mfyId: null,
            byMfy: false
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
      ManualService.OkedTypeSelectList()
         .then((res) => {
            this.OkedTypeList = res.data;
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
      this.getFields();
   },
   methods: {
      getFields() {
         this.fields = [
            {
               key: 'order',
               label: this.$t('№'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: this.filter.byRegion ? 'region' : '',
               label: this.$t('Oblast'),
               sortable: true,
               stickyColumn: !this.isMobileDevice()
            },
            {
               key: this.filter.byDistrict ? 'district' : '',
               label: this.$t('Region'),
               sortable: true,
               stickyColumn: !this.isMobileDevice()
            },
            {
               key: this.filter.byContractor ? 'contractor' : '',
               label: this.$t('contractorT'),
               sortable: true,
               stickyColumn: !this.isMobileDevice(),
               thStyle: {
                  minWidth: '350px'
               }
            },
            {
               key: this.filter.byContractor ? 'prtnContractType' : '',
               label: this.$t('prtnContractType'),
               sortable: true
            },

            {
               key: 'totalApplicationCount',
               label: this.$t('totalApplicationCount'),
               thClass: 'text-right',
               tdClass: 'text-right',
               sortable: true
            },

            {
               key: 'totalPrtnApplicationSentCount',
               label: this.$t('totalPrtnApplicationCount'),
               thClass: 'text-right',
               tdClass: 'text-right',
               sortable: true
            },

            {
               key: 'totalPrtnApplicationSentForReviewCount',
               label: this.$t('totalPrtnApplicationSentForReviewCount'),
               thClass: 'text-right',
               tdClass: 'text-right',
               sortable: true
            },
            {
               key: 'totalPrtnApplicationSentRejectedCount',
               label: this.$t('totalPrtnApplicationSentRejectedCount'),
               thClass: 'text-right',
               tdClass: 'text-right',
               sortable: true
            },

            {
               key: 'totalPrtnContractCount',
               label: this.$t('totalPrtnContractCount'),
               thClass: 'text-right',
               tdClass: 'text-right',
               sortable: true
            },

            {
               key: 'totalPrtnApplicationSentForExpertisesCount',
               label: this.$t('totalPrtnApplicationSentForExpertisesCount'),
               thClass: 'text-right',
               tdClass: 'text-right',
               sortable: true
            },

            {
               key: 'totalPrtnApplicationNotPassExpertisesCount',
               label: this.$t('totalPrtnApplicationNotPassExpertisesCount'),
               thClass: 'text-right',
               tdClass: 'text-right',
               sortable: true
            },

            {
               key: 'totalPrtnApplicationPassExpertisesCount',
               label: this.$t('totalPrtnApplicationPassExpertisesCount'),
               thClass: 'text-right',
               tdClass: 'text-right',
               sortable: true
            },
            {
               key: 'totalPrtnContractCancelCount',
               label: this.$t('totalPrtnContractCancelCount'),
               thClass: 'text-right',
               tdClass: 'text-right',
               sortable: true
            },
            {
               key: 'totalPrtnApplicationSignningCount',
               label: this.$t('totalPrtnApplicationSignningCount'),
               thClass: 'text-right',
               tdClass: 'text-right',
               sortable: true
            },
            {
               key: 'totalPrtnApplicationSignedCount',
               label: this.$t('totalPrtnApplicationSignedCount'),
               thClass: 'text-right',
               tdClass: 'text-right',
               sortable: true
            },

            {
               key: 'totalPrtnCertificateCount',
               label: this.$t('totalPrtnCertificateCount'),
               thClass: 'text-right',
               tdClass: 'text-right',
               sortable: true
            },

            {
               key: 'totalNewVacanciesCount',
               label: this.$t('totalNewVacanciesCount'),
               thClass: 'text-right',
               tdClass: 'text-right',
               sortable: true
            }
         ];
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
      Print(item) {
         if (item == 1) {
            ReportService.PrtnApplicationByContractTypeExcel(this.filter)
               .then((res) => {
                  this.forceFileDownload(res, this.$t('getprtnapplicationbycontracttype'));
               })
               .catch((error) => this.showApiError(error));
         } else if (item == 2) {
            this.PrintLoading = true;
            ReportService.SaveAsExcelAllIntegrationReportByContractor()
               .then((res) => {
                  this.forceFileDownload(res, this.$t('IntegrationReportByContractor'));
                  this.PrintLoading = false;
               })
               .catch((error) => this.showApiError(error));
         } else if (item == 4) {
            this.PrintLoading = true;
            ReportService.SaveAsExecelByContractType(this.filter)
               .then((res) => {
                  this.forceFileDownload(res, this.$t('IntegrationReportByContractor'));
                  this.PrintLoading = false;
               })
               .catch((error) => this.showApiError(error))
               .finally(() => {
                  this.PrintLoading = false;
               });
         } else {
            this.PrintLoading = true;
            ReportService.SaveAsExcelSummaryReportByokedTypes(this.filter)
               .then((res) => {
                  this.forceFileDownload(res, this.$t('IntegrationReportByContractor'));
                  this.PrintLoading = false;
               })
               .catch((error) => this.showApiError(error))
               .finally(() => {
                  this.PrintLoading = false;
               });
         }
      },

      Refresh() {
         this.getFields();
         this.isBusy = true;
         ReportService.GetPrtnApplicationByContractType(this.filter)
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
   }
};
</script>

<style lang="scss" scoped>
@import '../styles.scss';
</style>
