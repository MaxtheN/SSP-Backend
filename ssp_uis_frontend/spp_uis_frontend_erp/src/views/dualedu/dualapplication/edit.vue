<template>
   <b-overlay :show="show">
      <b-card>
         <b-row>
            <b-col cols="3">
               <form-input :label="$t('docnumber')" v-model="Application.application.docNumber" disabled></form-input>
            </b-col>
            <b-col cols="3">
               <form-input :label="$t('docOn')" v-model="Application.application.docOn" disabled></form-input>
            </b-col>
            <b-col cols="3">
               <form-input :label="$t('region')" v-model="Application.application.region" disabled></form-input>
            </b-col>
            <b-col cols="3">
               <form-input :label="$t('district')" v-model="Application.application.district" disabled></form-input>
            </b-col>
            <b-col cols="3" class="mt-1">
               <form-input :label="$t('contractor')" v-model="Application.application.contractor" disabled></form-input>
            </b-col>

            <b-col cols="3" class="mt-1">
               <form-input
                  :label="$t('contractorDirector')"
                  v-model="Application.application.contractorDirector"
                  disabled
               ></form-input>
            </b-col>
            <b-col cols="3" class="mt-1">
               <form-input :label="$t('inn')" v-model="Application.application.contractorInn" disabled></form-input>
            </b-col>
            <b-col cols="3" class="mt-1">
               <form-input
                  :label="$t('Adress')"
                  v-model="Application.application.contractorAddress"
                  disabled
               ></form-input>
            </b-col>
            <b-col cols="3" class="mt-1">
               <form-input
                  :label="$t('applicationType')"
                  v-model="Application.application.applicationType"
                  disabled
               ></form-input>
            </b-col>
         </b-row>
         <div class="simple-table">
            <b-table class="mt-4" :fields="fields" bordered :items="Application.tables"></b-table>
         </div>
      </b-card>
   </b-overlay>
</template>

<script>
import { BOverlay, BCard, BRow, BCol, BButton, BLink, BIcon, BBadge, BContainer, BTable } from 'bootstrap-vue';
import axios from 'axios';
import DualApplicationService from '@/services/dualedu/dualapplication.service';

export default {
   components: {
      BOverlay,
      BCard,
      BRow,
      BTable,
      BCol,
      BButton,
      BLink,
      BIcon,
      BBadge,
      BContainer
   },
   data() {
      return {
         show: false,
         iframeLoaded: false,
         Application: {},
         fields: [
            {
               key: 'orderNumber',
               label: this.$t('id'),
               thClass: 'text-center',
               tdClass: 'text-center'
            },
            {
               key: 'institute',
               label: this.$t('Institute')
            },
            {
               key: 'specialty',
               label: this.$t('Specialty')
            },
            {
               key: 'positionClassification',
               label: this.$t('positionClassification')
            },
            {
               key: 'emptyPositionsCount',
               label: this.$t('count5')
            },
            {
               key: 'details',
               label: this.$t('details')
            }
         ]
      };
   },
   computed: {
      IframeSrc() {
         return (
            axios.defaults.baseURL +
            `Dual/DualApplication/DownloadPdf?id2=${this.Application.application?.id2}&lang=${this.getPdfLang()}`
         );
      }
   },
   created() {
      this.GetApplication();
   },
   methods: {
      GetApplication() {
         this.show = true;
         DualApplicationService.Get(this.$route.params.id)
            .then((res) => {
               this.Application = res.data;
            })
            .catch((error) => {
               this.showApiError(error);
            })
            .finally(() => {
               this.show = false;
            });
      },
      Cancel() {
         this.$swal.fire({
            icon: 'question',
            title: this.$t('WantCancel'),
            showLoaderOnConfirm: true,
            preConfirm: () => {
               return DualApplicationService.Cancel({
                  id: this.Application.id,
                  message: ''
               })
                  .then(() => {
                     this.makeToast(this.$t('CancelMessage'), 'success');
                     this.GetApplication();
                  })
                  .catch(this.SwalError);
            },
            allowOutsideClick: () => !this.$swal.isLoading()
         });
      },
      Reject() {
         this.$swal.fire({
            icon: 'question',
            title: this.$t('WantReject'),
            showLoaderOnConfirm: true,
            preConfirm: () => {
               return DualApplicationService.Reject({
                  id: this.Application.id,
                  message: ''
               })
                  .then(() => {
                     this.makeToast(this.$t('RejectSuccess'), 'success');
                     this.GetApplication();
                  })
                  .catch(this.SwalError);
            },
            allowOutsideClick: () => !this.$swal.isLoading()
         });
      },
      Accept() {
         this.$swal.fire({
            icon: 'question',
            title: this.$t('WantAccept'),
            showLoaderOnConfirm: true,
            preConfirm: () => {
               return DualApplicationService.Accept({
                  id: this.Application.id,
                  message: ''
               })
                  .then(() => {
                     this.makeToast(this.$t('AcceptMessage'), 'success');
                     this.GetApplication();
                  })
                  .catch(this.SwalError);
            },
            allowOutsideClick: () => !this.$swal.isLoading()
         });
      }
   }
};
</script>
