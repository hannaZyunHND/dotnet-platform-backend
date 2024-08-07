<template>
    <div class="list-data">
        <loading :active.sync="isLoading" :height="35" :width="35" :is-full-page="true"></loading>
        <b-card header-tag="header" class="card-filter" footer-tag="footer">
            <div v-if="statistics">
                <b-row>
                    <b-col md="4" sm="12">
                        <b-card bg-variant="primary" text-variant="white" header="Tổng giá trị" class="text-center">
                            <b-card-text>
                                <h4>{{ statistics.totalGrossPrice.toLocaleString() }}</h4>
                            </b-card-text>
                        </b-card>
                    </b-col>
                    <b-col md="4" sm="12">
                        <b-card bg-variant="primary" text-variant="white" header="Tổng giá trị" class="text-center">
                            <b-card-text>
                                <h4>{{ statistics.totalPrice.toLocaleString() }}</h4>
                            </b-card-text>
                        </b-card>
                    </b-col>
                    <b-col md="4" sm="12">
                        <b-card bg-variant="primary" text-variant="white" header="Tổng đơn hàng" class="text-center">
                            <b-card-text>
                                <h4>{{ statistics.totalOrder }}</h4>
                            </b-card-text>

                        </b-card>
                    </b-col>
                </b-row>
                <b-row>
                    <b-col>
                        <b-card bg-variant="primary" text-variant="white" header="Tạo mới" class="text-center">
                            <b-card-text>
                                <h4>{{ statistics.total_TAO_MOI }}</h4>
                            </b-card-text>

                        </b-card>
                    </b-col>
                    <b-col>
                        <b-card bg-variant="primary" text-variant="white" header="Chấp nhận dịch vụ"
                            class="text-center">
                            <b-card-text>
                                <h4>{{ statistics.total_CHAP_NHAN_DICH_VU }}</h4>
                            </b-card-text>

                        </b-card>
                    </b-col>
                    <b-col>
                        <b-card bg-variant="primary" text-variant="white" header="Đã sử dụng dịch vụ"
                            class="text-center">
                            <b-card-text>
                                <h4>{{ statistics.total_DA_SU_DUNG_DICH_VU }}</h4>
                            </b-card-text>

                        </b-card>
                    </b-col>
                    <b-col>
                        <b-card bg-variant="primary" text-variant="white" header="Từ chối dịch vụ" class="text-center">
                            <b-card-text>
                                <h4>{{ statistics.total_TU_CHOI_DICH_VU }}</h4>
                            </b-card-text>

                        </b-card>
                    </b-col>
                    <b-col>
                        <b-card bg-variant="primary" text-variant="white" header="Yêu cầu hủy" class="text-center">
                            <b-card-text>
                                <h4>{{ statistics.total_YEU_CAU_HUY }}</h4>
                            </b-card-text>

                        </b-card>
                    </b-col>
                    <b-col>
                        <b-card bg-variant="primary" text-variant="white" header="Đã hủy" class="text-center">
                            <b-card-text>
                                <h4>{{ statistics.total_DA_HUY }}</h4>
                            </b-card-text>

                        </b-card>
                    </b-col>
                </b-row>
            </div>
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
                                <option v-for="u in userSuppliers" :value="u.value">{{ u.label }}</option>
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
                                    <th>Mã đơn hàng</th>
                                    <th>Thông tin Khách hàng</th>
                                    <th>Thông tin sản phẩm</th>
                                    <th>Thông tin đơn hàng</th>
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
                                            <li>SĐT: {{ item.phoneNumber }}</li>
                                            <li>Email: {{ item.email }}</li>
                                            <li><a href="javascript:;" @click="onViewThongTin(item)">Xem ghi chú</a>
                                            </li>
                                        </ul>
                                    </td>
                                    <td>
                                        <ul>
                                            <li>SP: {{ item.productParentTitle }}</li>
                                            <li>Package: {{ item.productChildTitle }}</li>
                                            <li>Options: {{ item.zoneTitles }}</li>
                                            <li>Đối tác: {{ item.supplierFullName }}</li>
                                            <li>Ngày đặt dịch vụ: {{ item.createdDate }}</li>
                                            <li>Ngày SD dịch vụ: {{ item.pickingDate }}</li>
                                        </ul>
                                    </td>
                                    <td>
                                        <ul>
                                            <li v-if="item.quantityAldut > 0">Ng.Lớn x {{ item.quantityAldut }}</li>
                                            <li v-if="item.quantityChildren > 0">Tr.Em x {{ item.quantityChildren }}
                                            </li>

                                        </ul>
                                        <ul>
                                            <li>Mã giảm giá: {{ item.voucher }}</li>
                                            <li>Giá bán: {{ item.logPrice.toLocaleString() }}</li>
                                            <li>Giá vốn: {{ item.logPriceGross.toLocaleString() }}</li>
                                        </ul>
                                    </td>
                                    <!-- <td>
                                        <ul>
                                            <li>Email: {{ item.emailSupplier }}</li>
                                        </ul>
                                    </td> -->
                                    <td>
                                        <div v-html="onConvertStatus(item.activeStatus)"></div>
                                        <div v-if="item.activeStatus == 'YEU_CAU_HUY' || item.activeStatus == 'DA_HUY'">
                                            <ul>
                                                <li>Thời gian Y/C hủy: {{ item.rollbackRequestDate }}</li>
                                                <li>Chính sách hoàn tiền: {{ item.rollbackValue }} %</li>
                                            </ul>
                                        </div>

                                    </td>
                                    <td>
                                        <b-button-group>
                                            <b-button variant="primary"
                                                @click="onOpenUpdateStatusDonHangModal(item)">Update TT đơn
                                                hàng</b-button>
                                            <b-button variant="success" @click="onOpenModalChat(item)">Giao tiếp
                                                KH</b-button>
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
                            <small style="color:red">Note: Nếu không có E-Ticket đính kèm, Email xác nhận dịch vụ của KH
                                sẽ có giá trị sử dụng tương đương E-Ticker</small>
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
        <div>
            <b-modal size="xl" ref="mdThongTin" title="Thông tin bổ sung cho đơn hàng">
                <b-card v-if="currentInfo">
                    <h4>Yêu cầu KH của đơn hàng: {{ currentInfo.orderCode }}</h4>
                    <div v-if="currentInfo.metaData">
                        <table class="table table-centered mb-3"
                            v-for="g in currentInfo.metaData.productBookingNoteGroups">
                            <thead>
                                <tr colspan="2"><b>{{ g.ZoneParentName }}</b></tr>
                            </thead>
                            <tbody>
                                <tr v-for="v in g.NoteList">
                                    <td>{{ v.ZoneName }}</td>
                                    <td>{{ v.noteValue }}</td>
                                </tr>


                            </tbody>
                        </table>
                        <table class="table table-centered mb-3">
                            <thead>
                                <tr colspan="2"><b>Thông tin bổ sung</b></tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td>Sử dụng ứng dụng khác SĐT: </td>
                                    <td>{{ currentInfo.useDiffrenceNumber }}</td>
                                </tr>
                                <tr>
                                    <td>Phần mềm liên lạc: </td>
                                    <td>{{ currentInfo.useAppContact }}</td>
                                </tr>
                                <tr>
                                    <td>Id liên lạc: </td>
                                    <td>{{ currentInfo.useAppContactValue }}</td>
                                </tr>
                                <tr>
                                    <td>Yêu cầu bổ sung: </td>
                                    <td>{{ currentInfo.noteSpecial }}</td>
                                </tr>
                            </tbody>
                        </table>
                    </div>

                </b-card>
            </b-modal>
        </div>
        <div>
            <b-modal size="xl" ref="mdChat" :hide-footer="true" @hidden="onHiddenChatModal()">
                <section v-if="currentChatSession">
                    <div class="container">

                        <div class="row d-flex justify-content-center">
                            <div class="col-12">

                                <div class="card" id="chat1" style="border-radius: 15px;">
                                    <div class="card-header d-flex justify-content-between align-items-center p-3 bg-info text-white border-bottom-0"
                                        style="border-top-left-radius: 15px; border-top-right-radius: 15px;">
                                        <i class="fas fa-angle-left"></i>
                                        <p class="mb-0 fw-bold">Live chat : {{ currentOrderCode }}</p>
                                        <i class="fas fa-times"></i>
                                    </div>
                                    <div class="card-body" style="height: 500px; overflow-y: auto" ref="cardBody">

                                        <div v-for="mess in currentChatSession.sessionDetails">
                                            <div class="d-flex flex-row justify-content-start mb-4"
                                                v-if="mess.senderType !== 3">
                                                <img src="https://mdbcdn.b-cdn.net/img/Photos/new-templates/bootstrap-chat/ava1-bg.webp"
                                                    alt="avatar 1" style="width: 45px; height: 100%;">
                                                <div class="p-3 ms-3"
                                                    style="border-radius: 15px; background-color: rgba(57, 192, 237,.2);"
                                                    v-if="mess.contentType == 1">
                                                    <p class="small mb-0">{{ mess.content }}</p>
                                                </div>
                                                <div class="ms-3" style="border-radius: 15px;"
                                                    v-if="mess.contentType == 2">
                                                    <div class="bg-image">
                                                        <img :src="mess.content"
                                                            style="border-radius: 15px; max-width: 500px;" alt="Image">
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="d-flex flex-row justify-content-end mb-4" v-else>
                                                <div class="p-3 ms-3"
                                                    style="border-radius: 15px; background-color: rgba(57, 192, 237,.2);"
                                                    v-if="mess.contentType == 1">
                                                    <p class="small mb-0">{{ mess.content }}</p>
                                                </div>
                                                <div class="ms-3" style="border-radius: 15px;"
                                                    v-if="mess.contentType == 2">
                                                    <div class="bg-image">
                                                        <img :src="mess.content"
                                                            style="border-radius: 15px; max-width: 500px;" alt="Image">
                                                    </div>
                                                </div>
                                                <img src="https://mdbcdn.b-cdn.net/img/Photos/new-templates/bootstrap-chat/ava1-bg.webp"
                                                    alt="avatar 1" style="width: 45px; height: 100%;">
                                            </div>


                                        </div>


                                    </div>
                                    <div class="card-footer">
                                        <div data-mdb-input-init class="form-outline">
                                            <textarea class="form-control bg-body-tertiary" id="textAreaExample"
                                                rows="4" v-model="currentMessage"></textarea>
                                            <div class="row mt-3">
                                                <div class="col-11">
                                                    <div class="upload-preview-container">
                                                        <div class="upload-container">
                                                            <div class="upload-button" @click="triggerFileInput">
                                                                <span>+</span>
                                                            </div>
                                                            <input type="file" ref="fileInput" accept="image/*"
                                                                @change="onFilesSelected" multiple hidden>
                                                        </div>
                                                        <div v-if="images.length" class="image-preview">

                                                            <div v-for="(image, index) in images" :key="index"
                                                                class="image-item">
                                                                <div class="delete-icon"
                                                                    @click="removeImage(index, images)">x</div>
                                                                <img :src="image.url" alt="Image Preview">
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-1 text-right">
                                                    <button class="btn btn-success"
                                                        @click="onClickSendMessage()">Gửi</button>
                                                </div>
                                            </div>


                                        </div>
                                    </div>
                                </div>

                            </div>
                        </div>

                    </div>
                </section>
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
                size: 10,
                emailSupplier: ''
            },
            total: 0,
            filterdOrders: [],
            userSuppliers: [],
            statistics: null,
            allActiveStatus: [
                { label: 'TẠO MỚI', value: 'TAO_MOI' },
                { label: 'CHẤP NHẬN DỊCH VỤ', value: 'CHAP_NHAN_DICH_VU' },
                { label: 'XÁC NHẬN ĐÃ SỬ DỤNG DỊCH VỤ', value: 'DA_SU_DUNG_DICH_VU' },
                { label: 'TỪ CHỐI DỊCH VỤ', value: 'TU_CHOI_DICH_VU' },
                { label: 'YÊU CẦU HỦY', value: 'YEU_CAU_HUY' },
                { label: 'HỦY DỊCH VỤ', value: 'DA_HUY' },
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
            currentCancelResponse: "",
            currentInfo: {},
            // region chat
            fileInput: null,
            images: [],
            currentMessage: '',
            currentChatSession: null,
            updateInterval: null, // To store the interval ID for clearing later
            currentOrderCode: "",
            // end region

        };
    },
    methods: {
        ...mapActions(["getOrders",
            "getOrderStatus",
            "updateOrderStatus",
            "getAllTypeOrder",
            "getListOrderV2",
            "updateActiveStatusForOrderDetail",
            "getAllUserSupplier",
            "exportExcelListOrderVersionOffice",
            "exportExcelListOrderVersionSupplier",
            "getChatSessionByOrderDetailId",
            "sendChatSessionMessageByCMSUser"]),

        // region Chat
        onHiddenChatModal() {
            this.currentChatSession = null;

            // Clear the interval
            if (this.updateInterval) {
                clearInterval(this.updateInterval);
                this.updateInterval = null;
            }
        },
        async onClickSendMessage() {
            if (this.currentChatSession) {
                let base64Images = await Promise.all(this.images.map(async (image) => {
                    return await this.convertFileToBase64(image.file);
                }));
                let data = {
                    orderChatSessionId: this.currentChatSession.id,
                    images: base64Images,
                    messages: this.currentMessage
                }
                this.sendChatSessionMessageByCMSUser(data).then(response => {
                    this.currentMessage = '';
                    this.images = [];
                    let reloadData = {
                        orderDetailId: this.currentChatSession.orderDetailId
                    }
                    this.getChatSessionByOrderDetailId(reloadData).then(response => {
                        this.currentChatSession = response;


                    })

                }).catch(err => {
                    alert("Không thể gửi được lời nhắn, liên hệ quản trị viên")
                })
            }

        },
        triggerFileInput() {
            this.$refs.fileInput.click();
        },
        onFilesSelected() {
            const files = Array.from(this.$refs.fileInput.files);
            const newImages = files.map(file => ({
                file,
                url: URL.createObjectURL(file)
            }));
            this.images = [...this.images, ...newImages];
        },
        removeImage(index) {
            this.images.splice(index, 1);
        },
        onOpenModalChat(item) {
            this.currentOrderCode = item.orderCode
            this.$refs.mdChat.show();
            let data = {
                orderDetailId: item.orderDetailId
            };

            this.fetchChatSession(data);
            console.log(this.$refs.mdChat)
            // Set up an interval to fetch data every 3 seconds if the modal is open
            this.updateInterval = setInterval(() => {
                if (this.$refs.mdChat.isVisible) {
                    this.fetchChatSession(data);
                } else {
                    clearInterval(this.updateInterval);
                    this.updateInterval = null;
                }
            }, 1000);
        },
        fetchChatSession(data) {

            this.getChatSessionByOrderDetailId(data).then(response => {

                if (!this.currentChatSession) {
                    this.currentChatSession = response;
                } else if (this.currentChatSession.id !== response.id) {
                    this.currentChatSession = response;

                } else if (this.currentChatSession.id == response.id) {
                    this.currentChatSession.sessionDetails = response.sessionDetails;
                }
                // const cardBody = this.$refs.cardBody; // Reference to the card-body element
                // cardBody.scrollTop = cardBody.scrollHeight;
                // const isScrolledToBottom = cardBody.scrollHeight - cardBody.scrollTop === cardBody.clientHeight;
                // // After updating the data, check if we need to scroll to the bottom
                // if (isScrolledToBottom) {
                //     this.$nextTick(() => {
                //         cardBody.scrollTop = cardBody.scrollHeight;
                //     });
                // }
                // this.currentChatSession = response;
            });
        },
        async convertFileToBase64(file) {
            return new Promise((resolve, reject) => {
                const reader = new FileReader();
                reader.onloadend = () => resolve(reader.result);
                reader.onerror = reject;
                reader.readAsDataURL(file);
            });
        },
        // end region
        onExportFinanceData() {
            this.exportExcelListOrderVersionOffice(this.filter).then(response => {
                this.downloadFile(response)

            })

        },
        onLoadData() {
            this.getListOrderV2(this.filter).then(response => {
                this.filterdOrders = response.orders;
                this.totalPage = response.total;
                if (this.filterdOrders.length > 0) {
                    this.currentOrder = this.filterdOrders[0];
                }
                if (this.filterdOrders.length > 0) {
                    this.currentInfo = this.filterdOrders[0];
                }
                this.statistics = response.statistics
                this.filterdOrders.forEach(r => {
                    r.metaData = JSON.parse(r.metaData)
                })

                this.filterdOrders.forEach(v => {
                    if (v.createdDate) {
                        v.createdDate = moment(v.createdDate).format("DD/MM/YYYY hh:mm:ss")
                    }
                    if (v.pickingDate) {
                        v.pickingDate = moment(v.pickingDate).format("DD/MM/YYYY")
                    }
                    if (v.rollbackRequestDate) {
                        v.rollbackRequestDate = moment(v.rollbackRequestDate).format("DD/MM/YYYY hh:mm:ss")
                    }
                })

            })
        },
        onGetSuppliers() {
            this.getAllUserSupplier().then(response => {
                // console.log(response);
                this.userSuppliers = response.map(r => ({
                    label: r.fullName,
                    value: r.email
                }))

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
        },
        onViewThongTin(item) {
            this.currentInfo = item;
            this.$refs.mdThongTin.show();
        },

        onConvertStatus(status) {
            var html = "";
            switch (status) {
                case "TAO_MOI":
                    html = `<span class="badge badge-pill badge-primary">Tạo mới</span>`
                    // html = `<b-badge pill variant="primary">Tạo mới</b-badge>`;
                    break;
                case "CHAP_NHAN_DICH_VU":
                    html = `<span class="badge badge-pill badge-success">Chấp nhận</span>`
                    // html = `<b-badge pill variant="success">Chấp nhận</b-badge>`;
                    break;
                case "DA_SU_DUNG_DICH_VU":
                    html = `<span class="badge badge-pill badge-success">Đã sử dụng</span>`
                    // html = `<b-badge pill variant="success">Đã sử dụng</b-badge>`;
                    break;
                case "TU_CHOI_DICH_VU":
                    html = `<span class="badge badge-pill badge-warning">Từ chối</span>`
                    // html = `<b-badge pill variant="warning">Từ chối</b-badge>`;
                    break;
                case "YEU_CAU_HUY":
                    html = `<span class="badge badge-pill badge-danger">Yêu cầu hủy</span>`
                    // html = `<b-badge pill variant="danger">Yêu cầu hủy</b-badge>`;
                    break;
                case "DA_HUY":
                    html = `<span class="badge badge-pill badge-danger">Đã hủy</span>`
                    // html = `<b-badge pill variant="danger">Đã hủy</b-badge>`;
                    break;

            }
            return html;
        },
        downloadFile(base64String) {
            // Convert base64 string to a byte array
            const byteCharacters = atob(base64String);
            const byteNumbers = new Array(byteCharacters.length);
            for (let i = 0; i < byteCharacters.length; i++) {
                byteNumbers[i] = byteCharacters.charCodeAt(i);
            }
            const byteArray = new Uint8Array(byteNumbers);

            // Create a blob from the byte array
            const blob = new Blob([byteArray], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });

            // Create a download link and trigger the download
            const url = window.URL.createObjectURL(blob);
            const a = document.createElement('a');
            a.href = url;
            a.download = 'FinanceData.xlsx';
            document.body.appendChild(a);
            a.click();
            a.remove();
            window.URL.revokeObjectURL(url);
        },

    },


    computed: {
        ...mapGetters(["orders"])
    },
    mounted() {
        this.onGetSuppliers();
        this.onLoadData();
    },
    watch: {
        filter: {
            handler(newVal, oldVal) {
                console.log('Filter changed from', oldVal, 'to', newVal);
                this.onLoadData();
            },
            deep: true
        },
        '$refs.mdChat.isVisible'(newValue) {
            if (!newValue) {
                // Modal is closed
                this.currentChatSession = null;

                // Clear the interval
                if (this.updateInterval) {
                    clearInterval(this.updateInterval);
                    this.updateInterval = null;
                }
            }
        }

    },
    beforeDestroy() {
        // Clear the interval when the component is destroyed
        if (this.updateInterval) {
            clearInterval(this.updateInterval);
        }
    }
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