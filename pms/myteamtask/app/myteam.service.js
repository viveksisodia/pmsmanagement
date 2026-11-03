(function () {
    'use strict';

    angular.module('myTeamTaskApp').factory('myTeamService', [
        '$http',
        '$q',
        function ($http, $q) {

            var api = '/pms/myteamtask/default.aspx/';

            // ============================================================
            // Common POST helpers
            // ============================================================
            function post(method, data) {
                return $http({
                    method: 'POST',
                    url: api + method,
                    headers: {
                        'Content-Type': 'application/json; charset=utf-8'
                    },
                    data: data || {}
                }).then(function (r) {
                    return r.data.d;
                });
            }

            function postLocal(method, data) {
                return $http({
                    method: 'POST',
                    url: api + method,
                    headers: {
                        'Content-Type': 'application/json; charset=utf-8'
                    },
                    data: data || {}
                }).then(function (r) {
                    return r.data.d;
                });
            }

            // ============================================================
            // Date / time helpers
            // ============================================================
            function dateOnly(value) {
                if (!value) {
                    return '';
                }

                if (Object.prototype.toString.call(value) === '[object Date]') {
                    return value.getFullYear() + '-' +
                        String(value.getMonth() + 1).padStart(2, '0') + '-' +
                        String(value.getDate()).padStart(2, '0');
                }

                var m = /^(\d{4})-(\d{2})-(\d{2})/.exec(String(value));

                return m
                    ? m[1] + '-' + m[2] + '-' + m[3]
                    : String(value);
            }

            function dur(text) {
                var p = String(text || '1:00')
                    .replace('/', ':')
                    .split(':');

                return Math.max(
                    0,
                    (+p[0] || 0) * 60 + (+p[1] || 0)
                );
            }

            function tm(t) {
                var d = moment(
                    t,
                    ['hh:mm A', 'h:mm A', 'HH:mm'],
                    true
                );

                return d.isValid()
                    ? d.hours() * 60 + d.minutes()
                    : 0;
            }

            function shiftMins(a, b) {
                var s = tm(a);
                var e = tm(b);

                if (e <= s) {
                    e += 1440;
                }

                return e - s;
            }

            function time(n) {
                n = ((n % 1440) + 1440) % 1440;

                return String(Math.floor(n / 60)).padStart(2, '0') + ':' +
                    String(n % 60).padStart(2, '0');
            }

            function durationLabel(mins) {
                mins = Math.max(
                    0,
                    parseInt(mins || 0, 10) || 0
                );

                return String(Math.floor(mins / 60)).padStart(2, '0') + ':' +
                    String(mins % 60).padStart(2, '0') +
                    ' hrs';
            }

            // ============================================================
            // Convert Assign Project employee response into My Team format
            // ============================================================
            function mapEmployee(u, selectedDate) {
                var rows = u.usertasks || [];

                var assignments = (u.taskSegments || [])
                    .filter(function (s) {
                        return !s.workDateKey ||
                            dateOnly(s.workDateKey) === selectedDate;
                    })
                    .map(function (s, index) {
                        var mins = +s.DurationMinutes || dur(s.TaskDuration);
                        var type = String(s.tasktype || '').toLowerCase();
                        var assignId = +s.assignid || 0;
                        var entityId = +s.entityid || +s.taskid || 0;

                        var src = rows.find(function (r) {
                            return assignId &&
                                String(r.assignid || '') === String(assignId);
                        });

                        if (!src) {
                            src = rows.find(function (r) {
                                return String(r.tasktype || '').toLowerCase() === type &&
                                    String(r.id || '') === String(s.taskid || '');
                            });
                        }

                        src = src || {};

                        var milestoneName = src.Milestone || src.milestone || '';
                        var taskName = src.Taskname || src.taskname || '';
                        var subTaskName = src.Subtaskname || src.subtaskname || '';

                        var title = (
                            type === 'subtask'
                                ? (subTaskName || s.taskname)
                                : (
                                    type === 'task'
                                        ? (taskName || s.taskname)
                                        : (milestoneName || s.taskname)
                                )
                        ) || 'Assigned task';

                        var durationText = durationLabel(mins);

                        function enc(v) {
                            return String(v || '').replace(/&/g, '&amp;').replace(/</g, '&lt;')
                                .replace(/>/g, '&gt;').replace(/"/g, '&quot;').replace(/'/g, '&#39;');
                        }
                        var description = src.Description || src.SubTaskDescription || src.taskdescripton || src.taskdescription ||
                            src.TaskDescription || src.miledescription || src.Description || '';
                        var label = type === 'subtask' ? 'Subtask Name' :
                            type === 'milestone' ? 'Milestone Name' : 'Task Name';
                        var tooltip = 'Duration: ' + enc(durationText) +
                            '<br>' + label + ': <strong>' + enc(title) + '</strong>' +
                            '<br>Description: ' + enc(description || '-').replace(/\r?\n/g, '<br>');

                        return {
                            ClientKey:
                                'seg-' +
                                assignId + '-' +
                                selectedDate + '-' +
                                String(s.StartTime || '') + '-' +
                                index,

                            AssignId: assignId,
                            EntityId: entityId,
                            ProjectId: +(src.ProjectId || s.projectid) || 0,
                            ProjectName: src.ProjectName || '',
                            MilestoneId: +(src.Milestoneid || s.mid) || 0,
                            MilestoneName: milestoneName,
                            TaskId: +(src.Task || s.tid) || 0,
                            TaskName: taskName,
                            SubTaskId: +(src.id || s.subtaskid) || 0,
                            SubTaskName: subTaskName,
                            Type: type,

                            TypeLabel:
                                type === 'subtask'
                                    ? 'Subtask'
                                    : (type === 'task' ? 'Task' : 'Milestone'),

                            Title: title,
                            StartTime: s.StartTime,
                            EndTime: s.EndTime,
                            DurationMinutes: mins,
                            Duration: durationText,
                            TooltipHtml: tooltip,
                            WorkDate: s.workDateKey || s.workDate
                        };
                    });

                var booked = assignments.reduce(function (n, x) {
                    return n + x.DurationMinutes;
                }, 0);

                return {
                    EmployeeId: +u.id,
                    Name: u.name,
                    EmployeeCode: u.empid,
                    Photo: u.profileimage,
                    ShiftStart: u.ShiftStart || '09:00 AM',
                    ShiftEnd: u.ShiftEnd || '06:00 PM',
                    ShiftMinutes: shiftMins(u.ShiftStart, u.ShiftEnd),
                    IsWorkingDay: true,
                    Assignments: assignments,
                    AvailableMinutes: Math.max(
                        0,
                        shiftMins(u.ShiftStart, u.ShiftEnd) - booked
                    )
                };
            }

            function ids(users) {
                return (users || [])
                    .map(function (u) {
                        return u.id;
                    })
                    .filter(Boolean)
                    .join(',');
            }

            // ============================================================
            // Initial data: projects + My Team employees
            // ============================================================
            function bootstrap() {
                return $q.all([
                    // My Team project list:
                    // Admin => all active projects.
                    // User  => only projects in which the user is involved.
                    postLocal('GetMyTeamProjects', {}),
                    postLocal('GetMyTeamEmployees', {})
                ]).then(function (x) {
                    return {
                        Projects: (x[0] || []).map(function (p) {
                            return {
                                Id: +p.id,
                                Name: p.ProjectName
                            };
                        }),
                        Employees: x[1] || [],
                        IsAdmin: !!(
                            x[1] &&
                            x[1][0] &&
                            +x[1][0].isadmin === 1
                        ),
                        DefaultShiftStart: '09:00 AM',
                        DefaultShiftEnd: '06:00 PM'
                    };
                });
            }

            // ============================================================
            // Load selected day employees + existing assigned task segments
            // ============================================================
            function day(workDate, projectId, allEmployees) {
                var selected = dateOnly(workDate);
                var allowed = {};

                (allEmployees || []).forEach(function (e) {
                    allowed[String(e.id)] = true;
                });

                var ep = projectId
                    ? post('GetMyTeamProjectMembers', {
                        projectId: String(projectId)
                    }).then(function (members) {
                        return (members || []).filter(function (e) {
                            return allowed[String(e.id)];
                        });
                    })
                    : $q.when(allEmployees || []);

                return ep
                    .then(function (es) {
                        var x = ids(es);

                        return x
                            ? post('GetMyTeamScheduleUsers', {
                                userIds: x,
                                checkdate: selected
                            })
                            : [];
                    })
                    .then(function (users) {
                        users = users || [];

                        var starts = users.map(function (u) {
                            return tm(u.ShiftStart);
                        });

                        var ends = users.map(function (u) {
                            var s = tm(u.ShiftStart);
                            var e = tm(u.ShiftEnd);

                            return e <= s
                                ? e + 1440
                                : e;
                        });

                        var start = starts.length
                            ? Math.min.apply(Math, starts)
                            : 540;

                        var end = ends.length
                            ? Math.max.apply(Math, ends)
                            : 1080;

                        // Use one common grid from the earliest employee shift start
                        // to the latest employee shift end. Different shifts must not
                        // force a 24-hour timeline.
                        start = Math.floor(start / 60) * 60;
                        end = Math.ceil(end / 60) * 60;

                        if (end <= start) {
                            end = start + 60;
                        }

                        return {
                            DayStart: time(start),
                            DayEnd: time(end),
                            Users: users.map(function (u) {
                                return mapEmployee(u, selected);
                            })
                        };
                    });
            }

            // ============================================================
            // Right-side assignable task list
            // ============================================================
            function taskName(t) {
                return t.Subtaskname ||
                    t.Taskname ||
                    t.taskname ||
                    t.Milestone ||
                    'Assigned task';
            }

            function assignable(employeeId, workDate, projectId) {
                // The My Team popup is intentionally independent from Assign Project.
                // projectId = 0 means: show all projects available to the logged-in user.
                return post('GetMyTeamAssignableTree', {
                    employeeId: String(employeeId || 0),
                    workDate: dateOnly(workDate),
                    projectId: String(projectId || 0)
                }).then(function (r) {
                    r = r || {};
                    return {
                        Projects: r.Projects || [],
                        Subtasks: r.Subtasks || []
                    };
                });
            }

            // ============================================================
            // Save helpers
            // ============================================================
            function ok(v) {
                if (v !== 1 && v !== '1') {
                    return $q.reject(v || 'Operation failed.');
                }

                return v;
            }

            function save(r) {
                var chain = $q.when();

                // Remove existing assignments first.
                (r.RemovedAssignments || []).forEach(function (a) {
                    chain = chain.then(function () {
                        return post('MyTeamRemoveAssignment', {
                            id: String(a.AssignId),
                            utype: a.Type
                        }).then(ok);
                    });
                });

                // Add new assignments or update existing assignment time.
                (r.Assignments || []).forEach(function (a) {
                    chain = chain.then(function () {
                        if (a.AssignId) {
                            return post('MyTeamUpdateAssignment', {
                                id: String(a.AssignId),
                                utype: a.TaskType,
                                starttime: a.StartTime,
                                taskduration: a.Duration,
                                changedate: dateOnly(r.WorkDate),
                                oldstarttime: a.OldStartTime || '',
                                oldchangedate: dateOnly(
                                    a.OldWorkDate || r.WorkDate
                                ),
                                editmode: a.EditMode || ''
                            }).then(ok);
                        }

                        return post('MyTeamAssignItem', {
                            id: String(a.EntityId),
                            empid: String(r.EmployeeId),
                            utype: a.TaskType,
                            changedate: dateOnly(r.WorkDate),
                            starttime: a.StartTime
                        }).then(ok);
                    });
                });

                return chain.then(function () {
                    return {
                        Message: 'Team assignments saved successfully.'
                    };
                });
            }

            // ============================================================
            // Parent hierarchy drop helpers
            // Milestone/Task are drag shortcuts only. The server still saves
            // one Subtask assignment at a time.
            // ============================================================
            function findNextAvailableSubtaskStart(item, employeeId, workDate, startTime) {
                item = item || {};
                return post('MyTeamFindNextAvailableSubtaskStart', {
                    id: String(item.EntityId || item.SubTaskId || 0),
                    empid: String(employeeId || 0),
                    changedate: dateOnly(workDate),
                    starttime: startTime || '09:00 AM'
                });
            }

            function assignSubtaskDirect(item, employeeId, workDate, startTime) {
                item = item || {};
                return post('MyTeamAssignItem', {
                    id: String(item.EntityId || item.SubTaskId || 0),
                    empid: String(employeeId || 0),
                    utype: 'subtask',
                    changedate: dateOnly(workDate),
                    starttime: startTime || '09:00 AM'
                });
            }

            // ============================================================
            // Public service methods
            // ============================================================
            return {
                moveAssignment: function (assignmentId, employeeId, workDate, startTime) {
                    return post('MyTeamMoveAssignment', {
                        id: String(assignmentId), empid: String(employeeId),
                        changedate: dateOnly(workDate), starttime: startTime
                    }).then(ok);
                },
                bootstrap: bootstrap,
                day: day,
                assignable: assignable,
                save: save,
                findNextAvailableSubtaskStart: findNextAvailableSubtaskStart,
                assignSubtaskDirect: assignSubtaskDirect,
                toDateOnly: dateOnly
            };
        }
    ]);
})();
