<template>
   <b-card no-body>
      <div class="mx-2">
         <b-row align-h="between " class="mt-2">
            <b-col sm="12" md="8" class="overflow-auto">
               <b-button-group @click="Refresh" size="sm" class="mr-2">
                  <b-button
                     @click="filter.contractTypeId = null"
                     :variant="null == filter.contractTypeId ? 'primary' : 'outline-primary'"
                     >{{ $t('all') }}</b-button
                  >
                  <b-button
                     v-for="type in PrtnContractTypeList"
                     :key="type.value"
                     @click="filter.contractTypeId = type.value"
                     :variant="type.value == filter.contractTypeId ? 'primary' : 'outline-primary'"
                     >{{ type.text }}</b-button
                  >
               </b-button-group>

               <b-breadcrumb class="mt-2">
                  <b-breadcrumb-item
                     :active="filter.byMainBank"
                     @click="
                        () => {
                           filter.byMainBank = true;
                           filter.byContractor = false;
                           filter.bank = '';
                           filter.mainBankId = null;
                           filter.bankId = null;
                           filter.byBank = false;
                           Refresh();
                        }
                     "
                  >
                     <b>{{ $t('Bank') }}</b>
                  </b-breadcrumb-item>
                  <b-breadcrumb-item
                     v-show="filter.mainBankId"
                     :active="filter.byBank"
                     @click="
                        () => {
                           filter.byBank = true;
                           filter.byContractor = false;
                           filter.bankId = null;
                           Refresh();
                        }
                     "
                  >
                     <b>{{ filter.mainBank }}</b>
                  </b-breadcrumb-item>
                  <b-breadcrumb-item
                     style="color: black"
                     v-show="filter.bankId"
                     @click="
                        () => {
                           filter.byBank = false;
                           filter.byContractor = true;
                           Refresh();
                        }
                     "
                  >
                     <b>{{ filter.bank }}</b>
                  </b-breadcrumb-item>
               </b-breadcrumb>
            </b-col>
            <b-col class="text-right">
               <b-button variant="primary" @click="downloadExcel" :disabled="PrintLoading">
                  <b-icon-printer /> {{ $t('Print') }}
               </b-button>
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
            <template #cell(mainBank)="{ item }">
               <span style="color: blue; cursor: pointer" @click="SortMianBank(item)">{{ item.mainBank }}</span>
            </template>
            <template #cell(bank)="{ item }">
               <span style="color: blue; cursor: pointer" @click="SortBank(item)">{{ item.bank }}</span>
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

            <template #cell(contractorCount)="{ item }">
               {{ currency(item.contractorCount) }}
            </template>
            <template #cell(foreignInvestment)="{ item }">
               {{ currency(item.foreignInvestment) }}
            </template>
            <template #cell(newVacanciesCount)="{ item }">
               {{ currency(item.newVacanciesCount) }}
            </template>
            <template #cell(ownInvestment)="{ item }">
               {{ currency(item.ownInvestment) }}
            </template>
            <template #cell(privilegeBankCredit)="{ item }">
               {{ currency(item.privilegeBankCredit) }}
            </template>
            <template #cell(projectCost)="{ item }">
               {{ currency(item.projectCost) }}
            </template>

            <!-- footer -->

            <template #foot(contractorCount)>
               {{ currency(totals.contractorCount) }}
            </template>
            <template #foot(foreignInvestment)>
               {{ currency(totals.foreignInvestment) }}
            </template>

            <template #foot(newVacanciesCount)>
               {{ currency(totals.newVacanciesCount) }}
            </template>

            <template #foot(ownInvestment)>
               {{ currency(totals.ownInvestment) }}
            </template>
            <template #foot(privilegeBankCredit)>
               {{ currency(totals.privilegeBankCredit) }}
            </template>
            <template #foot(projectCost)>
               {{ currency(totals.projectCost) }}
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
            contractTypeId: null,
            regionId: null,
            region: '',
            byRegion: false,
            districtId: null,
            district: '',
            byDistrict: false,
            contractorId: null,
            byContractor: false,
            byContractType: false,
            contractorInn: null,
            bankId: null,
            byBank: false,
            bank: '',
            mainBank: '',
            mainBankId: null,
            byMainBank: true
         },
         isBusy: false,
         totals: {
            foreignInvestment: 0,
            newVacanciesCount: 0,
            ownInvestment: 0,
            privilegeBankCredit: 0,
            projectCost: 0
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
               key: this.filter.byMainBank ? 'mainBank' : '',
               label: this.$t('Bank'),
               sortable: true
            },
            // {
            //    key: this.filter.byDistrict ? 'district' : '',
            //    label: this.$t('Region'),
            //    sortable: true
            // },
            {
               key: this.filter.byBank ? 'bank' : '',
               label: this.$t('contractorT'),
               sortable: true,
               thStyle: {
                  minWidth: '350px'
               }
            },
            // {
            //    key: this.filter.byContractor ? 'contractorPhoneNumber' : '',
            //    label: this.$t('phoneNumber'),
            //    sortable: true
            // },
            {
               key: this.filter.byContractor ? 'contractor' : '',
               label: this.$t('prtnContractType'),
               sortable: true
            },
            {
               key: 'contractorCount',
               label: this.$t('contractorCount'),
               thClass: 'text-right',
               tdClass: 'text-right text-nowrap',
               sortable: true
            },

            {
               key: 'newVacanciesCount',
               label: this.$t('newVacanciesCount'),
               thClass: 'text-right',
               tdClass: 'text-right text-nowrap',
               sortable: true
            },
            {
               key: 'privilegeBankCredit',
               label: this.$t('privilegeBankCredit'),
               thClass: 'text-right',
               tdClass: 'text-right text-nowrap',
               sortable: true
            },
            {
               key: 'projectCost',
               label: this.$t('projectCost'),
               thClass: 'text-right',
               tdClass: 'text-right text-nowrap',
               sortable: true
            },
            {
               key: 'ownInvestment',
               label: this.$t('ownInvestment'),
               thClass: 'text-right',
               tdClass: 'text-right text-nowrap',
               sortable: true
            }
         ];
      },
      goToBussnes(inn) {
         this.$router.push({ name: 'BusinessmanCard', query: { inn: inn } });
      },
      SortMianBank(item) {
         this.filter.byBank = true;
         this.filter.byMainBank = false;
         this.filter.mainBankId = item.mainBankId;
         this.filter.mainBank = item.mainBank;
         this.Refresh();
      },
      SortBank(item) {
         this.filter.byContractor = true;
         this.filter.byBank = false;
         this.filter.bankId = item.bankId;
         this.filter.bank = item.bank;
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

         ReportService.SaveAsExecelForPrtnCreditBank(this.filter)
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
         ReportService.GetPrtnCreditDemandInfoByBank(this.filter)
            .then((res) => {
               this.items = res.data;

               this.totals = {
                  contractorCount: 0,
                  foreignInvestment: 0,
                  newVacanciesCount: 0,
                  ownInvestment: 0,
                  privilegeBankCredit: 0,
                  projectCost: 0
               };

               res.data.forEach((item) => {
                  this.totals.contractorCount += item.contractorCount;
                  this.totals.foreignInvestment += item.foreignInvestment;
                  this.totals.newVacanciesCount += item.newVacanciesCount;
                  this.totals.ownInvestment += item.ownInvestment;
                  this.totals.privilegeBankCredit += item.privilegeBankCredit;
                  this.totals.projectCost += item.projectCost;
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
