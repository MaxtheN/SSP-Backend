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
            <form-input-hrm :label="$t('director')" disabled v-model="application.contractorDirector" />
         </b-col>
         <b-col sm="12" md="6" lg="6">
            <form-input-hrm
               :label="$t('region')"
               disabled
               :value="application.chooseLocation ? application.choosedRegion : application.application.region"
            />
         </b-col>
         <b-col sm="12" md="6" lg="6">
            <form-input-hrm
               :label="$t('district')"
               disabled
               :value="application.chooseLocation ? application.choosedDistrict : application.application.district"
            />
         </b-col>
         <b-col sm="12" md="12" lg="12">
            <form-input-hrm
               :label="$t('contractorActivityType')"
               disabled
               v-model="application.contractorActivityType"
            />
         </b-col>
         <b-col sm="12" md="6" lg="6">
            <form-input-hrm :label="$t('contractorCategory')" disabled v-model="application.contractorCategory" />
         </b-col>
         <b-col sm="12" md="6" lg="6">
            <form-input-hrm :label="$t('phone')" disabled v-model="application.contractorWorkPhoneNumber" />
         </b-col>
         <b-col sm="12" md="6" lg="6">
            <form-input-hrm :label="$t('oked')" disabled v-model="application.oked" />
         </b-col>
         <!-- summmma -->
         <b-col sm="12" md="6" lg="6">
            <form-currency-input :label="$t('yearlyEarnings')" disabled v-model="application.yearlyEarnings" />
         </b-col>
         <b-col sm="12" md="6" lg="6">
            <form-currency-input :label="$t('nowYearlyEarnings')" disabled v-model="application.nowYearlyEarnings" />
         </b-col>
         <b-col sm="12" md="6" lg="6">
            <form-currency-input :label="$t('yearlyExport')" disabled v-model="application.yearlyExport" />
         </b-col>
         <b-col sm="12" md="6" lg="6">
            <form-currency-input :label="$t('yearlyImport')" disabled v-model="application.yearlyImport" />
         </b-col>
         <b-col sm="12" md="6" lg="6">
            <form-currency-input :label="$t('yearlyManufacture')" disabled v-model="application.yearlyManufacture" />
         </b-col>
         <b-col sm="12" md="6" lg="6">
            <form-currency-input :label="$t('yearlyTaxes')" disabled v-model="application.yearlyTaxes" />
         </b-col>
      </b-row>
      <div class="mt-1" v-for="item in application.files" :key="item.id">
         <b-link variant="primary" target="_blank" :href="FileSrc(item.id)">{{ item.fileName || item.id }}</b-link>
      </div>
   </div>
</template>

<script>
import { BRow, BCol, BLink } from 'bootstrap-vue';
import axios from 'axios';

export default {
   props: {
      application: {
         type: Object,
         default: () => ({})
      }
   },
   components: {
      BRow,
      BCol,
      BLink
   },
   computed: {
      FileSrc() {
         return (id) => axios.defaults.baseURL + `Memship/MemshipApplication/DownloadFile/${id}`;
      }
   }
};
</script>
