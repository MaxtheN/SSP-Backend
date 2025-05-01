<template>
   <div>
      <form-input :value="employee" disabled :label="label" :required="required">
         <b-input-group-append>
            <b-button variant="primary" @click="openModal">
               <feather-icon icon="PlusIcon"></feather-icon>
            </b-button>
         </b-input-group-append>
      </form-input>
      <!-- employe manage list -->
      <b-modal size="xl" :title="$t('employeeManage')" v-model="dialog" hide-footer>
         <EmployeeManageList
            :is-organization="isOrganisation"
            :department-id="departmentId"
            :employee-manage-id="value"
            :organization-id="organizationId"
            :selectable="true"
            @row-selected="rowSelect"
            :employee-id="employeeId"
            :hide-is-only-working-employee="true"
            :is-only-working-employee="true"
         />
      </b-modal>
   </div>
</template>

<script>
// components
import { BButton, BModal, BInputGroup, BInputGroupAppend } from 'bootstrap-vue';
import EmployeeManageList from '@/views/components/hrm/EmployeeManageList.vue';
export default {
   components: {
      BButton,
      BModal,
      BInputGroup,
      BInputGroupAppend,
      EmployeeManageList
   },
   props: {
      required: {
         type: Boolean,
         default: false
      },

      isOrganisation: {
         type: Boolean,
         default: false
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

      employee: {
         type: String,
         default: ''
      },
      employeeId: {
         type: Number,
         default: null
      },
      organizationId: {
         type: Number,
         default: null
      },
      departmentId: {
         type: Number,
         default: null
      }
   },
   emit: ['input', 'update:data'],
   data() {
      return {
         dialog: false
      };
   },
   methods: {
      rowSelect(e) {
         this.dialog = false;
         this.$emit('update:data', e);
         if (e) {
            this.$emit('input', e.id);
         } else {
            this.$emit('input', null);
         }
      },
      openModal() {
         this.employeeId = null;
         this.dialog = true;
      }
   }
};
</script>
