<template>
    <div>
        <AppListHeaderForName title="PartnershipContract" />

        <div class="container-fluid">
            <b-overlay :show="Loading" class="loading-container" spinner-variant="info">
                <PdfViewer :link="IframeSrc" v-if="Application.id2">
                    <template #right-side>
                        <b-button v-if="Application.canSign || Application.statusId == 29" @click="OpenRejectModal" variant="danger">
                            {{ $t('Reject') }}
                        </b-button>
                        <b-button v-if="Application.canSign" @click="signModal = !signModal" variant="success" class="ml-2">
                            {{ $t('sign') }}
                        </b-button>
                    </template>
                </PdfViewer>
            </b-overlay>

            <b-modal v-model="signModal" size="md" hide-footer hide-header>
                <div style="text-align: right; margin-right: 10px; margin-top: -5px; margin-bottom: 5px; border-bottom: 1px solid lightgray">
                    <span @click="signModal = false" style="cursor: pointer; font-size: 30px"> &times; </span>
                </div>
                <div>
                    <just-sign :data-to-sign="Application" @sign="loginESP($event)"></just-sign>
                </div>
            </b-modal>
            <b-modal size="md" v-model="rejectModal" hide-footer hide-header :title="$t('Reject')">
                <b-row class="mr-2">
                    <b-col sm="12" md="12" class="m-3" style="margin-right: 20px !important">
                        <custom-select
                            :label="$t('RejectReason')"
                            style="width: 90%"
                            :clearable="true"
                            :valueid="'value'"
                            :valuename="'text'"
                            :options="RejectList"
                            v-model="reject.prtnRejectReasonId"
                        />
                    </b-col>
                    <b-col sm="12" md="12" class="m-3">
                        <b-form-textarea style="width: 90%" v-model="reject.message" :placeholder="$t('message')"> </b-form-textarea>
                    </b-col>
                </b-row>
                <b-row>
                    <b-col sm="12" md="12" class="d-flex justify-content-end">
                        <button class="button-style" @click="rejectModal = !rejectModal">
                            {{ $t('no') }}
                        </button>
                        <button class="button-style" @click="RejectReason">
                            {{ $t('yes') }}
                        </button>
                    </b-col>
                </b-row>
            </b-modal>
        </div>
    </div>
</template>

<script>
import customSelect from '@/components/elements/customSelect.vue';
import axios from 'axios';
import ContractService from '@/services/prtncontract.service';
import ManualService from '@/services/manual.service';
import JustSign from '@/components/justSign.vue';
import AppListHeaderForName from '@/components/application/AppListHeaderForName.vue';
import eimzoMixin from '@/mixins/eimzo';
import PdfViewer from '@/components/PdfViewer.vue';

export default {
    components: {
        customSelect,
        JustSign,
        AppListHeaderForName,
        PdfViewer
    },
    mixins: [eimzoMixin],
    data() {
        return {
            axios,
            Application: {},
            ApplicationHTMLData: {},
            RejectList: [],
            List: [],
            signModal: false,
            rejectModal: false,
            PrtnList: [],
            Loading: false,
            sendLoading: false,
            reject: {
                message: '',
                prtnRejectReasonId: 0,
                id: 0,
                statusId: 0
            }
        };
    },
    computed: {
        IframeSrc() {
            return axios.defaults.baseURL + `PrtnContract/PrintPrtnContractPdf?Id2=${this.Application.id2}&lang=${this.getPdfLang()}`;
        }
    },
    created() {
        this.Refresh();
    },
    methods: {
        OpenRejectModal() {
            this.rejectModal = true;
            ManualService.PrtnRejectReasonSelectList(this.Application.prtnContractTypeId).then((res) => {
                this.RejectList = res.data;
            });
        },
        check() {
            if (!this.reject.prtnRejectReasonId) {
                this.makeToast(this.$t('prtnRejectReasonNotSelected'), 'error');
                return false;
            }
            if (this.reject.message === null || this.reject.message === undefined || this.reject.message === 0 || this.reject.message === '') {
                this.makeToast(this.$t('messageNotSelected'), 'error');
                return false;
            }
            return true;
        },
        RejectReason() {
            if (!this.check()) {
                return false;
            }
            this.reject.id = this.Application.id;
            this.reject.statusId = this.Application.statusId;
            ContractService.Reject(this.reject)
                .then((res) => {
                    console.log(res);
                    this.$router.push({ name: 'Contract' });
                })
                .catch((error) => {
                    this.showApiError(error);
                });
        },
        Refresh() {
            this.Loading = true;
            ContractService.Get(this.$route.params.id)
                .then((res) => {
                    this.Application = res.data;
                    ContractService.GetPrtnContractAsHtml(this.Application.id2)
                        .then((res) => {
                            this.ApplicationHTMLData = res.data;
                        })
                        .catch((error) => {
                            this.showApiError(error);
                        });
                })
                .catch((error) => {
                    this.showApiError(error);
                })
                .finally(() => {
                    this.Loading = false;
                });
        },

        loginESP(data) {
            const isPinfl = this.isPinfl(data);

            const obj = {
                id: this.Application.id,
                signedData: data.key,
                isPinfl: isPinfl
            };
            this.signModal = false;

            ContractService.Sign(obj)
                .then((res) => {
                    this.$router.push({ name: 'Contract' });
                })
                .catch((error) => {
                    this.makeToast(error.response.data, 'error');
                });
        }
    }
};
</script>
