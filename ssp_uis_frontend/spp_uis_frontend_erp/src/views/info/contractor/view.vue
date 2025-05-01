<template>
   <b-card>
      <b-row>
         <b-col sm="12" md="3">
            <form-input-hrm
               v-model="Data.shortName"
               rules="required"
               :label="$t('shortname')"
               :placeholder="$t('shortname')"
               :disabled="disabled"
            />
         </b-col>
         <b-col sm="12" md="3">
            <form-input-hrm
               v-model="Data.fullName"
               rules="required"
               :label="$t('fullname')"
               :placeholder="$t('fullname')"
               :disabled="disabled"
            />
         </b-col>

         <b-col sm="12" md="3">
            <div class="form-group">
               <form-input-hrm v-model="Data.registrationDate" :disabled="disabled" :label="$t('registrationDate')" />
            </div>
         </b-col>

         <b-col sm="12" md="3">
            <div class="form-group">
               <form-input-hrm v-model="Data.country" :disabled="disabled" :label="$t('Country')" />
            </div>
         </b-col>

         <b-col sm="12" md="3">
            <div class="form-group">
               <form-input-hrm v-model="Data.region" :label="$t('region')" />
            </div>
         </b-col>

         <b-col sm="12" md="3">
            <div class="form-group">
               <form-input-hrm v-model="Data.district" :label="$t('Region')" />
            </div>
         </b-col>

         <b-col sm="12" md="3">
            <div class="form-group">
               <form-input-hrm v-model="Data.address" :label="$t('address')" />
            </div>
         </b-col>

         <b-col sm="12" md="3">
            <div class="form-group">
               <form-input-hrm v-model="Data.director" :label="$t('director')" />
            </div>
         </b-col>

         <b-col sm="12" md="3">
            <div class="form-group">
               <form-input-hrm v-model="Data.innOrPinfl" :label="$t('innOrPinfl')" />
            </div>
         </b-col>

         <b-col sm="12" md="3">
            <div class="form-group">
               <form-input-hrm v-model="Data.oked" :label="$t('Oked')" />
            </div>
         </b-col>
      </b-row>
      <b-row>
         <b-col cols="12">
            <table class="priceTable mt-2 w-100">
               <thead>
                  <tr>
                     <th class="text-center">{{ $t('mainquota') }}</th>
                     <th>{{ $t('bankname') }}</th>
                     <th>{{ $t('bankcode') }}</th>
                     <th>{{ $t('accountCode') }}</th>
                     <th>{{ $t('accountName') }}</th>
                  </tr>
               </thead>
               <tbody>
                  <tr v-for="(tab, j) in Data.settlementAccounts" :key="j + 'tab'">
                     <td class="text-center w-70px">
                        <feather-icon icon="CheckSquareIcon" v-if="tab.isMain" size="16" />
                        <feather-icon icon="SquareIcon" v-else size="16" />
                     </td>
                     <td>{{ tab.bank }}</td>
                     <td>{{ tab.bankCode }}</td>
                     <td>{{ tab.accountCode }}</td>
                     <td>{{ tab.accountName }}</td>
                  </tr>
               </tbody>
            </table>
         </b-col>
      </b-row>
   </b-card>
</template>
<script>
import ContractorService from '@/services/info/contractor.service';

import {
   BOverlay,
   BCard,
   BCardBody,
   BRow,
   BCol,
   BFormInput,
   BButton,
   BTable,
   BLink,
   BFormGroup,
   BModal,
   BCardText,
   BInputGroup,
   BInputGroupAppend,
   BFormCheckbox
} from 'bootstrap-vue';

export default {
   components: {
      BOverlay,
      BCard,
      BCardBody,
      BRow,
      BCol,
      BFormInput,
      BButton,
      BTable,
      BLink,
      BFormGroup,
      BModal,
      BCardText,
      BInputGroup,
      BInputGroupAppend,
      BFormCheckbox
   },
   data() {
      return {
         loadingButton: false,
         Data: {
            translates: [],
            person: {}
         },
         disabled: true
      };
   },
   created() {
      ContractorService.Get(this.$route.params.id).then((res) => {
         this.Data = res.data;
      });
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
}
</style>
