<template>
    <b-modal hide-header v-model="dialog" size="lg" no-close-on-backdrop hide-footer style="border-radius: 24px !important">
        <div class="p-3">
            <p v-if="lang == 'ru'" class="modal-text-style">
                Уважаемый предприниматель! Вам в рамках программы «20 тысяч предпринимателей – 500 тысяч квалифицированных специалистов» предлагается заполнить форму о
                необходимости получения льготного кредита.
            </p>
            <p v-if="lang == 'uz_latn'" class="modal-text-style">
                Hurmatli tadbirkor! Siz, “20 ming tadbirkor – 500 ming malakali mutaxassis” dasturi doirasida imtiyozli kreditga boʼlgan ehtiyojingiz toʼgʼrisidagi shaklni
                toʼldirib yuborishingiz soʼraladi.
            </p>
            <p v-if="lang == 'uz_cyrl'" class="modal-text-style">
                Ҳурматли тадбиркор! Сиз, “20 минг тадбиркор – 500 минг малакали мутахассис” дастури доирасида имтиёзли кредитга бўлган эҳтиёжингиз тўғрисидаги шаклни тўлдириб
                юборишингиз сўралади.
            </p>
            <div class="d-flex justify-content-end">
                <a
                    href="javascript: void(0);"
                    @click="$router.push({ name: 'PrtnCreditDemandEdit', params: { id: 0 } })"
                    class="btn btn-sm anim-opacity mt-2"
                    style="background-color: #219653; border-radius: 4px; color: white; font-size: 18px; white-space: nowrap !important"
                >
                    <b-icon-plus></b-icon-plus> {{ $t('AddPrtnCreditDemand') }}
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
