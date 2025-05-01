<template>
  <div class="my-custom-progress" :style="{ width : width,backgroundColor : bgColor }">

  </div>
</template>

<script>
export default {
    props : {
        width : {
            type : String,
            default : '10%'
        },
        bgColor : {
            type : String,
            default : '#fff'
        }
    }
}
</script>

<style lang="scss" scoped>
@import '../../assets/styles/variables.scss';
.my-custom-progress{
    height: 10px;
    border-radius: 10px;
}
</style>