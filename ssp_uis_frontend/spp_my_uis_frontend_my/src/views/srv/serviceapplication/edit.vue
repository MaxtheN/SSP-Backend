<template>
    <div>
        <AppListHeaderForName title="ServiceApplication" page-name="ServiceApplication" :query="{ type: $route.query.type }" />
        <div class="container">
            <b-overlay :show="Loading">
                <b-row class="justify-content-center">
                    <b-col sm="12" lg="9">
                        <b-card class="form-card">
                            <b-tabs pills card lazy @change="iframeLoaded = false">
                                <b-tab :active="tabActive == 1" :title="$t('AdditionalInfo')">
                                    <validation-observer ref="ValidationDTO" v-if="Data.application">
                                        <b-card-text>
                                            <b-row>
                                                <b-col sm="12" md="6" lg="6">
                                                    <WInput
                                                        rules="required"
                                                        :label="$t('docNumberApplication')"
                                                        name="docNumberApplication"
                                                        disabled
                                                        v-model="Data.application.docNumber"
                                                    />
                                                </b-col>
                                                <b-col sm="12" md="6" lg="6">
                                                    <WInput
                                                        rules="required"
                                                        :label="$t('docDateApplication')"
                                                        name="docDateApplication"
                                                        disabled
                                                        v-model="Data.application.docOn"
                                                    />
                                                </b-col>
                                                <b-col sm="12" md="12" lg="12">
                                                    <WInput rules="required" :label="$t('contractor')" name="contractor" disabled v-model="Data.application.contractor" />
                                                </b-col>

                                                <b-col sm="12" md="6" lg="6">
                                                    <WInput rules="required" :label="$t('inn')" name="inn" disabled v-model="Data.application.contractorInn" />
                                                </b-col>
                                                <b-col sm="12" md="6" lg="6">
                                                    <AppContractorSettlementAccount v-model="Data.application.contractorSettlementAccountId" />
                                                </b-col>
                                                <b-col sm="12" md="6" lg="6">
                                                    <WInput
                                                        rules="required"
                                                        :label="$t('director')"
                                                        name="contractorPositionName"
                                                        v-model="Data.application.contractorPositionName"
                                                    />
                                                </b-col>
                                                <b-col sm="12" md="6" lg="6">
                                                    <WSelect
                                                        :placeholder="$t('select')"
                                                        :options="RegionList"
                                                        :label="$t('liveoblastnameserv')"
                                                        v-model="Data.regionId"
                                                        @input="ChangeRegion"
                                                    />
                                                </b-col>
                                                <b-col sm="12" md="6" lg="6">
                                                    <WSelect
                                                        :rules="!Data.toRegionalOffice ? `required` : ''"
                                                        :disabled="Data.toRegionalOffice"
                                                        :placeholder="$t('select')"
                                                        :options="DistrictList"
                                                        :label="$t('liveregionname')"
                                                        v-model="Data.districtId"
                                                    />
                                                </b-col>
                                                <b-col sm="12" md="6" lg="6" class="d-flex align-items-center">
                                                    <b-form-checkbox v-model="Data.toRegionalOffice" @input="changeChekbox">
                                                        {{ $t('servesHududiy') }}
                                                    </b-form-checkbox>
                                                </b-col>
                                            </b-row>

                                            <template v-for="(priceTable, i) in Data.groups">
                                                <table class="priceTable mt-3 w-100" v-if="priceTable.tables.length" :key="i + 'groupth'">
                                                    <thead>
                                                        <tr>
                                                            <th class="text-center" colspan="3">{{ priceTable.group }}</th>
                                                            <th></th>
                                                        </tr>
                                                    </thead>
                                                    <tbody :key="i + 'grouptb'">
                                                        <tr v-for="(tab, j) in priceTable.tables" :key="j + 'tab' + i">
                                                            <td class="text-center w-50px">
                                                                <b-form-checkbox v-if="Data.id == 0" v-model="tab.isChecked" />
                                                                <b-icon v-else icon="check-square" />
                                                            </td>
                                                            <td v-if="tab.needChamberServiceId != 149" :colspan="WihtOfferta[tab.id] ? 1 : 2">{{ tab.needChamberService }}</td>
                                                            <td v-if="tab.needChamberServiceId == 149" :colspan="WihtOfferta[tab.id] ? 1 : 2">
                                                                <WInput :disabled="!tab.isChecked" v-model="tab.forBoshqaMessage" :placeholder="$t('forBoshqaMessage')" />
                                                            </td>
                                                            <td v-if="WihtOfferta[tab.id] && tab.needChamberServiceId != 149">
                                                                <WInput v-model="tab.offerServiceText" :placeholder="$t('offerServiceText')" />
                                                            </td>
                                                            <td v-if="tab.needChamberServiceId != 149">
                                                                <p class="mb-0" v-if="tab.servicePriceType">{{ tab.servicePriceType }}</p>
                                                                <p class="mb-0" v-if="tab.concreteCoef">{{ tab.concreteCoef }}</p>
                                                                <p class="mb-0" v-if="tab.beginCoef && tab.endCoef">{{ tab.beginCoef }}-{{ tab.endCoef }}</p>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </template>
                                        </b-card-text>
                                    </validation-observer>
                                </b-tab>

                                <b-tab :active="tabActive == 2" no-body :title="$t('Application')">
                                    <b-overlay :show="!iframeLoaded" spinner-variant="primary" spinner-type="grow" rounded="sm">
                                        <iframe
                                            v-if="Data.application && Data.application.id && Data.application.id2"
                                            :src="IframeSrc"
                                            width="100%"
                                            style="height: 100vh"
                                            frameborder="0"
                                            @load="iframeLoaded = true"
                                        ></iframe>
                                    </b-overlay>
                                </b-tab>
                            </b-tabs>
                        </b-card>
                    </b-col>

                    <b-col sm="12" lg="3" v-if="Data.canEdit || $route.params.id == 0 || Data.canDelete || Data.canSend || Data.canAccept">
                        <b-card class="form-card">
                            <b-card-text>
                                <b-button @click="SaveData" v-if="Data.canEdit || $route.params.id == 0" variant="success" block class="mb-1">
                                    <b-spinner v-if="saveLoading" small></b-spinner>
                                    <b-icon-check scale="0.8"></b-icon-check>
                                    {{ $t('save') }}
                                </b-button>
                                <!-- download -->

                                <!-- delete -->
                                <b-button v-if="Data.canDelete" @click="$bvModal.show('DeleteModal' + Data.id)" variant="danger" block class="mb-1">
                                    <b-icon-trash scale="0.6"></b-icon-trash>
                                    {{ $t('delete') }}
                                </b-button>
                                <!-- send -->
                                <b-button v-if="Data.canSend" @click="OpenSendModal(Data)" variant="primary" block class="mb-1">
                                    <b-icon-arrow-bar-up scale="0.6"></b-icon-arrow-bar-up>
                                    {{ $t('Send') }}
                                </b-button>
                                <!-- cancel  -->

                                <!-- accept  -->
                                <b-button v-if="Data.canAccept && Data.isFree" @click="acceptDialog = true" variant="success" block class="mb-1">
                                    <b-icon-check scale="0.6" />
                                    {{ $t('Accept') }}
                                </b-button>
                            </b-card-text>
                        </b-card>
                    </b-col>
                </b-row>
            </b-overlay>

            <b-modal :id="'DeleteModal' + Data.id" :title="$t('delete')" no-close-on-backdrop hide-footer>
                <p>{{ $t('WantDeleteAdm') }}</p>
                <b-row>
                    <b-col style="text-align: end" class="text-right">
                        <a @click="$bvModal.hide('DeleteModal' + Data.id)" style="margin-right: 5px" class="btn btn-sm btn-soft-danger mr-2 pr-btn">{{ $t('no') }}</a>
                        <a @click="Delete(Data)" class="btn btn-sm btn-success pr-btn">
                            <b-spinner v-if="DeleteLoading" small></b-spinner>
                            {{ $t('yes') }}
                        </a>
                    </b-col>
                </b-row>
            </b-modal>

            <b-modal :id="'SendModal' + Data.id" :title="$t('send')" no-close-on-backdrop hide-footer>
                <b-row class="mt-3">
                    <b-col style="text-align: end" class="text-right">
                        <a @click="$bvModal.hide('SendModal' + Data.id)" class="btn btn-sm btn-soft-danger mr-2 pr-btn" style="margin-right: 5px">{{ $t('no') }}</a>

                        <b-button variant="success" v-b-modal.ESPmodal>
                            <b-spinner v-if="SendLoading" small></b-spinner>
                            {{ $t('yes') }}
                        </b-button>
                    </b-col>
                </b-row>
            </b-modal>

            <b-modal :id="'CancelModal' + Data.id" :title="$t('Cancel')" no-close-on-backdrop hide-footer>
                <p>{{ $t('WantCancel') }}</p>
                <b-row>
                    <b-col style="text-align: end" class="text-right">
                        <a @click="$bvModal.hide('CancelModal' + Data.id)" class="btn btn-sm btn-soft-danger mr-2 pr-btn" style="margin-right: 5px">{{ $t('no') }}</a>
                        <a @click="Cancel(Data)" class="btn btn-sm btn-success pr-btn">
                            <b-spinner v-if="CancelLoading" small></b-spinner>
                            {{ $t('yes') }}
                        </a>
                    </b-col>
                </b-row>
            </b-modal>

            <b-modal id="ESPmodal" :title="$t('ESPmodal')" no-close-on-backdrop hide-footer>
                <just-sign @sign="GetESP($event)"></just-sign>

                <b-row>
                    <b-col style="text-align: end" class="text-right">
                        <a @click="$bvModal.hide('ESPmodal')" class="btn btn-sm btn-soft-danger mr-2 pr-btn" style="margin-right: 5px">{{ $t('no') }}</a>
                        <a @click="Send(Data)" class="btn btn-sm btn-success pr-btn">
                            <b-spinner v-if="CancelLoading" small></b-spinner>
                            {{ $t('yes') }}
                        </a>
                    </b-col>
                </b-row>
            </b-modal>

            <!-- accept -->
            <b-modal size="lg" no-enforce-focus :title="$t('Services')" v-model="acceptDialog" hide-footer>
                <WInput rules="required" :label="$t('message')" name="message" v-model="filterAccept.message" />

                <table class="priceTable mt-2 w-100" v-for="(priceTable, i) in groups" :key="i + 'group'">
                    <thead>
                        <tr>
                            <th class="text-center" colspan="2">{{ priceTable.group }}</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="(tab, j) in priceTable.tables" :key="j + 'tab' + i">
                            <td>{{ tab.needChamberService }}</td>
                            <td class="text-center" style="width: 70px">
                                <b-form-checkbox v-model="tab.isCompleted" />
                            </td>
                        </tr>
                    </tbody>
                </table>

                <b-button @click="Accept()" class="w-100 mb-1 mt-2" :disabled="AcceptLoading" variant="outline-success">
                    <b-spinner v-if="AcceptLoading" small></b-spinner>

                    {{ $t('Accept') }}
                </b-button>
            </b-modal>
            <Chat v-if="Data && Data.id" :table-id="Data.tableId || 117" :document-id="Data.id" />
        </div>
    </div>
</template>

<script>
// components
import WCurrencyInput from '@/components/forms/WCurrencyInput.vue';
import WSelect from '@/components/forms/WSelect.vue';
import WTextarea from '@/components/forms/WTextarea.vue';
import WInput from '@/components/forms/WInput.vue';
import WPhoneInput from '@/components/forms/WPhoneInput.vue';
import JustSign from '@/components/justSign.vue';
import ServicePriceService from '@/services/srv/serviceprice.service';
import ServiceApplicationService from '@/services/srv/serviceapplication.service';
import AppListHeaderForName from '@/components/application/AppListHeaderForName.vue';
import axios from 'axios';
import ApplicationMixin from '@/mixins/application';
import NeedChamberServiceService from '../../../services/needchamberservice.service';
import AppContractorSettlementAccount from '@/components/application/AppContractorSettlementAccount.vue';
import ManualService from '@/services/manual.service';
const Chat = () => import('@/components/DocumentChat/Chat.vue');

export default {
    components: {
        WCurrencyInput,
        WSelect,
        WTextarea,
        WInput,
        WPhoneInput,
        JustSign,
        AppListHeaderForName,
        AppContractorSettlementAccount,
        Chat
    },
    mixins: [ApplicationMixin],
    data() {
        return {
            iframeLoaded: false,
            axios,
            Data: {
                applicationId: 0,
                toRegionalOffice: false,
                application: {
                    docOn: '',
                    docNumber: '',
                    contractorPositionName: '',
                    applicationTypeId: 7
                },
                groups: []
            },
            tabActive: 1,
            ServicePriceList: [],
            filter: {
                id: 0,
                message: '',
                signedData: ''
            },
            filterAccept: {
                id: 0,
                message: ''
            },
            ESPmodal: false,
            acceptDialog: false,
            groups: [],
            List: [],
            RegionList: [],
            DistrictList: [],
            WihtOfferta: {},
            isOkedDisabled: false,
            Loading: false,
            saveLoading: false,
            fileLoading: false,
            DeleteLoading: false,
            SendLoading: false,
            CancelLoading: false,
            AcceptLoading: false,
            downloadLoading: false
        };
    },
    computed: {
        IframeSrc() {
            return axios.defaults.baseURL + `srv/ServiceApplication/DownloadPdf?id2=${this.Data.application.id2}&lang=${this.getPdfLang()}`;
        },
        FileSrc() {
            return (id) => axios.defaults.baseURL + `srv/ServiceApplication/DownloadFile/${id}`;
        }
    },
    created() {
        ManualService.RegionSelectList(this.langId)
            .then((res) => {
                this.RegionList = res.data;
            })
            .catch((error) => {
                this.showApiError(error);
            });
        ServicePriceService.GroupingByServicePrice({
            needChamberServiceGroupId: Number(this.$route.query.group),
            isPaid: this.$route.query.type == 2 ? true : this.$route.query.type == 3 ? false : true
        }).then((res) => {
            this.Refresh();
            this.ServicePriceList = res.data;
        });

        NeedChamberServiceService.WihtOfferta().then((res) => {
            this.WihtOfferta = res.data;
        });
    },
    methods: {
        changeChekbox(e) {
            if (e) {
                this.Data.districtId = null;
            }
        },

        ChangeRegion() {
            this.Data.districtId = '';
            this.getDistrictList();
        },
        getDistrictList() {
            if (!!this.Data.regionId) {
                ManualService.DistrictSelectList(this.Data.regionId)
                    .then((res) => {
                        this.DistrictList = res.data;
                    })
                    .catch((error) => {
                        this.showApiError(error);
                    });
            }
        },
        GetESP(data) {
            this.filter.signedData = data.key;
            this.$bvModal.hide('ESPmodal');

            this.Send(this.Data);
        },
        FileDownload(item) {
            this.downloadLoading = true;
            ServiceApplicationService.DownloadPdf(item.id2, this.getPdfLang())
                .then((res) => {
                    this.forceFileDownload(res, this.$t('Serviceapplication'));
                })
                .catch(this.showApiError)
                .finally(() => {
                    this.downloadLoading = false;
                });
        },
        UploadFile(event) {
            const formData = new FormData();
            formData.append('files', event.target.files[0]);
            this.fileLoading = true;
            ServiceApplicationService.UploadFiles(formData)
                .then((res) => {
                    this.Data.files.push(...res.data);
                })
                .catch(this.showApiError)
                .finally(() => {
                    this.fileLoading = false;
                });
        },
        DeleteFile(id) {
            ServiceApplicationService.DeleteFile(id).then(() => {
                this.Data.files = this.Data.files.filter((item) => item.id != id);
            });
        },
        Refresh() {
            this.Loading = true;
            ServiceApplicationService.Get(this.$route.params.id)
                .then((res) => {
                    this.Data = res.data;
                    this.groups = res.data.groups.map((e) => ({
                        id: e.id,
                        groupId: e.groupId,
                        group: e.group,
                        tables: e.tables.map((j) => ({ ...j, id: j.id, isCompleted: false }))
                    }));
                    if (this.$route.params.id == '0') {
                        this.Data.groups = this.ServicePriceList.map((group) => {
                            const dGroup = res.data.groups.find((e) => e.groupId == group.groupId);

                            const tables = group.tables.map((table) => {
                                const gTable = dGroup ? dGroup.tables.find((e) => e.needChamberServiceId == table.needChamberServiceId) : null;
                                return {
                                    ...table,
                                    id: this.$route.params.id != 0 ? gTable?.id || 0 : 0,
                                    needChamberServiceId: table.needChamberServiceId,
                                    needChamberService: table.needChamberService,
                                    offerServiceText: gTable?.offerServiceText || '',
                                    files: [],
                                    isChecked: dGroup ? !!gTable : false
                                };
                            });

                            return {
                                ...group,
                                id: dGroup?.id || 0,
                                tables: tables
                            };
                        });
                    } else {
                        if (this.Data.canAccept && this.Data.isFree) {
                            this.acceptDialog = true;
                        }
                    }
                })
                .catch(this.showApiError)
                .finally(() => {
                    this.Loading = false;
                });
        },
        OpenCancelModal(item) {
            this.filter.id = item.id;
            this.filter.message = '';
            this.$bvModal.show('CancelModal' + item.id);
        },
        OpenSendModal(item) {
            this.filter.id = item.id;
            this.filter.message = '';
            this.$bvModal.show('SendModal' + item.id);
        },
        Delete(item) {
            this.DeleteLoading = true;
            ServiceApplicationService.Delete(item.id)
                .then(() => {
                    this.DeleteLoading = false;
                    this.makeToast(this.$t('DeleteSuccess'), 'success');
                    this.$bvModal.hide('DeleteModal' + item.id);

                    this.$router.push({ name: 'sspapplication' });
                })
                .catch((error) => {
                    this.showApiError(error);
                    this.DeleteLoading = false;
                });
        },
        Send(item) {
            this.SendLoading = true;
            ServiceApplicationService.Send(this.filter)
                .then((res) => {
                    this.SendLoading = false;
                    this.$bvModal.hide('SendModal' + item.id);
                    this.$router.push({ name: 'ServiceApplication' });
                })
                .catch((error) => {
                    this.showApiError(error);
                    this.SendLoading = false;
                });
        },
        Cancel(item) {
            ServiceApplicationService.Cancel(this.filter)
                .then(() => {
                    this.CancelLoading = false;
                    this.$bvModal.hide('CancelModal' + item.id);
                    this.Refresh();
                })
                .catch((error) => {
                    this.showApiError(error);
                    this.$bvModal.hide('CancelModal' + item.id);
                    this.CancelLoading = false;
                });
        },
        Accept() {
            this.AcceptLoading = true;
            ServiceApplicationService.Accept({ ...this.filterAccept, id: this.Data.id, groups: this.groups })
                .then(() => {
                    this.acceptDialog = false;
                    this.Refresh();
                })
                .catch((error) => {
                    this.showApiError(error);
                })
                .finally(() => {
                    this.AcceptLoading = false;
                });
        },
        SaveData() {
            this.$refs.ValidationDTO.validateWithInfo().then(({ isValid, errors }) => {
                if (isValid) {
                    this.saveLoading = true;
                    this.Data.isFree = this.$route.query.type == 2 ? false : this.$route.query.type == 3 ? true : false;
                    this.Data.applicationId = null;
                    const bodyData = { ...this.Data, groups: this.Data.groups.map((e) => ({ ...e, tables: e.tables.filter((a) => a.isChecked) })) };
                    ServiceApplicationService.Update(bodyData)
                        .then((res) => {
                            this.makeToast(this.$t('SaveSuccess'), 'success');
                            this.$router.push({
                                name: 'ServiceApplication',
                                query: { type: this.$route.query.type }
                            });
                        })
                        .catch(this.showApiError)
                        .finally(() => {
                            this.saveLoading = false;
                        });
                } else {
                    this.showValidateError(errors);
                }
            });
        }
    }
};
</script>

<style lang="scss">
.priceTable {
    thead {
        tr {
            background-color: #f0f0f0;
        }
    }

    tr th,
    tr td {
        padding: 7px;
        border-collapse: collapse;
        border: 1px solid #f5f5f5;
    }

    tr:nth-child(even) {
        background-color: #f7f7f7;
    }

    .w-50px {
        width: 50px;
    }
}
</style>
