import Vue from 'vue';
import JsTreeList from '../js/js-tree-list.js';


//let _config = require('/../../appsettings.json');
//let BASE_URL = _config.AppSettings.BaseDomain;;
//let BASE_URLDoamin = _config.AppSettings.Domain;;

export function unflatten(arr) {

    //var tree = [],
    //    mappedArr = {},
    //    arrElem,
    //    mappedElem;

    //// First map the nodes of the array to an object -> create a hash table.
    //for (var i = 0, len = arr.length; i < len; i++) {
    //    arrElem = arr[i];
    //    mappedArr[arrElem.id] = arrElem;
    //    mappedArr[arrElem.id]['children'] = [];
    //}
    //for (var id in mappedArr) {
    //    if (mappedArr.hasOwnProperty(id)) {
    //        mappedElem = mappedArr[id];
    //        // If the element is not at the root level, add it to its parent array of children.
    //        if (mappedElem.parentId) {
    //            try {

    //                mappedArr[mappedElem['parentId']]['children'].push(mappedElem);
    //            }
    //            catch (ex) {
    //                console.log(ex);
    //            }
    //        }
    //        // If the element is at the root level, add it to first level elements array.
    //        else {
    //            tree.push(mappedElem);
    //        }
    //    }
    //}
    //console.log(tree);

    var ltt = new JsTreeList.ListToTree(arr, {
        key_id: 'id',
        key_parent: 'parentId',
        key_child: "children"
    })

    var tree = ltt.GetTree()

    return tree;
};

export function unflatten2(arr) {
    //var ltt = new JsTreeList.ListToTree(arr, {
    //    key_id: 'id',
    //    key_parent: 'parentId',
    //    key_child: "_children"
    //})

    var tree = unflatten(arr);
    return tree;
};

function unflatten(arr) {
    var tree = [],
        mappedArr = {},
        arrElem,
        mappedElem;
    // First map the nodes of the array to an object -> create a hash table.
    for (var i = 0, len = arr.length; i < len; i++) {
        arrElem = arr[i];
        mappedArr[arrElem.id] = arrElem;
        mappedArr[arrElem.id]['children'] = [];
    }
    for (var id in mappedArr) {
        if (mappedArr.hasOwnProperty(id)) {
            mappedElem = mappedArr[id];
            // If the element is not at the root level, add it to its parent array of children.
            if (mappedElem.parentId) {
                if (mappedElem.children) {
                    try {
                        mappedArr[mappedElem['parentId']]['children'].push(mappedElem);
                    }
                    catch (ex) {
                        console.log(ex);
                    }
                }

            }
            // If the element is at the root level, add it to first level elements array.
            else {
                tree.push(mappedElem);
            }
        }
    }
    return tree;
};




export function slug(str) {
    str = str.replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/g, "a");
    str = str.replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g, "e");
    str = str.replace(/ì|í|ị|ỉ|ĩ/g, "i");
    str = str.replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g, "o");
    str = str.replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g, "u");
    str = str.replace(/ỳ|ý|ỵ|ỷ|ỹ/g, "y");
    str = str.replace(/đ/g, "d");
    str = str.replace(/À|Á|Ạ|Ả|Ã|Â|Ầ|Ấ|Ậ|Ẩ|Ẫ|Ă|Ằ|Ắ|Ặ|Ẳ|Ẵ/g, "A");
    str = str.replace(/È|É|Ẹ|Ẻ|Ẽ|Ê|Ề|Ế|Ệ|Ể|Ễ/g, "E");
    str = str.replace(/Ì|Í|Ị|Ỉ|Ĩ/g, "I");
    str = str.replace(/Ò|Ó|Ọ|Ỏ|Õ|Ô|Ồ|Ố|Ộ|Ổ|Ỗ|Ơ|Ờ|Ớ|Ợ|Ở|Ỡ/g, "O");
    str = str.replace(/Ù|Ú|Ụ|Ủ|Ũ|Ư|Ừ|Ứ|Ự|Ử|Ữ/g, "U");
    str = str.replace(/Ỳ|Ý|Ỵ|Ỷ|Ỹ/g, "Y");
    str = str.replace(/Đ/g, "D");
    // Some system encode vietnamese combining accent as individual utf-8 characters
    // Một vài bộ encode coi các dấu mũ, dấu chữ như một kí tự riêng biệt nên thêm hai dòng này
    str = str.replace(/\u0300|\u0301|\u0303|\u0309|\u0323/g, ""); // ̀ ́ ̃ ̉ ̣  huyền, sắc, ngã, hỏi, nặng
    str = str.replace(/\u02C6|\u0306|\u031B/g, ""); // ˆ ̆ ̛  Â, Ê, Ă, Ơ, Ư
    // Remove extra spaces
    // Bỏ các khoảng trắng liền nhau
    str = str.replace(/ + /g, " ");
    str = str.trim();
    // Remove punctuations
    // Bỏ dấu câu, kí tự đặc biệt
    str = str.replace(/!|@|%|\^|\*|\(|\)|\+|\=|\<|\>|\?|\/|,|\.|\:|\;|\'|\"|\&|\#|\[|\]|~|\$|_|`|-|{|}|\||\\/g, " ");
    str = str.replace(/\s+/g, '-');
    return str;
};
// Change whitespace to "-"



export function pathImg(title) {
    let config = require('./../../appsettings.json');
    let cloudFontCDN = config.AppSettings.CloudFontCDN;
    if (title != null && title != undefined && title.length > 0) {
        title = "https://cms.joytime.vn/" + "uploads/thumb" + title;
    }
    return title;

};

export function urlBase(title) {
    if (title != null && title != undefined && title.length > 0) {
        title = "https://cms.joytime.vn/" + title + ".html";
    }
    return title;

};

