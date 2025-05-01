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
                  <form-input
                    v-model="Data.shortName"
                    required
                    :label="$t('shortname')"
                  >
                    <b-input-group-append>
                      <b-button
                        variant="primary"
                        @click="OpenTranslateModal('short_name')"
                      >
                        <feather-icon icon="GlobeIcon"></feather-icon>
                      </b-button>
                    </b-input-group-append>
                  </form-input>
                </div>
              </b-col>
              <b-col sm="12" md="3">
                <div class="form-group">
                  <form-input
                    v-model="Data.fullName"
                    required
                    :label="$t('fullname')"
                  >
                    <b-input-group-append>
                      <b-button
                        variant="primary"
                        @click="OpenTranslateModal('full_name')"
                      >
                        <feather-icon icon="GlobeIcon"></feather-icon>
                      </b-button>
                    </b-input-group-append>
                  </form-input>
                </div>
              </b-col>
              <!-- <b-col sm="12" md="3">
                <div class="form-group">
                  <label class="col-form-label" for>{{ $t("fullname") }}</label>
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
              </b-col>-->

              <b-col sm="12" md="3">
                <div class="form-group">
                  <form-input
                    v-model="Data.employeeRangeFrom"
                    :label="$t('employeeRangeFrom')"
                  />
                </div>
              </b-col>

              <b-col sm="12" md="3">
                <div class="form-group">
                  <form-input
                    v-model="Data.employeeRangeTo"
                    :label="$t('employeeRangeTo')"
                  />
                </div>
              </b-col>

              <b-col sm="12" md="3">
                <div class="form-group">
                  <form-input
                    v-model="Data.certificatePeriodInYears"
                    :label="$t('certificatePeriodInYears')"
                    required
                    :mask="'####'"
                    type="number"
                  />
                </div>
              </b-col>

              <!-- <b-col sm="12" md="3">
                <div class="form-group">
                  <label class="col-form-label" for>
                    {{
                    $t("certificatePeriodInYears")
                    }}
                  </label>
                  <div>
                    <b-form-input
                      type="number"
                      v-mask="'####'"
                      :placeholder="$t('certificatePeriodInYears')"
                      v-model="Data.certificatePeriodInYears"
                    />
                  </div>
                </div>
              </b-col>-->
              <!-- <b-col sm="12" md="3">
              <div class="form-group">
                <label class="col-form-label" for>{{ $t("Status") }}</label>
                <div>
                  <v-select
                    :options="StateList"
                    :reduce="(item) => item.value"
                    :placeholder="$t('ChooseBelow')"
                    label="text"
                    v-model="Data.stateId"
                  ></v-select>
                </div>
              </div>
              </b-col >-->
              <b-col sm="12" md="3">
                <form-select
                  :options="StateList"
                  v-model="Data.stateId"
                  required-star
                  :label="$t('Status')"
                ></form-select>
              </b-col>
            </b-row>
            <b-row>
              <b-col class="text-right">
                <b-button
                  @click="OpenTabrow"
                  size="sm"
                  variant="outline-primary"
                >
                  <feather-icon icon="PlusIcon"></feather-icon>
                  {{ $t("Add") }}
                </b-button>
              </b-col>
            </b-row>
            <b-row class="mt-2">
              <b-col>
                <b-table
                  :fields="TablesField"
                  small
                  responsive="sm"
                  :tbody-tr-class="rowClass"
                  :items="Data.tables"
                >
                  <template #cell(status)="{ item }">
                    <b-badge
                      :variant="
                        item.stateId == '1' ? 'light-danger' : 'light-success'
                      "
                      >{{ item.state }}</b-badge
                    >
                  </template>
                  <template #cell(actions)="{ item, index }">
                    <div class="text-center">
                      <b-link>
                        <feather-icon
                          style="margin-right: 5px"
                          @click="EditTabrow(item)"
                          icon="EditIcon"
                        ></feather-icon>
                      </b-link>
                      <b-link>
                        <feather-icon
                          @click="DeleteTabrow(index)"
                          icon="Trash2Icon"
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
      v-model="TabrowModal"
      no-close-on-backdrop
      hide-footer
      :title="$t('Tables')"
    >
      <validation-observer ref="ValidationTabrow">
        <b-row>
          <b-col sm="12" md="12" class="mb-1">
            <form-input
              v-model="tabrow.orderNumber"
              required
              :label="$t('code')"
            />
          </b-col>
          <b-col sm="12" md="12" class="mb-1">
            <form-select
              :options="SignOrganizationTypeList"
              v-model="tabrow.signOrganizationTypeId"
              required-star
              :label="$t('signOrganizationType')"
            ></form-select>
          </b-col>
          <b-col sm="12" md="12" class="mb-1">
            <form-select
              :options="PositionList"
              v-model="tabrow.positionId"
              required-star
              :label="$t('position')"
            ></form-select>
          </b-col>
          <b-col
            sm="12"
            md="12"
            class="mb-1"
            v-if="tabrow.signOrganizationTypeId == 4"
          >
            <form-select
              :options="OrganizationList"
              v-model="tabrow.organizationId"
              required-star
              :label="$t('organization')"
            ></form-select>
          </b-col>
          <b-col
            sm="12"
            md="12"
            class="mb-1"
            v-if="tabrow.signOrganizationTypeId == 3"
          >
            <form-select
              :options="RegionList"
              v-model="tabrow.regionId"
              :label="$t('region')"
            ></form-select>
          </b-col>
          <b-col sm="12" md="12" class="mb-1">
            <form-select
              :options="StateList"
              v-model="tabrow.stateId"
              required-star
              :label="$t('Status')"
            ></form-select>
          </b-col>
        </b-row>
      </validation-observer>
      <b-row>
        <b-col class="text-center">
          <b-button
            variant="outline-danger"
            @click="TabrowModal = false"
            class="mr-1"
            size="sm"
          >
            <feather-icon icon="ArrowLeftIcon"></feather-icon>
            {{ $t("back") }}
          </b-button>
          <b-button variant="outline-success" @click="AddTabrow" size="sm">
            <feather-icon icon="CheckIcon"></feather-icon>
            {{ $t("Save") }}
          </b-button>
        </b-col>
      </b-row>
    </b-modal>

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
              <b-form-input
                :placeholder="$t('fullname')"
                v-model="TranslateItem.translateText"
              />
            </b-td>
            <b-td class="text-center">
              <b-button variant="primary" @click="AddTranslate">
                <feather-icon icon="PlusIcon"></feather-icon>
              </b-button>
            </b-td>
          </b-tr>
        </template>
        <template #cell(translateText)="{ item }">
          <b-form-input
            :placeholder="$t('fullname')"
            v-model="item.translateText"
          />
        </template>
      </b-table>
      <b-row>
        <b-col class="text-right">
          <b-button @click="TranslateModal = false" variant="success">
            {{ $t("Save") }}
          </b-button>
        </b-col>
      </b-row>
    </b-modal>
  </b-overlay>
</template>
<script>
import ManualService from "@/services/others/manual.service";
import OrganizationService from "@/services/managment/organization.service";
import PrtnContractTypeService from "@/services/info/prtncontracttype.service";
import PositionService from "@/services/info/position.service";
import RegionService from "@/services/info/region.service";

import {
  BOverlay,
  BCard,
  BCardBody,
  BRow,
  BCol,
  BFormInput,
  BTabs,
  BTable,
  BTab,
  BButton,
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
} from "bootstrap-vue";

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
  },
  name: "Edit",

  data() {
    return {
      lang: "",
      show: false,
      OrganizationList: [],
      PositionList: [],
      StateList: [],
      RegionList: [],
      loadingButton: false,
      TabrowModal: false,
      saveLoading: false,
      Data: {
        translates: [],
        person: {},
      },
      tabrow: {
        id: 0,
        orderNumber: 0,
        signOrganizationTypeId: 0,
        signOrganizationType: "",
        position: "",
        positionId: 0,
        state: "",
        stateId: 0,
        organizationId: 0,
        organization: "",
        regionId: 0,
        region: "",
      },
      SignOrganizationTypeList: [],
      TranslateModal: false,
      TranslateFields: [
        {
          key: "language",
          label: this.$t("languagename"),
          class: "text-center",
        },
        {
          key: "translateText",
          label: this.$t("translatetext"),
          class: "text-center",
        },
        { key: "actions", label: this.$t("actions"), thClass: "text-center" },
      ],
      TranslateItem: {
        language: "",
        languageId: 0,
        columnName: "",
        translateText: "",
      },
      TablesField: [
        {
          key: "order",
          label: this.$t("№"),
          sortable: true,
        },
        {
          key: "orderNumber",
          label: this.$t("code"),

          sortable: true,
        },

        {
          key: "signOrganizationType",
          label: this.$t("signOrganizationType"),
          sortable: true,
        },
        {
          key: "position",
          label: this.$t("position"),
          sortable: true,
        },
        {
          key: "organization",
          label: this.$t("organization"),
          sortable: true,
        },
        {
          key: "region",
          label: this.$t("region"),
          sortable: true,
        },
        {
          key: "state",
          label: this.$t("state"),
          thClass: "text-center",
          tdClass: "text-center",
          sortable: true,
        },
        {
          key: "actions",
          label: this.$t("actions"),
          thClass: "text-center",
          tdClass: "text-center",
          sortable: true,
        },
      ],
      TranslateItems: [],
      LanguageList: [],
      filter: {
        Seria: "",
        Number: "",
        DateOfBirth: "",
      },
      personLoading: false,
    };
  },

  created() {
    this.lang = localStorage.getItem("locale") || "ru";
    this.show = true;
    PrtnContractTypeService.Get(this.$route.params.id).then((res) => {
      this.show = false;
      this.Data = res.data;
    });

    RegionService.GetAsSelectList(211)
      .then((res) => {
        this.RegionList = res.data;
      })
      .catch((error) => {
        this.makeToast(error.response.data.errors, "danger");
      });

    ManualService.LanguageSelectList()
      .then((res) => {
        this.LanguageList = res.data;
      })
      .catch((error) => {
        this.makeToast(error.response.data.error, "danger");
      });

    ManualService.StateSelectList()
      .then((res) => {
        this.StateList = res.data;
      })
      .catch((error) => {
        this.makeToast(error.response.data.error, "danger");
      });
    ManualService.SignOrganizationTypeSelectList({})
      .then((res) => {
        this.SignOrganizationTypeList = res.data;
      })
      .catch((error) => {
        this.makeToast(error.response.data.error, "danger");
      });
    PositionService.GetAsSelectList()
      .then((res) => {
        this.PositionList = res.data;
      })
      .catch((error) => {
        this.makeToast(error.response.data.error, "danger");
      });

    OrganizationService.GetAsSelectList()
      .then((res) => {
        this.OrganizationList = res.data;
      })
      .catch((error) => {
        this.makeToast(error.response.data.error, "danger");
      });
  },
  methods: {
    BindValue(value) {
      this.filter.DateOfBirth = value;
    },
    GetPerson() {
      this.personLoading = true;
      PrtnContractTypeService.GetByPassportData(
        this.filter.Seria,
        this.filter.Number,
        this.filter.DateOfBirth
      )
        .then((res) => {
          this.Data.person = res.data;
          this.personLoading = false;
        })
        .catch((error) => {
          this.makeToast(error.response.data.error, "danger");
          this.personLoading = false;
        });
    },
    DeleteTabrow(index) {
      this.Data.tables.splice(index, 1);
    },
    OpenTabrow() {
      this.TabrowModal = true;
      (this.tabrow = {
        id: 0,
        orderNumber: 0,
        signOrganizationTypeId: 0,
        signOrganizationType: "",
        position: "",
        positionId: 0,
        state: "",
        stateId: 0,
        organizationId: 0,
        organization: "",
        regionId: 0,
        region: "",
      }),
        (this.editedIndex1 = -1);
    },
    EditTabrow(item) {
      this.editedIndex1 = this.Data.tables.indexOf(item);
      this.tabrow = Object.assign({}, item);
      this.TabrowModal = true;
    },
    OpenTranslateModal(columnname) {
      this.TranslateModal = true;
      this.GetTranslateItems(columnname);
      this.clearLangTable(columnname);
    },
    GetTranslateItems(columnname) {
      this.TranslateItems = this.Data.translates.filter(
        (item) => item.columnName == columnname
      );
    },
    ChangeTranslate() {
      this.TranslateItem.language = !!this.TranslateItem.languageId
        ? this.LanguageList.filter(
            (item) => item.value == this.TranslateItem.languageId
          )[0].text
        : "";
    },
    clearLangTable(columnname) {
      this.TranslateItem = {
        language: "",
        languageId: 0,
        columnName: columnname,
        translateText: "",
      };
    },
    rowClass(item, type) {
      if (item.Status === 3) return "d-none";
    },
    AddTabrow() {
      console.log(this.RegionList);
      this.$refs.ValidationTabrow.validate().then((success) => {
        if (success) {
          this.tabrow.signOrganizationType =
            this.SignOrganizationTypeList.filter(
              (item) => this.tabrow.signOrganizationTypeId === item.value
            )[0].text;

          this.tabrow.organization = this.OrganizationList.filter(
            (item) => this.tabrow.organizationId === item.value
          )[0].text;
          this.tabrow.region = this.RegionList?.filter(
            (item) => this.tabrow.regionId === item.value
          )[0]?.text;

          this.tabrow.position = this.PositionList.filter(
            (item) => this.tabrow.positionId === item.value
          )[0].text;
          this.tabrow.state = this.StateList.filter(
            (item) => this.tabrow.stateId === item.value
          )[0].text;
          if (this.editedIndex1 > -1) {
            Object.assign(this.Data.tables[this.editedIndex1], this.tabrow);
          } else {
            this.Data.tables.push(this.tabrow);
          }
          this.TabrowModal = false;
        }
      });
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
          (item) =>
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
      this.$refs.ValidationDTO.validate().then((success) => {
        if (success) {
          this.saveLoading = true;

          PrtnContractTypeService.Update(this.Data)
            .then((res) => {
              this.makeToast(this.$t("SaveSuccess"), "success");
              this.$router.push({ name: "PrtnContractType" });
              this.saveLoading = false;
            })
            .catch((err) => {
              this.makeToast(this.$t(err), "danger");
              this.saveLoading = false;
            });
        }
      });
    },
  },
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
