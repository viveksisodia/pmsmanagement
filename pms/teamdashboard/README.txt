PMS TEAM LEADER DASHBOARD — DATABASE-BOUND VERSION

INSTALL
1. Execute sql/01_pms_ManagerDashboardAllowedEmployees_team.sql in your PMS database.
2. Execute sql/02_pms_ManagerDashboardProjects_team.sql in the same database.
3. Copy the complete teamdashboard folder into your existing /pms folder.
4. Open /pms/teamdashboard/default.aspx after signing in.

Uses your existing ~/pms/angularmaster/angular_head.master, with its head
and ContentPlaceHolder1 placeholders. AngularJS is supplied by that master.
Uses your existing CommonClass.DataAccess and login cookies. No Web.config,
connection string, original dashboard file or existing procedure is changed.
Deploy the two new procedures before opening the page.

ACCESS
Database role profile ID 1 identifies admin, as in the uploaded dashboard.
Admin: all active employees within the current tenant.
Non-admin team leader: self plus active DIRECT reporting employees only
through tbl_employee.councelor_id. No recursive descendant expansion.
Non-admin without reporting employees: page redirects to your user dashboard.
Employee IDs are resolved again on the server, never supplied by the browser.
Employee focus shows the selected employee's own assigned work.

DATA AND COUNTING
Actual active milestone/task/subtask assignments come from Pms_tbl_assigntasks.
Assignment employee membership uses uid, matching the supplied old manager
dashboard. ownerid does not override uid when filtering permitted employees.
Each item appears once in the team counts and task grid even when assigned to
multiple team members. Employee workload is counted separately per assignee.
Planned time uses the maximum HH:MM taskduration per employee/item, matching
the existing user dashboard's approach and avoiding repeated daily segments.
Spent time sums PausedTime minutes for active pause/stop timer entries by
employee and item. Recorded historical timer time is used for completed work.
Issues are active issues attached to the selected assigned items, counted
once by issue ID. Project-level issues are outside this assigned-work view.

FILTERS
Default: Today, All projects, All employees.
Periods: Today, This Week, This Month, Custom (up to one year).
Project selection and employee focus persist when the period changes.
Open work begun by the selected period's end carries forward, including overdue
work. Completed work uses a recorded completion date in the period; where no
completion date exists, assignment date overlap is used to select the item.
All lower charts and the task grid use the same selected assignment records.
Timesheets remain the current Monday–Sunday week, labelled separately.
Project filtering restricts logged timer time to that project; available
employee capacity remains their shift capacity, not project-specific capacity.
KPI/status clicks filter and scroll to the task grid. Search and 50-row client
paging are included. Data is fetched in one dashboard call, not a call per row.

CAPACITY AND EFFICIENCY
Shift hours minus lunch; approved full/half-day leave; holidays; default
weekoff policy are used. Half-day tooltips show first/second half when recorded.
No assumed shift is inserted if the employee has no usable configured shift.
Current-week expected time is capacity to date; full-week capacity is in the
weekly total tooltip. Future days are not reported as missing timesheets.
Team load compares selected outstanding planned time with capacity over the
next 10 working dates, after leave. Load bars are visually capped at 150%.
Date presets use the user's browser calendar date. Stored database date values
are interpreted as calendar dates; this package does not migrate timestamps.
Task efficiency uses actual completion date compared with item due date.
Time efficiency averages each eligible completed item's planned/spent ratio.
Items without positive planned/spent time are excluded from that average.
Shared assignments' planned/spent hours are combined for the team item metric.

FIELDS THAT NEED SCHEMA CONFIRMATION
The query recognises CompleteDate, CompletedDate or CompletionDate on the
item tables. If none exists, on-time efficiency shows unavailable; the page
never assumes a completed item finished on time.
Timer milestoneid/taskid columns are detected. If absent, that type's spent
time is unavailable; subtaskid is supported by the existing user code.
The task name column recognises Taskname or Task; the existing milestone and
subtask names are Milestone and Subtaskname.
Historical reopen events were not supplied. Quality shows CURRENTLY reopened
items using their current database status, without a made-up history/trend.
Suggestions are independent advisory candidates. They never save assignments;
use the existing Assign Project flow to validate project/team membership and
apply a reassignment.

VALIDATION
JavaScript syntax and renderer fixture checks passed for shared-item and issue
counts, time efficiency, overdue work, escaped data, filter persistence,
empty results and request failure/recovery. ASPX element IDs and layout
structure were checked. Live SQL execution, C# compilation and rendering
inside your master require your running ASP.NET/SQL Server environment;
those were not available here. The file contains no sample employee/task data.

ISOLATED TEAM PROCEDURES
The supplied CREATE OR ALTER statements target only names ending in _team.
They create the team procedures on first install and alter only those team
procedures on updates. Do not rename or replace the old dashboard procedures.
If the previous team-named procedures were already installed, they may remain;
the updated page no longer calls them. No old dashboard objects are removed.

REPLACE THE EARLIER SAMPLE PACKAGE
Replace all ASPX, C# and both JavaScript files together, not just the scripts.
Open /pms/teamdashboard/default.aspx explicitly, then press Ctrl+F5.
The correct page title is PMS Team Dashboard. It has a Period filter and
a Loading team data message. It does not contain Platform team, Nadeem Pasha,
Sample data or All 10 developers. If those remain, an older page is being served.
The reference font families are restored with optional Google Fonts; system
fonts are used when the font service is unavailable.

HEADER UPDATE
The reference brand/navigation/user header is removed. The page uses the
existing master header and the dashboard default-page sub-header layout:
title left, project/employee filters middle, calendar period and refresh right.
Native filter selections retain the existing teamdashboard data behavior.
No SQL change is required for this header-only update.

FOLDER URL FIX
An earlier ZIP included an index.html preview. IIS may still select that file
for /pms/teamdashboard/, bypassing the ASPX master and data calls.
Overwrite index.html with the included redirect file, as well as deploying
the current default.aspx, C# and JavaScript files.
For an immediate check open /pms/teamdashboard/default.aspx explicitly.
The site navigation and logged-in user header come from angular_head.master,
exactly as in the supplied existing dashboard default.aspx.

PARAMETER FIX
@isadmin explicitly uses SqlDbType.Int with Value=0 in both C# calls.
Replace default.aspx.cs for this fix. No SQL change is required.

NON-ADMIN ASSIGNMENT FIX
Run sql/02_pms_ManagerDashboardProjects_team.sql again to alter only the team
procedure. This removes ownerid precedence from the assignment access filter.
No original dashboard procedure is changed.

LATEST TEAM DASHBOARD UPDATE
Project options include parent projects when self/direct reports are owner or
team member at project, milestone, task or subtask level. Assignment is not
required for project visibility. Workload/task hours still require actual
assignments; membership alone does not invent assigned work or hours.
All date presets now available, including quarter/year and previous periods.
All passes NULL dates; calendar capacity remains the current local week.
Run the diagnostic script with real login IDs if the reporting count differs.
The admin role remains database profile ID 1, matching the existing dashboard.
No live SQL Server or ASP.NET compilation was available during validation.
