<template>
    <div>
        <AppListHeaderForName title="HakamlikSudi" page-name="MyCabinet" />

        <div class="container-fluid">
            <b-row>
                <b-col sm="12" md="4" v-if="false">
                    <WCard
                        title="application_create"
                        img-src="/images/ssp_images/application-button.svg"
                        @click="
                            $router.push({
                                name: 'ArbitrationCourtApplicationEdit',
                                params: { id: 0 }
                            })
                        "
                    />
                </b-col>
                <b-col sm="12" md="4">
                    <WCard
                        title="application"
                        count-url="ArbitrationCourtApplication/GetCount"
                        img-src="/images/ssp_images/application-button.svg"
                        @click="$router.push({ name: 'ArbitrationCourtApplication' })"
                    />
                </b-col>
                <b-col sm="12" md="4">
                    <WCard title="process" img-src="/images/ssp_images/application-button.svg" @click="$router.push({ name: 'ApplicationProcess' })" />
                </b-col>
                <b-col sm="12" md="4" v-if="false">
                    <WCard title="contract" img-src="/images/ssp_images/contract-button.svg" @click="$router.push({ name: 'MemshipContract' })" />
                </b-col>
                <b-col sm="12" md="4" v-if="false">
                    <WCard title="certificate" img-src="/images/ssp_images/cretificate-button.svg" @click="$router.push({ name: 'MemshipCertificate' })" />
                </b-col>
            </b-row>
        </div>
    </div>
</template>

<script>
import AccountService from '@/services/account.service';
import customButton from '@/components/elements/customButton.vue';
import AppListHeaderForName from '@/components/application/AppListHeaderForName.vue';
import WCard from '@/components/card/WCard.vue';
export default {
    data() {
        return {
            InfoCard: {},
            GetInfoData: {}
        };
    },
    created() {
        AccountService.GetContractorDocumentInfo().then((res) => {
            this.InfoCard = res.data;
        });
    },
    components: {
        customButton,
        AppListHeaderForName,
        WCard
    }
};
</script>
