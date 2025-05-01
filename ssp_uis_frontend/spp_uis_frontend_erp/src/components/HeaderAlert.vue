<script>
export default {
   data() {
      return {
         show: new Date() < new Date('12/04/2023')
      };
   }
};
</script>

<template>
   <div class="example1" v-if="show">
      <div>
         <h3>
            {{ $t('sspAlert') }}
            {{ $t('sspSorry') }}
         </h3>
         <h3>{{ $t('sspAlert') }} {{ $t('sspSorry') }}</h3>
      </div>
      <button type="button" aria-label="Close" class="close" @click="show = false">×</button>
   </div>
</template>

<style lang="scss">
.example1 {
   height: 35px;
   width: 100%;
   overflow: hidden;
   position: sticky;
   top: 0;
   z-index: 999;
   background-color: #f7eaeb;
}

.example1 > div {
   width: max-content;
   height: 35px;
   overflow: hidden;
   position: absolute;
}

.example1 h3 {
   padding-left: 40px;
   float: left;
   width: 50%;
   text-wrap: nowrap;
   line-height: 35px;
   font-size: 18px;
   color: red;

   /* Starting position */
   -moz-transform: translateX(100%);
   -webkit-transform: translateX(100%);
   transform: translateX(100%);
   /* Apply animation to this element */
   -moz-animation: example1 10s linear infinite;
   -webkit-animation: example1 10s linear infinite;
   animation: example1 10s linear infinite;
}
.example1:hover h3 {
   animation-play-state: paused;
}

.example1 button {
   height: 100%;
   padding: 0 20px;
}

@keyframes example1 {
   0% {
      -moz-transform: translateX(0%);
      -webkit-transform: translateX(0%);
      transform: translateX(0%);
   }
   100% {
      -moz-transform: translateX(-100%);
      -webkit-transform: translateX(-100%);
      transform: translateX(-100%);
   }
}
</style>
