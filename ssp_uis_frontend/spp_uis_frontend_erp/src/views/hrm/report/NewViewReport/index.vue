<template>
   <div>
      <b-card>
         <b-row class="mb-1">
            <b-col md="12" sm="12" lg="12" class="text-right">
               <feather-icon icon="ClockIcon"></feather-icon>
               <span style="font-weight: bold">{{ currentTime }}</span></b-col
            >
            <b-col sm="12" md="3" class="mt-2">
               <b-button-group @click="Refresh">
                  <b-button
                     @click="
                        () => {
                           (filter.byDay = true), (filter.byWeek = false), (filter.byMonth = false);
                        }
                     "
                     :variant="filter.byDay ? 'primary' : 'outline-primary'"
                     size="sm"
                  >
                     {{ $t('byDay') }}
                  </b-button>
                  <b-button
                     @click="
                        () => {
                           (filter.byDay = false), (filter.byWeek = true), (filter.byMonth = false);
                        }
                     "
                     :variant="filter.byWeek ? 'primary' : 'outline-primary'"
                     size="sm"
                  >
                     {{ $t('byWeek') }}
                  </b-button>
                  <b-button
                     @click="
                        () => {
                           (filter.byDay = false), (filter.byWeek = false), (filter.byMonth = true);
                        }
                     "
                     :variant="filter.byMonth ? 'primary' : 'outline-primary'"
                     size="sm"
                  >
                     {{ $t('byMonth') }}
                  </b-button>
               </b-button-group>
               <b-button-group @click="Refresh" class="ml-1">
                  <b-button @click="schedule = false" :variant="!schedule ? 'primary' : 'outline-primary'" size="sm">
                     {{ $t('fact') }}
                  </b-button>
                  <b-button @click="schedule = true" :variant="schedule ? 'primary' : 'outline-primary'" size="sm">
                     {{ $t('plan') }}
                  </b-button>
               </b-button-group>
            </b-col>

            <b-col md="2" sm="12" v-if="filter.byDay || filter.byWeek">
               <label for>{{ $t('docOn') }}</label>
               <date-picker
                  v-model="filter.date"
                  type="date"
                  size="sm"
                  class="w-100"
                  lang="ru"
                  :placeholder="$t('docOn')"
                  value-type="DD.MM.YYYY"
                  format="DD.MM.YYYY"
                  @change="Refresh"
               />
            </b-col>

            <b-col md="2" sm="12" v-if="filter.byMonth">
               <label for>{{ $t('docOn') }}</label>
               <date-picker
                  v-model="filter.date"
                  size="sm"
                  class="w-100"
                  lang="ru"
                  :placeholder="$t('MM.YYYY')"
                  value-type="DD.MM.YYYY"
                  type="month"
                  format="MM.YYYY"
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
         </b-row>
         <b-table-simple bordered responsive>
            <b-thead style="position: sticky; top: 0; z-index: 99">
               <b-tr style="position: sticky; top: 0">
                  <b-th sticky-header style="position: sticky; top: 0; border-top: 0px">
                     {{ $t('employee') }}
                  </b-th>
                  <b-th v-if="!schedule" sticky-header style="position: sticky; top: 0; border-top: 0px">
                     {{ $t('WorkTime') }}
                  </b-th>
                  <b-th v-if="schedule" sticky-header style="position: sticky; top: 0; border-top: 0px">
                     {{ $t('ScheduledWorkTime') }}
                  </b-th>
                  <template v-if="items[0]">
                     <b-th
                        sticky-header
                        v-for="(date, index1) in items[0].turnstileTimeInfos"
                        :key="index1"
                        style="white-space: nowrap; position: sticky; top: 0; border-top: 0px"
                     >
                        {{ date.eventDate }} <br />
                        <span style="font-size: 12px; color: #5f695f">{{ $t(date.weekDay) }}</span>
                     </b-th>
                  </template>
               </b-tr>
            </b-thead>
            <b-tbody>
               <b-tr v-for="(item, index) in items" :key="index">
                  <b-td sticky-column style="border-left: 0px">
                     {{ item.employeeName }} <br />
                     <span style="color: #28c76f; font-size: 12px">{{ item.positionName }}</span>
                  </b-td>
                  <b-td v-if="!schedule">
                     {{ formatTotalTime(item) }}
                  </b-td>
                  <b-td v-if="schedule">
                     {{ formatTotalSTime(item) }}
                  </b-td>
                  <template v-for="(log, logindex) in item.turnstileTimeInfos">
                     <b-td
                        :id="`${index}_${logindex}`"
                        :key="logindex"
                        class="text-center"
                        :class="{ backgroundGrey: !log.workTime }"
                        style="white-space: nowrap"
                        @click="openPopover(index, logindex, item, log.eventDate)"
                        v-if="shouldDisplayLog(item, log, logindex)"
                     >
                        <b-progress
                           :value="convertToMinutes(log)"
                           :max="480"
                           class="mb-3"
                           :variant="ProgressVariant(log)"
                        ></b-progress>
                        {{ formatTime(log) }}

                        <b-popover
                           :show="openPopoverId === `${index}_${logindex}`"
                           :target="`${index}_${logindex}`"
                           placement="auto"
                        >
                           <b-button @click="closePopover(`${index}_${logindex}`)" class="close" aria-label="Close">
                              <span aria-hidden="true">&times;</span>
                           </b-button>

                           <div>
                              <p style="font-weight: bold; font-size: 16px">
                                 {{ HoursByDay.eventOn }}
                              </p>
                              <hr />
                              <ul v-if="HoursByDay.timeReportList.length">
                                 <li
                                    style="list-style: none; padding-bottom: 15px; font-weight: bold"
                                    v-for="(time, timeIndex) in HoursByDay.timeReportList"
                                    :key="timeIndex"
                                 >
                                    {{ time.enterAt }} - {{ time.exitAt }}
                                 </li>
                              </ul>
                           </div>
                        </b-popover>
                     </b-td>
                  </template>
               </b-tr>
            </b-tbody>
         </b-table-simple>

         <div class="mx-2 mb-2">
            <b-row>
               <b-col
                  cols="12"
                  sm="6"
                  class="d-flex align-items-center justify-content-center justify-content-sm-start"
               >
                  <span class="text-muted"
                     >{{ $t('Showing') }} {{ firstNumber }} {{ $t('to') }} {{ lastNumber }} {{ $t('of') }}
                     {{ filter.totalRows }} {{ $t('entries') }}</span
                  >
                  <v-select
                     v-model="filter.pageSize"
                     :dir="$store.state.appConfig.isRTL ? 'rtl' : 'ltr'"
                     :options="filter.pageSizeOptions"
                     :clearable="false"
                     @input="Refresh"
                     class="per-page-selector d-inline-block ml-50 mr-1"
                  />
               </b-col>
               <!-- Pagination -->
               <b-col cols="12" sm="6" class="d-flex align-items-center justify-content-center justify-content-sm-end">
                  <b-pagination
                     v-model="filter.pageIndex"
                     :total-rows="filter.totalRows"
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
      </b-card>
   </div>
</template>

<script>
import {
   BCol,
   BTableSimple,
   VBTooltip,
   BRow,
   BCard,
   BTooltip,
   BTh,
   BTr,
   BThead,
   BTbody,
   BTd,
   BProgress,
   BInputGroup,
   BInputGroupAppend,
   BPagination,
   BButton,
   BButtonGroup,
   BFormInput,
   BPopover
} from 'bootstrap-vue';
import EmployeeTurnstileReportService from '@/services/hrm/employeeturnstilereport.service';
export default {
   components: {
      BCol,
      BRow,
      VBTooltip,
      BCard,
      BTooltip,
      BTableSimple,
      BTh,
      BTr,
      BThead,
      BTd,
      BTbody,
      BProgress,
      BInputGroup,
      BInputGroupAppend,
      BPagination,
      BButton,
      BButtonGroup,
      BFormInput,
      BPopover
   },
   directives: {
      'b-tooltip': VBTooltip
   },
   data() {
      return {
         currentTime: '',
         interval: null,
         popoverShow: false,
         showPopover: false,
         schedule: false,
         popoverTarget: null,
         openPopoverId: null,
         selectedEmployeeData: null,
         filter: {
            pageSize: 20,
            pageIndex: 1,
            pageSizeOptions: [10, 20, 50, 100],
            totalRows: 0,
            byMonth: false,
            byWeek: false,
            byDay: true,
            organizationId: null,
            date: ''
         },
         HoursByDay: { timeReportList: [] },
         items: [{ turnstileTimeInfos: [{ eventDate: null }] }],

         isBusy: false,
         pageKey: 'getemployeeturnstiletimereport',
         searchInputObject: {}
      };
   },
   mounted() {
      this.setCurrentTime();
      this.interval = setInterval(this.setCurrentTime, 1000);
   },
   beforeDestroy() {
      clearInterval(this.interval);
   },
   created() {
      var todaydate = new Date();
      var dd = String(todaydate.getDate()).padStart(2, '0');
      var mm = String(todaydate.getMonth() + 1).padStart(2, '0');
      var yyyy = todaydate.getFullYear();
      this.filter.enterTime = '09:00:00';
      this.filter.exitTime = '18:00:00';
      this.filter.organizationId = JSON.parse(localStorage.getItem('user_info')).organizationId;
      this.filter.date = dd + '.' + mm + '.' + yyyy;
      this.Refresh();
   },
   computed: {
      firstNumber() {
         return (this.filter.pageIndex - 1) * this.filter.pageSize + 1;
      },
      lastNumber() {
         if (this.filter.totalRows < this.filter.pageSize) {
            return this.filter.totalRows;
         } else {
            if (this.filter.pageIndex * this.filter.pageSize > this.filter.totalRows) {
               return this.filter.totalRows;
            } else {
               return this.filter.pageIndex * this.filter.pageSize;
            }
         }
      },
      formatTime() {
         return function (timeString) {
            if (this.schedule) {
               if (timeString.scheduledWorkTime) {
                  let [hours, minutes] = timeString.scheduledWorkTime.split(':');
                  hours = parseInt(hours, 10);
                  return `${hours}:${minutes}:00`;
               } else {
                  return '0h 0m';
               }
            } else {
               if (timeString.workTime) {
                  let [hours, minutes] = timeString.workTime.split(':');
                  hours = parseInt(hours, 10);
                  return `${hours}:${minutes}:00`;
               } else {
                  return '0h 0m';
               }
            }
         };
      },
      formatTotalTime() {
         return function (timeString) {
            if (timeString.totalWorkedTime) {
               let [hours, minutes] = timeString.totalWorkedTime.split(':');
               hours = parseInt(hours, 10);
               return `${hours}:${minutes}:00`;
            } else {
               return '0h 0m';
            }
         };
      },
      formatTotalSTime() {
         return function (timeString) {
            if (timeString.totalScheduledWorkTime) {
               let [hours, minutes] = timeString.totalScheduledWorkTime.split(':');
               hours = parseInt(hours, 10);
               return `${hours}:${minutes}:00`;
            } else {
               return '0h 0m';
            }
         };
      },
      ProgressVariant() {
         return function (timeString) {
            if (timeString.isLate) {
               return 'danger';
            } else if (timeString.isLeaveEarly) {
               return 'warning';
            } else {
               return 'success';
            }
         };
      }
   },
   methods: {
      shouldDisplayLog(item, log, logindex) {
         // Always show the first log
         if (logindex === 0) return true;

         // Compare with the previous log's eventDate
         const prevLog = item.turnstileTimeInfos[logindex - 1];
         return log.eventDate !== prevLog.eventDate;
      },
      async openPopover(index, logindex, employeeData, eventDate) {
         await EmployeeTurnstileReportService.GetEmployeeTurnstileReportById({
            onDate: eventDate,
            endDate: eventDate,
            employeeId: employeeData.employeeId,
            organizationId: this.filter.organizationId
         })
            .then((res) => {
               [this.HoursByDay] = res.data;
               if (res.data.length == 0) {
                  this.HoursByDay = {};
               }
            })
            .catch((error) => {
               this.makeToast(error.response.data, 'danger');

               this.HoursByDay = {};
            });
         setTimeout(() => {
            this.openPopoverId = `${index}_${logindex}`;
         }, 0);
      },

      closePopover() {
         this.openPopoverId = null;
      },
      OpenByDetails() {
         this.popoverShow = true;
      },

      Refresh() {
         this.isBusy = true;

         EmployeeTurnstileReportService.GetEmployeeTurnstileTimeReport(this.filter)
            .then((res) => {
               this.items = res.data.rows;
               if (res.data.rows == null) {
                  this.items = [{ turnstileTimeInfos: [{ eventDate: null }] }];
               }
               this.filter.totalRows = res.data.total;
               this.isBusy = false;
            })
            .catch((error) => {
               this.makeToast(error.response.data, 'danger');
            });
      },
      convertToMinutes(timeString) {
         if (timeString.workTime) {
            if (this.schedule) {
               const [hours, minutes] = timeString.scheduledWorkTime.split(':').map(Number);
               return hours * 60 + minutes;
            } else {
               const [hours, minutes] = timeString.workTime.split(':').map(Number);
               return hours * 60 + minutes;
            }
         }
      },
      setCurrentTime() {
         const now = new Date();
         const hours = now.getHours();
         const minutes = now.getMinutes().toString().padStart(2, '0');

         const day = now.getDate().toString().padStart(2, '0');
         const monthNames = ['01', '02', '03', '04', '05', '06', '07', '08', '09', '10', '11', '12'];
         const month = monthNames[now.getMonth()];
         const year = now.getFullYear();

         this.currentTime = ` ${day}.${month}.${year}, ${hours}:${minutes}`;
      }
   },
   beforeDestroy() {
      if (this.interval) {
         clearInterval(this.interval);
      }
   }
};
</script>

<style lang="scss" scoped>
.backgroundGrey {
   background-color: rgb(222, 222, 222) !important;
}
thead tr th {
   font-weight: bold;
   font-size: 14px !important;
}
tbody tr td {
   font-weight: bold;
}
.progress {
   margin-bottom: 5px !important;
}
</style>
<style lang="scss">
.popover {
   max-width: 260px !important;
}
.popover.b-popover {
   max-width: 260px !important;
   width: 260px !important;
}
</style>
