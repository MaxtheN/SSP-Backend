<template>
    <div class="cabinet-card">
        <div class="cabinet-card__header">
            <div>{{ title }}</div>
            <slot name="actions"></slot>
        </div>
        <slot></slot>
    </div>
</template>

<script>
export default {
    props: {
        title: String
    }
};
</script>

<style lang="scss">
.cabinet-card {
    display: flex;
    flex-direction: column;
    align-items: flex-start;
    gap: 10px;
    align-self: stretch;
    border-radius: 0.5rem;
    border: 1px solid var(--Light-gray, #d5d7e1);
    background: #fff;

    &__header {
        display: flex;
        justify-content: space-between;
        align-items: center;
        gap: 10px;
        width: 100%;
        border-bottom: 1px solid #d5d7e1;
        padding: 1rem;
        color: var(--Black, #000107);
        font-size: 18px;
        font-weight: 600;
    }
}
</style>
