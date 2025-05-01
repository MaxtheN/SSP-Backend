<template>
    <div class="my-register-page">
        <div class="my-container">
            <b-row class="mb-2">
                <b-col>
                    <h1 @click="$router.push('cabinet')" style="cursor: pointer"><b-icon-chevron-left></b-icon-chevron-left>{{ $t('videolessons') }}</h1>
                </b-col>
            </b-row>
            <div>
                <b-tabs pills card vertical>
                    <b-tab v-for="(item, index) in VideoCategoryList" :key="index" :title="item.text" @click="Refresh(item)"
                        ><b-card-text>
                            <b-row>
                                <b-col sm="12" lg="6" v-for="(item2, index2) in videolesson" :key="index2">
                                    <LazyYoutube :src="item2.uri" />

                                    <p class="text-center">{{ item2.theme }}</p>
                                </b-col>
                            </b-row>
                        </b-card-text></b-tab
                    >
                </b-tabs>
            </div>
        </div>
    </div>
</template>

<script>
import VideoLessonService from '@/services/videolesson.service';
import VideoCategoryService from '@/services/videocategory.service';
import { LazyYoutube, LazyVimeo } from 'vue-lazytube';

export default {
    components: {
        LazyYoutube,
        LazyVimeo
    },
    data() {
        return {
            videolesson: [],
            VideoCategoryList: [],
            filter: {
                page: 1,
                pageSize: 20,
                totalRows: 1,
                options: [
                    {
                        value: 10,
                        text: '10'
                    },
                    {
                        value: 20,
                        text: '20'
                    },
                    {
                        value: 50,
                        text: '50'
                    },
                    {
                        value: 100,
                        text: '100'
                    }
                ]
            }
        };
    },
    created() {
        VideoCategoryService.GetAsSelectList().then((res) => {
            this.VideoCategoryList = res.data;
            this.Refresh({ value: this.VideoCategoryList[0].value });
        });
    },
    methods: {
        Refresh(item) {
            VideoLessonService.GetList({
                filters: {
                    categoryId: {
                        value: item.value,
                        matchMode: 'equals'
                    }
                },
                search: '',
                sortBy: '',
                orderType: 'asc',
                page: 1,
                pageSize: 100
            })
                .then((res) => {
                    this.videolesson = res.data.rows;
                    this.filter.totalRows = res.data.total;
                })
                .catch((error) => {
                    this.Loading = false;
                    this.makeToast(error.response.data);
                });
        },
        GetComplaint(item) {
            this.$router.push({ name: 'EditComplaint', params: { id: item.id } });
        },
        Get(item) {
            this.$router.push({ name: 'RequestView', params: { id: item.id } });
        },
        GetIBContractor(item) {
            this.$router.push({
                name: 'InspectionBookOfContractorEdit',
                params: { id: item.id },
                query: { fromRequest: true }
            });
        }
    }
};
</script>

<style>
.nav-link {
    color: black !important;
}
</style>
