<template>
    <div>
        <AppListHeaderForName :title="$t('DualContract')" page-name="dual" />
        <div class="container-fluid">
            <CTable :items="list" :fields="fields" bordered :busy="Loading" :filter="filter" @request="Refresh">
                <template #cell(status)="{ item }">
                    <div class="d-flex align-items-center">
                        <AppStatusBadge :item="item" />
                    </div>
                </template>

                <template #cell(actions)="{ item }">
                    <div class="d-flex align-items-center justify-content-center">
                        <CButton
                            icon="eye"
                            class="mr-2"
                            @click="
                                $router.push({
                                    name: 'DualContractView',
                                    params: { id: item.id }
                                })
                            "
                        />
                        <CButton icon="file-earmark-arrow-down" @click="Download(item.id2)" :disabled="downloadloading" />
                    </div>
                </template>
            </CTable>
        </div>
    </div>
</template>

<script>
import AppListHeaderForName from '@/components/application/AppListHeaderForName.vue';
import WTextarea from '@/components/forms/WTextarea.vue';
import DualContractService from '@/services/dual/dualcontract.service';
import AppStatusBadge from '@/components/application/AppStatusBadge.vue';
import AppCard from '@/components/application/AppCard.vue';

import CTable from '@/components/table/CTable.vue';
import CButton from '@/components/CButton.vue';

export default {
    components: {
        AppStatusBadge,
        CTable,
        AppCard,
        AppListHeaderForName,
        WTextarea,
        CButton
    },
    data() {
        return {
            list: [],
            Loading: false,
            downloadloading: false,
            filter: {
                search: null,
                sortBy: null,
                orderType: null,
                page: 1,
                pageSize: 10,
                total: 0
            },
            fields: [
                {
                    key: 'contracttorFullName',
                    label: this.$t('contractor')
                },
                {
                    key: 'contractorInn',
                    label: this.$t('inn')
                },
                {
                    key: 'docNumber',
                    label: this.$t('docNumberApplication')
                },
                {
                    key: 'docOn',
                    label: this.$t('docDateApplication')
                },

                {
                    key: 'status',
                    label: this.$t('status')
                },

                {
                    key: 'actions',
                    label: this.$t('actions')
                }
            ]
        };
    },
    methods: {
        Refresh() {
            this.Loading = true;
            DualContractService.GetList(this.filter)
                .then((res) => {
                    this.list = res.data.rows;
                    this.filter.total = res.data.total;
                })
                .catch(this.showApiError)
                .finally(() => {
                    this.Loading = false;
                });
        },
        Download(id) {
            this.downloadloading = true;
            DualContractService.DualContractIntegrationDownloadPdf(id)
                .then((res) => {
                    this.forceFileDownload(res, this.$t('DualContract') + '_' + id, 'pdf');
                })
                .catch(this.showApiError)
                .finally(() => {
                    this.downloadloading = false;
                });
        }
    }
};
</script>
