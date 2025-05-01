<template>
   <b-overlay :show="isBusy">
      <b-table-simple hover small caption-top responsive border class="report-table">
         <b-thead>
            <b-tr>
               <b-th rowspan="3">{{ $t('order') }}</b-th>
               <b-th
                  v-if="filter.byRegion || tab2 == 1"
                  rowspan="3"
                  class="table-b-table-default"
                  :class="{ 'b-table-sticky-column': !isMobileDevice() }"
               >
                  <span>{{ $t('region') }}</span>
               </b-th>
               <b-th
                  v-if="filter.byDistrict || tab2 == 1"
                  rowspan="3"
                  class="table-b-table-default"
                  :class="{ 'b-table-sticky-column': !isMobileDevice() }"
               >
                  <span>{{ $t('Region') }}</span>
               </b-th>
               <b-th
                  v-if="filter.byContractor || tab2 == 1"
                  rowspan="3"
                  class="table-b-table-default"
                  :class="{ 'b-table-sticky-column': !isMobileDevice() }"
               >
                  <span>{{ $t('contractorT') }}</span>
               </b-th>

               <b-th rowspan="3">{{ $t('certificateCount') }}</b-th>
               <b-th rowspan="3">{{ $t('newVacanciesCount') }}</b-th>

               <template v-if="tab == 0 || tab2 == 1">
                  <b-th rowspan="3">{{ $t('totalTaxCount') }}</b-th>
                  <b-th colspan="12" class="text-center">{{ $t('ofThem') }}</b-th>
               </template>
            </b-tr>
            <template v-if="tab == 0 || tab2 == 1">
               <TRow0 />
            </template>
            <template v-else-if="tab == 1">
               <TRow1 />
            </template>
            <template v-else-if="tab == 2">
               <TRow2 />
            </template>
            <template v-else-if="tab == 3">
               <TRow3 />
            </template>
            <template v-else-if="tab == 4">
               <TRow4 />
            </template>

            <b-tr v-if="tab == 0 || tab2 == 1">
               <b-th>{{ $t('count') }}</b-th>
               <b-th>{{ $t('amount1') }}</b-th>
               <b-th>{{ $t('count') }}</b-th>
               <b-th>{{ $t('amount1') }}</b-th>
               <b-th>{{ $t('count') }}</b-th>
               <b-th>{{ $t('amount1') }}</b-th>
               <b-th>{{ $t('count') }}</b-th>
               <b-th>{{ $t('amount1') }}</b-th>
               <b-th>{{ $t('count') }}</b-th>
               <b-th>{{ $t('amount1') }}</b-th>
            </b-tr>
         </b-thead>

         <b-tbody v-if="items.length > 0">
            <b-tr v-for="(item, idx) in items" :key="idx + 'abc'">
               <b-td>{{ idx + 1 }}</b-td>
               <b-td v-if="filter.byRegion || tab2 == 1" style="max-width: 450px">
                  <span style="color: blue; cursor: pointer" @click="tab2 != 1 && SortRegion(item)">{{
                     item.region
                  }}</span>
               </b-td>
               <b-td v-if="filter.byDistrict || tab2 == 1" style="max-width: 450px">
                  <span style="color: blue; cursor: pointer" @click="tab2 != 1 && SortDistrict(item)">
                     {{ item.district }}
                  </span>
               </b-td>
               <b-td v-if="filter.byContractor || tab2 == 1" style="max-width: 450px">
                  <div>
                     <span style="color: blue; cursor: pointer" @click="goToBussnes(item.contractorInn)">{{
                        item.contractorInn
                     }}</span>
                     -
                     <span style="text-wrap: wrap"> {{ item.contractorFullName }}</span>
                  </div>
               </b-td>

               <b-td class="text-right">{{ currency(item.certificateCount) }}</b-td>
               <b-td class="text-right">{{ currency(item.newVacanciesCount) }}</b-td>

               <template v-if="tab == 0 || tab2 == 1">
                  <b-td class="text-right">{{ currency(item.totalContractApplicationCount) }}</b-td>
                  <b-td class="text-right">{{ currency(item.incomeTaxCount) }}</b-td>
                  <b-td class="text-right">{{ currency(item.incomeTaxSum) }}</b-td>
                  <b-td class="text-right">{{ currency(item.contractorSocialTaxCount) }}</b-td>
                  <b-td class="text-right">{{ currency(item.socialTaxSum) }}</b-td>
                  <b-td class="text-right">{{ currency(item.contractorLandTaxCount) }}</b-td>
                  <b-td class="text-right">{{ currency(item.landTaxSum) }}</b-td>
                  <b-td class="text-right">{{ currency(item.contractorPropertyTaxCount) }}</b-td>
                  <b-td class="text-right">{{ currency(item.propertyTaxSum) }}</b-td>
                  <b-td class="text-right">{{ currency(item.privilegeTaxSum) }}</b-td>
                  <b-td class="text-right">{{ currency(item.privilegeTaxCount) }}</b-td>
               </template>
               <template v-else-if="tab == 1">
                  <b-td class="text-right">{{ currency(item.propertyTaxCount) }}</b-td>
                  <b-td class="text-right">{{ currency(item.contractorPropertyTaxCount) }}</b-td>
                  <b-td class="text-right">{{ currency(item.propertyNewVacanciesCount) }}</b-td>
                  <b-td class="text-right">{{ 0 }}</b-td>
                  <b-td class="text-right">{{ currency(item.propertyTaxSum) }}</b-td>
               </template>
               <template v-else-if="tab == 2">
                  <b-td class="text-right">{{ currency(item.contractorLandTaxCount) }}</b-td>
                  <b-td class="text-right">{{ currency(item.landNewVacanciesCount) }}</b-td>
                  <b-td class="text-right">{{ 0 }}</b-td>
                  <b-td class="text-right">{{ currency(item.landTaxSum) }}</b-td>
               </template>
               <template v-else-if="tab == 3">
                  <b-td class="text-right">{{ currency(item.incomeTaxCount) }}</b-td>
                  <b-td class="text-right">{{ currency(item.incomeNewVacanciesCount) }}</b-td>
                  <b-td class="text-right">{{ 0 }}</b-td>
                  <b-td class="text-right">{{ currency(item.incomeTaxSum) }}</b-td>
               </template>
               <template v-else-if="tab == 4">
                  <b-td class="text-right">{{ currency(item.contractorSocialTaxCount) }}</b-td>
                  <b-td class="text-right">{{ currency(item.socialNewVacanciesCount) }}</b-td>
                  <b-td class="text-right">{{ 0 }}</b-td>
                  <b-td class="text-right">{{ currency(item.socialTaxSum) }}</b-td>
               </template>
            </b-tr>
         </b-tbody>
         <b-tfoot v-if="tab2 != 1">
            <b-tr variant="secondary">
               <b-td class="text-right" colspan="2">{{ $t('Total') }}</b-td>
               <b-td class="text-right">{{ currency(totals.totalcertificateCount) }}</b-td>
               <b-td class="text-right">{{ currency(totals.totalnewVacanciesCount) }}</b-td>
               <template v-if="tab == 0">
                  <b-td class="text-right">{{ currency(totals.totalContractApplicationCount) }}</b-td>
                  <b-td class="text-right">{{ currency(totals.incomeTaxCount) }}</b-td>
                  <b-td class="text-right">{{ currency(totals.incomeTaxSum) }}</b-td>
                  <b-td class="text-right">{{ currency(totals.contractorSocialTaxCount) }}</b-td>
                  <b-td class="text-right">{{ currency(totals.socialTaxSum) }}</b-td>
                  <b-td class="text-right">{{ currency(totals.contractorLandTaxCount) }}</b-td>
                  <b-td class="text-right">{{ currency(totals.landTaxSum) }}</b-td>
                  <b-td class="text-right">{{ currency(totals.contractorPropertyTaxCount) }}</b-td>
                  <b-td class="text-right">{{ currency(totals.propertyTaxSum) }}</b-td>
                  <b-td class="text-right">{{ currency(totals.privilegeTaxCount) }}</b-td>
                  <b-td class="text-right">{{ currency(totals.privilegeTaxSum) }}</b-td>
               </template>
               <template v-else-if="tab == 1">
                  <b-td class="text-right">{{ currency(totals.propertyTaxCount) }}</b-td>
                  <b-td class="text-right">{{ currency(totals.contractorPropertyTaxCount) }}</b-td>
                  <b-td class="text-right">{{ currency(totals.propertyNewVacanciesCount) }}</b-td>
                  <b-td class="text-right">{{ 0 }}</b-td>
                  <b-td class="text-right">{{ currency(totals.propertyTaxSum) }}</b-td>
               </template>
               <template v-else-if="tab == 2">
                  <b-td class="text-right">{{ currency(totals.contractorLandTaxCount) }}</b-td>
                  <b-td class="text-right">{{ currency(totals.landNewVacanciesCount) }}</b-td>
                  <b-td class="text-right">{{ 0 }}</b-td>
                  <b-td class="text-right">{{ currency(totals.landTaxSum) }}</b-td>
               </template>
               <template v-else-if="tab == 3">
                  <b-td class="text-right">{{ currency(totals.incomeTaxCount) }}</b-td>
                  <b-td class="text-right">{{ currency(totals.incomeNewVacanciesCount) }}</b-td>
                  <b-td class="text-right">{{ 0 }}</b-td>
                  <b-td class="text-right">{{ currency(totals.incomeTaxSum) }}</b-td>
               </template>
               <template v-else-if="tab == 4">
                  <b-td class="text-right">{{ currency(totals.contractorSocialTaxCount) }}</b-td>
                  <b-td class="text-right">{{ currency(totals.socialNewVacanciesCount) }}</b-td>
                  <b-td class="text-right">{{ 0 }}</b-td>
                  <b-td class="text-right">{{ currency(totals.socialTaxSum) }}</b-td>
               </template>
            </b-tr>
         </b-tfoot>
         <tr role="row" v-if="items.length == 0 && !isBusy" class="b-table-empty-row">
            <td colspan="20" role="cell" class="">
               <div role="alert" aria-live="polite">
                  <div class="text-center my-2">{{ $t('NotFound') }}</div>
               </div>
            </td>
         </tr>
      </b-table-simple>
      <template #overlay>
         <div class="text-center text-primary my-2">
            <b-spinner scale="2" class="align-middle mr-2"></b-spinner>
            <strong>{{ $t('Loading') }}...</strong>
         </div>
      </template>
   </b-overlay>
</template>

<script setup>
import { getCurrentInstance } from 'vue';
import { BSpinner, BTableSimple, BThead, BTbody, BTr, BTd, BTh, BOverlay, BTfoot } from 'bootstrap-vue';

import TRow0 from './TRow0.vue';
import TRow1 from './TRow1.vue';
import TRow2 from './TRow2.vue';
import TRow3 from './TRow3.vue';
import TRow4 from './TRow4.vue';

defineProps({
   items: {
      type: Array,
      default: () => []
   },
   filter: Object,
   isBusy: Boolean,
   totals: {
      type: Object,
      default: () => ({})
   },
   SortDistrict: Function,
   SortRegion: Function,
   tab: {
      type: Number,
      default: 1
   },
   tab2: {
      type: Number,
      default: 0
   }
});

const { $router } = getCurrentInstance().proxy;

const goToBussnes = (inn) => {
   $router.push({ name: 'BusinessmanCard', query: { inn: inn } });
};
</script>
<style lang="scss" scoped>
@import '../../styles.scss';
</style>
