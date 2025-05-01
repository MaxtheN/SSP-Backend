<template>
   <b-overlay :show="show">
      <b-row>
         <b-col sm="12" md="12" lg="12">
            <b-card>
               <validation-observer ref="ValidationDTO">
                  <b-row>
                     <b-col sm="12" md="4" class="mb-1">
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
                     <b-col sm="12" md="4" class="mb-1">
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
                     <b-col sm="12" md="4" align-self="center">
                        <b-form-checkbox v-model="Data.isGroup">
                           {{ $t('isGroup') }}
                        </b-form-checkbox>
                     </b-col>
                     <b-col sm="12" md="4" class="mb-1">
                        <form-input-hrm v-model="Data.numberOfGroup" name="numberOfGroup" rules="required|integer" :label="$t('numberOfGroup')" />
                     </b-col>
                     <b-col sm="12" md="2" class="mb-1">
                        <form-input-hrm v-model="Data.code1" name="code1" rules="required|max:2" :label="$t('code1')" />
                     </b-col>
                     <b-col sm="12" md="2" class="mb-1">
                        <form-input-hrm v-model="Data.code2" name="code2" rules="required|max:3" :label="$t('code2')" />
                     </b-col>
                     <b-col sm="12" md="2" class="mb-1">
                        <form-input-hrm v-model="Data.code3" name="code3" rules="required|max:3" :label="$t('code3')" />
                     </b-col>
                     <b-col sm="12" md="2" class="mb-1">
                        <form-input-hrm disabled v-model="Data.code" name="code" rules="required|max:7" :label="$t('kode')" />
                     </b-col>
                     <b-col sm="12" md="3" class="mb-1">
                        <form-select v-model="Data.parentId" :options="ItemOfExpenseList" label="parent1" />
                     </b-col>
                     <b-col sm="12" md="3" align-self="center">
                        <b-form-checkbox v-model="Data.ageingAllowed">
                           {{ $t('ageingAllowed') }}
                        </b-form-checkbox>
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
         </b-col>
      </b-row>
   </b-overlay>
</template>
<script>
// service
import ItemOfExpenseService from '@/services/hrm/itemofexpense.service';
// components
import { BOverlay, BCard, BRow, BCol, BFormInput, BTable, BButton, BLink, BFormGroup, BModal, BInputGroup, BInputGroupAppend, BFormCheckbox, BFormTextarea } from 'bootstrap-vue';
import FormInputTranslate from '@/components/translates/FormInputTranslate.vue';
import FormInputHrm from '@/components/forms/form-input-hrm.vue';
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
      FormInputTranslate,
      FormInputHrm
   },
   name: 'Edit',
   data() {
      return {
         show: false,
         ItemOfExpenseList: [],
         TabrowModal: false,
         saveLoading: false,
         Data: {
            numberOfGroup: 0,
            shortName: '',
            fullName: '',
            isGroup: false,
            code: '',
            code1: '',
            code2: '',
            code3: '',
            parentId: 0,
            ageingAllowed: false,
            translates: []
         }
      };
   },
   created() {
      this.show = true;
      ItemOfExpenseService.Get(this.$route.params.id)
         .then((res) => {
            this.Data = res.data;
         })
         .catch((error) => {
            this.makeToast(error, 'danger');
         })
         .finally(() => {
            this.show = false;
         });

      ItemOfExpenseService.GetAsSelectList()
         .then((res) => {
            this.ItemOfExpenseList = res.data;
         })
         .catch((error) => {
            this.makeToast(error, 'danger');
         });
   },
   computed: {
      computedCode() {
         return [this.Data.code1, this.Data.code2, this.Data.code3].join('');
      }
   },
   watch: {
      computedCode(newCode) {
         this.Data.code = newCode;
      }
   },
   methods: {
      SaveData() {
         this.$refs.ValidationDTO.validate().then((success) => {
            if (success) {
               this.saveLoading = true;
               ItemOfExpenseService.Update(this.Data)
                  .then((res) => {
                     this.makeToast(this.$t('SaveSuccess'), 'success');
                     this.$router.push({ name: 'ItemOfExpense' });
                  })
                  .catch((err) => {
                     this.makeToast(this.$t(err), 'danger');
                     // this.$refs.ValidationDTO.setErrors(err.response.data.errors);
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
