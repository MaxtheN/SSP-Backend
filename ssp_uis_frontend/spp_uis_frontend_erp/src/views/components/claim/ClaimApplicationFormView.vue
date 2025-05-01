<template>
   <div v-if="application && application.application">
      <b-row>
         <b-col sm="12" md="6" lg="6">
            <form-input-hrm :label="$t('docnumber')" disabled v-model="application.application.docNumber" />
         </b-col>
         <b-col sm="12" md="6" lg="6">
            <form-input-hrm :label="$t('docdate')" disabled v-model="application.application.docOn" />
         </b-col>
         <b-col sm="12" md="12" lg="12">
            <form-input-hrm :label="$t('contractor')" disabled v-model="application.application.contractor" />
         </b-col>
         <b-col sm="12" md="6" lg="6">
            <form-input-hrm :label="$t('contractorInn')" disabled v-model="application.application.contractorInn" />
         </b-col>
         <b-col sm="12" md="6" lg="6">
            <form-input-hrm :label="$t('director')" disabled v-model="application.application.contractorDirector" />
         </b-col>
         <b-col sm="12" md="6" lg="6">
            <form-input-hrm :label="$t('region')" disabled v-model="application.application.region" />
         </b-col>
         <b-col sm="12" md="6" lg="6">
            <form-input-hrm :label="$t('district')" disabled v-model="application.application.district" />
         </b-col>
         <b-col sm="12" md="6" lg="6">
            <form-input-hrm :label="$t('ClaimTheme')" disabled v-model="application.claimTheme" />
         </b-col>

         <hr class="w-100 mx-1" />
         <b-col cols="12">
            <h4>
               {{ $t('claimAmount') }}
            </h4>
         </b-col>
         <b-col sm="12" md="6" lg="6">
            <form-currency-input :label="$t('totalAmount')" disabled v-model="application.totalAmount" />
         </b-col>
         <b-col sm="12" md="6" lg="6">
            <form-input-hrm :label="$t('currency')" disabled v-model="application.currency" />
         </b-col>
         <b-col sm="12" md="6" lg="6">
            <form-currency-input :label="$t('mainDebt')" disabled v-model="application.mainDebt" />
         </b-col>
         <b-col sm="12" md="6" lg="6">
            <form-currency-input :label="$t('calculedPenalty')" disabled v-model="application.calculedPenalty" />
         </b-col>
         <b-col sm="12" md="6" lg="6">
            <form-currency-input :label="$t('penalty')" disabled v-model="application.penalty" />
         </b-col>
         <b-col sm="12" md="6" lg="6">
            <form-currency-input
               :label="$t('currentInterestRate')"
               disabled
               v-model="application.currentInterestRate"
            />
         </b-col>
         <b-col sm="12" md="6" lg="6">
            <form-currency-input
               :label="$t('currentPrincipalInterest')"
               disabled
               v-model="application.currentPrincipalInterest"
            />
         </b-col>
         <b-col sm="12" md="6" lg="6">
            <form-currency-input :label="$t('percentAmount')" disabled v-model="application.percent" />
         </b-col>
         <b-col sm="12" md="6" lg="6">
            <form-currency-input :label="$t('otherDebtRepayment')" disabled v-model="application.otherDebtRepayment" />
         </b-col>

         <!-- javobgarlar -->
         <hr class="w-100 mx-1" />
         <b-col cols="12">
            <h4>
               {{ $t('Javobgarlar') }}
            </h4>
         </b-col>
         <b-col cols="12">
            <table class="priceTable w-100">
               <thead>
                  <tr>
                     <th>{{ $t('fullname') }}</th>
                     <th>{{ $t('innOrPinfl') }}</th>
                     <th>{{ $t('phoneNumber') }}</th>
                     <th>{{ $t('address') }}</th>
                  </tr>
               </thead>
               <tbody>
                  <tr v-for="(tab, j) in application.tables" :key="j + 'tab'">
                     <td>{{ tab.fullName }}</td>
                     <td>{{ tab.innOrPinfl }}</td>
                     <td>{{ tab.phoneNumber }}</td>
                     <td>{{ tab.address }}</td>
                  </tr>
               </tbody>
            </table>
         </b-col>

         <hr class="w-100 mx-1" />
         <b-col cols="12">
            <h4>
               {{ $t('MemshipContract') }}
            </h4>
         </b-col>
         <b-col sm="12" md="6" lg="6">
            <form-input-hrm :label="$t('docnumber')" disabled v-model="application.memshipContractDocNumber" />
         </b-col>
         <b-col sm="12" md="6" lg="6">
            <form-input-hrm :label="$t('docdate')" disabled v-model="application.memshipContractDocOn" />
         </b-col>
      </b-row>
   </div>
</template>

<script>
import { BRow, BCol } from 'bootstrap-vue';
export default {
   props: {
      application: {
         type: Object,
         default: () => ({})
      }
   },
   components: {
      BRow,
      BCol
   }
};
</script>

<style lang="scss">
.priceTable {
   thead {
      tr {
         background-color: #f0f0f0;
      }
   }

   tr th,
   tr td {
      padding: 7px;
      border-collapse: collapse;
      border: 1px solid #f5f5f5;
   }

   tr:nth-child(even) {
      background-color: #f7f7f7;
   }

   .w-70px {
      width: 70px;
   }
}
</style>
