<template>
   <b-card no-body>
      <div class="mr-2 ml-2 mt-2">
         <b-tabs class="nav-tab">
            <b-tab :title="$t('variant-1')">
               <div class="mt-2">
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
                     <b-col class="text-right">
                        <b-button
                           variant="primary"
                           @click="downloadExcel"
                           :disabled="PrintLoading"
                           :class="{ 'mt-2': isMobileDevice() }"
                        >
                           <b-icon-printer /> {{ $t('Print') }}
                        </b-button>
                     </b-col>
                  </b-row>
                  <b-row align-h="between" class="overflow-auto">
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

               <div class="mt-2 mb-2">
                  <b-table
                     ref="refInvoiceListTable"
                     :items="items"
                     responsive
                     :fields="fields"
                     primary-key="id"
                     sticky-header="65vh"
                     no-border-collapse
                     no-footer-sorting
                     :busy="isBusy"
                     show-empty
                     :empty-text="$t('NotFound')"
                     class="position-relative"
                     foot-clone
                     @sort-changed="SortChange"
                  >
                     <template #cell(order)="{ index }">
                        <span>{{ index + 1 }}</span>
                     </template>
                     <template #cell(region)="{ item }">
                        <span style="color: blue; cursor: pointer" @click="SortRegion(item)">{{ item.region }}</span>
                     </template>

                     <template #cell(contractor)="{ item }">
                        <span style="color: blue; cursor: pointer" @click="goToBussnes(item.contractorInn)">{{
                           item.contractorInn
                        }}</span>
                        -
                        {{ item.contractor }}
                     </template>

                     <template #cell(district)="{ item }">
                        <span style="color: blue; cursor: pointer" @click="SortDistrict(item)">{{
                           item.district
                        }}</span>
                     </template>

                     <template #cell(totalDocCount)="{ item }">
                        {{ currency(item.totalDocCount) }}
                     </template>

                     <template #cell(totalForeignInvestment)="{ item }">
                        {{ currency(item.totalForeignInvestment, 2) }}
                     </template>

                     <template #cell(totalProjectCost)="{ item }">
                        {{ currency(item.totalProjectCost, 2) }}
                     </template>

                     <template #cell(totalPrivillageBankCredit)="{ item }">
                        {{ currency(item.totalPrivillageBankCredit, 2) }}
                     </template>

                     <template #cell(totalOwnInvestment)="{ item }">
                        {{ currency(item.totalOwnInvestment, 2) }}
                     </template>

                     <!-- footer -->

                     <template #foot(contractorCount)>
                        <span>{{ currency(totals.contractorCount) }}</span>
                     </template>

                     <template #foot(projectCost)>
                        <span>{{ currency(totals.projectCost, 2) }}</span>
                     </template>

                     <template #foot(ownInvestment)>
                        <span>{{ currency(totals.ownInvestment, 2) }}</span>
                     </template>
                     <template #foot(foreignInvestment)>
                        <span>{{ currency(totals.foreignInvestment, 2) }}</span>
                     </template>
                     <template #foot(privilegeBankCredit)>
                        <span>
                           {{ currency(totals.privilegeBankCredit, 2) }}
                        </span>
                     </template>

                     <template #foot(order)>
                        <span>{{ $t('Total') }}</span>
                     </template>
                     <template #foot(district)>
                        <span></span>
                     </template>
                     <template #foot(region)>
                        <span></span>
                     </template>
                     <template #foot(contractor)>
                        <span></span>
                     </template>
                     <template #foot(contractType)>
                        <span></span>
                     </template>
                     <template #foot(contractorInn)>
                        <span></span>
                     </template>
                     <template #foot(contractorPhoneNumber)>
                        <span></span>
                     </template>
                  </b-table>
               </div>
            </b-tab>
            <b-tab :title="$t('variant-2')">
               <div class="mt-2">
                  <b-row>
                     <b-col sm="12" md="3">
                        <div>
                           <label for>{{ $t('Oblast') }}</label>
                           <v-select
                              :options="RegionList"
                              :reduce="(item) => item.value"
                              :placeholder="$t('ChooseBelow')"
                              label="text"
                              v-model="filterPaged.regionId"
                              @input="ChangeRegionPaged"
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
                              v-model="filterPaged.districtId"
                              @input="ChangeDistrictPaged"
                              class="w-100"
                           ></v-select>
                        </div>
                     </b-col>
                     <b-col cols="12" md="4">
                        <label for>{{ $t('inn') }}</label>
                        <b-input-group class="text-right">
                           <b-form-input v-model="filterPaged.contractorInn" :placeholder="$t('search')" />
                           <b-input-group-append>
                              <b-button @click="RefreshPaged" variant="primary">
                                 <feather-icon icon="SearchIcon" />
                              </b-button>
                           </b-input-group-append>
                        </b-input-group>
                     </b-col>
                     <b-col class="text-right">
                        <b-button
                           variant="primary"
                           @click="downloadExcelPaged"
                           :disabled="PrintLoadingPaged"
                           :class="{ 'mt-2': isMobileDevice() }"
                        >
                           <b-icon-printer /> {{ $t('Print') }}
                        </b-button>
                     </b-col>
                  </b-row>
                  <b-row align-h="between">
                     <b-col sm="12" md="8" class="mt-2">
                        <b-button-group @click="RefreshPaged" size="sm" class="mr-2">
                           <b-button
                              @click="filterPaged.contractTypeId = null"
                              :variant="null == filterPaged.contractTypeId ? 'primary' : 'outline-primary'"
                              >{{ $t('all') }}</b-button
                           >
                           <b-button
                              v-for="type in PrtnContractTypeList"
                              :key="type.value"
                              @click="filterPaged.contractTypeId = type.value"
                              :variant="type.value == filterPaged.contractTypeId ? 'primary' : 'outline-primary'"
                              >{{ type.text }}</b-button
                           >
                        </b-button-group>

                        <b-breadcrumb class="mt-2">
                           <b-breadcrumb-item
                              :active="filterPaged.byRegion"
                              @click="
                                 () => {
                                    filterPaged.byDistrict = false;
                                    filterPaged.byRegion = true;
                                    filterPaged.byContractor = false;
                                    filterPaged.region = '';
                                    filterPaged.regionId = null;
                                    filterPaged.district = '';
                                    filterPaged.districtId = null;
                                    RefreshPaged();
                                 }
                              "
                           >
                              <b>{{ $t('uzb') }}</b>
                           </b-breadcrumb-item>
                           <b-breadcrumb-item
                              v-show="filterPaged.region"
                              :active="filterPaged.byDistrict"
                              @click="
                                 () => {
                                    filterPaged.byDistrict = true;
                                    filterPaged.byRegion = false;
                                    filterPaged.byContractor = false;
                                    filterPaged.district = '';
                                    filterPaged.districtId = null;
                                    RefreshPaged();
                                 }
                              "
                           >
                              <b>{{ filterPaged.region }}</b>
                           </b-breadcrumb-item>
                           <b-breadcrumb-item v-show="filterPaged.district" :active="filterPaged.byContractor">
                              <b>{{ filterPaged.district }}</b>
                           </b-breadcrumb-item>
                        </b-breadcrumb>
                     </b-col>
                  </b-row>
               </div>

               <div class="mt-2 mb-2">
                  <b-table
                     ref="refInvoiceListTable"
                     responsive
                     primary-key="id"
                     :fields="fieldsPaged"
                     :items="itemsPaged"
                     sticky-header="65vh"
                     no-border-collapse
                     no-footer-sorting
                     :busy="isBusy"
                     show-empty
                     :empty-text="$t('NotFound')"
                     class="position-relative"
                  >
                     <template #cell(order)="{ index }">
                        <span>{{ index + 1 }}</span>
                     </template>
                     <template #cell(contractor)="{ item }">
                        <span style="color: blue; cursor: pointer" @click="goToBussnes(item.contractorInn)">{{
                           item.contractorInn
                        }}</span>
                        -
                        {{ item.contractor }}
                     </template>
                     <template #cell(totalProjectCost)="{ item }">
                        {{ currency(item.totalProjectCost) }}
                     </template>
                     <template #cell(totalPrivillageBankCredit)="{ item }">
                        {{ currency(item.totalPrivillageBankCredit) }}
                     </template>
                     <template #cell(totalOwnInvestment)="{ item }">
                        {{ currency(item.totalOwnInvestment) }}
                     </template>
                     <template #cell(totalForeignInvestment)="{ item }">
                        {{ currency(item.totalForeignInvestment) }}
                     </template>
                  </b-table>
               </div>
               <div class="mx-2 mb-2">
                  <b-row>
                     <b-col
                        cols="12"
                        sm="6"
                        class="d-flex align-items-center justify-content-center justify-content-sm-start"
                     >
                        <span class="text-muted">
                           {{ $t('Showing') }} {{ firstNumber }} {{ $t('to') }} {{ lastNumber }} {{ $t('of') }}
                           {{ filterPaged.total }}
                           {{ $t('entries') }}
                        </span>
                        <v-select
                           v-model="filterPaged.pageSize"
                           :dir="$store.state.appConfig.isRTL ? 'rtl' : 'ltr'"
                           :options="filterPaged.pageOptions"
                           :clearable="false"
                           @input="RefreshPaged"
                           class="per-page-selector d-inline-block ml-50 mr-1"
                        />
                     </b-col>
                     <!-- Pagination -->
                     <b-col
                        cols="12"
                        sm="6"
                        class="d-flex align-items-center justify-content-center justify-content-sm-end"
                     >
                        <b-pagination
                           v-model="filterPaged.page"
                           :total-rows="filterPaged.total"
                           :per-page="filterPaged.pageSize"
                           first-number
                           last-number
                           @input="RefreshPaged"
                           class="mb-0 mt-1 mt-sm-0"
                           prev-class="prev-item"
                           next-class="next-item"
                        >
                           <template #prev-text>
                              <feather-icon icon="ChevronLeftIcon" size="18" />
                           </template>
                           <template #next-text>
                              <feather-icon icon="ChevronRightIcon" size="18" />
                           </template>
                        </b-pagination>
                     </b-col>
                  </b-row>
               </div>
            </b-tab>
         </b-tabs>
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
   BIconPrinter,
   BTabs,
   BTab
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
      BIconPrinter,
      BTabs,
      BTab
   },
   directives: {
      'b-tooltip': VBTooltip,
      'b-modal': VBModal
   },
   data() {
      return {
         PrintLoading: false,
         PrintLoadingPaged: false,
         items: [],
         fieldsPaged: [],
         itemsPaged: [],
         RegionList: [],
         DistrictList: [],
         PrtnContractTypeList: [],
         fields: [],
         lang: 'ru',
         filter: {
            contractTypeId: null,
            regionId: null,
            region: '',
            byRegion: true,
            districtId: null,
            district: '',
            byDistrict: false,
            contractorId: null,
            byContractor: false
         },
         filterPaged: {
            contractorInn: this.contractorInnD,
            contractTypeId: null,
            regionId: null,
            region: '',
            byRegion: true,
            districtId: null,
            district: '',
            byDistrict: true,
            contractorId: null,
            byContractor: true,
            page: 1,
            pageSize: 20,
            pageOptions: [10, 20, 50, 100],
            total: 0
         },
         isBusy: false,
         totals: {
            contractorCount: 0,
            projectCost: 0,
            ownInvestment: 0,
            foreignInvestment: 0,
            privilegeBankCredit: 0
         }
      };
   },
   computed: {
      firstNumber() {
         return (this.filterPaged.page - 1) * this.filterPaged.pageSize + 1;
      },
      lastNumber() {
         if (this.filterPaged.total < this.filterPaged.pageSize) {
            return this.filterPaged.total;
         } else {
            if (this.filterPaged.page * this.filterPaged.pageSize > this.filterPaged.total) {
               return this.filterPaged.total;
            } else {
               return this.filterPaged.page * this.filterPaged.pageSize;
            }
         }
      },
      contractorInnD() {
         return this.filterPaged.contractorInn == '' ? 'salom88' : this.filterPaged.contractorInn;
      }
   },
   // watch: {
   //    'filterPaged.contractorInn': function (newValue) {
   //       if (newValue == '') {
   //          this.fieldsPaged.contractorInn = null;
   //          console.log(this.fieldsPaged.contractorInn);
   //          console.log(this.fieldsPaged);
   //       }
   //    }
   // },
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
            this.Refresh();
            this.RefreshPaged();
         })
         .catch((error) => {
            this.showApiError(error);
         });
      this.getFields();
      this.getFieldsPaged();
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
               key: this.filter.byContractor ? 'contractorPhoneNumber' : '',
               label: this.$t('phoneNumber'),
               sortable: true
            },
            {
               key: this.filter.byContractor ? 'contractType' : '',
               label: this.$t('prtnContractType'),
               sortable: true
            },
            {
               key: 'contractorCount',
               label: this.$t('totalDocCount'),
               thClass: 'text-right',
               tdClass: 'text-right text-nowrap',
               sortable: true
            },
            {
               key: 'projectCost',
               label: this.$t('totalProjectCost'),
               thClass: 'text-right',
               tdClass: 'text-right text-nowrap',
               sortable: true
            },
            {
               key: 'ownInvestment',
               label: this.$t('totalOwnInvestment'),
               thClass: 'text-right',
               tdClass: 'text-right text-nowrap',
               sortable: true
            },
            {
               key: 'foreignInvestment',
               label: this.$t('totalForeignInvestment'),
               thClass: 'text-right',
               tdClass: 'text-right text-nowrap',
               sortable: true
            },
            {
               key: 'privilegeBankCredit',
               label: this.$t('totalPrivillageBankCredit'),
               thClass: 'text-right',
               tdClass: 'text-right text-nowrap',
               sortable: true
            }
         ];
      },
      getFieldsPaged() {
         this.fieldsPaged = [
            {
               key: 'order',
               label: this.$t('№'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: this.filterPaged.byRegion ? 'region' : '',
               label: this.$t('Oblast'),
               sortable: true,
               stickyColumn: !this.isMobileDevice()
            },
            {
               key: this.filterPaged.byDistrict ? 'district' : '',
               label: this.$t('Region'),
               sortable: true,
               stickyColumn: !this.isMobileDevice()
            },
            {
               key: 'contractor',
               label: this.$t('contractorT'),
               sortable: true,
               stickyColumn: !this.isMobileDevice(),
               thStyle: {
                  minWidth: '350px'
               }
            },
            {
               key: 'contractorPhoneNumber',
               label: this.$t('phoneNumber'),
               sortable: true
            },
            {
               key: 'contractType',
               label: this.$t('prtnContractType'),
               sortable: true
            },
            {
               key: 'totalDocCount',
               label: this.$t('totalDocCount'),
               thClass: 'text-right',
               tdClass: 'text-right text-nowrap',
               sortable: true
            },
            {
               key: 'totalProjectCost',
               label: this.$t('totalProjectCost'),
               thClass: 'text-right',
               tdClass: 'text-right text-nowrap',
               sortable: true
            },
            {
               key: 'totalOwnInvestment',
               label: this.$t('totalOwnInvestment'),
               thClass: 'text-right',
               tdClass: 'text-right text-nowrap',
               sortable: true
            },
            {
               key: 'totalForeignInvestment',
               label: this.$t('totalForeignInvestment'),
               thClass: 'text-right',
               tdClass: 'text-right text-nowrap',
               sortable: true
            },
            {
               key: 'totalPrivillageBankCredit',
               label: this.$t('totalPrivillageBankCredit'),
               thClass: 'text-right',
               tdClass: 'text-right text-nowrap',
               sortable: true
            }
         ];
      },
      goToBussnes(inn) {
         this.$router.push({ name: 'BusinessmanCard', query: { inn: inn } });
      },
      SortRegion(item) {
         this.filter.byDistrict = true;
         this.filter.byRegion = false;
         this.filter.byContractor = true;
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
            this.filter.byContractor = true;
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
      ChangeRegionPaged(id) {
         if (id) {
            this.filterPaged.districtId = null;
            this.filterPaged.byDistrict = true;
            this.filterPaged.byRegion = false;
            this.filterPaged.byContractor = true;
            this.filterPaged.region = this.filterPaged.regionId
               ? this.RegionList.filter((item) => item.value === this.filterPaged.regionId)[0].text
               : '';
            this.RefreshPaged();
            this.GetDistrict(id);
         } else {
            this.filterPaged.districtId = null;
            this.filterPaged.byDistrict = false;
            this.filterPaged.byRegion = true;
            this.filterPaged.region = '';
            this.filterPaged.byContractor = false;
            this.RefreshPaged();
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
            this.filter.byRegion = true;
            this.filter.byContractor = true;
            this.filter.district = this.filter.districtId
               ? this.DistrictList.filter((item) => item.value === this.filter.districtId)[0].text
               : '';
            this.Refresh();
         } else {
            this.filter.byDistrict = true;
            this.filter.byRegion = true;
            this.filter.byContractor = true;
            this.filter.district = '';
            this.Refresh();
         }
      },
      ChangeDistrictPaged(id) {
         if (id) {
            this.filterPaged.byDistrict = false;
            this.filterPaged.byRegion = true;
            this.filterPaged.byContractor = true;
            this.filterPaged.district = this.filterPaged.districtId
               ? this.DistrictList.filter((item) => item.value === this.filterPaged.districtId)[0].text
               : '';
            this.RefreshPaged();
         } else {
            this.filterPaged.byDistrict = true;
            this.filterPaged.byRegion = true;
            this.filterPaged.byContractor = true;
            this.filterPaged.district = '';
            this.RefreshPaged();
         }
      },
      downloadExcel() {
         this.PrintLoading = true;

         ReportService.SaveAsExecelForPrtnCreditDemand(this.filter)
            .then((res) => {
               this.makeToast(this.$t('SuccessMessage'), 'success');
               this.forceFileDownload(res, this.$t('GetPrtnCreditDemandInfo'));
            })
            .catch((error) => {
               this.showApiError(error);
            })
            .finally(() => {
               this.PrintLoading = false;
            });
      },
      downloadExcelPaged() {
         this.PrintLoadingPaged = true;
         ReportService.SaveAsExecelForPrtnCreditDemandPaged(this.filterPaged)
            .then((res) => {
               this.makeToast(this.$t('SuccessMessage'), 'success');
               this.forceFileDownload(res, this.$t('GetPrtnCreditDemandInfo'));
            })
            .catch((error) => {
               this.showApiError(error);
            })
            .finally(() => {
               this.PrintLoadingPaged = false;
            });
      },
      Refresh() {
         this.getFields();
         this.isBusy = true;
         ReportService.GetPrtnCreditDemandInfoByBank(this.filter)
            .then((res) => {
               this.items = res.data;

               this.totals = {
                  contractorCount: 0,
                  projectCost: 0,
                  ownInvestment: 0,
                  foreignInvestment: 0,
                  privilegeBankCredit: 0
               };
               res.data.forEach((item) => {
                  this.totals.contractorCount += item.contractorCount;
                  this.totals.projectCost += item.projectCost;
                  this.totals.ownInvestment += item.ownInvestment;
                  this.totals.foreignInvestment += item.foreignInvestment;
                  this.totals.privilegeBankCredit += item.privilegeBankCredit;
               });
            })
            .catch((error) => {
               this.showApiError(error);
            })
            .finally(() => {
               this.isBusy = false;
            });
      },
      RefreshPaged() {
         this.getFieldsPaged();
         this.isBusy = true;

         if (!this.filterPaged.contractorInn) {
            this.filterPaged.contractorInn = null;
         }

         ReportService.GetPrtnCreditDemandInfoPaged(this.filterPaged)
            .then((res) => {
               this.itemsPaged = res.data.rows;
               this.filterPaged.total = res.data.total;
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
