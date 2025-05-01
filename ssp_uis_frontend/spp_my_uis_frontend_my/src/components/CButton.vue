<template>
    <b-button @click="$emit('click')" :href="href" variant="light" class="c-button"> <b-icon :icon="icon" class="innericon"></b-icon> </b-button>
</template>

<script>
export default {
    props: {
        icon: {
            type: String,
            default: ''
        },
        href: {
            type: String,
            default: ''
        }
    }
};
</script>

<style lang="scss" scoped>
.c-button {
    background-color: #f6f6f7;
    height: 30px;
    width: 30px;
    padding: 10px;
    border-radius: 6px;
    .innericon {
        width: 20px;
        color: #000107;
        height: 20px;
    }
}
</style>
