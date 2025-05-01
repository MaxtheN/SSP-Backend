<template>
    <div>
        <b-dropdown no-caret text="Bildirishnomalar" class="noti-icon border-0" right ref="dropdown" variant="light" @show="backdropToggle" @hide="backdropToggle">
            <template #button-content>
                <b-img src="/images/ssp_images/Profile.svg" style="width: 30px; height: 24px" alt="shaxsiy kabinet" />
            </template>

            <b-dropdown-header>
                <div class="d-flex justify-content-between align-items-center">
                    <div class="d-flex align-items-center">
                        <b-avatar size="48" variant="light"> <b-img src="/images/ssp_images/Profile.svg" alt="shaxsiy kabinet" /> </b-avatar>
                        <div class="d-flex flex-column px-3">
                            <p class="p-0 m-0 font-weight-bold text-body">
                                {{ localStorageData.fullName }}
                            </p>
                        </div>
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
            </b-dropdown-header>

            <b-dropdown-item>
                <Navlink icon="person" label="Mening ma’lumotlarim" :to="{ name: 'info' }" />
            </b-dropdown-item>
            <b-dropdown-item>
                <Navlink icon="exclamation-circle" label="Ommaviy offerta" @click="openOferta" />
            </b-dropdown-item>
            <b-dropdown-divider></b-dropdown-divider>
            <div class="px-6 d-flex justify-content-start">
                <b-button variant="outline-danger" size="sm" @click="logout">
                    <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="none">
                        <path
                            d="M8.90002 7.56001C9.21002 3.96001 11.06 2.49001 15.11 2.49001H15.24C19.71 2.49001 21.5 4.28001 21.5 8.75001V15.27C21.5 19.74 19.71 21.53 15.24 21.53H15.11C11.09 21.53 9.24002 20.08 8.91002 16.54"
                            stroke="#FF4E54"
                            stroke-width="1.5"
                            stroke-linecap="round"
                            stroke-linejoin="round"
                        />
                        <path d="M15 12H3.62" stroke="#FF4E54" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round" />
                        <path d="M5.85 8.65L2.5 12L5.85 15.35" stroke="#FF4E54" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round" />
                    </svg>
                    {{ $t('logout') }}
                </b-button>
            </div>
        </b-dropdown>
        <oferta-modal v-model="ModalOferta" @closeOferta="closeModalOferta" />
    </div>
</template>

<script>
import Navlink from '../../../components/cabinet/Navlink.vue';
import AccountService from '@/services/account.service';
import ofertaModal from '../../../components/ofertaModal.vue';

export default {
    components: {
        Navlink,
        ofertaModal
    },
    data() {
        return {
            ModalOferta: false,
            localStorageData: JSON.parse(localStorage.getItem('user_info'))
        };
    },
    methods: {
        backdropToggle() {
            document.querySelector('#backdrop').classList.toggle('active');
        },
        closeModalOferta() {
            this.ModalOferta = false;
        },
        openOferta() {
            this.ModalOferta = true;
        },
        logout() {
            AccountService.Logout().then(() => {
                localStorage.clear();
                if (this.$route.name !== 'Home') {
                    this.$router.push('/');
                } else {
                    window.location.reload();
                }
            });
        }
    }
};
</script>
