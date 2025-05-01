<template>
   <b-overlay :show="show">
      <b-card>
         <validation-observer ref="ValidationDTO">
            <b-row>
               <b-col sm="12" md="3">
                  <div class="form-group">
                     <form-input v-model="Data.code" :label="$t('code')" />
                  </div>
               </b-col>
               <b-col sm="12" md="4" class="mb-1">
                  <form-input-translate
                     v-model="Data.fullName"
                     @update:translates="(e) => (Data.translates = e)"
                     :translates="Data.translates"
                     column-name="full_name"
                     required
                     @input="(v) => (Data.shortName = v)"
                     :label="$t('fullname')"
                     :placeholder="$t('fullname')"
                  />
               </b-col>
               <b-col sm="12" md="4" class="mb-1">
                  <form-input-translate
                     v-model="Data.shortName"
                     @update:translates="(e) => (Data.translates = e)"
                     :translates="Data.translates"
                     column-name="short_name"
                     required
                     :label="$t('shortname')"
                     :placeholder="$t('shortname')"
                  />
               </b-col>
               <b-col sm="12" md="3">
                  <form-select
                     :options="WorkScheduleKindList"
                     v-model="Data.workScheduleKindId"
                     label="workScheduleKind"
                  ></form-select>
               </b-col>

               <b-col sm="12" md="3">
                  <form-select :options="StateList" v-model="Data.stateId" required-star label="Status"></form-select>
               </b-col>
            </b-row>
            <hr />
            <b-tabs class="tab-nav" @input="editedIndex = -1">
               <b-tab :title="$t('dayHours')">
                  <b-row align-v="center">
                     <b-col sm="12" md="2">
                        <div class="form-group">
                           <form-input v-model="TabrowDayHours.dayNumber" :label="$t('dayNumber')" type="number" />
                        </div>
                     </b-col>

                     <b-col sm="12" md="2">
                        <b-form-checkbox v-model="TabrowDayHours.isDayOff">
                           {{ $t('isDayOff') }}
                        </b-form-checkbox>
                     </b-col>
                     <b-col sm="12" md="2">
                        <form-picker
                           v-model="TabrowDayHours.beginAt"
                           :label="$t('beginAt')"
                           :placeholder="$t('beginAt')"
                        ></form-picker>
                     </b-col>
                     <b-col sm="12" md="2">
                        <form-picker
                           v-model="TabrowDayHours.endAt"
                           :label="$t('endAt')"
                           :placeholder="$t('endAt')"
                        ></form-picker>
                     </b-col>

                     <b-col class="my-auto" md="4" sm="12">
                        <b-button @click="AddrowDayHours" size="sm" variant="outline-primary">
                           <feather-icon icon="PlusIcon"></feather-icon>
                           {{ editedIndex > -1 ? $t('Edit') : $t('Add') }}
                        </b-button>
                     </b-col>
                  </b-row>
                  <b-row class="mt-2">
                     <b-col>
                        <b-table
                           :fields="fieldsDayHours"
                           small
                           responsive="sm"
                           :items="Data.dayHours"
                           sticky-header
                           style="max-height: 500px"
                        >
                           <template #cell(isDayOff)="{ item }">
                              {{ item.isDayOff ? $t('yes') : $t('no') }}
                           </template>
                           <template #cell(actions)="{ item, index }">
                              <div class="text-center">
                                 <b-link>
                                    <feather-icon @click="EditTabrowDayHours(item)" icon="EditIcon"></feather-icon>
                                 </b-link>
                                 <b-link>
                                    <feather-icon
                                       @click="DeleteTabrowDayHours(index)"
                                       icon="Trash2Icon"
                                       class="text-danger mx-2"
                                    ></feather-icon>
                                 </b-link>
                              </div>
                           </template>
                           <template #cell(order)="{ index }">
                              <span>{{ index + 1 }}</span>
                           </template>
                        </b-table>
                     </b-col>
                  </b-row>
               </b-tab>
               <b-tab :title="$t('workHours')">
                  <b-row>
                     <b-col sm="12" md="2">
                        <form-picker
                           v-model="TabrowWorkHours.dateOn"
                           :label="$t('docOn')"
                           :placeholder="$t('docOn')"
                        ></form-picker>
                     </b-col>
                     <b-col sm="12" md="2">
                        <div class="form-group">
                           <form-input v-model="TabrowWorkHours.days" :label="$t('days')" type="number" />
                        </div>
                     </b-col>
                     <b-col sm="12" md="2">
                        <div class="form-group">
                           <form-input v-model="TabrowWorkHours.hours" :label="$t('hours')" type="number" />
                        </div>
                     </b-col>

                     <b-col class="my-auto">
                        <b-button @click="AddrowWorkHours" size="sm" variant="outline-primary">
                           <feather-icon icon="PlusIcon"></feather-icon>
                           {{ editedIndex > -1 ? $t('Edit') : $t('Add') }}
                        </b-button>
                     </b-col>
                  </b-row>

                  <b-table
                     class="mt-2 overflow-auto"
                     :fields="fieldsWorkHours"
                     small
                     responsive="sm"
                     sticky-header
                     style="max-height: 500px"
                     :items="Data.workHours"
                  >
                     <template #cell(actions)="{ item, index }">
                        <div class="text-center">
                           <b-link>
                              <feather-icon
                                 style="margin-right: 5px"
                                 @click="EditTabrowWorkHours(item)"
                                 icon="EditIcon"
                              ></feather-icon>
                           </b-link>
                           <b-link>
                              <feather-icon
                                 @click="DeleteTabrowWorkHours(index)"
                                 icon="Trash2Icon"
                                 class="text-danger mx-2"
                              ></feather-icon>
                           </b-link>
                        </div>
                     </template>
                     <template #cell(order)="{ index }">
                        <span>{{ index + 1 }}</span>
                     </template>
                  </b-table>
               </b-tab>
            </b-tabs>

            <b-row class="mt-3">
               <b-col sm="12" md="6" lg="6" class="text-left"></b-col>
               <b-col sm="12" md="6" lg="6" class="text-right">
                  <b-button :disabled="saveLoading" @click="SaveData" size="sm" variant="outline-success">
                     <feather-icon icon="CheckIcon"></feather-icon>
                     {{ $t('Save') }}
                  </b-button>
               </b-col>
            </b-row>
         </validation-observer>
      </b-card>
   </b-overlay>
</template>
<script>
import ManualService from '@/services/others/manual.service';
import WorkScheduleService from '@/services/info/workschedule.service';
import { ValidationObserver } from 'vee-validate';
import FormInputTranslate from '@/components/translates/FormInputTranslate.vue';

import {
   BOverlay,
   BCard,
   BCardBody,
   BRow,
   BCol,
   BFormInput,
   BButton,
   BFormGroup,
   BModal,
   BInputGroup,
   BInputGroupAppend,
   BFormCheckbox,
   BTable,
   BTd,
   BTr,
   BTabs,
   BTab,
   BLink
} from 'bootstrap-vue';

export default {
   components: {
      BOverlay,
      BCard,
      BCardBody,
      BRow,
      BCol,
      BFormInput,
      BButton,
      BFormGroup,
      BModal,
      BInputGroup,
      BInputGroupAppend,
      BFormCheckbox,
      ValidationObserver,
      BTable,
      BTd,
      BTr,
      BTabs,
      BTab,
      BLink,
      FormInputTranslate
   },
   name: 'Edit',

   data() {
      return {
         show: false,
         StateList: [],
         TabrowDayHours: {
            dayNumber: 0,
            isDayOff: false,
            beginAt: '',
            endAt: ''
         },
         TabrowWorkHours: {
            dateOn: '',
            days: 0,
            hours: 0
         },
         fieldsDayHours: [
            {
               key: 'order',
               label: this.$t('№'),
               sortable: true
            },
            {
               key: 'dayNumber',
               label: this.$t('dayNumber'),

               sortable: true
            },

            {
               key: 'isDayOff',
               label: this.$t('isDayOff'),
               sortable: true
            },
            {
               key: 'beginAt',
               label: this.$t('beginAt'),
               sortable: true
            },
            {
               key: 'endAt',
               label: this.$t('endAt'),
               sortable: true
            },

            {
               key: 'actions',
               label: this.$t('actions'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            }
         ],
         fieldsWorkHours: [
            {
               key: 'order',
               label: this.$t('№'),
               sortable: true
            },
            {
               key: 'dateOn',
               label: this.$t('docOn'),

               sortable: true
            },

            {
               key: 'days',
               label: this.$t('days'),
               sortable: true
            },
            {
               key: 'hours',
               label: this.$t('hours'),
               sortable: true
            },

            {
               key: 'actions',
               label: this.$t('actions'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            }
         ],
         WorkScheduleKindList: [],
         loadingButton: false,
         saveLoading: false,
         Data: {
            translates: []
         },
         editedIndex: -1
      };
   },

   created() {
      this.show = true;
      WorkScheduleService.Get(this.$route.params.id).then((res) => {
         this.show = false;
         this.Data = res.data;
      });

      ManualService.StateSelectList()
         .then((res) => {
            this.StateList = res.data;
         })
         .catch((error) => {
            this.showApiError(error);
         });
      ManualService.WorkScheduleKindSelectList()
         .then((res) => {
            this.WorkScheduleKindList = res.data;
         })
         .catch((error) => {
            this.showApiError(error);
         });
   },
   methods: {
      DeleteTabrowDayHours(index) {
         this.Data.dayHours.splice(index, 1);
      },
      DeleteTabrowWorkHours(index) {
         this.Data.workHours.splice(index, 1);
      },
      EditTabrowDayHours(item) {
         this.editedIndex = this.Data.dayHours.indexOf(item);
         this.TabrowDayHours = Object.assign({}, item);
         this.TabrowDayHours.beginAt = this.TabrowDayHours.beginAt.split(':').join('.');
         this.TabrowDayHours.endAt = this.TabrowDayHours.endAt.split(':').join('.');
      },
      EditTabrowWorkHours(item) {
         this.editedIndex = this.Data.workHours.indexOf(item);
         this.TabrowWorkHours = Object.assign({}, item);
      },
      AddrowDayHours() {
         this.TabrowDayHours.beginAt = this.TabrowDayHours.beginAt.split('.').join(':');
         this.TabrowDayHours.endAt = this.TabrowDayHours.endAt.split('.').join(':');

         this.TabrowDayHours.dayNumber = +this.TabrowDayHours.dayNumber;
         if (this.editedIndex > -1) {
            Object.assign(this.Data.dayHours[this.editedIndex], this.TabrowDayHours);
            this.editedIndex = -1;
            this.TabrowDayHours = {
               dayNumber: 0,
               isDayOff: false,
               beginAt: '',
               endAt: ''
            };
         } else {
            this.Data.dayHours.push(this.TabrowDayHours);
            this.TabrowDayHours = {
               dayNumber: 0,
               isDayOff: false,
               beginAt: '',
               endAt: ''
            };
         }
      },
      AddrowWorkHours() {
         this.TabrowWorkHours.days = +this.TabrowWorkHours.days;
         this.TabrowWorkHours.hours = +this.TabrowWorkHours.hours;
         if (this.editedIndex > -1) {
            Object.assign(this.Data.workHours[this.editedIndex], this.TabrowWorkHours);
            this.editedIndex = -1;
            this.TabrowWorkHours = {
               dateOn: '',
               days: 0,
               hours: 0
            };
         } else {
            this.Data.workHours.push(this.TabrowWorkHours);
            this.TabrowWorkHours = {
               dateOn: '',
               days: 0,
               hours: 0
            };
         }
      },
      SaveData() {
         this.$refs.ValidationDTO.validate().then((success) => {
            if (success) {
               this.saveLoading = true;

               WorkScheduleService.Update(this.Data)
                  .then((res) => {
                     this.makeToast(this.$t('SaveSuccess'), 'success');
                     this.$router.push({ name: 'WorkSchedule' });
                  })
                  .catch((err) => {
                     this.showApiError(err);
                  })
                  .finally(() => {
                     this.saveLoading = false;
                  });
            }
         });
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
@/services/info/position.service
