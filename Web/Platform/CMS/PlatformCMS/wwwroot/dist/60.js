webpackJsonp([60],{

/***/ 1093:
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(53)();
// imports


// module
exports.push([module.i, "\n.fade-enter-active[data-v-1e2c7174],\r\n.fade-leave-active[data-v-1e2c7174] {\r\n  transition: opacity 0.5s;\n}\n.fade-enter[data-v-1e2c7174],\r\n.fade-leave-to[data-v-1e2c7174] {\r\n  opacity: 0;\n}\r\n", "", {"version":3,"sources":["C:/WORKING/Joytime/dotnet-platform-backend/Web/Platform/CMS/PlatformCMS/ClientApp/pages/Forms.vue?fadd4e10"],"names":[],"mappings":";AA6hCA;;EAEA,yBAAA;CACA;AACA;;EAEA,WAAA;CACA","file":"Forms.vue","sourcesContent":["<template>\r\n  <div class=\"animated fadeIn\">\r\n    <b-row>\r\n      <b-col sm=\"6\">\r\n        <b-card>\r\n          <div slot=\"header\">\r\n            <strong>Credit Card </strong> <small>Form</small>\r\n          </div>\r\n          <b-row>\r\n            <b-col sm=\"12\">\r\n              <b-form-group>\r\n                <label for=\"name\">Name</label>\r\n                <b-form-input type=\"text\" id=\"name\" placeholder=\"Enter your name\"></b-form-input>\r\n              </b-form-group>\r\n            </b-col>\r\n          </b-row>\r\n          <b-row>\r\n            <b-col sm=\"12\">\r\n              <b-form-group>\r\n                <label for=\"ccnumber\">Credit Card Number</label>\r\n                <b-form-input type=\"text\" id=\"ccnumber\" placeholder=\"0000 0000 0000 0000\"></b-form-input>\r\n              </b-form-group>\r\n            </b-col>\r\n          </b-row>\r\n          <b-row>\r\n            <b-col sm=\"4\">\r\n              <b-form-group >\r\n                <label for=\"month1\">Month</label>\r\n                <b-form-select id=\"month1\"\r\n                  :plain=\"true\"\r\n                  :options=\"[1,2,3,4,5,6,7,8,9,10,11,12]\">\r\n                </b-form-select>\r\n              </b-form-group>\r\n            </b-col>\r\n            <b-col sm=\"4\">\r\n              <b-form-group>\r\n                <label for=\"year1\">Year</label>\r\n                <b-form-select id=\"year1\"\r\n                  :plain=\"true\"\r\n                  :options=\"[2014,2015,2016,2017,2018,2019,2020,2021,2022,2023,2024,2025]\">\r\n                </b-form-select>\r\n              </b-form-group>\r\n            </b-col>\r\n            <b-col sm=\"4\">\r\n              <b-form-group>\r\n                <label for=\"cvv\">CVV/CVC</label>\r\n                <b-form-input type=\"text\" id=\"cvv\" placeholder=\"123\"></b-form-input>\r\n              </b-form-group>\r\n            </b-col>\r\n          </b-row>\r\n        </b-card>\r\n      </b-col>\r\n      <b-col sm=\"6\">\r\n        <b-card>\r\n          <div slot=\"header\">\r\n            <strong>Company </strong><small>Form</small>\r\n          </div>\r\n          <b-form-group>\r\n            <label for=\"company\">Company</label>\r\n            <b-form-input type=\"text\" id=\"company\" placeholder=\"Enter your company name\"></b-form-input>\r\n          </b-form-group>\r\n          <b-form-group>\r\n            <label for=\"vat\">VAT</label>\r\n            <b-form-input type=\"text\" id=\"vat\" placeholder=\"PL1234567890\"></b-form-input>\r\n          </b-form-group>\r\n          <b-form-group>\r\n            <label for=\"street\">Street</label>\r\n            <b-form-input type=\"text\" id=\"street\" placeholder=\"Enter street name\"></b-form-input>\r\n          </b-form-group>\r\n          <b-row>\r\n            <b-col sm=\"8\">\r\n              <b-form-group>\r\n                <label for=\"city\">City</label>\r\n                <b-form-input type=\"text\" id=\"city\" placeholder=\"Enter your city\"></b-form-input>\r\n              </b-form-group>\r\n            </b-col>\r\n            <b-col sm=\"4\">\r\n              <b-form-group>\r\n                <label for=\"postal-code\">Postal Code</label>\r\n                <b-form-input type=\"text\" id=\"postal-code\" placeholder=\"Postal Code\"></b-form-input>\r\n              </b-form-group>\r\n            </b-col>\r\n          </b-row>\r\n          <b-form-group>\r\n            <label for=\"country\">Country</label>\r\n            <b-form-input type=\"text\" id=\"country\" placeholder=\"Country name\"></b-form-input>\r\n          </b-form-group>\r\n        </b-card>\r\n      </b-col>\r\n    </b-row>\r\n    <b-row>\r\n      <b-col md=\"6\">\r\n        <b-card>\r\n          <div slot=\"header\">\r\n            <strong>Basic Form</strong> Elements\r\n          </div>\r\n          <b-form>\r\n          <b-form-group\r\n            description=\"Let us know your full name.\"\r\n            label=\"Enter your name\"\r\n            label-for=\"basicName\"\r\n            :label-cols=\"3\"\r\n            >\r\n            <b-form-input id=\"basicName\" type=\"text\" autocomplete=\"name\"></b-form-input>\r\n          </b-form-group>\r\n          <b-form-group\r\n            label=\"Static\"\r\n            label-for=\"basicStatic\"\r\n            :label-cols=\"3\"\r\n            >\r\n            <b-form-input plaintext id=\"basicStatic\" type=\"text\" value=\"Username\"></b-form-input>\r\n          </b-form-group>\r\n          <b-form-group\r\n            description=\"This is a help text\"\r\n            label=\"Text Input\"\r\n            label-for=\"basicText\"\r\n            :label-cols=\"3\"\r\n            >\r\n            <b-form-input id=\"basicText\" type=\"text\" placeholder=\"Text\"></b-form-input>\r\n          </b-form-group>\r\n          <b-form-group\r\n            label=\"Date\" label-for=\"date\"\r\n            :label-cols=\"3\"\r\n            >\r\n            <b-form-input type=\"date\" id=\"date\"></b-form-input>\r\n          </b-form-group>\r\n          <b-form-group\r\n            description=\"Please enter your email\"\r\n            label=\"Email Input\"\r\n            label-for=\"basicEmail\"\r\n            :label-cols=\"3\"\r\n            >\r\n            <b-form-input id=\"basicEmail\" type=\"email\" placeholder=\"Enter your email\" autocomplete=\"email\"></b-form-input>\r\n          </b-form-group>\r\n          <b-form-group\r\n            description=\"Please enter a complex password\"\r\n            label=\"Password Input\"\r\n            label-for=\"basicPassword\"\r\n            :label-cols=\"3\"\r\n            >\r\n            <b-form-input id=\"basicPassword\" type=\"password\" placeholder=\"Enter your password\" autocomplete=\"current-password\"></b-form-input>\r\n          </b-form-group>\r\n          <b-form-group\r\n            label=\"Disabled Input\"\r\n            label-for=\"basicInputDisabled\"\r\n            :label-cols=\"3\"\r\n            >\r\n            <b-form-input id=\"basicInputDisabled\" type=\"text\" :disabled=\"true\" placeholder=\"Disabled\"></b-form-input>\r\n          </b-form-group>\r\n          <b-form-group\r\n            label=\"Textarea\"\r\n            label-for=\"basicTextarea\"\r\n            :label-cols=\"3\"\r\n            >\r\n            <b-form-textarea id=\"basicTextarea\" :rows=\"9\" placeholder=\"Content..\"></b-form-textarea>\r\n          </b-form-group>\r\n          <b-form-group\r\n            label=\"Select\"\r\n            label-for=\"basicSelect\"\r\n            :label-cols=\"3\"\r\n            >\r\n            <b-form-select id=\"basicSelect\"\r\n              :plain=\"true\"\r\n              :options=\"['Please select','Option 1', 'Option 2', 'Option 3']\"\r\n              value=\"Please select\">\r\n            </b-form-select>\r\n          </b-form-group>\r\n          <b-form-group\r\n            label=\"Select large\"\r\n            label-for=\"basicSelectLg\"\r\n            :label-cols=\"3\"\r\n            >\r\n            <b-form-select id=\"basicSelectLg\"\r\n              size=\"lg\"\r\n              :plain=\"true\"\r\n              :options=\"['Please select','Option 1', 'Option 2', 'Option 3']\"\r\n              value=\"Please select\">\r\n            </b-form-select>\r\n          </b-form-group>\r\n          <b-form-group\r\n            label=\"Select small\"\r\n            label-for=\"basicSelectSm\"\r\n            :label-cols=\"3\"\r\n            >\r\n            <b-form-select id=\"basicSelectSm\"\r\n              size=\"sm\"\r\n              :plain=\"true\"\r\n              :options=\"['Please select','Option 1', 'Option 2', 'Option 3']\"\r\n              value=\"Please select\">\r\n            </b-form-select>\r\n          </b-form-group>\r\n          <b-form-group\r\n            label=\"Disabled select\"\r\n            label-for=\"basicSelectDisabled\"\r\n            :label-cols=\"3\"\r\n            >\r\n            <b-form-select id=\"basicSelectDisabled\"\r\n              :plain=\"true\"\r\n              :options=\"['Please select','Option 1', 'Option 2', 'Option 3']\"\r\n              :disabled=\"true\"\r\n              value=\"Please select\">\r\n            </b-form-select>\r\n          </b-form-group>\r\n          <b-form-group\r\n            label=\"Select\"\r\n            label-for=\"basicMultiSelect\"\r\n            :label-cols=\"3\"\r\n            >\r\n            <b-form-select id=\"basicMultiSelect\"\r\n              :plain=\"true\"\r\n              :multiple=\"true\"\r\n              :options=\"[\r\n                {\r\n                  text: 'Please select some item',\r\n                  value: null\r\n                },\r\n                {\r\n                  text: 'This is First option',\r\n                  value: 'a'\r\n                }, {\r\n                  text: 'Default Selected Option',\r\n                  value: 'b'\r\n                }, {\r\n                  text: 'This is another option',\r\n                  value: 'c'\r\n                }, {\r\n                  text: 'This one is disabled',\r\n                  value: 'd',\r\n                  disabled: true\r\n                }]\"\r\n              :value=\"[null,'c']\">\r\n            </b-form-select>\r\n          </b-form-group>\r\n          <b-form-group\r\n            label=\"Radios\"\r\n            label-for=\"basicRadios\"\r\n            :label-cols=\"3\"\r\n            >\r\n            <b-form-radio-group id=\"basicRadios\"\r\n              :plain=\"true\"\r\n              :options=\"[\r\n                {text: 'Option 1 ',value: '1'},\r\n                {text: 'Option 2 ',value: '2'},\r\n                {text: 'Option 3 ',value: '3'}\r\n              ]\"\r\n              checked=\"2\"\r\n              stacked>\r\n            </b-form-radio-group>\r\n          </b-form-group>\r\n          <b-form-group\r\n            label=\"Inline radios\"\r\n            label-for=\"basicInlineRadios\"\r\n            :label-cols=\"3\"\r\n            >\r\n            <b-form-radio-group id=\"basicInlineRadios\"\r\n              :plain=\"true\"\r\n              :options=\"[\r\n                {text: 'Option 1 ',value: '1'},\r\n                {text: 'Option 2 ',value: '2'},\r\n                {text: 'Option 3 ',value: '3'}\r\n              ]\"\r\n              :checked=\"3\">\r\n            </b-form-radio-group>\r\n          </b-form-group>\r\n\r\n          <b-form-group\r\n            label=\"Checkboxes\"\r\n            label-for=\"basicCheckboxes\"\r\n            :label-cols=\"3\"\r\n            >\r\n            <b-form-checkbox-group stacked id=\"basicCheckboxes\" name=\"Checkboxes\" :plain=\"true\" :checked=\"[2,3]\">\r\n              <b-form-checkbox value=\"1\">Option 1</b-form-checkbox>\r\n              <b-form-checkbox value=\"2\">Option 2</b-form-checkbox>\r\n              <b-form-checkbox value=\"3\">Option 3</b-form-checkbox>\r\n            </b-form-checkbox-group>\r\n          </b-form-group>\r\n          <b-form-group\r\n            label=\"Inline checkboxes\"\r\n            label-for=\"basicInlineCheckboxes\"\r\n            :label-cols=\"3\"\r\n            >\r\n            <b-form-checkbox-group id=\"basicInlineCheckboxes\" name=\"InlineCheckboxes\" :plain=\"true\" :checked=\"[1,3]\">\r\n              <b-form-checkbox :plain=\"true\" value=\"1\">Option 1</b-form-checkbox>\r\n              <b-form-checkbox :plain=\"true\" value=\"2\">Option 2</b-form-checkbox>\r\n              <b-form-checkbox :plain=\"true\" value=\"3\">Option 3</b-form-checkbox>\r\n            </b-form-checkbox-group>\r\n          </b-form-group>\r\n          <!--custom controls - radios/checkboxes - temporary fix-->\r\n          <b-form-group\r\n            label=\"Radios - custom\"\r\n            label-for=\"basicRadiosCustom\"\r\n            :label-cols=\"3\"\r\n            >\r\n            <b-form-radio-group\r\n              id=\"basicRadiosCustom\"\r\n              value=\"1\"\r\n              stacked>\r\n              <div class=\"custom-control custom-radio\">\r\n                <input type=\"radio\" id=\"customRadio1\" name=\"customRadio\" class=\"custom-control-input\" value=\"1\">\r\n                <label class=\"custom-control-label\" for=\"customRadio1\">Option 1</label>\r\n              </div>\r\n              <div class=\"custom-control custom-radio\">\r\n                <input type=\"radio\" id=\"customRadio2\" name=\"customRadio\" class=\"custom-control-input\" value=\"2\" checked>\r\n                <label class=\"custom-control-label\" for=\"customRadio2\">Option 2</label>\r\n              </div>\r\n              <div class=\"custom-control custom-radio\">\r\n                <input type=\"radio\" id=\"customRadio3\" name=\"customRadio\" class=\"custom-control-input\" value=\"3\">\r\n                <label class=\"custom-control-label\" for=\"customRadio3\">Option 3</label>\r\n              </div>\r\n            </b-form-radio-group>\r\n          </b-form-group>\r\n          <b-form-group\r\n            label=\"Inline radios - custom\"\r\n            label-for=\"basicCustomRadios1\"\r\n            :label-cols=\"3\"\r\n            >\r\n            <b-form-radio-group\r\n              id=\"basicCustomRadios1\"\r\n              name=\"customRadioInline1\">\r\n              <div class=\"custom-control custom-radio custom-control-inline\">\r\n                <input type=\"radio\" id=\"customRadioInline1\" name=\"customRadioInline1\" class=\"custom-control-input\" value=\"1\">\r\n                <label class=\"custom-control-label\" for=\"customRadioInline1\">Option 1</label>\r\n              </div>\r\n              <div class=\"custom-control custom-radio custom-control-inline\">\r\n                <input type=\"radio\" id=\"customRadioInline2\" name=\"customRadioInline1\" class=\"custom-control-input\" value=\"2\" checked>\r\n                <label class=\"custom-control-label\" for=\"customRadioInline2\">Option 2</label>\r\n              </div>\r\n              <div class=\"custom-control custom-radio custom-control-inline\">\r\n                <input type=\"radio\" id=\"customRadioInline3\" name=\"customRadioInline1\" class=\"custom-control-input\" value=\"3\">\r\n                <label class=\"custom-control-label\" for=\"customRadioInline3\">Option 3</label>\r\n              </div>\r\n            </b-form-radio-group>\r\n          </b-form-group>\r\n          <b-form-group\r\n            label=\"Checkboxes - custom\"\r\n            label-for=\"basicCustomCheckboxes\"\r\n            :label-cols=\"3\"\r\n            >\r\n            <b-form-checkbox-group stacked id=\"basicCustomCheckboxes\">\r\n              <div class=\"custom-control custom-checkbox\">\r\n                <input type=\"checkbox\" class=\"custom-control-input\" id=\"customChk1\" value=\"1\" checked>\r\n                <label class=\"custom-control-label\" for=\"customChk1\">Option 1</label>\r\n              </div>\r\n              <div class=\"custom-control custom-checkbox\">\r\n                <input type=\"checkbox\" class=\"custom-control-input\" id=\"customChk2\" value=\"2\">\r\n                <label class=\"custom-control-label\" for=\"customChk2\">Option 2</label>\r\n              </div>\r\n              <div class=\"custom-control custom-checkbox\">\r\n                <input type=\"checkbox\" class=\"custom-control-input\" id=\"customChk3\" value=\"3\">\r\n                <label class=\"custom-control-label\" for=\"customChk3\">Option 3</label>\r\n              </div>\r\n            </b-form-checkbox-group>\r\n          </b-form-group>\r\n          <b-form-group\r\n            label=\"Inline checkboxes - custom\"\r\n            label-for=\"basicInlineCustomCheckboxes\"\r\n            :label-cols=\"3\"\r\n            >\r\n            <b-form-checkbox-group id=\"basicInlineCustomCheckboxes\">\r\n              <div class=\"custom-control custom-checkbox custom-control-inline\">\r\n                <input type=\"checkbox\" class=\"custom-control-input\" id=\"customInChk1\" value=\"1\">\r\n                <label class=\"custom-control-label\" for=\"customInChk1\">Option 1</label>\r\n              </div>\r\n              <div class=\"custom-control custom-checkbox custom-control-inline\">\r\n                <input type=\"checkbox\" class=\"custom-control-input\" id=\"customInChk2\" value=\"2\" checked>\r\n                <label class=\"custom-control-label\" for=\"customInChk2\">Option 2</label>\r\n              </div>\r\n              <div class=\"custom-control custom-checkbox custom-control-inline\">\r\n                <input type=\"checkbox\" class=\"custom-control-input\" id=\"customInChk3\" value=\"3\">\r\n                <label class=\"custom-control-label\" for=\"customInChk3\">Option 3</label>\r\n              </div>\r\n            </b-form-checkbox-group>\r\n          </b-form-group>\r\n          <b-form-group\r\n            label=\"File input\"\r\n            label-for=\"fileInput\"\r\n            :label-cols=\"3\"\r\n            >\r\n              <b-form-file id=\"fileInput\" :plain=\"true\"></b-form-file>\r\n          </b-form-group>\r\n          <b-form-group\r\n            label=\"Multiple file input\"\r\n            label-for=\"fileInputMulti\"\r\n            :label-cols=\"3\"\r\n            >\r\n              <b-form-file id=\"fileInputMulti\" :plain=\"true\" :multiple=\"true\"></b-form-file>\r\n          </b-form-group>\r\n          <div slot=\"footer\">\r\n            <b-button type=\"submit\" size=\"sm\" variant=\"primary\"><i class=\"fa fa-dot-circle-o\"></i> Submit</b-button>\r\n            <b-button type=\"reset\" size=\"sm\" variant=\"danger\"><i class=\"fa fa-ban\"></i> Reset</b-button>\r\n          </div>\r\n          </b-form>\r\n        </b-card>\r\n        <b-card>\r\n          <div slot=\"header\">\r\n            <strong>Inline</strong> Form\r\n          </div>\r\n          <!-- Bootstrap Vue has some problems with Inline forms that's why we use some standard bootstrap classes -->\r\n          <b-form inline>\r\n            <label class=\"mr-sm-2\" for=\"inlineInput1\">Name: </label>\r\n            <b-input id=\"inlineInput1\" type=\"text\" placeholder=\"Jane Doe\"></b-input>\r\n            <label class=\"mx-sm-2\" for=\"inlineInput2\">Email: </label>\r\n            <b-input id=\"inlineInput2\" type=\"email\" placeholder=\"jane.doe@example.com\" autocomplete=\"email\"></b-input>\r\n          </b-form>\r\n          <div slot=\"footer\">\r\n            <b-button type=\"submit\" size=\"sm\" variant=\"primary\"><i class=\"fa fa-dot-circle-o\"></i> Submit</b-button>\r\n            <b-button type=\"reset\" size=\"sm\" variant=\"danger\"><i class=\"fa fa-ban\"></i> Reset</b-button>\r\n          </div>\r\n        </b-card>\r\n      </b-col>\r\n      <b-col md=\"6\">\r\n        <b-card>\r\n          <div slot=\"header\">\r\n            <strong>Horizontal</strong> Form\r\n          </div>\r\n          <b-form>\r\n            <b-form-group\r\n              label=\"Email\"\r\n              label-for=\"horizEmail\"\r\n              description=\"Please enter your email.\"\r\n              :label-cols=\"3\"\r\n              >\r\n              <b-form-input id=\"horizEmail\" type=\"email\" placeholder=\"Enter Email..\" autocomplete=\"username email\"></b-form-input>\r\n            </b-form-group>\r\n            <b-form-group\r\n              label=\"Password\"\r\n              label-for=\"horizPass\"\r\n              description=\"Please enter your password.\"\r\n              :label-cols=\"3\"\r\n              >\r\n              <b-form-input id=\"horizPass\" type=\"password\" placeholder=\"Enter Password..\" autocomplete=\"current-password\"></b-form-input>\r\n            </b-form-group>\r\n            <div slot=\"footer\">\r\n              <b-button type=\"submit\" size=\"sm\" variant=\"primary\"><i class=\"fa fa-dot-circle-o\"></i> Submit</b-button>\r\n              <b-button type=\"reset\" size=\"sm\" variant=\"danger\"><i class=\"fa fa-ban\"></i> Reset</b-button>\r\n            </div>\r\n          </b-form>\r\n        </b-card>\r\n        <b-card>\r\n          <div slot=\"header\">\r\n            <strong>Normal</strong> Form\r\n          </div>\r\n          <b-form>\r\n            <b-form-group validated\r\n              label=\"Email\"\r\n              label-for=\"normalEmail\"\r\n              description=\"Please enter your email.\">\r\n              <b-form-input id=\"normalEmail\" type=\"email\" placeholder=\"Enter Email..\" required autocomplete=\"email\"></b-form-input>\r\n            </b-form-group>\r\n            <b-form-group validated\r\n              label=\"Password\"\r\n              label-for=\"normalPass\"\r\n              description=\"Please enter your password.\">\r\n              <b-form-input id=\"normalPass\" type=\"password\" placeholder=\"Enter Password..\" required autocomplete=\"current-password\"></b-form-input>\r\n            </b-form-group>\r\n            <div slot=\"footer\">\r\n              <b-button type=\"submit\" size=\"sm\" variant=\"primary\"><i class=\"fa fa-dot-circle-o\"></i> Submit</b-button>\r\n              <b-button type=\"reset\" size=\"sm\" variant=\"danger\"><i class=\"fa fa-ban\"></i> Reset</b-button>\r\n            </div>\r\n          </b-form>\r\n        </b-card>\r\n        <b-card no-body :no-block=\"true\">\r\n          <div slot=\"header\">\r\n            Input <strong>Grid</strong>\r\n          </div>\r\n          <b-card-body>\r\n            <b-row class=\"form-group\">\r\n              <b-col sm=\"3\">\r\n                <b-form-input type=\"text\" placeholder=\".col-sm-3\"></b-form-input>\r\n              </b-col>\r\n            </b-row>\r\n            <b-row class=\"form-group\">\r\n              <b-col sm=\"4\">\r\n                <b-form-input type=\"text\" placeholder=\".col-sm-4\"></b-form-input>\r\n              </b-col>\r\n            </b-row>\r\n            <b-row class=\"form-group\">\r\n              <b-col sm=\"5\">\r\n                <b-form-input type=\"text\" placeholder=\".col-sm-5\"></b-form-input>\r\n              </b-col>\r\n            </b-row>\r\n            <b-row class=\"form-group\">\r\n              <b-col sm=\"6\">\r\n                <b-form-input type=\"text\" placeholder=\".col-sm-6\"></b-form-input>\r\n              </b-col>\r\n            </b-row>\r\n            <b-row class=\"form-group\">\r\n              <b-col sm=\"7\">\r\n                <b-form-input type=\"text\" placeholder=\".col-sm-7\"></b-form-input>\r\n              </b-col>\r\n            </b-row>\r\n            <b-row class=\"form-group\">\r\n              <b-col sm=\"8\">\r\n                <b-form-input type=\"text\" placeholder=\".col-sm-8\"></b-form-input>\r\n              </b-col>\r\n            </b-row>\r\n            <b-row class=\"form-group\">\r\n              <b-col sm=\"9\">\r\n                <b-form-input type=\"text\" placeholder=\".col-sm-9\"></b-form-input>\r\n              </b-col>\r\n            </b-row>\r\n            <b-row class=\"form-group\">\r\n              <b-col sm=\"10\">\r\n                <b-form-input type=\"text\" placeholder=\".col-sm-10\"></b-form-input>\r\n              </b-col>\r\n            </b-row>\r\n            <b-row class=\"form-group\">\r\n              <b-col sm=\"11\">\r\n                <b-form-input type=\"text\" placeholder=\".col-sm-11\"></b-form-input>\r\n              </b-col>\r\n            </b-row>\r\n            <b-row class=\"form-group\">\r\n              <b-col sm=\"12\">\r\n                <b-form-input type=\"text\" placeholder=\".col-sm-12\"></b-form-input>\r\n              </b-col>\r\n            </b-row>\r\n          </b-card-body>\r\n          <div slot=\"footer\">\r\n            <b-button type=\"submit\" size=\"sm\" variant=\"primary\"><i class=\"fa fa-user\"></i> Login</b-button>\r\n            <b-button type=\"reset\" size=\"sm\" variant=\"danger\"><i class=\"fa fa-ban\"></i> Reset</b-button>\r\n          </div>\r\n        </b-card>\r\n        <b-card>\r\n          <div slot=\"header\">\r\n            Input <strong>Sizes</strong>\r\n          </div>\r\n          <b-form-group\r\n            label=\"Small input\"\r\n            label-for=\"smInput\"\r\n            label-size=\"sm\"\r\n            :label-cols=\"5\"\r\n            >\r\n            <b-form-input id=\"smInput\" type=\"text\" size=\"sm\" placeholder=\"size='sm'\"></b-form-input>\r\n          </b-form-group>\r\n          <b-form-group\r\n            label=\"Default input\"\r\n            label-for=\"defaultInput\"\r\n            :label-cols=\"5\"\r\n            >\r\n            <b-form-input  id=\"defaultInput\" type=\"text\" placeholder=\"normal\"></b-form-input>\r\n          </b-form-group>\r\n          <b-form-group\r\n            label=\"Large input\"\r\n            label-for=\"lgInput\"\r\n            label-size=\"lg\"\r\n            :label-cols=\"5\"\r\n            >\r\n            <b-form-input id=\"lgInput\" type=\"text\" size=\"lg\" placeholder=\"size='lg'\"></b-form-input>\r\n          </b-form-group>\r\n          <div slot=\"footer\">\r\n            <b-button type=\"submit\" size=\"sm\" variant=\"primary\"><i class=\"fa fa-dot-circle-o\"></i> Submit</b-button>\r\n            <b-button type=\"reset\" size=\"sm\" variant=\"danger\"><i class=\"fa fa-ban\"></i> Reset</b-button>\r\n          </div>\r\n        </b-card>\r\n      </b-col>\r\n    </b-row>\r\n    <b-row>\r\n      <b-col sm=\"12\" md=\"6\">\r\n        <b-card no-body :no-block=\"true\">\r\n          <div slot=\"header\">\r\n            <strong>Validation feedback</strong> Form\r\n          </div>\r\n          <b-card-body>\r\n            <b-form>\r\n              <b-form-group validated>\r\n                <label class=\"col-form-label\" for=\"inputIsValid\">Input is valid</label>\r\n                <input type=\"text\" class=\"form-control is-valid\" id=\"inputIsValid\">\r\n                <b-form-valid-feedback>\r\n                  Input is valid.\r\n                </b-form-valid-feedback>\r\n              </b-form-group>\r\n              <b-form-group>\r\n                <label class=\"col-form-label\" for=\"inputIsInvalid\">Input is invalid</label>\r\n                <input type=\"text\" class=\"form-control is-invalid\" id=\"inputIsInvalid\">\r\n                <b-form-invalid-feedback>\r\n                  Please provide a valid information.\r\n                </b-form-invalid-feedback>\r\n              </b-form-group>\r\n            </b-form>\r\n          </b-card-body>\r\n        </b-card>\r\n      </b-col>\r\n      <b-col sm=\"12\" md=\"6\">\r\n        <b-card no-body :no-block=\"true\">\r\n          <div slot=\"header\">\r\n            <strong>Validation feedback</strong> Form\r\n          </div>\r\n          <b-card-body>\r\n            <b-form validated novalidate>\r\n              <b-form-group label-for=\"inputSuccess2\" label=\"Non-required input\">\r\n                <b-form-input type=\"text\" class=\"form-control-success\" id=\"inputSuccess2\"></b-form-input>\r\n                <b-form-valid-feedback>\r\n                  Input is not required.\r\n                </b-form-valid-feedback>\r\n              </b-form-group>\r\n              <b-form-group label-for=\"inputError2\" label=\"Required input\">\r\n                <b-form-input type=\"text\" class=\"form-control-warning\" id=\"inputError2\" required></b-form-input>\r\n                <b-form-valid-feedback>\r\n                  Input provided.\r\n                </b-form-valid-feedback>\r\n                <b-form-invalid-feedback>\r\n                  Please provide a required input.\r\n                </b-form-invalid-feedback>\r\n              </b-form-group>\r\n            </b-form>\r\n          </b-card-body>\r\n        </b-card>\r\n      </b-col>\r\n    </b-row>\r\n    <b-row>\r\n      <b-col md=\"4\">\r\n        <b-card>\r\n          <div slot=\"header\">\r\n            <strong>Icon/Text</strong> Groups\r\n          </div>\r\n          <b-form-group>\r\n            <b-input-group>\r\n              <b-input-group-prepend>\r\n                <b-input-group-text><i class=\"fa fa-user\"></i></b-input-group-text>\r\n              </b-input-group-prepend>\r\n              <b-form-input type=\"text\" placeholder=\"Username\"></b-form-input>\r\n            </b-input-group>\r\n          </b-form-group>\r\n          <b-form-group>\r\n            <b-input-group>\r\n              <b-form-input type=\"email\" placeholder=\"Email\" autocomplete=\"email\"></b-form-input>\r\n              <b-input-group-append><b-input-group-text><i class=\"fa fa-envelope-o\"></i></b-input-group-text></b-input-group-append>\r\n            </b-input-group>\r\n          </b-form-group>\r\n          <b-form-group>\r\n            <b-input-group>\r\n              <b-input-group-prepend>\r\n                <b-input-group-text><i class=\"fa fa-euro\"></i></b-input-group-text>\r\n              </b-input-group-prepend>\r\n              <b-form-input type=\"text\" placeholder=\"ex. $1.000.000\"></b-form-input>\r\n              <b-input-group-append><b-input-group-text>.00</b-input-group-text></b-input-group-append>\r\n            </b-input-group>\r\n          </b-form-group>\r\n          <div slot=\"footer\">\r\n            <b-button type=\"submit\" size=\"sm\" variant=\"success\"><i class=\"fa fa-dot-circle-o\"></i> Submit</b-button>\r\n            <b-button type=\"reset\" size=\"sm\" variant=\"danger\"><i class=\"fa fa-ban\"></i> Reset</b-button>\r\n          </div>\r\n        </b-card>\r\n      </b-col>\r\n      <b-col md=\"4\">\r\n        <b-card>\r\n          <div slot=\"header\">\r\n            <strong>Buttons</strong> Groups\r\n          </div>\r\n          <b-form-group>\r\n            <b-input-group>\r\n              <!-- Attach Left button -->\r\n              <b-input-group-prepend>\r\n                <b-button variant=\"primary\">\r\n                  <i class=\"fa fa-search\"></i> Search\r\n                </b-button>\r\n              </b-input-group-prepend>\r\n              <b-form-input type=\"text\" placeholder=\"Username\"></b-form-input>\r\n            </b-input-group>\r\n          </b-form-group>\r\n          <b-form-group>\r\n            <b-input-group>\r\n              <b-form-input type=\"email\" placeholder=\"Email\" autocomplete=\"email\"></b-form-input>\r\n              <!-- Attach Right button -->\r\n              <b-input-group-append>\r\n                <b-button variant=\"primary\">Submit</b-button>\r\n              </b-input-group-append>\r\n            </b-input-group>\r\n          </b-form-group>\r\n          <b-form-group>\r\n            <b-input-group>\r\n              <!-- Attach Left button -->\r\n              <b-input-group-prepend>\r\n                <b-button variant=\"primary\"><i class=\"fa fa-facebook\"></i></b-button>\r\n              </b-input-group-prepend>\r\n              <b-form-input type=\"email\" placeholder=\"Email\" autocomplete=\"email\"></b-form-input>\r\n              <!-- Attach Left button -->\r\n              <b-input-group-append>\r\n                <b-button variant=\"primary\"><i class=\"fa fa-twitter\"></i></b-button>\r\n              </b-input-group-append>\r\n            </b-input-group>\r\n          </b-form-group>\r\n          <div slot=\"footer\">\r\n            <b-button type=\"submit\" size=\"sm\" variant=\"success\"><i class=\"fa fa-dot-circle-o\"></i> Submit</b-button>\r\n            <b-button type=\"reset\" size=\"sm\" variant=\"danger\"><i class=\"fa fa-ban\"></i> Reset</b-button>\r\n          </div>\r\n        </b-card>\r\n      </b-col>\r\n      <b-col md=\"4\" >\r\n        <b-card>\r\n          <div slot=\"header\">\r\n            <strong>Dropdowns</strong> Groups\r\n          </div>\r\n          <b-form-group>\r\n              <b-input-group>\r\n                <!-- Attach Left button -->\r\n                <b-input-group-prepend>\r\n                  <b-dropdown text=\"Action\" variant=\"primary\">\r\n                    <b-dropdown-item>Action</b-dropdown-item>\r\n                    <b-dropdown-item>Another action</b-dropdown-item>\r\n                    <b-dropdown-item>Something else here...</b-dropdown-item>\r\n                    <b-dropdown-item disabled>Disabled action</b-dropdown-item>\r\n                  </b-dropdown>\r\n                </b-input-group-prepend>\r\n                <b-form-input placeholder=\"Username\"></b-form-input>\r\n              </b-input-group>\r\n          </b-form-group>\r\n          <b-form-group>\r\n              <b-input-group>\r\n                <b-form-input placeholder=\"Email\"></b-form-input>\r\n                <!-- Attach Right button -->\r\n                <b-input-group-append>\r\n                  <b-dropdown text=\"Action\" variant=\"primary\" right>\r\n                    <b-dropdown-item>Action</b-dropdown-item>\r\n                    <b-dropdown-item>Another action</b-dropdown-item>\r\n                    <b-dropdown-item>Something else here...</b-dropdown-item>\r\n                    <b-dropdown-item disabled>Disabled action</b-dropdown-item>\r\n                  </b-dropdown>\r\n                </b-input-group-append>\r\n              </b-input-group>\r\n          </b-form-group>\r\n          <b-form-group>\r\n              <b-input-group>\r\n                <!-- Attach Left button -->\r\n                <b-input-group-prepend>\r\n                  <b-dropdown text=\"Split\" variant=\"primary\" split>\r\n                    <b-dropdown-item href=\"#\">Action</b-dropdown-item>\r\n                    <b-dropdown-item href=\"#\">Another action</b-dropdown-item>\r\n                    <b-dropdown-item href=\"#\">Something else here...</b-dropdown-item>\r\n                    <b-dropdown-item disabled>Disabled action</b-dropdown-item>\r\n                  </b-dropdown>\r\n                </b-input-group-prepend>\r\n                <b-form-input placeholder=\"...\"></b-form-input>\r\n                <!-- Attach Right button -->\r\n                <b-input-group-append>\r\n                  <b-dropdown text=\"Action\" variant=\"primary\" right>\r\n                    <b-dropdown-item>Action</b-dropdown-item>\r\n                    <b-dropdown-item>Another action</b-dropdown-item>\r\n                    <b-dropdown-item>Something else here...</b-dropdown-item>\r\n                    <b-dropdown-item disabled>Disabled action</b-dropdown-item>\r\n                  </b-dropdown>\r\n                </b-input-group-append>\r\n              </b-input-group>\r\n          </b-form-group>\r\n          <div slot=\"footer\">\r\n            <b-button type=\"submit\" size=\"sm\" variant=\"success\"><i class=\"fa fa-dot-circle-o\"></i> Submit</b-button>\r\n            <b-button type=\"reset\" size=\"sm\" variant=\"danger\"><i class=\"fa fa-ban\"></i> Reset</b-button>\r\n          </div>\r\n        </b-card>\r\n      </b-col>\r\n    </b-row>\r\n    <b-row>\r\n      <b-col md=\"6\">\r\n        <b-card>\r\n          <div slot=\"header\">\r\n            Use the grid for big devices! <small><code>.col-lg-*</code> <code>.col-md-*</code> <code>.col-sm-*</code></small>\r\n          </div>\r\n          <b-row class=\"form-group\">\r\n            <b-col md=\"8\">\r\n              <b-form-input type=\"text\" placeholder=\".col-md-8\"></b-form-input>\r\n            </b-col>\r\n            <b-col md=\"4\">\r\n              <b-form-input type=\"text\" placeholder=\".col-md-4\"></b-form-input>\r\n            </b-col>\r\n          </b-row>\r\n          <b-row class=\"form-group\">\r\n            <b-col md=\"7\">\r\n              <b-form-input type=\"text\" placeholder=\".col-md-7\"></b-form-input>\r\n            </b-col>\r\n            <b-col md=\"5\">\r\n              <b-form-input type=\"text\" placeholder=\".col-md-5\"></b-form-input>\r\n            </b-col>\r\n          </b-row>\r\n          <b-row class=\"form-group\">\r\n            <b-col md=\"6\">\r\n              <b-form-input type=\"text\" placeholder=\".col-md-6\"></b-form-input>\r\n            </b-col>\r\n            <b-col md=\"6\">\r\n              <b-form-input type=\"text\" placeholder=\".col-md-6\"></b-form-input>\r\n            </b-col>\r\n          </b-row>\r\n          <b-row class=\"form-group\">\r\n            <b-col md=\"5\">\r\n              <b-form-input type=\"text\" placeholder=\".col-md-5\"></b-form-input>\r\n            </b-col>\r\n            <b-col md=\"7\">\r\n              <b-form-input type=\"text\" placeholder=\".col-md-7\"></b-form-input>\r\n            </b-col>\r\n          </b-row>\r\n          <b-row class=\"form-group\">\r\n            <b-col md=\"4\">\r\n              <b-form-input type=\"text\" placeholder=\".col-md-4\"></b-form-input>\r\n            </b-col>\r\n            <b-col md=\"8\">\r\n              <b-form-input type=\"text\" placeholder=\".col-md-8\"></b-form-input>\r\n            </b-col>\r\n          </b-row>\r\n          <div slot=\"footer\">\r\n            <b-button type=\"submit\" size=\"sm\" variant=\"primary\">Action</b-button>\r\n            <b-button type=\"button\" size=\"sm\" variant=\"danger\">Action</b-button>\r\n            <b-button type=\"button\" class=\"btn btn-sm btn-warning\">Action</b-button>\r\n            <b-button type=\"button\" class=\"btn btn-sm btn-info\">Action</b-button>\r\n            <b-button type=\"button\" size=\"sm\" variant=\"success\">Action</b-button>\r\n          </div>\r\n        </b-card>\r\n      </b-col>\r\n      <b-col md=\"6\">\r\n        <b-card>\r\n          <div slot=\"header\">\r\n            Input Grid for small devices! <small> <code>.col-*</code></small>\r\n          </div>\r\n          <b-row class=\"form-group\">\r\n            <b-col cols=\"4\">\r\n              <b-form-input type=\"text\" placeholder=\".col-4\"></b-form-input>\r\n            </b-col>\r\n            <b-col cols=\"8\">\r\n              <b-form-input type=\"text\" placeholder=\".col-8\"></b-form-input>\r\n            </b-col>\r\n          </b-row>\r\n          <b-row class=\"form-group\">\r\n            <b-col cols=\"5\">\r\n              <b-form-input type=\"text\" placeholder=\".col-5\"></b-form-input>\r\n            </b-col>\r\n            <b-col cols=\"7\">\r\n              <b-form-input type=\"text\" placeholder=\".col-7\"></b-form-input>\r\n            </b-col>\r\n          </b-row>\r\n          <b-row class=\"form-group\">\r\n            <b-col cols=\"6\">\r\n              <b-form-input type=\"text\" placeholder=\".col-6\"></b-form-input>\r\n            </b-col>\r\n            <b-col cols=\"6\">\r\n              <b-form-input type=\"text\" placeholder=\".col-6\"></b-form-input>\r\n            </b-col>\r\n          </b-row>\r\n          <b-row class=\"form-group\">\r\n            <b-col cols=\"7\">\r\n              <b-form-input type=\"text\" placeholder=\".col-5\"></b-form-input>\r\n            </b-col>\r\n            <b-col cols=\"5\">\r\n              <b-form-input type=\"text\" placeholder=\".col-5\"></b-form-input>\r\n            </b-col>\r\n          </b-row>\r\n          <b-row class=\"form-group\">\r\n            <b-col cols=\"8\">\r\n              <b-form-input type=\"text\" placeholder=\".col-8\"></b-form-input>\r\n            </b-col>\r\n            <b-col cols=\"4\">\r\n              <b-form-input type=\"text\" placeholder=\".col-4\"></b-form-input>\r\n            </b-col>\r\n          </b-row>\r\n          <div slot=\"footer\">\r\n            <b-button type=\"submit\" size=\"sm\" variant=\"primary\">Action</b-button>\r\n            <b-button type=\"button\" size=\"sm\" variant=\"danger\">Action</b-button>\r\n            <b-button type=\"button\" class=\"btn btn-sm btn-warning\">Action</b-button>\r\n            <b-button type=\"button\" class=\"btn btn-sm btn-info\">Action</b-button>\r\n            <b-button type=\"button\" size=\"sm\" variant=\"success\">Action</b-button>\r\n          </div>\r\n        </b-card>\r\n      </b-col>\r\n    </b-row>\r\n    <b-row>\r\n      <b-col md=\"4\">\r\n        <b-card>\r\n          <div slot=\"header\">\r\n            Example Form\r\n          </div>\r\n          <b-form>\r\n            <b-form-group>\r\n              <b-input-group>\r\n                <b-input-group-prepend><b-input-group-text>Username</b-input-group-text></b-input-group-prepend>\r\n                <b-form-input type=\"text\"></b-form-input>\r\n                <b-input-group-append><b-input-group-text><i class=\"fa fa-user\"></i></b-input-group-text></b-input-group-append>\r\n              </b-input-group>\r\n            </b-form-group>\r\n            <b-form-group>\r\n              <b-input-group>\r\n                <b-input-group-prepend><b-input-group-text>Email</b-input-group-text></b-input-group-prepend>\r\n                <b-form-input type=\"email\" autocomplete=\"email\"></b-form-input>\r\n                <b-input-group-append><b-input-group-text><i class=\"fa fa-envelope\"></i></b-input-group-text></b-input-group-append>\r\n              </b-input-group>\r\n            </b-form-group>\r\n            <b-form-group>\r\n              <b-input-group>\r\n                <b-input-group-prepend><b-input-group-text>Password</b-input-group-text></b-input-group-prepend>\r\n                <b-form-input type=\"password\" autocomplete=\"current-password\"></b-form-input>\r\n                <b-input-group-append><b-input-group-text><i class=\"fa fa-asterisk\"></i></b-input-group-text></b-input-group-append>\r\n              </b-input-group>\r\n            </b-form-group>\r\n            <div class=\"form-group form-actions\">\r\n              <b-button type=\"submit\" size=\"sm\" variant=\"primary\">Submit</b-button>\r\n            </div>\r\n          </b-form>\r\n        </b-card>\r\n      </b-col>\r\n      <b-col md=\"4\">\r\n        <b-card>\r\n          <div slot=\"header\">\r\n            Example Form\r\n          </div>\r\n          <b-form>\r\n            <b-form-group>\r\n              <b-input-group>\r\n                <b-form-input type=\"text\" placeholder=\"Username\"></b-form-input>\r\n                <b-input-group-append><b-input-group-text><i class=\"fa fa-user\"></i></b-input-group-text></b-input-group-append>\r\n              </b-input-group>\r\n            </b-form-group>\r\n            <b-form-group>\r\n              <b-input-group>\r\n                <b-form-input type=\"email\" placeholder=\"Email\" autocomplete=\"email\"></b-form-input>\r\n                <b-input-group-append><b-input-group-text><i class=\"fa fa-envelope\"></i></b-input-group-text></b-input-group-append>\r\n              </b-input-group>\r\n            </b-form-group>\r\n            <b-form-group>\r\n              <b-input-group>\r\n                <b-form-input type=\"password\" placeholder=\"Password\" autocomplete=\"current-password\"></b-form-input>\r\n                <b-input-group-append><b-input-group-text><i class=\"fa fa-asterisk\"></i></b-input-group-text></b-input-group-append>\r\n              </b-input-group>\r\n            </b-form-group>\r\n            <div class=\"form-group form-actions\">\r\n              <b-button type=\"submit\" class=\"btn btn-sm btn-secondary\">Submit</b-button>\r\n            </div>\r\n          </b-form>\r\n        </b-card>\r\n      </b-col>\r\n      <b-col md=\"4\">\r\n        <b-card>\r\n          <div slot=\"header\">\r\n            Example Form\r\n          </div>\r\n          <b-form>\r\n            <b-form-group>\r\n              <b-input-group>\r\n                <b-input-group-prepend>\r\n                  <b-input-group-text><i class=\"fa fa-user\"></i></b-input-group-text>\r\n                </b-input-group-prepend>\r\n                <b-form-input type=\"text\" placeholder=\"Username\"></b-form-input>\r\n              </b-input-group>\r\n            </b-form-group>\r\n            <b-form-group>\r\n              <b-input-group>\r\n                <b-input-group-prepend>\r\n                  <b-input-group-text><i class=\"fa fa-envelope\"></i></b-input-group-text>\r\n                </b-input-group-prepend>\r\n                <b-form-input type=\"email\" placeholder=\"Email\" autocomplete=\"email\"></b-form-input>\r\n              </b-input-group>\r\n            </b-form-group>\r\n            <b-form-group>\r\n              <b-input-group>\r\n                <b-input-group-prepend>\r\n                  <b-input-group-text><i class=\"fa fa-asterisk\"></i></b-input-group-text>\r\n                </b-input-group-prepend>\r\n                <b-form-input type=\"password\" placeholder=\"Password\" autocomplete=\"current-password\"></b-form-input>\r\n              </b-input-group>\r\n            </b-form-group>\r\n            <div class=\"form-group form-actions\">\r\n              <b-button type=\"submit\" size=\"sm\" variant=\"success\">Submit</b-button>\r\n            </div>\r\n          </b-form>\r\n        </b-card>\r\n      </b-col>\r\n    </b-row>\r\n    <b-row>\r\n      <b-col lg=\"12\">\r\n        <transition name=\"fade\">\r\n          <b-card no-body v-if=\"show\">\r\n            <div slot=\"header\">\r\n              <i class=\"fa fa-edit\"></i> Form Elements\r\n              <div class=\"card-header-actions\">\r\n                <b-link href=\"#\" class=\"card-header-action btn-setting\">\r\n                  <i class=\"icon-settings\"></i>\r\n                </b-link>\r\n                <b-link class=\"card-header-action btn-minimize\" v-b-toggle.collapse1>\r\n                  <i class=\"icon-arrow-up\"></i>\r\n                </b-link>\r\n                <b-link href=\"#\" class=\"card-header-action btn-close\" v-on:click=\"show = !show\">\r\n                  <i class=\"icon-close\"></i>\r\n                </b-link>\r\n              </div>\r\n            </div>\r\n            <b-collapse id=\"collapse1\" visible>\r\n              <b-card-body>\r\n                <b-form-group label=\"Prepended text\" label-for=\"elementsEmail\" description=\"Here's some help text\">\r\n                  <b-input-group>\r\n                    <b-input-group-prepend>\r\n                      <b-input-group-text>@</b-input-group-text>\r\n                    </b-input-group-prepend>\r\n                    <b-form-input id=\"elementsEmail\" type=\"email\" autocomplete=\"email\"></b-form-input>\r\n                  </b-input-group>\r\n                </b-form-group>\r\n                <b-form-group label=\"Appended text\" label-for=\"elementsAppend\" description=\"Here's some help text\">\r\n                  <b-input-group>\r\n                    <b-form-input id=\"elementsAppend\" type=\"text\"></b-form-input>\r\n                    <b-input-group-append><b-input-group-text>.00</b-input-group-text></b-input-group-append>\r\n                  </b-input-group>\r\n                </b-form-group>\r\n                <b-form-group label=\"Append and prepend\" label-for=\"elementsPrependAppend\" description=\"Here's some help text\">\r\n                  <b-input-group>\r\n                    <b-input-group-prepend>\r\n                      <b-input-group-text>$</b-input-group-text>\r\n                    </b-input-group-prepend>\r\n                    <b-form-input id=\"elementsPrependAppend\" type=\"text\"></b-form-input>\r\n                    <b-input-group-append><b-input-group-text>.00</b-input-group-text></b-input-group-append>\r\n                  </b-input-group>\r\n                </b-form-group>\r\n                <b-form-group label=\"Append with button\" label-for=\"elementsAppendButton\" description=\"Here's some help text\">\r\n                  <b-input-group>\r\n                    <b-form-input id=\"elementsAppendButton\" type=\"text\"></b-form-input>\r\n                    <b-input-group-append>\r\n                      <b-button variant=\"primary\">Go!</b-button>\r\n                    </b-input-group-append>\r\n                  </b-input-group>\r\n                </b-form-group>\r\n                <b-form-group label=\"Two-buttons append\" label-for=\"elementsTwoButtons\">\r\n                  <b-input-group>\r\n                    <b-form-input id=\"elementsTwoButtons\" type=\"text\"></b-form-input>\r\n                    <b-input-group-append>\r\n                      <b-button variant=\"primary\">Search</b-button>\r\n                      <b-button variant=\"danger\">Options</b-button>\r\n                    </b-input-group-append>\r\n                  </b-input-group>\r\n                </b-form-group>\r\n                <div class=\"form-actions\">\r\n                  <b-button type=\"submit\" variant=\"primary\">Save changes</b-button>\r\n                  <b-button type=\"button\" variant=\"secondary\">Cancel</b-button>\r\n                </div>\r\n              </b-card-body>\r\n            </b-collapse>\r\n          </b-card>\r\n        </transition>\r\n      </b-col>\r\n    </b-row>\r\n  </div>\r\n</template>\r\n\r\n<script>\r\nexport default {\r\n  name: 'forms',\r\n  data () {\r\n    return {\r\n      selected: [], // Must be an array reference!\r\n      show: true\r\n    }\r\n  },\r\n  methods: {\r\n    click () {\r\n      // do nothing\r\n    }\r\n  }\r\n}\r\n</script>\r\n\r\n<style scoped>\r\n.fade-enter-active,\r\n.fade-leave-active {\r\n  transition: opacity 0.5s;\r\n}\r\n.fade-enter,\r\n.fade-leave-to {\r\n  opacity: 0;\r\n}\r\n</style>\r\n"],"sourceRoot":""}]);

// exports


/***/ }),

/***/ 1195:
/***/ (function(module, exports, __webpack_require__) {

"use strict";


Object.defineProperty(exports, "__esModule", {
  value: true
});
exports.default = {
  name: 'forms',
  data: function data() {
    return {
      selected: [],
      show: true
    };
  },

  methods: {
    click: function click() {}
  }
};

/***/ }),

/***/ 1525:
/***/ (function(module, exports, __webpack_require__) {

module.exports={render:function (){var _vm=this;var _h=_vm.$createElement;var _c=_vm._self._c||_h;
  return _c('div', {
    staticClass: "animated fadeIn"
  }, [_c('b-row', [_c('b-col', {
    attrs: {
      "sm": "6"
    }
  }, [_c('b-card', [_c('div', {
    attrs: {
      "slot": "header"
    },
    slot: "header"
  }, [_c('strong', [_vm._v("Credit Card ")]), _vm._v(" "), _c('small', [_vm._v("Form")])]), _vm._v(" "), _c('b-row', [_c('b-col', {
    attrs: {
      "sm": "12"
    }
  }, [_c('b-form-group', [_c('label', {
    attrs: {
      "for": "name"
    }
  }, [_vm._v("Name")]), _vm._v(" "), _c('b-form-input', {
    attrs: {
      "type": "text",
      "id": "name",
      "placeholder": "Enter your name"
    }
  })], 1)], 1)], 1), _vm._v(" "), _c('b-row', [_c('b-col', {
    attrs: {
      "sm": "12"
    }
  }, [_c('b-form-group', [_c('label', {
    attrs: {
      "for": "ccnumber"
    }
  }, [_vm._v("Credit Card Number")]), _vm._v(" "), _c('b-form-input', {
    attrs: {
      "type": "text",
      "id": "ccnumber",
      "placeholder": "0000 0000 0000 0000"
    }
  })], 1)], 1)], 1), _vm._v(" "), _c('b-row', [_c('b-col', {
    attrs: {
      "sm": "4"
    }
  }, [_c('b-form-group', [_c('label', {
    attrs: {
      "for": "month1"
    }
  }, [_vm._v("Month")]), _vm._v(" "), _c('b-form-select', {
    attrs: {
      "id": "month1",
      "plain": true,
      "options": [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12]
    }
  })], 1)], 1), _vm._v(" "), _c('b-col', {
    attrs: {
      "sm": "4"
    }
  }, [_c('b-form-group', [_c('label', {
    attrs: {
      "for": "year1"
    }
  }, [_vm._v("Year")]), _vm._v(" "), _c('b-form-select', {
    attrs: {
      "id": "year1",
      "plain": true,
      "options": [2014, 2015, 2016, 2017, 2018, 2019, 2020, 2021, 2022, 2023, 2024, 2025]
    }
  })], 1)], 1), _vm._v(" "), _c('b-col', {
    attrs: {
      "sm": "4"
    }
  }, [_c('b-form-group', [_c('label', {
    attrs: {
      "for": "cvv"
    }
  }, [_vm._v("CVV/CVC")]), _vm._v(" "), _c('b-form-input', {
    attrs: {
      "type": "text",
      "id": "cvv",
      "placeholder": "123"
    }
  })], 1)], 1)], 1)], 1)], 1), _vm._v(" "), _c('b-col', {
    attrs: {
      "sm": "6"
    }
  }, [_c('b-card', [_c('div', {
    attrs: {
      "slot": "header"
    },
    slot: "header"
  }, [_c('strong', [_vm._v("Company ")]), _c('small', [_vm._v("Form")])]), _vm._v(" "), _c('b-form-group', [_c('label', {
    attrs: {
      "for": "company"
    }
  }, [_vm._v("Company")]), _vm._v(" "), _c('b-form-input', {
    attrs: {
      "type": "text",
      "id": "company",
      "placeholder": "Enter your company name"
    }
  })], 1), _vm._v(" "), _c('b-form-group', [_c('label', {
    attrs: {
      "for": "vat"
    }
  }, [_vm._v("VAT")]), _vm._v(" "), _c('b-form-input', {
    attrs: {
      "type": "text",
      "id": "vat",
      "placeholder": "PL1234567890"
    }
  })], 1), _vm._v(" "), _c('b-form-group', [_c('label', {
    attrs: {
      "for": "street"
    }
  }, [_vm._v("Street")]), _vm._v(" "), _c('b-form-input', {
    attrs: {
      "type": "text",
      "id": "street",
      "placeholder": "Enter street name"
    }
  })], 1), _vm._v(" "), _c('b-row', [_c('b-col', {
    attrs: {
      "sm": "8"
    }
  }, [_c('b-form-group', [_c('label', {
    attrs: {
      "for": "city"
    }
  }, [_vm._v("City")]), _vm._v(" "), _c('b-form-input', {
    attrs: {
      "type": "text",
      "id": "city",
      "placeholder": "Enter your city"
    }
  })], 1)], 1), _vm._v(" "), _c('b-col', {
    attrs: {
      "sm": "4"
    }
  }, [_c('b-form-group', [_c('label', {
    attrs: {
      "for": "postal-code"
    }
  }, [_vm._v("Postal Code")]), _vm._v(" "), _c('b-form-input', {
    attrs: {
      "type": "text",
      "id": "postal-code",
      "placeholder": "Postal Code"
    }
  })], 1)], 1)], 1), _vm._v(" "), _c('b-form-group', [_c('label', {
    attrs: {
      "for": "country"
    }
  }, [_vm._v("Country")]), _vm._v(" "), _c('b-form-input', {
    attrs: {
      "type": "text",
      "id": "country",
      "placeholder": "Country name"
    }
  })], 1)], 1)], 1)], 1), _vm._v(" "), _c('b-row', [_c('b-col', {
    attrs: {
      "md": "6"
    }
  }, [_c('b-card', [_c('div', {
    attrs: {
      "slot": "header"
    },
    slot: "header"
  }, [_c('strong', [_vm._v("Basic Form")]), _vm._v(" Elements\n        ")]), _vm._v(" "), _c('b-form', [_c('b-form-group', {
    attrs: {
      "description": "Let us know your full name.",
      "label": "Enter your name",
      "label-for": "basicName",
      "label-cols": 3
    }
  }, [_c('b-form-input', {
    attrs: {
      "id": "basicName",
      "type": "text",
      "autocomplete": "name"
    }
  })], 1), _vm._v(" "), _c('b-form-group', {
    attrs: {
      "label": "Static",
      "label-for": "basicStatic",
      "label-cols": 3
    }
  }, [_c('b-form-input', {
    attrs: {
      "plaintext": "",
      "id": "basicStatic",
      "type": "text",
      "value": "Username"
    }
  })], 1), _vm._v(" "), _c('b-form-group', {
    attrs: {
      "description": "This is a help text",
      "label": "Text Input",
      "label-for": "basicText",
      "label-cols": 3
    }
  }, [_c('b-form-input', {
    attrs: {
      "id": "basicText",
      "type": "text",
      "placeholder": "Text"
    }
  })], 1), _vm._v(" "), _c('b-form-group', {
    attrs: {
      "label": "Date",
      "label-for": "date",
      "label-cols": 3
    }
  }, [_c('b-form-input', {
    attrs: {
      "type": "date",
      "id": "date"
    }
  })], 1), _vm._v(" "), _c('b-form-group', {
    attrs: {
      "description": "Please enter your email",
      "label": "Email Input",
      "label-for": "basicEmail",
      "label-cols": 3
    }
  }, [_c('b-form-input', {
    attrs: {
      "id": "basicEmail",
      "type": "email",
      "placeholder": "Enter your email",
      "autocomplete": "email"
    }
  })], 1), _vm._v(" "), _c('b-form-group', {
    attrs: {
      "description": "Please enter a complex password",
      "label": "Password Input",
      "label-for": "basicPassword",
      "label-cols": 3
    }
  }, [_c('b-form-input', {
    attrs: {
      "id": "basicPassword",
      "type": "password",
      "placeholder": "Enter your password",
      "autocomplete": "current-password"
    }
  })], 1), _vm._v(" "), _c('b-form-group', {
    attrs: {
      "label": "Disabled Input",
      "label-for": "basicInputDisabled",
      "label-cols": 3
    }
  }, [_c('b-form-input', {
    attrs: {
      "id": "basicInputDisabled",
      "type": "text",
      "disabled": true,
      "placeholder": "Disabled"
    }
  })], 1), _vm._v(" "), _c('b-form-group', {
    attrs: {
      "label": "Textarea",
      "label-for": "basicTextarea",
      "label-cols": 3
    }
  }, [_c('b-form-textarea', {
    attrs: {
      "id": "basicTextarea",
      "rows": 9,
      "placeholder": "Content.."
    }
  })], 1), _vm._v(" "), _c('b-form-group', {
    attrs: {
      "label": "Select",
      "label-for": "basicSelect",
      "label-cols": 3
    }
  }, [_c('b-form-select', {
    attrs: {
      "id": "basicSelect",
      "plain": true,
      "options": ['Please select', 'Option 1', 'Option 2', 'Option 3'],
      "value": "Please select"
    }
  })], 1), _vm._v(" "), _c('b-form-group', {
    attrs: {
      "label": "Select large",
      "label-for": "basicSelectLg",
      "label-cols": 3
    }
  }, [_c('b-form-select', {
    attrs: {
      "id": "basicSelectLg",
      "size": "lg",
      "plain": true,
      "options": ['Please select', 'Option 1', 'Option 2', 'Option 3'],
      "value": "Please select"
    }
  })], 1), _vm._v(" "), _c('b-form-group', {
    attrs: {
      "label": "Select small",
      "label-for": "basicSelectSm",
      "label-cols": 3
    }
  }, [_c('b-form-select', {
    attrs: {
      "id": "basicSelectSm",
      "size": "sm",
      "plain": true,
      "options": ['Please select', 'Option 1', 'Option 2', 'Option 3'],
      "value": "Please select"
    }
  })], 1), _vm._v(" "), _c('b-form-group', {
    attrs: {
      "label": "Disabled select",
      "label-for": "basicSelectDisabled",
      "label-cols": 3
    }
  }, [_c('b-form-select', {
    attrs: {
      "id": "basicSelectDisabled",
      "plain": true,
      "options": ['Please select', 'Option 1', 'Option 2', 'Option 3'],
      "disabled": true,
      "value": "Please select"
    }
  })], 1), _vm._v(" "), _c('b-form-group', {
    attrs: {
      "label": "Select",
      "label-for": "basicMultiSelect",
      "label-cols": 3
    }
  }, [_c('b-form-select', {
    attrs: {
      "id": "basicMultiSelect",
      "plain": true,
      "multiple": true,
      "options": [{
          text: 'Please select some item',
          value: null
        },
        {
          text: 'This is First option',
          value: 'a'
        }, {
          text: 'Default Selected Option',
          value: 'b'
        }, {
          text: 'This is another option',
          value: 'c'
        }, {
          text: 'This one is disabled',
          value: 'd',
          disabled: true
        }
      ],
      "value": [null, 'c']
    }
  })], 1), _vm._v(" "), _c('b-form-group', {
    attrs: {
      "label": "Radios",
      "label-for": "basicRadios",
      "label-cols": 3
    }
  }, [_c('b-form-radio-group', {
    attrs: {
      "id": "basicRadios",
      "plain": true,
      "options": [{
          text: 'Option 1 ',
          value: '1'
        },
        {
          text: 'Option 2 ',
          value: '2'
        },
        {
          text: 'Option 3 ',
          value: '3'
        }
      ],
      "checked": "2",
      "stacked": ""
    }
  })], 1), _vm._v(" "), _c('b-form-group', {
    attrs: {
      "label": "Inline radios",
      "label-for": "basicInlineRadios",
      "label-cols": 3
    }
  }, [_c('b-form-radio-group', {
    attrs: {
      "id": "basicInlineRadios",
      "plain": true,
      "options": [{
          text: 'Option 1 ',
          value: '1'
        },
        {
          text: 'Option 2 ',
          value: '2'
        },
        {
          text: 'Option 3 ',
          value: '3'
        }
      ],
      "checked": 3
    }
  })], 1), _vm._v(" "), _c('b-form-group', {
    attrs: {
      "label": "Checkboxes",
      "label-for": "basicCheckboxes",
      "label-cols": 3
    }
  }, [_c('b-form-checkbox-group', {
    attrs: {
      "stacked": "",
      "id": "basicCheckboxes",
      "name": "Checkboxes",
      "plain": true,
      "checked": [2, 3]
    }
  }, [_c('b-form-checkbox', {
    attrs: {
      "value": "1"
    }
  }, [_vm._v("Option 1")]), _vm._v(" "), _c('b-form-checkbox', {
    attrs: {
      "value": "2"
    }
  }, [_vm._v("Option 2")]), _vm._v(" "), _c('b-form-checkbox', {
    attrs: {
      "value": "3"
    }
  }, [_vm._v("Option 3")])], 1)], 1), _vm._v(" "), _c('b-form-group', {
    attrs: {
      "label": "Inline checkboxes",
      "label-for": "basicInlineCheckboxes",
      "label-cols": 3
    }
  }, [_c('b-form-checkbox-group', {
    attrs: {
      "id": "basicInlineCheckboxes",
      "name": "InlineCheckboxes",
      "plain": true,
      "checked": [1, 3]
    }
  }, [_c('b-form-checkbox', {
    attrs: {
      "plain": true,
      "value": "1"
    }
  }, [_vm._v("Option 1")]), _vm._v(" "), _c('b-form-checkbox', {
    attrs: {
      "plain": true,
      "value": "2"
    }
  }, [_vm._v("Option 2")]), _vm._v(" "), _c('b-form-checkbox', {
    attrs: {
      "plain": true,
      "value": "3"
    }
  }, [_vm._v("Option 3")])], 1)], 1), _vm._v(" "), _c('b-form-group', {
    attrs: {
      "label": "Radios - custom",
      "label-for": "basicRadiosCustom",
      "label-cols": 3
    }
  }, [_c('b-form-radio-group', {
    attrs: {
      "id": "basicRadiosCustom",
      "value": "1",
      "stacked": ""
    }
  }, [_c('div', {
    staticClass: "custom-control custom-radio"
  }, [_c('input', {
    staticClass: "custom-control-input",
    attrs: {
      "type": "radio",
      "id": "customRadio1",
      "name": "customRadio",
      "value": "1"
    }
  }), _vm._v(" "), _c('label', {
    staticClass: "custom-control-label",
    attrs: {
      "for": "customRadio1"
    }
  }, [_vm._v("Option 1")])]), _vm._v(" "), _c('div', {
    staticClass: "custom-control custom-radio"
  }, [_c('input', {
    staticClass: "custom-control-input",
    attrs: {
      "type": "radio",
      "id": "customRadio2",
      "name": "customRadio",
      "value": "2",
      "checked": ""
    }
  }), _vm._v(" "), _c('label', {
    staticClass: "custom-control-label",
    attrs: {
      "for": "customRadio2"
    }
  }, [_vm._v("Option 2")])]), _vm._v(" "), _c('div', {
    staticClass: "custom-control custom-radio"
  }, [_c('input', {
    staticClass: "custom-control-input",
    attrs: {
      "type": "radio",
      "id": "customRadio3",
      "name": "customRadio",
      "value": "3"
    }
  }), _vm._v(" "), _c('label', {
    staticClass: "custom-control-label",
    attrs: {
      "for": "customRadio3"
    }
  }, [_vm._v("Option 3")])])])], 1), _vm._v(" "), _c('b-form-group', {
    attrs: {
      "label": "Inline radios - custom",
      "label-for": "basicCustomRadios1",
      "label-cols": 3
    }
  }, [_c('b-form-radio-group', {
    attrs: {
      "id": "basicCustomRadios1",
      "name": "customRadioInline1"
    }
  }, [_c('div', {
    staticClass: "custom-control custom-radio custom-control-inline"
  }, [_c('input', {
    staticClass: "custom-control-input",
    attrs: {
      "type": "radio",
      "id": "customRadioInline1",
      "name": "customRadioInline1",
      "value": "1"
    }
  }), _vm._v(" "), _c('label', {
    staticClass: "custom-control-label",
    attrs: {
      "for": "customRadioInline1"
    }
  }, [_vm._v("Option 1")])]), _vm._v(" "), _c('div', {
    staticClass: "custom-control custom-radio custom-control-inline"
  }, [_c('input', {
    staticClass: "custom-control-input",
    attrs: {
      "type": "radio",
      "id": "customRadioInline2",
      "name": "customRadioInline1",
      "value": "2",
      "checked": ""
    }
  }), _vm._v(" "), _c('label', {
    staticClass: "custom-control-label",
    attrs: {
      "for": "customRadioInline2"
    }
  }, [_vm._v("Option 2")])]), _vm._v(" "), _c('div', {
    staticClass: "custom-control custom-radio custom-control-inline"
  }, [_c('input', {
    staticClass: "custom-control-input",
    attrs: {
      "type": "radio",
      "id": "customRadioInline3",
      "name": "customRadioInline1",
      "value": "3"
    }
  }), _vm._v(" "), _c('label', {
    staticClass: "custom-control-label",
    attrs: {
      "for": "customRadioInline3"
    }
  }, [_vm._v("Option 3")])])])], 1), _vm._v(" "), _c('b-form-group', {
    attrs: {
      "label": "Checkboxes - custom",
      "label-for": "basicCustomCheckboxes",
      "label-cols": 3
    }
  }, [_c('b-form-checkbox-group', {
    attrs: {
      "stacked": "",
      "id": "basicCustomCheckboxes"
    }
  }, [_c('div', {
    staticClass: "custom-control custom-checkbox"
  }, [_c('input', {
    staticClass: "custom-control-input",
    attrs: {
      "type": "checkbox",
      "id": "customChk1",
      "value": "1",
      "checked": ""
    }
  }), _vm._v(" "), _c('label', {
    staticClass: "custom-control-label",
    attrs: {
      "for": "customChk1"
    }
  }, [_vm._v("Option 1")])]), _vm._v(" "), _c('div', {
    staticClass: "custom-control custom-checkbox"
  }, [_c('input', {
    staticClass: "custom-control-input",
    attrs: {
      "type": "checkbox",
      "id": "customChk2",
      "value": "2"
    }
  }), _vm._v(" "), _c('label', {
    staticClass: "custom-control-label",
    attrs: {
      "for": "customChk2"
    }
  }, [_vm._v("Option 2")])]), _vm._v(" "), _c('div', {
    staticClass: "custom-control custom-checkbox"
  }, [_c('input', {
    staticClass: "custom-control-input",
    attrs: {
      "type": "checkbox",
      "id": "customChk3",
      "value": "3"
    }
  }), _vm._v(" "), _c('label', {
    staticClass: "custom-control-label",
    attrs: {
      "for": "customChk3"
    }
  }, [_vm._v("Option 3")])])])], 1), _vm._v(" "), _c('b-form-group', {
    attrs: {
      "label": "Inline checkboxes - custom",
      "label-for": "basicInlineCustomCheckboxes",
      "label-cols": 3
    }
  }, [_c('b-form-checkbox-group', {
    attrs: {
      "id": "basicInlineCustomCheckboxes"
    }
  }, [_c('div', {
    staticClass: "custom-control custom-checkbox custom-control-inline"
  }, [_c('input', {
    staticClass: "custom-control-input",
    attrs: {
      "type": "checkbox",
      "id": "customInChk1",
      "value": "1"
    }
  }), _vm._v(" "), _c('label', {
    staticClass: "custom-control-label",
    attrs: {
      "for": "customInChk1"
    }
  }, [_vm._v("Option 1")])]), _vm._v(" "), _c('div', {
    staticClass: "custom-control custom-checkbox custom-control-inline"
  }, [_c('input', {
    staticClass: "custom-control-input",
    attrs: {
      "type": "checkbox",
      "id": "customInChk2",
      "value": "2",
      "checked": ""
    }
  }), _vm._v(" "), _c('label', {
    staticClass: "custom-control-label",
    attrs: {
      "for": "customInChk2"
    }
  }, [_vm._v("Option 2")])]), _vm._v(" "), _c('div', {
    staticClass: "custom-control custom-checkbox custom-control-inline"
  }, [_c('input', {
    staticClass: "custom-control-input",
    attrs: {
      "type": "checkbox",
      "id": "customInChk3",
      "value": "3"
    }
  }), _vm._v(" "), _c('label', {
    staticClass: "custom-control-label",
    attrs: {
      "for": "customInChk3"
    }
  }, [_vm._v("Option 3")])])])], 1), _vm._v(" "), _c('b-form-group', {
    attrs: {
      "label": "File input",
      "label-for": "fileInput",
      "label-cols": 3
    }
  }, [_c('b-form-file', {
    attrs: {
      "id": "fileInput",
      "plain": true
    }
  })], 1), _vm._v(" "), _c('b-form-group', {
    attrs: {
      "label": "Multiple file input",
      "label-for": "fileInputMulti",
      "label-cols": 3
    }
  }, [_c('b-form-file', {
    attrs: {
      "id": "fileInputMulti",
      "plain": true,
      "multiple": true
    }
  })], 1), _vm._v(" "), _c('div', {
    attrs: {
      "slot": "footer"
    },
    slot: "footer"
  }, [_c('b-button', {
    attrs: {
      "type": "submit",
      "size": "sm",
      "variant": "primary"
    }
  }, [_c('i', {
    staticClass: "fa fa-dot-circle-o"
  }), _vm._v(" Submit")]), _vm._v(" "), _c('b-button', {
    attrs: {
      "type": "reset",
      "size": "sm",
      "variant": "danger"
    }
  }, [_c('i', {
    staticClass: "fa fa-ban"
  }), _vm._v(" Reset")])], 1)], 1)], 1), _vm._v(" "), _c('b-card', [_c('div', {
    attrs: {
      "slot": "header"
    },
    slot: "header"
  }, [_c('strong', [_vm._v("Inline")]), _vm._v(" Form\n        ")]), _vm._v(" "), _c('b-form', {
    attrs: {
      "inline": ""
    }
  }, [_c('label', {
    staticClass: "mr-sm-2",
    attrs: {
      "for": "inlineInput1"
    }
  }, [_vm._v("Name: ")]), _vm._v(" "), _c('b-input', {
    attrs: {
      "id": "inlineInput1",
      "type": "text",
      "placeholder": "Jane Doe"
    }
  }), _vm._v(" "), _c('label', {
    staticClass: "mx-sm-2",
    attrs: {
      "for": "inlineInput2"
    }
  }, [_vm._v("Email: ")]), _vm._v(" "), _c('b-input', {
    attrs: {
      "id": "inlineInput2",
      "type": "email",
      "placeholder": "jane.doe@example.com",
      "autocomplete": "email"
    }
  })], 1), _vm._v(" "), _c('div', {
    attrs: {
      "slot": "footer"
    },
    slot: "footer"
  }, [_c('b-button', {
    attrs: {
      "type": "submit",
      "size": "sm",
      "variant": "primary"
    }
  }, [_c('i', {
    staticClass: "fa fa-dot-circle-o"
  }), _vm._v(" Submit")]), _vm._v(" "), _c('b-button', {
    attrs: {
      "type": "reset",
      "size": "sm",
      "variant": "danger"
    }
  }, [_c('i', {
    staticClass: "fa fa-ban"
  }), _vm._v(" Reset")])], 1)], 1)], 1), _vm._v(" "), _c('b-col', {
    attrs: {
      "md": "6"
    }
  }, [_c('b-card', [_c('div', {
    attrs: {
      "slot": "header"
    },
    slot: "header"
  }, [_c('strong', [_vm._v("Horizontal")]), _vm._v(" Form\n        ")]), _vm._v(" "), _c('b-form', [_c('b-form-group', {
    attrs: {
      "label": "Email",
      "label-for": "horizEmail",
      "description": "Please enter your email.",
      "label-cols": 3
    }
  }, [_c('b-form-input', {
    attrs: {
      "id": "horizEmail",
      "type": "email",
      "placeholder": "Enter Email..",
      "autocomplete": "username email"
    }
  })], 1), _vm._v(" "), _c('b-form-group', {
    attrs: {
      "label": "Password",
      "label-for": "horizPass",
      "description": "Please enter your password.",
      "label-cols": 3
    }
  }, [_c('b-form-input', {
    attrs: {
      "id": "horizPass",
      "type": "password",
      "placeholder": "Enter Password..",
      "autocomplete": "current-password"
    }
  })], 1), _vm._v(" "), _c('div', {
    attrs: {
      "slot": "footer"
    },
    slot: "footer"
  }, [_c('b-button', {
    attrs: {
      "type": "submit",
      "size": "sm",
      "variant": "primary"
    }
  }, [_c('i', {
    staticClass: "fa fa-dot-circle-o"
  }), _vm._v(" Submit")]), _vm._v(" "), _c('b-button', {
    attrs: {
      "type": "reset",
      "size": "sm",
      "variant": "danger"
    }
  }, [_c('i', {
    staticClass: "fa fa-ban"
  }), _vm._v(" Reset")])], 1)], 1)], 1), _vm._v(" "), _c('b-card', [_c('div', {
    attrs: {
      "slot": "header"
    },
    slot: "header"
  }, [_c('strong', [_vm._v("Normal")]), _vm._v(" Form\n        ")]), _vm._v(" "), _c('b-form', [_c('b-form-group', {
    attrs: {
      "validated": "",
      "label": "Email",
      "label-for": "normalEmail",
      "description": "Please enter your email."
    }
  }, [_c('b-form-input', {
    attrs: {
      "id": "normalEmail",
      "type": "email",
      "placeholder": "Enter Email..",
      "required": "",
      "autocomplete": "email"
    }
  })], 1), _vm._v(" "), _c('b-form-group', {
    attrs: {
      "validated": "",
      "label": "Password",
      "label-for": "normalPass",
      "description": "Please enter your password."
    }
  }, [_c('b-form-input', {
    attrs: {
      "id": "normalPass",
      "type": "password",
      "placeholder": "Enter Password..",
      "required": "",
      "autocomplete": "current-password"
    }
  })], 1), _vm._v(" "), _c('div', {
    attrs: {
      "slot": "footer"
    },
    slot: "footer"
  }, [_c('b-button', {
    attrs: {
      "type": "submit",
      "size": "sm",
      "variant": "primary"
    }
  }, [_c('i', {
    staticClass: "fa fa-dot-circle-o"
  }), _vm._v(" Submit")]), _vm._v(" "), _c('b-button', {
    attrs: {
      "type": "reset",
      "size": "sm",
      "variant": "danger"
    }
  }, [_c('i', {
    staticClass: "fa fa-ban"
  }), _vm._v(" Reset")])], 1)], 1)], 1), _vm._v(" "), _c('b-card', {
    attrs: {
      "no-body": "",
      "no-block": true
    }
  }, [_c('div', {
    attrs: {
      "slot": "header"
    },
    slot: "header"
  }, [_vm._v("\n          Input "), _c('strong', [_vm._v("Grid")])]), _vm._v(" "), _c('b-card-body', [_c('b-row', {
    staticClass: "form-group"
  }, [_c('b-col', {
    attrs: {
      "sm": "3"
    }
  }, [_c('b-form-input', {
    attrs: {
      "type": "text",
      "placeholder": ".col-sm-3"
    }
  })], 1)], 1), _vm._v(" "), _c('b-row', {
    staticClass: "form-group"
  }, [_c('b-col', {
    attrs: {
      "sm": "4"
    }
  }, [_c('b-form-input', {
    attrs: {
      "type": "text",
      "placeholder": ".col-sm-4"
    }
  })], 1)], 1), _vm._v(" "), _c('b-row', {
    staticClass: "form-group"
  }, [_c('b-col', {
    attrs: {
      "sm": "5"
    }
  }, [_c('b-form-input', {
    attrs: {
      "type": "text",
      "placeholder": ".col-sm-5"
    }
  })], 1)], 1), _vm._v(" "), _c('b-row', {
    staticClass: "form-group"
  }, [_c('b-col', {
    attrs: {
      "sm": "6"
    }
  }, [_c('b-form-input', {
    attrs: {
      "type": "text",
      "placeholder": ".col-sm-6"
    }
  })], 1)], 1), _vm._v(" "), _c('b-row', {
    staticClass: "form-group"
  }, [_c('b-col', {
    attrs: {
      "sm": "7"
    }
  }, [_c('b-form-input', {
    attrs: {
      "type": "text",
      "placeholder": ".col-sm-7"
    }
  })], 1)], 1), _vm._v(" "), _c('b-row', {
    staticClass: "form-group"
  }, [_c('b-col', {
    attrs: {
      "sm": "8"
    }
  }, [_c('b-form-input', {
    attrs: {
      "type": "text",
      "placeholder": ".col-sm-8"
    }
  })], 1)], 1), _vm._v(" "), _c('b-row', {
    staticClass: "form-group"
  }, [_c('b-col', {
    attrs: {
      "sm": "9"
    }
  }, [_c('b-form-input', {
    attrs: {
      "type": "text",
      "placeholder": ".col-sm-9"
    }
  })], 1)], 1), _vm._v(" "), _c('b-row', {
    staticClass: "form-group"
  }, [_c('b-col', {
    attrs: {
      "sm": "10"
    }
  }, [_c('b-form-input', {
    attrs: {
      "type": "text",
      "placeholder": ".col-sm-10"
    }
  })], 1)], 1), _vm._v(" "), _c('b-row', {
    staticClass: "form-group"
  }, [_c('b-col', {
    attrs: {
      "sm": "11"
    }
  }, [_c('b-form-input', {
    attrs: {
      "type": "text",
      "placeholder": ".col-sm-11"
    }
  })], 1)], 1), _vm._v(" "), _c('b-row', {
    staticClass: "form-group"
  }, [_c('b-col', {
    attrs: {
      "sm": "12"
    }
  }, [_c('b-form-input', {
    attrs: {
      "type": "text",
      "placeholder": ".col-sm-12"
    }
  })], 1)], 1)], 1), _vm._v(" "), _c('div', {
    attrs: {
      "slot": "footer"
    },
    slot: "footer"
  }, [_c('b-button', {
    attrs: {
      "type": "submit",
      "size": "sm",
      "variant": "primary"
    }
  }, [_c('i', {
    staticClass: "fa fa-user"
  }), _vm._v(" Login")]), _vm._v(" "), _c('b-button', {
    attrs: {
      "type": "reset",
      "size": "sm",
      "variant": "danger"
    }
  }, [_c('i', {
    staticClass: "fa fa-ban"
  }), _vm._v(" Reset")])], 1)], 1), _vm._v(" "), _c('b-card', [_c('div', {
    attrs: {
      "slot": "header"
    },
    slot: "header"
  }, [_vm._v("\n          Input "), _c('strong', [_vm._v("Sizes")])]), _vm._v(" "), _c('b-form-group', {
    attrs: {
      "label": "Small input",
      "label-for": "smInput",
      "label-size": "sm",
      "label-cols": 5
    }
  }, [_c('b-form-input', {
    attrs: {
      "id": "smInput",
      "type": "text",
      "size": "sm",
      "placeholder": "size='sm'"
    }
  })], 1), _vm._v(" "), _c('b-form-group', {
    attrs: {
      "label": "Default input",
      "label-for": "defaultInput",
      "label-cols": 5
    }
  }, [_c('b-form-input', {
    attrs: {
      "id": "defaultInput",
      "type": "text",
      "placeholder": "normal"
    }
  })], 1), _vm._v(" "), _c('b-form-group', {
    attrs: {
      "label": "Large input",
      "label-for": "lgInput",
      "label-size": "lg",
      "label-cols": 5
    }
  }, [_c('b-form-input', {
    attrs: {
      "id": "lgInput",
      "type": "text",
      "size": "lg",
      "placeholder": "size='lg'"
    }
  })], 1), _vm._v(" "), _c('div', {
    attrs: {
      "slot": "footer"
    },
    slot: "footer"
  }, [_c('b-button', {
    attrs: {
      "type": "submit",
      "size": "sm",
      "variant": "primary"
    }
  }, [_c('i', {
    staticClass: "fa fa-dot-circle-o"
  }), _vm._v(" Submit")]), _vm._v(" "), _c('b-button', {
    attrs: {
      "type": "reset",
      "size": "sm",
      "variant": "danger"
    }
  }, [_c('i', {
    staticClass: "fa fa-ban"
  }), _vm._v(" Reset")])], 1)], 1)], 1)], 1), _vm._v(" "), _c('b-row', [_c('b-col', {
    attrs: {
      "sm": "12",
      "md": "6"
    }
  }, [_c('b-card', {
    attrs: {
      "no-body": "",
      "no-block": true
    }
  }, [_c('div', {
    attrs: {
      "slot": "header"
    },
    slot: "header"
  }, [_c('strong', [_vm._v("Validation feedback")]), _vm._v(" Form\n        ")]), _vm._v(" "), _c('b-card-body', [_c('b-form', [_c('b-form-group', {
    attrs: {
      "validated": ""
    }
  }, [_c('label', {
    staticClass: "col-form-label",
    attrs: {
      "for": "inputIsValid"
    }
  }, [_vm._v("Input is valid")]), _vm._v(" "), _c('input', {
    staticClass: "form-control is-valid",
    attrs: {
      "type": "text",
      "id": "inputIsValid"
    }
  }), _vm._v(" "), _c('b-form-valid-feedback', [_vm._v("\n                Input is valid.\n              ")])], 1), _vm._v(" "), _c('b-form-group', [_c('label', {
    staticClass: "col-form-label",
    attrs: {
      "for": "inputIsInvalid"
    }
  }, [_vm._v("Input is invalid")]), _vm._v(" "), _c('input', {
    staticClass: "form-control is-invalid",
    attrs: {
      "type": "text",
      "id": "inputIsInvalid"
    }
  }), _vm._v(" "), _c('b-form-invalid-feedback', [_vm._v("\n                Please provide a valid information.\n              ")])], 1)], 1)], 1)], 1)], 1), _vm._v(" "), _c('b-col', {
    attrs: {
      "sm": "12",
      "md": "6"
    }
  }, [_c('b-card', {
    attrs: {
      "no-body": "",
      "no-block": true
    }
  }, [_c('div', {
    attrs: {
      "slot": "header"
    },
    slot: "header"
  }, [_c('strong', [_vm._v("Validation feedback")]), _vm._v(" Form\n        ")]), _vm._v(" "), _c('b-card-body', [_c('b-form', {
    attrs: {
      "validated": "",
      "novalidate": ""
    }
  }, [_c('b-form-group', {
    attrs: {
      "label-for": "inputSuccess2",
      "label": "Non-required input"
    }
  }, [_c('b-form-input', {
    staticClass: "form-control-success",
    attrs: {
      "type": "text",
      "id": "inputSuccess2"
    }
  }), _vm._v(" "), _c('b-form-valid-feedback', [_vm._v("\n                Input is not required.\n              ")])], 1), _vm._v(" "), _c('b-form-group', {
    attrs: {
      "label-for": "inputError2",
      "label": "Required input"
    }
  }, [_c('b-form-input', {
    staticClass: "form-control-warning",
    attrs: {
      "type": "text",
      "id": "inputError2",
      "required": ""
    }
  }), _vm._v(" "), _c('b-form-valid-feedback', [_vm._v("\n                Input provided.\n              ")]), _vm._v(" "), _c('b-form-invalid-feedback', [_vm._v("\n                Please provide a required input.\n              ")])], 1)], 1)], 1)], 1)], 1)], 1), _vm._v(" "), _c('b-row', [_c('b-col', {
    attrs: {
      "md": "4"
    }
  }, [_c('b-card', [_c('div', {
    attrs: {
      "slot": "header"
    },
    slot: "header"
  }, [_c('strong', [_vm._v("Icon/Text")]), _vm._v(" Groups\n        ")]), _vm._v(" "), _c('b-form-group', [_c('b-input-group', [_c('b-input-group-prepend', [_c('b-input-group-text', [_c('i', {
    staticClass: "fa fa-user"
  })])], 1), _vm._v(" "), _c('b-form-input', {
    attrs: {
      "type": "text",
      "placeholder": "Username"
    }
  })], 1)], 1), _vm._v(" "), _c('b-form-group', [_c('b-input-group', [_c('b-form-input', {
    attrs: {
      "type": "email",
      "placeholder": "Email",
      "autocomplete": "email"
    }
  }), _vm._v(" "), _c('b-input-group-append', [_c('b-input-group-text', [_c('i', {
    staticClass: "fa fa-envelope-o"
  })])], 1)], 1)], 1), _vm._v(" "), _c('b-form-group', [_c('b-input-group', [_c('b-input-group-prepend', [_c('b-input-group-text', [_c('i', {
    staticClass: "fa fa-euro"
  })])], 1), _vm._v(" "), _c('b-form-input', {
    attrs: {
      "type": "text",
      "placeholder": "ex. $1.000.000"
    }
  }), _vm._v(" "), _c('b-input-group-append', [_c('b-input-group-text', [_vm._v(".00")])], 1)], 1)], 1), _vm._v(" "), _c('div', {
    attrs: {
      "slot": "footer"
    },
    slot: "footer"
  }, [_c('b-button', {
    attrs: {
      "type": "submit",
      "size": "sm",
      "variant": "success"
    }
  }, [_c('i', {
    staticClass: "fa fa-dot-circle-o"
  }), _vm._v(" Submit")]), _vm._v(" "), _c('b-button', {
    attrs: {
      "type": "reset",
      "size": "sm",
      "variant": "danger"
    }
  }, [_c('i', {
    staticClass: "fa fa-ban"
  }), _vm._v(" Reset")])], 1)], 1)], 1), _vm._v(" "), _c('b-col', {
    attrs: {
      "md": "4"
    }
  }, [_c('b-card', [_c('div', {
    attrs: {
      "slot": "header"
    },
    slot: "header"
  }, [_c('strong', [_vm._v("Buttons")]), _vm._v(" Groups\n        ")]), _vm._v(" "), _c('b-form-group', [_c('b-input-group', [_c('b-input-group-prepend', [_c('b-button', {
    attrs: {
      "variant": "primary"
    }
  }, [_c('i', {
    staticClass: "fa fa-search"
  }), _vm._v(" Search\n              ")])], 1), _vm._v(" "), _c('b-form-input', {
    attrs: {
      "type": "text",
      "placeholder": "Username"
    }
  })], 1)], 1), _vm._v(" "), _c('b-form-group', [_c('b-input-group', [_c('b-form-input', {
    attrs: {
      "type": "email",
      "placeholder": "Email",
      "autocomplete": "email"
    }
  }), _vm._v(" "), _c('b-input-group-append', [_c('b-button', {
    attrs: {
      "variant": "primary"
    }
  }, [_vm._v("Submit")])], 1)], 1)], 1), _vm._v(" "), _c('b-form-group', [_c('b-input-group', [_c('b-input-group-prepend', [_c('b-button', {
    attrs: {
      "variant": "primary"
    }
  }, [_c('i', {
    staticClass: "fa fa-facebook"
  })])], 1), _vm._v(" "), _c('b-form-input', {
    attrs: {
      "type": "email",
      "placeholder": "Email",
      "autocomplete": "email"
    }
  }), _vm._v(" "), _c('b-input-group-append', [_c('b-button', {
    attrs: {
      "variant": "primary"
    }
  }, [_c('i', {
    staticClass: "fa fa-twitter"
  })])], 1)], 1)], 1), _vm._v(" "), _c('div', {
    attrs: {
      "slot": "footer"
    },
    slot: "footer"
  }, [_c('b-button', {
    attrs: {
      "type": "submit",
      "size": "sm",
      "variant": "success"
    }
  }, [_c('i', {
    staticClass: "fa fa-dot-circle-o"
  }), _vm._v(" Submit")]), _vm._v(" "), _c('b-button', {
    attrs: {
      "type": "reset",
      "size": "sm",
      "variant": "danger"
    }
  }, [_c('i', {
    staticClass: "fa fa-ban"
  }), _vm._v(" Reset")])], 1)], 1)], 1), _vm._v(" "), _c('b-col', {
    attrs: {
      "md": "4"
    }
  }, [_c('b-card', [_c('div', {
    attrs: {
      "slot": "header"
    },
    slot: "header"
  }, [_c('strong', [_vm._v("Dropdowns")]), _vm._v(" Groups\n        ")]), _vm._v(" "), _c('b-form-group', [_c('b-input-group', [_c('b-input-group-prepend', [_c('b-dropdown', {
    attrs: {
      "text": "Action",
      "variant": "primary"
    }
  }, [_c('b-dropdown-item', [_vm._v("Action")]), _vm._v(" "), _c('b-dropdown-item', [_vm._v("Another action")]), _vm._v(" "), _c('b-dropdown-item', [_vm._v("Something else here...")]), _vm._v(" "), _c('b-dropdown-item', {
    attrs: {
      "disabled": ""
    }
  }, [_vm._v("Disabled action")])], 1)], 1), _vm._v(" "), _c('b-form-input', {
    attrs: {
      "placeholder": "Username"
    }
  })], 1)], 1), _vm._v(" "), _c('b-form-group', [_c('b-input-group', [_c('b-form-input', {
    attrs: {
      "placeholder": "Email"
    }
  }), _vm._v(" "), _c('b-input-group-append', [_c('b-dropdown', {
    attrs: {
      "text": "Action",
      "variant": "primary",
      "right": ""
    }
  }, [_c('b-dropdown-item', [_vm._v("Action")]), _vm._v(" "), _c('b-dropdown-item', [_vm._v("Another action")]), _vm._v(" "), _c('b-dropdown-item', [_vm._v("Something else here...")]), _vm._v(" "), _c('b-dropdown-item', {
    attrs: {
      "disabled": ""
    }
  }, [_vm._v("Disabled action")])], 1)], 1)], 1)], 1), _vm._v(" "), _c('b-form-group', [_c('b-input-group', [_c('b-input-group-prepend', [_c('b-dropdown', {
    attrs: {
      "text": "Split",
      "variant": "primary",
      "split": ""
    }
  }, [_c('b-dropdown-item', {
    attrs: {
      "href": "#"
    }
  }, [_vm._v("Action")]), _vm._v(" "), _c('b-dropdown-item', {
    attrs: {
      "href": "#"
    }
  }, [_vm._v("Another action")]), _vm._v(" "), _c('b-dropdown-item', {
    attrs: {
      "href": "#"
    }
  }, [_vm._v("Something else here...")]), _vm._v(" "), _c('b-dropdown-item', {
    attrs: {
      "disabled": ""
    }
  }, [_vm._v("Disabled action")])], 1)], 1), _vm._v(" "), _c('b-form-input', {
    attrs: {
      "placeholder": "..."
    }
  }), _vm._v(" "), _c('b-input-group-append', [_c('b-dropdown', {
    attrs: {
      "text": "Action",
      "variant": "primary",
      "right": ""
    }
  }, [_c('b-dropdown-item', [_vm._v("Action")]), _vm._v(" "), _c('b-dropdown-item', [_vm._v("Another action")]), _vm._v(" "), _c('b-dropdown-item', [_vm._v("Something else here...")]), _vm._v(" "), _c('b-dropdown-item', {
    attrs: {
      "disabled": ""
    }
  }, [_vm._v("Disabled action")])], 1)], 1)], 1)], 1), _vm._v(" "), _c('div', {
    attrs: {
      "slot": "footer"
    },
    slot: "footer"
  }, [_c('b-button', {
    attrs: {
      "type": "submit",
      "size": "sm",
      "variant": "success"
    }
  }, [_c('i', {
    staticClass: "fa fa-dot-circle-o"
  }), _vm._v(" Submit")]), _vm._v(" "), _c('b-button', {
    attrs: {
      "type": "reset",
      "size": "sm",
      "variant": "danger"
    }
  }, [_c('i', {
    staticClass: "fa fa-ban"
  }), _vm._v(" Reset")])], 1)], 1)], 1)], 1), _vm._v(" "), _c('b-row', [_c('b-col', {
    attrs: {
      "md": "6"
    }
  }, [_c('b-card', [_c('div', {
    attrs: {
      "slot": "header"
    },
    slot: "header"
  }, [_vm._v("\n          Use the grid for big devices! "), _c('small', [_c('code', [_vm._v(".col-lg-*")]), _vm._v(" "), _c('code', [_vm._v(".col-md-*")]), _vm._v(" "), _c('code', [_vm._v(".col-sm-*")])])]), _vm._v(" "), _c('b-row', {
    staticClass: "form-group"
  }, [_c('b-col', {
    attrs: {
      "md": "8"
    }
  }, [_c('b-form-input', {
    attrs: {
      "type": "text",
      "placeholder": ".col-md-8"
    }
  })], 1), _vm._v(" "), _c('b-col', {
    attrs: {
      "md": "4"
    }
  }, [_c('b-form-input', {
    attrs: {
      "type": "text",
      "placeholder": ".col-md-4"
    }
  })], 1)], 1), _vm._v(" "), _c('b-row', {
    staticClass: "form-group"
  }, [_c('b-col', {
    attrs: {
      "md": "7"
    }
  }, [_c('b-form-input', {
    attrs: {
      "type": "text",
      "placeholder": ".col-md-7"
    }
  })], 1), _vm._v(" "), _c('b-col', {
    attrs: {
      "md": "5"
    }
  }, [_c('b-form-input', {
    attrs: {
      "type": "text",
      "placeholder": ".col-md-5"
    }
  })], 1)], 1), _vm._v(" "), _c('b-row', {
    staticClass: "form-group"
  }, [_c('b-col', {
    attrs: {
      "md": "6"
    }
  }, [_c('b-form-input', {
    attrs: {
      "type": "text",
      "placeholder": ".col-md-6"
    }
  })], 1), _vm._v(" "), _c('b-col', {
    attrs: {
      "md": "6"
    }
  }, [_c('b-form-input', {
    attrs: {
      "type": "text",
      "placeholder": ".col-md-6"
    }
  })], 1)], 1), _vm._v(" "), _c('b-row', {
    staticClass: "form-group"
  }, [_c('b-col', {
    attrs: {
      "md": "5"
    }
  }, [_c('b-form-input', {
    attrs: {
      "type": "text",
      "placeholder": ".col-md-5"
    }
  })], 1), _vm._v(" "), _c('b-col', {
    attrs: {
      "md": "7"
    }
  }, [_c('b-form-input', {
    attrs: {
      "type": "text",
      "placeholder": ".col-md-7"
    }
  })], 1)], 1), _vm._v(" "), _c('b-row', {
    staticClass: "form-group"
  }, [_c('b-col', {
    attrs: {
      "md": "4"
    }
  }, [_c('b-form-input', {
    attrs: {
      "type": "text",
      "placeholder": ".col-md-4"
    }
  })], 1), _vm._v(" "), _c('b-col', {
    attrs: {
      "md": "8"
    }
  }, [_c('b-form-input', {
    attrs: {
      "type": "text",
      "placeholder": ".col-md-8"
    }
  })], 1)], 1), _vm._v(" "), _c('div', {
    attrs: {
      "slot": "footer"
    },
    slot: "footer"
  }, [_c('b-button', {
    attrs: {
      "type": "submit",
      "size": "sm",
      "variant": "primary"
    }
  }, [_vm._v("Action")]), _vm._v(" "), _c('b-button', {
    attrs: {
      "type": "button",
      "size": "sm",
      "variant": "danger"
    }
  }, [_vm._v("Action")]), _vm._v(" "), _c('b-button', {
    staticClass: "btn btn-sm btn-warning",
    attrs: {
      "type": "button"
    }
  }, [_vm._v("Action")]), _vm._v(" "), _c('b-button', {
    staticClass: "btn btn-sm btn-info",
    attrs: {
      "type": "button"
    }
  }, [_vm._v("Action")]), _vm._v(" "), _c('b-button', {
    attrs: {
      "type": "button",
      "size": "sm",
      "variant": "success"
    }
  }, [_vm._v("Action")])], 1)], 1)], 1), _vm._v(" "), _c('b-col', {
    attrs: {
      "md": "6"
    }
  }, [_c('b-card', [_c('div', {
    attrs: {
      "slot": "header"
    },
    slot: "header"
  }, [_vm._v("\n          Input Grid for small devices! "), _c('small', [_c('code', [_vm._v(".col-*")])])]), _vm._v(" "), _c('b-row', {
    staticClass: "form-group"
  }, [_c('b-col', {
    attrs: {
      "cols": "4"
    }
  }, [_c('b-form-input', {
    attrs: {
      "type": "text",
      "placeholder": ".col-4"
    }
  })], 1), _vm._v(" "), _c('b-col', {
    attrs: {
      "cols": "8"
    }
  }, [_c('b-form-input', {
    attrs: {
      "type": "text",
      "placeholder": ".col-8"
    }
  })], 1)], 1), _vm._v(" "), _c('b-row', {
    staticClass: "form-group"
  }, [_c('b-col', {
    attrs: {
      "cols": "5"
    }
  }, [_c('b-form-input', {
    attrs: {
      "type": "text",
      "placeholder": ".col-5"
    }
  })], 1), _vm._v(" "), _c('b-col', {
    attrs: {
      "cols": "7"
    }
  }, [_c('b-form-input', {
    attrs: {
      "type": "text",
      "placeholder": ".col-7"
    }
  })], 1)], 1), _vm._v(" "), _c('b-row', {
    staticClass: "form-group"
  }, [_c('b-col', {
    attrs: {
      "cols": "6"
    }
  }, [_c('b-form-input', {
    attrs: {
      "type": "text",
      "placeholder": ".col-6"
    }
  })], 1), _vm._v(" "), _c('b-col', {
    attrs: {
      "cols": "6"
    }
  }, [_c('b-form-input', {
    attrs: {
      "type": "text",
      "placeholder": ".col-6"
    }
  })], 1)], 1), _vm._v(" "), _c('b-row', {
    staticClass: "form-group"
  }, [_c('b-col', {
    attrs: {
      "cols": "7"
    }
  }, [_c('b-form-input', {
    attrs: {
      "type": "text",
      "placeholder": ".col-5"
    }
  })], 1), _vm._v(" "), _c('b-col', {
    attrs: {
      "cols": "5"
    }
  }, [_c('b-form-input', {
    attrs: {
      "type": "text",
      "placeholder": ".col-5"
    }
  })], 1)], 1), _vm._v(" "), _c('b-row', {
    staticClass: "form-group"
  }, [_c('b-col', {
    attrs: {
      "cols": "8"
    }
  }, [_c('b-form-input', {
    attrs: {
      "type": "text",
      "placeholder": ".col-8"
    }
  })], 1), _vm._v(" "), _c('b-col', {
    attrs: {
      "cols": "4"
    }
  }, [_c('b-form-input', {
    attrs: {
      "type": "text",
      "placeholder": ".col-4"
    }
  })], 1)], 1), _vm._v(" "), _c('div', {
    attrs: {
      "slot": "footer"
    },
    slot: "footer"
  }, [_c('b-button', {
    attrs: {
      "type": "submit",
      "size": "sm",
      "variant": "primary"
    }
  }, [_vm._v("Action")]), _vm._v(" "), _c('b-button', {
    attrs: {
      "type": "button",
      "size": "sm",
      "variant": "danger"
    }
  }, [_vm._v("Action")]), _vm._v(" "), _c('b-button', {
    staticClass: "btn btn-sm btn-warning",
    attrs: {
      "type": "button"
    }
  }, [_vm._v("Action")]), _vm._v(" "), _c('b-button', {
    staticClass: "btn btn-sm btn-info",
    attrs: {
      "type": "button"
    }
  }, [_vm._v("Action")]), _vm._v(" "), _c('b-button', {
    attrs: {
      "type": "button",
      "size": "sm",
      "variant": "success"
    }
  }, [_vm._v("Action")])], 1)], 1)], 1)], 1), _vm._v(" "), _c('b-row', [_c('b-col', {
    attrs: {
      "md": "4"
    }
  }, [_c('b-card', [_c('div', {
    attrs: {
      "slot": "header"
    },
    slot: "header"
  }, [_vm._v("\n          Example Form\n        ")]), _vm._v(" "), _c('b-form', [_c('b-form-group', [_c('b-input-group', [_c('b-input-group-prepend', [_c('b-input-group-text', [_vm._v("Username")])], 1), _vm._v(" "), _c('b-form-input', {
    attrs: {
      "type": "text"
    }
  }), _vm._v(" "), _c('b-input-group-append', [_c('b-input-group-text', [_c('i', {
    staticClass: "fa fa-user"
  })])], 1)], 1)], 1), _vm._v(" "), _c('b-form-group', [_c('b-input-group', [_c('b-input-group-prepend', [_c('b-input-group-text', [_vm._v("Email")])], 1), _vm._v(" "), _c('b-form-input', {
    attrs: {
      "type": "email",
      "autocomplete": "email"
    }
  }), _vm._v(" "), _c('b-input-group-append', [_c('b-input-group-text', [_c('i', {
    staticClass: "fa fa-envelope"
  })])], 1)], 1)], 1), _vm._v(" "), _c('b-form-group', [_c('b-input-group', [_c('b-input-group-prepend', [_c('b-input-group-text', [_vm._v("Password")])], 1), _vm._v(" "), _c('b-form-input', {
    attrs: {
      "type": "password",
      "autocomplete": "current-password"
    }
  }), _vm._v(" "), _c('b-input-group-append', [_c('b-input-group-text', [_c('i', {
    staticClass: "fa fa-asterisk"
  })])], 1)], 1)], 1), _vm._v(" "), _c('div', {
    staticClass: "form-group form-actions"
  }, [_c('b-button', {
    attrs: {
      "type": "submit",
      "size": "sm",
      "variant": "primary"
    }
  }, [_vm._v("Submit")])], 1)], 1)], 1)], 1), _vm._v(" "), _c('b-col', {
    attrs: {
      "md": "4"
    }
  }, [_c('b-card', [_c('div', {
    attrs: {
      "slot": "header"
    },
    slot: "header"
  }, [_vm._v("\n          Example Form\n        ")]), _vm._v(" "), _c('b-form', [_c('b-form-group', [_c('b-input-group', [_c('b-form-input', {
    attrs: {
      "type": "text",
      "placeholder": "Username"
    }
  }), _vm._v(" "), _c('b-input-group-append', [_c('b-input-group-text', [_c('i', {
    staticClass: "fa fa-user"
  })])], 1)], 1)], 1), _vm._v(" "), _c('b-form-group', [_c('b-input-group', [_c('b-form-input', {
    attrs: {
      "type": "email",
      "placeholder": "Email",
      "autocomplete": "email"
    }
  }), _vm._v(" "), _c('b-input-group-append', [_c('b-input-group-text', [_c('i', {
    staticClass: "fa fa-envelope"
  })])], 1)], 1)], 1), _vm._v(" "), _c('b-form-group', [_c('b-input-group', [_c('b-form-input', {
    attrs: {
      "type": "password",
      "placeholder": "Password",
      "autocomplete": "current-password"
    }
  }), _vm._v(" "), _c('b-input-group-append', [_c('b-input-group-text', [_c('i', {
    staticClass: "fa fa-asterisk"
  })])], 1)], 1)], 1), _vm._v(" "), _c('div', {
    staticClass: "form-group form-actions"
  }, [_c('b-button', {
    staticClass: "btn btn-sm btn-secondary",
    attrs: {
      "type": "submit"
    }
  }, [_vm._v("Submit")])], 1)], 1)], 1)], 1), _vm._v(" "), _c('b-col', {
    attrs: {
      "md": "4"
    }
  }, [_c('b-card', [_c('div', {
    attrs: {
      "slot": "header"
    },
    slot: "header"
  }, [_vm._v("\n          Example Form\n        ")]), _vm._v(" "), _c('b-form', [_c('b-form-group', [_c('b-input-group', [_c('b-input-group-prepend', [_c('b-input-group-text', [_c('i', {
    staticClass: "fa fa-user"
  })])], 1), _vm._v(" "), _c('b-form-input', {
    attrs: {
      "type": "text",
      "placeholder": "Username"
    }
  })], 1)], 1), _vm._v(" "), _c('b-form-group', [_c('b-input-group', [_c('b-input-group-prepend', [_c('b-input-group-text', [_c('i', {
    staticClass: "fa fa-envelope"
  })])], 1), _vm._v(" "), _c('b-form-input', {
    attrs: {
      "type": "email",
      "placeholder": "Email",
      "autocomplete": "email"
    }
  })], 1)], 1), _vm._v(" "), _c('b-form-group', [_c('b-input-group', [_c('b-input-group-prepend', [_c('b-input-group-text', [_c('i', {
    staticClass: "fa fa-asterisk"
  })])], 1), _vm._v(" "), _c('b-form-input', {
    attrs: {
      "type": "password",
      "placeholder": "Password",
      "autocomplete": "current-password"
    }
  })], 1)], 1), _vm._v(" "), _c('div', {
    staticClass: "form-group form-actions"
  }, [_c('b-button', {
    attrs: {
      "type": "submit",
      "size": "sm",
      "variant": "success"
    }
  }, [_vm._v("Submit")])], 1)], 1)], 1)], 1)], 1), _vm._v(" "), _c('b-row', [_c('b-col', {
    attrs: {
      "lg": "12"
    }
  }, [_c('transition', {
    attrs: {
      "name": "fade"
    }
  }, [(_vm.show) ? _c('b-card', {
    attrs: {
      "no-body": ""
    }
  }, [_c('div', {
    attrs: {
      "slot": "header"
    },
    slot: "header"
  }, [_c('i', {
    staticClass: "fa fa-edit"
  }), _vm._v(" Form Elements\n            "), _c('div', {
    staticClass: "card-header-actions"
  }, [_c('b-link', {
    staticClass: "card-header-action btn-setting",
    attrs: {
      "href": "#"
    }
  }, [_c('i', {
    staticClass: "icon-settings"
  })]), _vm._v(" "), _c('b-link', {
    directives: [{
      name: "b-toggle",
      rawName: "v-b-toggle.collapse1",
      modifiers: {
        "collapse1": true
      }
    }],
    staticClass: "card-header-action btn-minimize"
  }, [_c('i', {
    staticClass: "icon-arrow-up"
  })]), _vm._v(" "), _c('b-link', {
    staticClass: "card-header-action btn-close",
    attrs: {
      "href": "#"
    },
    on: {
      "click": function($event) {
        _vm.show = !_vm.show
      }
    }
  }, [_c('i', {
    staticClass: "icon-close"
  })])], 1)]), _vm._v(" "), _c('b-collapse', {
    attrs: {
      "id": "collapse1",
      "visible": ""
    }
  }, [_c('b-card-body', [_c('b-form-group', {
    attrs: {
      "label": "Prepended text",
      "label-for": "elementsEmail",
      "description": "Here's some help text"
    }
  }, [_c('b-input-group', [_c('b-input-group-prepend', [_c('b-input-group-text', [_vm._v("@")])], 1), _vm._v(" "), _c('b-form-input', {
    attrs: {
      "id": "elementsEmail",
      "type": "email",
      "autocomplete": "email"
    }
  })], 1)], 1), _vm._v(" "), _c('b-form-group', {
    attrs: {
      "label": "Appended text",
      "label-for": "elementsAppend",
      "description": "Here's some help text"
    }
  }, [_c('b-input-group', [_c('b-form-input', {
    attrs: {
      "id": "elementsAppend",
      "type": "text"
    }
  }), _vm._v(" "), _c('b-input-group-append', [_c('b-input-group-text', [_vm._v(".00")])], 1)], 1)], 1), _vm._v(" "), _c('b-form-group', {
    attrs: {
      "label": "Append and prepend",
      "label-for": "elementsPrependAppend",
      "description": "Here's some help text"
    }
  }, [_c('b-input-group', [_c('b-input-group-prepend', [_c('b-input-group-text', [_vm._v("$")])], 1), _vm._v(" "), _c('b-form-input', {
    attrs: {
      "id": "elementsPrependAppend",
      "type": "text"
    }
  }), _vm._v(" "), _c('b-input-group-append', [_c('b-input-group-text', [_vm._v(".00")])], 1)], 1)], 1), _vm._v(" "), _c('b-form-group', {
    attrs: {
      "label": "Append with button",
      "label-for": "elementsAppendButton",
      "description": "Here's some help text"
    }
  }, [_c('b-input-group', [_c('b-form-input', {
    attrs: {
      "id": "elementsAppendButton",
      "type": "text"
    }
  }), _vm._v(" "), _c('b-input-group-append', [_c('b-button', {
    attrs: {
      "variant": "primary"
    }
  }, [_vm._v("Go!")])], 1)], 1)], 1), _vm._v(" "), _c('b-form-group', {
    attrs: {
      "label": "Two-buttons append",
      "label-for": "elementsTwoButtons"
    }
  }, [_c('b-input-group', [_c('b-form-input', {
    attrs: {
      "id": "elementsTwoButtons",
      "type": "text"
    }
  }), _vm._v(" "), _c('b-input-group-append', [_c('b-button', {
    attrs: {
      "variant": "primary"
    }
  }, [_vm._v("Search")]), _vm._v(" "), _c('b-button', {
    attrs: {
      "variant": "danger"
    }
  }, [_vm._v("Options")])], 1)], 1)], 1), _vm._v(" "), _c('div', {
    staticClass: "form-actions"
  }, [_c('b-button', {
    attrs: {
      "type": "submit",
      "variant": "primary"
    }
  }, [_vm._v("Save changes")]), _vm._v(" "), _c('b-button', {
    attrs: {
      "type": "button",
      "variant": "secondary"
    }
  }, [_vm._v("Cancel")])], 1)], 1)], 1)], 1) : _vm._e()], 1)], 1)], 1)], 1)
},staticRenderFns: []}
module.exports.render._withStripped = true
if (true) {
  module.hot.accept()
  if (module.hot.data) {
     __webpack_require__(178).rerender("data-v-1e2c7174", module.exports)
  }
}

/***/ }),

/***/ 1602:
/***/ (function(module, exports, __webpack_require__) {

// style-loader: Adds some css to the DOM by adding a <style> tag

// load the styles
var content = __webpack_require__(1093);
if(typeof content === 'string') content = [[module.i, content, '']];
if(content.locals) module.exports = content.locals;
// add the styles to the DOM
var update = __webpack_require__(798)("51ec36e6", content, false);
// Hot Module Replacement
if(true) {
 // When the styles change, update the <style> tags
 if(!content.locals) {
   module.hot.accept(1093, function() {
     var newContent = __webpack_require__(1093);
     if(typeof newContent === 'string') newContent = [[module.i, newContent, '']];
     update(newContent);
   });
 }
 // When the module is disposed, remove the <style> tags
 module.hot.dispose(function() { update(); });
}

/***/ }),

/***/ 725:
/***/ (function(module, exports, __webpack_require__) {


/* styles */
__webpack_require__(1602)

var Component = __webpack_require__(374)(
  /* script */
  __webpack_require__(1195),
  /* template */
  __webpack_require__(1525),
  /* scopeId */
  "data-v-1e2c7174",
  /* cssModules */
  null
)
Component.options.__file = "C:\\WORKING\\Joytime\\dotnet-platform-backend\\Web\\Platform\\CMS\\PlatformCMS\\ClientApp\\pages\\Forms.vue"
if (Component.esModule && Object.keys(Component.esModule).some(function (key) {return key !== "default" && key !== "__esModule"})) {console.error("named exports are not supported in *.vue files.")}
if (Component.options.functional) {console.error("[vue-loader] Forms.vue: functional components are not supported with templates, they should use render functions.")}

/* hot reload */
if (true) {(function () {
  var hotAPI = __webpack_require__(178)
  hotAPI.install(__webpack_require__(26), false)
  if (!hotAPI.compatible) return
  module.hot.accept()
  if (!module.hot.data) {
    hotAPI.createRecord("data-v-1e2c7174", Component.options)
  } else {
    hotAPI.reload("data-v-1e2c7174", Component.options)
  }
})()}

module.exports = Component.exports


/***/ }),

/***/ 798:
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

var listToStyles = __webpack_require__(807)

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

/***/ 807:
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
//# sourceMappingURL=60.js.map