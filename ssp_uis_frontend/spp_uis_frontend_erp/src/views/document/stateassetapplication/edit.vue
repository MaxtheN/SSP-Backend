<template>
   <b-overlay :show="show">
      <b-card>
         <b-card-text>
            <b-row>
               <b-col sm="12" md="6" lg="6">
                  <form-input-hrm :label="$t('contractorInn')" disabled v-model="Data.contractorInn" />
               </b-col>
               <b-col sm="12" md="6" lg="6">
                  <form-input-hrm :label="$t('contractor')" disabled v-model="Data.contractor" />
               </b-col>
               <b-col sm="12" md="6" lg="6">
                  <form-input-hrm :label="$t('docnumber')" disabled v-model="Data.docNumber" />
               </b-col>
               <b-col sm="12" md="6" lg="6">
                  <form-input-hrm :label="$t('docdate')" disabled v-model="Data.docOn" />
               </b-col>
               <b-col sm="12" md="6" lg="6">
                  <form-input-hrm :label="$t('applicationType')" disabled v-model="Data.applicationType" />
               </b-col>
               <b-col sm="12" md="6" lg="6">
                  <form-input-hrm
                     :label="$t('prtnCertificateDocNumber')"
                     disabled
                     v-model="Data.prtnCertificateDocNumber"
                  />
               </b-col>
               <b-col sm="12" md="6" lg="6">
                  <form-input-hrm :label="$t('prtnCertificateDocOn')" disabled v-model="Data.prtnCertificateDocOn" />
               </b-col>
               <b-col sm="12" md="6" lg="6">
                  <form-input-hrm :label="$t('prtnContractType')" disabled v-model="Data.prtnContractType" />
               </b-col>
               <b-col sm="12" md="4" lg="4">
                  <form-input-hrm disabled :label="$t('region')" v-model="Data.region" />
               </b-col>
               <b-col sm="12" md="4" lg="4">
                  <form-input-hrm disabled :label="$t('district')" v-model="Data.district" />
               </b-col>

               <b-col sm="12" md="4" lg="4">
                  <form-input-hrm :label="$t('director')" disabled v-model="Data.contractorDirector" />
               </b-col>

               <b-col sm="12" md="4" lg="4">
                  <form-input-hrm :label="$t('directorAddress')" disabled v-model="Data.contractorAddress" />
               </b-col>

               <b-col sm="12" md="4" lg="4">
                  <form-input-hrm :label="$t('phoneNumber')" disabled v-model="Data.phoneNumber" />
               </b-col>
            </b-row>
         </b-card-text>
      </b-card>
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
import StateAssetApplicationService from '@/services/document/stateassetapplication.service';

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
      BBadge
   },
   directives: {
      'b-tooltip': VBTooltip,
      'b-modal': VBModal
   },
   data() {
      return {
         show: false,
         Data: {}
      };
   },
   created() {
      this.show = true;
      StateAssetApplicationService.Get(this.$route.params.id)
         .then((res) => {
            this.Data = res.data;
         })
         .finally(() => {
            this.show = false;
         });
   }
};
</script>
