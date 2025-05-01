<template>
    <div class="container">
        <AppListHeaderForName title="ClaimApplication" page-name="MyCabinet" />

        <div class="text-center" style="margin-top: 20px">
            <b-row style="margin-top: 60px">
                <b-col sm="12" md="4">
                    <WCard title="application" img-src="/images/ssp_images/application-button.svg" @click="$router.push({ name: 'ClaimApplication' })" />
                </b-col>
                <b-col sm="12" md="4">
                    <WCard title="MediationPlan" img-src="/images/ssp_images/contract-button.svg" @click="$router.push({ name: 'MediationPlan' })" />
                </b-col>
                <b-col sm="12" md="4">
                    <WCard title="Mediation" img-src="/images/ssp_images/contract-button.svg" @click="$router.push({ name: 'Mediation' })" />
                </b-col>
                <b-col sm="12" md="4" class="mt-3">
                    <WCard title="ApplicationForCourt" img-src="/images/ssp_images/contract-button.svg" @click="$router.push({ name: 'ApplicationForCourt' })" />
                </b-col>
            </b-row>
        </div>
    </div>
</template>

<script>
import AccountService from '@/services/account.service';
import AppListHeaderForName from '@/components/application/AppListHeaderForName.vue';
import WCard from '@/components/card/WCard.vue';
export default {
    components: {
        AppListHeaderForName,
        WCard
    },
    data() {
        return {
            InfoCard: {}
        };
    },
    created() {
        AccountService.GetContractorDocumentInfo().then((res) => {
            this.InfoCard = res.data;
        });
    }
};
</script>
