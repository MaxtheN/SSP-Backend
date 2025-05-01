<template>
    <div id="app" style="background-color: #F7F9FB">
        <template v-if="$route.meta.layout == 'full'">
            <router-view></router-view>
        </template>
        <template v-else-if="$route.meta.isAuth == false">
            <div :class="$route.path != '/' ? 'back-font-color' : ''" class="back-font">
                <div class="my-content">
                    <router-view></router-view>
                </div>
                <my-footer></my-footer>
            </div>
        </template>
        <CabinetLayout v-else-if="$route.meta.isAuth == true">
            <router-view></router-view>
        </CabinetLayout>
    </div>
</template>

<script>
const myFooter = () => import('@/layout/footer.vue');

const CabinetLayout = () => import('@/layout/CabinetLayout.vue');

export default {
    components: { myFooter, CabinetLayout }
};
</script>

<style>
.nav-link {
    color: white !important;
}
.my-nav-link {
    text-decoration: underline !important;
}

.back-font {
    background-repeat: no-repeat;
    background-size: cover;
    min-height: 100vh;
    width: 100vw;
    position: relative;
}
.back-font-color {
    background: linear-gradient(#c2babd60 0%, #c2babd2f 60%);
}
</style>
