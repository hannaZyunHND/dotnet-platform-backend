import actions from './actions'

const _store = {
    state: {
        surveys: [],

    },
    mutations: {
        SET_LOADING: (state, payload) => {
            
        },
        GET_CONTACTS: (state, payload) => {
            state.surveys = payload;
        }
    },
    actions,
    getters: {
        surveys: state => state.surveys,
    }
}
export default _store;
