<template>
    <b-modal v-model="dialog" size="lg" no-close-on-backdrop hide-footer style="border-radius: 24px !important">
        <div class="p-3">
            <p v-if="lang == 'ru'" class="modal-text-style">
                20 минг тадбиркор — 500 минг малакали мутахассис» дастури доирасида тадбиркорлик субъектларига солиқ ва божхона имтиёзлари ҳамда преференцияларни қўллаш тартиби
                тўғрисидаги низомнинг 4-бандида кўрсатилган шартларни тўлиқ бажарган тадбиркорлик субъекти ўзининг шахсий кабинети орқали «Юридик шахсларнинг солиқ имтиёзларидан
                фойдаланиш бўйича мурожаат интерактив хизмати» орқали мурожаат қилиш йўли билан солиқ имтиёзидан фойдаланилади. Ҳавола орқали ўтиш
                <br />
                https://my3.soliq.uz/services/shared-services/application-for-benefits
            </p>
            <p v-if="lang == 'uz_latn'" class="modal-text-style">
                20 ming tadbirkor — 500 ming malakali mutaxassis» dasturi doirasida tadbirkorlik subyektlariga soliq va bojxona imtiyozlari hamda preferensiyalarni qo‘llash tartibi
                to‘g‘risidagi nizomning 4-bandida ko‘rsatilgan shartlarni to‘liq bajargan tadbirkorlik subyekti o‘zining shaxsiy kabineti orqali «Yuridik shaxslarning soliq
                imtiyozlaridan foydalanish bo‘yicha murojaat interaktiv xizmati» orqali murojaat qilish yo‘li bilan soliq imtiyozidan foydalaniladi. Havola orqali o‘tish
                <br />
                https://my3.soliq.uz/services/shared-services/application-for-benefits
            </p>
            <p v-if="lang == 'uz_cyrl'" class="modal-text-style">
                20 минг тадбиркор — 500 минг малакали мутахассис» дастури доирасида тадбиркорлик субъектларига солиқ ва божхона имтиёзлари ҳамда преференцияларни қўллаш тартиби
                тўғрисидаги низомнинг 4-бандида кўрсатилган шартларни тўлиқ бажарган тадбиркорлик субъекти ўзининг шахсий кабинети орқали «Юридик шахсларнинг солиқ имтиёзларидан
                фойдаланиш бўйича мурожаат интерактив хизмати» орқали мурожаат қилиш йўли билан солиқ имтиёзидан фойдаланилади. Ҳавола орқали ўтиш
                <br />
                https://my3.soliq.uz/services/shared-services/application-for-benefits
            </p>
            <div class="d-flex justify-content-end">
                <a
                    href="https://my3.soliq.uz/services/shared-services/application-for-benefits"
                    target="_blank"
                    class="btn btn-sm anim-opacity mt-2"
                    @click="$emit('close')"
                    style="background-color: #219653; border-radius: 4px; color: white; font-size: 18px; white-space: nowrap !important"
                >
                    {{ $t('SoliqApplication') }}
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
