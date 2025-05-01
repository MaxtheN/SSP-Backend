<template>
   <b-overlay :show="show">
      <b-card>
         <DocTabs pdf-title="Appeal" view-title="Info" :value="1">
            <template #pdf>
               <WIframe :src="IframeSrc" style="height: 100vh" :show="Data?.id" />
            </template>
            <template #view>
               <validation-observer ref="ValidationDTO">
                  <validation-observer ref="ValidationDTO2">
                     <b-row>
                        <b-col sm="12" md="2" class="ma-0">
                           <form-select
                              :disabled="isdisabled"
                              :options="PERSON_TYPE"
                              v-model="personType"
                           ></form-select>
                        </b-col>
                        <template v-if="personType == 2">
                           <b-col sm="12" md="2">
                              <form-select
                                 :options="IdentityDocumentList"
                                 v-model="filter.documentTypeId"
                              ></form-select>
                           </b-col>
                           <b-col sm="12" md="2">
                              <form-input-hrm
                                 :disabled="isdisabled"
                                 v-model="filter.passportSeria"
                                 :value="filter.passportSeria"
                                 @input="(val) => (filter.passportSeria = (val || '').toUpperCase())"
                                 :label="$t('documentSeria')"
                                 :placeholder="$t('documentSeria')"
                                 v-mask="'AA'"
                              />
                           </b-col>
                           <b-col sm="12" md="2">
                              <form-input-hrm
                                 :disabled="isdisabled"
                                 v-model="filter.passportNumber"
                                 :label="$t('passportNumber')"
                                 :placeholder="$t('passportNumber')"
                              />
                           </b-col>
                           <b-col sm="12" md="3">
                              <form-picker
                                 v-model="filter.birthDate"
                                 :disabled="isdisabled"
                                 :placeholder="$t('birthDate')"
                                 :label="$t('birthDate')"
                                 value-type="format"
                                 format="DD.MM.YYYY"
                                 @keyup.enter="getPersonData"
                              ></form-picker>
                           </b-col>
                        </template>
                        <template v-else>
                           <b-col sm="12" md="2">
                              <form-input
                                 :disabled="isdisabled"
                                 v-model="Data.contractorInn"
                                 :label="$t('tin')"
                                 :placeholder="$t('tin')"
                                 @keyup.enter="getPersonData"
                                 required
                              />
                           </b-col>
                        </template>

                        <b-col class="col-auto">
                           <b-button
                              @click="getPersonData"
                              :disabled="soliqLoading"
                              class="mt-2"
                              variant="primary"
                              size="sm"
                              v-if="!isdisabled"
                           >
                              <b-spinner v-if="soliqLoading" style="height: 1.5rem; width: 1.5rem" color="primary" />
                              <feather-icon v-else icon="SearchIcon" size="21" />
                           </b-button>
                        </b-col>
                        <b-col cols="auto">
                           <b-button
                              @click="dialog = true"
                              :disabled="sendtoEDOC"
                              v-if="Data.canSendToEdoc"
                              class="mt-2"
                              variant="primary"
                              size="md"
                           >
                              {{ $t('SendToEdoc') }}
                           </b-button>

                           <!-- edoc modal -->

                           <b-modal no-close-on-backdrop size="md" v-model="dialog" :title="$t('SendToEdoc')">
                              <form-select
                                 v-model="organizationId"
                                 :options="OrganizationList"
                                 :label="$t('organization')"
                              />
                              <template #modal-footer>
                                 <b-button
                                    @click="dialog = false"
                                    :disabled="sendtoEDOC"
                                    class="mt-2"
                                    variant="danger"
                                    size="md"
                                 >
                                    {{ $t('Cancel') }}
                                 </b-button>
                                 <b-button
                                    @click="SendToEdoc"
                                    :disabled="organizationId ? false : true"
                                    class="mt-2"
                                    variant="primary"
                                    size="md"
                                 >
                                    {{ $t('Send') }}
                                 </b-button>
                              </template>
                           </b-modal>

                           <template v-if="Data.edocInfo">
                              <EdocInfo :edoc-info="Data.edocInfo" :status-id="Data.statusId" />
                           </template>
                        </b-col>
                        <b-col>
                           <b-button
                              v-if="Data.canReject"
                              @click="Cancel(Data)"
                              :disabled="saveLoading"
                              size="md"
                              class="mx-2 mt-2"
                              variant="outline-danger"
                           >
                              <feather-icon icon="CheckIcon"></feather-icon>
                              {{ $t('Cancel') }}
                           </b-button>
                        </b-col>
                     </b-row>
                  </validation-observer>

                  <!-- jismoniy -->
                  <template v-if="personType == 2">
                     <b-row>
                        <b-col sm="12" md="3">
                           <form-input-hrm
                              :disabled="isdisabled"
                              v-model="Data.personFullName"
                              :label="$t('fio')"
                              :placeholder="$t('fio')"
                           />
                        </b-col>
                        <b-col sm="12" md="3" v-if="Data.person">
                           <form-picker
                              v-model="Data.person.birthDate"
                              :placeholder="$t('birthDate')"
                              :label="$t('birthDate')"
                              value-type="format"
                              format="DD.MM.YYYY"
                              disabled
                           ></form-picker>
                        </b-col>
                        <b-col sm="12" md="3" v-if="Data.person">
                           <form-input-hrm
                              v-model="Data.person.pinfl"
                              :disabled="isdisabled"
                              :label="$t('pinfl')"
                              :placeholder="$t('pinfl')"
                           />
                        </b-col>
                     </b-row>
                  </template>
                  <!-- yuridik -->
                  <template v-if="personType == 1">
                     <b-row v-if="Data.contractorFulName">
                        <b-col sm="12" md="3">
                           <form-input-hrm
                              v-model="Data.contractorFulName"
                              :label="$t('contractorName')"
                              disabled
                              :placeholder="$t('contractorName')"
                           />
                        </b-col>
                        <b-col sm="12" md="3">
                           <form-input-hrm
                              v-model="Data.contractorInn"
                              :label="$t('inn')"
                              disabled
                              :placeholder="$t('inn')"
                           />
                        </b-col>
                     </b-row>
                  </template>

                  <b-row>
                     <b-col sm="12" md="3">
                        <form-select
                           :options="RegionList"
                           :disabled="isdisabled"
                           v-model="Data.regionId"
                           @input="ChangeRegion"
                           :label="$t('Oblast')"
                           required-star
                        ></form-select>
                     </b-col>
                     <b-col sm="12" md="3">
                        <form-select
                           :disabled="isdisabled"
                           :options="DistrictList"
                           v-model="Data.districtId"
                           required-star
                           :label="$t('District')"
                        ></form-select>
                     </b-col>
                     <b-col sm="12" md="3">
                        <form-input-hrm
                           :disabled="isdisabled"
                           v-model="Data.address"
                           rules="required"
                           :label="$t('Address')"
                           :placeholder="$t('Address')"
                        />
                     </b-col>
                     <b-col sm="12" md="3">
                        <form-input-hrm
                           :disabled="isdisabled"
                           v-model="Data.email"
                           rules="email"
                           :label="$t('Email')"
                           type="email"
                           :placeholder="$t('Email')"
                        />
                     </b-col>
                  </b-row>

                  <!-- director -->
                  <template v-if="personType == 1">
                     <hr />
                     <b-row>
                        <b-col sm="12" md="3">
                           <form-input-hrm
                              v-model="Data.director"
                              :label="$t('director')"
                              disabled
                              :placeholder="$t('director')"
                           />
                        </b-col>
                        <b-col sm="12" md="3">
                           <form-input-hrm
                              v-model="Data.contractorDirectorPinfl"
                              :label="$t('pinfl')"
                              disabled
                              :placeholder="$t('pinfl')"
                           />
                        </b-col>
                        <b-col sm="12" md="3">
                           <form-input-hrm
                              v-model="Data.contractorDirectorPassport"
                              :label="`${$t('passportNumber')} ${$t('passportSeries')}`"
                              disabled
                              :placeholder="`${$t('passportNumber')} ${$t('passportSeries')}`"
                           />
                        </b-col>
                        <b-col sm="12" md="3">
                           <form-input-hrm
                              v-model="Data.contractorDirectorBirthDate"
                              :label="$t('birthDate')"
                              disabled
                              :placeholder="$t('birthDate')"
                           />
                        </b-col>
                     </b-row>
                  </template>

                  <b-row>
                     <b-col sm="12" md="3">
                        <form-select
                           :options="bandlik"
                           :disabled="isdisabled"
                           v-model="Data.busyness"
                           :label="$t('employmentType')"
                        ></form-select>
                     </b-col>
                     <b-col sm="12" md="3">
                        <form-input-hrm
                           :disabled="isdisabled"
                           v-model="Data.phoneNumber"
                           v-mask="'+998 ## ### ## ##'"
                           :label="$t('phone')"
                           :placeholder="$t('phone')"
                           rules="required"
                        />
                     </b-col>
                  </b-row>
                  <hr />
                  <b-row>
                     <b-col sm="12" md="3">
                        <form-input-hrm
                           :disabled="isdisabled"
                           v-model="Data.docNumber"
                           rules="required"
                           :label="$t('docnumber')"
                           :placeholder="$t('docnumber')"
                        />
                     </b-col>
                     <b-col sm="12" md="3">
                        <form-picker v-model="Data.docOn" :disabled="isdisabled" />
                     </b-col>
                     <b-col sm="12" md="3">
                        <form-select
                           :options="openAppeal"
                           :disabled="isdisabled"
                           v-model="Data.openAppeal"
                           :label="$t('openAppeal')"
                        ></form-select>
                     </b-col>
                     <b-col sm="12" md="3">
                        <form-select
                           :options="AppealTypeSelectList"
                           v-model="Data.appealTypeId"
                           :disabled="isdisabled"
                           :label="$t('appealType')"
                           rules="required"
                           required-star
                        ></form-select>
                     </b-col>
                     <b-col sm="12" md="3">
                        <form-select
                           :disabled="isdisabled"
                           :options="AppealFormatTypeSelectList"
                           v-model="Data.appealFormatTypeId"
                           :label="$t('appealFormatType')"
                           required-star
                        ></form-select>
                     </b-col>
                     <b-col sm="12" md="3">
                        <form-select
                           :disabled="isdisabled"
                           :options="appealTypeArriveList"
                           v-model="Data.appealTypeArriveId"
                           :label="$t('AppealTypeArrive')"
                        ></form-select>
                     </b-col>
                  </b-row>
                  <b-row>
                     <b-col md="6" sm="6">
                        <h6 class="inputTitle">{{ $t('files') }}</h6>

                        <b-form-file
                           type="file"
                           accept=".pdf, .doc, .docx"
                           :placeholder="$t('fileupload')"
                           @change="UploadFile"
                           :disabled="isdisabled"
                        >
                        </b-form-file>
                        <div class="mt-1" v-for="item in Data.files" :key="item.id">
                           <b-link variant="primary" target="_blank" :href="FileSrc(item.id)">{{
                              item.fileName || item.id
                           }}</b-link>
                           <b-button
                              variant="danger"
                              :disabled="isdisabled"
                              size="sm"
                              class="ml-1"
                              @click="DeleteFile(item.id)"
                           >
                              <b-icon-trash scale="0.7" />
                           </b-button>
                        </div>
                     </b-col>
                     <b-col sm="12" md="12" class="mt-1">
                        <form-textarea
                           required
                           v-model="Data.details"
                           :disabled="isdisabled"
                           rows="2"
                           max-rows="6"
                           :label="$t('appealText')"
                           :placeholder="$t('appealText')"
                        />
                     </b-col>
                  </b-row>
                  <b-row class="mt-2" v-if="!isView">
                     <b-col sm="12" md="6" lg="6" class="text-left"></b-col>
                     <b-col sm="12" md="6" lg="6" class="text-right">
                        <b-button
                           :disabled="saveLoading"
                           v-if="!isdisabled"
                           @click="SaveData"
                           size="sm"
                           variant="outline-success"
                        >
                           <feather-icon icon="CheckIcon"></feather-icon>
                           {{ $t('Save') }}
                        </b-button>
                     </b-col>
                  </b-row>
               </validation-observer>
            </template>
         </DocTabs>
      </b-card>
   </b-overlay>
</template>
<script>
import axios from 'axios';
// service
import ManualService from '@/services/others/manual.service';
import AppealApplicationService from '@/services/appeal/AppealApplication.service';
import AppealDescriptionService from '@/services/appeal/AppealDescription.service';
import AppealTypeArriveService from '@/services/appeal/AppealTypeArrive.service';
import EdocInfo from '@/views/components/appeal/EdocInfo.vue';

// components
import {
   BOverlay,
   BCard,
   BRow,
   BCol,
   BButton,
   BFormFile,
   BSpinner,
   BFormTextarea,
   BModal,
   BIconTrash,
   BLink
} from 'bootstrap-vue';
import FormInputTranslate from '@/components/translates/FormInputTranslate.vue';
import PrtnCertificateService from '@/services/document/prtncertificate.service';
import PersonService from '@/services/others/person.service';
import DepartmentService from '@/services/info/department.service';
import DistrictService from '@/services/info/district.service';
import RegionService from '@/services/info/region.service';
import DocTabs from '@/views/components/document/DocTabs.vue';
import WIframe from '@/components/WIframe.vue';
import EmployeeService from '@/services/info/employee.service';
import IdentityDocumentService from '@/services/info/identitydocument.service';

export default {
   components: {
      BModal,
      BOverlay,
      BCard,
      BRow,
      BCol,
      BButton,
      FormInputTranslate,
      BFormFile,
      BSpinner,
      BFormTextarea,
      BIconTrash,
      BLink,
      EdocInfo,
      DocTabs,
      WIframe
   },
   data() {
      return {
         dialog: false,
         isdisabled: false,
         sendtoEDOC: false,
         show: false,
         saveLoading: false,
         RegionList: [],
         DistrictList: [],
         StateList: [],
         DepartmentList: [],
         appealTypeArriveList: [],
         AppealFormatTypeSelectList: [],
         AppealTypeSelectList: [],
         AppealDescriptionSelectList: [],
         bandlik: [
            { text: this.$t('Ishlaydi'), value: true },
            { text: this.$t('Ishsiz'), value: false }
         ],
         openAppeal: [
            { text: this.$t('yes'), value: true },
            { text: this.$t('no'), value: false }
         ],
         filter: {
            passportSeria: '',
            passportNumber: '',
            birthDate: '',
            inn: '',
            documentTypeId: 2
         },
         personType: 1,
         PERSON_TYPE: [
            {
               text: this.$t('item1'),
               value: 1
            },
            {
               text: this.$t('item2'),
               value: 2
            }
         ],
         soliqLoading: false,
         fileLoading: false,
         OrganizationList: [],
         IdentityDocumentList: [],
         organizationId: null,
         Data: {
            personId: null,
            person: {},
            contractorInn: '',
            contractor: {},
            contractorId: null,
            contractorDirectorPinfl: null,
            contractorDirectorPassport: null,
            contractorDirectorBirthDate: null,
            contractorDirector: '',
            contractor: {
               inn: '',
               shortName: ''
            },
            region: '',
            district: '',
            docNumber: '',
            phoneNumber: '',
            docOn: '',
            details: '',
            busyness: true,
            appealTypeId: null,
            appealFormatTypeId: null,
            openAppeal: true,
            regionId: null,
            districtId: null,
            appealTypeArriveId: null,
            appealDescriptionId: null,
            departmentId: null,
            address: '',
            email: ''
         },
         iframeLoaded: false
      };
   },
   created() {
      IdentityDocumentService.GetAsSelectList()
         .then((res) => {
            this.IdentityDocumentList = res.data.filter((item) => {
               return item.value != '6' && item.value != '1' && item.value != '4';
            });
         })
         .catch((error) => {
            this.makeToast(error, 'danger');
         });
      if (this.$route.query.inn) {
         this.personType = 1;
      } else {
         this.personType = 2;
      }

      if (this.$route.query.isView) {
         this.isdisabled = true;
      }

      RegionService.GetAsSelectList(211)
         .then((res) => {
            this.RegionList = res.data;
         })
         .catch(this.showApiError)
         .finally(() => {
            this.saveLoading = false;
         });
      ManualService.AppealTypeSelectList()
         .then((res) => {
            this.AppealTypeSelectList = res.data;
         })
         .catch(this.showApiError)
         .finally(() => {
            this.show = false;
         });
      ManualService.AppealFormatTypeSelectList()
         .then((res) => {
            this.AppealFormatTypeSelectList = res.data;
         })
         .catch(this.showApiError)
         .finally(() => {
            this.show = false;
         });

      AppealTypeArriveService.GetAsSelectList().then((res) => {
         this.appealTypeArriveList = res.data;
      });
      ManualService.OrganizationAsSelectListByGroup([1, 3]).then((res) => {
         this.OrganizationList = res.data;
      });

      AppealDescriptionService.GetAsSelectList()
         .then((res) => {
            this.AppealDescriptionSelectList = res.data;
         })
         .catch(this.showApiError)
         .finally(() => {
            this.show = false;
         });

      ManualService.StateSelectList()
         .then((res) => {
            this.StateList = res.data;
         })
         .catch(this.showApiError);

      this.show = true;
      AppealApplicationService.Get(this.$route.params.id)
         .then((res) => {
            this.Data = res.data;
            if (this.Data.id == 0) {
               this.Data.appealFormatTypeId = 1;
               this.Data.busyness = true;
               this.Data.person = null;
            }
            this.ChangeRegion();
         })
         .catch(this.showApiError)
         .finally(() => {
            this.show = false;
         });

      DepartmentService.GetAsSelectList(null, {})
         .then((res) => {
            if (Array.isArray(res.data)) {
               this.DepartmentList = res.data;
            }
         })
         .catch(this.showApiError);
   },
   computed: {
      FileSrc() {
         return (id) => axios.defaults.baseURL + `AppealApplication/DownloadFile/${id}`;
      },
      isView() {
         return this.$route.query.isView == 'true';
      },
      IframeSrc() {
         return axios.defaults.baseURL + `AppealApplication/DownloadPdf?id2=${this.Data?.id}&lang=${this.getPdfLang()}`;
      }
   },
   methods: {
      ChangeRegion() {
         if (this.Data.regionId)
            DistrictService.GetAsSelectList(this.Data.regionId)
               .then((res) => {
                  this.DistrictList = res.data;
               })
               .catch(this.showApiError)
               .finally(() => {
                  this.saveLoading = false;
               });
      },
      UploadFile(event) {
         const formData = new FormData();
         formData.append('files', event.target.files[0]);
         this.fileLoading = true;
         AppealApplicationService.UploadFile(formData).then((res) => {
            this.Data.files.push(...res.data);
            this.fileLoading = false;
         });
      },
      DeleteFile(id) {
         AppealApplicationService.DeleteFile(id).then(() => {
            this.Data.files = this.Data.files.filter((item) => item.id != id);
         });
      },
      SaveData() {
         this.$refs.ValidationDTO.validate().then((success) => {
            if (success) {
               this.saveLoading = true;
               AppealApplicationService.Update(this.Data)
                  .then(() => {
                     this.makeToast(this.$t('SaveSuccess'), 'success');
                     this.$router.push({ name: 'AppealApplication' });
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
      getPersonData() {
         this.$refs.ValidationDTO2.validate().then((success) => {
            if (success) {
               this.soliqLoading = true;
               if (this.personType == 1) {
                  PrtnCertificateService.GetFromSoliq(this.Data.contractorInn)
                     .then((res1) => {
                        this.Data.person = null;
                        this.Data.personId = null;
                        this.Data.director = res1.data.director?.lastName + ' ' + res1.data.director?.firstName;
                        this.Data.contractorFulName = res1.data.company?.name;
                        this.Data.address = res1.data.company?.streetName;
                        this.Data.contractorDirectorPinfl = res1.data.director?.pinfl;
                        this.Data.contractorDirectorPassport =
                           res1.data.director?.passportSeries + ' ' + res1.data.director?.passportNumber;
                        this.Data.contractorDirectorBirthDate = res1.data.director?.birthDate;
                     })
                     .catch(this.showApiError)
                     .finally(() => {
                        this.soliqLoading = false;
                     });
               } else {
                  EmployeeService.GetByPassportDataFromDigital(
                     this.filter.passportSeria + this.filter.passportNumber,
                     this.filter.birthDate
                  )
                     .then((res) => {
                        this.Data.person = res.data;
                        this.Data.personFullName = `${res.data.surnameLatin} ${res.data.nameLatin} ${res.data.patronymLatin}`;
                        this.Data.personId = res.data.id;
                        this.Data.districtId = res.data.livingDistrictId;
                        this.Data.regionId = res.data.livingRegionId;
                        this.ChangeRegion();
                     })
                     .catch(this.showApiError)
                     .finally(() => {
                        this.soliqLoading = false;
                     });
               }
            }
         });
      },
      SendToEdoc() {
         this.sendtoEDOC = true;
         this.dialog = false;
         AppealApplicationService.SendToEdoc(this.Data.id, this.organizationId)
            .then((res) => {
               this.sendtoEDOC = false;
               this.makeToast(this.$t('SendToEdoc'), 'success');
            })
            .catch((err) => {
               this.showApiError(err);
            })
            .finally(() => {
               this.sendtoEDOC = false;
            });
      },
      Cancel(item) {
         this.$swal.fire({
            icon: 'info',
            title: this.$t('WantReject'),
            showLoaderOnConfirm: true,
            input: 'text',
            inputPlaceholder: this.$t('RejectMessage'),
            preConfirm: (msg) => {
               return AppealApplicationService.Reject({
                  id: item.id,
                  message: msg
               })
                  .then(() => {
                     this.makeToast(this.$t('RejectSuccess'), 'success');
                     this.Refresh();
                  })
                  .catch(this.SwalError);
            },
            allowOutsideClick: () => !this.$swal.isLoading()
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
