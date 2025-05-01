<template>
    <div class="custom-datepicker" :class="{ 'my-datepicker-disabled': disabled }">
        <label for=""> {{ label }} </label>
        <date-picker
            v-model="inputVal"
            format="DD.MM.YYYY"
            type="date"
            :lang="$t('lang') == 'Ру' ? ru : $t('lang') == 'Ўз' ? ru : uz"
            @change="$emit('change', value)"
            input-class="my-custom-date "
            :clearable="false"
            :disabled="disabled"
            :disabled-date="disabledDate"
        >
            <template #input>
                <input
                    :disabled="disabled"
                    @keyup="$emit('keyup', value)"
                    @input="Change"
                    :placeholder="placeholder"
                    v-mask="'##.##.####'"
                    type="text"
                    maxlength="10"
                    v-model="value"
                    class="my-custom-date"
                />
            </template>
            <template #icon-calendar>
                <span></span>
            </template>
        </date-picker>
        <div class="my-custom-calendar-icon">
            <b-img src="/images/design/calendar.svg" />
        </div>
    </div>
</template>

<script>
import DatePicker from 'vue2-datepicker';
import uz from '../elements/calendarlang.js';
import ru from 'vue2-datepicker/locale/ru';
import { BImg } from 'bootstrap-vue';
export default {
    components: { DatePicker, BImg },
    props: {
        label: {
            type: String,
            default: ''
        },
        placeholder: {
            type: String,
            default: ''
        },
        disabled: {
            type: Boolean,
            default: false
        },
        // eslint-disable-next-line vue/require-default-prop, vue/require-prop-types
        disabledDate: {},
        // eslint-disable-next-line vue/require-default-prop, vue/require-prop-types
        value: {}
    },
    data() {
        return {
            value1: '',
            uz,
            ru
        };
    },
    created() {},
    computed: {
        inputVal: {
            get() {
                var arr = this.value !== undefined ? this.value.split('.') : '';

                return arr !== '' ? new Date(arr[2], arr[1] - 1, arr[0]) : this.value;
            },
            set(val) {
                var dd = String(val.getDate()).padStart(2, '0');
                var mm = String(val.getMonth() + 1).padStart(2, '0');
                var yy = val.getFullYear();
                this.value1 = `${dd}.${mm}.${yy}`;
                this.$emit('input', `${dd}.${mm}.${yy}`);
            }
        }
    },
    methods: {
        Change() {},
        handleInput() {
            this.$emit('input', this.value);
            // alert(this.value)
        }
    },
    watch: {
        value: {
            handler(newValue) {
                this.value1 = newValue;
            }
        }
    }
};
</script>

<style lang="scss" scoped>
@import '../../assets/styles/variables.scss';
@import 'vue2-datepicker/scss/index.scss';

.custom-datepicker {
    border-radius: 12px;
    background-color: #f7f7f7;
    position: relative;
    overflow: hidden;
    label {
        width: 100%;
        padding-left: 16px;
        text-transform: uppercase;
        font-size: 12px;
        color: $input-text-color;
    }
    .my-custom-calendar-icon {
        position: absolute;
        top: 15px;
        right: 15px;
    }
}
.mx-datepicker {
    .mx-input-wrapper {
        overflow: hidden;
        .my-custom-date {
            border: none !important;
            outline: none;
            background-color: #f7f7f7;
            margin-bottom: 8px;
            padding-left: 16px;
            width: fit-content;
            &::placeholder {
                color: $input-text-placeholder;
            }
        }
    }
}
.my-datepicker-disabled {
    input {
        cursor: not-allowed;
    }
}
</style>
