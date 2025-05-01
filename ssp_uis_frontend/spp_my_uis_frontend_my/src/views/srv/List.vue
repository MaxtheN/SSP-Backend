<template>
    <div>
        <AppListHeaderForName title="Srv" page-name="Srv" :query="{ type: $route.query.type }"> </AppListHeaderForName>
        <div class="container">
            <AppTable :busy="Loading" :items="NeedChamberServiceGroupList" :filter="filter" @request="Refresh">
                <template #item="{ item }">
                    <AppCard
                        class="pt-2 cursor-pointer"
                        @click.native="
                            $router.push({
                                name: 'ServiceApplicationEdit',
                                params: {
                                    id: 0
                                },
                                query: {
                                    group: item.id,
                                    type: $route.query.type
                                }
                            })
                        "
                    >
                        <template #body>
                            <h3 class="mb-0">
                                {{ item.fullName }}
                            </h3>
                        </template>
                    </AppCard>
                </template>
            </AppTable>
        </div>
    </div>
</template>

<script>
import WTextarea from '@/components/forms/WTextarea.vue';
import AppStatusBadge from '@/components/application/AppStatusBadge.vue';
import AppCard from '@/components/application/AppCard.vue';
import AppListHeaderForName from '@/components/application/AppListHeaderForName.vue';
import AppTable from '@/components/application/AppTable.vue';
import NeedChamberServiceGroupService from '@/services/needchamberservicegroup.service';
import NeedChamberServiceService from '@/services/needchamberservice.service';

export default {
    components: {
        AppStatusBadge,
        AppCard,
        WTextarea,
        AppListHeaderForName,
        AppTable
    },
    data() {
        return {
            NeedChamberServiceGroupList: [],
            Loading: false,
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
        Refresh() {
            this.Loading = true;
            if (this.$route.query.type == '3') {
                NeedChamberServiceService.GroupingByFreeServices({})
                    .then((res) => {
                        this.NeedChamberServiceGroupList = res.data.map((e) => ({ ...e, id: e.groupId, fullName: e.group }));
                    })
                    .catch(this.showApiError)
                    .finally(() => {
                        this.Loading = false;
                    });
            } else {
                NeedChamberServiceGroupService.GetList(this.filter)
                    .then((res) => {
                        this.NeedChamberServiceGroupList = res.data.rows;
                        this.filter.total = res.data.total;
                    })
                    .catch(this.showApiError)
                    .finally(() => {
                        this.Loading = false;
                    });
            }
        }
    }
};
</script>
