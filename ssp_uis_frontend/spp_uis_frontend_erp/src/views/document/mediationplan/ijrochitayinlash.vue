<template>
   <b-overlay>
      <b-card>
         <validation-observer ref="ValidationDTO">
            <b-row>
               <b-col sm="12" md="12" class="text-center mb-2"
                  ><h2>{{ $t('ijrochiga biriktirish') }}</h2></b-col
               >
               <b-col sm="12" md="4">
                  <h4>{{ $t('docnumber') }}: {{ Data.docNumber }}</h4>
               </b-col>
               <b-col sm="12" md="4"> </b-col>
               <b-col sm="12" md="4" class="text-right">
                  <h4>{{ $t('docOn') }}: {{ Data.docOn }}</h4>
               </b-col>
               <b-col sm="12" md="12">
                  <h4>
                     {{ $t('applicationDocNumber') }}: {{ Data.claimApplicationType }} {{ Data.applicaionDocNumber }} -
                     {{ Data.applicaionDocOn }}
                  </h4>
               </b-col>
               <b-col sm="12" md="12">
                  <h4>{{ $t('contractor') }}: {{ Data.contractor }}</h4>
               </b-col>
            </b-row>
            <b-row>
               <b-col sm="12" md="">
                  <EmployeeManageSelect
                     v-model="Data.employeeManageId"
                     :label="$t('ijrochi F.I.O.')"
                     :employee="Data.employeeFull"
                     :employee-id="Data.employeeId"
                     @update:data="onUpdateEmployeeManage"
                     required-star
                  />
               </b-col>
               <b-col sm="12" md="6">
                  <form-picker
                     v-model="Data.durationGivenPerformer"
                     :label="$t('berilgan muddat')"
                     :name="$t('berilgan muddat')"
                     required
                  />
               </b-col>
            </b-row>

            <b-row class="mt-2">
               <b-col cols="12" class="text-right">
                  <b-button @click="SaveData" variant="outline-success">
                     <feather-icon icon="CheckIcon"></feather-icon>
                     {{ $t('Save') }}
                  </b-button>
               </b-col>
            </b-row>
         </validation-observer>
      </b-card>
   </b-overlay>
</template>
<script>
// service
import ClaimApplicationService from '@/services/document/claimapplication.service';
// components
import { BOverlay, BCard, BRow, BCol, BTable, BButton, BLink } from 'bootstrap-vue';
import ClaimThemeService from '@/services/info/claimtheme.service';
import EmployeeManageSelect from '@/views/components/hrm/EmployeeManageSelect.vue';

export default {
   components: {
      BOverlay,
      BCard,
      BRow,
      BCol,
      BButton,
      BTable,
      BLink,
      EmployeeManageSelect
   },
   name: 'MediationPlanEdit',
   data() {
      return {
         ApplicationList: [],
         meditionAtDate: '',
         meditionAtTime: '',
         employeeList: [],
         saveLoading: false,
         Data: {
            id: 0,
            employeeManageId: null,
            durationGivenPerformer: ''
         }
      };
   },
   created() {
      this.show = true;
      // console.log(this.$route.params.id);
      ClaimApplicationService.GetList({}).then((res) => {
         this.ApplicationList = res.data.rows;
      });

      ClaimThemeService.GetAsSelectList().then((res) => {
         if (Array.isArray(res.data)) {
            this.ClaimThemeList = res.data;
         }
      });
   },

   methods: {
      onUpdateEmployeeManage(e) {
         this.Data.employeeId = e?.employeeId;
         this.Data.employeeFull = e?.employee;
      },
      SaveData() {
         this.$refs.ValidationDTO.validate().then((success) => {
            if (success) {
               this.saveLoading = true;

               ClaimApplicationService.EmployeeAttachment({ ...this.Data, id: this.$route.params.id })
                  .then(() => {
                     this.makeToast(this.$t('SaveSuccess'), 'success');
                     this.$router.push({ name: 'ClaimApplication' });
                  })
                  .catch((err) => {
                     this.showApiError(err);
                  })
                  .finally(() => {
                     this.saveLoading = false;
                  });
            }
         });
      }
   }
};
</script>
