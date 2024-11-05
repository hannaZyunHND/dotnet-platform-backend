<template>
    <div class="list-data">
        <loading :active.sync="isLoading" :height="35" :width="35" :is-full-page="true"></loading>
        <b-card header-tag="header" class="card-filter" footer-tag="footer">
            <div>
                <b-col md="12">
                    <b-row class="form-group">
                        <b-col>
                            <label>Keyword</label>
                            <b-form-input type="text" v-model="filter.keyword" placeholder="Keyword"></b-form-input>
                        </b-col>
                        <b-col>
                            <label>Nhà cung cấp</label>
                            <select class="form-control" v-model="filter.emailSupplier">
                                <option value="">TẤT CẢ</option>
                                <!-- <option v-for="u in userSuppliers" :value="u.value">{{ u.label }}</option> -->
                            </select>
                        </b-col>
                        <b-col>
                            <label>Từ ngày</label>
                            <b-form-input type="datetime-local" v-model="filter.startDate"></b-form-input>
                        </b-col>
                        <b-col>
                            <label>Đến ngày</label>
                            <b-form-input type="datetime-local" v-model="filter.endDate"></b-form-input>
                        </b-col>
                        <b-col>
                            <label>Trạng thái dịch vụ</label>
                            <select class="form-control" v-model="filter.activeStatus">
                                <option value="">ALL</option>
                                <!-- <option v-for="a in allActiveStatus" :value="a.value">{{ a.label }}</option> -->
                            </select>
                        </b-col>
                    </b-row>
                </b-col>
            </div>
        </b-card>
        <div class="card card-data">
            <div class="card-body">
                <div role="toolbar" class=" mb-2" aria-label="Toolbar with button groups and dropdown menu">
                    <div role="group" class="mx-1 btn-group">
                        <!--<router-link class="btn btn-success" :to="{ path: 'add' }"><i class="fa fa-plus"></i> Thêm mới</router-link>-->
                        <button type="button" class="btn btn-danger"><i class="fa fa-trash-o"></i> Xóa</button>
                    </div>
                    <b-dropdown class="mx-1" variant="info" right text="Hành động" icon>
                        <b-dropdown-item @click="onExportFinanceData()">Xuất báo cáo</b-dropdown-item>
                    </b-dropdown>

                    <div class="mx-1 btn-group mi-paging">
                        <b-pagination v-model="filter.index" :total-rows="totalPage" :per-page="filter.size"
                            aria-controls="_product"></b-pagination>
                    </div>
                </div>

                <div class="table-responsive">
                    <div class="dataTables_wrapper dt-bootstrap4 no-footer">
                        <div class="clear"></div>
                        <table class="table data-thumb-view dataTable no-footer table-bordered" role="grid">
                            <thead class="table table-centered table-nowrap">
                                <tr role="row">
                                    <th>ID</th>
                                    <th>Thông tin khách hàng</th>
                                    <th>Ngày tạo</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr role="row" class="odd" v-for="item in listEmails" :key="item.id">
                                    <td>{{ item.id }}</td>
                                    <td>
                                        {{ item.email }}
                                    </td>
                                    <td>
                                        {{ formatTime(item.createdAt) }}

                                    </td>

                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>

            </div>
        </div>
    </div>
</template>
<script>
import "vue-loading-overlay/dist/vue-loading.css";
import { mapGetters, mapActions } from "vuex";
import Loading from "vue-loading-overlay";
import moment from 'moment'

export default {
    name: "Feedback",
    components: {
        Loading
    },
    data() {
        return {
            filter: {
                keyword: '',
                startDate: '',
                endDate: '',
                activeStatus: '',
                supplierStatus: '',
                index: 1,
                size: 10,
                emailSupplier: ''
            },
            total: 0,
            listEmails: [],
            totalPage: 0,
            currentOrder: {},
            isLoading: false,
            currentInfo: {},
            // region chat
            fileInput: null,
            images: [],
            currentMessage: '',
            currentChatSession: null,
            updateInterval: null, // To store the interval ID for clearing later
            currentOrderCode: "",

        };
    },
    methods: {
        ...mapActions(["GetAllEmailSubcribe"]),

        onLoadData() {
            this.GetAllEmailSubcribe().then(response => {
                this.listEmails = response;
                this.totalPage = response.length;

            })
        },

        formatTime(dateString) {
            return moment(dateString).format('h:mm A DD-MM-YYYY');
        }
    },
    mounted() {
        this.onLoadData();
    },
    watch: {
    },
};
</script>
<style scoped>
.upload-preview-container {
    display: flex;
    align-items: center;
}

.upload-container,
.image-preview {
    display: flex;
    align-items: center;
}

.upload-button {
    width: 50px;
    height: 50px;
    border: 2px dashed #000;
    display: flex;
    align-items: center;
    justify-content: center;
    cursor: pointer;
    margin-right: 10px;
}

.upload-button span {
    font-size: 24px;
    font-weight: bold;
}

.image-preview {
    display: flex;
}

.image-item {
    position: relative;
    margin-right: 10px;
}

.image-item img {
    width: 50px;
    height: 50px;
    object-fit: cover;
    border: 1px solid #ccc;
    padding: 2px;
    background: #fff;
}

.delete-icon {
    position: absolute;
    top: 0;
    right: 0;
    background: rgba(255, 0, 0, 0.7);
    color: white;
    width: 15px;
    height: 15px;
    display: flex;
    align-items: center;
    justify-content: center;
    cursor: pointer;
    font-size: 12px;
    line-height: 15px;
}
</style>