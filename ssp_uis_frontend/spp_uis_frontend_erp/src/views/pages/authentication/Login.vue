<template>
   <div class="auth-wrapper auth-v2" style="position: relative">
      <div>
         <div class="d-flex images-login">
            <img v-if="login.languageId == 1" width="200px" src="@/assets/images/SSP_RU.png" alt="" />
            <img v-if="login.languageId == 3" width="200px" src="@/assets/images/SSP_UZB.png" alt="" />
            <img v-if="login.languageId == 2" width="200px" src="@/assets/images/SSP_UZB.png" alt="" />
         </div>
         <div class="language-login">
            <locale :is-login="true" @getLanguageId="getLanguageId" />
         </div>
      </div>
      <b-row class="auth-inner m-0">
         <!-- Brand logo-->
         <b-link class="brand-logo">
            <h2
               class="brand-text text-primary ml-1 header-name"
               :style="!isMobileDevice() ? 'width: 520px;' : 'width:300px; font-size:19px'"
            >
               {{ $t('appName') }}
            </h2>
         </b-link>

         <!-- /Brand logo-->

         <!-- Left Text-->
         <b-col lg="8" class="d-none d-lg-flex align-items-center p-5" style="background-color: #f3f4f9">
            <div class="w-100 d-lg-flex align-items-center justify-content-center px-5">
               <img src="@/assets/images/bg.jpg" alt="bg" />
            </div>
         </b-col>
         <!-- /Left Text-->

         <!-- Login-->

         <b-col lg="4" class="d-flex align-items-center auth-bg px-2 p-lg-5 header-content">
            <b-col sm="8" md="6" lg="12" class="px-xl-2 mx-auto">
               <b-card-title title-tag="h2" class="font-weight-bold mb-1">{{ $t('auth') }}</b-card-title>
               <!-- <b-card-text class="mb-2">{{ $t("Sign In to your account") }}</b-card-text> -->

               <!-- form -->

               <validation-observer ref="loginValidation">
                  <b-row>
                     <b-col lg="6">
                        <b-button
                           class="w-100"
                           :class="step == 1 && 'active'"
                           variant="outline-success"
                           @click="step = 1"
                           >{{ $t('enterLogin') }}</b-button
                        >
                     </b-col>
                     <b-col lg="6" v-if="!isMobileDevice()">
                        <b-button
                           class="w-100"
                           :class="step == 2 && 'active'"
                           variant="outline-primary"
                           @click="step = 2"
                        >
                           <feather-icon icon="KeyIcon" />
                           {{ $t('enterEImzo') }}
                        </b-button>
                     </b-col>
                     <b-col cols="12" class="my-1">
                        <b-button
                           class="w-100"
                           :disabled="oneIdLoading"
                           variant="outline-primary"
                           @click="SignbyOneId()"
                        >
                           <b-spinner small v-if="oneIdLoading" class="mr-1"></b-spinner>

                           <svg
                              width="65"
                              height="20"
                              viewBox="0 0 65 20"
                              fill="#4825C2"
                              xmlns="http://www.w3.org/2000/svg"
                           >
                              <path
                                 fill-rule="evenodd"
                                 clip-rule="evenodd"
                                 d="m64.5 10 -0.001 1 -0.001 0.414 -0.002 0.318 -0.002 0.268 -0.002 0.235 -0.002 0.214 -0.003 0.195 -0.004 0.182 -0.004 0.171 -0.005 0.162 -0.006 0.154 -0.006 0.146 -0.006 0.14 -0.006 0.135 -0.007 0.13 -0.008 0.126 -0.009 0.122 -0.009 0.118 -0.01 0.115 -0.01 0.111 -0.01 0.108 -0.011 0.106 -0.011 0.102 -0.013 0.102 -0.013 0.098 -0.013 0.097 -0.014 0.094 -0.014 0.093 -0.014 0.09 -0.014 0.09 -0.016 0.087 -0.016 0.086 -0.018 0.084 -0.018 0.082 -0.018 0.081 -0.018 0.079 -0.018 0.078 -0.019 0.077 -0.02 0.074 -0.021 0.074 -0.022 0.073 -0.022 0.072 -0.022 0.07 -0.022 0.07 -0.023 0.069 -0.023 0.068 -0.025 0.066 -0.025 0.066 -0.026 0.066 -0.026 0.063 -0.026 0.063 -0.027 0.062 -0.027 0.061 -0.028 0.06 -0.03 0.06 -0.03 0.058 -0.03 0.058 -0.03 0.058 -0.03 0.056 -0.031 0.055 -0.032 0.055 -0.033 0.054 -0.034 0.054 -0.034 0.053 -0.034 0.051 -0.035 0.05 -0.035 0.05 -0.037 0.05 -0.037 0.049 -0.038 0.048 -0.038 0.047 -0.038 0.046 -0.039 0.046 -0.04 0.046 -0.04 0.046 -0.042 0.043 -0.042 0.043 -0.042 0.042 -0.043 0.042 -0.043 0.042 -0.045 0.041 -0.046 0.04 -0.046 0.04 -0.046 0.038 -0.047 0.038 -0.047 0.038 -0.049 0.037 -0.05 0.037 -0.05 0.036 -0.05 0.035 -0.051 0.035 -0.052 0.034 -0.053 0.034 -0.054 0.034 -0.054 0.033 -0.055 0.032 -0.056 0.031 -0.057 0.03 -0.058 0.03 -0.058 0.03 -0.058 0.03 -0.06 0.029 -0.061 0.028 -0.062 0.027 -0.062 0.027 -0.063 0.026 -0.065 0.026 -0.066 0.026 -0.066 0.026 -0.067 0.025 -0.069 0.023 -0.07 0.023 -0.07 0.022 -0.071 0.022 -0.073 0.022 -0.074 0.022 -0.074 0.021 -0.076 0.02 -0.077 0.02 -0.078 0.018 -0.079 0.018 -0.082 0.018 -0.082 0.018 -0.085 0.018 -0.086 0.017 -0.087 0.016 -0.09 0.014 -0.09 0.014 -0.093 0.014 -0.094 0.014 -0.096 0.014 -0.098 0.013 -0.102 0.012 -0.102 0.012 -0.106 0.011 -0.108 0.01 -0.111 0.01 -0.114 0.01 -0.118 0.009 -0.121 0.009 -0.124 0.009 -0.129 0.007 -0.134 0.007 -0.138 0.006 -0.142 0.006 -0.15 0.006 -0.157 0.005 -0.165 0.005 -0.174 0.004 -0.186 0.004 -0.199 0.003 -0.217 0.003 -0.238 0.002 -0.271 0.002 -0.32 0.002 -0.413 0.001h-7.066l-0.437 -0.002 -0.33 -0.002 -0.278 -0.002 -0.243 -0.002 -0.22 -0.002 -0.202 -0.003 -0.187 -0.003 -0.175 -0.005 -0.166 -0.005 -0.158 -0.005 -0.15 -0.006 -0.145 -0.006 -0.138 -0.006 -0.134 -0.007 -0.13 -0.008 -0.126 -0.008 -0.121 -0.009 -0.118 -0.009 -0.114 -0.01 -0.111 -0.01 -0.109 -0.011 -0.106 -0.011 -0.103 -0.012 -0.102 -0.012 -0.099 -0.013 -0.097 -0.013 -0.095 -0.014 -0.093 -0.014 -0.091 -0.014 -0.09 -0.014 -0.087 -0.016 -0.086 -0.016 -0.085 -0.018 -0.082 -0.018 -0.082 -0.018 -0.08 -0.018 -0.078 -0.018 -0.078 -0.018 -0.076 -0.02 -0.074 -0.021 -0.074 -0.022 -0.073 -0.022 -0.072 -0.022 -0.07 -0.022 -0.07 -0.023 -0.068 -0.024 -0.067 -0.024 -0.066 -0.026 -0.066 -0.026 -0.066 -0.026 -0.063 -0.026 -0.062 -0.027 -0.062 -0.027 -0.061 -0.028 -0.06 -0.029 -0.058 -0.03 -0.058 -0.03 -0.058 -0.03 -0.057 -0.03 -0.056 -0.031 -0.055 -0.032 -0.054 -0.033 -0.054 -0.034 -0.054 -0.034 -0.052 -0.034 -0.05 -0.035 -0.05 -0.035 -0.05 -0.036 -0.05 -0.037 -0.049 -0.037 -0.047 -0.038 -0.047 -0.038 -0.046 -0.038 -0.046 -0.039 -0.046 -0.04 -0.045 -0.041 -0.043 -0.042 -0.043 -0.042 -0.042 -0.042 -0.042 -0.043 -0.041 -0.044 -0.041 -0.045 -0.04 -0.046 -0.038 -0.046 -0.038 -0.046 -0.038 -0.047 -0.038 -0.048 -0.037 -0.049 -0.036 -0.05 -0.036 -0.05 -0.035 -0.05 -0.034 -0.051 -0.034 -0.052 -0.034 -0.054 -0.034 -0.054 -0.032 -0.054 -0.032 -0.055 -0.03 -0.056 -0.03 -0.058 -0.03 -0.058 -0.03 -0.058 -0.03 -0.058 -0.028 -0.06 -0.027 -0.062 -0.027 -0.062 -0.026 -0.063 -0.026 -0.063 -0.026 -0.066 -0.026 -0.066 -0.026 -0.067 -0.023 -0.067 -0.023 -0.069 -0.023 -0.07 -0.022 -0.07 -0.022 -0.072 -0.022 -0.073 -0.02 -0.074 -0.021 -0.075 -0.02 -0.077 -0.018 -0.078 -0.018 -0.079 -0.018 -0.081 -0.018 -0.082 -0.017 -0.083 -0.017 -0.086 -0.016 -0.087 -0.014 -0.089 -0.014 -0.09 -0.014 -0.092 -0.014 -0.094 -0.013 -0.096 -0.013 -0.098 -0.012 -0.101 -0.011 -0.102 -0.011 -0.106 -0.011 -0.109 -0.01 -0.111 -0.01 -0.114 -0.009 -0.118 -0.009 -0.121 -0.008 -0.126 -0.007 -0.13 -0.007 -0.134 -0.006 -0.14 -0.006 -0.145 -0.006 -0.153 -0.005 -0.16 -0.005 -0.17 -0.004 -0.18 -0.003 -0.194 -0.002 -0.21 -0.002 -0.232 -0.002 -0.262 -0.002 -0.308 -0.001 -0.394 0.001 -0.674v-1.316l0.002 -0.44 0.002 -0.329 0.002 -0.274 0.002 -0.24 0.002 -0.217 0.003 -0.198 0.003 -0.184 0.005 -0.173 0.005 -0.162 0.005 -0.154 0.006 -0.147 0.006 -0.142 0.007 -0.136 0.007 -0.131 0.008 -0.126 0.008 -0.122 0.009 -0.118 0.01 -0.115 0.01 -0.112 0.01 -0.11 0.011 -0.106 0.011 -0.103 0.012 -0.102 0.013 -0.099 0.013 -0.097 0.014 -0.095 0.014 -0.093 0.014 -0.09 0.014 -0.09 0.014 -0.087 0.017 -0.086 0.018 -0.084 0.018 -0.082 0.018 -0.081 0.018 -0.08 0.018 -0.078 0.018 -0.077 0.02 -0.076 0.021 -0.074 0.022 -0.073 0.022 -0.072 0.022 -0.071 0.022 -0.07 0.023 -0.07 0.023 -0.067 0.025 -0.067 0.026 -0.066 0.026 -0.066 0.026 -0.064 0.026 -0.063 0.027 -0.062 0.027 -0.062 0.028 -0.06 0.03 -0.06 0.03 -0.058 0.03 -0.058 0.03 -0.058 0.03 -0.056 0.031 -0.055 0.031 -0.055 0.034 -0.054 0.034 -0.054 0.034 -0.053 0.035 -0.051 0.034 -0.05 0.035 -0.05 0.037 -0.05 0.037 -0.049 0.038 -0.048 0.038 -0.047 0.038 -0.047 0.038 -0.046 0.04 -0.046 0.04 -0.046 0.041 -0.044 0.042 -0.043 0.042 -0.043 0.043 -0.042 0.043 -0.042 0.045 -0.041 0.046 -0.04 0.046 -0.04 0.046 -0.038 0.047 -0.038 0.047 -0.038 0.048 -0.037 0.05 -0.037 0.05 -0.036 0.05 -0.035 0.05 -0.035 0.052 -0.034 0.053 -0.034 0.054 -0.034 0.054 -0.034 0.055 -0.032 0.055 -0.031 0.058 -0.03 0.058 -0.03 0.058 -0.03 0.058 -0.03 0.06 -0.03 0.06 -0.028 0.062 -0.027 0.062 -0.027 0.063 -0.026 0.064 -0.026 0.066 -0.026 0.066 -0.026 0.067 -0.026 0.068 -0.023 0.07 -0.023 0.07 -0.023 0.071 -0.022 0.072 -0.022 0.074 -0.022 0.074 -0.022 0.075 -0.02 0.078 -0.02 0.078 -0.018 0.08 -0.018 0.081 -0.018 0.082 -0.018 0.084 -0.018 0.086 -0.017 0.087 -0.016 0.089 -0.015 0.09 -0.014 0.093 -0.014 0.094 -0.014 0.097 -0.014 0.098 -0.013 0.1 -0.013 0.102 -0.011 0.106 -0.011 0.108 -0.011 0.11 -0.01 0.114 -0.01 0.117 -0.01 0.12 -0.009 0.123 -0.009 0.128 -0.008 0.132 -0.007 0.138 -0.006 0.142 -0.006 0.149 -0.006 0.154 -0.005 0.164 -0.005 0.173 -0.005 0.183 -0.003 0.197 -0.003 0.214 -0.002 0.234 -0.002 0.266 -0.002 0.311 -0.002 0.392 -0.001 0.635 0.003h6.422l0.466 0.002 0.342 0.002 0.284 0.002 0.248 0.002 0.223 0.003 0.204 0.003 0.19 0.003 0.178 0.004 0.167 0.005 0.158 0.005 0.152 0.006 0.145 0.006 0.139 0.007 0.134 0.007 0.13 0.007 0.126 0.009 0.122 0.009 0.118 0.009 0.115 0.01 0.112 0.01 0.109 0.01 0.106 0.011 0.103 0.011 0.102 0.012 0.099 0.013 0.098 0.013 0.095 0.014 0.094 0.014 0.091 0.014 0.09 0.014 0.088 0.016 0.086 0.016 0.085 0.018 0.083 0.018 0.082 0.018 0.081 0.018 0.078 0.018 0.078 0.02 0.077 0.019 0.075 0.02 0.074 0.022 0.073 0.022 0.072 0.022 0.07 0.022 0.07 0.023 0.07 0.023 0.067 0.024 0.066 0.026 0.066 0.026 0.065 0.026 0.064 0.026 0.062 0.027 0.062 0.027 0.061 0.028 0.06 0.029 0.059 0.03 0.058 0.03 0.058 0.03 0.057 0.03 0.056 0.03 0.055 0.032 0.054 0.032 0.054 0.034 0.054 0.034 0.052 0.034 0.051 0.035 0.05 0.035 0.05 0.036 0.05 0.036 0.049 0.037 0.048 0.038 0.047 0.038 0.046 0.038 0.046 0.04 0.046 0.04 0.045 0.04 0.043 0.042 0.043 0.042 0.042 0.042 0.042 0.043 0.041 0.044 0.041 0.044 0.04 0.046 0.039 0.046 0.038 0.046 0.038 0.047 0.038 0.048 0.037 0.049 0.037 0.05 0.035 0.05 0.035 0.05 0.034 0.051 0.034 0.052 0.034 0.054 0.034 0.054 0.032 0.054 0.032 0.055 0.03 0.055 0.03 0.058 0.03 0.058 0.03 0.058 0.029 0.058 0.029 0.06 0.028 0.061 0.027 0.062 0.026 0.063 0.026 0.063 0.026 0.065 0.026 0.066 0.024 0.066 0.024 0.067 0.023 0.069 0.023 0.07 0.022 0.07 0.022 0.072 0.022 0.073 0.021 0.074 0.021 0.074 0.019 0.077 0.018 0.078 0.018 0.079 0.018 0.08 0.018 0.082 0.018 0.083 0.017 0.086 0.016 0.086 0.014 0.088 0.014 0.09 0.014 0.092 0.014 0.094 0.014 0.095 0.013 0.098 0.012 0.1 0.012 0.102 0.011 0.106 0.01 0.107 0.01 0.11 0.01 0.114 0.009 0.118 0.009 0.12 0.008 0.126 0.007 0.129 0.007 0.134 0.006 0.138 0.006 0.145 0.006 0.15 0.006 0.158 0.005 0.168 0.004 0.178 0.003 0.19 0.003 0.207 0.002 0.227 0.002 0.257 0.002 0.299 0.002 0.374 0.001 0.584v0.566Zm-15 5.5h4c3.79 0 6 -2.358 6 -5.531 0 -3.206 -2.181 -5.469 -5.94 -5.469h-4.06v11Zm-2 0h-3V4.5h3v11ZM10.977 10c0 3.034 -2.198 5.75 -5.504 5.75S0 13.066 0 10c0 -3.034 2.198 -5.75 5.504 -5.75 3.306 0 5.474 2.685 5.474 5.75Zm-10.135 0c0 2.748 1.966 5.003 4.662 5.003s4.631 -2.224 4.631 -5.003c0 -2.748 -1.965 -5.003 -4.662 -5.003S0.842 7.221 0.842 10Zm13.931 5.5H14V4.5h0.758l7.47 9.57V4.5h0.774v11H22.4L14.774 5.726v9.774Zm19.227 0h-7.78V4.5h7.702v0.754H27.024v4.321h6.202v0.754H27.024v4.415h6.976v0.754Zm20.098 -1.927a4.538 4.538 0 0 0 1.098 -0.167 3.246 3.246 0 0 0 0.844 -0.362 3.176 3.176 0 0 0 0.211 -0.138l-0.002 -0.002a3.674 3.674 0 0 0 0.632 -0.591c0.086 -0.102 0.234 -0.315 0.318 -0.48 -0.194 -0.206 -0.49 -0.376 -0.768 -0.526l-0.03 -0.017a5.512 5.512 0 0 0 -0.578 -0.279l-0.072 -0.027c-0.187 -0.07 -0.406 -0.15 -0.429 -0.373 -0.018 -0.172 0.07 -0.279 0.174 -0.406 0.026 -0.03 0.05 -0.063 0.077 -0.098 0.303 -0.406 0.588 -0.957 0.537 -1.704 -0.05 -0.734 -0.52 -1.302 -1.306 -1.391a2.117 2.117 0 0 0 -0.266 -0.014 2.126 2.126 0 0 0 -0.266 0.014c-0.786 0.088 -1.254 0.658 -1.306 1.391 -0.051 0.747 0.234 1.298 0.537 1.704l0.077 0.098c0.104 0.128 0.19 0.234 0.174 0.406 -0.022 0.223 -0.242 0.304 -0.429 0.373l-0.071 0.027a5.498 5.498 0 0 0 -0.578 0.28l-0.03 0.015c-0.412 0.222 -0.864 0.488 -0.966 0.852a5.158 5.158 0 0 0 -0.178 1.413h0.703c0.196 0 0.398 0.002 0.602 0.004h0.001c0.422 0.004 0.858 0.008 1.29 -0.004Z"
                              />
                           </svg>
                        </b-button>
                     </b-col>
                  </b-row>
                  <div v-if="!isMobileDevice() && step == 2">
                     <just-sign @sign="loginESP($event)" />
                  </div>
                  <div v-show="step == 1">
                     <b-form class="auth-login-form mt-2" @submit.prevent>
                        <form-input v-model="login.username" required :label="$t('username')" />

                        <!-- forgot password -->
                        <b-form-group>
                           <div class="d-flex justify-content-between">
                              <label for="login-password">{{ $t('Password') }}</label>
                           </div>
                           <validation-provider #default="{ errors }" name="Password" rules="required">
                              <b-input-group class="input-group-merge" :class="errors.length > 0 ? 'is-invalid' : null">
                                 <b-form-input
                                    v-model="login.password"
                                    :state="errors.length > 0 ? false : null"
                                    class="form-control-merge"
                                    :type="passwordFieldType"
                                    name="login-password"
                                    placeholder="············"
                                 />
                                 <b-input-group-append is-text>
                                    <feather-icon
                                       class="cursor-pointer"
                                       :icon="passwordToggleIcon"
                                       @click="togglePasswordVisibility"
                                    />
                                 </b-input-group-append>
                              </b-input-group>
                              <small class="text-danger" v-show="errors.length > 0">{{ $t(`${errors[0]}`) }}</small>
                           </validation-provider>
                        </b-form-group>
                        <div class="d-flex mb-1 justify-content-end">
                           <b-link :to="{ name: 'forgot-password' }">
                              <small>{{ $t('forget-password') }}</small>
                           </b-link>
                        </div>

                        <!-- !!!!!!!!!  LOCAL BILAN SERVER AJRATILDI -->

                        <b-button type="submit" variant="primary" block @click="validationForm" :disabled="Loading">
                           <b-spinner v-if="Loading" small></b-spinner>
                           {{ $t('SignIn') }}
                        </b-button>
                     </b-form>
                  </div>
               </validation-observer>

               <b-modal v-model="sms.trusteddevice" hide-footer no-close-on-backdrop :title="$t('SmsCode')">
                  <b-alert show variant="success">
                     <p class="p-1">{{ $t('SendSmmYourPhone', { phoneNumber: sms.phoneNumber }) }}</p>
                  </b-alert>
                  <b-row>
                     <b-col>
                        <label for>{{ $t('SmsCode') }}</label>
                        <b-form-input maxlength="4" v-model="sms.smscode"></b-form-input>
                     </b-col>
                  </b-row>
                  <b-row class="mt-2">
                     <b-col class="text-center">
                        <b-button variant="danger" @click="sms.trusteddevice = false" class="mr-2">
                           <feather-icon icon="XIcon"></feather-icon>
                           {{ $t('Cancel') }}
                        </b-button>
                        <b-button variant="success" @click="SignTwoFactor">
                           <b-spinner small v-if="SignTwoFactorLoading"></b-spinner>
                           <feather-icon v-if="!SignTwoFactorLoading" icon="CheckSquareIcon"></feather-icon>
                           {{ $t('SignIn') }}
                        </b-button>
                     </b-col>
                  </b-row>
               </b-modal>
            </b-col>
         </b-col>
         <!-- /Login-->
      </b-row>
   </div>
</template>

<script>
import { ValidationProvider, ValidationObserver } from 'vee-validate';
import {
   BSpinner,
   BRow,
   BCol,
   BLink,
   BFormGroup,
   BFormInput,
   BInputGroupAppend,
   BInputGroup,
   BFormCheckbox,
   BCardText,
   BCardTitle,
   BImg,
   BForm,
   BButton,
   BModal,
   BAlert
} from 'bootstrap-vue';
import { required, email } from '@validations';
import { togglePasswordVisibility } from '@core/mixins/ui/forms';

import AccountService from '@/services/others/account.service';
import ApiService from '@/services/api.service';
import Locale from '@core/layouts/components/app-navbar/components/Locale.vue';
// import justSign from '@/components/justSign.vue';
import FormInput from '@/components/forms/form-input.vue';
// import Global from "@/mixins/global";
import Cookies from 'js-cookie';

export default {
   components: {
      BRow,
      BCol,
      BLink,
      BFormGroup,
      BFormInput,
      BInputGroupAppend,
      BInputGroup,
      BFormCheckbox,
      BCardText,
      BCardTitle,
      BImg,
      BForm,
      BButton,
      ValidationProvider,
      BSpinner,
      ValidationObserver,
      BModal,
      BAlert,
      Locale,
      justSign: () => import('@/components/justSign.vue'),
      FormInput
   },
   mixins: [togglePasswordVisibility],
   name: 'Login',
   data() {
      return {
         oneIdLoading: false,
         clientUrl: window.location.origin + window.location.pathname,
         tempLanguageId: null,
         status: '',
         password: '',
         userEmail: '',
         required,
         step: 1,
         email,
         Loading: false,

         login: {
            username: '',
            password: '',
            languageId: 3
         },
         sms: {
            trusteddevice: false,
            phoneNumber: ''
         },
         SignTwoFactorLoading: false
      };
   },
   computed: {
      passwordToggleIcon() {
         return this.passwordFieldType === 'password' ? 'EyeIcon' : 'EyeOffIcon';
      },
      isLocal() {
         if (window.location.href.indexOf('http://localhost') > -1) {
            return true;
         } else {
            return false;
         }
      },
      domainC() {
         let domain = '.apptest.uz';
         if (window.location.href.indexOf('http://erp.chamber.uz') > -1) {
            domain = '.chamber.uz/';
         }
         if (window.location.href.indexOf('https://erp.chamber.uz') > -1) {
            domain = '.chamber.uz/';
         }

         return domain;
      }
   },
   created() {
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
               if (res.data.user) {
                  localStorage.setItem('user_info', JSON.stringify(res.data.user));
                  Cookies.set('auth_token', res.data.token, { expires: 1, path: '', domain: this.domainC });
                  localStorage.setItem('auth_token', res.data.token);
                  ApiService.setHeader();

                  this.$store.dispatch('auth/login', res.data);
               }
               this.oneIdLoading = false;
               this.$router.push('/');
            })
            .catch(this.showApiError)
            .finally(() => {
               this.oneIdLoading = false;
            });
      }
   },
   methods: {
      loginESP(item) {
         if (!this.isMobileDevice()) {
            AccountService.LoginByEImzo({
               signData: item.key
            })
               .then((res) => {
                  Cookies.set('auth_token', res.data.token, { expires: 1, path: '', domain: this.domainC });
                  localStorage.setItem('auth_token', res.data.token);

                  let tempLocal = 'ru';

                  if (res.data.user?.languageId == 1) {
                     tempLocal = 'ru';
                  } else if (res.data.user?.languageId == 2) {
                     tempLocal = 'uz_cyrl';
                  } else if (res.data.user?.languageId == 3) {
                     tempLocal = 'uz_latn';
                  } else if (res.data.user?.languageId == 4) {
                     tempLocal = 'eng';
                  }

                  localStorage.setItem('locale', tempLocal);
                  localStorage.setItem('langId', res.data.user?.languageId);
                  this.$i18n.locale = tempLocal;

                  localStorage.setItem('user_info', JSON.stringify(res.data.user));
                  this.$store.dispatch('auth/login', res.data);
                  ApiService.setHeader();
                  this.Loading = false;
                  this.$router.push('/');
               })
               .catch((error) => {
                  this.Loading = false;

                  this.makeToast(error.response.data.errors, 'danger');
               });
         }
      },
      getLanguageId(id) {
         this.login.languageId = id;
      },
      validationForm() {
         this.$refs.loginValidation.validate().then((success) => {
            if (success) {
               this.Loading = true;
               AccountService.Login(this.login)
                  .then((res) => {
                     console.log(this.domainC);
                     localStorage.setItem('auth_token', res.data.token);
                     Cookies.set('auth_token', res.data.token, { expires: 1, path: '', domain: this.domainC });
                     let tempLocal = 'ru';
                     if (res.data.user?.languageId == 1) {
                        tempLocal = 'ru';
                     } else if (res.data.user?.languageId == 2) {
                        tempLocal = 'uz_cyrl';
                     } else if (res.data.user?.languageId == 3) {
                        tempLocal = 'uz_latn';
                     } else if (res.data.user?.languageId == 4) {
                        tempLocal = 'eng';
                     }
                     localStorage.setItem('locale', tempLocal);
                     localStorage.setItem('langId', res.data.user?.languageId);
                     this.$i18n.locale = tempLocal;
                     // this.$store.dispatch(
                     //   "auth/setOrganizationtype",
                     //   res.data.user.organizationtypeid
                     // );
                     localStorage.setItem('user_info', JSON.stringify(res.data.user));
                     this.$store.dispatch('auth/login', res.data);
                     ApiService.setHeader();
                     this.Loading = false;
                     this.$router.push('/');
                  })
                  .catch((error) => {
                     this.Loading = false;
                     this.showApiError(error);
                  });
            }
         });
      },
      Sign() {
         this.$refs.loginValidation.validate().then((success) => {
            if (success) {
               this.Loading = true;
               AccountService.Login(this.login)
                  .then((res) => {
                     if (res.data.user) {
                        localStorage.setItem('user_info', JSON.stringify(res.data.user));
                        this.$store.dispatch('auth/login', res.data);
                     }
                     this.Loading = false;
                     this.$router.push('/');
                     // if (res.data.trusteddevice) {
                     //   this.$router.push("/");
                     // }
                     // if (!res.data.trusteddevice) {
                     //   this.sms.trusteddevice = true;
                     //   this.sms.phoneNumber = res.data.phoneNumber;
                     //   this.sms.smscode = "";
                     // }
                  })
                  .catch((error) => {
                     this.showApiError(error);
                     this.Loading = false;
                  });
            }
         });
      },
      SignTwoFactor() {
         if (this.sms.smscode === undefined || this.sms.smscode === null || this.sms.smscode === '') {
            this.makeToast(this.$t('SmsCodeNotCorrect'), 'danger');
            return false;
         }
         this.SignTwoFactorLoading = true;
         AccountService.SignInTwoFactor(this.sms)
            .then((res) => {
               this.SignTwoFactorLoading = false;
               if (res.data.user) {
                  localStorage.setItem('user_info', JSON.stringify(res.data.user));
                  this.$store.dispatch('auth/login', res.data);
                  this.$router.push('/');
               }
            })
            .catch((error) => {
               this.showApiError(error);
               this.SignTwoFactorLoading = false;
            });
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

<style lang="scss">
@import '@core/scss/vue/pages/page-auth.scss';

@media screen and (max-width: 930px) {
   .header-name {
      margin-top: 70px;
   }
   .header-content {
      margin-top: 200px;
   }
   .language-login {
      display: none;
   }
}

.language-login {
   position: absolute;
   z-index: 5;
   right: 4rem;
   li {
      top: 2rem;
      list-style: none;
   }
}
.images-login {
   position: absolute;
   z-index: 1;
   top: 1rem;
   right: 20vw;
   li {
      top: 2rem;
      list-style: none;
   }
}
</style>
