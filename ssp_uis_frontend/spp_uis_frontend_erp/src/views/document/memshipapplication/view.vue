<template>
   <b-overlay :show="show">
      <b-container style="max-width: 1600px">
         <validation-observer v-if="ContractData && !Application.memshipContractId" ref="ValidationDTO">
            <b-card :title="$t('MemshipContract')">
               <b-row>
                  <b-col sm="12" md="3">
                     <form-input-hrm v-model="ContractData.docNumber" rules="required" :label="$t('docnumber')" />
                  </b-col>
                  <b-col sm="12" md="3">
                     <form-picker
                        v-model="ContractData.docOn"
                        :label="$t('startOn')"
                        @change="Refresh"
                        :placeholder="$t('docOn')"
                     />
                  </b-col>
                  <b-col sm="12" md="3">
                     <form-select
                        v-model="ContractData.memshipContractTypeId"
                        :options="MemshipContractTypeList"
                        required-star
                        :label="$t('memshipContractType')"
                     />
                  </b-col>

                  <template v-if="ContractData.memshipContractTypeId == 1">
                     <b-col sm="12" md="3">
                        <form-input-hrm
                           v-model="ContractData.baseFixedMinimumValue"
                           rules="required"
                           type="number"
                           :label="$t('baseFixedMinimumValue')"
                           debounce="500"
                        />
                     </b-col>
                  </template>
                  <b-col sm="12" md="3">
                     <form-select
                        :options="OrgSettlementAccountList"
                        v-model="ContractData.organizationSettlementAccountId"
                        @option:selected="(e) => (ContractData.organizationSettlementAccount = e ? e.text : '')"
                        requitred-star
                        label="orgSettlementAccount"
                     />
                  </b-col>
                  <b-col sm="12" md="3" v-if="ContractData.canSelectOrganization">
                     <form-select
                        :options="OrganizationGroupSelectList"
                        v-model="ContractData.regionalOrganizationId"
                        required-star
                        :label="$t('A\'zolikga jalb qilgan tashkilot')"
                     ></form-select>
                  </b-col>
                  <b-col sm="12" md="3">
                     <form-input-hrm v-model="ContractData.details" :label="$t('AdditionalInfo')" debounce="500" />
                  </b-col>
               </b-row>
            </b-card>
         </validation-observer>
         <b-row :class="{ 'justify-content-center': !ContractData }">
            <b-col md="6" cols="12">
               <b-card>
                  <DocTabs pdf-title="MemshipApplication" view-title="Info">
                     <template #pdf>
                        <WIframe
                           :src="IframeSrc"
                           style="height: 100vh"
                           :show="Application.application && Application.application.id2"
                        />
                        <div class="d-flex">
                           <!-- reject -->
                           <b-button
                              v-if="Application.canCancel"
                              class="mt-2 mr-2 w-100"
                              @click="Cancel"
                              size="xl"
                              variant="outline-danger"
                           >
                              <feather-icon icon="XCircleIcon"></feather-icon>
                              {{ $t('Cancel') }}
                           </b-button>
                           <b-button
                              v-if="Application.canPaidCancel"
                              class="mt-2 mr-2 w-100"
                              @click="Cancel"
                              size="xl"
                              variant="outline-danger"
                           >
                              <feather-icon icon="XCircleIcon"></feather-icon>
                              {{ $t('Cancel') }}
                           </b-button>
                           <!-- accept -->
                           <b-button
                              v-if="!Application.memshipContractId && Application.memshipContractTypeId != 2"
                              class="mt-2"
                              @click="GetContract"
                              style="width: 100%"
                              size="xl"
                              variant="outline-primary"
                           >
                              <feather-icon icon="CheckIcon"></feather-icon>
                              {{ $t('getAndCreateContract') }}
                           </b-button>
                        </div>
                     </template>
                     <template #view>
                        <MemshipApplicationFormView :application="Application" />
                     </template>
                  </DocTabs>
               </b-card>
            </b-col>
            <b-col md="6" cols="12" v-if="ContractData && ContractData.memshipContractTypeId">
               <b-card>
                  <DocTabs pdf-title="MemshipContract">
                     <template #pdf>
                        <WIframe
                           :src="IframeSrcContract"
                           style="height: 100vh"
                           :show="ContractData && ContractData.memshipContractTypeId"
                        />
                        <div class="d-flex">
                           <!-- back list -->
                           <b-button
                              v-if="!Application.memshipContractId"
                              class="mr-2 w-100 mt-2"
                              @click="$router.go(-1)"
                              size="xl"
                              variant="outline-danger"
                           >
                              <feather-icon icon="ArrowLeftIcon"></feather-icon>
                              {{ $t('back') }}
                           </b-button>
                           <b-button
                              v-if="!Application.memshipContractId"
                              @click="SaveData"
                              size="xl"
                              class="w-100 mt-2"
                              :disabled="contractSaveLoading"
                              variant="outline-success"
                           >
                              <feather-icon icon="CheckIcon"></feather-icon>
                              {{ $t('createMemshipContract') }}
                           </b-button>
                        </div>
                     </template>
                     <template #view>
                        <MemshipContractFormView :contract="ContractData" />
                     </template>
                  </DocTabs>
               </b-card>
            </b-col>

            <!-- reject message -->
            <b-col
               md="3"
               cols="12"
               order="1"
               v-if="
                  Application.application && Application.application.statusId == 24 && Application.application.message
               "
            >
               <b-alert class="mt-1" show variant="danger">
                  <p class="px-1">{{ Application.application.message }}</p>
               </b-alert>
            </b-col>
         </b-row>
      </b-container>

      <Chat v-if="Application && Application.id" :table-id="Application.tableId || 100" :document-id="Application.id" />
   </b-overlay>
</template>

<script>
import { BOverlay, BCard, BRow, BCol, BButton, BLink, BIcon, BBadge, BContainer, BAlert } from 'bootstrap-vue';
import axios from 'axios';
import MemshipApplicationService from '@/services/document/memshipapplication.service';
import MemshipContractService from '@/services/document/memshipcontract.service';
import ManualService from '@/services/others/manual.service';
import OrganizationService from '@/services/managment/organization.service';
import MemshipApplicationFormView from '@/views/components/memship/MemshipApplicationFormView.vue';
import MemshipContractFormView from '@/views/components/memship/MemshipContractFormView.vue';
import WIframe from '@/components/WIframe.vue';
import DocTabs from '@/views/components/document/DocTabs.vue';

const Chat = () => import('@/views/components/DocumentChat/Chat.vue');

export default {
   components: {
      BAlert,
      BOverlay,
      BCard,
      BRow,
      BCol,
      BButton,
      BLink,
      BIcon,
      BBadge,
      BContainer,
      MemshipApplicationFormView,
      MemshipContractFormView,
      WIframe,
      DocTabs,
      Chat
   },
   data() {
      return {
         OrganizationGroupSelectList: [],
         IframeSrcContract: '',
         show: false,
         contractSaveLoading: false,
         Application: {},
         ContractData: null,
         OrgSettlementAccountList: [],
         MemshipContractTypeList: [] // azolik shartnomasi turi,
      };
   },
   computed: {
      IframeSrc() {
         return (
            axios.defaults.baseURL +
            `Memship/MemshipApplication/DownloadPdf?id2=${this.Application?.application?.id2}&lang=${this.getPdfLang()}`
         );
      }
   },
   created() {
      this.GetApplication();
      ManualService.OrganizationAsSelectListByGroup([1, 3])
         .then((res) => {
            this.OrganizationGroupSelectList = res.data;
         })
         .catch(this.showApiError);

      ManualService.MemshipContractTypeSelectList().then((res) => {
         if (Array.isArray(res.data)) {
            this.MemshipContractTypeList = res.data;
         }
      });
      OrganizationService.GetAsSelectListOrgSettlementAccount()
         .then((res3) => {
            if (Array.isArray(res3.data)) {
               this.OrgSettlementAccountList = res3.data;
            }
         })
         .catch((error) => {
            this.showApiError(error);
         });
   },
   methods: {
      GetApplication() {
         this.show = true;
         MemshipApplicationService.Get(this.$route.params.id)
            .then((res) => {
               this.Application = res.data;

               if (this.Application.memshipContractId) {
                  MemshipContractService.Get(this.Application.memshipContractId)
                     .then((res2) => {
                        this.ContractData = res2.data;
                     })
                     .catch((error) => {
                        this.showApiError(error);
                     });
               }
            })
            .catch((error) => {
               this.showApiError(error);
            })
            .finally(() => {
               this.show = false;
            });
      },
      Cancel() {
         this.$swal.fire({
            icon: 'info',
            title: this.$t('WantCancel'),
            showLoaderOnConfirm: true,
            input: 'textarea',
            inputPlaceholder: this.$t('Cancel'),
            preConfirm: (msg) => {
               return MemshipApplicationService.Cancel({
                  id: this.Application.id,
                  message: msg
               })
                  .then(() => {
                     this.makeToast(this.$t('CancelApprovalSuccess'), 'success');
                     this.GetApplication();
                  })
                  .catch(this.SwalError);
            },
            allowOutsideClick: () => !this.$swal.isLoading()
         });
      },
      GetContract() {
         MemshipContractService.GetByApplication(this.Application.application.id)
            .then((res) => {
               this.ContractData = res.data;
            })
            .catch((error) => {
               this.showApiError(error);
            });
      },
      SaveData() {
         this.$refs.ValidationDTO.validate().then((success) => {
            if (success) {
               this.contractSaveLoading = true;
               MemshipContractService.Update(this.ContractData)
                  .then(() => {
                     this.makeToast(this.$t('SaveSuccess'), 'success');
                     this.$router.push({ name: 'MemshipContract' });
                  })
                  .catch((err) => {
                     this.showApiError(err);
                  })
                  .finally(() => {
                     this.contractSaveLoading = false;
                  });
            }
         });
      }
   },
   watch: {
      ContractData: {
         handler(newVal) {
            if (newVal && newVal.memshipContractTypeId) {
               if (newVal.id == 0) {
                  if (
                     (newVal.memshipContractTypeId == 1 && newVal.baseFixedMinimumValue) ||
                     newVal.memshipContractTypeId == 2
                  ) {
                     MemshipContractService.DownloadPdfCopy({ ...newVal, lang: this.getPdfLang() })
                        .then((res) => {
                           this.IframeSrcContract = URL.createObjectURL(res.data);
                        })
                        .catch((err) => {
                           this.showApiError(err);
                        });
                  }
               } else {
                  this.IframeSrcContract =
                     axios.defaults.baseURL +
                     `Memship/MemshipContract/DownloadPdf?id2=${newVal.id2}&lang=${this.getPdfLang()}`;
               }
            }
         },
         immediate: true,
         deep: true
      }
   }
};
</script>
