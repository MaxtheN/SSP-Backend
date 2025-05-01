<template>
   <div>
      <b-card no-body>
         <b-row class="m-1">
            <b-col cols="12" md="2">
               <div>
                  <label for>{{ $t('Hujjat sanasi (dan)') }}</label>
                  <form-picker v-model="filter.startDate" :placeholder="$t('Hujjat sanasi (dan)')" @change="refresh" />
               </div>
               <div>
                  <label for>{{ $t('Hujjat sanasi (gacha)') }}</label>
                  <form-picker v-model="filter.endDate" :placeholder="$t('Hujjat sanasi (gacha)')" @change="refresh" />
               </div>
            </b-col>
            <b-col cols="12" md="2">
               <div>
                  <label for>{{ $t('Yaratilingan sana(dan)') }}</label>
                  <form-picker
                     v-model="filter.startCreatedAt"
                     :placeholder="$t('Yaratilingan sana(dan)')"
                     @change="refresh"
                  />
               </div>
               <div>
                  <label for>{{ $t('Yaratilingan sana(gacha)') }}</label>
                  <form-picker
                     v-model="filter.endCreatedAt"
                     :placeholder="$t('Yaratilingan sana(gacha)')"
                     @change="refresh"
                  />
               </div>
            </b-col>
            <b-col cols="auto">
               <div class="mt-1">
                  <b-form-checkbox
                     v-model="filter.acting"
                     class="mr-0 mt-50"
                     @change="
                        () => {
                           (filter.interm = false), refresh();
                        }
                     "
                     name="is-rtl"
                     >{{ $t('V.B') }}
                  </b-form-checkbox>
               </div>
               <div>
                  <b-form-checkbox
                     v-model="filter.interm"
                     class="mr-0 mt-50"
                     @change="
                        () => {
                           (filter.acting = false), refresh();
                        }
                     "
                     name="is-rtl"
                     >{{ $t('V.B.B') }}</b-form-checkbox
                  >
               </div>
               <div>
                  <b-form-checkbox
                     v-model="filter.isProbation"
                     @change="
                        () => {
                           refresh();
                        }
                     "
                     class="mr-0 mt-50"
                     name="is-rtl"
                     >{{ $t('isProbation') }}</b-form-checkbox
                  >
               </div>
            </b-col>
            <b-col cols="auto">
               <div class="mt-1">
                  <b-form-checkbox
                     v-model="filter.byQuantity"
                     class="mr-0 mt-50"
                     @change="
                        () => {
                           (filter.byQuantityForNow = false), refresh();
                        }
                     "
                     name="is-rtl"
                     >{{ $t('Vakant lavozimlar') }}
                  </b-form-checkbox>
               </div>
               <div>
                  <b-form-checkbox
                     v-model="filter.byQuantityForNow"
                     class="mr-0 mt-50"
                     @change="
                        () => {
                           (filter.byQuantity = false), refresh();
                        }
                     "
                     name="is-rtl"
                     >{{ $t('Band lavozimlar') }}</b-form-checkbox
                  >
               </div>
            </b-col>
            <b-col></b-col>
            <b-col cols="12" md="3">
               <b-input-group class="text-right mt-2">
                  <b-form-input v-model="filter.search" :placeholder="$t('search')" />
                  <b-input-group-append>
                     <b-button @click="refresh" variant="primary">
                        <feather-icon icon="SearchIcon" />
                     </b-button>
                  </b-input-group-append>
               </b-input-group>
            </b-col>
         </b-row>
         <form-table-hrm
            :items="items"
            :actions="{}"
            :fields="fields"
            :filter.sync="filter"
            :busy="isBusy"
            @request="refresh"
            :hover="false"
            bordered
            :no-border-collapse="false"
         >
            <template #head(quantity)="{}">
               {{ $t('quantity') }} <br />
               ({{ allQuantity }})
            </template>
            <template #head(quantityForNow)="{}">
               {{ $t('quantityForNow') }} <br />
               ({{ allQuantityForNov }})
            </template>
            <template #head(employees)="{}">
               {{ $t('employees') }} <br />
               ({{ employeeCount }})
            </template>
            <template #cell(departmentId)="{ item }">
               {{ item.department }} ({{ countPosition(item.departmentId) }})
            </template>
            <template #cell(employees)="{ item }">
               <table class="w-100 border-left-0 border-top-0">
                  <tr
                     v-for="(emp, i) in item.employeeManageTables"
                     :key="i + emp.pinfl + 'nimadr'"
                     style="border: none"
                  >
                     <td style="min-width: 150px">
                        <template v-if="emp.appointEmployees && emp.appointEmployees.length > 0">
                           <p v-if="emp.appointEmployees[0].acting">{{ $t('V.B') }}</p>
                           <p v-if="emp.appointEmployees[0].interm">{{ $t('V.B.B') }}</p>
                           <!-- <feather-icon icon="CheckCircleIcon"  class="text-success" />
                     <feather-icon icon="XCircleIcon" v-else class="text-danger" /> -->
                        </template>
                     </td>

                     <td style="min-width: 250px">
                        <b-link :to="{ name: 'EmployeeCard', query: { pinfl: emp.pinfl } }">{{ emp.employees }}</b-link>
                     </td>
                     <td style="min-width: 150px">{{ emp.employeeRate }}</td>
                     <td style="min-width: 150px">
                        <template v-if="emp.appointEmployees && emp.appointEmployees.length > 0">
                           {{ emp.appointEmployees[0].docNumber }} <br />
                           {{ emp.appointEmployees[0].docOn }}
                        </template>
                     </td>

                     <td style="min-width: 150px">
                        <template v-if="emp.appointEmployees && emp.appointEmployees.length > 0">
                           {{ (emp.appointEmployees[0].createdAt + '').slice(0, 10) }}
                        </template>
                     </td>
                     <td style="min-width: 200px">
                        <div class="text-center" style="text-wrap: nowrap">
                           <b-dropdown variant="light" no-caret class="rounded-circle">
                              <template #button-content>
                                 <feather-icon icon="MoreHorizontalIcon"></feather-icon>
                              </template>
                              <b-dropdown-item
                                 :to="{
                                    name: 'EditAppointEmployee',
                                    params: { id: 0 },
                                    query: { empAppointOrderTypeId: 2, employeeManageId: emp.employeeManageId }
                                 }"
                                 >{{ $t('Boshqa lavozimga o‘tkazish') }}</b-dropdown-item
                              >

                              <b-dropdown-item
                                 :to="{
                                    name: 'EditAppointEmployee',
                                    params: { id: 0 },
                                    query: { empAppointOrderTypeId: 4, employeeManageId: emp.employeeManageId }
                                 }"
                                 >{{ $t('Bir nechta kasb va lavozimda ishlash') }}
                              </b-dropdown-item>

                              <b-dropdown-item
                                 :to="{
                                    name: 'EditEmployeeSendTrain',
                                    params: { id: 0 },
                                    query: { employeeId: emp.employeeId, departmentId: item.departmentId }
                                 }"
                                 >{{ $t('employeesendtrain') }}</b-dropdown-item
                              >
                              <b-dropdown-item
                                 :to="{
                                    name: 'EditEmployeeSickLeave',
                                    params: { id: 0 },
                                    query: { employeeManageId: emp.employeeManageId }
                                 }"
                                 >{{ $t('employeesickleave') }}</b-dropdown-item
                              >
                              <b-dropdown-item
                                 :to="{
                                    name: 'EditEmployeeLeaveOrder',
                                    params: { id: 0 },
                                    query: { employeeManageId: emp.employeeManageId }
                                 }"
                                 >{{ $t('employeeleaveorder') }}</b-dropdown-item
                              >

                              <b-dropdown-item
                                 :to="{
                                    name: 'EditRecallLeave',
                                    params: { id: 0 },
                                    query: { employeeManageId: emp.employeeManageId }
                                 }"
                                 >{{ $t('RecallLeave') }}</b-dropdown-item
                              >

                              <b-dropdown-item
                                 :to="{
                                    name: 'EditOrderToSendBusinessTrip',
                                    params: { id: 0 },
                                    query: { employeeManageId: emp.employeeManageId }
                                 }"
                                 >{{ $t('OrderToSendBusinessTrip') }}</b-dropdown-item
                              >
                              <b-dropdown-item
                                 :to="{
                                    name: 'EditChastisement',
                                    params: { id: 0 },
                                    query: { employeeManageId: emp.employeeManageId }
                                 }"
                                 >{{ $t('Chastisement') }}</b-dropdown-item
                              >
                              <b-dropdown-item
                                 :to="{
                                    name: 'EditWorkDayOff',
                                    params: { id: 0 },
                                    query: { employeeManageId: emp.employeeManageId }
                                 }"
                                 >{{ $t('WorkDayOff') }}</b-dropdown-item
                              >
                              <b-dropdown-item
                                 :to="{
                                    name: 'EditTempCalcKind',
                                    params: { id: 0 },
                                    query: { employeeManageId: emp.employeeManageId }
                                 }"
                                 >{{ $t('tempcalckind') }}</b-dropdown-item
                              >
                              <b-dropdown-item
                                 :to="{
                                    name: 'EditEmployeeSendStudy',
                                    params: { id: 0 },
                                    query: { employeeManageId: emp.employeeManageId }
                                 }"
                                 >{{ $t('employeesendstudy') }}</b-dropdown-item
                              >
                              <b-dropdown-item
                                 :to="{
                                    name: 'EditBolaparvarishi',
                                    params: { id: 0 },
                                    query: { employeeManageId: emp.employeeManageId }
                                 }"
                                 >{{ $t('Bolaparvarishi') }}</b-dropdown-item
                              >
                              <b-dropdown-item
                                 v-if="emp.genderId == 2"
                                 :to="{
                                    name: 'EditHomiladorliktatili',
                                    params: { id: 0 },
                                    query: { employeeManageId: emp.employeeManageId }
                                 }"
                                 >{{ $t('Homiladorliktatili') }}</b-dropdown-item
                              >
                           </b-dropdown>
                           <b-button
                              pill
                              variant="light"
                              :to="{
                                 name: 'EditAppointEmployee',
                                 params: { id: 0 },
                                 query: {
                                    empAppointOrderTypeId: 3,
                                    employeeManageId: emp.employeeManageId
                                 }
                              }"
                           >
                              <img src="@/assets/svg/person-walking.svg" alt="" />
                           </b-button>
                        </div>
                     </td>
                  </tr>

                  <tr v-if="item.quantityForNow > 0">
                     <td colspan="100%" class="border-0">
                        <!-- edit -->
                        <b-link
                           v-if="$can('AppointEmployeeEdit', 'permissions') && item.quantityForNow > 0"
                           :to="{
                              name: 'EditAppointEmployee',
                              params: { id: 0 },
                              query: {
                                 empAppointOrderTypeId: 1,
                                 positionId: item.positionId,
                                 departmentId: item.departmentId
                              }
                           }"
                           class="mr-1 cursor-pointer"
                        >
                           Ходимни расмийлаштириш
                        </b-link>
                     </td>
                  </tr>
               </table>
            </template>
            <template #cell(status)="{ item }">
               <b-badge :variant="getColor(item)">{{ item.status }}</b-badge>
            </template>
         </form-table-hrm>
      </b-card>
   </div>
</template>

<script>
import {
   BFormCheckbox,
   BInputGroup,
   BFormInput,
   BInputGroupAppend,
   BRow,
   BTable,
   BTr,
   BTd,
   BCard,
   BCardText,
   VBTooltip,
   BBadge,
   BButton,
   BLink,
   BModal,
   VBModal,
   BDropdown,
   BDropdownItem,
   BCol,
   BOverlay
} from 'bootstrap-vue';
import FormTableHrm from '@/components/forms/form-table-hrm.vue';
import ReportService from '@/services/report/report.service';
import EmployeeManageService from '@/services/hrm/employeemanage.service';
import DepartmentService from '@/services/info/department.service';
import EmployeeService from '@/services/info/employee.service';
import ManualService from '@/services/others/manual.service';
import StaffingService from '@/services/hrm/staffing.service';
const DefaultFilter = {
   acting: false,
   isProbation: false,
   interm: false,
   byQuantity: false,
   byQuantityForNow: false,
   startDate: '',
   startCreatedAt: '',
   endCreatedAt: '',
   createdAt: '',
   endDate: '',
   search: '',
   sortBy: '',
   orderType: 'asc',
   page: 1,
   pageSize: 20,
   perPageOptions: [10, 20, 50, 100, 300],
   total: 0
};
export default {
   components: {
      BFormCheckbox,
      BInputGroup,
      BFormInput,
      BInputGroupAppend,
      BTable,
      BCol,
      BRow,
      BTr,
      BTd,
      BCard,
      FormTableHrm,
      BCardText,
      BButton,
      BBadge,
      BLink,
      BModal,
      BDropdown,
      BDropdownItem,
      BOverlay
   },
   directives: {
      'b-tooltip': VBTooltip,
      'b-modal': VBModal
   },
   data() {
      return {
         DepartmentList: [],
         EmployeeList: [],
         PositionList: [],
         items: [],
         employeeCount: 0,
         fields: [
            {
               key: 'departmentId',
               label: this.$t('Department'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true,
               tdAttr(value, key, item) {
                  return {
                     rowSpan: item.rowSpan,
                     hide: !item.rowSpan
                  };
               }
            },
            {
               key: 'position',
               label: this.$t('position'),
               thClass: 'text-left',
               tdClass: 'text-left'
            },
            {
               key: 'quantity',
               label: this.$t('quantity'),
               thClass: 'text-center',
               tdClass: 'text-center'
            },
            {
               key: 'quantityForNow',
               label: this.$t('quantityForNow'),
               thClass: 'text-center',
               tdClass: 'text-center'
            },
            {
               key: 'acting',
               label: this.$t('V.B - V.B.B'),
               thClass: 'text-center',
               tdClass: 'text-center',
               tdAttr(value, key, item) {
                  return {
                     hide: true
                  };
               },
               thStyle: {
                  width: '150px'
               }
            },
            {
               key: 'employees',
               label: this.$t('employee'),
               thClass: 'text-center',
               tdClass: 'text-center p-0',
               tdAttr(value, key, item) {
                  return {
                     colSpan: 6
                  };
               },
               thStyle: {
                  width: '260px'
               },
               thAttr(value, key, item) {
                  return {
                     colSpan: 7
                  };
               }
            },
            {
               key: 'employeeRate',
               label: this.$t('employeeRate'),
               thClass: 'text-center',
               tdClass: 'text-center',
               thStyle: {
                  width: '150px'
               },
               tdAttr(value, key, item) {
                  return {
                     hide: true
                  };
               }
            },

            {
               key: 'docNumber',
               label: this.$t('docnumber'),
               thClass: 'text-center',
               tdClass: 'text-center',
               tdAttr(value, key, item) {
                  return {
                     hide: true
                  };
               },
               thStyle: {
                  width: '150px'
               }
            },

            {
               key: 'createdAt',
               label: this.$t('dateofcreated'),
               thClass: 'text-center',
               tdClass: 'text-center',
               tdAttr(value, key, item) {
                  return {
                     hide: true
                  };
               },
               thStyle: {
                  width: '150px'
               }
            },
            {
               key: 'actions',
               label: this.$t('actions'),
               thClass: 'text-center',
               tdClass: 'text-center',
               tdAttr(value, key, item) {
                  return {
                     hide: true
                  };
               },
               thStyle: {
                  width: '200px'
               }
            }
         ],
         allQuantity: 0,
         allQuantityForNov: 0,
         filter: {
            acting: false,
            interm: false,
            byQuantity: false,
            byQuantityForNow: false,
            startDate: '',
            isProbation: false,
            createdAt: '',
            endDate: '',
            search: '',
            sortBy: '',
            orderType: 'asc',
            page: 1,
            pageSize: 20,
            perPageOptions: [10, 20, 50, 100, 300],
            total: 0
         },
         isBusy: false
      };
   },

   created() {
      if (JSON.parse(localStorage.getItem('filterData5'))) {
         this.filter = JSON.parse(localStorage.getItem('filterData5'));
      } else {
         this.filter = DefaultFilter;
      }

      ManualService.OrganizationAsSelectListByGroup([1, 3]).then((res) => {
         this.OrganisationList = res.data;
      });
      DepartmentService.GetAsSelectList(null, {})
         .then((res) => {
            if (Array.isArray(res.data)) {
               this.DepartmentList = res.data;
            }
         })
         .catch((error) => {
            this.makeToast(error.response.data.errors, 'danger');
         });
      EmployeeService.GetAsSelectList({})
         .then((res) => {
            const { rows } = res.data;
            if (Array.isArray(rows)) {
               this.EmployeeList = rows;
            }
         })
         .catch((error) => {
            this.makeToast(error.response.data.errors, 'danger');
         });
      StaffingService.GetStaffingPositionClassification(null, null, null, this.organizationId).then((res) => {
         this.PositionList = res.data;
      });
   },
   computed: {
      PositionListFilter() {
         return (departmentId) => {
            return departmentId ? this.PositionList.filter((e) => e.departmentId == departmentId) : this.PositionList;
         };
      }
   },
   methods: {
      refresh() {
         this.isBusy = true;
         ReportService.GetStaffingSingleReport(this.filter)
            .then((res) => {
               this.items = [];
               const numArray = [];

               res.data.forEach((item) => {
                  item.employeeManageTables.forEach((item2) => numArray.push(item2.employeeId));
               });
               const unique = Array.from(new Set(numArray));
               this.employeeCount = unique.length;

               this.allQuantity = 0;
               this.allQuantityForNov = 0;
               res.data
                  .sort((a, b) => a.departmentId - b.departmentId)
                  .forEach((item) => {
                     const fList = res.data.filter((t) => t.departmentId == item.departmentId);
                     this.items.push({
                        ...item,
                        rowSpan: this.items.filter((t) => t.departmentId == item.departmentId).every((e) => !e.rowSpan)
                           ? fList.length
                           : 0
                     });
                     this.allQuantity += item.quantity;
                     this.allQuantityForNov += item.quantityForNow;
                  });

               this.filter.total = this.items.total;
            })
            .catch((error) => {
               this.showApiError(error);
            })
            .finally(() => {
               this.isBusy = false;
            });
      },
      countPosition(departmentId) {
         const count = this.items.filter((e) => e.departmentId == departmentId).reduce((a, b) => a + b.quantity, 0);
         return count;
      }
   },
   watch: {
      filter: {
         handler(newValue, oldValue) {
            localStorage.setItem('filterData5', JSON.stringify(newValue));
            this.filter = newValue;
            if (newValue.districtId) {
               this.GetDistrict();
            }
         },
         deep: true
      }
   }
};
</script>

<style>
td[hide='true'] {
   background: red;
   display: none;
}
</style>
