
app.controller("usertaskCtrl", function ($scope,$q,$http,$timeout,$document,usertaskService,$compile) {
   //alert("ok");
 console.log("controller loaded");
 //#region add variables
$scope.showTagDropdown = false;
$scope.mytasklist_bystatus =[];
    $scope.isload = "0";
    $scope.addnew_tag = "0";
    $scope.multiselect = "0";
    $scope.multiselectids = [];
    $scope.multiselect_status = [];
    $scope.selectedStatus =[];
    $scope.freesearch = "0";
   
    $scope.mytasklist_history = [];
    $scope.currentlistIndex = "0";
    $scope.currentlist=[];
    $scope.tagtype="project";
    $scope.currenttaskid="0";
    $scope.showlist = true;
    $scope.owners=[];
    $scope.mytasklist = [];
    $scope.mytasktaglist = [];
    $scope.selectedtagslistids = [];
    $scope.selectedtagIds = [];
    $scope.mytasklist_status = [];
    $scope.currentPage = 1;
    $scope.pageSizes = [10, 20, 50, 100];
    $scope.types = ["1", "2"];
    $scope.pageSize = 10;
    $scope.statuslists = [];
    $scope.prioritylists = [];
    //#endregion
    //#region add shorting
    $scope.sortColumn = "id";
    $scope.reverseSort = false;
   $scope.tagsnew = {
        id: "",
        tagname: "",
        tagcolor: ""
    };

    function waitForTable(callback) {

        let retries = 0;

        let interval = setInterval(function () {

            let table = document.querySelector('#tbDetails');

            if (
                table &&
                table.querySelectorAll('tbody tr').length > 0
            ) {

                clearInterval(interval);

                callback();
            }

            retries++;

            // safety stop
            if (retries > 100) {
                clearInterval(interval);
            }

        }, 100);
    }
    $scope.tableUiInitialized = false;
    $scope.isselectAll = false;
$scope.clearselectedtag=function (c) {
$scope.selectedtagIds =[];
}
$scope.checkselect=function (c) {
$scope.selectedStatus =[];
debugger
if(c.selected)
           {
        //var index = $scope.multiselectids.indexOf(c.id&&c.type);
        var index = $scope.multiselectids.findIndex(function(x) {return x.id == c.id && x.type == c.tasktype;}); 
        if (index ==-1) { 
if (c._oldcomplete_value == null || c._oldcomplete_value == undefined) 
{
         c._oldcomplete_value=c.complete_value;
         c._oldpending_value=c.pending_value; 
}         
        $scope.multiselectids.push({id:c.id,type:c.tasktype});
        }
     
}
else
{
        //var index = $scope.multiselectids.indexOf(c.id); 
        var index = $scope.multiselectids.findIndex(function(x) {return x.id == c.id && x.type == c.tasktype;}); 
        if (index >-1) {
        $scope.multiselectids.splice(index, 1);
                 
        }
}



    if($scope.multiselectids.length>0)
    {
    
    $scope.multiselect = "1";
    }
    else
    {
    $scope.multiselect = "0";
    $scope.multiselectids =[];
    }
};
    $scope.selectall = function (checked) {
debugger
        //alert('tt');
$scope.selectedStatus =[];
        $scope.multiselect="0";
       var Mselectedids="";
        $scope.isselectAll = checked;
        angular.forEach($scope.mytasklist, function (c) {
debugger
            c.selected = checked;
            if(checked)
            {                
                       // var index = $scope.multiselectids.indexOf(c.id); 
var index = $scope.multiselectids.findIndex(function(x) {return x.id == c.id && x.type == c.type;}); 
        
                        if (index ==-1) {           
                        //$scope.multiselectids.push(c.id);
if (c._oldcomplete_value == null || c._oldcomplete_value == undefined) 
{
                        c._oldcomplete_value=c.complete_value;
                        c._oldpending_value=c.pending_value;
}
                        $scope.multiselectids.push({id:c.id,type:c.tasktype});
                        }
            }
            else
            {
                        var index = $scope.multiselectids.findIndex(function(x) {return x.id == c.id && x.type == c.type;}); 
                    if (index >-1) {           
                        $scope.multiselectids.splice(index,1);
                        }
            }
        });
        if($scope.isselectAll==true)
        {
         if($scope.multiselectids.length>0)
         $scope.multiselect="1";
        else
        $scope.multiselect="0";
        }
        $scope.$applyAsync(); // VERY IMPORTANT
    };
   
    $scope.tableRendered = function () {

        // prevent multiple calls
        if ($scope.tableUiInitialized) {
            return;
        }

        $scope.tableUiInitialized = true;

        setTimeout(function () {

            // init
            initTableUI();

            $scope.$applyAsync(function () {
                $scope.isLoading = false;
            });

        }, 1000);

    };

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
    $scope.bindtasklist2 = function (fromDate, toDate) {
        debugger
        //alert(fromDate + "/" + toDate);
        //$scope.loadsubtasklist_count();
        $scope.loadsubtasklist();

    };
    $scope.viewsearch = function ()
    {
        debugger
        if ($scope.freesearch=="0")
            $scope.freesearch = "1";
        else
            $scope.freesearch = "0";
    };
    $scope.getRemainingOwners = function (owners) {
        if (!owners || owners.length <= 3) return "";

        return owners.slice(3).map(function (o) {
            return o.name;
        }).join(", ");
    };
    $scope.getRemainingtags = function (tags) {
        if (!tags || tags.length<=1) return "";

        return tags.slice(1).map(function (o) {
            return o.tagname;
        }).join(", ");
    };
    $scope.removetags = function (c,tagid) {
        debugger
            $scope.removeTagToServer(c,tagid);
           
        setTimeout(function () {
            initTableUI();

        }, 500);

    };
$scope.removetagsnew = function (tagid) {
debugger
        //var index = $scope.selectedtagIds.indexOf(tagid);
       var index = $scope.selectedtagIds.findIndex(x => x.id == tagid);
        if (index > -1) {
            $scope.selectedtagIds.splice(index, 1);
            
                $scope.removetagToServernew($scope.currentlist, $scope.selectedtagIds, tagid);
           

        }
       $scope.filteredTagList(); 

    };
$scope.removetagsnew_multi = function (tagid) {
debugger
        //var index = $scope.selectedtagIds.indexOf(tagid);
       var index = $scope.selectedtagIds.findIndex(x => x.id == tagid);
        if (index > -1) {
            $scope.selectedtagIds.splice(index, 1);           
        }
       $scope.filteredTagList(); 

    };
$scope.removetagToServernew = function (obj, selected_obj, removetagid) {
        debugger
        
        //var ids = selected_obj.join(",");
        var ids = selected_obj.map(function(x) {return x.id;}).join(",");
        let resultnew;

        
        resultnew = usertaskService.UpdateP_Tag(obj.id, ids);
        
        if (resultnew) {
            resultnew.then(function (res) {
                debugger;
                if (res.data.d == "1") {
                    var tindex=$scope.mytasklist.findIndex(m => m.id == obj.id);
                        if (tindex !== -1) {
                           var tasks=$scope.mytasklist[tindex];
                            var indx = tasks.tags.findIndex(m => m.id == removetagid);
                            if (indx !== -1) {                        
                                tasks.tags.splice(indx, 1);
                            }

                        }
                    
                }
                //console.log(res);
            })
                .catch(function (err) {
debugger
                    //console.error(err);
                    alert("Update failed");
                });
        }
    };

    $scope.removeTagToServer = function (c,tagid) {
        debugger
       
        let resultnew;
var updateids="";
    angular.forEach(c.tags, function (t) 
    {
        if(t.id!=tagid)
        {
            if(updateids=="")
            {updateids=t.id;}
            else
            {updateids=","+t.id;}
        }
    });
        
        resultnew = usertaskService.removetags(c.id,c.tasktype,updateids);
       
        if (resultnew) {
            resultnew.then(function (res) {
                debugger;
                if (res.data.d == "1") {                    
                    var tagindx = c.tags.findIndex(f => f.id === tagid);
                    if (tagindx > -1) {
                        c.tags.splice(tagindx, 1);

                    }
                }
                //console.log(res);
            })
                .catch(function (err) {
                    //console.error(err);
                    alert("Update failed");
                });
        }
    };
$scope.closenewtabgdiv = function () {
document.getElementById('dvnewtag').style.display = 'none';
var notes = document.getElementById('ContentPlaceHolder1_tags');
        if (!notes) { return; }
notes.value="";
}
$scope.stopTagEvent = function ($event) {
        if ($event) $event.stopPropagation();
    };

    $scope.toggleTag = function (tag) {
        debugger
if(tag.id=="-1")
{
document.getElementById('dvnewtag').style.display = '';
$scope.closelist();
}
else
{
         document.getElementById('dvnewtag').style.display = 'none';
        //var index = $scope.selectedtagIds.indexOf(tag.id); 
        var index = $scope.selectedtagIds.findIndex(function (x) {return x.id == tag.id;});       
        if (index > -1) {           
        } else {

           $scope.selectedtagIds.push({
            id:tag.id,
            name:tag.tagname
            }); // select
           $scope.updateTagsToServer($scope.currentlist,$scope.selectedtagIds, tag.id);            
        }
        $scope.tagSearch = "";
        $scope.filterTagList();
}
        
    };
$scope.toggleTag_multi = function (tag) {
        debugger
$scope.addnew_tag="0";
 if(tag.id=="-1")
{
document.getElementById('dvnewtag2').style.display = '';
$scope.closelist();
$scope.addnew_tag="1";
}
else
{
         document.getElementById('dvnewtag2').style.display = 'none';
        var index = $scope.selectedtagIds.findIndex(function (x) {return x.id == tag.id;});       
        if (index > -1) {           
        } else {
           $scope.selectedtagIds.push({
            id:tag.id,
            name:tag.tagname
            });                     
        }
        $scope.tagSearch = "";
        $scope.filterTagList();
}
        
    };
    $scope.updateTagsToServer = function (obj, selected_obj, newid) {
        debugger
        if (!selected_obj || selected_obj.length === 0) {
            alert("No tag selected");
            return;
        }
        //var ids = selected_obj.join(",");
       var ids = selected_obj.map(function(x) {return x.id;}).join(",");
        let resultnew;

       
        resultnew = usertaskService.UpdateP_Tag(obj.id,obj.tasktype, ids);
        
        if (resultnew) {
            resultnew.then(function (res) {
                debugger;
                if (res.data.d == "1") {
                    var addlist = $scope.mytasktaglist.find(m => m.id == newid);
                    if (addlist) {
                        let newrow = angular.copy($scope.tagsnew);
                        newrow.id = addlist.id;
                        newrow.tagname = addlist.tagname;                       
                        newrow.tagcolor = addlist.tagcolor;                       
                        obj.tags.push(newrow);
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


  $scope.filterTagList = function () {
debugger
        var search = ($scope.tagSearch || "").toLowerCase().trim();
        $scope.filteredTagList = $scope.mytasktaglist.filter(function (item) {
            var alreadySelected = $scope.selectedtagIds.some(function (s) {
                return s.id == item.id;
            });
            var match = item.tagname.toLowerCase().indexOf(search) !== -1;
            return !alreadySelected && match;
        });

        //$scope.supplierHighlightIndex = $scope.filterTagList.length > 0 ? 0 : -1;
    };
$scope.showhistory = function (taskid,htype) {
debugger
 usertaskService.gettaskhistory(taskid,htype).then(function (response) {

        debugger;

        $scope.mytasklist_history = response.data.d;
if ($scope.mytasklist_history) {
//alert($scope.mytasklist_history.taskname);
                        angular.forEach($scope.mytasklist_history.history, function (s) {
                            

                            if (s.startdate) {
                                if (typeof s.startdate === 'string' && s.startdate.indexOf('/Date(') === 0) {
                                    var timestamp = parseInt(s.startdate.substr(6));
                                    s.startdate = new Date(timestamp);
                                } else {
                                    s.startdate = new Date(s.startdate);
                                }
                            }
if (s.enddate) {
                                if (typeof s.enddate === 'string' && s.enddate.indexOf('/Date(') === 0) {
                                    var timestamp = parseInt(s.enddate.substr(6));
                                    s.enddate = new Date(timestamp);
                                } else {
                                    s.enddate = new Date(s.enddate);
                                }
                            }
});
}
        

    }).catch(function (error) {

        alert("HTTP ERROR: " + (error.statusText || error.message || "Unknown error"));
        console.error("HTTP ERROR:", error);

    }).finally(function () {

     
    

    });
}
$scope.getOwnerName = function (ownerid) {
        var owner = $scope.owners.find(function (o) {
            return o.id == ownerid;
        });

        return owner ? owner.name : '';
    };
$scope.getOwnerProfile = function (ownerid) {

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
$scope.openTagDropdown = function ($event) {
debugger
    $event.stopPropagation();
    $scope.showTagDropdown = true;
$scope.tagSearch = "";
        $scope.filterTagList();
};
$scope.closetagwindow = function() {
$scope.selectedtagIds=[];
 document.getElementById('dvnewtag').style.display = 'none';
 document.getElementById('ContentPlaceHolder1_tags').value = '';
 document.getElementById('tagscolor').value = '';
 document.getElementById('dvnewtag2').style.display = 'none';
 document.getElementById('ContentPlaceHolder1_tags2').value = '';
 document.getElementById('tagscolor2').value = '';
}
 $scope.closelist = function() {
debugger

if($scope.showTagDropdown==true)
    $scope.showTagDropdown = false;
};

//$scope.getSelectedTags = function () {
       // return $scope.mytasktaglist.filter(o => $scope.selectedtagIds.includes(o.id));
   // };
$scope.getSelectedTags = function () {

    return $scope.mytasktaglist.filter(t =>
        $scope.selectedtagIds.some(s => s.id == t.id)
    );

};

$scope.loadtaglist = function () {

    debugger;
    //$scope.isLoading = true;

    usertaskService.gettaglist()
        .then(function (response) {
            debugger;
            // assign data
            $scope.mytasktaglist = response.data.d;

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

        usertaskService.getuserlist()
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
    //$scope.isLoading = true;

    usertaskService.getstatuslist()
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
$scope.selectStatus_multi = function (item) {

        debugger;
        
        var pid = 0;
        let resultnew;
       if($scope.multiselectids.length==0)
       {return}
       if(item.id=="")
       {return}
            //var ids = $scope.multiselectids.join(",");
            var ids = $scope.multiselectids.map(function(x) {return x.id + ":" + x.type;}).join(",");
            resultnew = usertaskService.UpdateSubTask_StatusMulti(ids,item.id,item.iscomplete);        

        if (resultnew) {
            resultnew.then(function (res) {
                debugger;
                if (res.data.d != "1") {
                    alert(res.data.d);
                }
                else {
$scope.selectedStatus.push({name:item.name,colorname:item.colorname});
                 angular.forEach($scope.multiselectids, function (s) {
                    let sindex = $scope.mytasklist.findIndex(function (m) {return m.id == s.id&&m.tasktype==s.type;});
                     var task = $scope.mytasklist[sindex];                   
                    task.P_Status=item.id;
                    task.status_color = item.colorname;
                    task.status_name = item.name; 
                    if(item.iscomplete=="1")
                    {
                     task.complete_value ="100";
                     task.pending_value ="0";
                    }
                    else
                    {
                    task.complete_value =task._oldcomplete_value;
                    task.pending_value =task._oldpending_value;
                    }

});

        angular.forEach($scope.mytasklist, function (c) {
debugger
            c.selected = false;
});
try
{
$scope.multiselect = "0";
var checkall=document.getElementById('chkAll');
if(checkall)
checkall.checked=false;
}
catch{}
                    
                   
                }
                //alert(res.d);
                //console.log(res);
            })
                .catch(function (err) {
debugger
                    console.error(err);
                    //alert("Update failed");
                });
        }
    };
$scope.selectStatus = function (c,item) {

        debugger;
        
        var pid = 0;
        let resultnew;
        
            pid = c.id;
 if (c.tasktype == 'milestone') {        
        resultnew = usertaskService.UpdateMilestone_Status(c.id, item.id, item.iscomplete);
    }
    else if (c.tasktype == 'task') {       
        resultnew = usertaskService.UpdateTask_Status(c.id, item.id, item.iscomplete);
    }
    else if (c.tasktype == 'subtask') {       
        resultnew = usertaskService.UpdateSubTask_Status(c.id, item.id, item.iscomplete);
    }
           // resultnew = usertaskService.UpdateSubTask_Status(c.id,c.tasktype, item.id,item.iscomplete);        

        if (resultnew) {
            resultnew.then(function (res) {
                debugger;
                if (res.data.d != "1") {
                    alert(res.data.d);
           
                }
                else {

                    c.P_Status = item.id;
                    c.status_color = item.colorname;
                    c.status_name = item.name;                    
                    if(item.iscomplete=="1")
                    {
                      c.complete_value ="100";
                      c.pending_value ="0";
                    }
                    else
                    {
                    c.complete_value =c._oldcomplete_value;
                    c.pending_value =c._oldpending_value;
                    }
                   $scope.filterstatuslist_count();
                   $scope.$applyAsync();
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

$scope.loadprioritylist = function () {

    debugger;
    //$scope.isLoading = true;

    usertaskService.getprioritylist()
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
$scope.getContrastColor = function (hex) {

    if (!hex) return '#000';

    hex = hex.replace('#', '');

    if (hex.length === 3) {
        hex = hex[0]+hex[0]+hex[1]+hex[1]+hex[2]+hex[2];
    }

    var r = parseInt(hex.substr(0,2),16);
    var g = parseInt(hex.substr(2,2),16);
    var b = parseInt(hex.substr(4,2),16);

    var brightness = ((r * 299) + (g * 587) + (b * 114)) / 1000;

    return brightness > 155 ? '#000' : '#fff';
};
$scope.loadsubtasklist_count = function () {
//debugger
   // alert("ok");
    $scope.isLoading = true;
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
    usertaskService.getsubtaskslist_count(fdate, tdate).then(function (response) {

        debugger;

        $scope.mytasklist_status = response.data.d;
        

    }).catch(function (error) {

        alert("HTTP ERROR: " + (error.statusText || error.message || "Unknown error"));
        console.error("HTTP ERROR:", error);

    }).finally(function () {

       
    $scope.isLoading = false;
        

    });
      
};

    $scope.loadsubtasklist = function () {
debugger
    //alert("ok");
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
       
   
        usertaskService.getsubtaskslist(fdate, tdate).then(function (response) {

        debugger;

        $scope.mytasklist = response.data.d;
        $scope.isload = "1";
if ($scope.mytasklist && $scope.mytasklist.length > 0) {
$scope.filterstatuslist_count();
                        angular.forEach($scope.mytasklist, function (s) {
                            

                            if (s.Startdate) {
                                if (typeof s.Startdate === 'string' && s.Startdate.indexOf('/Date(') === 0) {
                                    var timestamp = parseInt(s.Startdate.substr(6));
                                    s.Startdate = new Date(timestamp);
                                } else {
                                    s.Startdate = new Date(s.Startdate);
                                }
                            }
if (s.Targetdate) {
                                if (typeof s.Targetdate === 'string' && s.Targetdate.indexOf('/Date(') === 0) {
                                    var timestamp = parseInt(s.Targetdate.substr(6));
                                    s.Targetdate = new Date(timestamp);
                                } else {
                                    s.Targetdate = new Date(s.Targetdate);
                                }
                            }
                            
                        });
    
    
    setTimeout(function () {

            // init

            initTableUI();


            $scope.$applyAsync(function () {               
            });

        }, 0);
    
                    }
        $scope.isLoading = false;
           $scope.autostartTimers(); 
    }).catch(function (error) {

        alert("HTTP ERROR: " + (error.statusText || error.message || "Unknown error"));
        console.error("HTTP ERROR:", error);

            })
    //        .finally(function () {

    //    $scope.autostartTimers();        
    //   $scope.isLoading = false;
    

    //});
       
};
$scope.activeTimerId = 0;
$scope.currentTimer = null;
$scope.autostartTimers = function () {

    angular.forEach($scope.mytasklist, function (c) {

        // server side value
        c.elapsedMilliseconds =
            c.elapsedMilliseconds || 0;

        // format existing timer
        c.timerText =
            formatTimer(c.elapsedMilliseconds);

        // auto start only running timers
        if (c.isend == 0&&c.isrun == 1 && c.ispause == 0) {

            // avoid duplicate interval
            if (c.currentTimer) {

                clearInterval(c.currentTimer);
            }

            c.currentTimer = setInterval(function () {

                c.elapsedMilliseconds += 10;

                c.timerText =
                    formatTimer(
                        c.elapsedMilliseconds
                    );

                $scope.$applyAsync();

            }, 10);
        }
    });
};


$scope.timerstart = function(c){
debugger
    if(c.isrun && !c.ispause)
        return;
usertaskService.SaveTimerstartdetails(c.tasktype,c.ProjectId,c.Milestoneid,c.Task,c.id)
.then(function(res) {
    if (!res) {
        alert("Failed to calculate end date");
        return;
    } 
    if(res.data.d!="0")
    {
      //alert("save successfully");
        c.isrun = 1;
        c.ispause = 0;
        c.isend = 0;
        c.elapsedMilliseconds = c.elapsedMilliseconds || 0;
        c.timerText = formatTimer(c.elapsedMilliseconds);
        c.currentTimer = setInterval(function(){
            c.elapsedMilliseconds += 10;
            c.timerText = formatTimer(c.elapsedMilliseconds);

            $scope.$apply();

        },10);
    }   
    //let end = normalize(parseDotNetDate(newEndDate.data.d));
     //newRow.EndDate=end;

})
.catch(function(err) {

    console.error("Update failed", err);

});
        /*c.isrun = 1;
        c.ispause = 0;
        c.elapsedMilliseconds = c.elapsedMilliseconds || 0;
        c.timerText = formatTimer(c.elapsedMilliseconds);
        c.currentTimer = setInterval(function(){
            c.elapsedMilliseconds += 10;
            c.timerText = formatTimer(c.elapsedMilliseconds);

            $scope.$apply();

        },10);*/
};
$scope.timerpause = function(c){
 debugger
    if(c.currentTimer){

        clearInterval(c.currentTimer);
    }
    $scope.currentlist=c;
    c.ispause = 1;
};
$scope.closepause = function()
{
debugger
    if (!$scope.currentlist)
        return;
    var sid = $scope.currentlist.id;
    var s = $scope.mytasklist.find(function (f) {
        return f.id == sid;

    });

    if (!s)
        return;
    s.isrun = 1;
    s.ispause = 0;
    // restart timer
    if (s.currentTimer) {

        clearInterval(s.currentTimer);
    }

    s.currentTimer = setInterval(function () {
        s.elapsedMilliseconds += 10;
        s.timerText =formatTimer(s.elapsedMilliseconds);

        $scope.$applyAsync();

    }, 10);
}

$scope.timerresume = function(c){
debugger
usertaskService.SaveTimerresumedetails(c.tasktype,c.ProjectId,c.Milestoneid,c.Task,c.id)
.then(function(res) {
    if (!res) {
        alert("Failed to calculate end date");
        return;
    } 
    if(res.data.d!="0")
    {
debugger
      //alert("save successfully");
        c.ispause = 0;
        c.currentTimer = setInterval(function(){
        c.elapsedMilliseconds += 10;
        c.timerText = formatTimer(c.elapsedMilliseconds);
        $scope.$apply();

        },10);
    }   
    //let end = normalize(parseDotNetDate(newEndDate.data.d));
     //newRow.EndDate=end;

})
.catch(function(err) {

    console.error("Update failed", err);

});
    /*c.ispause = 0;
    c.currentTimer = setInterval(function(){
        c.elapsedMilliseconds += 10;
        c.timerText = formatTimer(c.elapsedMilliseconds);
        $scope.$apply();

    },10);*/
};
$scope.timersstop = function(c){
debugger
    if(c.currentTimer){

        clearInterval(c.currentTimer);

    }
    //c.isrun = 0;
    //c.ispause = 0;
    //c.currentTimer = null;
    $scope.currentlist=c;
   
   
};
$scope.closestop = function () {
debugger
    if (!$scope.currentlist)
        return;
    var sid = $scope.currentlist.id;
    var s = $scope.mytasklist.find(function (f) {
        return f.id == sid;

    });

    if (!s)
        return;

    s.isrun = 1;
  if(s.ispause==0)
    {
    if (s.currentTimer) {

        clearInterval(s.currentTimer);
    }
    s.currentTimer = setInterval(function () {
        s.elapsedMilliseconds += 10;
        s.timerText =formatTimer(s.elapsedMilliseconds);

        $scope.$applyAsync();

    }, 10);
  }

};


function formatTimer(totalMilliseconds){

    var totalSeconds = Math.floor(totalMilliseconds / 1000);

    var hrs = Math.floor(totalSeconds / 3600);

    var mins = Math.floor((totalSeconds % 3600) / 60);

    var secs = totalSeconds % 60;

    var ms = totalMilliseconds % 1000;

    return pad(hrs) + ':' +
           pad(mins) + ':' +
           pad(secs) + ':' +
           padMs(ms);
}

function pad(n){

    return n < 10 ? '0' + n : n;
}

function padMs(n){

    if(n < 10) return '00' + n;

    if(n < 100) return '0' + n;

    return n;
}

$scope.SavePauseDetials=function()
{
//alert("ok");
debugger
    if(!$scope.currentlist)
    {
    return;
    }
var notes=document.getElementById('ContentPlaceHolder1_Notes');
if(!notes){return;}
if(notes.value==""){
alert("Please enter note");
        return;
    }
    document.getElementById('divpause').style.display = 'none';
    document.getElementById('pnlimgpause').style.display = 'block';
                var ProjectId = $scope.currentlist.ProjectId;
                var Milestoneid = $scope.currentlist.Milestoneid;
                var taskid = $scope.currentlist.Task;
                var subtaskid =$scope.currentlist.id;
                var utype =$scope.currentlist.tasktype;

usertaskService.SaveTimerpausedetails(utype,ProjectId,Milestoneid,taskid,subtaskid,notes.value)
.then(function(res) {
    if (!res) {
        alert("Failed to calculate end date");
        return;
    } 
    if(res.data.d=="1")
    {
debugger
      notes.value="";
     //alert("save successfully");
     document.getElementById('btnclsNotes2').click();
    } 
    document.getElementById('pnlimgpause').style.display = 'none';
    document.getElementById('divpause').style.display = 'block';
    //let end = normalize(parseDotNetDate(newEndDate.data.d));
     //newRow.EndDate=end;

})
.catch(function(err) {

    console.error("Update failed", err);
    alert("Update failed");
    document.getElementById('pnlimgpause').style.display = 'none';
    document.getElementById('divpause').style.display = 'block';
});

}
    $scope.SaveStopDetials=function()
{
//alert("ok");
debugger
    if(!$scope.currentlist)
    {
    return;
    }
    var notes=document.getElementById('ContentPlaceHolder1_TimerNotes');
    if(!notes){return;}
    if(notes.value==""){
    alert("Please enter note");
        return;
    }
    document.getElementById('divstop').style.display = 'none';
    document.getElementById('pnlimgstop').style.display = 'block';
                var ProjectId = $scope.currentlist.ProjectId;
                var Milestoneid = $scope.currentlist.Milestoneid;
                var taskid = $scope.currentlist.Task;
                var subtaskid =$scope.currentlist.id;
                var timingdetails =$scope.currentlist.timerText;
                var utype =$scope.currentlist.tasktype;

usertaskService.SaveTimer(utype,ProjectId,Milestoneid,taskid,subtaskid,notes.value,timingdetails)
.then(function(res) {
    if (!res) {
        alert("Failed to calculate end date");
        return;
    } 
    if(res.data.d=="1")
    {
debugger
      //alert("save successfully");
      notes.value=""
      document.getElementById('btnclsTimer2').click();
     let sindex = $scope.mytasklist.findIndex(m => m.id == subtaskid&&m.tasktype ==utype);
     var s=$scope.mytasklist[sindex];
         if (s.currentTimer) {

                clearInterval(s.currentTimer);

                s.currentTimer = null;
            }
            // reset flags
            s.isend = 1;
            s.isrun = 0;
            s.ispause = 0;
            s.elapsedMilliseconds = 0;        
            
            s.timerText = "00:00:00:000";
            usertaskService.Getspenttime_subtask(s.id,s.tasktype)
            .then(function (res) {
            debugger
                s.timerText_spent = res.data.d;
            })
            .catch(function (err) {
debugger
            });

            $scope.$applyAsync();
    }  
    document.getElementById('pnlimgstop').style.display = 'none';
    document.getElementById('divstop').style.display = 'block';
    

})
.catch(function(err) {
debugger
    console.error("Update failed", err);
    alert("Update failed");
    document.getElementById('pnlimgstop').style.display = 'none';
    document.getElementById('divstop').style.display = 'block';
});

    }
    $scope.Addtags = function (c,addtype)
    {
        debugger;
        $scope.currentlist = c;
        $scope.tagtype = addtype;
        $scope.selectedtagIds = [];
         if(c.tags)
        {
               // $scope.selectedtagIds = c.tags.map(t => t.id);
         $scope.selectedtagIds = c.tags.map(function (x) {
                            return {
                                id: x.id,
                                name: x.tagname
                            };
                        });
        }
$scope.filteredTagList();
        //$scope.SvaeadTags(addtype);
    }
    $scope.SvaeadTags = function ()
    {
        debugger;
        //alert($scope.tagtype);
        if (!$scope.currentlist){
            return;
        }
        var c = $scope.currentlist;
        var tagtype = $scope.tagtype;
        var notes = document.getElementById('ContentPlaceHolder1_tags');
        if (!notes) { return; }
        if (notes.value == "") {
            alert("Please enter tagname");
            notes.focus();
            return;
        }
        var notescolor = document.getElementById('tagscolor');
        if (!notescolor) { return; }
        if (notescolor.value == "") {
            alert("Please enter tag color");
            notescolor.focus();
            return;
        }
        usertaskService.SaveTags(c, notes.value, notescolor.value, tagtype)
            .then(function (res) {
                if (!res) {
                    alert("Failed");
                    return;
                }
                if (res.data.d!= "0"&& res.data.d!= "") {
                    debugger
                    //alert("save successfully");
                    document.getElementById('dvnewtag').style.display = 'none';
                    
                    let sindex = $scope.mytasklist.findIndex(m => m.id == c.id);
                    var s = $scope.mytasklist[sindex];
                    if (!s.tags) {
                        s.tags = [];
                    }
                var addNewIndex = $scope.mytasktaglist.findIndex(function (x) {
                    return x.id == -1;
                });
                var newTag = {
                    id: res.data.d,
                    tagname: notes.value,
                    tagcolor: notescolor.value
                };

                if (addNewIndex > -1) {
                    $scope.mytasktaglist.splice(addNewIndex, 0, newTag);
                } else {
                    $scope.mytasktaglist.push(newTag);
                }
 
                    s.tags.push({
                        id: res.data.d,
                        tagname: notes.value,
                        tagcolor: notescolor.value
                    }); 
                     $scope.selectedtagIds.push({
                    id:res.data.d,
                    name:notes.value
                    });
                    notes.value = ""
                    notescolor.value = ""
                    $scope.getSelectedTags();
                    //document.getElementById('btnclstags').click();
                    $scope.$applyAsync();
                }
                //document.getElementById('pnlimgstop').style.display = 'none';
                //document.getElementById('divstop').style.display = 'block';


            })
            .catch(function (err) {

                //console.error("Update failed", err);
                alert("Update failed");
                //document.getElementById('pnlimgstop').style.display = 'none';
                //document.getElementById('divstop').style.display = 'block';
            });
$scope.filteredTagList();
    }
$scope.SvaeadTags_Multi = function ()
    {
alert('ok');
        debugger;
        //alert($scope.tagtype);
        if ($scope.multiselectids.length==0){
            return;
        }
       
var new_tagname="";     
var new_tagcolor="";     
if($scope.addnew_tag=="1")
{  
        var notes = document.getElementById('ContentPlaceHolder1_tags2');
        if (!notes) { return; }
        if (notes.value == "") {
            alert("Please enter tagname");
            notes.focus();
            return;
        }

        var notescolor = document.getElementById('tagscolor2');
        if (!notescolor) { return; }
        if (notescolor.value == "") {
            alert("Please enter tag color");
            notescolor.focus();
            return;
        }
          usertaskService.SaveTagsNew(notes.value, notescolor.value)
            .then(function (res) {
                //if (!res) {
                    //alert("Failed");
                    //return;
                //}
                if (res.data.d!= "0"&& res.data.d!= "") {
                    debugger                    
                var addNewIndex = $scope.mytasktaglist.findIndex(function (x) {
                    return x.id == -1;
                });
                var newTag = {
                    id: res.data.d,
                    tagname: notes.value,
                    tagcolor: notescolor.value
                };

                if (addNewIndex > -1) {
                    $scope.mytasktaglist.splice(addNewIndex, 0, newTag);
                } else {
                    $scope.mytasktaglist.push(newTag);
                } 
                     $scope.selectedtagIds.push({
                    id:res.data.d,
                    name:notes.value
                    });
                    notes.value = ""
                    notescolor.value = ""
                    $scope.$applyAsync();
                }
               
            })
            .catch(function (err) {

            });

  }
 if ($scope.selectedtagIds.length==0){
            return;
        } 
        var ids = $scope.multiselectids.map(function(x) {return x.id + ":" + x.type;}).join(",");
        var tagids = $scope.selectedtagIds.map(function(x) {return x.id;}).join(",");
        usertaskService.SaveTags_update_Multi(ids,tagids)
            .then(function (res) {
debugger
                if (!res) {
                    alert("Failed");
                    return;
                }
                if (res.data.d!= "0"&& res.data.d!= "") {
                    debugger
                    //alert("save successfully");
angular.forEach($scope.multiselectids, function (t) {

    if ($scope.selectedtagIds.length > 0) {

        var tindex = $scope.mytasklist.findIndex(function (x) {
            return x.id == t.id && x.tasktype == t.type;
        });

        if (tindex > -1) {

            var task = $scope.mytasklist[tindex];

            var selectedTagListnew = $scope.mytasktaglist.filter(function (tag) {
                return $scope.selectedtagIds.some(function (sel) {
                    return sel.id == tag.id;
                });
            });

            // clear existing tags
            task.tags.length = 0;

            // push one by one
            angular.forEach(selectedTagListnew, function (tag) {
                task.tags.push(angular.copy(tag));
            });

            console.log(task.tags);
        }
    }
});

try
{
angular.forEach($scope.mytasklist, function (c) {
debugger
            c.selected = false;
});
$scope.multiselect = "0";
var checkall=document.getElementById('chkAll');
if(checkall)
checkall.checked=false;
}
catch{}
                    document.getElementById('dvnewtag2').style.display = 'none';
                    document.getElementById('btnclstags2').click();
                    $scope.$applyAsync();
                }
                //document.getElementById('pnlimgstop').style.display = 'none';
                //document.getElementById('divstop').style.display = 'block';


            })
            .catch(function (err) {
                //console.error("Update failed", err);
                alert("Update failed");
                //document.getElementById('pnlimgstop').style.display = 'none';
                //document.getElementById('divstop').style.display = 'block';
            });

    }

$scope.tagSearch = "";

//$scope.loadsubtasklist_count();
//$scope.loadsubtasklist();
$q.all([
$scope.loadstatuslist(),
$scope.loadprioritylist(),
$scope.loaduserlist(),
$scope.loadtaglist()

]).then(function () {
$scope.filteredTagList = angular.copy($scope.mytasktaglist);
    $scope.loadsubtasklist();
});

$scope.filterstatuslist_count = function () {
debugger
    if ($scope.statuslists && $scope.statuslists.length > 0) {

        $scope.mytasklist_bystatus = [];

        angular.forEach($scope.statuslists, function (status) {

            var total = $scope.mytasklist.filter(function (t) {
                return t.P_Status == status.id;
            }).length;

            if (total > 0) {
                $scope.mytasklist_bystatus.push({
                    statusid: status.id,
                    statusname: status.name,
                    statuscolor: status.colorname,
                    statusicon: status.statusicon,
                    total: total
                });
            }

        });
debugger
        console.log($scope.mytasklist_bystatus);
    }
};
//// end scopes
});

//function inittooltip() {

//    document.querySelectorAll('[data-bs-toggle="tooltip"]').forEach(function (el) {

//        let instance = bootstrap.Tooltip.getInstance(el);
//        let newTitle = el.getAttribute('data-bs-title');

//        if (instance) {
//            instance.setContent({ '.tooltip-inner': newTitle });
//        } else {
//            new bootstrap.Tooltip(el);
//        }

//    });


//}
function initTableUI() {
    debugger
    //inittooltip();
    initProjectTableColumnDragDrop();
}
function getProjectTableElement() {
    var ctrltbl= document.querySelector('.project-table');
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
    debugger
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
        debugger
        //var ctrltbl1 = document.querySelector('#tbDetails');
        setTimeout(function () {
            debugger
            var ctrltbl1 = document.querySelector('#tbDetails');
            var ctrltbl2 = document.querySelector('.project-table');
           // console.log(ctrltbl1);

        }, 500);
        
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
        //alert("ok");
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






