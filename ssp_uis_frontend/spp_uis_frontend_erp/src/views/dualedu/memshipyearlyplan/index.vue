<template>
   <form-table-hrm :items="items" :fields="fields" :filter.sync="filter" searchable :busy="isBusy"
      :delete-loading="deleteLoading" :actions="{
         create: {

            name: 'EditMemshipYearlyPlan',
            permission: 'MemshipYearlyPlanCreate'
         },
         edit: {

            name: 'EditMemshipYearlyPlan',
            permission: 'MemshipYearlyPlanEdit'
         },
         delete: {
            name: 'EditMemshipYearlyPlan',
            permission: 'MemshipYearlyPlanDelete'
         }
      }" @row-dblclicked="DbClick" @request="Refresh" @row-delete="Delete">
      <template #cell(actions)="{ item }">
         <div class="text-center" style="text-wrap: nowrap">
            <b-link v-if="item.statusId != 2" :to="{ name: 'EditMemshipYearlyPlan', params: { id: item.id } }" v-b-tooltip.hover.top="$t('Edit')"
               class="mr-1 cursor-pointer">
               <feather-icon icon="EditIcon"></feather-icon>
            </b-link>
            <b-link v-if="item.statusId != 2" class="mr-1 text-success cursor-pointer" v-b-tooltip.hover.top="$t('Approve')" @click="Approve(item)">
               <feather-icon icon="CheckCircleIcon"></feather-icon>
            </b-link>
            <b-link v-if="item.statusId != 3" class="text-danger cursor-pointer mr-1" v-b-tooltip.hover.top="$t('Cancel')"
               @click="CancelApprove(item)">
               <feather-icon icon="XCircleIcon"></feather-icon>
            </b-link>
            <b-link v-if="item.statusId != 2" class="text-danger mr-1 cursor-pointer" v-b-tooltip.hover.top="$t('Delete')" @click="Delete(item)">
               <feather-icon icon="TrashIcon"></feather-icon>
            </b-link>
         </div>
      </template>
   </form-table-hrm>
</template>
 
<script>
import { BCard, BBadge, BLink, VBTooltip } from 'bootstrap-vue';
import FormTableHrm from '@/components/forms/form-table-hrm.vue';
import MemshipYearlyPlanService from '@/services/dualedu/memshipyearlyplan.service';

export default {
   components: {
      BCard,
      FormTableHrm,
      BBadge,
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
               key: 'docNumber',
               label: this.$t('docnumber'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'year',
               label: this.$t('docyear'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'docOn',
               label: this.$t('ondate'),
               thClass: 'text-center',
               tdClass: 'text-center',
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
         this.$router.push({
            name: 'EditMemshipYearlyPlan',
            params: { id: item.id }
         });
      },
      Delete(item) {
         this.deleteLoading = true;
         MemshipYearlyPlanService.Delete(item.id)
            .then(() => {
               this.makeToast(this.$t('DeleteSuccess'), 'success');
               this.Refresh();
            })
            .catch((error) => {
               this.showApiError(error);
            })
            .finally(() => {
               this.deleteLoading = false;
            });
      },
      Approve(item) {
         console.log(item)
         this.$swal.fire({
            icon: 'info',
            title: this.$t('WantApprove'),
            showLoaderOnConfirm: true,
            preConfirm: () => {
               return MemshipYearlyPlanService.Accept({
                  statusId: item.statusId,
                  id: item.id
               })
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
               return MemshipYearlyPlanService.Cancel({
                  statusId: item.statusId,
                  id: item.id
               })
                  .then(() => {
                     this.makeToast(this.$t('CancelSuccess'), 'success');
                     this.Refresh();
                  })
                  .catch(this.SwalError);
            },
            allowOutsideClick: () => !this.$swal.isLoading()
         });
      },
      Refresh() {
         this.isBusy = true;
         MemshipYearlyPlanService.GetList(this.filter)
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
 