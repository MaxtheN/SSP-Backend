<template>
    <div class="d-flex gap-4 align-items-center">
        <div class="contractor-name" :class="{ 'mr-3': $route.path === '/' }">{{ userInfo?.contractor?.shortName }} - {{ userInfo?.contractor.director }}</div>

        <b-button variant="light" class="text-primary" v-b-tooltip.hover :title="$t('AddNewOrganization')" @click="$router.push({ name: 'info', query: { tabIndex: 1 } })">
            <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none">
                <path d="M6 12H18" stroke="#188EF6" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round" />
                <path d="M12 18V6" stroke="#188EF6" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round" />
            </svg>
        </b-button>
    </div>
</template>

<script>
export default {
    data() {
        return {
            userInfo: JSON.parse(this.$store.state.user_info)
        };
    }
};
</script>

<style>
.contractor-name {
    text-align: right;
    font-size: 14px;
    font-style: normal;
    font-weight: 500;
    line-height: normal;
    max-width: 350px;

    color: var(--Black, #000107);
    text-overflow: ellipsis;
    overflow: hidden;

    /* Required for multi-line truncation */
    display: -webkit-box;
    -webkit-box-orient: vertical;
    -webkit-line-clamp: 2;
}

@media (max-width: 767px) {
    .contractor-name {
        font-size: 10px;
    }
}
</style>
