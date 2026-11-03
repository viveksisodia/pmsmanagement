



app.controller("mprojectCtrl", function ($scope, $http, $timeout,$document,mprojectService, projectState, $compile) {
    // validate time
    alert('call mprojectCtrl');

    $scope.normalizeTime = function (val) {

        if (!val) return "";
        val = val.toString().replace(/[^0-9]/g, ""); // remove : if exists
        //if (val.length === 3) {
            //val = "0" + val;
        //}
        //if (val.length !== 4) return val;
        //return val.substring(0, 2) + ":" + val.substring(2, 4);
        val = val.substring(val.length - 2, val.length);
        return val;
    };
    $scope.normalizeTime_calhr = function (val) {

        if (!val) return 0;
        val = val.toString().replace(/[^0-9]/g, "");       
        let  val_hr = val.substring(0,val.length - 2);
        let  val_min = val.substring(val.length - 2, val.length);
        var ret = (parseInt(val_hr) * 60) + parseInt(val_min) ;
        return ret;
    };
    $scope.validateTime = function (c,index,ctype) {
        debugger
        if (!c || !c.TaskDuration) return;
        //val = $scope.normalizeTime(c.TaskDuration);
        var min = $scope.normalizeTime(c.TaskDuration);

        if (min >= 60) {
            alert("Minutes must be less than 60");
            c.TaskDuration = "00:00";
            setTimeout(function () {
                var el = document.querySelectorAll("input[ng-model='c.TaskDuration']")[index];
                if (ctype == "task")
                    el = document.querySelectorAll("input[ng-model='c1.TaskDuration']")[index];
                if (ctype == "subtask")
                    el = document.querySelectorAll("input[ng-model='c2.TaskDuration']")[index];
                if (el) el.focus();
            }, 0);
            return false;
        }
        return true;
    };
    $scope.validateTime_actual = function (c) {
        debugger
        if (!c || !c.TaskDuration) return;
        val = $scope.normalizeTime(c.TaskDuration);
        
        var match = val.match(/^([01]?\d|2[0-3]):([0-5]\d)$/);

        if (!match) {
            alert("Invalid time. Use HH:MM format");
            c.TaskDuration = "";
            return false;
        }

        var min = parseInt(match[2], 10);

        if (min >= 60) {
            alert("Minutes must be less than 60");
            c.TaskDuration = "";
            return false;
        }

        // normalize (keep HH:MM only)
        c.TaskDuration =
            match[1].padStart(2, "0") + ":" +
            match[2].padStart(2, "0");

        return true;
    };

    $scope.formatDuration = function (val) {

        if (val == null || val === "") return "";

        // if number (hours)
        if (!isNaN(val)) {
            var h = parseInt(val, 10);
            return (h < 10 ? "0" + h : h) + ":00";
        }

        return val; // already formatted
    };
    $scope.projectState = projectState;
    $scope.isload = "0";

    $scope.view = 'table';
    $scope.ganttScale = "day";
    $scope.allGanttRows = [];
    $scope.ganttRows = [];
    $scope.ganttCells = [];
    $scope.ganttHeaderGroups = [];
    $scope.ganttCellWidth = 40;
    $scope.ganttStart = null;
    $scope.ganttEnd = null;
//#region add variables
    $scope.currentIndex =0;
    $scope.currentlist =[];
    $scope.currentUserType = "";
    $scope.currentFileType = "";
    $scope.currentFProjectId ="";
    $scope.currentFMilestoneId ="";
    $scope.currentFTaskId ="";
    $scope.currentFId ="";
   
    
    $scope.currentownerslist =[];

$scope.owners = [];
$scope.ownersnew ={
id:"",
name:"",
profileimage:"",
shortname:""
};

$scope.selectedOwnerIds = [];
$scope.searchOwner = "";
//#endregion
//#region mange popup


$scope.loadform=function loadsrc(c,utype,seearchtype, val1, val2,val3) {
debugger
    $scope.currentlist =c;
    $scope.currentUserType = seearchtype;
    $scope.currentFileType = utype;
        count11++;        
        
            $scope.modalTitle = "Manage "+val3;    
            document.getElementById(val1).src="";
        if (val2.includes("aspx?")) {
            document.getElementById(val1).src = val2 + "&ctype=ang&count=" + count11;
        }
        else
        {

            document.getElementById(val1).src = val2 + "?ctype=ang&count=" + count11;
        }
        try {

            //document.getElementById("").innerHTML = val3;
        }
        catch (ex) {
        }
    };
$scope.loadformedit = function (c, utype, frameId, url, title) {

    debugger;
    document.getElementById("hdneditid").value="0";
    $scope.currentlist = c;
    $scope.currentFileType = utype;
    // safe counter
    $scope.popupCounter = ($scope.popupCounter || 0) + 1;

    // modal title
    $scope.modalTitle = "Edit " + title;

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
//#endregion
 
//#region manage date format
function convertDotNetDate(dateString) {
    // dateString example: "/Date(1679788800000)/"
    var matches = /\/Date\((\-?\d+)(?:[+-]\d+)?\)\//.exec(dateString);
    if (!matches) return null; // invalid format

   var timestamp = parseInt(matches[1], 10);
    var date = new Date(timestamp);

    var day = ('0' + date.getDate()).slice(-2);
    var month = ('0' + (date.getMonth() + 1)).slice(-2);
    var year = date.getFullYear();

    return day + '-' + month + '-' + year;
}
//#endregion


function parseDotNetDate(dotNetDateString) {
    if (!dotNetDateString) return null;

    // Example input: "/Date(1680758400000)/"
    var timestamp = dotNetDateString.match(/\d+/); // extract number
    if (timestamp && timestamp.length > 0) {
        return new Date(parseInt(timestamp[0], 10));
    }
    return null;
}
$scope.isnumeric = function (event) {
    let charCode = event.which || event.keyCode;

    // Allow: backspace, tab, enter, arrows, delete
    if (
        charCode === 8 ||   // backspace
        charCode === 9 ||   // tab
        charCode === 13 ||  // enter
        charCode === 46 ||  // delete / dot
        (charCode >= 35 && charCode <= 40) // arrows
    ) {
        return true;
    }

    // Allow digits (0-9)
    if (charCode >= 48 && charCode <= 57) {
        return true;
    }

    // Block everything else
    event.preventDefault();
    return false;
};
     //comman function
    function getHashParams() {
debugger
        //let hash = window.location.hash || "";

        //if (hash.startsWith("#")) {
           // hash = hash.substring(1);
        //}
        //if (hash.startsWith("#")) {
          //  hash = hash.substring(1);
       // }

        //return new URLSearchParams(hash);
let params = new URLSearchParams(window.location.search);
        return new URLSearchParams(params);
    }

    function setCustomHash(value) {
        window.location.hash = "##" + value;
    } 


    $scope.showlist = true;
       
   
            $timeout(function () {
                $scope.loadprojectdata();
                $scope.showlist = false;
                $scope.showAddForm = true;
            }, 0);
       
    $scope.highlightMatch = function (text, search) {
        if (!search || search.trim() === "") return text;
        let pattern = new RegExp("(" + search + ")", "gi");
        return text.replace(pattern, "<b>$1</b>");
    };



    
  $scope.loadprojectdata = function () {

      debugger
      alert("ok");
    $scope.isLoading = true;

      mprojectService.getprojectlist().then(function (response) {

        debugger;

        $scope.projectlist = response.data.d;
$scope.isload="1";
        angular.forEach($scope.projectlist.miles, function (c) {
            c.isExpanded = false;
            if (c.StartDate) {
                if (typeof c.StartDate === 'string' && c.StartDate.indexOf('/Date(') === 0) {
                    var timestamp = parseInt(c.StartDate.substr(6));
                    c.StartDate = new Date(timestamp);
                } else {
                    c.StartDate = new Date(c.StartDate);
                }
            }
 if (c.EndDate) {
                if (typeof c.EndDate === 'string' && c.EndDate.indexOf('/Date(') === 0) {
                    var timestamp = parseInt(c.EndDate.substr(6));
                    c.EndDate = new Date(timestamp);
                } else {
                    c.EndDate = new Date(c.EndDate);
                }
            }
            try {
                //c.TaskDuration = $scope.formatDuration(c.TaskDuration);
                c.TaskDuration = c.TaskDuration;
            }
            catch{}
            
            // TASKS
            if (c.tasks && c.tasks.length > 0) {

                angular.forEach(c.tasks, function (t) {

                    t.isExpanded = false;

                    if (t.Startdate) {
                        if (typeof t.Startdate === 'string' && t.Startdate.indexOf('/Date(') === 0) {
                            var timestamp = parseInt(t.Startdate.substr(6));
                            t.Startdate = new Date(timestamp);
                        } else {
                            t.Startdate = new Date(t.Startdate);
                        }
                    }
 if (t.Targetdate) {
                        if (typeof t.Targetdate === 'string' && t.Targetdate.indexOf('/Date(') === 0) {
                            var timestamp = parseInt(t.Targetdate.substr(6));
                            t.Targetdate = new Date(timestamp);
                        } else {
                            t.Targetdate = new Date(t.Targetdate);
                        }
                    }
                    try {
                        //t.TaskDuration = $scope.formatDuration(t.TaskDuration);
                        t.TaskDuration = t.TaskDuration;
                    }
                    catch{ }
                    // SUBTASKS (c2 FIX)
                    if (t.subtasks && t.subtasks.length > 0) {

                        angular.forEach(t.subtasks, function (s) {
                            

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
                            try {
                                //s.TaskDuration = $scope.formatDuration(s.TaskDuration);
                                s.TaskDuration = s.TaskDuration;
                            }
                            catch{ }
                        });

                    }

                });

            }

        });
setTimeout(initTableUI, 300);

    }).catch(function (error) {

        alert("HTTP ERROR: " + (error.statusText || error.message || "Unknown error"));
        console.error("HTTP ERROR:", error);

    }).finally(function () {

       
    $scope.isLoading = false;
    

    });
      $timeout(function () {
          //$scope.buildGantt();
          //alert($scope.ganttScale);
          debugger;
          $scope.changeGanttScale($scope.ganttScale);
      }, 0);
};

  

//// end scopes
});





function initTableUI() {
    // Initialize Bootstrap tooltips
    document.querySelectorAll('[data-bs-toggle="tooltip"]').forEach(function(el){
        if (bootstrap.Tooltip.getInstance(el)) {
            bootstrap.Tooltip.getInstance(el).dispose();
        }
        new bootstrap.Tooltip(el);
    });

    // Initialize or refresh DataTable if you are using it
try
{
    let tableEl = document.querySelector('.datatable'); // your table class
    if (tableEl) {
        if ($.fn.DataTable.isDataTable(tableEl)) {
            $(tableEl).DataTable().destroy();  // destroy old instance
        }
        $(tableEl).DataTable({
            scrollY: '300px',     // vertical scroll
            scrollCollapse: true,
            paging: false,        // remove paging
            searching: true,
            info: false,
        });
    }
}
catch
{}

    initProjectTableColumnDragDrop();
}




