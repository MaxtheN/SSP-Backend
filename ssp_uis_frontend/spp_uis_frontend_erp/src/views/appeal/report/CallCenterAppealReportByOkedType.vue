<template>
   <b-card>
      <b-tabs class="mb-1">
         <b-tab
            @click="
               () => {
                  filter.byOkedType = true;
                  Refresh();
               }
            "
            :title="this.$t('Мурожаат килган тадбиркорларнинг фаолият тури')"
            active
         ></b-tab>
         <b-tab
            :title="this.$t('Мурожаатлар йуналиши')"
            @click="
               () => {
                  filter.byOkedType = false;
                  Refresh();
               }
            "
         ></b-tab>
      </b-tabs>
      <b-row class="my-2">
         <b-col sm="12" md="2">
            <form-picker
               v-model="filter.fromDocOn"
               @input="Refresh"
               :label="$t('startDate')"
               :placeholder="$t('startDate')"
            />
         </b-col>
         <b-col sm="12" md="2">
            <form-picker
               @input="Refresh"
               v-model="filter.toDocOn"
               :label="$t('endDate')"
               :placeholder="$t('endDate')"
            />
         </b-col>
         <b-col cols="12" md="8" class="d-flex justify-content-end align-items-center">
            <b-button @click="Print" :disabled="PrintLoading" variant="primary" class="ml-1">
               <feather-icon icon="PrinterIcon"></feather-icon>
               {{ $t('Print') }}
            </b-button>
         </b-col>
      </b-row>
      <b-overlay :show="isBusy">
         <b-table-simple class="report-table" hover small caption-top responsive border>
            <b-thead>
               <b-tr>
                  <b-th rowspan="2">№</b-th>
                  <b-th rowspan="2">
                     <span v-if="filter.byOkedType">{{ $t('Мурожаат килган тадбиркорларнинг фаолият тури') }}</span>
                     <span v-else>{{ $t('Мурожаатлар йуналиши') }}</span>
                  </b-th>
                  <b-th colspan="2">{{ $t('Мурожаат сони') }}</b-th>
                  <b-th colspan="14">{{ $t('choosedRegion') }}</b-th>
               </b-tr>
               <b-tr>
                  <b-th>{{ $t('count') }}</b-th>
                  <b-th>{{ $t('percents') }}</b-th>
                  <b-th v-for="(item, index) in RegionList" :key="index">{{ item }} </b-th>
               </b-tr>
            </b-thead>
            <b-tbody>
               <b-tr v-for="(item, i) in items" :key="i">
                  <b-td>{{ i + 1 }} </b-td>
                  <b-td>{{ item.fullName }} </b-td>
                  <b-td style="text-align: end">{{ item.document_count }}</b-td>
                  <b-td style="text-align: end">{{ item.document_percentage }}</b-td>
                  <b-td style="text-align: end">{{ item.document_count_region_6 }}</b-td>
                  <b-td style="text-align: end">{{ item.document_count_region_3 }}</b-td>
                  <b-td style="text-align: end">{{ item.document_count_region_4 }}</b-td>
                  <b-td style="text-align: end">{{ item.document_count_region_5 }}</b-td>
                  <b-td style="text-align: end">{{ item.document_count_region_7 }}</b-td>
                  <b-td style="text-align: end">{{ item.document_count_region_8 }}</b-td>
                  <b-td style="text-align: end">{{ item.document_count_region_9 }}</b-td>
                  <b-td style="text-align: end">{{ item.document_count_region_10 }}</b-td>
                  <b-td style="text-align: end">{{ item.document_count_region_11 }}</b-td>
                  <b-td style="text-align: end">{{ item.document_count_region_12 }}</b-td>
                  <b-td style="text-align: end">{{ item.document_count_region_2 }}</b-td>
                  <b-td style="text-align: end">{{ item.document_count_region_13 }}</b-td>
                  <b-td style="text-align: end">{{ item.document_count_region_14 }}</b-td>
                  <b-td style="text-align: end">{{ item.document_count_region_1 }}</b-td>
               </b-tr>
            </b-tbody>
            <b-tfoot v-if="items.length > 0">
               <b-th colspan="2">
                  <span class="ml-3">{{ $t('Total') }}</span>
               </b-th>
               <b-th style="text-align: end"> {{ currency(totals.document_count) }} </b-th>
               <b-th style="text-align: end"> 100% </b-th>
               <b-th style="text-align: end"> {{ currency(totals.document_count_region_6) }} </b-th>
               <b-th style="text-align: end"> {{ currency(totals.document_count_region_3) }} </b-th>
               <b-th style="text-align: end"> {{ currency(totals.document_count_region_4) }} </b-th>
               <b-th style="text-align: end"> {{ currency(totals.document_count_region_5) }} </b-th>
               <b-th style="text-align: end"> {{ currency(totals.document_count_region_7) }} </b-th>
               <b-th style="text-align: end"> {{ currency(totals.document_count_region_8) }} </b-th>
               <b-th style="text-align: end"> {{ currency(totals.document_count_region_9) }} </b-th>
               <b-th style="text-align: end"> {{ currency(totals.document_count_region_10) }} </b-th>
               <b-th style="text-align: end"> {{ currency(totals.document_count_region_11) }} </b-th>
               <b-th style="text-align: end"> {{ currency(totals.document_count_region_12) }} </b-th>
               <b-th style="text-align: end"> {{ currency(totals.document_count_region_2) }} </b-th>
               <b-th style="text-align: end"> {{ currency(totals.document_count_region_13) }} </b-th>
               <b-th style="text-align: end"> {{ currency(totals.document_count_region_14) }} </b-th>
               <b-th style="text-align: end"> {{ currency(totals.document_count_region_1) }} </b-th>
            </b-tfoot>
            <template #overlay>
               <div class="text-center text-primary my-2">
                  <b-spinner scale="2" class="align-middle mr-2"></b-spinner>
                  <strong>{{ $t('Loading') }}...</strong>
               </div>
            </template>
         </b-table-simple>
      </b-overlay>
   </b-card>
</template>

<script>
import RegionService from '@/services/info/region.service';
import ReportService from '@/services/report/report.service';

import {
   BButton,
   BCard,
   BOverlay,
   BTable,
   BTableSimple,
   BThead,
   BTbody,
   BTh,
   BTd,
   BRow,
   BCol,
   BTabs,
   BTab,
   BTr,
   BTfoot
} from 'bootstrap-vue';

export default {
   components: {
      BButton,
      BCard,
      BOverlay,
      BTable,
      BTableSimple,
      BThead,
      BTbody,
      BTh,
      BTd,
      BRow,
      BCol,
      BTabs,
      BTab,
      BTr,
      BTfoot
   },
   data() {
      return {
         PrintLoading: false,
         tabIndex: 0,
         items: [],
         isBusy: false,
         RegionList: [],
         filter: {
            byOkedType: true,
            fromDocOn: '',
            toDocOn: ''
         },
         totals: {
            document_count: 0,
            document_percentage: 0,
            document_count_region_6: 0,
            document_count_region_3: 0,
            document_count_region_4: 0,
            document_count_region_5: 0,
            document_count_region_7: 0,
            document_count_region_8: 0,
            document_count_region_9: 0,
            document_count_region_10: 0,
            document_count_region_11: 0,
            document_count_region_12: 0,
            document_count_region_2: 0,
            document_count_region_13: 0,
            document_count_region_14: 0,
            document_count_region_1: 0,
            total_document_count: 0
         }
      };
   },
   created() {
      RegionService.GetAsSelectList(211)
         .then((res) => {
            res.data.forEach((item) => {
               if (item.value != 15) {
                  this.RegionList.push(item.text);
               }
            });
         })
         .catch((error) => {
            this.showApiError(error);
         });
      this.Refresh();
   },
   methods: {
      Refresh() {
         this.isBusy = true;
         ReportService.CallCenterAppealReportByOkedType(this.filter)
            .then((res) => {
               this.items = res.data;
               this.totals = {
                  document_count: 0,
                  document_percentage: 0,
                  document_count_region_6: 0,
                  document_count_region_3: 0,
                  document_count_region_4: 0,
                  document_count_region_5: 0,
                  document_count_region_7: 0,
                  document_count_region_8: 0,
                  document_count_region_9: 0,
                  document_count_region_10: 0,
                  document_count_region_11: 0,
                  document_count_region_12: 0,
                  document_count_region_2: 0,
                  document_count_region_13: 0,
                  document_count_region_14: 0,
                  document_count_region_1: 0,
                  total_document_count: 0
               };

               res.data.forEach((item) => {
                  this.totals.document_count += item.document_count;

                  this.totals.document_count_region_6 += item.document_count_region_6;
                  this.totals.document_count_region_3 += item.document_count_region_3;
                  this.totals.document_count_region_4 += item.document_count_region_4;
                  this.totals.document_count_region_5 += item.document_count_region_5;
                  this.totals.document_count_region_7 += item.document_count_region_7;
                  this.totals.document_count_region_8 += item.document_count_region_8;
                  this.totals.document_count_region_9 += item.document_count_region_9;
                  this.totals.document_count_region_10 += item.document_count_region_10;
                  this.totals.document_count_region_11 += item.document_count_region_11;
                  this.totals.document_count_region_12 += item.document_count_region_12;
                  this.totals.document_count_region_2 += item.document_count_region_2;
                  this.totals.document_count_region_13 += item.document_count_region_13;
                  this.totals.document_count_region_14 += item.document_count_region_14;
                  this.totals.document_count_region_1 += item.document_count_region_1;
                  this.totals.total_document_count += item.total_document_count;
               });

               this.isBusy = false;
            })
            .catch((error) => {
               this.showApiError(error);
            });
      },
      Print() {
         this.PrintLoading = true;
         ReportService.SaveAsExcelCallCenterAppealReportByOkedType(this.filter)
            .then((res) => {
               this.forceFileDownload(res, this.$t('CallCenterAppealReportByOkedType'));
               this.PrintLoading = false;
            })
            .catch((e) => {
               this.showApiError(e);
            });
      }
   }
};
</script>
<style lang="scss" scoped>
@import '/src/@core/scss/tablestyle.scss';
</style>
