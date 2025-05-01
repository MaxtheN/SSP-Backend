<template>
   <b-overlay :show="show">
      <b-row class="mt-2">
         <b-col sm="9" md="9" lg="9">
            <b-card>
               <validation-observer ref="ValidationDTO">
                  <b-row class="align-content-center">
                     <b-col sm="12" md="6" class="mb-1">
                        <span style="font-weight: bold">{{ $t('contractor') }}</span
                        >: {{ Data.contractorName }}
                     </b-col>
                     <b-col sm="12" md="6" class="mb-1">
                        <span style="font-weight: bold">{{ $t('responsible') }}</span
                        >: {{ Data.responsibleContractorName }}
                     </b-col>
                     <b-col sm="12" md="3">
                        <form-input-hrm
                           v-model="Data.docNumber"
                           :label="$t('docNumber')"
                           :placeholder="$t('docNumber')"
                        />
                     </b-col>
                     <b-col sm="12" md="3">
                        <form-picker
                           rules="required"
                           v-model="Data.docOn"
                           :label="$t('docOn')"
                           :placeholder="$t('docOn')"
                        />
                     </b-col>

                     <b-col sm="12" md="3">
                        <form-picker
                           v-model="Data.discussionDate"
                           required
                           :label="$t('discussionDate')"
                           :placeholder="$t('docOn')"
                        />
                     </b-col>

                     <b-col sm="12" md="6">
                        <h6 class="inputTitle">{{ $t('fileupload') }}</h6>
                        <b-form-file type="file" :placeholder="$t('Faylni tanlang')" @change="UploadFile">
                        </b-form-file>

                        <div class="mt-1" variant="info" v-if="Data.files.length && Data.files[0].id">
                           {{ Data.files[0].fileName }}
                           <b-link style="margin-left: 5px; margin-right: 4px" @click="DownloadFile">
                              <b-spinner v-if="DownloadLoading" small></b-spinner>
                              <feather-icon size="18" style="color: blue" v-if="!DownloadLoading" icon="DownloadIcon">
                              </feather-icon>
                           </b-link>
                           <b-link @click="DeleteFile">
                              <b-spinner v-if="DeleteLoading" small></b-spinner>
                              <feather-icon size="18" style="color: red" v-if="!DeleteLoading" icon="Trash2Icon">
                              </feather-icon>
                           </b-link>
                        </div>
                     </b-col>
                     <b-col sm="12" md="6" class="mt-2">
                        <b-button @click="Print" variant="outline-info">
                           <feather-icon icon="FileIcon"></feather-icon>
                           {{ $t('Print') }}
                        </b-button>
                     </b-col>
                  </b-row>
               </validation-observer>
            </b-card>
         </b-col>
         <b-col sm="3" md="3" lg="3">
            <b-button :disabled="saveLoading" @click="SaveData" style="width: 100%" variant="success">
               <feather-icon icon="CheckIcon"></feather-icon>
               {{ $t('Save') }}
            </b-button>
            <b-button v-if="Data.canSign" class="mt-2" @click="OpenSign" style="width: 100%" size="xl" variant="info">
               <feather-icon icon="CheckIcon"></feather-icon>
               {{ $t('Sign') }}
            </b-button>
         </b-col>
      </b-row>
      <b-modal v-model="EImzoModal" size="lg" :title="$t('EImzo')" hide-footer>
         <b-card-text>
            <just-sign :data-to-sign="Data" v-if="!SignLoading" @sign="loginESP($event)" />
            <div style="height: 600px" v-if="SignLoading" class="d-flex justify-content-center align-items-center">
               <b-spinner label="Spinning"></b-spinner>
            </div>
         </b-card-text>
      </b-modal>
   </b-overlay>
</template>

<script>
import {
   BOverlay,
   BIconCheckLg,
   BBadge,
   BCard,
   BRow,
   BCol,
   BButton,
   BFormFile,
   BInputGroupAppend,
   BSpinner,
   BLink,
   BFormCheckbox,
   BModal,
   BCardText
} from 'bootstrap-vue';
import justSign from '@/components/justSign.vue';
import ArbitrationCourtService from '@/services/arbitrationcourt/arbitrationcourt.service.js';
import ArbitrationDiscussionService from '@/services/arbitrationcourt/arbitrationdiscussion.service.js';
export default {
   components: {
      BOverlay,
      BInputGroupAppend,
      BCard,
      BRow,
      BCol,
      BButton,
      justSign,
      BFormFile,
      BIconCheckLg,
      BBadge,
      BSpinner,
      BLink,
      BFormCheckbox,
      BModal,
      BCardText
   },
   data() {
      return {
         saveLoading: false,
         show: false,
         SignLoading: false,
         EImzoModal: false,
         SignModal: false,
         filter: {},
         pBarSize: '',
         DeleteLoading: false,
         DownloadLoading: false,

         Data: {
            files: []
         }
      };
   },
   created() {
      if (this.$route.query.courtId) {
         ArbitrationDiscussionService.GetByArbitrationCourtApplicationId(this.$route.query.courtId)
            .then((res) => {
               this.Data = res.data;
            })
            .catch((error) => {
               this.makeToast(error.response.data.errors, 'danger');
            })
            .finally(() => {
               this.show = false;
            });
      } else {
         ArbitrationDiscussionService.Get(this.$route.params.id)
            .then((res) => {
               this.Data = res.data;
            })
            .catch((error) => {
               this.makeToast(error.response.data.errors, 'danger');
            })
            .finally(() => {
               this.show = false;
            });
      }
   },

   methods: {
      loginESP(item) {
         this.Sign(item.key);
      },
      Sign(signedData) {
         this.SignLoading = true;
         ArbitrationDiscussionService.Sign({
            id: this.Data.id,
            signedData: signedData
         })
            .then(() => {
               this.makeToast(this.$t('AcceptSuccess'), 'success');
               this.Refresh();
               this.EImzoModal = false;
            })
            .catch((err) => {
               this.showApiError(err);
            })
            .finally(() => {
               this.SignLoading = false;
            });
      },
      OpenSign() {
         this.EImzoModal = true;
         this.ClearFilter();
      },
      ClearFilter() {
         this.filter = {
            signedData: '',
            message: '',
            id: this.Data.id,
            prtnRejectReasonId: 0
         };
      },
      CheckFile(file) {
         this.Data.files.forEach((item) => {
            if (item.id == file.id) {
               item.canSign = true;
            } else {
               item.canSign = false;
            }
         });
      },

      UploadFile(event) {
         const formData = new FormData();
         formData.append('files', event.target.files[0]);
         this.fileLoading = true;

         ArbitrationDiscussionService.UploadFile(formData).then((res) => {
            this.Data.files.push(res.data[0]);

            this.fileLoading = false;
         });
      },
      DeleteFile() {
         this.DownloadLoading = true;
         ArbitrationDiscussionService.DeleteFile(this.Data.files[0].id).then((_res) => {
            this.DownloadLoading = false;
            this.Data.files.splice(0, 1);
         });
      },
      Print(file) {
         ArbitrationCourtService.DownloadTemplate(this.getPdfLang()).then((res) => {
            this.forceFileDownload(res, 'sign-template', '.docx');
         });
      },
      DownloadFile() {
         ArbitrationDiscussionService.DownloadFile(this.Data.files[0].id).then((res) => {
            this.forceFileDownload(res, this.Data.files[0].fileName, this.Data.files[0].fileExtension);
         });
      },
      SaveData() {
         this.$refs.ValidationDTO.validate().then((success) => {
            if (success) {
               this.saveLoading = true;
               ArbitrationDiscussionService.Update(this.Data)
                  .then((res) => {
                     this.makeToast(this.$t('SaveSuccess'), 'success');
                     ArbitrationDiscussionService.Get(res.data.id)
                        .then((res1) => {
                           this.Data = res1.data;
                        })
                        .catch((error) => {
                           this.makeToast(error.response.data.errors, 'danger');
                        })
                        .finally(() => {
                           this.show = false;
                        });
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
