
<template>
    <div class="list-data">
        <b-card header-tag="header" class="card-filter"
                footer-tag="footer">
            <div>
                <b-col md="12">
                    <b-row class="form-group">
                        <b-col md="4">
                            <b-form-input @keyup.13="onChangePaging()" v-model="SearchKeyword" type="text" placeholder="Tìm kiếm theo tên"></b-form-input>
                        </b-col>
                        <!--<b-col md="2">
                            <select v-model="SearchStatus" class="form-control">
                                <option v-for="item in ListStatus" :value="item.key">
                                    {{item.value}}
                                </option>
                            </select>
                        </b-col>-->
                        <b-col md="2">
                            <select v-model="SearchType" class="form-control">
                                <option v-for="item in ListType" :value="item.key">
                                    {{item.value}}
                                </option>
                            </select>
                        </b-col>
                        <b-col md="1">
                            <b-btn variant="info" class="col-lg-12"><i class="fa fa-search" aria-hidden="true"></i></b-btn>
                        </b-col>
                        <b-col md="1">
                            <b-btn variant="primary" class="col-lg-12"><i class="fa fa-refresh" aria-hidden="true"></i></b-btn>
                        </b-col>
                        <!--<b-col md="2">
                            <b-btn v-b-toggle.collapse1 variant="primary"><i class="fa fa-angle-double-down" aria-hidden="true"></i></b-btn>
                        </b-col>-->
                    </b-row>
                </b-col>
                <!--<b-collapse id="collapse1" class="mt-2">
                    <b-card>
                        <p class="card-text">Collapse contents Here</p>
                        <b-btn v-b-toggle.collapse1_inner size="sm">Toggle Inner Collapse</b-btn>
                        <b-collapse id=collapse1_inner class="mt-2">
                            <b-card>Hello!</b-card>
                        </b-collapse>
                    </b-card>
                </b-collapse>-->
            </div>
        </b-card>
        <div class="card card-data">
            <div class="card-body">
                <div class="table-responsive">
                    <div class="dataTables_wrapper dt-bootstrap4 no-footer">
                        <div class="clear"></div>
                        <table class="table data-thumb-view dataTable no-footer" role="grid">
                            <thead class="thead-dark table table-centered table-nowrap">
                                <tr role="row">
                                    <th><input type="checkbox"></th>
                                    <th>Sản phẩm</th>
                                    <th class="sorting">Hình ảnh</th>
                                    <th>Khách hàng</th>
                                    <th>Thông tin</th>
                                    <th>Ngày tạo</th>
                                    <th class="text-center">Trạng thái</th>
                                    <th style="width:200px;">Thao tác</th>
                                    <th>Hỗ trợ</th>
                                </tr>
                            </thead>
                            <tbody>
                                <template v-for="item in ListProduct">
                                    <tr role="row" class="odd">
                                        <td class="dt-checkboxes-cell"><input type="checkbox" class="dt-checkboxes"></td>
                                        <td style="max-width:300px">{{item.productName}}</td>
                                        <td class="product-img">
                                            <img style="width:100px;height:auto" class="img-thumbnail" :src="pathImgs(item.image)" alt="Ảnh lỗi">
                                        </td>
                                        <td style="max-width:300px">
                                            <p>Tên: {{item.fullName}}</p>
                                        </td>
                                        <td>
                                            <p v-if="item.phoneNumber != null && item.phoneNumber.length >0">Phone: {{item.phoneNumber}}</p>
                                            <p v-if="item.email != null && item.email.length >0">Mail: {{item.email}}</p>
                                        </td>
                                        <td v-html="format_date(item.createDate)"></td>
                                        <td class="text-center">
                                            <span v-if="item.type==1" class="badge bg-info">Bán lại</span>
                                            <span v-if="item.type==2" class="badge bg-success"> Đổi mới</span>
                                        </td>

                                        <td class="text-left">
                                            <span v-if="item.status !=2" class="action-show"><a v-b-tooltip.hover title="Đã hỗ trợ" @click="updateStatus(item,true)"><i style="color:green; margin-right:5px;" class="fa fa-check-circle"></i>Đã hỗ trợ</a></span>
        <!--<span v-if="item.status !=3" class="action-hidden"><a v-b-tooltip.hover title="Loại bỏ" @click="updateStatus(item,3)"><i style="color:gold" class="fa fa-circle-o"></i></a></span>-->
                                            <br><span v-b-tooltip.hover @click="edit(item)" class="action-edit"> <i class="fa fa-eye" style="margin-right: 5px;"></i>Xem</span>
                                        </td>
                                        <td class="text-center">
                                            <span v-if="item.isSupported==true" class="badge bg-success">Đã hỗ trợ</span>
                                            <span v-if="item.isSupported==false" class="badge bg-info"> Chưa hỗ trợ</span>
                                        </td>
                                    </tr>
                                </template>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
        <b-modal size="lg" id="edit" title="Thông tin chi tiết" hide-footer>
            <div class="row">
                <div class="col-md-12 col-xs-12" style="margin-bottom:10px" v-if="objRequest.type ==1">
                    <b style="color:green">{{objRequest.productName}}</b>
                </div>
                <div class="col-md-12 col-xs-12" v-if="objRequest.type ==1">
                    <img style="width:300px;height:350px" :src="pathImgs(objRequest.image)" alt="Ảnh lỗi">
                </div>
                <div class="col-md-12 col-xs-12 row" v-if="objRequest.type ==2">
                    <div class="col-md-5 col-xs-5 col-5">
                        <b style="color:green">{{objRequest.productName}}</b><br />
                        <b>Giá thu cũ:</b> <b style="color:red">{{objRequest.dealPriceStr}}</b> 
                        <img style="width:300px;height:350px" :src="pathImgs(objRequest.image)" alt="Ảnh lỗi">
                    </div>
                    <div class="col-md-2 col-xs-2 col-2 text-center" style="align-self:center;">
                        <i class="fa fa-forward" style="color:green; font-size:30px" aria-hidden="true"></i>
                    </div>
                    <div class="col-md-5 col-xs-5 col-5">
                        <b style="color:green">{{objRequest.newProductExchange.name}}</b><br />
                        <b>Giá máy mới:</b> <b style="color:red">{{objRequest.newProductPriceSTR}}</b>  
                        <img style="width:300px;height:350px" :src="pathImgs(objRequest.newProductExchange.avatar)" alt="Ảnh lỗi">
                    </div>
                </div>
                <div class="col-md-6 col-xs-12" style="margin-bottom:10px">
                    <b>Trạng thái:</b> <span v-if="objRequest.type==1" class="badge bg-info">Bán lại</span><span v-if="objRequest.type==2" class="badge bg-success"> Đổi mới</span>
                </div>

                <div class="col-md-6 col-xs-12" style="margin-bottom:10px">
                    <b>Họ Tên:</b> {{objRequest.fullName}}
                </div>
                <div class="col-md-6 col-xs-12" style="margin-bottom:10px">
                    <b>Số điện thoại:</b> {{objRequest.phoneNumber}}
                </div>
                <div class="col-md-6 col-xs-12" style="margin-bottom:10px">
                    <b>Email:</b> {{objRequest.email}}
                </div>
                <div class="col-md-6 col-xs-12" style="margin-bottom:10px" v-if="objRequest.type==1">
                    <b> Giá tham khảo:</b> <b style="color:red">{{objRequest.priceRefer}}</b>
                </div>
                <div class="col-md-6 col-xs-12" style="margin-bottom:10px" v-if="objRequest.type==2">
                    <b> Giá bù trừ:</b> <b style="color:red">{{objRequest.supportPriceSTR}}</b>
                </div>
                <div class="col-md-6 col-xs-12" style="margin-bottom:10px">
                    <b>Ngày tạo:</b>  {{objRequest.createDate}}
                </div>
            </div>
        </b-modal>
    </div>
</template>
<script>
    import "vue-loading-overlay/dist/vue-loading.css";
    import msgNotify from "./../../common/constant";
    import { mapGetters, mapActions } from "vuex";
    import Loading from "vue-loading-overlay";
    import moment from 'moment';
    import { pathImg } from "../../plugins/helper";
    export default {
        name: "comments",
        components: {
            Loading
        },
        data() {
            return {
                isLoading: false,

                messeger: "",
                currentSort: "Id",
                currentSortDir: "asc",
                SearchKeyword: "",
                SearchType: 0,
                SearchStatus: 1,
                currentPage: 1,
                pageSize: 10,
                loading: true,

                objRequest: {

                },
                objUser: {

                },
                ListContact: [],
                ListType: [],
                ListProduct: [],

                bootstrapPaginationClasses: {
                    ul: "pagination",
                    li: "page-item",
                    liActive: "active",
                    liDisable: "disabled",
                    button: "page-link"
                },
                customLabels: {
                    first: "First",
                    prev: "Previous",
                    next: "Next",
                    last: "Last"
                }
            };
        },
        created() {
            this.ListType = [
                {
                    "key": "0",
                    "value":"Tất cả"
                },
                {
                    "key": "2",
                    "value": "Đổi mới"
                },
                {
                    "key": "1",
                    "value": "Thu cũ"
                },
            ];

        },

        methods: {
            ...mapActions(["getProductRenewOld", "updateStatusProductOldRenewal"]),
            pathImgs(path) {
                return pathImg(path);
            },
            format_date(value) {
                if (value) {
                    return moment(String(value)).format('DD/MM/YYYY HH:mm')
                }
            },

            urlView(link) {
                
                return pathImg + link;
            },
            onChangePaging() {
                this.isLoading = true;
                let initial = this.$route.query.initial;
                initial = typeof initial != "undefined" ? initial.toLowerCase() : "";
                this.getProductRenewOld({
                    keyword: this.SearchKeyword,
                    pageIndex: this.currentPage,
                    pageSize: this.pageSize,
                    sortBy: this.currentSort,
                    sortDir: this.currentSortDir,
                    type: this.SearchType,
                    status: this.SearchStatus,
                }).then(respose => {
                    try {
                        this.ListProduct = respose.listData;
                    }
                    catch (ex) {

                    }
                });
                this.isLoading = false;
            },
            edit(obj) {
                //debugger-
                this.objRequest = Object.assign({}, obj);;

                this.$bvModal.show('edit');
            },
            toggeter(id) {
                //debugger-
                var str = "tr[data-x-id='" + id + "']";
                var menu = document.querySelectorAll(str).forEach(function (item) {
                    item.classList.toggle('hidden');
                });
            }
            ,
            sortor: function (s) {
                if (s === this.currentSort) {
                    this.currentSortDir = this.currentSortDir === "asc" ? "desc" : "asc";
                }
                this.currentSort = s;
                this.onChangePaging();
            },
            updateStatus: function (item, status) {
                debugger;
                item.isSupported = status;
                let initial = this.$route.query.initial;
                initial = typeof initial != "undefined" ? initial.toLowerCase() : "";
                this.updateStatusProductOldRenewal(item)
                    .then(response => {
                        if (response.Success == true) {
                            this.$toast.success(response.Message, {});
                            this.onChangePaging();
                            this.isLoading = false;
                        } else {
                            this.$toast.error(response.Message, {});
                            this.isLoading = false;
                        }
                    })
                    .catch(e => {
                        this.$toast.error(msgNotify.error + ". Error:" + e, {});
                    });
            }
        },
        computed: {
            ...mapGetters(["contacts"])
        },
        mounted() {
            this.onChangePaging();

        },
        watch: {
            currentPage: function (newVal) {
                this.currentPage = newVal;
                this.onChangePaging();
            },
            SearchStatus: function () {
                this.currentPage = 1;
                this.onChangePaging();
            },
            SearchType: function () {
                this.currentPage = 1;
                this.onChangePaging();
            },
        }
    };
</script>

