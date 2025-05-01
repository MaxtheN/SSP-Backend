<template>
   <div>
      <b-card no-body>
         <div class="m-2">
            <b-row>
               <b-col cols="12" md="4">
                  <b-input-group class="text-right">
                     <b-form-input v-model="filter.inn" :placeholder="$t('search')" />
                     <b-input-group-append>
                        <b-button @click="Refresh" variant="primary">
                           <feather-icon icon="SearchIcon" />
                        </b-button>
                     </b-input-group-append>
                  </b-input-group>
               </b-col>

               <b-col cols="12" md="8">
                  <h1 class="text-success" v-if="Data.soliqContractorByTin.company.tin">
                     {{ Data.soliqContractorByTin.company.tin }} - {{ Data.soliqContractorByTin.company.name }}
                  </h1>
               </b-col>
            </b-row>
         </div>
      </b-card>

      <b-overlay :show="isBusy">
         <b-card>
            <b-row>
               <b-col sm="12" md="12" class="text-left">
                  <h2 style="color: #003188">{{ $t('integrationCertificateDto') }}</h2>
               </b-col>
               <b-col sm="12" md="12" class="text-left">
                  <b-table-simple
                     hover
                     small
                     caption-top
                     responsive
                     bordered
                     v-if="Data.integrationCertificateDto.certificateNumber"
                  >
                     <b-tbody>
                        <b-tr>
                           <b-th style="width: 25%">{{ $t('certificateNumber') }}</b-th>
                           <b-td style="width: 25%">{{ Data.integrationCertificateDto.certificateNumber }}</b-td>
                           <b-th style="width: 25%">{{ $t('docOn') }}</b-th>
                           <b-td style="width: 25%">{{ Data.integrationCertificateDto.docOn }}</b-td>
                        </b-tr>

                        <b-tr>
                           <b-th style="width: 25%">{{ $t('expireOn') }}</b-th>
                           <b-td style="width: 25%">{{ Data.integrationCertificateDto.expireOn }}</b-td>
                           <b-th style="width: 25%">{{ $t('newVacanciesCount') }}</b-th>
                           <b-td style="width: 25%">{{ Data.integrationCertificateDto.newVacanciesCount }}</b-td>
                        </b-tr>

                        <b-tr variant="primary">
                           <b-td colspan="4">
                              <h4 class="text-center mb-0">{{ $t('contractInfo') }}</h4>
                           </b-td>
                        </b-tr>

                        <b-tr>
                           <b-th style="width: 25%">{{ $t('contractNumber') }}</b-th>
                           <b-td style="width: 25%">{{
                              Data.integrationCertificateDto.contractInfo.contractNumber
                           }}</b-td>

                           <b-th style="width: 25%">{{ $t('contractDocOn') }}</b-th>
                           <b-td style="width: 25%">{{
                              Data.integrationCertificateDto.contractInfo.contractDocOn
                           }}</b-td>
                        </b-tr>

                        <b-tr>
                           <b-th style="width: 25%">{{ $t('contractType') }}</b-th>
                           <b-td style="width: 25%">{{
                              Data.integrationCertificateDto.contractInfo.contractType
                           }}</b-td>
                           <b-th style="width: 25%"></b-th>
                           <b-th style="width: 25%"></b-th>
                        </b-tr>

                        <!-- <b-tr>
                        <b-th style="width: 25%">{{ $t('contractGraphs') }}</b-th>
                        <b-td style="width: 25%">{{ Data.integrationCertificateDto.contractGraphs }}</b-td>
                     </b-tr>-->

                        <b-tr>
                           <b-td colspan="4">
                              <b-row>
                                 <b-col sm="12" md="12" class="text-left">
                                    <h5>{{ $t('contractGraphs') }}</h5>

                                    <b-table-simple small caption-top responsive bordered>
                                       <tbody>
                                          <b-tr>
                                             <b-th rowspan="2">{{ $t('newCountEmp') }}*</b-th>
                                             <b-th colspan="13">{{ $t('newVacanciesCount') }}</b-th>
                                          </b-tr>
                                          <b-tr>
                                             <b-th>{{ $t('month1') }}</b-th>
                                             <b-th>{{ $t('month2') }}</b-th>
                                             <b-th>{{ $t('month3') }}</b-th>
                                             <b-th>{{ $t('month4') }}</b-th>
                                             <b-th>{{ $t('month5') }}</b-th>
                                             <b-th>{{ $t('month6') }}</b-th>
                                             <b-th>{{ $t('month7') }}</b-th>
                                             <b-th>{{ $t('month8') }}</b-th>
                                             <b-th>{{ $t('month9') }}</b-th>
                                             <b-th>{{ $t('month10') }}</b-th>
                                             <b-th>{{ $t('month11') }}</b-th>
                                             <b-th>{{ $t('month12') }}</b-th>
                                             <b-th>{{ $t('ofreport') }}</b-th>
                                          </b-tr>
                                          <b-tr v-for="(item, key) in contractGraphs" :key="key">
                                             <b-td>{{ key }}</b-td>
                                             <b-td v-for="number in 12" :key="number + 'num'">{{
                                                contractGraphs[key][number]
                                             }}</b-td>
                                             <b-td>{{ contractGraphs[key].total }}</b-td>
                                          </b-tr>
                                          <b-tr>
                                             <b-th>{{ $t('ofreport') }}</b-th>
                                             <b-td></b-td>
                                             <b-td></b-td>
                                             <b-td></b-td>
                                             <b-td></b-td>
                                             <b-td></b-td>
                                             <b-td></b-td>
                                             <b-td></b-td>
                                             <b-td></b-td>
                                             <b-td></b-td>
                                             <b-td></b-td>
                                             <b-td></b-td>
                                             <b-td></b-td>
                                             <b-td>{{ Data.integrationCertificateDto.newVacanciesCount }}</b-td>
                                          </b-tr>
                                       </tbody>
                                    </b-table-simple>
                                 </b-col>
                              </b-row>
                           </b-td>
                        </b-tr>
                     </b-tbody>
                  </b-table-simple>

                  <b-alert
                     show
                     variant="danger"
                     v-else
                     class="d-flex justify-content-center align-items-center"
                     style="height: 150px"
                  >
                     <feather-icon icon="ArchiveIcon" size="25" class="mr-1"></feather-icon>
                     <span style="font-size: 22px">{{ $t('hasNotCertificate') }}</span>
                  </b-alert>
               </b-col>
            </b-row>
         </b-card>
         <BojxonaCard is-component :data="Data.getGTDByInn" />
         <b-card>
            <b-row>
               <b-col sm="12" md="12" class="text-left">
                  <h2 style="color: #003188">{{ $t('soliqContractorByTin') }}</h2>

                  <b-table-simple
                     hover
                     small
                     caption-top
                     responsive
                     bordered
                     v-if="Data.soliqContractorByTin.company.tin"
                  >
                     <b-tbody>
                        <b-tr variant="primary">
                           <b-td colspan="4">
                              <h4 class="text-center mb-0">{{ $t('Infoglob') }}</h4>
                           </b-td>
                        </b-tr>
                        <b-tr>
                           <b-th style="width: 25%">{{ $t('tin') }}</b-th>
                           <b-td style="width: 25%">{{ Data.soliqContractorByTin.company.tin }}</b-td>
                           <b-th style="width: 25%">{{ $t('name') }}</b-th>
                           <b-td style="width: 25%">{{ Data.soliqContractorByTin.company.name }}</b-td>
                        </b-tr>
                        <b-tr>
                           <b-th style="width: 25%">{{ $t('businessTypeDetail') }}</b-th>
                           <b-td style="width: 25%">{{
                              GetLanguageItem(Data.soliqContractorByTin.company.businessTypeDetail)
                           }}</b-td>
                           <b-th style="width: 25%">{{ $t('opfDetail') }}</b-th>
                           <b-td style="width: 25%"
                              >{{ Data.soliqContractorByTin.company.opf }} -
                              {{ GetLanguageItem(Data.soliqContractorByTin.company.opfDetail) }}</b-td
                           >
                        </b-tr>

                        <b-tr>
                           <b-th style="width: 25%">{{ $t('okedDetail') }}</b-th>
                           <b-td style="width: 25%">
                              {{ Data.soliqContractorByTin.company.oked }} -
                              {{ GetLanguageItem(Data.soliqContractorByTin.company.okedDetail) }} ({{
                                 Data.soliqContractorByTin.company.okedDetail.pkm275
                              }}
                              {{ Data.soliqContractorByTin.company.okedDetail.section }})
                           </b-td>

                           <b-th style="width: 25%">{{ $t('vatNumber') }}</b-th>
                           <b-td style="width: 25%">{{ Data.soliqContractorByTin.company.vatNumber }}</b-td>
                        </b-tr>

                        <b-tr>
                           <b-th style="width: 25%">{{ $t('registrationNumber') }}</b-th>
                           <b-td style="width: 25%">{{ Data.soliqContractorByTin.company.registrationNumber }}</b-td>
                           <b-th style="width: 25%">{{ $t('registrationDate') }}</b-th>
                           <b-td style="width: 25%">{{ Data.soliqContractorByTin.company.registrationDate }}</b-td>
                        </b-tr>

                        <b-tr>
                           <b-th style="width: 25%">{{ $t('reregistrationDate') }}</b-th>
                           <b-td style="width: 25%">{{ Data.soliqContractorByTin.company.reregistrationDate }}</b-td>

                           <b-th style="width: 25%">{{ $t('soato') }}</b-th>
                           <b-td style="width: 25%">{{ Data.soliqContractorByTin.company.soato }}</b-td>
                        </b-tr>

                        <b-tr>
                           <b-th style="width: 25%">{{ $t('region') }}</b-th>
                           <b-td style="width: 25%">{{ Data.soliqContractorByTin.company.region }}</b-td>

                           <b-th style="width: 25%">{{ $t('district') }}</b-th>
                           <b-td style="width: 25%">{{ Data.soliqContractorByTin.company.district }}</b-td>
                        </b-tr>

                        <b-tr>
                           <b-th style="width: 25%">{{ $t('mfy') }}</b-th>
                           <b-td style="width: 25%">{{ Data.soliqContractorByTin.company.villageName }}</b-td>

                           <b-th style="width: 25%">{{ $t('streetName') }}</b-th>
                           <b-td style="width: 25%">{{ Data.soliqContractorByTin.company.streetName }}</b-td>
                        </b-tr>

                        <b-tr>
                           <b-th style="width: 25%">{{ $t('house') }}</b-th>
                           <b-td style="width: 25%">{{ Data.soliqContractorByTin.company.house }}</b-td>
                           <b-th style="width: 25%">{{ $t('flat') }}</b-th>
                           <b-td style="width: 25%">{{ Data.soliqContractorByTin.company.flat }}</b-td>
                        </b-tr>

                        <b-tr>
                           <b-th style="width: 25%">{{ $t('employee_limit_lf') }}</b-th>

                           <b-td style="width: 25%"
                              >{{ Data.soliqContractorByTin.company.okedDetail.employee_limit_mf }} -
                              {{ Data.soliqContractorByTin.company.okedDetail.employee_limit_lf }}</b-td
                           >

                           <b-th style="width: 25%">{{ $t('businessFund') }}</b-th>
                           <b-td style="width: 25%">{{
                              currency(Data.soliqContractorByTin.company.businessFund)
                           }}</b-td>
                        </b-tr>

                        <b-tr>
                           <b-th style="width: 25%">{{ $t('liquidationDate') }}</b-th>
                           <b-td style="width: 25%">{{ Data.soliqContractorByTin.company.liquidationDate }}</b-td>
                           <b-th style="width: 25%">{{ $t('liquidationReason') }}</b-th>
                           <b-td style="width: 25%">{{ Data.soliqContractorByTin.company.liquidationReason }}</b-td>
                        </b-tr>

                        <b-tr>
                           <b-th style="width: 25%">{{ $t('soogu') }}</b-th>
                           <b-td style="width: 25%"
                              >{{ Data.soliqContractorByTin.company.soogu }} -
                              {{ GetLanguageItem(Data.soliqContractorByTin.company.sooguDetail) }}</b-td
                           >

                           <b-th style="width: 25%">{{ $t('statusDetail') }}</b-th>
                           <b-td style="width: 25%">{{
                              GetLanguageItem(Data.soliqContractorByTin.company.statusDetail)
                           }}</b-td>
                        </b-tr>

                        <b-tr>
                           <b-th style="width: 25%">{{ $t('avgNumberEmployees') }}</b-th>
                           <b-td style="width: 25%">{{
                              Data.soliqContractorByTin.companyExtraInfo.avgNumberEmployees
                           }}</b-td>
                           <b-th style="width: 25%">{{ $t('monthlyNumberEmployees') }}</b-th>
                           <b-td style="width: 25%">{{
                              Data.soliqContractorByTin.companyExtraInfo.monthlyNumberEmployees
                           }}</b-td>
                        </b-tr>

                        <b-tr>
                           <b-th style="width: 25%">{{ $t('netIncomeLastYear') }}</b-th>
                           <b-td style="width: 25%">{{
                              currency(Data.soliqContractorFinanceBenefitByTinLastYear.netIncome)
                           }}</b-td>
                           <b-th style="width: 25%">{{ $t('netIncomeThisYear') }}</b-th>
                           <b-th style="width: 25%">{{
                              Data.soliqContractorFinanceBenefitByTinThisYear.netIncome != null
                                 ? currency(Data.soliqContractorFinanceBenefitByTinThisYear.netIncome)
                                 : $t('NotFound')
                           }}</b-th>
                        </b-tr>

                        <b-tr>
                           <b-th style="width: 25%">{{ $t('tax_debt') }}</b-th>
                           <b-th style="width: 25%">{{ Data.soliqContractorDebtByTin.tax_debt }}</b-th>
                        </b-tr>

                        <b-tr>
                           <b-td colspan="4">
                              <h5>{{ $t('activityTypes') }}</h5>
                              <b-table
                                 ref="refInvoiceListTable"
                                 :items="Data.soliqContractorByTin.company.activityTypes"
                                 responsive
                                 :fields="fieldsActivityTypes"
                                 primary-key="id"
                                 no-border-collapse
                                 show-empty
                                 :empty-text="$t('NotFound')"
                                 class="position-relative"
                              >
                                 <template #cell(name)="{ item }">{{ GetLanguageItem(item) }}</template>
                              </b-table>
                           </b-td>
                        </b-tr>

                        <b-tr variant="primary">
                           <b-td colspan="4">
                              <h4 class="text-center mb-0">{{ $t('soliqContractorEmployeeCountByTin') }}</h4>
                           </b-td>
                        </b-tr>

                        <b-tr>
                           <b-th style="width: 25%">{{ $t('docyear') }}</b-th>
                           <b-td style="width: 25%">{{ Data.soliqContractorEmployeeCountByTin.year }}</b-td>
                           <b-th style="width: 25%">{{ $t('forMonth') }}</b-th>
                           <b-td style="width: 25%">{{ Data.soliqContractorEmployeeCountByTin.month }}</b-td>
                        </b-tr>

                        <b-tr>
                           <b-th style="width: 25%">{{ $t('monthlyNumberEmployees') }}</b-th>
                           <b-td style="width: 25%">{{
                              Data.soliqContractorEmployeeCountByTin.monthlyNumberEmployees
                           }}</b-td>
                           <b-th style="width: 25%">{{ $t('paymentTax') }}</b-th>
                           <b-td style="width: 25%">{{ Data.soliqContractorEmployeeCountByTin.paymentTax }}</b-td>
                        </b-tr>

                        <b-tr variant="primary">
                           <b-td colspan="4">
                              <h4 class="text-center mb-0">{{ $t('companyBillingAddress') }}</h4>
                           </b-td>
                        </b-tr>

                        <!-- <b-th style="width: 25%">{{ $t('opf') }}</b-th>
                     <b-td style="width: 25%">{{ Data.soliqContractorByTin.company.opf }}</b-td>
                     <b-th style="width: 25%">{{ $t('opfDetail') }}</b-th>
                     <b-td
                        style="width: 25%"
                  >{{ GetLanguageItem(Data.soliqContractorByTin.company.opfDetail )}}</b-td>
                  
                   Lorem ipsum dolor sit amet consectetur adipisicing elit. Veritatis quis
                    placeat ipsam repudiandae atque quisquam fuga sunt! Corporis ipsam dolores 
                    accusantium, animi non quisquam, rerum eligendi, sapiente eum saepe ullam?
                  
                     -->

                        <b-tr>
                           <b-th style="width: 25%">{{ $t('region') }}</b-th>
                           <b-td style="width: 25%">{{
                              GetLanguageItem(Data.soliqContractorByTin.companyBillingAddress.region)
                           }}</b-td>
                           <b-th style="width: 25%">{{ $t('district') }}</b-th>
                           <b-td style="width: 25%">{{
                              GetLanguageItem(Data.soliqContractorByTin.companyBillingAddress.district)
                           }}</b-td>
                        </b-tr>

                        <b-tr>
                           <b-th style="width: 25%">{{ $t('mfy') }}</b-th>
                           <b-td style="width: 25%">{{ Data.soliqContractorByTin.companyBillingAddress.village }}</b-td>
                           <b-th style="width: 25%">{{ $t('streetName') }}</b-th>
                           <b-td style="width: 25%">{{
                              Data.soliqContractorByTin.companyBillingAddress.streetName
                           }}</b-td>
                        </b-tr>

                        <b-tr>
                           <b-th style="width: 25%">{{ $t('house') }}</b-th>
                           <b-td style="width: 25%">{{ Data.soliqContractorByTin.companyBillingAddress.house }}</b-td>
                           <b-th style="width: 25%">{{ $t('flat') }}</b-th>
                           <b-td style="width: 25%">{{ Data.soliqContractorByTin.companyBillingAddress.flat }}</b-td>
                        </b-tr>

                        <b-tr>
                           <b-th style="width: 25%">{{ $t('postcode') }}</b-th>
                           <b-td style="width: 25%">{{
                              Data.soliqContractorByTin.companyBillingAddress.postcode
                           }}</b-td>
                           <b-th style="width: 25%"></b-th>
                           <b-th style="width: 25%"></b-th>
                        </b-tr>

                        <b-tr variant="primary">
                           <b-td colspan="4">
                              <h4 class="text-center mb-0">{{ $t('companyContact') }}</h4>
                           </b-td>
                        </b-tr>

                        <b-tr>
                           <b-th style="width: 25%">{{ $t('phone') }}</b-th>
                           <b-td style="width: 25%">{{
                              Data.soliqContractorByTin.companyContact
                                 ? Data.soliqContractorByTin.companyContact.phone
                                 : null
                           }}</b-td>
                           <b-th style="width: 25%">{{ $t('email') }}</b-th>
                           <b-td style="width: 25%">{{
                              Data.soliqContractorByTin.companyContact
                                 ? Data.soliqContractorByTin.companyContact.email
                                 : null
                           }}</b-td>
                        </b-tr>

                        <b-tr variant="primary">
                           <b-td colspan="4">
                              <h4 class="text-center mb-0">{{ $t('director') }}</h4>
                           </b-td>
                        </b-tr>
                        <b-tr>
                           <b-th style="width: 25%">{{ $t('firstName') }}</b-th>
                           <b-td style="width: 25%">{{ Data.soliqContractorByTin.director.firstName }}</b-td>
                           <b-th style="width: 25%">{{ $t('lastName') }}</b-th>
                           <b-td style="width: 25%">{{ Data.soliqContractorByTin.director.lastName }}</b-td>
                        </b-tr>

                        <b-tr>
                           <b-th style="width: 25%">{{ $t('middleName') }}</b-th>
                           <b-td style="width: 25%">{{ Data.soliqContractorByTin.director.middleName }}</b-td>
                           <b-th style="width: 25%">{{ $t('birthDate') }}</b-th>
                           <b-td style="width: 25%">{{ Data.soliqContractorByTin.director.birthDate }}</b-td>
                        </b-tr>

                        <b-tr>
                           <b-th style="width: 25%">{{ $t('passportSeries') }}</b-th>
                           <b-td style="width: 25%">{{ Data.soliqContractorByTin.director.passportSeries }}</b-td>
                           <b-th style="width: 25%">{{ $t('passportNumber') }}</b-th>
                           <b-td style="width: 25%">{{ Data.soliqContractorByTin.director.passportNumber }}</b-td>
                        </b-tr>

                        <b-tr>
                           <b-th style="width: 25%">{{ $t('pinfl') }}</b-th>
                           <b-td style="width: 25%">{{ Data.soliqContractorByTin.director.pinfl }}</b-td>
                           <b-th style="width: 25%">{{ $t('tin') }}</b-th>
                           <b-td style="width: 25%">{{ Data.soliqContractorByTin.director.tin }}</b-td>
                        </b-tr>

                        <b-tr variant="primary">
                           <b-td colspan="4">
                              <h4 class="text-center mb-0">{{ $t('directorAddress') }}</h4>
                           </b-td>
                        </b-tr>

                        <b-tr>
                           <b-th style="width: 25%">{{ $t('region') }}</b-th>
                           <b-td style="width: 25%">{{
                              GetLanguageItem(Data.soliqContractorByTin.directorAddress.region)
                           }}</b-td>
                           <b-th style="width: 25%">{{ $t('district') }}</b-th>
                           <b-td style="width: 25%">{{
                              GetLanguageItem(Data.soliqContractorByTin.directorAddress.district)
                           }}</b-td>
                        </b-tr>

                        <b-tr>
                           <b-th style="width: 25%">{{ $t('mfy') }}</b-th>
                           <b-td style="width: 25%">{{ Data.soliqContractorByTin.directorAddress.village }}</b-td>
                           <b-th style="width: 25%">{{ $t('streetName') }}</b-th>
                           <b-td style="width: 25%">{{ Data.soliqContractorByTin.directorAddress.streetName }}</b-td>
                        </b-tr>

                        <b-tr>
                           <b-th style="width: 25%">{{ $t('house') }}</b-th>
                           <b-td style="width: 25%">{{ Data.soliqContractorByTin.directorAddress.house }}</b-td>
                           <b-th style="width: 25%">{{ $t('flat') }}</b-th>
                           <b-td style="width: 25%">{{ Data.soliqContractorByTin.directorAddress.flat }}</b-td>
                        </b-tr>

                        <b-tr>
                           <b-th style="width: 25%">{{ $t('postcode') }}</b-th>
                           <b-td style="width: 25%">{{ Data.soliqContractorByTin.directorAddress.postcode }}</b-td>
                           <b-th style="width: 25%"></b-th>
                           <b-th style="width: 25%"></b-th>
                        </b-tr>

                        <b-tr variant="primary">
                           <b-td colspan="4">
                              <h4 class="text-center mb-0">{{ $t('directorContact') }}</h4>
                           </b-td>
                        </b-tr>

                        <b-tr>
                           <b-th style="width: 25%">{{ $t('phone') }}</b-th>
                           <b-td style="width: 25%">{{ Data.soliqContractorByTin.directorContact.phone }}</b-td>
                           <b-th style="width: 25%">{{ $t('email') }}</b-th>
                           <b-td style="width: 25%">{{ Data.soliqContractorByTin.directorContact.email }}</b-td>
                        </b-tr>

                        <b-tr variant="primary">
                           <b-td colspan="4">
                              <h4 class="text-center mb-0">{{ $t('companyBanks') }}</h4>
                           </b-td>
                        </b-tr>

                        <b-tr>
                           <b-td colspan="4">
                              <b-table
                                 ref="refInvoiceListTable"
                                 :items="Data.soliqContractorByTin.companyBanks"
                                 responsive
                                 :fields="fieldsCompanyBanks"
                                 primary-key="id"
                                 no-border-collapse
                                 show-empty
                                 :empty-text="$t('NotFound')"
                                 class="position-relative"
                              ></b-table>
                           </b-td>
                        </b-tr>
                     </b-tbody>
                  </b-table-simple>

                  <b-alert
                     show
                     variant="danger"
                     v-else
                     class="d-flex justify-content-center align-items-center"
                     style="height: 150px"
                  >
                     <feather-icon icon="ArchiveIcon" size="25" class="mr-1"></feather-icon>
                     <span style="font-size: 22px">{{ $t('hasNotSoliqContractorByTin') }}</span>
                  </b-alert>
               </b-col>
               <!-- <b-col sm="12" md="12" class="text-left">
          <pre>
            {{ Data.soliqContractorByTin }}
          </pre>
            </b-col>-->
            </b-row>
         </b-card>

         <b-card>
            <b-row>
               <b-col sm="12" md="12" class="text-left">
                  <h2 style="color: #003188">{{ $t('davAktivContractor') }}</h2>
               </b-col>
               <b-col sm="12" md="12" class="text-left">
                  <b-table-simple hover small caption-top responsive bordered v-if="Data.davAktivContractor.TIN">
                     <b-tbody>
                        <b-tr>
                           <b-th style="width: 25%">{{ $t('address') }}</b-th>
                           <b-td style="width: 25%">{{ Data.davAktivContractor.ADDRESS }}</b-td>
                           <b-th style="width: 25%">{{ $t('name') }}</b-th>
                           <b-td style="width: 25%">{{ Data.davAktivContractor.NAME }}</b-td>
                        </b-tr>

                        <b-tr>
                           <b-th style="width: 25%">{{ $t('NS10') }}</b-th>
                           <b-td style="width: 25%">{{ Data.davAktivContractor.NS10 }}</b-td>
                           <b-th style="width: 25%">{{ $t('NS11') }}</b-th>
                           <b-td style="width: 25%">{{ Data.davAktivContractor.NS11 }}</b-td>
                        </b-tr>

                        <b-tr>
                           <b-th style="width: 25%">{{ $t('ORG_TYPE') }}</b-th>
                           <b-td style="width: 25%">{{ Data.davAktivContractor.ORG_TYPE }}</b-td>
                           <b-th style="width: 25%">{{ $t('SEND_DATE') }}</b-th>
                           <b-td style="width: 25%">{{ Data.davAktivContractor.SEND_DATE }}</b-td>
                        </b-tr>

                        <b-tr>
                           <b-th style="width: 25%">{{ $t('SHARE1') }}</b-th>
                           <b-td style="width: 25%">{{ Data.davAktivContractor.SHARE1 }}</b-td>
                           <b-th style="width: 25%">{{ $t('status') }}</b-th>
                           <b-td style="width: 25%">{{ Data.davAktivContractor.STATUS }}</b-td>
                        </b-tr>

                        <b-tr>
                           <b-th style="width: 25%">{{ $t('tin') }}</b-th>
                           <b-td style="width: 25%">{{ Data.davAktivContractor.TIN }}</b-td>
                           <b-th style="width: 25%"></b-th>
                           <b-th style="width: 25%"></b-th>
                        </b-tr>
                     </b-tbody>
                  </b-table-simple>

                  <b-alert
                     show
                     variant="danger"
                     v-else
                     class="d-flex justify-content-center align-items-center"
                     style="height: 150px"
                  >
                     <feather-icon icon="ArchiveIcon" size="25" class="mr-1"></feather-icon>
                     <span style="font-size: 22px">{{ $t('NotFoundDav') }}</span>
                  </b-alert>
               </b-col>
            </b-row>
         </b-card>

         <b-card>
            <b-row>
               <b-col sm="12" md="12" class="text-left">
                  <h2 style="color: #003188">{{ $t('tadbirkorFundContractor') }}</h2>
               </b-col>
               <b-col sm="12" md="12" class="text-left">
                  <b-table-simple
                     hover
                     small
                     caption-top
                     responsive
                     bordered
                     v-if="Data.tadbirkorFundContractor.tin_pinfl"
                  >
                     <b-tbody>
                        <b-tr>
                           <b-th style="width: 25%">{{ $t('innOrPinfl') }}</b-th>
                           <b-td style="width: 25%">{{ Data.tadbirkorFundContractor.tin_pinfl }}</b-td>

                           <b-th style="width: 25%">{{ $t('name') }}</b-th>
                           <b-td style="width: 25%">{{ Data.tadbirkorFundContractor.name }}</b-td>
                        </b-tr>

                        <b-tr>
                           <b-th style="width: 25%">{{ $t('ondate') }}</b-th>
                           <b-td style="width: 25%">{{ Data.tadbirkorFundContractor.dateon }}</b-td>
                           <b-th style="width: 25%">{{ $t('businesssctorname') }}</b-th>
                           <b-td style="width: 25%">{{ Data.tadbirkorFundContractor.businesssctorname }}</b-td>
                        </b-tr>

                        <b-tr>
                           <b-th style="width: 25%">{{ $t('businesssectortypename') }}</b-th>
                           <b-td style="width: 25%">{{ Data.tadbirkorFundContractor.businesssectortypename }}</b-td>
                           <b-th style="width: 25%">{{ $t('financialassistancename') }}</b-th>
                           <b-td style="width: 25%">{{ Data.tadbirkorFundContractor.financialassistancename }}</b-td>
                        </b-tr>

                        <b-tr>
                           <b-th style="width: 25%">{{ $t('bankname') }}</b-th>
                           <b-td style="width: 25%">{{ Data.tadbirkorFundContractor.bankname }}</b-td>
                           <b-th style="width: 25%">{{ $t('regionname') }}</b-th>
                           <b-td style="width: 25%">{{ Data.tadbirkorFundContractor.regionname }}</b-td>
                        </b-tr>

                        <b-tr>
                           <b-th style="width: 25%">{{ $t('district') }}</b-th>
                           <b-td style="width: 25%">{{ Data.tadbirkorFundContractor.districtname }}</b-td>
                           <b-th style="width: 25%">{{ $t('district') }} ({{ $t('soato') }})</b-th>
                           <b-td style="width: 25%">{{ Data.tadbirkorFundContractor.districtsoato }}</b-td>
                        </b-tr>

                        <b-tr>
                           <b-th style="width: 25%">{{ $t('aidamount') }}</b-th>
                           <b-td style="width: 25%">{{ Data.tadbirkorFundContractor.aidamount }}</b-td>
                           <b-th style="width: 25%">{{ $t('newjobposition') }}</b-th>
                           <b-td style="width: 25%">{{ Data.tadbirkorFundContractor.newjobposition }}</b-td>
                        </b-tr>

                        <b-tr>
                           <b-th style="width: 25%">{{ $t('realjobposition') }}</b-th>
                           <b-td style="width: 25%">{{ Data.tadbirkorFundContractor.realjobposition }}</b-td>
                           <b-th style="width: 25%"></b-th>
                           <b-td style="width: 25%"></b-td>
                        </b-tr>

                        <b-tr>
                           <b-td colspan="4">
                              <h5>{{ $t('credits') }}</h5>
                              <b-table
                                 ref="refInvoiceListTable"
                                 :items="Data.tadbirkorFundContractor.credits"
                                 responsive
                                 :fields="fieldsCredits"
                                 primary-key="id"
                                 no-border-collapse
                                 show-empty
                                 :empty-text="$t('NotFound')"
                                 class="position-relative"
                              >
                                 <!-- <template #cell(name)="{ item }">{{ GetLanguageItem(item) }}</template> -->
                              </b-table>
                           </b-td>
                        </b-tr>
                     </b-tbody>
                  </b-table-simple>

                  <b-alert
                     show
                     variant="danger"
                     v-else
                     class="d-flex justify-content-center align-items-center"
                     style="height: 150px"
                  >
                     <feather-icon icon="ArchiveIcon" size="25" class="mr-1"></feather-icon>
                     <span style="font-size: 22px">{{ $t('NotFoundTad') }}</span>
                  </b-alert>
               </b-col>
            </b-row>
         </b-card>

         <b-card>
            <b-row>
               <b-col sm="12" md="12" class="text-left">
                  <h2 style="color: #003188">{{ $t('markaziyBankContractorCreditHistory') }}</h2>
               </b-col>
               <b-col sm="12" md="12" class="text-left">
                  <b-table
                     ref="refInvoiceListTable"
                     :items="Data.markaziyBankContractorCreditHistory"
                     responsive
                     :fields="fieldsBankContractorCreditHistory"
                     primary-key="id"
                     no-border-collapse
                     show-empty
                     :empty-text="$t('NotFound')"
                     class="position-relative"
                     v-if="Data.markaziyBankContractorCreditHistory.length > 0"
                  >
                     <!-- <template #cell(name)="{ item }">{{ GetLanguageItem(item) }}</template> -->
                  </b-table>

                  <b-alert
                     show
                     variant="danger"
                     v-else
                     class="d-flex justify-content-center align-items-center"
                     style="height: 150px"
                  >
                     <feather-icon icon="ArchiveIcon" size="25" class="mr-1"></feather-icon>
                     <span style="font-size: 22px">{{ $t('NotFoundCredit') }}</span>
                  </b-alert>
               </b-col>
            </b-row>
         </b-card>
         <b-card>
            <b-row>
               <b-col sm="12" md="12" class="text-left">
                  <h2 style="color: #003188">{{ $t('GetFromInvestmentByInn') }}</h2>
               </b-col>
               <b-col sm="12" md="12" class="text-left">
                  <b-table
                     ref="refInvoiceListTable"
                     :items="Data.InvestmentByInn"
                     responsive
                     :fields="fieldsInvestmentByInn"
                     primary-key="id"
                     no-border-collapse
                     show-empty
                     :empty-text="$t('NotFound')"
                     class="position-relative"
                     v-if="InvestmentByInn.length"
                  >
                     <template #cell(cntrStatus)="{ item }">
                        <b-badge v-if="item.cntrStatus" variant="light-success">{{ $t('active') }}</b-badge>
                        <b-badge v-if="!item.cntrStatus" variant="light-danger"> {{ $t('passive') }}</b-badge>
                     </template>
                  </b-table>

                  <b-alert
                     show
                     variant="danger"
                     v-else
                     class="d-flex justify-content-center align-items-center"
                     style="height: 150px"
                  >
                     <feather-icon icon="ArchiveIcon" size="25" class="mr-1"></feather-icon>
                     <span style="font-size: 22px">{{ $t('NotFoundCredit') }}</span>
                  </b-alert>
               </b-col>
            </b-row>
         </b-card>
      </b-overlay>
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
   BListGroup,
   BListGroupItem,
   BTr,
   BTd,
   BTfoot,
   BTh,
   BThead,
   BTbody,
   BTableSimple,
   BAlert,
   BOverlay
} from 'bootstrap-vue';
import BusinessmanCardService from '@/services/managment/businessmancard.service';
import BojxonaCard from '../bojxona/index.vue';
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
      BListGroup,
      BListGroupItem,
      BTr,
      BojxonaCard,
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
   data() {
      return {
         fieldsActivityTypes: [
            // {
            //    key: 'id',
            //    label: this.$t('id'),
            //    thClass: 'text-center',
            //    tdClass: 'text-center',
            //    sortable: true
            // },
            {
               key: 'level_id',
               label: this.$t('level_id'),
               sortable: true
            },
            {
               key: 'name',
               label: this.$t('name'),
               sortable: true
            }
         ],
         fieldsCompanyBanks: [
            {
               key: 'companyTin',
               label: this.$t('companyTin'),
               thClass: 'text-center',
               sortable: true
            },
            {
               key: 'mfo',
               label: this.$t('mfo'),
               thClass: 'text-center',
               sortable: true
            },
            {
               key: 'bankName',
               label: this.$t('bankName'),
               thClass: 'text-center',
               sortable: true
            },
            {
               key: 'paymentAccount',
               label: this.$t('paymentAccount'),
               thClass: 'text-center',
               sortable: true
            },

            // {
            //    key: 'balance',
            //    label: this.$t('balance'),
            //    thClass: 'text-center',
            //    sortable: true
            // },

            {
               key: 'openDate',
               label: this.$t('openDate'),
               thClass: 'text-center',
               sortable: true
            },
            {
               key: 'closeDate',
               label: this.$t('closeDate'),
               thClass: 'text-center',
               sortable: true
            },
            {
               key: 'statusName',
               label: this.$t('statusName'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            }
         ],
         fieldsCredits: [
            {
               key: 'amount',
               label: this.$t('amount'),
               thClass: 'text-center',
               sortable: true
            },
            {
               key: 'currency',
               label: this.$t('currency'),
               thClass: 'text-center',
               sortable: true
            }
         ],
         fieldsBankContractorCreditHistory: [
            {
               key: 'tin',
               label: this.$t('tin'),
               thClass: 'text-center',
               sortable: true
            },
            {
               key: 'name',
               label: this.$t('name'),
               thClass: 'text-center',
               sortable: true
            },
            {
               key: 'bankcode',
               label: this.$t('bankcode'),
               thClass: 'text-center',
               sortable: true
            },
            {
               key: 'bankname',
               label: this.$t('bankname'),
               thClass: 'text-center',
               sortable: true
            },

            {
               key: 'accountnumbername',
               label: this.$t('accountnumbername'),
               thClass: 'text-center',
               sortable: true
            },
            {
               key: 'numberofaccount',
               label: this.$t('numberofaccount'),
               thClass: 'text-center',
               sortable: true
            },

            {
               key: 'credittype',
               label: this.$t('credittype'),
               thClass: 'text-center',
               sortable: true
            },
            {
               key: 'loaninterestrate',
               label: this.$t('loaninterestrate'),
               thClass: 'text-center',
               sortable: true
            },
            {
               key: 'numberofcreditsreceived',
               label: this.$t('numberofcreditsreceived'),
               thClass: 'text-center',
               sortable: true
            },
            {
               key: 'creditamount',
               label: this.$t('creditamount'),
               thClass: 'text-center',
               sortable: true
            },

            {
               key: 'fromdate',
               label: this.$t('fromdate'),
               thClass: 'text-center',
               sortable: true
            },
            {
               key: 'todate',
               label: this.$t('todate'),
               thClass: 'text-center',
               sortable: true
            }
         ],
         fieldsContractGraphs: [
            {
               key: 'companyTin',
               label: this.$t('companyTin'),
               thClass: 'text-center',
               sortable: true
            },
            {
               key: 'mfo',
               label: this.$t('mfo'),
               thClass: 'text-center',
               sortable: true
            }
         ],
         fieldsInvestmentByInn: [
            {
               key: 'idn',
               label: this.$t('idn'),
               thClass: 'text-center',
               sortable: true
            },
            {
               key: 'bankId',
               label: this.$t('bankId'),
               thClass: 'text-center',
               sortable: true
            },
            {
               key: 'contractorUzName',
               label: this.$t('contractorUzName'),
               thClass: 'text-center',
               sortable: true
            },
            {
               key: 'docNo',
               label: this.$t('docNo'),
               thClass: 'text-center',
               sortable: true
            },
            {
               key: 'docDate',
               label: this.$t('docDate'),
               thClass: 'text-center',
               sortable: true
            },
            {
               key: 'contractorForName',
               label: this.$t('contractorForName'),
               thClass: 'text-center',
               sortable: true
            },
            // {
            //    key: 'contractorForCountryCode',
            //    label: this.$t('contractorForCountryCode'),
            //    thClass: 'text-center',
            //    sortable: true
            // },
            {
               key: 'cntrStatus',
               label: this.$t('cntrStatus'),
               thClass: 'text-center',
               sortable: true
            }
         ],

         filter: {
            inn: '',
            docDateFrom: '',
            docDateTo: ''
         },
         Data: {
            markaziyBankContractorCreditHistory: [],
            tadbirkorFundContractor: {
               credits: []
            },
            davAktivContractor: {},
            integrationCertificateDto: {
               contractInfo: {}
            },

            soliqContractorByTin: {
               company: {
                  okedDetail: {},
                  sooguDetail: {},
                  statusDetail: {},
                  activityTypes: []
               },
               companyBanks: [],
               companyBillingAddress: {},
               companyContact: {},
               companyExtraInfo: {},
               companyShippingAddress: [],
               director: {},
               directorAddress: {},
               directorContact: {},
               names: {}
            },
            soliqContractorFinanceBenefitByTinLastYear: {},
            soliqContractorFinanceBenefitByTinThisYear: {},
            soliqContractorEmployeeCountByTin: {},
            soliqContractorDebtByTin: {},
            getGTDByInn: []
         },
         InvestmentByInn: [],
         isBusy: false,

         contractGraphs: {
            2023: { 1: 0, 2: 0, 3: 0, 4: 0, 5: 0, 6: 0, 7: 0, 8: 0, 9: 0, 10: 0, 11: 0, 12: 0, total: 0 },
            2024: { 1: 0, 2: 0, 3: 0, 4: 0, 5: 0, 6: 0, 7: 0, 8: 0, 9: 0, 10: 0, 11: 0, 12: 0, total: 0 },
            2025: { 1: 0, 2: 0, 3: 0, 4: 0, 5: 0, 6: 0, 7: 0, 8: 0, 9: 0, 10: 0, 11: 0, 12: 0, total: 0 },
            2026: { 1: 0, 2: 0, 3: 0, 4: 0, 5: 0, 6: 0, 7: 0, 8: 0, 9: 0, 10: 0, 11: 0, 12: 0, total: 0 }
         }
      };
   },
   created() {
      // this.Refresh();
      if (this.$route.query.inn) {
         this.filter.inn = this.$route.query.inn;
         this.Refresh();
      }
   },
   methods: {
      GetLanguageItem(item) {
         if (item) {
            const lang = localStorage.getItem('locale');

            if (lang == 'uz_latn') {
               return item.name_uz_latn;
            } else if (lang == 'uz_cyrl') {
               return item.name_uz_cyrl;
            } else if (lang == 'ru') {
               return item.name_ru;
            } else {
               return item.name;
            }
         }
      },
      SortChange(data) {
         this.filter.sortBy = data.sortBy;
         this.filter.orderType = data.sortDesc ? 'desc' : 'asc';
         this.Refresh();
      },
      Refresh() {
         this.isBusy = true;
         BusinessmanCardService.GetFromInvestmentByInn({ ...this.filter, ContractorUzInn: this.filter.inn })
            .then((res) => {
               this.InvestmentByInn = res.data;
            })
            .catch((e) => {
               this.showApiError(e);
            });
         BusinessmanCardService.GetByInn(this.filter.inn)
            .then((res) => {
               this.Data = res.data;
               console.log(this.Data, 'test');
            })
            .catch((e) => {
               this.showApiError(e);
            })
            .finally(() => {
               this.isBusy = false;
               if (this.Data.integrationCertificateDto && this.Data.integrationCertificateDto.contractGraphs) {
                  this.Data.integrationCertificateDto.contractGraphs.forEach((item) => {
                     this.contractGraphs[item.yearIn][item.monthIn] = item.newVacanciesCount;
                     this.contractGraphs[item.yearIn].total += item.newVacanciesCount;
                  });
               }
            });
      }
   }
};
</script>
