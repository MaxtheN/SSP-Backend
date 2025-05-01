<template>
    <div>
        <AppListHeaderForName @changeType="handletype" :type="type" :title="title" page-name="MyCabinet" />

        <div class="container-fluid">
            <b-row v-if="type == null">
                <b-col sm="12" md="4">
                    <WCard title="All" img-src="/images/ssp_images/contract-button.svg?v=2" @click="changeType(1)" />
                </b-col>
                <b-col sm="12" md="4">
                    <WCard title="Price" img-src="/images/ssp_images/contract-button.svg?v=2" @click="changeType(2)" />
                </b-col>
                <b-col sm="12" md="4">
                    <WCard title="Free" img-src="/images/ssp_images/contract-button.svg?v=2" @click="changeType(3)" />
                </b-col>
            </b-row>
            <b-row>
                <b-col sm="12" md="4" v-if="type != null">
                    <WCard
                        :count-url="type == 3 ? 'srv/ServiceApplication/GetFreeCount' : type == 1 ? 'srv/ServiceApplication/GetCount' : 'srv/ServiceApplication/GetPayedCount'"
                        title="application"
                        img-src="/images/ssp_images/application-button.svg?v=2"
                        @click="$router.push({ name: 'ServiceApplication', query: { type: type } })"
                    />
                </b-col>
                <b-col sm="12" md="4" v-if="type != 3 && type != null">
                    <WCard
                        count-url="srv/ServiceContract/GetCount"
                        title="contract"
                        img-src="/images/ssp_images/contract-button.svg?v=2"
                        @click="$router.push({ name: 'ServiceContract', query: { type: type } })"
                    />
                </b-col>
                <b-col sm="12" md="4" v-if="type != 3 && type != null">
                    <WCard
                        title="servicedeed"
                        count-url="srv/ServiceDeed/GetCount"
                        img-src="/images/ssp_images/contract-button.svg?v=2"
                        @click="$router.push({ name: 'ServiceDeed', query: { type: type } })"
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
    data() {
        return {
            type: null,
            title: 'ServiceApplication'
        };
    },
    methods: {
        changeType(type) {
            this.type = type;
        },
        handletype(e) {
            console.log(e);
            this.type = null;
        }
    },
    watch: {
        type(newVal) {
            if (newVal == null) {
                this.title = 'ServiceApplication';
            } else if (newVal == 1) {
                this.title = 'All';
            } else if (newVal == 2) {
                this.title = 'Price';
            } else if (newVal == 3) {
                this.title = 'Free';
            }
        }
    }
};
</script>
