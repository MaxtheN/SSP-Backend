<template>
   <b-overlay :show="show">
      <b-row>
         <b-col sm="12" md="12" lg="12">
            <b-card>
               <validation-observer ref="ValidationDTO">
                  <b-row>
                     <b-col sm="12" md="4" class="mb-1">
                        <form-input v-model.number="Data.title" :label="$t('title')" />
                     </b-col>
                     <b-col sm="12" md="4" class="mb-1">
                        <form-input v-model.number="Data.smsText" required :label="$t('smsText')" />
                     </b-col>

                     <b-col sm="12" md="4" class="mb-1">
                        <form-select :options="TableList" required-star v-model="Data.tableId" label="Table" />
                     </b-col>
                     <b-col sm="12" md="4" class="mb-1">
                        <form-select :options="StatusList" v-model="Data.fromStatusId" label="formStatus" />
                     </b-col>
                     <b-col sm="12" md="4" class="mb-1">
                        <form-select :options="StatusList" v-model="Data.toStatusId" label="toStatus" />
                     </b-col>

                     <b-col sm="12" md="4" class="mb-1">
                        <form-select :options="StateList" v-model="Data.stateId" label="Status" />
                     </b-col>
                  </b-row>
                  <b-row>
                     <b-col sm="12" md="6" lg="6" class="text-left"></b-col>
                     <b-col sm="12" md="6" lg="6" class="text-right">
                        <b-button :disabled="saveLoading" @click="SaveData" size="sm" variant="outline-success">
                           <feather-icon icon="CheckIcon"></feather-icon>
                           {{ $t('Save') }}
                        </b-button>
                     </b-col>
                  </b-row>
               </validation-observer>
            </b-card>
         </b-col>
      </b-row>
   </b-overlay>
</template>
<script>
// service
import ManualService from '@/services/others/manual.service';
import SendSmsConfigService from '@/services/notify/sendsmsconfig.service';
// components
import {
   BOverlay,
   BCard,
   BRow,
   BCol,
   BFormInput,
   BTable,
   BButton,
   BLink,
   BFormGroup,
   BModal,
   BInputGroup,
   BInputGroupAppend,
   BFormCheckbox,
   BFormTextarea
} from 'bootstrap-vue';
import FormInputTranslate from '@/components/translates/FormInputTranslate.vue';
export default {
   components: {
      BOverlay,
      BCard,
      BRow,
      BCol,
      BFormInput,
      BButton,
      BTable,
      BLink,
      BFormGroup,
      BModal,
      BInputGroup,
      BInputGroupAppend,
      BFormCheckbox,
      BFormTextarea,
      FormInputTranslate
   },
   name: 'Edit',
   data() {
      return {
         show: false,
         StateList: [],
         TableList: [],
         StatusList:[],
         saveLoading: false,
         Data: {
            title: '',
            smsText: '',
            tableId: 0,
            fromStatusId: 0,
            toStatusId: 0
         }
      };
   },
   created() {
      this.show = true;
      SendSmsConfigService.Get(this.$route.params.id)
         .then((res) => {
            this.Data = res.data;
         })
         .catch((error) => {
            this.showApiError(error);
         })
         .finally(() => {
            this.show = false;
         });

      ManualService.StateSelectList()
         .then((res) => {
            this.StateList = res.data;
         })
         .catch((error) => {
            this.showApiError(error);
         });

      ManualService.StatusSelectList()
         .then((res) => {
            this.StatusList = res.data;
         })
         .catch((error) => {
            this.showApiError(error);
         });

      ManualService.TableSelectList()
         .then((res) => {
            this.TableList = res.data;
         })
         .catch((error) => {
            this.showApiError(error);
         });
   },
   methods: {
      SaveData() {
         this.$refs.ValidationDTO.validate().then((success) => {
            if (success) {
               this.saveLoading = true;
               SendSmsConfigService.Update(this.Data)
                  .then((res) => {
                     this.makeToast(this.$t('SaveSuccess'), 'success');
                     this.$router.push({ name: 'SendSmsConfig' });
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
