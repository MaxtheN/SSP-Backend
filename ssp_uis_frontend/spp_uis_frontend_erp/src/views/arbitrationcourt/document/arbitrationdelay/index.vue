<template>
   <form-table-hrm
      :items="items"
      :fields="fields"
      :delete-loading="deleteLoading"
      :filter.sync="filter"
      searchable
      :busy="isBusy"
      :actions="{
         edit: {
            name: 'EditArbitrationDelay',
            permission: true
         },
         delete: {
            name: 'EditArbitrationDelay',
            permission: true
         }
      }"
      @row-dblclicked="DbClick"
      @request="Refresh"
      @row-delete="Delete"
   >
      <template #filter>
         <b-row>
            <b-col cols="12" md="2">
               <div>
                  <label for>{{ $t('startdate') }}</label>
                  <form-picker v-model="filter.fromDocDate" :placeholder="$t('startdate')" @input="Refresh" />
               </div>
            </b-col>

            <b-col cols="12" md="2">
               <div>
                  <label for>{{ $t('enddate') }}</label>
                  <form-picker v-model="filter.toDocDate" :placeholder="$t('enddate')" @input="Refresh" />
               </div>
            </b-col>
            <b-col cols="12" md="4">
               <form-select
                  :options="OrganizationList"
                  v-model="filter.organizationId"
                  :label="$t('organization')"
                  @change="Refresh"
               ></form-select>
            </b-col>

            <b-col cols="12" md="4">
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
      <template #cell(status)="{ item }">
         <b-badge :variant="getColor(item)">{{ item.status }}</b-badge>
      </template>
      <template #cell(actions)="{ item }">
         <div class="text-center">
            <!-- edit -->

            <b-link
               :to="{ name: 'EditArbitrationDelay', params: { id: item.id } }"
               style="margin-right: 5px; cursor: pointer"
               v-b-tooltip.hover.top="$t('Edit')"
            >
               <feather-icon icon="EditIcon"></feather-icon>
            </b-link>
            <!-- delete -->

            <b-link
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
import {
   BCard,
   BInputGroup,
   BInputGroupAppend,
   BSpinner,
   VBTooltip,
   BCardText,
   VBModal,
   BLink,
   BModal,
   BBadge,
   BCol,
   BRow,
   BFormInput,
   BButton
} from 'bootstrap-vue';
import FormTableHrm from '@/components/forms/form-table-hrm.vue';
import ArbitrationDelayService from '@/services/arbitrationcourt/arbitrationdelay.service.js';
import OrganizationService from '@/services/managment/organization.service';

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
      BSpinner,
      BInputGroup,
      BInputGroupAppend,
      BCol,
      BRow,
      BFormInput,
      BButton
   },
   directives: {
      'b-tooltip': VBTooltip,
      'b-modal': VBModal
   },
   data() {
      return {
         items: [],
         OrganizationList: [],
         filter: {
            search: '',
            sortBy: '',
            orderType: 'asc',
            page: 1,
            pageSize: 20
         },
         fields: [
            {
               key: 'actions',
               label: this.$t('actions'),
               thClass: 'text-center',
               tdClass: 'text-center',
               stickyColumn: true
            },
            {
               key: 'id',
               label: this.$t('id'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'docNumber',
               label: this.$t('docNumberfordiscussion'),
               thClass: 'text-left',
               tdClass: 'text-left',
               sortable: true
            },
            {
               key: 'docOn',
               label: this.$t('docOn'),
               sortable: true
            },
            {
               key: 'contractor',
               label: this.$t('contractor'),
               thClass: 'text-left',
               tdClass: 'text-left',
               sortable: true
            },
            {
               key: 'responsibleContractor',
               label: this.$t('responsibleContractor'),
               thClass: 'text-left',
               tdClass: 'text-left',
               sortable: true
            },
            {
               key: 'organization',
               label: this.$t('organization'),
               thClass: 'text-left',
               tdClass: 'text-left',
               sortable: true
            },

            {
               key: 'status',
               label: this.$t('status'),
               thClass: 'text-center',
               tdClass: 'text-center'
            }
         ],
         isBusy: false,
         deleteLoading: false
      };
   },
   created() {
      OrganizationService.GetAsSelectList(null, null, null, 3)
         .then((res) => {
            this.OrganizationList = res.data;
         })
         .catch((error) => {
            this.showApiError(error);
         });
   },
   methods: {
      DbClick(item) {
         this.$router.push({
            name: 'EditArbitrationDelay',
            params: { id: item.id }
         });
      },
      Refresh() {
         this.isBusy = true;
         ArbitrationDelayService.GetList(this.filter)
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
         ArbitrationDelayService.Delete(item.id)
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
      }
   }
};
</script>
