<template>
    <MainCard :title="$t('contractorRating')" class="contractor-rating">
        <div v-if="loading" class="p-3 w-100 gap-4 d-flex flex-column">
            <b-skeleton-img no-aspect height="150px"></b-skeleton-img>
            <b-skeleton width="85%"></b-skeleton>
            <b-skeleton width="55%"></b-skeleton>
            <b-skeleton width="70%"></b-skeleton>
        </div>

        <div v-else-if="createries" class="gap-4 d-flex flex-column p-3 w-100">
            <div class="d-flex font-weight-medium">
                <span class="text-black">{{ $t('contractorInn') }}: </span>
                <span class="text-primary"> &nbsp; {{ createries.tin }} | {{ createries.name }}</span>
            </div>

            <div class="d-flex gap-2 align-items-center">
                <div class="contractor-rating__avatar">
                    <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none">
                        <path d="M22 6.5H16" stroke="#292D32" stroke-width="1.5" stroke-miterlimit="10" stroke-linecap="round" stroke-linejoin="round" />
                        <path d="M6 6.5H2" stroke="#292D32" stroke-width="1.5" stroke-miterlimit="10" stroke-linecap="round" stroke-linejoin="round" />
                        <path
                            d="M10 10C11.933 10 13.5 8.433 13.5 6.5C13.5 4.567 11.933 3 10 3C8.067 3 6.5 4.567 6.5 6.5C6.5 8.433 8.067 10 10 10Z"
                            stroke="#292D32"
                            stroke-width="1.5"
                            stroke-miterlimit="10"
                            stroke-linecap="round"
                            stroke-linejoin="round"
                        />
                        <path d="M22 17.5H18" stroke="#292D32" stroke-width="1.5" stroke-miterlimit="10" stroke-linecap="round" stroke-linejoin="round" />
                        <path d="M8 17.5H2" stroke="#292D32" stroke-width="1.5" stroke-miterlimit="10" stroke-linecap="round" stroke-linejoin="round" />
                        <path
                            d="M14 21C15.933 21 17.5 19.433 17.5 17.5C17.5 15.567 15.933 14 14 14C12.067 14 10.5 15.567 10.5 17.5C10.5 19.433 12.067 21 14 21Z"
                            stroke="#292D32"
                            stroke-width="1.5"
                            stroke-miterlimit="10"
                            stroke-linecap="round"
                            stroke-linejoin="round"
                        />
                    </svg>
                </div>

                <div class="text-grey2">{{ $t("rating.faoliyat_turi") }}:</div>
                <div class="text-black font-weight-medium">{{ createries.okedDetail?.name }}</div>
            </div>

            <div class="d-flex gap-2 align-items-center">
                <div class="contractor-rating__avatar">
                    <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none">
                        <path
                            d="M8.00001 22H16C20.02 22 20.74 20.39 20.95 18.43L21.7 10.43C21.97 7.99 21.27 6 17 6H7.00001C2.73001 6 2.03001 7.99 2.30001 10.43L3.05001 18.43C3.26001 20.39 3.98001 22 8.00001 22Z"
                            stroke="#292D32"
                            stroke-width="1.5"
                            stroke-miterlimit="10"
                            stroke-linecap="round"
                            stroke-linejoin="round"
                        />
                        <path
                            d="M8 6V5.2C8 3.43 8 2 11.2 2H12.8C16 2 16 3.43 16 5.2V6"
                            stroke="#292D32"
                            stroke-width="1.5"
                            stroke-miterlimit="10"
                            stroke-linecap="round"
                            stroke-linejoin="round"
                        />
                        <path
                            d="M14 13V14C14 14.01 14 14.01 14 14.02C14 15.11 13.99 16 12 16C10.02 16 10 15.12 10 14.03V13C10 12 10 12 11 12H13C14 12 14 12 14 13Z"
                            stroke="#292D32"
                            stroke-width="1.5"
                            stroke-miterlimit="10"
                            stroke-linecap="round"
                            stroke-linejoin="round"
                        />
                        <path
                            d="M21.65 11C19.34 12.68 16.7 13.68 14 14.02"
                            stroke="#292D32"
                            stroke-width="1.5"
                            stroke-miterlimit="10"
                            stroke-linecap="round"
                            stroke-linejoin="round"
                        />
                        <path
                            d="M2.62 11.27C4.87 12.81 7.41 13.74 10 14.03"
                            stroke="#292D32"
                            stroke-width="1.5"
                            stroke-miterlimit="10"
                            stroke-linecap="round"
                            stroke-linejoin="round"
                        />
                    </svg>
                </div>

                <div class="text-grey2">{{ $t("rating.xizmat_soliq_organi") }}:</div>
                <div class="text-black font-weight-medium">{{ createries.taxpayername }}</div>
            </div>

            <div class="d-flex gap-2 align-items-center">
                <div class="contractor-rating__avatar">
                    <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none">
                        <path
                            d="M8.67 14H4C2.9 14 2 14.9 2 16V22H8.67V14Z"
                            stroke="#292D32"
                            stroke-width="1.5"
                            stroke-miterlimit="10"
                            stroke-linecap="round"
                            stroke-linejoin="round"
                        />
                        <path
                            d="M13.33 10H10.66C9.55997 10 8.65997 10.9 8.65997 12V22H15.33V12C15.33 10.9 14.44 10 13.33 10Z"
                            stroke="#292D32"
                            stroke-width="1.5"
                            stroke-miterlimit="10"
                            stroke-linecap="round"
                            stroke-linejoin="round"
                        />
                        <path
                            d="M20 17H15.33V22H22V19C22 17.9 21.1 17 20 17Z"
                            stroke="#292D32"
                            stroke-width="1.5"
                            stroke-miterlimit="10"
                            stroke-linecap="round"
                            stroke-linejoin="round"
                        />
                        <path
                            d="M12.52 2.07001L13.05 3.13C13.12 3.28 13.31 3.42 13.47 3.44L14.43 3.60001C15.04 3.70001 15.19 4.14999 14.75 4.57999L14 5.32999C13.87 5.45999 13.8 5.69999 13.84 5.86999L14.05 6.79001C14.22 7.52001 13.83 7.80001 13.19 7.42001L12.29 6.89001C12.13 6.79001 11.86 6.79001 11.7 6.89001L10.8 7.42001C10.16 7.80001 9.76998 7.52001 9.93998 6.79001L10.15 5.86999C10.19 5.69999 10.12 5.44999 9.98999 5.32999L9.24999 4.59C8.80999 4.15 8.94999 3.70998 9.56999 3.60998L10.53 3.45001C10.69 3.42001 10.88 3.28001 10.95 3.14001L11.48 2.07999C11.77 1.49999 12.23 1.50001 12.52 2.07001Z"
                            stroke="#292D32"
                            stroke-width="1.5"
                            stroke-linecap="round"
                            stroke-linejoin="round"
                        />
                    </svg>
                </div>

                <div class="text-grey2">{{ $t("rating.taxpayer") }}:</div>
                <div class="text-black font-weight-medium">{{ createries.type }}</div>
            </div>

            <div class="d-flex gap-2 align-items-center">
                <div class="contractor-rating__avatar">
                    <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none">
                        <path
                            d="M4.26001 11.02V15.99C4.26001 17.81 4.26001 17.81 5.98001 18.97L10.71 21.7C11.42 22.11 12.58 22.11 13.29 21.7L18.02 18.97C19.74 17.81 19.74 17.81 19.74 15.99V11.02C19.74 9.2 19.74 9.2 18.02 8.04L13.29 5.31C12.58 4.9 11.42 4.9 10.71 5.31L5.98001 8.04C4.26001 9.2 4.26001 9.2 4.26001 11.02Z"
                            stroke="#292D32"
                            stroke-width="1.5"
                            stroke-linecap="round"
                            stroke-linejoin="round"
                        />
                        <path d="M17.5 7.63V5C17.5 3 16.5 2 14.5 2H9.5C7.5 2 6.5 3 6.5 5V7.56" stroke="#292D32" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round" />
                        <path
                            d="M12.63 10.99L13.2 11.88C13.29 12.02 13.49 12.16 13.64 12.2L14.66 12.46C15.29 12.62 15.46 13.16 15.05 13.66L14.38 14.47C14.28 14.6 14.2 14.83 14.21 14.99L14.27 16.04C14.31 16.69 13.85 17.02 13.25 16.78L12.27 16.39C12.12 16.33 11.87 16.33 11.72 16.39L10.74 16.78C10.14 17.02 9.68002 16.68 9.72002 16.04L9.78002 14.99C9.79002 14.83 9.71002 14.59 9.61002 14.47L8.94002 13.66C8.53002 13.16 8.70002 12.62 9.33002 12.46L10.35 12.2C10.51 12.16 10.71 12.01 10.79 11.88L11.36 10.99C11.72 10.45 12.28 10.45 12.63 10.99Z"
                            stroke="#292D32"
                            stroke-width="1.5"
                            stroke-linecap="round"
                            stroke-linejoin="round"
                        />
                    </svg>
                </div>

                <div class="text-grey2">{{ $t("rating.ball") }}:</div>
                <div class="text-black font-weight-medium">{{ createries.criteriaAll }}</div>
            </div>
        </div>

        <template #actions>
            <!-- <DownloadButton /> -->
        </template>
    </MainCard>
</template>

<script>
import MainCard from '@/components/cabinet/MainCard.vue';
import DownloadButton from '@/components/cabinet/DownloadButton.vue';
import SoliqIntegrationService from '@/services/soliqintegration.service';

export default {
    components: {
        MainCard,
        DownloadButton
    },
    data() {
        return {
            loading: false,
            createries: null
        };
    },
    created() {
        this.GetCompanyCriteries();
    },
    methods: {
        GetCompanyCriteries() {
            this.loading = true;
            SoliqIntegrationService.GetCompanyCriteries()
                .then((res) => {
                    this.createries = res.data;
                })
                .catch(this.showApiError)
                .finally(() => {
                    this.loading = false;
                });
        }
    }
};
</script>

<style lang="scss">
.contractor-rating {
    &__avatar {
        display: flex;
        padding: 8px;
        justify-content: center;
        align-items: center;
        gap: 10px;
        border-radius: 6px;
        background: #e5ecf2;
    }
}
</style>
