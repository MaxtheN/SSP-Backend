<template>
    <router-link v-if="item.hide != false" :to="item.to">
        <div class="cabinet-menu__item" @click="closeSidebar" v-if="isMobile">
            <component class="cabinet-menu__icon" v-if="item.icon" :is="item.icon"></component>
            <div class="cabinet-menu__label">{{ $t(item.label) }}</div>
        </div>
        <div class="cabinet-menu__item" @click="closeSidebar" v-else>
            <component class="cabinet-menu__icon" v-if="item.icon" :is="item.icon"></component>
            <div class="cabinet-menu__label">{{ $t(item.label) }}</div>
        </div>
    </router-link>
</template>

<script>
import isMobileMixin from '@/mixins/isMobileMixin';
import { mapActions } from 'vuex';
export default {
    mixins: [isMobileMixin],
    name: 'MenuItem',
    props: {
        item: {
            type: Object,
            required: true
        }
    },
    methods: {
        ...mapActions(['toggleMobileMenu', 'closeMobileMenu']),
        closeSidebar() {
            this.closeMobileMenu();
        }
    }
};
</script>

<style lang="scss">
.cabinet-menu {
    &__item {
        display: flex;
        align-items: center;
        padding: 8px 14px 8px 21px;
        margin: 8px 0;
        gap: 10px;
        color: #b7c0cb;
        border-left: 3px solid transparent;

        &:hover,
        &.active {
            color: #fff;
            transition: color 0.15s linear;
            text-decoration: none;
            border-left: 3px solid #fff;
        }
    }

    &__label {
        overflow: hidden;
        text-overflow: ellipsis;
        font-size: 16px;
        font-style: normal;
        font-weight: 500;
        line-height: normal;
        display: -webkit-box;
        -webkit-box-orient: vertical;
        -webkit-line-clamp: 1;
        flex: 1 0 0;
        max-width: 100%;
    }

    &__icon {
        width: 24px;
        height: 24px;
    }
}
</style>
