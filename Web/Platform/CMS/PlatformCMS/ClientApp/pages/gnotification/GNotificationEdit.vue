<template>
    <div>
        <b-card title="Thông tin Notification" class="mb-4">
            <b-form-group label="Tên Notification">
                <b-form-input v-model="objRequest.notificationName" placeholder="Nhập tên thông báo" />
            </b-form-group>

            <b-row>
                <b-col md="4">
                    <b-form-group label="Icon Notification">
                        <a @click="openImg('icon')">
                            <div style="width:200px;height:200px;display:flex" class="gallery-upload-file ui-sortable">
                                <div style="width:100%;height:auto;margin:0" class="r-queue-item ui-sortable-handle">
                                    <div style="width:100%"
                                        v-if="objRequest.notificationIcon != null && objRequest.notificationIcon.length > 0">
                                        <img alt="Ảnh lỗi" style="height:200px;width:200px"
                                            :src="objRequest.notificationIcon">
                                    </div>
                                    <div v-else>
                                        <i class="fa fa-picture-o"></i>
                                        <p>[Chọn ảnh]</p>
                                    </div>
                                </div>
                            </div>
                        </a>
                    </b-form-group>
                </b-col>
                <b-col md="8">
                    <b-form-group label="Mô tả Notification">
                        <b-form-textarea v-model="objRequest.notificationDescription" rows="3"
                            placeholder="Nhập mô tả..." />
                    </b-form-group>
                </b-col>
            </b-row>

            <b-form-group label="Nội dung Notification">
                <div class="mi-editor">
                    <MIEditor :contentEditor="objRequest.notificationDetail" @handleEditorInput="getOrSetData" />
                </div>
            </b-form-group>
        </b-card>

        <b-card title="Chi tiết gửi thông báo">
            <b-button variant="outline-primary" @click="addDetail" class="mb-3">
                <i class="fa fa-plus-circle"></i> Thêm dòng gửi
            </b-button>

            <b-card v-for="(detail, index) in objRequest.details" :key="index" class="mb-3" header="Chi tiết gửi">
                <b-row>
                    <b-col md="4">
                        <b-form-group label="Chọn nhóm khách hàng">
                            <treeselect :multiple="false" :options="customerGroupOptions"
                                v-model="detail.customerGroupDetailId" placeholder="Chọn nhóm" />
                        </b-form-group>
                    </b-col>

                    <b-col md="4">
                        <b-form-group label="Thời gian dự kiến gửi">
                            <input v-model="detail.pusingSceduleTime" type="datetime-local" class="form-control" />
                        </b-form-group>
                    </b-col>

                    <b-col md="3">
                        <b-form-group label="Trạng thái">
                            <div>
                                <label><input type="radio" :value="false" v-model="detail.isPushed" /> Chưa gửi</label>
                                <label class="ml-2"><input type="radio" :value="true" v-model="detail.isPushed" /> Đã
                                    gửi</label>
                            </div>
                        </b-form-group>
                    </b-col>

                    <b-col md="1" class="d-flex align-items-end">
                        <b-button variant="danger" @click="removeDetail(index)">Xóa</b-button>
                    </b-col>
                </b-row>
            </b-card>

            <div class="text-right mt-4">
                <b-button variant="success" @click="submit">
                    <i class="fa fa-save"></i> Lưu
                </b-button>
            </div>
        </b-card>
        <FileManager v-on:handleAttackFile="DoAttackFile" :miKey="mikey1" />
    </div>
</template>

<script>
import "vue-loading-overlay/dist/vue-loading.css";
import msgNotify from "./../../common/constant";
import { mapGetters, mapActions } from "vuex";
import Loading from "vue-loading-overlay";
// import the component
import Treeselect from '@riophae/vue-treeselect'
// import the styles
import '@riophae/vue-treeselect/dist/vue-treeselect.css'

import 'vue-select/dist/vue-select.css';

import FileManager from './../../components/fileManager/list'
import { slug, pathImg, unflatten, urlBase } from "../../plugins/helper";
// Import component
// Import component

import EventBus from "./../../common/eventBus";
import VueTagsInput from '@johmun/vue-tags-input';
import { router } from "../../router";
import moment from 'moment';
import MIEditor from '../../components/editor/MIEditor';

export default {
    name: "EditGNotification",
    components: { MIEditor, Treeselect, FileManager },
    data() {
        return {
            objRequest: {
                id: 0,
                notificationName: "",
                notificationIcon: "",
                notificationDescription: "",
                notificationPushLink: "",
                notificationDetail: "",
                details: []
            },
            customerGroupOptions: [],
            mikey1: 'mikey1',
            preview: '/assets/img/unnamed.jpg',

        };
    },
    created() {
        this.loadCustomerGroups();
        const id = parseInt(this.$route.params.id);
        if (id > 0) {
            this.$store.dispatch("getNotificationById", id).then(res => {

                this.objRequest = res;
            });
        }
    },
    methods: {
        ...mapActions(["createNotification", "updateNotification", "getCustomerGroups", "getNotificationById"]),

        loadCustomerGroups() {
            this.getCustomerGroups({ pageIndex: 1, pageSize: 1000 }).then(res => {
                console.log(res) // chỗ này trả về undefined nhưng mà lại được gọi và có kết quả ạ
                // console.log(res)
                this.customerGroupOptions = res.items.map(x => ({ id: x.id, label: x.name }));
            });
        },

        getOrSetData(value) {
            this.objRequest.notificationDetail = value;
        },

        openImg(img) {
            this.choseImg = img;
            EventBus.$emit(this.mikey1, ''); // '': select one, 'multi': select multi file
        },
        DoAttackFile(value) {
            let vm = this;
            if (this.choseImg == "icon") {
                vm.objRequest.notificationIcon = value[0].path;
            }
        },
        addDetail() {
            this.objRequest.details.push({
                customerGroupDetailId: null,
                pusingSceduleTime: null,
                isPushed: false
            });
        },

        removeDetail(index) {
            this.objRequest.details.splice(index, 1);
        },

        submit() {
            const action = this.objRequest.id > 0 ? "updateNotification" : "createNotification";
            this.$store.dispatch(action, this.objRequest).then(() => {
                this.$toast.success("Lưu thành công");
                this.$router.push(`/admin/gnotification/list`);
            });
        }
    }
};
</script>

<style scoped>
.treeselect__control {
    min-height: 38px;
}

.mi-editor {
    border: 1px solid #ccc;
    border-radius: 4px;
    padding: 10px;
    background-color: white;
}
</style>