<template>
   <b-card no-body>
      <div class="m-2">
         <b-row>
            <b-col sm="12" md="3">
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
            <b-col sm="12" md="3">
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
            <b-col class="text-right">
               <b-button variant="primary" @click="downloadExcel" :disabled="PrintLoading">
                  <b-icon-printer /> {{ $t('Print') }}
               </b-button>
            </b-col>
         </b-row>
         <b-row align-h="between">
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
         </b-row>
      </div>
      <div class="m-2">
         <b-table
            ref="refInvoiceListTable"
            :items="items"
            responsive
            :fields="fields"
            primary-key="id"
            sticky-header="65vh"
            no-footer-sorting
            :busy="isBusy"
            show-empty
            bordered
            :empty-text="$t('NotFound')"
            class="position-relative report-table"
            foot-clone
            @sort-changed="SortChange"
         >
            <template #cell(order)="{ index }">
               <span>{{ index + 1 }}</span>
            </template>
            <template #cell(region)="{ item }">
               <span style="color: blue; cursor: pointer" @click="SortRegion(item)">{{ item.region }}</span>
            </template>

            <template #cell(contractor)="{ item }">
               <span style="color: blue; cursor: pointer" @click="goToBussnes(item.contractorInn)">{{
                  item.contractorInn
               }}</span>
               -
               {{ item.contractor }}
            </template>

            <template #cell(district)="{ item }">
               <span style="color: blue; cursor: pointer" @click="SortDistrict(item)">{{ item.district }}</span>
            </template>

            <template #cell(expiredApplicationCount)="{ item }">
               {{ currency(item.expiredApplicationCount) }}
            </template>

            <template #cell(expiredCertificateCount)="{ item }">
               {{ currency(item.expiredCertificateCount) }}
            </template>

            <!-- footer -->

            <template #foot(expiredApplicationCount)>
               <span>{{ currency(totals.expiredApplicationCount) }}</span>
            </template>

            <template #foot(expiredContractCount)>
               <span>{{ currency(totals.expiredContractCount) }}</span>
            </template>

            <template #foot(expiredCertificateCount)>
               <span>
                  {{ currency(totals.expiredCertificateCount) }}
               </span>
            </template>

            <template #foot(order)>
               <span>{{ $t('Total') }}</span>
            </template>
            <template #foot(district)>
               <span></span>
            </template>
            <template #foot(region)>
               <span></span>
            </template>
            <template #foot(contractor)>
               <span></span>
            </template>
            <template #foot(contractType)>
               <span></span>
            </template>
            <template #foot(contractorInn)>
               <span></span>
            </template>
            <template #foot(contractorPhoneNumber)>
               <span></span>
            </template>
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
   BBreadcrumbItem,
   BIconPrinter
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
      BBreadcrumbItem,
      BIconPrinter
   },
   directives: {
      'b-tooltip': VBTooltip,
      'b-modal': VBModal
   },
   data() {
      return {
         PrintLoading: false,
         items: [],
         RegionList: [],
         DistrictList: [],
         PrtnContractTypeList: [],
         fields: [],
         filter: {
            prtnContractTypeId: null,
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
            expiredApplicationCount: 0,
            expiredContractCount: 0,
            expiredCertificateCount: 0
         }
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
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: this.filter.byRegion ? 'region' : '',
               label: this.$t('Oblast'),
               sortable: true,
               stickyColumn: true
            },
            {
               key: this.filter.byDistrict ? 'district' : '',
               label: this.$t('Region'),
               sortable: true,
               stickyColumn: true
            },
            {
               key: this.filter.byContractor ? 'contractor' : '',
               label: this.$t('contractorT'),
               sortable: true,
               stickyColumn: true,
               thStyle: {
                  minWidth: '350px'
               }
            },
            {
               key: this.filter.byContractor ? 'contractorPhoneNumber' : '',
               label: this.$t('phoneNumber'),
               sortable: true
            },
            {
               key: this.filter.byContractor ? 'contractType' : '',
               label: this.$t('prtnContractType'),
               sortable: true
            },
            {
               key: 'expiredApplicationCount',
               label: this.$t('expiredApplicationCount'),
               thClass: 'text-right',
               tdClass: 'text-right text-nowrap',
               sortable: true
            },

            {
               key: 'expiredContractCount',
               label: this.$t('expiredContractCount'),
               thClass: 'text-right',
               tdClass: 'text-right text-nowrap',
               sortable: true
            },
            {
               key: 'expiredCertificateCount',
               label: this.$t('expiredCertificateCount'),
               thClass: 'text-right',
               tdClass: 'text-right text-nowrap',
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
            this.filter.byRegion = true;
            this.filter.byContractor = true;
            this.filter.district = this.filter.districtId
               ? this.DistrictList.filter((item) => item.value === this.filter.districtId)[0].text
               : '';
            this.Refresh();
         } else {
            this.filter.byDistrict = true;
            this.filter.byRegion = true;
            this.filter.byContractor = true;
            this.filter.district = '';
            this.Refresh();
         }
      },
      downloadExcel() {
         this.PrintLoading = true;

         ReportService.SaveAsExecelForPrtnCreditDemand(this.filter)
            .then((res) => {
               this.makeToast(this.$t('SuccessMessage'), 'success');
               this.forceFileDownload(res, this.$t('GetPrtnCreditDemandInfo'));
            })
            .catch((error) => {
               this.showApiError(error);
            })
            .finally(() => {
               this.PrintLoading = false;
            });
      },
      Refresh() {
         this.getFields();
         this.isBusy = true;
         ReportService.GetPrntExpiredDocuments(this.filter)
            .then((res) => {
               this.items = res.data;

               this.totals = {
                  expiredApplicationCount: 0,
                  expiredContractCount: 0,
                  expiredCertificateCount: 0
               };

               res.data.forEach((item) => {
                  this.totals.expiredApplicationCount += item.expiredApplicationCount;
                  this.totals.expiredContractCount += item.expiredContractCount;
                  this.totals.expiredCertificateCount += item.expiredCertificateCount;
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
