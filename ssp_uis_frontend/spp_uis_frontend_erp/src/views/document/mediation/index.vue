b-col
<template>
   <div>
      <form-table-hrm
         :items="items"
         :actions="{
            edit: {
               name: 'EditMediation',
               permission: 'MediationEdit'
            }
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
         <template #filter>
            <b-row>
               <b-col cols="12" md="5">
                  <StatusSelect v-model="filter.statusId" @input="refresh" :filter="[1, 2, 25]" />
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
               <b-col cols="12" md="4" sm="12" class="d-flex justify-content-end">
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
                  :to="{ name: 'ViewMediation', params: { id: item.id } }"
                  v-b-tooltip.hover.top="$t('View')"
                  class="mr-1 cursor-pointer"
               >
                  <feather-icon icon="EyeIcon"></feather-icon>
               </b-link>
               <!-- edit -->
               <b-link
                  v-if="$can('MediationEdit', 'permissions') && item.canEdit"
                  :to="{ name: 'EditMediation', params: { id: item.id } }"
                  v-b-tooltip.hover.top="$t('Edit')"
                  class="mr-1 cursor-pointer"
               >
                  <feather-icon icon="EditIcon"></feather-icon>
               </b-link>
               <!-- canCreateApplicationForCourt -->
               <b-link
                  v-if="$can('ApplicationForCourtEdit', 'permissions') && item.canCreateApplicationForCourt"
                  :to="{ name: 'EditApplicationForCourt', params: { id: 0 }, query: { mediationId: item.id } }"
                  v-b-tooltip.hover.top="$t('CreateApplicationForCourt')"
                  class="mr-1 cursor-pointer"
               >
                  <feather-icon icon="FileIcon"></feather-icon>
               </b-link>
               <!-- delete -->
               <b-link
                  v-if="$can('MediationDelete', 'permissions') && item.canDelete"
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
                  v-if="$can('MediationAccept', 'permissions') && item.canAccept"
                  @click="openSelectModal(item)"
                  class="mr-1 text-success cursor-pointer"
                  v-b-tooltip.hover.top="$t('Approve')"
               >
                  <feather-icon icon="CheckCircleIcon"></feather-icon>
               </b-link>

               <!-- cancel -->
               <template v-if="$can('MediationCancel', 'permissions') && item.canCancel">
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

      <!-- accept dialog -->
      <validation-observer ref="ValidationAcceptDTO">
         <b-modal v-model="acceptDialog" no-close-on-backdrop size="lg">
            <template #modal-title>
               {{ $t('Approve') }}
            </template>
            <b-card-text v-if="selectedItem">
               <h5>{{ $t('WantApprove') }}</h5>

               <template v-if="selectedItem.mediationResultId == 3">
                  <b-row>
                     <b-col sm="12" md="12">
                        <form-select
                           v-model="acceptData.meetingTypeId"
                           @change="changemeetingTypeId"
                           :options="MeetingTypeSelectList"
                           required-star
                           :label="$t('meetingType')"
                           :name="$t('meetingType')"
                        />
                     </b-col>
                     <b-col sm="12" md="6">
                        <form-picker v-model="acceptData.docOn" :label="$t('docOn')" :name="$t('docOn')" required />
                     </b-col>
                     <b-col sm="12" md="6">
                        <form-picker
                           v-model="acceptData.meditionAt"
                           type="datetime"
                           format="DD.MM.YYYY HH:mm:ss"
                           :label="$t('meditionAt')"
                           :name="$t('meditionAt')"
                           required
                        />
                     </b-col>
                     <!-- zoom link -->
                     <b-col cols="12" v-if="acceptData.meetingTypeId == 1">
                        <form-input-hrm
                           v-model="acceptData.addressOrUrl"
                           :rules="'required|max:250'"
                           :label="$t('addressOrUrl')"
                           :name="$t('addressOrUrl')"
                        />
                     </b-col>
                     <b-col cols="12" v-if="acceptData.meetingTypeId == 2">
                        <form-input-hrm
                           v-model="acceptData.addressOrUrl"
                           :rules="'required|max:250'"
                           :label="$t('addressOrUrlOffline')"
                           :name="$t('addressOrUrl')"
                        />
                     </b-col>
                     <b-col sm="12" md="12">
                        <form-input-hrm
                           rules="required"
                           v-model="acceptData.chamberPerson"
                           :label="$t('chamberPerson')"
                           :name="$t('chamberPerson')"
                        />
                     </b-col>
                  </b-row>
               </template>
            </b-card-text>
            <template #modal-footer="{ cancel }">
               <b-button variant="danger" @click="cancel()">{{ $t('Cancel') }}</b-button>
               <b-button variant="success" :disabled="acceptLoading" @click="Accept">
                  <b-spinner v-if="acceptLoading" small></b-spinner>
                  {{ $t('Approve') }}</b-button
               >
            </template>
         </b-modal>
      </validation-observer>
   </div>
</template>

<script>
import {
   BInputGroup,
   BFormInput,
   BInputGroupAppend,
   BCard,
   BCardText,
   VBTooltip,
   BBadge,
   BButton,
   BLink,
   BModal,
   VBModal,
   BRow,
   BCol,
   BSpinner
} from 'bootstrap-vue';
import FormTableHrm from '@/components/forms/form-table-hrm.vue';
import MediationService from '@/services/document/mediation.service';
import ManualService from '@/services/others/manual.service';
import StatusSelect from '@/views/components/document/StatusSelect.vue';

const acceptDataDef = {
   id: null,
   docOn: '',
   meetingTypeId: 0,
   addressOrUrl: '',
   meditionAt: '',
   chamberPerson: ''
};

export default {
   components: {
      BInputGroup,
      BFormInput,
      BInputGroupAppend,
      StatusSelect,
      BCard,
      FormTableHrm,
      BCardText,
      BButton,
      BBadge,
      BLink,
      BModal,
      BRow,
      BCol,
      BSpinner
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
               key: 'mediationPlanId',
               label: this.$t('MediationPlan')
            },
            {
               key: 'mediationResult',
               label: this.$t('mediationResult')
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
            statusId: 1,
            sortBy: '',
            orderType: 'asc',
            page: 1,
            pageSize: 20,
            perPageOptions: [10, 20, 50, 100, 300],
            total: 0
         },
         acceptData: { ...acceptDataDef },
         MeetingTypeSelectList: [],
         acceptDialog: false,
         selectedItem: {},
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
      ManualService.MeetingTypeSelectList().then((res) => {
         if (Array.isArray(res.data)) {
            this.MeetingTypeSelectList = res.data;
         }
      });
   },
   methods: {
      DbClick(item) {
         this.$router.push({
            name: 'EditMediation',
            params: { id: item.id }
         });
      },
      openSelectModal(item) {
         this.selectedItem = item;
         this.acceptData.id = item.id;
         this.acceptDialog = true;
      },
      changemeetingTypeId() {
         this.acceptData.addressOrUrl = '';
      },
      Delete(item) {
         this.deleteLoading = true;
         MediationService.Delete(item.id)
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
      Accept() {
         this.$refs.ValidationAcceptDTO.validate().then((success) => {
            if (success) {
               this.acceptLoading = true;
               MediationService.Accept(this.acceptData)
                  .then(() => {
                     this.makeToast(this.$t('AcceptSuccess'), 'success');
                     this.refresh();
                     this.acceptData = { ...acceptDataDef };
                     this.selectedItem = {};
                     this.acceptDialog = false;
                  })
                  .catch((err) => {
                     this.showApiError(err);
                  })
                  .finally(() => {
                     this.acceptLoading = false;
                  });
            }
         });
      },
      CancelApproval(item) {
         this.cancelLoading = true;
         MediationService.Cancel(item.id)
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
         MediationService.GetList(this.filter)
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
