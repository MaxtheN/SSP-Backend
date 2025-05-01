<template>
   <b-overlay :show="show">
      <validation-observer ref="ValidationDTO">
         <b-card>
            <b-row>
               <b-col sm="12" md="3" class="mb-1">
                  <form-input-hrm v-model="Data.code" :label="$t('kode')" :placeholder="$t('kode')" rules="required" />
               </b-col>
               <b-col sm="12" md="3" class="mb-1">
                  <form-input-hrm v-model="Data.orderCode" :label="$t('orderCode')" :placeholder="$t('orderCode')" />
               </b-col>
               <b-col sm="12" md="3" class="mb-1">
                  <form-input-translate
                     v-model="Data.shortName"
                     @update:translates="(e) => (Data.translates = e)"
                     :translates="Data.translates"
                     column-name="short_name"
                     required
                     :label="$t('shortname')"
                     :placeholder="$t('shortname')"
                  />
               </b-col>
               <b-col sm="12" md="3" class="mb-1">
                  <form-input-translate
                     v-model="Data.fullName"
                     @update:translates="(e) => (Data.translates = e)"
                     :translates="Data.translates"
                     column-name="full_name"
                     required
                     :label="$t('fullname')"
                     :placeholder="$t('fullname')"
                  />
               </b-col>
               <b-col sm="12" md="3" class="mb-1">
                  <form-input-hrm
                     v-model="Data.structureType"
                     :label="$t('structureType')"
                     :placeholder="$t('structureType')"
                     rules="required"
                  />
               </b-col>
               <b-col sm="12" md="3" class="mb-1">
                  <form-input-hrm
                     v-model="Data.corrCoef"
                     :label="$t('corrCoef')"
                     :placeholder="$t('corrCoef')"
                     rules="required"
                  />
               </b-col>
            </b-row>
         </b-card>

         <!-- tabs -->
         <b-card no-body>
            <b-tabs fill v-model="tableTabs" card nav-class="mb-0 mx-2" nav-wrapper-class="pb-0 align-items-center">
               <!-- structureCalculationKind -->
               <b-tab :title="$t('calculationKind')" active lazy>
                  <b-card-text>
                     <div class="d-flex justify-content-center mb-2">
                        <b-button variant="outline-primary" class="mx-2" @click="dialogCalculationKind = true">
                           <feather-icon icon="PlusIcon" />
                           {{ $t('Add') }}
                        </b-button>

                        <!-- calculationkind list dialog -->
                        <b-modal size="xl" :title="$t('calculationKind')" v-model="dialogCalculationKind" hide-footer>
                           <CalculationKindList
                              :calculation-kind-id="''"
                              :selectable="true"
                              @row-selected="AddCalculationKindTableRow"
                           />
                        </b-modal>
                        <form-input v-model="filtersCalculationKind" :placeholder="$t('search')" />
                     </div>
                     <b-table
                        :fields="fieldsCalculationKind"
                        :items="Data.structureCalculationKind"
                        :filter="filtersCalculationKind"
                        small
                        responsive="sm"
                        hover
                        show-empty
                        bordered
                        :empty-text="$t('NotFound')"
                     >
                        <template #cell(percentage)="{ item }">
                           <form-input-hrm
                              v-model.number="item.percentage"
                              type="number"
                              rules="max_value:100|min_value:0"
                           />
                        </template>
                        <template #cell(actions)="{ index }">
                           <div class="text-center">
                              <b-link class="text-danger">
                                 <feather-icon @click="DeleteCalculationKind(index)" icon="Trash2Icon"></feather-icon>
                              </b-link>
                           </div>
                        </template>
                     </b-table>
                  </b-card-text>
               </b-tab>

               <!-- organizations -->
               <b-tab :title="$t('OrganizationsList')" lazy>
                  <b-card-text>
                     <div class="d-flex justify-content-center mb-2">
                        <b-button variant="outline-primary" class="mx-2" @click="dialogOrganization = true">
                           <feather-icon icon="PlusIcon" />
                           {{ $t('Add') }}
                        </b-button>
                        <!-- Organization list dialog -->
                        <b-modal size="xl" :title="$t('Organization')" v-model="dialogOrganization" hide-footer>
                           <OrganizationList :selectable="true" @row-selected="AddOrganizationTableRow" />
                        </b-modal>
                        <form-input v-model="filtersOrganization" :placeholder="$t('search')" />
                     </div>
                     <b-table
                        :fields="fieldsOrganization"
                        :items="OrganizationList"
                        :filter="filtersOrganization"
                        small
                        responsive="sm"
                        hover
                        show-empty
                        bordered
                        :empty-text="$t('NotFound')"
                     >
                        <template #cell(actions)="{ item }">
                           <div class="text-center">
                              <b-link class="text-danger">
                                 <feather-icon @click="DeleteOrganization(item)" icon="Trash2Icon"></feather-icon>
                              </b-link>
                           </div>
                        </template>
                     </b-table>
                  </b-card-text>
               </b-tab>

               <!-- structureStaffingIndicator -->
               <b-tab :title="$t('staffingindicator')" lazy>
                  <b-card-text>
                     <div class="d-flex justify-content-center mb-2">
                        <b-button variant="outline-primary" class="mx-2" @click="dialogStaffingIndicator = true">
                           <feather-icon icon="PlusIcon" />
                           {{ $t('Add') }}
                        </b-button>
                        <!-- StaffingIndicator list dialog -->
                        <b-modal
                           size="xl"
                           :title="$t('staffingindicator')"
                           v-model="dialogStaffingIndicator"
                           hide-footer
                        >
                           <StaffingIndicatorList :selectable="true" @row-selected="AddStaffingIndicatorTableRow" />
                        </b-modal>
                        <form-input v-model="filtersStaffingIndicator" :placeholder="$t('search')" />
                     </div>
                     <b-table
                        :fields="fieldsStaffingIndicator"
                        :items="Data.structureStaffingIndicator"
                        small
                        responsive="sm"
                        hover
                        show-empty
                        bordered
                        :empty-text="$t('NotFound')"
                     >
                        <template #cell(calcOrderCode)="{ item }">
                           <form-input-hrm v-model="item.calcOrderCode" rules="numeric|min_value:0" />
                        </template>
                        <template #cell(displayOrderCode)="{ item }">
                           <form-input-hrm v-model="item.displayOrderCode" rules="numeric|min_value:0" />
                        </template>
                        <template #cell(isTotal)="{ item }">
                           <b-form-checkbox v-model="item.isTotal" />
                        </template>
                        <template #cell(isCalculationKindTotal)="{ item }">
                           <b-form-checkbox v-model="item.isCalculationKindTotal" />
                        </template>
                        <template #cell(indicatorTables)="{ item }">
                           <form-select
                              v-model="item.indicatorTables"
                              :options="IndicatorTablesList(item.staffingIndicatorId)"
                              multiple
                           />
                        </template>
                        <template #cell(percentage)="{ item }">
                           <form-input-hrm v-model="item.percentage" type="number" rules="max_value:100|min_value:0" />
                        </template>
                        <template #cell(actions)="{ index }">
                           <div class="text-center">
                              <b-link class="text-danger">
                                 <feather-icon @click="DeleteStaffingIndicator(index)" icon="Trash2Icon"></feather-icon>
                              </b-link>
                           </div>
                        </template>
                     </b-table>
                  </b-card-text>
               </b-tab>

               <!-- structurePosition -->
               <b-tab :title="$t('position')" lazy>
                  <b-card-text>
                     <validation-observer disabled ref="ValidationPositionRow">
                        <b-row>
                           <b-col sm="12" md="3">
                              <form-select
                                 :options="PositionTypeList"
                                 v-model="structurePositionRow.positionTypeId"
                                 :label="$t('PositionType')"
                                 required-star
                                 :placeholder="$t('PositionType')"
                              />
                           </b-col>
                           <b-col sm="12" md="3">
                              <form-select
                                 :options="PositionCategoryList"
                                 v-model="structurePositionRow.positionCategoryId"
                                 :label="$t('PositionCategory')"
                                 required-star
                                 :placeholder="$t('PositionType')"
                              />
                           </b-col>
                           <b-col sm="12" md="3">
                              <PositionSelect
                                 required
                                 v-model="structurePositionRow.positionId"
                                 :position="structurePositionRow.position"
                                 @update:data="onUpdatePosition"
                              />
                           </b-col>

                           <b-col sm="12" md="3">
                              <form-select
                                 :options="TariffScaleTypeList"
                                 v-model="structurePositionRow.tariffScaleTypeId"
                                 :label="$t('tariffScaleType')"
                                 required-star
                                 :placeholder="$t('tariffScaleType')"
                              />
                           </b-col>
                           <b-col sm="12" md="3">
                              <form-select
                                 :options="TariffScaleList"
                                 v-model="structurePositionRow.tariffScaleId"
                                 :label="$t('TariffScale')"
                                 required-star
                                 :placeholder="$t('TariffScale')"
                              />
                           </b-col>

                           <b-col sm="12" md="3">
                              <form-select
                                 :options="RankList"
                                 v-model="structurePositionRow.rankId"
                                 :label="$t('rankid')"
                                 required-star
                                 :placeholder="$t('rankid')"
                              />
                           </b-col>

                           <b-col sm="12" md="1" class="mt-2">
                              <b-button variant="primary" @click="AddPositionTableRow">
                                 <feather-icon icon="PlusIcon"></feather-icon>
                              </b-button>
                           </b-col>
                        </b-row>
                     </validation-observer>

                     <b-table
                        :fields="fieldsPosition"
                        :items="Data.structurePosition"
                        small
                        responsive="sm"
                        hover
                        show-empty
                        bordered
                        :empty-text="$t('NotFound')"
                     >
                        <template #cell(actions)="{ index }">
                           <div class="text-center">
                              <b-link class="text-danger">
                                 <feather-icon @click="DeletePosition(index)" icon="Trash2Icon"></feather-icon>
                              </b-link>
                           </div>
                        </template>
                     </b-table>
                  </b-card-text>
               </b-tab>
            </b-tabs>
         </b-card>

         <!-- save button -->
         <b-col cols="12" class="text-right mb-2">
            <b-button :disabled="saveLoading" @click="SaveData" variant="outline-success">
               <feather-icon icon="CheckIcon"></feather-icon>
               {{ $t('Save') }}
            </b-button>
         </b-col>
      </validation-observer>
   </b-overlay>
</template>
<script>
// service
import OrganizationalStructureService from '@/services/info/organizationalstructure.service';
import PositionTypeService from '@/services/hrm/positiontype.service';
import PositionCategoryService from '@/services/hrm/positioncategory.service';
import TariffScaleService from '@/services/hrm/tariffscale.service';
import ManualService from '@/services/others/manual.service';
import OrganizationService from '@/services/managment/organization.service';
import StaffingIndicatorService from '@/services/hrm/staffingindicator.service';

// components
import {
   BOverlay,
   BCard,
   BRow,
   BCol,
   BButton,
   BModal,
   BInputGroup,
   BInputGroupAppend,
   BFormCheckbox,
   BFormTextarea,
   BTab,
   BTabs,
   BTable,
   BCardText,
   BLink,
   BFormInput
} from 'bootstrap-vue';
import FormInputTranslate from '@/components/translates/FormInputTranslate.vue';
import CalculationKindList from '@/views/components/hrm/CalculationKindList.vue';
import OrganizationList from '@/views/components/organization/OrganizationList.vue';
import StaffingIndicatorList from '@/views/components/staffingindicator/StaffingIndicatorList.vue';
import PositionSelect from '@/views/components/position/PositionSelect.vue';

const structurePositionRowDef = {
   id: 0,
   positionId: null,
   position: '',
   positionTypeId: null,
   positionType: null,
   positionCategoryId: null,
   positionCategory: null,
   tariffScaleTypeId: null,
   tariffScaleType: null,
   tariffScaleId: null,
   tariffScale: null,
   rankId: null,
   rank: null,
   amount: 0,
   staffingQuantity: 0
};
export default {
   components: {
      BOverlay,
      BCard,
      BRow,
      BCol,
      BButton,
      BModal,
      BInputGroup,
      BInputGroupAppend,
      BFormCheckbox,
      BFormTextarea,
      BTab,
      BTabs,
      BCardText,
      BTable,
      BLink,
      BFormInput,
      FormInputTranslate,
      CalculationKindList,
      OrganizationList,
      StaffingIndicatorList,
      PositionSelect
   },
   name: 'OrganizationalStructureEdit',
   data() {
      return {
         show: false,
         saveLoading: false,
         tableTabs: 0,
         OrganizationList: [],
         PositionTypeList: [],
         PositionCategoryList: [],
         TariffScaleList: [],
         TariffScaleTypeList: [],
         RankList: [],
         StaffingIndicatorList: [],
         filtersCalculationKind: '',
         dialogCalculationKind: false,
         filtersOrganization: '',
         dialogOrganization: false,
         filtersStaffingIndicator: '',
         dialogStaffingIndicator: false,
         Data: {
            orderCode: '',
            code: '',
            structureType: null,
            shortName: '',
            fullName: '',
            corrCoef: null,
            translates: [],
            structureCalculationKind: [],
            structurePosition: [],
            structureStaffingIndicator: []
         },
         structurePositionRow: { ...structurePositionRowDef },
         fieldsCalculationKind: [
            {
               key: 'calculationKindId',
               label: this.$t('id')
            },
            {
               key: 'calculationKindName',
               label: this.$t('calculationKind')
            },
            {
               key: 'percentage',
               label: this.$t('percentage'),
               thClass: 'text-center',
               tdClass: 'text-center',
               thStyle: { width: '150px' }
            },
            {
               key: 'actions',
               label: this.$t('actions'),
               thClass: 'text-center',
               tdClass: 'text-center'
            }
         ],
         fieldsOrganization: [
            {
               key: 'inn',
               label: this.$t('inn')
            },
            {
               key: 'shortName',
               label: this.$t('shortname')
            },
            {
               key: 'actions',
               label: this.$t('actions'),
               thClass: 'text-center',
               tdClass: 'text-center'
            }
         ],
         fieldsStaffingIndicator: [
            {
               key: 'staffingIndicatorId',
               label: this.$t('id'),
               sort: true
            },
            {
               key: 'staffingIndicatorName',
               label: this.$t('staffingindicator')
            },
            {
               key: 'calcOrderCode',
               label: this.$t('calcOrderCode')
            },
            {
               key: 'displayOrderCode',
               label: this.$t('displayOrderCode')
            },
            {
               key: 'isTotal',
               label: this.$t('totals')
            },
            {
               key: 'isCalculationKindTotal',
               label: this.$t('isCalculationKindTotal')
            },
            {
               key: 'indicatorTables',
               label: this.$t('staffingindicator'),
               thStyle: { width: '300px' }
            },
            {
               key: 'percentage',
               label: this.$t('percentage'),
               thStyle: { width: '150px' }
            },
            {
               key: 'actions',
               label: this.$t('actions'),
               thClass: 'text-center',
               tdClass: 'text-center'
            }
         ],
         fieldsPosition: [
            {
               key: 'actions',
               sticky: true,
               label: this.$t('actions'),
               thClass: 'text-center',
               tdClass: 'text-center'
            },
            {
               key: 'id',
               label: this.$t('id'),
               sort: true
            },
            {
               key: 'positionType',
               label: this.$t('PositionType'),
               sort: true
            },
            {
               key: 'positionCategory',
               label: this.$t('PositionCategory'),
               sort: true
            },
            {
               key: 'position',
               label: this.$t('position')
            },
            {
               key: 'tariffScaleType',
               label: this.$t('tariffScaleType'),
               sort: true
            },
            {
               key: 'tariffScale',
               label: this.$t('TariffScale'),
               sort: true
            },
            {
               key: 'schoolGroupContingent',
               label: this.$t('edugroupcontingent'),
               sort: true
            },
            {
               key: 'amount',
               label: this.$t('baseSum')
            },
            {
               key: 'rank',
               label: this.$t('rankid')
            }
         ]
      };
   },
   created() {
      this.show = true;
      OrganizationalStructureService.Get(this.$route.params.id)
         .then((res) => {
            this.Data = res.data;
         })
         .catch((error) => {
            this.makeToast(error.response.data.errors, 'danger');
         })
         .finally(() => {
            this.show = false;
         });

      PositionTypeService.GetAsSelectList().then((res) => {
         if (Array.isArray(res.data)) {
            this.PositionTypeList = res.data;
         }
      });

      PositionCategoryService.GetAsSelectList().then((res) => {
         if (Array.isArray(res.data)) {
            this.PositionCategoryList = res.data;
         }
      });
      TariffScaleService.GetAsSelectList()
         .then((res) => {
            this.TariffScaleList = res.data;
         })
         .catch((error) => {
            this.makeToast(error.response.data.errors, 'danger');
         });

      ManualService.TariffScaleTypeSelectList({}).then((res) => {
         if (Array.isArray(res.data)) {
            this.TariffScaleTypeList = res.data;
         }
      });

      StaffingIndicatorService.GetAsSelectList({
         filterByOrganizationalStructure: false
      }).then((res) => {
         if (Array.isArray(res.data)) {
            this.StaffingIndicatorList = res.data;
         }
      });
   },
   watch: {
      'structurePositionRow.tariffScaleId': {
         handler(val) {
            if (val) {
               this.GetRankList(val);
            } else {
               this.RankList = [];
            }
         }
      },
      tableTabs: {
         handler(val) {
            if (val == 1) {
               this.GetOrganizationList();
            }
         }
      }
   },
   computed: {
      IndicatorTablesList() {
         return (id) => this.StaffingIndicatorList.filter((e) => e.id != id);
      }
   },
   methods: {
      GetRankList(tariffScaleId) {
         TariffScaleService.GetTableAsSelectList(tariffScaleId).then((res) => {
            this.RankList = res.data;
         });
      },
      GetOrganizationList() {
         OrganizationService.GetList({
            orderType: 'asc',
            page: 1,
            pageSize: 1000,
            organizationalStructureId: this.Data.id
         }).then((res) => {
            this.OrganizationList = res.data.rows;
         });
      },
      async UpdateOrgTable(organizationId, structureId) {
         try {
            await OrganizationService.UpdateStructure({
               organizationId,
               structureId
            });

            this.GetOrganizationList();
         } catch (err) {
            console.log(err);
         }
      },
      async AddOrganizationTableRow(e) {
         this.dialogOrganization = false;
         this.UpdateOrgTable(e.id, this.Data.id);
      },
      DeleteOrganization(e) {
         // ochirish uchun structureId:null yuboriladi
         this.UpdateOrgTable(e.id, null);
      },
      AddCalculationKindTableRow(e) {
         this.dialogCalculationKind = false;
         const index = this.Data.structureCalculationKind.findIndex((s) => s.calculationKindId == e.id);
         if (index > -1) {
            this.makeToast(this.$t('duplicatedCalculationKind'), 'danger');
            return false;
         }
         if (e) {
            this.Data.structureCalculationKind.push({
               id: 0,
               calculationKindId: e.id,
               calculationKindName: e.fullName,
               percentage: 0
            });
         }
      },
      AddStaffingIndicatorTableRow(e) {
         this.dialogStaffingIndicator = false;
         const index = this.Data.structureStaffingIndicator.findIndex((s) => s.StafffingIndicatorId == e.id);
         if (index > -1) {
            this.makeToast(this.$t('duplicatedStaffingIndicator'), 'danger');
            return false;
         }
         if (e) {
            this.Data.structureStaffingIndicator.push({
               id: 0,
               isTotal: false,
               isCalculationKindTotal: false,
               calcOrderCode: null,
               displayOrderCode: null,
               staffingIndicatorId: e.id,
               staffingIndicatorName: e.fullName,
               percentage: 0,
               indicatorTables: []
            });
         }
      },
      AddPositionTableRow() {
         this.$refs.ValidationPositionRow.validate().then((success) => {
            if (success) {
               this.structurePositionRow.positionType = this.PositionTypeList.find(
                  (e) => e.value == this.structurePositionRow.positionTypeId
               )?.text;
               this.structurePositionRow.positionCategory = this.PositionCategoryList.find(
                  (e) => e.value == this.structurePositionRow.positionCategoryId
               )?.text;
               this.structurePositionRow.tariffScaleType = this.TariffScaleTypeList.find(
                  (e) => e.value == this.structurePositionRow.tariffScaleTypeId
               )?.text;
               this.structurePositionRow.tariffScale = this.TariffScaleList.find(
                  (e) => e.value == this.structurePositionRow.tariffScaleId
               )?.text;
               this.structurePositionRow.rank = this.RankList.find(
                  (e) => e.value == this.structurePositionRow.rankId
               )?.text;
               this.Data.structurePosition.push(this.structurePositionRow);
               this.structurePositionRow = { ...this.structurePositionRowDef };
               this.$refs.ValidationPositionRow.reset();
            }
         });
      },
      onUpdatePosition(e) {
         this.structurePositionRow.position = e.fullName;
      },
      DeleteCalculationKind(e) {
         this.Data.structureCalculationKind.splice(e, 1);
      },
      DeleteStaffingIndicator(e) {
         this.Data.structureStaffingIndicator.splice(e, 1);
      },
      DeletePosition(e) {
         this.Data.structurePosition.splice(e, 1);
      },
      SaveData() {
         this.$refs.ValidationDTO.validate().then((success) => {
            if (success) {
               this.saveLoading = true;
               OrganizationalStructureService.Update(this.Data)
                  .then(() => {
                     this.makeToast(this.$t('SaveSuccess'), 'success');
                     this.$router.push({ name: 'OrganizationalStructure' });
                  })
                  .catch((err) => {
                     this.showApiError(err);
                  })
                  .finally(() => {
                     this.saveLoading = false;
                  });
            }
         });
      }
   }
};
</script>

<style>
.col-form-label,
label {
   line-height: initial;
}
</style>
