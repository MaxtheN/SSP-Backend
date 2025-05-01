<template>
    <div>
        <AppListHeaderForName title="settings" page-name="MyCabinet" />

        <div class="container-fluid">
            <b-overlay :show="getLoading" spinner-variant="info" rounded="sm" class="w-full">
                <div class="border rounded-xl bg-white p-md-3">
                    <table class="settings-table">
                        <tbody>
                            <tr>
                                <td>{{ $t('phone') }}:</td>
                                <td>{{ userInfo.userName }}</td>
                                <td>
                                    <b-button variant="light" class="text-black" @click="ChangePhoneNumberModal = true">
                                        <BIconPencil />
                                        <span class="d-none d-sm-block">{{ $t('edit') }} </span>
                                    </b-button>
                                </td>
                            </tr>
                            <tr>
                                <td>{{ $t('Parol') }}:</td>
                                <td>***********</td>
                                <td>
                                    <b-button variant="light" class="text-black" @click="forgotPasswordModal = true">
                                        <BIconPencil />
                                        <span class="d-none d-sm-block">{{ $t('edit') }} </span>
                                    </b-button>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </b-overlay>
        </div>

        <!-- change phone number -->
        <ChangePhoneNumber v-model="ChangePhoneNumberModal" />

        <!-- forgot pasword -->
        <b-modal v-model="forgotPasswordModal" hide-footer hide-header no-close-on-backdrop centered>
            <div class="d-flex justify-content-between align-items-center mb-2">
                <span class="modal-title">{{ $t('restore') }}</span>
                <CloseBtn @click="forgotPasswordModal = false" />
            </div>
            <div class="gap-4 d-flex flex-column">
                <LogincustomPhoneInput
                    disabled
                    :label="$t('phonenumber')"
                    v-mask="'+###-##-###-##-##'"
                    v-model="Restore.userName"
                    placeholder="+998 _ _  _ _ _  _ _  _ _"
                ></LogincustomPhoneInput>

                <template v-if="Restore.isRestore">
                    <p class="p-2">{{ $t('smssent', { phonenumber: Restore.userName }) }}</p>

                    <LogincustomInput type="password" :placeholder="$t('*******')" v-model="Restore.newPassword" :label="$t('password')"></LogincustomInput>

                    <LogincustomInput
                        class="mt-2"
                        type="password"
                        :placeholder="$t('*******')"
                        v-model="Restore.confirmedPassword"
                        :label="$t('confirmpassword')"
                    ></LogincustomInput>

                    <LogincustomInput
                        class="mt-2"
                        v-model="Restore.smsCode"
                        :placeholder="$t('0000')"
                        :label="$t('smskod')"
                        @keyup.native.enter="RestorePasswordConfirm"
                    ></LogincustomInput>
                </template>
            </div>

            <div class="mt-3">
                <b-button v-if="Restore.isRestore" @click="RestorePasswordConfirm" block variant="info">
                    <b-spinner v-if="restoreLoading" small class="mr-2"></b-spinner>
                    {{ $t('restore') }}
                </b-button>
                <b-button v-else @click="RestorePassword" :disabled="!Restore.userName" block variant="info">
                    <b-spinner v-if="restoreLoading" small class="mr-2"></b-spinner> {{ $t('Smskodloish') }}
                </b-button>
            </div>
        </b-modal>
    </div>
</template>

<script>
import AppListHeaderForName from '@/components/application/AppListHeaderForName.vue';
import LogincustomInput from '../components/elements/LogincustomInput.vue';
import AccountService from '@/services/account.service';
import { BIconPencil } from 'bootstrap-vue';
import ChangePhoneNumber from '@/views/account/widgets/ChangePhoneNumber.vue';
import CloseBtn from '@/components/CloseBtn.vue';
import LogincustomPhoneInput from '@/components/LogincustomPhoneInput.vue';

const RestoreDef = {
    userName: '',
    isRestore: false,
    smsCode: '',
    newPassword: '',
    confirmedPassword: ''
};

export default {
    components: {
        AppListHeaderForName,
        LogincustomInput,
        LogincustomPhoneInput,
        BIconPencil,
        ChangePhoneNumber,
        CloseBtn
    },
    data() {
        return {
            getLoading: false,
            restoreLoading: false,
            forgotPasswordModal: false,
            ChangePhoneNumberModal: false,
            userInfo: {},
            Restore: {
                ...RestoreDef
            }
        };
    },
    created() {
        this.GetSettings();
    },
    methods: {
        GetSettings() {
            this.getLoading = true;
            AccountService.GetUserInfo()
                .then((res) => {
                    this.userInfo = res.data;
                    this.Restore.userName = this.userInfo.userName;
                })
                .catch((error) => {
                    this.showApiError(error);
                })
                .finally(() => {
                    this.getLoading = false;
                });
        },
        RestorePasswordConfirm() {
            if (this.Restore.confirmedPassword && this.Restore.confirmedPassword == this.Restore.newPassword) {
                this.restoreLoading = true;
                AccountService.RestorePasswordConfirm({ ...this.Restore, userName: this.Restore.userName ? this.Restore.userName.replace(/\D/g, '') : '' })
                    .then((res) => {
                        this.forgotPasswordModal = false;
                        this.makeToast(this.$t('PasswordChangedSuccess'), 'success');
                        this.Restore = { ...RestoreDef };
                    })
                    .catch((error) => {
                        this.showApiError(error);
                    })
                    .finally(() => {
                        this.restoreLoading = false;
                    });
            } else {
                this.makeToast(this.$t('passwordNotEquals'), 'error');
            }
        },
        RestorePassword() {
            this.restoreLoading = true;
            AccountService.RestorePassword({ ...this.Restore, userName: this.Restore.userName ? this.Restore.userName.replace(/\D/g, '') : '' })
                .then((res) => {
                    this.Restore.isRestore = true;
                    this.restoreLoading = false;
                })
                .catch((error) => {
                    this.showApiError(error);
                    this.restoreLoading = false;
                });
        }
    }
};
</script>

<style lang="scss">
.settings-table {
    width: 100%;
    td {
        color: var(--Black, #000107);
        font-family: 'Museo Sans', serif;
        font-size: 18px;
        font-style: normal;
        font-weight: 600;
        line-height: normal;
        padding: 16px 6px;
    }

    tr:not(:last-child) {
        td {
            border-bottom: 1px solid var(--Light-gray, #d5d7e1);
        }
    }
}
</style>
