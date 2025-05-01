<template>
   <b-overlay :show="show">
      <b-card>
         <validation-observer ref="ValidationDTO">
            <b-row>
               <b-col sm="12" md="4">
                  <form-picker v-model="Data.dateOn" required :label="$t('ondate')" />
               </b-col>
               <b-col sm="12" md="4">
                  <form-select
                     :options="TariffScaleList"
                     v-model="Data.tariffScaleId"
                     required-star
                     :label="$t('TariffScale')"
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
                  <form-select
                     :options="RankList"
                     v-model="tabrow.rankCode"
                     requitred-star
                     :disabled="!Data.tariffScaleId"
                     :label="$t('rankCode')"
                     valueid="text"
                  ></form-select>
               </b-col>
               <b-col sm="12" md="3">
                  <form-input-hrm
                     v-model="tabrow.coef"
                     :label="$t('coef')"
                     rules="required|max:5"
                     :placeholder="$t('coef')"
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
import TariffScaleService from '@/services/hrm/tariffscale.service';
import TariffScaleCoefService from '@/services/hrm/tariffscalecoef.service';
// components
import { BOverlay, BCard, BRow, BCol, BTable, BButton, BLink } from 'bootstrap-vue';

const defaultTableRow = {
   id: 0,
   tariffScaleTableId: 0,
   rankCode: '',
   coef: null,
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
      BLink
   },
   name: 'TariffScaleCoefEdit',
   data() {
      return {
         show: false,
         TariffScaleList: [],
         RankList: [],
         loadingButton: false,
         saveLoading: false,
         Data: {
            id: 0,
            dateOn: null,
            tariffScaleId: null,
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
               key: 'coef',
               label: this.$t('coef'),
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
   watch: {
      'Data.tariffScaleId': {
         handler(newV) {
            if (newV) {
               TariffScaleService.GetTableAsSelectList(newV).then((res) => {
                  if (Array.isArray(res.data)) {
                     this.RankList = res.data;
                  }
               });
            }
         }
      }
   },
   created() {
      this.show = true;
      TariffScaleCoefService.Get(this.$route.params.id)
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

      TariffScaleService.GetAsSelectList()
         .then((res) => {
            this.TariffScaleList = res.data;
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
               this.tabrow.tariffScaleTableId = this.RankList.find((e) => e.text == this.tabrow.rankCode)?.value;

               if (this.editedIndex1 > -1) {
                  Object.assign(this.Data.tables[this.editedIndex1], this.tabrow);
               } else {
                  this.Data.tables.push(this.tabrow);
               }
               this.$refs.ValidationTabrow.reset();
               this.tabrow = { ...defaultTableRow };
               this.editedIndex1=-1
            }
         });
      },
      SaveData() {
         this.$refs.ValidationDTO.validate().then(async (success) => {
            if (success) {
               this.saveLoading = true;

               TariffScaleCoefService.Update(this.Data)
                  .then((res) => {
                     this.makeToast(this.$t('SaveSuccess'), 'success');
                     this.$router.push({ name: 'TariffScaleCoef' });
                  })
                  .catch((err) => {
                     this.showApiError(err);
                  })
                  .finally((e) => {
                     this.saveLoading = false;
                  });
            }
         });
      }
   }
};
</script>
