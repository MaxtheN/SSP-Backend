<template>
   <!-- Pagination -->
   <div class="mx-1 mb-1">
      <b-row>
         <b-col cols="12" sm="6" class="d-flex align-items-center justify-content-center justify-content-sm-start">
            <span class="text-muted">
               {{ $t('Showing') }} {{ firstNumber }} {{ $t('to') }} {{ lastNumber }} {{ $t('of') }}
               {{ innerFilter.total }}
               {{ $t('entries') }}
            </span>
            <v-select
               v-model="innerFilter.pageSize"
               :options="innerFilter.perPageOptions"
               @input="request"
               :clearable="false"
               class="per-page-selector d-inline-block ml-50 mr-1"
               :class="{ 'w-50': isMobileDevice() }"
            />
         </b-col>
         <b-col cols="12" sm="6" class="d-flex align-items-center justify-content-center justify-content-sm-end">
            <b-pagination
               v-model="innerFilter.page"
               :total-rows="innerFilter.total"
               :per-page="innerFilter.pageSize"
               first-number
               last-number
               @input="request"
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
</template>

<script>
import { BPagination, BRow, BCol } from 'bootstrap-vue';
export default {
   components: {
      BPagination,
      BRow,
      BCol
   },
   props: {
      filter: {
         type: Object,
         default: () => {}
      }
   },
   computed: {
      innerFilter: {
         get() {
            return this.filter;
         },
         set(v) {
            this.$emit('update:filter', v);
         }
      },
      firstNumber() {
         return (this.filter.page - 1) * this.filter.pageSize + 1;
      },
      lastNumber() {
         if (this.filter.totalRows < this.filter.pageSize) {
            return this.filter.totalRows;
         } else {
            if (this.filter.page * this.filter.pageSize > this.filter.totalRows) {
               return this.filter.totalRows;
            } else {
               return this.filter.page * this.filter.pageSize;
            }
         }
      }
   },
   methods: {
      request() {
         this.$emit('request');
      }
   }
};
</script>
