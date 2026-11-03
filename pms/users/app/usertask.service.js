app.factory("usertaskService", function ($http) {
    return {
         gettaskhistory: function (id,htype) {
           //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/users/default.aspx/bindtaskhistory",
                headers: { "Content-Type": "application/json" },
                data: JSON.stringify({subtaskid:id,tasktype:htype})
            });
        },
        getsubtaskslist_count: function (fdate, tdate) {
           //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/users/default.aspx/bindtaskdata_count",
                headers: { "Content-Type": "application/json" },
                data: JSON.stringify({ fromdate: fdate, todate: tdate})
            });
        },
        getsubtaskslist: function (fdate, tdate, listtype) {
           //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/users/default.aspx/bindsubtasklist",
                headers: { "Content-Type": "application/json" },
                data: JSON.stringify({fromdate:fdate,todate:tdate,listtype:listtype})
            });
        },
        getWeeklyReportData: function (weekStart, weekEnd) {
            return $http({
                method: "POST",
                url: "/pms/users/default.aspx/GetWeeklyReportData",
                headers: { "Content-Type": "application/json" },
                data: JSON.stringify({ weekStart: weekStart, weekEnd: weekEnd })
            });
        },
        getsubtaskslistold: function () {
            //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/users/default.aspx/bindsubtasklist",
                headers: { "Content-Type": "application/json" },
                data: JSON.stringify({})
            });
        },
gettaglist: function () {
           //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/users/default.aspx/bindtaglist",
                headers: { "Content-Type": "application/json" },
                data: JSON.stringify({})
            });
        },
UpdateP_Tag: function (upd_id,utype, upd_tagid) {
            //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/users/default.aspx/UpdateP_tags",
                headers: { "Content-Type": "application/json" },
                data: {
                    id: upd_id,
                tasktype:utype,
                    ids: upd_tagid
                }
            });
        },

        RemoveP_Tag: function (removetag) {
            
            return $http({
                method: "POST",
                url: '/pms/users/default.aspx/removetagbyid',
                headers: { "Content-Type": "application/json" },
                data: { removetags: removetag }
            });
        },
getstatuslist: function () {
           //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/users/default.aspx/bindstatuslist",
                headers: { "Content-Type": "application/json" },
                data: JSON.stringify({})
            });
        },
getprioritylist: function () {
           //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/users/default.aspx/bindprioritylist",
                headers: { "Content-Type": "application/json" },
                data: JSON.stringify({})
            });
        },
getuserlist: function () {
           //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/users/default.aspx/binduserlist",
                headers: { "Content-Type": "application/json" },
                data:{}
            });
        },
getuserlist_byrefid: function (refid, usertype, searchtype) {
    //alert(projectid);
    return $http({
        method: "POST",
        url: "/pms/users/default.aspx/binduserlist_byrefid",
        headers: { "Content-Type": "application/json" },
        data: {
            id: refid,
            utype: usertype,   // <-- comma added here
            stype: searchtype
        }
    });
},
UpdateSubTask_Status: function (upd_id,upd_statusid,upd_iscomplete) {
           //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/users/default.aspx/UpdateSubtask_Status",
                headers: { "Content-Type": "application/json" },
                data:{
    subtaskid:upd_id,
    statusid:upd_statusid,
    iscomplete:upd_iscomplete}
            });
        },
UpdateTask_Status: function (upd_id,upd_statusid,upd_iscomplete) {
           //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/users/default.aspx/UpdateTask_Status",
                headers: { "Content-Type": "application/json" },
                data:{
     taskid:upd_id,
    statusid:upd_statusid,
    iscomplete:upd_iscomplete}
            });
        },
UpdateMilestone_Status: function (upd_id,upd_statusid,upd_iscomplete) {
           //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/users/default.aspx/UpdateMilestone_Status",
                headers: { "Content-Type": "application/json" },
                data:{
     milstoneid:upd_id,
    statusid:upd_statusid,
    iscomplete:upd_iscomplete}
            });
        },
UpdateSubTask_StatusMulti: function (upd_ids,upd_statusid,upd_iscomplete) {
           //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/users/default.aspx/UpdateSubtask_StatusMulti",
                headers: { "Content-Type": "application/json" },
                data:{
    taskids:upd_ids,
    statusid:upd_statusid,
    iscomplete:upd_iscomplete}
            });
        },
UpdateKanbanPriority: function (upd_id, upd_type, upd_priorityid) {
            return $http({
                method: "POST",
                url: "/pms/users/default.aspx/UpdateKanbanPriority",
                headers: { "Content-Type": "application/json" },
                data: {
                    id: upd_id,
                    tasktype: upd_type,
                    priorityid: upd_priorityid
                }
            });
        },
SaveTimerpausedetails: function (upd_type,upd_pid,upd_mid,upd_tid,upd_stid,upd_notes) {
           //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/users/default.aspx/SaveTimerpausedetails",
                headers: { "Content-Type": "application/json" },
                data:{
    tasktype:upd_type,
    ProjectId:upd_pid,
    Milestoneid:upd_mid,
    taskid:upd_tid,
    id:upd_stid,
    Notes:upd_notes,
}
            });
        },
SaveTimerresumedetails: function (upd_type,upd_pid,upd_mid,upd_tid,upd_stid) {
           //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/users/default.aspx/SaveTimerresumedetails",
                headers: { "Content-Type": "application/json" },
                data:{
    tasktype:upd_type,
    ProjectId:upd_pid,
    Milestoneid:upd_mid,
    taskid:upd_tid,
    id:upd_stid,
    
}
            });
        },
GetRunningTimerBeforeStart: function (selectedType, selectedId) {
            return $http({
                method: "POST",
                url: "/pms/users/default.aspx/GetRunningTimerBeforeStart",
                headers: { "Content-Type": "application/json" },
                data: {
                    selectedType: selectedType || "",
                    selectedId: selectedId == null ? "" : String(selectedId)
                }
            });
        },
SaveTimerstartdetails: function (upd_type,upd_pid,upd_mid,upd_tid,upd_stid) {
           //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/users/default.aspx/SaveTimerstartdetails",
                headers: { "Content-Type": "application/json" },
                data:{
    tasktype:upd_type,
    ProjectId:upd_pid,
    Milestoneid:upd_mid,
    taskid:upd_tid,
    id:upd_stid,
    
}
            });
        },
SaveTimer: function (upd_type,upd_pid,upd_mid,upd_tid,upd_stid,updt_note,upd_timedetails) {
           //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/users/default.aspx/SaveTimer",
                headers: { "Content-Type": "application/json" },
                data:{
tasktype:upd_type,
    ProjectId:upd_pid,
    Milestoneid:upd_mid,
    taskid:upd_tid,
    subtaskid:upd_stid,
    timernotes:updt_note,
     timerdetails:upd_timedetails
    
}
            });
        },
        Getspenttime_subtask: function (tid,utype) {
            //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/users/default.aspx/Getspenttime_byid",
                headers: { "Content-Type": "application/json" },
                data: {
                    id: tid,
tasktype:utype
                }
            });
        },
        SaveTags: function (c,tagname,tagcolor,tagtype) {
            //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/users/default.aspx/SaveTags",
                headers: { "Content-Type": "application/json" },
                data: "{ 'projectid': '" + c.ProjectId + "','milestoneid': '" + c.Milestoneid + "','taskid': '" + c.Task + "','subtaskid': '" + c.id + "' ,'Tags': '" + tagname + "','TagsColor': '" + tagcolor + "','Tagstype':'" + tagtype + "' }",
            });
        },
SaveTagsNew: function (tagname,tagcolor) {
            //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/users/default.aspx/SaveTags_new",
                headers: { "Content-Type": "application/json" },
                data: "{ 'Tags': '" + tagname + "','TagsColor': '" + tagcolor + "'}",
            });
        },
     SaveTags_update_Multi: function (taskids,tagids) {
            //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/users/default.aspx/SaveTags_update_Multi",
                headers: { "Content-Type": "application/json" },
                data: "{ 'taskids': '" +taskids+ "','tagids': '" +tagids+ "'}",
            });
        },
        removetags: function (id,tasktype,updatetagids) {
            //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/users/default.aspx/removetagbyid",
                headers: { "Content-Type": "application/json" },
                data: {
                    id: id,
                    removetype:tasktype,
                    tagids:updatetagids
                }
            });
        }
    };
});

app.factory("usertaskState", function () {
    return {
        showlist: true,
        showAddForm: false
    };
});

 
