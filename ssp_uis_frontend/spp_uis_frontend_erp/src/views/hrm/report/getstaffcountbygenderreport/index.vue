<template>
   <b-card no-body>
      <div class="m-2 report-table">
         <b-overlay :show="isBusy">
            <!-- <b-tabs content-class="">
               <b-tab :title="$t('Hudud bo\'yicha')" active> </b-tab>
               <b-tab :title="$t('Lavozim bo\'yicha')"></b-tab>
            </b-tabs> -->
            <b-row>
               <b-col sm="12" md="3">
                  <form-picker :label="$t('startDate1')" v-model="filter.fromDate" @change="Refresh" />
               </b-col>
               <b-col sm="12" md="3">
                  <form-picker :label="$t('endDate1')" v-model="filter.toDate" @change="Refresh" />
               </b-col>
               <b-col sm="6" md="6" class="text-right mb-1">
                  <b-button @click="Print" variant="primary">
                     <feather-icon icon="PrinterIcon"></feather-icon>
                     {{ $t('Print') }}
                  </b-button>
               </b-col>
            </b-row>
            <b-breadcrumb class="mt-0 mb-1">
               <b-breadcrumb-item
                  active
                  @click="
                     () => {
                        filter.regionId = null;
                        filter.organization = '';
                        filter.Employee = '';
                        filter.byRegion = true;
                        filter.organizationId = null;
                        filter.byOrganization = false;
                        filter.departmentId = null;
                        filter.department = '';
                        filter.byDepartment = false;
                        hideColumn = true;
                        filter.position = '';
                        filter.region = '';
                        filter.byPositionCategory = false;
                        filter.byEmployee = false;
                        filter.positionId = null;
                        filter.byPosition = false;
                        Refresh();
                     }
                  "
               >
                  <b>{{ $t('uzb') }}</b>
               </b-breadcrumb-item>
               <b-breadcrumb-item
                  :active="filter.byDistrict"
                  @click="
                     () => {
                        hideColumn = true;
                        filter.byOrganization = true;
                        filter.organizationId = null;
                        filter.byDepartment = false;
                        filter.byEmployee = false;
                        filter.byPosition = false;
                        Refresh();
                     }
                  "
               >
                  <b>{{ filter.region }}</b>
               </b-breadcrumb-item>
               <b-breadcrumb-item
                  v-show="filter.organizationId"
                  :active="filter.byDepartment"
                  @click="
                     () => {
                        filter.byDepartment = true;
                        filter.byPosition = false;
                        filter.departmentId = null;
                        filter.byEmployee = false;
                        filter.positionId = null;
                        Refresh();
                     }
                  "
               >
                  <b>{{ filter.organization }}</b>
               </b-breadcrumb-item>

               <b-breadcrumb-item
                  v-show="filter.byPosition || filter.byEmployee"
                  :active="filter.byPosition"
                  @click="
                     () => {
                        filter.byPosition = true;
                        filter.byEmployee = false;
                        filter.positionId = null;
                        Refresh();
                     }
                  "
               >
                  <b>{{ filter.department }}</b>
               </b-breadcrumb-item>
            </b-breadcrumb>

            <b-table-simple hover v-show="!filter.byEmployee" small caption-top responsive border>
               <b-thead>
                  <b-tr>
                     <b-th
                        :class="isMobileDevice() ? '' : 'b-table-sticky-column'"
                        rowspan="2"
                        style="font-weight: 900; font-size: 14px; color: black; text-align: center"
                     >
                        {{ $t('order') }}
                     </b-th>
                     <b-th
                        rowspan="2"
                        :class="
                           isMobileDevice() ? 'table-b-table-default' : 'table-b-table-default b-table-sticky-column'
                        "
                     >
                        <div
                           v-show="filter.byRegion"
                           style="font-weight: 900; font-size: 14px; width: 300px; color: black"
                        >
                           {{ $t('region') }}
                        </div>
                        <div
                           v-show="filter.byOrganization"
                           style="font-weight: 900; font-size: 14px; width: 200px; color: black"
                        >
                           {{ $t('organization') }}
                        </div>
                        <div
                           v-show="filter.byDepartment"
                           style="font-weight: 900; font-size: 14px; width: 200px; color: black"
                        >
                           {{ $t('Department') }}
                        </div>
                        <div
                           v-show="filter.byPosition"
                           style="font-weight: 900; font-size: 14px; width: 200px; color: black"
                        >
                           {{ $t('position') }}
                        </div>
                     </b-th>
                     <b-th rowspan="2" style="font-weight: 900; font-size: 14px; color: black; text-align: center">
                        {{ $t('totalStaffingRate') }}
                     </b-th>
                     <b-th rowspan="2" style="font-weight: 900; font-size: 14px; color: black; text-align: center">
                        {{ $t('totalEmployeeManageRate') }}
                     </b-th>
                     <b-th rowspan="2" style="font-weight: 900; font-size: 14px; color: black; text-align: center">
                        {{ $t('Vakant orinlar soni jami') }}
                     </b-th>
                     <b-th rowspan="2" style="font-weight: 900; font-size: 14px; color: black; text-align: center">
                        {{ $t('Jami xodimlar soni') }}
                     </b-th>

                     <b-th colspan="2" style="font-weight: 900; font-size: 14px; color: black; text-align: center">
                        {{ $t('Shundan') }}
                     </b-th>
                     <b-th
                        v-if="hideColumn"
                        rowspan="2"
                        style="font-weight: 900; font-size: 14px; color: black; text-align: center"
                     >
                        {{ $t('Raxbariyat') }}
                     </b-th>
                     <b-th
                        v-if="hideColumn"
                        colspan="2"
                        style="font-weight: 900; font-size: 14px; color: black; text-align: center"
                     >
                        {{ $t('Shundan') }}
                     </b-th>
                     <b-th
                        v-if="hideColumn"
                        rowspan="2"
                        style="font-weight: 900; font-size: 14px; color: black; text-align: center"
                     >
                        {{ $t('Boshqaruv xodimlari') }}
                     </b-th>
                     <b-th
                        colspan="2"
                        v-if="hideColumn"
                        style="font-weight: 900; font-size: 14px; color: black; text-align: center"
                     >
                        {{ $t('Shundan') }}
                     </b-th>
                     <b-th
                        v-if="hideColumn"
                        rowspan="2"
                        style="font-weight: 900; font-size: 14px; color: black; text-align: center"
                     >
                        {{ $t('Ishlab chiqarish xodimlari') }}
                     </b-th>
                     <b-th
                        v-if="hideColumn"
                        colspan="2"
                        style="font-weight: 900; font-size: 14px; color: black; text-align: center"
                     >
                        {{ $t('Shundan') }}
                     </b-th>
                     <b-th
                        v-if="hideColumn"
                        rowspan="2"
                        style="font-weight: 900; font-size: 14px; color: black; text-align: center"
                     >
                        {{ $t('Texnik xodimlar') }}
                     </b-th>
                     <b-th
                        v-if="hideColumn"
                        colspan="2"
                        style="font-weight: 900; font-size: 14px; color: black; text-align: center"
                     >
                        {{ $t('Shundan') }}
                     </b-th>
                  </b-tr>
                  <b-tr>
                     <b-th style="font-weight: 900; font-size: 14px; color: black; text-align: center">
                        {{ $t('Erkak') }}
                     </b-th>
                     <b-th style="font-weight: 900; font-size: 14px; color: black; text-align: center">
                        {{ $t('Ayol') }}
                     </b-th>
                     <b-th
                        v-if="hideColumn"
                        style="font-weight: 900; font-size: 14px; color: black; text-align: center"
                     >
                        {{ $t('Erkak') }}
                     </b-th>
                     <b-th
                        v-if="hideColumn"
                        style="font-weight: 900; font-size: 14px; color: black; text-align: center"
                     >
                        {{ $t('Ayol') }}
                     </b-th>
                     <b-th
                        v-if="hideColumn"
                        style="font-weight: 900; font-size: 14px; color: black; text-align: center"
                     >
                        {{ $t('Erkak') }}
                     </b-th>
                     <b-th
                        v-if="hideColumn"
                        style="font-weight: 900; font-size: 14px; color: black; text-align: center"
                     >
                        {{ $t('Ayol') }}
                     </b-th>
                     <b-th
                        v-if="hideColumn"
                        style="font-weight: 900; font-size: 14px; color: black; text-align: center"
                     >
                        {{ $t('Erkak') }}
                     </b-th>
                     <b-th
                        v-if="hideColumn"
                        style="font-weight: 900; font-size: 14px; color: black; text-align: center"
                     >
                        {{ $t('Ayol') }}
                     </b-th>
                     <b-th
                        v-if="hideColumn"
                        style="font-weight: 900; font-size: 14px; color: black; text-align: center"
                     >
                        {{ $t('Erkak') }}
                     </b-th>
                     <b-th
                        v-if="hideColumn"
                        style="font-weight: 900; font-size: 14px; color: black; text-align: center"
                     >
                        {{ $t('Ayol') }}
                     </b-th>
                  </b-tr>
               </b-thead>
               <b-tbody>
                  <b-tr v-for="(item, idx) in items" :key="idx + 'abc'">
                     <b-td :class="isMobileDevice() ? '' : 'b-table-sticky-column'">
                        {{ idx + 1 }}
                     </b-td>
                     <b-td
                        :class="
                           isMobileDevice() ? 'table-b-table-default' : 'table-b-table-default b-table-sticky-column'
                        "
                     >
                        <span v-show="filter.byRegion" style="color: blue; cursor: pointer" @click="SortRegion(item)"
                           >{{ item.region }}
                        </span>
                        <div
                           v-show="filter.byOrganization"
                           style="color: blue; cursor: pointer; width: 600px; text-wrap: wrap"
                           @click="SortOrganization(item)"
                        >
                           {{ item.organization }}
                        </div>
                        <div
                           v-show="filter.byDepartment"
                           style="color: blue; cursor: pointer; width: 400px; text-wrap: wrap"
                           @click="SortDepartment(item)"
                        >
                           {{ item.department }}
                        </div>
                        <div
                           v-show="filter.byPosition"
                           style="color: blue; cursor: pointer; width: 400px; text-wrap: wrap"
                           @click="SortPosition(item)"
                        >
                           {{ item.position }}
                        </div>
                        <span
                           v-show="filter.byEmployee"
                           style="color: blue; cursor: pointer"
                           @click="SortPosition(item)"
                           >{{ item.Employee }}
                        </span>
                     </b-td>
                     <b-td class="text-right"> {{ item.totalStaffingRate }} </b-td>
                     <b-td class="text-right"> {{ item.totalEmployeeManageRate }} </b-td>
                     <b-td class="text-right"> {{ item.totalCount }} </b-td>
                     <b-td class="text-right"> {{ item.totalEmployee }} </b-td>
                     <b-td class="text-right"> {{ item.totalEmployeeMen }} </b-td>
                     <b-td class="text-right"> {{ item.totalEmployeeWomen }} </b-td>
                     <b-td v-if="hideColumn" class="text-right">
                        {{
                           item.positionCategorys && item.positionCategorys[0]?.count
                              ? item.positionCategorys[0]?.count
                              : '0'
                        }}
                     </b-td>
                     <b-td v-if="hideColumn" class="text-right">
                        {{
                           item.positionCategorys && item.positionCategorys[0]?.men
                              ? item.positionCategorys[0]?.men
                              : '0'
                        }}
                     </b-td>
                     <b-td v-if="hideColumn" class="text-right">
                        {{
                           item.positionCategorys && item.positionCategorys[0]?.women
                              ? item.positionCategorys[0]?.women
                              : '0'
                        }}
                     </b-td>
                     <b-td v-if="hideColumn" class="text-right">
                        {{
                           item.positionCategorys && item.positionCategorys[1]?.count
                              ? item.positionCategorys[1].count
                              : '0'
                        }}
                     </b-td>

                     <b-td v-if="hideColumn" class="text-right">
                        {{
                           item.positionCategorys && item.positionCategorys[1]?.men
                              ? item.positionCategorys[1]?.men
                              : '0'
                        }}
                     </b-td>
                     <b-td v-if="hideColumn" class="text-right">
                        {{
                           item.positionCategorys && item.positionCategorys[1]?.women
                              ? item.positionCategorys[1]?.women
                              : '0'
                        }}
                     </b-td>

                     <b-td v-if="hideColumn" class="text-right">
                        {{
                           item.positionCategorys && item.positionCategorys[2]?.count
                              ? item.positionCategorys[2]?.count
                              : '0'
                        }}
                     </b-td>

                     <b-td v-if="hideColumn" class="text-right">
                        {{
                           item.positionCategorys && item.positionCategorys[2]?.men
                              ? item.positionCategorys[2]?.men
                              : '0'
                        }}
                     </b-td>
                     <b-td v-if="hideColumn" class="text-right">
                        {{
                           item.positionCategorys && item.positionCategorys[2]?.women
                              ? item.positionCategorys[2]?.women
                              : '0'
                        }}
                     </b-td>
                     <b-td v-if="hideColumn" class="text-right">
                        {{
                           item.positionCategorys && item.positionCategorys[3]?.count
                              ? item.positionCategorys[3]?.count
                              : '0'
                        }}
                     </b-td>

                     <b-td v-if="hideColumn" class="text-right">
                        {{
                           item.positionCategorys && item.positionCategorys[3]?.men
                              ? item.positionCategorys[3]?.men
                              : '0'
                        }}
                     </b-td>
                     <b-td v-if="hideColumn" class="text-right">
                        {{
                           item.positionCategorys && item.positionCategorys[3]?.women
                              ? item.positionCategorys[3]?.women
                              : '0'
                        }}
                     </b-td>
                  </b-tr>
               </b-tbody>
               <b-tfoot v-if="items.length > 0">
                  <b-tr variant="secondary">
                     <b-td class="text-center b-table-sticky-column" colspan="2">{{ $t('Total') }}</b-td>
                     <b-td class="text-right">{{ counts.totalStaffingRate }}</b-td>
                     <b-td class="text-right">{{ counts.totalEmployeeManageRate }}</b-td>
                     <b-td class="text-right">{{ counts.totalCount }}</b-td>
                     <b-td class="text-right">{{ counts.totalEmployee }}</b-td>
                     <b-td class="text-right">{{ counts.totalEmployeeMen }}</b-td>
                     <b-td class="text-right">{{ counts.totalEmployeeWomen }}</b-td>
                     <b-td v-if="hideColumn" class="text-right">{{ counts.R.count }}</b-td>
                     <b-td v-if="hideColumn" class="text-right">{{ counts.R.men }}</b-td>
                     <b-td v-if="hideColumn" class="text-right">{{ counts.R.women }}</b-td>
                     <b-td v-if="hideColumn" class="text-right">{{ counts.B.count }}</b-td>
                     <b-td v-if="hideColumn" class="text-right">{{ counts.B.men }}</b-td>
                     <b-td v-if="hideColumn" class="text-right">{{ counts.B.women }}</b-td>
                     <b-td v-if="hideColumn" class="text-right">{{ counts.I.count }}</b-td>
                     <b-td v-if="hideColumn" class="text-right">{{ counts.I.men }}</b-td>
                     <b-td v-if="hideColumn" class="text-right">{{ counts.I.women }}</b-td>

                     <b-td v-if="hideColumn" class="text-right">{{ counts.T.count }}</b-td>
                     <b-td v-if="hideColumn" class="text-right">{{ counts.T.men }}</b-td>
                     <b-td v-if="hideColumn" class="text-right">{{ counts.T.women }}</b-td>
                  </b-tr>
               </b-tfoot>
            </b-table-simple>
            <div class="mt-2 mb-2" v-show="filter.byEmployee">
               <b-table
                  ref="refInvoiceListTable"
                  :items="items2"
                  :busy="isBusy"
                  responsive
                  :fields="fields2"
                  primary-key="id"
                  sticky-header="65vh"
                  no-border-collapse
                  show-empty
                  :empty-text="$t('NotFound')"
                  class="position-relative"
               >
                  <template #cell(order)="{ index }">
                     <span>{{ index + 1 }}</span>
                  </template>
                  <template #cell(employee)="{ item }">
                     <b-link :to="{ name: 'EmployeeCard', query: { pinfl: item.employeePinfl } }">{{
                        item.employee
                     }}</b-link>
                  </template>
               </b-table>
            </div>
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
   BOverlay,
   BTabs,
   BTab,
   BIconFileEarmarkExcel
} from 'bootstrap-vue';
import EmployeeTurnstileReportService from '@/services/hrm/employeeturnstilereport.service';
import ReportService from '@/services/report/report.service';
import DistrictService from '@/services/info/district.service';
export default {
   components: {
      BButton,
      BPagination,
      BTable,
      BTab,
      BTabs,
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
      BIconFileEarmarkExcel,
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
         hideColumn: true,
         items: [],
         localStorageData: [],
         items2: [],
         DistrictList: [],
         PrintLoading: false,
         counts: {
            totalStaffingRate: 0,
            totalEmployeeManageRate: 0,
            totalCount: 0,
            totalEmployee: 0,
            totalEmployeeMen: 0,
            totalEmployeeWomen: 0,
            R: {
               men: 0,
               women: 0,
               count: 0
            },
            B: {
               men: 0,
               women: 0,
               count: 0
            },
            I: {
               men: 0,
               women: 0,
               count: 0
            },
            T: {
               men: 0,
               women: 0,
               count: 0
            }
         },
         filter: {
            fromDate: '',
            toDate: '',
            regionId: null,
            organization: '',
            Employee: '',
            byRegion: false,
            organizationId: null,
            byOrganization: true,
            departmentId: null,
            department: '',
            byDepartment: false,
            position: '',
            region: '',
            byPositionCategory: false,
            byEmployee: false,
            positionId: null,
            byPosition: false
         },
         fields2: [
            {
               key: 'order',
               label: this.$t('№'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'docId',
               label: this.$t('docId'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'department',
               label: this.$t('department'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'position',
               label: this.$t('position'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'workSchedule',
               label: this.$t('workSchedule'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'startOn',
               label: this.$t('startdate'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'employee',
               label: this.$t('employee'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'employeeGender',
               label: this.$t('gender'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'employeePhoneNumber',
               label: this.$t('phone'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            }
         ]
      };
   },
   created() {
      this.getDataLocalStorage();
      this.Refresh();
   },
   methods: {
      Print() {
         this.PrintLoading = true;
         EmployeeTurnstileReportService.SaveAsExcelGetStaffCountByGenderReport(this.filter)
            .then((res) => {
               this.PrintLoading = false;

               this.forceFileDownload(res, this.$t('GetStaffCountByGenderReport'));
            })
            .catch((error) => {
               this.PrintLoading = false;
               this.makeToast(error.response.data, 'danger');
            });
      },
      forceFileDownload(response, name) {
         var headers = response.headers;
         var blob = new Blob([response.data]);
         const url = window.URL.createObjectURL(blob);
         const link = document.createElement('a');
         link.href = url;
         link.setAttribute('download', name + '.xlsx'); //or any other extension
         document.body.appendChild(link);
         link.click();
      },
      GetDistrict(id) {
         DistrictService.GetAsSelectList(id)
            .then((res) => {
               this.DistrictList = res.data;
            })
            .catch((error) => {
               // this.showApiError(error);
            });
      },
      Refresh() {
         this.isBusy = true;
         // console.log(this.localStorageData);
         if (this.localStorageData.organizationId != 1) {
            this.filter.regionId = this.localStorageData.organizationRegionId;
            this.filter.organizationId = this.filter.organizationId || this.localStorageData?.organizationId;
         }

         ReportService.GetStaffCountByGenderReport(this.filter)
            .then((res) => {
               if (this.filter.byEmployee) {
                  this.items2 = res.data.slice(1);
                  this.isBusy = false;
               } else {
                  this.isBusy = false;
                  this.items = res.data;
                  this.counts.totalCount = 0;
                  this.counts.totalEmployee = 0;
                  this.counts.totalEmployeeManageRate = 0;
                  this.counts.totalStaffingRate = 0;
                  this.counts.totalEmployeeMen = 0;
                  this.counts.totalEmployeeWomen = 0;
                  this.counts.R.men = 0;
                  this.counts.R.women = 0;
                  this.counts.R.count = 0;
                  this.counts.B.men = 0;
                  this.counts.B.women = 0;
                  this.counts.B.count = 0;
                  this.counts.I.men = 0;
                  this.counts.I.women = 0;
                  this.counts.I.count = 0;
                  this.counts.T.men = 0;
                  this.counts.T.women = 0;
                  this.counts.T.count = 0;
                  this.items.forEach((item) => {
                     this.counts.totalCount += item.totalCount;
                     this.counts.totalEmployee += item.totalEmployee;
                     this.counts.totalEmployeeManageRate += item.totalEmployeeManageRate;
                     this.counts.totalStaffingRate += item.totalStaffingRate;
                     this.counts.totalEmployeeMen += item.totalEmployeeMen;
                     this.counts.totalEmployeeWomen += item.totalEmployeeWomen;

                     if (item.positionCategorys) {
                        item.positionCategorys.forEach((e) => {
                           if (e.positionCategoryId == 1) {
                              this.counts.R.men += e.men;
                              this.counts.R.women += e.women;
                              this.counts.R.count += e.count;
                           }
                           if (e.positionCategoryId == 2) {
                              // console.log(e.count);
                              this.counts.B.men += e.men;
                              this.counts.B.women += e.women;
                              this.counts.B.count += e.count;
                           }
                           if (e.positionCategoryId == 3) {
                              this.counts.I.men += e.men;
                              this.counts.I.women += e.women;
                              this.counts.I.count += e.count;
                           }
                           if (e.positionCategoryId == 4) {
                              this.counts.T.men += e.men;
                              this.counts.T.women += e.women;
                              this.counts.T.count += e.count;
                           }
                        });
                     }
                     // console.log(this.counts.B.count);
                  });
               }
            })
            .catch((error) => {
               this.isBusy = false;
               // this.showApiError(error);
            });
      },
      SortRegion(item) {
         this.filter.regionId = item.regionId;
         this.filter.byRegion = false;
         this.filter.region = item.region;
         this.hideColumn = true;
         // (this.filter.organization = item.organization);
         this.filter.byOrganization = true;
         this.Refresh();
      },
      SortOrganization(item) {
         this.filter.byRegion = false;
         this.filter.organizationId = item.organizationId;
         this.filter.organization = item.organization;
         this.filter.byOrganization = false;
         this.filter.departmentId = null;
         this.filter.byDepartment = true;
         this.hideColumn = false;
         // this.filter.department = item.department;
         this.Refresh();
      },
      SortDepartment(item) {
         this.hideColumn = false;
         this.filter.byRegion = false;
         this.filter.byOrganization = false;
         this.filter.departmentId = item.departmentId;
         this.filter.department = item.department;
         this.filter.byDepartment = false;
         this.filter.byPositionCategory = false;
         this.filter.byEmployee = false;
         this.filter.byPosition = true;

         this.Refresh();
      },
      SortPosition(item) {
         this.hideColumn = false;
         this.filter.byRegion = false;
         this.filter.byOrganization = false;
         this.filter.byDepartment = false;
         this.filter.byPositionCategory = false;
         this.filter.byEmployee = true;
         // this.filter.Employee = item.Employee;
         this.filter.positionId = item.positionId;
         this.filter.byPosition = false;
         this.Refresh();
      },

      getDataLocalStorage() {
         const localdata = localStorage.getItem('user_info');
         this.localStorageData = JSON.parse(localdata);
      }
   }
};
</script>
<style lang="scss" scoped>
.report-table {
   thead {
      th {
         text-align: center;
         vertical-align: middle;
      }
   }

   td {
      white-space: nowrap;
      padding: 0.6rem 0.7rem !important;
   }

   .table:not(.table-dark) {
      td,
      th {
         border: 1px solid #ebe9f1 !important;
      }
   }
}

.breadcrumb-item.active {
   color: var(--primary);
   cursor: pointer;
}

.text-nowrap {
   white-space: nowrap !important;
}
</style>
