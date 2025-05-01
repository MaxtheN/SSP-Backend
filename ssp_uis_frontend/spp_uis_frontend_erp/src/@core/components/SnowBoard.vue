<script setup lang="ts"></script>

<template>
   <div class="snow-board">
      <template v-for="i in 50">
         <div class="snow" :key="i"></div>
         <div class="snow" :key="i"></div>
         <div class="snow snow-img" :key="i">❅</div>
         <div class="snow snow-img" :key="i">❅</div>
         <div class="snow snow-img" :key="i">❅</div>
      </template>
   </div>
</template>

<style lang="scss">
@use 'sass:math';
.snow-board {
   height: 100%;
   left: 0;
   position: fixed;
   top: 0;
   width: 100%;
   z-index: 0;
}

.snow {
   background: #b4efff;
   border-radius: 50%;
   color: #6bbbfd;
   font-size: 28px;
   height: 10px;
   position: absolute;
   width: 10px;
   z-index: 2999;
}

.snow-img {
   background: 0 0 !important;
}

@function random($min, $max) {
   @return $min + math.random() * ($max - $min);
}

@for $i from 1 through 150 {
   $animationDelay: (($i) %10-10 * 2s);
   $animationTime: ($i%10 * 2s);
   $translateX: round(random(0, 100));
   $translateY: round(random(30, 70));
   $scale: random(0.2, 1);

   .snow:nth-child(#{$i}) {
      animation: fall-#{$i} #{$animationTime} linear #{$animationDelay} infinite;
      opacity: random(0, 1);
      transform: translate($translateX + vw, -10px) scale($scale);
   }

   @keyframes fall-#{$i} {
      30% {
         transform: translate($translateX + vw, $translateY + vh) scale($scale);
      }

      to {
         transform: translate($translateX + vw, 100vh) scale($scale);
      }
   }
}
</style>
