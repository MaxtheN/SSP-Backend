<template>
   <form-table-hrm
      :items="items"
      :actions="{}"
      :fields="fields"
      :filter.sync="filter"
      searchable
      :busy="isBusy"
      @request="Refresh"
      :hover="false"
      bordered
      :no-border-collapse="false"
   >
      <template #filter>
         <b-row>
            <b-col cols="12" md="12">
               <form-select
                  :disabled="localStorageData.organizationId != 1"
                  :options="OrganizationList"
                  v-model="filter.organizationId"
                  :label="$t('organization')"
                  @change="Refresh"
               ></form-select>
            </b-col>
            <b-col cols="12" md="2">
               <div>
                  <label for>{{ $t('Hujjat sanasi (dan)') }}</label>
                  <form-picker v-model="filter.startDate" :placeholder="$t('Hujjat sanasi (dan)')" @change="Refresh" />
               </div>
               <div>
                  <label for>{{ $t('Hujjat sanasi (gacha)') }}</label>
                  <form-picker v-model="filter.endDate" :placeholder="$t('Hujjat sanasi (gacha)')" @change="Refresh" />
               </div>
            </b-col>

            <b-col cols="12" md="2">
               <div>
                  <label for>{{ $t('Yaratilingan sana(dan)') }}</label>
                  <form-picker
                     v-model="filter.startCreatedAt"
                     :placeholder="$t('Yaratilingan sana(dan)')"
                     @change="Refresh"
                  />
               </div>
               <div>
                  <label for>{{ $t('Yaratilingan sana(gacha)') }}</label>
                  <form-picker
                     v-model="filter.endCreatedAt"
                     :placeholder="$t('Yaratilingan sana(gacha)')"
                     @change="Refresh"
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
                           (filter.interm = false), Refresh();
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
                           (filter.acting = false), Refresh();
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
                           Refresh();
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
                           (filter.byQuantityForNow = false), Refresh();
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
                           (filter.byQuantity = false), Refresh();
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
                     <b-button @click="Refresh" variant="primary">
                        <feather-icon icon="SearchIcon" />
                     </b-button>
                  </b-input-group-append>
               </b-input-group>
            </b-col>
         </b-row>
      </template>
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
            <tr v-for="(emp, i) in item.employeeManageTables" :key="i + emp.pinfl + 'nimadr'" style="border: none">
               <td style="min-width: 150px">
                  <template v-if="emp.appointEmployees && emp.appointEmployees.length > 0">
                     <p v-if="emp.appointEmployees[0].acting">{{ $t('V.B') }}</p>
                     <p v-if="emp.appointEmployees[0].interm">{{ $t('V.B.B') }}</p>
                  </template>
               </td>

               <td style="min-width: 150px">
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
            </tr>

            <tr v-if="item.quantityForNow > 0">
               <td colspan="100%" class="border-0">
                  <!-- edit -->
                  {{ $t('Vakant') }}
               </td>
            </tr>
         </table>
      </template>
      <template #cell(status)="{ item }">
         <b-badge :variant="getColor(item)">{{ item.status }}</b-badge>
      </template>
   </form-table-hrm>
</template>

<script>
import {
   BFormCheckbox,
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
   BRow,
   BFormInput,
   BInputGroupAppend,
   BInputGroup
} from 'bootstrap-vue';
import FormTableHrm from '@/components/forms/form-table-hrm.vue';
import ReportService from '@/services/report/report.service';
import OrganizationService from '@/services/managment/organization.service';

export default {
   components: {
      BCard,
      FormTableHrm,
      BCardText,
      BButton,
      BBadge,
      BLink,
      BModal,
      BFormCheckbox,
      BDropdown,
      BDropdownItem,
      BCol,
      BRow,
      BFormInput,
      BInputGroupAppend,
      BInputGroup
   },
   directives: {
      'b-tooltip': VBTooltip,
      'b-modal': VBModal
   },
   data() {
      return {
         items: [],
         localStorageData: {},
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
                     colSpan: 5
                  };
               },
               thStyle: {
                  width: '250px'
               },
               thAttr(value, key, item) {
                  return {
                     colSpan: 6
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
            }
         ],
         allQuantity: 0,
         allQuantityForNov: 0,
         OrganizationList: [],
         filter: {
            startCreatedAt: '',
            endCreatedAt: '',
            isProbation: false,
            search: '',
            sortBy: '',
            orderType: 'asc',
            page: 1,
            pageSize: 20,
            perPageOptions: [10, 20, 50, 100, 300],
            total: 0,
            organizationId:
               JSON.parse(localStorage.getItem('user_info')).organizationId == 1
                  ? null
                  : JSON.parse(localStorage.getItem('user_info')).organizationId,
            departmentId: null
         },
         isBusy: false
      };
   },
   created() {
      if (JSON.parse(localStorage.getItem('filterData6'))) {
         this.filter = JSON.parse(localStorage.getItem('filterData6'));
      }
      this.localStorageData = JSON.parse(localStorage.getItem('user_info'));

      OrganizationService.GetAsSelectList(null, null, null, 3)
         .then((res) => {
            this.OrganizationList = res.data;
         })
         .catch((error) => {
            this.showApiError(error);
         });
   },
   methods: {
      Refresh() {
         this.isBusy = true;
         ReportService.GetStaffingSingleReportForParent(this.filter)
            .then((res) => {
               this.items = [];
               const numArray = [];
               this.allQuantity = 0;
               this.allQuantityForNov = 0;
               res.data.rows
                  .sort((a, b) => a.departmentId - b.departmentId)
                  .forEach((item) => {
                     const fList = res.data.rows.filter((t) => t.departmentId == item.departmentId);
                     this.items.push({
                        ...item,
                        rowSpan: this.items.filter((t) => t.departmentId == item.departmentId).every((e) => !e.rowSpan)
                           ? fList.length
                           : 0
                     });
                     this.allQuantity += item.quantity;
                     this.allQuantityForNov += item.quantityForNow;
                  });

               res.data.rows.forEach((item) => {
                  item.employeeManageTables.forEach((item2) => numArray.push(item2.employeeId));
               });
               const unique = Array.from(new Set(numArray));
               this.employeeCount = unique.length;
               this.filter.total = res.data.total;
               // console.log(res.data.total);
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
            localStorage.setItem('filterData6', JSON.stringify(newValue));
            this.filter = newValue;
            if (newValue.districtId) {
               this.GetDistrict();
            }
            if (this.localStorageData.organizationId != 1) {
               this.filter.organizationId = this.localStorageData.organizationId;
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
