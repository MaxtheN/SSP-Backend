<template>
   <b-card no-body>
      <div class="m-2">
         <b-row>
            <b-col sm="12" md="3">
               <div>
                  <label for>{{ $t('Oblast') }}</label>
                  <v-select
                     :options="RegionList"
                     :reduce="(item) => item.value"
                     :placeholder="$t('ChooseBelow')"
                     label="text"
                     v-model="filter.regionId"
                     @input="ChangeRegion"
                     class="w-100"
                  ></v-select>
               </div>
            </b-col>
            <b-col sm="12" md="3">
               <label for>{{ $t('Age') }}</label>
               <b-row>
                  <b-col>
                     <b-form-input v-model="filter.minAge" debounce="100" @update="Refresh" :placeholder="$t('dan')" />
                  </b-col>
                  <b-col>
                     <b-form-input
                        v-model="filter.maxAge"
                        debounce="100"
                        @update="Refresh"
                        :placeholder="$t('gacha')"
                     />
                  </b-col>
               </b-row>
            </b-col>
            <b-col sm="12" md="3">
               <div>
                  <label for>{{ $t("Yuridik ma'lumot bo'yicha") }}</label>
                  <v-select
                     :options="hasLegalEducationList"
                     :reduce="(item) => item.value"
                     :placeholder="$t('ChooseBelow')"
                     label="text"
                     v-model="filter.hasLegalEducation"
                     class="w-100"
                  ></v-select>
               </div>
            </b-col>
            <b-col sm="12" md="3">
               <div>
                  <label for>{{ $t('gender') }}</label>
                  <v-select
                     :options="GenderList"
                     :reduce="(item) => item.value"
                     :placeholder="$t('ChooseBelow')"
                     label="text"
                     v-model="filter.genderId"
                     class="w-100"
                  ></v-select>
               </div>
            </b-col>
         </b-row>
         <b-row class="mt-1">
            <b-col cols="12" md="3">
               <div>
                  <label for>{{ $t('startdate') }}</label>
                  <form-picker v-model="filter.startDate" :placeholder="$t('startdate')" @input="Refresh" />
               </div>
            </b-col>
            <b-col cols="12" md="3">
               <div>
                  <label for>{{ $t('enddate') }}</label>
                  <form-picker v-model="filter.endDate" :placeholder="$t('enddate')" @input="Refresh" />
               </div>
            </b-col>
            <b-col cols="12" md="4">
               <b-button @click="Refresh" class="mt-2" @keyup.enter="Refresh" variant="primary">
                  <feather-icon icon="RefreshCwIcon" />
               </b-button>
            </b-col>
            <!-- <b-col cols="12" md="4" class="mt-2">
               <b-input-group class="text-right">
                  <b-form-input v-model="filter.search" :placeholder="$t('search')" />
                  <b-input-group-append>
                     <b-button @click="Refresh" variant="primary">
                        <feather-icon icon="SearchIcon" />
                     </b-button>
                  </b-input-group-append>
               </b-input-group>
            </b-col> -->
            <b-col class="mt-2 text-right">
               <b-button @click="Print" :disabled="PrintLoading" variant="primary" class="ml-1">
                  <feather-icon icon="PrinterIcon"></feather-icon>
                  {{ $t('Print') }}
               </b-button>
            </b-col>
         </b-row>
      </div>
      <div class="m-2">
         <b-table
            ref="refInvoiceListTable"
            :items="items"
            small
            responsive
            :fields="fields"
            primary-key="id"
            no-border-collapse
            no-footer-sorting
            :busy="isBusy"
            show-empty
            :empty-text="$t('NotFound')"
            class="position-relative"
            @sort-changed="SortChange"
            foot-clone
         >
            <template #cell(order)="{ index }">
               <span>{{ index + 1 }}</span>
            </template>
            <template #cell(allArea)="{ item }">
               <span style="white-space: nowrap">{{ currency(item.allArea) }}</span>
            </template>
            <template #cell(freeArea)="{ item }">
               <span style="white-space: nowrap">{{ currency(item.freeArea) }}</span>
            </template>

            <template #foot(order)>
               <span>{{ $t('Total') }}</span>
            </template>

            <template #foot(regionName)>
               <span></span>
            </template>
            <template #foot(totalDismissedEmployeesCount)>
               <span class="text-end" style="white-space: nowrap">{{
                  currency(totals.totalDismissedEmployeesCount)
               }}</span>
            </template>

            <template #foot(totalEmployeesCount)>
               <span class="text-end" style="white-space: nowrap">{{ currency(totals.totalEmployeesCount) }}</span>
            </template>

            <template #foot(totalEmployeesInLeaveOrderCount)>
               <span class="text-end" style="white-space: nowrap">{{
                  currency(totals.totalEmployeesInLeaveOrderCount)
               }}</span>
            </template>

            <template #foot(totalEmployeesInMaternityLeaveCount)>
               <span class="text-end" style="white-space: nowrap">{{
                  currency(totals.totalEmployeesInMaternityLeaveCount)
               }}</span>
            </template>
            <template #foot(totalEmployeesInRecallLeaveCount)>
               <span class="text-end" style="white-space: nowrap">{{
                  currency(totals.totalEmployeesInRecallLeaveCount)
               }}</span>
            </template>
            <template #foot(totalEmployeesSendToTrainingCount)>
               <span class="text-end" style="white-space: nowrap">{{
                  currency(totals.totalEmployeesSendToTrainingCount)
               }}</span>
            </template>

            <template #foot(totalHiredEmployeesCount)>
               <span class="text-end" style="white-space: nowrap">{{ currency(totals.totalHiredEmployeesCount) }}</span>
            </template>

            <template #foot(totalStaffingPositionsCount)>
               <span class="text-end" style="white-space: nowrap">{{
                  currency(totals.totalStaffingPositionsCount)
               }}</span>
            </template>

            <template #foot(totalTransferredEmployeesCount)>
               <span class="text-end" style="white-space: nowrap">{{
                  currency(totals.totalTransferredEmployeesCount)
               }}</span>
            </template>
         </b-table>
      </div>
   </b-card>
</template>

<script>
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
   BBreadcrumbItem
} from 'bootstrap-vue';
import HrmReportService from '@/services/hrm/report.service';
import RegionService from '@/services/info/region.service';
import DistrictService from '@/services/info/district.service';
import ManualService from '@/services/others/manual.service';
export default {
   components: {
      BButton,
      BPagination,
      BTable,
      BCol,
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
      BBreadcrumbItem
   },
   name: 'Index',
   directives: {
      'b-tooltip': VBTooltip,
      'b-modal': VBModal
   },
   data() {
      return {
         items: [],
         RegionList: [],
         DistrictList: [],
         fields: [],

         GenderList: [],
         hasLegalEducationList: [
            { value: true, text: this.$t('hasLegalEducation') },
            { value: false, text: this.$t("Yuridik ma'lumotga ega emas") }
         ],
         filter: {
            regionId: null,
            region: '',
            birthRegionId: null,
            genderId: null,
            startDate: '',
            endDate: '',
            minAge: null,
            maxAge: null,
            hasLegalEducation: null
         },
         isBusy: false,
         totals: {
            totalEmployeesCount: 0,
            totalStaffingPositionsCount: 0,
            totalEmployeesInLeaveOrderCount: 0,
            totalEmployeesInRecallLeaveCount: 0,
            totalEmployeesInMaternityLeaveCount: 0,
            totalEmployeesSendToTrainingCount: 0,
            totalHiredEmployeesCount: 0,
            totalDismissedEmployeesCount: 0,
            totalTransferredEmployeesCount: 0
         },
         PrintLoading: false
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

      ManualService.GenderSelectList()
         .then((res) => {
            this.GenderList = res.data;
         })
         .catch((error) => {
            this.showApiError(error);
         });
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
               // sortable: true
            },
            {
               key: 'organizationName',
               label: this.$t('organization'),
               sortable: true,
               thClass: 'text-center',
               stickyColumn: this.isMobileDevice() ? false : true,
               thStyle: {
                  minWidth: '350px'
               }
            },
            {
               key: 'totalEmployeesCount',
               label: this.$t('totalEmployeesCount'),
               thClass: 'text-right',
               tdClass: 'text-right',
               sortable: true
               // stickyColumn: true
            },

            {
               key: 'totalStaffingPositionsCount',
               label: this.$t('totalStaffingPositionsCount'),
               thClass: 'text-right',
               tdClass: 'text-right',
               sortable: true
            },
            {
               key: 'totalEmployeesInLeaveOrderCount',
               label: this.$t('totalEmployeesInLeaveOrderCount'),
               thClass: 'text-right',
               tdClass: 'text-right',
               sortable: true
            },
            {
               key: 'totalEmployeesInRecallLeaveCount',
               label: this.$t('totalEmployeesInRecallLeaveCount'),
               thClass: 'text-right',
               tdClass: 'text-right',
               sortable: true
            },
            {
               key: 'totalEmployeesInMaternityLeaveCount',
               label: this.$t('totalEmployeesInMaternityLeaveCount'),
               thClass: 'text-right',
               tdClass: 'text-right',
               sortable: true
            },
            {
               key: 'totalEmployeesSendToTrainingCount',
               label: this.$t('totalEmployeesSendToTrainingCount'),
               thClass: 'text-right',
               tdClass: 'text-right',
               sortable: true
            },
            {
               key: 'totalHiredEmployeesCount',
               label: this.$t('totalHiredEmployeesCount'),
               thClass: 'text-right',
               tdClass: 'text-right',
               sortable: true
            },
            {
               key: 'totalDismissedEmployeesCount',
               label: this.$t('totalDismissedEmployeesCount'),
               thClass: 'text-right',
               tdClass: 'text-right',
               sortable: true
            },
            {
               key: 'totalTransferredEmployeesCount',
               label: this.$t('totalTransferredEmployeesCount'),
               thClass: 'text-right',
               tdClass: 'text-right',
               sortable: true
            }
         ];
      },
      goToBussnes(inn) {
         this.$router.push({ name: 'BusinessmanCard', query: { inn: inn } });
      },
      SortRegion(item) {
         this.filter.regionId = item.regionId;
         this.filter.region = item.region;
         this.GetDistrict(item.regionId);
         this.Refresh();
      },
      SortDistrict(item) {
         this.filter.byDistrict = false;
         this.filter.byContractor = true;
         this.filter.districtId = item.districtId;
         this.filter.district = item.district;
         this.Refresh();
      },
      SortChange(data) {
         this.filter.Sort = data.sortBy;
         this.filter.Order = data.sortDesc ? 'desc' : 'asc';
         this.Refresh();
      },
      ChangeRegion(id) {
         if (id) {
            this.filter.region = this.filter.regionId
               ? this.RegionList.filter((item) => item.value === this.filter.regionId)[0].text
               : '';

            this.Refresh();
         } else {
            this.filter.region = '';
            this.Refresh();
         }
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
      BindValue(value) {
         this.filter.dateofbirth = value;
      },
      Print() {
         this.PrintLoading = true;
         HrmReportService.SaveHrmReportAsExcel(this.filter)
            .then((res) => {
               this.forceFileDownload(res, this.$t('gethrmemployeeactivityreport'));
            })
            .finally(() => {
               this.PrintLoading = false;
            });
      },
      Refresh() {
         this.getFields();
         this.isBusy = true;
         HrmReportService.GetHrmEmployeeActivityReport(this.filter)
            .then((res) => {
               this.items = res.data;
               this.totals = {
                  totalEmployeesCount: 0,
                  totalStaffingPositionsCount: 0,
                  totalEmployeesInLeaveOrderCount: 0,
                  totalEmployeesInRecallLeaveCount: 0,
                  totalEmployeesInMaternityLeaveCount: 0,
                  totalEmployeesSendToTrainingCount: 0,
                  totalHiredEmployeesCount: 0,
                  totalDismissedEmployeesCount: 0,
                  totalTransferredEmployeesCount: 0
               };
               res.data.forEach((item) => {
                  this.totals.totalEmployeesCount += item.totalEmployeesCount;
                  this.totals.totalStaffingPositionsCount += item.totalStaffingPositionsCount;
                  this.totals.totalEmployeesInLeaveOrderCount += item.totalEmployeesInLeaveOrderCount;
                  this.totals.totalEmployeesInRecallLeaveCount += item.totalEmployeesInRecallLeaveCount;
                  this.totals.totalEmployeesInMaternityLeaveCount += item.totalEmployeesInMaternityLeaveCount;
                  this.totals.totalEmployeesSendToTrainingCount += item.totalEmployeesSendToTrainingCount;
                  this.totals.totalHiredEmployeesCount += item.totalHiredEmployeesCount;
                  this.totals.totalDismissedEmployeesCount += item.totalDismissedEmployeesCount;
                  this.totals.totalTransferredEmployeesCount += item.totalTransferredEmployeesCount;
               });

               this.isBusy = false;
            })
            .catch((error) => {
               this.isBusy = false;
               this.makeToast(error.response.data.errors, 'danger');
            });
      }
   }
};
</script>

<style lang="scss" scoped>
// @import '../styles.scss';
</style>
