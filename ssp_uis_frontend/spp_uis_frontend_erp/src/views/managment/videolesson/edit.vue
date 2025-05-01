<template>
   <b-overlay :show="show">
      <b-card>
         <validation-observer ref="ValidationDTO">
            <b-row>
               <b-col sm="12" md="3" class="mb-1">
                  <form-input-hrm
                     v-model="Data.number"
                     :label="$t('number')"
                     :placeholder="$t('number')"
                     rules="required"
                  />
               </b-col>
               <b-col sm="12" md="3" class="mb-1">
                  <form-input-hrm v-model="Data.orderCode" :label="$t('orderCode')" :placeholder="$t('orderCode')" />
               </b-col>
               <b-col sm="12" md="3" class="mb-1">
                  <form-input-hrm v-model="Data.theme" :label="$t('theme')" :placeholder="$t('theme')" />
               </b-col>
               <b-col sm="12" md="3" class="mb-1">
                  <form-input-hrm v-model="Data.tag" :label="$t('tag')" :placeholder="$t('tag')" />
               </b-col>
               <b-col sm="12" md="3" class="mb-1">
                  <form-input-hrm v-model="Data.uri" :label="$t('uri')" :placeholder="$t('uri')" />
               </b-col>
               <b-col sm="12" md="3" class="mb-1">
                  <form-select
                     v-model="Data.categoryId"
                     :options="categoryList"
                     :label="$t('VideoCategory')"
                     :placeholder="$t('VideoCategory')"
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
import VideoLessonService from '@/services/managment/videolesson.service';
// components
import { BOverlay, BCard, BRow, BCol, BButton } from 'bootstrap-vue';
import FormInputTranslate from '@/components/translates/FormInputTranslate.vue';
import VideoCategoryService from '@/services/managment/videocategory.service';

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
         categoryList: [],
         Data: {
            orderCode: '',
            number: '',
            categoryId: null,
            theme: '',
            tag: '',
            uri: ''
         }
      };
   },
   created() {
      this.show = true;
      VideoLessonService.Get(this.$route.params.id)
         .then((res) => {
            this.Data = res.data;
         })
         .catch((error) => {
            this.showApiError(error);
         })
         .finally(() => {
            this.show = false;
         });

      VideoCategoryService.GetAsSelectList().then((res) => {
         this.categoryList = res.data;
      });
   },
   methods: {
      SaveData() {
         this.$refs.ValidationDTO.validate().then((success) => {
            if (success) {
               this.saveLoading = true;
               VideoLessonService.Update(this.Data)
                  .then(() => {
                     this.makeToast(this.$t('SaveSuccess'), 'success');
                     this.$router.push({ name: 'VideoLesson' });
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
