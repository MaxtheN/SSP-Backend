<template>
   <form-table-hrm
      :items="items"
      :actions="{}"
      :fields="fields"
      :filter.sync="filter"
      :busy="isBusy"
      @request="Refresh"
      @row-dblclicked="DbClick"
   >
      <!-- filtes -->
      <template #filter>
         <b-row>
            <b-col cols="12" lg="3">
               <form-select
                  :options="RegionList"
                  v-model="filter.regionId"
                  @input="ChangeRegion"
                  :label="$t('Oblast')"
               ></form-select>
            </b-col>
            <b-col cols="12" lg="3">
               <form-select
                  :options="DistrictList"
                  :reduce="(item) => item.value"
                  :placeholder="$t('ChooseBelow')"
                  :label="$t('Region')"
                  v-model="filter.districtId"
                  @input="ChangeDistrict"
               />
            </b-col>
            <b-col cols="12" md="4" lg="2">
               <div>
                  <label for>{{ $t('contractorInn') }}</label>
                  <b-form-input
                     v-model="filter.contractorInn"
                     debounce="300"
                     v-mask="'#########'"
                     @update="Refresh"
                     :placeholder="$t('contractorInn')"
                  />
               </div>
            </b-col>
            <b-col cols="12" md="4" lg="2">
               <div>
                  <label for>{{ $t('startdate') }}</label>
                  <form-picker v-model="filter.fromDocDate" :placeholder="$t('startdate')" @input="Refresh" />
               </div>
            </b-col>
            <b-col cols="12" md="4" lg="2">
               <div>
                  <label for>{{ $t('enddate') }}</label>
                  <form-picker v-model="filter.toDocDate" :placeholder="$t('enddate')" @input="Refresh" />
               </div>
            </b-col>
         </b-row>
         <b-row align-v="center">
            <b-col cols="12" md="8"> </b-col>
            <b-col cols="12" md="4" class="text-right">
               <b-input-group>
                  <b-form-input v-model="filter.search" @keyup.enter="Refresh" :placeholder="$t('search')" />
                  <b-input-group-append>
                     <b-button @click="Refresh" variant="primary">
                        <feather-icon icon="SearchIcon" />
                     </b-button>
                  </b-input-group-append>
               </b-input-group>
            </b-col>
         </b-row>
      </template>
      <!-- items -->
      <template #cell(actions)="{ item }">
         <div class="text-center" style="text-wrap: nowrap">
            <!-- Edit -->
            <b-link
               :to="{ name: 'ViewJoinAntiCorruptionCertificate', params: { id: item.id } }"
               v-b-tooltip.hover.top="$t('View')"
               class="mr-1"
            >
               <feather-icon icon="EyeIcon"></feather-icon>
            </b-link>
            <!-- document history -->
            <HistoryModalButton :id="item.id" :table-id="item.tableId || 106" />
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
   BButtonGroup
} from 'bootstrap-vue';
import FormTableHrm from '@/components/forms/form-table-hrm.vue';
import JoinAntiCorruptionCertificateService from '@/services/corruption/joinanticorruptioncertificate.service';
import HistoryModalButton from '@/views/components/document/HistoryModalButton.vue';
import ContractorActivityTypeService from '@/services/hrm/contractoractivitytype.service';
import RegionService from '@/services/info/region.service';
import DistrictService from '@/services/info/district.service';

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
      BButtonGroup
   },
   directives: {
      'b-tooltip': VBTooltip,
      'b-modal': VBModal
   },
   emits: ['row-selected'],
   props: {
      selectable: {
         type: Boolean,
         default: false
      }
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
               key: 'contractorInn',
               label: this.$t('contractorInn')
            },
            {
               key: 'contractorFullName',
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
               key: 'contractorRegion',
               label: this.$t('region')
            },
            {
               key: 'contractorDistrict',
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
            statusIds: [],
            fromDocDate: '',
            toDocDate: '',
            statusId: null
         },
         isBusy: false
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
      Refresh() {
         this.isBusy = true;
         if (!this.filter.statusId) {
            this.filter.statusIds = [];
         } else {
            this.filter.statusIds = [this.filter.statusId];
         }
         JoinAntiCorruptionCertificateService.GetList({ ...this.filter })
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
      DbClick(item) {
         if (this.selectable) {
            this.$emit('row-selected', item);
         } else {
            this.$router.push({
               name: 'ViewJoinAntiCorruptionCertificate',
               params: { id: item.id }
            });
         }
      }
   }
};
</script>

<style>
.r-0 {
   right: 0;
}
</style>
