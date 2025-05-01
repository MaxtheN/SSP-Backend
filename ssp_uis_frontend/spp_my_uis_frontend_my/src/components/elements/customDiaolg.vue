<template>
  <div @input="$emit('input',value)" :class="`my-custom-dialog ${value ? 'my-open-dialog' : ''} ${position==='top'?'my-top-dialog' : 'my-bottom-dialog'}`" :style="{ height : height,top : position === 'top' ? `-${height}` : 'auto',bottom : position === 'bottom' ? `-${height}` : 'auto'   }">
      <slot></slot>
  </div>
</template>

<script>
export default {
  props:{
    value : {
      type : Boolean,
      default : false
    },
    height : {
      type : String,
      default : 'auto'
    },
    position : {
      type : String,
      default : 'top'
    }
  }
}
</script>

<style lang="scss" scope>
.my-custom-dialog{
  width : 100%;
  background-color: #fff;
  transition: 0.4s ease-in-out;
  z-index: 20 !important;
  position: fixed;
  left : 0;
}
.my-custom-dialog.my-top-dialog{
   top:0;
   
}
.my-open-dialog.my-top-dialog{
  top : 0 !important;
  transition: 0.4s ease-in-out;
  box-shadow: rgba(0, 0, 0, 0.1) 0px 4px 12px;
}
.my-custom-dialog.my-bottom-dialog{
   bottom:0;
}
.my-open-dialog.my-bottom-dialog{
  bottom : 0 !important;
  transition: 0.4s ease-in-out;
}

</style>