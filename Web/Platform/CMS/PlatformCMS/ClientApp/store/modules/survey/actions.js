import HttpService from '../../../plugins/http'

const getAllSurvey = async ({commit}, data) => {
    const response = await HttpService.post(`/api/survey/Get_All_Survey`,data).catch(e => {
        alert('ex found:' + e)
    });
    return response.data;
};

const getSurveyById = async ({commit}, id) => {
    const response = await HttpService.get(`/api/survey/Get_Survey_By_Id/${id}`).catch(e => {
        alert('ex found:' + e)
    });
    return response.data;
};

const getAllOptions = async ({commit}, id) => {
    const response = await HttpService.get(`/api/survey/Get_All_Options/${id}`).catch(e => {
        alert('ex found:' + e)
    });
    return response.data;
};

const getQuestionById = async ({commit}, id) => {
    const response = await HttpService.get(`/api/survey/Get_Question_By_Id/${id}`).catch(e => {
        alert('ex found:' + e)
    });
    return response.data;
};

export default {
    getAllSurvey,
    getSurveyById,
    getAllOptions,
    getQuestionById
}
