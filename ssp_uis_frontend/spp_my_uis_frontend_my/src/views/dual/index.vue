<template>
    <div>
        <AppListHeaderForName :title="$t('Dual')" page-name="MyCabinet" />

        <div class="container-fluid">
            <b-row>
                <b-col sm="12" md="4">
                    <WCard
                        title="DualApplication"
                        count-url="/Dual/DualApplication/GetCount"
                        img-src="/images/ssp_images/application-button.svg?v=2"
                        @click="$router.push({ name: 'DualApplication' })"
                    />
                </b-col>
                <b-col sm="12" md="4">
                    <WCard
                        title="DualContract"
                        count-url="Dual/DualContract/GetCount"
                        img-src="/images/ssp_images/application-button.svg?v=2"
                        @click="$router.push({ name: 'DualContract' })"
                    />
                </b-col>
                <b-col sm="12" md="4" v-if="isDev">
                    <WCard
                        title="SubsidyRequest"
                        count-url="Dual/SubsidyRequest/GetCount"
                        img-src="/images/ssp_images/contract-button.svg?v=2"
                        @click="$router.push({ name: 'SubsidyRequest' })"
                    />
                </b-col>
            </b-row>
        </div>
    </div>
</template>

<script>
import customButton from '@/components/elements/customButton.vue';
import AppListHeaderForName from '@/components/application/AppListHeaderForName.vue';
import WCard from '@/components/card/WCard.vue';
export default {
    components: {
        customButton,
        AppListHeaderForName,
        WCard
    },
    computed: {
        isDev() {
            return window.location.hostname != 'my.chamber.uz' || false;
        }
    }
};
</script>
