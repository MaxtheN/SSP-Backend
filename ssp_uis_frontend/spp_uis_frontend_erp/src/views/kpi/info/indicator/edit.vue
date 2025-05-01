<template>
   <b-overlay :show="show">
      <b-card>
         <validation-observer ref="ValidationDTO">
            <b-row>
               <b-col sm="12" md="2">
                  <form-input v-model="Data.code" required :label="$t('kode')" />
               </b-col>
               <b-col sm="12" md="2">
                  <form-input v-model="Data.orderCode" required :label="$t('orderCode')" />
               </b-col>

               <b-col sm="12" md="4">
                  <form-select
                     :options="DepartmentList"
                     required-star
                     v-model="Data.departmentId"
                     label="Department"
                  ></form-select>
               </b-col>
               <b-col sm="12" md="4">
                  <form-select
                     :options="uniteOfMeasureList"
                     required-star
                     v-model="Data.uniteOfMeasureId"
                     label="uniteOfMeasure"
                  ></form-select>
               </b-col>
               <b-col sm="12" md="4">
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
               <b-col sm="12" md="4">
                  <form-input-translate
                     v-model="Data.fullName"
                     @update:translates="(e) => (Data.translates = e)"
                     :translates="Data.translates"
                     column-name="full_name"
                     required
                     :label="$t('fullname')"
                     :placeholder="$t('fullname')"
                  />
               </b-col>
               <b-col sm="12" md="4">
                  <form-select :options="StateList" v-model="Data.stateId" label="Status"></form-select>
               </b-col>
            </b-row>
         </validation-observer>
      </b-card>
      <b-card>
         <validation-observer ref="ValidationTabrow">
            <b-row>
               <b-col sm="12" md="3">
                  <form-input v-model="tabrow.code" required :label="$t('kode')" :placeholder="$t('kode')" />
               </b-col>
               <b-col sm="12" md="3">
                  <form-input
                     v-model="tabrow.fullName"
                     required
                     :label="$t('fullname')"
                     :placeholder="$t('fullname')"
                  />
               </b-col>
               <b-col sm="12" md="3">
                  <form-input
                     v-model="tabrow.shortName"
                     required
                     :label="$t('shortname')"
                     :placeholder="$t('shortname')"
                  />
               </b-col>
               <b-col sm="12" md="1" class="mt-2">
                  <b-button variant="primary" @click="AddTabrow">
                     <feather-icon icon="PlusIcon" size="14"></feather-icon>
                  </b-button>
               </b-col>
            </b-row>
         </validation-observer>

         <b-table
            class="mt-2"
            :fields="TableFeilds"
            hover
            bordered
            show-empty
            :empty-text="$t('NotFound')"
            small
            responsive="sm"
            :items="Data.tables"
         >
            <template #cell(actions)="{ item, index }">
               <div class="text-center">
                  <b-link>
                     <feather-icon
                        style="margin-right: 5px"
                        @click="EditTabrow(item, index)"
                        icon="EditIcon"
                     ></feather-icon>
                  </b-link>
                  <b-link class="text-danger">
                     <feather-icon @click="Data.tables.splice(index, 1)" icon="Trash2Icon"></feather-icon>
                  </b-link>
               </div>
            </template>
            <template #cell(order)="{ index }">
               <span>{{ index + 1 }}</span>
            </template>
         </b-table>

         <b-row class="mt-3">
            <b-col sm="12" md="6" lg="6" class="text-left"></b-col>
            <b-col sm="12" md="6" lg="6" class="text-right">
               <b-button :disabled="saveLoading" @click="SaveData" size="sm" variant="outline-success">
                  <feather-icon icon="CheckIcon"></feather-icon>
                  {{ $t('Save') }}
               </b-button>
            </b-col>
         </b-row>
      </b-card>
   </b-overlay>
</template>
<script>
import ManualService from '@/services/others/manual.service';
import IndicatorService from '@/services/kpi/indicator.service';
import IndicatorDepartmentService from '@/services/kpi/indicatordepartment.service';
import { BOverlay, BCard, BRow, BCol, BButton, BModal, BTable, BLink, BSpinner } from 'bootstrap-vue';
import FormInputTranslate from '@/components/translates/FormInputTranslate.vue';
import UniteOfMeasureService from '@/services/info/uniteofmeasure.service';

export default {
   components: {
      BOverlay,
      BTable,
      BCard,
      BModal,
      BRow,
      BCol,
      BButton,
      BLink,
      FormInputTranslate,
      BSpinner
   },
   data() {
      return {
         showModal: true,
         DepartmentList: [],
         show: false,
         saveLoading: false,
         StateList: [],
         tabrow: {},
         uniteOfMeasureList: [],
         Tabindex: null,
         TableFeilds: [
            {
               key: 'code',
               label: this.$t('code'),
               thClass: 'text-center'
            },
            {
               key: 'fullName',
               label: this.$t('fullname'),
               thClass: 'text-center'
            },
            {
               key: 'shortName',
               label: this.$t('shortname'),
               thClass: 'text-center'
            },

            {
               key: 'actions',
               label: this.$t('actions'),
               thClass: 'text-center'
            }
         ],
         Data: {
            uniteOfMeasureId: null,
            departmentId: null,
            code: '',
            orderCode: '',
            shortName: '',
            fullName: '',
            departmentId: 0,
            translates: [],
            tables: []
         }
      };
   },
   created() {
      ManualService.StateSelectList()
         .then((res) => {
            this.StateList = res.data;
         })
         .catch((error) => {
            this.showApiError(error);
         });

      IndicatorDepartmentService.GetAsSelectList().then((res) => {
         if (Array.isArray(res.data)) {
            this.DepartmentList = res.data;
         }
      });
      UniteOfMeasureService.GetAsSelectList()
         .then((res) => {
            if (Array.isArray(res.data)) {
               this.uniteOfMeasureList = res.data;
            }
         })
         .catch(this.showApiError);

      IndicatorService.Get(this.$route.params.id, this.$route.query.tab)
         .then((res) => {
            this.Data = res.data;
         })
         .catch((error) => {
            this.showApiError(error);
         })
         .finally(() => {
            this.show = false;
         });
   },
   methods: {
      AddTabrow() {
         this.$refs.ValidationTabrow.validate().then((success) => {
            if (success) {
               if (this.Tabindex == null) {
                  this.Data.tables.push(this.tabrow);
                  this.tabrow = {};
                  this.$refs.ValidationTabrow.reset();
               } else {
                  Object.assign(this.Data.tables[this.Tabindex], this.tabrow);
                  this.tabrow = {};
                  this.$refs.ValidationTabrow.reset();
               }
            }
         });
      },
      EditTabrow(item, index) {
         this.tabrow = { ...item };
         this.Tabindex = index;
      },

      SaveData() {
         this.$refs.ValidationDTO.validate().then((success) => {
            if (success) {
               this.saveLoading = true;
               IndicatorService.Update(this.Data, this.$route.query.tab)
                  .then(() => {
                     this.makeToast(this.$t('SaveSuccess'), 'success');
                     this.$router.push({ name: 'Indicator' });
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
