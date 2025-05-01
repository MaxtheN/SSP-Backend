<template>
   <form-select :options="items" v-model="value" @input="changeValue" label="SSpMemberStatus"></form-select>
</template>

<script>
export default {
   emits: ['date'],
   data() {
      return {
         value: 1,
         items: [
            {
               value: 1,
               text: this.$t('all')
            },
            {
               value: 2,
               text: this.$t('active')
            },
            {
               value: 3,
               text: this.$t('1PlusMonthExpire')
            },
            {
               value: 4,
               text: this.$t('3PlusMonthExpire')
            },
            {
               value: 5,
               text: this.$t('3MinusMonthExpire')
            }
         ]
      };
   },
   methods: {
      changeValue(e) {
         const date = {
            fromExpireOn: null,
            toExpireOn: null
         };
         if (e == 1) {
            date.fromExpireOn = null;
            date.toExpireOn = null;
         } else if (e == 2) {
            date.fromExpireOn = this.formatDate(new Date());
            date.toExpireOn = null;
         } else if (e == 3) {
            date.fromExpireOn = null;
            date.toExpireOn = this.formatDate(new Date(), 1);
         } else if (e == 4) {
            date.fromExpireOn = null;
            date.toExpireOn = this.formatDate(new Date(), 3);
         } else if (e == 5) {
            date.fromExpireOn = null;
            date.toExpireOn = this.formatDate(new Date(), -3);
         }

         this.$emit('date', date);
      },
      formatDate(d, plusMonth = 0) {
         const date = new Date(d.setMonth(d.getMonth() + plusMonth));
         const day = String(date.getDate()).padStart(2, '0');
         const month = String(date.getMonth() + 1).padStart(2, '0');
         const year = date.getFullYear();

         return day + '.' + month + '.' + year;
      }
   }
};
</script>
