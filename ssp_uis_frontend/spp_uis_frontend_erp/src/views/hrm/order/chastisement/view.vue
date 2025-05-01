<template>
   <b-overlay :show="show">
      <b-row class="justify-content-center">
         <b-col cols="9">
            <b-card>
               <div class="d-flex justify-content-around mb-2">
                  <div>
                     <span style="font-weight: bold">{{ $t('docnumber') }}</span>
                     : {{ Data.docNumber }}
                  </div>
                  <div>
                     <span style="font-weight: bold">{{ $t('docdate') }}</span>
                     : {{ Data.docOn }}
                  </div>
               </div>
               <div>
                  <span style="font-weight: bold">{{ $t('orderDetails') }}</span>
                  : {{ Data.details }}
               </div>
               <b-row class="mt-2">
                  <b-col>
                     <b-table
                        :fields="TablesField"
                        bordered
                        small
                        responsive
                        :items="Data.tables"
                        hover
                        show-empty
                        :empty-text="$t('NotFound')"
                     >
                        <template #cell(isProbation)="{ item }">
                           <feather-icon icon="CheckCircleIcon" v-if="item.isProbation" class="text-success" />
                           <feather-icon icon="XCircleIcon" v-else class="text-danger" />
                        </template>
                        <template #cell(interm)="{ item }">
                           <feather-icon icon="CheckCircleIcon" v-if="item.interm" class="text-success" />
                           <feather-icon icon="XCircleIcon" v-else class="text-danger" />
                        </template>
                        <template #cell(acting)="{ item }">
                           <feather-icon icon="CheckCircleIcon" v-if="item.acting" class="text-success" />
                           <feather-icon icon="XCircleIcon" v-else class="text-danger" />
                        </template>
                        <template #cell(position)="{ item }">
                           {{ item.position || item.fromPosition }}
                        </template>
                        <template #cell(department)="{ item }">
                           {{ item.department || item.fromDepartment }}
                        </template>
                     </b-table>
                  </b-col>
               </b-row>
               <div class="d-flex justify-content-between">
                  <div v-if="signerDirector">
                     <span style="font-weight: bold">{{ $t('Principal signatory') }}</span
                     >:
                     {{ signerDirector.employee }}
                  </div>

                  <div v-if="signerHr">
                     <span style="font-weight: bold">{{ $t('Entered') }}</span
                     >: {{ signerHr.employee }}
                  </div>
               </div>

               <div class="mt-2">
                  <HrmSignerTableView :signer="Data.signer" />
               </div>

               <WIframe v-if="Data && Data.id2" :src="IframeSrc" style="height: 100vh" :show="Data && Data.id2" />
            </b-card>
         </b-col>
         <!-- v-if="Data.canSign || Data.canCancel || Data.signEmployee" -->
         <b-col>
            <b-button
               v-if="Data.canSign || Data.signEmployee"
               @click="EImzoModal = true"
               size="sm"
               variant="outline-success"
               class="w-100 mb-1"
            >
               <feather-icon icon="CheckIcon"></feather-icon>
               {{ $t('Sign') }}
            </b-button>
            <b-button v-if="Data.canCancel" @click="Cancel()" size="sm" variant="outline-warning" class="w-100 mb-1">
               <feather-icon icon="XIcon"></feather-icon>
               {{ $t('Cancel') }}
            </b-button>
            <template>
               <b-button class="mt-2" @click="DownloadTemplate" style="width: 100%" size="xl" variant="info">
                  <feather-icon icon="DownloadIcon"></feather-icon>
                  {{ $t('download') }}
               </b-button>
               <div class="mt-2">
                  <h6 class="inputTitle">{{ $t('fileupload') }}</h6>
                  <b-form-file type="file" :placeholder="$t('Faylni tanlang')" @change="ChastisementTemplateUpload">
                  </b-form-file>
               </div>
            </template>
         </b-col>
      </b-row>

      <!-- sign dialog -->
      <b-modal v-model="EImzoModal" size="lg" :title="$t('enterEImzo')" hide-footer>
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
// service
import ChastisementService from '@/services/hrm/chastisement.service';
// components
import {
   BOverlay,
   BCard,
   BCardText,
   BRow,
   BCol,
   BTable,
   BButton,
   BLink,
   BSpinner,
   BModal,
   BFormFile
} from 'bootstrap-vue';
import EmployeeManageSelect from '@/views/components/hrm/EmployeeManageSelect.vue';
import EmployeeSelect2 from '@/views/components/employee/EmployeeSelect2.vue';
import axios from 'axios';

const HrmSignerTableView = () => import('@/views/components/hrm/HrmSignerTableView.vue');
const justSign = () => import('@/components/justSign.vue');
const WIframe = () => import('@/components/WIframe.vue');

export default {
   components: {
      BOverlay,
      BCard,
      BCardText,
      BRow,
      BCol,
      BButton,
      BTable,
      BModal,
      BLink,
      EmployeeManageSelect,
      EmployeeSelect2,
      BFormFile,
      justSign,
      BSpinner,
      HrmSignerTableView,
      WIframe
   },
   data() {
      return {
         show: false,
         loadingButton: false,
         tabIndex: 1,
         EImzoModal: false,
         fileLoading: false,
         SignLoading: false,
         TablesField: [
            {
               key: 'department',
               label: this.$t('Department')
            },
            {
               key: 'employee',
               label: this.$t('employee'),
               sortable: true
            },
            {
               key: 'reason',
               label: this.$t('reason')
            },
            {
               key: 'fact',
               label: this.$t('fact')
            },
            {
               key: 'employeeRate',
               label: this.$t('employeeRate')
            },
            {
               key: 'details',
               label: this.$t('details')
            }
         ],
         Data: {
            id: null,
            docNumber: null,
            docOn: null,
            details: null,
            signer: []
         }
      };
   },
   async created() {
      this.Refresh();
   },
   computed: {
      signerHr() {
         return this.Data.signer.find((e) => e.isHr);
      },
      signerDirector() {
         return this.Data.signer.find((e) => e.isDirector);
      },
      IframeSrc() {
         return axios.defaults.baseURL + `hrm/Chastisement/DownloadPdf?id2=${this.Data.id2}&lang=${this.getPdfLang()}`;
      }
   },
   methods: {
      Refresh() {
         this.show = true;

         ChastisementService.Get(this.$route.params.id)
            .then((res) => {
               this.Data = res.data;
            })
            .catch((error) => {
               this.showApiError(error);
            })
            .finally(() => {
               this.show = false;
            });
      },
      loginESP(item) {
         this.Sign(item.key);
      },
      logout() {
         // Remove userData from localStorage
         Cookies.remove('auth_token');

         localStorage.clear();
         // Reset ability
         this.$ability.update(initialAbility);

         // Redirect to login page
         this.$router.push({ name: 'auth-login' });
      },
      Sign(signedData) {
         this.SignLoading = true;
         ChastisementService.Sign({
            id: this.Data.id,
            signedData: signedData
         })
            .then(() => {
               this.makeToast(this.$t('AcceptSuccess'), 'success');
               this.Refresh();
               this.logout();
               this.EImzoModal = false;
            })
            .catch((err) => {
               this.showApiError(err);
            })
            .finally(() => {
               this.SignLoading = false;
            });
      },
      Cancel() {
         this.$swal.fire({
            icon: 'question',
            title: this.$t('WantCancel'),
            showLoaderOnConfirm: true,
            input: 'text',
            inputPlaceholder: this.$t('RejectMessage'),
            preConfirm: (msg) => {
               return ChastisementService.Cancel({
                  id: this.Data.id,
                  message: msg,
                  signedData: 'cancel'
               })
                  .then(() => {
                     this.makeToast(this.$t('CancelMessage'), 'success');
                     this.Refresh();
                  })
                  .catch(this.SwalError);
            },
            allowOutsideClick: () => !this.$swal.isLoading()
         });
      },
      DownloadTemplate() {
         ChastisementService.DownloadTemplate().then((res) => {
            this.forceFileDownload(res, this.$t('Chastisement'), '.docx');
         });
      },
      ChastisementTemplateUpload(event) {
         const formData = new FormData();
         formData.append('files', event.target.files[0]);
         this.fileLoading = true;
         ChastisementService.ChastisementContractUpload(formData).then((res) => {
            this.Data.files.length = 0;
            this.Data.files.push(res.data[0]);
            ChastisementService.Update(this.Data).then((res) => {
               window.location.reload();
            });
            this.fileLoading = false;
         });
      }
   }
};
</script>
