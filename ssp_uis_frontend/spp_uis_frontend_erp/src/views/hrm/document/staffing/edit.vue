<template>
   <b-overlay :show="$store.getters['staffingStore/getLoading']">
      <b-row>
         <b-col sm="12" md="12">
            <validation-observer ref="validateTo">
               <b-card>
                  <b-row>
                     <b-col sm="12" md="2">
                        <form-picker
                           :label="$t('startdate')"
                           v-model="$store.getters['staffingStore/getData'].startOn"
                           required
                        >
                        </form-picker>
                     </b-col>
                     <b-col sm="12" md="2">
                        <form-picker
                           format="YYYY"
                           type="year"
                           required
                           v-model="$store.getters['staffingStore/getData'].financeYear"
                           :label="$t('financeYear')"
                        >
                        </form-picker>
                     </b-col>
                     <b-col sm="12" md="2">
                        <label for> {{ $t('summary') }} <span style="color: red">*</span> </label>
                        <b-form-input
                           type="number"
                           disabled
                           v-model="$store.getters['staffingStore/getData'].docSum"
                           required
                        >
                        </b-form-input>
                     </b-col>
                     <b-col sm="12" md="3">
                        <form-select
                           :label="$t('staffingType')"
                           :options="$store.getters['staffingStore/getStaffTypeBasicTariffList']"
                           v-model="$store.getters['staffingStore/getData'].staffingTypeId"
                           required-star
                           :disabled="isDisabledTopHeader"
                        >
                        </form-select>
                     </b-col>
                  </b-row>
               </b-card>
            </validation-observer>
            <b-card class="staffing-tables-area">
               <b-row style="margin-bottom: 5px; overflow-x: scroll; min-height: 450px">
                  <table class="w-100 h-100">
                     <Table />
                     <Draggable />
                  </table>
               </b-row>
            </b-card>
            <b-row class="mb-3">
               <b-col sm="12" md="6" lg="6" class="text-left"></b-col>
               <b-col sm="12" md="6" lg="6" class="text-right">
                  <b-button v-if="!isView" :disabled="saveLoading" @click="saveData" variant="outline-success">
                     <feather-icon icon="CheckIcon" v-if="!saveLoading"></feather-icon>
                     <b-spinner v-else small></b-spinner>
                     {{ $t('Save') }}
                  </b-button>
               </b-col>
            </b-row>
         </b-col>
      </b-row>
   </b-overlay>
</template>

<script>
// components
import {
   BOverlay,
   BCard,
   BCardBody,
   BRow,
   BCol,
   BFormInput,
   BTabs,
   BTable,
   BTab,
   BButton,
   BLink,
   BFormGroup,
   VBTooltip,
   BModal,
   VBModal,
   BCardText,
   BInputGroup,
   BInputGroupAppend,
   BFormCheckbox,
   BFormTextarea,
   BTableSimple,
   BThead,
   BTbody,
   BTr,
   BTd,
   BSpinner
} from 'bootstrap-vue';
import Table from './components/Table.vue';
import Draggable from './components/Draggable.vue';

export default {
   components: {
      Draggable,
      Table,
      BFormInput,
      BOverlay,
      BCard,
      BCardBody,
      BRow,
      BCol,
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
      BFormCheckbox,
      BFormTextarea,
      BTableSimple,
      BThead,
      BTbody,
      BTr,
      BTd,
      BSpinner
   },
   name: 'Edit',
   data() {
      return {
         saveLoading: false
      };
   },
   created() {
      this.$store.dispatch('staffingStore/resetPage');
      if (this.$route.params.mode == 'clone') {
         this.$store.dispatch('staffingStore/getCloneData', this.$route.params.id);
      } else {
         this.$store.dispatch('staffingStore/getDataList', this.$route.params.id);
      }
      this.$store.dispatch('staffingStore/getOrgSettleMentAccountList');
      this.$store.dispatch('staffingStore/getStaffTypeBasicTariffList');
   },
   computed: {
      isDisabledTopHeader() {
         if (
            !this.$store.getters['staffingStore/getData'].positions ||
            this.$store.getters['staffingStore/getData'].positions.length === 0
         ) {
            return false;
         }
         return true;
      },
      isView() {
         return this.$route.params.isView;
      }
   },
   methods: {
      changePosition(id) {
         const obj = this.$store.getters['staffingStore/getPositionList'].find((el) => el.value === id);
         if (!obj) return;
         this.$store.commit('staffingStore/ChangePosition', obj);
      },
      saveData() {
         this.$refs.validateTo.validate().then((success) => {
            if (success) {
               if (this.$store.getters['staffingStore/getData'].length === 0) {
                  this.makeToast(this.$t('fillpositions'), 'success');

                  return null;
               }
               this.saveLoading = true;

               this.$store.dispatch('staffingStore/saveData', {
                  cb: () => {
                     this.makeToast(this.$t('SuccessSave'), 'success');

                     this.saveLoading = false;
                     this.$router.push({ name: 'Staffing' });
                  },
                  err: (error) => {
                     this.showApiError(error);
                     this.saveLoading = false;
                  }
               });
            }
         });
      }
   }
};
</script>

<style lang="scss">
@import '@/assets/form-date.scss';

.bg-white {
   background: #f7f8fc;
}
.staffing-tables-area {
   .StaffingMyTable {
      .center {
         text-align: center;
         vertical-align: middle;
      }
      tr.active {
         background: #a6fcce !important;
      }
      th,
      td {
         border: 1px solid #e6e8f4;
         border-collapse: collapse;
         font-size: 12px;
         padding: 3px 5px;
         font-weight: 400;

         .tr-container {
            display: inline-block;
            text-align: center;
            vertical-align: middle;
         }
      }
      th {
         min-width: 120px;
      }
      .w-200 {
         width: 200px !important;
      }
      .w-100 {
         width: 100px !important;
      }
      .w-75 {
         width: 75px !important;
      }
      .w-60 {
         width: 60px !important;
      }
      .w-50 {
         width: 50px !important;
      }
      .w-40 {
         width: 40px !important;
      }
      .w-90 {
         width: 90px !important;
      }
      .h-100 {
         height: 100px !important;
      }
      .h-120 {
         height: 120px !important;
      }
      .h-40 {
         height: 40px !important;
      }
      .no-wrap {
         white-space: nowrap;
      }
      .pointer {
         cursor: pointer;
      }
      .h-30 {
         height: 30px;
      }
      .icon-bg {
         background: #8effc3;
      }
      .grap {
         cursor: grab;
      }
      small {
         color: tomato;
         font-size: 12px;
         margin-left: 2px;
      }
   }

   .layout-container {
      position: static !important;
      background-color: inherit !important;
      width: 100%;
   }
   .staff-card {
      border: 1px solid #e6e8f4;
      background: white;
      border-radius: 10px;
   }
   .tableClassAddRow {
      th {
         padding: 6px;
         .form-group {
            margin-bottom: 0;
         }
         .vs__dropdown-toggle {
            width: 100%;
            font-size: 1rem;
            font-weight: 400;
            line-height: 1.45;
         }
      }
   }
}
</style>
