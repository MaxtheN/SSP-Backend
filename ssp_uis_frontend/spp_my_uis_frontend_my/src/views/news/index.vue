<!-- eslint-disable vue/no-v-html -->
<template>
    <div class="container pt-2">
        <AppListHeaderForName title="News" class="rounded-sm mb-0">
            <template #top-right>
                <b-button
                    size="sm"
                    @click="
                        $router.push({
                            path: '/'
                        })
                    "
                    variant="danger"
                >
                    <b-icon-arrow-left></b-icon-arrow-left>
                    {{ $t('back') }}
                </b-button>
            </template>
        </AppListHeaderForName>

        <AppTable :busy="Loading" sm="6" md="6" lg="6" xl="4" :items="News" :filter="filter" @request="Refresh">
            <template #item="{ item }">
                <div class="news-card" @click="GetInfo(item)">
                    <div class="news-card__img">
                        <img :src="returnImgSrc(item.image)" alt="news1" />
                        <div class="news-card__date">
                            <b-badge variant="primary" v-if="item.news_tag">{{ item.news_tag.title }}</b-badge> | {{ formatDate(item.publishDate) }}
                        </div>
                    </div>
                    <div class="news-card__body">
                        <h4 class="news-card__title">
                            {{ item.title }}
                        </h4>
                        <h5 class="news-card__content">
                            {{ item.shortContent }}
                        </h5>
                        <div class="news-date mt-2 text-black-50">
                            <b-icon icon="eye"></b-icon>
                            <span style="margin-left: 2px">{{ item.viewCount }}</span>
                        </div>
                    </div>
                </div>
            </template>
        </AppTable>

        <b-sidebar v-model="sidebar" no-header width="100%" right shadow>
            <div class="w-100 h-100 bg-white overflow-auto">
                <div class="cl-stages-accepting-applications">
                    <b-row v-if="loadingsidebar" class="bg-white rounded-sm h-75">
                        <b-col md="12" sm="12" v-for="i in 1" :key="i + 'loader'" class="p-4 mb-2 border border-1 rounded-sm">
                            <b-skeleton-img class="skeleton" height="220" animation="wave"></b-skeleton-img>
                            <b-skeleton class="m-2"></b-skeleton>
                            <b-skeleton class="m-2"></b-skeleton>
                            <b-skeleton class="m-2"></b-skeleton>
                            <b-skeleton class="m-2"></b-skeleton>
                            <b-skeleton class="m-2"></b-skeleton>
                            <b-skeleton class="m-2"></b-skeleton>
                            <b-skeleton class="m-2"></b-skeleton>
                            <b-skeleton class="m-2"></b-skeleton>
                        </b-col>
                    </b-row>
                    <div v-else class="row justify-content-between pb-5">
                        <img class="w-100 h-auto my-3 rounded-lg mt-5" :src="returnImgSrcLarge(NewsInfo.image)" alt="news1" />
                        <p class="news-description px-2 mb-2 text-center" style="font-size: 22px; font-weight: bold">
                            {{ NewsInfo.title }}
                        </p>

                        <div v-html="NewsInfo.content" class="mb-3 b"></div>
                        <div v-if="NewsInfo.news_tag">
                            <b-badge variant="primary">{{ NewsInfo.news_tag.title }}</b-badge>
                        </div>
                        <div class="news-date mt-2 text-black-50">
                            <b-icon icon="calendar2-date"></b-icon><span style="margin-right: 15px; margin-left: 2px">{{ formatDate(NewsInfo.publishDate) }}</span>
                            <b-icon icon="eye"></b-icon>
                            <span style="margin-left: 2px">{{ NewsInfo.viewCount }}</span>
                        </div>
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
import AppListHeaderForName from '@/components/application/AppListHeaderForName.vue';
import NewsService from '@/services/news.service';
import AppTable from '@/components/application/AppTable.vue';
import AppCard from '@/components/application/AppCard.vue';
import { API_URL_CHAMBER } from '../../services/api.service';
import helperMixIn from '@/mixins/helper';

export default {
    mixins: [helperMixIn],
    data() {
        return {
            sidebar: false,
            axios,
            Loading: false,
            loadingsidebar: false,
            NewsInfo: {},
            News: [],
            lang: '',
            filter: {
                search: '',
                sortBy: '',
                orderType: 'asc',
                page: 1,
                pageSize: 10,
                total: 0
            }
        };
    },
    components: { AppTable, AppCard, AppListHeaderForName },
    created() {
        this.Refresh();
        if (this.$route.params.id) {
            this.GetInfo({ id: this.$route.params.id });
        }
    },
    computed: {
        returnImgSrc() {
            return (image) => API_URL_CHAMBER + image?.formats?.small?.url;
        },
        returnImgSrcLarge() {
            return (image) => API_URL_CHAMBER + image?.url;
        }
    },
    methods: {
        Refresh() {
            this.Loading = true;
            NewsService.GetList({ 'pagination[page]': this.filter.page, 'pagination[pageSize]': this.filter.pageSize, populate: '*' })
                .then((res) => {
                    this.News = res.data.data;
                    this.filter.total = res.data.meta.pagination.total;
                })
                .catch((error) => {
                    this.showApiError(error);
                })
                .finally(() => {
                    this.Loading = false;
                });
        },
        GetInfo(item) {
            this.loadingsidebar = true;
            this.sidebar = true;
            NewsService.Get(item.id, { params: { populate: '*' } }).then((res) => {
                this.NewsInfo = res.data.data;
                this.loadingsidebar = false;
            });
        }
    }
};
</script>
<style lang="scss">
@import '@/assets/styles/news-card.scss';
</style>
