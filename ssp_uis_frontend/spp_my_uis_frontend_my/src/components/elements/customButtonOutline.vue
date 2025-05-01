<template>
  <div>
      <div class="my-btn d-flex align-items-center justify-content-center" :style="css" :class="noHover ? ' no-hover' : ' hover'">
        <slot></slot>
      </div>
  </div>
</template>

<script>
export default {
  props:{
    noHover : {
      type : Boolean,
      default : false
    },
    css : {
      type : Object,
      default : () => {}
    }
  }
}
</script>

<style lang='scss' scoped>
@import '../../assets/styles/variables.scss';
  .my-btn{
    font-size: 15px;
    padding-top: 16px;
    padding-bottom: 16px;
    border:1px solid $primary-dark;
    border-radius: 12px;
    cursor: pointer;
    transition: 0.4s;
    
  }
  .hover{
    &:hover{
      background-color: $primary-dark;
      color : white;
      transition: 0.4s;
      div{
        color : white !important;
        background-color: white;
      }
    }
  }
</style>