<template>
    <b-overlay :show="show">
        <div class="my-register-page" style="font-size: 13px">
            <div class="my-container">
                <b-row class="mb-2">
                    <b-col>
                        <h1>{{ $t('Request') }}</h1>
                    </b-col>
                </b-row>

                <b-table-simple bordered responsive="sm">
                    <b-tbody>
                        <b-tr variant="primary">
                            <b-td colspan="4" class="text-center">
                                <span style="font-size: 18px; font-weight: bold">{{ $t('DocumentInfo') }}</span>
                            </b-td>
                        </b-tr>
                        <b-tr>
                            <b-td class="font-weight-bold">
                                {{ $t('docNumber') }}
                            </b-td>
                            <b-td>
                                {{ Request.docNumber }}
                            </b-td>
                            <b-td class="font-weight-bold">
                                {{ $t('docDate') }}
                            </b-td>
                            <b-td>
                                {{ Request.docDate }}
                            </b-td>
                        </b-tr>
                        <b-tr>
                            <b-td class="font-weight-bold">
                                {{ $t('checkCoverageStartDate') }}
                            </b-td>
                            <b-td>
                                {{ Request.checkCoverageStartDate }}
                            </b-td>
                            <b-td class="font-weight-bold">
                                {{ $t('checkCoverageEndDate') }}
                            </b-td>
                            <b-td>
                                {{ Request.checkCoverageEndDate }}
                            </b-td>
                        </b-tr>
                        <b-tr variant="primary">
                            <b-td colspan="4" class="text-center">
                                <span style="font-size: 18px; font-weight: bold">{{ $t('PersonalInfo') }}</span>
                            </b-td>
                        </b-tr>
                        <b-tr>
                            <b-td class="font-weight-bold">
                                {{ $t('inn') }}
                            </b-td>
                            <b-td>
                                {{ Request.contractor.inn }}
                            </b-td>
                            <b-td class="font-weight-bold">
                                {{ $t('name') }}
                            </b-td>
                            <b-td>
                                {{ Request.contractor.fullName }}
                            </b-td>
                        </b-tr>
                        <b-tr>
                            <b-td class="font-weight-bold">
                                {{ $t('oblast') }}
                            </b-td>
                            <b-td>
                                {{ Request.contractor.region }}
                            </b-td>
                            <b-td class="font-weight-bold">
                                {{ $t('region') }}
                            </b-td>
                            <b-td>
                                {{ Request.contractor.district }}
                            </b-td>
                        </b-tr>
                        <b-tr>
                            <b-td class="font-weight-bold">
                                {{ $t('oked') }}
                            </b-td>
                            <b-td>
                                {{ Request.contractor.oked }}
                            </b-td>
                            <b-td class="font-weight-bold">
                                {{ $t('checkType') }}
                            </b-td>
                            <b-td>
                                {{ Request.checkType }}
                            </b-td>
                        </b-tr>
                        <b-tr>
                            <b-td class="font-weight-bold">
                                {{ $t('checkBasis') }}
                            </b-td>
                            <b-td>
                                {{ Request.checkBasis }}
                            </b-td>
                            <b-td class="font-weight-bold">
                                {{ $t('basicFiles') }}
                            </b-td>
                            <b-td>
                                <span
                                    v-for="(item, index) in Request.basicFiles"
                                    :key="index"
                                    class="pb-1 px-2 rounded text-white"
                                    @click="downloadBasicFile(item)"
                                    style="background: #2a3b5d; cursor: pointer"
                                >
                                    {{ item.fileName }}
                                </span>
                            </b-td>
                        </b-tr>
                        <b-tr>
                            <b-td class="font-weight-bold">
                                {{ $t('checkStartDate') }}
                            </b-td>
                            <b-td>
                                {{ Request.checkStartDate }}
                            </b-td>
                            <b-td class="font-weight-bold">
                                {{ $t('checkEndDate') }}
                            </b-td>
                            <b-td>
                                {{ Request.checkEndDate }}
                            </b-td>
                        </b-tr>
                        <b-tr>
                            <b-td class="font-weight-bold">
                                {{ $t('checkDaysNumber') }}
                            </b-td>
                            <b-td>
                                {{ Request.checkDaysNumber }}
                            </b-td>
                            <b-td> </b-td>
                            <b-td> </b-td>
                        </b-tr>
                        <b-tr>
                            <b-td class="font-weight-bold">
                                {{ $t('orderedOrganization') }}
                            </b-td>
                            <b-td>
                                {{ Request.orderedOrganization }}
                            </b-td>
                            <b-td class="font-weight-bold">
                                {{ $t('inspectionOrganization') }}
                            </b-td>
                            <b-td>
                                {{ Request.inspectionOrganization }}
                            </b-td>
                        </b-tr>
                        <b-tr>
                            <b-td class="font-weight-bold">
                                {{ $t('authorizedOrganization') }}
                            </b-td>
                            <b-td>
                                {{ Request.authorizedOrganization }}
                            </b-td>
                            <b-td class="font-weight-bold">
                                {{ $t('checkSubjects') }} <br />
                                {{ $t('canViolatedLegalDocuments') }}
                            </b-td>
                            <b-td>
                                <div v-for="(item, index) in Request.checkSubjects" :key="index" class="mb-2">
                                    <span class="pb-1 px-2 rounded text-white text-nowrap" style="background: green; cursor: pointer"> {{ item }} </span> <br />
                                </div>
                                <div v-for="(item, index) in Request.canViolatedLegalDocuments" :key="index">
                                    <span class="pb-1 px-2 rounded text-white text-nowrap" style="background: #2a3b5d; cursor: pointer"> {{ item }} </span> <br />
                                </div>
                            </b-td>
                        </b-tr>
                        <b-tr>
                            <b-td class="font-weight-bold">
                                {{ $t('orderNumber') }}
                            </b-td>
                            <b-td>
                                {{ Request.orderNumber }}
                            </b-td>
                            <b-td class="font-weight-bold">
                                {{ $t('orderFiles') }}
                            </b-td>
                            <b-td>
                                <div v-for="(item, index) in Request.orderFiles" :key="index" class="mb-1">
                                    <span class="pb-1 px-2 rounded text-white mt-1" @click="downloadOrderFile(item)" style="background: #2a3b5d; cursor: pointer">
                                        {{ item.fileName }}
                                    </span>
                                </div>
                            </b-td>
                        </b-tr>
                        <b-tr>
                            <b-td class="font-weight-bold"> {{ $t('inspectors') }} <br /> </b-td>
                            <b-td colspan="3">
                                <div v-for="(item, index) in Request.inspectorNames" :key="index" class="mb-2 d-inline-block" style="margin-right: 10px">
                                    <span class="pb-1 px-2 rounded text-white text-nowrap" style="background: #2a3b5d; cursor: pointer"> {{ item }} </span>
                                </div>
                            </b-td>
                        </b-tr>
                    </b-tbody>
                </b-table-simple>

                <b-table-simple v-if="Request.inspectionBookId != null" bordered responsive="sm">
                    <b-tbody>
                        <b-tr variant="primary">
                            <b-td colspan="4" class="text-center">
                                <span style="font-size: 18px; font-weight: bold">{{ $t('InspectionBook') }}</span>
                            </b-td>
                        </b-tr>
                        <b-tr>
                            <b-td class="font-weight-bold">
                                {{ $t('docDate') }}
                            </b-td>
                            <b-td>
                                {{ InspectionBook.docDate }}
                            </b-td>
                            <b-td class="font-weight-bold">
                                {{ $t('orderNumber') }}
                            </b-td>
                            <b-td>
                                {{ InspectionBook.orderNumber }}
                            </b-td>
                        </b-tr>
                        <b-tr>
                            <b-td class="font-weight-bold">
                                {{ $t('startDate') }}
                            </b-td>
                            <b-td>
                                {{ InspectionBook.startDate }}
                            </b-td>
                            <b-td class="font-weight-bold">
                                {{ $t('endDate') }}
                            </b-td>
                            <b-td>
                                {{ InspectionBook.endDate }}
                            </b-td>
                        </b-tr>
                        <b-tr>
                            <b-td class="font-weight-bold">
                                {{ $t('checkDaysNumber') }}
                            </b-td>
                            <b-td>
                                {{ InspectionBook.checkDaysNumber }}
                            </b-td>
                            <b-td class="font-weight-bold">
                                {{ $t('inspectorNames') }}
                            </b-td>
                            <b-td>
                                <div v-for="(item, index) in InspectionBook.inspectorNames" :key="index">
                                    <span class="pb-1 px-2 rounded text-white text-nowrap" style="background: #2a3b5d; cursor: pointer"> {{ item }} </span> <br />
                                </div>
                            </b-td>
                        </b-tr>
                        <b-tr>
                            <b-td class="font-weight-bold">
                                {{ $t('inspectorNames') }}
                            </b-td>
                            <b-td colspan="3">
                                <div v-for="(item, index) in InspectionBook.controlFunctionNames" :key="index">
                                    <span class="pb-1 px-2 rounded text-white text-nowrap" style="background: #2a3b5d; cursor: pointer"> {{ item }} </span> <br />
                                </div>
                            </b-td>
                        </b-tr>
                        <b-tr>
                            <b-td colspan="4" class="text-center">
                                <span style="font-size: 18px; font-weight: bold">{{ $t('specialists') }}</span>
                            </b-td>
                        </b-tr>
                        <b-tr>
                            <b-th class="font-weight-bold">
                                {{ $t('fullName') }}
                            </b-th>
                            <b-th class="font-weight-bold">
                                {{ $t('workplace') }}
                            </b-th>

                            <b-th class="font-weight-bold">
                                {{ $t('directionOfActivity') }}
                            </b-th>
                            <b-th class="font-weight-bold">
                                {{ $t('contractNumber') }}
                            </b-th>
                        </b-tr>
                        <b-tr v-for="(item, index) in InspectionBook.specialists" :key="index">
                            <b-td>
                                {{ item.fullName }}
                            </b-td>
                            <b-td>
                                {{ item.workplace }}
                            </b-td>
                            <b-td>
                                {{ item.directionOfActivity }}
                            </b-td>
                            <b-td>
                                {{ item.contractNumber }}
                            </b-td>
                        </b-tr>
                    </b-tbody>
                </b-table-simple>

                <b-table-simple v-if="Request.inspectionResultId != null" bordered responsive="sm">
                    <b-tbody>
                        <b-tr variant="primary">
                            <b-td colspan="4" class="text-center">
                                <span style="font-size: 18px; font-weight: bold">{{ $t('InspectionResult') }}</span>
                            </b-td>
                        </b-tr>
                        <b-tr>
                            <b-td class="font-weight-bold">
                                {{ $t('docDate') }}
                            </b-td>
                            <b-td>
                                {{ InspectionResult.docDate }}
                            </b-td>
                            <b-td> </b-td>
                            <b-td> </b-td>
                        </b-tr>
                        <b-tr>
                            <b-td class="font-weight-bold">
                                {{ $t('startDate') }}
                            </b-td>
                            <b-td>
                                {{ InspectionResult.startDate }}
                            </b-td>
                            <b-td class="font-weight-bold">
                                {{ $t('endDate') }}
                            </b-td>
                            <b-td>
                                {{ InspectionBook.endDate }}
                            </b-td>
                        </b-tr>
                        <b-tr>
                            <b-td class="font-weight-bold">
                                {{ $t('inspectorNames') }}
                            </b-td>
                            <b-td>
                                <div v-for="(item, index) in InspectionResult.inspectorNames" :key="index">
                                    <span class="pb-1 px-2 rounded text-white text-nowrap" style="background: #2a3b5d; cursor: pointer"> {{ item }} </span> <br />
                                </div>
                            </b-td>
                            <b-td class="font-weight-bold">
                                {{ $t('canViolatedLegalDocuments') }}
                            </b-td>
                            <b-td>
                                <div v-for="(item, index) in InspectionResult.canViolatedLegalDocuments" :key="index">
                                    <span class="pb-1 px-2 rounded text-white text-nowrap" style="background: #2a3b5d; cursor: pointer"> {{ item }} </span> <br />
                                </div>
                            </b-td>
                        </b-tr>
                        <b-tr>
                            <b-td class="font-weight-bold">
                                {{ $t('comment') }}
                            </b-td>
                            <b-td>
                                {{ InspectionResult.comment }}
                            </b-td>
                            <b-td class="font-weight-bold">
                                {{ $t('actFiles') }}
                            </b-td>
                            <b-td>
                                <div v-for="(item, index) in InspectionResult.actFiles" :key="index" class="mb-1">
                                    <span class="pb-1 px-2 rounded text-white mt-1" @click="downloadActFile(item)" style="background: #2a3b5d; cursor: pointer">
                                        {{ item.fileName }}
                                    </span>
                                </div>
                            </b-td>
                        </b-tr>

                        <b-tr>
                            <b-td class="font-weight-bold">
                                {{ $t('measuresOfInfluence') }}
                            </b-td>
                            <b-td>
                                {{ InspectionResult.measuresOfInfluence }}
                            </b-td>
                            <b-td class="font-weight-bold">
                                {{ $t('measuresOfInfluenceFiles') }}
                            </b-td>
                            <b-td>
                                <div v-for="(item, index) in InspectionResult.measuresOfInfluenceFiles" :key="index" class="mb-1">
                                    <span class="pb-1 px-2 rounded text-white mt-1" @click="downloadMeasuresOfInfuenceFile(item)" style="background: #2a3b5d; cursor: pointer">
                                        {{ item.fileName }}
                                    </span>
                                </div>
                            </b-td>
                        </b-tr>

                        <b-tr>
                            <b-td class="font-weight-bold">
                                {{ $t('measuresResult') }}
                            </b-td>
                            <b-td>
                                {{ InspectionResult.measuresResult }}
                            </b-td>
                            <b-td class="font-weight-bold">
                                {{ $t('measuresResultFiles') }}
                            </b-td>
                            <b-td>
                                <div v-for="(item, index) in InspectionResult.measuresResultFiles" :key="index" class="mb-1">
                                    <span class="pb-1 px-2 rounded text-white mt-1" @click="downloadMeasuresResultFile(item)" style="background: #2a3b5d; cursor: pointer">
                                        {{ item.fileName }}
                                    </span>
                                </div>
                            </b-td>
                        </b-tr>

                        <b-tr>
                            <b-td class="font-weight-bold">
                                {{ $t('cancelledMeasures') }}
                            </b-td>
                            <b-td>
                                {{ InspectionResult.cancelledMeasures }}
                            </b-td>
                            <b-td class="font-weight-bold">
                                {{ $t('cancelledMeasuresFiles') }}
                            </b-td>
                            <b-td>
                                <div v-for="(item, index) in InspectionResult.cancelledMeasuresFiles" :key="index" class="mb-1">
                                    <span class="pb-1 px-2 rounded text-white mt-1" @click="downloadCancelledMeasuresFile(item)" style="background: #2a3b5d; cursor: pointer">
                                        {{ item.fileName }}
                                    </span>
                                </div>
                            </b-td>
                        </b-tr>
                    </b-tbody>
                </b-table-simple>
                <!-- <b-card> -->
                <b-row class="mb-3">
                    <b-button @click="back" style="width: 150px; margin-left: 12px" variant="outline-danger"> {{ $t('back') }} </b-button>
                </b-row>
                <!-- </b-card> -->
            </div>
        </div>
    </b-overlay>
</template>

<script>
import RequestService from '@/services/request.service';
import InspectionBookService from '@/services/inspectionbook.service';
import InspectionResultService from '@/services/inspectionresult.service';
export default {
    data() {
        return {
            Request: {},
            InspectionBook: {},
            InspectionResult: {},
            filter: {},
            show: false
        };
    },
    created() {
        this.Refresh();
    },
    methods: {
        Refresh() {
            this.show = true;

            RequestService.Get(this.$route.params.id)
                .then((res) => {
                    this.Request = res.data;
                    if (!!res.data.inspectionBookId) {
                        InspectionBookService.Get(res.data.inspectionBookId)
                            .then((resp) => {
                                this.InspectionBook = resp.data;
                            })
                            .catch((error) => {
                                this.makeToast(error.response.data, 'error');
                            });
                    }
                    if (!!res.data.inspectionResultId) {
                        InspectionResultService.Get(res.data.inspectionResultId)
                            .then((response) => {
                                this.InspectionResult = response.data;
                            })
                            .catch((error) => {
                                this.makeToast(error.response.data, 'error');
                            });
                    }

                    this.show = false;
                })
                .catch((error) => {
                    this.makeToast(error.response.data, 'error');
                    this.show = false;
                });
        },
        back() {
            // this.$router.push({ name : 'Request'})
            this.$router.go(-1);
        },
        forceFileDownload(response, name) {
            var headers = response.headers;
            var blob = new Blob([response.data]);
            const url = window.URL.createObjectURL(blob);
            const link = document.createElement('a');
            link.href = url;
            link.setAttribute('download', name); //or any other extension
            document.body.appendChild(link);
            link.click();
        },
        downloadBasicFile(item) {
            RequestService.DownloadBasicFile(item.id)
                .then((res) => {
                    this.forceFileDownload(res, item.fileName);
                })
                .catch((error) => {
                    this.makeToast(error.response.data, 'error');
                });
        },
        downloadOrderFile(item) {
            RequestService.DownloadOrderFile(item.id)
                .then((res) => {
                    this.forceFileDownload(res, item.fileName);
                })
                .catch((error) => {
                    console.log(error.response);
                    this.makeToast(error.response.data.errors, 'error');
                });
        },
        downloadActFile(item) {
            InspectionResultService.DownloadActFile(item.id)
                .then((res) => {
                    this.forceFileDownload(res, item.fileName);
                })
                .catch((error) => {
                    console.log(error.response);
                    this.makeToast(error.response.data.errors, 'error');
                });
        },
        downloadMeasuresOfInfuenceFile(item) {
            InspectionResultService.DownloadMeasuresOfInfluenceFile(item.id)
                .then((res) => {
                    this.forceFileDownload(res, item.fileName);
                })
                .catch((error) => {
                    console.log(error.response);
                    this.makeToast(error.response.data.errors, 'error');
                });
        },
        downloadMeasuresResultFile(item) {
            InspectionResultService.DownloadMeasuresResultFile(item.id)
                .then((res) => {
                    this.forceFileDownload(res, item.fileName);
                })
                .catch((error) => {
                    console.log(error.response);
                    this.makeToast(error.response.data.errors, 'error');
                });
        },
        downloadCancelledMeasuresFile(item) {
            InspectionResultService.DownloadCancelledMeasuresFile(item.id)
                .then((res) => {
                    this.forceFileDownload(res, item.fileName);
                })
                .catch((error) => {
                    console.log(error.response);
                    this.makeToast(error.response.data.errors, 'error');
                });
        },
        makeToast(message, type) {
            var a = '';
            if (message.status == 500) {
                a = message.title;
            }
            if (message.status == 400) {
                var errors = Object.values(message.errors);
                var a = errors.map((el, item) => item + 1 + '.' + el).join('\n');
            } else {
                a = message;
            }
            this.$toast.open({
                message: a,
                type: type,
                duration: 5000,
                dismissible: true
            });
        }
    }
};
</script>

<style></style>
