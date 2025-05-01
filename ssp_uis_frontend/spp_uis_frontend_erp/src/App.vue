<template>
   <div id="app" class="h-100" :class="[skinClasses]">
      <!-- alert -->
      <!-- <HeaderAlert /> -->
      <!-- <SnowBoard /> -->

      <component :is="layout">
         <router-view />
      </component>

      <scroll-to-top v-if="enableScrollToTop" />
      <!-- <CallCenterBtn v-if="['dashboard', 'auth-login'].includes($route.name)" /> -->
   </div>
</template>

<script>
import ScrollToTop from '@core/components/scroll-to-top/ScrollToTop.vue';

// This will be populated in `beforeCreate` hook
import { $themeColors, $themeBreakpoints, $themeConfig } from '@/themeConfig';
import { watch } from 'vue';
import useAppConfig from '@core/app-config/useAppConfig';

import { useWindowSize, useCssVar } from '@vueuse/core';
import store from '@/store';
const LayoutVertical = () => import('@/layouts/vertical/LayoutVertical.vue');
const LayoutHorizontal = () => import('@/layouts/horizontal/LayoutHorizontal.vue');
const LayoutFull = () => import('@/layouts/full/LayoutFull.vue');

export default {
   components: {
      LayoutHorizontal,
      LayoutVertical,
      LayoutFull,
      ScrollToTop
   },

   computed: {
      layout() {
         if (this.$route.meta.layout === 'full') return 'layout-full';
         return `layout-${this.contentLayoutType}`;
      },
      contentLayoutType() {
         return this.$store.state.appConfig.layout.type;
      }
   },
   beforeCreate() {
      // Set colors in theme
      const colors = ['primary', 'secondary', 'success', 'info', 'warning', 'danger', 'light', 'dark'];

      // eslint-disable-next-line no-plusplus
      for (let i = 0, len = colors.length; i < len; i++) {
         $themeColors[colors[i]] = useCssVar(`--${colors[i]}`, document.documentElement).value.trim();
      }

      // Set Theme Breakpoints
      const breakpoints = ['xs', 'sm', 'md', 'lg', 'xl'];

      // eslint-disable-next-line no-plusplus
      for (let i = 0, len = breakpoints.length; i < len; i++) {
         $themeBreakpoints[breakpoints[i]] = Number(
            useCssVar(`--breakpoint-${breakpoints[i]}`, document.documentElement).value.slice(0, -2)
         );
      }

      // Set RTL
      const { isRTL } = $themeConfig.layout;
      document.documentElement.setAttribute('dir', isRTL ? 'rtl' : 'ltr');
   },
   setup() {
      const { skin, skinClasses } = useAppConfig();
      const { enableScrollToTop } = $themeConfig.layout;
      // If skin is dark when initialized => Add class to body
      if (skin.value === 'dark') document.body.classList.add('dark-layout');

      // Provide toast for Composition API usage
      // This for those apps/components which uses composition API

      // Set Window Width in store
      store.commit('app/UPDATE_WINDOW_WIDTH', window.innerWidth);
      const { width: windowWidth } = useWindowSize();
      watch(windowWidth, (val) => {
         store.commit('app/UPDATE_WINDOW_WIDTH', val);
      });

      return {
         skinClasses,
         enableScrollToTop
      };
   }
};
</script>
