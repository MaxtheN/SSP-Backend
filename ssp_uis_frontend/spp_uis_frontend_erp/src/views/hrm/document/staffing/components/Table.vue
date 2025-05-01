<template>
   <validation-observer ref="addPositionRef" class="StaffingMyTable w-100" tag="thead">
      <tr class="text-center">
         <th class="center" style="width: 100px">{{ $t('Add') }}</th>
         <th class="center">{{ $t('departmentName') }}<small>*</small></th>
         <th class="center">{{ $t('positionClassification') }}<small>*</small></th>
         <th class="center">{{ $t('quantity') }} ({{ allCounts }})<small>*</small></th>
         <th class="center">{{ $t('rankid') }}<small>*</small></th>
         <th class="center">
            {{ $t('rankCoef') }}
         </th>
         <th class="center">
            {{ $t('corrCoef') }}
         </th>
         <th class="center">
            {{ $t('salary') }}
         </th>
         <th class="center">
            {{ $t('fot') }}
         </th>
         <th class="center">{{ $t('Add') }}</th>
      </tr>
      <tr class="tableClassAddRow">
         <th class="center">
            <b-button
               class="w-60"
               :disabled="isDisabledTable || $store.getters['staffingStore/getbtnDisabled1']"
               variant="primary"
               @click="AddPositions"
               v-if="!isView"
            >
               <b-spinner small v-if="$store.getters['staffingStore/getbtnDisabled1']" />
               <feather-icon v-else icon="PlusIcon"></feather-icon>
            </b-button>
         </th>

         <th style="min-width: 200px">
            <form-select
               v-model="$store.getters['staffingStore/getPositions'].departmentId"
               :options="$store.getters['staffingStore/getDepartMentList']"
               required-star
               hide-star
               :disabled="isDisabledTable"
            />
         </th>
         <th style="min-width: 200px">
            <form-select
               v-model="$store.getters['staffingStore/getPositions'].positionId"
               :options="$store.getters['staffingStore/getPositionList']"
               @change="ChangePosition"
               required-star
               hide-star
               :disabled="isDisabledTable"
            />
         </th>

         <th style="max-width: 60px">
            <b-form-input
               :value="$store.getters['staffingStore/getPositions'].quantity"
               type="number"
               required
               hide-star
               debounce="500"
               @update="(val) => $store.dispatch('staffingStore/ChangePositionQuantity', val)"
               :disabled="isDisabledTable"
            />
         </th>

         <th style="min-width: 200px">
            <form-select
               v-model="$store.getters['staffingStore/getPositions'].rankId"
               :options="$store.getters['staffingStore/getRankList']"
               @change="(val) => $store.dispatch('staffingStore/ChangeRank', val)"
               required-star
               hide-star
               :disabled="isDisabledTable"
            />
         </th>
         <th class="text-center">
            <b-form-input
               required
               disabled
               :value="$store.getters['staffingStore/getPositions'].rankCoef"
            ></b-form-input>
         </th>
         <th class="text-center">
            <b-form-input
               required
               v-model="$store.getters['staffingStore/getPositions'].corrCoef"
               debounce="500"
               @update="() => $store.dispatch('staffingStore/RecalcStaffingCalcKindTables')"
            ></b-form-input>
         </th>

         <th class="text-right" style="min-width: 200px">
            <b-form-input
               v-if="!$store.getters['staffingStore/getIsBaseSalary']"
               v-model="$store.getters['staffingStore/getPositions'].salary"
               type="number"
               disabled
            />
            <small v-else style="white-space: nowrap; width: auto"
               >{{
                  $options.filters.currency($store.getters['staffingStore/getPositions'].salary, {
                     symbol: '',
                     fractionCount: 2
                  })
               }}
            </small>
         </th>

         <th style="min-width: 200px" class="text-right">
            {{
               $options.filters.currency($store.getters['staffingStore/getPositions'].totalSum, {
                  symbol: '',
                  fractionCount: 2
               })
            }}
         </th>

         <th class="center">
            <b-button
               :disabled="isDisabledTable || $store.getters['staffingStore/getbtnDisabled1']"
               variant="primary"
               @click="AddPositions"
               v-if="!isView"
            >
               <b-spinner small v-if="$store.getters['staffingStore/getbtnDisabled1']" />
               <feather-icon v-else style="width: 13px" icon="PlusIcon"></feather-icon>
            </b-button>
         </th>
      </tr>
   </validation-observer>
</template>

<script>
import draggable from 'vuedraggable';
import StaffingMixins from '../mixins/staffing';

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

export default {
   components: {
      draggable,
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
   mixins: [StaffingMixins],
   computed: {
      isDisabledTable() {
         if (
            this.$store.getters['staffingStore/getData'].startOn &&
            this.$store.getters['staffingStore/getData'].financeYear &&
            this.$store.getters['staffingStore/getData'].staffingTypeId
         ) {
            return false;
         }
         return true;
      },
      isView() {
         return this.$route.params.isView;
      },
      allCounts() {
         return this.$store.getters['staffingStore/getData'].positions?.reduce((a, b) => a + Number(b.quantity), 0);
      }
   },
   mounted() {
      this.$store.dispatch('staffingStore/getDepartMentList');
      this.$store.dispatch('staffingStore/getPositionList');
      this.$store.dispatch(
         'staffingStore/getTableAsSelectList',
         this.$store.getters['staffingStore/getPositions'].tariffScaleId
      );
   },
   methods: {
      ChangePosition(id) {
         const obj = this.$store.getters['staffingStore/getPositionList'].find((el) => el.value === id);

         if (!obj) return;

         this.$store.dispatch('staffingStore/ChangePositions', obj);
      },
      AddPositions() {
         this.$refs.addPositionRef.validate().then((success) => {
            if (success) {
               this.$store.dispatch('staffingStore/AddPositions').then(() => {
                  this.$refs.addPositionRef.reset();
                  this.$store.commit('staffingStore/setEditedIndex2', -1);
               });
            }
         });
      }
   }
};
</script>
