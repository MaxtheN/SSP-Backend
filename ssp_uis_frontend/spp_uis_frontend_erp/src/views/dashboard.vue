<template>
   <div></div>
</template>

<script>
import AccountService from '@/services/others/account.service';
export default {
   mounted() {
      AccountService.GetUserInfo().catch(() => {
         localStorage.clear();
         this.$router.push('/login');
      });
   }
};
</script>
