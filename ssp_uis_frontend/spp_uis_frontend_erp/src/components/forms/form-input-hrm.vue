<template>
   <ValidationProvider
      :vid="vid"
      :name="$attrs.name"
      :rules="rules"
      :mode="$attrs.mode"
      v-slot="{ valid, errors, required }"
   >
      <b-form-group v-bind="$attrs">
         <template v-slot:label>
            {{ $attrs.label }}
            <span class="text-danger" v-if="required">*</span>
         </template>
         <b-form-input
            v-model="innerValue"
            v-bind="$attrs"
            v-mask="mask"
            :state="errors[0] ? false : valid ? true : null"
         >
            <slot></slot>
         </b-form-input>
         <b-form-invalid-feedback id="inputLiveFeedback">{{
            errors[0]
         }}</b-form-invalid-feedback>
      </b-form-group>
   </ValidationProvider>
</template>

<script>
import { ValidationProvider } from 'vee-validate';
import { BFormInput, BFormGroup, BFormInvalidFeedback } from 'bootstrap-vue';

export default {
   components: {
      ValidationProvider,
      BFormInput,
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
      value: [String, Number, null],
      mask: {
         type: String,
         default: ''
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
<style>
.col-form-label {
   font-size: 0.857rem !important;
}
.was-validated .form-control:valid,
.form-control.is-valid {
   background-image: none !important;
   border-color: #A7A8AB !important;
}
.was-validated .form-control:valid:focus,
.form-control.is-valid:focus {
   border-color: var(--primary) !important;
}
.dark-layout .was-validated .form-control:valid,
.dark-layout .form-control.is-valid {
   border-color: #A7A8AB !important;
}
</style>
