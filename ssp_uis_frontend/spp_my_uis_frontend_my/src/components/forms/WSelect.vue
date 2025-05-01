<template>
    <ValidationProvider :vid="vid" :name="$attrs.name" :rules="rules" v-slot="{ valid, errors, required }">
        <b-form-group v-bind="$attrs" class="w-select" :state="errors[0] ? false : valid ? true : null">
            <template v-slot:label>
                {{ $attrs.label }}
                <span class="text-danger" v-if="required">*</span>
            </template>
            <v-select
                v-bind="$attrs"
                v-on="$listeners"
                v-model="innerValue"
                :reduce="(item) => item[valueid]"
                :label="valuename"
                :placeholder="$t('c\hoose')"
                @option:selected="(e) => $emit('change', e)"
            >
                <slot></slot>
                <template #open-indicator="{ attributes }">
                    <span v-bind="attributes">
                        <img src="/images/design/arrow-down.svg" alt />
                    </span>
                </template>
                <span slot="no-options">{{ $t('nothinghere') }}</span>
                <template v-slot:option="option">
                    <slot name="option" v-bind="option"></slot>
                </template>
                <template v-slot:selected-option="option">
                    <slot name="selected-option" v-bind="option"></slot>
                </template>
            </v-select>
            <b-form-invalid-feedback style="display: block">{{ errors[0] }}</b-form-invalid-feedback>
        </b-form-group>
    </ValidationProvider>
</template>

<script>
import { ValidationProvider } from 'vee-validate';
import VSelect from 'vue-select';

export default {
    inheritAttrs: false,
    name: 'Edit',
    components: {
        ValidationProvider,
        VSelect
    },
    props: {
        vid: {
            type: String,
            default: ''
        },
        rules: {
            type: String,
            default: ''
        },
        value: {
            type: null,
            default: ''
        },
        valuename: {
            type: String,
            default: 'text'
        },
        valueid: {
            type: String,
            default: 'value'
        }
    },
    computed: {
        innerValue: {
            get() {
                return this.value === 0 ? null : this.value;
            },
            set(val) {
                this.$emit('input', val);
            }
        }
    }
};
</script>

<style lang="scss" scoped>
@import './style.scss';

.w-select {
    :deep(.v-select) {
        .vs__dropdown-toggle {
            border: 1px solid rgba(38, 41, 45, 0.1);
            border-radius: 6px;
            background-color: #fff;
            padding: 10px;

            &:focus-within {
                border-color: transparent;
                box-shadow: 0 0 0 0.2rem rgba($primary, 0.25);
            }
            .vs__selected-options {
                input {
                    padding-left: 6px;
                    padding-bottom: 6px;
                    padding-top: 6px;
                    margin-bottom: 0px;
                    background-color: #fff;
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

    &.is-invalid {
        :deep(.vs__dropdown-toggle) {
            border: $border-width solid $form-feedback-invalid-color;
            &:focus-within {
                border: $border-width solid $form-feedback-invalid-color;
                box-shadow: 0 0 0 0.2rem rgba($red, 0.25);
            }
        }
    }
    &:disabled {
        :deep(.vs__dropdown-toggle) {
            background-color: $input-disabled-bg !important;
        }

        :deep(.vs__open-indicator),
        :deep(.vs__search) {
            background-color: $input-disabled-bg !important;
        }
    }
}
</style>
