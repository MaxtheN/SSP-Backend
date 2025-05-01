<template>
    <div>
        <AppListHeaderForName title="Appeal" page-name="MyCabinet" />

        <b-row class="container-fluid">
            <b-col sm="12" md="4">
                <WCard
                    count-url="appeal/AppealApplication/GetCount"
                    title="appealSend"
                    img-src="/images/ssp_images/application-button.svg"
                    @click="$router.push({ name: 'AppealApplication' })"
                />
            </b-col>
        </b-row>
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
