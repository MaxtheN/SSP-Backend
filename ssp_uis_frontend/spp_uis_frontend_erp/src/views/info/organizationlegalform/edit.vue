<template>
  <b-overlay :show="show">
    <b-row>
      <b-col sm="12" md="12" lg="12">
        <b-card>
          <validation-observer ref="ValidationDTO">
            <b-row>
              <b-col sm="12" md="3">
                <div class="form-group">
                  <form-input v-model="Data.orderCode" :label="$t('code')" />
                </div>
              </b-col>

              <b-col sm="12" md="3">
                <div class="form-group">
                  <form-input v-model="Data.code" :label="$t('kode')" />
                </div>
              </b-col>
              <b-col sm="12" md="3">
                <div class="form-group">
                  <form-input v-model="Data.shortName" required :label="$t('shortname')">
                    <b-input-group-append>
                      <b-button variant="primary" @click="OpenTranslateModal('short_name')">
                        <feather-icon icon="GlobeIcon"></feather-icon>
                      </b-button>
                    </b-input-group-append>
                  </form-input>
                </div>
              </b-col>
              <b-col sm="12" md="3">
                <div class="form-group">
                  <form-input v-model="Data.fullName" required :label="$t('fullname')">
                    <b-input-group-append>
                      <b-button variant="primary" @click="OpenTranslateModal('full_name')">
                        <feather-icon icon="GlobeIcon"></feather-icon>
                      </b-button>
                    </b-input-group-append>
                  </form-input>
                </div>
              </b-col>

              <b-col sm="12" md="3">
                <form-select
                  :options="StateList"
                  v-model="Data.stateId"
                  required-star
                  :label="$t('Status')"
                ></form-select>
              </b-col>
            </b-row>
            <b-row class="mt-3">
              <b-col sm="12" md="6" lg="6" class="text-left"></b-col>
              <b-col sm="12" md="6" lg="6" class="text-right">
                <b-button
                  :disabled="saveLoading"
                  @click="SaveData"
                  size="sm"
                  variant="outline-success"
                >
                  <feather-icon icon="CheckIcon"></feather-icon>
                  {{ $t("Save") }}
                </b-button>
              </b-col>
            </b-row>
          </validation-observer>
        </b-card>
      </b-col>
    </b-row>

    <b-modal
      size="lg"
      :title="$t('Translates')"
      v-model="TranslateModal"
      hide-footer
      no-close-on-backdrop
    >
      <b-table
        :fields="TranslateFields"
        :items="TranslateItems"
        responsive="sm"
        striped
        bordered
        small
      >
        <template v-slot:thead-top>
          <b-tr>
            <b-td>
              <form-select
                :options="LanguageList"
                v-model="TranslateItem.languageId"
                @input="ChangeTranslate"
              ></form-select>
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
          <b-button @click="TranslateModal = false" variant="success">
            {{
            $t("Save")
            }}
          </b-button>
        </b-col>
      </b-row>
    </b-modal>
  </b-overlay>
</template>
<script>
import ManualService from "@/services/others/manual.service";
import OrganizationLegalFormService from "@/services/info/organizationlegalform.service";
import { ValidationObserver } from "vee-validate";

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
  BTr
} from "bootstrap-vue";
import PrtnContractTypeService from "@/services/info/prtncontracttype.service";

// import FormSelect from "@/components/forms/form-select.vue";
// import FormInput from "@/components/forms/form-input.vue";

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
    BTr
  },
  name: "Edit",

  data() {
    return {
      lang: "",
      show: false,
      StateList: [],
      loadingButton: false,
      saveLoading: false,
      Data: {
        translates: []
      },

      TranslateModal: false,
      TranslateFields: [
        {
          key: "language",
          label: this.$t("languagename"),
          class: "text-center"
        },
        {
          key: "translateText",
          label: this.$t("translatetext"),
          class: "text-center"
        },
        { key: "actions", label: this.$t("actions"), thClass: "text-center" }
      ],
      TranslateItem: {
        language: "",
        languageId: 0,
        columnName: "",
        translateText: ""
      },
      TranslateItems: [],
      LanguageList: []
    };
  },

  created() {
    this.lang = localStorage.getItem("locale") || "ru";
    this.show = true;
    OrganizationLegalFormService.Get(this.$route.params.id).then(res => {
      this.show = false;
      this.Data = res.data;
    });

    ManualService.LanguageSelectList()
      .then(res => {
        this.LanguageList = res.data;
      })
      .catch(error => {
        this.makeToast(error.response.data.error, "danger");
      });

    ManualService.StateSelectList()
      .then(res => {
        this.StateList = res.data;
      })
      .catch(error => {
        this.makeToast(error.response.data.error, "danger");
      });
    PrtnContractTypeService.GetAsSelectList()
      .then(res => {
        this.ContractList = res.data;
      })
      .catch(error => {
        this.makeToast(error.response.data.error, "danger");
      });
  },
  methods: {
    OpenTranslateModal(columnname) {
      this.TranslateModal = true;
      this.GetTranslateItems(columnname);
      this.clearLangTable(columnname);
    },
    GetTranslateItems(columnname) {
      this.TranslateItems = this.Data.translates.filter(
        item => item.columnName == columnname
      );
    },
    ChangeTranslate() {
      this.TranslateItem.language = !!this.TranslateItem.languageId
        ? this.LanguageList.filter(
            item => item.value == this.TranslateItem.languageId
          )[0].text
        : "";
    },
    clearLangTable(columnname) {
      this.TranslateItem = {
        language: "",
        languageId: 0,
        columnName: columnname,
        translateText: ""
      };
    },

    AddTranslate() {
      if (
        this.TranslateItem.languageId === null ||
        this.TranslateItem.languageId === undefined ||
        this.TranslateItem.languageId === 0 ||
        this.TranslateItem.languageId === ""
      ) {
        this.makeToast(this.$t("notSelectLang"), "danger");
        return false;
      }
      if (
        this.Data.translates.filter(
          item =>
            item.languageId === this.TranslateItem.languageId &&
            item.columnName == this.TranslateItem.columnName
        ).length > 0
      ) {
        this.makeToast(this.$t("AlreadySelectLang"), "danger");
        return false;
      }
      this.Data.translates.push(this.TranslateItem);
      this.GetTranslateItems(this.TranslateItem.columnName);
      this.clearLangTable(this.TranslateItem.columnName);
    },
    SaveData() {
      this.$refs.ValidationDTO.validate().then(success => {
        if (success) {
          this.saveLoading = true;

          OrganizationLegalFormService.Update(this.Data)
            .then(res => {
              this.makeToast(this.$t("SaveSuccess"), "success");
              this.$router.push({ name: "OrganizationLegalForm" });
              this.saveLoading = false;
            })
            .catch(err => {
              this.makeToast(this.$t(err), "danger");
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
  font: 1rem "Fira Sans", sans-serif;
}

input {
  margin: 0.4rem;
}
</style>
@/services/info/position.service