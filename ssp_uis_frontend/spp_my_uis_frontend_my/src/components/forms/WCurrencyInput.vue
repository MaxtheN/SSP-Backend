<template>
	<WInput ref="inputRef" type="text" class="text-rigth" :value="formattedValue" :label="label" v-bind="$attrs" :disabled="disabled" :rules="rules" />
</template>

<script>
import { useCurrencyInput } from "vue-currency-input";
import WInput from "./WInput.vue";
import { watch } from "vue";

const options = {
	locale: "pt-PT",
	currency: "EUR",
	currencyDisplay: "hidden",
	hideGroupingSeparatorOnFocus: false,
	autoDecimalDigits: false,
	useGrouping: true,
	accountingSign: false,
};

export default {
	components: { WInput },
	props: {
		value: {
			type: [String, Number],
			default: 0,
		},
		label: {
			type: String,
			default: "amount",
		},
		disabled: {
			type: Boolean,
			default: false,
		},
		rules: {
			type: String,
			default: "",
		},
	},
	setup(props) {
		const { inputRef, setValue, formattedValue } = useCurrencyInput(options);

		watch(
			() => props.value,
			(value) => {
				setValue(value);
			},
		);

		return { inputRef, options, formattedValue };
	},
};
</script>

<style lang="scss" scoped>
@import "./style.scss";
</style>
