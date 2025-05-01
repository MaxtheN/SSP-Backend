<template>
    <div>
        <AppListHeaderForName title="StateAssetApplication">
            <template #top-right>
                <b-button
                    @click="
                        $router.push({
                            name: 'EditStateAssetApplication',
                            params: { id: 0 }
                        })
                    "
                    variant="info"
                >
                    <b-icon-plus></b-icon-plus>
                    {{ $t('Add') }}
                </b-button>
            </template>
        </AppListHeaderForName>
        <div class="container-fluid">
            <CTable :items="Application" :fields="fields" bordered :busy="Loading">
                <template #cell(status)="{ item }">
                    <div class="d-flex align-items-center">
                        <AppStatusBadge :item="item" />
                    </div>
                </template>

                <template #cell(actions)="{ item }">
                    <div class="d-flex align-items-center justify-content-center">
                        <CButton
                            @click="
                                $router.push({
                                    name: 'EditStateAssetApplication',
                                    params: { id: item.id }
                                })
                            "
                            :icon="item.statusId == 8 || item.canModify ? 'pen' : 'eye'"
                        />
                    </div>
                </template>
            </CTable>

            <StateAssetApplicationForTashkent
                v-model="StateAssetApplicationmodal"
                @close="
                    $router.push({
                        name: 'Cabinet'
                    })
                "
            />
        </div>
    </div>
</template>

<script>
import axios from 'axios';
import CTable from '@/components/table/CTable.vue';
import StateAssetApplicationService from '@/services/stateassetapplication.service';
import AppListHeaderForName from '@/components/application/AppListHeaderForName.vue';
import AppTable from '@/components/application/AppTable.vue';
import AppCard from '@/components/application/AppCard.vue';
import StateAssetApplicationForTashkent from '@/components/application/StateAssetApplicationForTashkent.vue';
import AppStatusBadge from '@/components/application/AppStatusBadge.vue';
import CButton from '@/components/CButton.vue';

export default {
    components: {
        AppListHeaderForName,
        AppStatusBadge,
        AppTable,
        StateAssetApplicationForTashkent,
        AppCard,
        CTable,
        CButton
    },
    data() {
        return {
            axios,
            Application: [],
            DeleteLoading: false,
            downloadloading: false,
            StateAssetApplicationmodal: false,
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
                    key: 'status',
                    label: this.$t('status')
                },

                {
                    key: 'actions',
                    label: this.$t('actions')
                }
            ],

            SendLoading: false,
            CancelLoading: false,
            canCreate: false,
            FileModal: false,
            AcceptLoading: false,
            Loading: false,
            InfoModal: false,
            modalindex: 0,
            OrganizationFilial: '',
            ApplicationNumber: '',
            FileData: {},
            PersentTime: '',
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
            }
        };
    },
    created() {
        if (JSON.parse(localStorage.getItem('user_info')).contractor.regionId == 1) {
            this.StateAssetApplicationModalOpen();
        } else {
            this.Refresh();
            this.changeDate();
            StateAssetApplicationService.CanCreate().then((res) => {
                this.canCreate = res.data;
            });
        }
    },
    methods: {
        StateAssetApplicationModalOpen() {
            this.StateAssetApplicationmodal = true;
        },
        Download(id) {
            this.downloadloading = true;
            StateAssetApplicationService.GetAsPdf(id)
                .then((res) => {
                    this.forceFileDownload(res, 'stateassetapplication' + id, 'pdf');
                })
                .catch(this.showApiError)
                .finally(() => {
                    this.downloadloading = false;
                });
        },
        OpenFile(item) {
            this.FileModal = true;
            StateAssetApplicationService.GetMfyApplication(item.id2)
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
        OpenCancelModal(item) {
            this.filter.id = item.id;
            this.filter.message = '';
            this.$bvModal.show('CancelModal' + item.id);
        },
        changeDate() {
            var DateNow = Date.now();
            this.PersentTime = DateNow;
        },
        Cancel(item) {
            StateAssetApplicationService.Revoke(item.id)
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
            this.Loading = true;
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
            StateAssetApplicationService.GetList(this.dataFilter)
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
            StateAssetApplicationService.Send(this.filter)
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
            StateAssetApplicationService.Delete(item.id)
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
