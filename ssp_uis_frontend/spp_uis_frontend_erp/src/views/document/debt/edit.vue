<template>
   <b-overlay :show="show">
      <b-card class="mt-2">
         <validation-observer ref="ValidationDTO">
            <b-row>
               <b-col sm="12" md="3">
                  <form-input-hrm v-model="Data.docNumber" :label="$t('docnumber')" :placeholder="$t('docnumber')" />
               </b-col>

               <b-col sm="12" md="3" class="text-left">
                  <label for>{{ $t('docdate') }}</label>
                  <date-picker
                     v-model="Data.docOn"
                     style="width: 100%"
                     size="sm"
                     lang="ru"
                     :placeholder="$t('docdate')"
                     value-type="format"
                     format="DD.MM.YYYY"
                  ></date-picker>
               </b-col>
               <b-col sm="12" md="4">
                  <form-input-hrm v-model="Data.details" :label="$t('details')" :placeholder="$t('details')" />
               </b-col>
               <b-col sm="12" md="2">
                  <b-button variant="primary" @click="showModal = !showModal" class="mt-2">
                     <b-icon-plus></b-icon-plus> {{ $t('Add') }}
                  </b-button>
               </b-col>
            </b-row>

            <!-- table -->
            <b-table
               :items="tableData"
               :fields="fields"
               responsive
               no-border-collapse
               :busy="isBusy"
               show-empty
               :empty-text="$t('NotFound')"
            >
               <template #cell(id)="{ item, index }">
                  {{ index + 1 }}
               </template>
               <template #cell(status)="{ item }">
                  <b-badge :variant="getColor(item)">{{ item.status }}</b-badge>
               </template>

               <template #cell(debtAmount)="{ item }">
                  {{ currency(item.debtAmount) }}
               </template>
               <template #cell(entitlementAmount)="{ item }">
                  {{ currency(item.entitlementAmount) }}
               </template>

               <template #cell(actions)="{ item, index }">
                  <b-link @click="EditTable(item, index)" style="margin-right: 5px; cursor: pointer; color: blue">
                     <feather-icon icon="EditIcon"></feather-icon>
                  </b-link>
                  <b-link class="text-danger" @click="$refs['DeleteModal' + index].show()" style="cursor: pointer">
                     <feather-icon icon="TrashIcon"></feather-icon>
                  </b-link>
                  <b-modal
                     :ref="'DeleteModal' + index"
                     :cancel-title="$t('Cancel')"
                     :ok-title="$t('Accept')"
                     cancel-variant="danger"
                     ok-variant="success"
                     @ok="TableDelete(item, index)"
                  >
                     <template #modal-title>
                        {{ $t('Accept') }}
                        <b-spinner v-if="deleteLoading" small></b-spinner>
                     </template>
                     <b-card-text>
                        <h5>ID : {{ index + 1 }}</h5>
                        <h5>{{ $t('WantDelete') }}</h5>
                     </b-card-text>
                  </b-modal>
               </template>
            </b-table>
            <b-row>
               <b-col cols="12" class="text-right">
                  <b-button :disabled="saveLoading" @click="SaveDataBack" variant="outline-success">
                     <feather-icon icon="CheckIcon"></feather-icon>
                     {{ $t('Save') }}
                  </b-button>
               </b-col>
            </b-row>
            <!-- Modal -->

            <b-modal size="lg" v-model="showModal" hide-footer hide-close-button>
               <b-card>
                  <!-- {{ tableItem }} -->
                  <b-row>
                     <b-col sm="12" md="6">
                        <div class="form-group">
                           <form-input v-model="tableItem.contractorInn" :label="$t('inn')" :mask="'#########'">
                              <b-input-group-append>
                                 <b-button variant="primary" @click="GetBycontractorInn">
                                    <feather-icon icon="SearchIcon"></feather-icon>
                                 </b-button>
                              </b-input-group-append>
                           </form-input>
                        </div>
                     </b-col>
                     <b-col sm="12" md="6">
                        <form-input-hrm
                           v-model="tableItem.contractor"
                           :label="$t('contractorName')"
                           :placeholder="$t('contractorName')"
                        />
                     </b-col>
                  </b-row>
                  <div class="mt-5 flex-column align-items-center">
                     <div class="justify-content-start d-flex align-items-center">
                        <label for>{{ $t('prtnContractType') }}</label>
                        <v-select
                           :options="typeSelectList"
                           v-model="tableItem.applicationTypeId"
                           :placeholder="$t('ChooseBelow')"
                           label="text"
                           @input="changeAplicationType"
                           :reduce="(item) => item.value"
                           class="w-50 ml-2"
                        ></v-select>
                     </div>
                     <div class="justify-content-start d-flex align-items-center mt-2">
                        <label for>{{ $t('Haqdorlik') }}</label>
                        <form-input-hrm
                           class="w-50 ml-5"
                           v-model="tableItem.entitlementAmount"
                           :placeholder="$t('Haqdorlik')"
                        />
                     </div>
                     <div class="justify-content-start d-flex align-items-center mt-1">
                        <label for>{{ $t('Qarzdorlik') }}</label>
                        <form-input-hrm
                           class="w-50 ml-5"
                           v-model="tableItem.debtAmount"
                           :placeholder="$t('Qarzdorlik')"
                        />
                     </div>
                  </div>
                  <b-row class="mt-2">
                     <b-col sm="12" md="3">
                        <b-button variant="success" @click="SaveData" class="mt-2 w-100">
                           {{ $t('Add') }}
                        </b-button>
                     </b-col>
                     <b-col sm="12" md="3">
                        <b-button variant="danger" @click="showModal = !showModal" class="mt-2 w-100">
                           {{ $t('back') }}
                        </b-button>
                     </b-col>
                     <b-col></b-col>
                  </b-row>
               </b-card>
            </b-modal>
         </validation-observer>
      </b-card>
   </b-overlay>
</template>

<script>
import { ValidationProvider } from 'vee-validate';
import ContractorListSelect from '@/views/components/info/ContractorListSelect.vue';
import {
   BOverlay,
   BCardText,
   BIconCheckLg,
   BCard,
   BRow,
   BCol,
   BButton,
   BFormFile,
   BInputGroupAppend,
   BFormTextarea,
   BLink,
   BTooltip,
   BModal,
   BTable,
   BBadge,
   BFormSelect,
   VBTooltip,
   BIconPlus
} from 'bootstrap-vue';
import MemshipDebtService from '@/services/document/debt.service';
import ContractorService from '@/services/info/contractor.service';
export default {
   components: {
      BLink,
      BCardText,
      BTooltip,
      BOverlay,
      BInputGroupAppend,
      VBTooltip,
      BCard,
      BRow,
      BCol,
      BButton,
      ContractorListSelect,
      BFormFile,
      BIconCheckLg,
      BFormTextarea,
      BIconPlus,
      BModal,
      BFormSelect,
      BTable,
      BBadge
   },
   data() {
      return {
         saveLoading: false,
         deleteLoading: false,
         show: false,
         showModal: false,
         isBusy: false,
         id2: null,
         typeSelectList: [
            {
               value: 3,
               text: "A'zolik shartnomasi uchun ariza",
               orderCode: '3'
            },
            {
               value: 7,
               text: 'Pulli xizmatlar',
               orderCode: '7'
            }
         ],
         tableItem: {
            contractorInn: '',
            contractor: '',
            applicationType: '',
            contractorPinfl: null,
            contractorId: null,
            debtAmount: 0,
            entitlementAmount: 0,
            applicationTypeId: null
         },
         Data: {
            docOn: '',
            docNumber: null,
            details: '',
            tables: []
         },
         tableData: [],
         fields: [
            {
               key: 'id',
               label: this.$t('id'),
               sortable: false,
               tdClass: 'text-center',
               thClass: 'text-center'
            },
            {
               key: 'contractorInn',
               label: this.$t('inn'),
               sortable: false,
               tdClass: 'text-center',
               thClass: 'text-center'
            },
            {
               key: 'contractor',
               label: this.$t('contractorName'),
               sortable: false,
               tdClass: 'text-center',
               thClass: 'text-center'
            },
            {
               key: 'applicationType',
               label: this.$t('prtnContractType'),
               sortable: false,
               tdClass: 'text-center',
               thClass: 'text-center'
            },
            {
               key: 'entitlementAmount',
               label: this.$t('Haqdorlik'),
               sortable: false,
               tdClass: 'text-center',
               thClass: 'text-center',
               thStyle: {
                  minWidth: '200px'
               }
            },
            {
               key: 'debtAmount',
               label: this.$t('Қарздорлик'),
               sortable: false,
               tdClass: 'text-center',
               thClass: 'text-center',
               thStyle: {
                  minWidth: '200px'
               }
            },
            {
               key: 'actions',
               label: this.$t('actions'),
               sortable: false,
               tdClass: 'text-center',
               thClass: 'text-center'
            }
         ]
      };
   },

   created() {
      MemshipDebtService.Get(this.$route.params.id)
         .then((res) => {
            this.Data = res.data;
            this.tableData = [...res.data.tables];
         })
         .catch((error) => {
            this.makeToast(error.response.data.errors, 'danger');
         })
         .finally(() => {
            this.show = false;
         });
   },
   mounted() {
      if (!this.steps || this.steps.length == 0) {
         return;
      }

      this.steps = this.steps.map((s, i) => ({
         number: i + 1,
         selected: false,
         ...s
      }));

      this.steps[0].selected = true;

      this.$nextTick(() => {
         this.calculateBarPosition();
      });

      window.addEventListener('resize', this.calculateBarPosition);
   },

   methods: {
      GetBycontractorInn() {
         this.innLoading = true;
         ContractorService.SearchByInnPnfl(this.tableItem.contractorInn)
            .then((res) => {
               this.tableItem.contractor = res.data.shortName;
               this.tableItem.contractorId = res.data.id;
            })
            .catch(this.showApiError);
      },

      SaveData() {
         this.showModal = !this.showModal;

         if (this.id2 != null) {
            this.tableData.splice(this.id2, 1, this.tableItem);
            this.id2 = null;
         } else {
            this.tableData.push({
               ...this.tableItem,
               id2: Math.trunc(Math.random() * 1000),
               applicationType: this.typeSelectList.filter((item) => item.value == this.tableItem.applicationTypeId)[0]
                  .text
            });
            this.id2 = null;
         }
         this.tableItem = {
            contractorInn: '',
            contractor: '',
            applicationType: '',
            contractorPinfl: null,
            contractorId: null,
            debtAmount: 0,
            entitlementAmount: 0,
            applicationTypeId: null
         };
      },

      EditTable(item, index) {
         this.showModal = true;
         const editdata = this.tableData.filter((el) => el.contractorId == item.contractorId);
         this.tableItem = { ...editdata[0] };
         this.id2 = index;
         console.log(index);
      },

      SaveDataBack() {
         console.log({ ...this.Data, tables: this.tableData });
         MemshipDebtService.Update({ ...this.Data, tables: this.tableData })
            .then((res) => {
               this.$router.push({ name: 'Debt' });
            })
            .catch(this.showApiError);
      },
      TableDelete(item, index) {
         this.tableData = this.tableData.filter((el, idx) => {
            return index != idx;
         });
      },
      changeAplicationType(e) {
         this.tableItem.applicationType = this.typeSelectList.filter((item) => item.value == e)[0].text;
      }
   }
};
</script>

<style lang="scss" scoped></style>
