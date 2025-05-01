<template>
   <b-card no-body>
      <div class="m-2">
         <b-row>
            <b-col sm="6" md="2">
               <b-button variant="success" :to="{ name: 'EditProposal', params: { id: 0 } }">
                  <b-img src="@/assets/images/incoming.png" width="17px" style="margin-right: 2px"></b-img>
                  {{ $t('NewCall') }}
               </b-button>
            </b-col>
            <b-col sm="6" md="2">
               <b-button class="mr-1" variant="primary" @click="Print">
                  <b-icon-file-earmark-excel></b-icon-file-earmark-excel>
                  {{ $t('Load') }}
               </b-button>
            </b-col>

            <b-col sm="12" md="2">
               <form-picker
                  v-model="filter.fromDocDate"
                  @input="Refresh"
                  style="width: 100%"
                  size="sm"
                  lang="ru"
                  :placeholder="$t('fromdate')"
                  value-type="format"
                  format="DD.MM.YYYY"
               ></form-picker>
            </b-col>
            <b-col sm="12" md="2" class="text-left">
               <form-picker
                  v-model="filter.toDocDate"
                  @input="Refresh"
                  style="width: 100%"
                  size="sm"
                  lang="ru"
                  :placeholder="$t('todate')"
                  value-type="format"
                  format="DD.MM.YYYY"
               ></form-picker>
            </b-col>
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
         <b-row class="mt-1">
            <b-col>
               <b-button-group @click="Refresh" size="sm">
                  <b-button
                     @click="filter.externalSourctTypeId = null"
                     :variant="filter.externalSourctTypeId == null ? 'primary' : 'outline-primary'"
                     >{{ $t('all') }}</b-button
                  >
                  <b-button
                     @click="filter.externalSourctTypeId = 1"
                     :variant="filter.externalSourctTypeId == 1 ? 'primary' : 'outline-primary'"
                     >{{ $t('Proposal1') }}</b-button
                  >
                  <b-button
                     @click="filter.externalSourctTypeId = 2"
                     :variant="filter.externalSourctTypeId == 2 ? 'primary' : 'outline-primary'"
                     >{{ $t('website') }}</b-button
                  >
                  <b-button
                     @click="filter.externalSourctTypeId = 3"
                     :variant="filter.externalSourctTypeId == 3 ? 'primary' : 'outline-primary'"
                     >{{ $t('Telegram') }}</b-button
                  >
               </b-button-group>
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
         <template #cell(companyName)="{ item }">
            <span>{{ item.companyInn }}-{{ item.companyName }}</span>
         </template>
         <template #cell(phoneNumber)="{ item }">
            <span style="white-space: nowrap !important">{{ item.phoneNumber }}</span>
         </template>
         <template #cell(nameLatin)="{ item }">
            <span>{{ item.surnameLatin }} {{ item.nameLatin }}</span>
         </template>

         <template #cell(appealText)="{ item }">
            <span class="show-more" :title="item.appealText">{{ item.appealText }}</span>
         </template>

         <template #cell(proposalText)="{ item }">
            <span class="show-more" :title="item.proposalText">{{ item.proposalText }}</span>
         </template>

         <template #cell(actions)="{ item }">
            <div class="text-center">
               <!-- v-if="$store.state.auth.organizationtype !== 4 && $can('RoleEdit','permissions')" -->
               <b-link
                  v-if="$can('ProposalEdit', 'permissions')"
                  :to="{ name: 'EditProposal', params: { id: item.id } }"
                  style="margin-right: 5px; cursor: pointer"
                  v-b-tooltip.hover.top="$t('Edit')"
               >
                  <feather-icon icon="EditIcon"></feather-icon>
               </b-link>
               <!-- v-if="$store.state.auth.organizationtype !== 4 && $can('RoleDelete','permissions')" -->
               <!-- <b-link v-if="$can('UserDelete', 'permissions')" @click="$refs['DeleteModal' + item.id].show()" style="cursor: pointer" v-b-tooltip.hover.top="$t('Delete')">
                  <feather-icon icon="TrashIcon"></feather-icon>
               </b-link>-->
            </div>
            <!-- <b-modal :ref="'DeleteModal' + item.id" :cancel-title="$t('Cancel')" :ok-title="$t('Accept')" cancel-variant="danger" ok-variant="success" @ok="Delete(item)">
               <template #modal-title>
                  {{ $t('Accept') }}
                  <b-spinner v-if="DeleteLoading" small></b-spinner>
               </template>
               <b-card-text>
                  <h5>ID : {{ item.id }}</h5>
                  <h5>{{ $t('WantDelete') }}</h5>
               </b-card-text>
            </b-modal>-->
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
            <b-col cols="12" sm="6" class="d-flex align-items-center justify-content-center justify-content-sm-start">
               <span class="text-muted"
                  >{{ $t('Showing') }} {{ firstNumber }} {{ $t('to') }} {{ lastNumber }} {{ $t('of') }}
                  {{ filter.total }} {{ $t('entries') }}</span
               >

               <v-select
                  v-model="filter.pageSize"
                  :dir="$store.state.appConfig.isRTL ? 'rtl' : 'ltr'"
                  :options="filter.perPageOptions"
                  :clearable="false"
                  @input="Refresh"
                  class="per-page-selector d-inline-block ml-50 mr-1"
               />
            </b-col>

            <!-- v-if="$store.state.auth.organizationtype !== 4 && $can('RoleEdit','permissions')" -->

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
</template>

<script>
import {
   BSpinner,
   BButton,
   BButtonGroup,
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
   BLink,
   BCardText,
   BImg,
   BIconFileEarmarkExcel
} from 'bootstrap-vue';
import ProposalService from '@/services/document/proposal.service';

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
      BModal,
      BLink,
      BCardText,
      BButtonGroup,
      BImg,
      BIconFileEarmarkExcel
   },
   directives: {
      'b-tooltip': VBTooltip
   },
   data() {
      return {
         items: [],
         fields: [
            {
               key: 'id',
               label: this.$t('id'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'docNumber',
               label: this.$t('docnumber'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'docOn',
               label: this.$t('docOn'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'externalSourceTypeName',
               label: this.$t('externalSourceType'),
               sortable: true
            },
            {
               key: 'proposalTypeName',
               label: this.$t('proposalType'),
               sortable: true
            },
            {
               key: 'companyTypeName',
               label: this.$t('companyType'),
               sortable: true
            },
            {
               key: 'businessSectorName',
               label: this.$t('businessSector'),
               sortable: true
            },
            {
               key: 'phoneNumber',
               label: this.$t('phoneNumber'),
               sortable: true
            },

            {
               key: 'nameLatin',
               label: this.$t('nameLatin'),
               sortable: true
            },
            {
               key: 'companyName',
               label: this.$t('companyName'),
               sortable: true
            },
            // {
            //    key: 'gender',
            //    label: this.$t('gender'),
            //    sortable: true
            // },

            {
               key: 'regionName',
               label: this.$t('region'),
               sortable: true
            },
            {
               key: 'districtName',
               label: this.$t('District'),
               sortable: true
            },
            {
               key: 'mfyName',
               label: this.$t('mfy'),
               sortable: true
            },
            {
               key: 'addressName',
               label: this.$t('address'),
               sortable: true
            },
            // {
            //    key: 'employementTypeName',
            //    label: this.$t('employementType'),
            //    sortable: true
            // },
            {
               key: 'proposalSubjectName',
               label: this.$t('proposalSubject'),
               sortable: true
            },
            {
               key: 'appealText',
               label: this.$t('appealText'),
               sortable: true
            },

            {
               key: 'proposalText',
               label: this.$t('proposalText'),
               sortable: true
            },

            {
               key: 'actions',
               label: this.$t('actions'),
               thClass: 'text-center',
               tdClass: 'text-center'
            }
         ],
         PrintLoading: false,
         filter: {
            search: '',
            externalSourctTypeId: null,
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
      }
   },
   created() {
      var todaydate = new Date();
      var dd = String(todaydate.getDate()).padStart(2, '0');
      var mm = String(todaydate.getMonth() + 1).padStart(2, '0');
      var yyyy = todaydate.getFullYear();
      this.filter.fromDocDate = '01.' + mm + '.' + yyyy;
      this.filter.toDocDate = dd + '.' + mm + '.' + yyyy;
      this.Refresh();
   },
   methods: {
      Print() {
         this.PrintLoading = true;
         ProposalService.SaveAsExcel(this.filter)
            .then((res) => {
               this.PrintLoading = false;
               this.makeToast(this.$t('DownloadSuccess'));
               this.forceFileDownload(res, this.$t('Proposal1'));
            })
            .catch((error) => {
               this.PrintLoading = false;
               this.makeToast(error.response.data);
            });
      },
      forceFileDownload(response, name) {
         var { headers } = response;
         var blob = new Blob([response.data]);
         const url = window.URL.createObjectURL(blob);
         const link = document.createElement('a');
         link.href = url;
         link.setAttribute('download', name + '.xlsx'); //or any other extension
         document.body.appendChild(link);
         link.click();
      },
      DbClick(item) {
         this.$router.push({ name: 'EditProposal', params: { id: item.id } });
      },
      SortChange(data) {
         this.filter.sortBy = data.sortBy;
         this.filter.orderType = data.sortDesc ? 'desc' : 'asc';
         this.Refresh();
      },
      Delete(item) {
         this.DeleteLoading = true;
         ProposalService.Delete(item.id)
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
         ProposalService.GetList(this.filter).then((res) => {
            this.items = res.data.rows;
            this.filter.total = res.data.total;
            this.isBusy = false;
         });
      }
   }
};
</script>

<style lang="scss" scoped>
.show-more {
   display: inline-block;
   white-space: nowrap;
   width: 200px;
   overflow: hidden;
   text-overflow: ellipsis;
}
</style>
