<template>
   <b-overlay :show="show">
      <b-card>
         <DocTabs pdf-title="Appeal" view-title="Info" :value="$route.query.isView ? 1 : 0">
            <template #view>
               <validation-observer ref="ValidationDTO">
                  <validation-observer ref="ValidationDTO2">
                     <b-row>
                        <b-col sm="12" md="3">
                           <form-select
                              :disabled="isdisabled"
                              :options="appealTypeArriveList"
                              v-model="Data.appealTypeArriveId"
                              :label="$t('AppealTypeArrive')"
                           ></form-select>
                        </b-col>
                        <b-col sm="12" md="2" class="ma-0">
                           <form-select
                              :disabled="isdisabled"
                              :options="PERSON_TYPE"
                              v-model="personType"
                           ></form-select>
                        </b-col>
                        <template v-if="personType == 2">
                           <!-- <b-col sm="12" md="1">
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
                           <b-col sm="12" md="3">
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
                           </b-col> -->
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

                        <b-col class="col-auto" v-if="personType == 1">
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
                        <b-col>
                           <b-button
                              @click="SendToEdoc"
                              :disabled="sendtoEDOC"
                              v-if="Data.canSendToEdoc"
                              class="mt-2"
                              variant="primary"
                              size="md"
                           >
                              {{ $t('SendToEdoc') }}
                           </b-button>

                           <template v-if="Data.edocInfo">
                              <EdocInfo :edoc-info="Data.edocInfo" />
                           </template>
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
                     <b-col v-if="personType == 2" sm="12" md="4">
                        <form-select
                           v-model="Data.countryId"
                           :options="CountryList"
                           :label="$t('Country')"
                           @input="ChangeCountry"
                        />
                     </b-col>
                     <b-col sm="12" md="3">
                        <form-select
                           :options="RegionList"
                           :disabled="isdisabled || chekCountry"
                           v-model="Data.regionId"
                           @input="ChangeRegion"
                           :label="$t('Oblast')"
                           :required-star="!chekCountry"
                        ></form-select>
                     </b-col>
                     <b-col sm="12" md="3">
                        <form-select
                           :disabled="isdisabled || chekCountry"
                           :options="DistrictList"
                           v-model="Data.districtId"
                           :required-star="!chekCountry"
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
                     <b-col v-if="personType == 1" cols="12" md="3">
                        <form-select :options="Okedlist" v-model="Data.okedId" label="oked"></form-select>
                     </b-col>
                     <b-col v-if="personType == 1" sm="12" md="3">
                        <form-select
                           v-model="Data.businessType"
                           :options="ContractorCategoryList"
                           :label="$t('contractorCategory')"
                        />
                     </b-col>
                     <b-col v-if="personType == 1" sm="12" md="1" class="mt-2">
                        <b-form-checkbox class="mt-1" v-model="Data.isimporter">{{ $t('isimporter') }}</b-form-checkbox>
                     </b-col>
                     <b-col v-if="personType == 1" sm="12" md="1" class="mt-2">
                        <b-form-checkbox class="mt-1" v-model="Data.isexporter">{{ $t('isexporter') }}</b-form-checkbox>
                     </b-col>
                     <b-col md="3" v-if="personType == 1"></b-col>
                  </b-row>

                  <!-- director -->
                  <template v-if="personType == 1">
                     <hr />
                     <b-row>
                        <b-col sm="12" md="3">
                           <form-input-hrm
                              v-model="Data.director"
                              :label="$t('director')"
                              :placeholder="$t('director')"
                           />
                        </b-col>
                        <b-col sm="12" md="3">
                           <form-input-hrm
                              v-model="Data.contractorDirectorPinfl"
                              :label="$t('pinfl')"
                              :placeholder="$t('pinfl')"
                           />
                        </b-col>
                        <b-col sm="12" md="3">
                           <form-input-hrm
                              v-model="Data.contractorDirectorPassportSeria"
                              :label="` ${$t('passportSeries')}`"
                              :placeholder="`${$t('passportSeries')}`"
                           />
                        </b-col>
                        <b-col sm="12" md="3">
                           <form-input-hrm
                              v-model="Data.contractorDirectorPassportNumber"
                              :label="`${$t('passportNumber')}`"
                              :placeholder="`${$t('passportNumber')} `"
                           />
                        </b-col>
                        <b-col sm="12" md="3">
                           <form-picker
                              v-model="Data.fcontractorDirectorBirthDate"
                              :placeholder="$t('birthDate')"
                              :label="$t('birthDate')"
                              value-type="format"
                              format="DD.MM.YYYY"
                           ></form-picker>
                        </b-col>
                     </b-row>
                  </template>

                  <b-row>
                     <!-- <b-col sm="12" md="3">
                        <form-select
                           :options="bandlik"
                           :disabled="isdisabled"
                           v-model="Data.busyness"
                           :label="$t('employmentType')"
                        ></form-select>
                     </b-col> -->
                     <b-col sm="12" md="3">
                        <form-input-hrm
                           v-if="Data.countryId"
                           :disabled="isdisabled"
                           v-model="Data.phoneNumber"
                           v-mask="'+## ## ## ## ## ## ## ## '"
                           :label="$t('phone')"
                           :placeholder="$t('phone')"
                           rules="required"
                        />
                        <form-input-hrm
                           v-else
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
                           :disabled="true"
                           v-model="Data.docNumber"
                           :label="$t('docnumber')"
                           :placeholder="$t('docnumber')"
                        />
                     </b-col>
                     <b-col sm="12" md="3">
                        <form-picker v-model="Data.docOn" :label="$t('docdate')" :disabled="true" />
                     </b-col>
                     <!-- <b-col sm="12" md="3">
                        <form-select
                           :options="openAppeal"
                           :disabled="isdisabled"
                           v-model="Data.openAppeal"
                           :label="$t('openAppeal')"
                        ></form-select>
                     </b-col> -->
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
                           :options="AppealDescriptionSelectList"
                           v-model="Data.appealDescriptionId"
                           :label="$t('AppealDescription')"
                           required-star
                        ></form-select>
                     </b-col>
                  </b-row>
                  <b-row>
                     <b-col md="4" sm="4">
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
            <template #pdf>
               <WIframe :src="IframeSrc" style="height: 100vh" :show="iframeLoaded" />
            </template>
         </DocTabs>
      </b-card>
   </b-overlay>
</template>
<script>
import axios from 'axios';
// service
import ManualService from '@/services/others/manual.service';
// import AppealApplicationService from '@/services/appeal/AppealApplication.service';
import AppealDescriptionService from '@/services/appeal/AppealDescription.service';
import AppealTypeArriveService from '@/services/appeal/AppealTypeArrive.service';
import EdocInfo from '@/views/components/appeal/EdocInfo.vue';
import CallCenterAppealService from '@/services/document/callcenterappeal.service';
// components
import {
   BOverlay,
   BCard,
   BRow,
   BCol,
   BButton,
   BFormCheckbox,
   BFormFile,
   BSpinner,
   BFormTextarea,
   BIconTrash,
   BLink
} from 'bootstrap-vue';
import OkedService from '@/services/info/oked.service';
import FormInputTranslate from '@/components/translates/FormInputTranslate.vue';
import PrtnCertificateService from '@/services/document/prtncertificate.service';
import PersonService from '@/services/others/person.service';
import DepartmentService from '@/services/info/department.service';
import DistrictService from '@/services/info/district.service';
import RegionService from '@/services/info/region.service';
import CountryService from '@/services/info/country.service';
import DocTabs from '@/views/components/document/DocTabs.vue';
import WIframe from '@/components/WIframe.vue';

export default {
   components: {
      BOverlay,
      BFormCheckbox,
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
         ContractorCategoryList: [],
         Okedlist: [],
         chekCountry: false,
         CountryList: [],
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
            inn: ''
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
         Data: {
            businessStructure: null,
            personId: null,
            person: {},
            contractorInn: '',
            contractor: {},
            contractorId: null,
            contractorDirectorPinfl: null,
            contractorDirectorPassportSeria: null,
            contractorDirectorPassportNumber: null,
            fcontractorDirectorBirthDate: null,
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
            countryId: '',
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
      OkedService.GetSelectList('').then((res) => {
         this.Okedlist = res.data;
      });

      ManualService.ContractorCategorySelectList().then((res) => {
         this.ContractorCategoryList = res.data;
      });

      CountryService.GetAsSelectList()
         .then((res) => {
            this.CountryList = res.data;
         })
         .catch((error) => {
            this.showApiError(error);
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
      CallCenterAppealService.Get(this.$route.params.id)
         .then((res) => {
            this.iframeLoaded = true;
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
         return (id) => axios.defaults.baseURL + `CallCenterAppeal/DownloadFile/${id}`;
      },
      isView() {
         return this.$route.query.isView == 'true';
      },
      IframeSrc() {
         return axios.defaults.baseURL + `CallCenterAppeal/DownloadPdf?id2=${this.Data?.id}&lang=${this.getPdfLang()}`;
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
         CallCenterAppealService.UploadFile(formData).then((res) => {
            this.Data.files.push(...res.data);
            this.fileLoading = false;
         });
      },
      DeleteFile(id) {
         CallCenterAppealService.DeleteFile(id).then(() => {
            this.Data.files = this.Data.files.filter((item) => item.id != id);
         });
      },
      ChangeCountry(id) {
         if (id && id != 211) {
            this.chekCountry = true;
            this.Data.regionId = 0;
            this.Data.region = null;
            this.Data.districtId = null;
         } else {
            this.chekCountry = false;
         }
      },
      SaveData() {
         this.$refs.ValidationDTO.validate().then((success) => {
            if (success) {
               this.saveLoading = true;
               CallCenterAppealService.Update(this.Data)
                  .then(() => {
                     this.makeToast(this.$t('SaveSuccess'), 'success');
                     this.$router.push({ name: 'CallCenterAppeal' });
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
                  PrtnCertificateService.GetByInnFromSoliq(this.Data.contractorInn)
                     .then((res1) => {
                        this.Data.businessType = res1.data.businessType;
                        this.Data.okedId = res1.data.okedId;

                        this.Data.person = null;
                        this.Data.personId = null;
                        this.Data.director = res1.data.director;
                        this.Data.contractorFulName = res1.data.fullName;
                        this.Data.address = res1.data.address;
                        this.Data.regionId = res1.data.regionId;
                        this.Data.districtId = res1.data.districtId;
                        // this.$set(this.Data, 'districtId', res1.data.companyBillingAddress?.district?.districtId);
                        // this.$set(this.Data, 'regionId', res1.data.companyBillingAddress?.district?.regionId);

                        this.Data.contractorDirectorPinfl = res1.data.directorPinfl;
                        this.Data.contractorDirectorPassportSeria = res1.data.directorSeria;
                        this.Data.contractorDirectorPassportNumber = res1.data.directorNumber;
                        this.Data.fcontractorDirectorBirthDate = res1.data.directorBirthDate;
                        this.ChangeRegion();
                     })
                     .catch(this.showApiError)
                     .finally(() => {
                        this.soliqLoading = false;
                     });
               } else {
                  PersonService.GetByPassportData(
                     this.filter.passportSeria,
                     this.filter.passportNumber,
                     this.filter.birthDate
                  )
                     .then((res) => {
                        this.Data.person = res.data;
                        this.Data.personFullName = res.data.fullName;
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
         CallCenterAppealService.SendToEdoc(this.Data.id)
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
