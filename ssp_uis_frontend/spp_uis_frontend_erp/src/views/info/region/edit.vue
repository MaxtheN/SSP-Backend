<template>
  <div>
    <b-card>
      <b-row>
        <b-col sm="12" md="3" class="text-left mt-2">
          <div class="form-group">
            <label class="col-form-label" for>{{ $t("code") }}</label>
            <div>
              <b-form-input v-model="Data.orderCode" type="number" :placeholder="$t('code')"></b-form-input>
            </div>
          </div>
        </b-col>

        <b-col sm="12" md="4" class="text-left mt-2">
          <div class="form-group">
            <label class="col-form-label" for>{{ $t("shortname") }}</label>
            <div>
              <b-input-group>
                <b-form-input :placeholder="$t('shortname')" disabled v-model="Data.shortName" />
                <b-input-group-append>
                  <b-button disabled variant="primary" @click="OpenTranslateModal('short_name')">
                    <feather-icon icon="GlobeIcon"></feather-icon>
                  </b-button>
                </b-input-group-append>
              </b-input-group>
            </div>
          </div>
        </b-col>

        <b-col sm="12" md="4" class="text-left mt-2">
          <div class="form-group">
            <label class="col-form-label" for>{{ $t("fullname") }}</label>
            <div>
              <b-input-group>
                <b-form-input disabled :placeholder="$t('fullname')" v-model="Data.fullName" />
                <b-input-group-append>
                  <b-button disabled variant="primary" @click="OpenTranslateModal('full_name')">
                    <feather-icon icon="GlobeIcon"></feather-icon>
                  </b-button>
                </b-input-group-append>
              </b-input-group>
            </div>
          </div>
        </b-col>
      </b-row>
      <b-row class="mt-3">
        <b-col sm="12" md="6" lg="6" class="text-left"></b-col>
        <b-col sm="12" md="6" lg="6" class="text-right">
          <b-button @click="SaveData" size="sm" variant="outline-success">
            <feather-icon icon="CheckIcon"></feather-icon>
            {{ $t("Save") }}
          </b-button>
        </b-col>
      </b-row>
    </b-card>
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
          <b-button @click="TranslateModal = false" variant="success">{{ $t("Save") }}</b-button>
        </b-col>
      </b-row>
    </b-modal>
  </div>
</template>
<script>
import ManualService from "@/services/others/manual.service";
import RegionService from "@/services/info/region.service";

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
  BFormCheckbox
} from "bootstrap-vue";

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
    BFormCheckbox
  },
  name: "CreateOrUpdateRole",

  data() {
    return {
      lang: "",
      loadingButton: false,
      Data: {
        translates: [],
        person: {}
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
    RegionService.Get(this.$route.params.id).then(res => {
      this.Data = res.data;
    });

    ManualService.LanguageSelectList()
      .then(res => {
        this.LanguageList = res.data;
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
    backToList() {
      this.$router.push({ name: "role" });
    },
    checkValid() {
      let valid = false;
      if (this.Data.shortName !== "" && this.Data.shortName !== null) {
        valid = true;
      } else {
        valid = false;
      }
      return valid;
    },
    SaveData() {
      this.loadingButton = true;
      if (this.checkValid()) {
        RegionService.Update(this.Data)
          .then(res => {
            this.makeToast(this.$t("SaveSuccess"), "success");
            this.$router.push({ name: "Region" });
          })
          .catch(err => {
            this.makeToast(this.$t(err), "danger");
          });
      }
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