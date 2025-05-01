<template>
   <form-table-hrm
      :items="items"
      :actions="{}"
      :fields="fields"
      :filter.sync="filter"
      searchable
      :busy="isBusy"
      @row-dblclicked="DbClick"
      @request="Refresh"
   >
      <!-- filtes -->
      <template #filter>
         <b-row>
            <b-col cols="12" md="3">
               <form-select
                  :options="RegionList"
                  v-model="filter.contractorRegionId"
                  @input="ChangeRegion"
                  label="Oblast"
               ></form-select>
            </b-col>
            <b-col cols="12" md="3">
               <form-select
                  :options="DistrictList"
                  :reduce="(item) => item.value"
                  label="Region"
                  v-model="filter.contractorDistrictId"
                  @input="ChangeDistrict"
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
                        <b-button @click="Refresh" size="sm" variant="primary">
                           <feather-icon icon="SearchIcon" />
                        </b-button>
                     </b-input-group-append>
                  </b-input-group>
               </div>
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
               <label for>{{ $t('search') }}</label>
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
         <b-row align-v="center">
            <b-col cols="12" md="8" v-if="!hideStatus">
               <StatusSelect v-model="filter.statusId" @input="Refresh" :filter="[1, 24, 27, 21]" />
            </b-col>
         </b-row>
      </template>
      <template #cell(contractor)="{ item }">
         <span style="color: blue; cursor: pointer" @click="goToBussnes(item.contractorInn)">{{
            item.contractorInn
         }}</span>
         -
         {{ item.contractorFullName }}
      </template>
      <template #cell(actions)="{ item }">
         <div class="text-center" style="text-wrap: nowrap">
            <!-- View -->
            <b-link
               :to="{ name: 'EditServiceContract', params: { id: item.id } }"
               v-b-tooltip.hover.top="$t('View')"
               class="mr-1"
            >
               <feather-icon icon="EyeIcon"></feather-icon>
            </b-link>

            <!-- memshippayment -->
            <b-link
               v-if="item.canCreatePaymentOrder"
               :to="{ name: 'EditMemshipPaymentOrder', params: { id: 0 }, query: { appId: item.id } }"
               v-b-tooltip.hover.top="$t('MemshipPaymentOrder')"
               class="mr-1"
            >
               <feather-icon icon="FileTextIcon"></feather-icon>
            </b-link>

            <!-- document history -->
            <HistoryModalButton :id="item.id" :table-id="item.tableId || 99" />
         </div>
      </template>
      <template #cell(price)="{ item }">
         {{ currency(item.price) }}
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
   BSpinner,
   BRow,
   BCol,
   BFormInput,
   BInputGroup,
   BButtonGroup,
   BInputGroupAppend
} from 'bootstrap-vue';
import FormTableHrm from '@/components/forms/form-table-hrm.vue';
import ServiceContractService from '@/services/srv/ServiceContract.service';
import HistoryModalButton from '@/views/components/document/HistoryModalButton.vue';
import StatusSelect from '@/views/components/document/StatusSelect.vue';
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
      BSpinner,
      BRow,
      BCol,
      BInputGroup,
      BButtonGroup,
      BFormInput,
      BInputGroupAppend,
      HistoryModalButton,
      StatusSelect
   },
   directives: {
      'b-tooltip': VBTooltip,
      'b-modal': VBModal
   },
   props: {
      selectable: {
         type: Boolean,
         default: false
      },
      statusId: {
         type: Number,
         default: null
      },
      hideStatus: {
         type: Boolean,
         default: false
      }
   },
   data() {
      return {
         items: [],
         RegionList: [],
         DistrictList: [],
         filter: {
            contractorRegionId: null,
            contractorDistrictId: null,
            statusId: this.statusId || null,
            organizationId: null,
            contractorInn: null,
            search: '',
            sortBy: '',
            orderType: 'asc',
            page: 1,
            pageSize: 20,
            perPageOptions: [10, 20, 50, 100],
            total: 0
         },
         isBusy: false
      };
   },
   computed: {
      fields() {
         return [
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
               key: 'contractorRegion',
               label: this.$t('region')
            },
            {
               key: 'contractorDistrict',
               label: this.$t('District')
            },
            {
               key: 'contractor',
               label: this.$t('contractor')
            },
            {
               key: 'price',
               label: this.$t('price3'),
               tdClass: 'text-right',
               thStyle: { minWidth: '200px' }
            },
            {
               key: 'status',
               label: this.$t('status'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            }
         ];
      }
   },
   created() {
      RegionService.GetAsSelectList(211).then((res) => {
         this.RegionList = res.data;
      });
   },
   methods: {
      ChangeRegion() {
         if (this.filter.contractorRegionId) {
            this.filter.contractorDistrictId = null;
            this.GetDistrict();
         }
         this.Refresh();
      },
      GetDistrict() {
         if (this.filter.contractorRegionId) {
            DistrictService.GetAsSelectList(this.filter.contractorRegionId).then((res) => {
               this.DistrictList = res.data;
            });
         } else {
            this.filter.contractorDistrictId = null;
            this.DistrictList = [];
            this.Refresh();
         }
      },
      ChangeDistrict() {
         this.Refresh();
      },
      goToBussnes(inn) {
         if (this.$can('BusinessmanCardView', 'permissions')) {
            this.$router.push({ name: 'BusinessmanCard', query: { inn: inn } });
         }
      },
      DbClick(item) {
         if (this.selectable) {
            this.$emit('row-selected', item);
         } else {
            this.$router.push({
               name: 'EditServiceContract',
               params: { id: item.id }
            });
         }
      },
      Delete(item) {
         this.$swal.fire({
            icon: 'info',
            title: this.$t('WantDelete'),
            showLoaderOnConfirm: true,
            preConfirm: () => {
               return ServiceContractService.Delete(item.id)
                  .then(() => {
                     this.makeToast(this.$t('DeleteSuccess'), 'success');
                     this.Refresh();
                  })
                  .catch(this.SwalError);
            },
            allowOutsideClick: () => !this.$swal.isLoading()
         });
      },
      Refresh() {
         this.isBusy = true;
         ServiceContractService.GetList({ ...this.filter })
            .then((res) => {
               this.items = res.data.rows;
               this.filter.total = res.data.total;
            })
            .finally(() => {
               this.isBusy = false;
            });
      }
   }
};
</script>
