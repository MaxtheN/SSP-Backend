<template>
    <div class="cabinet-layout">
        <main class="w-100 position-relative d-flex flex-column">
            <CabinetHeader />

            <CabinetBreadcumbs v-if="$route.meta.breadcrumbs" />

            <div :class="$route.name == 'info' ? 'pb-0' : 'pb-3'" style="height: 100%">
                <slot></slot>
            </div>

            <div id="backdrop"></div>
        </main>

        <MobileBar v-if="isMobile" />
        <MobileSidebar v-if="isMobile" />

        <CabinetSidebar v-else />

        <div v-if="$route.name != 'info'" class="position-absolute" style="bottom: 5px; right: 8px">2024y. © version {{ version }}</div>
    </div>
</template>

<script>
import CabinetHeader from '@/layout/cabinet/CabinetHeader.vue';

import isMobileMixin from '@/mixins/isMobileMixin';

const CabinetSidebar = () => import('@/layout/cabinet/CabinetSidebar.vue');
const MobileBar = () => import('@/layout/cabinet/MobileBar.vue');
const MobileSidebar = () => import('@/layout/cabinet/MobileSidebar.vue');
const CabinetBreadcumbs = () => import('@/layout/cabinet/CabinetBreadcumbs.vue');
import { version } from '/package.json';

export default {
    components: {
        CabinetHeader,
        CabinetBreadcumbs,
        CabinetSidebar,
        MobileBar,
        MobileSidebar
    },
    mixins: [isMobileMixin],
    data() {
        return {
            version
        };
    }
};
</script>

<style>
@import '@/assets/styles/cabinet.scss';
</style>
