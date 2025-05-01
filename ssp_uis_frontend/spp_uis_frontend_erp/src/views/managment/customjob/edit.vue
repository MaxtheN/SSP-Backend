<template>
   <b-overlay :show="show">
      <b-card>
         <validation-observer ref="ValidationDTO">
            <b-row class="align-items-center">
               <b-col sm="12" md="3">
                  <form-select
                     v-model="Data.jobTypeId"
                     :options="CustomJobTypeSelectList"
                     :label="$t('JobType')"
                     :placeholder="$t('JobType')"
                     required-star
                  />
               </b-col>
               <b-col sm="12" md="3">
                  <form-picker v-model="Data.extendData" type="year" format="YYYY" :label="$t('extendData')" :placeholder="$t('extendData')" />
               </b-col>
               <b-col cols="12" md="3">
                  <form-select
                     :options="RegionList"
                     v-model="Data.regionId"
                     @input="ChangeRegion"
                     :label="$t('Oblast')"
                  ></form-select>
               </b-col>
               <b-col cols="12" md="3">
                  <form-select
                     :options="DistrictList"
                     :placeholder="$t('ChooseBelow')"
                     :label="$t('Region')"
                     v-model="Data.districtId"
                  />
               </b-col>
               <b-col sm="12" md="3">
                  <b-form-checkbox v-model="Data.isForceUpdate">
                     {{ $t('isForceUpdate') }}
                  </b-form-checkbox>
               </b-col>
               <b-col sm="12" md="3">
                  <form-select :options="StateList" v-model="Data.stateId" label="Status"></form-select>
               </b-col>

               <!-- save button -->
               <b-col cols="12" class="text-right">
                  <b-button :disabled="saveLoading" @click="SaveData" variant="outline-success">
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
import CustomJobService from '@/services/managment/customjob.service';
// components
import { BOverlay, BCard, BRow, BCol, BButton, BFormCheckbox } from 'bootstrap-vue';
import FormInputTranslate from '@/components/translates/FormInputTranslate.vue';
import ManualService from '@/services/others/manual.service';
import DistrictService from '@/services/info/district.service';
import RegionService from '@/services/info/region.service';

export default {
   components: {
      BOverlay,
      BCard,
      BRow,
      BCol,
      BButton,
      BFormCheckbox,
      FormInputTranslate
   },
   data() {
      return {
         show: false,
         saveLoading: false,
         RegionList: [],
         DistrictList: [],
         StateList: [],
         CustomJobTypeSelectList: [],
         Data: {
            jobTypeId: null,
            isForceUpdate: true,
            extendData: '',
            regionId: null,
            districtId: null,
            id: 0
         }
      };
   },
   created() {
      this.GetCustomJob();
      RegionService.GetAsSelectList(211).then((res) => {
         this.RegionList = res.data;
      });
      ManualService.StateSelectList().then((res) => {
         this.StateList = res.data;
      });

      ManualService.CustomJobTypeSelectList().then((res) => {
         this.CustomJobTypeSelectList = res.data;
      });
   },
   methods: {
      ChangeRegion() {
         if (this.Data.regionId) {
            this.Data.districtId = null;
            this.GetDistrict();
         }
      },
      GetDistrict() {
         if (this.Data.regionId) {
            DistrictService.GetAsSelectList(this.Data.regionId).then((res) => {
               this.DistrictList = res.data;
            });
         } else {
            this.Data.districtId = null;
            this.DistrictList = [];
         }
      },
      GetCustomJob() {
         CustomJobService.Get(this.$route.params.id)
            .then((res) => {
               this.Data = res.data;
               this.GetDistrict()
            })
            .catch((error) => {
               this.showApiError(error);
            })
            .finally(() => {
               this.show = false;
            });
      },
      SaveData() {
         this.$refs.ValidationDTO.validate().then((success) => {
            if (success) {
               this.saveLoading = true;
               CustomJobService.Update(this.Data)
                  .then(() => {
                     this.makeToast(this.$t('SaveSuccess'), 'success');
                     this.$router.push({ name: 'CustomJob' });
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

<style>
.col-form-label,
label {
   line-height: initial;
}
</style>
