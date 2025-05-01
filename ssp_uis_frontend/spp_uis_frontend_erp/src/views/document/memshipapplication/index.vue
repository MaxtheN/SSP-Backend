<template>
   <form-table-hrm
      :items="items"
      :fields="fields"
      :filter.sync="filter"
      :busy="isBusy"
      :actions="{}"
      @request="Refresh"
      @row-dblclicked="(e) => $router.push({ name: 'ViewMemshipApplication', params: { id: e.id } })"
   >
      <!-- filtes -->
      <template #filter>
         <b-row>
            <b-col cols="12" md="3" class="mb-1">
               <b-button variant="primary" :to="{ name: 'EditMemshipApplication', params: { id: 0 } }">
                  <feather-icon icon="PlusIcon"></feather-icon>
                  {{ $t('create') }}
               </b-button>
            </b-col>
            <b-col></b-col>
            <b-col cols="12" md="4" class="mb-1 text-right pr-0">
               <b-input-group>
                  <b-form-input v-model="filter.search" @keyup.enter="Refresh" :placeholder="$t('search')" />
                  <b-input-group-append>
                     <b-button @click="Refresh" variant="primary">
                        <feather-icon icon="SearchIcon" />
                     </b-button>
                  </b-input-group-append>
               </b-input-group>
            </b-col>
            <b-col cols="12" md="2" class="mb-1 text-right pl-0">
               <b-button
                  @click="Print"
                  v-b-tooltip.hover.top="$t('Print')"
                  :disabled="PrintLoading"
                  variant="primary"
                  class="ml-1"
               >
                  <feather-icon icon="PrinterIcon"></feather-icon>
                  {{ $t('Print') }}
               </b-button>
            </b-col>
            <b-col cols="12" md="3">
               <div>
                  <label for>{{ $t('innOrPinfl') }}</label>
                  <b-input-group>
                     <b-form-input
                        type="text"
                        v-model="filter.contractorInn"
                        debounce="300"
                        v-mask="['##############']"
                        @keyup.enter="Refresh"
                        :placeholder="$t('innOrPinfl')"
                     />
                     <b-input-group-append>
                        <b-button @click="Refresh" size="sm" variant="primary">
                           <feather-icon icon="SearchIcon" />
                        </b-button>
                     </b-input-group-append>
                  </b-input-group>
               </div>
            </b-col>
            <b-col cols="12" md="3">
               <form-select
                  :options="RegionList"
                  v-model="filter.regionId"
                  @input="ChangeRegion"
                  :disabled="localStorageData.organizationId != 1"
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
            <b-col cols="12" md="3">
               <IsPinflSelect v-model="filter.isPinfl" @input="Refresh" />
            </b-col>
            <b-col sm="12" md="3">
               <form-select
                  v-model="filter.contractorActivityTypeId"
                  :options="ContractorActivityTypeList"
                  label="contractorActivityType"
                  @input="Refresh"
               />
            </b-col>
            <b-col sm="12" md="3">
               <form-select
                  v-model="filter.contractorCategoryId"
                  :options="ContractorCategoryList"
                  label="contractorCategory"
                  @input="Refresh"
               />
            </b-col>
            <b-col cols="12" md="3">
               <div>
                  <label for>{{ $t('startdate') }}</label>
                  <form-picker v-model="filter.fromDocDate" :placeholder="$t('startdate')" @input="Refresh" />
               </div>
            </b-col>
            <b-col cols="12" md="3">
               <div>
                  <label for>{{ $t('enddate') }}</label>
                  <form-picker v-model="filter.toDocDate" :placeholder="$t('enddate')" @input="Refresh" />
               </div>
            </b-col>
            <b-col cols="12" md="3">
               <CertificateExpireSelect
                  @date="
                     (e) => {
                        filter.fromExpireOn = e.fromExpireOn;
                        filter.toExpireOn = e.toExpireOn;
                        Refresh();
                     }
                  "
               />
            </b-col>
            <b-col cols="12" md="3">
               <form-select
                  :options="OpfSelectList"
                  v-model="filter.opfId"
                  @input="ChangeRegion"
                  label="opf"
               ></form-select>
            </b-col>
            <b-col cols="12" md="" class="mt-2">
               <StatusSelect v-model="filter.statusId" @input="Refresh" :filter="[1, 2, 25, 30]" />
            </b-col>
         </b-row>
         <b-row align-v="center"> </b-row>
      </template>
      <!-- items -->

      <template #cell(contractor)="{ item }">
         <span style="color: blue; cursor: pointer" @click="goToBussnes(item.application.contractorInn)">
            {{ item.application.contractorInn }}
         </span>
         -
         {{ item.application.contractor }}
      </template>
      <template #cell(director)="{ item }">
         <span style="color: blue; cursor: pointer">{{ item.application.contractorPinfl }}</span>
         -
         {{ item.application.contractorDirector }}
      </template>
      <template #cell(actions)="{ item }">
         <div class="text-center" style="text-wrap: nowrap">
            <!-- Edit -->
            <b-link
               :to="{ name: 'ViewMemshipApplication', params: { id: item.id } }"
               v-b-tooltip.hover.top="$t('View')"
               class="mr-1"
            >
               <feather-icon icon="EyeIcon"></feather-icon>
            </b-link>
            <!-- document history -->
            <HistoryModalButton :id="item.id" :table-id="item.tableId || 100" />
         </div>
      </template>
      <template #cell(status)="{ item }">
         <b-badge :variant="getColor({ statusId: item.application.statusId, status: item.application.status })">{{
            item.application.status
         }}</b-badge>
      </template>
      <template #cell(docNumber)="{ item }">
         {{ item.application.docNumber }}
      </template>
      <template #cell(docOn)="{ item }">
         {{ item.application.docOn }}
      </template>
      <template #cell(contractorPhoneNumber)="{ item }">
         {{ item.application.contractorPhoneNumber }}
      </template>
      <template #cell(region)="{ item }">
         {{ item.chooseLocation ? item.choosedRegion : item.application.region }}
      </template>
      <template #cell(district)="{ item }">
         {{ item.chooseLocation ? item.choosedDistrict : item.application.district }}
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
import MemshipApplicationService from '@/services/document/memshipapplication.service';
import HistoryModalButton from '@/views/components/document/HistoryModalButton.vue';
import ContractorActivityTypeService from '@/services/hrm/contractoractivitytype.service';
import RegionService from '@/services/info/region.service';
import DistrictService from '@/services/info/district.service';
import StatusSelect from '@/views/components/document/StatusSelect.vue';
import ManualService from '@/services/others/manual.service';
import IsPinflSelect from '@/views/components/memship/IsPinflSelect.vue';
import CertificateExpireSelect from '@/views/components/memship/CertificateExpireSelect.vue';

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
      IsPinflSelect,
      CertificateExpireSelect
   },
   directives: {
      'b-tooltip': VBTooltip,
      'b-modal': VBModal
   },
   data() {
      return {
         items: [],
         ContractorActivityTypeList: [],
         ContractorCategoryList: [],
         RegionList: [],
         DistrictList: [],
         OpfSelectList: [],
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
               key: 'contractorCategory',
               label: this.$t('contractorCategory')
            },
            {
               key: 'director',
               label: this.$t('director'),
               thStyle: {
                  minWidth: '250px'
               }
            },
            {
               key: 'contractorPhoneNumber',
               label: this.$t('phoneNumber')
            },
            {
               key: 'contractorActivityType',
               label: this.$t('contractorActivityType'),
               thStyle: {
                  minWidth: '300px'
               }
            },
            {
               key: 'okedCode',
               label: this.$t('Oked')
            },
            {
               key: 'employeesCount',
               label: this.$t('employeesCount')
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
            statusIds: [],
            fromDocDate: '',
            opfId: null,
            toDocDate: '',
            statusId: null,
            contractorCategoryId: null,
            isPinfl: null
         },
         isBusy: false,
         PrintLoading: false,
         localStorageData: {}
      };
   },
   created() {
      this.GetlocalStorageData();
   },
   mounted() {
      ContractorActivityTypeService.GetAsSelectList().then((res) => {
         this.ContractorActivityTypeList = res.data;
      });
      RegionService.GetAsSelectList(211).then((res) => {
         this.RegionList = res.data;
      });
      ManualService.ContractorCategorySelectList().then((res) => {
         this.ContractorCategoryList = res.data;
      });
      ManualService.OpfSelectList().then((res) => {
         this.OpfSelectList = res.data;
      });
   },
   methods: {
      Print() {
         this.PrintLoading = true;
         MemshipApplicationService.SaveAsExcel(this.filter)
            .then((res) => {
               this.forceFileDownload(res, this.$t('MemshipApplication'));
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
         MemshipApplicationService.GetList({ ...this.filter })
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
      ChangeOPF() {
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
      GetlocalStorageData() {
         this.localStorageData = JSON.parse(localStorage.getItem('user_info'));
         if (this.localStorageData.organizationId != 1) {
            this.filter.regionId = this.localStorageData.organizationRegionId;
            this.GetDistrict(this.localStorageData.organizationRegionId);
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
