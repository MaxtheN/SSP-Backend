<template>
   <b-overlay>
      <b-card>
         <validation-observer ref="ValidationDTO">
            <b-row>
               <b-col sm="12" md="3" class="mb-1">
                  <form-input v-model="Data.docNumber" disabled type="text" required :label="$t('docnumber')" />
               </b-col>

               <b-col sm="12" md="3" class="mb-1">
                  <form-picker :label="$t('ondate')" disabled required v-model="Data.docOn" />
               </b-col>
               <b-col sm="12" md="6">
                  <validation-observer ref="ValidationDTOinner">
                     <b-row>
                        <b-col sm="12" md="6" class="mb-1">
                           <form-select
                              :options="YearList"
                              :disabled="isDisabled"
                              required-star
                              v-model="Data.year"
                              :label="$t('docyear')"
                           />
                        </b-col>

                        <b-col sm="12" md="6">
                           <form-select
                              :options="MonthList"
                              :disabled="isDisabled"
                              required-star
                              v-model="Data.month"
                              label="month"
                           />
                        </b-col>
                     </b-row>
                  </validation-observer>
               </b-col>
               <b-col sm="12" md="3" class="mb-1">
                  <form-input :value="SummAprojectNewVacanciesCount" disabled :label="$t('totalNewVacanciesCount')" />
               </b-col>
               <b-col sm="12" md="3" class="mb-1">
                  <form-input :value="SummTotalPaymentAmount" disabled :label="$t('totalPaymentAmount')" />
               </b-col>
               <b-col sm="12" md="3" class="mb-1">
                  <p class="mb-0">{{ $t('totalAverageSalary') }}</p>
                  <form-currency-input disabled :value="AverageSalary" />
               </b-col>
               <b-col sm="12" md="3" class="mb-1">
                  <form-input v-model="Data.details" :disabled="isDisabled" :label="$t('details')" />
               </b-col>
            </b-row>
         </validation-observer>
         <b-row>
            <b-col class="text-right mt-2">
               <b-button v-if="!isDisabled" @click="Fill" variant="primary">
                  <b-spinner v-if="isBusy" small></b-spinner>
                  <feather-icon v-else icon="AlignLeftIcon"></feather-icon>
                  {{ $t('Fill') }}
               </b-button>
               <b-button v-if="!isDisabled" class="ml-2" @click="Items = []" variant="danger">
                  <feather-icon icon="XCircleIcon"></feather-icon>
                  {{ $t('clear') }}
               </b-button>
            </b-col>
         </b-row>
         <b-row class="mt-1">
            <b-table-simple hover small caption-top responsive border class="report-table">
               <b-thead>
                  <b-tr>
                     <b-th style="padding: 20px 8px">{{ $t('T/R') }}</b-th>
                     <b-th style="padding: 20px 8px">{{ $t('TS') }}</b-th>
                     <b-th style="padding: 20px 8px">{{ $t('inn') }}</b-th>
                     <b-th style="padding: 20px 8px">{{ $t('RBMDYIO') }}</b-th>
                     <b-th style="padding: 20px 8px">{{ $t('LDYTEIO') }}</b-th>
                     <b-th style="padding: 20px 8px">{{ $t('HIH') }}</b-th>
                     <b-th style="padding: 20px 8px">{{ $t('BOIH') }}</b-th>
                  </b-tr>
               </b-thead>
               <b-tbody :busy="isBusy">
                  <b-tr v-for="(item, i) in Items" :key="i">
                     <b-td style="text-align: center; width: 20px">{{ i + 1 }}</b-td>
                     <b-td>{{ item.contractor }}</b-td>
                     <b-td>{{ item.contractorInn }}</b-td>
                     <b-td>{{ item.prtnNewVacanciesCount }}</b-td>
                     <b-td style="padding: 0 5px !important">
                        <form-currency-input
                           @input="handleInput(item, i)"
                           :disabled="isDisabled"
                           v-model.number="item.projectNewVacanciesCount"
                        />
                     </b-td>
                     <b-td class="p-0" style="padding: 0 5px !important">
                        <form-currency-input
                           :disabled="isDisabled"
                           @input="handleInput(item, i)"
                           v-model.number="item.salary"
                        />
                     </b-td>
                     <b-td class="p-0" style="padding: 0 5px !important">
                        <form-currency-input disabled v-model.number="item.averageSalary" />
                     </b-td>
                  </b-tr>
               </b-tbody>
            </b-table-simple>
         </b-row>
         <b-row>
            <b-col sm="12" md="6" lg="6" class="text-left"></b-col>
            <b-col sm="12" md="6" lg="6" class="text-right">
               <b-button @click="SaveData" size="sm" variant="outline-success">
                  <feather-icon icon="CheckIcon"></feather-icon>
                  {{ $t('Save') }}
               </b-button>
            </b-col>
         </b-row>
      </b-card>
   </b-overlay>
</template>

<script>
import FormCurrencyInput from '@/components/forms/form-currency-input.vue';
import ExecutionApplicationService from '@/services/document/executionapplication.service';
import ManualService from '@/services/others/manual.service';
import { ValidationObserver } from 'vee-validate';
import {
   BOverlay,
   BCard,
   BRow,
   BCol,
   BSpinner,
   BFormInput,
   BTable,
   BButton,
   BButtonGroup,
   BLink,
   BFormGroup,
   BModal,
   BInputGroup,
   BInputGroupAppend,
   BFormCheckbox,
   BFormTextarea,
   BTableSimple,
   BThead,
   BTr,
   BTh,
   BTd,
   BTbody,
   BTfoot,
   BFormFile,
   BIconTrash
} from 'bootstrap-vue';

export default {
   components: {
      BOverlay,
      BCard,
      BRow,
      BCol,
      BFormInput,
      BButtonGroup,
      BButton,
      BTable,
      BLink,
      BFormGroup,
      BModal,
      BSpinner,
      BInputGroup,
      BInputGroupAppend,
      BFormCheckbox,
      BFormTextarea,
      FormCurrencyInput,
      BTableSimple,
      BThead,
      BTr,
      BTh,
      BTd,
      BTbody,
      BTfoot,
      BFormFile,
      BIconTrash,
      ValidationObserver
   },

   data() {
      return {
         isDisabled: this.$route.query?.isview === 'true',
         isBusy: false,
         MonthList: [],
         YearList: [
            {
               id: 1,
               value: 2023,
               text: 2023
            },

            {
               id: 2,
               value: 2024,
               text: 2024
            },
            {
               id: 3,
               value: 2025,
               text: 2025
            },
            {
               id: 4,
               value: 2026,
               text: 2026
            }
         ],
         Items: [],
         saveLoading: false,
         Data: {
            docOn: '',
            docNumber: '',
            year: null,
            month: null,
            totalNewVacanciesCount: 0,
            totalPaymentAmount: 0,
            totalAverageSalary: 0,
            tables: []
         }
      };
   },
   created() {
      ManualService.GetMonthSelectList().then((res) => {
         this.MonthList = res.data;
      });

      ExecutionApplicationService.Get(this.$route.params.id)
         .then((res) => {
            this.Data = res.data;
            this.Items = res.data.tables;
         })
         .catch((error) => {
            this.showError(error);
         });
   },
   computed: {
      SummAprojectNewVacanciesCount() {
         let summa = 0;
         this.Items.forEach((item) => {
            summa += item.projectNewVacanciesCount;
         });

         return summa;
      },
      SummTotalPaymentAmount() {
         let summa = 0;
         this.Items.forEach((item) => {
            summa += item?.salary;
         });

         return summa;
      },

      AverageSalary() {
         if (this.SummAprojectNewVacanciesCount != 0) {
            return this.SummTotalPaymentAmount / this.SummAprojectNewVacanciesCount;
         } else {
            return '';
         }
      }
   },
   methods: {
      SaveData() {
         this.$refs.ValidationDTO.validate().then((success) => {
            this.saveLoading = true;
            if (success) {
               console.log('ddd');
               ExecutionApplicationService.Update({
                  ...this.Data,
                  tables: [...this.Items],
                  totalNewVacanciesCount: this.SummAprojectNewVacanciesCount,
                  totalPaymentAmount: this.SummTotalPaymentAmount,
                  totalAverageSalary: this.AverageSalary
               })
                  .then((res) => {
                     this.makeToast(this.$t('SaveSuccess'), 'success');
                     this.$router.push({ name: 'ExecutionApplication' });
                  })
                  .catch((err) => {
                     this.showApiError(err);
                  })
                  .finally(() => {
                     this.saveLoading = false;
                  });
            }
         });
      },
      Fill() {
         this.$refs.ValidationDTOinner.validate().then((success) => {
            if (success) {
               this.isBusy = true;
               ExecutionApplicationService.GetFromGraph({
                  year: this.Data.year,
                  month: this.Data.month
               })
                  .then((res) => {
                     this.Items = res.data;
                     this.makeToast(this.$t('SuccessMessage'), 'success');
                  })
                  .catch((err) => {
                     this.showApiError(err);
                  })
                  .finally(() => {
                     this.isBusy = false;
                  });
            }
         });
      },
      handleInput(item, i) {
         if (item.projectNewVacanciesCount != 0) {
            this.Items[i].averageSalary = item?.salary / item?.projectNewVacanciesCount;
         } else {
            this.Items[i].averageSalary = '';
         }
      }
   }
};
</script>

<style lang="scss" scoped>
@import '/src/@core/scss/tablestyle.scss';
</style>
