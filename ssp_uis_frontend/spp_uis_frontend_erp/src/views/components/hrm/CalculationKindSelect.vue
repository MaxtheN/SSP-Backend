<template>
   <div>
      <form-input
         :value="calculationKind"
         disabled
         :label="$t('calculationKind')"
         :required="required"
      >
         <b-input-group-append>
            <b-button variant="primary" @click="dialog = true">
               <feather-icon icon="PlusIcon"></feather-icon>
            </b-button>
         </b-input-group-append>
      </form-input>
      <!-- calculationkind list -->
      <b-modal
         size="xl"
         :title="$t('calculationKind')"
         v-model="dialog"
         hide-footer
      >
         <CalculationKindList
            :calculation-kind-id="value"
            :selectable="true"
            @row-selected="rowSelect"
         />
      </b-modal>
   </div>
</template>

<script>
// components
import { BButton, BModal, BInputGroup, BInputGroupAppend } from 'bootstrap-vue';
import CalculationKindList from '@/views/components/hrm/CalculationKindList.vue';
export default {
   components: {
      BButton,
      BModal,
      BInputGroup,
      BInputGroupAppend,
      CalculationKindList
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
      calculationKind: {
         type: String,
         default: ''
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
