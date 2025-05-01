<template>
    <div>
        <button :class="`my-button my-background-primary my-btn-${variant} ${shadow ? 'my-box-shadow-primary' : ''} ${block ? 'block' : ''}`">
            <slot></slot>
        </button>
    </div>
</template>

<script>
export default {
    props: {
        block: {
            type: Boolean,
            default: false
        },
        variant: {
            type: String,
            default: 'success'
        },
        shadow: {
            type: Boolean,
            default: false
        }
    }
};
</script>

<style lang="scss" scoped>
@import '../../assets/styles/variables.scss';
// .my-btn-custom{
//     padding-top: 18px;
//     padding-bottom: 18px;
//     outline:none;
//     width: fit-content;
//     border:none;
//     border-radius: 30px;
//     font-size: 15px;
//     transition: 0.4s;
// }
.block {
    width: 100%;
}
// .my-btn-warning{
//     background-color: #F8C80F;
// }
</style>
