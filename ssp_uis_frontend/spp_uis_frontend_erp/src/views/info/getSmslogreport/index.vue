<template>
   <b-card>
      <div>
         <b-row class="my-2">
            <b-col sm="6" md="6" lg="1" class="order-sms-3">
               <v-select
                  :options="filters.perPageOptions"
                  v-model="filters.pageSize"
                  :clearable="false"
                  @input="Refresh"
                  class="per-page-selector d-inline-block"
               >
               </v-select>
            </b-col>
            <b-col sm="6" md="4" class="order-sms-4"
               ><b-button @click="Print" :disabled="printLoding" variant="primary">
                  <feather-icon icon="PrinterIcon"></feather-icon>
                  {{ $t('Print') }}
               </b-button></b-col
            >
            <b-col></b-col>
            <b-col cols="12" md="3" class="order-sms-2">
               <b-input-group class="text-right">
                  <b-form-input v-model="filters.inn" @keyup.enter="Refresh" :placeholder="$t('inn')" />
                  <b-input-group-append>
                     <b-button @click="Refresh" variant="primary">
                        <feather-icon icon="SearchIcon" />
                     </b-button>
                  </b-input-group-append>
               </b-input-group>
            </b-col>

            <b-col cols="12" md="3" class="order-sms-1">
               <b-input-group class="text-right">
                  <b-form-input v-model="filters.search" @keyup.enter="Refresh" :placeholder="$t('search')" />
                  <b-input-group-append>
                     <b-button @click="Refresh" variant="primary">
                        <feather-icon icon="SearchIcon" />
                     </b-button>
                  </b-input-group-append>
               </b-input-group>
            </b-col>
            <!-- <b-col></b-col> -->
         </b-row>

         <b-table
            sticky-header="65vh"
            no-border-collapse
            :fields="fields"
            :items="items"
            show-empty
            @sort-changed="SortChange"
            :empty-text="$t('NotFound')"
            :busy="isBusy"
         >
            <template #cell(order)="{ item, index }"> {{ index + 1 }}</template>
            <template v-slot:table-busy>
               <div class="text-center text-primary my-2" style="vertical-align: middle">
                  <b-spinner class="align-middle mr-2"></b-spinner>
                  <strong>{{ $t('Loading') }}</strong>
               </div>
            </template>
            <template #cell(inn)="{ item }">
               <span style="color: blue">{{ item.inn }}</span>
            </template>
            <template #cell(toStatus)="{ item }">
               <b-badge :variant="getColor({ statusId: item.toStatusId, status: item.toStatus })">{{
                  item.toStatus
               }}</b-badge>
            </template>
         </b-table>
         <div class="mx-2 mb-2">
            <b-row>
               <b-col
                  cols="12"
                  sm="6"
                  class="d-flex align-items-center justify-content-center justify-content-sm-start"
               >
                  <span class="text-muted">
                     {{ $t('Showing') }} {{ firstNumber }} {{ $t('to') }} {{ lastNumber }} {{ $t('of') }}
                     {{ filters.total }}
                     {{ $t('entries') }}
                  </span>
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
   </b-card>
</template>

<script>
import ReportService from '@/services/report/report.service';

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
   BCardText
} from 'bootstrap-vue';
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
               key: 'createdUserId',
               label: this.$t('User ID'),
               thClass: 'text-center',
               tdClass: 'text-center'
            },
            {
               key: 'createdAt',
               label: this.$t('Sana'),
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
               key: 'phoneNumer',
               label: this.$t('phoneNumber'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },

            {
               key: 'smsText',
               label: this.$t('smsText'),
               thClass: 'text-center',
               tdClass: 'text-center'
            },
            {
               key: 'table',
               label: this.$t('Hujjat turi'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'fromStatus',
               label: this.$t('Holatdan'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'toStatus',
               label: this.$t('Holatga'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            }
         ],
         printLoding: false,
         filters: {
            fromStatusId: null,
            toStatusId: null,
            inn: '',
            search: '',
            sortBy: 'createdAt',
            orderType: 'desc',
            pageSize: 20,
            page: 1,
            perPageOptions: [10, 20, 50, 100, 500],
            total: 0
         },
         isBusy: false
      };
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
         ReportService.SaveAsExcelGetSmsLog(this.filters).then((res) => {
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
         ReportService.GetSmsLogReport(this.filters)
            .then((res) => {
               this.items = res.data.item1;
               this.filters.total = res.data.item2;
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
@media only screen and (max-width: 992px) {
   .order-sms-1 {
      order: 1;
      margin-bottom: 1rem;
   }
   .order-sms-2 {
      order: 2;
      margin-bottom: 1rem;
   }
   .order-sms-3 {
      order: 3;
   }
   .order-sms-4 {
      order: 4;
      text-align: end;
   }
}
</style>
