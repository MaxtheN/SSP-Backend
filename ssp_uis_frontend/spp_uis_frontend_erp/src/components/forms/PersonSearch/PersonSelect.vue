<template>
   <ValidationProvider ref="validator" :vid="vid" :name="$attrs.name" :rules="rules" v-slot="{ valid, errors }">
      <b-form-group id="exampleInputGroup3" v-bind="$attrs">
         <v-select v-model="innerValue" :reduce="(item) => item[valueid]" :clearable="false" :label="valuename" v-bind="$attrs" :state="errors[0] ? false : valid ? true : null">
            <slot></slot>
         </v-select>
         <b-form-invalid-feedback id="inputLiveFeedback">{{ errors[0] }}</b-form-invalid-feedback>
      </b-form-group>
   </ValidationProvider>
</template>

<script>
import { ValidationProvider } from 'vee-validate';
import { BFormSelect, BFormGroup, BFormInvalidFeedback } from 'bootstrap-vue';

export default {
   components: {
      ValidationProvider,
      BFormSelect,
      BFormGroup,
      BFormInvalidFeedback
   },
   props: {
      vid: {
         type: String
      },
      rules: {
         type: [Object, String],
         default: ''
      },
      // must be included in props
      value: {
         type: null
      },
      valuename: {
         type: String,
         default: 'text'
      },
      valueid: {
         type: String,
         default: 'value'
      }
   },
   data: () => ({
      innerValue: ''
   }),
   watch: {
      // Handles internal model changes.
      innerValue(newVal) {
         this.$emit('input', newVal);
      },
      // Handles external model changes.
      value(newVal) {
         this.innerValue = newVal;
      }
   },
   created() {
      if (this.value) {
         this.innerValue = this.value;
      }
   }
};
</script>
