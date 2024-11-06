import HttpService from '../../../plugins/http'
import { config } from 'vue-test-utils';

const GetAllEmailSubcribe = ({ commit }) => {
    return HttpService.get(`/api/Orders/GetAllEmailSubcribe`).then(response => {
        return response.data;
    }).catch(e => {
        alert('ex found:' + e)
    })
}



export default {
    GetAllEmailSubcribe

}
