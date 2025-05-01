<!-- eslint-disable vue/no-v-html -->
<template>
    <div>
        <b-overlay :show="Loading" style="min-height: calc(100vh - 460px)">
            <div class="container" style="margin-top: 100px">
                <b-row>
                    <b-col sm="12" lg="6">
                        <h2 @click="$router.go(-1)" style="cursor: pointer" class="textbackviewstyle">
                            <b-icon-chevron-left></b-icon-chevron-left>
                            {{ $t('certificate_second') }}
                        </h2>
                    </b-col>
                    <b-col sm="12" lg="6" style="text-align: end !important"> </b-col>
                </b-row>
                <b-row>
                    <b-col sm="12" md="2" lg="2"></b-col>
                    <b-col sm="12" md="8" lg="8">
                        <div v-html="HTMLData"></div>
                    </b-col>
                </b-row>
            </div>
        </b-overlay>
    </div>
</template>

<script>
import PrtnCertificateService from '@/services/prtncertificate.service';

export default {
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
