<template>
   <b-overlay :show="show">
      <b-row>
         <b-col sm="12" md="12" lg="12">
            <b-card>
               <validation-observer ref="ValidationDTO">
                  <b-row>
                     <b-col sm="12" md="3">
                        <div class="form-group">
                           <form-input v-model="Data.docNumber" required :label="$t('docnumber')" />
                        </div>
                     </b-col>
                     <b-col sm="12" md="3">
                        <form-picker v-model="Data.docOn" :label="$t('docOn')" :placeholder="$t('docOn')"></form-picker>
                     </b-col>
                     <b-col sm="12" md="3">
                        <form-select :options="DepartmentList" v-model="Data.departmentId" label="Department" />
                     </b-col>
                     <b-col sm="12" md="3">
                        <form-select
                           :options="TimeSheetTypeList"
                           v-model="Data.timesheetTypeId"
                           required-star
                           label="timesheetType"
                        />
                     </b-col>
                     <b-col sm="12" md="3">
                        <form-select :options="MonthList" v-model="Data.month" requitred-star label="month" />
                     </b-col>
                     <!-- <b-col sm="12" md="12">
                        <b-form-textarea
                           id="textarea"
                           v-model="Data.details"
                           :placeholder="$t('detailinfo')"
                           rows="3"
                           max-rows="6"
                        ></b-form-textarea>
                     </b-col> -->
                  </b-row>

                  <b-row class="mt-2">
                     <b-col sm="12" md="12" class="d-flex justify-content-center">
                        <b-button
                           v-if="Data.statusId != 2"
                           class="mr-3"
                           variant="success"
                           :disabled="fillLoading || Data.tables?.length > 0"
                           @click="FillTimeSheet"
                        >
                           <div v-if="fillLoading" class="spinner-border spinner-border-sm mr-2" role="status">
                              <span class="sr-only" />
                           </div>
                           {{ $t('FillTimeSheet') }}
                        </b-button>
                        <b-button
                           v-if="Data.statusId != 2 && Data.id > 0"
                           width="192px"
                           class="mr-3"
                           variant="warning"
                           :disabled="Data.tables?.length == 0"
                           @click="Clear"
                        >
                           {{ $t('clear') }}
                        </b-button>
                        <b-button
                           width="192px"
                           class="mr-3"
                           variant="primary"
                           :disabled="Data.tables?.length == 0 || PrintLoading"
                           @click="Print"
                        >
                           <feather-icon icon="PrinterIcon"></feather-icon>
                        </b-button>
                     </b-col>
                  </b-row>
                  <hr />
                  <!-- <div class="d-flex justify-content-center">
                     <b-col sm="12" md="4" lg="4">
                        <form-input-hrm
                           v-model.trim="searchData"
                           style="width: 100%"
                           bg="white"
                           class="mr-2"
                           :label="$t('search')"
                        />
                     </b-col>
                  </div> -->

                  <div class="overflow-auto">
                     <b-table
                        class="position-relative"
                        :fields="fields"
                        :items="secondData"
                        head-row-variant="secondary"
                        @selectedItem="selectedItem"
                        :filter="searchData"
                        show-empty
                        :empty-text="$t('NotFound')"
                     >
                        <template #cell(days)="{ item }">
                           <p class="m-0 p-0" style="white-space: nowrap">
                              {{ item.factDays }} / <b>{{ item.planDays }} </b>
                           </p>
                        </template>
                        <template #cell(hours)="{ item }">
                           <p class="m-0 p-0" style="white-space: nowrap">
                              {{ item.factHours }} / <b>{{ item.planHours }} </b>
                           </p>
                        </template>
                        <template #cell(employmentType)="{ item }">
                           <p class="m-0 p-0" style="white-space: nowrap">
                              {{ item.employmentType }} <b>({{ item.docId }} )</b>
                           </p>
                        </template>
                        <template #cell(fromDate)="{ item }">
                           <p class="m-0 p-0" style="white-space: nowrap">
                              {{ item.startOn ? item.startOn.split('.')[0] : ' ' }} -
                              {{ item.startOn ? item.endOn.split('.')[0] : '' }}
                           </p>
                        </template>
                        <template #cell(action)="{ item, index }">
                           <span style="cursor: pointer">
                              <feather-icon
                                 v-if="item.id != 0"
                                 class="mr-1"
                                 @click="Edit(item, index)"
                                 icon="EditIcon"
                              ></feather-icon>
                              <feather-icon
                                 v-if="item.id == 0"
                                 class="mr-1"
                                 @click="EditCreate(item, index)"
                                 icon="EditIcon"
                              ></feather-icon>
                           </span>
                        </template>
                     </b-table>
                  </div>

                  <!-- <b-row class="mt-3">
                     <b-col sm="12" md="6" lg="6" class="text-left"></b-col>
                     <b-col sm="12" md="6" lg="6" class="text-right">
                        <b-button :disabled="saveLoading" @click="SaveData" size="sm" variant="outline-success">
                           <feather-icon icon="CheckIcon"></feather-icon>
                           {{ $t('Save') }}
                        </b-button>
                     </b-col>
                  </b-row> -->
               </validation-observer>
            </b-card>
         </b-col>
      </b-row>
      <!-- table modal -->
      <validation-observer ref="ValidationTabrow">
         <b-modal size="xl" v-model="TimeSheetModal" v-if="TimeSheetModal" static no-close-on-backdrop scrollable>
            <template #modal-header="{}">
               <b-row class="w-100">
                  <b-col sm="12" md="4">
                     {{ $t('employee') }} : <span> {{ Tables.employee }}</span>
                  </b-col>
                  <b-col>
                     {{ $t('Department') }} : <span>{{ Tables.department }}</span>
                  </b-col>
                  <b-col>
                     {{ $t('position') }} : <span> {{ Tables.position }}</span>
                  </b-col>
                  <b-col>
                     {{ $t('employmentType') }} :
                     <span> {{ Tables.employmentType }}</span>
                  </b-col>
               </b-row>
            </template>
            <b-table
               class="py-3"
               :fields="fieldsItem"
               :items="Tables.tableDays"
               head-row-variant="secondary"
               @selectedItem="selectedItem"
            >
               <template #cell(dateOn)="{ item }">
                  <p class="m-0 p-0" style="white-space: nowrap">
                     {{ item.dateOn.split('.')[0] }}
                     <b> ( {{ getWeekday(item) }} ) </b>
                  </p>
               </template>
               <template #cell(timeSheetIndicatorName)="{ item }">
                  <div style="width: 200px">
                     <form-select
                        v-model="item.timeSheetIndicatorId"
                        :options="TimeSheetIndicatorList"
                        :label="$t('timeSheetIndicator')"
                     />
                  </div>
               </template>

               <template #cell(maintenanceHours)="{ item }">
                  <form-input-hrm v-model="item.maintenanceHours" :label="$t('maintenanceHours')" />
               </template>
               <template #cell(factHours)="{ item }">
                  <form-input-hrm v-model="item.factHours" :label="$t('factHours')" />
               </template>
               <template #cell(factDays)="{ item }">
                  <form-input-hrm v-model="item.factDays" :label="$t('factDays')" />
               </template>
               <template #cell(hourly)="{ item }">
                  <form-input-hrm v-model="item.hourly" :label="$t('hourly')" />
               </template>
               <template #cell(nightHours)="{ item }">
                  <form-input-hrm v-model="item.nightHours" :label="$t('nightHours')" />
               </template>
               <template #cell(planDays)="{ item }">
                  <form-input-hrm v-model="item.planDays" :label="$t('planDays')" />
               </template>
               <template #cell(planHours)="{ item }">
                  <form-input-hrm v-model="item.planHours" :label="$t('planHours')" disabled />
               </template>
            </b-table>

            <template #modal-footer="{}">
               <div class="d-flex align-items-center justify-content-end px-2">
                  <b-button variant="secondary" @click="TimeSheetModal = false">
                     {{ $t('back') }}
                  </b-button>
                  <b-button
                     v-if="Data.statusId != 2 && Data?.id != 0"
                     class="ml-2"
                     variant="success"
                     @click="SaveDataItem"
                  >
                     {{ $t('Save') }}
                  </b-button>
                  <b-button v-if="Data?.id == 0" class="ml-2" variant="success" @click="SaveDataItemCreate">
                     {{ $t('Save') }}
                  </b-button>
               </div>
            </template>
         </b-modal>
      </validation-observer>
   </b-overlay>
</template>
<script>
// service
import ManualService from '@/services/others/manual.service';
import OrganizationService from '@/services/managment/organization.service';
import TimesheetService from '@/services/hrm/timesheet.service';
import DepartmentService from '@/services/info/department.service';
import EmployeeService from '@/services/info/employee.service';

// components
import {
   BOverlay,
   BCard,
   BCardBody,
   BRow,
   BCol,
   BFormInput,
   BTabs,
   BTable,
   BTab,
   BButton,
   BLink,
   BFormGroup,
   VBTooltip,
   BModal,
   VBModal,
   BCardText,
   BInputGroup,
   BInputGroupAppend,
   BTr,
   BTd,
   BFormCheckbox,
   BFormTextarea
} from 'bootstrap-vue';

export default {
   components: {
      BOverlay,
      BCard,
      BCardBody,
      BRow,
      BCol,
      BFormInput,
      BTabs,
      BTab,
      BButton,
      BTable,
      BLink,
      BFormGroup,
      VBTooltip,
      BModal,
      VBModal,
      BCardText,
      BInputGroup,
      BInputGroupAppend,
      BTr,
      BTd,
      BFormCheckbox,
      BFormTextarea
   },
   name: 'Edit',
   data() {
      return {
         show: false,
         secondData: [],
         DepartmentList: [],
         PrintLoading: false,
         EmployeeList: [],
         TimeSheetTypeList: [],
         TimeSheetIndicatorList: [],
         MonthList: [],
         YearList: [],
         Tables: [],
         loadingButton: false,
         TimeSheetModal: false,
         saveLoading: false,
         fillLoading: false,
         Data: {
            id: null,
            docNumber: null,
            docOn: null,
            month: null,
            details: null,
            departmentId: null,
            timesheetTypeId: null,
            tables: []
         },
         fields: [],
         fieldsItem: [],
         selectItem: {},
         searchData: ''
      };
   },

   created() {
      this.show = true;
      TimesheetService.Get(this.$route.params.id)
         .then((res) => {
            const today = new Date();
            const yyyy = today.getFullYear();
            this.Data = res.data;
            this.secondData = res.data.tables;
            this.Data.monthOn = this.Data.docOn;
            this.getFields();
            this.YearList = [
               { text: `${yyyy - 1}`, value: yyyy - 1 },
               { text: `${yyyy}`, value: yyyy },
               { text: `${yyyy + 1}`, value: yyyy + 1 }
            ];
         })
         .catch((error) => {
            this.showApiError(error);
         })
         .finally(() => {
            this.show = false;
         });

      DepartmentService.GetAsSelectList(null, {})
         .then((res) => {
            if (Array.isArray(res.data)) {
               this.DepartmentList = res.data;
            }
         })
         .catch((error) => {
            this.showApiError(error);
         });

      EmployeeService.GetAsSelectList({})
         .then((res) => {
            const { rows } = res.data;
            if (Array.isArray(rows)) {
               this.EmployeeList = rows;
            }
         })
         .catch((error) => {
            this.showApiError(error);
         });

      OrganizationService.GetAsSelectListOrgSettlementAccount()
         .then((res) => {
            if (Array.isArray(res.data)) {
               this.OrgSettlementAccountList = res.data;
            }
         })
         .catch((error) => {
            this.showApiError(error);
         });

      ManualService.TimesheetTypeSelectList().then((res) => {
         this.TimeSheetTypeList = res.data;
      });

      ManualService.TimesheetIndicatorSelectList().then((res) => {
         this.TimeSheetIndicatorList = res.data;
      });

      ManualService.GetMonthSelectList().then((res) => {
         this.MonthList = res.data;
      });
   },
   methods: {
      selectedItem(item) {
         this.selectItem = item;
      },
      getFields() {
         this.fields = [
            {
               key: 'employee',
               label: this.$t('employee')
            },
            {
               key: 'employeeManageId',
               label: this.$t('employeeManage')
            },
            {
               key: 'department',
               label: this.$t('Department')
            },
            {
               key: 'position',
               label: this.$t('position')
            },
            {
               key: 'documentInfo',
               label: this.$t('documentid')
            },
            {
               key: 'employmentRate',
               label: this.$t('employeeRate')
            },
            {
               key: 'employmentType',
               label: this.$t('employmentType')
            },
            {
               key: 'startOn',
               label: this.$t('datefrom')
            },
            {
               key: 'days',
               label: this.$t('days')
            },
            {
               key: 'hours',
               label: this.$t('hours')
            },
            {
               key: 'dayOffHours',
               label: this.$t('dayOffHours')
            },

            {
               key: 'nightHours',
               label: this.$t('nightHours')
            },
            {
               key: 'action',
               label: this.$t('actions'),
               stickyColumn: true,
               thStyle: {
                  right: '0'
               },
               tdClass: 'r-0'
            }
         ];
      },
      getFieldsItem() {
         this.fieldsItem = [
            {
               key: 'dateOn',
               label: this.$t('onDate')
            },
            {
               key: 'timeSheetIndicatorName',
               label: this.$t('timeSheetIndicator')
            },
            {
               key: 'factHours',
               label: this.$t('factHours')
            },
            {
               key: 'factDays',
               label: this.$t('factDays')
            },
            {
               key: 'hourly',
               label: this.$t('hourly')
            },
            {
               key: 'nightHours',
               label: this.$t('nightHours')
            },
            {
               key: 'planDays',
               label: this.$t('planDays')
            },
            {
               key: 'planHours',
               label: this.$t('planHours')
            }
         ];
      },
      getWeekday(item) {
         const d = new Date(
            item.dateOn.split('.')[2],
            item.dateOn.split('.')[1] - 1,
            item.dateOn.split('.')[0]
         ).getDay();
         if (d === 0) {
            return 'Вс';
         }
         if (d === 1) {
            return 'Пн';
         }
         if (d === 2) {
            return 'Вт';
         }
         if (d === 3) {
            return 'Ср';
         }
         if (d === 4) {
            return 'Чт';
         }
         if (d === 5) {
            return 'Пт';
         }
         if (d === 6) {
            return 'Сб';
         }
      },
      Edit(item) {
         TimesheetService.GetTable(item.id).then((res) => {
            this.Tables = res.data;
         });
         this.getFieldsItem();
         this.TimeSheetModal = true;
      },
      EditCreate(item) {
         this.Tables = item;
         this.getFieldsItem();
         this.TimeSheetModal = true;
      },
      SaveDataItem() {
         TimesheetService.UpdateTable(this.Tables)
            .then(() => {
               this.makeToast(this.$t('SuccessSave'), 'success');
               this.TimeSheetModal = false;
               this.GetID(this.Data.id);
            })
            .catch((error) => {
               this.showApiError(error);
            });
      },
      SaveDataItemCreate(item) {
         this.TimeSheetModal = false;
      },
      FillTimeSheet() {
         this.$refs.ValidationDTO.validate().then(async (success) => {
            if (success) {
               try {
                  if (this.$route.params.id == 0) {
                     this.saveLoading = true;
                     await TimesheetService.Update(this.Data)
                        .then((res) => {
                           this.Data.id = res.data.id;
                           this.$router.push({ name: 'EditTimesheet', params: { id: res.data.id } });
                        })
                        .finally(() => {
                           this.saveLoading = false;
                        });
                  }
                  this.fillLoading = true;
                  TimesheetService.FillTimeSheet(this.Data)
                     .then((res) => {
                        this.GetID(res.data.id);
                        this.Data.id = res.data.id;
                        this.fillLoading = false;
                     })
                     .catch((error) => {
                        this.showApiError(error);
                        this.fillLoading = false;
                     });
               } catch (e) {
                  this.showApiError(error);
               }
            }
         });
      },
      GetID(id) {
         this.fillLoading = true;
         TimesheetService.Get(id)
            .then((res) => {
               this.Data.tables = res.data.tables;
               this.secondData = res.data.tables;
               this.fillLoading = false;
               this.getFields();
            })
            .catch((error) => {
               this.showApiError(error);

               this.fillLoading = false;
            });
      },
      Clear() {
         if (this.Data.id != 0) {
            TimesheetService.Clear({
               id: this.Data.id,
               tableIds: null,
               createLog: true
            })
               .then((res) => {
                  this.Data.tables = res.data.tables;
                  this.secondData = res.data.tables;
                  this.getFields();
               })
               .catch((error) => {
                  this.showApiError(error);
               });
         } else {
            this.Data.tables = [];
         }
      },
      Print() {
         this.PrintLoading = true;
         TimesheetService.SaveAsExecelForTabel(this.$route.params.id)
            .then((res) => {
               this.PrintLoading = false;

               this.forceFileDownload(res, this.$t('Table'));
            })
            .catch((error) => {
               this.PrintLoading = false;
               this.showApiError(error);
            })
            .finally(() => {
               this.PrintLoading = false;
            });
      }
      // SaveData() {
      //    this.$refs.ValidationDTO.validate().then((success) => {
      //       if (success) {
      //          this.saveLoading = true;
      //          TimesheetService.Update(this.Data)
      //             .then(() => {
      //                this.makeToast(this.$t('SaveSuccess'), 'success');
      //                this.$router.push({ name: 'Timesheet' });
      //             })
      //             .catch((err) => {
      //                this.makeToast(this.$t(err), 'danger');
      //             })
      //             .finally(() => {
      //                this.saveLoading = false;
      //             });
      //       }
      //    });
      // }
   }
};
</script>

<style>
.table1 table td {
   min-width: 200px;
   max-width: 600px;
}

.r-0 {
   right: 0;
}
</style>
