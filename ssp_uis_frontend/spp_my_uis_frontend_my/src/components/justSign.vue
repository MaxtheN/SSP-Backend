<template>
    <div>
        <b-alert v-if="showMessage" :show.sync="currentAlertCounter">
            {{ alertMessage }}
        </b-alert>
        <div v-if="loading">
            <div style="position: absolute; left: 0; top: 0; bottom: 0; right: 0; background-color: rgb(255, 255, 255, 0.4)">
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
            <div v-else style="overflow-y: scroll; overflow-x: hidden; height: 800px; margin-right: 5px">
                <b-row>
                    <b-col sm="12" md="12" lg="12" v-for="(item, index) in keysList" :key="`signkey-${index}`">
                        <div class="mr-3 mb-3 border rounded p-3 card-style-key" style="margin-right: 5px">
                            <div>
                                <div class="ml-2 text-center">
                                    <b> {{ item.CN }}</b>
                                </div>
                                <hr />

                                <div class="d-flex" style="white-space: nowrap">
                                    <div class="ml-2"><b>№ Сертификата</b>: {{ item.serialNumber }}</div>
                                    <div class="ml-2 mr-2" style="margin-left: 10%; text-align: right !important"><b>ИНН</b>: {{ item.TIN }}</div>
                                </div>
                                <div class="ml-2"><b>Организация</b>: {{ item.O }}</div>

                                <div class="ml-2 small">
                                    <b>Срок действия сертификата</b>: {{ getDateFormat(item.validFrom) }} -
                                    {{ getDateFormat(item.validTo) }}
                                </div>
                            </div>
                            <div class="d-flex justify-content-end mt-2" style="width: 100%">
                                <button :style="item.expired ? ' opacity: 0.2;' : ''" class="button-style" :disabled="item.expired" @click="openSignModal(item)">
                                    {{ $t('get_key') }}
                                </button>
                            </div>
                        </div>
                    </b-col>
                </b-row>
            </div>
        </div>
        <b-modal :title="modalTitle" color="default" :centered="true" :show.sync="signModal" v-model="signModal" hide-header hide-footer>
            <div class="d-flex align-items-center justify-content-between" style="border-bottom: 1px solid lightgray">
                <span style="cursor: pointer; font-size: 16px"> ESP </span>
                <span @click="signModal = false" style="cursor: pointer; font-size: 30px"> &times; </span>
            </div>
            <b-row class="mt-3">
                <b-col>
                    <h6>{{ modalTitle }}</h6>
                </b-col>
            </b-row>
            <b-row>
                <b-col class="text-right d-flex justify-content-end">
                    <button class="button-style" style="margin-right: 10px" @click="hideModal('signModal')">
                        <span class="mx-4"> {{ $t('no') }} </span>
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
import AppSettings from '../components/settings.json';
import customButton from '../components/elements/customButton.vue';
import AccountService from '@/services/account.service';

var EIMZO_MAJOR = 3;
var EIMZO_MINOR = 37;
const errorCAPIWS = 'Ошибка соединения с E-IMZO. Возможно у вас не установлен модуль E-IMZO или Браузер E-IMZO.';
const errorBrowserWS = 'Браузер не поддерживает технологию WebSocket. Установите последнюю версию браузера.';
const errorUpdateApp =
    'ВНИМАНИЕ !!! Установите новую версию приложения E-IMZO или Браузера E-IMZO.<br /><a href="https://e-imzo.uz/main/downloads/" role="button">Скачать ПО E-IMZO</a>';
const errorWrongPassword = 'Пароль неверный.';
export default {
    props: {
        // eslint-disable-next-line vue/require-prop-types
        dataToSign: {
            default: null
        }
    },
    components: {
        customButton
    },
    name: 'JustSign',
    data() {
        return {
            dataToSignIfNot: null,
            pkcs7: null,
            signByImzoText: null,
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
            let vm = this;
            vm.loading = true;
            EIMZOClient.API_KEYS = AppSettings.api_keys;
            EIMZOClient.checkVersion(
                function (major, minor) {
                    var newVersion = EIMZO_MAJOR * 100 + EIMZO_MINOR;
                    var installedVersion = parseInt(major) * 100 + parseInt(minor);
                    if (installedVersion < newVersion) {
                        vm.loading = false;

                        vm.uiShowMessage(errorUpdateApp, vm.$t('actions.error'), 'danger');
                    } else {
                        EIMZOClient.installApiKeys(
                            function () {
                                vm.loading = false;
                                vm.uiLoadKeys();
                            },
                            function (e, r) {
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
                function (e, r) {
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
            if (!this.dataToSign) {
                try {
                    await AccountService.GetChallenge().then((response) => {
                        if (response.data.challenge) {
                            this.dataToSignIfNot = response.data.challenge;
                        }
                    });
                } catch (error) {
                    console.log(error);
                }
            }
        },
        hideModal(modal) {
            this.$data[modal] = false;
        },
        // async timestamper(signature_hex, callback, fail) {

        // 	try {
        // 		await HelperService.getTimestamper(signature_hex).then(response => {
        // 			callback(response.data.data)
        // 		});
        // 	} catch (e) {

        // 		fail(e.request.response);
        // 	}

        // },
        async signByEimzo(data2) {
            this.loadingButton = true;
            let vm = this;
            var data = this.dataToSign ? JSON.stringify(this.dataToSign) : this.dataToSignIfNot || 'test';
            console.log(data);
            const promise = new Promise((resolve, reject) => {
                EIMZOClient.loadKey(
                    vm.currentItem,
                    function (id) {
                        EIMZOClient.createPkcs7(
                            id,
                            data,
                            '',
                            function (pkcs7) {
                                resolve(pkcs7);
                            },
                            function (e, r) {
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
                    function (e, r) {
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
        getDateFormat(date) {
            let data = new Date(date);
            let month = data.getMonth() + 1;
            return data.getDate().toString() + '.' + (month <= 9 ? '0' + month : month).toString() + '.' + data.getFullYear().toString();
        },

        async getImzo() {
            let vm = this;
            EIMZOClient.installApiKeys(
                function () {
                    vm.uiLoadKeys();
                },
                function (e, r) {
                    if (r) {
                        vm.uiShowMessage(r, vm.$t('actions.error'), 'danger');
                    } else {
                        vm.wsError(e);
                    }
                }
            );
        },
        uiCreateItem(itmkey, vo) {
            let vm = this;
            var now = new Date();
            vo.expired = dates?.compare(now, vo.validTo) > 0;
            var itm = document.createElement('option');
            itm.value = itmkey;
            itm.text = vo.CN;
            if (!vo.expired) {
            } else {
                itm.style.color = 'gray';
                itm.text = itm.text + ' (срок истек)';
            }
            vm.keysList.push(vo);
            itm.setAttribute('vo', JSON.stringify(vo));
            itm.setAttribute('id', itmkey);
            return itm;
        },
        async uiLoadKeys() {
            let vm = this;
            console.log(EIMZOClient);
            EIMZOClient.listAllUserKeys(
                function (o, i) {
                    var itemId = 'itm-' + o.serialNumber + '-' + i;
                    return itemId;
                },
                function (itemId, v) {
                    return vm.uiCreateItem(itemId, v);
                },
                function (items, firstId) {
                    //vm.uiFillCombo(items);
                    //vm.uiComboSelect(firstId);
                },
                function (e, r) {
                    vm.uiShowMessage(errorCAPIWS, vm.$t('actions.error'), 'danger');
                }
            );
        },
        makeToast(message, type) {
            this.$toast.open({
                message: message,
                type: type,
                duration: 5000,
                dismissible: true
            });
        },
        async uiShowMessage(message, title, type) {
            if (type == 'danger') {
                type = 'error';
            }
            this.$toast.open({
                message: message,
                type: type,
                duration: 5000,
                dismissible: true
            });
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
        },
        loadScript(src) {
            return new Promise((resolve, reject) => {
                const script = document.createElement('script');
                script.src = src;
                script.onload = resolve;
                script.onerror = reject;
                document.head.appendChild(script);
            });
        }
    },
    async mounted() {
        this.loading = true;
        if (window.EIMZOClient) {
            this.appLoad();
            this.loading = false;
        } else {
            await Promise.all([
                this.loadScript('/js/e-imzo.js'), // Replace with the actual script URLs
                this.loadScript('/js/e-imzo-client.js')
            ])
                .then(() => {
                    this.appLoad();
                })
                .catch((error) => {
                    console.error('Error loading scripts:', error);
                })
                .finally(() => {
                    this.loading = false;
                });
        }
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
