<template>
   <b-tabs class="nav-tabs nav-justified" :value="value">
      <b-tab :title="$t(viewTitle)">
         <slot name="view"></slot>
      </b-tab>
      <b-tab :title="$t(pdfTitle)">
         <slot name="pdf"></slot>
      </b-tab>
   </b-tabs>
</template>

<script>
import { BTab, BTabs } from 'bootstrap-vue';
export default {
   props: {
      viewTitle: {
         type: String,
         default: 'Info'
      },
      pdfTitle: {
         type: String,
         default: 'Pdf'
      },
      value: {
         type: Number,
         default: 0
      }
   },
   components: {
      BTab,
      BTabs
   }
};
</script>
