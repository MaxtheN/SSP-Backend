<template>
   <div>
      <b-card no-body>
         <div class="m-2">
            <b-row v-if="!isComponent">
               <b-col cols="12" md="3">
                  <form-picker v-model="filter.year" type="year" format="YYYY" :placeholder="$t('docyear')" />
               </b-col>
               <b-col cols="12" md="3">
                  <b-form-input
                     v-model="filter.stir"
                     :placeholder="$t('inn')"
                     v-mask="'#########'"
                     @change="checkCountry"
                  />
                  <!-- <b-input-group class="text-right">
                     
                     <b-input-group-append>
                        <b-button @click="Refresh" variant="primary">
                           <feather-icon icon="SearchIcon" />
                        </b-button>
                     </b-input-group-append>
                  </b-input-group> -->
               </b-col>
               <b-col sm="8" md="3" v-if="filter.year && filter.stir.length == 9">
                  <v-select
                     :options="CountryList"
                     v-model="filter.countryId"
                     :reduce="(item) => item.value"
                     label="text"
                     :placeholder="$t('Country')"
                  ></v-select>
               </b-col>
               <b-col sm="4">
                  <b-button @click="Refresh" variant="primary">
                     <feather-icon icon="SearchIcon" />
                  </b-button>
               </b-col>
               <b-col sm="12" md="3" class="text-right">
                  <b-button @click="Print" variant="primary" class="ml-1">
                     <feather-icon icon="PrinterIcon"></feather-icon>
                     {{ $t('Print') }}
                  </b-button>
               </b-col>
            </b-row>
            <div class="mt-1" v-if="!isBusy">
               <h5>
                  <span style="font-weight: 700">{{ $t('responsibleName') }} : </span>
                  <span>{{ organizationInn }}</span> - {{ organizationName }}
               </h5>
               <h5>
                  <span style="font-weight: 700">{{ $t('organization1adress') }}</span
                  >: {{ organisationAdress }}
               </h5>
            </div>
         </div>
      </b-card>
      <b-tabs pills>
         <b-tab :title="$t('export')">
            <b-overlay :show="isBusy">
               <b-card>
                  <b-row>
                     <b-col sm="12" md="12" class="text-left">
                        <b-table
                           :fields="fields"
                           :items="Data?.exports"
                           :busy="isBusy"
                           responsive
                           sticky-header="65vh"
                           striped
                           no-border-collapse
                           show-empty
                           hover
                           :empty-text="$t('NotFound')"
                           class="position-relative"
                        >
                           <template #cell(orderNumber)="{ item, index }">
                              <span>{{ index + 1 }}</span>
                           </template>
                           <template #cell(actions)="{ item, index }">
                              <b-link
                                 @click="$refs['ViewModal' + item.id].show()"
                                 style="margin-right: 5px"
                                 v-b-tooltip.hover.top="$t('View')"
                              >
                                 <feather-icon icon="EyeIcon"></feather-icon>
                              </b-link>
                              <b-modal :ref="'ViewModal' + item.id" hide-footer size="xl">
                                 <template #modal-title>
                                    <div style="font-size: 16px">
                                       {{ $t('View') }} - <b>{{ index + 1 }}</b>
                                    </div>
                                 </template>
                                 <b-card-text>
                                    <b-table
                                       :fields="fieldsGoods"
                                       :items="item.goods"
                                       :busy="isBusy"
                                       responsive
                                       sticky-header="65vh"
                                       striped
                                       no-border-collapse
                                       show-empty
                                       hover
                                       :empty-text="$t('NotFound')"
                                       class="position-relative"
                                    >
                                    </b-table>
                                 </b-card-text>
                              </b-modal>
                           </template>
                           <template v-slot:table-busy>
                              <div class="text-center text-primary my-2" style="vertical-align: middle">
                                 <b-spinner class="align-middle mr-2"></b-spinner>
                                 <strong>{{ $t('Loading') }}</strong>
                              </div>
                           </template>
                        </b-table>
                     </b-col>
                  </b-row>
               </b-card>
            </b-overlay>
         </b-tab>
         <b-tab :title="$t('import')">
            <b-overlay :show="isBusy">
               <b-card>
                  <b-row>
                     <b-col sm="12" md="12" class="text-left">
                        <b-table
                           :fields="fields"
                           :items="Data?.imports"
                           :busy="isBusy"
                           responsive
                           sticky-header="65vh"
                           striped
                           no-border-collapse
                           show-empty
                           hover
                           :empty-text="$t('NotFound')"
                           class="position-relative"
                        >
                           <template #cell(orderNumber)="{ item, index }">
                              <span>{{ index + 1 }}</span>
                           </template>
                           <template #cell(actions)="{ item, index }">
                              <b-link
                                 @click="$refs['ViewModal' + item.id].show()"
                                 style="margin-right: 5px"
                                 v-b-tooltip.hover.top="$t('View')"
                              >
                                 <feather-icon icon="EyeIcon"></feather-icon>
                              </b-link>
                              <b-modal :ref="'ViewModal' + item.id" hide-footer size="xl">
                                 <template #modal-title>
                                    <div style="font-size: 16px">
                                       {{ $t('View') }} - <b>{{ index + 1 }}</b>
                                    </div>
                                 </template>
                                 <b-card-text>
                                    <b-table
                                       :fields="fieldsGoods"
                                       :items="item.goods"
                                       :busy="isBusy"
                                       responsive
                                       sticky-header="65vh"
                                       striped
                                       no-border-collapse
                                       show-empty
                                       hover
                                       :empty-text="$t('NotFound')"
                                       class="position-relative"
                                    >
                                    </b-table>
                                 </b-card-text>
                              </b-modal>
                           </template>
                           <template v-slot:table-busy>
                              <div class="text-center text-primary my-2" style="vertical-align: middle">
                                 <b-spinner class="align-middle mr-2"></b-spinner>
                                 <strong>{{ $t('Loading') }}</strong>
                              </div>
                           </template>
                        </b-table>
                     </b-col>
                  </b-row>
               </b-card>
            </b-overlay>
         </b-tab>
      </b-tabs>
   </div>
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
   BTr,
   BTd,
   BTfoot,
   BTh,
   BThead,
   BTbody,
   BTableSimple,
   BAlert,
   BTabs,
   BTab,
   BOverlay
} from 'bootstrap-vue';
import ContractorService from '@/services/info/contractor.service';
import CountryService from '@/services/info/country.service';

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
      BTr,
      BTabs,
      BTab,
      BTd,
      BTfoot,
      BTh,
      BThead,
      BTbody,
      BTableSimple,
      BAlert,
      BOverlay
   },
   name: 'Index',
   directives: {
      'b-tooltip': VBTooltip,
      'b-modal': VBModal
   },
   props: {
      isComponent: {
         type: Boolean,
         default: false
      },
      Data: {
         type: Array,
         default: null
      }
   },
   data() {
      return {
         fieldsGoods: [
            {
               key: 'product_name',
               label: this.$t('product'),
               thClass: 'text-center',
               sortable: false
            },
            {
               key: 'numcontract',
               label: this.$t('numcontract'),
               thClass: 'text-center',
               sortable: false
            },
            {
               key: 'numbergoods',
               label: this.$t('numbergoods'),
               thClass: 'text-center',
               sortable: false
            },

            {
               key: 'netmassgoods',
               label: this.$t('netmassgoods'),
               thClass: 'text-center',
               sortable: false
            },
            {
               key: 'codetiftngoods',
               label: this.$t('codetiftngoods'),
               thClass: 'text-center',
               sortable: false
            },
            {
               key: 'additionalunitgoods',
               label: this.$t('additionalunitgoods'),
               thClass: 'text-center',
               sortable: false
            },
            {
               key: 'unitgoods',
               label: this.$t('unitgoods'),
               thClass: 'text-center',
               sortable: false
            },

            {
               key: 'valuegoods',
               label: this.$t('valuegoods'),
               thClass: 'text-center',
               sortable: false
            }
         ],
         CountryList: [],
         fields: [
            {
               key: 'orderNumber',
               label: this.$t('№'),
               thClass: 'text-center',
               sortable: false
            },
            {
               key: 'year',
               label: this.$t('docyear'),
               thClass: 'text-center',
               sortable: false
            },
            {
               key: 'ekimcountryFullName',
               label: this.$t('ekimcountryFullName'),
               thClass: 'text-center',
               sortable: false
            },
            {
               key: 'mode',
               label: this.$t('mode'),
               thClass: 'text-center',
               sortable: false
            },

            // {
            //    key: 'organization1name',
            //    label: this.$t('organization1name'),
            //    thClass: 'text-center',
            //    sortable: false
            // },

            // {
            //    key: 'organization1adress',
            //    label: this.$t('organization1adress'),
            //    thClass: 'text-center',
            //    sortable: false
            // },

            //{
            //  key: 'organization2name',
            //  label: this.$t('organization2name'),
            //  thClass: 'text-center',
            //   sortable: false
            //  },
            ///  {
            //    key: 'organization2adress',
            //   label: this.$t('organization2adress'),
            //   thClass: 'text-center',
            //    sortable: false
            //   },
            // {
            //    key: 'organizationtin',
            //    label: this.$t('organizationtin'),
            //    thClass: 'text-center',
            //    tdClass: 'text-center',
            //    sortable: false
            // },
            {
               key: 'typeincoterms',
               label: this.$t('typeincoterms'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: false
            },
            {
               key: 'typetransport',
               label: this.$t('typetransport'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: false
            },
            {
               key: 'actions',
               label: this.$t('actions'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: false
            }
         ],
         organizationName: '',
         organizationInn: '',
         organisationAdress: '',
         filter: {
            // stir: '206642759',
            // year: '2023'
            countryId: null,
            stir: '',
            year: new Date().getFullYear() + ''
         },
         // Data: [],
         isBusy: false
      };
   },
   created() {
      if (!this.isComponent && this.filter.countryId) {
         this.Refresh();
      }

      if (this.$route.query.inn) {
         this.filter.stir = this.$route.query.inn;
         this.Refresh();
      }
   },
   methods: {
      checkCountry(item) {
         if (item.length === 9 && this.filter.year) {
            CountryService.GetAsSelectListForBojXona({
               inn: item,
               year: this.filter.year
            })
               .then((res) => {
                  this.CountryList = res.data;
               })
               .catch(this.showApiError);
         }
      },
      Print() {
         if (this.filter.stir && this.filter.year) {
            ContractorService.SaveAsExcel(this.filter)
               .then((res) => {
                  this.forceFileDownload(res, this.$t('Bojxona'));
               })
               .catch((err) => {
                  this.showApiError(err);
               });
         } else {
            this.makeToast(this.$t('stirandYearNotSelected'), 'danger');
         }
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
      SortChange(data) {
         this.filter.sortBy = data.sortBy;
         this.filter.orderType = data.sortDesc ? 'desc' : 'asc';
         this.Refresh();
      },
      Refresh() {
         if (this.filter.stir && (this.isComponent || this.filter.year)) {
            this.isBusy = true;
            ContractorService.GetGTDFromBojxona(this.filter)
               .then((res) => {
                  this.Data = res.data;
                  this.organisationAdress = res.data.imports[0].organization2adress;
                  this.organizationName = res.data.imports[0].organization2name;
                  this.organizationInn = res.data.imports[0].organizationtin;
               })
               .catch((err) => {
                  this.showApiError(err);
               })
               .finally(() => {
                  this.isBusy = false;
               });
         } else {
            this.makeToast(this.$t('stirandYearNotSelected'), 'danger');
         }
      }
   }
};
</script>
