<template>
    <div>
        <AppListHeaderForName title="ServiceContract" page-name="Srv" :query="{ type: $route.query.type }"> </AppListHeaderForName>
        <div class="container-fluid">
            <CTable :is-pagination="true" :items="Contracts" :fields="fields" bordered :busy="Loading" :filter="filter" @request="Refresh">
                <template #cell(status)="{ item }">
                    <div class="d-flex align-items-center justify-content-center">
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
                                    name: 'ServiceContractEdit',
                                    params: { id: item.id },
                                    query: { type: $route.query.type }
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
import WTextarea from '@/components/forms/WTextarea.vue';
import ServiceContractService from '@/services/srv/servicecontract.service';
import AppStatusBadge from '@/components/application/AppStatusBadge.vue';
import AppCard from '@/components/application/AppCard.vue';
import AppListHeaderForName from '@/components/application/AppListHeaderForName.vue';
import AppTable from '@/components/application/AppTable.vue';
import CTable from '@/components/table/CTable.vue';
import CButton from '@/components/CButton.vue';

export default {
    components: {
        AppStatusBadge,
        AppCard,
        WTextarea,
        AppListHeaderForName,
        AppTable,
        CTable,
        CButton
    },
    data() {
        return {
            Contracts: [],
            dataFilter: {
                search: null,
                sortBy: null,
                orderType: null,
                page: 1,
                pageSize: 20
            },
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
                    key: 'contractorFullName',
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
        CreateContractEdit() {
            this.$router.push({ name: 'SrvList', query: { type: this.$route.query.type } });
        },
        Refresh() {
            this.Loading = true;
            ServiceContractService.GetList(this.filter)
                .then((res) => {
                    this.Contracts = res.data.rows;
                    this.filter.total = res.data.total;
                })
                .catch(this.showApiError)
                .finally(() => {
                    this.Loading = false;
                });
        },
        Download(id) {
            this.downloadloading = true;
            ServiceContractService.DownloadPdf(id)
                .then((res) => {
                    this.forceFileDownload(res, this.$t('ServiceContract') + '_' + id, 'pdf');
                })
                .catch(this.showApiError)
                .finally(() => {
                    this.downloadloading = false;
                });
        }
    }
};
</script>
