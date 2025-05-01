<template>
   <b-card no-body>
      <!-- search -->
      <div class="m-1" v-if="!hideFilter">
         <slot name="filter">
            <b-row>
               <b-col cols="12" md="6" class="d-flex align-items-center justify-content-start mb-1 mb-md-0 order-left">
                  <template v-if="actions.create && $can(actions.create.permission, 'permissions')">
                     <b-button
                        variant="primary"
                        :to="{ name: actions.edit.name, params: { id: 0 }, query: actions.create.query }"
                     >
                        <feather-icon icon="PlusIcon"></feather-icon>
                        {{ $t('create') }}
                     </b-button>
                  </template>
               </b-col>
               <b-col md="2"> </b-col>
               <b-col v-if="searchable" cols="12" md="4" class="order-right">
                  <b-input-group>
                     <b-form-input v-model="filter.search" @keyup.enter="request" :placeholder="$t('search')" />
                     <b-input-group-append>
                        <b-button @click="request" variant="primary">
                           <feather-icon icon="SearchIcon" />
                        </b-button>
                     </b-input-group-append>
                  </b-input-group>
               </b-col>
            </b-row>
         </slot>
         <slot name="status"> </slot>
      </div>

      <!-- table -->
      <b-table
         v-bind="$attrs"
         v-on="$listeners"
         responsive
         primary-key="id"
         sticky-header="65vh"
         :no-border-collapse="noBorderCollapse"
         show-empty
         :hover="hover"
         :empty-text="$t('NotFound')"
         class="position-relative table-responsive-md"
         @sort-changed="SortChange"
      >
         <!-- default slot -->
         <template #cell(status)="{ item }">
            <b-badge :variant="getColor(item)">{{ item.status }} </b-badge>
         </template>
         <template #cell(actions)="{ item }">
            <div class="text-center">
               <!-- edit -->
               <template v-if="actions.edit">
                  <b-link
                     v-if="$can(actions.edit.permission, 'permissions')"
                     :to="{ name: actions.edit.name, params: { id: item.id } }"
                     style="margin-right: 5px; cursor: pointer"
                     v-b-tooltip.hover.top="$t('Edit')"
                  >
                     <feather-icon icon="EditIcon"></feather-icon>
                  </b-link>
               </template>
               <!-- delete -->
               <template v-if="actions.delete">
                  <b-link
                     v-if="$can(actions.delete.permission, 'permissions')"
                     class="text-danger"
                     @click="$refs['DeleteModal' + item.id].show()"
                     style="cursor: pointer"
                     v-b-tooltip.hover.top="$t('Delete')"
                  >
                     <feather-icon icon="TrashIcon"></feather-icon>
                  </b-link>
                  <b-modal
                     :ref="'DeleteModal' + item.id"
                     :cancel-title="$t('Cancel')"
                     :ok-title="$t('Accept')"
                     cancel-variant="danger"
                     ok-variant="success"
                     @ok="rowDelete(item)"
                  >
                     <template #modal-title>
                        {{ $t('Accept') }}
                        <b-spinner v-if="deleteLoading" small></b-spinner>
                     </template>
                     <b-card-text>
                        <h5>ID : {{ item.id }}</h5>
                        <h5>{{ $t('WantDelete') }}</h5>
                     </b-card-text>
                  </b-modal>
               </template>
            </div>
         </template>
         <template v-slot:table-busy>
            <div class="text-center text-primary my-2" style="vertical-align: middle">
               <b-spinner class="align-middle mr-2"></b-spinner>
               <strong>{{ $t('Loading') }}</strong>
            </div>
         </template>
         <!-- slots -->
         <slot v-for="(_, name) in $slots" :name="name" :slot="name"></slot>
         <template v-for="(_, name) in $scopedSlots" :slot="name" slot-scope="slotData">
            <slot :name="name" v-bind="slotData"> </slot>
         </template>
      </b-table>

      <!-- Pagination -->
      <div v-if="isPagination" class="mx-1 mb-1">
         <b-row>
            <b-col cols="12" sm="6" class="d-flex align-items-center justify-content-center justify-content-sm-start">
               <span class="text-muted">
                  {{ $t('Showing') }} {{ firstNumber }} {{ $t('to') }} {{ lastNumber }} {{ $t('of') }}
                  {{ filter.total }} {{ $t('entries') }}
               </span>
               <v-select
                  :class="[isMobileDevice() ? 'w-50' : '']"
                  v-model="filter.pageSize"
                  :dir="$store.state.appConfig.isRTL ? 'rtl' : 'ltr'"
                  :options="filter.perPageOptions"
                  @input="request"
                  :clearable="false"
                  class="per-page-selector d-inline-block ml-50 mr-1"
               />
            </b-col>
            <b-col cols="12" sm="6" class="d-flex align-items-center justify-content-center justify-content-sm-end">
               <b-pagination
                  v-model="filter.page"
                  :total-rows="filter.total"
                  :per-page="filter.pageSize"
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
   </b-card>
</template>

<script>
import {
   BTableSimple,
   BTr,
   BTd,
   BTh,
   BTbody,
   BTfoot,
   BThead,
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
   BIconClockHistory,
   BInputGroupAppend,
   BLink,
   BModal,
   BCardText,
   BButtonGroup,
   BIcon
} from 'bootstrap-vue';

import StatusSelect from '@/views/components/document/StatusSelect.vue';
export default {
   inheritAttrs: false,
   components: {
      StatusSelect,
      BTableSimple,
      BTr,
      BTd,
      BTh,
      BTbody,
      BTfoot,
      BThead,
      BButton,
      BPagination,
      BTable,
      BCol,
      BIconClockHistory,
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
      BButtonGroup,
      BIcon
   },

   props: {
      actions: {
         type: Object,
         default: () => ({}),
         required: false
      },
      searchable: {
         type: Boolean,
         default: false
      },
      deleteLoading: {
         type: Boolean,
         default: false
      },
      filter: {
         type: Object,
         default: () => ({
            search: '',
            sortBy: '',
            orderType: 'asc',
            page: 1,
            pageSize: 20,
            perPageOptions: [10, 20, 50, 100],
            total: 0
         })
      },
      hover: {
         type: Boolean,
         default: () => true
      },
      noBorderCollapse: {
         type: Boolean,
         default: () => true
      },
      hideFilter: {
         type: Boolean,
         default: false
      },
      isPagination: {
         type: Boolean,
         default: true
      }
   },
   directives: {
      'b-tooltip': VBTooltip,
      'b-modal': VBModal
   },
   computed: {
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
   created() {
      this.request();
   },
   methods: {
      SortChange(data) {
         this.filter.sortBy = data.sortBy;
         this.filter.orderType = this.filter.orderType == 'asc' ? 'desc' : 'asc';
         this.request();
      },
      request() {
         this.$emit('request');
      },
      rowDelete(item) {
         this.$emit('row-delete', item);
      }
   }
};
</script>

<style lang="scss">
/* Jadvalni telefon ekranlari uchun moslashuvchan qilish */
@media (max-width: 576px) {
   .table-responsive-md {
      overflow-x: auto;
      -webkit-overflow-scrolling: touch;
   }
   .table-responsive-md .table {
      min-width: 100%;
      white-space: nowrap;
   }
}
</style>
