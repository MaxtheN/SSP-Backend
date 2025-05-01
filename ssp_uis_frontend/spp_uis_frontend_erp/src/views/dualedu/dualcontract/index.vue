<template>
   <form-table-hrm
      :items="items"
      :fields="fields"
      :filter.sync="filter"
      searchable
      :busy="isBusy"
      :actions="{}"
      @request="Refresh"
   >
      <template #cell(contracttorFullName)="{ item }"
         ><span style="color: blue"> {{ item.contracttorInn }}</span> -{{ item.contracttorFullName }}</template
      >

      <template #cell(actions)="{ item }">
         <b-link
            :to="{ name: `EditDualContract`, params: { id: item.id } }"
            v-b-tooltip.hover.top="$t('View')"
            style="margin-right: 5px"
         >
            <feather-icon icon="EyeIcon"></feather-icon>
         </b-link>
      </template>
   </form-table-hrm>
</template>

<script>
import { BCard, BLink, VBTooltip } from 'bootstrap-vue';
import FormTableHrm from '@/components/forms/form-table-hrm.vue';
import DualContractService from '@/services/dualedu/dualcontract.service';

export default {
   components: {
      BCard,
      BLink,
      FormTableHrm,
      VBTooltip
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
               key: 'contracttorFullName',
               label: this.$t('FIO'),

               sortable: true
            },
            {
               key: 'institute',
               label: this.$t('Institute'),

               sortable: true
            },
            {
               key: 'speciality',
               label: this.$t('Specialty'),
               sortable: true
            },
            {
               key: 'eduType',
               label: this.$t('eduType'),

               sortable: true
            },
            {
               key: 'dualEducationType',
               label: this.$t('DualEducationType'),
               sortable: true
            },
            {
               key: 'docDate',
               label: this.$t('docOn'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'status',
               label: this.$t('status'),
               thClass: 'text-center',
               tdClass: 'text-center'
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
      Refresh() {
         this.isBusy = true;
         DualContractService.GetList(this.filter)
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
