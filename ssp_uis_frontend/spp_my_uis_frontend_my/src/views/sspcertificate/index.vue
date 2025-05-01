<template>
    <div>
        <!-- <Navbar /> -->
        <b-overlay :show="Loading" style="min-height: calc(100vh - 460px)">
            <div class="container" style="margin-top: 100px">
                <b-row>
                    <b-col sm="12" lg="6">
                        <h2 @click="$router.go(-1)" style="cursor: pointer" class="textbackviewstyle">
                            <b-icon-chevron-left></b-icon-chevron-left>
                            {{ $t('certificate_second') }}
                        </h2>
                    </b-col>
                </b-row>

                <b-row>
                    <b-col class="mt-3" sm="12" md="6" v-for="(item, index) in Certificate" :key="index" v-show="Certificate.length > 0">
                        <div class="pricing-box px-4 pt-4 pb-4">
                            <b-row>
                                <b-col sm="12">
                                    <table>
                                        <tr>
                                            <td style="width: 40%">{{ $t('contractor') }} :</td>
                                            <th style="width: 60%">{{ item.contractor }}</th>
                                        </tr>
                                        <tr>
                                            <td style="width: 40%">{{ $t('inn') }} :</td>
                                            <th style="width: 60%">{{ item.contractorInn }}</th>
                                        </tr>
                                        <tr class="py-3">
                                            <td style="width: 40%">{{ $t('docNumberCertificate') }} :</td>
                                            <th style="width: 60%">{{ item.docNumber }}</th>
                                        </tr>
                                        <tr class="py-3">
                                            <td style="width: 40%">{{ $t('docOnCertificate') }} :</td>
                                            <th style="width: 60%">{{ item.docOn }}</th>
                                        </tr>
                                        <tr class="py-3">
                                            <td style="width: 40%">{{ $t('expireOnCertificate') }} :</td>
                                            <th style="width: 60%">{{ item.expireOn }}</th>
                                        </tr>
                                        <tr class="my-1">
                                            <td style="width: 40%">{{ $t('type') }} :</td>
                                            <th style="width: 60%">
                                                {{ item.prtnContractType }}
                                            </th>
                                        </tr>
                                        <tr>
                                            <td class="pr-btn" style="width: 40%">{{ $t('status') }} :</td>
                                            <th style="width: 60%">
                                                <b-badge
                                                    style="cursor: pointer"
                                                    @click="OpenHistory(item)"
                                                    :variant="getColor(item)"
                                                    :style="`background-color:${getColor(item)}`"
                                                    size="sm"
                                                >
                                                    {{ item.status }}
                                                </b-badge>
                                            </th>
                                        </tr>
                                    </table>
                                </b-col>
                            </b-row>
                            <b-row class="mt-2">
                                <b-col sm="12">
                                    <a
                                        @click="
                                            $router.push({
                                                name: 'PartnershipCertificateEdit',
                                                params: { id: item.id }
                                            })
                                        "
                                        style="margin-right: 5px"
                                        class="btn btn-sm btn-soft-primary mr-2 mt-2 pr-btn myButton"
                                    >
                                        <b-icon-eye scale="0.8"></b-icon-eye> {{ $t('View') }}
                                    </a>

                                    <a
                                        :href="axios.defaults.baseURL + `PrtnCertificate/PrintCertificatePdf?Id2=${item.id2}`"
                                        target="_bland"
                                        @click="changeDate()"
                                        style="margin-right: 5px; white-space: nowrap; width: 165px"
                                        class="btn btn-sm btn-soft-primary mr-2 mt-2 pr-btn myButton"
                                    >
                                        <b-icon-download scale="0.8"></b-icon-download>
                                        {{ $t('downloadfile') }}
                                    </a>
                                </b-col>
                            </b-row>
                        </div>
                    </b-col>
                </b-row>
                <b-sidebar no-header width="400px" shadow right v-model="historySidebar" bg-variant="white">
                    <div style="width: 100%; height: 100%">
                        <div class="container-fluid w-100" style="width: 100% !important; position: relative; overflow-y: auto">
                            <b-row class="w-100">
                                <b-col class="text-right close-icon">
                                    <b-icon-x scale="2.5" style="cursor: pointer; z-index: 9" @click="historySidebar = false"></b-icon-x>
                                </b-col>
                            </b-row>
                            <b-row class="p-0">
                                <b-col>
                                    <ul class="timeline">
                                        <li v-for="(el, i) in History" :key="i">
                                            <b-row class="p-0">
                                                <b-col class="float-left">
                                                    <b-row class="p-0">
                                                        <b-col class="float-left">
                                                            <div class="d-flex justify-content-between flex-sm-row flex-column mb-sm-0 mb-1">
                                                                <h6>{{ el.userInfo }}</h6>
                                                            </div>
                                                            <p>
                                                                <b-badge :variant="getColor(el)">{{ el.statusChangedDate }}</b-badge>
                                                                -
                                                                <b-badge :variant="getColor(el)">
                                                                    {{ el.status }}
                                                                </b-badge>
                                                                {{ el.message == null ? '' : '-' }}
                                                                {{ el.message }}
                                                            </p>
                                                        </b-col>
                                                    </b-row>
                                                </b-col>
                                            </b-row>
                                        </li>
                                    </ul>
                                </b-col>
                            </b-row>
                        </div>
                    </div>
                </b-sidebar>
            </div>
            <b-modal v-model="InfoModal" size="lg" :title="$t('Info')" no-close-on-backdrop hide-footer>
                <p>
                    {{
                        $t('Senttoorganization', {
                            OrganizationFilial: OrganizationFilial,
                            ApplicationNumber: ApplicationNumber
                        })
                    }}
                </p>
            </b-modal>
        </b-overlay>
        <!-- <Footer /> -->
    </div>
</template>

<script>
import axios from 'axios';
import PrtnCertificateService from '@/services/prtncertificate.service';

export default {
    data() {
        return {
            axios,
            lang: localStorage.getItem('locale') || 'uz_latn',
            Application: [],
            DeleteLoading: false,
            filter: {
                id: 0,
                message: ''
            },
            buttonfilter: {
                admissiontypeid: 0
            },
            Loading: false,
            historySidebar: false,
            InfoModal: false,
            modalindex: 0,
            OrganizationFilial: '',
            ApplicationNumber: '',
            History: [],
            histories: [],
            PersentTime: '',
            downloadloading: false,
            AdoptPermisConclusion: {
                docnumber: '',
                docdate: '',
                commenttext: '',
                projectfiletext: ''
            },
            Certificate: {},
            dataFilter: {
                search: null,
                sortBy: null,
                orderType: null,
                page: 1,
                pageSize: 20
            }
        };
    },
    created() {
        // this.Refresh();
        // this.changeDate();
    },
    methods: {
        DownloadFile(item) {
            this.downloadloading = true;
            PrtnCertificateService.PrintCertificatePdf(item.id2)
                .then((res) => {
                    this.downloadFile1(res, 'asdas');
                    this.downloadloading = false;
                })
                .catch((error) => {
                    this.showApiError(error);
                    this.downloadloading = false;
                });
        },

        getColor(item) {
            if (item.statusId == 24 || item.statusId == 25 || item.statusId == 23 || item.statusId == 5 || item.statusId == 3 || item.statusId == 10) {
                return 'red';
            } else if (
                item.statusId == 13 ||
                item.statusId == 11 ||
                item.statusId == 26 ||
                item.statusId == 9 ||
                item.statusId == 14 ||
                item.statusId == 16 ||
                item.statusId == 17 ||
                item.statusId == 18 ||
                item.statusId == 21 ||
                item.statusId == 2
            ) {
                return 'green';
            } else if (item.statusId == 6) {
                return 'orange';
            } else if (item.statusId == 7) {
                return '#7030A0';
            } else {
                return '#0669B4';
            }
        },
        downloadFile1(response, item) {
            var blob = new Blob([response.data]);
            const url = window.URL.createObjectURL(blob);
            const link = document.createElement('a');
            link.href = url;
            link.setAttribute('download', item.projectfiletext); //or any other extension
            document.body.appendChild(link);
            link.click();
        },
        OpenHistory(item) {
            ApplicationService.Get(item.id)
                .then((res) => {
                    this.histories = res.data.histories;
                    this.historySidebar = true;
                    this.History = [];
                    this.History = this.histories;
                })
                .catch((error) => {
                    this.showApiError(error);
                });
        },

        changeDate() {
            var DateNow = Date.now();
            this.PersentTime = DateNow;
        },
        Refresh() {
            // this.Loading = true;
            this.modalindex = this.$route.query.infomodal;
            PrtnCertificateService.GetList(this.dataFilter)
                .then((res) => {
                    this.Certificate = res.data.rows;
                    this.Loading = false;
                    if (this.modalindex == 1) {
                        this.InfoModal = true;
                        this.modalindex = 0;
                    }
                })
                .catch((error) => {
                    this.showApiError(error);
                    this.Loading = false;
                });
        }
    }
};
</script>
