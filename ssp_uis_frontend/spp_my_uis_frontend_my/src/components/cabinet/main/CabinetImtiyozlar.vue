<template>
    <MainCard :title="$t('Mavjud imtiyozlar')" class="py-0">
        <b-skeleton-table v-if="loading" :rows="5" :columns="4" :table-props="{ bordered: true, striped: true }"></b-skeleton-table>
        <table v-else class="table">
            <thead class="bg-grey border-0">
                <tr>
                    <th>№</th>
                    <th>{{ $t('contractor') }}</th>
                    <th>{{ $t('status') }}</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="(b, i) in benefits" :key="i">
                    <td>{{ i + 1 }}</td>
                    <td>{{ b.organizationName }}</td>
                    <td>{{ b.status }}</td>
                </tr>
            </tbody>
        </table>
        <template #actions>
            <!-- <DownloadButton /> -->
        </template>
    </MainCard>
</template>

<script>
import MainCard from '@/components/cabinet/MainCard.vue';
import DownloadButton from '@/components/cabinet/DownloadButton.vue';
import ApplicationService from '@/services/application.service';

export default {
    components: {
        MainCard,
        DownloadButton
    },
    data() {
        return {
            loading: false,
            benefits: []
        };
    },
    created() {
        this.GetCompanyCriteries();
    },
    methods: {
        GetCompanyCriteries() {
            this.loading = true;
            ApplicationService.GetBenefits()
                .then((res) => {
                    this.benefits = res.data;
                })
                .catch(this.showApiError)
                .finally(() => {
                    this.loading = false;
                });
        }
    }
};
</script>
