<template>
   <div class="container">
      <b-overlay :show="show">
         <b-card>
            <validation-observer ref="ValidationDTO">
               <b-row>
                  <b-col sm="12" md="12" class="text-center mb-2"
                     ><h2>{{ $t('Mediation') }}</h2></b-col
                  >
                  <b-col sm="12" md="4">
                     <h4>{{ $t('docnumber') }}: {{ Data.docNumber }}</h4>
                  </b-col>
                  <b-col sm="12" md="4"> </b-col>
                  <b-col sm="12" md="4" class="text-right">
                     <h4>{{ $t('docOn') }}: {{ Data.docOn }}</h4>
                  </b-col>

                  <b-col sm="12" md="12">
                     <h4>
                        {{ $t('planDocNumber') }}: {{ Data.claimApplicationType }} {{ Data.planDocNumber }} -
                        {{ Data.planDocDate }}
                     </h4>
                  </b-col>
                  <b-col sm="12" md="12">
                     <h4>{{ $t('chamberPerson') }}: {{ Data.chamberPerson }}</h4>
                  </b-col>
                  <b-col sm="12" md="12">
                     <h4>{{ $t('contractor') }}: {{ Data.contractor }}</h4>
                  </b-col>
                  <b-col sm="12" md="12" class="mt-2">
                     <h4>{{ $t('Javobgarlar') }}</h4>
                     <b-table
                        class="mt-2"
                        :fields="TablesField"
                        hover
                        bordered
                        show-empty
                        :empty-text="$t('NotFound')"
                        small
                        responsive="sm"
                        :items="Data.table"
                     >
                        <template #cell(isRegistred)="{ item }">
                           <div>
                              {{ item.isRegistred ? $t('yes') : $t('no') }}
                           </div>
                        </template>
                     </b-table>
                  </b-col>
                  <b-col sm="12" md="12">
                     <h4>{{ $t('ClaimTheme') }}: {{ Data.claimTheme }}</h4>
                  </b-col>
                  <b-col sm="12" md="12">
                     <h4>{{ $t('mediationResult') }}: {{ Data.mediationResult }}</h4>
                  </b-col>
                  <b-col sm="12" md="12">
                     <h4>{{ $t('meetingType') }}: {{ Data.meetingType }}</h4>
                  </b-col>
               </b-row>
               <b-row>
                  <b-col sm="12" md="6">
                     <form-input-hrm
                        v-model="Data.claimantPersonName"
                        :label="$t('davogar')"
                        :placeholder="$t('davogar')"
                     />
                  </b-col>
                  <b-col sm="12" md="6">
                     <form-input-hrm
                        v-model="Data.responsiblePersonName"
                        :label="$t('javobgar')"
                        :placeholder="$t('javobgar')"
                     />
                  </b-col>
                  <b-col sm="12" md="6">
                     <form-textarea
                        v-model="Data.contractorDetails"
                        type="textarea"
                        required
                        :label="$t('contractorDetails')"
                        :placeholder="$t('contractorDetails')"
                        :disabled="isComponent"
                     />
                  </b-col>
                  <b-col sm="12" md="6">
                     <form-textarea
                        required
                        v-model="Data.responsibleDetails"
                        :label="$t('responsibleDetails')"
                        :placeholder="$t('responsibleDetails')"
                        :disabled="isComponent"
                     />
                  </b-col>
               </b-row>
               <b-row class="mt-2">
                  <b-col sm="12" md="4">
                     <form-select
                        v-model="Data.mediationResultId"
                        :options="MediationResultList"
                        required-star
                        :label="$t('mediationResult')"
                        @change="ChangeMediationResult"
                        :disabled="isComponent"
                     />
                  </b-col>
                  <b-col sm="12" md="4">
                     <form-select
                        v-model="Data.claimNeedCourtId"
                        :options="ClaimNeedCourtList"
                        disabled
                        required-star
                        :label="$t('claimNeedCourt')"
                     />
                  </b-col>
                  <b-col sm="12" md="4" v-if="Data.mediationResultId == 3 && Data.claimNeedCourtId == 3">
                     <form-picker
                        v-model="Data.courtAt"
                        required
                        type="datetime"
                        format="DD.MM.YYYY HH:mm:ss"
                        :label="$t('courtAt')"
                        :placeholder="$t('courtAt')"
                        :disabled="isComponent"
                     />
                  </b-col>
               </b-row>

               <b-row>
                  <b-col md="4" sm="4" class="mt-2">
                     <h6 class="inputTitle">{{ $t('files') }}</h6>
                     <b-form-file
                        :disabled="isComponent"
                        type="file"
                        :placeholder="$t('Faylni tanlang')"
                        @change="UploadFile"
                     ></b-form-file>
                     <div class="mt-1" v-for="item in Data.files" :key="item.id">
                        <b-link variant="primary" target="_blank" :href="FileSrc(item.id)">{{
                           item.fileName || item.id
                        }}</b-link>

                        <b-button
                           variant="danger"
                           v-if="!isComponent"
                           size="sm"
                           class="ml-1"
                           @click="DeleteFile(item.id)"
                        >
                           <b-icon-trash scale="0.7" />
                        </b-button>
                     </div>
                  </b-col>

                  <b-col v-if="!isComponent" sm="12" md="6" lg="6" align-self="end" offset-md="6" class="text-right">
                     <b-button :disabled="saveLoading" @click="SaveData" variant="outline-success">
                        <feather-icon icon="CheckIcon"></feather-icon>
                        {{ $t('Save') }}
                     </b-button>
                  </b-col>
               </b-row>
            </validation-observer>
         </b-card>
      </b-overlay>
   </div>
</template>
<script>
// service
import MediationService from '@/services/document/mediation.service';
// components
import { BOverlay, BCard, BRow, BCol, BTable, BButton, BLink, BFormFile, BIconTrash } from 'bootstrap-vue';
import ManualService from '@/services/others/manual.service';
import axios from 'axios';

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
      BIconTrash
   },
   name: 'MediationEdit',
   props: {
      isComponent: {
         type: Boolean,
         default: false
      }
   },
   data() {
      return {
         show: false,
         ApplicationList: [],
         MediationResultList: [],
         ClaimNeedCourtList: [],
         loadingButton: false,
         TablesField: [
            {
               key: 'orderNumber',
               label: this.$t('orderCode'),
               tdClass: 'text-center',
               thClass: 'text-center'
            },
            {
               key: 'claimResponsibleTypeId',
               label: this.$t('type'),
               tdClass: 'text-center',
               thClass: 'text-center'
            },

            {
               key: 'innOrPinfl',
               label: this.$t('inn'),
               tdClass: 'text-center',
               thClass: 'text-center'
            },
            {
               key: 'fullName',
               label: this.$t('fullname'),
               tdClass: 'text-center',
               thClass: 'text-center'
            },
            {
               key: 'address',
               label: this.$t('address'),
               tdClass: 'text-center',
               thClass: 'text-center'
            },
            {
               key: 'phoneNumber',
               label: this.$t('phoneNumber'),
               tdClass: 'text-center',
               thClass: 'text-center'
            },
            {
               key: 'isRegistred',
               label: this.$t('isRegistred'),
               tdClass: 'text-center',
               thClass: 'text-center'
            }
         ],
         saveLoading: false,
         fileLoading: false,
         Data: {
            docOn: '',
            docNumber: '',
            mediationPlanId: null,
            contractorDetails: '',
            responsibleDetails: '',
            mediationResultId: null,
            responsiblePersonName: '',
            claimantPersonName: '',
            claimNeedCourtId: null,
            courtAt: '',
            contractorId: null,
            files: []
         }
      };
   },
   computed: {
      FileSrc() {
         return (id) => axios.defaults.baseURL + `Mediation/DownloadFile/${id}`;
      }
   },
   created() {
      this.show = true;
      if (this.$route.query.mediationPlanId) {
         MediationService.GetByPlanId(this.$route.query.mediationPlanId)
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
         MediationService.Get(this.$route.params.id)
            .then((res) => {
               this.Data = res.data;
            })
            .catch((error) => {
               this.makeToast(error, 'danger');
            })
            .finally(() => {
               this.show = false;
            });
      }

      ManualService.ClaimNeedCourtSelectList().then((res) => {
         this.ClaimNeedCourtList = res.data;
      });

      ManualService.MediationResultSelectList().then((res) => {
         this.MediationResultList = res.data;
      });
   },
   methods: {
      ChangeMediationResult(item) {
         this.Data.claimNeedCourtId = item;
      },
      UploadFile(event) {
         const formData = new FormData();
         formData.append('files', event.target.files[0]);
         this.fileLoading = true;
         MediationService.UploadFiles(formData).then((res) => {
            this.Data.files.push(...res.data);
            this.fileLoading = false;
         });
      },
      DeleteFile(id) {
         MediationService.DeleteFile(id).then(() => {
            this.Data.files = this.Data.files.filter((item) => item.id != id);
         });
      },
      SaveData() {
         this.$refs.ValidationDTO.validate().then((success) => {
            if (success) {
               this.saveLoading = true;
               MediationService.Update(this.Data)
                  .then((res) => {
                     this.makeToast(this.$t('SaveSuccess'), 'success');
                     this.$router.push({ name: 'Mediation' });
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
