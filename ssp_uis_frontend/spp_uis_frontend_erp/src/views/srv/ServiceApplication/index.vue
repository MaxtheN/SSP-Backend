<template>
   <div>
      <b-tabs class="nav-tabs mb-0" nav-class="mb-0" v-model="tab" @input="Refresh">
         <b-tab :title="$t('all')"> </b-tab>
         <b-tab :title="$t('pullik')"> </b-tab>
         <b-tab :title="$t('tekin')"> </b-tab>
      </b-tabs>
      <form-table-hrm
         :items="items"
         :actions="{}"
         :fields="fields"
         :filter.sync="filter"
         :busy="isBusy"
         @request="Refresh"
         @row-dblclicked="(e) => $router.push({ name: 'ViewServiceApplication', params: { id: e.id } })"
      >
         <!-- filtes -->
         <template #filter>
            <b-row>
               <b-col cols="12" md="4" lg="3">
                  <form-select
                     :options="RegionList"
                     v-model="filter.regionId"
                     @input="ChangeRegion"
                     label="Oblast"
                  ></form-select>
               </b-col>
               <b-col cols="12" md="4" lg="3">
                  <form-select
                     :options="DistrictList"
                     :reduce="(item) => item.value"
                     label="Region"
                     v-model="filter.districtId"
                     @input="ChangeDistrict"
                  />
               </b-col>
               <b-col cols="12" md="4" lg="2">
                  <div>
                     <label for>{{ $t('contractorInn') }}</label>
                     <b-form-input
                        v-model="filter.contractorInn"
                        v-mask="'#########'"
                        @input="Refresh"
                        placeholder="contractorInn"
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
               <b-col cols="12" md="6" lg="8">
                  <StatusSelect v-model="filter.statusId" @input="Refresh" :filter="[8, 24, 2]" />
               </b-col>
               <b-col cols="12" md="6" lg="4" class="d-flex no-wrap">
                  <b-input-group>
                     <b-form-input v-model="filter.search" @keyup.enter="Refresh" :placeholder="$t('search')" />
                     <b-input-group-append>
                        <b-button @click="Refresh" variant="primary">
                           <feather-icon icon="SearchIcon" />
                        </b-button>
                     </b-input-group-append>
                  </b-input-group>
                  <b-button
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
            <span style="color: blue; cursor: pointer" @click="goToBussnes(item.application.contractorInn)">
               {{ item.application.contractorInn }}
            </span>
            -
            {{ item.application.contractor }}
         </template>
         <template #cell(actions)="{ item }">
            <div class="text-center" style="text-wrap: nowrap">
               <!-- view -->
               <b-link
                  :to="{ name: 'ViewServiceApplication', params: { id: item.id } }"
                  v-b-tooltip.hover.top="$t('View')"
                  class="mr-1"
               >
                  <feather-icon icon="EyeIcon"></feather-icon>
               </b-link>

               <!-- Cancel -->
               <template v-if="item.canCancel">
                  <b-link
                     class="text-danger cursor-pointer mr-1"
                     v-b-tooltip.hover.top="$t('Cancel')"
                     @click="Cancel(item)"
                  >
                     <feather-icon icon="XCircleIcon"></feather-icon>
                  </b-link>
               </template>

               <b-link
                  v-if="item.canDelete"
                  class="text-danger mr-1 cursor-pointer"
                  @click="Delete(item)"
                  v-b-tooltip.hover.top="$t('Delete')"
               >
                  <feather-icon icon="TrashIcon"></feather-icon>
               </b-link>

               <!-- contract -->
               <b-link
                  v-if="item.canCreateContract"
                  :to="{ name: 'EditServiceContract', params: { id: 0 }, query: { appId: item.id } }"
                  v-b-tooltip.hover.top="$t('ServiceContract')"
                  class="mr-1"
               >
                  <feather-icon icon="FileTextIcon"></feather-icon>
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
            {{ item.region }}
         </template>
         <template #cell(district)="{ item }">
            {{ item.district }}
         </template>

         <template #cell(isFree)="{ item }">
            {{ item.isFree ? $t('true') : $t('false') }}
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
   BTabs,
   BTab,
   BButtonGroup
} from 'bootstrap-vue';
import FormTableHrm from '@/components/forms/form-table-hrm.vue';
import ServiceApplicationService from '@/services/srv/ServiceApplication.service';
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
      BTabs,
      BTab,
      StatusSelect
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
               key: 'isFree',
               label: this.$t('tekin')
            },
            {
               key: 'region',
               label: this.$t("Xizmat Ko'rsatish hududdi")
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
            sortBy: 'id',
            orderType: 'desc',
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
         isBusy: false,
         PrintLoading: false,
         tab: 0
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
         ServiceApplicationService.PrintGraphExcel(this.filter)
            .then((res) => {
               this.forceFileDownload(res, this.$t('ServiceApplication'));
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

         const isFree = this.tab == 0 ? null : this.tab != 1;

         ServiceApplicationService.GetList({ ...this.filter, isFree })
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
      Cancel(item) {
         this.$swal.fire({
            icon: 'info',
            title: this.$t('WantCancel'),
            showLoaderOnConfirm: true,
            input: 'text',
            inputPlaceholder: this.$t('CancelMessage'),
            preConfirm: (msg) => {
               return ServiceApplicationService.Cancel({
                  id: item.id,
                  message: msg
               })
                  .then(() => {
                     this.makeToast(this.$t('CancelSuccess'), 'success');
                     this.Refresh();
                  })
                  .catch(this.SwalError);
            },
            allowOutsideClick: () => !this.$swal.isLoading()
         });
      },
      Delete(item) {
         this.$swal.fire({
            icon: 'info',
            title: this.$t('WantDelete'),
            showLoaderOnConfirm: true,
            preConfirm: () => {
               return ServiceApplicationService.Delete(item.id)
                  .then(() => {
                     this.makeToast(this.$t('CancelSuccess'), 'success');
                     this.Refresh();
                  })
                  .catch(this.SwalError);
            },
            allowOutsideClick: () => !this.$swal.isLoading()
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
