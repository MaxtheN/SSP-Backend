<template>
   <b-overlay :show="show" class="container">
      <b-card>
         <b-row v-if="Data">
            <b-col sm="12" md="6" lg="6">
               <form-input-hrm :label="$t('docnumber')" disabled v-model="Data.docNumber" />
            </b-col>
            <b-col sm="12" md="6" lg="6">
               <form-input-hrm :label="$t('docdate')" disabled v-model="Data.docOn" />
            </b-col>
            <b-col sm="12" md="12" lg="12">
               <form-input-hrm :label="$t('responsibleName')" disabled v-model="Data.contractor" />
            </b-col>
            <!-- <b-col sm="12" md="6" lg="6">
               <form-input-hrm :label="$t('director')" disabled v-model="Data.director" />
            </b-col> -->
            <b-col sm="12" md="6" lg="6">
               <form-input-hrm :label="$t('inn')" disabled v-model="Data.contractorInn" />
            </b-col>
            <!-- <b-col sm="12" md="6" lg="6">
               <form-input-hrm :label="$t('pinfl')" disabled v-model="Data.contractorPinfl" />
            </b-col> -->
            <b-col sm="12" md="12" lg="6">
               <form-input-hrm :label="$t('region')" disabled v-model="Data.region" />
            </b-col>
            <b-col sm="12" md="6" lg="6">
               <form-input-hrm :label="$t('district')" disabled v-model="Data.district" />
            </b-col>
            <b-col sm="12" md="12" lg="6">
               <form-input-hrm :label="$t('address')" disabled v-model="Data.address" />
            </b-col>
            <b-col sm="12" md="6" lg="6">
               <form-input-hrm :label="$t('email')" disabled v-model="Data.email" />
            </b-col>
            <b-col sm="12" md="6" lg="6">
               <form-input-hrm :label="$t('docyear')" disabled v-model="Data.year" />
            </b-col>
            <b-col sm="12" md="6" lg="6">
               <form-input-hrm :label="$t('month')" disabled :value="$t('month' + Data.month)" />
            </b-col>
            <b-col sm="12" md="12" lg="6">
               <form-input-hrm :label="$t('bankname')" disabled v-model="Data.bank" />
            </b-col>
            <b-col sm="12" md="6" lg="6">
               <form-input-hrm
                  :label="$t('orgSettlementAccount')"
                  disabled
                  v-model="Data.contractorSettlementAccount"
               />
            </b-col>
            <b-col sm="12" md="6" lg="6">
               <form-input-hrm :label="$t('phone')" disabled v-model="Data.phone" />
            </b-col>
            <b-col sm="12" md="6" lg="6">
               <form-currency-input :label="$t('totalSubsidyAmount')" disabled v-model="Data.totalSubsidyAmount" />
            </b-col>

            <!--employees table  -->
            <hr class="w-100 mx-1" />
            <b-col cols="12">
               <h4>{{ $t('employeesInfo') }}</h4>
            </b-col>
            <b-col cols="12">
               <div class="simple-table">
                  <b-table-simple class="priceTable w-100">
                     <thead>
                        <tr>
                           <th>{{ $t('fullName') }}</th>
                           <th>{{ $t('pinfl') }}</th>
                           <th>{{ $t('passportNumber') }}</th>
                           <th>{{ $t('salary') }}</th>
                           <th>{{ $t('subsidy') }}</th>
                           <th>{{ $t('3_tomonlama_shartnoma_nusxasi') }}</th>
                        </tr>
                     </thead>
                     <tbody>
                        <tr v-for="(tab, j) in Data.tables" :key="j + 'tab'">
                           <td>{{ tab.surname }} {{ tab.name }} {{ tab.patronym }}</td>
                           <td>{{ tab.pinfl }}</td>
                           <td>{{ tab.seria }} {{ tab.number }}</td>
                           <td>{{ tab.salary }}</td>
                           <td>{{ tab.subsidy }}</td>
                           <td>
                              <template v-if="tab.files">
                                 <div class="mt-1" v-for="item in tab.files" :key="item.id">
                                    <b-link variant="primary" target="_blank" :href="FileSrc(item.id)"
                                       >{{ item.fileName || item.id }}
                                    </b-link>
                                 </div>
                              </template>
                           </td>
                        </tr>
                     </tbody>
                  </b-table-simple>
               </div>
            </b-col>
         </b-row>
      </b-card>
   </b-overlay>
</template>

<script>
import { BRow, BCol, BOverlay, BCard, BTableSimple } from 'bootstrap-vue';
import SubsidyRequestService from '@/services/dualedu/subsidyrequest.service';
import axios from 'axios';

export default {
   components: {
      BRow,
      BCol,
      BOverlay,
      BCard,
      BTableSimple
   },
   data() {
      return {
         Data: null,
         show: false
      };
   },
   created() {
      this.Refresh();
   },
   computed: {
      FileSrc() {
         return (id) => axios.defaults.baseURL + `Dual/SubsidyQequest/DownloadFile/${id}`;
      }
   },
   methods: {
      Refresh() {
         this.show = true;
         SubsidyRequestService.Get(this.$route.params.id)
            .then((res) => {
               this.Data = res.data;
            })
            .catch((error) => {
               this.showApiError(error);
            })
            .finally(() => {
               this.show = false;
            });
      }
   }
};
</script>
<style lang="scss">
.priceTable {
   thead {
      tr {
         background-color: #f0f0f0;
      }
   }

   tr th,
   tr td {
      padding: 7px;
      border-collapse: collapse;
      border: 1px solid #060606;
   }

   tr:nth-child(even) {
      background-color: #060606;
   }
}
</style>
