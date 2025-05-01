<template>
	<div v-if="isToken" class="px-2 d-none d-md-flex align-items-center">
		<b-img src="/img/header-star.svg" height="60px" alt="" />
		<div>
			<div class="font-weight-bold" v-if="InfoCard.isMember">
				{{ $t("savdosanoatazosi") }} <small class="font-weight-bold">({{ InfoCard.state }})</small>
			</div>
			<div class="font-weight-bold" v-else>{{ $t("savdosanoatazosi") }} ({{ $t("no") }})</div>
			<small class="font-weight-bold" v-if="InfoCard.state != 'Tekin' && InfoCard.isMember">
				{{ $t("balance") }} :
				<span :class="InfoCard.balance > 0 ? 'text-success' : 'text-danger'">
					{{ InfoCard.balance == 0 ? "" : InfoCard.balance > 0 ? "+" : "-" }} {{ formatCurrency(InfoCard.balance) }}</span
				>
			</small>
		</div>
	</div>
</template>

<script>
import AccountService from "@/services/account.service";
import CurrencyMixin from "@/mixins/currency";

export default {
	mixins: [CurrencyMixin],
	data() {
		return {
			InfoCard: {},
			isToken: localStorage.getItem("user_info"),
		};
	},
	created() {
		if (this.$route.name !== "Home" && this.isToken) {
			AccountService.GetContractorMemshipState().then((res) => {
				this.InfoCard = res.data;
			});
		}
	},
};
</script>
