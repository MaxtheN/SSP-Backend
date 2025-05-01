<template>
   <b-overlay>
      <b-card>
         <validation-observer ref="ValidationDTO">
            <b-row>
               <b-col cols="12" md="4">
                  <b-input-group>
                     <b-form-input
                        v-model="Data.application.contractorInn"
                        @keyup.enter="SearchInn"
                        :placeholder="$t('inn')"
                     />
                     <b-input-group-append>
                        <b-button @click="SearchInn" variant="primary" :disabled="getLoading">
                           <b-spinner v-if="getLoading" small></b-spinner>
                           <feather-icon v-else icon="SearchIcon" />
                        </b-button>
                     </b-input-group-append>
                  </b-input-group>
               </b-col>
            </b-row>
            <b-row v-if="Data.application?.contractorId">
               <b-col sm="12" md="4">
                  <form-picker
                     rules="required"
                     v-model="Data.application.docOn"
                     required
                     disabled
                     :label="$t('docOn')"
                     :placeholder="$t('docOn')"
                  />
               </b-col>
               <b-col sm="12" md="4">
                  <form-input-hrm
                     rules="required"
                     v-model="Data.application.docNumber"
                     required
                     disabled
                     :label="$t('docnumber')"
                  />
               </b-col>
               <b-col sm="12" md="4">
                  <form-picker
                     rules="required"
                     v-model="Data.registrationDate"
                     required
                     :label="$t('registrationDate')"
                     :placeholder="$t('registrationDate')"
                  />
               </b-col>
               <b-col sm="12" md="4">
                  <form-input-hrm
                     rules="required"
                     v-model="Data.application.contractor"
                     required
                     disabled
                     :label="$t('contractor')"
                  />
               </b-col>

               <b-col cols="12" md="4">
                  <form-select
                     :options="RegionList"
                     required
                     v-model="Data.application.regionId"
                     @change="
                        () => {
                           Data.application.districtId = null;
                           ChangeRegion();
                        }
                     "
                     label="Oblast"
                  ></form-select>
               </b-col>
               <b-col sm="12" md="3">
                  <form-select
                     :options="DistrictList"
                     v-model="Data.application.districtId"
                     :label="$t('District')"
                  ></form-select>
               </b-col>
               <b-col sm="12" md="4">
                  <form-input-hrm
                     rules="required"
                     v-model="Data.application.contractorDirector"
                     required
                     :label="$t('contractorDirector')"
                  />
               </b-col>
               <b-col sm="12" md="4">
                  <form-input-hrm
                     rules="required"
                     v-model="Data.contractorWorkPhoneNumber"
                     required
                     v-mask="'+998 ## ### ## ##'"
                     :label="$t('contractorWorkPhoneNumber')"
                  />
               </b-col>
               <b-col sm="12" md="4">
                  <form-input-hrm rules="required" v-model="Data.ownerName" required :label="$t('ownerName')" />
               </b-col>

               <b-col sm="12" md="4">
                  <form-input-hrm
                     rules="required"
                     v-model="Data.contractorMobilePhoneNumber"
                     required
                     v-mask="'+998 ## ### ## ##'"
                     :label="$t('contractorMobilePhoneNumber')"
                  />
               </b-col>
               <b-col sm="12" md="3" v-if="Data.application.contractorSettlementAccountId">
                  <form-select
                     :options="OrgSettlementAccountList"
                     v-model="Data.application.contractorSettlementAccountId"
                     label="contractorSettlementAccountId"
                  />
               </b-col>
               <b-col sm="12" md="4">
                  <form-input-hrm v-model="Data.contractorEmail" :label="$t('addressEmail')" />
               </b-col>
               <b-col sm="12" md="4">
                  <form-input-hrm
                     rules="required"
                     v-model="Data.yearlyEarnings"
                     required
                     :label="$t('yearlyEarnings')"
                  />
               </b-col>
               <b-col sm="12" md="4">
                  <form-input-hrm
                     rules="required"
                     v-model="Data.nowYearlyEarnings"
                     required
                     :label="$t('nowYearlyEarnings')"
                  />
               </b-col>
               <b-col sm="12" md="8">
                  <form-select
                     :options="OkedList"
                     v-model="Data.okedId"
                     required-star
                     :label="$t('Oked')"
                  ></form-select>
               </b-col>
               <b-col sm="12" md="3">
                  <form-select
                     v-model="Data.contractorCategoryId"
                     :options="ContractorCategoryList"
                     required-star
                     :label="$t('contractorCategory')"
                  />
               </b-col>
               <b-col sm="12" md="4">
                  <form-input-hrm
                     rules="required"
                     v-model="Data.employeesCount"
                     required
                     :label="$t('employeesCount')"
                  />
               </b-col>
            </b-row>

            <b-row v-if="Data.application?.contractorId">
               <b-col md="6" sm="12">
                  <h6 class="inputTitle">{{ $t('files') }}</h6>
                  <b-form-file type="file" :placeholder="$t('Faylni tanlang')" @change="UploadFile"></b-form-file>
                  <div class="mt-1" v-for="item in Data.files" :key="item.id">
                     <b-link variant="primary" target="_blank" :href="FileSrc(item.id)">{{
                        item.fileName || item.id
                     }}</b-link>
                  </div>
               </b-col>

               <b-col v-if="!isComponent" sm="12" md="6" lg="6" align-self="end" offset-md="6" class="text-right">
                  <b-button :disabled="saveLoading" @click="SaveData" variant="outline-success">
                     <b-spinner v-if="saveLoading" small></b-spinner>
                     <feather-icon v-else icon="CheckIcon"></feather-icon>
                     {{ $t('Save') }}
                  </b-button>
               </b-col>
            </b-row>
         </validation-observer>
      </b-card>
   </b-overlay>
</template>
<script>
// service
import RegionService from '@/services/info/region.service';

import ManualService from '@/services/others/manual.service';
import OkedService from '@/services/info/oked.service';
import DistrictService from '@/services/info/district.service';
import MemshipApplicationService from '@/services/document/memshipapplication.service';

import OrganizationService from '@/services/managment/organization.service';

// components
import HrmEmployeeManageSelect2 from '@/views/components/hrm/HrmEmployeeManageSelect2.vue';
import {
   BOverlay,
   BInputGroupAppend,
   BFormInput,
   BInputGroup,
   BCard,
   BRow,
   BCol,
   BTable,
   BModal,
   BCardText,
   BButton,
   BLink,
   BFormFile,
   BSpinner
} from 'bootstrap-vue';
import ClaimOrganizationService from '@/services/info/claimorganization.service';

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
      HrmEmployeeManageSelect2,
      BInputGroupAppend,
      BFormInput,
      BInputGroup,
      BSpinner
   },
   name: 'MemshipApplicationEdit',
   props: {
      isComponent: {
         type: Boolean,
         default: false
      }
   },
   data() {
      return {
         RegionList: [],
         OrgSettlementAccountList: [],
         OkedList: [],
         DistrictList: [],
         ContractorCategoryList: [],

         saveLoading: false,
         getLoading: false,
         organizationId: 0,
         fileLoading: false,
         Data: {
            files: [],
            application: {}
         },
         DataRes: {}
      };
   },

   created() {
      this.organizationId = JSON.parse(localStorage.getItem('user_info')).organizationId;

      RegionService.GetAsSelectList(211).then((res) => {
         this.RegionList = res.data;
      });
      ClaimOrganizationService.GetAsSelectList().then((res) => {
         this.ClaimOrganizationList = res.data;
      });
      OkedService.GetAsSelectList()
         .then((res) => {
            this.OkedList = res.data;
         })
         .catch(this.showApiError);
      ManualService.ContractorCategorySelectList().then((res) => {
         this.ContractorCategoryList = res.data;
      });
      OrganizationService.GetAsSelectListOrgSettlementAccount()
         .then((res) => {
            if (Array.isArray(res.data)) {
               this.OrgSettlementAccountList = res.data;
            }
         })
         .catch((error) => {
            this.showApiError(error);
         });
   },
   methods: {
      async SearchInn() {
         try {
            this.getLoading = true;
            const res = await MemshipApplicationService.GetForErp(this.Data.application.contractorInn);
            this.Data = res.data.result;
            this.DataRes = JSON.parse(JSON.stringify(res.data.result));
            this.ChangeRegion();
         } catch (error) {
            this.showApiError(error);
         } finally {
            this.getLoading = false;
         }
      },
      ChangeRegion() {
         if (this.Data.application.regionId)
            DistrictService.GetAsSelectList(this.Data.application.regionId)
               .then((res) => {
                  this.DistrictList = res.data;
               })
               .catch((err) => {
                  this.showApiError(err);
               })
               .finally(() => {
                  this.saveLoading = false;
               });
      },

      UploadFile(event) {
         const formData = new FormData();
         formData.append('files', event.target.files[0]);
         this.fileLoading = true;
         MemshipApplicationService.UploadFile(formData).then((res) => {
            this.Data.files = res.data.map((f) => ({ id: f.fileId }));
            this.fileLoading = false;
         });
      },

      SaveData() {
         this.$refs.ValidationDTO.validate().then((success) => {
            if (success) {
               this.saveLoading = true;
               this.Data.contractorInn = this.Data.application.contractorInn;
               // set chooseLocation
               if (this.Data.application.regionId != this.DataRes.application.regionId) {
                  this.Data.chooseLocation = true;
                  this.Data.choosedRegionId = this.Data.application.regionId;
                  this.Data.choosedDistrictId = this.Data.application.districtId;
               }

               MemshipApplicationService.CreateForErp(this.Data)
                  .then((res) => {
                     this.makeToast(this.$t('SaveSuccess'), 'success');
                     this.$router.push({ name: 'MemshipApplication' });
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
