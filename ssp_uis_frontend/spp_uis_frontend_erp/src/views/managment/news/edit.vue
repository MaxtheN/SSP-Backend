<template>
   <b-overlay :show="show">
      <b-card>
         <validation-observer ref="ValidationDTO">
            <b-row>
               <b-col sm="12" md="4" class="mb-1">
                  <form-input-translate
                     v-model="Data.title"
                     @update:translates="(e) => (Data.translates = e)"
                     :translates="Data.translates"
                     column-name="title"
                     required
                     :label="$t('title')"
                     :placeholder="$t('title')"
                  />
               </b-col>
               <b-col sm="12" md="4" class="mb-1">
                  <form-picker v-model="Data.date" :label="$t('ondate')" :placeholder="$t('ondate')" />
               </b-col>
               <b-col sm="12" md="4" class="mb-1">
                  <form-input-hrm
                     v-model="Data.shortContent"
                     :label="$t('shortContent')"
                     :placeholder="$t('shortContent')"
                  />
               </b-col>
               <b-col class="mb-1">
                  <vue-editor v-model="Data.content" :label="$t('content')"></vue-editor>
               </b-col>

               <b-col cols="12">
                  <h6 class="inputTitle">{{ $t('image') }}</h6>
                  <b-form-file
                     accept="image/*"
                     type="file"
                     :placeholder="$t('Faylni tanlang')"
                     @change="UploadFile"
                  ></b-form-file>
               </b-col>
               <b-col v-if="Data.image && Data.image.id" cols="12" class="mt-1">
                  <b-img :src="imageSrc" width="150" height="150" />
                  {{ Data.image.fileName }}
               </b-col>

               <!-- save button -->
               <b-col cols="12" class="text-right mt-2">
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
import NewsService from '@/services/managment/news.service';
// components
import { BOverlay, BCard, BRow, BCol, BButton, BFormFile, BImg } from 'bootstrap-vue';
import FormInputTranslate from '@/components/translates/FormInputTranslate.vue';
import axios from 'axios';
import { VueEditor } from 'vue2-editor';

export default {
   components: {
      BOverlay,
      BCard,
      BRow,
      BCol,
      BButton,
      BFormFile,
      BImg,
      FormInputTranslate,
      VueEditor
   },
   data() {
      return {
         show: false,
         saveLoading: false,
         fileLoading: false,
         axios,
         imageSrc: '',
         Data: {
            title: '',
            content: '',
            shortContent: '',
            date: '',
            translates: [],
            image: {}
         }
      };
   },
   created() {
      this.show = true;
      NewsService.Get(this.$route.params.id)
         .then((res) => {
            this.Data = res.data;
            if (this.Data.image && this.Data.image.id) {
               this.GetImage(this.Data.image.id);
            }
         })
         .catch((error) => {
            this.showApiError(error);
         })
         .finally(() => {
            this.show = false;
         });
   },
   methods: {
      UploadFile(event) {
         const formData = new FormData();
         formData.append('file', event.target.files[0]);
         this.fileLoading = true;
         NewsService.UploadNewsImage(formData)
            .then((res) => {
               this.Data.image = {
                  ...res.data,
                  id: res.data.fileId
               };
               this.GetImage(this.Data.image.id);
            })
            .catch((error) => {
               this.showApiError(error);
            })
            .finally(() => {
               this.fileLoading = false;
            });
      },
      GetImage(id) {
         NewsService.GetNewsImage(id).then((res) => {
            this.imageSrc = res.data;
         });
      },
      SaveData() {
         this.$refs.ValidationDTO.validate().then((success) => {
            if (success) {
               this.saveLoading = true;
               NewsService.Update(this.Data)
                  .then(() => {
                     this.makeToast(this.$t('SaveSuccess'), 'success');
                     this.$router.push({ name: 'News' });
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
