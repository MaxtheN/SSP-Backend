<template>
   <div>
      <form-table-hrm
         :items="items"
         :fields="fields"
         :filter.sync="filter"
         searchable
         :busy="isBusy"
         :actions="{
            create: {
               name: 'EditEmployeeMissedDay',
               permission: 'AppealApplicationCreate'
            },
            edit: {
               name: 'EditEmployeeMissedDay',
               permission: 'AppealApplicationEdit'
            },
            delete: {
               name: 'EditEmployeeMissedDay',
               permission: 'AppealApplicationDelete'
            }
         }"
         @request="Refresh"
      >
         <!-- filtes -->
         <template #filter>
            <b-row>
               <b-col>
                  <b-button variant="primary" :to="{ name: 'EditEmployeeMissedDay', params: { id: 0 } }">
                     <feather-icon icon="PlusIcon"></feather-icon>
                     {{ $t('create') }}
                  </b-button>
               </b-col>
               <b-col cols="12" md="4" sm="12" class="d-flex no-wrap">
                  <b-input-group>
                     <b-form-input v-model="filter.search" @keyup.enter="Refresh" :placeholder="$t('search')" />
                     <b-input-group-append>
                        <b-button @click="Refresh" variant="primary">
                           <feather-icon icon="SearchIcon" />
                        </b-button>
                     </b-input-group-append>
                  </b-input-group>
               </b-col>
               <b-col sm="12" md="12"></b-col>
               <b-col></b-col> </b-row
         ></template>

         <template #cell(employeeFullNames)="{ item }">
            {{ item.employeeFullNames.length ? item.employeeFullNames.toString() : $t('Barcha xodimlar') }}
         </template>
         <template #cell(department)="{ item }">
            {{ item.department ? item.department : '-' }}
         </template>

         <template #cell(actions)="{ item }">
            <div class="text-center" style="text-wrap: nowrap">
               <!-- view -->

               <b-link
                  :to="{
                     name: 'EditEmployeeMissedDay',
                     params: { id: item.id }
                  }"
                  v-b-tooltip.hover.top="$t('Edit')"
                  class="mr-1 cursor-pointer"
               >
                  <feather-icon icon="EyeIcon"></feather-icon>
               </b-link>
               <!-- edit -->
               <b-link
                  v-if="item.canEdit"
                  :to="{ name: 'EditEmployeeMissedDay', params: { id: item.id } }"
                  v-b-tooltip.hover.top="$t('Edit')"
                  class="mr-1 cursor-pointer"
               >
                  <feather-icon icon="EditIcon"></feather-icon>
               </b-link>

               <!-- delete -->

               <b-link
                  v-if="item.canDelete"
                  class="text-danger mr-1 cursor-pointer"
                  @click="Delete(item)"
                  v-b-tooltip.hover.top="$t('Delete')"
               >
                  <feather-icon icon="TrashIcon"></feather-icon>
               </b-link>
               <!-- accept -->
               <b-link
                  v-if="item.canAccept"
                  @click="Accept(item)"
                  class="mr-1 text-success cursor-pointer"
                  v-b-tooltip.hover.top="$t('Approve')"
               >
                  <feather-icon icon="CheckCircleIcon"></feather-icon>
               </b-link>
               <!-- Reject -->
               <template>
                  <b-link
                     v-if="item.canCancel"
                     class="text-danger cursor-pointer mr-1"
                     v-b-tooltip.hover.top="$t('Cancel')"
                     @click="Reject(item)"
                  >
                     <feather-icon icon="XSquareIcon"></feather-icon>
                  </b-link>
               </template>
            </div>
         </template>
      </form-table-hrm>
   </div>
</template>

<script>
import {
   BCard,
   BBadge,
   BLink,
   VBTooltip,
   BFormInput,
   BInputGroup,
   BInputGroupAppend,
   BRow,
   BCol,
   BButtonGroup,
   BButton
} from 'bootstrap-vue';
import VueEditor from '@/components/VueEditor.vue';
import FormTableHrm from '@/components/forms/form-table-hrm.vue';

import EmployeeMissedDayService from '@/services/hrm/employeemissedday.service';

import StatusSelect from '@/views/components/document/StatusSelect.vue';

export default {
   components: {
      BCard,
      VueEditor,
      BBadge,
      BLink,
      BFormInput,
      BInputGroup,
      BInputGroupAppend,
      BRow,
      BCol,
      BButton,
      BButtonGroup,
      FormTableHrm,
      StatusSelect
   },
   directives: {
      'b-tooltip': VBTooltip
   },
   data() {
      return {
         items: [],

         fields: [
            {
               key: 'id',
               label: this.$t('id'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },

            {
               key: 'employeeFullNames',
               label: this.$t('employee'),
               thClass: 'text-center',
               tdClass: 'text-left',
               sortable: true
            },
            {
               key: 'department',
               label: this.$t('department'),
               thClass: 'text-center',
               tdClass: 'text-left',
               sortable: true
            },
            {
               key: 'docDate',
               label: this.$t('docOn'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },

            {
               key: 'status',
               label: this.$t('status'),
               thClass: 'text-center',
               sortable: true,
               thClass: 'text-center',
               tdClass: 'text-center'
            },
            {
               key: 'actions',
               label: this.$t('actions'),
               thClass: 'text-center',
               tdClass: 'text-center',
               stickyColumn: true
            }
         ],
         filter: {
            statusId: null,
            regionId: null,
            personFullName: '',
            fromDocOn: '',
            toDocOn: '',
            contractorInn: '',
            districtId: null,
            appealFormatTypeId: null,
            appealTypeId: null,
            appealTypeArriveId: null,
            appealDescriptionId: null,
            search: '',
            sortBy: '',
            orderType: 'asc',
            page: 1,
            pageSize: 20,
            perPageOptions: [10, 20, 50, 100],
            total: 0
         },
         isBusy: false
      };
   },
   created() {},
   methods: {
      Delete(item) {
         this.$swal.fire({
            icon: 'info',
            title: this.$t('WantDelete'),
            showLoaderOnConfirm: true,
            preConfirm: () => {
               return EmployeeMissedDayService.Delete(item.id)
                  .then(() => {
                     this.makeToast(this.$t('DeleteSuccess'), 'success');
                     this.Refresh();
                  })
                  .catch(this.SwalError);
            },
            allowOutsideClick: () => !this.$swal.isLoading()
         });
      },
      Accept(item) {
         this.$swal.fire({
            icon: 'info',
            title: this.$t('WantAccept'),
            showLoaderOnConfirm: true,
            preConfirm: () => {
               return EmployeeMissedDayService.Approve({ id: item.id })
                  .then(() => {
                     this.makeToast(this.$t('DeleteSuccess'), 'success');
                     this.Refresh();
                  })
                  .catch(this.SwalError);
            },
            allowOutsideClick: () => !this.$swal.isLoading()
         });
      },
      Reject(item) {
         this.$swal.fire({
            icon: 'info',
            title: this.$t('WantCancel'),
            showLoaderOnConfirm: true,
            preConfirm: () => {
               return EmployeeMissedDayService.CancelApprove({ id: item.id })
                  .then(() => {
                     this.makeToast(this.$t('DeleteSuccess'), 'success');
                     this.Refresh();
                  })
                  .catch(this.SwalError);
            },
            allowOutsideClick: () => !this.$swal.isLoading()
         });
      },

      Refresh() {
         this.isBusy = true;
         EmployeeMissedDayService.GetList(this.filter)
            .then((res) => {
               this.items = res.data.rows;
               this.isBusy = false;
            })
            .catch(this.SwalError)
            .finally(() => {
               this.isBusy = false;
            });
      }
   }
};
</script>
<style>
.r-0 {
   right: 0;
}
</style>
