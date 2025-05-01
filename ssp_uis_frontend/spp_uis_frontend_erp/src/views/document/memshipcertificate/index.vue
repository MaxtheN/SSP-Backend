<template>
   <div>
      <b-tabs class="nav-tabs mb-0" nav-class="mb-0" v-model="tab">
         <b-tab title="Yangi"></b-tab>
         <b-tab title="Eski"> </b-tab>
         <b-tab title="Muddati o'tgan"> </b-tab>
         <b-tab title="Joriy yilda yirik toifaga o'tkanlar"> </b-tab>
      </b-tabs>
      <form-table-hrm
         :items="items"
         :actions="{}"
         :fields="tab != 3 ? fields : fields1"
         :filter.sync="filter"
         searchable
         :isPagination="tab != 3 ? true : false"
         :busy="isBusy"
         :delete-loading="deleteLoading"
         @row-dblclicked="DbClick"
         @request="Refresh"
         @row-delete="Delete"
      >
         <!-- filtes -->
         <template #filter v-if="tab != 3">
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
                     :disabled="localStorageData.organizationId != 1"
                     v-model="filter.regionId"
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
               <b-col cols="12" md="3">
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
                     @input="Refresh"
                     label="opf"
                  ></form-select>
               </b-col>
               <b-col cols="12" md="3">
                  <form-select
                     :options="Okdelist"
                     v-model="filter.contractorOkedId"
                     @input="Refresh"
                     label="oked"
                  ></form-select>
               </b-col>
               <b-col cols="12" md="3">
                  <label>{{ $t('search') }}</label>
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
            <b-row align-v="start">
               <b-col sm="12" md="6">
                  <StatusSelect v-model="filter.statusId" @input="Refresh" :filter="[26, 24]" />
               </b-col>
               <b-col class="text-right">
                  <b-button @click="Print" :disabled="PrintLoading" variant="primary">
                     <feather-icon icon="PrinterIcon"></feather-icon>
                     {{ $t('Print') }}
                  </b-button>
               </b-col>
            </b-row>
         </template>
         <template #cell(actions)="{ item }">
            <div class="text-center" style="text-wrap: nowrap">
               <!-- edit -->
               <b-link
                  :to="{ name: 'EditMemshipCertificate', params: { id: item.id } }"
                  v-b-tooltip.hover.top="$t('View')"
                  class="mr-1 cursor-pointer"
               >
                  <feather-icon icon="EyeIcon"></feather-icon>
               </b-link>
               <!-- document history -->
               <HistoryModalButton :id="item.id" :table-id="item.tableId || 98" class="mr-1" />
               <!-- edit -->
               <b-link
                  v-if="$can('MemshipCertificateEdit', 'permissions') && item.canEdit"
                  :to="{ name: 'EditMemshipCertificate', params: { id: item.id } }"
                  v-b-tooltip.hover.top="$t('Edit')"
                  class="mr-1 cursor-pointer"
               >
                  <feather-icon icon="EditIcon"></feather-icon>
               </b-link>
               <!-- delete -->
               <b-link
                  v-if="$can('MemshipCertificateDelete', 'permissions') && item.canDelete"
                  class="text-danger mr-1 cursor-pointer"
                  @click="$refs['DeleteModal' + item.id].show()"
                  v-b-tooltip.hover.top="$t('Delete')"
               >
                  <feather-icon icon="TrashIcon"></feather-icon>
               </b-link>

               <b-modal
                  :ref="'DeleteModal' + item.id"
                  :cancel-title="$t('Cancel')"
                  :ok-title="$t('Accept')"
                  cancel-variant="danger"
                  ok-variant="success"
                  @ok="Delete(item)"
               >
                  <template #modal-title>
                     {{ $t('Accept') }}
                     <b-spinner v-if="deleteLoading" small></b-spinner>
                  </template>
                  <b-card-text>
                     <h5>ID : {{ item.id }}</h5>
                     <h5>{{ $t('WantDelete') }}</h5>
                  </b-card-text>
               </b-modal>
               <!-- accept -->
               <b-link
                  v-if="$can('MemshipCertificateAccept', 'permissions') && item.canAccept"
                  @click="$refs['ApproveModal' + item.id].show()"
                  class="mr-1 text-success cursor-pointer"
                  v-b-tooltip.hover.top="$t('Approve')"
               >
                  <feather-icon icon="CheckCircleIcon"></feather-icon>
               </b-link>
               <b-modal
                  :ref="'ApproveModal' + item.id"
                  :cancel-title="$t('Cancel')"
                  :ok-title="$t('Approve')"
                  cancel-variant="danger"
                  ok-variant="success"
                  @ok="Accept(item)"
               >
                  <template #modal-title>{{ $t('Approve') }}</template>
                  <b-card-text>
                     <h5>ID : {{ item.id }}</h5>
                     <h5>{{ $t('WantApprove') }}</h5>
                  </b-card-text>
               </b-modal>
               <!-- cancel -->
               <template>
                  <b-link
                     v-if="$can('MemshipCertificateCancel', 'permissions') && item.canCancel"
                     class="text-danger cursor-pointer mr-1"
                     v-b-tooltip.hover.top="$t('Cancel')"
                     @click="Cancel(item)"
                  >
                     <feather-icon icon="XCircleIcon"></feather-icon>
                  </b-link>
               </template>
            </div>
         </template>
         <template #cell(contractor)="{ item }">
            <span style="color: blue; cursor: pointer" @click="goToBussnes(item.contractorInn)">
               {{ item.contractorInn }}
            </span>
            -
            {{ item.contractor }}
         </template>
         <template #cell(director)="{ item }">
            <span style="color: blue; cursor: pointer">
               {{ item.contractorPinfl }}
            </span>
            -
            {{ item.director }}
         </template>
         <template #cell(status)="{ item }">
            <b-badge :variant="getColor(item)">{{ item.status }}</b-badge>
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
import MemshipCertificateService from '@/services/document/memshipcertificate.service';
import StatusSelect from '@/views/components/document/StatusSelect.vue';
import IsPinflSelect from '@/views/components/memship/IsPinflSelect.vue';
import CertificateExpireSelect from '@/views/components/memship/CertificateExpireSelect.vue';
import RegionService from '@/services/info/region.service';
import DistrictService from '@/services/info/district.service';
import ManualService from '@/services/others/manual.service';
import HistoryModalButton from '@/views/components/document/HistoryModalButton.vue';
import OkedService from '@/services/info/oked.service';
export default {
   components: {
      HistoryModalButton,
      BCard,
      FormTableHrm,
      BCardText,
      BButton,
      BBadge,
      BLink,
      BModal,
      BRow,
      BCol,
      BFormInput,
      BInputGroup,
      BButtonGroup,
      BInputGroupAppend,
      StatusSelect,
      BTabs,
      BTab,
      IsPinflSelect,
      CertificateExpireSelect
   },
   directives: {
      'b-tooltip': VBTooltip,
      'b-modal': VBModal
   },
   data() {
      return {
         localStorageData: {},
         tab: Number(this.$route.query.tab) || 0,
         OpfSelectList: [],
         items: [],
         RegionList: [],
         DistrictList: [],
         ContractorCategoryList: [],
         MemshipContractTypeList: [],
         okedText: '',
         fields: [
            {
               key: 'actions',
               label: this.$t('actions'),
               thClass: 'text-center',
               tdClass: 'text-center'
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
               key: 'expireOn',
               label: this.$t('expireOn'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'region',
               label: this.$t('region'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'district',
               label: this.$t('district'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'contractor',
               label: this.$t('contractor')
            },
            {
               key: 'contractorOked',
               label: this.$t('oked')
            },
            {
               key: 'phoneNumber',
               label: this.$t('phone'),
               thStyle: {
                  minWidth: '200px'
               }
            },
            {
               key: 'director',
               label: this.$t('director')
            },
            {
               key: 'status',
               label: this.$t('status'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            }
         ],
         fields1: [
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
               key: 'contractorInn',
               label: this.$t('companyInn'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            }
         ],
         Okdelist: [],
         filter: {
            contractorOkedId: null,
            search: '',
            sortBy: '',
            orderType: 'asc',
            page: 1,
            pageSize: 20,
            perPageOptions: [10, 20, 50, 100, 300],
            total: 0,
            statusId: null,
            contractorInn: null,
            isOld: false,
            byExpireOn: false,
            isPinfl: null,
            fromExpireOn: null,
            toExpireOn: null,
            regionId: null,
            districtId: null,
            contractorCategoryId: Number(this.$route.query.contractorCategoryId) || null,
            memshipContractTypeId: Number(this.$route.query.memshipContractTypeId) || null,
            Less1MonthLeft: this.$route.query?.Less1MonthLeft == 'true' ? this.formatDate(new Date()) : null
         },
         isBusy: false,
         deleteLoading: false,
         acceptLoading: false,
         cancelLoading: false,
         PrintLoading: false
      };
   },
   watch: {
      tab: {
         handler(newVal) {
            if (newVal == 0) {
               this.filter.isOld = false;
               this.filter.byExpireOn = false;
            } else if (newVal == 1) {
               this.filter.byExpireOn = false;
               this.filter.isOld = true;
            } else {
               this.filter.byExpireOn = true;
               this.filter.isOld = null;
            }
            this.Refresh();
         },
         immediate: true
      }
   },
   created() {
      OkedService.GetSelectList(this.okedText).then((res) => {
         this.Okdelist = res.data;
         // console.log(this.Okdelist.rows);
      });
      this.GetlocalStorageData();
      RegionService.GetAsSelectList(211).then((res) => {
         this.RegionList = res.data;
      });
      ManualService.OpfSelectList().then((res) => {
         this.OpfSelectList = res.data;
      });
      ManualService.ContractorCategorySelectList().then((res) => {
         this.ContractorCategoryList = res.data;
      });
      ManualService.MemshipContractTypeSelectList().then((res) => {
         if (Array.isArray(res.data)) {
            this.MemshipContractTypeList = res.data;
         }
      });
   },
   methods: {
      formatDate(d, plusMonth = 0) {
         const date = new Date(d.setMonth(d.getMonth() + plusMonth));
         const day = String(date.getDate()).padStart(2, '0');
         const month = String(date.getMonth() + 1).padStart(2, '0');
         const year = date.getFullYear();

         return day + '.' + month + '.' + year;
      },
      Print() {
         this.PrintLoading = true;
         MemshipCertificateService.SaveAsExcel(this.filter)
            .then((res) => {
               this.forceFileDownload(res, this.$t('MemshipCertificate'));
            })
            .catch((e) => {
               this.showApiError(e);
            })
            .finally(() => {
               this.PrintLoading = false;
            });
      },
      goToBussnes(inn) {
         this.$router.push({ name: 'BusinessmanCard', query: { inn: inn } });
      },
      DbClick(item) {
         this.$router.push({
            name: 'EditMemshipCertificate',
            params: { id: item.id }
         });
      },
      Delete(item) {
         this.deleteLoading = true;
         MemshipCertificateService.Delete(item.id)
            .then(() => {
               this.makeToast(this.$t('DeleteSuccess'), 'success');
               this.Refresh();
            })
            .catch((err) => {
               this.showApiError(err);
            })
            .finally(() => {
               this.deleteLoading = false;
            });
      },
      Accept(item) {
         this.acceptLoading = true;
         MemshipCertificateService.Accept({
            statusId: item.statusId,
            id: item.id
         })
            .then(() => {
               this.makeToast(this.$t('AcceptSuccess'), 'success');
               this.Refresh();
            })
            .catch((err) => {
               this.showApiError(err);
            })
            .finally(() => {
               this.acceptLoading = false;
            });
      },

      async Cancel(item) {
         try {
            this.$swal.fire({
               icon: 'question',
               title: this.$t('WantCancel'),
               showLoaderOnConfirm: true,
               html: `
            <div style=" display: flex; flex-direction: column">
            <input id="reject-message" style="margin:0"  class="swal2-input" placeholder="${this.$t('RejectMessage')}">
            <div style="text-align: left; margin-top: 10px; display:flex;align-items:center">
             <input type="checkbox" style="width:18px;height:18px" id="confirm-checkbox">
             <label style="margin-top:8px;margin-left:10px" for="confirm-checkbox">${this.$t(
                'Ariza va shartnomani bekor qilish'
             )}</label>

             </div>
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
                  const isChecked = document.getElementById('confirm-checkbox').checked;
                  const fileInput = document.getElementById('file-upload');

                  const file = fileInput.files[0];
                  let uploadFile = [];

                  if (file) {
                     const formData = new FormData();
                     formData.append('files', file);
                     await MemshipCertificateService.UploadFile(formData)
                        .then((res) => {
                           uploadFile = res.data;
                        })
                        .catch((err) => {
                           console.log(err);
                        });
                  }

                  const sendData = {
                     id: item.id,
                     cancelDay: new Date(),
                     isRejectContractAndApplication: isChecked,
                     files: uploadFile
                  };

                  if (isChecked) {
                     sendData.cancelReasen = message;
                  } else {
                     sendData.message = message;
                  }
                  return MemshipCertificateService.Cancel(sendData)
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
            sh;
         }
      },

      Refresh() {
         this.isBusy = true;
         MemshipCertificateService.GetList(this.filter)
            .then((res) => {
               this.items = res.data.rows;
               this.filter.total = res.data.total;
            })
            .catch((err) => {
               this.showApiError(err);
            })
            .finally(() => {
               this.isBusy = false;
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
      },
      GetlocalStorageData() {
         this.localStorageData = JSON.parse(localStorage.getItem('user_info'));
         if (this.localStorageData.organizationId != 1) {
            this.filter.regionId = this.localStorageData.organizationRegionId;
            this.GetDistrict(this.localStorageData.organizationRegionId);
         }
      },
      UploadFile(event) {
         const formData = new FormData();
         formData.append('files', event.target.files[0]);
         this.fileLoading = true;
         MemshipCertificateService.UploadFile(formData).then((res) => {
            this.Data.files.push(...res.data);
            this.fileLoading = false;
         });
      }
   }
};
</script>
<style scoped></style>
