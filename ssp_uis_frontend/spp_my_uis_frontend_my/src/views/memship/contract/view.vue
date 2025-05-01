<template>
    <div>
        <AppListHeaderForName title="MemshipContract" page-name="MemshipContract" />
        <div class="container-fluid">
            <b-overlay :show="Loading" class="loading-container" spinner-variant="info">
                <PdfViewer :link="IframeSrc" v-if="Data.id2">
                    <template #right-side>
                        <b-card-text>
                            <label style="font-weight: bold">{{ $t('status') }}</label
                            >: <b-badge :variant="getColor(Data)">{{ Data.status }}</b-badge> <br />
                            <label style="font-weight: bold">{{ $t('message') }}</label
                            >: {{ Data.message }}
                        </b-card-text>

                        <b-card-text>
                            <b-button
                                v-if="Data.canSign"
                                @click="OpenSendModal(Data)"
                                style="margin-right: 5px; white-space: nowrap"
                                variant="primary"
                                class="btn btn-sm btn-soft-primary mr-2 mt-2 pr-btn myButton w-100"
                            >
                                <b-icon-arrow-bar-up scale="0.8"></b-icon-arrow-bar-up>
                                {{ $t('sign') }}
                            </b-button>
                        </b-card-text>
                    </template>
                </PdfViewer>
            </b-overlay>

            <b-modal :id="'SendModal' + Data.id" :title="$t('sign')" no-close-on-backdrop hide-footer>
                <b-row class="mt-3">
                    <b-col style="text-align: end" class="text-right">
                        <a @click="$bvModal.hide('SendModal' + Data.id)" class="btn btn-sm btn-soft-danger mr-2 pr-btn" style="margin-right: 5px">{{ $t('no') }}</a>

                        <b-button variant="success" v-b-modal.ESPmodal>
                            <b-spinner v-if="SendLoading" small></b-spinner>
                            {{ $t('yes') }}
                        </b-button>
                    </b-col>
                </b-row>
            </b-modal>

            <b-modal id="ESPmodal" :title="$t('ESPmodal')" no-close-on-backdrop hide-footer>
                <just-sign @sign="GetESP($event)"></just-sign>

                <b-row>
                    <b-col style="text-align: end" class="text-right">
                        <a @click="$bvModal.hide('ESPmodal')" class="btn btn-sm btn-soft-danger mr-2 pr-btn" style="margin-right: 5px">{{ $t('no') }}</a>
                        <a @click="Send(Data)" class="btn btn-sm btn-success pr-btn">
                            <b-spinner v-if="SendLoading" small></b-spinner>
                            {{ $t('yes') }}
                        </a>
                    </b-col>
                </b-row>
            </b-modal>

            <Chat v-if="Data && Data.id" :table-id="Data.tableId || 99" :document-id="Data.id" />
        </div>
    </div>
</template>

<script>
import ApplicationService from '@/services/application.service';
import MemshipContractService from '@/services/memshipcontract.service';
import JustSign from '@/components/justSign.vue';
import PdfViewer from '@/components/PdfViewer.vue';

import axios from 'axios';
import AppListHeaderForName from '@/components/application/AppListHeaderForName.vue';
const Chat = () => import('@/components/DocumentChat/Chat.vue');

export default {
    components: {
        JustSign,
        AppListHeaderForName,
        Chat,
        PdfViewer
    },
    data() {
        return {
            Application: {},
            Data: {},
            Loading: false,
            sendLoading: false,
            iframeLoaded: false,
            SendLoading: false,
            filter: {
                id: 0,
                message: '',
                isPinfl: false,
                signedData: ''
            }
        };
    },
    created() {
        this.Refresh();
    },
    computed: {
        IframeSrc() {
            return axios.defaults.baseURL + `Memship/MemshipContract/DownloadPdf?id2=${this.Data.id2}&lang=${this.getPdfLang()}`;
        }
    },
    methods: {
        GetESP(data) {
            var info = {};
            if (!!data.data.alias) {
                var arr = [];
                data.data.alias.split(',').forEach(function (item) {
                    arr.push(item.split('='));
                });
                const entries = new Map(arr);
                info = Object.fromEntries(entries);
            }
            var obj = {
                signedData: data.key,
                state: '',
                email: null,
                fullName: data.data.CN,
                inn: data.data.TIN,
                pinfl: info['1.2.860.3.16.1.2'],
                isPinfl: info['1.2.860.3.16.1.1'] ? false : true,
                eSignCertificateNumber: info.serialnumber
            };
            this.filter.signedData = obj.signedData;
            this.filter.isPinfl = obj.isPinfl;
            this.$bvModal.hide('ESPmodal');

            this.Send(this.Data);
        },
        FileDownload(item) {
            MemshipContractService.DownloadPdf(item.id2).then((res) => {
                this.forceFileDownload(res, this.$t('memshipcontract'));
            });
        },
        OpenSendModal(item) {
            this.filter.id = item.id;
            this.filter.message = '';
            this.$bvModal.show('SendModal' + item.id);
        },

        Send(item) {
            this.SendLoading = true;
            MemshipContractService.Sign(this.filter)
                .then(() => {
                    this.$bvModal.hide('SendModal' + item.id);
                    this.$router.go(-1);
                    // this.Refresh();
                })
                .catch((error) => {
                    this.showApiError(error);
                })
                .finally(() => {
                    this.SendLoading = false;
                });
        },

        Refresh() {
            this.Loading = true;
            MemshipContractService.Get(this.$route.params.id)
                .then((res) => {
                    this.Data = res.data;
                })
                .catch((error) => {
                    this.showApiError(error);
                })
                .finally(() => {
                    this.Loading = false;
                });
        },

        SendData() {
            this.sendLoading = true;
            ApplicationService.Update(this.Application)
                .then(() => {
                    this.$router.push({ name: 'MemshipContract' });
                    this.makeToast(this.$t('SaveSuccess'), 'success');
                })
                .catch((error) => {
                    this.showApiError(error);
                })
                .finally(() => {
                    this.sendLoading = false;
                });
        }
    }
};
</script>
