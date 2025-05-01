<template>
   <b-overlay :show="show">
      <b-card>
         <validation-observer ref="ValidationDTO">
            <b-row>
               <b-col sm="12" md="3">
                  <form-input-hrm v-model="Data.orderCode" :label="$t('orderCode')" :placeholder="$t('orderCode')" />
               </b-col>
               <b-col sm="12" md="3">
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
               <b-col sm="12" md="3">
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
               <b-col sm="12" md="3">
                  <form-input-hrm v-model="Data.pnRu" rules="required" :label="$t('pnRu')" :placeholder="$t('pnRu')" />
               </b-col>
               <b-col sm="12" md="3">
                  <form-input-hrm
                     v-model="Data.numRu"
                     rules="required"
                     :label="$t('numRu')"
                     :placeholder="$t('numRu')"
                  />
               </b-col>
               <b-col sm="12" md="3">
                  <form-input-hrm
                     v-model="Data.classRu"
                     rules="required"
                     :label="$t('classRu')"
                     :placeholder="$t('classRu')"
                  />
               </b-col>
               <b-col sm="12" md="3">
                  <form-input-hrm
                     v-model="Data.nskzCodeRu"
                     rules="required"
                     :label="$t('nskzCodeRu')"
                     :placeholder="$t('nskzCodeRu')"
                  />
               </b-col>
               <b-col sm="12" md="3">
                  <form-input-hrm
                     v-model="Data.categoryCodeRu"
                     rules="required"
                     :label="$t('categoryCodeRu')"
                     :placeholder="$t('categoryCodeRu')"
                  />
               </b-col>
               <b-col sm="12" md="3">
                  <form-input-hrm
                     v-model="Data.rangeCodeRu"
                     rules="required"
                     :label="$t('rangeCodeRu')"
                     :placeholder="$t('rangeCodeRu')"
                  />
               </b-col>
               <b-col sm="12" md="3">
                  <form-input-hrm
                     v-model="Data.minedCodeRu"
                     rules="required"
                     :label="$t('minedCodeRu')"
                     :placeholder="$t('minedCodeRu')"
                  />
               </b-col>
               <b-col sm="12" md="3">
                  <form-input-hrm
                     v-model="Data.specialityCodeRu"
                     rules="required"
                     :label="$t('specialityCodeRu')"
                     :placeholder="$t('specialityCodeRu')"
                  />
               </b-col>
               <b-col sm="12" md="3">
                  <form-input-hrm
                     v-model="Data.typeCodeRu"
                     rules="required"
                     :label="$t('typeCodeRu')"
                     :placeholder="$t('typeCodeRu')"
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
import PositionClassificationService from '@/services/hrm/positionclassification.service';
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
   data() {
      return {
         show: false,
         saveLoading: false,
         Data: {
            shortName: '',
            fullName: '',
            orderCode: '',
            pnRu: '',
            numRu: '',
            classRu: '',
            nskzCodeRu: '',
            categoryCodeRu: '',
            rangeCodeRu: '',
            minedCodeRu: '',
            specialityCodeRu: '',
            typeCodeRu: '',
            translates: []
         }
      };
   },
   created() {
      this.show = true;
      PositionClassificationService.Get(this.$route.params.id)
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
               PositionClassificationService.Update(this.Data)
                  .then(() => {
                     this.makeToast(this.$t('SaveSuccess'), 'success');
                     this.$router.push({ name: 'PositionClassification' });
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
