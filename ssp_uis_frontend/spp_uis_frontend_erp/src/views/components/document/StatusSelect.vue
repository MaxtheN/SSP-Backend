<template>
   <div>
      <b-button-group size="sm" v-if="!isMobileDevice()">
         <template v-if="statusListLoading">
            <b-skeleton
               v-for="i in 2"
               :key="i + 'skeleton'"
               animation="fade"
               width="150px"
               height="35px"
               class="mr-1 mb-0"
            ></b-skeleton>
         </template>

         <template v-else>
            <b-button
               v-for="status in statusList"
               :key="status.value"
               @click="$emit('input', status.value)"
               :variant="value == status.value ? 'primary' : 'outline-primary'"
            >
               {{ status.text }}
            </b-button>
         </template>
      </b-button-group>
      <template v-else>
         <form-select :options="statusList" @change="(e) => $emit('input', e)" :value="value" label="status" />
      </template>
   </div>
</template>

<script>
import { BButton, BButtonGroup, BSkeleton } from 'bootstrap-vue';
export default {
   props: {
      value: {
         type: [String, Number],
         default: null
      },
      filter: {
         type: Array,
         default: () => []
      },
      step: {
         type: Boolean,
         default: false
      }
   },
   components: {
      BButton,
      BButtonGroup,
      BSkeleton
   },
   computed: {
      statusList() {
         const list = this.$store.getters['status/filteredStatusList'](this.filter) || [];

         if (this.filter && this.filter.length > 0) {
            list
               .sort((a, b) => {
                  const aValueIndex = this.filter.indexOf(a.value);
                  const bValueIndex = this.filter.indexOf(b.value);
                  return bValueIndex - aValueIndex;
               })
               .reverse();
         }

         return [{ value: null, text: this.$t('all') }, ...list];
      },
      statusListLoading() {
         return this.$store.getters['status/statusListLoading'];
      }
   },
   created() {
      this.$store.dispatch('status/fetchStatusList');
   }
};
</script>
