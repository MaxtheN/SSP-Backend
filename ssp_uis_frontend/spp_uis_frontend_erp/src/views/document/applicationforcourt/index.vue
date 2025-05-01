<template>
   <div>
      <form-table-hrm
         :items="items"
         :actions="{}"
         :fields="fields"
         :filter.sync="filter"
         searchable
         :busy="isBusy"
         :delete-loading="deleteLoading"
         @row-dblclicked="DbClick"
         @request="Refresh"
         @row-delete="Delete"
      >
         <!-- filtes -->
         <template #filter>
            <b-row>
               <b-col cols="12" md="3">
                  <form-select
                     :options="ClaimOrganizationList"
                     v-model="filter.claimOrganizationId"
                     @input="Refresh"
                     label="ClaimOrganization"
                  ></form-select>
               </b-col>
               <b-col cols="12" md="3">
                  <label for>{{ $t('contractorInn') }}</label>
                  <b-input-group>
                     <b-form-input
                        v-model="filter.contractorInn"
                        debounce="300"
                        v-mask="'#########'"
                        @keyup.enter="Refresh"
                        :placeholder="$t('contractorInn')"
                     />
                     <b-input-group-append>
                        <b-button @click="Refresh" size="sm" variant="primary">
                           <feather-icon icon="SearchIcon" />
                        </b-button>
                     </b-input-group-append>
                  </b-input-group>
               </b-col>
               <b-col cols="12" md="3">
                  <div>
                     <label for>{{ $t('startdate') }}</label>
                     <form-picker v-model="filter.fromDocDate" :placeholder="$t('startdate')" @input="Refresh" />
                  </div>
               </b-col>
               <b-col cols="12" md="3">
                  <div>
                     <label for>{{ $t('enddate') }}</label>
                     <form-picker v-model="filter.toDocDate" :placeholder="$t('enddate')" @input="Refresh" />
                  </div>
               </b-col>
            </b-row>
            <b-row align-v="center">
               <b-col cols="12" md="6">
                  <StatusSelect v-model="filter.statusId" @input="Refresh" :filter="[1, 8, 2, 23, 25]" />
               </b-col>
               <b-col cols="12" md="3">
                  <v-select
                     :reduce="(item) => item.value"
                     :options="BankList"
                     placeholder="Bank"
                     v-model="filter.contractorId"
                     @input="Refresh"
                     label="text"
                  />
               </b-col>
               <b-col cols="12" md="3">
                  <b-input-group>
                     <b-form-input v-model="filter.search" @keyup.enter="Refresh" :placeholder="$t('search')" />
                     <b-input-group-append>
                        <b-button @click="Refresh" variant="primary">
                           <feather-icon icon="SearchIcon" />
                        </b-button>
                     </b-input-group-append>
                  </b-input-group>
               </b-col>
            </b-row>
         </template>
         <template #cell(actions)="{ item }">
            <div class="text-center" style="text-wrap: nowrap">
               <!-- View -->
               <b-link
                  v-if="$can('ApplicationForCourtView', 'permissions')"
                  :to="{ name: 'ViewApplicationForCourt', params: { id: item.id } }"
                  v-b-tooltip.hover.top="$t('View')"
                  class="mr-1 cursor-pointer"
               >
                  <feather-icon icon="EyeIcon"></feather-icon>
               </b-link>
               <!-- edit -->
               <b-link
                  v-if="$can('ApplicationForCourtEdit', 'permissions') && item.canEdit"
                  :to="{ name: 'EditApplicationForCourt', params: { id: item.id } }"
                  v-b-tooltip.hover.top="$t('Edit')"
                  class="mr-1 cursor-pointer"
               >
                  <feather-icon icon="EditIcon"></feather-icon>
               </b-link>
               <!-- delete -->
               <b-link
                  v-if="$can('ApplicationForCourtDelete', 'permissions')"
                  class="text-danger mr-1 cursor-pointer"
                  @click="Delete(item)"
                  v-b-tooltip.hover.top="$t('Delete')"
               >
                  <feather-icon icon="TrashIcon"></feather-icon>
               </b-link>
               <!-- accept -->

               <b-link
                  v-if="$can('ApplicationForCourtAccept', 'permissions') && item.canAccept"
                  @click="OpenSign(item)"
                  class="mr-1 text-success cursor-pointer"
                  v-b-tooltip.hover.top="$t('Approve')"
               >
                  <feather-icon icon="CheckCircleIcon"></feather-icon>
               </b-link>

               <!-- Send -->
               <b-link
                  v-if="$can('ApplicationForCourtSend', 'permissions') && item.canSend"
                  @click="Send(item)"
                  class="mr-1 text-success cursor-pointer"
                  v-b-tooltip.hover.top="$t('Send')"
               >
                  <feather-icon icon="CheckCircleIcon"></feather-icon>
               </b-link>
               <!-- NotAccept -->
               <template v-if="$can('ApplicationForCourtCancel', 'permissions') && item.canCancel">
                  <b-link
                     class="text-danger cursor-pointer mr-1"
                     v-b-tooltip.hover.top="$t('NotAccept')"
                     @click="NotAccept(item)"
                  >
                     <feather-icon icon="XCircleIcon"></feather-icon>
                  </b-link>
               </template>

               <!-- send court -->
               <b-link v-b-tooltip.hover.top="$t('send')" style="margin-left: 5px" @click="openModal(item)">
                  <feather-icon icon="SendIcon"></feather-icon>
               </b-link>
            </div>
         </template>
         <template #cell(step)="{ item }">
            <b-badge :variant="getColor({ statusId: item.stepId, status: item.step })">
               <!-- xodim -->
               <template v-if="filter.isEmployee">
                  {{
                     item.stepId == 7 ? $t('EXECUTING2') : item.stepId == 10 ? $t('MEDIATION_PLAN_CREATE2') : item.step
                  }}
               </template>
               <!-- rahbar  -->
               <template v-else> {{ item.step }} </template>
            </b-badge>
         </template>
         <template #cell(status)="{ item }">
            <b-badge :variant="getColor(item)">{{ item.status }}</b-badge>
         </template>
      </form-table-hrm>

      <!-- sign dialog -->
      <b-modal v-model="EImzoModal" size="lg" :title="$t('enterEImzo')" hide-footer>
         <b-card-text v-if="selectedItem">
            <just-sign :data-to-sign="selectedItem" v-if="!SignLoading" @sign="loginESP($event)" />
            <div style="height: 600px" v-if="SignLoading" class="d-flex justify-content-center align-items-center">
               <b-spinner label="Spinning"></b-spinner>
            </div>
         </b-card-text>
      </b-modal>
      <b-modal size="xl" v-model="courtModal" hide-footer>
         <validation-observer ref="ValidationDTO">
            <b-row>
               <b-col cols="12" md="4">
                  <SelectForCort
                     required-star
                     :label="$t('Oblast')"
                     v-model="sentCurt.regionId"
                     :options="intRegionList"
                     :lang-id="langId"
                     @change="ChangeRegion"
                  ></SelectForCort>
               </b-col>
               <b-col cols="12" md="4">
                  <SelectForCort
                     required-star
                     :label="$t('Region')"
                     v-model="sentCurt.districtId"
                     :options="intDistrcitList"
                     :lang-id="langId"
                  ></SelectForCort>
               </b-col>
               <b-col cols="12" md="4">
                  <SelectForCort
                     required-star
                     :label="$t('courtId')"
                     v-model="sentCurt.courtId"
                     :options="countryIdList"
                     :lang-id="langId"
                  ></SelectForCort>
               </b-col>
               <b-col>
                  <label for>
                     {{ $t(`participantType`) }}
                     <span style="color: red">*</span>
                  </label>
                  <v-select
                     :options="participantTypeList"
                     v-model="sentCurt.participantType"
                     :label="$t('participantType')"
                  ></v-select>
               </b-col>
               <b-col cols="12" md="4">
                  <SelectForCort
                     required-star
                     :label="$t('categoryId')"
                     v-model="sentCurt.categoryId"
                     :options="courtCategoryList"
                     :lang-id="langId"
                  ></SelectForCort>
               </b-col>
               <b-col cols="12" md="4">
                  <label for>
                     {{ $t(`currencyId`) }}
                     <span style="color: red">*</span>
                  </label>
                  <v-select
                     required-star
                     :options="currencyList"
                     :placeholder="$t('ChooseBelow')"
                     :reduce="(item) => item.id"
                     v-model="sentCurt.currencyId"
                     label="id"
                  >
                  </v-select>
               </b-col>
               <b-col cols="12" md="4">
                  <label for>
                     {{ $t(`entityType`) }}
                     <span style="color: red">*</span>
                  </label>
                  <v-select
                     :label="$t('entityType')"
                     v-model="sentCurt.entityType"
                     :options="entityTypeList"
                  ></v-select>
               </b-col>
               <b-col cols="12" md="4">
                  <label for>
                     {{ $t(`claimKind`) }}
                     <span style="color: red">*</span>
                  </label>
                  <v-select
                     required
                     :label="$t('claimKind')"
                     v-model="sentCurt.claimKind"
                     :options="claimKindList"
                  ></v-select>
               </b-col>
               <b-col cols="12" md="4">
                  <SelectForCort
                     required-star
                     :label="$t('typeId')"
                     v-model="sentCurt.typeId"
                     :options="intTypeList"
                     :lang-id="langId"
                  ></SelectForCort>
               </b-col>
               <b-col cols="12" md="4">
                  <SelectForCort
                     required-star
                     :label="$t('amountCategoryId')"
                     v-model="sentCurt.amountCategoryId"
                     :options="amountCategoryList"
                     :lang-id="langId"
                  ></SelectForCort>
               </b-col>
               <b-col cols="12" md="4">
                  <form-input
                     required
                     v-model="sentCurt.paymentAccount"
                     :label="$t('paymentAccount')"
                     :placeholder="$t('paymentAccount')"
                  />
               </b-col>
               <b-col md="12" sm="4" class="mt-5">
                  <h6 class="inputTitle">{{ $t('fileupload') }}</h6>
                  <b-form-file type="file" :placeholder="$t('Faylni tanlang')" @change="UploadFile"> </b-form-file>
               </b-col>
            </b-row>
         </validation-observer>
         <b-row class="mt-2">
            <b-col sm="12" md="6" lg="6" class="text-left"></b-col>
            <b-col sm="12" md="6" lg="6" class="text-right">
               <b-button
                  v-if="!sentCurt.signedData"
                  :disabled="saveLoading"
                  @click="openCourtImzo"
                  size="sm"
                  variant="outline-success"
                  class="mr-2"
               >
                  <feather-icon icon="CheckIcon"></feather-icon>
                  {{ $t('Agree') }}
               </b-button>
               <b-button
                  v-if="sentCurt.signedData"
                  :disabled="saveLoading"
                  @click="SaveData"
                  size="sm"
                  variant="outline-success"
               >
                  <feather-icon icon="CheckIcon"></feather-icon>
                  {{ $t('CountBulkMessage') }}
               </b-button>
               <b-button variant="danger" @click="courtModal = !courtModal" size="sm" class="ml-2">
                  {{ $t('back') }}
               </b-button>
            </b-col>
         </b-row>
         <b-modal v-model="courtImzo" size="lg" :title="$t('enterEImzo')" hide-footer>
            <b-card-text v-if="selectItem">
               <just-sign :data-to-sign="selectItem" v-if="!SignLoading" @sign="loginESPSud($event)" />
               <div style="height: 600px" v-if="SignLoading" class="d-flex justify-content-center align-items-center">
                  <b-spinner label="Spinning"></b-spinner>
               </div>
            </b-card-text>
         </b-modal>
      </b-modal>
   </div>
</template>

<script>
import { ValidationProvider } from 'vee-validate';
import SelectForCort from './components/SelectForCort.vue';
import {
   BCard,
   BCardText,
   VBTooltip,
   BBadge,
   BButton,
   BLink,
   BModal,
   VBModal,
   BFormInput,
   BInputGroup,
   BInputGroupAppend,
   BRow,
   BCol,
   BButtonGroup,
   BSpinner,
   BFormFile
} from 'bootstrap-vue';
import FormTableHrm from '@/components/forms/form-table-hrm.vue';
import StatusSelect from '@/views/components/document/StatusSelect.vue';
import ApplicationForCourtService from '@/services/document/applicationforcourt.service';
import ClaimOrganizationService from '@/services/info/claimorganization.service';
import justSign from '@/components/justSign.vue';
import eimzoMixin from '@/mixins/eimzo';
import ManualService from '@/services/others/manual.service';
import vSelect from 'vue-select';

export default {
   components: {
      BFormFile,
      SelectForCort,
      BCard,
      FormTableHrm,
      BCardText,
      BButton,
      BBadge,
      BLink,
      BModal,
      BInputGroup,
      BInputGroupAppend,
      BRow,
      BCol,
      BButtonGroup,
      BFormInput,
      StatusSelect,
      justSign,
      BSpinner,
      vSelect,
      ValidationProvider
   },
   directives: {
      'b-tooltip': VBTooltip,
      'b-modal': VBModal
   },
   mixins: [eimzoMixin],
   data() {
      return {
         selectItem: {},
         courtImzo: false,
         items: [],
         saveLoading: false,
         BankList: [],
         ClaimOrganizationList: [],
         fields: [
            {
               key: 'id',
               label: this.$t('id'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'docNumber',
               label: this.$t('docnumber'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'docOn',
               label: this.$t('docOn'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'contractor',
               label: this.$t('contractor')
            },
            {
               key: 'step',
               label: this.$t('step'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'status',
               label: this.$t('status'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'actions',
               label: this.$t('actions'),
               thClass: 'text-center',
               tdClass: 'text-center'
            }
         ],
         filter: {
            claimOrganizationId: null,
            contractorInn: null,
            statusId: null,
            statusIds: [],
            fromDocDate: '',
            toDocDate: '',
            search: '',
            sortBy: '',
            orderType: 'asc',
            page: 1,
            pageSize: 20,
            perPageOptions: [10, 20, 50, 100, 300],
            total: 0
         },
         sentCurt: {
            applicationForCourtId: 0,
            courtId: '',
            participantType: '',
            docNumber: '',
            categoryId: '',
            currencyId: '',
            entityType: '',
            claimKind: '',
            fileId: '',
            typeId: '',
            amountCategoryId: '',
            regionId: '',
            districtId: '',
            countryId: '',
            paymentAccount: 0,
            signedData: ''
         },
         isBusy: false,
         deleteLoading: false,
         acceptLoading: false,
         cancelLoading: false,
         EImzoModal: false,
         selectedItem: null,
         SignLoading: false,
         courtModal: false,
         intRegionList: [],
         intDistrcitList: [],
         participantTypeList: [],
         courtCategoryList: [],
         currencyList: [],
         entityTypeList: [],
         claimKindList: [],
         intTypeList: [],
         amountCategoryList: [],
         countryIdList: []
      };
   },
   created() {
      ApplicationForCourtService.GetSudRegionList().then((res) => {
         this.intRegionList = res.data;
      });
      ApplicationForCourtService.GetSudCourtList().then((res) => {
         this.countryIdList = res.data;
      });
      ApplicationForCourtService.GetSudCurrencyList().then((res) => {
         this.currencyList = res.data;
      });
      ApplicationForCourtService.GetSudParticipantTypeList().then((res) => {
         this.participantTypeList = res.data;
      });
      ApplicationForCourtService.GetSudCategoryList().then((res) => {
         this.courtCategoryList = res.data;
      });
      ApplicationForCourtService.GetSudEntityTypeList().then((res) => {
         this.entityTypeList = res.data;
      });
      ApplicationForCourtService.GetSudClaimKindList().then((res) => {
         this.claimKindList = res.data;
      });
      ApplicationForCourtService.GetSudDocumentTypesList().then((res) => {
         this.intTypeList = res.data;
      });
      ApplicationForCourtService.GetSudAmountCategoryList().then((res) => {
         this.amountCategoryList = res.data;
      });

      let localStorageData = localStorage.getItem('user_info');
      localStorageData = JSON.parse(localStorageData);
      this.localStorageData = localStorageData;
      if (this.localStorageData.positionCategoryId == 1 || this.localStorageData.id == 1) {
         this.filter.isEmployee = false;
      } else {
         this.filter.isEmployee = true;
      }
      ClaimOrganizationService.GetAsSelectList().then((res) => {
         this.ClaimOrganizationList = res.data;
      });
      ManualService.ContractorSelectList().then((res) => {
         this.BankList = res.data;
      });
   },
   computed: {
      langId() {
         // eslint-disable-next-line no-nested-ternary
         return localStorage.getItem('locale') == 'uz_latn'
            ? 'uz'
            : // eslint-disable-next-line no-nested-ternary
            localStorage.getItem('locale') == 'uz_cyrl'
            ? 'uz_cyr'
            : localStorage.getItem('locale') == 'ru'
            ? 'ru'
            : 'uz';
      }
   },
   methods: {
      SaveData() {
         this.$refs.ValidationDTO.validate().then(async (success) => {
            if (success) {
               this.saveLoading = true;
               ApplicationForCourtService.SendSudIntegration({
                  ...this.sentCurt,
                  applicationForCourtId: this.selectItem.id,
                  docNumber: this.selectItem.docNumber
               })
                  .then(() => {
                     this.makeToast(this.$t('SaveSuccess'), 'success');
                  })
                  .catch((err) => {
                     this.showApiError(err);
                  })
                  .finally(() => {
                     this.saveLoading = false;
                     this.courtModal = false;
                  });
            }
         });
      },
      ChangeRegion(id) {
         if (id) {
            this.getDistrictList(id);
         }
      },
      UploadFile(event) {
         const formData = new FormData();
         formData.append('file', event.target.files[0]);
         ApplicationForCourtService.SudUploadFileIntegration(formData).then((res) => {
            this.sentCurt.fileId = res.data.id;
         });
      },
      getDistrictList(id) {
         ApplicationForCourtService.GetSudDistrictList(id).then((res) => {
            this.intDistrcitList = res.data;
         });
      },
      openModal(item) {
         this.sentCurt = {};
         this.selectItem = item;
         this.courtModal = true;
      },

      loginESP(item) {
         const isPinfl = this.isPinfl(item);
         this.Accept(item.key, isPinfl);
      },
      loginESPSud(item) {
         this.sentCurt.signedData = item.key;
         this.courtImzo = false;
      },
      OpenSign(item) {
         this.EImzoModal = true;
         this.selectedItem = item;
      },
      openCourtImzo() {
         this.courtImzo = true;
      },
      Accept(key, isPinfl) {
         this.SignLoading = true;
         if (this.selectedItem) {
            const body = {
               id: this.selectedItem.id,
               signedData: key,
               isPinfl: isPinfl
            };

            if (false) {
               ApplicationForCourtService.AcceptForEmployee(body)
                  .then(() => {
                     this.makeToast(this.$t('ApproveMessage'), 'success');
                     this.EImzoModal = false;
                     this.Refresh();
                  })
                  .catch((error) => {
                     this.showApiError(error);
                  })
                  .finally(() => {
                     this.SignLoading = false;
                  });
            } else {
               ApplicationForCourtService.Accept(body)
                  .then(() => {
                     this.makeToast(this.$t('ApproveMessage'), 'success');
                     this.EImzoModal = false;
                     this.Refresh();
                  })
                  .catch((error) => {
                     this.showApiError(error);
                  })
                  .finally(() => {
                     this.SignLoading = false;
                  });
            }
         }
      },
      DbClick(item) {
         this.$router.push({
            name: 'EditApplicationForCourt',
            params: { id: item.id }
         });
      },
      Delete(item) {
         this.$swal.fire({
            icon: 'info',
            title: this.$t('WantDelete'),
            showLoaderOnConfirm: true,
            preConfirm: () => {
               return ApplicationForCourtService.Delete(item.id)
                  .then(() => {
                     this.makeToast(this.$t('DeleteSuccess'), 'success');
                     this.GetApplication();
                  })
                  .catch(this.SwalError);
            },
            allowOutsideClick: () => !this.$swal.isLoading()
         });
      },
      Send(item) {
         this.$swal.fire({
            icon: 'info',
            title: this.$t('WantSend'),
            showLoaderOnConfirm: true,
            preConfirm: () => {
               return ApplicationForCourtService.Send(item.id)
                  .then(() => {
                     this.makeToast(this.$t('SendSuccess'), 'success');
                     this.GetApplication();
                  })
                  .catch(this.SwalError);
            },
            allowOutsideClick: () => !this.$swal.isLoading()
         });
      },

      NotAccept(item) {
         this.$swal.fire({
            icon: 'info',
            title: this.$t('WantNotAccept'),
            showLoaderOnConfirm: true,
            preConfirm: () => {
               return ApplicationForCourtService.NotAccept(item.id)
                  .then(() => {
                     this.makeToast(this.$t('NotAcceptSuccess'), 'success');
                     this.GetApplication();
                  })
                  .catch(this.SwalError);
            },
            allowOutsideClick: () => !this.$swal.isLoading()
         });
      },
      Refresh() {
         this.isBusy = true;
         ApplicationForCourtService.GetList(this.filter)
            .then((res) => {
               this.items = res.data.rows;
               this.filter.total = res.data.total;
            })
            .finally(() => {
               this.isBusy = false;
            });
      },
      showModal() {}
   }
};
</script>
