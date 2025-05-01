<template>
	<ValidationProvider :name="$attrs.name" :rules="rules" v-slot="{ valid, errors }">
		<b-form-group>
			<b-form-checkbox-group :state="errors[0] ? false : valid ? true : null" v-model="innerValue">
				<slot />
			</b-form-checkbox-group>
			<b-form-invalid-feedback id="inputLiveFeedback">{{ errors[0] }}</b-form-invalid-feedback>
		</b-form-group>
	</ValidationProvider>
</template>

<script>
import { ValidationProvider } from "vee-validate";

export default {
	components: {
		ValidationProvider,
	},
	props: {
		// eslint-disable-next-line vue/require-default-prop
		vid: {
			type: String,
		},
		rules: {
			type: [Object, String],
			default: "",
		},
		// eslint-disable-next-line vue/require-default-prop
		value: {
			type: null,
		},
	},
	data: () => ({
		innerValue: "",
	}),
	watch: {
		// Handles internal model changes.
		innerValue(newVal) {
			this.$emit("input", newVal);
		},
		// Handles external model changes.
		value(newVal) {
			this.innerValue = newVal;
		},
	},
	created() {
		if (this.value) {
			this.innerValue = this.value;
		}
	},
};
</script>

<style lang="scss" scoped>
@import "./style.scss";
</style>