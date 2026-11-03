app.factory("userdashboardService", function ($http) {
    return {
         gettaskhistory: function (id) {
           //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/dashboarduser.aspx/bindtaskhistory",
                headers: { "Content-Type": "application/json" },
                data: JSON.stringify({subtaskid:id})
            });
        },
        getsubtaskslist_count: function (fdate, tdate) {
           //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/dashboarduser.aspx/bindtaskdata_count",
                headers: { "Content-Type": "application/json" },
                data: JSON.stringify({ fromdate: fdate, todate: tdate})
            });
        },
        getsubtaskslist: function (fdate, tdate) {
           //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/dashboarduser.aspx/bindsubtasklist",
                headers: { "Content-Type": "application/json" },
                data: JSON.stringify({fromdate:fdate,todate:tdate})
            });
        },
        getsubtaskslistold: function () {
            //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/dashboarduser.aspx/bindsubtasklist",
                headers: { "Content-Type": "application/json" },
                data: JSON.stringify({})
            });
        },
getstatuslist: function () {
           //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/dashboarduser.aspx/bindstatuslist",
                headers: { "Content-Type": "application/json" },
                data: JSON.stringify({})
            });
        },
getprioritylist: function () {
           //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/dashboarduser.aspx/bindprioritylist",
                headers: { "Content-Type": "application/json" },
                data: JSON.stringify({})
            });
        },
getuserlist: function () {
           //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/dashboarduser.aspx/binduserlist",
                headers: { "Content-Type": "application/json" },
                data:{}
            });
        },
getuserlist_byrefid: function (refid, usertype, searchtype) {
    //alert(projectid);
    return $http({
        method: "POST",
        url: "/pms/dashboarduser.aspx/binduserlist_byrefid",
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
                url: "/pms/projects/default.aspx/UpdateSubtask_Status",
                headers: { "Content-Type": "application/json" },
                data:{
    subtaskid:upd_id,
    statusid:upd_statusid,
    iscomplete:upd_iscomplete}
            });
        },
SaveTimerpausedetails: function (upd_pid,upd_mid,upd_tid,upd_stid,upd_notes) {
           //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/dashboarduser.aspx/SaveTimerpausedetails",
                headers: { "Content-Type": "application/json" },
                data:{
    ProjectId:upd_pid,
    Milestoneid:upd_mid,
    taskid:upd_tid,
    subtaskid:upd_stid,
    Notes:upd_notes,
}
            });
        },
SaveTimerresumedetails: function (upd_pid,upd_mid,upd_tid,upd_stid) {
           //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/dashboarduser.aspx/SaveTimerresumedetails",
                headers: { "Content-Type": "application/json" },
                data:{
    ProjectId:upd_pid,
    Milestoneid:upd_mid,
    taskid:upd_tid,
    subtaskid:upd_stid,
    
}
            });
        },
SaveTimerstartdetails: function (upd_pid,upd_mid,upd_tid,upd_stid) {
           //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/dashboarduser.aspx/SaveTimerstartdetails",
                headers: { "Content-Type": "application/json" },
                data:{
    ProjectId:upd_pid,
    Milestoneid:upd_mid,
    taskid:upd_tid,
    subtaskid:upd_stid,
    
}
            });
        },
SaveTimer: function (upd_pid,upd_mid,upd_tid,upd_stid,updt_note,upd_timedetails) {
           //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/dashboarduser.aspx/SaveTimer",
                headers: { "Content-Type": "application/json" },
                data:{
    ProjectId:upd_pid,
    Milestoneid:upd_mid,
    taskid:upd_tid,
    subtaskid:upd_stid,
    timernotes:updt_note,
     timerdetails:upd_timedetails
    
}
            });
        },
        Getspenttime_subtask: function (tid) {
            //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/dashboarduser.aspx/Getspenttime_byid",
                headers: { "Content-Type": "application/json" },
                data: {
                    id: tid
                }
            });
        },
        SaveTags: function (c,tagname,tagcolor,tagtype) {
            //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/dashboarduser.aspx/SaveTags",
                headers: { "Content-Type": "application/json" },
                data: "{ 'projectid': '" + c.ProjectId + "','milestoneid': '" + c.Milestoneid + "','taskid': '" + c.Task + "','subtaskid': '" + c.id + "' ,'Tags': '" + tagname + "','TagsColor': '" + tagcolor + "','Tagstype':'" + tagtype + "' }",
            });
        },
        removetags: function (tagid) {
            //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/dashboarduser.aspx/removetagbyid",
                headers: { "Content-Type": "application/json" },
                data: {
                    id: tagid
                }
            });
        }
    };
});

app.factory("userdashboardState", function () {
    return {
        showlist: true,
        showAddForm: false
    };
});

 