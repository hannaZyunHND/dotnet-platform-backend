import actions from './actions'

const _store = {
    state: {
        subscribers: [],

    },
    mutations: {
        SET_LOADING: (state, payload) => {

        },
        GET_CONTACTS: (state, payload) => {
            state.subscribers = payload;
        }
    },
    actions,
    getters: {
        subscribers: state => state.subscribers,
    }
}
export default _store;
