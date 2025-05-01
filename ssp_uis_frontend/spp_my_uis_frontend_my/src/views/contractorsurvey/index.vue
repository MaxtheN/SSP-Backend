<template>
    <div class="container">
        <AppListHeaderForName title="Questionnarie" page-name="claim_application" />

        <WTable :items="list" :fields="fields" searchable :busy="isBusy" @request="Refresh">
            <template #cell(actions)="{ item }">
                <div class="text-center" style="text-wrap: nowrap">
                    <b-link :to="{ name: 'QuestionnarieView', params: { id: item.id } }" v-b-tooltip.hover.top="$t('View')" style="margin-right: 5px">
                        <b-icon-eye />
                    </b-link>
                </div>
            </template>
        </WTable>
    </div>
</template>

<script>
import axios from 'axios';
import QuestionnarieService from '@/services/questionnarie.service';
import AppStatusBadge from '@/components/application/AppStatusBadge.vue';
import AppCard from '@/components/application/AppCard.vue';
import AppListHeaderForName from '@/components/application/AppListHeaderForName.vue';
import WTable from '@/components/table/WTable.vue';

export default {
    components: { AppStatusBadge, AppCard, AppListHeaderForName, WTable },
    data() {
        return {
            axios,
            downloadloading: false,
            list: [],
            fields: [
                {
                    key: 'id',
                    label: this.$t('id'),
                    thClass: 'text-center',
                    tdClass: 'text-center',
                    sortable: true
                },
                {
                    key: 'orderNumber',
                    label: this.$t('orderCode'),
                    thClass: 'text-center',
                    tdClass: 'text-center',
                    sortable: true
                },
                {
                    key: 'title',
                    label: this.$t('orderCode'),
                    thClass: 'text-center',
                    tdClass: 'text-center',
                    sortable: true
                },
                {
                    key: 'questionnaireType',
                    label: this.$t('questionnaireType')
                },
                {
                    key: 'details',
                    label: this.$t('details'),
                    thClass: 'text-center',
                    tdClass: 'text-center',
                    sortable: true
                },
                {
                    key: 'stateId',
                    label: this.$t('status'),
                    thClass: 'text-center',
                    tdClass: 'text-center',
                    sortable: true
                },
                {
                    key: 'actions',
                    label: this.$t('actions'),
                    thClass: 'text-center',
                    tdClass: 'text-center'
                }
            ],
            filter: {
                search: '',
                sortBy: '',
                orderType: 'asc',
                page: 1,
                pageSize: 20,
                perPageOptions: [10, 20, 50, 100],
                total: 0
            },
            isBusy: false
        };
    },
    methods: {
        Refresh() {
            this.isBusy = true;
            QuestionnarieService.GetList(this.filter)
                .then((res) => {
                    this.list = res.data.rows;
                })
                .catch((error) => {
                    this.showApiError(error);
                })
                .finally(() => {
                    this.isBusy = false;
                });
        }
    }
};
</script>
