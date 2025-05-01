<template>
   <b-overlay :show="show">
      <b-card>
         <validation-observer ref="ValidationDTO">
            <b-row>
               <b-col sm="12" md="4">
                  <form-input-hrm v-model="Data.code" rules="required" :label="$t('code')" :placeholder="$t('code')" />
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
                  <b-form-checkbox class="mt-2" switch v-model="Data.hasParent" label="hasParent">{{
                     $t('hasParent')
                  }}</b-form-checkbox>
               </b-col>

               <b-col sm="12" md="4">
                  <form-select :options="AppealDescriptionList" v-model="Data.parentId" label="parent1"></form-select>
               </b-col>

               <b-col sm="12" md="4">
                  <form-select :options="StateList" v-model="Data.stateId" label="Status"></form-select>
               </b-col>

               <!-- save button -->
               <b-col cols="12" class="text-right">
                  <b-button :disabled="saveLoading" @click="SaveData" variant="outline-success">
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
import ManualService from '@/services/others/manual.service';
import AppealDescriptionService from '@/services/appeal/AppealDescription.service';
// components
import { BOverlay, BCard, BRow, BCol, BButton, BFormCheckbox } from 'bootstrap-vue';
import FormInputTranslate from '@/components/translates/FormInputTranslate.vue';

export default {
   components: {
      BOverlay,
      BCard,
      BRow,
      BCol,
      BButton,
      FormInputTranslate,
      BFormCheckbox
   },
   data() {
      return {
         show: false,
         saveLoading: false,
         StateList: [],
         AppealDescriptionList: [],
         Data: {
            shortName: '',
            fullName: '',
            code: '',
            hasParent: false,
            parentId: 0,
            translates: []
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

      AppealDescriptionService.GetAsSelectList(true).then((res) => {
         this.AppealDescriptionList = res.data;
      });
      this.show = true;
      AppealDescriptionService.Get(this.$route.params.id)
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
               AppealDescriptionService.Update(this.Data)
                  .then(() => {
                     this.makeToast(this.$t('SaveSuccess'), 'success');
                     this.$router.push({ name: 'AppealDescription' });
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
