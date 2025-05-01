<template>
    <div>
        <AppListHeaderForName title="memshipcertificate" page-name="memship" />
        <div class="container-fluid">
            <CTable :items="Certificate" :fields="fields" bordered :busy="Loading" :filter="filter" @request="Refresh">
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
                                    name: 'MemshipCertificateEdit',
                                    params: { id: item.id }
                                })
                            "
                        />
                        <CButton icon="file-earmark-arrow-down" :disabled="downloadloading" @click="DownloadPDF(item.id2)" />
                    </div>
                </template>
            </CTable>
        </div>
    </div>
</template>

<script>
import axios from 'axios';
import MemshipCertificateService from '@/services/memshipcertificate.service';
import AppStatusBadge from '@/components/application/AppStatusBadge.vue';
import AppCard from '@/components/application/AppCard.vue';
import AppListHeaderForName from '@/components/application/AppListHeaderForName.vue';
import AppTable from '@/components/application/AppTable.vue';
import CTable from '@/components/table/CTable.vue';
import CButton from '@/components/CButton.vue';
export default {
    components: { AppStatusBadge, AppCard, AppListHeaderForName, AppTable, CTable, CButton },
    data() {
        return {
            axios,
            Loading: false,
            downloadloading: false,
            Certificate: [],
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
                    key: 'expireOn',
                    label: this.$t('expireOnCertificate')
                },
                {
                    key: 'prtnContractType',
                    label: this.$t('type')
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
        DownloadPDF(id2) {
            this.downloadloading = true;
            MemshipCertificateService.DownloadPdf(id2)
                .then((res) => {
                    this.forceFileDownload(res, this.$t('memshipcertificate'), 'pdf');
                })
                .catch(this.showApiError)
                .finally(() => {
                    this.downloadloading = false;
                });
        },
        Refresh() {
            this.Loading = true;
            MemshipCertificateService.GetList(this.filter)
                .then((res) => {
                    this.Certificate = res.data.rows;
                })
                .catch((error) => {
                    this.showApiError(error);
                })
                .finally(() => {
                    this.Loading = false;
                });
        }
    }
};
</script>
