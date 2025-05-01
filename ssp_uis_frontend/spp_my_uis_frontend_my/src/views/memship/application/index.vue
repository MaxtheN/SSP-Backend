<template>
    <div>
        <AppListHeaderForName title="MemshipApplication" page-name="memship">
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
                <template #cell(message)="{ item }">
                    <div v-if="item.application && item.application.statusId == 25 && item.application.message" class="d-flex align-items-center">
                        {{ item.application.message }}
                    </div>
                </template>
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
                                    name: 'MemshipApplicationEdit',
                                    params: { id: item.id }
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
import MemshipApplicationService from '@/services/memshipapplication.service';
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
                    key: 'message',
                    label: this.$t('message')
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
        MemshipApplicationService.CanCreate().then((res) => {
            this.canCreate = res.data;
        });
    },
    methods: {
        CreateApplicationEdit() {
            this.$router.push({ name: 'MemshipApplicationEdit', params: { id: 0 } });
        },
        Refresh() {
            this.Loading = true;
            MemshipApplicationService.GetList(this.filter)
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
            MemshipApplicationService.DownloadPdf(id)
                .then((res) => {
                    this.forceFileDownload(res, 'memshipapplication_' + id, 'pdf');
                })
                .catch(this.showApiError)
                .finally(() => {
                    this.downloadloading = false;
                });
        }
    }
};
</script>
