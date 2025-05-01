<template>
  <b-card no-body>
    <div class="m-2">
      <b-row>
        <b-col
          cols="12"
          md="6"
          class="d-flex align-items-center justify-content-start mb-1 mb-md-0"
        >
          <label>{{ $t("Entries") }}</label>
          <v-select
            v-model="filter.pageSize"
            :dir="$store.state.appConfig.isRTL ? 'rtl' : 'ltr'"
            :options="filter.perPageOptions"
            :clearable="false"
            @input="Refresh"
            class="per-page-selector d-inline-block ml-50 mr-1"
          />
          <!-- <b-button
            variant="primary"
            :to="{ name: 'EditUser', params: { id: 0 } }"
          >
            <feather-icon icon="PlusIcon"></feather-icon>
            {{ $t("create") }}
          </b-button> -->
        </b-col>
        <b-col md="2"></b-col>
        <b-col cols="12" md="4">
          <b-input-group class="text-right">
            <b-form-input v-model="filter.search" :placeholder="$t('search')" />
            <b-input-group-append>
              <b-button @click="Refresh" variant="primary">
                <feather-icon icon="SearchIcon" />
              </b-button>
            </b-input-group-append>
          </b-input-group>
        </b-col>
      </b-row>
    </div>

    <b-table
      ref="refInvoiceListTable"
      :items="list"
      responsive
      :fields="fields"
      primary-key="id"
      sticky-header="65vh"
      no-border-collapse
      :busy="isBusy"
      show-empty
      :empty-text="$t('NotFound')"
      class="position-relative"
      @sort-changed="SortChange"
    >
      <template #cell(status)="{ item }">
        <b-badge
          :variant="item.status == 'Пассив' ? 'light-danger' : 'light-success'"
          >{{ item.status }}</b-badge
        >
      </template>
      <template #cell(roles)="{ item }">
        <div v-for="(el, idx) in item.roles" :key="idx">
          {{ el }}
        </div>
      </template>
      <template #cell(actions)="{ item }">
        <div class="text-center">
          <feather-icon
            style="cursor: pointer"
            @click="setIsDetail(), setDetail(item.id)"
            icon="EyeIcon"
          ></feather-icon>
        </div>
      </template>
      <template v-slot:table-busy>
        <div
          class="text-center text-primary my-2"
          style="vertical-align: middle"
        >
          <b-spinner class="align-middle mr-2"></b-spinner>
          <strong>{{ $t("Loading") }}</strong>
        </div>
      </template>
    </b-table>
    <div class="mx-2 mb-2">
      <b-row>
        <b-col
          cols="12"
          sm="6"
          class="d-flex align-items-center justify-content-center justify-content-sm-start"
        >
          <span class="text-muted">
            {{ $t("Showing") }} {{ firstNumber }} {{ $t("to") }}
            {{ lastNumber }} {{ $t("of") }} {{ filter.total }}
            {{ $t("entries") }}
          </span>
        </b-col>
        <!-- Pagination -->
        <b-col
          cols="12"
          sm="6"
          class="d-flex align-items-center justify-content-center justify-content-sm-end"
        >
          <b-pagination
            v-model="filter.page"
            :total-rows="filter.total"
            :per-page="filter.pageSize"
            first-number
            last-number
            @input="Refresh"
            class="mb-0 mt-1 mt-sm-0"
            prev-class="prev-item"
            next-class="next-item"
          >
            <template #prev-text>
              <feather-icon icon="ChevronLeftIcon" size="18" />
            </template>
            <template #next-text>
              <feather-icon icon="ChevronRightIcon" size="18" />
            </template>
          </b-pagination>
        </b-col>
      </b-row>
    </div>

    <b-modal size="lg" v-model="isDetail" hide-footer>
      <b-overlay :show="detailLoader">
        <template #modal-title>
          {{ detail.type }}
        </template>
        <b-card-text style="overflow: auto;">
          <template v-for="key in Object.keys(detail)">
            <h5 v-if="key !== 'detail'">{{ `${key}: ${detail[key]}` }}</h5>
          </template>
        </b-card-text>
        <b-form-textarea plaintext rows="10" :value="detail.detail">
        </b-form-textarea>
      </b-overlay>
    </b-modal>
  </b-card>
</template>

<script>
import AppErrorService from "@/services/managment/apperror.service";
import {
  BSpinner,
  BButton,
  BPagination,
  BTable,
  BCol,
  BRow,
  BCard,
  BBadge,
  BInputGroup,
  BFormInput,
  BInputGroupAppend,
  BModal,
  BLink,
  BCardText,
  BIcon,
  BOverlay,
  BFormTextarea,
} from "bootstrap-vue";

export default {
  components: {
    BIcon,
    BButton,
    BPagination,
    BTable,
    BCol,
    BRow,
    BSpinner,
    BCard,
    BBadge,
    BInputGroup,
    BFormInput,
    BInputGroupAppend,
    BModal,
    BLink,
    BCardText,
    BOverlay,
    BFormTextarea,
  },
  data() {
    return {
      list: [],
      filter: {
        search: "",
        sortBy: "id",
        orderType: "desc",
        page: 1,
        pageSize: 20,
        perPageOptions: [10, 20, 50, 100],
        total: 0,
      },
      fields: [
        {
          key: "id",
          label: this.$t("id"),
          thClass: "text-center",
          tdClass: "text-center",
          sortable: true,
        },
        {
          key: "title",
          label: this.$t("title"),
          thClass: "text-center",
          tdClass: "text-center",
          sortable: true,
        },
        {
          key: "requestPath",
          label: this.$t("requestPath"),
          thClass: "text-center",
          tdClass: "text-center",
          sortable: true,
        },
        {
          key: "requestTraceId",
          label: this.$t("requestTraceId"),
          thClass: "text-center",
          tdClass: "text-center",
          sortable: true,
        },
        {
          key: "host",
          label: this.$t("host"),
          thClass: "text-center",
          tdClass: "text-center",
          sortable: true,
        },
        {
          key: "type",
          label: this.$t("type"),
          thClass: "text-center",
          tdClass: "text-center",
          sortable: true,
        },
        {
          key: "createdAt",
          label: this.$t("createdAt"),
          thClass: "text-center",
          tdClass: "text-center",
          sortable: true,
        },
        {
          key: "statusCode",
          label: this.$t("status"),
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
      isDetail: false,
      detail: {},

      isBusy: false,
      detailLoader: false,
    };
  },
  computed: {
    firstNumber() {
      return (this.filter.page - 1) * this.filter.pageSize + 1;
    },
    lastNumber() {
      if (this.filter.total < this.filter.pageSize) {
        return this.filter.total;
      } else {
        if (this.filter.page * this.filter.pageSize > this.filter.total) {
          return this.filter.total;
        } else {
          return this.filter.page * this.filter.pageSize;
        }
      }
    },
  },
  created() {
    this.Refresh();
  },
  methods: {
    setErrors(err) {
      for (const item in err) {
        this.makeToast(err[item], "danger");
      }
    },
    setIsDetail() {
      this.isDetail = !this.isDetail;
    },
    setDetail(id) {
      if (!id) return;
      this.detailLoader = true;

      AppErrorService.Get(id)
        .then((res) => {
          this.detail = res.data;
          this.detailLoader = false;
        })
        .catch((err) => {
          this.setErrors(err.response.data.errors);
        });
    },

    SortChange(data) {
      this.filter.sortBy = data.sortBy;
      this.filter.orderType = data.sortDesc ? "desc" : "asc";
      this.Refresh();
    },

    Refresh() {
      this.isBusy = true;
      AppErrorService.GetList(this.filter)
        .then((res) => {
          this.list = res.data.rows;
          this.filter.total = res.data.total;
          this.isBusy = false;
        })
        .catch((err) => {
          this.setErrors(err.response.data.errors);
        });
    },
  },
};
</script>
