<template>
   <b-card>
      <b-tabs v-model="tab">
         <b-tab :title="$t('variant-3')"></b-tab>
         <b-tab :title="$t('variant-4')"></b-tab>

         <form-table-hrm
            :items="items"
            :fields="fields"
            :filter.sync="filter"
            searchable
            :busy="isBusy"
            :deleteLoading="deleteLoading"
            @row-dblclicked="DbClick"
            @request="Refresh"
         >
            <template #cell(stateId)="{ item }">
               <b-badge :variant="item.stateId == '2' ? 'light-danger' : 'light-success'">{{ item.state }}</b-badge>
            </template>
            <template #cell(actions)="{ item }">
               <div class="text-center">
                  <!-- edit -->

                  <b-link
                     :to="{ name: 'EditIndicator', params: { id: item.id }, query: { tab: tab } }"
                     style="margin-right: 5px; cursor: pointer"
                     v-b-tooltip.hover.top="$t('Edit')"
                  >
                     <feather-icon icon="EditIcon"></feather-icon>
                  </b-link>

                  <!-- delete -->

                  <b-link
                     class="text-danger"
                     @click="$refs['DeleteModal' + item.id].show()"
                     style="cursor: pointer"
                     v-b-tooltip.hover.top="$t('Delete')"
                  >
                     <feather-icon icon="TrashIcon"></feather-icon>
                  </b-link>
                  <b-modal
                     :ref="'DeleteModal' + item.id"
                     :cancel-title="$t('Cancel')"
                     :ok-title="$t('Accept')"
                     cancel-variant="danger"
                     ok-variant="success"
                     @ok="Delete(item)"
                  >
                     <template #modal-title>
                        {{ $t('Accept') }}
                        <b-spinner v-if="deleteLoading" small></b-spinner>
                     </template>
                     <b-card-text>
                        <h5>ID : {{ item.id }}</h5>
                        <h5>{{ $t('WantDelete') }}</h5>
                     </b-card-text>
                  </b-modal>
               </div>
            </template>
            <template #filter>
               <b-row>
                  <b-col cols="2">
                     <b-button
                        variant="primary"
                        @click="$router.push({ name: 'EditIndicator', params: { id: 0 }, query: { tab: tab } })"
                     >
                        <feather-icon icon="PlusIcon"></feather-icon>
                        {{ $t('create') }}</b-button
                     >
                  </b-col>
               </b-row>
            </template>
         </form-table-hrm>
      </b-tabs>
   </b-card>
</template>

<script>
import {
   BCard,
   BBadge,
   BTab,
   BTabs,
   BLink,
   BModal,
   BCardText,
   BRow,
   VBTooltip,
   BCol,
   BButton,
   BSpinner,
   BTooltip
} from 'bootstrap-vue';
import FormTableHrm from '@/components/forms/form-table-hrm.vue';
import IndicatorService from '@/services/kpi/indicator.service';

export default {
   components: {
      BCard,
      BCardText,
      BLink,
      BModal,
      BTab,
      BTabs,
      BBadge,
      FormTableHrm,
      BRow,
      BCol,
      BButton,
      BSpinner,
      BTooltip
   },
   directives: {
      'b-tooltip': VBTooltip
   },
   data() {
      return {
         items: [],
         tab: 0,
         fields: [
            {
               key: 'id',
               label: this.$t('id'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'fullName',
               label: this.$t('fullname'),
               thClass: 'text-center',
               tdClass: 'text-left',
               sortable: true
            },
            {
               key: 'shortName',
               label: this.$t('shortname'),
               thClass: 'text-center',
               tdClass: 'text-left',
               sortable: true
            },
            {
               key: 'code',
               label: this.$t('ordercode')
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
            params: { id: item.id },
            query: { tab: this.tab }
         });
      },
      Delete(item) {
         this.deleteLoading = true;
         IndicatorService.Delete(item.id, this.tab)
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
         IndicatorService.GetList(this.filter, this.tab)
            .then((res) => {
               this.items = res.data.rows;
               this.filter.total = res.data.total;
            })
            .finally(() => {
               this.isBusy = false;
            });
      }
   },
   watch: {
      // eslint-disable-next-line func-names
      tab: function (newval, oldvalue) {
         this.Refresh();
      }
   }
};
</script>
