<template>
   <div>
      <b-tabs class="nav-tabs mb-0" nav-class="mb-0" v-model="tab">
         <b-tab title="Yangi"></b-tab>
         <b-tab title="Eski"> </b-tab>
         <b-tab :title="$t('AdditionalAgreement')" v-if="$can('AdditionalAgreementView', 'permissions')">
            <AdditionalAgreement />
         </b-tab>
      </b-tabs>
      <form-table-hrm
         v-if="tab != 2"
         :items="items"
         :actions="{}"
         :fields="fields"
         :filter.sync="filter"
         searchable
         :busy="isBusy"
         @row-dblclicked="DbClick"
         @request="Refresh"
      >
         <!-- filtes -->
         <template #filter>
            <b-row>
               <b-col cols="12" md="3">
                  <div>
                     <label for>{{ $t('innOrPinfl') }}</label>
                     <b-input-group>
                        <b-form-input
                           v-model="filter.contractorInn"
                           debounce="300"
                           v-mask="['##############']"
                           @keyup.enter="Refresh"
                           :placeholder="$t('innOrPinfl')"
                        />
                        <b-input-group-append>
                           <b-button @click="Refresh" size="sm" variant="primary">
                              <feather-icon icon="SearchIcon" />
                           </b-button>
                        </b-input-group-append>
                     </b-input-group>
                  </div>
               </b-col>
               <b-col cols="12" md="3">
                  <form-select
                     :options="RegionList"
                     v-model="filter.regionId"
                     :disabled="organizationId != 1"
                     @input="ChangeRegion"
                     label="region"
                  ></form-select>
               </b-col>
               <b-col cols="12" md="3">
                  <form-select
                     :options="DistrictList"
                     :reduce="(item) => item.value"
                     label="Region"
                     v-model="filter.districtId"
                     @input="ChangeDistrict"
                  />
               </b-col>
               <b-col cols="12" md="3">
                  <IsPinflSelect v-model="filter.isPinfl" @input="Refresh" />
               </b-col>
               <b-col sm="12" md="3">
                  <form-select
                     v-model="filter.contractorCategoryId"
                     :options="ContractorCategoryList"
                     label="contractorCategory"
                     @input="Refresh"
                  />
               </b-col>
               <b-col cols="12" md="3" v-if="!hideContractType">
                  <form-select
                     v-model="filter.memshipContractTypeId"
                     :options="MemshipContractTypeList"
                     label="memshipContractType"
                     @input="Refresh"
                  />
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

            <b-row>
               <b-col cols="12" md="3">
                  <CertificateExpireSelect
                     @date="
                        (e) => {
                           filter.fromExpireOn = e.fromExpireOn;
                           filter.toExpireOn = e.toExpireOn;
                           Refresh();
                        }
                     "
                  />
               </b-col>
               <b-col cols="12" md="3">
                  <form-select
                     :options="OpfSelectList"
                     v-model="filter.opfId"
                     @input="ChangeRegion"
                     label="opf"
                  ></form-select>
               </b-col>
               <b-col v-if="filter.statusId == 21" class="mt-2">
                  <div>
                     <b-button-group @click="Refresh" size="sm">
                        <b-button
                           @click="
                              () => {
                                 filter.hasCertificateCanceled = false;
                                 filter.hasCertificate = null;
                              }
                           "
                           :variant="
                              filter.hasCertificate == null && filter.hasCertificateCanceled == false
                                 ? 'primary'
                                 : 'outline-primary'
                           "
                           >{{ $t('all') }}</b-button
                        >
                        <b-button
                           @click="
                              () => {
                                 filter.hasCertificateCanceled = false;
                                 filter.hasCertificate = true;
                              }
                           "
                           :variant="filter.hasCertificate == true ? 'primary' : 'outline-primary'"
                           >{{ $t('hasMemshipCertificate') }}</b-button
                        >
                        <b-button
                           @click="
                              () => {
                                 filter.hasCertificateCanceled = false;
                                 filter.hasCertificate = false;
                              }
                           "
                           :variant="filter.hasCertificate == false ? 'primary' : 'outline-primary'"
                           >{{ $t('hasNotMemshipCertificate') }}</b-button
                        >
                        <b-button
                           @click="
                              () => {
                                 filter.hasCertificateCanceled = true;
                                 filter.hasCertificate = null;
                              }
                           "
                           :variant="
                              filter.hasCertificateCanceled && filter.hasCertificate == null
                                 ? 'primary'
                                 : 'outline-primary'
                           "
                           >{{ $t('hasNotMemshipCertificateCancel') }}</b-button
                        >
                     </b-button-group>
                  </div>
               </b-col>
            </b-row>

            <b-row align-v="center" align-h="between">
               <b-col cols="12" md="6" v-if="!hideStatus">
                  <StatusSelect v-model="filter.statusId" @input="Refresh" :filter="[1, 30, 24, 27, 21]" />
               </b-col>

               <b-col cols="12" md="4">
                  <b-input-group class="mb-1">
                     <b-form-input v-model="filter.search" @keyup.enter="Refresh" :placeholder="$t('search')" />
                     <b-input-group-append>
                        <b-button @click="Refresh" variant="primary">
                           <feather-icon icon="SearchIcon" />
                        </b-button>
                     </b-input-group-append>
                  </b-input-group>
               </b-col>
               <b-col cols="12" md="2">
                  <b-button @click="Print" :disabled="PrintLoading" variant="primary">
                     <feather-icon icon="PrinterIcon"></feather-icon>
                     {{ $t('Print') }}
                  </b-button>
               </b-col>
            </b-row>
         </template>
         <template #cell(contractor)="{ item }">
            <span style="color: blue; cursor: pointer" @dbclick="goToBussnes(item.contractorInn)">{{
               item.contractorInn
            }}</span>
            -
            {{ item.contractor }}
         </template>
         <template #cell(director)="{ item }">
            <span style="color: blue; cursor: pointer">{{ item.contractorPinfl }}</span>
            -
            {{ item.director }}
         </template>
         <template #cell(actions)="{ item }">
            <div class="text-center" style="text-wrap: nowrap">
               <!-- Edit -->
               <b-link
                  v-if="$can('ChangeContractorParametirs', 'permissions') && item.canChangeDocnumber"
                  class="mr-1"
                  :to="{
                     name: 'ViewMemshipContract',
                     params: { id: item.id, hasCertificate: item.hasCertificate, isChange: true }
                  }"
               >
                  <feather-icon icon="EditIcon"></feather-icon>
               </b-link>
               <b-link
                  :to="{ name: 'ViewMemshipContract', params: { id: item.id, hasCertificate: item.hasCertificate } }"
                  v-b-tooltip.hover.top="$t('View')"
                  class="mr-1"
               >
                  <feather-icon icon="EyeIcon"></feather-icon>
               </b-link>

               <!-- create certificate -->
               <b-link
                  v-if="item.canCreateCertificate && $can('MemshipCertificateCreate', 'permissions')"
                  v-b-tooltip.hover.top="$t('GetSertificate')"
                  @click="CertificateGet(item)"
                  class="mr-1"
               >
                  <feather-icon icon="FileIcon"></feather-icon>
               </b-link>
               <!-- delete -->
               <b-link
                  v-if="$can('MemshipContractDelete', 'permissions') && item.canDelete"
                  class="text-danger mr-1 cursor-pointer"
                  @click="Delete(item)"
                  v-b-tooltip.hover.top="$t('Delete')"
               >
                  <feather-icon icon="TrashIcon"></feather-icon>
               </b-link>

               <!-- reject -->
               <!-- <b-link
                  v-if="$can('MemshipContractReject', 'permissions') && item.canReject"
                  class="text-danger mr-1 cursor-pointer"
                  @click="Reject(item)"
                  v-b-tooltip.hover.top="$t('Reject')"
               >
                  <feather-icon icon="XSquareIcon"></feather-icon>
               </b-link> -->

               <!-- cancel -->
               <b-link
                  v-if="$can('MemshipContractCancel', 'permissions') && item.canCancel"
                  class="text-danger cursor-pointer mr-1"
                  v-b-tooltip.hover.top="$t('Cancel')"
                  @click="Cancel(item)"
               >
                  <feather-icon icon="XCircleIcon"></feather-icon>
               </b-link>

               <!-- document history -->
               <HistoryModalButton :id="item.id" :table-id="item.tableId || 99" class="mr-1" />

               <!-- create certificate -->
               <b-link
                  v-if="item.statusId == 21 && $can('AdditionalAgreementCreate', 'permissions')"
                  v-b-tooltip.hover.top="$t('AdditionalAgreement')"
                  @click="
                     $router.push({
                        name: 'EditAdditionalAgreement',
                        params: {
                           id: 0
                        },
                        query: {
                           memshipContractId: item.id,
                           contractorId: item.contractorId,
                           applicationTypeId: 3 // MEMSHIP
                        }
                     })
                  "
                  class="cursor-pointer"
               >
                  <feather-icon icon="FilePlusIcon"></feather-icon>
               </b-link>
            </div>
         </template>
         <template #cell(status)="{ item }">
            <b-badge :variant="getColor(item)">{{ item.status }}</b-badge>
         </template>
         <template #cell(hasCertificate)="{ item }">
            {{ item.hasCertificate ? $t('yes') : $t('no') }}
         </template>
      </form-table-hrm>
   </div>
</template>

<script>
import {
   BCard,
   BCardText,
   VBTooltip,
   BBadge,
   BButton,
   BLink,
   BModal,
   VBModal,
   BSpinner,
   BRow,
   BCol,
   BFormInput,
   BInputGroup,
   BButtonGroup,
   BInputGroupAppend,
   BTabs,
   BTab
} from 'bootstrap-vue';
import FormTableHrm from '@/components/forms/form-table-hrm.vue';
import MemshipContractService from '@/services/document/memshipcontract.service';
import HistoryModalButton from '@/views/components/document/HistoryModalButton.vue';
import StatusSelect from '@/views/components/document/StatusSelect.vue';
import ManualService from '@/services/others/manual.service';
import RegionService from '@/services/info/region.service';
import DistrictService from '@/services/info/district.service';
import AdditionalAgreement from '@/views/document/additionalagreement/index.vue';
import IsPinflSelect from '@/views/components/memship/IsPinflSelect.vue';
import CertificateExpireSelect from '@/views/components/memship/CertificateExpireSelect.vue';

export default {
   components: {
      BCard,
      FormTableHrm,
      BCardText,
      BButton,
      BBadge,
      BLink,
      BModal,
      BSpinner,
      BRow,
      BCol,
      BInputGroup,
      BButtonGroup,
      BFormInput,
      BInputGroupAppend,
      HistoryModalButton,
      StatusSelect,
      BTabs,
      BTab,
      AdditionalAgreement,
      IsPinflSelect,
      CertificateExpireSelect
   },
   props: {
      selectable: {
         type: Boolean,
         default: false
      },
      memshipContractTypeId: {
         type: Number,
         default: null
      },
      statusId: {
         type: Number,
         default: null
      },
      hideStatus: {
         type: Boolean,
         default: false
      },
      hideContractType: {
         type: Boolean,
         default: false
      }
   },
   directives: {
      'b-tooltip': VBTooltip,
      'b-modal': VBModal
   },
   data() {
      return {
         tab: this.$route.query.tab || 0,
         MemshipContractTypeList: [],
         RegionList: [],
         OpfSelectList: [],

         DistrictList: [],
         ContractorCategoryList: [],
         items: [],
         hasCertificateOptiosn: [
            {
               value: null,
               text: this.$t('all')
            },
            {
               value: true,
               text: this.$t('yes')
            },
            {
               value: false,
               text: this.$t('no')
            }
         ],
         filter: {
            statusId: this.statusId || null,
            organizationId: null,
            memshipContractTypeId: this.memshipContractTypeId,
            memshipApplicationId: null,
            contractorInn: null,
            contractorSettlementAccountId: null,
            organizationSettlementAccountId: null,
            search: '',
            opfId: null,
            sortBy: '',
            orderType: 'asc',
            page: 1,
            pageSize: 20,
            perPageOptions: [10, 20, 50, 100, 300, 500, 700, 1000],
            total: 0,
            isOld: false,
            regionId: null,
            districtId: null,
            isConfirmed: null,
            isPinfl: null,
            fromDocDate: '',
            toDocDate: '',
            fromExpireOn: '',
            toExpireOn: '',
            contractorCategoryId: null,
            hasCertificate: null
         },
         isBusy: false,
         PrintLoading: false,
         organizationId: JSON.parse(localStorage.getItem('user_info'))?.organizationId,
         organizationRegionId: JSON.parse(localStorage.getItem('user_info'))?.organizationRegionId
      };
   },
   computed: {
      fields() {
         return [
            {
               key: 'actions',
               label: this.$t('actions'),
               thClass: 'text-center',
               tdClass: 'text-center',
               stickyColumn: !this.isMobileDevice()
            },
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
               key: 'region',
               label: this.$t('region'),
               thClass: this.filter.isOld ? 'd-none' : '',
               tdClass: this.filter.isOld ? 'd-none' : ''
            },
            {
               key: 'district',
               label: this.$t('District'),
               thClass: this.filter.isOld ? 'd-none' : '',
               tdClass: this.filter.isOld ? 'd-none' : ''
            },
            {
               key: 'contractor',
               label: this.$t('contractor'),
               thStyle: {
                  minWidth: '300px'
               }
            },
            {
               key: 'contractorCategory',
               label: this.$t('contractorCategory')
            },
            {
               key: 'contractorOked',
               label: this.$t('oked')
            },
            {
               key: 'director',
               label: this.$t('director'),
               thStyle: {
                  minWidth: '250px'
               }
            },
            {
               key: 'memshipContractType',
               label: this.$t('memshipContractType')
            },
            {
               key: 'status',
               label: this.$t('status'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'hasCertificate',
               label: this.$t('memshipHasCertificate')
            }
         ];
      }
   },
   created() {
      if (this.$route.name == 'MemshipContract') {
         // set filter from query
         // eslint-disable-next-line no-restricted-syntax, guard-for-in
         for (const item in this.filter) {
            const newVal = this.$route.query[item] || this.filter[item];
            if (item == 'regionId' || item == 'districtId') {
               this.filter[item] = newVal ? Number(newVal) : null;
            } else if (item == 'perPageOptions') {
               // no need to
            } else {
               this.filter[item] = newVal;
            }
         }
      }

      ManualService.MemshipContractTypeSelectList().then((res) => {
         if (Array.isArray(res.data)) {
            this.MemshipContractTypeList = res.data;
         }
      });
      RegionService.GetAsSelectList(211).then((res) => {
         this.RegionList = res.data;
      });

      ManualService.ContractorCategorySelectList().then((res) => {
         this.ContractorCategoryList = res.data;
      });
      ManualService.OpfSelectList().then((res) => {
         this.OpfSelectList = res.data;
      });
   },
   watch: {
      tab: {
         handler(newVal) {
            if (newVal == 0) {
               this.filter.isOld = false;
            } else {
               this.filter.isOld = true;
            }
            this.Refresh();
         }
      }
   },
   methods: {
      async Reject(item) {
         try {
            this.$swal.fire({
               icon: 'question',
               title: this.$t('WantCancel'),
               showLoaderOnConfirm: true,
               html: `
            <div style=" display: flex; flex-direction: column">
            <input id="reject-message" style="margin:0"  class="swal2-input" placeholder="${this.$t('RejectMessage')}">
            
             <div style="margin-top: 10px; display: flex; align-items: center">
               <input
                  type="file"
                  id="file-upload"
                  style="width: 100%; padding: 5px"
                  class="swal2-file"
                  placeholder="${this.$t('Fayl tanlang')}"
                  accept=".pdf, .doc, .docx"
               />
               <button id="btnClear"   style="outline: none; border: none; background-color: red; color: white; font-weight: 800;padding:11px;margin-top:15px">
                  X
               </button>
          </div>
        `,
               preConfirm: async (msg) => {
                  const message = document.getElementById('reject-message').value;

                  const fileInput = document.getElementById('file-upload');

                  const file = fileInput.files[0];
                  let uploadFile = [];

                  if (file) {
                     const formData = new FormData();
                     formData.append('files', file);
                     await MemshipContractService.UploadFile(formData)
                        .then((res) => {
                           uploadFile = res.data;
                        })
                        .catch((err) => {
                           console.log(err);
                        });
                  }

                  const sendData = {
                     id: item.id,
                     rejectDate: new Date(),
                     rejectMessage: message,
                     files: uploadFile
                  };

                  return MemshipContractService.Reject(sendData)
                     .then(() => {
                        this.makeToast(this.$t('RejectSuccess'), 'success');
                        this.Refresh();
                     })
                     .catch(this.SwalError);
               },
               allowOutsideClick: () => !this.$swal.isLoading(),
               didOpen: () => {
                  // Button click event to clear file input
                  const clearButton = document.getElementById('btnClear');

                  const fileInput = document.getElementById('file-upload');
                  if (clearButton && fileInput) {
                     clearButton.addEventListener('click', () => {
                        document.getElementById('file-upload').value = '';
                     });
                  }
               }
            });
         } catch (error) {
            showApiError(error);
         }
      },

      createRouteQuery() {
         let { query } = this.$route;
         // eslint-disable-next-line no-restricted-syntax, guard-for-in
         for (const fil in this.filter) {
            query = {
               ...query,
               [fil]: this.filter[fil] || undefined
            };
         }
         delete query.perPageOptions;
         delete query.total;

         this.$router.replace({
            ...this.$route,
            query
         });
      },
      Print() {
         this.PrintLoading = true;
         MemshipContractService.SaveAsExcel(this.filter)
            .then((res) => {
               this.forceFileDownload(res, this.$t('MemshipApplication'));
            })
            .catch((e) => {
               this.showApiError(e);
            })
            .finally(() => {
               this.PrintLoading = false;
            });
      },
      goToBussnes(inn) {
         if (this.$can('BusinessmanCardView', 'permissions')) {
            this.$router.push({ name: 'BusinessmanCard', query: { inn: inn } });
         }
      },
      DbClick(item) {
         if (this.selectable) {
            this.$emit('row-selected', item);
         } else {
            this.$router.push({
               name: 'ViewMemshipContract',
               params: { id: item.id, hasCertificate: item.hasCertificate }
            });
         }
      },
      Delete(item) {
         this.$swal.fire({
            icon: 'info',
            title: this.$t('WantDelete'),
            showLoaderOnConfirm: true,
            preConfirm: () => {
               return MemshipContractService.Delete(item.id)
                  .then(() => {
                     this.makeToast(this.$t('DeleteSuccess'), 'success');
                     this.Refresh();
                  })
                  .catch(this.SwalError);
            },
            allowOutsideClick: () => !this.$swal.isLoading()
         });
      },
      Cancel(item) {
         this.$swal.fire({
            icon: 'question',
            title: this.$t('WantCancel'),
            showLoaderOnConfirm: true,
            input: 'text',
            inputPlaceholder: this.$t('RejectMessage'),
            preConfirm: (msg) => {
               return MemshipContractService.Cancel({
                  id: item.id,
                  message: msg
               })
                  .then(() => {
                     this.makeToast(this.$t('CancelMessage'), 'success');
                     this.Refresh();
                  })
                  .catch(this.SwalError);
            },
            allowOutsideClick: () => !this.$swal.isLoading()
         });
      },
      Refresh() {
         if (this.$route.name == 'MemshipContract') {
            this.createRouteQuery();
         }
         this.isBusy = true;
         if (this.organizationId != 1) {
            this.filter.regionId = this.organizationRegionId;
            this.GetDistrict(this.filter.regionId);
         }
         MemshipContractService.GetList({ ...this.filter })
            .then((res) => {
               this.items = res.data.rows;
               this.filter.total = res.data.total;
            })
            .finally(() => {
               this.isBusy = false;
            });
      },
      CertificateGet(item) {
         this.$router.push({
            name: 'EditMemshipCertificate',
            params: {
               id: 0
            },
            query: {
               contractId: item.id,
               isList: true
            }
         });
      },
      ChangeRegion() {
         if (this.filter.regionId) {
            this.filter.districtId = null;
            this.GetDistrict();
         }
         this.Refresh();
      },
      GetDistrict() {
         if (this.filter.regionId) {
            DistrictService.GetAsSelectList(this.filter.regionId).then((res) => {
               this.DistrictList = res.data;
            });
         } else {
            this.filter.districtId = null;
            this.DistrictList = [];
            this.Refresh();
         }
      },
      ChangeDistrict() {
         this.Refresh();
      }
   }
};
</script>
