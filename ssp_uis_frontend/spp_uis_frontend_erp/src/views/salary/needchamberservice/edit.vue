<template>
   <b-overlay :show="show">
      <b-card>
         <validation-observer ref="ValidationDTO">
            <b-row>
               <b-col sm="12" md="3" class="mb-1">
                  <form-input-hrm v-model="Data.code" :label="$t('kode')" :placeholder="$t('kode')" rules="required" />
               </b-col>
               <b-col sm="12" md="3" class="mb-1">
                  <form-input-hrm
                     v-model="Data.orderCode"
                     :label="$t('orderCode')"
                     :name="$t('orderCode')"
                     :placeholder="$t('orderCode')"
                     rules="required"
                  />
               </b-col>
               <b-col sm="12" md="6" class="mb-1">
                  <form-input-translate
                     v-model="Data.shortName"
                     @update:translates="(e) => (Data.translates = e)"
                     :translates="Data.translates"
                     column-name="short_name"
                     required
                     :label="$t('shortname')"
                     :name="$t('shortname')"
                     :placeholder="$t('shortname')"
                  />
               </b-col>
               <b-col sm="12" md="6" class="mb-1">
                  <form-input-translate
                     v-model="Data.fullName"
                     @update:translates="(e) => (Data.translates = e)"
                     :translates="Data.translates"
                     column-name="full_name"
                     required
                     :label="$t('fullname')"
                     :name="$t('fullname')"
                     :placeholder="$t('fullname')"
                  />
               </b-col>

               <b-col sm="12" md="3" class="mb-1">
                  <form-select
                     v-model="Data.needChamberServiceGroupId"
                     :options="needChamberServiceGroupList"
                     required-star
                     :label="$t('NeedChamberServiceGroup')"
                  />
               </b-col>
               <b-col sm="12" md="3" class="mb-1">
                  <employee-manage-select
                     :employee="Data.employeeFull"
                     v-model="Data.employeeManageId"
                     :label="$t('employeeManage')"
                     :name="$t('employeeManage')"
                     @update:data="onUpdateEmployeeManage"
                  />
               </b-col>

               <b-col sm="12" md="3" class="mb-1">
                  <form-select
                     v-model="Data.meetingTypeId"
                     :options="MeetingTypeSelectList"
                     :label="$t('meetingType')"
                     :name="$t('meetingType')"
                     :placeholder="$t('meetingType')"
                  />
               </b-col>
               <b-col sm="12" md="3" class="mb-1">
                  <form-select
                     v-model="Data.servicePriceTypeId"
                     :options="ServicePriceTypeSelectList"
                     label="servicePriceType"
                     :name="$t('servicePriceType')"
                     placeholder="servicePriceType"
                  />
               </b-col>
               <b-col sm="12" md="2" class="mb-1">
                  <b-form-checkbox class="mt-2" v-model="Data.isOnlyWorkingEmployee" name="check-button" switch>
                     {{ $t('canPayDivided') }}
                  </b-form-checkbox>
               </b-col>
               <b-col sm="12" md="2" class="mb-1">
                  <b-form-checkbox class="mt-2" v-model="Data.isOffer" name="check-button" switch>
                     {{ $t('isOffer') }}
                  </b-form-checkbox>
               </b-col>
               <b-col sm="12" md="2" class="mb-1">
                  <form-select :options="StateList" v-model="Data.stateId" label="Status"></form-select>
               </b-col>

               <b-col sm="12" md="12" class="mb-1">
                  <vue-editor v-model="Data.details" :label="$t('details')" :name="$t('details')" />
               </b-col>
               <b-col cols="12">
                  <h6 class="inputTitle">{{ $t('files') }}</h6>
                  <b-form-file type="file" :placeholder="$t('Faylni tanlang')" @change="UploadFile"></b-form-file>
               </b-col>
               <b-col v-for="item in Data.files" :key="item.id">
                  <b-img
                     :src="axios.defaults.baseURL + '/hrm/NeedChamberService/DownloadFile/' + item.id"
                     width="150"
                     height="150"
                  />
                  <a
                     :href="axios.defaults.baseURL + '/hrm/NeedChamberService/DownloadFile/' + item.id"
                     download
                     class="mt-1"
                     target="_blank"
                  >
                     {{ item.id }}
                  </a>
                  <b-button variant="danger" class="ml-1 cursor-pointer" @click="FileDelete(item.id)">
                     <feather-icon icon="Trash2Icon"></feather-icon>
                  </b-button>
               </b-col>
               <!-- save button -->
               <b-col cols="12" class="text-right mt-2">
                  <b-button :disabled="saveLoading" @click="SaveData" variant="outline-success">
                     <feather-icon icon="CheckIcon"></feather-icon>
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
import NeedChamberServiceService from '@/services/hrm/needchamberservice.service';
// components
import { BOverlay, BCard, BRow, BLink, BImg, BCol, BFormFile, BButton, BFormCheckbox } from 'bootstrap-vue';
import FormInputTranslate from '@/components/translates/FormInputTranslate.vue';
import ManualService from '@/services/others/manual.service';
import EmployeeManageSelect from '@/views/components/hrm/EmployeeManageSelect.vue';
import axios from 'axios';
import { VueEditor } from 'vue2-editor';
import NeedChamberServiceGroupService from '@/services/srv/needchamberservicegroup.service';

export default {
   components: {
      BOverlay,
      BCard,
      BRow,
      BLink,
      BFormFile,
      BImg,
      BCol,
      BButton,
      BFormCheckbox,
      FormInputTranslate,
      EmployeeManageSelect,
      VueEditor
   },
   name: 'Edit',
   data() {
      return {
         axios,
         show: false,
         StateList: [],
         saveLoading: false,
         MeetingTypeSelectList: [],
         ServicePriceTypeSelectList: [],
         needChamberServiceGroupList: [],
         Data: {
            id: 0,
            code: '',
            orderCode: '',
            shortName: '',
            fullName: '',
            canPayDivided: true,
            isOffer: true,
            servicePriceTypeId: 0,
            details: '',
            meetingTypeId: 0,
            employeeManageId: 0,
            needChamberServiceGroupId: 0,
            files: [],
            translates: []
         }
      };
   },
   created() {
      this.show = true;
      NeedChamberServiceService.Get(this.$route.params.id)
         .then((res) => {
            this.Data = res.data;
         })
         .catch((error) => {
            this.makeToast(error.response.data.errors, 'danger');
         })
         .finally(() => {
            this.show = false;
         });

      ManualService.MeetingTypeSelectList().then((res) => {
         this.MeetingTypeSelectList = res.data;
      });

      ManualService.ServicePriceTypeSelectList().then((res) => {
         this.ServicePriceTypeSelectList = res.data;
      });
      NeedChamberServiceGroupService.GetAsSelectList()
         .then((res) => {
            this.needChamberServiceGroupList = res.data;
         })
         .finally(() => {
            this.isBusy = false;
         });

      ManualService.StateSelectList()
         .then((res) => {
            this.StateList = res.data;
         })
         .catch((error) => {
            this.showApiError(error);
         });
   },
   methods: {
      UploadFile(event) {
         const formData = new FormData();
         formData.append('files', event.target.files[0]);
         this.fileLoading = true;
         NeedChamberServiceService.UploadFiles(formData)
            .then((res) => {
               this.Data.files.push(...res.data);
            })
            .finally(() => {
               this.fileLoading = false;
            });
      },
      onUpdateEmployeeManage(e) {
         this.Data.employeeId = e?.employeeId;
         this.Data.employeeFull = e?.employee;
      },
      SaveData() {
         this.$refs.ValidationDTO.validate().then((success) => {
            if (success) {
               this.saveLoading = true;
               NeedChamberServiceService.Update(this.Data)
                  .then(() => {
                     this.makeToast(this.$t('SaveSuccess'), 'success');
                     this.$router.push({ name: 'NeedChamberService' });
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

<style>
.col-form-label,
label {
   line-height: initial;
}
</style>
