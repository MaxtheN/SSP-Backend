<template>
   <b-overlay>
      <b-card>
         <validation-observer ref="ValidationDTO">
            <b-row>
               <b-col sm="12" md="2">
                  <form-input
                     disabled
                     v-model="Data.docNumber"
                     :label="$t('documentnumber')"
                     :placeholder="$t('documentnumber')"
                     rules="required"
                     required
                     :autocomplete="false"
                  />
               </b-col>
               <b-col sm="12" md="2">
                  <label for>{{ $t('docdate') }}</label>
                  <date-picker
                     v-model="Data.docOn"
                     style="width: 100%"
                     size="md"
                     rules="required"
                     required
                     required-star
                     lang="ru"
                     :placeholder="$t('docdate')"
                     value-type="format"
                     format="DD.MM.YYYY"
                  ></date-picker>
               </b-col>
               <b-col cols="12" md="2">
                  <div>
                     <form-select
                        size="md"
                        required-star
                        rules="required"
                        :options="DepartmentList"
                        v-model="Data.departmentId"
                        :label="$t('department')"
                     ></form-select>
                  </div>
               </b-col>
               <b-col cols="12" md="2">
                  <div>
                     <form-select
                        size="md"
                        required-star
                        rules="required"
                        :options="PositionList"
                        v-model="Data.positionId"
                        :label="$t('position')"
                     ></form-select>
                  </div>
               </b-col>
               <b-col cols="12" md="8">
                  <form-input
                     v-model="Data.docContent"
                     rules="required"
                     required
                     :label="$t('docContent')"
                     :placeholder="$t('docContent')"
                  />
               </b-col>
            </b-row>
         </validation-observer>
         <validation-observer ref="ValidationTabrow">
            <b-row class="mt-1">
               <b-col cols="12" md="3">
                  <EmployeeSelect2 v-model="tableRow.employeeId" @update:data="onUpdateEmployee" required-star />
               </b-col>
               <b-col cols="12" md="3">
                  <form-input-hrm
                     v-model="tableRow.details"
                     :label="$t('AdditionalInfo')"
                     :placeholder="$t('AdditionalInfo')"
                  />
               </b-col>
               <b-col sm="12" md="4" class="mb-2">
                  <h6 class="inputTitle">{{ $t('fileupload') }}</h6>
                  <b-form-file
                     v-model="selectedfile"
                     type="file"
                     @change="uploadFile"
                     accept=".pdf, .doc, .docx"
                     :placeholder="$t('Faylni tanlang')"
                  >
                  </b-form-file>
                  <div class="d-flex mt-2 justify-content-between">
                     <div>
                        {{ tableRow.files[0]?.fileName }}
                     </div>
                     <b-button
                        size="sm"
                        v-show="tableRow.files[0]?.fileName"
                        :disabled="saveLoading"
                        @click="DeleteFile"
                        variant="outline-danger"
                     >
                        <feather-icon icon="TrashIcon"></feather-icon>
                     </b-button>
                  </div>
               </b-col>
               <b-col class="text-left mt-2">
                  <b-button size="md" @click="AddTabRow" variant="primary">
                     <feather-icon icon="PlusIcon"></feather-icon>
                  </b-button>
               </b-col>
            </b-row>
         </validation-observer>
         <b-table
            :items="TableData"
            :fields="fields"
            responsive
            no-border-collapse
            :busy="isBusy"
            show-empty
            :empty-text="$t('NotFound')"
         >
            <template #cell(id)="{ item, index }">
               {{ index + 1 }}
            </template>

            <template #cell(actions)="{ item, index }">
               <b-link>
                  <feather-icon @click="TableEdit(item, index)" icon="EditIcon"></feather-icon>
               </b-link>
               <b-link class="text-danger" @click="TableDelete(item, index)" style="cursor: pointer">
                  <feather-icon icon="TrashIcon"></feather-icon>
               </b-link>
            </template>
            <template #cell(fileName)="{ item }"> {{ item.files[0]?.fileName }} </template>
         </b-table>
         <b-col cols="12" class="text-right mb-2">
            <b-button :disabled="saveLoading" @click="SaveData" variant="outline-success">
               <feather-icon icon="CheckIcon"></feather-icon>
               {{ $t('Save') }}
            </b-button>
         </b-col>
      </b-card>
   </b-overlay>
</template>

<script>
import { BOverlay, BCard, BRow, BCol, BFormFile, BButton, BTable, BModal, BLink, BCardText } from 'bootstrap-vue';
import EmployeeSelect2 from '@/views/components/employee/EmployeeSelect2.vue';

import DepartmentService from '@/services/info/department.service';
import PositionService from '@/services/info/position.service';
import CandidatesConfirmationService from '@/services/hrm/candidatesconfirmation.service';

export default {
   components: {
      BLink,
      BOverlay,
      BCardText,
      BCard,
      BModal,
      BRow,
      BCol,
      BFormFile,
      BButton,
      BTable,
      EmployeeSelect2
   },
   data() {
      return {
         isBusy: false,
         deleteLoading: false,
         saveLoading: false,
         DepartmentList: [],
         PositionList: [],
         editID: null,
         selectedfile: null,
         Data: {
            docOn: '',
            docNumber: '',
            docContent: '',
            departmentId: null,
            positionId: null,
            generalConclusion: '',
            tables: []
         },
         tableRow: {
            employeeId: null,
            employee: '',
            files: [],
            details: ''
         },
         TableData: [],
         fields: [
            {
               key: 'id',
               label: this.$t('№'),
               sortable: false,
               tdClass: 'text-center',
               thClass: 'text-center'
            },
            {
               key: 'employee',
               label: this.$t('employee'),
               tdClass: 'text-center',
               thClass: 'text-center'
            },
            {
               key: 'details',
               label: this.$t('details'),
               tdClass: 'text-center',
               thClass: 'text-center'
            },
            {
               key: 'fileName',
               label: this.$t('fileName'),
               tdClass: 'text-center',
               thClass: 'text-center'
            },
            {
               key: 'actions',
               label: this.$t('actions'),
               tdClass: 'text-center',
               thClass: 'text-center'
            }
         ]
      };
   },

   created() {
      CandidatesConfirmationService.Get(this.$route.params.id).then((res) => {
         this.Data = res.data;
         this.TableData = res.data.tables;
      });

      DepartmentService.GetAsSelectList().then((res) => {
         this.DepartmentList = res.data;
      });
      PositionService.GetAsSelectList().then((res) => {
         this.PositionList = res.data;
      });
   },
   methods: {
      AddTabRow() {
         this.$refs.ValidationTabrow.validate().then((success) => {
            if (success) {
               if (this.editID != null) {
                  this.TableData.splice(this.editID, 1, this.tableRow);
                  this.editID = null;
                  this.selectedfile = null;
                  this.tableRow = {
                     employeeId: null,
                     employee: '',
                     files: []
                  };
               } else {
                  this.TableData.push({ ...this.tableRow });
                  this.selectedfile = null;
                  this.tableRow = {
                     employeeId: null,
                     employee: '',
                     files: []
                  };
               }
            }
         });
         this.$refs.ValidationTabrow.reset();
      },

      onUpdateEmployee(e) {
         this.tableRow.employeeId = e.id;
         this.tableRow.employee = e.fullName;
      },
      TableDelete(item, index) {
         this.TableData = this.TableData.filter((el, idx) => idx != index);
      },
      TableEdit(item, index) {
         this.editID = index;
         this.tableRow = { ...item };
      },
      SaveData() {
         this.$refs.ValidationDTO.validate().then((success) => {
            if (success) {
               this.Data = { ...this.Data, tables: [...this.TableData] };

               CandidatesConfirmationService.Update(this.Data)
                  .then(() => {
                     this.makeToast(this.$t('SaveSuccess'), 'success');
                     this.$router.push({ name: 'CandidatesConfirmation' });
                  })
                  .catch((err) => {
                     this.showApiError(err);
                  })
                  .finally(() => {
                     this.saveLoading = false;
                  });
            }
         });
      },
      uploadFile(event) {
         const formData = new FormData();
         formData.append('files', event.target.files[0]);
         // console.log(formData, 'dd');
         this.tableRow.files = [];
         CandidatesConfirmationService.UploadFile(formData).then((res) => {
            this.tableRow.files.push({ ...res.data[0], id: res.data[0]?.fileId });
         });
      },
      DeleteFile() {
         this.tableRow.files = [];
         this.selectedfile = null;
      }
   }
};
</script>
