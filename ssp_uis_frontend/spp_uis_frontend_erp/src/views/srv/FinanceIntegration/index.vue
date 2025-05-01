<template>
   <div>
      <form-table-hrm
         :items="financeIntegrationList"
         :actions="{}"
         :fields="fields"
         :filter.sync="filter"
         searchable
         :busy="isBusy"
         @request="Refresh"
      >
         <!-- filtes -->
         <template #filter>
            <b-row>
               <b-col cols="12" md="3">
                  <div>
                     <label for>{{ $t('innOrPinfl') }}</label>
                     <b-input-group>
                        <b-form-input
                           v-model="filter.inn"
                           debounce="300"
                           v-mask="['##############']"
                           @keyup.enter="Refresh"
                           :placeholder="$t('innOrPinfl')"
                        />
                        <b-input-group-append>
                           <b-button @click="Refresh" size="sm" variant="primary">
                              <feather-icon icon="SearchIcon" />
                           </b-button>
                        </b-input-group-append>
                     </b-input-group>
                  </div>
               </b-col>
               <b-col></b-col>
               <b-col cols="12" md="3">
                  <label>{{ $t('search') }}</label>
                  <b-input-group>
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
         <template #cell(sumPay)="{ item }">
            {{ currency(item.sumPay) }}
         </template>
      </form-table-hrm>
   </div>
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
   BInputGroup,
   BButtonGroup,
   BInputGroupAppend,
   BTabs,
   BTab
} from 'bootstrap-vue';
import FormTableHrm from '@/components/forms/form-table-hrm.vue';
import ServiceInfoService from '@/services/srv/ServiceInfo.service';
export default {
   components: {
      BCard,
      FormTableHrm,
      BCardText,
      BButton,
      BBadge,
      BLink,
      BModal,
      BRow,
      BCol,
      BFormInput,
      BInputGroup,
      BButtonGroup,
      BInputGroupAppend,
      BTabs,
      BTab
   },
   directives: {
      'b-tooltip': VBTooltip,
      'b-modal': VBModal
   },
   data() {
      return {
         isBusy: false,
         perPageOptions: [10, 20, 50, 100, 300],
         fields: [
            {
               key: 'id',
               label: this.$t('Id'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'coName',
               label: this.$t('company'),
               thClass: 'text-center',
               tdClass: 'text-left',
               sortable: true
            },
            {
               key: 'coInn',
               label: this.$t('inn'),
               thClass: 'text-center',
               tdClass: 'text-right',
               sortable: true
            },

            {
               key: 'sumPay',
               label: this.$t('amount'),
               thClass: 'text-center',
               tdClass: 'text-right',
               tdStyle: {
                  minWidth: '200px'
               },
               sortable: true
            },
            {
               key: 'purpose',
               label: this.$t('purpose'),
               thClass: 'text-center',
               tdClass: 'text-left',
               sortable: true,
               thStyle: {
                  minwidth: '200px'
               }
            },
            {
               key: 'bankDate',
               label: this.$t('bankDate'),
               thClass: 'text-center',
               tdClass: 'text-right',
               sortable: true
            }
         ],
         financeIntegrationList: [],
         filter: {
            perPageOptions: [10, 20, 50, 100, 300],
            inn: '',
            search: '',
            sortBy: '',
            orderType: '',
            page: 1,
            pageSize: 20
         }
      };
   },

   created() {
      this.Refresh();
   },
   methods: {
      Refresh() {
         ServiceInfoService.FinanceIntegration(this.filter).then((res) => {
            this.financeIntegrationList = res.data.rows;
         });
      }
   }
   // watch: {
   //    filter: {
   //       handler(newVal) {
   //          this.Refresh(newVal);
   //       },
   //       deep: true
   //    }
   // }
};
</script>
