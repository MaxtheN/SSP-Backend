<template>
   <b-overlay :show="show">
      <b-card>
         <validation-observer ref="ValidationDTO">
            <b-row>
               <b-col sm="12" md="4" class="mb-1">
                  <form-input-translate
                     v-model="Data.shortName"
                     @update:translates="(e) => (Data.translates = e)"
                     :translates="Data.translates"
                     columnName="short_name"
                     required
                     :label="$t('shortname')"
                     :placeholder="$t('shortname')"
                  />
               </b-col>
               <b-col sm="12" md="4" class="mb-1">
                  <form-input-translate
                     v-model="Data.fullName"
                     @update:translates="(e) => (Data.translates = e)"
                     :translates="Data.translates"
                     columnName="full_name"
                     required
                     :label="$t('fullname')"
                     :placeholder="$t('fullname')"
                  />
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
import PositionCategoryService from '@/services/hrm/positioncategory.service';
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
         saveLoading:false,
         Data: {
            shortName: '',
            fullName: '',
            translates: []
         }
      };
   },
   created() {
      this.show = true;
      PositionCategoryService.Get(this.$route.params.id)
         .then((res) => {
            this.Data = res.data;
         })
         .catch((error) => {
            this.makeToast(error.response.data.errors, 'danger');
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
               PositionCategoryService.Update(this.Data)
                  .then(() => {
                     this.makeToast(this.$t('SaveSuccess'), 'success');
                     this.$router.push({ name: 'PositionCategory' });
                  })
                  .catch((err) => {
                     this.showApiError(err)
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
