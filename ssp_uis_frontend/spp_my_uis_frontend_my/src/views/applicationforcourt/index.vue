<template>
    <div class="container">
        <AppListHeaderForName title="ApplicationForCourt" page-name="claim_application" />

        <AppTable :busy="Loading" :items="Certificate" :filter="filter" @request="Refresh">
            <template #item="{ item }">
                <AppCard>
                    <template #body>
                        <table>
                            <tr>
                                <td style="width: 40%">{{ $t('contractor') }} :</td>
                                <th style="width: 60%">{{ item.contractor }}</th>
                            </tr>
                            <tr>
                                <td style="width: 40%">{{ $t('chamberPerson') }} :</td>
                                <th style="width: 60%">{{ item.chamberPerson }}</th>
                            </tr>
                            <tr class="py-3">
                                <td style="width: 40%">{{ $t('docNumberCertificate') }} :</td>
                                <th style="width: 60%">{{ item.docNumber }}</th>
                            </tr>
                            <tr class="py-3">
                                <td style="width: 40%">{{ $t('docOnCertificate') }} :</td>
                                <th style="width: 60%">{{ item.docOn }}</th>
                            </tr>
                            <tr class="py-3">
                                <td style="width: 40%">{{ $t('mediationPlanDocNumber') }} :</td>
                                <th style="width: 60%">{{ item.mediationPlanDocNumber }}</th>
                            </tr>
                            <tr class="py-3">
                                <td style="width: 40%">{{ $t('mediationResult') }} :</td>
                                <th style="width: 60%">{{ item.mediationResult }}</th>
                            </tr>
                            <tr class="py-3" v-if="item.courtAt">
                                <td style="width: 40%">{{ $t('courtAt') }} :</td>
                                <th style="width: 60%">{{ item.courtAt }}</th>
                            </tr>
                            <tr class="my-1">
                                <td style="width: 40%">{{ $t('claimNeedCourt') }} :</td>
                                <th style="width: 60%">
                                    {{ item.claimNeedCourt }}
                                </th>
                            </tr>
                            <tr>
                                <td class="pr-btn" style="width: 40%">{{ $t('status') }} :</td>
                                <th style="width: 60%">
                                    <AppStatusBadge :item="item" />
                                </th>
                            </tr>
                        </table>
                    </template>
                    <template #footer>
                        <b-button @click="DownloadPDF(item.id2)" :disabled="downloadloading" size="sm" download variant="success">
                            <b-icon-download scale="0.8"></b-icon-download>
                            {{ $t('download') }}
                        </b-button>
                    </template>
                </AppCard>
            </template>
        </AppTable>
    </div>
</template>

<script>
import axios from 'axios';
import ApplicationForCourtService from '@/services/applicationforcourt.service';
import AppStatusBadge from '@/components/application/AppStatusBadge.vue';
import AppCard from '@/components/application/AppCard.vue';
import AppListHeaderForName from '@/components/application/AppListHeaderForName.vue';
import AppTable from '@/components/application/AppTable.vue';

export default {
    components: { AppStatusBadge, AppCard, AppListHeaderForName, AppTable },
    data() {
        return {
            axios,
            Loading: false,
            downloadloading: false,
            Certificate: [],
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
            ApplicationForCourtService.DownloadPdf(id2)
                .then((res) => {
                    this.forceFileDownload(res, this.$t('applicationforcourt'), 'pdf');
                })
                .catch(this.showApiError)
                .finally(() => {
                    this.downloadloading = false;
                });
        },
        Refresh() {
            this.Loading = true;
            ApplicationForCourtService.GetList(this.filter)
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
