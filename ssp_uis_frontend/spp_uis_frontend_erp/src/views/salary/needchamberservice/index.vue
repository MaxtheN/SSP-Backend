<template>
   <div>
      <b-tabs class="nav-tabs mb-0" nav-class="mb-0" v-model="tab" @input="Refresh">
         <b-tab :title="$t('all')"> </b-tab>
         <b-tab :title="$t('pullik')"> </b-tab>
         <b-tab :title="$t('tekin')"> </b-tab>
      </b-tabs>

      <form-table-hrm
         :items="items"
         :fields="fields"
         :filter.sync="filter"
         searchable
         :busy="isBusy"
         :delete-loading="deleteLoading"
         :actions="{
            create: {
               name: 'EditNeedChamberService',
               permission: 'NeedChamberServiceCreate'
            },
            edit: {
               name: 'EditNeedChamberService',
               permission: 'NeedChamberServiceEdit'
            },
            delete: {
               name: 'EditNeedChamberService',
               permission: 'NeedChamberServiceDelete'
            }
         }"
         @row-dblclicked="DbClick"
         @request="Refresh"
         @row-delete="Delete"
      >
         <template #cell(stateId)="{ item }">
            <b-badge :variant="item.stateId == '2' ? 'light-danger' : 'light-success'">{{ item.state }}</b-badge>
         </template>
         <template #cell(isOffer)="{ item }">
            <feather-icon v-if="item.isOffer" icon="CheckSquareIcon" size="16" />
            <feather-icon v-else icon="SquareIcon" size="16" />
         </template>
         <template #cell(canPayDivided)="{ item }">
            <feather-icon v-if="item.canPayDivided" icon="CheckSquareIcon" size="16" />
            <feather-icon v-else icon="SquareIcon" size="16" />
         </template>
      </form-table-hrm>
   </div>
</template>

<script>
import { BCard, BBadge, BTabs, BTab } from 'bootstrap-vue';
import FormTableHrm from '@/components/forms/form-table-hrm.vue';
import NeedChamberServiceService from '@/services/hrm/needchamberservice.service';

export default {
   components: {
      BCard,
      BBadge,
      BTabs,
      BTab,
      FormTableHrm
   },
   data() {
      return {
         tab: 0,
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
               label: this.$t('kode'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            // {
            //    key: 'orderCode',
            //    label: this.$t('orderCode'),
            //    thClass: 'text-center',
            //    tdClass: 'text-center',
            //    sortable: true
            // },
            {
               key: 'fullName',
               label: this.$t('fullname'),
               sortable: true
            },
            // {
            //    key: 'shortName',
            //    label: this.$t('shortname'),
            //    sortable: true
            // },
            {
               key: 'servicePriceType',
               label: this.$t('servicePriceType'),
               sortable: true
            },
            // {
            //    key: 'meetingType',
            //    label: this.$t('meetingType'),
            //    thClass: 'text-center',
            //    tdClass: 'text-center',
            //    sortable: true
            // },
            // {
            //    key: 'isOffer',
            //    label: this.$t('isOffer'),
            //    sortable: true,
            //    thClass: 'text-center',
            //    tdClass: 'text-center'
            // },
            // {
            //    key: 'canPayDivided',
            //    label: this.$t('canPayDivided'),
            //    sortable: true,
            //    tdClass: 'text-center',
            //    tdClass: 'text-center'
            // },
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
            total: 0,
            isPaid: null
         },
         isBusy: false,
         deleteLoading: false
      };
   },
   methods: {
      DbClick(item) {
         this.$router.push({
            name: 'EditNeedChamberService',
            params: { id: item.id }
         });
      },
      Delete(item) {
         this.deleteLoading = true;
         NeedChamberServiceService.Delete(item.id)
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
         const isPaid = this.tab == 0 ? null : this.tab == 1;
         NeedChamberServiceService.GetList({ ...this.filter, isPaid })
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
