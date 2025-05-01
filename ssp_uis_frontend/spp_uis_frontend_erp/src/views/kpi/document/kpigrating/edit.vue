<template>
   <b-overlay>
      <b-card>
         <validation-observer ref="ValidationDTO">
            <b-row>
               <b-col sm="12" md="3">
                  <form-input-hrm
                     v-model="Data.docNumber"
                     rules="required"
                     :label="$t('docnumber')"
                     :placeholder="$t('docnumber')"
                  />
               </b-col>
               <b-col sm="12" md="3">
                  <form-picker v-model="Data.docOn" :label="$t('docdate')" />
               </b-col>

               <!-- <b-col sm="12" md="5">
                  <form-select
                     v-model="Data.organizationId"
                     :options="OrganizationList"
                     required-star
                     :label="$t('organization')"
                  />
               </b-col> -->
            </b-row>

            <!-- <b-row>
               <b-col class="pt-2 text-center">
                  <b-button @click="Fill" variant="primary">
                     <feather-icon icon="AlignLeftIcon"></feather-icon>
                     {{ $t('Fill') }}
                  </b-button>
                  <b-button class="ml-2" @click="Data.indicators = []" variant="danger">
                     <feather-icon icon="XCircleIcon"></feather-icon>
                     {{ $t('clear') }}
                  </b-button>
               </b-col>
            </b-row> -->
            <b-row>
               <b-col class="mt-2">
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
                           <b-th style="width: 50px">
                              {{ $t('order') }}
                           </b-th>
                           <b-th style="max-width: 900px">
                              {{ $t('Indicator') }}
                           </b-th>
                           <b-th> {{ $t('Add') }} </b-th>
                        </b-tr>
                     </b-thead>
                     <b-tbody v-for="(item, index) in Data.indicators" :key="index">
                        <b-tr>
                           <b-td style="text-align: center">{{ index + 1 }}</b-td>
                           <b-td>
                              <div style="width: 900px; text-wrap: wrap">{{ item.indicator }}</div>
                           </b-td>
                           <b-td style="text-align: center">
                              <b-button size="sm" variant="success" class="mr-2" @click="Opentable(item)">
                                 <feather-icon icon="EyeIcon"></feather-icon>
                              </b-button>
                              <b-button
                                 v-if="!isView"
                                 size="sm"
                                 variant="primary"
                                 class="mr-2"
                                 @click="Opentable(item)"
                              >
                                 <b-icon-plus></b-icon-plus>
                              </b-button>

                              <b-button size="sm" variant="danger" @click="DeleteTable(item)">
                                 <feather-icon icon="TrashIcon"></feather-icon>
                              </b-button>
                           </b-td>
                        </b-tr>
                     </b-tbody>
                  </b-table-simple>
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

         <!-- Indicator Table -->
         <b-modal size="xl" v-model="showModal">
            <validation-observer ref="ValidationDTO2">
               <b-row>
                  <b-col sm="12" md="2">
                     <form-input-hrm
                        v-model="indicatortabrow.minIndicator"
                        :label="$t('minIndicator')"
                        rules="required"
                        :placeholder="$t('minIndicator')"
                     />
                  </b-col>
                  <b-col sm="12" md="2">
                     <form-input-hrm
                        rules="required"
                        v-model="indicatortabrow.maxIndicator"
                        :label="$t('maxIndicator')"
                        :placeholder="$t('maxIndicator')"
                     />
                  </b-col>
                  <b-col sm="12" md="2">
                     <form-input-hrm
                        rules="required"
                        v-model="indicatortabrow.score"
                        :label="$t('score')"
                        :placeholder="$t('score')"
                     />
                  </b-col>
                  <b-col sm="12" md="3">
                     <form-select
                        @change="
                           (id) =>
                              (indicatortabrow.uniteOfMeasure = uniteOfMeasureList.find(
                                 (item) => item.value == id
                              ).text)
                        "
                        v-model="indicatortabrow.uniteOfMeasureId"
                        :options="uniteOfMeasureList"
                        :label="$t('uniteOfMeasure')"
                     />
                  </b-col>
                  <b-col sm="12" md="2">
                     <b-button variant="primary" size="sm" class="mt-2" @click="IndicatorAddTabrow">
                        <b-icon-plus></b-icon-plus>
                     </b-button>
                  </b-col>
                  <b-col>
                     <b-table
                        ref="refInvoiceListTable"
                        :items="indicatorTables"
                        responsive
                        :fields="fields"
                        primary-key="id"
                        no-border-collapse
                        show-empty
                        :empty-text="$t('NotFound')"
                        class="position-relative report-table"
                     >
                        <template #cell(id)="{ item, index }">
                           {{ index + 1 }}
                        </template>
                        <template #cell(actions)="{ item, index }">
                           <div class="text-center">
                              <!-- edit -->
                              <b-link @click="EditTabrow(item, index)" style="margin-right: 5px; cursor: pointer">
                                 <feather-icon icon="EditIcon"></feather-icon>
                              </b-link>
                              <!-- delete -->

                              <b-link class="text-danger" @click="DeleteTabrow(index)" style="cursor: pointer">
                                 <feather-icon icon="TrashIcon"></feather-icon>
                              </b-link>
                           </div>
                        </template>
                        >
                     </b-table>
                  </b-col>
               </b-row>
            </validation-observer>
            <template #modal-footer="{ cancel }">
               <b-button v-if="!isView" size="sm" variant="success" @click="AddIndicatorTable">
                  {{ $t('Save') }}
               </b-button>
               <b-button size="sm" v-if="!isView" variant="danger" @click="cancel"> {{ $t('Cancel') }}</b-button>
            </template>
         </b-modal>
      </b-card>
   </b-overlay>
</template>
<script>
// service
import ManualService from '@/services/others/manual.service';
import KpiGratingService from '@/services/kpi/kpigrating.service';
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
import FormInputTranslate from '@/components/translates/FormInputTranslate.vue';

export default {
   components: {
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
      FormInputTranslate,
      EdocInfo
   },
   data() {
      return {
         isBusy: false,
         isView: false,
         uniteOfMeasureList: [],
         tables: [],
         indicatorTabrowIndex: null,
         indicatortabrow: {
            minIndicator: '',
            maxIndicator: '',
            score: null,
            uniteOfMeasureId: null,
            uniteOfMeasure: ''
         },
         indicatorTables: [],
         showModal: false,
         indicatorList: [],
         Items: [],
         Data: {
            docOn: null,
            docNumber: null,
            organizationId: null,
            indicators: []
         },
         fields: [
            {
               key: 'id',
               label: this.$t('order'),
               thClass: 'text-center',
               tdClass: 'text-center',
               thStyle: {
                  minWidth: '20px'
               }
            },
            {
               key: 'minIndicator',
               label: this.$t('minIndicator'),
               thClass: 'text-center',
               tdClass: 'text-center'
            },
            {
               key: 'maxIndicator',
               label: this.$t('maxIndicator'),
               thClass: 'text-center',
               tdClass: 'text-center'
            },
            {
               key: 'score',
               label: this.$t('score'),
               thClass: 'text-center',
               tdClass: 'text-center'
            },
            {
               key: 'uniteOfMeasure',
               label: this.$t('uniteOfMeasure'),
               thClass: 'text-center',
               tdClass: 'text-center'
            },
            {
               key: 'actions',
               label: this.$t('actions'),
               thClass: 'text-center',
               tdClass: 'text-center'
            }
         ],
         IndicatorId: null
      };
   },
   created() {
      // ManualService.OrganizationAsSelectListByGroup([1, 3]).then((res) => {
      //    this.OrganizationList = res.data;
      // });

      this.isView = this.$route.query.isView;

      UniteOfMeasureService.GetAsSelectList().then((res) => {
         this.uniteOfMeasureList = res.data;
      });

      IndicatorService.GetAsSelectList()
         .then((res) => {
            this.indicatorList = res.data;
         })
         .catch((error) => {
            this.showApiError(error);
         });

      KpiGratingService.Get(this.$route.params.id)
         .then((res) => {
            this.Data = res.data;
            if (this.$route.params.id == 0) {
               KpiGratingService.FillIndicator()
                  .then((res2) => {
                     this.Data.indicators = res2.data;
                  })
                  .catch((error) => {
                     this.showApiError(error);
                  });
            }
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
               KpiGratingService.Update(this.Data)
                  .then(() => {
                     this.makeToast(this.$t('SaveSuccess'), 'success');
                     this.$router.push({ name: 'KpiGrating' });
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
      // Fill() {
      //    KpiGratingService.FillIndicator()
      //       .then((res) => {
      //          this.Data.indicators = res.data;
      //       })
      //       .catch((error) => {
      //          this.showApiError(error);
      //       });
      // },
      IndicatorAddTabrow() {
         this.$refs.ValidationDTO2.validate().then((success) => {
            if (success) {
               if (this.indicatorTabrowIndex == null) {
                  this.indicatorTables.push(this.indicatortabrow);
                  this.indicatortabrow = {};
                  this.indicatorTabrowIndex = null;
                  this.$refs.ValidationDTO2.reset();
               } else {
                  this.indicatorTables = this.indicatorTables.map((item, ind) => {
                     if (ind == this.indicatorTabrowIndex) {
                        return this.indicatortabrow;
                     } else {
                        return item;
                     }
                  });
                  this.indicatortabrow = {};
                  this.indicatorTabrowIndex = null;
                  this.$refs.ValidationDTO2.reset();
               }
            }
         });
      },
      DeleteTabrow(index) {
         this.indicatorTables.splice(index, 1);
      },

      EditTabrow(item, index) {
         this.indicatorTabrowIndex = index;
         this.indicatortabrow = { ...item };
      },

      DeleteTable(item) {
         this.Data.indicators = this.Data.indicators.filter((item1) => item1.indicatorId != item.indicatorId);
      },

      AddIndicatorTable() {
         if (this.IndicatorId) {
            this.Data.indicators.forEach((item) => {
               if (item.indicatorId == this.IndicatorId) {
                  item.tables = [...this.indicatorTables];
               }
            });
         }
         this.showModal = false;
      },
      Opentable(item) {
         this.IndicatorId = item.indicatorId;
         this.showModal = true;
         this.indicatorTables = [...item.tables];
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
