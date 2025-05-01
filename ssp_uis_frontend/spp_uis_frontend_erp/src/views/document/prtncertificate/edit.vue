<template>
   <b-overlay :show="show">
      <b-row>
         <b-col sm="12" md="12" lg="12">
            <b-row class="justify-content-end">
               <b-col sm="12" md="3" lg="3" class="text-center">
                  <b-card v-if="SoliqData.company.tin">
                     <b-row>
                        <b-col class="text-left" sm="12" md="12" lg="12">
                           <p class="mb-0 pb-0">
                              <span>{{ SoliqData.company.tin }}</span>
                              - {{ SoliqData.company.name }}
                           </p>
                        </b-col>
                        <b-col class="text-left" sm="12" md="12" lg="12">
                           <p class="mb-0 pb-0">
                              <span>
                                 <b>{{ $t('registrationNumber') }}</b>
                              </span>
                              - {{ SoliqData.company.registrationNumber }}
                           </p>
                        </b-col>
                        <b-col class="text-left" sm="12" md="12" lg="12">
                           <p class="mb-0 pb-0">
                              <span>
                                 <b>{{ $t('registrationDate') }}</b>
                              </span>
                              - {{ SoliqData.company.registrationDate }}
                           </p>
                        </b-col>
                        <b-col class="text-left" sm="12" md="12" lg="12">
                           <p class="mb-0 pb-0">
                              <span>
                                 <b>{{ $t('director') }}</b>
                              </span>
                              - {{ SoliqData.director.lastName }} {{ SoliqData.director.firstName }}
                              {{ SoliqData.director.middleName }}
                           </p>
                        </b-col>
                        <b-col class="text-left" sm="12" md="12" lg="12">
                           <p class="mb-0 pb-0">
                              <span>
                                 <b>{{ $t('avgNumberEmployees') }}</b>
                              </span>
                              - {{ SoliqData.companyExtraInfo.avgNumberEmployees }}
                           </p>
                        </b-col>
                     </b-row>
                  </b-card>
               </b-col>
               <b-col sm="12" md="6" lg="6" class="text-center">
                  <b-card>
                     <div style="max-height: 90vh; overflow-y: auto">
                        <div v-if="Data.id == 0" style="min-height: 80vh" v-html="HtmlData" class="pl-2 pr-3"></div>
                        <iframe
                           v-if="Data.id != 0 && Data.id2"
                           style="width: 100%; height: 80vh"
                           :src="
                              axios.defaults.baseURL +
                              `PrtnCertificate/PrintCertificatePdf?Id2=${Data.id2}&lang=${getPdfLang()}`
                           "
                           frameborder="0"
                        ></iframe>
                     </div>
                  </b-card>
               </b-col>
               <b-col sm="6" md="3" lg="3" class="text-center">
                  <div class>
                     <b-button
                        v-if="Data.canCancel"
                        class="mr-2 mb-1"
                        @click="Cancel"
                        style="width: 100%"
                        size="xl"
                        variant="danger"
                     >
                        <feather-icon icon="XCircleIcon"></feather-icon>
                        {{ $t('Cancel') }}
                     </b-button>

                     <b-button
                        v-if="Data.id == 0"
                        class="mr-2 mt-1"
                        @click="SaveData"
                        style="width: 100%"
                        size="xl"
                        variant="success"
                     >
                        <feather-icon icon="CheckCircleIcon"></feather-icon>
                        {{ $t('create') }}
                     </b-button>
                     <b-button
                        v-if="Data.id == 0"
                        class="mr-2 mt-1"
                        @click="GoBack"
                        style="width: 100%"
                        size="xl"
                        variant="danger"
                     >
                        <feather-icon icon="ArrowLeftIcon"></feather-icon>
                        {{ $t('goBack') }}
                     </b-button>
                  </div>
               </b-col>
            </b-row>
         </b-col>
      </b-row>

      <!-- sign dialog -->
      <b-modal v-model="EImzoModal" size="lg" :title="$t('enterEImzo')" hide-footer no-enforce-focus>
         <b-card-text v-if="Data">
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
   BSpinner
} from 'bootstrap-vue';
import Ripple from 'vue-ripple-directive';
import axios from 'axios';
import flatPickr from 'vue-flatpickr-component';
import Cleave from 'vue-cleave-component';
import PrtnCertificateService from '@/services/document/prtncertificate.service';
const justSign = () => import('@/components/justSign.vue');
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
      BSpinner,
      justSign
   },
   directives: {
      'b-tooltip': VBTooltip,
      'b-modal': VBModal
   },
   data() {
      return {
         axios,
         HtmlData: {},
         EImzoModal: false,
         SoliqData: {
            company: {
               tin: ''
            },
            director: {},
            companyExtraInfo: {}
         },
         DavAktivData: {},
         show: false,
         Data: {},
         loading: false,
         SignLoading: false,
         CancelMessage: '',
         cancelApplication: true,
         cancelContract: true
      };
   },
   created() {
      this.show = true;

      if (this.$route.query.isList) {
         PrtnCertificateService.GetByPrtnContractId(this.$route.params.id)
            .then((res) => {
               this.show = false;
               this.Data = res.data;

               PrtnCertificateService.GetFromSoliq(res.data.contractorInn)
                  .then((res1) => {
                     this.SoliqData = res1.data;
                  })
                  .catch((err) => {
                     this.makeToast(this.$t(err), 'danger');
                  });
               PrtnCertificateService.GetFromDavAkiv(res.data.contractorInn)
                  .then((res1) => {
                     this.DavAktivData = res1.data;
                  })
                  .catch((err) => {
                     this.makeToast(this.$t(err), 'danger');
                  });
               this.getHtml(this.Data);
            })
            .catch((err) => {
               this.makeToast(this.$t(err), 'danger');
            });
      } else {
         PrtnCertificateService.Get(this.$route.params.id)
            .then((res) => {
               this.show = false;
               this.Data = res.data;
               PrtnCertificateService.GetFromSoliq(res.data.contractorInn)
                  .then((res1) => {
                     this.SoliqData = res1.data;
                  })
                  .catch((err) => {
                     this.makeToast(this.$t(err), 'danger');
                  });
               PrtnCertificateService.GetFromDavAkiv(res.data.contractorInn)
                  .then((res1) => {
                     this.DavAktivData = res1.data;
                  })
                  .catch((err) => {
                     this.makeToast(this.$t(err), 'danger');
                  });
            })
            .catch((err) => {
               this.makeToast(this.$t(err), 'danger');
            });
      }
   },
   directives: {
      Ripple
   },
   methods: {
      loginESP(item) {
         this.SignLoading = true;
         return PrtnCertificateService.Cancel({
            id: this.Data.id,
            message: this.CancelMessage,
            signedData: item.key,
            cancelContract: this.cancelContract,
            cancelApplication: this.cancelApplication
         })
            .then(() => {
               this.makeToast(this.$t('CancelMessage'), 'success');
               this.EImzoModal = false;
               this.$router.push({ name: 'PrtnCertificate' });
            })
            .catch(this.showApiError)
            .finally(() => {
               this.SignLoading = false;
            });
      },
      Cancel() {
         this.$swal.fire({
            icon: 'question',
            title: this.$t('WantCancel'),
            showLoaderOnConfirm: true,
            html: `
             <div class='d-flex'>
               <label class="swal2-checkbox" style="display: flex;">
                  <input type="checkbox" id="contract-checkbox" value="0">
                  <span class="swal2-label">Shartnomani bekor qilish</span>
              </label>
              <label class="swal2-checkbox" style="display: flex;">
                  <input type="checkbox" id="application-checkbox" value="0">
                  <span class="swal2-label">Arizani bekor qilish</span>
              </label>
             </div>
               <input id="message-input1" class="swal2-input" placeholder='${this.$t('RejectMessage')}'>
            `,
            preConfirm: () => {
               const msg = document.getElementById('message-input1').value;
               this.cancelContract = document.getElementById('contract-checkbox').checked;
               this.cancelApplication = document.getElementById('application-checkbox').checked;
               if (this.cancelApplication && !this.cancelContract) {
                  this.SwalError("Faqat Arizani o'zini bekor qila olmaysiz!");
               } else {
                  if (msg) {
                     this.CancelMessage = msg;
                     this.EImzoModal = true;
                  } else {
                     this.SwalError('Iltimos bekor qilish sababini kiriting!');
                  }
               }
            },
            allowOutsideClick: () => !this.$swal.isLoading()
         });
      },
      Print(id) {
         PrtnCertificateService.PrintCertificatePdf(id).then((res) => {
            this.forceFileDownload(res, this.$t('prtncertificate'));
         });
      },
      forceFileDownload(response, name) {
         var { headers } = response;
         var blob = new Blob([response.data]);
         const url = window.URL.createObjectURL(blob);
         const link = document.createElement('a');
         link.href = url;
         link.setAttribute('download', name + '.pdf'); //or any other extension
         document.body.appendChild(link);
         link.click();
      },
      getHtml(data) {
         PrtnCertificateService.GetHtmlTemplate(data)
            .then((res) => {
               this.HtmlData = res.data;
            })
            .catch((err) => {
               this.makeToast(this.$t(err), 'danger');
            });
      },
      GoBack() {
         this.$router.push({ name: 'PrtnCertificate' });
      },
      SaveData() {
         PrtnCertificateService.Create(this.Data)
            .then((res) => {
               this.makeToast(this.$t('SaveSuccess'), 'success');
               // this.$router.push({ name: "PrtnCertificate" });
               this.$router.go(-1);
            })
            .catch((err) => {
               this.makeToast(this.$t(err), 'danger');
            });
      }
   },
   watch: {},
   computed: {}
};
</script>
