<template>
   <b-overlay :show="show">
      <b-card>
         <validation-observer ref="ValidationDTO">
            <b-row>
               <b-col sm="12" md="4">
                  <div class="form-group">
                     <form-input v-model="Data.docNumber" :disabled="disabled" required :label="$t('docnumber')" />
                  </div>
               </b-col>
               <b-col sm="12" md="4">
                  <form-picker
                     v-model="Data.docOn"
                     :disabled="disabled"
                     required
                     :label="$t('docOn')"
                     :placeholder="$t('docOn')"
                  />
               </b-col>
               <b-col sm="12" md="4">
                  <form-input
                     v-model="Data.details"
                     :disabled="disabled"
                     :label="$t('details')"
                     :placeholder="$t('details')"
                  />
               </b-col>
            </b-row>
            <hr />
            <b-row>
               <b-col cols="12">
                  <h5 class="card-title">{{ $t('chairmen') }}</h5>
               </b-col>
               <b-col sm="12" md="4">
                  <div class="form-group">
                     <form-select
                        :disabled="disabled"
                        v-model="Data.chairmenOrganizationId"
                        :options="OrganizationGroupSelectList"
                        required-star
                        :label="$t('organization')"
                     />
                  </div>
               </b-col>
               <b-col sm="12" md="4">
                  <form-select
                     :disabled="disabled"
                     v-model="Data.chairmenPositionId"
                     :options="PositionList"
                     required-star
                     :label="$t('position')"
                     :placeholder="$t('position')"
                  />
               </b-col>
               <b-col sm="12" md="4">
                  <Userselect
                     v-model="Data.chairmenId"
                     :disabled="disabled"
                     required-star
                     :organization-id="Data.chairmenOrganizationId"
                     :label="$t('fio')"
                     :placeholder="$t('fio')"
                     @option:selected="SelectChamber"
                     @input="InputChamber"
                     :roleId="3"
                  />
               </b-col>
            </b-row>
            <hr />
            <b-row>
               <b-col cols="12">
                  <h5 class="card-title">{{ $t('member') }}</h5>
               </b-col>
            </b-row>
            <b-row v-for="i in 4" :key="i + 'member'">
               <b-col sm="12" md="4">
                  <div class="form-group">
                     <form-select
                        :disabled="disabled"
                        v-model="Data[`member${i}OrganizationId`]"
                        :options="OrganizationGroupSelectList"
                        :required-star="i == 1 ? true : false"
                        :label="$t('organization')"
                     />
                  </div>
               </b-col>
               <b-col sm="12" md="4">
                  <form-select
                     :disabled="disabled"
                     v-model="Data[`member${i}PositionId`]"
                     :options="PositionList"
                     :required-star="i == 1 ? true : false"
                     :label="$t('position')"
                     :placeholder="$t('position')"
                  />
               </b-col>
               <b-col sm="12" md="4">
                  <Userselect
                     v-model="Data[`member${i}Id`]"
                     :disabled="disabled"
                     :required-star="i < 3"
                     :organization-id="Data[`member${i}OrganizationId`]"
                     :label="$t('fio')"
                     :placeholder="$t('fio')"
                     @option:selected="(value) => SelectMemeber(value, i)"
                     @input="(value) => InputMemeber(value, i)"
                  />
                  <!-- <form-input-hrm
                     :disabled="disabled"
                     v-model="Data[`member${i}Fio`]"
                     :rules="i == 1 ? 'required' : null"
                     :label="$t('fio')"
                     :placeholder="$t('fio')"
                  /> -->
               </b-col>
            </b-row>
            <b-row>
               <b-col md="6" sm="12">
                  <h6 class="inputTitle">{{ $t('files') }}</h6>
                  <b-form-file
                     :disabled="disabled"
                     type="file"
                     :placeholder="$t('Faylni tanlang')"
                     @change="UploadFile"
                  ></b-form-file>
               </b-col>
            </b-row>
            <b-row class="mt-1">
               <b-col>
                  <b-link class="mt-1" v-for="item in Data.files" :key="item.id">
                     <b-img
                        :src="axios.defaults.baseURL + '/corruption/JoinAntiCorruptionResult/DownloadFile/' + item.id"
                        width="150"
                        height="150"
                     />
                     {{ item.fileName }}
                     <b-button variant="danger" class="ml-1 cursor-pointer" @click="FileDelete(item.id)">
                        <feather-icon icon="Trash2Icon"></feather-icon>
                     </b-button>
                  </b-link>
               </b-col>
            </b-row>
         </validation-observer>
      </b-card>

      <b-card>
         <validation-observer ref="ValidationTabrow">
            <b-row>
               <b-col sm="12" md="4">
                  <JoinAntiCorruptionAppSelect
                     v-model="tabrow.applicationId"
                     required
                     :appName="name"
                     withoutCertificate
                     @update:data="updateApplication"
                  />
               </b-col>
               <b-col sm="12" md="4">
                  <form-select
                     :disabled="disabled"
                     v-model="tabrow.joinAntiCorruptionResultTypeId"
                     :options="JoinAntiCorruptionResultTypeSelectList"
                     :label="$t('JoinAntiCorruptionResultType')"
                     required-star
                  />
               </b-col>
               <template v-if="tabrow.joinAntiCorruptionResultTypeId == 1">
                  <b-col sm="12" md="4">
                     <form-input-hrm
                        disabled
                        required-star
                        required
                        rules="required"
                        v-model="tabrow.corruptionCertificateNumber"
                        :label="$t('corruptionCertificateNumber')"
                     />
                  </b-col>
                  <b-col sm="12" md="4">
                     <form-picker
                        disabled
                        v-model="tabrow.corruptionCertificateOn"
                        :label="$t('corruptionCertificateOn')"
                        :placeholder="$t('corruptionCertificateOn')"
                     />
                  </b-col>
                  <b-col sm="12" md="4">
                     <form-picker
                        :disabled="disabled"
                        v-model="tabrow.corruptionCertificateExpireOn"
                        :label="$t('corruptionCertificateExpireOn')"
                        :placeholder="$t('corruptionCertificateExpireOn')"
                     />
                  </b-col>
               </template>
               <b-col sm="12" md="1" class="mt-2">
                  <b-button :disabled="disabled" variant="primary" @click="AddTabrow">
                     <feather-icon icon="PlusIcon" size="14"></feather-icon>
                  </b-button>
               </b-col>
            </b-row>
         </validation-observer>

         <b-table
            class="mt-2"
            :fields="TablesField"
            hover
            bordered
            show-empty
            :empty-text="$t('NotFound')"
            small
            responsive="sm"
            :items="Data.tables"
         >
            <template #cell(application)="{ item }">
               {{ item.docNumber }}
            </template>
            <template #cell(contractorInn)="{ item }">
               <span style="color: blue"> {{ item.contractorInn }}</span> -{{ item.contractor }}
            </template>
            <template #cell(corruptionCertificateOn)="{ item }">
               {{ item.joinAntiCorruptionResultTypeId == 2 ? '' : item.corruptionCertificateOn }}
            </template>
            <template #cell(corruptionCertificateNumber)="{ item }">
               {{ item.joinAntiCorruptionResultTypeId == 2 ? '' : item.corruptionCertificateNumber }}
            </template>
            <template #cell(actions)="{ item, index }">
               <div class="text-center">
                  <b-link>
                     <feather-icon style="margin-right: 5px" @click="EditTabrow(item)" icon="EditIcon"></feather-icon>
                  </b-link>
                  <b-link class="text-danger">
                     <feather-icon @click="DeleteTabrow(index)" icon="Trash2Icon"></feather-icon>
                  </b-link>
               </div>
            </template>
         </b-table>

         <b-row class="mt-3 justify-content-end" v-if="!disabled">
            <b-col sm="12" md="6" lg="6" class="text-right">
               <b-button :disabled="saveLoading" @click="SaveData" variant="outline-success">
                  <feather-icon icon="CheckIcon"></feather-icon>
                  {{ $t('Save') }}
               </b-button>
            </b-col>
         </b-row>
      </b-card>
   </b-overlay>
</template>
<script>
// service
import JoinAntiCorruptionResultService from '@/services/corruption/joinanticorruptionresult.service';
// components

import { BOverlay, BCard, BRow, BCol, BTable, BButton, BLink, BFormFile, BBadge, BImg } from 'bootstrap-vue';
import PositionService from '@/services/info/position.service';
import ManualService from '@/services/others/manual.service';

import axios from 'axios';
import JoinAntiCorruptionAppSelect from '@/views/components/corruption/JoinAntiCorruptionAppSelect.vue';
import Userselect from './components/userselect.vue';

const today = new Date();

// Sana qismlarini ajratib olish
const day = String(today.getDate()).padStart(2, '0'); // Kun
const month = String(today.getMonth() + 1).padStart(2, '0'); // Oy (0-indexed)
const year = today.getFullYear();
const formattedDate = `${day}.${month}.${year}`;
const defaultTableRow = {
   id: 0,
   applicationId: null,
   application: null,
   joinAntiCorruptionResultTypeId: null,
   joinAntiCorruptionResultType: null,
   corruptionCertificateNumber: '',
   corruptionCertificateOn: formattedDate,
   corruptionCertificateExpireOn: '',
   applicationDocNumber: '',
   contractorInn: '',
   contractor: ''
};

export default {
   components: {
      BOverlay,
      BCard,
      BRow,
      BCol,
      BButton,
      BTable,
      BLink,
      BFormFile,
      BBadge,
      BImg,
      JoinAntiCorruptionAppSelect,
      Userselect
   },
   name: 'JoinAntiCorruptionResultEdit',
   data() {
      return {
         disabled: this.$route.query.view == 'true',
         axios,
         name: '',
         show: false,
         OrganizationList: [],
         OrganizationGroupSelectList: [],
         PositionList: [],
         JoinAntiCorruptionResultTypeSelectList: [],
         loadingButton: false,
         saveLoading: false,
         fileLoading: false,
         Data: {
            docNumber: '',
            docOn: '',
            details: '',
            chairmenId: null,
            chairmenOrganizationId: null,
            chairmenPositionI: null,
            chairmenFio: 'string',
            member1Id: 0,
            member1OrganizationId: 2147483647,
            member1PositionId: 2147483647,
            member1Fio: 'string',
            member2Id: 0,
            member2OrganizationId: 0,
            member2PositionId: 0,
            member2Fio: 'string',
            member3Id: 0,
            member3OrganizationId: 0,
            member3PositionId: 0,
            member3Fio: 'string',
            member4Id: 0,
            member4OrganizationId: 0,
            member4PositionId: 0,
            member4Fio: '',
            files: [],
            tables: []
         },
         tabrow: { ...defaultTableRow },
         TablesField: [
            {
               key: 'applicationDocNumber',
               label: this.$t('Application')
            },
            {
               key: 'joinAntiCorruptionResultType',
               label: this.$t('JoinAntiCorruptionResultType')
            },
            {
               key: 'contractorInn',
               label: this.$t('contractor')
            },
            {
               key: 'corruptionCertificateNumber',
               label: this.$t('corruptionCertificateNumber')
            },
            {
               key: 'corruptionCertificateOn',
               label: this.$t('corruptionCertificateOn')
            },
            {
               key: 'corruptionCertificateExpireOn',
               label: this.$t('corruptionCertificateExpireOn')
            },
            {
               key: 'actions',
               label: this.$t('actions'),
               thClass: 'text-center',
               tdClass: 'text-center'
            }
         ]
      };
   },
   created() {
      this.show = true;
      JoinAntiCorruptionResultService.Get(this.$route.params.id)
         .then((res) => {
            this.Data = res.data;
            this.Data.chairmenOrganizationId = 1;
            if (isNaN(res.data.docNumber)) {
               this.tabrow.corruptionCertificateNumber = res.data.docNumber + 1;
            } else {
               this.tabrow.corruptionCertificateNumber = Number(res.data.docNumber) + 1 + '';
            }

            if (!Array.isArray(res.data?.tables)) {
               this.Data.tables = [];
            }
            if (!Array.isArray(res.data?.files)) {
               this.Data.files = [];
            }
            if (!res.data?.details) {
               this.Data.details = '';
            }
         })
         .catch((error) => {
            this.makeToast(error.response.data.errors, 'danger');
         })
         .finally(() => {
            this.show = false;
         });

      ManualService.OrganizationAsSelectListByGroup([1, 3]).then((res) => {
         this.OrganizationList = res.data;
      });
      ManualService.OrganizationCorruptionGroupSelectList().then((res) => {
         this.OrganizationGroupSelectList = res.data;
      });

      PositionService.GetAsSelectList().then((res) => {
         this.PositionList = res.data;
      });

      ManualService.JoinAntiCorruptionResultTypeSelectList().then((res) => {
         this.JoinAntiCorruptionResultTypeSelectList = res.data;
      });
   },
   methods: {
      updateApplication(e) {
         this.name = e.application.contractor;
         this.tabrow.contractor = e.application.contractor;
         this.tabrow.contractorInn = e.application.contractorInn;
         this.tabrow.applicationDocNumber = e.application.docNumber;
         console.log(this.tabrow, 'ddd');
      },
      UploadFile(event) {
         const formData = new FormData();
         formData.append('files', event.target.files[0]);
         this.fileLoading = true;
         JoinAntiCorruptionResultService.UploadFiles(formData)
            .then((res) => {
               this.Data.files.push(...res.data);
            })
            .finally(() => {
               this.fileLoading = false;
            });
      },
      FileDelete(id) {
         JoinAntiCorruptionResultService.DeleteFile(id).then(() => {
            this.Data.files = this.Data.files.filter((item) => item.id != id);
         });
      },
      DownloadFile(id) {
         JoinAntiCorruptionResultService.DownloadFile(id).then(() => {});
      },
      DeleteTabrow(index) {
         this.Data.tables.splice(index, 1);
      },
      EditTabrow(item) {
         this.editedIndex1 = this.Data.tables.indexOf(item);
         this.tabrow = Object.assign({}, item);
      },
      AddTabrow() {
         this.$refs.ValidationTabrow.validate().then((success) => {
            if (success) {
               this.tabrow.joinAntiCorruptionResultType = this.JoinAntiCorruptionResultTypeSelectList.find(
                  (e) => e.value == this.tabrow?.joinAntiCorruptionResultTypeId
               )?.text;
               if (this.editedIndex1 > -1) {
                  Object.assign(this.Data.tables[this.editedIndex1], this.tabrow);
               } else {
                  this.Data.tables.push(this.tabrow);
               }
               this.$refs.ValidationTabrow.reset();
               this.tabrow = { ...defaultTableRow };
               this.name = '';

               if (!(this.editedIndex1 > -1)) {
                  if (isNaN(this.Data.tables[this.Data.tables.length - 1].corruptionCertificateNumber)) {
                     this.tabrow.corruptionCertificateNumber =
                        this.Data.tables[this.Data.tables.length - 1].corruptionCertificateNumber +
                        Math.trunc(Math.random() * 10);
                  } else {
                     this.tabrow.corruptionCertificateNumber =
                        Number(this.Data.tables[this.Data.tables.length - 1].corruptionCertificateNumber) + 1 + '';
                  }
               }
            }
         });
      },
      SaveData() {
         this.$refs.ValidationDTO.validate().then((success) => {
            if (success) {
               this.saveLoading = true;
               JoinAntiCorruptionResultService.Update(this.Data)
                  .then((res) => {
                     this.makeToast(this.$t('SaveSuccess'), 'success');
                     this.$router.push({ name: 'JoinAntiCorruptionResult' });
                  })
                  .catch((err) => {
                     this.showApiError(err);
                  })
                  .finally(() => {
                     this.saveLoading = false;
                  });
            }
         });
      },
      SelectMemeber(value, i) {
         console.log(this.Data);

         this.Data[`member${i}Id`] = value.value;
         this.Data[`member${i}Fio`] = value.text;
      },
      InputMemeber(value, i) {
         if (!value) {
            this.Data[`member${i}Fio`] = '';
         }
      },
      SelectChamber(e) {
         console.log(e, 'SelectChamber');
         this.Data.chairmenId = e.value;
         this.Data.chairmenFio = e.text;
      },
      InputChamber(e) {
         if (!e) {
            this.Data.chairmenFio = '';
         }
      }
   }
};
</script>
