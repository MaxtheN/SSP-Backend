<template>
    <div>
        <div class="my-input" :class="{ 'no-label': label === '', 'uppercase-text': uppertext }">
            <label class="input-color-label" v-if="label !== ''"> {{ label }} </label>
            <div class="my-label-content">
                {{ content }}
            </div>
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
        uppertext: {
            type: Boolean,
            default: false
        },
        content: {
            type: String,
            default: ''
        }
    },
    data() {
        return {
            // inputVal : ''
        };
    },
    methods: {}
};
</script>

<style lang="scss" scoped>
@import '../../assets/styles/variables.scss';
.my-input {
    .my-label-content {
        min-height: 33px;

        color: $primary-dark;
        background-color: #f7f7f7;
        border-radius: 12px;
        padding: 15px;
    }
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
        background-color: #f7f7f7;
        padding-left: 16px;
        border-radius: 12px;
        margin-bottom: 8px;
        &::placeholder {
            color: $input-text-placeholder;
        }
    }
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
</style>
