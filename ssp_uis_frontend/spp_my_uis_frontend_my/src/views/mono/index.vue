<template>
    <div>
        <AppListHeaderForName title="MonoApplication" />
        <div class="container-fluid">
            <b-row>
                <b-col sm="12" md="4">
                    <WCard title="application" img-src="/images/ssp_images/application-button.svg?v=2" @click="$router.push({ name: 'MonoApplication' })" />
                </b-col>
                <b-col sm="12" md="4" v-if="0">
                    <WCard title="contract" img-src="/images/ssp_images/contract-button.svg?v=2" @click="$router.push({ name: 'MonoContract' })" />
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
    }
};
</script>
