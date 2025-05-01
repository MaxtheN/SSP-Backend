<template>
    <div>
        <AppListHeaderForName title="ServiceApplication" page-name="Srv" :query="{ type: $route.query.type }">
            <template #top-right>
                <b-button @click="CreateApplicationEdit" variant="info">
                    <b-icon-plus></b-icon-plus>
                    {{ $t('Add') }}
                </b-button>
            </template>
        </AppListHeaderForName>
        <div class="container-fluid">
            <CTable :is-pagination="true" :items="Application" :fields="fields" bordered :busy="Loading" :filter="filter" @request="Refresh">
                <template #cell(status)="{ item }">
                    <div class="d-flex align-items-center justify-content-center">
                        <AppStatusBadge :item="item" />
                    </div>
                </template>
                <template #cell(contractor)="{ item }">
                    {{ item.application.contractor }}
                </template>
                <template #cell(docNumber)="{ item }">
                    {{ item.application.docNumber }}
                </template>
                <template #cell(contractorInn)="{ item }">
                    {{ item.application.contractorInn }}
                </template>
                <template #cell(docOn)="{ item }">
                    {{ item.application.docOn }}
                </template>

                <template #cell(actions)="{ item }">
                    <div class="d-flex align-items-center justify-content-center">
                        <CButton
                            class="mr-2"
                            icon="eye"
                            @click="
                                $router.push({
                                    name: 'ServiceApplicationEdit',
                                    params: { id: item.id },
                                    query: { type: $route.query.type }
                                })
                            "
                        />
                        <CButton icon="file-earmark-arrow-down" @click="Download(item.application.id2)" />
                    </div>
                </template>
            </CTable>
        </div>
    </div>
</template>

<script>
import WTextarea from '@/components/forms/WTextarea.vue';
import ServiceApplicationService from '@/services/srv/serviceapplication.service';
import AppStatusBadge from '@/components/application/AppStatusBadge.vue';
import AppCard from '@/components/application/AppCard.vue';
import AppListHeaderForName from '@/components/application/AppListHeaderForName.vue';

import CTable from '@/components/table/CTable.vue';
import CButton from '@/components/CButton.vue';

export default {
    components: {
        AppStatusBadge,
        AppCard,
        WTextarea,
        AppListHeaderForName,
        CButton,
        CTable
    },
    data() {
        return {
            Application: [],
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
                isFree: null,
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
    methods: {
        CreateApplicationEdit() {
            this.$router.push({ name: 'SrvList', query: { type: this.$route.query.type } });
        },
        Refresh() {
            this.Loading = true;
            this.filter.isFree = this.$route.query.type == 2 ? false : this.$route.query.type == 3 ? true : null;
            ServiceApplicationService.GetList(this.filter)
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
            ServiceApplicationService.DownloadPdf(id, this.gePdflang())
                .then((res) => {
                    this.forceFileDownload(res, 'Serviceapplication_' + id, 'pdf');
                })
                .catch(this.showApiError)
                .finally(() => {
                    this.downloadloading = false;
                });
        }
    }
};
</script>
