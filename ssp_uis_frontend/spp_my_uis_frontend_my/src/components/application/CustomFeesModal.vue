<template>
  <b-modal
    
    v-model="dialog"
    size="lg"
    no-close-on-backdrop
    hide-footer
    style="border-radius: 24px !important"
  >
    <div class="p-3">
      <p v-if="lang == 'ru'" class="modal-text-style">
        Ўзбекистон республикаси Вазирлар махкамасининг 2023 йил 7 июльдаги
        279-сон қарорига мувофиқ Божхона кодексининг 329 — 333-моддаларида
        белгиланган тартибда божхона тўловларини таъминотсиз ва фоизсиз қуйидаги
        муддатларга бўлиб тўлаш ҳуқуқи берилади: 51 тадан 100 тагача янги иш
        ўрни яратадиган тадбиркорлик субъектлари учун — уч ой муддатда; 101
        тадан 200 тагача янги иш ўрни яратадиган тадбиркорлик субъектлари учун —
        олти ой; 200 тадан ортиқ янги иш ўрни яратадиган тадбиркорлик
        субъектлари учун — ўн икки ой. ҳаволага ўтиш:
        https://new.customs.uz/uz/services/ppay
      </p>
      <p v-if="lang == 'uz_latn'" class="modal-text-style">
        O‘zbekiston respublikasi Vazirlar maxkamasining 2023 yil 7 iyuldagi
        279-son qaroriga muvofiq Bojxona kodeksining 329 — 333-moddalarida
        belgilangan tartibda bojxona to‘lovlarini taʼminotsiz va foizsiz
        quyidagi muddatlarga bo‘lib to‘lash huquqi beriladi: 51 tadan 100
        tagacha yangi ish o‘rni yaratadigan tadbirkorlik subyektlari uchun — uch
        oy muddatda; 101 tadan 200 tagacha yangi ish o‘rni yaratadigan
        tadbirkorlik subyektlari uchun — olti oy; 200 tadan ortiq yangi ish
        o‘rni yaratadigan tadbirkorlik subyektlari uchun — o‘n ikki oy. havolaga
        o‘tish: https://new.customs.uz/uz/services/ppay
      </p>
      <p v-if="lang == 'uz_cyrl'" class="modal-text-style">
        Ўзбекистон республикаси Вазирлар махкамасининг 2023 йил 7 июльдаги
        279-сон қарорига мувофиқ Божхона кодексининг 329 — 333-моддаларида
        белгиланган тартибда божхона тўловларини таъминотсиз ва фоизсиз қуйидаги
        муддатларга бўлиб тўлаш ҳуқуқи берилади: 51 тадан 100 тагача янги иш
        ўрни яратадиган тадбиркорлик субъектлари учун — уч ой муддатда; 101
        тадан 200 тагача янги иш ўрни яратадиган тадбиркорлик субъектлари учун —
        олти ой; 200 тадан ортиқ янги иш ўрни яратадиган тадбиркорлик
        субъектлари учун — ўн икки ой. ҳаволага ўтиш:
        https://new.customs.uz/uz/services/ppay
      </p>
      <div class="d-flex justify-content-end">
        <a
          href="https://new.customs.uz/uz/services/ppay"
          target="_blank"
          class="btn btn-sm anim-opacity mt-2"
          @click="$emit('close')"
          style="
            background-color: #219653;
            border-radius: 4px;
            color: white;
            font-size: 18px;
            white-space: nowrap !important;
          "
        >
          {{ $t("CustomFeesApplication") }}
        </a>
      </div>
    </div>
  </b-modal>
</template>

<script>
import AccountService from "@/services/account.service";
export default {
  props: {
    value: {
      type: Boolean,
      default: false,
    },
  },
  computed: {
    dialog: {
      get() {
        return this.value;
      },
      set(val) {
        this.$emit("input", val);
      },
    },
  },
  data() {
    return {
      GetInfoData: {},
      lang: localStorage.getItem("locale") || "uz_latn",
    };
  },
  mounted() {
    // AccountService.GetContractorInfo()
    //   .then((res) => {
    //     this.GetInfoData = res.data;
    //   })
    //   .catch(this.showApiError);
  },
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
