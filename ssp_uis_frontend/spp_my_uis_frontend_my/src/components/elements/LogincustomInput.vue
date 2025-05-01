<template>
    <div>
        <label class="input-color-label" v-if="label !== ''"> {{ label }} </label>
        <div class="my-input">
            <input
                :type="type"
                @focus="$emit('focus', value)"
                @keyup="$emit('keyup', value)"
                :disabled="disabled"
                :value="value"
                v-mask="mask"
                @input="updateValue($event.target.value)"
                :placeholder="placeholder"
            />
            <div class="right-addon-icon">
                <slot name="right-icon"></slot>
            </div>
        </div>
    </div>
</template>

<script>
export default {
    props: {
        label: {
            type: String,
            default: 'Label'
        },
        required: {
            type: Boolean,
            default: false
        },
        placeholder: {
            type: String,
            default: ''
        },
        disabled: {
            type: Boolean,
            default: false
        },
        uppertext: {
            type: Boolean,
            default: false
        },
        mask: {
            type: String,
            default: null
        },
        type: {
            type: String,
            default: 'text'
        },
        // eslint-disable-next-line vue/require-default-prop, vue/require-prop-types
        value: {}
    },
    data() {
        return {
            // inputVal : ''
        };
    },
    methods: {
        updateValue: function (value) {
            this.$emit('input', this.uppertext ? value.toUpperCase() : value);
        }
    }
};
</script>

<style lang="scss" scoped>
@import '../../assets/styles/variables.scss';
.my-input {
    background-color: white;
    border: 1px solid #d5d7e1;
    padding: 16px 20px;
    border-radius: 8px;
    position: relative;
    .right-addon-icon {
        position: absolute;
        top: 15px;
        right: 15px;
        vertical-align: middle;
        cursor: pointer;
    }
    input {
        width: 100%;
        outline: none;
        border: none;
        &::placeholder {
            color: $input-text-placeholder;
        }
    }
}
.my-input.my-input-disabled input {
    cursor: not-allowed;
}

.input-color-label {
    color: #111827;
    font-size: 14px;
    font-weight: 500;
}
</style>
