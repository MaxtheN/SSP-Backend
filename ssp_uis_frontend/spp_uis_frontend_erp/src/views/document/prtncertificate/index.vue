<template>
   <b-card no-body>
      <div class="m-2">
         <b-row align-h="between">
            <b-col sm="12" md="2">
               <div>
                  <form-select
                     :options="RegionList"
                     v-model="filter.regionId"
                     @input="ChangeRegion"
                     :label="$t('Oblast')"
                  ></form-select>
               </div>
            </b-col>
            <b-col sm="12" md="2">
               <div>
                  <label for>{{ $t('Region') }}</label>
                  <v-select
                     :options="DistrictList"
                     :reduce="(item) => item.value"
                     :placeholder="$t('ChooseBelow')"
                     label="text"
                     v-model="filter.districtId"
                     @input="ChangeDistrict"
                     class="w-100"
                  ></v-select>
               </div>
            </b-col>
            <b-col sm="12" md="2">
               <div>
                  <label for>{{ $t('mfy') }}</label>
                  <v-select
                     :options="MfyList"
                     :reduce="(item) => item.value"
                     :placeholder="$t('ChooseBelow')"
                     label="text"
                     v-model="filter.mfyId"
                     @input="Refresh"
                     class="w-100"
                  ></v-select>
               </div>
            </b-col>
            <b-col sm="12" md="2">
               <div>
                  <label for>{{ $t('prtnContractType') }}</label>
                  <v-select
                     :options="ContractTypeList"
                     :reduce="(item) => item.value"
                     :placeholder="$t('ChooseBelow')"
                     label="text"
                     v-model="filter.prtnContractTypeId"
                     class="w-100"
                  ></v-select>
               </div>
            </b-col>
            <b-col sm="12" md="2">
               <div>
                  <form-input v-model="filter.okedCode" :label="$t('Oked')" mask="#####" />
               </div>
            </b-col>
            <b-col sm="12" md="2">
               <div>
                  <label for>{{ $t('inn') }}</label>
                  <b-form-input v-model="filter.contractorInn" :placeholder="$t('inn')"></b-form-input>
               </div>
            </b-col>
         </b-row>
         <b-row align-h="between">
            <b-col cols="12" md="5" class="mt-2">
               <StatusSelect v-model="filter.statusId" @input="Refresh" :filter="[0, 25, 26, 24]" />
            </b-col>
            <b-col cols="12" md="4"
               ><b-input-group class="text-right w-100 mt-2">
                  <b-form-input v-model="filter.search" :placeholder="$t('search')" />
                  <b-input-group-append>
                     <b-button @click="Refresh" variant="primary">
                        <feather-icon icon="SearchIcon" />
                     </b-button>
                  </b-input-group-append> </b-input-group
            ></b-col>
            <b-col cols="12" md="3" class="text-right mt-2 d-flex justify-content-end">
               <b-button @click="Print" variant="primary" class="ml-1" style="white-space: nowrap">
                  <feather-icon icon="PrinterIcon"></feather-icon>
                  {{ $t('Print') }}
               </b-button>

               <b-button @click="PrintPrinGraphExcel" variant="primary" class="ml-1" style="white-space: nowrap">
                  <feather-icon icon="PrinterIcon"></feather-icon>
                  <b-spinner v-if="PrinGraphLoading" small></b-spinner>
                  {{ $t('PrintPrinGraph') }}
               </b-button>
            </b-col>
         </b-row>
      </div>

      <b-table
         ref="refInvoiceListTable"
         :items="items"
         responsive
         :fields="fields"
         primary-key="id"
         sticky-header="65vh"
         no-border-collapse
         :busy="isBusy"
         show-empty
         :empty-text="$t('NotFound')"
         class="position-relative"
         @row-dblclicked="Edit"
         @row-clicked="rowclicked"
         @sort-changed="SortChange"
      >
         <template #cell(status)="{ item }">
            <b-badge :variant="getColor(item)">{{ item.status }}</b-badge>
         </template>

         <template #cell(contractorInn)="{ item }">
            <span style="color: blue; cursor: pointer" @click="goToBussnes(item.contractorInn)">{{
               item.contractorInn
            }}</span>
         </template>
         <template #cell(oked)="{ item }">{{ item.okedCode }} - {{ item.oked }}</template>

         <template #cell(actions)="{ item }">
            <b-link @click="Edit(item)" v-b-tooltip.hover.top="$t('View')" style="margin-right: 5px">
               <feather-icon icon="EyeIcon"></feather-icon>
            </b-link>

            <b-link
               v-if="item.statusid != 2 && $can('PrtnCertificateEdit', 'permissions')"
               v-b-tooltip.hover.top="$t('Edit')"
               @click="Edit(item)"
               style="margin-right: 5px"
            >
               <feather-icon icon="EditIcon"></feather-icon>
            </b-link>

            <b-link
               v-if="item.statusid != 2 && $can('PrtnCertificateApprove', 'permissions')"
               @click="$refs['ApproveModal' + item.id].show()"
               v-b-tooltip.hover.top="$t('Approve')"
               style="margin-right: 5px"
            >
               <feather-icon icon="CheckIcon"></feather-icon>
            </b-link>
            <b-link
               v-if="
                  item.statusid != 3 &&
                  item.statusid != 1 &&
                  item.statusid != 4 &&
                  $can('PrtnCertificateCancelApproval', 'permissions')
               "
               @click="$refs['CancelModal' + item.id].show()"
               v-b-tooltip.hover.top="$t('CancelApproval')"
               style="margin-right: 5px"
            >
               <feather-icon icon="XCircleIcon"></feather-icon>
            </b-link>
            <b-link
               v-if="item.statusid != 2 && $can('PrtnCertificateDelete', 'permissions')"
               @click="$refs['DeleteModal' + item.id].show()"
               style="margin-right: 5px"
               v-b-tooltip.hover.top="$t('Delete')"
            >
               <feather-icon icon="Trash2Icon"></feather-icon>
            </b-link>
            <b-link @click="OpenHistory(item)" style="margin-right: 5px" v-b-tooltip.hover.top="$t('history')">
               <b-icon-clock-history></b-icon-clock-history>
            </b-link>
            <b-modal size="xl" :ref="'HistoryModal' + item.id" hide-footer :title="$t('history')">
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
            <b-modal
               :ref="'ApproveModal' + item.id"
               :cancel-title="$t('Approve')"
               :ok-title="$t('Approve')"
               cancel-variant="danger"
               ok-variant="success"
               @ok="Approve(item)"
            >
               <template #modal-title>
                  {{ $t('Approve') }}
                  <b-spinner v-if="ApproveLoading" small></b-spinner>
               </template>
               <b-card-text>
                  <h5>ID : {{ item.id }}</h5>
                  <h5>{{ $t('WantApprove') }}</h5>
               </b-card-text>
            </b-modal>
            <b-modal
               :ref="'CancelModal' + item.id"
               :title="$t('Approve')"
               :cancel-title="$t('Cancel')"
               :ok-title="$t('Accept')"
               cancel-variant="danger"
               ok-variant="success"
               @ok="CancelApproval(item)"
            >
               <b-card-text>
                  <h5>{{ $t('admissiontype') }} : {{ item.admissiontypename }}</h5>
                  <h5>{{ $t('WantCancel') }}</h5>
               </b-card-text>
            </b-modal>
            <b-modal
               :ref="'DeleteModal' + item.id"
               :cancel-title="$t('Cancel')"
               :ok-title="$t('Accept')"
               cancel-variant="danger"
               ok-variant="success"
               @ok="Delete(item)"
            >
               <template #modal-title>
                  {{ $t('Delete') }}
                  <b-spinner v-if="DeleteLoading" small></b-spinner>
               </template>
               <b-card-text>
                  <h5>ID : {{ item.id }}</h5>
                  <h5>{{ $t('WantDelete') }}</h5>
               </b-card-text>
            </b-modal>
         </template>
         <template v-slot:table-busy>
            <div class="text-center text-primary my-2" style="vertical-align: middle">
               <b-spinner class="align-middle mr-2"></b-spinner>
               <strong>{{ $t('Loading') }}</strong>
            </div>
         </template>
      </b-table>

      <div class="mx-2 mb-2">
         <b-row>
            <b-col cols="12" sm="6" class="d-flex align-items-center justify-content-center justify-content-sm-start">
               <span class="text-muted">
                  {{ $t('Showing') }} {{ firstNumber }} {{ $t('to') }} {{ lastNumber }} {{ $t('of') }}
                  {{ filter.total }}
                  {{ $t('entries') }}
               </span>
               <v-select
                  :class="[isMobileDevice() ? 'w-50' : '']"
                  v-model="filter.pageSize"
                  :dir="$store.state.appConfig.isRTL ? 'rtl' : 'ltr'"
                  :options="filter.pageOptions"
                  :clearable="false"
                  @input="Refresh"
                  class="per-page-selector d-inline-block ml-50 mr-1"
               />
            </b-col>
            <!-- Pagination -->
            <b-col cols="12" sm="6" class="d-flex align-items-center justify-content-center justify-content-sm-end">
               <b-pagination
                  v-model="filter.page"
                  :total-rows="filter.total"
                  :per-page="filter.pageSize"
                  first-number
                  last-number
                  @input="Refresh"
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
   BInputGroupAppend,
   BIconClockHistory,
   BLink,
   BModal,
   BCardText,
   BButtonGroup,
   BIcon
} from 'bootstrap-vue';
import StatusSelect from '@/views/components/document/StatusSelect.vue';
import PrtnCertificateService from '@/services/document/prtncertificate.service';
import ManualService from '@/services/others/manual.service';
import RegionService from '@/services/info/region.service';
import DistrictService from '@/services/info/district.service';
import MfyService from '@/services/info/mfy.service';
import PrtnContractTypeService from '@/services/info/prtncontracttype.service';
export default {
   name: 'Index',
   components: {
      BButton,
      StatusSelect,
      BPagination,
      BTable,
      BCol,
      BRow,
      BSpinner,
      BIconClockHistory,
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
         filterModal: false,
         cache_filters: null,
         items: [],

         ContractTypeList: [],
         RegionList: [],
         DistrictList: [],
         MfyList: [],
         historyData: [],
         PrinGraphLoading: false,

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
         ],

         fields: [
            {
               key: 'actions',
               tdClass: 'text-right',
               thClass: 'text-center',
               label: this.$t('actions')
            },
            {
               key: 'id',
               label: this.$t('id'),
               sortable: false,
               tdClass: 'text-center',
               thClass: 'text-center'
            },
            {
               key: 'docNumber',
               label: this.$t('docnumber'),
               sortable: false
            },
            {
               key: 'docOn',
               label: this.$t('docdate'),
               sortable: false,
               tdClass: 'text-center',
               thClass: 'text-center'
            },
            {
               key: 'contractorRegion',
               label: this.$t('region'),
               sortable: false
            },
            {
               key: 'contractorDistrict',
               label: this.$t('Region'),
               sortable: false
            },
            {
               key: 'prtnContractType',
               label: this.$t('prtnContractType'),
               sortable: false
            },
            {
               key: 'contractorInn',
               label: this.$t('inn'),
               sortable: false
            },
            {
               key: 'contractor',
               label: this.$t('contractorT'),
               sortable: false
            },

            {
               key: 'oked',
               label: this.$t('Oked'),
               sortable: false
            },
            {
               key: 'expireOn',
               label: this.$t('expireOn'),
               sortable: false
            },
            {
               key: 'status',
               label: this.$t('status'),
               sortable: false,
               tdClass: 'text-center',
               thClass: 'text-center'
            }
         ],
         filter: {
            regionId: null,
            region: '',
            districtId: null,
            district: '',
            contractorInn: '',
            prtnContractTypeId: null,
            isTotalSuccessPost: true,
            prtnContractType: '',
            search: '',
            sortBy: '',
            orderType: 'asc',
            page: 1,
            okedCode: null,
            statusIds: [],
            pageSize: 20,
            pageOptions: [10, 20, 50, 100],
            total: 0,
            statusId: null
         },
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
         if (this.filter.total < this.filter.pageSize) {
            return this.filter.total;
         } else {
            if (this.filter.page * this.filter.pageSize > this.filter.total) {
               return this.filter.total;
            } else {
               return this.filter.page * this.filter.pageSize;
            }
         }
      }
   },
   created() {
      this.lang = localStorage.getItem('locale') || 'ru';
      if (this.$can('PrtnCertificateViewByRegion', 'permissions')) {
         this.filter.regionId = JSON.parse(localStorage.getItem('user_info')).organizationRegionId;
         this.GetFilter();
         this.GetDistrict();
      }
      this.GetFilter();
      this.Refresh();

      RegionService.GetAsSelectList(211)
         .then((res) => {
            this.RegionList = res.data;
         })
         .catch((error) => {
            this.showApiError(error);
         });

      PrtnContractTypeService.GetAsSelectList()
         .then((res) => {
            this.ContractTypeList = res.data;
         })
         .catch((error) => {
            this.showApiError(error);
         });
   },
   methods: {
      goToBussnes(inn) {
         this.$router.push({ name: 'BusinessmanCard', query: { inn: inn } });
      },
      SetFilter() {
         const tempFilter = {
            regionId: this.filter.regionId,
            mfyId: this.filter.mfyId,
            districtId: this.filter.districtId,
            contractorInn: this.filter.contractorInn,
            prtnContractTypeId: this.filter.prtnContractTypeId,
            search: this.filter.search,
            statusIds: this.filter.statusIds,
            statusId: this.filter.statusId
         };
         localStorage.setItem('prtncertificate_filters', JSON.stringify(tempFilter));
      },

      GetFilter() {
         const filter = localStorage.getItem('prtncertificate_filters');
         if (filter) {
            this.filter = { ...this.filter, ...JSON.parse(filter) };
         }
      },

      Print() {
         PrtnCertificateService.SaveAsExecel(this.filter).then((res) => {
            this.forceFileDownload(res, this.$t('prtncertificate'));
         });
      },
      PrintPrinGraphExcel() {
         this.PrinGraphLoading = true;
         PrtnCertificateService.PrinGraphExcel(this.filter).then((res) => {
            this.PrinGraphLoading = false;
            this.forceFileDownload(res, this.$t('PrintPrinGraph'));
         });
      },
      ChangeRegion() {
         this.filter.districtId = null;
         this.DistrictList = [];
         this.filter.mfyId = null;
         this.MfyList = [];

         if (this.filter.regionId) {
            this.GetDistrict();
            this.GetMfy();
         } else {
            this.Refresh();
         }
      },
      GetDistrict() {
         if (this.filter.regionId) {
            DistrictService.GetAsSelectList(this.filter.regionId)
               .then((res) => {
                  this.DistrictList = res.data;
                  this.GetMfy();
               })
               .catch((error) => {
                  this.showApiError(error);
               });
         } else {
            this.filter.districtId = null;
            this.filter.mfyId = null;
            this.DistrictList = [];
            this.MfyList = [];
            this.Refresh();
         }
      },
      ChangeDistrict() {
         this.filter.mfyId = null;
         this.MfyList = [];

         this.GetMfy();
      },
      GetMfy() {
         if (this.filter.regionId || this.filter.districtId) {
            MfyService.GetAsSelectList(this.filter.regionId, this.filter.districtId)
               .then((res) => {
                  this.MfyList = res.data;
                  this.Refresh();
               })
               .catch((error) => {
                  this.showApiError(error);
               });
         } else {
            this.filter.mfyId = null;
            this.MfyList = [];
            this.Refresh();
         }
      },
      OpenHistory(item) {
         this.$refs['HistoryModal' + item.id].show();
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
         this.filter.SortColumn = data.sortBy;
         this.filter.OrderType = data.sortDesc ? 'desc' : 'asc';
         this.Refresh();
      },

      Delete(item) {
         this.DeleteLoading = true;
         PrtnCertificateService.Delete(item.id)
            .then((res) => {
               this.DeleteLoading = false;
               this.Refresh();
               this.makeToast(this.$t('DeleteSuccess'), 'success');
            })
            .catch((error) => {
               this.DeleteLoading = false;
               this.showApiError(error);
            });
      },
      Edit(item) {
         if (item.statusid == 2) {
            this.$router.push({
               name: 'ViewPrtnCertificate',
               params: {
                  id: item.id
               }
            });
         } else {
            this.$router.push({
               name: 'EditPrtnCertificate',
               params: {
                  id: item.id
               }
            });
         }
      },
      Create() {
         this.$router.push({
            name: 'EditPrtnCertificate',
            params: {
               id: 0
            }
         });
      },

      Approve(item) {
         var self = this;
         PrtnCertificateService.Approve(item.id)
            .then((res) => {
               self.makeToast(self.$t('ApproveSuccess'), 'success');
               this.Refresh();
            })
            .catch((error) => {
               self.makeToast(
                  error.response.data.error,

                  'danger'
               );
            });
      },
      CancelApproval(item) {
         var self = this;

         PrtnCertificateService.CancelApproval(item.id)
            .then((res) => {
               self.makeToast(
                  self.$t('CancelMessage'),

                  'success'
               );
               this.Refresh();
            })
            .catch((error) => {
               self.makeToast(
                  error.response.data.error,

                  'danger'
               );
            });
      },
      Refresh() {
         this.isBusy = true;
         if (this.filter.statusId == 25) {
            this.filter.isTotalSuccessPost = false;
         } else {
            this.filter.isTotalSuccessPost = true;
         }
         if (this.filter.statusId == null || this.filter.statusId == 25) {
            this.filter.statusIds = [];
         } else {
            this.filter.statusIds = [this.filter.statusId];
         }

         this.SetFilter();

         PrtnCertificateService.GetList(this.filter)
            .then((res) => {
               this.items = res.data.rows;
               this.filter.total = res.data.total;
               this.isBusy = false;
            })
            .catch((error) => {
               this.makeToast(
                  error.response.data,

                  'danger'
               );
            })
            .finally(() => {
               this.isBusy = false;
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
