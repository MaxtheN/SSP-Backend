<template>
    <b-card no-body class="form-card">
        <!-- search -->
        <div class="p-3">
            <slot name="filter">
                <b-row>
                    <b-col></b-col>
                    <b-col v-if="searchable" cols="12" md="4">
                        <b-input-group>
                            <b-form-input v-model="filter.search" @keyup.enter="request" :placeholder="$t('search')" />
                            <b-input-group-append>
                                <b-button @click="request" variant="primary">
                                    <b-icon-search class="text-h6" />
                                </b-button>
                            </b-input-group-append>
                        </b-input-group>
                    </b-col>
                </b-row>
            </slot>
        </div>

        <!-- table -->
        <b-table
            v-bind="$attrs"
            v-on="$listeners"
            responsive
            primary-key="id"
            sticky-header="65vh"
            :no-border-collapse="noBorderCollapse"
            show-empty
            :hover="hover"
            :empty-text="$t('NotFound')"
            class="position-relative"
            @sort-changed="SortChange"
        >
            <!-- default slot -->
            <template #cell(status)="{ item }">
                <b-badge :variant="getColor(item)">{{ item.status }} </b-badge>
            </template>
            <template v-slot:table-busy>
                <div class="text-center text-primary my-2" style="vertical-align: middle">
                    <b-spinner class="align-middle mr-2"></b-spinner>
                    <strong>{{ $t('Loading') }}</strong>
                </div>
            </template>
            <!-- slots -->
            <slot v-for="(_, name) in $slots" :name="name" :slot="name"></slot>
            <template v-for="(_, name) in $scopedSlots" :slot="name" slot-scope="slotData">
                <slot :name="name" v-bind="slotData"> </slot>
            </template>
        </b-table>

        <!-- Pagination -->
        <div class="mx-1 mb-1">
            <b-row>
                <b-col cols="12" sm="6" class="d-flex align-items-center justify-content-center justify-content-sm-start">
                    <span class="text-muted"> {{ $t('Showing') }} {{ firstNumber }} {{ $t('to') }} {{ lastNumber }} {{ $t('of') }} {{ filter.total }} {{ $t('entries') }} </span>
                    <v-select v-model="filter.pageSize" :options="filter.perPageOptions" @input="request" :clearable="false" class="per-page-selector d-inline-block ml-50 mr-1" />
                </b-col>
                <b-col cols="12" sm="6" class="d-flex align-items-center justify-content-center justify-content-sm-end">
                    <b-pagination
                        v-model="filter.page"
                        :total-rows="filter.total"
                        :per-page="filter.pageSize"
                        first-number
                        last-number
                        @input="request"
                        class="mb-0 mt-1 mt-sm-0"
                        prev-class="prev-item"
                        next-class="next-item"
                    >
                        <template #prev-text>
                            <b-icon-chevron-left />
                        </template>
                        <template #next-text>
                            <b-icon-chevron-right />
                        </template>
                    </b-pagination>
                </b-col>
            </b-row>
        </div>
    </b-card>
</template>

<script>
import VSelect from 'vue-select';

export default {
    inheritAttrs: false,
    components: {
        VSelect
    },
    props: {
        searchable: {
            type: Boolean,
            default: false
        },
        filter: {
            type: Object,
            default: () => ({
                search: '',
                sortBy: '',
                orderType: 'asc',
                page: 1,
                pageSize: 20,
                perPageOptions: [10, 20, 50, 100],
                total: 0
            })
        },
        hover: {
            type: Boolean,
            default: () => true
        },
        noBorderCollapse: {
            type: Boolean,
            default: () => true
        }
    },
    computed: {
        firstNumber() {
            return (this.filter.page - 1) * this.filter.pageSize + 1;
        },
        lastNumber() {
            if (this.filter.totalRows < this.filter.pageSize) {
                return this.filter.totalRows;
            } else {
                if (this.filter.page * this.filter.pageSize > this.filter.totalRows) {
                    return this.filter.totalRows;
                } else {
                    return this.filter.page * this.filter.pageSize;
                }
            }
        }
    },
    created() {
        this.request();
    },
    methods: {
        SortChange(data) {
            this.filter.sortBy = data.sortBy;
            this.filter.orderType = this.filter.orderType == 'asc' ? 'desc' : 'asc';
            this.request();
        },
        request() {
            this.$emit('request');
        }
    }
};
</script>

<style lang="scss">
.text-muted {
    color: #b9b9c3 !important;
}
</style>
