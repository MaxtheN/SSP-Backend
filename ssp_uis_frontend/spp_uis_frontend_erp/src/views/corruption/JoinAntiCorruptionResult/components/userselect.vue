<template>
   <div>
      <validation-provider
         #default="validationContext"
         :name="label"
         :rules="requiredStar ? 'required' : null"
         :vid="vid"
      >
         <b-form-group :state="getValidationState(validationContext)">
            <label v-if="!hideStar">
               {{ $t(`${label}`) }}
               <span v-if="requiredStar && !hideStar" style="color: red">*</span>
            </label>
            <div>
               <v-select
                  :options="dynamicOptions"
                  :label="valuename"
                  :placeholder="$t(`${placeholder}`)"
                  :clearable="clearable"
                  :reduce="(item) => item['value']"
                  :disabled="disabled"
                  :loading="isLoading"
                  v-bind="$attrs"
                  v-on="$listeners"
               ></v-select>
            </div>
            <b-form-invalid-feedback :state="getValidationState(validationContext)">
               {{ $t('fieldNotEmpty') }}
            </b-form-invalid-feedback>
         </b-form-group>
      </validation-provider>
   </div>
</template>

<script>
import { ValidationProvider } from 'vee-validate';
import { BFormGroup, BFormInvalidFeedback } from 'bootstrap-vue';
import { watch, ref } from 'vue';
import UserService from '@/services/managment/user.service';
import formValidation from '@core/comp-functions/forms/form-validation';

export default {
   components: {
      ValidationProvider,
      BFormGroup,
      BFormInvalidFeedback
   },
   props: {
      organizationId: {
         type: Number,
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
      placeholder: {
         type: String,
         default: 'ChooseBelow'
      },
      label: {
         type: String,
         default: ''
      },
      hideStar: {
         type: Boolean,
         default: false
      },
      vid: {
         type: [String, Number],
         default: null
      },
      roleId: {
         type: Number,
         default: null
      }
   },
   setup(props) {
      const dynamicOptions = ref([]);
      const isLoading = ref(false);

      const fetchOptions = async (organizationId) => {
         if (!organizationId) return;
         try {
            isLoading.value = true;
            const response = await UserService.GetAsSelectList({
               organizationId,
               roleId: props.roleId,
               pageSize: 3000,
               isInitQuery: false,
               search: '',
               sortBy: '',
               orderType: 'asc',
               page: 1
            });
            dynamicOptions.value = response.data.rows;
         } catch (error) {
            console.error('Error fetching options:', error);
         } finally {
            isLoading.value = false;
         }
      };

      watch(
         () => props.organizationId,
         (newVal) => {
            fetchOptions(newVal); // `organizationId` o'zgarganda API chaqiriladi
         },
         { immediate: true } // Dastlabki chaqirish uchun
      );

      return {
         dynamicOptions,
         isLoading,
         fetchOptions,
         getValidationState: formValidation().getValidationState
      };
   }
};
</script>
