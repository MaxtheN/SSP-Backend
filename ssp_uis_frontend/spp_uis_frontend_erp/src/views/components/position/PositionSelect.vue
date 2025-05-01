<template>
   <div>
      <form-input
         :value="position"
         disabled
         :label="$t('position')"
         :required="required"
      >
         <b-input-group-append>
            <b-button variant="primary" @click="dialog = true">
               <feather-icon icon="PlusIcon"></feather-icon>
            </b-button>
         </b-input-group-append>
      </form-input>
      <!-- position list -->
      <b-modal size="xl" :title="$t('position')" v-model="dialog" hide-footer>
         <PositionList :selectable="true" @row-selected="rowSelect" />
      </b-modal>
   </div>
</template>

<script>
// components
import { BButton, BModal, BInputGroup, BInputGroupAppend } from 'bootstrap-vue';
import PositionList from '@/views/components/position/PositionList.vue';
export default {
   components: {
      BButton,
      BModal,
      BInputGroup,
      BInputGroupAppend,
      PositionList
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
      value: {
         type: Number,
         default: null
      },
      position: {
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
