<template>
   <div>
      <validation-provider #default="validationContext" name="employee" :rules="requiredStar ? 'required' : null">
         <b-form-group :state="getValidationState(validationContext)">
            <label>
               {{ label ? $t(`${label}`) : $t(`employee`) }}
               <span v-if="requiredStar" style="color: red">*</span>
            </label>
            <div>
               <v-select
                  :options="options"
                  :placeholder="$t(`employee`)"
                  v-bind="$attrs"
                  :reduce="(e) => e.id"
                  :value="value"
                  label="fullName"
                  :filterable="false"
                  @input="onInput"
                  @open="onOpen"
                  @close="onClose"
                  @search="inputSearch"
                  @option:selected="onSelected"
               >
                  <template #list-header v-if="!hideAdd">
                     <li style="text-align: center; padding: 3px">
                        <b-button @click="addDialog = true" block size="sm" variant="outline-primary">
                           <feather-icon icon="PlusIcon"></feather-icon>
                           {{ $t('Add') }}
                        </b-button>
                     </li>
                  </template>
                  <template #list-footer>
                     <li v-show="hasNextPage" ref="load" style="text-align: center">Loading more options...</li>
                  </template>
               </v-select>
            </div>

            <b-form-invalid-feedback :state="getValidationState(validationContext)">{{
               $t('fieldNotEmpty')
            }}</b-form-invalid-feedback>
         </b-form-group>
      </validation-provider>

      <b-modal v-model="addDialog" size="xl" :title="$t('employee')" hide-footer>
         <EmployeeAdd is-dialog @employee:add="employeeAdd" />
      </b-modal>
   </div>
</template>

<script>
import EmployeeService from '@/services/info/employee.service.js';
import axios from 'axios';
import formValidation from '@core/comp-functions/forms/form-validation';
import { ValidationProvider } from 'vee-validate';
import { BFormGroup, BFormInvalidFeedback, BButton, BModal } from 'bootstrap-vue';
const { CancelToken } = axios;
const source = CancelToken.source();

const debounce = (func, delay) => {
   let debounceTimer;
   // eslint-disable-next-line func-names
   return function () {
      const context = this;
      // eslint-disable-next-line prefer-rest-params
      const args = arguments;
      clearTimeout(debounceTimer);
      debounceTimer = setTimeout(() => func.apply(context, args), delay);
   };
};

export default {
   components: {
      ValidationProvider,
      BFormGroup,
      BFormInvalidFeedback,
      BButton,
      BModal,
      EmployeeAdd: () => import('@/views/hrm/info/employee/edit.vue')
   },
   props: {
      organisation: {
         type: [Number, null],
         default: null
      },
      label: {
         type: String,
         default: 'employee'
      },
      value: {
         type: [Number, String],
         default: null
      },
      requiredStar: {
         type: Boolean,
         default: false
      },
      hideAdd: {
         type: Boolean,
         default: false
      }
   },
   emits: ['input', 'update:data'],
   data() {
      return {
         observer: null,
         options: [],
         page: 0,
         pageSize: 20,
         total: 0,
         addDialog: false
      };
   },
   setup() {
      const { getValidationState } = formValidation();

      return {
         getValidationState
      };
   },
   mounted() {
      this.observer = new IntersectionObserver(this.infiniteScroll);
      if (this.value) {
         this.getOptions('', undefined, {
            id: {
               value: this.value,
               matchMode: 'equal'
            }
         });
      } else {
         this.getOptions();
      }
   },
   computed: {
      hasNextPage() {
         return this.options.length < this.total;
      }
   },
   methods: {
      async getEmployee(id) {
         await EmployeeService.Get(id).then((res) => {
            if (res.data && res.data.person) {
               this.options.push({ ...res.data.person, id: res.data.id });
            }
            this.total = 1;
         });
      },
      async employeeAdd(e) {
         console.log(e, 'employee add');
         this.onInput(e.id);
         this.addDialog = false;
         await this.getEmployee(e.id);
         const employee = this.options.find((o) => o.id == e.id);
         this.onSelected(employee);
      },
      onInput(e) {
         this.$emit('input', e);
         if (!e) {
            this.$emit('update:data', null);
         }
      },
      onSelected(selectedOption) {
         this.$emit('update:data', selectedOption);
      },
      async onOpen() {
         if (this.hasNextPage) {
            await this.$nextTick();
            this.observer.observe(this.$refs.load);
         }
      },
      onClose() {
         this.observer.disconnect();
      },
      async infiniteScroll([{ isIntersecting, target }]) {
         if (isIntersecting) {
            const ul = target.offsetParent;
            const { scrollTop } = target.offsetParent;
            await this.getOptions();
            await this.$nextTick();
            ul.scrollTop = scrollTop;
         }
      },
      async getOptions(search, loading = () => {}, filters = {}) {
         this.page++;
         loading(true);
         source.cancel();

         EmployeeService.GetList(
            {
               organisationId: this.$props.organisation,
               page: this.page,
               pageSize: this.pageSize,
               total: this.total,
               search: search,
               orderType: 'asc',
               filters: filters,
               ids: this.value ? [this.value] : []
            },
            {
               cancelToken: source.token
            }
         )
            .then((res) => {
               const { rows, total = 0 } = res.data;
               if (Array.isArray(rows)) {
                  this.options.push(...rows);
               }
               this.total = total;
            })
            .finally(() => {
               loading(false);
            });
      },
      inputSearch: debounce(async function (search, loading) {
         if (search) {
            this.options = [];
            this.page = 0;
            this.getOptions(search, loading);
         } else {
            this.getOptions(search, loading);
         }
      }, 300)
   },
   watch: {
      organisation() {
         this.options = [];
         this.getOptions();
      }
   }
};
</script>
