<template>
   <b-overlay :show="show">
      <b-row>
         <b-col sm="12" md="12" lg="12">
            <b-card>
               <validation-observer ref="ValidationDTO">
                  <b-row>
                     <b-col sm="12" md="4" class="mb-1">
                        <form-input
                           v-model="Data.orderCode"
                           :label="$t('orderCode')"
                        />
                     </b-col>
                     <b-col sm="12" md="4" class="mb-1">
                        <form-input
                           v-model="Data.code"
                           required
                           :label="$t('code')"
                        />
                     </b-col>
                     <b-col sm="12" md="4" class="mb-1">
                        <form-input-translate
                           v-model="Data.shortName"
                           @update:translates="(e) => (Data.translates = e)"
                           :translates="Data.translates"
                           columnName="short_name"
                           required
                           :label="$t('shortname')"
                           :placeholder="$t('shortname')"
                        />
                     </b-col>
                     <b-col sm="12" md="4" class="mb-1">
                        <form-input-translate
                           v-model="Data.fullName"
                           @update:translates="(e) => (Data.translates = e)"
                           :translates="Data.translates"
                           columnName="full_name"
                           required
                           :label="$t('fullname')"
                           :placeholder="$t('fullname')"
                        />
                     </b-col>
                     <b-col sm="12" md="4">
                        <form-input
                           v-model="Data.firstSignPosition"
                           required
                           :label="$t('firstSignPosition')"
                        ></form-input>
                     </b-col>
                     <b-col sm="12" md="4">
                        <form-input
                           v-model="Data.secondSignPosition"
                           required
                           :label="$t('secondSignPosition')"
                        ></form-input>
                     </b-col>
                     <b-col sm="12" md="4">
                        <form-input
                           v-model="Data.firstSignOrganization"
                           required
                           :label="$t('firstSignOrganization')"
                        ></form-input>
                     </b-col>
                     <b-col sm="12" md="4">
                        <form-input
                           v-model="Data.secondSignOrganization"
                           required
                           :label="$t('secondSignOrganization')"
                        ></form-input>
                     </b-col>
                     <b-col sm="12" md="4" class="mb-1">
                        <form-select
                           :options="StateList"
                           v-model="Data.stateId"
                           label="Status"
                        ></form-select>
                     </b-col>
                  </b-row>
                  <b-row>
                     <b-col sm="12" md="6" lg="6" class="text-left"></b-col>
                     <b-col sm="12" md="6" lg="6" class="text-right">
                        <b-button
                           :disabled="saveLoading"
                           @click="SaveData"
                           size="sm"
                           variant="outline-success"
                        >
                           <feather-icon icon="CheckIcon"></feather-icon>
                           {{ $t('Save') }}
                        </b-button>
                     </b-col>
                  </b-row>
               </validation-observer>
            </b-card>
         </b-col>
      </b-row>
   </b-overlay>
</template>
<script>
// service
import ManualService from '@/services/others/manual.service';
// components
import {
   BOverlay,
   BCard,
   BRow,
   BCol,
   BFormInput,
   BTable,
   BButton,
   BLink,
   BFormGroup,
   BModal,
   BInputGroup,
   BInputGroupAppend,
   BFormCheckbox,
   BFormTextarea
} from 'bootstrap-vue';
import FormInputTranslate from '@/components/translates/FormInputTranslate.vue';
import LevelCodeService from '@/services/hrm/levelcode.service';
export default {
   components: {
      BOverlay,
      BCard,
      BRow,
      BCol,
      BFormInput,
      BButton,
      BTable,
      BLink,
      BFormGroup,
      BModal,
      BInputGroup,
      BInputGroupAppend,
      BFormCheckbox,
      BFormTextarea,
      FormInputTranslate
   },
   name: 'Edit',
   data() {
      return {
         show: false,
         MinimumValueTypeList: [],
         StateList: [],
         TabrowModal: false,
         saveLoading: false,
         Data: {
            orderCode: '',
            code: '',
            shortName: '',
            fullName: '',
            firstSignPosition: '',
            secondSignPosition: '',
            firstSignOrganization: '',
            secondSignOrganization: '',
            translates: []
         }
      };
   },
   created() {
      this.show = true;
      LevelCodeService.Get(this.$route.params.id)
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

      ManualService.StateSelectList()
         .then((res) => {
            this.StateList = res.data;
         })
         .catch((error) => {
            this.showApiError(error);
         });
   },
   methods: {
      SaveData() {
         this.$refs.ValidationDTO.validate().then((success) => {
            if (success) {
               this.saveLoading = true;
               LevelCodeService.Update(this.Data)
                  .then((res) => {
                     this.makeToast(this.$t('SaveSuccess'), 'success');
                     this.$router.push({ name: 'LevelCode' });
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
