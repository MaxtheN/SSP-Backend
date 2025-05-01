<template>
    <div>
        <AppListHeaderForName title="PartnershipCertificate" />

        <div class="container-fluid">
            <CTable :items="Certificate" :fields="fields" bordered @request="Refresh" :busy="Loading">
                <template #cell(status)="{ item }">
                    <AppStatusBadge :item="item" />
                </template>
                <template #cell(actions)="{ item }">
                    <div class="d-flex align-items-center justify-content-center">
                        <CButton
                            class="mr-2"
                            icon="eye"
                            @click="
                                $router.push({
                                    name: 'PartnershipCertificateEdit',
                                    params: { id: item.id }
                                })
                            "
                        />

                        <CButton
                            target="_blank"
                            :href="axios.defaults.baseURL + `PrtnCertificate/PrintCertificatePdf?Id2=${item.id2}&lang=${getPdfLang()}`"
                            icon="file-earmark-arrow-down"
                        />
                    </div>
                </template>
            </CTable>
        </div>
    </div>
</template>

<script>
import axios from 'axios';
import PrtnCertificateService from '@/services/prtncertificate.service';
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
            Certificate: [],
            filter: {
                search: null,
                sortBy: null,
                orderType: null,
                page: 1,
                pageSize: 20,
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
                    label: this.$t('docNumberCertificate')
                },
                {
                    key: 'docOn',
                    label: this.$t('docOnCertificate')
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
            ]
        };
    },
    methods: {
        Refresh() {
            this.Loading = true;
            PrtnCertificateService.GetList(this.filter)
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
