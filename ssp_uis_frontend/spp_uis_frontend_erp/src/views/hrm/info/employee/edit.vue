<template>
   <b-overlay :show="show">
      <div>
         <!-- personsearch -->
         <b-card>
            <FormPersonSearch
               @update:person="onUpdatePerson"
               @get:img="getImg"
               :person="Data.person"
               :hide-organization="true"
            />

            <b-col sm="12" md="4" class="m-0 p-0 mb-1" v-if="$can('AllEmployeeCreate', 'permissions')">
               <form-select
                  v-model="Data.organizationId"
                  :options="OrganizationList"
                  required-star
                  :label="$t('organization')"
               />
            </b-col>
            <b-row>
               <b-col class="p-0" md="6"
                  ><UploadImage
                     ref="uploadImage"
                     v-if="Data.person"
                     :person-id="Data.person.id"
                     :picture-id="Data.person.pictureId"
                     @update:pictureId="(e) => (Data.person.pictureId = e)"
               /></b-col>

               <b-col v-if="ImgData.person?.passportNumber" class="p-0" cols="auto"
                  ><b-button variant="primary" @click="setImgGTSP" class="mt-2">{{
                     $t('GTSP dan rasm olish')
                  }}</b-button></b-col
               >
            </b-row>
         </b-card>

         <b-card>
            <validation-observer ref="ValidationDTO">
               <b-row class="align-items-center">
                  <b-col sm="12" md="4" class="text-left">
                     <form-input-hrm
                        v-model="Data.phoneNumber"
                        rules="required|validatorPhone"
                        mask="(998) ## ### ## ##"
                        :label="$t('phoneNumber')"
                        placeholder="(998) ## ### ## ##"
                     />
                  </b-col>
                  <b-col class="d-flex">
                     <b-form-checkbox v-model="Data.hasMilitary">{{ $t('hasMilitary') }}</b-form-checkbox>
                     <b-form-checkbox v-model="Data.hasLegalEducation" class="ml-3">{{
                        $t('hasLegalEducation')
                     }}</b-form-checkbox>
                  </b-col>
               </b-row>
            </validation-observer>
            <b-row class="mt-2">
               <b-col sm="12" md="12">
                  <b-tabs pills>
                     <b-tab :title="$t('relatives')" active>
                        <!-- personsearch relatives -->
                        <FormPersonSearch
                           ref="PersonRelativesRef"
                           :dead="true"
                           :hideOrganization="true"
                           @update:person="onUpdatePersonRelatives"
                        />

                        <validation-observer ref="ValidationTabrow1">
                           <b-row>
                              <!-- <b-col sm="12" md="3">
                                 <form-picker v-model="tabrow1.onDate" :label="$t('ondate')" required></form-picker>
                              </b-col> -->

                              <b-col sm="12" md="3">
                                 <form-select
                                    :options="RelativeDegreeList"
                                    v-model="tabrow1.relativeDegreeId"
                                    required-star
                                    label="RelativeDegree"
                                 ></form-select>
                              </b-col>
                              <b-col cols="12" md="3">
                                 <form-select
                                    :options="RegionList"
                                    v-model="tabrow1.regionId"
                                    @input="ChangeRegion"
                                    label="region"
                                 ></form-select>
                              </b-col>
                              <b-col cols="12" md="3">
                                 <form-select
                                    :options="DistrictList"
                                    :reduce="(item) => item.value"
                                    label="Region"
                                    v-model="tabrow1.districtId"
                                    @input="GetDistrict"
                                 />
                              </b-col>
                              <b-col sm="12" md="3">
                                 <div class="form-group">
                                    <form-input
                                       v-model="tabrow1.address"
                                       :disabled="tabrow1.hasDied"
                                       :label="$t('address')"
                                    />
                                 </div>
                              </b-col>
                              <b-col sm="12" md="3">
                                 <div class="form-group">
                                    <form-input
                                       v-model="tabrow1.relativeWorkPlace"
                                       :disabled="tabrow1.hasDied"
                                       :label="$t('placeofwork')"
                                    />
                                 </div>
                              </b-col>
                              <b-col sm="12" md="3">
                                 <div class="form-group">
                                    <form-input
                                       v-model="tabrow1.relativeWorkPlacePosition"
                                       :disabled="tabrow1.hasDied"
                                       :label="$t('position')"
                                    />
                                 </div>
                              </b-col>

                              <b-col sm="12" md="2">
                                 <div class="form-group">
                                    <form-input
                                       :disabled="tabrow1.hasDied"
                                       v-model="tabrow1.phoneNumber"
                                       :label="$t('phoneNumber')"
                                       mask="(998) ## ### ## ##"
                                    />
                                 </div>
                              </b-col>

                              <b-col>
                                 <b-button @click="AddRow1" class="mt-2" variant="primary">
                                    <feather-icon icon="PlusIcon" size="14"></feather-icon>
                                 </b-button>
                              </b-col>
                           </b-row>
                        </validation-observer>

                        <b-row>
                           <b-col md="12">
                              <!-- <b-table
                                 style="vertical-align: middle"
                                 :fields="fieldsTabrow1"
                                 :items="Data.relatives"
                                 class="bg-color-table text-center"
                                 bordered
                                 :responsive="true"
                              >
                                 <template #cell(user)="{ item }">{{
                                    `${item.lastName} ${item.firstName} ${item.familyName}`
                                 }}</template>
                                 <template #cell(actions)="{ index }">
                                    <div class="text-center"> -->
                              <!-- <b-link @click="Edit(item)" class="mr-1" v-c-tooltip="{ content: $t('Edit') }">
                                 <feather-icon icon="EditIcon"></feather-icon>
                               </b-link> -->
                              <!-- <b-link
                                          class="mr-1"
                                          v-b-tooltip.hover.top="$t('Delete')"
                                          @click="Data.relatives.splice(index, 1)"
                                       >
                                          <feather-icon icon="Trash2Icon"></feather-icon>
                                       </b-link>
                                    </div>
                                 </template>
                              </b-table> -->

                              <b-table-simple :bordered="true">
                                 <b-thead>
                                    <b-tr>
                                       <b-th style="text-align: center">{{ this.$t('user') }}</b-th>
                                       <b-th style="text-align: center">{{ this.$t('placeofwork') }}</b-th>
                                       <b-th style="text-align: center">{{ this.$t('position') }}</b-th>
                                       <b-th style="text-align: center">{{ this.$t('dateofbirth') }}</b-th>
                                       <b-th style="text-align: center">{{ this.$t('RelativeDegree') }}</b-th>
                                       <b-th style="text-align: center">{{ this.$t('actions') }}</b-th>
                                    </b-tr>
                                 </b-thead>
                                 <b-tbody>
                                    <b-tr v-for="(item, i) in Data.relatives" :key="i">
                                       <b-td style="text-align: left">{{
                                          `${item.lastName} ${item.firstName} ${item.familyName}`
                                       }}</b-td>
                                       <b-td style="text-align: center" :colspan="item.hasDied ? 2 : null">{{
                                          item.hasDied ? $t('vafot etgan') : item.relativeWorkPlace
                                       }}</b-td>
                                       <b-td style="text-align: center" v-if="!item?.hasDied">{{
                                          item.relativeWorkPlacePosition
                                       }}</b-td>
                                       <b-td style="text-align: center">{{ item.dateOfBirth }}</b-td>
                                       <b-td style="text-align: center">{{ item.relativeDegree }}</b-td>
                                       <b-td style="text-align: center"
                                          ><b-link
                                             class="mr-1 ;"
                                             v-b-tooltip.hover.top="$t('Delete')"
                                             @click="Data.relatives.splice(index, 1)"
                                          >
                                             <feather-icon icon="Trash2Icon"></feather-icon> </b-link
                                       ></b-td>
                                    </b-tr>
                                 </b-tbody>
                              </b-table-simple>
                           </b-col>
                        </b-row>
                     </b-tab>

                     <b-tab :title="$t('placeOfWorks')">
                        <b-row>
                           <b-col sm="12" md="3">
                              <form-input-hrm
                                 v-model="Data.person.pinfl"
                                 rules="required"
                                 disabled
                                 :label="$t('tin')"
                              ></form-input-hrm>
                           </b-col>
                           <b-col sm="12" md="1" class="pt-2">
                              <b-button @click="GetMehnatInfo(mehnatPinf)" :disabled="personLoading" variant="primary">
                                 <b-spinner v-if="personLoading" small></b-spinner>
                                 <feather-icon icon="SearchIcon" />
                              </b-button>
                           </b-col>
                        </b-row>

                        <validation-observer ref="ValidationTabrow3">
                           <b-row>
                              <b-col sm="12" md="3">
                                 <form-picker
                                    v-model="tabrow3.startOn"
                                    :label="$t('startdate')"
                                    required
                                    :placeholder="$t('startdate')"
                                 ></form-picker>
                              </b-col>
                              <b-col sm="12" md="3">
                                 <form-picker
                                    v-model="tabrow3.endOn"
                                    :label="$t('enddate')"
                                    :placeholder="$t('enddate')"
                                 ></form-picker>
                              </b-col>
                              <b-col sm="12" md="3">
                                 <form-input-hrm
                                    v-model="tabrow3.contractorName"
                                    rules="required"
                                    :label="$t('placeofwork')"
                                 ></form-input-hrm>
                                 <!-- <ContractorListSelect
                                    :label="$t('placeofwork')"
                                    v-model="tabrow3.contractorId"
                                    :valuename="tabrow3.contractorName"
                                    @update:valuename="(e) => (tabrow3.contractorName = e)"
                                    @update:data="(e) => (tabrow3.contractorName = e ? e.fullName : '')"
                                 /> -->
                              </b-col>
                              <b-col sm="12" md="3">
                                 <form-input-hrm
                                    v-model="tabrow3.positionName"
                                    rules="required"
                                    :label="$t('position')"
                                 ></form-input-hrm>
                              </b-col>

                              <b-col sm="12" md="3">
                                 <form-select
                                    :options="EmploymentTypeList"
                                    v-model="tabrow3.employmentTypeId"
                                    required-star
                                    label="employmentType"
                                 ></form-select>
                              </b-col>
                              <b-col sm="12" md="3">
                                 <b-button @click="AddRow3" class="mt-2" variant="primary">
                                    <feather-icon icon="PlusIcon" size="14"></feather-icon>
                                 </b-button>
                              </b-col>
                           </b-row>
                        </validation-observer>

                        <b-row class="mt-2">
                           <b-col md="12">
                              <b-table
                                 style="vertical-align: middle"
                                 :fields="fieldsTabrow3"
                                 :items="Data.placeOfWorks"
                                 class="bg-color-table text-center"
                                 bordered
                                 :responsive="true"
                              >
                                 <template #cell(endOn)="{ item }">
                                    {{ item.endOn ? item.endOn : $t('hozirgi kunda') }}
                                 </template>

                                 <template #cell(actions)="{ index }">
                                    <div class="text-center">
                                       <!-- <b-link @click="Edit(item)" class="mr-1" v-c-tooltip="{ content: $t('Edit') }">
                            <feather-icon icon="EditIcon"></feather-icon>
                        </b-link>-->
                                       <b-link
                                          class="mr-1"
                                          v-b-tooltip.hover.top="$t('Delete')"
                                          @click="Data.placeOfWorks.splice(index, 1)"
                                       >
                                          <feather-icon icon="Trash2Icon"></feather-icon>
                                       </b-link>
                                    </div>
                                 </template>
                              </b-table>
                           </b-col>
                        </b-row>
                     </b-tab>

                     <b-tab :title="$t('higherEdu')">
                        <validation-observer ref="ValidationTabrow4">
                           <b-row>
                              <b-col sm="12" md="3">
                                 <form-select
                                    :options="InstituteList"
                                    label="Institute"
                                    v-model="tabrow4.instituteId"
                                    @option:selected="(e) => (tabrow4.instituteName = e ? e.text : '')"
                                    @change="changeInstitute"
                                 />
                              </b-col>
                              <b-col sm="12" md="3">
                                 <form-select
                                    :options="SpecialtyList"
                                    label="Specialty"
                                    required-star
                                    v-model="tabrow4.specialtyId"
                                    @option:selected="(e) => (tabrow4.specialty = e ? e.text : '')"
                                 />
                              </b-col>

                              <!--EmployeeHigherEduDegreeSelectList -->
                              <b-col sm="12" md="3">
                                 <form-select
                                    :options="EmployeeHigherEduDegreeSelectList"
                                    label="employeeHigherEduDegreeId"
                                    required-star
                                    v-model="tabrow4.employeeHigherEduDegreeId"
                                    @option:selected="(e) => (tabrow4.employeeHigherEduDegree = e ? e.text : '')"
                                 />
                              </b-col>
                              <b-col sm="12" md="3">
                                 <form-input-hrm
                                    v-model="tabrow4.documentSeries"
                                    rules="required"
                                    :label="$t('documentseries')"
                                 ></form-input-hrm>
                              </b-col>

                              <b-col sm="12" md="3">
                                 <form-input-hrm
                                    v-model="tabrow4.documentNumber"
                                    rules="required"
                                    :label="$t('docnumber')"
                                 ></form-input-hrm>
                              </b-col>

                              <b-col sm="12" md="3">
                                 <form-picker
                                    v-model="tabrow4.dateOfIssue"
                                    :label="$t('dateofissue')"
                                    required
                                    :placeholder="$t('dateofissue')"
                                 ></form-picker>
                              </b-col>
                              <b-col sm="12" md="3">
                                 <b-button @click="AddRow4" class="mt-2" variant="primary">
                                    <feather-icon icon="PlusIcon" size="14"></feather-icon>
                                 </b-button>
                              </b-col>
                           </b-row>
                        </validation-observer>

                        <b-row class="mt-2">
                           <b-col md="12">
                              <b-table
                                 style="vertical-align: middle"
                                 :fields="fieldsTabrow4"
                                 :items="Data.higherEdu"
                                 class="bg-color-table text-center"
                                 bordered
                                 :responsive="true"
                              >
                                 <template #cell(documentid)="{ item }">
                                    {{ item.documentSeries }} {{ item.documentNumber }}
                                 </template>
                                 <template #cell(actions)="{ index }">
                                    <div class="text-center">
                                       <b-link
                                          class="mr-1"
                                          v-b-tooltip.hover.top="$t('Delete')"
                                          @click="Data.higherEdu.splice(index, 1)"
                                       >
                                          <feather-icon icon="Trash2Icon"></feather-icon>
                                       </b-link>
                                    </div>
                                 </template>
                              </b-table>
                           </b-col>
                        </b-row>
                     </b-tab>
                     <!-- academic degree -->
                     <b-tab :title="$t('academicDegree')">
                        <validation-observer ref="ValidationTabrow5">
                           <b-row>
                              <b-col sm="12" md="3">
                                 <form-select
                                    :options="AcademicDegreeList"
                                    label="academicDegree"
                                    required-star
                                    v-model="tabrow5.academicDegreeId"
                                    @option:selected="(e) => (tabrow5.academicDegree = e ? e.text : '')"
                                 />
                              </b-col>
                              <b-col sm="12" md="3">
                                 <b-button @click="AddRow5" class="mt-2" variant="primary">
                                    <feather-icon icon="PlusIcon" size="14"></feather-icon>
                                 </b-button>
                              </b-col>
                           </b-row>
                        </validation-observer>

                        <b-row class="mt-2">
                           <b-col md="12">
                              <b-table
                                 style="vertical-align: middle"
                                 :fields="fieldsTabrow5"
                                 :items="Data.academicDegrees"
                                 class="bg-color-table text-center"
                                 bordered
                                 :responsive="true"
                              >
                                 <template #cell(actions)="{ index }">
                                    <div class="text-center">
                                       <b-link
                                          class="mr-1"
                                          v-b-tooltip.hover.top="$t('Delete')"
                                          @click="Data.academicDegrees.splice(index, 1)"
                                       >
                                          <feather-icon icon="Trash2Icon"></feather-icon>
                                       </b-link>
                                    </div>
                                 </template>
                              </b-table>
                           </b-col>
                        </b-row>
                     </b-tab>

                     <!-- degressTitle -->
                     <b-tab :title="$t('degreeTitles')">
                        <validation-observer ref="ValidationTabrow6">
                           <b-row>
                              <b-col sm="12" md="3">
                                 <form-select
                                    :options="DegreeTitlesLit"
                                    label="degreeTitles"
                                    required-star
                                    v-model="tabrow6.degreeTitleId"
                                    @option:selected="(e) => (tabrow6.degreeTitle = e ? e.text : '')"
                                 />
                              </b-col>
                              <b-col sm="12" md="3">
                                 <form-picker
                                    v-model="tabrow6.year"
                                    :label="$t('dateofissue')"
                                    required
                                    :placeholder="$t('dateofissue')"
                                 ></form-picker>
                              </b-col>
                              <b-col sm="12" md="3">
                                 <b-button @click="AddRow6" class="mt-2" variant="primary">
                                    <feather-icon icon="PlusIcon" size="14"></feather-icon>
                                 </b-button>
                              </b-col>
                           </b-row>
                        </validation-observer>

                        <b-row class="mt-2">
                           <b-col md="12">
                              <b-table
                                 style="vertical-align: middle"
                                 :fields="fieldsTabrow6"
                                 :items="Data.degreeTitles"
                                 class="bg-color-table text-center"
                                 bordered
                                 :responsive="true"
                              >
                                 <template #cell(actions)="{ index }">
                                    <div class="text-center">
                                       <b-link
                                          class="mr-1"
                                          v-b-tooltip.hover.top="$t('Delete')"
                                          @click="Data.degreeTitles.splice(index, 1)"
                                       >
                                          <feather-icon icon="Trash2Icon"></feather-icon>
                                       </b-link>
                                    </div>
                                 </template>
                              </b-table>
                           </b-col>
                        </b-row>
                     </b-tab>

                     <!-- election member -->
                     <b-tab :title="$t('electionMember')">
                        <validation-observer ref="ValidationTabrow7">
                           <b-row>
                              <b-col sm="12" md="3">
                                 <form-select
                                    :options="ElectionMemberList"
                                    label="electionMember"
                                    required-star
                                    v-model="tabrow7.electionMemberId"
                                    @option:selected="(e) => (tabrow7.electionMember = e ? e.text : '')"
                                 />
                              </b-col>
                              <b-col sm="12" md="3">
                                 <form-picker
                                    v-model="tabrow7.year"
                                    :label="$t('dateofissue')"
                                    required
                                    :placeholder="$t('dateofissue')"
                                 ></form-picker>
                              </b-col>
                              <b-col sm="12" md="3">
                                 <b-button @click="AddRow7" class="mt-2" variant="primary">
                                    <feather-icon icon="PlusIcon" size="14"></feather-icon>
                                 </b-button>
                              </b-col>
                           </b-row>
                        </validation-observer>

                        <b-row class="mt-2">
                           <b-col md="12">
                              <b-table
                                 style="vertical-align: middle"
                                 :fields="fieldsTabrow7"
                                 :items="Data.electionMembers"
                                 class="bg-color-table text-center"
                                 bordered
                                 :responsive="true"
                              >
                                 <template #cell(actions)="{ index }">
                                    <div class="text-center">
                                       <b-link
                                          class="mr-1"
                                          v-b-tooltip.hover.top="$t('Delete')"
                                          @click="Data.electionMembers.splice(index, 1)"
                                       >
                                          <feather-icon icon="Trash2Icon"></feather-icon>
                                       </b-link>
                                    </div>
                                 </template>
                              </b-table>
                           </b-col>
                        </b-row>
                     </b-tab>

                     <!-- partisanships -->
                     <b-tab :title="$t('partisanships')">
                        <validation-observer ref="ValidationTabrow8">
                           <b-row>
                              <b-col sm="12" md="3">
                                 <form-select
                                    :options="PartisanshipsList"
                                    label="partisanships"
                                    required-star
                                    v-model="tabrow8.partisanshipId"
                                    @option:selected="(e) => (tabrow8.partisanship = e ? e.text : '')"
                                 />
                              </b-col>
                              <b-col sm="12" md="3">
                                 <form-picker
                                    v-model="tabrow8.year"
                                    :label="$t('dateofissue')"
                                    required
                                    :placeholder="$t('dateofissue')"
                                 ></form-picker>
                              </b-col>
                              <b-col sm="12" md="3">
                                 <b-button @click="AddRow8" class="mt-2" variant="primary">
                                    <feather-icon icon="PlusIcon" size="14"></feather-icon>
                                 </b-button>
                              </b-col>
                           </b-row>
                        </validation-observer>

                        <b-row class="mt-2">
                           <b-col md="12">
                              <b-table
                                 style="vertical-align: middle"
                                 :fields="fieldsTabrow8"
                                 :items="Data.partisanships"
                                 class="bg-color-table text-center"
                                 bordered
                                 :responsive="true"
                              >
                                 <template #cell(actions)="{ index }">
                                    <div class="text-center">
                                       <b-link
                                          class="mr-1"
                                          v-b-tooltip.hover.top="$t('Delete')"
                                          @click="Data.partisanships.splice(index, 1)"
                                       >
                                          <feather-icon icon="Trash2Icon"></feather-icon>
                                       </b-link>
                                    </div>
                                 </template>
                              </b-table>
                           </b-col>
                        </b-row>
                     </b-tab>

                     <!-- scientificDegrees -->
                     <b-tab :title="$t('scientificDegrees')">
                        <validation-observer ref="ValidationTabrow9">
                           <b-row>
                              <b-col sm="12" md="3">
                                 <form-select
                                    :options="ScientificDegreesList"
                                    label="scientificDegrees"
                                    required-star
                                    v-model="tabrow9.scientificDegreeId"
                                    @option:selected="(e) => (tabrow9.scientificDegree = e ? e.text : '')"
                                 />
                              </b-col>
                              <b-col sm="12" md="3">
                                 <b-button @click="AddRow9" class="mt-2" variant="primary">
                                    <feather-icon icon="PlusIcon" size="14"></feather-icon>
                                 </b-button>
                              </b-col>
                           </b-row>
                        </validation-observer>

                        <b-row class="mt-2">
                           <b-col md="12">
                              <b-table
                                 style="vertical-align: middle"
                                 :fields="fieldsTabrow9"
                                 :items="Data.scientificDegrees"
                                 class="bg-color-table text-center"
                                 bordered
                                 :responsive="true"
                              >
                                 <template #cell(actions)="{ index }">
                                    <div class="text-center">
                                       <b-link
                                          class="mr-1"
                                          v-b-tooltip.hover.top="$t('Delete')"
                                          @click="Data.scientificDegrees.splice(index, 1)"
                                       >
                                          <feather-icon icon="Trash2Icon"></feather-icon>
                                       </b-link>
                                    </div>
                                 </template>
                              </b-table>
                           </b-col>
                        </b-row>
                     </b-tab>
                     <!-- stateAwards -->
                     <b-tab :title="$t('stateAwards')">
                        <validation-observer ref="ValidationTabrow10">
                           <b-row>
                              <b-col sm="12" md="3">
                                 <form-select
                                    :options="StateAwardList"
                                    label="stateAwards"
                                    required-star
                                    v-model="tabrow10.stateAwardsId"
                                    @option:selected="(e) => (tabrow10.stateAwards = e ? e.text : '')"
                                 />
                              </b-col>
                              <b-col sm="12" md="3">
                                 <form-picker
                                    v-model="tabrow10.year"
                                    :label="$t('dateofissue')"
                                    required
                                    :placeholder="$t('dateofissue')"
                                 ></form-picker>
                              </b-col>
                              <b-col sm="12" md="3">
                                 <b-button @click="AddRow10" class="mt-2" variant="primary">
                                    <feather-icon icon="PlusIcon" size="14"></feather-icon>
                                 </b-button>
                              </b-col>
                           </b-row>
                        </validation-observer>

                        <b-row class="mt-2">
                           <b-col md="12">
                              <b-table
                                 style="vertical-align: middle"
                                 :fields="fieldsTabrow10"
                                 :items="Data.stateAwards"
                                 class="bg-color-table text-center"
                                 bordered
                                 :responsive="true"
                              >
                                 <template #cell(actions)="{ index }">
                                    <div class="text-center">
                                       <b-link
                                          class="mr-1"
                                          v-b-tooltip.hover.top="$t('Delete')"
                                          @click="Data.stateAwards.splice(index, 1)"
                                       >
                                          <feather-icon icon="Trash2Icon"></feather-icon>
                                       </b-link>
                                    </div>
                                 </template>
                              </b-table>
                           </b-col>
                        </b-row>
                     </b-tab>
                     <!-- militaryRanks -->
                     <b-tab :title="$t('militaryRanks')">
                        <validation-observer ref="ValidationTabrow11">
                           <b-row>
                              <b-col sm="12" md="3">
                                 <form-select
                                    :options="MilitaryRankList"
                                    label="militaryRanks"
                                    required-star
                                    v-model="tabrow11.militaryRankId"
                                    @option:selected="(e) => (tabrow11.militaryRank = e ? e.text : '')"
                                 />
                              </b-col>
                              <b-col sm="12" md="3">
                                 <b-button @click="AddRow11" class="mt-2" variant="primary">
                                    <feather-icon icon="PlusIcon" size="14"></feather-icon>
                                 </b-button>
                              </b-col>
                           </b-row>
                        </validation-observer>

                        <b-row class="mt-2">
                           <b-col md="12">
                              <b-table
                                 style="vertical-align: middle"
                                 :fields="fieldsTabrow11"
                                 :items="Data.militaryRanks"
                                 class="bg-color-table text-center"
                                 bordered
                                 :responsive="true"
                              >
                                 <template #cell(actions)="{ item, index }">
                                    <div class="text-center">
                                       <b-link
                                          class="mr-1"
                                          v-b-tooltip.hover.top="$t('Delete')"
                                          @click="Data.militaryRanks.splice(index, 1)"
                                       >
                                          <feather-icon icon="Trash2Icon"></feather-icon>
                                       </b-link>
                                    </div>
                                 </template>
                              </b-table>
                           </b-col>
                        </b-row>
                     </b-tab>
                     <!-- languageProficiencys -->
                     <b-tab :title="$t('languageProficiencys')">
                        <validation-observer ref="ValidationTabrow12">
                           <b-row>
                              <b-col sm="12" md="3">
                                 <form-select
                                    :options="LanguageProficiencyList"
                                    label="languageProficiencys"
                                    required-star
                                    v-model="tabrow12.languagperoficiencyId"
                                    @option:selected="(e) => (tabrow12.languagperoficiency = e ? e.text : '')"
                                 />
                              </b-col>

                              <b-col sm="12" md="3">
                                 <form-input-hrm
                                    v-model="tabrow12.languageDegree"
                                    :label="$t('languageDegree')"
                                 ></form-input-hrm>
                              </b-col>
                              <b-col sm="12" md="3">
                                 <b-button @click="AddRow12" class="mt-2" variant="primary">
                                    <feather-icon icon="PlusIcon" size="14"></feather-icon>
                                 </b-button>
                              </b-col>
                           </b-row>
                        </validation-observer>

                        <b-row class="mt-2">
                           <b-col md="12">
                              <b-table
                                 style="vertical-align: middle"
                                 :fields="fieldsTabrow12"
                                 :items="Data.languageProficiencys"
                                 class="bg-color-table text-center"
                                 bordered
                                 :responsive="true"
                              >
                                 <template #cell(actions)="{ index }">
                                    <div class="text-center">
                                       <b-link
                                          class="mr-1"
                                          v-b-tooltip.hover.top="$t('Delete')"
                                          @click="Data.languageProficiencys.splice(index, 1)"
                                       >
                                          <feather-icon icon="Trash2Icon"></feather-icon>
                                       </b-link>
                                    </div>
                                 </template>
                              </b-table>
                           </b-col>
                        </b-row>
                     </b-tab>
                     <!-- documents -->
                     <b-tab @click="Getdocs" v-if="DocumentList.length" :title="$t('document')">
                        <b-row>
                           <b-col>
                              <b-table-simple>
                                 <b-thead>
                                    <b-tr>
                                       <b-th>{{ $t('F.I.O') }}</b-th>
                                       <!-- <b-th>{{ $t('IdentityDocument') }}</b-th> -->
                                       <b-th>{{ $t('documentseries') }}</b-th>
                                       <b-th>{{ $t('documentnumber') }}</b-th>
                                       <b-th>{{ $t('dateofissue') }}</b-th>
                                       <b-th>{{ $t('givenOrganization') }}</b-th>
                                       <b-th>{{ $t('dateofcreated') }}</b-th>
                                    </b-tr>
                                 </b-thead>
                                 <b-tbody>
                                    <b-tr v-for="td in DocumentList" :key="td.id">
                                       <b-td>{{ td.person }}</b-td>
                                       <b-td>{{ td.passportSeria }}</b-td>
                                       <b-td>{{ td.passportNumber }}</b-td>
                                       <b-td>{{ td.passportDate }}</b-td>
                                       <b-td>{{ td.passportDivName }}</b-td>
                                       <b-td>{{ td.passportExpiration }}</b-td>
                                    </b-tr>
                                 </b-tbody>
                              </b-table-simple>
                           </b-col>
                        </b-row>
                     </b-tab>
                  </b-tabs>
               </b-col>
            </b-row>

            <b-row class="mt-3">
               <b-col sm="12" md="6" lg="6" class="text-left"></b-col>
               <b-col sm="12" md="6" lg="6" class="text-right">
                  <b-button @click="SaveData" size="sm" variant="outline-success" :disabled="loadingButton">
                     <feather-icon icon="CheckIcon"></feather-icon>
                     {{ $t('Save') }}
                  </b-button>
               </b-col>
            </b-row>
         </b-card>
      </div>
   </b-overlay>
</template>
<script>
import ManualService from '@/services/others/manual.service';
import OrganizationService from '@/services/managment/organization.service';
import EmployeeService from '@/services/info/employee.service';
import AcademicDegreeService from '@/services/hrm/academicdegree.service';
import DegreeTitleService from '@/services/hrm/degreetitle.service';
import ElectionMemberService from '@/services/hrm/electionmember.service';
import PartisanshipsService from '@/services/hrm/partisanship.service';
import StateAwardService from '@/services/hrm/stateaward.service';
import ScientificDegreeService from '@/services/hrm/scientificdegree.service';
import PositionService from '@/services/info/position.service';
import MilitaryRankService from '@/services/hrm/militaryrank.service';
import LanguageProficiencyService from '@/services/hrm/languageproficiency.service';
import IdentityDocumentService from '@/services/info/identitydocument.service';
import RelativeDegreeService from '@/services/info/relativedegree.service';
import FormPicker from '@/components/forms/form-picker.vue';
import FormPersonSearch from '@/components/forms/PersonSearch/form-person-search.vue';
import ContractorListSelect from '@/views/components/info/ContractorListSelect.vue';
import RegionService from '@/services/info/region.service';
import DistrictService from '@/services/info/district.service';
import {
   BOverlay,
   BCard,
   BCardBody,
   BRow,
   BCol,
   BTbody,
   BThead,
   BFormInput,
   BTabs,
   BTab,
   BButton,
   BTable,
   BLink,
   BFormGroup,
   VBTooltip,
   BModal,
   VBModal,
   BCardText,
   BInputGroup,
   BInputGroupAppend,
   BTr,
   BTh,
   BSpinner,
   BTableSimple,
   BTd,
   BFormCheckbox,
   BListGroup,
   BListGroupItem
} from 'bootstrap-vue';
import InstituteService from '@/services/dualedu/institute.service';
import SpecialtyService from '@/services/dualedu/specialty.service';
import UploadImage from './widgets/UploadImage.vue';
import PersonService from '@/services/others/person.service';
const tabrow1Def = {
   id: 0,
   onDate: new Date().toLocaleDateString('ru-RU'),
   relativeDegreeId: null,
   relativeDegree: '',
   familyName: '',
   firstName: '',
   lastName: '',
   dateOfBirth: '',
   pinfl: '',
   hasDied: false,
   dateOfDeath: '',
   countryId: null,
   regionId: null,
   districtId: null,
   address: '',
   phoneNumber: '',
   identityDocumentId: null,
   identityDocument: '',
   documentSeries: '',
   documentNumber: '',
   dateOfIssue: '',
   dateOfExpire: '',
   issueOrganization: '',
   nationalityId: null,
   citizenshipId: null
};

const tabrow3Def = {
   id: 0,
   contractorName: '',
   contractorId: null,
   positionId: null,
   positionName: '',
   employmentTypeId: null,
   employmentType: '',
   startOn: '',
   endOn: ''
};

const tabrow4Def = {
   id: 0,
   specialtyId: null,
   specialty: '',
   instituteId: null,
   instituteName: '',
   documentSeries: '',
   documentNumber: '',
   dateOfIssue: null,
   employeeHigherEduDegreeId: null,
   employeeHigherEduDegree: ''
};
const tabrow5Def = {
   id: 0,
   academicDegreeId: null,
   academicDegree: ''
};
const tabrow6Def = {
   id: 0,
   degreeTitleId: null,
   year: '',
   degreeTitle: ''
};
const tabrow7Def = {
   id: 0,
   electionMemberId: null,
   electionMember: '',
   year: ''
};
const tabrow8Def = {
   id: 0,
   partisanshipId: null,
   partisanship: '',
   year: ''
};
const tabrow9Def = {
   id: 0,
   scientificDegreeId: null,
   scientificDegree: ''
};
const tabrow10Def = {
   id: 0,
   stateAwardsId: null,
   stateAwards: null,
   year: ''
};
const tabrow11Def = {
   id: 0,
   militaryRankId: null,
   militaryRank: ''
};

const tabrow12Def = {
   id: 0,
   languagperoficiencyId: null,
   languagperoficiency: '',
   languageDegree: ''
};

export default {
   name: 'EmployeeEdit',
   components: {
      BSpinner,
      BOverlay,
      BCard,
      BCardBody,
      BRow,
      BCol,
      BFormInput,
      BTabs,
      BTab,
      BButton,
      BTable,
      BLink,
      BFormGroup,
      FormPicker,
      VBTooltip,
      BModal,
      VBModal,
      BTbody,
      BThead,
      BCardText,
      BInputGroup,
      BInputGroupAppend,
      BTr,
      BTd,
      BFormCheckbox,
      BListGroup,
      BListGroupItem,
      FormPersonSearch,
      ContractorListSelect,
      UploadImage,
      BTh,
      BTableSimple
   },
   directives: {
      'b-tooltip': VBTooltip
   },
   props: {
      isDialog: {
         type: Boolean,
         default: false
      }
   },
   data() {
      return {
         DocumentList: [],
         ImgData: {},
         RegionList: [],
         DistrictList: [],
         personLoading: false,
         show: true,
         PositionList: [],
         EmploymentTypeList: [],
         InstituteList: [],
         AcademicDegreeList: [],
         SpecialtyList: [],
         ElectionMemberList: [],
         OrganizationList: [],
         DegreeTitlesLit: [],
         PartisanshipsList: [],
         ScientificDegreesList: [],
         StateAwardList: [],
         MilitaryRankList: [],
         EmployeeHigherEduDegreeSelectList: [],
         LanguageProficiencyList: [],
         loadingButton: false,
         experiences: [],
         profile: {},
         mehnatPinf: '',
         Data: {
            hasMilitary: false,
            person: {},
            relatives: [],
            placeOfWorks: [],
            higherEdu: [],
            academicDegrees: [],
            degreeTitles: [],
            electionMembers: [],
            languageProficiencys: [],
            partisanships: [],
            scientificDegrees: [],
            stateAwards: [],
            militaryRanks: []
         },
         RelativeDegreeList: [],
         IdentityDocumentList: [],
         tabrow1: { ...tabrow1Def },
         tabrow3: { ...tabrow3Def },
         tabrow4: { ...tabrow4Def },
         tabrow5: { ...tabrow5Def },
         tabrow6: { ...tabrow6Def },
         tabrow7: { ...tabrow7Def },
         tabrow8: { ...tabrow8Def },
         tabrow9: { ...tabrow9Def },
         tabrow10: { ...tabrow10Def },
         tabrow11: { ...tabrow11Def },
         tabrow12: { ...tabrow12Def },
         fieldsTabrow1: [
            {
               key: 'user',
               label: this.$t('user'),
               tdClass: 'text-left',
               thClass: 'text-center'
            },
            {
               key: 'relativeWorkPlace',
               label: this.$t('placeofwork'),
               tdAttr(value, key, item) {
                  return {
                     colSpan: 2
                  };
               },
               thAttr(value, key, item) {
                  return {
                     colSpan: 2
                  };
               }
            },
            {
               key: 'relativeWorkPlacePosition',
               label: this.$t('position'),
               thAttr(value, key, item) {
                  return {
                     hide: true
                  };
               },
               tdAttr(value, key, item) {
                  return {
                     hide: true
                  };
               }
            },
            {
               key: 'dateOfBirth',
               label: this.$t('dateofbirth'),
               tdClass: 'text-left',
               thClass: 'text-center'
            },
            {
               key: 'relativeDegree',
               label: this.$t('RelativeDegree'),
               tdClass: 'text-left',
               thClass: 'text-center'
            },
            {
               key: 'actions',
               label: this.$t('actions')
            }
         ],
         fieldsTabrow3: [
            {
               key: 'contractorName',
               label: this.$t('placeofwork'),
               tdClass: 'text-left',
               thClass: 'text-center'
            },
            {
               key: 'startOn',
               label: this.$t('startdate'),
               tdClass: 'text-left',
               thClass: 'text-center'
            },
            {
               key: 'endOn',
               label: this.$t('enddate'),
               tdClass: 'text-left',
               thClass: 'text-center'
            },
            {
               key: 'employmentType',
               label: this.$t('employmentType'),
               tdClass: 'text-left',
               thClass: 'text-center'
            },
            {
               key: 'positionName',
               label: this.$t('position'),
               tdClass: 'text-left',
               thClass: 'text-center'
            },
            { key: 'actions', label: this.$t('actions') }
         ],
         fieldsTabrow4: [
            {
               key: 'instituteName',
               label: this.$t('Institute'),
               tdClass: 'text-left',
               thClass: 'text-center'
            },
            {
               key: 'specialty',
               label: this.$t('Specialty'),
               tdClass: 'text-left',
               thClass: 'text-center'
            },
            {
               key: 'employeeHigherEduDegree',
               label: this.$t('employeeHigherEduDegreeId'),
               tdClass: 'text-left',
               thClass: 'text-center'
            },
            {
               key: 'documentid',
               label: this.$t('documentid'),
               tdClass: 'text-left',
               thClass: 'text-center'
            },
            // {
            //    key: 'documentNumber',
            //    label: this.$t('docnumber'),
            //    tdClass: 'text-left',
            //    thClass: 'text-center'
            // },
            {
               key: 'dateOfIssue',
               label: this.$t('dateofissue'),
               tdClass: 'text-left',
               thClass: 'text-center'
            },
            { key: 'actions', label: this.$t('actions') }
         ],
         fieldsTabrow5: [
            {
               key: 'academicDegree',
               label: this.$t('academicDegree'),
               tdClass: 'text-left',
               thClass: 'text-center'
            },
            { key: 'actions', label: this.$t('actions') }
         ],
         fieldsTabrow6: [
            {
               key: 'degreeTitle',
               label: this.$t('degreeTitles'),
               tdClass: 'text-left',
               thClass: 'text-center'
            },
            {
               key: 'year',
               label: this.$t('dateofissue'),
               tdClass: 'text-left',
               thClass: 'text-center'
            },
            { key: 'actions', label: this.$t('actions') }
         ],
         fieldsTabrow7: [
            {
               key: 'electionMember',
               label: this.$t('electionMember'),
               tdClass: 'text-left',
               thClass: 'text-center'
            },
            {
               key: 'year',
               label: this.$t('dateofissue'),
               tdClass: 'text-left',
               thClass: 'text-center'
            },
            { key: 'actions', label: this.$t('actions') }
         ],
         fieldsTabrow8: [
            {
               key: 'partisanship',
               label: this.$t('partisanships'),
               tdClass: 'text-left',
               thClass: 'text-center'
            },
            {
               key: 'year',
               label: this.$t('dateofissue'),
               tdClass: 'text-left',
               thClass: 'text-center'
            },
            { key: 'actions', label: this.$t('actions') }
         ],
         fieldsTabrow9: [
            {
               key: 'scientificDegree',
               label: this.$t('scientificDegrees'),
               tdClass: 'text-left',
               thClass: 'text-center'
            },
            { key: 'actions', label: this.$t('actions') }
         ],
         fieldsTabrow10: [
            {
               key: 'stateAwards',
               label: this.$t('stateAwards'),
               tdClass: 'text-left',
               thClass: 'text-center'
            },
            {
               key: 'year',
               label: this.$t('dateofissue'),
               tdClass: 'text-left',
               thClass: 'text-center'
            },
            { key: 'actions', label: this.$t('actions') }
         ],
         fieldsTabrow11: [
            {
               key: 'militaryRanks',
               label: this.$t('militaryRanks'),
               tdClass: 'text-left',
               thClass: 'text-center'
            },
            { key: 'actions', label: this.$t('actions') }
         ],
         fieldsTabrow12: [
            {
               key: 'languagperoficiency',
               label: this.$t('languageProficiencys'),
               tdClass: 'text-left',
               thClass: 'text-center'
            },
            {
               key: 'languageDegree',
               label: this.$t('languageDegree'),
               tdClass: 'text-left',
               thClass: 'text-center'
            },
            { key: 'actions', label: this.$t('actions') }
         ]
      };
   },
   computed: {
      sortMehnat() {
         const sortData = this.Data.placeOfWorks?.slice(0)?.sort((a, b) => {
            const dateA = new Date(a.startOn.split('.').reverse().join('-'));
            const dateB = new Date(b.startOn.split('.').reverse().join('-'));
            return dateA - dateB;
         });
         return sortData;
      }
   },
   created() {
      this.show = true;
      const pId = this.$route.name == 'EditEmployee' ? this.$route.params.id : 0;
      RegionService.GetAsSelectList(211).then((res) => {
         this.RegionList = res.data;
      });
      EmployeeService.Get(pId)
         .then((res) => {
            this.Data = res.data;
            this.mehnatPinf = this.Data.person?.pinfl;
            // this.GetMehnatInfo(res.data.person?.pinfl);
            if (pId != 0) {
               EmployeeService.GetWorkYearFromMehnat({
                  // employeeId: res.data?.person?.id,
                  pinfl: res.data?.person?.pinfl
               })
                  .then((respo) => {
                     console.log(respo.data);
                     this.Data.totalWorkedDay = respo.data.totalDay;
                     this.Data.totalWorkedMonth = respo.data.totalMonth;
                     this.Data.totalWorkedYear = respo.data.totalYear;
                  })
                  .catch((error) => {
                     this.showApiError(error);
                  });
               EmployeeService.GetWorkYearFromEmpManage({
                  employeeId: res.data?.person?.id
               }).catch((error) => {
                  this.showApiError(error);
               });
            }
         })
         .finally(() => {
            this.show = false;
         });

      ManualService.EmploymentTypeSelectList()
         .then((res) => {
            this.EmploymentTypeList = res.data;
         })
         .catch((error) => {
            this.showApiError(error);
         });

      // ManualService.LanguageDegreeSelectList()
      //    .then((res) => {
      //       this.LanguageDegreeSelectList = res.data;
      //    })
      //    .catch((error) => {
      //       this.showApiError(error);
      //    });

      ManualService.EmployeeHigherEduDegreeSelectList().then((res) => {
         this.EmployeeHigherEduDegreeSelectList = res.data;
      });

      PositionService.GetAsSelectList()
         .then((res) => {
            this.PositionList = res.data;
         })
         .catch((error) => {
            this.showApiError(error);
         });

      RelativeDegreeService.GetAsSelectList()
         .then((res) => {
            this.RelativeDegreeList = res.data;
         })
         .catch((error) => {
            this.showApiError(error);
         });
      OrganizationService.GetAsSelectList().then((res) => {
         this.OrganizationList = res.data;
      });
      IdentityDocumentService.GetAsSelectList()
         .then((res) => {
            this.IdentityDocumentList = res.data;
         })
         .catch((error) => {
            this.showApiError(error);
         });

      InstituteService.GetAsSelectList().then((res) => {
         this.InstituteList = res.data;
      });

      AcademicDegreeService.GetAsSelectList().then((res) => {
         this.AcademicDegreeList = res.data;
      });
      DegreeTitleService.GetAsSelectList().then((res) => {
         this.DegreeTitlesLit = res.data;
      });
      ElectionMemberService.GetAsSelectList().then((res) => {
         this.ElectionMemberList = res.data;
      });
      PartisanshipsService.GetAsSelectList().then((res) => {
         this.PartisanshipsList = res.data;
      });
      ScientificDegreeService.GetAsSelectList().then((res) => {
         this.ScientificDegreesList = res.data;
      });
      StateAwardService.GetAsSelectList().then((res) => {
         this.StateAwardList = res.data;
      });
      MilitaryRankService.GetAsSelectList().then((res) => {
         this.MilitaryRankList = res.data;
      });
      LanguageProficiencyService.GetAsSelectList().then((res) => {
         this.LanguageProficiencyList = res.data;
      });
   },
   methods: {
      GetDistrict(id) {
         if (this.tabrow1.regionId) {
            DistrictService.GetAsSelectList(id).then((res) => {
               this.DistrictList = res.data;
            });
         } else {
            this.tabrow1.districtId = null;
            this.DistrictList = [];
            this.Refresh();
         }
      },
      ChangeRegion() {
         if (this.tabrow1.regionId) {
            this.GetDistrict(this.tabrow1.regionId);
         }
      },

      onUpdatePerson(e) {
         this.Data.person = Object.assign({}, e.person);
         if (e.person?.pinfl) {
            this.mehnatPinf = e.person?.pinfl;
            // this.GetMehnatInfo(e.person.pinfl);
            console.log(e.person.pinfl);
            EmployeeService.GetWorkYearFromMehnat({ pinfl: e.person?.pinfl }).then((res) => {
               console.log(res);
            });
         }
      },
      GetMehnatInfo(pinfl) {
         this.personLoading = true;
         // if (this.$route.params.id == 0) {}
         PersonService.GetMehnatHistory({ pin: pinfl })
            .then((res) => {
               this.experiences = res.data.experiences;

               res.data.experiences.forEach((item) => {
                  // start date
                  const inputDate = new Date(item.start_date);
                  let day = inputDate.getDate();
                  let month = inputDate.getMonth() + 1;
                  const year = inputDate.getFullYear();
                  day = (day < 10 ? '0' : '') + day;
                  month = (month < 10 ? '0' : '') + month;
                  const outputDateString = day + '.' + month + '.' + year;

                  // end date
                  const inputDateE = new Date(item.end_date);
                  let dayE = inputDateE.getDate();
                  let monthE = inputDateE.getMonth() + 1;
                  const yearE = inputDateE.getFullYear();
                  dayE = (dayE < 10 ? '0' : '') + dayE;
                  monthE = (monthE < 10 ? '0' : '') + monthE;
                  const outputDateStringE = dayE + '.' + monthE + '.' + yearE;

                  if (
                     !this.Data.placeOfWorks.some((obj) => {
                        return obj.additionId == item.transaction_id;
                     })
                  ) {
                     this.Data.placeOfWorks.push({
                        positionName: item.position_name,
                        contractorName: item.company_name,
                        additionId: item.transaction_id,
                        employmentTypeId: 1,
                        employmentType: 'Asosiy joy',
                        startOn: outputDateString,
                        endOn: item.end_date ? outputDateStringE : null
                     });
                  }
               });

               this.profile = res.data.profile;
               this.personLoading = false;
            })
            .catch((err) => {
               this.showApiError(err);
            })
            .finally(() => {
               this.personLoading = false;
            });
      },
      onUpdatePersonRelatives(e) {
         const { person = {}, filter = {} } = e;
         if (person) {
            if (filter.documentTypeId == 6) {
               this.tabrow1.hasDied = true;
            }
            this.tabrow1.familyName = person.patronymLatin || person.patronym;
            this.tabrow1.firstName = person.nameLatin || person.name;
            this.tabrow1.lastName = person.surnameLatin || person.surname;
            this.tabrow1.dateOfBirth = person.birthDate || person.birth_date;
            this.tabrow1.pinfl = person?.pinfl || person?.pnfl;
            this.tabrow1.countryId = person.birthCountryId;
            this.tabrow1.regionId = person.livingRegionId;
            this.tabrow1.districtId = person.livingDistrictId;
            this.tabrow1.documentSeries = person.passportSeria || person.cert_series;
            this.tabrow1.documentNumber = person.passportNumber || person.cert_number;
            this.tabrow1.nationalityId = person.nationalityId;
            this.tabrow1.citizenshipId = person.citizenshipId;
            this.tabrow1.dateOfIssue = person.passportDate?.slice(0, 10);
            this.tabrow1.dateOfExpire = person.passportExpiration?.slice(0, 10);
            this.tabrow1.issueOrganization = person.passportDivName;
            this.tabrow1.identityDocumentId = filter.documentTypeId;
            this.GetDistrict(person.livingRegionId);
         }
      },
      changeInstitute(id) {
         this.tabrow4.specialtyId = null;
         SpecialtyService.GetAsSelectList(id).then((res) => {
            this.SpecialtyList = res.data;
         });
      },
      AddRow1() {
         this.$refs.ValidationTabrow1.validate().then((success) => {
            if (success && this.tabrow1.pinfl) {
               this.tabrow1.relativeDegree = this.tabrow1.relativeDegreeId
                  ? this.RelativeDegreeList.filter((item) => item.value === this.tabrow1.relativeDegreeId)[0].text
                  : '';
               this.tabrow1.identityDocument = this.tabrow1.identityDocumentId
                  ? this.IdentityDocumentList.filter((item) => item.value === this.tabrow1.identityDocumentId)[0].text
                  : '';

               if (this.editedIndex1 > -1) {
                  Object.assign(this.Data.relatives[this.editedIndex1], this.tabrow1);
               } else {
                  this.Data.relatives.push(this.tabrow1);
               }

               this.tabrow1 = { ...tabrow1Def };
               this.$refs.PersonRelativesRef.filterReset();
               this.$refs.ValidationTabrow1.reset();
            }
         });
      },
      AddRow3() {
         if (this.tabrow3.startOn) {
            if (this.tabrow3.startOn.length != 10) {
               this.makeToast('Error: ' + this.$t('startDate'), 'danger');
               return false;
            }
         }
         if (this.tabrow3.endOn) {
            if (this.tabrow3.endOn.length != 10) {
               this.makeToast('Error: ' + this.$t('endDate'), 'danger');
               return false;
            }
         }
         this.$refs.ValidationTabrow3.validate().then((success) => {
            if (success) {
               this.tabrow3.employmentType = this.tabrow3.employmentTypeId
                  ? this.EmploymentTypeList.find((item) => item.value == this.tabrow3.employmentTypeId)?.text
                  : '';

               if (this.editedIndex1 > -1) {
                  Object.assign(this.Data.placeOfWorks[this.editedIndex1], this.tabrow3);
               } else {
                  this.Data.placeOfWorks.push(this.tabrow3);
               }
               this.tabrow3 = { ...tabrow3Def };
               this.$refs.ValidationTabrow3.reset();
            }
         });
      },
      AddRow4() {
         this.$refs.ValidationTabrow4.validate().then((success) => {
            if (success) {
               if (this.editedIndex1 > -1) {
                  Object.assign(this.Data.higherEdu[this.editedIndex1], this.tabrow4);
               } else {
                  this.Data.higherEdu.push(this.tabrow4);
               }
               this.tabrow4 = { ...tabrow4Def };
               this.$refs.ValidationTabrow4.reset();
            }
         });
      },
      AddRow5() {
         this.$refs.ValidationTabrow5.validate().then((success) => {
            if (success) {
               if (this.editedIndex1 > -1) {
                  Object.assign(this.Data.academicDegrees[this.editedIndex1], this.tabrow5);
               } else {
                  this.Data.academicDegrees.push(this.tabrow5);
               }
               this.tabrow5 = { ...tabrow5Def };
               this.$refs.ValidationTabrow5.reset();
            }
         });
      },
      AddRow6() {
         this.$refs.ValidationTabrow6.validate().then((success) => {
            if (success) {
               if (this.editedIndex1 > -1) {
                  Object.assign(this.Data.degreeTitles[this.editedIndex1], this.tabrow6);
               } else {
                  this.Data.degreeTitles.push(this.tabrow6);
               }
               this.tabrow6 = { ...tabrow6Def };
               this.$refs.ValidationTabrow6.reset();
            }
         });
      },
      AddRow7() {
         this.$refs.ValidationTabrow7.validate().then((success) => {
            if (success) {
               if (this.editedIndex1 > -1) {
                  Object.assign(this.Data.electionMembers[this.editedIndex1], this.tabrow7);
               } else {
                  this.Data.electionMembers.push(this.tabrow7);
               }
               this.tabrow7 = { ...tabrow7Def };
               this.$refs.ValidationTabrow7.reset();
            }
         });
      },
      AddRow8() {
         this.$refs.ValidationTabrow8.validate().then((success) => {
            if (success) {
               if (this.editedIndex1 > -1) {
                  Object.assign(this.Data.partisanships[this.editedIndex1], this.tabrow8);
               } else {
                  this.Data.partisanships.push(this.tabrow8);
               }
               this.tabrow8 = { ...tabrow8Def };
               this.$refs.ValidationTabrow8.reset();
            }
         });
      },
      AddRow9() {
         this.$refs.ValidationTabrow9.validate().then((success) => {
            if (success) {
               if (this.editedIndex1 > -1) {
                  Object.assign(this.Data.scientificDegrees[this.editedIndex1], this.tabrow9);
               } else {
                  this.Data.scientificDegrees.push(this.tabrow9);
               }
               this.tabrow9 = { ...tabrow9Def };
               this.$refs.ValidationTabrow9.reset();
            }
         });
      },
      AddRow10() {
         this.$refs.ValidationTabrow10.validate().then((success) => {
            if (success) {
               if (this.editedIndex1 > -1) {
                  Object.assign(this.Data.stateAwards[this.editedIndex1], this.tabrow10);
               } else {
                  this.Data.stateAwards.push(this.tabrow10);
               }
               this.tabrow9 = { ...tabrow10Def };
               this.$refs.ValidationTabrow10.reset();
            }
         });
      },
      AddRow11() {
         this.$refs.ValidationTabrow11.validate().then((success) => {
            if (success) {
               if (this.editedIndex1 > -1) {
                  Object.assign(this.Data.militaryRanks[this.editedIndex1], this.tabrow11);
               } else {
                  this.Data.militaryRanks.push(this.tabrow11);
               }
               this.tabrow11 = { ...tabrow11Def };
               this.$refs.ValidationTabrow11.reset();
            }
         });
      },
      AddRow12() {
         this.$refs.ValidationTabrow12.validate().then((success) => {
            if (success) {
               if (this.editedIndex1 > -1) {
                  Object.assign(this.Data.languageProficiencys[this.editedIndex1], this.tabrow12);
               } else {
                  this.Data.languageProficiencys.push(this.tabrow12);
               }
               this.tabrow12 = { ...tabrow12Def };
               this.$refs.ValidationTabrow12.reset();
            }
         });
      },
      SaveData() {
         this.$refs.ValidationDTO.validate().then((success) => {
            if (success) {
               this.loadingButton = true;
               EmployeeService.Update({ ...this.Data, personId: this.Data.person?.id || 0 })

                  .then((res) => {
                     this.makeToast(this.$t('SaveSuccess'), 'success');
                     if (this.isDialog) {
                        this.$emit('employee:add', res.data);
                     } else {
                        this.$router.push({ name: 'Employee' });
                     }
                  })
                  .catch((err) => {
                     this.showApiError(err);
                  })
                  .finally(() => {
                     this.loadingButton = false;
                  });
            }
         });
      },
      getImg(e) {
         this.ImgData = e;
         // console.log(e, 'ddd');
      },
      setImgGTSP() {
         this.$refs.uploadImage?.getImg(this.ImgData);
      },
      Getdocs() {
         if (this.$route.params.id != 0) {
            PersonService.GetByEmployeeId(this.$route.params.id)
               .then((res) => {
                  this.DocumentList = res.data;
               })
               .catch((error) => {
                  this.showApiError(error);
               });
         }
      }
   }
};
</script>
<style scoped>
legend {
   background-color: #000;
   color: #fff;
   padding: 3px 6px;
}

.output {
   font: 1rem 'Fira Sans', sans-serif;
}

input {
   margin: 0.4rem;
}
</style>
