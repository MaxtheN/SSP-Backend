<template>
   <b-row>
      <b-col sm="12" md="12" lg="12">
         <b-card>
            <h1 class="mb-2">Unversial Print</h1>
            <validation-observer ref="valiadationDownload">
               <b-row>
                  <b-col sm="6" md="6">
                     <b-row>
                        <b-col>
                           <form-select :options="SelectList" required-star v-model="printId" :label="$t('appId')" />
                        </b-col>
                        <b-col class="mt-2">
                           <b-button @click="Print" :disabled="PrintLoading" variant="primary" class="ml-1">
                              <feather-icon icon="PrinterIcon"></feather-icon>
                              {{ $t('Print') }}
                           </b-button>
                        </b-col>
                     </b-row>
                  </b-col>
               </b-row>
            </validation-observer>
            <hr class="my-3" />
            <validation-observer ref="validationUpload">
               <b-row>
                  <b-col>
                     <form-select :options="SelectList" required-star v-model="Data.tableId" label="Hujjat turi">
                     </form-select>
                  </b-col>
                  <b-col sm="12" md="3">
                     <form-select
                        :options="LanguageSelectList"
                        v-model="Data.languageId"
                        label="Hujjat tili"
                     ></form-select>
                  </b-col>
                  <b-col sm="12" md="3">
                     <b-form-file
                        v-model="file"
                        class="mt-2"
                        placeholder="Choose a file or drop it here..."
                        drop-placeholder="Drop file here..."
                        required-star
                        accept=".docx"
                        @change="UploadFile"
                     ></b-form-file>
                     <span v-if="error" class="text-danger">{{ error }}</span>
                  </b-col>
                  <b-col sm="12" md="3">
                     <b-button variant="success" @click="saveFile" class="mt-2">
                        <feather-icon icon="SaveIcon"></feather-icon>
                        {{ $t('Save') }}
                     </b-button>
                  </b-col>
               </b-row>
            </validation-observer>
         </b-card>
      </b-col>
   </b-row>
</template>

<script>
//componnets
import { ValidationProvider, ValidationObserver } from 'vee-validate';
import {
   BOverlay,
   BCard,
   BRow,
   BCol,
   BSpinner,
   BFormInput,
   BTable,
   BButton,
   BButtonGroup,
   BLink,
   BFormGroup,
   BModal,
   BInputGroup,
   BInputGroupAppend,
   BFormCheckbox,
   BFormTextarea,
   BTableSimple,
   BThead,
   BTr,
   BTh,
   BTd,
   BTbody,
   BTfoot,
   BFormFile,
   BIconTrash
} from 'bootstrap-vue';

import UnversalPrintService from '@/services/managment/unversalprint.service';
// service
import ManualService from '@/services/others/manual.service';

const forceFileDownload = (response, title) => {
   const url = window.URL.createObjectURL(new Blob([response.data]));
   const link = document.createElement('a');
   link.href = url;
   link.download = `${title}.docx`;
   link.click();
};

export default {
   components: {
      BOverlay,
      BCard,
      BRow,
      BCol,
      BFormInput,
      BButtonGroup,
      BButton,
      BTable,
      BLink,
      BFormGroup,
      BModal,
      BSpinner,
      BInputGroup,
      BInputGroupAppend,
      BFormCheckbox,
      BFormTextarea,
      ValidationProvider,
      ValidationObserver,
      BTableSimple,
      BThead,
      BTr,
      BTh,
      BTd,
      BTbody,
      BTfoot,
      BFormFile,
      BIconTrash
   },
   data() {
      return {
         PrintLoading: false,
         printId: null,
         validate: false,
         SelectList: [],
         file: [],
         LanguageSelectList: [],
         error: '',
         Data: {
            id: '',
            fileName: '',
            fileExtension: '',
            tableId: null,
            languageId: null
         }
      };
   },

   created() {
      UnversalPrintService.SelectList()
         .then((res) => (this.SelectList = res.data))
         .catch((error) => {
            this.makeToast(error.response.data.errors, 'danger');
         });
      ManualService.LanguageSelectList().then((res) => (this.LanguageSelectList = res.data));
   },
   methods: {
      Print() {
         this.$refs.valiadationDownload.validate().then((success) => {
            if (success) {
               this.PrintLoading = true;
               UnversalPrintService.GenerateWord(this.printId)
                  .then((response) => {
                     forceFileDownload(response, this.$t('unversialPrint'));
                     this.PrintLoading = false;
                     this.printId = 0;
                     this.$refs.valiadationDownload.reset();
                  })
                  .catch((error) => {
                     this.showApiError(error);
                  });
            }
         });
      },
      UploadFile(e) {
         const formData = new FormData();
         formData.append('files', e.target.files[0]);
         UnversalPrintService.UploadFile(formData).then((res) => {
            this.validate = true;
            this.Data.fileName = res.data.fileName;
            this.Data.id = res.data.id;
         });
      },
      saveFile() {
         this.$refs.validationUpload.validate().then((success) => {
            if (success) {
               if (this.file.length == 0) {
                  this.error = 'file yuklanmagan';
               } else {
                  this.error = '';
               }

               // console.log(this.Data, 'ddd');
               UnversalPrintService.SaveTemplate(this.Data)
                  .then((res) => {
                     if (res.status == 200) {
                        this.makeToast(this.$t('SaveSuccess'), 'success');
                        this.Data.languageId = null;
                        this.Data.fileName = '';
                        this.Data.tableId = '';
                        this.Data.id = '';
                        this.Data.fileExtension = '';
                        this.file = [];
                        this.$refs.valiadationDownload.reset();
                     }
                     this.$refs.validationUpload.reset();
                  })
                  .catch((error) => this.showApiError(error));
               // console.log(this.Data);
            }
         });
      }
   }
};
</script>

<style lang="scss" scoped></style>
