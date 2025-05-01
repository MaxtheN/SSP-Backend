<script setup>
import { ref } from 'vue';
import { BButton, BFormInput, BInputGroup, BInputGroupAppend, BRow, BCol } from 'bootstrap-vue';
import FormTableHrm from '@/components/forms/form-table-hrm.vue';
import FinanceIntegrationReportService from '@/services/financeIntegration/report.service';
import { useUtils as useI18nUtils } from '@core/libs/i18n';
import ManualService from '@/services/others/manual.service';

const { t } = useI18nUtils();
const accountNumberList = ref([]);
const items = ref([]);
const isBusy = ref(false);
const filter = ref({
   search: '',
   sortBy: '',
   orderType: 'asc',
   page: 1,
   pageSize: 20,
   perPageOptions: [10, 20, 50, 100, 300],
   fromDate: '',
   toDate: '',
   accountNumber: null,
   total: 0
});

const fields = ref([
   {
      key: 'id',
      label: t('id'),
      thClass: 'text-center',
      tdClass: 'text-center',
      sortable: true
   },
   {
      key: 'id2',
      label: t('id2'),
      thClass: 'text-center',
      tdClass: 'text-center',
      sortable: true
   },
   {
      key: 'bankDate',
      label: t('bankDate'),
      thClass: 'text-center',
      tdClass: 'text-center',
      sortable: true
   },
   {
      key: 'bankDocId',
      label: t('bankDocId')
   },
   {
      key: 'clAcc',
      label: t('clAcc')
   },
   {
      key: 'clInn',
      label: t('clInn')
   },
   {
      key: 'clMfo',
      label: t('clMfo')
   },
   {
      key: 'clName',
      label: t('clName'),
      thStyle: {
         minWidth: '350px',
         maxWidth: '400px'
      }
   },
   {
      key: 'coAcc',
      label: t('coAcc')
   },
   {
      key: 'coInn',
      label: t('coInn')
   },
   {
      key: 'coMfo',
      label: t('coMfo')
   },
   {
      key: 'coName',
      label: t('coName'),
      thStyle: {
         minWidth: '350px',
         maxWidth: '400px'
      }
   },
   {
      key: 'docDate',
      label: t('docDate')
   },
   {
      key: 'docNumb',
      label: t('docNumb')
   },
   {
      key: 'finYear',
      label: t('finYear')
   },
   {
      key: 'sumPay',
      label: t('sumPay')
   },
   {
      key: 'purpose',
      label: t('purpose'),
      thStyle: {
         minWidth: '450px',
         maxWidth: '600px'
      }
   }
]);

const GetPayDocsByAccEqualsAndBankDateBetween = () => {
   isBusy.value = true;
   FinanceIntegrationReportService.GetPayDocsByAccEqualsAndBankDateBetween(filter.value)
      .then((res) => {
         items.value = res.data.rows;
         filter.value.total = res.data.total;
      })
      .finally(() => {
         isBusy.value = false;
      });
};

const getAccountNumber = () => {
   ManualService.AccountNumberSelectList().then((res) => {
      accountNumberList.value = res.data.map((e) => ({
         text: e.text,
         value: e.text
      }));
   });
};

getAccountNumber();
</script>

<template>
   <form-table-hrm
      :items="items"
      :actions="{}"
      :fields="fields"
      :filter.sync="filter"
      searchable
      :busy="isBusy"
      @request="GetPayDocsByAccEqualsAndBankDateBetween"
   >
      <!-- filtes -->
      <template #filter>
         <b-row>
            <b-col cols="12" md="3">
               <form-select
                  :options="accountNumberList"
                  placeholder="ChooseBelow"
                  label="accountNumber"
                  v-model="filter.accountNumber"
                  @input="GetPayDocsByAccEqualsAndBankDateBetween"
               />
            </b-col>
            <b-col cols="12" md="2">
               <div>
                  <label for>{{ $t('startdate') }}</label>
                  <form-picker
                     v-model="filter.fromDate"
                     :placeholder="$t('startdate')"
                     @input="GetPayDocsByAccEqualsAndBankDateBetween"
                  />
               </div>
            </b-col>
            <b-col cols="12" md="2">
               <div>
                  <label for>{{ $t('enddate') }}</label>
                  <form-picker
                     v-model="filter.toDate"
                     :placeholder="$t('enddate')"
                     @input="GetPayDocsByAccEqualsAndBankDateBetween"
                  />
               </div>
            </b-col>
            <b-col cols="12" md="4">
               <b-input-group class="mt-2">
                  <b-form-input
                     v-model="filter.search"
                     @keyup.enter="GetPayDocsByAccEqualsAndBankDateBetween"
                     :placeholder="$t('search')"
                  />
                  <b-input-group-append>
                     <b-button @click="GetPayDocsByAccEqualsAndBankDateBetween" variant="primary">
                        <feather-icon icon="SearchIcon" />
                     </b-button>
                  </b-input-group-append>
               </b-input-group>
            </b-col>
         </b-row>
      </template>
   </form-table-hrm>
</template>
