<template>
	<div class="service-card">
		<div class="service-card__header">
			<div class="service-card__title">{{ $t(item.shortName) }}</div>
			<div class="service-card__icon">
				<img :src="item.icon" alt="" />
			</div>
		</div>
		<div>
			<div class="service-card__img">
				<img :src="imgSrc || item.img" :alt="$t(item.shortName)" />
			</div>
		</div>
	</div>
</template>

<script>
import axios from "axios";
export default {
	props: {
		item: {
			type: Object,
			required: true,
		},
	},
	computed: {
		imgSrc() {
			if (this.item?.files && this.item?.files.length > 0&&0) {
				return axios.defaults.baseURL + "" + this.item?.files[0]?.id;
			}
			return "";
		},
	},
};
</script>

<style scoped lang="scss">
.service-card {
	border-radius: 25px;
	background: #fff;
	box-shadow: 5px 0px 25px 0px rgba(0, 0, 0, 0.1);
	padding: 0.7rem;
	cursor: pointer;

	&__header {
		display: flex;
		justify-content: space-between;
		padding: 0.6rem 1rem;
	}

	&__icon {
		border-radius: 15px;
		background: #edeceb;
		flex-shrink: 0;
		width: 55px;
		height: 55px;
		display: flex;
		justify-content: center;
		align-items: center;
	}

	&__title {
		color: #000;
		font-family: Montserrat;
		font-size: 16px;
		font-style: normal;
		font-weight: 600;
		line-height: normal;
		text-transform: uppercase;
		padding: 0.5rem 0;
	}

	&__img {
		overflow: hidden;
		border-radius: 25px;
		img {
			width: 100%;
			transition: 0.2s ease-in-out transform;
			transform: scale(1.01);
		}
	}

	&:hover {
		.service-card__img {
			img {
				transition: 0.2s ease-in-out transform;
				width: 100%;
				transform: scale(1.1);
			}
		}
	}
}
</style>
