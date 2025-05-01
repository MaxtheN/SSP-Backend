<template>
    <div>
        <AppListHeaderForName title="appealSend" page-name="Appeal">
            <template #top-right>
                <b-button @click="CreateApplicationEdit" variant="info">
                    <b-icon-plus></b-icon-plus>
                    {{ $t('Add') }}
                </b-button>
            </template>
        </AppListHeaderForName>
        <div class="container-fluid">
            <CTable :items="Application" :fields="fields" bordered :busy="Loading" :filter="dataFilter" @request="Refresh">
                <template #cell(status)="{ item }">
                    <div class="d-flex align-items-center justify-content-center">
                        <AppStatusBadge :item="item" />
                    </div>
                </template>

                <template #cell(actions)="{ item }">
                    <div class="d-flex align-items-center justify-content-center">
                        <CButton
                            icon="eye"
                            @click="
                                $router.push({
                                    name: 'AppealApplicationEdit',
                                    params: { id: item.id },
                                    query: { type: $route.query.type }
                                })
                            "
                        />
                    </div>
                </template>
            </CTable>
        </div>
    </div>
</template>

<script>
import WTextarea from '@/components/forms/WTextarea.vue';
import AppealApplicationService from '@/services/appeal/AppealApplication.service';
import AppStatusBadge from '@/components/application/AppStatusBadge.vue';
import AppListHeaderForName from '@/components/application/AppListHeaderForName.vue';
import CTable from '@/components/table/CTable.vue';
import CButton from '@/components/CButton.vue';

export default {
    components: {
        AppStatusBadge,
        WTextarea,
        AppListHeaderForName,
        CTable,
        CButton
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
            this.$router.push({ name: 'AppealApplicationEdit', params: { id: 0 } });
        },
        Refresh() {
            this.Loading = true;
            AppealApplicationService.GetList(this.filter)
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
            AppealApplicationService.DownloadPdf(id)
                .then((res) => {
                    this.forceFileDownload(res, 'AppealApplication_' + id, 'pdf');
                })
                .catch(this.showApiError)
                .finally(() => {
                    this.downloadloading = false;
                });
        }
    }
};
</script>
