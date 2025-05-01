<template>
	<b-modal hide-header v-model="dialog" size="lg" no-close-on-backdrop hide-footer style="border-radius: 24px !important">
		<b-row>
			<b-col sm="12" md="12" class="d-flex justify-content-end">
				<img @click="dialog = !dialog" style="cursor: pointer" width="25px" src="/images/ssp_images/cross-button.svg" alt="" />
			</b-col>
		</b-row>
		<div class="p-3">
			<p v-if="lang == 'ru'" class="modal-text-style">
				Указ Президента Республики Узбекистан от 12 июня 2023 года № ПФ-93 «О мерах по установлению взаимовыгодного сотрудничества с субъектами
				предпринимательства по сокращению бедности» Раздел 4 в пункте <br />
				а). Определено, что в программе могут участвовать хозяйствующие субъекты - юридические лица, осуществляющие деятельность не менее двух лет и
				не имеющие государственной доли в уставном капитале.
			</p>
			<p v-if="lang == 'uz_latn'" class="modal-text-style">
				Oʼzbekiston Respublikasi Prezidentining 2023 yil 12 iyundagi “Kambagʼallikni qisqartirishda tadbirkorlik subʼektlari bilan oʼzaro manfaatli
				hamkorlik oʼrnatishga qaratilgan chora-tadbirlar toʼgʼrisida”gi PF-93-son farmoni 4-qismi <br />
				a) bandida Dasturda kamida ikki yil faoliyat koʼrsatayotgan va ustav kapitalida davlat ulushi mavjud boʼlmagan tadbirkorlik subʼektlari —
				yuridik shaxslar ishtirok etishi mumkinligi belgilab oʼtilgan.
			</p>
			<p v-if="lang == 'uz_cyrl'" class="modal-text-style">
				Ўзбекистон Республикаси Президентининг 2023 йил 12 июндаги “Камбағалликни қисқартиришда тадбиркорлик субъектлари билан ўзаро манфаатли
				ҳамкорлик ўрнатишга қаратилган чора-тадбирлар тўғрисида”ги ПФ-93-сон фармони 4-қисми <br />
				а) бандида Дастурда камида икки йил фаолият кўрсатаётган ва устав капиталида давлат улуши мавжуд бўлмаган тадбиркорлик субъектлари — юридик
				шахслар иштирок этиши мумкинлиги белгилаб ўтилган.
			</p>
			<div class="modal-text-style">
				<div style="margin-left: 18px">
					<b> {{ $t("registrationDate") }} : {{ GetInfoData.registrationDate }}</b>
				</div>
				<div>
					<img v-if="GetInfoData.canCreate" src="/images/ssp_images/check-button.svg" alt="" />
					<img v-if="!GetInfoData.canCreate" width="25px" src="/images/ssp_images/cross-button.svg" alt="" />
					{{ $t("differenceInYears") }} : {{ GetInfoData.differenceInYears }} {{ $t("year") }} {{ GetInfoData.differenceInMonths }} {{ $t("month") }}
					{{ GetInfoData.differenceInDays }}
					{{ $t("day") }}
				</div>
				<div>
					<img v-if="!GetInfoData.isBudget && GetInfoData.canCreate" src="/images/ssp_images/check-button.svg" alt="" />
					<img v-if="GetInfoData.isBudget && !GetInfoData.canCreate" width="25px" src="/images/ssp_images/cross-button.svg" alt="" />
					{{ $t("isBudget") }} :
					{{ GetInfoData.isBudget ? $t("yes") : $t("no") }}
				</div>
				<div class="create-text-success mt-2" v-if="GetInfoData.canCreate">
					<div class="d-flex p-2">
						<img class="mx-3" src="/images/ssp_images/ellipse-success.svg" alt="" />
						{{ $t("isCanCreate") }}
					</div>
				</div>
				<div class="create-text-danger mt-2" v-if="!GetInfoData.canCreate">
					<div class="d-flex p-2">
						<img class="mx-3" src="/images/ssp_images/ellipse-danger.svg" alt="" />
						{{ $t("isNotCanCreate") }}
					</div>
				</div>
			</div>
			<div class="d-flex justify-content-end">
				<a
					href="javascript: void(0);"
					@click="dialog = !dialog"
					class="btn btn-sm anim-opacity mt-2 mr-2"
					style="
						background-color: #c75353;
						border-radius: 4px;
						border: 1px solid #e4e7ed;
						color: white;
						margin-right: 10px;
						white-space: nowrap !important;
					"
				>
					<b-icon-arrow-left></b-icon-arrow-left> {{ $t("back") }}
				</a>
				<a
					v-if="GetInfoData.canCreate"
					href="javascript: void(0);"
					@click="$router.push({ name: 'PartnershipApplicationEdit', params: { id: 0 } })"
					class="btn btn-sm anim-opacity mt-2"
					style="background-color: #219653; border-radius: 4px; color: white; white-space: nowrap !important"
				>
					<b-icon-plus></b-icon-plus> {{ $t("addAppilication") }}
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
		AccountService.GetContractorInfo()
			.then((res) => {
				this.GetInfoData = res.data;
			})
			.catch(this.showApiError);
	},
};
</script>

<style lang="scss" scoped>
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
