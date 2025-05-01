<template>
   <form-table-hrm :items="items" :fields="fields" :filter.sync="filter" searchable :busy="isBusy"
      :delete-loading="deleteLoading" :actions="{
         edit: {
            name: 'EditRestrictionSendingApp',
            permission: 'RestrictionSendingAppEdit'

         },
         create: {
            name: 'EditRestrictionSendingApp',
            permission: 'RestrictionSendingAppEdit'
         },
         delete: {
            name: 'EditRestrictionSendingApp',
            permission: 'RestrictionSendingAppDelete'
         }
      }" @row-dblclicked="DbClick" @request="Refresh" @row-delete="Delete">
      <template #cell(stateId)="{ item }">
         <b-badge :variant="item.stateId == '2' ? 'light-danger' : 'light-success'">{{ item.state }}</b-badge>
      </template>
   </form-table-hrm>
</template>
 
<script>
import { BCard, BBadge } from 'bootstrap-vue';
import FormTableHrm from '@/components/forms/form-table-hrm.vue';
import RestrictionSendingAppService from '@/services/managment/RestrictionSendingApp.service';

export default {
   components: {
      BCard,
      BBadge,
      FormTableHrm
   },
   data() {
      return {
         items: [],
         fields: [
            {
               key: 'id',
               label: this.$t('id'),
               thClass: 'text-left',
               tdClass: 'text-left',
               sortable: true
            },
            {
               key: 'applicatonModel',
               label: this.$t('appId'),
               thClass: 'text-left',
               tdClass: 'text-left',
               sortable: true
            },

            {
               key: 'table',
               label: this.$t('tableId'),
               thClass: 'text-left',
               tdClass: 'text-left',
               sortable: true
            },
            
            {
               key: 'startAt',
               label: this.$t('startAt'),
               thClass: 'text-right',
               tdClass: 'text-right',
               sortable: true
            },

            {
               key: 'endAt',
               label: this.$t('endAtt'),
               thClass: 'text-right',
               tdClass: 'text-right',
               sortable: true
            },
            {
               key: 'details',
               label: this.$t('details'),
               thClass: 'text-left',
               tdClass: 'text-left',
               sortable: true
            },
            {
               key: 'stateId',
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
            tableId: null,
            appId: null,
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
            name: 'EditRestrictionSendingApp',
            params: { id: item.id }
         });
      },
      Delete(item) {
         this.deleteLoading = true;
         RestrictionSendingAppService.Delete(item.id)
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
      Refresh() {
         this.isBusy = true;
         RestrictionSendingAppService.GetList(this.filter)
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
 