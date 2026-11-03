app.controller("myprojectCtrl", function ($scope, myprojectService) {

    //alert("Controller Loaded");
    debugger

 $scope.showTagDropdown = false;
$scope.showqcustomersDropdown= false;
$scope.openTagDropdown = function ($event) {
debugger
    $event.stopPropagation();
    $scope.showTagDropdown = true;
};
   $scope.modalTitle = "Add teams";
   $scope.freesearch = "0";
   $scope.viewsearch = function ()
    {
        debugger
        if ($scope.freesearch=="0")
            $scope.freesearch = "1";
        else
            $scope.freesearch = "0";
    };
    $scope.currentPage = 1;
    $scope.pageSizes = [10, 20, 50, 100];
    $scope.pageSize = 10;
    $scope.projectlist = [];
    $scope.mytotalprojectlist =0;
    $scope.myprojectlist_bystatus =[];

  $scope.filterstatuslist_count = function () {
debugger
    if ($scope.statuslists && $scope.statuslists.length > 0) {

        $scope.myprojectlist_bystatus = [];

        angular.forEach($scope.statuslists, function (status) {

            var total = $scope.projectlist.filter(function (p) {
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
debugger
        console.log($scope.myprojectlist_bystatus);
    }
};
//EstimateDate: new Date(),
$scope.selectedPriority = "";
    $scope.projectTitle="Add Project"
$scope.newproject = {
        id: 0,
        ProjectID:"",
        ProjectName: "",
        Owner: "",
        team:null,        
        ProjectStatus:null,
        ProjectAddress1: "",
        ProjectAddress2: "",
        City: "",
        PostalCode: "",
        StateName: "",
        Countryid:null,
        ClientName:"",
        Priority:null,
        Budget:null,
        Actual:null,
        AwardDate:new Date(),
        StartDate:new Date(),
        TargetDate:new Date(),
        ApprovalDrawing:null,
        DrawingApprovalDate:new Date(),
        Uploaddrawing:"",
        Weightage:null,
        isdrawing:null,
        iscomplete:null,
        iscompletedate:new Date(),
        iscompletepercentage:null,
        checkprojectWeightage:null,
        salesorderid:null,
        customerid:null,
        hastask:null,
        checktaskWeightage:null,
        hassubtask:null,
        checksubtaskWeightage:null,
tagids:""
    };
    $scope.isshowall = "0";
    $scope.chkall = false;
    $scope.showfileslist = [];
    $scope.currentprojectid = "0";
    
    $scope.customerlist = [];
    $scope.solist = [];
    $scope.tagslist = [];
    $scope.owners = [];
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
$scope.selectclient = function (clientid) {
        debugger
                
       
        $scope.ClientName = clientid.ClientName;
        $scope.showqcustomersDropdown = false;        
myprojectService.getsolist(clientid.customerid,'')
           
            .then(function (response) {
debugger
               $scope.solist=response.data.d; 
  

            })
            .catch(function (error) {

                debugger;
                alert("HTTP ERROR: " + (error.statusText || error.message || "Unknown error"));
                console.error("HTTP ERROR:", error);

            })
            .finally(function () {

                //$scope.isLoading = false;

            });
        
    };
    //#region bind owner details
    $scope.getSelectedOwners = function () {
        return $scope.owners.filter(o => $scope.selectedOwnerIds.includes(o.id));
    };
    $scope.selectOwner = function (c, o) {
        debugger
                
                

        myprojectService.UpdateP_Owner(c.id, o.id)
            .then(function (response) {

                debugger;
                c.Owner = o.id;   

            })
            .catch(function (error) {

                debugger;
                alert("HTTP ERROR: " + (error.statusText || error.message || "Unknown error"));
                console.error("HTTP ERROR:", error);

            })
            .finally(function () {

                //$scope.isLoading = false;

            });
        
    };
    
    $scope.focusOwnerSearch = function ($event, c) {

        setTimeout(function () {

            // find current dropdown container
            var dropdown = $event.target.closest(".dropdown");

            if (!dropdown) return;

            // find input inside it
            var input = dropdown.querySelector(".owner-search");

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
$scope.getOwnerProfile = function (ownerid) {
debugger
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
$scope.getOwnerProfileold = function (ownerid) {

    if (!$scope.owners || !$scope.owners.length) {
        return '/pms/new/assets/img/avatar.jpg';
    }
    var owner = $scope.owners.find(function (o) {
        return o.id == ownerid;
    });

    if (owner && owner.profileimage && owner.profileimage !== '') {
        return owner.profileimage;
    }

    return '/pms/new/assets/img/avatar.jpg';
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
    $scope.removeteams = function (obj1, obj2) {
        debugger;

        var projectteams = {
            id: obj1.id,
            Team: obj2.id,
            
        };
        let resultnew;
        resultnew = myprojectService.RemoveP_Team(projectteams);
        if (resultnew) {
            resultnew.then(function (res) {
                debugger;
                if (res.data.d == "1") {
                    let indx = obj1.teams.findIndex(m => m.id == obj2.id);
                    obj1.teams.splice(indx, 1);
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
        var index = $scope.selectedOwnerIds.indexOf(ownerId);
        if (index > -1) {
            $scope.selectedOwnerIds.splice(index, 1);
            
                $scope.removeTeamToServer($scope.currentlist, $scope.selectedOwnerIds, ownerId);
           

        }
        

    };
    
    $scope.removeTeamToServer = function (obj, selected_obj, newid) {
        debugger
        
        var ids = selected_obj.join(",");
        let resultnew;

        
        resultnew = myprojectService.UpdateP_Team(obj.id, ids);
        
        if (resultnew) {
            resultnew.then(function (res) {
                debugger;
                if (res.data.d == "1") {
                    var indx = obj.teams.findIndex(m => m.id == newid);
                    if (indx !== -1) {
                        obj.teams.splice(indx, 1);
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
    $scope.removeTeam_byid = function (obj, selected_obj, newid) {
        debugger

        var ids = selected_obj.join(",");
        let resultnew;


        resultnew = myprojectService.UpdateP_Team(obj.id, ids);

        if (resultnew) {
            resultnew.then(function (res) {
                debugger;
                if (res.data.d == "1") {
                    var indx = obj.teams.findIndex(m => m.id == newid);
                    if (indx !== -1) {
                        obj.teams.splice(indx, 1);
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
    $scope.toggleOwner = function (owner) {
        debugger
        var index = $scope.selectedOwnerIds.indexOf(owner.id);

        if (index > -1) {
           
        } else {

            $scope.selectedOwnerIds.push(owner.id); // select
            $scope.updateTeamToServer($scope.currentlist, $scope.selectedOwnerIds, owner.id);
            
        }
        
    };
    $scope.updateTeamToServer = function (obj, selected_obj, newid) {
        debugger
        if (!selected_obj || selected_obj.length === 0) {
            alert("No owners selected");
            return;
        }
        var ids = selected_obj.join(",");
        let resultnew;

       
        resultnew = myprojectService.UpdateP_Team(obj.id, ids);
        
        if (resultnew) {
            resultnew.then(function (res) {
                debugger;
                if (res.data.d == "1") {
                    var addlist = $scope.owners.find(m => m.id == newid);
                    if (addlist) {
                        let newrow = angular.copy($scope.ownersnew);
                        newrow.id = addlist.id;
                        newrow.name = addlist.name;
                        newrow.profileimage = addlist.profileimage;
                        newrow.shortname = addlist.shortname;
                        obj.Team = ids;
                        obj.teams.push(newrow);
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
    $scope.loadSrcOwnersnew = function (c,index) {
        debugger;

        // Determine reference ID based on type
        let refId;
        let reftype = "";
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
       if(c.teams)
        $scope.selectedOwnerIds = c.teams.map(t => t.id);
        
    };
    $scope.loadtags = function (c,frameId, url) {

        debugger;
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

        debugger;
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

        debugger;
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
            debugger;

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
        debugger;

        refid = "0";
        myprojectService.getuserlist_byrefid(refid, usertype, "team")
            .then(function (response) {
                debugger;
                // assign data
                $scope.owners = response.data.d;
            })
            .catch(function (error) {
                debugger;
                alert("HTTP ERROR: " + (error.statusText || error.message || "Unknown error"));
                console.error("HTTP ERROR:", error);
            })
            .finally(function () {

            });
    };
    $scope.loadsrc_ang = function (frameId,url) {
        debugger;

        let frame = document.getElementById(frameId);

        if (frame) {
            frame.src = "about:blank";   // force reset

            setTimeout(function () {
                frame.src = url + "&r=" + Date.now();
            }, 50);
        }

    };
    //#endregin
    //#load maters
$scope.loadcustomerlist = function () {

        debugger;
        //$scope.isLoading = true;

        myprojectService.getcustomerlist()
            .then(function (response) {
                debugger;
                // assign data
                $scope.customerlist = response.data.d;

            })
            .catch(function (error) {

                debugger;
                alert("HTTP ERROR: " + (error.statusText || error.message || "Unknown error"));
                console.error("HTTP ERROR:", error);

            })
            .finally(function () {

                //$scope.isLoading = false;

            });
    };
$scope.loadsolist = function (customerid) {

        debugger;
        //$scope.isLoading = true;

        myprojectService.getsolist(customerid)
            .then(function (response) {
                debugger;
                // assign data
                $scope.solist = response.data.d;

            })
            .catch(function (error) {

                debugger;
                alert("HTTP ERROR: " + (error.statusText || error.message || "Unknown error"));
                console.error("HTTP ERROR:", error);

            })
            .finally(function () {

                //$scope.isLoading = false;

            });
    };
    $scope.loaduserlist = function () {

        debugger;
        //$scope.isLoading = true;

        myprojectService.getuserlist()
            .then(function (response) {

                debugger;
                // assign data
                $scope.owners = response.data.d;

            })
            .catch(function (error) {

                debugger;
                alert("HTTP ERROR: " + (error.statusText || error.message || "Unknown error"));
                console.error("HTTP ERROR:", error);

            })
            .finally(function () {

                //$scope.isLoading = false;

            });
    };
    $scope.loadstatuslist = function () {

        debugger;
        

        myprojectService.getstatuslist()
            .then(function (response) {

                debugger;
                // assign data
                $scope.statuslists = response.data.d;

            })
            .catch(function (error) {

                debugger;
                alert("HTTP ERROR: " + (error.statusText || error.message || "Unknown error"));
                console.error("HTTP ERROR:", error);

            })
            .finally(function () {

                //$scope.isLoading = false;

            });
    };
    $scope.loadprioritylist = function () {

        debugger;
        //$scope.isLoading = true;

        myprojectService.getprioritylist()
            .then(function (response) {

                debugger;
                // assign data
                $scope.prioritylists = response.data.d;

            })
            .catch(function (error) {

                debugger;
                alert("HTTP ERROR: " + (error.statusText || error.message || "Unknown error"));
                console.error("HTTP ERROR:", error);

            })
            .finally(function () {

                //$scope.isLoading = false;

            });
    };
    $scope.loadtaglist = function () {

        debugger;
        //$scope.isLoading = true;

        myprojectService.gettaglist()
            .then(function (response) {

                debugger;
                // assign data
                $scope.tagslist = response.data.d;

            })
            .catch(function (error) {

                debugger;
                alert("HTTP ERROR: " + (error.statusText || error.message || "Unknown error"));
                console.error("HTTP ERROR:", error);

            })
            .finally(function () {

                //$scope.isLoading = false;

            });
    };
    $scope.loadprojectfiles = function (c,index) {

        debugger;
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
        debugger
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

        debugger;
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

        debugger;
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
        debugger
        var isupdate = "0";
        myprojectService.removeproject(c.id)
            .then(function (response) {
                debugger
                if (response.data.d) {
                    isupdate = response.data.d;
                    if (isupdate == "1") {
                        debugger
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

        var payload = {
            projctid: $scope.currentlist.id,
            extendStrtdate: $scope.extendObj.fromdate,
            extendEnddate: $scope.extendObj.todate
        };

        myprojectService.SvaeExtended_details(payload)
            .then(function (response) {
                debugger
                if (response.data.d) {

                    let newrow = response.data.d;

                    if (newrow.status == "1") {

                        let pindex = $scope.projectlist.findIndex(
                            m => m.id == $scope.currentlist.id
                        );

                        if (pindex !== -1) {

                            if (!$scope.projectlist[pindex].extendedlists) {
                                $scope.projectlist[pindex].extendedlists = [];
                            }

                            $scope.projectlist[pindex].extendedlists.push({
                                extended_from:newrow.extended_from,
                                extended_to:newrow.extended_to,
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
    $scope.SvaeExtended11 = function () {
       debugger
        if (!$scope.currentlist) return;

        var payload = {
            projctid: $scope.currentlist.id,
            extendStrtdate:$scope.extendObj.fromdate,
            extendEnddate: $scope.extendObj.todate
        };

        myprojectService.SvaeExtended_details(payload)
            .then(function (response) {
                debugger
                if (response.data.d) {
                    let newrow = response.data.d
                    if (newrow.status == "1") {

                        $scope.$applyAsync(function () {
                            let pindex = $scope.projectlist.findIndex(m => m.id == $scope.currentlist.id);
                            $scope.projectlist[pindex].push(newrow);

                            //$scope.extendObj =[];
                        });
                    }
                }

                //$('#btnclsextended').modal('hide');

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
        debugger;

        if ($scope.currentprojectid != "0" && $scope.currentprojectid != "") {
            if (!$scope.currentlist) return;
            let obj = $scope.currentlist;

            let resultnew = null;
            // API call based on type

            resultnew = myprojectService.getitemdetails(obj.id);

            if (!resultnew) return;

            resultnew.then(function (res) {
                debugger;
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
        debugger;
        var pid = "0";
        if (document.getElementById("ContentPlaceHolder1_hdnnewprojectid").value != "") {
            pid = document.getElementById("ContentPlaceHolder1_hdnnewprojectid").value;
        }


        let resultnew = null;
        // API call based on type

        resultnew = myprojectService.getitemdetails(pid);

        if (!resultnew) return;

        resultnew.then(function (res) {
            debugger;
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
        debugger;
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
                debugger;
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

        debugger;
        
        var pid = 0;
        let resultnew;
        
            pid = c.id;
            resultnew = myprojectService.UpdateP_Status(c.id, item.id, item.iscomplete);        

        if (resultnew) {
            resultnew.then(function (res) {
                debugger;
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

        debugger;


        var pid = 0;
        let resultnew;

        pid = c.id;
        resultnew = myprojectService.UpdateP_Priority(c.id, item.id);

        if (resultnew) {
            resultnew.then(function (res) {
                debugger;
                if (res.data.d != "1") {
                    alert(res.data.d);
                }
                else {
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
        debugger
        $scope.isLoading = true;
      var fdate = moment()
            .startOf('month')
            .format('MM/DD/YYYY');

        var tdate = moment()
            .endOf('month')
            .format('MM/DD/YYYY');

        // GET PICKER IF AVAILABLE
        var picker = $('#daterange').data('daterangepicker');
        if (picker) {
            fdate = picker.startDate.format('MM/DD/YYYY');
            tdate = picker.endDate.format('MM/DD/YYYY');
        }
        myprojectService.getprojectlist(fdate, tdate).then(function (response) {            
            var data = response.data.d;
            if (typeof data === "string") {
                data = JSON.parse(data);
            }
            $scope.projectlist = data || [];
$scope.myprojectlist_bystatus.length=0;
            $scope.mytotalprojectlist=$scope.projectlist.length;
if($scope.statuslists.length>0)
{
    angular.forEach($scope.statuslists, function (status) {

        var total = $scope.projectlist.filter(function (p) {
            return p.ProjectStatus == status.id;
        }).length;
if(total>0)
{
        $scope.myprojectlist_bystatus.push({
            statusid: status.id,
            statusname: status.name,
            statuscolor: status.colorname,
            statusicon: status.statusicon,
            total: total
        });
}

    });
}
            angular.forEach($scope.projectlist, function (c) {

                c.StartDate = parseDate(c.StartDate);
                c.TargetDate = parseDate(c.TargetDate);
                c.AwardDate = parseDate(c.AwardDate);
                c.DrawingApprovalDate = parseDate(c.DrawingApprovalDate);
                if (c.extendedlists && c.extendedlists.length > 0) {
                    angular.forEach(c.extendedlists, function (e) {
                        e.fromdate = parseDate(e.fromdate);
                        e.todate = parseDate(e.todate);
                    });
                }

            });
            
                    initTableUI();
            $scope.isLoading = false;
            
        }, function (error) {
            console.error(error);
            alert("Error: " + error.status);
        });
        
    };


   
        $scope.loadstatuslist();
        $scope.loadprioritylist();    
        $scope.loaduserlist();
        $scope.loadtaglist();
        $scope.loadcustomerlist();
        $scope.loadprojectdata();
      //#region manage project files
        $scope.uploadedFiles = [];
       $scope.bindtasklist2 = function (fromDate, toDate) {
        debugger
        //alert(fromDate + "/" + toDate);
        $scope.loadprojectdata();

    };

    const dropZone = document.getElementById("dropZone");
    const fileInput = document.getElementById("fileupload");

    // Click drop zone → open file picker
    $scope.openFilePicker = function () {


        fileInput.click();
    };

    // Files selected via input
    $scope.filesSelected = function (files) {
        debugger

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
        debugger

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
        debugger
        myprojectService.uploadfiles(files, pid, id, type).then(function (res) {
            debugger
           
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
        debugger       
        //if (!confirm("Are you sure you want to delete this file?")) return;
        myprojectService.removedoc(fileId).then(function (res) {
            debugger
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
        debugger
        
    };
    //#endregion

// end scope
});
function formatDateDDMMYYYY(dateInput) {
    debugger
    //alert(dateInput);
    if (!dateInput) return "";

    var d;
    if (Object.prototype.toString.call(dateInput) === "[object Date]") {
        d = dateInput;
    }
    // ✅ If string like "Tue Mar 24 2026..."
    else {
        d = new Date(dateInput);
    }

    if (isNaN(d)) return "";

    var day = ('0' + d.getDate()).slice(-2);
    var month = ('0' + (d.getMonth() + 1)).slice(-2);
    var year = d.getFullYear();

    return day + '-' + month + '-' + year;
}
function toServerDate(ddmmyyyy) {

    if (!ddmmyyyy) return "";

    var parts = ddmmyyyy.split("-");
    return parts[2] + "-" + parts[1] + "-" + parts[0]; // yyyy-MM-dd
}
function parseDate(val) {
    if (!val) return null;

    if (typeof val === 'string' && val.indexOf('/Date(') === 0) {
        var timestamp = parseInt(val.replace(/[^0-9]/g, ''));
        return new Date(timestamp);
    }

    var d = new Date(val);
    return isNaN(d.getTime()) ? null : d;
}
function inittooltip() {

    //document.querySelectorAll('[data-bs-toggle="tooltip"]').forEach(function (el) {

    //    let instance = bootstrap.Tooltip.getInstance(el);

    //    if (instance) {
    //        instance.dispose();   // IMPORTANT FIX
    //    }

    //});

   
    //document.querySelectorAll('[data-bs-toggle="tooltip"]').forEach(function (el) {
    //    new bootstrap.Tooltip(el);
    //});
    document.querySelectorAll('[data-bs-toggle="tooltip"]').forEach(function (el) {

        let instance = bootstrap.Tooltip.getInstance(el);
        let newTitle = el.getAttribute('data-bs-title');

        if (instance) {
            instance.setContent({ '.tooltip-inner': newTitle });
        } else {
            new bootstrap.Tooltip(el);
        }

    });


}
function initTableUI() {
    //alert("call");
    inittooltip(); 
    initProjectTableColumnDragDrop();
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
        debugger
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
                b1.innerHTML = '<i class="bi bi-arrow-left-square me-1" title="Add Before"></i> ';

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
                b2.innerHTML = ' <i class="bi bi-arrow-right-square ms-1" title="Add After"></i>';

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
