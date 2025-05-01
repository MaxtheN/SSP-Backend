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
         <b-table
            :fields="fields"
            :items="items"
            show-empty
            @sort-changed="SortChange"
            :empty-text="$t('NotFound')"
            :busy="isBusy"
         >
            <template #cell(order)="{ item, index }"> {{ index + 1 }}</template>
            <template #cell(status)="{ item }">
               <b-badge :variant="getColor(item)">{{ item.status }}</b-badge>
            </template>
            <!-- <template #cell(adress)="{ item }"> {{ item.contractorRegion }} {{ item.contractorDistrict }}</template> -->
            <template v-slot:table-busy>
               <div class="text-center text-primary my-2" style="vertical-align: middle">
                  <b-spinner class="align-middle mr-2"></b-spinner>
                  <strong>{{ $t('Loading') }}</strong>
               </div>
            </template>
         </b-table>
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
   BCardText
} from 'bootstrap-vue';
export default {
   components: {
      BFormSelect,
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
               key: 'contractorInn',
               label: this.$t('contractorInn'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'sentForExamination',
               label: this.$t('sentForExamination'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'dateOfConclusionByJustice',
               label: this.$t('dateOfConclusionByJustice'),
               thClass: 'text-center',
               tdClass: 'text-center'
            },
            {
               key: 'daysLate',
               label: this.$t('daysLate'),
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
         ReportService.PrintPrtnApplicationByPetitionInfo(this.filters).then((res) => {
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
         ReportService.PrtnApplicationByPetitionInfo(this.filters)
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
