<template>
   <b-card>
      <b-row>
         <b-col sm="12" md="2">
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
         <b-col sm="12" md="2">
            <div>
               <label for>{{ $t('Region') }}</label>
               <v-select
                  :options="DistrictList"
                  :reduce="(item) => item.value"
                  :placeholder="$t('ChooseBelow')"
                  label="text"
                  v-model="filter.districtId"
                  @input="ChangeDistrict"
                  class="w-100"
               ></v-select>
            </div>
         </b-col>
         <b-col sm="12" md="2">
            <form-picker :label="$t('startDate')" v-model="filter.startDate" />
         </b-col>
         <b-col sm="12" md="2">
            <form-picker :label="$t('endDate')" v-model="filter.endDate" />
         </b-col>
         <b-col cols="12" md="4" class="mb-1" :class="{ 'mt-2': !isMobileDevice() }">
            <b-input-group class="text-right">
               <b-form-input v-model="filter.search" :placeholder="$t('search')" />
               <b-input-group-append>
                  <b-button @click="Refresh" variant="primary">
                     <feather-icon icon="SearchIcon" />
                  </b-button>
               </b-input-group-append>
            </b-input-group>
         </b-col>
      </b-row>
      <b-row>
         <b-col sm="12" md="8">
            <b-button-group @click="Refresh" size="sm" class="mr-2">
               <b-button
                  @click="filter.prtnContractTypeId = null"
                  :variant="null == filter.prtnContractTypeId ? 'primary' : 'outline-primary'"
                  >{{ $t('all') }}</b-button
               >
               <b-button
                  v-for="type in PrtnContractTypeList"
                  :key="type.value"
                  @click="filter.prtnContractTypeId = type.value"
                  :variant="type.value == filter.prtnContractTypeId ? 'primary' : 'outline-primary'"
                  >{{ type.text }}</b-button
               >
            </b-button-group>

            <b-breadcrumb class="mt-2">
               <b-breadcrumb-item
                  :active="filter.byRegion"
                  @click="
                     () => {
                        filter.byDistrict = false;
                        filter.byRegion = true;
                        filter.byContractor = false;
                        filter.region = '';
                        filter.regionId = null;
                        filter.district = '';
                        filter.districtId = null;
                        Refresh();
                     }
                  "
               >
                  <b>{{ $t('uzb') }}</b>
               </b-breadcrumb-item>
               <b-breadcrumb-item
                  v-show="filter.region"
                  :active="filter.byDistrict"
                  @click="
                     () => {
                        filter.byDistrict = true;
                        filter.byRegion = false;
                        filter.byContractor = false;
                        filter.district = '';
                        filter.districtId = null;
                        Refresh();
                     }
                  "
               >
                  <b>{{ filter.region }}</b>
               </b-breadcrumb-item>
               <b-breadcrumb-item v-show="filter.district" :active="filter.byContractor">
                  <b>{{ filter.district }}</b>
               </b-breadcrumb-item>
            </b-breadcrumb>
         </b-col>

         <b-col class="text-right">
            <b-button @click="Print" :disabled="PrintLoading" variant="primary" class="ml-1">
               <feather-icon icon="PrinterIcon"></feather-icon>
               {{ $t('Print') }}
            </b-button>
            <b-button
               @click="PrintForSum"
               :class="{ 'mt-1 mb-1': isMobileDevice() }"
               :disabled="PrintForSumLoading"
               variant="success"
               class="ml-1"
            >
               <feather-icon icon="PrinterIcon"></feather-icon>
               {{ $t('Print2') }}
            </b-button>
         </b-col>
      </b-row>

      <b-table
         ref="refInvoiceListTable"
         :items="items"
         responsive
         :fields="fields"
         primary-key="id"
         sticky-header="65vh"
         no-border-collapse
         no-footer-sorting
         :busy="isBusy"
         show-empty
         :empty-text="$t('NotFound')"
         class="position-relative"
         foot-clone
         @sort-changed="SortChange"
      >
         <template #cell(order)="{ index }">
            <span>{{ index + 1 }}</span>
         </template>
         <template #cell(region)="{ item }">
            <span style="color: blue; cursor: pointer" @click="SortRegion(item)">{{ item.region }}</span>
         </template>

         <template #cell(contractor)="{ item }">
            <span style="color: blue; cursor: pointer" @click="goToBussnes(item.contractorInn)">{{
               item.contractorInn
            }}</span>
            -
            {{ item.contractor }}
         </template>

         <template #cell(district)="{ item }">
            <span style="color: blue; cursor: pointer" @click="SortDistrict(item)">{{ item.district }}</span>
         </template>

         <template #cell(totalPrtnApplicationSentCount)="{ item }">
            {{ currency(item.totalPrtnApplicationSentCount) }}
         </template>

         <template #cell(totalApplicationCountAll)="{ item }">
            <div style="color: blue; font-weight: 600">
               {{
                  currency(
                     item.totalPrtnApplicationSentRejectedCount +
                        item.totalPrtnApplicationSentForReviewCount +
                        item.totalPrtnApplicationSentAcceptedCount +
                        item.totalPrtnApplicationSentCount +
                        item.totalPrtnApplicationCanceledCount
                  )
               }}
            </div>
         </template>

         <template #cell(totalPrtnApplicationCanceledCount)="{ item }">
            <div style="color: blue; font-weight: 600">
               {{ currency(item.totalPrtnApplicationCanceledCount) }}
            </div>
         </template>

         <template #cell(totalApplicationCount)="{ item }">
            <div style="color: blue; font-weight: 600">
               {{ currency(item.totalPrtnApplicationCount) }}
            </div>
         </template>

         <template #cell(totalPrtnContractCount)="{ item }">
            {{ currency(item.totalPrtnContractCount) }}
         </template>
         <template #cell(totalPrtnApplicationIsOffersCount)="{ item }">
            {{ currency(item.totalPrtnApplicationIsOffersCount) }}
         </template>
         <template #cell(totalPrtnApplicationSentAcceptedCount)="{ item }">
            {{ currency(item.totalPrtnApplicationSentAcceptedCount) }}
         </template>
         <template #cell(totalPrtnApplicationSentForReviewCount)="{ item }">
            {{ currency(item.totalPrtnApplicationSentForReviewCount) }}
         </template>
         <template #cell(totalPrtnCertificateCanceledCount)="{ item }">
            {{ currency(item.totalPrtnCertificateCanceledCount) }}
         </template>
         <template #cell(totalPrtnApplicationSentRejectedCount)="{ item }">
            <div style="color: red; font-weight: 600">
               {{ currency(item.totalPrtnApplicationSentRejectedCount) }}
            </div>
         </template>

         <template #cell(totalPrtnContractCancelCount)="{ item }">
            <div style="color: red; font-weight: 600">
               {{ currency(item.totalPrtnContractCancelCount + item.totalPrtnContractRejectedCount) }}
            </div>
         </template>

         <template #cell(totalPrtnCertificateCount)="{ item }">
            {{ currency(item.totalPrtnCertificateCount) }}
         </template>

         <template #cell(totalNewVacanciesCount)="{ item }">
            {{ currency(item.totalNewVacanciesCount) }}
         </template>

         <template #foot(totalPrtnApplicationSentCount)>
            <span>{{ currency(totals.totalPrtnApplicationSentCount) }}</span>
         </template>

         <template #foot(totalApplicationCountAll)>
            <span>{{
               currency(
                  totals.totalPrtnApplicationSentRejectedCount +
                     totals.totalPrtnApplicationSentForReviewCount +
                     totals.totalPrtnApplicationSentAcceptedCount +
                     totals.totalPrtnApplicationSentCount +
                     totals.totalPrtnApplicationCanceledCount
               )
            }}</span>
         </template>

         <template #foot(totalApplicationCount)>
            <span style="color: blue; font-weight: 600">{{ currency(totals.totalApplicationCount) }}</span>
         </template>

         <template #foot(totalPrtnContractCount)>
            <span>{{ currency(totals.totalPrtnContractCount) }}</span>
         </template>
         <template #foot(totalPrtnApplicationCanceledCount)>
            <span>{{ currency(totals.totalPrtnApplicationCanceledCount) }}</span>
         </template>
         <template #foot(totalPrtnApplicationSentForReviewCount)>
            <span>
               {{ currency(totals.totalPrtnApplicationSentForReviewCount) }}
            </span>
         </template>
         <template #foot(totalPrtnApplicationSentRejectedCount)>
            <span style="color: red; font-weight: 600">
               {{ currency(totals.totalPrtnApplicationSentRejectedCount) }}
            </span>
         </template>

         <template #foot(totalPrtnApplicationNotPassExpertisesCount)>
            <span>{{ currency(totals.totalPrtnApplicationNotPassExpertisesCount) }}</span>
         </template>
         <template #foot(totalPrtnApplicationPassExpertisesCount)>
            <span>{{ currency(totals.totalPrtnApplicationPassExpertisesCount) }}</span>
         </template>
         <template #foot(totalPrtnApplicationSignedCount)>
            <span>{{ currency(totals.totalPrtnApplicationSignedCount) }}</span>
         </template>
         <template #foot(totalPrtnCertificateCanceledCount)>
            <span>{{ currency(totals.totalPrtnCertificateCanceledCount) }}</span>
         </template>
         <template #foot(totalPrtnApplicationSignningCount)>
            <span>{{ currency(totals.totalPrtnApplicationSignningCount) }}</span>
         </template>
         <template #foot(totalPrtnApplicationSentAcceptedCount)>
            <span>{{ currency(totals.totalPrtnApplicationSentAcceptedCount) }}</span>
         </template>

         <template #foot(totalPrtnCertificateCount)>
            <span>{{ currency(totals.totalPrtnCertificateCount) }}</span>
         </template>

         <template #foot(totalNewVacanciesCount)>
            <span class="text-end">
               {{ currency(totals.totalNewVacanciesCount) }}
               <!-- {{ totals.totalNewVacanciesCount }} -->
            </span>
         </template>

         <template #foot(totalPrtnApplicationSentForExpertisesCount)>
            <span>{{ currency(totals.totalPrtnApplicationSentForExpertisesCount) }}</span>
         </template>

         <template #foot(totalPrtnApplicationIsOffersCount)>
            <span>{{ currency(totals.totalPrtnApplicationIsOffersCount) }}</span>
         </template>

         <template #foot(totalPrtnContractCancelCount)>
            <span style="color: red; font-weight: 600">{{
               currency(totals.totalPrtnContractCancelCount + totals.totalPrtnContractRejectedCount)
            }}</span>
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
         <template #foot(prtnContractType)>
            <span></span>
         </template>
      </b-table>
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
   BBreadcrumbItem,
   BTabs,
   BTab
} from 'bootstrap-vue';
import ReportService from '@/services/report/report.service';
import PrtnContractTypeService from '@/services/info/prtncontracttype.service';

import RegionService from '@/services/info/region.service';
import DistrictService from '@/services/info/district.service';

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
      BBreadcrumbItem,
      BTabs,
      BTab
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
         PrtnContractTypeList: [],
         fields: [],
         filter: {
            startDate: null,
            endDate: null,
            prtnContractTypeId: null,
            regionId: null,
            region: '',
            byRegion: true,
            districtId: null,
            district: '',
            byDistrict: false,
            contractorId: null,
            byContractor: false
         },
         isBusy: false,
         totals: {
            totalPrtnApplicationSentCount: 0,
            totalApplicationCount: 0,
            totalPrtnApplicationSentForReviewCount: 0,
            totalPrtnCertificateCanceledCount: 0,
            totalPrtnApplicationSentRejectedCount: 0,
            totalPrtnContractCount: 0,
            totalPrtnApplicationCanceledCount: 0,

            totalPrtnApplicationNotPassExpertisesCount: 0,
            totalPrtnApplicationPassExpertisesCount: 0,
            totalPrtnApplicationSignedCount: 0,
            totalPrtnApplicationSignningCount: 0,
            totalPrtnApplicationSentAcceptedCount: 0,

            totalPrtnCertificateCount: 0,
            totalNewVacanciesCount: 0,

            totalPrtnApplicationSentForExpertisesCount: 0,
            totalPrtnContractCancelCount: 0,
            totalPrtnContractRejectedCount: 0,
            totalPrtnApplicationIsOffersCount: 0
         },
         PrintLoading: false,
         PrintForSumLoading: false
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

      PrtnContractTypeService.GetAsSelectList()
         .then((res) => {
            this.PrtnContractTypeList = res.data;

            this.Refresh();
         })
         .catch((error) => {
            this.showApiError(error);
         });

      this.getFields();
   },
   methods: {
      getFields() {
         this.fields = [
            {
               key: 'order',
               label: this.$t('№'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: this.filter.byRegion ? 'region' : '',
               label: this.$t('region'),
               sortable: true,
               stickyColumn: !this.isMobileDevice()
            },
            {
               key: this.filter.byDistrict ? 'district' : '',
               label: this.$t('Region'),
               sortable: true,
               stickyColumn: !this.isMobileDevice()
            },
            {
               key: this.filter.byContractor ? 'contractor' : '',
               label: this.$t('contractorT'),
               sortable: true,
               stickyColumn: !this.isMobileDevice(),
               thStyle: {
                  minWidth: '350px'
               }
            },
            {
               key: this.filter.byContractor ? 'prtnContractType' : '',
               label: this.$t('prtnContractType'),
               sortable: true
            },

            {
               key: 'totalApplicationCountAll',
               label: this.$t('totalApplicationCount'),
               thClass: 'text-right',
               tdClass: 'text-right',
               sortable: true
            },

            {
               key: 'totalPrtnApplicationCanceledCount',
               label: this.$t('totalPrtnApplicationCanceledCount'),
               thClass: 'text-right',
               tdClass: 'text-right',
               sortable: true
            },

            {
               key: 'totalApplicationCount',
               label: this.$t('totalApplicationCountActive'),
               thClass: 'text-right',
               tdClass: 'text-right',
               sortable: true
            },

            {
               key: 'totalPrtnApplicationSentCount',
               label: this.$t('totalPrtnApplicationCount'),
               thClass: 'text-right',
               tdClass: 'text-right',
               sortable: true
            },
            {
               key: 'totalPrtnApplicationSentAcceptedCount',
               label: this.$t('TheNumberOfApplicationsWithPositiveConclusion'),
               thClass: 'text-right',
               tdClass: 'text-right',
               sortable: true
            },
            {
               key: 'totalPrtnApplicationSentForReviewCount',
               label: this.$t('totalPrtnApplicationSentForReviewCount'),
               thClass: 'text-right',
               tdClass: 'text-right',
               sortable: true
            },

            {
               key: 'totalPrtnApplicationSentRejectedCount',
               label: this.$t('totalPrtnApplicationSentRejectedCount'),
               thClass: 'text-right',
               tdClass: 'text-right',
               sortable: true
            },

            {
               key: 'totalPrtnContractCount',
               label: this.$t('totalPrtnContractCount'),
               thClass: 'text-right',
               tdClass: 'text-right',
               sortable: true
            },

            {
               key: 'totalPrtnApplicationSentForExpertisesCount',
               label: this.$t('totalPrtnApplicationSentForExpertisesCount'),
               thClass: 'text-right',
               tdClass: 'text-right',
               sortable: true
            },

            {
               key: 'totalPrtnApplicationNotPassExpertisesCount',
               label: this.$t('totalPrtnApplicationNotPassExpertisesCount'),
               thClass: 'text-right',
               tdClass: 'text-right',
               sortable: true
            },

            {
               key: 'totalPrtnApplicationPassExpertisesCount',
               label: this.$t('totalPrtnApplicationPassExpertisesCount'),
               thClass: 'text-right',
               tdClass: 'text-right',
               sortable: true
            },
            {
               key: 'totalPrtnContractCancelCount',
               label: this.$t('totalPrtnContractCancelCount'),
               thClass: 'text-right',
               tdClass: 'text-right',
               sortable: true
            },

            {
               key: 'totalPrtnApplicationSignningCount',
               label: this.$t('totalPrtnApplicationSignningCount'),
               thClass: 'text-right',
               tdClass: 'text-right',
               sortable: true
            },

            {
               key: 'totalPrtnApplicationSignedCount',
               label: this.$t('totalPrtnApplicationSignedCount'),
               thClass: 'text-right',
               tdClass: 'text-right',
               sortable: true
            },

            {
               key: 'totalPrtnCertificateCount',
               label: this.$t('totalPrtnCertificateCount'),
               thClass: 'text-right',
               tdClass: 'text-right',
               sortable: true
            },

            {
               key: 'totalNewVacanciesCount',
               label: this.$t('totalNewVacanciesCount'),
               thClass: 'text-right',
               tdClass: 'text-right',
               sortable: true
            },
            {
               key: 'totalPrtnCertificateCanceledCount',
               label: this.$t('TotalPrtnCertificateCanceledCount'),
               thClass: 'text-right',
               tdClass: 'text-right',
               sortable: true
            },
            {
               key: 'totalPrtnApplicationIsOffersCount',
               label: this.$t('totalPrtnApplicationIsOffersCount'),
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
         this.filter.byDistrict = true;
         this.filter.byRegion = false;
         this.filter.byContractor = false;
         this.filter.regionId = item.regionId;
         this.filter.region = item.region;
         this.GetDistrict(item.regionId);
         this.Refresh();
      },
      SortDistrict(item) {
         this.filter.byDistrict = false;
         this.filter.byRegion = false;
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
            this.filter.districtId = null;

            this.filter.byDistrict = true;
            this.filter.byRegion = false;
            this.filter.byContractor = false;

            this.filter.region = this.filter.regionId
               ? this.RegionList.filter((item) => item.value === this.filter.regionId)[0].text
               : '';

            this.Refresh();
            this.GetDistrict(id);
         } else {
            this.filter.districtId = null;
            this.filter.byDistrict = false;
            this.filter.byRegion = true;
            this.filter.region = '';
            this.filter.byContractor = false;
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
      ChangeDistrict(id) {
         if (id) {
            this.filter.byDistrict = false;
            this.filter.byRegion = false;
            this.filter.byContractor = true;
            this.filter.district = this.filter.districtId
               ? this.DistrictList.filter((item) => item.value === this.filter.districtId)[0].text
               : '';
            this.Refresh();
         } else {
            this.filter.byDistrict = true;
            this.filter.byRegion = false;
            this.filter.byContractor = false;
            this.filter.district = '';
            this.Refresh();
         }

         // this.GetDistrict(id);
      },
      BindValue(value) {
         this.filter.dateofbirth = value;
      },
      Print() {
         this.PrintLoading = true;
         ReportService.SaveAsExecel(this.filter)
            .then((res) => {
               this.forceFileDownload(res, this.$t('prtnapplicationandcontractinfo'));
            })
            .finally(() => {
               this.PrintLoading = false;
            });
      },
      PrintForSum() {
         this.PrintForSumLoading = true;
         ReportService.SaveAsExecelForSum(this.filter)
            .then((res) => {
               this.forceFileDownload(res, this.$t('prtnapplicationandcontractinfo'));
            })
            .finally(() => {
               this.PrintForSumLoading = false;
            });
      },
      changeTotal() {
         this.filter.haslocation = null;
         this.filter.isParij = null;
         this.Refresh();
      },
      Refresh() {
         this.getFields();
         // if (!this.filter.prtnContractTypeId) {
         //   this.makeToast(
         //     `${this.$t("prtnContractType")} ${this.$t("NotSelect")}`,
         //     "danger"
         //   );
         //   return false;
         // }
         this.isBusy = true;
         ReportService.GetPrtnApplicationAndContractInfo(this.filter)
            .then((res) => {
               this.items = res.data;

               this.totals = {
                  totalPrtnApplicationSentCount: 0,
                  totalPrtnApplicationSentForReviewCount: 0,
                  totalPrtnApplicationCanceledCount: 0,
                  totalPrtnCertificateCanceledCount: 0,
                  totalPrtnApplicationSentRejectedCount: 0,
                  totalPrtnContractCount: 0,

                  totalApplicationCount: 0,
                  totalPrtnApplicationNotPassExpertisesCount: 0,
                  totalPrtnApplicationPassExpertisesCount: 0,
                  totalPrtnApplicationSignedCount: 0,
                  totalPrtnApplicationSignningCount: 0,
                  totalPrtnApplicationSentAcceptedCount: 0,
                  totalPrtnCertificateCount: 0,
                  totalNewVacanciesCount: 0,
                  totalPrtnApplicationSentForExpertisesCount: 0,
                  totalPrtnContractCancelCount: 0,
                  totalPrtnContractRejectedCount: 0,
                  totalPrtnApplicationIsOffersCount: 0
               };

               res.data.forEach((item) => {
                  this.totals.totalPrtnApplicationSentCount += item.totalPrtnApplicationSentCount;
                  this.totals.totalPrtnApplicationSentForReviewCount += item.totalPrtnApplicationSentForReviewCount;
                  this.totals.totalPrtnApplicationCanceledCount += item.totalPrtnApplicationCanceledCount;
                  this.totals.totalPrtnCertificateCanceledCount += item.totalPrtnCertificateCanceledCount;
                  this.totals.totalPrtnApplicationSentRejectedCount += item.totalPrtnApplicationSentRejectedCount;
                  this.totals.totalPrtnContractCount += item.totalPrtnContractCount;

                  this.totals.totalPrtnApplicationNotPassExpertisesCount +=
                     item.totalPrtnApplicationNotPassExpertisesCount;
                  this.totals.totalPrtnApplicationPassExpertisesCount += item.totalPrtnApplicationPassExpertisesCount;
                  this.totals.totalPrtnApplicationSignedCount += item.totalPrtnApplicationSignedCount;
                  this.totals.totalPrtnApplicationSignningCount += item.totalPrtnApplicationSignningCount;
                  this.totals.totalPrtnApplicationSentAcceptedCount += item.totalPrtnApplicationSentAcceptedCount;

                  this.totals.totalPrtnCertificateCount += item.totalPrtnCertificateCount;
                  this.totals.totalNewVacanciesCount += item.totalNewVacanciesCount;

                  this.totals.totalPrtnApplicationSentForExpertisesCount +=
                     item.totalPrtnApplicationSentForExpertisesCount;
                  this.totals.totalPrtnContractCancelCount += item.totalPrtnContractCancelCount;
                  this.totals.totalPrtnContractRejectedCount += item.totalPrtnContractRejectedCount;
                  this.totals.totalPrtnApplicationIsOffersCount += item.totalPrtnApplicationIsOffersCount;
               });

               this.totals.totalApplicationCount =
                  this.totals.totalPrtnApplicationSentCount +
                  this.totals.totalPrtnApplicationSentForReviewCount +
                  this.totals.totalPrtnContractCount;

               this.isBusy = false;
            })
            .catch((error) => {
               this.isBusy = false;
               this.showApiError(error);
            });
      }
   }
};
</script>

<style lang="scss" scoped>
@import '../styles.scss';
</style>
