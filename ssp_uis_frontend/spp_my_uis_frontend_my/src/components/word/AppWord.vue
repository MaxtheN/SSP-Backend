<template>
	<div class="sertificate-html">
		<div>
			<h4 style="text-align: center"><b> “20 минг тадбиркор – 500 минг малакали мутахассис”</b> дастурида иштирок этиш тўғрисида</h4>
			<h4 style="color: #6eadff; text-align: center"><b>А Р И З А</b></h4>
			<div style="display: flex; margin-bottom: 30px">
				<p style="margin-left: 20px">
					<b style="white-space: nowrap">№ {{ data.docNumber }}</b>
				</p>
				<p style="float: right; margin-left: 460px">
					<b style="white-space: nowrap">Сана: {{ data.docOn }}</b>
				</p>
			</div>
			<p style="font-family: Montserrat">
				<span v-if="!data.chooseLocation">{{ data.regionName }} {{ data.districtName }} {{ data.mfyName }}</span
				><span v-if="data.chooseLocation">{{ data.choosedRegion }} {{ data.choosedDistrict }} {{ data.mfyName }}</span>
				да фаолият юритувчи
			</p>
			<p style="font-size: 16px">
				<span><em>(вилоят ва туман номи)</em></span>
			</p>
			<p style="font-family: Montserrat">{{ data.contractor }} ({{ data.contractorInn }}) дан</p>
			<p style="margin-left: 50px; font-size: 16px">
				<span><em>(тадбиркорлик субъекти номи ва СТИР рақами)</em></span>
			</p>
			<p style="text-align: justify; margin: 0; padding: 0; word-break: keep-all">
				Ўзбекистон Республикаси Президентининг 2023 йил 12 июндаги
				<b> “Камбағалликни қисқартиришда тадбиркорлик субъектлари билан ўзаро манфаатли ҳамкорлик ўрнатишга қаратилган чора-тадбирлар тўғрисида”</b>ги
				ПФ-93-сон Фармонига мувофиқ,<span v-if="!data.prtnContractTypeId">_________</span>
				<span style="margin-right: 5px; font-weight: 700" v-if="data.prtnContractTypeId"> {{ data.prtnContractType }}</span
				>тагача янги иш ўрни яратиш ҳамда берилган имтиёзлар ва преференциялардан фойдаланиш мақсадида дастурда иштирок этиш истагини билдираман.
			</p>
			<p>
				Шу боис,
				<span v-if="data.prtnContractTypeId">{{ orgName }}</span>
				<span v-if="!data.prtnContractTypeId">________________________________________________________________</span>
				ва
			</p>
			<p style="margin-left: 100px; font-size: 16px">
				<span><em>(вазирлик, вилоят ёки туман номи)</em></span>
			</p>
			{{ data.contractor }} ўртасида
			<p style="margin-left: 70px; font-size: 16px">
				<span><em>(тадбиркорлик субъекти номи)</em></span>
			</p>
			<b> ҳамкорлик шартномаси</b> тузиш масаласини кўриб чиқишингизни сўрайман.
			<p style="text-align: justify; word-break: keep-all">
				Ҳамкорлик шартномаси бўйича корхона зиммасига қўйиладиган мажбуриятларни ўз вақти ва тўлиқ бажаришга кафолат бераман.
			</p>
			<div style="display: flex">
				<p style="margin-left: 80px">
					{{ data.contractor }} <br /><span style="font-size: 16px"><em>(тадбиркорлик субъекти номи)</em></span>
				</p>
				<p style="float: right; margin-left: 270px">
					{{ data.contractorInn }}<br /><span style="font-size: 16px; white-space: nowrap"><em>(СТИР рақами)</em></span>
				</p>
			</div>
			<div style="display: flex; margin-top: 50px">
				<p><b>Корхона раҳбарининг Ф.И.Ш.</b></p>
				<p class="main-imzo"><em>(электрон имзо)</em></p>
			</div>
		</div>
	</div>
</template>

<script>
export default {
	props: {
		data: {
			type: Object,
			default: () => ({}),
		},
		orgName: {
			type: String,
			default: "",
		},
	},
};
</script>

<style lang="scss">
.sertificate-html {
	width: 750px;
	margin: 30px auto;
	font-size: 18.6px;
	text-align: center;
	.main-imzo {
		margin-left: 300px;
	}
}
</style>
