(function () {
    'use strict';

    angular.module('pmsUserReport.services', [])
        .factory('UserReportService', ['$http', function ($http) {
            function unwrap(response) {
                var data = response && response.data ? response.data : {};
                data = data.d !== undefined ? data.d : data;
                if (typeof data === 'string') data = JSON.parse(data);
                return data || {};
            }
            function localDateRequestConfig() {
                return { headers: { 'X-Pms-UtcOffsetMinutes': String(new Date().getTimezoneOffset()) } };
            }
            return {
                getGroupedReport: function (filters) {
                    return $http.post('default.aspx/GetGroupedReportData', filters, localDateRequestConfig()).then(unwrap);
                },
                getReport: function (filters) {
                    return $http.post('default.aspx/GetReportData', filters, localDateRequestConfig()).then(unwrap);
                }
            };
        }]);
}());
