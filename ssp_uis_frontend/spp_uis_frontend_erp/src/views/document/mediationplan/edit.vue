<template>
   <b-overlay :show="show">
      <b-card>
         <validation-observer ref="ValidationDTO">
            <b-row>
               <b-col sm="12" md="12" class="text-center mb-2"
                  ><h2>{{ $t('MediationPlan') }}</h2></b-col
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
                     {{ $t('applicationDocNumber') }}: {{ Data.claimApplicationType }} {{ Data.applicaionDocNumber }} -
                     {{ Data.applicaionDocOn }}
                  </h4>
               </b-col>
               <b-col sm="12" md="12">
                  <h4>{{ $t('contractor') }}: {{ Data.contractor }}</h4>
               </b-col>
               <b-col sm="12" md="12">
                  <form-input-hrm
                     :disabled="view"
                     rules="required"
                     v-model="Data.chamberPerson"
                     :label="$t('chamberPerson')"
                     :name="$t('chamberPerson')"
                  />
               </b-col>

               <b-col sm="12" md="12" class="mb-2">
                  <form-textarea
                     :disabled="view"
                     v-model="Data.details"
                     :label="$t('details')"
                     :placeholder="$t('details')"
                  />
               </b-col>
            </b-row>

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
               :items="Data.tables"
            >
               <template #cell(isRegistred)="{ item }">
                  <div>
                     {{ item.isRegistred ? $t('yes') : $t('no') }}
                  </div>
               </template>
               <template #cell(address)="{ item }">
                  <div class="d-flex align-items-center flex-column justify-content-between">
                     <div class="d-flex align-items-center w-100">
                        <p v-if="!editAddress" class="mr-2 pb-0 mb-0">{{ item.address }}</p>
                        <b-form-input v-else v-model="item.address" class="mr-2"></b-form-input>
                        <b-link class="cursor-pointer d-flex mr-1">
                           <feather-icon
                              v-if="!editAddress && !view"
                              @click="editAddress = true"
                              icon="EditIcon"
                           ></feather-icon>

                           <feather-icon
                              v-else
                              icon="CheckIcon"
                              @click="EditAdresFortable(item)"
                              color="success"
                           ></feather-icon>
                        </b-link>
                     </div>
                  </div>
               </template>
            </b-table>

            <b-row>
               <b-col sm="12" md="6">
                  <form-select
                     :disabled="view"
                     v-model="Data.claimThemeId"
                     :options="ClaimThemeList"
                     required-star
                     :label="$t('ClaimTheme')"
                     :name="$t('ClaimTheme')"
                  />
               </b-col>
               <b-col sm="12" md="6">
                  <form-select
                     :disabled="view"
                     v-model="Data.meetingTypeId"
                     @change="changemeetingTypeId"
                     :options="MeetingTypeSelectList"
                     required-star
                     :label="$t('meetingType')"
                     :name="$t('meetingType')"
                  />
               </b-col>

               <b-col sm="12" md="6">
                  <form-picker
                     :disabled="view"
                     v-model="Data.meditionAt"
                     type="datetime"
                     format="DD.MM.YYYY HH:mm:ss"
                     :label="$t('meditionAt')"
                     :name="$t('meditionAt')"
                     required
                  />
               </b-col>

               <!-- zoom link -->
               <b-col cols="12" v-if="Data.meetingTypeId == 1">
                  <form-input-hrm
                     :disabled="view"
                     v-model="Data.addressOrUrl"
                     :rules="'required|max:250'"
                     :label="$t('addressOrUrl')"
                     :name="$t('addressOrUrl')"
                  />
               </b-col>
               <b-col cols="12" v-if="Data.meetingTypeId == 2">
                  <form-input-hrm
                     :disabled="view"
                     v-model="Data.addressOrUrl"
                     :rules="'required|max:250'"
                     :label="$t('addressOrUrlOffline')"
                     :name="$t('addressOrUrl')"
                  />
               </b-col>
            </b-row>
            <b-row class="mt-2">
               <b-col cols="12" v-if="!view" class="text-right">
                  <b-button :disabled="saveLoading" @click="SaveData" variant="outline-success">
                     <feather-icon icon="CheckIcon"></feather-icon>
                     {{ $t('Save') }}
                  </b-button>
               </b-col>
               <b-col v-if="Data.message"
                  ><b-alert variant="danger" class="p-1" show>{{ Data.message }} </b-alert></b-col
               >
            </b-row>
         </validation-observer>
      </b-card>
   </b-overlay>
</template>
<script>
// service
import ClaimApplicationService from '@/services/document/claimapplication.service';
import MediationPlanService from '@/services/document/mediationplan.service';
// components
import { BOverlay, BCard, BRow, BCol, BTable, BButton, BLink, BFormInput, BAlert } from 'bootstrap-vue';
import ManualService from '@/services/others/manual.service';
import ClaimThemeService from '@/services/info/claimtheme.service';

export default {
   props: {
      view: { type: Boolean, default: false }
   },
   components: {
      BOverlay,
      BCard,
      BRow,
      BCol,
      BButton,
      BTable,
      BLink,
      BFormInput,
      BAlert
   },
   name: 'MediationPlanEdit',
   data() {
      return {
         show: false,
         ApplicationList: [],
         MeetingTypeSelectList: [],
         MediationTypeSelectList: [], // mediatsiya yig'ilishi turi
         ClaimThemeList: [], //murojat predmeti
         loadingButton: false,
         editAddress: false,
         saveLoading: false,
         meditionAtDate: '',
         meditionAtTime: '',
         Data: {
            docNumber: '',
            docOn: '',
            applicationId: null,
            mediationTypeId: null,
            addressOrUrl: '',
            contractorId: null,
            meditionAt: ''
         },
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
         ]
      };
   },
   created() {
      this.show = true;
      if (this.$route.query.applicationId) {
         MediationPlanService.GetByApplication(this.$route.query.applicationId)
            .then((res) => {
               this.Data = res.data;
            })
            .catch((error) => {
               this.showApiError(error);
            })
            .finally(() => {
               this.show = false;
            });
      } else if (this.$route.params.id > 0) {
         MediationPlanService.Get(this.$route.params.id)
            .then((res) => {
               this.Data = res.data;
            })
            .catch((error) => {
               this.showApiError(error);
            })
            .finally(() => {
               this.show = false;
            });
      } else {
         this.$router.push({ name: 'MediationPlan' });
      }

      ClaimApplicationService.GetList({}).then((res) => {
         this.ApplicationList = res.data.rows;
      });

      ManualService.MeetingTypeSelectList().then((res) => {
         if (Array.isArray(res.data)) {
            this.MeetingTypeSelectList = res.data;
         }
      });

      ClaimThemeService.GetAsSelectList().then((res) => {
         if (Array.isArray(res.data)) {
            this.ClaimThemeList = res.data;
         }
      });
   },

   methods: {
      changemeetingTypeId() {
         this.Data.addressOrUrl = '';
      },
      SaveData() {
         this.$refs.ValidationDTO.validate().then((success) => {
            if (success) {
               this.saveLoading = true;
               MediationPlanService.Update({
                  ...this.Data
                  // meditionAt: this.meditionAtDate + ' ' + this.meditionAtTime,
               })
                  .then(() => {
                     this.makeToast(this.$t('SaveSuccess'), 'success');
                     this.$router.push({ name: 'MediationPlan' });
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
      EditAdresFortable(item) {
         this.editAddress = false;
         console.log(item);

         ClaimApplicationService.UpdateClaimApplicationTable(item.id, item.address)
            .then(() => {
               this.makeToast(this.$t('SaveSuccess'), 'success');
            })
            .catch((err) => {
               this.showApiError(err);
            })
            .finally(() => {
               this.saveLoading = false;
            });
      }
   }
};
</script>
