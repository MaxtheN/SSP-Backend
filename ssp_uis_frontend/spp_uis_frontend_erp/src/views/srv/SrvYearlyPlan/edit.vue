<template>
   <b-overlay :show="show">
      <b-row>
         <b-col sm="12" md="12" lg="12">
            <b-card>
               <!-- <validation-observer ref="ValidationDTO"> -->
               <b-row>
                  <b-col sm="12" md="3" class="mb-1">
                     <form-input v-model="Data.docNumber" :disabled="isdisable" :label="$t('docnumber')" />
                  </b-col>
                  <b-col sm="12" md="3" class="mb-1">
                     <form-picker :label="$t('ondate')" :disabled="isdisable" v-model="Data.docOn" />
                  </b-col>
                  <b-col sm="12" md="3" class="mb-1">
                     <form-picker
                        type="year"
                        required
                        format="YYYY"
                        v-model="Data.year"
                        :disabled="isdisable"
                        :label="$t('docyear')"
                     />
                  </b-col>
                  <b-col sm="12" md="3">
                     <form-select
                        :options="MonthList"
                        required
                        :disabled="isdisable"
                        v-model="Data.monthOn"
                        label="month"
                     />
                  </b-col>
               </b-row>
               <b-row>
                  <b-col sm="12" md="12" class="mb-1">
                     <b-form-textarea
                        :disabled="isdisable"
                        id="textarea"
                        rows="2"
                        max-rows="6"
                        v-model="Data.details"
                        :placeholder="$t('detailinfo')"
                     ></b-form-textarea>
                  </b-col>
               </b-row>
               <b-row>
                  <b-col md="4" sm="4" class="mt-2">
                     <h6 class="inputTitle">{{ $t('fileupload') }}</h6>
                     <b-form-file
                        :disabled="isdisable"
                        type="file"
                        :placeholder="$t('Faylni tanlang')"
                        @change="UploadFile"
                     >
                     </b-form-file>
                     <div class="mt-1" v-for="item in Data.files" :key="item.id">
                        <b-link variant="primary" target="_blank" :href="FileSrc(item.id)">{{
                           item.fileName || item.id
                        }}</b-link>
                        <b-button
                           variant="danger"
                           size="sm"
                           :disabled="isdisable"
                           class="ml-1"
                           @click="DeleteFile(item.id)"
                        >
                           <b-icon-trash scale="0.7" />
                        </b-button>
                     </div>
                  </b-col>
               </b-row>
               <hr />
               <b-row>
                  <b-col lg="6">
                     <form-select
                        :options="RegionList"
                        :disabled="isdisable"
                        v-model="regionId"
                        :label="$t('Oblast')"
                     ></form-select>
                  </b-col>
                  <b-col class="pt-2 text-right" lg="6">
                     <b-button @click="Fill" variant="primary" :disabled="!regionId">
                        <b-spinner v-if="isBusy" small></b-spinner>
                        <feather-icon v-else icon="AlignLeftIcon"></feather-icon>
                        {{ $t('Fill') }}
                     </b-button>
                     <b-button :disabled="isdisable" class="ml-2" @click="Items = []" variant="danger">
                        <feather-icon icon="XCircleIcon"></feather-icon>
                        {{ $t('clear') }}
                     </b-button>
                  </b-col>
               </b-row>
               <b-row class="mt-2">
                  <b-col>
                     <div>
                        <b-table-simple
                           hover
                           small
                           caption-top
                           responsive
                           border
                           :empty-text="$t('NotFound')"
                           class="report-table"
                        >
                           <b-thead>
                              <b-tr>
                                 <b-th rowspan="2">
                                    {{ $t('order') }}
                                 </b-th>
                                 <b-th rowspan="2">
                                    {{ $t('region') }}
                                 </b-th>
                                 <b-th rowspan="2">
                                    {{ $t('Cheklangan Xodimlar soni') }}
                                 </b-th>
                                 <b-th>
                                    {{ $t('Bepul xizmatlar') }}
                                 </b-th>
                                 <b-th colspan="2">
                                    {{ $t('Pullik xizmatlar') }}
                                 </b-th>
                                 <b-th rowspan="2">
                                    {{ $t('Yuridik') }}
                                 </b-th>
                                 <b-th rowspan="2">
                                    {{ $t('Iqtisod') }}
                                 </b-th>
                              </b-tr>
                              <b-tr>
                                 <b-th>
                                    {{ $t('reja') }}
                                 </b-th>
                                 <b-th>
                                    {{ $t('reja(soni)') }}
                                 </b-th>
                                 <b-th>
                                    {{ $t('reja(summasi)') }}
                                 </b-th>
                              </b-tr>
                           </b-thead>
                           <b-tbody :busy="isBusy" v-for="(item, index) in Items" :key="index">
                              <b-tr v-if="item?.valueForDistricts">
                                 <b-td>1</b-td>

                                 <b-td>{{ item.region }}</b-td>
                                 <b-td>
                                    <form-currency-input
                                       :disabled="isdisable"
                                       v-model.number="item.regionEmployeeCount"
                                    />
                                 </b-td>
                                 <b-td>
                                    <form-currency-input :disabled="isdisable" v-model.number="item.regionFreeCount" />
                                 </b-td>

                                 <b-td>
                                    <form-currency-input :disabled="isdisable" v-model.number="item.regionPaidCount" />
                                 </b-td>
                                 <b-td>
                                    <form-currency-input disabled v-model.number="item.regionAmount" />
                                 </b-td>
                                 <b-td>
                                    <form-currency-input
                                       :disabled="isdisable"
                                       @input="handleRegionInput(item, index)"
                                       v-model.number="item.regionLegalAmount"
                                    />
                                 </b-td>
                                 <b-td>
                                    <form-currency-input
                                       @input="handleRegionInput(item, index)"
                                       :disabled="isdisable"
                                       v-model.number="item.regionEconomyAmount"
                                    />
                                 </b-td>
                              </b-tr>
                              <template v-for="(item2, i) in item?.valueForDistricts">
                                 <b-tr :key="i + 'p'">
                                    <b-td>
                                       {{ i + 2 }}
                                    </b-td>
                                    <b-td>
                                       {{ item2.district }}
                                    </b-td>
                                    <b-td>
                                       <form-currency-input
                                          :disabled="isdisable"
                                          v-model.number="item2.employeeCount"
                                       />
                                    </b-td>
                                    <b-td>
                                       <form-currency-input :disabled="isdisable" v-model.number="item2.freeCount" />
                                    </b-td>

                                    <b-td>
                                       <form-currency-input :disabled="isdisable" v-model.number="item2.paidCount" />
                                    </b-td>
                                    <b-td>
                                       <form-currency-input disabled v-model.number="item2.amount" />
                                    </b-td>
                                    <b-td>
                                       <form-currency-input
                                          @input="handleInput(item2, i, item)"
                                          :disabled="isdisable"
                                          v-model.number="item2.legalAmount"
                                       />
                                    </b-td>
                                    <b-td>
                                       <form-currency-input
                                          @input="handleInput(item2, i, item)"
                                          :disabled="isdisable"
                                          v-model.number="item2.economyAmount"
                                       />
                                    </b-td>
                                 </b-tr>
                              </template>
                           </b-tbody>
                           <b-tfoot>
                              <b-tr>
                                 <b-th colspan="2">
                                    <span class="ml-3">{{ $t('Total') }}</span>
                                 </b-th>
                                 <b-th class="text-right"> {{ currency(SumEmployeeCount) }}</b-th>
                                 <b-th class="text-right"> {{ currency(SumFreeCount) }}</b-th>
                                 <b-th class="text-right"> {{ currency(SumPaidCount) }}</b-th>
                                 <b-th class="text-right"> {{ currency(SumAmount) }}</b-th>
                                 <b-th class="text-right"> {{ currency(SumLegalAmount) }}</b-th>

                                 <b-th class="text-right"> {{ currency(SumEconomyAmount) }}</b-th>
                              </b-tr>
                           </b-tfoot>
                        </b-table-simple>
                     </div>
                  </b-col>
               </b-row>
               <b-row>
                  <b-col sm="12" md="6" lg="6" class="text-left"></b-col>
                  <b-col sm="12" md="6" lg="6" class="text-right">
                     <b-button :disabled="saveLoading" @click="SaveData" size="sm" variant="outline-success">
                        <b-spinner small v-if="saveLoading" />
                        <feather-icon v-else icon="CheckIcon"></feather-icon>
                        {{ $t('Save') }}
                     </b-button>
                  </b-col>
               </b-row>
               <!-- </validation-observer> -->
            </b-card>
         </b-col>
      </b-row>
   </b-overlay>
</template>
<script>
// service
import SrvApplicationYearlyPlanService from '@/services/srv/SrvYearlyPlan.service';
import axios from 'axios';
import RegionService from '@/services/info/region.service';
import ManualService from '@/services/others/manual.service';

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
import FormCurrencyInput from '@/components/forms/form-currency-input.vue';

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
   data() {
      return {
         isdisable: false,
         show: false,
         saveLoading: false,
         Items: [],
         MonthList: [],

         Data: {
            docNumber: '',
            docOn: '',
            monthOn: '',
            details: '',
            year: '',
            cellTables: [],
            files: []
         },
         regionId: null,
         RegionList: [],
         TablesField: [
            {
               key: 'monthOn',
               label: this.$t('monthOn'),
               sortable: true
            },
            {
               key: 'regionId',
               label: this.$t('region'),
               sortable: true
            },
            {
               key: 'districtId',
               label: this.$t('district'),
               sortable: true
            },
            {
               key: 'membersCount',
               label: this.$t('membersCount'),
               sortable: true
            },
            {
               key: 'actions',
               label: this.$t('actions'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            }
         ],
         isBusy: false
      };
   },
   computed: {
      FileSrc() {
         return (id) => axios.defaults.baseURL + `srv/SrvApplicationYearlyPlan/DownloadFile/${id}`;
      },

      SumAmount() {
         let amount = 0;
         this.Items[0]?.valueForDistricts?.forEach((item) => {
            amount += Number(item.amount);
         });
         return amount + this.Items[0]?.regionAmount;
      },
      SumLegalAmount() {
         let amount = 0;
         this.Items[0]?.valueForDistricts?.forEach((item) => {
            amount += Number(item.legalAmount);
         });
         return amount + this.Items[0]?.regionLegalAmount;
      },
      SumEconomyAmount() {
         let amount = 0;
         this.Items[0]?.valueForDistricts?.forEach((item) => {
            amount += Number(item.economyAmount);
         });
         return amount + this.Items[0]?.regionEconomyAmount;
      },
      SumEmployeeCount() {
         let amount = 0;
         this.Items[0]?.valueForDistricts?.forEach((item) => {
            amount += Number(item.employeeCount);
         });
         return amount + this.Items[0]?.regionEmployeeCount;
      },
      SumFreeCount() {
         let amount = 0;
         this.Items[0]?.valueForDistricts?.forEach((item) => {
            amount += Number(item.freeCount);
         });
         return amount + this.Items[0]?.regionFreeCount;
      },
      SumPaidCount() {
         let amount = 0;
         this.Items[0]?.valueForDistricts?.forEach((item) => {
            amount += Number(item.paidCount);
         });
         return amount + this.Items[0]?.regionPaidCount;
      }
   },
   created() {
      ManualService.GetMonthSelectList().then((res) => {
         this.MonthList = res.data;
      });
      this.show = true;

      if (this.$route.query.isView) {
         this.isdisable = true;
      }
      SrvApplicationYearlyPlanService.Get(this.$route.params.id)
         .then((res) => {
            this.Data = res.data;
            this.Items = res.data.cellTables;
         })
         .catch((error) => {
            this.showApiError(error);
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
         SrvApplicationYearlyPlanService.UploadFile(formData).then((res) => {
            this.Data.files.push(...res.data);
            this.fileLoading = false;
         });
      },
      handleInput(item, index, item3) {
         item3.valueForDistricts[index].amount = item.legalAmount + item.economyAmount;
      },

      handleRegionInput(item, index) {
         item.regionAmount = item.regionLegalAmount + item.regionEconomyAmount;
      },

      DeleteFile(id) {
         SrvApplicationYearlyPlanService.DeleteFile(id).then(() => {
            this.Data.files = this.Data.files.filter((item) => item.id != id);
         });
      },
      Fill() {
         this.isBusy = true;
         SrvApplicationYearlyPlanService.ConvertToCellTables(this.regionId)
            .then((res) => {
               this.Items = res.data;

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
         console.log(this.Items);
         SrvApplicationYearlyPlanService.Update({
            ...this.Data,
            year: +this.Data.year,
            cellTables: [this.Items[0]]
         })
            .then((res) => {
               this.makeToast(this.$t('SaveSuccess'), 'success');
               this.$router.push({ name: 'SrvYearlyPlan' });
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
@import '@/views/report/styles.scss';
</style>
