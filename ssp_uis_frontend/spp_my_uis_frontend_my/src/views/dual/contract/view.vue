<template>
    <div>
        <AppListHeaderForName title="DualContract" page-name="DualContract" />

        <div class="container">
            <b-overlay :show="Loading" class="loading-container" spinner-variant="info">
                <PdfViewer :link="IframeSrc" v-if="Data.id2">
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
        </div>
    </div>
</template>

<script>
import JustSign from '@/components/justSign.vue';
import DualContractService from '@/services/dual/dualcontract.service';
import axios from 'axios';
import AppListHeaderForName from '@/components/application/AppListHeaderForName.vue';
import eimzoMixin from '@/mixins/eimzo';
import PdfViewer from '@/components/PdfViewer.vue';

export default {
    components: {
        JustSign,
        AppListHeaderForName,
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
            return axios.defaults.baseURL + `Dual/DualContractIntegration/DownloadContract/${this.Data.id2}`;
        }
    },
    methods: {
        OpenSendModal(item) {
            this.selectedItem = item;
            this.signModal = true;
        },

        Send(data) {
            this.SendLoading = true;
            DualContractService.Sign({
                id: this.Data.id,
                signedData: data.key,
                isPinfl: this.isPinfl(data)
            })
                .then(() => {
                    this.signModal = false;
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
            DualContractService.Get(this.$route.params.id)
                .then((res) => {
                    this.Data = res.data;
                    this.Loading = false;
                })
                .catch((error) => {
                    this.showApiError(error);
                });
        }
    }
};
</script>
