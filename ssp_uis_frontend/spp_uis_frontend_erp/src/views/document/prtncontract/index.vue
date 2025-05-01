<template>
   <form-table-hrm
      :items="items"
      :fields="fields"
      :filter.sync="filter"
      :busy="isBusy"
      :actions="{}"
      @request="Refresh"
      @row-dblclicked="Edit"
   >
      <!-- filtes -->
      <template #filter>
         <b-row align-h="between">
            <b-col sm="12" md="2">
               <div>
                  <form-select
                     :options="RegionList"
                     v-model="filter.regionId"
                     @input="ChangeRegion"
                     label="Oblast"
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
                  <label for>{{ $t('inn') }}</label>
                  <b-form-input
                     v-model="filter.contractorInn"
                     @keyup.enter="Refresh"
                     :placeholder="$t('inn')"
                  ></b-form-input>
               </div>
            </b-col>
            <b-col sm="12" md="2">
               <div>
                  <form-input v-model="filter.okedCode" :label="$t('Oked')" mask="#####" />
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
                     @input="Refresh"
                  ></v-select>
               </div>
            </b-col>
         </b-row>
         <b-row align-h="between" align-v="center">
            <b-col cols="12" md="" class="mt-1">
               <StatusSelect v-model="filter.statusId" @input="Refresh" :filter="[31, 28, 29, 27, 21, 25]" />
            </b-col>

            <b-col cols="12" lg="3" md="3" class="text-right mt-1">
               <!-- <label>{{ $t("search") }}</label> -->
               <b-input-group class="text-right">
                  <b-form-input v-model="filter.Search" :placeholder="$t('search')" />
                  <b-input-group-append>
                     <b-button @click="Refresh" variant="primary">
                        <feather-icon icon="SearchIcon" />
                     </b-button>
                     <b-button @click="Print2" variant="success" class="ml-1">
                        <feather-icon icon="PrinterIcon"></feather-icon>
                        <!-- {{ $t("Print") }} -->
                     </b-button>
                     <b-button @click="Print" variant="primary" class="ml-1">
                        <feather-icon icon="PrinterIcon"></feather-icon>
                        <!-- {{ $t("Print") }} -->
                     </b-button>
                  </b-input-group-append>
               </b-input-group>
            </b-col>
         </b-row>
         <b-row v-if="filter.statusId == 21 || filter.statusId == 0">
            <b-col cols="12" md="7" class="mt-2">
               <div>
                  <b-button-group @click="Refresh" size="sm">
                     <b-button
                        @click="filter.hasCertificate = null"
                        :variant="filter.hasCertificate == null ? 'primary' : 'outline-primary'"
                        >{{ $t('all') }}</b-button
                     >
                     <b-button
                        @click="filter.hasCertificate = true"
                        :variant="filter.hasCertificate == true ? 'primary' : 'outline-primary'"
                        >{{ $t('hasCertificate') }}</b-button
                     >
                     <b-button
                        @click="filter.hasCertificate = false"
                        :variant="filter.hasCertificate == false ? 'primary' : 'outline-primary'"
                        >{{ $t('hasNotCertificate') }}</b-button
                     >
                  </b-button-group>
               </div>
            </b-col>
         </b-row>
      </template>

      <!-- items -->
      <template #cell(status)="{ item }">
         <b-badge :variant="getColor(item)">{{ item.status }}</b-badge>
      </template>

      <template #cell(contractorInn)="{ item }">
         <span style="color: blue; cursor: pointer" @click="goToBussnes(item.contractorInn)">{{
            item.contractorInn
         }}</span>
      </template>

      <template #cell(oked)="{ item }">{{ item.okedCode }} - {{ item.oked }}</template>
      <template #cell(prtnCertificateStatus)="{ item }">
         <b-badge :variant="getColorStatus(item)">
            {{ item.prtnCertificateStatus }}
         </b-badge>
      </template>
      <template #cell(signed)="{ item }">
         <ul style="list-style-type: circle">
            <li v-for="(el, index) in item.signed" :key="index">
               {{ el.fullName }} {{ el.fio ? ' - ' + el.fio : '' }},
            </li>
         </ul>
      </template>
      <template #cell(notSigned)="{ item }">
         <ul style="list-style-type: circle">
            <li v-for="(el, index) in item.notSigned" :key="index">
               {{ el.fullName }} {{ el.fio ? ' - ' + el.fio : '' }},
            </li>
         </ul>
      </template>
      <template #cell(actions)="{ item }">
         <b-link
            :to="{ name: 'ViewPrtnContract', params: { id: item.id } }"
            v-b-tooltip.hover.top="$t('View')"
            style="margin-right: 5px"
         >
            <feather-icon icon="EyeIcon"></feather-icon>
         </b-link>

         <b-link
            v-if="
               item.statusId == 21 && $can('PrtnCertificateCreate', 'permissions') && item.prtnCertificateStatusId != 26
            "
            v-b-tooltip.hover.top="$t('GetSertificate')"
            @click="CertificateGet(item)"
            style="margin-right: 5px"
         >
            <feather-icon icon="FileIcon"></feather-icon>
         </b-link>
         <b-link
            v-if="item.statusId != 2 && $can('PrtnContractApprove', 'permissions')"
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
               $can('PrtnContractCancelApproval', 'permissions')
            "
            @click="$refs['CancelModal' + item.id].show()"
            v-b-tooltip.hover.top="$t('CancelApproval')"
            style="margin-right: 5px"
         >
            <feather-icon icon="XCircleIcon"></feather-icon>
         </b-link>

         <!-- document history -->
         <HistoryModalButton :id="item.id" :table-id="item.tableId || 100" />

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
   </form-table-hrm>
</template>

<script>
import {
   BButton,
   BPagination,
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
import PrtnContractService from '@/services/document/prtncontract.service';
import RegionService from '@/services/info/region.service';
import DistrictService from '@/services/info/district.service';
import MfyService from '@/services/info/mfy.service';
import PrtnContractTypeService from '@/services/info/prtncontracttype.service';
import FormTableHrm from '@/components/forms/form-table-hrm.vue';
import HistoryModalButton from '@/views/components/document/HistoryModalButton.vue';
import StatusSelect from '@/views/components/document/StatusSelect.vue';

export default {
   components: {
      BButton,
      BPagination,
      BCol,
      BRow,
      BIconClockHistory,
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
      FormTableHrm,
      HistoryModalButton,
      StatusSelect
   },
   directives: {
      'b-tooltip': VBTooltip,
      'b-modal': VBModal
   },
   data() {
      return {
         items: [],
         ContractTypeList: [],
         RegionList: [],
         DistrictList: [],
         historyData: [],
         MfyList: [],
         fields: [
            {
               key: 'actions',
               tdClass: 'text-center',
               thClass: 'text-center',
               label: this.$t('actions'),
               stickyColumn: !this.isMobileDevice()
            },
            {
               key: 'id',
               label: this.$t('id'),
               sortable: true,
               tdClass: 'text-center',
               thClass: 'text-center'
            },
            {
               key: 'docNumber',
               label: this.$t('docnumber'),
               sortable: true,
               tdClass: 'text-center',
               thClass: 'text-center'
            },
            {
               key: 'docOn',
               label: this.$t('docOn'),
               sortable: false,
               tdClass: 'text-center',
               thClass: 'text-center'
            },
            {
               key: 'contractorRegion',
               label: this.$t('oblast'),
               sortable: false,
               tdClass: 'text-center',
               thClass: 'text-center'
            },
            {
               key: 'contractorDistrict',
               label: this.$t('region'),
               sortable: false,
               tdClass: 'text-center',
               thClass: 'text-center'
            },
            {
               key: 'prtnContractType',
               label: this.$t('prtnContractType'),
               sortable: false,
               thStyle: {
                  minWidth: '300px'
               }
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
               sortable: false,
               thStyle: {
                  minWidth: '250px'
               }
            },
            {
               key: 'organization',
               label: this.$t('organizationT'),
               sortable: false,
               thStyle: {
                  minWidth: '250px'
               }
            },
            {
               key: 'signed',
               label: this.$t('signed'),
               sortable: false,
               thStyle: {
                  minWidth: '300px'
               }
            },
            {
               key: 'notSigned',
               label: this.$t('notSigned'),
               sortable: false,
               thStyle: {
                  minWidth: '300px'
               }
            },

            {
               key: 'status',
               label: this.$t('status'),
               sortable: false,
               tdClass: 'text-center',
               thClass: 'text-center'
            },
            {
               key: 'prtnCertificateStatus',
               label: this.$t('prtnCertificateStatus'),
               sortable: false,
               tdClass: 'text-center',
               thClass: 'text-center',
               stickyColumn: !this.isMobileDevice(),
               thStyle: {
                  right: this.isMobileDevice() ? '' : '0'
               },
               tdClass: 'r-0'
            }
         ],
         filter: {
            regionId: null,
            districtId: null,
            mfyId: null,
            hasCertificate: null,
            contractorInn: '',
            prtnContractTypeId: null,
            search: '',
            sortBy: '',
            orderType: 'asc',
            page: 1,
            pageSize: 20,
            statusId: null,
            okedCode: null,
            statusIds: [],
            perPageOptions: [10, 20, 50, 100],
            total: 0
         },
         isBusy: false,
         DeleteLoading: false
      };
   },
   created() {
      if (this.$can('PrtnContractViewByRegion', 'permissions')) {
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
            this.makeToast(error.response.data.errors, 'danger');
         });
      PrtnContractTypeService.GetAsSelectList()
         .then((res) => {
            this.ContractTypeList = res.data;
         })
         .catch((error) => {
            this.makeToast(error.response.data.errors, 'danger');
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
            statusId: this.filter.statusId,
            hasCertificate: this.filter.hasCertificate
         };
         localStorage.setItem('prtncontract_filters', JSON.stringify(tempFilter));
      },

      GetFilter() {
         const filter = localStorage.getItem('prtncontract_filters');
         if (filter) {
            this.filter = { ...this.filter, ...JSON.parse(filter) };
         }
      },
      Print() {
         PrtnContractService.SaveAsExecel(this.filter).then((res) => {
            this.forceFileDownload(res, this.$t('prtncontract'));
         });
      },
      Print2() {
         PrtnContractService.SaveAsExcelPrtnContracts(this.filter).then((res) => {
            this.forceFileDownload(res, this.$t('prtncontractH'));
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
            this.filter.districtId = null;
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
         this.filter.MfyList = [];

         this.GetMfy();
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
                  this.showApiError(error);
               });
         } else {
            this.filter.mfyId = null;
            this.MfyList = [];
            this.Refresh();
         }
      },

      Delete(item) {
         this.DeleteLoading = true;
         PrtnContractService.Delete(item.id)
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
      CertificateGet(item) {
         this.$router.push({
            name: 'EditPrtnCertificate',
            params: {
               id: item.id
            },
            query: {
               isList: true
            }
         });
      },
      Edit(item) {
         this.$router.push({
            name: 'ViewPrtnContract',
            params: {
               id: item.id
            }
         });
      },
      Create() {
         this.$router.push({
            name: 'EditPrtnContract',
            params: {
               id: 0
            }
         });
      },

      Approve(item) {
         var self = this;
         PrtnContractService.Approve(item.id)
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
         PrtnContractService.CancelApproval(item.id)
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
         if (this.filter.statusId == null) {
            this.filter.statusIds = [];
         } else {
            this.filter.statusIds = [this.filter.statusId];
         }

         this.SetFilter();

         PrtnContractService.GetList(this.filter)
            .then((res) => {
               this.items = res.data.rows;
               this.filter.total = res.data.total;
               this.isBusy = false;
            })
            .catch((error) => {
               this.makeToast(error.response.data, 'danger');
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
