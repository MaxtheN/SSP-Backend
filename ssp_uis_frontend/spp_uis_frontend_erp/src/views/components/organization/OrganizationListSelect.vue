<template>
   <div class="form-group">
      <form-input
         :value="valuename"
         @input="(e) => $emit('update:valuename', e)"
         :label="$t(label)"
         :required="required"
         :disabled="disabled"
      >
         <b-input-group-append>
            <b-button variant="primary" @click="dialog = true">
               <feather-icon icon="PlusIcon"></feather-icon>
            </b-button>
         </b-input-group-append>
      </form-input>
      <!-- OrganizationList -->
      <b-modal size="xl" :title="$t(label)" v-model="dialog" hide-footer>
         <OrganizationList :selectable="true" @row-selected="rowSelect" />
      </b-modal>
   </div>
</template>

<script>
// components
import { BButton, BModal, BInputGroup, BInputGroupAppend } from 'bootstrap-vue';
import OrganizationList from '@/views/components/organization/OrganizationList.vue';
export default {
   components: {
      BButton,
      BModal,
      BInputGroup,
      BInputGroupAppend,
      OrganizationList
   },
   props: {
      required: {
         type: Boolean,
         default: false
      },
      disabled: {
         type: Boolean,
         default: false
      },
      placeholder: {
         type: String,
         default: 'ChooseBelow'
      },
      value: {},
      valuename: {
         type: [String, Number, Array],
         default: ''
      },
      label: {
         type: String,
         default: 'organization'
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
