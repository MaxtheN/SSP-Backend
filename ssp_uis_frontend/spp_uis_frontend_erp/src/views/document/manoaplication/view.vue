<template>
   <b-overlay :show="false">
      <b-card>
         <b-row v-if="Data">
            <b-col>
               <h3 class="ml-1 my-1">{{ $t("Tashkilot ma'lumotlari") }}</h3>
               <b-col>
                  <form-input-hrm :label="$t('docdate')" disabled v-model="Data.application.docOn" />
               </b-col>
               <b-col>
                  <form-input-hrm :label="$t('docnumber')" disabled v-model="Data.application.docNumber" />
               </b-col>
               <b-col>
                  <form-input-hrm :label="$t('contractor')" disabled v-model="Data.application.contractor" />
               </b-col>

               <b-col>
                  <form-input-hrm :label="$t('director')" disabled v-model="Data.application.contractorDirector" />
               </b-col>
               <b-col>
                  <form-input-hrm :label="$t('inn')" disabled v-model="Data.application.contractorInn" />
               </b-col>
               <b-col>
                  <form-input-hrm :label="$t('region')" disabled v-model="Data.application.region" />
               </b-col>
               <b-col>
                  <form-input-hrm :label="$t('district')" disabled v-model="Data.application.district" />
               </b-col>
               <b-col>
                  <form-input-hrm :label="$t('address')" disabled v-model="Data.application.contractorAddress" />
               </b-col>
            </b-col>
            <b-col>
               <h3 class="ml-1 my-1">{{ $t("Monomarkaz ma'lumotlari") }}</h3>
               <b-col>
                  <form-input-hrm :label="$t('region')" disabled v-model="Data.monoRegion" />
               </b-col>
               <b-col>
                  <form-input-hrm :label="$t('district')" disabled v-model="Data.monoDistrict" />
               </b-col>
               <b-col>
                  <form-input-hrm :label="$t('mfy')" disabled v-model="Data.monoMfy" />
               </b-col>
               <b-col>
                  <form-input-hrm :label="$t('address')" disabled v-model="Data.monoAdress" />
               </b-col>
            </b-col>
         </b-row>
      </b-card>
   </b-overlay>
</template>

<script>
import { BOverlay, BCard, BRow, BCol, BButton, BLink, BIcon, BBadge, BContainer, BAlert } from 'bootstrap-vue';
import axios from 'axios';
import MonoAplicationService from '@/services/monoaplication/monoaplication.service';

export default {
   components: {
      BAlert,
      BOverlay,
      BCard,
      BRow,
      BCol,
      BButton,
      BLink,
      BIcon,
      BBadge,
      BContainer
   },
   data() {
      return {
         Data: {}
      };
   },

   created() {
      this.GetApplication();
   },
   methods: {
      GetApplication() {
         this.show = true;
         MonoAplicationService.Get(this.$route.params.id)
            .then((res) => {
               this.Data = res.data;
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
