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
         @row-dblclicked="(e) => $router.push({ name: 'EditClaimApplication', params: { id: e.id } })"
      >
         <!-- filtes -->
         <template #filter>
            <b-row class="mb-1">
               <b-col
                  ><b-button size="sm" @click="openFilter = !openFilter" variant="primary">{{
                     $t('filter')
                  }}</b-button></b-col
               >
            </b-row>
            <div v-show="openFilter">
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
                        v-model="filter.organizationId"
                        :options="OrganizationList"
                        @input="Refresh"
                        :label="$t('organization')"
                     />
                  </b-col>

                  <b-col cols="12" md="3">
                     <form-select
                        :options="ClaimThemeList"
                        placeholder="ChooseBelow"
                        label="ClaimTheme"
                        v-model="filter.claimThemeId"
                        @input="Refresh"
                     />
                  </b-col>
                  <b-col cols="12" md="3">
                     <form-select
                        :options="timeList"
                        placeholder="ChooseBelow"
                        label="timeList"
                        :disabled="timeListdisabled"
                        @change="filterTime"
                     />
                  </b-col>
                  <b-col cols="12" md="3">
                     <div>
                        <label for>{{ $t('contractorInn') }}</label>
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
                  <b-col cols="12" md="2">
                     <div>
                        <label for>{{ $t('startdate') }}</label>
                        <form-picker
                           :disabled="timedisabled"
                           v-model="filter.fromDocDate"
                           :placeholder="$t('startdate')"
                           @input="Refresh"
                           @change="changeTime"
                        />
                     </div>
                  </b-col>
                  <b-col cols="12" md="2">
                     <div>
                        <label for>{{ $t('enddate') }}</label>
                        <form-picker
                           v-model="filter.toDocDate"
                           :disabled="timedisabled"
                           :placeholder="$t('enddate')"
                           @change="changeTime"
                           @input="Refresh"
                        />
                     </div>
                  </b-col>
                  <b-col cols="12" md="2">
                     <form-select
                        :options="BankList"
                        placeholder="ChooseBelow"
                        label="Bank"
                        v-model="filter.contractorId"
                        @input="Refresh"
                     />
                  </b-col>
               </b-row>
            </div>
            <b-row>
               <b-col md="9"></b-col>
               <b-col cols="12" md="3">
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
            <!-- rahbar -->
            <b-row align-v="center" v-show="visible" class="mt-2">
               <b-col cols="12" md="11" class="mb-1 justify">
                  <b-button-group size="sm " class="d-flex flex-wrap">
                     <b-button
                        size="sm"
                        @click="handledata(null, null, null)"
                        :variant="activebtn == null ? 'primary' : 'outline-primary'"
                        >{{ $t('all') }}</b-button
                     >
                     <b-button
                        @click="handledata(null, 6, 1)"
                        :variant="activebtn == 1 ? 'primary' : 'outline-primary'"
                        >{{ $t('Yangi') }}</b-button
                     >

                     <b-button
                        @click="handledata(null, 7, 3)"
                        :variant="activebtn == 3 ? 'primary' : 'outline-primary'"
                        >{{ $t('MEDIATION_PLAN_CREATE') }}</b-button
                     >
                     <b-button
                        @click="handledata(null, 10, 4)"
                        :variant="activebtn == 4 ? 'primary' : 'outline-primary'"
                        >{{ $t('MEDIATION_CREATE') }}</b-button
                     >

                     <b-button
                        @click="handledata(null, 11, 6)"
                        :variant="activebtn == 6 ? 'primary' : 'outline-primary'"
                        >{{ $t('APPLICATION_FOR_COURT_CREATE') }}
                     </b-button>
                     <b-button
                        @click="handledata(null, 12, 5)"
                        :variant="activebtn == 5 ? 'primary' : 'outline-primary'"
                        >{{ $t('ACCEPT_THAT_EMPLOYEE') }}
                     </b-button>
                     <b-button
                        @click="handledata(null, 9, 7)"
                        :variant="activebtn == 7 ? 'primary' : 'outline-primary'"
                     >
                        {{ $t('ACCEPT') }}
                     </b-button>
                     <b-button
                        @click="handledata(null, 13, 8)"
                        :variant="activebtn == 8 ? 'primary' : 'outline-primary'"
                        >{{ $t('CLAIM_APPLICATION_CANCEL') }}
                     </b-button>
                     <b-button
                        @click="handledata(25, null, 9)"
                        :variant="activebtn == 9 ? 'primary' : 'outline-primary'"
                        >{{ $t('rejects') }}
                     </b-button>
                  </b-button-group>
               </b-col>
            </b-row>
            <!-- xodim -->
            <b-row align-v="center" v-show="!visible">
               <b-col cols="12" md="11" class="mb-2">
                  <b-button-group size="sm">
                     <b-button
                        @click="handledata(null, null, null)"
                        :variant="activebtn == null ? 'primary' : 'outline-primary'"
                        >{{ $t('all') }}</b-button
                     >
                     <b-button
                        @click="handledata(null, 7, 2)"
                        :variant="activebtn == 2 ? 'primary' : 'outline-primary'"
                        >{{ $t('EXECUTING2') }}</b-button
                     >
                     <b-button
                        @click="handledata(null, 10, 3)"
                        :variant="activebtn == 3 ? 'primary' : 'outline-primary'"
                     >
                        {{ $t('MEDIATION_PLAN_CREATE2') }}</b-button
                     >
                     <b-button
                        @click="handledata(null, 11, 4)"
                        :variant="activebtn == 4 ? 'primary' : 'outline-primary'"
                        >{{ $t('MEDIATION_CREATE2') }}</b-button
                     >
                     <b-button
                        @click="handledata(null, 12, 5)"
                        :variant="activebtn == 5 ? 'primary' : 'outline-primary'"
                        >{{ $t('APPLICATION_FOR_COURT_CREATE2') }}
                     </b-button>
                     <b-button @click="handledata(null, 8, 6)" :variant="activebtn == 6 ? 'primary' : 'outline-primary'"
                        >{{ $t('ACCEPT_THAT_EMPLOYEE2') }}
                     </b-button>
                     <b-button
                        @click="handledata(null, 13, 8)"
                        :variant="activebtn == 8 ? 'primary' : 'outline-primary'"
                        >{{ $t('CLAIM_APPLICATION_CANCEL2') }}
                     </b-button>
                     <b-button @click="handledata(null, 9, 7)" :variant="activebtn == 7 ? 'primary' : 'outline-primary'"
                        >{{ $t('ACCEPT2') }}
                     </b-button>
                  </b-button-group>
               </b-col>
            </b-row>
         </template>
         <template #cell(actions)="{ item }">
            <div class="text-center" style="text-wrap: nowrap">
               <b-link
                  v-if="filter.stepId == 10 || filter.stepId == 11 || filter.stepId == 12"
                  @click="sendToMedetion(item)"
                  v-b-tooltip.hover.top="$t('send')"
                  style="margin-right: 5px"
               >
                  <feather-icon icon="CornerRightUpIcon"></feather-icon>
               </b-link>

               <!-- Edit -->
               <b-link
                  :to="{ name: 'EditClaimApplication', params: { id: item.id } }"
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
            <b-badge
               :variant="
                  getColor({ statusId: item.application.currentStepId, status: item.application.step, step: true })
               "
            >
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
         <template #cell(message)="{ item }">
            {{ item.application.message || '-' }}
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
      StatusSelect
   },
   directives: {
      'b-tooltip': VBTooltip,
      'b-modal': VBModal
   },
   data() {
      return {
         openModal: false,
         openFilter: false,
         timeListdisabled: false,
         timedisabled: false,
         visible: false,
         localStorageData: {},
         stepId: null,
         StatusId: null,
         activebtn: null,
         items: [],
         BankList: [],
         ContractorActivityTypeList: [],
         RegionList: [],
         ClaimThemeList: [],
         ClaimApplicationTypeSelectList: [],
         DistrictList: [],
         OrganizationList: [],
         timeList: [
            {
               value: 1,
               text: this.$t('oneWeek'),
               orderCode: '01'
            },
            {
               value: 2,
               text: this.$t('twoWeek'),
               orderCode: '02'
            },
            {
               value: 3,
               text: this.$t('threeWeek'),
               orderCode: '03'
            },
            {
               value: 4,
               text: this.$t('month'),
               orderCode: '04'
            }
         ],

         filter: {
            search: '',
            forSecondGetList: false,
            calimAppType: false,
            isEmployee: false,
            stepId: null,
            statusId: null,
            sortBy: '',
            orderType: 'asc',
            page: 1,
            pageSize: 20,
            perPageOptions: [10, 20, 50, 100, 300],
            total: 0,
            contractorInn: '',
            regionId: null,
            districtId: null,
            claimApplicationTypeId: null,
            claimThemeId: null,
            fromDocDate: '',
            toDocDate: '',
            statusIds: [],
            statusId: null,
            oneWeek: false,
            twoWeek: false,
            threeWeek: false,
            month: false
         },
         fields: [],
         isBusy: false,
         IntRegionList: []
      };
   },
   created() {
      this.filter = JSON.parse(localStorage.getItem('claimFilter'))
         ? JSON.parse(localStorage.getItem('claimFilter'))
         : {
              search: '',
              forSecondGetList: false,
              calimAppType: false,
              isEmployee: false,
              stepId: null,
              statusId: null,
              sortBy: '',
              orderType: 'asc',
              page: 1,
              pageSize: 20,
              perPageOptions: [10, 20, 50, 100, 300],
              total: 0,
              contractorInn: '',
              regionId: null,
              districtId: null,
              claimApplicationTypeId: null,
              claimThemeId: null,
              fromDocDate: '',
              toDocDate: '',
              statusIds: [],
              statusId: null,
              oneWeek: false,
              twoWeek: false,
              threeWeek: false,
              month: false
           };
      this.activebtn = JSON.parse(localStorage.getItem('activebtn'));
      ManualService.OrganizationAsSelectListByGroup([1, 3]).then((res) => {
         this.OrganizationList = res.data;
      });
      this.localStorageData = JSON.parse(localStorage.getItem('user_info'));
      if (this.localStorageData.positionCategoryId == 1 || this.localStorageData.id == 1) {
         this.filter.isEmployee = false;
         this.visible = true;
      } else {
         this.filter.isEmployee = true;
         this.visible = false;
      }
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
      ManualService.ContractorSelectList().then((res) => {
         this.BankList = res.data;
      });
      ManualService.ClaimApplicationTypeSelectList().then((res) => {
         if (Array.isArray(res.data)) {
            this.ClaimApplicationTypeSelectList = res.data;
         }
      });
   },
   methods: {
      showModal(item) {
         this.openModal = true;
         console.log(item);
      },
      sendToMedetion(item) {
         ClaimApplicationService.GetForInfoID(item.application.id, this.filter.stepId).then((res) => {
            console.log(res.data);

            if (this.filter.stepId == 10) {
               this.$router.push({
                  name: 'EditMediationPlan',
                  params: { id: res.data.mediationPlan.id }
               });
            }
            if (this.filter.stepId == 11) {
               this.$router.push({
                  name: 'EditMediation',
                  params: {
                     id: res.data.mediation.id
                  }
               });
            }
            if (this.filter.stepId == 12) {
               this.$router.push({
                  name: 'EditApplicationForCourt',
                  params: {
                     id: res.data.applicationForCourt.id
                  }
               });
            }
         });
      },

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
               key: 'claimApplicationType',
               label: this.$t('ClaimApplicationType')
            },
            {
               key: 'region',
               label: this.$t('region')
            },
            {
               key: 'organizationName',
               label: this.$t('organization'),
               thStyle: {
                  minWidth: '300px'
               }
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
               sortable: true
            },
            {
               key: 'message',
               label: this.$t('message1'),
               thClass: 'text-center',
               tdClass: 'text-center'
            },
            {
               key: 'step',
               label: this.$t('step'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true,
               stickyColumn: !this.isMobileDevice(),
               thStyle: {
                  right: !this.isMobileDevice() ? '0' : ''
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
               // console.log(this.items);
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
         this.filter.stepId = stepId;
         this.activebtn = activebtn;
         this.Refresh();
      },
      filterTime(e) {
         this.timedisabled = true;
         this.timeListdisabled = false;

         if (e == 1) {
            this.filter.oneWeek = true;
            this.filter.twoWeek = false;
            this.filter.threeWeek = false;
            this.filter.month = false;
         }
         if (e == 2) {
            this.filter.oneWeek = false;
            this.filter.twoWeek = true;
            this.filter.threeWeek = false;
            this.filter.month = false;
         }
         if (e == 3) {
            this.filter.oneWeek = false;
            this.filter.twoWeek = false;
            this.filter.threeWeek = true;
            this.filter.month = false;
         }
         if (e == 4) {
            this.filter.oneWeek = false;
            this.filter.twoWeek = false;
            this.filter.threeWeek = false;
            this.filter.month = true;
         }
         if (e == null) {
            this.timedisabled = false;
            this.timeListdisabled = false;
            this.filter.oneWeek = false;
            this.filter.twoWeek = false;
            this.filter.threeWeek = false;
            this.filter.month = false;
         }
         this.Refresh();
      },
      changeTime(e) {
         if (e) {
            this.timedisabled = false;
            this.timeListdisabled = true;
         } else {
            if (!this.filter.toDocDate && !this.filter.fromDocDate) {
               this.timedisabled = false;
               this.timeListdisabled = false;
            }
         }
      }
   },
   watch: {
      filter: {
         handler(val) {
            localStorage.setItem('claimFilter', JSON.stringify(val));
            localStorage.setItem('activebtn', JSON.stringify(this.activebtn));
         },
         deep: true
      }
   }
};
</script>
<style>
.r-0 {
   right: 0;
}
</style>
