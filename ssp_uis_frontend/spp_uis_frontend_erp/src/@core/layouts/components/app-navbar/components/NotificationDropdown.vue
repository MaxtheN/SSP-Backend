<template>
   <b-nav-item-dropdown class="dropdown-notification mr-25" menu-class="dropdown-menu-media" right>
      <template #button-content>
         <feather-icon
            :badge="notificationsList.length"
            badge-classes="bg-danger"
            class="text-body"
            icon="BellIcon"
            size="21"
         />
         <b-badge pill variant="light-danger" style="transform: translate(-5px, -10px)">
            {{ ExpiredPaymentCertificateListComp.length }}
         </b-badge>
      </template>

      <!-- Header -->
      <li class="dropdown-menu-header">
         <div class="dropdown-header d-flex">
            <h4 class="notification-title mb-0 mr-auto">
               {{ $t('News') }}
            </h4>
            <b-badge pill variant="light-primary"> {{ notificationsList.length }} </b-badge>
         </div>
      </li>

      <!-- Notifications -->
      <vue-perfect-scrollbar :settings="settings" class="scrollable-container media-list scroll-area" tagname="li">
         <!-- Account Notification -->

         <!-- System Notifications -->
         <b-link v-for="notification in notificationsList" :key="notification.subtitle">
            <b-media>
               <template #aside>
                  <b-avatar
                     size="32"
                     :variant="notification.type"
                     :src="lightboxImages"
                     class="badge-minimal"
                     badge-variant="success"
                  >
                     <!-- <feather-icon :icon="notification.icon" /> -->
                  </b-avatar>
               </template>
               <p class="media-heading">
                  <span class="font-weight-bolder">
                     {{ notification.employee }}
                  </span>
               </p>
               <small class="notification-text">
                  {{
                     notification.daysUntilBirthday
                        ? $t('kundan keyin tavallud ayyomini nishonlaydi', { msg: notification.daysUntilBirthday })
                        : $t('Bugun tavallud ayyomini nishonlamoqda')
                  }}</small
               >
            </b-media>
         </b-link>

         <!-- memship notifications -->
         <b-link
            v-for="(memcertificate, index) in ExpiredPaymentCertificateListComp"
            :key="memcertificate.url"
            :href="index == 0 ? memcertificate.url : memcertificate.url + '&Less1MonthLeft=true'"
            class="d-flex justify-content-between px-1 py-1 border-bottom"
         >
            <div class="media-heading font-weight-bold">
               {{ memcertificate.message }}
            </div>
            <b-badge variant="light-danger" class="align-self-baseline"> {{ memcertificate.expiredCount }} </b-badge>
         </b-link>
         <!-- memship notifications for paid to free -->

         <!-- <div
            v-if="NotificationList.length"
            class="d-flex justify-content-between px-1 py-1 border-bottom font-weight-bold"
         >
            <b-link @click="modalShow = true">{{ $t('Yirik toifaga o’tgan tadbirkorlik subyektlari mavjud') }} </b-link>
            <b-badge variant="light-danger" class="align-self-baseline"> {{ NotificationList.length }} </b-badge>
         </div> -->
         <!-- <b-modal v-model="modalShow" size="xl">
            <h2 class="text-center">{{ $t('Yirik toifaga o’tgan tadbirkorlik subyektlari') }}</h2>

            <form-table-hrm :isPagination="false" :items="NotificationList" :fields="fields" :busy="isBusy">
            </form-table-hrm>
         </b-modal> -->
      </vue-perfect-scrollbar>

      <!-- Cart Footer -->
      <!-- <li class="dropdown-menu-footer">
         <b-button v-ripple.400="'rgba(255, 255, 255, 0.15)'" variant="primary" block>Read all notifications</b-button>
      </li> -->
   </b-nav-item-dropdown>
</template>

<script>
import { BNavItemDropdown, BBadge, BMedia, BLink, BAvatar, BButton, BFormCheckbox, BModal } from 'bootstrap-vue';
import VuePerfectScrollbar from 'vue-perfect-scrollbar';
// import FormTableHrm from '@/components/forms/form-table-hrm.vue';
import axios from 'axios';
import Ripple from 'vue-ripple-directive';
import BirthdayService from '@/services/others/birthdays.service';
import MemshipCertificateService from '@/services/document/memshipcertificate.service';

export default {
   components: {
      BNavItemDropdown,
      BBadge,
      BMedia,
      BLink,
      BAvatar,
      VuePerfectScrollbar,
      BButton,
      BFormCheckbox,
      // FormTableHrm,
      BModal
   },
   directives: {
      Ripple
   },
   data() {
      return {
         isBusy: false,
         modalShow: false,
         notificationsList: [],
         localStorageData: null,
         settings: {
            maxScrollbarLength: 60
         },
         ExpiredPaymentCertificateList: [],
         NotificationList: []
         // fields: [
         //    {
         //       key: 'id',
         //       label: this.$t('id'),
         //       thClass: 'text-center',
         //       tdClass: 'text-center',
         //       sortable: true
         //    },
         //    {
         //       key: 'docNumber',
         //       label: this.$t('docnumber'),
         //       thClass: 'text-center',
         //       tdClass: 'text-center',
         //       sortable: true
         //    },

         //    {
         //       key: 'contractorInn',
         //       label: this.$t('companyInn'),
         //       thClass: 'text-center',
         //       tdClass: 'text-center',
         //       sortable: true
         //    }
         // ]
      };
   },
   created() {
      this.getDataLocalStorage();
      // this.getEmployee();
      this.ExpiredPaymentCertificate();
      this.MemshipCertificateToPaidNotification();
   },
   computed: {
      lightboxImages() {
         return axios.defaults.baseURL + 'Person/DownloadFile/' + this.localStorageData.pictureId;
      },
      ExpiredPaymentCertificateListComp() {
         return this.ExpiredPaymentCertificateList.filter((e) => e.url);
      }
   },
   methods: {
      getDataLocalStorage() {
         const localdata = localStorage.getItem('user_info');
         this.localStorageData = JSON.parse(localdata);
      },
      getEmployee() {
         BirthdayService.GetEmployeeBirthDate({ regionId: this.localStorageData.organizationRegionId }).then((res) => {
            this.notificationsList = res.data;
         });
      },
      ExpiredPaymentCertificate() {
         if (this.$can('MemshipCertificateViewAll', 'permissions')) {
            MemshipCertificateService.ExpiredPaymentCertificate().then((res) => {
               this.ExpiredPaymentCertificateList = res.data?.deadlines || [];
            });
         }
      },
      MemshipCertificateToPaidNotification() {
         if (this.$can('MemshipCertificateViewAll', 'permissions')) {
            MemshipCertificateService.MemshipCertificateToPaidNotification().then((res) => {
               this.NotificationList = res.data.notifications;
            });
         }
      }
   }
};
</script>

<style></style>
