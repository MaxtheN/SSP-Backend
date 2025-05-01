<template>
   <b-overlay :show="show">
      <b-row>
         <b-col sm="12" md="12" lg="12">
            <b-card>
               <b-row>
                  <b-col sm="12" md="4" class="text-left mt-2">
                     <label for>{{ $t('docnumber') }}</label>
                     <b-form-input v-model="Data.docNumber" :placeholder="$t('docnumber')"></b-form-input>
                  </b-col>
                  <b-col sm="12" md="4" class="text-left mt-1">
                     <label for>{{ $t('docdate') }}</label>
                     <date-picker
                        v-model="Data.docOn"
                        style="width: 100%"
                        size="sm"
                        lang="ru"
                        :placeholder="$t('docdate')"
                        value-type="format"
                        format="DD.MM.YYYY"
                        disabled
                     ></date-picker>
                  </b-col>

                  <b-col sm="12" md="4" class="text-left mt-1">
                     <label for>{{ $t('contractor') }}</label>
                     <b-form-input v-model="Data.contractor" disabled :placeholder="$t('contractor')"></b-form-input>
                  </b-col>

                  <b-col sm="12" md="4" class="text-left mt-2">
                     <label for>{{ $t('contractorInn') }}</label>
                     <b-form-input
                        v-model="Data.contractorInn"
                        disabled
                        :placeholder="$t('contractorInn')"
                     ></b-form-input>
                  </b-col>

                  <b-col md="4" sm="12">
                     <div class="form-group">
                        <label class="col-form-label" for>{{ $t('prtnContractType') }}</label>
                        <b-form-input
                           v-model="Data.prtnContractType"
                           disabled
                           :placeholder="$t('prtnContractType')"
                        ></b-form-input>
                     </div>
                  </b-col>
                  <b-col sm="12" md="4" class="text-left mt-2">
                     <label for>{{ $t('applicationDocNumber') }}</label>
                     <b-form-input
                        v-model="Data.applicationDocNumber"
                        :placeholder="$t('applicationDocNumber')"
                     ></b-form-input>
                  </b-col>
                  <b-col sm="12" md="4" class="text-left mt-2">
                     <label for>{{ $t('applicationDocOn') }}</label>
                     <b-form-input v-model="Data.applicationDocOn" :placeholder="$t('applicationDocOn')"></b-form-input>
                  </b-col>

                  <b-col sm="12" md="4" class="mt-1">
                     <label for>{{ $t('newVacanciesCount') }}</label>
                     <b-form-input
                        v-mask="'###########################'"
                        v-model="Data.newVacanciesCount"
                        :placeholder="$t('newVacanciesCount')"
                     ></b-form-input>
                  </b-col>
               </b-row>
               <b-row class="mt-3">
                  <b-col sm="12" md="6" lg="6" class="text-left"></b-col>
                  <b-col sm="12" md="6" lg="6" class="text-right">
                     <b-button @click="SaveData" size="sm" variant="outline-success">
                        <feather-icon icon="CheckIcon"></feather-icon>
                        {{ $t('Save') }}
                     </b-button>
                  </b-col>
               </b-row>
            </b-card>
         </b-col>
      </b-row>
   </b-overlay>
</template>

<script>
import {
   BOverlay,
   BCard,
   BRow,
   BCol,
   BFormInput,
   BTabs,
   BTab,
   BButton,
   BTable,
   BLink,
   BFormGroup,
   VBTooltip,
   BModal,
   VBModal,
   BCardText,
   BInputGroup,
   BInputGroupAppend,
   BTr,
   BTd,
   BFormTextarea,
   BFormCheckbox,
   BIcon,
   BBadge
} from 'bootstrap-vue';

import Ripple from 'vue-ripple-directive';
import flatPickr from 'vue-flatpickr-component';
import Cleave from 'vue-cleave-component';
import PrtnContractService from '@/services/document/prtncontract.service';
import '@core/scss/vue/libs/vue-flatpicker.scss';

export default {
   components: {
      BOverlay,
      BCard,
      BRow,
      BCol,
      BFormInput,
      BTabs,
      BTab,
      BButton,
      BTable,
      BLink,
      flatPickr,
      BFormGroup,
      BModal,
      BCardText,
      BInputGroup,
      BInputGroupAppend,
      BTr,
      BTd,
      BFormTextarea,
      BFormCheckbox,
      BIcon,
      BBadge,
      Cleave
   },
   directives: {
      'b-tooltip': VBTooltip,
      'b-modal': VBModal
   },
   data() {
      return {
         PrtnContractModal: false,
         ContractorList: [],
         ApplicationList: [],
         PrtnContractTypeList: [],
         show: false,
         Data: {},
         lang: 'ru',
         config: {
            dateFormat: 'd.m.Y'
         }
      };
   },

   created() {
      this.lang = localStorage.getItem('locale') || 'ru';
      this.show = true;

      PrtnContractService.GetByApplicationId(this.$route.params.id)
         .then((res) => {
            this.show = false;
            this.Data = res.data;
         })
         .catch((error) => {
            this.makeToast(
               error.response.data,

               'danger'
            );
         });
   },
   directives: {
      Ripple
   },
   methods: {
      SaveData() {
         PrtnContractService.Update(this.Data)
            .then((res) => {
               this.makeToast(this.$t('SaveSuccess'), 'success');
               this.$router.push({ name: 'PrtnContract' });
            })
            .catch((err) => {
               this.makeToast(err.response.data.errors, 'danger');
            });
      }
   },
   watch: {},
   computed: {}
};
</script>
