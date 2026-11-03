(function () {
    'use strict';
    angular.module('pmsTeamDashboardApp').factory('TeamDashboardService', ['$http', '$q', function ($http, $q) {
        return {load: function (filters, cancellation) {
            return $http.post('/pms/teamdashboard/default.aspx/GetTeamDashboardData', filters, {timeout: cancellation}).then(function (response) {
                var data = response.data.d;
                return typeof data === 'string' ? JSON.parse(data) : data;
            }, function (response) {
                if (response.status === -1) return $q.reject({cancelled: true});
                if (window.console) console.error('Team dashboard request failed', response.status, response.data);
                var detail = response.data;
                if (typeof detail === 'string') {
                    try { detail = JSON.parse(detail); } catch (ignored) { detail = null; }
                }
                var message = detail && typeof detail.Message === 'string' ? detail.Message.slice(0, 700) : '';
                var fallback = response.status === 401 || response.status === 403 ? 'Please sign in with a permitted employee account.' : 'Could not load team data. Open F12 > Network > GetTeamDashboardData > Response for the server error.';
                return $q.reject(new Error(message ? 'Team data error (HTTP ' + response.status + '): ' + message : fallback + ' HTTP ' + response.status));
            });
        }};
    }]);
}());
