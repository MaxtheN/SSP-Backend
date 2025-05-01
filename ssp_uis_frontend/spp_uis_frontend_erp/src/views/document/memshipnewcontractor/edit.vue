<template>
   <b-overlay :show="show">
      <b-row>
         <b-col sm="12" md="12" lg="12">
            <b-card>
               <b-row>
                  <b-col sm="12" md="4">
                     <form-input v-model.number="Data.docNumber" :label="$t('docnumber')" />
                  </b-col>
                  <b-col sm="12" md="2">
                     <form-picker :label="$t('docdate')" v-model="Data.docOn" />
                  </b-col>
                  <b-col sm="12" md="3">
                     <form-picker :label="$t('startDate1')" v-model="Data.fromDate" />
                  </b-col>
                  <b-col sm="12" md="3">
                     <form-picker :label="$t('endDate1')" v-model="Data.toDate" />
                  </b-col>
                  <b-col cols="12" md="4">
                     <form-select :options="RegionList" required v-model="Data.regionId" label="Oblast"></form-select>
                  </b-col>
                  <b-col sm="12" md="4">
                     <form-input disabled v-model.number="computedTotalLegalCount" :label="$t('totalLegalCount')" />
                  </b-col>
                  <b-col sm="12" md="4">
                     <form-input
                        disabled
                        v-model.number="computedTotalPhysicalCount"
                        :label="$t('totalPhysicalCount')"
                     />
                  </b-col>
               </b-row>
               <b-row>
                  <b-col sm="12" md="4">
                     <form-input v-model.number="Data.details" :label="$t('details')" />
                  </b-col>

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
                  <b-col class="text-right mt-2">
                     <b-button @click="Fill" variant="primary">
                        <b-spinner v-if="isBusy" small></b-spinner>
                        <feather-icon v-else icon="AlignLeftIcon"></feather-icon>
                        {{ $t('Fill') }}
                     </b-button>
                     <b-button class="ml-2" @click="Data.tables = []" variant="danger">
                        <feather-icon icon="XCircleIcon"></feather-icon>
                        {{ $t('clear') }}
                     </b-button>
                  </b-col>
               </b-row>
               <b-row class="mt-2">
                  <b-col>
                     <div>
                        <b-table-simple hover small caption-top responsive border :empty-text="$t('NotFound')">
                           <b-thead>
                              <b-tr>
                                 <b-th
                                    style="
                                       font-weight: 900;
                                       font-size: 14px;
                                       color: black;
                                       max-width: 10px;
                                       text-align: center;
                                    "
                                 >
                                    {{ $t('order') }}
                                 </b-th>
                                 <b-th
                                    style="font-weight: 900; font-size: 14px; color: black"
                                    class="table-b-table-default b-table-sticky-column"
                                 >
                                    {{ $t('district') }}
                                 </b-th>
                                 <b-th
                                    style="font-weight: 900; font-size: 14px; color: black; text-align: center"
                                    class="table-b-table-default b-table-sticky-column"
                                 >
                                    {{ $t('legalCount') }}
                                 </b-th>
                                 <b-th
                                    style="font-weight: 900; font-size: 14px; color: black; text-align: center"
                                    class="table-b-table-default b-table-sticky-column"
                                 >
                                    {{ $t('physicalCount') }}
                                 </b-th>
                              </b-tr>
                           </b-thead>
                           <b-tbody :busy="isBusy" v-for="(item, index) in Data.tables" :key="index">
                              <b-tr>
                                 <b-td
                                    style="
                                       font-weight: 900;
                                       font-size: 14px;
                                       color: black;
                                       max-width: 10px;
                                       text-align: center;
                                    "
                                 >
                                    {{ index + 1 }}
                                 </b-td>
                                 <b-td style="font-size: 14px; color: black">
                                    {{ item.district }}
                                 </b-td>
                                 <b-td style="font-size: 14px; color: black; text-align: center">
                                    <div style="width: 30%; margin: auto">
                                       <form-currency-input v-model="item.legalCount" />
                                    </div>
                                 </b-td>
                                 <b-td style="font-size: 14px; color: black; text-align: center">
                                    <div style="width: 30%; margin: auto">
                                       <form-currency-input v-model="item.physicalCount" />
                                    </div>
                                 </b-td>
                              </b-tr>
                           </b-tbody>
                        </b-table-simple>
                     </div>
                  </b-col>
               </b-row>
               <b-row>
                  <b-col sm="12" md="6" lg="6" class="text-left"></b-col>
                  <b-col sm="12" md="6" lg="6" class="text-right">
                     <b-button :disabled="saveLoading" @click="SaveData" size="sm" variant="outline-success">
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
// service
import MemshipNewContractorService from '@/services/document/memshipnewcontractor.service';
import axios from 'axios';
// components
import {
   BOverlay,
   BCard,
   BRow,
   BCol,
   BSpinner,
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
   BTableSimple,
   BThead,
   BTr,
   BTh,
   BTd,
   BTbody,
   BTfoot,
   BFormFile,
   BIconTrash
} from 'bootstrap-vue';
import RegionService from '@/services/info/region.service';
import FormCurrencyInput from '@/components/forms/form-currency-input.vue';
const defaultTableRow = {
   id: 0,
   monthOn: null,
   regionId: null,
   region: null,
   districtId: null,
   membersCount: 0
};
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
      BSpinner,
      BInputGroup,
      BInputGroupAppend,
      BFormCheckbox,
      BFormTextarea,
      FormCurrencyInput,
      BTableSimple,
      BThead,
      BTr,
      BTh,
      BTd,
      BTbody,
      BTfoot,
      BFormFile,
      BIconTrash
   },
   name: 'Edit',
   data() {
      return {
         axios,
         show: false,
         saveLoading: false,
         Data: {},

         RegionList: [],

         isBusy: false
      };
   },
   computed: {
      FileSrc() {
         return (id) => axios.defaults.baseURL + `MemshipNewContractorService/DownloadFile/${id}`;
      },
      computedTotalLegalCount() {
         const total = this.Data.tables.reduce((sum, item) => sum + Number(item.legalCount || 0), 0);
         this.$set(this.Data, 'totalLegalCount', total);
         return total;
      },

      // Computed property for total physical count
      computedTotalPhysicalCount() {
         const total = this.Data.tables.reduce((sum, item) => sum + Number(item.physicalCount || 0), 0);
         this.$set(this.Data, 'totalPhysicalCount', total);
         return total;
      }
   },
   created() {
      this.show = true;
      MemshipNewContractorService.Get(this.$route.params.id)
         .then((res) => {
            this.Data = res.data;
         })
         .catch((error) => {
            this.makeToast(error.response.data.errors, 'danger');
         })
         .finally(() => {
            this.show = false;
         });
      RegionService.GetAsSelectList(211).then((res) => {
         this.RegionList = res.data;
      });
   },
   methods: {
      UploadFile(event) {
         const formData = new FormData();
         formData.append('files', event.target.files[0]);
         this.fileLoading = true;
         MemshipNewContractorService.UploadFile(formData).then((res) => {
            this.Data.files.push(...res.data);
            this.fileLoading = false;
         });
      },
      DeleteFile(id) {
         MemshipNewContractorService.DeleteFile(id).then(() => {
            this.Data.files = this.Data.files.filter((item) => item.id != id);
         });
      },
      Fill() {
         this.isBusy = true;
         MemshipNewContractorService.FillTable(this.Data.regionId)
            .then((res) => {
               this.Data.tables = res.data;
               this.makeToast(this.$t('SuccessMessage'), 'success');
            })
            .catch((err) => {
               this.showApiError(err);
            })
            .finally(() => {
               this.isBusy = false;
            });
      },
      SaveData() {
         this.saveLoading = true;
         MemshipNewContractorService.Update(this.Data)
            .then((res) => {
               this.makeToast(this.$t('SaveSuccess'), 'success');
               this.$router.push({ name: 'MemshipNewContractor' });
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
