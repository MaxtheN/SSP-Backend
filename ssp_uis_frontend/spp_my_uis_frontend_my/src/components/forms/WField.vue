<template>
	<b-form-group>
		<template v-slot:label>
			{{ label }}
			<span class="text-danger" v-if="required">*</span>
		</template>
		<div class="form-control">{{ value }}</div>
	</b-form-group>
</template>

<script>
export default {
	props: {
		value: {
			type: String,
			default: "",
		},
		label: {
			type: String,
			default: "",
		},
		required: {
			type: Boolean,
			default: false,
		},
	},
};
</script>

<style lang="scss">
@import "./style.scss";

div.form-control {
	height: 58px;
}
</style>
