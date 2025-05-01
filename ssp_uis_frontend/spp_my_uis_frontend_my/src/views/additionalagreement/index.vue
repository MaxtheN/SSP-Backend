<template>
	<AppTable :busy="Loading" :items="list" :filter="filter" @request="Refresh">
		<template #item="{ item }">
			<AppCard>
				<template #body>
					<table>
						<tr>
							<td style="width: 40%">{{ $t("contractor") }} :</td>
							<th style="width: 60%">{{ item.contractor }}</th>
						</tr>
						<tr>
							<td style="width: 40%">{{ $t("inn") }} :</td>
							<th style="width: 60%">{{ item.contractorInn }}</th>
						</tr>
						<tr>
							<td style="width: 40%">{{ $t("docNumberApplication") }} :</td>
							<th style="width: 60%">{{ item.docNumber }}</th>
						</tr>
						<tr class="py-3">
							<td style="width: 40%">{{ $t("docDateApplication") }} :</td>
							<th style="width: 60%">{{ item.docOn }}</th>
						</tr>
						<tr class="py-3">
							<td style="width: 40%">{{ $t("baseFixedMinimumValue") }} :</td>
							<th style="width: 60%">{{ item.baseFixedMinimumValue }}</th>
						</tr>
						<tr>
							<td class="pr-btn" style="width: 40%">{{ $t("status") }} :</td>
							<th style="width: 60%">
								<AppStatusBadge :item="item" />
							</th>
						</tr>
					</table>
				</template>
				<template #footer>
					<b-button
						@click="
							$router.push({
								name: 'AdditionalAgreementView',
								params: { id: item.id },
							})
						"
						variant="light"
						class="mr-1"
						size="sm"
					>
						<b-icon-eye scale="0.8"></b-icon-eye>
						{{ $t("View") }}
					</b-button>

					<b-button @click="Download(item.id2)" :disabled="downloadloading" size="sm" variant="success">
						<b-icon-download scale="0.8"></b-icon-download>
						{{ $t("download") }}
					</b-button>
				</template>
			</AppCard>
		</template>
	</AppTable>
</template>

<script>
import WTextarea from "@/components/forms/WTextarea.vue";
import AdditionalAgreementService from "@/services/additionalagreement.service";
import AppStatusBadge from "@/components/application/AppStatusBadge.vue";
import AppCard from "@/components/application/AppCard.vue";
import AppTable from "@/components/application/AppTable.vue";

export default {
	components: {
		AppStatusBadge,
		AppCard,
		WTextarea,
		AppTable,
	},
	data() {
		return {
			list: [],
			Loading: false,
			downloadloading: false,
			filter: {
				search: null,
				sortBy: null,
				orderType: null,
				page: 1,
				pageSize: 10,
				total: 0,
			},
		};
	},
	methods: {
		Refresh() {
			this.Loading = true;
			AdditionalAgreementService.GetList(this.filter)
				.then((res) => {
					this.list = res.data.rows;
					this.filter.total = res.data.total;
				})
				.catch(this.showApiError)
				.finally(() => {
					this.Loading = false;
				});
		},
		Download(id) {
			this.downloadloading = true;
			AdditionalAgreementService.DownloadPdf(id)
				.then((res) => {
					this.forceFileDownload(res, this.$t("AdditionalAgreement") + "_" + id, "pdf");
				})
				.catch(this.showApiError)
				.finally(() => {
					this.downloadloading = false;
				});
		},
	},
};
</script>
