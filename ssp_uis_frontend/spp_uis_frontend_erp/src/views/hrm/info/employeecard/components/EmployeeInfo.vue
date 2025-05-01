<template>
   <div>
      <div>
         <b-col class="text-right mb-2">
            <b-button variant="primary" @click="PrintEmployeeCard">
               <feather-icon icon="PrinterIcon" v-if="!printLoading"></feather-icon>
               <b-spinner v-if="printLoading" small></b-spinner>
               {{ $t('download') }}
            </b-button>
         </b-col>
      </div>
      <div class="row">
         <div class="col-lg-4">
            <b-card>
               <div class="d-flex justify-content-center">
                  <b-avatar :src="lightboxImages" size="8rem"> </b-avatar>
               </div>
               <div class="info_person">
                  <h4 class="reference">{{ employeeInfo.department }}, {{ employeeInfo.position }}</h4>
                  <p class="text-center fullname mb-4 text-primary" style="font-size: 20px">
                     <b style="text-transform: uppercase"> {{ employeeInfo.fullName }} </b>
                  </p>
                  <p class="person-position">{{ employeeInfo.issueOrganization }}</p>
                  <!-- <p class="person-position" style="font-size: 18px">администрация</p> -->
                  <p class="person-position" style="font-size: 18px">{{ employeeInfo.positionName }}</p>
                  <div class="about-person-info">
                     <hr class="about-hr" />
                     <div class="person-info-items">
                        <div class="person-info-list">
                           <feather-icon icon="CalendarIcon" class="text-primary mt-1 mr-1" size="30"></feather-icon>
                           <div class="info">
                              <p class="birthdate-text">{{ $t('birthDate') }}</p>
                              <p class="birthdate-count">
                                 <b>{{ employeeInfo.birthDate }}</b>
                              </p>
                           </div>
                        </div>
                        <div class="person-info">
                           <hr />
                           <div class="person-info-list">
                              <feather-icon icon="UserIcon" class="text-primary mt-1 mr-1" size="30"></feather-icon>

                              <div class="info">
                                 <p class="birthdate-text">{{ $t('pinfl') }}</p>
                                 <p class="birthdate-count">
                                    <b> {{ employeeInfo.pinfl }} </b>
                                 </p>
                              </div>
                           </div>
                        </div>
                        <div class="person-info">
                           <hr />
                           <div class="person-info-list">
                              <feather-icon icon="FileIcon" class="text-primary mt-1 mr-1" size="30"></feather-icon>

                              <div class="info">
                                 <p class="birthdate-text">{{ $t('documentseriesnumber') }}</p>
                                 <p class="birthdate-count">
                                    <b>
                                       <div class="d-flex">
                                          <div>{{ employeeInfo.passportSeria }}</div>
                                          <div class="text-center pl-2">{{ employeeInfo.passportNumber }}</div>
                                       </div>
                                    </b>
                                 </p>
                              </div>
                           </div>
                        </div>
                        <div class="person-info">
                           <hr />
                           <div class="person-info-list">
                              <feather-icon icon="MapPinIcon" class="text-primary mt-1 mr-1" size="30"></feather-icon>
                              <div class="info">
                                 <p class="birthdate-text">{{ $t('PlaceOfBirth') }}</p>
                                 <p class="birthdate-count">
                                    <b>{{ employeeInfo.birthCountry }}</b>
                                 </p>
                              </div>
                           </div>
                        </div>
                        <!-- <div class="person-info">
                           <hr />
                           <div class="person-info-list">
                              <feather-icon icon="MapPinIcon" class="text-primary mt-1 mr-1" size="30"></feather-icon>
                              <div class="info">
                                 <p class="birthdate-text">{{ $t('liveplace') }}</p>
                                 <p class="birthdate-count">
                                    <b>{{ employeeInfo.livingRegion }} , {{ employeeInfo.livingDistrict }}</b>
                                 </p>
                              </div>
                           </div>
                        </div> -->
                        <div class="person-info">
                           <hr />
                           <div class="person-info-list">
                              <feather-icon icon="GlobeIcon" class="text-primary mt-1 mr-1" size="30"></feather-icon>
                              <div class="info">
                                 <p class="birthdate-text">{{ $t('nationality') }}</p>
                                 <p class="birthdate-count">
                                    <b>{{ employeeInfo.nationality }}</b>
                                 </p>
                              </div>
                           </div>
                        </div>
                     </div>
                  </div>
               </div>
            </b-card>
         </div>
         <div class="col-lg-8">
            <div class="row" style="height: auto">
               <div class="col-lg-6">
                  <div class="card" style="min-height: 565px">
                     <div class="card-body">
                        <div class="person-info-items">
                           <div class="person-info-list">
                              <div class="info">
                                 <p class="birthdate-text">{{ $t('partisanships') }}:</p>
                                 <p
                                    class="birthdate-count"
                                    v-for="(partisanship, i) in employeeInfo.partisanships"
                                    :key="'partisanship' + i"
                                 >
                                    <b> {{ partisanship.partisanship }} </b>
                                 </p>
                              </div>
                           </div>
                           <template v-for="(h, i) in employeeInfo.higherEdu">
                              <div class="person-info" :key="'higherE' + i">
                                 <hr />
                                 <div class="person-info-list">
                                    <div class="info">
                                       <p class="birthdate-text">{{ $t('higherEdu') }}:</p>
                                    </div>
                                    <p class="birthdate-count">
                                       <b> {{ h.institute }} </b>
                                    </p>
                                    <p class="birthdate-count">
                                       <b> {{ h.documentSeries }} </b> {{ h.documentNumber }}
                                    </p>
                                 </div>
                                 <hr />
                                 <div class="person-info-list">
                                    <div class="info">
                                       <p class="birthdate-text">{{ $t('Specialty') }}:</p>
                                       <p class="birthdate-count">
                                          <b> {{ h.specialty }} </b>
                                       </p>
                                    </div>
                                 </div>
                              </div>
                           </template>

                           <div class="person-info">
                              <hr />
                              <div class="person-info-list">
                                 <div class="info">
                                    <p class="birthdate-text">Norezident:</p>
                                    <p class="birthdate-count"><b> Yo`q </b></p>
                                 </div>
                              </div>
                           </div>
                        </div>
                     </div>
                  </div>
               </div>

               <div class="col-lg-6" v-if="employeeInfo.academicDegrees.length">
                  <div class="card" style="min-height: 525px">
                     <div class="card-body">
                        <div class="person-info-items">
                           <div class="person-info">
                              <div class="person-info-list">
                                 <div class="info">
                                    <p class="birthdate-text">{{ $t('academicDegree') }}:</p>
                                 </div>

                                 <template v-for="(h, i) in employeeInfo.academicDegrees">
                                    <p class="birthdate-count" :key="'academicDegrees' + i">
                                       <b> {{ i + 1 }} ) {{ h.academicDegree }} </b>
                                    </p>
                                 </template>
                              </div>
                           </div>
                           <div class="person-info">
                              <hr />

                              <div class="person-info-list">
                                 <div class="info">
                                    <p class="birthdate-text">{{ $t('scientificDegrees') }}:</p>
                                 </div>
                                 <template v-for="(h, i) in employeeInfo.scientificDegrees">
                                    <p class="birthdate-count" :key="'scientificDegrees' + i">
                                       <b> {{ i + 1 }} ) {{ h.scientificDegree }} </b>
                                    </p>
                                 </template>
                              </div>
                           </div>

                           <div class="person-info">
                              <hr />
                              <div class="person-info-list">
                                 <div class="info">
                                    <p class="birthdate-text">{{ $t('languageProficiencys') }}:</p>
                                 </div>
                                 <template v-for="(h, i) in employeeInfo.languageProficiencys">
                                    <p class="birthdate-count" :key="'languageProficiencys' + i">
                                       <b> {{ i + 1 }} ) {{ h.languagperoficiency }} </b>
                                    </p>
                                 </template>
                              </div>
                           </div>

                           <div class="person-info">
                              <hr />
                              <div class="person-info-list">
                                 <div class="info">
                                    <p class="birthdate-text">{{ $t('stateAwards') }}:</p>
                                 </div>
                                 <template v-for="(h, i) in employeeInfo.stateAwards">
                                    <p class="birthdate-count" :key="'stateAwards' + i">
                                       <b> {{ i + 1 }} ) {{ h.stateAwards }} </b>
                                    </p>
                                 </template>
                              </div>
                           </div>
                        </div>
                     </div>
                  </div>
               </div>
            </div>

            <!-- МЕҲНАТ ФАОЛИЯТИ -->
            <b-card v-if="employeeInfo.placeOfWorks.length">
               <div class="header-info">
                  <feather-icon icon="ShoppingBagIcon" class="text-primary mr-1" size="30"></feather-icon>
                  <div style="width: 100%">
                     <div class="d-flex justify-content-between">
                        <h3 class="title text-primary mb-0">{{ $t('placeOfWorks') }}</h3>
                     </div>
                     <hr />
                  </div>
               </div>
               <div class="portfolio-items">
                  <div class="step active" v-for="(works, j) in employeeInfo.placeOfWorks" :key="j + 'works'">
                     <div class="v-stepper">
                        <div class="circle"></div>
                        <div class="line"></div>
                     </div>
                     <div class="content">
                        <div class="portfolio-list">
                           <p class="portfolio-year" style="white-space: nowrap">
                              {{ works.startOn }} - <span v-if="works.endOn"> {{ works.endOn }}</span>
                              <span v-else> {{ $t('hozirgi kunda') }}</span>
                           </p>
                           <p class="text">{{ works.contractor }}</p>
                           <span style="font-size: 16px">{{ works.positionName }} </span>
                        </div>
                     </div>
                  </div>
               </div>
            </b-card>
         </div>
      </div>

      <!-- relatives -->
      <b-card v-if="employeeInfo.relatives.length">
         <div class="header-info">
            <feather-icon icon="UsersIcon" class="text-primary mr-1" size="30"></feather-icon>

            <div style="width: 100%">
               <h3 class="title text-primary">{{ $t('relatives') }}</h3>
               <hr />
            </div>
         </div>
         <div class="custom-class-table-simple table-responsive" style="border-radius: 10px">
            <table role="table" class="table b-table b-table-no-border-collapse table-sm b-table-caption-top">
               <thead role="rowgroup" class="">
                  <tr role="row" class="">
                     <th role="columnheader" scope="col" class="">{{ $t('RelativeDegree') }}</th>
                     <th role="columnheader" scope="col" class="">{{ $t('FIO') }}</th>
                     <th role="columnheader" scope="col" class="">{{ $t('dateOfbirth') }}</th>
                     <th role="columnheader" scope="col" class="">
                        {{ $t('placeofwork') }}
                     </th>
                     <th role="columnheader" scope="col" class="">{{ $t('liveplace') }}</th>
                  </tr>
               </thead>
               <tbody role="rowgroup">
                  <tr v-for="(relative, i) in employeeInfo.relatives" :key="i + 'relatives'">
                     <td>{{ relative.relativeDegree }}</td>
                     <td>{{ relative.familyName }} {{ relative.firstName }} {{ relative.lastName }}</td>
                     <td>{{ relative.dateOfBirth }}</td>
                     <td>{{ relative.relativeWorkPlace }}</td>
                     <td>{{ relative.region }} {{ relative.district }} {{ relative.address }}</td>
                  </tr>
               </tbody>
            </table>
         </div>
      </b-card>
   </div>
</template>

<script>
import EmployeeService from '@/services/info/employee.service';
import PersonService from '@/services/others/person.service';
import axios from 'axios';

import { BCard, BCardText, BRow, BCol, BButton, BSpinner, BOverlay, BAvatar, BTab, BTabs } from 'bootstrap-vue';
export default {
   components: {
      BCard,
      BTab,
      BCardText,
      BRow,
      BCol,
      BButton,
      BSpinner,
      BOverlay,
      BAvatar,
      BTabs
   },
   props: {
      employeeInfo: {
         type: Object,
         default: () => ({})
      }
   },
   data() {
      return {
         printLoading: false
      };
   },
   computed: {
      lightboxImages() {
         return axios.defaults.baseURL + 'Person/DownloadFile/' + this.employeeInfo.pictureId;
      }
      // toCapitalaze(data) {
      //    return data.toLowercase().lo
      // }
   },
   methods: {
      DonwloadUserimg(imgId) {
         PersonService.DownloadFile(imgId).then((response) => {
            const arrayBuffer = response.data;
            const base64String = btoa(String.fromCharCode.apply(null, new Uint8Array(arrayBuffer)));
            this.userImg = `data:${response.headers['content-type']};base64,${base64String}`;
         });
      },

      PrintEmployeeCard() {
         this.printLoading = true;
         EmployeeService.DownloadEmployeeCv(this.employeeInfo.pinfl)
            .then((res) => {
               this.printLoading = false;
               this.forceFileDownload(res, this.employeeInfo?.pinfl + '_' + this.employeeInfo?.fullName, '.pdf');
            })
            .catch((error) => {
               this.showApiError(error);
            });
      }
   }
};
</script>

<style lang="scss" scoped>
.step {
   padding: 10px;
   display: -webkit-box;
   display: -ms-flexbox;
   display: flex;
   -webkit-box-orient: horizontal;
   -webkit-box-direction: normal;
   -ms-flex-direction: row;
   flex-direction: row;
   -webkit-box-pack: start;
   -ms-flex-pack: start;
   justify-content: flex-start;
   background-color: cream;
}

.v-stepper {
   position: relative;
   top: 30px;
}

.step .circle {
   background-color: #fff;
   border: 3px solid #26154f;
   border-radius: 100%;
   width: 20px;
   height: 20px;
   display: inline-block;
}

.step .line {
   top: 23px;
   left: 9px;
   height: 100%;
   position: absolute;
   border-left: 3px solid #26154f;
}

.step.completed .circle {
   visibility: visible;
   background-color: #26154f;
   border-color: #26154f;
}

.step.completed .line {
   border-left: 3px solid #26154f;
}

.step.active .circle {
   visibility: visible;
   border-color: #26154f;
}

.step.empty .circle {
   visibility: hidden;
}

.step.empty .line {
   top: 0;
   height: 150%;
}

.step:last-child .line {
   border-left: 3px solid #fff;
   z-index: -1;
}

.content {
   margin-left: 20px;
   display: inline-block;
}
.info_person .reference {
   color: #707070;
   font-size: 17px;
   text-align: center;
   margin-top: 20px;
}

.info_person .fullname {
   font-size: 32px;
}

.info_person .year-title {
   padding: 5px 10px;
   border: 1px solid #26154f;
   border-radius: 7px;
   text-align: center;
   min-width: 240px;
   font-size: 19px;
   color: #26154f;
}

.info_person .person-position {
   font-size: 24px;
   color: #000;
   font-weight: 500;
}

.info_person .person-info-items {
   display: block;
}

.info_person .person-info-items .person-info-list {
   display: -webkit-box;
   display: -ms-flexbox;
   display: flex;
}

.info_person .person-info-items .person-info-list .image {
   margin-top: 17px;
   margin-right: 15px;
}

.info_person .person-info-items .person-info-list .info .birthdate-text {
   padding: 0;
   margin: 0;
   font-size: 18px;
   color: #707070;
}

.info_person .person-info-items .person-info-list .info .birthdate-count {
   padding-top: 3px;
   font-size: 22px;
   color: #000;
}

.person-info-right .birthdate-text {
   padding: 0;
   margin: 0;
   font-size: 16px;
   color: #707070;
}

.person-info-right .birthdate-count {
   padding-top: 3px;
   font-size: 18px;
   color: #000;
}

.all-info {
   display: -webkit-box;
   display: -ms-flexbox;
   display: flex;
   padding-left: 70px;
   padding-top: 40px;
}

.all-info .birthdate-text {
   padding: 0;
   margin: 0;
   font-size: 16px;
   color: #707070;
}

.all-info .birthdate-count {
   padding-top: 3px;
   font-size: 18px;
   color: #000;
}

.header-info {
   -webkit-box-align: start;
   -ms-flex-align: start;
   align-items: flex-start;
}

.header-info,
.header-info .portfolio-img {
   display: -webkit-box;
   display: -ms-flexbox;
   display: flex;
}

.header-info .portfolio-img {
   width: 51px;
   height: 51px;
   border-radius: 50%;
   -webkit-box-align: center;
   -ms-flex-align: center;
   align-items: center;
   -webkit-box-pack: center;
   -ms-flex-pack: center;
   justify-content: center;
   background-color: #26154f;
   margin-right: 20px;
}

.header-info .title {
   text-transform: uppercase;
   font-size: 28px;
}

.portfolio-items,
.portfolio-items .portfolio-list {
   margin-top: 20px;
}

.portfolio-items .portfolio-list .portfolio-year {
   padding: 6px 12px;
   color: #fff;
   text-align: center;
   font-size: 18px;
   min-width: 150px;
   width: -webkit-fit-content;
   width: -moz-fit-content;
   width: fit-content;
   border-radius: 7px;
   font-weight: 500;
   margin-bottom: 10px;
   background-color: #02469b;
}

.portfolio-items .portfolio-list .text {
   font-size: 22px;
   color: #000;
   line-height: 25px;
   font-weight: 500;
   text-transform: capitalize;
}

.custom-class-table-simple table thead th {
   padding: 16px;
   font-size: 16px;
}

.custom-class-table-simple table tbody tr td {
   padding: 12px;
}
</style>
