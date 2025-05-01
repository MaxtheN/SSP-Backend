<template>
    <div>
        <AppListHeaderForName title="PrtnCreditDemand">
            <template #top-right>
                <b-button
                    @click="
                        $router.push({
                            name: 'PrtnCreditDemandEdit',
                            params: { id: 0 }
                        })
                    "
                    variant="info"
                >
                    <b-icon-plus></b-icon-plus>
                    {{ $t('Add') }}
                </b-button>
            </template>
        </AppListHeaderForName>
        <div class="container-fluid">
            <div class="text-right">
                <a href="https://bank-kredit.uz/public/forms/ht_credit_application" target="_blank">
                    <!-- {{ $t("goUrl") }} -->
                    https://bank-kredit.uz
                </a>
            </div>
            <CTable :items="Application" :fields="fields" bordered :busy="Loading">
                <template #cell(status)="{ item }">
                    <div class="d-flex align-items-center">
                        <AppStatusBadge :item="item" />
                    </div>
                </template>
                <template #cell(prtnContractStatus)="{ item }">
                    <div class="d-flex align-items-center" v-if="item.prtnContractStatusId">
                        <AppStatusBadge @click="OpenHistory(item)" :item="{ ...item, statusId: item.prtnContractStatusId, status: item.prtnContractStatus }" />
                        <router-link :to="{ name: 'Contract' }" style="font-size: 25px; margin-left: 15px; cursor: pointer !important">
                            <b-icon style="cursor: pointer !important" icon="file-earmark-arrow-down" variant="primary" scale="1"></b-icon>
                        </router-link>
                    </div>
                </template>

                <template #cell(actions)="{ item }">
                    <div class="d-flex align-items-center justify-content-center">
                        <CButton
                            icon="pencil"
                            @click="
                                $router.push({
                                    name: 'PrtnCreditDemandEdit',
                                    params: { id: item.id }
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
import axios from 'axios';
import PrtnCreditDemandService from '@/services/prtncreditdemand.service';
import CustomButton from '@/components/elements/customButton.vue';
import AppStartModal from '@/components/application/AppStartModal.vue';
import AppListHeaderForName from '@/components/application/AppListHeaderForName.vue';
import AppStatusBadge from '@/components/application/AppStatusBadge.vue';
import AppCard from '@/components/application/AppCard.vue';
import AppTable from '@/components/application/AppTable.vue';
import CurrencyMixin from '@/mixins/currency';
import CTable from '@/components/table/CTable.vue';
import CButton from '@/components/CButton.vue';

export default {
    mixins: [CurrencyMixin],
    components: {
        CustomButton,
        AppStartModal,
        AppStatusBadge,
        AppCard,
        AppListHeaderForName,
        AppTable,
        CTable,
        CButton
    },
    data() {
        return {
            axios,
            Application: [],
            DeleteLoading: false,
            filter: {
                id: 0,
                message: ''
            },
            canCreate: false,
            Loading: false,
            downloadloading: false,

            dataFilter: {
                search: null,
                sortBy: null,
                orderType: null,
                page: 1,
                pageSize: 20
            },
            fields: [
                {
                    key: 'docNumber',
                    label: this.$t('number')
                },
                {
                    key: 'docOn',
                    label: this.$t('docOn')
                },
                {
                    key: 'prtnContractType',
                    label: this.$t('type')
                },
                {
                    key: 'implementedProjectName',
                    label: this.$t('implementedProjectName')
                },

                {
                    key: 'bank',
                    label: this.$t('bank')
                },
                {
                    key: 'projectCost',
                    label: this.$t('projectCost')
                },
                {
                    key: 'privillageBankCredit',
                    label: this.$t('privillageBankCredit')
                },
                {
                    key: 'prtnContractStatus',
                    label: this.$t('prtnContractStatus')
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
            PrtnCreditDemandService.GetList(this.dataFilter)
                .then((res) => {
                    this.Application = res.data.rows;
                    this.Loading = false;
                })
                .catch((error) => {
                    this.showApiError(error);
                    this.Loading = false;
                });
        },

        Delete(item) {
            this.DeleteLoading = true;
            PrtnCreditDemandService.Delete(item.id)
                .then((res) => {
                    this.DeleteLoading = false;
                    this.makeToast(this.$t('DeleteSuccess'), 'success');
                    this.$bvModal.hide('DeleteModal' + item.id);
                    this.Refresh();
                })
                .catch((error) => {
                    this.showApiError(error);
                    this.DeleteLoading = false;
                });
        }
    }
};
</script>

<style lang="scss">
.modal-text-style {
    padding: 10px;
    margin: 10px;
}
</style>
