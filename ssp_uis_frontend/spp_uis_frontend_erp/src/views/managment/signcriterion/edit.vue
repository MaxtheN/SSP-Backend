<template>
   <b-overlay :show="show">
      <b-card>
         <validation-observer ref="ValidationDTO">
            <b-row class="align-items-center">
               <b-col sm="12" md="3">
                  <form-picker v-model="Data.dateOn" :label="$t('ondate')" :placeholder="$t('ondate')" />
               </b-col>

               <b-col sm="12" md="3">
                  <form-select
                     :options="OrganizationGroupSelectList"
                     v-model="Data.organizationGroupId"
                     required-star
                     :label="$t('group')"
                  ></form-select>
               </b-col>

               <b-col sm="12" md="3">
                  <form-select
                     :options="ContractorCategoryList"
                     v-model="Data.contractorCategoryId"
                     required-star
                     :label="$t('contractorCategory')"
                  ></form-select>
               </b-col>

               <b-col sm="12" md="3">
                  <form-select
                     :options="PositionList"
                     v-model="Data.positionId"
                     required-star
                     :label="$t('position')"
                  ></form-select>
               </b-col>

               <b-col sm="12" md="3">
                  <form-select
                     :options="ApplicationTypeIdConst"
                     v-model="Data.applicationTypeId"
                     required-star
                     :label="$t('applicationType')"
                  ></form-select>
               </b-col>

               <b-col sm="12" md="3">
                  <form-select :options="StateList" v-model="Data.stateId" label="Status"></form-select>
               </b-col>

               <!-- save button -->
               <b-col cols="12" class="text-right">
                  <b-button :disabled="saveLoading" @click="SaveData" variant="outline-success">
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
import SignCriterionService from '@/services/managment/signcriterion.service';
// components
import { BOverlay, BCard, BRow, BCol, BButton, BFormCheckbox } from 'bootstrap-vue';
import FormInputTranslate from '@/components/translates/FormInputTranslate.vue';
import ManualService from '@/services/others/manual.service';
import PositionService from '@/services/info/position.service';
import ApplicationMixin from '@/mixins/application';

export default {
   components: {
      BOverlay,
      BCard,
      BRow,
      BCol,
      BButton,
      BFormCheckbox,
      FormInputTranslate
   },
   mixins: [ApplicationMixin],
   data() {
      return {
         show: false,
         saveLoading: false,
         StateList: [],
         OrganizationGroupSelectList: [],
         ContractorCategoryList: [],
         PositionList: [],
         Data: {
            dateOn: '',
            positionId: null,
            contractorCategoryId: null,
            organizationGroupId: null,
            applicationTypeId: null,
            id: 0
         }
      };
   },
   created() {
      this.GetSignCriterion();

      ManualService.StateSelectList().then((res) => {
         this.StateList = res.data;
      });

      ManualService.OrganizationGroupSelectList()
         .then((res) => {
            this.OrganizationGroupSelectList = res.data;
         })
         .catch(this.showApiError);

      ManualService.ContractorCategorySelectList().then((res) => {
         this.ContractorCategoryList = res.data;
      });

      PositionService.GetAsSelectList()
         .then((res) => {
            this.PositionList = res.data;
         })
         .catch((error) => {
            this.showApiError(error);
         });
   },
   methods: {
      GetSignCriterion() {
         SignCriterionService.Get(this.$route.params.id)
            .then((res) => {
               this.Data = res.data;
            })
            .catch((error) => {
               this.showApiError(error);
            })
            .finally(() => {
               this.show = false;
            });
      },
      SaveData() {
         this.$refs.ValidationDTO.validate().then((success) => {
            if (success) {
               this.saveLoading = true;
               SignCriterionService.Update(this.Data)
                  .then(() => {
                     this.makeToast(this.$t('SaveSuccess'), 'success');
                     this.$router.push({ name: 'SignCriterion' });
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
