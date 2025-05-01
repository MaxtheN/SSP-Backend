<template>
   <b-card>
      <div>
         <b-row class="my-2">
            <!-- <b-col sm="12" md="3">
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
                     @input="ChangeDistrict"
                     class="w-100"
                  ></v-select>
               </div>
            </b-col> -->
            <b-col sm="12" md="4"
               ><b-button @click="Print" :disabled="printLoding" variant="primary">
                  <feather-icon icon="PrinterIcon"></feather-icon>
                  {{ $t('Print') }}
               </b-button></b-col
            >
            <!-- <b-col></b-col>
            <b-col cols="12" md="3">
               <b-input-group class="text-right">
                  <b-form-input v-model="filters.inn" @keyup.enter="Refresh" :placeholder="$t('inn')" />
                  <b-input-group-append>
                     <b-button @click="Refresh" variant="primary">
                        <feather-icon icon="SearchIcon" />
                     </b-button>
                  </b-input-group-append>
               </b-input-group>
            </b-col>

            <b-col cols="12" md="3">
               <b-input-group class="text-right">
                  <b-form-input v-model="filters.search" @keyup.enter="Refresh" :placeholder="$t('search')" />
                  <b-input-group-append>
                     <b-button @click="Refresh" variant="primary">
                        <feather-icon icon="SearchIcon" />
                     </b-button>
                  </b-input-group-append>
               </b-input-group>
            </b-col> -->
            <!-- <b-col></b-col> -->
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
               <!-- <template #cell(adress)="{ item }"> {{ item.contractorRegion }} {{ item.contractorDistrict }}</template> -->
               <template v-slot:table-busy>
                  <div class="text-center text-primary my-2" style="vertical-align: middle">
                     <b-spinner class="align-middle mr-2"></b-spinner>
                     <strong>{{ $t('Loading') }}</strong>
                  </div>
               </template>
            </b-table>
         </div>
      </div>
   </b-card>
</template>

<script>
import ReportService from '@/services/report/report.service';
import RegionService from '@/services/info/region.service';
import DistrictService from '@/services/info/district.service';
import {
   BFormSelect,
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
   BCardText
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
      BCardText
   },
   data() {
      return {
         RegionList: [],
         DistrictList: [],
         items: [],
         fields: [
            {
               key: 'order',
               label: '№',
               thClass: 'text-center',
               tdClass: 'text-center'
            },
            {
               key: 'contractorFullName',
               label: this.$t('Корхона (ташкилот) номи'),
               thClass: 'text-center',
               tdClass: 'text-center'
            },
            {
               key: 'contractorOpf',
               label: this.$t('Корхона (ташкилот)нинг ташкилий-ҳуқуқий шакли'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },

            {
               key: 'contractorRegion',
               label: this.$t('choosedRegion'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'contractorDistrict',
               label: this.$t('Region'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'adress',
               label: this.$t('Adress'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'contractorInn',
               label: this.$t('inn'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'docNumber',
               label: this.$t('Хартияга аъзолик сертификатининг рақами'),
               thClass: 'text-center',
               tdClass: 'text-center'
            },
            {
               key: 'docOn',
               label: this.$t('Сертификат Реестрга киритилган сана'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },

            {
               key: 'expireOn',
               label: this.$t('Сертификат берилган сана'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            },
            {
               key: 'contractorPhoneNumber',
               label: this.$t('phoneNumber'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            }
         ],
         printLoding: false,
         filters: {
            inn: '',
            regionId: null,
            districtId: null,
            fromDate: '',
            toDate: ''
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
            this.makeToast(error.reaponse.data, 'danger');
         });
   },
   methods: {
      Print() {
         this.printLoding = true;
         ReportService.SaveAsExcelCharterMembersRegisterReport(this.filters).then((res) => {
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
         ReportService.GetCharterMembersRegisterReport(this.filters)
            .then((res) => {
               this.items = res.data;
               // this.filters.total = res.data.item2;
               this.isBusy = false;
            })

            .catch((error) => {
               this.showApiError(error);
            });
      },
      ChangeRegion(id) {
         DistrictService.GetAsSelectList(id)
            .then((res) => {
               this.DistrictList = res.data;
            })
            .catch((error) => this.makeToast(error.response.data, 'danger'));
      }
   }
};
</script>

<style lang="scss" scoped></style>
