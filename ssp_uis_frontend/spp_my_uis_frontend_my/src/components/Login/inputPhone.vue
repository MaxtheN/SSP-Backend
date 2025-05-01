<template>
    <div class="d-flex flex-column">
        <label class="inputlabel" for="login">label</label>
        <input type="text" class="myinput" name="" id="login" />
        <p class="subtitle">{{ $t('usephonenumber') }}</p>
    </div>
</template>

<script>
export default {
    props: {
        // eslint-disable-next-line vue/require-prop-types
        label: {
            typeof: String,
            default: 'inter'
        }
    }
};
</script>

<style lang="scss" scoped>
.inputlabel {
    font-weight: 500;
    font-size: 14px;
    color: #111827;
}

.myinput {
    border: 1px solid #d5d7e1;
    border-radius: 8px;
    padding: 16px 20px;
    outline: none;
}
.subtitle {
    font-size: 12px;
    color: #6a6d7d;
    padding-top: 6px;
}
</style>
