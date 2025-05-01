<template>
   <validation-provider #default="{ errors }" :name="label" :rules="required ? 'required' : null">
      <label for v-show="label">
         {{ label }}
         <span v-show="required" style="color: red">*</span>
      </label>
      <b-input-group>
         <b-form-textarea
            v-model="model"
            :state="required && errors.length > 0 ? false : null"
            :name="label"
            :type="type"
            v-mask="mask"
            :placeholder="placeholder ? placeholder : label"
            :disabled="disabled"
         />
         <slot></slot>
      </b-input-group>
      <small class="text-danger" v-show="required && errors.length > 0">
         {{ $t(`fieldNotEmpty`) }}
      </small>
   </validation-provider>
</template>

<script>
/* eslint-disable global-require */
import { ValidationProvider } from 'vee-validate';
import { BFormInput, BInputGroup, BFormTextarea } from 'bootstrap-vue';
export default {
   components: {
      BFormTextarea,
      BFormInput,
      BInputGroup,
      ValidationProvider
   },
   name: 'FormInput',
   data() {
      return {
         //   required,
         vData: ''
      };
   },
   computed: {
      model: {
         get() {
            return this.value == 0 ? null : this.value;
         },
         set(value) {
            if (value == 0) {
               this.$emit('input', null);
            } else {
               this.$emit('input', value);
            }
         }
      }
   },
   props: {
      placeholder: {
         type: String,
         default: ''
      },
      type: {
         type: String,
         default: 'text'
      },
      mask: {
         type: String,
         default: ''
      },
      required: {
         type: Boolean,
         default: false
      },
      label: {
         type: String,
         default: ''
      },
      value: {
         required: true
      },
      disabled: {
         type: Boolean,
         default: false
      }
   },

   methods: {}
};
</script>

<style lang="scss">
@import '@core/scss/vue/pages/page-auth.scss';
.language-login {
   position: absolute;
   z-index: 10000;
   right: 4rem;
   li {
      top: 2rem;
      list-style: none;
   }
}
</style>
