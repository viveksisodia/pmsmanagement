(function () {
    'use strict';

    angular.module('pmsUserDashboard.services', [])
        .factory('UserDashboardService', ['$http', '$q', function ($http, $q) {
            var apiUrl = 'default.aspx/GetUserDashboardData';

            return {
                getDashboard: function (fromDate, toDate, projectId) {
                    return $http.post(apiUrl, { fromDate: fromDate, toDate: toDate, projectId: projectId || "" }).then(function (response) {
                        var data = response.data && response.data.d ? response.data.d : response.data;
                        if (typeof data === 'string') data = JSON.parse(data);
                        return data;
                    });
                },
                getTaskHours: function (fromDate, toDate, projectId) {
                    return $http.post('default.aspx/GetTaskHoursData', { fromDate: fromDate, toDate: toDate, projectId: projectId || "" }).then(function (response) {
                        var data = response.data && response.data.d ? response.data.d : response.data;
                        if (typeof data === 'string') data = JSON.parse(data);
                        return data;
                    });
                },
                getWeeklyReport: function (weekStart, weekEnd) {
                    return $q.all([
                        $http.post('/pms/users/default.aspx/GetWeeklyReportData', { weekStart: weekStart, weekEnd: weekEnd }),
                        $http.post('default.aspx/GetTaskHoursLeaveData', { fromDate: weekStart, toDate: weekEnd })
                    ]).then(function (responses) {
                        function decode(response) {
                            var data = response.data && response.data.d ? response.data.d : response.data;
                            return typeof data === 'string' ? JSON.parse(data) : data;
                        }
                        var report = decode(responses[0]);
                        report.leaveData = decode(responses[1]);
                        return report;
                    });
                }
            };
        }]);
}());
