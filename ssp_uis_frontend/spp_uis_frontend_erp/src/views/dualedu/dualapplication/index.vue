<template>
   <div>
      <form-table-hrm
         :items="items"
         :actions="{}"
         :fields="fields"
         :filter.sync="filter"
         searchable
         :busy="isBusy"
         @request="Refresh"
         @row-dblclicked="(e) => $router.push({ name: 'EditDualApplication', params: { id: e.id } })"
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
                     placeholder="ChooseBelow"
                     label="Region"
                     v-model="filter.districtId"
                     @input="ChangeDistrict"
                  />
               </b-col>
               <b-col cols="12" md="2">
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
                           <b-button @click="Refresh" size="sm" variant="primary">
                              <feather-icon icon="SearchIcon" />
                           </b-button>
                        </b-input-group-append>
                     </b-input-group>
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
               <b-col cols="12" md="7">
                  <StatusSelect v-model="filter.statusId" @input="Refresh" :filter="[1, 8, 30, 2, 23, 25]" />
               </b-col>

               <b-col cols="12" md="4">
                  <b-input-group>
                     <b-form-input v-model="filter.search" @keyup.enter="Refresh" :placeholder="$t('search')" />
                     <b-input-group-append>
                        <b-button @click="Refresh" variant="primary">
                           <feather-icon icon="SearchIcon" />
                        </b-button>
                     </b-input-group-append>
                  </b-input-group>
               </b-col>
               <b-col cols="1" class="text-right">
                  <b-button
                     @click="Print"
                     v-b-tooltip.hover.top="$t('Print')"
                     :disabled="PrintLoading"
                     variant="primary"
                  >
                     <feather-icon icon="PrinterIcon"></feather-icon>
                  </b-button>
               </b-col>
            </b-row>
         </template>
         <template #cell(actions)="{ item }">
            <div class="text-center" style="text-wrap: nowrap">
               <!-- Edit -->
               <b-link
                  :to="{ name: 'EditDualApplication', params: { id: item.id } }"
                  v-b-tooltip.hover.top="$t('View')"
                  style="margin-right: 5px"
               >
                  <feather-icon icon="EyeIcon"></feather-icon>
               </b-link>
               <!-- document history -->
               <HistoryModalButton :id="item.id" :table-id="item.tableId" />
            </div>
         </template>
         <template #cell(status)="{ item }">
            <b-badge :variant="getColor(item)">{{ item.status }}</b-badge>
         </template>
         <template #cell(contractor)="{ item }">
            <span
               ><b-link
                  :to="{ name: 'BusinessmanCard', query: { inn: item.contractorInn } }"
                  class="font-weight-bold"
                  >{{ item.contractorInn }}</b-link
               >
               - {{ item.contractor }}</span
            >
         </template>
      </form-table-hrm>
   </div>
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
   BButtonGroup
} from 'bootstrap-vue';
import FormTableHrm from '@/components/forms/form-table-hrm.vue';
import DualApplicationService from '@/services/dualedu/dualapplication.service';
import HistoryModalButton from '@/views/components/document/HistoryModalButton.vue';
import RegionService from '@/services/info/region.service';
import StatusSelect from '@/views/components/document/StatusSelect.vue';
import DistrictService from '@/services/info/district.service';

import Filtermodal from '@/components/filter/filtermodal.vue';

export default {
   components: {
      BCard,
      FormTableHrm,
      BCardText,
      BButton,
      BBadge,
      BLink,
      BModal,
      HistoryModalButton,
      BInputGroup,
      BInputGroupAppend,
      BRow,
      BCol,
      BButtonGroup,
      BFormInput,
      StatusSelect,
      Filtermodal
   },
   directives: {
      'b-tooltip': VBTooltip,
      'b-modal': VBModal
   },
   data() {
      return {
         PrintLoading: false,
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
               label: this.$t('contractor')
            },
            {
               key: 'contractorPhoneNumber',
               label: this.$t('phoneNumber')
            },
            {
               key: 'region',
               label: this.$t('region')
            },
            {
               key: 'district',
               label: this.$t('District')
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
            DualApplicationTypeId: null,
            claimThemeId: null,
            fromDocDate: '',
            toDocDate: '',
            statusIds: [],
            statusId: null
         },
         isBusy: false
      };
   },
   mounted() {
      RegionService.GetAsSelectList(211).then((res) => {
         this.RegionList = res.data;
      });
   },
   methods: {
      Print() {
         this.PrintLoading = true;
         DualApplicationService.SaveExcelDualApplication(this.filter)
            .then((res) => {
               this.forceFileDownload(res, this.$t('dualedu'));
            })
            .catch((e) => {
               this.showApiError(e);
            })
            .finally(() => {
               this.PrintLoading = false;
            });
      },

      Refresh() {
         this.isBusy = true;
         if (!this.filter.statusId) {
            this.filter.statusIds = [];
         } else {
            this.filter.statusIds = [this.filter.statusId];
         }
         DualApplicationService.GetList(this.filter)
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
      }
   }
};
</script>
<style>
.r-0 {
   right: 0;
}
</style>
