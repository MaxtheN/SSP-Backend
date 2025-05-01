<template>
	<b-modal v-model="dialog" hide-footer hide-header no-close-on-backdrop>
		<div style="text-align: right; margin-right: 10px; margin-top: -5px; margin-bottom: 5px; border-bottom: 1px solid lightgray">
			<span @click="dialog = false" style="cursor: pointer; font-size: 30px"> &times; </span>
		</div>
		<div>
			<WSelect
				:options="ContractorList"
				v-model="filter.contractorId"
				:label="$t('contractor')"
				valueid="tin"
				valuename="name"
				@input="selectContractor()"
			></WSelect>
		</div>
	</b-modal>
</template>

<script>
import AccountService from "@/services/account.service";
import WSelect from "@/components/forms/WSelect.vue";

export default {
	components: {
		WSelect,
	},
	data() {
		return {
			filter: {
				contractorId: 0,
			},
			ContractorList: [],
			dialog: false,
		};
	},
	mounted() {
		this.userName = JSON.parse(localStorage.getItem("user_info"));
		if (!this.ContractorId) {
			this.dialog = true;
			AccountService.GetContractorsList().then((res) => {
				this.ContractorList = res.data;
			});
		}
	},
	methods: {
		selectContractor() {
			if (!!this.filter.contractorId) {
				AccountService.SelectContractor(this.filter.contractorId)
					.then((res) => {
						localStorage.setItem("user_info", JSON.stringify(res.data));
						this.dialog = false;
					})
					.catch((error) => {
						this.showApiError(error);
					});
			}
		},
	},
	computed: {
		ContractorId() {
			return JSON.parse(localStorage.getItem("user_info"))?.contractorId;
		},
	},
};
</script>
