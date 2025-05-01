<template>
   <div>
      <b-row>
         <b-col>
            <b-card>
               <!-- <div class="form-group form-row mb-0">
            <label class="col-form-label col-sm-5 d-flex align-items-center" for>{{
                            $t("shortname")
                        }}</label>
                        <div class="col-sm-7">
                            <b-form-input v-model="Data.ShortName"></b-form-input>
            </div>
          </div>-->

               <div class="form-group">
                  <label class="col-form-label" for>{{ $t('shortname') }}</label>
                  <div>
                     <b-input-group>
                        <b-form-input :placeholder="$t('shortname')" v-model="Data.shortName" />
                        <b-input-group-append>
                           <b-button variant="primary" @click="OpenTranslateModal('short_name')">
                              <feather-icon icon="GlobeIcon"></feather-icon>
                           </b-button>
                        </b-input-group-append>
                     </b-input-group>
                  </div>
               </div>

               <!-- <div class="form-group form-row mb-0">
            <label class="col-form-label col-sm-5 d-flex align-items-center" for>
              {{
              $t("fullname")
              }}
            </label>
            <div class="col-sm-7">
              <b-form-input v-model="Data.FullName"></b-form-input>
            </div>
          </div>-->

               <div class="form-group">
                  <label class="col-form-label" for>{{ $t('fullname') }}</label>
                  <div>
                     <b-input-group>
                        <b-form-input :placeholder="$t('fullname')" v-model="Data.fullName" />
                        <b-input-group-append>
                           <b-button variant="primary" @click="OpenTranslateModal('full_name')">
                              <feather-icon icon="GlobeIcon"></feather-icon>
                           </b-button>
                        </b-input-group-append>
                     </b-input-group>
                  </div>
               </div>
            </b-card>
         </b-col>
         <b-col>
            <b-card>
               <div class="form-group form-row mb-0">
                  <label class="col-form-label col-sm-5 d-flex align-items-center" for>{{ $t('isdefault') }}</label>
                  <div class="col-sm-7">
                     <b-form-checkbox v-model="Data.isDefault" name="check-button1" switch></b-form-checkbox>
                  </div>
               </div>
               <div class="form-group form-row mb-0">
                  <label class="col-form-label col-sm-5 d-flex align-items-center" for>
                     {{ $t('isadmin') }}
                  </label>
                  <div class="col-sm-7">
                     <b-form-checkbox v-model="Data.isTopInspector" name="check-button" switch></b-form-checkbox>
                  </div>
               </div>
               <!-- <div class="form-group form-row mb-0">
            <label class="col-form-label col-sm-5 d-flex align-items-center" for>
              {{
              $t("isHr")
              }}
            </label>
            <div class="col-sm-7">
              <b-form-checkbox v-model="Data.isHr" name="check-button" switch></b-form-checkbox>
            </div>
          </div>-->
            </b-card>
         </b-col>
      </b-row>
      <b-card>
         <b-card-body>
            <b-row>
               <b-col sm="12" md="12">
                  <div
                     v-for="(value, index) in moduleList"
                     class="row"
                     :key="index"
                     style="padding: 3px; border: solid 1px #a4a8ab; margin-top: 25px"
                  >
                     <div class="form-check col-sm-12" style="margin-top: -15px">
                        <mark style="background-color: #f5f5f5"></mark>

                        <span class="form-check-label" style="color: green">
                           <mark style="background-color: #f5f5f5">
                              <feather-icon icon="CheckIcon" style="margin-right: 5px"></feather-icon>
                              {{ value.fullName }}
                           </mark>
                        </span>
                     </div>
                     <div
                        v-for="(item, index1) in value.subGroups"
                        class="row w-100"
                        :key="index1"
                        style="padding: 3px; margin-top: 25px"
                     >
                        <div class="form-check col-sm-12 col-md-12" style="margin-top: -15px">
                           <mark style="background-color: #f5f5f5"></mark>

                           <span class="form-check-label" style="color: green">
                              <mark style="background-color: #f5f5f5">
                                 <feather-icon icon="CheckIcon" style="margin-right: 5px"></feather-icon>
                                 {{ item.fullName }} <span class="text-primary">({{ item.code }})</span>
                              </mark>
                           </span>
                        </div>
                        <div
                           v-for="(child, el) in item.modules"
                           class="col-sm-6 col-md-2 pl-5 pt-1"
                           :key="child.id + el"
                        >
                           <input
                              :id="'child' + child.id"
                              class="form-check-input"
                              type="checkbox"
                              :value="child.id"
                              v-model="Data.modules"
                              @change="onChangeCheckbox(child.id)"
                           />

                           <label style="margin-left: 25px; color: #000" :for="'child' + child.id">{{
                              child.fullName
                           }}</label>
                        </div>
                     </div>
                  </div>
               </b-col>
            </b-row>
            <b-row>
               <b-col>
                  <b-button variant="success" :disabled="loadingButton" @click="saveRole()" class="float-right mt-5">
                     <b-spinner small v-if="loadingButton"></b-spinner>
                     {{ $route.params.id > 0 ? $t('Save') : $t('create') }}
                  </b-button>
               </b-col>
            </b-row>

            <b-modal size="lg" :title="$t('Translates')" v-model="TranslateModal" hide-footer no-close-on-backdrop>
               <b-table :fields="TranslateFields" :items="TranslateItems" responsive="sm" striped bordered small>
                  <template v-slot:thead-top>
                     <b-tr>
                        <b-td>
                           <v-select
                              :options="LanguageList"
                              :reduce="(item) => item.value"
                              :placeholder="$t('ChooseBelow')"
                              label="text"
                              v-model="TranslateItem.languageId"
                              @input="ChangeTranslate"
                           ></v-select>
                        </b-td>
                        <b-td>
                           <b-form-input :placeholder="$t('fullname')" v-model="TranslateItem.translateText" />
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
                     <b-button @click="TranslateModal = false" variant="success">{{ $t('Save') }}</b-button>
                  </b-col>
               </b-row>
            </b-modal>
         </b-card-body>
      </b-card>
   </div>
</template>
<script>
import ManualService from '@/services/others/manual.service';
import RoleService from '@/services/managment/role.service';
import {
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
   VBTooltip,
   BModal,
   VBModal,
   BCardText,
   BInputGroup,
   BInputGroupAppend,
   BTr,
   BTd,
   BFormCheckbox,
   BSpinner
} from 'bootstrap-vue';

export default {
   components: {
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
      VBTooltip,
      BModal,
      VBModal,
      BCardText,
      BInputGroup,
      BInputGroupAppend,
      BTr,
      BTd,
      BFormCheckbox,
      BSpinner
   },
   name: 'Edit',
   data() {
      return {
         lang: '',
         moduleList: [],
         loadingButton: false,
         Data: {
            translates: []
         },
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
         TranslateItem: {
            language: '',
            languageId: 0,
            columnName: '',
            translateText: ''
         },
         TranslateItems: [],
         LanguageList: []
      };
   },
   methods: {
      OpenTranslateModal(columnname) {
         this.TranslateModal = true;
         this.GetTranslateItems(columnname);
         this.clearLangTable(columnname);
      },
      GetTranslateItems(columnname) {
         this.TranslateItems = this.Data.translates.filter((item) => item.columnName == columnname);
      },
      ChangeTranslate() {
         this.TranslateItem.language = !!this.TranslateItem.languageId
            ? this.LanguageList.filter((item) => item.value == this.TranslateItem.languageId)[0].text
            : '';
         console.log('LLLL', this.TranslateItem.language);
      },
      clearLangTable(columnname) {
         this.TranslateItem = {
            language: '',
            languageId: 0,
            columnName: columnname,
            translateText: ''
         };
      },
      AddTranslate() {
         if (
            this.TranslateItem.languageId === null ||
            this.TranslateItem.languageId === undefined ||
            this.TranslateItem.languageId === 0 ||
            this.TranslateItem.languageId === ''
         ) {
            this.makeToast(this.$t('notSelectLang'), 'danger');
            return false;
         }
         if (
            this.Data.translates.filter(
               (item) =>
                  item.languageId === this.TranslateItem.languageId && item.columnName == this.TranslateItem.columnName
            ).length > 0
         ) {
            this.makeToast(this.$t('AlreadySelectLang'), 'danger');
            return false;
         }
         this.Data.translates.push(this.TranslateItem);
         this.GetTranslateItems(this.TranslateItem.columnName);
         this.clearLangTable(this.TranslateItem.columnName);
      },
      backToList() {
         this.$router.push({ name: 'role' });
      },
      onChangeCheckbox(id) {
         console.log();
      },
      checkValid() {
         let valid = false;
         if (this.Data.shortName !== '' && this.Data.shortName !== null) {
            valid = true;
         } else {
            valid = false;
         }
         return valid;
      },
      saveRole() {
         this.loadingButton = true;
         const vm = this;
         if (this.checkValid()) {
            try {
               RoleService.Update(this.Data).then((response) => {
                  vm.loadingButton = false;

                  if (this.$route.params.id) {
                     vm.makeToast(vm.$t('SuccessSave'), 'success');
                  } else {
                     vm.makeToast(vm.$t('SuccessSave'), 'success');
                  }
                  setTimeout(() => {
                     vm.$router.push({ name: 'role' });
                  }, 500);
               });
            } catch (e) {
               this.loadingButton = false;
               const response = JSON.parse(e.request.response);
               this.makeToast(response.error, 'danger');
            }
         } else {
            this.loadingButton = false;
            this.makeToast(this.$t('messages.all_field_required'), 'danger');
         }
      },
      getModules() {
         ManualService.GetModuleSelectList(this.lang).then((response) => {
            this.moduleList = response.data;
         });
      }
   },
   created() {
      this.lang = localStorage.getItem('locale') || 'ru';
      RoleService.Get(this.$route.params.id).then((res) => {
         this.Data = res.data;
      });
      ManualService.LanguageSelectList()
         .then((res) => {
            this.LanguageList = res.data;
         })
         .catch((error) => {
            this.showApiError(error);
         });
      this.getModules();
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
