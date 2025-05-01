<template>
   <b-card no-body>
      <div class="m-2">
         <b-row>
            <b-col v-if="loacalData.organizationId == 1" cols="12" md="3">
               <form-select
                  :options="OrganisationList"
                  v-model="filter.organizationId"
                  :label="$t('organization')"
                  @change="Refresh"
               />
            </b-col>

            <b-col cols="12" md="3">
               <form-select
                  :options="RegionList"
                  v-model="filter.regionId"
                  @input="ChangeRegion"
                  label="Oblast"
               ></form-select>
            </b-col>
            <b-col cols="12" md="2">
               <form-select
                  :options="DistrictList"
                  placeholder="ChooseBelow"
                  label="Region"
                  v-model="filter.districtId"
                  @input="ChangeDistrict"
               />
            </b-col>
            <b-col sm="12" md="2">
               <form-input-hrm
                  v-model="filter.minAge"
                  @input="Refresh"
                  :label="$t('minAge')"
                  :placeholder="$t('minAge')"
                  v
               />
            </b-col>
            <b-col sm="12" md="2" class="mb-1">
               <form-input-hrm
                  v-model="filter.maxAge"
                  @input="Refresh"
                  :label="$t('maxAge')"
                  :placeholder="$t('maxAge')"
                  v
               />
            </b-col>
         </b-row>
         <b-row>
            <b-col cols="12" md="6" class="d-flex align-items-center justify-content-start mb-1 mb-md-0">
               <b-button variant="primary" :to="{ name: 'EditEmployee', params: { id: 0 } }">
                  <feather-icon icon="PlusIcon"></feather-icon>
                  {{ $t('create') }}
               </b-button>
            </b-col>
            <b-col md="2"></b-col>
            <b-col cols="12" md="4">
               <b-input-group class="text-right">
                  <b-form-input v-model="filter.search" @keyup.enter="Refresh" :placeholder="$t('search')" />
                  <b-input-group-append>
                     <b-button @click="Refresh" variant="primary">
                        <feather-icon icon="SearchIcon" />
                     </b-button>
                  </b-input-group-append>
               </b-input-group>
            </b-col>
         </b-row>
      </div>

      <b-table
         ref="refInvoiceListTable"
         :items="items"
         responsive
         :fields="fields"
         primary-key="id"
         sticky-header="65vh"
         no-border-collapse
         :busy="isBusy"
         show-empty
         :empty-text="$t('NotFound')"
         class="position-relative"
         @sort-changed="SortChange"
         @row-dblclicked="DbClick"
      >
         <template #cell(status)="{ item }">
            <b-badge :variant="item.status == 'Пассив' ? 'light-danger' : 'light-success'">{{ item.status }}</b-badge>
         </template>
         <template #cell(fullName)="{ item }">
            <b-link :to="{ name: 'EmployeeCard', query: { pinfl: item.pinfl } }">{{ item.fullName }}</b-link>
         </template>
         <template #cell(pinfl)="{ item }">
            <b-link :to="{ name: 'EmployeeCard', query: { pinfl: item.pinfl } }">{{ item.pinfl }}</b-link>
         </template>
         <template #cell(actions)="{ item }">
            <div class="text-center d-flex no-wrap">
               <b-link
                  v-if="$can('EmployeeEdit', 'permissions')"
                  :to="{ name: 'EditEmployee', params: { id: item.id } }"
                  class="cursor-pointer mr-1"
                  v-b-tooltip.hover.top="$t('Edit')"
               >
                  <feather-icon icon="EditIcon"></feather-icon>
               </b-link>
               <b-link
                  v-if="$can('EmployeeDelete', 'permissions')"
                  @click="$refs['DeleteModal' + item.id].show()"
                  class="cursor-pointer mr-1 text-danger"
                  v-b-tooltip.hover.top="$t('Delete')"
               >
                  <feather-icon icon="TrashIcon"></feather-icon>
               </b-link>
            </div>
            <b-modal
               :ref="'DeleteModal' + item.id"
               :cancel-title="$t('Cancel')"
               :ok-title="$t('Accept')"
               cancel-variant="danger"
               ok-variant="success"
               @ok="Delete(item)"
            >
               <template #modal-title>
                  {{ $t('Accept') }}
                  <b-spinner v-if="DeleteLoading" small></b-spinner>
               </template>
               <b-card-text>
                  <h5>ID : {{ item.id }}</h5>
                  <h5>{{ $t('WantDelete') }}</h5>
               </b-card-text>
            </b-modal>
         </template>
         <template #cell(pictureId)="{ item }">
            <img width="60" :src="userImg(item.pictureId)" alt="" />
         </template>
         <template #cell(workYear)="{ item }">
            <span style="text-transform: lowercase"
               >{{ item.totalWorkedYear || 0 }} {{ $t('docyear') }} {{ item.totalWorkedMonth || 0 }} {{ $t('month') }}
               {{ item.totalWorkedDay || 0 }} {{ $t('dayNumber') }}</span
            >
         </template>
         <template #cell(birthDate)="{ item }">
            <span> {{ item.birthDate }} ({{ calculateaAge(item.birthDate) }})</span>
         </template>
         <template v-slot:table-busy>
            <div class="text-center text-primary my-2" style="vertical-align: middle">
               <b-spinner class="align-middle mr-2"></b-spinner>
               <strong>{{ $t('Loading') }}</strong>
            </div>
         </template>
      </b-table>
      <div class="mx-2 mb-2">
         <b-row>
            <b-col
               cols="auto"
               class="d-flex align-items-center justify-content-center justify-content-sm-start cols-auto"
            >
               <span class="text-muted">
                  {{ $t('Showing') }} {{ firstNumber }} {{ $t('to') }} {{ lastNumber }} {{ $t('of') }}
                  {{ filter.total }} {{ $t('entries') }}
               </span>
            </b-col>
            <b-col cols="auto">
               <v-select
                  v-model="filter.pageSize"
                  :dir="$store.state.appConfig.isRTL ? 'rtl' : 'ltr'"
                  :options="filter.perPageOptions"
                  :clearable="false"
                  @input="Refresh"
                  class="per-page-selector d-inline-block ml-50 mr-1"
               />
            </b-col>
            <!-- Pagination -->
            <b-col cols="12" sm="8" class="d-flex align-items-center justify-content-center justify-content-sm-end">
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
</template>

<script>
import {
   BSpinner,
   BButton,
   BPagination,
   BTable,
   BCol,
   BRow,
   BCard,
   BTooltip,
   BBadge,
   BInputGroup,
   BFormInput,
   BInputGroupAppend,
   VBTooltip,
   BModal,
   BFormCheckbox,
   BLink,
   BCardText
} from 'bootstrap-vue';
import EmployeeService from '@/services/info/employee.service';
import axios from 'axios';
import RegionService from '@/services/info/region.service';
import DistrictService from '@/services/info/district.service';
import ManualService from '@/services/others/manual.service';

const DefaultFilter = {
   organizationId: null,
   regionId: null,
   // isInitQuery: true,
   search: '',
   sortBy: '',
   minAge: null,
   maxAge: null,
   orderType: 'asc',
   page: 1,
   pageSize: 20,
   perPageOptions: [10, 20, 50, 100],
   total: 0
};

export default {
   components: {
      BButton,
      BFormCheckbox,
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
      BModal,
      BLink,
      BCardText
   },
   directives: {
      'b-tooltip': VBTooltip
   },

   data() {
      return {
         loacalData: JSON.parse(localStorage.getItem('user_info')),
         items: [],
         RegionList: [],
         DistrictList: [],
         OrganisationList: [],
         fields: [
            {
               key: 'id',
               label: this.$t('id'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'fullName',
               label: this.$t('fio'),
               sortable: true
            },
            {
               key: 'passportInfo',
               label: this.$t('passportNumber'),
               thClass: 'text-center',
               tdClass: 'text-center text-nowrap'
            },
            {
               key: 'pinfl',
               label: this.$t('pinfl'),
               thClass: 'text-center',
               tdClass: 'text-center'
            },
            {
               key: 'phoneNumber',
               label: this.$t('phoneNumber'),
               thClass: 'text-center',
               tdClass: 'text-center text-nowrap'
            },
            {
               key: 'birthDate',
               label: this.$t('birthDate'),
               thClass: 'text-center',
               tdClass: 'text-center',
               thStyle: {
                  minWidth: '200px'
               }
            },
            {
               key: 'workYear',
               label: this.$t('yearWorkExp'),
               thClass: 'text-center',
               tdClass: 'text-center',
               thStyle: {
                  minWidth: '200px'
               }
            },
            {
               key: 'pictureId',
               label: this.$t('photo'),
               thClass: 'text-center',
               tdClass: 'text-center'
            },
            {
               key: 'organization',
               label: this.$t('organization'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'actions',
               label: this.$t('actions'),
               thClass: 'text-center',
               tdClass: 'text-center'
            }
         ],
         filter: {
            organizationId: null,
            regionId: null,
            // isInitQuery: true,
            search: '',
            sortBy: '',
            orderType: 'asc',
            page: 1,
            pageSize: 20,
            perPageOptions: [10, 20, 50, 100],
            total: 0
         },
         isBusy: false,
         DeleteLoading: false
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
      },
      userImg() {
         return (pictureId) => [axios.defaults.baseURL + 'Person/DownloadFile/' + pictureId];
      },

      calculateaAge() {
         return (birthDate) => {
            const [day, month, year] = birthDate.split('.').map(Number);

            // Create a new Date object with the birth year, month (0-indexed), and day
            const birthDateObj = new Date(year, month - 1, day);

            // Get the current date
            const currentDate = new Date();

            // Calculate the age
            let age = currentDate.getFullYear() - birthDateObj.getFullYear();

            // Check if the current date hasn't reached the birth month and day yet
            if (
               currentDate.getMonth() < birthDateObj.getMonth() ||
               (currentDate.getMonth() === birthDateObj.getMonth() && currentDate.getDate() < birthDateObj.getDate())
            ) {
               age--;
            }
            return age;
         };
      }
   },
   created() {
      if (JSON.parse(localStorage.getItem('filterData1'))) {
         this.filter = JSON.parse(localStorage.getItem('filterData1'));
      } else {
         this.filter = DefaultFilter;
      }
      this.Refresh();
      ManualService.OrganizationAsSelectListByGroup([1, 3]).then((res) => {
         this.OrganisationList = res.data;
      });
      RegionService.GetAsSelectList(211)
         .then((res) => {
            this.RegionList = res.data;
         })
         .catch((err) => {
            this.showApiError(err);
         })
         .finally(() => {
            this.saveLoading = false;
         });
   },
   methods: {
      DbClick(item) {
         this.$router.push({ name: 'EditEmployee', params: { id: item.id } });
      },
      SortChange(data) {
         this.filter.sortBy = data.sortBy;
         this.filter.orderType = data.sortDesc ? 'desc' : 'asc';
         this.Refresh();
      },
      Delete(item) {
         this.DeleteLoading = true;
         EmployeeService.Delete(item.id)
            .then((res) => {
               this.DeleteLoading = false;
               this.makeToast(this.$t('DeleteSuccess'), 'success');
               this.Refresh();
            })
            .catch((error) => {
               this.showApiError(error);
               this.DeleteLoading = false;
            });
      },
      Refresh() {
         this.isBusy = true;
         EmployeeService.GetList(this.filter).then((res) => {
            this.items = res.data.rows;
            this.filter.total = res.data.total;
            this.isBusy = false;
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
      }
   },
   watch: {
      filter: {
         handler(newValue, oldValue) {
            localStorage.setItem('filterData1', JSON.stringify(newValue));
            this.filter = newValue;
            if (newValue.districtId) {
               this.GetDistrict();
            }
         },
         deep: true
      }
   }
};
</script>

<style lang="scss" scoped></style>
