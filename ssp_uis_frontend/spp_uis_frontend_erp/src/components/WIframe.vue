<template>
   <b-overlay :show="!iframeLoaded" spinner-variant="primary" spinner-type="grow" rounded="sm">
      <b-alert v-if="iframeError" show variant="danger">
         {{ $t('notLoadIframe') }}
      </b-alert>
      <iframe
         v-else-if="showIframe"
         :src="src"
         width="100%"
         height="100%"
         v-bind="$attrs"
         v-on="$listeners"
         frameborder="0"
         @error="iframeError = true"
         @load="iframeLoaded = true"
      ></iframe>
   </b-overlay>
</template>

<script>
import { BOverlay } from 'bootstrap-vue';
export default {
   inheritAttrs: false,
   props: {
      src: {
         type: String,
         default: ''
      },
      show: {
         type: [Boolean, String, Number],
         default: false
      }
   },
   components: {
      BOverlay
   },
   computed: {
      showIframe() {
         return Boolean(this.show);
      }
   },
   data() {
      return {
         iframeLoaded: false,
         iframeError: false
      };
   }
};
</script>
