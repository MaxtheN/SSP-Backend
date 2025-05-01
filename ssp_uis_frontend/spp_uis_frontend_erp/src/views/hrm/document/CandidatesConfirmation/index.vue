<template>
   <form-table-hrm
      :items="items"
      :fields="fields"
      :filter.sync="filter"
      searchable
      :busy="isBusy"
      :actions="{
         create: {
            name: 'EditCandidatesConfirmation',
            permission: 'CandidatesConfirmationCreate'
         },
         edit: {
            name: 'EditCandidatesConfirmation',
            permission: 'CandidatesConfirmationEdit'
         },
         delete: {
            name: 'EditCandidatesConfirmation',
            permission: 'CandidatesConfirmationDelete'
         }
      }"
      @row-dblclicked="DbClick"
      @request="Refresh"
   >
      <template #cell(actions)="{ item }">
         <div class="text-center" style="text-wrap: nowrap">
            <!-- edit -->
            <b-link
               v-if="$can('CandidatesConfirmationEdit', 'permissions') && item.canEdit"
               :to="{ name: 'EditCandidatesConfirmation', params: { id: item.id } }"
               v-b-tooltip.hover.top="$t('Edit')"
               class="mr-1 cursor-pointer"
            >
               <feather-icon icon="EditIcon"></feather-icon>
            </b-link>
            <!-- delete -->
            <b-link
               v-if="$can('CandidatesConfirmationDelete', 'permissions') && item.canDelete"
               class="text-danger mr-1 cursor-pointer"
               @click="Delete(item)"
               v-b-tooltip.hover.top="$t('Delete')"
            >
               <feather-icon icon="TrashIcon"></feather-icon>
            </b-link>
            <!-- Send -->

            <b-link
               v-if="$can('CandidatesConfirmationSend', 'permissions') && item.canSend"
               @click="Send(item)"
               class="mr-1 text-success cursor-pointer"
               v-b-tooltip.hover.top="$t('Send')"
            >
               <feather-icon icon="SendIcon"></feather-icon>
            </b-link>
            <!-- accept -->
            <b-link
               v-if="$can('CandidatesConfirmationAccept', 'permissions') && item.canAccept"
               icon="CheckCircleIcon"
               @click="Accept(item)"
               v-b-tooltip.hover.top="$t('accept')"
            >
               <feather-icon></feather-icon>
            </b-link>

            <!-- cancel -->
            <b-link
               class="text-danger cursor-pointer ml-1"
               v-if="$can('CandidatesConfirmationCancel', 'permissions') && item.canAccept"
               v-b-tooltip.hover.top="$t('Cancel')"
               @click="Cancel(item)"
            >
               <feather-icon icon="XCircleIcon"></feather-icon>
            </b-link>
         </div>
      </template>
   </form-table-hrm>
</template>

<script>
import { BCard, BLink, VBTooltip, BRow } from 'bootstrap-vue';
import FormTableHrm from '@/components/forms/form-table-hrm.vue';
import CandidatesConfirmationService from '@/services/hrm/candidatesconfirmation.service';

export default {
   components: {
      BLink,
      BRow,
      BCard,
      FormTableHrm
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
               key: 'docNumber',
               label: this.$t('docnumber'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'docContent',
               label: this.$t('docContent'),
               thClass: 'text-center',
               tdClass: 'text-center'
            },
            {
               key: 'docOn',
               label: this.$t('docOn'),
               thClass: 'text-center',
               tdClass: 'text-center'
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
            statusId: null,
            departmentId: null,
            positionId: null,
            search: '',
            sortBy: '',
            orderType: '',
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
            name: 'EditCandidatesConfirmation',
            params: { id: item.id }
         });
      },
      Delete(item) {
         this.$swal.fire({
            icon: 'info',
            title: this.$t('WantDelete'),
            showLoaderOnConfirm: true,
            preConfirm: () => {
               return CandidatesConfirmationService.Delete(item.id)
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
               return CandidatesConfirmationService.Accept({ statusId: item.statusId, id: item.id })
                  .then(() => {
                     this.makeToast(this.$t('AcceptSuccess'), 'success');
                     this.Refresh();
                  })
                  .catch(this.SwalError);
            },
            allowOutsideClick: () => !this.$swal.isLoading()
         });
      },
      Cancel(item) {
         this.$swal.fire({
            icon: 'info',
            title: this.$t('WantCancel'),
            showLoaderOnConfirm: true,
            preConfirm: () => {
               return CandidatesConfirmationService.Cancel({ statusId: item.statusId, id: item.id })
                  .then(() => {
                     this.makeToast(this.$t('CancelMessage'), 'success');
                     this.Refresh();
                  })
                  .catch(this.SwalError);
            },
            allowOutsideClick: () => !this.$swal.isLoading()
         });
      },
      Send(item) {
         this.$swal.fire({
            icon: 'info',
            title: this.$t('WantCancel'),
            showLoaderOnConfirm: true,
            preConfirm: () => {
               return CandidatesConfirmationService.Send({ statusId: item.statusId, id: item.id })
                  .then(() => {
                     this.makeToast(this.$t('CancelMessage'), 'success');
                     this.Refresh();
                  })
                  .catch(this.SwalError);
            },
            allowOutsideClick: () => !this.$swal.isLoading()
         });
      },

      Refresh() {
         this.isBusy = true;
         CandidatesConfirmationService.GetList(this.filter)
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
