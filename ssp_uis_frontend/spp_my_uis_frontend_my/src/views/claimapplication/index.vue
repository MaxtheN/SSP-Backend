<template>
    <div>
        <AppListHeaderForName title="ClaimApplication" page-name="claim_application">
            <template #top-right>
                <b-button @click="CreateApplicationEdit" variant="info">
                    <b-icon-plus></b-icon-plus>
                    {{ $t('Add') }}
                </b-button>
            </template>
        </AppListHeaderForName>

        <div class="container-fluid">
            <CTable :items="Application" :fields="fields" bordered :busy="Loading" :filter="filter" @request="Refresh">
                <template #cell(contractor)="{ item }">
                    {{ item.application.contractor }}
                </template>
                <template #cell(contractorInn)="{ item }">
                    {{ item.application.contractorInn }}
                </template>
                <template #cell(docNumber)="{ item }">
                    {{ item.application.docNumber }}
                </template>
                <template #cell(docOn)="{ item }">
                    {{ item.application.docOn }}
                </template>

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
                                    name: 'ClaimApplicationEdit',
                                    params: { id: item.id }
                                })
                            "
                        />
                        <CButton icon="file-earmark-arrow-down" @click="Download(item.application.id2)" :disabled="downloadloading" />
                    </div>
                </template>
            </CTable>
        </div>
    </div>
</template>

<script>
import ClaimApplicationService from '@/services/claimapplication.servise';
import AppStatusBadge from '@/components/application/AppStatusBadge.vue';
import AppListHeaderForName from '@/components/application/AppListHeaderForName.vue';
import CTable from '@/components/table/CTable.vue';
import CButton from '@/components/CButton.vue';

export default {
    components: {
        CTable,
        AppStatusBadge,
        AppListHeaderForName,
        CButton
    },
    data() {
        return {
            Application: [],
            canCreate: false,
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
                    key: 'contractor',
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
    created() {
        ClaimApplicationService.CanCreate().then((res) => {
            this.canCreate = res.data;
        });
    },
    methods: {
        CreateApplicationEdit() {
            this.$router.push({ name: 'ClaimApplicationEdit', params: { id: 0 } });
        },
        Refresh() {
            this.Loading = true;
            ClaimApplicationService.GetList(this.filter)
                .then((res) => {
                    this.Application = res.data.rows;
                    this.filter.total = res.data.total;
                })
                .catch(this.showApiError)
                .finally(() => {
                    this.Loading = false;
                });
        },
        Download(id) {
            this.downloadloading = true;
            ClaimApplicationService.DownloadPdf(id)
                .then((res) => {
                    this.forceFileDownload(res, 'claimapplication_' + id, 'pdf');
                })
                .catch(this.showApiError)
                .finally(() => {
                    this.downloadloading = false;
                });
        }
    }
};
</script>
