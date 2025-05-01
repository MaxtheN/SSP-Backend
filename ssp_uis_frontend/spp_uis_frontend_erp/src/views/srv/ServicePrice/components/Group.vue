<template>
   <b-card no-body>
      <validation-observer :ref="'ValidationTabrow' + index">
         <b-card-header class="d-flex py-0 pt-1 full-width align-items-center bg-light">
            <form-select
               style="width: 50%"
               v-model="groupComp.groupId"
               :options="needChamberServiceGroupList"
               required-star
               label="NeedChamberServiceGroup"
               :loading="needChamberServiceListLoading"
               @input="groupComp.tables = []"
            />

            <b-button variant="outline-primary" @click="addGroup">{{ $t('AddGroup') }}</b-button>
         </b-card-header>
         <b-card-body class="mt-2">
            <b-row>
               <b-col sm="12" md="4">
                  <form-select
                     v-model="tabrow.needChamberServiceId"
                     :options="needChamberServiceList"
                     required-star
                     label="NeedChamberService"
                  />
               </b-col>
               <b-col sm="12" md="2">
                  <form-currency-input
                     v-model="tabrow.beginCoef"
                     :label="$t('beginCoef')"
                     :placeholder="$t('beginCoef')"
                  />
               </b-col>
               <b-col sm="12" md="2">
                  <form-currency-input v-model="tabrow.endCoef" :label="$t('endCoef')" :placeholder="$t('endCoef')" />
               </b-col>
               <b-col sm="12" md="2">
                  <form-currency-input
                     v-model="tabrow.concreteCoef"
                     :label="$t('concreteCoef')"
                     :placeholder="$t('concreteCoef')"
                  />
               </b-col>
               <b-col sm="12" md="1" class="mt-2">
                  <b-button variant="primary" @click="AddTabrow()">
                     <feather-icon icon="PlusIcon" size="14"></feather-icon>
                  </b-button>
               </b-col>
            </b-row>
         </b-card-body>
      </validation-observer>
      <div class="simple-table">
         <b-table
            class="mx-2"
            :fields="TablesField"
            hover
            bordered
            show-empty
            :empty-text="$t('NotFound')"
            small
            responsive="sm"
            :items="group.tables"
         >
            <template #cell(actions)="{ item, index }">
               <div class="text-center">
                  <feather-icon style="margin-right: 5px" @click="EditTabrow(item)" icon="EditIcon"></feather-icon>
                  <feather-icon class="text-danger" @click="DeleteTabrow(index)" icon="Trash2Icon"></feather-icon>
               </div>
            </template>
            <template #cell(order)="{ index }">
               <span>{{ index + 1 }}</span>
            </template>
         </b-table>
      </div>
   </b-card>
</template>

<script>
import NeedChamberServiceService from '@/services/hrm/needchamberservice.service';
import { BCard, BRow, BCol, BButton, BTable, BCardHeader, BCardBody } from 'bootstrap-vue';

const defaultTableRow = {
   id: 0,
   needChamberServiceId: 0,
   needChamberService: '',
   beginCoef: 0,
   endCoef: 0,
   concreteCoef: 0
};

export default {
   components: {
      BCard,
      BRow,
      BCol,
      BButton,
      BTable,
      BCardHeader,
      BCardBody
   },
   props: {
      group: {
         type: Object,
         default: () => ({}),
         required: true
      },
      needChamberServiceGroupList: {
         type: Array,
         default: () => []
      },
      index: {
         type: Number,
         default: 0
      }
   },
   emits: ['addGroup', 'update:group'],
   data() {
      return {
         editedIndex1: -1,
         tabrow: { ...defaultTableRow },
         needChamberServiceList: [],
         needChamberServiceListLoading: false,
         TablesField: [
            {
               key: 'needChamberService',
               label: this.$t('NeedChamberService'),
               sortable: true
            },
            {
               key: 'beginCoef',
               label: this.$t('beginCoef'),
               sortable: true
            },
            {
               key: 'endCoef',
               label: this.$t('endCoef'),
               sortable: true
            },
            {
               key: 'concreteCoef',
               label: this.$t('concreteCoef'),
               sortable: true
            },
            {
               key: 'actions',
               label: this.$t('actions'),
               thClass: 'text-center',
               tdClass: 'text-center',
               sortable: true
            }
         ]
      };
   },
   computed: {
      groupComp: {
         get() {
            return this.group;
         },
         set(v) {
            console.log(v, 'save');
            this.$emits('update:group', v);
         }
      }
   },
   watch: {
      'group.groupId': {
         handler(e) {
            if (e) {
               this.getNeedchamberServiceList(e);
            } else {
               this.needChamberServiceList = [];
            }
         }
      }
   },
   methods: {
      getNeedchamberServiceList(groupId) {
         if (groupId) {
            this.needChamberServiceList = [];
            this.needChamberServiceListLoading = true;
            NeedChamberServiceService.GetAsSelectList(groupId)
               .then((res) => {
                  this.needChamberServiceList = res.data;
               })
               .finally(() => {
                  this.needChamberServiceListLoading = false;
               });
         }
      },
      addGroup() {
         this.$emit('addGroup');
      },
      DeleteTabrow(index) {
         this.groupComp.tables.splice(index, 1);
      },
      EditTabrow(item) {
         this.editedIndex1 = this.groupComp.tables.indexOf(item);
         this.tabrow = JSON.parse(JSON.stringify(item));
      },
      AddTabrow() {
         this.$refs['ValidationTabrow' + this.index].validate().then((success) => {
            if (success) {
               this.tabrow.needChamberService = this.needChamberServiceList.find(
                  (e) => e.value == this.tabrow.needChamberServiceId
               )?.text;

               if (this.editedIndex1 > -1) {
                  Object.assign(this.groupComp.tables[this.editedIndex1], this.tabrow);
               } else {
                  this.groupComp.tables.push(this.tabrow);
               }
               this.$refs['ValidationTabrow' + this.index].reset();
               this.tabrow = { ...defaultTableRow };
            }
         });
      }
   }
};
</script>
<style>
.table.b-table > tbody .b-table-row-selected.table-active td {
   background-color: #8effc3 !important;
}
</style>
