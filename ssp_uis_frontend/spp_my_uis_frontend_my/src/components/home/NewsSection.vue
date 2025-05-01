<template>
    <div class="news-section" v-if="!loading && NewsList.length > 0">
        <b-row v-if="loading" class="bg-white border border-border-color rounded-sm">
            <b-col md="4" sm="6" v-for="i in 3" :key="i + 'loader'" class="p-4 mb-2 border border-1 rounded-sm">
                <b-skeleton-img class="skeleton" height="220" animation="wave"></b-skeleton-img>
            </b-col>
        </b-row>

        <b-row v-else-if="NewsList.length" class="bg-white border border-border-color rounded-sm">
            <b-col cols="12" class="d-flex justify-content-between p-4 mb-2 align-items-center border-bottom">
                <h3>{{ $t('Yangiliklar') }}</h3>
                <router-link to="/news" class="text-primary text-decoration-none"> {{ $t('Barcha yangiliklar') }} <b-icon-chevron-right /> </router-link>
            </b-col>
            <b-col class="newWarapper" sm="12" md="6" v-if="news1">
                <div class="news-card1" @click="$router.push({ name: 'News', params: { id: news1.id } })">
                    <img class="news-card1__img" :src="returnImgSrc(news1.image)" :alt="news1.title" />
                    <div class="d-flex flex-column justify-content-between news-card1__footer">
                        <div>
                            <h4 class="news-card1__title">{{ news1.title }}</h4>
                            <h4 class="news-content" v-html="firstParagraph"></h4>
                        </div>

                        <h6 class="news-card1__date d-flex justify-content-between align-items-center">
                            {{ formatDate(news1.publishDate) }}
                            <b-button
                                @click="$router.push({ name: 'News', params: { id: news1.id } })"
                                class="bg-white bg-white rounded-sm border border-border-color p-2"
                                size="sm"
                            >
                                <b-icon-chevron-right variant="info" class="newCard_btn"></b-icon-chevron-right
                            ></b-button>
                        </h6>
                    </div>
                </div>
            </b-col>
            <b-col class="newWarapper" sm="12" md="6" v-if="news2">
                <div class="news-card1" @click="$router.push({ name: 'News', params: { id: news2.id } })">
                    <img class="news-card1__img" :src="returnImgSrc(news2.image)" :alt="news2.title" />
                    <div class="d-flex flex-column justify-content-between news-card1__footer">
                        <div>
                            <h4 class="news-card1__title">{{ news2.title }}</h4>
                            <h4 class="news-content" v-html="secondParagraph"></h4>
                        </div>

                        <h6 class="news-card1__date d-flex justify-content-between align-items-center">
                            {{ formatDate(news2.publishDate) }}
                            <b-button
                                @click="$router.push({ name: 'News', params: { id: news2.id } })"
                                class="bg-white bg-white rounded-sm border border-border-color p-2"
                                size="sm"
                            >
                                <b-icon-chevron-right variant="info" class="newCard_btn"></b-icon-chevron-right
                            ></b-button>
                        </h6>
                    </div>
                </div>
            </b-col>
            <b-col class="neewrapper2" sm="12" md="12">
                <template v-for="(n, i) in NewsList">
                    <div :key="i" class="news-card2">
                        <img class="news-card2__img" :src="returnImgSrc(n.image)" :alt="n.title" />
                        <div>
                            <router-link class="news-card2__title" :to="{ name: 'News', params: { id: n.id } }">{{ n.title }} </router-link>
                            <p class="news-card2__content" v-html="getFirstParagraph(n.content)"></p>
                            <h6 class="news-card2__date d-flex justify-content-between align-items-center mt-2">
                                <span class="mx-1">
                                    <b-icon icon="calendar2-date"></b-icon>
                                    {{ formatDate(n.publishDate) }}
                                </span>

                                <span class="mx-1"
                                    ><b-button
                                        @click="$router.push({ name: 'News', params: { id: n.id } })"
                                        class="bg-white bg-white rounded-sm border border-border-color p-2"
                                        size="sm"
                                    >
                                        <b-icon-chevron-right variant="info" class="newCard_btn"></b-icon-chevron-right></b-button
                                ></span>
                            </h6>
                        </div>
                    </div>
                </template>
            </b-col>
        </b-row>
    </div>
</template>

<script>
import NewsService from '@/services/news.service';
import { API_URL_CHAMBER } from '../../services/api.service';
import helperMixIn from '@/mixins/helper';

export default {
    mixins: [helperMixIn],
    data() {
        return {
            loading: false,
            NewsList: []
        };
    },
    created() {
        this.getNews();
    },
    computed: {
        firstParagraph() {
            const content = this.news1.content || '';
            const firstParagraphMatch = content.match(/<p[^>]*>(.*?)<\/p>/i);
            return firstParagraphMatch ? firstParagraphMatch[0] : ''; // Return the first <p> tag's content or an empty string if not found
        },
        secondParagraph() {
            const content = this.news1.content || '';
            const secondParagraphMatch = content.match(/<p[^>]*>(.*?)<\/p>/i);
            return secondParagraphMatch ? secondParagraphMatch[0] : ''; // Return the first <p> tag's content or an empty string if not found
        },

        news1() {
            return this.NewsList[0];
        },
        news2() {
            return this.NewsList[1];
        },
        returnImgSrc() {
            return (image) => API_URL_CHAMBER + image.formats?.small?.url;
        },
        returnImgSrcLarge() {
            return (image) => API_URL_CHAMBER + image?.url;
        }
    },
    methods: {
        getFirstParagraph(content) {
            const firstParagraphMatch = content.match(/<p[^>]*>(.*?)<\/p>/i);
            return firstParagraphMatch ? firstParagraphMatch[0] : ''; // Return the first <p> tag's content or an empty string if not found
        },
        getNews() {
            this.loading = true;
            NewsService.GetList({ 'pagination[page]': 1, 'pagination[pageSize]': 3, populate: '*' })
                .then((res) => {
                    this.NewsList = res.data.data;
                })
                .finally(() => {
                    this.loading = false;
                });
        }
    }
};
</script>

<style lang="scss" scoped>
.news-content {
    font-size: 16px;
}
.news-card1 {
    display: flex;
    justify-content: center;
    border-radius: 8px;
    height: auto;
    border: 1px #d5d7e1 solid;
    padding: 1.3rem;
    margin: 1rem;
    cursor: pointer;
    transition: all 0.2s ease-in-out;
    &:hover {
        transition: all 0.2s ease-in-out;
        filter: contrast(70%);
    }

    &__footer {
        border-radius: 8px;
        color: #000;
        padding: 1.5rem;
    }

    &__title {
        color: #000;
        font-size: 18px;
        font-weight: 700;
        display: -webkit-box;
        max-width: 400px;
        -webkit-line-clamp: 2;
        -webkit-box-orient: vertical;
        overflow: hidden;
        text-overflow: ellipsis;
        line-height: normal;
    }
    &__date {
        color: #000;
        font-size: 14px;
        font-weight: 400;
        .newCard_btn {
            width: 24px;
            height: 24px;
        }
    }
    &__img {
        border-radius: 8px;
        width: 400px;
        height: 300px;
        object-fit: cover;
    }
}
.news-card2 {
    display: grid;
    grid-template-rows: row;
    grid-gap: 10px;
    align-content: space-between;
    border-radius: 8px;
    padding: 1.3rem;
    // margin: 1rem;
    transition: all 0.2s ease-in-out;
    border: 1px #d5d7e1 solid;
    &__title {
        font-size: 18px;
        font-weight: 600;
        line-height: 25px;
        overflow: hidden;
        text-overflow: ellipsis;
        display: -webkit-box;
        // max-width: 400px;
        -webkit-line-clamp: 2;
        -webkit-box-orient: vertical;
        color: #080f18;
        text-decoration: none;
        font-style: normal;
        transition: 0.1s ease-out;
        &:hover {
            color: var(--primary);
            transition: 0.1s ease-out;
        }
    }
    &__content {
        overflow: hidden;
        text-overflow: ellipsis;
        display: -webkit-box;
        max-width: 400px;
        -webkit-line-clamp: 4;
        -webkit-box-orient: vertical;
        color: #646464;
        font-size: 14px;
        font-style: normal;
        font-weight: 400;
        line-height: 22px;
    }
    &__date {
        color: #646464;
        font-size: 14px;
        font-weight: 400;
    }

    &__img {
        width: 100%;
        height: 300px;
        object-fit: cover;
        border-radius: 15px;
        background-size: 100%;
        transition: all 0.2s ease-in-out;
    }
}

.newWarapper {
    padding-right: 0;
    padding-left: 0;
}

.neewrapper2 {
    display: grid;

    grid-template-columns: repeat(4, 1fr);
    grid-gap: 24px;
    margin-bottom: 24px;
}

// Extra small devices (portrait phones, less than 576px)
@media (max-width: 576px) {
    .newWarapper {
        margin-bottom: 1rem;
    }
    .news-card1 {
        display: flex;
        border: none;
        flex-direction: column;
        padding: 0.7rem;
        margin: 0rem;
        &__footer {
            padding: 0.5rem;
        }

        &__title {
            min-width: 250px;
            overflow: hidden;
        }

        &__date {
            .newCard_btn {
                width: 15px;
                height: 18px;
            }
        }

        &__img {
            width: 100%;
        }
    }

    .neewrapper2 {
        display: none;
    }
}

// Small devices (landscape phones, 576px and up)
@media (min-width: 576px) and (max-width: 767.98px) {
    .newWarapper {
        margin-bottom: 1rem;
    }

    .news-card1 {
        display: flex;
        border: none;
        flex-direction: column;
        padding: 1rem;
        margin: 0.4rem;
        &__footer {
            padding: 1rem;
        }

        &__title {
            max-width: 350px;
            overflow: hidden;
        }

        &__date {
            .newCard_btn {
                width: 15px;
                height: 20px;
            }
        }

        &__img {
            width: 100%;
        }
    }

    .news-card2 {
        grid-gap: 8px;
        padding: 1rem;
        margin: 0;
        &__title {
            line-height: 22px;
            overflow: hidden;
        }

        &__img {
            display: none;
        }
    }
    .neewrapper2 {
        display: none;
    }
}

// Medium devices (tablets, 768px and up)
@media (min-width: 768px) and (max-width: 1024px) {
    .newWarapper {
        margin-bottom: 1rem;
    }

    .news-card1 {
        display: flex;
        flex-direction: column;
        padding: 1rem;
        margin: 0.5rem;
        &__footer {
            padding: 1rem;
        }

        &__title {
            max-width: 350px;
            overflow: hidden;
        }

        &__date {
            .newCard_btn {
                width: 15px;
                height: 20px;
            }
        }

        &__img {
            width: 100%;
        }
    }

    .news-card2 {
        grid-gap: 8px;
        padding: 1rem;
        mar &__title {
            line-height: 22px;
            overflow: hidden;
        }

        &__img {
            width: 100%;
            display: none;
        }
    }
}

@media (min-width: 1024px) and (max-width: 1520px) {
    .news-card1 {
        display: flex;
        flex-direction: column;
        padding: 1rem;
        margin: 0.8rem;
        &__footer {
            padding: 1rem;
        }

        &__title {
            max-width: 350px;
            overflow: hidden;
        }

        &__date {
            .newCard_btn {
                width: 15px;
                height: 20px;
            }
        }

        &__img {
            width: 100%;
        }
    }

    .neewrapper2 {
        display: grid;
        grid-template-columns: repeat(2, 1fr);
        grid-gap: 24px;
        margin-bottom: 24px;
    }

    .news-card2 {
        grid-gap: 8px;
        padding: 1rem;
        mar &__title {
            line-height: 22px;
            overflow: hidden;
        }

        &__img {
            width: 100%;
        }
    }
}
@media (min-width: 1520px) and (max-width: 1696px) {
    .news-card1 {
        padding: 1rem;
        margin: 0.8rem;
        &__footer {
            padding: 1rem;
        }

        &__title {
            max-width: 350px;
            overflow: hidden;
        }

        &__date {
            color: #000;
            font-size: 14px;
            font-weight: 400;
            .newCard_btn {
                width: 20px;
                height: 20px;
            }
        }

        &__img {
            border-radius: 8px;
            width: 350px;
            height: 250px;
            object-fit: cover;
        }
    }
}
</style>
