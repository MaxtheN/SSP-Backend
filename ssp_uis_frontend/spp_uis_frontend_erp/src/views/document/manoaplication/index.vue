<template>
   <div>
      <form-table-hrm
         :items="items"
         :fields="fields"
         :filter.sync="filter"
         :busy="isBusy"
         :actions="{}"
         @row-dblclicked="DbClick"
         @request="Refresh"
      >
         <template #filter>
            <b-row>
               <b-col cols="12" md="3" lg="3">
                  <form-select
                     :options="RegionList"
                     v-model="filter.regionId"
                     @input="Refresh"
                     @change="ChangeRegion"
                     :label="$t('Oblast')"
                  ></form-select>
               </b-col>
               <b-col cols="12" md="3" lg="3">
                  <form-select
                     :options="DistrictList"
                     v-model="filter.districtId"
                     @input="Refresh"
                     :label="$t('District')"
                  ></form-select>
               </b-col>
               <b-col cols="12" md="3" lg="3">
                  <form-picker
                     v-model="filter.fromDocDate"
                     @change="Refresh"
                     :label="$t('fromdate')"
                     :placeholder="$t('fromdate')"
                  ></form-picker>
               </b-col>
               <b-col cols="12" md="3" lg="3">
                  <form-picker
                     v-model="filter.toDocDate"
                     @change="Refresh"
                     :label="$t('todate')"
                     :placeholder="$t('todate')"
                  ></form-picker>
               </b-col>
            </b-row>
            <b-row class="mt-1 d-flex align-items-center">
               <b-col>
                  <StatusSelect v-model="filter.statusId" :filter="[1, 23, 25, 2, 24, 30]" />
               </b-col>

               <b-col cols="12" md="4">
                  <div>
                     <label for>{{ $t('search') }}</label>
                     <b-input-group>
                        <b-form-input
                           type="text"
                           v-model="filter.search"
                           debounce="300"
                           @keyup.enter="Refresh"
                           :placeholder="$t('search')"
                        />
                        <b-input-group-append>
                           <b-button @click="Refresh" size="sm" variant="primary">
                              <feather-icon icon="SearchIcon" />
                           </b-button>
                        </b-input-group-append>
                     </b-input-group>
                  </div>
               </b-col>
            </b-row>
         </template>
         <template #cell(actions)="{ item }">
            <div class="text-center" style="text-wrap: nowrap">
               <b-link
                  :to="{ name: 'monoaplicationView', params: { id: item.id } }"
                  v-b-tooltip.hover.top="$t('View')"
                  class="mr-1"
               >
                  <feather-icon icon="EyeIcon"></feather-icon>
               </b-link>
               <b-link
                  v-if="item.bandlikResponseStatusId == 2"
                  @click="ViewInfo(item)"
                  v-b-tooltip.hover.top="$t('View')"
                  class="mr-1"
               >
                  <feather-icon icon="FileIcon"></feather-icon>
               </b-link>
               <b-link
                  v-if="item.bandlikResponseStatusId == 1"
                  @click="ViewInfo(item)"
                  v-b-tooltip.hover.top="$t('View')"
                  class="mr-1"
               >
                  <feather-icon icon="FileIcon"></feather-icon>
               </b-link>
            </div>
         </template>
         <template #cell(application.contractor)="{ item }">
            <span style="color: blue; cursor: pointer">
               {{ item.application.contractorInn }}
            </span>
            -
            {{ item.application.contractor }}
         </template>
         <template #cell(totalAmount)="{ item }">
            {{ currency(item.totalAmount) }}
         </template>
         <template #cell(bandlikResponseStatus)="{ item }">
            <b-badge :variant="getColor({ statusId: item.bandlikResponseStatusId })">
               {{ item.bandlikResponseStatus }}</b-badge
            >
         </template>
      </form-table-hrm>
      <b-modal v-model="InfoModal" :cancel-title="$t('Cancel')" cancel-variant="danger">
         <template #modal-title> {{ $t('Info') }} {{ InfoList.applicationId }} </template>
         <b-card-text>
            <h5>{{ $t('responsibleFio') }} : {{ InfoList.responsibleFio }}</h5>
            <h5>{{ $t('phone') }} : {{ InfoList.responsiblePhone }}</h5>
            <h5 v-if="InfoList.rejectReason">{{ $t('rejectReason') }} : {{ InfoList.rejectReason }}</h5>
            <h5 v-if="InfoList.subsidyAmount">{{ $t('subsidyAmount') }} : {{ InfoList.subsidyAmount }}</h5>
         </b-card-text></b-modal
      >
   </div>
</template>

<script>
import FormTableHrm from '@/components/forms/form-table-hrm.vue';
import {
   BCard,
   BCardText,
   BButton,
   BBadge,
   BLink,
   BFormInput,
   BInputGroup,
   BInputGroupAppend,
   BRow,
   BCol,
   VBTooltip,
   BModal,
   VBModal,
   BButtonGroup
} from 'bootstrap-vue';

import MonoAplicationService from '@/services/monoaplication/monoaplication.service';
import RegionService from '@/services/info/region.service';
import DistrictService from '@/services/info/district.service';
import StatusSelect from '@/views/components/document/StatusSelect.vue';

export default {
   components: {
      BCard,
      StatusSelect,
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
      BButtonGroup
   },
   directives: {
      'b-tooltip': VBTooltip,
      'b-modal': VBModal
   },
   data() {
      return {
         items: [],
         InfoList: {},
         InfoModal: false,
         RegionList: [],
         DistrictList: [],
         filter: {
            search: '',
            sortBy: '',
            orderType: 'asc',
            page: 1,
            pageSize: 20,
            fromDocDate: '',
            toDocDate: '',
            regionId: '',
            districtId: '',
            statusId: null,
            statusIds: [0]
         },
         fields: [
            {
               key: 'actions',
               label: this.$t('actions'),
               thClass: 'text-center',
               tdClass: 'text-center',
               stickyColumn: !this.isMobileDevice()
            },
            {
               key: 'application.id',
               label: this.$t('id'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'application.docNumber',
               label: this.$t('docnumber'),
               thClass: 'text-center',
               sortable: true
            },
            {
               key: 'application.docOn',
               label: this.$t('docOn'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'totalAmount',
               label: this.$t('totalAmount'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true,
               thStyle: {
                  minWidth: '200px'
               }
            },
            {
               key: 'application.contractor',
               label: this.$t('contractor'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true,
               thStyle: {
                  minWidth: '400px'
               }
            },

            {
               key: 'application.contractorPhoneNumber',
               label: this.$t('phoneNumber')
            },
            {
               key: 'application.region',
               label: this.$t('region')
            },
            {
               key: 'application.district',
               label: this.$t('District')
            },
            {
               key: 'bandlikResponseStatus',
               label: this.$t('bandlikResponseStatus'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true,
               stickyColumn: !this.isMobileDevice(),
               thStyle: {
                  right: this.isMobileDevice() ? '' : '0'
               },
               tdClass: 'r-0'
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
         isBusy: false
      };
   },
   created() {
      RegionService.GetAsSelectList(211).then((res) => {
         this.RegionList = res.data;
      });
   },

   methods: {
      ViewInfo(item) {
         MonoAplicationService.GetByMonoAppId(item.id).then((res) => {
            this.InfoList = res.data;
            this.InfoModal = true;
         });
      },
      DbClick() {},
      Refresh() {
         this.isBusy = true;
         MonoAplicationService.GetList(this.filter)
            .then((res) => {
               this.items = res.data.rows;
               this.filter.total = res.data.total;
            })
            .finally(() => {
               this.isBusy = false;
            });
      },
      getDistrictList(id) {
         DistrictService.GetAsSelectList(id).then((res) => {
            this.DistrictList = res.data;
         });
      },
      ChangeRegion(id) {
         if (id) {
            this.getDistrictList(id);
         } else {
            this.filter.districtId = null;
         }
      }
   },
   watch: {
      'filter.statusId': function (newVal) {
         if (newVal == null) {
            this.filter.statusIds = [0];
            this.Refresh();
         } else {
            this.filter.statusIds[0] = newVal;
            this.Refresh();
         }
      }
   }
};
</script>
