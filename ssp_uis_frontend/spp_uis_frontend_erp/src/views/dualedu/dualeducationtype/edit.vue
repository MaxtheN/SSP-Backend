<template>
   <b-overlay :show="show">
      <b-row>
         <b-col sm="12" md="12" lg="12">
            <b-card>
               <validation-observer ref="ValidationDTO">
                  <b-row>
                     <b-col sm="12" md="4" class="mb-1">
                        <form-input v-model="Data.orderCode" :label="$t('orderCode')" />
                     </b-col>
                     <b-col sm="12" md="4" class="mb-1">
                        <form-input v-model="Data.code" required :label="$t('kode')" />
                     </b-col>
                     <b-col sm="12" md="4" class="mb-1">
                        <form-input-translate
                           v-model="Data.fullName"
                           @update:translates="(e) => (Data.translates = e)"
                           :translates="Data.translates"
                           column-name="full_name"
                           required
                           @input="(v) => (Data.shortName = v)"
                           :label="$t('fullname')"
                           :placeholder="$t('fullname')"
                        />
                     </b-col>
                     <b-col sm="12" md="4" class="mb-1">
                        <form-input-translate
                           v-model="Data.shortName"
                           @update:translates="(e) => (Data.translates = e)"
                           :translates="Data.translates"
                           column-name="short_name"
                           required
                           :label="$t('shortname')"
                           :placeholder="$t('shortname')"
                        />
                     </b-col>

                     <b-col sm="12" md="4" class="mb-1">
                        <form-select :options="StateList" v-model="Data.stateId" label="Status"></form-select>
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
import DualEducationTypeService from '@/services/dualedu/dualeducationtype.service';
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
         TabrowModal: false,
         saveLoading: false,
         Data: {
            orderCode: '',
            code: '',
            shortName: '',
            fullName: '',
            stateId: 0,
            translates: []
         }
      };
   },
   created() {
      this.show = true;
      DualEducationTypeService.Get(this.$route.params.id)
         .then((res) => {
            this.Data = res.data;
         })
         .catch((error) => {
            this.makeToast(error.response.data.errors, 'danger');
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
   },
   methods: {
      SaveData() {
         this.$refs.ValidationDTO.validate().then((success) => {
            if (success) {
               this.saveLoading = true;
               DualEducationTypeService.Update(this.Data)
                  .then((res) => {
                     this.makeToast(this.$t('SaveSuccess'), 'success');
                     this.$router.push({ name: 'DualEducationType' });
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
