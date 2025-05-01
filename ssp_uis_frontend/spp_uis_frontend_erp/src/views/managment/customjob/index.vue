<template>
   <form-table-hrm
      :items="items"
      :fields="fields"
      :filter.sync="filter"
      searchable
      :busy="isBusy"
      :delete-loading="deleteLoading"
      :actions="{
         create: {
            name: 'EditCustomJob',
            permission: 'CustomJobCreate'
         },
         edit: {
            name: 'EditCustomJob',
            permission: 'CustomJobEdit'
         },
         delete: {
            name: 'EditCustomJob',
            permission: 'CustomJobDelete'
         }
      }"
      @row-dblclicked="DbClick"
      @request="Refresh"
      @row-delete="Delete"
   >
      <template #cell(actions)="{ item }">
         <div class="text-center" style="text-wrap: nowrap">
            <!-- edit -->
            <b-link
               v-if="$can('CustomJobEdit', 'permissions') && ![12, 6, 17].includes(item.statusId)"
               :to="{ name: 'EditCustomJob', params: { id: item.id } }"
               v-b-tooltip.hover.top="$t('Edit')"
               class="mr-1 cursor-pointer"
            >
               <feather-icon icon="EditIcon"></feather-icon>
            </b-link>
            <!-- delete -->
            <b-link
               v-if="$can('CustomJobDelete', 'permissions') && item.statusId != 17"
               class="text-danger mr-1 cursor-pointer"
               v-b-tooltip.hover.top="$t('Delete')"
               @click="Delete(item)"
            >
               <feather-icon icon="TrashIcon"></feather-icon>
            </b-link>
            <!-- Approve -->
            <b-link
               v-if="$can('CustomJobApprove', 'permissions') && ![12, 6, 17].includes(item.statusId)"
               class="mr-1 text-success cursor-pointer"
               v-b-tooltip.hover.top="$t('Approve')"
               @click="Approve(item)"
            >
               <feather-icon icon="CheckCircleIcon"></feather-icon>
            </b-link>
            <!-- CancelApprove -->
            <!-- <b-link
               v-if="$can('CustomJobCancelApprove', 'permissions')&& item.statusId==17"
               class="text-danger cursor-pointer mr-1"
               v-b-tooltip.hover.top="$t('Cancel')"
               @click="CancelApprove(item)"
            >
               <feather-icon icon="XCircleIcon"></feather-icon>
            </b-link> -->
         </div>
      </template>
      <template #cell(isForceUpdate)="{ item }">
         <feather-icon v-if="item.isForceUpdate" class="text-success" icon="CheckCircleIcon"></feather-icon>
         <feather-icon v-else class="text-danger" icon="XCircleIcon"></feather-icon>
      </template>
      <template #cell(totalCount)="{ item }">
         <div class="d-flex text-nowrap">
            {{ currency(item.totalCount) }}
         </div>
      </template>
      <template #cell(succesCount)="{ item }">
         <div class="d-flex text-nowrap">
            {{ currency(item.succesCount) }}
         </div>
      </template>
      <template #cell(errorCount)="{ item }">
         <div class="d-flex text-nowrap">
            {{ currency(item.errorCount) }}
         </div>
      </template>
   </form-table-hrm>
</template>

<script>
import { BCard, BBadge, BLink, VBTooltip } from 'bootstrap-vue';
import FormTableHrm from '@/components/forms/form-table-hrm.vue';
import CustomJobService from '@/services/managment/customjob.service';

export default {
   components: {
      BCard,
      BBadge,
      FormTableHrm,
      BLink
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
               key: 'jobType',
               label: this.$t('JobType'),
               thClass: 'text-center',
               tdClass: 'text-center'
            },
            {
               key: 'extendData',
               label: this.$t('extendData'),
               thClass: 'text-center',
               tdClass: 'text-center'
            },
            {
               key: 'region',
               label: this.$t('Oblast'),
               thClass: 'text-center',
               tdClass: 'text-center'
            },
            {
               key: 'district',
               label: this.$t('Region'),
               thClass: 'text-center',
               tdClass: 'text-center'
            },
            {
               key: 'docDate',
               label: this.$t('ondate'),
               thClass: 'text-center',
               tdClass: 'text-center'
            },
            {
               key: 'isForceUpdate',
               label: this.$t('isForceUpdate'),
               thClass: 'text-center',
               tdClass: 'text-center'
            },
            {
               key: 'totalCount',
               label: this.$t('totalCount'),
               thClass: 'text-center',
               tdClass: 'text-center ',
               sortable: true
            },
            {
               key: 'succesCount',
               label: this.$t('succesCount'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'errorCount',
               label: this.$t('errorCount'),
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
         isBusy: false,
         deleteLoading: false
      };
   },
   methods: {
      DbClick(item) {
         if (this.$can('CustomJobEdit', 'permissions') && ![12, 6, 17].includes(item.statusId)) {
            this.$router.push({
               name: 'EditCustomJob',
               params: { id: item.id }
            });
         }
      },
      Delete(item) {
         this.$swal.fire({
            icon: 'info',
            title: this.$t('WantDelete'),
            showLoaderOnConfirm: true,
            preConfirm: () => {
               this.deleteLoading = true;

               return CustomJobService.Delete(item.id)
                  .then(() => {
                     this.makeToast(this.$t('DeleteSuccess'), 'success');
                     this.Refresh();
                  })
                  .catch(this.SwalError)
                  .finally(() => {
                     this.deleteLoading = false;
                  });
            },
            allowOutsideClick: () => !this.$swal.isLoading()
         });
      },
      Refresh() {
         this.isBusy = true;
         CustomJobService.GetList(this.filter)
            .then((res) => {
               this.items = res.data.rows;
               this.filter.total = res.data.total;
            })
            .finally(() => {
               this.isBusy = false;
            });
      },
      Approve(item) {
         this.$swal.fire({
            icon: 'info',
            title: this.$t('WantApprove'),
            showLoaderOnConfirm: true,
            preConfirm: () => {
               return CustomJobService.Approve(item.id)
                  .then(() => {
                     this.makeToast(this.$t('ApproveSuccess'), 'success');
                     this.Refresh();
                  })
                  .catch(this.SwalError);
            },
            allowOutsideClick: () => !this.$swal.isLoading()
         });
      },
      CancelApprove(item) {
         this.$swal.fire({
            icon: 'info',
            title: this.$t('WantCancel'),
            showLoaderOnConfirm: true,
            preConfirm: () => {
               return CustomJobService.CancelApprove(item.id)
                  .then(() => {
                     this.makeToast(this.$t('CancelSuccess'), 'success');
                     this.Refresh();
                  })
                  .catch(this.SwalError);
            },
            allowOutsideClick: () => !this.$swal.isLoading()
         });
      }
   }
};
</script>
