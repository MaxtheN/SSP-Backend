<template>
   <form-table-hrm
      :items="items"
      :fields="fields"
      :delete-loading="deleteLoading"
      :filter.sync="filter"
      searchable
      :busy="isBusy"
      :actions="{
         edit: {
            name: 'EditArbitrationcourt',
            permission: 'ArbitrationCourtApplicationEdit'
         },
         delete: {
            name: 'EditArbitrationcourt',
            permission: 'ArbitrationCourtApplicationDelete'
         }
      }"
      @row-dblclicked="DbClick"
      @request="Refresh"
      @row-delete="Delete"
   >
      <template #filter>
         <b-row>
            <b-col cols="auto">
               <b-button
                  class="mt-2"
                  variant="primary"
                  @click="$router.push({ name: 'EditArbitrationcourt', params: { id: 0 } })"
               >
                  <feather-icon icon="PlusIcon"></feather-icon>
                  {{ $t('create') }}</b-button
               >
            </b-col>
            <b-col></b-col>
            <b-col cols="12" md="4">
               <div>
                  <label for>{{ $t('innOrPinfl') }}</label>
                  <b-input-group>
                     <b-form-input
                        type="text"
                        v-model="filter.search"
                        debounce="300"
                        @keyup.enter="Refresh"
                        :placeholder="$t('innOrPinfl')"
                     />
                     <b-input-group-append>
                        <b-button @click="Refresh" size="sm" variant="primary">
                           <feather-icon icon="SearchIcon" />
                        </b-button>
                     </b-input-group-append>
                  </b-input-group>
               </div>
            </b-col>
         </b-row>
         <b-row class="mt-1 d-flex align-items-center">
            <b-col>
               <StatusSelect v-model="filter.statusId" :filter="[1, 4]" />
            </b-col>
            <b-col cols="12" md="3" lg="3">
               <form-picker
                  v-model="filter.fromDocDate"
                  @change="Refresh"
                  :label="$t('fromdate')"
                  :placeholder="$t('fromdate')"
               ></form-picker>
            </b-col>
            <b-col cols="12" md="3" lg="3">
               <form-picker
                  v-model="filter.toDocDate"
                  @change="Refresh"
                  :label="$t('todate')"
                  :placeholder="$t('todate')"
               ></form-picker>
            </b-col>
         </b-row>
      </template>
      <template #cell(actions)="{ item }">
         <div class="text-center">
            <!-- edit -->

            <b-link
               v-if="$can('ArbitrationCourtApplicationEdit', 'permissions')"
               :to="{ name: 'EditArbitrationcourt', params: { id: item.id } }"
               style="margin-right: 5px; cursor: pointer"
               v-b-tooltip.hover.top="$t('Edit')"
            >
               <feather-icon icon="EditIcon"></feather-icon>
            </b-link>
            <b-link
               :to="{ name: 'ViewArbitrationcourt', params: { id: item.id } }"
               v-b-tooltip.hover.top="$t('View')"
               v-if="$can('ArbitrationCourtApplicationView', 'permissions')"
               class="mr-1 cursor-pointer"
            >
               <feather-icon icon="EyeIcon"></feather-icon>
            </b-link>

            <!-- delete -->

            <b-link
               v-if="$can('ArbitrationCourtApplicationDelete', 'permissions')"
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
      <template #cell(amount)="{ item }">
         <span style="white-space: nowrap">{{ currency(item.amount) }}</span>
      </template>
      <template #cell(arbitrationAmount)="{ item }">
         <span style="white-space: nowrap"> {{ currency(item.arbitrationAmount) }}</span>
      </template>
      <template #cell(status)="{ item }">
         <b-badge :variant="getColor({ statusId: item.application.statusId, status: item.application.status })">{{
            item.application.status
         }}</b-badge>
      </template>
   </form-table-hrm>
</template>

<script>
import {
   BCard,
   BSpinner,
   VBTooltip,
   BCardText,
   VBModal,
   BLink,
   BModal,
   BBadge,
   BRow,
   BCol,
   BInputGroup,
   BInputGroupAppend,
   BFormInput,
   BButton
} from 'bootstrap-vue';
import FormTableHrm from '@/components/forms/form-table-hrm.vue';

import ArbitrationCourtService from '@/services/arbitrationcourt/arbitrationcourt.service.js';
import StatusSelect from '@/views/components/document/StatusSelect.vue';

export default {
   components: {
      BRow,
      BCol,
      BInputGroup,
      BInputGroupAppend,
      BFormInput,
      BButton,
      StatusSelect,
      BCard,
      BBadge,
      FormTableHrm,
      BModal,
      BLink,
      VBTooltip,
      VBModal,
      BCardText,
      BSpinner
   },
   directives: {
      'b-tooltip': VBTooltip,
      'b-modal': VBModal
   },
   data() {
      return {
         items: [],

         discussionDate: '',
         filter: {
            statusId: null,
            statusIds: [0],
            sortBy: '',
            search: '',
            fromDocDate: '',
            toDocDate: '',
            orderType: 'asc',
            page: 1,
            pageSize: 20,
            perPageOptions: [10, 20, 50, 100]
         },

         fields: [
            {
               key: 'actions',
               label: this.$t('actions'),
               thClass: 'text-center',
               tdClass: 'text-center',
               stickyColumn: true
            },
            {
               key: 'application.docOn',
               label: this.$t('docOn'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'applicationContractor',
               label: this.$t('contractor1'),

               sortable: true
            },
            {
               key: 'application.step',
               label: this.$t('Step'),

               sortable: true
            },

            {
               key: 'arbitrationApplicationType',
               label: this.$t('arbitrationApplicationType'),
               thStyle: {
                  minWidth: '200px'
               }
            },
            {
               key: 'responsibleContractor',
               label: this.$t('responsibleContractor')
            },

            {
               key: 'amount',
               label: this.$t('amount'),
               tdClass: 'text-right'
            },
            {
               key: 'arbitrationAmount',
               label: this.$t('arbitrationAmount'),
               tdClass: 'text-right'
            },
            {
               key: 'status',
               label: this.$t('status'),
               tdClass: 'text-right',
               stickyColumn: true
            }
         ],
         isBusy: false,
         deleteLoading: false
      };
   },

   methods: {
      DbClick(item) {
         this.$router.push({
            name: 'EditArbitrationcourt',
            params: { id: item.id }
         });
      },
      Refresh() {
         this.isBusy = true;
         ArbitrationCourtService.GetList(this.filter)
            .then((res) => {
               this.items = res.data.rows;
               this.filter.total = res.data.total;
            })
            .finally(() => {
               this.isBusy = false;
            });
      },
      Delete(item) {
         this.deleteLoading = true;
         ArbitrationCourtService.Delete(item.id)
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
      }
   },
   watch: {
      'filter.statusId': function (newVal) {
         if (newVal == null) {
            this.filter.statusIds = [0];
            this.Refresh();
         } else {
            this.filter.statusIds[0] = newVal;
            this.Refresh();
         }
      }
   }
};
</script>
