import Vue from 'vue'
import Vuex from 'vuex'
import axios from 'axios'
import store from './store'
import { sync } from 'vuex-router-sync'
import App from './App.vue'
import BootstrapVue from 'bootstrap-vue'
import CoreuiVue from '@coreui/vue';
import { BadgePlugin } from 'bootstrap-vue'


import Toast from "vue-toastification";
import Loading from 'vue-loading-overlay';
import vSelect from 'vue-select'
import CKEditor from 'ckeditor4-vue';


import Vuelidate from 'vuelidate';
//import VueDatepickerUi from 'vue-datepicker-ui'
//import 'vue-datepicker-ui/lib/vuedatepickerui.css';
import VueI18n from 'vue-i18n';

Vue.use(Loading,
    {});



import "vue-toastification/dist/index.css";
import { router } from "./router";


Vue.component('v-select', vSelect);


const options = {
    position: "top-right",
    timeout: 2769,
    closeOnClick: true,
    pauseOnFocusLoss: true,
    pauseOnHover: true,
    draggable: true,
    draggablePercent: 0.1,
    hideCloseButton: false,
    hideProgressBar: false,
    icon: true,
    transition: "Vue-Toastification__fade",
    maxToasts: 8,
    newestOnTop: true
};

Vue.component('v-select', vSelect);
Vue.use(VueI18n);

const messages = {
    en: {
        partnerCoupon: 'Partner (Coupon)',
        orderListTitle: 'Partner coupon order list',
        totalValue: 'Total value',
        totalOrders: 'Total orders',
        created: 'Created',
        serviceAccepted: 'Service accepted',
        serviceUsed: 'Service used',
        serviceRejected: 'Service rejected',
        cancelRequested: 'Cancellation requested',
        canceled: 'Canceled',
        keyword: 'Keyword',
        fromDate: 'From date',
        toDate: 'To date',
        serviceStatus: 'Service status',
        all: 'ALL',
        delete: 'Delete',
        action: 'Action',
        exportReport: 'Export report',
        orderInfo: 'Order info',
        customerInfo: 'Customer info',
        productInfo: 'Product info',
        status: 'Status',
        TAO_MOI: 'Created',
        CHAP_NHAN_DICH_VU: 'Service accepted',
        DA_SU_DUNG_DICH_VU: 'Service used',
        TU_CHOI_DICH_VU: 'Service rejected',
        YEU_CAU_HUY: 'Cancellation requested',
        DA_HUY: 'Canceled',
        ALL: "All",
        orderCode: 'Order Code',
        customerInfo: 'Customer Information',
        productInfo: 'Product Information',
        orderInfo: 'Order Information',
        status: 'Status',
        fullName: 'Name',
        phoneNumber: 'Phone Number',
        email: 'Email',
        product: 'Product',
        package: 'Package',
        options: 'Options',
        partner: 'Partner',
        orderDate: 'Order Date',
        serviceDate: 'Service Date',
        adultQuantity: 'Adult x',
        childQuantity: 'Child x',
        voucher: 'Voucher',
        sellingPrice: 'Selling Price',
        requestCancellationTime: 'Cancellation Request Time',
        refundPolicy: 'Refund Policy',
        rollbackPercentage: '%',

        loginTitle: 'Login',
        loginSubtitle: 'Login to your account',
        emailPlaceholder: 'Login Email',
        passwordPlaceholder: 'Password',
        loginButton: 'Login',
        emailRequired: 'Email is required',
        passwordRequired: 'Password is required',
        errorMessage: 'Error',
    },
    vi: {
        partnerCoupon: 'Đối tác (Coupon)',
        orderListTitle: 'Danh sách order đối tác coupon',
        totalValue: 'Tổng giá trị',
        totalOrders: 'Tổng đơn hàng',
        created: 'Tạo mới',
        serviceAccepted: 'Chấp nhận dịch vụ',
        serviceUsed: 'Đã sử dụng dịch vụ',
        serviceRejected: 'Từ chối dịch vụ',
        cancelRequested: 'Yêu cầu hủy',
        canceled: 'Đã hủy',
        keyword: 'Keyword',
        fromDate: 'Từ ngày',
        toDate: 'Đến ngày',
        serviceStatus: 'Trạng thái dịch vụ',
        all: 'Tất cả',
        delete: 'Xóa',
        action: 'Hành động',
        exportReport: 'Xuất báo cáo',
        orderInfo: 'Thông tin đơn hàng',
        customerInfo: 'Thông tin khách hàng',
        productInfo: 'Thông tin sản phẩm',
        status: 'Trạng thái',
        TAO_MOI: 'Tạo mới',
        CHAP_NHAN_DICH_VU: 'Chấp nhận dịch vụ',
        DA_SU_DUNG_DICH_VU: 'Xác nhận đã sử dụng dịch vụ',
        TU_CHOI_DICH_VU: 'Từ chối dịch vụ',
        YEU_CAU_HUY: 'Yêu cầu hủy',
        DA_HUY: 'Hủy dịch vụ',
        ALL: "Tất cả",
        orderCode: 'Mã đơn hàng',
        customerInfo: 'Thông tin Khách hàng',
        productInfo: 'Thông tin sản phẩm',
        orderInfo: 'Thông tin đơn hàng',
        status: 'Trạng thái',
        fullName: 'Tên',
        phoneNumber: 'SĐT',
        email: 'Email',
        product: 'SP',
        package: 'Package',
        options: 'Options',
        partner: 'Đối tác',
        orderDate: 'Ngày đặt dịch vụ',
        serviceDate: 'Ngày SD dịch vụ',
        adultQuantity: 'Ng.Lớn x',
        childQuantity: 'Tr.Em x',
        voucher: 'Mã giảm giá',
        sellingPrice: 'Giá bán',
        requestCancellationTime: 'Thời gian Y/C hủy',
        refundPolicy: 'Chính sách hoàn tiền',
        rollbackPercentage: '%',
        loginTitle: 'Đăng nhập',
        loginSubtitle: 'Đăng nhập vào tài khoản của bạn',
        emailPlaceholder: 'Email đăng nhập',
        passwordPlaceholder: 'Mật khẩu',
        loginButton: 'Đăng nhập',
        emailRequired: 'Bắt buộc phải nhập Email',
        passwordRequired: 'Bắt buộc phải nhập mật khẩu',
        errorMessage: 'Lỗi',
    },
    zh: {
        partnerCoupon: '合作伙伴（优惠券）',
        orderListTitle: '合作伙伴优惠券订单列表',
        totalValue: '总价值',
        totalOrders: '总订单',
        created: '新建',
        serviceAccepted: '接受服务',
        serviceUsed: '已使用服务',
        serviceRejected: '拒绝服务',
        cancelRequested: '取消请求',
        canceled: '已取消',
        keyword: '关键词',
        fromDate: '开始日期',
        toDate: '结束日期',
        serviceStatus: '服务状态',
        all: '全部',
        delete: '删除',
        action: '操作',
        exportReport: '导出报告',
        orderInfo: '订单信息',
        customerInfo: '客户信息',
        productInfo: '产品信息',
        status: '状态',
        TAO_MOI: '新建',
        CHAP_NHAN_DICH_VU: '接受服务',
        DA_SU_DUNG_DICH_VU: '确认已使用服务',
        TU_CHOI_DICH_VU: '拒绝服务',
        YEU_CAU_HUY: '取消请求',
        DA_HUY: '已取消',
        ALL: "全部",
        orderCode: '订单代码',
        customerInfo: '客户信息',
        productInfo: '产品信息',
        orderInfo: '订单信息',
        status: '状态',
        fullName: '姓名',
        phoneNumber: '电话号码',
        email: '电子邮件',
        product: '产品',
        package: '套餐',
        options: '选项',
        partner: '合作伙伴',
        orderDate: '订单日期',
        serviceDate: '服务日期',
        adultQuantity: '成人 x',
        childQuantity: '儿童 x',
        voucher: '代金券',
        sellingPrice: '销售价格',
        requestCancellationTime: '取消请求时间',
        refundPolicy: '退款政策',
        rollbackPercentage: '%',
        loginTitle: '登录',
        loginSubtitle: '登录到您的账户',
        emailPlaceholder: '登录邮箱',
        passwordPlaceholder: '密码',
        loginButton: '登录',
        emailRequired: '邮箱是必填项',
        passwordRequired: '密码是必填项',
        errorMessage: '错误',
    }
};

// Tạo instance của VueI18n

// Lấy ngôn ngữ từ localStorage, nếu không có thì dùng 'en' làm mặc định
const savedLanguage = localStorage.getItem('language') || 'vi';

const i18n = new VueI18n({
    locale: savedLanguage, // Ngôn ngữ mặc định (hoặc đã được lưu trước đó)
    messages
});


Vue.use(CoreuiVue);
Vue.use(Toast, options);
//Vue.component('Datepicker', VueDatepickerUi)

Vue.prototype.$http = axios;

Vue.use(BootstrapVue);
Vue.use(BadgePlugin)

Vue.use(Vuex);

Vue.use(CKEditor);
Vue.use(Vuelidate);

sync(store, router);




const app = new Vue({
    store,
    router,
    ...App,
    i18n,
    render: h => h(App)
});

export {
    app,
    router,
    store
}
