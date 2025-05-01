<template>
   <b-card no-body>
      <div class="m-2">
         <b-row>
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
                  @input="Refresh"
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
                  @input="Refresh"
               ></date-picker>
            </b-col>
            <b-col md="2" sm="12">
               <label for>{{ $t('startDate') }}</label>
               <date-picker
                  v-model="filter.onDate"
                  v-mask="'##.##.####'"
                  size="sm"
                  class="w-100"
                  first-day-of-week="1"
                  lang="ru"
                  :placeholder="$t('startDate')"
                  value-type="format"
                  format="DD.MM.YYYY"
                  @change="Refresh"
               />
            </b-col>
            <b-col md="2" sm="12">
               <label for>{{ $t('endDate') }}</label>
               <date-picker
                  v-model="filter.endDate"
                  v-mask="'##.##.####'"
                  size="sm"
                  class="w-100"
                  first-day-of-week="1"
                  lang="ru"
                  :placeholder="$t('endDate')"
                  value-type="format"
                  format="DD.MM.YYYY"
                  @change="Refresh"
               />
            </b-col>
            <b-col md="3" sm="12">
               <label for>{{ $t('employee') }}</label>
               <div>
                  <v-select
                     :options="EmployeeList"
                     :reduce="(item) => item.employeeId"
                     :placeholder="$t('employee')"
                     label="employee"
                     v-model="filter.employeeId"
                     @input="Refresh"
                  ></v-select>
               </div>
            </b-col>
            <b-col class="cols-auto mt-2">
               <b-button @click="Print" :disabled="PrintLoading" variant="primary">
                  <feather-icon icon="PrinterIcon"></feather-icon>
               </b-button>
            </b-col>
         </b-row>
      </div>
      <b-table-simple bordered responsive striped sticky-header="80vh" no-border-collapse>
         <b-thead>
            <b-tr>
               <b-td
                  v-for="(item, index) in items"
                  style="vertical-align: middle; background-color: #1151a1"
                  :key="index + 'cell1'"
                  class="text-center"
               >
                  <span style="color: white; font-weight: bold">{{ item.eventOn }} ({{ $t(`${item.weekDay}`) }})</span>
               </b-td>
            </b-tr>
         </b-thead>
         <b-tbody>
            <b-tr>
               <b-td
                  v-for="(item, index) in items"
                  :key="index"
                  style="font-weight: bold; font-size: 16px; vertical-align: top; text-align: center"
               >
                  <template v-for="(itemlog, indexlog) in item.timeReportList">
                     <p :key="indexlog" style="white-space: nowrap">
                        <span :style="itemlog.enterAtSchedule != itemlog.enterAt ? { color: '#b93131' } : {}">{{
                           itemlog.enterAt
                        }}</span>
                        -
                        <span :style="itemlog.exitAtSchedule != itemlog.exitAt ? { color: '#b93131' } : {}"
                           >{{ itemlog.exitAt }}
                        </span>
                        <span> ({{ periodHour(itemlog.periodMinute) }})</span>
                     </p>
                  </template>
               </b-td>
            </b-tr>
            <b-tr>
               <b-td
                  v-for="(item, index) in items"
                  :key="index"
                  style="
                     font-weight: bold;
                     font-size: 16px;
                     vertical-align: top;
                     text-align: center;
                     white-space: nowrap;
                  "
               >
                  <p>{{ item.totalPeriodTime }} ({{ item.totalPeriodTimeSchedule }} {{ $t('schedule') }})</p>
               </b-td>
            </b-tr>
         </b-tbody>
      </b-table-simple>
   </b-card>
</template>

<script>
import {
   BButton,
   BPagination,
   BTableSimple,
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
   BTd,
   BCardText,
   BIconFileEarmarkExcel,
   BTbody,
   BThead
} from 'bootstrap-vue';
import EmployeeManageService from '@/services/hrm/employeemanage.service';
import EmployeeTurnstileReportService from '@/services/hrm/employeeturnstilereport.service';
export default {
   components: {
      BButton,
      BPagination,
      BTableSimple,
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
      BTd,
      BIconFileEarmarkExcel,
      BTbody,
      BThead
   },
   directives: {
      'b-tooltip': VBTooltip
   },
   data() {
      return {
         items: [],
         EmployeeList: [],
         PrintLoading: false,
         filter: {
            search: '',
            sortBy: '',
            orderType: 'asc',
            onDate: '',
            endDate: '',
            organizationId: null
         },
         filterEmployee: {
            pageSize: 920,
            search: '',
            sortBy: '',
            orderType: 'asc',
            onDate: '',
            endDate: '',
            departmentId: null,
            isOnlyWorkingEmployee: true
         },
         employeeFilter: {
            pageSize: 920,
            search: '',
            page: 1
         },
         isBusy: false
      };
   },
   computed: {
      periodHour() {
         return (minutes) => {
            const hours = Math.floor(minutes / 60);
            const mins = minutes % 60;
            return `${hours}:${mins < 10 ? '0' : ''}${mins}`;
         };
      }
   },
   created() {
      var todaydate = new Date();
      var dd = String(todaydate.getDate()).padStart(2, '0');
      var mm = String(todaydate.getMonth() + 1).padStart(2, '0');
      var yyyy = todaydate.getFullYear();
      this.filter.onDate = '01.' + mm + '.' + yyyy;
      this.filter.endDate = dd + '.' + mm + '.' + yyyy;
      this.filter.organizationId = JSON.parse(localStorage.getItem('user_info')).organizationId;
      EmployeeManageService.GetList(this.filterEmployee)
         .then((res) => {
            this.EmployeeList = res.data.rows;
         })
         .catch((error) => {
            this.makeToast(error.response.data, 'danger');
         });
      if (this.$route.query.employeeId) {
         this.filter.onDate = this.$route.query.onDate;
         this.filter.endDate = this.$route.query.endDate;
         this.filter.employeeId = this.$route.query.employeeId;
      }

      this.Refresh();
   },
   methods: {
      SortChange(data) {
         this.filter.sortBy = data.sortBy;
         this.filter.orderType = data.sortDesc ? 'desc' : 'asc';
         this.Refresh();
      },
      Refresh() {
         this.orderNumber = 0;
         this.isBusy = true;

         EmployeeTurnstileReportService.GetEmployeeTurnstileReportById(this.filter)
            .then((res) => {
               this.items = res.data;
               this.isBusy = false;
            })
            .catch((error) => {
               this.makeToast(error.response.data, 'danger');
            });
      },
      Print() {
         this.PrintLoading = true;
         EmployeeTurnstileReportService.SaveEmployeeTurnstileReportById(this.filter)
            .then((res) => {
               this.forceFileDownload(res, this.$t('EmployeeTurnstileReportById'));
            })
            .catch((e) => {
               this.showApiError(e);
            })
            .finally(() => {
               this.PrintLoading = false;
            });
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
