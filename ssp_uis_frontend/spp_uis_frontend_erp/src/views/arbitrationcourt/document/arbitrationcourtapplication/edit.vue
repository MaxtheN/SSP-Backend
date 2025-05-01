<template>
   <b-overlay :show="show">
      <div v-if="$route.params.id">
         <div>
            <div class="flex-container container">
               <div
                  class="step"
                  v-for="(step, i) in steps"
                  :key="i"
                  :class="{
                     done: step.number < currentStep,
                     current: step.number === currentStep
                  }"
               >
                  <div class="step-number" :id="'step-' + step.number" @click="moveStep(step.number)">
                     <i v-if="step.number <= currentStep"><b-icon-check-lg scale="0.8"></b-icon-check-lg></i>
                  </div>
                  <div class="step-label">{{ step.label }}</div>
               </div>
            </div>
         </div>
      </div>
      <b-row class="mt-2">
         <b-col sm="9" md="9" lg="9">
            <b-card>
               <validation-observer ref="ValidationDTO">
                  <h3 class="text-center my-2">{{ $t('Hakamlik sudiga ariza') }}</h3>
                  <b-row>
                     <b-col sm="12" md="6">
                        <b-row class="flex-column align-content-center">
                           <h4 class="ml-1 my-2">{{ $t('Davogar') }}</h4>
                           <b-col sm="12" md="10">
                              <b-form-checkbox class="mb-1" v-model="Data.isForeignContractor">{{
                                 $t('isForeignContractor')
                              }}</b-form-checkbox>
                           </b-col>
                           <b-col sm="12" md="10" v-if="!Data.isForeignContractor">
                              <div class="form-group">
                                 <form-input
                                    v-model="Data.contractorInnPnfl"
                                    :label="$t('inn')"
                                    :mask="'#########'"
                                    :disabled="$route.params.id != 0"
                                 >
                                    <b-input-group-append>
                                       <b-button
                                          variant="primary"
                                          @click="GetByContractorInn"
                                          :disabled="$route.params.id != 0"
                                       >
                                          <feather-icon icon="SearchIcon"></feather-icon>
                                       </b-button>
                                    </b-input-group-append>
                                 </form-input>
                              </div>
                           </b-col>
                           <b-col sm="12" md="10" v-else
                              ><form-input-hrm
                                 v-model="Data.foreignContractorInn"
                                 required-star
                                 rules="required"
                                 :label="$t('inn')"
                                 :placeholder="$t('inn')"
                           /></b-col>
                           <b-col sm="12" md="10">
                              <form-select
                                 required-star
                                 rules="required"
                                 :options="ResponsibleSelectList"
                                 v-model="Data.contractorResponsibleTypeId"
                                 :label="$t('contractorResponsibleTypeId')"
                              ></form-select>
                           </b-col>
                           <b-col sm="12" md="10">
                              <!-- :disabled="!Data.isForeignContractor" -->

                              <form-input-hrm
                                 v-if="Data.contractorId"
                                 v-model="Data.eCourtNumber"
                                 required-star
                                 rules="required"
                                 :label="$t('eCourtNumber')"
                                 :placeholder="$t('eCourtNumber')"
                           /></b-col>

                           <b-col sm="12" md="10" v-if="!Data.isForeignContractor">
                              <form-input-hrm
                                 v-model="Data.application.contractor"
                                 required-star
                                 disabled
                                 rules="required"
                                 :label="$t('contractorName')"
                                 :placeholder="$t('contractorName')"
                              />
                           </b-col>
                           <b-col sm="12" md="10" v-else>
                              <form-input-hrm
                                 v-model="Data.foreignContractorName"
                                 required-star
                                 rules="required"
                                 :label="$t('contractorName')"
                                 :placeholder="$t('contractorName')"
                              />
                           </b-col>

                           <b-col sm="12" md="10">
                              <form-input-hrm
                                 :disabled="!Data.isForeignContractor"
                                 v-model="Data.contractorAddress"
                                 required-star
                                 rules="required"
                                 :label="$t('contractorAddress')"
                                 :placeholder="$t('contractorAddress')"
                              />
                           </b-col>
                           <b-col sm="12" md="10">
                              <form-input-hrm
                                 v-model="Data.contractorPhonber"
                                 rules="required|validatorPhone"
                                 mask="(998) ## ### ## ##"
                                 :label="$t('phoneNumber')"
                                 placeholder="(998) ## ### ## ##"
                              />
                           </b-col>

                           <b-col sm="12" md="10">
                              <b-row>
                                 <b-col>
                                    <form-picker
                                       rules="required"
                                       v-model="Data.application.docOn"
                                       :label="$t('docOn')"
                                       :placeholder="$t('docOn')"
                                    />
                                 </b-col>
                                 <b-col>
                                    <form-input-hrm
                                       v-model="Data.application.docNumber"
                                       :label="$t('docNumber')"
                                       :placeholder="$t('docNumber')"
                                    />
                                 </b-col>
                              </b-row>
                           </b-col>
                           <b-col sm="12" md="10">
                              <form-select
                                 :options="AplicationTypeList"
                                 v-model="Data.arbitrationApplicationTypeId"
                                 :label="$t('arbitrationApplicationTypeId')"
                                 required-star
                                 rules="required"
                              ></form-select>
                           </b-col>
                           <b-col sm="12" md="10">
                              <form-select
                                 :options="organizarionList"
                                 v-model="Data.organizationId"
                                 required-star
                                 :label="$t('arbitrationRegion')"
                              ></form-select>
                           </b-col>

                           <b-col sm="12" md="10" class="mb-2">
                              <h6 class="inputTitle">{{ $t('fileupload') }}</h6>
                              <b-form-file type="file" :placeholder="$t('Faylni tanlang')" @change="UploadFile">
                              </b-form-file>
                              <ul
                                 v-for="(file, index) in Data.files"
                                 :key="index"
                                 style="padding: 0; margin: 0; padding-top: 5px"
                              >
                                 <li style="list-style: none; display: flex" v-if="file.isCreatedErp">
                                    <b-form-checkbox
                                       v-if="Data.canUploadSignFile"
                                       v-model="file.canSign"
                                       @change="CheckFile(file)"
                                    ></b-form-checkbox>
                                    <b-badge variant="info"
                                       >{{ file.fileName }}
                                       <b-link style="margin-left: 5px; margin-right: 4px" @click="DownloadFile(file)">
                                          <b-spinner v-if="DownloadLoading" small></b-spinner>
                                          <feather-icon
                                             size="18"
                                             style="color: black"
                                             v-if="!DownloadLoading"
                                             icon="DownloadIcon"
                                          >
                                          </feather-icon>
                                       </b-link>
                                       <b-link @click="DeleteFile(file, index)">
                                          <b-spinner v-if="DeleteLoading" small></b-spinner>
                                          <feather-icon
                                             size="18"
                                             style="color: black"
                                             v-if="!DeleteLoading"
                                             icon="Trash2Icon"
                                          >
                                          </feather-icon> </b-link
                                    ></b-badge>
                                 </li>
                                 <li style="list-style: none; display: flex" v-else>
                                    <b-form-checkbox
                                       v-if="Data.canUploadSignFile"
                                       v-model="file.canSign"
                                       @change="CheckFile(file)"
                                    ></b-form-checkbox>
                                    <b-badge variant="success"
                                       >{{ file.fileName }}
                                       <b-link style="margin-left: 5px; margin-right: 4px" @click="DownloadFile(file)">
                                          <b-spinner v-if="DownloadLoading" small></b-spinner>
                                          <feather-icon
                                             size="18"
                                             style="color: black"
                                             v-if="!DownloadLoading"
                                             icon="DownloadIcon"
                                          >
                                          </feather-icon>
                                       </b-link>
                                       <b-link @click="DeleteFile(file, index)">
                                          <b-spinner v-if="DeleteLoading" small></b-spinner>
                                          <feather-icon
                                             size="18"
                                             style="color: black"
                                             v-if="!DeleteLoading"
                                             icon="Trash2Icon"
                                          >
                                          </feather-icon> </b-link
                                    ></b-badge>
                                 </li>
                              </ul>
                           </b-col>
                        </b-row>
                     </b-col>
                     <b-col sm="12" md="6" class="">
                        <h4 class="ml-1 my-2">{{ $t('responsible') }}</h4>
                        <b-row>
                           <b-col sm="12" md="10">
                              <b-form-checkbox class="mb-1" v-model="Data.isForeignResponsible">{{
                                 $t('isForeignResponsible')
                              }}</b-form-checkbox>
                           </b-col>

                           <b-col sm="12" md="10" v-if="!Data.isForeignResponsible">
                              <div class="form-group">
                                 <form-input
                                    v-model="Data.responsibleInn"
                                    :label="$t('inn')"
                                    :mask="'#########'"
                                    :disabled="$route.params.id != 0"
                                 >
                                    <b-input-group-append>
                                       <b-button
                                          variant="primary"
                                          @click="GetByresponsibleInn"
                                          :disabled="$route.params.id != 0"
                                       >
                                          <feather-icon icon="SearchIcon"></feather-icon>
                                       </b-button>
                                    </b-input-group-append>
                                 </form-input>
                              </div>
                           </b-col>
                           <b-col sm="12" md="10" v-else>
                              <form-input-hrm
                                 v-model="Data.foreignResponsibleInn"
                                 rules="required"
                                 :label="$t('inn')"
                                 :placeholder="$t('inn')"
                              />
                           </b-col>
                           <b-col sm="12" md="10">
                              <form-select
                                 required-star
                                 rules="required"
                                 :options="ResponsibleSelectList"
                                 v-model="Data.responsibleTypeId"
                                 :label="$t('responsibleTypeId')"
                              ></form-select>
                           </b-col>

                           <b-col sm="12" md="10" v-if="!Data.isForeignResponsible">
                              <form-input-hrm
                                 v-model="Data.responsible"
                                 rules="required"
                                 disabled
                                 :label="$t('responsible')"
                                 :placeholder="$t('responsible')"
                              />
                           </b-col>

                           <b-col sm="12" md="10" v-else>
                              <form-input-hrm
                                 v-model="Data.foreignResponsibleName"
                                 :label="$t('responsible')"
                                 :placeholder="$t('responsible')"
                                 required-star
                                 rules="required"
                              />
                           </b-col>
                           <b-col sm="12" md="10">
                              <form-input-hrm
                                 v-model="Data.responsibleAddress"
                                 :disabled="!Data.isForeignResponsible"
                                 :label="$t('responsibleAddress')"
                                 :placeholder="$t('responsibleAddress')"
                                 required-star
                                 rules="required"
                              />
                           </b-col>
                           <b-col sm="12" md="10">
                              <form-input-hrm
                                 v-model="Data.responsiblePhonber"
                                 rules="required|validatorPhone"
                                 mask="(998) ## ### ## ##"
                                 :label="$t('phoneNumber')"
                                 placeholder="(998) ## ### ## ##"
                              />
                           </b-col>
                           <b-col cols="12" md="10">
                              <form-select
                                 v-if="Data.canSetJudge"
                                 multiple
                                 :options="JudgeList"
                                 v-model="arbitrationJudgeIds"
                                 :disabled="Data.application.currentStep.id > 2"
                                 required-star
                                 :label="$t('judge')"
                              ></form-select>
                           </b-col>

                           <b-col sm="12" md="5">
                              <form-input-hrm
                                 required-star
                                 rules="required"
                                 v-model="Data.arbitrationAmount"
                                 :label="$t('arbitrationAmount')"
                                 :placeholder="$t('arbitrationAmount')"
                              />
                           </b-col>

                           <b-col sm="12" md="5" class="mt-2">
                              <b-form-checkbox class="mb-1" v-model="Data.canByDivided">{{
                                 $t('canByDivided')
                              }}</b-form-checkbox>
                           </b-col>
                           <b-col sm="12" md="5">
                              <form-input-hrm v-model="Data.amount" :label="$t('amount')" :placeholder="$t('amount')" />
                           </b-col>
                           <b-col sm="12" md="5" v-if="Data.application.currentStep.id > 3">
                              <form-picker
                                 :disabled="Data.application.currentStep.id > 3"
                                 v-model="Data.discussionDate"
                                 required
                                 :label="$t('discussionDate')"
                                 :placeholder="$t('discussionDate')"
                              />
                           </b-col>
                           <b-col cols="12" md="5">
                              <form-select
                                 :options="CurrencySelectList"
                                 v-model="Data.currencyId"
                                 :label="$t('currencyId')"
                                 text-field="text"
                                 required-star
                                 rules="required"
                                 value-field="value"
                              ></form-select>
                           </b-col>
                           <b-col sm="12" md="10">
                              <p v-show="this.$route.params.id" style="font-weight: 600" class="mt-2">
                                 {{ $t('CreatedUser') }} : {{ Data.createdUser }}
                              </p>
                           </b-col>
                           <b-col sm="12" md="10" v-if="Data.canUploadSignFile">
                              <b-button @click="Print" variant="outline-info">
                                 <feather-icon icon="FileIcon"></feather-icon>
                                 {{ $t('Print') }}
                              </b-button>
                           </b-col>
                        </b-row>
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
            <b-button v-if="Data.canChangeStep" @click="ChangeStep" style="width: 100%" class="mt-1" variant="info">
               <feather-icon icon="ArrowRightCircleIcon"></feather-icon>
               {{ steps[Data.application.currentStep.id].label }}
            </b-button>
            <b-button
               v-if="Data.canCreateDiscussionDoc"
               @click="GoDiscussion"
               style="width: 100%"
               class="mt-1"
               variant="info"
            >
               <feather-icon icon="ArrowRightCircleIcon"></feather-icon>
               {{ $t('step3') }}
            </b-button>
            <b-button v-if="Data.canCreateDelayDoc" @click="GoDelay" style="width: 100%" class="mt-1" variant="warning">
               <feather-icon icon="ArrowRightCircleIcon"></feather-icon>
               {{ $t('step4') }}
            </b-button>
            <b-button v-if="Data.canCreateResultDoc" @click="GoResult" style="width: 100%" class="mt-1" variant="info">
               <feather-icon icon="ArrowRightCircleIcon"></feather-icon>
               {{ $t('step5') }}
            </b-button>
         </b-col>
      </b-row>
   </b-overlay>
</template>

<script>
import ManualService from '@/services/others/manual.service';
import CurrencyService from '@/services/others/currency.service';
import ArbitrationJudgeService from '@/services/arbitrationjudge/arbitrationjudge.service';
import ContractorListSelect from '@/views/components/info/ContractorListSelect.vue';
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
   BFormCheckbox
} from 'bootstrap-vue';
import ArbitrationCourtService from '@/services/arbitrationcourt/arbitrationcourt.service.js';
import ContractorService from '@/services/info/contractor.service';
export default {
   components: {
      BOverlay,
      BInputGroupAppend,
      BCard,
      BRow,
      BCol,
      BButton,
      ContractorListSelect,
      BFormFile,
      BIconCheckLg,
      BBadge,
      BSpinner,
      BLink,
      BFormCheckbox
   },
   data() {
      return {
         saveLoading: false,
         show: false,
         organizarionList: [],
         innLoading: false,
         ResponsibleSelectList: [],
         CurrencySelectList: [],
         DepartmentList: [],
         AplicationTypeList: [],
         arbitrationJudgeIds: [],
         JudgeList: [],
         ArbitrationCourtList: [],
         steps: [
            { label: this.$t('step1') },
            { label: this.$t('step2') },
            { label: this.$t('step3') },
            { label: this.$t('step4') },
            { label: this.$t('step5') }
         ],
         pBarSize: '',
         DeleteLoading: false,
         DownloadLoading: false,
         currentStep: 1,
         Data: {
            // responsibleInnPnfl: '',
            eCourtNumber: '',
            contractorResponsibleTypeId: null,
            contractorPhonber: '',
            contractorId: null,
            contractorInn: '',
            contractorAddress: '',
            fileLoading: false,
            files: [],
            responsiblePhonber: '',
            responsibleTypeId: null,
            responsibleAddress: '',
            responsible: '',
            responsibleContractorId: null,
            responsibleTypeId: null,
            currencyId: null,
            amount: null,
            arbitrationAmount: null,
            arbitrationApplicationTypeId: null,

            application: {
               docOn: '',
               docNumber: null,
               applicationTypeId: 8,
               contractorPositionName: '',
               contractor: ''
            }
         }
      };
   },
   computed: {
      // eslint-disable-next-line vue/return-in-computed-property
      progress() {
         if (this.$route.query.isProcess) {
            if (this.currentStep > this.steps.length) {
               return `width: 100%`;
            }
            const first = document.getElementById('step-1');
            const current = document.getElementById(`step-${this.currentStep}`);
            if (first && current) {
               const delta = current.getBoundingClientRect().right - first.getBoundingClientRect().right;
               return `width: ${delta}px;`;
            }
         }
      }
   },
   created() {
      ManualService.OrganizationAsSelectListByGroup([1, 3]).then((res) => {
         this.organizarionList = res.data;
      });
      ArbitrationCourtService.Get(this.$route.params.id)
         .then((res) => {
            this.Data = res.data;
            this.currentStep = this.Data.application.currentStep.id;
            if (res.data.signer.length != 0) {
               this.arbitrationJudgeIds = res.data.signer?.map((item) => item.arbitrationJudgeId);
            }
         })
         .catch((error) => {
            this.makeToast(error.response.data.errors, 'danger');
         })
         .finally(() => {
            this.show = false;
         });
      ArbitrationJudgeService.GetAsSelectList().then((res) => {
         this.JudgeList = res.data;
      });
      CurrencyService.GetAsSelectList(1)
         .then((res) => {
            this.CurrencySelectList = res.data;
         })
         .catch((errors) => {
            this.makeToast(errors.response.data.errors);
         })
         .finally(() => {
            this.show = false;
         });

      ManualService.ClaimResponsibleTypeSelectList()
         .then((res) => {
            this.ResponsibleSelectList = res.data;
         })
         .catch((errors) => {
            this.makeToast(errors.response.data.errors, 'danger');
         });

      ManualService.ArbitrationApplicationTypeSelectList()
         .then((res) => {
            this.AplicationTypeList = res.data;
         })
         .catch((errors) => {
            this.makeToast(errors.response.data.errors, 'danger');
         });
      ManualService.ArbitrationCourtSelectList()
         .then((res) => {
            this.ArbitrationCourtList = res.data;
         })
         .catch((errors) => {
            this.makeToast(errors.response.data.errors, 'danger');
         });
   },
   mounted() {
      if (!this.steps || this.steps.length == 0) {
         return;
      }

      this.steps = this.steps.map((s, i) => ({
         number: i + 1,
         selected: false,
         ...s
      }));

      this.steps[0].selected = true;

      this.$nextTick(() => {
         this.calculateBarPosition();
      });

      window.addEventListener('resize', this.calculateBarPosition);
   },
   beforeDestroy() {
      window.removeEventListener('resize', this.calculateBarPosition);
   },
   methods: {
      GoDiscussion() {
         this.$router.push({
            name: 'EditArbitrationDiscussion',
            query: {
               courtId: this.Data.id
            }
         });
      },
      GoResult() {
         this.$router.push({
            name: 'EditArbitrationResult',
            query: {
               courtId: this.Data.id,
               canByDivided: this.Data.canByDivided
            }
         });
      },
      GoDelay() {
         this.$router.push({
            name: 'EditArbitrationDelay',
            query: {
               courtId: this.Data.id
            }
         });
      },
      async ChangeStep() {
         this.$refs.ValidationDTO.validate().then(async (success) => {
            if (success) {
               try {
                  this.arbitrationJudgeIds.forEach((item, index) => {
                     return this.Data.signer.push({ arbitrationJudgeId: item });
                  });
                  await ArbitrationCourtService.Update(this.Data)
                     .then(() => {
                        this.makeToast(this.$t('SaveSuccess'), 'success');
                        this.arbitrationJudgeIds = [];
                     })
                     .catch((err) => {
                        this.showApiError(err);
                     })
                     .finally(() => {
                        this.saveLoading = false;
                     });
                  ArbitrationCourtService.NextStep(this.Data.id)
                     .then(() => {
                        this.makeToast(this.$t('ChangeStepSuccess'), 'success');
                        ArbitrationCourtService.Get(this.$route.params.id)
                           .then((res) => {
                              this.Data = res.data;
                              this.currentStep = this.Data.application.currentStep.id;
                              if (res.data.signer.length != 0) {
                                 this.arbitrationJudgeIds = res.data.signer.map((item) => item.arbitrationJudgeId);
                              }
                           })
                           .catch((error) => {
                              this.makeToast(error.response.data.errors, 'danger');
                           })
                           .finally(() => {
                              this.show = false;
                           });
                     })
                     .catch((error) => {
                        this.showApiError(error);
                     })
                     .finally(() => {
                        this.deleteLoading = false;
                     });
               } catch (e) {
                  this.showApiError(e);
               }
            }
         });
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
      moveStep(stepNumber) {
         if (stepNumber <= this.Data.application.currentStep.id) {
            this.currentStep = stepNumber;
         }
      },
      calculateBarPosition() {
         if (this.$route.query.isProcess) {
            const docEl = document.documentElement;
            const first = document.getElementById('step-1');
            const rect = first.getBoundingClientRect();
            const offset = rect.left + (window.scrollX || docEl.scrollLeft || 0);
            const top = rect.top + rect.height / 2 - 2;
            // this.pBarSize = `left: ${offset}px; right: ${offset}px; top: ${top}px`;
            this.pBarSize = `left: ${0}px; right: ${0}px;`;
         }
      },
      UploadFile(event) {
         const formData = new FormData();
         formData.append('files', event.target.files[0]);
         this.fileLoading = true;

         ArbitrationCourtService.UploadFile(formData).then((res) => {
            const updatedFiles = res.data.map((file) => ({
               ...file,
               stepId: `${this.Data.application.currentStep.id ? this.Data.application.currentStep.id : 1}`
            }));

            this.Data.files.push(...updatedFiles);

            this.fileLoading = false;
         });
      },
      DeleteFile(file, index) {
         this.DownloadLoading = true;
         ArbitrationCourtService.DeleteFile(file.id).then((_res) => {
            this.DownloadLoading = false;
            this.Data.files.splice(index, 1);
         });
      },
      Print(file) {
         ArbitrationCourtService.DownloadTemplate(this.getPdfLang()).then((res) => {
            this.forceFileDownload(res, 'sign-template', '.docx');
         });
      },
      DownloadFile(file) {
         ArbitrationCourtService.DownloadFile(file.id).then((res) => {
            this.forceFileDownload(res, file.fileName, file.fileExtension);
         });
      },

      async GetByContractorInn() {
         try {
            this.innLoading = true;
            const res = await ContractorService.SearchByInnPnfl(this.Data.contractorInnPnfl);
            console.log(res);
            this.Data.contractorInn = res.data.inn;
            this.Data.contractorAddress = res.data.address;
            this.Data.application.contractor = res.data.fullName;
            this.Data.contractorId = res.data.id;
            this.Data.contractorPositionName = res.data.fullName;

            if (!+this.$route.params.id) {
               const res1 = await ArbitrationCourtService.GetByContractorId(this.Data.contractorId);
               this.Data = res1.data;
            }
         } catch (error) {
            this.showApiError(error);
         } finally {
            this.innLoading = false;
         }
      },
      GetByresponsibleInn() {
         this.innLoading = true;
         ContractorService.SearchByInnPnfl(this.Data.responsibleInn)
            .then((res) => {
               this.Data.responsibleInn = res.data.inn;
               this.Data.responsibleAddress = res.data.address;
               this.Data.responsible = res.data.fullName;
               this.Data.responsibleContractorId = res.data.id;
            })
            .catch(this.showApiError);
      },

      SaveData() {
         this.$refs.ValidationDTO.validate().then((success) => {
            if (success) {
               this.saveLoading = true;
               this.Data.signer = [];
               this.arbitrationJudgeIds.forEach((item, index) => this.Data.signer.push({ arbitrationJudgeId: item }));
               if (this.Data.isForeignContractor && this.$route.params.id == 0) {
                  this.Data.application.contractorPositionName = '';
               }
               ArbitrationCourtService.Update(this.Data)
                  .then((res) => {
                     this.makeToast(this.$t('SaveSuccess'), 'success');
                     // Replace the current route with the new ID in the query parameter
                     this.$router.replace({ params: { ...this.$route.params, id: res.data.id } }).catch((err) => {
                        // Handle duplicate navigation errors or log it as it's a common case when replacing with the same route
                        if (err.name !== 'NavigationDuplicated') {
                           console.error(err);
                        }
                     });

                     ArbitrationCourtService.Get(res.data.id)
                        .then((res1) => {
                           this.Data = res1.data;
                           this.currentStep = this.Data.application.currentStep.id;
                           if (res1.data.signer.length != 0) {
                              this.arbitrationJudgeIds = res1.data.signer?.map((item) => item.arbitrationJudgeId);
                           }
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
<style lang="scss" scoped>
::v-deep .vs--disabled {
   .vs__selected {
      color: #222222;
      background-color: #cdcdcd;
   }
}
::v-deep .vs__selected {
   color: #fff;
}
.flex-container {
   display: flex;
   flex-direction: row;
   justify-content: space-between;
   padding: 1em;
   position: relative;

   .step {
      text-align: center;
      z-index: 2;
      position: relative;
      flex: 1;

      &:first-child {
         &::before {
            width: 50%;
         }
         &::after {
            width: 0;
         }
      }

      &:last-child {
         &::before {
            width: 50%;
            left: 0;
         }
         &::after {
            width: 0;
         }
      }

      &::before,
      &::after {
         content: '';
         position: absolute;
         top: 1em;
         height: 5px;
         width: 50%;
         background-color: #cdcdcd;
         z-index: -1;
      }

      &::before {
         right: 0;
      }

      &::after {
         left: 0;
      }

      &.done {
         &::before {
            background-color: #46c0bd;
         }

         .step-number {
            background-color: #46c0bd;
         }
      }

      &.done + .step {
         &::after {
            background-color: #46c0bd;
         }
         &:last-child::before {
            background-color: #46c0bd;
         }
      }

      .step-number {
         cursor: pointer;
         background-color: #cdcdcd;
         display: inline-flex;
         justify-content: center;
         align-items: center;
         padding: 0.5em;
         color: white;
         border-radius: 2em;
         width: 40px;
         height: 40px;
         z-index: 3;
         background-size: 0% 0%;
         background-position: center;
         background-image: radial-gradient(circle at center, #46c0bd 50%, transparent 50%);
         background-repeat: no-repeat;
      }

      &.current {
         .step-number {
            background-size: 200% 200%;
            transition: all 0.3s;
         }
      }

      .step-label {
         padding-top: 5px;
      }
   }
}
</style>
