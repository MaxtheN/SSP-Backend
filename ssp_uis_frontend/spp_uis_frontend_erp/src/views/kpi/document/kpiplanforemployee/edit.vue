<template>
   <b-overlay>
      <b-card>
         <validation-observer ref="ValidationDTO">
            <b-row>
               <b-col sm="12" md="3">
                  <form-input-hrm
                     v-model="Data.docNumber"
                     :disabled="isView"
                     rules="required"
                     :label="$t('docnumber')"
                     :placeholder="$t('docnumber')"
                  />
               </b-col>
               <b-col sm="12" md="3">
                  <form-picker :disabled="isView" v-model="Data.docOn" :label="$t('docdate')" />
               </b-col>
               <b-col sm="12" md="5">
                  <form-select
                     :disabled="isView"
                     v-model="Data.organizationId"
                     :options="OrganizationList"
                     required-star
                     :label="$t('organization')"
                  />
               </b-col>
               <b-col class="pt-2 text-center" cols="12">
                  <b-button @click="Fill" v-if="!isView" :disabled="!Data.organizationId" variant="primary">
                     <feather-icon icon="AlignLeftIcon"></feather-icon>
                     {{ $t('Fill') }}
                  </b-button>
                  <b-button class="ml-2" v-if="!isView" @click="Data.tables = []" variant="danger">
                     <feather-icon icon="XCircleIcon"></feather-icon>
                     {{ $t('clear') }}
                  </b-button>
               </b-col>
            </b-row>

            <b-row>
               <b-col class="mt-2">
                  <b-overlay :show="isBusy">
                     <b-table-simple
                        sticky-header="150vh"
                        :busy="isBusy"
                        responsive
                        :empty-text="$t('NotFound')"
                        class="report-table"
                     >
                        <b-thead>
                           <b-tr>
                              <b-th style="max-width: 20px">
                                 {{ $t('order') }}
                              </b-th>
                              <b-th style="min-width: 200px">
                                 {{ $t('employee') }}
                              </b-th>
                              <b-th style="width: 200px"> {{ $t('department') }} </b-th>
                              <b-th v-for="indicator in indicatorList" style="width: 300px" :key="indicator.value">
                                 {{ indicator.text }}
                              </b-th>
                           </b-tr>
                        </b-thead>
                        <b-tbody v-for="(item, index) in Data.tables" :key="index">
                           <b-tr>
                              <b-td style="text-align: center">{{ index + 1 }}</b-td>
                              <b-td style="max-width: 200px; text-wrap: wrap">{{ item.employeeManage }}</b-td>
                              <b-td>
                                 <div style="width: 200px; text-wrap: wrap">
                                    {{ item.department }}
                                 </div>
                              </b-td>
                              <b-td v-for="create in item.creates" style="width: 300px" :key="create.indicatorId">
                                 <form-currency-input v-model="create.amount" :disabled="!create.isAble" />
                              </b-td>
                           </b-tr>
                        </b-tbody>
                     </b-table-simple>
                  </b-overlay>
               </b-col>
            </b-row>

            <b-row class="mt-2">
               <b-col sm="12" md="6" lg="6" class="text-left"></b-col>
               <b-col sm="12" md="6" lg="6" class="text-right">
                  <b-button @click="SaveData" v-if="!isView" size="sm" variant="outline-success">
                     <feather-icon icon="CheckIcon"></feather-icon>
                     {{ $t('Save') }}
                  </b-button>
               </b-col>
            </b-row>
         </validation-observer>
      </b-card>
   </b-overlay>
</template>
<script>
// service

import ManualService from '@/services/others/manual.service';
import KpiPlanForEmployeeService from '@/services/kpi/kpiplanforemployee.service';
import EdocInfo from '@/views/components/appeal/EdocInfo.vue';
import IndicatorService from '@/services/kpi/indicator.service';
import UniteOfMeasureService from '@/services/info/uniteofmeasure.service';
// components
import {
   BOverlay,
   BIconPlus,
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
      FormCurrencyInput,
      BIconPlus,
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
      BTable,
      BTh,
      BTd,
      BTbody,
      BTfoot,
      BFormFile,
      BIconTrash,

      EdocInfo
   },
   data() {
      return {
         isBusy: true,
         OrganizationList: [],
         isView: false,
         tables: [],
         indicatorList: [],
         Data: {
            docOn: null,
            docNumber: null,
            organizationId: null,
            tables: []
         }
      };
   },
   created() {
      ManualService.OrganizationAsSelectListByGroup([1, 3]).then((res) => {
         this.OrganizationList = res.data;
      });

      IndicatorService.GetAsSelectList().then((res) => {
         this.indicatorList = res.data;
      });

      this.isView = this.$route.query.isView;

      KpiPlanForEmployeeService.Get(this.$route.params.id)
         .then((res) => {
            this.Data = res.data;
            this.isBusy = false;
         })
         .catch((error) => {
            this.showApiError(error);
         });
   },

   computed: {},
   methods: {
      SaveData() {
         this.$refs.ValidationDTO.validate().then((success) => {
            if (success) {
               this.saveLoading = true;
               KpiPlanForEmployeeService.Update(this.Data)
                  .then(() => {
                     this.makeToast(this.$t('SaveSuccess'), 'success');
                     this.$router.push({ name: 'KpiPlanForEmployee' });
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
      Fill() {
         if (this.Data.organizationId) {
            this.isBusy = true;
            KpiPlanForEmployeeService.FillTable(this.Data.organizationId)
               .then((res) => {
                  this.Data.tables = res.data;
                  this.isBusy = false;
               })
               .catch((error) => {
                  this.showApiError(error);
               });
         }
      }
   }
};
</script>

<style lang="scss">
.report-table {
   thead {
      th {
         text-align: center;
         vertical-align: middle;
      }
   }

   td {
      white-space: nowrap;
   }

   .table:not(.table-dark) {
      td,
      th {
         border: 1px solid #ebe9f1 !important;
      }
   }
}

.breadcrumb-item.active {
   color: var(--primary);
   cursor: pointer;
}

.text-nowrap {
   white-space: nowrap !important;
}
</style>
