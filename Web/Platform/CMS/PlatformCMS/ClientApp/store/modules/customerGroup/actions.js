import HttpService from "../../../plugins/http";

const getCustomerGroups = ({ commit }, params) => {
  return HttpService.get("/api/CustomerGroups/GetAll", { params })
    .then((response) => {
      commit("SET_CUSTOMER_GROUPS", response.data);
      return response.data;

    })
    .catch((e) => {
      alert("Lỗi khi tải nhóm khách hàng: " + e);
    });
};
const createCustomerGroup = ({ commit }, data) => {
  return HttpService.post(`/api/CustomerGroups/Create`, data);
};

const updateCustomerGroup = ({ commit }, data) => {
  return HttpService.put(`/api/CustomerGroups/Update/${data.id}`, data);
};

const searchCustomers = ({ commit }, data) => {
  return HttpService.post(`/api/CustomerGroups/SearchCustomer`, data).then(res => res.data);
};

const getSelectedCustomerInCustomerGroupById = ({ commit }, id) => {
  return HttpService.get(`/api/CustomerGroups/GetCustomerSelectedById/${id}`).then(res => res.data);
};

export default {
  getCustomerGroups,
  createCustomerGroup,
  updateCustomerGroup,
  searchCustomers,
  getSelectedCustomerInCustomerGroupById
};