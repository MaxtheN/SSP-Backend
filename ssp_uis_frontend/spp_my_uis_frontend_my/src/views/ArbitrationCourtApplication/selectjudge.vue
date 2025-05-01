<template>
    <div>
        <AppListHeaderForName title="Hakamlik sudyasini tanlash" page-name="SelectJudge" />

        <div class="container">
            <b-overlay :show="Loading">
                <b-row class="justify-content-center">
                    <b-col sm="12" lg="12">
                        <b-card class="form-card">
                            <validation-observer ref="ValidationDTO">
                                <b-card-text>
                                    <b-row>
                                        <b-col sm="12" md="6" lg="6">
                                            <WSelect
                                                multiple
                                                v-model="arbitrationJudgeIds"
                                                rules="required"
                                                :name="$t('judge')"
                                                :label="$t('judge')"
                                                :options="JudgeList"
                                                valueid="value"
                                                :clearable="true"
                                                valuename="text"
                                            />
                                        </b-col>

                                        <b-col lg="9" md="9"></b-col>
                                        <b-col sm="12" lg="3" md="3">
                                            <b-button @click="SaveData" variant="success" block class="mb-1">
                                                <b-spinner v-if="saveLoading" small></b-spinner>
                                                <b-icon-check scale="0.8"></b-icon-check>
                                                {{ $t('save') }}
                                            </b-button>
                                        </b-col>
                                    </b-row>
                                </b-card-text>
                            </validation-observer>
                        </b-card>
                    </b-col>
                </b-row>
            </b-overlay>
        </div>
    </div>
</template>

<script>
import WSelect from '@/components/forms/WSelect.vue';
import AppListHeaderForName from '@/components/application/AppListHeaderForName.vue';
import ArbitrationJudgeService from '@/services/hakamliksudi/arbitrationjudge.service';
import ArbitrationCourtApplicationService from '@/services/hakamliksudi/arbitrationcourtapplication.service';

export default {
    components: {
        AppListHeaderForName,
        WSelect
    },

    data() {
        return {
            Data: {},
            JudgeList: [],
            arbitrationJudgeIds: [],
            Loading: false,

            saveLoading: false
        };
    },

    created() {
        ArbitrationCourtApplicationService.Get(this.$route.params.id)
            .then((res) => {
                this.Data = res.data;
                if (res.data.signer.length != 0) {
                    this.arbitrationJudgeIds = res.data.signer.map((item) => item.arbitrationJudgeId);
                }
            })
            .catch(this.showApiError)
            .finally(() => {
                this.Loading = false;
            });
        ArbitrationJudgeService.GetAsSelectList().then((res) => {
            this.JudgeList = res.data;
        });
    },

    methods: {
        SaveData() {
            this.$refs.ValidationDTO.validateWithInfo().then(({ isValid, errors }) => {
                if (isValid) {
                    this.saveLoading = true;
                    this.Data.signer = [];
                    this.arbitrationJudgeIds.forEach((item, index) => this.Data.signer.push({ arbitrationJudgeId: item }));
                    ArbitrationCourtApplicationService.Update(this.Data)
                        .then((res) => {
                            this.makeToast(this.$t('SaveSuccess'), 'success');
                            this.$router.push({ name: 'ArbitrationCourtApplication' });
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
<style lang="scss" scoped></style>
