<template>
   <b-card no-body>
      <div class="m-2 report-table">
         <b-overlay :show="isBusy">
            <b-breadcrumb class="mb-1 mt-0">
               <b-breadcrumb-item
                  @click="
                     () => {
                        filter.region = '';
                        filter.regionId = null;
                        filter.districtId = null;
                        filter.byRegion = true;
                        filter.district = null;
                        Refresh();
                     }
                  "
               >
                  <b>{{ $t('uzb') }}</b>
               </b-breadcrumb-item>
               <b-breadcrumb-item
                  @click="
                     () => {
                        filter.districtId = null;
                        filter.byRegion = false;
                        filter.byDistrict = true;
                        Refresh();
                     }
                  "
               >
                  <b>{{ filter.region }}</b>
               </b-breadcrumb-item>
            </b-breadcrumb>
            <b-table-simple hover small caption-top responsive border>
               <b-thead>
                  <b-tr>
                     <b-th class="b-table-sticky-column" rowspan="3">
                        {{ $t('order') }}
                     </b-th>
                     <b-th rowspan="3" class="table-b-table-default b-table-sticky-column">
                        <span style="font-weight: 900; font-size: 14px; color: black">{{ $t('region') }}</span>
                     </b-th>
                     <b-th rowspan="2" colspan="3">
                        {{ $t('Jami Arizalar') }}
                     </b-th>
                     <b-th colspan="13">
                        {{ $t('ofThem') }}
                     </b-th>
                  </b-tr>
                  <b-tr>
                     <b-th colspan="3">
                        {{ $t('Юборилган') }}
                     </b-th>
                     <b-th colspan="3">
                        {{ $t('Яратилган') }}
                     </b-th>
                     <b-th colspan="3">
                        {{ $t('EXECUTING') }}
                     </b-th>
                     <b-th colspan="3">
                        {{ $t('Ijrosi tanimlagan') }}
                     </b-th>
                     <b-th rowspan="2">
                        {{ $t('rejects') }}
                     </b-th>
                  </b-tr>
                  <b-tr>
                     <template v-for="item in 5">
                        <b-th :key="item + 'hjkh'">
                           {{ $t('Application') }}
                        </b-th>
                        <b-th :key="item + 'hjdkh'">
                           {{ $t('Taklif') }}
                        </b-th>
                        <b-th :key="item + 'hjkddh'">
                           {{ $t('Shikoyat') }}
                        </b-th>
                     </template>
                  </b-tr>
               </b-thead>
               <b-tbody>
                  <b-tr v-for="(item, idx) in items" :key="idx + 'abc'">
                     <b-td class="b-table-sticky-column">
                        {{ idx + 1 }}
                     </b-td>
                     <b-td class="table-b-table-default b-table-sticky-column">
                        <span v-if="filter.byRegion" style="color: blue; cursor: pointer" @click="SortRegion(item)">
                           {{ item.regionName }}
                        </span>
                        <span v-if="filter.byDistrict" style="color: blue; cursor: pointer" @click="SortDistrict(item)">
                           {{ item.districtName }}
                        </span>
                     </b-td>
                     <b-td class="text-right"> {{ item.totalAppealApplicationType?.item1 }} </b-td>
                     <b-td class="text-right"> {{ item.totalAppealApplicationType?.item2 }} </b-td>
                     <b-td class="text-right"> {{ item.totalAppealApplicationType?.item3 }} </b-td>

                     <b-td class="text-right"> {{ item.totalAppealApplicationTypeSent?.item1 }} </b-td>
                     <b-td class="text-right"> {{ item.totalAppealApplicationTypeSent?.item2 }} </b-td>
                     <b-td class="text-right"> {{ item.totalAppealApplicationTypeSent?.item3 }} </b-td>

                     <b-td class="text-right"> {{ item.totalAppealApplicationTypeCreate?.item1 }} </b-td>
                     <b-td class="text-right"> {{ item.totalAppealApplicationTypeCreate?.item2 }} </b-td>
                     <b-td class="text-right"> {{ item.totalAppealApplicationTypeCreate?.item3 }} </b-td>

                     <b-td class="text-right"> {{ item.totalAppealApplicationTypeInExecution?.item1 }} </b-td>
                     <b-td class="text-right"> {{ item.totalAppealApplicationTypeInExecution?.item2 }} </b-td>
                     <b-td class="text-right"> {{ item.totalAppealApplicationTypeInExecution?.item3 }} </b-td>

                     <b-td class="text-right"> {{ item.totalAppealApplicationTypeExecuted?.item1 }} </b-td>
                     <b-td class="text-right"> {{ item.totalAppealApplicationTypeExecuted?.item2 }} </b-td>
                     <b-td class="text-right"> {{ item.totalAppealApplicationTypeExecuted?.item3 }} </b-td>

                     <b-td class="text-right"> {{ item.totalAppealApplicationCanceled }} </b-td>
                  </b-tr>
               </b-tbody>
               <b-tfoot v-if="items.length > 0">
                  <b-tr variant="secondary">
                     <b-td class="text-right b-table-sticky-column" colspan="2">{{ $t('Total') }}</b-td>
                     <b-td class="text-right"> {{ totals.totalAppealApplicationType.item1 }} </b-td>
                     <b-td class="text-right"> {{ totals.totalAppealApplicationType.item2 }} </b-td>
                     <b-td class="text-right"> {{ totals.totalAppealApplicationType.item3 }} </b-td>

                     <b-td class="text-right"> {{ totals.totalAppealApplicationTypeSent.item1 }} </b-td>
                     <b-td class="text-right"> {{ totals.totalAppealApplicationTypeSent.item2 }} </b-td>
                     <b-td class="text-right"> {{ totals.totalAppealApplicationTypeSent.item3 }} </b-td>

                     <b-td class="text-right"> {{ totals.totalAppealApplicationTypeCreate.item1 }} </b-td>
                     <b-td class="text-right"> {{ totals.totalAppealApplicationTypeCreate.item2 }} </b-td>
                     <b-td class="text-right"> {{ totals.totalAppealApplicationTypeCreate.item3 }} </b-td>

                     <b-td class="text-right"> {{ totals.totalAppealApplicationTypeInExecution.item1 }} </b-td>
                     <b-td class="text-right"> {{ totals.totalAppealApplicationTypeInExecution.item2 }} </b-td>
                     <b-td class="text-right"> {{ totals.totalAppealApplicationTypeInExecution.item3 }} </b-td>

                     <b-td class="text-right"> {{ totals.totalAppealApplicationTypeExecuted.item1 }} </b-td>
                     <b-td class="text-right"> {{ totals.totalAppealApplicationTypeExecuted.item2 }} </b-td>
                     <b-td class="text-right"> {{ totals.totalAppealApplicationTypeExecuted.item3 }} </b-td>

                     <b-td class="text-right"> {{ totals.totalAppealApplicationCanceled }} </b-td>
                  </b-tr>
               </b-tfoot>
            </b-table-simple>
         </b-overlay>
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
import DistrictService from '@/services/info/district.service';
export default {
   components: {
      BButton,
      BPagination,
      BTable,
      BTab,
      BTabs,
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
         items: [],
         localStorageData: [],
         DistrictList: [],
         totals: {},
         filter: {
            organizationId: null,
            byRegion: true,
            regionId: null,
            region: '',
            byDistrict: false,
            districtId: null,
            page: 1,
            pageSize: 20,
            perPageOptions: [10, 20, 50, 100, 300],
            total: 0
         }
      };
   },
   created() {
      this.Refresh();
   },
   methods: {
      GetDistrict(id) {
         DistrictService.GetAsSelectList(id)
            .then((res) => {
               console.log(res);
               this.DistrictList = res.data;
            })
            .catch((error) => {
               // this.showApiError(error);
            });
      },
      Refresh() {
         this.isBusy = true;
         ReportService.GetAppealApplicationReport(this.filter)
            .then((res) => {
               this.isBusy = false;
               this.items = res.data;

               this.totals = {
                  totalAppealApplicationType: {
                     item1: 0,
                     item2: 0,
                     item3: 0
                  },
                  totalAppealApplicationTypeSent: {
                     item1: 0,
                     item2: 0,
                     item3: 0
                  },
                  totalAppealApplicationTypeCreate: {
                     item1: 0,
                     item2: 0,
                     item3: 0
                  },
                  totalAppealApplicationTypeInExecution: {
                     item1: 0,
                     item2: 0,
                     item3: 0
                  },
                  totalAppealApplicationTypeExecuted: {
                     item1: 0,
                     item2: 0,
                     item3: 0
                  },
                  totalAppealApplicationCanceled: 0
               };

               this.items.forEach((item) => {
                  this.totals.totalAppealApplicationType.item1 += item.totalAppealApplicationType.item1;
                  this.totals.totalAppealApplicationType.item2 += item.totalAppealApplicationType.item2;
                  this.totals.totalAppealApplicationType.item3 += item.totalAppealApplicationType.item3;

                  this.totals.totalAppealApplicationTypeCreate.item1 += item.totalAppealApplicationTypeCreate.item1;
                  this.totals.totalAppealApplicationTypeCreate.item2 += item.totalAppealApplicationTypeCreate.item2;
                  this.totals.totalAppealApplicationTypeCreate.item3 += item.totalAppealApplicationTypeCreate.item3;

                  this.totals.totalAppealApplicationTypeInExecution.item1 +=
                     item.totalAppealApplicationTypeInExecution.item1;
                  this.totals.totalAppealApplicationTypeInExecution.item2 +=
                     item.totalAppealApplicationTypeInExecution.item2;
                  this.totals.totalAppealApplicationTypeInExecution.item3 +=
                     item.totalAppealApplicationTypeInExecution.item3;

                  this.totals.totalAppealApplicationTypeExecuted.item1 += item.totalAppealApplicationTypeExecuted.item1;
                  this.totals.totalAppealApplicationTypeExecuted.item2 += item.totalAppealApplicationTypeExecuted.item2;
                  this.totals.totalAppealApplicationTypeExecuted.item3 += item.totalAppealApplicationTypeExecuted.item3;

                  this.totalAppealApplicationCanceled += item.totalAppealApplicationCanceled;
               });
            })
            .catch((error) => {
               this.isBusy = false;
               this.showApiError(error);
            });
      },
      SortRegion(item) {
         this.filter.regionId = item.regionId;
         this.filter.region = item.regionName;
         this.filter.byDistrict = true;
         this.filter.byRegion = false;
         this.Refresh();
      }
   }
};
</script>
<style lang="scss" scoped>
@import '/src/@core/scss/tablestyle.scss';
</style>
