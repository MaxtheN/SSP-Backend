<template>
   <b-overlay :show="isBusy">
      <b-table-simple hover small caption-top responsive border>
         <b-thead>
            <b-tr>
               <b-th rowspan="2">{{ $t('order') }}</b-th>
               <b-th rowspan="2" class="table-b-table-default" :class="{ 'b-table-sticky-column': !isMobileDevice() }">
                  <span v-show="filter.byRegion">{{ $t('region') }}</span>
                  <span v-show="filter.byDistrict">{{ $t('Region') }}</span>
                  <span v-show="filter.byContractor">{{ $t('contractorT') }}</span>
               </b-th>
               <b-th rowspan="2">{{ $t('certificateCount') }}</b-th>
               <b-th rowspan="2">{{ $t('newVacanciesCount') }}</b-th>

               <template v-if="tab == 0">
                  <b-th colspan="2" class="text-center">{{ $t('GreenLane') }}</b-th>
                  <b-th colspan="2" class="text-center">{{ $t('Applications') }}</b-th>
                  <b-th colspan="2" class="text-center">{{ $t('RejectedApplications') }}</b-th>
                  <b-th colspan="2" class="text-center">{{ $t('SatisfiedApplications') }}</b-th>
                  <b-th rowspan="2">{{ $t('custom5') }}</b-th>
               </template>
               <template v-if="tab == 1">
                  <b-th>{{ $t('NumberOfEntitiesScheme') }}</b-th>
                  <b-th>{{ $t('NumberOfCasesWasUsed') }}</b-th>
                  <b-th>{{ $t('NumberOfJobsCreated') }}</b-th>
               </template>
               <template v-if="tab == 2">
                  <b-th class="text-center">{{ $t('TheNumberPayment') }}</b-th>
                  <b-th class="text-center">{{ $t('TheNumberSubjectInstallments') }}</b-th>
                  <b-th class="text-center">{{ $t('TheAmountCustomsInstallments') }}</b-th>
               </template>
            </b-tr>
            <b-tr v-if="tab == 0">
               <b-th>{{ $t('Яшил йўлак тартибидан фойдаланган субъектлар сони') }}</b-th>
               <b-th>{{ $t('custom4') }}</b-th>
               <b-th>{{ $t('appContractorCount') }}</b-th>
               <b-th>{{ $t('custom1') }}</b-th>
               <b-th>{{ $t('rejContractorCount') }}</b-th>
               <b-th>{{ $t('custom3') }}</b-th>
               <b-th>{{ $t('devContractorCount') }}</b-th>
               <b-th>{{ $t('custom2') }}</b-th>
            </b-tr>
         </b-thead>
         <b-tbody v-if="items.length > 0">
            <b-tr v-for="(item, idx) in items" :key="idx + 'abc'">
               <b-td>{{ idx + 1 }}</b-td>
               <b-td class="table-b-table-default" :class="{ 'b-table-sticky-column': !isMobileDevice() }">
                  <span v-show="filter.byRegion" style="color: blue; cursor: pointer" @click="SortRegion(item)">{{
                     item.region
                  }}</span>

                  <span v-show="filter.byDistrict" style="color: blue; cursor: pointer" @click="SortDistrict(item)">{{
                     item.district
                  }}</span>

                  <span v-show="filter.byContractor">
                     {{ item.contractorInn }} -
                     {{ item.contractor }}
                  </span>
               </b-td>
               <b-td class="text-right">{{ currency(item.certificateCount) }}</b-td>
               <b-td class="text-right">{{ currency(item.newVacanciesCount) }}</b-td>

               <template v-if="tab == 0">
                  <b-td class="text-right">{{ currency(item.grChanContractorCount) }}</b-td>
                  <b-td class="text-right">{{ currency(item.grChanCount) }}</b-td>
                  <b-td class="text-right">{{ currency(item.appContractorCount) }}</b-td>
                  <b-td class="text-right">{{ currency(item.appCount) }}</b-td>
                  <b-td class="text-right">{{ currency(item.rejContractorCount) }}</b-td>
                  <b-td class="text-right">{{ currency(item.rejCount) }}</b-td>
                  <b-td class="text-right">{{ currency(item.devContractorCount) }}</b-td>
                  <b-td class="text-right">{{ currency(item.devCount) }}</b-td>
                  <b-td class="text-right">{{ currency(item.sum) }}</b-td>
               </template>
               <template v-else-if="tab == 1">
                  <b-td class="text-right">{{ currency(item.grChanContractorCount) }}</b-td>
                  <b-td class="text-right">{{ currency(item.grChanCount) }}</b-td>
                  <b-td class="text-right">{{ currency(item.grChanNewVacanciesCount) }}</b-td>
               </template>
               <template v-else-if="tab == 2">
                  <b-td class="text-right">{{ currency(item.appContractorCount) }}</b-td>
                  <b-td class="text-right">{{ currency(item.devContractorCount) }}</b-td>
                  <b-td class="text-right">{{ currency(item.sum) }}</b-td>
               </template>
            </b-tr>
         </b-tbody>
         <b-tfoot v-if="items.length > 0">
            <b-tr variant="secondary">
               <template v-if="tab == 0">
                  <b-td class="text-right" colspan="2">{{ $t('Total') }}</b-td>
                  <b-td class="text-right">{{ currency(itemsTotal.certificateCount) }}</b-td>
                  <b-td class="text-right">{{ currency(itemsTotal.newVacanciesCount) }}</b-td>
                  <b-td class="text-right">{{ currency(itemsTotal.grChanContractorCount) }}</b-td>
                  <b-td class="text-right">{{ currency(itemsTotal.grChanCount) }}</b-td>
                  <b-td class="text-right">{{ currency(itemsTotal.appContractorCount) }}</b-td>
                  <b-td class="text-right">{{ currency(itemsTotal.appCount) }}</b-td>
                  <b-td class="text-right">{{ currency(itemsTotal.rejContractorCount) }}</b-td>
                  <b-td class="text-right">{{ currency(itemsTotal.rejCount) }}</b-td>
                  <b-td class="text-right">{{ currency(itemsTotal.devContractorCount) }}</b-td>
                  <b-td class="text-right">{{ currency(itemsTotal.devCount) }}</b-td>
                  <b-td class="text-right">{{ currency(itemsTotal.sum) }}</b-td>
               </template>
               <template v-else-if="tab == 1">
                  <b-td class="text-right" colspan="2">{{ $t('Total') }}</b-td>
                  <b-td class="text-right">{{ currency(itemsTotal.certificateCount) }}</b-td>
                  <b-td class="text-right">{{ currency(itemsTotal.newVacanciesCount) }}</b-td>
                  <b-td class="text-right">{{ currency(itemsTotal.grChanContractorCount) }}</b-td>
                  <b-td class="text-right">{{ currency(itemsTotal.grChanCount) }}</b-td>
                  <b-td class="text-right">{{ currency(itemsTotal.grChanNewVacanciesCount) }}</b-td>
               </template>
               <template v-else-if="tab == 2">
                  <b-td class="text-right" colspan="2">{{ $t('Total') }}</b-td>
                  <b-td class="text-right">{{ currency(itemsTotal.certificateCount) }}</b-td>
                  <b-td class="text-right">{{ currency(itemsTotal.newVacanciesCount) }}</b-td>
                  <b-td class="text-right">{{ currency(itemsTotal.appContractorCount) }}</b-td>
                  <b-td class="text-right">{{ currency(itemsTotal.devContractorCount) }}</b-td>
                  <b-td class="text-right">{{ currency(itemsTotal.sum) }}</b-td>
               </template>
            </b-tr>
         </b-tfoot>
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

defineProps({
   items: {
      type: Array,
      default: () => []
   },
   filter: Object,
   isBusy: Boolean,
   itemsTotal: {
      type: Object,
      default: () => ({})
   },
   SortDistrict: Function,
   SortRegion: Function,
   tab: {
      type: Number,
      default: 1
   }
});

const { $router } = getCurrentInstance().proxy;

const goToBussnes = (inn) => {
   $router.push({
      name: 'BusinessmanCard',
      query: { inn: inn }
   });
};
</script>
<style lang="scss" scoped>
@import '../../styles.scss';
</style>
