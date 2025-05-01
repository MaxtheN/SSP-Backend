<template>
    <b-tabs class="memship-tabs" v-model="tab">
        <b-tab :title="$t('MemshipContract')" lazy active :title-link-class="tab == 0 ? 'bg-primary' : 'text-primary bg-white'">
            <AppListHeaderForName title="MemshipContract" page-name="memship" />
            <div class="container-fluid">
                <MemshipContractList />
            </div>
        </b-tab>
        <b-tab v-if="hasAdditionalAgreement" :title="$t('AdditionalAgreement')" lazy :title-link-class="tab == 1 ? 'bg-primary' : 'text-primary bg-white'">
            <AppListHeaderForName title="AdditionalAgreement" page-name="memship" />
            <div class="container-fluid">
                <AdditionalAgreementList />
            </div>
        </b-tab>
    </b-tabs>
</template>

<script>
import MemshipContractList from './index.vue';
import AdditionalAgreementList from '@/views/additionalagreement/index.vue';
import AppListHeaderForName from '@/components/application/AppListHeaderForName.vue';
import AccountService from '@/services/account.service';

export default {
    components: {
        AdditionalAgreementList,
        MemshipContractList,
        AppListHeaderForName
    },
    data() {
        return {
            tab: 0,
            hasAdditionalAgreement: false
        };
    },
    created() {
        this.tab = this.$route.query.tab || 0;

        AccountService.GetContractorDocumentInfo().then((res) => {
            this.hasAdditionalAgreement = res.data?.hasAdditionalAgreement;
        });
    }
};
</script>

<style>
.memship-tabs .nav-link {
    color: var(--primary) !important;
}

.memship-tabs .nav-link.active {
    color: var(--white) !important;
}
</style>
