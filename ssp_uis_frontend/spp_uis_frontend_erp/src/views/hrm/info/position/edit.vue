<template>
   <b-overlay :show="show">
      <validation-observer ref="ValidationDTO">
         <b-card>
            <b-row>
               <b-col sm="12" md="4">
                  <form-input-hrm
                     v-model="Data.orderCode"
                     type="number"
                     :placeholder="$t('code')"
                     :label="$t('code')"
                     rules="required|numeric"
                  />
               </b-col>
               <b-col sm="12" md="4">
                  <form-input-hrm v-model="Data.indexCode" :placeholder="$t('indexCode')" :label="$t('indexCode')" />
               </b-col>

               <b-col sm="12" md="4">
                  <form-input-translate
                     v-model="Data.fullName"
                     @update:translates="(e) => (Data.translates = e)"
                     :translates="Data.translates"
                     column-name="full_name"
                     required
                     :label="$t('fullname')"
                     @input="(v) => (Data.shortName = v)"
                     :placeholder="$t('fullname')"
                  />
               </b-col>

               <b-col sm="12" md="4">
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

               <b-col sm="12" md="4">
                  <form-select
                     :options="TariffScaleTypeList"
                     v-model="Data.tariffScaleTypeId"
                     :label="$t('tariffScaleType')"
                  />
               </b-col>
               <b-col sm="12" md="4">
                  <form-select
                     :options="PositionCategoryList"
                     v-model="Data.positionCategoryId"
                     :label="$t('PositionCategory')"
                     :placeholder="$t('PositionType')"
                  />
               </b-col>
               <b-col sm="12" md="4">
                  <form-select
                     :options="PositionClassificationList"
                     v-model="Data.positionClassificationId"
                     :label="$t('PositionClassification')"
                  />
               </b-col>

               <!-- <b-col sm="12" md="4">
                  <form-select
                     :options="StaffTypeBasicTariff"
                     v-model="Data.staffTypeBasicTariffId"
                     :label="$t('staffTypeBasicTariff')"
                  />
               </b-col> -->
            </b-row>
            <b-row class="mt-1 justify-content-end">
               <b-col sm="12" md="6" lg="6" class="text-right">
                  <b-button @click="SaveData" size="sm" variant="outline-success">
                     <feather-icon icon="CheckIcon"></feather-icon>
                     {{ $t('Save') }}
                  </b-button>
               </b-col>
            </b-row>
         </b-card>
      </validation-observer>
   </b-overlay>
</template>
<script>
import ManualService from '@/services/others/manual.service';
import PositionService from '@/services/info/position.service';
import PositionClassificationService from '@/services/hrm/positionclassification.service';
// import StaffTypeBasicTariffService from '@/services/hrm/stafftypebasictariff.service';
import PositionCategoryService from '@/services/hrm/positioncategory.service';

import { BOverlay, BCard, BRow, BCol, BButton, BTable } from 'bootstrap-vue';
import FormInputTranslate from '@/components/translates/FormInputTranslate.vue';

export default {
   components: {
      BOverlay,
      BCard,
      BRow,
      BCol,
      BButton,
      BTable,
      FormInputTranslate
   },
   name: 'PositionEdit',
   data() {
      return {
         show: true,
         loadingButton: false,
         PositionCategoryList: [],
         TariffScaleTypeList: [],
         PositionClassificationList: [],
         // StaffTypeBasicTariff: [],
         Data: {
            id: 0,
            tariffScaleTypeId: 0,
            shortName: '',
            fullName: '',
            positionClassificationId: 0,
            positionCategoryId: 0,
            staffTypeBasicTariffId: 0,
            translates: []
         }
      };
   },
   created() {
      this.show = true;
      PositionService.Get(this.$route.params.id)
         .then((res) => {
            this.Data = res.data;
         })
         .finally(() => {
            this.show = false;
         });

      ManualService.TariffScaleTypeSelectList({}).then((res) => {
         if (Array.isArray(res.data)) {
            this.TariffScaleTypeList = res.data;
         }
      });

      // StaffTypeBasicTariffService.GetAsSelectList().then((res) => {
      //    this.StaffTypeBasicTariff = res.data;
      // });

      PositionClassificationService.GetAsSelectList().then((res) => {
         this.PositionClassificationList = res.data;
      });
      PositionCategoryService.GetAsSelectList().then((res) => {
         if (Array.isArray(res.data)) {
            this.PositionCategoryList = res.data;
         }
      });
   },
   methods: {
      SaveData() {
         this.$refs.ValidationDTO.validate().then((success) => {
            if (success) {
               this.loadingButton = true;
               PositionService.Update(this.Data)
                  .then((res) => {
                     this.makeToast(this.$t('SaveSuccess'), 'success');
                     this.$router.push({ name: 'Position' });
                  })
                  .catch((err) => {
                     this.showApiError(err);
                  })
                  .finally(() => {
                     this.loadingButton = false;
                  });
            }
         });
      }
   }
};
</script>
<style scoped>
legend {
   background-color: #000;
   color: #fff;
   padding: 3px 6px;
}

.output {
   font: 1rem 'Fira Sans', sans-serif;
}

input {
   margin: 0.4rem;
}
</style>
