<template>
   <form-table-hrm
      :items="items"
      :fields="fields"
      :delete-loading="deleteLoading"
      :filter.sync="filter"
      searchable
      :busy="isBusy"
      :actions="{
         create: {
            name: 'DebtEdit',
            permission: 'DebtCreate'
         },
         edit: {
            name: 'DebtEdit',
            permission: 'DebtEdit'
         },
         delete: {
            name: 'DebtEdit',
            permission: 'DebtDelete'
         }
      }"
      @row-dblclicked="DbClick"
      @request="Refresh"
      @row-delete="Delete"
   >
      <template #cell(key)="{ item, index }">
         {{ index + 1 }}
      </template>
      <template #cell(totalDebtAmount)="{ item }">
         {{ currency(item.totalDebtAmount) }}
      </template>
      <template #cell(totalEntitlementAmount)="{ item }">
         {{ currency(item.totalEntitlementAmount) }}
      </template>

      <template #cell(actions)="{ item }">
         <div class="text-center">
            <!-- edit -->

            <b-link
               v-if="$can('DebtEdit', 'permissions')"
               :to="{ name: 'DebtEdit', params: { id: item.id } }"
               style="margin-right: 5px; cursor: pointer"
               v-b-tooltip.hover.top="$t('Edit')"
            >
               <feather-icon icon="EditIcon"></feather-icon>
            </b-link>

            <!-- delete -->

            <b-link
               v-if="$can('DebtDelete', 'permissions')"
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
               @ok="Delete(item)"
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
         </div>
      </template>
   </form-table-hrm>
</template>

<script>
import { BCard, BSpinner, VBTooltip, BCardText, VBModal, BLink, BModal, BBadge } from 'bootstrap-vue';
import FormTableHrm from '@/components/forms/form-table-hrm.vue';
import MemshipDebtService from '@/services/document/debt.service.js';

export default {
   components: {
      BCard,
      BBadge,
      FormTableHrm,
      BModal,
      BLink,
      VBTooltip,
      VBModal,
      BCardText,
      BSpinner
   },
   directives: {
      'b-tooltip': VBTooltip,
      'b-modal': VBModal
   },
   data() {
      return {
         items: [],
         filter: {
            organizationId: null,
            statusId: null,
            search: '',
            sortBy: '',
            orderType: 'asc',
            page: 1,
            pageSize: 20,
            perPageOptions: [10, 20, 50, 100],
            orderType: 'asc'
         },

         fields: [
            {
               key: 'id',
               label: this.$t('№'),
               thClass: 'text-center',
               tdClass: 'text-center',
               stickyColumn: !this.isMobileDevice()
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
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'organization',
               label: this.$t('organization'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'totalEntitlementAmount',
               label: this.$t("J'ami haqdorlik(ming so'm"),
               sortable: true,
               thStyle: {
                  minWidth: '200px'
               }
            },
            {
               key: 'totalDebtAmount',
               label: this.$t("Qarzdorlik (ming so'm"),
               sortable: true,
               thStyle: {
                  minWidth: '200px'
               }
            },
            {
               key: 'contractorPhonber',
               label: this.$t('details'),
               thStyle: {
                  minWidth: '200px'
               }
            },
            {
               key: 'status',
               label: this.$t('status'),
               thClass: 'text-center',
               tdClass: 'text-center',
               stickyColumn: !this.isMobileDevice()
            },
            {
               key: 'actions',
               label: this.$t('actions'),
               thClass: 'text-center',
               tdClass: 'text-center',
               stickyColumn: !this.isMobileDevice()
            }
         ],
         isBusy: false,
         deleteLoading: false
      };
   },

   methods: {
      DbClick(item) {
         this.$router.push({
            name: 'EditArbitrationcourt',
            params: { id: item.id }
         });
      },
      Refresh() {
         this.isBusy = true;
         MemshipDebtService.GetList(this.filter)
            .then((res) => {
               this.items = res.data.rows;
               this.filter.total = res.data.total;
            })
            .finally(() => {
               this.isBusy = false;
            });
      },
      Delete(item) {
         this.deleteLoading = true;
         console.log(item.id);
         MemshipDebtService.Delete(item.id)
            .then(() => {
               this.makeToast(this.$t('DeleteSuccess'), 'success');
               this.Refresh();
            })
            .catch((error) => {
               this.makeToast(error.response.data.errors[''], 'danger');
            })
            .finally(() => {
               this.deleteLoading = false;
            });
      }
   }
};
</script>
