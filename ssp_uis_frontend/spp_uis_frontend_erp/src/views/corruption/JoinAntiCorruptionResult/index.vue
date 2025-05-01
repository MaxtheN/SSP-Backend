<template>
   <form-table-hrm
      :items="items"
      :actions="{
         create: {
            name: 'EditJoinAntiCorruptionResult',
            permission: 'JoinAntiCorruptionResultEdit'
         },
         edit: {
            name: 'EditJoinAntiCorruptionResult',
            permissions: 'JoinAntiCorruptionResultEdit'
         }
      }"
      :fields="fields"
      :filter.sync="filter"
      searchable
      :busy="isBusy"
      @row-dblclicked="DbClick"
      @request="Refresh"
      @row-delete="Delete"
   >
      <template #filter>
         <b-row>
            <b-col cols="auto">
               <b-button
                  class="mt-2"
                  variant="primary"
                  @click="$router.push({ name: 'EditJoinAntiCorruptionResult', params: { id: 0 } })"
               >
                  <feather-icon icon="PlusIcon"></feather-icon>
                  {{ $t('create') }}</b-button
               >
            </b-col>
            <b-col cols="12" lg="2">
               <form-picker
                  v-model="filter.fromDocDate"
                  :label="$t('startOn')"
                  @change="Refresh"
                  :placeholder="$t('docOn')"
               />
            </b-col>
            <b-col cols="12" lg="2">
               <form-picker
                  v-model="filter.toDocDate"
                  :label="$t('endDate')"
                  @change="Refresh"
                  :placeholder="$t('docOn')"
               />
            </b-col>
            <b-col></b-col>
            <b-col cols="12" lg="4">
               <b-input-group class="mt-2">
                  <b-form-input v-model="filter.search" @keyup.enter="Refresh" :placeholder="$t('search')" />
                  <b-input-group-append>
                     <b-button @click="Refresh" variant="primary">
                        <feather-icon icon="SearchIcon" />
                     </b-button>
                  </b-input-group-append>
               </b-input-group>
            </b-col>
         </b-row>
      </template>
      <template #cell(actions)="{ item }">
         <div class="text-center" style="text-wrap: nowrap">
            <b-link
               :to="{ name: 'EditJoinAntiCorruptionResult', params: { id: item.id } }"
               v-b-tooltip.hover.top="$t('Edit')"
               class="mr-1 cursor-pointer"
            >
               <feather-icon icon="EditIcon"></feather-icon>
            </b-link>
            <!-- delete -->
            <b-link class="text-danger mr-1 cursor-pointer" @click="Delete(item)" v-b-tooltip.hover.top="$t('Delete')">
               <feather-icon icon="TrashIcon"></feather-icon>
            </b-link>
            <!-- accept -->
            <b-link
               v-if="$can('JoinAntiCorruptionResultAccept', 'permissions') && item.canAccept"
               @click="Accept(item)"
               class="mr-1 text-success cursor-pointer"
               v-b-tooltip.hover.top="$t('Approve')"
            >
               <feather-icon icon="CheckCircleIcon"></feather-icon>
            </b-link>
            <!-- cancel -->
            <b-link
               v-if="$can('JoinAntiCorruptionResultCancel', 'permissions') && item.canCancel"
               class="text-danger cursor-pointer mr-1"
               v-b-tooltip.hover.top="$t('Cancel')"
               @click="Cancel(item)"
            >
               <feather-icon icon="XCircleIcon"></feather-icon>
            </b-link>
         </div>
      </template>
      <template #cell(status)="{ item }">
         <b-badge :variant="getColor(item)">{{ item.status }}</b-badge>
      </template>
   </form-table-hrm>
</template>

<script>
import {
   BCard,
   BCardText,
   VBTooltip,
   BBadge,
   BButton,
   BLink,
   BModal,
   VBModal,
   BRow,
   BCol,
   BFormInput,
   BInputGroupAppend,
   BInputGroup
} from 'bootstrap-vue';
import FormTableHrm from '@/components/forms/form-table-hrm.vue';
import HistoryModalButton from '@/views/components/document/HistoryModalButton.vue';
import JoinAntiCorruptionResultService from '@/services/corruption/joinanticorruptionresult.service';

export default {
   components: {
      HistoryModalButton,
      BInputGroup,
      BFormInput,
      BInputGroupAppend,
      BButton,
      BRow,
      BCol,
      BCard,
      FormTableHrm,
      BCardText,
      BButton,
      BBadge,
      BLink,
      BModal
   },
   directives: {
      'b-tooltip': VBTooltip,
      'b-modal': VBModal
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
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'chairmenFio',
               label: this.$t('chairmenFio')
            },
            {
               key: 'member1Fio',
               label: this.$t('member1Fio')
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
            perPageOptions: [10, 20, 50, 100, 300],
            total: 0,
            docOn: ''
         },
         isBusy: false
      };
   },
   methods: {
      DbClick(item) {
         this.$router.push({
            name: 'EditJoinAntiCorruptionResult',
            params: { id: item.id }
         });
      },
      Delete(item) {
         this.$swal.fire({
            icon: 'info',
            title: this.$t('WantDelete'),
            showLoaderOnConfirm: true,
            preConfirm: () => {
               return JoinAntiCorruptionResultService.Delete(item.id)
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
               return JoinAntiCorruptionResultService.Accept({
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
            input: 'text',
            inputPlaceholder: this.$t('RejectMessage'),
            preConfirm: (msg) => {
               return JoinAntiCorruptionResultService.Cancel({
                  id: item.id,
                  message: msg
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
         JoinAntiCorruptionResultService.GetList(this.filter)
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
