<template>
   <form-table-hrm
      :items="items"
      :fields="fields"
      :filter.sync="filter"
      :busy="isBusy"
      :actions="{
         //  create: {
         //     name: 'EditStaffing',
         //     permission: 'StaffingCreate'
         //  },
         edit: {
            name: 'EditStaffing',
            permission: 'StaffingEdit'
         }
      }"
      @row-dblclicked="DbClick"
      @request="Refresh"
   >
      <template #cell(actions)="{ item }">
         <div class="text-right" style="text-wrap: nowrap">
            <!-- edit -->
            <b-link
               v-if="$can('StaffingView', 'permissions')"
               :to="{ name: 'EditStaffing', params: { id: item.id, isView: true } }"
               v-b-tooltip.hover.top="$t('View')"
               class="mr-1 cursor-pointer"
            >
               <feather-icon icon="EyeIcon"></feather-icon>
            </b-link>
            <!-- Receieved -->
            <b-link
               v-if="$can('StaffingReceieved', 'permissions') && item.canReceived"
               @click="Receieved(item)"
               class="mr-1 text-success cursor-pointer"
               v-b-tooltip.hover.top="$t('Receieved')"
            >
               <feather-icon icon="CheckCircleIcon"></feather-icon>
            </b-link>
            <!-- reject -->
            <b-link
               v-if="$can('StaffingReject', 'permissions') && item.canReject"
               @click="Reject(item)"
               class="mr-1 text-danger cursor-pointer"
               v-b-tooltip.hover.top="$t('Reject')"
            >
               <feather-icon icon="XCircleIcon"></feather-icon>
            </b-link>
         </div>
      </template>
   </form-table-hrm>
</template>

<script>
import { BCard, BCardText, VBTooltip, BBadge, BButton, BLink, BModal, VBModal } from 'bootstrap-vue';
import FormTableHrm from '@/components/forms/form-table-hrm.vue';
import StaffingService from '@/services/hrm/staffing.service';

export default {
   components: {
      BCard,
      FormTableHrm,
      BCardText,
      BButton,
      BBadge,
      BLink,
      BModal
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
         if (this.$can('StaffingView', 'permissions')) {
            this.$router.push({
               name: 'EditStaffing',
               params: { id: item.id, isView: true }
            });
         }
      },
      Reject(item) {
         this.$swal.fire({
            icon: 'question',
            title: this.$t('WantReject'),
            showLoaderOnConfirm: true,
            input: 'text',
            inputPlaceholder: this.$t('RejectMessage'),
            preConfirm: (msg) => {
               return StaffingService.Reject({
                  id: item.id,
                  message: msg
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
      Receieved(item) {
         this.$swal.fire({
            icon: 'question',
            title: this.$t('WantReceieved'),
            showLoaderOnConfirm: true,
            preConfirm: () => {
               return StaffingService.Receieved({
                  id: item.id,
                  message: ''
               })
                  .then(() => {
                     this.makeToast(this.$t('ReceievedSuccess'), 'success');
                     this.Refresh();
                  })
                  .catch(this.SwalError);
            },
            allowOutsideClick: () => !this.$swal.isLoading()
         });
      },
      Refresh() {
         this.isBusy = true;
         StaffingService.GetListForOwnOrg(this.filter)
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
