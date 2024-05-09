<template>
    <div class="productadd">
        <loading :active.sync="isLoading"
                 :height="35"
                 :width="35"
                 :color="color"
                 :is-full-page="fullPage"></loading>
        <b-tabs class="col-md-12" pills>
            <b-tab title="1. Thông tin chung" active>
                <div class="row productedit">
                    <div class="col-sm-6 col-md-8">
                        <div class="card">
                            <div class="card-header">
                                Thông tin chính
                            </div>
                            <div class="card-body">
                                <b-form class="form-horizontal">
                                    <b-form-group label="Tên linh kiện">
                                        <b-form-input v-model="objRequest.name" placeholder="Tên sản phẩm" required></b-form-input>
                                    </b-form-group>
                                    <b-form-checkbox v-model="objRequest.isShow" style="padding-bottom:7px">
                                        Hiển thị
                                    </b-form-checkbox>
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
    //import ClassicEditor from '@ckeditor/ckeditor5-build-classic';
    import msgNotify from "../../common/constant";
    import { mapGetters, mapActions } from "vuex";
    import Loading from "vue-loading-overlay";

    // import the component
    import Treeselect from '@riophae/vue-treeselect'
    // import the styles
    import '@riophae/vue-treeselect/dist/vue-treeselect.css'

    import 'vue-select/dist/vue-select.css';
    import { unflatten, slug, pathImg, urlBase } from "../../plugins/helper";

    import EventBus from "./../../common/eventBus";
    import moment from 'moment';
    import VoerroTagsInput from './VoerroTagsInput';
    import FileManager from './../../components/fileManager/list'
    import MIEditor from '../../components/editor/MIEditor';
    // Import component
    import { ASYNC_SEARCH, LOAD_ROOT_OPTIONS } from '@riophae/vue-treeselect'

    const simulateAsyncOperation = fn => {
        setTimeout(fn, 2000)
    }
    export default {
        name: "productaddedit",
        data() {
            return {
                mikey1: 'mikey1',
                isLoading: false,
                fullPage: false,
                color: "#007bff",
                isLoadLang: false,
                //editor: ClassicEditor,
                //editor1: ClassicEditor,
                editorData: '<p>Content of the editor.</p>',
                currentSort: "Id",
                currentSortDir: "asc",
                productName: "",
                currentPage: 1,
                pageSize: 10,
                loading: true,
                langSelected: "",
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
                selected: [],
                objLocations: {
                    locationId: 0,
                },
                objCombo: {
                    name: ''
                },
                Locations: [],
                editorConfig: {
                    allowedContent: true,
                    extraPlugins: "",

                },
                statusNoindex: false,
                statusCanonical: false,
                valueCanonical: "",
                objRequest: {
                    id: 0,
                    name: "",
                    isShow: true,
                },
                
            };
        },
        created: {},
        components: {
            MIEditor,
            Loading,

            Treeselect,

            FileManager,
            "v-tags-input": VoerroTagsInput
        },
        //mounted() {

        //},



        computed: {
            ...mapGetters(["product", "fileName"]),
        },
        created() {
            if (this.$route.params.id > 0) {

                var $this = this;
                this.isLoading = true;
                let initial = this.$route.query.initial;
                initial = typeof initial != "undefined" ? initial.toLowerCase() : "";

                try {
                    this.getProductComponentById(this.$route.params.id).then(respose => {
                        this.objRequest = respose.data;
                    });
                } catch(ex) {
                    this. objRequest = {
                        id: 0,
                        name: "",
                        isShow: true,
                    }
                }
                this.isLoading = false;
            }
            this.onChangePaging();

        },
        destroyed() {
            EventBus.$off('FileSelected');
        },
        methods: {
            ...mapActions([
                "getProductComponentById",
                "updateProductComponent",
            ]),
            getUser() {
                //debugger-
                var data = JSON.parse(localStorage.getItem('currentUser'));
                return JSON.parse(localStorage.getItem('currentUser'));
            },

            onChangePaging() {
                this.isLoading = true;
                let initial = this.$route.query.initial;
                initial = typeof initial != "undefined" ? initial.toLowerCase() : "";
                this.isLoading = false;
            },
            StartLoadLang() {
                this.isLoadLang = true;
            },
            removeImg(index) {
                this.ListImg.splice(index, 1);
            },
            DoAddEdit() {
                this.isLoading = true;
                this.updateProductComponent(this.objRequest)
                    .then(response => {
                        if (response.success == true) {
                            this.$toast.success(response.message, {});
                        }
                        else {
                            this.$toast.error(response.message, {});
                        }
                        this.$router.push({
                            path: "/admin/productComponent/edit/" + response.id,
                        });
                        this.isLoading = false;

                    })
                    .catch(e => {
                        this.$toast.error(msgNotify.error + ". Error:" + e, {});
                        this.isLoading = false;
                    });
            },
        },
        watch: {
            isLoadLang: function (val) {
                this.onLoadLang();
            },
            //ArticleValues: function (val) {
            //    this.
            //},

        },
    };
</script>
<style>
    /* The input */
    .tags-input {
        display: flex;
        flex-wrap: wrap;
        align-items: center;
    }

        .tags-input input {
            flex: 1;
            background: transparent;
            border: none;
        }

            .tags-input input:focus {
                outline: none;
            }

            .tags-input input[type="text"] {
                color: #495057;
            }

    .tags-input-wrapper-default {
        padding: .5em .25em;
        background: #fff;
        border: 1px solid transparent;
        border-radius: .25em;
        border-color: #dbdbdb;
    }

        .tags-input-wrapper-default.active {
            border: 1px solid #8bbafe;
            box-shadow: 0 0 0 0.2em rgba(13, 110, 253, 0.25);
            outline: 0 none;
        }

    /* The tag badges & the remove icon */
    .tags-input span {
        margin-right: 0.3em;
    }

    .tags-input-remove {
        cursor: pointer;
        position: absolute;
        display: inline-block;
        right: 0.3em;
        top: 0.3em;
        padding: 0.5em;
        overflow: hidden;
    }

        .tags-input-remove:focus {
            outline: none;
        }

        .tags-input-remove:before, .tags-input-remove:after {
            content: '';
            position: absolute;
            width: 75%;
            left: 0.15em;
            background: #5dc282;
            height: 2px;
            margin-top: -1px;
        }

        .tags-input-remove:before {
            transform: rotate(45deg);
        }

        .tags-input-remove:after {
            transform: rotate(-45deg);
        }

    /* Tag badge styles */
    .tags-input-badge {
        position: relative;
        display: inline-block;
        padding: 0.25em 0.4em;
        font-size: 75%;
        font-weight: 700;
        line-height: 1;
        text-align: center;
        white-space: nowrap;
        vertical-align: baseline;
        border-radius: 0.25em;
        overflow: hidden;
        text-overflow: ellipsis;
    }

    .tags-input-badge-pill {
        padding-right: 1.25em;
        padding-left: 0.6em;
        border-radius: 10em;
    }

    .tags-input-badge-selected-default {
        color: #212529;
        background-color: #f0f1f2;
    }

    /* Typeahead */
    .typeahead-hide-btn {
        color: #999 !important;
        font-style: italic;
    }

    /* Typeahead - badges */
    .typeahead-badges > span {
        cursor: pointer;
        margin-right: 0.3em;
    }

    /* Typeahead - dropdown */
    .typeahead-dropdown {
        list-style-type: none;
        padding: 0;
        margin: 0;
        position: absolute;
        width: 100%;
        z-index: 1000;
    }

        .typeahead-dropdown li {
            padding: .25em 1em;
            cursor: pointer;
        }

    /* Typeahead elements style/theme */
    .tags-input-typeahead-item-default {
        color: #fff;
        background-color: #343a40;
    }

    .tags-input-typeahead-item-highlighted-default {
        color: #fff;
        background-color: #007bff;
    }
</style>
