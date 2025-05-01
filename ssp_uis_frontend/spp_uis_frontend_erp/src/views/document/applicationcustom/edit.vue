<template>
   <b-overlay :show="show">
      <b-row>
         <b-col sm="12" md="12" lg="12" class="text-center">
            <b-row :class="!Application.prtnContractId && 'justify-content-center'">
               <b-card style="width: 50%">
                  <iframe
                     v-if="Application.id2"
                     style="width: 80vh; height: 100vh"
                     :src="axios.defaults.baseURL + `/Application/PrintApplicationPdf?Id=${Application.id2}`"
                     frameborder="0"
                  ></iframe>
                  <div class="d-flex">
                     <b-button
                        v-if="Application.canReject"
                        class="mt-2 mr-2"
                        @click="OpenReject"
                        style="width: 100%"
                        size="xl"
                        variant="outline-danger"
                     >
                        <feather-icon icon="XCircleIcon"></feather-icon>
                        {{ $t('Reject') }}
                     </b-button>
                     <b-button
                        v-if="Application.canAccept"
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
               </b-card>
               <b-card style="width: 49%; margin-left: 5px" v-if="Data.id2">
                  <div>
                     <iframe
                        style="width: 80vh; height: 100vh"
                        :src="axios.defaults.baseURL + `/PrtnContract/GetPdfTemplate?applicationId=${$route.params.id}`"
                        frameborder="0"
                     ></iframe>
                  </div>
                  <div class="d-flex">
                     <b-button
                        v-if="Application.canAccept"
                        class="mt-2 mr-2"
                        @click="$router.go(-1)"
                        style="width: 100%"
                        size="xl"
                        variant="outline-danger"
                     >
                        <feather-icon icon="ArrowLeftIcon"></feather-icon>
                        {{ $t('back') }}
                     </b-button>
                     <b-button
                        v-if="Application.canAccept"
                        class="mt-2"
                        @click="SaveData"
                        style="width: 100%"
                        size="xl"
                        variant="outline-success"
                     >
                        <feather-icon icon="CheckIcon"></feather-icon>
                        {{ $t('createContract') }}
                     </b-button>
                  </div>
               </b-card>
            </b-row>
            <b-modal v-model="RejectModal" :title="$t('Reject')" hide-footer>
               <b-card-text>
                  <h5 class="mb-2">{{ $t('WantReject') }}</h5>

                  <div class="form-group">
                     <form-input v-model="filter.message" :label="$t('message')" />
                  </div>
                  <div class="form-group">
                     <form-select
                        :options="PrtnRejectReasonList"
                        v-model="filter.prtnRejectReasonId"
                        :label="$t('prtnRejectReason')"
                     ></form-select>
                  </div>

                  <div class="d-flex justify-content-end">
                     <b-button
                        class="mt-2 mr-2"
                        @click="RejectModal = !RejectModal"
                        style="width: 100%"
                        size="xl"
                        variant="danger"
                        >{{ $t('no') }}</b-button
                     >
                     <b-button class="mt-2" @click="CancelApproval" style="width: 100%" size="xl" variant="success">{{
                        $t('yes')
                     }}</b-button>
                  </div>
               </b-card-text>
            </b-modal>
         </b-col>
      </b-row>
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
   BBadge
} from 'bootstrap-vue';
import Ripple from 'vue-ripple-directive';
import flatPickr from 'vue-flatpickr-component';
import Cleave from 'vue-cleave-component';
import PrtnRejectReasonService from '@/services/info/prtnrejectreason.service';
import PrtnContractService from '@/services/document/prtncontract.service';
import ApplicationService from '@/services/document/application.service';
import axios from 'axios';

import '@core/scss/vue/libs/vue-flatpicker.scss';

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
      flatPickr,
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
      Cleave
   },
   directives: {
      'b-tooltip': VBTooltip,
      'b-modal': VBModal
   },
   data() {
      return {
         axios,
         PrtnContractModal: false,
         ContractorList: [],
         PdfData: {},
         RejectModal: false,
         closeButton: false,
         ApplicationList: [],
         PDFDataUrl: '',
         PrtnRejectReasonList: [],
         PrtnContractTypeList: [],
         show: false,
         Data: {},
         filter: {
            message: '',
            id: 0,
            prtnRejectReasonId: 0
         },
         HtmlData: {},
         Application: {},
         HtmlDataContract: '',
         config: {
            dateFormat: 'd.m.Y'
         }
      };
   },
   created() {
      this.show = true;
      ApplicationService.Get(this.$route.params.id).then((res) => {
         this.Application = res.data;
         if (this.Application.prtnContractId) {
            this.GetContract();
         }
      });

      PrtnRejectReasonService.GetAsSelectList()
         .then((res) => {
            this.PrtnRejectReasonList = res.data;
         })
         .catch((error) => {
            this.showApiError(error);
         });

      ApplicationService.GetApplicationAsHtml(this.$route.params.id)
         .then((res) => {
            this.HtmlData = res.data;
            this.show = false;
         })
         .catch((error) => {
            this.showApiError(error);
         });
      this.show = false;
   },
   directives: {
      Ripple
   },
   methods: {
      OpenReject() {
         this.RejectModal = true;
         this.filter = {
            message: '',
            id: this.Application.id,
            prtnRejectReasonId: 0
         };
      },
      CancelApproval(item) {
         ApplicationService.Reject(this.filter)
            .then((res) => {
               this.makeToast(this.$t('RejectMessage'), 'success');
               this.Refresh();
            })
            .catch((error) => {
               this.showApiError(error);
            })
            .finally(() => {
               this.RejectModal = false;
            });
      },
      GetContract() {
         PrtnContractService.GetByApplicationId(this.$route.params.id)
            .then((res) => {
               this.show = false;
               this.Data = res.data;
               PrtnContractService.GetPdfTemplate(this.$route.params.id)
                  .then((res1) => {
                     this.HtmlDataContract = res1.data;
                     ApplicationService.Get(this.$route.params.id).then((res2) => {
                        this.Application = res2.data;
                     });
                  })
                  .catch((error) => {
                     this.showApiError(error);
                  });
            })
            .catch((error) => {
               this.showApiError(error);
            });
      },
      GetPDF() {
         PrtnContractService.GetPdfTemplate(this.Data)
            .then((res1) => {
               this.HtmlDataContract = res1.data;
            })
            .catch((error) => {
               this.showApiError(error);
            });
      },
      SaveData() {
         PrtnContractService.Update(this.Data)
            .then((res) => {
               this.makeToast(this.$t('SaveSuccess'), 'success');
               this.$router.push({ name: 'PrtnContract' });
            })
            .catch((err) => {
               this.showApiError(error);
            });
      }
   },
   watch: {},
   computed: {}
};
</script>
