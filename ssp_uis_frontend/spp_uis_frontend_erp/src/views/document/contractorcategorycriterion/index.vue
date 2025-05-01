<template>
   <form-table-hrm
      :items="items"
      :actions="{
         
         edit: {
            name: 'EditContractorCategoryCriterion',
            permission: 'ContractorCategoryCriterionEdit'
         },
         create: {
            name: 'EditContractorCategoryCriterion',
            permission: 'ContractorCategoryCriterionCreate'
         },
        
      }"
      :fields="fields"
      :filter.sync="filter"
      searchable
      :busy="isBusy"
      :delete-loading="deleteLoading"
      @row-dblclicked="DbClick"
      @request="refresh"
      @row-delete="Delete"
   >
   <template #cell(maxAmount)="{ item }">
         <div style="white-space: nowrap">
            {{ currency(item.maxAmount) }}
         </div>
      </template>
      <template #cell(minAmount)="{ item }">
         <div style="white-space: nowrap">
            {{ currency(item.minAmount) }}
         </div>
      </template>
      <template #cell(actions)="{ item }">
         <div class="text-center" style="text-wrap: nowrap">
             <!-- View -->
             <b-link
               v-if="$can('ContractorCategoryCriterionView', 'permissions') "
               :to="{ name: 'EditContractorCategoryCriterion', params: { id: item.id } }"
               v-b-tooltip.hover.top="$t('View')"
               class="mr-1 cursor-pointer"
            >
               <feather-icon icon="EyeIcon"></feather-icon>
            </b-link>
            <!-- edit -->
            <b-link
               v-if="$can('ContractorCategoryCriterionEdit', 'permissions')&& item.statusId !=2"
               :to="{ name: 'EditContractorCategoryCriterion', params: { id: item.id } }"
               v-b-tooltip.hover.top="$t('Edit')"
               class="mr-1 cursor-pointer"
            >
               <feather-icon icon="EditIcon"></feather-icon>
            </b-link>
            <!-- delete -->
            <b-link
               v-if="$can('ContractorCategoryCriterionDelete', 'permissions')&& item.statusId !=2"
               class="text-danger mr-1 cursor-pointer"
               @click="$refs['DeleteModal' + item.id].show()"
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
            <!-- accept -->
            <b-link
               v-if="$can('ContractorCategoryCriterionAccept', 'permissions')&& item.statusId !=2"
               @click="$refs['ApproveModal' + item.id].show()"
               class="mr-1 text-success cursor-pointer"
               v-b-tooltip.hover.top="$t('Approve')"
            >
               <feather-icon icon="CheckCircleIcon"></feather-icon>
            </b-link>
            <b-modal
               :ref="'ApproveModal' + item.id"
               :cancel-title="$t('Cancel')"
               :ok-title="$t('Approve')"
               cancel-variant="danger"
               ok-variant="success"
               @ok="Accept(item)"
            >
               <template #modal-title>
                  {{ $t('Approve') }}
               </template>
               <b-card-text>
                  <h5>ID : {{ item.id }}</h5>
                  <h5>{{ $t('WantApprove') }}</h5>
               </b-card-text>
            </b-modal>
            <!-- cancel -->
            <template v-if="$can('ContractorCategoryCriterionCancel', 'permissions')&& item.statusId ==2">
               <b-link
                  class="text-danger cursor-pointer mr-1"
                  v-b-tooltip.hover.top="$t('Cancel')"
                  @click="$refs['CancelModal' + item.id].show()"
               >
                  <feather-icon icon="XCircleIcon"></feather-icon>
               </b-link>
               <b-modal
                  :ref="'CancelModal' + item.id"
                  :title="$t('Reject')"
                  :cancel-title="$t('Cancel')"
                  :ok-title="$t('Accept')"
                  cancel-variant="danger"
                  ok-variant="success"
                  @ok="CancelApproval(item)"
               >
                  <b-card-text>
                     <h5>{{ $t('WantReject') }}</h5>
                  </b-card-text>
               </b-modal>
            </template>
         </div>
      </template>
      <template #cell(status)="{ item }">
         <b-badge :variant="getColor(item)">{{ item.status }}</b-badge>
      </template>
   </form-table-hrm>
</template>

<script>
import { BCard, BCardText, VBTooltip, BBadge, BButton, BLink, BModal, VBModal } from 'bootstrap-vue';
import FormTableHrm from '@/components/forms/form-table-hrm.vue';
import ContractorCategoryCriterionService from '@/services/document/contractorcategorycriterion.service';

export default {
   components: {
      BCard,
      FormTableHrm,
      BCardText,
      BButton,
      BBadge,
      BLink,
      BModal
   },
   directives: {
      'b-tooltip': VBTooltip,
      'b-modal': VBModal
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
               key: 'docNumber',
               label: this.$t('docnumber'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'docOn',
               label: this.$t('docOn'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'contractorCategory',
               label: this.$t('contractorCategory')
            },
            {
               key: 'expirationDate',
               label: this.$t('expirationDate')
            },
            {
               key: 'minAmount',
               label: this.$t('minAmount')
            },
            {
               key: 'maxAmount',
               label: this.$t('maxAmount')
            },
            {
               key: 'status',
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
            perPageOptions: [10, 20, 50, 100, 300],
            total: 0
         },
         isBusy: false,
         deleteLoading: false,
         acceptLoading: false,
         cancelLoading: false
      };
   },
   methods: {
      DbClick(item) {
         this.$router.push({
            name: 'EditContractorCategoryCriterion',
            params: { id: item.id }
         });
      },
      Delete(item) {
         this.deleteLoading = true;
         ContractorCategoryCriterionService.Delete(item.id)
            .then(() => {
               this.makeToast(this.$t('DeleteSuccess'), 'success');
               this.refresh();
            })
            .catch((err) => {
               this.showApiError(err);
            })
            .finally(() => {
               this.deleteLoading = false;
            });
      },
      Accept(item) {
         this.acceptLoading = true;
         ContractorCategoryCriterionService.Accept({
            statusId: item.statusId,
            id: item.id
         })
            .then(() => {
               this.makeToast(this.$t('AcceptSuccess'), 'success');
               this.refresh();
            })
            .catch((err) => {
               this.showApiError(err);
            })
            .finally(() => {
               this.acceptLoading = false;
            });
      },
      CancelApproval(item) {
         this.cancelLoading = true;
         ContractorCategoryCriterionService.Cancel({
            statusId: item.statusId,
            id: item.id
         })
            .then(() => {
               this.makeToast(this.$t('CancelSuccess'), 'success');
               this.refresh();
            })
            .catch((err) => {
               this.showApiError(err);
            })
            .finally(() => {
               this.cancelLoading = false;
            });
      },
      refresh() {
         this.isBusy = true;
         ContractorCategoryCriterionService.GetList(this.filter)
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
