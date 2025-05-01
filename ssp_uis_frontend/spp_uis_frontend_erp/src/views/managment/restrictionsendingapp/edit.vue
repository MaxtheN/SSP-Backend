<template>
    <b-overlay :show="show">
        <validation-observer ref="ValidationDTO">
            <b-card>
                <b-row >
                    <b-col cols="12" sm="3" >
                        <form-select v-model="Data.appId" required-star :options="ApplicationModelCodeList" :label="$t('appId')"
                            :placeholder="$t('appId')" />
                    </b-col>
                    <b-col cols="12" sm="3" >
                        <form-select v-model="Data.tableId" required-star :options="TableList" :label="$t('tableId')"
                            :placeholder="$t('tableId')" />
                    </b-col>
                    <b-col sm="12" md="3">

                        <form-picker v-model="Data.startAt" :label="$t('startAt')" required type="datetime"
                            format="DD.MM.YYYY HH:mm:ss" :placeholder="$t('startAt')" />
                    </b-col>
                    <b-col sm="12" md="3">
                        <form-picker v-model="Data.endAt" :label="$t('endAtt')" required type="datetime"
                            format="DD.MM.YYYY HH:mm:ss" :placeholder="$t('endAtt')" />
                    </b-col>
                    
                </b-row>
                <b-row>
                    <b-col sm="12" md="3">
                        <form-textarea v-model="Data.details" :label="$t('details')" required type="text"
                            :placeholder="$t('details')" />
                    </b-col>
                </b-row>
                <b-row>
                    <b-col class="text-right">
                        <b-button variant="success" @click="Save"> {{ $t('Save') }} </b-button>
                    </b-col>
                </b-row>
            </b-card>
        </validation-observer>
    </b-overlay>
</template>
<script>
import {
    BOverlay,
    BCard,
    BRow,
    BCol,
    BContainer,
    BButton,
    BInputGroup,
    BFormInput,
    BInputGroupAppend,
    BLink,
    BListGroup,
    BListGroupItem,
    BCollapse,
    VBToggle,
    BCardBody,
    BCardHeader,
    BBadge,
    VBTooltip
} from 'bootstrap-vue';
import QuestionareInput from '@/views/components/managment/QuestionareInput.vue';
import ManualService from '@/services/others/manual.service';
import RestrictionSendingAppService from '@/services/managment/RestrictionSendingApp.service';
import FeatherIcon from '@/@core/components/feather-icon/FeatherIcon.vue';
import FormInput from '@/components/forms/form-input.vue';
import FormTextarea from '@/components/forms/form-textarea.vue';
export default {
    components: {
        BOverlay,
        BCard,
        BRow,
        BCol,
        BContainer,
        BButton,
        BInputGroup,
        BFormInput,
        BInputGroupAppend,
        BLink,
        BListGroup,
        BListGroupItem,
        QuestionareInput,
        BCollapse,
        BCardBody,
        BCardHeader,
        FeatherIcon,
        BBadge,
        FormInput,
        FormTextarea
    },
    directives: {
        'b-toggle': VBToggle,
        'b-tooltip': VBTooltip
    },
    data() {
        return {
            Data: {},
            TableList: [],
            ApplicationModelCodeList: [],
            show: false,
            showTimePanel: false,
        };
    },
    created() {
        RestrictionSendingAppService.Get(this.$route.params.id)
            .then((res) => {
                this.Data = res.data;
            })
            .catch((err) => {
                this.showApiError(err);
            });

        ManualService.TableSelectList()
            .then((res) => {
                this.TableList = res.data;
            })
            .catch((error) => {
                this.showApiError(error);
            });

            ManualService.ApplicationModelCodeSelectList()
            .then((res) => {
                this.ApplicationModelCodeList = res.data;
            })
            .catch((error) => {
                this.showApiError(error);
            });
    },
    methods: {
        toggleTimePanel() {
            this.showTimePanel = !this.showTimePanel;
        },
        toggleTimeRangePanel() {
            this.showTimeRangePanel = !this.showTimeRangePanel;
        },
        handleOpenChange() {
            this.showTimePanel = false;
        },
        handleRangeClose() {
            this.showTimeRangePanel = false;
        },
        Save() {
            this.$refs.ValidationDTO.validate().then((success) => {
                if (success) {
                    this.saveLoading = true;
                    RestrictionSendingAppService.Update(this.Data)
                        .then(() => {
                            this.makeToast(this.$t('SaveSuccess'), 'success');
                            this.$router.push({ name: 'RestrictionSendingApp' });
                        })
                        .catch((err) => {
                            this.showApiError(err);
                        })
                        .finally(() => {
                            this.saveLoading = false;
                        });
                }
            });
        }
    }
};
</script>
<style lang="scss">
.type-hovered:hover {
    color: #003188;
}

.question_class::placeholder {
    color: red;
}
</style>
 