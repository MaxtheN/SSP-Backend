<template>
   <form-table-hrm
      :items="items"
      :actions="{}"
      :fields="fields"
      :filter.sync="filter"
      searchable
      :busy="isBusy"
      @request="Refresh"
      @row-dblclicked="(e) => $router.push({ name: 'EditClaimApplication2', params: { id: e.id } })"
   >
      <!-- filtes -->
      <template #filter>
         <b-row>
            <b-col cols="12" md="3">
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
            <b-col cols="12" md="2">
               <form-select
                  :options="ClaimApplicationTypeSelectList"
                  placeholder="ChooseBelow"
                  label="ClaimApplicationType"
                  v-model="filter.claimApplicationTypeId"
                  @input="Refresh"
            /></b-col>
            <b-col cols="12" md="3">
               <b-input-group class="mt-2">
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
            <b-col cols="12" md="6" class="">
               <b-button-group size="sm" class="d-flex flex-wrap">
                  <b-button
                     @click="(filter.statusId = null), Refresh()"
                     :variant="filter.statusId == null ? 'primary' : 'outline-primary'"
                     >{{ $t('all') }}</b-button
                  >
                  <b-button
                     @click="(filter.statusId = 1), Refresh()"
                     :variant="filter.statusId == 1 ? 'primary' : 'outline-primary'"
                     >{{ $t('new') }}</b-button
                  >
                  <b-button
                     @click="(filter.statusId = 8), Refresh()"
                     :variant="filter.statusId == 8 ? 'primary' : 'outline-primary'"
                  >
                     {{ $t('Imzoga yuborilgan') }}</b-button
                  >
                  <b-button
                     @click="(filter.statusId = 2), Refresh()"
                     :variant="filter.statusId == 2 ? 'primary' : 'outline-primary'"
                     >{{ $t('Tasdiqlangan') }}</b-button
                  >
                  <b-button
                     @click="(filter.statusId = 24), Refresh()"
                     :variant="filter.statusId == 24 ? 'primary' : 'outline-primary'"
                     >{{ $t('CLAIM_APPLICATION_CANCEL2') }}
                  </b-button>
               </b-button-group>
            </b-col>
            <b-col cols="12" md="2" class="">
               <div class="d-flex align-items-center">
                  <span class="font-weight-bold mr-2"> {{ $t('Person') }} </span>
                  <b-form-checkbox
                     @change="Refresh"
                     v-model="filter.isIndividual"
                     name="is-menu-visible"
                     class="mr-0"
                     switch
                     inline
                  />
               </div>
            </b-col>
            <b-col cols="12" md="4">
               <form-select
                  :options="BankList"
                  placeholder="ChooseBelow"
                  label="Bank"
                  v-model="filter.contractorId"
                  @input="Refresh"
               />
            </b-col>
         </b-row>
      </template>
      <template #cell(actions)="{ item }">
         <div class="text-center" style="text-wrap: nowrap">
            <!-- Edit -->
            <b-link
               :to="{ name: 'EditClaimApplication2', params: { id: item.id } }"
               v-b-tooltip.hover.top="$t('View')"
               style="margin-right: 5px"
            >
               <feather-icon icon="EyeIcon"></feather-icon>
            </b-link>
            <!-- document history -->
            <HistoryModalButton :id="item.id" :table-id="item.tableId" />
         </div>
      </template>

      <template #cell(contractor)="{ item }">
         <span style="color: blue; cursor: pointer" @click="goToBussnes(item.application.contractorInn)">
            {{ item.application.contractorInn }}
         </span>
         -
         {{ item.application.contractor }}
      </template>
      <template #cell(employeeManage)="{ item }">
         {{ item.employeeManageId ? item.employeeManage : '-' }}
      </template>
      <template #cell(id)="{ item }">
         {{ item.application.id }}
      </template>
      <template #cell(durationGivenPerformer)="{ item }">
         {{ item.employeeManageId ? item.durationGivenPerformer : '-' }}
      </template>
      <template #cell(step)="{ item }">
         <b-badge :variant="getColor({ statusId: item.application.currentStepId, status: item.application.step })">
            <!-- xodim -->
            <template v-if="filter.isEmployee">
               {{
                  item.application.currentStepId == 7
                     ? $t('EXECUTING2')
                     : item.application.currentStepId == 10
                     ? $t('MEDIATION_PLAN_CREATE2')
                     : item.application.step
               }}
            </template>
            <!-- rahbar  -->
            <template v-else> {{ item.application.step }} </template>
         </b-badge>
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
import ClaimApplicationService from '@/services/document/claimapplication.service';
import HistoryModalButton from '@/views/components/document/HistoryModalButton.vue';
import RegionService from '@/services/info/region.service';
import ClaimThemeService from '@/services/info/claimtheme.service';
import ManualService from '@/services/others/manual.service';
import StatusSelect from '@/views/components/document/StatusSelect.vue';
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
      HistoryModalButton,
      BInputGroup,
      BInputGroupAppend,
      BRow,
      BCol,
      BButtonGroup,
      BFormInput,
      StatusSelect,
      BFormCheckbox
   },
   directives: {
      'b-tooltip': VBTooltip,
      'b-modal': VBModal
   },
   data() {
      return {
         visible: true,
         localStorageData: {},
         stepId: null,
         StatusId: null,
         activebtn: null,
         items: [],
         ContractorActivityTypeList: [],
         RegionList: [],
         ClaimThemeList: [],
         ClaimApplicationTypeSelectList: [
            {
               value: 1,
               text: this.$t('Заявлениевадминистративныйсуд'),
               orderCode: '001'
            },

            {
               value: 3,
               text: this.$t('Апелляция/Кассационная жалоба'),
               orderCode: '004'
            },
            {
               value: 4,
               text: this.$t('Проверка - ревизия'),
               orderCode: '004'
            },
            {
               value: 5,
               text: this.$t('Подача встречного иска'),
               orderCode: '005'
            }
         ],
         DistrictList: [],
         BankList: [],
         filter: {
            search: '',
            isIndividual: false,
            isEmployee: true,
            stepId: null,
            statusId: null,
            sortBy: '',
            orderType: 'asc',
            page: 1,
            pageSize: 20,
            perPageOptions: [10, 20, 50, 100, 300],
            total: 0,
            contractorInn: '',
            forSecondGetList: true,
            regionId: null,
            districtId: null,
            claimApplicationTypeId: null,
            claimThemeId: null,
            fromDocDate: '',
            toDocDate: '',
            statusIds: [],
            statusId: null,
            calimAppType: true
         },
         fields: [],
         isBusy: false
      };
   },
   created() {
      let localStorageData = localStorage.getItem('user_info');
      localStorageData = JSON.parse(localStorageData);
      this.localStorageData = localStorageData;
      if (this.localStorageData.positionCategoryId == 1 || this.localStorageData.id == 1) {
         this.filter.isEmployee = false;
         this.visible = true;
      } else {
         this.filter.isEmployee = true;
         this.visible = false;
      }
      ManualService.ContractorSelectList().then((res) => {
         this.BankList = res.data;
      });
   },
   mounted() {
      RegionService.GetAsSelectList(211).then((res) => {
         this.RegionList = res.data;
      });
      ClaimThemeService.GetAsSelectList().then((res) => {
         if (Array.isArray(res.data)) {
            this.ClaimThemeList = res.data;
         }
      });
   },
   methods: {
      GetFields() {
         this.fields = [
            {
               key: 'actions',
               label: this.$t('actions'),
               thClass: 'text-center',
               tdClass: 'text-center',
               stickyColumn: true
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
               key: 'claimTheme',
               label: this.$t('ClaimTheme'),
               thStyle: {
                  minWidth: '300px'
               }
            },

            {
               key: 'region',
               label: this.$t('region')
            },

            {
               key: 'employeeManage',
               label: this.$t('ijrochi hodim'),
               thStyle: {
                  minWidth: '300px'
               }
            },
            {
               key: 'durationGivenPerformer',
               label: this.$t('ijro muddati')
            },

            {
               key: this.filter.isEmployee ? '' : 'status',
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
         ];
      },
      goToBussnes(inn) {
         this.$router.push({ name: 'BusinessmanCard', query: { inn: inn } });
      },
      Refresh() {
         this.GetFields();
         this.isBusy = true;
         if (!this.filter.statusId) {
            this.filter.statusIds = [];
         } else {
            this.filter.statusIds = [this.filter.statusId];
         }
         ClaimApplicationService.GetList(this.filter)
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
      handledata(statusId, stepId, activebtn) {
         this.filter.statusId = statusId;
         this.activebtn = activebtn;
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
