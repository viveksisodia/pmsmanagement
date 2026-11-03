app.factory("assignprojectService", function ($http) {
    function getGanttNonWorkingDates(startdate, enddate) {
        return $http.post('/pms/assignprojects/default.aspx/GetGanttNonWorkingDates', {
            startdate: startdate, enddate: enddate
        });
    }
    var configuredPerDayHours = 8;

    function normalizePerDayHours(value) {
        var parsed = Number(value);
        if (!isFinite(parsed) || parsed <= 0) parsed = 8;
        return parsed;
    }

    function normalizeStartTime(value) {
        return value === undefined || value === null
            ? ''
            : String(value).trim();
    }


    function normalizeProjectId(value) {
        if (value && typeof value === 'object') {
            value = value.ProjectId || value.projectId || value.projectid || value.id;
        }

        if (value !== undefined && value !== null) {
            var text = String(value).trim();
            if (text && text !== '0') return text;
        }

        try {
            var queryId = new URLSearchParams(window.location.search).get('id');
            if (queryId && String(queryId).trim() !== '0') {
                return String(queryId).trim();
            }
        } catch (ignore) { }

        return '0';
    }

    // Hierarchy APIs accept only yyyy-MM-dd; never serialize a JavaScript Date as UTC.
    function normalizeDurationText(value, separateMinutes) {
        var raw = value === undefined || value === null ? '' : String(value).trim();
        var rawMinutes = separateMinutes === undefined || separateMinutes === null ? '' : String(separateMinutes).trim();
        if (!raw && !rawMinutes) return '';

        raw = raw.replace(/hrs?|hours?/ig, '').trim();
        var separator = raw.indexOf(':') >= 0 ? ':' : (raw.indexOf('/') >= 0 ? '/' : null);
        var hours = 0;
        var minutes = 0;

        if (separator) {
            var parts = raw.split(separator);
            hours = parseInt(parts[0] || '0', 10);
            minutes = parseInt((parts[1] !== undefined && String(parts[1]).trim() !== '') ? parts[1] : (rawMinutes || '0'), 10);
        } else if (raw.indexOf('.') >= 0 && rawMinutes === '') {
            var decimalHours = parseFloat(raw);
            if (!isNaN(decimalHours)) {
                var decimalTotal = Math.round(decimalHours * 60);
                hours = Math.floor(decimalTotal / 60);
                minutes = decimalTotal % 60;
            }
        } else {
            hours = parseInt(raw || '0', 10);
            minutes = parseInt(rawMinutes || '0', 10);
        }

        if (isNaN(hours) || hours < 0) hours = 0;
        if (isNaN(minutes) || minutes < 0) minutes = 0;

        var total = (hours * 60) + minutes;
        return String(Math.floor(total / 60)) + ':' + String(total % 60).padStart(2, '0');
    }

    function toHierarchyDateOnly(value) {
        if (value === null || value === undefined || value === '') return value;

        if (Object.prototype.toString.call(value) === '[object Date]') {
            if (isNaN(value.getTime())) return '';
            return value.getFullYear() + '-' +
                String(value.getMonth() + 1).padStart(2, '0') + '-' +
                String(value.getDate()).padStart(2, '0');
        }

        var text = String(value).trim();
        var iso = /^(\d{4})-(\d{2})-(\d{2})/.exec(text);
        if (iso) return iso[1] + '-' + iso[2] + '-' + iso[3];

        var ui = /^(\d{2})[\/-](\d{2})[\/-](\d{4})$/.exec(text);
        if (ui) return ui[3] + '-' + ui[2] + '-' + ui[1];

        return text;
    }

    function normalizeHierarchyObject(value, type) {
        var obj = angular.copy(value || {});
        type = String(type || '').toLowerCase();

        if (type === 'milestone') {
            obj.StartDate = toHierarchyDateOnly(obj.StartDate);
            obj.EndDate = toHierarchyDateOnly(obj.EndDate);
        } else {
            obj.Startdate = toHierarchyDateOnly(obj.Startdate);
            obj.Targetdate = toHierarchyDateOnly(obj.Targetdate);
        }

        if (obj.iscompletedate !== undefined && obj.iscompletedate !== null && obj.iscompletedate !== '') {
            obj.iscompletedate = toHierarchyDateOnly(obj.iscompletedate);
        }

        // Always send duration as one invariant H:mm value. This prevents an IIS/server
        // response with separate fields (2 and 30) from being saved as only 2:.
        var normalizedDuration = normalizeDurationText(obj.TaskDuration, obj.TaskDuration_min);
        if (normalizedDuration) obj.TaskDuration = normalizedDuration;
        return obj;
    }

    function normalizeHierarchyColumnValue(columnName, value) {
        var name = String(columnName || '').toLowerCase();
        if (name === 'startdate' || name === 'enddate' || name === 'targetdate' ||
            name === 'awarddate' || name === 'iscompletedate')
        {
            return toHierarchyDateOnly(value);
        }
        if (name === 'taskduration') {
            return normalizeDurationText(value, '');
        }
        return value;
    }
    function toAssignDateOnly(value) {
        if (value === null || value === undefined || value === '') return '';
        if (Object.prototype.toString.call(value) === '[object Date]') {
            if (isNaN(value.getTime())) return '';
            return value.getFullYear() + '-' +
                String(value.getMonth() + 1).padStart(2, '0') + '-' +
                String(value.getDate()).padStart(2, '0');
        }
        var text = String(value).trim();
        var iso = /^(\d{4})-(\d{2})-(\d{2})/.exec(text);
        if (iso) return iso[1] + '-' + iso[2] + '-' + iso[3];
        // Scheduler/assignment UI uses MM/DD/YYYY.
        var us = /^(\d{1,2})\/(\d{1,2})\/(\d{4})$/.exec(text);
        if (us) return us[3] + '-' + String(us[1]).padStart(2, '0') + '-' + String(us[2]).padStart(2, '0');

        return text;
    }

    return {
        getGanttNonWorkingDates: getGanttNonWorkingDates,
        setPerDayHours: function (value) {
            configuredPerDayHours = normalizePerDayHours(value);
            return configuredPerDayHours;
        },
        getPerDayHours: function () {
            return configuredPerDayHours;
        },
        getprojectmasterlist: function (cpid) {
            return $http({
                method: "POST",
                url: "/pms/assignprojects/default.aspx/bindProjectdata",
                headers: { "Content-Type": "application/json" },
                data: { currentid: cpid }
            });
        },
        getprevious_projectdetails: function (cpid) {
            return $http({
                method: "POST",
                url: "/pms/assignprojects/default.aspx/bindProjectdetail_pre",
                headers: { "Content-Type": "application/json" },
                data: { RecordId: cpid }
            });
        },
        copyProject: function (cpid, projectid) {
            return $http({
                method: "POST",
                url: "/pms/assignprojects/default.aspx/copyproject",
                headers: { "Content-Type": "application/json" },
                data: { currentid: cpid, copyprojectid: projectid }
            });
        },
        getprojectlistbyid: function (projectid) {
            //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/assignprojects/default.aspx/bindtaskdata",
                headers: { "Content-Type": "application/json" },
                data: JSON.stringify({
                    RecordId: projectid

                })
            });
        },
        gettimespent: function (tid,ttype,ownerid) {
            //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/assignprojects/default.aspx/gettimespam",
                headers: { "Content-Type": "application/json" },
                data: JSON.stringify({
                    id: tid,
                    tasktype:ttype,
                    owner:ownerid

                })
            });
        },
        getunassigntasklist: function (c,gtype) {
            //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/assignprojects/default.aspx/getunassigntasklist",
                headers: { "Content-Type": "application/json" },
                data: JSON.stringify({
                    ProjectId: c.ProjectId,
                    MilestoneId: (gtype == 'milestone'?c.id:c.Milestoneid),
                    RecordId: c.id,
                    Rtype: gtype

                })
            });
        },
        getuserwithtasklist: function (userIds,checkdates) {
            //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/assignprojects/default.aspx/getuserwithtasklist",
                headers: { "Content-Type": "application/json" },
                data: JSON.stringify({
                    userIds: userIds,
                    checkdate: toAssignDateOnly(checkdates)

                })
            });
        },
getuserwithtasklist_bysingleid: function (userIds,checkdates) {
            //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/assignprojects/default.aspx/getuserwithtasklist_byempid",
                headers: { "Content-Type": "application/json" },
                data: JSON.stringify({
                    userIds: userIds,
                    checkdate: toAssignDateOnly(checkdates)

                })
            });
        },
        gettasklistbyid: function (milestonid) {
            //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/assignprojects/default.aspx/bindmile_tasklist",
                headers: { "Content-Type": "application/json" },
                data: JSON.stringify({
                    RecordId: milestonid

                })
            });
        },
        bindcompletiondetails_mile: function (milestonid) {
            //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/assignprojects/default.aspx/bindcompletiondetails_mile",
                headers: { "Content-Type": "application/json" },
                data: JSON.stringify({
                    RecordId: milestonid

                })
            });
        },
        bindcompletiondetails_task: function (taskid) {
            //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/assignprojects/default.aspx/bindcompletiondetails_task",
                headers: { "Content-Type": "application/json" },
                data: JSON.stringify({
                    RecordId: taskid

                })
            });
        },
        bindcompletiondetails_subtask: function (subtaskid) {
            //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/assignprojects/default.aspx/bindcompletiondetails_subtask",
                headers: { "Content-Type": "application/json" },
                data: JSON.stringify({
                    RecordId: subtaskid

                })
            });
        },
        getstatuslist: function () {
            //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/assignprojects/default.aspx/bindstatuslist",
                headers: { "Content-Type": "application/json" },
                data: JSON.stringify({})
            });
        },
        gettasksituationlist: function () {
            return $http({
                method: "POST",
                url: "/pms/assignprojects/default.aspx/bindtasksituationlist",
                headers: { "Content-Type": "application/json" },
                data: JSON.stringify({})
            });
        },
        getprioritylist: function () {
            //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/assignprojects/default.aspx/bindprioritylist",
                headers: { "Content-Type": "application/json" },
                data: JSON.stringify({})
            });
        },
        getuserlist: function () {
            //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/assignprojects/default.aspx/binduserlist",
                headers: { "Content-Type": "application/json" },
                data: {}
            });
        },
        getuserlist_byrefid: function (refid, usertype, searchtype) {
            //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/assignprojects/default.aspx/binduserlist_byrefid",
                headers: { "Content-Type": "application/json" },
                data: {
                    id: refid,
                    utype: usertype,   // <-- comma added here
                    stype: searchtype
                }
            });
        },

        UpdateMilestone_Status: function (upd_id, upd_statusid, upd_iscomplete) {
            //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/assignprojects/default.aspx/UpdateMilestone_Status",
                headers: { "Content-Type": "application/json" },
                data: {
                    milstoneid: upd_id,
                    statusid: upd_statusid,
                    iscomplete: upd_iscomplete
                }
            });
        },
        MileUpdatePriority_Status: function (upd_id, upd_priorityid) {
            //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/assignprojects/default.aspx/MileUpdatePriority_Status",
                headers: { "Content-Type": "application/json" },
                data: {
                    milstoneid: upd_id,
                    priorityid: upd_priorityid
                }
            });
        },
        updatedetails_mile_bycol: function (upd_id, col_name, col_value, perdayhrs) {


            return $http({
                method: "POST",
                url: "/pms/assignprojects/default.aspx/MileUpdateDetails",
                headers: { "Content-Type": "application/json" },
                data: {
                    id: upd_id,
                    col_name: col_name,
                    col_value: normalizeHierarchyColumnValue(col_name, col_value),
                    perdayhrs: String(normalizePerDayHours(
                        perdayhrs === undefined ? configuredPerDayHours : perdayhrs
                    ))
                }
            });
        },
        addnewmilestonerow: function (object, orderno) {
            debugger
            return $http({
                method: "POST",
                url: "/pms/assignprojects/default.aspx/addnewmilestonerrequest",
                headers: { "Content-Type": "application/json" },
                data: angular.toJson({
                    request: {
                        milestone: normalizeHierarchyObject(object, 'milestone'),
                        orderno: orderno
                    }
                })
            });
        },
        removemilestone: function (mid) {
            debugger
            return $http({
                method: "POST",
                url: "/pms/assignprojects/default.aspx/deletemilestone",
                headers: { "Content-Type": "application/json" },
                data: { id: mid }
            });
        },
        updateorderno_milestone: function (mid, morderno) {
            debugger
            return $http({
                method: "POST",
                url: "/pms/assignprojects/default.aspx/updatemilestone_order",
                headers: { "Content-Type": "application/json" },
                data: { id: mid, orderno: morderno }
            });
        },
        updatetimeline_mile: function (mid, adddays, fdate, starttime, perdayhrs, calculateddays) {
            return $http({
                method: "POST",
                url: "/pms/assignprojects/default.aspx/updatetimeline_mile",
                headers: { "Content-Type": "application/json" },
                data: JSON.stringify({
                    id: mid,
                    ndays: adddays,
                    startdate: toHierarchyDateOnly(fdate),
                    starttime: normalizeStartTime(starttime),
                    perdayhrs: String(normalizePerDayHours(
                        perdayhrs === undefined ? configuredPerDayHours : perdayhrs
                    )),
                    calculateddays: calculateddays === undefined || calculateddays === null
                        ? ''
                        : String(calculateddays)
                })
            });
        },
        addnewtaskrow: function (object, orderno) {
            debugger
            return $http({
                method: "POST",
                url: "/pms/assignprojects/default.aspx/addnewtaskrequest",
                headers: { "Content-Type": "application/json" },
                data: angular.toJson({
                    request: {
                        task: normalizeHierarchyObject(object, 'task'),
                        orderno: orderno
                    }
                })
            });
        },
        removetask: function (tid) {
            debugger
            return $http({
                method: "POST",
                url: "/pms/assignprojects/default.aspx/deletetask",
                headers: { "Content-Type": "application/json" },
                data: { id: tid }
            });
        },
        updateorderno_task: function (tid, morderno) {
            debugger
            return $http({
                method: "POST",
                url: "/pms/assignprojects/default.aspx/updatetask_order",
                headers: { "Content-Type": "application/json" },
                data: { id: tid, orderno: morderno }
            });
        },
        UpdateTask_Status: function (upd_id, upd_statusid, upd_iscomplete) {
            //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/assignprojects/default.aspx/UpdateTask_Status",
                headers: { "Content-Type": "application/json" },
                data: {
                    taskid: upd_id,
                    statusid: upd_statusid,
                    iscomplete: upd_iscomplete
                }
            });
        },
        TaskUpdatePriority_Status: function (upd_id, upd_priorityid) {
            //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/assignprojects/default.aspx/TaskUpdatePriority_Status",
                headers: { "Content-Type": "application/json" },
                data: {
                    taskid: upd_id,
                    priorityid: upd_priorityid
                }
            });
        },
        updatedetails_task_bycol: function (upd_id, col_name, col_value, perdayhrs) {


            return $http({
                method: "POST",
                url: "/pms/assignprojects/default.aspx/TaskUpdateDetails",
                headers: { "Content-Type": "application/json" },
                data: {
                    id: upd_id,
                    col_name: col_name,
                    col_value: normalizeHierarchyColumnValue(col_name, col_value),
                    perdayhrs: String(normalizePerDayHours(
                        perdayhrs === undefined ? configuredPerDayHours : perdayhrs
                    ))
                }
            });
        },

        updatetimeline_task: function (tid, adddays, fdate, starttime, perdayhrs, calculateddays) {
            return $http({
                method: "POST",
                url: "/pms/assignprojects/default.aspx/updatetimeline_task",
                headers: { "Content-Type": "application/json" },
                data: JSON.stringify({
                    id: tid,
                    ndays: adddays,
                    startdate: toHierarchyDateOnly(fdate),
                    starttime: normalizeStartTime(starttime),
                    perdayhrs: String(normalizePerDayHours(
                        perdayhrs === undefined ? configuredPerDayHours : perdayhrs
                    )),
                    calculateddays: calculateddays === undefined || calculateddays === null
                        ? ''
                        : String(calculateddays)
                })
            });
        },
        addnewsubtaskrow: function (object, orderno) {
            debugger
            return $http({
                method: "POST",
                url: "/pms/assignprojects/default.aspx/addnewsubtaskrequest",
                headers: { "Content-Type": "application/json" },
                data: angular.toJson({
                    request: {
                        subtask: normalizeHierarchyObject(object, 'subtask'),
                        orderno: orderno
                    }
                })
            });
        },
        removesubtask: function (sid) {
            debugger
            return $http({
                method: "POST",
                url: "/pms/assignprojects/default.aspx/deletesubtask",
                headers: { "Content-Type": "application/json" },
                data: { id: sid }
            });
        },
        updateorderno_subtask: function (sid, morderno) {
            debugger
            return $http({
                method: "POST",
                url: "/pms/assignprojects/default.aspx/updatesubtask_order",
                headers: { "Content-Type": "application/json" },
                data: { id: sid, orderno: morderno }
            });
        },
        UpdateSubTask_Status: function (upd_id, upd_statusid, upd_iscomplete) {
            //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/assignprojects/default.aspx/UpdateSubtask_Status",
                headers: { "Content-Type": "application/json" },
                data: {
                    subtaskid: upd_id,
                    statusid: upd_statusid,
                    iscomplete: upd_iscomplete
                }
            });
        },
        SubtaskUpdatePriority_Status: function (upd_id, upd_priorityid) {
            //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/assignprojects/default.aspx/SubtaskUpdatePriority_Status",
                headers: { "Content-Type": "application/json" },
                data: {
                    subtaskid: upd_id,
                    priorityid: upd_priorityid
                }
            });
        },
        updatedetails_subtask_bycol: function (upd_id, col_name, col_value, perdayhrs) {


            return $http({
                method: "POST",
                url: "/pms/assignprojects/default.aspx/SubTaskUpdateDetails",
                headers: { "Content-Type": "application/json" },
                data: {
                    id: upd_id,
                    col_name: col_name,
                    col_value: normalizeHierarchyColumnValue(col_name, col_value),
                    perdayhrs: String(normalizePerDayHours(
                        perdayhrs === undefined ? configuredPerDayHours : perdayhrs
                    ))
                }
            });
        },

        updatetimeline_subtask: function (sid, adddays, fdate, starttime, perdayhrs, calculateddays) {
            return $http({
                method: "POST",
                url: "/pms/assignprojects/default.aspx/updatetimeline_subtask",
                headers: { "Content-Type": "application/json" },
                data: JSON.stringify({
                    id: sid,
                    ndays: adddays,
                    startdate: toHierarchyDateOnly(fdate),
                    starttime: normalizeStartTime(starttime),
                    perdayhrs: String(normalizePerDayHours(
                        perdayhrs === undefined ? configuredPerDayHours : perdayhrs
                    )),
                    calculateddays: calculateddays === undefined || calculateddays === null
                        ? ''
                        : String(calculateddays)
                })
            });
        },
        getleftdays: function (id, utype) {
            debugger
            return $http({
                method: "POST",
                url: "/pms/assignprojects/default.aspx/getleftdays",
                headers: { "Content-Type": "application/json" },
                data: JSON.stringify({
                    id: id,
                    gtype: utype

                })
            });
        },
        getweightage: function (id, utype) {
            debugger
            return $http({
                method: "POST",
                url: "/pms/assignprojects/default.aspx/get_newweightage",
                headers: { "Content-Type": "application/json" },
                data: JSON.stringify({
                    id: id,
                    gtype: utype

                })
            });
        },
        getweightage_default: function (id, utype) {
            debugger
            return $http({
                method: "POST",
                url: "/pms/assignprojects/default.aspx/get_newweightage2",
                headers: { "Content-Type": "application/json" },
                data: JSON.stringify({
                    id: id,
                    gtype: utype

                })
            });
        },
        getnew_enddate: function (fdate, adddays,projectid) {
            debugger
            return $http({
                method: "POST",
                url: "/pms/assignprojects/default.aspx/manage_enddate",
                headers: { "Content-Type": "application/json" },
                data: JSON.stringify({
                    ndays: adddays,
                    startdate: toHierarchyDateOnly(fdate),
                    pid: normalizeProjectId(projectid)

                })
            });
        },
        getProjectDurationDays: function (startdate, enddate) {
            return $http({
                method: "POST",
                url: "/pms/assignprojects/default.aspx/manage_durationdays",
                headers: { "Content-Type": "application/json" },
                data: JSON.stringify({
                    startdate: toHierarchyDateOnly(startdate),
                    enddate: toHierarchyDateOnly(enddate)
                })
            });
        },

        UpdateMilestone_Owner: function (upd_id, upd_ownerid) {
            //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/assignprojects/default.aspx/UpdateMilestone_Owner",
                headers: { "Content-Type": "application/json" },
                data: {
                    id: upd_id,
                    empids: upd_ownerid
                }
            });
        },
        UpdateTask_Owner: function (upd_id, upd_ownerid) {
            //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/assignprojects/default.aspx/UpdateTask_Owner",
                headers: { "Content-Type": "application/json" },
                data: {
                    id: upd_id,
                    empids: upd_ownerid
                }
            });
        },
        UpdateSubtask_Owner: function (upd_id, upd_ownerid) {
            //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/assignprojects/default.aspx/UpdateSubtask_Owner",
                headers: { "Content-Type": "application/json" },
                data: {
                    id: upd_id,
                    empids: upd_ownerid
                }
            });
        },
        UpdateMilestone_Team: function (upd_id, upd_ownerid) {
            //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/assignprojects/default.aspx/UpdateMilestone_Team",
                headers: { "Content-Type": "application/json" },
                data: {
                    id: upd_id,
                    empids:upd_ownerid
                }
            });
        },
        UpdateTask_Team: function (upd_id, upd_ownerid) {
            //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/assignprojects/default.aspx/UpdateTask_Team",
                headers: { "Content-Type": "application/json" },
                data: {
                    id: upd_id,
                    empids: upd_ownerid
                }
            });
        },
        UpdateSubtask_Team: function (upd_id, upd_ownerid) {
            //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/assignprojects/default.aspx/UpdateSubtask_Team",
                headers: { "Content-Type": "application/json" },
                data: {
                    id: upd_id,
                    empids: upd_ownerid
                }
            });
        },
        getissuesdetails: function (id, utype) {
            //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/assignprojects/default.aspx/bindissues_details",
                headers: { "Content-Type": "application/json" },
                data: {
                    id: id,
                    issuetype: utype
                }
            });
        },
        getdependencydetails: function (id, utype) {
            //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/assignprojects/default.aspx/binddependency_details",
                headers: { "Content-Type": "application/json" },
                data: {
                    id: id,
                    type: utype
                }
            });
        },
        ganttDependency: function (action, payload) {
            return $http({
                method: 'POST',
                url: '/pms/assignprojects/default.aspx/GanttDependency',
                headers: { 'Content-Type': 'application/json' },
                data: angular.extend({ action: action }, payload || {})
            });
        },
        updateGanttDependency: function (payload) {
            return $http({
                method: 'POST',
                url: '/pms/assignprojects/default.aspx/UpdateGanttDependency',
                headers: { 'Content-Type': 'application/json' },
                data: payload || {}
            });
        },
        getitemdetails: function (id, utype) {
            //alert(projectid);
            var geturl = "/pms/assignprojects/default.aspx/binditemdetails_mile";
            if (utype == "task") {
                geturl = "/pms/assignprojects/default.aspx/binditemdetails_task";
            }
            else if (utype == "subtask") {
                geturl = "/pms/assignprojects/default.aspx/binditemdetails_subtask";
            }
            return $http({
                method: "POST",
                url: geturl,
                headers: { "Content-Type": "application/json" },
                data: {
                    id: id,
                    type: utype
                }
            });
        },
Updatetaskassignto: function (assignid, assignto, utype,ondate,ontime) {
            //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/assignprojects/default.aspx/updateOwner_assign",
                headers: { "Content-Type": "application/json" },
                data: {
                    id: assignid,
                    empid: assignto,
                    utype: utype,
                    changedate: toAssignDateOnly(ondate),
                    starttime:ontime,

                }
            });
        }
        ,
        SelectUpdateowner: function (assignid, assignto, utype) {
            //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/assignprojects/default.aspx/updateOwner",
                headers: { "Content-Type": "application/json" },
                data: {
                    id: assignid,
                    empid: assignto,
                    utype: utype

                }
            });
        },
        updateKanbanOwner: function (recordId, ownerId, utype) {
            return $http({
                method: "POST",
                url: "/pms/assignprojects/default.aspx/UpdateKanbanOwner",
                headers: { "Content-Type": "application/json" },
                data: {
                    id: recordId,
                    empid: ownerId,
                    utype: utype
                }
            });
        },
        updateKanbanDuration: function (recordId, hours, minutes, utype, perdayhrs) {
            return $http({
                method: "POST",
                url: "/pms/assignprojects/default.aspx/UpdateKanbanDuration",
                headers: { "Content-Type": "application/json" },
                data: {
                    id: recordId,
                    hours: hours,
                    minutes: minutes,
                    utype: utype,
                    perdayhrs: String(normalizePerDayHours(
                        perdayhrs === undefined ? configuredPerDayHours : perdayhrs
                    ))
                }
            });
        },
ShiftAssignTaskWithExisting: function (assignid, assignto, utype, ondate, ontime) {
            return $http({
                method: "POST",
                url: "/pms/assignprojects/default.aspx/shiftAssignTaskWithExisting",
                headers: { "Content-Type": "application/json" },
                data: {
                    id: assignid,
                    empid: assignto,
                    utype: utype,
                    changedate: toAssignDateOnly(ondate),
                    starttime: ontime
                }
            });
        }

,

FindNextAvailableAssignStart: function (assignid, assignto, utype, ondate, ontime) {
            return $http({
                method: "POST",
                url: "/pms/assignprojects/default.aspx/findNextAvailableAssignStart",
                headers: { "Content-Type": "application/json" },
                data: {
                    id: assignid,
                    empid: assignto,
                    utype: utype,
                    changedate: toAssignDateOnly(ondate),
                    starttime: ontime
                }
            });
        }

,

UpdateAssignOwnerTime: function (assignid, empid, changedate, starttime) {
            return $http({
                method: "POST",
                url: "/pms/assignprojects/default.aspx/updateAssignOwnerTime",
                headers: { "Content-Type": "application/json" },
                data: {
                    assignid: assignid,
                    empid: empid,
                    changedate: toAssignDateOnly(changedate),
                    starttime: starttime
                }
            });
        }
,
UpdateAssignTaskTime: function (assignid, utype, starttime, taskduration, changedate, oldstarttime, oldchangedate, editmode) {
            return $http({
                method: "POST",
                url: "/pms/assignprojects/default.aspx/updateAssignTaskTime",
                headers: { "Content-Type": "application/json" },
                data: {
                    id: assignid,
                    utype: utype,
                    starttime: starttime,
                    taskduration: taskduration,
                    changedate: toAssignDateOnly(changedate),
                    oldstarttime: oldstarttime || '',
                    oldchangedate: toAssignDateOnly(oldchangedate || changedate),
                    editmode: editmode || ''
                }
            });
        }
        ,
        updateHierarchyEndDate: function (recordId, utype, enddate) {
            return $http({
                method: "POST",
                url: "/pms/assignprojects/default.aspx/UpdateHierarchyEndDate",
                headers: { "Content-Type": "application/json" },
                data: JSON.stringify({
                    id: String(recordId),
                    utype: String(utype || ''),
                    enddate: toHierarchyDateOnly(enddate)
                })
            });
        },
        extendHierarchyTimeline: function (
            recordId,
            startdate,
            newEndDate,
            oldEndDate,
            extendtype,
            durationDays,
            starttime,
            perdayhrs,
            changeSource
        ) {
            return $http({
                method: "POST",
                url: "/pms/assignprojects/default.aspx/ExtendProjectTimeline_new",
                headers: { "Content-Type": "application/json" },
                data: JSON.stringify({
                    id: String(recordId),
                    extendStrtdate: toHierarchyDateOnly(startdate),
                    extendEnddate: toHierarchyDateOnly(newEndDate),
                    oldEnddate: toHierarchyDateOnly(oldEndDate),
                    extendtype: String(extendtype || '').toLowerCase(),
                    durationDays: durationDays === undefined || durationDays === null
                        ? ''
                        : String(durationDays),
                    starttime: normalizeStartTime(starttime),
                    perdayhrs: String(normalizePerDayHours(
                        perdayhrs === undefined ? configuredPerDayHours : perdayhrs
                    )),
                    changeSource: String(changeSource || 'enddate').toLowerCase()
                })
            });
        },
        extendProjectTimeline: function (
            projectId,
            projectStartDate,
            newTargetDate,
            oldTargetDate,
        ) {
            return $http({
                method: "POST",
                url:
                    "/pms/assignprojects/default.aspx/" +
                    "ExtendProjectTimeline",
                headers: {
                    "Content-Type": "application/json"
                },
                data: JSON.stringify({
                    projctid: String(projectId),
                    extendStrtdate: projectStartDate,
                    extendEnddate: newTargetDate,
                    oldEnddate: oldTargetDate
                })
            });
        },
Unassigntaskfromuser: function (assignid, utype) {
            return $http({
                method: "POST",
                url: "/pms/assignprojects/default.aspx/removeOwner_assign",
                headers: { "Content-Type": "application/json" },
                data: {
                    id: assignid,
                    utype: utype
                }
            });
}



    };
});

app.factory("assignprojectState", function () {
    return {
        showlist: true,
        showAddForm: false
    };
});

 
