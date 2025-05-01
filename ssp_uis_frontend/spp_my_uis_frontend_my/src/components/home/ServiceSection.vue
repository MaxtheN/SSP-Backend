<template>
	<div class="container py-4">
		<b-row>
			<b-col>
				<h5 class="enter-text">{{ $t("Xizmatlar") }}</h5>
			</b-col>
			<b-col>
				<div class="text-right">
					<router-link to="/services" class="text-primary text-decoration-none"> {{ $t("Barcha xizmatlar") }} <b-icon-chevron-right /> </router-link>
				</div>
			</b-col>
		</b-row>
		<b-row class="gx-4 gy-4">
			<template v-for="(service, i) in services">
				<b-col sm="6" lg="4" :key="i + 'service'" class="my-3">
					<ServiceCard
						@click.native="$router.push({ path: '/services', query: { id: service.id } })"
						:item="service"
						:data-aos="dataAos(i)"
						data-aos-anchor-placement="center-bottom"
					/>
				</b-col>
			</template>
		</b-row>
	</div>
</template>

<script>
import ServiceCard from "@/components/card/ServiceCard.vue";
import ManualService from "@/services/manual.service";
import NeedChamberServiceService from "@/services/needchamberservice.service";
export default {
	components: { ServiceCard },
	data() {
		return {
			services: [],
			servicesTest: [
				{
					title: "SSP xizmatlari",
					img: "/images/services/service1.png",
					icon: "/images/services/icon1.svg",
				},
				{
					title: "SSP bilan bog'lanish",
					img: "/images/services/service2.png",
					icon: "/images/services/icon2.svg",
				},
				{
					title: "Koronavirus bilan kurashish COVID-19",
					img: "/images/services/service3.png",
					icon: "/images/services/icon3.svg",
				},
				{
					title: "Hakamlik sudlari",
					img: "/images/services/service4.png",
					icon: "/images/services/icon4.svg",
				},
				{
					title: "Toshkent Xalqaro Arbitraj Markazi",
					img: "/images/services/service5.png",
					icon: "/images/services/icon5.svg",
				},
				{
					title: "TOBB universiteti",
					img: "/images/services/service6.png",
					icon: "/images/services/icon6.svg",
				},
			],
			filter: {
				search: "",
				sortBy: "id",
				orderType: "",
				page: 1,
				pageSize: 6,
			},
		};
	},
	computed: {
		dataAos() {
			return (i) => (i % 3 == 0 ? "fade-up-right" : i % 3 == 1 ? "fade-up" : "fade-up-left");
		},
	},
	created() {
		ManualService.NeedChamberServiceSelectList();
		NeedChamberServiceService.GetList(this.filter).then((res) => {
			this.services = res.data.rows.map((e, i) => ({ ...e, icon: this.servicesTest[i % 6].icon, img: this.servicesTest[i % 6].img }));
		});
	},
};
</script>
