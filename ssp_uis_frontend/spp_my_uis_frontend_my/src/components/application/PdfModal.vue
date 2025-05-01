<template>
    <b-modal v-model="dialog" size="lg" no-close-on-backdrop :title="$t('PdfView')" hide-footer style="border-radius: 24px !important">
        <div class="p-3">
            <iframe v-if="type == 1" src="/маълумот51_100.pdf" frameborder="0" width="100%" height="600px"></iframe>
            <iframe v-if="type == 2" src="/маълумот101_200.pdf" frameborder="0" width="100%" height="600px"></iframe>
            <iframe v-if="type == 3" src="/маълумот200+.pdf" frameborder="0" width="100%" height="600px"></iframe>
        </div>
    </b-modal>
</template>

<script>
export default {
    props: {
        value: {
            type: Boolean,
            default: false
        },
        type: {
            type: Number,
            default: 0
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
    },
    data() {
        return {
            GetInfoData: {},
            lang: localStorage.getItem('locale') || 'uz_latn'
        };
    }
};
</script>

<style lang="scss" scoped>
.modal-text-style {
    padding: 10px;
    margin: 10px;
    font-size: 18px;
}
.create-text-success {
    background-color: #27ae60;
    border-radius: 50px;
    width: 40%;
    color: #fff;
    font-family: Montserrat;
    font-size: 16px;
    font-style: normal;
    font-weight: 500;
    line-height: normal;
}
.create-text-danger {
    background-color: #c75353;
    border-radius: 50px;
    width: 40%;
    color: #fff;
    font-family: Montserrat;
    font-size: 16px;
    font-style: normal;
    font-weight: 500;
    line-height: normal;
}
</style>
