<template>
    <div class="my-register-page mb-5">
        <div class="my-container">
            <b-row class="mb-2 mt-3 pt-3">
                <b-col sm="12" md="10">
                    <h1>{{ $t('Request') }}</h1>
                </b-col>
                <b-col class="mt-4" style="text-align: right">
                    <b-button style="width: 100px" @click="$router.go(-1)" variant="outline-danger">
                        {{ $t('back') }}
                    </b-button>
                </b-col>
            </b-row>
            <b-table-simple bordered>
                <b-thead class="text-center">
                    <b-tr variant="primary">
                        <b-th style="vertical-align: middle">
                            {{ $t('docNumber') }}
                        </b-th>

                        <b-th style="vertical-align: middle">
                            {{ $t('checkDate') }}
                        </b-th>
                        <b-th style="vertical-align: middle">
                            {{ $t('checkCoverageDate') }}
                        </b-th>
                        <b-th style="vertical-align: middle">
                            {{ $t('orderedOrganization') }}
                        </b-th>
                        <b-th style="vertical-align: middle">
                            {{ $t('inspectionOrganization') }}
                        </b-th>

                        <b-th style="vertical-align: middle">
                            {{ $t('actions') }}
                        </b-th>
                    </b-tr>
                </b-thead>
                <b-tbody>
                    <b-tr v-for="(item, index) in Requests" :key="index">
                        <b-td class="text-nowrap" style="vertical-align: middle">
                            {{ item.docNumber }}
                        </b-td>

                        <b-td style="white-space: nowrap"> {{ item.checkStartDate }} - {{ item.checkEndDate }} ({{ item.checkDaysNumber }} {{ $t('day') }} ) </b-td>
                        <b-td style="white-space: nowrap">
                            {{ item.checkCoverageStartDate }} -
                            {{ item.checkCoverageEndDate }}
                        </b-td>
                        <b-td>
                            {{ item.orderedOrganization }}
                        </b-td>
                        <b-td>
                            {{ item.inspectionOrganization }}
                        </b-td>

                        <b-td style="vertical-align: middle; text-align: center">
                            <b-icon
                                v-b-tooltip.hover
                                :title="$t('Complaint')"
                                style="cursor: pointer; margin-right: 10px"
                                @click="GetComplaint(item)"
                                icon="exclamation-circle-fill"
                                size="20"
                            ></b-icon>
                        </b-td>
                    </b-tr>
                </b-tbody>
            </b-table-simple>
            <div>
                <b-pagination
                    v-model="filter.page"
                    :total-rows="filter.totalRows"
                    :per-page="filter.pageSize"
                    first-number
                    last-number
                    @input="Refresh"
                    class="mb-0 mt-1 mt-sm-0"
                    prev-class="prev-item"
                    next-class="next-item"
                >
                    <template #prev-text>
                        <b-icon icon="chevron-double-left" size="18" />
                    </template>
                    <template #next-text>
                        <b-icon icon="chevron-double-right" size="18" />
                    </template>
                </b-pagination>
            </div>
        </div>
    </div>
</template>

<script>
import RequestService from '@/services/request.service';
import customSelect from '../../../components/elements/customSelect.vue';
export default {
    data() {
        return {
            Requests: [],
            filter: {
                page: 1,
                pageSize: 20,
                totalRows: 1,
                options: [
                    {
                        value: 10,
                        text: '10'
                    },
                    {
                        value: 20,
                        text: '20'
                    },
                    {
                        value: 50,
                        text: '50'
                    },
                    {
                        value: 100,
                        text: '100'
                    }
                ]
            }
        };
    },
    components: {
        customSelect
    },
    created() {
        this.Refresh();
    },
    methods: {
        Refresh() {
            RequestService.GetList(this.filter)
                .then((res) => {
                    this.Requests = res.data.rows;
                    this.filter.totalRows = res.data.total;
                })
                .catch((error) => {
                    this.Loading = false;
                    this.$message(error.response.data);
                });
        },
        GetComplaint(item) {
            // ComplaintService.GetByRequestId(item.id)
            // .then((res) => {
            //   // this.Complaint = res.data;

            //     alert('success')
            // })
            // .catch((error) => {
            //   // this.showApiError(error)
            //   alert('danger')
            // });
            this.$router.push({ name: 'EditComplaint', params: { id: item.id } });
        },
        Get(item) {
            this.$router.push({ name: 'RequestView', params: { id: item.id } });
        },
        GetIBContractor(item) {
            this.$router.push({
                name: 'InspectionBookOfContractorEdit',
                params: { id: item.id },
                query: { fromRequest: true }
            });
        },
        GetIB(item) {
            this.$router.push({
                name: 'InspectionBookEdit',
                params: { id: item.id },
                query: { fromRequest: true }
            });
        }
    }
};
</script>

<style></style>
