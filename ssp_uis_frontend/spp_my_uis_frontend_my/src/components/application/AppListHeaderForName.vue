<template>
    <div class="container-fluid border-bottom doc-header">
        <div class="doc-header__title">{{ $t(title) }}</div>
        <slot name="top-right"></slot>
    </div>
</template>

<script>
export default {
    props: {
        title: {
            type: String,
            default: ''
        },
        query: {
            type: Object,
            default: null
        },
        type: {
            type: Number,
            default: null
        }
    },
    methods: {
        changeType() {
            this.$emit('changeType', this.$props.type);
        }
    }
};
</script>
<style lang="scss">
.doc-header {
    display: flex;
    align-items: center;
    justify-content: space-between;
    background-color: #fff;
    padding: 16px 24px;
    gap: 10px;
    margin-bottom: 16px;

    &__title {
        color: var(--Black, #000107);
        font-family: 'Museo Sans', serif;
        font-size: 24px;
        font-style: normal;
        font-weight: 600;
        line-height: normal;
    }
}
</style>
