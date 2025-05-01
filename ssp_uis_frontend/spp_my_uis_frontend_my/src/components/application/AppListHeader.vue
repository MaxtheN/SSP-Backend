<template>
	<div class="applist-header">
		<div class="d-flex justify-content-between">
			<div @click="$router.go(-1)" class="applist-header__back">
				<img src="/images/ssp_images/back-icon.svg" alt="back" />
				{{ $t("back") }}
			</div>
			<slot name="top-right"></slot>
		</div>
		<div class="applist-header__title">{{ $t(title) }}</div>
	</div>
</template>

<script>
export default {
	props: {
		title: {
			type: String,
			default: "",
		},
	},
};
</script>
<style lang="scss">
.applist-header {
	margin-top: 20px !important;
	font-style: normal;
	margin-bottom: 16px;

	&__back {
		color: #151f4a;
		font-size: 16px;
		font-weight: 500;
		line-height: normal;
		cursor: pointer;
	}

	&__title {
		color: #151f4a;
		font-size: 24px;
		text-align: center;
		font-weight: 600;
		line-height: 36px;
	}
}
</style>
