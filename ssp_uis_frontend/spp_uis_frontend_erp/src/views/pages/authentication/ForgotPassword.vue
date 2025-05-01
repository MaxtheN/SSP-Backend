<template>
   <div class="auth-wrapper auth-v2" style="position: relative">
      <div>
         <div class="d-flex images-login">
            <img v-if="login.languageId == 1" width="200px" src="@/assets/images/SSP_RU.png" alt="" />
            <img v-if="login.languageId == 3" width="200px" src="@/assets/images/SSP_UZB.png" alt="" />
            <img v-if="login.languageId == 2" width="200px" src="@/assets/images/SSP_UZB.png" alt="" />
         </div>
         <div class="language-login">
            <locale :is-login="true" @getLanguageId="getLanguageId" />
         </div>
      </div>
      <b-row class="auth-inner m-0">
         <!-- Brand logo-->
         <b-link class="brand-logo">
            <h2 class="brand-text text-primary ml-1" style="width: 520px">{{ $t('appName') }}</h2>
         </b-link>
         <!-- /Brand logo-->

         <!-- Left Text-->
         <b-col lg="8" class="d-none d-lg-flex align-items-center p-5" style="background-color: #f3f4f9">
            <div class="w-100 d-lg-flex align-items-center justify-content-center px-5">
               <img src="@/assets/images/bg.jpg" alt="bg" />
            </div>
         </b-col>
         <!-- /Left Text-->

         <!-- Forgot password-->
         <b-col lg="4" class="d-flex align-items-center auth-bg px-2 p-lg-5">
            <b-col sm="8" md="6" lg="12" class="px-xl-2 mx-auto">
               <b-card-title class="mb-1"> {{ $t('forget-password') }} 🔒 </b-card-title>

               <!-- form -->
               <template v-if="!isSend">
                  <validation-observer ref="smsRules">
                     <b-form class="auth-forgot-password-form mt-2" @submit.prevent="validationForm">
                        <b-form-group :label="$t('username')" label-for="username">
                           <validation-provider #default="{ errors }" name="username" rules="required">
                              <b-form-input
                                 id="username"
                                 v-model="login.username"
                                 :state="errors.length > 0 ? false : null"
                                 name="username"
                                 :placeholder="$t('username')"
                              />
                              <small class="text-danger">{{ errors[0] }}</small>
                           </validation-provider>
                        </b-form-group>
                        <b-button type="submit" variant="primary" block @click="sendCode" :disabled="Loading">
                           <b-spinner v-if="Loading" small></b-spinner>
                           {{ $t('Send') }}
                        </b-button>
                     </b-form>
                  </validation-observer>
               </template>
               <template v-else>
                  <validation-observer ref="simpleRules">
                     <b-form class="auth-forgot-password-form mt-2" @submit.prevent="validationForm">
                        <b-alert variant="success" show class="px-2 py-1">
                           {{ $t('username') }} : <b>{{ login.username }}</b>
                        </b-alert>
                        <b-form-group :label="$t('smsCode')" label-for="smsCode">
                           <validation-provider #default="{ errors }" name="smsCode" rules="required">
                              <b-form-input
                                 id="smsCode"
                                 v-model="login.smsCode"
                                 :state="errors.length > 0 ? false : null"
                                 name="smsCode"
                                 :placeholder="$t('smsCode')"
                              />
                              <small class="text-danger">{{ errors[0] }}</small>
                           </validation-provider>
                        </b-form-group>
                        <b-form-group :label="$t('newPassword')" label-for="newPassword">
                           <validation-provider #default="{ errors }" name="newPassword" rules="required|min:6">
                              <b-form-input
                                 id="newPassword"
                                 v-model="login.newPassword"
                                 :state="errors.length > 0 ? false : null"
                                 name="newPassword"
                                 :placeholder="$t('newPassword')"
                              />
                              <small class="text-danger">{{ errors[0] }}</small>
                           </validation-provider>
                        </b-form-group>
                        <b-form-group :label="$t('confirmNewPassword')" label-for="confirmedNewPassword">
                           <validation-provider
                              #default="{ errors }"
                              name="confirmedNewPassword"
                              rules="required|min:6|confirmed:newPassword"
                           >
                              <b-form-input
                                 id="confirmedNewPassword"
                                 v-model="login.confirmedNewPassword"
                                 :state="errors.length > 0 ? false : null"
                                 name="confirmedNewPassword"
                                 :placeholder="$t('confirmNewPassword')"
                              />
                              <small class="text-danger">{{ errors[0] }}</small>
                           </validation-provider>
                        </b-form-group>

                        <b-button type="submit" variant="primary" block @click="validationForm" :disabled="Loading">
                           <b-spinner v-if="Loading" small></b-spinner>
                           {{ $t('Save') }}
                        </b-button>
                     </b-form>
                  </validation-observer>
               </template>

               <p class="text-center mt-2">
                  <b-link :to="{ name: 'auth-login' }">
                     <feather-icon icon="ChevronLeftIcon" /> {{ $t('backToLogin') }}
                  </b-link>
               </p>
            </b-col>
         </b-col>
         <!-- /Forgot password-->
      </b-row>
   </div>
</template>

<script>
import { ValidationProvider, ValidationObserver } from 'vee-validate';
import { required, email } from '@validations';
import Locale from '@core/layouts/components/app-navbar/components/Locale.vue';
import {
   BSpinner,
   BRow,
   BCol,
   BLink,
   BFormGroup,
   BFormInput,
   BInputGroupAppend,
   BInputGroup,
   BFormCheckbox,
   BCardText,
   BCardTitle,
   BImg,
   BForm,
   BButton,
   BModal,
   BAlert
} from 'bootstrap-vue';
import AccountService from '@/services/others/account.service';

export default {
   components: {
      ValidationProvider,
      ValidationObserver,
      Locale,
      BSpinner,
      BRow,
      BCol,
      BLink,
      BFormGroup,
      BFormInput,
      BInputGroupAppend,
      BInputGroup,
      BFormCheckbox,
      BCardText,
      BCardTitle,
      BImg,
      BForm,
      BButton,
      BModal,
      BAlert
   },
   data() {
      return {
         userEmail: '',
         required,
         email,
         login: {
            appKeyHash: '',
            username: '',
            smsCode: '',
            newPassword: '',
            confirmedNewPassword: '',
            languageId: 3
         },
         Loading: false,
         isSend: false
      };
   },
   methods: {
      getLanguageId(id) {
         this.login.languageId = id;
      },
      sendCode() {
         this.$refs.smsRules.validate().then((success) => {
            if (success) {
               this.Loading = true;
               AccountService.RecoverPassword({
                  username: this.login.username,
                  appKeyHash: ''
               })
                  .then(() => {
                     this.isSend = true;
                  })
                  .catch((error) => {
                     this.showApiError(error);
                  })
                  .finally(() => {
                     this.Loading = false;
                  });
            }
         });
      },
      validationForm() {
         this.$refs.simpleRules.validate().then((success) => {
            if (success) {
               this.Loading = true;
               AccountService.RecoveredPassword(this.login)
                  .then(() => {
                     this.makeToast(this.$t('SuccessSave'), 'success');
                     this.$router.push({ name: 'auth-login' });
                  })
                  .catch((error) => {
                     this.Loading = false;
                     this.showApiError(error);
                  });
            }
         });
      }
   }
};
</script>

<style lang="scss">
@import '@core/scss/vue/pages/page-auth.scss';
.language-login {
   position: absolute;
   z-index: 5;
   right: 4rem;
   li {
      top: 2rem;
      list-style: none;
   }
}
.images-login {
   position: absolute;
   z-index: 1;
   top: 1rem;
   right: 20vw;
   li {
      top: 2rem;
      list-style: none;
   }
}
</style>
