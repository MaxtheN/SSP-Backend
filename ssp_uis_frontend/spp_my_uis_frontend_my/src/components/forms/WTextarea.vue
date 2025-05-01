<template>
    <ValidationProvider :vid="vid" :name="$attrs.name" :rules="rules" v-slot="{ valid, errors }">
        <b-form-group v-bind="$attrs">
            <template v-slot:label>
                {{ $attrs.label }}
                <span class="text-danger" v-if="rules">*</span>
            </template>
            <b-form-textarea v-model="innerValue" v-bind="$attrs" v-mask="mask" :state="errors[0] ? false : valid ? true : null"></b-form-textarea>
            <b-form-invalid-feedback id="inputLiveFeedback">{{ errors[0] }}</b-form-invalid-feedback>
        </b-form-group>
    </ValidationProvider>
</template>

<script>
import { ValidationProvider } from 'vee-validate';

export default {
    components: {
        ValidationProvider
    },
    props: {
        vid: {
            type: String,
            default: ''
        },
        rules: {
            type: [Object, String],
            default: ''
        },
        value: {
            type: null,
            default: ''
        },
        mask: {
            type: String,
            default: ''
        }
    },
    data: () => ({
        innerValue: ''
    }),
    watch: {
        // Handles internal model changes.
        innerValue(newVal) {
            this.$emit('input', newVal);
        },
        // Handles external model changes.
        value(newVal) {
            this.innerValue = newVal;
        }
    },
    created() {
        if (this.value) {
            this.innerValue = this.value;
        }
    }
};
</script>

<style lang="scss" scoped>
@import './style.scss';
</style>
