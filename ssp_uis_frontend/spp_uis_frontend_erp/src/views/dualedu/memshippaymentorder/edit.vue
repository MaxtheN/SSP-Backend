<template>
   <b-overlay :show="show">
      <b-row>
         <b-col sm="12" md="12" lg="12">
            <b-card>
               <validation-observer ref="ValidationDTO">
                  <b-row>
                     <b-col sm="12" md="4" class="mb-1">
                        <form-input v-model.number="Data.docNumber" :label="$t('docnumber')" />
                     </b-col>
                     <b-col sm="12" md="4" class="mb-1">
                        <form-picker :label="$t('ondate')" v-model="Data.docOn" />
                     </b-col>
                     <b-col sm="12" md="4">
                        <form-select
                           :options="ApplicationTypeIdConstComp"
                           v-model="Data.applicationTypeId"
                           required-star
                           :label="$t('applicationType')"
                        ></form-select>
                     </b-col>
                     <!-- memship -->
                     <template v-if="Data.applicationTypeId == 3">
                        <b-col sm="12" md="4" class="mb-1">
                           <form-input :value="Data.memshipContractNumber" disabled :label="$t('MemshipContract')">
                              <b-input-group-append>
                                 <b-button variant="primary" @click="dialog = true">
                                    <feather-icon icon="PlusIcon"></feather-icon>
                                 </b-button>
                              </b-input-group-append>
                           </form-input>
                        </b-col>

                        <b-modal size="xl" :title="$t('MemshipContract')" v-model="dialog" hide-footer>
                           <MemshipContractList
                              :selectable="true"
                              @row-selected="memshipSelect"
                              hide-status
                              hide-contract-type
                              :memship-contract-type-id="1"
                              :status-id="21"
                           />
                        </b-modal>
                     </template>
                     <!-- memship end -->

                     <!-- service  -->
                     <template v-if="Data.applicationTypeId == 7">
                        <b-col sm="12" md="4" class="mb-1">
                           <form-input :value="Data.serviceContractNumber" disabled :label="$t('ServiceContract')">
                              <b-input-group-append>
                                 <b-button variant="primary" @click="dialog = true">
                                    <feather-icon icon="PlusIcon"></feather-icon>
                                 </b-button>
                              </b-input-group-append>
                           </form-input>
                        </b-col>

                        <b-modal size="xl" :title="$t('ServiceContract')" v-model="dialog" hide-footer>
                           <ServiceContractList
                              :selectable="true"
                              @row-selected="serviceSelect"
                              hide-status
                              :status-id="21"
                           />
                        </b-modal>
                     </template>
                     <!-- service end -->

                     <b-col sm="12" md="4" class="mb-1">
                        <form-select :options="BankList" v-model="Data.bankId" label="Bank" required-star />
                     </b-col>
                     <b-col sm="12" md="4" class="mb-1">
                        <form-input v-model="Data.contractor" disabled :label="$t('contractor')" />
                     </b-col>

                     <b-col sm="12" md="4" class="mb-1">
                        <form-currency-input
                           v-if="Data.applicationTypeId != 7"
                           v-model="Data.amount"
                           :label="$t('amount')"
                        />
                     </b-col>

                     <b-col sm="12" md="8" class="mb-1">
                        <form-input-hrm
                           v-if="Data.applicationTypeId != 7"
                           v-model="Data.details"
                           :label="$t('details')"
                        />
                     </b-col>
                  </b-row>
               </validation-observer>
               <template v-if="Data.applicationTypeId == 7">
                  <validation-observer ref="ValidationDTO2">
                     <b-row>
                        <b-col sm="12" md="4" class="mb-1">
                           <form-select
                              required-star
                              :options="needChamberServicList"
                              v-model="tabrow.needChamberServiceId"
                              @change="handle"
                              :label="$t('Needchamberserviceid')"
                           ></form-select>
                        </b-col>

                        <b-col sm="12" md="4" class="">
                           <form-currency-input v-model="tabrow.amount" required :label="$t('amount')" />
                        </b-col>
                        <b-col cols="12" md="2"
                           ><b-button block size="md" @click="addTabrow" class="mt-2" variant="primary">
                              <feather-icon icon="PlusIcon"></feather-icon>
                              {{ $t('Add') }}
                           </b-button></b-col
                        >
                        <b-col cols="12" md="12">
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
                              <template #cell(id)="{ item, index }"> {{ index + 1 }}</template>
                              <template #cell(amount)="{ item }"> {{ currency(item.amount) }} </template>
                              <template #cell(actions)="{ item, index }">
                                 <!-- edit -->
                                 <b-link @click="EditTabRow(item, index)" class="mr-1 cursor-pointer">
                                    <feather-icon icon="EditIcon"></feather-icon>
                                 </b-link>
                                 <!-- delete -->
                                 <b-link class="text-danger mr-1 cursor-pointer" @click="DeleteTabRow(index)">
                                    <feather-icon icon="TrashIcon"></feather-icon>
                                 </b-link>
                              </template>
                           </b-table>
                        </b-col>
                     </b-row>
                  </validation-observer>
               </template>

               <b-row>
                  <b-col md="4" sm="4">
                     <h6 class="inputTitle">{{ $t('fileupload') }}</h6>
                     <b-form-file type="file" :placeholder="$t('Faylni tanlang')" @change="UploadFile"> </b-form-file>
                     <div class="mt-1" v-for="item in Data.files" :key="item.id">
                        <b-link variant="primary" target="_blank" :href="FileSrc(item.id)">{{
                           item.fileName || item.id
                        }}</b-link>
                        <b-button variant="danger" size="sm" class="ml-1" @click="DeleteFile(item.id)">
                           <b-icon-trash scale="0.7" />
                        </b-button>
                     </div>
                  </b-col>
               </b-row>
               <b-row>
                  <b-col sm="12" md="6" lg="6" class="text-left"></b-col>
                  <b-col sm="12" md="6" lg="6" class="text-right">
                     <b-button
                        v-if="Data.id != 0 && Data.serviceContractId && Data.canCancel && $route.query.view"
                        :disabled="saveLoading"
                        @click="Cancel(Data)"
                        size="sm"
                        class="mx-2"
                        variant="outline-danger"
                     >
                        <feather-icon icon="CheckIcon"></feather-icon>
                        {{ $t('Cancel') }}
                     </b-button>

                     <b-button
                        v-if="Data.id != 0 && Data.serviceContractId && Data.canAccept && $route.query.view"
                        :disabled="saveLoading"
                        @click="Accept(Data)"
                        size="sm"
                        class="mx-2"
                        variant="outline-success"
                     >
                        <feather-icon icon="CheckIcon"></feather-icon>
                        {{ $t('Accept') }}
                     </b-button>
                     <b-button
                        :disabled="saveLoading"
                        @click="SaveData"
                        v-if="!$route.query.view"
                        size="sm"
                        variant="outline-success"
                     >
                        <feather-icon icon="CheckIcon"></feather-icon>
                        {{ $t('Save') }}
                     </b-button>
                  </b-col>
               </b-row>
            </b-card>
         </b-col>
      </b-row>
   </b-overlay>
</template>
<script>
import axios from 'axios';
// service
import MemshipPaymentOrderService from '@/services/dualedu/memshippaymentorder.service';
import NeedChamberServiceService from '@/services/hrm/needchamberservice.service';
// components
import {
   BOverlay,
   BCard,
   BRow,
   BCol,
   BFormInput,
   BTable,
   BButton,
   BButtonGroup,
   BLink,
   BFormGroup,
   BModal,
   BInputGroup,
   BInputGroupAppend,
   BFormCheckbox,
   BFormTextarea,
   BFormFile,
   BIconTrash
} from 'bootstrap-vue';
import BankService from '@/services/info/bank.service';
import FormCurrencyInput from '@/components/forms/form-currency-input.vue';
import ApplicationMixin from '@/mixins/application';

const MemshipContractList = () => import('@/views/document/memshipcontract/index.vue');
const ServiceContractList = () => import('@/views/srv/ServiceContract/index.vue');

export default {
   components: {
      BOverlay,
      BCard,
      BRow,
      BCol,
      BFormInput,
      BButtonGroup,
      BButton,
      BTable,
      BLink,
      BFormGroup,
      BModal,
      BInputGroup,
      BInputGroupAppend,
      BFormCheckbox,
      BFormTextarea,
      BFormFile,
      FormCurrencyInput,
      BIconTrash,
      MemshipContractList,
      ServiceContractList
   },
   name: 'Edit',
   mixins: [ApplicationMixin],

   data() {
      return {
         tabIndex: null,
         show: false,
         tabrow: {
            needChamberServiceId: null,
            amount: null,
            needChamberService: ''
         },
         needChamberServicList: [],
         StateList: [],
         ContractorList: [],
         BankList: [],
         dialog: false,
         saveLoading: false,
         fileLoading: false,

         TablesField: [
            {
               key: 'id',
               thStyle: 'width:50px',
               label: this.$t('№'),
               tdClass: 'text-center',
               thClass: 'text-center'
            },

            {
               key: 'needChamberService',
               label: this.$t('needChamberService'),
               tdClass: 'text-left',
               thClass: 'text-center',
               thStyle: 'width:900px'
            },
            {
               key: 'amount',
               label: this.$t('amount'),
               tdClass: 'text-center',
               thClass: 'text-center'
            },
            {
               key: 'actions',
               label: this.$t('actions'),
               tdClass: 'text-center',
               thClass: 'text-center'
            }
         ],
         Data: {
            docOn: '',
            docNumber: '',
            details: '',
            contractorId: null,
            contractor: null,
            memshipContractId: null,
            bankId: null,
            amount: 0,
            applicationTypeId: 3,
            serviceContractId: null,
            serviceContractTableId: null,
            files: [],
            tables: []
         }
      };
   },
   computed: {
      ApplicationTypeIdConstComp() {
         return this.ApplicationTypeIdConst.filter((e) => [3, 7].includes(e.value));
      },
      FileSrc() {
         return (id) => axios.defaults.baseURL + `dualedu/MemshipPaymentOrder/DownloadFile/${id}`;
      }
   },
   created() {
      this.show = true;
      MemshipPaymentOrderService.Get(this.$route.params.id)
         .then((res) => {
            this.Data = res.data;
            if (this.$route.params.id == 0) {
               this.Data.applicationTypeId = +this.$route.query.applicationTypeId || 3;
            }
            if (this.$route.query['0']) {
               this.Data.applicationTypeId = 7;
            }
         })
         .catch((error) => {
            this.showApiError(error);
         })
         .finally(() => {
            this.show = false;
         });

      NeedChamberServiceService.GetAsSelectList()
         .then((res) => {
            this.needChamberServicList = res.data;
         })
         .catch((error) => {
            this.showApiError(error);
         })
         .finally(() => {
            this.show = false;
         });

      BankService.GetAsSelectList().then((res) => {
         this.BankList = res.data;
      });
   },
   methods: {
      UploadFile(event) {
         const formData = new FormData();
         formData.append('files', event.target.files[0]);
         this.fileLoading = true;
         MemshipPaymentOrderService.UploadFile(formData).then((res) => {
            this.Data.files.push(...res.data);
            this.fileLoading = false;
         });
      },
      DeleteFile(id) {
         MemshipPaymentOrderService.DeleteFile(id).then(() => {
            this.Data.files = this.Data.files.filter((item) => item.id != id);
         });
      },
      memshipSelect(item) {
         this.dialog = false;
         this.Data.memshipContractId = item.id;
         this.Data.memshipContractNumber = item.docNumber;
         this.Data.memshipContractDocOn = item.docOn;
         this.Data.contractorId = item.contractorId;
         this.Data.contractor = item.contractor;
      },
      serviceSelect(item) {
         this.dialog = false;
         this.Data.serviceContractId = item.id;
         this.Data.serviceContractNumber = item.docNumber;
         this.Data.serviceContractDocOn = item.docOn;
         this.Data.contractorId = item.contractorId;
         this.Data.contractor = item.contractor;
      },
      SaveData() {
         this.$refs.ValidationDTO.validate().then((success) => {
            if (success) {
               this.saveLoading = true;
               MemshipPaymentOrderService.Update(this.Data)
                  .then((res) => {
                     this.makeToast(this.$t('SaveSuccess'), 'success');
                     this.$router.push({
                        name: 'MemshipPaymentOrder',
                        query: { applicationTypeId: this.Data.applicationTypeId == 3 ? 3 : 7 }
                     });
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
      addTabrow() {
         this.$refs.ValidationDTO2.validate().then((success) => {
            if (success) {
               if (this.tabIndex == null) {
                  this.Data.tables.push({ ...this.tabrow, applicationTypeId: this.Data.applicationTypeId });
                  this.tabrow = {};
                  this.$refs.ValidationDTO2.reset();
               } else {
                  // this.Data.tables[this.tabIndex] = this.tabrow;
                  Object.assign(this.Data.tables[this.tabIndex], this.tabrow);
                  this.tabrow = {};
                  this.$refs.ValidationDTO2.reset();
               }
            }
         });
      },

      DeleteTabRow(index) {
         this.Data.tables = this.Data.tables.filter((a, b) => index != b);
      },
      EditTabRow(item, index) {
         this.tabrow = { ...item };
         this.tabIndex = index;
      },
      handle(id) {
         this.tabrow.needChamberService = this.needChamberServicList.filter((item) => item.value == id)[0].text;
      },
      Accept(item) {
         this.$swal.fire({
            icon: 'info',
            title: this.$t('WantAccept'),
            // showLoaderOnConfirm: true,
            // input: 'text',
            inputPlaceholder: this.$t('RejectMessage'),
            preConfirm: () => {
               return MemshipPaymentOrderService.Accept({
                  id: item.id,
                  message: ''
               })
                  .then(() => {
                     this.makeToast(this.$t('AcceptSuccess'), 'success');
                     this.$router.push({
                        name: 'MemshipPaymentOrder',
                        query: { applicationTypeId: this.Data.applicationTypeId == 3 ? 3 : 7 }
                     });
                  })
                  .catch(this.SwalError);
            },
            allowOutsideClick: () => !this.$swal.isLoading()
         });
      },
      Cancel(item) {
         this.$swal.fire({
            icon: 'info',
            title: this.$t('WantReject'),
            showLoaderOnConfirm: true,
            // input: 'text',
            // inputPlaceholder: this.$t('RejectMessage'),
            preConfirm: () => {
               return MemshipPaymentOrderService.Cancel({
                  id: item.id,
                  message: ''
               })
                  .then(() => {
                     this.makeToast(this.$t('CancelMessage'), 'success');
                     this.$router.push({
                        name: 'MemshipPaymentOrder',
                        query: { applicationTypeId: this.Data.applicationTypeId == 3 ? 3 : 7 }
                     });
                  })
                  .catch(this.SwalError);
            },
            allowOutsideClick: () => !this.$swal.isLoading()
         });
      }
   }
};
</script>
