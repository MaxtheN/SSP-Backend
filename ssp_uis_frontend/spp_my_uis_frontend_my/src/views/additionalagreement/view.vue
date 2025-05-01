<template>
	<div class="container">
		<AppListHeaderForName title="AdditionalAgreement" page-name="MemshipContract" />
		<b-overlay :show="Loading" style="min-height: calc(100vh - 460px)">
			<b-row :class="{ 'justify-content-center': true }">
				<b-col md="9" cols="12">
					<b-card class="form-card">
						<b-overlay :show="!iframeLoaded" spinner-variant="primary" spinner-type="grow" rounded="sm">
							<iframe
								v-if="Data.id2"
								:src="IframeSrc"
								width="100%"
								style="height: 100vh"
								name="application"
								id="application"
								frameborder="0"
								@load="iframeLoaded = true"
							></iframe>
						</b-overlay>
					</b-card>
				</b-col>

				<b-col sm="12" md="3" v-if="Data.canSign">
					<b-card class="form-card">
						<b-card-text>
							<b-button
								v-if="Data.canSign"
								:disabled="SendLoading"
								@click="OpenSendModal(Data)"
								style="margin-right: 5px; white-space: nowrap"
								variant="primary"
								class="btn btn-sm btn-soft-primary mr-2 mt-2 pr-btn myButton w-100"
							>
								<b-icon-arrow-bar-up scale="0.8"></b-icon-arrow-bar-up>
								{{ $t("sign") }}
							</b-button>
						</b-card-text>
					</b-card>
				</b-col>
			</b-row>
		</b-overlay>

		<b-modal v-model="signModal" :title="$t('ESPmodal')" no-close-on-backdrop hide-footer>
			<just-sign @sign="Send($event)"></just-sign>
		</b-modal>
	</div>
</template>

<script>
import AdditionalAgreementService from "@/services/additionalagreement.service";
import JustSign from "@/components/justSign.vue";

import axios from "axios";
import AppListHeaderForName from "@/components/application/AppListHeaderForName.vue";
import eimzoMixin from "@/mixins/eimzo";
export default {
	components: {
		JustSign,
		AppListHeaderForName,
	},
	mixins: [eimzoMixin],
	data() {
		return {
			Application: {},
			Data: {},
			Loading: false,
			sendLoading: false,
			iframeLoaded: false,
			SendLoading: false,
			signModal: false,
			selectedItem: {},
		};
	},
	created() {
		this.Refresh();
	},
	computed: {
		IframeSrc() {
			return axios.defaults.baseURL + `Memship/AdditionalAgreement/DownloadPdf?id2=${this.Data.id2}&lang=${this.getPdfLang()}`;
		},
	},
	methods: {
		FileDownload(item) {
			AdditionalAgreementService.DownloadPdf(item.id2).then((res) => {
				this.forceFileDownload(res, this.$t("AdditionalAgreement"));
			});
		},
		OpenSendModal(item) {
			this.selectedItem = item;
			this.signModal = true;
		},

		Send(data) {
			this.SendLoading = true;
			AdditionalAgreementService.Sign({
				id: this.Data.id,
				signedData: data.key,
				isPinfl: this.isPinfl(data),
			})
				.then(() => {
					this.signModal = false;
					this.Refresh();
				})
				.catch((error) => {
					this.showApiError(error);
				})
				.finally(() => {
					this.SendLoading = false;
				});
		},
		Refresh() {
			AdditionalAgreementService.Get(this.$route.params.id)
				.then((res) => {
					this.Data = res.data;
				})
				.catch((error) => {
					this.showApiError(error);
				});
		},
	},
};
</script>
