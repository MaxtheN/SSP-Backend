<template>
   <b-overlay :show="isBusy">
      <b-card>
         <b-row>
            <b-col cols="12" md="3">
               <div>
                  <label for>{{ $t('innOrPinfl') }}</label>
                  <b-input-group>
                     <b-form-input
                        type="text"
                        v-model="filter.contractorInn"
                        debounce="300"
                        v-mask="['##############']"
                        @keyup.enter="Refresh"
                        :placeholder="$t('innOrPinfl')"
                     />
                     <b-input-group-append>
                        <b-button @click="Refresh" size="sm" variant="primary">
                           <feather-icon icon="SearchIcon" />
                        </b-button>
                     </b-input-group-append>
                  </b-input-group>
               </div>
            </b-col>
            <b-col cols="12" md="3">
               <form-select
                  :options="RegionList"
                  :reduce="(item) => item.value"
                  label="region"
                  v-model="filter.regionId"
                  @input="ChangeRegion"
               />
            </b-col>
            <b-col cols="12" md="3">
               <form-select
                  :options="DistrictList"
                  :reduce="(item) => item.value"
                  label="Region"
                  v-model="filter.districtId"
                  @input="ChangeDistrict"
               />
            </b-col>

            <b-col cols="12" md="2">
               <div>
                  <label for>{{ $t('startdate') }}</label>
                  <form-picker v-model="filter.fromDocDate" :placeholder="$t('startdate')" @input="Refresh" />
               </div>
            </b-col>
            <b-col cols="12" md="2">
               <div>
                  <label for>{{ $t('enddate') }}</label>
                  <form-picker v-model="filter.toDocDate" :placeholder="$t('enddate')" @input="Refresh" />
               </div>
            </b-col>
            <b-col sm="12" md="2">
               <form-select
                  :options="AppealFormatTypeSelectList"
                  v-model="filter.appealFormatTypeId"
                  :label="$t('appealFormatType')"
                  rules="required"
                  @input="Refresh"
               ></form-select>
            </b-col>
            <b-col sm="12" md="2">
               <form-select
                  :options="AppealTypeSelectList"
                  v-model="filter.appealTypeId"
                  :label="$t('appealType')"
                  rules="required"
                  @input="Refresh"
               ></form-select>
            </b-col>
            <b-col sm="12" md="3">
               <form-select
                  :options="appealTypeArriveList"
                  v-model="filter.appealTypeArriveId"
                  :label="$t('AppealTypeArrive')"
                  rules="required"
                  @input="Refresh"
               ></form-select>
            </b-col>
         </b-row>
         <b-row class="px-1">
            <b-table-simple class="report-table" hover small caption-top responsive border>
               <b-thead>
                  <b-tr>
                     <b-th rowspan="2"> {{ $t('№') }}</b-th>
                     <b-th rowspan="2"> {{ $t('Murojaat raqami va sanasi') }}</b-th>
                     <b-th rowspan="2"> {{ $t('Mulkchilik turi') }}</b-th>
                     <b-th rowspan="2"> {{ $t('Murojaatchi F.I.SH') }}</b-th>
                     <b-th rowspan="2" style="width: 200px"> {{ $t('Yuridik shaxs INN va nomi') }}</b-th>
                     <b-th rowspan="2"> {{ $t('phoneNumber') }}</b-th>
                     <b-th rowspan="2"> {{ $t('Murojaat holati') }}</b-th>
                     <b-th colspan="5"> {{ $t("e-doc ma'lumotlari") }}</b-th>
                  </b-tr>
                  <b-tr>
                     <b-th>{{ $t('Ijroga yuborilgan tashkilot') }}</b-th>
                     <b-th>{{ $t('Registratsiya raqam va sanasi') }}</b-th>
                     <b-th>{{ $t('ijrochi F.I.O.') }}</b-th>
                     <b-th>{{ $t('ijro muddati') }}</b-th>
                     <b-th>{{ $t('Javob xati chiqan sana') }}</b-th>
                  </b-tr>
               </b-thead>
               <b-tbody>
                  <b-tr v-for="(item, i) in items" :key="item.id">
                     <b-td style="width: 40px">{{ i + 1 }}</b-td>

                     <b-td
                        ><div style="width: 150px; text-align: center">
                           {{ item.docNumber }} <br />
                           {{ item.docOn }}
                        </div></b-td
                     >
                     <b-td>
                        {{ item.appealType }}
                     </b-td>
                     <b-td>
                        <div style="width: 300px; white-space: wrap">{{ item.personName }}</div>
                     </b-td>
                     <b-td>
                        <div style="width: 300px; white-space: wrap">
                           <span style="color: blue"> {{ item.contractorInn }}</span> - {{ item.contractor }}
                        </div>
                     </b-td>
                     <b-td> {{ item.phoneNumber }} </b-td>
                     <b-td> {{ item.status }} </b-td>
                     <b-td
                        ><div style="width: 400px; white-space: wrap">
                           {{ item.edocInfoForList?.organization }}
                        </div></b-td
                     >
                     <b-td>
                        <span class="mr-2">{{ item.edocInfoForList?.regNumber }}</span>
                        <span>{{ item.edocInfoForList?.regDate }}</span>
                     </b-td>
                     <b-td>
                        <div style="width: 300px; white-space: wrap">{{ item.edocInfoForList?.assignment }}</div></b-td
                     >
                     <b-td> {{ item.edocInfoForList?.termExecution }} </b-td>
                     <b-td> {{ item.edocInfoForList?.outgoingDocCreatedData }} </b-td>
                  </b-tr>
               </b-tbody>
            </b-table-simple>
         </b-row>
         <div class="mx-2 mb-2">
            <b-row>
               <b-col
                  cols="12"
                  sm="6"
                  class="d-flex align-items-center justify-content-center justify-content-sm-start"
               >
                  <span class="text-muted">
                     {{ $t('Showing') }} {{ firstNumber }} {{ $t('to') }} {{ lastNumber }} {{ $t('of') }}
                     {{ filter.total }}
                     {{ $t('entries') }}
                  </span>
                  <v-select
                     v-model="filter.pageSize"
                     :dir="$store.state.appConfig.isRTL ? 'rtl' : 'ltr'"
                     :options="filter.perPageOptions"
                     @input="Refresh"
                     :clearable="false"
                     class="per-page-selector d-inline-block ml-50 mr-1"
                  />
               </b-col>
               <!-- Pagination -->
               <b-col cols="12" sm="6" class="d-flex align-items-center justify-content-center justify-content-sm-end">
                  <b-pagination
                     v-model="filter.page"
                     :total-rows="filter.total"
                     :per-page="filter.pageSize"
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
      </b-card>
   </b-overlay>
</template>

<script>
import {
   BRow,
   BOverlay,
   BTableSimple,
   BTr,
   BTd,
   BTh,
   BTbody,
   BTfoot,
   BThead,
   BButton,
   BPagination,
   BTable,
   BCol,
   BCard,
   BBadge,
   BLink,
   VBTooltip,
   BFormInput,
   BInputGroup,
   BInputGroupAppend,
   BButtonGroup
} from 'bootstrap-vue';
import FormTableHrm from '@/components/forms/form-table-hrm.vue';

import AppealApplicationService from '@/services/appeal/AppealApplication.service';
import AppealDescriptionService from '@/services/appeal/AppealDescription.service';
import AppealTypeArriveService from '@/services/appeal/AppealTypeArrive.service';
import RegionService from '@/services/info/region.service';
import ManualService from '@/services/others/manual.service';
import DistrictService from '@/services/info/district.service';
import StatusSelect from '@/views/components/document/StatusSelect.vue';

export default {
   components: {
      BOverlay,
      BRow,
      BTableSimple,
      BTr,
      BTd,
      BTh,
      BTbody,
      BTfoot,
      BThead,
      BButton,
      BPagination,
      BTable,
      BCol,
      BCard,
      BBadge,
      BLink,
      BFormInput,
      BInputGroup,
      BInputGroupAppend,

      BCol,
      BButton,
      BButtonGroup,
      FormTableHrm,
      StatusSelect,
      BTableSimple,
      BTbody
   },
   directives: {
      'b-tooltip': VBTooltip
   },
   data() {
      return {
         isBusy: false,
         printLoding: false,
         items: [],
         RegionList: [],
         DistrictList: [],
         appealTypeArriveList: [],
         AppealFormatTypeSelectList: [],
         AppealTypeSelectList: [],
         AppealDescriptionSelectList: [],

         filter: {
            statusId: null,
            regionId: null,
            personFullName: '',
            fromDocOn: '',
            toDocOn: '',
            contractorInn: '',
            districtId: null,
            appealFormatTypeId: null,
            appealTypeId: null,
            appealTypeArriveId: null,
            appealDescriptionId: null,
            search: '',
            sortBy: '',
            orderType: 'asc',
            page: 1,
            pageSize: 20,
            perPageOptions: [10, 20, 50, 100],
            total: 0
         }
      };
   },
   computed: {
      firstNumber() {
         return (this.filter.page - 1) * this.filter.pageSize + 1;
      },
      lastNumber() {
         if (this.filter.total < this.filter.pageSize) {
            return this.filter.total;
         } else {
            if (this.filter.page * this.filter.pageSize > this.filter.total) {
               return this.filter.total;
            } else {
               return this.filter.page * this.filter.pageSize;
            }
         }
      }
   },
   created() {
      this.Refresh();
      RegionService.GetAsSelectList(211)
         .then((res) => {
            this.RegionList = res.data;
         })
         .catch(this.showApiError)
         .finally(() => {
            this.saveLoading = false;
         });
      ManualService.AppealTypeSelectList()
         .then((res) => {
            this.AppealTypeSelectList = res.data;
         })
         .catch(this.showApiError)
         .finally(() => {
            this.show = false;
         });
      ManualService.AppealFormatTypeSelectList()
         .then((res) => {
            this.AppealFormatTypeSelectList = res.data;
         })
         .catch(this.showApiError)
         .finally(() => {
            this.show = false;
         });
      AppealTypeArriveService.GetAsSelectList().then((res) => {
         this.appealTypeArriveList = res.data;
      });

      AppealDescriptionService.GetAsSelectList()
         .then((res) => {
            this.AppealDescriptionSelectList = res.data;
         })
         .catch(this.showApiError)
         .finally(() => {
            this.show = false;
         });
   },
   methods: {
      Print() {
         this.printLoding = true;
         AppealApplicationService.PrinAppealApplicationExcel(this.filter).then((res) => {
            this.forceFileDownload(res, this.$t('AppealApplication'));
            this.printLoding = false;
         });
      },
      ChangeRegion() {
         if (this.filter.regionId) {
            this.filter.districtId = null;
            this.GetDistrict();
         }
         this.Refresh();
      },
      GetDistrict() {
         if (this.filter.regionId) {
            DistrictService.GetAsSelectList(this.filter.regionId).then((res) => {
               this.DistrictList = res.data;
            });
         } else {
            this.filter.districtId = null;
            this.DistrictList = [];
            this.Refresh();
         }
      },
      ChangeDistrict() {
         this.Refresh();
      },

      Refresh() {
         this.isBusy = true;
         AppealApplicationService.GetList(this.filter)
            .then((res) => {
               this.items = res.data.rows;
               this.filter.total = res.data.total;
            })
            .finally(() => {
               this.isBusy = false;
            });
      }
   }
};
</script>
<style lang="scss" scoped>
@import '/src/@core/scss/tablestyle.scss';
</style>
