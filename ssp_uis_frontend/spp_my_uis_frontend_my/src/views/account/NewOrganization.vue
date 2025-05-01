<template>
	<div class="container">
		<AppListHeader title="AddNewOrganization" />

		<b-card class="bg-white border-0" style="border-radius: 25px">
			<b-card-text>
				<b-row class="align-center">
					<b-col sm="12" lg="5" md="6">
						<!-- mask="#########" -->
						<WInput v-model="filter.companyInn" rules="required" :label="$t('inn')" :placeholder="$t('inn')" name="inn" @keyup.enter="GetFromSoliq" />
					</b-col>
					<b-col>
						<b-button style="margin-top: 30px; padding: 12px 20px" @click="GetFromSoliq" :disabled="SoliqLoading" variant="primary" size="lg">
							<b-icon icon="search" v-if="!SoliqLoading"></b-icon>
							<b-spinner v-if="SoliqLoading" small></b-spinner>
						</b-button>
					</b-col>
				</b-row>

				<hr />

				<!-- company data -->
				<b-row class="mt-4" v-if="companyData.innOrPinfl">
					<b-col sm="12" md="4">
						<custom-label :content="companyData.innOrPinfl" :label="$t('inn')"></custom-label>
					</b-col>
					<b-col sm="12" md="4">
						<custom-label :content="companyData.name" :label="$t('contractor')"></custom-label>
					</b-col>
				</b-row>

				<b-button :disabled="SaveLoading || !companyData.innOrPinfl" @click="signModal = true" variant="primary" class="w-100 mt-4">
					<b-icon-plus scale="0.8"></b-icon-plus>
					{{ $t("add") }}
				</b-button>
			</b-card-text>
		</b-card>

		<!-- sign -->

		<b-modal v-model="signModal" :title="$t('ESPmodal')" no-close-on-backdrop hide-footer>
			<just-sign @sign="AddNewOrganization($event)"></just-sign>
		</b-modal>

		<!-- set organization modal -->
		<SetOrganizationModal v-if="setOrganizationModal" v-model="setOrganizationModal" />
	</div>
</template>

<script>
import { BIconPlus } from "bootstrap-vue";
import AccountService from "../../services/account.service";
import WInput from "../../components/forms/WInput.vue";
import AppListHeader from "../../components/application/AppListHeader.vue";
import customLabel from "../../components/elements/customLabel.vue";
import eimzoMixin from "@/mixins/eimzo";
import JustSign from "@/components/justSign.vue";
import SetOrganizationModal from "@/views/account/widgets/SetOrganizationModal.vue";

export default {
	components: { BIconPlus, WInput, AppListHeader, JustSign, customLabel, SetOrganizationModal },
	mixins: [eimzoMixin],
	data() {
		return {
			SoliqLoading: false,
			SaveLoading: false,
			signModal: false,
			setOrganizationModal: false,
			filter: {
				companyInn: "",
			},
			companyData: {},
		};
	},
	methods: {
		AddNewOrganization(data) {
			this.SaveLoading = true;
			const isPinfl = this.isPinfl(data);
			AccountService.AddNewOrganization({
				inn: isPinfl ? null : this.companyData.innOrPinfl,
				pinfl: isPinfl ? this.companyData.innOrPinfl : null,
				signedData: data.key,
				isPinfl: isPinfl,
			})
				.then(() => {
					this.makeToast(this.$t("SaveSuccess"), "success");
					// this.setOrganizationModal = true;
					this.signModal=false
					this.$router.push({ name: "MyCabinet" });
				})
				.catch((error) => {
					this.showApiError(error);
				})
				.finally(() => {
					this.SaveLoading = false;
				});
		},
		GetFromSoliq() {
			const length = this.filter.companyInn.length;
			if (length == 9 || length == 14) {
				this.SoliqLoading = true;
				this.companyData = {};
				if (length == 9) {
					AccountService.GetFromSoliq(this.filter.companyInn)
						.then((res) => {
							this.companyData.innOrPinfl = res.data.company.tin;
							this.companyData.name = res.data.company.shortName;
						})
						.catch((error) => {
							this.showApiError(error);
						})
						.finally(() => {
							this.SoliqLoading = false;
						});
				} else {
					AccountService.GetFromSoliqByPinfl(this.filter.companyInn)
						.then((res) => {
							this.companyData.innOrPinfl = this.filter.companyInn;
							this.companyData.name = res.data.fullName.uz;
						})
						.catch((error) => {
							this.showApiError(error);
						})
						.finally(() => {
							this.SoliqLoading = false;
						});
				}
			}
		},
	},
};
</script>

<style lang="scss" scoped></style>
