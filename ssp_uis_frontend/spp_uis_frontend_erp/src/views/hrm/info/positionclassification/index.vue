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
            name: 'EditPositionClassification',
            permission: 'PositionClassificationCreate'
         },
         edit: {
            name: 'EditPositionClassification',
            permission: 'PositionClassificationEdit'
         },
         delete: {
            name: 'EditPositionClassification',
            permission: 'PositionClassificationDelete'
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
import PositionClassificationService from '@/services/hrm/positionclassification.service';

const DefaultFilter = {
   organizationId: null,
   regionId: null,
   // isInitQuery: true,
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
               key: 'fullName',
               label: this.$t('fullname'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'shortName',
               label: this.$t('shortname'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'classRu',
               label: this.$t('classRu'),
               isAllView: true,
               sort: true
            },
            {
               key: 'nskzCodeRu',
               label: this.$t('nskzCodeRu'),
               sort: true
            },
            {
               key: 'categoryCodeRu',
               label: this.$t('categoryCodeRu'),
               sort: true
            },
            {
               key: 'rangeCodeRu',
               label: this.$t('rangeCodeRu'),
               sort: true
            },
            {
               key: 'minedCodeRu',
               label: this.$t('minedCodeRu'),
               sort: true
            },
            {
               key: 'specialityCodeRu',
               label: this.$t('specialityCodeRu'),
               sort: true
            },
            {
               key: 'typeCodeRu',
               label: this.$t('typeCodeRu'),
               sort: true
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
   created() {
      if (JSON.parse(localStorage.getItem('filterData2'))) {
         this.filter = JSON.parse(localStorage.getItem('filterData2'));
      } else {
         this.filter = DefaultFilter;
      }
   },
   methods: {
      DbClick(item) {
         this.$router.push({
            name: 'EditPositionClassification',
            params: { id: item.id }
         });
      },
      Delete(item) {
         this.deleteLoading = true;
         PositionClassificationService.Delete(item.id)
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
         PositionClassificationService.GetList(this.filter)
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
            localStorage.setItem('filterData2', JSON.stringify(newValue));
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
