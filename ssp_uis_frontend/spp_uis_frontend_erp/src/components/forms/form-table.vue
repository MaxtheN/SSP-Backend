<template>
   <b-card no-body>
      <b-table-simple
         responsive
         primary-key="id"
         sticky-header="65vh"
         no-border-collapse
         hover
         small
         striped
         :busy="isBusy"
         show-empty
         :empty-text="$t('NotFound')"
         class="position-relative"
         :sort-by.sync="filter.sortBy"
         :sort-desc.sync="filter.orderType"
         @row-dblclicked="Edit"
         @row-clicked="rowclicked"
      >
         <b-thead>
            <b-tr>
               <b-th v-if="actions" style="position: sticky; left: 0; z-index: 100">
                  <div>{{ $t('actions') }}</div>
               </b-th>
               <b-th
                  v-for="(field, indexField) in fields"
                  :key="indexField"
                  @click="SortChange(field)"
                  :style="field.sortable ? 'cursor:pointer;white-space:nowrap' : 'white-space:nowrap'"
               >
                  <div style="display: flex; align-items: center">
                     <span>{{ $t(`${field.label}`) }}</span>
                     <span v-if="field.sortable" class="ml-1" style="display: flex; flex-direction: column">
                        <feather-icon
                           :style="
                              filter.orderType == 'asc' && filter.sortBy == field.key
                                 ? 'color:#9F9BAD'
                                 : 'color:#DBD9E1'
                           "
                           style="margin-bottom: -3px"
                           icon="ChevronUpIcon"
                        ></feather-icon>
                        <feather-icon
                           :style="
                              filter.orderType == 'desc' && filter.sortBy == field.key
                                 ? 'color:#9F9BAD'
                                 : 'color:#DBD9E1'
                           "
                           icon="ChevronDownIcon"
                        ></feather-icon>
                     </span>
                  </div>
               </b-th>
            </b-tr>
         </b-thead>
         <b-tbody v-if="isBusy">
            <b-tr class="text-center">
               <b-td :colspan="fields.length + 1">
                  <b-spinner></b-spinner>
               </b-td>
            </b-tr>
         </b-tbody>
         <b-tbody v-if="data.length == 0">
            <b-tr class="text-center">
               <b-td :colspan="fields.length + 1">{{ $t('NotFound') }}</b-td>
            </b-tr>
         </b-tbody>
         <b-tbody v-if="!isBusy">
            <b-tr v-for="(item, index) in data" :key="index">
               <b-td v-if="actions" style="position: sticky; left: 0; background-color: white">
                  <div class="text-center" style="text-wrap: nowrap">
                     <b-link
                        v-if="$can(`${permission}View`, 'permissions') && item.canModify && actions.edit"
                        :to="{ name: `Edit${router}`, params: { id: item.id } }"
                        v-b-tooltip.hover.top="$t('View')"
                        style="margin-right: 5px"
                     >
                        <feather-icon icon="EditIcon"></feather-icon>
                     </b-link>
                     <b-link
                        v-if="actions.view"
                        :to="{ name: `Edit${router}`, params: { id: item.id } }"
                        v-b-tooltip.hover.top="$t('View')"
                        style="margin-right: 5px"
                     >
                        <feather-icon icon="EyeIcon"></feather-icon>
                     </b-link>
                     <b-link
                        v-if="$can(`${permission}Delete`, 'permissions') && item.canDelete && actions.delete"
                        @click="OpenDeleteModal(item)"
                        v-b-tooltip.hover.top="$t('Delete')"
                        style="margin-right: 5px"
                     >
                        <feather-icon icon="TrashIcon"></feather-icon>
                     </b-link>
                     <b-link
                        v-if="actions.history"
                        @click="OpenHistory(item)"
                        style="margin-right: 5px"
                        v-b-tooltip.hover.top="$t('history')"
                     >
                        <b-icon-clock-history></b-icon-clock-history>
                     </b-link>
                     <b-link
                        v-if="
                           actions.mfyApplication && (item.statusId == 2 || item.statusId == 25 || item.statusId == 14)
                        "
                        @click="GetMfyApplication(item)"
                        style="margin-right: 5px"
                        v-b-tooltip.hover.top="$t('mfyApplication')"
                     >
                        <feather-icon icon="FolderIcon"></feather-icon>
                     </b-link>
                  </div>
               </b-td>
               <b-td v-for="(field, indexField) in fields" :key="indexField">
                  <div
                     v-if="
                        field.key != 'status' &&
                        field.key != 'prtnContractStatus' &&
                        field.key != 'oked' &&
                        field.key != 'chooseLocation'
                     "
                  >
                     {{ $t(`${item[field.key] ? item[field.key] : ''}`) }}
                  </div>

                  <div v-if="field.key == 'oked'">{{ item.okedCode }} - {{ item[field.key] }}</div>
                  <b-badge v-if="field.key == 'status'" :variant="getColor(item)">{{ item[field.key] }}</b-badge>

                  <b-badge v-if="field.key == 'prtnContractStatus'" :variant="getColorStatus(item)">{{
                     item[field.key]
                  }}</b-badge>

                  <div v-if="field.key == 'chooseLocation'">
                     <b-badge v-if="item[field.key]" variant="success">
                        {{ $t('yes') }}
                     </b-badge>

                     <b-badge v-else variant="danger">{{ $t('no') }}</b-badge>
                  </div>
               </b-td>
            </b-tr>
         </b-tbody>
      </b-table-simple>
      <b-modal v-model="DeleteModal" hide-footer>
         <template #modal-title>
            {{ $t('Delete') }}
            <b-spinner v-if="DeleteLoading" small></b-spinner>
         </template>
         <b-card-text>
            <h5>ID : {{ selectItem.id }}</h5>
            <h5>{{ $t('WantDelete') }}</h5>
         </b-card-text>
         <b-row>
            <b-col>
               <div class="d-flex justify-content-end mt-1">
                  <b-button class="mr-1" @click="DeleteModal != DeleteModal" size="xl" variant="danger">{{
                     $t('no')
                  }}</b-button>
                  <b-button @click="Delete" size="xl" variant="success">
                     {{ $t('yes') }}
                  </b-button>
               </div>
            </b-col>
         </b-row>
      </b-modal>
      <b-modal size="xl" v-model="historyModal" hide-footer :title="$t('history')">
         <b-table
            :items="historyData"
            :fields="historyFields"
            responsive
            no-border-collapse
            :busy="isBusy"
            show-empty
            :empty-text="$t('NotFound')"
         >
            <template #cell(status)="{ item }">
               <b-badge :variant="getColor(item)">{{ item.status }}</b-badge>
            </template>
         </b-table>
      </b-modal>
      <b-modal size="xl" v-model="mfyApplication" hide-footer :title="$t('mfyApplication')">
         <b-row style="height: 600px">
            <b-col md="6" sm="6" lg="6">
               <b-row class="mt-5 ml-2">
                  <b-col md="12" sm="12" lg="12" class="mb-1">
                     <label style="font-weight: bold">{{ $t('dateofcreated') }}:</label>
                     <span>{{ MfyApplicationData.createdAt }}</span>
                  </b-col>
                  <b-col md="12" sm="12" lg="12" class="mb-1">
                     <label style="font-weight: bold">{{ $t('conclusingPersonFio') }}:</label>
                     <span>{{ MfyApplicationData.conclusingPersonFio }}</span>
                  </b-col>

                  <b-col md="12" sm="12" lg="12" class="mb-1">
                     <label style="font-weight: bold">{{ $t('conclusingPersonPhone') }}:</label>
                     <span>{{ MfyApplicationData.conclusingPersonPhone }}</span>
                  </b-col>

                  <b-col md="12" sm="12" lg="12" class="mb-1">
                     <label style="font-weight: bold">{{ $t('detailinfo') }}:</label>
                     <span>{{ MfyApplicationData.details }}</span>
                  </b-col>
                  <b-col md="12" sm="12" lg="12" class="mb-1">
                     <label style="font-weight: bold">{{ $t('isAccepted') }}:</label>
                     <b-badge :variant="MfyApplicationData.isAccepted ? 'success' : 'danger'">
                        {{ MfyApplicationData.isAccepted ? $t('yes') : $t('no') }}
                     </b-badge>
                  </b-col>
               </b-row>
            </b-col>
            <b-col md="6" sm="6" lg="6">
               <iframe :src="MfyApplicationData.fileUrl" width="100%" height="100%" frameborder="0"></iframe>
            </b-col>
         </b-row>
      </b-modal>

      <div class="mx-2 mb-2">
         <b-row>
            <b-col cols="12" sm="6" class="d-flex align-items-center justify-content-center justify-content-sm-start">
               <span class="text-muted">
                  {{ $t('Showing') }} {{ firstNumber }} {{ $t('to') }} {{ lastNumber }} {{ $t('of') }}
                  {{ filter.totalRows }}
                  {{ $t('entries') }}
               </span>
               <v-select
                  v-model="filter.pageSize"
                  :dir="$store.state.appConfig.isRTL ? 'rtl' : 'ltr'"
                  :options="filter.pageOptions"
                  :clearable="false"
                  @input="refresh"
                  class="per-page-selector d-inline-block ml-50 mr-1"
               />
            </b-col>
            <!-- Pagination -->
            <b-col cols="12" sm="6" class="d-flex align-items-center justify-content-center justify-content-sm-end">
               <b-pagination
                  v-model="filter.page"
                  :total-rows="filter.totalRows"
                  :per-page="filter.pageSize"
                  first-number
                  last-number
                  @input="refresh"
                  class="mb-0 mt-1 mt-sm-0"
                  prev-class="prev-item"
                  next-class="next-item"
               >
                  <template #prev-text>
                     <feather-icon icon="ChevronLeftIcon" size="18" />
                  </template>
                  <template #next-text>
                     <feather-icon icon="ChevronRightIcon" size="18" />
                  </template>
               </b-pagination>
            </b-col>
         </b-row>
      </div>
   </b-card>
</template>

<script>
import {
   BTableSimple,
   BTr,
   BTd,
   BTh,
   BTbody,
   BTfoot,
   BThead,
   BButton,
   BPagination,
   BTable,
   BCol,
   VBTooltip,
   VBModal,
   BRow,
   BSpinner,
   BCard,
   BTooltip,
   BBadge,
   BInputGroup,
   BFormInput,
   BIconClockHistory,
   BInputGroupAppend,
   BLink,
   BModal,
   BCardText,
   BButtonGroup,
   BIcon
} from 'bootstrap-vue';
import ManualService from '@/services/others/manual.service';
import ApplicationService from '@/services/document/application.service';
export default {
   props: {
      // data: {
      //   type: Array,
      //   default: null,
      // },
      fields: {
         type: Array,
         default: null
      },
      filter: {
         type: Object,
         default: null
      },
      service: {
         type: Object,
         default: null
      },
      router: {
         type: String,
         default: ''
      },
      permission: {
         type: String,
         default: ''
      },
      actions: {
         type: Object,
         default: null
      }
   },
   components: {
      BTableSimple,
      BTr,
      BTd,
      BTh,
      BTbody,
      BTfoot,
      BThead,
      BButton,
      BPagination,
      BTable,
      BCol,
      BIconClockHistory,
      BRow,
      BSpinner,
      BCard,
      BTooltip,
      BBadge,
      BInputGroup,
      BFormInput,
      BInputGroupAppend,
      BLink,
      BModal,
      BCardText,
      BButtonGroup,
      BIcon
   },
   directives: {
      'b-tooltip': VBTooltip,
      'b-modal': VBModal
   },
   data() {
      return {
         data: [],
         historyFields: [
            {
               key: 'id',
               label: this.$t('id'),
               sortable: true,
               tdClass: 'text-center',
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
         ],

         statusId: 0,
         historyModal: false,
         mfyApplication: false,
         historyData: [],
         MfyApplicationData: {},
         DeleteModal: false,
         selectItem: {},
         isBusy: false,
         DeleteLoading: false,
         lang: ''
      };
   },

   computed: {
      firstNumber() {
         return (this.filter.page - 1) * this.filter.pageSize + 1;
      },
      lastNumber() {
         if (this.filter.totalRows < this.filter.pageSize) {
            return this.filter.totalRows;
         } else {
            if (this.filter.page * this.filter.pageSize > this.filter.totalRows) {
               return this.filter.totalRows;
            } else {
               return this.filter.page * this.filter.pageSize;
            }
         }
      }
   },
   created() {
      this.lang = localStorage.getItem('locale') || 'ru';
      this.refresh();
   },
   methods: {
      GetMfyApplication(item) {
         this.mfyApplication = true;
         ApplicationService.GetMfyApplication(item.id2)
            .then((res) => {
               this.MfyApplicationData = res.data;
            })
            .catch((error) => {
               this.showApiError(error);
            });
      },
      OpenHistory(item) {
         this.historyModal = true;
         ManualService.GetListByDocumentId(item.tableId, item.id)
            .then((res) => {
               this.historyData = res.data;
            })
            .catch((error) => {
               this.showApiError(error);
            });
      },

      rowclicked() {},
      SortChange(data) {
         if (data.sortable) {
            this.filter.sortBy = data.key;
            this.filter.orderType = this.filter.orderType == 'asc' ? 'desc' : 'asc';
            this.refresh();
         }
      },

      Edit(item) {
         this.$emit('Edit', item);
         //   this.$router.push({
         //     name: "EditApplication",
         //     params: {
         //       id: item.id,
         //     },
         //   });
      },
      Create() {
         this.$emit('Create');
         //   this.$router.push({
         //     name: "EditApplication",
         //     params: {
         //       id: 0,
         //     },
         //   });
      },
      OpenDeleteModal(item) {
         this.selectItem = item;
         this.DeleteModal = true;
      },
      Delete() {
         var self = this;
         this.service
            .Delete(this.selectItem.id)
            .then((res) => {
               self.makeToast(self.$t('DeleteMessage'), 'success');
               this.refresh();
            })
            .catch((error) => {
               self.makeToast(error.response.data.errors, 'danger');
            })
            .finally(() => {
               this.DeleteModal = false;
            });
      },

      makeToast(message, variant) {
         this.$toast({
            component: ToastificationContent,
            props: {
               title: message,
               icon: variant == 'success' ? 'CheckSquareIcon' : 'AlertTriangleIcon',
               variant: variant
            }
         });
      },

      refresh() {
         this.isBusy = true;
         // console.log(this.filter)
         if (this.filter.statusId == 0) {
            this.filter.statusIds = [];
         } else {
            this.filter.statusIds = [this.filter.statusId];
         }
         this.service
            .GetList(this.filter)
            .then((res) => {
               this.data = res.data.rows;
               this.filter.totalRows = res.data.total;
               this.isBusy = false;
            })
            .catch((error) => {
               this.isBusy = false;
               this.makeToast(error.response.data, 'danger');
            });
      }
   }
};
</script>

<style lang="scss" scoped>
.per-page-selector {
   width: 90px;
}

.invoice-filter-select {
   min-width: 190px;

   ::v-deep .vs__selected-options {
      flex-wrap: nowrap;
   }

   ::v-deep .vs__selected {
      width: 100px;
   }
}
</style>

<style lang="scss">
@import '@core/scss/vue/libs/vue-select.scss';
</style>
