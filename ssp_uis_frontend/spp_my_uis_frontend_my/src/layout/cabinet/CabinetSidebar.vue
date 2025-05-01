<template>
    <aside class="cabinet-sidebar">
        <router-link to="/" class="d-flex py-4 px-4 justify-content-center align-items-center">
            <div style="width: 30px">
                <b-img src="/img/Logo_SSP.svg" width="30" :alt="$t('publiceducation')" />
            </div>
            <div class="cabinet-sidebar__title text-white pl-2">{{ $t('publiceducation') }}</div>
        </router-link>

        <div class="cabinet-sidebar__menu">
            <div>
                <MenuItem v-for="(item, i) in menusFilter" :key="i" :item="item" />
            </div>
            <div>
                <div class="px-4 text-grey">
                    {{ $t('another') }}
                </div>
                <MenuItem v-for="(item, i) in menus2" :key="i" :item="item" />

                <div class="border-bottom"></div>

                <MenuItemLogout />
            </div>
        </div>
    </aside>
</template>

<script setup>
import { computed } from 'vue';
import MenuItem from './menu/MenuItem.vue';
import MenuItemLogout from './menu/MenuItemLogout.vue';
import { BIconGrid, BIconPeople, BIconPerson, BIconGear, BIconBox } from 'bootstrap-vue';
import DualIcon from './icons/DualIcon.vue';
import CorrupsionIcon from './icons/CorrupsionIcon.vue';
import ServiceIcon from './icons/ServiceIcon.vue';
import JudgeIcon from './icons/JudgeIcon.vue';
import AppealIcon from './icons/AppealIcon.vue';
import HelpIcon from './icons/HelpIcon.vue';

const menus = [
    {
        to: '/mycabinet',
        icon: BIconGrid,
        label: 'isMain'
    },
    {
        to: '/partnership',
        icon: BIconPeople,
        label: 'yigirmaming_tadbirkor'
    },
    {
        to: '/memship',
        icon: BIconPerson,
        label: 'memship'
    },
    {
        to: '/claimapplication',
        icon: BIconBox,
        label: 'ClaimApplication',
        hide: false
    },
    {
        to: '/dual',
        icon: DualIcon,
        label: 'Dual'
    },
    {
        to: '/joinanticorruptionapplication',
        icon: CorrupsionIcon,
        label: 'JoinAntiCorruptionApplication'
    },
    {
        to: '/srv',
        icon: ServiceIcon,
        label: 'Services'
    },
    {
        to: '/hakamliksudi',
        icon: JudgeIcon,
        label: 'HakamlikSudi'
    },
    {
        to: '/appeal',
        icon: AppealIcon,
        label: 'Appeal'
    }
];

const menusFilter = computed(() => menus.filter((e) => !e.hide));

const menus2 = [
    {
        to: '/settings',
        icon: BIconGear,
        label: 'settings'
    },
    {
        to: '/help',
        icon: HelpIcon,
        label: 'help'
    }
];
</script>

<style lang="scss">
.cabinet-sidebar {
    width: 264px;
    background: #043776;
    height: 100vh;
    display: flex;
    flex-direction: column;
    flex-shrink: 0;
    flex-grow: 0;
    position: fixed;
    z-index: 1040;

    &__title {
        font-size: 1.125rem;
        font-weight: 600;
        width: auto;
    }

    &__menu {
        color: #fff;
        display: flex;
        flex-direction: column;
        justify-content: space-between;
        height: 100%;
        overflow: auto;
    }
}
</style>
