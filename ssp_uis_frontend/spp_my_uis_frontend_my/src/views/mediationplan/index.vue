<template>
    <div class="container">
        <AppListHeaderForName title="MediationPlan" page-name="claim_application" />

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
                                <td style="width: 40%">{{ $t('documentnumber') }} :</td>
                                <th style="width: 60%">{{ item.docNumber }}</th>
                            </tr>
                            <tr class="py-3">
                                <td style="width: 40%">{{ $t('docdate') }} :</td>
                                <th style="width: 60%">{{ item.docOn }}</th>
                            </tr>
                            <tr class="py-3">
                                <td style="width: 40%">{{ $t('meetingType') }} :</td>
                                <th style="width: 60%">{{ item.meetingType }}</th>
                            </tr>
                            <tr class="my-1">
                                <td style="width: 40%">{{ $t('meditionAt') }} :</td>
                                <th style="width: 60%">
                                    {{ item.meditionAt }}
                                </th>
                            </tr>
                            <tr class="my-1">
                                <td style="width: 40%">{{ $t('addressOrUrl') }} :</td>
                                <th style="width: 60%">
                                    {{ item.addressOrUrl }}
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
                </AppCard>
            </template>
        </AppTable>
    </div>
</template>

<script>
import axios from 'axios';
import MediationPlanService from '@/services/mediationplan.service';
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
            MediationPlanService.DownloadPdf(id2, this.getPdfLang())
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
            MediationPlanService.GetList(this.filter)
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
