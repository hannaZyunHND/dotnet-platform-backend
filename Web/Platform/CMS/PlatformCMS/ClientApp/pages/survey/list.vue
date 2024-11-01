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
                                    <th>Answer_Id</th>
                                    <th>Email</th>
                                    <th>Thông tin khảo sát</th>
                                    <th>Thời gian</th>
                                    <th>Đánh giá</th>
                                    <th>Hành động</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr role="row" class="odd" v-for="item in results">
                                    <td>
                                        <span>{{ item.answer_Id }}</span>
                                    </td>
                                    <td>
                                        <span>Email: {{ item.email }}</span>
                                    </td>

                                    <td>
                                        <span>Title: {{ item.survey_Name }}</span>
                                    </td>
                                    <td>
                                        <span>{{ formatTime(item.time_To_Submit) }}</span>

                                    <td>
                                        <span>{{ item.total_Responses }}</span>
                                    </td>

                                    </td>
                                    <td>
                                        <b-button-group>
                                            <b-form-checkbox :checked="item.isConfirm"
                                                @change="acceptDeleteSurvey(item.id, item.isConfirm)">
                                                Delete
                                            </b-form-checkbox>
                                        </b-button-group>

                                        <b-button-group>
                                            <b-button v-b-modal.modal-1
                                                @click="showDetail(item.answer_Id)">Xem</b-button>
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
            <b-modal id="modal-1" title="Chi tiết khảo sát" v-if="detail_Result.length">
                <!-- Danh sách câu hỏi và các tùy chọn -->
                <ul v-for="(result, index) in detail_Result" :key="index" class="question-list mb-3">
                    <li class="center d-flex align-items-center" style="gap: 50px;">
                        <p><strong>{{ questions[result.questionId] || '' }}</strong></p>
                        <div class="options">
                            <b-form-radio-group v-model="result.selectedOption">
                                <b-form-radio v-for="option in options[result.questionId]" :key="option.option_id"
                                    :value="option.text">
                                    {{ option.text }}
                                </b-form-radio>
                            </b-form-radio-group>
                        </div>
                    </li>
                </ul>
            </b-modal>

        </div>
    </div>
    <!-- Tooltip Component -->

</template>
<script>
import "vue-loading-overlay/dist/vue-loading.css";
import { mapGetters, mapActions } from "vuex";
import Loading from "vue-loading-overlay";
import moment from 'moment'

export default {
    name: "Survey",
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
            results: [],
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

            questions: {},
            detail_Result: [],
            options: {},
            fields: [
                { key: 'questionText', label: 'Câu hỏi' },
                { key: 'options', label: 'Lựa chọn' },
                // Có thể thêm các trường khác nếu cần
            ],
        };
    },
    methods: {
        ...mapActions(["getAllSurvey", "getSurveyById", "getAllOptions", "getQuestionById"]),

        onLoadData() {
            const data = {
                page_index: this.filter.index,
                page_size: this.filter.size
            }
            this.getAllSurvey(data).then(response => {
                this.results = response.results;
                this.totalPage = response.totalCount;

            })
        },

        async showDetail(Id) {
            const response = await this.getSurveyById(Id);
            if (response) {
                this.detail_Result = response.responses;
                await this.loadAllOptions();
            } else {
                alert("cập nhật thất bại");
            }
        },

        async loadAllOptions() {
            const questionIds = this.detail_Result.map(result => result.questionId);
            for (let questionId of questionIds) {
                await this.onLoadQuestion(questionId);
                await this.onLoadOptions(questionId);
            }
        },

        async onLoadQuestion(questionId) {
            if (!this.questions[questionId]) {
                const response = await this.getQuestionById(questionId);
                if (response) {
                    this.$set(this.questions, questionId, response.text);
                }
            }
        },

        async onLoadOptions(questionId) {
            const response = await this.getAllOptions(questionId);
            if (response) {
                this.$set(this.options, questionId, response);
            }
        },

        formatTime(dateString) {
            return moment(dateString).format('h:mm A DD-MM-YYYY');
        },


    },
    mounted() {
        this.onLoadData();
    },
    watch: {
    },
};
</script>
<style scoped>
.question-list {
    list-style-type: none;
    /* Xóa dấu chấm cho danh sách */
    padding: 0;
    /* Xóa padding của danh sách */
    margin: 0;
    /* Xóa margin của danh sách */
}

.center {
    display: flex;
    /* Sử dụng flexbox */
    justify-content: space-between;
    /* Tách đều giữa câu hỏi và tùy chọn */
    align-items: center;
    /* Căn giữa theo chiều dọc */
}

.options {
    flex: 1;
    /* Tùy chọn sẽ chiếm không gian còn lại */
    text-align: left;
    /* Căn lề trái cho các tùy chọn */
}

.question-list p {
    margin: 0;
    /* Xóa margin của đoạn văn */
    text-align: left;
    /* Căn lề trái cho câu hỏi */
}

.list-unstyled {
    padding: 0;
    margin: 0;
}

/* Optional: Style the radio buttons to match the image more closely */
::v-deep .custom-radio {
    display: inline-flex;
    align-items: center;
    gap: 0.5rem;
}

.question-list {
    list-style: none;
    padding: 0;
}

.options {
    display: flex;
    flex-direction: column;
    margin-top: 8px;
}

.options b-form-radio {
    margin-right: 15px;
}

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