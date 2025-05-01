<template>
   <b-overlay :show="isBusy">
      <b-card>
         <b-row class="mb-1">
            <b-col class="order-left">
               <b-button
                  variant="primary"
                  v-if="$can('SrvApplicationYearlyPlanCreate', 'permissions')"
                  :to="{ name: 'EditSrvYearlyPlan', params: { id: 0 } }"
               >
                  <feather-icon icon="PlusIcon"></feather-icon>
                  {{ $t('create') }}
               </b-button>
            </b-col>
            <b-col cols="12" md="4" class="order-right">
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
         <div class="report-table">
            <b-table-simple
               hover
               small
               caption-top
               responsive
               bordered
               :busy="isBusy"
               show-empty
               :empty-text="$t('NotFound')"
               class="position-relative"
               border
            >
               <b-thead>
                  <b-tr>
                     <b-th class="text-center" style="width: 30px" rowspan="3">
                        {{ this.$t('№') }}
                     </b-th>
                     <b-th class="text-center" rowspan="3">
                        {{ this.$t('region') }}
                     </b-th>
                     <b-th class="text-center" colspan="2">
                        {{ this.$t('reja') }}
                     </b-th>
                     <b-th class="text-center" rowspan="3">
                        {{ this.$t('tekin') }}
                     </b-th>

                     <b-th class="text-center" rowspan="3">
                        {{ this.$t('ondate') }}
                     </b-th>
                     <b-th class="text-center" rowspan="3">
                        {{ this.$t('status') }}
                     </b-th>
                     <b-th class="text-center" rowspan="3">
                        {{ this.$t('actions') }}
                     </b-th>
                  </b-tr>
                  <b-tr>
                     <b-th class="text-center" rowspan="2">
                        {{ this.$t('reja(soni)') }}
                     </b-th>
                     <b-th class="text-center" rowspan="2">
                        {{ this.$t('reja(summasi)') }}
                     </b-th>
                  </b-tr>
                  <b-tr> </b-tr>
               </b-thead>
               <b-tbody>
                  <b-tr v-for="(item, index) in items" :key="index">
                     <b-td class="text-center">{{ index + 1 }}</b-td>
                     <b-td class="text-center">{{ item.region }}</b-td>
                     <b-td class="text-center"> {{ currency(item.paidCount) }}</b-td>
                     <b-td class="text-center"> {{ currency(item.amount) }}</b-td>
                     <b-td class="text-center"> {{ currency(item.freeCount) }} </b-td>
                     <b-td class="text-center">{{ item.docOn }}</b-td>
                     <b-td class="text-center">
                        <b-badge :variant="getColor(item)">{{ item.status }}</b-badge>
                     </b-td>
                     <b-td class="text-center">
                        <div class="text-center" style="text-wrap: nowrap">
                           <!-- view -->
                           <b-link
                              :to="{
                                 name: 'EditSrvYearlyPlan',
                                 query: { isView: true },
                                 params: { id: item.id }
                              }"
                              v-b-tooltip.hover.top="$t('View')"
                              style="margin-right: 15px"
                           >
                              <feather-icon icon="EyeIcon"></feather-icon>
                           </b-link>
                           <!-- edit -->
                           <b-link
                              v-if="$can('SrvApplicationYearlyPlanEdit', 'permissions') && item.canEdit"
                              :to="{ name: 'EditSrvYearlyPlan', params: { id: item.id } }"
                              v-b-tooltip.hover.top="$t('Edit')"
                              class="mr-1 cursor-pointer"
                           >
                              <feather-icon icon="EditIcon"></feather-icon>
                           </b-link>
                           <!-- delete -->
                           <b-link
                              v-if="$can('SrvApplicationYearlyPlanDelete', 'permissions') && item.canDelete"
                              class="text-danger mr-1 cursor-pointer"
                              @click="Delete(item)"
                              v-b-tooltip.hover.top="$t('Delete')"
                           >
                              <feather-icon icon="TrashIcon"></feather-icon>
                           </b-link>
                           <!-- accept -->
                           <b-link
                              v-if="$can('SrvApplicationYearlyPlanAccept', 'permissions') && item.canAccept"
                              @click="Accept(item)"
                              class="mr-1 text-success cursor-pointer"
                              v-b-tooltip.hover.top="$t('Approve')"
                           >
                              <feather-icon icon="CheckCircleIcon"></feather-icon>
                           </b-link>
                           <!-- cancel -->
                           <!-- <template v-if="$can('SrvYearlyPlanCancel', 'permissions') && item.canCancel"> -->
                           <b-link
                              v-if="$can('SrvApplicationYearlyPlanCancel', 'permissions') && item.canCancel"
                              class="text-danger cursor-pointer mr-1"
                              v-b-tooltip.hover.top="$t('Cancel')"
                              @click="Cancel(item)"
                           >
                              <feather-icon icon="XCircleIcon"></feather-icon>
                           </b-link>
                        </div>
                     </b-td>
                  </b-tr>
               </b-tbody>
            </b-table-simple>
         </div>
      </b-card>
   </b-overlay>
</template>

<script>
import {
   BFormInput,
   BInputGroup,
   BInputGroupAppend,
   BRow,
   BCol,
   BCard,
   BLink,
   VBTooltip,
   BOverlay,
   BTable,
   BBadge,
   BTd,
   BTh,
   BTr,
   BThead,
   BTbody,
   BTableSimple,
   BButton
} from 'bootstrap-vue';
import FormTableHrm from '@/components/forms/form-table-hrm.vue';
import SrvYearlyPlanService from '@/services/srv/SrvYearlyPlan.service';

export default {
   components: {
      BButton,
      BFormInput,
      BInputGroup,
      BInputGroupAppend,
      BRow,
      BCol,
      BLink,
      BCard,
      FormTableHrm,
      BOverlay,
      BTable,
      BTd,
      BTh,
      BTr,
      BThead,
      BTbody,
      BTableSimple,
      BBadge
   },
   directives: {
      'b-tooltip': VBTooltip
   },
   data() {
      return {
         items: [],

         filter: {
            search: '',
            sortBy: '',
            orderType: 'asc',
            page: 1,
            pageSize: 20,
            perPageOptions: [10, 20, 50, 100],
            total: 0
         },
         isBusy: false
      };
   },
   methods: {
      Delete(item) {
         this.$swal.fire({
            icon: 'info',
            title: this.$t('WantDelete'),
            showLoaderOnConfirm: true,
            preConfirm: () => {
               return SrvYearlyPlanService.Delete(item.id)
                  .then(() => {
                     this.makeToast(this.$t('DeleteSuccess'), 'success');
                     this.Refresh();
                  })
                  .catch(this.SwalError);
            },
            allowOutsideClick: () => !this.$swal.isLoading()
         });
      },

      Accept(item) {
         this.$swal.fire({
            icon: 'info',
            title: this.$t('WantAccept'),
            showLoaderOnConfirm: true,
            preConfirm: () => {
               return SrvYearlyPlanService.Accept({
                  id: item.id,
                  message: ''
               })
                  .then(() => {
                     this.makeToast(this.$t('AcceptSuccess'), 'success');
                     this.Refresh();
                  })
                  .catch(this.SwalError);
            },
            allowOutsideClick: () => !this.$swal.isLoading()
         });
      },
      Cancel(item) {
         this.$swal.fire({
            icon: 'question',
            title: this.$t('WantCancel'),
            showLoaderOnConfirm: true,
            preConfirm: () => {
               return SrvYearlyPlanService.Cancel({
                  statusId: item.statusId,
                  id: item.id
               })
                  .then(() => {
                     this.makeToast(this.$t('CancelMessage'), 'success');
                     this.Refresh();
                  })
                  .catch(this.SwalError);
            },
            allowOutsideClick: () => !this.$swal.isLoading()
         });
      },
      Refresh() {
         this.isBusy = true;
         SrvYearlyPlanService.GetList(this.filter)
            .then((res) => {
               this.items = res.data.rows;
               this.filter.total = res.data.total;
            })
            .finally(() => {
               this.isBusy = false;
            });
      }
   },
   created() {
      this.Refresh();
   }
};
</script>
<style lang="scss">
.report-table {
   thead {
      th {
         text-align: center;
         vertical-align: middle;
      }
   }

   td {
      white-space: nowrap;
      padding: 0.6rem 0.7rem !important;
   }

   .table:not(.table-dark) {
      td,
      th {
         border: 1px solid #ebe9f1 !important;
      }
   }
}
</style>
