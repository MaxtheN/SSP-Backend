<template>
   <div>
      <b-card no-body>
         <div class="m-2">
            <b-row align-h="between">
               <b-col sm="12" md="3">
                  <div>
                     <form-select
                        :options="RegionList"
                        v-model="filter.regionId"
                        @input="ChangeRegion"
                        :label="$t('Oblast')"
                        @change="ChangeRegion"
                        :disabled="!$can('ApplicationViewByRegion', 'permissions')"
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
                        @input="Refresh"
                        class="w-100"
                     ></v-select>
                  </div>
               </b-col>
               <b-col sm="12" md="2">
                  <div>
                     <label for>{{ $t('inn') }}</label>
                     <b-form-input v-model="filter.contractorInn" :placeholder="$t('inn')"></b-form-input>
                  </div>
               </b-col>

               <b-col sm="12" md="3">
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

               <b-col class="mt-2" md="2">
                  <b-button variant="outline-primary" @click="Refresh" class="mr-2">
                     <feather-icon icon="RefreshCwIcon" />
                  </b-button>
                  <b-button @click="Print" variant="primary">
                     <feather-icon icon="PrinterIcon"></feather-icon>
                     {{ $t('Print') }}
                  </b-button>
               </b-col>
            </b-row>
            <b-row align-h="between" align-v="center">
               <b-col cols="12" md="8" class="mt-2"> </b-col>
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
            </b-row>
         </div>

         <form-table
            ref="myChild"
            :service="ApplicationService"
            :router="'Application'"
            :fields="fields"
            :actions="actions"
            :filter="filter"
            :permission="'Application'"
         ></form-table>
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
import FormTableHrm from '@/components/forms/form-table-hrm.vue';
import ApplicationService from '@/services/document/application.service';
import RegionService from '@/services/info/region.service';
import DistrictService from '@/services/info/district.service';
import PrtnContractTypeService from '@/services/info/prtncontracttype.service';
import JustSign from '@/components/justSign.vue';
import FormTable from '@/components/forms/form-table.vue';
export default {
   components: {
      BButton,
      FormTableHrm,
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
      JustSign
   },
   directives: {
      'b-tooltip': VBTooltip,
      'b-modal': VBModal
   },
   data() {
      return {
         data: [],
         ContractTypeList: [],
         ApplicationService,
         RegionList: [],
         DistrictList: [],
         actions: {
            edit: true,
            view: true,
            delete: true,
            history: true,
            mfyApplication: true
         },

         fields: [
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
               sortable: true,
               tdClass: 'text-center',
               thClass: 'text-center'
            },
            {
               key: 'docOn',
               label: this.$t('docOn'),
               sortable: true,
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
               key: 'mfy',
               label: this.$t('mfy'),
               sortable: false,
               tdClass: 'text-center',
               thClass: 'text-center'
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
               key: 'organization',
               label: this.$t('organizationT'),
               sortable: false
            },
            {
               key: 'status',
               label: this.$t('status'),
               sortable: false,
               tdClass: 'text-center',
               thClass: 'text-center'
            },
            {
               key: 'prtnContractStatus',
               label: this.$t('prtnContractStatus'),
               sortable: false,
               tdClass: 'text-center',
               thClass: 'text-center'
            }
         ],
         filter: {
            regionId: null,
            districtId: null,
            contractorInn: '',
            prtnContractTypeId: null,
            search: '',
            sortBy: '',
            orderType: 'asc',
            page: 1,
            pageSize: 20,
            statusId: 14,
            statusIds: [14],
            pageOptions: [10, 20, 50, 100],
            total: 0
         },
         isBusy: false,
         DeleteLoading: false
      };
   },
   created() {
      this.GetFilter();
      if (this.$can('ApplicationViewByRegion', 'permissions')) {
         this.filter.regionId = JSON.parse(localStorage.getItem('user_info')).organizationRegionId;
         this.GetFilter();

         this.GetDistrict(this.filter.regionId);
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
         localStorage.setItem('applicationcustom_filters', JSON.stringify(tempFilter));
      },

      GetFilter() {
         const filter = localStorage.getItem('applicationcustom_filters');
         if (filter) {
            this.filter = { ...this.filter, ...JSON.parse(filter) };
         }
      },
      Print() {
         ApplicationService.SaveAsExecel(this.filter).then((res) => {
            this.forceFileDownload(res, this.$t('application'));
         });
      },
      ChangeRegion(id) {
         if (id) {
            this.filter.districtId = null;

            this.Refresh();
            this.GetDistrict(id);
         } else {
            this.filter.districtId = null;

            this.Refresh();
         }
      },
      GetDistrict(id) {
         if (id) {
            DistrictService.GetAsSelectList(id)
               .then((res) => {
                  this.DistrictList = res.data;
               })
               .catch((error) => {
                  this.showApiError(error);
               });
         }
      },
      SortChange(data) {
         this.filter.SortColumn = data.sortBy;
         this.filter.OrderType = data.sortDesc ? 'desc' : 'asc';
         this.Refresh();
      },
      Refresh() {
         this.isBusy = true;
         this.SetFilter();
         this.$refs.myChild.refresh();
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
