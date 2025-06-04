import actions from "./actions";

const _store = {
  state: {
    customerGroups: {
      total: 0,
      items: []
    }
  },
  mutations: {
    SET_CUSTOMER_GROUPS(state, payload) {
      state.customerGroups.total = payload.totalItems;
      state.customerGroups.items = payload.items;
    }
  },
  actions,
  getters: {
    customerGroups: (state) => state.customerGroups
  }
};

export default _store;
