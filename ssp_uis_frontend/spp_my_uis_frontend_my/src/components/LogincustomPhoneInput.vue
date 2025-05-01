<template>
    <div>
        <label class="input-color-label"> {{ label }} </label>
        <div class="my-input">
            <b-img src="/images/ssp_images/flag-uzb.png" class="flag-img" alt="uz flag" />
            <input
                :type="type"
                @focus="$emit('focus', value)"
                @keyup="$emit('keyup', value)"
                :disabled="disabled"
                :value="value"
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
        type: {
            type: String,
            default: 'text'
        },
        // eslint-disable-next-line vue/require-default-prop, vue/require-prop-types
        value: {}
    },

    methods: {
        updateValue: function (value) {
            this.$emit('input', this.uppertext ? value.toUpperCase() : value);
        }
    }
};
</script>

<style lang="scss" scoped>
@import '../assets/styles/variables.scss';
.my-input {
    display: block;
    border-radius: 12px;
    width: 100%;
    background-color: #fff;
    border: 1px solid #d5d7e1;
    position: relative;
    padding: 16px 20px;
    input {
        width: 100%;
        outline: none;
        border: none;
        background-color: #fff;
        padding-left: 50px;
        color: #a0aec0;
        font-size: 14px;
        &::placeholder {
            color: $input-text-placeholder;
        }
    }
}

.flag-img {
    width: 27px;
    position: absolute;
    top: 14px;
    left: 36px;
}

.input-color-label {
    font-weight: 500px;
    font-size: 14px;
    color: #111827;
}
</style>
