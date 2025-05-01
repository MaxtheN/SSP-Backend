<template>
   <div>
      <b-row class="my-2">
         <b-col sm="12" md="4"
            ><b-button @click="Print" :disabled="printLoding" variant="primary">
               <feather-icon icon="PrinterIcon"></feather-icon>
               {{ $t('Print') }}
            </b-button></b-col
         >
      </b-row>
      <div class="simple-table">
         <b-table-simple hover small caption-top responsive>
            <b-thead>
               <b-tr>
                  <b-th rowspan="2">{{ $t('order') }}</b-th>

                  <b-th rowspan="2">{{ $t('region') }}</b-th>
                  <b-th rowspan="2">{{ $t('district') }}</b-th>
                  <b-th rowspan="2" class="text-center">{{ $t('mfy') }}</b-th>
                  <b-th rowspan="2" class="text-center">{{ $t('organization') }}</b-th>
                  <b-th rowspan="2" class="text-center">{{ $t('inn') }}</b-th>
                  <b-th rowspan="2" class="text-center">{{ $t('dateSendedOfApplication') }}</b-th>
                  <b-th colspan="10" class="text-center">{{ $t('process') }}</b-th>
               </b-tr>
               <b-tr>
                  <b-th>{{ $t('dateBusinessmanApplicationSigned') }}</b-th>
                  <b-th>{{ $t('daysBusinessmanApplicationSigned') }}</b-th>
                  <b-th>{{ $t('businessmanStatus') }}</b-th>
                  <b-th>{{ $t('dateEmploymentApplicationSigned') }}</b-th>
                  <b-th>{{ $t('daysEmploymentApplicationSigned') }}</b-th>
                  <b-th>{{ $t('employmentStatus') }}</b-th>
                  <b-th>{{ $t('dateEconomyApplicationSigned') }}</b-th>
                  <b-th>{{ $t('daysEconomyApplicationSigned') }}</b-th>
                  <b-th>{{ $t('economyStatus') }}</b-th>
                  <b-th>{{ $t('dateOfCertificate') }}</b-th>
               </b-tr>
            </b-thead>
            <b-tbody v-if="items.length > 0">
               <b-tr v-for="(item, idx) in items" :key="idx + 'abc'">
                  <b-td>{{ idx + 1 }}</b-td>

                  <b-td class="text-right">{{ currency(item.region) }}</b-td>
                  <b-td class="text-right">{{ currency(item.district) }}</b-td>
                  <b-td class="text-right">{{ currency(item.mfy) }}</b-td>
                  <b-td class="text-right">{{ currency(item.organization) }}</b-td>
                  <b-td class="text-right" style="white-space: nowrap">{{ currency(item.inn) }}</b-td>
                  <b-td class="text-right">{{ currency(item.dateSendedOfApplication) }}</b-td>
                  <b-td class="text-right">{{ currency(item.dateBusinessmanApplicationSigned) }}</b-td>
                  <b-td class="text-right">{{ currency(item.daysBusinessmanApplicationSigned) }}</b-td>
                  <b-td class="text-right">{{ currency(item.businessmanStatus) }}</b-td>
                  <b-td class="text-right">{{ currency(item.dateEmploymentApplicationSigned) }}</b-td>
                  <b-td class="text-right">{{ currency(item.daysEmploymentApplicationSigned) }}</b-td>
                  <b-td class="text-right">{{ currency(item.employmentStatus) }}</b-td>
                  <b-td class="text-right">{{ currency(item.dateEconomyApplicationSigned) }}</b-td>
                  <b-td class="text-right">{{ currency(item.daysEconomyApplicationSigned) }}</b-td>
                  <b-td class="text-right">{{ currency(item.economyStatus) }}</b-td>
                  <b-td class="text-right">{{ currency(item.dateOfCertificate) }}</b-td>
               </b-tr>
            </b-tbody>
         </b-table-simple>
      </div>
      <div class="mx-2 mb-2">
         <b-row>
            <b-col cols="12" sm="6" class="d-flex align-items-center justify-content-center justify-content-sm-start">
               <span class="text-muted">
                  {{ $t('Showing') }} {{ firstNumber }} {{ $t('to') }} {{ lastNumber }} {{ $t('of') }}
                  {{ filters.total }}
                  {{ $t('entries') }}
               </span>
               <v-select
                  v-model="filters.pageSize"
                  :dir="$store.state.appConfig.isRTL ? 'rtl' : 'ltr'"
                  :options="filters.perPageOptions"
                  @input="Refresh"
                  :clearable="false"
                  class="per-page-selector d-inline-block ml-50 mr-1"
               />
            </b-col>
            <!-- Pagination -->
            <b-col cols="12" sm="6" class="d-flex align-items-center justify-content-center justify-content-sm-end">
               <b-pagination
                  v-model="filters.page"
                  :total-rows="filters.total"
                  :per-page="filters.pageSize"
                  first-number
                  last-number
                  @input="Refresh"
                  class="mb-0 mt-1 mt-sm-0"
                  prev-class="prev-item"
                  next-class="next-item"
               >
                  <template #prev-text>
                     <feather-icon icon="ChevronLeftIcon" size="18" />
                  </template>
                  <template #next-text>
                     <feather-icon icon="ChevronRightIcon" size="18" />
                  </template>
               </b-pagination>
            </b-col>
         </b-row>
      </div>
   </div>
</template>

<script>
import ReportService from '@/services/report/report.service';

import {
   BFormSelect,
   BButton,
   BPagination,
   BTableSimple,
   BTr,
   BTh,
   BTd,
   BThead,
   BTbody,
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
   BCardText
} from 'bootstrap-vue';
export default {
   components: {
      BFormSelect,
      BButton,
      BPagination,
      BTableSimple,
      BTr,
      BTh,
      BTd,
      BThead,
      BTbody,
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
      BCardText
   },
   data() {
      return {
         items: [],
         fields: [
            {
               key: 'order',
               label: '№',
               thClass: 'text-center',
               tdClass: 'text-center'
            },
            {
               key: 'region',
               label: this.$t('region'),
               thClass: 'text-center',
               tdClass: 'text-center'
            },
            {
               key: 'district',
               label: this.$t('district'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },

            {
               key: 'mfy',
               label: this.$t('mfy'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'organizationName',
               label: this.$t('organizationName'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'inn',
               label: this.$t('inn'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'dateSendedOfApplication',
               label: this.$t('dateSendedOfApplication'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'dateApplicationSigned',
               label: this.$t('dateApplicationSigned'),
               thClass: 'text-center',
               tdClass: 'text-center'
            },
            {
               key: 'daysLateToApplicationSign',
               label: this.$t('daysLateToApplicationSign'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'status',
               label: this.$t('status'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            }
         ],
         printLoding: false,

         isBusy: false
      };
   },
   props: {
      filters: {
         type: Object,
         default: {}
      }
   },
   watch: {
      filters: {
         handler() {
            this.Refresh(); // Call the refresh function when filters change
         },
         deep: true
      }
   },
   computed: {
      firstNumber() {
         return (this.filters.page - 1) * this.filters.pageSize + 1;
      },
      lastNumber() {
         if (this.filters.total < this.filters.pageSize) {
            return this.filters.total;
         } else {
            if (this.filters.page * this.filters.pageSize > this.filters.total) {
               return this.filters.total;
            } else {
               return this.filters.page * this.filters.pageSize;
            }
         }
      }
   },
   created() {
      this.Refresh();
   },
   methods: {
      Print() {
         this.printLoding = true;
         ReportService.PrintPrtnApplicationByContractNewInfo(this.filters).then((res) => {
            this.forceFileDownload(res, this.$t('GetSmsLogReport'));
            this.printLoding = false;
         });
      },
      SortChange(data) {
         this.filters.sortBy = data.sortBy;
         this.filters.orderType = data.sortDesc ? 'desc' : 'asc';
         this.Refresh();
      },
      Refresh() {
         this.isBusy = true;
         ReportService.GetPrtnApplicationByContractNewInfo(this.filters)
            .then((res) => {
               this.items = res.data.rows;
               this.filters.total = res.data.total;
               this.isBusy = false;
            })

            .catch((error) => {
               this.showApiError(error);
            });
      }
   }
};
</script>
<style lang="scss" scoped>
@import '/src/@core/scss/tablestyle.scss';
</style>
