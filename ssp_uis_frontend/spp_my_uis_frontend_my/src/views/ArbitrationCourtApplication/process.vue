<template>
    <div>
        <AppListHeaderForName title="ApplicationProcess" page-name="HakamlikSudi"> </AppListHeaderForName>
        <div class="container-fluid">
            <CTable :is-pagination="true" :items="Application" :fields="fields" bordered :busy="Loading" :filter="dataFilter" @request="Refresh">
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
                                    name: 'ArbitrationCourtApplicationEdit',
                                    params: { id: item.id },
                                    query: { isProcess: true }
                                })
                            "
                        />
                        <CButton icon="file-earmark-arrow-down" class="mr-2" @click="Download(item.application.id2)" :disabled="downloadloading" />
                        <CButton
                            v-if="item.canSelectJudge"
                            icon="person-plus"
                            @click="
                                $router.push({
                                    name: 'SelectJudge',
                                    params: { id: item.id },
                                    query: { isProcess: true }
                                })
                            "
                            :disabled="downloadloading"
                        />
                    </div>
                </template>
            </CTable>

            <b-sidebar no-header width="400px" shadow right v-model="historySidebar" bg-variant="white">
                <div style="width: 100%; height: 100%">
                    <div class="container-fluid w-100" style="width: 100% !important; position: relative; overflow-y: auto">
                        <b-row class="w-100">
                            <b-col class="text-right close-icon">
                                <b-icon-x scale="2.5" style="cursor: pointer; z-index: 9" @click="historySidebar = false"></b-icon-x>
                            </b-col>
                        </b-row>
                        <b-row class="p-0" style="height: 80vh !important">
                            <b-col v-for="(item, index) in chatData" :key="index">
                                {{ item }}
                            </b-col>
                        </b-row>
                    </div>
                </div>
            </b-sidebar>
        </div>
    </div>
</template>

<script>
import WTextarea from '@/components/forms/WTextarea.vue';
import AppStatusBadge from '@/components/application/AppStatusBadge.vue';
import AppListHeaderForName from '@/components/application/AppListHeaderForName.vue';
import ArbitrationCourtApplicationService from '@/services/hakamliksudi/arbitrationcourtapplication.service';
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

            chatFilter: {
                tableId: 100,
                documentId: 0,
                search: '',
                sortBy: '',
                orderType: '',
                page: 1,
                pageSize: 1000
            },
            dataFilter: {
                search: null,
                sortBy: null,
                orderType: null,
                page: 1,
                pageSize: 20
            },
            historySidebar: false,
            chatData: [],
            chatLoading: false,
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

    methods: {
        moveStep(stepNumber) {
            this.currentStep = stepNumber;
        },
        calculateBarPosition() {
            let docEl = document.documentElement;
            const first = document.getElementById('step-1');
            let rect = first.getBoundingClientRect();
            const offset = rect.left + (window.scrollX || docEl.scrollLeft || 0);
            const top = rect.top + rect.height / 2 - 2;
            this.pBarSize = `left: ${0}px; right: ${0}px;`;
        },

        CreateApplicationEdit() {
            this.$router.push({
                name: 'ArbitrationCourtApplicationEdit',
                params: { id: 0 }
            });
        },
        Refresh() {
            this.Loading = true;
            ArbitrationCourtApplicationService.GetList(this.filter)
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
            ArbitrationCourtApplicationService.DownloadPdf(id)
                .then((res) => {
                    this.forceFileDownload(res, this.$t('ArbitrationCourtApplication'), 'pdf');
                })
                .catch(this.showApiError)
                .finally(() => {
                    this.downloadloading = false;
                });
        }
    },
    created() {}
};
</script>
