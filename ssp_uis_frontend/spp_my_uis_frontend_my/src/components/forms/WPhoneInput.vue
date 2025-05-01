<template>
	<WInput
		type="text"
		:rules="rules"
		:label="label || $t('phonenumber')"
		:disabled="disabled"
		mask="+998 (##) ### ## ##"
		v-bind="$attrs"
		v-on="$listeners"
	/>
</template>

<script>
import WInput from "./WInput.vue";

export default {
	components: { WInput },
	props: {
		modelValue: {
			type: [String, Number],
			default: 0,
		},
		label: {
			type: String,
			default: "",
		},
		rules: {
			type: String,
			default: "",
		},
		disabled: {
			type: Boolean,
			default: false,
		},
	}
};
</script>

<style lang="scss" scoped>
@import "./style.scss";
</style>
