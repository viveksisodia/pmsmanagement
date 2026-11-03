PMS PROJECT GRID PERFORMANCE FIX - LATEST DEFAULT FILES

Replace only:
1. /pms/projects/default.aspx
2. /pms/projects/default.aspx.cs

What changed:
- Existing project UI/grid markup and controls are preserved.
- Only the project list request (bindProjectdata) is routed to default.aspx/bindProjectdata.
- All other existing myprojects.aspx WebMethods remain unchanged.
- bindProjectdata bulk-loads repeated lookup/summary/file/extension data instead of calling SQL helper methods for every project row.
- Existing cumulative team stored procedure pms_getproject_com_teamnew is preserved.
- Existing date-only response behavior is preserved.
- JS cache version now changes only when the physical JS file changes, instead of DateTime.Now.Ticks on every page request.

No database/stored-procedure change is required.
