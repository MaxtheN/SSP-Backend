<template>
  <div class="my-radio-button d-flex align-items-center justify-content-center">
      <div class="my-radio-button-bg" :class="{ 'active' : active }">
          
      </div>
  </div>
</template>

<script>
export default {
    props:{
        active:{
            type : Boolean,
            default : false
        }
    }
}
</script>

<style lang="scss" scope>
@import '../../assets/styles/variables.scss';
.my-radio-button{
    width: 32px;
    height: 32px;
    border-radius: 50%;
    border:3px solid $success;
    cursor: pointer;
    
    .my-radio-button-bg.active{
        padding: 8px;
        width: 16px;
        height: 16px;
        border-radius: 50%;
        background-color: $success;
    }
}
</style>