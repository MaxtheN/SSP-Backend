<template>
	<b-row>
		<b-col sm="12" md="4" lg="4">
			<WSelect
				@input="ChangeRegion"
				:disabled="type == 'address' ? true : false"
				:label="$t('region')"
				:name="$t('region')"
				:options="RegionList"
				:clearable="false"
				:value="regionId"
				:rules="type == 'address' ? '' : 'required'"
			/>
		</b-col>
		<b-col sm="12" md="4" lg="4">
			<WSelect
				:rules="type == 'address' ? '' : 'required'"
				:disabled="type == 'address' ? true : false"
				:name="$t('district')"
				:label="$t('district')"
				:options="DistrictList"
				:clearable="false"
				@input="ChangeDistrict"
				:value="districtId"
			/>
		</b-col>
		<b-col sm="12" md="4" lg="4">
			<WSelect
				:placeholder="$t('select')"
				:options="MfySelectList"
				:label="$t('livemfyname')"
				:name="type == 'address' ? 'mfy' : 'monoMfy'"
				:value="mfyId"
				:rules="type == 'address' ? 'required' : 'required'"
				@input="(e) => $emit('input:mfyId', e)"
			/>
		</b-col>

		<b-col sm="12" md="12" lg="12">
			<WInput
				:label="$t('address')"
				:name="$t('address')"
				:placeholder="$t('address')"
				@input="(e) => $emit('input:address', e)"
				:value="address"
				:disabled="type == 'address' ? true : false"
				:rules="type == 'address' ? '' : 'required'"
			/>
		</b-col>
	</b-row>
</template>

<script>
import WSelect from "@/components/forms/WSelect.vue";
import WInput from "@/components/forms/WInput.vue";
import { BRow, BCol } from "bootstrap-vue";
import ManualService from "@/services/manual.service";

export default {
	components: {
		WSelect,
		WInput,
		BRow,
		BCol,
	},
	props: {
		regionId: {
			type: Number,
			default: 0,
		},
		districtId: {
			type: Number,
			default: 0,
		},
		mfyId: {
			type: Number,
			default: 0,
		},
		address: {
			type: String,
			default: "",
		},
		type: {
			type: String,
			default: "address",
		},
	},
	emits: ["input:regionId", "input:districtId", "input:mfyId", "input:address"],
	data() {
		return {
			DistrictList: [],
			RegionList: [],
			MfySelectList: [],
		};
	},
	created() {
		ManualService.RegionSelectList().then((res) => {
			this.RegionList = res.data;
		});
		if (this.regionId) {
			ManualService.DistrictSelectList(this.regionId).then((res) => {
				this.DistrictList = res.data;
			});
		}

		if (this.districtId) {
			this.getMfyList(this.districtId);
		}
	},
	methods: {
		ChangeDistrict(districtId) {
			this.$emit("input:districtId", districtId);
			this.$emit("input:mfyId", null);
			this.getMfyList(districtId);
		},
		getMfyList(districtId) {
			if (districtId) {
				ManualService.MfySelectList(districtId)
					.then((res) => {
						this.MfySelectList = res.data;
					})
					.catch((error) => {
						this.showApiError(error);
					});
			}
		},
		ChangeRegion(item) {
			this.$emit("input:regionId", item);
			this.$emit("input:districtId", null);
			this.$emit("input:mfyId", null);
			ManualService.DistrictSelectList(item).then((res) => {
				this.DistrictList = res.data;
			});
		},
	},
};
</script>
