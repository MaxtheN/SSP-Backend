<template>
    <div>
        <AppListHeaderForName title="memship" page-name="MyCabinet" />

        <div class="container-fluid text-center">
            <b-row>
                <b-col sm="12" md="4" v-if="!InfoCard.hasMemshipApplication">
                    <WCard
                        title="application_create"
                        img-src="/images/ssp_images/application-button.svg?v=2"
                        @click="$router.push({ name: 'MemshipApplicationEdit', params: { id: 0 } })"
                    />
                </b-col>
                <b-col sm="12" md="4" v-if="InfoCard.hasMemshipApplication">
                    <WCard
                        key="MemshipApplication"
                        count-url="MemshipApplication/GetCount"
                        title="MemshipApplication"
                        img-src="/images/ssp_images/application-button.svg?v=2"
                        @click="$router.push({ name: 'MemshipApplication' })"
                    />
                </b-col>
                <b-col sm="12" md="4" v-if="InfoCard.hasMemshipContract">
                    <WCard
                        key="MemshipContract"
                        title="MemshipContract"
                        count-url="Memship/MemshipContract/GetCount"
                        img-src="/images/ssp_images/contract-button.svg?v=2"
                        @click="$router.push({ name: 'MemshipContract' })"
                    />
                </b-col>
                <b-col sm="12" md="4" v-if="InfoCard.hasMemshipCertificate">
                    <WCard
                        key="MemshipCertificate"
                        count-url="MemshipCertificate/GetCount"
                        title="memshipcertificate"
                        img-src="/images/ssp_images/cretificate-button.svg?v=2"
                        @click="$router.push({ name: 'MemshipCertificate' })"
                    />
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
