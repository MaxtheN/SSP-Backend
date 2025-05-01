<template>
   <b-overlay :show="show">
      <b-row class="justify-content-end">
         <b-col sm="12" md="8" lg="8">
            <b-card>
               <b-tabs class="nav-tabs nav-justified">
                  <b-tab :title="$t('Info')">
                     <b-row>
                        <b-col sm="12" md="6" lg="6">
                           <form-input-hrm :label="$t('docnumber')" disabled v-model="Contract.docNumber" />
                        </b-col>
                        <b-col sm="12" md="6" lg="6">
                           <form-input-hrm :label="$t('docOn')" disabled v-model="Contract.docOn" />
                        </b-col>
                        <b-col sm="12" md="12" lg="12">
                           <form-input-hrm :label="$t('contractor')" disabled v-model="Contract.contractorFullName" />
                        </b-col>
                        <b-col sm="12" md="6" lg="6">
                           <form-input-hrm :label="$t('contractorInn')" disabled v-model="Contract.contractorInn" />
                        </b-col>
                        <b-col sm="12" md="6" lg="6">
                           <form-input-hrm :label="$t('director')" disabled v-model="Contract.contractorPositionName" />
                        </b-col>
                        <b-col sm="12" md="6" lg="6">
                           <form-input-hrm :label="$t('oblast')" disabled v-model="Contract.contractorRegion" />
                        </b-col>
                        <b-col sm="12" md="6" lg="6">
                           <form-input-hrm :label="$t('district')" disabled v-model="Contract.contractorDistrict" />
                        </b-col>
                     </b-row>
                     <template v-for="(priceTable, i) in Contract.groups">
                        <table v-if="priceTable.tables.length" class="priceTable mt-2 w-100" :key="i + 'group'">
                           <thead>
                              <tr>
                                 <th class="text-center" colspan="3">{{ priceTable.group }}</th>
                                 <th class="text-center" style="width: 16%">
                                    {{ $t('price') }} <span style="font-size: 11px">({{ $t('BXMD') }})</span>
                                 </th>
                                 <th style="width: 12%"></th>
                              </tr>
                           </thead>
                           <tbody>
                              <tr v-for="(tab, j) in priceTable.tables" :key="j + 'tab' + i">
                                 <td class="text-center w-70px">
                                    <feather-icon icon="CheckSquareIcon" size="16" />
                                 </td>
                                 <td>{{ tab.needChamberService }}</td>
                                 <td class="text-primary">
                                    {{ tab.offerServiceText }}
                                 </td>
                                 <td>
                                    <form-currency-input
                                       :placeholder="$t('0-1000')"
                                       v-model.number="tab.realCoef"
                                       :disabled="!!tab.concreteCoef"
                                       @input="formatRealCoef(j)"
                                    />
                                    <span v-if="tab.realCoef >= 1000" class="text-danger font-small-2">{{
                                       $t('1000 BXM dan oshmasligi kerak')
                                    }}</span>
                                 </td>
                                 <td>
                                    <p class="mb-0" v-if="tab.servicePriceType">{{ tab.servicePriceType }}</p>
                                    <p class="mb-0" v-if="tab.concreteCoef">{{ tab.concreteCoef }}</p>
                                    <p class="mb-0" v-if="tab.beginCoef && tab.endCoef">
                                       {{ tab.beginCoef }}-{{ tab.endCoef }}
                                    </p>
                                 </td>
                              </tr>
                           </tbody>
                        </table>
                     </template>
                  </b-tab>
                  <b-tab :title="$t('ServiceContract')" v-if="$route.params.id > 0">
                     <b-overlay
                        :show="!iframeLoaded"
                        v-if="Contract.id2"
                        spinner-variant="primary"
                        spinner-type="grow"
                        rounded="sm"
                     >
                        <iframe
                           v-if="Contract.id2"
                           style="height: 100vh"
                           :src="IframeSrcContract"
                           width="100%"
                           frameborder="0"
                           @load="iframeLoaded = true"
                        ></iframe>
                     </b-overlay>
                  </b-tab>
               </b-tabs>
            </b-card>
         </b-col>
         <b-col sm="6" md="3" lg="3">
            <div>
               <!-- save -->
               <b-button
                  @click="SaveData"
                  v-if="!Contract?.id"
                  size="xl"
                  class="mt-2 w-100"
                  :disabled="saveLoading"
                  variant="outline-success"
               >
                  <feather-icon icon="CheckIcon"></feather-icon>
                  {{ $t('Save') }}
               </b-button>

               <!-- print -->
               <a
                  v-if="Contract?.id2"
                  class="mt-2 btn btn-primary"
                  :href="IframeSrcContract"
                  target="_blank"
                  style="width: 100%"
               >
                  <feather-icon icon="PrinterIcon"></feather-icon>
                  {{ $t('Print') }}
               </a>

               <!-- reject -->
               <b-button
                  v-if="Contract.canReject"
                  class="mt-2 w-100"
                  @click="Reject"
                  size="xl"
                  variant="outline-warning"
               >
                  <feather-icon icon="XCircleIcon"></feather-icon>
                  {{ $t('Reject') }}
               </b-button>
               <!-- signed -->
               <b-button
                  class="mt-2 w-100"
                  v-if="Contract.canSign"
                  @click="OpenSign"
                  size="xl"
                  variant="outline-success"
               >
                  <feather-icon icon="CheckIcon"></feather-icon>
                  {{ $t('Sign') }}
               </b-button>

               <!-- signed -->
               <b-button
                  class="mt-2 w-100"
                  v-if="Contract.canCreateDeedDoc"
                  :to="{ name: 'EditServiceDeed', query: { srvContractId: Contract.id }, params: { id: 0 } }"
                  size="xl"
                  variant="outline-success"
               >
                  <feather-icon icon="CheckIcon"></feather-icon>
                  {{ $t('createServiceDeed') }}
               </b-button>
            </div>
            <b-alert v-if="Contract.message" variant="danger" class="mt-1" show>
               <p class="px-2">{{ Contract.message }}</p>
            </b-alert>
         </b-col>
      </b-row>

      <!-- sign dialog -->
      <b-modal v-model="EImzoModal" size="lg" :title="$t('enterEImzo')" hide-footer>
         <b-card-text>
            <just-sign :data-to-sign="Contract" v-if="!SignLoading" @sign="loginESP($event)" />
            <div style="height: 600px" v-if="SignLoading" class="d-flex justify-content-center align-items-center">
               <b-spinner label="Spinning"></b-spinner>
            </div>
         </b-card-text>
      </b-modal>

      <Chat v-if="Contract && Contract.id" :table-id="Contract.tableId || 119" :document-id="Contract.id" />
   </b-overlay>
</template>

<script>
import {
   BOverlay,
   BCard,
   BRow,
   BCol,
   BFormInput,
   BTabs,
   BTab,
   BButton,
   BTable,
   BLink,
   BFormGroup,
   VBTooltip,
   BModal,
   VBModal,
   BCardText,
   BInputGroup,
   BInputGroupAppend,
   BTr,
   BTd,
   BFormTextarea,
   BFormCheckbox,
   BIcon,
   BBadge,
   BSpinner,
   BFormFile,
   BAlert
} from 'bootstrap-vue';

import justSign from '@/components/justSign.vue';
import ServiceContractService from '@/services/srv/ServiceContract.service';
import axios from 'axios';
import eimzoMixin from '@/mixins/eimzo';
const Chat = () => import('@/views/components/DocumentChat/Chat.vue');

export default {
   components: {
      BOverlay,
      BCard,
      BRow,
      BCol,
      BFormInput,
      BTabs,
      BTab,
      BButton,
      BTable,
      BLink,
      BFormGroup,
      BModal,
      BCardText,
      BInputGroup,
      BInputGroupAppend,
      BTr,
      BTd,
      BFormTextarea,
      BFormCheckbox,
      BIcon,
      BBadge,
      justSign,
      BSpinner,
      BFormFile,
      Chat,
      BAlert
   },
   mixins: [eimzoMixin],
   directives: {
      'b-tooltip': VBTooltip,
      'b-modal': VBModal
   },
   data() {
      return {
         axios,
         DocumentHistoryList: {},
         EImzoModal: false,
         SignLoading: false,
         pdfShow: false,
         show: false,
         Contract: {},
         iframeLoaded: false,
         saveLoading: false
      };
   },
   computed: {
      IframeSrcContract() {
         return (
            axios.defaults.baseURL +
            `srv/ServiceContract/DownloadPdf?id2=${this.Contract?.id2}&lang=${this.getPdfLang()}`
         );
      }
   },
   created() {
      if (this.$route.query.appId && this.$route.params.id == 0) {
         this.GetContractByAppId();
      } else {
         this.GetContract();
      }
   },
   methods: {
      formatRealCoef(index) {
         let coef = this.Contract.groups.find((group) => group.tables.some((_, idx) => idx === index)).tables[index]
            .realCoef;

         if (parseFloat(coef) > 1000) {
            coef = 0;
         }
         this.Contract.groups.forEach((group) => {
            group.tables.forEach((table, idx) => {
               if (idx === index) {
                  table.realCoef = coef;
               }
            });
         });
      },

      GetContract() {
         this.show = true;
         ServiceContractService.Get(this.$route.params.id)
            .then((res) => {
               this.Contract = res.data;
            })
            .catch((error) => {
               this.showApiError(error);
            })
            .finally(() => {
               this.show = false;
            });
      },
      GetContractByAppId() {
         this.show = true;
         ServiceContractService.GetByApplicationId(this.$route.query.appId)
            .then((res) => {
               this.Contract = res.data;
               this.Contract.id = 0;
               this.Contract.groups = res.data.groups.map((g) => {
                  return {
                     ...g,
                     id: 0,
                     tables: g.tables.map((j) => ({ ...j, realCoef: j.isConcrete ? j.concreteCoef : j.realCoef }))
                  };
               });
            })
            .catch((error) => {
               this.showApiError(error);
            })
            .finally(() => {
               this.show = false;
            });
      },
      loginESP(item) {
         const isPinfl = this.isPinfl(item);
         this.Sign(item.key, isPinfl);
      },
      Reject() {
         this.$swal.fire({
            icon: 'info',
            title: this.$t('WantReject'),
            showLoaderOnConfirm: true,
            input: 'text',
            inputPlaceholder: this.$t('RejectMessage'),
            preConfirm: (msg) => {
               return ServiceContractService.Reject({
                  id: this.Contract.id,
                  message: msg
               })
                  .then(() => {
                     this.makeToast(this.$t('RejectSuccess'), 'success');
                     this.GetContract();
                  })
                  .catch(this.SwalError);
            },
            allowOutsideClick: () => !this.$swal.isLoading()
         });
      },
      OpenSign() {
         this.EImzoModal = true;
      },
      Sign(key, isPinfl) {
         this.SignLoading = true;
         ServiceContractService.Signing({
            isPinfl,
            signedData: key,
            id: this.Contract.id
         })
            .then(() => {
               this.makeToast(this.$t('SignMessage'), 'success');
               this.SignLoading = false;
               this.EImzoModal = false;
               this.GetContract();
            })
            .catch((error) => {
               this.showApiError(error);
            })
            .finally(() => {
               this.SignLoading = false;
            });
      },
      SaveData() {
         this.saveLoading = true;
         ServiceContractService.Update(this.Contract)
            .then((res) => {
               this.makeToast(this.$t('SaveSuccess'), 'success');
               if (this.Contract.id == 0) {
                  this.$router.push({ name: 'EditServiceContract', params: { id: res.data.id } });
               }
               this.GetContract();
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
<style lang="scss">
.priceTable {
   thead {
      tr {
         background-color: #f0f0f0;
      }
   }

   tr th,
   tr td {
      padding: 7px;
      border-collapse: collapse;
      border: 1px solid #f5f5f5;
   }

   tr:nth-child(even) {
      background-color: #f7f7f7;
   }

   .w-70px {
      width: 70px;
   }
}
</style>
