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
            <b-col cols="12" md="3" lg="2">
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
            <b-col cols="12" md="3" lg="2">
               <div>
                  <label for>{{ $t('startdate') }}</label>
                  <form-picker v-model="filter.fromDocDate" :placeholder="$t('startdate')" @input="Refresh" />
               </div>
            </b-col>
            <b-col cols="12" md="3" lg="2">
               <div>
                  <label for>{{ $t('enddate') }}</label>
                  <form-picker v-model="filter.toDocDate" :placeholder="$t('enddate')" @input="Refresh" />
               </div>
            </b-col>
         </b-row>
         <b-row align-v="center">
            <b-col cols="12" lg="9" class="mb-1" v-if="hideStep">
               <div class="button-group-mob">
                  <b-button-group @click="Refresh" size="sm">
                     <b-button
                        @click="filter.currentStepId = null"
                        :variant="filter.currentStepId == null ? 'primary' : 'outline-primary'"
                        >{{ $t('all') }}</b-button
                     >
                     <b-button
                        @click="filter.currentStepId = 16"
                        :variant="filter.currentStepId == 16 ? 'primary' : 'outline-primary'"
                        >{{ $t('totalApplicationSendToOmbusmanCount') }}</b-button
                     >
                     <b-button
                        @click="filter.currentStepId = 17"
                        :variant="filter.currentStepId == 30 ? 'primary' : 'outline-primary'"
                        >{{ $t('KQKA ga yuborilgan') }}</b-button
                     >
                     <b-button
                        @click="filter.currentStepId = 18"
                        :variant="filter.currentStepId == 18 ? 'primary' : 'outline-primary'"
                        >{{ $t('rejectedombudsman') }}</b-button
                     >
                     <b-button
                        @click="filter.currentStepId = 19"
                        :variant="filter.currentStepId == 19 ? 'primary' : 'outline-primary'"
                        >{{ $t('rejectedAnticoruptionagensy') }}</b-button
                     >
                     <b-button
                        @click="filter.currentStepId = 20"
                        :variant="filter.currentStepId == 20 ? 'primary' : 'outline-primary'"
                        >{{ $t('rejectedSPP') }}</b-button
                     >
                  </b-button-group>
               </div>
            </b-col>

            <b-col class="mb-2 col-auto align-center d-flex">
               <b-form-checkbox v-model="filter.withoutCertificate" @input="Refresh" class="mt-1">
                  {{ $t('withoutCertificate') }}
               </b-form-checkbox>
            </b-col>

            <b-col cols="12" lg="6">
               <StatusSelect v-if="!hideStatus" v-model="filter.statusId" @input="Refresh" :filter="[8, 2, 24, 25]" />
            </b-col>
            <b-col></b-col>
            <b-col cols="12" lg="4">
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
            <!-- certificateLink -->
            <b-link
               v-if="item.certificateId"
               :to="{ name: 'ViewJoinAntiCorruptionCertificate', params: { id: item.certificateId } }"
               v-b-tooltip.hover.top="$t('certificateLink')"
               class="mr-1 cursor-pointer"
            >
               <feather-icon icon="LinkIcon"></feather-icon>
            </b-link>
            <!-- Edit -->
            <b-link
               :to="{ name: 'ViewJoinAntiCorruptionApplication', params: { id: item.id } }"
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
      <template #cell(step)="{ item }">
         <b-badge
            v-if="item.application.currentStepId"
            :variant="getColor({ statusId: item.application.currentStepId, status: item.application.step })"
         >
            {{ item.application.step }}
         </b-badge>
      </template>
      <template #cell(docNumber)="{ item }">
         {{ item.application.docNumber }}
      </template>
      <template #cell(docOn)="{ item }">
         {{ item.application.docOn }}
      </template>
      <template #cell(contractorInn)="{ item }">
         {{ item.application.contractorInn }}
      </template>
      <template #cell(contractor)="{ item }">
         {{ item.application.contractor }}
      </template>
      <template #cell(contractorPhoneNumber)="{ item }">
         {{ item.application.contractorPhoneNumber }}
      </template>
      <template #cell(region)="{ item }">
         {{ item.application.region }}
      </template>
      <template #cell(district)="{ item }">
         {{ item.application.district }}
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
   BFormCheckbox
} from 'bootstrap-vue';
import FormTableHrm from '@/components/forms/form-table-hrm.vue';
import JoinAntiCorruptionApplicationService from '@/services/corruption/joinanticorruptionapplication.service';
import HistoryModalButton from '@/views/components/document/HistoryModalButton.vue';
import ContractorActivityTypeService from '@/services/hrm/contractoractivitytype.service';
import RegionService from '@/services/info/region.service';
import DistrictService from '@/services/info/district.service';
import StatusSelect from '../document/StatusSelect.vue';

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
      BFormCheckbox
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
      },
      hideStatus: {
         type: Boolean,
         default: false
      },
      hideStep: {
         type: Boolean,
         default: false
      },
      statusId: {
         type: Number,
         default: null
      },
      withoutCertificate: {
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
               key: 'avgEmployeesCount',
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
               sortable: true
            }
         ],
         filter: {
            search: '',
            currentStepId: null,
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
            statusId: this.statusId,
            withoutCertificate: this.withoutCertificate
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
         JoinAntiCorruptionApplicationService.GetList({ ...this.filter })
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
      UploadFile(event) {
         const formData = new FormData();
         formData.append('files', event.target.files[0]);
         this.fileLoading = true;
         MemshipApplicationService.UploadFiles(formData)
            .then((res) => {
               this.Data.files.push(...res.data);
            })
            .catch(this.showApiError);
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
               name: 'ViewJoinAntiCorruptionApplication',
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
               return JoinAntiCorruptionApplicationService.Delete(item.id)
                  .then(() => {
                     this.makeToast(this.$t('DeleteSuccess'), 'success');
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
