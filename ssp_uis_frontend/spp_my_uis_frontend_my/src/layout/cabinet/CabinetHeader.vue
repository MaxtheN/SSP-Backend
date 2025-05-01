<template>
    <b-navbar class="border-bottom md-px-4 px-3 text-black bg-white" sticky type="light" style="height: 72px">
        <b-navbar-brand href="#" class="d-md-block d-none">
            <b-img src="/img/logo.png" alt="logo" />
            <span class="ml-1 text-uppercase h6 text-black font-weight-medium">O’zbekiston savdo-sanoat palatasi</span>
        </b-navbar-brand>

        <div class="ml-auto gap-4 d-flex">
            <OrganizationBtn />
            <NotificationBtn />

            <LanguageSwitcher v-if="!isMobile" />
            <ProfileBtn v-if="!isMobile" />
        </div>
    </b-navbar>
</template>

<script>
import LanguageSwitcher from './header/LanguageSwitcher.vue';
import ProfileBtn from './header/ProfileBtn.vue';
import OrganizationBtn from './header/OrganizationBtn.vue';
import NotificationBtn from './header/NotificationBtn.vue';
import isMobileMixin from '@/mixins/isMobileMixin';

export default {
    components: {
        LanguageSwitcher,
        ProfileBtn,
        OrganizationBtn,
        NotificationBtn
    },
    mixins: [isMobileMixin]
};
</script>
