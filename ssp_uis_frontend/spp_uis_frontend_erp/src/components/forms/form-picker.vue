<template>
   <div>
      <validation-provider #default="validationContext" :name="label" :rules="required ? 'required' : null">
         <b-form-group>
            <label v-if="label">
               {{ label }}
               <span v-if="required" style="color: red; font-size: 14px; position: absolute; top: 0; margin-left: 2px"
                  >*</span
               >
            </label>

            <div
               style="border: 1px solid #a7a8ab"
               :class="`my-datepicker ${label === '' ? 'no-label' : ''} 
          ${disabled ? 'my-datepicker-disabled' : ''}
          ${uppertext ? 'uppercase-text' : ''} my-bg-${bg}`"
            >
               <date-picker
                  ref="datepicker"
                  v-model="updateVal"
                  :format="format"
                  value-type="format"
                  :type="type"
                  :range="range"
                  :disabled="disabled"
                  :lang="lang"
                  :placeholder="placeholder || label"
                  :disabled-date="!!cdisabledDate ? cdisabledDate : disabledBeforeStartDay"
                  @focus="$emit('focus', value)"
                  @keyup="$emit('keyup', $event)"
                  @blur="onBlur"
                  @input="updateValue($event)"
                  @close="onClose"
                  @change="onChange"
                  @pick="onPick"
               >
                  <template #input>
                     <input
                        v-model="updateVal"
                        v-mask="
                           type == 'year'
                              ? '####'
                              : type == 'datetime'
                              ? '##.##.#### ##:##:##'
                              : type != 'year' && type != 'month'
                              ? '##.##.####'
                              : ''
                        "
                        :disabled="disabled"
                        :placeholder="placeholder || label"
                        type="text"
                        maxlength="10"
                        :style="todayDate == updateVal ? 'color:green;font-size:1rem' : 'color:#6e6b7b;font-size:1rem'"
                        class="my-custom-date"
                        @keyup="$emit('keyup', $event)"
                        @input="Change"
                     />
                  </template>
               </date-picker>
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
import { ref, toRefs } from 'vue';
import { required, email, url } from '@validations';
import formValidation from '@core/comp-functions/forms/form-validation';
import DatePicker from 'vue2-datepicker';
import 'vue2-datepicker/index.css';

export default {
   components: {
      DatePicker,
      ValidationProvider,
      BFormGroup,
      BFormInvalidFeedback
   },
   props: {
      label: {
         type: String,
         default: ''
      },
      format: {
         type: String,
         default: 'DD.MM.YYYY'
      },
      type: {
         type: String,
         default: ''
      },
      placeholder: {
         type: String,
         default: ''
      },
      mask: {
         type: String,
         default: ''
      },
      disabled: {
         type: [Boolean, Number],
         default: false
      },
      uppertext: {
         type: Boolean,
         default: false
      },
      range: {
         type: Boolean,
         default: false
      },
      bg: {
         type: String,
         default: ''
      },
      required: {
         type: Boolean,
         default: false
      },
      state: {
         type: Boolean,
         default: false
      },
      value: {},
      startdate: {},
      enddate: {},
      cdisabledDate: {}
   },
   data() {
      return {
         inputVal: false,
         todayDate: '',
         // updateVal: null,
         lang: {},
         lang_Uz: {
            formatLocale: {
               // MMMM
               months: [
                  'Yanvar',
                  'Fevral',
                  'Mart',
                  'Aprel',
                  'May',
                  'Iyun',
                  'Iyul',
                  'August',
                  'Sentabr',
                  'Oktabr',
                  'Noyabr',
                  'Dekabr'
               ],
               // MMM
               monthsShort: ['Yan', 'Fev', 'Mar', 'Apr', 'May', 'Iyun', 'Iyul', 'Avg', 'Sen', 'Okt', 'Noy', 'Dek'],
               // dddd
               weekdays: ['Yakshanba', 'Dushanba', 'Seshanba', 'Chorshanba', 'Payshanba', 'Juma', 'Shanba'],
               // ddd
               weekdaysShort: ['Yak', 'Dush', 'Sesh', 'Chor', 'Pay', 'Jum', 'Shan'],
               // dd
               weekdaysMin: ['Ya', 'Du', 'Se', 'Ch', 'Pa', 'Ju', 'Sh'],
               // first day of week
               firstDayOfWeek: 1
            }
         },
         lang_Ru: {
            formatLocale: {
               // MMMM
               months: [
                  'Январь',
                  'Февраль',
                  'Март',
                  'Апрель',
                  'Май',
                  'Июнь',
                  'Июль',
                  'Август',
                  'Сентябрь',
                  'Октябрь',
                  'Ноябрь',
                  'Декабрь'
               ],
               // MMM
               monthsShort: ['Янв', 'Фев', 'Март', 'Апрь', 'Май', 'Июнь', 'Июль', 'Авг', 'Сен', 'Окт', 'Ноя', 'Дек'],
               // dddd
               weekdays: ['Воскресенье', 'Понедельник', 'Вторник', 'Среда', 'Четверг', 'Пятница', 'Суббота'],
               // ddd
               weekdaysShort: ['Вос', 'Пон', 'Вто', 'Сре', 'Чет', 'Пят', 'Суб'],
               // dd
               weekdaysMin: ['Во', 'По', 'Вт', 'Ср', 'Че', 'Пят', 'Су'],
               // first day of week
               firstDayOfWeek: 1
            }
         },
         lang_UzCyrl: {
            formatLocale: {
               // MMMM
               months: [
                  'Январь',
                  'Февраль',
                  'Март',
                  'Апрель',
                  'Май',
                  'Июнь',
                  'Июль',
                  'Август',
                  'Сентябрь',
                  'Октябрь',
                  'Ноябрь',
                  'Декабрь'
               ],
               // MMM
               monthsShort: ['Янв', 'Фев', 'Март', 'Апрь', 'Май', 'Июнь', 'Июль', 'Авг', 'Сен', 'Окт', 'Ноя', 'Дек'],
               // dddd
               weekdays: ['Якшанба', 'Душанба', 'Сешанба', 'Чоршанба', 'Пайшанба', 'Жума', 'Шанба'],
               // ddd
               weekdaysShort: ['Якш', 'Ду', 'Се', 'Чор', 'Пай', 'Жу', 'Шан'],
               // dd
               weekdaysMin: ['Якш', 'Ду', 'Се', 'Чор', 'Пай', 'Жу', 'Шан'],
               // first day of week
               firstDayOfWeek: 1
            }
         }
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
   computed: {
      ErrorClass() {
         if (this.inputVal) {
            if (this.state) {
               return this.state;
            } else {
               return !this.value;
            }
         }
         return '';
      },
      updateVal: {
         get() {
            return this.value == 0 || (this.value && this.value.length == 0) ? 0 : this.value;
         },
         set(val) {
            if (val == 0 || (val && val.length == 0)) {
               this.$emit('input', null);
            } else {
               this.$emit('input', val);
            }
         }
      },
      language() {
         return localStorage.getItem('locale') || 'uz';
      }
   },
   // watch: {
   //   updateVal (val) {
   //     if (val && val.length > 9) {
   //       this.$emit('input', val)
   //     }
   //   }
   // },
   created() {
      const today = new Date();
      const day = String(today.getDate()).padStart(2, '0');
      const month = String(today.getMonth() + 1).padStart(2, '0');
      const year = today.getFullYear();
      this.todayDate = day + '.' + month + '.' + year;
      if (this.language == 'ru') {
         this.lang = this.lang_Ru;
      } else if (this.language == '') {
         this.lang = this.lang_UZ;
      } else {
         this.lang = this.lang_UzCyrl;
      }
   },

   methods: {
      Change() {
         if (this.inputVal == null) {
            this.updateVal = null;
         }
      },
      onClose() {
         // this.checkInput()
      },
      onChange(newValue) {
         this.$emit('change', newValue);
      },
      checkInput() {
         this.inputVal = true;
      },
      resetInput() {
         this.inputVal = false;
      },
      onBlur($event) {
         this.$emit('blur', $event);
      },
      updateValue(value) {
         this.$emit('input', this.uppertext ? value.toUpperCase() : value);
      },
      onPick() {
         if (this.type != 'datetime') {
            this.$refs.datepicker.closePopup();
         }
      },
      disabledBeforeStartDay(date) {
         if (this.enddate) {
            const dFormat1 = this.enddate.slice(0, 10).split('.').reverse().join('-');
            const endDate = new Date(dFormat1);
            return date > endDate;
         }
         if (this.enddate && this.startdate) {
            const dFormat = this.startdate.slice(0, 10).split('.').reverse().join('-');
            const dFormat1 = this.enddate.slice(0, 10).split('.').reverse().join('-');
            const startDate = new Date(dFormat);
            const endDate = new Date(dFormat1);
            return date > endDate || date < startDate;
         }
         if (this.startdate) {
            const dFormat = this.startdate.slice(0, 10).split('.').reverse().join('-');
            const startDate = new Date(dFormat);
            return date < startDate;
         }
      }
   }
};
</script>

<style lang="scss" scoped>
@import '@/assets/form-date.scss';
.RedBorder {
   border: 1px solid red !important;
   // :style="

   //          ? 'border: 1px solid red'
   //          : 'border: 1px solid #A7A8AB'
   //      "
}
</style>
