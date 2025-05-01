<template>
    <div class="w-card" v-on="$listeners" v-bind="$attrs">
        <div class="w-card__inner">
            <b-img class="img" :src="imgSrc" :alt="$t(title)" />
            <b-skeleton v-if="loading" class="loader"></b-skeleton>
            <p class="count">
                {{ count }}
            </p>
        </div>
        <div class="w-card__title">
            {{ $t(title) }}
        </div>
        <div class="w-card__badge" v-if="badge">{{ badge }}</div>
    </div>
</template>

<script>
import ApiService from '@/services/api.service';
import { BSkeleton } from 'bootstrap-vue';
export default {
    comments: {
        BSkeleton
    },
    props: {
        title: {
            type: String,
            default: ''
        },
        imgSrc: {
            type: String,
            default: '/images/ssp_images/contract-button.svg'
        },
        badge: {
            type: Number,
            default: null
        },
        countUrl: {
            type: String,
            default: '',
            required: false
        }
    },
    created() {
        if (this.$props.countUrl) {
            this.GetCount();
        }
    },
    data() {
        return {
            count: '',
            loading: false
        };
    },
    methods: {
        GetCount() {
            this.loading = true;
            ApiService.post(`${this.countUrl}`)
                .then((res) => {
                    this.count = res.data;
                })
                .catch((err) => {})
                .finally(() => {
                    this.loading = false;
                });
        }
    }
};
</script>

<style lang="scss">
.loader {
    width: 20px;
    height: 20px;
}
.w-card {
    display: flex;
    padding: 24px;
    flex-direction: column;
    justify-content: center;
    align-items: flex-start;
    gap: 16px;
    flex: 1 0 0;
    border-radius: 8px;
    border: 1px solid #d5d7e1;
    background: var(--Others-White, #fff);
    overflow: hidden;

    &:hover {
        box-shadow: 5px 0px 25px 0px rgba(0, 0, 0, 0.1);
        transition: 0.2s ease-in-out all;
        cursor: pointer;
    }
    &__inner {
        display: flex;
        gap: 1rem;
        align-items: center;
        .img {
            display: flex;
            padding: 16px;
            align-items: flex-start;
            gap: 10px;
            border-radius: 100%;
            background: var(--Greyscale-100, #f8f8f8);
        }
        .count {
            font-size: 40px;
            margin: 0;
            font-weight: bold;
        }
    }

    &__title {
        max-width: -webkit-fill-available;
        overflow: hidden;
        color: var(--Black, #000107);
        text-overflow: ellipsis;
        white-space: nowrap;
        font-size: 18px;
        font-style: normal;
        font-weight: 500;
        line-height: 150%; /* 27px */
    }
    &__badge {
        border-radius: 24px;
        background: var(--color-red-300, #f56c6c);
        display: inline-flex;
        padding: 1px 6.5px;
        flex-direction: column;
        align-items: center;
        color: var(--white);
        font-family: Noto Sans SC;
        font-size: 12px;
        font-style: normal;
        font-weight: 400;
        line-height: 18px;
        margin-left: 1rem;
    }
}
</style>
