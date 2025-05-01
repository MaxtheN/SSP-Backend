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
      <b-card class="mt-2">
         <validation-observer ref="ValidationDTO">
            <h3 class="text-center my-2">{{ $t('Hakamlik sudiga ariza') }}</h3>
            <b-row>
               <b-col sm="12" md="6">
                  <b-row class="flex-column align-content-center">
                     <h4 class="ml-1 my-2">{{ $t('Davogar') }}</h4>
                     <b-col sm="12" md="10">
                        <form-select
                           disabled
                           required-star
                           rules="required"
                           :options="ResponsibleSelectList"
                           v-model="Data.contractorResponsibleTypeId"
                           :label="$t('contractorResponsibleTypeId')"
                        ></form-select>
                     </b-col>
                     <b-col sm="12" md="10"
                        ><form-input-hrm
                           v-if="Data.contractorId"
                           v-model="Data.eCourtNumber"
                           disabled
                           required-star
                           rules="required"
                           :label="$t('eCourtNumber')"
                           :placeholder="$t('eCourtNumber')"
                     /></b-col>
                     <b-col sm="12" md="10">
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
                     <b-col sm="12" md="10">
                        <form-input-hrm
                           v-model="Data.application.contractor"
                           required-star
                           disabled
                           rules="required"
                           :label="$t('contractorName')"
                           :placeholder="$t('contractorName')"
                        />
                     </b-col>

                     <b-col sm="12" md="10">
                        <form-input-hrm
                           disabled
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
                           disabled
                           :label="$t('phoneNumber')"
                           placeholder="(998) ## ### ## ##"
                        />
                     </b-col>

                     <b-col sm="12" md="10">
                        <b-row>
                           <b-col>
                              <form-picker
                                 disabled
                                 rules="required"
                                 v-model="Data.application.docOn"
                                 :label="$t('docOn')"
                                 :placeholder="$t('docOn')"
                              />
                           </b-col>
                           <b-col>
                              <form-input-hrm
                                 disabled
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
                           disabled
                           rules="required"
                        ></form-select>
                     </b-col>
                     <b-col sm="12" md="10">
                        <form-select
                           :options="ArbitrationCourtList"
                           v-model="Data.arbitrationCourtId"
                           required-star
                           disabled
                           :label="$t('arbitrationCourtId')"
                        ></form-select>
                     </b-col>

                     <b-col sm="12" md="10" class="mb-2">
                        <h6 class="inputTitle">{{ $t('fileupload') }}</h6>
                        <b-form-file disabled type="file" :placeholder="$t('Faylni tanlang')" @change="UploadFile">
                        </b-form-file>
                     </b-col>
                     <b-col sm="12" md="10">
                        <form-input-hrm
                           disabled
                           v-model="Data.amount"
                           :label="$t('amount')"
                           :placeholder="$t('amount')"
                        />
                     </b-col>
                  </b-row>
               </b-col>
               <b-col sm="12" md="6" class="">
                  <b-row class="flex-column align-content-center">
                     <h4 class="ml-1 my-2">{{ $t('responsible') }}</h4>
                     <b-col sm="12" md="10">
                        <form-select
                           required-star
                           disabled
                           rules="required"
                           :options="ResponsibleSelectList"
                           v-model="Data.responsibleTypeId"
                           :label="$t('responsibleTypeId')"
                        ></form-select>
                     </b-col>
                     <b-col sm="12" md="10">
                        <div class="form-group">
                           <form-input
                              v-model="Data.responsibleInnPnfl"
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
                     <b-col sm="12" md="10">
                        <form-input-hrm
                           v-model="Data.responsible"
                           rules="required"
                           disabled
                           :label="$t('responsible')"
                           :placeholder="$t('responsible')"
                        />
                     </b-col>

                     <b-col sm="12" md="10">
                        <form-input-hrm
                           v-model="Data.responsibleAddress"
                           disabled
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
                           disabled
                           placeholder="(998) ## ### ## ##"
                        />
                     </b-col>

                     <b-col cols="12" md="10">
                        <form-select
                           multiple
                           :options="JudgeList"
                           v-model="Data.signer.arbitrationJudgeId"
                           :label="$t('judge')"
                           text-field="text"
                           required-star
                           disabled
                           rules="required"
                           value-field="value"
                        ></form-select>
                     </b-col>
                     <b-col cols="12" md="10">
                        <form-select
                           :options="CurrencySelectList"
                           v-model="Data.currencyId"
                           :label="$t('currencyId')"
                           text-field="text"
                           required-star
                           disabled
                           rules="required"
                           value-field="value"
                        ></form-select>
                     </b-col>
                     <b-col sm="12" md="10">
                        <form-input-hrm
                           required-star
                           rules="required"
                           disabled
                           v-model="Data.arbitrationAmount"
                           :label="$t('arbitrationAmount')"
                           :placeholder="$t('arbitrationAmount')"
                        />
                     </b-col>
                     <b-col sm="12" md="10">
                        <p v-show="this.$route.params.id" style="font-weight: 600" class="mt-2">
                           {{ $t('CreatedUser') }} : {{ Data.createdUser }}
                        </p>
                     </b-col>
                  </b-row>
               </b-col>
               <!-- save button -->
               <b-col cols="12" class="text-right">
                  <!-- <b-button v-if="!Data.canSign" :disabled="saveLoading" @click="SaveData" variant="outline-success">
                     <feather-icon icon="CheckIcon"></feather-icon>
                     {{ $t('Save') }}
                  </b-button> -->

                  <b-button
                     v-if="$can('ArbitrationCourtApplicationSign', 'permissions') && Data.canSign"
                     class="text-right"
                     @click="OpenSign"
                     size="xl"
                     variant="outline-success"
                  >
                     <feather-icon icon="CheckIcon"></feather-icon>
                     {{ $t('Sign') }}
                  </b-button>
               </b-col>
            </b-row>
         </validation-observer>
      </b-card>
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
import ManualService from '@/services/others/manual.service';
import CurrencyService from '@/services/others/currency.service';
import ArbitrationJudgeService from '@/services/arbitrationjudge/arbitrationjudge.service';
import ContractorListSelect from '@/views/components/info/ContractorListSelect.vue';
import {
   BOverlay,
   BModal,
   BIconCheckLg,
   BCard,
   BRow,
   BCol,
   BButton,
   BFormFile,
   BInputGroupAppend,
   BCardText
} from 'bootstrap-vue';
import ArbitrationCourtService from '@/services/arbitrationcourt/arbitrationcourt.service.js';
import ContractorService from '@/services/info/contractor.service';
import justSign from '@/components/justSign.vue';
import eimzoMixin from '@/mixins/eimzo';

export default {
   components: {
      BOverlay,
      BInputGroupAppend,
      BCard,
      BRow,
      BModal,
      BCol,
      BButton,
      ContractorListSelect,
      BFormFile,
      BIconCheckLg,
      justSign,
      BCardText
   },
   mixins: [eimzoMixin],
   data() {
      return {
         saveLoading: false,
         show: false,
         EImzoModal: false,
         SignModal: false,
         SignLoading: false,
         innLoading: false,
         ResponsibleSelectList: [],
         CurrencySelectList: [],
         DepartmentList: [],
         AplicationTypeList: [],
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
         currentStep: 1,
         Data: {
            contractorInnPnfl: '',
            responsibleInnPnfl: '',
            eCourtNumber: '',
            contractorResponsibleTypeId: null,
            contractorPhonber: '',
            contractorId: null,
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

            filter: {
               signedData: '',
               id: 0,
               message: '',
               cancelApplication: false,
               files: []
            },
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
      ArbitrationCourtService.Get(this.$route.params.id)
         .then((res) => {
            this.Data = res.data;
            this.currentStep = this.Data.application.currentStep.id;
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
      loginESP(item) {
         const isPinfl = this.isPinfl(item);
         this.Sign(item.key, isPinfl);
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
            prtnRejectReasonId: 0,
            files: []
         };
      },
      Sign(key, isPinfl) {
         this.SignLoading = true;
         this.filter.signedData = key;
         this.filter.isPinfl = isPinfl;
         ArbitrationCourtService.Sign(this.filter)
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
               columnName: this.Data.application.currentStep.id
            }));

            this.Data.files.push(...updatedFiles);

            this.fileLoading = false;
         });
      },
      // handleContrcatorinfo(e) {
      //    this.Data.contractorAddress = e.address;
      //    this.Data.contractorName = e.fullName;
      //    // console.log(e);
      // },
      // handleResponsibleinfo(e) {
      //    this.Data.responsibleAddress = e.address;
      //    this.Data.responsibleName = e.fullName;
      // },

      async GetByContractorInn() {
         try {
            this.innLoading = true;
            const res = await ContractorService.SearchByInnPnfl(this.Data.contractorInnPnfl);
            this.Data.contractorInnPnfl = res.data.inn;
            this.Data.contractorAddress = res.data.address;
            this.Data.application.contractor = res.data.fullName;
            this.Data.contractorId = res.data.id;
            this.Data.application.contractorPositionName = res.data.fullName;

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
         ContractorService.SearchByInnPnfl(this.Data.responsibleInnPnfl)
            .then((res) => {
               this.Data.responsibleInnPnfl = res.data.inn;
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

               ArbitrationCourtService.Update(this.Data)
                  .then(() => {
                     this.makeToast(this.$t('SaveSuccess'), 'success');
                     this.$router.push({ name: 'arbitrationcourt' });
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
