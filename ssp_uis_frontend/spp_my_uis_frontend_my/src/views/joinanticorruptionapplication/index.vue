<template>
    <div>
        <AppListHeaderForName title="JoinAntiCorruptionApplication">
            <template #top-right v-if="canCreate">
                <b-button @click="ofertaModal = true" variant="info">
                    <b-icon-plus></b-icon-plus>
                    {{ $t('Add') }}
                </b-button>
            </template>
        </AppListHeaderForName>
        <div class="container-fluid">
            <b-modal v-if="0" class="modal align-items-center pa-0" size="lg" v-model="dialog" hide-footer>
                <div @click="throwTest">
                    <b-alert show variant="success mb-0 cursor-pointer ">So'rovnoma</b-alert>
                </div>
            </b-modal>
            <CTable :items="Application" :fields="fields" bordered :busy="Loading" :filter="filter" @request="Refresh">
                <template #cell(status)="{ item }">
                    <div class="d-flex align-items-center justify-content-center">
                        <AppStatusBadge :item="item" />
                    </div>
                </template>
                <template #cell(contractor)="{ item }">
                    {{ item.application.contractor }}
                </template>
                <template #cell(docNumber)="{ item }">
                    {{ item.application.docNumber }}
                </template>
                <template #cell(contractorInn)="{ item }">
                    {{ item.application.contractorInn }}
                </template>
                <template #cell(docOn)="{ item }">
                    {{ item.application.docOn }}
                </template>

                <template #cell(actions)="{ item }">
                    <div class="d-flex align-items-center justify-content-center">
                        <CButton
                            icon="eye"
                            @click="
                                $router.push({
                                    name: 'JoinAntiCorruptionApplicationEdit',
                                    params: { id: item.id }
                                })
                            "
                        />
                    </div>
                </template>
            </CTable>

            <CorruptionOferta v-if="ofertaModal" v-model="ofertaModal" @accept="CreateApplicationEdit" />
        </div>
    </div>
</template>

<script>
import JoinAntiCorruptionApplicationService from '@/services/joinanticorruptionapplication.service';
import AppStatusBadge from '@/components/application/AppStatusBadge.vue';
import CTable from '@/components/table/CTable.vue';
import CButton from '@/components/CButton.vue';
import AppListHeaderForName from '@/components/application/AppListHeaderForName.vue';
const CorruptionOferta = () => import('./components/CorruptionOferta.vue');

export default {
    components: {
        AppStatusBadge,
        CTable,
        AppListHeaderForName,
        CorruptionOferta,
        CButton
    },
    data() {
        return {
            dialog: true,
            Application: [],
            canCreate: false,
            ofertaModal: false,
            Loading: false,
            downloadloading: false,
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
            filter: {
                search: null,
                sortBy: null,
                orderType: null,
                page: 1,
                pageSize: 10,
                total: 0
            }
        };
    },
    created() {
        if (this.$route.query.modaClose == true) {
            this.dialog = false;
        }

        JoinAntiCorruptionApplicationService.CanCreate().then((res) => {
            this.canCreate = res.data;
        });
    },
    methods: {
        CreateApplicationEdit() {
            this.$router.push({ name: 'JoinAntiCorruptionApplicationEdit', params: { id: 0 } });
        },
        Refresh() {
            this.Loading = true;
            JoinAntiCorruptionApplicationService.GetList(this.filter)
                .then((res) => {
                    this.Application = res.data.rows;
                    this.filter.total = res.data.total;
                })
                .catch(this.showApiError)
                .finally(() => {
                    this.Loading = false;
                });
        },
        Download(id) {
            this.downloadloading = true;
            JoinAntiCorruptionApplicationService.DownloadFile(id)
                .then((res) => {
                    this.forceFileDownload(res, 'JoinAntiCorruptionApplication_' + id, 'pdf');
                })
                .catch(this.showApiError)
                .finally(() => {
                    this.downloadloading = false;
                });
        },
        throwTest() {
            this.dialog = false;
            this.$router.push({ name: 'QuestionnarieView', params: { id: 4 } });
        }
    }
};
</script>

<style>
.modal-header {
    padding: 5px 0;
    display: flex !important;
    justify-content: center !important;
}

.modal-header .modal-title {
    display: none !important;
}

.modal-header .close {
    display: block;
    padding: 0;
    margin: 0;
    font-size: 40px;
    color: red;
    text-align: center;
}
</style>
