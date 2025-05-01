<template>
    <b-modal hide-header v-model="dialog" size="lg" no-close-on-backdrop hide-footer style="border-radius: 24px !important">
        <div class="p-3">
            <p v-if="lang == 'ru'" class="modal-text-style">
                Ўзбекистон Республикаси Давлат активларини бошқариш агентлиги ёки унинг ҳудудий бошқармаси Дастурга киритилган тадбиркорлик субъектларига табақалашган миқдорлардан
                келиб чиққан ҳолда, давлат кўчмас мулки объектлари ёки ерга бўлган ҳуқуқларни сотиб олиш билан боғлиқ тўловлар қуйидаги муддатларда амалга оширилади: 51 тадан 100
                тагача янги иш ўрни яратадиган тадбиркорлик субъектлари учун — уч йилдан кўп бўлмаган муддатда; 101 тадан 200 тагача янги иш ўрни яратадиган тадбиркорлик
                субъектлари учун — тўрт йилдан кўп бўлмаган муддатда; 200 тадан ортиқ янги иш ўрни яратадиган тадбиркорлик субъектлари учун — беш йилдан кўп бўлмаган муддатда.
            </p>
            <p v-if="lang == 'ru'">
                <b>Ўзбекистон республикаси Вазирлар махкамасининг 2023 йил 7 июльдаги 279-сон қарори</b>
            </p>
            <p v-if="lang == 'uz_latn'" class="modal-text-style">
                 O‘zbekiston Respublikasi Davlat aktivlarini boshqarish agentligi yoki uning         hududiy boshqarmasi Dasturga kiritilgan tadbirkorlik subyektlariga        
                tabaqalashgan miqdorlardan kelib chiqqan holda, davlat ko‘chmas mulki         obyektlari yoki yerga bo‘lgan huquqlarni sotib olish bilan bog‘liq to‘lovlar        
                quyidagi muddatlarda amalga oshiriladi: 51 tadan 100 tagacha yangi ish o‘rni         yaratadigan tadbirkorlik subyektlari uchun — uch yildan ko‘p bo‘lmagan        
                muddatda; 101 tadan 200 tagacha yangi ish o‘rni yaratadigan tadbirkorlik         subyektlari uchun — to‘rt yildan ko‘p bo‘lmagan muddatda; 200 tadan ortiq        
                yangi ish o‘rni yaratadigan tadbirkorlik subyektlari uchun — besh yildan ko‘p         bo‘lmagan muddatda.
            </p>
            <p v-if="lang == 'uz_latn'">
                <b>O‘zbekiston respublikasi Vazirlar maxkamasining 2023 yil 7 iyuldagi 279-son qarori</b>
            </p>
            <p v-if="lang == 'uz_cyrl'" class="modal-text-style">
                Ўзбекистон Республикаси Давлат активларини бошқариш агентлиги ёки унинг ҳудудий бошқармаси Дастурга киритилган тадбиркорлик субъектларига табақалашган миқдорлардан
                келиб чиққан ҳолда, давлат кўчмас мулки объектлари ёки ерга бўлган ҳуқуқларни сотиб олиш билан боғлиқ тўловлар қуйидаги муддатларда амалга оширилади: 51 тадан 100
                тагача янги иш ўрни яратадиган тадбиркорлик субъектлари учун — уч йилдан кўп бўлмаган муддатда; 101 тадан 200 тагача янги иш ўрни яратадиган тадбиркорлик
                субъектлари учун — тўрт йилдан кўп бўлмаган муддатда; 200 тадан ортиқ янги иш ўрни яратадиган тадбиркорлик субъектлари учун — беш йилдан кўп бўлмаган муддатда.
            </p>
            <p v-if="lang == 'uz_cyrl'">
                <b>Ўзбекистон республикаси Вазирлар махкамасининг 2023 йил 7 июльдаги 279-сон қарори</b>
            </p>
            <div class="d-flex justify-content-end">
                <a
                    href="javascript: void(0);"
                    @click="$router.push({ name: 'StateAssetApplication' })"
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
import AccountService from '@/services/account.service';
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
