<template>
    <b-button variant="link" class="cabinet-menu__logout" @click="logout()">
        <svg class="flex-grow-0" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24"
            fill="none">
            <path
                d="M8.90002 7.55999C9.21002 3.95999 11.06 2.48999 15.11 2.48999H15.24C19.71 2.48999 21.5 4.27999 21.5 8.74999V15.27C21.5 19.74 19.71 21.53 15.24 21.53H15.11C11.09 21.53 9.24002 20.08 8.91002 16.54"
                stroke="#FF4E54" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round" />
            <path d="M15 12H3.62" stroke="#FF4E54" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round" />
            <path d="M5.85 8.64999L2.5 12L5.85 15.35" stroke="#FF4E54" stroke-width="1.5" stroke-linecap="round"
                stroke-linejoin="round" />
        </svg>
        <div class=" text-danger">{{ $t('logout') }}</div>
    </b-button>
</template>

<script>
import AccountService from '@/services/account.service';

export default {
    methods: {
        logout() {
            AccountService.Logout().then(() => {
                localStorage.clear();
                if (this.$route.name !== 'Home') {
                    this.$router.push('/');
                } else {
                    window.location.reload();
                }
            });
        }
    }
}

</script>

<style lang="scss">
.cabinet-menu__logout {
    width: 100%;
    display: flex;
    padding: 8px 24px;
    margin: 8px 0;
    gap: 10px;
    text-decoration: none !important;
    outline: none;
}
</style>