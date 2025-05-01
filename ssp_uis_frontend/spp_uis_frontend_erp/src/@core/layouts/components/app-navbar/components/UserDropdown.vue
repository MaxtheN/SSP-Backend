<template>
   <b-nav-item-dropdown right toggle-class="d-flex align-items-center dropdown-user-link" class="dropdown-user">
      <template #button-content>
         <div class="d-sm-flex d-none user-nav">
            <span class="user-status">{{ userdisplayname }}</span>
         </div>
         <b-avatar
            :src="lightboxImages"
            size="40"
            variant="light-primary"
            badge
            class="badge-minimal"
            badge-variant="success"
         >
            <feather-icon v-if="!localStoreData.pictureId" icon="UserIcon" size="22" />
         </b-avatar>
      </template>

      <b-dropdown-item link-class="d-flex align-items-center">
         <feather-icon size="16" icon="UserIcon" class="mr-50" />
         <span>
            {{ username }}
         </span>
      </b-dropdown-item>
      <b-dropdown-item link-class="d-flex align-items-center">
         <feather-icon size="16" icon="PhoneIcon" class="mr-50" />
         <span>{{ phoneNumber }}</span>
      </b-dropdown-item>
      <b-dropdown-item link-class="d-flex align-items-center">
         <feather-icon size="16" icon="SlidersIcon" class="mr-50" />
         <span>{{ role }}</span>
      </b-dropdown-item>
      <!-- <b-dropdown-item :to="{ name: 'apps-chat' }" link-class="d-flex align-items-center">
         <feather-icon size="16" icon="MessageSquareIcon" class="mr-50" />
         <span>Chat</span>
      </b-dropdown-item>

      <b-dropdown-divider />

      <b-dropdown-item :to="{ name: 'pages-account-setting' }" link-class="d-flex align-items-center">
         <feather-icon size="16" icon="SettingsIcon" class="mr-50" />
         <span>Settings</span>
      </b-dropdown-item> -->
      <!-- <b-dropdown-item
      :to="{ name: 'pages-pricing' }"
      link-class="d-flex align-items-center"
    >
      <feather-icon
        size="16"
        icon="CreditCardIcon"
        class="mr-50"
      />
      <span>Pricing</span>
    </b-dropdown-item>-->
      <!-- <b-dropdown-item
      :to="{ name: 'pages-faq' }"
      link-class="d-flex align-items-center"
    >
      <feather-icon
        size="16"
        icon="HelpCircleIcon"
        class="mr-50"
      />
      <span>FAQ</span>
    </b-dropdown-item>-->
      <b-dropdown-item link-class="d-flex align-items-center" @click="logout">
         <feather-icon size="16" icon="LogOutIcon" class="mr-50" />
         <span>{{ $t('Logout') }}</span>
      </b-dropdown-item>
   </b-nav-item-dropdown>
</template>

<script>
import axios from 'axios';
import { BNavItemDropdown, BDropdownItem, BDropdownDivider, BAvatar } from 'bootstrap-vue';
import { initialAbility } from '@/libs/acl/config';
import { avatarText } from '@core/utils/filter';
import Cookies from 'js-cookie';
export default {
   components: {
      BNavItemDropdown,
      BDropdownItem,
      BDropdownDivider,
      BAvatar,
      axios
   },
   data() {
      return {
         userData: JSON.parse(localStorage.getItem('userData')),
         avatarText,
         localStoreData: ''
      };
   },
   computed: {
      userdisplayname() {
         return localStorage.getItem('user_info') ? JSON.parse(localStorage.getItem('user_info')).UserDisplayName : '';
      },
      username() {
         return localStorage.getItem('user_info') ? JSON.parse(localStorage.getItem('user_info')).userName : '';
      },
      phoneNumber() {
         return localStorage.getItem('user_info') ? JSON.parse(localStorage.getItem('user_info')).phoneNumber : '';
      },
      role() {
         return localStorage.getItem('user_info') ? JSON.parse(localStorage.getItem('user_info')).roles.join(',') : '';
      },

      lightboxImages() {
         return axios.defaults.baseURL + 'Person/DownloadFile/' + this.localStoreData.pictureId;
      }
   },

   created() {
      this.localStoreData = JSON.parse(localStorage.getItem('user_info'));
   },

   methods: {
      logout() {
         // Remove userData from localStorage
         Cookies.remove('auth_token');
         localStorage.removeItem('auth_token');

         localStorage.clear();
         // Reset ability
         this.$ability.update(initialAbility);

         // Redirect to login page
         this.$router.push({ name: 'auth-login' });
      }
   }
};
</script>
