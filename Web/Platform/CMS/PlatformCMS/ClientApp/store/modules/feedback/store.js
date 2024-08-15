import actions from './actions'
const _feedbackstore = {
    state: {
        feedbacks: [{}],
        feedback: '',
    },
    mutations: {
        GET_FEEDBACKS: (state, payload) => {
            state.feedbacks = payload;
        }
    },
    actions,// cal action
    getters: {
        feedbacks: state => state.feedbacks,
        feedback: state => state.feedback,

    }


}
export default _feedbackstore;
