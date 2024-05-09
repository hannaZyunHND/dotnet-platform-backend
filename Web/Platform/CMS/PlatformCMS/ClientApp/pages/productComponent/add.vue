<template>
    <div class="productadd">
        <loading :active.sync="isLoading"
                 :height="35"
                 :width="35"
                 :color="color"
                 :is-full-page="fullPage"></loading>
        <b-tabs class="col-md-12" pills vertical>
            <b-tab title="1. Thông tin chung" active>
                <div class="row productedit">
                    <div class="col-sm-6 col-md-8">
                        <div class="card">
                            <div class="card-header">
                                Thông tin chính
                            </div>
                            <div class="card-body">
                                <b-form class="form-horizontal">
                                    <b-form-group label="Tiêu đề" label-for="input-1">
                                        <b-form-input id="input-1"
                                                      v-model="objRequest.title"
                                                      type="text"
                                                      required
                                                      placeholder="Tiêu đề"></b-form-input>
                                    </b-form-group>
                                    <b-form-group label="Mã sản phẩm">
                                        <b-form-input v-model="objRequest.code" placeholder="Mã sản phẩm" required></b-form-input>
                                    </b-form-group>
                                    <b-form-group label="Đường dẫn">
                                        <b-form-input v-model="objRequest.url" placeholder="Đường dẫn" required></b-form-input>
                                    </b-form-group>
                                </b-form>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6 col-md-4">
                        <div class="card">
                            <div class="card-header">
                                Đăng bài
                            </div>
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-6">
                                        <button class="btn btn-info btn-submit-form col-md-12 btncus" type="submit" @click="DoAddEdit()">
                                            <i class="fa fa-save"></i> Cập nhật
                                        </button>
                                    </div>
                                    <div class="col-md-6">
                                        <button class="btn btn-success col-md-12 btncus" type="button" @click="DoRefesh()">
                                            <i class="fa fa-refresh"></i> Làm mới
                                        </button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </b-tab>
        </b-tabs>
    </div>

</template>


<script>
    import "vue-loading-overlay/dist/vue-loading.css";
    import ClassicEditor from '@ckeditor/ckeditor5-build-classic';
    import msgNotify from "../../common/constant";
    import { mapGetters, mapActions } from "vuex";
    import Loading from "vue-loading-overlay";
 
    // Import component

    export default {
        name: "productcomponentaddedit",
        components: {
    
        },
        data() {
            return {
                isLoading: false,
                fullPage: false,
                preview: '/assets/img/unnamed.jpg',
                previews: '/assets/img/unnamed.jpg',
                color: "#007bff",
                editor: ClassicEditor,
                editor1: ClassicEditor,
                editorData: '<p>Content of the editor.</p>',
                editorConfig: {
                    allowedContent: true,
                    extraPlugins: "",

                },
                objRequest: {
                    Id: 0,
                    Code: '',
                    Status: 1,
                    Url: '',
                    Avatar: '',
                    AvatarArray: '',
                    Price: 0,
                    DiscountPrice: 0,
                    Warranty: '',
                    MetaTitle: '',
                    MetaKeyword: '',
                    MetaDescription: '',
                    SocialImage: '',
                    SocialTitle: '',
                    SocialDescription: '',
                    ManufacturerId: 0,
                    Unit: '',
                    Quantity: '',
                    PropertyId: 0
                },
                objRequestDetail: {
                    Title: '',
                    Description: '',
                    Content: '',
                    ViewCount: '',
                    Url: '',
                    PromotionInfo: '',
                    MetaTitle: '',
                    MetaKeyword: '',
                    MetaDescription: '',
                    SocialImage: '',
                    SocialTitle: '',
                    SocialDescription: '',
                    LanguageCode: '',
                    Catalog: '',
                    ProductId: 0
                },
                ListTags: [],
                Tags: [

                ],
                LanguageCodes: [

                ],
                ManufacturerIds: [

                ]
            };
        },
        created: {},
        components: {
            Loading
        },

        mounted() {
            if (this.$route.params.id > 0) {
                this.isLoading = true;
                let initial = this.$route.query.initial;
                initial = typeof initial != "undefined" ? initial.toLowerCase() : "";
                this.getProduct(this.$route.params.id).then(respose => {
                    this.objRequest = respose.data;
                });
                this.isLoading = false;
            }

            this.getAllLanguages().then(respose => {
                this.LanguageCodes = respose.listData;

            });
            this.getTagAll().then(respose => {
                this.Tags = respose.listData;

            });
            this.getAllManufacturers().then(response => {
                this.ManufacturerIds = response.listData;
            });
        },

        computed: {
            ...mapGetters(["product"], ["fileName"]),
        },

        methods: {
            ...mapActions(["updateProduct",
                "addProduct",
                "getProduct",
                "uploadFile",
                "getAllManufacturers",
                "getTagAll",
                "getAllLanguages",
                "addProductInLanguage"
            ]),
            DoAddEdit() {
                this.isLoading = true;
                if (this.objRequest.Id > 0) {
                    this.updateProduct(this.objRequest)
                        .then(response => {
                            if (response.success == true) {
                                this.$toast.success(response.message, {});
                                this.isLoading = false;
                            }
                            else {
                                this.$toast.error(response.message, {});
                                this.isLoading = false;
                            }

                        })
                        .catch(e => {
                            this.$toast.error(msgNotify.error + ". Error:" + e, {});
                            this.isLoading = false;
                        });
                } else {
                    let objProduct = {};
                    objProduct = this.objRequest;
                    //let tagsData = [];
                    //let lstTags = this.ListTags;
                    //for (let i = 0; i < lstTags.length; i++) {
                    //    let datatag = {};
                    //    datatag.TagId = lstTags[i].id;
                    //    datatag.TagMode = 0;
                    //    tagsData.push(datatag);
                    //}
                    //objProduct.TagInProduct = tagsData;
                    this.addProduct(objProduct)
                        .then(response => {
                            if (response.success == true) {
                                this.$toast.success(response.message, {});
                                this.objRequestDetail.ProductId = response.data.productID;
                                this.isLoading = false;
                            }
                            else {
                                this.$toast.error(response.message, {});
                                this.isLoading = false;
                            }
                        })
                        .catch(e => {
                            this.$toast.error(msgNotify.error + ". Error:" + e, {});
                            this.isLoading = false;
                        });
                }
            },
            DoAddDetail() {
                let check = this.objRequestDetail;
                //debugger-;
                this.addProductInLanguage(this.objRequestDetail)
                    .then(response => {
                        if (response.success == true) {
                            this.$toast.success(response.message, {});
                            this.isLoading = false;
                        }
                        else {
                            this.$toast.error(response.message, {});
                            this.isLoading = false;
                        }
                    })
                    .catch(e => {
                        this.$toast.error(msgNotify.error + ". Error:" + e, {});
                        this.isLoading = false;
                    });
            },
            openUpload() {
                document.getElementById('file-field').click()
            },
            updatePreview(e) {
                var reader, files = e.target.files
                if (files.length === 0) {
                    console.log('Empty')
                }
                this.selectedFile = files[0];
                reader = new FileReader();
                var data = new FormData();
                for (var x = 0; x < files.length; x++) {
                    data.append("file" + x, files[x]);
                }
                this.uploadFile(data).then(response => {
                    if (response.Success == true) {
                        reader.onload = (e) => {
                            this.preview = e.target.result;
                            this.objRequest.Avatar = response.data[0].name;
                            console.log(this.selectedFile);
                        }
                        reader.readAsDataURL(files[0])
                    }
                    else {
                        this.$toast.error(msgNotify.error + ". Error:" + e, {});
                        this.isLoading = false;
                    }
                }).catch(e => {
                    this.$toast.error(msgNotify.error + ". Error:" + e, {});
                    this.isLoading = false;
                });

            },
            openUploads() {
                document.getElementById('files-field').click()
            },
            updatePreviews(e) {
                var reader, files = e.target.files
                if (files.length === 0) {
                    console.log('Empty')
                }
                this.selectedFile = files[0];
                reader = new FileReader();
                var data = new FormData();
                for (var x = 0; x < files.length; x++) {
                    data.append("file" + x, files[x]);
                }
                this.uploadFile(data).then(response => {
                    if (response.Success == true) {
                        reader.onload = (e) => {
                            this.previews = e.target.result;
                            this.objRequest.AvatarArray = response.data[0].name;
                            console.log(this.selectedFile);
                        }
                        reader.readAsDataURL(files[0])
                    }
                    else {
                        this.$toast.error(msgNotify.error + ". Error:" + e, {});
                        this.isLoading = false;
                    }
                }).catch(e => {
                    this.$toast.error(msgNotify.error + ". Error:" + e, {});
                    this.isLoading = false;
                });
            }
        }
    };
</script>
<style>
</style>
