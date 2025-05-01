<template>
    <div v-if="!to" class="p-2" @click="$emit('click')">
        <b-icon :icon="icon"></b-icon>
        <span class="ml-2">{{ label }}</span>
    </div>
    <div v-else class="p-2">
        <router-link :to="to" class="router-link-reset" @click="$emit('click')">
            <b-icon :icon="icon"></b-icon>
            <span class="ml-2">{{ label }}</span>
        </router-link>
    </div>
</template>

<script setup>
import { RouterLink } from 'vue-router';

const props = defineProps({
    icon: {
        type: String
    },
    label: {
        type: String
    },
    to: {
        type: Object,
        default: null
    }
});
</script>

<style lang="scss" scoped>
.router-link-reset {
    text-decoration: none; /* Remove underline */
    color: inherit; /* Inherit color from parent */

    /* Optionally, remove other default anchor styles */
    &:hover,
    &:visited,
    &:focus,
    &:active {
        color: inherit;
        text-decoration: none;
    }
}
</style>
