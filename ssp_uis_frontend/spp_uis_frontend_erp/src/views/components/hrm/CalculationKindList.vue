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
            name: 'EditCalculationKind',
            permission: 'CalculationKindCreate'
         },
         edit: {
            name: 'EditCalculationKind',
            permission: 'CalculationKindEdit'
         },
         delete: {
            name: 'DeleteCalculationKind',
            permission: 'CalculationKindDelete'
         }
      }"
      select-mode="single"
      :selectable="selectable"
      @row-dblclicked="DbClick"
      @request="Refresh"
      @row-delete="Delete"
   >
      <template #cell(stateId)="{ item }">
         <b-badge :variant="item.stateId == '2' ? 'light-danger' : 'light-success'">{{ item.state }}</b-badge>
      </template>
      <template #cell(formula)="{ item }">
         {{ item.formula ? $t('yes') : $t('no') }}
      </template>
   </form-table-hrm>
</template>

<script>
import { BCard, BBadge } from 'bootstrap-vue';
import FormTableHrm from '@/components/forms/form-table-hrm.vue';
import CalculationKindService from '@/services/hrm/calculationkind.service';

export default {
   components: {
      BCard,
      BBadge,
      FormTableHrm
   },
   emits: ['row-selected'],
   // eslint-disable-next-line vue/require-prop-types
   props: ['selectable', 'calculationKindId'],
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
               label: this.$t('code'),
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
               key: 'formula',
               label: this.$t('formula'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'percentage',
               label: this.$t('percentage'),
               thClass: 'text-center',
               tdClass: 'text-center',
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
         if (this.selectable) {
            this.$emit('row-selected', item);
         } else {
            this.$router.push({
               name: 'EditCalculationKind',
               params: { id: item.id }
            });
         }
      },
      Delete(item) {
         this.deleteLoading = true;
         CalculationKindService.Delete(item.id)
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
         CalculationKindService.GetList(this.filter)
            .then((res) => {
               this.items = res.data.rows;
               this.filter.total = res.data.total;
            })
            .finally(() => {
               this.isBusy = false;
            });
      },
      rowClass(item, type) {
         if (item && type === 'row') {
            if (item.id == this.calculationKindId) {
               return 'b-table-row-selected table-active';
            } else {
               return '';
            }
         } else {
            return null;
         }
      }
   }
};
</script>
<style>
.table.b-table > tbody .b-table-row-selected.table-active td {
   background-color: #8effc3 !important;
}
</style>
