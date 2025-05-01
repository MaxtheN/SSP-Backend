<template>
	<b-col class="text-center">
		<div class="d-flex justify-content-center">
			<div class="word">
				<table style="border-collapse: collapse; border: none">
					<tbody>
						<tr>
							<td style="width: 233.6pt; padding: 0in 5.4pt; height: 78pt; vertical-align: top">
								<p class="p-right">
									<em>
										<span style="font-size: 19px">Кимга:</span>
									</em>
								</p>
							</td>
							<td style="width: 233.65pt; padding: 0in 5.4pt; height: 78pt; vertical-align: top">
								<p class="p-center">
									<strong>
										<span style="font-size: 19px">Ўзбекистон Республикаси</span>
									</strong>
								</p>
								<p class="p-center">
									<strong>
										<span style="font-size: 19px">Савдо-саноат палатаси</span>
									</strong>
								</p>
								<p class="p-center">
									<strong>
										<span style="font-size: 19px; background: yellow">Тошкент вилояти</span>
									</strong>
								</p>
								<p class="p-center">
									<strong>
										<span style="font-size: 19px">ҳудудий бошқармасига</span>
									</strong>
								</p>
							</td>
						</tr>
						<tr>
							<td style="width: 233.6pt; padding: 0in 5.4pt; vertical-align: top">
								<p class="p-right">
									<em>
										<span style="font-size: 19px">Кимдан:</span>
									</em>
								</p>
							</td>
							<td style="width: 233.65pt; padding: 0in 5.4pt; vertical-align: top">
								<p class="p-center">
									<strong>
										<span style="font-size: 19px">Палата аъзоси</span>
									</strong>
								</p>
								<p class="p-center">
									<strong>
										<span style="font-size: 19px">
											{{ data.contractor }}
											<br />&nbsp;
										</span>
									</strong>
									<em>
										<span style="font-size: 16px">Манзил: {{ data.region }} вилояти, {{ data.district }} тумани (шаҳри)</span>
									</em>
								</p>
								<p class="p-center">
									<em>
										<span style="font-size: 16px">_______________ МФЙ;</span>
									</em>
								</p>
								<p class="p-center">
									<em>
										<span style="font-size: 16px">Банк ва ҳисоб рақами:&nbsp;</span>
									</em>
									<em>
										<span style="font-size: 16px">_______________</span>
									</em>
								</p>
								<p class="p-center">
									<em>
										<span style="font-size: 16px">______________________________</span>
									</em>
								</p>
								<p class="p-center">
									<em>
										<span style="font-size: 16px">СТИР: ______________</span>
									</em>
								</p>
							</td>
						</tr>
					</tbody>
				</table>
				<p
					style="
						margin-top: 0in;
						margin-right: 0in;
						margin-bottom: 0in;
						margin-left: 0in;
						font-size: 11pt;
						font-family: 'Calibri', sans-serif;
						text-align: right;
						line-height: normal;
					"
				>
					<strong>
						<span style="font-size: 11px">&nbsp;</span>
					</strong>
				</p>
				<p class="p-center">
					<strong>
						<span style="font-size: 19px; color: #0070c0">Палата аъзоси манфаатида судга даъво аризаси киритиш тўғрисида</span>
					</strong>
				</p>
				<p class="p-center">
					<strong>
						<span style="font-size: 19px; color: #c00000">АРИЗА</span>
					</strong>
				</p>
				<p class="p-justify">
					<span style="font-size: 9px">&nbsp;</span>
				</p>
				<p class="p-justify">
					<strong>
						<span style="font-size: 17px; display: flex; justify-content: space-between">
							<div>№ {{ data.docNumber }}</div>
							<div>Сана: {{ data.docOn }}</div>
						</span>
					</strong>
				</p>
				<p class="p-justify">
					<strong>
						<span style="font-size: 17px">&nbsp;</span>
					</strong>
				</p>
				<p class="p-justify-2">
					<span style="font-size: 17px">
						&ldquo;Ўзбекистон Республикаси Савдо-саноат палатаси тўғрисида&rdquo;ги Қонун ва Палата аъзоси билан тузилган аъзолик шартномасига асосан,
						_______________________________________ манфаатида жавобгар(лар)
						<br />&nbsp;
					</span>
					<span style="font-size: 13px">
						<em style="margin-left: 20px"> (Палата аъзосининг номи) </em>
					</span>
				</p>
				<p class="p-justify">
					<span style="font-size: 17px">
						___________________________________________________га нисбатан судга&nbsp;
						<br />
					</span>
					<span style="font-size: 13px">
						<em style="margin-left: 20px"> (Корхона номи ёки ж/ш исм-шарифи) </em>
					</span>
				</p>
				<p class="p-justify">
					<span style="font-size: 17px">
						_______________________________ тўғрисида даъво аризаси киритишда
						<br />&nbsp;
					</span>
					<span style="font-size: 13px">
						<em style="margin-left: 20px"> (даъво предмети) </em>
					</span>
				</p>
				<p class="p-justify">
					<span style="font-size: 17px">амалий ёрдам беришингизни сўрайман.</span>
				</p>
				<p class="p-justify">
					<span style="font-size: 11px">&nbsp;</span>
				</p>
				<p class="p-justify-2">
					<strong>
						<span style="font-size: 17px">Жавобгар(лар) тўғрисида маълумот:</span>
					</strong>
				</p>
				<p
					style="
						margin-top: 0in;
						margin-right: 0in;
						margin-bottom: 0in;
						margin-left: 28.35pt;
						font-size: 11pt;
						font-family: 'Calibri', sans-serif;
						text-align: justify;
						line-height: normal;
					"
				>
					<span style="font-size: 17px">
						_______________________________&nbsp;
						<br />&nbsp;
					</span>
					<span style="font-size: 13px">
						&nbsp; &nbsp; &nbsp; &nbsp;
						<em>(Корхона номи ёки ж/ш исм-шарифи) &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;</em>
					</span>
				</p>
				<p class="p-justify-2">
					<strong>
						<span style="font-size: 17px">Манзил:</span>
					</strong>
					<span style="font-size: 17px">&nbsp;___________ вилоят, __________ туман (шаҳар)</span>
				</p>
				<p class="p-justify-2">
					<span style="font-size: 17px">&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; ____________ МФЙ, __________ кўчаси, ___ уй.</span>
				</p>
				<p class="p-justify-2">
					<strong>
						<span style="font-size: 17px">Телефон:</span>
					</strong>
					<span style="font-size: 17px">&nbsp;_____________.</span>
				</p>
				<p class="p-justify-2">
					<strong>
						<span style="font-size: 17px">Банк ҳисоб рақами:&nbsp;</span>
					</strong>
					<span style="font-size: 17px">_____________________________</span>
				</p>
				<p class="p-justify-2">
					<span style="font-size: 17px">СТИР: _____________; Ж</span>
					<span style="font-size: 17px">Ш</span>
					<span style="font-size: 17px">ШИР: _____________________&nbsp;</span>
					<em>
						<span style="font-size: 13px">(агар жисмоний б</span>
					</em>
					<em>
						<span style="font-size: 13px">ўлса)</span>
					</em>
				</p>
				<p class="p-justify-2">
					<span style="font-size: 17px">&nbsp;</span>
				</p>
				<p class="p-justify-2">
					<strong>
						<span style="font-size: 17px">Даъво суммаси жами:&nbsp;</span>
					</strong>
					<span style="font-size: 17px"> {{ data.totalAmount }} ({{ data.currency }}) </span>
					<em>
						<span style="font-size: 13px">(даъво предметидан келиб чиқиб мавжуд бўлганда ) :</span>
					</em>
				</p>
				<p class="p-justify-2">
					<strong>
						<span style="font-size: 17px">Шундан:</span>
					</strong>
				</p>
				<p class="p-justify-2">
					<span style="font-size: 17px">асоссий қарздорлик -&nbsp;</span>
					<span style="font-size: 17px">{{ data.mainDebt }} ({{ data.currency }}) </span>
				</p>
				<p class="p-justify-2">
					<span style="font-size: 17px">ҳисобланган пеня -&nbsp;</span>
					<span style="font-size: 17px">{{ data.calculedPenalty }} ({{ data.currency }}) </span>
				</p>
				<p class="p-justify-2">
					<span style="font-size: 17px">жарима -&nbsp;</span>
					<span style="font-size: 17px">{{ data.penalty }} ({{ data.currency }}) </span>
				</p>
				<p class="p-justify-2">
					<span style="font-size: 17px">фоиз -&nbsp;</span>
					<span style="font-size: 17px">{{ data.percent }} ({{ data.currency }}) </span>
				</p>
				<p class="p-justify-2">
					<em>
						<span style="font-size: 16px">Илова: электрон шаклда мавжуд&nbsp;</span>
					</em>
				</p>
				<p class="p-justify-2">
					<span style="font-size: 17px">&nbsp;</span>
				</p>
				<p
					style="
						margin-top: 0in;
						margin-right: 0in;
						margin-bottom: 8pt;
						margin-left: 0in;
						font-size: 11pt;
						font-family: 'Calibri', sans-serif;
						text-align: justify;
						text-indent: 28.35pt;
					"
				>
					<strong>
						<span style="font-size: 19px">Корхона</span>
					</strong>
					<span style="font-size: 19px">
						&nbsp;
						<strong>раҳбарининг Ф.И.Ш.</strong>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
						&nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
						<em>(электрон имзо)</em>
					</span>
				</p>
			</div>
		</div>
	</b-col>
</template>

<script>
export default {
	props: {
		data: {
			type: Object,
			default: () => ({}),
		},
	},
};
</script>

<style lang="scss">
.word {
	max-width: 750px;
	.p-right {
		margin-top: 0in;
		margin-right: 0in;
		margin-bottom: 0in;
		margin-left: 0in;
		font-size: 11pt;
		font-family: "Calibri", sans-serif;
		text-align: right;
		line-height: normal;
	}
	.p-center {
		margin-top: 0in;
		margin-right: 0in;
		margin-bottom: 0in;
		margin-left: 0in;
		font-size: 11pt;
		font-family: "Calibri", sans-serif;
		text-align: center;
		line-height: normal;
	}
	.p-justify {
		margin-top: 0in;
		margin-right: 0in;
		margin-bottom: 0in;
		margin-left: 0in;
		font-size: 11pt;
		font-family: "Calibri", sans-serif;
		text-align: justify;
		line-height: normal;
	}

	.p-justify-2 {
		margin-top: 0in;
		margin-right: 0in;
		margin-bottom: 0in;
		margin-left: 0in;
		font-size: 11pt;
		font-family: "Calibri", sans-serif;
		text-align: justify;
		text-indent: 28.35pt;
		line-height: normal;
	}
}
</style>
