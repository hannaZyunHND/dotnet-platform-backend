webpackJsonp([65],{

/***/ 1248:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _extends2 = __webpack_require__(8);

var _extends3 = _interopRequireDefault(_extends2);

var _regenerator = __webpack_require__(75);

var _regenerator2 = _interopRequireDefault(_regenerator);

var _asyncToGenerator2 = __webpack_require__(74);

var _asyncToGenerator3 = _interopRequireDefault(_asyncToGenerator2);

__webpack_require__(792);

var _constant = __webpack_require__(793);

var _constant2 = _interopRequireDefault(_constant);

var _vuex = __webpack_require__(180);

var _vueLoadingOverlay = __webpack_require__(376);

var _vueLoadingOverlay2 = _interopRequireDefault(_vueLoadingOverlay);

var _authenticationRepository = __webpack_require__(114);

var _router = __webpack_require__(189);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

exports.default = {
    name: "Profile",
    data: function data() {
        return {
            editorData: '<p>Content of the editor.</p>',
            editorConfig: {
                allowedContent: true,
                extraPlugins: ""
            },
            disabled: false,
            selectedFile: null,
            preview: '',
            isLoading: false,
            fullPage: false,
            color: "#007bff",
            objRequest: {},
            LanguageCodes: [],
            LanguageValues: null,
            currentUser: null
        };
    },
    created: function created() {
        var _this = this;

        return (0, _asyncToGenerator3.default)(_regenerator2.default.mark(function _callee() {
            return _regenerator2.default.wrap(function _callee$(_context) {
                while (1) {
                    switch (_context.prev = _context.next) {
                        case 0:
                            _this.GetById();

                        case 1:
                        case "end":
                            return _context.stop();
                    }
                }
            }, _callee, _this);
        }))();
    },

    components: {
        Loading: _vueLoadingOverlay2.default
    },

    mounted: function mounted() {},


    computed: (0, _extends3.default)({}, (0, _vuex.mapGetters)(["fileName"])),
    watch: {},
    methods: (0, _extends3.default)({}, (0, _vuex.mapActions)(["uploadFile", "updateProfile"]), {
        GetById: function GetById() {
            var _this2 = this;

            return (0, _asyncToGenerator3.default)(_regenerator2.default.mark(function _callee2() {
                return _regenerator2.default.wrap(function _callee2$(_context2) {
                    while (1) {
                        switch (_context2.prev = _context2.next) {
                            case 0:
                                _context2.next = 2;
                                return _authenticationRepository.authenticationRepository.getCurrentUser();

                            case 2:
                                _this2.objRequest = _context2.sent;

                                if (_this2.objRequest.avatar) {
                                    _this2.preview = "/uploads/" + _this2.objRequest.avatar;
                                }
                                _this2.isLoading = false;

                            case 5:
                            case "end":
                                return _context2.stop();
                        }
                    }
                }, _callee2, _this2);
            }))();
        },
        DoAddEdit: function DoAddEdit() {
            var _this3 = this;

            return (0, _asyncToGenerator3.default)(_regenerator2.default.mark(function _callee3() {
                var vm, result;
                return _regenerator2.default.wrap(function _callee3$(_context3) {
                    while (1) {
                        switch (_context3.prev = _context3.next) {
                            case 0:
                                vm = _this3;

                                _this3.isLoading = true;

                                if (!_this3.objRequest.id) {
                                    _context3.next = 7;
                                    break;
                                }

                                _context3.next = 5;
                                return _this3.updateProfile(_this3.objRequest);

                            case 5:
                                result = _context3.sent;

                                if (result.success == true) {
                                    _this3.$toast.success("tạo thành công", {});
                                    _this3.isLoading = false;
                                    _this3.$router.go(-1);
                                } else {
                                    _this3.$router.go(-1);
                                    _this3.$toast.error("cập nhật thất bại", {});
                                    _this3.isLoading = false;
                                }

                            case 7:
                            case "end":
                                return _context3.stop();
                        }
                    }
                }, _callee3, _this3);
            }))();
        },
        DoRefesh: function DoRefesh() {
            this.objRequest.Title = "";
        },
        GetRouterChangePassword: function GetRouterChangePassword() {
            _router.router.push('/admin/profile/change-password');
        },
        openUpload: function openUpload() {
            document.getElementById('file-field').click();
        },
        updatePreview: function updatePreview(e) {
            var _this4 = this;

            debugger;
            var reader,
                files = e.target.files;
            if (files.length === 0) {
                console.log('Empty');
            }
            this.selectedFile = files[0];
            reader = new FileReader();
            var data = new FormData();
            for (var x = 0; x < files.length; x++) {
                data.append("file" + x, files[x]);
            }
            this.uploadFile(data).then(function (response) {
                if (response.success == true) {
                    reader.onload = function (e) {
                        if (response.data[0].path != '/') {
                            _this4.objRequest.avatar = response.data[0].path;
                            _this4.preview = e.target.result;
                        } else _this4.$toast.error(response.data[0].messages, {});
                    };
                    reader.readAsDataURL(files[0]);
                } else {
                    _this4.$toast.error(_constant2.default.error + ". Error:" + e, {});
                    _this4.isLoading = false;
                }
            }).catch(function (e) {
                _this4.$toast.error(_constant2.default.error + ". Error:" + e, {});
                _this4.isLoading = false;
            });
        }
    })
};

/***/ }),

/***/ 1575:
/***/ (function(module, exports, __webpack_require__) {

module.exports={render:function (){var _vm=this;var _h=_vm.$createElement;var _c=_vm._self._c||_h;
  return _c('div', {
    staticStyle: {
      "display": "flex",
      "width": "100%"
    }
  }, [_c('loading', {
    attrs: {
      "active": _vm.isLoading,
      "height": 35,
      "width": 35,
      "color": _vm.color,
      "is-full-page": _vm.fullPage
    },
    on: {
      "update:active": function($event) {
        _vm.isLoading = $event
      }
    }
  }), _vm._v(" "), _c('b-tabs', {
    staticClass: "col-md-12",
    attrs: {
      "pills": ""
    }
  }, [_c('b-tab', {
    attrs: {
      "title": "1. Nhập thông tin tài khoản",
      "active": ""
    }
  }, [_c('div', {
    staticClass: "row productedit"
  }, [_c('div', {
    staticClass: "col-md-12"
  }, [_c('b-card', {
    staticClass: "mt-3 ",
    attrs: {
      "header": "Cập nhật tài khoản"
    }
  }, [_c('b-form', {
    staticClass: "form-horizontal"
  }, [_c('div', {
    staticClass: "row"
  }, [_c('div', {
    staticClass: "col-md-4"
  }, [_c('b-form-group', [_c('label', [_vm._v("Ảnh đại diện")]), _vm._v(" "), _c('b-form-file', {
    attrs: {
      "id": "file-field",
      "size": "sm"
    },
    on: {
      "change": _vm.updatePreview
    },
    model: {
      value: (_vm.objRequest.avatar),
      callback: function($$v) {
        _vm.$set(_vm.objRequest, "avatar", $$v)
      },
      expression: "objRequest.avatar"
    }
  }), _vm._v(" "), _c('img', {
    staticClass: "preview-image img-thumbnail",
    staticStyle: {
      "margin-top": "10px"
    },
    attrs: {
      "src": _vm.preview
    },
    on: {
      "click": _vm.openUpload
    }
  })], 1)], 1), _vm._v(" "), _c('div', {
    staticClass: "col-md-8"
  }, [_c('div', {
    staticClass: "col-md-12"
  }, [_c('b-form-group', {
    attrs: {
      "label": "Email"
    }
  }, [_c('b-form-input', {
    attrs: {
      "placeholder": "Email",
      "readonly": ""
    },
    model: {
      value: (_vm.objRequest.email),
      callback: function($$v) {
        _vm.$set(_vm.objRequest, "email", $$v)
      },
      expression: "objRequest.email"
    }
  })], 1)], 1), _vm._v(" "), _c('div', {
    staticClass: "col-md-12"
  }, [_c('b-form-group', {
    attrs: {
      "label": "Tên người dùng"
    }
  }, [_c('b-form-input', {
    attrs: {
      "placeholder": "Tên người dùng",
      "readonly": ""
    },
    model: {
      value: (_vm.objRequest.userName),
      callback: function($$v) {
        _vm.$set(_vm.objRequest, "userName", $$v)
      },
      expression: "objRequest.userName"
    }
  })], 1)], 1), _vm._v(" "), _c('div', {
    staticClass: "col-md-12"
  }, [_c('b-form-group', {
    attrs: {
      "label": "Tên đầy đủ"
    }
  }, [_c('b-form-input', {
    attrs: {
      "placeholder": "Tên đầy đủ"
    },
    model: {
      value: (_vm.objRequest.fullName),
      callback: function($$v) {
        _vm.$set(_vm.objRequest, "fullName", $$v)
      },
      expression: "objRequest.fullName"
    }
  })], 1)], 1), _vm._v(" "), _c('div', {
    staticClass: "col-md-12"
  }, [_c('b-form-group', {
    attrs: {
      "label": "Nhóm người dùng"
    }
  }, [_c('b-form-input', {
    attrs: {
      "placeholder": "Nhóm người dùng",
      "readonly": ""
    },
    model: {
      value: (_vm.objRequest.roles),
      callback: function($$v) {
        _vm.$set(_vm.objRequest, "roles", $$v)
      },
      expression: "objRequest.roles"
    }
  })], 1)], 1), _vm._v(" "), _c('div', {
    staticClass: "col-md-12"
  }, [_c('b-form-group', {
    attrs: {
      "label": "Địa chỉ"
    }
  }, [_c('b-form-input', {
    attrs: {
      "placeholder": "Địa chỉ"
    },
    model: {
      value: (_vm.objRequest.address),
      callback: function($$v) {
        _vm.$set(_vm.objRequest, "address", $$v)
      },
      expression: "objRequest.address"
    }
  })], 1)], 1), _vm._v(" "), _c('div', {
    staticClass: "col-md-12"
  }, [_c('div', {
    staticClass: "mt-3"
  }, [_c('div', {
    staticClass: "row"
  }, [_c('div', {
    staticClass: "col-md-4"
  }, [_c('button', {
    staticClass: "btn btn-info btn-submit-form col-md-12 btncus",
    attrs: {
      "type": "button"
    },
    on: {
      "click": function($event) {
        return _vm.DoAddEdit()
      }
    }
  }, [_c('i', {
    staticClass: "fa fa-save"
  }), _vm._v(" Cập nhật\n                                                    ")])]), _vm._v(" "), _c('div', {
    staticClass: "col-md-4"
  }, [_c('button', {
    staticClass: "btn btn-success col-md-12 btncus",
    attrs: {
      "type": "button"
    },
    on: {
      "click": function($event) {
        return _vm.DoRefesh()
      }
    }
  }, [_c('i', {
    staticClass: "fa fa-refresh"
  }), _vm._v(" Làm mới\n                                                    ")])]), _vm._v(" "), _c('div', {
    staticClass: "col-md-4"
  }, [_c('button', {
    staticClass: "btn btn-warning col-md-12 btncus",
    attrs: {
      "type": "button"
    },
    on: {
      "click": function($event) {
        return _vm.GetRouterChangePassword()
      }
    }
  }, [_c('i', {
    staticClass: "fa fa-refresh"
  }), _vm._v(" Thay đổi mật khẩu\n                                                    ")])])])])])])])])], 1)], 1)])])], 1)], 1)
},staticRenderFns: []}
module.exports.render._withStripped = true
if (true) {
  module.hot.accept()
  if (module.hot.data) {
     __webpack_require__(178).rerender("data-v-d423a34a", module.exports)
  }
}

/***/ }),

/***/ 774:
/***/ (function(module, exports, __webpack_require__) {

var Component = __webpack_require__(374)(
  /* script */
  __webpack_require__(1248),
  /* template */
  __webpack_require__(1575),
  /* scopeId */
  null,
  /* cssModules */
  null
)
Component.options.__file = "C:\\PlatformDuLich\\BackEnd\\Web\\Platform\\CMS\\PlatformCMS\\ClientApp\\pages\\profile\\edit.vue"
if (Component.esModule && Object.keys(Component.esModule).some(function (key) {return key !== "default" && key !== "__esModule"})) {console.error("named exports are not supported in *.vue files.")}
if (Component.options.functional) {console.error("[vue-loader] edit.vue: functional components are not supported with templates, they should use render functions.")}

/* hot reload */
if (true) {(function () {
  var hotAPI = __webpack_require__(178)
  hotAPI.install(__webpack_require__(26), false)
  if (!hotAPI.compatible) return
  module.hot.accept()
  if (!module.hot.data) {
    hotAPI.createRecord("data-v-d423a34a", Component.options)
  } else {
    hotAPI.reload("data-v-d423a34a", Component.options)
  }
})()}

module.exports = Component.exports


/***/ }),

/***/ 791:
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(53)();
// imports


// module
exports.push([module.i, ".vld-overlay {\n  bottom: 0;\n  left: 0;\n  position: absolute;\n  right: 0;\n  top: 0;\n  align-items: center;\n  display: none;\n  justify-content: center;\n  overflow: hidden;\n  z-index: 9999;\n}\n\n.vld-overlay.is-active {\n  display: flex;\n}\n\n.vld-overlay.is-full-page {\n  z-index: 9999;\n  position: fixed;\n}\n\n.vld-overlay .vld-background {\n  bottom: 0;\n  left: 0;\n  position: absolute;\n  right: 0;\n  top: 0;\n  background: #fff;\n  opacity: 0.5;\n}\n\n.vld-overlay .vld-icon, .vld-parent {\n  position: relative;\n}\n\n", ""]);

// exports


/***/ }),

/***/ 792:
/***/ (function(module, exports, __webpack_require__) {

// style-loader: Adds some css to the DOM by adding a <style> tag

// load the styles
var content = __webpack_require__(791);
if(typeof content === 'string') content = [[module.i, content, '']];
// add the styles to the DOM
var update = __webpack_require__(179)(content, {});
if(content.locals) module.exports = content.locals;
// Hot Module Replacement
if(true) {
	// When the styles change, update the <style> tags
	if(!content.locals) {
		module.hot.accept(791, function() {
			var newContent = __webpack_require__(791);
			if(typeof newContent === 'string') newContent = [[module.i, newContent, '']];
			update(newContent);
		});
	}
	// When the module is disposed, remove the <style> tags
	module.hot.dispose(function() { update(); });
}

/***/ }),

/***/ 793:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
   value: true
});
var msgNotify = {};
exports.default = msgNotify;

/***/ })

});
//# sourceMappingURL=65.js.map