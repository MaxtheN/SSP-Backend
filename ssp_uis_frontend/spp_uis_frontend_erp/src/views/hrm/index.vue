<script>
import hrm from '@/navigation/vertical/hrm';

import { BCard, BListGroup, BListGroupItem } from 'bootstrap-vue';

export default {
   components: {
      BCard,
      BListGroup,
      BListGroupItem
   },
   data() {
      return {
         hrm: hrm
      };
   },
   computed: {
      items() {
         return hrm.find((e) => e.route == this.$route.name);
      }
   }
};
</script>

<template>
   <b-card>
      <template v-if="items">
         <template v-if="items.isParent">
            <b-list-group>
               <template v-for="item in hrm">
                  <b-list-group-item
                     v-if="!item.isParent"
                     class="font-weight-bold"
                     :to="{ name: item.route }"
                     :key="item.route"
                  >
                     {{ $t(item.title) }}
                  </b-list-group-item>
               </template>
            </b-list-group>
         </template>

         <b-list-group v-if="items.children">
            <b-list-group-item
               v-for="item in items.children"
               :to="{ name: item.route }"
               :key="item.route"
               class="font-weight-bold"
            >
               {{ $t(item.title) }}

               <b-list-group v-if="item.children" class="mt-1 ml-2">
                  <b-list-group-item
                     variant="light"
                     v-for="item1 in item.children"
                     :to="{ name: item1.route }"
                     :key="item1.route"
                     class="font-weight-bold text-info"
                  >
                     {{ $t(item1.title) }}
                  </b-list-group-item>
               </b-list-group>
            </b-list-group-item>
         </b-list-group>
      </template>
   </b-card>
</template>

<style lang="scss" scoped></style>
