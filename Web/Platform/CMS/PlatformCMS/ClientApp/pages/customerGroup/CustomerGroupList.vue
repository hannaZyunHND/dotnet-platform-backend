<template>
    <div class="list-data">
        <b-card header-tag="header" class="card-filter" footer-tag="footer">
            <b-row class="form-group">
                <b-col md="6">
                    <b-form-input v-model="searchKeyword" @keyup.enter="fetchCustomerGroups" type="text"
                        placeholder="Tìm kiếm theo tên nhóm" />
                </b-col>
                <b-col md="2">
                    <b-button variant="info" block @click="fetchCustomerGroups">
                        <i class="fa fa-search" /> Tìm
                    </b-button>
                </b-col>
                <b-col md="2">
                    <b-button variant="primary" block @click="resetSearch">
                        <i class="fa fa-refresh" /> Làm mới
                    </b-button>
                </b-col>
                <b-col md="2">
                    <b-button variant="success" block @click="create">
                        <i class="fa fa-plus" /> Thêm mới
                    </b-button>
                </b-col>
            </b-row>
        </b-card>

        <div class="card card-data">
            <div class="card-body">
                <div class="table-responsive">
                    <table class="table table-striped">
                        <thead class="thead-dark">
                            <tr>
                                <th>#</th>
                                <th>Tên nhóm</th>
                                <th>Ngày tạo</th>
                                <th>Thao tác</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr v-for="(item, index) in customerGroups.items" :key="item.id">
                                <td>{{ index + 1 + (currentPage - 1) * pageSize }}</td>
                                <td>{{ item.name }}</td>
                                <td>{{ formatDate(item.createdDate) }}</td>
                                <td>
                                    <b-button size="sm" variant="info" @click="edit(item)">
                                        <i class="fa fa-edit"></i>
                                    </b-button>
                                </td>
                            </tr>
                            <tr v-if="customerGroups.items.length === 0">
                                <td colspan="4" class="text-center">Không có dữ liệu</td>
                            </tr>
                        </tbody>
                    </table>
                </div>

                <b-pagination v-model="currentPage" :total-rows="customerGroups.total" :per-page="pageSize"
                    @input="fetchCustomerGroups" />
            </div>
        </div>
        <b-modal id="group-modal" v-model="showModal" title="Nhóm khách hàng" size="xl" hide-footer>
            <b-form-group label="Tên nhóm">
                <b-form-input v-model="groupForm.name" placeholder="Nhập tên nhóm" />
            </b-form-group>

            <b-card>
                <b-row>
                    <b-col md="3">
                        <b-form-input v-model="userFilter.fullname" placeholder="Tìm theo tên"
                            @keyup.enter="fetchUsers" />
                    </b-col>
                    <b-col md="3">
                        <b-form-input v-model="userFilter.email" placeholder="Tìm theo email"
                            @keyup.enter="fetchUsers" />
                    </b-col>
                    <b-col md="3">
                        <b-form-input v-model="userFilter.country" placeholder="Tìm theo quốc gia"
                            @keyup.enter="fetchUsers" />
                    </b-col>
                    <b-col md="3">
                        <b-button variant="info" @click="fetchUsers">Tìm</b-button>
                    </b-col>


                </b-row>

                <div class="table-responsive mt-3">
                    <table class="table table-bordered table-hover">
                        <thead class="thead-light">
                            <tr>
                                <th><input type="checkbox" :checked="isAllSelected" @change="toggleSelectAll" /></th>
                                <th>Fullname</th>
                                <th>Email</th>
                                <th>Country</th>
                                <th>Sản phẩm đã mua</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr v-for="user in userList" :key="user.id">
                                <td>
                                    <input type="checkbox" :value="user.id" :checked="selectedUserIds.includes(user.id)"
                                        @change="handleUserCheckbox(user.id)" />
                                </td>
                                <td>{{ user.fullname }}</td>
                                <td>{{ user.email }}</td>
                                <td>{{ user.country }}</td>
                                <td>{{ user.totalOrderDetails || 0 }}</td>
                            </tr>
                            <tr v-if="userList.length === 0">
                                <td colspan="5" class="text-center">Không có dữ liệu</td>
                            </tr>
                        </tbody>
                    </table>
                </div>




                <b-pagination v-model="userFilter.pageIndex" :total-rows="userTotal" :per-page="userFilter.pageSize"
                    size="sm" @input="onPageChange" />
            </b-card>

            <div class="text-right mt-3">
                <b-button variant="secondary" @click="showModal = false">Hủy</b-button>
                <b-button variant="primary" @click="submitGroup">Lưu</b-button>
            </div>
        </b-modal>

    </div>
</template>

<script>
import { mapActions, mapGetters } from "vuex";
import moment from "moment";

export default {
    name: "CustomerGroupList",
    data() {
        return {
            currentPage: 1,
            pageSize: 10,
            searchKeyword: "",

            showModal: false,
            isEditing: false,
            groupForm: {
                id: null,
                name: "",
                customerIds: []
            },
            userFields: [
                { key: "select", label: "Select" },
                { key: "fullname", label: "Fullname" },
                { key: "email", label: "Email" },
                { key: "country", label: "Country" },
                { key: "total", label: "Sản phẩm đã mua" }
            ],

            // data filter cho bảng user trong modal
            userFilter: {
                email: "",
                country: "",
                fullname: "",
                pageIndex: 1,
                pageSize: 20
            },
            userList: [],
            userTotal: 0,
            userLoading: false,
            selectedUserIds: []

        };
    },
    computed: {
        ...mapGetters(["customerGroups"])

    },

    methods: {
        ...mapActions(["getCustomerGroups", "searchCustomers", "getSelectedCustomerInCustomerGroupById "]),



        fetchCustomerGroups() {
            this.getCustomerGroups({
                pageIndex: this.currentPage,
                pageSize: this.pageSize,
                filter: this.searchKeyword
            });
        },
        onPageChange() {
            this.fetchUsers();
        },
        resetSearch() {
            this.searchKeyword = "";
            this.currentPage = 1;
            this.fetchCustomerGroups();
        },

        formatDate(value) {
            return value ? moment(String(value)).format("DD/MM/YYYY HH:mm") : "";
        },

        create() {
            this.groupForm = { id: null, name: "", customerIds: [] };
            this.selectedUserIds = [];
            this.isEditing = false;
            this.showModal = true;
            this.fetchUsers();
        },

        edit(item) {
            this.isEditing = true;
            this.showModal = true;

            this.$store.dispatch("getSelectedCustomerInCustomerGroupById", item.id).then(res => {
                this.groupForm = {
                    id: res.id,
                    name: res.name,
                    customerIds: res.customerIds
                };

                this.selectedUserIds = [...res.customerIds];

                this.fetchUsers(); // Gọi sau khi set selectedUserIds
            });
        },

        fetchUsers() {
            this.userLoading = true;
            this.searchCustomers(this.userFilter).then((res) => {
                // Force reactivity
                this.userList = [];
                this.$nextTick(() => {
                    this.$set(this, 'userList', res.items || []);
                });

                // Giữ lại các id đã chọn trước đó
                const allCurrentIds = res.items.map(u => u.id);
                this.selectedUserIds = Array.from(
                    new Set([...this.selectedUserIds.filter(id => !allCurrentIds.includes(id)), ...this.selectedUserIds])
                );

                this.userTotal = res.totalItems;
                this.userLoading = false;
            });
        },


        toggleSelectAll() {
            const currentPageIds = this.userList.map(u => u.id);
            const allSelected = currentPageIds.every(id => this.selectedUserIds.includes(id));

            if (allSelected) {
                this.selectedUserIds = this.selectedUserIds.filter(id => !currentPageIds.includes(id));
            } else {
                this.selectedUserIds = Array.from(new Set([...this.selectedUserIds, ...currentPageIds]));
            }
        },

        handleUserCheckbox(userId) {
            const idx = this.selectedUserIds.indexOf(userId);
            if (idx === -1) {
                this.selectedUserIds.push(userId);
            } else {
                this.selectedUserIds.splice(idx, 1);
            }
        },

        submitGroup() {
            const payload = {
                id: this.groupForm.id,
                name: this.groupForm.name,
                customerIds: this.selectedUserIds
            };

            const action = this.isEditing ? "updateCustomerGroup" : "createCustomerGroup";

            this.$store.dispatch(action, payload).then((res) => {
                this.showModal = false;
                this.fetchCustomerGroups();
                this.$toast.success("Thành công");
            });
        }

    },
    mounted() {
        this.fetchCustomerGroups();
    }
};
</script>
