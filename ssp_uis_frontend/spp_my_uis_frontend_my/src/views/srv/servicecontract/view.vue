<template>
    <div>
        <AppListHeaderForName title="ServiceContract" page-name="ServiceContract" :query="{ type: $route.query.type }" />
        <div class="container-fluid">
            <b-overlay :show="Loading" class="loading-container" spinner-variant="info">
                <PdfViewer :link="IframeSrc" v-if="Data.id2">
                    <template #right-side__number>{{ Data.docNumber }}</template>
                    <template #right-side__date>{{ Data.docOn }}</template>
                    <template #right-side__status>{{ Data.status }}</template>
                    <template #right-side__message>{{ Data.message }}</template>
                    <template #right-side>
                        <b-card-text>
                            <b-button
                                v-if="Data.canSign"
                                :disabled="SendLoading"
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

            <b-modal v-model="signModal" :title="$t('ESPmodal')" no-close-on-backdrop hide-footer>
                <just-sign @sign="Send($event)"></just-sign>
            </b-modal>

            <Chat v-if="Data && Data.id" :table-id="Data.tableId || 119" :document-id="Data.id" />
        </div>
    </div>
</template>

<script>
import ServiceContractService from '@/services/srv/servicecontract.service';
import JustSign from '@/components/justSign.vue';

import axios from 'axios';
import AppListHeaderForName from '@/components/application/AppListHeaderForName.vue';
import eimzoMixin from '@/mixins/eimzo';
import PdfViewer from '@/components/PdfViewer.vue';

const Chat = () => import('@/components/DocumentChat/Chat.vue');

export default {
    components: {
        JustSign,
        AppListHeaderForName,
        Chat,
        PdfViewer
    },
    mixins: [eimzoMixin],
    data() {
        return {
            Application: {},
            Data: {},
            Loading: false,
            sendLoading: false,
            SendLoading: false,
            signModal: false,
            selectedItem: {}
        };
    },
    created() {
        this.Refresh();
    },
    computed: {
        IframeSrc() {
            return axios.defaults.baseURL + `srv/ServiceContract/DownloadPdf?id2=${this.Data.id2}&lang=${this.getPdfLang()}`;
        }
    },
    methods: {
        FileDownload(item) {
            ServiceContractService.DownloadPdf(item.id2).then((res) => {
                this.forceFileDownload(res, this.$t('ServiceContract'));
            });
        },
        OpenSendModal(item) {
            this.selectedItem = item;
            this.signModal = true;
        },

        Send(data) {
            this.SendLoading = true;
            ServiceContractService.Signed({
                id: this.Data.id,
                signedData: data.key,
                isPinfl: this.isPinfl(data)
            })
                .then(() => {
                    this.signModal = false;
                    this.makeToast(this.$t('SaveSuccess'), 'success');
                    this.Refresh();
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
            ServiceContractService.Get(this.$route.params.id)
                .then((res) => {
                    this.Data = res.data;
                })
                .catch((error) => {
                    this.showApiError(error);
                })
                .finally(() => {
                    this.Loading = false;
                });
        }
    }
};
</script>
