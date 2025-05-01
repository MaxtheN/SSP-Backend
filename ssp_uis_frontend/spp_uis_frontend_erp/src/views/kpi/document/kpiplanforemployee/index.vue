<template>
   <form-table-hrm
      :items="items"
      :fields="fields"
      :filter.sync="filter"
      searchable
      :busy="isBusy"
      :actions="{
         create: {
            name: 'EditKpiPlanForEmployee',
            permission: 'KpiPlanForEmployeeCreate'
         },
         edit: {
            name: 'EditKpiPlanForEmployee',
            permission: 'KpiPlanForEmployeeEdit'
         },
         delete: {
            name: 'EditKpiPlanForEmployee',
            permission: 'KpiPlanForEmployeeDelete'
         }
      }"
      @request="Refresh"
   >
      <!-- filtes -->
      <template #filter>
         <b-row>
            <b-col cols="12" md="6" :class="{ 'mb-1': isMobileDevice() }">
               <template>
                  <b-button variant="primary" :to="{ name: 'EditKpiPlanForEmployee', params: { id: 0 } }">
                     <feather-icon icon="PlusIcon"></feather-icon>
                     {{ $t('create') }}
                  </b-button>
               </template>
            </b-col>
            <b-col cols="12" md="6" class="d-flex no-wrap">
               <b-input-group>
                  <b-form-input v-model="filter.search" @keyup.enter="Refresh" :placeholder="$t('search')" />
                  <b-input-group-append>
                     <b-button @click="Refresh" variant="primary">
                        <feather-icon icon="SearchIcon" />
                     </b-button>
                  </b-input-group-append>
               </b-input-group>
            </b-col>
         </b-row>
      </template>

      <template #cell(actions)="{ item }">
         <div class="text-center" style="text-wrap: nowrap">
            <!-- view -->
            <b-link
               v-if="$can('KpiPlanForEmployeeViewAll', 'permissions')"
               :to="{
                  name: 'EditKpiPlanForEmployee',
                  params: { id: item.id },
                  query: { isView: true }
               }"
               v-b-tooltip.hover.top="$t('Edit')"
               class="mr-1 cursor-pointer"
            >
               <feather-icon icon="EyeIcon"></feather-icon>
            </b-link>
            <!-- edit -->
            <b-link
               v-if="$can('KpiPlanForEmployeeEdit', 'permissions') && item.canEdit"
               :to="{ name: 'EditKpiPlanForEmployee', params: { id: item.id } }"
               v-b-tooltip.hover.top="$t('Edit')"
               class="mr-1 cursor-pointer"
            >
               <feather-icon icon="EditIcon"></feather-icon>
            </b-link>

            <!-- delete -->
            <b-link
               v-if="$can('KpiPlanForEmployeeDelete', 'permissions') && item.canDelete"
               class="text-danger mr-1 cursor-pointer"
               @click="Delete(item)"
               v-b-tooltip.hover.top="$t('Delete')"
            >
               <feather-icon icon="TrashIcon"></feather-icon>
            </b-link>
            <!-- accept -->
            <b-link
               v-if="$can('KpiPlanForEmployeeAccept', 'permissions') && item.canAccept"
               @click="Accept(item)"
               class="mr-1 text-success cursor-pointer"
               v-b-tooltip.hover.top="$t('Approve')"
            >
               <feather-icon icon="CheckCircleIcon"></feather-icon>
            </b-link>
            <!-- Reject -->
            <template v-if="$can('KpiPlanForEmployeeCancel', 'permissions') && item.canCancel">
               <b-link
                  class="text-danger cursor-pointer mr-1"
                  v-b-tooltip.hover.top="$t('Reject')"
                  @click="Reject(item)"
               >
                  <feather-icon icon="XSquareIcon"></feather-icon>
               </b-link>
            </template>
         </div>
      </template>

      <template #cell(personName)="{ item }"> {{ item.personName ? item.personName : item.personFullName }} </template>
      <template #cell(status)="{ item }">
         <b-badge :variant="getColor(item)">{{ item.status }}</b-badge>
      </template>
   </form-table-hrm>
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
import FormTableHrm from '@/components/forms/form-table-hrm.vue';
import KpiPlanForEmployeeService from '@/services/kpi/kpiplanforemployee.service';
import ManualService from '@/services/others/manual.service';
import StatusSelect from '@/views/components/document/StatusSelect.vue';

export default {
   components: {
      BCard,
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
         printLoding: false,
         items: [],
         OrganisationList: [],

         fields: [
            {
               key: 'actions',
               label: this.$t('actions'),
               thClass: 'text-center',
               tdClass: 'text-center',
               stickyColumn: !this.isMobileDevice()
            },
            {
               key: 'id',
               label: this.$t('id'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },

            {
               key: 'docOn',
               label: this.$t('docOn'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },

            {
               key: 'docNumber',
               label: this.$t('docnumber'),
               thClass: 'text-center',
               tdClass: 'text-left',
               sortable: true
            },
            {
               key: 'organization',
               label: this.$t('organization'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },

            {
               key: 'status',
               label: this.$t('status'),
               thClass: 'text-center',
               sortable: true,
               stickyColumn: !this.isMobileDevice(),
               thStyle: {
                  right: this.isMobileDevice() ? '' : '0'
               },
               tdClass: 'r-0'
            }
         ],
         filter: {
            statusId: 0,
            organizationId: 0,
            search: '',
            sortBy: '',
            page: 1,
            pageSize: 20,
            perPageOptions: [10, 20, 50, 100],
            total: 0
         },
         isBusy: false
      };
   },
   created() {
      ManualService.OrganizationAsSelectListByGroup([1, 3]).then((res) => {
         this.OrganisationList = res.data;
      });
   },
   methods: {
      Delete(item) {
         this.$swal.fire({
            icon: 'info',
            title: this.$t('WantDelete'),
            showLoaderOnConfirm: true,
            preConfirm: () => {
               return KpiPlanForEmployeeService.Delete(item.id)
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
               return KpiPlanForEmployeeService.Accept({
                  id: item.id,
                  message: ''
               })
                  .then(() => {
                     this.makeToast(this.$t('AcceptSuccess'), 'success');
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
            title: this.$t('WantReject'),
            showLoaderOnConfirm: true,
            preConfirm: () => {
               return KpiPlanForEmployeeService.Cancel({
                  id: item.id,
                  message: ''
               })
                  .then(() => {
                     this.makeToast(this.$t('RejectSuccess'), 'success');
                     this.Refresh();
                  })
                  .catch(this.SwalError);
            },
            allowOutsideClick: () => !this.$swal.isLoading()
         });
      },
      Refresh() {
         this.isBusy = true;
         KpiPlanForEmployeeService.GetList(this.filter)
            .then((res) => {
               this.items = res.data.rows;
               this.filter.total = res.data.total;
            })
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
