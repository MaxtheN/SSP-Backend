<template>
    <div class="apptable">
        <!-- loader -->
        <slot v-if="busy" name="table-busy">
            <b-row>
                <b-col md="4" sm="6" v-for="i in 3" :key="i + 'loader'">
                    <b-skeleton-img class="skeleton" height="200" animation="wave"></b-skeleton-img>
                </b-col>
            </b-row>
        </slot>

        <!-- list -->
        <b-row v-else-if="items.length > 0">
            <b-col sm="6" v-bind="$attrs" v-for="(item, i) in items" :key="i + '_item'" class="my-3">
                <slot name="item" :item="item" :index="i"></slot>
            </b-col>
        </b-row>
        <b-row v-else>
            <b-col class="text-center card-text">
                <h4>{{ $t('nothinghere') }}</h4>
            </b-col>
        </b-row>

        <!-- pagination -->
        <div v-if="filter.total > filter.pageSize" class="mt-4 d-flex align-items-center justify-content-center">
            <b-pagination v-model="filter.page" :total-rows="filter.total" :per-page="filter.pageSize" pills @input="request"> </b-pagination>
        </div>
    </div>
</template>

<script>
export default {
    props: {
        busy: {
            type: Boolean,
            default: false
        },
        items: {
            type: Array,
            default: () => []
        },
        filter: {
            type: Object,
            default: () => ({
                search: '',
                sortBy: '',
                orderType: 'asc',
                page: 1,
                pageSize: 10,
                total: 0
            })
        }
    },
    emits: ['request'],
    created() {
        this.request();
    },
    methods: {
        request() {
            this.$emit('request');
        }
    }
};
</script>

<style lang="scss">
.apptable {
    .skeleton {
        border-radius: 16px;
        overflow: hidden;
        background-color: rgba(0, 0, 0, 0.08);
    }
}
</style>
