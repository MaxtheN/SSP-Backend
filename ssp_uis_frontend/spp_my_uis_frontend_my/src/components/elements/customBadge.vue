<template>
  <div :class="`my-custom-badge ${variant}-badge`">
      <slot></slot>
  </div>
</template>

<script>
export default {
    props : {
        variant : {
            type : String,
            default : "success"
        }
    }
}
</script>

<style lang="scss" scoped>
@import '../../assets/styles/variables.scss';
    .my-custom-badge{
        padding: 0px 8px;
        
        color: white;
        border-radius: 7px;
    }
    .success-badge{
        background-color: $success;
    }
    .danger-badge{
        background-color: $danger;
    }

</style>