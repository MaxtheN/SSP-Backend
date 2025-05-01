export default {
   components: {},
   data() {
      return {};
   },
   methods: {
      compareColCalcKindSum(tr, item) {
         let s = '';
         item.calcKinds.forEach((el) => {
            if (el.calculationKindId === tr.calculationKindId) {
               s = el.calcSum;
            }
         });
         return s;
      }
   }
};
