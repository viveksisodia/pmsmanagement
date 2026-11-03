INSTALLATION
Copy the complete newuserdashboard folder into your existing /pms/ directory.
Open /pms/newuserdashboard/default.aspx while logged in.
The existing userdashboard files are not replaced.

FILES
  default.aspx
  default.aspx.cs
  app/userdashboard.main.js
  app/userdashboard.service.js

DATA
Uses your existing dbo.usp_Pms_UserDashboard_GetAssignedSubtasks procedure.
No stored procedure or table changes are required.
Uses the existing Angular master page, AngularJS, Bootstrap tooltip support,
CommonClass.DataAccess, and login cookies.
Counts assigned subtasks as tasks, following the original dashboard.
The top strip reads active configured database statuses, including zero counts.
Total and Overdue are separate cards; clicking any top card filters My tasks,
highlights the matching bottom filter, and automatically scrolls to the task grid.
Project and date filters refresh the dashboard; the hours selector can use the
main range, This Week, or Today. Planned hours remain employee capacity;
a project filter limits recorded spent hours to its selected assigned subtasks.
The table shows full recorded timer duration for each task, matching the
existing completed-task time-efficiency approach, not only time in the range.
Task efficiency and situation labels retain your existing database calculations.
The reference HTML uses different sample rules; no invented trend is shown.
Due dates and project names are resolved from supported installed schema columns.
Missing due dates show No due date and are excluded from overdue/attention counts.

VALIDATION
JavaScript syntax and controller checks passed for dynamic counts, priority
matrix, project list, overdue filtering, seconds, and hours scaling.
This environment does not have your ASP.NET application or database; server
compilation, installed column mappings, and live data need validation on your server.
