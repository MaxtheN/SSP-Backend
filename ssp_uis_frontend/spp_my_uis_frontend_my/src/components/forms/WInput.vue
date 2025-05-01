<template>
	<ValidationProvider :vid="vid" :name="$attrs.name" :rules="rules" v-slot="{ valid, errors, required }">
		<b-form-group v-bind="$attrs">
			<template v-slot:label>
				{{ $attrs.label }}
				<span class="text-danger" v-if="required">*</span>
			</template>
			<b-form-input v-bind="$attrs" v-on="$listeners" v-mask="mask" :state="errors[0] ? false : valid ? true : null"></b-form-input>
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
		vid: {
			type: String,
			default: "",
		},
		rules: {
			type: [Object, String],
			default: "",
		},
		mask: {
			type: String,
			default: "",
		},
	},
};
</script>

<style lang="scss" scoped>
@import "./style.scss";
</style>
