<template>
   <ValidationProvider :vid="vid" :name="$attrs.name" :rules="rules" v-slot="{ valid, required, errors }">
      <b-form-group v-bind="$attrs" :state="errors[0] ? false : valid ? true : null">
         <template v-slot:label> {{ $attrs.label }} <span class="text-danger" v-if="required">*</span> </template>
         <cleave
            v-model="innerValue"
            v-bind="$attrs"
            v-on="$listeners"
            :class="{ 'is-invalid': errors[0] ? true : valid ? false : null }"
            @keydown.native.,="keydown"
            class="form-control"
            :raw="true"
            :options="cleveOptions"
         />
         <b-form-invalid-feedback id="inputLiveFeedback" :state="errors[0] ? false : valid ? true : null">{{
            errors[0]
         }}</b-form-invalid-feedback>
      </b-form-group>
   </ValidationProvider>
</template>

<script>
import { ValidationProvider } from 'vee-validate';
import { BFormInput, BFormGroup, BFormInvalidFeedback } from 'bootstrap-vue';
import Cleave from 'vue-cleave-component';

export default {
   components: {
      ValidationProvider,
      BFormInput,
      BFormGroup,
      BFormInvalidFeedback,
      Cleave
   },
   props: {
      // eslint-disable-next-line vue/require-default-prop
      vid: {
         type: String
      },
      rules: {
         type: [Object, String],
         default: ''
      },
      // eslint-disable-next-line vue/require-default-prop
      value: {
         type: null
      }
   },
   data: () => ({
      innerValue: null,
      cleveOptions: {
         numeral: true,
         numeralDecimalScale: 2,
         delimiter: ' ',
         numeralDecimalMark: '.'
      }
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
   },
   methods: {
      keydown(e) {
         if (e.target.value) {
            e.target.value += this.cleveOptions.numeralDecimalMark;
         }
      }
   }
};
</script>

<style>
.col-form-label {
   font-size: 0.857rem !important;
}
</style>
