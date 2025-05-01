<template>
   <b-card>
      <b-row>
         <b-col cols="12" md="6">
            <form-select
               v-if="localStorageData?.organizationId == 1"
               :options="OrganisationList"
               v-model="filter.organizationId"
               :label="$t('organization')"
               @change="HandleRegion"
            />
         </b-col>

         <b-col sm="12" md="2">
            <form-picker :label="$t('startDate1')" v-model="filter.fromDate" @change="Refresh" />
         </b-col>
         <b-col sm="12" md="2">
            <form-picker :label="$t('endDate1')" v-model="filter.toDate" @change="Refresh" />
         </b-col>

         <b-col sm="12" :md="localStorageData?.organizationId == 1 ? 2 : 8" class="text-right mt-2">
            <b-button @click="Print" variant="primary">
               <feather-icon icon="PrinterIcon"></feather-icon>
               {{ $t('Print') }}
            </b-button>
         </b-col>
      </b-row>
      <b-breadcrumb class="mb-1 mt-0">
         <b-breadcrumb-item
            @click="
               () => {
                  (filter.byOrganization = true),
                     (filter.organizationName = ''),
                     (filter.department = ''),
                     (filter.position = ''),
                     (filter.organizationId = null),
                     (filter.byDepartment = false),
                     (filter.departmentId = null),
                     (filter.byPosition = false),
                     (filter.positionId = 0),
                     (filter.byEmployee = false),
                     (filter.employeeId = null);
                  Refresh();
               }
            "
         >
            <b>{{ $t('uzb') }}</b>
         </b-breadcrumb-item>
         <b-breadcrumb-item
            v-show="filter.organizationName"
            @click="
               () => {
                  filter.byOrganization = false;
                  filter.byDepartment = true;
                  filter.byEmployee = false;
                  filter.department = '';
                  filter.position = '';
                  filter.byPosition = false;
                  Refresh();
               }
            "
         >
            <b>{{ filter.organizationName }}</b>
         </b-breadcrumb-item>
         <b-breadcrumb-item
            v-show="filter.department"
            @click="
               () => {
                  filter.position = '';
                  filter.byDepartment = false;
                  filter.byPosition = true;
                  filter.byOrganization = false;
                  filter.byEmployee = false;

                  filter.byEmployee = false;
                  Refresh();
               }
            "
         >
            <b>{{ filter.department }}</b>
         </b-breadcrumb-item>

         <b-breadcrumb-item
            v-show="filter.position"
            @click="
               () => {
                  filter.byPosition = true;
                  filter.byEmployee = false;
                  filter.byDepartment = false;
                  filter.byOrganization = false;
                  Refresh();
               }
            "
         >
            <b>{{ filter.position }}</b>
         </b-breadcrumb-item>
      </b-breadcrumb>
      <b-overlay :show="isBusy">
         <b-table
            ref="refInvoiceListTable"
            bordered
            small
            no-footer-sorting
            foot-clone
            class="report-border"
            :items="items"
            :fields="fields"
            responsive
            :busy="isBusy"
            show-empty
            :empty-text="$t('NotFound')"
         >
            <template #cell(order)="{ item, index }">
               {{ index + 1 }}
            </template>
            <template #cell(organizationName)="{ item }">
               <span v-if="filter.byOrganization" @click="SortOrganization(item)" style="cursor: pointer; color: blue">
                  {{ item.organizationName }}
               </span>
               <span v-if="filter.byDepartment" @click="SortDepatment(item)" style="cursor: pointer; color: blue">
                  {{ item.department }}
               </span>
               <span v-if="filter.byPosition" @click="SortPosition(item)" style="cursor: pointer; color: blue">
                  {{ item.position }}
               </span>
               <span v-if="filter.byEmployee" style="cursor: pointer; color: blue">
                  {{ item.employee }}
               </span>
            </template>
            <template #foot(order)>
               <span></span>
            </template>

            <template #foot(organizationName)>
               <span>{{ $t('Total') }}</span>
            </template>

            <template #foot(totalAppoimtEmployeesHireCount)>
               <span style="text-align: right"> {{ currency(totals.totalAppoimtEmployeesHireCount) }} </span>
            </template>
            <template #foot(totalAppoimtEmployeesTransferCount)>
               <span style="text-align: right"> {{ currency(totals.totalAppoimtEmployeesTransferCount) }} </span>
            </template>
            <template #foot(totalAppoimtEmployeesDismissilCount)>
               <span style="text-align: right"> {{ currency(totals.totalAppoimtEmployeesDismissilCount) }} </span>
            </template>
            <template #foot(totalChastisementPenaltyCount)>
               <span style="text-align: right"> {{ currency(totals.totalChastisementPenaltyCount) }} </span>
            </template>
            <template #foot(totalChastisementReprimandCount)>
               <span style="text-align: right"> {{ currency(totals.totalChastisementReprimandCount) }} </span>
            </template>
            <template #foot(totalTempCalcKindFinancialCount)>
               <span style="text-align: right"> {{ currency(totals.totalTempCalcKindFinancialCount) }} </span>
            </template>
            <template #foot(totalTempCalcKindIncentiveCount)>
               <span style="text-align: right"> {{ currency(totals.totalTempCalcKindIncentiveCount) }} </span>
            </template>
            <template #foot(totalOrderToSendBusinessTripCount)>
               <span style="text-align: right"> {{ currency(totals.totalOrderToSendBusinessTripCount) }} </span>
            </template>
            <template #foot(totalOrderToSendBusinessTripCountryCount)>
               <span style="text-align: right"> {{ currency(totals.totalOrderToSendBusinessTripCountryCount) }} </span>
            </template>
            <template #foot(totalOrderToSendBusinessTripAnotherOrgCount)>
               <span style="text-align: right">
                  {{ currency(totals.totalOrderToSendBusinessTripAnotherOrgCount) }}
               </span>
            </template>
            <template #foot(totalEmployeeLeaveOrderCount)>
               <span style="text-align: right"> {{ currency(totals.totalEmployeeLeaveOrderCount) }} </span>
            </template>
            <template #foot(totalEmployeeSendTrainCount)>
               <span style="text-align: right"> {{ currency(totals.totalEmployeeSendTrainCount) }} </span>
            </template>
            <template #foot(totalEmployeeSickLeaveHomladorlikCount)>
               <span style="text-align: right"> {{ currency(totals.totalEmployeeSickLeaveHomladorlikCount) }} </span>
            </template>
            <template #foot(totalEmployeeSickLeaveBolaParvarishiCount)>
               <span style="text-align: right"> {{ currency(totals.totalEmployeeSickLeaveBolaParvarishiCount) }} </span>
            </template>
            <template #foot(totalRecallLeaveCount)>
               <span style="text-align: right"> {{ currency(totals.totalRecallLeaveCount) }} </span>
            </template>
         </b-table>
      </b-overlay>
   </b-card>
</template>

<script>
import HrmReportService from '@/services/hrm/report.service';
import FormTableHrm from '@/components/forms/form-table-hrm.vue';
import {
   BTable,
   BCard,
   BButton,
   BIconFileEarmarkExcel,
   BRow,
   BOverlay,
   BCol,
   BBreadcrumb,
   BBreadcrumbItem
} from 'bootstrap-vue';

import ManualService from '@/services/others/manual.service';
import RegionService from '@/services/info/region.service';
import ReportService from '@/services/report/report.service';
import DistrictService from '@/services/info/district.service';
export default {
   components: {
      BTable,
      BCol,
      BOverlay,
      BRow,
      BCard,
      BButton,
      BIconFileEarmarkExcel,
      BBreadcrumb,
      BBreadcrumbItem
   },
   data() {
      return {
         items: [],
         OrganisationList: [],
         localStorageData: {},
         isBusy: false,
         PrintLoading: false,
         filter: {
            byOrganization: true,
            organization: '',
            department: '',
            position: '',
            organizationId: null,
            regionId: null,
            byDepartment: false,
            departmentId: null,
            byPosition: false,
            positionId: 0,
            byEmployee: false,
            employeeId: null,
            Employee: '',
            byRegion: false,
            region: '',
            byPositionCategory: false
         },
         totals: {
            totalAppoimtEmployeesHireCount: 0,
            totalAppoimtEmployeesTransferCount: 0.0,
            totalAppoimtEmployeesDismissilCount: 0,
            totalChastisementPenaltyCount: 0,
            totalChastisementReprimandCount: 0,
            totalTempCalcKindFinancialCount: 0,
            totalTempCalcKindIncentiveCount: 0,
            totalOrderToSendBusinessTripCount: 0,
            totalOrderToSendBusinessTripCountryCount: 0,
            totalOrderToSendBusinessTripAnotherOrgCount: 0,
            totalEmployeeLeaveOrderCount: 0,
            totalEmployeeSendTrainCount: 0,
            totalEmployeeSickLeaveHomladorlikCount: 0,
            totalEmployeeSickLeaveBolaParvarishiCount: 0,
            totalRecallLeaveCount: 0
         },
         fields: []
      };
   },
   created() {
      ManualService.OrganizationAsSelectListByGroup([1, 3]).then((res) => {
         this.OrganisationList = res.data;
      });
      this.getDataLocalStorage();
      this.Refresh();
   },
   computed: {
      handlelabel() {
         if (this.filter.byOrganization) {
            return this.$t('organization');
         } else if (this.filter.byDepartment) {
            return this.$t('department');
         } else if (this.filter.byPosition) {
            return this.$t('position');
         } else {
            return this.$t('employee');
         }
      }
   },
   methods: {
      Print() {
         this.PrintLoading = true;
         ReportService.SaveAsExcelGetHrmCommands(this.filter)
            .then((res) => {
               this.PrintLoading = false;

               this.forceFileDownload(res, 'Buyruqlar hisoboti');
            })
            .catch((error) => {
               this.PrintLoading = false;
               this.makeToast(error.response.data, 'danger');
            });
      },

      Refresh() {
         this.getFeilds();
         this.isBusy = true;
         if (this.localStorageData.organizationId != 1) {
            this.filter.regionId = this.localStorageData.organizationRegionId;
            this.filter.organizationId = this.filter.organizationId || this.localStorageData?.organizationId;
         }
         HrmReportService.GetReportDocumentsForHrm(this.filter)
            .then((res) => {
               this.items = res.data;
               this.isBusy = false;

               this.totals = {
                  totalAppoimtEmployeesHireCount: 0,
                  totalAppoimtEmployeesTransferCount: 0,
                  totalAppoimtEmployeesDismissilCount: 0,
                  totalChastisementPenaltyCount: 0,
                  totalChastisementReprimandCount: 0,
                  totalTempCalcKindFinancialCount: 0,
                  totalTempCalcKindIncentiveCount: 0,
                  totalOrderToSendBusinessTripCount: 0,
                  totalOrderToSendBusinessTripCountryCount: 0,
                  totalOrderToSendBusinessTripAnotherOrgCount: 0,
                  totalEmployeeLeaveOrderCount: 0,
                  totalEmployeeSendTrainCount: 0,
                  totalEmployeeSickLeaveHomladorlikCount: 0,
                  totalEmployeeSickLeaveBolaParvarishiCount: 0,
                  totalRecallLeaveCount: 0
               };

               this.items.forEach((item) => {
                  this.totals.totalAppoimtEmployeesHireCount += item.totalAppoimtEmployeesHireCount;
                  this.totals.totalAppoimtEmployeesTransferCount += item.totalAppoimtEmployeesTransferCount;
                  this.totals.totalAppoimtEmployeesDismissilCount += item.totalAppoimtEmployeesDismissilCount;
                  this.totals.totalChastisementPenaltyCount += item.totalChastisementPenaltyCount;
                  this.totals.totalChastisementReprimandCount += item.totalChastisementReprimandCount;
                  this.totals.totalTempCalcKindFinancialCount += item.totalTempCalcKindFinancialCount;
                  this.totals.totalTempCalcKindIncentiveCount += item.totalTempCalcKindIncentiveCount;
                  this.totals.totalOrderToSendBusinessTripCount += item.totalOrderToSendBusinessTripCount;
                  this.totals.totalOrderToSendBusinessTripCountryCount += item.totalOrderToSendBusinessTripCountryCount;
                  this.totals.totalOrderToSendBusinessTripAnotherOrgCount +=
                     item.totalOrderToSendBusinessTripAnotherOrgCount;
                  this.totals.totalEmployeeLeaveOrderCount += item.totalEmployeeLeaveOrderCount;
                  this.totals.totalEmployeeSendTrainCount += item.totalEmployeeSendTrainCount;
                  this.totals.totalEmployeeSickLeaveHomladorlikCount += item.totalEmployeeSickLeaveHomladorlikCount;
                  this.totals.totalEmployeeSickLeaveBolaParvarishiCount +=
                     item.totalEmployeeSickLeaveBolaParvarishiCount;
                  this.totals.totalRecallLeaveCount += item.totalRecallLeaveCount;
               });
            })
            .catch((error) => {
               this.showApiError(error);
            });
      },
      getDataLocalStorage() {
         const localdata = localStorage.getItem('user_info');
         this.localStorageData = JSON.parse(localdata);

         if (this.localStorageData?.organizationId && this.localStorageData?.organizationId != 1) {
            this.filter.byOrganization = true;
            this.filter.byRegion = false;
            this.filter.regionId = this.localStorageData?.organizationRegionId;
         }
      },
      SortOrganization(item) {
         this.filter.byOrganization = false;
         this.filter.byDepartment = true;
         this.filter.organizationId = item.organizationId;
         this.filter.byOrganization = false;
         this.filter.organizationName = item.organizationName;
         this.filter.byPosition = false;
         this.filter.byEmployee = false;
         this.Refresh();
      },
      SortDepatment(item) {
         this.filter.departmentId = item.departmentId;
         this.filter.byOrganization = false;
         this.filter.byEmployee = false;
         this.filter.byDepartment = false;
         this.filter.byPosition = true;
         this.filter.department = item.department;
         this.Refresh();
      },
      SortPosition(item) {
         this.filter.byOrganization = false;
         this.filter.byDepartment = false;
         this.filter.position = item.position;
         this.filter.byPosition = false;
         this.filter.positionId = item.positionId;
         this.filter.byEmployee = true;
         this.Refresh();
      },

      HandleRegion() {
         if (this.filter.organizationId == null) {
            console.log('sss');
            (this.filter.byOrganization = true),
               (this.filter.organizationName = ''),
               (this.filter.department = ''),
               (this.filter.position = ''),
               (this.filter.byDepartment = false),
               (this.filter.departmentId = null),
               (this.filter.byPosition = false),
               (this.filter.positionId = 0),
               (this.filter.byEmployee = false),
               (this.filter.employeeId = null);
            this.Refresh();
         }
         this.Refresh();
      },

      getFeilds() {
         this.fields = [
            {
               key: 'order',
               label: this.$t('№'),
               thClass: 'text-center report-border',
               sortable: false,
               tdClass: 'text-center'
            },
            {
               key: 'organizationName',
               label: this.handlelabel,
               stickyColumn: this.isMobileDevice() ? false : true,
               sortable: true,
               thClass: 'text-center report',
               sortable: false,
               thStyle: {
                  minWidth: '350px'
               }
            },

            {
               key: 'totalAppoimtEmployeesHireCount',
               label: this.$t('totalAppoimtEmployeesHireCount'),
               thClass: 'text-center',
               tdClass: 'text-right',
               sortable: false
            },

            {
               key: 'totalAppoimtEmployeesTransferCount',
               label: this.$t('totalAppoimtEmployeesTransferCount'),
               thClass: 'text-center',
               tdClass: 'text-right',
               sortable: false
            },
            {
               key: 'totalAppoimtEmployeesDismissilCount',
               label: this.$t('totalAppoimtEmployeesDismissilCount'),
               thClass: 'text-center',
               tdClass: 'text-right',
               sortable: false
            },
            {
               key: 'totalChastisementPenaltyCount',
               label: this.$t('totalChastisementPenaltyCount'),
               thClass: 'text-center',
               tdClass: 'text-right',
               sortable: false
            },
            {
               key: 'totalChastisementReprimandCount',
               label: this.$t('totalChastisementReprimandCount'),
               thClass: 'text-center',
               tdClass: 'text-right',
               sortable: false
            },
            {
               key: 'totalTempCalcKindFinancialCount',
               label: this.$t('totalTempCalcKindFinancialCount'),
               thClass: 'text-center',
               tdClass: 'text-right',
               sortable: false
            },

            {
               key: 'totalTempCalcKindIncentiveCount',
               label: this.$t('totalTempCalcKindIncentiveCount'),
               thClass: 'text-center',
               tdClass: 'text-right',
               sortable: false
            },
            {
               key: 'totalOrderToSendBusinessTripCount',
               label: this.$t('totalOrderToSendBusinessTripCount'),
               thClass: 'text-center',
               tdClass: 'text-right',
               sortable: false
            },
            {
               key: 'totalOrderToSendBusinessTripCountryCount',
               label: this.$t('totalOrderToSendBusinessTripCountryCount'),
               thClass: 'text-center',
               tdClass: 'text-right',
               sortable: false
            },
            {
               key: 'totalOrderToSendBusinessTripAnotherOrgCount',
               label: this.$t('totalOrderToSendBusinessTripAnotherOrgCount'),
               thClass: 'text-center',
               tdClass: 'text-right',
               sortable: false
            },
            {
               key: 'totalEmployeeLeaveOrderCount',
               label: this.$t('totalEmployeeLeaveOrderCount'),
               thClass: 'text-center',
               tdClass: 'text-right',
               sortable: false
            },
            {
               key: 'totalEmployeeSendTrainCount',
               label: this.$t('totalEmployeeSendTrainCount'),
               thClass: 'text-center',
               tdClass: 'text-right',
               sortable: false
            },
            {
               key: 'totalEmployeeSickLeaveHomladorlikCount',
               label: this.$t('totalEmployeeSickLeaveHomladorlikCount'),
               thClass: 'text-right',
               tdClass: 'text-right',
               sortable: false
            },
            {
               key: 'totalEmployeeSickLeaveBolaParvarishiCount',
               label: this.$t('totalEmployeeSickLeaveBolaParvarishiCount'),
               thClass: 'text-center',
               tdClass: 'text-right',
               sortable: false
            },
            {
               key: 'totalRecallLeaveCount',
               label: this.$t('totalRecallLeaveCount'),
               thClass: 'text-center',
               tdClass: 'text-right',
               sortable: false
            }
         ];
      }
   }
};
</script>

<style lang="scss" scoped></style>
