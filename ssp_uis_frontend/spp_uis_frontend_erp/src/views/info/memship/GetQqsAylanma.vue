<template>
   <div>
      <b-card no-body>
         <div class="m-2">
            <b-row>
               <h3>{{ $t('Qqs') }}</h3>
            </b-row>
            <b-row>
               <b-col sm="12" md="3" class="mb-1">
                  <form-picker type="year" format="YYYY" v-model="filterQqs.year" :label="$t('docyear')" />
               </b-col>
               <b-col sm="12" md="3">
                  <form-select :options="MonthList" v-model="filterQqs.month" label="month" />
               </b-col>
               <b-col cols="12" md="4">
                  <form-input-hrm :label="$t('inn')" v-mask="'#########'" v-model="filterQqs.inn" />
               </b-col>
               <b-col>
                  <b-input-group-append class="mt-2">
                     <b-button :disabled="isdisableBtn" variant="primary" @click="RefreshQqs">
                        <feather-icon icon="SearchIcon" />
                     </b-button>
                  </b-input-group-append>
               </b-col>
            </b-row>
            <b-row> </b-row>
         </div>
      </b-card>
      <b-overlay :show="isBusyQqs">
         <b-card v-if="dataQqs.length != 0">
            <div class="d-flex gap-1">
               <h2 style="margin-right: 10px">{{ $t('companyName') }} -</h2>

               <span style="color: blue; font-size: 18px"> {{ dataQqs.name }}</span>
            </div>
            <div class="d-flex">
               <h2 style="margin-right: 10px">{{ $t('inn') }} -</h2>

               <span style="color: blue; font-size: 18px">{{ dataQqs.tin }}</span>
            </div>
            <div class="d-flex">
               <h2 style="margin-right: 10px">{{ $t('netIncomeWithoutVat') }} -</h2>

               <span style="color: blue; font-size: 18px">{{ currency(dataQqs.netIncomeWithoutVat) }}</span>
            </div>
            <div class="d-flex">
               <h2 style="margin-right: 10px">{{ $t('vatSum') }} -</h2>

               <span style="color: blue; font-size: 18px">{{ currency(dataQqs.vatSum) }}</span>
            </div>
         </b-card>
         <!-- <b-alert
            v-else
            show
            variant="danger"
            class="d-flex justify-content-center align-items-center"
            style="height: 150px"
         >
            <feather-icon icon="ArchiveIcon" size="25" class="mr-1"></feather-icon>
            <span style="font-size: 22px">{{ $t('soliqContractorByTin') }}</span>
         </b-alert> -->
      </b-overlay>
      <b-card no-body>
         <div class="m-2">
            <b-row>
               <h3>{{ $t('Aos') }}</h3>
            </b-row>
            <b-row>
               <b-col sm="12" md="3" class="mb-1">
                  <form-picker type="year" format="YYYY" v-model="filterAos.year" :label="$t('docyear')" />
               </b-col>
               <!-- <b-col sm="12" md="3">
                  <form-select :options="MonthList" v-model="filterAos.month" label="month" />
               </b-col> -->
               <b-col cols="12" md="4">
                  <form-input-hrm :label="$t('inn')" v-mask="'#########'" v-model="filterAos.inn" />
               </b-col>
               <b-col>
                  <b-input-group-append class="mt-2">
                     <b-button :disabled="isdisableBtn" variant="primary" @click="RefreshAos">
                        <feather-icon icon="SearchIcon" />
                     </b-button>
                  </b-input-group-append>
               </b-col>
            </b-row>
            <b-row> </b-row>
         </div>
      </b-card>
      <b-overlay :show="isBusyAos">
         <b-card v-if="dataAos.length != 0">
            <div class="d-flex gap-1">
               <h2 style="margin-right: 10px">{{ $t('companyName') }} -</h2>

               <span style="color: blue; font-size: 18px"> {{ dataAos.name }}</span>
            </div>
            <div class="d-flex">
               <h2 style="margin-right: 10px">{{ $t('inn') }} -</h2>

               <span style="color: blue; font-size: 18px">{{ dataAos.tin }}</span>
            </div>
            <div class="d-flex">
               <h2 style="margin-right: 10px">{{ $t('netIncome') }} -</h2>

               <span style="color: blue; font-size: 18px">{{ currency(dataAos.netIncome) }}</span>
            </div>
         </b-card>
         <!-- <b-alert
            v-else
            show
            variant="danger"
            class="d-flex justify-content-center align-items-center"
            style="height: 150px"
         >
            <feather-icon icon="ArchiveIcon" size="25" class="mr-1"></feather-icon>
            <span style="font-size: 22px">{{ $t('soliqContractorByTin') }}</span>
         </b-alert> -->
      </b-overlay>
   </div>
</template>

<script>
import MemshipCertificateService from '@/services/document/memshipcertificate.service';
import ManualService from '@/services/others/manual.service';
import {
   BButton,
   BPagination,
   BTable,
   BCol,
   BRow,
   BSpinner,
   BCard,
   BTooltip,
   BBadge,
   BInputGroup,
   BFormInput,
   BInputGroupAppend,
   BLink,
   BModal,
   BCardText,
   BListGroup,
   BListGroupItem,
   BTr,
   BTd,
   BTfoot,
   BTh,
   BThead,
   BTbody,
   BTableSimple,
   BAlert,
   BOverlay
} from 'bootstrap-vue';
export default {
   components: {
      BButton,
      BPagination,
      BTable,
      BCol,
      BRow,
      BSpinner,
      BCard,
      BTooltip,
      BBadge,
      BInputGroup,
      BFormInput,
      BInputGroupAppend,
      BLink,
      BModal,
      BCardText,
      BListGroup,
      BListGroupItem,
      BTr,
      BTd,
      BTfoot,
      BTh,
      BThead,
      BTbody,
      BTableSimple,
      BAlert,
      BOverlay
   },
   data() {
      return {
         isBusyQqs: false,
         isBusyAos: false,
         MonthList: [],
         filterQqs: {
            inn: '',
            year: '',
            month: ''
         },
         filterAos: {
            inn: '',
            year: '',
            month: ''
         },
         dataQqs: [],
         dataAos: []
      };
   },
   created() {
      ManualService.GetMonthSelectList().then((res) => {
         this.MonthList = res.data;
      });
   },
   methods: {
      RefreshQqs() {
         this.isBusyQqs = true;
         MemshipCertificateService.GetQqsAylanma(this.filterQqs)
            .then((res) => {
               this.dataQqs = res.data;
               this.isBusyQqs = false;
            })
            .catch((e) => {
               this.showApiError(e);
               this.dataQqs = [];
            })
            .finally(() => {
               this.isBusyQqs = false;
            });
      },
      RefreshAos() {
         this.isBusyAos = true;
         MemshipCertificateService.GetAosAylanma(this.filterAos)
            .then((res) => {
               this.dataAos = res.data;
               this.isBusyAos = false;
            })
            .catch((e) => {
               this.showApiError(e);
               this.dataAos = [];
            })
            .finally(() => {
               this.isBusyAos = false;
            });
      }
   },

   computed: {
      isdisableBtnQqs() {
         if (this.filterQqs.inn.length && this.filterQqs.month && this.filterQqs.year) {
            return false;
         } else {
            return true;
         }
      },
      isdisableBtnAos() {
         if (this.filterAos.inn.length && this.filterAos.month && this.filterAos.year) {
            return false;
         } else {
            return true;
         }
      }
   }
};
</script>

<style lang="scss" scoped></style>
