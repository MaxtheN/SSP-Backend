<template>
   <div>
      <validation-provider
         #default="validationContext"
         :name="label"
         :rules="requiredStar ? 'required' : null"
         :vid="vid"
      >
         <b-form-group :state="getValidationState(validationContext)">
            <!-- {{ getValidationState(validationContext)}} -->
            <label for v-if="!hideStar">
               {{ $t(`${label}`) }}
               <span v-if="requiredStar && !hideStar" style="color: red">*</span>
            </label>
            <div>
               <v-select
                  v-model="inputValue"
                  :options="options"
                  :reduce="(item) => item[valueid]"
                  :label="valuename"
                  :placeholder="$t(`${placeholder}`)"
                  :clearable="clearable"
                  :disabled="disabled"
                  :multiple="multiple"
                  :loading="loading"
                  :selectable="selectable"
                  @input="ChangeItem"
                  @search="handleSearch"
                  @option:selected="(e) => $emit('option:selected', e)"
               >
                  <template v-for="(_, name) in $scopedSlots" :slot="name" slot-scope="slotData">
                     <slot :name="name" v-bind="slotData"> </slot>
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
      multiple: {
         type: Boolean,
         default: false
      },
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
      },
      selectable: {
         type: [Function, Boolean, undefined],
         default: () => true
      }
   },
   data() {
      return {
         required
      };
   },
   setup(props, { emit }) {
      /*
     ? This is handled quite differently in SFC due to deadlock of `useFormValidation` and this composition function.
     ? If we don't handle it the way it is being handled then either of two composition function used by this SFC get undefined as one of it's argument.
     * The Trick:

     * We created reactive property `clearFormData` and set to null so we can get `resetEventLocal` from `useCalendarEventHandler` composition function.
     * Once we get `resetEventLocal` function which is required by `useFormValidation` we will pass it to `useFormValidation` and in return we will get `clearForm` function which shall be original value of `clearFormData`.
     * Later we just assign `clearForm` to `clearFormData` and can resolve the deadlock. 😎

     ? Behind The Scene
     ? When we passed it to `useCalendarEventHandler` for first time it will be null but right after it we are getting correct value (which is `clearForm`) and assigning that correct value.
     ? As `clearFormData` is reactive it is being changed from `null` to corrent value and thanks to reactivity it is also update in `useCalendarEventHandler` composition function and it is getting correct value in second time and can work w/o any issues.
    */
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
            return this.value === 0 ? null : this.value;
         },
         set(val) {
            if (val === 0) {
               this.$emit('input', null);
            } else {
               this.$emit('input', val);
            }
         }
      }
   },
   methods: {
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
