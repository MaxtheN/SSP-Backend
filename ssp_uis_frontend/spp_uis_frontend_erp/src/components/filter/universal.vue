<template>
  <div>
    <b-button variant="outline-primary" @click="$emit('show', true)">
      <feather-icon icon="FilterIcon" />
    </b-button>

    <b-modal v-model="show" :title="$t('Filter')" hide-footer @close="onClose" no-close-on-backdrop>
      <b-card-text>
        <div class="form-group" v-if="checkKeyInFilter('contractorRegionId')">
          <form-select
            :options="RegionList"
            v-model="filter.contractorRegionId"
            @input="ChangeRegion"
            :label="$t('Oblast')"
            @change="
              changeHandler('text', 'contractorRegion', )
            "
          ></form-select>
        </div>

        <div class="form-group" v-if="checkKeyInFilter('contractorDistrictId')">
          <form-select
            :options="DistrictList"
            v-model="filter.contractorDistrictId"
            :label="$t('Region')"
          ></form-select>
        </div>

        <div class="form-group" v-if="checkKeyInFilter('contractorInn')">
          <form-input v-model="filter.contractorInn" :label="$t('inn')" />
        </div>
        <!-- @input="inputHandeler($event, 'contractorInn')" -->

        <div class="form-group" v-if="checkKeyInFilter('prtnContractTypeId')">
          <form-select
            :options="ContractTypeList"
            v-model="filter.prtnContractTypeId"
            :label="$t('prtnContractType')"
          ></form-select>
        </div>

        <div class="d-flex justify-content-end mt-2 mb-1">
          <b-button
            class="mr-2"
            @click="cleanFilter"
            style="width: 100%"
            size="xl"
            variant="danger"
          >{{ $t("clean") }}</b-button>
          <b-button
            @click="search"
            style="width: 100%"
            size="xl"
            variant="success"
          >{{ $t("search") }}</b-button>

          <!-- @click="CancelApproval" -->
        </div>
      </b-card-text>
    </b-modal>
  </div>
</template>
<script>
import { BButton, BModal, VBModal, BCardText } from "bootstrap-vue";
import RegionService from "@/services/info/region.service";
import DistrictService from "@/services/info/district.service";
import PrtnContractTypeService from "@/services/info/prtncontracttype.service";

export default {
  created() {
    RegionService.GetAsSelectList(211)
      .then(res => {
        this.RegionList = res.data;
      })
      .catch(error => {
        this.makeToast(error.response.data.error, "danger");
      });
    PrtnContractTypeService.GetAsSelectList()
      .then(res => {
        this.ContractTypeList = res.data;
      })
      .catch(error => {
        this.makeToast(error.response.data.errors, "danger");
      });
  },
  props: {
    filter: {
      type: Object,
      default: () => {}
    },
    show: {
      type: Boolean,
      default: false
    }
  },
  data() {
    return {
      FilterModal: false,
      ContractTypeList: [],
      RegionList: [],
      DistrictList: []
    };
  },
  computed: {
    openModal() {
      return this.show;
    }
  },
  components: {
    BButton,
    BModal,
    VBModal,
    BCardText
  },
  methods: {
    changeHandler(itemKey, key) {
      // console.log("AAA", e);
      // if (key == "statusIds") {
      //   // this.statusId = e[itemKey];
      //   this.$emit("input", {
      //     ...this.filter,
      //     [key]: [e[itemKey]],
      //     [key2]: e[itemKey2]
      //   });
      // } else {
      //   this.$emit("input", {
      //     ...this.filter,
      //     [key]: "AAAAAAAAAAAAAAAA"
      //   });
      // }
    },
    // inputHandeler(e, key) {
    //   this.$emit("input", {
    //     ...this.filter,
    //     [key]: e
    //   });
    // },
    onClose() {
      this.$emit("close", true);
    },
    search() {
      this.$emit("search", true);
    },
    cleanFilter() {
      // this.statusId = null;
      this.$emit("clear", true);
    },
    ChangeRegion(id) {
      if (id) {
        this.filter.contractorDistrictId = null;
        // this.Refresh();
        this.GetDistrict(id);
      } else {
        this.filter.contractorDistrictId = null;
        // this.Refresh();
      }
    },
    GetDistrict(id) {
      DistrictService.GetAsSelectList(id)
        .then(res => {
          this.DistrictList = res.data;
        })
        .catch(error => {
          this.makeToast(error.response.data.error, "danger");
        });
    },
    checkKeyInFilter(key) {
      return key in this.filter;
    }
  }

  // watch: {
  //   openModal(newVal) {
  //     if (newVal) {
  //       RegionService.GetAsSelectList(211)
  //         .then(res => {
  //           this.RegionList = res.data;
  //         })
  //         .catch(error => {
  //           this.makeToast(error.response.data.error, "danger");
  //         });
  //       PrtnContractTypeService.GetAsSelectList()
  //         .then(res => {
  //           this.ContractTypeList = res.data;
  //         })
  //         .catch(error => {
  //           this.makeToast(error.response.data.errors, "danger");
  //         });
  //     }
  //   }
  // }
};
</script>