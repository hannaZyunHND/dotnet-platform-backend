import HttpService from '../../../plugins/http'
import { config } from 'vue-test-utils';

const getOrderFeedbacks = ({ commit }) => {
    return HttpService.get(`/api/Orders/GetListOrderFeedback`).then(response => {
        return response.data;
    }).catch(e => {
        alert('ex found:' + e)
    })
}
const updateOrderFeedback = ({ commit }, data) => {
    return HttpService.post(`/api/Orders/UpdateReviewFeedback`, data)
        .then(response => {
            return response.data;
        })
        .catch(e => {
            alert('ex found:' + e)
        })
}


export default {
    getOrderFeedbacks,
    updateOrderFeedback,
}
