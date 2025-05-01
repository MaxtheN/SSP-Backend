<template>
   <div>
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
            <b-row align-h="between" align-v="center">
               <b-col cols="12" md="7" class="mt-2">
                  <StatusSelect v-model="filter.statusId" @input="Refresh" :filter="[0, 8, 30, 2, 25]" />
               </b-col>
               <b-col cols="12" lg="3" class="text-right mt-2">
                  <b-input-group class="text-right">
                     <b-form-input v-model="filter.search" :placeholder="$t('search')" />
                     <b-input-group-append>
                        <b-button @click="Refresh" variant="primary">
                           <feather-icon icon="SearchIcon" />
                        </b-button>
                     </b-input-group-append>
                  </b-input-group>
               </b-col>
               <b-col class="mt-2" md="2">
                  <b-button @click="Print" variant="primary">
                     <feather-icon icon="PrinterIcon"></feather-icon>
                     {{ $t('Print') }}
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
            class="position-relative table-responsive-md"
            @row-dblclicked="Edit"
            @sort-changed="SortChange"
         >
            <template #cell(status)="{ item }">
               <b-badge :variant="getColor(item)">{{ item.status }}</b-badge>
            </template>
            <template #cell(prtnContractStatus)="{ item }">
               <b-badge :variant="getColorStatus(item)">{{ item.prtnContractStatus }}</b-badge>
            </template>
            <template #cell(oked)="{ item }">{{ item.okedCode }} - {{ item.oked }}</template>
            <template #cell(chooseLocation)="{ item }">
               <b-badge v-if="item.chooseLocation" variant="success">
                  {{ $t('yes') }}
               </b-badge>

               <b-badge v-else variant="danger">{{ $t('no') }}</b-badge>
            </template>
            <template #cell(isLastOffer)="{ item }">
               <b-badge v-if="item.isLastOffer" variant="success">
                  {{ $t('yes') }}
               </b-badge>

               <b-badge v-else variant="danger">{{ $t('no') }}</b-badge>
            </template>
            <template #cell(actions)="{ item }">
               <b-link
                  v-if="item.statusid == 2"
                  :to="{ name: 'ViewApplication', params: { id: item.id } }"
                  v-b-tooltip.hover.top="$t('View')"
                  style="margin-right: 5px"
               >
                  <feather-icon icon="EyeIcon"></feather-icon>
               </b-link>
               <b-link v-b-tooltip.hover.top="$t('seeinfo')" @click="Edit(item)" style="margin-right: 5px">
                  <feather-icon icon="EyeIcon"></feather-icon>
               </b-link>
               <b-link
                  v-if="item.canViewMahallaResult"
                  @click="GetMfyApplication(item)"
                  style="margin-right: 5px"
                  v-b-tooltip.hover.top="$t('mfyApplication')"
               >
                  <feather-icon icon="FolderIcon"></feather-icon>
               </b-link>

               <b-link @click="OpenHistory(item)" style="margin-right: 5px" v-b-tooltip.hover.top="$t('history')">
                  <b-icon-clock-history></b-icon-clock-history>
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
                     {{ $t('Delete') }}
                     <b-spinner v-if="DeleteLoading" small></b-spinner>
                  </template>
                  <b-card-text>
                     <h5>ID : {{ item.id }}</h5>
                     <h5>{{ $t('WantDelete') }}</h5>
                  </b-card-text>
               </b-modal>
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
               <b-modal size="xl" :ref="'MfyApplication' + item.id" hide-footer :title="$t('mfyApplication')">
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
                              <b-badge
                                 v-show="!isEmpty(MfyApplicationData)"
                                 :variant="MfyApplicationData.isAccepted ? 'success' : 'danger'"
                              >
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
               <b-col
                  cols="12"
                  sm="6"
                  class="d-flex align-items-center justify-content-center justify-content-sm-start"
               >
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
   </div>
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
   BIconClockHistory,
   BInputGroupAppend,
   BLink,
   BModal,
   BCardText,
   BButtonGroup,
   BIcon
} from 'bootstrap-vue';
import ApplicationService from '@/services/document/application.service';
import RegionService from '@/services/info/region.service';
import DistrictService from '@/services/info/district.service';
import MfyService from '@/services/info/mfy.service';
import ManualService from '@/services/others/manual.service';
import PrtnContractTypeService from '@/services/info/prtncontracttype.service';
import JustSign from '@/components/justSign.vue';
import OkedService from '@/services/info/oked.service';
import FormTable from '@/components/forms/form-table.vue';
import StatusSelect from '@/views/components/document/StatusSelect.vue';
export default {
   components: {
      BButton,
      BPagination,
      FormTable,
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
      BIcon,
      StatusSelect,
      JustSign
   },
   directives: {
      'b-tooltip': VBTooltip,
      'b-modal': VBModal
   },
   name: 'Index',
   data() {
      return {
         data: [],
         items: [],
         ContractTypeList: [],
         ApplicationService,
         RegionList: [],
         OkedList: [],
         DistrictList: [],
         actions: {
            edit: true,
            view: true,
            delete: true,
            history: true,
            mfyApplication: true
         },
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
               label: this.$t('actions')
            },
            {
               key: 'isLastOffer',
               label: this.$t('isLastOffer'),
               sortable: false
            },
            {
               key: 'id',
               label: this.$t('id'),
               sortable: false
            },
            {
               key: 'docNumber',
               label: this.$t('docnumber'),
               sortable: true
            },
            {
               key: 'docOn',
               label: this.$t('docOn'),
               sortable: true
            },
            {
               key: 'contractorRegion',
               label: this.$t('oblast'),
               sortable: false
            },
            {
               key: 'contractorDistrict',
               label: this.$t('region'),
               sortable: false
            },
            {
               key: 'chooseLocation',
               label: this.$t('chooseLocation'),
               sortable: false
            },
            {
               key: 'choosedRegion',
               label: this.$t('choosedRegion'),
               sortable: false
            },

            {
               key: 'choosedDistrict',
               label: this.$t('choosedDistrict'),
               sortable: false
            },

            {
               key: 'mfy',
               label: this.$t('mfy'),
               sortable: false
            },
            {
               key: 'prtnApplicationNewVacanciesCount',
               label: this.$t('prtnApplicationNewVacanciesCount'),
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
               key: 'contractorPhoneNumber',
               label: this.$t('phoneNumber'),
               sortable: false
            },
            {
               key: 'organization',
               label: this.$t('organizationT'),
               sortable: false
            },
            {
               key: 'status',
               label: this.$t('status'),
               sortable: false
            },

            {
               key: 'prtnContractStatus',
               label: this.$t('prtnContractStatus'),
               sortable: false
            }
         ],
         MfyList: [],
         historyData: [],
         mfyApplication: false,
         MfyApplicationData: {},
         filter: {
            regionId: null,
            districtId: null,
            mfyId: null,
            contractorInn: '',
            prtnContractTypeId: null,
            okedId: null,
            okedCode: null,
            search: '',
            sortBy: '',
            orderType: 'asc',
            page: 1,
            pageSize: 20,
            statusId: null,
            statusIds: [],
            pageOptions: [10, 20, 50, 100],
            total: 0
         },
         isBusy: false,
         DeleteLoading: false
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
      this.GetFilter();
      if (this.$can('ApplicationViewByRegion', 'permissions')) {
         this.filter.regionId = JSON.parse(localStorage.getItem('user_info')).organizationRegionId;

         this.GetFilter();
         this.GetDistrict();
      } else {
         this.Refresh();
      }

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
      OkedService.GetAsSelectList()
         .then((res) => {
            this.OkedList = res.data;
         })
         .catch((error) => {
            this.showApiError(error);
         });
   },
   methods: {
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
         localStorage.setItem('application_filters', JSON.stringify(tempFilter));
      },

      GetFilter() {
         const filter = localStorage.getItem('application_filters');
         if (filter) {
            this.filter = { ...this.filter, ...JSON.parse(filter) };
         }
      },
      Print() {
         ApplicationService.SaveAsExecel(this.filter).then((res) => {
            this.forceFileDownload(res, this.$t('Application'));
         });
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
      GetMfyApplication(item) {
         this.MfyApplicationData = {};
         this.$refs['MfyApplication' + item.id].show();
         ApplicationService.GetMfyApplication(item.id2)
            .then((res) => {
               this.MfyApplicationData = res.data;
               console.log(1111, this.MfyApplicationData);
            })
            .catch((error) => {
               this.showApiError(error);
            });
      },
      ChangeRegion() {
         if (this.filter.regionId) {
            this.filter.districtId = null;
            this.filter.mfyId = null;
            this.GetDistrict();
            this.GetMfy();
         } else {
            this.filter.districtId = null;
            this.filter.mfyId = null;
            this.MfyList = [];
            this.DistrictList = [];
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
         if (this.filter.districtId) {
            this.filter.mfyId = null;
            this.GetMfy();
         } else {
            this.filter.mfyId = null;
            this.filter.MfyList = [];
         }

         this.Refresh();
      },
      GetMfy() {
         if (this.filter.regionId || this.filter.districtId) {
            MfyService.GetAsSelectList(this.filter.regionId, this.filter.districtId)
               .then((res) => {
                  this.MfyList = res.data;
                  this.Refresh();
               })
               .catch((error) => {
                  console.log(error);
                  this.showApiError(error);
               });
         } else {
            this.filter.mfyId = null;
            this.MfyList = [];
            this.Refresh();
         }
      },
      SortChange(data) {
         this.filter.SortColumn = data.sortBy;
         this.filter.OrderType = data.sortDesc ? 'desc' : 'asc';
         this.Refresh();
      },

      Edit(item) {
         this.$router.push({
            name: 'EditApplication',
            params: {
               id: item.id
            }
         });
      },
      Create() {
         this.$router.push({
            name: 'EditApplication',
            params: {
               id: 0
            }
         });
      },
      Refresh() {
         this.isBusy = true;
         if (this.filter.statusId == null) {
            this.filter.statusIds = [];
         } else {
            this.filter.statusIds = [this.filter.statusId];
         }
         this.SetFilter();
         ApplicationService.GetList(this.filter)
            .then((res) => {
               this.items = res.data.rows;
               this.filter.total = res.data.total;
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
@import '@core/scss/vue/libs/vue-select.scss';

/* Jadvalni telefon ekranlari uchun moslashuvchan qilish */
@media (max-width: 576px) {
   .table-responsive-md {
      overflow-x: auto;
      -webkit-overflow-scrolling: touch;
   }
   .table-responsive-md .table {
      min-width: 100%;
      white-space: nowrap;
   }
}
</style>
