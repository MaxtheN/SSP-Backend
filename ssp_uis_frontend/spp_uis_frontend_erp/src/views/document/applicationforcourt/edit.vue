<template>
   <b-overlay :show="show">
      <b-card>
         <validation-observer ref="ValidationDTO">
            <b-row>
               <b-col sm="12" md="4">
                  <form-input-hrm
                     rules="required"
                     v-model="Data.docNumber"
                     required
                     :label="$t('docnumber')"
                     :disabled="isComponent"
                  />
               </b-col>
               <b-col sm="12" md="4">
                  <form-picker
                     rules="required"
                     v-model="Data.docOn"
                     required
                     :label="$t('docOn')"
                     :placeholder="$t('docOn')"
                     :disabled="isComponent"
                  />
               </b-col>
               <b-col cols="12" md="4">
                  <form-select
                     rules="required"
                     :options="ClaimOrganizationList"
                     v-model="Data.claimOrganizationId"
                     label="ClaimOrganization"
                     :disabled="isComponent"
                  ></form-select>
               </b-col>
               <b-col cols="12" md="4">
                  <form-select
                     :options="DepartmentListFilter([1, 10])"
                     v-model="Data.departmentId"
                     label="Department"
                     required-star
                     :disabled="isComponent"
                  ></form-select>
               </b-col>
               <b-col cols="12" md="4">
                  <form-select
                     :options="PositionListFilter(Data.departmentId)"
                     v-model="Data.positionId"
                     label="position"
                     valueid="positionId"
                     valuename="positionName"
                     :disabled="isComponent"
                     required-star
                     @input="
                        () => {
                           Data.employeeManageId = null;
                           Data.employee = null;
                        }
                     "
                  ></form-select>
               </b-col>
               <b-col cols="12" md="4">
                  <HrmEmployeeManageSelect2
                     required-star
                     :organization-id="organizationId"
                     :position-id="Data.positionId"
                     :department-id="Data.departmentId"
                     v-model="Data.employeeManageId"
                     :disabled="isComponent"
                  />
               </b-col>
            </b-row>

            <b-row>
               <b-col md="6" sm="12">
                  <h6 class="inputTitle">{{ $t('files') }}</h6>
                  <b-form-file
                     type="file"
                     :disabled="isComponent"
                     :placeholder="$t('Faylni tanlang')"
                     @change="UploadFile"
                  ></b-form-file>
                  <div class="mt-1" v-for="item in Data.files" :key="item.id">
                     <b-link variant="primary" target="_blank" :href="FileSrc(item.id)">{{
                        item.fileName || item.id
                     }}</b-link>
                  </div>
               </b-col>
               <b-col md="6" sm="12">
                  <b-button class="mt-2" @click="OpenCheckFromBank" variant="info">
                     <feather-icon icon="CheckIcon"></feather-icon>
                     {{ $t('CheckFromBank') }}
                  </b-button>
               </b-col>

               <b-col v-if="!isComponent" sm="12" md="6" lg="6" align-self="end" offset-md="6" class="text-right">
                  <b-button :disabled="saveLoading" @click="SaveData" variant="outline-success">
                     <feather-icon icon="CheckIcon"></feather-icon>
                     {{ $t('Save') }}
                  </b-button>
               </b-col>
            </b-row>
         </validation-observer>
         <b-modal v-model="CheckFromBank" size="xl" :title="$t('CheckFromBank')" hide-footer>
            <b-card-text>
               <b-row class="font-weight-bold">
                  <b-col sm="6" md="6" class="px-5">
                     <b-col md="12" sm="12" class="text-center"
                        ><h3>{{ TempData?.bankName }}</h3></b-col
                     >
                     <b-row style="border-bottom: 1px solid #a9a9a9">
                        <b-col md="6" sm="6" style="padding: 5px">{{ $t('ApplicaitonDocOn') }}: </b-col>
                        <b-col md="6" sm="6" style="padding: 5px" class="text-right">{{
                           TempData?.dateFromBank ? TempData?.dateFromBank : ''
                        }}</b-col>
                     </b-row>
                     <b-row style="border-bottom: 1px solid #a9a9a9">
                        <b-col md="6" sm="6" style="padding: 5px">{{ $t('mainDebt') }}: </b-col>
                        <b-col md="6" sm="6" style="padding: 5px" class="text-right">
                           {{ currency(TempData?.mainDebtFromBank ? TempData?.mainDebtFromBank : 0) }}</b-col
                        >
                     </b-row>
                     <b-row style="border-bottom: 1px solid #a9a9a9">
                        <b-col md="6" sm="6" style="padding: 5px">{{ $t('calculedPenalty') }}: </b-col>
                        <b-col md="6" sm="6" style="padding: 5px" class="text-right">
                           {{
                              currency(TempData?.calculedPenaltyFromBank ? TempData?.calculedPenaltyFromBank : 0)
                           }}</b-col
                        >
                     </b-row>
                     <b-row style="border-bottom: 1px solid #a9a9a9">
                        <b-col md="6" sm="6" style="padding: 5px">{{ $t('penalty') }}: </b-col>
                        <b-col md="6" sm="6" style="padding: 5px" class="text-right">
                           {{ currency(TempData?.penaltyFromBank ? TempData?.penaltyFromBank : 0) }}</b-col
                        >
                     </b-row>
                     <b-row style="border-bottom: 1px solid #a9a9a9">
                        <b-col md="6" sm="6" style="padding: 5px">{{ $t('percent') }}: </b-col>
                        <b-col md="6" sm="6" style="padding: 5px" class="text-right">
                           {{ currency(TempData?.percentFromBank ? TempData?.percentFromBank : 0) }}</b-col
                        >
                     </b-row>
                     <b-row style="border-bottom: 1px solid #a9a9a9">
                        <b-col md="6" sm="6" style="padding: 5px">{{ $t('currentPrincipalInterest') }}: </b-col>
                        <b-col md="6" sm="6" style="padding: 5px" class="text-right">
                           {{
                              currency(
                                 TempData?.currentPrincipalInterestFromBank
                                    ? TempData?.currentPrincipalInterestFromBank
                                    : 0
                              )
                           }}</b-col
                        >
                     </b-row>
                     <b-row style="border-bottom: 1px solid #a9a9a9">
                        <b-col md="6" sm="6" style="padding: 5px">{{ $t('currentInterestRate') }}: </b-col>
                        <b-col md="6" sm="6" style="padding: 5px" class="text-right">
                           {{
                              currency(
                                 TempData?.currentInterestRateFromBank ? TempData?.currentInterestRateFromBank : 0
                              )
                           }}</b-col
                        >
                     </b-row>
                     <b-row style="border-bottom: 1px solid #a9a9a9">
                        <b-col md="6" sm="6" style="padding: 5px">{{ $t('otherDebtRepayment') }}: </b-col>
                        <b-col md="6" sm="6" style="padding: 5px" class="text-right">
                           {{
                              currency(TempData?.otherDebtRePaymentFromBank ? TempData?.otherDebtRePaymentFromBank : 0)
                           }}</b-col
                        >
                     </b-row>
                  </b-col>
                  <b-col sm="6" md="6" class="px-5">
                     <b-col md="12" sm="12" class="text-center"
                        ><h3>{{ $t('fromSSP') }}</h3></b-col
                     >
                     <b-row style="border-bottom: 1px solid #a9a9a9">
                        <b-col md="6" sm="6" style="padding: 5px">{{ $t('ApplicaitonDocOn') }}: </b-col>
                        <b-col md="6" sm="6" style="padding: 5px" class="text-right">
                           {{ TempData?.dateFromSsp ? TempData?.dateFromSsp : '' }}</b-col
                        >
                     </b-row>
                     <b-row style="border-bottom: 1px solid #a9a9a9">
                        <b-col md="6" sm="6" style="padding: 5px">{{ $t('mainDebt') }}: </b-col>
                        <b-col md="6" sm="6" style="padding: 5px" class="text-right">{{
                           currency(TempData?.mainDebtFromSsp ? TempData?.mainDebtFromSsp : 0)
                        }}</b-col>
                     </b-row>
                     <b-row style="border-bottom: 1px solid #a9a9a9">
                        <b-col md="6" sm="6" style="padding: 5px">{{ $t('calculedPenalty') }}: </b-col>
                        <b-col md="6" sm="6" style="padding: 5px" class="text-right">
                           {{
                              currency(TempData?.calculedPenaltyFromSsp ? TempData?.calculedPenaltyFromSsp : 0)
                           }}</b-col
                        >
                     </b-row>
                     <b-row style="border-bottom: 1px solid #a9a9a9">
                        <b-col md="6" sm="6" style="padding: 5px">{{ $t('penalty') }}: </b-col>
                        <b-col md="6" sm="6" style="padding: 5px" class="text-right">
                           {{ currency(TempData?.penaltyFromSsp ? TempData?.penaltyFromSsp : 0) }}</b-col
                        >
                     </b-row>
                     <b-row style="border-bottom: 1px solid #a9a9a9">
                        <b-col md="6" sm="6" style="padding: 5px">{{ $t('percent') }}: </b-col>
                        <b-col md="6" sm="6" style="padding: 5px" class="text-right">{{
                           currency(TempData?.percentFromSsp ? TempData?.percentFromSsp : 0)
                        }}</b-col>
                     </b-row>
                     <b-row style="border-bottom: 1px solid #a9a9a9">
                        <b-col md="6" sm="6" style="padding: 5px">{{ $t('currentPrincipalInterest') }}: </b-col>
                        <b-col md="6" sm="6" style="padding: 5px" class="text-right">
                           {{
                              currency(
                                 TempData?.currentPrincipalInterestFromSsp
                                    ? TempData?.currentPrincipalInterestFromSsp
                                    : 0
                              )
                           }}</b-col
                        >
                     </b-row>
                     <b-row style="border-bottom: 1px solid #a9a9a9">
                        <b-col md="6" sm="6" style="padding: 5px">{{ $t('currentInterestRate') }}: </b-col>
                        <b-col md="6" sm="6" style="padding: 5px" class="text-right">
                           {{
                              currency(TempData?.currentInterestRateFromSsp ? TempData?.currentInterestRateFromSsp : 0)
                           }}</b-col
                        >
                     </b-row>
                     <b-row style="border-bottom: 1px solid #a9a9a9">
                        <b-col md="6" sm="6" style="padding: 5px">{{ $t('otherDebtRepayment') }}: </b-col>
                        <b-col md="6" sm="6" style="padding: 5px" class="text-right">
                           {{
                              currency(TempData?.otherDebtRePaymentFromSsp ? TempData?.otherDebtRePaymentFromSsp : 0)
                           }}</b-col
                        >
                     </b-row>
                  </b-col>
               </b-row>
               <b-row
                  ><b-col class="text-right pt-1">
                     <b-button variant="primary" @click="DownloadFile">{{ $t('downloadPdf') }}</b-button>
                  </b-col></b-row
               >
            </b-card-text>
         </b-modal>
      </b-card>
   </b-overlay>
</template>
<script>
// service
import DepartmentService from '@/services/info/department.service';
import StaffingService from '@/services/hrm/staffing.service';
import ClaimApplicationService from '@/services/document/claimapplication.service';
import ApplicationForCourtService from '@/services/document/applicationforcourt.service';
// components
import { DEPARTMENT_CODE } from '@/constants/department';
import HrmEmployeeManageSelect2 from '@/views/components/hrm/HrmEmployeeManageSelect2.vue';
import { BOverlay, BCard, BRow, BCol, BTable, BModal, BCardText, BButton, BLink, BFormFile } from 'bootstrap-vue';
import ClaimOrganizationService from '@/services/info/claimorganization.service';
import axios from 'axios';

export default {
   components: {
      BCardText,
      BModal,
      BOverlay,
      BCard,
      BRow,
      BCol,
      BButton,
      BTable,
      BLink,
      BFormFile,
      HrmEmployeeManageSelect2
   },
   name: 'ApplicationForCourtEdit',
   props: {
      isComponent: {
         type: Boolean,
         default: false
      }
   },
   data() {
      return {
         DEPARTMENT_CODE: DEPARTMENT_CODE,
         show: false,
         CheckFromBank: false,
         ClaimOrganizationList: [],
         PositionList: [],
         TempData: {},
         DepartmentList: [],
         saveLoading: false,
         organizationId: 0,
         fileLoading: false,
         Data: {
            docOn: '',
            docNumber: '',
            mediationId: null,
            claimOrganizationId: null,
            contractorDetails: '',
            details: '',
            files: []
         }
      };
   },
   computed: {
      FileSrc() {
         return (id) => axios.defaults.baseURL + `ApplicationForCourt/DownloadFile/${id}`;
      },
      PositionListFilter() {
         return (departmentId) => {
            return departmentId ? this.PositionList.filter((e) => e.departmentId == departmentId) : this.PositionList;
         };
      },
      DepartmentListFilter() {
         return (code) => {
            return code
               ? this.DepartmentList.filter((e) => (Array.isArray(code) ? code.includes(e.code) : e.code == code))
               : this.DepartmentList.filter((e) => e.code != DEPARTMENT_CODE['director']);
         };
      }
   },
   created() {
      this.organizationId = JSON.parse(localStorage.getItem('user_info')).organizationId;
      this.show = true;
      if (this.$route.query.mediationId) {
         ApplicationForCourtService.GetByMediationId(this.$route.query.mediationId)
            .then((res) => {
               this.Data = res.data;
            })
            .catch((error) => {
               this.makeToast(error, 'danger');
            })
            .finally(() => {
               this.show = false;
            });
      } else {
         ApplicationForCourtService.Get(this.$route.params.id)
            .then((res) => {
               this.Data = res.data;
               this.Data.applicationId = this.$route.query.applicationID;
               if (this.$route.query.docNumber) {
                  this.Data.docNumber = this.$route.query.docNumber;
               }
               if (this.$route.query.claimAplication2) {
                  ClaimApplicationService.GetForInfo(this.$route.query.claimAplication2)
                     .then((result) => {
                        this.Data.docNumber = result.data.mediation?.docNumber;
                        this.Data.mediationId = result.data.mediation?.id;
                        this.Data.claimOrganizationId = result.data.applicationForCourt?.claimOrganizationId;
                        this.Data.contractorDetails = result.data.mediation?.contractorDetails;
                        this.Data.details = result.data.claimApplication?.details;
                        this.Data.files = result.data.mediation?.files;
                     })
                     .catch((error) => {
                        this.makeToast(error, 'danger');
                     })
                     .finally(() => {
                        this.show = false;
                     });
               }
            })
            .catch((error) => {
               this.makeToast(error, 'danger');
            })
            .finally(() => {
               this.show = false;
            });
      }
      ClaimOrganizationService.GetAsSelectList().then((res) => {
         this.ClaimOrganizationList = res.data;
      });

      DepartmentService.GetAsSelectList(this.organizationId, {})
         .then((res) => {
            if (Array.isArray(res.data)) {
               this.DepartmentList = res.data;
            }
         })
         .catch((error) => {
            this.showApiError(error);
         });
      StaffingService.GetStaffingPositionClassification(null, null, null, this.organizationId).then((res) => {
         res.data.forEach((item) => {
            if (item.positionId != 17) {
               this.PositionList.push(item);
            }
         });
      });
   },
   methods: {
      OpenCheckFromBank() {
         ApplicationForCourtService.ChekAllBanks(this.Data.mediationId).then((res) => {
            this.TempData = res.data;

            this.CheckFromBank = true;
            console.log('CheckFromBank:', this.CheckFromBank);

            this.CheckFromBank = true;
         });
      },
      DownloadFile() {
         ApplicationForCourtService.DownloadBankDocument(this.Data.mediationId, this.getPdfLang()).then((res) => {
            this.forceFileDownload(res, 'DataFromBank', '.pdf');
         });
      },
      UploadFile(event) {
         const formData = new FormData();
         formData.append('files', event.target.files[0]);
         this.fileLoading = true;
         ApplicationForCourtService.UploadFiles(formData).then((res) => {
            this.Data.files = res.data.map((f) => ({ id: f.fileId }));
            this.fileLoading = false;
         });
      },

      SaveData() {
         this.$refs.ValidationDTO.validate().then((success) => {
            if (success) {
               this.saveLoading = true;
               ApplicationForCourtService.Update({
                  ...this.Data
               })
                  .then((res) => {
                     this.makeToast(this.$t('SaveSuccess'), 'success');
                     this.$router.push({ name: 'ApplicationForCourt' });
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
