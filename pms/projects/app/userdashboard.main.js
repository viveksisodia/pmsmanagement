
app.controller("userdashboardCtrl", function ($scope,$http,$timeout,$document,userdashboardService,$compile) {
   //alert("ok");
 console.log("controller loaded");
 //#region add variables
    $scope.isload = "0";
    $scope.totaltask_status = 0;
    $scope.totaltask_priority =0;
    $scope.freesearch = "0";
    $scope.mytasklist_history = [];
    $scope.currentlist=[];
    $scope.tagtype="project";
    $scope.currenttaskid="0";
    $scope.showlist = true;
    $scope.owners=[];
    $scope.mytasklist = [];
    $scope.mytasklist_status = [];
    $scope.currentPage = 1;
    $scope.pageSizes = [10, 20, 50, 100];
    $scope.types = ["1", "2"];
    $scope.pageSize = 1000;
    $scope.statuslists = [];
    $scope.prioritylists = [];
    //#endregion
    //#region add shorting
    $scope.sortColumn = "id";
    $scope.reverseSort = false;
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
    $scope.selectall = function (checked) {
        //alert('tt');
        $scope.isselectAll = checked;

        angular.forEach($scope.mytasklist, function (c) {
            c.selected = checked;
        });

        $scope.$applyAsync(); // VERY IMPORTANT
    };
   // $scope.selectall = function () {
       // alert('tt');
        
        //angular.forEach($scope.mytasklist, function (c) {
        //    debugger
           
        //    c.selected = $scope.isselectAll;

        //});

    //};
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
    //#endregion    $scope.bindtasklist2 = function (fromDate, toDate) {
        debugger
        //alert(fromDate + "/" + toDate);
        $scope.loadsubtasklist_count();
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
    $scope.removeTagToServer = function (c,tagid) {
        debugger
       
        let resultnew;

        
        resultnew = userdashboardService.removetags(tagid);
       
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
$scope.showhistory = function (taskid) {
debugger
 userdashboardService.gettaskhistory(taskid).then(function (response) {

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
$scope.loaduserlist = function () {

        debugger;
        //$scope.isLoading = true;

        userdashboardService.getuserlist()
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

    userdashboardService.getstatuslist()
        .then(function (response) {

            debugger;
            // assign data
            $scope.statuslists = response.data.d;
           window.statusLabels = [];

            angular.forEach($scope.statuslists, function (item) {
                window.statusLabels.push(item.statusname);
            });
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
$scope.selectStatus = function (c,item) {

        debugger;
        
        var pid = 0;
        let resultnew;
        
            pid = c.id;
            resultnew = userdashboardService.UpdateSubTask_Status(c.id, item.id, item.iscomplete);        

        if (resultnew) {
            resultnew.then(function (res) {
                debugger;
                if (res.data.d != "1") {
                    alert(res.data.d);
                }
                else {
                    c.status_color = item.colorname;
                    c.status_name = item.name;
                   
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

    userdashboardService.getprioritylist()
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

    debugger;

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

    userdashboardService.getsubtaskslist_count(fdate, tdate)
        .then(function (response) {

            $scope.totaltask_status = 0;
            $scope.mytasklist_status = response.data.d;

            angular.forEach($scope.mytasklist_status, function (s) {

                var val = parseInt(s.total || 0);

                $scope.totaltask_status += val;

            });
     setTimeout(function () {
debugger
    if (window.drawBubbles) {
        window.drawBubbles(
            $scope.mytasklist_status
        );
    }

    if (window.renderAllCharts) {
        window.renderAllCharts(
            $scope.mytasklist_status
        );
    }

}, 100);

        })
        .catch(function (error) {

            alert("HTTP ERROR: " + (error.statusText || error.message || "Unknown error"));
            console.error("HTTP ERROR:", error);

        })
        .finally(function () {

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
       
   
        userdashboardService.getsubtaskslist(fdate, tdate).then(function (response) {

        debugger;

        $scope.mytasklist = response.data.d;
        $scope.isload = "1";
if ($scope.mytasklist && $scope.mytasklist.length > 0) {

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
    //$scope.autostartTimers();
    
   
    $scope.isLoading = false;
                    }
        
            
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

   
$scope.loadstatuslist();
$scope.loadprioritylist();
$scope.loaduserlist();
$scope.loadsubtasklist_count();
$scope.loadsubtasklist();

//// end scopes
});

           
window.statusLabels = [];
function drawBubbles(statusData) {
debugger

    
}

        (function () {
            let animationProgress = 0;
            const animationDuration = 750;
            let startTime = null;

            const priorityValues = [30, 14, 8];
            const priorityColors = ['#ff999a', '#a5c7ff', '#ffe696'];

            const issuesValues = [14, 38];
            const issuesColors = ['#ff999a', '#a5c7ff'];

            const assignedHoursArray = [7.2, 6.0, 5.2, 7.5, 7.1, 7.3, 7.2, 0.4];
            const spentHoursArray = [5.8, 5.4, 4.8, 7.5, 6.9, 7.1, 7.5, 0.0];

            const hoverStates = {
                statusChart: { index: -1 },
                priorityChart: { index: -1 },
                taskEfficiencyGauge: { index: -1 },
                hoursChart: { index: -1, type: null },
                issuesChart: { index: -1 },
                timeEfficiencyGauge: { index: -1 }
            };

            function easeOutCubic(x) { return 1 - Math.pow(1 - x, 3); }

            function getDprContext(canvasId) {
                const canvas = document.getElementById(canvasId);
                if (!canvas) return null;
                const ctx = canvas.getContext('2d');
                const dpr = window.devicePixelRatio || 1;
                return { canvas, ctx, dpr };
            }

            function syncCanvasSize(canvas, ctx, dpr) {
                const rect = canvas.getBoundingClientRect();
                canvas.width = rect.width * dpr;
                canvas.height = rect.height * dpr;
                ctx.scale(dpr, dpr);
                return rect;
            }

            function showGlobalTooltip(text, clientX, clientY) {
                const tooltip = document.getElementById('global-chart-tooltip');
                if (!tooltip) return;
                tooltip.innerHTML = text;
                tooltip.style.opacity = '1';
                tooltip.style.transform = `translate3d(${clientX + 10}px, ${clientY - 25}px, 0)`;
            }

            function hideGlobalTooltip() {
                const tooltip = document.getElementById('global-chart-tooltip');
                if (tooltip) tooltip.style.opacity = '0';
            }

            const bubbleData = [
                { value: '28', xRatio: 0.58, yRatio: 0.32, maxR: 52, color: '#ffe7d8', textColor: '#f96e18', fontSize: 40 },
                { value: '14', xRatio: 0.83, yRatio: 0.50, maxR: 40, color: '#d5ffd7', textColor: '#00cc0e', fontSize: 30 },
                { value: '3', xRatio: 0.65, yRatio: 0.80, maxR: 32, color: '#fff4d1', textColor: '#f3bb00', fontSize: 26 },
                { value: '2', xRatio: 0.46, yRatio: 0.72, maxR: 25, color: '#ebdbff', textColor: '#8a38f5', fontSize: 18 }
            ];
 function drawBubbles(statusData) {

        //alert("ok1");

        const layout = getDprContext('statusChart');
    if (!layout) return;

    const rect = syncCanvasSize(
        layout.canvas,
        layout.ctx,
        layout.dpr
    );

    const ctx = layout.ctx;

    // predefined positions
    const positions = [
        { x: 0.58, y: 0.32 },
        { x: 0.83, y: 0.50 },
        { x: 0.65, y: 0.80 },
        { x: 0.46, y: 0.72 },
        { x: 0.25, y: 0.50 },
        { x: 0.75, y: 0.20 }
    ];

    angular.forEach(statusData, function (item, idx) {
        const total = parseInt(item.total || 0);

        const pos =
            positions[idx] ||
            {
                x: Math.random() * 0.7 + 0.15,
                y: Math.random() * 0.6 + 0.20
            };

        const cx = rect.width * pos.x;
        const cy = rect.height * pos.y;

        // dynamic bubble size
        const maxR = Math.max(
            25,
            Math.min(
                65,
                20 + (total * 1.5)
            )
        );

        const baseRadius =
            maxR * animationProgress;

        const isHovered =
            hoverStates.statusChart.index === idx;

        const r =
            isHovered
                ? baseRadius + 4
                : baseRadius;

        // bubble
        ctx.beginPath();
        ctx.arc(
            cx,
            cy,
            r,
            0,
            Math.PI * 2
        );

        ctx.fillStyle =
            item.statuscolor || '#cccccc';

        ctx.fill();

        // count text
        if (
            animationProgress > 0.4 &&
            r > 5
        ) {

            ctx.save();

            ctx.fillStyle = '#000';

            const fontSize =
                Math.max(
                    12,
                    Math.floor(maxR / 2)
                );

            ctx.font =
                `600 ${fontSize}px Arial`;

            ctx.textAlign = 'center';
            ctx.textBaseline = 'middle';

            ctx.fillText(
                total,
                cx,
                cy
            );

            ctx.restore();
        }
    });
    }

    window.drawBubbles = drawBubbles;

            function drawBubbles_old() {
                const layout = getDprContext('statusChart');
                if (!layout) return;
                const rect = syncCanvasSize(layout.canvas, layout.ctx, layout.dpr);
                const ctx = layout.ctx;

                bubbleData.forEach((b, idx) => {
                    const cx = rect.width * b.xRatio;
                    const cy = rect.height * b.yRatio;
                    const baseRadius = b.maxR * animationProgress;
                    const isHovered = hoverStates.statusChart.index === idx;
                    const r = isHovered ? baseRadius + 3 : baseRadius;


                    ctx.beginPath();
                    ctx.arc(cx, cy, r, 0, Math.PI * 2);
                    ctx.fillStyle = b.color;
                    ctx.fill();

                    if (isHovered) {
                        ctx.restore();
                    }

                    // Custom individual typography mapping execution step
                    if (animationProgress > 0.4 && r > 5) {
                        ctx.save();

                        // Applies individual target parameters directly out of the dataset matrix
                        ctx.fillStyle = b.textColor;

                        // Scales the independent base font size matching the animation track multiplier
                        const scaledFontSize = Math.max(8, Math.floor(b.fontSize * animationProgress));
                        ctx.font = `500 ${scaledFontSize}px -apple-system, BlinkMacSystemFont, "Segoe UI", sans-serif`;

                        ctx.textAlign = 'center';
                        ctx.textBaseline = 'middle';
                        ctx.fillText(b.value, cx, cy);
                        ctx.restore();
                    }
                });
            }

            function drawDoughnutChart(canvasId, values, colors, centerTotal, centerLabel) {
                const layout = getDprContext(canvasId);
                if (!layout) return;
                const rect = syncCanvasSize(layout.canvas, layout.ctx, layout.dpr);
                const ctx = layout.ctx;

                const cx = rect.width / 2;
                const cy = rect.height / 2;
                const baseRadius = Math.min(rect.width, rect.height) * 0.48;
                const thickness = baseRadius * 0.20;

                let startAngle = -Math.PI / 2;
                const total = values.reduce((a, b) => a + b, 0);

                values.forEach((val, i) => {
                    const fullSlice = (val / total) * (Math.PI * 2);
                    const currentSlice = fullSlice * animationProgress;
                    const isHovered = hoverStates[canvasId].index === i;
                    const r = isHovered ? baseRadius + 3 : baseRadius;

                    ctx.beginPath();
                    ctx.arc(cx, cy, r, startAngle, startAngle + currentSlice);
                    ctx.arc(cx, cy, r - thickness, startAngle + currentSlice, startAngle, true);
                    ctx.closePath();
                    ctx.fillStyle = colors[i];
                    ctx.fill();

                    startAngle += fullSlice;
                });

                if (centerTotal !== undefined && animationProgress > 0.5) {
                    ctx.save();
                    ctx.fillStyle = '#000';
                    ctx.font = 'bold 26px -apple-system, sans-serif';
                    ctx.textAlign = 'center';
                    ctx.textBaseline = 'middle';
                    ctx.fillText(centerTotal, cx, cy - 5);

                    ctx.fillStyle = '#000';
                    ctx.font = '500 11px -apple-system, sans-serif';
                    ctx.fillText(centerLabel, cx, cy + 12);
                    ctx.restore();
                }
            }

            function drawGaugeChart(canvasId, value) {
                const layout = getDprContext(canvasId);
                if (!layout) return;
                const rect = syncCanvasSize(layout.canvas, layout.ctx, layout.dpr);
                const ctx = layout.ctx;

                const cx = rect.width / 2;
                const cy = rect.height - 10;
                const radius = Math.min(rect.width, rect.height) * 0.80;

                // Reduced from 0.18 to a sleek 0.08 ratio thickness
                const thickness = radius * 0.13;

                const segments = [25, 25, 25, 25];
                const segmentColors = ['#ff999a', '#ffe696', '#a5c7ff', '#c5f8c8'];

                let startAngle = Math.PI;
                segments.forEach((seg, i) => {
                    const sliceAngle = (seg / 100) * Math.PI * animationProgress;
                    const isHovered = hoverStates[canvasId].index === i;
                    const r = isHovered ? radius + 3 : radius;

                    ctx.beginPath();
                    ctx.arc(cx, cy, r, startAngle, startAngle + sliceAngle);
                    ctx.arc(cx, cy, r - thickness, startAngle + sliceAngle, startAngle, true);
                    ctx.closePath();
                    ctx.fillStyle = segmentColors[i];
                    ctx.fill();
                    startAngle += (seg / 100) * Math.PI;
                });

                const needleAngle = Math.PI + ((value * animationProgress) / 100) * Math.PI;
                ctx.save();
                ctx.translate(cx, cy);
                ctx.rotate(needleAngle);

                ctx.beginPath();
                ctx.moveTo(0, -1.2);
                ctx.lineTo(radius - 2, 0);
                ctx.lineTo(0, 1.2);
                ctx.fillStyle = '#000';
                ctx.fill();

                ctx.fillStyle = '#000';
                ctx.fillRect(-8, -2.5, 16, 5);
                ctx.restore();
            }

            const barLabels = ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun'];
            const yTicks = ['08:00:00', '06:00:00', '04:00:00', '02:00:00', '00:00:00'];

            function drawBars() {
                const layout = getDprContext('hoursChart');
                if (!layout) return;
                const rect = syncCanvasSize(layout.canvas, layout.ctx, layout.dpr);
                const ctx = layout.ctx;

                const padLeft = 55, padBottom = 15, padTop = 20;
                const chartWidth = rect.width - padLeft - 0;
                const chartHeight = rect.height - padBottom - padTop;

                ctx.lineWidth = 1;
                ctx.textAlign = 'right';
                ctx.textBaseline = 'middle';
                ctx.font = '500 10px sans-serif';

                for (let i = 0; i < 5; i++) {
                    const y = padTop + (chartHeight / 4) * i;
                    ctx.strokeStyle = '#f1f5f9';
                    ctx.beginPath();
                    ctx.moveTo(padLeft, y);
                    ctx.lineTo(rect.width - 10, y);
                    ctx.stroke();

                    ctx.fillStyle = '#000';
                    ctx.fillText(yTicks[i], padLeft - 8, y);
                }

                const colWidth = chartWidth / 7;
                barLabels.forEach((label, i) => {
                    const colX = padLeft + (i * colWidth) + (colWidth * 0.15);
                    const barW = colWidth * 0.22;

                    const hAssigned = ((assignedHoursArray[i] / 8) * chartHeight) * animationProgress;
                    const hSpent = ((spentHoursArray[i] / 8) * chartHeight) * animationProgress;

                    const isAssignedHovered = hoverStates.hoursChart.index === i && hoverStates.hoursChart.type === 'assigned';
                    const isSpentHovered = hoverStates.hoursChart.index === i && hoverStates.hoursChart.type === 'spent';

                    ctx.fillStyle = isAssignedHovered ? '#70a2f3' : '#8bb7ff';
                    ctx.fillRect(colX, padTop + chartHeight - hAssigned, barW, hAssigned);

                    ctx.fillStyle = isSpentHovered ? '#a6eaa9' : '#c5f8c8';
                    ctx.fillRect(colX + barW + 2, padTop + chartHeight - hSpent, barW, hSpent);

                    ctx.fillStyle = '#000';
                    ctx.font = '500 10px sans-serif';
                    ctx.textAlign = 'center';
                    ctx.fillText(label, colX + barW + 1, rect.height - 4);
                });
            }

            function renderAllCharts() {
//alert('ok');
                //drawBubbles();
                drawDoughnutChart('priorityChart', priorityValues, priorityColors, "52", "Total");
                drawGaugeChart('taskEfficiencyGauge', 60);
                drawBars();
                drawDoughnutChart('issuesChart', issuesValues, issuesColors, "52", "Total Issues");
                drawGaugeChart('timeEfficiencyGauge', 75);
            }

            function hookInteractions(canvasId, hitTestFn) {
                const canvas = document.getElementById(canvasId);
                if (!canvas) return;

                canvas.addEventListener('mousemove', (e) => {
                    const rect = canvas.getBoundingClientRect();
                    hitTestFn(e.clientX - rect.left, e.clientY - rect.top, rect, e.clientX, e.clientY);
                });

                canvas.addEventListener('mouseleave', () => {
                    if (hoverStates[canvasId].type !== undefined) {
                        hoverStates[canvasId].index = -1; hoverStates[canvasId].type = null;
                    } else { hoverStates[canvasId].index = -1; }
                    hideGlobalTooltip();
                    renderAllCharts();
                });
            }

            window.addEventListener('load', () => {
                requestAnimationFrame(function step(t) {
                    if (!startTime) startTime = t;
                    const elapsed = t - startTime;
                    if (elapsed >= animationDuration) {
                        animationProgress = 1;
                        renderAllCharts();
                    } else {
                        animationProgress = easeOutCubic(elapsed / animationDuration);
                        renderAllCharts();
                        requestAnimationFrame(step);
                    }
                });

                hookInteractions('statusChart', (mx, my, rect, clX, clY) => {
                    let activeIdx = -1;
                    bubbleData.forEach((b, idx) => {
                        const cx = rect.width * b.xRatio;
                        const cy = rect.height * b.yRatio;
                        if (Math.sqrt((mx - cx) ** 2 + (my - cy) ** 2) <= b.maxR) activeIdx = idx;
                    });
                    if (activeIdx !== hoverStates.statusChart.index) {
                        hoverStates.statusChart.index = activeIdx;
                        renderAllCharts();
                    }
                    const statusLabels = ["Open", "Completed", "Work In Progress", "Reopen"];
                    if (activeIdx !== -1) showGlobalTooltip(`<strong>${statusLabels[activeIdx]}: ${bubbleData[activeIdx].value}</strong>`, clX, clY);
                    else hideGlobalTooltip();
                });

                function processDoughnutHover(canvasId, values, labels) {
                    hookInteractions(canvasId, (mx, my, rect, clX, clY) => {
                        const cx = rect.width / 2;
                        const cy = rect.height / 2;
                        const dist = Math.sqrt((mx - cx) ** 2 + (my - cy) ** 2);
                        const radius = Math.min(rect.width, rect.height) * 0.44;

                        let activeIdx = -1;
                        if (dist <= radius && dist >= radius * 0.5) {
                            let angle = Math.atan2(my - cy, mx - cx);
                            if (angle < 0) angle += Math.PI * 2;
                            let checkAngle = (angle + Math.PI / 2) % (Math.PI * 2);

                            let targetStart = 0;
                            const total = values.reduce((a, b) => a + b, 0);
                            values.forEach((val, i) => {
                                const slice = (val / total) * (Math.PI * 2);
                                if (checkAngle >= targetStart && checkAngle <= targetStart + slice) activeIdx = i;
                                targetStart += slice;
                            });
                        }
                        if (activeIdx !== hoverStates[canvasId].index) {
                            hoverStates[canvasId].index = activeIdx;
                            renderAllCharts();
                        }
                        if (activeIdx !== -1) showGlobalTooltip(`<strong>${labels[activeIdx]}: ${values[activeIdx]}</strong>`, clX, clY);
                        else hideGlobalTooltip();
                    });
                }

                processDoughnutHover('priorityChart', priorityValues, ["High", "Medium", "Low"]);
                processDoughnutHover('issuesChart', issuesValues, ["Issues", "Resolved"]);

                function processGaugeHover(canvasId, zoneLabels) {
                    hookInteractions(canvasId, (mx, my, rect, clX, clY) => {
                        const cx = rect.width / 2;
                        const cy = rect.height - 4;
                        const dx = mx - cx;
                        const dy = my - cy;
                        const dist = Math.sqrt(dx * dx + dy * dy);
                        const radius = Math.min(rect.width, rect.height) * 0.85;

                        let activeIdx = -1;
                        if (dist <= radius && dist >= radius * 0.5 && dy <= 0) {
                            let angle = Math.atan2(dy, dx);
                            if (angle < 0) angle += Math.PI * 2;

                            if (angle >= Math.PI && angle <= Math.PI * 2) {
                                const sectorAngle = (angle - Math.PI) / (Math.PI / 4);
                                activeIdx = Math.min(3, Math.floor(sectorAngle));
                            }
                        }

                        if (activeIdx !== hoverStates[canvasId].index) {
                            hoverStates[canvasId].index = activeIdx;
                            renderAllCharts();
                        }

                        if (activeIdx !== -1) showGlobalTooltip(`<strong>${zoneLabels[activeIdx]}</strong>`, clX, clY);
                        else hideGlobalTooltip();
                    });
                }

                processGaugeHover('taskEfficiencyGauge', ['Low', 'Average', 'Good', 'Excellent']);
                processGaugeHover('timeEfficiencyGauge', ['Low', 'Average', 'Good', 'Excellent']);

                hookInteractions('hoursChart', (mx, my, rect, clX, clY) => {
                    const padLeft = 55, padBottom = 15, padTop = 10;
                    const chartWidth = rect.width - padLeft - 10;
                    const chartHeight = rect.height - padBottom - padTop;
                    const colWidth = chartWidth / 7;

                    let activeIdx = -1, type = null;

                    if (mx >= padLeft && mx <= rect.width - 10 && my >= padTop && my <= rect.height - padBottom) {
                        const i = Math.floor((mx - padLeft) / colWidth);
                        if (i >= 0 && i < 7) {
                            const colX = padLeft + (i * colWidth) + (colWidth * 0.12);
                            const barW = colWidth * 0.28;
                            const hAssigned = (assignedHoursArray[i] / 8) * chartHeight;
                            const hSpent = (spentHoursArray[i] / 8) * chartHeight;

                            if (mx >= colX && mx <= colX + barW && my >= (padTop + chartHeight - hAssigned)) {
                                activeIdx = i; type = 'assigned';
                            }
                            else if (mx >= colX + barW + 2 && mx <= colX + (barW * 2) + 2 && my >= (padTop + chartHeight - hSpent)) {
                                activeIdx = i; type = 'spent';
                            }
                        }
                    }

                    if (activeIdx !== hoverStates.hoursChart.index || type !== hoverStates.hoursChart.type) {
                        hoverStates.hoursChart.index = activeIdx; hoverStates.hoursChart.type = type;
                        renderAllCharts();
                    }

                    if (activeIdx !== -1) showGlobalTooltip(barLabels[activeIdx] + " " + type + ": <strong>" + (type === 'assigned' ? assignedHoursArray[activeIdx] : spentHoursArray[activeIdx]) + " hrs</strong>", clX, clY);
                    else hideGlobalTooltip();
                });
            });
        })();
    