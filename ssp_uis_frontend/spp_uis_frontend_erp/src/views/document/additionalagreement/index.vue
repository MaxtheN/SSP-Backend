<template>
   <form-table-hrm
      :items="items"
      :actions="{}"
      :fields="fields"
      :filter.sync="filter"
      :busy="isBusy"
      @request="Refresh"
      @row-dblclicked="(e) => $router.push({ name: 'ViewAdditionalAgreement', params: { id: e.id } })"
   >
      <!-- filtes -->
      <template #filter>
         <b-row>
            <b-col cols="12" md="3">
               <form-select
                  :options="RegionList"
                  v-model="filter.regionId"
                  @input="ChangeRegion"
                  label="region"
               ></form-select>
            </b-col>
            <b-col cols="12" md="3">
               <form-select
                  :options="DistrictList"
                  :reduce="(item) => item.value"
                  label="Region"
                  v-model="filter.districtId"
                  @input="ChangeDistrict"
               />
            </b-col>
            <b-col cols="12" md="2">
               <div>
                  <label for>{{ $t('contractorInn') }}</label>
                  <b-form-input
                     v-model="filter.contractorInn"
                     debounce="500"
                     v-mask="'#########'"
                     @update="Refresh"
                     placeholder="contractorInn"
                  />
               </div>
            </b-col>
            <b-col cols="12" md="2">
               <div>
                  <label for>{{ $t('startdate') }}</label>
                  <form-picker v-model="filter.fromDocDate" :placeholder="$t('startdate')" @input="Refresh" />
               </div>
            </b-col>
            <b-col cols="12" md="2">
               <div>
                  <label for>{{ $t('enddate') }}</label>
                  <form-picker v-model="filter.toDocDate" :placeholder="$t('enddate')" @input="Refresh" />
               </div>
            </b-col>
         </b-row>
         <b-row align-v="center">
            <b-col cols="12" md="8">
               <StatusSelect v-model="filter.statusId" @input="Refresh" :filter="[1, 25, 27, 21]" />
            </b-col>
            <b-col cols="4" class="d-flex no-wrap">
               <b-input-group>
                  <b-form-input v-model="filter.search" @keyup.enter="Refresh" :placeholder="$t('search')" />
                  <b-input-group-append>
                     <b-button @click="Refresh" variant="primary">
                        <feather-icon icon="SearchIcon" />
                     </b-button>
                  </b-input-group-append>
               </b-input-group>
               <b-button
                  v-if="0"
                  @click="Print"
                  v-b-tooltip.hover.top="$t('Print')"
                  :disabled="PrintLoading"
                  variant="primary"
                  class="ml-1"
               >
                  <feather-icon icon="PrinterIcon"></feather-icon>
               </b-button>
            </b-col>
         </b-row>
      </template>
      <!-- items -->

      <template #cell(contractor)="{ item }">
         <span style="color: blue; cursor: pointer" @click="goToBussnes(item.contractorInn)">
            {{ item.contractorInn }}
         </span>
         -
         {{ item.contractor }}
      </template>
      <template #cell(actions)="{ item }">
         <div class="text-center" style="text-wrap: nowrap">
            <!-- view -->
            <b-link
               :to="{ name: 'ViewAdditionalAgreement', params: { id: item.id } }"
               v-b-tooltip.hover.top="$t('View')"
               class="mr-1"
            >
               <feather-icon icon="EyeIcon"></feather-icon>
            </b-link>

            <!-- delete -->
            <b-link
               v-if="$can('AdditionalAgreementDelete', 'permissions') && item.canDelete"
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
            <!-- document history -->
            <HistoryModalButton :id="item.id" :table-id="item.tableId || 100" />
            <!-- edit -->
            <b-link
               v-if="$can('AdditionalAgreementCreate', 'permissions') && item.canEdit"
               v-b-tooltip.hover.top="$t('Edit')"
               @click="
                  $router.push({
                     name: 'EditAdditionalAgreement',
                     params: {
                        id: item.id
                     },
                     query: {
                        memshipContractId: item.id,
                        contractorId: item.contractorId,
                        applicationTypeId: 3 // MEMSHIP
                     }
                  })
               "
               class="ml-1 cursor-pointer"
            >
               <feather-icon icon="EditIcon"></feather-icon>
            </b-link>
         </div>
      </template>
      <template #cell(status)="{ item }">
         <b-badge :variant="getColor(item)">{{ item.status }}</b-badge>
      </template>
      <template #cell(docNumber)="{ item }">
         {{ item.docNumber }}
      </template>
      <template #cell(docOn)="{ item }">
         {{ item.docOn }}
      </template>
      <template #cell(contractorPhoneNumber)="{ item }">
         {{ item.contractorPhoneNumber }}
      </template>
      <template #cell(region)="{ item }">
         {{ item.region }}
      </template>
      <template #cell(district)="{ item }">
         {{ item.district }}
      </template>
   </form-table-hrm>
</template>

<script>
import {
   BCard,
   BCardText,
   VBTooltip,
   BBadge,
   BButton,
   BLink,
   BModal,
   VBModal,
   BFormInput,
   BInputGroup,
   BInputGroupAppend,
   BRow,
   BCol,
   BButtonGroup,
   BSpinner
} from 'bootstrap-vue';
import FormTableHrm from '@/components/forms/form-table-hrm.vue';
import AdditionalAgreementService from '@/services/document/additionalagreement.service';
import HistoryModalButton from '@/views/components/document/HistoryModalButton.vue';
import ContractorActivityTypeService from '@/services/hrm/contractoractivitytype.service';
import RegionService from '@/services/info/region.service';
import DistrictService from '@/services/info/district.service';
import StatusSelect from '@/views/components/document/StatusSelect.vue';

export default {
   components: {
      BCard,
      FormTableHrm,
      BCardText,
      BButton,
      BBadge,
      BLink,
      BModal,
      BFormInput,
      HistoryModalButton,
      BInputGroup,
      BInputGroupAppend,
      BRow,
      BCol,
      BButtonGroup,
      StatusSelect,
      BSpinner
   },
   directives: {
      'b-tooltip': VBTooltip,
      'b-modal': VBModal
   },
   data() {
      return {
         items: [],
         ContractorActivityTypeList: [],
         RegionList: [],
         DistrictList: [],
         fields: [
            {
               key: 'actions',
               label: this.$t('actions'),
               thClass: 'text-center',
               tdClass: 'text-center',
               stickyColumn: true
            },
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
               label: this.$t('contractor'),
               thStyle: {
                  minWidth: '300px'
               }
            },
            {
               key: 'contractorPhoneNumber',
               label: this.$t('phoneNumber')
            },
            {
               key: 'contractorActivityType',
               label: this.$t('contractorActivityType')
            },
            {
               key: 'status',
               label: this.$t('status'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true,
               stickyColumn: true,
               thStyle: {
                  right: '0'
               },
               tdClass: 'r-0'
            }
         ],
         filter: {
            search: '',
            sortBy: '',
            orderType: 'asc',
            page: 1,
            pageSize: 20,
            perPageOptions: [10, 20, 50, 100, 300],
            total: 0,
            contractorInn: '',
            regionId: null,
            districtId: null,
            statusIds: [],
            fromDocDate: '',
            toDocDate: '',
            statusId: null,
            docNumber: '',
            docOn: ''
         },
         deleteLoading: false,
         isBusy: false,
         PrintLoading: false
      };
   },
   mounted() {
      ContractorActivityTypeService.GetAsSelectList().then((res) => {
         this.ContractorActivityTypeList = res.data;
      });
      RegionService.GetAsSelectList(211).then((res) => {
         this.RegionList = res.data;
      });
   },
   methods: {
      Print() {
         this.PrintLoading = true;
         AdditionalAgreementService.SaveAsExcel(this.filter)
            .then((res) => {
               this.forceFileDownload(res, this.$t('AdditionalAgreement'));
            })
            .catch((e) => {
               this.showApiError(e);
            })
            .finally(() => {
               this.PrintLoading = false;
            });
      },
      goToBussnes(inn) {
         this.$router.push({ name: 'BusinessmanCard', query: { inn: inn } });
      },
      Refresh() {
         this.isBusy = true;
         if (!this.filter.statusId) {
            this.filter.statusIds = [];
         } else {
            this.filter.statusIds = [this.filter.statusId];
         }
         AdditionalAgreementService.GetList({ ...this.filter })
            .then((res) => {
               this.items = res.data.rows;

               this.filter.total = res.data.total;
            })
            .catch((err) => {
               this.showApiError(err);
            })
            .finally(() => {
               this.isBusy = false;
            });
      },
      ChangeRegion() {
         if (this.filter.regionId) {
            this.filter.districtId = null;
            this.GetDistrict();
         }
         this.Refresh();
      },
      GetDistrict() {
         if (this.filter.regionId) {
            DistrictService.GetAsSelectList(this.filter.regionId).then((res) => {
               this.DistrictList = res.data;
            });
         } else {
            this.filter.districtId = null;
            this.DistrictList = [];
            this.Refresh();
         }
      },
      ChangeDistrict() {
         this.Refresh();
      },
      Delete(item) {
         this.deleteLoading = true;
         AdditionalAgreementService.Delete(item.id)
            .then(() => {
               this.makeToast(this.$t('DeleteSuccess'), 'success');
               this.Refresh();
            })
            .catch((err) => {
               this.showApiError(err);
            })
            .finally(() => {
               this.deleteLoading = false;
            });
      }
   }
};
</script>

<style>
.r-0 {
   right: 0;
}
</style>
