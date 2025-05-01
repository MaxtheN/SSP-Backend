<!-- eslint-disable vue/no-v-html -->
<template>
    <div>
        <AppListHeaderForName title="DualApplication" page-name="DualApplication" />

        <div class="container">
            <b-overlay :show="Loading" style="min-height: calc(100vh - 460px)">
                <b-row>
                    <b-col sm="12" lg="9">
                        <b-card class="form-card" v-if="Application.application">
                            <validation-observer ref="ValidationDTO">
                                <b-tabs pills card lazy>
                                    <b-tab active :title="$t('AdditionalInfo')">
                                        <b-row>
                                            <b-col sm="12" md="6" lg="6">
                                                <WInput
                                                    :label="$t('contractorInn')"
                                                    :name="$t('contractorInn')"
                                                    :placeholder="$t('contractorInn')"
                                                    disabled
                                                    v-model="Application.application.contractorInn"
                                                    rules="required"
                                                />
                                            </b-col>
                                            <b-col sm="12" md="6" lg="6">
                                                <WInput
                                                    :label="$t('contractor')"
                                                    :name="$t('contractor')"
                                                    :placeholder="$t('contractor')"
                                                    disabled
                                                    :value="
                                                        Application.application.contractorInn && Application.application.contractorInn.length == 14
                                                            ? Application.application.contractor + ' - ' + Application.application.contractorDirector
                                                            : Application.application.contractor
                                                    "
                                                    rules="required"
                                                />
                                            </b-col>
                                            <b-col sm="12" md="6" lg="6">
                                                <WInput
                                                    :label="$t('contractorDirector')"
                                                    :name="$t('contractorDirector')"
                                                    :placeholder="$t('contractorDirector')"
                                                    disabled
                                                    v-model="Application.application.contractorDirector"
                                                    rules="required"
                                                />
                                            </b-col>
                                            <b-col sm="12" md="6" lg="6">
                                                <AppContractorSettlementAccount v-model="Application.application.contractorSettlementAccountId" />
                                            </b-col>
                                            <b-col sm="12" md="6" lg="6">
                                                <WInput
                                                    :label="$t('docNumber')"
                                                    :name="$t('docNumber')"
                                                    :placeholder="$t('docNumber')"
                                                    disabled
                                                    v-model="Application.application.docNumber"
                                                    rules="required"
                                                />
                                            </b-col>
                                            <b-col sm="12" md="6" lg="6">
                                                <WInput
                                                    :label="$t('docOn')"
                                                    :name="$t('docOn')"
                                                    :placeholder="$t('docOn')"
                                                    disabled
                                                    v-model="Application.application.docOn"
                                                    rules="required"
                                                />
                                            </b-col>
                                            <b-col sm="12" md="6" lg="6">
                                                <WSelect
                                                    rules="required"
                                                    disabled
                                                    :label="$t('dualEducationType')"
                                                    :name="$t('dualEducationType')"
                                                    :clearable="true"
                                                    :options="DualEducationTypeList"
                                                    v-model="Application.dualEducationTypeId"
                                                />
                                            </b-col>
                                        </b-row>
                                        <b-row class="mt-2">
                                            <p class="text-left address-text">{{ $t('address') }}</p>
                                            <b-col sm="12" md="3" lg="3">
                                                <WInput
                                                    :label="$t('region')"
                                                    :name="$t('region')"
                                                    :placeholder="$t('region')"
                                                    disabled
                                                    v-model="Application.application.region"
                                                    rules="required"
                                                />
                                            </b-col>
                                            <b-col sm="12" md="3" lg="3">
                                                <WInput
                                                    :label="$t('district')"
                                                    :name="$t('district')"
                                                    :placeholder="$t('district')"
                                                    disabled
                                                    v-model="Application.application.district"
                                                    rules="required"
                                                />
                                            </b-col>
                                            <b-col sm="12" md="6" lg="6">
                                                <WInput
                                                    :label="$t('contractorAddress')"
                                                    :name="$t('contractorAddress')"
                                                    :placeholder="$t('contractorAddress')"
                                                    disabled
                                                    v-model="Application.application.contractorAddress"
                                                    rules="required"
                                                />
                                            </b-col>
                                        </b-row>

                                        <b-row>
                                            <b-col sm="12" lg="12" class="mt-2">
                                                <b-table :items="Application.tables" :fields="fields" responsive striped bordered> </b-table>
                                            </b-col>
                                        </b-row>
                                    </b-tab>
                                </b-tabs>
                            </validation-observer>
                        </b-card>
                    </b-col>
                    <b-col sm="12" lg="3">
                        <b-card class="form-card">
                            <b-card-text>
                                <!-- send -->
                                <b-button @click="$bvModal.show('ESPmodal')" variant="primary" block class="mb-1">
                                    <b-icon-arrow-bar-up scale="0.6"></b-icon-arrow-bar-up>
                                    {{ $t('Send') }}
                                </b-button>
                                <!-- canRevoke  -->
                                <b-button v-if="Application.canRevoke" @click="OpenRevokeModal(Application)" variant="warning" block class="mb-1">
                                    <b-icon-x-circle scale="0.6"></b-icon-x-circle>
                                    {{ $t('Revoke') }}
                                </b-button>
                            </b-card-text>
                        </b-card>
                    </b-col>
                </b-row>
            </b-overlay>
            <b-modal :id="'DeleteModal' + Application.id" :title="$t('delete')" no-close-on-backdrop hide-footer>
                <p>{{ $t('WantDeleteAdm') }}</p>
                <b-row>
                    <b-col style="text-align: end" class="text-right">
                        <a @click="$bvModal.hide('DeleteModal' + Application.id)" style="margin-right: 5px" class="btn btn-sm btn-soft-danger mr-2 pr-btn">{{ $t('no') }}</a>
                        <a @click="Delete(Application)" class="btn btn-sm btn-success pr-btn">
                            <b-spinner v-if="DeleteLoading" small></b-spinner>
                            {{ $t('yes') }}
                        </a>
                    </b-col>
                </b-row>
            </b-modal>
            <b-modal :id="'SendModal' + Application.id" :title="$t('send')" no-close-on-backdrop hide-footer>
                <p>{{ $t('wantSend') }}</p>

                <b-row class="mt-3">
                    <b-col style="text-align: end" class="text-right">
                        <a @click="$bvModal.hide('SendModal' + Application.id)" class="btn btn-sm btn-soft-danger mr-2 pr-btn" style="margin-right: 5px">{{ $t('no') }}</a>

                        <b-button variant="success" @click="Send(Application)">
                            <b-spinner v-if="SendLoading" small></b-spinner>
                            {{ $t('yes') }}
                        </b-button>
                    </b-col>
                </b-row>
            </b-modal>

            <b-modal :id="'CancelModal' + Application.id" :title="$t('Revoke')" no-close-on-backdrop hide-footer>
                <p>{{ $t('WantRevoke') }}</p>
                <b-row>
                    <b-col style="text-align: end" class="text-right">
                        <a @click="$bvModal.hide('CancelModal' + Application.id)" class="btn btn-sm btn-soft-danger mr-2 pr-btn" style="margin-right: 5px">{{ $t('no') }}</a>
                        <a @click="Revoke(Application)" class="btn btn-sm btn-success pr-btn">
                            <b-spinner v-if="CancelLoading" small></b-spinner>
                            {{ $t('yes') }}
                        </a>
                    </b-col>
                </b-row>
            </b-modal>

            <b-modal id="ESPmodal" :title="$t('ESPmodal')" no-close-on-backdrop hide-footer>
                <just-sign @sign="GetESP($event)"></just-sign>

                <b-row>
                    <b-col style="text-align: end" class="text-right">
                        <a @click="$bvModal.hide('ESPmodal')" class="btn btn-sm btn-soft-danger mr-2 pr-btn" style="margin-right: 5px">{{ $t('no') }}</a>
                    </b-col>
                </b-row>
            </b-modal>
        </div>
    </div>
</template>

<script>
import DualApplicationService from '@/services/dual/dualapplication.service';
import ManualService from '@/services/manual.service';
import AppListHeaderForName from '@/components/application/AppListHeaderForName.vue';
import WInput from '@/components/forms/WInput.vue';
import WSelect from '@/components/forms/WSelect.vue';
import AppWord from '@/components/word/AppWord.vue';
import AppContractorSettlementAccount from '@/components/application/AppContractorSettlementAccount.vue';
import JustSign from '@/components/justSign.vue';

export default {
    components: {
        JustSign,
        AppListHeaderForName,
        WSelect,
        WInput,
        AppWord,
        AppContractorSettlementAccount
    },
    data() {
        return {
            Application: {},
            PositionList: [],
            InstituteList: [],
            SpecialtyList: [],
            DualEducationTypeList: [],
            ApplicationHTMLData: {},
            fields: [
                {
                    key: 'positionClassification',
                    label: this.$t('position'),
                    sortable: false,
                    tdClass: 'text-center',
                    thClass: 'text-center'
                },
                {
                    key: 'institute',
                    label: this.$t('institute'),
                    sortable: false,
                    tdClass: 'text-center',
                    thClass: 'text-center'
                },

                {
                    key: 'specialty',
                    label: this.$t('specialty'),
                    sortable: false,
                    tdClass: 'text-center',
                    thClass: 'text-center'
                },
                {
                    key: 'emptyPositionsCount',
                    label: this.$t('emptyPositionsCount'),
                    sortable: false,
                    tdClass: 'text-center',
                    thClass: 'text-center'
                },

                {
                    key: 'details',
                    label: this.$t('details'),
                    sortable: false,
                    tdClass: 'text-center',
                    thClass: 'text-center'
                }
            ],
            filter: {
                signedData: '',
                isPinfl: true,
                id: 1,
                message: ''
            },

            Loading: false,
            orgName: '',
            saveLoading: false,
            DeleteLoading: false,
            SendLoading: false,
            CancelLoading: false
        };
    },

    created() {
        this.Refresh();
        ManualService.DualEducationTypeSelectList().then((res) => {
            this.DualEducationTypeList = res.data;
        });
        ManualService.GetAsSelectList().then((res) => {
            this.PositionList = res.data;
        });
        ManualService.InstituteSelectList().then((res) => {
            this.InstituteList = res.data;
        });
    },
    methods: {
        Refresh() {
            this.Loading = true;
            DualApplicationService.Get(this.$route.params.id)
                .then((res) => {
                    this.Application = res.data;
                })
                .catch(this.showApiError)
                .finally(() => {
                    this.Loading = false;
                });
        },

        OpenRevokeModal(item) {
            this.filter.id = item.id;
            this.filter.message = '';
            this.$bvModal.show('CancelModal' + item.id);
        },
        OpenSendModal(item) {
            this.filter.id = item.id;
            this.filter.message = '';
            this.$bvModal.show('SendModal' + item.id);
        },
        GetESP(data) {
            this.$bvModal.hide('ESPmodal');

            this.Sign(data);
        },
        Sign(item) {
            this.SendLoading = true;
            DualApplicationService.Send({ id: this.Application.id, message: this.filter.message, signedData: item.key })
                .then(() => {
                    this.$bvModal.hide('SendModal' + item.id);
                    this.makeToast(this.$t('SuccessSend'), 'success');
                    this.$router.push({ name: 'DualApplication' });
                })
                .catch((error) => {
                    this.showApiError(error);
                })
                .finally(() => {
                    this.SendLoading = false;
                });
        },
        Revoke(item) {
            DualApplicationService.Revoke({ id: this.Application.id, message: this.filter.message })
                .then((res) => {
                    this.$bvModal.hide('CancelModal' + this.item.id);
                    this.Refresh();
                })
                .catch((error) => {
                    this.showApiError(error);
                })
                .finally(() => {
                    this.SendLoading = false;
                });
        }
    }
};
</script>
