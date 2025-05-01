<template>
  <div class="my-label-text text-primary" :class="{ 'simple-label' : content === null || content === undefined || content === '' }">
      {{ content || placeholder }}
  </div>
</template>

<script>
export default {
    props:{
        content : {
            type : String,
            default : ""
        },
        placeholder : {
            type : String,
            default : ""
        }
    }
}
</script>

<style>
.my-label-text{
    width: 100%;
    display: flex;
    align-items: center;
    border: 1px solid #ccc;
    border-radius: 6px;
    padding: 8px;
    /* height: 10px; */
}
.simple-label{
    color : #ccc !important
}
</style>