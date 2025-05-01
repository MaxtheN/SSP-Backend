<template>
   <div v-if="application && application.application">
      <b-row>
         <b-col sm="12" md="6" lg="6">
            <form-input-hrm :label="$t('docnumber')" disabled v-model="application.application.docNumber" />
         </b-col>
         <b-col sm="12" md="6" lg="6">
            <form-input-hrm :label="$t('docdate')" disabled v-model="application.application.docOn" />
         </b-col>
         <b-col sm="12" md="12" lg="12">
            <form-input-hrm :label="$t('contractor')" disabled v-model="application.application.contractor" />
         </b-col>
         <b-col sm="12" md="6" lg="6">
            <form-input-hrm :label="$t('contractorInn')" disabled v-model="application.application.contractorInn" />
         </b-col>
         <b-col sm="12" md="6" lg="6">
            <form-input-hrm :label="$t('director')" disabled v-model="application.application.contractorDirector" />
         </b-col>
         <b-col sm="12" md="6" lg="6">
            <form-input-hrm :label="$t('region')" disabled v-model="application.application.region" />
         </b-col>
         <b-col sm="12" md="6" lg="6">
            <form-input-hrm :label="$t('district')" disabled v-model="application.application.district" />
         </b-col>
         <b-col sm="12" md="6" lg="6">
            <form-input-hrm :label="$t('corruptionReviewType')" disabled v-model="application.corruptionReviewType" />
         </b-col>
         <b-col sm="12" md="6" lg="6">
            <form-input-hrm
               :label="$t('contractorActivityType')"
               disabled
               v-model="application.contractorActivityType"
            />
         </b-col>
         <b-col sm="12" md="6" lg="6">
            <form-input-hrm
               :label="$t('contractorUnionActivityType')"
               disabled
               v-model="application.contractorUnionActivityType"
            />
         </b-col>
         <b-col sm="12" md="6" lg="6">
            <form-input-hrm :label="$t('unionMemberCount')" disabled v-model="application.unionMemberCount" />
         </b-col>

         <b-col sm="12" md="6" lg="6">
            <form-input-hrm :label="$t('prevYearlyEarnings')" disabled v-model="application.prevYearlyEarnings" />
         </b-col>
         <b-col sm="12" md="6" lg="6">
            <form-input-hrm :label="$t('avgEmployeesCount')" disabled v-model="application.avgEmployeesCount" />
         </b-col>
         <b-col cols="12">
            <form-input-hrm :label="$t('details')" disabled v-model="application.comment" />
         </b-col>

         <!--employees table  -->
         <hr class="w-100 mx-1" />
         <b-col cols="12">
            <h4>{{ $t('employeesInfo') }}</h4>
         </b-col>
         <b-col cols="12">
            <table class="priceTable w-100">
               <thead>
                  <tr>
                     <th>{{ $t('employee') }}</th>
                     <th>{{ $t('position') }}</th>
                     <th>{{ $t('phoneNumber') }}</th>
                     <th>{{ $t('email') }}</th>
                  </tr>
               </thead>
               <tbody>
                  <tr v-for="(tab, j) in application.employees" :key="j + 'tab'">
                     <td>{{ tab.person }}</td>
                     <td>{{ tab.position }}</td>
                     <td>{{ tab.phoneNumber }}</td>
                     <td>{{ tab.email }}</td>
                  </tr>
               </tbody>
            </table>
         </b-col>

         <!--tables table  -->
         <hr class="w-100 mx-1" />
         <b-col cols="12">
            <h4>{{ $t('tablesInfo') }}</h4>
         </b-col>
         <b-col cols="12">
            <table class="priceTable w-100">
               <thead>
                  <tr>
                     <th>{{ $t('measures') }}</th>
                     <th>{{ $t('expireOn') }}</th>
                     <th>{{ $t('responsibleFio') }}</th>
                     <th>{{ $t('measuresResult') }}</th>
                  </tr>
               </thead>
               <tbody>
                  <tr v-for="(tab, j) in application.tables" :key="j + 'tab'">
                     <td>{{ tab.measures }}</td>
                     <td>{{ tab.expireOn }}</td>
                     <td>{{ tab.responsibleFio }}</td>
                     <td>{{ tab.measuresResult }}</td>
                  </tr>
               </tbody>
            </table>
         </b-col>

         <!--participates table  -->
         <hr class="w-100 mx-1" />
         <b-col cols="12">
            <h4>{{ $t('participates') }}</h4>
         </b-col>
         <b-col cols="12">
            <table class="priceTable w-100">
               <thead>
                  <tr>
                     <th>{{ $t('yearIn') }}</th>
                     <th>{{ $t('investigationOrganization') }}</th>
                     <th>{{ $t('basisForInvestigation') }}</th>
                     <th>{{ $t('investigatedPersonFio') }}</th>
                     <th>{{ $t('investigatedResult') }}</th>
                  </tr>
               </thead>
               <tbody>
                  <tr v-for="(tab, j) in application.participates" :key="j + 'tab'">
                     <td>{{ tab.yearIn }}</td>
                     <td>{{ tab.investigationOrganization }}</td>
                     <td>{{ tab.basisForInvestigation }}</td>
                     <td>{{ tab.investigatedPersonFio }}</td>
                     <td>{{ tab.investigatedResult }}</td>
                  </tr>
               </tbody>
            </table>
         </b-col>
      </b-row>
   </div>
</template>

<script>
import { BRow, BCol } from 'bootstrap-vue';
export default {
   props: {
      application: {
         type: Object,
         default: () => ({})
      }
   },
   components: {
      BRow,
      BCol
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
      border: 1px solid #f5f5f5;
   }

   tr:nth-child(even) {
      background-color: #f7f7f7;
   }
}
</style>
