<template>
   <div>
      <b-card no-body
         ><b-row class="m-1">
            <b-col sm="12" md="3" v-if="localStorageData?.organizationId == 1">
               <label for>{{ $t('Oblast') }}</label>
               <v-select
                  :reduce="(item) => item.value"
                  :options="RegionList"
                  :placeholder="$t('ChooseBelow')"
                  label="text"
                  v-model="filter.regionId"
                  @input="ChangeRegion"
                  class="w-100"
               ></v-select>
            </b-col>
            <b-col sm="12" md="3">
               <form-select
                  :options="Positionlist"
                  :placeholder="$t('ChooseBelow')"
                  @input="ChangePosition"
                  valueid="positionId"
                  label="position"
                  valuename="positionName"
                  class="w-100"
               ></form-select>
            </b-col>

            <b-col sm="12" md="2">
               <form-picker :label="$t('startDate1')" v-model="filter.fromDate" @change="Refresh" />
            </b-col>
            <b-col sm="12" md="2">
               <form-picker :label="$t('endDate1')" v-model="filter.toDate" @change="Refresh" />
            </b-col>

            <b-col class="text-right mt-2 cols-auto">
               <b-button @click="Print" variant="primary">
                  <feather-icon icon="PrinterIcon"></feather-icon>
                  {{ $t('Print') }}
               </b-button>
            </b-col>
         </b-row>
         <b-breadcrumb class="mb-1 ml-1">
            <b-breadcrumb-item
               active
               @click="
                  () => {
                     filter.regionId = null;
                     filter.byRegion = false;
                     filter.organizationId = null;
                     filter.byOrganization = true;
                     filter.departmentId = null;
                     filter.byDepartment = false;
                     filter.positionId = null;
                     filter.byPosition = false;
                     filter.region = '';
                     Refresh();
                  }
               "
            >
               <b>{{ $t('uzb') }}</b>
            </b-breadcrumb-item>
            <b-breadcrumb-item
               :active="filter.byDistrict"
               v-show="filter.byOrganization == true && filter.region"
               @click="
                  () => {
                     filter.byRegion = false;
                     filter.organizationId = null;
                     filter.byOrganization = true;
                     filter.departmentId = null;
                     filter.byDepartment = false;
                     filter.positionId = null;
                     filter.byPosition = false;
                     Refresh();
                  }
               "
            >
               <b>{{ filter.region }}</b>
            </b-breadcrumb-item>
            <b-breadcrumb-item
               v-show="filter.byDepartment"
               :active="filter.byDepartment"
               @click="
                  () => {
                     filter.byRegion = false;
                     filter.byPosition = false;
                     filter.byOrganization = false;
                     filter.byDepartment = true;
                     filter.departmentId = null;
                     Refresh();
                  }
               "
            >
               <b>{{ filter.organization }}</b>
            </b-breadcrumb-item>

            <b-breadcrumb-item v-show="filter.byPosition || filter.byEmployee" :active="false">
               <b>{{ filter.department }}</b>
            </b-breadcrumb-item>
         </b-breadcrumb>
         <div class="mb-2">
            <b-table
               :items="items"
               :busy="isBusy"
               :fields="fields"
               primary-key="id"
               no-border-collapse
               show-empty
               foot-clone
               responsive
               no-footer-sorting
               :empty-text="$t('NotFound')"
               class="position-relative"
            >
               <template #cell(order)="{ index }">
                  <span>{{ index + 1 }}</span>
               </template>
               <template #cell(region)="{ item }">
                  <span style="color: blue; cursor: pointer" @click="SortRegion(item)">{{ item.region }}</span>
               </template>
               <template #cell(organization)="{ item }">
                  <span style="color: blue; cursor: pointer" @click="SortOrganization(item)">{{
                     item.organization
                  }}</span>
               </template>

               <template #cell(department)="{ item }">
                  <span style="color: blue; cursor: pointer" @click="SortPosition(item)">{{ item.department }}</span>
               </template>

               <template #cell(totalStaffingRate)="{ item }">
                  {{ item.totalStaffingRate ? item.totalStaffingRate : 0 }}
               </template>

               <template #cell(totalEmployeeManageRate)="{ item }">
                  <div>
                     {{ item.totalEmployeeManageRate ? item.totalEmployeeManageRate : 0 }}
                  </div>
               </template>

               <template #cell(totalCount)="{ item }">
                  <div>
                     {{ item.totalCount ? item.totalCount : 0 }}
                  </div>
               </template>

               <template #foot(totalStaffingRate)>
                  <div style="text-align: right !important">{{ totals.totalStaffingRate }}</div>
               </template>

               <template #foot(totalEmployeeManageRate)>
                  <div style="text-align: right !important">{{ totals.totalEmployeeManageRate }}</div>
               </template>
               <template #foot(totalCount)>
                  <div style="text-align: right !important">
                     {{ totals.totalCount }}
                  </div>
               </template>

               <template #foot(order)>
                  <span>{{ $t('Total') }}</span>
               </template>
               <template #foot(district)>
                  <span></span>
               </template>
               <template #foot(region)>
                  <span></span>
               </template>
               <template #foot(contractor)>
                  <span></span>
               </template>
               <template #foot(department)>
                  <span></span>
               </template>
            </b-table>
         </div>
      </b-card>
   </div>
</template>

<script>
import StaffingService from '@/services/hrm/staffing.service';
import RegionService from '@/services/info/region.service';
import ReportService from '@/services/report/report.service';
import DistrictService from '@/services/info/district.service';
import EmployeeTurnstileReportService from '@/services/hrm/employeeturnstilereport.service';
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
   BInputGroupAppend,
   BLink,
   BModal,
   BCardText,
   BTableSimple,
   BThead,
   BTbody,
   BTr,
   BTd,
   BTh,
   BButtonGroup,
   BFormCheckbox,
   BBreadcrumb,
   BBreadcrumbItem,
   BTfoot,
   BOverlay,
   BTabs,
   BTab
} from 'bootstrap-vue';
export default {
   components: {
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
      BInputGroupAppend,
      BLink,
      BModal,
      BCardText,
      BTableSimple,
      BThead,
      BTbody,
      BTr,
      BTd,
      BTh,
      BButtonGroup,
      BFormCheckbox,
      BBreadcrumb,
      BBreadcrumbItem,
      BTfoot,
      BOverlay,
      BTabs,
      BTab
   },
   data() {
      return {
         back: 'region',
         localStorageData: [],
         items: [],
         fields: [],
         RegionList: [],
         DistrictList: [],
         Positionlist: [],
         PrtnContractTypeList: [],
         Organizationlist: [],
         filter: {
            organization: '',
            department: '',
            region: '',
            regionId: null,
            byRegion: false,
            organizationId: null,
            byOrganization: true,
            departmentId: null,
            byDepartment: false,
            positionId: null,
            byPosition: false,
            toDate: '',
            fromDate: ''
         },
         PrintLoading: false,
         totals: {
            totalStaffingRate: 0,
            totalEmployeeManageRate: 0,
            totalCount: 0
         }
      };
   },
   created() {
      RegionService.GetAsSelectList(211)
         .then((res) => {
            this.RegionList = res.data;
         })
         .catch((error) => {
            this.showApiError(error);
         });

      StaffingService.GetAllStaffingPositionsAll().then((res) => {
         this.Positionlist = res.data;
      });
      this.getDataLocalStorage();
      this.Refresh();
   },
   methods: {
      getFields() {
         this.fields = [
            {
               key: 'order',
               label: this.$t('№'),
               thClass: 'text-center',
               tdClass: 'text-center'
            },
            {
               key: this.filter.byRegion ? 'region' : '',
               label: this.$t('Oblast'),

               stickyColumn: this.isMobileDevice() ? false : true
            },
            {
               key: this.filter.byOrganization ? 'organization' : '',
               label: this.$t('organization'),

               stickyColumn: this.isMobileDevice() ? false : true,
               thStyle: {
                  minWidth: '350px'
               }
            },
            {
               key: this.filter.byDepartment ? 'department' : '',
               label: this.$t('department'),

               stickyColumn: this.isMobileDevice() ? false : true,
               thStyle: {
                  minWidth: '350px'
               }
            },
            {
               key: this.filter.byPosition ? 'position' : '',
               label: this.$t('position'),

               stickyColumn: this.isMobileDevice() ? false : true,
               thStyle: {
                  minWidth: '350px'
               }
            },

            {
               key: 'totalStaffingRate',
               label: this.$t('totalStaffingRate'),
               thClass: 'text-center',
               tdClass: 'text-right'
            },

            {
               key: 'totalEmployeeManageRate',
               label: this.$t('totalEmployeeManageRate'),
               thClass: 'text-center',
               tdClass: 'text-right'
            },

            {
               key: 'totalCount',
               label: this.$t('totalCount2'),
               thClass: 'text-center',
               tdClass: 'text-right'
            }
         ];
      },
      GetDistrict(id) {
         DistrictService.GetAsSelectList(id)
            .then((res) => {
               this.DistrictList = res.data;
            })
            .catch((error) => {
               this.showApiError(error);
            });
      },
      ChangeRegion(id) {
         if (id) {
            this.filter.byRegion = false;
            this.filter.byOrganization = true;
            this.filter.byDepartment = false;
            this.filter.byPosition = false;

            this.filter.region = this.filter.regionId
               ? this.RegionList.filter((item) => item.value === this.filter.regionId)[0].text
               : '';
            this.Refresh();
         } else {
            this.filter.regionId = null;
            this.filter.region = '';
            this.filter.byRegion = true;
            this.filter.organizationId = null;
            this.filter.byOrganization = false;
            this.filter.departmentId = null;
            this.filter.byDepartment = false;
            this.filter.positionId = null;
            this.filter.byPosition = false;
            this.Refresh();
         }
      },
      SortChange(data) {
         this.filter.Sort = data.sortBy;
         this.filter.Order = data.sortDesc ? 'desc' : 'asc';
         this.Refresh();
      },
      SortRegion(item) {
         this.filter.byRegion = false;
         this.filter.byOrganization = true;
         this.filter.byDepartment = false;
         this.filter.byPosition = false;
         this.filter.region = item.region;
         this.filter.regionId = item.regionId;
         this.Refresh();
      },
      SortOrganization(item) {
         this.filter.byRegion = false;
         this.filter.byOrganization = false;
         this.filter.byDepartment = true;
         this.filter.byPosition = false;
         this.filter.organizationId = item.organizationId;
         this.filter.organization = item.organization;
         this.Refresh();
      },
      SortDepartment(item) {
         this.filter.byRegion = false;
         this.filter.byOrganization = false;
         this.filter.byDepartment = false;
         this.filter.byPosition = true;
         this.filter.departmentId = item.departmentId;
         this.filter.department = item.department;
         this.Refresh();
      },

      SortPosition(item) {
         this.filter.byRegion = false;
         this.filter.byOrganization = false;
         this.filter.byDepartment = false;
         this.filter.byPosition = true;
         this.filter.departmentId = item.departmentId;
         this.filter.department = item.department;
         this.Refresh();
      },
      Print() {
         this.PrintLoading = true;

         console.log(this.filter);
         EmployeeTurnstileReportService.SaveAsExcelGetStateEmploymentReport(this.filter)
            .then((res) => {
               this.PrintLoading = false;

               this.forceFileDownload(res, this.$t('GetStaffCountReport'));
            })
            .catch((error) => {
               this.PrintLoading = false;
               this.makeToast(error.response.data, 'danger');
            });
      },
      Refresh() {
         this.isBusy = true;
         if (this.localStorageData.organizationId != 1) {
            this.filter.regionId = this.localStorageData.organizationRegionId;
            if (this.localStorageData.userName == 'kadr-toshkentshahar') {
               this.filter.byDepartment = true;
               this.filter.byOrganization = false;
               this.filter.byPosition = false;
               this.filter.byRegion = false;
               this.filter.organization =
                  'O‘ZBEKISTON RESPUBLIKASI SAVDO-SANOAT PALATASI TOSHKENT SHAHRI HUDUDIY BOSHQARMASI';
               this.filter.organizationId = 196;
               this.filter.positionId = null;
               this.filter.region = 'Toshkent shahri';
               this.filter.regionId = 1;
            }
            this.filter.organizationId = this.filter.organizationId || this.localStorageData?.organizationId;
         }

         this.getFields();
         ReportService.GetStaffCountReport(this.filter)
            .then((res) => {
               // console.log(res.data);
               this.items = res.data;

               this.totals = {
                  totalStaffingRate: 0,
                  totalEmployeeManageRate: 0,
                  totalCount: 0
               };

               res.data.forEach((item) => {
                  this.totals.totalStaffingRate += item.totalStaffingRate;
                  this.totals.totalEmployeeManageRate += item.totalEmployeeManageRate;
                  this.totals.totalCount += item.totalCount;
               });

               this.isBusy = false;
            })
            .catch((error) => {
               this.isBusy = false;
               this.showApiError(error);
            });
      },
      ChangePosition(id) {
         if (id) {
            this.filter.region = null;
            this.filter.regionId = null;
            this.filter.organizationId = null;
            this.filter.departmentId = null;
            this.filter.byRegion = true;
            this.filter.regionId = null;
            this.filter.organizationId = null;
            this.filter.byOrganization = false;
            this.filter.departmentId = null;
            this.filter.byDepartment = false;
            this.filter.positionId = id;
            this.filter.byPosition = true;
            this.Refresh();
         } else {
            this.filter.region = false;
            this.filter.byRegion = true;
            this.filter.regionId = null;
            this.filter.organizationId = null;
            this.filter.byOrganization = false;
            this.filter.departmentId = null;
            this.filter.byDepartment = false;
            this.filter.positionId = null;
            this.filter.organization = '';
            this.filter.department = '';
            this.filter.byPosition = false;
            this.Refresh();
         }
      },
      getDataLocalStorage() {
         const localdata = localStorage.getItem('user_info');
         this.localStorageData = JSON.parse(localdata);

         if (this.localStorageData?.organizationId && this.localStorageData?.organizationId != 1) {
            this.filter.byOrganization = true;
            this.filter.byRegion = false;
            this.filter.regionId = this.localStorageData?.organizationRegionId;
         }
      }
   }
};
</script>

<style lang="scss" scoped>
.breadcrumb-item.active {
   color: var(--primary);
   cursor: pointer;
}
</style>
