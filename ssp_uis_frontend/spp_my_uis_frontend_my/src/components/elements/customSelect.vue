<template>
    <div class="my-custom-select">
        <label> {{ label }} <span v-if="required" style="color: red; font-size: 18px">*</span> </label>
        <v-select
            :options="options"
            :reduce="(item) => item[valueid]"
            :label="valuename"
            :placeholder="$t('c\hoose')"
            :clearable="clearable"
            :disabled="disabled"
            v-model="updateVal"
        >
            <template #open-indicator="{ attributes }">
                <span v-bind="attributes"> <img src="/images/design/arrow-down.svg" alt="" /> </span>
            </template>
            <span slot="no-options"> {{ $t('nothinghere') }} </span>
        </v-select>
    </div>
</template>

<script>
import vSelect from 'vue-select';
export default {
    components: { vSelect },
    props: {
        options: {
            type: Array,
            default: () => []
        },
        label: {
            type: String,
            default: ''
        },
        required: {
            type: Boolean,
            default: false
        },
        disabled: {
            type: Boolean,
            default: false
        },
        valuename: {
            type: String,
            default: 'name'
        },
        valueid: {
            type: String,
            default: 'id'
        },
        clearable: {
            type: Boolean,
            default: false
        },
        // eslint-disable-next-line vue/require-default-prop, vue/require-prop-types
        value: {}
    },
    computed: {
        updateVal: {
            get: function () {
                return this.value == 0 ? null : this.value;
            },
            set: function (val) {
                this.$emit('input', val);
            }
        }
    }
};
</script>

<style lang="scss" scope>
@import '../../assets/styles/variables.scss';
.my-custom-select {
    label {
        width: 100%;
        padding-left: 16px;
        border-radius: 12px;
        text-transform: uppercase;
        // background-color: #F7F7F7;
        font-size: 12px;
    }
    .v-select.vs--single.vs--searchable {
        .vs__dropdown-toggle {
            border: none;
            background-color: #f7f7f7;
            border-radius: 12px;
            padding: 10px;
            .vs__selected-options {
                input {
                    padding-left: 12px;
                    padding-bottom: 5px;
                    padding-top: 5px;
                    margin-bottom: 0px;
                    background-color: #f7f7f7;
                    margin-top: 0;
                    &::placeholder {
                        font-size: 15px;
                        color: $input-text-placeholder;
                    }
                }
                .vs__selected {
                    padding-left: 0px;
                    margin-top: 0;
                    margin-bottom: 0px;
                }
            }
            .vs__actions {
                .vs__open-indicator {
                    margin-top: 0px;
                    margin-right: 15px;
                }
                .vs__clear {
                    margin-top: 0px;
                    margin-right: 15px;
                }
            }
        }
        .vs__dropdown-menu {
            margin-top: 16px;
            border-radius: 12px;
        }
    }
}
</style>
