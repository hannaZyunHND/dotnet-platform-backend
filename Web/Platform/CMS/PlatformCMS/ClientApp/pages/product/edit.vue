<template>
    <div class="productadd">
        <loading :active.sync="isLoading" :height="35" :width="35" :color="color" :is-full-page="fullPage"></loading>
        <b-tabs class="col-md-12" pills>
            <b-tab title="1. Thông tin chung" active>
                <div class="row productedit">
                    <div class="col-md-12">
                        <div class="card">
                            <div class="card-header">
                                Đăng bài
                            </div>
                            <div class="card-body">
                                <div class="row">
                                    <div class="col-md-6">
                                        <button class="btn btn-info btn-submit-form col-md-12 btncus" type="submit"
                                            @click="DoAddEdit()">
                                            <i class="fa fa-save"></i> Cập nhật
                                        </button>
                                    </div>
                                    <div class="col-md-6">
                                        <button class="btn btn-success col-md-12 btncus" type="button"
                                            @click="DoRefesh()">
                                            <i class="fa fa-refresh"></i> Làm mới
                                        </button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-6 col-md-12">

                        <div class="card" hidden>
                            <div class="card-header">
                                Phân loại sản phẩm
                            </div>
                            <div class="card-body">
                                <b-form-group label="Mô tả thời gian">
                                    <!--<Datepicker v-model="demoDate" />-->
                                    <b-form-input v-model="objRequest.ngayDem" type="text"
                                        placeholder="số ngày và số đêm" required></b-form-input>
                                </b-form-group>
                                <b-form-group label="Ngày bắt đầu">
                                    <!--<Datepicker v-model="demoDate" />-->
                                    <b-form-input v-model="objRequest.ngayBatDau" type="date" placeholder=""
                                        required></b-form-input>
                                </b-form-group>
                                <b-form-group label="Ngày kết thúc">
                                    <!--<Datepicker v-model="demoDate" />-->
                                    <b-form-input v-model="objRequest.ngayKetThuc" type="date" placeholder=""
                                        required></b-form-input>
                                </b-form-group>
                                <b-form-group label="Phương tiện">
                                    <treeselect :multiple="true" :flat="false" :options="ListPhuongTien"
                                        placeholder="Chọn phương tiện" v-model="requestPhuongTien" />
                                </b-form-group>
                                <b-form-group label="Nhà cung cấp">
                                    <v-select v-model="objRequest.manufacturerId" :options="ManufacturerIds"
                                        :reduce="x => x.id" label="name" placeholder="Chọn nhà cung cấp"></v-select>
                                </b-form-group>
                                <b-form-group label="Đơn vị">
                                    <b-form-input v-model="objRequest.unit" placeholder="Đơn vị"
                                        required></b-form-input>
                                </b-form-group>
                                <b-form-group label="Số lượng">
                                    <b-form-input v-model="objRequest.quantity" placeholder="Số lượng" required
                                        type="number"></b-form-input>
                                </b-form-group>
                                <b-form-group label="Mã Voucher">
                                    <b-form-input v-model="objRequest.voucher" placeholder="Mã Voucher"
                                        required></b-form-input>
                                </b-form-group>
                                <b-form-group label="Ngày hết hạn khuyến mại">
                                    <b-form-input type="date" v-model="objRequest.exprirePromotion"
                                        placeholder="Ngày hết hạn khuyến mại"></b-form-input>
                                </b-form-group>
                                <b-form-group>
                                    <b-form-checkbox v-model="objRequest.isInstallment"> Trả góp</b-form-checkbox>
                                </b-form-group>
                                <b-form-group>
                                    <b-form-checkbox v-model="objRequest.vat"> Có VAT</b-form-checkbox>
                                </b-form-group>
                                <b-form-group>
                                    <label class="typo__label">Chọn thuộc tính</label>
                                    <treeselect :multiple="true" :flat="true" :options="Properties"
                                        placeholder="Xin mời bạn lựa chọn thuộc tính" v-model="ListProperty" />
                                </b-form-group>
                                <b-form-group label="Chính sách bảo hành">
                                    <select v-model="objRequest.guarantee" class="form-control">
                                        <option value="">Chọn chính sách</option>
                                        <option :value="item.key" v-for="item in ListGuarantee">
                                            {{ item.value }}
                                        </option>
                                    </select>
                                </b-form-group>
                                <b-form-group label="Loại linh kiện">
                                    <select v-model="objRequest.productCpnId" class="form-control">
                                        <option value="0">Chọn</option>
                                        <option :value="item.id" v-for="item in ListProductComponent">
                                            {{ item.name }}
                                        </option>
                                    </select>
                                </b-form-group>
                            </div>


                        </div>

                    </div>
                    <div class="col-sm-6 col-md-12">
                        <div class="card">
                            <div class="card-header">
                                Thông tin chính
                            </div>
                            <div class="card-body">
                                <b-form class="form-horizontal">
                                    <b-form-group label="Tiêu đề">
                                        <b-form-input v-model="objRequest.name" placeholder="Tên sản phẩm"
                                            required></b-form-input>
                                    </b-form-group>
                                    <b-form-group label="Chọn danh mục">
                                        <treeselect :multiple="true" :flat="true" :options="ZoneOptions"
                                            placeholder="Xin mời bạn lựa chọn danh mục" v-model="ZoneValues"
                                            :default-expanded-level="Infinity" />
                                    </b-form-group>
                                    <b-form-group label="Chọn điểm đến">
                                        <treeselect :multiple="true" :flat="true" :options="DiemDenOptions"
                                            placeholder="Xin mời bạn lựa chọn điểm đến" v-model="DiemDenValues"
                                            :default-expanded-level="Infinity" />
                                    </b-form-group>
                                    <b-form-group label="Chọn Tag">
                                        <treeselect :multiple="true" :flat="true" :options="TagOptions"
                                            placeholder="Xin mời bạn lựa chọn tag" v-model="TagValues"
                                            :default-expanded-level="Infinity" />
                                    </b-form-group>
                                    <b-form-group label="Note khi đặt hàng">
                                        <treeselect :multiple="true" :flat="true" :options="BookingNotesOptions"
                                            placeholder="Xin mời bạn lựa chọn tag" v-model="NoteValues"
                                            :default-expanded-level="Infinity" />
                                    </b-form-group>
                                    <b-form-group label="Giảm giá sản phẩm">
                                        <treeselect :multiple="true" :flat="true" :options="CouponZoneOptions"
                                            placeholder="Xin mời bạn lựa chọn tag" v-model="CouponZoneValues"
                                            :default-expanded-level="Infinity" />
                                    </b-form-group>
                                    <b-form-group label="Chọn các option sản phẩm" v-if="objRequest.id > 0">
                                        <treeselect :multiple="true" :flat="true" :options="OpOptions"
                                            placeholder="Xin mời bạn lựa chọn các option sản phẩm"
                                            v-model="optionValues" :default-expanded-level="Infinity"
                                            @input="updateProductOptions()" />
                                    </b-form-group>
                                    <b-form-group label="Tọa độ trên Google Map">
                                        <b-form-input v-model="objRequest.googleMapCrood" placeholder="(lat - long)"
                                            required></b-form-input>
                                    </b-form-group>
                                    <b-form-group label="Thời gian confirm booking">
                                        <b-form-input v-model="objRequest.confirmOption"
                                            @input="onChangePackageConfirmOption()"
                                            placeholder="Điền thời gian confirm theo giờ.'AUTO' hoặc để trống sẽ là AUTO CONFIRM"></b-form-input>
                                    </b-form-group>

                                    <b-form-group label="Danh sách ảnh">
                                        <div class="notice-upload-image">
                                            <p>
                                                <b style="font-size: 14px;">Đăng nhiều ảnh để công cụ tìm kiếm dễ thấy
                                                    bạn hơn!</b><br><i
                                                    style="font-size: 11px; padding-top: 5px; color: #666;">
                                                    Kéo ảnh lên vị trí đầu tiên để chọn làm
                                                    Thumbnail.
                                                </i>
                                            </p>
                                        </div>
                                        <div class="row gallery-upload-file ui-sortable">

                                            <draggable v-model="ListImg" class="row col-md-10">
                                                <div style="padding:0"
                                                    class="col-md-2 col-xs-4 r-queue-item added ui-sortable-handle"
                                                    v-for="(itemimg, index) in ListImg" :key="index">
                                                    <div class="item"><img style="width:100%;height:100px" alt="Ảnh lỗi"
                                                            :src="pathImgs(itemimg)"></div><i @click="removeImg(index)"
                                                        class="fa fa-times"></i>
                                                </div>
                                            </draggable>




                                            <div class="col-md-2 col-xs-4 _library ui-sortable-handle">
                                                <i class="fa fa-folder-open" @click="openImg('ListImg')"></i>
                                                <p>[Thư viện ảnh]</p>
                                            </div>
                                        </div>

                                    </b-form-group>
                                    <b-form-group hidden>
                                        <b-form-checkbox v-model="IsChoseFileDowload" style="padding-bottom:7px">
                                            File Dowload
                                        </b-form-checkbox>

                                        <b-form-input v-if="!IsChoseFileDowload" v-model="FileMeta.FileDowload"
                                            placeholder="File dowload" required></b-form-input>
                                        <template v-if="IsChoseFileDowload">
                                            <button @click="openImg('filedowload')" type="button"
                                                class="btn btn-success">Chọn file</button>
                                            <a v-if="FileMeta.FileDowload != null && FileMeta.FileDowload.length > 0"
                                                target="_blank" href="FileMeta.FileDowload"><i class="fa fa-file-o"></i>
                                                {{ FileMeta.FileDowload }}</a>
                                        </template>
                                    </b-form-group>
                                    <b-form-group hidden>
                                        <b-form-checkbox v-model="IsChoseImg360" style="padding-bottom:7px">
                                            Hình 360
                                        </b-form-checkbox>
                                        <b-form-input v-if="!IsChoseImg360" v-model="FileMeta.File360"
                                            placeholder="Hình ảnh 360" required></b-form-input>
                                        <template v-if="IsChoseImg360">
                                            <button @click="openImg('file360')" type="button"
                                                class="btn btn-success">Chọn file</button>
                                            <a v-if="FileMeta.File360 != null && FileMeta.File360.length > 0"
                                                target="_blank" href="FileMeta.File360"><i class="fa fa-file-o"></i>
                                                {{ FileMeta.File360 }}</a>
                                        </template>
                                    </b-form-group>
                                    <b-form-group hidden>
                                        <b-form-checkbox v-model="IsChoseVideo" style="padding-bottom:7px">
                                            File Video
                                        </b-form-checkbox>
                                        <b-form-input v-if="!IsChoseVideo" v-model="FileMeta.FileVideo"
                                            placeholder="File Video" required></b-form-input>
                                        <template v-if="IsChoseVideo">
                                            <button @click="openImg('fileVideo')" type="button"
                                                class="btn btn-success">Chọn file</button>
                                            <a v-if="FileMeta.FileVideo != null && FileMeta.FileVideo.length > 0"
                                                target="_blank" href="FileMeta.FileVideo"><i class="fa fa-file-o"></i>
                                                {{ FileMeta.FileVideo }}</a>
                                        </template>
                                    </b-form-group>
                                </b-form>
                            </div>
                        </div>
                        <div class="card">
                            <div class="card-header">
                                <b-row>
                                    <b-col md="10">
                                        Ảnh
                                    </b-col>
                                </b-row>
                            </div>
                            <div class="card-body">
                                <b-form-group label="Ảnh đại diện">
                                    <b-form-group>
                                        <a @click="openImg('Img')">
                                            <div style="width:150px;display:flex"
                                                class=" gallery-upload-file ui-sortable">
                                                <div style="padding:0"
                                                    class="col-md-12 col-xs-12  r-queue-item ui-sortable-handle">
                                                    <div
                                                        v-if="objRequest.avatar != null && objRequest.avatar != undefined && objRequest.avatar.length > 0">
                                                        <img alt="Ảnh lỗi" style="height:150px;width:150px"
                                                            :src="pathImgs(objRequest.avatar)"
                                                            class="preview-image img-thumbnail">
                                                    </div>
                                                    <div v-else>
                                                        <i class="fa fa-picture-o"></i>
                                                        <p>[Chọn ảnh]</p>
                                                    </div>
                                                </div>
                                            </div>
                                        </a>
                                    </b-form-group>
                                </b-form-group>
                            </div>
                        </div>

                        <div class="card">
                            <div class="card-header">
                                More Info
                            </div>
                            <div class="card-body">

                                <b-form-group label="Sản Phẩm cha">
                                    <treeselect :multiple="false" :flat="false" :options="ListProductParent"
                                        placeholder="Xin mời bạn lựa chọn SP cha" v-model="ProductParent" />
                                </b-form-group>
                            </div>
                        </div>
                        <div class="card">
                            <div class="card-header">
                                Giá sản phẩm
                            </div>
                            <div class="card-body">
                                <b-row>
                                    <b-form-group style="display:none" label="Màu sắc">
                                        <treeselect :multiple="true" :flat="true" :options="Colors"
                                            placeholder="Xin mời bạn lựa chọn thuộc tính" v-model="ListColors" />
                                    </b-form-group>
                                </b-row>
                                <b-row>
                                    <b-col>
                                        <b-form-group label="Giá sản phẩm">
                                            <b-form-input @change="sumTotal" :min="0" v-model="objRequest.price"
                                                type="number" placeholder="Giá sản phẩm" required></b-form-input>
                                        </b-form-group>
                                    </b-col>
                                    <b-col>
                                        <b-form-group label="Phần trăm">
                                            <b-form-input @change="sumTotal" :min="0" :max="100"
                                                v-model="objRequest.discountPercent" type="number"
                                                placeholder="Phần trăm" required></b-form-input>
                                        </b-form-group>
                                    </b-col>
                                    <b-col>
                                        <b-form-group label="Giảm giá">
                                            <b-form-input v-model="objRequest.discountPrice" :min="0" type="number"
                                                placeholder="Giảm giá" required></b-form-input>
                                        </b-form-group>
                                    </b-col>

                                </b-row>
                                <b-row>
                                    <b-col>
                                        <b-form-group label="Giá người lớn">
                                            <b-form-input :min="0" v-model="objRequest.giaNguoiLon" type="number"
                                                placeholder="Giá người lớn" required></b-form-input>
                                        </b-form-group>
                                    </b-col>
                                    <b-col>
                                        <b-form-group label="Giá trẻ em">
                                            <b-form-input :min="0" v-model="objRequest.giaTreEm" type="number"
                                                placeholder="Giá trẻ em" required></b-form-input>
                                        </b-form-group>
                                    </b-col>
                                    <b-col>
                                        <b-form-group label="Giá em bé">
                                            <b-form-input v-model="objRequest.giaEmBe" :min="0" type="number"
                                                placeholder="Giá em bé" required></b-form-input>
                                        </b-form-group>
                                    </b-col>

                                </b-row>
                                <b-form-group label="Bảo hành" hidden>
                                    <b-form-input v-model="objRequest.warranty" placeholder="Bảo hành"
                                        required></b-form-input>
                                </b-form-group>

                                <b-form-group hidden>
                                    <label class="typo__label">Thêm bài viết liên quan</label>
                                    <v-tags-input element-id="tags" placeholder="Bài viết liên quan"
                                        :only-existing-tags="true" v-model="ArticleValues" :typeahead="true"
                                        :existingTags="ListArticle"></v-tags-input>
                                </b-form-group>
                            </div>
                        </div>
                        <div class="card" v-if="objRequest.id > 0">
                            <div class="card-header">
                                <b-row>
                                    <b-col md="11">
                                        Chính sách hoàn hủy
                                    </b-col>
                                    <b-col md="1">
                                        <b-btn v-b-toggle.collapsecancelpolicy variant="primary" class="pull-right"><i
                                                class="fa fa-angle-double-down" aria-hidden="true"></i></b-btn>
                                    </b-col>
                                </b-row>
                            </div>
                            <b-collapse id="collapsecancelpolicy" class="mt-2 scrolling-wrapper" fluid>
                                <div class="card-body">
                                    <h4>Thêm chính sách hoàn hủy</h4><small style="color:red">(Áp dụng với sản phẩm cha)</small>
                                    <b-row>
                                        <b-col>
                                            <button class="btn btn-info btn-submit-form col-md-12 btncus" type="submit"
                                                @click="AddNewCancelPolicy()">
                                                <i class="fa fa-plus"></i> Thêm mới
                                            </button>
                                        </b-col>
                                    </b-row>
                                    <br />
                                    <b-container fluid>
                                        <b-card v-for="(cp, index) in objRequestCancelPolicy">
                                            <b-row>
                                                <b-col md="4">
                                                    <b-form-group label="Số ngày hủy trước">
                                                        <b-form-input type="number" min="0" v-model="cp.beforeDate"></b-form-input>
                                                    </b-form-group>
                                                </b-col>
                                                <b-col md="4">
                                                    <b-form-group label="Giá trị hoàn trả">
                                                        <b-form-input type="number" min="0" v-model="cp.rollbackValue"></b-form-input>
                                                    </b-form-group>
                                                </b-col>
                                                <b-col md="2">
                                                    <button class="btn btn-danger" @click="onRemoveCancelPolicy(index)"><i class="fa fa-trash" aria-hidden="true"></i>
                                                    </button>
                                                </b-col>
                                            </b-row>
                                        </b-card>
                                    </b-container>
                                </div>
                            </b-collapse>
                        </div>
                        <div class="card" v-if="objRequest.id > 0">
                            <div class="card-header">
                                <b-row>
                                    <b-col md="11">
                                        Option giá
                                    </b-col>
                                    <b-col md="1">
                                        <b-btn v-b-toggle.collapseolerenewal variant="primary" class="pull-right"><i
                                                class="fa fa-angle-double-down" aria-hidden="true"></i></b-btn>
                                    </b-col>
                                </b-row>
                            </div>
                            <b-collapse id="collapseolerenewal" class="mt-2 scrolling-wrapper" fluid>
                                <div class="card-body">
                                    <h4>Thêm Option giá</h4>
                                    <b-row>
                                        <b-col>
                                            <button class="btn btn-info btn-submit-form col-md-12 btncus" type="submit"
                                                @click="AddNewPhienBan()">
                                                <i class="fa fa-plus"></i> Thêm mới
                                            </button>
                                        </b-col>
                                    </b-row>
                                    <br />
                                    <b-container fluid>
                                        <b-card v-for="(item, index) in objRequestPhienBans" class="scrolling-wrapper">
                                            <b-row>
                                                <b-col style="min-width: 100px;" class="flex-item"
                                                    v-for="(option, indexOption) in selectedProductOptions">
                                                    <b-form-group :label="option.parent.label">
                                                        <treeselect :multiple="false" :flat="false"
                                                            :options="option.childs"
                                                            placeholder="Xin mời bạn lựa chọn SP cha"
                                                            v-model="item.selectedOptions[indexOption]" />
                                                    </b-form-group>
                                                </b-col>
                                                <b-col>
                                                    <b-form-group label="Action">
                                                        <b-button-group>
                                                            <b-button
                                                                class="btn btn-success btn-submit-form col-md-12 btncus"
                                                                type="submit" @click="AddNewPhienBan()">
                                                                <i class="fa fa-plus"></i>
                                                            </b-button>
                                                            <b-button
                                                                class="btn btn-danger btn-submit-form col-md-12 btncus"
                                                                type="submit" @click="DoDelPhienBan(index)">
                                                                <i class="fa fa-trash"></i>
                                                            </b-button>
                                                            <b-button
                                                                class="btn btn-danger btn-submit-form col-md-12 btncus"
                                                                type="submit" @click="DoAddGiaPhienBanTheoNgay(item)">
                                                                <i class="fa fa-money"></i>
                                                            </b-button>
                                                        </b-button-group>

                                                    </b-form-group>


                                                </b-col>
                                            </b-row>
                                            <b-row>
                                                <b-col style="min-width: 100px;" class="flex-item">
                                                    <b-form-group label="Ng.lớn (Giá mặc định)">
                                                        <b-form-input @change="" :min="0"
                                                            v-model="item.priceEachNguoiLon" type="number"
                                                            placeholder="Giá người lớn" required></b-form-input>
                                                    </b-form-group>
                                                </b-col>
                                                <b-col style="min-width: 100px;" class="flex-item">
                                                    <b-form-group label="Trẻ em">
                                                        <b-form-input @change="" :min="0" v-model="item.priceEachTreEm"
                                                            type="number" placeholder="Giá trẻ em"></b-form-input>
                                                    </b-form-group>
                                                </b-col>
                                                <b-col style="min-width: 100px;" class="flex-item">
                                                    <b-form-group label="Ng.lớn (Giá NET)">
                                                        <b-form-input @change="" :min="0" v-model="item.netEachNguoiLon"
                                                            type="number" placeholder="Giá người lớn (NET)"
                                                            required></b-form-input>
                                                    </b-form-group>
                                                </b-col>
                                                <b-col style="min-width: 100px;" class="flex-item">
                                                    <b-form-group label="Trẻ em (Giá NET)">
                                                        <b-form-input @change="" :min="0" v-model="item.netEachTreEm"
                                                            type="number" placeholder="Giá trẻ em (NET)"></b-form-input>
                                                    </b-form-group>
                                                </b-col>
                                                <b-col style="min-width: 100px;" class="flex-item">
                                                    <b-form-group label="Ng.già">
                                                        <b-form-input @change="" :min="0"
                                                            v-model="item.priceEachNguoiGia" type="number"
                                                            placeholder="Giá người già" required></b-form-input>
                                                    </b-form-group>
                                                </b-col>
                                                <b-col style="min-width: 100px;" class="flex-item">
                                                    <b-form-group label="Ng.già (Giá NET)">
                                                        <b-form-input @change="" :min="0" v-model="item.netEachNguoiGia"
                                                            type="number"
                                                            placeholder="Giá người già (NET)"></b-form-input>
                                                    </b-form-group>
                                                </b-col>
                                            </b-row>
                                            <b-row>
                                                <b-col style="min-width: 100px;" class="flex-item">
                                                    <b-form-group label="SL Ng.Lớn tối thiểu">
                                                        <b-form-input @change="" :min="1" v-model="item.minimumNguoiLon"
                                                            type="number" placeholder="SL người lớn"
                                                            required></b-form-input>
                                                    </b-form-group>
                                                </b-col>
                                                <b-col style="min-width: 100px;" class="flex-item">
                                                    <b-form-group label="SL Tr.em tối thiểu">
                                                        <b-form-input @change="" :min="0" v-model="item.minimumTreEm"
                                                            type="number" placeholder="SL trẻ em"
                                                            required></b-form-input>
                                                    </b-form-group>
                                                </b-col>
                                                <b-col style="min-width: 100px;" class="flex-item">
                                                    <b-form-group label="SL Ng.già tối thiểu">
                                                        <b-form-input @change="" :min="0" v-model="item.minimumNguoiGia"
                                                            type="number" placeholder="SL người già"
                                                            required></b-form-input>
                                                    </b-form-group>
                                                </b-col>
                                                <b-col style="min-width: 500px;" class="flex-item">
                                                    <b-form-group label="Email thông báo đối tác">
                                                        <treeselect :multiple="false" :flat="true"
                                                            :options="userSuppliers"
                                                            placeholder="Xin mời bạn lựa chọn tag"
                                                            v-model="item.emailSupplier"
                                                            :default-expanded-level="Infinity" />
                                                    </b-form-group>
                                                </b-col>

                                            </b-row>
                                            <b-row>
                                                <b-col md=6>
                                                    <b-form-group label="Thời gian confirm">
                                                        <b-form-input v-model="item.confirmOption"
                                                            placeholder="Điền thời gian confirm theo giờ.'AUTO' hoặc để trống sẽ là AUTO CONFIRM"></b-form-input>
                                                    </b-form-group>
                                                </b-col>
                                            </b-row>
                                            <b-row>
                                                <b-col md=6>
                                                    <b-form-group label="Thời gian đặt Last Minute">
                                                        <b-row>
                                                            <b-col md="4">
                                                                <b-form-group label="Số ngày tối thiểu">
                                                                    <b-form-input type="number" min="0" v-model="item.lastMinuteSetupDay"></b-form-input>
                                                                </b-form-group>
                                                            </b-col>
                                                            <b-col md="4">
                                                                <b-form-group label="Thời gian Last Minute">
                                                                    <b-form-input v-model="item.lastMinuteSetupTime" placeholder="hh:mm"></b-form-input>
                                                                </b-form-group>
                                                            </b-col>
                                                        </b-row>
                                                    </b-form-group>
                                                    
                                                </b-col>
                                            </b-row>
                                        </b-card>
                                    </b-container>

                                </div>
                            </b-collapse>
                        </div>
                        <div>
                            <b-modal title="Cập nhật giá phiên bản" ref="pByDateModal" size="xl"
                                @ok="onUpdateGiaPhienBanTheoNgay()">
                                <b-card>
                                    <b-row>
                                        <b-col md="3">
                                            <b-form-group label="Từ ngày">
                                                <b-form-input type="date"
                                                    v-model="filterGiaPhienBanTheoNgay.from"></b-form-input>
                                            </b-form-group>

                                        </b-col>
                                        <b-col md="3">
                                            <b-form-group label="Đến ngày">
                                                <b-form-input type="date"
                                                    v-model="filterGiaPhienBanTheoNgay.to"></b-form-input>
                                            </b-form-group>

                                        </b-col>
                                        <b-col md="6">
                                            <b-form-group label="Thứ">
                                                <b-row>
                                                    <b-col v-for="d in filterGiaPhienBanTheoNgay.dayOfWeek">
                                                        <b-form-checkbox v-model="d.checked">
                                                            {{ d.label }}
                                                        </b-form-checkbox>
                                                    </b-col>
                                                </b-row>


                                            </b-form-group>
                                        </b-col>
                                    </b-row>
                                    <b-row></b-row>
                                </b-card>
                                <b-card>
                                    <table class="table table-bordered table-responsive">
                                        <thead>
                                            <tr>
                                                <th>Ngày</th>
                                                <th>Thứ</th>
                                                <th>Giá người lớn</th>
                                                <th>Giá trẻ em</th>
                                                <th>Giá người già</th>
                                                <th>Giá người lớn (NET)</th>
                                                <th>Giá trẻ em (NET)</th>
                                                <th>Giá người già (NET)</th>
                                            </tr>
                                            <tr>
                                                <th colspan="2">
                                                    <button class="form-control btn btn-success"
                                                        @click="onUpdateAllCurrentGiaPhienBanTheoNgay()">Cập nhật
                                                        ALL</button>
                                                </th>
                                                <th>
                                                    <b-form-input type="number"
                                                        v-model="updateAllGiaPhienBanTheoNgay.priceEachNguoiLon"></b-form-input>
                                                </th>
                                                <th>
                                                    <b-form-input type="number"
                                                        v-model="updateAllGiaPhienBanTheoNgay.priceEachTreEm"></b-form-input>
                                                </th>
                                                <th>
                                                    <b-form-input type="number"
                                                        v-model="updateAllGiaPhienBanTheoNgay.priceEachNguoiGia"></b-form-input>
                                                </th>
                                                <th>
                                                    <b-form-input type="number"
                                                        v-model="updateAllGiaPhienBanTheoNgay.netEachNguoiLon"></b-form-input>
                                                </th>
                                                <th>
                                                    <b-form-input type="number"
                                                        v-model="updateAllGiaPhienBanTheoNgay.netEachTreEm"></b-form-input>
                                                </th>
                                                <th>
                                                    <b-form-input type="number"
                                                        v-model="updateAllGiaPhienBanTheoNgay.netEachNguoiGia"></b-form-input>
                                                </th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr v-for="c in currentGiaPhienBanTheoNgay" v-if="c.filterd">
                                                <td>{{ onFormatDate(c.date) }}</td>
                                                <td>{{ c.dayOfWeek }}</td>
                                                <td>
                                                    <b-form-input v-model="c.priceEachNguoiLon"
                                                        type="number"></b-form-input>
                                                </td>
                                                <td>
                                                    <b-form-input v-model="c.priceEachTreEm"
                                                        type="number"></b-form-input>
                                                </td>
                                                <td>
                                                    <b-form-input v-model="c.priceEachNguoiGia"
                                                        type="number"></b-form-input>
                                                </td>
                                                <td>
                                                    <b-form-input v-model="c.netEachNguoiLon"
                                                        type="number"></b-form-input>
                                                </td>
                                                <td>
                                                    <b-form-input v-model="c.netEachTreEm" type="number"></b-form-input>
                                                </td>
                                                <td>
                                                    <b-form-input v-model="c.netEachNguoiGia"
                                                        type="number"></b-form-input>
                                                </td>

                                            </tr>
                                        </tbody>
                                    </table>
                                </b-card>
                            </b-modal>
                        </div>

                        <div class="card" hidden>
                            <div class="card-header">
                                <b-row>
                                    <b-col md="11">
                                        Avalible Top-Up
                                    </b-col>
                                    <b-col md="1">
                                        <b-btn v-b-toggle.collapseolerenewal variant="primary" class="pull-right"><i
                                                class="fa fa-angle-double-down" aria-hidden="true"></i></b-btn>
                                    </b-col>
                                </b-row>
                            </div>
                            <b-collapse id="collapseolerenewal" class="mt-2">
                                <div class="card-body">
                                    <h4>Thêm Topup</h4>
                                    <b-row>
                                        <b-col>
                                            <button class="btn btn-info btn-submit-form col-md-12 btncus" type="submit"
                                                @click="AddNewTopUp()">
                                                <i class="fa fa-plus"></i> Thêm mới
                                            </button>
                                        </b-col>
                                    </b-row>
                                    <br />
                                    <b-row v-for="(item, index) in objRequestTopUp">
                                        <b-col>
                                            <b-row>
                                                <b-col>
                                                    <b-form-group label="Title">
                                                        <b-form-input v-model="item.title" type="text"
                                                            placeholder="Tên Top up" required></b-form-input>
                                                    </b-form-group>
                                                </b-col>
                                                <b-col>
                                                    <b-form-group label="Data">
                                                        <b-form-input v-model="item.dataLimit" type="text"
                                                            placeholder="Data Limit" required></b-form-input>
                                                    </b-form-group>
                                                </b-col>
                                                <b-col>
                                                    <b-form-group label="Validaty">
                                                        <b-form-input v-model="item.validaty" type="text"
                                                            placeholder="Validaty" required></b-form-input>
                                                    </b-form-group>
                                                </b-col>

                                            </b-row>
                                            <b-row>
                                                <b-col>
                                                    <b-form-group label="SMS Number">
                                                        <b-form-input v-model="item.smsNumber" type="text"
                                                            placeholder="Sms" required></b-form-input>
                                                    </b-form-group>
                                                </b-col>
                                                <b-col>
                                                    <b-form-group label="Phone Minute">
                                                        <b-form-input v-model="item.phoneMinute" type="text"
                                                            placeholder="Phone" required></b-form-input>
                                                    </b-form-group>
                                                </b-col>
                                                <b-col>
                                                    <b-form-group label="Price">
                                                        <b-form-input @change="" :min="0" v-model="item.price"
                                                            type="text" placeholder=""></b-form-input>
                                                    </b-form-group>
                                                </b-col>
                                                <b-col>
                                                    <b-form-group label="Action">
                                                        <button class="btn btn-danger btn-submit-form col-md-12 btncus"
                                                            type="submit" @click="DoDelTopUp(index)">
                                                            <i class="fa fa-trash"></i> Xóa
                                                        </button>
                                                    </b-form-group>

                                                </b-col>
                                            </b-row>
                                            <hr />
                                        </b-col>
                                    </b-row>
                                </div>
                            </b-collapse>
                        </div>
                        <div class="card" hidden>
                            <div class="card-header">
                                <b-row>
                                    <b-col md="11">
                                        SIM Serial
                                    </b-col>
                                    <b-col md="1">
                                        <b-btn v-b-toggle.collapSerial variant="primary" class="pull-right"><i
                                                class="fa fa-angle-double-down" aria-hidden="true"></i></b-btn>
                                    </b-col>
                                </b-row>
                            </div>
                            <b-collapse id="collapSerial" class="mt-2">
                                <div class="card-body">
                                    <h4>Serial Number: </h4>
                                    <b-row>
                                        <b-col>
                                            <button class="btn btn-info btn-submit-form col-md-12 btncus" type="submit"
                                                @click="onUploadSerialClick()">
                                                <i class="fa fa-plus"></i> Upload Data
                                            </button>
                                            <input ref="fileInput" type="file" accept="xlsx" style="display: none"
                                                @change="handleFileChange" />
                                        </b-col>
                                    </b-row>
                                    <br />
                                    <table class="table table-bordered">
                                        <thead>
                                            <tr>
                                                <th>Serial</th>
                                                <th>Phone number</th>
                                                <th>Status</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr v-for="s in serialNumbers" :key="s.id">
                                                <td>{{ s.serialNumber }}</td>
                                                <td>{{ s.phoneNumber }}</td>
                                                <td>{{ s.status }}</td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </b-collapse>
                        </div>


                    </div>

                </div>
            </b-tab>
            <b-tab v-if="objRequest.id > 0" @click="StartLoadLang()" title="2. Thông tin ngôn ngữ">
                <div>
                    <div class="row productedit">
                        <div class="col-sm-6 col-md-12">
                            <div class="card">
                                <div class="card-header">
                                    Thao tác
                                </div>
                                <div class="card-body">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <button class="btn btn-info btn-submit-form col-md-12 btncus" type="submit"
                                                @click="DoAddDetail()">
                                                <i class="fa fa-save"></i> Cập nhật
                                            </button>
                                        </div>
                                        <div class="col-md-6">
                                            <button class="btn btn-success col-md-12 btncus" type="button"
                                                @click="DoRefesh()">
                                                <i class="fa fa-refresh"></i> Làm mới
                                            </button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-sm-6 col-md-12">
                            <div class="card">
                                <div class="card-header">
                                    Thông tin ngôn ngữ
                                </div>
                                <div class="card-body">
                                    <b-form class="form-horizontal">
                                        <b-row>
                                            <b-col>
                                                <b-form-group label="Ngôn ngữ">
                                                    <b-form-select v-model="langSelected" @change="onChangeSelectd"
                                                                   :options="Languages"></b-form-select>
                                                </b-form-group>
                                            </b-col>
                                            <!--<b-col title="Xem trên website" style="margin-top:25px;font-size:30px">
                        <a target="_blank" :href="urlBases(objRequestDetail.url)"><i class="fa fa-eye"></i></a>
                    </b-col>-->
                                            <b-col></b-col>
                                        </b-row>
                                        <b-form-group label="Tiêu đề theo ngôn ngữ" label-for="input-1">
                                            <b-form-input id="input-1" v-model="objRequestDetail.title" type="text"
                                                          required v-on:keyup.13="slugM()" placeholder="Tiêu đề"></b-form-input>
                                        </b-form-group>

                                        <b-form-group label="Đường dẫn">
                                            <b-form-input v-model="objRequestDetail.url" placeholder="Đường dẫn"
                                                          required></b-form-input>
                                        </b-form-group>
                                        <b-form-group label="Đường dẫn cũ">
                                            <b-form-input v-model="objRequestDetail.urlOld" placeholder="Đường dẫn"
                                                          required></b-form-input>
                                        </b-form-group>
                                        <b-form-group label="Location">
                                            <b-form-input v-model="objRequestDetail.location" placeholder="Địa chỉ"
                                                          required></b-form-input>
                                        </b-form-group>
                                        <b-form-group label="Location Iframe">
                                            <b-form-input v-model="objRequestDetail.locationIframe"
                                                          placeholder="Mã embed bản đồ" required></b-form-input>
                                        </b-form-group>
                                        <b-form-group label="Đơn vị sản phẩm">
                                            <b-form-input v-model="objRequestDetail.unit" placeholder="Đơn vị sản phẩm"
                                                          required></b-form-input>
                                        </b-form-group>

                                        <b-form-group label="Mô tả">
                                            <ckeditor tag-name="textarea" v-model="objRequestDetail.description"
                                                      :rows="3" :config="editorConfig">
                                            </ckeditor>
                                        </b-form-group>
                                        <b-form-group label="Nội dung">
                                            <MIEditor :contentEditor="objRequestDetail.content"
                                                      v-on:handleEditorInput="getOrSetData" :index="-1"></MIEditor>
                                        </b-form-group>
                                        <b-row>
                                            <b-col>
                                                <b-form-group>
                                                    <label class="typo__label">Thêm tag</label>
                                                    <v-tags-input element-id="tags" v-model="Tags" :typeahead="true"
                                                                  :existingTags="ListAllTags"></v-tags-input>

                                                </b-form-group>
                                            </b-col>
                                        </b-row>
                                    </b-form>
                                </div>
                            </div>
                            <div class="card">
                                <div class="card-header">
                                    <b-row>
                                        <b-col md="11">
                                            Khuyến mại (Hiển thị ở góc sản phẩm)
                                        </b-col>
                                        <b-col md="1">
                                            <b-btn v-b-toggle.collapsePromo variant="primary" class="pull-right">
                                                <i class="fa fa-angle-double-down" aria-hidden="true"></i>
                                            </b-btn>
                                        </b-col>
                                    </b-row>
                                </div>
                                <b-collapse id="collapsePromo" class="mt-2">
                                    <div class="card-body">
                                        <b-row>
                                            <b-col md="3">
                                                <a @click="openImg('ImgIconCorner')">
                                                    <div style="width:150px;display:flex"
                                                         class=" gallery-upload-file ui-sortable">
                                                        <div style="padding:0"
                                                             class="col-md-12 col-xs-12  r-queue-item ui-sortable-handle">
                                                            <div v-if="objRequestDetail.iconCornerTag != null && objRequestDetail.iconCornerTag != undefined && objRequestDetail.iconCornerTag.length > 0">
                                                                <img alt="Ảnh lỗi" style="height:150px;width:150px"
                                                                     :src="pathImgs(objRequestDetail.iconCornerTag)"
                                                                     class="preview-image img-thumbnail">
                                                            </div>
                                                            <div v-else>
                                                                <i class="fa fa-picture-o"></i>
                                                                <p>[Chọn ảnh]</p>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </a>
                                            </b-col>
                                            <b-col md="9">
                                                <b-form-group label="Text icon tag góc">
                                                    <b-form-input v-model="objRequestDetail.textCornerTag" placeholder=""
                                                                  required></b-form-input>
                                                </b-form-group>
                                                <b-form-group label="Mã màu Icon tag góc">
                                                    <b-form-input v-model="objRequestDetail.colorCornerTag" placeholder=""
                                                                  required></b-form-input>
                                                </b-form-group>
                                            </b-col>
                                        </b-row>
                                        


                                    </div>
                                </b-collapse>
                            </div>
                            <div class="card">
                                <div class="card-header">
                                    <b-row>
                                        <b-col md="11">
                                            Thông tin
                                        </b-col>
                                        <b-col md="1">
                                            <b-btn v-b-toggle.collapseSocial3 variant="primary" class="pull-right">
                                                <i class="fa fa-angle-double-down" aria-hidden="true"></i>
                                            </b-btn>
                                        </b-col>
                                    </b-row>
                                </div>
                                <b-collapse id="collapseSocial3" class="mt-2">
                                    <div class="card-body">

                                        <b-row v-for="(item, index) in LichTrinhTour">
                                            <b-col md="10">
                                                <b-form-group label="Tiêu đề">
                                                    <b-form-input v-model="item.tieuDe" placeholder="Tiêu đề"
                                                                  required></b-form-input>
                                                </b-form-group>
                                                <b-form-group label="Nội dung">
                                                    <MIEditor :contentEditor="item.noiDung"
                                                              v-on:handleEditorInput="tour_getOrSetData_lichTour"
                                                              :index="index">
                                                    </MIEditor>
                                                </b-form-group>
                                            </b-col>
                                            <b-col md="2">
                                                <b-row>
                                                    <b-col md="12">
                                                        <button class="btn btn-primary btn-submit-form col-md-12 btncus"
                                                                type="submit" @click="tour_DoAddLichTour()">
                                                            <i class="fa fa-trash"></i> Thêm
                                                        </button>
                                                    </b-col>
                                                    <b-col md="12">
                                                        <button class="btn btn-danger btn-submit-form col-md-12 btncus"
                                                                type="submit" @click="tour_DoDelLichTour(index)">
                                                            <i class="fa fa-trash"></i> Xóa
                                                        </button>
                                                    </b-col>
                                                </b-row>

                                            </b-col>
                                        </b-row>

                                    </div>
                                </b-collapse>
                            </div>
                            <div class="card">
                                <div class="card-header">
                                    <b-row>
                                        <b-col md="11">
                                            Note
                                        </b-col>
                                        <b-col md="1">
                                            <b-btn v-b-toggle.collapseSocial4 variant="primary" class="pull-right">
                                                <i class="fa fa-angle-double-down" aria-hidden="true"></i>
                                            </b-btn>
                                        </b-col>
                                    </b-row>
                                </div>
                                <b-collapse id="collapseSocial4" class="mt-2">
                                    <div class="card-body">

                                        <b-row v-for="(item, index) in ThongTinTour">
                                            <b-col md="10">
                                                <b-form-group label="Tiêu đề">
                                                    <b-form-input v-model="item.tieuDe" placeholder="Tiêu đề"
                                                                  required></b-form-input>
                                                </b-form-group>
                                                <b-form-group label="Nội dung">
                                                    <MIEditor :contentEditor="item.noiDung"
                                                              v-on:handleEditorInput="tour_getOrSetData_thongTinTour"
                                                              :index="index">
                                                    </MIEditor>
                                                </b-form-group>
                                            </b-col>
                                            <b-col md="2">
                                                <b-row>
                                                    <b-col md="12">
                                                        <button class="btn btn-primary btn-submit-form col-md-12 btncus"
                                                                type="submit" @click="tour_DoAddThongTinTour()">
                                                            <i class="fa fa-trash"></i> Thêm
                                                        </button>
                                                    </b-col>
                                                    <b-col md="12">
                                                        <button class="btn btn-danger btn-submit-form col-md-12 btncus"
                                                                type="submit" @click="tour_DoDelThongTinTour(index)">
                                                            <i class="fa fa-trash"></i> Xóa
                                                        </button>
                                                    </b-col>
                                                </b-row>

                                            </b-col>


                                        </b-row>

                                    </div>
                                </b-collapse>
                            </div>
                            <div class="card">
                                <div class="card-header">
                                    <b-row>
                                        <b-col md="11">
                                            Policy
                                        </b-col>
                                        <b-col md="1">
                                            <b-btn v-b-toggle.collapseSocial5 variant="primary" class="pull-right">
                                                <i class="fa fa-angle-double-down" aria-hidden="true"></i>
                                            </b-btn>
                                        </b-col>
                                    </b-row>
                                </div>
                                <b-collapse id="collapseSocial5" class="mt-2">
                                    <div class="card-body">

                                        <b-row v-for="(item, index) in ThuTucVisa">
                                            <b-col md="10">
                                                <b-form-group label="Tiêu đề">
                                                    <b-form-input v-model="item.tieuDe" placeholder="Tiêu đề"
                                                                  required></b-form-input>
                                                </b-form-group>
                                                <b-form-group label="Nội dung">
                                                    <MIEditor :contentEditor="item.noiDung"
                                                              v-on:handleEditorInput="tour_getOrSetData_thuTucVisa"
                                                              :index="index">
                                                    </MIEditor>
                                                </b-form-group>
                                            </b-col>
                                            <b-col md="2">
                                                <b-row>
                                                    <b-col md="12">
                                                        <button class="btn btn-primary btn-submit-form col-md-12 btncus"
                                                                type="submit" @click="tour_DoAddThuTucVisa()">
                                                            <i class="fa fa-trash"></i> Thêm
                                                        </button>
                                                    </b-col>
                                                    <b-col md="12">
                                                        <button class="btn btn-danger btn-submit-form col-md-12 btncus"
                                                                type="submit" @click="tour_DoDelThuTucVisa(index)">
                                                            <i class="fa fa-trash"></i> Xóa
                                                        </button>
                                                    </b-col>
                                                </b-row>
                                            </b-col>
                                        </b-row>

                                    </div>
                                </b-collapse>
                            </div>
                            <div class="card">
                                <div class="card-header">
                                    <b-row>
                                        <b-col md="11">
                                            SEO Analysis
                                        </b-col>
                                        <b-col md="1">
                                            <b-btn v-b-toggle.collapseSEO2 variant="primary" class="pull-right">
                                                <i class="fa fa-angle-double-down" aria-hidden="true"></i>
                                            </b-btn>
                                        </b-col>
                                    </b-row>
                                </div>
                                <b-collapse id="collapseSEO2" class="mt-2">
                                    <div class="card-body">
                                        <b-form-group label="Tiêu đề SEO">
                                            <b-form-input v-model="objRequestDetail.metaTitle" placeholder="Tiêu đề SEO"
                                                          required></b-form-input>
                                        </b-form-group>
                                        <b-form-group label="Từ khóa SEO">
                                            <b-form-input v-model="objRequestDetail.metaKeyword"
                                                          placeholder="Từ khóa SEO" required></b-form-input>
                                        </b-form-group>
                                        <b-form-group label="Mô tả SEO">
                                            <b-form-textarea rows="3" max-rows="6"
                                                             v-model="objRequestDetail.metaDescription" placeholder="Mô tả"
                                                             required></b-form-textarea>
                                        </b-form-group>
                                        <b-form-group label="Script WebPage">
                                            <b-form-textarea rows="3" max-rows="6"
                                                             v-model="objRequestDetail.metaWebPage" placeholder="WebPage"
                                                             required></b-form-textarea>
                                        </b-form-group>
                                        <b-form-group label="">
                                            <div class="row">
                                                <div class="col-md-6">
                                                    <b-form-checkbox placeholder="Canonical" v-model="statusCanonical"
                                                                     required>
                                                        Chặn
                                                        trùng lặp nội dung (Canonical)
                                                    </b-form-checkbox>
                                                </div>
                                                <div class="col-md-6">
                                                    <b-form-checkbox placeholder="NoIndex" v-model="statusNoindex"
                                                                     required>
                                                        Chặn lập
                                                        chỉ mục (NoIndex)
                                                    </b-form-checkbox>
                                                </div>
                                            </div>
                                        </b-form-group>
                                        <b-form-group label="Giá trị (Canonical)">
                                            <b-form-input placeholder="Canonical" v-model="valueCanonical"
                                                          required></b-form-input>
                                        </b-form-group>
                                    </div>

                                </b-collapse>
                            </div>
                            <div class="card">
                                <div class="card-header">
                                    <b-row>
                                        <b-col md="11">
                                            Social Share
                                        </b-col>
                                        <b-col md="1">
                                            <b-btn v-b-toggle.collapseSocial2 variant="primary" class="pull-right">
                                                <i class="fa fa-angle-double-down" aria-hidden="true"></i>
                                            </b-btn>
                                        </b-col>
                                    </b-row>
                                </div>
                                <b-collapse id="collapseSocial2" class="mt-2">
                                    <div class="card-body">
                                        <b-form-group label="Ảnh mô tả">
                                            <a @click="openImg('ImgS')">
                                                <div class="row gallery-upload-file ui-sortable">
                                                    <div style="width:200px;height:200px"
                                                         class="r-queue-item ui-sortable-handle">
                                                        <div v-if="objRequestDetail.socialImage != null && objRequestDetail.socialImage != undefined && objRequestDetail.socialImage.length > 0">
                                                            <img alt="Ảnh lỗi"
                                                                 :src="pathImgs(objRequestDetail.socialImage)"
                                                                 style="height:200px;width:200px" />
                                                        </div>
                                                        <div v-else>
                                                            <i class="fa fa-picture-o"></i>
                                                            <p>[Chọn ảnh]</p>
                                                        </div>
                                                    </div>

                                                </div>
                                            </a>
                                        </b-form-group>
                                        <b-form-group label="Tiêu đề">
                                            <b-form-input v-model="objRequestDetail.socialTitle" placeholder="Tiêu đề"
                                                          required></b-form-input>
                                        </b-form-group>
                                        <b-form-group label="Mô tả">
                                            <b-form-input v-model="objRequestDetail.socialDescription"
                                                          placeholder="Mô tả" required></b-form-input>
                                        </b-form-group>
                                    </div>
                                </b-collapse>
                            </div>

                        </div>

                    </div>
                </div>
            </b-tab>
        </b-tabs>
        <FileManager v-on:handleAttackFile="DoAttackFile" :miKey="mikey1" />
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
//import 'vue-datepicker-ui/lib/vuedatepickerui.css';
//import VueDatepickerUi from 'vue-datepicker-ui';
import draggable from 'vuedraggable'

const simulateAsyncOperation = fn => {
    setTimeout(fn, 2000)
}
export default {

    name: "productaddedit",
    data() {
        return {
            demoDate: new Date(),
            IsChoseImg360: false,
            IsChoseFileDowload: false,
            IsChoseVideo: false,

            mikey1: 'mikey1',

            content: '',
            contentContent: '',

            choseImg: "",

            isLoading: false,
            fullPage: false,
            color: "#007bff",
            isLoadLang: false,


            preview: '/assets/img/unnamed.jpg',
            previews: '/assets/img/unnamed.jpg',

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
                avatar: "",
                status: 2,
                materialType: 0,
                guarantee: "",
                price: 0,
                discountPrice: 0,
                discountPercent: 0,
                vat: false,
                exprirePromotion: moment(String(new Date())).format('YYYY-MM-DD'),
                productCpnId: 0,
                coverage: [],
                id: 0
            },
            objRequestDetail: {
                url: "",
                title: "",
                content: "",
                description: ""
            },

            objRequestDetails: [],
            objRequestPhienBans: [],
            objRequestTopUp: [],
            templatePhienBan: {
                giaNguoiLon: 0,
                giaTreEm: 0,
                selectedOptions: [],
            },

            currentPhienBan: {},
            selectedPhienBan: [],
            objRequestOldRenewal: {
                discountPercent: 0,
                discountAmount: 0
            },
            Tag: "",
            Tags: [],
            ListImg: [],//Danh sách hình ảnh

            Languages: [],//Danh sách ngôn ngữ
            ManufacturerIds: [],//Danh sách nhà cung cấp
            ZoneOptions: [],
            DiemDenOptions: [],
            TagOptions: [],
            OpOptions: [],
            BookingNotesOptions: [],
            CouponZoneOptions: [],
            ZoneValues: [],
            DiemDenValues: [],
            TagValues: [],
            NoteValues: [],
            CouponZoneValues: [],
            ArticleValues: [],
            ProductUnit: [],
            Properties: [],
            ListProperty: [],
            Colors: [],
            ListColors: [],
            ProductParent: 0,
            ListProductParent: [],
            ListPhuongTien: [],
            ListAllTags: [],
            ListGuarantee: [],
            ListTypeMaterial: [],
            ListProductComponent: [],
            objUser: {},
            FileMeta: {
                File360: "",
                FileDowload: "",
                FileVideo: "",
            },
            ListArticleValue: [],
            ListArticle: [],
            ListOldRenewal: [],
            RdoOldRenewal: "",
            LichTrinhTour: [],
            ThongTinTour: [],
            ThuTucVisa: [],
            TourItemTemplate: {
                tieuDe: "",
                noiDung: ""
            },
            requestPhuongTien: [],
            serialNumbers: [],
            countryOptions: [],
            optionValues: [],
            selectedProductOptions: [],
            isUpdatingSelectProductOption: false,  // Biến cờ để theo dõi trạng thái cập nhật,
            convertedArr: [],
            zoneTreeViewOptionById: [],
            currentProductId: 0,
            currentGiaPhienBanTheoNgay: [],
            filterGiaPhienBanTheoNgay: {
                form: null,
                to: null,
                dayOfWeek: [
                    {
                        label: 'Mon',
                        checked: false
                    },
                    {
                        label: 'Tue',
                        checked: false
                    },
                    {
                        label: 'Wed',
                        checked: false
                    },
                    {
                        label: 'Thu',
                        checked: false
                    },
                    {
                        label: 'Fri',
                        checked: false
                    },
                    {
                        label: 'Sat',
                        checked: false
                    },
                    {
                        label: 'Sun',
                        checked: false
                    }
                ]
            },
            updateAllGiaPhienBanTheoNgay: {

                priceEachNguoiLon: 0,
                priceEachTreEm: 0,
                priceEachNguoiGia: 0,
                netEachNguoiLon: 0,
                netEachTreEm: 0,
                netEachNguoiGia: 0
            },
            currentSelectedOptionCombination: '',
            userSuppliers: [],
            objRequestCancelPolicy: [],

        };
    },
    created: {},
    components: {
        MIEditor,
        Loading,

        Treeselect,

        FileManager,
        "v-tags-input": VoerroTagsInput,
        draggable
        /*Datepicker: VueDatepickerUi*/
    },
    //mounted() {

    //},



    computed: {
        ...mapGetters(["product", "fileName"]),
    },
    created() {
        if (this.$route.params.id > 0) {
            this.currentProductId = this.$route.params.id;
            var $this = this;
            this.isLoading = true;
            let initial = this.$route.query.initial;
            initial = typeof initial != "undefined" ? initial.toLowerCase() : "";

            this.getProduct(this.$route.params.id).then(respose => {
                this.objRequest = respose.data;
                ////console.log(this.objRequest)

                $this.ListProperty = respose.listProp;
                $this.ListColors = respose.listColor;
                $this.ZoneValues = respose.listCat;
                $this.DiemDenValues = respose.listDiemDen;
                $this.TagValues = respose.listTagOption;
                $this.NoteValues = respose.listNoteOption;
                $this.CouponZoneValues = respose.listCouponZoneOptions;
                $this.optionValues = respose.listProductOption;
                console.log('optionValues when creatd', $this.optionValues);
                this.onChangePaging();
                //$this.optionValues.forEach((value, index) => {

                //})


                $this.ListArticle = respose.listArticle;
                $this.ListImg = this.objRequest.avatarArray.split(",");
                if (respose.data.parentId) {
                    $this.ProductParent = respose.data.parentId;
                }




                $this.loadOptionArticleDefault(respose.data.articleId);


            });
            this.getPhienBanInProduct(this.$route.params.id).then(response => {

                if (response.data) {
                    this.objRequestPhienBans = response.data;
                    // console.log('objRequestPhienBan', this.objRequestPhienBans);
                }


            });
            this.getCancelPolicies(this.$route.params.id).then(response => {
                if(response.data){
                    this.objRequestCancelPolicy = response.data;
                }
            })
            this.getTopUpInProduct(this.$route.params.id).then((response) => {
                ////console.log(response.data)
                response.data.map(element => {
                    element.title = element.title.split(';')[0]
                })
                this.objRequestTopUp = response.data;
            })
            this.getSerialNumbersByProductId(this.$route.params.id).then((response) => {
                //////console.log(response);
                this.serialNumbers = response;
            })



            this.LoadListOld(this.$route.params.id)
            this.isLoading = false;
        }

        let vm = this;
        EventBus.$on('FileSelected', value => {

        });

        this.objUser = this.getUser();

        this.getAllLanguages().then(respose => {
            let lang = respose.listData;
            this.Languages = lang.map(function (item) {
                return {
                    value: item.languageCode,
                    text: item.name
                }
            });
        });
        this.getAllColors().then(respose => {
            this.Colors = respose;
        });
        this.getListProductParent().then(respose => {
            this.ListProductParent = respose.listData;
        });
        this.getAllManufacturers().then(response => {

            this.ManufacturerIds = response.listData;
        });

        this.productUnitGet().then(response => {
            this.ProductUnit = response.listData;
        });
        this.onGetSuppliers();
        this.getPropertyProduct().then(response => {
            this.Properties = response;
        });
        this.getTagAll().then(response => {
            try {
                this.ListAllTags = this.convertTags(response.listData);
            }
            catch (ex) {
                ////console.log(ex);
            }
        });
        this.getGuarantee().then(response => {
            try {
                this.ListGuarantee = response;
            }
            catch (ex) {
                ////console.log(ex);
            }
        });
        this.getAllTypeMaterial().then(response => {
            try {
                this.ListTypeMaterial = response.listData;
            }
            catch (ex) {
                ////console.log(ex);
            }
        });
        this.getAllCatProductComponent().then(response => {
            try {
                this.ListProductComponent = response.listData;
            }
            catch (ex) {
                ////console.log(ex);
            }
        });



        // this.onChangePaging();




        this.tour_DoAddLichTour();
        this.tour_DoAddThuTucVisa();
        this.tour_DoAddThongTinTour();
        this.ListPhuongTien.push({
            id: 1,
            label: 'Ô tô'
        })
        this.ListPhuongTien.push({
            id: 2,
            label: 'Xe máy'
        })
        this.ListPhuongTien.push({
            id: 3,
            label: 'Máy bay'
        })

    },
    mounted() {
        // this.onChangePaging();
    },
    destroyed() {
        EventBus.$off('FileSelected');
    },
    methods: {
        ...mapActions(["updateProduct",
            "addProduct",
            "getProduct",
            "getAllManufacturers",
            "getAllLanguages",
            "addProductInLanguage",
            "getZones",
            "getZonesTreeviewById",
            "deleteProduct",
            "productUnitGet",
            "getPropertyProduct",
            "getLanguageById",
            "getTagAll",
            "getGuarantee",
            "getAllColors",
            "getAllTypeMaterial",
            "getListProductParent",
            "deleteOldRenewal",
            "insertOrUpdate",
            "getProductOldRenewal",
            "getPhienBanInProduct",
            "getAllCatProductComponent",
            "getTopUpInProduct",
            "getSerialNumbersByProductId",
            "fmFileUploadExcel_ImportSeiralNumber",
            "getGiaPhienBanTheoNgay",
            "updateGiaPhienBanTheoNgay",
            "getAllUserSupplier",
            "getCancelPolicies"
        ]),
        AddNewCancelPolicy() {
            this.objRequestCancelPolicy.push({
                productId: 0,
                beforeDate: 0,
                rollbackValue: 0,
                rollbackValueOption: 1
            })
        },
        onRemoveCancelPolicy(index){
            this.objRequestCancelPolicy.splice(index, 1)
        },
        onChangePackageConfirmOption() {
            //HOOK
            //objRequestPhienBans
            // console.log(this.objRequestPhienBans)
            if (this.objRequestPhienBans.length > 0) {
                this.objRequestPhienBans.forEach(item => {
                    item.confirmOption = this.objRequest.confirmOption
                })
            }
        },
        onUpdateGiaPhienBanTheoNgay() {
            this.updateGiaPhienBanTheoNgay(this.currentGiaPhienBanTheoNgay).then(response => {
                if (response) {
                    this.$toast.success("Cập nhật thành công!", {});
                    //reset các tham số
                    this.$refs.pByDateModal.hide();
                    this.filterGiaPhienBanTheoNgay = {
                        form: null,
                        to: null,
                        dayOfWeek: [
                            {
                                label: 'Mon',
                                checked: false
                            },
                            {
                                label: 'Tue',
                                checked: false
                            },
                            {
                                label: 'Wed',
                                checked: false
                            },
                            {
                                label: 'Thu',
                                checked: false
                            },
                            {
                                label: 'Fri',
                                checked: false
                            },
                            {
                                label: 'Sat',
                                checked: false
                            },
                            {
                                label: 'Sun',
                                checked: false
                            }
                        ]
                    }

                } else {
                    this.$toast.error("Cập nhật thất bại! Liên hệ quản trị viên", {});
                }
            })

        },
        onUpdateAllCurrentGiaPhienBanTheoNgay() {
            //Can phai clone ra de tranh reference object
            let u = JSON.parse(JSON.stringify(this.updateAllGiaPhienBanTheoNgay));
            this.currentGiaPhienBanTheoNgay.forEach(v => {
                if (v.filterd) {
                    if (u.priceEachNguoiLon >= 0) {
                        v.priceEachNguoiLon = u.priceEachNguoiLon;
                    }
                    if (u.priceEachTreEm >= 0) {
                        v.priceEachTreEm = u.priceEachTreEm;
                    }
                    if (u.priceEachNguoiGia >= 0) {
                        v.priceEachNguoiGia = u.priceEachNguoiGia;
                    }
                    if (u.netEachNguoiLon >= 0) {
                        v.netEachNguoiLon = u.netEachNguoiLon;
                    }
                    if (u.netEachTreEm >= 0) {
                        v.netEachTreEm = u.netEachTreEm;
                    }
                    if (u.netEachNguoiGia >= 0) {
                        v.netEachNguoiGia = u.netEachNguoiGia;
                    }
                }
            })
            // Tra lai gia tri mac dinh cho object mau
            this.updateAllGiaPhienBanTheoNgay = {

                priceEachNguoiLon: 0,
                priceEachTreEm: 0,
                priceEachNguoiGia: 0,
                netEachNguoiLon: 0,
                netEachTreEm: 0,
                netEachNguoiGia: 0
            }

        },
        on_filterGiaPhienBanTheoNgay() {
            //console.log(this.filterGiaPhienBanTheoNgay);
            this.currentGiaPhienBanTheoNgay.forEach(v => {
                v.filterd = false;
            })
            let pickedDayOfWeek = [];
            this.filterGiaPhienBanTheoNgay.dayOfWeek.forEach(v => {
                if (v.checked) {
                    pickedDayOfWeek.push(v.label)
                }
            })
            let result = JSON.parse(JSON.stringify(this.currentGiaPhienBanTheoNgay));
            if (this.filterGiaPhienBanTheoNgay.from) {
                result = result.filter(r => moment(r.date) >= moment(this.filterGiaPhienBanTheoNgay.from));
            }
            if (this.filterGiaPhienBanTheoNgay.to) {
                result = result.filter(r => moment(r.date) <= moment(this.filterGiaPhienBanTheoNgay.to));
            }
            if (pickedDayOfWeek.length > 0) {
                result = result.filter(r => pickedDayOfWeek.indexOf(r.dayOfWeek) >= 0);
            }
            let dateFilterd = result.map(r => r.date);

            this.currentGiaPhienBanTheoNgay.forEach(v => {
                if (dateFilterd.indexOf(v.date) >= 0) {
                    v.filterd = true;
                }
            })
        },
        onFormatDate(date) {
            return moment(date).format('DD/MM/YYYY')
        },
        DoAddGiaPhienBanTheoNgay(item) {
            this.currentSelectedOptionCombination = item.selectedOptions.toString()
            this.$refs.pByDateModal.show();
            let data = {
                selectedOptions: item.selectedOptions.toString(),
                productId: this.currentProductId
            }
            this.getGiaPhienBanTheoNgay(data).then(response => {
                this.currentGiaPhienBanTheoNgay = response;
                this.currentGiaPhienBanTheoNgay.forEach(v => {
                    v.filterd = true;
                })
                // console.log(this.currentGiaPhienBanTheoNgay);
            })
        },
        updateProductOptions() {
            //console.log(this.zoneTreeViewOptionById)
            console.log('optionValue when mouted', this.optionValues)
            // Đặt cờ isUpdating để ngăn chặn các lời gọi hàm tiếp theo
            if (this.zoneTreeViewOptionById) {
                if (this.selectedProductOptions.length > 0) {
                    this.selectedProductOptions = [];
                }
                //sort lại optionValue ở đây
                this.optionValues.sort((a, b) => a - b)
                this.convertdArr = [];
                //debugger
                this.optionValues.forEach((item, index) => {
                    let _parent = this.zoneTreeViewOptionById.find(r => r.id == item);
                    let _childs = this.zoneTreeViewOptionById.filter(r => r.parentId == item);
                    ////console.log(_childs)
                    let obj = {
                        parent: _parent,
                        childs: _childs
                    }
                    ////console.log(obj)
                    let arrItem = []
                    obj.childs.forEach((c, index_c) => {
                        arrItem.push(c.id)
                    })
                    console.log(item)
                    this.convertdArr.push(arrItem)
                    this.selectedProductOptions.push(obj);
                })
                //Bat dau kiem tra o day
                console.log(this.selectedProductOptions)
                if (this.convertdArr.length > 0) {
                    if (this.objRequestPhienBans) {
                        this.objRequestPhienBans.forEach((pb, indexPb) => {
                            let isAdd = "";
                            if (pb.selectedOptions.length < this.convertdArr.length) {
                                isAdd = "add"
                            }
                            else if (pb.selectedOptions.length > this.convertdArr.length) {
                                isAdd = "minus"
                            }
                            else {

                            }

                            if (isAdd === "add") {
                                for (var i = 0; i < this.convertdArr.length; i++) {
                                    //Kiem tra xem arr convertedArr[i] co chua phan tu pb.selectedOptions[i] khong
                                    //Neu khong co thi them vao mang pb.selectedOptions[i] gia tri -1
                                    //neu do dai ma khong dai bang gia tri thu i thi push them vao
                                    console.log(this.convertdArr[i], pb.selectedOptions[i])
                                    if (!this.convertdArr[i].includes(pb.selectedOptions[i])) {
                                        pb.selectedOptions.splice(i, 0, -1)
                                    }
                                    if (pb.selectedOptions.length < i + 1) {
                                        pb.selectedOptions.push(-1)
                                    }
                                }
                            }
                            if (isAdd === "minus") {
                                for (var i = 0; i < this.convertdArr.length; i++) {
                                    //Kiem tra xem arr convertedArr[i] co chua phan tu pb.selectedOptions[i] khong
                                    //Neu khong co thi them vao mang pb.selectedOptions[i] gia tri -1
                                    //neu do dai ma khong dai bang gia tri thu i thi push them vao

                                    if (!this.convertdArr[i].includes(pb.selectedOptions[i])) {
                                        pb.selectedOptions.splice(i, 1)
                                    }
                                }
                            }
                        })
                    }
                }
            }

        },
        onUploadSerialClick() {
            // Mở input file khi click vào nút "Upload Data"
            this.$refs.fileInput.click();
        },
        async handleFileChange(event) {
            const selectedFile = event.target.files[0];

            // Kiểm tra xem có file được chọn không
            if (!selectedFile) {
                //////console.log('Không có file nào được chọn.');
                return;
            }

            // Lấy productId từ nơi bạn lấy dữ liệu, ví dụ như từ state hoặc props
            const productId = this.$route.params.id; // Đảm bảo bạn đã khai báo và gán giá trị cho this.productId

            try {
                // Tạo formData để chứa file và các tham số khác
                const formData = new FormData();
                formData.append('file', selectedFile);
                formData.append('productId', productId); // Thêm tham số productId vào formData

                this.fmFileUploadExcel_ImportSeiralNumber(formData).then((response) => {
                    ////console.log(response);
                })

                // Gửi formData lên server bằng fetch hoặc axios
                const response = await fetch('/FileUploadV2/import_serial_sim', {
                    method: 'POST',
                    body: formData
                });

                if (!response.ok) {
                    throw new Error('Lỗi khi gửi file lên server.');
                }

                ////console.log('File đã được gửi thành công lên server.');

                // Đọc dữ liệu từ file Excel và in ra cột 1 và 2
                const excelData = await response.json();
                ////console.log('Dữ liệu từ file Excel:');
                for (const row of excelData) {
                    ////console.log('Cột 1:', row.column1, ', Cột 2:', row.column2);
                }
            } catch (error) {
                console.error('Đã xảy ra lỗi:', error.message);
            }
        },

        tour_DoDelThuTucVisa(index) {
            if (this.ThuTucVisa.length > 0)
                this.ThuTucVisa.splice(index, 1);
        },
        tour_DoAddThuTucVisa() {
            let tourItem = {
                tieuDe: "",
                noiDung: ""
            };
            this.ThuTucVisa.push(tourItem);
        },
        tour_DoDelThongTinTour(index) {
            if (this.ThongTinTour.length > 0)
                this.ThongTinTour.splice(index, 1);
        },
        tour_DoAddThongTinTour() {
            let tourItem = {
                tieuDe: "",
                noiDung: ""
            };
            this.ThongTinTour.push(tourItem);
        },
        tour_DoDelLichTour(index) {
            if (this.LichTrinhTour.length > 0)
                this.LichTrinhTour.splice(index, 1);
        },
        tour_DoAddLichTour() {
            this.LichTrinhTour.push({
                tieuDe: "",
                noiDung: ""
            });
        },
        tour_getOrSetData_lichTour(value) {
            //////console.log(value)
            var dt = JSON.parse(value);
            ////console.log(dt)
            this.LichTrinhTour[dt.index].noiDung = dt.content;
        },
        tour_getOrSetData_thongTinTour(value) {
            //////console.log(value)
            var dt = JSON.parse(value);
            ////console.log(dt)
            this.ThongTinTour[dt.index].noiDung = dt.content;

        },
        tour_getOrSetData_thuTucVisa(value) {
            //////console.log(value)
            var dt = JSON.parse(value);
            ////console.log(dt)
            this.ThuTucVisa[dt.index].noiDung = dt.content;
        },
        loadOptions({ action, searchQuery, callback }) {
            if (action === ASYNC_SEARCH) {
                var $this = this;
                simulateAsyncOperation(() => {
                    const options = $this.ListArticle.filter(function (item) {
                        return (item.label.toLowerCase().indexOf(searchQuery.toLowerCase()) !== -1)
                    }).slice(0, 30);
                    callback(null, options)
                })
            }
        },
        loadOptionArticleDefault(ArticleId) {
            if (ArticleId != null && ArticleId.length > 0) {
                var split = ArticleId.split(",");
                var $this = this;

                this.ListArticle.forEach(function (item, index) {
                    if (split.some(x => parseInt(x) == item.key)) {
                        $this.ArticleValues.push(item);
                        //$this.ArticleValues.push(item.id);
                    }
                });
            }

        },
        getOrSetData(value) {
            this.objRequestDetail.content = value;
        },
        sumTotal() {
            this.objRequest.discountPrice = this.objRequest.price - (this.objRequest.discountPercent * (this.objRequest.price / 100))
        },
        pathImgs(path) {
            return pathImg(path);
        },
        urlBases(path) {
            return urlBase(path);
        },
        getUser() {
            //debugger-
            var data = JSON.parse(localStorage.getItem('currentUser'));
            return JSON.parse(localStorage.getItem('currentUser'));
        },
        onGetSuppliers() {
            this.getAllUserSupplier().then(response => {
                // console.log(response);
                this.userSuppliers = response.map(r => ({
                    label: r.fullName,
                    id: r.email
                }))

            })
        },
        onChangePaging() {
            this.isLoading = true;
            let initial = this.$route.query.initial;
            initial = typeof initial != "undefined" ? initial.toLowerCase() : "";
            this.getZones(1).then(respose => {

                var data = respose.listData;
                ////console.log(data)
                this.ZoneOptions = unflatten(data);
                ////console.log(this.ZoneOptions);
                //var countries = respose.listData.filter(element => element.parentId == 3 || element.id == 3);
                //////console.log(countries);
                //this.countryOptions = unflatten(countries);
            });
            this.getZones(5).then(respose => {
                var data = respose.listData;

                this.DiemDenOptions = unflatten(data);
            });
            this.getZones(4).then(respose => {
                var data = respose.listData;

                this.TagOptions = unflatten(data);
            });
            this.getZones(6).then(response => {
                var data = response.listData;
                this.BookingNotesOptions = unflatten(data)
            })
            this.getZones(8).then(response => {
                var data = response.listData;
                this.CouponZoneOptions = unflatten(data)
            })
            this.getZones(3).then(response => { //Lay danh sach option san pham
                var data = response.listData;
                ////console.log(data)
                this.OpOptions = unflatten(data);
                ////console.log(this.OpOptions)
            })
            this.getZonesTreeviewById(0).then((r) => {
                r.listData.forEach(v => {
                    this.zoneTreeViewOptionById.push(v)
                })
                this.updateProductOptions();
            })

            this.isLoading = false;
        },
        slugM: function () {
            //debugger-
            if (this.objRequestDetail != null && this.objRequestDetail != undefined) {
                //this.objRequestDetail.url = "";
                this.objRequestDetail.url = slug(this.objRequestDetail.title);
            }
        },

        defaultObj: function () {
            this.objRequestDetail = Object.assign({}, this.objRequestDetail);
            this.objRequestDetail.id = "00000000-0000-0000-0000-000000000000";
            this.objRequestDetail.title = "";
            this.objRequestDetail.url = "";
            this.objRequestDetail.content = "";
            this.objRequestDetail.description = "";
            this.Tags = [];
        },

        onLoadLang() {
            //debugger-
            this.isLoading = true;
            this.getLanguageById(this.objRequest.id).then(respose => {
                if (respose.listData != null && respose.listData.length > 0) {
                    //////console.log(respose);
                    //debugger-
                    this.objRequestDetails = respose.listData;
                    if (this.objRequestDetails != null && this.objRequestDetails != undefined && this.objRequestDetails.length > 0) {
                        let obj = this.objRequestDetails.filter(x => x.languageCode == 'vi-VN     ');
                        if (obj != null && obj.length > 0) {
                            console.log(this.objRequestDetails)
                            this.objRequestDetail = obj[0];
                            //
                            if (obj[0].metaNoIndex != null) {
                                let objNoIndex = JSON.parse(obj[0].metaNoIndex);
                                this.statusNoindex = objNoIndex.Status;
                            }
                            if (obj[0].metaCanonical != null) {
                                let objCanonical = JSON.parse(obj[0].metaCanonical);
                                this.statusCanonical = objCanonical.Status;
                                this.valueCanonical = objCanonical.Value;
                            }
                            ////console.log(obj[0])
                            if (obj[0].lichTour != null)
                                this.LichTrinhTour = JSON.parse(obj[0].lichTour);
                            if (obj[0].thongTinTour != null)
                                this.ThongTinTour = JSON.parse(obj[0].thongTinTour);
                            if (obj[0].thuTucVisa != null)
                                this.ThuTucVisa = JSON.parse(obj[0].thuTucVisa);
                            //////console.log(this.LichTrinhTour, this.ThongTinTour, this.ThuTucVisa)
                        } else {
                            this.objRequestDetail = this.objRequestDetails[0];
                            console.log(this.objRequestDetail)
                            //
                            if (this.objRequestDetails[0].metaNoIndex != null) {
                                let objNoIndex = JSON.parse(this.objRequestDetails[0].metaNoIndex);
                                this.statusNoindex = objNoIndex.Status;
                            }
                            if (this.objRequestDetails[0].metaCanonical != null) {
                                let objCanonical = JSON.parse(this.objRequestDetails[0].metaCanonical);
                                this.statusCanonical = objCanonical.Status;
                                this.valueCanonical = objCanonical.Value;
                            }
                            ////console.log(this.objRequestDetails[0].lichTour)
                            if (this.objRequestDetails[0].lichTour != null)
                                this.LichTrinhTour = JSON.parse(this.objRequestDetails[0].lichTour);
                            else
                                this.tour_DoAddLichTour();
                            if (this.objRequestDetails[0].thongTinTour != null)
                                this.ThongTinTour = JSON.parse(this.objRequestDetails[0].thongTinTour);
                            else
                                this.tour_DoAddThongTinTour();
                            if (this.objRequestDetails[0].thuTucVisa != null)
                                this.ThuTucVisa = JSON.parse(this.objRequestDetails[0].thuTucVisa);
                            else
                                this.tour_DoAddThuTucVisa();
                            //

                        }

                        this.Tags = this.convertTags(this.objRequestDetail.listTagName);
                    } else {
                        this.defaultObj();
                        this.statusNoindex = false;
                        this.statusCanonical = false;
                        this.valueCanonical = "";
                    }
                }
                this.langSelected = this.objRequestDetail.languageCode;
            });
            this.contentContent = this.objRequestDetail.content;

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
            //debugger
            this.objRequest.metaFile = JSON.stringify(this.FileMeta);
            this.objRequest.avatarArray = this.ListImg.join();
            this.objRequest.propertyId = this.ListProperty.join();
            this.objRequest.color = this.ListColors.join();
            this.objRequest.modifyBy = this.objUser.userName;
            this.objRequest.ParentId = this.ProductParent;
            this.objRequest.articleId = this.ArticleValues.map(x => x.key).join();
            this.objRequest.phuongTien = this.requestPhuongTien.join();

            let lstzone = [];
            var requestId = this.objRequest.id;
            this.ZoneValues.forEach(function (item, index) {
                var isPrimarykey = false;
                if (index == 0) {
                    isPrimarykey = true;
                }
                try {
                    var obj = {};
                    obj.ZoneId = item;
                    obj.ProductId = requestId;
                    obj.IsPrimary = isPrimarykey;
                    /*obj.IsHot = 999;*/
                    obj.BigThumb = "";
                    lstzone.push(obj);
                }
                catch (ex) {
                    ////console.log(ex);
                }
            });
            this.DiemDenValues.forEach(function (item, index) {
                var isPrimarykey = false;
                if (index == 0) {
                    isPrimarykey = true;
                }
                try {
                    var obj = {};
                    obj.ZoneId = item;
                    obj.ProductId = requestId;
                    obj.IsPrimary = isPrimarykey;
                    //obj.IsHot = false;
                    obj.BigThumb = "";
                    lstzone.push(obj);
                }
                catch (ex) {
                    ////console.log(ex);
                }
            });
            this.TagValues.forEach(function (item, index) {
                var isPrimarykey = false;
                if (index == 0) {
                    isPrimarykey = true;
                }
                try {
                    var obj = {};
                    obj.ZoneId = item;
                    obj.ProductId = requestId;
                    obj.IsPrimary = isPrimarykey;
                    //obj.IsHot = false;
                    obj.BigThumb = "";
                    lstzone.push(obj);
                }
                catch (ex) {
                    ////console.log(ex);
                }
            });
            this.NoteValues.forEach(function (item, index) {
                var isPrimarykey = false;
                if (index == 0) {
                    isPrimarykey = true;
                }
                try {
                    var obj = {};
                    obj.ZoneId = item;
                    obj.ProductId = requestId;
                    obj.IsPrimary = isPrimarykey;
                    //obj.IsHot = false;
                    obj.BigThumb = "";
                    lstzone.push(obj);
                }
                catch (ex) {
                    ////console.log(ex);
                }
            });
            this.CouponZoneValues.forEach(function (item, index) {
                var isPrimarykey = false;
                if (index == 0) {
                    isPrimarykey = true;
                }
                try {
                    var obj = {};
                    obj.ZoneId = item;
                    obj.ProductId = requestId;
                    obj.IsPrimary = isPrimarykey;
                    //obj.IsHot = false;
                    obj.BigThumb = "";
                    lstzone.push(obj);
                }
                catch (ex) {
                    ////console.log(ex);
                }
            });
            this.optionValues.forEach(function (item, index) {
                var isPrimarykey = false;
                if (index == 0) {
                    isPrimarykey = true;
                }
                try {
                    var obj = {};
                    obj.ZoneId = item;
                    obj.ProductId = requestId;
                    obj.IsPrimary = isPrimarykey;
                    //obj.IsHot = false;
                    obj.BigThumb = "";
                    lstzone.push(obj);
                }
                catch (ex) {
                    ////console.log(ex);
                }
            });
            ////console.log(this.optionValues)
            this.objRequest.ProductInZone = lstzone;
            //this.objRequest.avatar = this.Img;
            ////console.log(this.objRequestPhienBans);

            var data = {
                product: this.objRequest,
                phienBans: JSON.parse(JSON.stringify(this.objRequestPhienBans)),
                cancelPolicies: JSON.parse(JSON.stringify(this.objRequestCancelPolicy))
            }
            //console.log(data);
            if (this.objRequest.id > 0) {
                this.updateProduct(data)
                    .then(response => {
                        this.isLoading = false;
                        if (response.success == true) {

                            this.$toast.success(response.message, {});
                            this.isLoading = false;
                            // router.push({ path: `/admin/product/list` });
                        }
                        else {
                            this.$toast.error(response.message, {});
                            this.isLoading = false;

                        }

                    })
                    .catch(e => {
                        this.$toast.error(msgNotify.error + ". error:" + e, {});
                        this.isLoading = false;
                    })
            } else {
                this.addProduct(data.product)
                    .then(response => {
                        //console.log(response)
                        this.isLoading = false;
                        if (response.success == true) {


                            this.$toast.success(response.message, {});
                            this.objRequest.id = response.id;
                            this.objRequestDetail.productid = response.id;
                            this.$router.push({
                                path: "/admin/product/edit/" + response.id,
                            });
                            this.isLoading = false;
                        }
                        else {
                            this.$toast.error(response.message, {});
                            this.isLoading = false;
                        }
                    })
                    .catch(e => {
                        this.$toast.error(msgNotify.error + ". error:" + e, {});
                        this.isLoading = false;
                    })
            }
        },
        getSelectedUser(node, id) {
            this.objRequest.ZoneId = node.id;
        },
        convertTags(value) {
            if (value != null && value != undefined && value.length > 0) {
                return value.map(x => {
                    return { key: slug(x), value: x }
                });
            }
            return value;
        },
        onChangeSelectd() {

            if (this.objRequestDetails != null && this.objRequestDetails.length > 0) {

                let lang = this.langSelected || "vi-VN     ";
                let lstObjLang = this.objRequestDetails.filter(function (item) {
                    return item.languageCode.trim() === lang.trim()
                });
                if (lstObjLang != null && lstObjLang != undefined && lstObjLang.length > 0) {
                    this.objRequestDetail = lstObjLang[0];
                    //
                    if (lstObjLang[0].metaNoIndex != null) {
                        let objNoIndex = JSON.parse(lstObjLang[0].metaNoIndex);
                        this.statusNoindex = objNoIndex.Status;
                    }
                    if (lstObjLang[0].metaCanonical != null) {
                        let objCanonical = JSON.parse(lstObjLang[0].metaCanonical);
                        this.statusCanonical = objCanonical.Status;
                        this.valueCanonical = objCanonical.Value;
                    }
                    ////console.log(obj[0])
                    if (lstObjLang[0].lichTour != null)
                        this.LichTrinhTour = JSON.parse(lstObjLang[0].lichTour);
                    if (lstObjLang[0].thongTinTour != null)
                        this.ThongTinTour = JSON.parse(lstObjLang[0].thongTinTour);
                    if (lstObjLang[0].thuTucVisa != null)
                        this.ThuTucVisa = JSON.parse(lstObjLang[0].thuTucVisa);
                    //
                    this.Tags = this.convertTags(this.objRequestDetail.listTagName);
                } else {
                    this.defaultObj();
                    this.statusNoindex = false;
                    this.statusCanonical = false;
                    this.valueCanonical = "";
                }
            }
            this.objRequestDetail.languageCode = this.langSelected;

        },

        DoAddDetail() {
            this.objRequestDetail.productId = this.objRequest.id;
            //
            let objMetaNoIndex = {
                Value: "",
                Status: this.statusNoindex
            }
            let objMetaCanonical = {
                Value: this.valueCanonical,
                Status: this.statusCanonical
            }
            this.objRequestDetail.metaNoIndex = JSON.stringify(objMetaNoIndex);
            this.objRequestDetail.metaCanonical = JSON.stringify(objMetaCanonical)

            this.objRequestDetail.lichTour = JSON.stringify(this.LichTrinhTour)
            this.objRequestDetail.thongTinTour = JSON.stringify(this.ThongTinTour)
            this.objRequestDetail.thuTucVisa = JSON.stringify(this.ThuTucVisa)
            //

            ////console.log(this.LichTrinhTour)
            ////console.log(this.ThongTinTour)
            ////console.log(this.ThuTucVisa)
            this.objRequestDetail.listTagName = this.Tags.map(x => {
                return x.value
            });

            this.addProductInLanguage(this.objRequestDetail)
                .then(response => {
                    if (response.success == true) {
                        if (response.data != null && response.data != undefined) {
                            this.objRequestDetail.id = response.data.Id;
                            if (!this.objRequestDetails.some(x => x.languageCode == this.objRequestDetail.languageCode)) {
                                this.objRequestDetails.push(this.objRequestDetail);
                            }
                        }
                        this.$toast.success(response.message, {});
                        this.$router.push({ path: `/admin/product/list` });
                    }
                    else {
                        this.$toast.error(response.message, {});
                    }
                })
                .catch(e => {
                    this.$toast.error(msgNotify.error + ". Error:" + e, {});
                    this.isLoading = false;
                });
        },
        DoAttackFile(value) {
            let vm = this;
            if (this.choseImg == "ListImg") {
                value.map(x => {
                    vm.ListImg.push(x.path)
                });
            } else if (this.choseImg == "Img") {
                vm.objRequest.avatar = value[0].path;
            }
            else if (this.choseImg == "ImgS") {
                vm.objRequestDetail.socialImage = value[0].path;
            }
            else if (this.choseImg == "ImgIconCorner") {
                vm.objRequestDetail.iconCornerTag = value[0].path;
            }
            else if (this.choseImg == "fileVideo") {
                vm.FileMeta.FileVideo = value[0].path;
            }
            else if (this.choseImg == "file360") {
                vm.FileMeta.File360 = value[0].path;
            }
            else if (this.choseImg == "filedowload") {
                vm.FileMeta.FileDowload = value[0].path;
            }
        },
        openImg(img) {
            this.choseImg = img;
            if (img == "ListImg") {
                //EventBus.$emit("FileSelected", );
                EventBus.$emit(this.mikey1, 'multi'); // '': select one, 'multi': select multi file
            } else {
                //EventBus.$emit("FileSelected", this.Img);
                EventBus.$emit(this.mikey1, ''); // '': select one, 'multi': select multi file
            }
        },
        LoadListOld(productid) {
            this.getProductOldRenewal(productid).then(respose => {

                this.ListOldRenewal = respose.data;

            });
        },
        DoAddRenewal() {

            if (!this.objRequestOldRenewal.descriptionType || !this.objRequestOldRenewal.priceRefer)
                return this.$toast.error("Không bỏ trống tình trạng hoặc giá", {});
            this.objRequestOldRenewal.teamid = this.ListOldRenewal.length + 1;
            this.ListOldRenewal.push(this.objRequestOldRenewal);
            this.objRequestOldRenewal = {};
        },
        AddNewPhienBan() {
            //debugger
            //if (!this.objRequestOldRenewal.descriptionType || !this.objRequestOldRenewal.priceRefer)
            //    return this.$toast.error("Không bỏ trống tình trạng hoặc giá", {});
            //this.objRequestOldRenewal.teamid = this.ListOldRenewal.length + 1;
            //this.ListOldRenewal.push(this.objRequestOldRenewal);
            //this.objRequestOldRenewal = {};
            this.objRequestPhienBans.push({
                priceEachNguoiLon: 0,
                priceEachTreEm: 0,
                netEachNguoiLon: 0,
                netEachTreEm: 0,
                priceEachNguoiGia: 0,
                netEachNguoiGia: 0,
                selectedOptions: [],
            });

        },
        AddNewTopUp() {
            this.objRequestTopUp.push({
                id: 0,
                parentId: 0,
                title: "",
                validaty: "",
                price: 0,
                smsNumber: 0,
                phoneMinute: 0,
                simType: "",
                gradientColor: "",
                coverage: "",
                dataLimit: "",

            })
        },

        DoDelTopUp(i) {
            this.objRequestTopUp.splice(i, 1);
        },

        DoDelPhienBan(i) {
            this.objRequestPhienBans.splice(i, 1);
        },


        DoEditRenewal() {
            this.objRequestOldRenewal = this.RdoOldRenewal;
            if (this.RdoOldRenewal.id) {
                this.ListOldRenewal = this.ListOldRenewal.filter(x => x.id != this.RdoOldRenewal.id)
            }
            else {
                this.ListOldRenewal = this.ListOldRenewal.filter(x => x.teamid != this.RdoOldRenewal.teamid)
            }
        },
        DoDelRenewal() {
            let item = this.ListOldRenewal.filter(x => x.teamid == this.RdoOldRenewal.teamid)
            if (item) {
                if (this.RdoOldRenewal.id) {
                    this.deleteOldRenewal(this.RdoOldRenewal.id)
                        .then(response => {
                            if (response.success) {
                                this.$toast.success(response.message, {});
                                this.ListOldRenewal = this.ListOldRenewal.filter(x => x.id != this.RdoOldRenewal.id)
                            }
                            else {
                                this.$toast.error(response.message, {});
                            }

                        })
                }
                else {
                    this.ListOldRenewal = this.ListOldRenewal.filter(x => x.teamid != this.RdoOldRenewal.teamid)
                }
            }
        }
    },
    watch: {
        isLoadLang: function (val) {
            this.onLoadLang();
        },
        filterGiaPhienBanTheoNgay: {
            handler(newVal, oldVal) {
                this.filterGiaPhienBanTheoNgay = newVal;
                this.on_filterGiaPhienBanTheoNgay();
            },
            deep: true
        }
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

.tags-input-remove:before,
.tags-input-remove:after {
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
.typeahead-badges>span {
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

.modal-body {
    max-height: 500px;
    /* Set the desired height */
    overflow-y: auto;
    /* Enable vertical scrolling */
}
</style>
