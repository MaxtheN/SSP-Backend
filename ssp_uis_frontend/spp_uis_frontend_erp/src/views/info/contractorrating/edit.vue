<template>
   <b-card>
      <validation-observer ref="ValidationDTO">
         <b-row>
            <b-col sm="12" md="2">
               <form-input required v-model="Data.code" :label="$t('T/p')" :placeholder="$t('T/p')" />
            </b-col>
            <b-col sm="12" md="2">
               <form-input required v-model="Data.orderCode" :label="$t('code')" :placeholder="$t('code')" />
            </b-col>
            <b-col sm="12" md="2">
               <form-select
                  required-star
                  :options="ContractorTypeList"
                  v-model="Data.contractorTypeId"
                  :label="$t('Mulkchilik turi')"
               ></form-select>
            </b-col>
            <b-col sm="12" md="2">
               <form-input
                  required
                  required-star
                  type="number"
                  rules="required"
                  v-model="Data.minimumPercentage"
                  :label="$t('minimumPercentage')"
                  :placeholder="$t('minimumPercentage')"
               />
            </b-col>
            <b-col sm="12" md="2">
               <form-input
                  required
                  required-star
                  type="number"
                  rules="required"
                  v-model="Data.maximumPercentage"
                  :label="$t('maximumPercentage')"
                  :placeholder="$t('maximumPercentage')"
               />
            </b-col>
            <b-col sm="12" md="2">
               <form-input
                  required
                  v-model="Data.score"
                  type="number"
                  :label="$t('score')"
                  :placeholder="$t('score')"
               />
            </b-col>

            <b-col sm="12" md="2">
               <form-select
                  required-star
                  :options="StateList"
                  v-model="Data.stateId"
                  :label="$t('state')"
               ></form-select>
            </b-col>
         </b-row>
      </validation-observer>
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
</template>
<script>
import ManualService from '@/services/others/manual.service';
import ContractorRatingService from '@/services/info/contractorrating.service';
import { ValidationObserver } from 'vee-validate';
import {
   BOverlay,
   BCard,
   BCardBody,
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
   BFormCheckbox
} from 'bootstrap-vue';

export default {
   components: {
      BOverlay,
      ValidationObserver,
      BCard,
      BCardBody,
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
      BFormCheckbox
   },
   name: 'CreateOrUpdateRole',

   data() {
      return {
         StateList: [],
         Data: {
            code: '',
            ratingId: null,
            contractorTypeId: null,
            orderCode: '',
            shortName: '',
            fullName: '',
            minimumPercentage: null,
            maximumPercentage: null,
            score: null,
            translates: []
         },

         ContractorTypeList: []
      };
   },
   created() {
      ContractorRatingService.Get(this.$route.params.id).then((res) => {
         this.Data = res.data;
      });

      ManualService.StateSelectList()
         .then((res) => {
            this.StateList = res.data;
         })
         .catch((error) => {
            this.showApiError(error);
         });

      ManualService.ContractorTypeSelectList()
         .then((res) => {
            this.ContractorTypeList = res.data;
         })
         .catch((error) => {
            this.makeToast(error.response.data.error, 'danger');
         });
   },
   methods: {
      SaveData() {
         ContractorRatingService.Update({
            ...this.Data,
            minimumPercentage: +this.Data.minimumPercentage,
            maximumPercentage: +this.Data.maximumPercentage,
            score: +this.Data.score
         })
            .then((res) => {
               this.makeToast(this.$t('SaveSuccess'), 'success');
               this.$router.push({ name: 'ContractorRating' });
            })
            .catch((err) => {
               this.makeToast(this.$t(err), 'danger');
            });
      }
   }
};
</script>
<style scoped>
legend {
   background-color: #000;
   color: #fff;
   padding: 3px 6px;
}

.output {
   font: 1rem 'Fira Sans', sans-serif;
}

input {
   margin: 0.4rem;
}
</style>
