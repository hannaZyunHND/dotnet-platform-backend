import HttpService from '../../../plugins/http';

const getNotifications = ({ commit }, { pageIndex = 1, pageSize = 10, filter = '' }) => {
    return HttpService.get(`/api/G_Notifications/GetNotifications?pageIndex=${pageIndex}&pageSize=${pageSize}&filter=${encodeURIComponent(filter)}`)
        .then(response => {
            commit('SET_GNOTIFICATIONS', response.data);
            return response.data;
        })
        .catch(e => {
            alert('Error: ' + e);
        });
};
const createNotification = ({ commit }, data) => {
    return HttpService.post('/api/G_Notifications/PostNotification', data)
        .then(response => response.data)
        .catch(e => alert('Error: ' + e));
};

const updateNotification = ({ commit }, data) => {
    return HttpService.put(`/api/G_Notifications/PutNotification/${data.id}`, data)
        .then(response => response.data)
        .catch(e => alert('Error: ' + e));
};

const deleteNotification = ({ commit }, id) => {
    return HttpService.delete(`/api/G_Notifications/Delete/${id}`)
        .then(response => response.data)
        .catch(e => alert('Error: ' + e));
};
const getNotificationById = ({ commit }, id) => {
  return HttpService.get(`/api/G_Notifications/GetNotificationById?id=${id}`)
    .then((res) => res.data)
    .catch((err) => {
      console.error("Lỗi khi load Notification:", err);
      throw err;
    });
};
export default {
    getNotifications,
    createNotification,
    updateNotification,
    deleteNotification,
    getNotificationById
};
