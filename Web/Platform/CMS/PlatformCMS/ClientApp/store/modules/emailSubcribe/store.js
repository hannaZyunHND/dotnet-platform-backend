import actions from './actions'
const _emailstore = {
    state: {
        emails: [{}],
        email: '',
    },
    mutations: {
        GET_EMAILS: (state, payload) => {
            state.emails = payload;
        }
    },
    actions,// cal action
    getters: {
        emails: state => state.emails,
        email: state => state.email,

    }


}
export default _emailstore;
