<template>
   <div>
      <validation-provider
         #default="validationContext"
         :name="label"
         :rules="requiredStar ? 'required' : null"
         :vid="vid"
      >
         <b-form-group :state="getValidationState(validationContext)">
            <label for v-if="!hideStar">
               {{ $t(`${label}`) }}
               <span v-if="requiredStar && !hideStar" style="color: red">*</span>
            </label>
            <div>
               <v-select
                  v-model="inputValue"
                  :reduce="(item) => item.id"
                  :options="options"
                  :get-option-label="getOptionLabel"
                  :placeholder="$t(`${placeholder}`)"
                  :clearable="clearable"
                  :disabled="disabled"
                  :loading="loading"
                  @input="ChangeItem"
               >
                  <template v-slot:option="option">
                     {{ option.names[langId] }}
                  </template>
                  <template v-slot:selected-option="option">
                     {{ option.names[langId] }}
                  </template>
               </v-select>
            </div>

            <b-form-invalid-feedback :state="getValidationState(validationContext)">{{
               $t('fieldNotEmpty')
            }}</b-form-invalid-feedback>
         </b-form-group>
      </validation-provider>
   </div>
</template>
<script>
import { ValidationProvider } from 'vee-validate';
import { BFormGroup, BFormInvalidFeedback } from 'bootstrap-vue';
import { ref } from 'vue';
import { required } from '@validations';
import formValidation from '@core/comp-functions/forms/form-validation';
export default {
   components: {
      ValidationProvider,
      BFormGroup,
      BFormInvalidFeedback
   },
   props: {
      langId: {
         type: String,
         default: null
      },
      options: {
         type: Array,
         default: null
      },
      requiredStar: {
         type: Boolean,
         default: false
      },
      disabled: {
         type: Boolean,
         default: false
      },
      valuename: {
         type: String,
         default: 'text'
      },
      valueid: {
         type: String,
         default: 'value'
      },
      clearable: {
         type: Boolean,
         default: true
      },
      disabledOption: {
         type: String,
         default: 'isDisabled'
      },
      placeholder: {
         type: String,
         default: 'ChooseBelow'
      },
      value: {},

      label: {
         type: String,
         default: ''
      },
      hideStar: {
         type: Boolean,
         default: false
      },
      loading: {
         type: Boolean,
         default: false
      },
      vid: {
         type: [String, Number],
         default: null
      }
   },
   data() {
      return {
         required
      };
   },
   setup(props, { emit }) {
      const clearFormData = ref(null);

      const { refFormObserver, getValidationState, resetForm, clearForm } = formValidation(props.clearEventData);

      clearFormData.value = clearForm;

      return {
         // Ad

         // Form Validation
         resetForm,
         refFormObserver,
         getValidationState
      };
   },

   created() {},
   computed: {
      inputValue: {
         get() {
            // Boshli yoki mavjud bo'lmagan qiymatlar uchun default qiymatni qaytaramiz
            return this.value === null || this.value === undefined ? 0 : this.value;
         },
         set(val) {
            if (val === null || val === 0) {
               this.$emit('input', null);
            } else {
               this.$emit('input', val);
            }
         }
      }
   },
   methods: {
      getOptionLabel(option) {
         return option.names[this.langId] || option.names['uz']; // langId ga qarab label tanlaymiz
      },
      ChangeItem(val) {
         this.$emit('change', val);
      },
      handleSearch(searchTerm) {
         if (searchTerm && searchTerm.length > 2) {
            this.$emit('search', searchTerm);
         }
      }
   }
};
</script>
