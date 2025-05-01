<template>
   <b-table-simple hover small caption-top responsive border>
      <b-thead>
         <b-tr>
            <b-th rowspan="3">{{ $t('order') }}</b-th>
            <b-th rowspan="3" class="table-b-table-default b-table-sticky-column">
               <p v-show="filter.byRegion">{{ $t('region') }}</p>
               <p v-show="filter.byDistrict">{{ $t('Region') }}</p>
               <p v-show="filter.byContractor">{{ $t('contractorT') }}</p>
            </b-th>
            <b-th colspan="12">{{ $t('Пуллик хизматлар') }}</b-th>
            <b-th :colspan="4">{{ $t('Бепул хизматлар') }}</b-th>
         </b-tr>
         <b-tr>
            <b-th colspan="2">{{ $t('reja') }}</b-th>
            <b-th colspan="2">{{ $t('ofThem') }}</b-th>
            <b-th colspan="2">{{ $t('Ижро ҳолати') }}</b-th>
            <b-th colspan="3">{{ $t('ofThem') }}</b-th>
            <b-th colspan="2">{{ $t('Ижро фоизда') }}</b-th>
            <b-th rowspan="2">{{ $t('Коэффициент ҳисобида') }}</b-th>
            <b-th rowspan="2">{{ $t('reja') }}</b-th>
            <b-th colspan="2">{{ $t('Ижро ҳолати') }}</b-th>
            <b-th rowspan="2">{{ $t('Коэффициент ҳисобида') }}</b-th>
         </b-tr>
         <b-tr>
            <b-th>{{ $t('count') }}</b-th>
            <b-th>{{ $t('Суммаси') }}</b-th>
            <b-th>{{ $t('юридик') }}</b-th>
            <b-th>{{ $t('иқтисод') }}</b-th>
            <b-th>{{ $t('count') }}</b-th>
            <b-th>{{ $t('Суммаси') }}</b-th>
            <b-th>{{ $t('юридик') }}</b-th>
            <b-th>{{ $t('иқтисод') }}</b-th>
            <b-th>{{ $t('биржа') }}</b-th>
            <b-th>{{ $t('count') }}</b-th>
            <b-th>{{ $t('Суммаси') }}</b-th>
            <b-th>{{ $t('Рақамда') }}</b-th>
            <b-th>{{ $t('Фоизда') }}</b-th>
         </b-tr>
      </b-thead>
      <b-tbody>
         <b-tr v-for="(item, idx) in items" :key="idx + 'abc'">
            <b-td>{{ idx + 1 }}</b-td>
            <b-td class="table-b-table-default b-table-sticky-column">
               <span v-show="filter.byRegion" style="color: blue; cursor: pointer" @click="SortRegion(item)">{{
                  item.region
               }}</span>
               <span v-show="filter.byDistrict" style="color: blue; cursor: pointer" @click="SortDistrict(item)"
                  >{{ item.district ? item.district : $t('Hududiy_boshqarma') }}
               </span>
               <span v-show="filter.byContractor" style="color: blue; cursor: pointer">{{ item.contractor }} </span>
            </b-td>
            <b-td class="text-right">{{ currency(item.planPaidCount) }}</b-td>
            <b-td class="text-right">{{ currency(item.planPaidSum) }}</b-td>
            <b-td class="text-right">{{ currency(item.legalAmount) }}</b-td>
            <b-td class="text-right">{{ currency(item.economomyAmount) }}</b-td>
            <b-td class="text-right">{{ currency(item.totalMemshipPaymentOrderCount?.totalCount ?? 0) }}</b-td>
            <b-td class="text-right">{{ currency(item.totalMemshipPaymentOrderCount?.totalSum ?? 0) }}</b-td>
            <b-td class="text-right">{{ currency(item.totalMemshipPaymentOrderCount?.legalAmount ?? 0) }}</b-td>
            <b-td class="text-right">{{ currency(item.totalMemshipPaymentOrderCount?.economomyAmount ?? 0) }}</b-td>
            <b-td class="text-right">{{ currency(item.totalMemshipPaymentOrderCount?.birjaAmount ?? 0) }}</b-td>
            <b-td class="text-right"> {{ item.acceptedPaidCountPercentage }}</b-td>
            <b-td class="text-right">{{ item.acceptedPaidSumPercentage }} </b-td>
            <b-td class="text-right">{{ item.paidCoef }} </b-td>
            <b-td class="text-right">{{ currency(item.planFreeCount) }}</b-td>
            <b-td class="text-right">{{ currency(item.acceptedFreeCount) }}</b-td>
            <b-td class="text-right">{{ currency(item.acceptedFreePercentage) }}</b-td>
            <b-td class="text-right">{{ currency(item.freecoef) }}</b-td>
         </b-tr>
      </b-tbody>
      <b-tfoot>
         <b-tr variant="secondary">
            <b-td></b-td>
            <b-td class="text-center b-table-sticky-column">{{ $t('Total') }}</b-td>
            <b-td class="text-right">{{ currency(totals.planPaidCount) }}</b-td>
            <b-td class="text-right">{{ currency(totals.planPaidSum) }}</b-td>
            <b-td class="text-right">{{ currency(totals.legalAmount) }}</b-td>
            <b-td class="text-right">{{ currency(totals.economomyAmount) }}</b-td>
            <b-td class="text-right">{{ currency(totals.totalMemshipPaymentOrderCount?.totalCount ?? 0) }}</b-td>
            <b-td class="text-right">{{ currency(totals.totalMemshipPaymentOrderCount?.totalSum ?? 0) }}</b-td>
            <b-td class="text-right">{{ currency(totals.totalMemshipPaymentOrderCount?.legalAmount ?? 0) }}</b-td>
            <b-td class="text-right">{{ currency(totals.totalMemshipPaymentOrderCount?.economomyAmount ?? 0) }}</b-td>
            <b-td class="text-right">{{ currency(totals.totalMemshipPaymentOrderCount?.birjaAmount ?? 0) }}</b-td>
            <b-td class="text-right">{{ currency(totals.acceptedPaidCountPercentage) }}</b-td>
            <b-td class="text-right">{{ totals.acceptedPaidSumPercentage }} </b-td>
            <b-td class="text-right">{{ totals.paidCoef }} </b-td>
            <b-td class="text-right">{{ currency(totals.planFreeCount) }}</b-td>
            <b-td class="text-right">{{ currency(totals.acceptedFreeCount) }}</b-td>
            <b-td class="text-right">{{ currency(totals.acceptedFreePercentage) }}</b-td>
            <b-td class="text-right">{{ currency(totals.freecoef) }}</b-td>
         </b-tr>
      </b-tfoot>
   </b-table-simple>
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
   BTfoot,
   BOverlay,
   BTabs,
   BTab
} from 'bootstrap-vue';
export default {
   name: 'PaidTable',
   props: {
      items: {
         type: Array,
         required: true,
         default: () => []
      },
      filter: {
         type: Object,
         required: true,
         default: () => ({})
      },
      totals: {
         type: Object,
         required: true,
         default: () => ({})
      }
   },
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
      BTfoot,
      BOverlay,
      BTabs,
      BTab
   },
   methods: {
      SortRegion(item) {
         this.$emit('sortRegion', item);
      },
      SortDistrict(item) {
         this.$emit('sortDistrict', item);
      }
   }
};
</script>
<style lang="scss" scoped>
@import './styles.scss';
</style>
