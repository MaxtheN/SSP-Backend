<template>
   <b-overlay :show="show">
      <b-container max-width="2000">
         <b-card :title="$t('PrtnCreditDemand')">
            <b-row>
               <b-col sm="12" md="6">
                  <form-input-hrm
                     inline
                     :value="PrtnCreditDemand.contractorInn"
                     disabled
                     :label="$t('contractorInn')"
                  />
               </b-col>
               <b-col sm="12" md="6">
                  <form-input-hrm :value="PrtnCreditDemand.contractor" disabled :label="$t('contractor')" />
               </b-col>
               <b-col sm="12" md="6">
                  <form-input-hrm :value="PrtnCreditDemand.docNumber" disabled :label="$t('docnumber')" />
               </b-col>
               <b-col sm="12" md="6">
                  <form-input-hrm :value="PrtnCreditDemand.docOn" disabled :label="$t('docOn')" />
               </b-col>

               <b-col sm="12" md="6">
                  <form-input-hrm
                     :value="PrtnCreditDemand.prtnCertificateDocNumber"
                     disabled
                     :label="$t('prtnCertificateDocNumber')"
                  />
               </b-col>
               <b-col sm="12" md="6">
                  <form-input-hrm
                     :value="PrtnCreditDemand.prtnCertificateDocOn"
                     disabled
                     :label="$t('prtnCertificateDocOn')"
                  />
               </b-col>
               <b-col sm="12" md="6">
                  <form-input-hrm :value="PrtnCreditDemand.prtnContractType" disabled :label="$t('prtnContractType')" />
               </b-col>
               <b-col sm="12" md="6">
                  <form-input-hrm
                     :value="PrtnCreditDemand.regionName + ' ' + PrtnCreditDemand.districtName"
                     disabled
                     :label="$t('address')"
                  />
               </b-col>
               <b-col sm="12" md="6">
                  <form-input-hrm
                     :value="PrtnCreditDemand.newVacanciesCount"
                     disabled
                     :label="$t('newVacanciesCount')"
                  />
               </b-col>
               <b-col sm="12" md="6">
                  <form-input-hrm
                     :value="PrtnCreditDemand.implementedProjectName"
                     disabled
                     :label="$t('implementedProjectName')"
                  />
               </b-col>
               <b-col sm="12" md="6">
                  <form-input-hrm :value="PrtnCreditDemand.ownInvestment" disabled :label="$t('ownInvestment')" />
               </b-col>
               <b-col sm="12" md="6">
                  <form-input-hrm
                     :value="PrtnCreditDemand.foreignInvestment"
                     disabled
                     :label="$t('foreignInvestment')"
                  />
               </b-col>
               <b-col sm="12" md="6">
                  <form-input-hrm :value="PrtnCreditDemand.projectCost" disabled :label="$t('projectCost')" />
               </b-col>
               <b-col sm="12" md="6">
                  <form-input-hrm
                     :value="PrtnCreditDemand.privillageBankCredit"
                     disabled
                     :label="$t('privillageBankCredit')"
                  />
               </b-col>
               <b-col sm="12" md="6">
                  <form-input-hrm :value="PrtnCreditDemand.bank" disabled :label="$t('bankName')" />
               </b-col>
            </b-row>
         </b-card>
      </b-container>
   </b-overlay>
</template>

<script>
import { BOverlay, BCard, BRow, BCol, BContainer } from 'bootstrap-vue';
import PrtnCreditDemandService from '@/services/document/prtncreditdemand.service';

export default {
   components: {
      BOverlay,
      BCard,
      BRow,
      BCol,
      BContainer
   },
   data() {
      return {
         show: false,
         PrtnCreditDemand: {}
      };
   },
   created() {
      this.GetPrtnCreditDemand();
   },
   methods: {
      GetPrtnCreditDemand() {
         this.show = true;
         PrtnCreditDemandService.Get(this.$route.params.id)
            .then((res) => {
               this.PrtnCreditDemand = res.data;
            })
            .catch((error) => {
               this.showApiError(error);
            })
            .finally(() => {
               this.show = false;
            });
      }
   }
};
</script>
