app.factory("mprojectService", function ($http) {
    return {
        
        getprojectlist: function () {
            return $http({
                method: "POST",
                url: "/pms/AddprojectAng.aspx/bindProjectdata",
                headers: { "Content-Type": "application/json" },
                data: {}
            });
        },
        getstatuslist: function () {
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
                data: {}
            });
        },
        gettaglist: function () {
            //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/AddprojectAng.aspx/bindtaglist",
                headers: { "Content-Type": "application/json" },
                data: {}
            });
        },
        getprojectfiles: function (pid) {
            //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/AddprojectAng.aspx/bindprojectfileslist",
                headers: { "Content-Type": "application/json" },
                data: { id: pid }
            });
        },
        getsummarydetails: function (id, utype) {
            //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/AddprojectAng.aspx/projectsummarydetails",
                headers: { "Content-Type": "application/json" },
                data: {
                    id: id,
                    stype: utype
                }
            });
        },
        
        uploadfiles: function (files, pid, id, type) {
            let formData = new FormData();
            for (let i = 0; i < files.length; i++) {
                formData.append("files", files[i]);
            }
            formData.append("pid", pid);
            formData.append("ftype", type);
            formData.append("id", id);

            return $http.post("/pms/SingleFileUploadHandlerANG.ashx", formData, {
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            });
        },
        removedoc: function (fileId) {
            debugger
            return $http({
                method: 'POST',
                url: '/pms/DeleteimagehandlerANG.ashx',
                data: {id:fileId },
                headers: { 'Content-Type': 'application/json' }
            })
        },
        UpdateP_Status: function (upd_id, upd_statusid, upd_iscomplete) {
            //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/AddprojectAng.aspx/UpdateProject_Status",
                headers: { "Content-Type": "application/json" },
                data: {
                    projectid: upd_id,
                    statusid: upd_statusid,
                    iscomplete: upd_iscomplete
                }
            });
        },
        UpdateP_Priority: function (upd_id, upd_priorityid) {
            //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/AddprojectAng.aspx/UpdatePriority_Status",
                headers: { "Content-Type": "application/json" },
                data: {
                    projectid: upd_id,
                    priorityid: upd_priorityid
                }
            });
        }, UpdateP_Owner: function (id, ownerid) {
            //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/AddprojectAng.aspx/updateOwner",
                headers: { "Content-Type": "application/json" },
                data: {
                    id: id,
                    ownerid: ownerid
                }
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
        UpdateP_Team: function (upd_id, upd_ownerid) {
            //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/AddprojectAng.aspx/UpdateP_Team",
                headers: { "Content-Type": "application/json" },
                data: {
                    id: upd_id,
                    ownerids: upd_ownerid
                }
            });
        },
        RemoveP_Team: function (projectteams) {
            
            return $http({
                method: "POST",
                url: '/pms/AddprojectAng.aspx/removeteambyid',
                headers: { "Content-Type": "application/json" },
                data: { projectteams: projectteams }
            });
        },
        removeproject: function (pid) {
            debugger
            return $http({
                method: "POST",
                url: "/pms/AddprojectAng.aspx/deleteproject",
                headers: { "Content-Type": "application/json" },
                data: { id: pid }
            });
        },
        getitemdetails: function (id) {
            //alert(projectid);
            var geturl = "/pms/AddprojectAng.aspx/bindProjectdata_byid";
            
            return $http({
                method: "POST",
                url: geturl,
                headers: { "Content-Type": "application/json" },
                data: {
                    id: id
                }
            });
        },
        SvaeExtended_details: function (obj) {
            //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/AddprojectAng.aspx/UpdateExtendeddatedtls",
                headers: { "Content-Type": "application/json" },
                data: {
                    projctid: obj.projctid,
                    extendStrtdate: obj.extendStrtdate,
                    extendEnddate: obj.extendEnddate
                }
            });
        },
        getissuesdetails: function (id,utype) {
            //alert(projectid);
            var geturl = "/pms/AddprojectAng.aspx/bind_projectdetails";

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
    };
});

app.factory("mprojectState", function () {
    return {
        showlist: true,
        showAddForm: false
    };
});

 