<template>
    <div>
        <AppListHeaderForName title="MonoApplication" page-name="Mono">
            <template #top-right>
                <b-button v-if="1 || canCreate" @click="CreateApplicationEdit" variant="info">
                    <b-icon-plus></b-icon-plus>
                    {{ $t('Add') }}
                </b-button>
            </template>
        </AppListHeaderForName>
        <div class="container-fluid">
            <CTable :items="Application" :fields="fields" bordered :filter="filter" :is-pagination="true" @request="Refresh" :busy="isBusy">
                <template #cell(contractor)="{ item }">
                    {{ item.application.contractor }}
                </template>
                <template #cell(inn)="{ item }">
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
                            icon="eye"
                            class="mr-2"
                            @click="
                                $router.push({
                                    name: 'MonoApplicationEdit',
                                    params: { id: item.id }
                                })
                            "
                        />
                        <CButton icon="file-earmark" @click="OpenInfo(item.id)" />
                    </div>
                </template>
            </CTable>

            <b-modal v-model="InfoModal" :title="$t('Info')" hide-footer>
                <h5>{{ $t('responsibleFio') }} : {{ InfoList.responsibleFio }}</h5>
                <h5>{{ $t('phone') }} : {{ InfoList.responsiblePhone }}</h5>
                <h5 v-if="InfoList.rejectReason">{{ $t('rejectReason') }} : {{ InfoList.rejectReason }}</h5>
                <h5 v-if="InfoList.subsidyAmount">{{ $t('subsidyAmount') }} : {{ InfoList.subsidyAmount }}</h5>
            </b-modal>
        </div>
    </div>
</template>

<script>
import WTextarea from '@/components/forms/WTextarea.vue';
import MonoApplicationService from '@/services/mono/monoapplication.service';
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
            InfoList: {},
            InfoModal: false,
            canCreate: false,
            isBusy: false,
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
                    key: 'inn',
                    label: this.$t('contractorInn')
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
        MonoApplicationService.CanCreate().then((res) => {
            this.canCreate = res.data;
        });
    },
    methods: {
        OpenInfo(id) {
            MonoApplicationService.GetByMonoAppId(id)
                .then((res) => {
                    this.InfoList = res.data;
                    this.InfoModal = true;
                })
                .catch(this.showApiError)
                .finally(() => {
                    this.Loading = false;
                });
        },
        CreateApplicationEdit() {
            this.$router.push({ name: 'MonoApplicationEdit', params: { id: 0 } });
        },
        Refresh() {
            this.Loading = true;
            this.isBusy = true;
            MonoApplicationService.GetList(this.filter)
                .then((res) => {
                    this.Application = res.data.rows;
                    this.filter.total = res.data.total;
                })
                .catch(this.showApiError)
                .finally(() => {
                    this.Loading = false;
                    this.isBusy = false;
                });
        },
        Download(id) {
            this.downloadloading = true;
            MonoApplicationService.DownloadPdf(id, this.getPdfLang())
                .then((res) => {
                    this.forceFileDownload(res, 'MonoApplication_' + id, 'pdf');
                })
                .catch(this.showApiError)
                .finally(() => {
                    this.downloadloading = false;
                });
        }
    }
};
</script>
