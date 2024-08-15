webpackJsonp([72],{

/***/ 1204:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
  value: true
});
exports.default = {
  name: 'Page404'
};

/***/ }),

/***/ 1524:
/***/ (function(module, exports, __webpack_require__) {

module.exports={render:function (){var _vm=this;var _h=_vm.$createElement;var _c=_vm._self._c||_h;
  return _c('div', {
    staticClass: "app flex-row align-items-center"
  }, [_c('div', {
    staticClass: "container"
  }, [_c('b-row', {
    staticClass: "justify-content-center"
  }, [_c('b-col', {
    attrs: {
      "md": "6"
    }
  }, [_c('div', {
    staticClass: "clearfix"
  }, [_c('h1', {
    staticClass: "float-left display-3 mr-4"
  }, [_vm._v("404")]), _vm._v(" "), _c('h4', {
    staticClass: "pt-3"
  }, [_vm._v("Oops! You're lost.")]), _vm._v(" "), _c('p', {
    staticClass: "text-muted"
  }, [_vm._v("The page you are looking for was not found.")])]), _vm._v(" "), _c('b-input-group', [_c('b-input-group-prepend', [_c('b-input-group-text', [_c('i', {
    staticClass: "fa fa-search"
  })])], 1), _vm._v(" "), _c('input', {
    staticClass: "form-control",
    attrs: {
      "id": "prependedInput",
      "size": "16",
      "type": "text",
      "placeholder": "What are you looking for?"
    }
  }), _vm._v(" "), _c('b-input-group-append', [_c('b-button', {
    attrs: {
      "variant": "info"
    }
  }, [_vm._v("Search")])], 1)], 1)], 1)], 1)], 1)])
},staticRenderFns: []}
module.exports.render._withStripped = true
if (true) {
  module.hot.accept()
  if (module.hot.data) {
     __webpack_require__(178).rerender("data-v-16c5994e", module.exports)
  }
}

/***/ }),

/***/ 732:
/***/ (function(module, exports, __webpack_require__) {

var Component = __webpack_require__(374)(
  /* script */
  __webpack_require__(1204),
  /* template */
  __webpack_require__(1524),
  /* scopeId */
  null,
  /* cssModules */
  null
)
Component.options.__file = "D:\\Code\\WORKING\\dotnet-platform-backend\\Web\\Platform\\CMS\\PlatformCMS\\ClientApp\\pages\\Page404.vue"
if (Component.esModule && Object.keys(Component.esModule).some(function (key) {return key !== "default" && key !== "__esModule"})) {console.error("named exports are not supported in *.vue files.")}
if (Component.options.functional) {console.error("[vue-loader] Page404.vue: functional components are not supported with templates, they should use render functions.")}

/* hot reload */
if (true) {(function () {
  var hotAPI = __webpack_require__(178)
  hotAPI.install(__webpack_require__(26), false)
  if (!hotAPI.compatible) return
  module.hot.accept()
  if (!module.hot.data) {
    hotAPI.createRecord("data-v-16c5994e", Component.options)
  } else {
    hotAPI.reload("data-v-16c5994e", Component.options)
  }
})()}

module.exports = Component.exports


/***/ })

});
//# sourceMappingURL=72.js.map