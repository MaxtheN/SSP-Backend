<template>
   <div class="mt-2">
      <b-alert v-if="showMessage" :show.sync="currentAlertCounter">{{ alertMessage }}</b-alert>
      <div v-if="loading">
         <div
            style="position: absolute; left: 0; top: 0; bottom: 0; right: 0; background-color: rgb(255, 255, 255, 0.4)"
         >
            <div style="position: absolute; bottom: 50%; left: 50%; transform: translateX(-50%)">
               <b-spinner variant="success" />
            </div>
         </div>
      </div>
      <div v-else>
         <div v-if="keysList.length === 0">
            <b-card>
               <b-card-body>
                  <h1 class="text-center text-danger">{{ $t('no_keys') }}</h1>
               </b-card-body>
            </b-card>
         </div>

         <div v-else v-once style="height: 600px" class="scroll-area overflow-auto">
            <div
               class="my-1 border rounded p-2 card-style-key"
               v-for="(item, index) in keysList"
               :key="`signkey-${index}`"
            >
               <div>
                  <div class="ml-2 text-left">
                     <b>{{ item.CN }}</b>
                  </div>
                  <hr />

                  <div class="d-flex" style="white-space: nowrap">
                     <div class="ml-2">
                        <b>№ Сертификата</b>
                        : {{ item.serialNumber }}
                     </div>
                     <div class="ml-2 mr-2" style="margin-left: 10%; text-align: right !important">
                        <b>ИНН</b>
                        : {{ item.TIN }}
                     </div>
                  </div>
                  <div class="ml-2">
                     <b>Организация</b>
                     : {{ item.O }}
                  </div>

                  <div class="ml-2 small">
                     <b>Срок действия сертификата</b>
                     :
                     {{ getDateFormat(item.validFrom) }} -
                     {{ getDateFormat(item.validTo) }}
                  </div>
               </div>
               <div class="d-flex justify-content-end mt-2" style="width: 100%">
                  <button
                     class="button-style"
                     :style="item.expired ? ' opacity: 0.2;' : ''"
                     :disabled="item.expired"
                     @click="openSignModal(item)"
                  >
                     <!-- <span style="margin-left: 20px; margin-right: 20px; margin-top: -5px; margin-bottom: -5px"> -->
                     {{ $t('get_key') }}
                     <!-- </span> -->
                  </button>
               </div>
            </div>
         </div>
      </div>
      <b-modal
         :title="modalTitle"
         color="default"
         :centered="true"
         :show.sync="signModal"
         v-model="signModal"
         hide-header
         hide-footer
      >
         <div class="d-flex align-items-center justify-content-between" style="border-bottom: 1px solid lightgray">
            <span style="cursor: pointer; font-size: 16px">ESP</span>
            <span @click="signModal = false" style="cursor: pointer; font-size: 30px">&times;</span>
         </div>
         <b-row class="mt-3">
            <b-col>
               <h6>{{ modalTitle }}</h6>
            </b-col>
         </b-row>
         <b-row>
            <b-col class="text-right d-flex justify-content-end">
               <button class="button-style" style="margin-right: 10px" @click="hideModal('signModal')">
                  <span class="mx-4">{{ $t('no') }}</span>
               </button>
               <button class="button-style" @click="signByEimzo" :disabled="loadingButton">
                  <span class="mx-4">
                     <b-spinner style="margin-right: 10px" v-if="loadingButton" small></b-spinner>
                     {{ $t('yes') }}
                  </span>
               </button>
            </b-col>
         </b-row>
      </b-modal>
   </div>
</template>

<script>
import AppSettings from '@/plugins/settings.json';
import AccountService from '@/services/others/account.service';

import {
   BCard,
   BAlert,
   BRow,
   BCol,
   BSpinner,
   BCardHeader,
   BCardBody,
   BCardFooter,
   BModal,
   BButton
} from 'bootstrap-vue';
import VuePerfectScrollbar from 'vue-perfect-scrollbar';

var EIMZO_MAJOR = 3;
var EIMZO_MINOR = 37;
const errorCAPIWS = 'Ошибка соединения с E-IMZO. Возможно у вас не установлен модуль E-IMZO или Браузер E-IMZO.';
const errorBrowserWS = 'Браузер не поддерживает технологию WebSocket. Установите последнюю версию браузера.';
const errorUpdateApp =
   'ВНИМАНИЕ !!! Установите новую версию приложения E-IMZO или Браузера E-IMZO.<br /><a href="https://e-imzo.uz/main/downloads/" role="button">Скачать ПО E-IMZO</a>';
const errorWrongPassword = 'Пароль неверный.';
export default {
   props: {
      dataToSign: {
         default: null
      }
   },
   components: {
      BCard,
      BAlert,
      BRow,
      BCol,
      BSpinner,
      BCardHeader,
      BCardBody,
      BCardFooter,
      BModal,
      BButton,
      VuePerfectScrollbar
   },
   name: 'JustSign',
   data() {
      return {
         dataToSignIfNot: null,
         pkcs7: null,
         vo: null,
         keysList: [],
         alertMessage: null,
         showMessage: false,
         currentAlertCounter: 5,
         loading: false,
         signModal: false,
         currentItem: null,
         modalTitle: null,
         loadingButton: false,
         AppSettings: AppSettings
      };
   },
   methods: {
      appLoad() {
         const vm = this;
         vm.loading = true;
         EIMZOClient.API_KEYS = AppSettings.api_keys;
         EIMZOClient.checkVersion(
            (major, minor) => {
               var newVersion = EIMZO_MAJOR * 100 + EIMZO_MINOR;
               var installedVersion = parseInt(major) * 100 + parseInt(minor);
               if (installedVersion < newVersion) {
                  vm.loading = false;

                  vm.uiShowMessage(errorUpdateApp, vm.$t('actions.error'), 'danger');
               } else {
                  EIMZOClient.installApiKeys(
                     () => {
                        vm.loading = false;
                        vm.uiLoadKeys();
                     },
                     (e, r) => {
                        vm.loading = false;
                        if (r) {
                           vm.uiShowMessage(r, vm.$t('actions.error'), 'danger');
                        } else {
                           vm.wsError(e, vm.$t('actions.error'), 'danger');
                        }
                     }
                  );
               }
            },
            (e, r) => {
               vm.loading = false;

               if (r) {
                  vm.uiShowMessage(r, vm.$t('actions.error'), 'danger');
               } else {
                  vm.uiShowMessage(e, vm.$t('actions.error'), 'danger');
               }
            }
         );
      },
      async openSignModal(item) {
         this.signModal = true;
         this.currentItem = item;
         this.modalTitle = item.serialNumber + ' - ' + item.CN;
         await AccountService.GetChallenge().then((response) => {
            this.dataToSignIfNot = response.data.challenge;
         });
      },
      hideModal(modal) {
         this.$data[modal] = false;
      },
      async signByEimzo(data2) {
         // var data = this.dataToSignIfNot;
         this.loadingButton = true;
         const vm = this;
         // console.log(this.dataToSign)
         if (this.$route.path != '/login') {
            var data = JSON.stringify(this.dataToSign ? this.dataToSign : this.dataToSignIfNot);
         } else {
            var data = this.dataToSign ? this.dataToSign : this.dataToSignIfNot;
         }

         // console.log("-----", data);
         const promise = new Promise((resolve, reject) => {
            EIMZOClient.loadKey(
               vm.currentItem,
               (id) => {
                  EIMZOClient.createPkcs7(
                     id,
                     data,
                     null,
                     (pkcs7) => {
                        resolve(pkcs7);
                     },
                     (e, r) => {
                        reject('error');
                        vm.loadingButton = false;
                        if (r) {
                           if (r.indexOf('BadPaddingException') != -1) {
                              vm.uiShowMessage(errorWrongPassword, vm.$t('actions.error'), 'danger');
                           } else {
                              vm.uiShowMessage(r, vm.$t('actions.error'), 'danger');
                           }
                        } else {
                           document.getElementById('keyId').innerHTML = '';
                           vm.uiShowMessage(errorBrowserWS, vm.$t('actions.error'), 'danger');
                        }
                        if (e) vm.wsError(e);
                     }
                  );
               },
               (e, r) => {
                  reject('error');
                  vm.loadingButton = false;
                  if (r) {
                     if (r.indexOf('BadPaddingException') != -1) {
                        vm.uiShowMessage(errorWrongPassword, vm.$t('actions.error'), 'danger');
                     } else {
                        vm.uiShowMessage(r, vm.$t('actions.error'), 'danger');
                     }
                  } else {
                     vm.uiShowMessage(errorBrowserWS, vm.$t('actions.error'), 'danger');
                  }
                  if (e) vm.wsError(e);
               }
            );
         });
         promise
            .then(
               (success) => {
                  vm.$emit('sign', { key: success, data: this.currentItem });
               },
               (error) => {
                  vm.uiShowMessage(vm.$t('actions.messages.server_error'), vm.$t('actions.error'), 'danger');
               }
            )
            .finally(() => {
               vm.signModal = false;
               vm.loadingButton = false;
            });
      },
      getPublicSertificate() {
         console.log(this.currentItem);
         EIMZOClient.loadKey(
            this.currentItem,
            (id) => {
               CAPIWS.callFunction(
                  {
                     plugin: 'x509',
                     name: 'get_certificate_chain',
                     arguments: [
                        //Идентификатор ключа
                        id
                     ]
                  },
                  function (event, data) {
                     console.log(data);
                  },
                  function (error) {
                     window.alert(error);
                  }
               );
            },
            (e, r) => {
               reject('error');
               vm.loadingButton = false;
               if (r) {
                  if (r.indexOf('BadPaddingException') != -1) {
                     vm.uiShowMessage(errorWrongPassword, vm.$t('actions.error'), 'danger');
                  } else {
                     vm.uiShowMessage(r, vm.$t('actions.error'), 'danger');
                  }
               } else {
                  vm.uiShowMessage(errorBrowserWS, vm.$t('actions.error'), 'danger');
               }
               if (e) vm.wsError(e);
            }
         );
      },
      getCedrtificateInfo() {
         EIMZOClient.loadKey(
            this.currentItem,
            (id) => {
               CAPIWS.callFunction(
                  {
                     plugin: 'x509',
                     name: 'get_certificate_info',
                     arguments: [
                        //Сертификат в кодировке BASE64
                        window.Base.encode(id)
                     ]
                  },
                  function (event, data) {
                     console.log(data);
                  },
                  function (error) {
                     window.alert(error);
                  }
               );
            },
            (e, r) => {
               reject('error');
               vm.loadingButton = false;
               if (r) {
                  if (r.indexOf('BadPaddingException') != -1) {
                     vm.uiShowMessage(errorWrongPassword, vm.$t('actions.error'), 'danger');
                  } else {
                     vm.uiShowMessage(r, vm.$t('actions.error'), 'danger');
                  }
               } else {
                  vm.uiShowMessage(errorBrowserWS, vm.$t('actions.error'), 'danger');
               }
               if (e) vm.wsError(e);
            }
         );
      },
      getDateFormat(date) {
         const data = new Date(date);
         const month = data.getMonth() + 1;
         return (
            data.getDate().toString() +
            '.' +
            (month <= 9 ? '0' + month : month).toString() +
            '.' +
            data.getFullYear().toString()
         );
      },

      async getImzo() {
         const vm = this;
         EIMZOClient.installApiKeys(
            () => {
               vm.uiLoadKeys();
            },
            (e, r) => {
               if (r) {
                  vm.uiShowMessage(r, vm.$t('actions.error'), 'danger');
               } else {
                  vm.wsError(e);
               }
            }
         );
      },
      uiCreateItem(itmkey, vo) {
         const vm = this;
         var now = new Date();
         vo.expired = dates.compare(now, vo.validTo) > 0;
         var itm = document.createElement('option');
         itm.value = itmkey;
         itm.text = vo.CN;
         if (!vo.expired) {
         } else {
            itm.style.color = 'gray';
            itm.text += ' (срок истек)';
         }
         vm.keysList.push(vo);
         itm.setAttribute('vo', JSON.stringify(vo));
         itm.setAttribute('id', itmkey);
         return itm;
      },
      async uiLoadKeys() {
         const vm = this;
         EIMZOClient.listAllUserKeys(
            (o, i) => {
               var itemId = 'itm-' + o.serialNumber + '-' + i;
               return itemId;
            },
            (itemId, v) => {
               return vm.uiCreateItem(itemId, v);
            },
            (items, firstId) => {
               //vm.uiFillCombo(items);
               //vm.uiComboSelect(firstId);
            },
            (e, r) => {
               vm.uiShowMessage(errorCAPIWS, vm.$t('actions.error'), 'danger');
            }
         );
      },
      async uiShowMessage(message, title, type) {
         if (type == 'danger') {
            type = 'error';
         }

         if (message && message.type) {
            this.makeToast(errorCAPIWS, 'danger');
         } else {
            this.makeToast(message, 'danger');
         }

         // this.$toast.open({
         //   message: message,
         //   type: type,
         //   duration: 5000,
         //   dismissible: true
         // });
      },
      async wsError() {
         if (e) {
            this.uiShowMessage('errorCAPIWS' + ' : ' + e, this.$t('actions.error'), 'danger');
         } else {
            this.uiShowMessage('errorBrowserWS', this.$t('actions.error'), 'danger');
         }
      },
      uiComboSelect(itm) {
         if (itm) {
            var id = document.getElementById(itm);
            id.setAttribute('selected', 'true');
         }
      },

      cbChanged(c) {
         document.getElementById('keyId').innerHTML = '';
      },
      uiClearCombo() {
         var combo = document.getElementById('user_eimzo_keys');
         combo.length = 0;
      },

      uiFillCombo(items) {
         var combo = document.getElementById('user_eimzo_keys');
         this.itemList = items;
      }
   },
   async created() {
      this.loading = true;
      this.appLoad();
      this.loading = false;
   }
};
</script>

<style lang="scss">
.card-style-key {
   box-shadow: rgba(99, 99, 99, 0.2) 0px 2px 8px 0px;
}
.button-style {
   margin-right: 12px;
   background-color: #2a82c4;
   border: none;
   cursor: pointer;
   text-align: center;
   padding: 5px;
   font-size: 14px;
   color: #fff;
   border-radius: 4px;
   width: 50%;
}
</style>
