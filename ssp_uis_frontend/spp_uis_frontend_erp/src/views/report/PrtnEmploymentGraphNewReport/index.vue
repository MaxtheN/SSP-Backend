<template>
   <b-card>
      <b-row>
         <b-col sm="12" md="2">
            <div>
               <label for>{{ $t('Oblast') }}</label>
               <v-select
                  :options="RegionList"
                  :reduce="(item) => item.value"
                  :placeholder="$t('ChooseBelow')"
                  label="text"
                  v-model="filters.regionId"
                  @input="ChangeRegion"
                  class="w-100"
               ></v-select>
            </div>
         </b-col>
         <b-col sm="12" md="2">
            <div>
               <label for>{{ $t('Region') }}</label>
               <v-select
                  :options="DistrictList"
                  :reduce="(item) => item.value"
                  :placeholder="$t('ChooseBelow')"
                  label="text"
                  v-model="filters.districtId"
                  @input="Refresh"
                  class="w-100"
               ></v-select>
            </div>
         </b-col>
         <b-col sm="12" md="2">
            <form-picker
               :label="$t('startDate')"
               v-model="filters.startDate"
               :placeholder="$t('startDate')"
               @change="Refresh"
            ></form-picker>
         </b-col>
         <b-col sm="12" md="2">
            <form-picker
               :label="$t('endDate')"
               v-model="filters.endDate"
               :placeholder="$t('endDate')"
               @change="Refresh"
            ></form-picker>
         </b-col>

         <b-col cols="12" md="4">
            <label for>{{ $t('inn') }}</label>
            <b-input-group class="text-right mb-1">
               <b-form-input v-model="filters.contractorInn" :placeholder="$t('search')" />
               <b-input-group-append>
                  <b-button @click="Refresh" variant="primary">
                     <feather-icon icon="SearchIcon" />
                  </b-button>
               </b-input-group-append>
            </b-input-group>
         </b-col>

         <b-col sm="12" md="8" class="mb-2">
            <b-button-group @click="Refresh" size="sm" class="mr-2">
               <b-button
                  @click="filters.prtnContractTypeId = null"
                  :variant="filters.prtnContractTypeId == null ? 'primary' : 'outline-primary'"
                  >{{ $t('all') }}
               </b-button>

               <b-button
                  v-for="type in PrtnContractTypeList"
                  :key="type.value"
                  @click="filters.prtnContractTypeId = type.value"
                  :variant="type.value == filters.prtnContractTypeId ? 'primary' : 'outline-primary'"
               >
                  {{ type.text }}
               </b-button>
            </b-button-group>
         </b-col>
      </b-row>
      <b-tabs pills class="mx-1 tabheader">
         <b-tab :title="$t('PrtnEmploymentGraphNewReport')" @click="Refresh" active>
            <div>
               <b-row class="my-2">
                  <b-col sm="12" md="4"
                     ><b-button @click="Print" :disabled="printLoding" variant="primary">
                        <feather-icon icon="PrinterIcon"></feather-icon>
                        {{ $t('Print') }}
                     </b-button></b-col
                  >
               </b-row>
               <div class="simple-table">
                  <b-table
                     :fields="fields"
                     :items="items"
                     show-empty
                     @sort-changed="SortChange"
                     :empty-text="$t('NotFound')"
                     :busy="isBusy"
                  >
                     <template #cell(order)="{ item, index }"> {{ index + 1 }}</template>
                     <template #cell(status)="{ item }">
                        <b-badge :variant="getColor(item)">{{ item.status }}</b-badge>
                     </template>
                     <template v-slot:table-busy>
                        <div class="text-center text-primary my-2" style="vertical-align: middle">
                           <b-spinner class="align-middle mr-2"></b-spinner>
                           <strong>{{ $t('Loading') }}</strong>
                        </div>
                     </template>
                  </b-table>
               </div>
               <div class="mx-2 mb-2">
                  <b-row>
                     <b-col
                        cols="12"
                        sm="6"
                        class="d-flex align-items-center justify-content-center justify-content-sm-start"
                     >
                        <span class="text-muted">
                           {{ $t('Showing') }} {{ firstNumber }} {{ $t('to') }} {{ lastNumber }} {{ $t('of') }}
                           {{ filters.total }}
                           {{ $t('entries') }}
                        </span>
                        <v-select
                           v-model="filters.pageSize"
                           :dir="$store.state.appConfig.isRTL ? 'rtl' : 'ltr'"
                           :options="filters.perPageOptions"
                           @input="Refresh"
                           :clearable="false"
                           class="per-page-selector d-inline-block ml-50 mr-1"
                        />
                     </b-col>
                     <!-- Pagination -->
                     <b-col
                        cols="12"
                        sm="6"
                        class="d-flex align-items-center justify-content-center justify-content-sm-end"
                     >
                        <b-pagination
                           v-model="filters.page"
                           :total-rows="filters.total"
                           :per-page="filters.pageSize"
                           first-number
                           last-number
                           @input="Refresh"
                           class="mb-0 mt-1 mt-sm-0"
                           prev-class="prev-item"
                           next-class="next-item"
                        >
                           <template #prev-text>
                              <feather-icon icon="ChevronLeftIcon" size="18" />
                           </template>
                           <template #next-text>
                              <feather-icon icon="ChevronRightIcon" size="18" />
                           </template>
                        </b-pagination>
                     </b-col>
                  </b-row>
               </div>
            </div>
         </b-tab>
         <b-tab :title="$t('PrtnApplicationByPetitionInfo')" lazy>
            <PrtnApplicationByPetitionInfo :filters="filters" />
         </b-tab>
         <b-tab :title="$t('GetPrtnApplicationByContractNewInfo')" lazy>
            <GetPrtnApplicationByContractNewInfo :filters="filters" />
         </b-tab>
         <b-tab :title="$t('GetPrtnApplicationByFullInfo')" lazy>
            <GetPrtnApplicationByFullInfo :filters="filters" />
         </b-tab>
      </b-tabs>
   </b-card>
</template>

<script>
import ReportService from '@/services/report/report.service';
import PrtnApplicationByPetitionInfo from '../PrtnApplicationByPetitionInfo/index.vue';
import GetPrtnApplicationByContractNewInfo from '../GetPrtnApplicationByContractNewInfo/index.vue';
import GetPrtnApplicationByFullInfo from '../GetPrtnApplicationByFullInfo/index.vue';
import RegionService from '@/services/info/region.service';
import PrtnContractTypeService from '@/services/info/prtncontracttype.service';
import DistrictService from '@/services/info/district.service';
import {
   BFormSelect,
   BButton,
   BPagination,
   BTable,
   BCol,
   BRow,
   BSpinner,
   BCard,
   BTooltip,
   BBadge,
   BInputGroup,
   BFormInput,
   BInputGroupAppend,
   BLink,
   BModal,
   BCardText,
   BTabs,
   BTab,
   BButtonGroup
} from 'bootstrap-vue';
export default {
   components: {
      BFormSelect,
      BButton,
      BPagination,
      BTable,
      BCol,
      BRow,
      BSpinner,
      BCard,
      BTooltip,
      BBadge,
      BInputGroup,
      BFormInput,
      BInputGroupAppend,
      BLink,
      BModal,
      BCardText,
      BTabs,
      BTab,
      PrtnApplicationByPetitionInfo,
      GetPrtnApplicationByContractNewInfo,
      GetPrtnApplicationByFullInfo,
      BButtonGroup
   },
   data() {
      return {
         items: [],
         RegionList: [],
         DistrictList: [],
         PrtnContractTypeList: [],

         fields: [
            {
               key: 'order',
               label: '№',
               thClass: 'text-center',
               tdClass: 'text-center'
            },
            {
               key: 'region',
               label: this.$t('region'),
               thClass: 'text-center',
               tdClass: 'text-center'
            },
            {
               key: 'district',
               label: this.$t('district'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },

            {
               key: 'mfy',
               label: this.$t('mfy'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'organizationName',
               label: this.$t('organizationName'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'contractorInn',
               label: this.$t('contractorInn'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'sentForExamination',
               label: this.$t('sentForExamination'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'dateOfConclusionByJustice',
               label: this.$t('dateOfConclusionByJustice'),
               thClass: 'text-center',
               tdClass: 'text-center'
            },
            {
               key: 'daysLate',
               label: this.$t('daysLate'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'status',
               label: this.$t('status'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            }
         ],
         printLoding: false,
         filters: {
            prtnContractTypeId: null,
            regionId: null,
            districtId: null,
            contractorId: null,
            startDate: '',
            endDate: '',
            mfyId: null,
            byMfy: false,
            contractorInn: null,

            page: 1,
            pageSize: 20,
            perPageOptions: [10, 20, 50, 100],
            total: 0
         },
         isBusy: false
      };
   },
   computed: {
      firstNumber() {
         return (this.filters.page - 1) * this.filters.pageSize + 1;
      },
      lastNumber() {
         if (this.filters.total < this.filters.pageSize) {
            return this.filters.total;
         } else {
            if (this.filters.page * this.filters.pageSize > this.filters.total) {
               return this.filters.total;
            } else {
               return this.filters.page * this.filters.pageSize;
            }
         }
      }
   },
   created() {
      RegionService.GetAsSelectList(211)
         .then((res) => {
            this.RegionList = res.data;
         })
         .catch((error) => {
            this.showApiError(error);
         });
      PrtnContractTypeService.GetAsSelectList()
         .then((res) => {
            this.PrtnContractTypeList = res.data;
         })
         .catch((error) => {
            this.showApiError(error);
         });
      this.Refresh();
   },
   methods: {
      ChangeRegion(id) {
         if (id) {
            this.filters.regionId = id;
            this.Refresh();
            this.GetDistrict(id);
         } else {
            this.Refresh();
         }
      },
      GetDistrict(id) {
         DistrictService.GetAsSelectList(id)
            .then((res) => {
               this.DistrictList = res.data;
            })
            .catch((error) => {
               this.showApiError(error);
            });
      },
      Print() {
         this.printLoding = true;
         ReportService.PrintPrtnEmploymentGraphNewReport(this.filters).then((res) => {
            this.forceFileDownload(res, this.$t('GetSmsLogReport'));
            this.printLoding = false;
         });
      },
      SortChange(data) {
         this.filters.sortBy = data.sortBy;
         this.filters.orderType = data.sortDesc ? 'desc' : 'asc';
         this.Refresh();
      },
      Refresh() {
         this.isBusy = true;
         ReportService.PrtnEmploymentGraphNewReport(this.filters)
            .then((res) => {
               this.items = res.data.rows;
               this.filters.total = res.data.total;
               this.isBusy = false;
            })

            .catch((error) => {
               this.showApiError(error);
            });
      }
   }
};
</script>
