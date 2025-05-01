<!-- eslint-disable vue/no-v-html -->
<template>
    <div class="container">
        <AppListHeader title="Services" />

        <AppTable :busy="Loading" sm="6" md="6" lg="6" xl="4" :items="Services" :filter="filter" @request="Refresh">
            <template #item="{ item, index }">
                <ServiceCard :item="item" :data-aos="dataAos(index)" data-aos-anchor-placement="center-bottom" @click.native="GetInfo(item.id)" />
            </template>
        </AppTable>

        <b-sidebar v-model="sidebar" shadow width="100vw" no-header right v-if="ServiceInfo">
            <div class="w-100 h-100 bg-white">
                <div class="cl-stages-accepting-applications">
                    <div class="row justify-content-between py-5">
                        <p class="Services-description px-2 mb-2 text-center" style="font-size: 22px; font-weight: bold">
                            {{ ServiceInfo.fullName }}
                        </p>
                        <img class="w-100 my-3" :src="returnImgSrc(ServiceInfo.image?.id)" :alt="ServiceInfo.fullName" />
                        <div v-html="ServiceInfo.details" class="mb-3 b"></div>
                    </div>
                </div>
                <span class="close-btn" style="position: absolute; top: 0px; right: 0px; cursor: pointer" @click="sidebar = false">
                    <img src="/images/design/fill-close.svg" alt="" />
                </span>
            </div>
        </b-sidebar>
    </div>
</template>

<script>
import axios from 'axios';
import NeedChamberServiceService from '@/services/needchamberservice.service';
import AppListHeader from '@/components/application/AppListHeader.vue';
import AppTable from '@/components/application/AppTable.vue';
import AppCard from '@/components/application/AppCard.vue';
import ServiceCard from '@/components/card/ServiceCard.vue';
export default {
    data() {
        return {
            sidebar: false,
            axios,
            Loading: false,
            ServiceInfo: {},
            Services: [],
            lang: '',
            filter: {
                search: '',
                sortBy: '',
                orderType: '',
                page: 1,
                pageSize: 9,
                total: 0
            }
        };
    },
    components: { AppListHeader, AppTable, AppCard, ServiceCard },
    created() {
        this.Refresh();
    },
    computed: {
        returnImgSrc() {
            return (id) => axios.defaults.baseURL + 'NeedChamberService/getimage/' + id;
        },
        dataAos() {
            return (i) => (i % 3 == 0 ? 'fade-up-right' : i % 3 == 1 ? 'fade-up' : 'fade-up-left');
        }
    },
    watch: {
        '$route.query': {
            handler(newQuery) {
                if (newQuery.id) {
                    this.GetInfo(newQuery.id);
                }
            },
            immediate: true
        }
    },
    methods: {
        Refresh() {
            this.Loading = true;
            NeedChamberServiceService.GetList(this.filter)
                .then((res) => {
                    this.Services = res.data.rows;
                    this.filter.total = res.data.total;
                })
                .catch((error) => {
                    this.showApiError(error);
                })
                .finally(() => {
                    this.Loading = false;
                });
        },
        GetInfo(id) {
            this.sidebar = true;
            NeedChamberServiceService.Get(id).then((res) => {
                this.ServiceInfo = res.data;
            });
        }
    }
};
</script>

<style>
body.overflow-hidden {
    overflow: hidden;
}
</style>
