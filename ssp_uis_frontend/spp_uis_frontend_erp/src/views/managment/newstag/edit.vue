<template>
   <b-overlay :show="show">
      <b-card>
         <validation-observer ref="ValidationDTO">
            <b-row>
               <b-col sm="12" md="3" class="mb-1">
                  <form-input-hrm
                     v-model="Data.news"
                     :label="$t('news')"
                     :placeholder="$t('News')"
                     rules="required"
                  />
               </b-col>
               <b-col sm="12" md="3" class="mb-1">
                  <form-picker v-model="Data.docDate" :label="$t('ondate')" :placeholder="$t('ondate')" />
               </b-col>
               <b-col sm="12" md="4" class="mb-1">
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
import NewsTagService from '@/services/managment/newstag.service';
// components
import { BOverlay, BCard, BRow, BCol, BButton } from 'bootstrap-vue';
import FormInputTranslate from '@/components/translates/FormInputTranslate.vue';
import ManualService from '@/services/others/manual.service';

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
         StateList: [],
         Data: {
            news: '',
            docDate: '',
            statusId: 0
         }
      };
   },
   created() {
      this.show = true;
      NewsTagService.Get(this.$route.params.id)
         .then((res) => {
            this.Data = res.data;
         })
         .catch((error) => {
            this.showApiError(error);
         })
         .finally(() => {
            this.show = false;
         });

      ManualService.StateSelectList().then((res) => {
         this.StateList = res.data;
      });
   },
   methods: {
      SaveData() {
         this.$refs.ValidationDTO.validate().then((success) => {
            if (success) {
               this.saveLoading = true;
               NewsTagService.Update(this.Data)
                  .then(() => {
                     this.makeToast(this.$t('SaveSuccess'), 'success');
                     this.$router.push({ name: 'NewsTag' });
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
