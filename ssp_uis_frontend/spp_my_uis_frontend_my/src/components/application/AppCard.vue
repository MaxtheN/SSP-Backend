<template>
	<div class="app-card">
		<div class="app-card__header">
			<slot name="header"></slot>
		</div>
		<div class="app-card__body">
			<slot name="body"></slot>
		</div>
		<div class="app-card__footer">
			<slot name="footer"></slot>
		</div>
	</div>
</template>

<script>
export default {
	inheritAttrs: false,
};
</script>
<style lang="scss">
.app-card {
	box-shadow: rgba(149, 157, 165, 0.2) 0px 8px 24px;
	background: var(--white);
	color: var(--gray-dark);
	border-radius: 25px;
	transition: 0.2s ease-in transform;
	&:hover {
		transform: translateY(-5px);
		transition: 0.2s ease-in transform;
	}

	&__body {
		padding: 1.25rem;
		& > table {
			td,
			th {
				padding: 8px 2px;
				vertical-align: baseline;
			}
		}
	}

	&__footer {
		display: flex;
		align-content: center;
		padding: 1.25rem;
		padding-top: 0;
	}
}
</style>
