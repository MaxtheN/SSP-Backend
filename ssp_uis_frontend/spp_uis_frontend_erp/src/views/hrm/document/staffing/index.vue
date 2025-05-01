<template>
   <form-table-hrm
      :items="items"
      :fields="fields"
      :filter.sync="filter"
      searchable
      :busy="isBusy"
      :actions="{
         edit: {
            name: 'EditStaffing',
            permission: 'StaffingEdit'
         }
      }"
      @row-dblclicked="DbClick"
      @request="Refresh"
      @row-delete="Delete"
   >
      <template #filter>
         <b-row>
            <b-col cols="12" md="6" class="d-flex align-items-center justify-content-start mb-1 mb-md-0">
               <template>
                  <b-button
                     variant="primary"
                     :to="{
                        name: 'EditStaffing',
                        params: { id: 0 }
                     }"
                  >
                     <feather-icon icon="PlusIcon"></feather-icon>
                     {{ $t('create') }}
                  </b-button>
               </template>
            </b-col>
            <b-col md="2">
               <form-picker
                  type="year"
                  format="YYYY"
                  v-model="filter.financeYear"
                  @change="Refresh"
                  :label="$t('docyear')"
               />
            </b-col>
            <b-col>
               <b-input-group class="mt-2">
                  <b-form-input v-model="filter.search" @keyup.enter="Refresh" :placeholder="$t('search')" />
                  <b-input-group-append>
                     <b-button @click="Refresh" variant="primary">
                        <feather-icon icon="SearchIcon" />
                     </b-button>
                  </b-input-group-append>
               </b-input-group>
            </b-col>
         </b-row>
         <b-row>
            <b-col class="mt-1">
               <b-button-group @click="Refresh" size="sm">
                  <b-button
                     @click="filter.statusIds.length = 0"
                     :variant="filter.statusIds.length == 0 ? 'primary' : 'outline-primary'"
                     >{{ $t('all') }}</b-button
                  >
                  <b-button
                     @click="filter.statusIds.push(7)"
                     :variant="filter.statusIds != 0 ? 'primary' : 'outline-primary'"
                     >{{ $t('archived') }}</b-button
                  >
               </b-button-group>
            </b-col>
         </b-row>
      </template>

      <template #cell(actions)="{ item }">
         <div class="text-right" style="text-wrap: nowrap">
            <!-- edit -->
            <b-link
               v-if="$can('StaffingView', 'permissions') && item.canEdit"
               :to="{ name: 'EditStaffing', params: { id: item.id } }"
               v-b-tooltip.hover.top="$t('Edit')"
               class="mr-1 cursor-pointer"
            >
               <feather-icon icon="EditIcon"></feather-icon>
            </b-link>
            <!-- view -->
            <b-link
               v-if="$can('StaffingView', 'permissions')"
               :to="{ name: 'EditStaffing', params: { id: item.id, isView: true } }"
               v-b-tooltip.hover.top="$t('View')"
               class="mr-1 cursor-pointer"
            >
               <feather-icon icon="EyeIcon"></feather-icon>
            </b-link>
            <!-- delete -->
            <b-link
               v-if="$can('StaffingDelete', 'permissions') && item.canDelete"
               class="text-danger mr-1 cursor-pointer"
               @click="Delete(item)"
               v-b-tooltip.hover.top="$t('Delete')"
            >
               <feather-icon icon="TrashIcon"></feather-icon>
            </b-link>
            <!-- accept -->
            <b-link
               v-if="$can('StaffingAccept', 'permissions') && item.canAccept"
               @click="Accept(item)"
               class="mr-1 text-success cursor-pointer"
               v-b-tooltip.hover.top="$t('Approve')"
            >
               <feather-icon icon="CheckCircleIcon"></feather-icon>
            </b-link>
            <!-- Send -->
            <b-link
               v-if="$can('StaffingSend', 'permissions') && item.canSend"
               @click="Send(item)"
               class="mr-1 text-success cursor-pointer"
               v-b-tooltip.hover.top="$t('Send')"
            >
               <feather-icon icon="SendIcon"></feather-icon>
            </b-link>
            <!-- cancel -->
            <template v-if="$can('StaffingCancel', 'permissions') && item.canCancel">
               <b-link
                  class="text-danger cursor-pointer mr-1"
                  v-b-tooltip.hover.top="$t('Cancel')"
                  @click="Cancel(item)"
               >
                  <feather-icon icon="XCircleIcon"></feather-icon>
               </b-link>
            </template>
            <!-- Revoke -->
            <b-link
               v-if="$can('StaffingRevoke', 'permissions') && item.canRevoke"
               @click="Revoke(item)"
               class="mr-1 text-warning cursor-pointer"
               v-b-tooltip.hover.top="$t('Revoke')"
            >
               <feather-icon icon="XCircleIcon"></feather-icon>
            </b-link>
            <!-- document history -->
            <history-modal-button :id="item.id" :table-id="item.tableId || 93" />
            <!-- clone -->
            <b-link
               v-if="$can('StaffingClone', 'permissions')"
               :to="{ name: 'EditStaffing', params: { id: item.id, mode: 'clone' } }"
               v-b-tooltip.hover.top="$t('clone')"
               class="mx-1 cursor-pointer"
            >
               <feather-icon icon="CopyIcon"></feather-icon>
            </b-link>
            <!-- to arxiv -->
            <b-link
               v-if="item.statusId != 7"
               @click="SendArchive(item.id)"
               v-b-tooltip.hover.top="$t('archive')"
               class="mx-1 cursor-pointer"
            >
               <feather-icon icon="ArchiveIcon"></feather-icon>
            </b-link>
            <b-link
               v-if="item.statusId == 7"
               @click="RecallFromArchive(item.id)"
               v-b-tooltip.hover.top="$t('unarchive')"
               class="mx-1 cursor-pointer"
            >
               <span style="position: relative">
                  <feather-icon icon="ArchiveIcon"></feather-icon>
                  <feather-icon style="position: absolute; top: -5; right: 0.5" icon="ArrowUpIcon"></feather-icon>
               </span>
            </b-link>
         </div>
      </template>
   </form-table-hrm>
</template>

<script>
import {
   BRow,
   BCol,
   BCard,
   BCardText,
   VBTooltip,
   BBadge,
   BButton,
   BLink,
   BModal,
   VBModal,
   BButtonGroup,
   BInputGroupAppend,
   BFormInput,
   BInputGroup
} from 'bootstrap-vue';
import FormTableHrm from '@/components/forms/form-table-hrm.vue';
import StaffingService from '@/services/hrm/staffing.service';
import HistoryModalButton from '@/views/components/document/HistoryModalButton.vue';
const DefaultFilter = {
   financeYear: '',
   search: '',
   sortBy: '',
   orderType: 'asc',
   page: 1,
   pageSize: 20,
   perPageOptions: [10, 20, 50, 100],
   total: 0,
   statusIds: []
};
export default {
   components: {
      BRow,
      BCol,
      BCard,
      FormTableHrm,
      BCardText,
      BButton,
      BBadge,
      BLink,
      BModal,
      HistoryModalButton,
      BButtonGroup,
      BInputGroupAppend,
      BFormInput,
      BInputGroup
   },
   directives: {
      'b-tooltip': VBTooltip,
      'b-modal': VBModal
   },
   data() {
      return {
         loacalData: JSON.parse(localStorage.getItem('user_info')),
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
               key: 'docOn',
               label: this.$t('docOn'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'staffingType',
               label: this.$t('staffingType'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'financeYear',
               label: this.$t('financeYear'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'docSum',
               label: this.$t('summary')
            },
            {
               key: 'organization',
               label: this.$t('organization')
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
               thClass: 'text-right',
               tdClass: 'text-right'
            }
         ],
         filter: {
            financeYear: '',
            search: '',
            sortBy: '',
            orderType: 'asc',
            page: 1,
            pageSize: 20,
            perPageOptions: [10, 20, 50, 100],
            total: 0,
            statusIds: []
         },
         isBusy: false
      };
   },
   created() {
      if (JSON.parse(localStorage.getItem('filterData3'))) {
         this.filter = JSON.parse(localStorage.getItem('filterData3'));
      } else {
         this.filter = DefaultFilter;
      }
   },
   methods: {
      DbClick(item) {
         if (this.$can('StaffingView', 'permissions')) {
            this.$router.push({
               name: 'EditStaffing',
               params: { id: item.id, isView: true }
            });
         }
      },
      Delete(item) {
         this.$swal.fire({
            icon: 'info',
            title: this.$t('WantDelete'),
            showLoaderOnConfirm: true,
            preConfirm: () => {
               return StaffingService.Delete(item.id)
                  .then(() => {
                     this.makeToast(this.$t('DeleteSuccess'), 'success');
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
            title: this.$t('WantSend'),
            showLoaderOnConfirm: true,
            preConfirm: () => {
               return StaffingService.Send(item.id)
                  .then(() => {
                     this.makeToast(this.$t('SendSuccess'), 'success');
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
               return StaffingService.Accept({
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
      Cancel(item) {
         this.$swal.fire({
            icon: 'question',
            title: this.$t('WantCancel'),
            showLoaderOnConfirm: true,
            preConfirm: () => {
               return StaffingService.Cancel({
                  statusIds: item.statusId,
                  id: item.id
               })
                  .then(() => {
                     this.makeToast(this.$t('CancelMessage'), 'success');
                     this.Refresh();
                  })
                  .catch(this.SwalError);
            },
            allowOutsideClick: () => !this.$swal.isLoading()
         });
      },
      Revoke(item) {
         this.$swal.fire({
            icon: 'question',
            title: this.$t('WantRevoke'),
            showLoaderOnConfirm: true,
            preConfirm: () => {
               return StaffingService.Revoke({
                  id: item.id,
                  message: ''
               })
                  .then(() => {
                     this.makeToast(this.$t('RevokeSuccess'), 'success');
                     this.Refresh();
                  })
                  .catch(this.SwalError);
            },
            allowOutsideClick: () => !this.$swal.isLoading()
         });
      },
      // send to archive
      SendArchive(id) {
         StaffingService.SendToArchive(id)
            .then(() => {
               this.makeToast(this.$t('ArchiveSuccess'), 'success');
               this.Refresh();
            })
            .catch(this.SwalError);
      },

      // back to archive
      RecallFromArchive(id) {
         StaffingService.RecallFromArchive(id)
            .then(() => {
               this.makeToast(this.$t('RevokeSuccess'), 'success');
               this.Refresh();
            })
            .catch(this.SwalError);
      },

      Refresh() {
         this.isBusy = true;
         StaffingService.GetList(this.filter)
            .then((res) => {
               this.items = res.data.rows;
               this.filter.total = res.data.total;
            })
            .finally(() => {
               this.isBusy = false;
            });
      }
   },
   watch: {
      filter: {
         handler(newValue, oldValue) {
            localStorage.setItem('filterData3', JSON.stringify(newValue));
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
