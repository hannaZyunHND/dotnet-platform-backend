
<template>
    <div class="list-data">
        <loading :active.sync="isLoading"
                 :height="35"
                 :width="35"
                 :color="color"
                 :is-full-page="false"></loading>
        <b-card header-tag="header" class="card-filter"
                footer-tag="footer">
            <div>
                <b-row class="form-group">
                    <b-col md="3">
                        <b-form-input type="text" v-model="searchKey" v-on:keyup.enter="onChangePaging()" placeholder="Tìm kiếm theo tên"></b-form-input>
                    </b-col>
                </b-row>
            </div>
        </b-card>
        <div class="card card-data">
            <div class="card-body">
                <div role="toolbar" class=" mb-2" aria-label="Toolbar with button groups and dropdown menu">
                    <div role="group" class="btn-group">
                        <router-link class="btn btn-success" :to="{ path: 'add'}" target='_blank'><i class="fa fa-plus"></i> Thêm mới</router-link>
                    </div>
                    <div class="mx-1 btn-group mi-paging">
                        <b-pagination v-model="currentPage"
                                      :total-rows="total"
                                      :per-page="pageSize"></b-pagination>
                        <div class="mx-1" style="padding-top:5px">Số lượng : {{total}}</div>
                    </div>
                </div>
                <div class="table-responsive">
                    <div class="dataTables_wrapper dt-bootstrap4 no-footer">
                        <div class="clear"></div>
                        <table class="table data-thumb-view dataTable no-footer" role="grid">
                            <thead class="table table-centered table-nowrap">
                                <tr role="row">
                                    <th class="sorting_desc">Tên linh kiện</th>
                                    <th class="sorting text-center">Trạng thái</th>
                                    <th class="sorting text-center">Thao tác</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr role="row" class="odd" v-for="item in ListProduct">
                                    <td style="width:200px;height:auto">
                                        <p>{{item.name}}</p>
                                    </td>
                                    <td class="text-center">
                                        <span v-if="item.isShow ==true" class="badge bg-success">Hiển thị</span>
                                        <span v-if="item.isShow ==false" class="badge bg-danger">Ẩn</span>
                                    </td>
                                    <td class="product-action text-center">
                                       <router-link v-b-tooltip.hover title="Sửa linh kiện" :to="{path: 'edit/'+ item.id}" target='_blank'>
                                            <span class="action-edit"><i class="fa fa-edit"></i></span>
                                        </router-link>
                                        <span class="action-delete"><a v-b-tooltip.hover title="Xóa linh kiện" @click="remove(item.id)"><i style="color:red" class="fa fa-trash"></i></a></span>
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
    import msgNotify from "./../../common/constant";
    import { unflatten, pathImg } from "../../plugins/helper";
    import { mapGetters, mapActions } from "vuex";
    import Loading from "vue-loading-overlay";

    import Treeselect from '@riophae/vue-treeselect';
    import '@riophae/vue-treeselect/dist/vue-treeselect.css';
    const fields = [
        { key: "id", label: "Id" },
        { key: "name", label: "Tên linh kiện" },
    ];

    export default {
        name: "productComponent",
        components: {
            Loading,
            Treeselect
        },
        data() {
            return {
                valueConsistsOf: "BRANCH_PRIORITY",
                isLoading: false,
                fields,
                
                messeger: "",
                currentSort: "Id",
                currentSortDir: "desc",
                searchKey: "",
                searchStatus: 0,
                SearchLanguageCode: "vi-VN     ",
                ListStatus: [],
                SearchZoneId: [],
                SearchPromotionId: 0,
                SearchParrentVoucher: 0,
                ListZone: [],
                Language: [

                ],
                Promotions: [],
                voucherKey: "",

                IsInstallment: false,

                ListParrentVoucher: [],
                ListVoucher: [],

                ListVoucherChecked: [],
                ListProductChecked: [],
                ListProduct: [],
                currentPage: 1,
                pageSize: 10,
                color: "#007bff",
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
                },
                total: 1
            };
        },
        methods: {
            ...mapActions(["getProductComponents", "updateSort","deleteProductComponentById"]),

            RemoveItem(index) {
                this.ListProductChecked.splice(index, 1)
            },
            updateSorts(event, id) {
                debugger
                var sort = event.target.value;
                var obj = {};
                obj.id = id;
                obj.sortNew = sort;
                this.updateSort(obj).then(response => {
                    if (response.success == true) {
                        this.$toast.success(response.message, {});
                        //this.onChangePaging();
                        this.isLoading = false;
                    } else {
                        this.$toast.error(response.message, {});
                        this.isLoading = false;
                    }
                }).catch(e => {
                    this.$toast.error(msgNotify.error + ". Error:" + e, {});
                });
            },
            onChangePaging() {
                this.isLoading = true;
                let initial = this.$route.query.initial;
                initial = typeof initial != "undefined" ? initial.toLowerCase() : "";
                this.getProductComponents({
                    pageIndex: this.currentPage,
                    pageSize: this.pageSize,
                    keyword: this.searchKey,
                    sortDir: this.currentSortDir,
                    sortBy: this.currentSort,
                }).then(respose => {
                    try {
                        this.ListProduct = respose.listData;
                        this.total = respose.total;
                    }
                    catch (ex) {

                    }
                });
                this.isLoading = false;
            },
     
            sortor: function (s) {
                if (s === this.currentSort) {
                    this.currentSortDir = this.currentSortDir === "asc" ? "desc" : "asc";
                }
                this.currentSort = s;
                this.onChangePaging();
            },
            remove: function (id) {
                if (confirm("Bạn có thực sự muốn xóa ?")) {
                    this.deleteProductComponentById(id)
                        .then(response => {
                            if (response.success == true) {
                                this.$toast.success(response.message, {});
                                this.onChangePaging();
                                this.isLoading = false;
                            } else {
                                this.$toast.error(response.message, {});
                                this.isLoading = false;
                            }
                        })
                        .catch(e => {
                            this.$toast.error(msgNotify.error + ". Error:" + e, {});
                        });
                }
            }
        },
        created() {
            this.onChangePaging();
            //this.exportPriceLocation();
        },
        watch: {
            currentPage: function (newVal) {
                this.currentPage = newVal;
                this.onChangePaging();
            },
            searchStatus: function (newVal) {
                this.currentPage = 1;
                this.onChangePaging();
            },
            SearchZoneId: function (newVal) {
                this.currentPage = 1;
                this.onChangePaging();
            },
            SearchLanguageCode: function () {
                this.currentPage = 1;
                this.onChangePaging();
            },
            SearchPromotionId: function () {
                this.currentPage = 1;
                this.onChangePaging();
            },
            IsInstallment: function () {
                this.currentPage = 1;
                this.onChangePaging();
            },
            SearchParrentVoucher: function () {

                this.GetDataCouponsChildParrentId();
            }
        }
    };
</script>

<style>
</style>
