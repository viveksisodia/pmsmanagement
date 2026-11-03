




app.directive('ngDragstart', function ($parse) {
    return function (scope, element, attrs) {
        var fn = $parse(attrs.ngDragstart);
        element.on('dragstart', function (event) {
            scope.$applyAsync(function () { fn(scope, { $event: event.originalEvent || event }); });
        });
    };
});
app.directive('ngDragover', function ($parse) {
    return function (scope, element, attrs) {
        var fn = $parse(attrs.ngDragover);
        element.on('dragover', function (event) {
            scope.$applyAsync(function () { fn(scope, { $event: event.originalEvent || event }); });
        });
    };
});
app.directive('ngDragleave', function ($parse) {
    return function (scope, element, attrs) {
        var fn = $parse(attrs.ngDragleave);
        element.on('dragleave', function (event) {
            scope.$applyAsync(function () { fn(scope, { $event: event.originalEvent || event }); });
        });
    };
});
app.directive('ngDrop', function ($parse) {
    return function (scope, element, attrs) {
        var fn = $parse(attrs.ngDrop);
        element.on('drop', function (event) {
            scope.$applyAsync(function () { fn(scope, { $event: event.originalEvent || event }); });
        });
    };
});
app.directive('ngDragend', function ($parse) {
    return function (scope, element, attrs) {
        var fn = $parse(attrs.ngDragend);
        element.on('dragend', function (event) {
            scope.$applyAsync(function () { fn(scope, { $event: event.originalEvent || event }); });
        });
    };
});

app.controller("projectCtrl", function ($scope, $http, $timeout,$document,projectService, projectState, $compile) {
    //#region add variables
    $scope.projectState = projectState;
    $scope.isload = "0";
    $scope.projectmasterlist = [];
    $scope.view = 'table';
    $scope.ganttScale = "day";
    $scope.allGanttRows = [];
    $scope.ganttRows = [];
    $scope.ganttCells = [];
    $scope.ganttHeaderGroups = [];
    $scope.ganttCellWidth = 40;
    $scope.ganttStart = null;
    $scope.ganttEnd = null;
    $scope.currentIndex = 0;
    $scope.currentlist = [];
    $scope.currentUserType = "";
    $scope.currentFileType = "";
    $scope.currentFProjectId = "";
    $scope.currentFMilestoneId = "";
    $scope.currentFTaskId = "";
    $scope.currentFId = "";


    $scope.currentownerslist = [];

    $scope.owners = [];
    $scope.ownersnew = {
        id: "",
        name: "",
        profileimage: "",
        shortname: ""
    };

    $scope.selectedOwnerIds = [];
    $scope.searchOwner = "";
    $scope.currentPage = 1;
    $scope.searchText = "";
    $scope.sortColumn = "id";
    $scope.reverseSort = false;
    $scope.pageSizes = [10, 20, 50, 100];
    $scope.types = ["1", "2"];
    $scope.pageSize = 10;
    $scope.ProjectName = "";
    //End comman function
    $scope.statuslists = [];
    $scope.prioritylists = [];
    $scope.projectlist = [];
    $scope.milestonetasklist = [];
    $scope.showfileslist = [];
    $scope.currentprojectid = "0";
    $scope.selectedprojectid = "";
    $scope.selectedprojectname = "";
//#endregion

//#region Add utility
    $scope.formatDateToDDMMYYYY = function (date) {
        if (!date) return '';
        var d = new Date(date);
        var day = ('0' + d.getDate()).slice(-2);
        var month = ('0' + (d.getMonth() + 1)).slice(-2);
        var year = d.getFullYear();
        return day + '/' + month + '/' + year;
    };
    $scope.confirmCopy = function () {
        Swal.fire({
            title: "Copy?",
            text: "Are you sure you want to copy this project data?",
            icon: "warning",
            showCancelButton: true,
            confirmButtonText: "Yes, Copy",
            cancelButtonText: "Cancel"
        }).then((result) => {
            if (result.isConfirmed) {
                $scope.savecopy();
            }
        });
    };
    $scope.formatDuration = function (c2) {

        let v = (c2.TaskDuration || '').replace(/\D/g, ''); // only digits

        // before = all digits except last 2
        let before = v.slice(0, -2);
        let after = v.slice(-2);

        if (v.length === 0) {
            c2.TaskDuration = '/';
        }
        else if (v.length <= 2) {
            c2.TaskDuration = v + '/';
        }
        else {
            c2.TaskDuration = before + '/' + after;
        }
    };
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

    $scope.normalizeTime_calhr_new = function (val1, val2) {

        
        
        let val_hr = (val1 == "" ? "0" : val1);        
        let val_min = (val2 == "" ? "0" : val2);      
       
        var ret = (parseInt(val_hr) * 60) + parseInt(val_min);
        return ret;
    };
    $scope.validateTime = function (c, index, ctype) {
        debugger
        if (!c || !c.TaskDuration_min) return;
        //val = $scope.normalizeTime(c.TaskDuration);
        var min = c.TaskDuration_min;

        if (min >= 60) {
            alert("Minutes must be less than 60");
            c.TaskDuration_min = "";
            setTimeout(function () {
                var el = document.querySelectorAll("input[ng-model='c.TaskDuration_min']")[index];
                if (ctype == "task")
                    el = document.querySelectorAll("input[ng-model='c1.TaskDuration_min']")[index];
                if (ctype == "subtask")
                    el = document.querySelectorAll("input[ng-model='c2.TaskDuration_min']")[index];
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
    $scope.highlightMatch = function (text, search) {
        if (!search || search.trim() === "") return text;
        let pattern = new RegExp("(" + search + ")", "gi");
        return text.replace(pattern, "<b>$1</b>");
    };
    $scope.initTableDrag11 = function () {
        debugger
        alert("ok");
        if ($scope.view == "table") {

            initProjectTableColumnDragDrop();
        }
    };
//#endregion
document.addEventListener('click', function () {

    var scope = angular.element(document.body).scope();

    if (!scope) return;

    scope.$applyAsync(function () {

        angular.forEach(scope.ganttRows || [], function (r) {

            r.showActionMenu = false;

        });

    });

});
$scope.openDescriptionPopup = function ($event, c, colname, utype) {

    let rect = $event.target.getBoundingClientRect();

    Swal.fire({

        input: 'textarea',

        inputValue:
            c.miledescription ||
            c.taskdescripton ||
            c.SubTaskDescription ||
            '',

        showConfirmButton: false,
        showCancelButton: false,

        backdrop: false,

        width: '350px',

        customClass: {
            popup: 'small-desc-popup'
        },

        didOpen: function () {

            let popup = Swal.getPopup();

            // POSITION NEAR INPUT
            popup.style.position = 'fixed';

            popup.style.margin = '0';

            popup.style.left = rect.left + 'px';

            popup.style.top =
                (rect.bottom + 5) + 'px';

            const textarea = Swal.getInput();

            textarea.focus();

            function saveData() {

                var val = textarea.value || '';

                if (utype == "milestone") {

                    c.miledescription = val;

                    $scope.updatedetails_mile(c, colname);
                }

                else if (utype == "task") {

                    c.taskdescripton = val;

                    $scope.updatedetails_task(c, colname);
                }

                else if (utype == "subtask") {

                    c.SubTaskDescription = val;

                    $scope.updatedetails_subtask(c, colname);
                }

                Swal.close();
            }

            textarea.addEventListener('keydown', function (e) {

                if (e.key === 'Tab') {

                    e.preventDefault();

                    saveData();
                }
            });

            textarea.addEventListener('blur', function () {

                saveData();
            });
        }
    });
};
$scope.openDescriptionPopup2 = function (c, colname, utype) {

    Swal.fire({

        input: 'textarea',

        inputValue:
            c.miledescription ||
            c.taskdescription ||
            c.SubTaskDescription ||
            '',

        inputPlaceholder: 'Enter description...',

        showConfirmButton: false,

        showCancelButton: false,

        width: '400px',

        padding: '10px',

        allowOutsideClick: true,

        customClass: {
            popup: 'small-desc-popup',
            title: 'small-desc-title',
            input: 'small-desc-input'
        },

        // ===== POPUP OPEN =====
        didOpen: function () {

            const textarea = Swal.getInput();

            textarea.focus();

            // ===== SAVE =====
            function saveData() {

                var val = textarea.value || '';

                // MILESTONE
                if (utype == "milestone") {

                    c.miledescription = val;

                    $scope.updatedetails_mile(c, colname);
                }

                // TASK
                else if (utype == "task") {

                    c.taskdescripton = val;

                    $scope.updatedetails_task(c, colname);
                }

                // SUBTASK
                else if (utype == "subtask") {

                    c.SubTaskDescription = val;

                    $scope.updatedetails_subtask(c, colname);
                }

                c.isDescPopupOpen = false;

                Swal.close();
            }

            // TAB / CTRL+ENTER SAVE
            textarea.addEventListener('keydown', function (e) {

                if (e.key === 'Tab') {

                    e.preventDefault();

                    saveData();
                }

                if (e.ctrlKey && e.key === 'Enter') {

                    e.preventDefault();

                    saveData();
                }
            });

            // OUTSIDE CLICK SAVE
            document.addEventListener(
                'click',
                outsideClickHandler,
                true
            );

            function outsideClickHandler(e) {

                if (!Swal.getPopup()) return;

                if (!Swal.getPopup().contains(e.target)) {

                    document.removeEventListener(
                        'click',
                        outsideClickHandler,
                        true
                    );

                    saveData();
                }
            }
        },

        // ===== POPUP CLOSE =====
        willClose: function () {

            c.isDescPopupOpen = false;
        }
    });
};
    $scope.openDescriptionPopupold = function (c,colname,utype) {
debugger
    Swal.fire({
        title: 'Description',
        input: 'textarea',
        inputValue: c.Description || '',
        inputPlaceholder: 'Enter description...',
        showCancelButton: true,
        confirmButtonText: 'Save',
        cancelButtonText: 'Close',
        width: '400px',
        padding: '10px',

        customClass: {
            popup: 'small-desc-popup',
            title: 'small-desc-title',
            input: 'small-desc-input',
            confirmButton: 'small-desc-btn',
            cancelButton: 'small-desc-btn'
        },

        inputValidator: (value) => {

            if (!value) {
                return 'Please enter description';
            }
        }

    }).then((result) => {

        if (result.isConfirmed) {

            // UPDATE UI
        if(utype=="milestone")
            {
             c.miledescription = result.value;            
             $scope.updatedetails_mile(c,colname);
            }

            Swal.fire({
                icon: 'success',
                title: 'Saved',
                timer: 1000,
                showConfirmButton: false
            });
        }
    });
};


//#region manage projectmaster list
    $scope.selectedprojectid = "0";
    $scope.selectedprojectname = "";
    $scope.selectProject = function (p) {
        $scope.selectedprojectid = "0";
        $scope.selectedprojectname = "";
        $scope.selectedprojectid = p.id;
        $scope.selectedprojectname = p.ProjectName;
    };
    $scope.getProjectName = function () {
        if (!$scope.projectmasterlist) return '';

        var proj = $scope.projectmasterlist.find(function (p) {
            return String(p.id) === String($scope.selectedprojectid);
        });

        return proj ? proj.ProjectName : '';
    };
    $scope.focusProjectSearch = function ($event) {
        setTimeout(function () {
            var dropdown = $event.target.closest(".dropdown");
            if (!dropdown) return;

            var input = dropdown.querySelector(".project-search");
            if (input) input.focus();
        }, 200);
    };
    $scope.loadprojectlist = function (cpid) {
        
      debugger
        projectService.getprojectmasterlist(cpid).then(function (response) {
            var data = response.data.d;
            if (typeof data === "string") {
                data = JSON.parse(data);
            }
            $scope.projectmasterlist = data || [];

        }, function (error) {
            console.error(error);
            alert("Error: " + error.status);
        });

    };
    $scope.loadprojectlist_reset = function (cpid) {
        //alert(cpid);
        //return;
        debugger
       
        projectService.getprevious_projectdetails(cpid).then(function (response) {
            var data = response.data.d;
            if (typeof data === "string") {
                data = JSON.parse(data);
            }
            $scope.projectlist = [];
            $scope.projectlist = data || [];
            
            $scope.isload = "1";
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
                catch{ }

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
            debugger
            
            setTimeout(initTableUI, 300);

        }, function (error) {
            console.error(error);
            alert("Error: " + error.status);
        });
        
    };
    

    $scope.selectProject = function (p) {
        $scope.selectedprojectid = p.id;
        $scope.selectedprojectname = p.ProjectName;
    };
    $scope.savecopy = function () {
        debugger
        
        if (!$scope.selectedprojectid || $scope.selectedprojectid == 0) {
            alert("Please select a project");
            return;
        }
        $scope.currentprojectid = $scope.projectlist.id;
        if (!$scope.currentprojectid || $scope.currentprojectid == 0) {
            alert("Invalid current project");
            return;
        }
        //if (!confirm("Are you sure you want to copy this project data?")) {
         //   return; // stop if user clicks Cancel
        //}
        
        projectService.copyProject($scope.currentprojectid, $scope.selectedprojectid)
            .then(function (res) {

                var data = res.data.d;
                if ($scope.currentprojectid) {
                    $timeout(function () {
                        $scope.loadprojectbyid($scope.currentprojectid);
                        $scope.showlist = false;
                        $scope.showAddForm = true;
                    }, 0);
                } 
                var modalEl = document.getElementById('addcopy');
                var modal = bootstrap.Modal.getInstance(modalEl);
                if (modal) modal.hide();
                //alert("Copied successfully");


            })
            .catch(function (err) {
                console.error(err);
                alert("Error occurred while copying");
            });
    };
   
//#endregion


//#region mange popup

$scope.closemile = function () {
debugger;
//alert('ok');
    
document.getElementById('btnclsmile').click();
if(document.getElementById("hdneditid").value=="1")
{
    if (!$scope.currentlist) return;
    let obj = $scope.currentlist;
    let utype = $scope.currentFileType;  
    let searchtype=$scope.currentUserType;
    let resultnew = null;
    // API call based on type
    var mid="0";
        resultnew = projectService.getitemdetails(obj.id, utype);
    
if (!resultnew) return;

      resultnew.then(function (res) {
        debugger;
        let getlist = res.data.d;
        if (getlist) {
            if (utype === 'milestone') {
mid=obj.id;
            if (getlist.StartDate) {
                if (typeof getlist.StartDate === 'string' && getlist.StartDate.indexOf('/Date(') === 0) {
                    var timestamp = parseInt(getlist.StartDate.substr(6));
                    getlist.StartDate = new Date(timestamp);
                } else {
                    getlist.StartDate = new Date(getlist.StartDate);
                }
            }
        if (getlist.EndDate) {
                if (typeof getlist.EndDate === 'string' && getlist.EndDate.indexOf('/Date(') === 0) {
                    var timestamp = parseInt(getlist.EndDate.substr(6));
                    getlist.EndDate = new Date(timestamp);
                } else {
                    getlist.EndDate = new Date(getlist.EndDate);
                }
            }
            let mindex = $scope.projectlist.miles.findIndex(m => m.id == obj.id);
            let ulist = $scope.projectlist.miles[mindex];
            ulist.Milestone=getlist.Milestone;                
            ulist.Weightage=getlist.Weightage;                
            ulist.StartDate=getlist.StartDate;                
            ulist.EndDate=getlist.EndDate;                
            ulist.Owner=getlist.Owner;                
            ulist.Access=getlist.Access;                
            ulist.team=getlist.team;                
            ulist.P_Status=getlist.P_Status;                
            ulist.status_name=getlist.status_name;                
            ulist.status_color=getlist.status_color;                
            ulist.Priority=getlist.Priority;                
            ulist.priority_name=getlist.priority_name;                
            ulist.priority_color=getlist.priority_color;                
            ulist.Budget=getlist.Budget;                
            ulist.Actual=getlist.Actual;                
            ulist.uid=getlist.uid;                
            ulist.cdate=getlist.cdate; 
            ulist.statuscomplete=getlist.statuscomplete; 
            ulist.istask=getlist.istask; 
            ulist.checkmileWeightage=getlist.checkmileWeightage; 
            ulist.iscomplete=getlist.iscomplete; 
            ulist.iscompletedate=getlist.iscompletedate; 
            ulist.iscompletepercentage=getlist.iscompletepercentage; 
            ulist.Durationtype=getlist.Durationtype; 
            ulist.TaskDuration=getlist.TaskDuration; 
            ulist.Durationhourcal=getlist.Durationhourcal; 
            ulist.orderno=getlist.orderno; 
            ulist.miledescription=getlist.miledescription; 
            ulist.Uploaddrawing=getlist.Uploaddrawing; 
            ulist.txtdays=getlist.txtdays; 
            ulist.complete_value=getlist.complete_value; 
            ulist.pending_value=getlist.pending_value; 
            ulist.taskcount=getlist.taskcount; 
            ulist.leftdays=getlist.leftdays; 
            ulist.owners=getlist.owners; 
            ulist.teams=getlist.teams; 
            ulist.viewfiles=getlist.viewfiles; 
            ulist.tasks=getlist.tasks;
            }
            else if (utype === 'task') {
mid=obj.Milestoneid;
if (getlist.Startdate) {
                if (typeof getlist.Startdate === 'string' && getlist.Startdate.indexOf('/Date(') === 0) {
                    var timestamp = parseInt(getlist.Startdate.substr(6));
                    getlist.Startdate = new Date(timestamp);
                } else {
                    getlist.Startdate = new Date(getlist.Startdate);
                }
            }
        if (getlist.Targetdate) {
                if (typeof getlist.Targetdate === 'string' && getlist.Targetdate.indexOf('/Date(') === 0) {
                    var timestamp = parseInt(getlist.Targetdate.substr(6));
                    getlist.Targetdate = new Date(timestamp);
                } else {
                    getlist.Targetdate = new Date(getlist.Targetdate);
                }
            }

            let mindex = $scope.projectlist.miles.findIndex(m => m.id == obj.Milestoneid);
            let tindex = $scope.projectlist.miles[mindex].tasks.findIndex(m => m.id == obj.id);
            let ulist = $scope.projectlist.miles[mindex].tasks[tindex];
            ulist.taskname=getlist.taskname;                
            ulist.taskdescripton=getlist.taskdescripton;                
            ulist.owner=getlist.owner;                
            ulist.team=getlist.team;                
            ulist.Startdate=getlist.Startdate;                
            ulist.Targetdate=getlist.Targetdate;                
            ulist.Weightage=getlist.Weightage;                
            ulist.StartTime=getlist.StartTime;                
            ulist.Timeline=getlist.Timeline;                
            ulist.TaskDuration=getlist.TaskDuration;                
            ulist.P_Status=getlist.P_Status;                
            ulist.status_name=getlist.status_name;                
            ulist.status_color=getlist.status_color;                
            ulist.Priority=getlist.Priority;                
            ulist.priority_name=getlist.priority_name;                
            ulist.priority_color=getlist.priority_color;                
            ulist.Budget=getlist.Budget; 
            ulist.Actual=getlist.Actual; 
            ulist.Reminder=getlist.Reminder; 
            ulist.ReminderType=getlist.ReminderType; 
            ulist.Days=getlist.Days; 
            ulist.Time=getlist.Time; 
            ulist.uid=getlist.uid; 
            ulist.cdate=getlist.cdate; 
            ulist.tagto=getlist.tagto; 
            ulist.dependancy=getlist.dependancy; 
            ulist.issubtask=getlist.issubtask; 
            ulist.checkTaskWeightage=getlist.checkTaskWeightage; 
            ulist.iscomplete=getlist.iscomplete; 
            ulist.iscompletepercentage=getlist.iscompletepercentage; 
            ulist.orderno=getlist.orderno; 
            ulist.txtdays=getlist.txtdays; 
            ulist.complete_value=getlist.complete_value; 
            ulist.pending_value=getlist.pending_value; 
            ulist.leftdays=getlist.leftdays; 
            ulist.owners=getlist.owners; 
            ulist.teams=getlist.teams; 
            ulist.viewfiles=getlist.viewfiles;
            ulist.subtasks=getlist.subtasks;
                
            }
            else if (utype === 'subtask') {
mid=obj.Milestoneid;
               if (getlist.Startdate) {
                if (typeof getlist.Startdate === 'string' && getlist.Startdate.indexOf('/Date(') === 0) {
                    var timestamp = parseInt(getlist.Startdate.substr(6));
                    getlist.Startdate = new Date(timestamp);
                } else {
                    getlist.Startdate = new Date(getlist.Startdate);
                }
            }
        if (getlist.Targetdate) {
                if (typeof getlist.Targetdate === 'string' && getlist.Targetdate.indexOf('/Date(') === 0) {
                    var timestamp = parseInt(getlist.Targetdate.substr(6));
                    getlist.Targetdate = new Date(timestamp);
                } else {
                    getlist.Targetdate = new Date(getlist.Targetdate);
                }
            }
mid=obj.Milestoneid;
            let mindex = $scope.projectlist.miles.findIndex(m => m.id == obj.Milestoneid);
            let tindex = $scope.projectlist.miles[mindex].tasks.findIndex(m => m.id == obj.Task);
            let stindex = $scope.projectlist.miles[mindex].tasks[tindex].subtasks.findIndex(m => m.id == obj.id);
            let ulist = $scope.projectlist.miles[mindex].tasks[tindex].subtasks[stindex];
            ulist.Subtaskname=getlist.Subtaskname;                
            ulist.SubTaskDescription=getlist.SubTaskDescription;                
            ulist.owner=getlist.owner;                
            ulist.team=getlist.team;                
            ulist.Startdate=getlist.Startdate;                
            ulist.Targetdate=getlist.Targetdate;                
            ulist.Weightage=getlist.Weightage;                
            ulist.StartTime=getlist.StartTime;                
            ulist.Timeline=getlist.Timeline;                
            ulist.TaskDuration=getlist.TaskDuration;                
            ulist.P_Status=getlist.P_Status;                
            ulist.status_name=getlist.status_name;                
            ulist.status_color=getlist.status_color;                
            ulist.Priority=getlist.Priority;                
            ulist.priority_name=getlist.priority_name;                
            ulist.priority_color=getlist.priority_color;                
            ulist.Budget=getlist.Budget; 
            ulist.Actual=getlist.Actual; 
            ulist.Reminder=getlist.Reminder; 
            ulist.ReminderType=getlist.ReminderType; 
            ulist.Days=getlist.Days; 
            ulist.Time=getlist.Time; 
            ulist.uid=getlist.uid; 
            ulist.cdate=getlist.cdate; 
            ulist.tagto=getlist.tagto; 
            ulist.dependancy=getlist.dependancy; 
            ulist.iscomplete=getlist.iscomplete; 
            ulist.iscompletepercentage=getlist.iscompletepercentage; 
            ulist.orderno=getlist.orderno; 
            ulist.txtdays=getlist.txtdays; 
            ulist.complete_value=getlist.complete_value; 
            ulist.pending_value=getlist.pending_value; 
            ulist.leftdays=getlist.leftdays; 
            ulist.owners=getlist.owners; 
            ulist.teams=getlist.teams; 
            ulist.viewfiles=getlist.viewfiles;
            

            } 
        }
if($scope.view=="gantt")
        {
            $scope.updateGanttCompletion(mid);       
         }

    })
    .catch(function (err) {
        console.error(err);
        alert("Update failed");
    })
    .finally(function () {
        
    });
 }
$scope.$applyAsync();
};
 $scope.closepopup_canvas_issue = function () {

    debugger;
    //window.parent.document.getElementById("btnclosemainpopup_issue").click();
    try
{
var btn = (window.parent && window.parent.document)
    ? window.parent.document.getElementById("btnclosemainpopup_issue")
    : document.getElementById("btnclosemainpopup_issue");
 if (btn==null) {
        btn = document.getElementById("btnclosemainpopup_issue");
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
    let utype = $scope.currentFileType;  
    let searchtype=$scope.currentUserType;
    let resultnew = null;
    // API call based on type
    if (utype === 'milestone') {
        resultnew = projectService.getissuesdetails(obj.id, utype);
    }
    else if (utype === 'task') {
        resultnew = projectService.getissuesdetails(obj.id, utype);
    }
    else if (utype === 'subtask') {
        resultnew = projectService.getissuesdetails(obj.id, utype);
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
            obj.completeissue_rate = getlist.completeissues_rate||0;
            obj.pendingissue_rate = getlist.pendingissues_rate||0;
        }

    })
    .catch(function (err) {
        console.error(err);
        alert("Update failed");
    })
    .finally(function () {
        
    });
};
$scope.closepopup_canvas_dependency = function () {

    debugger;
    try
{
var btn = (window.parent && window.parent.document)
    ? window.parent.document.getElementById("btnclosemainpopupD")
    : document.getElementById("btnclosemainpopupD");
 if (btn==null) {
        btn = document.getElementById("btnclosemainpopupD");
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
    let utype = $scope.currentFileType;  
    let searchtype=$scope.currentUserType;
    let resultnew = null;
    if (utype === 'milestone') {
        resultnew = projectService.getdependencydetails(obj.id, utype);
    }
    else if (utype === 'task') {
        resultnew = projectService.getdependencydetails(obj.id, utype);
    }
    else if (utype === 'subtask') {
        resultnew = projectService.getdependencydetails(obj.id, utype);
    }

    if (!resultnew) return;

      resultnew.then(function (res) {
        debugger;

        // res.data.d is expected as object
        let getD = res.data.d;
//alert(getD);
        obj.totaldependency=getD;

    })
    .catch(function (err) {
        console.error(err);
        alert("Update failed");
    })
    .finally(function () {
        
    });
  

};
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
$scope.loadformedit_gantt = function (c, utype, frameId, url, title) {

        debugger;
    if (utype == "milestone") {
        let mindex = $scope.projectlist.miles.findIndex(f => f.id === c.id);
        if (mindex !== -1) {
            let c1 = $scope.projectlist.miles[mindex];
            $scope.loadformedit(c1, utype, frameId, url, title);
        }
    }
    else if (utype == "task") {
        let mindex = $scope.projectlist.miles.findIndex(f => f.id === c.Milestoneid);

        let tindex = $scope.projectlist.miles[mindex].tasks.findIndex(f => f.id === c.id);
        if (tindex !== -1) {
            let c1 = $scope.projectlist.miles[mindex].tasks[tindex];
            $scope.loadformedit(c1, utype, frameId, url, title);
        }
    }
    else if (utype == "subtask") {
        let mindex = $scope.projectlist.miles.findIndex(f => f.id === c.Milestoneid);
        let tindex = $scope.projectlist.miles[mindex].tasks.findIndex(f => f.id === c.Task);
        if (tindex !== -1) {
            let sindex = $scope.projectlist.miles[mindex].tasks[tindex].subtasks.findIndex(f => f.id === c.id);
            if (sindex !== -1) {
                let c1 = $scope.projectlist.miles[mindex].tasks[tindex].subtasks[sindex];
                $scope.loadformedit(c1, utype, frameId, url, title);
            }
        }
    }

    };
//#endregion
  //#region mange owners and teams  


// Toggle selection on click
$scope.toggleOwner = function(owner) {
debugger
    var index = $scope.selectedOwnerIds.indexOf(owner.id);

    if(index > -1){
       // $scope.selectedOwnerIds.splice(index, 1); // deselect
    //if($scope.currentUserType=="team")
    //{
    //$scope.removeTeamToServer($scope.currentFileType,$scope.currentlist,$scope.selectedOwnerIds,owner.id);
     //   }
    //else
    //{
//$scope.removeOwnerToServer($scope.currentFileType,$scope.currentlist,$scope.selectedOwnerIds,owner.id);
    //}
} else {

        $scope.selectedOwnerIds.push(owner.id); // select
if($scope.currentUserType=="team")
    {
    $scope.updateTeamToServer($scope.currentFileType,$scope.currentlist,$scope.selectedOwnerIds,owner.id);
        }
    else
    {
        $scope.updateOwnerToServer($scope.currentFileType,$scope.currentlist,$scope.selectedOwnerIds,owner.id);
    }
}
initTableUI();
};

// Remove selected owner
$scope.removeOwner = function(ownerId) {
    var index = $scope.selectedOwnerIds.indexOf(ownerId);
    if(index > -1){
        $scope.selectedOwnerIds.splice(index, 1);
if($scope.currentUserType=="team")
    {
    $scope.removeTeamToServer($scope.currentFileType,$scope.currentlist,$scope.selectedOwnerIds,ownerId);
        }
    else
    {
        $scope.removeOwnerToServer($scope.currentFileType,$scope.currentlist,$scope.selectedOwnerIds,ownerId);
    }

    }
initTableUI();

};

// Get selected owner objects for displaying tags
$scope.getSelectedOwners = function() {
    return $scope.owners.filter(o => $scope.selectedOwnerIds.includes(o.id));
};

$scope.closelist = function() {
if($scope.showOwnerDropdown==true)
    $scope.showOwnerDropdown = false;
};
$scope.openlist = function() {
if($scope.showOwnerDropdown==false)
    $scope.showOwnerDropdown = true;
};

$scope.loadSrcOwners = function (c,addtype, strtype, index) {
debugger
    if(strtype=="milestone")
    {
    $scope.currentFMilestoneId=c.id;
    }
    else if(strtype=="task"||strtype=="subtask")
    {
        $scope.currentFMilestoneId=c.Milestoneid;
        if(strtype=="subtask")
        {
         $scope.currentFTaskId=c.Task;
        }
    }

    

    $scope.currentFileType = "";
    $scope.currentFProjectId ="";
    $scope.currentFId ="";
    $scope.currentUserType = "";


$scope.currentUserType = addtype;
    $scope.currentlist=c;
    $scope.currentIndex =index;    
    $scope.currentFileType = strtype;
    $scope.currentFProjectId = c.ProjectId;
    $scope.currentFId = c.id;

    switch (addtype) {
        case "owner":
            $scope.modalTitle = "Add owners";
            break;
        case "team":
            $scope.modalTitle = "Add teams";
            break;
        default:
            $scope.modalTitle = "Add owners";
    }
     $scope.selectedItem = c;
    $scope.selectedIndex = index;
    // Example: open modal flag
    $scope.showOwnerDropdown = false;
    $scope.selectedOwnerIds=[];
    if(addtype=="owner")
    {
       if(c.owners && c.owners.length > 0) {
       angular.forEach(c.owners, function(owner) {
            $scope.selectedOwnerIds.push(owner.id);;
        });
       }
    }
  else if(addtype=="team")
    {
       if(c.teams && c.teams.length > 0) {
       angular.forEach(c.teams, function(owner) {
            $scope.selectedOwnerIds.push(owner.id);;
        });
       }
    }
};
$scope.loadSrcOwnersnew = function (c, addtype, strtype, index) {
    debugger;

    // Determine reference ID based on type
    let refId;
    let reftype="";
    if(strtype === "milestone") {
        $scope.currentFMilestoneId = c.id;
        refId = c.ProjectId;
reftype="project";
        //$scope.loaduserlist_byrefid(refId, 'project', addtype);
    } else if(strtype === "task" || strtype === "subtask") {
        $scope.currentFMilestoneId = c.Milestoneid;
        if(strtype === "task") {
            refId = c.Milestoneid;
reftype="milestone";
            //$scope.loaduserlist_byrefid(refId, 'milestone', addtype);
        } else {
            $scope.currentFTaskId = c.Task;
            refId = c.Task;
reftype="task";
            //$scope.loaduserlist_byrefid(refId, 'task', addtype);
        }
    }
refId="0";
    $scope.loaduserlist_byrefid(refId, 'project', addtype);
    // Set scope variables
    $scope.currentUserType = addtype;
    $scope.currentFileType = strtype;
    $scope.currentFProjectId = c.ProjectId;
    $scope.currentFId = c.id;
    $scope.currentlist = c;
    $scope.currentIndex = index;

    $scope.modalTitle = addtype === "team" ? "Add teams" : "Add owners";
    $scope.selectedItem = c;
    $scope.selectedIndex = index;

    // Populate selected IDs
    $scope.selectedOwnerIds = [];
    if(addtype === "owner" && c.owners) {
        $scope.selectedOwnerIds = c.owners.map(o => o.id);
    } else if(addtype === "team" && c.teams) {
        $scope.selectedOwnerIds = c.teams.map(t => t.id);
    }
};

//#endregion
//#region update owner
$scope.updateOwnerToServer = function(utype, obj, selected_obj,newid) {
debugger
    if (!selected_obj || selected_obj.length === 0) {
        alert("No owners selected");
        return;
    }
    var ids = selected_obj.join(",");
    let resultnew;

    if (utype == 'milestone') {
        resultnew = projectService.UpdateMilestone_Owner(obj.id,ids);
    }
    else if (utype == 'task') {
        resultnew = projectService.UpdateTask_Owner(obj.id,ids);
    }
    else if (utype == 'subtask') {
        resultnew = projectService.UpdateSubtask_Owner(obj.id,ids);
    }
    if (resultnew) {
        resultnew.then(function (res) {
            debugger;
            if(res.data.d=="1"){
                var addlist=$scope.owners.find(m => m.id == newid);
                if(addlist){ 
                let newrow = angular.copy($scope.ownersnew);
                newrow.id = addlist.id;
                newrow.name = addlist.name;
                newrow.profileimage = addlist.profileimage;
                newrow.shortname = addlist.shortname;
                obj.owners.push(newrow);
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
//#endregion
//#region remove owner
$scope.removeOwnerToServer = function(utype, obj, selected_obj,newid) {
debugger
    //if (!selected_obj || selected_obj.length === 0) {
       // alert("No owners selected");
        //return;
    //}
    var ids = selected_obj.join(",");
    let resultnew;

    if (utype == 'milestone') {
        resultnew = projectService.UpdateMilestone_Owner(obj.id,ids);
    }
    else if (utype == 'task') {
        resultnew = projectService.UpdateTask_Owner(obj.id,ids);
    }
    else if (utype == 'subtask') {
        resultnew = projectService.UpdateSubtask_Owner(obj.id,ids);
    }
    if (resultnew) {
        resultnew.then(function (res) {
            debugger;
            if(res.data.d=="1"){
                var indx=obj.owners.findIndex(m => m.id == newid);
                if(indx!==-1){
                obj.owners.splice(indx,1);
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
//#endregion
//#region update team
$scope.updateTeamToServer = function(utype, obj, selected_obj,newid) {
debugger
    if (!selected_obj || selected_obj.length === 0) {
        alert("No owners selected");
        return;
    }
    var ids = selected_obj.join(",");
    let resultnew;

    if (utype == 'milestone') {
        resultnew = projectService.UpdateMilestone_Team(obj.id,ids);
    }
    else if (utype == 'task') {
        resultnew = projectService.UpdateTask_Team(obj.id,ids);
    }
    else if (utype == 'subtask') {
        resultnew = projectService.UpdateSubtask_Team(obj.id,ids);
    }
    if (resultnew) {
        resultnew.then(function (res) {
            debugger;
            if(res.data.d=="1"){
                var addlist=$scope.owners.find(m => m.id == newid);
                if(addlist){ 
                let newrow = angular.copy($scope.ownersnew);
                newrow.id = addlist.id;
                newrow.name = addlist.name;
                newrow.profileimage = addlist.profileimage;
                newrow.shortname = addlist.shortname;
                obj.teams.push(newrow);
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
//#endregion
//#region remove team
$scope.removeTeamToServer = function(utype, obj, selected_obj,newid) {
debugger
    //if (!selected_obj || selected_obj.length === 0) {
       // alert("No owners selected");
       // return;
    //}
    var ids = selected_obj.join(",");
    let resultnew;

    if (utype == 'milestone') {
        resultnew = projectService.UpdateMilestone_Team(obj.id,ids);
    }
    else if (utype == 'task') {
        resultnew = projectService.UpdateTask_Team(obj.id,ids);
    }
    else if (utype == 'subtask') {
        resultnew = projectService.UpdateSubtask_Team(obj.id,ids);
    }
    if (resultnew) {
        resultnew.then(function (res) {
            debugger;
            if(res.data.d=="1"){
                var indx=obj.teams.findIndex(m => m.id == newid);
                if(indx!==-1){
                obj.teams.splice(indx,1);
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

$scope.closepopupmainmulti=function closeproject()
{
window.parent.document.getElementById('btnclosemainpopupmulti').click();
}
$scope.getRemainingOwners = function (owners) {
    if (!owners || owners.length <= 3) return "";

    return owners.slice(3).map(function(o) {
        return o.name;
    }).join(", ");
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
       
    let params = getHashParams();
    if (params.get("id")) {
        let id = params.get("id");

        if (id) {
            $timeout(function () {
                $scope.loadprojectbyid(id);
                $scope.showlist = false;
                $scope.showAddForm = true;
            }, 0);
        } 
    }
    else {
        $scope.view = params.get("viewtype") || "table";
        $scope.setView = function (mode) {
            $scope.view = mode;
            setCustomHash("viewtype=" + mode);
        };
    }
    
   
   
     $timeout(function () {
                $scope.loadstatuslist();
                
            }, 0);
 $timeout(function () {
                $scope.loadprioritylist();
                
            }, 0);
$timeout(function () {
                $scope.loaduserlist();
                
            }, 0);



    
  $scope.loadprojectbyid = function (id) {


    $scope.isLoading = true;

    projectService.getprojectlistbyid(id).then(function (response) {

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

  $scope.loadstatuslist = function () {

    debugger;
    //$scope.isLoading = true;

    projectService.getstatuslist()
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
$scope.selectStatus_gantt = function (c, item, utype) {
    debugger
if(utype=="milestone")
    {
         let mindex = $scope.projectlist.miles.findIndex(f => f.id === c.id);
            if (mindex !== -1) {            
                let c1 = $scope.projectlist.miles[mindex];
                $scope.selectStatus(c1,item,utype);
            }
    }
else if(utype=="task")
    {
         let mindex = $scope.projectlist.miles.findIndex(f => f.id === c.Milestoneid);
            if (mindex !== -1) { 
          let tindex = $scope.projectlist.miles[mindex].tasks.findIndex(f => f.id === c.id);           
                let c1 = $scope.projectlist.miles[mindex].tasks[tindex];
                $scope.selectStatus(c1,item,utype);
            }
    }
else if(utype=="subtask")
    {
         let mindex = $scope.projectlist.miles.findIndex(f => f.id === c.Milestoneid);
            if (mindex !== -1) { 
            let tindex = $scope.projectlist.miles[mindex].tasks.findIndex(f => f.id === c.Task);
                if (tindex !== -1) { 
                let sindex = $scope.projectlist.miles[mindex].tasks[tindex].subtasks.findIndex(f => f.id === c.id);          
                let c1 = $scope.projectlist.miles[mindex].tasks[tindex].subtasks[sindex];
                $scope.selectStatus(c1,item,utype);
                }
            }
    }
}
$scope.selectStatus = function (c, item, utype) {

    debugger;

   
    var mid = "0";
    let resultnew;
    if (utype == 'milestone') {
        mid = c.id;
        resultnew = projectService.UpdateMilestone_Status(c.id, item.id, item.iscomplete);
    }
    else if (utype == 'task') {
        mid = c.Milestoneid;
        resultnew = projectService.UpdateTask_Status(c.id, item.id, item.iscomplete);
    }
    else if (utype == 'subtask') {
        mid = c.Milestoneid;
        resultnew = projectService.UpdateSubTask_Status(c.id, item.id, item.iscomplete);
    }

    if (resultnew) {
        resultnew.then(function (res) {
            debugger;
if(res.data.d!="1")
{
 alert(res.data.d);
}
else
{
c.status_color = item.colorname;
c.status_name = item.name;
    if ($scope.view == "gantt") {
        if (utype == 'milestone') {
            let mindex = $scope.projectlist.miles.findIndex(m => m.id == c.id);
            $scope.projectlist.miles[mindex].status_color = item.colorname;
            $scope.projectlist.miles[mindex].status_name = item.name;

        }
        else if (utype == 'task') {
            let mindex = $scope.projectlist.miles.findIndex(m => m.id == c.Milestoneid);
            var tindex = $scope.projectlist.miles[mindex].tasks.findIndex(m => m.id == c.id);
            $scope.projectlist.miles[mindex].tasks[tindex].status_color = item.colorname;
            $scope.projectlist.miles[mindex].tasks[tindex].status_name = item.name;
        }
        else if (utype == 'subtask') {
            let mindex = $scope.projectlist.miles.findIndex(m => m.id == c.Milestoneid);
            var tindex = $scope.projectlist.miles[mindex].tasks.findIndex(m => m.id == c.Task);
            let sindex = $scope.projectlist.miles[mindex].tasks[tindex].subtasks.findIndex(m => m.id == c.id);;
            $scope.projectlist.miles[mindex].tasks[tindex].subtasks[sindex].status_color = item.colorname;
            $scope.projectlist.miles[mindex].tasks[tindex].subtasks[sindex].status_name = item.name;

        }
        
    }
    
        if (mid != "0") {
            $scope.loadmilecompletiondetails(mid);
        }
   
     $scope.$applyAsync();
    
}
            //alert(res.d);
            //console.log(res);
        })
        .catch(function (err) {
            //console.error(err);
            alert("Update failed");
        });
    }

};


$scope.selectPriority = function (c, item,utype) {

    debugger;
let resultnew;
    // Update UI instantly
    c.priority_color = item.colorname;
    c.priority_name = item.name;

    if(utype=='milestone')
      resultnew=projectService.MileUpdatePriority_Status(c.id, item.id)
else if(utype=='task')
      resultnew=projectService.TaskUpdatePriority_Status(c.id,item.id)
else if(utype=='subtask')
      resultnew=projectService.SubtaskUpdatePriority_Status(c.id,item.id)
        resultnew.then(function (res) {
debugger
            console.log("Status updated");
        })
        .catch(function (err) {
debugger
            console.error("Update failed", err);
        });
};

 $scope.loadprioritylist = function () {

    debugger;
    //$scope.isLoading = true;

    projectService.getprioritylist()
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
    $scope.getSelectedOwnerId = function (row) {
       // debugger
        //alert(row ? (row.Owner || row.owner || null) : null);
        return row ? (row.Owner || row.owner || null) : null;
        

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
        //alert(owner ? owner.name : '')
        return owner ? owner.name : '';
    };
$scope.selectOwner_gantt = function (c, o,type) {
debugger

        
        c.Owner = o.id;
        var data = {
            id: c.id,
            ownerId:o.id,
            utype:type
        };

        $http({
            method: "POST",
            url: "/pms/projects/default.aspx/updateOwner",
            data: JSON.stringify(data),
            headers: {
                "Content-Type": "application/json; charset=utf-8"
            }
        }).then(function (res) {
            debugger
            if (res.data.d == "1") {
                if (type == "milestone") {
                    let mindex = $scope.projectlist.miles.findIndex(m => m.id == c.id);
                    $scope.projectlist.miles[mindex].Owner = o.id;                    
                }
                else if (type == "task") {                    
                    let mindex = $scope.projectlist.miles.findIndex(m => m.id == c.Milestoneid);
                    let tindex = $scope.projectlist.miles[mindex].tasks.findIndex(m => m.id == c.id);
                    $scope.projectlist.miles[mindex].tasks[tindex].owner = o.id;
                }
                else if (type == "subtask") {                    
                    let mindex = $scope.projectlist.miles.findIndex(m => m.id == c.Milestoneid);
                    let tindex = $scope.projectlist.miles[mindex].tasks.findIndex(m => m.id == c.Task);
                    let stindex = $scope.projectlist.miles[mindex].tasks[tindex].subtasks.findIndex(m => m.id == c.id);
                    $scope.projectlist.miles[mindex].tasks[tindex].subtasks[stindex].owner = o.id;
                }   
                //console.log("Owner updated successfully");
            } else {
                alert("Update failed");
            }

            }).catch(function (err) {
            debugger
            console.error(err);
            alert("Server error");
        });
    };
    $scope.selectOwner = function (c, o,type) {
debugger

        if (type=="milestone")
            c.Owner = o.id;
        if (type == "task" || type == "subtask")
            c.owner = o.id;

        var data = {
            id: c.id,
            ownerId:o.id,
            utype:type
        };

        $http({
            method: "POST",
            url: "/pms/projects/default.aspx/updateOwner",
            data: JSON.stringify(data),
            headers: {
                "Content-Type": "application/json; charset=utf-8"
            }
        }).then(function (res) {
            debugger
            if (res.data.d == "1") {
                if (type == "milestone") {
                    let mindex = $scope.projectlist.miles.findIndex(m => m.id == c.id);
                    $scope.projectlist.miles[mindex].Owner = o.id;                    
                }
                else if (type == "task") {                    
                    let mindex = $scope.projectlist.miles.findIndex(m => m.id == c.Milestoneid);
                    let tindex = $scope.projectlist.miles[mindex].tasks.findIndex(m => m.id == c.id);
                    $scope.projectlist.miles[mindex].tasks[tindex].owner = o.id;
                }
                else if (type == "subtask") {                    
                    let mindex = $scope.projectlist.miles.findIndex(m => m.id == c.Milestoneid);
                    let tindex = $scope.projectlist.miles[mindex].tasks.findIndex(m => m.id == c.Task);
                    let stindex = $scope.projectlist.miles[mindex].tasks[tindex].subtasks.findIndex(m => m.id == c.id);
                    $scope.projectlist.miles[mindex].tasks[tindex].subtasks[stindex].owner = o.id;
                }   
                //console.log("Owner updated successfully");
            } else {
                alert("Update failed");
            }

            }).catch(function (err) {
            debugger
            console.error(err);
            alert("Server error");
        });
    };
$scope.loaduserlist = function () {

    debugger;
    //$scope.isLoading = true;

    projectService.getuserlist()
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
$scope.loaduserlist_byrefid = function (refid, usertype, searchtype) {
    debugger;
  

    projectService.getuserlist_byrefid(refid, usertype, searchtype)
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

// update milestone details


$scope.updatedetails_mile = function (c,colname) {

    debugger;

    
      projectService.updatedetails_mile_bycol(c.id,colname,c[colname])

        .then(function (res) {
            console.log("Status updated");
        })
        .catch(function (err) {
debugger
            console.error("Update failed", err);
        });
};
$scope.updatedetails_task = function (c,colname) {

    debugger;

    
      projectService.updatedetails_task_bycol(c.id,colname,c[colname])

        .then(function (res) {
            console.log("Status updated");
        })
        .catch(function (err) {
debugger
            console.error("Update failed", err);
        });
};
$scope.updatedetails_subtask = function (c,colname) {

    debugger;

    
      projectService.updatedetails_subtask_bycol(c.id,colname,c[colname])

        .then(function (res) {
            console.log("Status updated");
        })
        .catch(function (err) {
debugger
            console.error("Update failed", err);
        });
};

$scope.update_duration = function(c, utype,updatefrom) {
    debugger;

    // Project dates
    let projectStart = new Date(parseDotNetDate($scope.projectlist.StartDate));
    let projectEnd = new Date(parseDotNetDate($scope.projectlist.TargetDate));

    // Current start date
     
    let start = new Date(c.StartDate);
    // Normalize dates (fix 1-day issue)
    function normalize(d) {
        if (!d) return null;
        d = new Date(d);
        d.setHours(0,0,0,0);
        return d;
    }

    projectStart = normalize(projectStart);
    projectEnd = normalize(projectEnd);
    start = normalize(start);
    // Check start date before project start
    if (start && projectStart && start < projectStart) {
        alert("Milestone start date cannot be before Project start date");
        c._isUpdating = true;
        
        if (updatefrom=="duration")
        {
        c.txtdays = c._oldtxtdays || null;
        }
        else
        {
        c.StartDate = c._oldStartDate || null;
        }
       c._isUpdating = false;
        return;
    }
if (start && projectStart && start > projectEnd) {
        alert("Milestone start date cannot be after Project end date");
        c._isUpdating = true;   
        if (updatefrom=="duration")
        {
        c.txtdays = c._oldtxtdays || null;
        }
        else
        {
        c.StartDate = c._oldStartDate || null;
        }
        c._isUpdating = false;
        return;
    }
    // Get new end date asynchronously
    projectService.getnew_enddate(start, c.txtdays).then(function(newEndDate) {
        if (!newEndDate) {
            alert("Failed to calculate end date");
            return;
        }

        // Normalize new end date
debugger
        let end =normalize(parseDotNetDate(newEndDate.data.d));
        c.EndDate = end;

        // Validation against project end
        if (end && projectEnd && end > projectEnd) {
            alert("Milestone end date cannot be after Project end date");
            
c._isUpdating = true;
        
        if (updatefrom=="duration")
        {
        c.txtdays = c._oldtxtdays || null;
        }
        else
        {
        c.EndDate = c._oldEndDate || null;
        }
c._isUpdating = false;
            return;
        }

        // Validation start < end
        if (start && end && end < start) {
            alert("End date cannot be before Start date");
            //c.EndDate = c._oldEndDate || null;
            //if (updatefrom=="duration")
            //{
            //c.txtdays = c._oldtxtdays || null;
           // }
c._isUpdating = true;
        
        if (updatefrom=="duration")
        {
        c.txtdays = c._oldtxtdays || null;
        }
        else
        {
        c.EndDate = c._oldEndDate || null;
        }
c._isUpdating = false;
            return;
        }

        c._oldStartDate = angular.copy(c.StartDate);
        c._oldEndDate = angular.copy(c.EndDate);

        // Update timeline
        let resultnew;
        if (utype === 'milestone') {
            resultnew = projectService.updatetimeline_mile(c.id, c.txtdays, start);
        }

        if (!resultnew) return;

            resultnew.then(function(res) {
            let newDate = parseDotNetDate(res.data.d);
            c.EndDate = newDate; 
            // get left days
            return projectService.getleftdays(c.id, utype);
        }).then(function(resdays) {
            if (resdays && resdays.data && resdays.data.d) {
                c.leftdays = resdays.data.d;
            }
        }).catch(function(err) {
            console.error("Update failed", err);
        });

    }).catch(function(err) {
        console.error("Failed to calculate new end date", err);
    });
    };
    
$scope.update_duration_task = function(c, m,updatefrom) {
    debugger;

    // Project dates
    let projectStart = new Date(m.StartDate);
    let projectEnd = new Date(m.EndDate);

    // Current start date
    let start = new Date(c.Startdate);

    // Normalize dates (fix 1-day issue)
    function normalize(d) {
        if (!d) return null;
        d = new Date(d);
        d.setHours(0,0,0,0);
        return d;
    }

    projectStart = normalize(projectStart);
    projectEnd = normalize(projectEnd);
    start = normalize(start);

    // Check start date before project start
    if (start && projectStart && start < projectStart) {
        alert("Task start date cannot be before Milestone start date");
        
        
c._isUpdating = true;
        
        if (updatefrom=="duration")
        {
        c.txtdays = c._oldtxtdays || null;
        }
        else
        {
        c.Startdate = c._oldStartdate || null;
        }
c._isUpdating = false;
        return;
    }
if (start && projectStart && start > projectEnd) {
        alert("Task start date cannot be after Milestone end date");
        //c.Startdate = c._oldStartdate || null;
        //if (updatefrom=="duration")
        //{
        //c.txtdays = c._oldtxtdays || null;
        //}
c._isUpdating = true;
        
        if (updatefrom=="duration")
        {
        c.txtdays = c._oldtxtdays || null;
        }
        else
        {
        c.Startdate = c._oldStartdate || null;
        }
c._isUpdating = false;
        return;
    }
    // Get new end date asynchronously
    projectService.getnew_enddate(start, c.txtdays).then(function(newEndDate) {
        if (!newEndDate) {
            alert("Failed to calculate end date");
            return;
        }

        // Normalize new end date
debugger
        let end =normalize(parseDotNetDate(newEndDate.data.d));
        c.Targetdate = end;

        // Validation against project end
        if (end && projectEnd && end > projectEnd) {
            alert("Task end date cannot be after Milestone end date");
            //c.Targetdate = c._oldTargetdate || null;
            //if (updatefrom=="duration")
            //{
            //c.txtdays = c._oldtxtdays || null;
            //}
c._isUpdating = true;
        
        if (updatefrom=="duration")
        {
        c.txtdays = c._oldtxtdays || null;
        }
        else
        {
        c.Targetdate = c._oldTargetdate || null;
        }
c._isUpdating = false;
            return;
        }

        // Validation start < end
        if (start && end && end < start) {
            alert("End date cannot be before Start date");
            //c.Targetdate = c._oldTargetdate || null;
           // if (updatefrom=="duration")
            //{
            //c.txtdays = c._oldtxtdays || null;
           // }
c._isUpdating = true;
        
        if (updatefrom=="duration")
        {
        c.txtdays = c._oldtxtdays || null;
        }
        else
        {
        c.Targetdate = c._oldTargetdate || null;
        }
c._isUpdating = false;
            return;
        }

        c._oldStartdate = angular.copy(c.Startdate);
        c._oldTargetdate = angular.copy(c.Targetdate);

        // Update timeline
        let resultnew;
       
            resultnew = projectService.updatetimeline_task(c.id, c.txtdays, start);
        

        if (!resultnew) return;

            resultnew.then(function(res) {
            let newDate = parseDotNetDate(res.data.d);
            c.Targetdate = newDate; 
            // get left days
            return projectService.getleftdays(c.id, 'task');
        }).then(function(resdays) {
            if (resdays && resdays.data && resdays.data.d) {
                c.leftdays = resdays.data.d;
            }
        }).catch(function(err) {
            console.error("Update failed", err);
        });

    }).catch(function(err) {
        console.error("Failed to calculate new end date", err);
    });
};

$scope.update_duration_subtask = function(c, t,updatefrom) {
    debugger;

    // Project dates
    let projectStart = new Date(parseDotNetDate(t.Startdate));
    let projectEnd = new Date(parseDotNetDate(t.Targetdate));

    // Current start date
    let start = new Date(c.Startdate);

    // Normalize dates (fix 1-day issue)
    function normalize(d) {
        if (!d) return null;
        d = new Date(d);
        d.setHours(0,0,0,0);
        return d;
    }

    projectStart = normalize(projectStart);
    projectEnd = normalize(projectEnd);
    start = normalize(start);

    // Check start date before project start
    if (start && projectStart && start < projectStart) {
        alert("Sub Task start date cannot be before Task start date");
        //c.Startdate = c._oldStartdate || null;
       // if (updatefrom=="duration")
        //{
        //c.txtdays = c._oldtxtdays || null;
        //}
c._isUpdating = true;
        
        if (updatefrom=="duration")
        {
        c.txtdays = c._oldtxtdays || null;
        }
        else
        {
        c.Startdate = c._oldStartdate || null;
        }
c._isUpdating = false;
        return;
    }
if (start && projectStart && start > projectEnd) {
        alert("Sub Task start date cannot be after Task end date");
        //c.Startdate = c._oldStartdate || null;
        //if (updatefrom=="duration")
        //{
        //c.txtdays = c._oldtxtdays || null;
        //}
c._isUpdating = true;
        
        if (updatefrom=="duration")
        {
        c.txtdays = c._oldtxtdays || null;
        }
        else
        {
        c.Startdate = c._oldStartdate || null;
        }
c._isUpdating = false;
        return;
    }
    // Get new end date asynchronously
    projectService.getnew_enddate(start, c.txtdays).then(function(newEndDate) {
        if (!newEndDate) {
            alert("Failed to calculate end date");
            return;
        }

        // Normalize new end date
debugger
        let end =normalize(parseDotNetDate(newEndDate.data.d));
        c.Targetdate = end;

        // Validation against project end
        if (end && projectEnd && end > projectEnd) {
            alert("Sub Task end date cannot be after Task end date");
           // c.Targetdate = c._oldTargetdate || null;
            //if (updatefrom=="duration")
            //{
            //c.txtdays = c._oldtxtdays || null;
            //}
c._isUpdating = true;
        
        if (updatefrom=="duration")
        {
        c.txtdays = c._oldtxtdays || null;
        }
        else
        {
        c.Targetdate = c._oldTargetdate || null;
        }
c._isUpdating = false;
            return;
        }

        // Validation start < end
        if (start && end && end < start) {
            alert("End date cannot be before Start date");
            //c.Targetdate = c._oldTargetdate || null;
            //if (updatefrom=="duration")
            //{
            //c.txtdays = c._oldtxtdays || null;
            //}
c._isUpdating = true;
        
        if (updatefrom=="duration")
        {
        c.txtdays = c._oldtxtdays || null;
        }
        else
        {
        c.Targetdate = c._oldTargetdate || null;
        }
c._isUpdating = false;
            return;
        }

        c._oldStartdate = angular.copy(c.Startdate);
        c._oldTargetdate = angular.copy(c.Targetdate);

        // Update timeline
        let resultnew;
       
            resultnew = projectService.updatetimeline_subtask(c.id, c.txtdays, start);
        

        if (!resultnew) return;

            resultnew.then(function(res) {
            let newDate = parseDotNetDate(res.data.d);
            c.Targetdate = newDate; 
            // get left days
            return projectService.getleftdays(c.id, 'subtask');
        }).then(function(resdays) {
            if (resdays && resdays.data && resdays.data.d) {
                c.leftdays = resdays.data.d;
            }
        }).catch(function(err) {
            console.error("Update failed", err);
        });

    }).catch(function(err) {
        console.error("Failed to calculate new end date", err);
    });
};
$scope.update_durationold = function (c, utype) {
    debugger;

    let resultnew;

    if (utype == 'milestone') {
        resultnew = projectService.updatetimeline_mile(c.id, c.txtdays, new Date(c.StartDate));
    }
    else if (utype == 'task') {
        resultnew = projectService.updatetimeline_task(c.id, c.txtdays, new Date(c.Startdate));
    }
    else if (utype == 'subtask') {
        resultnew = projectService.updatetimeline_subtask(c.id, c.txtdays, new Date(c.Startdate));
    }

    if (!resultnew) return;

    resultnew.then(function (res) {

        let newDate = parseDotNetDate(res.data.d);

        if (utype == 'milestone') {
            c.EndDate = newDate;
        } else {
            c.Targetdate = newDate;
        }

        return projectService.getleftdays(c.id, utype);

    }).then(function (resdays) {

        debugger;

        if (resdays && resdays.data && resdays.data.d) {
            c.leftdays = resdays.data.d;
        }

    }).catch(function (err) {
        debugger;
        console.error("Update failed", err);
    });
    };

    $scope.parseDuration = function (val1,val2) {
        
        let hrs = parseInt(val1) || 0;
        let min = parseInt(val2) || 0;

        return hrs + (min / 60);
    };
    $scope.getTotal = function (list) {
        let total = 0;

        (list || []).forEach(item => {
            total += $scope.parseDuration(item.TaskDuration, item.TaskDuration_min);
        });

        return total;
    };
    $scope.validateMilestone = function (milestone) {

        let milestoneHrs = $scope.parseDuration(milestone.TaskDuration, milestone.TaskDuration_min);
        let totalTaskHrs = $scope.getTotal(milestone.tasks);

        return totalTaskHrs <= milestoneHrs;
    };    
    $scope.validateTask = function (task) {

        let taskHrs = $scope.parseDuration(task.TaskDuration, task.TaskDuration_min);
        let totalSubtaskHrs = $scope.getTotal(task.subtasks);

        return totalSubtaskHrs <= taskHrs;
    };
    $scope.hrs_validate = function (c, utype) {
        let ret_hrs = 0;       
        if (utype == "task") {
            ret_hrs += parseFloat($scope.normalizeTime_calhr_new(c.TaskDuration, c.TaskDuration_min));
            
        }
        if (utype == "subtask") {
        }
        return ret_hrs;
    }
    $scope.checkLimit = function (item, parent, type) {
        debugger
        let isValid = true;

        if (type === 'task') {
            isValid = $scope.validateMilestone(parent);
        }

        if (type === 'subtask') {
            isValid = $scope.validateTask(parent);
        }

        if (!isValid) {
            alert("Hours exceeded!");

            // revert value
            item.TaskDuration = item._oldTaskDuration || '';
            item.TaskDuration_min = item._oldTaskDuration_min || '';

        } else {
            item._oldTaskDuration = item.TaskDuration;
            item._oldTaskDuration_min = item.TaskDuration_min;
            $scope.update_hrs(item, type);
        }

    };
    $scope.update_hrs = function (c, utype) {
        debugger;

        //let newval = parseFloat(c.TaskDuration || "0");
        let newval = parseFloat($scope.normalizeTime_calhr_new(c.TaskDuration, c.TaskDuration_min));

        // Get start and end dates
        let start = utype === 'milestone' ? new Date(c.StartDate) : new Date(c.Startdate);
        let end = utype === 'milestone' ? new Date(c.EndDate) : new Date(c.Targetdate);

        if (!start || !end || isNaN(start) || isNaN(end)) return;

        // Normalize dates to remove time
        start.setHours(0, 0, 0, 0);
        end.setHours(0, 0, 0, 0);

        // Calculate difference in days (at least 1)
        let diffDays = Math.max(1, Math.floor((end - start) / (1000 * 60 * 60 * 24)) + 1);

        // Convert to hours (1 day = 24 hours)
        let hours = (diffDays * 24)*60;

        if (newval > hours) {

            alert("Working Hrs cannot be greater than " + (diffDays * 24) + " hours");
            c._isUpdating = true;
            c.TaskDuration = c._oldTaskDuration; // reset to max allowed
            c._isUpdating = false;
            return;
        }
        var duration_val = "";

        if (c.TaskDuration && c.TaskDuration_min) {
            duration_val = c.TaskDuration + ":" + c.TaskDuration_min;
        } else if (c.TaskDuration) {
            duration_val = c.TaskDuration+":";
        } else if (c.TaskDuration_min) {
            duration_val = ":" + c.TaskDuration_min;
        }
        if (utype == "milestone")
            projectService.updatedetails_mile_bycol(c.id, 'TaskDuration', duration_val)
        else if (utype == "task")
            projectService.updatedetails_task_bycol(c.id, 'TaskDuration', duration_val)
        else if (utype == "subtask")
            projectService.updatedetails_subtask_bycol(c.id, 'TaskDuration', duration_val)

                .then(function (res) {
                    console.log("Status updated");
                })
                .catch(function (err) {
                    debugger
                    console.error("Update failed", err);
                });

    };
    $scope.update_hrs_old = function (c, utype) {
        debugger;

        //let newval = parseFloat(c.TaskDuration || "0");
        let newval = parseFloat($scope.normalizeTime_calhr(c.TaskDuration));

        // Get start and end dates
        let start = utype === 'milestone' ? new Date(c.StartDate) : new Date(c.Startdate);
        let end = utype === 'milestone' ? new Date(c.EndDate) : new Date(c.Targetdate);

        if (!start || !end || isNaN(start) || isNaN(end)) return;

        // Normalize dates to remove time
        start.setHours(0, 0, 0, 0);
        end.setHours(0, 0, 0, 0);

        // Calculate difference in days (at least 1)
        let diffDays = Math.max(1, Math.floor((end - start) / (1000 * 60 * 60 * 24)) + 1);

        // Convert to hours (1 day = 24 hours)
        let hours = (diffDays * 24) * 60;

        if (newval > hours) {

            alert("Working Hrs cannot be greater than " + (diffDays * 24) + " hours");
            c._isUpdating = true;
            c.TaskDuration = c._oldTaskDuration; // reset to max allowed
            c._isUpdating = false;
            return;
        }
        if (utype == "milestone")
            projectService.updatedetails_mile_bycol(c.id, 'TaskDuration', c.TaskDuration)
        else if (utype == "task")
            projectService.updatedetails_task_bycol(c.id, 'TaskDuration', c.TaskDuration)
        else if (utype == "subtask")
            projectService.updatedetails_subtask_bycol(c.id, 'TaskDuration', c.TaskDuration)

                .then(function (res) {
                    console.log("Status updated");
                })
                .catch(function (err) {
                    debugger
                    console.error("Update failed", err);
                });

    };
// end update details milestone

 

//#region manage files upload
   
$scope.modalTitle = ""; // Default

$scope.loadSrcFiles = function(c, strtype,index) {
debugger
    if(strtype=="milestone")
    {
    $scope.currentFMilestoneId=c.id;
    }
    else if(strtype=="task"||strtype=="subtask")
    {
    $scope.currentFMilestoneId=c.Milestoneid;
    if(strtype=="subtask")
    {
    $scope.currentFTaskId=c.Task;
    }
    }

    $scope.showfileslist=null;
    $scope.currentFileType = "";
    $scope.currentFProjectId ="";
    $scope.currentFId ="";

    $scope.currentlist=c;
    $scope.currentIndex =index;    
    $scope.currentFileType = strtype;
    $scope.currentFProjectId = c.ProjectId;
    $scope.currentFId = c.id;

    //$scope.showfileslist=angular.copy($scope.projectlist.miles[index].viewfiles);
    $scope.showfileslist=angular.copy(c.viewfiles);
  
    // Set modal title based on type
    switch(strtype) {
        case "milestone":
            $scope.modalTitle = "Upload milestone documents";
            break;
        case "task":
            $scope.modalTitle = "Upload task documents";
            break;
        case "subtask":
            $scope.modalTitle = "Upload sub task documents";
            break;
        default:
            $scope.modalTitle = "Upload Files";
    }

    // Update label in the modal
    var lbl = document.getElementById('ContentPlaceHolder1_lblfiles');
    if(lbl) lbl.innerHTML = $scope.modalTitle;

    
   // $scope.bindTaskData(strid, strtype);
};


    $scope.isDragging = false;
    $scope.uploadedFiles = [];
    

    const dropZone = document.getElementById("dropZone");
    const fileInput = document.getElementById("fileupload");

    // Click drop zone → open file picker
    $scope.openFilePicker = function() {
   

        fileInput.click();
    };

    // Files selected via input
    $scope.filesSelected = function(files) {
    debugger  
     
    $scope.processFiles(files, $scope.currentFProjectId, $scope.currentFId, $scope.currentFileType,$scope.currentlist);
    };

    // Drag over
    dropZone.addEventListener("dragover", function(e) {
        e.preventDefault();
dropZone.style.borderColor = "#007bff";
    dropZone.style.backgroundColor = "#f0f8ff";
        $scope.$apply(() => { $scope.isDragging = true; });
    });

    // Drag leave
    dropZone.addEventListener("dragleave", function(e) {    
        e.preventDefault();
dropZone.style.borderColor = "#ccc";
    dropZone.style.backgroundColor = "";
        $scope.$apply(() => { $scope.isDragging = false; });
    });

    // Drop files
    dropZone.addEventListener("drop", function(e) {
        e.preventDefault();
dropZone.style.borderColor = "#ccc";
    dropZone.style.backgroundColor = "";
        $scope.$apply(() => { $scope.isDragging = false; });
        $scope.processFiles(
            e.dataTransfer.files,   
            $scope.currentFProjectId,
            $scope.currentFId,
            $scope.currentFileType,
$scope.currentlist
);
    });

   $scope.processFiles = function(files,fpid,fid,ftype,obj) {
debugger

        for (let i = 0; i < files.length; i++) {
            let file = files[i];
            let reader = new FileReader();
            reader.onload = function(evt) {
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
        $scope.uploadFilesToServer(files,fpid,fid,ftype,obj);
    };
 
$scope.uploadFilesToServer = function(files,pid,id,type,obj) {
debugger
        let formData = new FormData();
        for (let i = 0; i < files.length; i++) {
            formData.append("files", files[i]);
        }
     formData.append("pid", pid);
    formData.append("ftype", type);
    formData.append("id", id);
        $http.post("/pms/SingleFileUploadHandlerANG.ashx", formData, {
            headers: { 'Content-Type': undefined },
            transformRequest: angular.identity
        }).then(function(res) { 
debugger  
try
{
res.data.forEach(file => {
            //$scope.projectlist.miles[$scope.currentIndex].viewfiles.push(file);
            obj.viewfiles.push(file);
        });
}
catch
{}
//$scope.showfileslist = angular.copy($scope.projectlist.miles[$scope.currentIndex].viewfiles);
$scope.showfileslist = angular.copy($scope.currentlist.viewfiles);
            console.log("Upload success", res.data);
        }, function(err) {

            console.error("Upload failed", err);
        });
    };

$scope.removedocumentnew = function(fileId) {
    //if (!confirm("Are you sure you want to delete this file?")) return;

  debugger
    $http({
        method: 'POST',
        url: '/pms/DeleteimagehandlerANG.ashx', 
        data: {id:fileId},
        headers: { 'Content-Type':'application/json' }
    }).then(function(res) {
debugger
        if (res.data.success) {
if($scope.currentFileType=="milestone")
{
            let index = $scope.projectlist.miles[$scope.currentIndex].viewfiles.findIndex(f => f.id == fileId);
            if (index !== -1) {
                //$scope.projectlist.miles[$scope.currentIndex].viewfiles.splice(index, 1);
                $scope.projectlist.miles[$scope.currentIndex].viewfiles.splice(index, 1);
                $scope.showfileslist = angular.copy($scope.projectlist.miles[$scope.currentIndex].viewfiles);
            }
}
else if($scope.currentFileType=="task")
{
            
            var milesid=$scope.currentFMilestoneId;
            let mindex = $scope.projectlist.miles.findIndex(m => m.id == milesid);
            let index = $scope.projectlist.miles[mindex].tasks[$scope.currentIndex].viewfiles.findIndex(f => f.id == fileId);
            if (index !== -1) {
                $scope.projectlist.miles[mindex].tasks[$scope.currentIndex].viewfiles.splice(index, 1);
                $scope.showfileslist = angular.copy($scope.projectlist.miles[mindex].tasks[$scope.currentIndex].viewfiles);
            }
}
else if($scope.currentFileType=="subtask")
{
            var milesid=$scope.currentFMilestoneId;
            let mindex = $scope.projectlist.miles.findIndex(m => m.id == milesid);
            var taskid=$scope.currentFTaskId;
            let tindex = $scope.projectlist.miles[mindex].tasks.findIndex(m => m.id == taskid);
            let index = $scope.projectlist.miles[mindex].tasks[tindex].subtasks[$scope.currentIndex].viewfiles.findIndex(f => f.id == fileId);
            if (index !== -1) {
                $scope.projectlist.miles[mindex].tasks[tindex].subtasks[$scope.currentIndex].viewfiles.splice(index, 1);
                $scope.showfileslist = angular.copy($scope.projectlist.miles[mindex].tasks[tindex].subtasks[$scope.currentIndex].viewfiles);
            }
}
        } else {
            alert("Failed to delete file: " + res.data.message);
        }
    }, function(err) {
debugger
        
        alert("Error deleting file");
    });
};
//#endregion
//#region manage milestone new
$scope.newmilestonelist = {
                id: 0,
                ProjectId: 0,
                Milestone: "",
                Weightage:null,
                StartDate:null,
                EndDate:null,
                Owner:"",
                Access:null,
                team:"",
                P_Status:null,
                status_name:"-",
                status_color:"",
                Priority:null,
                priority_name:"-",
                priority_color:"",
                Budget:null,
                Actual:null,
                uid: 0,
                cdate:null,
                statuscomplete: 0,
                istask: 0,
                checkmileWeightage: 0,
                iscomplete: 0,
                iscompletedate:null,
                iscompletepercentage: 0,
                Durationtype: 0,
                TaskDuration:"",
                TaskDuration_min:"",
                Durationhourcal: 0,
                orderno:null,
                miledescription:"",
                Uploaddrawing:"",
                txtdays:"",
                complete_value:"0",
                pending_value:"100",
                taskcount:0,
                isExpanded:false,
                leftdays:"",
                owners: [],
                teams: [],
                viewfiles: [],
                tasks:[]
                
            };
$scope.newMid="0";
$scope.addRowUpDown_default = function () {
debugger
    if (!$scope.projectlist || !$scope.projectlist.miles) {
        $scope.projectlist.miles = [];
    }

    // Copy the default template
    let newRow = angular.copy($scope.newmilestonelist);

    // Set dates
    if ($scope.projectlist.StartDate) {
        newRow.StartDate = $scope.projectlist.StartDate.indexOf('/Date(') === 0
            ? new Date(parseInt($scope.projectlist.StartDate.substr(6)))
            : new Date($scope.projectlist.StartDate);
    }

    if ($scope.projectlist.TargetDate) {
        newRow.EndDate = $scope.projectlist.TargetDate.indexOf('/Date(') === 0
            ? new Date(parseInt($scope.projectlist.TargetDate.substr(6)))
            : new Date($scope.projectlist.TargetDate);
    }
try
{
newRow.txtdays=getDaysBetween(newRow.StartDate,newRow.EndDate);
/**/
projectService.getnew_enddate(newRow.StartDate, newRow.txtdays)
.then(function(newEndDate) {
    if (!newEndDate) {
        alert("Failed to calculate end date");
        return;
    }    
    let end = normalize(parseDotNetDate(newEndDate.data.d));
     newRow.EndDate=end;

})
.catch(function(err) {

    console.error("Update failed", err);

});

}
catch
{
}
    // Copy other properties
   // newRow.P_Status = $scope.projectlist.ProjectStatus;
  var nstatus =0;
var nstatus_name = "";
var nstatus_color = "";
try
{
var ststuddefault = ($scope.statuslists || []).find(m => Number(m.isdefault) === 1);
            if (ststuddefault) {
                nstatus = ststuddefault.id;
                nstatus_name = ststuddefault.name;
                nstatus_color = ststuddefault.colorname;
            }

}
catch
{}
newRow.P_Status=nstatus;    
   newRow.status_name=nstatus_name;  
   newRow.status_color=nstatus_color;

    //newRow.Priority = $scope.projectlist.Priority;
    var npriority = 0;
    var npriority_name = "";
    var npriority_color = "";
    try {
        var prioritydefault = ($scope.prioritylists || []).find(m => Number(m.isdefault) === 1);
        if (prioritydefault) {
            npriority = prioritydefault.id;
            npriority_name = prioritydefault.name;
            npriority_color = prioritydefault.colorname;
        }

    }
    catch
    { }
    newRow.Priority = npriority;
    newRow.priority_name = npriority_name;
    newRow.priority_color = npriority_color;


    newRow.Owner = $scope.projectlist.Owner;
    newRow.owners = $scope.projectlist.owners;
    newRow.team = $scope.projectlist.team;
    newRow.teams = $scope.projectlist.teams;
    newRow.ProjectId = $scope.projectlist.id;
if($scope.projectlist.hastask==1)
        {newRow.istask=1;}
    if($scope.projectlist.checkprojectWeightage==1)
        {newRow.checkmileWeightage=1;}

    // Set order number
    newRow.orderno = $scope.projectlist.miles.length + 1;
    // **Push immediately so UI updates**
    //$scope.projectlist.miles.splice(insertIndex, 0, newRow);
    $scope.projectlist.miles.push(newRow);

    // Clear temporary fields
    newRow.cdate = null;
    newRow.iscompletedate = null;

    // Save to DB asynchronously
    $scope.addnewmilestone(newRow, newRow.orderno)
        .then(async function (response) {
            let newid = response;
  
            // Update the row ID without removing/re-adding
            newRow.id = newid;
        // let new_weightage = await projectService.getweightage(newid,'milestone');
         let new_weightage = await projectService.getweightage_default(newRow.ProjectId,'milestone')
newRow.Weightage=new_weightage.data.d;
angular.forEach($scope.projectlist.miles, function (m){
        m.Weightage = new_weightage.data.d;
     });
            // Refresh order numbers if needed
            $scope.refreshOrderNo();
        if($scope.view=="gantt")
        {
            $scope.buildGantt();
            setTimeout(initTableUI, 300);            
         }
            
            

            // Make sure Angular notices changes
            if (!$scope.$$phase) $scope.$apply();
        })
        .catch(function (err) {
            // On failure, remove the row immediately
            let index = $scope.projectlist.miles.indexOf(newRow);
            if (index > -1) $scope.projectlist.miles.splice(index, 1);
            console.error("Failed to save default milestone", err);
        });
};
    $scope.addRowUpDown_mile_gantt = function ($index, c, position) {
        debugger
        let mindex = $scope.projectlist.miles.findIndex(f => f.id === c.id);
        if (mindex !== -1) {            
            let c1 = $scope.projectlist.miles[mindex];
            $scope.addRowUpDown_mile(mindex,c1, position);
        }
        
    }
$scope.addRowUpDown_mile = function ($index,c, position) {
    debugger
    //alert(index+","+c+","+ position);
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
$scope.newmilestonelist.StartDate=c.StartDate
$scope.newmilestonelist.EndDate=c.EndDate

 let newRow = angular.copy($scope.newmilestonelist);
    let insertIndex,newOrderNo;
    if (position === 'up') {
        insertIndex = $index;        // UI
        newOrderNo = c.orderno;        // DB
    } else {
        insertIndex = $index + 1;    
        newOrderNo = c.orderno + 1;
    }
var nstatus =0;
var nstatus_name = "";
var nstatus_color = "";
//nstatus=c.P_Status;    
//nstatus_name=c.status_name;    
//nstatus_color=c.status_color;
try
{
newRow.txtdays=getDaysBetween(newRow.StartDate,newRow.EndDate);
}
catch
{
}
try
{
var ststuddefault = ($scope.statuslists || []).find(m => Number(m.isdefault) === 1);
            if (ststuddefault) {
                nstatus = ststuddefault.id;
                nstatus_name = ststuddefault.name;
                nstatus_color = ststuddefault.colorname;
            }
    var npriority = 0;
    var npriority_name = "";
    var npriority_color = "";
    try {
        var prioritydefault = ($scope.prioritylists || []).find(m => Number(m.isdefault) === 1);
        if (prioritydefault) {
            npriority = prioritydefault.id;
            npriority_name = prioritydefault.name;
            npriority_color = prioritydefault.colorname;
        }

    }
    catch
    { }
   
}
catch
{}
    newRow.orderno=newOrderNo;
    newRow.ProjectId=c.ProjectId;
   
    //newRow.P_Status=c.P_Status;    
    //newRow.status_name=c.status_name;    
    //newRow.status_color=c.status_color; 
   newRow.P_Status=nstatus;    
   newRow.status_name=nstatus_name;  
   newRow.status_color=nstatus_color;

    newRow.Priority = npriority;
    newRow.priority_name = npriority_name;
    newRow.priority_color = npriority_color;



    newRow.Owner=c.Owner;    
    newRow.owners=c.owners;    
    newRow.team=c.team; 
    newRow.teams=c.teams; 
try
{
if($scope.projectlist.hastask==1)
        {newRow.istask=1;}
    if($scope.projectlist.checkprojectWeightage==1)
        {newRow.checkmileWeightage=1;}
}
catch{}

    // Insert in UI
    $scope.projectlist.miles.splice(insertIndex, 0, newRow); 
   // Insert in DB    
    let c1 = angular.copy(c);
    c1.cdate=null;
    c1.iscompletedate=null;
    c1.tasks=[];
    c1.viewfiles=[];
    //$scope.addnewmilestone(c,newOrderNo)
    $scope.addnewmilestone(c1,newOrderNo)
    .then(async function(response) {
debugger
       var newid = response;
       //let new_weightage = await projectService.getweightage(newid,'milestone');
       let new_weightage = await projectService.getweightage_default(c1.ProjectId,'milestone')
        newRow.Weightage=new_weightage.data.d;
      angular.forEach($scope.projectlist.miles, function (m){
        m.Weightage = new_weightage.data.d;
     });

        $scope.projectlist.miles[insertIndex].id=newid;       
        $scope.refreshOrderNo();
        if ($scope.view == "gantt"){
            debugger
            let mindex = $scope.projectlist.miles.findIndex(f => f.id === newid);
            var m = $scope.projectlist.miles[mindex];
            var mr = $scope.makeGanttRow(m, "milestone", 0, null);
            var ganttInsertIndex = 0;
            var milestoneCount = 0;
            for (var i = 0; i < $scope.ganttRows.length; i++) {

                if ($scope.ganttRows[i].type === "milestone") {

                    if (milestoneCount === mindex) {

                        ganttInsertIndex = i;

                        break;
                    }

                    milestoneCount++;
                }

                ganttInsertIndex = i + 1;
            }
            // ================= INSERT AT SAME POSITION =================
            $scope.ganttRows.splice(ganttInsertIndex, 0, mr);
           
        }
    
    })
    .catch(function(err) {
debugger
        // If server fails, remove the temporary row
        $scope.projectlist.miles.splice(insertIndex, 1);
        //alert("Failed to save row");
    });
    initTableUI();
};

$scope.refreshOrderNo = function () {
debugger

    if (!$scope.projectlist||!$scope.projectlist.miles) return;

    let updatePromises = [];

    angular.forEach($scope.projectlist.miles, function (item, index) {
        // Update UI order number

        item.orderno = index + 1;
debugger
        // Call service to update DB and store promise
        let promise = projectService.updateorderno_milestone(item.id, item.orderno)
            .then(function(response) {
debugger
                if (response.data && response.data.d) {
                    console.log('Updated orderno for id:', item.id, 'to', item.orderno);
                } else {
                    console.warn('Update returned false for id:', item.id);
                }
            })
            .catch(function(err){
                console.error('Failed to update orderno for id:', item.id, err);
            });

        updatePromises.push(promise);
    });

    // Wait for all updates to finish
    Promise.all(updatePromises).then(function() {
        console.log('All order numbers updated successfully.');
    });
    
};
$scope.addnewmilestone = function(object,orderno) {
debugger
    return projectService.addnewmilestonerow(object,orderno)
        .then(function(response) {
            if(response.data.d) {
                return response.data.d;
            } else {
                return $q.reject({message: "Server did not return data"});
            }
        });
};
$scope.deletedatamile = function (index,c) {
   debugger
var isupdate="0";
projectService.removemilestone(c.id)
        .then(async function(response) {debugger
            if(response.data.d) {
                isupdate= response.data.d;
                if(isupdate=="1")
                {
                    debugger
                    var removeid = c.id;
                    let realIndex = $scope.projectlist.miles.findIndex(f => f.id === removeid);
                    if(realIndex !== -1){
                        $scope.projectlist.miles.splice(realIndex, 1);
                        if ($scope.view == "gantt") {
                            debugger
                            $scope.deleteGanttRowById(removeid,'milestone');

                        }
                    }
                }
      let new_weightage = await projectService.getweightage_default(c.ProjectId,'milestone');
       var WeightageN=new_weightage.data.d;
    angular.forEach($scope.projectlist.miles, function (m){
        m.Weightage =WeightageN;
     });
 $scope.$applyAsync();
            } 
        })

   
    
    };  
    $scope.deleteGanttRowById = function (id, type) {

        var prefix = "";

        if (type === "milestone") {
            prefix = "m_";
        }
        else if (type === "task") {
            prefix = "t_";
        }
        else if (type === "subtask") {
            prefix = "s_";
        }

        var uid = prefix + id;

        // remove selected row
        var index = $scope.ganttRows.findIndex(function (r) {
            return r.uid === uid;
        });

        if (index > -1) {
            $scope.ganttRows.splice(index, 1);
        }

        // also remove child rows
        //$scope.ganttRows = $scope.ganttRows.filter(function (r) {

        //    if (type === "milestone") {
        //        return r.parentUid !== uid;
        //    }

        //    if (type === "task") {
        //        return r.parentUid !== uid;
        //    }

        //    return true;
        //});

        // update original rows also
        //$scope.originalRows = angular.copy($scope.ganttRows);

        //$scope.buildGanttTimeline();
    };
    $scope.deleteGanttRowById11 = function (id, type) {

        var uid = "";

        if (type === "milestone") {
            uid = "m_" + id;
        }
        else if (type === "task") {
            uid = "t_" + id;
        }
        else {
            uid = "s_" + id;
        }

        // ================= GET ALL CHILD UIDS =================

        var removeMap = {};

        function collectChildren(parentUid) {

            removeMap[parentUid] = true;

            angular.forEach($scope.ganttRows, function (r) {

                if (r.parentUid === parentUid) {

                    collectChildren(r.uid);
                }
            });
        }

        collectChildren(uid);

        // ================= REMOVE ONLY RELATED ROWS =================

        $scope.ganttRows = $scope.ganttRows.filter(function (r) {

            return !removeMap[r.uid];
        });

        // update original rows
        $scope.originalRows = angular.copy($scope.ganttRows);

        $scope.buildGanttTimeline();
    };
$scope.deletedata_gantt = function (c,utype) {
        debugger
        
        if (utype == "milestone") {
            let mindex = $scope.projectlist.miles.findIndex(f => f.id === c.id);
            if (mindex !== -1) {
                let c1 = $scope.projectlist.miles[mindex];
                $scope.deletedatamile(mindex, c1);
            }
        }
        else if (utype == "task") {
            let mindex = $scope.projectlist.miles.findIndex(f => f.id === c.Milestoneid);

            let tindex = $scope.projectlist.miles[mindex].tasks.findIndex(f => f.id === c.id);
            if (tindex !== -1) {
                let c1 = $scope.projectlist.miles[mindex].tasks[tindex];
               
                    $scope.deletedatatask(tindex, c1, $scope.projectlist.miles[mindex]);
            }
        }
        else if (utype == "subtask") {
            let mindex = $scope.projectlist.miles.findIndex(f => f.id === c.Milestoneid);
            let tindex = $scope.projectlist.miles[mindex].tasks.findIndex(f => f.id === c.Task);
            if (tindex !== -1) {
                let sindex = $scope.projectlist.miles[mindex].tasks[tindex].subtasks.findIndex(f => f.id === c.id);
                if (sindex !== -1) {
                    let c1 = $scope.projectlist.miles[mindex].tasks[tindex].subtasks[sindex];
                    $scope.deletedatasubtask(tindex, c1, $scope.projectlist.miles[mindex].tasks[tindex]);                   
                }
            }
        }


    }; 
//#endregion
//#region manage milestone task
$scope.newtasklist = {

    id:0,
    ProjectId:0,
    Milestoneid:0,
    taskname:"",
    taskdescripton:"",
    owner:"",
    team:"",
    Startdate:null,
    Targetdate:null,
    Weightage:null,
    StartTime:"",
    Timeline:"",
    TaskDuration: "",
    TaskDuration_min: "",
    P_Status:null,
    status_name:"-",
    status_color:"",
    Priority:null,
    priority_name:"-",
    priority_color:"",
    Budget:null,
    Actual:null,
    Reminder:"",
    ReminderType:null,
    Days:"",
    Time:"",
    uid :null,
    cdate:null,
    tagto:"",
    dependancy:"",
    issubtask:null,
    checkTaskWeightage :"",
    iscomplete:"",
    iscompletepercentage:0,
    orderno:null,    
    txtdays:"",
    complete_value:"0",
    pending_value:"100",
    isExpanded:false, 
   leftdays:"",
    owners:[],
    teams:[],
    viewfiles:[],
    subtasks:[]
   };
$scope.addRowUpDown_taskdefault = function (m) {
debugger
    if (!m.tasks) {
        m.tasks = [];
    }

    // Copy the default template
    let newRow = angular.copy($scope.newtasklist);

    // Set dates
    if (m.StartDate) {
        
try{

 newRow.Startdate = m.StartDate.indexOf('/Date(') === 0
            ? new Date(parseInt(m.StartDate.substr(6)))
            : new Date(m.StartDate);
}catch{
      newRow.Startdate=new Date(m.StartDate)
}
    }

    if (m.EndDate) {
try{
newRow.Targetdate = m.EndDate.indexOf('/Date(') === 0
            ? new Date(parseInt(m.EndDate.substr(6)))
            : new Date(m.EndDate);

}catch{
     newRow.Targetdate=new Date(m.EndDate)   
}
    }
/*

try
{
newRow.txtdays=getDaysBetween(newRow.Startdate,newRow.Targetdate);
}
catch
{
}*/
    // Copy other properties
var nstatus =0;
var nstatus_name = "";
var nstatus_color = "";
try
{
var ststuddefault = ($scope.statuslists || []).find(m => Number(m.isdefault) === 1);
            if (ststuddefault) {
                nstatus = ststuddefault.id;
                nstatus_name = ststuddefault.name;
                nstatus_color = ststuddefault.colorname;
            }
    var npriority = 0;
    var npriority_name = "";
    var npriority_color = "";
    try {
        var prioritydefault = ($scope.prioritylists || []).find(m => Number(m.isdefault) === 1);
        if (prioritydefault) {
            npriority = prioritydefault.id;
            npriority_name = prioritydefault.name;
            npriority_color = prioritydefault.colorname;
        }

    }
    catch
    { }
}
catch
{}
    newRow.ProjectId =m.ProjectId;
    newRow.Milestoneid = m.id;

    //newRow.P_Status = m.P_Status;
   newRow.P_Status=nstatus;    
   newRow.status_name=nstatus_name;  
   newRow.status_color=nstatus_color;

    newRow.Priority = npriority;
    newRow.priority_name = npriority_name;
    newRow.priority_color = npriority_color;


    //newRow.Priority = m.Priority;
    newRow.Owner =m.Owner;
    newRow.owners =m.owners;
    newRow.team =m.team;  
    newRow.teams =m.teams;  
    if ($scope.projectlist.hassubtask == 1) { newRow.issubtask = 1; }
   if ($scope.projectlist.checktaskWeightage == 1) { newRow.checkmileWeightage = 1; }
    // Set order number
    newRow.orderno =m.tasks.length + 1;

    // **Push immediately so UI updates**
    //$scope.projectlist.miles.splice(insertIndex, 0, newRow);
    m.tasks.push(newRow);

    // Clear temporary fields
    newRow.cdate = null;
    newRow.iscompletedate = null;

    // Save to DB asynchronously
    $scope.addnewtask(newRow, newRow.orderno)
        .then(async function (response) {
            let newid = response;
debugger
            // Update the row ID without removing/re-adding
            newRow.id = newid;
//let new_weightage = await projectService.getweightage(newid,'task');
let new_weightage = await projectService.getweightage_default(newRow.Milestoneid,'task')
newRow.Weightage=new_weightage.data.d;
var gmindex= $scope.projectlist.miles.findIndex(f => f.id ===newRow.Milestoneid);
angular.forEach($scope.projectlist.miles[gmindex].tasks, function (t){
        t.Weightage = new_weightage.data.d;
     });
            // Refresh order numbers if needed
            $scope.refreshOrderNo_tasks(m);
            m.taskcount=1;
            $scope.toggleMile_default(m);
            //toggleMilestone(m);
            if ($scope.view == "gantt") {               

                    debugger
                    let mindex = $scope.projectlist.miles.findIndex(f => f.id === m.id);
                    let tindex = m.tasks.findIndex(f => f.id === newid);
                    var t = m.tasks[tindex];
                var mr = $scope.makeGanttRow(t, "task", 1, "m_" + m.id);
                try {
                    $scope.ganttRows.push(mr);
                    let gmindex = $scope.ganttRows.findIndex(r =>r.type === "milestone" &&r.id == m.id);
                    if (gmindex !== -1) {
                        $scope.ganttRows[gmindex].hasChildren =true;       
                        $scope.ganttRows[gmindex].raw.hasChildren =true; 
                     }
                }
                catch (e) {
                    debugger
                    alert(e.message);
                }
                    //setTimeout(initTableUI, 300);
                
            }
            // Make sure Angular notices changes
            if (!$scope.$$phase) $scope.$apply();
        })
        .catch(function (err) {
            // On failure, remove the row immediately
            let index = m.tasks.indexOf(newRow);
            if (index > -1) m.tasks.splice(index, 1);
            console.error("Failed to save default milestone", err);
        });
    };
 $scope.addRowUpDown_task_gantt = function ($index, c, position) {
     let mindex = $scope.projectlist.miles.findIndex(f => f.id === c.Milestoneid);
     
        let tindex = $scope.projectlist.miles[mindex].tasks.findIndex(f => f.id === c.id);
        if (tindex !== -1) {
            let c1 =$scope.projectlist.miles[mindex].tasks[tindex];
            $scope.addRowUpDown_task(tindex, $scope.projectlist.miles[mindex], c1, position);
        }

    }
$scope.addRowUpDown_task = function ($index,m,c, position) {
   debugger
if (c.Startdate) {
    if (typeof c.Startdate === 'string' && c.Startdate.indexOf('/Date(') === 0) {
        var timestamp = parseInt(c.Startdate.substr(6));
        c.Startdate = new Date(timestamp);
    } else {
        c.Startdate = new Date(c.Startdate);
    }
}
if (c.Targetdate) {
    if (typeof c.Targetdate === 'string' && c.Targetdate.indexOf('/Date(') === 0) {
        var timestamp = parseInt(c.Targetdate.substr(6));
        c.Targetdate = new Date(timestamp);
    } else {
        c.Targetdate = new Date(c.Targetdate);
    }
}  
$scope.newtasklist.Startdate=c.Startdate
$scope.newtasklist.Targetdate=c.Targetdate
 let newRow = angular.copy($scope.newtasklist);
    let insertIndex,newOrderNo;

    if (position === 'up') {
        insertIndex = $index;        // UI
        newOrderNo = c.orderno;        // DB
    } else {
        insertIndex = $index + 1;    
        newOrderNo = c.orderno + 1;
    }
var nstatus =0;
var nstatus_name = "";
var nstatus_color = "";
try
{
var ststuddefault = ($scope.statuslists || []).find(m => Number(m.isdefault) === 1);
            if (ststuddefault) {
                nstatus = ststuddefault.id;
                nstatus_name = ststuddefault.name;
                nstatus_color = ststuddefault.colorname;
    }


    var npriority = 0;
    var npriority_name = "";
    var npriority_color = "";
    try {
        var prioritydefault = ($scope.prioritylists || []).find(m => Number(m.isdefault) === 1);
        if (prioritydefault) {
            npriority = prioritydefault.id;
            npriority_name = prioritydefault.name;
            npriority_color = prioritydefault.colorname;
        }

    }
    catch
    { }

}
catch
{}
   newRow.P_Status=nstatus;    
   newRow.status_name=nstatus_name;  
    newRow.status_color = nstatus_color;

    newRow.Priority = npriority;
    newRow.priority_name = npriority_name;
    newRow.priority_color = npriority_color;


    newRow.orderno=newOrderNo;
    newRow.ProjectId=c.ProjectId;    
    newRow.Milestoneid = c.Milestoneid; 


    //newRow.P_Status=c.P_Status;    
    //newRow.Priority=c.Priority;  
/*

try
{
newRow.txtdays=getDaysBetween(newRow.Startdate,newRow.Targetdate);
}
catch
{
}*/


    newRow.Owner=c.Owner;    
    newRow.owners=c.owners;    
    newRow.team=c.team;    
    newRow.teams = c.teams;  
    if ($scope.projectlist.hassubtask == 1) { newRow.issubtask = 1; }
   if ($scope.projectlist.checktaskWeightage == 1) { newRow.checkmileWeightage = 1; }
    // Insert in UI
    //$scope.projectlist.miles.tasks.splice(insertIndex, 0, newRow); 
    m.tasks.splice(insertIndex, 0, newRow); 
   // Insert in DB    
    let c1 = angular.copy(c);
    c1.cdate=null;
    c1.iscompletedate=null;
    c1.subtasks=[];
    c1.viewfiles=[];
    $scope.addnewtask(c1,newOrderNo)
    .then(async function(response) {
debugger
       var newid = response;
//let new_weightage = await projectService.getweightage(newid,'task');
let new_weightage = await projectService.getweightage_default(c1.Milestoneid,'task')
newRow.Weightage=new_weightage.data.d;
var gmindex= $scope.projectlist.miles.findIndex(f => f.id ===newRow.Milestoneid);
angular.forEach($scope.projectlist.miles[gmindex].tasks, function (t){
        t.Weightage = new_weightage.data.d;
     });
        //$scope.projectlist.miles.tasks[insertIndex].id=newid;       
        m.tasks[insertIndex].id=newid;       
        $scope.refreshOrderNo_tasks(m);
        if ($scope.view == "gantt") {

            debugger
            let mindex = $scope.projectlist.miles.findIndex(f => f.id === m.id);
            let tindex = $scope.projectlist.miles[mindex].tasks.findIndex(f => f.id === newid);
            var t = $scope.projectlist.miles[mindex].tasks[tindex];
            var mr = $scope.makeGanttRow(t, "task", 1, "m_" + m.id);
            var ganttInsertIndex = 0;
            var milestoneCount = 0;
            for (var i = 0; i < $scope.ganttRows.length; i++) {

                if ($scope.ganttRows[i].type === "task") {
                    if (milestoneCount === tindex) {
                        ganttInsertIndex = i;
                        break;
                    }
                    milestoneCount++;
                }
                ganttInsertIndex = i + 1;
            }
            // ================= INSERT AT SAME POSITION =================
            $scope.ganttRows.splice(ganttInsertIndex, 0, mr);
                    let gmindex = $scope.ganttRows.findIndex(r =>r.type === "milestone" &&r.id == m.id);
                    if (gmindex !== -1) {
                        $scope.ganttRows[gmindex].hasChildren =true;       
                        $scope.ganttRows[gmindex].raw.hasChildren =true; 
                     }
            //$scope.buildGantt();
            //setTimeout(initTableUI, 300);
        }
    
    })
    .catch(function(err) {
debugger
        // If server fails, remove the temporary row
        m.tasks.splice(insertIndex, 1);
        //alert("Failed to save row");
    });
    initTableUI();
};
$scope.refreshOrderNo_tasks = function (m) {
debugger
    if (!m.tasks) return;

    let updatePromises = [];
    angular.forEach(m.tasks, function (item, index) {
        // Update UI order number

        item.orderno = index + 1;
debugger
        // Call service to update DB and store promise
        let promise = projectService.updateorderno_task(item.id, item.orderno)
            .then(function(response) {
debugger
                if (response.data && response.data.d) {
                    console.log('Updated orderno for id:', item.id, 'to', item.orderno);
                } else {
                    console.warn('Update returned false for id:', item.id);
                }
            })
            .catch(function(err){
                console.error('Failed to update orderno for id:', item.id, err);
            });

        updatePromises.push(promise);
    });

    // Wait for all updates to finish
    Promise.all(updatePromises).then(function() {
        console.log('All order numbers updated successfully.');
    });
};
$scope.addnewtask = function(object,orderno) {
debugger
    return projectService.addnewtaskrow(object,orderno)
        .then(function(response) {
            if(response.data.d) {
                return response.data.d;
            } else {
                return $q.reject({message: "Server did not return data"});
            }
        });
};
$scope.deletedatatask = function (index,c,m) {
   debugger
var isupdate="0";
projectService.removetask(c.id)
        .then(async function(response) {debugger
            if(response.data.d) {
                isupdate= response.data.d;
                if(isupdate=="1")
                {
                    debugger
                    var removeid = c.id;
                    let realIndex = m.tasks.findIndex(f => f.id === removeid);
                    if(realIndex !== -1){
                        m.tasks.splice(realIndex, 1);
                        if ($scope.view == "gantt") {
                            debugger
                            $scope.deleteGanttRowById(removeid, 'task');

                        }
                    }
                }
let new_weightage = await projectService.getweightage_default(c.Milestoneid,'task');
       var WeightageN=new_weightage.data.d;
var gmindex= $scope.projectlist.miles.findIndex(f => f.id ===c.Milestoneid);
    angular.forEach($scope.projectlist.miles[gmindex].tasks, function (t){
        t.Weightage = WeightageN;
     });
$scope.$applyAsync();
            } 
        })

   
    
};
//#endregion
//#region manage milestone subtask
$scope.newsubtasklist = {

    id:0,
    ProjectId:0,
    Milestoneid:0,
    Task:0,
    Subtaskname:"",
    SubTaskDescription:"",
    owner:"",
    team:"",
    Startdate:null,
    Targetdate:null,
    Weightage:null,
    StartTime:"",
    Timeline:"",
    TaskDuration: "",
    TaskDuration_min: "",
    P_Status:null,
    status_name:"-",
    status_color:"",
    Priority:null,
    priority_name:"-",
    priority_color:"",
    Budget:null,
    Actual:null,
    Reminder:"",
    ReminderType:null,
    Days:"",
    Time:"",
    uid :null,
    cdate:null,
    tagto:"",
    dependancy:"",
    iscomplete:"",
    iscompletepercentage:0,
    orderno:null,    
    txtdays:"",
    complete_value:"0",
    pending_value:"100",
leftdays:"",
    owners:[],
    teams:[],
    viewfiles:[]
   };
$scope.addRowUpDown_subtaskdefault = function (t) {
debugger
    if (!t.subtasks) {
        t.subtasks = [];
    }

    // Copy the default template
    let newRow = angular.copy($scope.newsubtasklist);

    // Set dates
    if (t.Startdate) {
        
try{
 newRow.Startdate = t.Startdate.indexOf('/Date(') === 0
            ? new Date(parseInt(t.Startdate.substr(6)))
            : new Date(t.Startdate);

}catch{
    newRow.Startdate=new Date(t.Startdate)  
}
    }

    if (t.Targetdate) {
try{
newRow.Targetdate = t.Targetdate.indexOf('/Date(') === 0
            ? new Date(parseInt(t.Targetdate.substr(6)))
            : new Date(t.Targetdate);

}catch{
      newRow.Targetdate=new Date(t.Targetdate)  
}
    }
/*

try
{
newRow.txtdays=getDaysBetween(newRow.Startdate,newRow.Targetdate);
}
catch
{
}*/
    // Copy other properties
  var nstatus =0;
var nstatus_name = "";
var nstatus_color = "";
try
{
var ststuddefault = ($scope.statuslists || []).find(m => Number(m.isdefault) === 1);
            if (ststuddefault) {
                nstatus = ststuddefault.id;
                nstatus_name = ststuddefault.name;
                nstatus_color = ststuddefault.colorname;
            }

    var npriority = 0;
    var npriority_name = "";
    var npriority_color = "";
    try {
        var prioritydefault = ($scope.prioritylists || []).find(m => Number(m.isdefault) === 1);
        if (prioritydefault) {
            npriority = prioritydefault.id;
            npriority_name = prioritydefault.name;
            npriority_color = prioritydefault.colorname;
        }

    }
    catch
    { }

}
catch
{}
newRow.P_Status=nstatus;    
   newRow.status_name=nstatus_name;  
    newRow.status_color = nstatus_color;

    newRow.Priority = npriority;
    newRow.priority_name = npriority_name;
    newRow.priority_color = npriority_color;


    newRow.ProjectId =t.ProjectId;
    newRow.Milestoneid =t.Milestoneid;
    newRow.Task =t.id;

    //newRow.P_Status = t.P_Status;
    //newRow.Priority = t.Priority;
    newRow.Owner =t.Owner;
    newRow.owners =t.owners;
    newRow.team =t.team;   
    newRow.teams =t.teams; 

  

    // Set order number
    newRow.orderno =t.subtasks.length + 1;

    // **Push immediately so UI updates**
    //$scope.projectlist.miles.splice(insertIndex, 0, newRow);
    t.subtasks.push(newRow);

    // Clear temporary fields
    newRow.cdate = null;
    newRow.iscompletedate = null;

    // Save to DB asynchronously
    $scope.addnewsubtask(newRow, newRow.orderno)
        .then(async function (response) {
debugger
            let newid = response;

            // Update the row ID without removing/re-adding
            newRow.id = newid;
//let new_weightage = await projectService.getweightage(newid,'subtask');
let new_weightage = await projectService.getweightage_default(newRow.Task,'subtask');
newRow.Weightage=new_weightage.data.d;
var gmindex= $scope.projectlist.miles.findIndex(f => f.id ===newRow.Milestoneid);
var gtindex= $scope.projectlist.miles[gmindex].tasks.findIndex(f => f.id ===newRow.Task);
angular.forEach($scope.projectlist.miles[gmindex].tasks[gtindex].subtasks, function (s){
        s.Weightage = new_weightage.data.d;
     });
            // Refresh order numbers if needed
            $scope.refreshOrderNo_subtasks(t);
            $scope.toggleTask_default(t);
            if ($scope.view == "gantt") {

                let sindex = t.subtasks.findIndex(f => f.id === newid);
                var s = t.subtasks[sindex];
                var mr = $scope.makeGanttRow(s, "subtask", 2, "t_" + t.id);
                $scope.ganttRows.push(mr);
try
{
 let gtindex = $scope.ganttRows.findIndex(r =>r.type === "task" &&r.id == t.id);
                    if (gtindex !== -1) {
                        $scope.ganttRows[gtindex].hasChildren =true;       
                        $scope.ganttRows[gtindex].raw.hasChildren =true; 
                     }
}
catch{}

                setTimeout(initTableUI, 300);
            }

            // Make sure Angular notices changes
            if (!$scope.$$phase) $scope.$apply();
        })
        .catch(function (err) {
debugger
//alert(err)
            // On failure, remove the row immediately
            let index = t.subtasks.indexOf(newRow);
            if (index > -1) t.subtasks.splice(index, 1);
            console.error("Failed to save default milestone", err);
        });
    };
$scope.addRowUpDown_subtask_gantt = function ($index, c, position) {
        let mindex = $scope.projectlist.miles.findIndex(f => f.id === c.Milestoneid);
        let tindex = $scope.projectlist.miles[mindex].tasks.findIndex(f => f.id === c.Task);
     if (tindex !== -1) {
           let sindex = $scope.projectlist.miles[mindex].tasks[tindex].subtasks.findIndex(f => f.id === c.id);
         if (sindex !== -1) {
             let c1 = $scope.projectlist.miles[mindex].tasks[tindex].subtasks[sindex];
             $scope.addRowUpDown_subtask(sindex,$scope.projectlist.miles[mindex].tasks[tindex],c1, position);
         }
        }

    }
$scope.addRowUpDown_subtask = function ($index,t,c, position) {
   debugger
if (c.Startdate) {
    if (typeof c.Startdate === 'string' && c.Startdate.indexOf('/Date(') === 0) {
        var timestamp = parseInt(c.Startdate.substr(6));
        c.Startdate = new Date(timestamp);
    } else {
        c.Startdate = new Date(c.Startdate);
    }
}
if (c.Targetdate) {
    if (typeof c.Targetdate === 'string' && c.Targetdate.indexOf('/Date(') === 0) {
        var timestamp = parseInt(c.Targetdate.substr(6));
        c.Targetdate = new Date(timestamp);
    } else {
        c.Targetdate = new Date(c.Targetdate);
    }
}  
$scope.newsubtasklist.Startdate=c.Startdate
$scope.newsubtasklist.Targetdate=c.Targetdate

 let newRow = angular.copy($scope.newsubtasklist);
    let insertIndex,newOrderNo;

    if (position === 'up') {
        insertIndex = $index;        // UI
        newOrderNo = c.orderno;        // DB
    } else {
        insertIndex = $index + 1;    
        newOrderNo = c.orderno + 1;
    }
    newRow.orderno=newOrderNo;
    newRow.ProjectId=c.ProjectId;    
    newRow.Milestoneid=c.Milestoneid;    
    newRow.Task = c.Task;  


     var nstatus =0;
var nstatus_name = "";
var nstatus_color = "";
try
{
var ststuddefault = ($scope.statuslists || []).find(m => Number(m.isdefault) === 1);
            if (ststuddefault) {
                nstatus = ststuddefault.id;
                nstatus_name = ststuddefault.name;
                nstatus_color = ststuddefault.colorname;
            }

    var npriority = 0;
    var npriority_name = "";
    var npriority_color = "";
    try {
        var prioritydefault = ($scope.prioritylists || []).find(m => Number(m.isdefault) === 1);
        if (prioritydefault) {
            npriority = prioritydefault.id;
            npriority_name = prioritydefault.name;
            npriority_color = prioritydefault.colorname;
        }

    }
    catch
    { }

}
catch
{}
/*
try
{
newRow.txtdays=getDaysBetween(newRow.Startdate,newRow.Targetdate);
}
catch
{
}*/
newRow.P_Status=nstatus;    
   newRow.status_name=nstatus_name;  
    newRow.status_color = nstatus_color;

    newRow.Priority = npriority;
    newRow.priority_name = npriority_name;
    newRow.priority_color = npriority_color; 



    newRow.Owner=c.Owner;    
    newRow.team = c.team;
    if ($scope.projectlist.checksubtaskWeightage == 1) { newRow.Weightage = c.Weightage; }
   // Insert in UI
    //$scope.projectlist.miles.tasks.splice(insertIndex, 0, newRow); 
    t.subtasks.splice(insertIndex, 0, newRow); 
   // Insert in DB    
  
    c.cdate=null;
    c.iscompletedate=null;
    c.viewfiles=[];
    $scope.addnewsubtask(c,newOrderNo)
    .then(async function(response) {
debugger
       var newid = response;
 //let new_weightage = await projectService.getweightage(newid,'subtask');
 let new_weightage = await projectService.getweightage_default(c.Task,'subtask');
  newRow.Weightage=new_weightage.data.d;
var gmindex= $scope.projectlist.miles.findIndex(f => f.id ===newRow.Milestoneid);
var gtindex= $scope.projectlist.miles[gmindex].tasks.findIndex(f => f.id ===newRow.Task);
angular.forEach($scope.projectlist.miles[gmindex].tasks[gtindex].subtasks, function (s){
        s.Weightage = new_weightage.data.d;
     });
        //$scope.projectlist.miles.tasks[insertIndex].id=newid;       
        t.subtasks[insertIndex].id=newid;       
        $scope.refreshOrderNo_subtasks(t);
        if ($scope.view == "gantt") {
            debugger
            let mindex = $scope.projectlist.miles.findIndex(f => f.id === t.Milestoneid);
            let tindex = $scope.projectlist.miles[mindex].tasks.findIndex(f => f.id === t.id);
            let sindex = $scope.projectlist.miles[mindex].tasks[tindex].subtasks.findIndex(f => f.id === newid);
            var s = $scope.projectlist.miles[mindex].tasks[tindex].subtasks[sindex];
            var mr = $scope.makeGanttRow(s, "subtask", 2, "t_" + t.id);
            var ganttInsertIndex = 0;
            var milestoneCount = 0;
            for (var i = 0; i < $scope.ganttRows.length; i++) {

                if ($scope.ganttRows[i].type === "subtask") {
                    if (milestoneCount === sindex) {
                        ganttInsertIndex = i;
                        break;
                    }
                    milestoneCount++;
                }
                ganttInsertIndex = i + 1;
            }
            // ================= INSERT AT SAME POSITION =================
            $scope.ganttRows.splice(ganttInsertIndex, 0, mr);
            try
            {
             let gtindex = $scope.ganttRows.findIndex(r =>r.type === "task" &&r.id == t.id);
                                if (gtindex !== -1) {
                                    $scope.ganttRows[gtindex].hasChildren =true;       
                                    $scope.ganttRows[gtindex].raw.hasChildren =true; 
                                 }
            }
            catch{}
        }
    
    })
    .catch(function(err) {
debugger
        // If server fails, remove the temporary row
        t.subtasks.splice(insertIndex, 1);
        //alert("Failed to save row");
    });
    
};
$scope.refreshOrderNo_subtasks = function (t) {
debugger
    if (!t.subtasks) return;

    let updatePromises = [];
    angular.forEach(t.subtasks, function (item, index) {
        // Update UI order number

        item.orderno = index + 1;
debugger
        // Call service to update DB and store promise
        let promise = projectService.updateorderno_subtask(item.id, item.orderno)
            .then(function(response) {
debugger
                if (response.data && response.data.d) {
                    console.log('Updated orderno for id:', item.id, 'to', item.orderno);
                } else {
                    console.warn('Update returned false for id:', item.id);
                }
            })
            .catch(function(err){
                console.error('Failed to update orderno for id:', item.id, err);
            });

        updatePromises.push(promise);
    });

    // Wait for all updates to finish
    Promise.all(updatePromises).then(function() {
        console.log('All order numbers updated successfully.');
    });
};
$scope.addnewsubtask = function(object,orderno) {
debugger
    return projectService.addnewsubtaskrow(object,orderno)
        .then(function(response) {
            if(response.data.d) {
                return response.data.d;
            } else {
                return $q.reject({message: "Server did not return data"});
            }
        });
};
$scope.deletedatasubtask = function (index,c,t) {
   debugger
var isupdate="0";
projectService.removesubtask(c.id)
        .then(async function(response) {debugger
            if(response.data.d) {
                isupdate= response.data.d;
                if(isupdate=="1")
                {
                    debugger
                    var removeid = c.id;
                    let realIndex = t.subtasks.findIndex(f => f.id === removeid);
                    if(realIndex !== -1){
                        t.subtasks.splice(realIndex, 1);
                        if ($scope.view == "gantt") {
                            debugger
                            $scope.deleteGanttRowById(removeid, 'subtask');

                        }
                    }
                }
let new_weightage = await projectService.getweightage_default(c.Task,'subtask');
       var WeightageN=new_weightage.data.d;
var gmindex= $scope.projectlist.miles.findIndex(f => f.id ===c.Milestoneid);
var gtindex= $scope.projectlist.miles[gmindex].tasks.findIndex(f => f.id ===c.Task);
    angular.forEach($scope.projectlist.miles[gmindex].tasks[gtindex].subtasks, function (s){
        s.Weightage = new_weightage.data.d;
     });
$timeout(function () {

}, 0);
            } 
        })

 initTableUI(); 
    
};
//#endregion
    //#region add default gantt
    $scope.adddefault_gantt = function (c, utype) {
        debugger
         if (utype == "milestone") {
            let mindex = $scope.projectlist.miles.findIndex(f => f.id === c.id);
             if (mindex !== -1) {
                let c1 = $scope.projectlist.miles[mindex];
                $scope.addRowUpDown_taskdefault(c1);
                
            }
        }
        else if (utype == "task") {
            let mindex = $scope.projectlist.miles.findIndex(f => f.id === c.Milestoneid);
            let tindex = $scope.projectlist.miles[mindex].tasks.findIndex(f => f.id === c.id);
             if (tindex !== -1) {
                 let c1 = $scope.projectlist.miles[mindex].tasks[tindex];
                 $scope.addRowUpDown_subtaskdefault(c1);
             }
            }
        }


    //#endregion
//#region bind lists
$scope.removeowners = function(obj1,obj2,utype) {
    debugger;

    var projectowneres = {
        id: obj1.id,
        Owner:obj2.id,
        Ownertype:utype
    };

    $http({
        method: "POST",
        url: '/pms/projects/default.aspx/removeownerbyid',
        headers: { "Content-Type": "application/json" },
        data: { projectowneres: projectowneres }
    }).then(function(response) {
        debugger;
        if(response.data.d=="1")
        {
         let indx = obj1.owners.findIndex(m => m.id == obj2.id);
        if (indx !== -1) {
         obj1.owners.splice(indx, 1);
//start
if ($scope.view === "gantt") {

    let rid = obj1.id;
    let ownid = obj2.id;
    //MILESTONE
    if (utype === 'milestone') {
        let m = $scope.projectlist.miles.find(x => x.id == rid);
        if (m && m.owners) {
            let idx = m.owners.findIndex(o => o.id == ownid);
            if (idx !== -1) {
                m.owners.splice(idx, 1);
            }
        }
    }
    //TASK
    else if (utype === 'task') {
        let m = $scope.projectlist.miles.find(x => x.id == obj1.Milestoneid);
        if (m && m.tasks) {
            let t = m.tasks.find(x => x.id == rid);
            if (t && t.owners) {
                let idx = t.owners.findIndex(o => o.id == ownid);
                if (idx !== -1) {
                    t.owners.splice(idx, 1);
                }
            }
        }
    }
    // SUBTASK
    else if (utype === 'subtask') {
        let m = $scope.projectlist.miles.find(x => x.id == obj1.Milestoneid);
        if (m && m.tasks) {
            let t = m.tasks.find(x => x.id == obj1.Taskid); // ✅ use Taskid
            if (t && t.subtasks) {
                let s = t.subtasks.find(x => x.id == rid);
                if (s && s.owners) {
                    let idx = s.owners.findIndex(o => o.id == ownid);
                    if (idx !== -1) {
                        s.owners.splice(idx, 1);
                    }
                }
            }
        }
    }
}// end
          }

         
        }
       
    }, function(error) {
        debugger;
        alert("Error: " + (error.data || error.statusText));
    });
initTableUI();
    return false;
};
$scope.removeteams = function(obj1,obj2,utype) {
    debugger;

    var projectteams = {
        id: obj1.id,
        Team:obj2.id,
        Teamtype:utype
    };

    $http({
        method: "POST",
        url: '/pms/projects/default.aspx/removeteambyid',
        headers: { "Content-Type": "application/json" },
        data: { projectteams: projectteams }
    }).then(function(response) {
        debugger;
        if(response.data.d=="1")
        {
         let indx = obj1.teams.findIndex(m => m.id == obj2.id);
         obj1.teams.splice(indx, 1);
$scope.$applyAsync();
           setTimeout(function () {
                initTableUI11();
            }, 300);
        }
       
    }, function(error) {
        debugger;
        alert("Error: " + (error.data || error.statusText));
    });


    return false;
};
$scope.toggleMile_default = function (item) {
debugger
    if(!item.isExpanded)
    {
    item.isExpanded=true;
    }
setTimeout(initTableUI, 300);
};
$scope.toggleTask_default = function (item) {
debugger
    if(!item.isExpanded)
    {
    item.isExpanded=true;
    }
setTimeout(initTableUI, 300);
};

$scope.toggleMilestone = function (item) {
debugger
    item.isExpanded = !item.isExpanded;
setTimeout(initTableUI, 300);
};
$scope.toggleTask = function (item) {
debugger
    item.isExpanded = !item.isExpanded;
setTimeout(initTableUI, 300);
};

//$scope.updatesrc = function($event,mid) {
//debugger
 //   var img = $event.currentTarget.querySelector('img'); // get clicked image
   // if (!img) return;

    //var currentSrc = img.src;

    //if (currentSrc.includes('open.png')) {
       
       // img.src = '/pms/assets/images/close.png';
        //img.alt = 'close';
    //} else {
        //img.src = '/pms/assets/images/open.png';
        //img.alt = 'open';
   // }
//};
$scope.loadtasksbyid = function (id) {
debugger
        //$scope.isLoading = true;
        projectService.gettasklistbyid(id
        ).then(function (response) {
debugger
            $scope.milestonetasklist = response.data.d;
//setTimeout(initTableUI, 300);
angular.forEach($scope.milestonetasklist.tasks, function (c) {
            if (c.StartDate) {

        // Handle ASP.NET /Date(....)/
        if (typeof c.StartDate === 'string' && c.StartDate.indexOf('/Date(') === 0) {
            var timestamp = parseInt(c.StartDate.substr(6));
            c.StartDate = new Date(timestamp);
        } 
        else {
            c.StartDate = new Date(c.StartDate);
        }
    }
        });
 
        }).catch(function (error) {

            alert("HTTP ERROR: " + (error.statusText || error.message || "Unknown error"));
            console.error("HTTP ERROR:", error);
        }).finally(function () {
            //$scope.isLoading = false;
        });
    };
//#endregion



    $scope.startOfDay = function (dt) {
        if (!dt) return null;
        var d = new Date(dt);
        d.setHours(0, 0, 0, 0);
        return d;
    };

    $scope.addDays = function (dt, days) {
        var d = new Date(dt);
        d.setDate(d.getDate() + days);
        return d;
    };

    $scope.diffDays = function (d1, d2) {
        var ms = 1000 * 60 * 60 * 24;
        return Math.round(($scope.startOfDay(d1) - $scope.startOfDay(d2)) / ms);
    };

    $scope.getTaskEnd = function (obj, type) {
        if (!obj) return null;

        if (type === "milestone") {
            if (obj.EndDate) return new Date(obj.EndDate);
            if (obj.StartDate && obj.txtdays) return $scope.addDays(obj.StartDate, parseInt(obj.txtdays, 10) || 0);
            return obj.StartDate ? new Date(obj.StartDate) : null;
        }

        if (type === "task" || type === "subtask") {
            if (obj.Targetdate) return new Date(obj.Targetdate);
            if (obj.Startdate && obj.txtdays) return $scope.addDays(obj.Startdate, parseInt(obj.txtdays, 10) || 0);
            return obj.Startdate ? new Date(obj.Startdate) : null;
        }

        return null;
    };

    $scope.toProgress = function (val) {
        //debugger
        var n = parseFloat(val || 0);
        if (isNaN(n)) n = 0;
        if (n < 0) n = 0;
        if (n > 100) n = 100;
        return n;
    };
$scope.updatedetails_gant = function (c) {

    debugger;
    let resultnew = null;   
    if (c.type === 'milestone') {
        resultnew = projectService.updatedetails_mile_bycol(c.id, 'Milestone', c.name);
    } 
    else if (c.type === 'task') {
        resultnew = projectService.updatedetails_task_bycol(c.id, 'taskname', c.name);
    } 
    else if (c.type === 'subtask') {
        resultnew = projectService.updatedetails_subtask_bycol(c.id, 'Subtaskname', c.name);
    }

    if (!resultnew) return;

    resultnew.then(function (res) {
        debugger;
        if (res.data.d === "1") {
            if ($scope.view === "gantt") {
                let rid = c.id;
                if (c.type === 'milestone') {
                    let mindex = $scope.projectlist.miles.findIndex(m => m.id == rid);
                    if (mindex !== -1) {
                        $scope.projectlist.miles[mindex].Milestone = c.name;
                    }
                }
                else if (c.type === 'task') {
                    let mid = c.Milestoneid;
                    let mindex = $scope.projectlist.miles.findIndex(m => m.id == mid);
                    if (mindex !== -1) {
                        let tindex = $scope.projectlist.miles[mindex].tasks.findIndex(t => t.id == rid);
                        if (tindex !== -1) {
                            $scope.projectlist.miles[mindex].tasks[tindex].taskname = c.name;
                        }
                    }
                }
                else if (c.type === 'subtask') {
                    let mid = c.Milestoneid;
                    let tid = c.Task;
                    let mindex = $scope.projectlist.miles.findIndex(m => m.id == mid);
                    if (mindex !== -1) {
                        let tindex = $scope.projectlist.miles[mindex].tasks.findIndex(t => t.id == tid);
                        if (tindex !== -1) {
                            let sindex = $scope.projectlist.miles[mindex].tasks[tindex].subtasks.findIndex(s => s.id == rid);
                            if (sindex !== -1) {
                                $scope.projectlist.miles[mindex].tasks[tindex].subtasks[sindex].Subtaskname = c.name;
                            }
                        }
                    }
                }
            }
        }

    }).catch(function () {
        alert("Update failed");
    });
};
$scope.loadmilecompletiondetails = function (id) {

    debugger;

    projectService.bindcompletiondetails_mile(id)
        .then(function (response) {

            debugger;

            var mile_listdetails = response.data.d;

            if (mile_listdetails) {
                let mindex = $scope.projectlist.miles.findIndex(m => m.id == id);
                $scope.projectlist.miles[mindex].complete_value = mile_listdetails.complete_value;
                $scope.projectlist.miles[mindex].pending_value = mile_listdetails.pending_value;
                if (mile_listdetails.tasks_details && mile_listdetails.tasks_details.length > 0) {
                    angular.forEach(mile_listdetails.tasks_details, function (t) {
                        let tindex = $scope.projectlist.miles[mindex].tasks.findIndex(m => m.id == t.id);
                        if (tindex !== -1) {

                            // ================= Tasks =================
                            $scope.projectlist.miles[mindex].tasks[tindex].complete_value = t.complete_value;
                            $scope.projectlist.miles[mindex].tasks[tindex].pending_value = t.pending_value;

                            // ================= Subt tasks =================
                            if (t.subtasks_details && t.subtasks_details.length > 0) {

                                angular.forEach(t.subtasks_details, function (s) {

                                    let sindex = $scope.projectlist.miles[mindex].tasks[tindex].subtasks.findIndex(m => m.id == s.id);
                                    if (sindex !== -1) {
                                        $scope.projectlist.miles[mindex].tasks[tindex].subtasks[sindex].complete_value = s.complete_value;
                                        $scope.projectlist.miles[mindex].tasks[tindex].subtasks[sindex].pending_value = s.pending_value;

                                    }
                                });
                            }
                        }
                    });

                }
                $scope.$applyAsync();
            }
            if ($scope.view == "gantt") {
                //updateGanttHierarchyCompletion('milestone',);
                $scope.updateGanttCompletion(id);
            }
        })
        .catch(function (error) {

            alert("HTTP ERROR: " + (error.statusText || error.message || "Unknown error"));
            console.error("HTTP ERROR:", error);

        })
        .finally(function () {
            // $scope.isLoading = false;
        });

    
    };
$scope.loadtaskcompletiondetails = function (id) {

        debugger;

        projectService.bindcompletiondetails_task(id)
            .then(function (response) {

                debugger;

                var task_listdetails = response.data.d;

                if (task_listdetails) {
                   var mid = task_listdetails.Milestoneid;
                    let mindex = $scope.projectlist.miles.findIndex(m => m.id == mid);
                    let tindex = $scope.projectlist.miles[mindex].tasks.findIndex(m => m.id == task_listdetails.id);
                    if (tindex !== -1) {
                        $scope.projectlist.miles[mindex].tasks[tindex].complete_value = task_listdetails.complete_value;
                        $scope.projectlist.miles[mindex].tasks[tindex].pending_value = task_listdetails.pending_value;
                        if (task_listdetails.subtasks_details && task_listdetails.subtasks_details.length > 0) {

                            angular.forEach(task_listdetails.subtasks_details, function (s) {

                                let sindex = $scope.projectlist.miles[mindex].tasks[tindex].subtasks.findIndex(m => m.id == s.id);
                                if (sindex !== -1) {
                                    $scope.projectlist.miles[mindex].tasks[tindex].subtasks[tindex].complete_value = s.complete_value;
                                    $scope.projectlist.miles[mindex].tasks[tindex].subtasks[sindex].pending_value = s.pending_value;

                                }
                            });
                        }
            if ($scope.view == "gantt") {
               $scope.updateGanttCompletion(mid);
            }
                    }
                   
                        $scope.$applyAsync();
                }

            })
            .catch(function (error) {

                alert("HTTP ERROR: " + (error.statusText || error.message || "Unknown error"));
                console.error("HTTP ERROR:", error);

            })
            .finally(function () {
                // $scope.isLoading = false;
            });
    };
$scope.loadsubtaskcompletiondetails = function (id) {

        debugger;

        projectService.bindcompletiondetails_subtask(id)
            .then(function (response) {
                debugger;

                var subtask_listdetails = response.data.d;

                if (subtask_listdetails) {
                    var mid = subtask_listdetails.Milestoneid;
                    var tid = subtask_listdetails.Task;
                    var sid = subtask_listdetails.id;
                    let mindex = $scope.projectlist.miles.findIndex(m => m.id == mid);
                    let tindex = $scope.projectlist.miles[mindex].tasks.findIndex(m => m.id == tid);
                    if (tindex !== -1) {
                        let sindex = $scope.projectlist.miles[mindex].tasks[tindex].subtasks.findIndex(m => m.id ==sid);
                        if (sindex !== -1) {
                            $scope.projectlist.miles[mindex].tasks[tindex].subtasks[tindex].complete_value = subtask_listdetails.complete_value;
                            $scope.projectlist.miles[mindex].tasks[tindex].subtasks[tindex].pending_value = subtask_listdetails.pending_value;

                        }
                    }
                   if ($scope.view == "gantt") {
                   $scope.updateGanttCompletion(mid);
                   }
                        $scope.$applyAsync();
                }

            })
            .catch(function (error) {

                alert("HTTP ERROR: " + (error.statusText || error.message || "Unknown error"));
                console.error("HTTP ERROR:", error);

            })
            .finally(function () {
                // $scope.isLoading = false;
            });
    };
$scope.updateGanttCompletion = function (mileId) {

    // ================= FIND MILESTONE =================
    let mile =($scope.projectlist.miles || []).find(m => m.id == mileId);
    if (!mile) return;

    // =====================================================
    // MILESTONE
    // =====================================================
    let gmindex = $scope.ganttRows.findIndex(r =>
        r.type === "milestone" &&
        r.id == mile.id
    );

    if (gmindex !== -1) {

        $scope.ganttRows[gmindex].status_id = mile.P_Status;
        $scope.ganttRows[gmindex].status_name = mile.status_name;
        $scope.ganttRows[gmindex].status_color = mile.status_color;
        $scope.ganttRows[gmindex].progress =$scope.toProgress(mile.complete_value);
        // UPDATE RAW VALUES
        $scope.ganttRows[gmindex].raw.status_id =mile.P_Status;
        $scope.ganttRows[gmindex].raw.status_name =mile.status_name;
        $scope.ganttRows[gmindex].raw.status_color =mile.status_color;
        $scope.ganttRows[gmindex].raw.complete_value =mile.complete_value;
        $scope.ganttRows[gmindex].raw.pending_value =mile.pending_value;

        // UPDATE PROGRESS BAR
        $scope.ganttRows[gmindex].progress =$scope.toProgress(mile.complete_value||mile.iscompletepercentage);
    }

    // =====================================================
    // TASKS
    // =====================================================

    angular.forEach((mile.tasks || []), function (t) {
debugger
        let gtindex = $scope.ganttRows.findIndex(r =>
            r.type === "task" &&
            r.id == t.id
        );
        if (gtindex !== -1) {
        $scope.ganttRows[gtindex].status_id = t.P_Status;
        $scope.ganttRows[gtindex].status_name = t.status_name;
        $scope.ganttRows[gtindex].status_color = t.status_color;
        $scope.ganttRows[gtindex].progress =$scope.toProgress(t.complete_value);
            // UPDATE RAW VALUES
            $scope.ganttRows[gtindex].raw.status_id =t.P_Status;
            $scope.ganttRows[gtindex].raw.status_name =t.status_name;
            $scope.ganttRows[gtindex].raw.status_color =t.status_color;
            $scope.ganttRows[gtindex].raw.complete_value =t.complete_value;
            $scope.ganttRows[gtindex].raw.pending_value =t.pending_value;
            $scope.ganttRows[gtindex].progress =$scope.toProgress(t.complete_value ||t.iscompletepercentage);
           
            
        }

        // =====================================================
        // SUBTASKS
        // =====================================================

        angular.forEach((t.subtasks || []), function (s) {

            let gsindex = $scope.ganttRows.findIndex(r =>
                r.type === "subtask" &&
                r.id == s.id
            );

            if (gsindex !== -1) {

        $scope.ganttRows[gsindex].status_id = s.P_Status;
        $scope.ganttRows[gsindex].status_name = s.status_name;
        $scope.ganttRows[gsindex].status_color = s.status_color;
        $scope.ganttRows[gsindex].progress =$scope.toProgress(s.complete_value);
                // UPDATE RAW VALUES
        $scope.ganttRows[gsindex].raw.status_id =s.P_Status;
        $scope.ganttRows[gsindex].raw.status_name =s.status_name;
        $scope.ganttRows[gsindex].raw.status_color =s.status_color;
                $scope.ganttRows[gsindex].raw.complete_value =s.complete_value;
                $scope.ganttRows[gsindex].raw.pending_value =s.pending_value;

                // UPDATE PROGRESS
                $scope.ganttRows[gsindex].progress =
                    $scope.toProgress(
                        s.complete_value ||
                        s.iscompletepercentage
                    );
            }
        });
    });

    $scope.$applyAsync();
};
 $scope.updateGanttHierarchyCompletion = function (type, item) {

    function updateRow(rowType, rowId) {

        // FIND GANTT ROW
        let gindex = $scope.ganttRows.findIndex(r =>
            r.type === rowType &&
            r.id == rowId
        );

        if (gindex === -1) return;

        // FIND ACTUAL DATA
        var data = null;

        // ================= MILESTONE =================
        if (rowType === "milestone") {

            data = ($scope.projectlist.miles || [])
                .find(m => m.id == rowId);
        }

        // ================= TASK =================
        else if (rowType === "task") {

            angular.forEach(($scope.projectlist.miles || []), function (m) {

                let t =
                    (m.tasks || [])
                    .find(x => x.id == rowId);

                if (t) {
                    data = t;
                }
            });
        }

        // ================= SUBTASK =================
        else if (rowType === "subtask") {

            angular.forEach(($scope.projectlist.miles || []), function (m) {

                angular.forEach((m.tasks || []), function (t) {

                    let s =
                        (t.subtasks || [])
                        .find(x => x.id == rowId);

                    if (s) {
                        data = s;
                    }
                });
            });
        }

        if (!data) return;

        // UPDATE RAW VALUES
        $scope.ganttRows[gindex].raw.complete_value =
            data.complete_value;

        $scope.ganttRows[gindex].raw.pending_value =
            data.pending_value;

        // UPDATE PROGRESS
        $scope.ganttRows[gindex].progress =
            $scope.toProgress(
                data.complete_value ||
                data.iscompletepercentage
            );
    }

    // =====================================================
    // MILESTONE
    // =====================================================

    if (type === "milestone") {

        updateRow(
            "milestone",
            item.id
        );
    }

    // =====================================================
    // TASK
    // =====================================================

    else if (type === "task") {

        // TASK
        updateRow(
            "task",
            item.id
        );

        // MILESTONE
        updateRow(
            "milestone",
            item.Milestoneid
        );
    }

    // =====================================================
    // SUBTASK
    // =====================================================

    else if (type === "subtask") {

        // SUBTASK
        updateRow(
            "subtask",
            item.id
        );

        // TASK
        updateRow(
            "task",
            item.Task
        );

        // MILESTONE
        updateRow(
            "milestone",
            item.Milestoneid
        );
    }

    $scope.$applyAsync();
};
    $scope.makeGanttRow = function (item, type, level, parentUid) {
        //debugger
        var _start = null;
        var _end = null;
        var _name = "";
        var _owners = [];
        var _status_id = "";
        var _status_name = "";
        var _status_color = "";
        var _progress = 0;
        var _uid = "";
        var _istask =0;
        var _issubtask =0;
        var _ProjectId = "";
        var _Milestoneid = "";
        var _Task = "";
        var _ownerid = "0";
       var _hasChildren=false;

        if (type === "milestone") {
            _start = item.StartDate ? new Date(item.StartDate) : null;
            _end = $scope.getTaskEnd(item, "milestone");
            _name = item.Milestone || "";
            _istask = item.istask || 0;
            _ownerid = item.Owner || "0";
            _owners = item.owners || [];
            _status_id=item.id;
            _status_name = item.status_name || "";
            _status_color = item.status_color || "";
            _progress = $scope.toProgress(item.complete_value || item.iscompletepercentage);
            _uid = "m_" + item.id;
            _ProjectId = item.ProjectId;
            _Milestoneid = item.id;

        }
        else if (type === "task") {
            _start = item.Startdate ? new Date(item.Startdate) : null;
            _end = $scope.getTaskEnd(item, "task");
            _name = item.taskname || "";
            _issubtask = item.istask || 0;
            _ownerid = item.owner || "0";
            _owners = item.owners || [];
            _status_id=item.id;
            _status_name = item.status_name || "";
            _status_color = item.status_color || "";
            _progress = $scope.toProgress(item.complete_value || item.iscompletepercentage);
            _uid = "t_" + item.id;
            _ProjectId = item.ProjectId;
            _Milestoneid = item.Milestoneid;

        }
        else {
            _start = item.Startdate ? new Date(item.Startdate) : null;
            _end = $scope.getTaskEnd(item, "subtask");
            _name = item.Subtaskname || "";
            _ownerid = item.owner||"0";
            _owners = item.owners || [];
            _status_id=item.id;
            _status_name = item.status_name || "";
            _status_color = item.status_color || "";
            _progress = $scope.toProgress(item.complete_value || item.iscompletepercentage);
            _uid = "s_" + item.id;
            _ProjectId = item.ProjectId;
            _Milestoneid = item.Milestoneid;
            _Task =item.Task;

        }

        return {
            uid:_uid,
            id:item.id,
            parentUid:parentUid || null,
            type:type,
            level:level,
            name:_name,
            start:_start,
            end:_end,
            owners:_owners,
            status_id:_status_id,
            status_name:_status_name,
            status_color:_status_color,
            progress:_progress,
            expanded:true,
            visible:true,
            hasChildren:false,
            ProjectId:_ProjectId,
            Milestoneid:_Milestoneid,
            Task:_Task,
            Owner:_ownerid,
            istask:_istask,
            issubtask:_issubtask,
            raw:item
        };
    };

    $scope.buildGanttRows = function () {

        var rows = [];

        angular.forEach(($scope.projectlist.miles || []), function (m) {
            var mr = $scope.makeGanttRow(m, "milestone", 0, null);
            mr.hasChildren = !!(m.tasks && m.tasks.length);
            rows.push(mr);
            angular.forEach((m.tasks || []), function (t) {
                var tr = $scope.makeGanttRow(t, "task", 1, mr.uid);
                tr.hasChildren = !!(t.subtasks && t.subtasks.length);
                rows.push(tr);

                angular.forEach((t.subtasks || []), function (s) {
                    var sr = $scope.makeGanttRow(s, "subtask", 2, tr.uid);
                    rows.push(sr);
                });
            });
        });

        return rows;
    };

    $scope.buildGanttTimeline = function () {
        var rows = $scope.ganttRows;
        var minDate = null;
        var maxDate = null;

        angular.forEach(rows, function (r) {
            if (!r.start || !r.end) return;

            var s = $scope.startOfDay(r.start);
            var e = $scope.startOfDay(r.end);

            if (!minDate || s < minDate) minDate = s;
            if (!maxDate || e > maxDate) maxDate = e;
        });

        if (!minDate || !maxDate) {
            $scope.ganttCells = [];
            $scope.ganttHeaderGroups = [];
            return;
        }

        minDate = $scope.addDays(minDate, -2);
        maxDate = $scope.addDays(maxDate, 2);

        $scope.ganttStart = minDate;
        $scope.ganttEnd = maxDate;

        var cells = [];
        var groups = [];
        var cursor = new Date(minDate);
        //if ($scope.ganttScale === "month") {

        //    $scope.ganttCellWidth = 90;

        //    cells = [];
        //    groups = [];

        //    // current year
        //    var year = new Date().getFullYear();

        //    // OR use project start year
        //    // var year = minDate.getFullYear();

        //    for (var m = 0; m < 12; m++) {

        //        var first = new Date(year, m, 1);

        //        cells.push({
        //            date: first,
        //            label: first.toLocaleDateString('en-GB', {
        //                month: 'short',
        //                year: 'numeric'
        //            })
        //        });
        //    }

        //    groups = [{
        //        label: year,
        //        width: cells.length * $scope.ganttCellWidth
        //    }];
        //}
        //else
            if ($scope.ganttScale === "month") {
            $scope.ganttCellWidth = 90;
            while (cursor <= maxDate) {
                var first = new Date(cursor.getFullYear(), cursor.getMonth(), 1);
                var next = new Date(cursor.getFullYear(), cursor.getMonth() + 1, 1);

                cells.push({
                    date: new Date(first),
                    label: first.toLocaleDateString('en-GB', { month: 'short', year: 'numeric' })
                });

                cursor = next;
            }

            groups = [{
                label: "Timeline",
                width: cells.length * $scope.ganttCellWidth
            }];
        }
        else if ($scope.ganttScale === "week") {

            $scope.ganttCellWidth = 70;

            cells = [];
            groups = [];

            var current = new Date(
                minDate.getFullYear(),
                minDate.getMonth(),
                1
            );

            while (current <= maxDate) {

                var year = current.getFullYear();
                var month = current.getMonth();

                var monthName = current.toLocaleDateString('en-GB', {
                    month: 'long',
                    year: 'numeric'
                });

                var daysInMonth = new Date(year, month + 1, 0).getDate();

                var firstDay = new Date(year, month, 1).getDay();

                var totalWeeks = Math.ceil(
                    (daysInMonth + firstDay) / 7
                );

                for (var w = 1; w <= totalWeeks; w++) {

                    cells.push({
                        date: new Date(year, month, ((w - 1) * 7) + 1),
                        label: "W" + w
                    });
                }

                groups.push({
                    label: monthName,
                    width: totalWeeks * $scope.ganttCellWidth
                });

                current = new Date(year, month + 1, 1);
            }
        }
        else {
            $scope.ganttCellWidth = 40;
debugger
            while (cursor <= maxDate) {
                cells.push({
                    date: new Date(cursor),
                    label: cursor.getDate()
                });
                cursor = $scope.addDays(cursor, 1);
            }

            var gi = 0;
            while (gi < cells.length) {
                var firstCellDate = cells[gi].date;
                var y = firstCellDate.getFullYear();
                var m = firstCellDate.getMonth();
                var count = 0;

                while ((gi + count) < cells.length) {
                    var tempDate = cells[gi + count].date;
                    if (tempDate.getFullYear() !== y || tempDate.getMonth() !== m) break;
                    count++;
                }

                groups.push({
                    label: firstCellDate.toLocaleDateString('en-GB', { month: 'long', year: 'numeric' }),
                    width: count * $scope.ganttCellWidth
                });

                gi += count;
            }
        }

        $scope.ganttCells = cells;
        $scope.ganttHeaderGroups = groups;
    };
    $scope.getMonthWeekIndex = function (date) {

        if (!date) return 0;

        var total = 0;

        var startMonth = new Date(
            $scope.ganttStart.getFullYear(),
            $scope.ganttStart.getMonth(),
            1
        );

        var currentMonth = new Date(
            date.getFullYear(),
            date.getMonth(),
            1
        );

        // count previous month weeks
        while (startMonth < currentMonth) {

            var y = startMonth.getFullYear();
            var m = startMonth.getMonth();

            var daysInMonth = new Date(y, m + 1, 0).getDate();

            var firstDay = new Date(y, m, 1).getDay();

            var totalWeeks = Math.ceil(
                (daysInMonth + firstDay) / 7
            );

            total += totalWeeks;
            startMonth.setMonth(startMonth.getMonth() + 1);
        }

        // current month week
        var firstDayCurrent = new Date(
            date.getFullYear(),
            date.getMonth(),
            1
        ).getDay();

        var weekNo = Math.ceil(
            (date.getDate() + firstDayCurrent) / 7
        );

        total += (weekNo - 1);

        return total;
    };
    $scope.buildGantt11 = function () {
       // alert('hi');
        var allRows = $scope.buildGanttRows();
        $scope.originalRows=allRows;
        $scope.ganttRows =$scope.getVisibleGanttRows(allRows);

        $scope.buildGanttTimeline();
    };
$scope.buildGantt = function () {

    var allRows = $scope.buildGanttRows();

    $scope.ganttRows = $scope.initializeGanttState(allRows);

    $scope.buildGanttTimeline();
//setTimeout(syncGanttRowHeight, 100);
};
$scope.initializeTableState = function () {
debugger
//if(!$scope.originalRows)
   //return;
    angular.forEach($scope.projectlist.miles, function (r) {

        if (r.isExpanded) {
            r.isExpanded = false;
        } 
        if(r.type=="milestone")
        {
debugger
                   let mindex = $scope.projectlist.miles.findIndex(f => f.id===r.id);
                    var mr=$scope.projectlist.miles[mindex];
                    if(mr.isExpanded)
                    {
                       let gmindex = allRows.findIndex(x =>x.type === "milestone" &&x.uid == "m_" +r.id);
                        if (gmindex !== -1) {
                            allRows[gmindex].hasChildren =true;       
                            allRows[gmindex].raw.hasChildren =true; 
                            allRows[gmindex].expanded =true; 
                            allRows[gmindex].raw.expanded =true; 
                         }
                    }
        }   
          
    });
    
};
$scope.initializeGanttState = function (allRows) {
debugger
    angular.forEach(allRows, function (r) {

        if (!r.parentUid) {
            r.expanded = false;
        } else {
            r.expanded = true;
            if(r.hasChildren)
            r.expanded = false;
        }
        if(r.type=="milestone")
        {
debugger
                  let mindex = $scope.projectlist.miles.findIndex(f => f.id===r.id);
                    var mr=$scope.projectlist.miles[mindex];
                    if(mr.isExpanded)
                    {
                       let gmindex = allRows.findIndex(x =>x.type === "milestone" &&x.uid == "m_" +r.id);
                        if (gmindex !== -1) {
                            allRows[gmindex].hasChildren =true;       
                            allRows[gmindex].raw.hasChildren =true; 
                            allRows[gmindex].expanded =true; 
                            allRows[gmindex].raw.expanded =true; 
                         }
                    }
        }   
 else if(r.type=="task")
        {
debugger
                   let mindex = $scope.projectlist.miles.findIndex(f => f.id===r.Milestoneid);
                   let tindex = $scope.projectlist.miles[mindex].tasks.findIndex(f => f.id===r.id);
                    var tr=$scope.projectlist.miles[mindex].tasks[tindex];
                    if(tr.isExpanded)
                    {
                       let gtindex = allRows.findIndex(x =>x.type === "task" &&x.uid == "t_" +r.id);
                        if (gtindex !== -1) {
                            allRows[gtindex].hasChildren =true;       
                            allRows[gtindex].raw.hasChildren =true; 
                            allRows[gtindex].expanded =true; 
                            allRows[gtindex].raw.expanded =true; 
                         }
                    }
        }         
    });
    $scope.originalRows = allRows;
    return $scope.getVisibleGanttRows(allRows);
};
    $scope.getVisibleGanttRows = function (allRows) {
        var rowMap = {};

        angular.forEach(allRows, function (r) {
            rowMap[r.uid] = r;
        });

        function isVisible(row) {
            var parentUid = row.parentUid;

            while (parentUid) {
                var parent = rowMap[parentUid];
                if (!parent) break;
                if (parent.expanded === false) return false;
                parentUid = parent.parentUid;
            }

            return true;
        }

        return allRows.filter(function (r) {
            return isVisible(r);
        });
    };


//#region Gantt row soft drag and drop
$scope.ganttDraggingRow = null;
$scope.ganttDragOverRow = null;
$scope.ganttInvalidDropRow = null;

$scope.clearGanttSoftDragMarks = function () {
    $scope.ganttDragOverRow = null;
    $scope.ganttInvalidDropRow = null;
    try {
        var rows = document.querySelectorAll('.gantt-row-over,.gantt-row-invalid,.gantt-row-moving');
        angular.forEach(rows, function (r) {
            r.classList.remove('gantt-row-over');
            r.classList.remove('gantt-row-invalid');
            r.classList.remove('gantt-row-moving');
        });
    } catch (e) { }
};

$scope.canDropGanttRow = function (sourceRow, targetRow) {
    if (!sourceRow || !targetRow) return false;
    if (sourceRow.uid === targetRow.uid) return false;

    // Same hierarchy validation as table view:
    // 1) Milestone can be reordered only with milestone rows.
    // 2) Task can be reordered only with task rows inside the SAME milestone.
    // 3) Subtask can be reordered only with subtask rows inside the SAME task.
    if (sourceRow.type === "milestone") {
        return targetRow.type === "milestone";
    }

    if (sourceRow.type === "task") {
        return targetRow.type === "task" &&
            String(sourceRow.parentId || sourceRow.Milestoneid || sourceRow.milestoneid || "") ===
            String(targetRow.parentId || targetRow.Milestoneid || targetRow.milestoneid || "");
    }

    if (sourceRow.type === "subtask") {
        return targetRow.type === "subtask" &&
            String(sourceRow.parentId || sourceRow.Task || sourceRow.task || "") ===
            String(targetRow.parentId || targetRow.Task || targetRow.task || "");
    }

    return false;
};

$scope.startSoftGanttRowDrag = function ($event, sourceRow) {
    var e = $event && ($event.originalEvent || $event);
    if (!e || !sourceRow || !sourceRow.uid) return;
    if (e.type === 'mousedown' && e.button !== 0) return;

    var p = getDragPoint(e);
    if (e.preventDefault) e.preventDefault();
    if (e.stopPropagation) e.stopPropagation();

    var startEl = closestGanttRow(e.target);
    $scope.ganttDraggingRow = sourceRow;
    $scope.ganttDragOverRow = null;
    $scope.ganttInvalidDropRow = null;

    var ghost = document.createElement('div');
    ghost.className = 'gantt-soft-ghost';
    ghost.innerHTML = '<i class="ri-drag-move-2-line"></i> ' + htmlEncode(sourceRow.name || sourceRow.type || 'Move row');
    document.body.appendChild(ghost);
    document.body.classList.add('gantt-soft-dragging');
    if (startEl) startEl.classList.add('gantt-row-moving');

    function getDragPoint(ev) {
        if (ev.touches && ev.touches.length) return { x: ev.touches[0].clientX, y: ev.touches[0].clientY };
        if (ev.changedTouches && ev.changedTouches.length) return { x: ev.changedTouches[0].clientX, y: ev.changedTouches[0].clientY };
        return { x: ev.clientX, y: ev.clientY };
    }
    function htmlEncode(v) {
        return String(v || '').replace(/[&<>"]/g, function (c) {
            return ({ '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;' })[c];
        });
    }
    function closestGanttRow(el) {
        while (el && el !== document) {
            if (el.getAttribute && el.getAttribute('data-gantt-row')) return el;
            el = el.parentNode;
        }
        return null;
    }
    function findRowByUid(uid) {
        var found = null;
        angular.forEach($scope.ganttRows || [], function (r) {
            if (!found && String(r.uid) === String(uid)) found = r;
        });
        return found;
    }
    function markPair(uid, cls) {
        var els = document.querySelectorAll('[data-gantt-row="' + String(uid).replace(/"/g, '\\"') + '"]');
        angular.forEach(els, function (el) { el.classList.add(cls); });
    }
    function clearDomMarks() {
        var rows = document.querySelectorAll('.gantt-row-over,.gantt-row-invalid,.gantt-row-moving');
        angular.forEach(rows, function (r) {
            r.classList.remove('gantt-row-over');
            r.classList.remove('gantt-row-invalid');
            r.classList.remove('gantt-row-moving');
        });
        if (sourceRow && sourceRow.uid) markPair(sourceRow.uid, 'gantt-row-moving');
    }
    function setGhost(x, y) {
        ghost.style.left = (x + 14) + 'px';
        ghost.style.top = (y + 14) + 'px';
    }

    var targetRow = null;
    function onMove(ev) {
        if (ev.cancelable !== false && ev.preventDefault) ev.preventDefault();
        var pt = getDragPoint(ev);
        setGhost(pt.x, pt.y);
        clearDomMarks();

        var el = document.elementFromPoint(pt.x, pt.y);
        var rowEl = closestGanttRow(el);
        targetRow = null;
        $scope.ganttDragOverRow = null;
        $scope.ganttInvalidDropRow = null;

        if (!rowEl) return;
        var uid = rowEl.getAttribute('data-gantt-row');
        var tr = findRowByUid(uid);
        if (!tr || tr.uid === sourceRow.uid) return;

        targetRow = tr;
        if ($scope.canDropGanttRow(sourceRow, tr)) {
            markPair(tr.uid, 'gantt-row-over');
            if (ghost) ghost.classList.remove('invalid-drop');
            $scope.ganttDragOverRow = tr;
        } else {
            markPair(tr.uid, 'gantt-row-invalid');
            if (ghost) ghost.classList.add('invalid-drop');
            $scope.ganttInvalidDropRow = tr;
        }
    }

    function finish(ev) {
        if (ev && ev.cancelable !== false && ev.preventDefault) ev.preventDefault();
        document.removeEventListener('mousemove', onMove, true);
        document.removeEventListener('mouseup', finish, true);
        document.removeEventListener('touchmove', onMove, true);
        document.removeEventListener('touchend', finish, true);
        document.removeEventListener('touchcancel', finish, true);
        document.body.classList.remove('gantt-soft-dragging');
        if (ghost && ghost.parentNode) ghost.parentNode.removeChild(ghost);
        clearDomMarks();

        var dropTarget = targetRow;
        $scope.$applyAsync(function () {
            if (dropTarget && $scope.canDropGanttRow(sourceRow, dropTarget)) {
                if (sourceRow.type === 'milestone') $scope.moveGanttMilestone(sourceRow, dropTarget);
                else if (sourceRow.type === 'task') $scope.moveGanttTask(sourceRow, dropTarget);
                else if (sourceRow.type === 'subtask') $scope.moveGanttSubtask(sourceRow, dropTarget);
                $scope.rebuildGanttAfterRowDrop();
            }
            $scope.ganttDraggingRow = null;
            $scope.clearGanttSoftDragMarks();
        });
    }

    setGhost(p.x, p.y);
    document.addEventListener('mousemove', onMove, true);
    document.addEventListener('mouseup', finish, true);
    document.addEventListener('touchmove', onMove, { capture: true, passive: false });
    document.addEventListener('touchend', finish, true);
    document.addEventListener('touchcancel', finish, true);
};

$scope.findGanttMilestoneIndex = function (mid) {
    if (!$scope.projectlist || !$scope.projectlist.miles) return -1;
    return $scope.projectlist.miles.findIndex(function (m) { return String(m.id) === String(mid); });
};

$scope.findGanttTaskLocation = function (tid) {
    var result = { mIndex: -1, tIndex: -1, milestone: null, task: null };
    angular.forEach(($scope.projectlist.miles || []), function (m, mi) {
        if (result.task) return;
        var ti = (m.tasks || []).findIndex(function (t) { return String(t.id) === String(tid); });
        if (ti !== -1) { result.mIndex = mi; result.tIndex = ti; result.milestone = m; result.task = m.tasks[ti]; }
    });
    return result;
};

$scope.findGanttSubtaskLocation = function (sid) {
    var result = { mIndex: -1, tIndex: -1, sIndex: -1, milestone: null, task: null, subtask: null };
    angular.forEach(($scope.projectlist.miles || []), function (m, mi) {
        if (result.subtask) return;
        angular.forEach((m.tasks || []), function (t, ti) {
            if (result.subtask) return;
            var si = (t.subtasks || []).findIndex(function (s) { return String(s.id) === String(sid); });
            if (si !== -1) { result.mIndex = mi; result.tIndex = ti; result.sIndex = si; result.milestone = m; result.task = t; result.subtask = t.subtasks[si]; }
        });
    });
    return result;
};

$scope.moveGanttMilestone = function (sourceRow, targetRow) {
debugger
    var list = $scope.projectlist.miles || [];
    var fromIndex = $scope.findGanttMilestoneIndex(sourceRow.id);
    var toIndex = $scope.findGanttMilestoneIndex(targetRow.id);
    if (fromIndex < 0 || toIndex < 0 || fromIndex === toIndex) return;
    var item = list.splice(fromIndex, 1)[0];
    if (fromIndex < toIndex) toIndex--;
    list.splice(toIndex, 0, item);
    $scope.refreshOrderNo();
};

$scope.moveGanttTask = function (sourceRow, targetRow) {
debugger
    var from = $scope.findGanttTaskLocation(sourceRow.id);
    var target = $scope.findGanttTaskLocation(targetRow.id);
    if (!from.task || !target.task) return;
    if (String(from.milestone.id) !== String(target.milestone.id)) return;

    var list = from.milestone.tasks || [];
    var fromIndex = from.tIndex;
    var toIndex = target.tIndex;
    if (fromIndex < 0 || toIndex < 0 || fromIndex === toIndex) return;

    var moved = list.splice(fromIndex, 1)[0];
    if (fromIndex < toIndex) toIndex--;
    list.splice(toIndex, 0, moved);
    $scope.refreshOrderNo_tasks(from.milestone);
};

$scope.moveGanttSubtask = function (sourceRow, targetRow) {
debugger
    var from = $scope.findGanttSubtaskLocation(sourceRow.id);
    var target = $scope.findGanttSubtaskLocation(targetRow.id);
    if (!from.subtask || !target.subtask) return;
    if (String(from.task.id) !== String(target.task.id)) return;

    var list = from.task.subtasks || [];
    var fromIndex = from.sIndex;
    var toIndex = target.sIndex;
    if (fromIndex < 0 || toIndex < 0 || fromIndex === toIndex) return;

    var moved = list.splice(fromIndex, 1)[0];
    if (fromIndex < toIndex) toIndex--;
    list.splice(toIndex, 0, moved);
    $scope.refreshOrderNo_subtasks(from.task);
};

$scope.rebuildGanttAfterRowDrop = function () {
    var expandedMap = {};
    angular.forEach($scope.originalRows || [], function (r) { expandedMap[r.uid] = r.expanded; });
    var allRows = $scope.buildGanttRows();
    angular.forEach(allRows, function (r) { if (expandedMap.hasOwnProperty(r.uid)) r.expanded = expandedMap[r.uid]; });
    $scope.originalRows = allRows;
    $scope.ganttRows = $scope.getVisibleGanttRows(allRows);
    $scope.buildGanttTimeline();
    $timeout(function () { initTableUI(); }, 0);
};
//#endregion Gantt row soft drag and drop

$scope.originalRows=[];
$scope.changeGanttScale= function (type) {
        debugger;
        $scope.ganttScale = type; 
        $scope.isLoading = true;       
        $scope.buildGantt();
        $scope.isLoading = false;
        setTimeout(initTableUI, 300);
    };
$scope.$watch('ganttRows', function () {
    setTimeout(initTableUI, 300);
}, true);

$scope.toggleGanttRow = function (row, $event) {
debugger
    if ($event) $event.stopPropagation();

    row.expanded = !row.expanded;

    $scope.ganttRows = $scope.originalRows.filter(function (r) {
        return isVisible(r, $scope.originalRows);
    });

    $scope.buildGanttTimeline();

    if (!$scope.$$phase) {
        $scope.$applyAsync();
    }
function isVisible(r, allRows) {

    var parentId = r.parentUid;

    while (parentId) {

        var parent = null;

        for (var i = 0; i < allRows.length; i++) {
            if (allRows[i].uid === parentId) {
                parent = allRows[i];
                break;
            }
        }
        if (parent && parent.expanded === false) {
            return false;
        }
        parentId = parent ? parent.parentUid : null;
    }
    return true;
}
};
    $scope.toggleGanttRowold = function (row, $event) {
debugger
        if ($event) $event.stopPropagation();
        row.expanded = !row.expanded;
        var allRows = $scope.buildGanttRows();
        var collapseMap = {};
        function isVisible(r) {
            var p = r.parentUid;
            while (p) {
                if (collapseMap[p] === true) return false;
                var parent = null;
                for (var i = 0; i < allRows.length; i++) {
                    if (allRows[i].uid === p) {
                        parent = allRows[i];
                        break;
                    }
                }
                p = parent ? parent.parentUid : null;
            }
            return true;
        }

        angular.forEach(allRows, function (r) {
            if (r.uid === row.uid) {
                r.expanded = row.expanded;
            }
        });

        angular.forEach(allRows, function (r) {
            if (!r.expanded) collapseMap[r.uid] = true;
        });

        $scope.ganttRows = allRows.filter(function (r) {
            return isVisible(r);
        });

        $scope.buildGanttTimeline();
    };
  

    $scope.getBarStyle = function (row) {
        if (!row.start || !row.end || !$scope.ganttStart) return {};

        var leftIndex = 0;
        var widthUnits = 1;

        if ($scope.ganttScale === "month") {
            leftIndex = (row.start.getFullYear() - $scope.ganttStart.getFullYear()) * 12 +
                (row.start.getMonth() - $scope.ganttStart.getMonth());

            widthUnits = ((row.end.getFullYear() - row.start.getFullYear()) * 12) +
                (row.end.getMonth() - row.start.getMonth()) + 1;
        }
        else if ($scope.ganttScale === "week11") {
            leftIndex = Math.floor($scope.diffDays(row.start, $scope.ganttStart) / 7);
            widthUnits = Math.floor($scope.diffDays(row.end, row.start) / 7) + 1;
        }
        else if ($scope.ganttScale === "week") {

            var startIndex = $scope.getMonthWeekIndex(row.start);

            var endIndex = $scope.getMonthWeekIndex(row.end);

            leftIndex = startIndex;

            widthUnits = (endIndex - startIndex) + 1;
        }
        else {
            leftIndex = $scope.diffDays(row.start, $scope.ganttStart);
            widthUnits = $scope.diffDays(row.end, row.start) + 1;
        }

        if (widthUnits < 1) widthUnits = 1;
        var left = (leftIndex * $scope.ganttCellWidth);

        var width = (widthUnits * $scope.ganttCellWidth);

        // total gantt width
        var maxWidth = $scope.ganttCells.length * $scope.ganttCellWidth;

        // STOP OVERFLOW RIGHT
        if ((left + width) > maxWidth) {

            width = maxWidth - left;
        }

        // STOP NEGATIVE
        if (width < $scope.ganttCellWidth) {

            width = $scope.ganttCellWidth;
        }

        return {
            left: left + "px",
            width: width + "px"
        };
        //return {
        //    left: (leftIndex * $scope.ganttCellWidth) + "px",
        //    width: (widthUnits * $scope.ganttCellWidth) + "px"
        //};
    };  

    $scope.changeView = function (type, $event) {
        if ($event) {
            $event.preventDefault();
            $event.stopPropagation();
        }

        $scope.view = type;
        // load gantt when needed
        if (type === "gantt") {
            $timeout(function () {
                $scope.changeGanttScale($scope.ganttScale);
            }, 0);
        }
        else {
            $timeout(function () {
                initTableUI();
            }, 150);
        }
let dropdown = bootstrap.Dropdown.getInstance(
        document.getElementById('btnGroupDrop1')
    );
    if (dropdown) {
        dropdown.hide();
    }
   console.log("View changed:", $scope.view); // debug
    };   
$scope.showTooltip = false;
$scope.toggleTooltip = function ($event) {
    $event.stopPropagation(); 
    $scope.showTooltip = !$scope.showTooltip;
};


$document.on("click", function () {
    $scope.$apply(function () {
        $scope.showTooltip = false;
    });
});


$scope.$on('$destroy', function () {
    $document.off("click");
});

    $scope.resetStartDateHierarchy = function (node) {

        function reset(item) {
            debugger
            if (!item) return;
            item.StartDate = baseDate;
            item.Startdate = baseDate;
            if (item.tasks && item.tasks.length > 0) {
                item.tasks.forEach(function (t) {
                    // baseDate = t.StartDate || t.Startdate;
                    reset(t);
                });
            }
           if (item.subtasks && item.subtasks.length > 0) {
                item.subtasks.forEach(function (s) {
                    //baseDate = s.StartDate || s.Startdate;
                    reset(s);
                });
            }
            

        }
        var baseDate = node.StartDate || node.Startdate;
        if (!baseDate) return;
        reset(node);

       
    };

   
    $scope.resetStartDateHierarchy_subtask = async function (node, subtasks, index, updatefrom) {
        debugger
        function normalize(d) {
            if (!d) return null;
            d = new Date(d);
            d.setHours(0, 0, 0, 0);
            return d;
        }
        async function reset_subtasks(item, startIndex) {
            if (!item || !item.subtasks || item.subtasks.length === 0) return;

            let subtaskStart = normalize(item.Startdate);
            let subtaskEnd = normalize(item.Targetdate);
            let baseDate = normalize(node.Startdate);
            if (!baseDate) return;
            // tasks           
                // subtasks
                debugger
            for (var z = startIndex; z < item.subtasks.length; z++) {
                        debugger                       
                let start_subtask = normalize(baseDate);
                
                let currentSubTask = item.subtasks[z]
                       currentSubTask.Startdate = new Date(baseDate);
                        // validate sub task start date
                        if (start_subtask && subtaskStart && start_subtask < subtaskStart) {
                            alert("Sub task start date cannot be before Task start date");
                            if (updatefrom === "duration") {
                                currentSubTask.txtdays = currentSubTask._oldtxtdays || null;
                            } else {
                                currentSubTask.Startdate = currentSubTask._oldStartdate || null;
                            }
                            return;
                        }
                        if (start_subtask && subtaskEnd && start_subtask > subtaskEnd) {
                            alert("Sub task start date cannot be after Task end date");
                            if (updatefrom === "duration") {
                                currentSubTask.txtdays = currentSubTask._oldtxtdays || null;
                            } else {
                                currentSubTask.Startdate = currentSubTask._oldStartdate || null;
                            }
                            return;
                       }
                
                        // calculate subtask end date
                let response2 = await projectService.getnew_enddate(currentSubTask.Startdate, currentSubTask.txtdays);

                        if (!response2 || !response2.data || !response2.data.d) {
                            alert("Failed to calculate end date");
                            return;
                        }
                       let end2 = normalize(parseDotNetDate(response2.data.d));
                         currentSubTask.Targetdate = end2;
                if (end2 && subtaskEnd && end2 > subtaskEnd) {
                    alert("Subt task end date cannot be after Task end date");
                    if (updatefrom === "duration") {
                        currentSubTask.txtdays = currentSubTask._oldtxtdays || null;
                    } else {
                        currentSubTask.Enddate = currentSubTask._oldEnddate || null;
                    }
                    return;
                }
                else if (start_subtask && end2 && end2 < start_subtask) {
                    alert("End date cannot be before Start date");

                    if (updatefrom === "duration") {
                        currentSubTask.txtdays = currentSubTask._oldtxtdays || null;
                    } else {
                        currentSubTask.Targetdate = currentSubTask._oldTargetdate || null;
                    }
                    return;
                }
                else {
                    debugger
                    let resultnew;
                    resultnew = projectService.updatetimeline_subtask(currentSubTask.id, currentSubTask.txtdays, new Date(currentSubTask.Startdate));
                    debugger

                    if (!resultnew) return;

                    resultnew.then(function (res) {
                        let newDate = parseDotNetDate(res.data.d);
                        // alert(newDate);
                        //c.Targetdate = newDate;
                        // get left days
                        return projectService.getleftdays(currentSubTask.id, 'subtask');
                    }).then(function (resdays) {
                        if (resdays && resdays.data && resdays.data.d) {
                            currentSubTask.leftdays = resdays.data.d;
                        }
                    }).catch(function (err) {
                        //alert(err);
                        console.error("Update failed", err);
                        return;
                    });
                }

                       
                //baseDate = new Date(end2);
                baseDate = new Date(end2);
                baseDate.setDate(baseDate.getDate() + 1);
                    }
                
            


        }

        await reset_subtasks(subtasks, index, '');
    };
    
    $scope.resetStartDateHierarchy_task = async function (node, tasks, index, updatefrom) {
        debugger
        function normalize(d) {
            if (!d) return null;
            d = new Date(d);
            d.setHours(0, 0, 0, 0);
            return d;
        }

        async function reset_tasks(item, startIndex) {
            if (!item || !item.tasks || item.tasks.length === 0) return;

           
           
            let mileStart = normalize(item.StartDate);
            let mileEnd = normalize(item.EndDate);
            let baseDate = normalize(node.StartDate || node.Startdate);
            if (!baseDate) return;

           
                // tasks
                debugger
               
            for (var y = startIndex; y < item.tasks.length; y++) {
                let currentTask = item.tasks[y];
                let start_task = normalize(baseDate);
                currentTask.Startdate = new Date(baseDate);
                        if (start_task && mileStart && start_task < mileStart) {
                            alert("Task start date cannot be before Milestone start date");

                            if (updatefrom === "duration") {
                                currentTask.txtdays = currentTask._oldtxtdays || null;
                            } else {
                                currentTask.Startdate = currentTask._oldStartdate || null;
                            }
                            return;
                        }
                        if (start_task && mileEnd && start_task > mileEnd) {
                            alert("Task start date cannot be after Milestone end date");

                            if (updatefrom === "duration") {
                                currentTask.txtdays = currentTask._oldtxtdays || null;
                            } else {
                                currentTask.Startdate = currentTask._oldStartdate || null;
                            }
                            return;
                        }
                
                        // calculate task end date
                        let response1 = await projectService.getnew_enddate(currentTask.Startdate, currentTask.txtdays);

                        if (!response1 || !response1.data || !response1.data.d) {
                            alert("Failed to calculate end date");
                            return;
                        }
                let end1 = normalize(parseDotNetDate(response1.data.d));
                currentTask.Targetdate = end1;
                        if (end1 && mileEnd && end1 > mileEnd) {
                            alert("Task end date cannot be after Milestone end date");

                            if (updatefrom === "duration") {
                                currentTask.txtdays = currentTask._oldtxtdays || null;
                            } else {
                                currentTask.Targetdate = currentTask._oldTargetdate || null;
                            }
                            return;
                        }
                        else if (start_task && end1 && end1 < start_task) {
                            alert("End date cannot be before Start date");

                            if (updatefrom === "duration") {
                                currentTask.txtdays = currentTask._oldtxtdays || null;
                            } else {
                                currentTask.Targetdate = currentTask._oldTargetdate || null;
                            }
                            return;
                        } else {
                            debugger
                            let resultnew;
                            resultnew = projectService.updatetimeline_task(currentTask.id, currentTask.txtdays, currentTask.Startdate);
                            debugger

                            if (!resultnew) return;

                            resultnew.then(function (res) {
                                let newDate = parseDotNetDate(res.data.d);
                                
                                return projectService.getleftdays(currentTask.id, 'task');
                            }).then(function (resdays) {
                                if (resdays && resdays.data && resdays.data.d) {
                                    currentTask.leftdays = resdays.data.d;
                                }
                            }).catch(function (err) {
                                alert(err);
                                console.error("Update failed", err);
                                return;
                            });
                        }
                        
                //baseDate = new Date(end1);
                baseDate = new Date(end1);
                baseDate.setDate(baseDate.getDate() + 1);
                        // subtasks
                        debugger
                        if (currentTask.subtasks && currentTask.subtasks.length > 0) {
                            let subtaskBaseDate = normalize(currentTask.Startdate);
                            let subtaskStart = normalize(currentTask.Startdate);
                            let subtaskEnd = normalize(currentTask.Targetdate);
                            
                            for (var z = 0; z < currentTask.subtasks.length; z++) {
                                debugger
                                let start_subtask = normalize(subtaskBaseDate);
                                let currentSubTask = currentTask.subtasks[z]
                                // validate sub task start date
                                if (start_subtask && subtaskStart && start_subtask < subtaskStart) {
                                    alert("Sub task start date cannot be before Task start date");

                                    if (updatefrom === "duration") {
                                        currentSubTask.txtdays = currentSubTask._oldtxtdays || null;
                                    } else {
                                        currentSubTask.Startdate = currentSubTask._oldStartdate || null;
                                    }
                                    return;
                                }

                                if (start_subtask && subtaskEnd && start_subtask > subtaskEnd) {
                                    alert("Sub task start date cannot be after Task end date");

                                    if (updatefrom === "duration") {
                                        currentSubTask.txtdays = currentSubTask._oldtxtdays || null;
                                    } else {
                                        currentSubTask.Startdate = currentSubTask._oldStartdate || null;
                                    }
                                    return;
                                }
                                currentSubTask.Startdate = new Date(subtaskBaseDate);
                                // calculate subtask end date
                                let response2 = await projectService.getnew_enddate(currentSubTask.Startdate, currentSubTask.txtdays);

                                if (!response2 || !response2.data || !response2.data.d) {
                                    alert("Failed to calculate end date");
                                    return;
                                }
                                let end2 = normalize(parseDotNetDate(response2.data.d));
                                currentTask.subtasks[z].Targetdate = end2;
                                if (end2 && subtaskEnd && end2 > subtaskEnd) {
                                    alert("Subt task end date cannot be after Task end date");

                                    if (updatefrom === "duration") {
                                        currentSubTask.txtdays = currentSubTask._oldtxtdays || null;
                                    } else {
                                        currentSubTask.Targetdate = currentSubTask._oldTargetdate || null;
                                    }
                                    return;
                                }
                                else if (start_subtask && end2 && end2 < start_subtask) {
                                    alert("End date cannot be before Start date");

                                    if (updatefrom === "duration") {
                                        currentSubTask.txtdays = currentSubTask._oldtxtdays || null;
                                    } else {
                                        currentSubTask.Targetdate = currentSubTask._oldTargetdate || null;
                                    }
                                    return;
                                }
                                else {
                                    debugger
                                    let resultnew;
                                    resultnew = projectService.updatetimeline_subtask(currentSubTask.id, currentSubTask.txtdays, new Date(currentSubTask.Startdate));
                                    debugger

                                    if (!resultnew) return;

                                    resultnew.then(function (res) {
                                        let newDate = parseDotNetDate(res.data.d);
                                        // alert(newDate);
                                        //c.Targetdate = newDate;
                                        // get left days
                                        return projectService.getleftdays(currentSubTask.id, 'subtask');
                                    }).then(function (resdays) {
                                        if (resdays && resdays.data && resdays.data.d) {
                                            currentSubTask.leftdays = resdays.data.d;
                                        }
                                    }).catch(function (err) {
                                        //alert(err);
                                        console.error("Update failed", err);
                                        return;
                                    });
                                }                                
                                //subtaskBaseDate = new Date(end2);
                                subtaskBaseDate = new Date(end2);
                                subtaskBaseDate.setDate(subtaskBaseDate.getDate() + 1);
                            }
                        }
                    }
                
            
        }

        await reset_tasks(tasks, index, '');
    };


    $scope.resetStartDateHierarchy_mile = async function (node, projectlist, index,updatefrom) {
debugger
        function normalize(d) {
            if (!d) return null;
            d = new Date(d);
            d.setHours(0, 0, 0, 0);
            return d;
        }

        async function reset_miles(item, startIndex) {
            if (!item || !item.miles || item.miles.length === 0) return;

            let projectStart = normalize(parseDotNetDate($scope.projectlist.StartDate));
            let projectEnd = normalize(parseDotNetDate($scope.projectlist.TargetDate));

            let baseDate = normalize(node.StartDate || node.Startdate);
            if (!baseDate) return;

            for (var x = startIndex; x < item.miles.length; x++) {debugger
                let currentMile = item.miles[x];

                currentMile.StartDate = new Date(baseDate);
                let start = normalize(currentMile.StartDate);
                // validate start date
                if (start && projectStart && start < projectStart) {
                    alert("Milestone start date cannot be before Project start date");

                    if (updatefrom === "duration") {
                        currentMile.txtdays = currentMile._oldtxtdays || null;
                    } else {
                        currentMile.StartDate = currentMile._oldStartDate || null;
                    }
                    return;
                }
                else if (start && projectEnd && start > projectEnd) {
                    alert("Milestone start date cannot be after Project end date");

                    if (updatefrom === "duration") {
                        currentMile.txtdays = currentMile._oldtxtdays || null;
                    } else {
                        currentMile.StartDate = currentMile._oldStartDate || null;
                    }
                    return;
                }

                // calculate end date
                let response = await projectService.getnew_enddate(start, currentMile.txtdays);

                if (!response || !response.data || !response.data.d) {
                    alert("Failed to calculate end date");
                    return;
                }

                let end = normalize(parseDotNetDate(response.data.d));
                currentMile.EndDate = end;

                // validate end date
                if (end && projectEnd && end > projectEnd) {
                    alert("Milestone end date cannot be after Project end date");

                    if (updatefrom === "duration") {
                        currentMile.txtdays = currentMile._oldtxtdays || null;
                    } else {
                        currentMile.EndDate = currentMile._oldEndDate || null;
                    }
                    return;
                }

                else if (start && end && end < start) {
                    alert("End date cannot be before Start date");

                    if (updatefrom === "duration") {
                        currentMile.txtdays = currentMile._oldtxtdays || null;
                    } else {
                        currentMile.EndDate = currentMile._oldEndDate || null;
                    }
                    return;
                }

                else {
                    debugger
                    let resultnew;
                    resultnew = projectService.updatetimeline_mile(currentMile.id, currentMile.txtdays, currentMile.StartDate);
                    debugger

                    if (!resultnew) return;

                    resultnew.then(function (res) {
                        let newDate = parseDotNetDate(res.data.d);
                        
                        // get left days
                        return projectService.getleftdays(currentMile.id, 'milestone');
                    }).then(function (resdays) {
                        if (resdays && resdays.data && resdays.data.d) {
                            currentMile.leftdays = resdays.data.d;
                        }
                        }).catch(function (err) {
                           
                        console.error("Update failed", err);
                        return;
                    });
                }

                // next milestone will start from current milestone end date
                //baseDate = new Date(end);
                baseDate = new Date(end);
                baseDate.setDate(baseDate.getDate() + 1);
                // tasks
                debugger
                if (currentMile.tasks && currentMile.tasks.length > 0) {
                    let taskBaseDate = normalize(currentMile.StartDate);
                    let mileStart = normalize(currentMile.StartDate);
                    let mileEnd = normalize(currentMile.EndDate);
                    for (var y = 0; y < currentMile.tasks.length; y++) {
                        let currentTask = currentMile.tasks[y];
                        let start_task = normalize(taskBaseDate);
                        if (start_task && mileStart && start_task < mileStart) {
                            alert("Task start date cannot be before Milestone start date");

                            if (updatefrom === "duration") {
                                currentTask.txtdays = currentTask._oldtxtdays || null;
                            } else {
                                currentTask.Startdate = currentTask._oldStartdate || null;
                            }
                            return;
                        }

                        else if (start_task && mileEnd && start_task > mileEnd) {
                            alert("Task start date cannot be after Milestone end date");

                            if (updatefrom === "duration") {
                                currentTask.txtdays = currentTask._oldtxtdays || null;
                            } else {
                                currentTask.Startdate = currentTask._oldStartdate || null;
                            }
                            return;
                        }
                        currentTask.Startdate = new Date(taskBaseDate);
// calculate task end date
                let response1 = await projectService.getnew_enddate(currentTask.Startdate, currentTask.txtdays);

                if (!response1 || !response1.data || !response1.data.d) {
                    alert("Failed to calculate end date");
                    return;
                }
                        let end1 = normalize(parseDotNetDate(response1.data.d));
                        currentTask.Targetdate = end1;
                        if (end1 && projectEnd && end1 > projectEnd) {
                            alert("Task end date cannot be after Milestone end date");

                            if (updatefrom === "duration") {
                                currentTask.txtdays = currentTask._oldtxtdays || null;
                            } else {
                                currentTask.Targetdate = currentTask._oldTargetdate || null;
                            }
                            return;
                        }
                        else if (start_task && end1 && end1 < start_task) {
                            alert("End date cannot be before Start date");

                            if (updatefrom === "duration") {
                                currentTask.txtdays = currentTask._oldtxtdays || null;
                            } else {
                                currentTask.Targetdate = currentTask._oldTargetdate || null;
                            }
                            return;
                        }
                        else {
                            debugger
                            let resultnew;
                            resultnew = projectService.updatetimeline_task(currentTask.id, currentTask.txtdays, new Date(currentTask.Startdate) );
                            debugger

                            if (!resultnew) return;

                            resultnew.then(function (res) {
                                let newDate = parseDotNetDate(res.data.d);
                              
                                return projectService.getleftdays(currentTask.id, 'task');
                            }).then(function (resdays) {
                                if (resdays && resdays.data && resdays.data.d) {
                                    currentTask.leftdays = resdays.data.d;
                                }
                            }).catch(function (err) {
                                
                                console.error("Update failed", err);
                                return;
                            });
                        }
                       
                        //taskBaseDate = new Date(end1);
                        taskBaseDate = new Date(end1);
                        taskBaseDate.setDate(taskBaseDate.getDate() + 1);
                        // subtasks
                        debugger
                        if (currentTask.subtasks && currentTask.subtasks.length > 0) {
                            let subtaskBaseDate = normalize(currentTask.Startdate);                            
                            let subtaskStart = normalize(currentTask.Startdate);
                            let subtaskEnd = normalize(currentTask.Targetdate);
                            let start_subtask = normalize(subtaskBaseDate);
                            for (var z = 0; z < currentTask.subtasks.length; z++) {
                                debugger
                                let currentSubTask = currentTask.subtasks[z];
                                currentSubTask.Startdate = new Date(subtaskBaseDate);
                                // validate sub task start date
                                if (start_subtask && subtaskStart && start_subtask < subtaskStart) {
                                    alert("Sub task start date cannot be before Task start date");

                                    if (updatefrom === "duration") {
                                        currentSubTask.txtdays = currentSubTask._oldtxtdays || null;
                                    } else {
                                        currentSubTask.Startdate = currentSubTask._oldStartdate || null;
                                    }
                                    return;
                                }
                                else if (start_subtask && subtaskEnd && start_subtask > subtaskEnd) {
                                    alert("Sub task start date cannot be after Task end date");

                                    if (updatefrom === "duration") {
                                        currentSubTask.txtdays = currentSubTask._oldtxtdays || null;
                                    } else {
                                        currentSubTask.Startdate = currentSubTask._oldStartdate || null;
                                    }
                                    return;
                                }
// calculate subtask end date
                                let response2 = await projectService.getnew_enddate(currentSubTask.Startdate, currentSubTask.txtdays);

                if (!response2 || !response2.data || !response2.data.d) {
                    alert("Failed to calculate end date");
                    return;
                                }
                                let end2 = normalize(parseDotNetDate(response2.data.d));
                                currentSubTask.Targetdate = end2;
                                if (end2 && subtaskEnd && end2 > subtaskEnd) {
                                    alert("Subt task end date cannot be after Task end date");

                                    if (updatefrom === "duration") {
                                        currentSubTask.txtdays = currentSubTask._oldtxtdays || null;
                                    } else {
                                        currentSubTask.Targetdate = currentSubTask._oldTargetdate || null;
                                    }
                                    return;
                                }
                                else if (start_subtask && end2 && end2 < start_subtask) {
                                    alert("End date cannot be before Start date");

                                    if (updatefrom === "duration") {
                                        currentSubTask.txtdays = currentSubTask._oldtxtdays || null;
                                    } else {
                                        currentSubTask.Targetdate = currentSubTask._oldTargetdate || null;
                                    }
                                    return;
                                } else {
                                    debugger
                                    let resultnew;
                                    resultnew = projectService.updatetimeline_subtask(currentSubTask.id, currentSubTask.txtdays, new Date(currentSubTask.Startdate));
                                    debugger

                                    if (!resultnew) return;

                                    resultnew.then(function (res) {
                                        let newDate = parseDotNetDate(res.data.d);
                                        
                                        // get left days
                                        return projectService.getleftdays(currentSubTask.id, 'subtask');
                                    }).then(function (resdays) {
                                        if (resdays && resdays.data && resdays.data.d) {
                                            currentSubTask.leftdays = resdays.data.d;
                                        }
                                    }).catch(function (err) {
                                        //alert(err);
                                        console.error("Update failed", err);
                                        return;
                                    });
                                }
                                            
                                
                                //subtaskBaseDate = new Date(end2);
                                subtaskBaseDate = new Date(end2);
                                subtaskBaseDate.setDate(subtaskBaseDate.getDate() + 1);
                            }
                        }
                    }
                }
            }
        }

        await reset_miles(projectlist, index,'');
    };
    
    $scope.zoomLevel = 100;
    $scope.applyZoom = function () {
        var scale = $scope.zoomLevel / 100;
        var table = document.querySelector(".project-table");
        if (table) {
            table.style.transform = "scale(" + scale + ")";
            table.style.transformOrigin = "top left";
            table.style.width = (100 / scale) + "%";  // 🔥 keeps scroll correct
        }
    };

    
 $scope.activePopover = null;



$scope.togglePopover = function ($event, item, itemP, level, index) {
    debugger;

    var el = $event.currentTarget;
if ($scope.activePopover) {
            $scope.activePopover.dispose();
            $scope.activePopover = null;
        }

    var old = bootstrap.Popover.getInstance(el);
    if (old) {
        old.dispose();
        return;
    }

    $scope.popContext = {
        item: item,
        itemP: itemP,
        level: level,
        index: index
    };
function closePopover() {
            if ($scope.activePopover) {
                $scope.activePopover.dispose();
                $scope.activePopover = null;
            }
setTimeout(function () {
        document.querySelectorAll('[data-bs-toggle="tooltip"]').forEach(function (el) {
            new bootstrap.Tooltip(el);
        });
    }, 100);
        }
    window.addRowUpDown_mile_wrap = function (dir) {
      //  alert($scope.popContext.item);
        $scope.$applyAsync(function () {
            $scope.addRowUpDown_mile(
                $scope.popContext.index,
                $scope.popContext.item,
                dir
            );

        });
closePopover();
    };

    window.addRowUpDown_task_wrap = function (dir) {
        $scope.$applyAsync(function () {
            $scope.addRowUpDown_task(
                $scope.popContext.index,
                $scope.popContext.itemP,
                $scope.popContext.item,
                dir
            );
        });
closePopover();
    };

    window.addRowUpDown_taskdefault_wrap = function () {
        $scope.$applyAsync(function () {
            $scope.addRowUpDown_taskdefault($scope.popContext.item);
        });
closePopover();
    };

    window.addRowUpDown_subtaskdefault_wrap = function () {
        $scope.$applyAsync(function () {
            $scope.addRowUpDown_subtaskdefault($scope.popContext.item);
        });
closePopover();
    };

    window.addRowUpDown_subtask_wrap = function (dir) {
        $scope.$applyAsync(function () {
            $scope.addRowUpDown_subtask(
                $scope.popContext.index,
                $scope.popContext.itemP,
                $scope.popContext.item,                
                dir,
                
            );
        });
closePopover();
    };

    var content = "";
debugger
    if (level === "milestone") {
var TaskButton = '';

if (item.istask == 1 || item.istask == '1') {
    TaskButton = `
        <li style="display:inline-block;">
                    <a href="javascript:void(0);"
                       onclick="addRowUpDown_taskdefault_wrap()"
                       class="dropdown-item p-1">
                       <i class="ri-list-unordered label-icon align-middle me-1"  title="Add Task"></i>
                    </a>
                </li>
    `;
}
        content = `
            <ul class="dropdown-menu dropdownmenu-secondary show" style="position:static;display:block;height:35px;padding:2px;">
                <li style="display:inline-block;">
                    <a href="javascript:void(0);"
                       onclick="addRowUpDown_mile_wrap('up')"
                       class="dropdown-item p-1"> <i class='bi bi-arrow-up-square'  title='Add Above'></i></a>
                </li>

                <li style="display:inline-block; margin:0 15px;">
                    <a href="javascript:void(0);"
                       onclick="addRowUpDown_mile_wrap('down')"
                       class="dropdown-item p-1"><i class='bi bi-arrow-down-square'  title='Add Below'></i></a>
                </li>
            ${TaskButton}
                
            </ul>
        `;
    }
    else if (level === "task") {
var subTaskButton = '';

if (item.issubtask == 1 || item.issubtask == '1') {
    subTaskButton = `
        <li style="display:inline-block;" ng-if="item.issubtask==1">
                    <a href="javascript:void(0);"
                       onclick="addRowUpDown_subtaskdefault_wrap()"
                       class="dropdown-item p-1">
                       <i class="ri-list-unordered label-icon align-middle me-1"  title="Add Sub Task"></i>
                    </a>
                </li>
    `;
}
        content = `
            <ul class="dropdown-menu dropdownmenu-secondary show" style="position:static;display:block;;height:35px;padding:2px;">
                <li style="display:inline-block;">
                    <a href="javascript:void(0);"
                       onclick="addRowUpDown_task_wrap('up')"
                       class="dropdown-item p-1"><i class='bi bi-arrow-up-square'  title='Add Above'></i></a>
                </li>

                <li style="display:inline-block; margin:0 15px;">
                    <a href="javascript:void(0);"
                       onclick="addRowUpDown_task_wrap('down')"
                       class="dropdown-item p-1"><i class='bi bi-arrow-down-square' title='Add Below'></i></a>
                </li>
${subTaskButton}
                
            </ul>
        `;
    }
    else if (level === "subtask") {
debugger

        content = `
            <ul class="dropdown-menu dropdownmenu-secondary show" style="position:static;display:block;;height:35px;padding:2px;">
                <li style="display:inline-block;">
                    <a href="javascript:void(0);"
                       onclick="addRowUpDown_subtask_wrap('up')"
                       class="dropdown-item p-1"><i class='bi bi-arrow-up-square'  title='Add Above'></i></a>
                </li>

                <li style="display:inline-block; margin:0 15px;">
                    <a href="javascript:void(0);"
                       onclick="addRowUpDown_subtask_wrap('down')"
                       class="dropdown-item p-1"><span  title="Add Below"><i class='bi bi-arrow-down-square'></i></span></a>
                </li>
            </ul>
        `;
    }

   

    var pop = new bootstrap.Popover(el, {
        html: true,
        trigger: 'manual',
        placement: 'bottom',
        sanitize: false,
        content: content
    });
    
    pop.show();
$scope.activePopover = pop;
setTimeout(function () {
        document.querySelectorAll('[data-bs-toggle="tooltip"]').forEach(function (el) {
            new bootstrap.Tooltip(el);
        });
    }, 100);
 
};

$scope.togglePopover_action = function ($event, item, itemP, level, index,ProjectName) {
    var el = $event.currentTarget;
if ($scope.activePopover) {
            $scope.activePopover.dispose();
            $scope.activePopover = null;
        }

    var old = bootstrap.Popover.getInstance(el);
    if (old) {
        old.dispose();
        return;
    }

    $scope.popContext = {
        item: item,
        itemP: itemP,
        level: level,
        index: index
    };
function closePopover_action() {
            if ($scope.activePopover) {
                $scope.activePopover.dispose();
                $scope.activePopover = null;
            }
setTimeout(function () {
        document.querySelectorAll('[data-bs-toggle="tooltip"]').forEach(function (el) {
            new bootstrap.Tooltip(el);
        });
    }, 100);
        }

    // wrapper function
    window.loadSrcFiles_wrap = function (dir) {        
        $scope.$applyAsync(function () {
            $scope.loadSrcFiles(
                $scope.popContext.item,
                dir,
                $scope.popContext.index
            );
        });
closePopover_action();
    };
window.loadform_wrap = function (dir,formtype,ifname,urls,urldetails) {
//alert(formtype);
       debugger
        if(urls!="")
        {
            if(formtype=="dependency")
            {
             if(dir=="milestone")
                urls+="?id="+$scope.popContext.item.id+"&mprojectid="+$scope.popContext.item.ProjectId+"&type=milestone&pid="+$scope.popContext.item.ProjectId;
             if(dir=="task")
               urls+="?id="+$scope.popContext.item.id+"&mid="+$scope.popContext.item.MilestonId+"&mprojectid="+$scope.popContext.item.ProjectId+"&type=task&pid="+$scope.popContext.item.ProjectId;
             if(dir=="subtask")
                urls+="?id="+$scope.popContext.item.id+"&tid="+$scope.popContext.item.task+"&mid="+$scope.popContext.item.MilestonId+"&pid="+$scope.popContext.item.ProjectId+"&mprojectid="+$scope.popContext.item.ProjectId+"&type=subtask";
            }
            else if(formtype=="issue")
            {
                if(dir=="milestone")
                    urls+="?id="+$scope.popContext.item.ProjectId+"&milstoneid="+$scope.popContext.item.id+"&mprojectid="+$scope.popContext.item.ProjectId+"&type=milstone&pid="+$scope.popContext.item.ProjectId;
                 if(dir=="task")
                urls+="?id="+$scope.popContext.item.ProjectId+"&milstoneid="+$scope.popContext.item.MilestonId+"$taskid="+$scope.popContext.item.id+"&mprojectid="+$scope.popContext.item.ProjectId+"&type=task";
             if(dir=="subtask")
                urls+="?id="+$scope.popContext.item.ProjectId+"&taskid="+$scope.popContext.item.task+"&milstoneid="+$scope.popContext.item.MilestonId+"&subtaskid="+$scope.popContext.item.id+"&tasklistid=0&mprojectid="+$scope.popContext.item.ProjectId+"&type=subtask";
   
            }        
        }
 //alert(urls); 
        $scope.$applyAsync(function () {
            $scope.loadform(
                $scope.popContext.item,
                dir,
                formtype,
             ifname,
             urls,
             urldetails
                
            );
        });
closePopover_action();
    };

    var fileCount = (item.viewfiles && item.viewfiles.length) ? item.viewfiles.length : 0;
    var dependancyCount =(item.totaldependency) ? item.totaldependency : 0;
    var issueCount = (item.pendingissue) ? item.pendingissue : 0;

    var content = "";

    
        content = `
            <div class='action-dropdown1' style='padding:6px 6px 6px 0px;'>

                <a href="javascript:void(0);" onclick="loadSrcFiles_wrap('${level}')" class="position-relative" data-bs-toggle="modal" data-bs-target="#addfiles"> <span class="btn-plus2"  title="Upload Documents" style="font-size:12px;height:22px;width:22px;margin-left:9px"><i class="bi bi-file-earmark-arrow-up"></i></span><span class="files_count">${fileCount}</span></a>
                <a href='javascript:void(0);'  class='position-relative' type='button' data-bs-toggle='offcanvas' data-bs-target='#canvas_popup_dependency' onclick="loadform_wrap('${level}','dependency','inner_popupD','/pms/AddSTDependency.aspx',' Dependency of ${ProjectName}')"><span class='btn-plus2'  title='+ Dependency' style='font-size:12px;height:22px;width:22px;margin-left:9px'>D</span><span class='dependency_count' >${dependancyCount}</span></a>
                <a href='javascript:void(0);' class='position-relative' type='button' data-bs-toggle='offcanvas' data-bs-target='#canvas_popup_issue' onclick="loadform_wrap('${level}','issue','inner_popup','/pms/AddIssuesPopup.aspx',' Issues of ${ProjectName}')"><span class='btn-plus2'  title='+ Issues' style='font-size:12px;height:22px;width:22px;margin-left:9px'>I</span><span class='issue_count' >${issueCount}</span></a>

            </div>
        `;
    

    var pop = new bootstrap.Popover(el, {
        html: true,
        trigger: 'manual',
        placement: 'bottom',
        sanitize: false,
        content: content
    });
   $scope.activePopover = pop;
    //$scope.activePopover = pop;
   //$scope.activePopover = null;
    pop.show();
 

    setTimeout(function () {
        document.querySelectorAll('[data-bs-toggle="tooltip"]').forEach(function (el) {
            new bootstrap.Tooltip(el);
        });
    }, 100);
};
  

$document.on('click', function (e) {
debugger
    // if no popover → do nothing
    if (!$scope.activePopover) return;
    var popEl = document.querySelector('.popover');
    // clicked inside popover → ignore
    if (popEl && popEl.contains(e.target)) return;
    // clicked on toggle button → ignore

    if (e.target.closest('.btn-plus2')) return;
    if (e.target.closest('.bi-three-dots-vertical')) return;
    // otherwise → close
    $scope.$applyAsync(function () {
        $scope.activePopover.dispose();
        $scope.activePopover = null;
    });
});


$scope.dropMilestone = function (event) {
    moveItemInArray($scope.projectlist.miles, event.previousIndex, event.currentIndex);
};

$scope.dropTask = function (event, milestone) {
    moveItemInArray(milestone.tasks, event.previousIndex, event.currentIndex);
};

$scope.dropSubTask = function (event, task) {
    moveItemInArray(task.subtasks, event.previousIndex, event.currentIndex);
};

function moveItemInArray(arr, from, to) {
    const item = arr.splice(from, 1)[0];
    arr.splice(to, 0, item);
}


/* ==========================================================
   V10 Angular row drag/drop - Chrome safe
   Start is called by ng-mousedown, so it does not depend on
   angular.element(row).scope() (which can fail when debugInfo is off).
========================================================== */
$scope.startPmsRowDrag = function ($event, rowType, dragItem, dragList, parentItem) {
    var e = $event && ($event.originalEvent || $event);
    if (!e || (e.button !== undefined && e.button !== 0)) return;
    if (!dragItem || !dragList || !dragList.length) return;
    if (e.preventDefault) e.preventDefault();
    if (e.stopPropagation) e.stopPropagation();

    function closest(el, selector) {
        while (el && el !== document) {
            if (el.matches && el.matches(selector)) return el;
            el = el.parentNode;
        }
        return null;
    }
    var startRow = closest(e.target, 'tr[data-pms-row]');
    if (!startRow) return;
    var startIndex = dragList.indexOf(dragItem);
    if (startIndex < 0) return;

    var requiredParent = rowType === 'milestone'
        ? String(($scope.projectlist && $scope.projectlist.id) || (parentItem && parentItem.id) || 'project')
        : String((parentItem && parentItem.id) || '');

    var ghost = startRow.cloneNode(true);
    ghost.className = 'pms-drag-ghost-v10';
    ghost.style.width = startRow.offsetWidth + 'px';
    document.body.appendChild(ghost);

    if (!document.getElementById('pms-row-drag-v10-style')) {
        var style = document.createElement('style');
        style.id = 'pms-row-drag-v10-style';
        style.innerHTML =
            '.pms-row-drag-handle{display:inline-flex;align-items:center;justify-content:center;width:20px;height:20px;margin-right:4px;cursor:grab;color:#64748b;vertical-align:middle;user-select:none;-webkit-user-drag:none;}' +
            '.pms-row-drag-handle *{pointer-events:none;}' +
            'body.pms-row-dragging-v10{user-select:none;cursor:grabbing;}' +
            '.pms-row-drop.drag-row-moving{opacity:.55;}' +
            '.pms-row-drop.drag-row-over td{box-shadow:inset 0 3px 0 #198754;background:rgba(25,135,84,.08)!important;}' +
            '.pms-row-drop.drag-row-invalid td{box-shadow:inset 0 3px 0 #dc3545;background:rgba(220,53,69,.08)!important;}' +
            '.pms-drag-ghost-v10{position:fixed;z-index:2147483647;opacity:.88;background:#fff;border:1px solid #b6c2d2;box-shadow:0 12px 30px rgba(15,23,42,.22);border-radius:6px;overflow:hidden;pointer-events:none;}' +
            '.pms-drag-ghost-v10 td{padding:6px 8px;white-space:nowrap;}';
        document.head.appendChild(style);
    }

    function clearMark() {
        var rows = document.querySelectorAll('.pms-row-drop.drag-row-over,.pms-row-drop.drag-row-invalid,.pms-row-drop.drag-row-moving');
        angular.forEach(rows, function (r) {
            r.classList.remove('drag-row-over');
            r.classList.remove('drag-row-invalid');
            r.classList.remove('drag-row-moving');
        });
    }
    function setGhost(x, y) {
        ghost.style.left = (x + 12) + 'px';
        ghost.style.top = (y + 12) + 'px';
    }
    function validTarget(row) {
        if (!row || row === startRow) return false;
        if (row.getAttribute('data-pms-row') !== rowType) return false;
        if (rowType !== 'milestone' && String(row.getAttribute('data-pms-parent-id') || '') !== requiredParent) return false;
        return true;
    }
    function getTargetIndex(row) {
        var idx = parseInt(row.getAttribute('data-pms-index'), 10);
        if (!isNaN(idx) && idx >= 0 && idx < dragList.length) return idx;
        var id = String(row.getAttribute('data-pms-id') || '');
        for (var i = 0; i < dragList.length; i++) {
            if (String(dragList[i].id || '') === id) return i;
        }
        return -1;
    }

    var lastTargetRow = null;
    function onMove(ev) {
        if (ev.preventDefault) ev.preventDefault();
        setGhost(ev.clientX, ev.clientY);
        var el = document.elementFromPoint(ev.clientX, ev.clientY);
        var row = closest(el, 'tr[data-pms-row]');
        clearMark();
        startRow.classList.add('drag-row-moving');
        lastTargetRow = null;
        if (!row || row === startRow) return;
        if (validTarget(row)) { row.classList.add('drag-row-over'); lastTargetRow = row; }
        else { row.classList.add('drag-row-invalid'); }
    }
    function onUp(ev) {
        if (ev && ev.preventDefault) ev.preventDefault();
        document.removeEventListener('mousemove', onMove, true);
        document.removeEventListener('mouseup', onUp, true);
        document.body.classList.remove('pms-row-dragging-v10');
        var targetRow = lastTargetRow;
        if (ghost && ghost.parentNode) ghost.parentNode.removeChild(ghost);
        clearMark();
        if (!targetRow || !validTarget(targetRow)) return;
        var fromIndex = dragList.indexOf(dragItem);
        var toIndex = getTargetIndex(targetRow);
        if (fromIndex < 0 || toIndex < 0 || fromIndex === toIndex) return;
        $scope.$applyAsync(function () {
            var moved = dragList.splice(fromIndex, 1)[0];
            if (fromIndex < toIndex) toIndex = toIndex - 1;
            dragList.splice(toIndex, 0, moved);
            if (rowType === 'milestone' && $scope.refreshOrderNo) $scope.refreshOrderNo();
            if (rowType === 'task' && $scope.refreshOrderNo_tasks) $scope.refreshOrderNo_tasks(parentItem);
            if (rowType === 'subtask' && $scope.refreshOrderNo_subtasks) $scope.refreshOrderNo_subtasks(parentItem);
        });
    }
    document.body.classList.add('pms-row-dragging-v10');
    startRow.classList.add('drag-row-moving');
    setGhost(e.clientX, e.clientY);
    document.addEventListener('mousemove', onMove, true);
    document.addEventListener('mouseup', onUp, true);
};



//#region manage gantt resise
    $scope.dragInfo = null;

    $scope.startDragBar = function ($event, row) {

        if ($($event.target).hasClass('resize-handle')) {
            return;
        }

        $event.preventDefault();

        $scope.dragInfo = {
            mode: 'move',
            row: row,
            startX: $event.pageX,
            startDate: new Date(row.start),
            endDate: new Date(row.end)
        };

        angular.element(document).on('mousemove', onBarMouseMove);
        angular.element(document).on('mouseup', onBarMouseUp);
    };

    $scope.startResizeBar = function ($event, row, side) {
debugger
        $event.stopPropagation();
        $event.preventDefault();

        $scope.dragInfo = {
            mode: 'resize',
            side: side,
            row: row,
            startX: $event.pageX,
            startDate: new Date(row.start),
            endDate: new Date(row.end)
        };

        angular.element(document).on('mousemove', onBarMouseMove);
        angular.element(document).on('mouseup', onBarMouseUp);
    };

    function onBarMouseMove(e) {
    if ($scope.ganttScale == "week" ||
        $scope.ganttScale == "month") {
        return;
    }
        if (!$scope.dragInfo) return;

        var info = $scope.dragInfo;

        var dx = e.pageX - info.startX;

        var days = Math.round(dx / $scope.ganttCellWidth);

        $scope.$applyAsync(function () {
//alert(info.mode);
            if (info.mode === 'move') {

                info.row.start = $scope.addDays(info.startDate, days);
                info.row.end = $scope.addDays(info.endDate, days);

            }

            else if (info.mode === 'resize') {

                if (info.side === 'left') {

                    var newStart = $scope.addDays(info.startDate, days);

                    if (newStart < info.row.end) {
                        info.row.start = newStart;
                    }
                }

                if (info.side === 'right') {

                    var newEnd = $scope.addDays(info.endDate, days);

                    if (newEnd > info.row.start) {
                        info.row.end = newEnd;

                    }
                }
            }
        });
    }
function getDaysBetween(start, end) {

    if (!start || !end) return 0;

    // REMOVE TIME
    var s = new Date(start.getFullYear(), start.getMonth(), start.getDate());
    var e = new Date(end.getFullYear(), end.getMonth(), end.getDate());

    // DIFFERENCE
    var diff = e - s;

    // DAYS
    return Math.floor(diff / (1000 * 60 * 60 * 24));
}
    function onBarMouseMove2(e) {

        if (!$scope.dragInfo) return;

        var info = $scope.dragInfo;

        var dx = e.pageX - info.startX;

        var units = Math.round(dx / $scope.ganttCellWidth);

        function moveByScale(date, value) {

            var d = new Date(date);

            // DAY SCALE
            if ($scope.ganttScale === "day") {

                d.setDate(d.getDate() + value);
            }

            // WEEK SCALE
            else if ($scope.ganttScale === "week") {

                d.setDate(d.getDate() + (value * 7));
            }

            // MONTH SCALE
            else if ($scope.ganttScale === "month") {

                d.setMonth(d.getMonth() + value);
            }

            return d;
        }

        function isBeyondMax(date) {

            return date > $scope.ganttEnd;
        }

        function isBeforeMin(date) {

            return date < $scope.ganttStart;
        }

        $scope.$applyAsync(function () {

            // ================= MOVE =================
            if (info.mode === 'move') {

                var newStart = moveByScale(info.startDate, units);

                var newEnd = moveByScale(info.endDate, units);

                // STOP AT MAX
                if (isBeyondMax(newEnd)) {
                    return;
                }

                // STOP AT MIN
                if (isBeforeMin(newStart)) {
                    return;
                }

                info.row.start = newStart;

                info.row.end = newEnd;
            }

            // ================= RESIZE =================
            else if (info.mode === 'resize') {

                // LEFT RESIZE
                if (info.side === 'left') {

                    var leftDate = moveByScale(info.startDate, units);

                    if (leftDate < info.row.end &&
                        !isBeforeMin(leftDate)) {

                        info.row.start = leftDate;
                    }
                }

                // RIGHT RESIZE
                else if (info.side === 'right') {

                    var rightDate = moveByScale(info.endDate, units);

                    if (rightDate > info.row.start &&
                        !isBeyondMax(rightDate)) {

                        info.row.end = rightDate;
                    }
                }
            }
        });
    }
    function onBarMouseMove3(e) {

        if (!$scope.dragInfo) return;

        var info = $scope.dragInfo;

        var dx = e.pageX - info.startX;

        var units = Math.round(dx / $scope.ganttCellWidth);

        // ================= DATE MOVE BY SCALE =================
        function moveByScale(date, value) {

            var d = new Date(date);

            // DAY
            if ($scope.ganttScale === "day") {

                d.setDate(d.getDate() + value);
            }

            // WEEK
            else if ($scope.ganttScale === "week") {

                d.setDate(d.getDate() + (value * 7));
            }

            // MONTH
            else if ($scope.ganttScale === "month") {

                d.setMonth(d.getMonth() + value);
            }

            return d;
        }

        // ================= MIN GAP =================
        function getMinGap() {

            if ($scope.ganttScale === "day") {
                return 1;
            }

            if ($scope.ganttScale === "week") {
                return 7;
            }

            return 30;
        }

        // ================= APPLY =================
        $scope.$applyAsync(function () {

            // =====================================================
            // MOVE BAR
            // =====================================================
            if (info.mode === 'move') {

                var newStart = moveByScale(info.startDate, units);

                var newEnd = moveByScale(info.endDate, units);

                // STOP LEFT
                if (newStart < $scope.ganttStart) {

                    var diff1 = Math.ceil(
                        ($scope.ganttStart - newStart) / (1000 * 60 * 60 * 24)
                    );

                    newStart = $scope.addDays(newStart, diff1);

                    newEnd = $scope.addDays(newEnd, diff1);
                }

                // STOP RIGHT
                if (newEnd > $scope.ganttEnd) {

                    var diff2 = Math.ceil(
                        (newEnd - $scope.ganttEnd) / (1000 * 60 * 60 * 24)
                    );

                    newStart = $scope.addDays(newStart, -diff2);

                    newEnd = $scope.addDays(newEnd, -diff2);
                }

                info.row.start = newStart;

                info.row.end = newEnd;
            }

            // =====================================================
            // RESIZE BAR
            // =====================================================
            else if (info.mode === 'resize') {

                // ================= LEFT RESIZE =================
                if (info.side === 'left') {

                    var leftDate = moveByScale(info.startDate, units);

                    // MIN LIMIT
                    if (leftDate < $scope.ganttStart) {

                        leftDate = new Date($scope.ganttStart);
                    }

                    // SHOULD NOT CROSS END
                    if (leftDate >= info.row.end) {

                        leftDate = new Date(info.row.end);

                        if ($scope.ganttScale === "day") {

                            leftDate.setDate(leftDate.getDate() - 1);
                        }
                        else if ($scope.ganttScale === "week") {

                            leftDate.setDate(leftDate.getDate() - 7);
                        }
                        else if ($scope.ganttScale === "month") {

                            leftDate.setMonth(leftDate.getMonth() - 1);
                        }
                    }

                    info.row.start = leftDate;
                }

                // ================= RIGHT RESIZE =================
                else if (info.side === 'right') {
                    var rightDate = moveByScale(info.endDate, units);
                    // MAX LIMIT
                    if (rightDate > $scope.ganttEnd) {

                        rightDate = new Date($scope.ganttEnd);
                    }
                    // SHOULD NOT CROSS START
                    if (rightDate <= info.row.start) {
                        rightDate = new Date(info.row.start);
                        if ($scope.ganttScale === "day") {
                            rightDate.setDate(rightDate.getDate() + 1);
                        }
                        else if ($scope.ganttScale === "week") {

                            rightDate.setDate(rightDate.getDate() + 7);
                        }
                        else if ($scope.ganttScale === "month") {

                            rightDate.setMonth(rightDate.getMonth() + 1);
                        }
                    }

                    info.row.end = rightDate;
                }
            }
        });
    }

    function onBarMouseUp() {
debugger

if ($scope.ganttScale == "week" ||
        $scope.ganttScale == "month") {
        return;
    }
        angular.element(document).off('mousemove', onBarMouseMove);
        angular.element(document).off('mouseup', onBarMouseUp);
        //alert("ok");
        if ($scope.dragInfo) {

            var row = $scope.dragInfo.row;
            var chkmode=$scope.dragInfo.mode;
            var nofdays=0;
            if(chkmode=="resize")
            {
             nofdays=getDaysBetween(row.start, row.end);
            }
            debugger
            
            if (row.type === "milestone") {
                row.raw.StartDate = new Date(row.start);
                row.raw.EndDate = new Date(row.end);                
                let mindex = $scope.projectlist.miles.findIndex(f => f.id===row.id);
                if (mindex !== -1) {
                let c1 = $scope.projectlist.miles[mindex];
                if(chkmode=="resize"){c1.txtdays=nofdays;}
                 $scope.resetStartDateHierarchy_mile(c1,$scope.projectlist,mindex);
                }
            }

            else if (row.type === "task") {

                row.raw.Startdate = new Date(row.start);
                row.raw.Targetdate = new Date(row.end);
                let mindex = $scope.projectlist.miles.findIndex(f => f.id === row.Milestoneid);
                let c = $scope.projectlist.miles[mindex];
                let tindex = $scope.projectlist.miles[mindex].tasks.findIndex(f => f.id === row.id);
                if (tindex !== -1) {
                    let c1 = $scope.projectlist.miles[mindex].tasks[tindex];
                    if(chkmode=="resize"){c1.txtdays=nofdays;}
                    $scope.resetStartDateHierarchy_task(c1,c,tindex);
                }
            }

            else if (row.type === "subtask") {

                row.raw.Startdate = new Date(row.start);
                row.raw.Targetdate = new Date(row.end);
                let mindex = $scope.projectlist.miles.findIndex(f => f.id === row.Milestoneid);
                let tindex = $scope.projectlist.miles[mindex].tasks.findIndex(f => f.id === row.Task);
                if (tindex !== -1) {
                    let c = $scope.projectlist.miles[mindex].tasks[tindex];
                    let sindex = $scope.projectlist.miles[mindex].tasks[tindex].subtasks.findIndex(f => f.id === row.id);
                    if (sindex !== -1) {
                        let c1 = $scope.projectlist.miles[mindex].tasks[tindex].subtasks[sindex];
                       if(chkmode=="resize"){c1.txtdays=nofdays;}
                        $scope.resetStartDateHierarchy_subtask(c1,c,tindex);
                    }
                }
            }
            console.log("UPDATED:", row.start, row.end);

            // save api here
            // updatedetails_gant(row);

            $scope.dragInfo = null;

            $scope.$applyAsync();
        }
    }
//#endregion
//// end scopes
});

function getWeekNumber(d) {

    d = new Date(Date.UTC(d.getFullYear(), d.getMonth(), d.getDate()));

    // Thursday decides the year
    d.setUTCDate(d.getUTCDate() + 4 - (d.getUTCDay() || 7));

    var yearStart = new Date(Date.UTC(d.getUTCFullYear(), 0, 1));

    var weekNo = Math.ceil((((d - yearStart) / 86400000) + 1) / 7);

    return weekNo;
}

setTimeout(function () {
    document.querySelectorAll("#projectTableContainer th, #projectTableContainer td")
        .forEach(function (cell) {

            if (!cell.getAttribute("data-base-width")) {
                cell.setAttribute("data-base-width", cell.offsetWidth);
            }

        });

}, 500);
function applyExcelZoom(zoomLevel) {
    debugger
    alert(zoomLevel);
    var scale = zoomLevel / 100;

    var table = document.querySelector(".project-table");

    if (table) {
        table.style.transform = "scale(" + scale + ")";
        table.style.transformOrigin = "top left";
        table.style.width = (100 / scale) + "%"; // 🔥 keeps scroll correct
    }
}



function initTableUI() {

    // ===== 1. DESTROY TOOLTIP FIRST =====
    document.querySelectorAll('[data-bs-toggle="tooltip"]').forEach(function (el) {

        let instance = bootstrap.Tooltip.getInstance(el);

        if (instance) {
            instance.dispose();
        }

    });

    // ===== 2. RECREATE TOOLTIP AFTER CLEAN =====
    document.querySelectorAll('[data-bs-toggle="tooltip"]').forEach(function (el) {
        new bootstrap.Tooltip(el);
    });


    // ===== 3. HANDLE DATATABLE SAFELY =====
    try {

        let tableEl = document.querySelector('.datatable');

        if (tableEl) {

            if ($.fn.DataTable.isDataTable(tableEl)) {
                $(tableEl).DataTable().clear().destroy();
            }

            setTimeout(function () {

                $(tableEl).DataTable({
                    scrollY: '300px',
                    scrollCollapse: true,
                    paging: false,
                    searching: true,
                    info: false
                });

            }, 50);
        }

    } catch (e) {

        console.log(e);
    }
    finally {
        
    }
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
    } catch (ex) {}
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
   // alert("ok");
    var table = getProjectTableElement();
    if (!table || !table.tHead || !table.tHead.rows.length) return;

    var headerRow = table.tHead.rows[0];
    var fixedColumns = 2; // Action + Milestone stay fixed

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
                } catch (ex) {}
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
    var FIXED_COLUMNS = 2; // Action + Milestone cannot hide/move
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
        } catch (ex) {}
        return 'pms_project_table_column_config_' + projectId;
    }

    function oldOrderKey() {
        var projectId = 'default';
        try {
            var hash = window.location.hash || '';
            var match = hash.match(/id=([^&]+)/i);
            if (match && match[1]) projectId = decodeURIComponent(match[1]);
        } catch (ex) {}
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
        } catch (ex) {}

        // Backward support from old v5/v6 order-only storage
        if (!cfg.order || !cfg.order.length) {
            try {
                var old = JSON.parse(localStorage.getItem(oldOrderKey()) || '[]');
                if (old && old.length) cfg.order = old;
            } catch (ex2) {}
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
                    try { e.dataTransfer.setData('text/plain', fromIndex.toString()); } catch (ex) {}
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


/* ==========================================================
   V9 - Chrome + Firefox Smooth Hierarchy Row Drag & Drop
   Milestone  -> move only inside project milestone list
   Task       -> move only inside the same milestone
   Sub Task   -> move only inside the same task
   Important: this version does NOT use HTML5 drag/drop. Chrome often
   shows a red no-drop icon inside tables with native drag/drop.
   This mouse/pointer based version is smoother and browser safe.
========================================================== */
(function () {
    var dragState = null;
    var lastTargetRow = null;

    function closestRow(el) {
        while (el && el !== document) {
            if (el.getAttribute && el.getAttribute('data-pms-row')) return el;
            el = el.parentNode;
        }
        return null;
    }

    function getRowScope(row) {
        try { return angular.element(row).scope(); } catch (e) { return null; }
    }

    function rowInfo(row) {
        var sc = getRowScope(row);
        if (!sc) return null;

        var type = row.getAttribute('data-pms-row');
        var item = null, parent = null, list = null, index = -1;

        if (type === 'milestone') {
            item = sc.c;
            parent = sc.projectlist || (sc.$parent && sc.$parent.projectlist);
            list = parent && parent.miles;
        } else if (type === 'task') {
            item = sc.c1;
            parent = sc.c;
            list = parent && parent.tasks;
        } else if (type === 'subtask') {
            item = sc.c2;
            parent = sc.c1;
            list = parent && parent.subtasks;
        }

        if (!item || !list) return null;
        index = list.indexOf(item);
        return { scope: sc, row: row, type: type, item: item, parent: parent, list: list, index: index };
    }

    function sameParent(a, b) {
        if (!a || !b || a.type !== b.type) return false;
        if (a.type === 'milestone') return true;
        if (!a.parent || !b.parent) return false;
        return String(a.parent.id || '') === String(b.parent.id || '');
    }

    function clearRowClasses() {
        var rows = document.querySelectorAll('.pms-row-drop.drag-row-over, .pms-row-drop.drag-row-invalid, .pms-row-drop.drag-row-moving');
        for (var i = 0; i < rows.length; i++) {
            rows[i].classList.remove('drag-row-over');
            rows[i].classList.remove('drag-row-invalid');
            rows[i].classList.remove('drag-row-moving');
        }
    }

    function saveOrder(info) {
        if (!info || !info.scope) return;
        if (info.type === 'milestone' && info.scope.refreshOrderNo) {
            info.scope.refreshOrderNo();
        } else if (info.type === 'task' && info.scope.refreshOrderNo_tasks) {
            info.scope.refreshOrderNo_tasks(info.parent);
        } else if (info.type === 'subtask' && info.scope.refreshOrderNo_subtasks) {
            info.scope.refreshOrderNo_subtasks(info.parent);
        }
    }

    function moveRow(from, to) {
        if (!from || !to || !sameParent(from, to)) return false;
        from.index = from.list.indexOf(from.item);
        to.index = to.list.indexOf(to.item);
        if (from.index < 0 || to.index < 0 || from.index === to.index) return false;
        var moved = from.list.splice(from.index, 1)[0];
        var newIndex = to.index;
        if (from.index < to.index) newIndex = to.index - 1;
        from.list.splice(newIndex, 0, moved);
        saveOrder(from);
        return true;
    }

    function makeGhost(row, startX, startY) {
        var ghost = row.cloneNode(true);
        ghost.className = 'pms-drag-ghost';
        ghost.style.width = row.offsetWidth + 'px';
        ghost.style.left = startX + 12 + 'px';
        ghost.style.top = startY + 12 + 'px';
        ghost.style.pointerEvents = 'none';
        document.body.appendChild(ghost);
        return ghost;
    }

    function getPoint(e) {
        if (e.touches && e.touches.length) return { x: e.touches[0].clientX, y: e.touches[0].clientY };
        if (e.changedTouches && e.changedTouches.length) return { x: e.changedTouches[0].clientX, y: e.changedTouches[0].clientY };
        return { x: e.clientX, y: e.clientY };
    }

    function updateTargetAt(x, y) {
        if (!dragState) return;
        var el = document.elementFromPoint(x, y);
        var targetRow = closestRow(el);
        clearRowClasses();
        dragState.row.classList.add('drag-row-moving');
        lastTargetRow = null;
        if (!targetRow || targetRow === dragState.row) return;
        var target = rowInfo(targetRow);
        if (!target) return;
        lastTargetRow = targetRow;
        if (sameParent(dragState.info, target)) targetRow.classList.add('drag-row-over');
        else targetRow.classList.add('drag-row-invalid');
    }

    function onMove(e) {
        if (!dragState) return;
        if (e.cancelable !== false) e.preventDefault();
        var p = getPoint(e);
        dragState.ghost.style.left = (p.x + 12) + 'px';
        dragState.ghost.style.top = (p.y + 12) + 'px';
        updateTargetAt(p.x, p.y);
    }

    function onUp(e) {
        if (!dragState) return;
        if (e.cancelable !== false) e.preventDefault();
        var targetRow = lastTargetRow;
        var target = targetRow ? rowInfo(targetRow) : null;
        var from = dragState.info;
        var sc = (target && target.scope) || from.scope;
        if (dragState.ghost && dragState.ghost.parentNode) dragState.ghost.parentNode.removeChild(dragState.ghost);
        document.body.classList.remove('pms-row-dragging-active');
        document.removeEventListener('mousemove', onMove, true);
        document.removeEventListener('mouseup', onUp, true);
        document.removeEventListener('touchmove', onMove, true);
        document.removeEventListener('touchend', onUp, true);
        document.removeEventListener('touchcancel', onUp, true);
        if (target && sameParent(from, target)) {
            sc.$applyAsync(function () {
                moveRow(from, target);
                dragState = null; lastTargetRow = null; clearRowClasses();
            });
        } else {
            dragState = null; lastTargetRow = null; clearRowClasses();
        }
    }

    function startDrag(e) {
        if (e.type === 'mousedown' && e.button !== 0) return;
        if (e.cancelable !== false) e.preventDefault();
        e.stopPropagation();
        var row = closestRow(this);
        var info = rowInfo(row);
        if (!row || !info || info.index < 0) return;
        var p = getPoint(e);
        dragState = { row: row, info: info, ghost: makeGhost(row, p.x, p.y) };
        document.body.classList.add('pms-row-dragging-active');
        row.classList.add('drag-row-moving');
        document.addEventListener('mousemove', onMove, true);
        document.addEventListener('mouseup', onUp, true);
        document.addEventListener('touchmove', onMove, { capture: true, passive: false });
        document.addEventListener('touchend', onUp, true);
        document.addEventListener('touchcancel', onUp, true);
    }

    function bindRowDragDrop() { return;// disabled in v10
        var table = document.querySelector('.project-table');
        if (!table) return;
        var handles = table.querySelectorAll('.pms-row-drag-handle');
        for (var i = 0; i < handles.length; i++) {
            var h = handles[i];
            h.setAttribute('draggable', 'false');
            if (h.dataset.rowDragBoundV9 === '1') continue;
            h.dataset.rowDragBoundV9 = '1';
            h.addEventListener('dragstart', function (e) { e.preventDefault(); return false; }, true);
            h.addEventListener('mousedown', startDrag, true);
            h.addEventListener('touchstart', startDrag, true);
        }
    }

    function addRowDragStyles() {
        if (document.getElementById('pms-row-drag-v9-style')) return;
        var css = document.createElement('style');
        css.id = 'pms-row-drag-v9-style';
        css.innerHTML =
            '.pms-row-drag-handle{display:inline-flex;align-items:center;justify-content:center;width:18px;height:18px;margin-right:4px;cursor:grab;color:#64748b;vertical-align:middle;touch-action:none;user-select:none;-webkit-user-drag:none;}' +
            '.pms-row-drag-handle *{pointer-events:none;}' +
            '.pms-row-drag-handle:active{cursor:grabbing;}' +
            'body.pms-row-dragging-active{user-select:none;cursor:grabbing;}' +
            '.pms-row-drop.drag-row-moving{opacity:.55;}' +
            '.pms-row-drop.drag-row-over td{box-shadow:inset 0 3px 0 #0d6efd;background:rgba(13,110,253,.05);}' +
            '.pms-row-drop.drag-row-invalid td{box-shadow:inset 0 3px 0 #dc3545;background:rgba(220,53,69,.06);}' +
            '.pms-drag-ghost{position:fixed;z-index:2147483647;opacity:.86;background:#fff;border:1px solid #b6c2d2;box-shadow:0 12px 30px rgba(15,23,42,.22);border-radius:6px;overflow:hidden;}' +
            '.pms-drag-ghost td{padding:6px 8px;white-space:nowrap;}';
        document.head.appendChild(css);
    }

    function initRowDragV9() { addRowDragStyles(); bindRowDragDrop(); }
    if (document.readyState === 'loading') document.addEventListener('DOMContentLoaded', initRowDragV9);
    else initRowDragV9();
    if (window.MutationObserver) {
        var mo = new MutationObserver(function () { setTimeout(initRowDragV9, 0); });
        mo.observe(document.documentElement, { childList: true, subtree: true });
    }
})();

//========= for gantt
(function () {

    var dragData = null;

    function closestRow(el) {
        while (el && el !== document) {
            if (
                el.classList &&
                el.classList.contains('gantt-left-row')
            ) {
                return el;
            }
            el = el.parentNode;
        }

        return null;
    }

    function getScope(el) {

        try {
            return angular.element(el).scope();
        }
        catch (e) {
            return null;
        }
    }

    function clearClasses() {

        document.querySelectorAll(
            '.gantt-row-over,.gantt-row-moving'
        ).forEach(function (r) {

            r.classList.remove(
                'gantt-row-over',
                'gantt-row-moving'
            );

        });
    }

    function moveItem(arr, from, to) {

        if (!arr) return;

        var item = arr.splice(from, 1)[0];

        arr.splice(to, 0, item);
    }

    function getRootScope(sc) {

        while (sc.$parent) {

            if (sc.projectlist)
                return sc;

            sc = sc.$parent;
        }

        return sc;
    }

    function findOriginalArray(root, row) {

        if (!root || !root.projectlist)
            return null;

        // milestone
        if (row.type === 'milestone') {

            return root.projectlist.miles;
        }

        // task
        if (row.type === 'task') {

            for (var i = 0; i < root.projectlist.miles.length; i++) {

                var m =
                    root.projectlist.miles[i];

                var idx =
                    m.tasks.findIndex(function (x) {

                        return x.id == row.id;

                    });

                if (idx > -1) {

                    return m.tasks;
                }
            }
        }

        // subtask
        if (row.type === 'subtask') {

            for (var i = 0; i < root.projectlist.miles.length; i++) {

                var m =
                    root.projectlist.miles[i];

                for (var j = 0; j < m.tasks.length; j++) {

                    var t =
                        m.tasks[j];

                    var idx =
                        t.subtasks.findIndex(function (x) {

                            return x.id == row.id;

                        });

                    if (idx > -1) {

                        return t.subtasks;
                    }
                }
            }
        }

        return null;
    }

    function onMove(e) {

        if (!dragData)
            return;

        e.preventDefault();

        var target =
            closestRow(e.target);

        clearClasses();

        dragData.row.classList.add(
            'gantt-row-moving'
        );

        if (!target ||
            target === dragData.row)
            return;

        var fromScope =
            getScope(dragData.row);

        var toScope =
            getScope(target);

        if (!fromScope || !toScope)
            return;

        var fromRow =
            fromScope.row;

        var toRow =
            toScope.row;

        if (!fromRow || !toRow)
            return;

        // same type only
        if (fromRow.type !== toRow.type)
            return;

        target.classList.add(
            'gantt-row-over'
        );

        dragData.target = target;
    }

    function onUp() {

        if (!dragData)
            return;

        document.removeEventListener(
            'mousemove',
            onMove,
            true
        );

        document.removeEventListener(
            'mouseup',
            onUp,
            true
        );

        if (!dragData.target) {

            clearClasses();
            dragData = null;
            return;
        }

        var fromScope =
            getScope(dragData.row);

        var toScope =
            getScope(dragData.target);

        if (!fromScope || !toScope) {

            clearClasses();
            dragData = null;
            return;
        }

        var fromRow =
            fromScope.row;

        var toRow =
            toScope.row;

        var root =
            getRootScope(fromScope);

        var arr =
            findOriginalArray(root, fromRow);

        if (!arr) {

            clearClasses();
            dragData = null;
            return;
        }

        var fromIndex =
            arr.findIndex(function (x) {

                return x.id == fromRow.id;

            });

        var toIndex =
            arr.findIndex(function (x) {

                return x.id == toRow.id;

            });

        if (fromIndex === -1 ||
            toIndex === -1) {

            clearClasses();
            dragData = null;
            return;
        }

        fromScope.$applyAsync(function () {

            moveItem(
                arr,
                fromIndex,
                toIndex
            );

            if (root.buildGantt)
                root.buildGantt();

            setTimeout(function () {

                if (window.syncGanttRowHeight)
                    syncGanttRowHeight();

            }, 100);

        });

        clearClasses();

        dragData = null;
    }

    function startDrag(e) {

        if (e.button !== 0)
            return;

        e.preventDefault();

        e.stopPropagation();

        var row =
            closestRow(this);

        if (!row)
            return;

        dragData = {

            row: row,
            target: null

        };

        document.addEventListener(
            'mousemove',
            onMove,
            true
        );

        document.addEventListener(
            'mouseup',
            onUp,
            true
        );
    }

    function bind() {
        return; // disabled: Gantt now uses Angular startSoftGanttRowDrag soft drag

        document.querySelectorAll(
            '.gantt-row-drag-handle'
        ).forEach(function (h) {

            if (h.dataset.dragbind == '1')
                return;
            h.dataset.dragbind = '1';

            h.addEventListener(
                'mousedown',
                startDrag,
                true
            );
        });
    }

    function init() {
        bind();
    }
    init();
    new MutationObserver(function () {
        setTimeout(init, 0);
    }).observe(document.body, {
        childList: true,
        subtree: true
    });

})();

