<template>
   <form-table-hrm
      :items="items"
      :actions="{}"
      :fields="fields"
      :filter.sync="filter"
      :busy="isBusy"
      @request="Refresh"
      @row-dblclicked="(e) => $router.push({ name: 'ViewPrtnCreditDemand', params: { id: e.id } })"
   >
      <!-- filtes -->
      <template #filter>
         <b-row>
            <b-col cols="12" md="3">
               <form-select
                  :options="RegionList"
                  v-model="filter.regionId"
                  @input="ChangeRegion"
                  label="Oblast"
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
            <b-col cols="12" md="3">
               <form-select
                  :options="PrtnContractTypeList"
                  label="prtnContractType"
                  v-model="filter.prtnContractTypeId"
                  @input="Refresh"
               />
            </b-col>
            <b-col cols="12" md="3">
               <div>
                  <label for>{{ $t('contractorInn') }}</label>
                  <b-input-group>
                     <b-form-input
                        v-model="filter.contractorInn"
                        debounce="300"
                        v-mask="'#########'"
                        @keyup.enter="Refresh"
                        :placeholder="$t('contractorInn')"
                     />
                     <b-input-group-append>
                        <b-button @click="Refresh" variant="primary">
                           <feather-icon icon="SearchIcon" />
                        </b-button>
                     </b-input-group-append>
                  </b-input-group>
               </div>
            </b-col>
         </b-row>
         <b-row align-v="center">
            <b-col cols="12" md="4">
               <b-input-group class="mt-1">
                  <b-form-input v-model="filter.search" @keyup.enter="Refresh" :placeholder="$t('search')" />
                  <b-input-group-append>
                     <b-button @click="Refresh" variant="primary">
                        <feather-icon icon="SearchIcon" />
                     </b-button>
                  </b-input-group-append>
               </b-input-group>
            </b-col>
            <b-col class="text-right mt-1">
               <b-button variant="primary" @click="downloadExcel" :disabled="PrintLoading">
                  <b-icon-printer /> {{ $t('Print') }}
               </b-button>
            </b-col>
         </b-row>
      </template>
      <!-- items -->
      <template #cell(actions)="{ item }">
         <div class="text-center" style="text-wrap: nowrap">
            <!-- Edit -->
            <b-link
               :to="{ name: 'ViewPrtnCreditDemand', params: { id: item.id } }"
               v-b-tooltip.hover.top="$t('View')"
               class="mr-1"
            >
               <feather-icon icon="EyeIcon"></feather-icon>
            </b-link>
         </div>
      </template>
      <template #cell(projectCost)="{ item }">
         <div style="white-space: nowrap">
            {{ currency(item.projectCost) }}
         </div>
      </template>
      <template #cell(foreignInvestment)="{ item }">
         <div style="white-space: nowrap">
            {{ currency(item.foreignInvestment) }}
         </div>
      </template>
      <template #cell(ownInvestment)="{ item }">
         <div style="white-space: nowrap">
            {{ currency(item.ownInvestment) }}
         </div>
      </template>
      <template #cell(privillageBankCredit)="{ item }">
         <div style="white-space: nowrap">
            {{ currency(item.privillageBankCredit) }}
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
   BIconPrinter
} from 'bootstrap-vue';
import PrtnCreditDemandService from '@/services/document/prtncreditdemand.service';
import RegionService from '@/services/info/region.service';
import DistrictService from '@/services/info/district.service';
import PrtnContractTypeService from '@/services/info/prtncontracttype.service';

import FormTableHrm from '@/components/forms/form-table-hrm.vue';
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
      BInputGroup,
      BInputGroupAppend,
      BRow,
      BCol,
      BButtonGroup,
      BIconPrinter
   },
   directives: {
      'b-tooltip': VBTooltip,
      'b-modal': VBModal
   },
   data() {
      return {
         PrintLoading: false,
         items: [],
         RegionList: [],
         DistrictList: [],
         PrtnContractTypeList: [],
         fields: [
            {
               key: 'actions',
               label: this.$t('actions'),
               thClass: 'text-center',
               tdClass: 'text-center',
               stickyColumn: !this.isMobileDevice()
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
               sortable: true,
               thStyle: {
                  minWidth: '200px'
               }
            },
            {
               key: 'docOn',
               label: this.$t('docOn'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'regionName',
               label: this.$t('region')
            },
            {
               key: 'districtName',
               label: this.$t('District')
            },
            {
               key: 'prtnContractType',
               label: this.$t('prtnContractType'),
               thStyle: {
                  minWidth: '200px'
               }
            },
            {
               key: 'contractorInn',
               label: this.$t('contractorInn')
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
               key: 'implementedProjectName',
               label: this.$t('implementedProjectName'),
               thStyle: {
                  minWidth: '300px'
               }
            },
            {
               key: 'projectCost',
               label: this.$t('projectCost')
            },
            {
               key: 'ownInvestment',
               label: this.$t('ownInvestment')
            },
            {
               key: 'foreignInvestment',
               label: this.$t('foreignInvestment')
            },
            {
               key: 'privillageBankCredit',
               label: this.$t('privillageBankCredit')
            },
            {
               key: 'bankCode',
               label: this.$t('bankcode')
            },
            {
               key: 'bank',
               label: this.$t('bankName'),
               thStyle: {
                  minWidth: '200px'
               }
            },
            {
               key: 'status',
               label: this.$t('status'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true,
               stickyColumn: !this.isMobileDevice(),
               thStyle: {
                  right: this.isMobileDevice() ? '' : '0'
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
            prtnContractTypeId: null,
            statusId: null
         },
         isBusy: false
      };
   },
   mounted() {
      RegionService.GetAsSelectList(211).then((res) => {
         this.RegionList = res.data;
      });

      PrtnContractTypeService.GetAsSelectList().then((res) => {
         this.PrtnContractTypeList = res.data;
      });
   },
   methods: {
      Refresh() {
         this.isBusy = true;
         if (this.filter.statusId == 0) {
            this.filter.statusIds = [];
         } else {
            this.filter.statusIds = [this.filter.statusId];
         }
         PrtnCreditDemandService.GetList({ ...this.filter })
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
      downloadExcel() {
         this.PrintLoading = true;

         PrtnCreditDemandService.SaveAsExcel(this.filter)
            .then((res) => {
               this.makeToast(this.$t('SuccessMessage'), 'success');
               this.forceFileDownload(res, this.$t('PrtnCreditDemand'));
            })
            .catch((error) => {
               this.showApiError(error);
            })
            .finally(() => {
               this.PrintLoading = false;
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
