<template>
   <b-overlay :show="show">
      <b-row>
         <b-col sm="12" md="12" lg="12" class="text-center">
            <b-row class="justify-content-end">
               <b-col sm="12" md="6" lg="6" class="text-center">
                  <b-card>
                     <div style="max-height: 80vh; overflow-y: auto" class="pt-4">
                        <iframe
                           v-if="Data.id2"
                           style="width: 80vh; height: 100vh"
                           :src="
                              axios.defaults.baseURL + `PrtnContract/PrintPrtnContractPdf?Id2=${Data.id2}&lang=${lang}`
                           "
                           frameborder="0"
                        ></iframe>
                     </div>
                  </b-card>
               </b-col>
               <b-col sm="6" md="3" lg="3">
                  <div class>
                     <b-button class="mr-2" @click="Print(Data.id2)" style="width: 100%" size="xl" variant="primary">
                        <feather-icon icon="PrinterIcon"></feather-icon>
                        {{ $t('Print') }}
                     </b-button>
                     <b-button
                        v-if="$can('PrtnContractReject', 'permissions') && Data.canReject"
                        class="mt-2 mr-2"
                        @click="OpenReject"
                        style="width: 100%"
                        size="xl"
                        variant="outline-danger"
                     >
                        <feather-icon icon="XCircleIcon"></feather-icon>
                        {{ $t('Reject') }}
                     </b-button>
                     <b-button
                        v-if="$can('PrtnContractAgree', 'permissions') && Data.canSign"
                        class="mt-2"
                        @click="OpenAgree"
                        style="width: 100%"
                        size="xl"
                        variant="outline-success"
                     >
                        <feather-icon icon="CheckIcon"></feather-icon>
                        {{ $t('Agree') }}
                     </b-button>
                     <b-button
                        v-if="$can('PrtnContractCancel', 'permissions') && Data.canCancel"
                        class="mt-2"
                        @click="OpenCancel"
                        style="width: 100%"
                        size="xl"
                        variant="outline-danger"
                     >
                        <feather-icon icon="XCircleIcon"></feather-icon>
                        {{ $t('Cancel') }}
                     </b-button>
                     <b-button
                        v-if="$can('PrtnContractSign', 'permissions') && Data.canSign"
                        class="mt-2"
                        @click="OpenSign"
                        style="width: 100%"
                        size="xl"
                        variant="outline-success"
                     >
                        <feather-icon icon="CheckIcon"></feather-icon>
                        {{ $t('Sign') }}
                     </b-button>

                     <b-button
                        v-if="$can('PrtnContractPassExpertise', 'permissions') && Data.canPassExpertise"
                        class="mt-2"
                        @click="OpenPassExpertise"
                        style="width: 100%"
                        size="xl"
                        variant="outline-success"
                     >
                        <feather-icon icon="CheckIcon"></feather-icon>
                        {{ $t('PassExpertise') }}
                     </b-button>
                     <!-- $can('PrtnContractResendExpertise', 'permissions') && -->
                     <b-button
                        v-if="Data.reSendForExpertise"
                        class="mt-2"
                        @click="OpenResendExpertise"
                        style="width: 100%"
                        size="xl"
                        variant="outline-success"
                     >
                        <feather-icon icon="CheckIcon"></feather-icon>
                        {{ $t('AgainPassExpertise') }}
                     </b-button>
                     <b-button
                        v-if="$can('PrtnContractNotPassExpertise', 'permissions') && Data.canNotPassExpertise"
                        class="mt-2"
                        @click="OpenNotPassExpertise"
                        style="width: 100%"
                        size="xl"
                        variant="outline-danger"
                     >
                        <feather-icon icon="XCircleIcon"></feather-icon>
                        {{ $t('NotPassExpertise') }}
                     </b-button>

                     <!-- <div class="mt-1" v-if="DocumentHistoryList.message">
                {{$t('historyLastMessage')}} :
                <b-badge variant="primary">aa</b-badge>
              </div>-->

                     <b-card class="mt-2 w-100" v-if="DocumentHistoryList.message">
                        <b-card-text>{{ DocumentHistoryList.message }}</b-card-text>
                     </b-card>

                     <div class="mt-1" v-for="item in Data.files" :key="item.id" style="cursor: pointer">
                        {{ item.fileName }}
                        <b-badge variant="success" class="ml-1" @click="FileDownload(item)">
                           <feather-icon icon="DownloadIcon"></feather-icon>
                        </b-badge>
                     </div>
                  </div>
               </b-col>
            </b-row>
            <b-modal v-model="RejectModal" no-close-on-backdrop :title="$t('Reject')" hide-footer>
               <b-card-text>
                  <h5 class="mb-2">{{ $t('WantReject') }}</h5>
                  <div class="form-group">
                     <b-form-checkbox v-model="filter.cancelApplication">{{ $t('cancelApplication') }}</b-form-checkbox>
                  </div>
                  <div class="form-group">
                     <form-input v-model="filter.message" :label="$t('message')" />
                  </div>
                  <div class="form-group">
                     <form-select
                        :options="PrtnRejectReasonList"
                        v-model="filter.prtnRejectReasonId"
                        :label="$t('prtnRejectReason')"
                     ></form-select>
                  </div>
                  <div class="d-flex justify-content-end">
                     <b-button
                        class="mt-2 mr-2"
                        @click="RejectModal = !RejectModal"
                        style="width: 100%"
                        size="xl"
                        variant="danger"
                        >{{ $t('no') }}</b-button
                     >
                     <b-button class="mt-2" @click="Reject" style="width: 100%" size="xl" variant="success">{{
                        $t('yes')
                     }}</b-button>
                  </div>
               </b-card-text>
            </b-modal>

            <b-modal v-model="CancelModal" :title="$t('Cancel')" hide-footer>
               <b-card-text>
                  <h5 class="mb-2">{{ $t('WantCancel') }}</h5>
                  <div class="form-group">
                     <b-form-checkbox v-model="filter.cancelApplication">{{ $t('cancelApplication') }}</b-form-checkbox>
                  </div>
                  <b-form-textarea
                     id="textarea"
                     v-model="filter.message"
                     :placeholder="$t('message')"
                     rows="3"
                     max-rows="6"
                  ></b-form-textarea>
                  <div class="d-flex justify-content-end">
                     <b-button
                        class="mt-2 mr-2"
                        @click="CancelModal = !CancelModal"
                        style="width: 100%"
                        size="xl"
                        variant="danger"
                        >{{ $t('no') }}</b-button
                     >
                     <b-button class="mt-2" @click="Cancel" style="width: 100%" size="xl" variant="success">{{
                        $t('yes')
                     }}</b-button>
                  </div>
               </b-card-text>
            </b-modal>
            <b-modal v-model="PassExpertiseModal" :title="$t('PassExpertise')" hide-footer>
               <b-card-text>
                  <h5 class="mb-2">{{ $t('WantPassExpertise') }}</h5>
                  <b-form-textarea
                     id="textarea"
                     v-model="filter.message"
                     :placeholder="$t('message')"
                     rows="3"
                     max-rows="6"
                  ></b-form-textarea>

                  <b-form-file
                     type="file"
                     :placeholder="$t('chooseFile')"
                     class="mt-2"
                     @change="UploadFile"
                  ></b-form-file>

                  <div class="mt-1" v-for="item in filter.files" :key="item.id">
                     <b-badge variant="danger" class="ml-1" @click="FileDelete(item.id)">
                        <feather-icon icon="Trash2Icon"></feather-icon>
                     </b-badge>
                  </div>

                  <div class="d-flex justify-content-end">
                     <b-button
                        class="mt-2 mr-2"
                        @click="PassExpertiseModal = !PassExpertiseModal"
                        style="width: 100%"
                        size="xl"
                        variant="danger"
                        >{{ $t('no') }}</b-button
                     >
                     <b-button class="mt-2" @click="PassExpertise" style="width: 100%" size="xl" variant="success">{{
                        $t('yes')
                     }}</b-button>
                  </div>
               </b-card-text>
            </b-modal>
            <b-modal v-model="ResendExpertiseModal" :title="$t('AgainPassExpertise')" hide-footer>
               <b-card-text>
                  <b-form-textarea
                     id="textarea"
                     v-model="filter.message"
                     :placeholder="$t('message')"
                     rows="3"
                     max-rows="6"
                  ></b-form-textarea>

                  <div class="d-flex justify-content-end">
                     <b-button
                        class="mt-2 mr-2"
                        @click="ResendExpertiseModal = !ResendExpertiseModal"
                        style="width: 100%"
                        size="xl"
                        variant="danger"
                        >{{ $t('no') }}</b-button
                     >
                     <b-button class="mt-2" @click="ResendExpertise" style="width: 100%" size="xl" variant="success">{{
                        $t('yes')
                     }}</b-button>
                  </div>
               </b-card-text>
            </b-modal>
            <b-modal v-model="NotPassExpertiseModal" :title="$t('NotPassExpertise')" hide-footer>
               <b-card-text>
                  <h5 class="mb-2">{{ $t('WantNotPassExpertise') }}</h5>

                  <b-form-textarea
                     id="textarea"
                     v-model="filter.message"
                     :placeholder="$t('message')"
                     rows="3"
                     max-rows="6"
                  ></b-form-textarea>

                  <b-form-file
                     type="file"
                     :placeholder="$t('chooseFile')"
                     class="mt-2"
                     @change="UploadFile"
                  ></b-form-file>

                  <div class="mt-1" v-for="item in filter.files" :key="item.id">
                     {{ item.fileName }}
                     <b-badge variant="danger" class="ml-1" @click="FileDelete(item.id)">
                        <feather-icon icon="Trash2Icon"></feather-icon>
                     </b-badge>
                  </div>

                  <div class="d-flex justify-content-end">
                     <b-button
                        class="mt-2 mr-2"
                        @click="NotPassExpertiseModal = !NotPassExpertiseModal"
                        style="width: 100%"
                        size="xl"
                        variant="danger"
                        >{{ $t('no') }}</b-button
                     >
                     <b-button class="mt-2" @click="NotPassExpertise" style="width: 100%" size="xl" variant="success">{{
                        $t('yes')
                     }}</b-button>
                  </div>
               </b-card-text>
            </b-modal>
            <b-modal v-model="EImzoModal" size="lg" :title="$t('EImzo')" hide-footer>
               <b-card-text>
                  <just-sign :data-to-sign="Data" v-if="!AgreeLoading || !SignLoading" @sign="loginESP($event)" />
                  <div
                     style="height: 600px"
                     v-if="AgreeLoading || SignLoading"
                     class="d-flex justify-content-center align-items-center"
                  >
                     <b-spinner label="Spinning"></b-spinner>
                  </div>
               </b-card-text>
            </b-modal>
         </b-col>
      </b-row>
   </b-overlay>
</template>

<script>
import {
   BOverlay,
   BCard,
   BRow,
   BCol,
   BFormInput,
   BTabs,
   BTab,
   BButton,
   BTable,
   BLink,
   BFormGroup,
   VBTooltip,
   BModal,
   VBModal,
   BCardText,
   BInputGroup,
   BInputGroupAppend,
   BTr,
   BTd,
   BFormTextarea,
   BFormCheckbox,
   BIcon,
   BBadge,
   BSpinner,
   BFormFile
} from 'bootstrap-vue';

import Ripple from 'vue-ripple-directive';
import flatPickr from 'vue-flatpickr-component';
import Cleave from 'vue-cleave-component';
import justSign from '@/components/justSign.vue';
import PrtnContractService from '@/services/document/prtncontract.service';
import PrtnRejectReasonService from '@/services/info/prtnrejectreason.service';
import DocumentHistoryService from '@/services/others/documenthistory.service';

import axios from 'axios';
import eimzoMixin from '@/mixins/eimzo';
import '@core/scss/vue/libs/vue-flatpicker.scss';

export default {
   components: {
      BOverlay,
      BCard,
      BRow,
      BCol,
      BFormInput,
      BTabs,
      BTab,
      BButton,
      BTable,
      BLink,
      flatPickr,
      BFormGroup,
      BModal,
      BCardText,
      BInputGroup,
      BInputGroupAppend,
      BTr,
      BTd,
      BFormTextarea,
      BFormCheckbox,
      BIcon,
      BBadge,
      Cleave,
      justSign,
      BSpinner,
      BFormFile
   },
   directives: {
      'b-tooltip': VBTooltip,
      'b-modal': VBModal
   },
   mixins: [eimzoMixin],
   data() {
      return {
         axios,
         PrtnContractModal: false,
         PrtnRejectReasonList: [],
         DocumentHistoryList: {},

         EImzoModal: false,

         RejectModal: false,
         SignModal: false,
         CancelModal: false,
         AgreeModal: false,
         NotPassExpertiseModal: false,
         PassExpertiseModal: false,
         ResendExpertiseModal: false,

         RejectLoading: false,
         SignLoading: false,
         CancelLoading: false,
         AgreeLoading: false,
         PassExpertiseLoading: false,
         NotPassExpertiseLoading: false,
         ResendExpertiseLoading: false,

         step: '',
         show: false,
         Data: {},
         filter: {
            signedData: '',
            id: 0,
            message: '',
            prtnRejectReasonId: 0,
            cancelApplication: false,
            files: []
         },
         HtmlData: {},
         HtmlDataInfo: '',
         lang: 'ru',
         fileLoading: false
      };
   },
   created() {
      this.lang = localStorage.getItem('locale') || 'ru';
      if (this.lang == 'uz_cyrl') {
         this.lang = 'uz-cyrl';
      }
      if (this.lang == 'uz_latn') {
         this.lang = 'uz-latn';
      }
      this.show = true;
      PrtnContractService.Get(this.$route.params.id)
         .then((res) => {
            this.Data = res.data;

            this.show = false;

            DocumentHistoryService.GetLastMessage({
               DocumentId: this.Data.id,
               TableId: res.data.tableId
            }).then((result) => {
               this.DocumentHistoryList = result.data;
            });
         })
         .catch((error) => {
            this.makeToast(error.response.data, 'danger');
         });

      PrtnRejectReasonService.GetAsSelectList()
         .then((res) => {
            this.PrtnRejectReasonList = res.data;
         })
         .catch((error) => {
            this.showApiError(error);
         });
   },
   directives: {
      Ripple
   },
   methods: {
      UploadFile(event) {
         const formData = new FormData();
         formData.append('files', event.target.files[0]);
         this.fileLoading = true;
         PrtnContractService.UploadFile(formData).then((res) => {
            this.filter.files.push(res.data[0]);
            this.fileLoading = false;
         });
      },
      FileDelete(id) {
         PrtnContractService.DeleteFile(id).then((res) => {
            this.filter.files = this.filter.files.filter((item) => item.id != id);
         });
      },
      FileDownload(item) {
         PrtnContractService.DownloadFile(item.id).then((res) => {
            this.forceFileDownload(res, item.fileName);
         });
      },
      Print(id) {
         PrtnContractService.PrintPrtnContractPdf(id).then((res) => {
            this.forceFileDownload(res, this.$t('prtncontract'));
         });
      },
      forceFileDownload(response, name) {
         var { headers } = response;
         var blob = new Blob([response.data]);
         const url = window.URL.createObjectURL(blob);
         const link = document.createElement('a');
         link.href = url;
         link.setAttribute('download', name); //or any other extension
         document.body.appendChild(link);
         link.click();
      },
      loginESP(item) {
         const isPinfl = this.isPinfl(item);
         if (this.step == 'Agree') {
            this.Agree(item.key, isPinfl);
         } else if (this.step == 'Sign') {
            this.Sign(item.key, isPinfl);
         }
      },
      ClearFilter() {
         this.filter = {
            signedData: '',
            message: '',
            id: this.Data.id,
            prtnRejectReasonId: 0,
            files: []
         };
      },

      OpenCancel() {
         this.CancelModal = true;
         this.ClearFilter();
      },
      Cancel() {
         var self = this;
         this.CancelLoading = true;
         PrtnContractService.Cancel(this.filter)
            .then((res) => {
               self.makeToast(self.$t('CancelMessage'), 'success');
               this.CancelLoading = false;
               this.CancelModal = false;
               this.$router.push({
                  name: 'PrtnContract',
                  params: {
                     id: 0
                  }
               });
            })
            .catch((error) => {
               this.CancelLoading = false;
               self.makeToast(error.response.data.errors, 'danger');
            });
      },
      OpenAgree() {
         this.EImzoModal = true;
         this.step = 'Agree';
         this.ClearFilter();
      },

      Agree(key, isPinfl) {
         var self = this;
         this.AgreeLoading = true;
         this.filter.signedData = key;
         this.filter.isPinfl = isPinfl;
         PrtnContractService.Agree(this.filter)
            .then((res) => {
               self.makeToast(self.$t('AgreeMessage'), 'success');
               this.AgreeLoading = false;
               this.AgreeModal = false;
               this.EImzoModal = false;
               this.$router.push({
                  name: 'PrtnContract',
                  params: {
                     id: 0
                  }
               });
            })
            .catch((error) => {
               self.makeToast(error.response.data.errors, 'danger');
               this.AgreeLoading = false;
            });
      },
      OpenSign() {
         this.EImzoModal = true;
         this.step = 'Sign';
         this.ClearFilter();
      },
      Sign(key, isPinfl) {
         var self = this;
         this.SignLoading = true;
         this.filter.signedData = key;
         this.filter.isPinfl = isPinfl;
         PrtnContractService.Sign(this.filter)
            .then((res) => {
               self.makeToast(self.$t('SignMessage'), 'success');
               this.SignLoading = false;

               this.SignModal = false;
               this.EImzoModal = false;
               this.$router.push({
                  name: 'PrtnContract',
                  params: {
                     id: 0
                  }
               });
            })
            .catch((error) => {
               self.makeToast(error.response.data.errors, 'danger');
               this.SignLoading = false;
            });
      },
      OpenResendExpertise() {
         this.ResendExpertiseModal = true;
         this.ClearFilter();
      },
      ResendExpertise() {
         var self = this;
         this.ResendExpertiseLoading = true;
         PrtnContractService.ResendExpertise(this.filter)
            .then((res) => {
               self.makeToast(self.$t('ResendExpertiseMessage'), 'success');
               this.ResendExpertiseLoading = false;
               this.ResendExpertiseModal = false;
               this.$router.push({
                  name: 'PrtnContract',
                  params: {
                     id: 0
                  }
               });
            })
            .catch((error) => {
               self.makeToast(error.response.data.errors, 'danger');
               this.PassExpertiseLoading = false;
            });
      },
      OpenPassExpertise() {
         this.PassExpertiseModal = true;
         this.ClearFilter();
      },
      PassExpertise() {
         var self = this;
         this.PassExpertiseLoading = true;
         PrtnContractService.PassExpertise(this.filter)
            .then((res) => {
               self.makeToast(self.$t('PassExpertiseMessage'), 'success');
               this.PassExpertiseLoading = false;
               this.PassExpertiseModal = false;
               this.$router.push({
                  name: 'PrtnContract',
                  params: {
                     id: 0
                  }
               });
            })
            .catch((error) => {
               self.makeToast(error.response.data.errors, 'danger');
               this.PassExpertiseLoading = false;
            });
      },
      OpenNotPassExpertise() {
         this.NotPassExpertiseModal = true;
         this.ClearFilter();
      },
      NotPassExpertise() {
         var self = this;
         this.NotPassExpertiseLoading = true;
         PrtnContractService.NotPassExpertise(this.filter)
            .then((res) => {
               self.makeToast(self.$t('NotPassExpertiseMessage'), 'success');
               this.NotPassExpertiseLoading = false;
               this.NotPassExpertiseModal = false;
               this.$router.push({
                  name: 'PrtnContract',
                  params: {
                     id: 0
                  }
               });
            })
            .catch((error) => {
               self.makeToast(error.response.data.errors, 'danger');
               this.NotPassExpertiseLoading = false;
            });
      },
      OpenReject() {
         this.RejectModal = true;
         this.ClearFilter();
      },
      Reject(item) {
         var self = this;
         PrtnContractService.Reject(this.filter)
            .then((res) => {
               self.makeToast(self.$t('RejectMessage'), 'success');
               this.RejectModal = false;
               this.$router.push({
                  name: 'PrtnContract',
                  params: {
                     id: 0
                  }
               });
            })
            .catch((error) => {
               self.makeToast(error.response.data.errors, 'danger');
            });
      },
      SaveData() {
         PrtnContractService.Update(this.Data)
            .then((res) => {
               this.makeToast(this.$t('SaveSuccess'), 'success');
               this.$router.push({ name: 'PrtnContract' });
            })
            .catch((err) => {
               this.makeToast(err.response.data.errors, 'danger');
            });
      }
   }
};
</script>
