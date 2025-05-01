<template>
    <b-modal hide-header v-model="dialog" size="lg" no-close-on-backdrop hide-footer>
        <iframe src="/ХАРТИЯтасдиқланган.pdf" style="height: 80vh" width="100%" frameborder="0"></iframe>

        <b-button @click="$emit('accept')" size="sm" variant="success" class="float-right mt-2">
            {{ $t('Accept') }}
        </b-button>
        <b-button @click="dialog = false" size="sm" variant="light" class="float-right mt-2 mr-2">
            {{ $t('close') }}
        </b-button>
    </b-modal>
</template>

<script>
export default {
    props: {
        value: {
            type: Boolean,
            default: false
        }
    },
    computed: {
        dialog: {
            get() {
                return this.value;
            },
            set(val) {
                this.$emit('input', val);
            }
        }
    }
};
</script>
