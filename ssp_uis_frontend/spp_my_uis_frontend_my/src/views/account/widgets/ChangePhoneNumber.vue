<template>
    <span>
        <b-modal v-model="valueComp" centered hide-footer hide-header no-close-on-backdrop>
            <div class="d-flex justify-content-between align-items-center mb-2">
                <span class="modal-title">{{ $t('ChangePhoneNumber') }}</span>

                <CloseBtn @click="$emit('input', false)" />
            </div>

            <template v-if="isSmsCode">
                <LogincustomInput v-mask="'####'" v-model="smsCode" :label="$t('smskod')" placeholder="1234"></LogincustomInput>
                <b-button :disabled="saveLoading" @click="signModal = true" variant="info" class="w-100 mt-4">
                    {{ $t('confirm') }}
                </b-button>
            </template>
            <template v-else>
                <LogincustomPhoneInput
                    :label="$t('phonenumber')"
                    v-mask="'+998-##-###-##-##'"
                    v-model="phoneNumber"
                    placeholder="+998 _ _  _ _ _  _ _  _ _"
                ></LogincustomPhoneInput>

                <b-button :disabled="saveLoading" @click="SendSMSCodeForChangePhoneNumber" variant="info" class="w-100 mt-4">
                    {{ $t('Smskodloish') }}
                </b-button>
            </template>
        </b-modal>

        <b-modal v-model="signModal" centered size="md" hide-footer hide-header>
            <div style="text-align: right; margin-right: 10px; margin-top: -5px; margin-bottom: 5px; border-bottom: 1px solid lightgray">
                <span @click="signModal = false" style="cursor: pointer; font-size: 30px"> &times; </span>
            </div>
            <div>
                <just-sign @sign="ChangePhoneNumber($event)"></just-sign>
            </div>
        </b-modal>
    </span>
</template>

<script>
import AccountService from '@/services/account.service';
import JustSign from '@/components/justSign.vue';
import customPhone from '@/components/customPhoneInput.vue';
import LogincustomInput from '@/components/elements/LogincustomInput.vue';
import eimzoMixin from '@/mixins/eimzo';
import CloseBtn from '@/components/CloseBtn.vue';
import LogincustomPhoneInput from '@/components/LogincustomPhoneInput.vue';

export default {
    components: {
        customPhone,
        LogincustomInput,
        JustSign,
        CloseBtn,
        LogincustomPhoneInput
    },
    props: {
        value: {
            type: Boolean,
            default: false
        }
    },
    emits: ['input'],
    mixins: [eimzoMixin],
    data() {
        return {
            phoneNumber: null,
            isSmsCode: false,
            saveLoading: false,
            signModal: false,
            smsCode: null
        };
    },
    computed: {
        valueComp: {
            get: function () {
                return this.value;
            },
            set: function (v) {
                this.$emit('input', v);
            }
        }
    },
    methods: {
        SendSMSCodeForChangePhoneNumber() {
            this.saveLoading = true;
            AccountService.SendSMSCodeForChangePhoneNumber({
                phoneNumber: this.phoneNumber ? this.phoneNumber.replace(/\D/g, '') : ''
            })
                .then(() => {
                    this.isSmsCode = true;
                })
                .catch((error) => {
                    this.showApiError(error);
                })
                .finally(() => {
                    this.saveLoading = false;
                });
        },
        ChangePhoneNumber(data) {
            const isPinfl = this.isPinfl(data);
            AccountService.ChangePhoneNumber({
                smsCode: isPinfl ? null : this.smsCode,
                signedData: data.key,
                isPinfl: isPinfl
            })
                .then(() => {
                    this.makeToast(this.$t('SaveSuccess'), 'success');
                    this.setOrganizationModal = true;
                    this.smsCode = null;
                    this.isSmsCode = false;
                    this.signModal = false;
                })
                .catch((error) => {
                    this.showApiError(error);
                })
                .finally(() => {
                    this.SaveLoading = false;
                });
        }
    }
};
</script>
