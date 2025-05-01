<template>
    <div>
        <AppListHeaderForName title="StateAssetApplication" page-name="StateAssetApplication" />
        <div class="container">
            <b-overlay :show="Loading" style="min-height: calc(100vh - 460px)">
                <b-card class="form-card">
                    <b-tabs pills card>
                        <b-tab active :title="$t('AdditionalInfo')"
                            ><b-card-text>
                                <b-row>
                                    <b-col sm="12" md="5" lg="5" class="m-2">
                                        <WInput rules="required" :label="$t('contractorInn')" placeholder="" disabled v-model="Application.contractorInn"></WInput>
                                    </b-col>
                                    <b-col sm="12" md="5" lg="5" class="m-2">
                                        <WInput :label="$t('contractor')" placeholder="" disabled v-model="Application.contractor"></WInput>
                                    </b-col>
                                    <b-col sm="12" md="6" lg="6">
                                        <AppContractorSettlementAccount v-model="Application.contractorSettlementAccountId" />
                                    </b-col>
                                    <b-col sm="12" md="5" lg="5" class="m-2">
                                        <WInput rules="required" :label="$t('region')" placeholder="" disabled v-model="Application.region"></WInput> </b-col
                                    ><b-col sm="12" md="5" lg="5" class="m-2">
                                        <WInput rules="required" :label="$t('district')" placeholder="" disabled v-model="Application.district"></WInput>
                                    </b-col>
                                    <b-col sm="12" md="5" lg="5" class="m-2">
                                        <WInput rules="required" :label="$t('contractorAddress')" placeholder="" disabled v-model="Application.contractorAddress"></WInput>
                                    </b-col>
                                    <b-col sm="12" md="5" lg="5" class="m-2">
                                        <WInput rules="required" :label="$t('phoneNumber')" placeholder="" disabled v-model="Application.phoneNumber"></WInput>
                                    </b-col>
                                    <b-col sm="12" md="5" lg="5" class="m-2">
                                        <WInput
                                            rules="required"
                                            :label="$t('prtnCertificateDocNumber')"
                                            placeholder=""
                                            disabled
                                            v-model="Application.prtnCertificateDocNumber"
                                        ></WInput>
                                    </b-col>
                                    <b-col sm="12" md="5" lg="5" class="m-2">
                                        <WInput rules="required" :label="$t('prtnCertificateDocOn')" placeholder="" disabled v-model="Application.prtnCertificateDocOn"></WInput>
                                    </b-col>
                                    <b-col sm="12" md="5" lg="5" class="m-2">
                                        <WInput
                                            rules="required"
                                            v-mask="'#####################'"
                                            :label="$t('auctionDocNumber')"
                                            :name="$t('auctionDocNumber')"
                                            placeholder="1234"
                                            v-model="Application.auctionDocNumber"
                                        ></WInput>
                                    </b-col>
                                    <b-col sm="12" md="5" lg="5" class="m-2">
                                        <WDatePicker
                                            v-model="Application.auctionDocOn"
                                            :label="$t('auctionDocOn')"
                                            :name="$t('auctionDocOn')"
                                            @keyup="docDateValue"
                                            format="DD.MM.YYYY"
                                            type="date"
                                            :clearable="true"
                                            :placeholder="$t('auctionDocOn')"
                                        >
                                        </WDatePicker>
                                    </b-col>
                                    <b-col sm="12" md="10" lg="10" class="m-2">
                                        <WInput
                                            rules="required"
                                            :label="$t('stateAssetName')"
                                            :name="$t('stateAssetName')"
                                            :placeholder="$t('stateAssetName')"
                                            v-model="Application.stateAssetName"
                                        ></WInput>
                                    </b-col>
                                    <b-col sm="12" md="12" style="text-align: right !important">
                                        <b-button v-if="Application.id == 0 || Application.canEdit" variant="success" @click="SendData">
                                            <b-icon-check></b-icon-check> {{ $t('save') }}
                                        </b-button>
                                    </b-col>
                                </b-row>
                            </b-card-text></b-tab
                        >
                    </b-tabs>
                </b-card>
            </b-overlay>
        </div>
    </div>
</template>

<script>
import WDatePicker from '@/components/forms/WDatePicker.vue';
import WInput from '@/components/forms/WInput.vue';
import WSelect from '@/components/forms/WSelect.vue';
import AppListHeaderForName from '@/components/application/AppListHeaderForName.vue';
import StateAssetApplicationService from '@/services/stateassetapplication.service';
import AppContractorSettlementAccount from '@/components/application/AppContractorSettlementAccount.vue';

export default {
    components: {
        WDatePicker,
        AppListHeaderForName,
        WInput,
        WSelect,
        AppContractorSettlementAccount
    },
    data() {
        return {
            Application: {},
            MonthInList: [],
            RegionList: [],
            MfyList1: [],
            YearList: [],
            DistrictList: [],
            ChooseDistrictList: [],
            MfyListLocationFalse: [],
            ApplicationHTMLData: {},
            List: [],
            PrtnList: [],
            HtmlData: {},
            Loading: false,
            saveLoading: false
        };
    },
    created() {
        const today = new Date();
        const yyyy = today.getFullYear();
        this.Refresh();
    },
    methods: {
        GetAsHtmlPost() {
            StateAssetApplicationService.GetAsHtmlPost(this.Application)
                .then((res) => {
                    this.HtmlData = res.data;
                })
                .catch((error) => {
                    this.makeToast(error.response.data);
                });
        },
        docDateValue(value) {
            this.Application.auctionDocOn = value;
        },

        Refresh() {
            StateAssetApplicationService.Get(this.$route.params.id)
                .then((res) => {
                    this.Application = res.data;
                    this.GetAsHtmlPost();
                })
                .catch((error) => {
                    this.showApiError(error);
                });
        },

        check() {
            if (!this.Application.auctionDocNumber) {
                this.makeToast(this.$t('auctionDocNumberNotEntered'), 'error');
                return false;
            }
            if (!this.Application.auctionDocOn) {
                this.makeToast(this.$t('auctionDocOnNotEntered'), 'error');
                return false;
            }
            if (!this.Application.stateAssetName) {
                this.makeToast(this.$t('stateAssetNameNotEntered'), 'error');
                return false;
            }
            return true;
        },

        SendData() {
            if (!this.check()) {
                return false;
            }
            this.saveLoading = true;
            StateAssetApplicationService.Update(this.Application)
                .then((res) => {
                    this.$router.push({ name: 'StateAssetApplication' });
                    this.makeToast(this.$t('SaveSuccess'), 'success');
                })
                .catch((error) => {
                    this.showApiError(error);
                })
                .finally(() => {
                    this.saveLoading = false;
                });
        }
    }
};
</script>
