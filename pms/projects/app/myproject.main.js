app.controller("myprojectCtrl", function ($scope, $timeout, $http, myprojectService) {
$scope.onSearchInputChange = function() {
    // 1. Force the custom dropdown to open as you type
    $scope.showqcustomersDropdown = true;

    // 2. CRITICAL: If the user changes or clears the text field, 
    // sever the old selection ID so the filter unblocks itself.
    if ($scope.newproject.customerid) {
        $scope.newproject.customerid = null; // Clears the stuck ID
    }
    
    // 3. Optional safety fallback: If the input is completely empty, 
    // reset both values explicitly.
    if (!$scope.newproject.ClientName) {
        $scope.newproject.ClientName = '';
        $scope.newproject.customerid = null;
    }
};

    // Project-level completion dependencies and issues, shared by List and Tiles.
    $scope.completionDependency = { open: false };
    var completionDependencyRequest = 0;
    function completionDependencyPost(method, body) {
        return $http.post('/pms/AddSTDependency.aspx/' + method, body)
            .then(function (response) { return response.data.d; });
    }
    function completionOptions(rows) {
        return (rows || []).filter(function (option) { return option && Number(option.Value) > 0; });
    }
    function completionHierarchy(id, type, item) {
        var path = { milestone: type === 'milestone' ? id : (item.Milestoneid || item.MilestonId || item.MilestoneId || 0),
            task: type === 'task' ? id : (type === 'subtask' ? (item.Task || item.task || item.TaskId || 0) : 0),
            subtask: type === 'subtask' ? id : 0 };
        angular.forEach((($scope.projectlist || {}).miles || []), function (milestone) {
            if (type === 'milestone' && String(milestone.id) === String(id)) path.milestone = milestone.id;
            angular.forEach(milestone.tasks || [], function (task) {
                if (type === 'task' && String(task.id) === String(id)) path.milestone = milestone.id;
                angular.forEach(task.subtasks || [], function (subtask) {
                    if (type === 'subtask' && String(subtask.id) === String(id)) {
                        path.milestone = milestone.id; path.task = task.id;
                    }
                });
            });
        });
        return path;
    }
    function completionLoadSubtasks(state, seedId) {
        var ids = state.selected.task.map(function (row) { return row.Value; }).join(',');
        var request = state.subtaskLoadRequest = (state.subtaskLoadRequest || 0) + 1;
        state.loadingSubtasks = !!ids;
        // Keep existing choices while another task is added. Once the new list
        // arrives, retain only selections that still belong to selected tasks.
        if (!ids) { state.selected.subtask = []; state.subtasks = []; return; }
        completionDependencyPost('bindsubtasks', { mprojectid: String(state.projectId), tid: ids, stid: '0' })
            .then(function (rows) {
                if (!state.open || request !== state.subtaskLoadRequest ||
                    ids !== state.selected.task.map(function (row) { return row.Value; }).join(',')) return;
                var choices = completionOptions(rows).filter(function (row) {
                    return state.type !== 'subtask' || String(row.Value) !== String(state.id);
                });
                var retained = state.selected.subtask.map(function (row) { return String(row.Value); });
                var seedIds = state.editSeed ? state.editSeed.subtask :
                    (state.type === 'subtask' ? [] : [String(seedId || 0)]);
                state.loadingSubtasks = false;
                state.subtasks = choices;
                state.selected.subtask = choices.filter(function (row) {
                    return retained.indexOf(String(row.Value)) >= 0 || seedIds.indexOf(String(row.Value)) >= 0;
                });
            }, function () { if (state.open && request === state.subtaskLoadRequest) { state.loadingSubtasks = false; state.error = 'Unable to load subtasks.'; } });
    }
    function completionLoadTasks(state, seedId) {
        var ids = state.selected.milestone.map(function (row) { return row.Value; }).join(',');
        var request = state.taskLoadRequest = (state.taskLoadRequest || 0) + 1;
        state.loadingTasks = !!ids;
        if (!ids) {
            ++state.subtaskLoadRequest;
            state.loadingSubtasks = false;
            state.selected.task = []; state.selected.subtask = [];
            state.tasks = []; state.subtasks = [];
            return;
        }
        completionDependencyPost('bindtasks', { mprojectid: String(state.projectId), mid: ids, tid: '0' })
            .then(function (rows) {
                if (!state.open || request !== state.taskLoadRequest ||
                    ids !== state.selected.milestone.map(function (row) { return row.Value; }).join(',')) return;
                var choices = completionOptions(rows);
                var retained = state.selected.task.map(function (row) { return String(row.Value); });
                var seedIds = state.editSeed ? state.editSeed.task : [String(seedId || 0)];
                state.loadingTasks = false;
                state.tasks = choices;
                state.selected.task = choices.filter(function (row) {
                    return retained.indexOf(String(row.Value)) >= 0 || seedIds.indexOf(String(row.Value)) >= 0;
                });
                completionLoadSubtasks(state, state.defaults.subtask);
            }, function () { if (state.open && request === state.taskLoadRequest) { state.loadingTasks = false; state.error = 'Unable to load tasks.'; } });
    }
    function completionLoadExisting(state) {
        state.loadingExisting = true;
        completionDependencyPost('bindtaskdata', { RecordId: String(state.id), RecordType: state.type })
            .then(function (html) {
                if (!state.open || $scope.completionDependency !== state) return;
                var detached = document.createElement('div');
                detached.innerHTML = html || '';
                state.existing = Array.prototype.map.call(detached.querySelectorAll('tbody tr'), function (row) {
                    var cells = row.querySelectorAll('td');
                    var editLink = row.querySelector('a[onclick*="Binddetails"]');
                    var match = editLink && (editLink.getAttribute('onclick') || '').match(/Binddetails\((\d+)\)/);
                    var value = function (index) { return cells[index] ? (cells[index].textContent || '').replace(/\s+/g, ' ').trim() : ''; };
                    var dependencyNames = cells[6] ? (cells[6].innerText || cells[6].textContent || '').split(/\n|\s*[,;]\s*/).map(function (part) { return part.trim(); }).filter(Boolean) : [];
                    return cells.length >= 7 && match ? {
                        id: Number(match[1]), milestone: value(3), task: value(4),
                        subtask: value(5), name: value(6), names: dependencyNames.length ? dependencyNames : [value(6)]
                    } : null;
                }).filter(function (row) { return row && row.name; });
                // The legacy HTML concatenates multiple linked names. Read the
                // stored ID lists for each row and display each level separately.
                state.existing.forEach(function (existing) {
                    completionDependencyPost('Binddetail', { RecordId: existing.id }).then(function (xml) {
                        if (!state.open || $scope.completionDependency !== state || state.existing.indexOf(existing) < 0) return;
                        var entry = new DOMParser().parseFromString(xml, 'text/xml').getElementsByTagName('Table')[0];
                        if (!entry) return;
                        if (state.type === 'project' &&
                            (completionCsv(entry, 'projectid')[0] !== String(state.id) ||
                             ['milestoneid', 'taskid', 'subtaskid'].some(function (key) {
                                 var value = completionCsv(entry, key)[0]; return value && value !== '0';
                             }))) {
                            state.existing.splice(state.existing.indexOf(existing), 1);
                            return;
                        }
                        var miles = state.milestones.map(function (row) { return {id:row.Value, Milestone:row.Text}; }), tasks = [], subtasks = [];
                        miles.forEach(function (mile) { (mile.tasks || []).forEach(function (task) {
                            tasks.push(task); (task.subtasks || []).forEach(function (subtask) { subtasks.push(subtask); });
                        }); });
                        var levels = [
                            { key: 'dmilestoneid', label: 'Milestones', rows: miles, name: function (row) { return row.Milestone; } },
                            { key: 'dtaskid', label: 'Tasks', rows: tasks, name: function (row) { return row.TaskName || row.taskname; } },
                            { key: 'dsubtaskid', label: 'Subtasks', rows: subtasks, name: function (row) { return row.SubtaskName || row.subtaskname; } }
                        ];
                        function renderLinkedNames() {
                            existing.names = levels.map(function (level) {
                            var ids = completionCsv(entry, level.key).filter(function (id) { return id !== '0'; });
                            if (!ids.length) return '';
                            return level.label + ': ' + ids.map(function (id) {
                                var option = (level.options || []).find(function (row) { return String(row.Value) === String(id); });
                                var match = level.rows.find(function (row) { return String(row.id) === String(id); });
                                return option ? option.Text : (match ? (level.name(match) || '#' + id) : '#' + id);
                            }).join(', ');
                            }).filter(Boolean);
                        }
                        renderLinkedNames();
                        var savedMiles = completionCsv(entry, 'dmilestoneid').filter(function (id) { return id !== '0'; });
                        if (savedMiles.length && completionCsv(entry, 'dtaskid').length) {
                            completionDependencyPost('bindtasks', { mprojectid: String(state.projectId), mid: savedMiles.join(','), tid: '0' })
                                .then(function (options) { levels[1].options = completionOptions(options); renderLinkedNames(); });
                        }
                        // Some subtask arrays are not populated in the current
                        // view. Resolve their saved IDs from the same project
                        // dropdown endpoint used by the editor.
                        var taskIds = completionCsv(entry, 'dtaskid').filter(function (id) { return id !== '0'; });
                        if (taskIds.length && completionCsv(entry, 'dsubtaskid').length) {
                            completionDependencyPost('bindsubtasks', {
                                mprojectid: String(state.projectId), tid: taskIds.join(','), stid: '0'
                            }).then(function (options) {
                                if (!state.open || $scope.completionDependency !== state || state.existing.indexOf(existing) < 0) return;
                                levels[2].options = completionOptions(options);
                                renderLinkedNames();
                            });
                        }
                    });
                });
            }, function () { if (state.open) state.error = 'Unable to load existing dependencies.'; })
            .finally(function () { state.loadingExisting = false; });
    }
    $scope.showCompletionDependencyPopup = function (id, type, projectId, name, item) {
        var request = ++completionDependencyRequest;
        $scope.completionDependency.open = false;
        var defaults = completionHierarchy(id, type, item || {});
        var state = $scope.completionDependency = {
            open: true, mode: 'list', id: id, type: type, projectId: projectId,
            name: name, menu: '', search: {}, selected: { milestone: [], task: [], subtask: [] },
            defaults: defaults, milestones: [], tasks: [], subtasks: [], existing: [],
            loadingExisting: false, error: '', notice: '', saving: false
        };
        completionDependencyPost('bindmilestonedata', { mprojectid: String(projectId), mid: '0' })
            .then(function (rows) {
                if (request !== completionDependencyRequest) return;
                state.milestones = completionOptions(rows);
                var own = state.milestones.find(function (row) { return String(row.Value) === String(defaults.milestone); });
                if (own) { state.selected.milestone = [own]; completionLoadTasks(state, defaults.task); }
                completionLoadExisting(state);
            }, function () {
                if (request === completionDependencyRequest) {
                    state.error = 'Unable to load milestones.';
                    completionLoadExisting(state);
                }
            });
    };
    $scope.closeCompletionDependencyPopup = function () {
        ++completionDependencyRequest;
        $scope.completionDependency.open = false;
    };
    function completionCsv(xmlRow, name) {
        var node = xmlRow.getElementsByTagName(name)[0];
        return node && node.textContent ? node.textContent.split(',').map(function (part) { return part.trim(); }).filter(Boolean) : [];
    }
    $scope.addCompletionDependency = function () {
        $scope.resetCompletionDependencyEdit();
        $scope.completionDependency.mode = 'add';
    };
    $scope.backToCompletionDependencyList = function () {
        var state = $scope.completionDependency;
        state.mode = 'list'; state.menu = ''; state.error = '';
        state.editingId = 0; state.editSeed = null;
    };
    $scope.editCompletionDependency = function (existing) {
        var state = $scope.completionDependency;
        state.error = ''; state.notice = '';
        completionDependencyPost('Binddetail', { RecordId: Number(existing.id) }).then(function (xml) {
            if (!state.open) return;
            var parsed = new DOMParser().parseFromString(xml, 'text/xml');
            var entry = parsed.getElementsByTagName('Table')[0];
            if (!entry || completionCsv(entry, 'id')[0] !== String(existing.id)) {
                state.error = 'Unable to load dependency details.'; return;
            }
            state.editingId = Number(existing.id);
            state.editSeed = { milestone: completionCsv(entry, 'dmilestoneid'),
                task: completionCsv(entry, 'dtaskid'), subtask: completionCsv(entry, 'dsubtaskid') };
            state.selected = { milestone: [], task: [], subtask: [] };
            state.selected.milestone = state.milestones.filter(function (row) {
                return state.editSeed.milestone.indexOf(String(row.Value)) >= 0;
            });
            state.menu = '';
            completionLoadTasks(state, 0);
            state.mode = 'edit';
        }, function () { state.error = 'Unable to load dependency details.'; });
    };
    $scope.resetCompletionDependencyEdit = function () {
        var state = $scope.completionDependency;
        state.editingId = 0; state.editSeed = null;
        state.error = ''; state.notice = '';
        state.selected = { milestone: [], task: [], subtask: [] };
        var own = state.milestones.find(function (row) { return String(row.Value) === String(state.defaults.milestone); });
        if (own) { state.selected.milestone = [own]; completionLoadTasks(state, state.defaults.task); }
    };
    $scope.deleteCompletionDependency = function (existing) {
        var state = $scope.completionDependency;
        if (!window.confirm('Delete this dependency?')) return;
        state.error = ''; state.notice = '';
        completionDependencyPost('DeleteDependency', { RecordId: Number(existing.id) }).then(function (result) {
            if (!state.open) return;
            if (String(result) !== '1') { state.error = 'Dependency could not be deleted.'; return; }
            state.notice = 'Dependency deleted.';
            if (state.editingId === existing.id) $scope.resetCompletionDependencyEdit();
            completionLoadExisting(state);
        }, function () { state.error = 'Dependency could not be deleted.'; });
    };
    $scope.completionDependencySelected = function (level, id) {
        return ($scope.completionDependency.selected[level] || []).some(function (row) { return String(row.Value) === String(id); });
    };
    $scope.completionDependencyOutsideClick = function (event) {
        if (!event.target.closest('.pms-completion-picker')) {
            $scope.completionDependency.menu = '';
        }
    };
    $scope.completionDependencyLabel = function (level) {
        var rows = $scope.completionDependency.selected[level] || [];
        return rows.length ? rows.map(function (row) { return row.Text; }).join(', ') :
            'Select ' + (level === 'subtask' ? 'subtasks' : level + 's') + ' from this project';
    };
    $scope.toggleCompletionDependency = function (level, option) {
        var state = $scope.completionDependency;
        var selected = state.selected[level];
        var index = selected.findIndex(function (row) { return String(row.Value) === String(option.Value); });
        var removing = index >= 0;
        if (!removing) selected.push(option); else selected.splice(index, 1);
        state.search[level] = '';
        state.error = '';
        if (level === 'milestone') {
            state.editSeed = null;
            completionLoadTasks(state, removing ? 0 : state.defaults.task);
        } else if (level === 'task') {
            state.editSeed = null;
            completionLoadSubtasks(state, removing ? 0 : state.defaults.subtask);
        }
    };
    $scope.saveCompletionDependency = function () {
        var state = $scope.completionDependency;
        if (!state.selected.milestone.length) { state.error = 'Select at least one milestone.'; return; }
        var selectedIds = function (level) { return state.selected[level].map(function (row) { return String(row.Value); }); };
        var explicit = ['milestone', 'task', 'subtask'].some(function (level) {
            return selectedIds(level).some(function (value) { return value !== String(state.defaults[level] || 0); });
        });
        if (!explicit && !state.editingId) { state.error = 'Select another milestone, task, or subtask as the dependency.'; return; }
        state.saving = true; state.error = ''; state.notice = '';
        var milestones = selectedIds('milestone');
        var tasks = selectedIds('task');
        var subtasks = selectedIds('subtask');
        // Preselected ancestors navigate the picker; include one only when a
        // chosen child belongs to it, so it cannot become a self dependency.
        var explicitTasks = tasks.filter(function (value) { return value !== String(state.defaults.task || 0); });
        var explicitSubtasks = subtasks.filter(function (value) { return value !== String(state.defaults.subtask || 0); });
        var childInOwnTask = false, childInOwnMilestone = false;
        angular.forEach((($scope.projectlist || {}).miles || []), function (milestone) {
            if (String(milestone.id) !== String(state.defaults.milestone)) return;
            angular.forEach(milestone.tasks || [], function (task) {
                if (explicitTasks.indexOf(String(task.id)) >= 0) childInOwnMilestone = true;
                angular.forEach(task.subtasks || [], function (subtask) {
                    if (explicitSubtasks.indexOf(String(subtask.id)) < 0) return;
                    childInOwnMilestone = true;
                    if (String(task.id) === String(state.defaults.task)) childInOwnTask = true;
                });
            });
        });
        if (!childInOwnMilestone) milestones = milestones.filter(function (value) { return value !== String(state.defaults.milestone || 0); });
        if (!childInOwnTask) tasks = tasks.filter(function (value) { return value !== String(state.defaults.task || 0); });
        subtasks = explicitSubtasks;
        if (!milestones.length && (tasks.length || subtasks.length)) {
            // Legacy sp_dependencydetails joins tasks through their milestones.
            milestones = [String(state.defaults.milestone)];
        }
        var details = { id: String(state.editingId || 0), mprojectid: String(state.projectId), did: String(state.id), dtype: state.type,
            pid: '0', mid: state.type === 'milestone' ? String(state.id) : '0',
            tid: state.type === 'task' ? String(state.id) : '0',
            stid: state.type === 'subtask' ? String(state.id) : '0',
            dmid: milestones.join(','), dtaskid: tasks.join(','), dsubtaskid: subtasks.join(',') };
        completionDependencyPost('SaveeDependency', { mdetails: details }).then(function (result) {
            if (String(result) === '0') { state.error = 'Dependency could not be saved.'; return; }
            state.notice = 'Dependency saved.';
            state.mode = 'list'; state.menu = '';
            state.editingId = 0; state.editSeed = null;
            state.selected = { milestone: [], task: [], subtask: [] };
            state.tasks = []; state.subtasks = [];
            var own = state.milestones.find(function (row) { return String(row.Value) === String(state.defaults.milestone); });
            if (own) { state.selected.milestone = [own]; completionLoadTasks(state, state.defaults.task); }
            completionLoadExisting(state);
        }, function (error) {
            state.error = (error.data && error.data.Message) || 'Dependency could not be saved.';
        }).finally(function () { state.saving = false; });
    };

    // List view uses completion rules; Gantt scheduling links remain in the
    // separate predecessor/successor popup.

    $scope.issuePanel = { open: false, mode: 'list', rows: [] };
    var issuePanelRequest = 0;
    function issueContext(project) {
        return { item: project, type: 'project', title: project.ProjectName || project.projectname || 'Project',
            breadcrumb: project.ProjectName || project.projectname || 'Project',
            projectId: project.id, milestoneId: 0, taskId: 0, subtaskId: 0 };
    }
    $scope.openIssueList = function (item, type, title, $event) {
        if ($event) $event.preventDefault();
        if (!item || !item.id) return;
        ++issuePanelRequest;
        $scope.issuePanel = { open: true, mode: 'list', rows: [], loading: true, error: '', search: '', page: 1, pageSize: 10,
            context: issueContext(item) };
        $scope.refreshIssueList();
    };
    $scope.refreshIssueList = function () {
        var state = $scope.issuePanel;
        if (!state.open) return;
        var request = ++issuePanelRequest, context = state.context;
        state.loading = true; state.error = '';
        return $http.post('/pms/Addissues.aspx/bindIssuesdata', {
            RecordId: String(context.projectId), milstoneid: String(context.milestoneId),
            taskid: String(context.taskId), subtaskid: String(context.subtaskId),
            issuetype: context.type === 'milestone' ? 'milstone' : context.type
        }).then(function (response) {
            if (request !== issuePanelRequest || !state.open) return;
            var detached = document.createElement('div');
            detached.innerHTML = response.data.d || '';
            state.rows = Array.prototype.map.call(detached.querySelectorAll('tbody tr'), function (tr) {
                var cells = tr.querySelectorAll('td');
                if (cells.length < 10) return null;
                var text = function (n) { return cells[n] ? (cells[n].textContent || '').replace(/\s+/g, ' ').trim() : ''; };
                // The legacy cells contain a hidden Bootstrap dropdown with
                // every possible choice. Read only the visible selected badge.
                var selected = function (n, prefix) {
                    var badge = cells[n] && cells[n].querySelector('a[id^="' + prefix + '"]');
                    return badge ? (badge.textContent || '').replace(/\s+/g, ' ').trim() : text(n);
                };
                var people = function (n) {
                    if (!cells[n]) return [];
                    var names = [];
                    Array.prototype.forEach.call(cells[n].querySelectorAll('.avatar-group .avatar-group-item'), function (anchor) {
                        var overflow = anchor.getAttribute('data-bs-original-title') === 'more';
                        var value = overflow ? anchor.getAttribute('title') :
                            (anchor.getAttribute('data-bs-original-title') || anchor.getAttribute('title'));
                        if (!value) return;
                        (overflow ? value.split(',') : [value]).forEach(function (name) {
                            name = name.replace(/\s+/g, ' ').trim();
                            if (name && name.toLowerCase() !== 'more' && names.indexOf(name) === -1) names.push(name);
                        });
                    });
                    return names;
                };
                var link = Array.prototype.find.call(tr.querySelectorAll('a[onclick]'), function (anchor) {
                    return /\bBinddetails\(\d+\)/.test(anchor.getAttribute('onclick') || '');
                });
                var deleteLink = tr.querySelector('a[onclick*="deletedata("]');
                var match = link && (link.getAttribute('onclick') || '').match(/Binddetails\((\d+)\)/);
                var owners = people(2), teams = people(3);
                return { id: match ? Number(match[1]) : null, canEdit: !!link && link.style.display !== 'none',
                    canDelete: !!deleteLink && deleteLink.style.display !== 'none', number: text(0), name: text(1), owner: owners.join(', '), owners: owners,
                    team: teams.join(', '), teams: teams, status: selected(4, 'btnsts_'), priority: selected(5, 'btnpri_'), assignee: text(7),
                    created: text(8), target: text(9), issueType: selected(10, 'btnissutype_'), milestone: text(12) };
            }).filter(Boolean);
            state.page = 1;
        }, function () { if (request === issuePanelRequest) state.error = 'Unable to load issues. Please try again.'; })
        .finally(function () { if (request === issuePanelRequest) state.loading = false; });
    };
    $scope.issueFilteredRows = function () {
        var state = $scope.issuePanel, needle = (state.search || '').toLowerCase();
        return (state.rows || []).filter(function (row) {
            return !needle || [row.name, row.owner, row.team, row.status, row.priority, row.assignee, row.issueType].join(' ').toLowerCase().indexOf(needle) !== -1;
        });
    };
    $scope.issuePageRows = function () {
        var state = $scope.issuePanel, rows = $scope.issueFilteredRows();
        return rows.slice((state.page - 1) * state.pageSize, state.page * state.pageSize);
    };
    $scope.issueFormOptions = { employees: [], statuses: [], priorities: [], types: [], replicabilities: [] };
    $scope.issueMulti = { open: '', active: 0, search: { Owner: '', team: '', AssignTo: '' } };
    function newIssueForm() {
        return { Issues: '', Description: '', Owner: '', team: [], AssignTo: '', StartDate: null, EndDate: null,
            Reminder: '0', ReminderType: '', Days: null, Time: null, Notifyusers: '', ChooseDate: null,
            IssueStatus: '', Priority: '', IssueType: '', Replicability: '', UploadImage: '' };
    }
    $scope.issueForm = newIssueForm();
    function loadIssueOptions() {
        if ($scope.issueFormOptions.employees.length) return Promise.resolve();
        $scope.issuePanel.loadingOptions = true;
        return $http.post('/pms/Addissues.aspx/GetIssueFormOptions', {}).then(function (response) {
            $scope.issueFormOptions = response.data.d || $scope.issueFormOptions;
        }, function () {
            throw new Error('Unable to load issue options. Please try again.');
        }).finally(function () { $scope.issuePanel.loadingOptions = false; });
    }
    $scope.openIssueEditor = function () {
        $scope.issuePanel.mode = 'add';
        $scope.issuePanel.formError = '';
        $scope.issueForm = newIssueForm();
        $scope.issueMulti.open = '';
        $scope.issueMulti.search = { Owner: '', team: '', AssignTo: '' };
        loadIssueOptions().catch(function (error) { $scope.issuePanel.formError = error.message; });
    };
    function issueXmlField(record, name) {
        var node = Array.prototype.find.call(record.children, function (child) {
            return child.localName.toLowerCase() === name.toLowerCase();
        });
        return node ? node.textContent.trim() : '';
    }
    function issueFormDate(value) {
        var match = (value || '').match(/^(\d{4})-(\d\d)-(\d\d)/);
        return match ? new Date(Date.UTC(+match[1], +match[2] - 1, +match[3])) : null;
    }
    function issueFormTime(value) {
        var match = (value || '').match(/(?:T|^)(\d\d):(\d\d)/);
        return match ? new Date(Date.UTC(1970, 0, 1, +match[1], +match[2])) : null;
    }
    $scope.editIssue = function (issue) {
        if (!issue.id || !issue.canEdit) return;
        var state = $scope.issuePanel;
        state.mode = 'add'; state.formError = ''; state.loadingOptions = true;
        $scope.issueForm = newIssueForm();
        $scope.issueMulti.open = '';
        $scope.issueMulti.search = { Owner: '', team: '', AssignTo: '' };
        Promise.all([loadIssueOptions(), $http.post('/pms/Addissues.aspx/Bindissuesdetail', { RecordId: issue.id })])
            .then(function (results) {
                if (!state.open) return;
                var doc = new DOMParser().parseFromString(results[1].data.d || '', 'application/xml');
                var record = doc.documentElement && doc.documentElement.firstElementChild;
                if (!record || doc.querySelector('parsererror')) throw new Error('Unable to read issue details.');
                var get = function (name) { return issueXmlField(record, name); };
                var multi = function (name) { return get(name).split(',').map(function (id) { return id.trim(); }).filter(Boolean); };
                $scope.$evalAsync(function () {
                    $scope.issueForm = {
                        id: String(issue.id), Issues: get('Issues'), Description: get('Description'), Owner: multi('Owner')[0] || '', team: multi('team'),
                        AssignTo: get('AssignTo'), StartDate: issueFormDate(get('StartDate')), EndDate: issueFormDate(get('EndDate')),
                        Reminder: get('Reminder') || '0', ReminderType: get('ReminderType'), Days: get('Days'),
                        Time: issueFormTime(get('Time')), Notifyusers: get('Notifyusers'), ChooseDate: issueFormDate(get('ChooseDate')),
                        IssueStatus: get('IssueStatus'), Priority: get('Priority'), IssueType: get('IssueType'),
                        Replicability: get('Replicability'), UploadImage: get('UploadImage'),
                        ProjectId: get('ProjectId'), Milestoneid: get('Milestoneid'), tasklistid: get('tasklistid'),
                        taskid: get('taskid'), subtaskid: get('subtaskid')
                    };
                    $scope.issueMulti.search.AssignTo = $scope.issueForm.AssignTo ? $scope.issuePersonName($scope.issueForm.AssignTo) : '';
                    $scope.issueMulti.search.Owner = $scope.issueForm.Owner ? $scope.issuePersonName($scope.issueForm.Owner) : '';
                });
            }).catch(function (error) { state.formError = error.message || 'Unable to load issue details.'; })
            .finally(function () { state.loadingOptions = false; $scope.$evalAsync(); });
    };
    $scope.deleteIssue = function (issue) {
        if (!issue.id || !issue.canDelete || !window.confirm('Delete issue "' + issue.name + '"?')) return;
        var state = $scope.issuePanel;
        state.deletingId = issue.id; state.error = '';
        $http.post('/pms/Addissues.aspx/Deleteissuesdata', { RecordId: issue.id }).then(function () {
            $scope.refreshIssueList();
        }, function () { state.error = 'Unable to delete this issue. Please try again.'; })
        .finally(function () { state.deletingId = null; });
    };
    $scope.toggleIssuePerson = function (field, option) {
        var items = $scope.issueForm[field], index = items.indexOf(String(option.id));
        if (index >= 0) items.splice(index, 1);
        else items.push(String(option.id));
        $scope.issueMulti.search[field] = '';
        $scope.issueMulti.active = 0;
    };
    $scope.issuePersonMatches = function (field) {
        var needle = ($scope.issueMulti.search[field] || '').trim().toLowerCase();
        return ($scope.issueFormOptions.employees || []).filter(function (person) {
            return !needle || String(person.name || '').toLowerCase().indexOf(needle) !== -1;
        });
    };
    $scope.openIssuePeople = function (field) {
        if ($scope.issueMulti.open !== field) $scope.issueMulti.active = -1;
        if ((field === 'AssignTo' || field === 'Owner') && $scope.issueMulti.open !== field) $scope.issueMulti.search[field] = '';
        $scope.issueMulti.open = field;
    };
    $scope.issuePersonKeydown = function (field, event) {
        // MAC FIX: ignore keys that belong to an active text composition (IME / accents).
        if (event.isComposing || event.keyCode === 229) return;
        if (event.key === 'Escape') { $scope.issueMulti.open = ''; event.preventDefault(); return; }
        if (event.key === 'ArrowDown' || event.key === 'ArrowUp') {
            event.preventDefault();
            if ($scope.issueMulti.open !== field) $scope.openIssuePeople(field);
            var list = $scope.issuePersonMatches(field);
            if (list.length) {
                var current = Math.min($scope.issueMulti.active, list.length - 1);
                $scope.issueMulti.active = event.key === 'ArrowDown' ? (current + 1) % list.length :
                    (current <= 0 ? list.length - 1 : current - 1);
                setTimeout(function () {
                    var menu = event.target.closest('.pms-issue-field').querySelector('.pms-issue-person-list');
                    var option = menu && menu.children[$scope.issueMulti.active];
                    // MAC FIX: scroll inside the list only (Safari scrollIntoView also moves the panel/page).
                    if (option && menu) {
                        var top = option.offsetParent === menu ? option.offsetTop : option.offsetTop - menu.offsetTop;
                        var bottom = top + option.offsetHeight;
                        if (top < menu.scrollTop) menu.scrollTop = top;
                        else if (bottom > menu.scrollTop + menu.clientHeight) menu.scrollTop = bottom - menu.clientHeight;
                    }
                }, 0);
            }
        } else if (event.key === 'Enter' && $scope.issueMulti.open === field) {
            var matches = $scope.issuePersonMatches(field);
            if (matches.length) {
                event.preventDefault();
                $scope.chooseIssuePerson(field, matches[Math.max(0, Math.min($scope.issueMulti.active, matches.length - 1))]);
            }
        }
    };
    $scope.chooseIssuePerson = function (field, person) {
        if (field === 'AssignTo' || field === 'Owner') {
            $scope.issueForm[field] = String(person.id);
            $scope.issueMulti.search[field] = person.name;
            $scope.issueMulti.open = '';
            if (document.activeElement &&
                (document.activeElement.id === 'pmsIssueAssigneeSearch' || document.activeElement.id === 'pmsIssueOwnerSearch'))
                document.activeElement.blur();
        } else $scope.toggleIssuePerson(field, person);
        $scope.issueMulti.active = 0;
    };
    $scope.issuePersonName = function (id) {
        var option = $scope.issueFormOptions.employees.find(function (row) { return String(row.id) === String(id); });
        return option ? option.name : id;
    };
    $scope.closeIssuePersonList = function ($event) {
        if (!$event.target.closest('.pms-issue-multi') && !$event.target.closest('.pms-issue-single') && !$event.target.closest('.pms-issue-person-list')) {
            $scope.issueMulti.open = '';
            $scope.issueMulti.search.AssignTo = $scope.issueForm.AssignTo ? $scope.issuePersonName($scope.issueForm.AssignTo) : '';
            $scope.issueMulti.search.Owner = $scope.issueForm.Owner ? $scope.issuePersonName($scope.issueForm.Owner) : '';
        }
    };
    function issueDateValue(value) {
        if (!value) return '';
        if (typeof value === 'string') return value;
        return value.getUTCFullYear() + '-' + String(value.getUTCMonth() + 1).padStart(2, '0') + '-' + String(value.getUTCDate()).padStart(2, '0');
    }
    function issueTimeValue(value) {
        if (!value) return '';
        if (typeof value === 'string') return value;
        return String(value.getUTCHours()).padStart(2, '0') + ':' + String(value.getUTCMinutes()).padStart(2, '0');
    }
    $scope.saveIssueForm = function (addMore) {
        var state = $scope.issuePanel, form = $scope.issueForm, context = state.context;
        state.formError = '';
        if (state.loadingOptions || state.saving) return;
        if (!form.Issues.trim()) { state.formError = 'Enter the issue title.'; return; }
        if (!form.AssignTo || !form.IssueStatus || !form.Priority || !form.IssueType || !form.Replicability) {
            state.formError = 'Select assignee, status, priority, issue type and replicability.'; return;
        }
        if (form.StartDate && form.EndDate && form.EndDate < form.StartDate) {
            state.formError = 'End date must be on or after start date.'; return;
        }
        var payload = {
            id: form.id || '0', ProjectId: form.ProjectId || String(context.projectId), Milestoneid: form.Milestoneid || String(context.milestoneId),
            tasklistid: form.tasklistid || '0', taskid: form.taskid || String(context.taskId), subtaskid: form.subtaskid || String(context.subtaskId),
            Issues: form.Issues.trim(), StartDate: issueDateValue(form.StartDate), EndDate: issueDateValue(form.EndDate),
            Owner: form.Owner || '', team: form.team.join(','), Issueon: context.type,
            project_id: String(context.projectId), Description: form.Description || '',
            Reminder: form.Reminder || '0', ReminderType: form.Reminder === '1' ? (form.ReminderType || '') : '',
            Days: form.Reminder === '1' && form.ReminderType === '3' ? String(form.Days || '') : '',
            Time: form.Reminder !== '0' ? issueTimeValue(form.Time) : '',
            Notifyusers: form.Reminder !== '0' ? (form.Notifyusers || '') : '',
            ChooseDate: form.Reminder === '2' ? issueDateValue(form.ChooseDate) : '', AssignTo: form.AssignTo,
            IssueStatus: form.IssueStatus, Priority: form.Priority, IssueType: form.IssueType,
            Replicability: form.Replicability, UploadImage: form.UploadImage || ''
        };
        state.saving = true;
        $http.post('/pms/Addissues.aspx/SaveIsues', { issuesdetails: payload }).then(function (response) {
            var savedId = Number(response.data.d);
            if (!savedId) throw new Error('Issue could not be saved.');
            var input = document.getElementById('pmsIssueFiles');
            if (!input || !input.files || !input.files.length) return;
            var data = new FormData();
            Array.prototype.forEach.call(input.files, function (file) { data.append(file.name, file); });
            return $http.post('/pms/FileUploadHandlerIssues.ashx?id=' + encodeURIComponent(savedId), data, {
                transformRequest: angular.identity, headers: { 'Content-Type': undefined }
            }).catch(function () { throw new Error('Issue saved, but the file upload failed. Return to issues to see the saved record.'); });
        }).then(function () {
            state.saving = false;
            if (addMore) {
                $scope.issueForm = newIssueForm();
                var fileInput = document.getElementById('pmsIssueFiles');
                if (fileInput) fileInput.value = '';
            } else $scope.backToIssueList();
        }).catch(function (error) {
            state.formError = error.message || (error.data && error.data.Message) || 'Issue could not be saved.';
            state.saving = false;
        });
    };
    $scope.backToIssueList = function () { $scope.issuePanel.mode = 'list'; $scope.refreshIssueList(); };
    $scope.closeIssueList = function () { ++issuePanelRequest; $scope.issuePanel.open = false; };

    $scope.openProjectDependency = function (project, event) {
        if (event) event.preventDefault();
        if (!project || !project.id) return;
        hideProjectMenu(event);
        $scope.showCompletionDependencyPopup(project.id, 'project', project.id,
            project.ProjectName || project.projectname || 'Project', project);
    };
    $scope.openProjectIssue = function (project, event) {
        if (event) event.preventDefault();
        if (!project || !project.id) return;
        hideProjectMenu(event);
        $scope.openIssueList(project, 'project', project.ProjectName || project.projectname || 'Project');
    };
    function hideProjectMenu(event) {
        var menu = event && event.target && event.target.closest('.dropdown-menu');
        var trigger = menu && menu.previousElementSibling;
        if (trigger && window.bootstrap && bootstrap.Dropdown) {
            var dropdown = bootstrap.Dropdown.getInstance(trigger);
            if (dropdown) dropdown.hide();
        }
    }

// manage zooming
 $scope.zoom = {level: 100};
 $scope.zoomLevel = 100;
    $scope.applyZoom = function (zoomValue) {
        var value = parseInt(zoomValue, 10);

        if (isNaN(value)) {
            value = 100;
        }

        value = Math.max(50, Math.min(150, value));
        $scope.zoom.level = value;

        var scale = value / 100;
        var table = document.querySelector(".project-table");

        if (table) {
            table.style.transform = "scale(" + scale + ")";
            table.style.transformOrigin = "top left";
            table.style.width = (100 / scale) + "%";
        }
    };
//end
    // Timezone-independent display helpers.
    // Date-only values are calendar dates, not timestamps.
    $scope.formatDateOnly = function (value) {
        return formatDateOnly(value);
    };

    // UTC is used as the neutral application timezone for actual timestamps.
    // This intentionally shows exactly the same date/time in every country.
    $scope.formatDateTimeSameEverywhere = function (value, format) {
        return formatDateTimeSameEverywhere(value, format);
    };


    // -----------------------------------------------------------------
    // Project-list timeline progress bar.
    // This is display-only and does not change any project save/update flow.
    // The currently running day is not counted in Spent Days.
    // -----------------------------------------------------------------
    function clampProjectTimelinePercent(value) {
        value = Number(value);
        if (!isFinite(value)) return 0;
        return Math.max(0, Math.min(100, Math.round(value * 100) / 100));
    }

    $scope.getProjectTimelineMetrics = function (project) {
        if (!project) {
            return {
                valid: false,
                percent: 0,
                totalDays: 0,
                spentDays: 0,
                leftDays: 0
            };
        }

        var startParts = getDateOnlyParts(project.StartDate);
        var endParts = getDateOnlyParts(project.TargetDate);

        if (!isValidDateOnlyParts(startParts) || !isValidDateOnlyParts(endParts)) {
            return {
                valid: false,
                percent: 0,
                totalDays: 0,
                spentDays: 0,
                leftDays: 0
            };
        }

        var dayMs = 86400000;
        var startDay = Math.floor(Date.UTC(
            startParts.year,
            startParts.month - 1,
            startParts.day
        ) / dayMs);
        var endDay = Math.floor(Date.UTC(
            endParts.year,
            endParts.month - 1,
            endParts.day
        ) / dayMs);

        if (endDay < startDay) {
            return {
                valid: false,
                percent: 0,
                totalDays: 0,
                spentDays: 0,
                leftDays: 0
            };
        }

        var now = new Date();
        var todayDay = Math.floor(Date.UTC(
            now.getFullYear(),
            now.getMonth(),
            now.getDate()
        ) / dayMs);

        // Inclusive timeline duration: Start Date and End Date are both counted.
        var totalDays = (endDay - startDay) + 1;
        var spentDays = 0;

        if (todayDay < startDay) {
            spentDays = 0;
        } else if (todayDay > endDay) {
            spentDays = totalDays;
        } else {
            // Count only fully completed days. Today is still running,
            // therefore it stays in Left Days and is not part of Spent Days.
            spentDays = Math.max(todayDay - startDay, 0);
        }

        var leftDays = Math.max(totalDays - spentDays, 0);
        var percent = totalDays > 0
            ? clampProjectTimelinePercent((spentDays / totalDays) * 100)
            : 0;

        return {
            valid: true,
            percent: percent,
            totalDays: totalDays,
            spentDays: spentDays,
            leftDays: leftDays
        };
    };

    $scope.getProjectTimelinePercent = function (project) {
        return $scope.getProjectTimelineMetrics(project).percent;
    };

    var projectTimelineMonthNames = [
        'Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun',
        'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'
    ];

    function formatProjectTimelineShortDate(value) {
        var parts = getDateOnlyParts(value);
        if (!isValidDateOnlyParts(parts)) return '';

        return String(parts.day).padStart(2, '0') + ' ' +
            projectTimelineMonthNames[parts.month - 1];
    }

    $scope.getProjectTimelineDateRange = function (project) {
        var metrics = $scope.getProjectTimelineMetrics(project);
        if (!metrics.valid) return '';

        return formatProjectTimelineShortDate(project.StartDate) + ' - ' +
            formatProjectTimelineShortDate(project.TargetDate);
    };

    $scope.getProjectTimelineTooltip = function (project) {
        var metrics = $scope.getProjectTimelineMetrics(project);

        if (!metrics.valid) {
            return 'Timeline dates are not available';
        }

        return metrics.leftDays + (metrics.leftDays === 1 ? ' day left' : ' days left');
    };

    // Update and show the existing tooltip on the same hover event.
    // Do not dispose it here, because recreating a tooltip after mouseenter
    // prevents it from opening until a later hover.
    $scope.refreshProjectTimelineTooltip = function ($event, project) {
        var element = $event && ($event.currentTarget || $event.target);
        if (!element) return;

        var title = $scope.getProjectTimelineTooltip(project);
        element.setAttribute('title', title);
        element.setAttribute('data-bs-title', title);
        element.setAttribute('data-bs-original-title', title);

        if (!window.bootstrap || !bootstrap.Tooltip) return;

        var tooltip = bootstrap.Tooltip.getInstance(element);

        if (!tooltip) {
            tooltip = new bootstrap.Tooltip(element, {
                container: 'body',
                trigger: 'manual',
                boundary: 'window',
                html: false,
                title: title,
                customClass: 'project-timeline-tooltip'
            });
        } else if (typeof tooltip.setContent === 'function') {
            tooltip.setContent({ '.tooltip-inner': title });
        }

        tooltip.show();

        var hideTimelineTooltip = function () {
            tooltip.hide();
            element.removeEventListener('mouseleave', hideTimelineTooltip);
        };

        element.addEventListener('mouseleave', hideTimelineTooltip);
    };

    //alert("Controller Loaded");
$scope.showToast = function (msg, duration = 3000) {
        let toast = document.getElementById("toast");
        let msgBox = document.getElementById("toastMessage");
        let bar = document.getElementById("toastBar");

        if (!toast || !msgBox || !bar) return;

        msgBox.innerHTML = msg;

        bar.style.transition = "none";
        bar.style.width = "100%";

        toast.classList.add("show");

        setTimeout(() => {
            bar.style.transition = `width ${duration}ms linear`;
            bar.style.width = "0%";
        }, 50);

        setTimeout(() => {
            toast.classList.remove("show");
        }, duration + 200);
    };
$scope.presearch = "";
$scope.projecttype = "insert";
    $scope.historyprojectname = "";
    $scope.projectextendedhistory = [];
    $scope.uploadedFiles2 = [];
 $scope.showTagDropdown = false;
$scope.showqcustomersDropdown= false;
    $scope.showSODropdown = false;
    $scope.showHeadDropdown = false;
    $scope.showTeamDropdown = false;
    $scope.showStateDropdown = false;
    $scope.showCountryDropdown = false;


$scope.openTagDropdown = function ($event) {
    $event.stopPropagation();
    $scope.showTagDropdown = true;
};
   $scope.modalTitle = "Add teams";
   $scope.freesearch = "0";
   $scope.viewsearch = function ()
    {
       if ($scope.freesearch == "0") {
           $scope.freesearch = "1";
       }
       else {
           $scope.freesearch = "0";
           $scope.searchText = "";
       }
    };
    $scope.currentPage = 1;
    $scope.pageSizes = [10, 20, 50, 100];
    $scope.pageSize = 20;
    $scope.countrylist = [];
    $scope.statelist = [];
    $scope.projectlist = [];
    $scope.mytotalprojectlist =0;
    $scope.myprojectlist_bystatus = [];
    $scope.projectSituationLists = [];
    $scope.projectFilter = {
        situationId: 'all'
    };

    // Calendar view is derived only from the already-loaded projectlist.
    // It does not make another request or change List/Tiles behavior.
    $scope.projectCalendarMonth = new Date(new Date().getFullYear(), new Date().getMonth(), 1, 12, 0, 0, 0);
    $scope.projectCalendarTitle = '';
    $scope.projectCalendarWeeks = [];
    $scope.projectCalendarSelectedDay = null;
    $scope.projectCalendarIsAdmin = false;
    $scope.selectedProjectStatusId = '';

    // Shared status filter for List, Tiles and Calendar views.
    // Blank means Total Project (all statuses).
    $scope.projectStatusFilter = function (project) {
        if (!project) return false;

        var statusMatch = !$scope.selectedProjectStatusId ||
            String(project.ProjectStatus) === String($scope.selectedProjectStatusId);

        return statusMatch && $scope.projectSituationFilter(project);
    };

    $scope.projectSituationFilter = function (project) {
        if (!project) return false;

        var selectedSituation = String(($scope.projectFilter || {}).situationId || 'all');
        if (selectedSituation === 'all') return true;

        var projectSituation = String(project.CriticalLabelId || '0');
        if (selectedSituation === 'none') {
            return projectSituation === '0' || projectSituation === '';
        }

        return projectSituation === selectedSituation;
    };

    $scope.setProjectStatusFilter = function (statusId) {
        $scope.selectedProjectStatusId = statusId ? String(statusId) : '';
        $scope.currentPage = 1;
    };

    $scope.applyProjectSituationFilter = function () {
        if (!$scope.projectFilter) {
            $scope.projectFilter = { situationId: 'all' };
        }
        $scope.projectFilter.situationId = String($scope.projectFilter.situationId || 'all');
        $scope.currentPage = 1;
        $scope.filterstatuslist_count();
    };

    function projectCalendarDate(value) {
        if (!value) return null;
        if (value instanceof Date && !isNaN(value.getTime())) {
            return new Date(value.getFullYear(), value.getMonth(), value.getDate(), 12, 0, 0, 0);
        }
        var parsed = parseDate(value);
        return parsed && !isNaN(parsed.getTime())
            ? new Date(parsed.getFullYear(), parsed.getMonth(), parsed.getDate(), 12, 0, 0, 0)
            : null;
    }

    function projectCalendarDayKey(value) {
        return value.getFullYear() + '-' +
            String(value.getMonth() + 1).padStart(2, '0') + '-' +
            String(value.getDate()).padStart(2, '0');
    }

    $scope.buildProjectCalendar = function () {
        var month = $scope.projectCalendarMonth || new Date();
        month = new Date(month.getFullYear(), month.getMonth(), 1, 12, 0, 0, 0);
        $scope.projectCalendarMonth = month;
        $scope.projectCalendarTitle = month.toLocaleString('en-US', { month: 'long', year: 'numeric' });

        var gridStart = new Date(month.getFullYear(), month.getMonth(), 1, 12, 0, 0, 0);
        gridStart.setDate(gridStart.getDate() - gridStart.getDay());
        var todayKey = projectCalendarDayKey(new Date());
        var weeks = [];
        var selectedKey = $scope.projectCalendarSelectedDay
            ? $scope.projectCalendarSelectedDay.key
            : '';
        var selectedDay = null;
        var firstCurrentMonthDay = null;

        // Parse project bounds once per rebuild rather than once for each of 42 days.
        var calendarRanges = [];
        angular.forEach($scope.projectlist || [], function (project) {
            var start = projectCalendarDate(project.StartDate);
            var end = projectCalendarDate(project.TargetDate);
            if (!start && !end) return;
            if (!start) start = end;
            if (!end) end = start;
            var startTime = start.getTime();
            var endTime = end.getTime();
            calendarRanges.push({ project: project,
                start: Math.min(startTime, endTime), end: Math.max(startTime, endTime) });
        });

        for (var weekIndex = 0; weekIndex < 6; weekIndex++) {
            var week = [];
            for (var dayIndex = 0; dayIndex < 7; dayIndex++) {
                var day = new Date(gridStart.getFullYear(), gridStart.getMonth(),
                    gridStart.getDate() + (weekIndex * 7) + dayIndex, 12, 0, 0, 0);
                var dayTime = day.getTime();
                var projects = [];

                angular.forEach(calendarRanges, function (range) {
                    if (dayTime >= range.start && dayTime <= range.end) {
                        projects.push(range.project);
                    }
                });

                var calendarDay = {
                    date: day,
                    key: projectCalendarDayKey(day),
                    dayNumber: day.getDate(),
                    isCurrentMonth: day.getMonth() === month.getMonth(),
                    isToday: projectCalendarDayKey(day) === todayKey,
                    projects: projects
                };
                if (!firstCurrentMonthDay && calendarDay.isCurrentMonth) {
                    firstCurrentMonthDay = calendarDay;
                }
                if (calendarDay.key === selectedKey || (!selectedKey && calendarDay.isToday)) {
                    selectedDay = calendarDay;
                }
                week.push(calendarDay);
            }
            weeks.push(week);
        }
        $scope.projectCalendarWeeks = weeks;
        $scope.projectCalendarSelectedDay = selectedDay || firstCurrentMonthDay;
    };

    $scope.selectProjectCalendarDay = function (day) {
        $scope.projectCalendarSelectedDay = day;
    };

    $scope.projectCalendarSelectedProjects = function () {
        var day = $scope.projectCalendarSelectedDay;
        return $scope.calendarVisibleProjects(day ? day.projects : []);
    };

    $scope.projectCalendarTotal = function (projects, fieldName) {
        var total = 0;
        angular.forEach($scope.calendarVisibleProjects(projects || []), function (project) {
            var value = parseInt(project[fieldName], 10);
            total += isNaN(value) ? 0 : value;
        });
        return total;
    };

    $scope.changeProjectCalendarMonth = function (offset) {
        var month = $scope.projectCalendarMonth || new Date();
        $scope.projectCalendarMonth = new Date(month.getFullYear(), month.getMonth() + offset, 1, 12, 0, 0, 0);
        $scope.buildProjectCalendar();
    };

    $scope.showCurrentProjectCalendarMonth = function () {
        var today = new Date();
        $scope.projectCalendarMonth = new Date(today.getFullYear(), today.getMonth(), 1, 12, 0, 0, 0);
        // Today must also become the selected day. Without clearing the old
        // selection, buildProjectCalendar falls back to the first day of month.
        $scope.projectCalendarSelectedDay = null;
        $scope.buildProjectCalendar();
    };

    $scope.calendarVisibleProjects = function (projects) {
        var text = String($scope.searchText || '').trim().toLowerCase();
        return (projects || []).filter(function (project) {
            if (!$scope.projectStatusFilter(project)) return false;
            if (!text) return true;
            return [project.ProjectID, project.ProjectName, project.ClientName,
                project.status_name, project.priority_name]
                .join(' ').toLowerCase().indexOf(text) !== -1;
        });
    };

    // Keep the project table/header hidden until the current project request finishes.
    // This prevents the empty table header from flashing behind the loader on browser refresh.
    $scope.projectDataReady = false;

    function getInitialProjectView() {
        var allowedViews = ['table', 'tiles', 'calendar'];
        var savedView = '';

        try {
            savedView = window.localStorage.getItem('pms_project_view') || '';
        } catch (e) {
            savedView = '';
        }

        if (allowedViews.indexOf(savedView) !== -1) {
            return savedView;
        }

        var hashMatch = (window.location.hash || '').match(/viewtype=(table|tiles|calendar)/i);
        return hashMatch ? hashMatch[1].toLowerCase() : 'table';
    }

    $scope.view = getInitialProjectView();

    $scope.setView = function (mode) {
        var allowedViews = ['table', 'tiles', 'calendar'];
        if (allowedViews.indexOf(mode) === -1) return;

        hideProjectTooltips();
        $scope.view = mode;

        try {
            window.localStorage.setItem('pms_project_view', mode);
        } catch (e) {
            // Keep view switching functional even when storage is unavailable.
        }

        setCustomHash("viewtype=" + mode);

        if (mode === 'table') {
            $timeout(function () {
                if (typeof initTableUI === 'function') {
                    initTableUI();
                }
            }, 0);
        }
        else if (mode === 'calendar') {
            $scope.buildProjectCalendar();
        }
    };
    $scope.Bindextenddate_history = function (c) {
        $scope.historyprojectname = c.ProjectName;
        myprojectService.getprojectextendhistory(c.id)
            .then(function (response) {

                $scope.projectextendedhistory = response.data.d;


            })
            .catch(function (error) {
                alert("HTTP ERROR: " +
                    (error.statusText || error.message || "Unknown error"));

                console.error(error);

            });
    }

    function setCustomHash(value) {
        window.location.hash = "##" + value;
    }
    $scope.loadassignform = function (ifname, pid) {
        $scope.setView('vtasks');
        let finalUrl = "/pms/assignprojects/default.aspx?id=" + pid +"";

        if (finalUrl.indexOf("aspx?") !== -1) {
            finalUrl = finalUrl + "&ctype=ang&count=" + $scope.popupCounter;
        } else {
            finalUrl = finalUrl + "?ctype=ang&count=" + $scope.popupCounter;
        }

        // set iframe src
        let frame = document.getElementById(ifname);
        if (frame) {
            frame.src = "";
            frame.src = finalUrl;
        }
    }
    // Page permissions are rendered from default.aspx.cs.
    // Edit and Assign are separate permissions for Milestone, Task and Subtask.
    var pmsPerm = window.PmsAssignPermissions || {};
    function toPermInt(value) {
        return parseInt(value || 0, 10) === 1 ? 1 : 0;
    }
    $scope.pmsPermissions = {
        project: {
            canEdit: toPermInt(pmsPerm.ischeckEdit)            
        }
    };
    
    $scope.canEdit = function (utype) {
        var key = "project";
        return !!(key && $scope.pmsPermissions[key] && $scope.pmsPermissions[key].canEdit === 1);
    };

  $scope.filterstatuslist_count = function () {
    if ($scope.statuslists && $scope.statuslists.length > 0) {

        $scope.myprojectlist_bystatus = [];
        var situationProjects = ($scope.projectlist || []).filter(function (project) {
            return $scope.projectSituationFilter(project);
        });

        $scope.mytotalprojectlist = situationProjects.length;

        angular.forEach($scope.statuslists, function (status) {

            var total = situationProjects.filter(function (p) {
                return p.ProjectStatus == status.id;
            }).length;

            if (total > 0) {
                $scope.myprojectlist_bystatus.push({
                    statusid: status.id,
                    statusname: status.name,
                    statuscolor: status.colorname,
                    statusicon: status.statusicon,
                    total: total
                });
            }

        });
        console.log($scope.myprojectlist_bystatus);
    }
};
//EstimateDate: new Date(),
$scope.selectedPriority = "";

// Add Project popup priority selection
$scope.selectPopupPriority = function (priority) {
    if (!priority) return;
    $scope.selectedPriority = priority.id;
    if ($scope.newproject) {
        $scope.newproject.Priority = priority.id;
        $scope.newproject.PriorityName = priority.name;
    }
};
$scope.isPopupPrioritySelected = function (priority) {
//debugger
    if (!priority) return false;

    return Number($scope.newproject.Priority) === Number(priority.id);
};

    $scope.selectedStatus = "";

    // Add Project popup priority selection
    $scope.selectPopupStatus = function (status) {
        if (!status) return;
        $scope.selectedStatus = status.id;
        if ($scope.newproject) {
            $scope.newproject.ProjectStatus = status.id;
            $scope.newproject.StatusName = status.name;
        }
    };
$scope.isPopupStatusSelected = function (status) {

        if (!status) return false;
        return Number($scope.newproject.ProjectStatus) === Number(status.id);
    };
    
    $scope.projectTitle="Add Project"
    $scope.newproject = {
        id: 0,
        ProjectID:"",
        ProjectName: "",
        Owner: "",
        OwnerName: "",
        team:null,        
        ProjectStatus:null,
        StatusName:"",
        ProjectAddress1: "",
        ProjectAddress2: "",
        City: "",
        PostalCode: "",
        Stateid:null,
        StateName: "",
        Countryid:null,
        CountryName:"",
        Priority:null,
        PriorityName:"",
        Budget:null,
        Actual:null,
        AwardDate: null,
        StartDate: null,
        TargetDate: null,
        RequiredDrawing:false,
        ApprovalDrawing:false,
        DrawingApprovalDate:null,
        Weightage:"0",
        iscomplete:null,
        iscompletedate:new Date(),
        iscompletepercentage:null,
        checkprojectWeightage:null,        
        customerid: null,
        ClientName:"", 
        salesorderid: null,
        Estimateno:null,       
        hasmile: false,
        checkmilesameWeightage:false,
        hastask:false,
        checktasksameWeightage:false,
        hassubtask:false,
        checksubtasksameWeightage:false,
        tagids: "",
        iscountholydays: false,
        iscountweekoff: false,
        uploadprojectfiles:[],
         CriticalLabelId:"0",
        CriticalLabel:"",
        CriticalLabelcolor:""
    };
 $scope.newdefaultproject = {
        id: 0,
        ProjectID:"",
        ProjectName: "",
        Owner: "",
        OwnerName: "",
        team:null,        
        ProjectStatus:null,
        StatusName:"",
        ProjectAddress1: "",
        ProjectAddress2: "",
        City: "",
        PostalCode: "",
        Stateid:null,
        StateName: "",
        Countryid:null,
        CountryName:"",
        Priority:null,
        PriorityName:"",
        Budget:null,
        Actual:null,
        AwardDate: null,
        StartDate: null,
        TargetDate: null,
        RequiredDrawing:false,
        ApprovalDrawing:false,
        DrawingApprovalDate:null,
        Weightage:"0",
        iscomplete:null,
        iscompletedate:new Date(),
        iscompletepercentage:null,
        checkprojectWeightage:null,        
        customerid: null,
        ClientName:"", 
        salesorderid: null,
        Estimateno:null,       
        hasmile: false,
        checkmilesameWeightage:false,
        hastask:false,
        checktasksameWeightage:false,
        hassubtask:false,
        checksubtasksameWeightage:false,
     tagids: "",
     iscountholydays: false,
     iscountweekoff: false,
        uploadprojectfiles:[],
  CriticalLabelId:"0",
        CriticalLabel:"",
        CriticalLabelcolor:""

    };
$scope.openproject = function (pid,ptype) {
$scope.newproject = angular.copy($scope.newdefaultproject);
    if(pid!="0"&&pid!=""&&ptype=="update")
    {

     $scope.currentprojectid =pid;
     $scope.projecttype="update";
    if($scope.newproject)
    {
      let pindex = $scope.projectlist.findIndex(m => m.id ==pid);
    var prj=$scope.projectlist[pindex]
    if(prj)
    {$scope.newproject.id=prj.id;
                    $scope.newproject.ProjectID = prj.ProjectID;
                    $scope.newproject.ProjectName = prj.ProjectName;
                    $scope.newproject.customerid = prj.customerid;
                    $scope.newproject.ClientName = prj.ClientName;
                    try
                    {
                    var CLindex=$scope.customerlist.findIndex(m => m.customerid ==prj.customerid)
                    var clients=$scope.customerlist[CLindex];
                    // Customer list may still be loading: use the project's own client values.
                    if (!clients && prj.customerid && prj.customerid != "0") {
                        clients = { customerid: prj.customerid, ClientName: prj.ClientName };
                    }
                    $scope.selectclient(clients);
                    $scope.newproject.salesorderid = prj.salesorderid;
                    $scope.newproject.Estimateno = prj.Estimateno;
                    }
                    catch{}
                    $scope.newproject.Owner = prj.Owner;

                    if(prj.Owner!="")
                    $scope.newproject.OwnerName =$scope.getOwnerName(prj.Owner);

                    $scope.newproject.team = prj.team;
                    $scope.makeselectedteamlist($scope.newproject.team);
                    if(prj.Priority!="")
                    {          
                    $scope.newproject.Priority = parseInt(prj.Priority);
                    $scope.newproject.PriorityName = prj.priority_name;
                    }
                if(prj.ProjectStatus!="")
                 {          
                    $scope.newproject.ProjectStatus = parseInt(prj.ProjectStatus);
                    $scope.newproject.StatusName = prj.status_name;
                }
                    
                    $scope.newproject.ProjectAddress1 = prj.ProjectAddress1;
                    $scope.newproject.ProjectAddress2 = prj.ProjectAddress2;
                    $scope.newproject.City = prj.City;
                    $scope.newproject.CityName = prj.CityName;
                    $scope.newproject.PostalCode = prj.PostalCode;
                    $scope.newproject.Countryid = prj.Countryid;
                    $scope.newproject.CountryName = prj.CountryName;
                    try
                    {
                    var cindex=$scope.countrylist.findIndex(m => m.Countryid ==prj.Countryid)
                    var countryL=$scope.countrylist[cindex];

                    $scope.selectcountry(countryL);
                    }
                    catch{}
                    $scope.newproject.Stateid = prj.StateName;
                    $scope.newproject.StateName = prj.StateName2;                            
                    
                    
                    
                    $scope.newproject.Budget = prj.Budget;
                    $scope.newproject.Actual = prj.Actual;
                    try {

                        $scope.newproject.AwardDate =
                            moment(parseDate(prj.AwardDate)).format('DD/MM/YYYY');

                        $scope.newproject.StartDate =
                            moment(parseDate(prj.StartDate)).format('DD/MM/YYYY');

                        $scope.newproject.TargetDate =
                            moment(parseDate(prj.TargetDate)).format('DD/MM/YYYY');

                        setPickerDate('#AwardDate', $scope.newproject.AwardDate);
                        setPickerDate('#StartDate', $scope.newproject.StartDate);
                        setPickerDate('#TargetDate', $scope.newproject.TargetDate);

                    } catch (ex) {
                        console.log(ex);
                    }

                    $scope.newproject.ApprovalDrawing = (prj.ApprovalDrawing=="1"?true:false);
                    $scope.newproject.DrawingApprovalDate = parseDate(prj.DrawingApprovalDate);
                    $scope.newproject.Uploaddrawing = "";
                    $scope.newproject.Weightage = prj.Weightage;
                    $scope.newproject.RequiredDrawing = prj.isdrawing == "1" ? true : false;
                    $scope.newproject.iscountholydays = prj.iscountholydays == "1" ? true : false;
                    $scope.newproject.iscountweekoff = prj.iscountweekoff == "1" ? true : false;

                    //$scope.newproject.hasmile = prj.checkprojectWeightage=="1"?true:false;
                    $scope.newproject.hasmile = prj.hasmile=="1"?true:false;

                    $scope.newproject.checkmilesameWeightage = prj.checkprojectWeightage=="1"?true:false;
                    
                    $scope.newproject.hastask = prj.hastask=="1"?true:false;
                    $scope.newproject.checktasksameWeightage = prj.checktaskWeightage=="1"?true:false;
                    $scope.newproject.hassubtask = prj.hassubtask=="1"?true:false;
                    $scope.newproject.checksubtasksameWeightage = prj.checksubtaskWeightage=="1"?true:false;                    
                    $scope.newproject.teams = prj.teams;
                    $scope.newproject.leftdays = prj.leftdays;
                    $scope.newproject.txtdays = prj.txtdays;
                    $scope.newproject.owners = prj.owners;
                    $scope.newproject.pending_value = prj.pending_value;
                    if(prj.viewfiles.length>0)
                    {
                    angular.forEach(prj.viewfiles, function (f) {
                    $scope.uploadedFiles2.push({
                                            id:f.id,
                                            Uploadby:f.Uploadby,
                                            file: f.filedes,                        
                                            name: f.filedes,
                                            type: f.fext,
                                            size: '',
                                            fext: f.fext,
                                            data:f.fileurl
                                        });

                    });

                    }
                   
                    
    }
    }
    
    }
    else
    {
     $scope.projecttype="insert";
     $scope.newproject = angular.copy($scope.newdefaultproject);
    }
}
$scope.closesave = function ()
{
$scope.uploadedFiles2=[];
//document.getElementById('btnsave')
}

$scope.getprojectdetails_byid = function (pid) {

    if (!pid) return;

    myprojectService.getprojectdetailbyid(pid)
        .then(function (response) {

            var freshProject = response.data.d;

            // Date formatting
            freshProject.StartDate = parseDate(freshProject.StartDate);
            freshProject.TargetDate = parseDate(freshProject.TargetDate);
            freshProject.AwardDate = parseDate(freshProject.AwardDate);
            freshProject.DrawingApprovalDate = parseDate(freshProject.DrawingApprovalDate);
            freshProject.lastextendstartdate = parseDate(freshProject.lastextendstartdate);
            freshProject.lastextendenddate = parseDate(freshProject.lastextendenddate);
            if (freshProject.extendedlists && freshProject.extendedlists.length > 0) {

                angular.forEach(freshProject.extendedlists, function (e) {

                    e.fromdate = parseDate(e.fromdate);
                    e.todate = parseDate(e.todate);

                });

            }

            var pindex = $scope.projectlist.findIndex(function (x) {
                return x.id == pid;
            });

            if (pindex > -1) {

                // Replace existing project
                $scope.projectlist[pindex] = angular.copy(freshProject);

            } else {

                // Add at last index
                $scope.projectlist.push(
                    angular.copy(freshProject)
                );

            }

        })
        .catch(function (error) {

            alert("HTTP ERROR: " +
                (error.statusText || error.message || "Unknown error"));

            console.error(error);

        });

};
$scope.getprojectfiles = function (pid) {
if(pid=="")
return;
myprojectService.getprojectfiles(pid)
            .then(function (response) {

               var pindex=$scope.projectlist.findIndex(m=>m.id==pid)
               var prj2=$scope.projectlist[pindex];
                prj2.viewfiles = response.data.d;

           })
            .catch(function (error) {

                alert("HTTP ERROR: " + (error.statusText || error.message || "Unknown error"));
                console.error("HTTP ERROR:", error);

            })
            .finally(function () {

                //$scope.isLoading = false;

            });
}
$scope.addnewprojectdetails = function (id, fproject) {

    $scope.projectlist.unshift({

        id: id,
        ProjectID: fproject.ProjectID,
        ProjectName: fproject.ProjectName,
        Owner:fproject.Owner,
        team:fproject.team,
        ProjectStatus: String(fproject.ProjectStatus),
        status_name: fproject.StatusName,
        status_color: $scope.getstatuscolor(fproject.ProjectStatus),
        ProjectAddress1: fproject.ProjectAddress1 || "",
        ProjectAddress2: fproject.ProjectAddress2 || "",
        Completeaddress: fproject.Completeaddress || "",

        City: fproject.City || "",
        CityName: fproject.City || "",

        PostalCode: fproject.PostalCode || "",

        StateName: fproject.Stateid,
        StateName2: fproject.StateName,

        Countryid: fproject.Countryid,
        CountryName: fproject.CountryName,

        customerid: fproject.customerid,
        ClientName: fproject.ClientName,

        salesorderid: fproject.salesorderid,
        Estimateno: fproject.Estimateno,

        Priority: String(fproject.Priority),
        priority_name: fproject.PriorityName,
        priority_color: $scope.getprioritycolor(fproject.Priority),

        Budget: fproject.Budget || "0",
        Actual: fproject.Actual || "0",

        AwardDate: parseDate(fproject.AwardDate),
        StartDate: parseDate(fproject.StartDate),
        TargetDate: parseDate(fproject.TargetDate),
        isdrawing: (fproject.RequiredDrawing == true || fproject.RequiredDrawing == "1") ? "1" : "0",
        ApprovalDrawing:(fproject.ApprovalDrawing == true || fproject.ApprovalDrawing == "1") ? "1" : "0",


        DrawingApprovalDate: parseDate(fproject.DrawingApprovalDate),

        Uploaddrawing: "",
        Weightage: fproject.Weightage || "0",

        
checkprojectWeightage:(fproject.checkmilesameWeightage == true || fproject.checkmilesameWeightage == "1") ? "1" : "0",
hasmile:(fproject.hasmile == true || fproject.hasmile == "1") ? "1" :"0" ,
hastask:(fproject.hastask == true || fproject.hastask == "1") ? "1" :"0" ,
checktaskWeightage:(fproject.checktasksameWeightage == true || fproject.checktasksameWeightage == "1") ? "1" :"0" ,
hassubtask:(fproject.hassubtask == true || fproject.hassubtask == "1") ? "1" :"0" ,
checksubtaskWeightage:(fproject.checksubtasksameWeightage == true || fproject.checksubtasksameWeightage == "1") ? "1" :"0" ,

        leftdays: fproject.leftdays || "",
        txtdays: fproject.txtdays || "",

        complete_value: fproject.complete_value || "0",
        pending_value: fproject.pending_value || "0",

        totaldependency: fproject.totaldependency || "0",

        totalmile: fproject.totalmile || "0",
        completemile: fproject.completemile || "0",
        completemile_rate: fproject.completemile_rate || "0",

        pendingmile: fproject.pendingmile || "0",
        pendingmile_rate: fproject.pendingmile_rate || "0",

        totaltask: fproject.totaltask || "0",
        completetask: fproject.completetask || "0",
        completetask_rate: fproject.completetask_rate || "0",

        pendingtask: fproject.pendingtask || "0",
        pendingtask_rate: fproject.pendingtask_rate || "0",

        totalissue: fproject.totalissue || "0",
        completeissue: fproject.completeissue || "0",
        completeissue_rate: fproject.completeissue_rate || "0",

        pendingissue: fproject.pendingissue || "0",
        pendingissue_rate: fproject.pendingissue_rate || "0",

        extendedlists: angular.copy(fproject.extendedlists || []),

        tags: [],

        owners: angular.copy(fproject.owners || []),

        teams: angular.copy(
            fproject.teams || $scope.maketeamlist(fproject.team)
        ),

        viewfiles: []
    });
};
$scope.resetprojectdetails = function (fproject, tproject) {

    tproject.ProjectID = fproject.ProjectID;
    tproject.ProjectName = fproject.ProjectName;
    tproject.Owner = fproject.Owner;
    tproject.team = fproject.team;

    tproject.ProjectStatus = String(fproject.ProjectStatus);
    tproject.status_name = fproject.StatusName;
    tproject.status_color = $scope.getstatuscolor(fproject.ProjectStatus);

    tproject.ProjectAddress1 = fproject.ProjectAddress1 || "";
    tproject.ProjectAddress2 = fproject.ProjectAddress2 || "";
    tproject.Completeaddress = fproject.Completeaddress || "";

    tproject.City = fproject.City || "";
    tproject.CityName = fproject.City || "";
    tproject.PostalCode = fproject.PostalCode || "";

    tproject.StateName = fproject.Stateid || fproject.Stateid;
    tproject.StateName2 = fproject.StateName || fproject.StateName;

    tproject.Countryid = fproject.Countryid;
    tproject.CountryName = fproject.CountryName;

    tproject.ClientName = fproject.ClientName;

    tproject.Priority = String(fproject.Priority);
    tproject.priority_name = fproject.PriorityName;
    tproject.priority_color = $scope.getprioritycolor(fproject.Priority);

    tproject.Budget = fproject.Budget || "0";
    tproject.Actual = fproject.Actual || "0";

    tproject.AwardDate = parseDate(fproject.AwardDate);
    tproject.StartDate = parseDate(fproject.StartDate);
    tproject.TargetDate = parseDate(fproject.TargetDate);

   tproject.ApprovalDrawing = (fproject.ApprovalDrawing == true || fproject.ApprovalDrawing == "1") ? "1" : "0";

    tproject.DrawingApprovalDate = parseDate(fproject.DrawingApprovalDate);

    tproject.Uploaddrawing = "";
    tproject.Weightage = fproject.Weightage || "0";

tproject.isdrawing = (fproject.RequiredDrawing == true || fproject.RequiredDrawing == "1") ? "1" : "0";
tproject.checkprojectWeightage = (fproject.checkmilesameWeightage == true || checkmilesameWeightage.RequiredDrawing == "1") ? "1" : "0";
tproject.customerid = fproject.customerid;
tproject.salesorderid = fproject.salesorderid;
tproject.Estimateno = fproject.Estimateno;
tproject.hasmile = (fproject.hasmile == true || fproject.hasmile == "1") ? "1" : "0";
tproject.hastask = (fproject.hastask == true || fproject.hastask == "1") ? "1" : "0";
tproject.checktaskWeightage = (fproject.checktasksameWeightage == true || fproject.checktasksameWeightage == "1") ? "1" : "0";    
tproject.hassubtask = (fproject.hassubtask == true || fproject.hassubtask == "1") ? "1" : "0";   
tproject.checksubtaskWeightage = (fproject.checksubtasksameWeightage == true || fproject.checksubtasksameWeightage == "1") ? "1" : "0";    

    tproject.leftdays = fproject.leftdays || "";
    tproject.txtdays = fproject.txtdays || "";

    tproject.complete_value = fproject.complete_value || "0";
    tproject.pending_value = fproject.pending_value || "0";

    tproject.totaldependency = fproject.totaldependency || "0";

    tproject.totalmile = fproject.totalmile || "0";
    tproject.completemile = fproject.completemile || "0";
    tproject.completemile_rate = fproject.completemile_rate || "0";
    tproject.pendingmile = fproject.pendingmile || "0";
    tproject.pendingmile_rate = fproject.pendingmile_rate || "0";

    tproject.totaltask = fproject.totaltask || "0";
    tproject.completetask = fproject.completetask || "0";
    tproject.completetask_rate = fproject.completetask_rate || "0";
    tproject.pendingtask = fproject.pendingtask || "0";
    tproject.pendingtask_rate = fproject.pendingtask_rate || "0";

    tproject.totalissue = fproject.totalissue || "0";
    tproject.completeissue = fproject.completeissue || "0";
    tproject.completeissue_rate = fproject.completeissue_rate || "0";
    tproject.pendingissue = fproject.pendingissue || "0";
    tproject.pendingissue_rate = fproject.pendingissue_rate || "0";

    tproject.extendedlists = angular.copy(fproject.extendedlists || []);

    tproject.tags = [];
    tproject.owners = angular.copy(fproject.owners || []);

    tproject.teams = angular.copy(
        fproject.teams || $scope.maketeamlist(fproject.team)
    );

    tproject.viewfiles = [];
};
$scope.clearProjectForm = function () {

    $scope.newproject = angular.copy($scope.newdefaultproject);

    // Team
    $scope.selectedTeams = [];
    $scope.teamSearch = '';

    // Files
    $scope.uploadedFiles = [];
    $scope.uploadedFiles2 = [];

    // Dropdowns
    $scope.solist = [];

    $scope.showqcustomersDropdown = false;
    $scope.showSODropdown = false;
    $scope.showHeadDropdown = false;
    $scope.showCountryDropdown = false;
    $scope.showStateDropdown = false;

    // Clear file input
    var fileInput = document.getElementById('fileupload2');
    if (fileInput) {
        fileInput.value = '';
    }

    // Clear date controls
    $('#StartDate').val('');
    $('#TargetDate').val('');
    $('#AwardDate').val('');

    $scope.newproject.StartDate = null;
    $scope.newproject.TargetDate = null;
    $scope.newproject.AwardDate = null;

    // Reset daterangepicker
    ['#StartDate', '#TargetDate', '#AwardDate'].forEach(function (id) {

        var picker = $(id).data('daterangepicker');

        if (picker) {
            picker.setStartDate(moment());
            picker.setEndDate(moment());
        }
    });
};
    $scope.saveproject = function () {

        if (!$scope.newproject) {
            return;
        }

        var saveModel = angular.copy($scope.newproject);

        if (saveModel.StartDate === null ||
            saveModel.StartDate === undefined ||
            saveModel.StartDate === '') {

            alert("Please Enter Start Date First!!");
            return;
        }

        var toast = document.getElementById("toast");
        if (toast) {
            toast.classList.add("toast-position-20");
        }

        var toasterror = document.getElementById("toasterror");
        if (toasterror) {
            toasterror.classList.add("toast-position-20");
        }

        document.getElementById("pnlimg").style.display = "block";
        document.getElementById("divsub").style.display = "none";

        // Existing save process. It runs only after duplicate validation completes.
        function continueProjectSave() {

            // Send calendar dates as ISO date-only strings. Never send JavaScript Date
            // objects or ISO timestamps for these fields.
            saveModel.StartDate = toServerDateOnly(saveModel.StartDate);
            saveModel.AwardDate = toServerDateOnly(saveModel.AwardDate);
            saveModel.TargetDate = toServerDateOnly(saveModel.TargetDate);
            saveModel.DrawingApprovalDate = toServerDateOnly(saveModel.DrawingApprovalDate);

            if (!$scope.newproject.Estimateno ||
                $scope.newproject.Estimateno.toString().trim() === '') {
                $scope.newproject.salesorderid = "0";
                $scope.newproject.Estimateno = "";
            }

            let resultnew;
            resultnew = myprojectService.saveproject(saveModel, $scope.projecttype);

            if (resultnew) {
                resultnew.then(function (res) {

                    if (res.data.d != "0") {
                        var pid = res.data.d;

                        $scope.uploadFilesToServer2(pid)
                            .then(function () {

                                $scope.showToast(
                                    ($scope.projecttype == "update"
                                        ? "✔ Project updated successfully."
                                        : "✔ Project saved successfully."),
                                    4000
                                );

                                document.getElementById("pnlimg").style.display = "none";
                                document.getElementById("divsub").style.display = "block";

                                $scope.getprojectdetails_byid(pid);
                                $scope.clearProjectForm();
                                document.getElementById('btnsave').click();

                                $timeout(function () {
                                    if (toast) {
                                        toast.classList.remove("toast-position-20");
                                    }
                                    if (toasterror) {
                                        toasterror.classList.remove("toast-position-20");
                                    }
                                }, 4200);
                            })
                            .catch(function (err) {

                                document.getElementById("pnlimg").style.display = "none";
                                document.getElementById("divsub").style.display = "block";
                                $scope.showToast("File upload failed.", 4000);

                                $timeout(function () {
                                    if (toast) {
                                        toast.classList.remove("toast-position-20");
                                    }
                                    if (toasterror) {
                                        toasterror.classList.remove("toast-position-20");
                                    }
                                }, 4200);
                            });
                    }
                })
                .catch(function (err) {

                    document.getElementById("pnlimg").style.display = "none";
                    document.getElementById("divsub").style.display = "block";
                    $scope.showToast(
                        "Error - " +
                        (err.message || err.data || "Unknown error"),
                        4000
                    );

                    $timeout(function () {
                        if (toast) {
                            toast.classList.remove("toast-position-20");
                        }
                        if (toasterror) {
                            toasterror.classList.remove("toast-position-20");
                        }
                    }, 4200);
                });
            }
        }

        var projectCode = (saveModel.ProjectID || "").toString().trim();

        if (projectCode !== "") {
            var resultduplicate = myprojectService.checkduplicate(
                projectCode,
                saveModel.id || "0"
            );

            if (resultduplicate) {
                resultduplicate
                    .then(function (res) {

                        var duplicateResult = "-1";

                        if (res &&
                            res.data &&
                            res.data.d !== null &&
                            res.data.d !== undefined) {
                            duplicateResult = res.data.d.toString().trim();
                        }

                        if (duplicateResult === "1") {
 $timeout(function () {
                                    if (toast) {
                                        toast.classList.remove("toast-position-20");
                                    }
                                    if (toasterror) {
                                        toasterror.classList.remove("toast-position-20");
                                    }
                                }, 4200);
                            $scope.showToast(
                                "✔ This Project Id Already Exist.",
                                4000
                            );

                            document.getElementById("pnlimg").style.display = "none";
                            document.getElementById("divsub").style.display = "block";

                            return;
                        }

                        if (duplicateResult !== "0") {
                            $scope.showToast(
                                "Unable to check duplicate Project Id. Please try again.",
                                4000
                            );

                            document.getElementById("pnlimg").style.display = "none";
                            document.getElementById("divsub").style.display = "block";
                            return;
                        }

                        // No duplicate found. Continue the existing save process.
                        continueProjectSave();
                    })
                    .catch(function (error) {

                        console.error(
                            "Error while checking duplicate project:",
                            error
                        );

                        $scope.showToast(
                            "Unable to check duplicate Project Id. Please try again.",
                            4000
                        );

                        document.getElementById("pnlimg").style.display = "none";
                        document.getElementById("divsub").style.display = "block";
                    });
            }

            // Stop here so save cannot run before the duplicate response.
            return;
        }

        // Blank Project Id keeps the existing auto-generated Project Id working.
        continueProjectSave();
    }

 $scope.saveproject_clone = function (prj) {
     var toast = document.getElementById("toast");
     if (toast) {
         toast.classList.add("toast-position-20");
     }
     var toasterror = document.getElementById("toasterror");
     if (toasterror) {
         toasterror.classList.add("toast-position-20");
     }
Swal.fire({
    title: ('Clone of Project?'),
        
    text: ('Are you sure you want to save clone of this project?'),

    icon: 'question',
    showCancelButton: true,
    confirmButtonColor: '#3085d6',
    cancelButtonColor: '#6c757d',

    confirmButtonText: ('Yes, Save'),

    cancelButtonText: 'Cancel'

}).then(function (result) {

        if (!result.isConfirmed)
            return;
        if (!$scope.newproject)
        return;  
    
$scope.newproject.StartDate = formatDateOnly(prj.StartDate);
$scope.newproject.AwardDate = formatDateOnly(prj.AwardDate);
$scope.newproject.TargetDate = formatDateOnly(prj.TargetDate);
$scope.newproject.Countryid=prj.Countryid;
$scope.newproject.CountryName=prj.CountryName;
$scope.newproject.Stateid=prj.StateName;
$scope.newproject.StateName=prj.StateName2;

$scope.newproject.Priority=prj.Priority;
$scope.newproject.PriorityName=prj.priority_name;
$scope.newproject.ProjectStatus=$scope.defaultstatus(); 
try
{
$scope.newproject.StatusName=$scope.getstatusname($scope.newproject.ProjectStatus); 
}
catch{}
$scope.newproject.Owner=prj.Owner; 
$scope.newproject.team=prj.team;
$scope.newproject.RequiredDrawing=(prj.isdrawing=="1"?true:false);  
$scope.newproject.ApprovalDrawing=(prj.ApprovalDrawing=="1"?true:false);
$scope.newproject.hasmile=(prj.hasmile=="1"?true:false); 
$scope.newproject.checkmilesameWeightage=(prj.checkprojectWeightage=="1"?true:false); 
$scope.newproject.hastask=(prj.hastask=="1"?true:false); 
$scope.newproject.checktasksameWeightage=(prj.checktaskWeightage=="1"?true:false); 
$scope.newproject.hassubtask=(prj.hassubtask=="1"?true:false); 
$scope.newproject.checksubtasksameWeightage=(prj.checksubtaskWeightage=="1"?true:false);

 let resultnew;
        var cloneSaveModel = angular.copy($scope.newproject);
        cloneSaveModel.StartDate = toServerDateOnly(cloneSaveModel.StartDate);
        cloneSaveModel.AwardDate = toServerDateOnly(cloneSaveModel.AwardDate);
        cloneSaveModel.TargetDate = toServerDateOnly(cloneSaveModel.TargetDate);
        cloneSaveModel.DrawingApprovalDate = toServerDateOnly(cloneSaveModel.DrawingApprovalDate);
        resultnew = myprojectService.saveproject(cloneSaveModel,'insert');
        
        if (resultnew) {
            resultnew.then(function (res) {
                if (res.data.d != "0") {
                var pid=res.data.d;
 $scope.showToast(
        ( "✔ Clone saved successfully."),
        4000
    ); 
                //$scope.newproject.ProjectID="P"+pid;               
                //$scope.showToast(($scope.projecttype=="update" ? "✔ Project updated successfully." : "✔ Project saved successfully."), 4000);
                //$scope.addnewprojectdetails(pid,$scope.newproject);
                $scope.getprojectdetails_byid(pid);
                 
                $scope.clearProjectForm();
                                
                 
                }
                //console.log(res);
            })
                .catch(function (err) {
                    if (toast) {
                        toast.classList.add("toasterror-position-20");
                    }
                    if (toasterror) {
                        toasterror.classList.add("toasterror-position-20");
                    }
$scope.showToast(
            "Error - " +
            (err.message || err.data || "Unknown error"),
            4000
);
                   

                });
        }
        })
            .catch(function (err) {
                

                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'Something went wrong.'
                });
            });
    }
 
function resetPicker(id) {

    var picker = $(id).data('daterangepicker');

    if (picker) {

        var today = moment();

        picker.setStartDate(today);
        picker.setEndDate(today);
        
        $(id).val('');
    }
}
    $scope.isshowall = "0";
    $scope.chkall = false;
    $scope.showfileslist = [];
    $scope.currentprojectid = "0";
    
    $scope.customerlist = [];
    $scope.solist = [];
    $scope.tagslist = [];
    $scope.owners = [];
    $scope.projectteams = [];
    $scope.prioritylists = [];
    $scope.statuslists = [];
    $scope.currentlist = [];
    $scope.currentlist_extendlist = [];
    $scope.extendObj = {
        fromdate: "",
        todate: ""
    };

    $scope.currentFtype ="";
    $scope.currentFId = "";
    $scope.currentIndex = 0;
    $scope.selectedOwnerIds = [];
    $scope.ownersnew = {
        id: "",
        name: "",
        profileimage: "",
        shortname: ""
    };
    //#region add shorting
    $scope.sortColumn = "id";
    $scope.reverseSort = false;
    $scope.sortData = function (column) {
        $scope.reverseSort = ($scope.sortColumn == column) ? !$scope.reverseSort : false;
        $scope.sortColumn = column;
    };
    $scope.getSortClass = function (column) {
        if ($scope.sortColumn == column) {
            return $scope.reverseSort ? "↓" : "↑";
        }
        return "";
    };
    //#endregion
//#region manage teams details
    $scope.allprojectTeams = [];
    $scope.selectedfilterTeams = [];
    $scope.selectedfilterTeams2 = [];
    $scope.selectedTeams = [];
    $scope.teamSearch = "";
    $scope.filterTeamList = function () {
        $scope.selectedfilterTeams = $scope.projectteams.filter(function (item) {
            var alreadySelected = $scope.selectedTeams.some(function (s) {
                return s.id == item.id;
            });
            // Keep all unselected employees available for the next search.
            // The template applies the current search without shrinking this source.
            return !alreadySelected;
        });

        //$scope.supplierHighlightIndex = $scope.filterTagList.length > 0 ? 0 : -1;
    };
    $scope.filterTeamList2 = function () {
        var search = ($scope.teamSearch || '').toLowerCase();
        $scope.selectedfilterTeams2 = $scope.owners.filter(function (item) {
            var alreadySelected = $scope.selectedOwnerIds.some(function (s) {
                return s.id == item.id;
            });
            var match = item.name.toLowerCase().indexOf(search) !== -1;
            return !alreadySelected && match;
        });

        //$scope.supplierHighlightIndex = $scope.filterTagList.length > 0 ? 0 : -1;
    };
    
   
    $scope.addTeam = function (team) {

        var exists = $scope.selectedTeams.some(function (t) {
            return t.id == team.id;
        });

        if (!exists) {

            $scope.selectedTeams.push({
                id: team.id,
                name: team.name
            });

            $scope.newproject.team = $scope.selectedTeams
                .map(function (t) {
                    return t.id;
                })
                .join(',');

            $scope.teamSearch = '';
            $scope.filterTeamList();
        }
    };
    $scope.removeTeam = function (team) {
        $scope.selectedTeams = $scope.selectedTeams.filter(function (t) {
            return t.id != team.id;
        });

        $scope.newproject.team = $scope.selectedTeams
            .map(function (t) {
                return t.id;
            })
            .join(',');

        $scope.filterTeamList();
    };
    //#endregion
     $scope.highlightMatch = function (text, search) {
        if (!search || search.trim() === "") return text;
        var escapedSearch = search.replace(/[.*+?^${}()|[\]\\]/g, "\\$&");
        let pattern = new RegExp("(" + escapedSearch + ")", "gi");
        return text.replace(pattern, "<b>$1</b>");
    };
    //#region manage client details
$scope.checkSelectAllOrType = function (event) {
        let input = event.target;

        if (input.selectionStart === 0 &&
            input.selectionEnd === input.value.length &&
            input.value.length > 0) {
            $scope.showqcustomersDropdown = true;
            $scope.$applyAsync();
            return;
        }

        $scope.filterCountry = function (item) {
            if (!$scope.newAddress.CountrySearch) return true;
            let q = $scope.newAddress.CountrySearch.toLowerCase();
            return item.name.toLowerCase().includes(q);
        };
    };
   var soListRequestId = 0;
   $scope.selectclient = function (clientid) {

    var previousCustomerId = $scope.newproject.customerid;

    $scope.newproject.customerid = clientid.customerid;
    $scope.newproject.ClientName = clientid.ClientName;

    $scope.showqcustomersDropdown = false;

    // A different client: the previous client's Sales Order no longer applies.
    // (Edit mode sets the saved Sales Order again right after this call.)
    if (String(previousCustomerId || '') !== String(clientid.customerid || '')) {
        $scope.newproject.salesorderid = null;
        $scope.newproject.Estimateno = '';
    }

    // Do not show the previous client's Sales Orders while the new list loads,
    // and ignore a slower response from an earlier client selection.
    $scope.solist = [];
    var requestId = ++soListRequestId;

    var mode = ($scope.projecttype == "update") ? "edit" : "";

    myprojectService.getsolist(clientid.customerid, mode)
        .then(function (response) {

            if (requestId !== soListRequestId) return;
            $scope.solist = (response.data && response.data.d) || [];

        })
        .catch(function (error) {


            alert("HTTP ERROR: " +
                (error.statusText ||
                 error.message ||
                 "Unknown error"));

            console.error("HTTP ERROR:", error);

        })
        .finally(function () {

            //$scope.isLoading = false;

        });
};
    $scope.selectSO = function (sales) {
        //alert(sales.salesorderid);
        $scope.newproject.salesorderid = sales.salesorderid;
        $scope.newproject.Estimateno = sales.Estimateno;
        $scope.showSODropdown = false;
        

    };
    $scope.selectowner = function (owner) {
        //alert(sales.salesorderid);
        $scope.newproject.Owner = owner.id;
        $scope.newproject.OwnerName = owner.name;
        $scope.showHeadDropdown = false;
        

    };
//#endregion
    //#region bind owner details
    $scope.getSelectedOwners = function () {
        return $scope.owners.filter(o => $scope.selectedOwnerIds.includes(o.id));
    };
    $scope.selectOwner = function (c, o) {
                
                

        myprojectService.UpdateP_Owner(c.id, o.id)
            .then(function (response) {

                c.Owner = o.id;   

            })
            .catch(function (error) {

                alert("HTTP ERROR: " + (error.statusText || error.message || "Unknown error"));
                console.error("HTTP ERROR:", error);

            })
            .finally(function () {

                //$scope.isLoading = false;

            });
        
    };
    
    // PERFORMANCE: the per-row owner menu renders the full owner list only after
    // that row's dropdown is opened the first time (then it stays rendered).
    $scope.ownerMenuOpened = {};
    $scope.focusOwnerSearch = function ($event, c) {
        if (c && c.id !== undefined && c.id !== null) {
            $scope.ownerMenuOpened[c.id] = true;
        }

        setTimeout(function () {

            // find current dropdown container
            var dropdown = $event.target.closest(".dropdown");

            if (!dropdown) return;

            // find input inside it
            // The row search box uses .search__input (no .owner-search exists), so the
            // focus never happened; keep .owner-search first for compatibility.
            var input = dropdown.querySelector(".owner-search") ||
                dropdown.querySelector(".dropdown-menu input[type='text']");

            if (input) {
                input.focus();
                input.value = input.value; // helps cursor position fix in some browsers
            }

        }, 200); // important delay for bootstrap render
    };
    $scope.getOwnerName = function (ownerid) {
        var owner = $scope.owners.find(function (o) {
            return o.id == ownerid;
        });

        return owner ? owner.name : '';
    };
$scope.getOwnerProfilenew = function (ownerid) {
//debugger
    var defaultImage = 'Image/user.jpg';
    try {
        if (!ownerid || !$scope.owners || !$scope.owners.length) {
            return defaultImage;
        }

        var owner = $scope.owners.find(function (o) {
            return o.id == ownerid;
        });

        return (owner && owner.profileimage && owner.profileimage != '')
            ? owner.profileimage
            : defaultImage;
    }
    catch{
        return defaultImage;
    }
};
$scope.getprioritycolor = function (priorityid) {
        var priorityC = $scope.prioritylists.find(function (o) {
            return o.id == priorityid;
        });

        return priorityC ? priorityC.colorname : '';
    };
$scope.getstatuscolor = function (statusid) {
        var statusC = $scope.statuslists.find(function (o) {
            return o.id == statusid;
        });

        return statusC ? statusC.colorname : '';
    };
$scope.getstatusname = function (statusid) {
        var statusN = $scope.statuslists.find(function (o) {
            return o.id == statusid;
        });

        return statusN ? statusN.name : '';
    };
$scope.defaultstatus = function () {
        var statusC = $scope.statuslists.find(function (o) {
            return o.isdefault =="1";
        });

        return statusC ? statusC.id : '';
    };
    //#endregion
    //#region bind team
    $scope.closelist = function() {
if($scope.showOwnerDropdown==true)
    $scope.showOwnerDropdown = false;
};
     $scope.getRemainingOwners = function (owners) {
        if (!owners || owners.length <= 2) return "";

        return owners.slice(2).map(function (o) {
            return o.name;
        }).join(", ");
    };
    $scope.getRemainingTags = function (tags) {
        if (!tags || tags.length <= 1) return "";

        return tags.slice(1).map(function (o) {
            return o.tag_name;
        }).join(", ");
    };

    function removeProjectTeamFromClient(project, employeeId) {
        if (!project) return;

        project.teams = (project.teams || []).filter(function (member) {
            return String(member.id) !== String(employeeId);
        });

        project.teams_all = (project.teams_all || []).filter(function (member) {
            return String(member.id) !== String(employeeId);
        });

        project.team = String(project.team || '')
            .split(',')
            .map(function (value) { return value.trim(); })
            .filter(function (value, index, list) {
                return value !== ''
                    && String(value) !== String(employeeId)
                    && list.indexOf(value) === index;
            })
            .join(',');
    }

    $scope.removeteams = function (obj1, obj2) {

        var projectteams = {
            id: obj1.id,
            Team: obj2.id,
            
        };
        let resultnew;
        resultnew = myprojectService.RemoveP_Team(projectteams);
        if (resultnew) {
            resultnew.then(function (res) {
                if (res.data.d == "1") {
                    removeProjectTeamFromClient(obj1, obj2.id);
                    $scope.$applyAsync();
                }
                //console.log(res);
            })
                .catch(function (err) {
                    //console.error(err);
                    //alert("Update failed");
                });
        }

        
    };
 $scope.removeteams_cumulative = function (obj1, obj2) {

        var projectteams = {
            id: obj1.id,
            Team: obj2.id,
            
        };
        let resultnew;
        //resultnew = myprojectService.RemoveP_Team(projectteams);
        resultnew = myprojectService.RemoveP_CumulativeTeam(projectteams);
        if (resultnew) {
            resultnew.then(function (res) {
                if (res.data.d == "1") {
                    removeProjectTeamFromClient(obj1, obj2.id);
                    $scope.$applyAsync();
                }
                //console.log(res);
            })
                .catch(function (err) {
                    //console.error(err);
                    //alert("Update failed");
                });
        }

        
    };
    $scope.removeOwner = function (ownerId) {
        var tasklist = $scope.currentlist;
        if (tasklist.teams_all) {
            var index = tasklist.teams_all.findIndex(m => m.id == ownerId);
            if (index > -1) {
                var selectteam = tasklist.teams_all[index];
                $scope.removeteams_cumulative($scope.currentlist, selectteam);
                var indexD = $scope.selectedOwnerIds.indexOf(ownerId);
                if (indexD > -1) {
                    $scope.selectedOwnerIds.splice(indexD, 1);

                }
            }
        }
        
        //var index = $scope.selectedOwnerIds.indexOf(ownerId);
        //if (index > -1) {
        //    $scope.selectedOwnerIds.splice(index, 1);
            
        //        $scope.removeTeamToServer($scope.currentlist, $scope.selectedOwnerIds, ownerId);
               

        //}
        
        $scope.filterTeamList2();
    };
    $scope.removeOwner_old = function (ownerId) {
        var index = $scope.selectedOwnerIds.indexOf(ownerId);
        if (index > -1) {
            $scope.selectedOwnerIds.splice(index, 1);
            
                $scope.removeTeamToServer($scope.currentlist, $scope.selectedOwnerIds, ownerId);
               

        }
        
        $scope.filterTeamList2();
    };
    
    $scope.removeTeamToServer = function (obj, selected_obj, newid) {
        
        var ids = selected_obj.join(",");
        let resultnew;

        
        resultnew = myprojectService.UpdateP_Team(obj.id, ids, newid, 'remove');
        
        if (resultnew) {
            resultnew.then(function (res) {
                if (res.data.d == "1") {
                    removeProjectTeamFromClient(obj, newid);
                    $scope.getRemainingOwners(obj.teams);
                }
                //console.log(res);
            })
                .catch(function (err) {
                    //console.error(err);
                    //alert("Update failed");
                });
        }
    };
    $scope.removeTeam_byid = function (obj, selected_obj, newid) {

        var ids = selected_obj.join(",");
        let resultnew;


        resultnew = myprojectService.UpdateP_Team(obj.id, ids, newid, 'remove');

        if (resultnew) {
            resultnew.then(function (res) {
                if (res.data.d == "1") {
                    removeProjectTeamFromClient(obj, newid);
                }
                //console.log(res);
            })
                .catch(function (err) {
                    //console.error(err);
                    //alert("Update failed");
                });
        }
    };
    $scope.toggleOwner = function (owner) {
        var index = $scope.selectedOwnerIds.indexOf(owner.id);

        if (index > -1) {
           
        } else {

            $scope.selectedOwnerIds.push(owner.id); // select
            $scope.updateTeamToServer($scope.currentlist, $scope.selectedOwnerIds, owner.id);
            
        }
        $scope.filterTeamList2();
        
    };
    $scope.updateTeamToServer = function (obj, selected_obj, newid) {
        if (!selected_obj || selected_obj.length === 0) {
            alert("No owners selected");
            return;
        }
        var ids = selected_obj.join(",");
        let resultnew;

       
        resultnew = myprojectService.UpdateP_Team(obj.id, ids, newid, 'add');
        
        if (resultnew) {
            resultnew.then(function (res) {
                if (res.data.d == "1") {
                    var addlist = $scope.owners.find(m => m.id == newid);
                    if (addlist) {
                        let newrow = angular.copy($scope.ownersnew);
                        newrow.id = addlist.id;
                        newrow.name = addlist.name;
                        newrow.profileimage = addlist.profileimage;
                        newrow.shortname = addlist.shortname;
                        var directTeamIds = String(obj.team || '')
                            .split(',')
                            .map(function (value) { return value.trim(); })
                            .filter(function (value, index, list) {
                                return value !== '' && list.indexOf(value) === index;
                            });

                        if (directTeamIds.indexOf(String(newrow.id)) === -1) {
                            directTeamIds.push(String(newrow.id));
                        }
                        obj.team = directTeamIds.join(',');
                        obj.teams = obj.teams || [];
                        obj.teams_all = obj.teams_all || [];

                        if (!obj.teams.some(function (member) {
                            return String(member.id) === String(newrow.id);
                        })) {
                            obj.teams.push(angular.copy(newrow));
                        }

                        if (!obj.teams_all.some(function (member) {
                            return String(member.id) === String(newrow.id);
                        })) {
                            obj.teams_all.push(angular.copy(newrow));
                        }
                    }
                }
                //console.log(res);
            })
                .catch(function (err) {
                    //console.error(err);
                    //alert("Update failed");
                });
        }
    };
    $scope.loadSrcOwnersnew = function (c,type,index) {

        // Determine reference ID based on type
        let refId;
        let reftype = type;
        $scope.modalTitle = "Add teams";
        $scope.loaduserlist_byrefid(refId, 'project');
        // Set scope variables        
       
        $scope.currentFId = c.id;
        $scope.currentlist = c;
        $scope.currentIndex = index;
        $scope.selectedItem = c;
        $scope.selectedIndex = index;
        // Populate selected IDs
        $scope.selectedOwnerIds = [];
        //if (c.teams) {
        //    $scope.selectedOwnerIds = c.teams.map(t => t.id);
        //}
        if (c.teams_all) {
            $scope.selectedOwnerIds = c.teams_all.map(t => t.id);
        }
        $scope.filterTeamList2();
        
    };
    $scope.loadtags = function (c,frameId, url) {

        $scope.currentprojectid = c.id;
        $scope.currentlist = c;       
        // safe counter
        $scope.popupCounter = ($scope.popupCounter || 0) + 1;

        // modal title
        $scope.modalTitle = "Add new tag";

        // build URL
        let finalUrl = "";

        if (url.indexOf("aspx?") !== -1) {
            finalUrl = url + "&ctype=ang&count=" + $scope.popupCounter;
        } else {
            finalUrl = url + "?ctype=ang&count=" + $scope.popupCounter;
        }

        // set iframe src
        let frame = document.getElementById(frameId);
        if (frame) {
            frame.src = "";
            frame.src = finalUrl;
        }

    };
    $scope.loadsrc = function (c, frameId, url,addtype) {

        $scope.projectTitle = "Add Project";
        $scope.currentprojectid = c.id;
        $scope.currentlist = c;
        $scope.currentFtype = addtype;
        // safe counter
        $scope.popupCounter = ($scope.popupCounter || 0) + 1;

        // modal title
        switch (addtype) {
            case "milestone":
                $scope.modalTitle = c.ProjectName;
                break;
            case "task":
                $scope.modalTitle = c.ProjectName;
                break;
            case "issue":
                $scope.modalTitle = c.ProjectName;
                break;
            default:
                $scope.modalTitle = c.ProjectName;
        }
        

        // build URL
        let finalUrl = "";

        if (url.indexOf("aspx?") !== -1) {
            finalUrl = url + "&ctype=ang&count=" + $scope.popupCounter;
        } else {
            finalUrl = url + "?ctype=ang&count=" + $scope.popupCounter;
        }

        // set iframe src
        let frame = document.getElementById(frameId);
        if (frame) {
            frame.src = "";
            frame.src = finalUrl;
        }

    };
    $scope.closepopup_canvas211 = function () {

        try {
            var btn = (window.parent && window.parent.document)
                ? window.parent.document.getElementById("btnclosemainpopup2")
                : document.getElementById("btnclosemainpopup2");
            if (btn == null) {
                btn = document.getElementById("btnclosemainpopup2");
            }
            if (btn) {
                btn.click();
            }
        }
        catch
        {

        }
        
        if (!$scope.currentlist) return;
        let obj = $scope.currentlist;
        let utype = $scope.currentFtype;        
        let resultnew = null;
        // API call based on type
        if (utype === 'milestone') {
            resultnew = myprojectService.getissuesdetails(obj.id, utype);
        }
        else if (utype === 'task') {
            resultnew = myprojectService.getissuesdetails(obj.id, utype);
        }
        else if (utype === 'issue') {
            resultnew = myprojectService.getissuesdetails(obj.id, utype);
        }

        if (!resultnew) return;

        resultnew.then(function (res) {

            // res.data.d is expected as object
            let getlist = res.data.d;
            if (getlist) {
                obj.totalissue = getlist.totalissue || 0;
                obj.completeissue = getlist.completeissue || 0;
                obj.pendingissue = getlist.pendingissue || 0;

                // Calculate percentages safely
                obj.completeissue_rate = getlist.completeissues_rate || 0;
                obj.pendingissue_rate = getlist.pendingissues_rate || 0;
            }

        })
            .catch(function (err) {
                console.error(err);
                //alert("Update failed");
            })
            .finally(function () {

            });

    };
    $scope.loaduserlist_byrefid = function (refid, usertype) {

        refid = "0";
        myprojectService.getuserlist_byrefid(refid, usertype, "team")
            .then(function (response) {
                // assign data
                $scope.owners = response.data.d;
                $scope.selectedfilterTeams2 = angular.copy($scope.owners);
                
            })
            .catch(function (error) {
                alert("HTTP ERROR: " + (error.statusText || error.message || "Unknown error"));
                console.error("HTTP ERROR:", error);
            })
            .finally(function () {

            });
    };
    $scope.loadsrc_ang = function (frameId,url) {

        let frame = document.getElementById(frameId);

        if (frame) {
            frame.src = "about:blank";   // force reset

            setTimeout(function () {
                frame.src = url + "&r=" + Date.now();
            }, 50);
        }

    };
    //#endregin
    //#region country and state details
    $scope.selectcountry = function (country) {


        $scope.newproject.Countryid = country.Countryid;
        $scope.newproject.CountryName = country.CountryName;
        $scope.showCountryDropdown = false;
        myprojectService.getstatelist(country.Countryid)

            .then(function (response) {
                $scope.statelist = response.data.d;


            })
            .catch(function (error) {

                alert("HTTP ERROR: " + (error.statusText || error.message || "Unknown error"));
                console.error("HTTP ERROR:", error);

            })
            .finally(function () {

                //$scope.isLoading = false;

            });

    };
    $scope.selectstate = function (state) {


        $scope.newproject.Stateid = state.Stateid;
        $scope.newproject.StateName = state.StateName;
        $scope.showStateDropdown = false;
        

    };
    //#endregion
    //#load maters
    $scope.loadcountrylist = function () {

        //$scope.isLoading = true;

        myprojectService.getcountrylist()
            .then(function (response) {
                // assign data
                $scope.countrylist = response.data.d;

            })
            .catch(function (error) {

                alert("HTTP ERROR: " + (error.statusText || error.message || "Unknown error"));
                console.error("HTTP ERROR:", error);

            })
            .finally(function () {

                //$scope.isLoading = false;

            });
    };
   
$scope.loadcustomerlist = function () {

        //$scope.isLoading = true;

        myprojectService.getcustomerlist()
            .then(function (response) {
                // assign data
                $scope.customerlist = response.data.d;

            })
            .catch(function (error) {

                alert("HTTP ERROR: " + (error.statusText || error.message || "Unknown error"));
                console.error("HTTP ERROR:", error);

            })
            .finally(function () {

                //$scope.isLoading = false;

            });
    };
$scope.loadsolist = function (customerid) {

        //$scope.isLoading = true;

        myprojectService.getsolist(customerid)
            .then(function (response) {
                // assign data
                $scope.solist = response.data.d;

            })
            .catch(function (error) {

                alert("HTTP ERROR: " + (error.statusText || error.message || "Unknown error"));
                console.error("HTTP ERROR:", error);

            })
            .finally(function () {

                //$scope.isLoading = false;

            });
};
$scope.maketeamlist = function (teams) {

    if (!teams) {
        return [];
    }

    var ids = teams.split(',');

    var teamlist = $scope.projectteams.filter(function (t) {
        return ids.indexOf(t.id.toString()) > -1;
    });

    //$scope.selectedTeams = angular.copy(teamlist);
    return teamlist;
};
$scope.makeselectedteamlist = function (teams) {

    $scope.selectedTeams = [];

    if (!teams) {
        return;
    }

    var ids = teams.split(',');

    angular.forEach($scope.projectteams, function (team) {

        if (ids.indexOf(team.id.toString()) > -1) {

            $scope.selectedTeams.push({
                id: team.id,
                name: team.name
            });
        }
    });

};
    $scope.loadteamlist = function () {

        //$scope.isLoading = true;

        myprojectService.getteamlist()
            .then(function (response) {

                // assign data
                $scope.projectteams = response.data.d;
                $scope.selectedfilterTeams = angular.copy($scope.projectteams);
            })
            .catch(function (error) {

                alert("HTTP ERROR: " + (error.statusText || error.message || "Unknown error"));
                console.error("HTTP ERROR:", error);

            })
            .finally(function () {

                //$scope.isLoading = false;

            });
    };
    $scope.loaduserlist = function () {
        //alert("load users");
        //$scope.isLoading = true;
        
        myprojectService.getuserlist()
            .then(function (response) {

                // Same employee master is used for Owner and Team.
                // Reuse this response instead of calling binduserlist twice on page load.
                $scope.owners = response.data.d || [];
                $scope.projectteams = angular.copy($scope.owners);
                $scope.selectedfilterTeams = angular.copy($scope.projectteams);

            })
            .catch(function (error) {

                alert("HTTP ERROR: " + (error.statusText || error.message || "Unknown error"));
                console.error("HTTP ERROR:", error);

            })
            .finally(function () {

                //$scope.isLoading = false;

            });
    };
    $scope.loadstatuslist = function () {

        

        myprojectService.getstatuslist()
            .then(function (response) {

                // assign data
                $scope.statuslists = response.data.d;

                // Project data may now arrive before the status list (it is requested first).
                // Rebuild the status count chips once both are available.
                if ($scope.projectDataReady) {
                    $scope.filterstatuslist_count();
                }

            })
            .catch(function (error) {

                alert("HTTP ERROR: " + (error.statusText || error.message || "Unknown error"));
                console.error("HTTP ERROR:", error);

            })
            .finally(function () {

                //$scope.isLoading = false;

            });
    };
    $scope.loadProjectSituationList = function () {
        myprojectService.getProjectSituationList()
            .then(function (response) {
                $scope.projectSituationLists = response.data.d || [];
            })
            .catch(function (error) {
                console.error("Project Situation list could not be loaded:", error);
            });
    };
    $scope.loadprioritylist = function () {

        //$scope.isLoading = true;

        myprojectService.getprioritylist()
            .then(function (response) {

                // assign data
                $scope.prioritylists = response.data.d;

            })
            .catch(function (error) {

                alert("HTTP ERROR: " + (error.statusText || error.message || "Unknown error"));
                console.error("HTTP ERROR:", error);

            })
            .finally(function () {

                //$scope.isLoading = false;

            });
    };
    $scope.loadtaglist = function () {

        //$scope.isLoading = true;

        myprojectService.gettaglist()
            .then(function (response) {

                // assign data
                $scope.tagslist = response.data.d;

            })
            .catch(function (error) {

                alert("HTTP ERROR: " + (error.statusText || error.message || "Unknown error"));
                console.error("HTTP ERROR:", error);

            })
            .finally(function () {

                //$scope.isLoading = false;

            });
    };
    $scope.loadprojectfiles = function (c,index) {

        //$scope.isLoading = true;
        
        $scope.currentprojectid = c.id;
        $scope.currentlist = c;
        $scope.currentIndex = index;
        $scope.modalTitle = "Uploaded documents";

        // Update label in the modal
        var lbl = document.getElementById('ContentPlaceHolder1_lblfiles');
        if (lbl) lbl.innerHTML = $scope.modalTitle;
        $scope.showfileslist = c.viewfiles;
        //myprojectService.getprojectfiles(c.id)
        //    .then(function (response) {

        //        debugger;
        //        // assign data
        //        $scope.showfileslist = response.data.d;

        //    })
        //    .catch(function (error) {

        //        debugger;
        //        alert("HTTP ERROR: " + (error.statusText || error.message || "Unknown error"));
        //        console.error("HTTP ERROR:", error);

        //    })
        //    .finally(function () {

        //        //$scope.isLoading = false;

        //    });
    };
    $scope.viewallfiles = function () {
        if ($scope.showfileslist && $scope.showfileslist.length > 0) {
            angular.forEach($scope.showfileslist, function (c) {
                c.isshow = true;
                //if ($scope.chkall) {
                //    c.isshow = true;
                //}
                //else {
                //    if (c.ftypename=="project") {
                //        c.isshow = true;
                //    }
                //    else {
                //        c.isshow = false;
                //    }
                //}
                
            });
            
        }
    }
    //#endregion
    //#regin bind extended details
    $scope.Bindextenddate_Details = function (c, index) {

        //$scope.isLoading = true;

        $scope.currentprojectid = c.id;
        $scope.currentlist = c;
        $scope.currentIndex = index; 
        $scope.currentlist_extendlist = c.extendedlists;
        var last = c.extendedlists[c.extendedlists.length - 1];
     
        //$scope.extendStart = formatDateDDMMYYYY(last.fromdate);
       // $scope.extendEnd = formatDateDDMMYYYY(last.todate);
        $scope.extendObj.fromdate = formatDateDDMMYYYY(last.fromdate);
        $scope.extendObj.todate = formatDateDDMMYYYY(last.todate);
       
        
    };
    $scope.loadformedit = function (c,frameId, url, title) {

        $scope.currentprojectid = c.id;
        $scope.currentlist = c;       
        $scope.popupCounter = ($scope.popupCounter || 0) + 1;

        // modal title
        $scope.projectTitle = "Edit Project";

        // build URL
        let finalUrl = "";

        if (url.indexOf("aspx?") !== -1) {
            finalUrl = url + "&ctype=ang&count=" + $scope.popupCounter;
        } else {
            finalUrl = url + "?ctype=ang&count=" + $scope.popupCounter;
        }

        // set iframe src
        let frame = document.getElementById(frameId);
        if (frame) {
            frame.src = "";
            frame.src = finalUrl;
        }

    };
$scope.deletedataproject = function (c) {

    Swal.fire({
        title: 'Delete Project?',
        text: 'Are you sure you want to delete this project?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#6c757d',
        confirmButtonText: 'Yes, Delete',
        cancelButtonText: 'Cancel'
    }).then(function (result) {

        if (!result.isConfirmed)
            return;
        myprojectService.removeproject(c.id)
            .then(function (response) {

                if (response.data.d == "1") {
                    let realIndex = $scope.projectlist.findIndex(f => f.id === c.id);

                    if (realIndex !== -1) {
                        $scope.projectlist.splice(realIndex, 1);
                    }

                    Swal.fire({
                        icon: 'success',
                        title: 'Deleted!',
                        text: 'Project deleted successfully.',
                        timer: 2000,
                        showConfirmButton: false
                    });
                }
                else {

                    Swal.fire({
                        icon: 'error',
                        title: 'Error',
                        text: 'Unable to delete project.'
                    });
                }
            })
            .catch(function (err) {
                console.error(err);

                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: 'Something went wrong.'
                });
            });

    });
};
    $scope.deletedataprojectold = function (c) {
        var isupdate = "0";
        myprojectService.removeproject(c.id)
            .then(function (response) {
                if (response.data.d) {
                    isupdate = response.data.d;
                    if (isupdate == "1") {
                        let realIndex = $scope.projectlist.findIndex(f => f.id === c.id);
                        if (realIndex !== -1) {
                            $scope.projectlist.splice(realIndex, 1);
                        }
                    }
                }
            })



    };
    $scope.SvaeExtended = function () {
        if (!$scope.currentlist) return;

        //var payload = {
        //    projctid: $scope.currentlist.id,
        //    extendStrtdate: $scope.extendObj.fromdate,
        //    extendEnddate: $scope.extendObj.todate
        //};
        var payload = {
            projctid: $scope.currentlist.id,
            extendStrtdate: toServerDateOnly($scope.extendObj.fromdate),
            extendEnddate: toServerDateOnly($scope.extendObj.todate)
        };

        myprojectService.SvaeExtended_details(payload)
            .then(function (response) {
                if (response.data.d) {

                    let newrow = response.data.d;

                    if (newrow.status == "1") {

                        let pindex = $scope.projectlist.findIndex(
                            m => m.id == $scope.currentlist.id
                        );

                        if (pindex>-1) {

                $scope.projectlist[pindex].StartDate=parseDate($scope.extendObj.fromdate);
                $scope.projectlist[pindex].lastextendstartdate=parseDate($scope.extendObj.fromdate);
                $scope.projectlist[pindex].lastextendenddate=parseDate($scope.extendObj.todate);
                $scope.projectlist[pindex].TargetDate=parseDate($scope.extendObj.todate);
                            if (!$scope.projectlist[pindex].extendedlists) {
                                $scope.projectlist[pindex].extendedlists = [];
                            }

                            $scope.projectlist[pindex].extendedlists.push({
                                extended_from:parseDate(newrow.extended_from),
                                extended_to:parseDate(newrow.extended_to),
                                extended_days:newrow.extended_days || 0,
                                delayed_days:newrow.delayed_days || 0,
                                extended_type:newrow.extended_type || 'days'
                            });
                        }

                        $scope.extendObj = {};
                        document.getElementById("btnclsextended").click();
                    }
                }

               // $('#ExtendedTimline').modal('hide');

            })
            .catch(function (error) {
                alert("HTTP ERROR: " + (error.statusText || error.message));
            });
    };
    
    //$scope.closedata = function () {
       // debugger;
        //alert('ok');
    //}
    $scope.closedata = function () {

        if ($scope.currentprojectid != "0" && $scope.currentprojectid != "") {
            if (!$scope.currentlist) return;
            let obj = $scope.currentlist;

            let resultnew = null;
            // API call based on type

            resultnew = myprojectService.getitemdetails(obj.id);

            if (!resultnew) return;

            resultnew.then(function (res) {
                let getlist = res.data.d;
                if (getlist) {



                    getlist.StartDate = parseDate(getlist.StartDate);
                    getlist.TargetDate = parseDate(getlist.TargetDate);
                    getlist.AwardDate = parseDate(getlist.AwardDate);
                    getlist.DrawingApprovalDate = parseDate(getlist.DrawingApprovalDate);
                    if (getlist.extendedlists && getlist.extendedlists.length > 0) {
                        angular.forEach(getlist.extendedlists, function (e) {
                            e.fromdate = parseDate(e.fromdate);
                            e.todate = parseDate(e.todate);
                        });
                    }



                    let pindex = $scope.projectlist.findIndex(m => m.id == obj.id);
                    let ulist = $scope.projectlist[pindex];
                    ulist.ProjectID = getlist.ProjectID;
                    ulist.ProjectName = getlist.ProjectName;
                    ulist.ClientName = getlist.ClientName;
                    ulist.Owner = getlist.Owner;
                    ulist.team = getlist.team;
                    ulist.ProjectStatus = getlist.ProjectStatus;
                    ulist.status_name = getlist.status_name;
                    ulist.status_color = getlist.status_color;
                    ulist.ProjectAddress1 = getlist.ProjectAddress1;
                    ulist.ProjectAddress2 = getlist.ProjectAddress2;
                    ulist.Completeaddress = getlist.Completeaddress;
                    ulist.City = getlist.City;
                    ulist.CityName = getlist.CityName;
                    ulist.PostalCode = getlist.PostalCode;
                    ulist.StateName = getlist.StateName;
                    ulist.Countryid = getlist.Countryid;
                    ulist.CountryName = getlist.CountryName;
                    ulist.ClientName = getlist.ClientName;
                    ulist.Priority = getlist.Priority;
                    ulist.priority_name = getlist.priority_name;
                    ulist.priority_color = getlist.priority_color;
                    ulist.Budget = getlist.Budget;
                    ulist.Actual = getlist.Actual;
                    ulist.AwardDate = getlist.AwardDate;
                    ulist.StartDate = getlist.StartDate;
                    ulist.TargetDate = getlist.TargetDate;
                    ulist.ApprovalDrawing = getlist.ApprovalDrawing;
                    ulist.DrawingApprovalDate = getlist.DrawingApprovalDate;
                    ulist.Uploaddrawing = getlist.Uploaddrawing;
                    ulist.Weightage = getlist.Weightage;
                    ulist.isdrawing = getlist.isdrawing;
                    ulist.checkprojectWeightage = getlist.checkprojectWeightage;
                    ulist.customerid = getlist.customerid;
                    ulist.salesorderid = getlist.salesorderid;
                    ulist.hasmile = getlist.hasmile;
                    ulist.hastask = getlist.hastask;
                    ulist.checktaskWeightage = getlist.checktaskWeightage;
                    ulist.hassubtask = getlist.hassubtask;
                    ulist.checksubtaskWeightage = getlist.checksubtaskWeightage;
                    ulist.leftdays = getlist.leftdays;
                    ulist.txtdays = getlist.txtdays;
                    ulist.complete_value = getlist.complete_value;
                    ulist.pending_value = getlist.pending_value;
                    ulist.totalmile = getlist.totalmile;
                    ulist.completemile = getlist.completemile;
                    ulist.completemile_rate = getlist.completemile_rate;
                    ulist.pendingmile = getlist.pendingmile;
                    ulist.pendingmile_rate = getlist.pendingmile_rate;
                    ulist.totaltask = getlist.totaltask;
                    ulist.completetask = getlist.completetask;
                    ulist.completetask_rate = getlist.completetask_rate;
                    ulist.pendingtask = getlist.pendingtask;
                    ulist.pendingtask_rate = getlist.pendingtask_rate;
                    ulist.totalissue = getlist.totalissue;
                    ulist.completeissue = getlist.completeissue;
                    ulist.completeissue_rate = getlist.completeissue_rate;
                    ulist.pendingissue = getlist.pendingissue;
                    ulist.pendingissue_rate = getlist.pendingissue_rate;
                    ulist.extendedlists = getlist.extendedlists;
                    ulist.tags = getlist.tags;
                    ulist.owners = getlist.owners;
                    ulist.teams = getlist.teams;
                    ulist.viewfiles = getlist.viewfiles;


                }

            })
                .catch(function (err) {
                    console.error(err);
                    //alert("Update failed");
                })
                .finally(function () {

                });
        }

    };
    $scope.closedatanew = function () {
        var pid = "0";
        if (document.getElementById("ContentPlaceHolder1_hdnnewprojectid").value != "") {
            pid = document.getElementById("ContentPlaceHolder1_hdnnewprojectid").value;
        }


        let resultnew = null;
        // API call based on type

        resultnew = myprojectService.getitemdetails(pid);

        if (!resultnew) return;

        resultnew.then(function (res) {
            let getlist = res.data.d;
            if (getlist) {

                //$scope.projectlist.push({
                $scope.projectlist.unshift({
                    id: getlist.id,
                    ProjectID: getlist.ProjectID,
                    ProjectName: getlist.ProjectName,
                    ClientName: getlist.ClientName,
                    Owner: getlist.Owner,
                    team: getlist.team,
                    ProjectStatus: getlist.ProjectStatus,
                    status_name: getlist.status_name,
                    status_color: getlist.status_color,
                    ProjectAddress1: getlist.ProjectAddress1,
                    ProjectAddress2: getlist.ProjectAddress2,
                    Completeaddress: getlist.Completeaddress,
                    City: getlist.City,
                    CityName: getlist.CityName,
                    PostalCode: getlist.PostalCode,
                    StateName: getlist.StateName,
                    Countryid: getlist.Countryid,
                    CountryName: getlist.CountryName,
                    ClientName: getlist.ClientName,
                    Priority: getlist.Priority,
                    priority_name: getlist.priority_name,
                    priority_color: getlist.priority_color,
                    Budget: getlist.Budget,
                    Actual: getlist.Actual,
                    AwardDate: parseDate(getlist.AwardDate),
                    StartDate: parseDate(getlist.StartDate),
                    TargetDate: parseDate(getlist.TargetDate),
                    ApprovalDrawing: getlist.ApprovalDrawing,
                    DrawingApprovalDate: getlist.DrawingApprovalDate,
                    Uploaddrawing: getlist.Uploaddrawing,
                    Weightage: getlist.Weightage,
                    isdrawing: getlist.isdrawing,
                    checkprojectWeightage: getlist.checkprojectWeightage,
                    customerid: getlist.customerid,
                    salesorderid: getlist.salesorderid,
                    hasmile: getlist.hastask,
                    hastask: getlist.hastask,
                    checktaskWeightage: getlist.checktaskWeightage,
                    hassubtask: getlist.hassubtask,
                    checksubtaskWeightage: getlist.checksubtaskWeightage,
                    leftdays: getlist.leftdays,
                    txtdays: getlist.txtdays,
                    complete_value: getlist.complete_value,
                    pending_value: getlist.pending_value,
                    totalmile: getlist.totalmile,
                    completemile: getlist.completemile,
                    completemile_rate: getlist.completemile_rate,
                    pendingmile: getlist.pendingmile,
                    pendingmile_rate: getlist.pendingmile_rate,
                    totaltask: getlist.totaltask,
                    completetask: getlist.completetask,
                    completetask_rate: getlist.completetask_rate,
                    pendingtask: getlist.pendingtask,
                    pendingtask_rate: getlist.pendingtask_rate,
                    totalissue: getlist.totalissue,
                    completeissue: getlist.completeissue,
                    completeissue_rate: getlist.completeissue_rate,
                    pendingissue: getlist.pendingissue,
                    pendingissue_rate: getlist.pendingissue_rate,
                    extendedlists: getlist.extendedlists,
                    tags: getlist.tags,
                    owners: getlist.owners,
                    teams: getlist.teams,
                    viewfiles: getlist.viewfiles,
                })

            }

        })
            .catch(function (err) {
                console.error(err);
                //alert("Update failed");
            })
            .finally(function () {

            });
    }
    $scope.closepopup_canvas2 = function () {
        try {
            var btn = (window.parent && window.parent.document)
                ? window.parent.document.getElementById("btnclosemainpopup2")
                : document.getElementById("btnclosemainpopup2");
            if (btn == null) {
                btn = document.getElementById("btnclosemainpopup2");
            }
            if (btn) {
                btn.click();
            }
        }
        catch
        {

        }
        var utype = $scope.currentFtype;
        if ($scope.currentprojectid != "0" && $scope.currentprojectid != "") {
            if (!$scope.currentlist) return;
            let obj = $scope.currentlist;
            let resultnew = null;
            // API call based on type
            if (utype === 'milestone') {
                resultnew = myprojectService.getissuesdetails(obj.id, utype);
            }
            else if (utype === 'task') {
                resultnew = myprojectService.getissuesdetails(obj.id, utype);
            }
            else if (utype === 'issue') {
                resultnew = myprojectService.getissuesdetails(obj.id, utype);
            }
            else if (utype === 'dependency') {
                resultnew = myprojectService.getissuesdetails(obj.id, utype);
            }
            

            if (!resultnew) return;

            resultnew.then(function (res) {
                let getlist = res.data.d;
                if (getlist) {

                    let pindex = $scope.projectlist.findIndex(m => m.id == obj.id);
                    let ulist = $scope.projectlist[pindex];
                    ulist.complete_value = getlist.pcopmelete_value;
                    ulist.pending_value = getlist.ppending_value
                    if (utype === 'milestone') {
                        ulist.totalmile = getlist.total;
                        ulist.completemile = getlist.complete;
                        ulist.completemile_rate = getlist.complete_rate;
                        ulist.pendingmile = getlist.pending;
                        ulist.pendingmile_rate = getlist.pending_rate;
                    }
                    else if (utype === 'task') {
                        ulist.totaltask = getlist.total;
                        ulist.completetask = getlist.complete;
                        ulist.completetask_rate = getlist.complete_rate;
                        ulist.pendingtask = getlist.pending;
                        ulist.pendingtask_rate = getlist.pending_rate;
                    }
                    else if (utype === 'issue') {
                        ulist.totalissue = getlist.total;
                        ulist.completeissue = getlist.complete;
                        ulist.completeissue_rate = getlist.complete_rate;
                        ulist.pendingissue = getlist.pending;
                        ulist.pendingissue_rate = getlist.pending_rate;
                    }
                    
                }
                //$timeout(function () {                   
                   
                    inittooltip();
               // }, 0);

            })
                .catch(function (err) {
                    console.error(err);
                    //alert("Update failed");
                })
                .finally(function () {

                });
        }

    };
    //#endregion
    $scope.selectStatus = function (c, item) {

        
        var pid = 0;
        let resultnew;
        
            pid = c.id;
            resultnew = myprojectService.UpdateP_Status(c.id, item.id, item.iscomplete);        

        if (resultnew) {
            resultnew.then(function (res) {
                if (res.data.d != "1") {
                    alert(res.data.d);
                }
                else {
                    c.ProjectStatus = item.id;
                    c.status_color = item.colorname;
                    c.status_name = item.name;
                   
                }
                $scope.filterstatuslist_count();
                $scope.$applyAsync();

                //alert(res.d);
                //console.log(res);
            })
                .catch(function (err) {
                    //console.error(err);
                    //alert("Update failed");
                });
        }
    
    };
    $scope.selectPriority = function (c, item) {



        var pid = 0;
        let resultnew;

        pid = c.id;
        resultnew = myprojectService.UpdateP_Priority(c.id, item.id);

        if (resultnew) {
            resultnew.then(function (res) {
                if (res.data.d != "1") {
                    alert(res.data.d);
                }
                else {
                    c.Priority = item.id;
                    c.priority_color = item.colorname;
                    c.priority_name = item.name;

                }
                //alert(res.d);
                //console.log(res);
            })
                .catch(function (err) {
                    //console.error(err);
                    //alert("Update failed");
                });
        }
    };
    $scope.loadprojectdata = function () {
        $scope.isLoading = true;
        $scope.projectDataReady = false;

        var fdate = moment().startOf('month').format('MM/DD/YYYY');
        var tdate = moment().endOf('month').format('MM/DD/YYYY');

        // Initial saved filter can be used immediately even if daterangepicker
        // has not finished initializing yet. This removes the old up-to-2-second wait.
        if ($scope.pendingProjectDateRange) {
            fdate = $scope.pendingProjectDateRange.fdate;
            tdate = $scope.pendingProjectDateRange.tdate;
            $scope.pendingProjectDateRange = null;
        }
        else {
            var picker = $('#daterange').data('daterangepicker');
            if (picker) {
                fdate = picker.startDate.format('MM/DD/YYYY');
                tdate = picker.endDate.format('MM/DD/YYYY');
            }
        }

        myprojectService.getprojectlist(fdate, tdate)
            .then(function (response) {
                var data = response.data.d;
                if (typeof data === "string") {
                    data = JSON.parse(data);
                }

                $scope.projectlist = data || [];
                $scope.projectCalendarIsAdmin = $scope.projectlist.length > 0 &&
                    String($scope.projectlist[0].isadmin || '0') === '1';
                $scope.myprojectlist_bystatus.length = 0;
                $scope.filterstatuslist_count();
                $scope.mytotalprojectlist = ($scope.projectlist || []).filter(function (project) {
                    return $scope.projectSituationFilter(project);
                }).length;

                angular.forEach($scope.projectlist, function (c) {
                    c.StartDate = parseDate(c.StartDate);
                    c.TargetDate = parseDate(c.TargetDate);
                    c.AwardDate = parseDate(c.AwardDate);
                    c.DrawingApprovalDate = parseDate(c.DrawingApprovalDate);
                    c.lastextendstartdate = parseDate(c.lastextendstartdate);
                    c.lastextendenddate = parseDate(c.lastextendenddate);

                    if (c.extendedlists && c.extendedlists.length > 0) {
                        angular.forEach(c.extendedlists, function (e) {
                            e.fromdate = parseDate(e.fromdate);
                            e.todate = parseDate(e.todate);
                        });
                    }
                });

                $scope.buildProjectCalendar();

                // Data (including a valid empty result) has now finished loading.
                // The table/header is allowed to render only after this point.
                $scope.projectDataReady = true;

                // Let Angular finish rendering the new filtered rows before
                // initializing table UI/tooltips.
                $timeout(function () {
                    initTableUI();
                }, 0);
            })
            .catch(function (error) {
                $scope.projectDataReady = false;
                console.error("Project data load error:", error);

                // Only show an alert for a real HTTP/server failure.
                // A client-side UI exception has no .status and previously showed
                // the misleading message: Error: undefined.
                if (error && typeof error.status !== "undefined") {
                    var serverMessage = "";
                    if (error.data) {
                        serverMessage = error.data.Message || error.data.message || "";
                    }

                    var message = "Project data could not be loaded";
                    if (error.status) {
                        message += " (HTTP " + error.status + ")";
                    }
                    if (serverMessage) {
                        message += ": " + serverMessage;
                    }
                    alert(message);
                }
            })
            .finally(function () {
                // Never leave the full-screen loader stuck after an HTTP/server error.
                $scope.isLoading = false;
            });
    };


   // load details
    // PERFORMANCE: master/lookup lists are now requested right after the project
    // data request (see after $scope.loadsavedfilter() below), so the grid request
    // is not queued behind 7 lookup requests (browser limit ~6 per host).
   
   
      //#region manage project files
    $scope.uploadedFiles = [];
    var filterMap = {
        "all": "0",
        "today": "1",
        "this week": "2",
        "this month": "3",
        "this quarter": "4",
        "this year": "5",
        "yesterday": "6",
        "previous week": "7",
        "previous month": "8",
        "previous quarter": "9",
        "previous year": "10",
        "custom": "11",
        "custom range": "11"
    };

    function getDateRangeByFilterId(filterId) {
        var id = parseInt(filterId, 10);

        // DB currently stores only the filter id. Exact Custom Range dates
        // are not stored, so Custom Range safely falls back to This Month.
        if (isNaN(id) || id < 0 || id > 11 || id === 11) {
            id = 3;
        }

        switch (id) {
            case 0:
                return { id: "0", label: "All", startDate: moment('2020-01-01'), endDate: moment() };
            case 1:
                return { id: "1", label: "Today", startDate: moment(), endDate: moment() };
            case 2:
                return { id: "2", label: "This Week", startDate: moment().startOf('week'), endDate: moment().endOf('week') };
            case 3:
                return { id: "3", label: "This Month", startDate: moment().startOf('month'), endDate: moment().endOf('month') };
            case 4:
                return { id: "4", label: "This Quarter", startDate: moment().startOf('quarter'), endDate: moment().endOf('quarter') };
            case 5:
                return { id: "5", label: "This Year", startDate: moment().startOf('year'), endDate: moment().endOf('year') };
            case 6:
                return { id: "6", label: "Yesterday", startDate: moment().subtract(1, 'days'), endDate: moment().subtract(1, 'days') };
            case 7:
                return { id: "7", label: "Previous Week", startDate: moment().subtract(1, 'week').startOf('week'), endDate: moment().subtract(1, 'week').endOf('week') };
            case 8:
                return { id: "8", label: "Previous Month", startDate: moment().subtract(1, 'month').startOf('month'), endDate: moment().subtract(1, 'month').endOf('month') };
            case 9:
                return { id: "9", label: "Previous Quarter", startDate: moment().subtract(1, 'quarter').startOf('quarter'), endDate: moment().subtract(1, 'quarter').endOf('quarter') };
            case 10:
                return { id: "10", label: "Previous Year", startDate: moment().subtract(1, 'year').startOf('year'), endDate: moment().subtract(1, 'year').endOf('year') };
            default:
                return { id: "3", label: "This Month", startDate: moment().startOf('month'), endDate: moment().endOf('month') };
        }
    }

    function waitForMainDateRangePicker(callback, attempt) {
        attempt = attempt || 0;

        var picker = $('#daterange').data('daterangepicker');
        if (picker) {
            callback(picker);
            return;
        }

        if (attempt < 40) {
            $timeout(function () {
                waitForMainDateRangePicker(callback, attempt + 1);
            }, 50);
            return;
        }

        callback(null);
    }

    function applySavedDateFilter(filterId) {
        var selectedRange = getDateRangeByFilterId(filterId);
        $scope.presearch = selectedRange.id;

        // Preserve the exact saved filter dates for the first project request.
        // Project data does not need to wait for the visual daterangepicker plugin.
        $scope.pendingProjectDateRange = {
            fdate: selectedRange.startDate.format('MM/DD/YYYY'),
            tdate: selectedRange.endDate.format('MM/DD/YYYY')
        };

        $scope.loadprojectdata();

        // Update only the picker UI when it becomes available.
        waitForMainDateRangePicker(function (picker) {
            if (picker) {
                picker.setStartDate(selectedRange.startDate);
                picker.setEndDate(selectedRange.endDate);
                picker.chosenLabel = selectedRange.label;

                picker.container.find('.ranges li').removeClass('active');
                picker.container
                    .find('.ranges li[data-range-key="' + selectedRange.label + '"]')
                    .addClass('active');

                $('#daterange').val(
                    selectedRange.startDate.format('DD/MM/YYYY') +
                    ' - ' +
                    selectedRange.endDate.format('DD/MM/YYYY')
                );
            }
        });
    }

    $scope.loadsavedfilter = function () {
$scope.isLoading = true;
        // PERFORMANCE: use the saved filter rendered by default.aspx Page_Load
        // (same value as getfilter) to avoid one extra round trip before the grid loads.
        if (typeof window.PmsSavedProjectFilter !== 'undefined' &&
            window.PmsSavedProjectFilter !== null &&
            window.PmsSavedProjectFilter !== '') {
            var inlineFilter = String(window.PmsSavedProjectFilter);
            window.PmsSavedProjectFilter = null; // only for the first load
            applySavedDateFilter(inlineFilter);
            return;
        }
        myprojectService.getfilter().then(
            function (res) {
                var savedFilter = res && res.data ? res.data.d : null;
                applySavedDateFilter(savedFilter);
            },
            function (err) {
                console.error("Saved filter not loaded", err);
                applySavedDateFilter("3");
            }
        );
    };
       $scope.bindtasklist2 = function (fromDate, toDate, searchtype) {
           var normalizedSearchType = (searchtype || "").trim().toLowerCase();

           if (filterMap.hasOwnProperty(normalizedSearchType)) {
               $scope.presearch = filterMap[normalizedSearchType];
               $scope.savefilter($scope.presearch);
           }

           // Use the exact dates supplied by daterangepicker.
           // This avoids any timing/race issue while the picker UI is updating.
           var fromMoment = moment(fromDate, 'DD/MM/YYYY', true);
           var toMoment = moment(toDate, 'DD/MM/YYYY', true);
           if (fromMoment.isValid() && toMoment.isValid()) {
               $scope.pendingProjectDateRange = {
                   fdate: fromMoment.format('MM/DD/YYYY'),
                   tdate: toMoment.format('MM/DD/YYYY')
               };
           }

           $scope.loadprojectdata();
       };
    $scope.savefilter = function (filtername)
    {


        // "0" is valid for All
        if (filtername === undefined ||
            filtername === null ||
            filtername === "") {
            return;
        }
        myprojectService.managefilter(filtername).then(
            function (res) {
                //console.log("Filter saved successfully");
            },
            function (err) {
                //console.error("Filter not saved", err);
            }
        );
    };

    $scope.loadsavedfilter();

    // Lookup lists (same calls as before, only issued after the project request).
    $scope.loadstatuslist();
    $scope.loadProjectSituationList();
    $scope.loadprioritylist();
    $scope.loaduserlist();
    // Team uses the same employee list; loaduserlist() fills projectteams too.
    $scope.loadtaglist();
    $scope.loadcountrylist();
    $scope.loadcustomerlist();
   
    const dropZone2 = document.getElementById("dropZone2");
    const fileInput2 = document.getElementById("fileupload2");
    $scope.removeFile2 = function (index,id) {
        $scope.uploadedFiles2.splice(index, 1);
        if(id!="")
        {
$scope.removedocumentnew(id);
}
    };

    $scope.viewFile2 = function (file) {
        var win = window.open("");

        if (file.type.indexOf('image/') === 0) {

            win.document.write(`
            <img src="${file.data}"
                 style="max-width:100%;height:auto;">
        `);

        }
        else if (file.type === 'application/pdf') {
            win.document.write(`
            <iframe src="${file.data}"
                    width="100%"
                    height="100%"
                    style="position:fixed;top:0;left:0;border:none;">
            </iframe>
        `);

        }
    };
$scope.viewExistfile = function (file) {

    var win = window.open("", "_blank");

    var ext = (file.fext || "").toLowerCase().replace(".", "");

    // Images
    if (['jpg', 'jpeg', 'png', 'gif', 'bmp', 'webp'].includes(ext)) {

        win.document.write(`
            <img src="${file.data}"
                 style="max-width:100%;height:auto;">
        `);

    }
    // PDF
    else if (ext === 'pdf') {

        win.document.write(`
            <iframe src="${file.data}"
                    width="100%"
                    height="100%"
                    style="position:fixed;top:0;left:0;border:none;">
            </iframe>
        `);

    }
    // Other files
    else {

        win.location.href = file.data;
        // OR
        // window.open(file.file, '_blank');
    }
};
$scope.getFileCategory = function (file) {

    // Existing file from DB
    if (file.id) {

        var ext = (file.fext || '').toLowerCase();

        if (['.jpg', '.jpeg', '.png', '.gif', '.bmp', '.webp'].includes(ext))
            return 'image';

        if (ext === '.pdf')
            return 'pdf';

        if (['.xls', '.xlsx', '.csv'].includes(ext))
            return 'excel';

        if (['.doc', '.docx'].includes(ext))
            return 'word';

        if (['.txt'].includes(ext))
            return 'text';

        return 'other';
    }

    // New uploaded file
    var type = (file.type || '').toLowerCase();

    if (type.indexOf('image/') === 0)
        return 'image';

    if (type === 'application/pdf')
        return 'pdf';

    if (
        type.indexOf('spreadsheet') >= 0 ||
        type.indexOf('excel') >= 0 ||
        type === 'text/csv'
    )
        return 'excel';

    if (
        type.indexOf('word') >= 0 ||
        type.indexOf('document') >= 0
    )
        return 'word';

    if (type === 'text/plain')
        return 'text';

    return 'other';
};


    $scope.openFilePicker2 = function () {


        fileInput2.click();
    };
    $scope.filesSelected2 = function (files) {

        $scope.processFiles2(files, 'project');
    };
    dropZone2.addEventListener("dragover", function (e) {
        e.preventDefault();
        dropZone2.style.borderColor = "#007bff";
        dropZone2.style.backgroundColor = "#f0f8ff";
        $scope.$apply(() => { $scope.isDragging = true; });
    });
    dropZone2.addEventListener("dragleave", function (e) {
        e.preventDefault();
        dropZone2.style.borderColor = "#ccc";
        dropZone2.style.backgroundColor = "";
        $scope.$apply(() => { $scope.isDragging = false; });
    });
    dropZone2.addEventListener("drop", function (e) {
        e.preventDefault();
        dropZone2.style.borderColor = "#ccc";
        dropZone2.style.backgroundColor = "";
        $scope.$apply(() => { $scope.isDragging = false; });
        $scope.processFiles2(
            e.dataTransfer.files,
            'project'
        );
    });
    $scope.processFiles2 = function (files, ftype) {

        for (let i = 0; i < files.length; i++) {
            let file = files[i];
            let reader = new FileReader();
            reader.onload = function (evt) {
                $scope.$apply(() => {
                    $scope.uploadedFiles2.push({
                        id:"",
                        Uploadby:"",
                        file: file,                        
                        name: file.name,
                        type: file.type,
                        size: file.size,
                        fext: file.name.split('.').pop().toLowerCase(),
                        data: evt.target.result
                    });

                });
            };
            reader.readAsDataURL(file);
        }


    };
$scope.uploadFilesToServer2 = function (pid) {

    if (!$scope.uploadedFiles2 || $scope.uploadedFiles2.length === 0) {
        return Promise.resolve(); // no files, continue
    }

    return myprojectService
        .uploadfiles2(pid, $scope.uploadedFiles2)
        .then(function (res) {

            console.log("Files uploaded successfully");
            return res;

        })
        .catch(function (err) {

            console.error("Upload failed", err);
            throw err;

        });
};
$scope.uploadFilesToServer2old = function (pid) {

    if (!$scope.uploadedFiles2 || $scope.uploadedFiles2.length === 0) {
        //alert("No files to upload");
        return;
    }
 myprojectService.uploadfiles2(pid,$scope.uploadedFiles2).then(function (res) {
           //alert(res.data);                          
            
        }, function (err) {

            console.error("Upload failed", err);
        });
    
};
    const dropZone = document.getElementById("dropZone");    
    const fileInput = document.getElementById("fileupload");
    

    // Click drop zone → open file picker
    $scope.openFilePicker = function () {


        fileInput.click();
    };

    // Files selected via input
    $scope.filesSelected = function (files) {

        $scope.processFiles(files, $scope.currentprojectid, $scope.currentprojectid,'project', $scope.currentlist);
    };
   
    // Drag over
    dropZone.addEventListener("dragover", function (e) {
        e.preventDefault();
        dropZone.style.borderColor = "#007bff";
        dropZone.style.backgroundColor = "#f0f8ff";
        $scope.$apply(() => { $scope.isDragging = true; });
    });
    
    // Drag leave
    dropZone.addEventListener("dragleave", function (e) {
        e.preventDefault();
        dropZone.style.borderColor = "#ccc";
        dropZone.style.backgroundColor = "";
        $scope.$apply(() => { $scope.isDragging = false; });
    });
    
    // Drop files
    dropZone.addEventListener("drop", function (e) {
        e.preventDefault();
        dropZone.style.borderColor = "#ccc";
        dropZone.style.backgroundColor = "";
        $scope.$apply(() => { $scope.isDragging = false; });
        $scope.processFiles(
            e.dataTransfer.files,
            $scope.currentprojectid,
            $scope.currentprojectid,
            'project',
            $scope.currentlist
        );
    });
    
    $scope.processFiles = function (files, fpid, fid, ftype, obj) {

        for (let i = 0; i < files.length; i++) {
            let file = files[i];
            let reader = new FileReader();
            reader.onload = function (evt) {
                $scope.$apply(() => {
                    $scope.uploadedFiles.push({
                        name: file.name,
                        type: file.type,
                        data: evt.target.result
                    });
                });
            };
            reader.readAsDataURL(file);
        }

        // Optional: auto-upload
        $scope.uploadFilesToServer(files, fpid, fid, ftype, obj);
    };
    
    $scope.uploadFilesToServer = function (files, pid, id, type, obj) {
        myprojectService.uploadfiles(files, pid, id, type).then(function (res) {
           
                res.data.forEach(file => {
                   obj.viewfiles.push(file);
                });
            
            //$scope.showfileslist = angular.copy($scope.currentlist.viewfiles);
            $scope.showfileslist = angular.copy(obj.viewfiles);
            //$scop.viewallfiles();
            inittooltip();
            console.log("Upload success", res.data);
        }, function (err) {

            console.error("Upload failed", err);
        });
    };
$scope.downloadFile = function (file) {

    var link = document.createElement('a');

    link.href = file.fileurl;
    link.download = file.filedes || '';

    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
};
    $scope.removedocumentnew = function (fileId) {
        //if (!confirm("Are you sure you want to delete this file?")) return;
        myprojectService.removedoc(fileId).then(function (res) {
            if (res.data.success) {
                let pindex = $scope.projectlist.findIndex(f => f.id == $scope.currentprojectid);
                let index = $scope.projectlist[pindex].viewfiles.findIndex(f => f.id == fileId);
                if (index !== -1) {                    
                    $scope.projectlist[pindex].viewfiles.splice(index, 1);
                }
                $scope.showfileslist = angular.copy($scope.projectlist[pindex].viewfiles);
                //$scop.viewallfiles();
            }
            inittooltip();
            console.log("Upload success", res.data);
        }, function (err) {

            console.error("Upload failed", err);
        });
        
    };
    //#endregion

// end scope
});
function getDateOnlyParts(value) {
    if (value === null || value === undefined || value === '') return null;

    // A Date created by parseDate() uses local noon and therefore preserves
    // its calendar components in every browser timezone.
    if (Object.prototype.toString.call(value) === '[object Date]') {
        if (isNaN(value.getTime())) return null;
        return {
            year: value.getFullYear(),
            month: value.getMonth() + 1,
            day: value.getDate()
        };
    }

    var text = String(value).trim();
    var match;

    // Preferred API format: yyyy-MM-dd (optionally followed by a time).
    match = /^(\d{4})-(\d{2})-(\d{2})(?:[T\s].*)?$/.exec(text);
    if (match) {
        return { year: +match[1], month: +match[2], day: +match[3] };
    }

    // UI format: dd/MM/yyyy or dd-MM-yyyy.
    match = /^(\d{2})[\/-](\d{2})[\/-](\d{4})$/.exec(text);
    if (match) {
        return { year: +match[3], month: +match[2], day: +match[1] };
    }

    // Backward compatibility for old ASP.NET /Date(...)/ responses.
    //
    // SQL project dates are calendar dates created in the India business
    // timezone. ASP.NET can serialize 30/06/2026 midnight as a UTC millisecond
    // value representing 29/06/2026 18:30 UTC. Reading that value with the
    // browser's local getters makes Dubai/USA show the previous day.
    //
    // Decode the source calendar components first, then parseDate() creates a
    // local-noon Date object. Existing timeline calculations therefore keep
    // working without browser-timezone date shifting.
    match = /^\/Date\((-?\d+)(?:([+-])(\d{2})(\d{2}))?\)\/$/.exec(text);
    if (match) {
        var milliseconds = parseInt(match[1], 10);
        if (!isFinite(milliseconds)) return null;

        // Prefer an explicit ASP.NET offset when supplied.
        // Older JavaScriptSerializer responses normally omit it, so use the
        // application's India date-source offset (+05:30) as the fallback.
        var sourceOffsetMinutes = 330;

        if (match[2] && match[3] && match[4]) {
            sourceOffsetMinutes =
                (parseInt(match[3], 10) * 60) +
                parseInt(match[4], 10);

            if (match[2] === '-') {
                sourceOffsetMinutes = -sourceOffsetMinutes;
            }
        }

        var sourceCalendar = new Date(
            milliseconds + (sourceOffsetMinutes * 60000)
        );

        if (isNaN(sourceCalendar.getTime())) return null;

        return {
            year: sourceCalendar.getUTCFullYear(),
            month: sourceCalendar.getUTCMonth() + 1,
            day: sourceCalendar.getUTCDate()
        };
    }

    return null;
}

function isValidDateOnlyParts(parts) {
    if (!parts) return false;

    var test = new Date(parts.year, parts.month - 1, parts.day, 12, 0, 0, 0);
    return test.getFullYear() === parts.year &&
           test.getMonth() === parts.month - 1 &&
           test.getDate() === parts.day;
}

function parseDate(value) {
    var parts = getDateOnlyParts(value);
    if (!isValidDateOnlyParts(parts)) return null;

    // Keep the existing Date-object contract required by the timeline.
    // Local noon is created from already-normalized calendar components, so
    // the same project date is displayed in every browser country.
    // Never convert this date-only value with toISOString().
    return new Date(parts.year, parts.month - 1, parts.day, 12, 0, 0, 0);
}

function formatDateOnly(value) {
    
    var parts = getDateOnlyParts(value);
    if (!isValidDateOnlyParts(parts)) return '';

    return String(parts.day).padStart(2, '0') + '/' +
           String(parts.month).padStart(2, '0') + '/' +
           parts.year;
}

function formatDateDDMMYYYY(value) {
    return formatDateOnly(value).replace(/\//g, '-');
}

function toServerDateOnly(value) {
    var parts = getDateOnlyParts(value);
    if (!isValidDateOnlyParts(parts)) return null;

    return parts.year + '-' +
           String(parts.month).padStart(2, '0') + '-' +
           String(parts.day).padStart(2, '0');
}

function toServerDate(value) {
    return toServerDateOnly(value) || '';
}

function formatDateTimeSameEverywhere(value, format) {
    if (!value) return '';

    var outputFormat = format || 'DD/MM/YYYY HH:mm:ss';
    var utcMoment;

    if (typeof value === 'string') {
        var aspNetMatch = /^\/Date\((-?\d+)(?:[+-]\d{4})?\)\/$/.exec(value.trim());
        if (aspNetMatch) {
            utcMoment = moment.utc(parseInt(aspNetMatch[1], 10));
        } else {
            // A timestamp without an offset is deliberately interpreted as UTC,
            // so the rendered clock value remains identical in every country.
            utcMoment = moment.utc(value);
        }
    } else if (Object.prototype.toString.call(value) === '[object Date]') {
        utcMoment = moment.utc(value.getTime());
    } else {
        utcMoment = moment.utc(value);
    }

    return utcMoment.isValid() ? utcMoment.format(outputFormat) : '';
}

function setPickerDate(id, value) {

    if (!value) return;

    var m = moment(value, 'DD/MM/YYYY', true);

    if (!m.isValid()) return;

    var picker = $(id).data('daterangepicker');

    if (picker) {
        picker.setStartDate(m);
        picker.setEndDate(m);
    }

    $(id).val(m.format('DD/MM/YYYY'));
}
function hideProjectTooltips() {
    if (!window.bootstrap || !bootstrap.Tooltip) return;
    document.querySelectorAll('[data-bs-toggle="tooltip"]').forEach(function (el) {
        var instance = bootstrap.Tooltip.getInstance(el);
        if (instance) instance.hide();
    });
}
function inittooltip() {
    // pmsapp.js also has a custom bs-tooltip directive. On this page we use
    // normal Bootstrap data attributes and initialize them safely here.
    if (!window.bootstrap || !bootstrap.Tooltip) return;

    document.querySelectorAll('[data-bs-toggle="tooltip"]').forEach(function (el) {
        try {
            // Timeline has its own live-value manual hover handler.
            if (el.classList.contains('project-timeline-wrap')) return;
            var newTitle = el.getAttribute('data-bs-title') ||
                           el.getAttribute('title') ||
                           el.getAttribute('data-bs-original-title') ||
                           '';

            newTitle = String(newTitle || '').trim();

            var instance = bootstrap.Tooltip.getInstance(el);

            // Bootstrap 5 does not accept title:null. Skip empty tooltips instead
            // of throwing and breaking the project filter promise chain.
            if (!newTitle) {
                if (instance) instance.dispose();
                return;
            }

            el.setAttribute('data-bs-title', newTitle);

            if (!el.dataset.pmsTooltipBound) {
                if (instance) instance.dispose();
                new bootstrap.Tooltip(el, { title: newTitle, container: 'body', trigger: 'manual' });
                el.dataset.pmsTooltipBound = '1';
                el.addEventListener('mouseenter', function () {
                    hideProjectTooltips();
                    var current = bootstrap.Tooltip.getInstance(el);
                    if (current) current.show();
                });
                el.addEventListener('mouseleave', function () {
                    var current = bootstrap.Tooltip.getInstance(el);
                    if (current) current.hide();
                });
                el.addEventListener('focusin', function () {
                    var current = bootstrap.Tooltip.getInstance(el);
                    if (current) current.show();
                });
                el.addEventListener('focusout', function () {
                    var current = bootstrap.Tooltip.getInstance(el);
                    if (current) current.hide();
                });
            } else if (!instance) {
                new bootstrap.Tooltip(el, { title: newTitle, container: 'body', trigger: 'manual' });
            } else if (typeof instance.setContent === 'function') {
                instance.setContent({ '.tooltip-inner': newTitle });
            }
        }
        catch (tooltipError) {
            // A tooltip must never block project grid/filter rendering.
            console.warn('Tooltip initialization skipped:', tooltipError);
        }
    });
}
function initTableUI() {
    // Keep optional UI helpers isolated: one failure must not make a successful
    // bindProjectdata request look like an HTTP error.
    try {
        inittooltip();
    }
    catch (tooltipError) {
        console.warn('Tooltip UI error:', tooltipError);
    }

    try {
        initProjectTableColumnDragDrop();
    }
    catch (tableError) {
        console.warn('Project table UI error:', tableError);
    }
}
function getProjectTableElement() {
    return document.querySelector('.project-table');
}

function getProjectTableStorageKey() {
    var projectId = 'default';
    try {
        var hash = window.location.hash || '';
        var match = hash.match(/id=([^&]+)/i);
        if (match && match[1]) {
            projectId = decodeURIComponent(match[1]);
        }
    } catch (ex) { }
    return 'pms_project_table_column_order_' + projectId;
}

function getHeaderIndex(cell) {
    if (!cell || !cell.parentNode) return -1;
    return Array.prototype.indexOf.call(cell.parentNode.children, cell);
}

function ensureProjectTableOriginalIndexes(table) {
    if (!table || !table.rows) return;

    Array.prototype.forEach.call(table.rows, function (row) {
        Array.prototype.forEach.call(row.children, function (cell, index) {
            if (!cell.hasAttribute('data-original-index')) {
                cell.setAttribute('data-original-index', index);
            }
        });
    });
}

function getProjectTableHeaderOrder(table) {
    if (!table || !table.tHead || !table.tHead.rows.length) return [];
    return Array.prototype.map.call(table.tHead.rows[0].cells, function (th) {
        return parseInt(th.getAttribute('data-original-index'), 10);
    });
}

function syncProjectTableRowsWithHeaderOrder(table) {
    if (!table || !table.tHead || !table.tHead.rows.length || !table.tBodies.length) return;

    ensureProjectTableOriginalIndexes(table);

    var order = getProjectTableHeaderOrder(table);
    if (!order.length) return;

    Array.prototype.forEach.call(table.tBodies[0].rows, function (row) {
        if (!row || !row.children || row.children.length !== order.length) return;

        order.forEach(function (originalIndex) {
            var foundCell = null;

            Array.prototype.forEach.call(row.children, function (cell) {
                if (parseInt(cell.getAttribute('data-original-index'), 10) === originalIndex) {
                    foundCell = cell;
                }
            });

            if (foundCell) {
                row.appendChild(foundCell);
            }
        });
    });
}

function startProjectTableRowObserver(table) {
    if (!table || !table.tBodies.length || table.dataset.rowObserverStarted === '1') return;

    table.dataset.rowObserverStarted = '1';

    var observer = new MutationObserver(function (mutations) {
        // IMPORTANT:
        // Do NOT use setTimeout here. setTimeout allows the new task/subtask
        // row to paint first in the old column position, then it moves again,
        // so the user sees blinking/flicker. MutationObserver runs before the
        // browser paints, so sync immediately.
        var hasNewRows = false;
        for (var i = 0; i < mutations.length; i++) {
            if (mutations[i].addedNodes && mutations[i].addedNodes.length) {
                hasNewRows = true;
                break;
            }
        }

        if (hasNewRows) {
            syncProjectTableRowsWithHeaderOrder(table);
        }
    });

    observer.observe(table.tBodies[0], { childList: true });
}

function clearProjectTableDragVisualState(table) {
    if (!table || !table.tHead || !table.tHead.rows.length) return;
    Array.prototype.forEach.call(table.tHead.rows[0].cells, function (th) {
        th.classList.remove('dragging-col');
        th.classList.remove('drag-over-col');
    });
}

function clearProjectTableDragState(table) {
    clearProjectTableDragVisualState(table);
    if (table && table.dataset) {
        delete table.dataset.dragFromIndex;
    }
}

function moveProjectTableColumn(table, fromIndex, toIndex) {
    if (!table || fromIndex === toIndex || fromIndex < 0 || toIndex < 0) return;

    ensureProjectTableOriginalIndexes(table);

    Array.prototype.forEach.call(table.rows, function (row) {
        if (!row || !row.children || row.children.length <= Math.max(fromIndex, toIndex)) return;

        var movingCell = row.children[fromIndex];
        if (!movingCell) return;

        var referenceCell = (fromIndex < toIndex)
            ? row.children[toIndex].nextSibling
            : row.children[toIndex];

        row.insertBefore(movingCell, referenceCell);
    });

    syncProjectTableRowsWithHeaderOrder(table);
}

function saveProjectTableColumnOrder(table) {
    if (!table || !table.tHead || !table.tHead.rows.length) return;

    var order = Array.prototype.map.call(table.tHead.rows[0].cells, function (th) {
        return parseInt(th.getAttribute('data-original-index'), 10);
    });

    localStorage.setItem(getProjectTableStorageKey(), JSON.stringify(order));
}

function applySavedProjectTableColumnOrder(table) {
    if (!table || !table.tHead || !table.tHead.rows.length) return;

    var savedOrder = [];
    try {
        savedOrder = JSON.parse(localStorage.getItem(getProjectTableStorageKey()) || '[]');
    } catch (ex) {
        savedOrder = [];
    }

    var headerCells = table.tHead.rows[0].cells;
    if (!savedOrder.length || savedOrder.length !== headerCells.length) return;

    savedOrder.forEach(function (originalIndex, targetIndex) {
        var currentCells = table.tHead.rows[0].cells;
        var currentIndex = -1;

        Array.prototype.forEach.call(currentCells, function (th, idx) {
            if (parseInt(th.getAttribute('data-original-index'), 10) === originalIndex) {
                currentIndex = idx;
            }
        });

        if (currentIndex > -1 && currentIndex !== targetIndex) {
            moveProjectTableColumn(table, currentIndex, targetIndex);
        }
    });
}

function initProjectTableColumnDragDrop() {
    var table = getProjectTableElement();
    if (!table || !table.tHead || !table.tHead.rows.length) return;

    var headerRow = table.tHead.rows[0];
    var fixedColumns =0; // Action + Milestone stay fixed

    Array.prototype.forEach.call(headerRow.cells, function (th, index) {
        if (!th.hasAttribute('data-original-index')) {
            th.setAttribute('data-original-index', index);
        }

        if (index < fixedColumns) {
            th.classList.add('column-fixed');
            th.removeAttribute('draggable');
            return;
        }

        th.classList.add('column-draggable');

        if (th.dataset.columnDragBound === '1') return;

        th.setAttribute('draggable', 'true');
        th.dataset.columnDragBound = '1';

        th.addEventListener('dragstart', function (e) {
            var fromIndex = getHeaderIndex(this);
            if (fromIndex < fixedColumns) {
                e.preventDefault();
                return;
            }

            table.dataset.dragFromIndex = fromIndex;
            this.classList.add('dragging-col');

            if (e.dataTransfer) {
                e.dataTransfer.effectAllowed = 'move';
                try {
                    e.dataTransfer.setData('text/plain', fromIndex.toString());
                } catch (ex) { }
            }
        });

        th.addEventListener('dragover', function (e) {
            var fromIndex = parseInt(table.dataset.dragFromIndex, 10);
            var toIndex = getHeaderIndex(this);
            if (isNaN(fromIndex) || fromIndex < fixedColumns || toIndex < fixedColumns) return;

            e.preventDefault();
            clearProjectTableDragVisualState(table);
            this.classList.add('drag-over-col');

            var draggingCell = headerRow.cells[fromIndex];
            if (draggingCell) {
                draggingCell.classList.add('dragging-col');
            }
        });

        th.addEventListener('drop', function (e) {
            e.preventDefault();

            var fromIndex = parseInt(table.dataset.dragFromIndex, 10);
            var toIndex = getHeaderIndex(this);

            if (isNaN(fromIndex) || fromIndex < fixedColumns || toIndex < fixedColumns || fromIndex === toIndex) {
                clearProjectTableDragState(table);
                return;
            }

            moveProjectTableColumn(table, fromIndex, toIndex);
            saveProjectTableColumnOrder(table);
            clearProjectTableDragState(table);
        });

        th.addEventListener('dragend', function () {
            clearProjectTableDragState(table);
        });
    });

    ensureProjectTableOriginalIndexes(table);

    if (table.dataset.columnOrderApplied !== '1') {
        applySavedProjectTableColumnOrder(table);
        table.dataset.columnOrderApplied = '1';
    }

    syncProjectTableRowsWithHeaderOrder(table);
    startProjectTableRowObserver(table);
}
/* =========================================================
   PROJECT TABLE COLUMN MENU - V7
   Features:
   1) Drag/drop column reorder
   2) Hide current column
   3) Show hidden column before/after current header
   4) Persist order + hidden columns in localStorage
   ========================================================= */
(function () {
    var FIXED_COLUMNS =0; // Action + Milestone cannot hide/move
    var MENU_ID = 'pmsProjectColumnHeaderMenu';

    function tableEl() {
        return document.querySelector('.project-table');
    }

    function storageKey() {
        var projectId = 'default';
        try {
            var hash = window.location.hash || '';
            var match = hash.match(/id=([^&]+)/i);
            if (match && match[1]) projectId = decodeURIComponent(match[1]);
        } catch (ex) { }
        return 'pms_project_table_column_config_' + projectId;
    }

    function oldOrderKey() {
        var projectId = 'default';
        try {
            var hash = window.location.hash || '';
            var match = hash.match(/id=([^&]+)/i);
            if (match && match[1]) projectId = decodeURIComponent(match[1]);
        } catch (ex) { }
        return 'pms_project_table_column_order_' + projectId;
    }

    function injectColumnMenuStyle() {
        if (document.getElementById('pmsColumnMenuStyle')) return;
        var css = document.createElement('style');
        css.id = 'pmsColumnMenuStyle';
        css.innerHTML = `
            .project-table thead th{ position:sticky; }
            .pms-th-inner{ display:flex; align-items:center; justify-content:center; gap:4px; width:100%; }
            .pms-th-text{ flex:1 1 auto; text-align:center; }
            .pms-th-menu-btn{
                flex:0 0 auto; border:0; background:transparent; color:#6b7280;
                width:20px; height:20px; line-height:18px; border-radius:50%;
                display:inline-flex; align-items:center; justify-content:center;
                font-size:15px; cursor:pointer; padding:0; margin-left:2px;
            }
            .pms-th-menu-btn:hover{ background:#f3e8ff; color:#7c006d; }
            .pms-column-menu{
                position:fixed; z-index:999999; min-width:150px; background:#fff;
                border:1px solid #ddd; border-radius:8px; box-shadow:0 8px 24px rgba(0,0,0,.18);
                padding:6px; font-size:13px; color:#111827;
            }
            .pms-column-menu .menu-title{font-weight:600; padding:7px 9px; border-bottom:1px solid #eee; margin-bottom:4px;}
            .pms-column-menu button{
                display:block; text-align:left; border:0; background:#fff;
                padding:7px 9px; border-radius:6px; cursor:pointer; color:#111827;
            }
            .pms-column-menu button:hover{background:#f3f4f6;}
            .pms-column-menu button:disabled{color:#aaa; cursor:not-allowed; background:#fff;}
            .pms-column-menu .menu-sep{height:1px;background:#eee;margin:5px 0;}
            .project-table thead th.column-draggable{ cursor:move; }
            .project-table thead th.column-draggable::after{ content:"" !important; }
        `;
        document.head.appendChild(css);
    }

    function ensureIndexes(table) {
        if (!table || !table.rows) return;
        Array.prototype.forEach.call(table.rows, function (row) {
            Array.prototype.forEach.call(row.children, function (cell, index) {
                if (!cell.hasAttribute('data-original-index')) {
                    cell.setAttribute('data-original-index', index);
                }
            });
        });
    }

    function getOriginalIndex(cell) {
        return parseInt(cell.getAttribute('data-original-index'), 10);
    }

    function getHeaderRow(table) {
        return table && table.tHead && table.tHead.rows.length ? table.tHead.rows[0] : null;
    }

    function getAllOriginalIndexes(table) {
        var row = getHeaderRow(table);
        if (!row) return [];
        return Array.prototype.map.call(row.cells, function (th) { return getOriginalIndex(th); });
    }

    function headerTitle(th) {
        if (!th) return '';
        var clone = th.cloneNode(true);
        Array.prototype.forEach.call(clone.querySelectorAll('.pms-th-menu-btn'), function (x) { x.remove(); });
        return (clone.innerText || clone.textContent || '').replace(/[↕]/g, '').replace(/\s+/g, ' ').trim() || 'Column';
    }

    function loadConfig(table) {
        var cfg = { order: [], hidden: [] };
        try {
            var raw = localStorage.getItem(storageKey());
            if (raw) cfg = JSON.parse(raw) || cfg;
        } catch (ex) { }

        // Backward support from old v5/v6 order-only storage
        if (!cfg.order || !cfg.order.length) {
            try {
                var old = JSON.parse(localStorage.getItem(oldOrderKey()) || '[]');
                if (old && old.length) cfg.order = old;
            } catch (ex2) { }
        }

        var all = getAllOriginalIndexes(table);
        if (!cfg.order || cfg.order.length !== all.length) cfg.order = all;
        if (!cfg.hidden) cfg.hidden = [];
        cfg.hidden = cfg.hidden.map(function (x) { return parseInt(x, 10); }).filter(function (x) { return !isNaN(x); });
        return cfg;
    }

    function saveConfig(table) {
        if (!table) return;
        var cfg = {
            order: getAllOriginalIndexes(table),
            hidden: []
        };
        Array.prototype.forEach.call(table.querySelectorAll('[data-original-index]'), function (cell) {
            var oi = getOriginalIndex(cell);
            if (cell.style.display === 'none' && cfg.hidden.indexOf(oi) === -1) cfg.hidden.push(oi);
        });
        localStorage.setItem(storageKey(), JSON.stringify(cfg));
        localStorage.setItem(oldOrderKey(), JSON.stringify(cfg.order));
    }

    function applyOrder(table, order) {
        if (!table || !order || !order.length) return;
        ensureIndexes(table);
        Array.prototype.forEach.call(table.rows, function (row) {
            order.forEach(function (originalIndex) {
                var found = null;
                Array.prototype.forEach.call(row.children, function (cell) {
                    if (getOriginalIndex(cell) === originalIndex) found = cell;
                });
                if (found) row.appendChild(found);
            });
        });
    }

    function applyHidden(table, hidden) {
        if (!table) return;
        hidden = hidden || [];
        Array.prototype.forEach.call(table.querySelectorAll('[data-original-index]'), function (cell) {
            var oi = getOriginalIndex(cell);
            cell.style.display = hidden.indexOf(oi) > -1 ? 'none' : '';
        });
    }

    function applyConfig(table) {
        if (!table) return;
        ensureIndexes(table);
        var cfg = loadConfig(table);
        applyOrder(table, cfg.order);
        applyHidden(table, cfg.hidden);
        decorateHeaders(table);
    }

    function closeMenu() {
        var old = document.getElementById(MENU_ID);
        if (old) old.remove();
    }

    function findHeaderByOriginal(table, originalIndex) {
        var row = getHeaderRow(table);
        if (!row) return null;
        var found = null;
        Array.prototype.forEach.call(row.cells, function (th) {
            if (getOriginalIndex(th) === originalIndex) found = th;
        });
        return found;
    }

    function moveOriginalBeforeAfter(table, hiddenOriginal, targetOriginal, pos) {
        var cfg = loadConfig(table);
        cfg.hidden = cfg.hidden.filter(function (x) { return x !== hiddenOriginal; });
        cfg.order = cfg.order.filter(function (x) { return x !== hiddenOriginal; });
        var targetIndex = cfg.order.indexOf(targetOriginal);
        if (targetIndex < 0) targetIndex = cfg.order.length - 1;
        if (pos === 'after') targetIndex++;
        cfg.order.splice(targetIndex, 0, hiddenOriginal);
        localStorage.setItem(storageKey(), JSON.stringify(cfg));
        localStorage.setItem(oldOrderKey(), JSON.stringify(cfg.order));
        applyConfig(table);
    }

    function hideColumn(table, originalIndex) {
        if (originalIndex < FIXED_COLUMNS) return;
        var cfg = loadConfig(table);
        if (cfg.hidden.indexOf(originalIndex) === -1) cfg.hidden.push(originalIndex);
        localStorage.setItem(storageKey(), JSON.stringify(cfg));
        applyConfig(table);
    }

    function showAllColumns(table) {
        var cfg = loadConfig(table);
        cfg.hidden = [];
        localStorage.setItem(storageKey(), JSON.stringify(cfg));
        applyConfig(table);
    }

    function hiddenColumns(table) {
        var cfg = loadConfig(table);
        return cfg.hidden.map(function (oi) {
            var th = findHeaderByOriginal(table, oi);
            return { originalIndex: oi, title: headerTitle(th) };
        });
    }

    function openMenu(e, th) {
        e.preventDefault();
        e.stopPropagation();
        closeMenu();

        var table = tableEl();
        if (!table) return;
        var oi = getOriginalIndex(th);
        var hidden = hiddenColumns(table);
        var title = headerTitle(th);

        var menu = document.createElement('div');
        menu.id = MENU_ID;
        menu.className = 'pms-column-menu';
        //  menu.innerHTML = '<div class="menu-title">' + title + '</div>';

        var hideBtn = document.createElement('button');
        hideBtn.type = 'button';
        hideBtn.innerHTML = 'Hide';
        hideBtn.className = 'p-0';
        hideBtn.disabled = oi < FIXED_COLUMNS;
        hideBtn.onclick = function (ev) { ev.stopPropagation(); hideColumn(table, oi); closeMenu(); };
        menu.appendChild(hideBtn);

        var showAll = document.createElement('button');
        showAll.type = 'button';
        showAll.innerHTML = 'Show all';
        showAll.className = 'p-0';
        showAll.disabled = hidden.length === 0;
        showAll.onclick = function (ev) { ev.stopPropagation(); showAllColumns(table); closeMenu(); };
        menu.appendChild(showAll);

        var sep = document.createElement('div');
        sep.className = 'menu-sep';
        menu.appendChild(sep);

        if (!hidden.length) {
            var none = document.createElement('button');
            none.type = 'button';
            none.disabled = true;
            none.innerHTML = 'No hidden columns';
            menu.appendChild(none);
        } else {
            hidden.forEach(function (h) {
                //var b1 = document.createElement('button');
                //b1.type = 'button';
                //b1.innerHTML = '<i class="bi bi-arrow-left-square" title="Add Before"></i> ' + h.title;
                //b1.onclick = function (ev) { ev.stopPropagation(); moveOriginalBeforeAfter(table, h.originalIndex, oi, 'before'); closeMenu(); };
                //menu.appendChild(b1);

                //var b2 = document.createElement('button');
                //b2.type = 'button';
                //b2.innerHTML = '<i class="bi bi-arrow-right-square" title="Add After"></i> ' + h.title;
                //b2.onclick = function (ev) { ev.stopPropagation(); moveOriginalBeforeAfter(table, h.originalIndex, oi, 'after'); closeMenu(); };
                //menu.appendChild(b2);

                var rowWrap = document.createElement('div');
                rowWrap.style.display = 'flex';
                rowWrap.style.gap = '6px';
                rowWrap.style.alignItems = 'center';

                // BEFORE BUTTON
                var b1 = document.createElement('button');
                b1.type = 'button';
                b1.className = 'd-flex align-items-center p-0';
                b1.innerHTML = '<i class="bx bx-caret-left me-1" bs-tooltip data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Add Before"></i> ';

                b1.onclick = function (ev) {
                    ev.stopPropagation();
                    moveOriginalBeforeAfter(table, h.originalIndex, oi, 'before');
                    closeMenu();
                };


                var bspan = document.createElement('span');
                bspan.type = 'span';
                bspan.className = 'd-flex align-items-center';
                bspan.innerHTML = ' ' + h.title + ' ';

                // AFTER BUTTON
                var b2 = document.createElement('button');
                b2.type = 'button';
                b2.className = 'd-flex align-items-center  p-0';
                b2.innerHTML = ' <i class="bx bx-caret-right ms-1" bs-tooltip data-bs-toggle="tooltip" data-bs-placement="top" data-bs-trigger="hover" title="Add After"></i>';

                b2.onclick = function (ev) {
                    ev.stopPropagation();
                    moveOriginalBeforeAfter(table, h.originalIndex, oi, 'after');
                    closeMenu();
                };

                // APPEND BOTH IN ONE LINE
                rowWrap.appendChild(b1);
                rowWrap.appendChild(bspan);
                rowWrap.appendChild(b2);

                menu.appendChild(rowWrap);

            });
        }

        document.body.appendChild(menu);
        var rect = th.getBoundingClientRect();
        var left = Math.min(rect.right - 150, window.innerWidth - 150);
        if (left < 8) left = 8;
        menu.style.left = left + 'px';
        menu.style.top = (rect.bottom + 4) + 'px';
    }

    function decorateHeaders(table) {
        injectColumnMenuStyle();
        var row = getHeaderRow(table);
        if (!row) return;

        Array.prototype.forEach.call(row.cells, function (th) {
            if (th.dataset.menuDecorated === '1') return;
            th.dataset.menuDecorated = '1';

            var originalHtml = th.innerHTML;
            th.innerHTML = '<div class="pms-th-inner"><span class="pms-th-text">' + originalHtml + '</span><button type="button" class="pms-th-menu-btn" title="Column options">⋮</button></div>';

            var btn = th.querySelector('.pms-th-menu-btn');
            btn.addEventListener('click', function (ev) { openMenu(ev, th); });
            btn.addEventListener('mousedown', function (ev) { ev.stopPropagation(); });
            btn.addEventListener('dragstart', function (ev) { ev.preventDefault(); ev.stopPropagation(); });
        });
    }

    function getHeaderIndex(cell) {
        if (!cell || !cell.parentNode) return -1;
        return Array.prototype.indexOf.call(cell.parentNode.children, cell);
    }

    function clearVisual(table) {
        var row = getHeaderRow(table);
        if (!row) return;
        Array.prototype.forEach.call(row.cells, function (th) {
            th.classList.remove('dragging-col');
            th.classList.remove('drag-over-col');
        });
    }

    function clearDrag(table) {
        clearVisual(table);
        if (table && table.dataset) delete table.dataset.dragFromIndex;
    }

    function moveColumn(table, fromIndex, toIndex) {
        if (!table || fromIndex === toIndex || fromIndex < 0 || toIndex < 0) return;
        ensureIndexes(table);
        Array.prototype.forEach.call(table.rows, function (row) {
            if (!row || !row.children || row.children.length <= Math.max(fromIndex, toIndex)) return;
            var movingCell = row.children[fromIndex];
            var ref = fromIndex < toIndex ? row.children[toIndex].nextSibling : row.children[toIndex];
            row.insertBefore(movingCell, ref);
        });
        applyHidden(table, loadConfig(table).hidden);
    }

    function rowObserver(table) {
        if (!table || !table.tBodies.length || table.dataset.rowObserverStartedV7 === '1') return;
        table.dataset.rowObserverStartedV7 = '1';
        var obs = new MutationObserver(function (mutations) {
            var hasNew = false;
            for (var i = 0; i < mutations.length; i++) {
                if (mutations[i].addedNodes && mutations[i].addedNodes.length) { hasNew = true; break; }
            }
            if (hasNew) applyConfig(table);
        });
        obs.observe(table.tBodies[0], { childList: true });
    }

    // Override previous function from v5/v6
    window.initProjectTableColumnDragDrop = function () {
       
        var table = tableEl();
        if (!table || !getHeaderRow(table)) return;

        injectColumnMenuStyle();
        ensureIndexes(table);
        decorateHeaders(table);
        applyConfig(table);

        var row = getHeaderRow(table);
        Array.prototype.forEach.call(row.cells, function (th) {
            var idx = getHeaderIndex(th);
            var oi = getOriginalIndex(th);

            if (oi < FIXED_COLUMNS) {
                th.classList.add('column-fixed');
                th.removeAttribute('draggable');
                return;
            }

            th.classList.add('column-draggable');
            th.setAttribute('draggable', 'true');

            if (th.dataset.dragBoundV7 === '1') return;
            th.dataset.dragBoundV7 = '1';

            th.addEventListener('dragstart', function (e) {
                closeMenu();
                var fromIndex = getHeaderIndex(this);
                var originalIndex = getOriginalIndex(this);
                if (originalIndex < FIXED_COLUMNS) { e.preventDefault(); return; }
                table.dataset.dragFromIndex = fromIndex;
                this.classList.add('dragging-col');
                if (e.dataTransfer) {
                    e.dataTransfer.effectAllowed = 'move';
                    try { e.dataTransfer.setData('text/plain', fromIndex.toString()); } catch (ex) { }
                }
            });

            th.addEventListener('dragover', function (e) {
                var fromIndex = parseInt(table.dataset.dragFromIndex, 10);
                var toIndex = getHeaderIndex(this);
                if (isNaN(fromIndex) || getOriginalIndex(this) < FIXED_COLUMNS) return;
                e.preventDefault();
                clearVisual(table);
                this.classList.add('drag-over-col');
                if (row.cells[fromIndex]) row.cells[fromIndex].classList.add('dragging-col');
            });

            th.addEventListener('drop', function (e) {
                e.preventDefault();
                var fromIndex = parseInt(table.dataset.dragFromIndex, 10);
                var toIndex = getHeaderIndex(this);
                if (isNaN(fromIndex) || getOriginalIndex(this) < FIXED_COLUMNS || fromIndex === toIndex) {
                    clearDrag(table);
                    return;
                }
                moveColumn(table, fromIndex, toIndex);
                saveConfig(table);
                clearDrag(table);
            });

            th.addEventListener('dragend', function () { clearDrag(table); });
        });

        rowObserver(table);
        applyConfig(table);
    };

    document.addEventListener('click', function () { closeMenu(); });
    window.addEventListener('resize', function () { closeMenu(); });
})();
