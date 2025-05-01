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
            <b-col sm="12" md="3">
               <div>
                  <label for>{{ $t('mfy') }}</label>
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
            </b-col>
            <!-- <b-col cols="12" md="3">
               <validation-observer ref="ValidationAcceptDTO" class="d-flex justify-content-between g-1">
                  <form-picker
                     required
                     type="year"
                     class="w-25"
                     format="YYYY"
                     @input="Refresh"
                     v-model="filter.yearIn"
                     :label="$t('docyear')"
                  />
                  
                  <form-select
                     required-star
                     class="w-50 mr-1"
                     :options="MonthList"
                     v-model="filter.month"
                     @input="Refresh"
                     label="month"
                  />
               </validation-observer>
            </b-col> -->
            <b-col sm="12" md="2">
               <form-input mask="#########" v-model="filter.contractorInn" :label="$t('inn')" />
            </b-col>
            <b-col cols="12" md="2">
               <div>
                  <label for>{{ $t('startdate') }}</label>
                  <form-picker v-model="filter.startDate" :placeholder="$t('startdate')" @input="Refresh" />
               </div>
            </b-col>
            <b-col cols="12" md="2">
               <div>
                  <label for>{{ $t('enddate') }}</label>
                  <form-picker v-model="filter.endDate" :placeholder="$t('enddate')" @input="Refresh" />
               </div>
            </b-col>
            <b-col cols="12" md="3" class="mt-2">
               <b-input-group class="text-right">
                  <b-form-input v-model="filter.search" :placeholder="$t('search')" />
                  <b-input-group-append>
                     <b-button @click="Refresh" variant="primary">
                        <feather-icon icon="SearchIcon" />
                     </b-button>
                  </b-input-group-append>
               </b-input-group>
            </b-col>

            <b-col class="mt-2 d-flex col justify-content-end">
               <b-button @click="modalShow = true" :disabled="PrintLoading2" variant="success" class="ml-1">
                  <b-spinner v-if="PrintLoading2" small></b-spinner>

                  <feather-icon v-else icon="PrinterIcon"></feather-icon>
                  {{ $t('Print') }} ({{ $t('variant-1') }})
               </b-button>

               <b-button @click="Print" :disabled="PrintLoading" variant="primary" class="ml-1">
                  <b-spinner v-if="PrintLoading" small></b-spinner>

                  <feather-icon v-else icon="PrinterIcon"></feather-icon>
                  {{ $t('Print') }}
               </b-button>
            </b-col>
         </b-row>
         <b-row>
            <b-col sm="12" md="8" class="mt-2">
               <b-button-group @click="Refresh" size="sm" class="mr-2">
                  <b-button
                     @click="filter.contractorTypeId = null"
                     :variant="null == filter.contractorTypeId ? 'primary' : 'outline-primary'"
                     >{{ $t('all') }}</b-button
                  >
                  <b-button
                     v-for="type in PrtnContractTypeList"
                     :key="type.value"
                     @click="filter.contractorTypeId = type.value"
                     :variant="type.value == filter.contractorTypeId ? 'primary' : 'outline-primary'"
                     >{{ type.text }}</b-button
                  >
               </b-button-group></b-col
            >
         </b-row>
         <b-modal v-model="modalShow" static no-close-on-backdrop>
            <validation-observer ref="ValidationAcceptDTO" class="d-flex justify-content-between g-1">
               <b-row style="width: 100%">
                  <b-col md="6" sm="6">
                     <form-picker required type="year" format="YYYY" v-model="filter.yearIn" :label="$t('docyear')" />
                  </b-col>
                  <b-col md="6" sm="6">
                     <form-select required-star :options="MonthList" v-model="filter.month" label="month"
                  /></b-col>
                  <b-col md="6" sm="6">
                     <b-form-checkbox v-model="filter.byOrganization">{{
                        $t('byOrganization')
                     }}</b-form-checkbox></b-col
                  >
               </b-row>
            </validation-observer>
            <template #modal-footer="{ cancel }">
               <b-button size="sm" variant="success" @click="Print2"> OK </b-button>
               <b-button size="sm" variant="danger" @click="cancel()"> Cancel </b-button>
            </template>
         </b-modal>
      </div>
      <div class="mx-2">
         <b-overlay :show="isBusy">
            <b-table-simple hover small caption-top responsive bordered sticky-header="75vh">
               <b-thead>
                  <b-tr>
                     <b-th rowspan="3">№</b-th>
                     <b-th rowspan="3">Вилоят номи</b-th>
                     <b-th rowspan="3">Туман номи</b-th>
                     <b-th rowspan="3">МФЙ номи</b-th>
                     <b-th rowspan="3">Корхона номи</b-th>
                     <b-th rowspan="3">СТИР (ИНН)</b-th>
                     <b-th rowspan="3"> Хисобот бўйича 2023 йил 1 июнь холатида мавжуд ходимлар сони </b-th>
                     <b-th colspan="3" rowspan="2" class="text-center">ЖАМИ</b-th>
                     <b-th colspan="3" rowspan="2" class="text-center">ЖАМИ {{ filter.yearIn }} йил</b-th>
                     <b-th colspan="36">шундан</b-th>
                  </b-tr>
                  <b-tr style="position: sticky; top: 41px">
                     <b-th colspan="3" v-for="(month, idx) in items.columns" :key="idx + 'columnmonth'">{{
                        month
                     }}</b-th>
                  </b-tr>

                  <b-tr style="position: sticky; top: 81.9px">
                     <!-- years -->
                     <b-th>режа график</b-th>
                     <b-th>хисобот бўйича</b-th>
                     <b-th>Фарқи</b-th>

                     <b-th>режа график</b-th>
                     <b-th>хисобот бўйича</b-th>
                     <b-th>Фарқи</b-th>

                     <template v-for="(month, idx) in items.columns">
                        <!-- <span :key="idx+'columnmonthitem'"> -->
                        <b-th :key="idx + 1 + 'columnmonthitem'">режа график</b-th>
                        <b-th :key="idx + 2 + 'columnmonthitem'">хисобот бўйича</b-th>
                        <b-th :key="idx + 3 + 'columnmonthitem'">Фарқи</b-th>
                        <!-- </span> -->
                     </template>
                  </b-tr>
               </b-thead>

               <b-tbody v-if="items.rows.length > 0">
                  <b-tr v-for="(item, idx) in items.rows" :key="idx + 'rows'">
                     <b-td>{{ idx + 1 }}</b-td>
                     <b-td>{{ item.region }}</b-td>
                     <b-td>{{ item.district }}</b-td>
                     <b-td>{{ item.mfy }}</b-td>
                     <b-td>{{ item.contractorName }}</b-td>
                     <b-td>{{ item.contractorInn }}</b-td>
                     <b-td class="text-right">{{ currency(item.employeesCountUntilFounded) }}</b-td>

                     <!-- years -->
                     <b-td class="text-right">{{ currency(item.years.item1) }}</b-td>
                     <b-td class="text-right">{{ currency(item.years.item2) }}</b-td>
                     <b-td class="text-right">{{ currency(item.years.item3) }}</b-td>

                     <b-td class="text-right">{{ currency(item.rowsTotal.item1) }}</b-td>
                     <b-td class="text-right">{{ currency(item.rowsTotal.item2) }}</b-td>
                     <b-td class="text-right">{{ currency(item.rowsTotal.item3) }}</b-td>

                     <template v-for="(itemValue, propertyName, index) in items.columns">
                        <b-td :key="index + 'AitemValue' + idx" class="text-right">{{
                           currency(item?.rowsMonthly[propertyName]?.item1)
                        }}</b-td>
                        <b-td :key="index + 'BitemValue' + idx" class="text-right">{{
                           currency(item?.rowsMonthly[propertyName]?.item2)
                        }}</b-td>
                        <b-td :key="index + 'CitemValue' + idx" class="text-right">{{
                           currency(item?.rowsMonthly[propertyName]?.item3)
                        }}</b-td>
                     </template>
                  </b-tr>
               </b-tbody>
            </b-table-simple>
         </b-overlay>

         <div class="mx-2 mb-2">
            <b-row>
               <b-col
                  cols="12"
                  sm="6"
                  class="d-flex align-items-center justify-content-center justify-content-sm-start"
               >
                  <span class="text-muted">
                     {{ $t('Showing') }} {{ firstNumber }} {{ $t('to') }} {{ lastNumber }} {{ $t('of') }}
                     {{ filter.total }}
                     {{ $t('entries') }}
                  </span>
                  <v-select
                     v-model="filter.pageSize"
                     :dir="$store.state.appConfig.isRTL ? 'rtl' : 'ltr'"
                     :options="filter.perPageOptions"
                     :clearable="false"
                     @input="Refresh"
                     class="per-page-selector d-inline-block ml-50 mr-1"
                  />
               </b-col>
               <!-- Pagination -->
               <b-col cols="12" sm="6" class="d-flex align-items-center justify-content-center justify-content-sm-end">
                  <b-pagination
                     v-model="filter.page"
                     :total-rows="filter.total"
                     :per-page="filter.pageSize"
                     first-number
                     last-number
                     @input="Refresh"
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
   BOverlay
} from 'bootstrap-vue';
import ReportService from '@/services/report/report.service';
import RegionService from '@/services/info/region.service';
import DistrictService from '@/services/info/district.service';
import MfyService from '@/services/info/mfy.service';
import ManualService from '@/services/others/manual.service';
import PrtnContractTypeService from '@/services/info/prtncontracttype.service';

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
      BOverlay
   },
   name: 'Index',
   directives: {
      'b-tooltip': VBTooltip,
      'b-modal': VBModal
   },
   data() {
      return {
         items: {
            columns: {},
            rows: [],
            total: {},
            totalEmployeesCountUntilFounded: 0,
            totalMonthly: []
         },
         MonthList: [],
         RegionList: [],
         DistrictList: [],
         PrtnContractTypeList: [],
         MfyList: [],
         a: true,
         fields: [],
         modalShow: false,
         filter: {
            yearIn: 2023,
            regionId: null,
            districtId: null,
            contractorInn: '',
            startDate: '',
            endDate: '',
            mfyId: null,
            contractorId: null,
            search: '',
            sortBy: '',
            orderType: 'asc',
            page: 1,
            pageSize: 20,
            perPageOptions: [10, 20, 50, 100, 500],
            total: 0
         },
         isBusy: false,

         PrintLoading: false,
         PrintLoading2: false
      };
   },
   computed: {
      firstNumber() {
         return (this.filter.page - 1) * this.filter.pageSize + 1;
      },
      lastNumber() {
         if (this.filter.total < this.filter.pageSize) {
            return this.filter.total;
         } else {
            if (this.filter.page * this.filter.pageSize > this.filter.total) {
               return this.filter.total;
            } else {
               return this.filter.page * this.filter.pageSize;
            }
         }
      },
      MonthList2() {
         if (this.filter.yearIn == Number(new Date().getFullYear())) {
            const nowMonth = Number(new Date().getMonth());
            return 8;
         } else {
            return this.MonthList;
         }
      }
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
      this.Refresh();

      ManualService.GetMonthSelectList().then((res) => {
         this.MonthList = res.data;
         console.log(this.MonthList);
      });
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
                  console.log(error);
                  this.showApiError(error);
               });
         } else {
            this.filter.mfyId = null;
            this.MfyList = [];
            this.Refresh();
         }
      },
      goToBussnes(inn) {
         this.$router.push({ name: 'BusinessmanCard', query: { inn: inn } });
      },

      ChangeRegion(id) {
         this.filter.districtId = null;
         this.filter.mfyId = null;
         if (id) {
            this.Refresh();
            this.GetDistrict(id);
         } else {
            this.DistrictList = [];
            this.MfyList = [];
            this.Refresh();
         }
      },
      ChangeDistrict(id) {
         this.filter.mfyId = null;

         if (id) {
            this.Refresh();
         } else {
            this.MfyList = [];
            this.Refresh();
         }
         this.GetMfy();
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

      BindValue(value) {
         this.filter.dateofbirth = value;
      },
      Print() {
         this.PrintLoading = true;
         ReportService.PrtnEmploymentGraphExcel(this.filter)
            .then((res) => {
               this.forceFileDownload(res, this.$t('prtnapplicationandcontractinfo'));
            })
            .finally(() => {
               this.PrintLoading = false;
            });
      },
      Print2() {
         this.$refs.ValidationAcceptDTO.validate().then((success) => {
            if (success) {
               this.PrintLoading2 = true;
               this.modalShow = false;
               ReportService.SaveAsExcelAllIntegrationReportByRegion({
                  year: +this.filter.yearIn,
                  month: this.filter.month,
                  regionId: this.filter.regionId,
                  byOrganization: this.filter.byOrganization,
                  contractorTypeId: this.filter.contractorTypeId
               })
                  .then((res) => {
                     this.forceFileDownload(res, this.$t('prtnapplicationandcontractinfo'));
                     this.filter.month = '';
                  })
                  .catch((error) => {
                     this.showApiError(error);
                  })
                  .finally(() => {
                     this.PrintLoading2 = false;
                  });
            }
         });
      },

      Refresh() {
         this.isBusy = true;
         ReportService.PrtnEmploymentGraphReport(this.filter)
            .then((res) => {
               this.items = res.data;
               this.isBusy = false;
               this.filter.total = res.data.totalCount;
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
@import '../styles.scss';
</style>
