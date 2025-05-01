<template>
    <div>
        <AppListHeaderForName title="appealSend" page-name="AppealApplication" :query="{ type: $route.query.type }" />
        <div class="container">
            <b-overlay :show="Loading">
                <b-row class="justify-content-center">
                    <b-col sm="12" lg="9">
                        <b-card class="form-card">
                            <validation-observer ref="ValidationDTO">
                                <b-card-text>
                                    <b-row>
                                        <template v-if="Data.contractor">
                                            <b-col sm="12" md="6" lg="6">
                                                <WInput rules="required" :label="$t('inn')" name="inn" disabled v-model="Data.contractor.innOrPinfl" />
                                            </b-col>
                                            <b-col sm="12" md="6" lg="6">
                                                <WInput
                                                    rules="required"
                                                    :label="$t('contractor')"
                                                    name="contractor"
                                                    disabled
                                                    :value="
                                                        Data.contractor.innOrPinfl && Data.contractor.innOrPinfl.length == 14
                                                            ? Data.contractor.fullName + ' - ' + Data.contractor.director
                                                            : Data.contractor.fullName
                                                    "
                                                />
                                            </b-col>
                                        </template>
                                        <b-col sm="12" md="6" lg="6">
                                            <WSelect :placeholder="$t('select')" :options="RegionList" :label="$t('region')" v-model="Data.regionId" @input="ChangeRegion" />
                                        </b-col>
                                        <b-col sm="12" md="6" lg="6">
                                            <WSelect :placeholder="$t('select')" :options="DistrictList" :label="$t('liveregionname')" v-model="Data.districtId" />
                                        </b-col>
                                        <b-col sm="12" md="6" lg="6">
                                            <WInput rules="required" :label="$t('address')" :placeholder="$t('address')" name="address" v-model="Data.address" />
                                        </b-col>
                                        <b-col sm="12" md="6" lg="6">
                                            <WInput rules="email" :label="$t('email')" :placeholder="$t('email')" name="email" v-model="Data.email" />
                                        </b-col>
                                    </b-row>
                                    <hr />
                                    <b-row>
                                        <b-col sm="12" md="4">
                                            <WInput
                                                style="flex: 1 1"
                                                v-model="filterPerson.Seria"
                                                rules="required"
                                                v-mask="'AA'"
                                                v-uppercase
                                                :label="$t('Seria')"
                                                :name="$t('Seria')"
                                                :placeholder="$t('AA')"
                                            >
                                            </WInput>
                                        </b-col>
                                        <b-col sm="12" md="4">
                                            <WInput
                                                style="flex: 1 1"
                                                v-model="filterPerson.Number"
                                                rules="required"
                                                :label="$t('Number')"
                                                v-mask="'#######'"
                                                :name="$t('Number')"
                                                :placeholder="$t('1234567')"
                                            >
                                            </WInput>
                                        </b-col>
                                        <b-col sm="12" md="4">
                                            <b-input-group class="w-input-group">
                                                <WDatePicker
                                                    style="flex: 1 1"
                                                    v-model="filterPerson.DateOfBirth"
                                                    rules="required"
                                                    :label="$t('dateofbirth')"
                                                    :name="$t('dateofbirth')"
                                                    :placeholder="$t('dateofbirth')"
                                                >
                                                </WDatePicker>
                                                <b-input-group-append>
                                                    <b-button @click="GetPerson" :disabled="soliqLoading" variant="primary" class="px-3">
                                                        <b-spinner v-if="soliqLoading" small style="margin-right: 8px"></b-spinner>
                                                        <b-icon scale="0.9" icon="search"></b-icon
                                                    ></b-button>
                                                </b-input-group-append>
                                            </b-input-group>
                                        </b-col>
                                        <template v-if="Data.person">
                                            <b-col sm="12" md="6" lg="6">
                                                <WInput rules="required" :label="$t('director')" name="contractorDirector" v-model="Data.person.fullName" />
                                            </b-col>
                                            <b-col sm="12" md="6" lg="6">
                                                <WInput rules="required" :label="$t('pinfl')" name="pinfl" v-model="Data.person.pinfl" />
                                            </b-col>
                                            <b-col sm="12" md="6" lg="6">
                                                <WInput
                                                    rules="required"
                                                    :label="$t('passportSeriaAndPassportNumber')"
                                                    name="passportSeriaAndPassportNumber"
                                                    disabled
                                                    :value="Data.person.passportSeria + ' ' + Data.person.passportNumber"
                                                />
                                            </b-col>
                                            <b-col sm="12" md="6" lg="6">
                                                <WInput rules="required" :label="$t('birthDate')" name="birthDate" v-model="Data.person.birthDate" />
                                            </b-col>
                                            <b-col sm="12" md="6" lg="6">
                                                <WSelect :options="bandlik" v-model="Data.busyness" :label="$t('employmentType')"></WSelect>
                                            </b-col>
                                            <b-col sm="12" md="6" lg="6">
                                                <WPhoneInput :label="$t('phone')" name="phone" rules="required|validatorPhone" placeholder v-model="Data.phoneNumber" />
                                            </b-col>
                                        </template>
                                    </b-row>
                                    <hr class="mt-2" />
                                    <b-row>
                                        <b-col sm="12" md="6" lg="6">
                                            <WInput rules="required" :label="$t('appealDocNumber')" name="appealDocNumber" v-model="Data.docNumber" />
                                        </b-col>
                                        <b-col sm="12" md="6" lg="6">
                                            <WInput rules="required" :label="$t('appealDocOn')" name="appealDocOn" v-model="Data.docOn" />
                                        </b-col>
                                        <b-col sm="12" md="6" lg="6">
                                            <WSelect :options="openAppeal" v-model="Data.openAppeal" :label="$t('openAppeal')" />
                                        </b-col>
                                        <b-col sm="12" md="6" lg="6">
                                            <WSelect
                                                :placeholder="$t('select')"
                                                rules="required"
                                                :options="AppealTypeSelectList"
                                                :label="$t('appealType')"
                                                v-model="Data.appealTypeId"
                                                name="appealType"
                                            />
                                        </b-col>
                                        <b-col sm="12" md="6" lg="6">
                                            <WSelect
                                                :placeholder="$t('select')"
                                                rules="required"
                                                :options="AppealFormatTypeSelectList"
                                                :label="$t('appealFormatType')"
                                                v-model="Data.appealFormatTypeId"
                                                disabled
                                                name="appealFormatType"
                                            />
                                        </b-col>
                                        <b-col sm="12" md="6" lg="6">
                                            <WSelect
                                                :placeholder="$t('select')"
                                                :options="AppealTypeArriveSelectList"
                                                :label="$t('AppealTypeArrive')"
                                                v-model="Data.appealTypeArriveId"
                                                name="AppealTypeArrive"
                                            />
                                        </b-col>
                                        <b-col sm="12" cols="12" class="mt-3">
                                            <WTextarea rows="4" v-model="Data.details" rules="required" name="appealText" :label="$t('Taklif matni')" />
                                        </b-col>
                                        <b-col sm="12" md="12" lg="12">
                                            <h6 class="inputTitle">
                                                <span>*</span>
                                                {{ $t('Taklifga ilovalar (Fayl yuklash)') }}
                                            </h6>
                                            <b-form-file
                                                type="file"
                                                :placeholder="$t('selectFile')"
                                                class="mt-2"
                                                @change="UploadFile"
                                                :browse-text="$t('select')"
                                                :disabled="fileLoading"
                                            />
                                            <div class="mt-3" v-for="item in Data.files" :key="item.id">
                                                <b-link variant="primary" target="_blank" :href="FileSrc(item.id)">{{ item.fileName || item.id }}</b-link>
                                                <b-button v-if="canUpdate" variant="danger" size="sm" class="ml-1" @click="DeleteFile(item.id)">
                                                    <b-icon-trash scale="0.7" />
                                                </b-button>
                                            </div>
                                        </b-col>
                                    </b-row>
                                </b-card-text>
                            </validation-observer>
                        </b-card>
                    </b-col>
                    <b-col sm="12" lg="3" v-if="Data.canEdit || $route.params.id == 0 || Data.canDelete || Data.canSign || Data.canAccept">
                        <b-card class="form-card">
                            <b-card-text>
                                <b-button @click="SaveData" v-if="Data.canEdit || $route.params.id == 0" variant="success" block class="mb-1">
                                    <b-spinner v-if="saveLoading" small></b-spinner>
                                    <b-icon-check scale="0.8"></b-icon-check>
                                    {{ $t('save') }}
                                </b-button>
                                <b-button v-if="Data.canDelete" @click="$bvModal.show('DeleteModal' + Data.id)" variant="danger" block class="mb-1">
                                    <b-icon-trash scale="0.6"></b-icon-trash>
                                    {{ $t('delete') }}
                                </b-button>
                                <!-- Sign -->
                                <b-button v-if="Data.canSign" @click="$bvModal.show('ESPmodal')" variant="primary" block class="mb-1">
                                    <b-icon-arrow-bar-up scale="0.6"></b-icon-arrow-bar-up>
                                    {{ $t('send') }}
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
            <b-modal id="ESPmodal" :title="$t('ESPmodal')" no-close-on-backdrop hide-footer>
                <just-sign @sign="GetESP($event)"></just-sign>
                <b-row>
                    <b-col style="text-align: end" class="text-right">
                        <a @click="$bvModal.hide('ESPmodal')" class="btn btn-sm btn-soft-danger mr-2 pr-btn" style="margin-right: 5px">{{ $t('no') }}</a>
                        <a @click="Send(Data)" class="btn btn-sm btn-success pr-btn">
                            <b-spinner v-if="SignLoading" small></b-spinner>
                            {{ $t('yes') }}
                        </a>
                    </b-col>
                </b-row>
            </b-modal>
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
import AppealApplicationService from '@/services/appeal/AppealApplication.service';
import AppListHeaderForName from '@/components/application/AppListHeaderForName.vue';
import axios from 'axios';
import AppContractorSettlementAccount from '@/components/application/AppContractorSettlementAccount.vue';
import ManualService from '@/services/manual.service';
import WDatePicker from '@/components/forms/WDatePicker.vue';
import PersonService from '../../../services/person.service';
import JustSign from '@/components/justSign.vue';

export default {
    components: {
        WCurrencyInput,
        WSelect,
        WTextarea,
        WInput,
        WPhoneInput,
        AppListHeaderForName,
        AppContractorSettlementAccount,
        WDatePicker,
        JustSign
    },
    directives: {
        uppercase: {
            update: function (el) {
                const inputEl = el.querySelector('input');
                if (inputEl) {
                    inputEl.value = inputEl.value.toUpperCase();
                }
            }
        }
    },
    data() {
        return {
            axios,
            openAppeal: [
                { text: this.$t('yes'), value: true },
                { text: this.$t('no'), value: false }
            ],
            Data: {},
            AppealFormatTypeSelectList: [],
            AppealTypeSelectList: [],
            AppealTypeArriveSelectList: [],
            RegionList: [],
            DistrictList: [],
            Loading: false,
            saveLoading: false,
            fileLoading: false,
            DeleteLoading: false,
            SignLoading: false,
            soliqLoading: false,
            filterPerson: {
                Seria: '',
                Number: '',
                DateOfBirth: ''
            },
            bandlik: [
                { text: this.$t('Ishlaydi'), value: true },
                { text: this.$t('Ishsiz'), value: false }
            ]
        };
    },
    created() {
        ManualService.RegionSelectList(this.langId)
            .then((res) => {
                this.RegionList = res.data;
            })
            .catch((error) => {
                this.showApiError(error);
            });

        ManualService.AppealFormatTypeSelectList()
            .then((res) => {
                this.AppealFormatTypeSelectList = res.data;
            })
            .catch((error) => {
                this.showApiError(error);
            });
        ManualService.AppealTypeSelectList()
            .then((res) => {
                this.AppealTypeSelectList = res.data;
            })
            .catch((error) => {
                this.showApiError(error);
            });
        ManualService.AppealTypeArriveSelectList()
            .then((res) => {
                this.AppealTypeArriveSelectList = res.data;
            })
            .catch((error) => {
                this.showApiError(error);
            });

        this.Refresh();
    },
    computed: {
        FileSrc() {
            return (id) => axios.defaults.baseURL + `appeal/AppealApplication/DownloadFile/${id}`;
        },
        canUpdate() {
            return this.Data.canSend || this.Data.id == 0 || this.Data.canEdit;
        }
    },
    methods: {
        UploadFile(event) {
            const formData = new FormData();
            formData.append('files', event.target.files[0]);
            this.fileLoading = true;
            AppealApplicationService.UploadFile(formData)
                .then((res) => {
                    this.Data.files.push(...res.data);
                })
                .catch(this.showApiError)
                .finally(() => {
                    this.fileLoading = false;
                });
        },
        DeleteFile(id) {
            AppealApplicationService.DeleteFile(id).then(() => {
                this.Data.files = this.Data.files.filter((item) => item.id != id);
            });
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
        GetPerson() {
            this.soliqLoading = true;
            PersonService.GetByPassportDataFromDigital(this.filterPerson.Seria, this.filterPerson.Number, this.filterPerson.DateOfBirth)
                .then((res) => {
                    this.Data.person = res.data;
                    if (!this.Data.person?.fullName) {
                        this.Data.person.fullName = res.data.surnameLatin + ' ' + res.data.nameLatin + ' ' + res.data.patronymLatin;
                    }
                })
                .catch(this.showApiError)
                .finally(() => {
                    this.soliqLoading = false;
                });
        },
        Refresh() {
            this.Loading = true;
            AppealApplicationService.Get(this.$route.params.id)
                .then((res) => {
                    this.Data = res.data;
                    if (this.Data.regionId) {
                        this.getDistrictList();
                    }
                    if (this.Data.person && !this.Data.person.id) {
                        this.Data.person = null;
                    }
                    if (this.$route.params.id == 0) {
                        this.Data.address = this.Data.contractor?.address;
                        this.Data.busyness = true;
                        this.Data.appealFormatTypeId = 3;
                    }
                })
                .catch(this.showApiError)
                .finally(() => {
                    this.Loading = false;
                });
        },
        Delete(item) {
            this.DeleteLoading = true;
            AppealApplicationService.Delete(item.id)
                .then(() => {
                    this.DeleteLoading = false;
                    this.makeToast(this.$t('DeleteSuccess'), 'success');
                    this.$bvModal.hide('DeleteModal' + item.id);

                    this.$router.push({ name: 'AppealApplication' });
                })
                .catch((error) => {
                    this.showApiError(error);
                    this.DeleteLoading = false;
                });
        },
        GetESP(data) {
            this.$bvModal.hide('ESPmodal');

            this.Sign(data);
        },
        Sign(data) {
            this.SignLoading = true;
            AppealApplicationService.Sign({ signedData: data.key, id: this.Data.id })
                .then((res) => {
                    this.$bvModal.hide('ESPmodal');
                    this.$router.push({ name: 'AppealApplication' });
                })
                .catch((error) => {
                    this.showApiError(error);
                })
                .finally(() => {
                    this.SignLoading = false;
                });
        },
        SaveData() {
            this.$refs.ValidationDTO.validateWithInfo().then(({ isValid, errors }) => {
                if (isValid) {
                    this.saveLoading = true;
                    AppealApplicationService.Update(this.Data)
                        .then((res) => {
                            this.makeToast(this.$t('SaveSuccess'), 'success');
                            this.$router.push({
                                name: 'AppealApplication'
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
