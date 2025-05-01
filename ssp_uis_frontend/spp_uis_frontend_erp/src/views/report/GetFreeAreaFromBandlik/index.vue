<template>
   <b-card no-body>
      <div class="m-2">
         <!-- <b-row>
            <b-col sm="12" md="2">
               <div>
                  <label for>{{ $t('Oblast') }}</label>
                  <v-select
                     :options="RegionList"
                     :reduce="(item) => item.value"
                     :placeholder="$t('ChooseBelow')"
                     label="text"
                     v-model="filter.regionId"
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
                     v-model="filter.districtId"
                     @input="ChangeDistrict"
                     class="w-100"
                  ></v-select>
               </div>
            </b-col>

            <b-col cols="12" md="4" class="mt-2">
               <b-input-group class="text-right">
                  <b-form-input v-model="filter.search" :placeholder="$t('search')" />
                  <b-input-group-append>
                     <b-button @click="Refresh" variant="primary">
                        <feather-icon icon="SearchIcon" />
                     </b-button>
                  </b-input-group-append>
               </b-input-group>
            </b-col>
            <b-col class="mt-2 text-right">
               <b-button @click="Print" :disabled="PrintLoading" variant="primary" class="ml-1">
                  <feather-icon icon="PrinterIcon"></feather-icon>
                  {{ $t('Print') }}
               </b-button>
            </b-col>
         </b-row>-->
         <!-- <b-row align-h="between">
            <b-col sm="12" md="8" class="mt-2">
               <b-button-group @click="Refresh" size="sm" class="mr-2">
                  <b-button
                     @click="filter.prtnContractTypeId = null"
                     :variant="null == filter.prtnContractTypeId ? 'primary' : 'outline-primary'"
                     >{{ $t('all') }}</b-button
                  >
                  <b-button
                     v-for="type in PrtnContractTypeList"
                     :key="type.value"
                     @click="filter.prtnContractTypeId = type.value"
                     :variant="type.value == filter.prtnContractTypeId ? 'primary' : 'outline-primary'"
                     >{{ type.text }}</b-button
                  >
               </b-button-group>

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
         </b-row>-->
      </div>
      <div class="m-2">
         <b-table
            ref="refInvoiceListTable"
            :items="items"
            responsive
            :fields="fields"
            primary-key="id"
            sticky-header="65vh"
            no-border-collapse
            no-footer-sorting
            :busy="isBusy"
            show-empty
            :empty-text="$t('NotFound')"
            class="position-relative"
            @sort-changed="SortChange"
         >
            <template #cell(order)="{ index }">
               <span>{{ index + 1 }}</span>
            </template>
            <!-- <template #cell(region)="{ item }">{{ item.region }}</template> -->

            <!-- <template #cell(contractor)="{ item }">
               <span style="color: blue; cursor: pointer" @click="goToBussnes(item.contractorInn)">
                  {{
                  item.contractorInn
                  }}
               </span>
               -
               {{ item.contractor }}
            </template>-->

            <template #cell(allArea)="{ item }">
               <span style="white-space: nowrap">{{ currency(item.allArea) }}</span>
            </template>
            <template #cell(freeArea)="{ item }">
               <span style="white-space: nowrap">{{ currency(item.freeArea) }}</span>
            </template>
            <!-- 
            <template #foot(order)>
               <span>{{ $t('Total') }}</span>
            </template>
            <template #foot(district)>
               <span></span>
            </template>
            <template #foot(region)>
               <span></span>
            </template>

            <template #foot(companyName)>
               <span></span>
            </template>
            <template #foot(companyTin)>
               <span></span>
            </template>
            <template #foot(companyAddress)>
               <span></span>
            </template>

            <template #foot(allArea)>
               <span class="text-end" style="white-space: nowrap;">{{ currency(totals.allArea) }}</span>
            </template>

            <template #foot(freeArea)>
               <span class="text-end" style="white-space: nowrap;">{{ currency(totals.freeArea) }}</span>
            </template>-->
         </b-table>
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
   BBreadcrumbItem
} from 'bootstrap-vue';
import ReportService from '@/services/report/report.service';
import PrtnContractTypeService from '@/services/info/prtncontracttype.service';

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
      BBreadcrumbItem
   },
   name: 'Index',
   directives: {
      'b-tooltip': VBTooltip,
      'b-modal': VBModal
   },
   data() {
      return {
         items: [],
         RegionList: [],
         DistrictList: [],
         PrtnContractTypeList: [],
         fields: [],
         filter: {
            contractTypeId: null,
            regionId: null,
            region: '',
            byRegion: true,
            districtId: null,
            district: '',
            byDistrict: false,
            contractorId: null,
            byContractor: false
         },
         isBusy: false,
         totals: {
            allArea: 0,
            freeArea: 0
         },
         PrintLoading: false,
         PrintForSumLoading: false
      };
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

            this.Refresh();
         })
         .catch((error) => {
            this.showApiError(error);
         });
      this.getFields();
   },
   methods: {
      getFields() {
         this.fields = [
            {
               key: 'order',
               label: this.$t('№'),
               thClass: 'text-center',
               tdClass: 'text-center'
               // sortable: true
            },
            {
               key: 'region',
               label: this.$t('Oblast'),
               sortable: true
               // stickyColumn: true
            },
            {
               key: 'district',
               label: this.$t('Region'),
               sortable: true
               // stickyColumn: true
            },
            {
               key: 'companyAddress',
               label: this.$t('companyAddress'),
               sortable: true
            },
            {
               key: 'companyTin',
               label: this.$t('companyTin'),
               sortable: true
            },
            {
               key: 'companyName',
               label: this.$t('companyName'),
               sortable: true
            },
            {
               key: 'allArea',
               label: this.$t('allArea'),
               thClass: 'text-right',
               tdClass: 'text-right',
               sortable: true
            },
            {
               key: 'freeArea',
               label: this.$t('freeArea'),
               thClass: 'text-right',
               tdClass: 'text-right',
               sortable: true
            }
         ];
      },
      goToBussnes(inn) {
         this.$router.push({ name: 'BusinessmanCard', query: { inn: inn } });
      },
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
            this.Refresh();
         }

         // this.GetDistrict(id);
      },
      BindValue(value) {
         this.filter.dateofbirth = value;
      },
      Print() {
         this.PrintLoading = true;
         ReportService.SaveAsExecel(this.filter)
            .then((res) => {
               this.forceFileDownload(res, this.$t('prtnapplicationandcontractinfo'));
            })
            .finally(() => {
               this.PrintLoading = false;
            });
      },
      PrintForSum() {
         this.PrintForSumLoading = true;
         ReportService.SaveAsExecelForSum(this.filter)
            .then((res) => {
               this.forceFileDownload(res, this.$t('prtnapplicationandcontractinfo'));
            })
            .finally(() => {
               this.PrintForSumLoading = false;
            });
      },
      changeTotal() {
         this.filter.haslocation = null;
         this.filter.isParij = null;
         this.Refresh();
      },
      Refresh() {
         this.getFields();
         // if (!this.filter.prtnContractTypeId) {
         //   this.makeToast(
         //     `${this.$t("prtnContractType")} ${this.$t("NotSelect")}`,
         //     "danger"
         //   );
         //   return false;
         // }
         this.isBusy = true;
         ReportService.GetFreeAreaFromBandlik(this.filter)
            .then((res) => {
               this.items = res.data;

               this.totals = {
                  freeArea: 0,
                  allArea: 0
               };
               res.data.forEach((item) => {
                  this.totals.freeArea += item.freeArea;
                  this.totals.allArea += item.allArea;
               });

               this.isBusy = false;
            })
            .catch((error) => {
               this.isBusy = false;
               this.makeToast(error.response.data.errors, 'danger');
            });
      }
   }
};
</script>

<style lang="scss" scoped>
@import '../styles.scss';
</style>
