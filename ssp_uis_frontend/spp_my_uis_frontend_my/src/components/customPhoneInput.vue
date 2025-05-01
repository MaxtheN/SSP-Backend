<template>
    <div>
        <div class="my-input" :class="{ 'no-label': label === '', 'my-input-disabled': disabled, 'uppercase-text': uppertext }">
            <label class="input-color-label" v-if="label !== ''"> {{ label }} </label>
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
    position: relative;

    .right-addon-icon {
        position: absolute;
        top: 15px;
        right: 15px;
        vertical-align: middle;
        cursor: pointer;
    }
    label {
        width: 100%;
        padding-left: 16px;
        text-transform: uppercase;
        font-size: 12px;
    }
    input {
        width: 100%;
        outline: none;
        border: none;
        background-color: #fff;
        padding-left: 50px;
        border-radius: 12px;
        margin-bottom: 8px;
        &::placeholder {
            color: $input-text-placeholder;
        }
    }
}
.my-input.my-input-disabled input {
    cursor: not-allowed;
}
.no-label {
    padding-top: 16px;
    padding-bottom: 8px;
    .input-color-label {
        display: none;
    }
}
.uppercase-text input {
    text-transform: uppercase;
}
.flag-img {
    width: 27px;
    position: absolute;
    top: 23px;
    left: 16px;
}
</style>
