<template>
   <form-table-hrm
      :items="items"
      :fields="fields"
      :filter.sync="filter"
      searchable
      :busy="isBusy"
      :deleteLoading="deleteLoading"
      :actions="{
         create: {
            name: 'EditUniteOfMeasure',
            permission: 'UniteOfMeasureCreate'
         },
         edit: {
            name: 'EditUniteOfMeasure',
            permission: 'UniteOfMeasureEdit'
         },
         delete: {
            name: 'EditUniteOfMeasure',
            permission: 'UniteOfMeasureDelete'
         }
      }"
      @row-dblclicked="DbClick"
      @request="Refresh"
      @row-delete="Delete"
   >
      <template #cell(stateId)="{ item }">
         <b-badge :variant="item.stateId == '2' ? 'light-danger' : 'light-success'">{{ item.state }}</b-badge>
      </template>
   </form-table-hrm>
</template>

<script>
import { BCard, BBadge } from 'bootstrap-vue';
import FormTableHrm from '@/components/forms/form-table-hrm.vue';
import UniteOfMeasureService from '@/services/info/uniteofmeasure.service';

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
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'code',
               label: this.$t('code')
            },
            {
               key: 'fullName',
               label: this.$t('fullname'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'shortName',
               label: this.$t('shortname'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },

            {
               key: 'state',
               label: this.$t('state')
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
            name: 'EditIndicator',
            params: { id: item.id }
         });
      },
      Delete(item) {
         this.deleteLoading = true;
         UniteOfMeasureService.Delete(item.id)
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
         UniteOfMeasureService.GetList(this.filter)
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
