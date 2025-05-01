<template>
   <draggable :list="Data.positions" tag="tbody" style="margin-bottom: 14rem" class="StaffingMyTable w-100">
      <tr
         v-for="(item, idx) in Data.positions"
         :key="idx"
         class="h-40 grab"
         :class="{ active: $store.getters['staffingStore/getEditedIndex2'] == idx }"
      >
         <td class="text-center">
            <div class="tr-container w-60 pointer" v-if="!$store.getters['staffingStore/getData'].canCancel&&!isView">
               <feather-icon
                  icon="EditIcon"
                  @click="
                     $store.dispatch('staffingStore/EditPositions', {
                        item,
                        idx
                     })
                  "
                  class="mr-1"
               ></feather-icon>
               <feather-icon
                  icon="TrashIcon"
                  class="text-danger"
                  @click="$store.dispatch('staffingStore/DeletePositions', idx)"
               ></feather-icon>
            </div>
         </td>
         <td>
            <div class="tr-container w-200">
               {{ item.departmentName }}
            </div>
         </td>
         <td>
            <div class="tr-container w-200">
               {{ item.positionName }}
            </div>
         </td>
         <td>
            <div class="tr-container w-100">
               {{ item.quantity }}
            </div>
         </td>
         <td style="white-space: nowrap">
            <div class="tr-container w-200">
               {{ item.rankName }}
            </div>
         </td>
         <td class="text-center">
            {{ item.rankCoef }}
         </td>
         <td class="text-center">
            {{ item.corrCoef }}
         </td>
         <td>
            <div class="tr-container no-wrap w-100">
               {{
                  $options.filters.currency(item.salary, {
                     symbol: '',
                     fractionCount: 2
                  })
               }}
            </div>
         </td>
         <td class="text-right">
            {{
               $options.filters.currency(item.totalSum, {
                  symbol: '',
                  fractionCount: 2
               })
            }}
         </td>
         <td></td>
      </tr>
   </draggable>
</template>
<script>
import draggable from 'vuedraggable';
import StaffingMixins from '../mixins/staffing';

export default {
   components: {
      draggable
   },
   data() {
      return {};
   },
   computed: {
      Data() {
         return this.$store.state['staffingStore'].Data;
      },
      isView() {
         return this.$route.params.isView;
      }
   },
   mixins: [StaffingMixins],
   methods: {
      changePosition(id) {
         const obj = this.$store.getters['staffingStore/getPositionList'].find((el) => el.value === id);
         if (!obj) return null;

         this.$store.dispatch('staffingStore/ChangePositions', obj);
      }
   }
};
</script>
