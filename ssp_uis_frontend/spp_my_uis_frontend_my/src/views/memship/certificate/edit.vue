<template>
    <div>
        <AppListHeaderForName title="PartnershipCertificate" page-name="MemshipCertificate" />
        <div class="container-fluid">
            <b-overlay :show="Loading" class="loading-container" spinner-variant="info">
                <PdfViewer :link="IframeSrc" v-if="Data.id2">
                    <template v-if="Data.message" #right-side>
                        <b-card-title class="text-danger text-center">{{ $t('CLAIM_APPLICATION_CANCEL2') }}</b-card-title>
                        <b-card-text> {{ Data.message }}</b-card-text>
                    </template>
                </PdfViewer>
            </b-overlay>
        </div>
    </div>
</template>

<script>
import MemshipCertificateService from '@/services/memshipcertificate.service';
import AppListHeaderForName from '@/components/application/AppListHeaderForName.vue';
import axios from 'axios';
import PdfViewer from '@/components/PdfViewer.vue';

export default {
    components: {
        AppListHeaderForName,
        PdfViewer
    },
    data() {
        return {
            Data: {},
            Loading: false,
            sendLoading: false
        };
    },
    created() {
        this.Refresh();
    },
    computed: {
        IframeSrc() {
            return axios.defaults.baseURL + `MemshipCertificate/DownloadPdf?id2=${this.Data.id2}&lang=${this.getPdfLang()}`;
        }
    },
    methods: {
        Refresh() {
            MemshipCertificateService.Get(this.$route.params.id)
                .then((res) => {
                    this.Data = res.data;
                })
                .catch((error) => {
                    this.showApiError(error);
                });
        }
    }
};
</script>
