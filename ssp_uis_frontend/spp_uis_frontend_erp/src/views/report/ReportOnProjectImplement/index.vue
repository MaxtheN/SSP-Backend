<template>
   <b-card no-body>
      <div class="mr-2 ml-2 mt-2">
         <b-tabs pills>
            <!-- <b-tab @click="Refresh()" :title="$t('variant-1')"> -->
            <b-row>
               <b-col sm="12" md="2">
                  <div>
                     <label for>{{ $t('Oblast') }}</label>
                     <v-select
                        :options="RegionList"
                        :reduce="(item) => item.value"
                        :placeholder="$t('ChooseBelow')"
                        label="text"
                        v-model="filter.regionId"
                        class="w-100"
                        @input="ChangeRegion"
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
                        v-model="filter.districtId"
                        @input="ChangeDistrict"
                        class="w-100"
                     ></v-select>
                  </div>
               </b-col>
               <b-col sm="12" md="2">
                  <form-picker
                     :label="$t('startDate')"
                     v-model="filter.startDate"
                     :placeholder="$t('startDate')"
                  ></form-picker>
                  <!-- @update:modelValue="Refresh" -->
               </b-col>
               <b-col sm="12" md="2">
                  <form-picker
                     :label="$t('endDate')"
                     v-model="filter.endDate"
                     :placeholder="$t('endDate')"
                  ></form-picker>
                  <!-- @update:modelValue="Refresh" -->
               </b-col>
               <b-col sm="12" md="2" class="ml-auto text-right mt-2">
                  <b-button variant="primary">
                     <feather-icon icon="PrinterIcon"></feather-icon>
                     {{ $t('Print') }}
                  </b-button>
               </b-col>
            </b-row>

            <b-col sm="12" md="8" class="mt-2">
               <b-breadcrumb class="mt-2">
                  <b-breadcrumb-item
                     :active="filter.byRegion"
                     @click="
                        () => {
                           filter.byDistrict = false;
                           filter.byRegion = true;
                           filter.byContractor = false;
                           filter.region = '';
                           filter.regionId = null;
                           filter.district = '';
                           filter.districtId = null;
                           Refresh();
                        }
                     "
                  >
                     <b>{{ $t('uzb') }}</b>
                  </b-breadcrumb-item>
                  <b-breadcrumb-item
                     v-show="filter.region"
                     :active="filter.byDistrict"
                     @click="
                        () => {
                           filter.byDistrict = true;
                           filter.byRegion = false;
                           filter.byContractor = false;
                           filter.district = '';
                           filter.districtId = null;
                           Refresh();
                        }
                     "
                  >
                     <b>{{ filter.region }}</b>
                  </b-breadcrumb-item>
                  <b-breadcrumb-item v-show="filter.district" :active="filter.byContractor">
                     <b>{{ filter.district }}</b>
                  </b-breadcrumb-item>
               </b-breadcrumb>
            </b-col>

            <div class="mt-2 mb-2 report-table">
               <b-overlay :show="isBusy">
                  <b-table-simple responsive>
                     <b-thead>
                        <b-tr>
                           <b-th
                              class="table-b-table-default"
                              :class="{ 'b-table-sticky-column': !isMobileDevice() }"
                              rowspan="4"
                           >
                              №
                           </b-th>
                           <b-th
                              class="table-b-table-default"
                              :class="{ 'b-table-sticky-column': !isMobileDevice() }"
                              rowspan="4"
                              ><span v-show="filter.byRegion" style="font-weight: 900; font-size: 14px; color: black">{{
                                 $t('region')
                              }}</span>
                              <span
                                 v-show="filter.byDistrict"
                                 style="font-weight: 900; font-size: 14px; color: black"
                                 >{{ $t('Region') }}</span
                              >
                              <div
                                 v-show="filter.byContractor"
                                 style="font-weight: 900; font-size: 14px; color: black; width: 150px"
                              >
                                 {{ $t('contractorT') }}
                              </div></b-th
                           >
                           <b-th colspan="2" rowspan="2"> {{ $t('givenCertificate') }} </b-th>
                           <b-th colspan="2" rowspan="2">
                              {{ $t("Ishga tushgan loyihalar(Reja bo'yicha ish otni yaratish muddati kelganlar)") }}
                           </b-th>
                           <b-th colspan="3" rowspan="2">
                              {{ $t("Soliqdan olingan ma'lumotga ko'ra yangi yaratilgan ish o'rinlari soni") }}
                           </b-th>
                           <b-th colspan="3" rowspan="2">
                              {{ $t("Hokimiyatlar taqdim etgan ma'lumotga ko'rayangi yaratilgan ish o'rinlari soni") }}
                           </b-th>
                           <b-th colspan="19"> {{ $t('Imtiyozlar') }} </b-th>
                        </b-tr>
                        <b-tr>
                           <b-th colspan="2"> {{ $t('Ajratilgan imtiyozli kreditlat') }} </b-th>
                           <b-th colspan="2"> {{ $t('Berilgan kafillik') }} </b-th>
                           <b-th colspan="10"> {{ $t('Soliqdan') }} </b-th>
                           <b-th colspan="3"> {{ $t('Bojxonadan') }} </b-th>
                           <b-th colspan="2"> {{ $t('Amaliy monomarkaz') }} </b-th>
                        </b-tr>
                        <b-tr>
                           <b-th rowspan="2"> {{ $t('totalDocCount') }} </b-th>
                           <b-th rowspan="2"> {{ $t("Reja bo'yicha ish o'rinlari soni") }} </b-th>
                           <b-th rowspan="2"> {{ $t('totalDocCount') }} </b-th>
                           <b-th rowspan="2"> {{ $t("Reja bo'yicha ish o'rinlari soni") }} </b-th>
                           <b-th rowspan="2"> {{ $t('totalDocCount') }} </b-th>
                           <b-th rowspan="2"> {{ $t("Yangi yaratilgan ish  o'rinlari soni") }} </b-th>
                           <b-th rowspan="2"> {{ $t("O'rtacha ish haqi") }} </b-th>
                           <b-th rowspan="2"> {{ $t('totalDocCount') }} </b-th>
                           <b-th rowspan="2"> {{ $t("Yangi yaratilgan ish  o'rinlari soni") }} </b-th>
                           <b-th rowspan="2"> {{ $t("O'rtacha ish haqi") }} </b-th>
                           <b-th rowspan="2"> {{ $t('count') }} </b-th>
                           <b-th rowspan="2"> {{ $t('amount1') }} </b-th>
                           <b-th rowspan="2"> {{ $t('count') }} </b-th>
                           <b-th rowspan="2"> {{ $t('amount1') }} </b-th>
                           <b-th colspan="2"> {{ $t("Mol mulk va yer solig'i") }} </b-th>
                           <b-th colspan="2"> {{ $t('IncomeTax') }} </b-th>
                           <b-th colspan="2"> {{ $t('Soliq stavkasining 50 foiz miqdori ') }}</b-th>
                           <b-th colspan="2"> {{ $t("QQS ni o'zaro hisobga olishount") }} </b-th>
                           <b-th colspan="2">
                              {{ $t('Soliqlarni taminotsiz va foizsiz bolib tolash huquqini olganlar soni') }}
                           </b-th>
                           <b-th rowspan="2">
                              {{
                                 $t(
                                    'Bojxona tartib-tamoillariga rioya qilgan xolda "yashil yo\'lak" tartibi qollaganlar'
                                 )
                              }}
                           </b-th>
                           <b-th colspan="2">
                              {{ $t("Bojxona to'lovlarini taminotsiz va foizsiz bolib tolash huquqini olganlar soni") }}
                           </b-th>
                           <b-th rowspan="2"> {{ $t('Auksionda sotilmasdan turgan bino va inshoatlarda') }} </b-th>
                           <b-th rowspan="2"> {{ $t('KXK larning bosh turgan binolari') }} </b-th>
                        </b-tr>
                        <b-tr height="23">
                           <b-th>{{ $t('count') }}</b-th>
                           <b-th>{{ $t('amount1') }}</b-th>
                           <b-th>{{ $t('count') }}</b-th>
                           <b-th>{{ $t('amount1') }}</b-th>
                           <b-th>{{ $t('count') }}</b-th>
                           <b-th>{{ $t('amount1') }}</b-th>
                           <b-th>{{ $t('count') }}</b-th>
                           <b-th>{{ $t('amount1') }}</b-th>
                           <b-th>{{ $t('count') }}</b-th>
                           <b-th>{{ $t('amount1') }}</b-th>
                           <b-th>{{ $t('count') }}</b-th>
                           <b-th>{{ $t('amount1') }}</b-th>
                        </b-tr>
                     </b-thead>
                     <b-tbody>
                        <b-tr v-for="(item, inx) in items" :key="inx">
                           <b-td
                              class="table-b-table-default"
                              :class="{ 'b-table-sticky-column': !isMobileDevice() }"
                              >{{ inx + 1 }}</b-td
                           >
                           <b-td class="table-b-table-default" :class="{ 'b-table-sticky-column': !isMobileDevice() }">
                              <span
                                 v-show="filter.byRegion"
                                 style="color: blue; cursor: pointer"
                                 @click="SortRegion(item)"
                                 >{{ item.region }}</span
                              >

                              <span
                                 v-show="filter.byDistrict"
                                 style="color: blue; cursor: pointer"
                                 @click="SortDistrict(item)"
                                 >{{ item.district }}</span
                              >

                              <div style="width: 400px; text-wrap: wrap" v-show="filter.byContractor">
                                 {{ item.contractorInn }} -
                                 {{ item.contractor }}
                              </div>

                              <!-- {{ item.region }} -->
                           </b-td>
                           <b-td class="text-right">{{ currency(item?.certificateHolders?.item1) }}</b-td>
                           <b-td class="text-right">{{ currency(item?.certificateHolders?.item2) }}</b-td>
                           <b-td class="text-right">{{ currency(item?.relationToLaunchedProjects?.item1) }}</b-td>
                           <b-td class="text-right">{{ currency(item?.relationToLaunchedProjects?.item2) }}</b-td>
                           <b-td class="text-right">{{ currency(item?.dataFromTaxContrator?.item1) }}</b-td>
                           <b-td class="text-right">{{ currency(item?.dataFromTaxContrator?.item2) }}</b-td>
                           <b-td class="text-right">{{ currency(item?.dataFromTaxContrator?.item3) }}</b-td>
                           <b-td class="text-right">{{ currency(item?.dataFromGovernmentContratorCount?.item1) }}</b-td>
                           <b-td class="text-right">{{ currency(item?.dataFromGovernmentContratorCount?.item2) }}</b-td>
                           <b-td class="text-right">{{ currency(item?.dataFromGovernmentContratorCount?.item3) }}</b-td>
                           <b-td class="text-right">{{ currency(item?.aprovedCredit?.item1) }}</b-td>
                           <b-td class="text-right">{{ currency(item?.aprovedCredit?.item2) }}</b-td>
                           <b-td class="text-right">{{ currency(item?.separatePreferentialCredit?.item1) }}</b-td>
                           <b-td class="text-right">{{ currency(item?.separatePreferentialCredit?.item2) }}</b-td>
                           <b-td class="text-right">{{ currency(item?.incomeTax?.item1) }}</b-td>
                           <b-td class="text-right">{{ currency(item?.incomeTax?.item2) }}</b-td>
                           <b-td class="text-right">{{ currency(item?.incomeTax?.item1) }}</b-td>
                           <b-td class="text-right">{{ currency(item?.incomeTax?.item2) }}</b-td>
                           <b-td class="text-right">{{ currency(item?.fiftyPercentOfTheTaxRate?.item1) }}</b-td>
                           <b-td class="text-right">{{ currency(item?.fiftyPercentOfTheTaxRate?.item2) }}</b-td>
                           <b-td class="text-right">{{ currency(item?.crossAccountingOfVAT?.item1) }}</b-td>
                           <b-td class="text-right">{{ currency(item?.crossAccountingOfVAT?.item2) }}</b-td>
                           <b-td class="text-right">{{
                              currency(item?.peopleTaxesWithoutInsuranceAndWithoutInterest?.item1)
                           }}</b-td>
                           <b-td class="text-right">{{
                              currency(item?.peopleTaxesWithoutInsuranceAndWithoutInterest?.item2)
                           }}</b-td>
                           <b-td class="text-right">{{ currency(item?.fromCustoms?.item1) }}</b-td>
                           <b-td class="text-right">{{ currency(item?.fromCustoms?.item2) }}</b-td>
                           <b-td class="text-right">{{ currency(item?.fromCustoms?.item3) }}</b-td>
                           <b-td class="text-right">{{ currency(item?.practicalMonocenter?.item1) }}</b-td>
                           <b-td class="text-right">{{ currency(item?.practicalMonocenter?.item2) }}</b-td>
                        </b-tr>
                     </b-tbody>
                     <b-tfoot v-if="items.length > 0">
                        <b-tr variant="secondary">
                           <b-td class="text-right" :class="{ 'b-table-sticky-column': !isMobileDevice() }"></b-td>
                           <b-td class="text-right" :class="{ 'b-table-sticky-column': !isMobileDevice() }">{{
                              $t('Total')
                           }}</b-td>

                           <b-td class="text-right">{{ currency(totals?.certificateHolders?.item1) }}</b-td>
                           <b-td class="text-right">{{ currency(totals?.certificateHolders?.item2) }}</b-td>
                           <b-td class="text-right">{{ currency(totals?.relationToLaunchedProjects?.item1) }}</b-td>
                           <b-td class="text-right">{{ currency(totals?.relationToLaunchedProjects?.item2) }}</b-td>
                           <b-td class="text-right">{{ currency(totals?.dataFromTaxContrator?.item1) }}</b-td>
                           <b-td class="text-right">{{ currency(totals?.dataFromTaxContrator?.item2) }}</b-td>
                           <b-td class="text-right">{{ currency(totals?.dataFromTaxContrator?.item3) }}</b-td>
                           <b-td class="text-right">{{
                              currency(totals?.dataFromGovernmentContratorCount?.item1)
                           }}</b-td>
                           <b-td class="text-right">{{
                              currency(totals?.dataFromGovernmentContratorCount?.item2)
                           }}</b-td>
                           <b-td class="text-right">{{
                              currency(totals?.dataFromGovernmentContratorCount?.item3)
                           }}</b-td>
                           <b-td class="text-right">{{ currency(totals?.aprovedCredit?.item1) }}</b-td>
                           <b-td class="text-right">{{ currency(totals?.aprovedCredit?.item2) }}</b-td>
                           <b-td class="text-right">{{ currency(totals?.separatePreferentialCredit?.item1) }}</b-td>
                           <b-td class="text-right">{{ currency(totals?.separatePreferentialCredit?.item2) }}</b-td>
                           <b-td class="text-right">{{ currency(totals?.incomeTax?.item1) }}</b-td>
                           <b-td class="text-right">{{ currency(totals?.incomeTax?.item2) }}</b-td>
                           <b-td class="text-right">{{ currency(totals?.incomeTax?.item1) }}</b-td>
                           <b-td class="text-right">{{ currency(totals?.incomeTax?.item2) }}</b-td>
                           <b-td class="text-right">{{ currency(totals?.fiftyPercentOfTheTaxRate?.item1) }}</b-td>
                           <b-td class="text-right">{{ currency(totals?.fiftyPercentOfTheTaxRate?.item2) }}</b-td>
                           <b-td class="text-right">{{ currency(totals?.crossAccountingOfVAT?.item1) }}</b-td>
                           <b-td class="text-right">{{ currency(totals?.crossAccountingOfVAT?.item2) }}</b-td>
                           <b-td class="text-right">{{
                              currency(totals?.peopleTaxesWithoutInsuranceAndWithoutInterest?.item1)
                           }}</b-td>
                           <b-td class="text-right">{{
                              currency(totals?.peopleTaxesWithoutInsuranceAndWithoutInterest?.item2)
                           }}</b-td>
                           <b-td class="text-right">{{ currency(totals?.fromCustoms?.item1) }}</b-td>
                           <b-td class="text-right">{{ currency(totals?.fromCustoms?.item2) }}</b-td>
                           <b-td class="text-right">{{ currency(totals?.fromCustoms?.item3) }}</b-td>
                           <b-td class="text-right">{{ currency(totals?.practicalMonocenter?.item1) }}</b-td>
                           <b-td class="text-right">{{ currency(totals?.practicalMonocenter?.item2) }}</b-td>
                        </b-tr>
                     </b-tfoot>
                     <template #overlay>
                        <div class="text-center text-primary my-2">
                           <b-spinner scale="2" class="align-middle mr-2"></b-spinner>
                           <strong>{{ $t('Loading') }}...</strong>
                        </div>
                     </template>
                  </b-table-simple>
               </b-overlay>
            </div>
         </b-tabs>
      </div>
   </b-card>
</template>

<script>
import {
   BButton,
   BPagination,
   BTable,
   BCol,
   VBTooltip,
   VBModal,
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
   BTableSimple,
   BThead,
   BTbody,
   BTr,
   BTd,
   BTh,
   BButtonGroup,
   BFormCheckbox,
   BBreadcrumb,
   BBreadcrumbItem,
   BTfoot,
   BOverlay,
   BTabs,
   BTab
} from 'bootstrap-vue';
import ReportService from '@/services/report/report.service';

import RegionService from '@/services/info/region.service';
import DistrictService from '@/services/info/district.service';

export default {
   components: {
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
      BTableSimple,
      BThead,
      BTbody,
      BTr,
      BTd,
      BTh,
      BButtonGroup,
      BFormCheckbox,
      BBreadcrumb,
      BBreadcrumbItem,
      BTfoot,
      BOverlay,
      BTabs,
      BTab
   },
   name: 'Index',
   directives: {
      'b-tooltip': VBTooltip,
      'b-modal': VBModal
   },
   data() {
      return {
         tabData: 1,
         items: [],
         RegionList: [],
         DistrictList: [],
         PrtnContractTypeList: [],
         filter: {
            regionId: null,
            byRegion: true,
            startDate: '01.01.2023',
            endDate: '17.07.2024',
            districtId: null,
            byDistrict: false,
            hasDistrict: false,
            hasRegion: false,
            byContractor: false
         },
         totals: {
            certificateHolders: {
               item1: 0,
               item2: 0
            },
            relationToLaunchedProjects: {
               item1: 0,
               item2: 0
            },
            dataFromTaxContrator: {
               item1: 0.0,
               item2: 0,
               item3: 0
            },
            dataFromGovernmentContratorCount: {
               item1: 0.0,
               item2: 0,
               item3: 0
            },
            aprovedCredit: {
               item1: 0,
               item2: 0
            },
            separatePreferentialCredit: {
               item1: 0.0,
               item2: 0
            },
            propertyAndLandTax: {
               item1: 0,
               item2: 0
            },
            incomeTax: {
               item1: 0,
               item2: 0
            },
            fiftyPercentOfTheTaxRate: {
               item1: 0,
               item2: 0
            },
            crossAccountingOfVAT: {
               item1: 0,
               item2: 0
            },
            peopleTaxesWithoutInsuranceAndWithoutInterest: {
               item1: 0,
               item2: 0
            },
            fromCustoms: {
               item1: 0,
               item2: 0,
               item3: 0
            },
            practicalMonocenter: {
               item1: 0,
               item2: 0
            }
         },

         isBusy: false
      };
   },
   created() {
      this.Refresh();
      RegionService.GetAsSelectList(211)
         .then((res) => {
            this.RegionList = res.data;
         })
         .catch((error) => {
            this.showApiError(error);
         });
   },
   computed: {
      firstNumber() {
         return (this.filter2.page - 1) * this.filter2.pageSize + 1;
      },
      lastNumber() {
         if (this.filter2.total < this.filter2.pageSize) {
            return this.filter2.total;
         } else {
            if (this.filter2.page * this.filter2.pageSize > this.filter2.total) {
               return this.filter2.total;
            } else {
               return this.filter2.page * this.filter2.pageSize;
            }
         }
      }
   },
   methods: {
      SortRegion(item) {
         this.filter.byDistrict = true;
         this.filter.byRegion = false;
         this.filter.byContractor = false;
         this.filter.regionId = item.regionId;
         this.filter.region = item.region;
         this.GetDistrict(item.regionId);
         this.Refresh();
      },
      SortDistrict(item) {
         this.filter.byDistrict = false;
         this.filter.byRegion = false;
         this.filter.byContractor = true;
         this.filter.districtId = item.districtId;
         this.filter.district = item.district;
         this.Refresh();
      },
      SortChange(data) {
         this.filter.Sort = data.sortBy;
         this.filter.Order = data.sortDesc ? 'desc' : 'asc';
         this.Refresh();
      },
      ChangeRegion(id) {
         if (id) {
            this.filter.districtId = null;
            this.filter.byDistrict = true;
            this.filter.byRegion = false;
            this.filter.byContractor = false;

            this.filter.region = this.filter.regionId
               ? this.RegionList.filter((item) => item.value === this.filter.regionId)[0].text
               : '';

            this.Refresh();
            this.GetDistrict(id);
         } else {
            this.filter.districtId = null;
            this.filter.byDistrict = false;
            this.filter.byRegion = true;
            this.filter.region = '';
            this.filter.hasRegion = false;
            this.filter.byContractor = false;
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
      ChangeDistrict(id) {
         if (id) {
            this.filter.byDistrict = false;
            this.filter.byRegion = false;
            this.filter.byContractor = true;
            this.filter.district = this.filter.districtId
               ? this.DistrictList.filter((item) => item.value === this.filter.districtId)[0].text
               : '';

            this.Refresh();
         } else {
            this.filter.byDistrict = true;
            this.filter.byRegion = false;
            this.filter.byContractor = false;
            this.filter.district = '';
            this.filter.hasDistrict = false;
            this.Refresh();
         }
      },
      BindValue(value) {
         this.filter.dateofbirth = value;
      },
      // Print() {
      //    ReportService.SaveasExcelSoliqImtiyoz(this.filter).then((res) => {
      //       this.forceFileDownload(res, this.$t('soliq'));
      //    });
      // },
      Refresh() {
         this.isBusy = true;

         if (this.filter.regionId) {
            this.filter.byRegion = false;
            this.filter.hasRegion = true;
         }

         if (this.filter.districtId) {
            this.filter.byDistrict = false;
            this.filter.hasDistrict = true;
         }

         if (this.filter.districtId) {
            this.byRegion = false;
            this.byDistrict = false;
            this.hasDistrict = true;
            this.hasRegion = true;
            this.byContractor = true;
         }
         ReportService.ReportOnProjectImplement(this.filter)
            .then((res) => {
               this.items = res.data.result;

               this.totals = {
                  certificateHolders: {
                     item1: 0,
                     item2: 0
                  },
                  relationToLaunchedProjects: {
                     item1: 0,
                     item2: 0
                  },
                  dataFromTaxContrator: {
                     item1: 0.0,
                     item2: 0,
                     item3: 0
                  },
                  dataFromGovernmentContratorCount: {
                     item1: 0.0,
                     item2: 0,
                     item3: 0
                  },
                  aprovedCredit: {
                     item1: 0,
                     item2: 0
                  },
                  separatePreferentialCredit: {
                     item1: 0.0,
                     item2: 0
                  },
                  propertyAndLandTax: {
                     item1: 0,
                     item2: 0
                  },
                  incomeTax: {
                     item1: 0,
                     item2: 0
                  },
                  fiftyPercentOfTheTaxRate: {
                     item1: 0,
                     item2: 0
                  },
                  crossAccountingOfVAT: {
                     item1: 0,
                     item2: 0
                  },
                  peopleTaxesWithoutInsuranceAndWithoutInterest: {
                     item1: 0,
                     item2: 0
                  },
                  fromCustoms: {
                     item1: 0,
                     item2: 0,
                     item3: 0
                  },
                  practicalMonocenter: {
                     item1: 0,
                     item2: 0
                  }
               };

               res.data.result.forEach((item) => {
                  this.totals.certificateHolders.item1 += item.certificateHolders?.item1;
                  this.totals.certificateHolders.item2 += item?.certificateHolders?.item2;
                  this.totals.relationToLaunchedProjects.item1 += item?.relationToLaunchedProjects?.item1;
                  this.totals.relationToLaunchedProjects.item2 += item?.relationToLaunchedProjects?.item2;
                  this.totals.dataFromTaxContrator.item1 += item?.dataFromTaxContrator?.item1;
                  this.totals.dataFromTaxContrator.item2 += item?.dataFromTaxContrator?.item2;
                  this.totals.dataFromTaxContrator.item3 += item?.dataFromTaxContrator?.item3;
                  this.totals.dataFromGovernmentContratorCount.item1 += item?.dataFromGovernmentContratorCount?.item1;
                  this.totals.dataFromGovernmentContratorCount.item2 += item?.dataFromGovernmentContratorCount?.item2;
                  this.totals.dataFromGovernmentContratorCount.item3 += item?.dataFromGovernmentContratorCount?.item3;
                  this.totals.aprovedCredit.item1 += item?.aprovedCredit?.item1;
                  this.totals.aprovedCredit.item2 += item?.aprovedCredit?.item2;
                  this.totals.separatePreferentialCredit.item1 += item?.separatePreferentialCredit?.item1;
                  this.totals.separatePreferentialCredit.item2 += item?.separatePreferentialCredit?.item2;
                  this.totals.incomeTax.item1 += item?.incomeTax?.item1;
                  this.totals.incomeTax.item2 += item?.incomeTax?.item2;
                  this.totals.incomeTax.item1 += item?.incomeTax?.item1;
                  this.totals.incomeTax.item2 += item?.incomeTax?.item2;
                  this.totals.fiftyPercentOfTheTaxRate.item1 += item?.fiftyPercentOfTheTaxRate?.item1;
                  this.totals.fiftyPercentOfTheTaxRate.item2 += item?.fiftyPercentOfTheTaxRate?.item2;
                  this.totals.crossAccountingOfVAT.item1 += item?.crossAccountingOfVAT?.item1;
                  this.totals.crossAccountingOfVAT.item2 += item?.crossAccountingOfVAT?.item2;
                  this.totals.fromCustoms.item1 += item?.fromCustoms?.item1;
                  this.totals.fromCustoms.item2 += item?.fromCustoms?.item2;
                  this.totals.fromCustoms.item3 += item?.fromCustoms?.item3;
                  this.totals.practicalMonocenter.item1 += item?.practicalMonocenter?.item1;
                  this.totals.practicalMonocenter.item2 += item?.practicalMonocenter?.item2;

                  this.totals.peopleTaxesWithoutInsuranceAndWithoutInterest.item1 +=
                     item?.peopleTaxesWithoutInsuranceAndWithoutInterest?.item1;
                  this.totals.peopleTaxesWithoutInsuranceAndWithoutInterest.item2 +=
                     item?.peopleTaxesWithoutInsuranceAndWithoutInterest?.item2;
               });
            })
            .catch((error) => {
               this.showApiError(error);
            })
            .finally(() => {
               this.isBusy = false;
            });
      }
   }
};
</script>

<style lang="scss" scoped>
@import '../styles.scss';
</style>
