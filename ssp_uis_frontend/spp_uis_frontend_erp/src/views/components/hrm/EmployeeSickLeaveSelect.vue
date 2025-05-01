<template>
   <div>
      <form-input :value="employee" disabled :label="label" :required="required">
         <b-input-group-append>
            <b-button variant="primary" @click="dialog = true">
               <feather-icon icon="PlusIcon"></feather-icon>
            </b-button>
         </b-input-group-append>
      </form-input>
      <!-- employe manage list -->
      <b-modal size="xl" v-model="dialog" hide-footer>
         <EmployeSickLeaveList :selectable="true" @row-selected="rowSelect" :employeeid="employeeId" />
      </b-modal>
   </div>
</template>

<script>
// components
import { BButton, BModal, BInputGroup, BInputGroupAppend } from 'bootstrap-vue';
import EmployeeManageList from '@/views/components/hrm/EmployeeManageList.vue';
import EmployeSickLeaveList from '@/views/components/hrm/EmployeeSickLeaveList.vue';
export default {
   components: {
      BButton,
      BModal,
      BInputGroup,
      BInputGroupAppend,
      EmployeeManageList,
      EmployeSickLeaveList
   },
   props: {
      required: {
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
      }
   }
};
</script>
