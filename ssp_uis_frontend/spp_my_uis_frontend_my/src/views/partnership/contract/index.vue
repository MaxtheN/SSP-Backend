<template>
    <div>
        <AppListHeaderForName title="PartnershipContract" />
        <div class="container-fluid">
            <CTable bordered responsive :empty-text="$t('NotFound')" v-bind="$attrs" v-on="$listeners" :items="Contract" :fields="fields" :busy="isBusy">
                <template #cell(actions)="{ item }">
                    <div class="d-flex align-items-center justify-content-center">
                        <CButton
                            icon="eye"
                            class="mr-2"
                            @click="
                                $router.push({
                                    name: 'PartnershipContractEdit',
                                    params: { id: item.id }
                                })
                            "
                        />
                        <CButton
                            class="mr-2"
                            icon="file-earmark-arrow-down"
                            v-if="item.statusId == 21"
                            target="_bland"
                            :href="axios.defaults.baseURL + `PrtnContract/PrintPrtnContractPdf?Id2=${item.id2}&lang=${getPdfLang()}`"
                        />
                    </div>
                </template>
            </CTable>
        </div>
    </div>
</template>

<script>
import CTable from '@/components/table/CTable.vue';
import axios from 'axios';
import PrtnContractService from '@/services/prtncontract.service';
import AppListHeaderForName from '@/components/application/AppListHeaderForName.vue';
import AppStatusBadge from '@/components/application/AppStatusBadge.vue';
import AppCard from '@/components/application/AppCard.vue';
import AppTable from '@/components/application/AppTable.vue';
import CButton from '@/components/CButton.vue';

export default {
    components: { AppStatusBadge, AppCard, AppListHeaderForName, AppTable, CTable, CButton },
    data() {
        return {
            axios,
            Loading: false,
            isBusy: false,
            Contract: [],
            filter: {
                search: null,
                sortBy: null,
                orderType: null,
                page: 1,
                pageSize: 20
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
                    label: this.$t('docNumberContract')
                },
                {
                    key: 'docOn',
                    label: this.$t('docDateContract')
                },
                {
                    key: 'contractorInn',
                    label: this.$t('contractorInn')
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
    created() {
        this.Refresh();
    },
    methods: {
        Refresh() {
            this.isBusy = true;
            this.Loading = true;
            PrtnContractService.GetList(this.filter)
                .then((res) => {
                    this.Contract = res.data.rows;
                })
                .catch((error) => {
                    this.showApiError(error);
                })
                .finally(() => {
                    this.isBusy = false;
                    this.Loading = false;
                });
        }
    }
};
</script>
