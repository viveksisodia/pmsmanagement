/* miles.service.js
   Service layer for milestone edit page.
   It uses existing WebForms WebMethods from MultiProjectTask.aspx.
*/
(function () {
    'use strict';

    angular.module('PMSerp').service('milesService', ['$http', function ($http) {
        function post(url, data) {
            return $http({
                method: 'POST',
                url: url,
                data: data || {},
                headers: { 'Content-Type': 'application/json; charset=utf-8' }
            }).then(function (res) {
                return res.data ? res.data.d : null;
            });
        }

        this.bindMilestoneDetail = function (recordId) {
            return post('/pms/MultiProjectTask.aspx/Bindmilestonedetail', {
                RecordId: String(recordId || '0')
            });
        };

        this.saveMilestone = function (milestoneDetails) {
            return post('/pms/MultiProjectTask.aspx/SaveMilestone', {
                milestonedetails: milestoneDetails
            });
        };

        this.addMilestoneStatus = function (statusName, color) {
            return post('/pms/MultiProjectTask.aspx/AddAddMilestonetatus', {
                statusName: statusName || '',
                color: color || ''
            });
        };

        this.addMilestonePriority = function (priorityName, color) {
            return post('/pms/MultiProjectTask.aspx/AddMilestonePrioritytype', {
                priorityName: priorityName || '',
                color: color || ''
            });
        };
    }]);
})();
