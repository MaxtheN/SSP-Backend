<template>
   <b-row class="col-auto">
      <b-col md="12" sm="12">
         <h6 class="inputTitle">{{ $t('employeePicture') }} ({{ $t('Fayl hajmi 200 kb dan oshmasligi kerak') }})</h6>
         <b-form-file
            ref="file-input"
            type="file"
            accept=".jpg, .png, .gif, .jpeg"
            :placeholder="$t('admimageid')"
            @change="UploadFile"
         ></b-form-file>
         <p v-if="errorFile" style="font-weight: 600; color: red">{{ $t('Fayl hajmi 200 KB dan ortiq ') }}</p>
      </b-col>
      <div class="mt-1 ml-1 position-relative" v-if="pictureId">
         <b-img
            :src="axios.defaults.baseURL + 'Person/DownloadFile/' + pictureId"
            width="150"
            height="150"
            class="cursor-pointer"
            rounded
            @click="() => showImg(0)"
         />
         <b-button
            variant="danger"
            style="right: 5px; top: 5px"
            class="position-absolute"
            @click="FileDelete(pictureId)"
            rounded
         >
            <feather-icon icon="Trash2Icon"></feather-icon>
         </b-button>
      </div>

      <vue-easy-lightbox :visible="visible" :imgs="lightboxImages" :index="indexRef" @hide="onHide"></vue-easy-lightbox>
   </b-row>
</template>

<script>
import { BRow, BCol, BButton, BFormFile, BImg } from 'bootstrap-vue';
import PersonService from '@/services/others/person.service.js';
import axios from 'axios';
import Compressor from 'compressorjs';
import VueEasyLightbox from 'vue-easy-lightbox';
import 'vue-easy-lightbox/dist/external-css/vue-easy-lightbox.css';
import EmployeeService from '@/services/info/employee.service';

export default {
   data() {
      return {
         visible: false,
         indexRef: 0,
         axios,
         errorFile: false
      };
   },
   components: {
      BRow,
      BCol,
      BButton,
      BFormFile,
      BImg,
      VueEasyLightbox
   },
   props: {
      pictureId: {
         type: String,
         default: null
      },
      personId: {
         type: Number,
         default: 0
      }
   },
   emits: ['update:pictureId'],
   computed: {
      lightboxImages() {
         return [axios.defaults.baseURL + 'Person/DownloadFile/' + this.pictureId];
      }
   },
   methods: {
      onHide() {
         this.visible = false;
      },
      showImg(i) {
         this.indexRef = i;
         this.visible = true;
      },
      getFileExtension(filename) {
         const ext = /^.+\.([^.]+)$/.exec(filename);
         return ext == null ? '' : ext[1];
      },
      getImg(e) {
         const selfThis = this;
         PersonService.GetFromGSP({
            transaction_id: 3,
            is_consent: 'Y',
            langId: 1,
            document: `${e.filter.Seria}${e.filter.Number}`,
            birth_date: e.filter.DateOfBirth.split('.').reverse().join('-'),
            is_photo: 'Y'
         })
            .then((res) => {
               const fileInput = this.$refs['file-input']?.$el;
               fileInput.value = 8;

               const base64Image = `${res.data[0].photo}`;

               const byteCharacters = atob(base64Image);
               const byteNumbers = new Array(byteCharacters.length);
               for (let i = 0; i < byteCharacters.length; i++) {
                  byteNumbers[i] = byteCharacters.charCodeAt(i);
               }
               const byteArray = new Uint8Array(byteNumbers);
               const blob = new Blob([byteArray]);

               const fileName = 'image.jpg';
               const imageFile = new File([blob], fileName, { type: 'image/jpeg' });
               fileInput.files = [imageFile];

               const compressorFile = new Compressor(imageFile, {
                  quality: 0.5,

                  // which means you have to access the `result` in the `success` hook function.
                  success(result) {
                     const myFile = new File([result], result.name, {
                        type: result.type
                     });

                     selfThis.UploadFile({
                        target: {
                           files: [myFile]
                        }
                     });
                  },
                  error(err) {
                     console.log(err.message);
                  }
               });
            })
            .catch((err) => {
               this.showApiError(err);
            })
            .finally(() => {
               this.personLoading = false;
            });
      },
      UploadFile(event) {
         const formData = new FormData();
         formData.append('file', event.target.files[0]);
         this.fileLoading = true;
         PersonService.UploadFile(formData)
            .then((res) => {
               const id = res.data.fileId;
               this.$emit('update:pictureId', id);

               if (id) {
                  this.errorFile = false;
               }
               // if (this.personId) {
               //    EmployeeService.AddOrUpdatePersonFiles({ personId: this.personId, pictureId: id })
               //       .then(() => {
               //          this.errorFile = false;
               //          this.makeToast(this.$t('SaveSuccess'), '200');
               //       })
               //       .catch((e) => {
               //          this.showApiError(e);
               //       });
               // }
            })
            .catch((error) => {
               this.$refs['file-input'].reset();
               this.errorFile = true;
               this.makeToast(error.response.data, 'danger');
            })
            .finally(() => {
               this.fileLoading = false;
            });
      },
      FileDelete(id) {
         PersonService.DeleteFile(id)
            .then(() => {
               this.$emit('update:pictureId', null);
            })
            .catch((e) => {
               this.showApiError(e);
            });
      },
      DownloadFile(id) {
         PersonService.DownloadFile(id).then(() => {});
      }
   }
};
</script>

<style lang="scss" scoped></style>
