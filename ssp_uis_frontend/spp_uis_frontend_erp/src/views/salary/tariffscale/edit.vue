<template>
   <b-overlay :show="show">
      <b-card>
         <validation-observer ref="ValidationDTO">
            <b-row>
               <b-col sm="12" md="4">
                  <form-input v-model="Data.code" required :label="$t('kode')" />
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
               <b-col sm="12" md="4" class="mt-1">
                  <form-select
                     :options="TariffScaleTypeList"
                     v-model="Data.tariffScaleTypeId"
                     requitred-star
                     :label="$t('tariffScaleType')"
                  ></form-select>
               </b-col>
               <b-col sm="12" md="4" class="mt-1">
                  <form-select
                     :options="MinimumValueTypeList"
                     v-model="Data.minimumValueTypeId"
                     requitred-star
                     :label="$t('minimumValueType')"
                  ></form-select>
               </b-col>
            </b-row>
         </validation-observer>
      </b-card>

      <b-card>
         <validation-observer ref="ValidationTabrow">
            <b-row>
               <b-col sm="12" md="4">
                  <form-input-hrm
                     v-model="tabrow.orderCode"
                     :label="$t('orderCode')"
                     rules="required"
                     :placeholder="$t('orderCode')"
                  />
               </b-col>
               <b-col sm="12" md="4">
                  <form-input-hrm
                     v-model="tabrow.rankCode"
                     :label="$t('rankCode')"
                     rules="required|max:5"
                     :placeholder="$t('rankCode')"
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
            :fields="TablesField"
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
                     <feather-icon style="margin-right: 5px" @click="EditTabrow(item)" icon="EditIcon"></feather-icon>
                  </b-link>
                  <b-link class="text-danger">
                     <feather-icon @click="DeleteTabrow(index)" icon="Trash2Icon"></feather-icon>
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
// service
import ManualService from '@/services/others/manual.service';
import TariffScaleService from '@/services/hrm/tariffscale.service';
// components
import { BOverlay, BCard, BRow, BCol, BTable, BButton, BLink } from 'bootstrap-vue';
import FormInputTranslate from '@/components/translates/FormInputTranslate.vue';

const defaultTableRow = {
   id: 0,
   rankCode: '',
   orderCode: ''
};

export default {
   components: {
      BOverlay,
      BCard,
      BRow,
      BCol,
      BButton,
      BTable,
      BLink,
      FormInputTranslate
   },
   name: 'TariffScaleEdit',
   data() {
      return {
         show: false,
         TariffScaleTypeList: [],
         MinimumValueTypeList: [],
         loadingButton: false,
         saveLoading: false,
         Data: {
            id: 0,
            code: null,
            shortName: null,
            fullName: null,
            tariffScaleTypeId: 0,
            minimumValueTypeId: 0,
            translates: [],
            tables: []
         },
         tabrow: { ...defaultTableRow },
         TablesField: [
            {
               key: 'orderCode',
               label: this.$t('orderCode'),
               sortable: true
            },
            {
               key: 'rankCode',
               label: this.$t('rankCode'),
               sortable: true
            },
            {
               key: 'actions',
               label: this.$t('actions'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            }
         ]
      };
   },
   created() {
      this.show = true;
      TariffScaleService.Get(this.$route.params.id)
         .then((res) => {
            this.Data = res.data;
            if (!Array.isArray(res.data?.tables)) {
               this.Data.tables = [];
            }
         })
         .catch((error) => {
            this.makeToast(error.response.data.errors, 'danger');
         })
         .finally(() => {
            this.show = false;
         });

      ManualService.TariffScaleTypeSelectList({})
         .then((res) => {
            if (Array.isArray(res.data)) {
               this.TariffScaleTypeList = res.data;
            }
         })
         .catch((error) => {
            this.makeToast(error.response.data.errors, 'danger');
         });

      ManualService.MinimumValueTypeSelectList()
         .then((res) => {
            this.MinimumValueTypeList = res.data;
         })
         .catch((error) => {
            this.makeToast(error.response.data.errors, 'danger');
         });
   },
   methods: {
      DeleteTabrow(index) {
         this.Data.tables.splice(index, 1);
      },
      EditTabrow(item) {
         this.editedIndex1 = this.Data.tables.indexOf(item);
         this.tabrow = Object.assign({}, item);
      },
      AddTabrow() {
         this.$refs.ValidationTabrow.validate().then((success) => {
            if (success) {
               if (this.editedIndex1 > -1) {
                  Object.assign(this.Data.tables[this.editedIndex1], this.tabrow);
               } else {
                  this.Data.tables.push(this.tabrow);
               }
               this.$refs.ValidationTabrow.reset();
               this.tabrow = { ...defaultTableRow };
            }
         });
      },
      SaveData() {
         this.$refs.ValidationDTO.validate().then((success) => {
            if (success) {
               this.saveLoading = true;
               TariffScaleService.Update(this.Data)
                  .then((res) => {
                     this.makeToast(this.$t('SaveSuccess'), 'success');
                     this.$router.push({ name: 'TariffScale' });
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
<style scoped>
input {
   margin: 0.4rem;
}
</style>
