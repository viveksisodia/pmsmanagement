MANAGE TEAM TASKS

Copy this folder to:
/pms/myteamtask/

Open:
/pms/myteamtask/default.aspx

This is a new Manage Team Tasks view. It does not copy the Assign Project UI.
It uses ~/pms/angularmaster/angular_head.master and Inherits="products_default".
It uses its own myTeamTaskApp AngularJS module so a dependency failure in the
shared PMSerp module cannot stop this page from binding.
It reuses the existing, tested assignment WebMethods from:
/pms/assignprojects/default.aspx

Existing methods reused:
- bindProjectdata
- bindProjectdetail_pre
- getuserwithtasklist
- getunassigntasklist
- updateOwner_assign
- updateAssignTaskTime
- removeOwner_assign

Therefore the existing code remains authoritative for:
- Pms_tbl_assigntasks insert/update/archive
- employee shifts and night shifts
- GenerateTaskSegments multi-day display
- active-assignment filtering
- lowest-child hierarchy validation
- TaskDuration normalization
- server overlap validation
- yyyy-MM-dd date-only handling
- not updating the original hierarchy Owner during scheduler assignment

Files:
- default.aspx             New Manage Team Tasks view
- default.aspx.cs          Page code-behind only; no duplicate database methods
- app/myteam.main.js       View behavior, timeline and drag/drop
- app/myteam.service.js    Adapter for existing Assign Project WebMethods

No database change or stored procedure change is required by this package.

Employee visibility is loaded by GetMyTeamEmployees in default.aspx.cs. It calls
checkadmin(uid), then executes bindemployee_forTransfer with @regid, @isadmin and
@uid. Therefore an administrator receives all employees and a normal login
receives only the employees returned for that user by the existing procedure.
