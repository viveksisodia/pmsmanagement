/* miles.main.js
   AngularJS controller for editmile.aspx.
   This file intentionally keeps WebForms server controls and fills them by ClientID.
*/
(function () {
    'use strict';

    angular.module('PMSerp').controller('milesController', ['$scope', '$timeout', 'milesService', function ($scope, $timeout, milesService) {
        var ids = window.milesClientIds || {};

        function el(key) {
            var id = ids[key] || key;
            return document.getElementById(id);
        }

        function getValue(key) {
            var x = el(key);
            return x ? x.value : '';
        }

        function setValue(key, value) {
            var x = el(key);
            if (x) x.value = value == null ? '' : value;
        }

        function setChecked(key, value) {
            var x = el(key);
            if (x) x.checked = String(value) === '1' || value === true;
        }

        function getCheckedValue(key) {
            var x = el(key);
            return x && x.checked ? 1 : 0;
        }

        function setSelect2Value(key, value) {
            setValue(key, value);
            if (window.jQuery) {
                var arr = [];
                if (value != null && String(value).length > 0) {
                    arr = String(value).split(',').filter(function (v) { return v !== ''; });
                }
                try { window.jQuery(el(key)).val(arr).trigger('change'); } catch (e) { }
            }
        }

        function showLoading(isShow) {
            var p = document.getElementById('pnlimgmile');
            var b = document.getElementById('divsubmile');
            if (p) p.style.display = isShow ? 'block' : 'none';
            if (b) b.style.display = isShow ? 'none' : 'block';
        }

        function xmlToTable(xmlText) {
            if (!xmlText) return {};
            var parser = new DOMParser();
            var xmlDoc = parser.parseFromString(xmlText, 'text/xml');
            var table = xmlDoc.getElementsByTagName('Table')[0];
            var obj = {};
            if (!table) return obj;

            for (var i = 0; i < table.childNodes.length; i++) {
                var node = table.childNodes[i];
                if (node.nodeType === 1) {
                    obj[node.nodeName] = node.textContent || '';
                }
            }
            return obj;
        }

        function bindPreviewFiles(uploadData) {
            var previewContainer = document.getElementById('previewContainer1');
            if (!previewContainer) return;
            previewContainer.innerHTML = '';

            if (!uploadData || uploadData === '0') return;
            var basePath = '/pms/MileImages/';
            uploadData.split('^').forEach(function (file) {
                file = (file || '').trim();
                if (!file) return;

                var filePath = basePath + file;
                var fileName = file.split('/').pop();
                var ext = fileName.split('.').pop().toLowerCase();
                var link = document.createElement('a');
                link.href = filePath;
                link.target = '_blank';
                link.title = fileName;
                link.style.display = 'inline-block';
                link.style.margin = '5px';

                if (['jpg', 'jpeg', 'png', 'gif', 'bmp'].indexOf(ext) >= 0) {
                    var img = document.createElement('img');
                    img.src = filePath;
                    img.alt = fileName;
                    img.className = 'preview-item';
                    img.style.maxWidth = '100px';
                    link.appendChild(img);
                } else if (ext === 'pdf') {
                    link.textContent = '📄';
                    link.style.fontSize = '40px';
                    link.style.textDecoration = 'none';
                } else {
                    link.textContent = fileName;
                }
                previewContainer.appendChild(link);
            });
        }

        $scope.init = function (options) {
            options = options || {};
            $scope.recordId = options.id || '0';
            $scope.projectId = options.pid || '0';

            if ($scope.recordId && $scope.recordId !== '0' && $scope.recordId !== '') {
                $scope.bindMilestone($scope.recordId);
            }
        };

        $scope.bindMilestone = function (recordId) {
            showLoading(true);
            milesService.bindMilestoneDetail(recordId).then(function (result) {
                var m = xmlToTable(result);
                if (!m || !m.id) return;

                setValue('hdnidmile', m.id);
                setValue('hdiprojidmile', m.ProjectId);
                setValue('ProjectIdmile', m.ProjectId);
                setValue('hdnProjectIdmile', m.ProjectId);
                setValue('Milestone', m.Milestone);
                setValue('Weightagemile', m.Weightage);
                setValue('StartDatemile', m.StartDate);
                setValue('hdnStartDatemile', m.StartDate);
                setValue('EndDatemile', m.EndDate);
                setValue('hdnEndDatemile', m.EndDate);
                setValue('Accessmile', m.Access);
                setValue('hdnAccessmile', m.Access);
                setValue('Budgetmile', m.Budget);
                setValue('Actualmile', m.Actual);
                setValue('P_Statusmile', m.P_Status);
                setValue('hdnP_Statusmile', m.P_Status);
                setValue('Prioritymile', m.Priority);
                setValue('hdnPrioritymile', m.Priority);
                setValue('hdnownermile', m.Owner);
                setValue('hdnteammile', m.team);
                setValue('ddldurationtype', m.Durationtype);
                setValue('hdndurationtype', m.Durationtype);
                setValue('taskDurationmile', m.TaskDuration);
                setChecked('istask', m.istask);
                setChecked('checkmileWeightage', m.checkmileWeightage);

                $timeout(function () {
                    setSelect2Value('Ownermile', m.Owner);
                    setSelect2Value('teammile', m.team);
                }, 0);

                bindPreviewFiles(m.Uploaddrawing);
            }).catch(function (err) {
                alert((err && err.data && err.data.Message) || (err && err.statusText) || 'Milestone detail not loaded.');
            }).finally(function () {
                showLoading(false);
            });
        };

        $scope.validateMilestone = function () {
            if (!getValue('Milestone')) {
                alert('Please enter milestone.');
                var mile = el('Milestone'); if (mile) mile.focus();
                return false;
            }
            var sd = getValue('StartDatemile');
            var ed = getValue('EndDatemile');
            if (sd && ed && new Date(sd) > new Date(ed)) {
                alert('End date should not be less than start date.');
                var end = el('EndDatemile'); if (end) end.focus();
                return false;
            }
            return true;
        };

        $scope.collectMilestone = function () {
            return {
                id: getValue('hdnidmile') || '0',
                ProjectId: getValue('ProjectIdmile') || getValue('hdnProjectIdmile'),
                Milestone: getValue('Milestone'),
                Weightage: getValue('Weightagemile'),
                StartDate: getValue('StartDatemile'),
                EndDate: getValue('EndDatemile'),
                Owner: getValue('hdnownermile'),
                Access: getValue('Accessmile'),
                team: getValue('hdnteammile'),
                P_Status: getValue('P_Statusmile'),
                Priority: getValue('Prioritymile'),
                Budget: getValue('Budgetmile'),
                Actual: getValue('Actualmile'),
                istask: getCheckedValue('istask'),
                checkmileWeightage: getCheckedValue('checkmileWeightage'),
                Durationtype: getValue('ddldurationtype'),
                TaskDuration: getValue('taskDurationmile')
            };
        };

        $scope.saveMilestone = function (mode) {
            if (!$scope.validateMilestone()) return false;
            showLoading(true);
            var model = $scope.collectMilestone();

            milesService.saveMilestone(model).then(function (saveId) {
                if (typeof window.UploadFiles === 'function') {
                    try { window.UploadFiles(saveId); } catch (e) { }
                }
                alert('Data Saved successfully.');

                if (mode === 'savemore') {
                    setValue('hdnidmile', '0');
                    setValue('Milestone', '');
                    return;
                }

                try {
                    if (window.parent && window.parent.document) {
                        var hdn = window.parent.document.getElementById('hdneditid');
                        if (hdn) hdn.value = '1';
                        var btn = window.parent.document.getElementById('btncloseedit');
                        if (btn) btn.click();
                    }
                } catch (e) { }
            }).catch(function (err) {
                alert((err && err.data && err.data.Message) || (err && err.statusText) || 'Milestone not saved.');
            }).finally(function () {
                showLoading(false);
            });
            return false;
        };
    }]);
})();
