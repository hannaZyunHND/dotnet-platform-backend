webpackJsonp([68],{

/***/ 1220:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _extends2 = __webpack_require__(8);

var _extends3 = _interopRequireDefault(_extends2);

__webpack_require__(793);

var _constant = __webpack_require__(794);

var _constant2 = _interopRequireDefault(_constant);

var _vuex = __webpack_require__(180);

var _vueLoadingOverlay = __webpack_require__(376);

var _vueLoadingOverlay2 = _interopRequireDefault(_vueLoadingOverlay);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

var fields = [{ key: "LanguageCode", label: "Mã ngôn ngữ" }, { key: "Name", label: "Tên ngôn ngữ", sortable: true }, { key: "SetDefault", label: "Ngôn ngữ mặc định", sortable: true }, { key: "Is", label: "Thao tác" }];

exports.default = {
    name: "language",
    components: {
        Loading: _vueLoadingOverlay2.default
    },
    data: function data() {
        return {
            isLoading: false,
            fields: fields,
            _language: {
                LanguageCode: "",
                Name: "",
                SetDefault: true
            },
            messeger: "",
            currentSort: "LanguageCode",
            currentSortDir: "asc",

            currentPage: 1,
            pageSize: 10,
            loading: true,
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

    methods: (0, _extends3.default)({}, (0, _vuex.mapActions)(["getLanguages", "deleteLanguage"]), {
        onChangePaging: function onChangePaging() {
            this.isLoading = true;
            var initial = this.$route.query.initial;
            initial = typeof initial != "undefined" ? initial.toLowerCase() : "";
            this.getLanguages({
                initial: initial,
                keyword: this.keyword,
                pageIndex: this.currentPage,
                pageSize: this.pageSize,
                sortBy: this.currentSort,
                sortDir: this.currentSortDir
            });
            this.isLoading = false;
        },

        sortor: function sortor(s) {
            if (s === this.currentSort) {
                this.currentSortDir = this.currentSortDir === "asc" ? "desc" : "asc";
            }
            this.currentSort = s;
            this.onChangePaging();
        },
        remove: function remove(item) {
            var _this = this;

            var initial = this.$route.query.initial;
            initial = typeof initial != "undefined" ? initial.toLowerCase() : "";
            this.deleteLanguage(item).then(function (response) {
                if (response.Success == true) {
                    _this.$toast.success(response.Message, {});
                    _this.isLoading = false;
                    _this.onChangePaging();
                } else {
                    _this.$toast.success(response.Message, {});
                    _this.isLoading = false;
                }
            }).catch(function (e) {
                _this.$toast.error(_constant2.default.error + ". Error:" + e, {});
            });
        }
    }),
    computed: (0, _extends3.default)({}, (0, _vuex.mapGetters)(["languages"])),
    mounted: function mounted() {
        this.onChangePaging();
    },

    watch: {
        currentPage: function currentPage(newVal) {
            this.currentPage = newVal;
            this.onChangePaging();
        }
    }
};

/***/ }),

/***/ 1514:
/***/ (function(module, exports, __webpack_require__) {

module.exports={render:function (){var _vm=this;var _h=_vm.$createElement;var _c=_vm._self._c||_h;
  return _c('div', [_c('b-card', {
    staticClass: "card-filter",
    attrs: {
      "header-tag": "header",
      "footer-tag": "footer"
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
  }), _vm._v(" "), _c('div', [_c('b-col', {
    attrs: {
      "md": "12"
    }
  }, [_c('b-row', {
    staticClass: "form-group"
  }, [_c('b-col', {
    attrs: {
      "md": "4"
    }
  }, [_c('b-form-input', {
    attrs: {
      "type": "text",
      "placeholder": "Tìm kiếm theo tên"
    },
    on: {
      "keyup": function($event) {
        if (!$event.type.indexOf('key') && _vm._k($event.keyCode, "enter", 13, $event.key, "Enter")) { return null; }
        return _vm.onChangePaging()
      }
    },
    model: {
      value: (_vm.keyword),
      callback: function($$v) {
        _vm.keyword = $$v
      },
      expression: "keyword"
    }
  })], 1), _vm._v(" "), _c('b-col', {
    attrs: {
      "md": "2"
    }
  }, [_c('b-form-select', {
    attrs: {
      "id": "basicSelect",
      "plain": true,
      "options": ['Chọn danh mục', 'Option 1', 'Option 2', 'Option 3'],
      "value": "Chọn danh mục"
    }
  })], 1), _vm._v(" "), _c('b-col', {
    attrs: {
      "md": "2"
    }
  }, [_c('b-btn', {
    directives: [{
      name: "b-toggle",
      rawName: "v-b-toggle.collapse1",
      modifiers: {
        "collapse1": true
      }
    }],
    attrs: {
      "variant": "primary"
    }
  }, [_c('i', {
    staticClass: "fa fa-angle-double-down",
    attrs: {
      "aria-hidden": "true"
    }
  })])], 1)], 1)], 1), _vm._v(" "), _c('b-collapse', {
    staticClass: "mt-2",
    attrs: {
      "id": "collapse1"
    }
  }, [_c('b-card', [_c('p', {
    staticClass: "card-text"
  }, [_vm._v("Collapse contents Here")]), _vm._v(" "), _c('b-btn', {
    directives: [{
      name: "b-toggle",
      rawName: "v-b-toggle.collapse1_inner",
      modifiers: {
        "collapse1_inner": true
      }
    }],
    attrs: {
      "size": "sm"
    }
  }, [_vm._v("Toggle Inner Collapse")]), _vm._v(" "), _c('b-collapse', {
    staticClass: "mt-2",
    attrs: {
      "id": "collapse1_inner"
    }
  }, [_c('b-card', [_vm._v("Hello!")])], 1)], 1)], 1)], 1)], 1), _vm._v(" "), _c('div', {
    staticClass: "card card-data"
  }, [_c('div', {
    staticClass: "card-body"
  }, [_c('div', {
    staticClass: "mb-2",
    attrs: {
      "role": "toolbar",
      "aria-label": "Toolbar with button groups and dropdown menu"
    }
  }, [_c('div', {
    staticClass: "mx-1 btn-group",
    attrs: {
      "role": "group"
    }
  }, [_c('router-link', {
    staticClass: "btn btn-success",
    attrs: {
      "to": {
        path: 'add'
      }
    }
  }, [_c('i', {
    staticClass: "fa fa-plus"
  }), _vm._v(" Thêm mới\n                    ")]), _vm._v(" "), _vm._m(0)], 1), _vm._v(" "), _c('b-dropdown', {
    staticClass: "mx-1",
    attrs: {
      "variant": "info",
      "right": "",
      "text": "Hành động",
      "icon": ""
    }
  }, [_c('b-dropdown-item', [_vm._v("Kích hoạt")]), _vm._v(" "), _c('b-dropdown-item', [_vm._v("Không kích hoạt")])], 1), _vm._v(" "), _c('div', {
    staticClass: "mx-1 btn-group mi-paging"
  }, [_c('b-pagination', {
    attrs: {
      "total-rows": _vm.languages.Total,
      "per-page": _vm.pageSize,
      "aria-controls": "_language"
    },
    model: {
      value: (_vm.currentPage),
      callback: function($$v) {
        _vm.currentPage = $$v
      },
      expression: "currentPage"
    }
  })], 1)], 1), _vm._v(" "), _c('table', {
    staticClass: "table"
  }, [_c('thead', {
    staticClass: "thead-dark table table-centered table-nowrap"
  }, [_c('tr', _vm._l((_vm.fields), function(field) {
    return _c('th', {
      staticClass: "text-center",
      staticStyle: {
        "max-width": "150px"
      },
      attrs: {
        "scope": "col"
      },
      on: {
        "click": function($event) {
          field.sortable ? _vm.sortor(field.key) : null
        }
      }
    }, [_vm._v(_vm._s(field.label))])
  }), 0)]), _vm._v(" "), _c('tbody', _vm._l((_vm.languages.ListData), function(item) {
    return _c('tr', [_c('td', {
      staticClass: "text-center",
      attrs: {
        "scope": "row"
      }
    }, [_vm._v(_vm._s(item.LanguageCode))]), _vm._v(" "), _c('td', {
      staticClass: "text-center"
    }, [_vm._v(_vm._s(item.Name))]), _vm._v(" "), _c('td', {
      staticClass: "text-center"
    }, [_vm._v(_vm._s(item.SetDefault))]), _vm._v(" "), _c('td', {
      staticClass: "text-center"
    }, [_c('button', {
      staticClass: "btn btn-xs btn-danger",
      on: {
        "click": function($event) {
          return _vm.remove(item)
        }
      }
    }, [_c('i', {
      staticClass: "fa fa-minus-circle"
    })])])])
  }), 0)])])])], 1)
},staticRenderFns: [function (){var _vm=this;var _h=_vm.$createElement;var _c=_vm._self._c||_h;
  return _c('button', {
    staticClass: "btn btn-danger",
    attrs: {
      "type": "button"
    }
  }, [_c('i', {
    staticClass: "fa fa-trash-o"
  }), _vm._v(" Xóa\n                    ")])
}]}
module.exports.render._withStripped = true
if (true) {
  module.hot.accept()
  if (module.hot.data) {
     __webpack_require__(178).rerender("data-v-1029cb4c", module.exports)
  }
}

/***/ }),

/***/ 750:
/***/ (function(module, exports, __webpack_require__) {

var Component = __webpack_require__(374)(
  /* script */
  __webpack_require__(1220),
  /* template */
  __webpack_require__(1514),
  /* scopeId */
  null,
  /* cssModules */
  null
)
Component.options.__file = "D:\\Code\\WORKING\\dotnet-platform-backend\\Web\\Platform\\CMS\\PlatformCMS\\ClientApp\\pages\\language\\list.vue"
if (Component.esModule && Object.keys(Component.esModule).some(function (key) {return key !== "default" && key !== "__esModule"})) {console.error("named exports are not supported in *.vue files.")}
if (Component.options.functional) {console.error("[vue-loader] list.vue: functional components are not supported with templates, they should use render functions.")}

/* hot reload */
if (true) {(function () {
  var hotAPI = __webpack_require__(178)
  hotAPI.install(__webpack_require__(26), false)
  if (!hotAPI.compatible) return
  module.hot.accept()
  if (!module.hot.data) {
    hotAPI.createRecord("data-v-1029cb4c", Component.options)
  } else {
    hotAPI.reload("data-v-1029cb4c", Component.options)
  }
})()}

module.exports = Component.exports


/***/ }),

/***/ 792:
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(53)();
// imports


// module
exports.push([module.i, ".vld-overlay {\n  bottom: 0;\n  left: 0;\n  position: absolute;\n  right: 0;\n  top: 0;\n  align-items: center;\n  display: none;\n  justify-content: center;\n  overflow: hidden;\n  z-index: 9999;\n}\n\n.vld-overlay.is-active {\n  display: flex;\n}\n\n.vld-overlay.is-full-page {\n  z-index: 9999;\n  position: fixed;\n}\n\n.vld-overlay .vld-background {\n  bottom: 0;\n  left: 0;\n  position: absolute;\n  right: 0;\n  top: 0;\n  background: #fff;\n  opacity: 0.5;\n}\n\n.vld-overlay .vld-icon, .vld-parent {\n  position: relative;\n}\n\n", ""]);

// exports


/***/ }),

/***/ 793:
/***/ (function(module, exports, __webpack_require__) {

// style-loader: Adds some css to the DOM by adding a <style> tag

// load the styles
var content = __webpack_require__(792);
if(typeof content === 'string') content = [[module.i, content, '']];
// add the styles to the DOM
var update = __webpack_require__(179)(content, {});
if(content.locals) module.exports = content.locals;
// Hot Module Replacement
if(true) {
	// When the styles change, update the <style> tags
	if(!content.locals) {
		module.hot.accept(792, function() {
			var newContent = __webpack_require__(792);
			if(typeof newContent === 'string') newContent = [[module.i, newContent, '']];
			update(newContent);
		});
	}
	// When the module is disposed, remove the <style> tags
	module.hot.dispose(function() { update(); });
}

/***/ }),

/***/ 794:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
   value: true
});
var msgNotify = {};
exports.default = msgNotify;

/***/ })

});
//# sourceMappingURL=68.js.map