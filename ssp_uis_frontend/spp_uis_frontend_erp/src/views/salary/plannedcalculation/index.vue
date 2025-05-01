<template>
   <form-table-hrm
      :items="items"
      :fields="fields"
      :filter.sync="filter"
      searchable
      :busy="isBusy"
      :actions="{
         create: {
            name: 'EditPlannedCalculation',
            permission: 'PlannedCalculationCreate'
         },
         edit: {
            name: 'EditPlannedCalculation',
            permission: 'PlannedCalculationEdit'
         },
         delete: {
            name: 'EditPlannedCalculation',
            permission: 'PlannedCalculationDelete'
         }
      }"
      @request="Refresh"
      @row-delete="Delete"
   >
      <template #cell(actions)="{ item }">
         <div class="text-center" style="text-wrap: nowrap">
            <!-- edit -->
            <b-link
               v-if="$can('PlannedCalculationEdit', 'permissions') && item.canEdit"
               :to="{ name: 'EditPlannedCalculation', params: { id: item.id } }"
               v-b-tooltip.hover.top="$t('Edit')"
               class="mr-1 cursor-pointer"
            >
               <feather-icon icon="EditIcon"></feather-icon>
            </b-link>
            <!-- delete -->
            <b-link
               v-if="$can('PlannedCalculationDelete', 'permissions') && item.canDelete"
               class="text-danger mr-1 cursor-pointer"
               @click="Delete(item)"
               v-b-tooltip.hover.top="$t('Delete')"
            >
               <feather-icon icon="TrashIcon"></feather-icon>
            </b-link>
         </div>
      </template>
   </form-table-hrm>
</template>

<script>
import { BCard, BLink, VBTooltip } from 'bootstrap-vue';
import FormTableHrm from '@/components/forms/form-table-hrm.vue';
import PlannedCalculationService from '@/services/hrm/plannedcalculation.service';

export default {
   components: {
      BCard,
      BLink,
      FormTableHrm
   },
   directives: {
      'b-tooltip': VBTooltip
   },
   data() {
      return {
         items: [],
         fields: [
            {
               key: 'id',
               label: this.$t('id'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },

            {
               key: 'docNumber',
               label: this.$t('docnumber'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'docOn',
               label: this.$t('docOn'),
               thClass: 'text-center',
               tdClass: 'text-center'
            },
            {
               key: 'organization',
               label: this.$t('organization'),
               thClass: 'text-center',
               tdClass: 'text-center'
            },
            {
               key: 'calculationKind',
               label: this.$t('calculationKind'),
               thClass: 'text-center',
               tdClass: 'text-center'
            },
            {
               key: 'orgSettlementAccount',
               label: this.$t('orgSettlementAccount'),
               thClass: 'text-center',
               tdClass: 'text-center'
            },
            {
               key: 'roundingType',
               label: this.$t('roundingType'),
               thClass: 'text-center',
               tdClass: 'text-center'
            },
            {
               key: 'isCancelation',
               label: this.$t('isCancelation'),
               thClass: 'text-center',
               tdClass: 'text-center'
            },
            {
               key: 'status',
               label: this.$t('status'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'actions',
               label: this.$t('actions'),
               thClass: 'text-center',
               tdClass: 'text-center'
            }
         ],
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
               return PlannedCalculationService.Delete(item.id)
                  .then(() => {
                     this.makeToast(this.$t('DeleteSuccess'), 'success');
                     this.Refresh();
                  })
                  .catch(this.SwalError);
            },
            allowOutsideClick: () => !this.$swal.isLoading()
         });
      },
      Refresh() {
         this.isBusy = true;
         PlannedCalculationService.GetList(this.filter)
            .then((res) => {
               this.items = res.data.rows;
               this.filter.total = res.data.total;
            })
            .finally(() => {
               this.isBusy = false;
            });
      }
   }
};
</script>
