<template>
    <div>
        <LoginNavbar />
        <div class="container-fluid pt-2 px-6">
            <b-row>
                <b-col cols="12" sm="12" md="6" lg="6" v-if="!isToken" class="p-0">
                    <div class="rounded-sm border border-border-color mr-0 mr-md-2">
                        <div class="my-sidebar-body">
                            <b-row>
                                <b-col>
                                    <p class="mobileEnter">{{ $t('enter') }}</p>
                                </b-col>
                            </b-row>
                            <b-row>
                                <b-col class="pb-sm-4 pb-md-5">
                                    <logincustomPhone
                                        :label="$t('phonenumber')"
                                        @keyup="ChangePhone"
                                        @keyup.native.enter="isCheckAccount"
                                        v-mask="'+998-##-###-##-##'"
                                        v-model="Account.phonenumber"
                                        placeholder="+998 _ _  _ _ _  _ _  _ _"
                                    ></logincustomPhone>
                                    <p class="text-info-sign">{{ $t('usephonenumber') }}</p>
                                </b-col>
                            </b-row>
                            <b-row v-if="filter.isPassword === null && filter.isSmsCode === false">
                                <b-col>
                                    <custom-button @click.native="isCheckAccount" block class="register-btn pt-2">
                                        <b-spinner v-if="checkAccountLoading" small style="margin-right: 8px"></b-spinner>
                                        {{ $t('register') }}
                                    </custom-button>
                                </b-col>
                            </b-row>
                            <b-row v-if="filter.isPassword === true" style="margin-bottom: 16px">
                                <b-col sm="12">
                                    <LogincustomInput :type="inputType" @keyup.native.enter="SignIn" v-model="Account.password" :label="$t('password')" placeholder="****">
                                        <template #right-icon>
                                            <b-icon-eye v-if="inputType == 'password'" @click="changeType" />
                                            <b-icon-eye-slash v-if="inputType == 'text'" @click="changeTypepassword" />
                                        </template>
                                    </LogincustomInput>
                                </b-col>
                                <b-col sm="12" class="mt-2 d-flex justify-content-end">
                                    <a @click="OpenForgotPass" class="cursor-pointer" style="color: #003d45; text-decoration: none">{{ $t('forgotpassword') }}</a>
                                </b-col>
                            </b-row>
                            <b-row v-if="filter.isSmsCode" style="margin-bottom: 16px">
                                <b-col sm="12"></b-col>
                                <b-col sm="12">
                                    <LogincustomInput v-mask="'####'" v-model="Account.smscode" :label="$t('smskod')" placeholder="1234"> </LogincustomInput>
                                </b-col>
                            </b-row>
                            <b-row v-if="filter.isPassword === true">
                                <b-col>
                                    <custom-button @click.native="SignIn" block class="register-btn">
                                        <b-spinner v-if="SignLoading" small style="margin-right: 8px"></b-spinner>
                                        {{ $t('auth') }}
                                    </custom-button>
                                </b-col>
                            </b-row>
                            <b-row v-if="filter.isPassword === false">
                                <b-col sm="12" style="margin-bottom: 16px" class="text-center">
                                    <span class="text-center" style="color: #ff9c00">{{ $t('goregister') }}</span>
                                </b-col>
                                <b-col sm="12" style="margin-bottom: 16px">
                                    <LogincustomInput :type="inputType" v-model="Account.password" :label="$t('password')" placeholder="******">
                                        <template #right-icon>
                                            <b-icon-eye v-if="inputType == 'password'" @click="changeType" />
                                            <b-icon-eye-slash v-if="inputType == 'text'" @click="changeTypepassword" />
                                        </template>
                                    </LogincustomInput>
                                </b-col>
                                <b-col sm="12" style="margin-bottom: 16px">
                                    <LogincustomInput :type="inputType" v-model="Account.passwordconfirm" :label="$t('confirmpassword')" placeholder="******">
                                        <template #right-icon>
                                            <b-icon-eye v-if="inputType == 'password'" @click="changeType" />
                                            <b-icon-eye-slash v-if="inputType == 'text'" @click="changeTypepassword" />
                                        </template>
                                    </LogincustomInput>
                                </b-col>
                                <b-col sm="12" v-if="filter.isSmsForRegister" style="margin-bottom: 16px" class="text-center">
                                    <span class="text-center text-danger">
                                        {{
                                            $t('lang') == 'Ру' ? $t('entersmscodesentnumber') + ' ' + Account.phonenumber : Account.phonenumber + ' ' + $t('entersmscodesentnumber')
                                        }}
                                    </span>
                                </b-col>
                                <b-col sm="12" v-if="filter.isSmsForRegister" style="margin-bottom: 16px">
                                    <LogincustomInput v-mask="'####'" v-model="Account.smscode" :label="$t('smskod')" placeholder="1234"></LogincustomInput>
                                </b-col>
                                <b-col sm="12" v-if="!filter.isSms">
                                    <custom-button @click.native="GiveSmsCode" block class="register-btn">
                                        <b-spinner v-if="SignLoading" small style="margin-right: 8px"></b-spinner>
                                        {{ $t('Smskodloish') }}
                                    </custom-button>
                                </b-col>
                                <b-col sm="12" v-if="filter.isSmsForRegister">
                                    <custom-button @click.native="SendSmsCode" block class="register-btn">
                                        <b-spinner v-if="SignLoading" small style="margin-right: 8px"></b-spinner>
                                        {{ $t('registration') }}
                                    </custom-button>
                                </b-col>
                            </b-row>
                            <b-row class="mt-3">
                                <b-col cols="12">
                                    <div class="divider">
                                        <span>{{ $t('Orloginwith') }}</span>
                                    </div>
                                </b-col>
                                <b-col sm="6" md="6" lg="6" xl="6" class="mb-4 mb-sm-0">
                                    <custom-button variant="white" block class="registeroneid-btn text-center p-0 m-0" @click.native="signByEImzoModal = true">
                                        <b-img class="registeroneid-btn_img" src="/images/ssp_images/eimzo.png" alt="eimzo login" />
                                    </custom-button>
                                    <LoginByEImzo v-if="signByEImzoModal" v-model="signByEImzoModal" />
                                </b-col>
                                <!-- oneid -->
                                <b-col sm="6" md="6" lg="6" xl="6">
                                    <custom-button class="registeroneid-btn" :disabled="oneIdLoading" variant="white" block @click.native="SignbyOneId()">
                                        <b-spinner v-if="oneIdLoading" small style="margin-right: 8px"></b-spinner>

                                        <b-img class="registeroneid-btn_img" src="/images/ssp_images/oneIdimg.png" alt="one id login" />
                                    </custom-button>
                                </b-col>
                            </b-row>
                        </div>
                    </div>
                </b-col>
                <b-col cols="12" sm="12" md="6" lg="6" class="p-0 mt-2 mt-sm-0">
                    <Swipper />
                </b-col>
            </b-row>

            <b-row class="appeal bg-white rounded-sm border border-border-color justify-content-center mt-2">
                <div class="d-flex justify-content-center text-center">
                    <h4 class="my-3 mainPagetitle mobileTaklif px-6">{{ $t('taklif2023') }}</h4>
                </div>

                <b-row class="my-3 justify-content-center">
                    <b-col lg="3" md="6" sm="12" class="my-3" style="cursor: pointer">
                        <a href="https://my.chamber.uz/ochiq_muloqot" style="text-decoration: none">
                            <b-card class="infoCards">
                                <div class="main-card-img">
                                    <svg xmlns="http://www.w3.org/2000/svg" width="50" height="50" viewBox="0 0 36 36" fill="none">
                                        <path
                                            fill-rule="evenodd"
                                            clip-rule="evenodd"
                                            d="M18 4.125C10.337 4.125 4.125 10.337 4.125 18C4.125 25.663 10.337 31.875 18 31.875C25.663 31.875 31.875 25.663 31.875 18C31.875 10.337 25.663 4.125 18 4.125ZM1.875 18C1.875 9.09441 9.09441 1.875 18 1.875C26.9056 1.875 34.125 9.09441 34.125 18C34.125 26.9056 26.9056 34.125 18 34.125C9.09441 34.125 1.875 26.9056 1.875 18Z"
                                            fill="#000107"
                                        />
                                        <path
                                            fill-rule="evenodd"
                                            clip-rule="evenodd"
                                            d="M10.875 4.5C10.875 3.87868 11.3787 3.375 12 3.375H13.5C13.8617 3.375 14.2014 3.54893 14.4128 3.84244C14.6242 4.13594 14.6817 4.5132 14.5671 4.8563C11.7193 13.385 11.7193 22.615 14.5671 31.1437C14.6817 31.4868 14.6242 31.8641 14.4128 32.1576C14.2014 32.4511 13.8617 32.625 13.5 32.625H12C11.3787 32.625 10.875 32.1213 10.875 31.5C10.875 30.8896 11.3612 30.3927 11.9675 30.3755C9.58583 22.2992 9.58583 13.7008 11.9675 5.62454C11.3612 5.60734 10.875 5.11045 10.875 4.5Z"
                                            fill="#000107"
                                        />
                                        <path
                                            fill-rule="evenodd"
                                            clip-rule="evenodd"
                                            d="M22.1437 3.43291C22.733 3.23613 23.3703 3.55436 23.5671 4.1437C26.5693 13.135 26.5693 22.865 23.5671 31.8563C23.3703 32.4456 22.733 32.7639 22.1437 32.5671C21.5544 32.3703 21.2361 31.733 21.4329 31.1437C24.2807 22.615 24.2807 13.385 21.4329 4.8563C21.2361 4.26697 21.5544 3.6297 22.1437 3.43291Z"
                                            fill="#000107"
                                        />
                                        <path
                                            fill-rule="evenodd"
                                            clip-rule="evenodd"
                                            d="M3.84244 21.5872C4.13594 21.3758 4.5132 21.3183 4.8563 21.4329C13.385 24.2807 22.615 24.2807 31.1437 21.4329C31.4868 21.3183 31.8641 21.3758 32.1576 21.5872C32.4511 21.7986 32.625 22.1383 32.625 22.5V24C32.625 24.6213 32.1213 25.125 31.5 25.125C30.8896 25.125 30.3927 24.6388 30.3755 24.0325C22.2992 26.4142 13.7008 26.4142 5.62454 24.0325C5.60734 24.6388 5.11045 25.125 4.5 25.125C3.87868 25.125 3.375 24.6213 3.375 24V22.5C3.375 22.1383 3.54893 21.7986 3.84244 21.5872Z"
                                            fill="#000107"
                                        />
                                        <path
                                            fill-rule="evenodd"
                                            clip-rule="evenodd"
                                            d="M31.1437 14.5671C22.615 11.7193 13.385 11.7193 4.8563 14.5671C4.26697 14.7639 3.6297 14.4456 3.43291 13.8563C3.23613 13.267 3.55436 12.6297 4.1437 12.4329C13.135 9.4307 22.865 9.4307 31.8563 12.4329C32.4456 12.6297 32.7639 13.267 32.5671 13.8563C32.3703 14.4456 31.733 14.7639 31.1437 14.5671Z"
                                            fill="#000107"
                                        />
                                    </svg>
                                    <h4 style="font-size: 18px; font-weight: bold">{{ $t('Veb') }}</h4>
                                </div>
                            </b-card>
                        </a>
                    </b-col>
                    <b-col lg="3" md="6" sm="12" class="my-3">
                        <a href="https://t.me/Taklif2023bot" style="text-decoration: none">
                            <b-card class="infoCards">
                                <div class="main-card-img">
                                    <svg xmlns="http://www.w3.org/2000/svg" width="50" height="50" viewBox="0 0 36 36" fill="none">
                                        <path
                                            d="M11.1 9.47999L23.835 5.23499C29.55 3.32999 32.655 6.44999 30.765 12.165L26.52 24.9C23.67 33.465 18.99 33.465 16.14 24.9L14.88 21.12L11.1 19.86C2.53502 17.01 2.53502 12.345 11.1 9.47999Z"
                                            stroke="#292D32"
                                            stroke-width="2"
                                            stroke-linecap="round"
                                            stroke-linejoin="round"
                                        />
                                        <path d="M15.165 20.4751L20.535 15.0901" stroke="#292D32" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" />
                                    </svg>
                                    <h4 style="font-size: 18px; font-weight: bold">{{ $t('Murojaatni') }}</h4>
                                </div>
                            </b-card>
                        </a>
                    </b-col>
                    <b-col lg="3" md="6" sm="12" class="my-3">
                        <b-card class="infoCards">
                            <div class="main-card-img">
                                <svg xmlns="http://www.w3.org/2000/svg" width="50" height="50" viewBox="0 0 36 36" fill="none">
                                    <path
                                        fill-rule="evenodd"
                                        clip-rule="evenodd"
                                        d="M7.5 7.125C7.00272 7.125 6.52581 7.32254 6.17417 7.67417C5.82976 8.01859 5.63317 8.48321 5.62525 8.96943C5.97251 14.5323 8.33894 19.7775 12.2807 23.7193C16.2225 27.6611 21.4677 30.0275 27.0306 30.3748C27.5168 30.3668 27.9814 30.1702 28.3258 29.8258C28.6775 29.4742 28.875 28.9973 28.875 28.5V23.2617L22.9717 20.9004L21.2147 23.8288C20.9134 24.3309 20.2775 24.5179 19.7524 24.259C16.273 22.543 13.457 19.727 11.741 16.2476C11.4821 15.7225 11.6691 15.0866 12.1712 14.7853L15.0996 13.0283L12.7383 7.125H7.5ZM4.58318 6.08318C5.35677 5.3096 6.40598 4.875 7.5 4.875H13.5C13.96 4.875 14.3737 5.15507 14.5445 5.58219L17.5445 13.0822C17.7496 13.5949 17.5523 14.1806 17.0788 14.4647L14.2471 16.1637C15.5911 18.4818 17.5182 20.4089 19.8363 21.7529L21.5353 18.9212C21.8194 18.4477 22.4051 18.2504 22.9178 18.4555L30.4178 21.4555C30.8449 21.6263 31.125 22.04 31.125 22.5V28.5C31.125 29.594 30.6904 30.6432 29.9168 31.4168C29.1432 32.1904 28.094 32.625 27 32.625C26.9772 32.625 26.9545 32.6243 26.9318 32.6229C20.8064 32.2507 15.029 29.6495 10.6897 25.3103C6.35046 20.971 3.74931 15.1936 3.37707 9.06824C3.37569 9.04552 3.375 9.02276 3.375 9C3.375 7.90598 3.8096 6.85677 4.58318 6.08318Z"
                                        fill="#111827"
                                    />
                                </svg>
                                <h4 style="font-size: 18px; font-weight: bold">{{ $t('telifonnn') }}</h4>
                            </div>
                        </b-card>
                    </b-col>
                    <b-col lg="3" md="6" sm="12" class="my-3" style="cursor: pointer">
                        <a href="https://my.chamber.uz/ochiq_muloqot" style="text-decoration: none">
                            <b-card class="infoCards">
                                <div class="main-card-img">
                                    <svg xmlns="http://www.w3.org/2000/svg" width="50" height="50" viewBox="0 0 36 36" fill="none">
                                        <path
                                            fill-rule="evenodd"
                                            clip-rule="evenodd"
                                            d="M12.0832 3.08318C12.8568 2.3096 13.906 1.875 15.0001 1.875H21.0001C22.0941 1.875 23.1433 2.3096 23.9169 3.08318C24.6905 3.85677 25.1251 4.90598 25.1251 6V7.875H28.5001C29.5941 7.875 30.6433 8.3096 31.4169 9.08318C32.1905 9.85677 32.6251 10.906 32.6251 12V19.8563C32.6255 19.8736 32.6255 19.8908 32.6251 19.9079V27C32.6251 28.094 32.1905 29.1432 31.4169 29.9168C30.6433 30.6904 29.5941 31.125 28.5001 31.125H7.50005C6.40604 31.125 5.35683 30.6904 4.58324 29.9168C3.80965 29.1432 3.37505 28.094 3.37505 27V19.9079C3.37466 19.8908 3.37466 19.8736 3.37505 19.8563V12C3.37505 10.906 3.80965 9.85677 4.58324 9.08318C5.35683 8.3096 6.40604 7.875 7.50005 7.875H10.8751V6C10.8751 4.90598 11.3097 3.85677 12.0832 3.08318ZM13.1251 7.875H22.8751V6C22.8751 5.50272 22.6775 5.02581 22.3259 4.67417C21.9742 4.32254 21.4973 4.125 21.0001 4.125H15.0001C14.5028 4.125 14.0259 4.32254 13.6742 4.67417C13.3226 5.02581 13.1251 5.50272 13.1251 6V7.875ZM7.50005 10.125C7.00277 10.125 6.52586 10.3225 6.17423 10.6742C5.8226 11.0258 5.62505 11.5027 5.62505 12V19.1153C9.47054 20.5749 13.6399 21.375 18.0001 21.375C22.2298 21.3802 26.4242 20.6136 30.3751 19.1151V12C30.3751 11.5027 30.1775 11.0258 29.8259 10.6742C29.4742 10.3225 28.9973 10.125 28.5001 10.125H7.50005ZM18.0001 23.625C22.2193 23.63 26.4043 22.9137 30.3751 21.511L18.0001 23.625ZM30.3751 21.511V27C30.3751 27.4973 30.1775 27.9742 29.8259 28.3258C29.4742 28.6775 28.9973 28.875 28.5001 28.875H7.50005C7.00277 28.875 6.52586 28.6775 6.17423 28.3258C5.8226 27.9742 5.62505 27.4973 5.62505 27V21.5114C9.49712 22.88 13.6622 23.6249 18.0001 23.625M16.8751 18C16.8751 17.3787 17.3787 16.875 18.0001 16.875H18.0151C18.6364 16.875 19.1401 17.3787 19.1401 18C19.1401 18.6213 18.6364 19.125 18.0151 19.125H18.0001C17.3787 19.125 16.8751 18.6213 16.8751 18Z"
                                            fill="#111827"
                                        />
                                    </svg>
                                    <h4 style="font-size: 18px; font-weight: bold">{{ $t('Tadbirkorlarbilanochiqmuloqot') }}</h4>
                                </div>
                            </b-card>
                        </a>
                    </b-col>
                    <b-col cols="12"> </b-col>
                </b-row>

                <b-col cols="12" class="px-6">
                    <div class="faq-text d-block d-sm-flex align-items-center p-3 px-sm-1">
                        <div class="text-center">
                            <svg class="mr-2" xmlns="http://www.w3.org/2000/svg" width="36" height="36" viewBox="0 0 36 36" fill="none">
                                <path
                                    d="M32.3398 16.1101L30.3148 13.7401C29.9398 13.2901 29.6248 12.4501 29.6248 11.8501V9.30012C29.6248 7.71012 28.3198 6.40512 26.7298 6.40512H24.1798C23.5798 6.40512 22.7248 6.09012 22.2748 5.71512L19.9048 3.69012C18.8698 2.80512 17.1748 2.80512 16.1398 3.69012L13.7398 5.71512C13.2898 6.09012 12.4498 6.40512 11.8498 6.40512H9.25482C7.66482 6.40512 6.35982 7.71012 6.35982 9.30012V11.8501C6.35982 12.4351 6.05982 13.2751 5.68482 13.7251L3.65982 16.1101C2.78982 17.1601 2.78982 18.8401 3.65982 19.8601L5.68482 22.2451C6.05982 22.6801 6.35982 23.5351 6.35982 24.1201V26.6851C6.35982 28.2751 7.66482 29.5801 9.25482 29.5801H11.8648C12.4498 29.5801 13.3048 29.8951 13.7548 30.2701L16.1248 32.2951C17.1598 33.1801 18.8548 33.1801 19.8898 32.2951L22.2598 30.2701C22.7098 29.8951 23.5498 29.5801 24.1498 29.5801H26.6998C28.2898 29.5801 29.5948 28.2751 29.5948 26.6851V24.1351C29.5948 23.5351 29.9098 22.6951 30.2848 22.2451L32.3098 19.8751C33.2248 18.8551 33.2248 17.1601 32.3398 16.1101ZM16.8748 12.1951C16.8748 11.5801 17.3848 11.0701 17.9998 11.0701C18.6148 11.0701 19.1248 11.5801 19.1248 12.1951V19.4401C19.1248 20.0551 18.6148 20.5651 17.9998 20.5651C17.3848 20.5651 16.8748 20.0551 16.8748 19.4401V12.1951ZM17.9998 25.3051C17.1748 25.3051 16.4998 24.6301 16.4998 23.8051C16.4998 22.9801 17.1598 22.3051 17.9998 22.3051C18.8248 22.3051 19.4998 22.9801 19.4998 23.8051C19.4998 24.6301 18.8398 25.3051 17.9998 25.3051Z"
                                    fill="#188EF6"
                                />
                            </svg>
                        </div>
                        <h4 class="my-3 mainPagetitle" style="font-weight: 700">
                            {{ $t('survey1') }}
                            <a href="https://ee.humanitarianresponse.info/QvDmcMon" target="_blank">{{ $t('survey2') }}</a>
                            {{ $t('survey3') }}.
                        </h4>
                    </div>
                </b-col>
            </b-row>
            <NewsSection class="mt-3" />

            <section class="pt-2 d-flex flex-column mb-2">
                <b-row class="bg-white rounded-sm border border-border-color appeal px-3 px-md-6">
                    <!-- Title -->
                    <div class="d-flex justify-content-center text-center">
                        <h4 class="my-3 mainPagetitle mobileTaklif">
                            {{ $t('FreeAreaFromBandlik') }}
                        </h4>
                    </div>

                    <!-- Table -->
                    <div class="table-responsive">
                        <b-table-simple bordered responsive class="custom-header">
                            <thead class="thead-custom">
                                <tr>
                                    <th v-for="field in fields" :key="field.key" :class="field.thClass">
                                        {{ field.label }}
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr v-for="(item, index) in items" :key="index" class="custom-row">
                                    <td v-for="field in fields" :key="field.key" :class="field.tdClass + (field.key === 'order' ? ' border-cell' : '')">
                                        {{ item[field.key] }}
                                    </td>
                                </tr>
                            </tbody>
                        </b-table-simple>
                    </div>
                </b-row>
            </section>

            <!-- map -->
            <MapSection />
        </div>

        <b-modal v-model="ForgotPasswordModal" hide-footer hide-header no-close-on-backdrop centered>
            <b-row>
                <b-col class="d-flex justify-content-between align-items-center">
                    <span>{{ $t('restore') }}</span>
                    <img class="cursor-pointer" @click="ForgotPasswordModal = false" src="/images/design/fill-close.svg" alt />
                </b-col>
            </b-row>
            <b-row>
                <b-col>
                    <LogincustomInput
                        disabled
                        :label="$t('phonenumber')"
                        v-mask="'+998-##-###-##-##'"
                        v-model="Restore.username"
                        placeholder="+998 _ _  _ _ _  _ _  _ _"
                    ></LogincustomInput>
                </b-col>
            </b-row>
            <div>
                <b-row>
                    <b-col>
                        <p class="p-2">{{ $t('smssent', { phonenumber: Restore.phonenumber }) }}</p>
                    </b-col>
                </b-row>
                <b-row class="mt-2">
                    <b-col>
                        <LogincustomInput type="password" :placeholder="$t('*******')" v-model="Restore.newpassword" :label="$t('password')"></LogincustomInput>
                    </b-col>
                </b-row>
                <b-row class="mt-3">
                    <b-col>
                        <custom-input type="password" :placeholder="$t('*******')" v-model="Restore.confirmedpassword" :label="$t('confirmpassword')"></custom-input>
                    </b-col>
                </b-row>
                <b-row class="mt-3">
                    <b-col>
                        <custom-input v-model="Restore.smscode" :placeholder="$t('0000')" :label="$t('smskod')"></custom-input>
                    </b-col>
                </b-row>
            </div>
            <b-row class="mt-3">
                <b-col>
                    <b-button v-if="Restore.isRestore" @click="RestorePasswordConfirm" block variant="warning">
                        <b-spinner v-if="RestoreLoading" small style="margin-right: 8px"></b-spinner>
                        {{ $t('restore') }}
                    </b-button>
                </b-col>
            </b-row>
            <b-row></b-row>
        </b-modal>

        <SetOrganizationModal v-if="setOrganizationModal" v-model="setOrganizationModal" />
    </div>
</template>

<script>
import customPhone from '../components/customPhoneInput.vue';
import LogincustomPhone from '../components/LogincustomPhoneInput.vue';
import customInput from '../components/elements/customInput.vue';
import LogincustomInput from '../components/elements/LogincustomInput.vue';
import customSelect from '../components/elements/customSelect.vue';
import customButton from '../components/elements/customButton.vue';
import customButtonOutline from '../components/elements/customButtonOutline.vue';
import AccountService from '@/services/account.service';
import ReportService from '@/services/report.service';
import AOS from 'aos';
import 'aos/dist/aos.css';
import LoginNavbar from '../components/home/Loginnavbar.vue';
import Swipper from '../components/home/Swipper.vue';
const MapSection = () => import('@/components/home/MapSection.vue');
const NewsSection = () => import('@/components/home/NewsSection.vue');
const SetOrganizationModal = () => import('@/views/account/widgets/SetOrganizationModal.vue');
const LoginByEImzo = () => import('@/components/LoginByEImzo.vue');

export default {
    components: {
        customPhone,
        customInput,
        customSelect,
        customButton,
        customButtonOutline,
        LogincustomPhone,
        NewsSection,
        LoginNavbar,
        MapSection,
        SetOrganizationModal,
        LoginByEImzo,
        LogincustomInput,
        Swipper
    },
    data() {
        return {
            inputType: 'password',
            signByEImzoModal: false,
            setOrganizationModal: false,
            ForgotPasswordModal: false,
            SignLoading: false,
            checkAccountLoading: false,
            isToken: false,
            Account: {
                phonenumber: '',
                password: '',
                passwordconfirm: '',
                smscode: ''
            },
            Restore: {
                username: '',
                isRestore: false,
                phonenumber: '',
                smscode: '',
                newpassword: '',
                confirmedpassword: ''
            },
            filter: {
                isPassword: null,
                isSmsCode: false,
                isSmsForRegister: false,
                isSms: false,
                admissiontypeid: 0,
                orderType: '',
                page: 1,
                pageSize: 3,
                search: '',
                sortBy: ''
            },
            fields: [],
            isBusy: false,
            totals: {
                freeArea: 0,
                allArea: 0
            },
            oneIdLoading: false,
            items: [],
            filter2: {
                contractTypeId: null,
                regionId: null,
                region: '',
                byRegion: true,
                districtId: null,
                district: '',
                byDistrict: false,
                contractorId: null,
                byContractor: false
            },
            clientUrl: window.location.origin
        };
    },
    created() {
        AOS.init();
        this.userName = JSON.parse(localStorage.getItem('user_info'));
        this.Account.phonenumber = '+998';
        this.getFields();
        this.GetFreeAreaFromBandlik();

        //oneid
        const e = window.location.search;
        const n = new URLSearchParams(e);
        const o = n.get('code');
        if (o && o.length > 0) {
            localStorage.setItem('code', o);
        }

        const r = localStorage.getItem('code');
        if (r) {
            this.oneIdLoading = true;
            AccountService.OneIdLogin({
                code: o,
                redirectUrl: this.clientUrl
            })
                .then((res) => {
                    if (res.status == 200) {
                        localStorage.setItem('businessmanUserId', res.data.businessmanUserId);
                        localStorage.setItem('user_info', JSON.stringify(res.data.user));
                        localStorage.removeItem('code');
                        if (res.data && res.data.user) {
                            this.$router.replace({ name: 'MyCabinet' });
                        } else {
                            this.setOrganizationModal = true;
                        }
                    }
                })
                .catch(this.showApiError)
                .finally(() => {
                    this.oneIdLoading = false;
                });
        }
    },
    mounted() {
        this.popupItem = this.$el;
    },
    computed: {
        lang() {
            return localStorage.getItem('locale') || 'uz_cyrl';
        }
    },
    methods: {
        changeType() {
            this.inputType = 'text';
        },
        changeTypepassword() {
            this.inputType = 'password';
        },
        GetFreeAreaFromBandlik() {
            this.isBusy = true;
            ReportService.GetFreeAreaFromBandlik(this.filter2)
                .then((res) => {
                    this.items = res.data;

                    this.totals = {
                        freeArea: 0,
                        allArea: 0
                    };
                    res.data.map((item) => {
                        this.totals.freeArea += item.freeArea;
                        this.totals.allArea += item.allArea;
                    });

                    this.isBusy = false;
                })
                .catch((error) => {
                    this.isBusy = false;
                });
        },
        getFields() {
            this.fields = [
                {
                    key: 'order',
                    label: this.$t('№'),
                    thClass: 'text-center',
                    tdClass: 'text-center'
                },
                {
                    key: 'region',
                    label: this.$t('region'),
                    sortable: false
                },
                {
                    key: 'district',
                    label: this.$t('liveregionname'),
                    sortable: false
                },
                {
                    key: 'companyAddress',
                    label: this.$t('companyAddress'),
                    sortable: false
                },
                {
                    key: 'companyTin',
                    label: this.$t('companyTin'),
                    sortable: false
                },
                {
                    key: 'companyName',
                    label: this.$t('companyName'),
                    sortable: false
                },
                {
                    key: 'allArea',
                    label: this.$t('allArea'),
                    thClass: 'text-right',
                    tdClass: 'text-right',
                    sortable: false
                },
                {
                    key: 'freeArea',
                    label: this.$t('freeArea'),
                    thClass: 'text-right',
                    tdClass: 'text-right',
                    sortable: false
                }
            ];
        },
        GoProposal() {
            this.$router.push({ name: 'Proposal' });
        },
        RestorePasswordConfirm() {
            if (this.Restore.confirmedpassword && this.Restore.confirmedpassword == this.Restore.newpassword) {
                this.RestoreLoading = true;
                AccountService.RestorePasswordConfirm(this.Restore)
                    .then((res) => {
                        this.RestoreLoading = false;
                        this.ForgotPasswordModal = false;
                        this.makeToast(this.$t('PasswordChangedSuccess'), 'success');
                    })
                    .catch((error) => {
                        this.RestoreLoading = false;
                        this.showApiError(error);
                    });
            } else {
                this.makeToast(this.$t('passwordNotEquals'), 'error');
            }
        },
        RestorePassword() {
            this.RestoreLoading = true;
            AccountService.RestorePassword(this.Restore)
                .then((res) => {
                    this.Restore.isRestore = true;
                    this.Restore.phonenumber = res.data.phoneNumber;
                    this.RestoreLoading = false;
                })
                .catch((error) => {
                    this.showApiError(error);
                    this.RestoreLoading = false;
                });
        },
        OpenForgotPass() {
            this.ForgotPasswordModal = true;
            this.Restore = {
                username: this.Account.phonenumber,
                isRestore: false,
                phonenumber: '',
                smscode: '',
                newpassword: '',
                confirmedpassword: ''
            };
            this.RestorePassword();
        },
        Logout() {
            AccountService.Logout().then((res) => {
                localStorage.clear();
                if (this.$route.name !== 'Home') {
                    this.$router.push('/');
                } else {
                    window.location.reload();
                }
            });
        },
        ChangePhone() {
            this.Account.password = '';
            this.Account.passwordconfirm = '';
            this.Account.smscode = '';
            this.filter.isPassword = null;
            this.filter.isSmsCode = false;
            this.filter.isSmsForRegister = false;
            this.filter.isSms = false;
        },
        clear() {
            this.Account = {
                phonenumber: '+998',
                password: '',
                passwordconfirm: '',
                smscode: ''
            };
            this.filter = {
                isPassword: null,
                isSmsCode: false,
                isSmsForRegister: false,
                isSms: false
            };
        },
        check() {
            if (this.Account.password.length < 5) {
                this.makeToast(this.$t('enterPasswordMinLengthSix'), 'error');
                return false;
            }
            if (this.Account.passwordconfirm.length < 5) {
                this.makeToast(this.$t('enterPasswordMinLengthSix'), 'error');
                return false;
            }
            return true;
        },
        SendSmsCode() {
            this.SignLoading = true;
            if (!this.filter.isSmsForRegister) {
                AccountService.SendSMSCode(this.Account)
                    .then((res) => {
                        this.SignLoading = false;
                        this.filter.isSmsForRegister = true;
                        this.filter.isSms = true;
                    })
                    .catch((error) => {
                        this.SignLoading = false;
                        this.showApiError(error);
                    });
            } else {
                if (this.Account.smscode.length === 4) {
                    AccountService.CheckSMSCode({ smscode: this.Account.smscode })
                        .then((res) => {
                            this.$router.push({ name: 'Register' });
                            this.SignLoading = false;
                        })
                        .catch((error) => {
                            this.showApiError(error);
                            this.SignLoading = false;
                        });
                } else {
                    this.SignLoading = false;
                    this.makeToast(this.$t('entersmscode'), 'error');
                }
            }
        },
        GiveSmsCode() {
            if (!this.check()) {
                return false;
            }
            this.SignLoading = true;

            AccountService.SendSMSCode(this.Account)
                .then((res) => {
                    this.SignLoading = false;
                    this.filter.isSmsForRegister = true;
                    this.filter.isSms = true;
                })
                .catch((error) => {
                    this.SignLoading = false;
                    this.showApiError(error);
                });
        },
        isCheckAccount() {
            if (this.Account.phonenumber.length === 17) {
                this.checkAccountLoading = true;
                AccountService.IsCheckAccount(this.Account)
                    .then((res) => {
                        this.checkAccountLoading = false;
                        if (res.data.success) {
                            this.filter.isPassword = true;
                        }
                        if (!res.data.success) {
                            this.filter.isPassword = false;
                        }
                    })
                    .catch((error) => {
                        this.checkAccountLoading = false;
                        this.showApiError(error);
                    });
            }
        },
        SignIn() {
            this.SignLoading = true;
            if (!this.filter.isSmsCode) {
                this.Account.username = this.Account.phonenumber;
                AccountService.SignIn(this.Account)
                    .then((res) => {
                        if (!res.data.trustedDevice) {
                            this.filter.isSmsCode = true;
                        } else {
                            localStorage.setItem('businessmanUserId', res.data.businessmanUserId);
                            if (res.data.user) {
                                localStorage.setItem('user_info', JSON.stringify(res.data.user));
                                this.$store.commit('setUserInfo', JSON.stringify(res.data.user));
                            }
                            if (res.data && res.data.user) {
                                this.$router.replace({ name: 'MyCabinet' });
                            } else {
                                this.setOrganizationModal = true;
                            }
                        }
                    })
                    .catch((error) => {
                        this.showApiError(error);
                    })
                    .finally(() => {
                        this.SignLoading = false;
                    });
            } else {
                AccountService.SignInTwoFactor(this.Account)
                    .then((res) => {
                        localStorage.setItem('businessmanUserId', res.data.businessmanUserId);
                        localStorage.setItem('user_info', JSON.stringify(res.data.user));
                        if (res.data && res.data.user) {
                            this.$router.replace({ name: 'MyCabinet' });
                        } else {
                            this.setOrganizationModal = true;
                        }
                    })
                    .catch((error) => {
                        this.showApiError(error);
                    })
                    .finally(() => {
                        this.SignLoading = false;
                    });
            }
        },
        topFunction() {
            document.body.scrollTop = 0;
            document.documentElement.scrollTop = 0;
        },
        SignbyOneId() {
            window.location.replace(
                'https://sso.egov.uz/sso/oauth/Authorization.do?response_type=one_code&client_id=my_chamber_uz&redirect_uri=' +
                    this.clientUrl +
                    '&scope=my_chamber_uz&secret=Yqw0y4MHK6wRfHeoC0A2SZsv&use_proxy=false&state=realState'
            );
        }
    }
};
</script>
<style scoped>
@import url('../assets/styles/landing_style.scss');
</style>
