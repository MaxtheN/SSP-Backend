<!-- eslint-disable vue/no-v-html -->
<template>
    <div>
        <AppListHeaderForName title="PrtnCreditDemand" />
        <div class="container">
            <b-overlay :show="Loading" style="min-height: calc(100vh - 460px)">
                <b-card class="form-card">
                    <validation-observer ref="ValidationDTO">
                        <b-tabs pills card lazy>
                            <b-tab active :title="$t('AdditionalInfo')">
                                <b-row>
                                    <b-col sm="12" md="6" lg="6">
                                        <WInput :label="$t('contractorInn')" :placeholder="$t('contractorInn')" disabled v-model="Application.contractorInn" rules="required" />
                                    </b-col>
                                    <b-col sm="12" md="6" lg="6">
                                        <WInput :label="$t('contractor')" :placeholder="$t('contractor')" disabled v-model="Application.contractor" rules="required" />
                                    </b-col>
                                </b-row>
                                <b-row class="mt-2">
                                    <b-col sm="12" md="6" lg="6">
                                        <WInput :label="$t('number')" :placeholder="$t('number')" disabled v-model="Application.docNumber" rules="required" />
                                    </b-col>
                                    <b-col sm="12" md="6" lg="6">
                                        <WInput :label="$t('docOn')" :placeholder="$t('docOn')" disabled v-model="Application.docOn" rules="required" />
                                    </b-col>
                                    <b-col sm="12" md="6" lg="6">
                                        <WInput
                                            :label="$t('prtnCertificateDocNumber')"
                                            :placeholder="$t('prtnCertificateDocNumber')"
                                            disabled
                                            v-model="Application.prtnCertificateDocNumber"
                                            rules="required"
                                        />
                                    </b-col>
                                    <b-col sm="12" md="6" lg="6">
                                        <WInput
                                            :label="$t('prtnCertificateDocOn')"
                                            :placeholder="$t('prtnCertificateDocOn')"
                                            disabled
                                            v-model="Application.prtnCertificateDocOn"
                                            rules="required"
                                        />
                                    </b-col>
                                    <b-col sm="12" md="6" lg="6">
                                        <WInput
                                            :label="$t('prtnContractType')"
                                            :placeholder="$t('prtnContractType')"
                                            disabled
                                            v-model="Application.prtnContractType"
                                            rules="required"
                                        />
                                    </b-col>
                                    <b-col sm="12" md="6" lg="6">
                                        <WInput
                                            :label="$t('newVacanciesCount')"
                                            :placeholder="$t('newVacanciesCount')"
                                            disabled
                                            v-model="Application.newVacanciesCount"
                                            rules="required"
                                        />
                                    </b-col>
                                    <b-col sm="12" md="12" lg="12">
                                        <WInput :label="$t('address')" :placeholder="$t('address')" disabled v-model="Application.address" rules="required" />
                                    </b-col>
                                </b-row>
                                <hr />
                                <b-row class="mt-2">
                                    <b-col sm="12" md="12" lg="12">
                                        <WInput
                                            :label="$t('implementedProjectName')"
                                            :placeholder="$t('implementedProjectName')"
                                            v-model="Application.implementedProjectName"
                                            rules="required"
                                            name="implementedProjectName"
                                        />
                                    </b-col>
                                    <b-col sm="12" md="6" lg="6">
                                        <WCurrencyInput
                                            :label="$t('projectCost')"
                                            :placeholder="$t('projectCost')"
                                            disabled
                                            v-model="Application.projectCost"
                                            rules="required"
                                            name="projectCost"
                                        />
                                    </b-col>
                                    <b-col sm="12" md="6" lg="6">
                                        <WCurrencyInput
                                            @input="calculate"
                                            :label="$t('ownInvestment')"
                                            :placeholder="$t('ownInvestment')"
                                            v-model="Application.ownInvestment"
                                            rules="required"
                                            name="ownInvestment"
                                        />
                                    </b-col>
                                    <b-col sm="12" md="6" lg="6">
                                        <WCurrencyInput
                                            @input="calculate"
                                            :label="$t('foreignInvestment')"
                                            :placeholder="$t('foreignInvestment')"
                                            v-model="Application.foreignInvestment"
                                            rules="required"
                                            name="foreignInvestment"
                                        />
                                    </b-col>
                                    <b-col sm="12" md="6" lg="6">
                                        <WCurrencyInput
                                            @input="calculate"
                                            :label="$t('privillageBankCredit')"
                                            :placeholder="$t('privillageBankCredit')"
                                            v-model="Application.privillageBankCredit"
                                            rules="required"
                                            name="privillageBankCredit"
                                        />
                                    </b-col>
                                    <b-col sm="12" md="6" lg="6">
                                        <WSelect rules="required" :label="$t('bankInfo')" :clearable="true" name="Bank" :options="BankList" v-model="Application.bankId" />
                                    </b-col>
                                    <b-col sm="12" style="text-align: end !important">
                                        <b-button @click="SaveData" :disabled="saveLoading" variant="success"> <b-icon-check /> {{ $t('Save') }} </b-button>
                                    </b-col>
                                </b-row>
                            </b-tab>
                        </b-tabs>
                    </validation-observer>
                </b-card>
            </b-overlay>
            <b-modal v-model="modalUrl" :title="$t('')" no-close-on-backdrop hide-footer hide-header>
                <b-row style="padding: 10px">
                    <b-col>
                        <p v-if="lang == 'uz_cyrl'">
                            Ҳурматли тадбиркор! Сизнинг, “20 минг тадбиркор – 500 минг малакали мутахассис” дастури доирасида имтиёзли кредитга бўлган эҳтиёжларингиз тўғрисидаги
                            маълумотларингиз қабул қилинди. Имтиёзли кредит олиш учун
                            <br />
                            <span style="font-weight: 700; color: blue">https://bank-kredit.uz/public/forms/ht_credit_application</span> ҳаволи орқали ариза қолдиринг.
                        </p>
                        <p v-if="lang == 'uz_ru'">
                            Уважаемый предприниматель! Ваша информация о Ваших потребностях в льготном кредите в рамках программы «20 тысяч предпринимателей – 500 тысяч
                            квалифицированных специалистов» получена. Подайте заявку на льготный кредит по ссылке
                            <br />
                            <span style="font-weight: 700; color: blue">https://bank-kredit.uz/public/forms/ht_credit_application</span>.
                        </p>
                        <p v-if="lang == 'uz_latn'">
                            Hurmatli tadbirkor! Sizning, “20 ming tadbirkor – 500 ming malakali mutaxassis” dasturi doirasida imtiyozli kreditga boʼlgan ehtiyojlaringiz
                            toʼgʼrisidagi maʼlumotlaringiz qabul qilindi. Imtiyozli kredit olish uchun <br />
                            <span style="font-weight: 700; color: blue">https://bank-kredit.uz/public/forms/ht_credit_application</span> havoli orqali ariza qoldiring.
                        </p>
                    </b-col>
                </b-row>
                <b-row class="mt-3">
                    <b-col style="text-align: end" class="text-right">
                        <a @click="GoUrl" href="https://bank-kredit.uz/public/forms/ht_credit_application" target="_blank" class="btn btn-sm btn-success pr-btn">
                            {{ $t('goUrl') }}
                        </a>
                    </b-col>
                </b-row>
            </b-modal>
        </div>
    </div>
</template>

<script>
import PrtnCreditDemandService from '@/services/prtncreditdemand.service';
import ManualService from '@/services/manual.service';
import AppListHeaderForName from '@/components/application/AppListHeaderForName.vue';
import WInput from '@/components/forms/WInput.vue';
import WCurrencyInput from '@/components/forms/WCurrencyInput.vue';
import WSelect from '@/components/forms/WSelect.vue';
import AppWord from '@/components/word/AppWord.vue';

const tabrowDef = {
    id: 0,
    yearIn: 0,
    monthIn: 0,

    newVacanciesCount: 0
};
export default {
    components: {
        AppListHeaderForName,
        WSelect,
        WInput,
        WCurrencyInput,
        AppWord
    },
    data() {
        return {
            Application: {},
            modalUrl: false,
            BankList: [],
            MonthInList: [],
            MonthInList1: [],
            RegionList: [],
            MfyList1: [],
            YearList: [],
            DistrictList: [],
            ChooseDistrictList: [],
            MfyListLocationFalse: [],
            ApplicationHTMLData: {},
            List: [],
            fields: [
                {
                    key: 'yearIn',
                    label: this.$t('yearIn'),
                    sortable: false,
                    tdClass: 'text-center',
                    thClass: 'text-center'
                },
                {
                    key: 'monthIn',
                    label: this.$t('monthIn'),
                    sortable: false,
                    tdClass: 'text-center',
                    thClass: 'text-center'
                },

                {
                    key: 'newVacanciesCount',
                    label: this.$t('newVacanciesCount'),
                    sortable: false,
                    tdClass: 'text-center',
                    thClass: 'text-center'
                },
                {
                    key: 'actions',
                    tdClass: 'text-center',
                    thClass: 'text-center',
                    label: this.$t('actions')
                }
            ],
            lang: localStorage.getItem('locale') || 'uz_latn',
            PrtnList: [],
            tabrow: { ...tabrowDef },
            Loading: false,
            orgName: '',
            saveLoading: false
        };
    },
    computed: {
        getMonthName() {
            return (month) => this.MonthInList1.filter((el) => el.value == month)[0]?.text;
        }
    },
    created() {
        this.Refresh();
        ManualService.BankSelectList().then((res) => {
            this.BankList = res.data;
        });
    },
    methods: {
        calculate() {
            if (this.Application.prtnContractTypeId == 1) {
                if (this.Application.privillageBankCredit > 5000000000) {
                    this.makeToast(this.$t('prtnContractTypeFiveSum'), 'error');
                    this.Application.privillageBankCredit = null;
                }
            } else if (this.Application.prtnContractTypeId == 2) {
                if (this.Application.privillageBankCredit > 10000000000) {
                    this.makeToast(this.$t('prtnContractTypeTenSum'), 'error');
                    this.Application.privillageBankCredit = null;
                }
            } else if (this.Application.prtnContractTypeId == 3) {
                if (this.Application.privillageBankCredit > 15000000000) {
                    this.makeToast(this.$t('prtnContractTypeFifteenSum'), 'error');
                    this.Application.privillageBankCredit = null;
                }
            }
            this.Application.projectCost = this.Application.ownInvestment + this.Application.foreignInvestment + this.Application.privillageBankCredit;
        },

        Refresh() {
            PrtnCreditDemandService.Get(this.$route.params.id)
                .then((res) => {
                    this.Application = res.data;
                })
                .catch(this.showApiError);
        },
        GoUrl() {
            this.modalUrl = false;
            this.$router.push({ name: 'PrtnCreditDemand' });
        },
        SaveData() {
            this.$refs.ValidationDTO.validateWithInfo().then(({ isValid, errors }) => {
                if (isValid) {
                    this.saveLoading = true;
                    PrtnCreditDemandService.Update(this.Application)
                        .then(() => {
                            this.modalUrl = true;
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
