<!-- eslint-disable vue/no-v-html -->
<template>
    <div>
        <AppListHeaderForName title="PartnershipCertificate" />
        <div class="container">
            <b-overlay :show="Loading" style="min-height: calc(100vh - 460px)">
                <div class="d-flex justify-content-center">
                    <b-card class="form-card p-4">
                        <div v-html="HTMLData" style="max-width: 750px"></div>
                    </b-card>
                </div>
            </b-overlay>
        </div>
    </div>
</template>

<script>
import PrtnCertificateService from '@/services/prtncertificate.service';
import AppListHeaderForName from '@/components/application/AppListHeaderForName.vue';

export default {
    components: {
        AppListHeaderForName
    },
    data() {
        return {
            HTMLData: {},
            Loading: false,
            sendLoading: false
        };
    },
    created() {
        this.Refresh();
    },
    methods: {
        Refresh() {
            PrtnCertificateService.Get(this.$route.params.id)
                .then((res) => {
                    PrtnCertificateService.GetCertificateAsHtml(res.data.id2)
                        .then((res) => {
                            this.HTMLData = res.data;
                        })
                        .catch((error) => {
                            this.showApiError(error);
                        });
                })
                .catch((error) => {
                    this.showApiError(error);
                });
        }
    }
};
</script>
