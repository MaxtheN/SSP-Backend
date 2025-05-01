<template>
   <b-overlay :show="show">
      <b-container max-width="2000">
         <validation-observer ref="ValidationDTO">
            <b-card :title="$t('AdditionalAgreement')">
               <b-row>
                  <b-col cols="12" md="3">
                     <form-input-hrm
                        v-model="Data.docNumber"
                        rules="required"
                        :label="$t('docnumber')"
                        :placeholder="$t('docnumber')"
                     />
                  </b-col>
                  <b-col cols="12" md="3">
                     <form-picker v-model="Data.docOn" required :label="$t('docOn')" :placeholder="$t('docOn')" />
                  </b-col>

                  <b-col cols="12" md="6">
                     <form-select
                        :options="ApplicationTypeIdConst"
                        v-model="Data.applicationTypeId"
                        disabled
                        required-star
                        :label="$t('applicationType')"
                     ></form-select>
                  </b-col>

                  <b-col cols="12" md="3">
                     <form-input-hrm
                        v-model="Data.memshipContractDocNumber"
                        :placeholder="$t('contractNumber')"
                        :label="$t('contractNumber')"
                     />
                  </b-col>
                  <b-col cols="12" md="3">
                     <form-picker
                        v-model="Data.memshipContractDocOn"
                        :label="$t('contractDocOn')"
                        :placeholder="$t('contractDocOn')"
                     />
                  </b-col>
                  <b-col cols="12" md="3">
                     <form-input-hrm
                        v-model="Data.baseFixedMinimumValue"
                        :placeholder="$t('baseFixedMinimumValue')"
                        :label="$t('baseFixedMinimumValue')"
                     />
                  </b-col>
                  <b-col cols="12" md="3" align-self="center">
                     <b-row class="row" align-v="center">
                        <b-form-checkbox v-model="Data.canPayDivided"> {{ $t('canPayDivided') }} </b-form-checkbox>
                     </b-row>
                  </b-col>
               </b-row>
               <b-row>
                  <b-col md="12" cols="12">
                     <b-form-group :label="$t('details')">
                        <b-form-textarea v-model="Data.details" :placeholder="$t('details')"></b-form-textarea>
                     </b-form-group>
                  </b-col>
               </b-row>

               <div class="text-right">
                  <b-button @click="SaveData" size="xl" class="mt-2" :disabled="saveLoading" variant="outline-success">
                     <feather-icon icon="CheckIcon"></feather-icon>
                     {{ $t('Save') }}
                  </b-button>
               </div>
            </b-card>
         </validation-observer>
      </b-container>
   </b-overlay>
</template>

<script>
import {
   BOverlay,
   BFormGroup,
   BCard,
   BRow,
   BCol,
   BButton,
   BLink,
   BIcon,
   BBadge,
   BContainer,
   BFormTextarea,
   BFormCheckbox
} from 'bootstrap-vue';
import AdditionalAgreementService from '@/services/document/additionalagreement.service';
import ApplicationMixin from '@/mixins/application';

export default {
   components: {
      BFormGroup,
      BOverlay,
      BCard,
      BRow,
      BCol,
      BButton,
      BLink,
      BIcon,
      BBadge,
      BContainer,
      BFormTextarea,
      BFormCheckbox
   },
   mixins: [ApplicationMixin],
   data() {
      return {
         show: false,
         saveLoading: false,
         Contract: {},
         Data: {
            id: 0,
            docOn: '',
            docNumber: '',
            contractorId: 0,
            applicationTypeId: 0,
            statusId: 0,
            organizationId: 0,
            memshipContractId: 0,
            baseFixedMinimumValue: 0,
            canPayDivided: true,
            details: '',
            memshipContractDocNumber: '',
            memshipContractDocOn: ''
         }
      };
   },
   created() {
      this.GetData();
   },
   methods: {
      GetData() {
         this.show = true;
         AdditionalAgreementService.Get(this.$route.params.id)
            .then((res) => {
               this.Data = res.data;
               if (this.$route.params.id == 0) {
                  this.Data.contractorId = this.$route.query.contractorId;
                  this.Data.applicationTypeId = this.$route.query.applicationTypeId;
                  this.Data.memshipContractId = this.$route.query.memshipContractId;
                  AdditionalAgreementService.GetByMemshipContractId(this.Data.memshipContractId)
                     .then((res2) => {
                        this.Contract = res2.data;
                        this.Data.memshipContractDocNumber = res2.data.memshipContractDocNumber;
                        this.Data.memshipContractDocOn = res2.data.memshipContractDocOn;
                     })
                     .catch((error) => {
                        this.showApiError(error);
                     });
               }
            })
            .catch((error) => {
               this.showApiError(error);
            })
            .finally(() => {
               this.show = false;
            });
      },
      SaveData() {
         this.$refs.ValidationDTO.validate().then((success) => {
            if (success) {
               this.saveLoading = true;
               AdditionalAgreementService.Update(this.Data)
                  .then((res) => {
                     this.makeToast(this.$t('SaveSuccess'), 'success');
                     this.$router.push({
                        name: 'ViewAdditionalAgreement',
                        params: { id: res.data.id || this.$route.params.id }
                     });
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
