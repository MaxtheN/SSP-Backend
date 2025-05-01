<template>
    <div>
        <AppListHeaderForName title="SubsidyRequest" page-name="dual">
            <template #top-right>
                <b-button @click="CreateApplicationEdit" variant="info">
                    <b-icon-plus></b-icon-plus>
                    {{ $t('Add') }}
                </b-button>
            </template>
        </AppListHeaderForName>
        <div class="container-fluid">
            <CTable :items="Application" :fields="fields" bordered :busy="Loading" :filter="filter" @request="Refresh">
                <template #cell(status)="{ item }">
                    <div class="d-flex align-items-center">
                        <AppStatusBadge :item="item" />
                    </div>
                </template>

                <template #cell(actions)="{ item }">
                    <div class="d-flex align-items-center justify-content-center">
                        <CButton
                            icon="eye"
                            @click="
                                $router.push({
                                    name: 'SubsidyRequestEdit',
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
import WTextarea from '@/components/forms/WTextarea.vue';
import SubsidyRequestService from '@/services/dual/SubsidyRequest.service';
import AppStatusBadge from '@/components/application/AppStatusBadge.vue';
import AppCard from '@/components/application/AppCard.vue';
import AppListHeaderForName from '@/components/application/AppListHeaderForName.vue';
import CTable from '@/components/table/CTable.vue';
import CButton from '@/components/CButton.vue';

export default {
    components: {
        AppStatusBadge,
        AppCard,
        WTextarea,
        AppListHeaderForName,
        CTable,
        CButton
    },
    data() {
        return {
            Application: [],
            Loading: false,
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
                    key: 'contractorInnPinfl',
                    label: this.$t('inn')
                },
                {
                    key: 'docNumber',
                    label: this.$t('documentnumber')
                },
                {
                    key: 'docOn',
                    label: this.$t('docdate')
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
            this.$router.push({ name: 'SubsidyRequestEdit', params: { id: 0 } });
        },
        Refresh() {
            this.Loading = true;
            SubsidyRequestService.GetList(this.filter)
                .then((res) => {
                    this.Application = res.data.rows;
                    this.filter.total = res.data.total;
                })
                .catch(this.showApiError)
                .finally(() => {
                    this.Loading = false;
                });
        }
    }
};
</script>
