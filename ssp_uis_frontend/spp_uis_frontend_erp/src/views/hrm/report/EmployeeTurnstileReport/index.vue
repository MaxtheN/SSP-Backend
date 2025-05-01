<template>
   <b-card no-body>
      <div class="m-2">
         <b-row>
            <b-col md="2" sm="12">
               <label for>{{ $t('startDate1') }}</label>
               <date-picker
                  v-model="filter.onDate"
                  v-mask="'##.##.####'"
                  size="sm"
                  class="w-100"
                  first-day-of-week="1"
                  lang="ru"
                  :placeholder="$t('startDate1')"
                  value-type="format"
                  format="DD.MM.YYYY"
                  @change="Refresh"
               />
            </b-col>
            <b-col md="2" sm="12">
               <label for>{{ $t('endDate1') }}</label>
               <date-picker
                  v-model="filter.endDate"
                  v-mask="'##.##.####'"
                  size="sm"
                  class="w-100"
                  first-day-of-week="1"
                  lang="ru"
                  :placeholder="$t('endDate1')"
                  value-type="format"
                  format="DD.MM.YYYY"
                  @change="Refresh"
               />
            </b-col>
            <b-col sm="12" md="2">
               <label for>{{ $t('enterTime') }}</label>
               <date-picker
                  v-model="filter.enterTime"
                  style="width: 100%"
                  size="sm"
                  lang="ru"
                  placeholder="HH:mm:ss"
                  value-type="format"
                  type="time"
                  format="HH:mm:ss"
                  @change="Refresh"
               ></date-picker>
            </b-col>
            <b-col sm="12" md="2">
               <label for>{{ $t('exitTime') }}</label>
               <date-picker
                  v-model="filter.exitTime"
                  style="width: 100%"
                  size="sm"
                  lang="ru"
                  placeholder="HH:mm:ss"
                  value-type="format"
                  type="time"
                  format="HH:mm:ss"
                  @change="Refresh"
               ></date-picker>
            </b-col>
            <b-col cols="12" md="3">
               <label for>{{ $t('employee') }}</label>
               <b-input-group class="text-right">
                  <b-form-input v-model="filter.employee" @keyup.enter="Refresh" :placeholder="$t('employee')" />
                  <b-input-group-append>
                     <b-button @click="Refresh" variant="primary">
                        <feather-icon icon="SearchIcon" />
                     </b-button>
                  </b-input-group-append>
               </b-input-group>
            </b-col>
            <b-col sm="12" md="4" lg="4" v-if="isAdmin">
               <label for>{{ $t('organization') }}</label>
               <div>
                  <v-select
                     :options="OrganizationList"
                     :reduce="(item) => item.value"
                     :placeholder="$t('organization')"
                     label="text"
                     v-model="filter.organizationId"
                     @input="Refresh"
                  ></v-select>
               </div>
            </b-col>
            <b-col sm="12" md="2" class="mt-1">
               <b-button class="mr-1" variant="primary" @click="Print">
                  <b-icon-file-earmark-excel></b-icon-file-earmark-excel>
                  {{ $t('Load') }}
               </b-button>
            </b-col>
         </b-row>
      </div>
      <b-tabs v-model="tabIndex" pills class="mx-1 tabheader">
         <b-tab
            :title="$t('EmployeeGeneral')"
            @click="
               () => {
                  (tabIndex = 0), Refresh();
               }
            "
         >
            <b-table
               style="font-weight: bold"
               ref="refInvoiceListTable"
               :items="items"
               responsive
               :fields="fields"
               primary-key="id"
               sticky-header="65vh"
               no-border-collapse
               :busy="isBusy"
               striped
               show-empty
               @row-dblclicked="onRowSelected"
               :empty-text="$t('NotFound')"
               class="position-relative"
               @sort-changed="SortChange"
            >
               <template #cell(employeeName)="{ item }">
                  <span style="color: #1c60b1; cursor: pointer">{{ item.employeeName }}</span>
               </template>
               <template #cell(periodMinute)="{ item }">
                  {{ periodMinuteF(item.periodMinute) }}
               </template>
               <template #cell(periodMinuteSchedule)="{ item }">
                  {{ periodMinuteScheduleF(item.periodMinuteSchedule) }}
               </template>
               <template #cell(periodMinuteTotal)="{ item }">
                  {{ periodMinuteTotalF(item.periodMinuteTotal) }}
               </template>
               <template #cell(eventOn)="{ item }">
                  {{ item.eventOn }}
                  <!-- <br />
                  <span style="font-size: 10px">{{ $t(item.weekDay) }}</span> -->
               </template>
               <template v-slot:table-busy>
                  <div class="text-center text-primary my-2" style="vertical-align: middle">
                     <b-spinner class="align-middle mr-2"></b-spinner>
                     <strong>{{ $t('Loading') }}</strong>
                  </div>
               </template>
            </b-table>
         </b-tab>
         <b-tab
            :title="$t('EmployeeArrived')"
            @click="
               () => {
                  (tabIndex = 1), Refresh();
               }
            "
         >
            <b-table
               style="font-weight: bold"
               ref="refInvoiceListTable"
               :items="items"
               responsive
               :fields="fields"
               primary-key="id"
               sticky-header="65vh"
               no-border-collapse
               :busy="isBusy"
               striped
               show-empty
               :empty-text="$t('NotFound')"
               class="position-relative"
               @sort-changed="SortChange"
               @row-dblclicked="onRowSelected"
            >
               <template #cell(periodMinute)="{ item }">
                  {{ periodMinuteF(item.periodMinute) }}
               </template>
               <template #cell(periodMinuteSchedule)="{ item }">
                  {{ periodMinuteScheduleF(item.periodMinuteSchedule) }}
               </template>
               <template #cell(periodMinuteTotal)="{ item }">
                  {{ periodMinuteTotalF(item.periodMinuteTotal) }}
               </template>
               <template #cell(employeeName)="{ item }">
                  <span style="color: #1c60b1; cursor: pointer">{{ item.employeeName }}</span>
               </template>
               <template #cell(eventOn)="{ item }">
                  {{ item.eventOn }} <br />
                  {{ $t(item.weekDay) }}
               </template>
               <template v-slot:table-busy>
                  <div class="text-center text-primary my-2" style="vertical-align: middle">
                     <b-spinner class="align-middle mr-2"></b-spinner>
                     <strong>{{ $t('Loading') }}</strong>
                  </div>
               </template>
            </b-table>
         </b-tab>
         <b-tab
            :title="$t('EmployeeLate')"
            @click="
               () => {
                  (tabIndex = 2), Refresh();
               }
            "
         >
            <b-table
               style="font-weight: bold"
               ref="refInvoiceListTable"
               :items="items"
               responsive
               :fields="fields"
               primary-key="id"
               sticky-header="65vh"
               no-border-collapse
               :busy="isBusy"
               striped
               show-empty
               :empty-text="$t('NotFound')"
               class="position-relative"
               @sort-changed="SortChange"
            >
               <template #cell(periodMinute)="{ item }">
                  {{ periodMinuteF(item.periodMinute) }}
               </template>
               <template #cell(periodMinuteSchedule)="{ item }">
                  {{ periodMinuteScheduleF(item.periodMinuteSchedule) }}
               </template>
               <template #cell(periodMinuteTotal)="{ item }">
                  {{ periodMinuteTotalF(item.periodMinuteTotal) }}
               </template>
               <template v-slot:table-busy>
                  <div class="text-center text-primary my-2" style="vertical-align: middle">
                     <b-spinner class="align-middle mr-2"></b-spinner>
                     <strong>{{ $t('Loading') }}</strong>
                  </div>
               </template>
            </b-table>
         </b-tab>
         <b-tab
            :title="$t('EmployeeMissed')"
            @click="
               () => {
                  (tabIndex = 3), Refresh();
               }
            "
         >
            <b-table
               style="font-weight: bold"
               ref="refInvoiceListTable"
               :items="items"
               responsive
               :fields="fields"
               primary-key="id"
               sticky-header="65vh"
               no-border-collapse
               :busy="isBusy"
               striped
               show-empty
               :empty-text="$t('NotFound')"
               class="position-relative"
               @sort-changed="SortChange"
            >
               <template v-slot:table-busy>
                  <div class="text-center text-primary my-2" style="vertical-align: middle">
                     <b-spinner class="align-middle mr-2"></b-spinner>
                     <strong>{{ $t('Loading') }}</strong>
                  </div>
               </template>
            </b-table>
         </b-tab>
         <b-tab
            :title="$t('BreakLunchTime')"
            @click="
               () => {
                  (tabIndex = 4), Refresh();
               }
            "
         >
            <b-table
               style="font-weight: bold"
               ref="refInvoiceListTable"
               :items="items"
               responsive
               :fields="fields"
               primary-key="id"
               sticky-header="65vh"
               no-border-collapse
               :busy="isBusy"
               striped
               show-empty
               :empty-text="$t('NotFound')"
               class="position-relative"
               @sort-changed="SortChange"
            >
               <template v-slot:table-busy>
                  <div class="text-center text-primary my-2" style="vertical-align: middle">
                     <b-spinner class="align-middle mr-2"></b-spinner>
                     <strong>{{ $t('Loading') }}</strong>
                  </div>
               </template>
            </b-table>
         </b-tab>
         <b-tab
            :title="$t('LeftEarly')"
            @click="
               () => {
                  (tabIndex = 5), Refresh();
               }
            "
         >
            <b-table
               style="font-weight: bold"
               ref="refInvoiceListTable"
               :items="items"
               responsive
               :fields="fields"
               primary-key="id"
               sticky-header="65vh"
               no-border-collapse
               :busy="isBusy"
               striped
               show-empty
               :empty-text="$t('NotFound')"
               class="position-relative"
               @sort-changed="SortChange"
            >
               <template v-slot:table-busy>
                  <div class="text-center text-primary my-2" style="vertical-align: middle">
                     <b-spinner class="align-middle mr-2"></b-spinner>
                     <strong>{{ $t('Loading') }}</strong>
                  </div>
               </template>
            </b-table>
         </b-tab>
      </b-tabs>
   </b-card>
</template>

<script>
import {
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
   VBTooltip,
   BTr,
   BTh,
   BCardText,
   BIconFileEarmarkExcel,
   BTabs,
   BTab
} from 'bootstrap-vue';
// import EmployeeManageService from '@/services/hrm/employeemanage.service';

import EmployeeService from '@/services/info/employee.service';
import EmployeeTurnstileReportService from '@/services/hrm/employeeturnstilereport.service';
import OrganizationService from '@/services/managment/organization.service';
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
      BTr,
      BTh,
      BIconFileEarmarkExcel,
      BTabs,
      BTab
   },
   directives: {
      'b-tooltip': VBTooltip
   },
   data() {
      return {
         items: [],
         fields: [],
         tabIndex: 0,
         OrganizationList: [],
         PrintLoading: false,
         EmployeeList: [],
         filter: {
            search: '',
            sortBy: '',
            orderType: 'asc',
            organizationId: null,
            onDate: '',
            endDate: ''
         },
         employeeFilter: {
            pageSize: 920,
            search: '',
            page: 1
         },

         isAdmin: false,
         isBusy: false
      };
   },
   computed: {
      periodMinuteF() {
         return (minutes) => {
            const hours = Math.floor(minutes / 60);
            const mins = minutes % 60;
            return `${hours}:${mins < 10 ? '0' : ''}${mins}:00`;
         };
      },
      periodMinuteTotalF() {
         return (minutes) => {
            const hours = Math.floor(minutes / 60);
            const mins = minutes % 60;
            return `${hours}:${mins < 10 ? '0' : ''}${mins}:00`;
         };
      },
      periodMinuteScheduleF() {
         return (minutes) => {
            const hours = Math.floor(minutes / 60);
            const mins = minutes % 60;
            return `${hours}:${mins < 10 ? '0' : ''}${mins}:00`;
         };
      }
   },
   created() {
      var todaydate = new Date();
      var dd = String(todaydate.getDate()).padStart(2, '0');
      var mm = String(todaydate.getMonth() + 1).padStart(2, '0');
      var yyyy = todaydate.getFullYear();
      this.filter.onDate = dd + '.' + mm + '.' + yyyy;
      this.filter.endDate = dd + '.' + mm + '.' + yyyy;

      this.isAdmin = !JSON.parse(localStorage.getItem('user_info')).employeeManageId;
      this.filter.organizationId = JSON.parse(localStorage.getItem('user_info')).organizationId;

      OrganizationService.GetAsSelectList()
         .then((res) => {
            this.OrganizationList = res.data;
         })
         .catch((err) => {
            this.makeToast(err.response.data, 'danger');
         });
      EmployeeService.GetAsSelectList(this.employeeFilter).then((res) => {
         this.EmployeeList = res.data.rows;
      });

      this.Refresh();
   },
   methods: {
      Print() {
         this.PrintLoading = true;
         EmployeeTurnstileReportService.SaveEmployeeTurnstileReportAsExcel(this.filter)
            .then((res) => {
               this.PrintLoading = false;

               this.forceFileDownload(res, 'Турникет для сотрудников');
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
      async SearchProduct(search, loading) {
         loading(true);
         this.warehouseShelfFilter.search = search;

         this.productFilter.search = search;
         if (search.length) {
            await WhmProductService.GetAsSelectList(this.productFilter).then((res) => {
               this.ProductList = res.data.rows;
               loading(false);
            });
         } else {
            loading(false);
         }
      },
      ChangeProduct() {
         if (this.filter.productIds) {
            this.Refresh();
         } else {
            this.filter.productIds = [];
            this.Refresh();
         }
      },

      async Search(search, loading) {
         loading(true);
         this.warehouseShelfFilter.search = search;
         if (search.length && this.filter.warehouseId) {
            this.warehouseShelfFilter.warehouseId = this.filter.warehouseId;
            await WhmWarehouseShelfService.GetAsSelectList(this.warehouseShelfFilter).then((res) => {
               this.WarehouseShelfList = res.data.rows;
               loading(false);
            });
         } else {
            loading(false);
         }
      },
      getFields() {
         this.fields = [
            {
               key: 'employeeName',
               label: this.$t('employee'),
               thClass: 'text-center',
               tdClass: 'text-left',
               sortable: true
            },
            {
               key: 'positionName',
               label: this.$t('position'),
               thClass: 'text-center',
               tdClass: 'text-left',
               sortable: true
            },

            {
               key: 'eventOn',
               label: this.$t('docOn'),
               thClass: 'text-center',
               tdClass: 'text-right',
               sortable: true
            },
            {
               key: this.tabIndex != 3 && this.tabIndex != 4 ? 'enterAt' : null,
               label: this.$t('enterTime'),
               thClass: 'text-center',
               tdClass: 'text-right',
               sortable: true
            },
            {
               key: this.tabIndex != 3 && this.tabIndex == 4 ? 'exitAt' : null,
               label: this.$t('exitTime'),
               thClass: 'text-center',
               tdClass: 'text-right',
               sortable: true
            },
            {
               key: this.tabIndex != 3 && this.tabIndex == 4 ? 'enterAt' : null,
               label: this.$t('enterTime'),
               thClass: 'text-center',
               tdClass: 'text-right',
               sortable: true
            },
            {
               key: this.tabIndex != 3 && this.tabIndex != 4 ? 'exitAt' : null,
               label: this.$t('exitTime'),
               thClass: 'text-center',
               tdClass: 'text-right',
               sortable: true
            },

            {
               key: this.tabIndex != 3 ? 'periodMinute' : null,
               label: this.$t('duration'),
               thClass: 'text-center',
               tdClass: 'text-right',
               sortable: true
            },
            {
               key: this.tabIndex != 3 ? 'periodMinuteSchedule' : null,
               label: this.$t('durationSchedule'),
               thClass: 'text-center',
               tdClass: 'text-right',
               sortable: true
            },
            {
               key: this.tabIndex != 3 ? 'periodMinuteTotal' : null,
               label: this.$t('periodMinuteTotal'),
               thClass: 'text-center',
               tdClass: 'text-right',
               sortable: true
            },
            {
               key: this.tabIndex != 3 ? 'enterCount' : null,
               label: this.$t('enterCount'),
               thClass: 'text-center',
               tdClass: 'text-right',
               sortable: true
            },
            {
               key: this.tabIndex != 3 ? 'exitCount' : null,
               label: this.$t('exitCount'),
               thClass: 'text-center',
               tdClass: 'text-right',
               sortable: true
            }
         ];
      },

      SortChange(data) {
         this.filter.sortBy = data.sortBy;
         this.filter.orderType = data.sortDesc ? 'desc' : 'asc';
         this.Refresh();
      },
      onRowSelected(res) {
         this.$router.push({
            name: 'EmployeeTurnstileReportById',
            query: {
               onDate: this.filter.onDate,
               endDate: this.filter.endDate,
               employeeId: res.employeeId
            }
         });

         this.ContractorOrderModal = false;
      },
      Refresh() {
         this.orderNumber = 0;
         this.isBusy = true;
         if (this.tabIndex == 0) {
            this.filter.isMissed = false;
            this.filter.isArrived = false;
            this.filter.isLate = false;
            this.filter.isBreakLunchTime = false;
            this.filter.isLeftEarly = false;
         }
         if (this.tabIndex == 1) {
            this.filter.isMissed = false;
            this.filter.isArrived = true;
            this.filter.isLate = false;
            this.filter.isBreakLunchTime = false;
            this.filter.isLeftEarly = false;
         }
         if (this.tabIndex == 2) {
            this.filter.isMissed = false;
            this.filter.isArrived = false;
            this.filter.isLate = true;
            this.filter.enterTime = '09:00:00';
            this.filter.exitTime = '18:00:00';
            this.filter.isBreakLunchTime = false;
            this.filter.isLeftEarly = false;
         }
         if (this.tabIndex == 3) {
            this.filter.isMissed = true;
            this.filter.isArrived = false;
            this.filter.isLate = false;
            this.filter.isBreakLunchTime = false;
            this.filter.isLeftEarly = false;
         }
         if (this.tabIndex == 4) {
            this.filter.isMissed = false;
            this.filter.isArrived = false;
            this.filter.isLate = false;
            this.filter.isBreakLunchTime = true;
            this.filter.isLeftEarly = false;
         }
         if (this.tabIndex == 5) {
            this.filter.isMissed = false;
            this.filter.isArrived = false;
            this.filter.isLate = false;
            this.filter.isBreakLunchTime = false;
            this.filter.isLeftEarly = false;
            this.filter.isLeftEarly = true;
         }

         // this.SaveFilter();
         if (this.isAdmin && this.filter.organizationId) {
            EmployeeTurnstileReportService.GetEmployeeTurnstileReport(this.filter)
               .then((res) => {
                  this.items = res.data;

                  this.isBusy = false;
                  this.getFields();
               })
               .catch((error) => {
                  this.makeToast(error.response.data, 'danger');
               });
         }
         if (!this.isAdmin) {
            EmployeeTurnstileReportService.GetEmployeeTurnstileReport(this.filter)
               .then((res) => {
                  this.items = res.data;

                  this.isBusy = false;
                  this.getFields();
               })
               .catch((error) => {
                  this.makeToast(error.response.data, 'danger');
               });
         }
      }
   }
};
</script>

<style lang="scss" scoped>
.per-page-selector {
   width: 90px;
}

.invoice-filter-select {
   min-width: 190px;

   ::v-deep .vs__selected-options {
      flex-wrap: nowrap;
   }

   ::v-deep .vs__selected {
      width: 100px;
   }
}
</style>

<style lang="scss">
@import '@core/scss/vue/libs/vue-select.scss';
.tabheader {
   .nav {
      background: #f3f2f7;
      padding: 10px 15px;
      width: fit-content;
      font-weight: bold;
   }
}
</style>
