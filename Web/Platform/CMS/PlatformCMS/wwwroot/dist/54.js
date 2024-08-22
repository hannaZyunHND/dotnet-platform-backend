webpackJsonp([54],{

/***/ 1174:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
    value: true
});

var _extends2 = __webpack_require__(8);

var _extends3 = _interopRequireDefault(_extends2);

__webpack_require__(779);

var _constant = __webpack_require__(781);

var _constant2 = _interopRequireDefault(_constant);

var _vuex = __webpack_require__(176);

var _vueLoadingOverlay = __webpack_require__(373);

var _vueLoadingOverlay2 = _interopRequireDefault(_vueLoadingOverlay);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

exports.default = {
    name: "policy",
    components: {
        Loading: _vueLoadingOverlay2.default
    },
    data: function data() {
        return {
            editedItem: null,
            editMode: false,
            isLoading: false,
            _product: {},
            messeger: "",
            currentSort: "Id",
            currentSortDir: "asc",
            SearchKeyword: "",
            SearchLanguageCode: "vi-VN     ",
            SearchLocation: 0,
            currentPage: 1,
            pageSize: 10,
            loading: true,
            item: {},
            Language: [],
            Location: []

        };
    },

    methods: (0, _extends3.default)({}, (0, _vuex.mapActions)(["getBannerAdss", "removeBannerAds"]), {
        onChangePaging: function onChangePaging() {
            this.isLoading = true;
            var initial = this.$route.query.initial;
            initial = typeof initial != "undefined" ? initial.toLowerCase() : "";
            this.getBannerAdss({
                keyword: this.SearchKeyword
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

            if (confirm("Bạn có thực sự muốn xoá?")) {
                var initial = this.$route.query.initial;
                initial = typeof initial != "undefined" ? initial.toLowerCase() : "";
                this.removeBannerAds(item.key).then(function (response) {
                    debugger;
                    if (response.key == true) {
                        _this.$toast.success(response.value, {});
                        _this.onChangePaging();
                        _this.isLoading = false;
                    } else {
                        _this.$toast.error(response.value, {});
                        _this.isLoading = false;
                    }
                }).catch(function (e) {
                    _this.$toast.error(_constant2.default.error + ". Error:" + e, {});
                });
            }
        },
        saveData: function saveData(item) {
            var _this2 = this;

            if (item.id > 0) {
                this.updateDepartment(item).then(function (response) {
                    if (response.success == true) {
                        _this2.$toast.success(response.message, {});
                        _this2.isLoading = false;
                        _this2.editedItem = null;
                    } else {
                        _this2.$toast.error(response.message, {});
                        _this2.isLoading = false;
                    }
                }).catch(function (e) {
                    _this2.$toast.error(_constant2.default.error + ". Error:" + e, {});
                    _this2.isLoading = false;
                });
            }
        },
        editData: function editData(item) {
            this.beforEditCache = item;
            this.editedItem = item;
        }
    }),
    computed: (0, _extends3.default)({}, (0, _vuex.mapGetters)(["banneradss"])),
    mounted: function mounted() {
        this.onChangePaging();
    },

    watch: {
        currentPage: function currentPage(newVal) {
            this.currentPage = newVal;
            this.onChangePaging();
        },
        SearchLanguageCode: function SearchLanguageCode() {
            this.currentPage = 1;
            this.onChangePaging();
        },
        SearchLocation: function SearchLocation() {
            this.currentPage = 1;
            this.onChangePaging();
        }
    }
};

/***/ }),

/***/ 1454:
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(53)();
// imports


// module
exports.push([module.i, ".edit,[v-cloak]{display:none}.editing .edit{display:block}.editing .view{display:none}", ""]);

// exports


/***/ }),

/***/ 1573:
/***/ (function(module, exports) {

module.exports={render:function (){var _vm=this;var _h=_vm.$createElement;var _c=_vm._self._c||_h;
  return _c('div', {
    staticClass: "list-data"
  }, [_c('b-card', {
    staticClass: "card-filter",
    attrs: {
      "header-tag": "header",
      "footer-tag": "footer"
    }
  }, [_c('div', [_c('b-col', {
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
        if (!$event.type.indexOf('key') && $event.keyCode !== 13) { return null; }
        return _vm.onChangePaging()
      }
    },
    model: {
      value: (_vm.SearchKeyword),
      callback: function($$v) {
        _vm.SearchKeyword = $$v
      },
      expression: "SearchKeyword"
    }
  })], 1), _vm._v(" "), _c('b-col', {
    attrs: {
      "md": "1"
    }
  }, [_c('b-btn', {
    staticClass: "col-lg-12",
    attrs: {
      "variant": "info"
    }
  }, [_c('i', {
    staticClass: "fa fa-search",
    attrs: {
      "aria-hidden": "true"
    }
  })])], 1), _vm._v(" "), _c('b-col', {
    attrs: {
      "md": "1"
    }
  }, [_c('b-btn', {
    staticClass: "col-lg-12",
    attrs: {
      "variant": "primary"
    }
  }, [_c('i', {
    staticClass: "fa fa-refresh",
    attrs: {
      "aria-hidden": "true"
    }
  })])], 1), _vm._v(" "), _c('b-col', {
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
  }, [_c('b-card', [_vm._v("Hello!")])], 1)], 1)], 1)], 1)]), _vm._v(" "), _c('div', {
    staticClass: "card card-data"
  }, [_c('div', {
    staticClass: "card-body"
  }, [_c('div', {
    staticClass: " mb-2",
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
  }), _vm._v(" Thêm mới")]), _vm._v(" "), _vm._m(0)], 1), _vm._v(" "), _c('b-dropdown', {
    staticClass: "mx-1",
    attrs: {
      "variant": "info",
      "right": "",
      "text": "Hành động",
      "icon": ""
    }
  }, [_c('b-dropdown-item', [_vm._v("Kích hoạt")]), _vm._v(" "), _c('b-dropdown-item', [_vm._v("Không kích hoạt")])], 1)], 1), _vm._v(" "), _c('div', {
    staticClass: "table-responsive"
  }, [_c('div', {
    staticClass: "dataTables_wrapper dt-bootstrap4 no-footer"
  }, [_c('div', {
    staticClass: "clear"
  }), _vm._v(" "), _c('table', {
    staticClass: "table data-thumb-view dataTable no-footer",
    attrs: {
      "role": "grid"
    }
  }, [_vm._m(1), _vm._v(" "), _c('tbody', _vm._l((_vm.banneradss), function(item, index) {
    return _c('tr', [_vm._m(2, true), _vm._v(" "), _c('td', {
      staticClass: "text-center"
    }, [_vm._v(_vm._s(index))]), _vm._v(" "), _c('td', {
      staticClass: "text-center"
    }, [_vm._v("\n                                    " + _vm._s(item.key) + "\n                                ")]), _vm._v(" "), _c('td', {
      staticClass: "text-center"
    }, [_c('p', [_vm._v("Ngôn ngữ: " + _vm._s(item.value))])]), _vm._v(" "), _c('td', [_c('b-row', [_c('div', {
      staticStyle: {
        "padding": "5px"
      }
    }, [_c('router-link', {
      attrs: {
        "to": {
          path: 'edit/' + item.key
        }
      }
    }, [_c('span', {
      staticClass: "action-edit"
    }, [_c('i', {
      staticClass: "fa fa-edit"
    })])])], 1), _vm._v(" "), _c('div', {
      staticStyle: {
        "padding": "5px"
      }
    }, [_c('span', {
      staticClass: "action-delete"
    }, [_c('a', {
      on: {
        "click": function($event) {
          return _vm.remove(item)
        }
      }
    }, [_c('i', {
      staticClass: "fa fa-trash",
      staticStyle: {
        "color": "red"
      }
    })])])])])], 1)])
  }), 0)])])])])])], 1)
},staticRenderFns: [function (){var _vm=this;var _h=_vm.$createElement;var _c=_vm._self._c||_h;
  return _c('button', {
    staticClass: "btn btn-danger",
    attrs: {
      "type": "button"
    }
  }, [_c('i', {
    staticClass: "fa fa-trash-o"
  }), _vm._v(" Xóa")])
},function (){var _vm=this;var _h=_vm.$createElement;var _c=_vm._self._c||_h;
  return _c('thead', {
    staticClass: "table table-centered table-nowrap"
  }, [_c('tr', {
    attrs: {
      "role": "row"
    }
  }, [_c('th'), _vm._v(" "), _c('th', {
    staticClass: "text-center"
  }, [_vm._v("STT")]), _vm._v(" "), _c('th', {
    staticClass: "text-center"
  }, [_vm._v("Mã chính sách")]), _vm._v(" "), _c('th', {
    staticClass: "text-center"
  }, [_vm._v("Ngôn ngữ")]), _vm._v(" "), _c('th', [_vm._v("Thao tác")])])])
},function (){var _vm=this;var _h=_vm.$createElement;var _c=_vm._self._c||_h;
  return _c('td', {
    staticClass: "dt-checkboxes-cell"
  }, [_c('input', {
    staticClass: "dt-checkboxes",
    attrs: {
      "type": "checkbox"
    }
  })])
}]}

/***/ }),

/***/ 1616:
/***/ (function(module, exports, __webpack_require__) {

// style-loader: Adds some css to the DOM by adding a <style> tag

// load the styles
var content = __webpack_require__(1454);
if(typeof content === 'string') content = [[module.i, content, '']];
if(content.locals) module.exports = content.locals;
// add the styles to the DOM
var update = __webpack_require__(784)("08d9b835", content, true);

/***/ }),

/***/ 745:
/***/ (function(module, exports, __webpack_require__) {


/* styles */
__webpack_require__(1616)

var Component = __webpack_require__(371)(
  /* script */
  __webpack_require__(1174),
  /* template */
  __webpack_require__(1573),
  /* scopeId */
  null,
  /* cssModules */
  null
)

module.exports = Component.exports


/***/ }),

/***/ 779:
/***/ (function(module, exports, __webpack_require__) {

// style-loader: Adds some css to the DOM by adding a <style> tag

// load the styles
var content = __webpack_require__(780);
if(typeof content === 'string') content = [[module.i, content, '']];
// add the styles to the DOM
var update = __webpack_require__(175)(content, {});
if(content.locals) module.exports = content.locals;
// Hot Module Replacement
if(false) {
	// When the styles change, update the <style> tags
	if(!content.locals) {
		module.hot.accept("!!../../css-loader/index.js!./vue-loading.css", function() {
			var newContent = require("!!../../css-loader/index.js!./vue-loading.css");
			if(typeof newContent === 'string') newContent = [[module.id, newContent, '']];
			update(newContent);
		});
	}
	// When the module is disposed, remove the <style> tags
	module.hot.dispose(function() { update(); });
}

/***/ }),

/***/ 780:
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(53)();
// imports


// module
exports.push([module.i, ".vld-overlay {\n  bottom: 0;\n  left: 0;\n  position: absolute;\n  right: 0;\n  top: 0;\n  align-items: center;\n  display: none;\n  justify-content: center;\n  overflow: hidden;\n  z-index: 9999;\n}\n\n.vld-overlay.is-active {\n  display: flex;\n}\n\n.vld-overlay.is-full-page {\n  z-index: 9999;\n  position: fixed;\n}\n\n.vld-overlay .vld-background {\n  bottom: 0;\n  left: 0;\n  position: absolute;\n  right: 0;\n  top: 0;\n  background: #fff;\n  opacity: 0.5;\n}\n\n.vld-overlay .vld-icon, .vld-parent {\n  position: relative;\n}\n\n", ""]);

// exports


/***/ }),

/***/ 781:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
   value: true
});
var msgNotify = {};
exports.default = msgNotify;

/***/ }),

/***/ 784:
/***/ (function(module, exports, __webpack_require__) {

/*
  MIT License http://www.opensource.org/licenses/mit-license.php
  Author Tobias Koppers @sokra
  Modified by Evan You @yyx990803
*/

var hasDocument = typeof document !== 'undefined'

if (typeof DEBUG !== 'undefined' && DEBUG) {
  if (!hasDocument) {
    throw new Error(
    'vue-style-loader cannot be used in a non-browser environment. ' +
    "Use { target: 'node' } in your Webpack config to indicate a server-rendering environment."
  ) }
}

var listToStyles = __webpack_require__(792)

/*
type StyleObject = {
  id: number;
  parts: Array<StyleObjectPart>
}

type StyleObjectPart = {
  css: string;
  media: string;
  sourceMap: ?string
}
*/

var stylesInDom = {/*
  [id: number]: {
    id: number,
    refs: number,
    parts: Array<(obj?: StyleObjectPart) => void>
  }
*/}

var head = hasDocument && (document.head || document.getElementsByTagName('head')[0])
var singletonElement = null
var singletonCounter = 0
var isProduction = false
var noop = function () {}

// Force single-tag solution on IE6-9, which has a hard limit on the # of <style>
// tags it will allow on a page
var isOldIE = typeof navigator !== 'undefined' && /msie [6-9]\b/.test(navigator.userAgent.toLowerCase())

module.exports = function (parentId, list, _isProduction) {
  isProduction = _isProduction

  var styles = listToStyles(parentId, list)
  addStylesToDom(styles)

  return function update (newList) {
    var mayRemove = []
    for (var i = 0; i < styles.length; i++) {
      var item = styles[i]
      var domStyle = stylesInDom[item.id]
      domStyle.refs--
      mayRemove.push(domStyle)
    }
    if (newList) {
      styles = listToStyles(parentId, newList)
      addStylesToDom(styles)
    } else {
      styles = []
    }
    for (var i = 0; i < mayRemove.length; i++) {
      var domStyle = mayRemove[i]
      if (domStyle.refs === 0) {
        for (var j = 0; j < domStyle.parts.length; j++) {
          domStyle.parts[j]()
        }
        delete stylesInDom[domStyle.id]
      }
    }
  }
}

function addStylesToDom (styles /* Array<StyleObject> */) {
  for (var i = 0; i < styles.length; i++) {
    var item = styles[i]
    var domStyle = stylesInDom[item.id]
    if (domStyle) {
      domStyle.refs++
      for (var j = 0; j < domStyle.parts.length; j++) {
        domStyle.parts[j](item.parts[j])
      }
      for (; j < item.parts.length; j++) {
        domStyle.parts.push(addStyle(item.parts[j]))
      }
      if (domStyle.parts.length > item.parts.length) {
        domStyle.parts.length = item.parts.length
      }
    } else {
      var parts = []
      for (var j = 0; j < item.parts.length; j++) {
        parts.push(addStyle(item.parts[j]))
      }
      stylesInDom[item.id] = { id: item.id, refs: 1, parts: parts }
    }
  }
}

function createStyleElement () {
  var styleElement = document.createElement('style')
  styleElement.type = 'text/css'
  head.appendChild(styleElement)
  return styleElement
}

function addStyle (obj /* StyleObjectPart */) {
  var update, remove
  var styleElement = document.querySelector('style[data-vue-ssr-id~="' + obj.id + '"]')

  if (styleElement) {
    if (isProduction) {
      // has SSR styles and in production mode.
      // simply do nothing.
      return noop
    } else {
      // has SSR styles but in dev mode.
      // for some reason Chrome can't handle source map in server-rendered
      // style tags - source maps in <style> only works if the style tag is
      // created and inserted dynamically. So we remove the server rendered
      // styles and inject new ones.
      styleElement.parentNode.removeChild(styleElement)
    }
  }

  if (isOldIE) {
    // use singleton mode for IE9.
    var styleIndex = singletonCounter++
    styleElement = singletonElement || (singletonElement = createStyleElement())
    update = applyToSingletonTag.bind(null, styleElement, styleIndex, false)
    remove = applyToSingletonTag.bind(null, styleElement, styleIndex, true)
  } else {
    // use multi-style-tag mode in all other cases
    styleElement = createStyleElement()
    update = applyToTag.bind(null, styleElement)
    remove = function () {
      styleElement.parentNode.removeChild(styleElement)
    }
  }

  update(obj)

  return function updateStyle (newObj /* StyleObjectPart */) {
    if (newObj) {
      if (newObj.css === obj.css &&
          newObj.media === obj.media &&
          newObj.sourceMap === obj.sourceMap) {
        return
      }
      update(obj = newObj)
    } else {
      remove()
    }
  }
}

var replaceText = (function () {
  var textStore = []

  return function (index, replacement) {
    textStore[index] = replacement
    return textStore.filter(Boolean).join('\n')
  }
})()

function applyToSingletonTag (styleElement, index, remove, obj) {
  var css = remove ? '' : obj.css

  if (styleElement.styleSheet) {
    styleElement.styleSheet.cssText = replaceText(index, css)
  } else {
    var cssNode = document.createTextNode(css)
    var childNodes = styleElement.childNodes
    if (childNodes[index]) styleElement.removeChild(childNodes[index])
    if (childNodes.length) {
      styleElement.insertBefore(cssNode, childNodes[index])
    } else {
      styleElement.appendChild(cssNode)
    }
  }
}

function applyToTag (styleElement, obj) {
  var css = obj.css
  var media = obj.media
  var sourceMap = obj.sourceMap

  if (media) {
    styleElement.setAttribute('media', media)
  }

  if (sourceMap) {
    // https://developer.chrome.com/devtools/docs/javascript-debugging
    // this makes source maps inside style tags work properly in Chrome
    css += '\n/*# sourceURL=' + sourceMap.sources[0] + ' */'
    // http://stackoverflow.com/a/26603875
    css += '\n/*# sourceMappingURL=data:application/json;base64,' + btoa(unescape(encodeURIComponent(JSON.stringify(sourceMap)))) + ' */'
  }

  if (styleElement.styleSheet) {
    styleElement.styleSheet.cssText = css
  } else {
    while (styleElement.firstChild) {
      styleElement.removeChild(styleElement.firstChild)
    }
    styleElement.appendChild(document.createTextNode(css))
  }
}


/***/ }),

/***/ 792:
/***/ (function(module, exports) {

/**
 * Translates the list format produced by css-loader into something
 * easier to manipulate.
 */
module.exports = function listToStyles (parentId, list) {
  var styles = []
  var newStyles = {}
  for (var i = 0; i < list.length; i++) {
    var item = list[i]
    var id = item[0]
    var css = item[1]
    var media = item[2]
    var sourceMap = item[3]
    var part = {
      id: parentId + ':' + i,
      css: css,
      media: media,
      sourceMap: sourceMap
    }
    if (!newStyles[id]) {
      styles.push(newStyles[id] = { id: id, parts: [part] })
    } else {
      newStyles[id].parts.push(part)
    }
  }
  return styles
}


/***/ })

});
//# sourceMappingURL=54.js.map