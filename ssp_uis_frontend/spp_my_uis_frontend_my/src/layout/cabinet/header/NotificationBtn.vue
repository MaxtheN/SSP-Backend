<template>
    <b-dropdown no-caret text="Bildirishnomalar" class="noti-icon border-0" right ref="dropdown" variant="light" @show="backdropToggle" @hide="backdropToggle">
        <template #button-content>
            <svg :class="{ 'notification-bounce': notifications.length }" xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none">
                <path
                    fill-rule="evenodd"
                    clip-rule="evenodd"
                    d="M12 3.75C11.6685 3.75 11.3505 3.8817 11.1161 4.11612C10.8848 4.34743 10.7535 4.66008 10.7501 4.98684C10.7526 5.13381 10.7116 5.27814 10.6337 5.40135C10.5773 5.49051 10.5017 5.5686 10.4092 5.62862C10.3766 5.6499 10.3422 5.66871 10.3064 5.68479C9.28714 6.16987 8.41823 6.92198 7.792 7.86132C7.16601 8.80031 6.80599 9.89117 6.75001 11.0181V14C6.75001 14.0301 6.74819 14.0602 6.74457 14.0901C6.65521 14.8284 6.39374 15.5354 5.98123 16.1542C5.95974 16.1864 5.93787 16.2183 5.91563 16.25H18.0844C18.0621 16.2183 18.0403 16.1864 18.0188 16.1542C17.6063 15.5354 17.3448 14.8284 17.2554 14.0901C17.2518 14.0602 17.25 14.0301 17.25 14V11.0181C17.194 9.89117 16.834 8.80031 16.208 7.86132C15.5789 6.9176 14.7048 6.16287 13.6794 5.67802C13.4172 5.55405 13.25 5.29002 13.25 5C13.25 4.66848 13.1183 4.35054 12.8839 4.11612C12.6495 3.8817 12.3315 3.75 12 3.75ZM9.2925 4.51836C9.39043 3.96791 9.65472 3.45621 10.0555 3.05546C10.5712 2.53973 11.2707 2.25 12 2.25C12.7294 2.25 13.4288 2.53973 13.9446 3.05546C14.3453 3.45621 14.6096 3.96791 14.7075 4.51836C15.8122 5.11853 16.7563 5.97962 17.4561 7.02927C18.2362 8.19949 18.6832 9.55995 18.7492 10.9648C18.7497 10.9765 18.75 10.9883 18.75 11V13.9524C18.8159 14.4422 18.9925 14.9106 19.2669 15.3221C19.5491 15.7455 19.9268 16.0966 20.3696 16.3474C20.666 16.5152 20.812 16.8617 20.7253 17.1911C20.6385 17.5205 20.3406 17.75 20 17.75H4.00001C3.65939 17.75 3.36154 17.5205 3.27476 17.1911C3.18798 16.8617 3.33405 16.5152 3.63044 16.3474C4.07321 16.0966 4.45091 15.7455 4.73316 15.3221C5.00747 14.9106 5.18416 14.4422 5.25001 13.9524V11C5.25001 10.9883 5.25028 10.9765 5.25083 10.9648C5.31677 9.55995 5.76377 8.19949 6.54392 7.02927C7.24369 5.97962 8.18783 5.11852 9.2925 4.51836Z"
                    fill="#6A6D7D"
                />
                <path
                    fill-rule="evenodd"
                    clip-rule="evenodd"
                    d="M9.00001 16.25C9.41422 16.25 9.75001 16.5858 9.75001 17V18C9.75001 18.5967 9.98706 19.169 10.409 19.591C10.831 20.0129 11.4033 20.25 12 20.25C12.5967 20.25 13.169 20.0129 13.591 19.591C14.013 19.169 14.25 18.5967 14.25 18V17C14.25 16.5858 14.5858 16.25 15 16.25C15.4142 16.25 15.75 16.5858 15.75 17V18C15.75 18.9946 15.3549 19.9484 14.6517 20.6517C13.9484 21.3549 12.9946 21.75 12 21.75C11.0054 21.75 10.0516 21.3549 9.34836 20.6517C8.64509 19.9484 8.25001 18.9946 8.25001 18V17C8.25001 16.5858 8.58579 16.25 9.00001 16.25Z"
                    fill="#6A6D7D"
                />
            </svg>

            <span v-if="notifications.length" class="badge bg-danger rounded-pill">{{ notifications.length }}</span>
        </template>

        <div class="d-flex align-items-center px-2 py-1">
            <div class="col h6">
                {{ $t('notification') }}
            </div>
            <b-button variant="light" size="sm" class="border-0" @click="$refs.dropdown.hide()">
                <svg width="24" height="25" viewBox="0 0 24 25" fill="none" xmlns="http://www.w3.org/2000/svg">
                    <path
                        fill-rule="evenodd"
                        clip-rule="evenodd"
                        d="M5.29289 5.79289C5.68342 5.40237 6.31658 5.40237 6.70711 5.79289L12 11.0858L17.2929 5.79289C17.6834 5.40237 18.3166 5.40237 18.7071 5.79289C19.0976 6.18342 19.0976 6.81658 18.7071 7.20711L13.4142 12.5L18.7071 17.7929C19.0976 18.1834 19.0976 18.8166 18.7071 19.2071C18.3166 19.5976 17.6834 19.5976 17.2929 19.2071L12 13.9142L6.70711 19.2071C6.31658 19.5976 5.68342 19.5976 5.29289 19.2071C4.90237 18.8166 4.90237 18.1834 5.29289 17.7929L10.5858 12.5L5.29289 7.20711C4.90237 6.81658 4.90237 6.18342 5.29289 5.79289Z"
                        fill="#6A6D7D"
                    />
                </svg>
            </b-button>
        </div>

        <div v-if="notifications.length" style="max-height: 400px" class="overflow-auto">
            <b-dropdown-item v-for="(notification, index) in notifications" :key="index" @click="IsRead(notification.type, notification.id, notification.url)">
                <a :href="notification.url" target="_blank" class="text-decoration-none d-flex align-items-center gap-2 text-black">
                    <svg width="36" height="36" viewBox="0 0 36 36" fill="none" xmlns="http://www.w3.org/2000/svg">
                        <rect x="0.5" y="0.5" width="35" height="35" rx="5.5" fill="white" />
                        <rect x="0.5" y="0.5" width="35" height="35" rx="5.5" stroke="#D5D7E1" />
                        <path
                            fill-rule="evenodd"
                            clip-rule="evenodd"
                            d="M18 9.75C17.6685 9.75 17.3505 9.8817 17.1161 10.1161C16.8848 10.3474 16.7535 10.6601 16.7501 10.9868C16.7526 11.1338 16.7116 11.2781 16.6337 11.4013C16.5773 11.4905 16.5017 11.5686 16.4092 11.6286C16.3766 11.6499 16.3422 11.6687 16.3064 11.6848C15.2871 12.1699 14.4182 12.922 13.792 13.8613C13.166 14.8003 12.806 15.8912 12.75 17.0181V20C12.75 20.0301 12.7482 20.0602 12.7446 20.0901C12.6552 20.8284 12.3937 21.5354 11.9812 22.1542C11.9597 22.1864 11.9379 22.2183 11.9156 22.25H24.0844C24.0621 22.2183 24.0403 22.1864 24.0188 22.1542C23.6063 21.5354 23.3448 20.8284 23.2554 20.0901C23.2518 20.0602 23.25 20.0301 23.25 20V17.0181C23.194 15.8912 22.834 14.8003 22.208 13.8613C21.5789 12.9176 20.7048 12.1629 19.6794 11.678C19.4172 11.554 19.25 11.29 19.25 11C19.25 10.6685 19.1183 10.3505 18.8839 10.1161C18.6495 9.8817 18.3315 9.75 18 9.75ZM15.2925 10.5184C15.3904 9.96791 15.6547 9.45621 16.0555 9.05546C16.5712 8.53973 17.2707 8.25 18 8.25C18.7294 8.25 19.4288 8.53973 19.9446 9.05546C20.3453 9.45621 20.6096 9.96791 20.7075 10.5184C21.8122 11.1185 22.7563 11.9796 23.4561 13.0293C24.2362 14.1995 24.6832 15.56 24.7492 16.9648C24.7497 16.9765 24.75 16.9883 24.75 17V19.9524C24.8159 20.4422 24.9925 20.9106 25.2669 21.3221C25.5491 21.7455 25.9268 22.0966 26.3696 22.3474C26.666 22.5152 26.812 22.8617 26.7253 23.1911C26.6385 23.5205 26.3406 23.75 26 23.75H10C9.65939 23.75 9.36154 23.5205 9.27476 23.1911C9.18798 22.8617 9.33405 22.5152 9.63044 22.3474C10.0732 22.0966 10.4509 21.7455 10.7332 21.3221C11.0075 20.9106 11.1842 20.4422 11.25 19.9524V17C11.25 16.9883 11.2503 16.9765 11.2508 16.9648C11.3168 15.56 11.7638 14.1995 12.5439 13.0293C13.2437 11.9796 14.1878 11.1185 15.2925 10.5184Z"
                            fill="#6A6D7D"
                        />
                        <path
                            fill-rule="evenodd"
                            clip-rule="evenodd"
                            d="M15 22.25C15.4142 22.25 15.75 22.5858 15.75 23V24C15.75 24.5967 15.9871 25.169 16.409 25.591C16.831 26.0129 17.4033 26.25 18 26.25C18.5967 26.25 19.169 26.0129 19.591 25.591C20.013 25.169 20.25 24.5967 20.25 24V23C20.25 22.5858 20.5858 22.25 21 22.25C21.4142 22.25 21.75 22.5858 21.75 23V24C21.75 24.9946 21.3549 25.9484 20.6517 26.6517C19.9484 27.3549 18.9946 27.75 18 27.75C17.0054 27.75 16.0516 27.3549 15.3484 26.6517C14.6451 25.9484 14.25 24.9946 14.25 24V23C14.25 22.5858 14.5858 22.25 15 22.25Z"
                            fill="#6A6D7D"
                        />
                    </svg>
                    <span> {{ notification.message }}</span>
                </a>
            </b-dropdown-item>
        </div>
        <div v-else class="d-flex justify-content-center align-items-center flex-column gap-4 py-md-3 py-2" style="min-width: 350px">
            <b-img src="/images/no-notification.jpg" width="200" />
            <div class="text-black font-weight-regular h6 text-center">{{ $t('noNotifications') }}</div>
        </div>

        <div class="px-3 pt-2 d-flex justify-content-end">
            <b-button variant="info" @click="readAll" size="sm">
                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 16 16" fill="none">
                    <path
                        d="M2.40002 8.00026L5.06355 10.6669L10.4 5.3336M8.26355 10.6669L13.6 5.3336"
                        stroke="currentColor"
                        stroke-width="1.5"
                        stroke-linecap="round"
                        stroke-linejoin="round"
                    />
                </svg>
                {{ $t('readAll') }}
            </b-button>
        </div>
    </b-dropdown>
</template>
<script>
import ApplicationService from '@/services/application.service';

export default {
    components: {},
    data() {
        return {
            loading: false,
            notifications: []
        };
    },
    created() {
        this.GetApplicationNotification();
    },
    methods: {
        GetApplicationNotification() {
            this.loading = true;
            ApplicationService.GetApplicationNotification()
                .then((res) => {
                    this.notifications = res.data;
                })
                .catch(this.showApiError)
                .finally(() => {
                    this.loading = false;
                });
        },
        IsRead(type, id, url) {
            ApplicationService.IsRead(type, id).then((res) => {
                this.GetApplicationNotification();
                window.open(url, '_blank');
            });
        },
        readAll() {
            this.notifications.forEach((e) => {
                this.IsRead(e.type, e.id);
            });
        },
        clearNotifications() {
            notifications.value = [];
        },
        backdropToggle() {
            document.querySelector('#backdrop').classList.toggle('active');
        }
    }
};
</script>

<style lang="scss">
.noti-icon {
    .badge {
        position: absolute;
        top: -6px;
        right: -6px;
        color: #fff;
    }
    .dropdown-menu {
        margin-top: 17px;
        border-radius: 8px;
        border: 0;
    }
}

.notification-icon {
    font-size: 24px;
    color: #fff;
}

.notification-bounce {
    animation: tada 1.5s ease infinite;
}

@keyframes tada {
    0% {
        transform: scaleX(1);
    }

    10%,
    20% {
        transform: scale3d(0.95, 0.95, 0.95) rotate(-10deg);
    }

    30%,
    50%,
    70%,
    90% {
        transform: scaleX(1) rotate(10deg);
    }

    40%,
    60%,
    80% {
        transform: rotate(-10deg);
    }

    to {
        transform: scaleX(1);
    }
}
</style>
