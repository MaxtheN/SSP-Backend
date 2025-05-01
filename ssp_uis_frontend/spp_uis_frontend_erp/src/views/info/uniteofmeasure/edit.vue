<template>
   <b-overlay :show="show">
      <b-card>
         <validation-observer ref="ValidationDTO">
            <b-row>
               <b-col sm="12" md="4">
                  <form-input v-model="Data.orderCode" required :label="$t('orderCode')" />
               </b-col>
               <b-col sm="12" md="4">
                  <form-input v-model="Data.code" required :label="$t('kode')" />
               </b-col>
               <b-col sm="12" md="4">
                  <form-input-translate
                     v-model="Data.shortName"
                     @update:translates="(e) => (Data.translates = e)"
                     :translates="Data.translates"
                     column-name="short_name"
                     required
                     :label="$t('shortname')"
                     :placeholder="$t('shortname')"
                  />
               </b-col>
               <b-col sm="12" md="4">
                  <form-input-translate
                     v-model="Data.fullName"
                     @update:translates="(e) => (Data.translates = e)"
                     :translates="Data.translates"
                     column-name="full_name"
                     required
                     :label="$t('fullname')"
                     :placeholder="$t('fullname')"
                  />
               </b-col>
               <b-col sm="12" md="4">
                  <form-select :options="StateList" v-model="Data.stateId" label="Status"></form-select>
               </b-col>
            </b-row>
            <b-row class="mt-3">
               <b-col sm="12" md="6" lg="6" class="text-left"></b-col>
               <b-col sm="12" md="6" lg="6" class="text-right">
                  <b-button :disabled="saveLoading" @click="SaveData" size="sm" variant="outline-success">
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
import ManualService from '@/services/others/manual.service';
import UniteOfMeasureService from '@/services/info/uniteofmeasure.service';

import { BOverlay, BCard, BRow, BCol, BButton, BModal, BTable, BLink } from 'bootstrap-vue';
import FormInputTranslate from '@/components/translates/FormInputTranslate.vue';

export default {
   components: {
      BOverlay,
      BTable,
      BCard,
      BModal,
      BRow,
      BCol,
      BButton,
      BLink,
      FormInputTranslate
   },
   data() {
      return {
         showModal: true,

         show: false,
         saveLoading: false,
         StateList: [],
         tabrow: {},
         Tabindex: null,
         TableFeilds: [
            {
               key: 'code',
               label: this.$t('code'),
               thClass: 'text-center'
            },
            {
               key: 'shortName',
               label: this.$t('shortname'),
               thClass: 'text-center'
            },
            {
               key: 'fullName',
               label: this.$t('fullname'),
               thClass: 'text-center'
            },
            {
               key: 'actions',
               label: this.$t('actions'),
               thClass: 'text-center'
            }
         ],
         Data: {
            code: '',
            orderCode: '',
            shortName: '',
            fullName: '',
            translates: [],
            stateId: null
         }
      };
   },
   created() {
      ManualService.StateSelectList()
         .then((res) => {
            this.StateList = res.data;
         })
         .catch((error) => {
            this.showApiError(error);
         });
      this.show = true;
      UniteOfMeasureService.Get(this.$route.params.id)
         .then((res) => {
            this.Data = res.data;
         })
         .catch((error) => {
            this.showApiError(error);
         })
         .finally(() => {
            this.show = false;
         });
   },
   methods: {
      SaveData() {
         this.$refs.ValidationDTO.validate().then((success) => {
            if (success) {
               this.saveLoading = true;
               UniteOfMeasureService.Update(this.Data)
                  .then(() => {
                     this.makeToast(this.$t('SaveSuccess'), 'success');
                     this.$router.push({ name: 'UniteOfMeasure' });
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

<style>
.col-form-label,
label {
   line-height: initial;
}
</style>
