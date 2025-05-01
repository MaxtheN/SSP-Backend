<template>
   <form-table-hrm
      :items="items"
      :fields="fields"
      :delete-loading="deleteLoading"
      :filter.sync="filter"
      searchable
      :busy="isBusy"
      :actions="{
         create: {
            name: 'EditArbitrationjudge',
            permission: 'ArbitrationJudgeCreate'
         },
         edit: {
            name: 'EditArbitrationjudge',
            permission: 'ArbitrationJudgeEdit'
         },
         delete: {
            name: 'EditArbitrationjudge',
            permission: 'ArbitrationJudgeDelete'
         }
      }"
      @row-dblclicked="DbClick"
      @request="Refresh"
      @row-delete="Delete"
   >
      <template #cell(fullName)="{ item }"> {{ item.lastName }} {{ item.firstName }} {{ item.middleName }} </template>
      <template #cell(state)="{ item }">
         <b-badge :variant="item.stateId == '2' ? 'light-danger' : 'light-success'">{{ item.state }}</b-badge>
      </template>
      <template #cell(actions)="{ item }">
         <div class="text-center">
            <!-- edit -->

            <b-link
               v-if="$can('ArbitrationJudgeEdit', 'permissions')"
               :to="{ name: 'EditArbitrationjudge', params: { id: item.id } }"
               style="margin-right: 5px; cursor: pointer"
               v-b-tooltip.hover.top="$t('Edit')"
            >
               <feather-icon icon="EditIcon"></feather-icon>
            </b-link>

            <!-- delete -->

            <b-link
               v-if="$can('ArbitrationJudgeDelete', 'permissions')"
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
   </form-table-hrm>
</template>

<script>
import { BCard, BSpinner, VBTooltip, BCardText, VBModal, BLink, BModal, BBadge } from 'bootstrap-vue';
import FormTableHrm from '@/components/forms/form-table-hrm.vue';
import ArbitrationJudgeService from '@/services/arbitrationjudge/arbitrationjudge.service.js';

export default {
   components: {
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
         filter: {
            search: '',
            sortBy: '',
            orderType: 'asc',
            page: 1,
            pageSize: 20
         },

         fields: [
            {
               key: 'actions',
               label: this.$t('actions'),
               thClass: 'text-center',
               tdStyle: {
                  maxWidth: '10px'
               },
               stickyColumn: true
            },
            {
               key: 'fullName',
               label: this.$t('fullName'),
               thClass: 'text-center',
               tdClass: 'text-left',
               sortable: true
            },
            {
               key: 'positionName',
               label: this.$t('position'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },

            {
               key: 'organizationName',
               label: this.$t('organization'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'state',
               label: this.$t('state'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            }
         ],
         isBusy: false,
         deleteLoading: false
      };
   },

   methods: {
      DbClick(item) {
         this.$router.push({
            name: 'EditArbitrationjudge',
            params: { id: item.id }
         });
      },
      Refresh() {
         this.isBusy = true;
         ArbitrationJudgeService.GetList(this.filter)
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
         ArbitrationJudgeService.Delete(item.id)
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
   }
};
</script>
