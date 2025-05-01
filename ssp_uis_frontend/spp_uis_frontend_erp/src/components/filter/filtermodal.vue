<template>
   <b-sidebar shadow width="15%" class="shadow" id="collapse1" :title="$t('filter')">
      <b-row class="px-2 mt-2">
         <slot />
         <b-col class="mt-2 text-right" @click="claerBtn">
            <b-button variant="danger">
               {{ $t('clear') }}
            </b-button>
         </b-col>
      </b-row>
   </b-sidebar>
</template>

<script>
import {
   VBToggle,
   BSidebar,
   BRow,
   BCol,
   BFormInput,
   BInputGroup,
   BInputGroupAppend,
   BButton,
   BInputGroupPrepend
} from 'bootstrap-vue';

export default {
   data() {
      return {
         olma: true
      };
   },
   components: {
      BRow,
      BCol,
      BFormInput,
      BInputGroup,
      BInputGroupAppend,
      BButton,
      BInputGroupPrepend,
      VBToggle,
      BSidebar
   },
   directives: {
      'b-toggle': VBToggle
   },

   methods: {
      claerBtn() {
         this.$emit('clearBtn');
      }
   }
};
</script>

<style>
.r-0 {
   right: 0;
}

.close {
   color: white !important;
   background-color: red !important;
}

.b-sidebar-header {
   padding: 20px 30px 0px 20px !important;
}
.collapse1___title__ {
   color: red !important;
}
#collapse1 {
   background-color: white !important;
}
</style>
