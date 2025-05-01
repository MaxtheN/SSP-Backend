<template>
   <form-table-hrm
      :items="items"
      :actions="{
         create: {
            name: 'EditWorkSchedule',
            permission: 'WorkScheduleCreate'
         },
         edit: {
            name: 'EditWorkSchedule',
            permission: 'WorkScheduleEdit'
         }
      }"
      :fields="fields"
      :filter.sync="filter"
      searchable
      :busy="isBusy"
      @row-dblclicked="DbClick"
      @request="Refresh"
   >
      <template #cell(employees)="{ item }">
         {{ item.employees.join(', ') }}
      </template>
      <template #cell(actions)="{ item }">
         <div class="text-center" style="text-wrap: nowrap">
            <!-- edit -->
            <b-link
               v-if="$can('WorkScheduleEdit', 'permissions')"
               :to="{ name: 'EditWorkSchedule', params: { id: item.id } }"
               v-b-tooltip.hover.top="$t('Edit')"
               class="mr-1 cursor-pointer"
            >
               <feather-icon icon="EditIcon"></feather-icon>
            </b-link>
            <!-- delete -->
            <b-link
               v-if="$can('WorkScheduleDelete', 'permissions')"
               class="text-danger mr-1 cursor-pointer"
               @click="Delete(item)"
               v-b-tooltip.hover.top="$t('Delete')"
            >
               <feather-icon icon="TrashIcon"></feather-icon>
            </b-link>

            <!-- document history -->
            <history-modal-button :id="item.id" :table-id="item.tableId || 85" />
         </div>
      </template>
      <template #cell(status)="{ item }">
         <b-badge :variant="getColor(item)">{{ item.status }}</b-badge>
      </template>
   </form-table-hrm>
</template>

<script>
import { BCard, BCardText, VBTooltip, BBadge, BButton, BLink, BModal, VBModal } from 'bootstrap-vue';
import FormTableHrm from '@/components/forms/form-table-hrm.vue';
import WorkScheduleService from '@/services/info/workschedule.service';
import HistoryModalButton from '@/views/components/document/HistoryModalButton.vue';

export default {
   components: {
      BCard,
      FormTableHrm,
      BCardText,
      BButton,
      BBadge,
      BLink,
      BModal,
      HistoryModalButton
   },
   directives: {
      'b-tooltip': VBTooltip,
      'b-modal': VBModal
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
               key: 'shortName',
               label: this.$t('shortname'),
               thClass: 'text-center',
               sortable: true
            },
            {
               key: 'fullName',
               label: this.$t('fullname'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'workScheduleKind',
               label: this.$t('workScheduleKind'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },

            {
               key: 'status',
               label: this.$t('status'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'actions',
               label: this.$t('actions'),
               thClass: 'text-center',
               tdClass: 'text-center'
            }
         ],
         filter: {
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
   methods: {
      DbClick(item) {
         this.$router.push({
            name: 'EditWorkSchedule',
            params: { id: item.id, isView: true }
         });
      },
      Delete(item) {
         this.$swal.fire({
            icon: 'info',
            title: this.$t('WantDelete'),
            showLoaderOnConfirm: true,
            preConfirm: () => {
               return WorkScheduleService.Delete(item.id)
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
         WorkScheduleService.GetList(this.filter)
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
