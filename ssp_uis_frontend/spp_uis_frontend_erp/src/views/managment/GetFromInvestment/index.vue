<template>
   <b-card no-body class="mt-2">
      <validation-observer ref="ValidationDTO">
         <b-row class="m-2">
            <b-col sm="12" md="3">
               <form-picker
                  required-star
                  v-model="filters.docDateFrom"
                  type="date"
                  format="DD.MM.YYYY"
                  :placeholder="$t('docDateFrom')"
               />
            </b-col>
            <b-col sm="12" md="3">
               <form-picker
                  required-star
                  v-model="filters.docDateTo"
                  format="DD.MM.YYYY"
                  type="date"
                  :placeholder="$t('docDateTo')"
               />
            </b-col>

            <b-col sm="12" md="4">
               <b-input-group>
                  <b-form-input required v-model="filters.contractorUzInn"></b-form-input>
                  <b-input-group-append>
                     <b-button @click="Refresh" variant="primary">
                        <feather-icon icon="SearchIcon" />
                     </b-button>
                  </b-input-group-append>
               </b-input-group>
            </b-col>
            <!-- <b-form-input v-model="filter.stir" :placeholder="$t('inn')" v-mask="'#########'" @change="checkCountry" /> -->
         </b-row>
      </validation-observer>

      <b-table
         :items="items"
         :fields="fields"
         :busy="isBusy"
         responsive
         sticky-header="65vh"
         striped
         no-border-collapse
         show-empty
         hover
         :empty-text="$t('NotFound')"
         class="position-relative"
      >
         <template #cell(№)="{ item, index }">
            {{ index + 1 }}
         </template>
      </b-table>
   </b-card>
</template>
<script>
import {
   BButton,
   BPagination,
   BTable,
   BCol,
   VBTooltip,
   VBModal,
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
   BTr,
   BTd,
   BTfoot,
   BTh,
   BThead,
   BTbody,
   BTableSimple,
   BAlert,
   BTabs,
   BTab,
   BOverlay
} from 'bootstrap-vue';
import BusinessmanCardService from '@/services/managment/businessmancard.service';
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
      BTr,
      BTabs,
      BTab,
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
         items: [],
         isBusy: false,
         filters: {
            docDateFrom: '',
            docDateTo: '',
            contractorUzInn: ''
         },
         fields: [
            {
               key: '№',
               label: this.$t('№'),
               thClass: 'text-center'
            },
            {
               key: 'idn',
               label: this.$t('idn'),
               thClass: 'text-center'
            },
            {
               key: 'bankId',
               label: this.$t('bankId'),
               thClass: 'text-center'
            },
            {
               key: 'contractorUzName',
               label: this.$t('contractorUzName'),
               thClass: 'text-center'
            },
            {
               key: 'docNo',
               label: this.$t('docNo'),
               thClass: 'text-center'
            },
            {
               key: 'docDate',
               label: this.$t('docDate'),
               thClass: 'text-center'
            },
            {
               key: 'contractorForName',
               label: this.$t('contractorForName'),
               thClass: 'text-center'
            },
            // {
            //    key: 'contractorForCountryCode',
            //    label: this.$t('contractorForCountryCode'),
            //    thClass: 'text-center',
            //
            // },
            {
               key: 'cntrStatus',
               label: this.$t('cntrStatus'),
               thClass: 'text-center'
            }
         ],
      };
   },
   methods: {
      Refresh() {
         this.$refs.ValidationDTO.validate().then((success) => {
            console.log(success);
            if (this.filters.contractorUzInn) {
               this.isBusy = true;
               BusinessmanCardService.GetFromInvestmentByInn(this.filters)
                  .then((res) => {
                     this.items = res.data;
                     this.isBusy = false;
                  })
                  .catch((error) => {
                     this.showApiError(error);
                  });
            } else {
               this.makeToast('STR kiritilmagan', 'danger');
            }
         });
      }
   }
};
</script>
