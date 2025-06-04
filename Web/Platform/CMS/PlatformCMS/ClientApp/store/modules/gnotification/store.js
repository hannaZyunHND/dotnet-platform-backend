import actions from "./actions";

const _store = {
    state: {
        gnotification: {
            total: 0,
            items: []
        }
    },
    mutations: {
        SET_GNOTIFICATIONS(state, payload) {
            state.gnotification.total = payload.totalItems;
            state.gnotification.items = payload.items;
        }
    },
    actions,
    getters: {
        gnotification: (state) => state.gnotification
    }
};

export default _store;
