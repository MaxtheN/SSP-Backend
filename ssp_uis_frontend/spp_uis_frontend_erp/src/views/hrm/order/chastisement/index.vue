<template>
   <div>
      <form-table-hrm
         :items="items"
         :fields="fields"
         :filter.sync="filter"
         searchable
         :busy="isBusy"
         :actions="{
            create: {
               name: 'EditChastisement',
               permission: 'ChastisementCreate'
            },
            edit: {
               name: 'EditChastisement',
               permission: 'ChastisementEdit'
            },
            delete: {
               name: 'EditChastisement',
               permission: 'ChastisementDelete'
            }
         }"
         @request="Refresh"
      >
         <template #status>
            <b-row>
               <b-col>
                  <status-select
                     class="mt-1"
                     v-model="filter.statusId"
                     @input="ChangeStatus"
                     :filter="[1, 2, 3, 4, 27, 24]"
                  />
               </b-col> </b-row
         ></template>
         <template #cell(actions)="{ item }">
            <div class="text-center" style="text-wrap: nowrap">
               <!-- edit -->
               <b-link
                  v-if="$can('ChastisementEdit', 'permissions')"
                  :to="{
                     name: 'EditChastisement',
                     params: { id: item.id }
                  }"
                  v-b-tooltip.hover.top="$t('Edit')"
                  class="mr-1 cursor-pointer"
               >
                  <feather-icon icon="EditIcon"></feather-icon>
               </b-link>
               <!-- view -->
               <b-link
                  :to="{ name: 'ViewChastisement', params: { id: item.id } }"
                  v-b-tooltip.hover.top="$t('View')"
                  v-if="$can('ChastisementView', 'permissions') || $can('ChastisementSignerView', 'permissions')"
                  class="mr-1 cursor-pointer"
               >
                  <feather-icon icon="EyeIcon"></feather-icon>
               </b-link>
               <!-- delete -->
               <b-link
                  v-if="$can('ChastisementDelete', 'permissions') && item.canDelete"
                  class="text-danger mr-1 cursor-pointer"
                  @click="Delete(item)"
                  v-b-tooltip.hover.top="$t('Delete')"
               >
                  <feather-icon icon="TrashIcon"></feather-icon>
               </b-link>
               <!-- Sign -->
               <b-link
                  v-if="$can('ChastisementSign', 'permissions') && (item.canSign || item.signEmployee)"
                  @click="OpenSign(item, 'Sign')"
                  class="mr-1 text-success cursor-pointer"
                  v-b-tooltip.hover.top="$t('Approve')"
               >
                  <feather-icon icon="CheckCircleIcon"></feather-icon>
               </b-link>
               <!-- Cancel -->
               <template v-if="$can('ChastisementCancel', 'permissions') && item.canCancel">
                  <b-link
                     class="text-danger cursor-pointer mr-1"
                     v-b-tooltip.hover.top="$t('Cancel')"
                     @click="Cancel(item)"
                  >
                     <feather-icon icon="XCircleIcon"></feather-icon>
                  </b-link>
               </template>

               <!-- document history -->
               <HistoryModalButton :id="item.id" :table-id="item.tableId || 136" />
            </div>
         </template>
      </form-table-hrm>

      <!-- sign dialog -->
      <b-modal v-model="EImzoModal" size="md" :title="$t('enterEImzo')" hide-footer>
         <b-card-text v-if="selectedItem">
            <just-sign :data-to-sign="selectedItem" v-if="!SignLoading" @sign="loginESP($event)" />
            <div style="height: 400px" v-if="SignLoading" class="d-flex justify-content-center align-items-center">
               <b-spinner label="Spinning"></b-spinner>
            </div>
         </b-card-text>
      </b-modal>
   </div>
</template>

<script>
import { BCard, VBTooltip, BModal, VBModal, BLink, BCardText, BSpinner, BRow, BCol } from 'bootstrap-vue';
import FormTableHrm from '@/components/forms/form-table-hrm.vue';
import StatusSelect from '@/views/components/document/StatusSelect.vue';
import ChastisementService from '@/services/hrm/chastisement.service';
import eimzoMixin from '@/mixins/eimzo';
import justSign from '@/components/justSign.vue';
import HistoryModalButton from '@/views/components/document/HistoryModalButton.vue';
import AccountService from '@/services/others/account.service';
import Cookies from 'js-cookie';

const DefaultFilter = {
   organizationId: null,
   regionId: null,
   // isInitQuery: true,
   search: '',
   sortBy: '',
   orderType: 'asc',
   page: 1,
   pageSize: 20,
   perPageOptions: [10, 20, 50, 100],
   total: 0
};

export default {
   components: {
      BCard,
      BRow,
      BCol,
      BLink,
      BModal,
      BCardText,
      BSpinner,
      StatusSelect,
      VBModal,
      FormTableHrm,
      justSign,
      HistoryModalButton
   },
   directives: {
      'b-tooltip': VBTooltip,
      'b-modal': VBModal
   },
   mixins: [eimzoMixin],
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
               label: this.$t('ondate'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'details',
               label: this.$t('details')
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
            statusId: null,
            statusIds: [],
            orderType: 'asc',
            page: 1,
            pageSize: 20,
            perPageOptions: [10, 20, 50, 100],
            total: 0
         },
         isBusy: false,
         selectedItem: null,
         EImzoModal: false,
         SignLoading: false,
         activeSignAction: null
      };
   },

   created() {
      if (JSON.parse(localStorage.getItem('filterData'))) {
         this.filter = JSON.parse(localStorage.getItem('filterData'));
      } else {
         this.filter = DefaultFilter;
      }
   },
   watch: {
      '$route.name': function () {
         this.Refresh();
      },

      filter: {
         handler(newValue, oldValue) {
            localStorage.setItem('filterData', JSON.stringify(newValue));
            this.filter = newValue;
            if (newValue.districtId) {
               this.GetDistrict();
            }
         },
         deep: true
      }
   },
   methods: {
      ChangeStatus() {
         if (this.filter.statusId == null) {
            this.filter.statusIds = [];
            this.Refresh();
         } else {
            this.filter.statusIds[0] = this.filter.statusId;
            this.Refresh();
         }
      },
      Refresh() {
         this.isBusy = true;
         if (this.$route.name == 'ChastisementForSigner') {
            ChastisementService.GetListForSigner(this.filter)
               .then((res) => {
                  this.items = res.data.rows;
                  this.filter.total = res.data.total;
               })
               .finally(() => {
                  this.isBusy = false;
               });
         } else {
            ChastisementService.GetList(this.filter)
               .then((res) => {
                  this.items = res.data.rows;
                  this.filter.total = res.data.total;
               })
               .finally(() => {
                  this.isBusy = false;
               });
         }
      },
      Delete(item) {
         this.$swal.fire({
            icon: 'info',
            title: this.$t('WantDelete'),
            showLoaderOnConfirm: true,
            preConfirm: () => {
               return ChastisementService.Delete(item.id)
                  .then(() => {
                     this.makeToast(this.$t('DeleteSuccess'), 'success');
                     this.Refresh();
                  })
                  .catch(this.SwalError);
            },
            allowOutsideClick: () => !this.$swal.isLoading()
         });
      },
      loginESP(item) {
         const isPinfl = this.isPinfl(item);
         if (this.activeSignAction == 'Sign') {
            this.Sign(item.key);
         }
      },
      OpenSign(item, type) {
         this.activeSignAction = type;
         this.EImzoModal = true;
         this.selectedItem = item;
      },
      logout() {
         // Remove userData from localStorage
         Cookies.remove('auth_token');

         localStorage.clear();
         // Reset ability
         this.$ability.update(initialAbility);

         // Redirect to login page
         this.$router.push({ name: 'auth-login' });
      },
      Sign(signedData, isPinfl) {
         this.SignLoading = true;

         ChastisementService.Sign({
            id: this.selectedItem.id,
            signedData: signedData,
            isPinfl: isPinfl
         })
            .then(() => {
               this.makeToast(this.$t('AcceptSuccess'), 'success');
               this.Refresh();
               this.logout();
               this.EImzoModal = false;
            })
            .catch(this.showApiError)
            .finally(() => {
               this.SignLoading = false;
            });
      },
      Cancel(item) {
         this.$swal.fire({
            icon: 'question',
            title: this.$t('WantCancel'),
            showLoaderOnConfirm: true,
            input: 'text',
            inputPlaceholder: this.$t('RejectMessage'),
            preConfirm: (msg) => {
               return ChastisementService.Cancel({
                  id: item.id,
                  message: msg,
                  signedData: 'cancel'
               })
                  .then(() => {
                     this.makeToast(this.$t('CancelMessage'), 'success');
                     this.Refresh();
                  })
                  .catch(this.SwalError);
            },
            allowOutsideClick: () => !this.$swal.isLoading()
         });
      }
   }
};
</script>
