<template>
   <b-overlay :show="show">
      <b-card>
         <validation-observer ref="ValidationDTO">
            <b-row>
               <b-col sm="12" md="3">
                  <form-picker v-model="Data.dateOn" required :label="$t('ondate')" />
               </b-col>
               <b-col sm="12" md="3">
                  <form-currency-input v-model="Data.fixedValue" rules="required" :label="$t('fixedValue')" :placeholder="$t('fixedValue')" />
               </b-col>
               <b-col sm="12" md="3">
                  <form-input-hrm v-model="Data.changePercentage" rules="required|numeric" :label="$t('changePercentage')" :placeholder="$t('changePercentage')" />
               </b-col>
               <b-col sm="12" md="3">
                  <form-input-hrm v-model="Data.normativeDoc" rules="required" :label="$t('normativedoc')" :placeholder="$t('normativedoc')" />
               </b-col>
               <b-col sm="12" md="3">
                  <form-select :options="MinimumValueTypeList" v-model="Data.minimumValueTypeId" requitred-star :label="$t('minimumValueType')"></form-select>
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
         </validation-observer>
      </b-card>
   </b-overlay>
</template>
<script>
// service
import ManualService from '@/services/others/manual.service';
import FixedMinimumValueService from '@/services/hrm/fixedminimumvalue.service';
// components
import { BOverlay, BCard, BRow, BCol, BTable, BButton, BLink } from 'bootstrap-vue';

export default {
   components: {
      BOverlay,
      BCard,
      BRow,
      BCol,
      BButton,
      BTable,
      BLink
   },
   name: 'FixedMinimumValueEdit',
   data() {
      return {
         show: false,
         MinimumValueTypeList: [],
         saveLoading: false,
         Data: {
            id: 0,
            normativeDoc: null,
            dateOn: null,
            minimumValueTypeId: null,
            fixedValue: 0,
            changePercentage: 0
         }
      };
   },
   created() {
      this.show = true;
      FixedMinimumValueService.Get(this.$route.params.id)
         .then((res) => {
            this.Data = res.data;
         })
         .catch((error) => {
            this.makeToast(error.response.data.errors, 'danger');
         })
         .finally(() => {
            this.show = false;
         });

      ManualService.MinimumValueTypeSelectList()
         .then((res) => {
            this.MinimumValueTypeList = res.data;
         })
         .catch((error) => {
            this.makeToast(error.response.data.errors, 'danger');
         });
   },
   methods: {
      SaveData() {
         this.$refs.ValidationDTO.validate().then((success) => {
            if (success) {
               this.saveLoading = true;
               FixedMinimumValueService.Update(this.Data)
                  .then((res) => {
                     this.makeToast(this.$t('SaveSuccess'), 'success');
                     this.$router.push({ name: 'FixedMinimumValue' });
                  })
                  .catch((err) => {
                     if (err?.response?.data?.errors) {
                        const errors = err.response.data.errors;
                        Object.keys(errors).forEach((key) => {
                           this.makeToast(key + ' : ' + errors[key], 'danger');
                        });
                     } else {
                        this.makeToast(this.$t(err), 'danger');
                     }
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
<style scoped>
input {
   margin: 0.4rem;
}
</style>
