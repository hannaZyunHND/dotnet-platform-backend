import HttpService from '../../../plugins/http'
import { config } from 'vue-test-utils';

const getProducts = ({ commit }, data) => {
    return HttpService.post(`/api/Product/Search`, data).then(response => {
        commit("GET_PRODUCTS", { ...response.data })
    }).catch(e => {
        alert('ex found:' + e)
    })
}



const addPromotionWithProduct = ({ commit }, data) => {
    return HttpService.post(`/api/Promotion/CreatePromotionWithProduct`, data).then(response => {
        return response.data
    }).catch(e => {
        alert('ex found:' + e)
    })
}


const getProduct = ({ commit }, id) => {
    return HttpService.get(`/api/Product/GetById?id=${id}`, {
    }).then(response => {
        console.log(response.data);
        //console.log('ProductById'+id+response.data);
        return response.data;
    }).catch(e => {
        alert('ex found:' + e)
    })
}
const getListProductParent = ({ commit }) => {
    return HttpService.get(`/api/Product/GetListProductParent`, {}).then(response => {
        //console.log('ProductById'+id+response.data);
        return response.data;
    }).catch(e => {
        alert('ex found:' + e)
    })
}


const getNameProduct = ({ commit }, id) => {
    return HttpService.get(`/api/Product/GetById?id=${id}`, {
    }).then(response => {
        //console.log('ProductById'+id+response.data);
        return response.data;
    }).catch(e => {
        alert('ex found:' + e)
    })
}

const getPriceByLocation = ({ commit }, id) => {
    return HttpService.get(`/api/Product/GetPriceByLocation?idProduct=${id}`, {
    }).then(response => {
        //console.log('ProductById'+id+response.data);
        return response.data;
    }).catch(e => {
        alert('ex found:' + e)
    })
}

const getAllRegion = ({ commit }, id) => {
    return HttpService.get(`/api/ProductInRegion/GetAllRegion`, {
    }).then(response => {
        //console.log('ProductById'+id+response.data);
        return response.data;
    }).catch(e => {
        alert('ex found:' + e)
    })
}
const getProductByRegion = ({ commit }, data) => {

    return HttpService.get(`/api/ProductInRegion/getProductByRegion?zoneId=${data.zoneId}`, {
    }).then(response => {
        return response.data;
    }).catch(e => {
        alert('ex found:' + e)
    })
}

const getAllTypeMaterial = ({ commit }) => {

    return HttpService.get(`/api/Common/GetAllTypeMaterial`, {}).then(response => {
        return response.data;
    }).catch(e => {
        alert('ex found:' + e)
    })
}


const addProductByRegion = ({ commit }, params) => {
    return HttpService.post('/api/ProductInRegion/Add', params)
        .then(response => {
            return response.data;
        })
        .catch(e => {
            alert('ex found:' + e)
        })
}

const getPropertyProduct = ({ commit }) => {
    return HttpService.get('/api/Product/GetPropertyProduct')
        .then(response => {
            return response.data;
        })
        .catch(e => {
            alert('ex found:' + e)
        })
}

const getLanguageById = ({ commit }, id) => {
    return HttpService.get(`/api/Product/GetLanguageById?id=${id}`, {})
        .then(response => {
            return response.data;
        })
        .catch(e => {
            alert('ex found:' + e)
        })
}

const productUnitGet = ({ commit }, data) => {
    return HttpService.get('/api/Common/ProductUnitGet')
        .then(response => {
            return response.data;
        })
        .catch(e => {
            alert('ex found:' + e)
        })
}

const updateProduct = ({ commit }, data) => {
    return HttpService.put('/api/Product/Update', data)
        .then(response => {
            return response.data;
        })
        .catch(e => {
            alert('ex found:' + e)
        })
}
const getPhienBanInProduct = ({ commit }, id) => {
    return HttpService.get(`/api/Product/GetPhienBan?idProduct=${id}`, {
    }).then(response => {
        return response;
        //commit("GET_ADS", { ...response.data })
    }).catch(e => {
        alert('ex found:' + e)
    })
}

const getTopUpInProduct = ({ commit }, id) => {
    return HttpService.get(`/api/Product/GetTopUps?idProduct=${id}`, {
    }).then(response => {
        return response;
        //commit("GET_ADS", { ...response.data })
    }).catch(e => {
        alert('ex found:' + e)
    })
}

const getSerialNumbersByProductId = ({ commit }, id) => {
    return HttpService.get(`/api/Product/GetSerialNumbers?idProduct=${id}`).then(response => {
        return response.data;
    }).catch(e => {
        alert(`ex found: ${e}`);
    })
}
//GetTopUps

const addProduct = ({ commit }, params) => {
    return HttpService.post('/api/Product/Add', params)
        .then(response => {
            return response.data;
        })
        .catch(e => {
            alert('ex found:' + e)
        })
}

const deleteProduct = ({ commit }, data) => {
    return HttpService.put(`/api/Product/Delete?id=${data.id}&status=${data.status}`)
        .then(response => {
            return response.data;
        })
        .catch(e => {
            alert('ex found:' + e)
        })
}
const supportsProduct = ({ commit }, data) => {
    return HttpService.get('/api/Product/Supports')
        .then(response => {
            return response.data;
        })
        .catch(e => {
            alert('ex found:' + e)
        })
}
const exportPriceInLocation = ({ commit }, data) => {
    return HttpService.post('/api/ImportExport/ExportProductPriceLocation', data).then(response => {
        return response;
        //return response;
    }).catch(e => {
        alert('ex found:' + e)
    })
}
const exportSpectifications = ({ commit }, data) => {
    return HttpService.post('/api/ImportExport/ExportSpectification', data).then(response => {
        return response;
        //return response;
    }).catch(e => {
        alert('ex found:' + e)
    })
}

const updateSort = ({ commit }, data) => {
    return HttpService.put(`/api/Product/UpdateSort?id=${data.id}&sortNew=${data.sortNew}`, {}).then(response => {
        return response.data;
        //return response;
    }).catch(e => {
        alert('ex found:' + e)
    })
}

const getProductComponents = ({ commit }, data) => {
    return HttpService.post(`/api/Product/GetAllProductComponent`, data).then(response => {
        return response.data;
    }).catch(e => {
        alert('ex found:' + e)
    })
}

const getProductComponentById = ({ commit }, id) => {
    return HttpService.get(`/api/Product/GetProductComponentById?id=${id}`, {
    }).then(response => {
        //console.log('ProductById'+id+response.data);
        return response.data;
    }).catch(e => {
        alert('ex found:' + e)
    })
}
const deleteProductComponentById = ({ commit }, id) => {
    return HttpService.put(`/api/Product/DeleteProductComponentById?id=${id}`)
        .then(response => {
            return response.data;
        })
        .catch(e => {
            alert('ex found:' + e)
        })
}
const updateProductComponent = ({ commit }, data) => {
    return HttpService.put('/api/Product/UpdateProductComponent', data)
        .then(response => {
            return response.data;
        })
        .catch(e => {
            alert('ex found:' + e)
        })
}

const getAllCatProductComponent = ({ commit }) => {

    return HttpService.get(`/api/Product/GetAllCatProductComponent`, {}).then(response => {
        return response.data;
    }).catch(e => {
        alert('ex found:' + e)
    })
}
//GetGiaPhienBanTheoNgay/{zoneList}/{productId}
const getGiaPhienBanTheoNgay = ({commit}, data) => {
    return HttpService.get(`/api/Product/GetGiaPhienBanTheoNgay/${data.selectedOptions}/${data.productId}`).then(response => {
        return response.data;
    }).catch(e => {
        alert('ex found:' + e)
    })
}
//UpdateGiaPhienBanTheoNgay
const updateGiaPhienBanTheoNgay = ({commit}, data) => {
    return HttpService.post(`/api/Product/UpdateGiaPhienBanTheoNgay`, data).then(response => {
        return response.data;
    }).catch(e => {
        alert('ex found:' + e)
    })
}
const getCancelPolicies = ({commit}, id) => {
    return HttpService.get(`/api/Product/GetCancelPolicies?idProduct=${id}`, {
    }).then(response => {
        return response;
        //commit("GET_ADS", { ...response.data })
    }).catch(e => {
        alert('ex found:' + e)
    })
}
const exportAnalyzeProductWithDestinationAndService = ({commit}) => {
    return HttpService.post(`/api/Analyzes/AnalyzeProductByServiceAndDestination`, {
        responseType: 'blob' // Expect the response as a blob (binary data)
    }).then(response => {
        return response;
        //commit("GET_ADS", { ...response.data })
    }).catch(e => {
        alert('ex found:' + e)
    })
}
const exportAnalyzeFullProductDetail = ({ commit }) => {
    return HttpService.post(`/api/Analyzes/AnalyzeFullProductDetail`, {
        responseType: 'blob' // Expect the response as a blob (binary data)
    }).then(response => {
        return response;
        //commit("GET_ADS", { ...response.data })
    }).catch(e => {
        alert('ex found:' + e)
    })
}
export default {
    getProducts,
    getProduct,
    updateProduct,
    deleteProduct,
    addProduct,

    getPriceByLocation,
    productUnitGet,
    getAllRegion,
    getProductByRegion,
    addProductByRegion,
    supportsProduct,
    getPropertyProduct,
    getLanguageById,

    addPromotionWithProduct,
    getAllTypeMaterial,
    getNameProduct,
    exportPriceInLocation,
    exportSpectifications,
    updateSort,
    getListProductParent,
    getPhienBanInProduct,
    getProductComponents,
    getProductComponentById,
    updateProductComponent,
    deleteProductComponentById,
    getAllCatProductComponent,
    getTopUpInProduct,
    getSerialNumbersByProductId,
    getGiaPhienBanTheoNgay,
    updateGiaPhienBanTheoNgay,
    getCancelPolicies,
    exportAnalyzeProductWithDestinationAndService,
    exportAnalyzeFullProductDetail
}
