<template>
   <div>
      <form-table-hrm
         :items="items"
         :fields="fields"
         :filter.sync="filter"
         searchable
         :busy="isBusy"
         @request="Refresh"
      >
         <!-- filtes -->
         <template #filter>
            <b-row>
               <b-col cols="12" md="3">
                  <form-select
                     :options="OrderList"
                     v-model="filter.tableId"
                     @input="Refresh"
                     :label="$t('orders')"
                  ></form-select>
               </b-col>
            </b-row>
            <b-row align-v="center">
               <b-col cols="12" md="8">
                  <StatusSelect v-model="filter.statusId" @input="Refresh" :filter="[1, 27, 21, 25, 24]" />
               </b-col>
               <b-col cols="12" md="4">
                  <b-input-group>
                     <b-form-input v-model="filter.search" @keyup.enter="Refresh" :placeholder="$t('search')" />
                     <b-input-group-append>
                        <b-button @click="Refresh" variant="primary">
                           <feather-icon icon="SearchIcon" />
                        </b-button>
                     </b-input-group-append>
                  </b-input-group>
               </b-col>
            </b-row>
         </template>

         <template #cell(tableId)="{ item }">
            {{ TableName(item.tableId) }}
         </template>

         <template #cell(actions)="{ item }">
            <div class="text-center" style="text-wrap: nowrap">
               <!-- view -->
               <b-link
                  :to="{ name: viewName(item.tableId), params: { id: item.id } }"
                  v-b-tooltip.hover.top="$t('View')"
                  class="mr-1 cursor-pointer"
               >
                  <feather-icon icon="EyeIcon"></feather-icon>
               </b-link>

               <!-- document history -->
               <HistoryModalButton :id="item.id" :table-id="item.tableId" />
            </div>
         </template>
      </form-table-hrm>
   </div>
</template>
<script>
import {
   BRow,
   BCol,
   BCard,
   VBTooltip,
   BLink,
   BCardText,
   BSpinner,
   BInputGroup,
   BFormGroup,
   BInputGroupAppend,
   BButton,
   BFormInput
} from 'bootstrap-vue';
import FormTableHrm from '@/components/forms/form-table-hrm.vue';
import HistoryModalButton from '@/views/components/document/HistoryModalButton.vue';
import StatusSelect from '@/views/components/document/StatusSelect.vue';
import DocumentHeldForEmpService from '@/services/hrm/DocumentHeldForEmp.service.js';
import HeldForSignOrEmpMixin from '@/mixins/hrm/HeldForSignOrEmp.js';

export default {
   components: {
      BCard,
      BRow,
      BCol,
      BLink,
      BCardText,
      BSpinner,
      BFormInput,
      BInputGroup,
      BFormGroup,
      BInputGroupAppend,
      BButton,
      FormTableHrm,
      HistoryModalButton,
      StatusSelect
   },
   mixins: [HeldForSignOrEmpMixin],
   directives: {
      'b-tooltip': VBTooltip
   },
   props: {
      employeeInfo: {
         type: Object,
         default: () => ({})
      }
   },
   data() {
      return {
         items: [],
         isBusy: false
      };
   },
   methods: {
      Refresh() {
         this.isBusy = true;
         DocumentHeldForEmpService.GetPinflData({ ...this.filter, personId: this.employeeInfo?.id })
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
