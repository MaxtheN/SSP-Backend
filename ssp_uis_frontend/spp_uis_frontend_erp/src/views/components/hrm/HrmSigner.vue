<template>
   <validation-observer ref="ValidationHrmSigner" disabled>
      <b-row class="mt-2">
         <b-col>
            <b-row v-for="(agree, index) in allSigner" :key="index">
               <b-col cols="12">
                  <!-- isDirector code=1 -->
                  <h2 v-if="agree.isDirector">{{ $t('Principal signatory') }}</h2>
                  <!-- isHr -->
                  <h2 v-else-if="agree.isHr">{{ $t('Entered') }}</h2>
                  <!-- other -->
                  <h2 v-else-if="index == 2 && !agree.isHr && !agree.isDirector">
                     {{ $t('Agreed') }}
                  </h2>
               </b-col>
               <b-col sm="12" md="3">
                  <form-select
                     :options="
                        DepartmentListFilter(
                           agree.isDirector ? DEPARTMENT_CODE['director'] : agree.isHr ? DEPARTMENT_CODE['hr'] : null
                        )
                     "
                     v-model="agree.departmentId"
                     label="Department"
                     :required-star="index < 2"
                     @option:selected="(e) => (agree.department = e ? e.text : '')"
                     @input="
                        () => {
                           agree.positionId = null;
                           agree.position = null;
                        }
                     "
                     :vid="'department' + index"
                  ></form-select>
               </b-col>
               <b-col sm="12" md="3">
                  <form-select
                     :options="PositionListFilter(agree.departmentId)"
                     v-model="agree.positionId"
                     label="position"
                     valueid="positionId"
                     valuename="positionName"
                     :required-star="index < 2"
                     @input="
                        () => {
                           agree.employeeManageId = null;
                           agree.employee = null;
                        }
                     "
                     @option:selected="(e) => (agree.position = e ? e.positionName : '')"
                     :vid="'position' + index"
                  ></form-select>
               </b-col>

               <b-col sm="12" md="3">
                  <HrmEmployeeManageSelect2
                     :required-star="index < 2"
                     :organization-id="mainOrganizationId"
                     :position-id="agree.positionId"
                     :department-id="agree.departmentId"
                     v-model="agree.employeeManageId"
                     :director-id="signerDirector?.employeeManageId"
                     :hr-id="signerHr?.employeeManageId"
                     @option:selected="(e) => (agree.employee = e ? e.employee : '')"
                     :vid="'employeManage' + index"
                     :isEmployee="!agree.isDirector && !agree.isHr"
                     :allSigner="allSigner"
                     :disabledEmployeManageIds="disabledEmployeManageIds"
                  />
               </b-col>
               <b-col sm="12" md="3" v-if="index > 1 && !isView">
                  <b-button variant="outline-danger" class="mt-2" size="sm" @click="deleteSigner(index)">
                     <feather-icon icon="XIcon"></feather-icon>
                     {{ $t('Delete') }}
                  </b-button>
               </b-col>
            </b-row>
            <h2 v-if="allSigner.length == 2">
               {{ $t('Agreed') }}
            </h2>
            <b-row v-if="!isView">
               <b-col>
                  <b-button @click="addNewEmptyRow" size="sm" variant="outline-primary">
                     <feather-icon icon="PlusIcon"></feather-icon>
                     {{ $t('Add') }}
                  </b-button>
               </b-col>
            </b-row>
         </b-col>
      </b-row>

      <!-- actions -->
      <b-row class="text-center">
         <b-col class="text-right">
            <b-button @click="$emit('back')" size="sm" variant="outline-danger">
               <feather-icon icon="ArrowLeftIcon"></feather-icon>
               {{ $t('back') }}
            </b-button>
         </b-col>
         <b-col class="text-left">
            <b-button @click="Continue" size="sm" variant="outline-primary">
               <feather-icon icon="ArrowRightIcon"></feather-icon>
               {{ $t('Continue') }}
            </b-button>
         </b-col>
      </b-row>
   </validation-observer>
</template>

<script>
import { BRow, BCol, BButton } from 'bootstrap-vue';
import StaffingService from '@/services/hrm/staffing.service';
import DepartmentService from '@/services/info/department.service';
import { DEPARTMENT_CODE } from '@/constants/department';
import HrmEmployeeManageSelect2 from '@/views/components/hrm/HrmEmployeeManageSelect2.vue';
import EmployeeManageService from '@/services/hrm/employeemanage.service';

const signerDef = {
   id: 0,
   signOrder: null,
   departmentId: null,
   positionId: null,
   employeeManageId: null,
   department: '',
   position: '',
   employee: '',
   isHr: false,
   isDirector: false
};

function customSort(a, b) {
   // Compare based on isDirector property
   const directorComparison = b.isDirector - a.isDirector;

   // If isDirector is the same, compare based on isHr property
   if (directorComparison === 0) {
      return b.isHr - a.isHr;
   }

   return directorComparison;
}

export default {
   components: {
      BRow,
      BCol,
      BButton,
      HrmEmployeeManageSelect2
   },
   props: {
      signer: {
         type: Array,
         default: () => []
      },
      organizationId: {
         type: Number,
         default: null
      },
      isView: {
         type: Boolean,
         default: false
      }
   },
   emits: ['update:signer', 'back', 'continue'],
   data() {
      return {
         DEPARTMENT_CODE: DEPARTMENT_CODE,
         DepartmentList: [],
         PositionList: [],
         disabledEmployeManageIds: []
      };
   },
   computed: {
      mainOrganizationId() {
         const authOrgId = JSON.parse(localStorage.getItem('user_info') || '{}').organizationId;
         return authOrgId == 1 ? authOrgId : this.organizationId;
      },
      signerHr() {
         return this.signer.find((e) => e.isHr);
      },
      signerDirector() {
         return this.signer.find((e) => e.isDirector);
      },
      allSigner: {
         get() {
            const sList = this.signer;
            // if (sList.length === 0) {
            //    sList.push({ ...signerDef });
            // }
            if (!this.signerHr) {
               sList.push({ ...signerDef, isHr: true });
            }
            if (!this.signerDirector) {
               sList.push({ ...signerDef, isDirector: true });
            }

            sList.sort(customSort);
            return sList;
         },
         set(newSignerList) {
            this.$emit('update:signer', newSignerList);
         }
      },
      PositionListFilter() {
         return (departmentId) => {
            return departmentId ? this.PositionList.filter((e) => e.departmentId == departmentId) : this.PositionList;
         };
      },
      DepartmentListFilter() {
         return (code) => {
            return code
               ? this.DepartmentList.filter((e) => (Array.isArray(code) ? code.includes(e.code) : e.code == code))
               : this.DepartmentList;
         };
      }
   },
   created() {
      StaffingService.GetStaffingPositionClassification(null, null, null, this.mainOrganizationId).then((res) => {
         this.PositionList = res.data;
      });

      DepartmentService.GetAsSelectList(this.mainOrganizationId, {})
         .then((res) => {
            if (Array.isArray(res.data)) {
               this.DepartmentList = res.data;
            }
         })
         .catch((error) => {
            this.showApiError(error);
         });

      EmployeeManageService.CheckSigners().then((res) => {
         const { employeeBusinessTripIds = [], employeeLeaveOrderIds = [], employeeSickLeaveIds = [] } = res.data;
         this.disabledEmployeManageIds = [
            ...employeeBusinessTripIds,
            ...employeeLeaveOrderIds,
            ...employeeSickLeaveIds
         ];
      });
   },
   methods: {
      addNewEmptyRow() {
         this.allSigner.push(JSON.parse(JSON.stringify({ ...signerDef, signOrder: this.allSigner.length + 1 })));
      },
      deleteSigner(index) {
         this.allSigner.splice(index, 1);
      },
      Continue() {
         this.$refs.ValidationHrmSigner.validateWithInfo().then(({ isValid, errors }) => {
            if (isValid) {
               this.$emit('continue');
            } else {
               this.showValidateError(errors);
            }
         });
      }
   }
};
</script>

<style lang="scss" scoped></style>
