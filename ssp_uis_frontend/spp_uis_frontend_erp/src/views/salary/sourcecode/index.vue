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
            name: 'EditSourceCode',
            permission: 'TaxBenefitTypeCreate'
         },
         edit: {
            name: 'EditSourceCode',
            permission: 'TaxBenefitTypeEdit'
         },
         delete: {
            name: 'EditSourceCode',
            permission: 'TaxBenefitTypeDelete'
         }
      }"
      @row-dblclicked="DbClick"
      @request="Refresh"
      @row-delete="Delete"
   />
</template>

<script>
import { BCard } from 'bootstrap-vue';
import FormTableHrm from '@/components/forms/form-table-hrm.vue';
import SourceCodeService from '@/services/hrm/sourcecode.service';

export default {
   components: {
      BCard,
      FormTableHrm
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
               key: 'code',
               label: this.$t('code'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'fullName',
               label: this.$t('fullname'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'shortName',
               label: this.$t('shortname')
            },
            {
               key: 'state',
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
         this.$router.push({
            name: 'EditTaxBenefitType',
            params: { id: item.id }
         });
      },
      Delete(item) {
         this.deleteLoading = true;
         SourceCodeService.Delete(item.id)
            .then(() => {
               this.makeToast(this.$t('DeleteSuccess'), 'success');
               this.Refresh();
            })
            .catch((error) => {
               this.showApiError(error);
            })
            .finally(() => {
               this.deleteLoading = false;
            });
      },
      Refresh() {
         this.isBusy = true;
         SourceCodeService.GetList(this.filter)
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
