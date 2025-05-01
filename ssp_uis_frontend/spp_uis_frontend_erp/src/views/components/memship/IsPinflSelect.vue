<template>
   <form-select
      :options="items"
      :value="computedValue"
      @input="(e) => $emit('input', e === -1 ? null : e)"
      label="contractorIsPinfl"
   ></form-select>
</template>

<script>
export default {
   props: {
      value: {
         type: [Boolean, null],
         default: null
      }
   },
   emits: ['input'],
   computed: {
      computedValue() {
         if (this.value === null) return -1;
         else return this.value;
      }
   },
   data() {
      return {
         items: [
            {
               value: -1,
               text: this.$t('all')
            },
            {
               value: false,
               text: this.$t('Yuridik')
            },
            {
               value: true,
               text: this.$t('YaTT')
            }
         ]
      };
   }
};
</script>
