<template>
   <b-card>
      <b-row>
         <b-col md="4" lg="4">
            <div class="form-group">
               <form-select
                  :options="ApplicantTypeList"
                  v-model="Data.proposalTypeId"
                  required-star
                  :label="$t('proposalType')"
               ></form-select>
            </div>
         </b-col>
         <b-col md="4" lg="4">
            <div class="form-group">
               <form-select
                  :options="CompanyTypeList"
                  v-model="Data.companyTypeId"
                  required-star
                  :label="$t('companyType')"
               ></form-select>
            </div>
         </b-col>
         <b-col md="4" lg="4">
            <div class="form-group">
               <form-select
                  :options="BusinessSectorList"
                  v-model="Data.businessSectorId"
                  required-star
                  :label="$t('businessSector')"
               ></form-select>
            </div>
         </b-col>
      </b-row>
      <hr />
      <b-row>
         <b-col sm="12" md="6" class="text-left mt-2" v-if="Data.proposalTypeId == 1">
            <div class="form-group">
               <label for="">{{ $t('companyInn') }}</label>
               <b-input-group>
                  <b-form-input v-model="Data.companyInn" placeholder="companyInn" />
                  <b-input-group-append>
                     <b-button @click="GetFromSoliq" variant="primary">
                        <feather-icon icon="SearchIcon" />
                     </b-button>
                  </b-input-group-append>
               </b-input-group>
            </div>
         </b-col>
         <b-col sm="12" md="6" class="text-left mt-2" v-if="Data.proposalTypeId == 1">
            <div class="form-group">
               <form-input v-model="Data.companyName" required :label="$t('companyName')" />
            </div>
         </b-col>
         <b-col sm="12" md="4">
            <div class="form-group">
               <form-input v-model="Data.surnameLatin" required :label="$t('lastName')" />
            </div>
         </b-col>
         <b-col sm="12" md="4">
            <div class="form-group">
               <form-input v-model="Data.nameLatin" required :label="$t('firstName')" />
            </div>
         </b-col>

         <b-col md="4" lg="4">
            <div class="form-group">
               <form-select
                  :options="GenderList"
                  v-model="Data.genderId"
                  required-star
                  :label="$t('gender')"
               ></form-select>
            </div>
         </b-col>
         <b-col sm="12" md="4" class="text-left mt-2">
            <div class="form-group">
               <form-input
                  v-model="Data.phoneNumber"
                  required
                  :label="$t('phoneNumber')"
                  v-mask="'+998 ## ### ## ##'"
               />
            </div>
         </b-col>
      </b-row>
      <hr />
      <b-row>
         <b-col md="6" lg="6">
            <div class="form-group">
               <form-select
                  :options="RegionList"
                  @input="ChangeDistrict"
                  v-model="Data.regionId"
                  required-star
                  :label="$t('region')"
               ></form-select>
            </div>
         </b-col>
         <b-col md="6" lg="6">
            <div class="form-group">
               <form-select
                  :options="DistrictList"
                  @input="ChangeMfy"
                  v-model="Data.districtId"
                  required-star
                  :label="$t('District')"
               ></form-select>
            </div>
         </b-col>
         <b-col md="6" lg="6">
            <div class="form-group">
               <form-select :options="MfyList" v-model="Data.mfyId" required-star :label="$t('mfy')"></form-select>
            </div>
         </b-col>

         <b-col sm="6" md="6">
            <div class="form-group">
               <form-input v-model="Data.address" required :label="$t('address')" />
            </div>
         </b-col>
      </b-row>
      <hr />
      <b-row>
         <b-col md="6" lg="6">
            <div class="form-group">
               <form-select
                  :options="ProposalSubjectList"
                  v-model="Data.proposalSubjectId"
                  required-star
                  :label="$t('proposalSubject')"
               ></form-select>
            </div>
         </b-col>
      </b-row>
      <hr />
      <b-row>
         <b-col sm="12" md="12" class="text-left">
            <h6 class="inputTitle">
               {{ $t('appealText') }}
               <span style="color: red">*</span>
            </h6>
            <b-form-textarea v-model="Data.appealText" :placeholder="$t('appealText')"></b-form-textarea>
         </b-col>
         <b-col lg="12" sm="12" class="mt-2">
            <h6 class="inputTitle">
               {{ $t('proposalText') }}
               <span style="color: red">*</span>
            </h6>
            <b-form-textarea type="text" rows="4" v-model="Data.proposalText" />
         </b-col>

         <hr style="margin: 45px 0" />
         <b-col lg="4" md="6" sm="12" class="mt-2">
            <h6 class="inputTitle">{{ $t('files') }}</h6>
            <b-form-file type="file" :placeholder="$t('Faylni tanlang')" @change="UploadFile"></b-form-file>
         </b-col>
      </b-row>
      <b-row class="mt-3">
         <b-col sm="12" md="6" lg="6" class="text-left"></b-col>
         <b-col sm="12" md="6" lg="6" class="text-right">
            <b-button @click="SaveData" size="sm" variant="outline-success">
               <feather-icon icon="CheckIcon"></feather-icon>
               {{ $t('Save') }}
            </b-button>
         </b-col>
      </b-row>
   </b-card>
</template>

<script>
import {
   BSpinner,
   BFormFile,
   BButton,
   BPagination,
   BTable,
   BCol,
   BRow,
   BCard,
   BTooltip,
   BBadge,
   BInputGroup,
   BFormInput,
   BInputGroupAppend,
   BModal,
   BLink,
   BCardText,
   BFormTextarea
} from 'bootstrap-vue';
import RegionService from '@/services/info/region.service';
import DistrictService from '@/services/info/district.service';
import MfyService from '@/services/info/mfy.service';
import OrganizationService from '@/services/managment/organization.service';

import ProposalService from '@/services/document/proposal.service';
import ManualService from '@/services/others/manual.service';
export default {
   components: {
      BButton,
      BPagination,
      BTable,
      BCol,
      BRow,
      BSpinner,
      BCard,
      BTooltip,
      BFormFile,
      BBadge,
      BInputGroup,
      BFormInput,
      BInputGroupAppend,
      BModal,
      BLink,
      BCardText,
      BFormTextarea
   },
   data() {
      return {
         Data: {},
         ApplicantTypeList: [],
         BusinessSectorList: [],
         GenderList: [],
         RegionList: [],
         DistrictList: [],
         MfyList: [],
         // EmploymentTypeList: [],
         ProposalSubjectList: [],
         proposalDisclosure: [],
         OrganizationList: [],
         CompanyTypeList: [],
         loadingButton: false,
         fileLoading: false
      };
   },
   created() {
      this.lang = localStorage.getItem('locale') || 'ru';
      ProposalService.Get(this.$route.params.id).then((res) => {
         this.Data = res.data;
      });
      ManualService.ApplicantTypeSelectList().then((res) => {
         this.ApplicantTypeList = res.data;
      });
      ManualService.ProposalSubjectSelectList().then((res) => {
         this.ProposalSubjectList = res.data;
      });
      ManualService.ProposalDisclosureSelectList().then((res) => {
         this.ProposalDisclosureList = res.data;
      });
      ManualService.CompanyTypeSelectList().then((res) => {
         this.CompanyTypeList = res.data;
      });

      ManualService.GenderSelectList().then((res) => {
         this.GenderList = res.data;
      });
      ManualService.BusinessSectorSelectList().then((res) => {
         this.BusinessSectorList = res.data;
      });
      OrganizationService.GetAsSelectList()
         .then((res) => {
            this.OrganizationList = res.data;
         })
         .catch((error) => {
            this.showApiError(error);
         });
      RegionService.GetAsSelectList().then((res) => {
         this.RegionList = res.data;
      });
      if (this.Data.regionId) {
         this.ChangeDistrict();
      }
      if (this.Data.regionId && this.Data.districtId) {
         this.ChangeMfy();
      }
   },
   methods: {
      GetFromSoliq() {
         if (this.Data.companyInn.length == 9) {
            ProposalService.GetContractorFromSoliq(this.Data.companyInn).then((res) => {
               this.Data.companyName = res.data.shortName;
            });
         } else {
            return this.makeToast(this.$t('InnShouldBe9Symbols'));
         }
      },

      UploadFile(event) {
         const formData = new FormData();
         formData.append('files', event.target.files[0]);
         this.fileLoading = true;
         ProposalService.UploadFiles(formData).then((res) => {
            this.Data.files.push(res.data[0]);
            this.fileLoading = false;
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
      ChangeDistrict() {
         if (this.Data.regionId) {
            DistrictService.GetAsSelectList(this.Data.regionId).then((res) => {
               this.DistrictList = res.data;
            });
         } else {
            this.DistrictList = [];
            this.Data.districtId = null;
         }
      },
      ChangeMfy() {
         if (this.Data.regionId && this.Data.districtId) {
            MfyService.GetAsSelectList(this.Data.regionId, this.Data.districtId).then((res) => {
               this.MfyList = res.data;
            });
         } else {
            this.MfyList = [];
            this.Data.mfyId = null;
         }
      },
      SaveData() {
         this.loadingButton = true;

         this.Data.externalSourceTypeId = 1;
         ProposalService.Update(this.Data)
            .then((res) => {
               this.makeToast(this.$t('SaveSuccess'), 'success');
               this.$router.push({ name: 'Proposal' });
            })
            .catch((err) => {
               console.log(err);
               this.makeToast(this.$t(err), 'danger');
            });
      }
   }
};
</script>

<style></style>
