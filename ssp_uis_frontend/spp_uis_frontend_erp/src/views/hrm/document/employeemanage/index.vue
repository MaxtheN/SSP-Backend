<template>
   <div>
      <EmployeeManageList
         :selectable="false"
         :is-organization="localData.organizationId == 1 ? true : false"
         :organization-id="localData.organizationId"
      />
   </div>
</template>

<script>
import EmployeeManageList from '@/views/components/hrm/EmployeeManageList.vue';
export default {
   components: {
      EmployeeManageList
   },
   data() {
      return {
         localData: JSON.parse(localStorage.getItem('user_info'))
      };
   }
};
</script>
