<template>
    <b-card class="myinfo_profile">
        <b-row>
            <b-col
                ><h6>{{ $t('personaldata') }}</h6></b-col
            >
            <hr />
        </b-row>
        <b-row>
            <b-col>
                <div class="d-flex gap-5 align-items-center header">
                    <b-avatar size="100"></b-avatar>
                    <div>
                        <p>We only support .JPG, .JPEG, or .PNG file</p>
                        <div class="d-flex gap-2">
                            <b-button variant="light" class="border-info px-4" size="sm">
                                <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none">
                                    <path
                                        d="M13.2601 3.6L5.0501 12.29C4.7401 12.62 4.4401 13.27 4.3801 13.72L4.0101 16.96C3.8801 18.13 4.7201 18.93 5.8801 18.73L9.1001 18.18C9.5501 18.1 10.1801 17.77 10.4901 17.43L18.7001 8.74C20.1201 7.24 20.7601 5.53 18.5501 3.44C16.3501 1.37 14.6801 2.1 13.2601 3.6Z"
                                        stroke="#188EF6"
                                        stroke-width="1.5"
                                        stroke-miterlimit="10"
                                        stroke-linecap="round"
                                        stroke-linejoin="round"
                                    />
                                    <path
                                        d="M11.8899 5.05C12.3199 7.81 14.5599 9.92 17.3399 10.2"
                                        stroke="#188EF6"
                                        stroke-width="1.5"
                                        stroke-miterlimit="10"
                                        stroke-linecap="round"
                                        stroke-linejoin="round"
                                    />
                                    <path d="M3 22H21" stroke="#188EF6" stroke-width="1.5" stroke-miterlimit="10" stroke-linecap="round" stroke-linejoin="round" />
                                </svg>
                                <span style="color: #188ef6">{{ $t('logout') }}</span>
                            </b-button>
                            <div class="d-flex justify-content-start">
                                <b-button variant="light" class="border-danger px-4" size="sm">
                                    <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none">
                                        <path
                                            d="M13.2601 3.6L5.0501 12.29C4.7401 12.62 4.4401 13.27 4.3801 13.72L4.0101 16.96C3.8801 18.13 4.7201 18.93 5.8801 18.73L9.1001 18.18C9.5501 18.1 10.1801 17.77 10.4901 17.43L18.7001 8.74C20.1201 7.24 20.7601 5.53 18.5501 3.44C16.3501 1.37 14.6801 2.1 13.2601 3.6Z"
                                            stroke="#FF4E54"
                                            stroke-width="1.5"
                                            stroke-miterlimit="10"
                                            stroke-linecap="round"
                                            stroke-linejoin="round"
                                        />
                                        <path
                                            d="M11.8899 5.05C12.3199 7.81 14.5599 9.92 17.3399 10.2"
                                            stroke="#FF4E54"
                                            stroke-width="1.5"
                                            stroke-miterlimit="10"
                                            stroke-linecap="round"
                                            stroke-linejoin="round"
                                        />
                                        <path d="M3 22H21" stroke="#FF4E54" stroke-width="1.5" stroke-miterlimit="10" stroke-linecap="round" stroke-linejoin="round" />
                                    </svg>
                                    <span style="color: #ff4e54">{{ $t('delete') }}</span>
                                </b-button>
                            </div>
                        </div>
                    </div>
                </div>
            </b-col>
        </b-row>
        <b-row class="mt-6">
            <b-col sm="12" md="4">
                <WInput :label="$t('inn')" disabled v-model="Parent.inn" />
            </b-col>
            <b-col sm="12" md="4">
                <WInput rules="required" :label="$t('registrationnumber')" disabled v-model="Parent.registrationNumber" />
            </b-col>
            <b-col sm="12" md="4">
                <WInput rules="required" :label="$t('registrationDate')" disabled v-model="Parent.registrationDate" />
            </b-col>
        </b-row>
        <b-row style="margin-top: 16px">
            <b-col sm="12" md="4">
                <WInput :label="$t('shortname')" disabled v-model="Parent.shortName" />
            </b-col>
            <b-col sm="12" md="4">
                <WInput rules="required" :label="$t('fullname')" disabled v-model="Parent.fullName" />
            </b-col>
            <b-col sm="12" md="4">
                <WInput rules="required" :label="$t('supervisor')" disabled v-model="Parent.director" />
            </b-col>
        </b-row>
        <b-row style="margin-top: 16px">
            <b-col sm="12" md="4">
                <WInput rules="required" :label="$t('oked')" disabled v-model="Parent.oked" />
            </b-col>

            <b-col sm="12" md="4">
                <WInput rules="required" :label="$t('country')" disabled v-model="Parent.country" />
            </b-col>
            <b-col sm="12" md="4">
                <WInput rules="required" :label="$t('region')" disabled v-model="Parent.region" />
            </b-col>
        </b-row>
        <b-row style="margin-top: 16px">
            <b-col sm="12" md="4">
                <WInput rules="required" :label="$t('district')" disabled v-model="Parent.district" />
            </b-col>
            <b-col sm="12" md="4">
                <WInput rules="required" :label="$t('address')" disabled v-model="Parent.address" />
            </b-col>
            <b-col sm="12" md="4">
                <WInput rules="required" :label="$t('bank')" disabled v-model="Parent.bank" />
            </b-col>
        </b-row>
        <b-row style="margin-top: 16px">
            <b-col sm="12" md="4">
                <WInput rules="required" :label="$t('pinfl')" disabled v-model="Data.pinfl" />
            </b-col>
            <b-col sm="12" md="4">
                <WInput rules="required" :label="$t('userName')" disabled v-model="Data.userName" />
            </b-col>
        </b-row>
        <hr />
        <b-row style="margin-top: 16px">
            <b-col cols="12">
                <div class="d-flex gap-2 justify-content-end">
                    <b-button variant="light" class="border-secondary px-4" size="sm">
                        <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none">
                            <path
                                d="M13.2601 3.6L5.0501 12.29C4.7401 12.62 4.4401 13.27 4.3801 13.72L4.0101 16.96C3.8801 18.13 4.7201 18.93 5.8801 18.73L9.1001 18.18C9.5501 18.1 10.1801 17.77 10.4901 17.43L18.7001 8.74C20.1201 7.24 20.7601 5.53 18.5501 3.44C16.3501 1.37 14.6801 2.1 13.2601 3.6Z"
                                stroke="#000"
                                stroke-width="1.5"
                                stroke-miterlimit="10"
                                stroke-linecap="round"
                                stroke-linejoin="round"
                            />
                            <path
                                d="M11.8899 5.05C12.3199 7.81 14.5599 9.92 17.3399 10.2"
                                stroke="#000"
                                stroke-width="1.5"
                                stroke-miterlimit="10"
                                stroke-linecap="round"
                                stroke-linejoin="round"
                            />
                            <path d="M3 22H21" stroke="#000" stroke-width="1.5" stroke-miterlimit="10" stroke-linecap="round" stroke-linejoin="round" />
                        </svg>
                        <span style="color: #000">{{ $t('edit') }}</span>
                    </b-button>
                    <div class="d-flex justify-content-start">
                        <b-button variant="info" class="px-5" size="sm">
                            <span>{{ $t('save') }}</span>
                        </b-button>
                    </div>
                </div>
            </b-col>
        </b-row>
    </b-card>
</template>
<script>
import WInput from '@/components/forms/WInput.vue';
import AccountService from '@/services/account.service';
export default {
    created() {
        this.Refresh();
    },
    components: {
        WInput
    },
    data() {
        return {
            Data: {},
            Parent: {}
        };
    },
    methods: {
        Refresh() {
            this.SearchLoading = true;
            AccountService.GetUserInfo()
                .then((res) => {
                    this.Data = res.data;
                    this.Parent = res.data.contractor;
                })
                .catch((error) => {
                    this.makeToast(error.response.data.error, 'error');
                });
        }
    }
};
</script>
