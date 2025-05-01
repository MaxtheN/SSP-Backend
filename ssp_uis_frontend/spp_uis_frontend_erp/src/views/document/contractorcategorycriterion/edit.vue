<template>
   <b-overlay :show="show">
      <b-card>
         <validation-observer ref="ValidationDTO">
            <b-row>
               <b-col sm="12" md="3">
                  <form-input-hrm
                     rules="required"
                     v-model="Data.docNumber"
                     :placeholder="$t('docnumber')"
                     :label="$t('docnumber')"
                     :name="$t('docnumber')"
                  />
               </b-col>
               <b-col sm="12" md="3">
                  <form-picker
                     v-model="Data.docOn"
                     type="date"
                     format="DD.MM.YYYY"
                     :label="$t('docOn')"
                     :name="$t('docOn')"
                     required
                  />
               </b-col>
               <b-col sm="12" md="3">
                  <form-select
                     v-model="Data.contractorCategoryId"
                     :options="ContractorCategoryList"
                     required-star
                     :label="$t('contractorCategory')"
                  />
               </b-col>
               <b-col sm="12" md="3">
                  <form-picker
                     v-model="Data.expirationDate"
                     type="date"
                     format="DD.MM.YYYY"
                     :label="$t('expirationDate')"
                     :name="$t('expirationDate')"
                  />
               </b-col>
               <b-col sm="12" md="3">
                  <form-currency-input
                     :rules="Data.maxAmount?'':'required|numeric'"
                     v-model="Data.minAmount"
                     :label="$t('minAmount')"
                     :name="$t('minAmount')"
                     :placeholder="$t('minAmount')"
                  />
               </b-col>
               <b-col sm="12" md="3">
                  <form-currency-input
                     :rules="Data.minAmount?'':'required|numeric'"
                     v-model="Data.maxAmount"
                     :label="$t('maxAmount')"
                     :placeholder="$t('maxAmount')"
                     :name="$t('maxAmount')"
                  />
               </b-col>
            </b-row>
            <b-row>
               <b-col v-if="Data.statusId !=2" sm="12" md="6" lg="6" align-self="end" offset-md="6" class="text-right">
                  <b-button :disabled="saveLoading" @click="SaveData" variant="outline-success">
                     <feather-icon icon="CheckIcon"></feather-icon>
                     {{ $t('Save') }}
                  </b-button>
               </b-col>
            </b-row>
         </validation-observer>
      </b-card>
   </b-overlay>
</template>
<script>
// service
import ContractorCategoryCriterionService from '@/services/document/contractorcategorycriterion.service';
// components
import { BOverlay, BCard, BRow, BCol, BTable, BButton, BLink, BFormFile, BIconTrash } from 'bootstrap-vue';
import ManualService from '@/services/others/manual.service';
import axios from 'axios';

export default {
   components: {
      BOverlay,
      BCard,
      BRow,
      BCol,
      BButton,
      BTable,
      BLink,
      BFormFile,
      BIconTrash
   },
   name: 'ContractorCategoryCriterionEdit',
   data() {
      return {
         show: false,
         loadingButton: false,
         saveLoading: false,
         Data: {},
         ContractorCategoryList: []
      };
   },

   created() {
      this.show = true;

      ContractorCategoryCriterionService.Get(this.$route.params.id)
         .then((res) => {
            this.Data = res.data;
         })
         .catch((error) => {
            this.makeToast(error, 'danger');
         })
         .finally(() => {
            this.show = false;
         });
      ManualService.ContractorCategorySelectList().then((res) => {
         this.ContractorCategoryList = res.data;
      });
   },
   methods: {
      SaveData() {
         this.$refs.ValidationDTO.validate().then((success) => {
            if (success) {
               this.saveLoading = true;
               ContractorCategoryCriterionService.Update(this.Data)
                  .then((res) => {
                     this.makeToast(this.$t('SaveSuccess'), 'success');
                     this.$router.push({ name: 'ContractorCategoryCriterion' });
                  })
                  .catch((err) => {
                     this.showApiError(err);
                  })
                  .finally(() => {
                     this.saveLoading = false;
                  });
            }
         });
      }
   }
};
</script>
