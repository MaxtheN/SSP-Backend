<template>
   <b-overlay :show="show">
      <b-card>
         <validation-observer ref="ValidationDTO">
            <b-row>
               <b-col sm="12" md="3" class="mb-1">
                  <form-input-hrm 
                     v-model="Data.code"
                     :label="$t('kode')"
                     :placeholder="$t('kode')"
                     rules="required"
                  />   
               </b-col>
               <b-col sm="12" md="3" class="mb-1">
                  <form-input-hrm 
                     v-model="Data.orderCode"
                     :label="$t('orderCode')"
                     :placeholder="$t('orderCode')"
                  />   
               </b-col>
               <b-col sm="12" md="3" class="mb-1">
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
               <b-col sm="12" md="3" class="mb-1">
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
               <!-- save button -->
               <b-col cols="12" class="text-right">
                  <b-button
                     :disabled="saveLoading"
                     @click="SaveData"
                     variant="outline-success"
                  >
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
// service
import VideoCategoryService from '@/services/managment/videocategory.service';
// components
import { BOverlay, BCard, BRow, BCol, BButton } from 'bootstrap-vue';
import FormInputTranslate from '@/components/translates/FormInputTranslate.vue';

export default {
   components: {
      BOverlay,
      BCard,
      BRow,
      BCol,
      BButton,
      FormInputTranslate
   },
   name: 'Edit',
   data() {
      return {
         show: false,
         saveLoading: false,
         Data: {
            code: '',
            orderCode: '',
            shortName: '',
            fullName: '',
            translates: []
         }
      };
   },
   created() {
      this.show = true;
      VideoCategoryService.Get(this.$route.params.id)
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
               VideoCategoryService.Update(this.Data)
                  .then(() => {
                     this.makeToast(this.$t('SaveSuccess'), 'success');
                     this.$router.push({ name: 'VideoCategory' });
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
