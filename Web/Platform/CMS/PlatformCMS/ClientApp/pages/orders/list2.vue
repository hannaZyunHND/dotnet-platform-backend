<template>
    <div class="list-data">
        <loading :active.sync="isLoading" :height="35" :width="35" :is-full-page="true"></loading>
        <b-card header-tag="header" class="card-filter" footer-tag="footer">
            <div>
                <b-col md="12">
                    <b-row class="form-group">
                        <b-col md="3">
                            <label>Keyword</label>
                            <b-form-input type="text" v-model="filter.keyword" placeholder="Keyword"></b-form-input>
                        </b-col>
                        <b-col md="3">
                            <label>Ngày bắt đầu</label>
                            <b-form-input type="datetime-local" v-model="filter.startDate"></b-form-input>
                        </b-col>
                        <b-col md="3">
                            <label>Ngày kết thúc</label>
                            <b-form-input type="datetime-local" v-model="filter.endDate"></b-form-input>
                        </b-col>
                        <b-col md="3">
                            <label>Trạng thái dịch vụ</label>
                            <select class="form-control" v-model="filter.activeStatus">
                                <option value="">ALL</option>
                                <option v-for="a in allActiveStatus" :value="a.value">{{ a.label }}</option>
                            </select>
                        </b-col>
                        <!-- <b-col md="3">
                            <label>Trạng thái NCC</label>
                            <select class="form-control" v-model="filter.supplierStatus">
                                <option value="">ALL</option>
                                <option v-for="a in allSupplierStatus" :value="a.value">{{ a.label }}</option>
                            </select>
                        </b-col> -->


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
                        <b-dropdown-item>Kích hoạt</b-dropdown-item>
                        <b-dropdown-item>Không kích hoạt</b-dropdown-item>
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
                                    <th>Mã đơn hàng</th>
                                    <th>Thông tin Khách hàng</th>
                                    <th>Thông tin sản phẩm</th>
                                    <th>Thông tin đơn hàng</th>
                                    <!-- <th>Email đối tác</th> -->
                                    <th>Trạng thái</th>
                                    <th>Hành động</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr role="row" class="odd" v-for="item in filterdOrders">
                                    <td>{{ item.orderCode }}</td>
                                    <td>
                                        <ul>
                                            <li>Tên: {{ item.fullName }}</li>
                                            <li>Email: {{ item.email }}</li>
                                            <li>SĐT: {{ item.phoneNumber }}</li>
                                        </ul>
                                    </td>
                                    <td>
                                        <ul>
                                            <li>SP: {{ item.productParentTitle }}</li>
                                            <li>Package: {{ item.productChildTitle }}</li>
                                            <li>Options: {{ item.zoneTitles }}</li>
                                            <li>Đ.tác: {{ item.emailSupplier }}</li>
                                        </ul>
                                    </td>
                                    <td>
                                        <ul>
                                            <li v-if="item.quantityAldut > 0">Ng.Lớn x {{ item.quantityAldut }}</li>
                                            <li v-if="item.quantityChildren > 0">Tr.Em x {{ item.quantityChildren }}
                                            </li>
                                        </ul>
                                        Tổng GT: {{ item.logPrice.toLocaleString() }}
                                    </td>
                                    <!-- <td>
                                        <ul>
                                            <li>Email: {{ item.emailSupplier }}</li>
                                        </ul>
                                    </td> -->
                                    <td>
                                        <ul>
                                            <li>TT. Đơn hàng: {{ item.activeStatus }}</li>
                                            <li>TT. NCC: {{ item.supplierStatus }}</li>
                                        </ul>
                                    </td>
                                    <td>
                                        <b-button-group>
                                            <b-button variant="primary"
                                                @click="onOpenUpdateStatusDonHangModal(item)">Update TT đơn
                                                hàng</b-button>
                                            <!-- <b-button variant="success"
                                                @click="onOpenUpdateStatusDoiTacModal(item)">Update TT đối
                                                tác</b-button> -->

                                        </b-button-group>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>

            </div>
        </div>
        <div>
            <b-modal ref="mdDonHang" title="CẬP NHẬT TRẠNG THÁI ĐƠN HÀNG" @ok="onOKUpdateTrangThaiDonHang">
                <b-card>
                    <b-row class="form-group">
                        <b-col md="12">
                            <label>Trạng thái đơn hàng</label>
                            <select class="form-control" v-model="currentOrder.activeStatus">
                                <option v-for="a in allActiveStatus" :value="a.value">{{ a.label }}</option>
                            </select>
                        </b-col>
                        <b-col md="12">
                            <hr>
                        </b-col>
                        <b-col md="12" v-if="currentOrder.activeStatus == 'CHAP_NHAN_DICH_VU'">
                            <label>File Upload E-Ticket</label>
                            <input type="file" class="form-control" ref="currentOrderFiles" multiple
                                accept=".jpg,.jpeg,.png,.xls,.xlsx,.pdf,.doc,.docx,.zip,.rar">
                        </b-col>
                        <b-col md="12" v-if="currentOrder.activeStatus == 'TU_CHOI_DICH_VU'">
                            <label>Lý do từ chối dịch vụ ({{ currentOrder.defaultLanguage }})</label>
                            <textarea class="form-control" rows="5" placeholder="Điền lý do từ chối dịch vụ vào đây"
                                v-model="currentCancelResponse"></textarea>
                        </b-col>
                    </b-row>
                </b-card>
            </b-modal>
        </div>
    </div>
</template>
<script>
import "vue-loading-overlay/dist/vue-loading.css";
import msgNotify from "../../common/constant";
import { mapGetters, mapActions } from "vuex";
import Loading from "vue-loading-overlay";
import moment from 'moment';
export default {
    name: "order",
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
                size: 10
            },
            total: 0,
            filterdOrders: [],
            allActiveStatus: [
                { label: 'TẠO MỚI', value: 'TAO_MOI' },
                { label: 'CHẤP NHẬN DỊCH VỤ', value: 'CHAP_NHAN_DICH_VU' },
                { label: 'ĐÃ SỬ DỤNG DỊCH VỤ', value: 'DA_SU_DUNG_DICH_VU' },
                { label: 'TỪ CHỐI DỊCH VỤ', value: 'TU_CHOI_DICH_VU' },
                { label: 'YÊU CẦU HỦY', value: 'YEU_CAU_HUY' },
                { label: 'ĐÃ HỦY', value: 'DA_HUY' },
            ],
            allSupplierStatus: [
                { label: 'PENDING', value: 'PENDING' },
                { label: 'ĐÃ GỬI MAIL YÊU CẦU', value: 'DA_GUI_EMAIL_YEU_CAU' },
                { label: 'CHẤP NHẬN DỊCH VỤ', value: 'CHAP_NHAN_DICH_VU' },
                { label: 'XÁC NHẬN HOÀN THÀNH', value: 'XAC_NHAN_HOAN_THANH' },
                { label: 'TỪ CHỐI DỊCH VỤ', value: 'TU_CHOI_DICH_VU' },

            ],
            totalPage: 0,
            currentOrder: {},
            isLoading: false,
            currentCancelResponse: ""

        };
    }, 
    methods: {
        ...mapActions(["getOrders", "getOrderStatus", "updateOrderStatus", "getAllTypeOrder", "getListOrderV2", "updateActiveStatusForOrderDetail"]),
        onLoadData() {
            this.getListOrderV2(this.filter).then(response => {
                this.filterdOrders = response.orders;
                this.totalPage = response.total;
                if (this.filterdOrders.length > 0) {
                    this.currentOrder = this.filterdOrders[0];
                }

            })
        },
        onOKUpdateTrangThaiDonHang() {
            let formData = new FormData();
            formData.append('orderDetailId', this.currentOrder.orderDetailId);
            formData.append('activeStatus', this.currentOrder.activeStatus);
            formData.append('cancelResponse', this.currentCancelResponse);
            if (this.$refs.currentOrderFiles) {
                const files = this.$refs.currentOrderFiles.files;
                for (let i = 0; i < files.length; i++) {
                    formData.append('files', files[i]);
                }
            }

            this.isLoading = true
            this.updateActiveStatusForOrderDetail(formData).then(response => {
                alert("Đổi trạng thái thành công!");
                this.$refs.mdDonHang.hide();
                this.isLoading = false
                this.onLoadData()

            })

        },
        onOpenUpdateStatusDonHangModal(item) {
            this.currentOrder = JSON.parse(JSON.stringify(item));
            this.$refs.mdDonHang.show();
        }
    },


    computed: {
        ...mapGetters(["orders"])
    },
    mounted() {
        this.onLoadData();
    },
    watch: {
        filter: {
            handler(newVal, oldVal) {
                console.log('Filter changed from', oldVal, 'to', newVal);
                this.onLoadData();
            },
            deep: true
        }

    },
};
</script>
