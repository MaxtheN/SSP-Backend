<template>
    <b-overlay :show="show">
        <b-row>
            <b-col sm="12" md="12" lg="12">
                <b-card>
                    <!-- <validation-observer ref="ValidationDTO"> -->
                    <b-row>
                        <b-col sm="12" md="4" class="mb-1">
                            <form-input v-model.number="Data.docNumber" :label="$t('docnumber')" />
                        </b-col>
                        <b-col sm="12" md="4" class="mb-1">
                            <form-picker :label="$t('ondate')" v-model="Data.docOn" />
                        </b-col>
                        <b-col sm="12" md="4" class="mb-1">
                            <form-input v-model.number="Data.year" :label="$t('docyear')" />
                        </b-col>
                    </b-row>
                    <b-row>
                        <b-col sm="12" md="12" class="mb-1">
                            <b-form-textarea id="textarea" rows="2" max-rows="6" v-model="Data.details"
                                :placeholder="$t('detailinfo')"></b-form-textarea>
                        </b-col>
                    </b-row>
                    <b-row>
                        <b-col md="4" sm="4" class="mt-2">
                            <h6 class="inputTitle">{{ $t('fileupload') }}</h6>
                            <b-form-file type="file" :placeholder="$t('Faylni tanlang')" @change="UploadFile">
                            </b-form-file>
                            <div class="mt-1" v-for="item in Data.files" :key="item.id">
                                <b-link variant="primary" target="_blank" :href="FileSrc(item.id)">{{
                                    item.fileName || item.id
                                }}</b-link>
                                <b-button variant="danger" size="sm" class="ml-1" @click="DeleteFile(item.id)">
                                    <b-icon-trash scale="0.7" />
                                </b-button>
                            </div>
                        </b-col>
                    </b-row>
                    <b-row>
                        <b-col class="text-right mt-2">
                            <b-button @click="Fill" variant="primary">
                                <b-spinner v-if="isBusy" small></b-spinner>
                                <feather-icon v-else icon="AlignLeftIcon"></feather-icon>
                                {{ $t('Fill') }}
                            </b-button>
                            <b-button class="ml-2" @click="Items = []" variant="danger">
                                <feather-icon icon="XCircleIcon"></feather-icon>
                                {{ $t('clear') }}
                            </b-button>
                        </b-col>
                    </b-row>
                    <b-row class="mt-2">
                        <b-col>
                            <div>
                                <b-table-simple hover small caption-top responsive border :empty-text="$t('NotFound')">
                                    <b-thead>
                                        <b-tr>
                                            <b-th style="font-weight:900;font-size:14px;color:black">
                                                {{ $t('order')
                                                }}
                                            </b-th>
                                            <b-th v-if="Data.organizationId === 1" style="font-weight:900;font-size:14px;color:black"
                                                class="table-b-table-default b-table-sticky-column">
                                                {{ $t('region') }}
                                            </b-th>
                                            <b-th v-if="Data.organizationId !== 1" style="font-weight:900;font-size:14px;color:black"
                                                class="table-b-table-default b-table-sticky-column">
                                                {{ $t('district') }}
                                            </b-th>
                                            <b-th style="font-weight:900;font-size:14px;color:black"
                                                class="table-b-table-default b-table-sticky-column">
                                                {{ $t('month1') }}
                                            </b-th>
                                            <b-th style="font-weight:900;font-size:14px;color:black"
                                                class="table-b-table-default b-table-sticky-column">
                                                {{ $t('month2') }}
                                            </b-th>
                                            <b-th style="font-weight:900;font-size:14px;color:black"
                                                class="table-b-table-default b-table-sticky-column">
                                                {{ $t('month3') }}
                                            </b-th>
                                            <b-th style="font-weight:900;font-size:14px;color:black"
                                                v-show="$route.params.id > 0"
                                                class="table-b-table-default b-table-sticky-column">
                                                {{ $t('1-kvartal') }}
                                            </b-th>
                                            <b-th style="font-weight:900;font-size:14px;color:black"
                                                class="table-b-table-default b-table-sticky-column">
                                                {{ $t('month4') }}
                                            </b-th>
                                            <b-th style="font-weight:900;font-size:14px;color:black"
                                                class="table-b-table-default b-table-sticky-column">
                                                {{ $t('month5') }}
                                            </b-th>
                                            <b-th style="font-weight:900;font-size:14px;color:black"
                                                class="table-b-table-default b-table-sticky-column">
                                                {{ $t('month6') }}
                                            </b-th>
                                            <b-th style="font-weight:900;font-size:14px;color:black"
                                                v-show="$route.params.id > 0"
                                                class="table-b-table-default b-table-sticky-column">
                                                {{ $t('2-kvartal') }}
                                            </b-th>
                                            <b-th style="font-weight:900;font-size:14px;color:black"
                                                class="table-b-table-default b-table-sticky-column">
                                                {{ $t('month7') }}
                                            </b-th>
                                            <b-th style="font-weight:900;font-size:14px;color:black"
                                                class="table-b-table-default b-table-sticky-column">
                                                {{ $t('month8') }}
                                            </b-th>
                                            <b-th style="font-weight:900;font-size:14px;color:black"
                                                class="table-b-table-default b-table-sticky-column">
                                                {{ $t('month9') }}
                                            </b-th>
                                            <b-th style="font-weight:900;font-size:14px;color:black"
                                                v-show="$route.params.id > 0"
                                                class="table-b-table-default b-table-sticky-column">
                                                {{ $t('3-kvartal') }}
                                            </b-th>
                                            <b-th style="font-weight:900;font-size:14px;color:black"
                                                class="table-b-table-default b-table-sticky-column">
                                                {{ $t('month10') }}
                                            </b-th>
                                            <b-th style="font-weight:900;font-size:14px;color:black"
                                                class="table-b-table-default b-table-sticky-column">
                                                {{ $t('month11') }}
                                            </b-th>
                                            <b-th style="font-weight:900;font-size:14px;color:black"
                                                class="table-b-table-default b-table-sticky-column">
                                                {{ $t('month12') }}
                                            </b-th>
                                            <b-th style="font-weight:900;font-size:14px;color:black"
                                                v-show="$route.params.id > 0"
                                                class="table-b-table-default b-table-sticky-column">
                                                {{ $t('4-kvartal') }}
                                            </b-th>
                                            <b-th style="font-weight:900;font-size:14px;color:black"
                                                v-show="$route.params.id > 0"
                                                class="table-b-table-default b-table-sticky-column">
                                                {{ $t('Yillik') }}
                                            </b-th>
                                        </b-tr>
                                    </b-thead>
                                    <b-tbody :busy="isBusy" v-for="(item, index) in Items" :key="index">
                                        <template v-for="item2 in item.cellTables">
                                            <b-tr v-for="(el, eldx) in item2.monthVsValues" :key="eldx + 'a' + 1">
                                                <b-td style="font-weight:900;font-size:14px;color:black">
                                                    {{ eldx + 1 }}
                                                </b-td>
                                                <b-td v-if="Data.organizationId === 1" style="font-size:14px;color:black">
                                                    {{ el.region }}
                                                </b-td>
                                                <b-td v-if="Data.organizationId !== 1" style="font-size:14px;color:black">
                                                    {{ el.district }}
                                                </b-td>
                                                <template v-for="(i, eldx1) in el.values">
                                                    <b-td :key="eldx1 + 'v' + 1">
                                                        <form-input v-model.number="i.membersCount" />
                                                    </b-td>
                                                </template>
                                                <b-td style="width: 5%;" v-show="$route.params.id > 0">
                                                    <form-input v-model.number="el.totalForYear" />
                                                </b-td>
                                            </b-tr>
                                        </template>
                                    </b-tbody>
                                    <b-tfoot>
                                    </b-tfoot>
                                </b-table-simple>
                            </div>
                        </b-col>
                    </b-row>
                    <b-row>
                        <b-col sm="12" md="6" lg="6" class="text-left"></b-col>
                        <b-col sm="12" md="6" lg="6" class="text-right">
                            <b-button :disabled="saveLoading" @click="SaveData" size="sm" variant="outline-success">
                                <feather-icon icon="CheckIcon"></feather-icon>
                                {{ $t('Save') }}
                            </b-button>
                        </b-col>
                    </b-row>
                    <!-- </validation-observer> -->
                </b-card>
            </b-col>
        </b-row>
    </b-overlay>
</template>
<script>
// service
import MemshipYearlyPlanService from '@/services/dualedu/memshipyearlyplan.service';
import axios from 'axios';
// components
import {
    BOverlay,
    BCard,
    BRow,
    BCol,
    BSpinner,
    BFormInput,
    BTable,
    BButton,
    BButtonGroup,
    BLink,
    BFormGroup,
    BModal,
    BInputGroup,
    BInputGroupAppend,
    BFormCheckbox,
    BFormTextarea,
    BTableSimple,
    BThead,
    BTr,
    BTh,
    BTd,
    BTbody,
    BTfoot,
    BFormFile,
    BIconTrash,
} from 'bootstrap-vue';
import FormCurrencyInput from '@/components/forms/form-currency-input.vue';
const defaultTableRow = {
    id: 0,
    monthOn: null,
    regionId: null,
    region: null,
    districtId: null,
    membersCount: 0
}
export default {
    components: {
        BOverlay,
        BCard,
        BRow,
        BCol,
        BFormInput,
        BButtonGroup,
        BButton,
        BTable,
        BLink,
        BFormGroup,
        BModal,
        BSpinner,
        BInputGroup,
        BInputGroupAppend,
        BFormCheckbox,
        BFormTextarea,
        FormCurrencyInput,
        BTableSimple,
        BThead,
        BTr,
        BTh,
        BTd,
        BTbody,
        BTfoot,
        BFormFile,
        BIconTrash
    },
    name: 'Edit',
    data() {
        return {
            axios,
            show: false,
            saveLoading: false,
            Data: {
                docNumber: '',
                docOn: '',
                details: '',
                year: new Date().getFullYear(),
                files: []
            },
            Items: [],
            monthlist: [
                { monthName: "Yanvar", monthOn: 1 }
            ],
            obj: {},
            TabrowModal: false,
            tabrow: { ...defaultTableRow },
            TablesField: [
                {
                    key: 'monthOn',
                    label: this.$t('monthOn'),
                    sortable: true
                },
                {
                    key: 'regionId',
                    label: this.$t('region'),
                    sortable: true
                },
                {
                    key: 'districtId',
                    label: this.$t('district'),
                    sortable: true
                },
                {
                    key: 'membersCount',
                    label: this.$t('membersCount'),
                    sortable: true
                },
                {
                    key: 'actions',
                    label: this.$t('actions'),
                    thClass: 'text-center',
                    tdClass: 'text-center',
                    sortable: true
                }
            ],
            isBusy: false
        };
    },
    computed: {
        FileSrc() {
            return (id) => axios.defaults.baseURL + `MemshipYearlyPlanService/DownloadFile/${id}`;
        }
    },
    created() {
        this.show = true;
        MemshipYearlyPlanService.Get(this.$route.params.id)
            .then((res) => {
                this.Data = res.data;
                this.Items = [res.data]
            })
            .catch((error) => {
                this.makeToast(error.response.data.errors, 'danger');
            })
            .finally(() => {
                this.show = false;
            });
    },
    methods: {
        UploadFile(event) {
            const formData = new FormData();
            formData.append('files', event.target.files[0]);
            this.fileLoading = true;
            MemshipYearlyPlanService.UploadFile(formData).then((res) => {
                this.Data.files.push(...res.data);
                this.fileLoading = false;
            });
        },
        DeleteFile(id) {
            MemshipYearlyPlanService.DeleteFile(id).then(() => {
                this.Data.files = this.Data.files.filter((item) => item.id != id);
            });
        },
        Fill() {
            this.isBusy = true;
            MemshipYearlyPlanService.FillTable()
                .then((res) => {
                    this.Items = res.data
                    this.makeToast(this.$t('SuccessMessage'), 'success');
                })
                .catch((err) => {
                    this.showApiError(err)
                })
                .finally(() => {
                    this.isBusy = false;
                })
        },
        SaveData() {
            this.saveLoading = true;
            MemshipYearlyPlanService.Update(
                {
                    canAccept: this.Data.canAccept,
                    canCancel: this.Data.canCancel,
                    canDelete: this.Data.canDelete,
                    canModify: this.Data.canModify,
                    cellTables: this.Items[0].cellTables,
                    details: this.Data.details,
                    docNumber: this.Data.docNumber.toString(),
                    docOn: this.Data.docOn,
                    files: this.Data.files,
                    id: this.Data.id,
                    isRegion: this.Data.isRegion,
                    organization: this.Data.organization,
                    organizationId: this.Data.organizationId,
                    status: this.Data.status,
                    statusId: this.Data.statusId,
                    tableId: this.Data.tableId,
                    year: this.Data.year,
                })
                .then((res) => {
                    this.makeToast(this.$t('SaveSuccess'), 'success');
                    this.$router.push({ name: 'MemshipYearlyPlan' });
                })
                .catch((err) => {
                    this.showApiError(err);
                })
                .finally(() => {
                    this.saveLoading = false;
                });
        },
    },
};
</script>
 