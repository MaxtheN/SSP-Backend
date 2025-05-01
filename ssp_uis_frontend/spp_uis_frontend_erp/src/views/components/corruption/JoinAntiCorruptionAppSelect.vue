<template>
   <div>
      <form-input :value="appName" disabled :label="$t('JoinAntiCorruptionApplication')" :required="required">
         <b-input-group-append>
            <b-button variant="primary" @click="dialog = true">
               <feather-icon icon="PlusIcon"></feather-icon>
            </b-button>
         </b-input-group-append>
      </form-input>
      <!-- JoinAntiCorruptionApplication list -->
      <b-modal size="xl" :title="$t('JoinAntiCorruptionApplication')" v-model="dialog" hide-footer>
         <JoinAntiCorruptionAppList
            :selectable="true"
            :statusId="2"
            hideStatus
            @row-selected="rowSelect"
            :withoutCertificate="withoutCertificate"
         />
      </b-modal>
   </div>
</template>

<script>
// components
import { BButton, BModal, BInputGroup, BInputGroupAppend } from 'bootstrap-vue';
import JoinAntiCorruptionAppList from '@/views/components/corruption/JoinAntiCorruptionAppList.vue';
export default {
   components: {
      BButton,
      BModal,
      BInputGroup,
      BInputGroupAppend,
      JoinAntiCorruptionAppList
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
      appName: {
         type: [String, Number, Array],
         default: ''
      },
      withoutCertificate: {
         type: Boolean,
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
            this.$emit('input', e.application.id);
         } else {
            this.$emit('input', null);
         }
      }
   }
};
</script>
