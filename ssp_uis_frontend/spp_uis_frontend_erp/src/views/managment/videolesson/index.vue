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
            name: 'EditVideoLesson',
            permission: 'VideoLessonCreate'
         },
         edit: {
            name: 'EditVideoLesson',
            permission: 'VideoLessonEdit'
         },
         delete: {
            name: 'EditVideoLesson',
            permission: 'VideoLessonDelete'
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
import VideoLessonService from '@/services/managment/videolesson.service';

export default {
   components: {
      BCard,
      BBadge,
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
               key: 'number',
               label: this.$t('kode'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'orderCode',
               label: this.$t('orderCode'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'theme',
               label: this.$t('theme'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'tag',
               label: this.$t('tag'),
               thClass: 'text-center',
               tdClass: 'text-center',
            },
            {
               key: 'uri',
               label: this.$t('uri'),
               thClass: 'text-center',
               tdClass: 'text-center',
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
         this.$router.push({
            name: 'EditVideoLesson',
            params: { id: item.id }
         });
      },
      Delete(item) {
         this.deleteLoading = true;
         VideoLessonService.Delete(item.id)
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
         VideoLessonService.GetList(this.filter)
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
