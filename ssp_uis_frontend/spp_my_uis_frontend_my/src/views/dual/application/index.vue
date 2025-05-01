<template>
    <div>
        <AppListHeaderForName page-name="dual" title="DualApplication">
            <template #top-right v-if="canCreate">
                <b-button
                    @click="
                        $router.push({
                            name: 'DualApplicationEdit',
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
            <CTable :items="Application" :fields="fields" bordered :busy="Loading" :filter="dataFilter" @request="Refresh">
                <template #cell(status)="{ item }">
                    <div class="d-flex align-items-center">
                        <AppStatusBadge :item="item" />
                    </div>
                </template>

                <template #cell(actions)="{ item }">
                    <div class="d-flex align-items-center justify-content-center">
                        <CButton
                            :icon="item.canEdit ? 'pencil' : 'eye'"
                            class="mr-2"
                            @click="
                                $router.push({
                                    name: 'DualApplicationEdit',
                                    params: { id: item.id }
                                })
                            "
                        />

                        <CButton v-if="item.statusId == 8" @click="OpenCancelModal(item)" icon="x-circle" class="mr-2" />
                        <CButton v-if="item.canSend" @click="OpenSendModal(item)" icon="arrow-bar-up" />
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
                    </div>
                </template>
            </CTable>
        </div>
        <b-modal id="ESPmodal" :title="$t('ESPmodal')" no-close-on-backdrop hide-footer>
            <just-sign @sign="GetESP($event)"></just-sign>
            <b-row>
                <b-col style="text-align: end" class="text-right">
                    <a @click="$bvModal.hide('ESPmodal')" class="btn btn-sm btn-soft-danger mr-2 pr-btn" style="margin-right: 5px">{{ $t('no') }}</a>
                </b-col>
            </b-row>
        </b-modal>

        <b-sidebar no-header width="400px" shadow right v-model="historySidebar" bg-variant="white">
            <div style="width: 100%; height: 100%">
                <div class="container-fluid w-100" style="width: 100% !important; position: relative; overflow-y: auto">
                    <b-row class="w-100">
                        <b-col class="text-right close-icon">
                            <b-icon-x scale="2.5" style="cursor: pointer; z-index: 9" @click="historySidebar = false"></b-icon-x>
                        </b-col>
                    </b-row>
                    <b-row class="p-0" style="height: 80vh !important">
                        <b-col v-for="(item, index) in chatData" :key="index">
                            {{ item }}
                        </b-col>
                    </b-row>
                    <b-row>
                        <b-col class="align-items-center">
                            <b-button-group>
                                <WTextarea style="width: 250px" :name="$t('messsage')" :placeholder="$t('messsage')" v-model="createChatData.messageText" />
                                <b-button @click="CreateChat" variant="primary" style="max-height: 80px">
                                    <b-spinner v-if="chatLoading" small style="margin-right: 8px"></b-spinner>
                                    <b-icon-play-fill></b-icon-play-fill>
                                </b-button>
                            </b-button-group>
                        </b-col>
                    </b-row>
                </div>
            </div>
        </b-sidebar>
    </div>
</template>

<script>
import axios from 'axios';
import WTextarea from '@/components/forms/WTextarea.vue';
import DualApplicationService from '@/services/dual/dualapplication.service';
import CustomButton from '@/components/elements/customButton.vue';
import AppListHeaderForName from '@/components/application/AppListHeaderForName.vue';
import AppStatusBadge from '@/components/application/AppStatusBadge.vue';
import AppCard from '@/components/application/AppCard.vue';
import AppTable from '@/components/application/AppTable.vue';
import DocumentChatService from '@/services/documentchat.service';
import JustSign from '@/components/justSign.vue';
import CTable from '@/components/table/CTable.vue';
import CButton from '@/components/CButton.vue';

export default {
    components: {
        CustomButton,
        JustSign,
        WTextarea,
        AppStatusBadge,
        AppCard,
        AppListHeaderForName,
        AppTable,
        CTable,
        CButton
    },
    data() {
        return {
            axios,
            Application: [],
            filter: {
                id: 0,
                message: ''
            },
            chatFilter: {
                tableId: 110,
                documentId: 0,
                search: '',
                sortBy: '',
                orderType: '',
                page: 1,
                pageSize: 1000
            },

            CancelLoading: false,
            createModal: false,
            canCreate: false,
            Loading: false,
            chatData: [],
            historySidebar: false,
            downloadloading: false,
            chatLoading: false,
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
                    key: 'oked',
                    label: this.$t('oked')
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
            createChatData: {}
        };
    },
    created() {
        DualApplicationService.CanCreate().then((res) => {
            this.canCreate = res.data;
        });
    },
    watch: {
        historySidebar: {
            handler(newValue) {
                if (newValue) {
                    DocumentChatService.GetList(this.chatFilter)
                        .then((res) => {
                            this.chatData = res.data.rows;
                        })
                        .catch((error) => {
                            this.showApiError(error);
                        });
                }
            }
        }
    },
    methods: {
        CreateChat() {
            this.chatLoading = true;
            this.createChatData = { ...this.createChatData, tableId: 110, documentId: 9358 };
            DocumentChatService.Create(this.createChatData)
                .then((res) => {
                    this.chatData = res.data.rows;
                })
                .catch((error) => {
                    this.showApiError(error);
                })
                .finally(() => {
                    this.chatLoading = false;
                });
        },

        OpenCancelModal(item) {
            this.filter.id = item.id;
            this.filter.message = '';
            this.$bvModal.show('CancelModal' + item.id);
        },
        Cancel(item) {
            DualApplicationService.Revoke(item.id)
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
            DualApplicationService.GetList(this.dataFilter)
                .then((res) => {
                    this.Application = res.data.rows;
                    this.Loading = false;
                })
                .catch((error) => {
                    this.showApiError(error);
                    this.Loading = false;
                });
        },
        Delete(item) {
            this.DeleteLoading = true;
            DualApplicationService.Delete(item.id)
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
        },
        OpenSendModal(item) {
            this.filter.id = item.id;
            this.filter.message = '';
            this.$bvModal.show('ESPmodal');
        },
        GetESP(data) {
            this.$bvModal.hide('ESPmodal');

            this.Sign(data);
        },
        Sign(item) {
            this.SendLoading = true;
            DualApplicationService.Send({ id: this.filter.id, message: this.filter.message, signedData: item.key })
                .then(() => {
                    this.$bvModal.hide('SendModal' + item.id);
                    this.makeToast(this.$t('SuccessSend'), 'success');
                    this.$router.push({ name: 'DualApplication' });
                })
                .catch((error) => {
                    this.showApiError(error);
                })
                .finally(() => {
                    this.SendLoading = false;
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
