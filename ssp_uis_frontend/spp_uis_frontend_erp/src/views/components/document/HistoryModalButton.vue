<template>
   <span>
      <b-link @click="OpenHistory" v-b-tooltip.hover.top="$t('history')">
         <b-icon-clock-history></b-icon-clock-history>
      </b-link>

      <b-modal size="xl" v-model="dialog" hide-footer :title="$t('history')">
         <b-table
            :items="historyData"
            :fields="historyFields"
            responsive
            no-border-collapse
            :busy="isBusy"
            show-empty
            :empty-text="$t('NotFound')"
         >
            <template #cell(FullName)="{ item }">{{ JSON.parse(item.userInfo).FullName }}</template>
            <template #cell(status)="{ item }">
               <b-badge :variant="getColor(item)">{{ item.status }}</b-badge>
            </template>
         </b-table>
      </b-modal>
   </span>
</template>

<script>
import ManualService from '@/services/others/manual.service';

import { BLink, BModal, BTable, BBadge, VBTooltip, BIconClockHistory } from 'bootstrap-vue';

export default {
   components: {
      BLink,
      BModal,
      BTable,
      BBadge,
      BIconClockHistory
   },
   directives: {
      'b-tooltip': VBTooltip
   },
   props: {
      id: {
         type: Number,
         required: true
      },
      tableId: {
         type: Number,
         required: true,
      }
   },
   data() {
      return {
         isBusy: false,
         dialog: false,
         historyData: [],
         historyFields: [
            {
               key: 'id',
               label: this.$t('id'),
               sortable: false,
               tdClass: 'text-center',
               thClass: 'text-center'
            },
            {
               key: 'FullName',
               label: this.$t('fullname'),
               sortable: false,
               tdClass: 'text-left',
               thClass: 'text-center'
            },
            {
               key: 'dateAt',
               label: this.$t('docdate'),
               sortable: false,
               tdClass: 'text-center',
               thClass: 'text-center'
            },
            {
               key: 'ipAddress',
               label: this.$t('ipAddress'),
               sortable: false,
               tdClass: 'text-center',
               thClass: 'text-center'
            },
            {
               key: 'message',
               label: this.$t('message'),
               sortable: false,
               tdClass: 'text-center',
               thClass: 'text-center'
            },
            {
               key: 'status',
               label: this.$t('status'),
               sortable: false,
               tdClass: 'text-center',
               thClass: 'text-center'
            }
         ]
      };
   },
   methods: {
      OpenHistory() {
         this.dialog = true;
         this.isBusy = true;
         ManualService.GetListByDocumentId(this.tableId, this.id)
            .then((res) => {
               this.historyData = res.data;
            })
            .catch((error) => {
               this.showApiError(error);
            })
            .finally(() => {
               this.isBusy = false;
            });
      }
   }
};
</script>
