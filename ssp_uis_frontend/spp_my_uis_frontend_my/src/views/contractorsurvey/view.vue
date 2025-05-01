<template>
    <div class="container" style="position: relative">
        <div style="width: 100%; background-color: red; position: fixed; top: 0; left: 0; z-index: 100"></div>
        <AppListHeaderForName title="contractorsurvey" page-name="Questionnarie" />

        <b-overlay :show="Loading" style="min-height: calc(100vh - 460px)">
            <b-card no-body class="form-card p-4">
                <b-row>
                    <b-col md="4">
                        <WInput style="flex: 1 1" v-model="Data.DocNumber" rules="required" :label="$t('Number')" :name="$t('Number')" :placeholder="$t('Number')"> </WInput>
                    </b-col>
                    <b-col md="4">
                        <WDatePicker style="flex: 1 1" v-model="Data.startOn" rules="required" :label="$t('startOn')" :name="$t('startOn')" :placeholder="$t('startOn')">
                        </WDatePicker>
                    </b-col>
                    <b-col md="4">
                        <WDatePicker style="flex: 1 1" rules="required" v-model="Data.startEnd" :label="$t('startEnd')" :name="$t('startEnd')" :placeholder="$t('startEnd')">
                        </WDatePicker>
                    </b-col>
                    <b-col md="4">
                        <WDatePicker
                            style="flex: 1 1"
                            rules="required"
                            v-model="Data.realStartOn"
                            :label="$t('realStartOn')"
                            :name="$t('realStartOn')"
                            :placeholder="$t('realStartOn')"
                        >
                        </WDatePicker>
                    </b-col>
                    <b-col md="4">
                        <WDatePicker
                            style="flex: 1 1"
                            rules="required"
                            v-model="Data.realStartEnd"
                            :label="$t('realStartEnd')"
                            :name="$t('realStartEnd')"
                            :placeholder="$t('realStartEnd')"
                        >
                        </WDatePicker>
                    </b-col>
                    <b-col md="4">
                        <WSelect
                            :placeholder="$t('select')"
                            :options="InspectionTypeSelectList"
                            :label="$t('InspectionTypeSelectList')"
                            rules="required"
                            v-model="Data.inspectionTypeId"
                        />
                    </b-col>
                    <b-col md="4">
                        <WSelect
                            :placeholder="$t('select')"
                            :options="QuestionnaireTypeSelectList"
                            :label="$t('QuestionnaireTypeSelectList')"
                            rules="required"
                            v-model="Data.questionnaireId"
                        />
                    </b-col>
                </b-row>
                <b-row class="mt-3">
                    <ol style="list-style-type: decimal">
                        <li v-for="(group, groupIndex) in Data.group" :key="'group' + groupIndex">
                            <div class="d-flex full-width align-items-center">
                                <h3 class="col text-left">
                                    {{ group.title }}
                                </h3>
                            </div>
                            <ol>
                                <li v-for="(question, questionIndex) in group.questions" :key="questionIndex + groupIndex + 'question'" class="mb-2 px-1 my-4">
                                    <h5 class="question">
                                        {{ question.questionText }}
                                    </h5>

                                    <!-- answers -->
                                    <div class="pl-2 pt-2" v-for="answer in question.answers" :key="answer.id">
                                        <span v-if="question.answerTypeId == 1">
                                            <b-form-radio class="answerq1" name="some-radios" :value="answer.id">{{ answer.answerText }}</b-form-radio>
                                        </span>
                                        <span v-if="question.answerTypeId == 2">
                                            <b-form-checkbox v-model="answer.isChecked">{{ answer.answerText }}</b-form-checkbox>
                                        </span>
                                        <span v-if="question.answerTypeId == 3">
                                            <b-form-input v-model="answer.answerText" style="width: 50%"></b-form-input>
                                        </span>
                                    </div>
                                </li>
                            </ol>
                        </li>

                        <hr />
                    </ol>
                </b-row>
            </b-card>
            <div class="d-flex justify-content-end w-100" @click="SendAnswer">
                <b-button variant="success" class="mt-4 text-right">
                    <b-icon-check scale="0.8"></b-icon-check>
                    {{ $t('save') }}
                </b-button>
            </div>
        </b-overlay>
    </div>
</template>

<script>
import QueizManualService from '@/services/queizmanual.service';
import WDatePicker from '@/components/forms/WDatePicker.vue';
import QuestionnarieService from '@/services/questionnarie.service';
import AppListHeaderForName from '@/components/application/AppListHeaderForName.vue';
import ContractorSurveyService from '@/services/contractorsurvey.service';
import WInput from '@/components/forms/WInput.vue';
import WSelect from '@/components/forms/WSelect.vue';
import axios from 'axios';
export default {
    components: {
        AppListHeaderForName,
        WDatePicker,
        WInput,
        WSelect
    },
    data() {
        return {
            AnswerTypeSelectList: [],
            InspectionTypeSelectList: [],
            QuestionnaireTypeSelectList: [],
            answers: [],
            Data: {},
            axios,
            Loading: false,
            sendLoading: false
        };
    },
    created() {
        this.Refresh();

        QueizManualService.InspectionTypeSelectList()
            .then((res) => {
                this.InspectionTypeSelectList = res.data;
            })
            .catch((error) => {
                this.showApiError(error);
            });
        QueizManualService.QuestionnaireTypeSelectList()
            .then((res) => {
                this.QuestionnaireTypeSelectList = res.data;
            })
            .catch((error) => {
                this.showApiError(error);
            });
        QueizManualService.AnswerTypeSelectList()
            .then((res) => {
                this.AnswerTypeSelectList = res.data;
            })
            .catch((error) => {
                this.showApiError(error);
            });
    },
    methods: {
        handleAnswer(answer, type) {
            console.log(answer, type);
        },

        Refresh() {
            this.Loading = true;
            QuestionnarieService.Get(this.$route.params.id)
                .then((res) => {
                    this.Data = res.data;
                })
                .catch((error) => {
                    this.showApiError(error);
                })
                .finally(() => {
                    this.Loading = false;
                });
        },
        SendAnswer() {
            ContractorSurveyService.Create(this.Data)
                .then((res) => {
                    this.makeToast(this.$t('SaveSuccess'), 'success');
                    this.$router.push({ name: 'JoinAntiCorruptionApplication', query: { modaClose: true } });
                })
                .catch((error) => {
                    this.showApiError(error);
                })
                .finally(() => {
                    this.Loading = false;
                });
        }
    },
    computed: {
        length() {
            let width = 0;
            this.Data.group?.forEach((question) => {
                width = width + question.questions.length;
            });

            return width;
        }
    }
};
</script>

<style>
.question {
    font-weight: 700;
}
.answerq1 {
    cursor: pointer;
}

.custom-control.custom-checkbox label:before {
    border-radius: 0px !important;
}
</style>
