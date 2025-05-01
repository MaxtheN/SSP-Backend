<template>
   <b-overlay :show="show">
      <b-row class="justify-content-end">
         <b-col sm="12" md="7" lg="7">
            <b-card>
               <b-tabs class="nav-tabs nav-justified">
                  <b-tab :title="$t('Info')">
                     <MemshipContractFormView :contract="Contract" :isChange="$route.params.isChange" />
                  </b-tab>
                  <b-tab active :title="$t('MemshipContract')">
                     <WIframe :src="IframeSrcContract" style="height: 100vh" :show="Contract.id2" />
                  </b-tab>
                  <b-tab :title="$t('MemshipApplication')">
                     <WIframe
                        :src="IframeSrcApplication"
                        style="height: 100vh"
                        :show="Contract.memshipApplicationId2"
                     />
                  </b-tab>
               </b-tabs>
            </b-card>
         </b-col>
         <b-col sm="6" md="3" lg="3">
            <div>
               <!-- print -->
               <a class="mr-2 btn btn-primary" :href="IframeSrcContract" target="_blank" style="width: 100%">
                  <feather-icon icon="PrinterIcon"></feather-icon>
                  {{ $t('Print') }}
               </a>

               <b-button
                  v-if="$can('ChangeContractorToPad', 'permissions') && Contract.canChangeToPaid"
                  class="mt-2"
                  @click="OpenChange"
                  style="width: 100%"
                  size="xl"
                  variant="outline-primary"
               >
                  <feather-icon icon="RefreshCwIcon"></feather-icon>
                  {{ $t('ChangeToPaid') }}
               </b-button>
               <b-button
                  v-if="$can('MemshipContractSign', 'permissions') && Contract.canSign"
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
                  v-if="
                     Contract.id && !Contract.canSign && Contract.statusId == 21 && $route.params.hasCertificate != true
                  "
                  :to="{
                     name: 'EditMemshipCertificate',
                     params: { id: 0 },
                     query: {
                        contractId: Contract.id,
                        isList: true
                     }
                  }"
                  class="mt-2 w-100"
                  size="xl"
                  variant="outline-primary"
               >
                  <feather-icon icon="FileIcon"></feather-icon>
                  {{ $t('GetSertificate') }}
               </b-button>

               <b-alert v-if="Contract.statusId == 24 && Contract.message" class="mt-1" show variant="danger">
                  <p class="px-1">{{ Contract.message }}</p>
               </b-alert>

               <template v-if="Contract.canConfirm">
                  <!-- reject -->
                  <!-- <b-button class="mt-2 mr-2" @click="Reject" style="width: 100%" size="xl" variant="outline-danger">
                     <feather-icon icon="XCircleIcon"></feather-icon>
                     {{ $t('Reject') }}
                  </b-button> -->
                  <b-button
                     class="mt-2 w-100"
                     size="xl"
                     variant="success"
                     @click="confirmUpdate()"
                     :disabled="canConfirmLoading"
                  >
                     <feather-icon icon="CheckIcon"></feather-icon>
                     {{ $t('Imzoga tayyor') }}
                  </b-button>
               </template>

               <template v-if="Contract.canModify">
                  <b-button class="mt-2" @click="DownloadTemplate" style="width: 100%" size="xl" variant="info">
                     <feather-icon icon="DownloadIcon"></feather-icon>
                     {{ $t('download') }}
                  </b-button>
                  <div class="mt-2">
                     <h6 class="inputTitle">{{ $t('fileupload') }}</h6>
                     <b-form-file type="file" :placeholder="$t('Faylni tanlang')" @change="MemshipContractUpload">
                     </b-form-file>
                  </div>
               </template>

               <b-card class="mt-2 w-100" v-if="DocumentHistoryList.message">
                  <b-card-text>{{ DocumentHistoryList.message }}</b-card-text>
               </b-card>

               <div class="mt-1" v-for="item in Contract.files" :key="item.id" style="cursor: pointer">
                  {{ item.fileName }}
                  <b-badge variant="success" class="ml-1" @click="FileDownload(item)">
                     <feather-icon icon="DownloadIcon"></feather-icon>
                  </b-badge>
               </div>
            </div>
         </b-col>
      </b-row>
      <b-modal v-model="ChangeToPaidModal" hide-footer>
         <template #modal-title>
            {{ $t('ChangeToPaid') }}
         </template>
         <b-card-text>
            <b-col sm="12" md="12">
               <form-input-hrm v-model="selectedItem.docNumber" :label="$t('AdditionalInfo')" debounce="500" />
            </b-col>
            <b-col sm="12" md="12">
               <form-input-hrm
                  v-model="selectedItem.baseFixedMinimumValue"
                  rules="required"
                  type="number"
                  :label="$t('baseFixedMinimumValue')"
                  debounce="500"
               />
            </b-col>
            <b-col sm="12" md="12">
               <form-select
                  :options="OrgSettlementAccountList"
                  v-model="selectedItem.organizationSettlementAccountId"
                  @option:selected="(e) => (selectedItem.organizationSettlementAccount = e ? e.text : '')"
                  requitred-star
                  label="orgSettlementAccount"
               />
            </b-col>
            <b-col sm="12" md="12">
               <form-select
                  :options="OrganizationGroupSelectList"
                  v-model="selectedItem.regionalOrganizationId"
                  @option:selected="(e) => (selectedItem.organizationSettlementAccount = e ? e.text : '')"
                  requitred-star
                  label="organizationGroup"
               />
            </b-col>
            <b-col sm="12" md="12">
               <form-input-hrm v-model="selectedItem.details" :label="$t('AdditionalInfo')" debounce="500" />
            </b-col>
         </b-card-text>
         <b-row>
            <b-col>
               <div class="d-flex justify-content-end mt-1">
                  <b-button class="mr-1" @click="ChangeToPaidModal != ChangeToPaidModal" size="xl" variant="danger">{{
                     $t('no')
                  }}</b-button>
                  <b-button @click="ChangeToPaid" size="xl" variant="success">
                     {{ $t('yes') }}
                  </b-button>
               </div>
            </b-col>
         </b-row>
      </b-modal>
      <!-- sign dialog -->
      <b-modal v-model="EImzoModal" size="lg" :title="$t('EImzo')" hide-footer>
         <b-card-text>
            <just-sign :data-to-sign="Contract" v-if="!SignLoading" @sign="loginESP($event)" />
            <div style="height: 600px" v-if="SignLoading" class="d-flex justify-content-center align-items-center">
               <b-spinner label="Spinning"></b-spinner>
            </div>
         </b-card-text>
      </b-modal>

      <Chat v-if="Contract && Contract.id" :table-id="Contract.tableId || 99" :document-id="Contract.id" />
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
   BFormFile,
   BAlert
} from 'bootstrap-vue';

import justSign from '@/components/justSign.vue';
import MemshipContractService from '@/services/document/memshipcontract.service';
import OrganizationService from '@/services/managment/organization.service';
import axios from 'axios';
import ManualService from '@/services/others/manual.service';
import eimzoMixin from '@/mixins/eimzo';
import WIframe from '@/components/WIframe.vue';
import MemshipContractFormView from '@/views/components/memship/MemshipContractFormView.vue';
const Chat = () => import('@/views/components/DocumentChat/Chat.vue');

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
      justSign,
      BSpinner,
      BFormFile,
      WIframe,
      BAlert,
      MemshipContractFormView,
      Chat
   },
   directives: {
      'b-tooltip': VBTooltip,
      'b-modal': VBModal
   },
   mixins: [eimzoMixin],
   data() {
      return {
         page: false,
         ChangeToPaidModal: false,
         axios,
         selectedItem: {},
         DocumentHistoryList: {},
         OrgSettlementAccountList: [],
         OrganizationGroupSelectList: [],
         EImzoModal: false,
         SignModal: false,
         SignLoading: false,
         show: false,
         Contract: {},
         canConfirmLoading: false,
         filter: {
            signedData: '',
            id: 0,
            message: '',
            cancelApplication: false,
            files: []
         },
         fileLoading: false
         // IframeSrcContract: ''
      };
   },
   computed: {
      IframeSrcContract() {
         return (
            axios.defaults.baseURL +
            `Memship/MemshipContract/DownloadPdf?id2=${this.Contract?.id2}&lang=${this.getPdfLang()}`
         );
      },

      IframeSrcApplication() {
         return (
            axios.defaults.baseURL +
            `Memship/MemshipApplication/DownloadPdf?id2=${
               this.Contract?.memshipApplicationId2
            }&lang=${this.getPdfLang()}`
         );
      }
   },

   created() {
      this.GetContract();
      OrganizationService.GetAsSelectListOrgSettlementAccount()
         .then((res3) => {
            if (Array.isArray(res3.data)) {
               this.OrgSettlementAccountList = res3.data;
            }
         })
         .catch((error) => {
            this.showApiError(error);
         });
      ManualService.OrganizationAsSelectListByGroup([1, 3])
         .then((res) => {
            this.OrganizationGroupSelectList = res.data;
         })
         .catch(this.showApiError);
   },
   methods: {
      OpenChange() {
         this.ChangeToPaidModal = true;
      },
      GetContract() {
         this.show = true;
         MemshipContractService.Get(this.$route.params.id)
            .then((res) => {
               this.Contract = res.data;
               this.show = false;
            })
            .catch((error) => {
               this.makeToast(error.response.data, 'danger');
            });
      },
      UploadFile(event) {
         const formData = new FormData();
         formContract.append('files', event.target.files[0]);
         this.fileLoading = true;
         MemshipContractService.UploadFile(formData).then((res) => {
            this.filter.files.push(res.data[0]);
            this.fileLoading = false;
         });
      },
      FileDelete(id) {
         MemshipContractService.DeleteFile(id).then((res) => {
            this.filter.files = this.filter.files.filter((item) => item.id != id);
         });
      },
      FileDownload(item) {
         MemshipContractService.DownloadFile(item.id).then((res) => {
            this.forceFileDownload(res, item.fileName, '');
         });
      },
      loginESP(item) {
         const isPinfl = this.isPinfl(item);
         this.Sign(item.key, isPinfl);
      },
      ClearFilter() {
         this.filter = {
            signedData: '',
            message: '',
            id: this.Contract.id,
            prtnRejectReasonId: 0,
            files: []
         };
      },

      ChangeToPaid() {
         MemshipContractService.ChangeContractorToPaid({
            id: this.Contract.id,
            baseFixedMinimumValue: this.selectedItem.baseFixedMinimumValue,
            docNumber: this.selectedItem.docNumber,
            details: this.selectedItem.details,
            organizationSettlementAccountId: this.selectedItem.organizationSettlementAccountId,
            regionalOrganizationId: this.selectedItem.regionalOrganizationId
         })
            .then(() => {
               this.makeToast(this.$t('ChangeToPaidSuccess'), 'success');
               this.GetContract();
               this.ChangeToPaidModal = false;
               this.GetContract();
            })
            .catch(this.SwalError);
      },
      async Reject(item) {
         try {
            this.$swal.fire({
               icon: 'question',
               title: this.$t('WantReject'),
               showLoaderOnConfirm: true,
               html: `
            <div style=" display: flex; flex-direction: column">
            <input id="reject-message" style="margin:0"  class="swal2-input" placeholder="${this.$t('RejectMessage')}">

             <div style="margin-top: 10px; display: flex; align-items: center">
               <input
                  type="file"
                  id="file-upload"
                  style="width: 100%; padding: 5px"
                  class="swal2-file"
                  placeholder="${this.$t('Fayl tanlang')}"
                  accept=".pdf, .doc, .docx"
               />
               <button id="btnClear"   style="outline: none; border: none; background-color: red; color: white; font-weight: 800;padding:11px;margin-top:15px">
                  X
               </button>
          </div>
        `,
               preConfirm: async (msg) => {
                  const message = document.getElementById('reject-message').value;

                  const fileInput = document.getElementById('file-upload');

                  const file = fileInput.files[0];
                  let uploadFile = [];

                  if (file) {
                     const formData = new FormData();
                     formData.append('files', file);
                     await MemshipContractService.UploadFile(formData)
                        .then((res) => {
                           uploadFile = res.data;
                        })
                        .catch((err) => {
                           console.log(err);
                        });
                  }

                  const sendData = {
                     id: item.id,
                     rejectDate: new Date(),
                     rejectMessage: message,
                     files: uploadFile
                  };

                  return MemshipContractService.Reject(sendData)
                     .then(() => {
                        this.makeToast(this.$t('RejectSuccess'), 'success');
                        this.Refresh();
                     })
                     .catch(this.SwalError);
               },
               allowOutsideClick: () => !this.$swal.isLoading(),
               didOpen: () => {
                  // Button click event to clear file input
                  const clearButton = document.getElementById('btnClear');

                  const fileInput = document.getElementById('file-upload');
                  if (clearButton && fileInput) {
                     clearButton.addEventListener('click', () => {
                        document.getElementById('file-upload').value = '';
                     });
                  }
               }
            });
         } catch (error) {
            showApiError(error);
         }
      },
      OpenSign() {
         this.EImzoModal = true;
         this.ClearFilter();
      },
      Sign(key, isPinfl) {
         this.SignLoading = true;
         this.filter.signedData = key;
         this.filter.isPinfl = isPinfl;
         MemshipContractService.Sign(this.filter)
            .then(() => {
               this.makeToast(this.$t('SignMessage'), 'success');
               this.SignLoading = false;
               this.SignModal = false;
               this.EImzoModal = false;
               this.GetContract();
            })
            .catch((error) => {
               this.showApiError(error);
            })
            .finally(() => {
               this.SignLoading = false;
            });
      },
      SaveData() {
         MemshipContractService.Update(this.Contract)
            .then(() => {
               this.makeToast(this.$t('SaveSuccess'), 'success');

               this.$router.push({ name: 'MemshipContract' });
            })
            .catch((err) => {
               this.showApiError(err);
            });
      },
      confirmUpdate() {
         this.canConfirmLoading = true;
         MemshipContractService.Confirm({ id: this.Contract.id })
            .then(() => {
               this.makeToast(this.$t('SaveSuccess'), 'success');
               this.GetContract();
            })
            .catch((err) => {
               this.showApiError(err);
            })
            .finally(() => {
               this.canConfirmLoading = false;
            });
      },

      DownloadTemplate() {
         MemshipContractService.DownloadTemplate(this.Contract.id2).then((res) => {
            this.forceFileDownload(res, this.$t('MemshipContract'), '.docx');
         });
      },
      MemshipContractUpload(event) {
         const formData = new FormData();
         formData.append('files', event.target.files[0]);
         this.fileLoading = true;
         MemshipContractService.MemshipContractUpload(formData).then((res) => {
            console.log(res);
            this.Contract.files.push(res.data[0]);
            this.fileLoading = false;
            this.SaveData();
            this.page = true;
         });
      }
   }
};
</script>
