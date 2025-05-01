<template>
    <aside class="cabinet-sidebar w-100" :class="{ 'd-none': !isMobileMenuOpen }">
        <div class="d-flex py-4 px-4 justify-content-between align-items-center">
            <router-link to="/" class="d-flex align-items-center">
                <b-img src="/img/Logo_SSP.svg" width="30" :alt="$t('publiceducation')" />
                <div class="cabinet-sidebar__title text-white pl-2">{{ $t('publiceducation') }}</div>
            </router-link>

            <b-icon-x-lg color="white" class="cursor-pointer" @click="closeSidebar"></b-icon-x-lg>
        </div>

        <div class="cabinet-sidebar__menu">
            <div>
                <MenuItem v-for="(item, i) in menusFilter" :key="i" :item="item" @click="closeSidebar" />
            </div>
            <div>
                <MenuItemLogout />
            </div>
        </div>
    </aside>
</template>

<script>
import MenuItem from './menu/MenuItem.vue';
import MenuItemLogout from './menu/MenuItemLogout.vue';
import { BIconGrid, BIconPeople, BIconPerson, BIconGear, BIconBox } from 'bootstrap-vue';
import DualIcon from './icons/DualIcon.vue';
import CorrupsionIcon from './icons/CorrupsionIcon.vue';
import ServiceIcon from './icons/ServiceIcon.vue';
import JudgeIcon from './icons/JudgeIcon.vue';
import AppealIcon from './icons/AppealIcon.vue';
import HelpIcon from './icons/HelpIcon.vue';
import { mapGetters, mapActions } from 'vuex';

export default {
    components: {
        MenuItem,
        MenuItemLogout,
        BIconGrid,
        BIconPeople,
        BIconPerson,
        BIconGear,
        BIconBox,
        DualIcon,
        CorrupsionIcon,
        ServiceIcon,
        JudgeIcon,
        AppealIcon,
        HelpIcon
    },
    data() {
        return {
            menus: [
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
            ],
            menus2: [
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
            ]
        };
    },
    computed: {
        menusFilter() {
            // Filtrlash (hide: true bo'lgan elementlarni olib tashlaydi)
            return this.menus.filter((menu) => !menu.hide);
        },

        ...mapGetters(['isMobileMenuOpen'])
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
