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
            <form-input-hrm
               :label="$t('Xizmat Ko\'rsatish hududdi')"
               disabled
               v-model="application.application.region"
            />
         </b-col>
         <b-col sm="12" md="6" lg="6">
            <form-input-hrm :label="$t('organization2adress')" disabled v-model="application.application.district" />
         </b-col>
      </b-row>

      <template v-for="(priceTable, i) in application.groups">
         <table v-if="priceTable.tables.length" class="priceTable mt-2 w-100" :key="i + 'group'">
            <thead>
               <tr>
                  <th class="text-center" colspan="3">{{ priceTable.group }}</th>
               </tr>
            </thead>
            <tbody>
               <tr v-for="(tab, j) in priceTable.tables" :key="j + 'tab' + i">
                  <td class="text-center w-70px">
                     <feather-icon icon="CheckSquareIcon" size="16" />
                  </td>
                  <td>{{ tab.needChamberService }}</td>
                  <td class="text-primary">
                     {{ tab.offerServiceText }}
                  </td>
               </tr>
            </tbody>
         </table>
      </template>
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

   .w-70px {
      width: 70px;
   }
}
</style>
