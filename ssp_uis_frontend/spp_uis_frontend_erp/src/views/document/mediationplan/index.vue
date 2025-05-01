<template>
   <form-table-hrm
      :items="items"
      :actions="{}"
      :fields="fields"
      :filter.sync="filter"
      searchable
      :busy="isBusy"
      :delete-loading="deleteLoading"
      @row-dblclicked="DbClick"
      @request="refresh"
      @row-delete="Delete"
   >
      <template #filter>
         <b-row>
            <b-col cols="12" md="5">
               <StatusSelect v-model="filter.statusId" @input="refresh" :filter="[1, 2, 3]" />
            </b-col>
            <b-col cols="12" md="3">
               <v-select
                  :reduce="(item) => item.value"
                  :options="BankList"
                  placeholder="Bank"
                  v-model="filter.contractorId"
                  @input="refresh"
                  label="text"
               />
            </b-col>
            <b-col cols="12" sm="12" md="4" class="d-flex justify-content-end">
               <b-input-group>
                  <b-form-input v-model="filter.search" @keyup.enter="refresh" :placeholder="$t('search')" />
                  <b-input-group-append>
                     <b-button @click="refresh" variant="primary">
                        <feather-icon icon="SearchIcon" />
                     </b-button>
                  </b-input-group-append>
               </b-input-group>
            </b-col>
         </b-row>
      </template>
      <template #cell(actions)="{ item }">
         <div class="text-center" style="text-wrap: nowrap">
            <!-- View -->
            <b-link
               v-if="$can('MediationView', 'permissions')"
               :to="{ name: 'ViewMediationPlan', params: { id: item.id } }"
               v-b-tooltip.hover.top="$t('View')"
               class="mr-1 cursor-pointer"
            >
               <feather-icon icon="EyeIcon"></feather-icon>
            </b-link>
            <!-- edit -->
            <b-link
               v-if="$can('MediationPlanEdit', 'permissions') && item.canEdit"
               :to="{ name: 'EditMediationPlan', params: { id: item.id } }"
               v-b-tooltip.hover.top="$t('Edit')"
               class="mr-1 cursor-pointer"
            >
               <feather-icon icon="EditIcon"></feather-icon>
            </b-link>
            <!-- createMediation -->
            <b-link
               v-if="$can('MediationPlanEdit', 'permissions') && item.canCreateMediation"
               :to="{ name: 'EditMediation', params: { id: 0 }, query: { mediationPlanId: item.id } }"
               v-b-tooltip.hover.top="$t('CreateMediation')"
               class="mr-1 cursor-pointer"
            >
               <feather-icon icon="FileIcon"></feather-icon>
            </b-link>
            <!-- delete -->
            <b-link
               v-if="$can('MediationPlanDelete', 'permissions') && item.canDelete"
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
               v-if="$can('MediationPlanAccept', 'permissions') && item.canAccept"
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
            <!--  -->
            <template>
               <b-link
                  v-if="$can('MediationPlanCancel', 'permissions') && item.canCancel"
                  class="text-danger cursor-pointer mr-1"
                  v-b-tooltip.hover.top="$t('Cancel')"
                  @click="Cancel(item)"
               >
                  <feather-icon icon="XCircleIcon"></feather-icon>
               </b-link>
            </template>
         </div>
      </template>
      <template #cell(status)="{ item }">
         <b-badge :variant="getColor(item)">{{ item.status }}</b-badge>
      </template>
   </form-table-hrm>
</template>

<script>
import {
   BCard,
   BRow,
   BCol,
   BCardText,
   VBTooltip,
   BBadge,
   BButton,
   BLink,
   BModal,
   VBModal,
   BInputGroup,
   BFormInput,
   BInputGroupAppend
} from 'bootstrap-vue';
import ManualService from '@/services/others/manual.service';
import FormTableHrm from '@/components/forms/form-table-hrm.vue';
import MediationPlanService from '@/services/document/mediationplan.service';
import StatusSelect from '@/views/components/document/StatusSelect.vue';
export default {
   components: {
      BRow,
      BCol,
      StatusSelect,
      BInputGroup,
      BFormInput,
      BInputGroupAppend,
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
         BankList: [],
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
               key: 'contractor',
               label: this.$t('contractor')
            },
            {
               key: 'mediationType',
               label: this.$t('mediationType')
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
            statusId: 1,
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
   created() {
      let localStorageData = localStorage.getItem('user_info');
      localStorageData = JSON.parse(localStorageData);
      this.localStorageData = localStorageData;
      if (this.localStorageData.positionCategoryId == 1 || this.localStorageData.id == 1) {
         this.filter.isEmployee = false;
      } else {
         this.filter.isEmployee = true;
      }
      ManualService.ContractorSelectList().then((res) => {
         this.BankList = res.data;
      });
   },
   methods: {
      DbClick(item) {
         this.$router.push({
            name: 'EditMediationPlan',
            params: { id: item.id }
         });
      },
      Delete(item) {
         this.deleteLoading = true;
         MediationPlanService.Delete(item.id)
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
         MediationPlanService.Accept({
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

      Cancel(item) {
         this.$swal.fire({
            icon: 'question',
            title: this.$t('WantCancel'),
            input: 'textarea',
            inputPlaceholder: this.$t('Cancel'),
            showLoaderOnConfirm: true,
            preConfirm: (msg) => {
               return MediationPlanService.Cancel({
                  statusId: item.statusId,
                  id: item.id,
                  message: msg
               })
                  .then(() => {
                     this.makeToast(this.$t('CancelMessage'), 'success');
                  })
                  .catch(this.SwalError);
            },
            allowOutsideClick: () => !this.$swal.isLoading()
         });
      },

      refresh() {
         this.isBusy = true;
         MediationPlanService.GetList(this.filter)
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
