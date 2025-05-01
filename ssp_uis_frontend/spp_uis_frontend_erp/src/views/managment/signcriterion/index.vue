<template>
   <form-table-hrm
      :items="items"
      :fields="fields"
      :filter.sync="filter"
      searchable
      :busy="isBusy"
      :delete-loading="deleteLoading"
      :actions="{
         create: {
            name: 'EditSignCriterion',
            permission: 'SignCriterionCreate'
         },
         edit: {
            name: 'EditSignCriterion',
            permission: 'SignCriterionEdit'
         },
         delete: {
            name: 'EditSignCriterion',
            permission: 'SignCriterionDelete'
         }
      }"
      @row-dblclicked="DbClick"
      @request="Refresh"
      @row-delete="Delete"
   >
      <template #cell(stateId)="{ item }">
         <b-badge :variant="item.stateId == '2' ? 'light-danger' : 'light-success'">{{ item.state }}</b-badge>
      </template>
      <template #cell(actions)="{ item }">
         <div class="text-center" style="text-wrap: nowrap">
            <!-- edit -->
            <b-link
               v-if="$can('SignCriterionEdit', 'permissions') && ![12, 6, 17].includes(item.statusId)"
               :to="{ name: 'EditSignCriterion', params: { id: item.id } }"
               v-b-tooltip.hover.top="$t('Edit')"
               class="mr-1 cursor-pointer"
            >
               <feather-icon icon="EditIcon"></feather-icon>
            </b-link>
            <!-- delete -->
            <b-link
               v-if="$can('SignCriterionDelete', 'permissions') && item.statusId != 17"
               class="text-danger mr-1 cursor-pointer"
               v-b-tooltip.hover.top="$t('Delete')"
               @click="Delete(item)"
            >
               <feather-icon icon="TrashIcon"></feather-icon>
            </b-link>
         </div>
      </template>
   </form-table-hrm>
</template>

<script>
import { BCard, BBadge, BLink, VBTooltip } from 'bootstrap-vue';
import FormTableHrm from '@/components/forms/form-table-hrm.vue';
import SignCriterionService from '@/services/managment/signcriterion.service';

export default {
   components: {
      BCard,
      BBadge,
      FormTableHrm,
      BLink
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
               key: 'applicationType',
               label: this.$t('applicationType'),
               thClass: 'text-center',
               tdClass: 'text-center'
            },
            {
               key: 'contractorCategory',
               label: this.$t('contractorCategory'),
               thClass: 'text-center',
               tdClass: 'text-center'
            },
            {
               key: 'organizationGroup',
               label: this.$t('group'),
               thClass: 'text-center',
               tdClass: 'text-center'
            },
            {
               key: 'position',
               label: this.$t('position'),
               thClass: 'text-center',
               tdClass: 'text-center'
            },
            {
               key: 'stateId',
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
         isBusy: false,
         deleteLoading: false
      };
   },
   methods: {
      DbClick(item) {
         if (this.$can('SignCriterionEdit', 'permissions')) {
            this.$router.push({
               name: 'EditSignCriterion',
               params: { id: item.id }
            });
         }
      },
      Delete(item) {
         this.$swal.fire({
            icon: 'info',
            title: this.$t('WantDelete'),
            showLoaderOnConfirm: true,
            preConfirm: () => {
               this.deleteLoading = true;

               return SignCriterionService.Delete(item.id)
                  .then(() => {
                     this.makeToast(this.$t('DeleteSuccess'), 'success');
                     this.Refresh();
                  })
                  .catch(this.SwalError)
                  .finally(() => {
                     this.deleteLoading = false;
                  });
            },
            allowOutsideClick: () => !this.$swal.isLoading()
         });
      },
      Refresh() {
         this.isBusy = true;
         SignCriterionService.GetList(this.filter)
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
