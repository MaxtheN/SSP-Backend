<template>
    <b-modal hide-header v-model="dialog" size="lg" no-close-on-backdrop hide-footer style="border-radius: 24px !important">
        <div class="p-3">
            <p v-if="lang == 'ru'" class="modal-text-style">
                Вазирлар Маҳкамасининг 2023 йил 7 июлдаги 279-сон қарорига 2-илова «20 минг тадбиркор — 500 минг малакали мутахассис» дастурини амалга ошириш тартиби тўғрисида
                низомида назарда тутилган имтиёзлар Дастурга киритилган Тошкент шаҳридан рўйхатдан ўтган тадбиркорлик субъектларига нисбатан қўлланилмайди.
            </p>
            <p v-if="lang == 'uz_latn'" class="modal-text-style">
                Vazirlar Mahkamasining 2023 yil 7 iyuldagi 279-son qaroriga 2-ilova «20 ming tadbirkor — 500 ming malakali mutaxassis» dasturini amalga oshirish tartibi to‘g‘risida
                nizomida nazarda tutilgan imtiyozlar Dasturga kiritilgan Toshkent shahridan ro‘yxatdan o‘tgan tadbirkorlik subyektlariga nisbatan qo‘llanilmaydi.
            </p>
            <p v-if="lang == 'uz_cyrl'" class="modal-text-style">
                Вазирлар Маҳкамасининг 2023 йил 7 июлдаги 279-сон қарорига 2-илова «20 минг тадбиркор — 500 минг малакали мутахассис» дастурини амалга ошириш тартиби тўғрисида
                низомида назарда тутилган имтиёзлар Дастурга киритилган Тошкент шаҳридан рўйхатдан ўтган тадбиркорлик субъектларига нисбатан қўлланилмайди.
            </p>
            <div class="d-flex justify-content-end">
                <a
                    href="javascript: void(0);"
                    @click="$router.push({ name: 'Cabinet' })"
                    class="btn btn-sm anim-opacity mt-2"
                    style="background-color: #219653; border-radius: 4px; color: white; font-size: 18px; white-space: nowrap !important"
                >
                    <!-- <b-icon-plus></b-icon-plus> -->
                    {{ $t('ok') }}
                </a>
            </div>
        </div>
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
