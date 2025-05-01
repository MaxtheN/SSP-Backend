<template>
   <li
      v-if="item.visible === true ? true : !isVisible(item.visible)"
      class="nav-item"
      :class="{
         active: isActive,
         disabled: item.disabled
         // 'd-none' : item.visible === true ? false : !$can(item.visible,'permissions')
      }"
      :title="$t(item.title)"
   >
      <!-- v-if="canViewVerticalNavMenuLink(item)" -->
      <b-link v-bind="linkProps" class="d-flex align-items-center">
         <feather-icon class="h1" v-if="item.icon" :icon="item.icon" />
         <b-icon-circle v-if="!item.icon" />
         <span class="menu-title text-truncate">{{ t(item.title) }}</span>
         <b-badge v-if="item.tag" pill :variant="item.tagVariant || 'primary'" class="mr-1 ml-auto">
            {{ item.tag }}
         </b-badge>
      </b-link>
   </li>
</template>

<script>
import { useUtils as useAclUtils } from '@core/libs/acl';
import { BLink, BBadge, BIconCircle } from 'bootstrap-vue';
import { useUtils as useI18nUtils } from '@core/libs/i18n';
import useVerticalNavMenuLink from './useVerticalNavMenuLink';
import mixinVerticalNavMenuLink from './mixinVerticalNavMenuLink';

export default {
   components: {
      BLink,
      BBadge,
      BIconCircle
   },
   mixins: [mixinVerticalNavMenuLink],
   props: {
      item: {
         type: Object,
         required: true
      }
   },
   methods: {
      isVisible(arr) {
         let bool = null;
         if (Array.isArray(arr)) {
            for (let i = 0; i < arr.length; i++) {
               if (arr[i] === true) return false;
               bool = bool || this.$can(arr[i], 'permissions');
            }
         } else {
            bool = this.$can(arr, 'permissions');
         }
         return !bool;
      }
   },
   setup(props) {
      const { isActive, linkProps, updateIsActive } = useVerticalNavMenuLink(props.item);
      const { t } = useI18nUtils();
      const { canViewVerticalNavMenuLink } = useAclUtils();

      return {
         isActive,
         linkProps,
         updateIsActive,

         // ACL
         canViewVerticalNavMenuLink,

         // i18n
         t
      };
   }
};
</script>
