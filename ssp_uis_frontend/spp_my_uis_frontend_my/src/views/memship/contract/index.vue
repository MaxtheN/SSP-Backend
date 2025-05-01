<template>
    <div>
        <CTable :items="ContractList" :fields="fields" bordered :busy="Loading" :filter="filter" @request="Refresh">
            <template #cell(status)="{ item }">
                <div class="d-flex align-items-center">
                    <AppStatusBadge :item="item" />
                </div>
            </template>

            <template #cell(actions)="{ item }">
                <div class="d-flex align-items-center justify-content-center">
                    <CButton
                        class="mr-2"
                        icon="eye"
                        @click="
                            $router.push({
                                name: 'MemshipContractEdit',
                                params: { id: item.id }
                            })
                        "
                    />
                    <CButton icon="file-earmark-arrow-down" @click="DownloadFile(item.id2)" :disabled="downloadloading" />
                </div>
            </template>
        </CTable>
    </div>
</template>

<script>
import MemshipContractService from '@/services/memshipcontract.service';
import AppStatusBadge from '@/components/application/AppStatusBadge.vue';
import AppCard from '@/components/application/AppCard.vue';
import AppTable from '@/components/application/AppTable.vue';
import CTable from '@/components/table/CTable.vue';
import CButton from '@/components/CButton.vue';

export default {
    components: { AppStatusBadge, AppCard, AppTable, CTable, CButton },
    data() {
        return {
            DeleteLoading: false,
            Loading: false,
            downloadloading: false,
            ContractList: [],
            fields: [
                {
                    key: 'contractor',
                    label: this.$t('contractor')
                },
                {
                    key: 'contractorInn',
                    label: this.$t('inn')
                },
                {
                    key: 'docNumber',
                    label: this.$t('docNumber')
                },
                {
                    key: 'docOn',
                    label: this.$t('docdate')
                },
                {
                    key: 'status',
                    label: this.$t('status')
                },

                {
                    key: 'actions',
                    label: this.$t('actions')
                }
            ],
            filter: {
                search: null,
                sortBy: null,
                orderType: null,
                page: 1,
                pageSize: 20,
                total: 0
            }
        };
    },
    methods: {
        DownloadFile(id) {
            this.downloadloading = true;
            MemshipContractService.DownloadPdf(id)
                .then((res) => {
                    this.forceFileDownload(res, 'memshipcontract_' + id, 'pdf');
                })
                .catch(this.showApiError)
                .finally(() => {
                    this.downloadloading = false;
                });
        },
        Refresh() {
            this.Loading = true;
            MemshipContractService.GetList(this.filter)
                .then((res) => {
                    this.ContractList = res.data.rows;
                    this.filter.total = res.data.total;
                })
                .catch(this.showApiError)
                .finally(() => {
                    this.Loading = false;
                });
        }
    }
};
</script>
