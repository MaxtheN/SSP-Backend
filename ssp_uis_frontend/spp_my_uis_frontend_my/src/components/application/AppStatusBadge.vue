<template>
	<b-badge class="appbadge" :variant="getColor(item.application || item)">{{ item.status || item.application.status }}</b-badge>
</template>

<script>
export default {
	props: {
		item: {
			type: Object,
			default: () => ({}),
		},
	},
};
</script>

<style>
.appbadge {
	padding: 0.5em 0.7em;
}
</style>
