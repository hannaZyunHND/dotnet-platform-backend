<template>
  <div class="list-data">
    <!-- Filter -->
    <b-card header-tag="header" class="card-filter" footer-tag="footer">
      <b-row class="form-group">
        <b-col md="6">
          <b-form-input v-model="searchKeyword" @keyup.enter="fetchNotifications"
            placeholder="Tìm kiếm theo tiêu đề thông báo" />
        </b-col>
        <b-col md="2">
          <b-button variant="info" block @click="fetchNotifications">
            <i class="fa fa-search" /> Tìm
          </b-button>
        </b-col>
        <b-col md="2">
          <b-button variant="primary" block @click="resetSearch">
            <i class="fa fa-refresh" /> Làm mới
          </b-button>
        </b-col>
        <b-col md="2">
          <router-link :to="{ name: 'GNotificationCreate' }">
            <b-button variant="success" block>
              <i class="fa fa-plus" /> Thêm mới
            </b-button>
          </router-link>
        </b-col>
      </b-row>
    </b-card>

    <!-- Table -->
    <div class="card card-data">
      <div class="card-body">
        <div class="table-responsive">
          <table class="table table-striped">
            <thead class="thead-dark">
              <tr>
                <th>#</th>
                <th>Tiêu đề</th>
                <th>Mô tả</th>
                <th>Người tạo</th>
                <th>Ngày tạo</th>
                <th>Thao tác</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="(item, index) in gnotification.items" :key="item.id">
                <td>{{ index + 1 + (currentPage - 1) * pageSize }}</td>
                <td>{{ item.notificationName }}</td>
                <td>{{ item.notificationDescription }}</td>
                <td>{{ item.createdByUsername }}</td>
                <td>{{ formatDate(item.createdDate) }}</td>
                <td>
                  <router-link :to="{ name: 'GNotificationEdit', params: { id: item.id } }">
                    <b-button size="sm" variant="info">
                      <i class="fa fa-edit" />
                    </b-button>
                  </router-link>
                </td>
              </tr>
              <tr v-if="gnotification.items.length === 0">
                <td colspan="6" class="text-center">Không có dữ liệu</td>
              </tr>
            </tbody>
          </table>
        </div>

        <!-- Pagination -->
        <b-pagination v-model="currentPage" :total-rows="gnotification.total" :per-page="pageSize"
          @input="fetchNotifications" />
      </div>
    </div>
  </div>
</template>

<script>
import { mapActions, mapGetters } from "vuex";
import moment from "moment";

export default {
  name: "GNotificationList",
  data() {
    return {
      currentPage: 1,
      pageSize: 10,
      searchKeyword: "",
    };
  },
  computed: {
    ...mapGetters(["gnotification"]),
  },
  methods: {
    ...mapActions(["getNotifications"]),

    fetchNotifications() {
      this.getNotifications({
        pageIndex: this.currentPage,
        pageSize: this.pageSize,
        filter: this.searchKeyword,
      });
    },

    resetSearch() {
      this.searchKeyword = "";
      this.currentPage = 1;
      this.fetchNotifications();
    },

    formatDate(value) {
      return value ? moment(String(value)).format("DD/MM/YYYY HH:mm") : "";
    },
  },
  mounted() {
    this.fetchNotifications();
  },
};
</script>
