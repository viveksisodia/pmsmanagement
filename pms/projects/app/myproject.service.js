app.factory("myprojectService", function ($http) {
    return {
        managefilter: function (filtertype) {
            //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/projects/default.aspx/savefilter",
                headers: { "Content-Type": "application/json" },
                data: { filtername: filtertype }
            });
        },
        getfilter: function () {
            return $http({
                method: "POST",
                url: "/pms/projects/default.aspx/getfilter",
                headers: { "Content-Type": "application/json" },
                data: {}
            });
        },
        getprojectlist: function (fdate, tdate) {
            return $http({
                method: "POST",
                url: "/pms/projects/default.aspx/bindProjectdata",
                headers: { "Content-Type": "application/json" },
                data: JSON.stringify({fromdate:fdate,todate:tdate})
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
        getProjectSituationList: function () {
            return $http({
                method: "POST",
                url: "/pms/projects/default.aspx/bindprojectsituationlist",
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
           // alert("ok");
            return $http({
                method: "POST",
                url: "/pms/projects/default.aspx/binduserlist",
                headers: { "Content-Type": "application/json" },
                data: {}
            });
        },
        getteamlist: function () {
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
                url: "/pms/projects/default.aspx/bindtaglist",
                headers: { "Content-Type": "application/json" },
                data: {}
            });
        },
getcustomerlist: function () {
            //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/projects/default.aspx/bindcustomerlist",
                headers: { "Content-Type": "application/json" },
                data: {}
            });
        },
getsolist: function (clientid,utype) {
            //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/projects/default.aspx/bindsolist",
                headers: { "Content-Type": "application/json" },
                data: {clientid:clientid,bindtype:utype}
            });
        },
        getcountrylist: function () {
            //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/projects/default.aspx/bindcountrylist",
                headers: { "Content-Type": "application/json" },
                data: {}
            });
        },
        getstatelist: function (countryid) {
            //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/projects/default.aspx/bindstatelist",
                headers: { "Content-Type": "application/json" },
                data: { Countryid: countryid}
            });
        },
        getprojectfiles: function (pid) {
            //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/projects/default.aspx/bindprojectfileslist",
                headers: { "Content-Type": "application/json" },
                data: { id: pid }
            });
        },
getprojectdetailbyid: function (pid) {
            //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/projects/default.aspx/getorojectdetails",
                headers: { "Content-Type": "application/json" },
                data: { id: pid }
            });
        },
        getprojectextendhistory: function (id) {
            //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/projects/default.aspx/get_extendedhistory",
                headers: { "Content-Type": "application/json" },
                data: {pid: id }
            });
        },
        getsummarydetails: function (id, utype) {
            //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/projects/default.aspx/projectsummarydetails",
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
  uploadfiles2: function (pid,files) {
            let formData = new FormData();
            formData.append("id", pid);
            for (let i = 0; i < files.length; i++) {
                if(files[i].id=="")
                formData.append("files", files[i].file);            }            
            return $http.post("/pms/FileUploadHandlerProjectAng.ashx", formData, {
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            });
        },
        removedoc: function (fileId) {
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
                url: "/pms/projects/default.aspx/UpdateProject_Status",
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
                url: "/pms/projects/default.aspx/UpdatePriority_Status",
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
                url: "/pms/projects/default.aspx/updateOwner",
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
        UpdateP_Team: function (upd_id, upd_ownerid, employeeid, actiontype) {
            //alert(projectid);
            return $http({
                method: "POST",
                url: "/pms/projects/default.aspx/UpdateP_Team",
                headers: { "Content-Type": "application/json" },
                data: {
                    id: upd_id,
                    ownerids: upd_ownerid,
                    employeeid: employeeid,
                    actiontype: actiontype
                }
            });
        },
        RemoveP_Team: function (projectteams) {
            
            return $http({
                method: "POST",
                url: '/pms/projects/default.aspx/removeteambyid',
                headers: { "Content-Type": "application/json" },
                data: { projectteams: projectteams }
            });
        },
 RemoveP_CumulativeTeam: function (projectteams) {
            
            return $http({
                method: "POST",
                url: '/pms/projects/default.aspx/removecumulativeteam',
                headers: { "Content-Type": "application/json" },
                data: { projectteams: projectteams }
            });
        },
        removeproject: function (pid) {
            return $http({
                method: "POST",
                url: "/pms/projects/default.aspx/deleteproject",
                headers: { "Content-Type": "application/json" },
                data: { id: pid }
            });
        },
checkduplicate: function (pid, id) {
            return $http({
                method: "POST",
                url: "/pms/projects/default.aspx/checkduplicate",
                headers: { "Content-Type": "application/json" },
                data: {
                    projectid: pid,
                    id: id || "0"
                }
            });
        },
         saveproject: function (project,type) {
            return $http({
                method: "POST",
                url: "/pms/projects/default.aspx/saveproject",
                headers: { "Content-Type": "application/json" },
                data: {projectdetails: project,ptype:type }
            });
        },
        getitemdetails: function (id) {
            //alert(projectid);
            var geturl = "/pms/projects/default.aspx/bindProjectdata_byid";
            
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
                url: "/pms/projects/default.aspx/UpdateExtendeddatedtls",
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
            var geturl = "/pms/projects/default.aspx/bind_projectdetails";

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

app.factory("myprojectState", function () {
    return {
        showlist: true,
        showAddForm: false
    };
});

 
