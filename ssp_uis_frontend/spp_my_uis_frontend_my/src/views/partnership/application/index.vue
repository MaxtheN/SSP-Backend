<template>
    <div>
        <AppListHeaderForName title="PartnershipApplication">
            <template #top-right v-if="canCreate">
                <b-button @click="createModal = true" variant="info">
                    <b-icon-plus></b-icon-plus>
                    {{ $t('Add') }}
                </b-button>
            </template>
        </AppListHeaderForName>
        <div class="container-fluid">
            <CTable :items="Application" :fields="fields" bordered :busy="isBusy">
                <template #cell(status)="{ item }">
                    <div class="d-flex align-items-center">
                        <AppStatusBadge @click="OpenHistory(item)" :item="item" />
                        <router-link :to="{ name: 'Contract' }" style="font-size: 25px; margin-left: 15px; cursor: pointer !important">
                            <b-icon style="cursor: pointer !important" icon="file-earmark-arrow-down" variant="primary" scale="1"></b-icon>
                        </router-link>
                    </div>
                </template>
                <template #cell(prtnContractStatus)="{ item }">
                    <div class="d-flex align-items-center">
                        <AppStatusBadge @click="OpenHistory(item)" :item="{ ...item, statusId: item.prtnContractStatusId, status: item.prtnContractStatus }" />
                        <router-link :to="{ name: 'Contract' }" style="font-size: 25px; margin-left: 15px; cursor: pointer !important">
                            <b-icon style="cursor: pointer !important" icon="file-earmark-arrow-down" variant="primary" scale="1"></b-icon>
                        </router-link>
                    </div>
                </template>
                <template #cell(recommendation)="{ item }">
                    <b-button @click="OpenFile(item)" size="sm" class="p-0 py-1 px-2" variant="light">
                        <b-icon-eye scale="0.8"></b-icon-eye>
                        {{ $t('View') }}
                    </b-button>
                </template>
                <template #cell(actions)="{ item }">
                    <div class="d-flex align-items-center">
                        <CButton
                            class="mr-2"
                            :icon="item.statusId == 8 || item.canModify ? 'pen' : 'eye'"
                            @click="
                                $router.push({
                                    name: 'PartnershipApplicationEdit',
                                    params: { id: item.id }
                                })
                            "
                        />
                        <CButton
                            icon="file-earmark-arrow-down"
                            class="mr-2"
                            target="_bland"
                            :disabled="downloadloading"
                            :href="axios.defaults.baseURL + `Application/PrintApplicationPdf?Id=${item.id2}&lang=${getPdfLang()}`"
                        />

                        <CButton
                            @click="$bvModal.show('DeleteModal' + item.id)"
                            icon="trash"
                            v-if="item.statusId != 8 && item.statusId != 24 && item.statusId != 2 && item.statusId != 30"
                        />

                        <b-button v-if="item.statusId == 8" @click="OpenCancelModal(item)" class="p-1" size="sm" variant="warning">
                            <b-icon-x-circle scale="0.8"></b-icon-x-circle>
                        </b-button>
                    </div>
                    <b-modal :id="'AppModal' + item.id" no-close-on-backdrop hide-footer :title="$t('xulosa')">
                        <table>
                            <tr>
                                <td style="width: 60% !important">{{ $t('docnumber') }} :</td>
                                <th style="width: 40%">{{ AdoptPermisConclusion.docnumber }}</th>
                            </tr>
                            <tr>
                                <td>{{ $t('docdate') }} :</td>
                                <th>{{ AdoptPermisConclusion.docdate }}</th>
                            </tr>
                            <tr>
                                <td>{{ $t('commenttext') }} :</td>
                                <th>{{ AdoptPermisConclusion.commenttext }}</th>
                            </tr>
                            <tr>
                                <td>{{ $t('xulosa') }} :</td>
                                <th>
                                    <b-badge variant="primary" @click="DownloadFile(item)">
                                        <b-spinner small v-if="downloadloading"></b-spinner>
                                        <b-icon-cloud-download v-if="!downloadloading"></b-icon-cloud-download>
                                        {{ AdoptPermisConclusion.projectfiletext }}
                                    </b-badge>
                                </th>
                            </tr>
                        </table>
                        <b-row class="mt-3">
                            <b-col class="text-right">
                                <b-button @click="$bvModal.hide('AppModal' + item.id)" variant="success">ok</b-button>
                            </b-col>
                        </b-row>
                    </b-modal>
                    <b-modal :id="'DeleteModal' + item.id" :title="$t('delete')" no-close-on-backdrop hide-footer>
                        <p>{{ $t('WantDeleteAdm') }}</p>
                        <b-row>
                            <b-col style="text-align: end" class="text-right">
                                <a @click="$bvModal.hide('DeleteModal' + item.id)" style="margin-right: 5px" class="btn btn-sm btn-soft-danger mr-2 pr-btn">{{ $t('no') }}</a>
                                <a @click="Delete(item)" class="btn btn-sm btn-success pr-btn">
                                    <b-spinner v-if="DeleteLoading" small></b-spinner>
                                    {{ $t('yes') }}
                                </a>
                            </b-col>
                        </b-row>
                    </b-modal>
                    <b-modal :id="'SendModal' + item.id" :title="$t('send')" no-close-on-backdrop hide-footer>
                        <b-row class="mt-3">
                            <b-col style="text-align: end" class="text-right">
                                <a @click="$bvModal.hide('SendModal' + item.id)" class="btn btn-sm btn-soft-danger mr-2 pr-btn" style="margin-right: 5px">{{ $t('no') }}</a>
                                <a @click="Send(item)" class="btn btn-sm btn-success pr-btn">
                                    <b-spinner v-if="SendLoading" small></b-spinner>
                                    {{ $t('yes') }}
                                </a>
                            </b-col>
                        </b-row>
                    </b-modal>
                    <b-modal :id="'CancelModal' + item.id" :title="$t('Revoke')" no-close-on-backdrop hide-footer>
                        <p>{{ $t('WantRevoke') }}</p>
                        <b-row>
                            <b-col style="text-align: end" class="text-right">
                                <a @click="$bvModal.hide('CancelModal' + item.id)" class="btn btn-sm btn-soft-danger mr-2 pr-btn" style="margin-right: 5px">{{ $t('no') }}</a>
                                <a @click="Cancel(item)" class="btn btn-sm btn-success pr-btn">
                                    <b-spinner v-if="CancelLoading" small></b-spinner>
                                    {{ $t('yes') }}
                                </a>
                            </b-col>
                        </b-row>
                    </b-modal>
                </template>
            </CTable>

            <AppStartModal v-model="createModal" />
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
                                                            <b-badge :variant="getColor(el)"> {{ el.statusChangedDate }} </b-badge>-
                                                            <b-badge :variant="getColor(el)">{{ el.status }}</b-badge>
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
            <b-modal size="lg" :title="$t('File')" no-close-on-backdrop hide-footer v-model="FileModal" hide-header>
                <b-row>
                    <b-col sm="12" md="12" lg="12" class="text-center">
                        <b>{{ FileData.conclusingPersonFio }}</b>
                    </b-col>
                    <b-col sm="12" md="12" lg="12">
                        {{ $t('phonenumber') }} -
                        <b>{{ FileData.conclusingPersonPhone }}</b>
                    </b-col>
                    <b-col sm="12" md="12" lg="12">
                        {{ $t('createdAt') }} -
                        <b>{{ FileData.createdAt }}</b>
                    </b-col>
                    <b-col sm="12" md="12" lg="12" class="mt-2" style="height: 51vh">
                        <iframe :src="FileData.fileUrl" frameborder="0" style="height: 50vh" width="100%"></iframe>
                    </b-col>
                </b-row>
                <b-row class="mt-3">
                    <b-col sm="12" md="12" class="d-flex justify-content-end">
                        <a @click="FileModal = !FileModal" class="btn btn-sm btn-soft-danger mr-2 pr-btn">{{ $t('close') }}</a>
                    </b-col>
                </b-row>
            </b-modal>
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
        </div>
    </div>
</template>

<script>
import axios from 'axios';
import CTable from '../../../components/table/CTable.vue';
import ApplicationService from '@/services/application.service';
import CustomButton from '@/components/elements/customButton.vue';
import AppStartModal from '@/components/application/AppStartModal.vue';
import AppListHeaderForName from '@/components/application/AppListHeaderForName.vue';
import AppStatusBadge from '@/components/application/AppStatusBadge.vue';
import CButton from '@/components/CButton.vue';

export default {
    components: {
        CButton,
        CTable,
        CustomButton,
        AppStartModal,
        AppStatusBadge,
        AppListHeaderForName
    },
    data() {
        return {
            axios,
            Application: [],
            DeleteLoading: false,
            isBusy: false,
            filter: {
                id: 0,
                message: ''
            },
            buttonfilter: {
                admissiontypeid: 0
            },
            EducationTypeList: [
                {
                    id: 1,
                    shortname: this.$t('DTM')
                },
                {
                    id: 2,
                    shortname: this.$t('HEMIS')
                }
            ],
            SendLoading: false,
            CancelLoading: false,
            createModal: false,
            canCreate: false,
            FileModal: false,
            AcceptLoading: false,
            Loading: false,
            historySidebar: false,
            InfoModal: false,
            modalindex: 0,
            OrganizationFilial: '',
            ApplicationNumber: '',
            History: [],
            histories: [],
            GetInfoData: {},
            FileData: {},
            downloadloading: false,
            AdoptPermisConclusion: {
                docnumber: '',
                docdate: '',
                commenttext: '',
                projectfiletext: ''
            },
            ApplicationList: {},
            dataFilter: {
                search: null,
                sortBy: null,
                orderType: null,
                page: 1,
                pageSize: 20
            },
            fields: [
                {
                    key: 'contractor',
                    label: this.$t('contractor')
                },
                {
                    key: 'contractorInn',
                    label: this.$t('inn')
                },
                {
                    key: 'docNumber',
                    label: this.$t('docNumberApplication')
                },
                {
                    key: 'docOn',
                    label: this.$t('docDateApplication')
                },
                {
                    key: 'contractorInn',
                    label: this.$t('contractorInn')
                },
                {
                    key: 'prtnContractType',
                    label: this.$t('type')
                },
                {
                    key: 'status',
                    label: this.$t('status')
                },
                {
                    key: 'prtnContractStatus',
                    label: this.$t('prtnContractStatus')
                },
                {
                    key: 'actions',
                    label: this.$t('actions')
                },
                {
                    key: 'recommendation',
                    label: this.$t('recommendation')
                }
            ]
        };
    },
    created() {
        ApplicationService.CanCreateApplication(JSON.parse(localStorage.getItem('user_info')).contractor.inn).then((res) => {
            this.canCreate = res.data;
        });
        this.Refresh();
    },
    methods: {
        hasan() {
            console.log('dd');
        },
        OpenFile(item) {
            this.FileModal = true;
            ApplicationService.GetMfyApplication(item.id2)
                .then((res) => {
                    this.FileData = res.data;
                })
                .catch((error) => {
                    this.showApiError(error);
                });
        },
        DownloadFile(item) {
            this.downloadloading = true;
            FileManageService.Get(item.AdoptPermisConclusion.projectfileid)
                .then((res) => {
                    this.downloadFile1(res, item.AdoptPermisConclusion);
                    this.downloadloading = false;
                })
                .catch((error) => {
                    this.showApiError(error);
                    this.downloadloading = false;
                });
        },
        ChangeAdm(item) {
            console.log(item);
            this.buttonfilter.admissiontypeid = item.id;
            this.Refresh();
        },
        OpenAppModal(item) {
            this.$bvModal.show('AppModal' + item.id);
            this.AdoptPermisConclusion = item.AdoptPermisConclusion;
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
                .catch(this.showApiError);
        },
        OpenCancelModal(item) {
            this.filter.id = item.id;
            this.filter.message = '';
            this.$bvModal.show('CancelModal' + item.id);
        },
        Cancel(item) {
            ApplicationService.Revoke(item.id)
                .then((res) => {
                    this.CancelLoading = false;
                    this.$bvModal.hide('CancelModal' + this.filter.id);
                    this.Refresh();
                })
                .catch((error) => {
                    this.showApiError(error);
                    this.$bvModal.hide('CancelModal' + this.filter.id);
                    this.CancelLoading = false;
                });
        },

        Refresh() {
            // this.Loading = true;
            this.isBusy = true;
            this.modalindex = this.$route.query.infomodal;
            this.OrganizationFilial = this.$route.query.organizationFilial;
            this.ApplicationNumber = this.$route.query.applicationNumber;
            if (!!this.$route.query.infomodal) {
                this.$router.replace({
                    query: {
                        infomodal: null,
                        organizationFilial: null,
                        applicationNumber: null
                    }
                });
            }
            ApplicationService.GetList(this.dataFilter)
                .then((res) => {
                    this.Application = res.data.rows;
                    this.ApplicationList = res.data;
                    this.Loading = false;
                    if (this.modalindex == 1) {
                        this.InfoModal = true;
                        this.modalindex = 0;
                    }
                })
                .catch((error) => {
                    this.showApiError(error);
                    this.Loading = false;
                })
                .finally(() => {
                    this.isBusy = false;
                });
        },
        OpenSendModal(item) {
            this.filter.id = item.id;
            this.filter.message = '';
            this.$bvModal.show('SendModal' + item.id);
        },
        Send(item) {
            this.SendLoading = true;
            this.filter.message = '';
            ApplicationService.Send(this.filter)
                .then((res) => {
                    this.SendLoading = false;
                    this.$bvModal.hide('SendModal' + item.id);
                    this.modalindex = 1;
                    this.Refresh();
                })
                .catch((error) => {
                    this.showApiError(error);
                    this.SendLoading = false;
                });
        },
        Delete(item) {
            this.DeleteLoading = true;
            ApplicationService.Delete(item.id)
                .then((res) => {
                    this.DeleteLoading = false;
                    this.makeToast(this.$t('DeleteSuccess'), 'success');
                    this.$bvModal.hide('DeleteModal' + item.id);
                    this.Refresh();
                })
                .catch((error) => {
                    this.showApiError(error);
                    this.DeleteLoading = false;
                });
        }
    }
};
</script>

<style lang="scss">
.modal-text-style {
    padding: 10px;
    margin: 10px;
}
</style>
