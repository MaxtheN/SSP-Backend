<template>
    <div>
        <!-- table -->
        <b-table class="bg-white" responsive show-empty v-bind="$attrs" v-on="$listeners" :hover="hover" :items="items" :fields="fields">
            <template #cell(actions)="{ item }">
                <div class="text-center">
                    <!-- edit -->
                    <template v-if="actions.edit">
                        <b-link :to="{ name: actions.edit.name, params: { id: item.id } }" style="margin-right: 5px; cursor: pointer" v-b-tooltip.hover.top="$t('edit')">
                            <b-icon-pen icon="EditIcon"></b-icon-pen>
                        </b-link>
                    </template>
                    <!-- delete -->
                    <template v-if="actions.delete">
                        <b-link class="text-danger" @click="$refs['DeleteModal' + item.id].show()" style="cursor: pointer" v-b-tooltip.hover.top="$t('delete')">
                            <b-icon-trash icon="TrashIcon"></b-icon-trash>
                        </b-link>
                        <b-modal
                            :ref="'DeleteModal' + item.id"
                            :cancel-title="$t('Cancel')"
                            :ok-title="$t('Accept')"
                            cancel-variant="danger"
                            ok-variant="success"
                            @ok="rowDelete(item)"
                        >
                            <template #modal-title>
                                {{ $t('Accept') }}
                                <b-spinner v-if="deleteLoading" small></b-spinner>
                            </template>
                            <b-card-text>
                                <h5>{{ $t('WantDeleteAdm') }}</h5>
                                <h5>ID : {{ item.id }}</h5>
                            </b-card-text>
                        </b-modal>
                    </template>
                </div>
            </template>

            <template v-slot:table-busy>
                <div class="text-center text-primary my-2" style="vertical-align: middle">
                    <b-spinner class="align-middle mr-2"></b-spinner>
                    <strong>{{ $t('Loading') }}</strong>
                </div>
            </template>

            <template #empty="scope">
                <div class="text-center d-flex justify-content-center align-items-center m-4">
                    <b-icon-folder-x scale="1.5" class="mr-3"></b-icon-folder-x>
                    <p class="m-0 text-black">{{ $t('NotFound') }}</p>
                </div>
            </template>

            <slot v-for="(_, name) in $slots" :name="name" :slot="name"></slot>
            <template v-for="(_, name) in $scopedSlots" :slot="name" slot-scope="slotData"> <slot :name="name" v-bind="slotData"> </slot> </template
        ></b-table>

        <!-- pagination -->
        <div v-if="isPagination" class="mx-1 mb-1">
            <div v-if="filter.total > filter.pageSize" class="mt-4 d-flex align-items-center justify-content-center">
                <b-pagination v-model="filter.page" :total-rows="filter.total" :per-page="filter.pageSize" pills @input="request"> </b-pagination>
            </div>
        </div>
    </div>
</template>

<script>
import VSelect from 'vue-select';
export default {
    inheritAttrs: false,
    components: {
        VSelect
    },

    props: {
        actions: {
            type: Object,
            default: () => ({}),
            required: false
        },
        items: {
            type: Array,
            default: () => []
        },
        fields: {
            type: Array,
            default: () => []
        },
        hover: {
            type: Boolean,
            default: false
        },
        isPagination: {
            type: Boolean,
            default: false
        },
        deleteLoading: {
            type: Boolean,
            default: false,
            required: false
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
        }
    },
    created() {
        this.request();
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
    methods: {
        rowDelete(item) {
            this.$emit('deleteItem', item);
        },
        request() {
            this.$emit('request');
        }
    }
};
</script>

<style lang="scss" scoped></style>
