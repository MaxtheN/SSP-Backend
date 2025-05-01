<template>
    <div class="col-lg-3 col-md-6 d-flex flex-column col-12 gap-5">
        <div class="base-card d-flex flex-column justify-content-between" style="min-height: 158px">
            <Vacancyskeleton v-if="loading" />
            <template v-else>
                <div class="d-flex justify-content-between align-items-center">
                    <div class="text-black font-weight-regular">{{ $t('main.newVacanciesCount') }}</div>
                    <svg width="24" height="24" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                        <path
                            fill-rule="evenodd"
                            clip-rule="evenodd"
                            d="M8.05544 2.05546C8.57117 1.53973 9.27064 1.25 9.99998 1.25H14C14.7293 1.25 15.4288 1.53973 15.9445 2.05546C16.4603 2.57118 16.75 3.27065 16.75 4V5.25H19C19.7293 5.25 20.4288 5.53973 20.9445 6.05546C21.4603 6.57118 21.75 7.27065 21.75 8V13.2375C21.7502 13.249 21.7502 13.2605 21.75 13.272V18C21.75 18.7293 21.4603 19.4288 20.9445 19.9445C20.4288 20.4603 19.7293 20.75 19 20.75H4.99998C4.27064 20.75 3.57117 20.4603 3.05544 19.9445C2.53972 19.4288 2.24998 18.7293 2.24998 18V13.2719C2.24972 13.2605 2.24972 13.249 2.24998 13.2376V8C2.24998 7.27065 2.53972 6.57118 3.05544 6.05546C3.57117 5.53973 4.27064 5.25 4.99998 5.25H7.24998V4C7.24998 3.27065 7.53972 2.57118 8.05544 2.05546ZM8.74998 5.25H15.25V4C15.25 3.66848 15.1183 3.35054 14.8839 3.11612C14.6494 2.8817 14.3315 2.75 14 2.75H9.99998C9.66846 2.75 9.35052 2.8817 9.1161 3.11612C8.88168 3.35054 8.74998 3.66848 8.74998 4V5.25ZM4.99998 6.75C4.66846 6.75 4.35052 6.8817 4.1161 7.11612C3.88168 7.35054 3.74998 7.66848 3.74998 8V12.7435C6.31364 13.7166 9.09323 14.25 12 14.25C14.8198 14.2534 17.6161 13.7424 20.25 12.7434V8C20.25 7.66848 20.1183 7.35054 19.8839 7.11612C19.6494 6.8817 19.3315 6.75 19 6.75H4.99998ZM12 15.75C14.8128 15.7533 17.6028 15.2758 20.25 14.3407L12 15.75ZM20.25 14.3407V18C20.25 18.3315 20.1183 18.6495 19.8839 18.8839C19.6494 19.1183 19.3315 19.25 19 19.25H4.99998C4.66846 19.25 4.35052 19.1183 4.1161 18.8839C3.88168 18.6495 3.74998 18.3315 3.74998 18V14.341C6.33136 15.2533 9.10811 15.7499 12 15.75M11.25 12C11.25 11.5858 11.5858 11.25 12 11.25H12.01C12.4242 11.25 12.76 11.5858 12.76 12C12.76 12.4142 12.4242 12.75 12.01 12.75H12C11.5858 12.75 11.25 12.4142 11.25 12Z"
                            fill="#6A6D7D"
                        />
                    </svg>
                </div>

                <div class="text-black-2 font-weight-bold h2">{{ vacancies.newVacanciesCount || 0 }}</div>
            </template>
        </div>

        <div class="base-card d-flex flex-column justify-content-between" style="min-height: 158px">
            <Vacancyskeleton v-if="loading" />
            <template v-else>
                <div class="d-flex justify-content-between align-items-center">
                    <div class="text-black font-weight-regular">{{ $t('main.reportVacanciesCount') }}</div>
                    <svg width="24" height="24" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                        <path
                            d="M16.7 18.98H7.29999C6.87999 18.98 6.40999 18.65 6.26999 18.25L2.12999 6.67C1.53999 5.01 2.22999 4.5 3.64999 5.52L7.54999 8.31C8.19999 8.76 8.93999 8.53 9.21999 7.8L10.98 3.11C11.54 1.61 12.47 1.61 13.03 3.11L14.79 7.8C15.07 8.53 15.81 8.76 16.45 8.31L20.11 5.7C21.67 4.58 22.42 5.15 21.78 6.96L17.74 18.27C17.59 18.65 17.12 18.98 16.7 18.98Z"
                            stroke="#6A6D7D"
                            stroke-width="1.5"
                            stroke-linecap="round"
                            stroke-linejoin="round"
                        />
                        <path d="M6.5 22H17.5" stroke="#6A6D7D" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round" />
                        <path d="M9.5 14H14.5" stroke="#6A6D7D" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round" />
                    </svg>
                </div>

                <div class="text-black-2 font-weight-bold h2">{{ vacancies.reportVacanciesCount || 0 }}</div>
            </template>
        </div>
    </div>
</template>

<script>
import ApplicationService from '@/services/application.service';
import Vacancyskeleton from '@/components/cabinet/main/Vacancyskeleton.vue';

export default {
    components: {
        Vacancyskeleton
    },
    data() {
        return {
            loading: false,
            vacancies: []
        };
    },
    created() {
        this.GetCompanyCriteries();
    },
    methods: {
        GetCompanyCriteries() {
            this.loading = true;
            ApplicationService.GetApplicationVacancies()
                .then((res) => {
                    this.vacancies = res.data || {};
                })
                .catch(this.showApiError)
                .finally(() => {
                    this.loading = false;
                });
        }
    }
};
</script>
