<template>
   <b-card no-body>
      <div class="mr-2 ml-2 mt-2">
         <b-tabs pills>
            <b-tab @click="(tabData = 1), Refresh()" :title="$t('variant-1')">
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
                  <b-col sm="12" md="2">
                     <form-picker
                        :label="$t('startDate')"
                        v-model="filter.startDate"
                        :placeholder="$t('startDate')"
                        @update:modelValue="Refresh"
                     ></form-picker>
                  </b-col>
                  <b-col sm="12" md="2">
                     <form-picker
                        :label="$t('endDate')"
                        v-model="filter.endDate"
                        :placeholder="$t('endDate')"
                        @update:modelValue="Refresh"
                     ></form-picker>
                  </b-col>

                  <!-- </b-row>
            </b-col>-->
                  <b-col cols="12" md="4">
                     <label for>{{ $t('inn') }}</label>
                     <b-input-group class="text-right">
                        <b-form-input v-model="filter.contractorInn" :placeholder="$t('search')" />
                        <b-input-group-append>
                           <b-button @click="Refresh" variant="primary">
                              <feather-icon icon="SearchIcon" />
                           </b-button>
                        </b-input-group-append>
                     </b-input-group>
                  </b-col>
                  <b-col sm="12" md="2" class="ml-auto text-right" :class="{ 'mt-2': isMobileDevice() }">
                     <b-button @click="Print" variant="primary">
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
                     <b-table-simple hover small caption-top responsive border>
                        <b-thead>
                           <b-tr>
                              <b-th rowspan="3" style="font-weight: 900; font-size: 14px; color: black">{{
                                 $t('order')
                              }}</b-th>
                              <b-th
                                 rowspan="3"
                                 class="table-b-table-default"
                                 :class="{ 'b-table-sticky-column': !isMobileDevice() }"
                              >
                                 <span
                                    v-show="filter.byRegion"
                                    style="font-weight: 900; font-size: 14px; color: black"
                                    >{{ $t('region') }}</span
                                 >
                                 <span
                                    v-show="filter.byDistrict"
                                    style="font-weight: 900; font-size: 14px; color: black"
                                    >{{ $t('Region') }}</span
                                 >
                                 <span
                                    v-show="filter.byContractor"
                                    style="font-weight: 900; font-size: 14px; color: black"
                                    >{{ $t('contractorT') }}</span
                                 >
                              </b-th>
                              <b-th colspan="2" rowspan="2" style="font-weight: 900; font-size: 14px; color: black">{{
                                 $t('totalAplication')
                              }}</b-th>

                              <th colspan="2" rowspan="2" style="font-weight: 900; font-size: 14px; color: black">
                                 {{ $t('givenCertificate') }}
                              </th>
                           </b-tr>
                           <b-tr>
                              <b-th colspan="3" style="font-weight: 900; font-size: 14px; color: black">{{
                                 $t('PropertyAndLandTax')
                              }}</b-th>
                              <b-th colspan="2" style="font-weight: 900; font-size: 14px; color: black">{{
                                 $t('RefundOfOverpaidTax')
                              }}</b-th>
                              <b-th colspan="2" style="font-weight: 900; font-size: 14px; color: black">{{
                                 $t('RefundOfTheNegativeDifferenceAmount')
                              }}</b-th>
                              <b-th rowspan="2"
                                 ><span style="font-weight: 900; font-size: 14px; color: black">{{
                                    $t('SocialTax')
                                 }}</span>
                                 <br />
                                 {{ $t('PaidAt50%Calculated') }}</b-th
                              >
                              <b-th rowspan="2"
                                 ><span style="font-weight: 900; font-size: 14px; color: black">{{
                                    $t('IncomeTax')
                                 }}</span>
                                 <br />
                                 {{ $t('IncomeTaxPaidOnMonthly') }}</b-th
                              >
                           </b-tr>
                           <b-tr>
                              <b-th>{{ $t('count') }}</b-th>
                              <b-th>{{ $t('workerCount') }}</b-th>

                              <b-th>{{ $t('count') }}</b-th>
                              <b-th>{{ $t('workerCount') }}</b-th>
                              <b-th
                                 >{{ $t('51-100people') }} <br />
                                 <span style="color: red; white-space: nowrap"> {{ $t('50Percent') }}</span>
                              </b-th>
                              <b-th
                                 >{{ $t('101-200people') }} <br />
                                 <span style="color: red; white-space: nowrap">{{ $t('70Percent') }}</span>
                              </b-th>
                              <b-th
                                 >{{ $t('MoreThan200people') }} <br /><span style="color: red; white-space: nowrap">{{
                                    $t('75Percent')
                                 }}</span>
                              </b-th>
                              <b-th
                                 >{{ $t('51-200people') }} <br />
                                 <span style="color: red; white-space: nowrap">{{ $t('100Percent') }}</span>
                              </b-th>
                              <b-th
                                 >{{ $t('MoreThan200people') }} <br /><span style="color: red; white-space: nowrap">{{
                                    $t('In1Days')
                                 }}</span>
                              </b-th>
                              <b-th
                                 >{{ $t('51-200people') }} <br /><span style="color: red; white-space: nowrap">{{
                                    $t('In3Days')
                                 }}</span>
                              </b-th>
                              <b-th
                                 >{{ $t('MoreThan200people') }} <br /><span style="color: red; white-space: nowrap">{{
                                    $t('In11Days')
                                 }}</span>
                              </b-th>
                           </b-tr>
                        </b-thead>
                        <b-tbody v-if="items.rows.length > 0">
                           <b-tr v-for="(item, idx) in items.rows" :key="idx + 'abc'">
                              <b-td>{{ idx + 1 }}</b-td>
                              <b-td
                                 class="table-b-table-default"
                                 :class="{ 'b-table-sticky-column': !isMobileDevice() }"
                              >
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

                                 <span v-show="filter.byContractor">
                                    {{ item.contractorInn }} -
                                    {{ item.contractor }}
                                 </span>

                                 <!-- {{ item.region }} -->
                              </b-td>
                              <b-td class="text-right">{{ item.totalApplication.item1 }}</b-td>
                              <b-td class="text-right">{{ item.totalApplication.item2 }}</b-td>

                              <b-td class="text-right">{{ item.totalCertificate.item1 }}</b-td>
                              <b-td class="text-right">{{ item.totalCertificate.item2 }}</b-td>
                              <b-td class="text-right">{{ item.countCertificates['1'].item1 }}</b-td>
                              <b-td class="text-right">{{ item.countCertificates['2'].item1 }}</b-td>
                              <b-td class="text-right">{{ item.countCertificates['3'].item1 }}</b-td>
                              <b-td class="text-right">{{
                                 item.countCertificates['1'].item1 + item.countCertificates['2'].item1
                              }}</b-td>
                              <b-td class="text-right">{{ item.countCertificates['3'].item1 }}</b-td>
                              <b-td class="text-right">{{
                                 item.countCertificates['1'].item1 + item.countCertificates['2'].item1
                              }}</b-td>
                              <b-td class="text-right">{{ item.countCertificates['3'].item1 }}</b-td>
                              <b-td class="text-right">{{ item.totalCertificate.item1 }}</b-td>
                              <b-td class="text-right">{{ item.totalCertificate.item1 }}</b-td>
                           </b-tr>
                        </b-tbody>
                        <b-tfoot v-if="items.rows.length > 0">
                           <b-tr variant="secondary">
                              <b-td class="text-right" colspan="2">{{ $t('Total') }}</b-td>
                              <b-td class="text-right">{{ items.applicationTotals.item1 }}</b-td>
                              <b-td class="text-right">{{ items.applicationTotals.item2 }}</b-td>

                              <b-td class="text-right">{{ items.certificateTotals.item1 }}</b-td>
                              <b-td class="text-right">{{ items.certificateTotals.item2 }}</b-td>
                              <b-td class="text-right">{{ items.certificateColumnTotals['1'].item1 }}</b-td>
                              <b-td class="text-right">{{ items.certificateColumnTotals['2'].item1 }}</b-td>
                              <b-td class="text-right">{{ items.certificateColumnTotals['3'].item1 }}</b-td>
                              <b-td class="text-right">{{
                                 items.certificateColumnTotals['1'].item1 + items.certificateColumnTotals['2'].item1
                              }}</b-td>
                              <b-td class="text-right">{{ items.certificateColumnTotals['3'].item1 }}</b-td>
                              <b-td class="text-right">{{
                                 items.certificateColumnTotals['1'].item1 + items.certificateColumnTotals['2'].item1
                              }}</b-td>
                              <b-td class="text-right">{{ items.certificateColumnTotals['3'].item1 }}</b-td>
                              <b-td class="text-right">{{ items.certificateTotals.item1 }}</b-td>
                              <b-td class="text-right">{{ items.certificateTotals.item1 }}</b-td>
                           </b-tr>
                        </b-tfoot>
                     </b-table-simple>
                     <template #overlay>
                        <div class="text-center text-primary my-2">
                           <b-spinner scale="2" class="align-middle mr-2"></b-spinner>
                           <strong>{{ $t('Loading') }}...</strong>
                        </div>
                     </template>
                  </b-overlay>
               </div>
            </b-tab>
            <b-tab @click="(tabData = 2), Refresh()" :title="$t('variant-2')">
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
                  <b-col sm="12" md="2">
                     <form-picker
                        :label="$t('startDate')"
                        v-model="filter.startDate"
                        :placeholder="$t('startDate')"
                        @update:modelValue="Refresh"
                     ></form-picker>
                  </b-col>
                  <b-col sm="12" md="2">
                     <form-picker
                        :label="$t('endDate')"
                        v-model="filter.endDate"
                        :placeholder="$t('endDate')"
                        @update:modelValue="Refresh"
                     ></form-picker>
                  </b-col>

                  <!-- </b-row>
            </b-col>-->
                  <b-col cols="12" md="4">
                     <label for>{{ $t('search') }}</label>
                     <b-input-group class="text-right">
                        <b-form-input v-model="filter.contractorInn" :placeholder="$t('search')" />
                        <b-input-group-append>
                           <b-button @click="Refresh" variant="primary">
                              <feather-icon icon="SearchIcon" />
                           </b-button>
                        </b-input-group-append>
                     </b-input-group>
                  </b-col>
                  <!-- <b-col sm="12" md="2" class="ml-auto text-right">
                     <b-button @click="Print" variant="primary">
                        <feather-icon icon="PrinterIcon"></feather-icon>
                        {{ $t('Print') }}
                     </b-button>
                  </b-col> -->
               </b-row>
               <b-row align-h="between">
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
                        <!-- <b-breadcrumb-item v-show="filter.byContractor" active>Baz</b-breadcrumb-item> -->
                     </b-breadcrumb>
                  </b-col>
               </b-row>
               <div class="mt-2 mb-2 report-table">
                  <b-overlay :show="isBusy">
                     <b-table-simple hover small caption-top responsive border>
                        <b-thead>
                           <b-tr>
                              <b-th rowspan="3" style="font-weight: 900; font-size: 14px; color: black">{{
                                 $t('order')
                              }}</b-th>
                              <b-th
                                 rowspan="3"
                                 class="table-b-table-default"
                                 :class="{ 'b-table-sticky-column': !isMobileDevice() }"
                              >
                                 <span style="font-weight: 900; font-size: 14px; color: black">{{ $t('region') }}</span>
                              </b-th>
                              <b-th
                                 rowspan="3"
                                 class="table-b-table-default"
                                 :class="{ 'b-table-sticky-column': !isMobileDevice() }"
                              >
                                 <span style="font-weight: 900; font-size: 14px; color: black">{{
                                    $t('district')
                                 }}</span>
                              </b-th>
                              <b-th
                                 rowspan="3"
                                 class="table-b-table-default"
                                 :class="{ 'b-table-sticky-column': !isMobileDevice() }"
                              >
                                 <span style="font-weight: 900; font-size: 14px; color: black">{{
                                    $t('contractorT')
                                 }}</span>
                              </b-th>

                              <b-th colspan="2" rowspan="2" style="font-weight: 900; font-size: 14px; color: black">{{
                                 $t('totalAplication')
                              }}</b-th>

                              <th colspan="2" rowspan="2" style="font-weight: 900; font-size: 14px; color: black">
                                 {{ $t('givenCertificate') }}
                              </th>
                           </b-tr>
                           <b-tr>
                              <b-th colspan="3" style="font-weight: 900; font-size: 14px; color: black">{{
                                 $t('PropertyAndLandTax')
                              }}</b-th>
                              <b-th colspan="2" style="font-weight: 900; font-size: 14px; color: black">{{
                                 $t('RefundOfOverpaidTax')
                              }}</b-th>
                              <b-th colspan="2" style="font-weight: 900; font-size: 14px; color: black">{{
                                 $t('RefundOfTheNegativeDifferenceAmount')
                              }}</b-th>
                              <b-th rowspan="2"
                                 ><span style="font-weight: 900; font-size: 14px; color: black">{{
                                    $t('SocialTax')
                                 }}</span>
                                 <br />
                                 {{ $t('PaidAt50%Calculated') }}</b-th
                              >
                              <b-th rowspan="2"
                                 ><span style="font-weight: 900; font-size: 14px; color: black">{{
                                    $t('IncomeTax')
                                 }}</span>
                                 <br />
                                 {{ $t('IncomeTaxPaidOnMonthly') }}</b-th
                              >
                           </b-tr>
                           <b-tr>
                              <b-th>{{ $t('count') }}</b-th>
                              <b-th>{{ $t('workerCount') }}</b-th>

                              <b-th>{{ $t('count') }}</b-th>
                              <b-th>{{ $t('workerCount') }}</b-th>
                              <b-th
                                 >{{ $t('51-100people') }} <br />
                                 <span style="color: red; white-space: nowrap"> {{ $t('50Percent') }}</span>
                              </b-th>
                              <b-th
                                 >{{ $t('101-200people') }} <br />
                                 <span style="color: red; white-space: nowrap">{{ $t('70Percent') }}</span>
                              </b-th>
                              <b-th
                                 >{{ $t('MoreThan200people') }} <br /><span style="color: red; white-space: nowrap">{{
                                    $t('75Percent')
                                 }}</span>
                              </b-th>
                              <b-th
                                 >{{ $t('51-200people') }} <br />
                                 <span style="color: red; white-space: nowrap">{{ $t('100Percent') }}</span>
                              </b-th>
                              <b-th
                                 >{{ $t('MoreThan200people') }} <br /><span style="color: red; white-space: nowrap">{{
                                    $t('In1Days')
                                 }}</span>
                              </b-th>
                              <b-th
                                 >{{ $t('51-200people') }} <br /><span style="color: red; white-space: nowrap">{{
                                    $t('In3Days')
                                 }}</span>
                              </b-th>
                              <b-th
                                 >{{ $t('MoreThan200people') }} <br /><span style="color: red; white-space: nowrap">{{
                                    $t('In11Days')
                                 }}</span>
                              </b-th>
                           </b-tr>
                        </b-thead>
                        <b-tbody v-if="items.rows.length > 0">
                           <b-tr v-for="(item, idx) in items.rows" :key="idx + 'abc'">
                              <template v-if="item.contractorId">
                                 <b-td>{{ idx + 1 }}</b-td>
                                 <b-td class="text-right">{{ item.region }}</b-td>
                                 <b-td class="text-right">{{ item.district }}</b-td>
                                 <b-td
                                    class="table-b-table-default"
                                    :class="{ 'b-table-sticky-column': !isMobileDevice() }"
                                 >
                                    <span>
                                       {{ item.contractorInn }} -
                                       {{ item.contractor }}
                                    </span>
                                    <!-- {{ item.region }} -->
                                 </b-td>
                                 <b-td class="text-right">{{ item.totalApplication.item1 }}</b-td>
                                 <b-td class="text-right">{{ item.totalApplication.item2 }}</b-td>

                                 <b-td class="text-right">{{ item.totalCertificate.item1 }}</b-td>
                                 <b-td class="text-right">{{ item.totalCertificate.item2 }}</b-td>
                                 <b-td class="text-right">{{ item.countCertificates['1'].item1 }}</b-td>
                                 <b-td class="text-right">{{ item.countCertificates['2'].item1 }}</b-td>
                                 <b-td class="text-right">{{ item.countCertificates['3'].item1 }}</b-td>
                                 <b-td class="text-right">{{
                                    item.countCertificates['1'].item1 + item.countCertificates['2'].item1
                                 }}</b-td>
                                 <b-td class="text-right">{{ item.countCertificates['3'].item1 }}</b-td>
                                 <b-td class="text-right">{{
                                    item.countCertificates['1'].item1 + item.countCertificates['2'].item1
                                 }}</b-td>
                                 <b-td class="text-right">{{ item.countCertificates['3'].item1 }}</b-td>
                                 <b-td class="text-right">{{ item.totalCertificate.item1 }}</b-td>
                                 <b-td class="text-right">{{ item.totalCertificate.item1 }}</b-td>
                              </template>
                           </b-tr>
                        </b-tbody>
                        <!-- <b-tfoot v-if="items.rows.length > 0">
                           <b-tr variant="secondary">
                              <b-td class="text-right" colspan="4">{{ $t('Total') }}</b-td>
                              <b-td class="text-right">{{ items.applicationTotals.item1 }}</b-td>
                              <b-td class="text-right">{{ items.applicationTotals.item2 }}</b-td>
                              <b-td class="text-right">{{ items.certificateTotals.item1 }}</b-td>
                              <b-td class="text-right">{{ items.certificateTotals.item2 }}</b-td>
                              <b-td class="text-right">{{ items.certificateColumnTotals['1'].item1 }}</b-td>
                              <b-td class="text-right">{{ items.certificateColumnTotals['2'].item1 }}</b-td>
                              <b-td class="text-right">{{ items.certificateColumnTotals['3'].item1 }}</b-td>
                              <b-td class="text-right">{{
                                 items.certificateColumnTotals['1'].item1 + items.certificateColumnTotals['2'].item1
                              }}</b-td>
                              <b-td class="text-right">{{ items.certificateColumnTotals['3'].item1 }}</b-td>
                              <b-td class="text-right">{{
                                 items.certificateColumnTotals['1'].item1 + items.certificateColumnTotals['2'].item1
                              }}</b-td>
                              <b-td class="text-right">{{ items.certificateColumnTotals['3'].item1 }}</b-td>
                              <b-td class="text-right">{{ items.certificateTotals.item1 }}</b-td>
                              <b-td class="text-right">{{ items.certificateTotals.item1 }}</b-td>
                           </b-tr>
                        </b-tfoot> -->
                     </b-table-simple>
                     <template #overlay>
                        <div class="text-center text-primary my-2">
                           <b-spinner scale="2" class="align-middle mr-2"></b-spinner>
                           <strong>{{ $t('Loading') }}...</strong>
                        </div>
                     </template>
                  </b-overlay>
               </div>
               <div class="mx-2 mb-2">
                  <b-row>
                     <b-col
                        cols="12"
                        sm="6"
                        class="d-flex align-items-center justify-content-center justify-content-sm-start"
                     >
                        <span class="text-muted">
                           {{ $t('Showing') }} {{ firstNumber }} {{ $t('to') }} {{ lastNumber }} {{ $t('of') }}
                           {{ filter2.total }}
                           {{ $t('entries') }}
                        </span>
                        <v-select
                           v-model="filter2.pageSize"
                           :dir="$store.state.appConfig.isRTL ? 'rtl' : 'ltr'"
                           :options="filter2.pageOptions"
                           :clearable="false"
                           @input="Refresh"
                           class="per-page-selector d-inline-block ml-50 mr-1"
                        />
                     </b-col>
                     <!-- Pagination -->
                     <b-col
                        cols="12"
                        sm="6"
                        class="d-flex align-items-center justify-content-center justify-content-sm-end"
                     >
                        <b-pagination
                           v-model="filter2.page"
                           :total-rows="filter2.total"
                           :per-page="filter2.pageSize"
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
            </b-tab>
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
         items: {
            rows: [],
            contractColumnTotals: {},
            certificateColumnTotals: {},
            applicationColumnTotals: {}
         },
         RegionList: [],
         DistrictList: [],
         PrtnContractTypeList: [],
         filter: {
            prtnContractTypeId: null,
            regionId: null,
            region: '',
            byRegion: true,
            districtId: null,
            district: '',
            byDistrict: false,
            contractorId: null,
            byContractor: false,
            startDate: '',
            endDate: '',
            mfyId: null,
            byMfy: false,
            contractorInn: null
         },
         filter2: {
            byRegion: true,
            byContractor: true,
            page: 1,
            pageSize: 20,
            pageOptions: [10, 20, 50, 100],
            total: 0,
            byDistrict: true
         },

         isBusy: false
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
      },
      BindValue(value) {
         this.filter.dateofbirth = value;
      },
      Print() {
         ReportService.SaveasExcelSoliqImtiyoz(this.filter).then((res) => {
            this.forceFileDownload(res, this.$t('soliq'));
         });
      },
      Refresh() {
         this.isBusy = true;
         if (this.tabData == 1) {
            ReportService.GetPrtnApplicationByContractType(this.filter)
               .then((res) => {
                  this.items = res.data;
               })
               .catch((error) => {
                  this.showApiError(error);
               })
               .finally(() => {
                  this.isBusy = false;
               });
         }
         if (this.tabData == 2) {
            ReportService.GetPrtnApplicationByContractTypePaged({ ...this.filter, ...this.filter2 })
               .then((res) => {
                  this.items = res.data;
               })
               .catch((error) => {
                  this.showApiError(error);
               })
               .finally(() => {
                  this.isBusy = false;
               });
         }
      }
   }
};
</script>

<style lang="scss" scoped>
@import '../styles.scss';
</style>
