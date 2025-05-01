<template>
    <div>
        <AppListHeaderForName title="yigirmaming_tadbirkor" page-name="MyCabinet" />

        <div class="container-fluid">
            <b-row>
                <b-col sm="12" lg="3" md="4" v-if="!InfoCard.hasPrtnApplication" class="mt-3">
                    <WCard title="application_create" img-src="/images/ssp_images/application-button.svg?v=2" @click="createModal = true" />
                    <AppStartModal v-model="createModal" />
                </b-col>
                <b-col sm="12" lg="3" md="4" v-if="InfoCard.hasPrtnApplication" class="mt-3">
                    <WCard
                        count-url="Application/GetCount"
                        title="application"
                        img-src="/images/ssp_images/application-button.svg?v=2"
                        @click="$router.push({ name: 'PartnershipApplication' })"
                    />
                </b-col>
                <b-col sm="12" lg="3" md="4" v-if="InfoCard.hasPrtnContract" class="mt-3">
                    <WCard
                        title="contract"
                        count-url="PrtnContract/GetCount"
                        img-src="/images/ssp_images/contract-button.svg?v=2"
                        @click="$router.push({ name: 'PartnershipContract' })"
                    />
                </b-col>
                <b-col sm="12" lg="3" md="4" v-if="InfoCard.hasPrtnCertificate" class="mt-3">
                    <WCard
                        title="certificate"
                        count-url="PrtnCertificate/GetCount"
                        img-src="/images/ssp_images/cretificate-button.svg?v=2"
                        @click="$router.push({ name: 'PartnershipCertificate' })"
                    />
                </b-col>
                <b-col sm="12" lg="3" md="4" v-if="InfoCard.hasPrtnCertificate" class="mt-3">
                    <WCard
                        count-url="StateAssetApplication/GetCount"
                        title="StateAssetApplication"
                        img-src="/images/ssp_images/menuboard.svg?v=2"
                        @click="StateAssetApplicationmodal = true"
                    />
                    <StateAssetApplicationModal v-model="StateAssetApplicationmodal" @close="StateAssetApplicationmodal = false" />
                </b-col>
                <b-col sm="12" lg="3" md="4" class="mt-3" v-if="InfoCard.hasPrtnCertificate">
                    <WCard
                        count-url="PrtnCreditDemand/GetCount"
                        title="PrtnCreditDemand"
                        img-src="/images/ssp_images/application-button.svg?v=2"
                        @click="$router.push({ name: 'PrtnCreditDemand' })"
                    />
                </b-col>
                <b-col sm="12" lg="3" md="4" v-if="InfoCard.hasPrtnCertificate" class="mt-3">
                    <WCard title="CustomFeesApplication" img-src="/images/ssp_images/receipttext.svg?v=2" @click="CustomFeesModal = true" />
                    <CustomFeesModal v-model="CustomFeesModal" @close="CustomFeesModal = false" />
                </b-col>
                <b-col sm="12" lg="3" md="4" v-if="InfoCard.hasPrtnCertificate" class="mt-3">
                    <WCard title="SoliqApplication" img-src="/images/ssp_images/application-button.svg?v=2" @click="soliqModal = true" />
                    <SoliqModal v-model="soliqModal" @close="soliqModal = false" />
                </b-col>
                <b-col sm="12" lg="3" md="4" v-if="InfoCard.hasPrtnCertificate" class="mt-3">
                    <WCard title="PdfView" img-src="/images/ssp_images/clipboardtick.svg?v=2" @click="pdfModal = true" />
                    <PdfModal :type="InfoCard.prntContractTypeId" v-model="pdfModal" @close="pdfModal = false" />
                </b-col>
                <b-col sm="12" lg="3" md="4" class="mt-3">
                    <WCard count-url="MonoApplication/GetCount" title="MonoApplication" img-src="/images/ssp_images/stickynotes.svg?v=2" @click="$router.push('mono')" />
                </b-col>
            </b-row>
        </div>
    </div>
</template>

<script>
import AccountService from '@/services/account.service';
import AppListHeaderForName from '@/components/application/AppListHeaderForName.vue';
import WCard from '@/components/card/WCard.vue';

const AppStartModal = () => import('@/components/application/AppStartModal.vue');
const CustomFeesModal = () => import('@/components/application/CustomFeesModal.vue');
const PdfModal = () => import('@/components/application/PdfModal.vue');
const SoliqModal = () => import('@/components/application/SoliqModal.vue');
const StateAssetApplicationModal = () => import('@/components/application/StateAssetApplicationModal.vue');

export default {
    components: {
        AppListHeaderForName,
        WCard,
        AppStartModal,
        PdfModal,
        CustomFeesModal,
        SoliqModal,
        StateAssetApplicationModal
    },
    data() {
        return {
            InfoCard: {},
            createModal: false,
            CustomFeesModal: false,
            pdfModal: false,
            soliqModal: false,
            StateAssetApplicationmodal: false
        };
    },
    created() {
        AccountService.GetContractorDocumentInfo().then((res) => {
            this.InfoCard = res.data;
        });
    }
};
</script>
