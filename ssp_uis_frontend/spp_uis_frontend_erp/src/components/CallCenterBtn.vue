<template>
   <div class="menu pmd-floating-action" role="navigation">
      <a
         href="https://t.me/aloqa0522"
         class="pmd-floating-action-btn btn btn-sm pmd-btn-fab pmd-btn-raised pmd-ripple-effect btn-light"
         data-title="https://t.me/aloqa0522"
         target="_blank"
      >
         <span class="pmd-floating-hidden">Telegram</span>
         <img src="@/assets/images/callcenter/telegram-icon.svg" height="40px" alt="" />
      </a>
      <!-- <a
         href="tel:+998781138050"
         target="_blank"
         class="pmd-floating-action-btn btn btn-sm pmd-btn-fab pmd-btn-raised pmd-ripple-effect btn-light"
         data-title="78 113 80 50"
      >
         <img src="@/assets/images/callcenter/phone-icon.svg" height="40px" alt="" />
      </a> -->
      <a
         data-title="Call-center"
         class="pmd-floating-action-btn btn pmd-btn-fab pmd-btn-raised pmd-ripple-effect bg-light"
         href="javascript:void(0);"
      >
         <img src="@/assets/images/callcenter/callcenter.png" class="" height="40px" alt="" />
      </a>
   </div>
</template>

<script></script>

<style lang="scss" scoped>
.pmd-floating-action {
   bottom: 1rem;
   position: fixed;
   right: 1rem;
   z-index: 1000;
}

.pmd-floating-action:hover,
.menu--floating--open {
   .pmd-floating-action-btn {
      opacity: 1;
      -ms-transform: none;
      transform: none;
      position: relative;
      bottom: auto;
      overflow: hidden;
      &:before {
         opacity: 1;
      }
   }
}
.pmd-floating-action-btn {
   display: block;
   position: relative;
   &:not(:last-child):before {
      bottom: 10%;
      content: attr(data-title);
      opacity: 0;
      position: absolute;
      right: 100%;
      transition: all 0.2s ease-in-out;
      white-space: nowrap;
      background-color: #fff;
      padding: 0.375rem 0.75rem;
      border-radius: 0.125rem;
      color: rgba(0, 0, 0, 0.87);
      font-size: 0.75rem;
      margin-right: 0.3125rem;
      display: inline-block;
      -moz-box-shadow: 0 3px 1px -2px rgba(0, 0, 0, 0.2), 0 2px 2px 0 rgba(0, 0, 0, 0.14),
         0 1px 5px 0 rgba(0, 0, 0, 0.12);
      -webkit-box-shadow: 0 3px 1px -2px rgba(0, 0, 0, 0.2), 0 2px 2px 0 rgba(0, 0, 0, 0.14),
         0 1px 5px 0 rgba(0, 0, 0, 0.12);
      box-shadow: 0 3px 1px -2px rgba(0, 0, 0, 0.2), 0 2px 2px 0 rgba(0, 0, 0, 0.14), 0 1px 5px 0 rgba(0, 0, 0, 0.12);
   }
   &:last-child {
      box-shadow: 0 19px 38px 0 rgba(34, 36, 43, 0.3);
   }
   &:last-child:before {
      font-size: 14px;
      bottom: 25%;
   }
   &:not(:last-child) {
      opacity: 0;
      transform: translateY(20px) scale(0.3);
      -ms-transform: translateY(20px) scale(0.3);
      margin-bottom: 0.9375rem;
      margin-left: 0;
      position: absolute;
      bottom: 0;
      &:nth-last-child(1) {
         transition-delay: 50ms;
      }
      &:nth-last-child(2) {
         transition-delay: 100ms;
      }
      &:nth-last-child(3) {
         transition-delay: 150ms;
      }
      &:nth-last-child(4) {
         transition-delay: 200ms;
      }
      &:nth-last-child(5) {
         transition-delay: 250ms;
      }
      &:nth-last-child(6) {
         transition-delay: 300ms;
      }
   }
   &.btn:hover {
      overflow: visible;
   }
   .ink {
      width: 50px;
      height: 50px;
   }
}
.pmd-floating-hidden {
   display: none;
}

.btn.pmd-btn-fab {
   padding: 0;
   border-radius: 50%;
   width: 56px;
   height: 56px;
   min-width: 56px;
   display: flex;
   justify-content: center;
   align-items: center;
   font-size: 1.5rem;
}

.pmd-floating-action-btn {
   display: block;
   position: relative;
   transition: all 0.2s ease-in-out;
   transition: all 0.3s cubic-bezier(0.55, 0, 0.1, 1);
}
</style>
