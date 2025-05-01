<template>
   <div>
      <div class="mt-2 font-weight-bolder mb-1">
         {{ $t('Agreed') }}
      </div>
      <b-table
         :fields="SignerField"
         bordered
         small
         responsive
         :items="signerComp"
         hover
         show-empty
         :empty-text="$t('NotFound')"
      >
      </b-table>
   </div>
</template>

<script>
import { BTable } from 'bootstrap-vue';
export default {
   components: {
      BTable
   },
   props: {
      signer: {
         type: Array,
         default: () => []
      }
   },
   data() {
      return {
         SignerField: [
            {
               key: 'employee',
               label: this.$t('employee'),
               sortable: true
            },
            {
               key: 'department',
               label: this.$t('department'),
               sortable: true
            },

            {
               key: 'position',
               label: this.$t('position'),
               sortable: true
            }
         ]
      };
   },
   computed: {
      signerComp() {
         return this.signer.filter((e) => !e.isHr && !e.isDirector);
      }
   }
};
</script>
