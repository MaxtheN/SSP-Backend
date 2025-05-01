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
            name: 'EditDepartment',
            permission: 'DepartmentCreate'
         },
         edit: {
            name: 'EditDepartment',
            permission: 'DepartmentEdit'
         },
         delete: {
            name: 'DeleteDepartment',
            permission: 'DepartmentDelete'
         }
      }"
      @row-dblclicked="DbClick"
      @request="Refresh"
      @row-delete="Delete"
   >
      <template #cell(stateId)="{ item }">
         <b-badge :variant="item.stateId == '2' ? 'light-danger' : 'light-success'">{{ item.state }}</b-badge>
      </template>
   </form-table-hrm>
</template>

<script>
import { BCard, BBadge } from 'bootstrap-vue';
import FormTableHrm from '@/components/forms/form-table-hrm.vue';
import DepartmentService from '@/services/info/department.service';

const DefaultFilter = {
   search: '',
   sortBy: '',
   orderType: 'asc',
   page: 1,
   pageSize: 20,
   perPageOptions: [10, 20, 50, 100],
   total: 0
};

export default {
   components: {
      BCard,
      BBadge,
      FormTableHrm
   },
   data() {
      return {
         loacalData: JSON.parse(localStorage.getItem('user_info')),
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
               label: this.$t('kode'),
               thClass: 'text-center',
               sortable: true
            },
            {
               key: 'orderCode',
               label: this.$t('orderCode'),
               sortable: true
            },
            {
               key: 'indicatorDepartment',
               label: this.$t('indicatorDepartment'),
               sortable: true
            },
            {
               key: 'shortName',
               label: this.$t('shortname'),
               sortable: true
            },
            {
               key: 'fullName',
               label: this.$t('fullname'),
               sortable: true
            },
            {
               key: 'parent',
               label: this.$t('parent1'),
               sortable: true
            },
            {
               key: 'organization',
               label: this.$t('organization'),
               sortable: true
            },
            {
               key: 'stateId',
               label: this.$t('state'),
               thClass: 'text-center',
               tdClass: 'text-center'
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
   created() {
      if (JSON.parse(localStorage.getItem('filterData'))) {
         this.filter = JSON.parse(localStorage.getItem('filterData'));
      } else {
         this.filter = DefaultFilter;
      }
   },
   methods: {
      DbClick(item) {
         this.$router.push({
            name: 'EditDepartment',
            params: { id: item.id }
         });
      },
      Delete(item) {
         this.deleteLoading = true;
         DepartmentService.Delete(item.id)
            .then(() => {
               this.makeToast(this.$t('DeleteSuccess'), 'success');
               this.Refresh();
            })
            .catch((error) => {
               this.makeToast(error, 'danger');
            })
            .finally(() => {
               this.deleteLoading = false;
            });
      },
      Refresh() {
         this.isBusy = true;
         DepartmentService.GetList(this.filter)
            .then((res) => {
               this.items = res.data.rows;
               this.filter.total = res.data.total;
            })
            .finally(() => {
               this.isBusy = false;
            });
      }
   },

   watch: {
      filter: {
         handler(newValue, oldValue) {
            localStorage.setItem('filterData', JSON.stringify(newValue));
            this.filter = newValue;
            if (newValue.districtId) {
               this.GetDistrict();
            }
         },
         deep: true
      }
   }
};
</script>
