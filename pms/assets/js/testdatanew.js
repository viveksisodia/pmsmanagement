// Fetch Gantt data dynamically from server
function loadGanttData(pid) {
debugger
    $.ajax({
        type: "POST",
        //url: "/pms/makemilestone.aspx/GetGanttData", // adjust to your page/method
        url: "/pms/GanttData.aspx/GetGanttData", // adjust to your page/method
        data: JSON.stringify({ pid: pid }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
       async:true,
        success: function (response) {
debugger
            var ganttData = response.d;
          var ganttDataString = JSON.stringify(response.d);
            gantt.parse(ganttData); // Load into Gantt chart
           
        },
        error: function (err) {
debugger
            console.error("Failed to load data", err);
        }


    });
}




function generateData(count, dateFrom, dateTo){
	var tasks =  {
		data:[],
		links:[]
	};

	count = parseInt(count, 10) || 100;

	var date = new Date(dateFrom.getFullYear(),5,1);
	var project_id = 1;
	tasks.data.push({
		id:  project_id,
		text: "Project1",
		start_date: date,
		type: gantt.config.types.project,
		open:true
	});
	for (var i = 1; i < count; i++) {
		date = gantt.date.add(date, 1, "day");
		var task = {
			id: i + 1,
			start_date: date,
			text: "Task " + (i + 1),
			duration: 8,
			parent: project_id
		};

		if(gantt.date.add(date, 8, "day").valueOf() > dateTo.valueOf()){
			date = new Date(dateFrom);
			project_id = i + 1;
			delete task.parent;
			task.open = true;
		}
		tasks.data.push(task);

	}
	return tasks;
}