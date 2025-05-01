<template>
   <b-overlay :show="show">
      <b-card>
         <validation-observer ref="ValidationDTO">
            <b-row>
               <b-col sm="12" md="4">
                  <div class="form-group">
                     <form-input v-model="Data.docNumber" required :label="$t('docnumber')" />
                  </div>
               </b-col>
               <b-col sm="12" md="4">
                  <form-picker v-model="Data.docOn" required :label="$t('docOn')" :placeholder="$t('docOn')" />
               </b-col>
               <b-col sm="12" md="4">
                  <form-input v-model="Data.details" :placeholder="$t('details')" :label="$t('details')"></form-input>
               </b-col>
            </b-row>
         </validation-observer>
      </b-card>
      <b-card v-if="Data.groups.length == 0">
         <b-button variant="outline-primary" @click="addGroup">{{ $t('AddGroup') }}</b-button>
      </b-card>

      <Group
         v-for="(group, groupIndex) in Data.groups"
         :group="group"
         :key="groupIndex"
         :index="groupIndex"
         :needChamberServiceGroupList="needChamberServiceGroupList"
         @update:group="(e) => (group = e)"
         @addGroup="addGroup"
      />

      <b-row>
         <b-col sm="12" md="6" lg="6" class="text-left"></b-col>
         <b-col sm="12" md="6" lg="6" class="text-right">
            <b-button :disabled="saveLoading" @click="SaveData" size="sm" variant="outline-success">
               <feather-icon icon="CheckIcon"></feather-icon>
               {{ $t('Save') }}
            </b-button>
         </b-col>
      </b-row>
   </b-overlay>
</template>
<script>
// service
import ServicePriceService from '@/services/srv/ServicePrice.service';
import NeedChamberServiceGroupService from '@/services/srv/needchamberservicegroup.service';

// components
import {
   BOverlay,
   BCard,
   BRow,
   BCol,
   BButton,
   BFormGroup,
   BFormTextarea,
   BTable,
   BCardHeader,
   BCardBody
} from 'bootstrap-vue';
import Group from './components/Group.vue';

const defaultGroupRow = {
   id: 0,
   groupId: 0,
   tables: []
};

export default {
   components: {
      BOverlay,
      BCard,
      BRow,
      BCol,
      BButton,
      BFormGroup,
      BFormTextarea,
      BTable,
      BCardHeader,
      BCardBody,
      Group
   },
   name: 'Edit',
   data() {
      return {
         show: false,
         saveLoading: false,
         defaultGroupRow,
         needChamberServiceGroupList: [],
         Data: {
            id: 0,
            groups: [],
            statusId: 1,
            status: null,
            organizationId: 0,
            organization: null,
            createdAt: '31.10.2023 12:26:53',
            docNumber: null,
            docOn: '31.10.2023',
            details: null
         }
      };
   },
   created() {
      this.show = true;
      ServicePriceService.Get(this.$route.params.id)
         .then((res) => {
            this.Data = res.data;
         })
         .catch((error) => {
            this.showApiError(error);
         })
         .finally(() => {
            this.show = false;
         });

      NeedChamberServiceGroupService.GetAsSelectList()
         .then((res) => {
            this.needChamberServiceGroupList = res.data;
         })
         .finally(() => {
            this.isBusy = false;
         });
   },
   methods: {
      addGroup() {
         this.Data.groups.push(JSON.parse(JSON.stringify(defaultGroupRow)));
      },
      SaveData() {
         this.$refs.ValidationDTO.validate().then((success) => {
            if (success) {
               this.saveLoading = true;
               ServicePriceService.Update(this.Data)
                  .then(() => {
                     this.makeToast(this.$t('SaveSuccess'), 'success');
                     this.$router.push({ name: 'ServicePrice' });
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
