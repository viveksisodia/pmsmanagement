app.factory("projectService", function ($http) {
    return {
        getprojectmasterlist: function (cpid) {
            return $http({
                method: "POST",
                url: "/pms/projects/default.aspx/bindProjectdata",
                headers: { "Content-Type": "application/json" },
                data: { currentid: cpid}
            });
        },
        getprevious_projectdetails: function (cpid) {
            return $http({
                method: "POST",
                url: "/pms/projects/default.aspx/bindProjectdetail_pre",
                headers: { "Content-Type": "application/json" },
                data: { RecordId: cpid }
            });
        },
        copyProject: function (cpid,projectid) {
            return $http({
                method: "POST",
                url: "/pms/projects/default.aspx/copyproject",
                headers: { "Content-Type": "application/json" },
                data: { currentid: cpid, copyprojectid: projectid}
            });
        },
    getprojectlistbyid: function (projectid) {
           //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/projects/default.aspx/bindtaskdata",
                headers: { "Content-Type": "application/json" },
                data: JSON.stringify({
                    RecordId: projectid 
                   
                })
            });
        },
gettasklistbyid: function (milestonid) {
           //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/projects/default.aspx/bindmile_tasklist",
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
                url: "/pms/projects/default.aspx/bindcompletiondetails_mile",
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
                url: "/pms/projects/default.aspx/bindcompletiondetails_task",
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
                url: "/pms/projects/default.aspx/bindcompletiondetails_subtask",
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
                url: "/pms/projects/default.aspx/bindstatuslist",
                headers: { "Content-Type": "application/json" },
                data: JSON.stringify({})
            });
        },
getprioritylist: function () {
           //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/projects/default.aspx/bindprioritylist",
                headers: { "Content-Type": "application/json" },
                data: JSON.stringify({})
            });
        },
getuserlist: function () {
           //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/projects/default.aspx/binduserlist",
                headers: { "Content-Type": "application/json" },
                data:{}
            });
        },
getuserlist_byrefid: function (refid, usertype, searchtype) {
    //alert(projectid);
    return $http({
        method: "POST",
        url: "/pms/projects/default.aspx/binduserlist_byrefid",
        headers: { "Content-Type": "application/json" },
        data: {
            id: refid,
            utype: usertype,   // <-- comma added here
            stype: searchtype
        }
    });
},

UpdateMilestone_Status: function (upd_id,upd_statusid,upd_iscomplete) {
           //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/projects/default.aspx/UpdateMilestone_Status",
                headers: { "Content-Type": "application/json" },
                data:{
    milstoneid:upd_id,
    statusid:upd_statusid,
    iscomplete:upd_iscomplete}
            });
        },
MileUpdatePriority_Status: function (upd_id,upd_priorityid) {
           //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/projects/default.aspx/MileUpdatePriority_Status",
                headers: { "Content-Type": "application/json" },
                data:{
    milstoneid:upd_id,
    priorityid:upd_priorityid}
            });
        },
updatedetails_mile_bycol: function (upd_id,col_name,col_value) {
           

            return $http({
                method: "POST",
                url: "/pms/projects/default.aspx/MileUpdateDetails",
                headers: { "Content-Type": "application/json" },
                data:{
    id:upd_id,
    col_name:col_name,
    col_value:col_value
}
            });
        },
addnewmilestonerow: function(object, orderno) {
debugger
    return $http({
        method: "POST",
        url: "/pms/projects/default.aspx/addnewmilestonerrequest",
        headers: { "Content-Type": "application/json" },
        data: angular.toJson({
            request: {              
                milestone: object,
                orderno: orderno
            }
        })
    });
},
removemilestone: function(mid) {
debugger
    return $http({
        method: "POST",
        url: "/pms/projects/default.aspx/deletemilestone",
        headers: { "Content-Type": "application/json" },
        data:{id:mid}
    });
},
updateorderno_milestone:function(mid,morderno) {
debugger
    return $http({
        method: "POST",
        url: "/pms/projects/default.aspx/updatemilestone_order",
        headers: { "Content-Type": "application/json" },
        data:{id:mid,orderno:morderno}
    });
},
updatetimeline_mile: function (mid, adddays, fdate) {
    return $http({
        method: "POST",
        url: "/pms/projects/default.aspx/updatetimeline_mile",
        headers: { "Content-Type": "application/json" },
        data: JSON.stringify({
            id: mid,
            ndays: adddays,
            startdate: fdate
        })
    });
},
addnewtaskrow: function(object, orderno) {
debugger
    return $http({
        method: "POST",
        url: "/pms/projects/default.aspx/addnewtaskrequest",
        headers: { "Content-Type": "application/json" },
        data: angular.toJson({
            request: {              
                task: object,
                orderno: orderno
            }
        })
    });
},
removetask: function(tid) {
debugger
    return $http({
        method: "POST",
        url: "/pms/projects/default.aspx/deletetask",
        headers: { "Content-Type": "application/json" },
        data:{id:tid}
    });
},
updateorderno_task:function(tid,morderno) {
debugger
    return $http({
        method: "POST",
        url: "/pms/projects/default.aspx/updatetask_order",
        headers: { "Content-Type": "application/json" },
        data:{id:tid,orderno:morderno}
    });
},
UpdateTask_Status: function (upd_id,upd_statusid,upd_iscomplete) {
           //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/projects/default.aspx/UpdateTask_Status",
                headers: { "Content-Type": "application/json" },
                data:{
    taskid:upd_id,
    statusid:upd_statusid,
    iscomplete:upd_iscomplete}
            });
        },
TaskUpdatePriority_Status: function (upd_id,upd_priorityid) {
           //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/projects/default.aspx/TaskUpdatePriority_Status",
                headers: { "Content-Type": "application/json" },
                data:{
    taskid:upd_id,
    priorityid:upd_priorityid}
            });
        },
updatedetails_task_bycol: function (upd_id,col_name,col_value) {
           

            return $http({
                method: "POST",
                url: "/pms/projects/default.aspx/TaskUpdateDetails",
                headers: { "Content-Type": "application/json" },
                data:{
    id:upd_id,
    col_name:col_name,
    col_value:col_value
}
            });
        },

updatetimeline_task: function (tid, adddays, fdate) {
    return $http({
        method: "POST",
        url: "/pms/projects/default.aspx/updatetimeline_task",
        headers: { "Content-Type": "application/json" },
        data: JSON.stringify({
            id: tid,
            ndays: adddays,
            startdate: fdate
        })
    });
},
addnewsubtaskrow: function(object, orderno) {
debugger
    return $http({
        method: "POST",
        url: "/pms/projects/default.aspx/addnewsubtaskrequest",
        headers: { "Content-Type": "application/json" },
        data: angular.toJson({
            request: {              
                subtask: object,
                orderno: orderno
            }
        })
    });
},
removesubtask: function(sid) {
debugger
    return $http({
        method: "POST",
        url: "/pms/projects/default.aspx/deletesubtask",
        headers: { "Content-Type": "application/json" },
        data:{id:sid}
    });
},
updateorderno_subtask:function(sid,morderno) {
debugger
    return $http({
        method: "POST",
        url: "/pms/projects/default.aspx/updatesubtask_order",
        headers: { "Content-Type": "application/json" },
        data:{id:sid,orderno:morderno}
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
SubtaskUpdatePriority_Status: function (upd_id,upd_priorityid) {
           //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/projects/default.aspx/SubtaskUpdatePriority_Status",
                headers: { "Content-Type": "application/json" },
                data:{
    subtaskid:upd_id,
    priorityid:upd_priorityid}
            });
        },
updatedetails_subtask_bycol: function (upd_id,col_name,col_value) {
           

            return $http({
                method: "POST",
                url: "/pms/projects/default.aspx/SubTaskUpdateDetails",
                headers: { "Content-Type": "application/json" },
                data:{
    id:upd_id,
    col_name:col_name,
    col_value:col_value
}
            });
        },

updatetimeline_subtask: function (sid, adddays, fdate) {
    return $http({
        method: "POST",
        url: "/pms/projects/default.aspx/updatetimeline_subtask",
        headers: { "Content-Type": "application/json" },
        data: JSON.stringify({
            id: sid,
            ndays: adddays,
            startdate: fdate
        })
    });
},
getleftdays: function (id,utype) {
debugger
    return $http({
        method: "POST",
        url: "/pms/projects/default.aspx/getleftdays",
        headers: { "Content-Type": "application/json" },
        data: JSON.stringify({
            id: id,
            gtype: utype
            
        })
    });
},
getweightage: function (id,utype) {
debugger
    return $http({
        method: "POST",
        url: "/pms/projects/default.aspx/get_newweightage",
        headers: { "Content-Type": "application/json" },
        data: JSON.stringify({
            id: id,
            gtype: utype
            
        })
    });
},
getweightage_default: function (id,utype) {
debugger
    return $http({
        method: "POST",
        url: "/pms/projects/default.aspx/get_newweightage2",
        headers: { "Content-Type": "application/json" },
        data: JSON.stringify({
            id: id,
            gtype: utype
            
        })
    });
},
getnew_enddate: function (fdate,adddays) {
debugger
    return $http({
        method: "POST",
        url: "/pms/projects/default.aspx/manage_enddate",
        headers: { "Content-Type": "application/json" },
        data: JSON.stringify({
       ndays: adddays,
            startdate: fdate
            
            
        })
    });
},

UpdateMilestone_Owner: function (upd_id,upd_ownerid) {
           //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/projects/default.aspx/UpdateMilestone_Owner",
                headers: { "Content-Type": "application/json" },
                data:{
    id:upd_id,
    ownerids:upd_ownerid}
            });
        },
UpdateTask_Owner: function (upd_id,upd_ownerid) {
           //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/projects/default.aspx/UpdateTask_Owner",
                headers: { "Content-Type": "application/json" },
                data:{
    id:upd_id,
    ownerids:upd_ownerid}
            });
        },
UpdateSubtask_Owner: function (upd_id,upd_ownerid) {
           //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/projects/default.aspx/UpdateSubtask_Owner",
                headers: { "Content-Type": "application/json" },
                data:{
    id:upd_id,
    ownerids:upd_ownerid}
            });
        },
UpdateMilestone_Team: function (upd_id,upd_ownerid) {
           //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/projects/default.aspx/UpdateMilestone_Team",
                headers: { "Content-Type": "application/json" },
                data:{
    id:upd_id,
    ownerids:upd_ownerid}
            });
        },
UpdateTask_Team: function (upd_id,upd_ownerid) {
           //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/projects/default.aspx/UpdateTask_Team",
                headers: { "Content-Type": "application/json" },
                data:{
    id:upd_id,
    ownerids:upd_ownerid}
            });
        },
UpdateSubtask_Team: function (upd_id,upd_ownerid) {
           //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/projects/default.aspx/UpdateSubtask_Team",
                headers: { "Content-Type": "application/json" },
                data:{
    id:upd_id,
    ownerids:upd_ownerid}
            });
        },
getissuesdetails: function (id,utype) {
           //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/projects/default.aspx/bindissues_details",
                headers: { "Content-Type": "application/json" },
                data:{
                id:id,
                issuetype:utype}
            });
        },
getdependencydetails: function (id,utype) {
           //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/projects/default.aspx/binddependency_details",
                headers: { "Content-Type": "application/json" },
                data:{
                id:id,
                type:utype}
            });
        },
getitemdetails: function (id,utype) {
           //alert(projectid);
        var geturl="/pms/projects/default.aspx/binditemdetails_mile";
        if(utype=="task")
        {
        geturl="/pms/projects/default.aspx/binditemdetails_task";
        }
        else if(utype=="subtask")
        {
        geturl="/pms/projects/default.aspx/binditemdetails_subtask";
        }
            return $http({
                method: "POST",
                url:geturl,
                headers: { "Content-Type": "application/json" },
                data:{
                id:id,
                type:utype}
            });
        },

    };
});

app.factory("projectState", function () {
    return {
        showlist: true,
        showAddForm: false
    };
});

 