<template>
   <div>
      <form-input :value="value" @input="e=>$emit('input',e)" v-bind="$attrs">
         <b-input-group-append>
            <b-button variant="primary" @click="OpenTranslateModal">
               <feather-icon icon="GlobeIcon"></feather-icon>
            </b-button>
         </b-input-group-append>
      </form-input>

      <!-- modal -->
      <b-modal size="lg" :title="$t('Translates')" v-model="TranslateModal" hide-footer no-close-on-backdrop>
         <b-table :fields="TranslateFields" :items="TranslateItems" responsive="sm" striped bordered small>
            <template v-slot:thead-top>
               <b-tr>
                  <b-td>
                     <form-select :options="LanguageList" v-model="TranslateItem.languageId" @input="ChangeTranslate"></form-select>
                  </b-td>
                  <b-td>
                     <b-form-textarea :placeholder="$t('fullname')" v-model="TranslateItem.translateText" />
                  </b-td>
                  <b-td class="text-center">
                     <b-button variant="primary" @click="AddTranslate">
                        <feather-icon icon="PlusIcon"></feather-icon>
                     </b-button>
                  </b-td>
               </b-tr>
            </template>
            <template #cell(translateText)="{ item }">
               <b-form-input :placeholder="$t('fullname')" v-model="item.translateText" />
            </template>
         </b-table>
         <b-row>
            <b-col class="text-right">
               <b-button @click="handleSave" variant="success">
                  {{ $t('Save') }}
               </b-button>
            </b-col>
         </b-row>
      </b-modal>
   </div>
</template>

<script>
import { BInputGroupAppend,BFormTextarea, BCard, BRow, BCol, BFormInput, BTable, BButton, BModal, BTr, BTd } from 'bootstrap-vue';
import ManualService from '@/services/others/manual.service';

const defaultItem = {
   language: '',
   languageId: 0,
   columnName: '',
   translateText: ''
};
export default {
   name: 'FormInputTranslate',
   components: {
      BInputGroupAppend,
      BButton,
      BCard,
      BRow,
      BFormTextarea,
      BCol,
      BFormInput,
      BButton,
      BTable,
      BModal,
      BTr,
      BTd
   },
   props: {
      columnName: {
         type: String,
         default: ''
      },
      value: {
         type: String,
         default: ''
      },
      translates: {
         type: Array,
         default: []
      }
   },
   watch: {
      translates: {
         handler(newVal) {
            this.TranslateItems = newVal.filter((item) => item.columnName == this.columnName);
         }
      }
   },
   data() {
      return {
         TranslateModal: false,
         TranslateFields: [
            {
               key: 'language',
               label: this.$t('languagename'),
               class: 'text-center'
            },
            {
               key: 'translateText',
               label: this.$t('translatetext'),
               class: 'text-center'
            },
            { key: 'actions', label: this.$t('actions'), thClass: 'text-center' }
         ],
         TranslateItems: [],
         TranslateItem: { ...defaultItem },
         LanguageList: []
      };
   },
   methods: {
      OpenTranslateModal() {
         this.ResetItem();
         this.TranslateModal = true;
         ManualService.LanguageSelectList()
            .then((res) => {
               this.LanguageList = res.data;
            })
            .catch((error) => {
               this.showApiError(error);
            });
      },
      ChangeTranslate() {
         this.TranslateItem.language = !!this.TranslateItem.languageId ? this.LanguageList.filter((item) => item.value == this.TranslateItem.languageId)[0].text : '';
      },
      AddTranslate() {
         if (!this.TranslateItem.languageId) {
            this.makeToast(this.$t('notSelectLang'), 'danger');
            return false;
         }
         if (this.TranslateItems.filter((item) => item.languageId === this.TranslateItem.languageId && item.columnName == this.TranslateItem.columnName).length > 0) {
            this.makeToast(this.$t('AlreadySelectLang'), 'danger');
            return false;
         }
         this.TranslateItems.push({ ...this.TranslateItem });
         this.ResetItem();
      },
      ResetItem() {
         this.TranslateItem = { ...defaultItem, columnName: this.columnName };
      },
      handleSave() {
         this.TranslateModal = false;
         this.$emit('update:translates', [...this.translates.filter((e) => e.columnName != this.columnName), ...this.TranslateItems]);
      }
   }
};
</script>
