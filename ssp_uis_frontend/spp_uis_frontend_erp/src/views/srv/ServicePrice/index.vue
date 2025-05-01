<template>
   <form-table-hrm
      :items="items"
      :fields="fields"
      :filter.sync="filter"
      searchable
      :busy="isBusy"
      :actions="{
         create: {
            name: 'EditServicePrice',
            permission: 'SrvServicePriceCreate'
         },
         edit: {
            name: 'EditServicePrice',
            permission: 'SrvServicePriceEdit'
         },
         delete: {
            name: 'EditServicePrice',
            permission: 'SrvServicePriceDelete'
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
               v-if="$can('SrvServicePriceEdit', 'permissions') && item.canEdit"
               :to="{ name: 'EditServicePrice', params: { id: item.id } }"
               v-b-tooltip.hover.top="$t('Edit')"
               class="mr-1 cursor-pointer"
            >
               <feather-icon icon="EditIcon"></feather-icon>
            </b-link>
            <!-- delete -->
            <b-link
               v-if="$can('SrvServicePriceDelete', 'permissions') && item.canDelete"
               class="text-danger mr-1 cursor-pointer"
               @click="Delete(item)"
               v-b-tooltip.hover.top="$t('Delete')"
            >
               <feather-icon icon="TrashIcon"></feather-icon>
            </b-link>
            <!-- accept -->
            <b-link
               v-if="$can('SrvServicePriceAccept', 'permissions') && item.canAccept"
               @click="Accept(item)"
               class="mr-1 text-success cursor-pointer"
               v-b-tooltip.hover.top="$t('Approve')"
            >
               <feather-icon icon="CheckCircleIcon"></feather-icon>
            </b-link>
            <!-- cancel -->
            <template v-if="$can('SrvServicePriceCancel', 'permissions') && item.canCancel">
               <b-link
                  class="text-danger cursor-pointer mr-1"
                  v-b-tooltip.hover.top="$t('Cancel')"
                  @click="Cancel(item)"
               >
                  <feather-icon icon="XCircleIcon"></feather-icon>
               </b-link>
            </template>
         </div>
      </template>
   </form-table-hrm>
</template>

<script>
import { BCard, BLink, VBTooltip } from 'bootstrap-vue';
import FormTableHrm from '@/components/forms/form-table-hrm.vue';
import ServicePriceService from '@/services/srv/ServicePrice.service';

export default {
   components: {
      BCard,
      BLink,
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
               key: 'docOn',
               label: this.$t('docOn'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            // {
            //    key: 'details',
            //    label: this.$t('details')
            // },
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
            name: 'EditServicePrice',
            params: { id: item.id }
         });
      },
      Delete(item) {
         this.$swal.fire({
            icon: 'info',
            title: this.$t('WantDelete'),
            showLoaderOnConfirm: true,
            preConfirm: () => {
               return ServicePriceService.Delete(item.id)
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
         ServicePriceService.GetList(this.filter)
            .then((res) => {
               this.items = res.data.rows;
               this.filter.total = res.data.total;
            })
            .finally(() => {
               this.isBusy = false;
            });
      },
      Accept(item) {
         this.$swal.fire({
            icon: 'info',
            title: this.$t('WantAccept'),
            showLoaderOnConfirm: true,
            preConfirm: () => {
               return ServicePriceService.Accept({
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
               return ServicePriceService.Cancel({
                  statusId: item.statusId,
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
      }
   }
};
</script>
